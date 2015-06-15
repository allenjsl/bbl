using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Common.Enum;

namespace Web.caiwuguanli
{
    /// <summary>
    /// 财务管理：团队核算
    /// 功能：显示团队核算列表
    /// 创建人：戴银柱
    /// 创建时间： 2011-01-19
    /// </summary>
    /// 修改人：柴逸宁
    /// 修改时间：2011-06-21
    /// 修改内容：金额栏添加金额的合计
    public partial class TeamAccount : Eyousoft.Common.Page.BackPage
    {
        #region 分页变量
        protected int pageSize = 10;
        protected int pageIndex = 1;
        protected int recordCount;

        protected string TourTypeSearchOptionHTML = string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {            
            //判断权限
            if (!CheckGrant(TravelPermission.财务管理_团队核算_栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.财务管理_团队核算_栏目, false);
                return;
            }

            TourTypeSearchOptionHTML = Utils.GetTourTypeSearchOptionHTML(CurrentUserCompanyID, Utils.GetQueryStringValue("tourtype"), true);

            if (!IsPostBack)
            {

                //获得参数
                string teamNum = Utils.GetQueryStringValue("teamNum");   //团号
                string id = Utils.GetQueryStringValue("xId");  //线路ID
                string name = Server.UrlDecode(Utils.GetQueryStringValue("name")); //线路名称
                DateTime? time = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("time")); //出团时间
                string isAccount = Utils.GetQueryStringValue("isAccount");  //是否未核算
                if (isAccount == "") { isAccount = "no"; }
                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1); //当前页

                //页面控件赋值
                if (teamNum != "")
                {
                    this.txtTeamNum.Text = teamNum;

                }
                if (id != "")
                {
                    this.xianluWindow1.Id = id;
                }
                if (name != "")
                {
                    this.xianluWindow1.Name = name;
                }
                if (time != null)
                {
                    this.txtTime.Text = Convert.ToDateTime(time).ToString("yyyy-MM-dd");
                }
                //设置控件类型
                this.xianluWindow1.publishType = 3;
                //初始化
                DataInit(teamNum, name, time, isAccount);
            }
        }


        #region 核算列表
        /// <summary>
        /// 列表初始化
        /// </summary>
        /// <param name="teamNum"></param>
        /// <param name="id"></param>
        /// <param name="time"></param>
        protected void DataInit(string teamNum, string name, DateTime? time, string isAccount)
        {
            EyouSoft.Model.TourStructure.TourSearchInfo searchModel = new EyouSoft.Model.TourStructure.TourSearchInfo();
            searchModel.SDate = time;
            //searchModel.FDate = time;
            searchModel.TourCode = teamNum;
            searchModel.RouteName = name;
            searchModel.EDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));
            searchModel.OperatorDepartIds = Utils.GetIntArray(Utils.GetQueryStringValue("departids"), ",");
            searchModel.OperatorIds = Utils.GetIntArray(Utils.GetQueryStringValue("operatorids"), ",");
            searchModel.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType?)Utils.GetEnumValue(typeof(EyouSoft.Model.EnumType.TourStructure.TourType), Utils.GetQueryStringValue("tourtype"), null);

            IList<EyouSoft.Model.TourStructure.LBAccountingTourInfo> list = null;
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            #region 总计参数
            //总收入
            decimal income = 0;
            //总支出
            decimal expenditure = 0;
            //总利润分配
            decimal payOff = 0;
            //总成人数
            int adultNumber = 0;
            //总儿童数
            int childNumber = 0;
            #endregion
            //获取统计
            if (isAccount == "no")
            {
                list = bll.GetToursNotAccounting(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, searchModel);
                //获取待核算总计结果
                bll.GetToursNotAccounting(SiteUserInfo.CompanyID, searchModel, ref income, ref expenditure, ref payOff, ref adultNumber, ref childNumber);

            }
            else
            {
                list = bll.GetToursAccounting(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, searchModel);
                //获取以核算总计结果
                bll.GetToursAccounting(SiteUserInfo.CompanyID, searchModel, ref income, ref expenditure, ref payOff, ref adultNumber, ref childNumber);

            }

            #region 总计赋值
            //总收入
            lblincome.Text = income.ToString("￥#0.##");
            //总支出
            lblexpenditure.Text = expenditure.ToString("￥#0.##");
            //总利润分配
            lblpayOff.Text = payOff.ToString("#0.##");
            //总人数
            lblNumber.Text = adultNumber.ToString()+" + " + childNumber.ToString();
            //总毛利
            lblml.Text = (income - expenditure).ToString("#0.##");
            //纯利
            lblcl.Text = (income - expenditure - payOff).ToString("#0.##");

            int renShu = adultNumber + childNumber;
            decimal maoLi = income - expenditure;

            if (renShu == 0)
            {
                ltrRenJunMaoLi.Text = "0";
            }
            else
            {
                ltrRenJunMaoLi.Text = (maoLi / renShu).ToString("C2");
            }

            if (income <= 0)
            {
                ltrMaoLiLv.Text = "0%";
            }
            else
            {
                ltrMaoLiLv.Text = ((maoLi / income) * 100).ToString("F2") + "%";
            }
            #endregion

            int pageCount = recordCount / pageSize;
            if (recordCount % pageSize > 0) pageCount++;
            if (pageIndex > pageCount) pageIndex = pageCount;
            if (pageIndex < 1) pageIndex = 1;

            if (list != null && list.Count > 0)
            {
                this.rptList.DataSource = list;
                this.rptList.DataBind();
                BindPage();
            }
            else
            {
                this.ExportPageInfo1.Visible = false;
                this.lblMsg.Text = "未找到相关数据!";
            }
        }
        #endregion

        #region 获得计划或服务的URL
        protected string GetUrlByType(string name, string planType, object singleServices)
        {
            switch (planType)
            {
                //单项服务
                case "单项服务": if (singleServices != null)
                    {
                        return singleServices.ToString();
                    }
                    else
                    {
                        return "";
                    };

                default:
                    return name;
            }
        }
        #endregion

        #region 设置分页
        protected void BindPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams = Request.QueryString;
            this.ExportPageInfo1.intPageSize = pageSize;
            this.ExportPageInfo1.CurrencyPage = pageIndex;
            this.ExportPageInfo1.intRecordCount = recordCount;
        }
        #endregion
    }
}
