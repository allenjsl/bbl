using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Eyousoft.Common.Page;
using Common.Enum;

namespace Web.TeamPlan
{
    /// <summary>
    /// 团队计划列表页
    /// 功能：显示团队计划
    /// 创建人：戴银柱
    /// 创建时间： 2011-01-13 
    /// </summary>
    public partial class TeamPlanList : BackPage
    {
        #region 分页变量
        protected int pageSize = 10;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            //权限判断
            PowerControl();
            if (!IsPostBack)
            {
                #region 传参变量
                //团队编号
                string teamNumber = Utils.GetQueryStringValue("teamNumber");
                //团队名称
                string routeName = Utils.GetQueryStringValue("teamName");
                //游客姓名
                string orderName = Utils.GetQueryStringValue("orderName");
                //天数
                int? dayCount = Utils.GetIntNull(Utils.GetQueryStringValue("dayCount"));
                //开始日期
                DateTime? beginDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("beginDate"));
                //结束日期
                DateTime? endDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("endDate"));
                //当前页
                pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
                //线路ID
                int? areaId = Utils.GetInt(Utils.GetQueryStringValue("xlid"));
                if (areaId == 0) { areaId = null; }
                //销售员编号
                int[] SellerID = ConvertToIntArray(Utils.GetQueryStringValue("SellerId").Split(','));
                //计调员编号
                int[] CoordinatorId = ConvertToIntArray(Utils.GetQueryStringValue("CoordinatorId").Split(','));
                #endregion

                #region 查询条件赋值
                if (teamNumber != "")
                {
                    this.txtTeamNumber.Text = teamNumber;
                }
                if (routeName != "")
                {
                    this.txtTeamName.Text = routeName;
                }
                if (orderName!=null)
                {
                    txt_Name.Text = orderName;
                }
                if (dayCount != null)
                {
                    this.txtDayCount.Text = dayCount.ToString();
                }
                if (beginDate != null)
                {
                    this.txtBeginDate.Text = Convert.ToDateTime(beginDate).ToString("yyyy-MM-dd");
                }
                if (endDate != null)
                {
                    this.txtEndDate.Text = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd");
                }
                this.selectOperator1.OperId = Utils.GetQueryStringValue("SellerId");
                this.selectOperator1.OperName = Utils.GetQueryStringValue("SellerName");
                this.selectOperator2.OperId = Utils.GetQueryStringValue("CoordinatorId");
                this.selectOperator2.OperName = Utils.GetQueryStringValue("CoordinatorName");
                #endregion
                //页面初始化
                DataInit(teamNumber, routeName, dayCount, beginDate, endDate, areaId, SellerID, CoordinatorId,orderName);
            }

        }

        #region 团队计划列表初始化
        /// <summary>
        ///  页面初始化方法
        /// </summary>
        /// <param name="teamNumber">团队编号</param>
        /// <param name="teamName">团队名称</param>
        /// <param name="dayCount">天数</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        protected void DataInit(string teamNumber, string routeName, int? dayCount, DateTime? beginDate, DateTime? endDate, int? areaId, int[] SellerId, int[] CoordinatorId,string orderName)
        {
            //声明查询对象
            EyouSoft.Model.TourStructure.TourSearchInfo searchModel = new EyouSoft.Model.TourStructure.TourSearchInfo();
            //团号查询
            searchModel.TourCode = teamNumber;
            //线路名称
            searchModel.RouteName = routeName;
            //游客姓名
            searchModel.YouKeName = orderName;
            //团队天数
            searchModel.TourDays = dayCount;
            //出团日期
            searchModel.SDate = beginDate;
            //截止日期
            searchModel.EDate = endDate;
            //线路区域编号
            searchModel.AreaId = areaId;
            //销售员编号
            searchModel.Sellers = SellerId;
            //计调员编号
            searchModel.Coordinators = CoordinatorId;
            searchModel.TourStatus = (EyouSoft.Model.EnumType.TourStructure.TourStatus?)Utils.GetEnumValue(typeof(EyouSoft.Model.EnumType.TourStructure.TourStatus), Utils.GetQueryStringValue("tourStatus"), null);
            //人数、团款合计
            int peopleSum = 0;
            decimal paraSum = 0;
            //声明bll对象
            EyouSoft.BLL.TourStructure.Tour bllOrder = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            //获得团队计划集合
            IList<EyouSoft.Model.TourStructure.LBTeamTourInfo> list = bllOrder.GetToursTeam(SiteUserInfo.CompanyID, pageSize, pageIndex, ref recordCount, searchModel);
            bllOrder.GetToursTeamHeJi(SiteUserInfo.CompanyID, searchModel, out peopleSum, out paraSum);

            //判断集合是否有数据
            if (list != null && list.Count > 0)
            {
                lt_peopleNum.Text = peopleSum.ToString();
                lt_paraSum.Text = "￥" + paraSum.ToString("0.00");
                //页面控件数据绑定
                this.rptList.DataSource = list;
                this.rptList.DataBind();
                //设置分页
                BindPage();
                //不显示无数据提示
                lblMsg.Visible = false;
            }
            else
            {
                lt_paraSum.Visible = false;
                lt_peopleNum.Visible = false;
                //没有数据隐藏分页控件
                this.ExportPageInfo1.Visible = false;
                lblMsg.Visible = true;
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

        #region 权限控制
        protected void PowerControl()
        {
            //判断该栏目权限
            if (CheckGrant(TravelPermission.团队计划_团队计划_栏目))
            {
                //新增权限
                if (!CheckGrant(TravelPermission.团队计划_团队计划_新增计划))
                {
                    this.pnlAdd.Visible = false;
                    this.pnlCopy.Visible = false;
                }
                //删除权限
                if (!CheckGrant(TravelPermission.团队计划_团队计划_删除计划))
                {
                    this.penDelete.Visible = false;
                }
                //修改权限
                if (!CheckGrant(TravelPermission.团队计划_团队计划_修改计划))
                {
                    this.pnlUpdate.Visible = false;
                }
            }
            else
            {
                Utils.ResponseNoPermit(TravelPermission.团队计划_团队计划_栏目, true);
            }
        }
        #endregion

        protected string GetPrintUrl(string type, string tourId)
        {
            string url = "";
            if (type == "Normal")
            {
                url = "<a target=\"_blank\" href=\"" + new EyouSoft.BLL.CompanyStructure.CompanySetting().GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.团队计划标准发布行程单) + "?tourId=" + tourId + "\">";
            }
            else
            {
                url = "<a target=\"_blank\" href=\"" + new EyouSoft.BLL.CompanyStructure.CompanySetting().GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.团队计划快速发布行程单) + "?tourId=" + tourId + "\">";
            }
            return url;
        }

        protected string GetStatusByTicket(string status, string ticketStatus)
        {
            switch (ticketStatus)
            {
                case "机票申请": return "机票申请";
                case "审核通过": return "未出票";
                default: return status;
            }
        }

        //将字符串数组转化成整型数组
        private int[] ConvertToIntArray(string[] source)
        {
            int[] to = new int[source.Length];
            for (int i = 0; i < source.Length; i++)//将全部的数字存到数组里。
            {
                if (!string.IsNullOrEmpty(source[i].ToString()))
                {
                    to[i] = Utils.GetInt(source[i].ToString());
                }
            }
            if (to[0] == 0)
            {
                return null;
            }
            return to;
        }


    }
}
