using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EyouSoft.Common;
using System.Text;

namespace Web.GroupEnd
{
    /// <summary>
    /// 页面：组团首页
    /// 功能：组团首页
    /// Author:dj
    /// Date:2011-01-24
    /// </summary>
    public partial class Default : Eyousoft.Common.Page.FrontPage
    {
        #region 定义变量
        protected int i = 1;
        protected int UserSiteID = 1; //当前登录用户编号

        //消息模块
        protected int remindnum = 0;//留位信息数量

        //公告信息模块
        protected IList<EyouSoft.Model.PersonalCenterStructure.NoticeNews> newslist = null;//公告信息

        //业务咨询模块变量
        protected int recordcount;
        protected int pagesize = 3;
        protected int pageindex = 1;//当前页码
        protected IList<EyouSoft.Model.CompanyStructure.ContactPersonInfo> list = null;//联系人列表


        //线路列表
        protected IList<IList<EyouSoft.Model.TourStructure.LBZTTours>> arealist = new List<IList<EyouSoft.Model.TourStructure.LBZTTours>>();
        //线路名称
        protected IList<string> areaname = new List<string>();
        protected IList<string> areaIdList = new List<string>();
        //BLL
        EyouSoft.BLL.CompanyStructure.News nBll = null;
        EyouSoft.BLL.TourStructure.Tour tBll = null;
        EyouSoft.BLL.CompanyStructure.Area aBll = null;
        /// <summary>
        /// 团队展示方式
        /// </summary>
        EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType TourDisplayType = EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType.明细团;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            string act = Utils.GetQueryStringValue("act");
            switch (act)
            {
                case "getcontent":
                    GetContent();
                    break;
            }
            if (!IsPostBack)
            {
                InitBindOrderlist();
                BindProClamationList();
                InitBindLine();
                InitBindAuothor();
                InitBindMsg();
            }
        }
        #region 绑定欠款信息
        /// <summary>
        /// 绑定欠款信息
        /// </summary>
        private void InitBindMsg()
        {

            EyouSoft.BLL.CompanyStructure.Customer custBll = new EyouSoft.BLL.CompanyStructure.Customer();//客户资料bll
            //已欠款金额
            decimal debtAmount = 0;
            //最高欠款金额
            decimal maxDebtAmount = 0;
            custBll.GetCustomerDebt(CurrentUserCompanyID, out debtAmount, out maxDebtAmount);
            if (debtAmount >= maxDebtAmount)
            {
                lblMsg.Text = "您已超过最高欠款额度，请结账后再继续预订，咨询电话：";
                EyouSoft.BLL.CompanyStructure.CompanyInfo companyBll = new EyouSoft.BLL.CompanyStructure.CompanyInfo();
                EyouSoft.Model.CompanyStructure.CompanyInfo infoModel = companyBll.GetModel(SiteUserInfo.CompanyID, SiteUserInfo.SysId);//公司信息实体

                if (infoModel != null)
                {
                    lblMsg.Text += infoModel.ContactTel.ToString();//电话

                }

            }
        }
        #endregion



        #region 绑定订单提醒
        protected void InitBindOrderlist()
        {
            EyouSoft.BLL.PersonalCenterStructure.TranRemind trBll = new EyouSoft.BLL.PersonalCenterStructure.TranRemind(SiteUserInfo);
            remindnum = trBll.GetTotalOrderCountByBuyCompanyId(CurrentUserCompanyID);


        }
        #endregion

        #region 绑定公告信息
        protected void BindProClamationList()
        {
            nBll = new EyouSoft.BLL.CompanyStructure.News();
            newslist = nBll.GetZuTuanAcceptNews(SiteUserInfo.CompanyID);
        }
        #endregion

        #region 绑定联系人信息(ajax请求)
        protected void InitBindAuothor()
        {
            pageindex = 1;
            EyouSoft.BLL.CompanyStructure.CompanyUser cuBll = new EyouSoft.BLL.CompanyStructure.CompanyUser(SiteUserInfo);
            list = cuBll.GetAreaJobsByTourUserID(pagesize, pageindex, ref recordcount, SiteUserInfo.ID);
        }
        protected void GetContent()
        {
            pageindex = Utils.GetInt(Utils.GetQueryStringValue("page"), 1);
            EyouSoft.BLL.CompanyStructure.CompanyUser cuBll = new EyouSoft.BLL.CompanyStructure.CompanyUser(SiteUserInfo);
            list = cuBll.GetAreaJobsByTourUserID(pagesize, pageindex, ref recordcount, SiteUserInfo.ID);
            GetJson();
        }
        /// <summary>
        /// 得到联系人JSON数据
        /// </summary>
        protected void GetJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{{\"page\":{0},\"maxpage\":{1}", pageindex, recordcount / pagesize + 1);
            if (list != null)
            {
                sb.Append(",\"content\":[");
                int temp = 0;
                foreach (EyouSoft.Model.CompanyStructure.ContactPersonInfo cp in list)
                {
                    if (temp != 0)
                        sb.Append(",");
                    sb.AppendFormat("{{\"name\":\"{0}\",\"phone\":\"{1}\",\"mobile\":\"{2}\",\"fax\":\"{3}\",\"qq\":\"{4}\"}}",
                        cp.ContactName, cp.ContactTel, cp.ContactMobile, cp.ContactFax, cp.QQ);
                    temp++;

                }
                sb.Append("]}");
            }
            else
            {
                sb.Append("}");
            }
            Response.Clear();
            Response.Write(sb.ToString());
            Response.End();
        }
        #endregion

        #region 绑定线路专线
        /// <summary>
        /// 绑定线路专线
        /// </summary>
        protected void InitBindLine()
        {
            TourDisplayType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetSiteTourDisplayType(SiteUserInfo.CompanyID);
            tBll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo, true);
            aBll = new EyouSoft.BLL.CompanyStructure.Area(SiteUserInfo);

            for (int i = 0; i < SiteUserInfo.Areas.Length; i++)
            {
                EyouSoft.Model.CompanyStructure.Area aModel = aBll.GetModel(SiteUserInfo.Areas[i]);
                if (aModel != null)
                {
                    IList<EyouSoft.Model.TourStructure.LBZTTours> li = tBll.GetToursZTDSY(SiteUserInfo.CompanyID, SiteUserInfo.Areas[i], 4, TourDisplayType);
                    if (li != null)
                    {
                        arealist.Add(li);
                        areaname.Add(aModel.AreaName);
                        areaIdList.Add(aModel.Id.ToString());
                    }
                }
            }
        }
        /// <summary>
        /// 根据团号得到报价
        /// </summary>
        protected string GetPrice(string tourid)
        {
            EyouSoft.BLL.TourStructure.Tour tour = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            decimal crPrice;
            decimal rtPrice;
            tour.GetTourMarketPrice(tourid, out crPrice, out rtPrice);
            return Utils.FilterEndOfTheZeroDecimal(crPrice) + "/" + Utils.FilterEndOfTheZeroDecimal(rtPrice);
        }

        /// <summary>
        /// 获取出团日期字符串
        /// </summary>
        /// <param name="tourInfo">计划信息</param>
        /// <returns></returns>
        protected string GetLDateString(EyouSoft.Model.TourStructure.LBZTTours tourInfo)
        {
            string s = string.Empty;

            if (TourDisplayType == EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType.明细团)
            {
                s = string.Format("<span>{0}</span>", tourInfo.LDate.ToString("yyyy-MM-dd"));
                //s = string.Format("<span><a href=\"javascript:void(0)\" data_selector=\"tour_date\" data_tourid=\"{0}\" data_leavedate=\"{1}\">{1}</a></span>", tourInfo.TourId, tourInfo.LDate.ToString("yyyy-MM-dd"));
            }
            else
            {
                s = string.Format("<span title=\"点击可查看全部发班计划\"><a href=\"javascript:void(0)\" data_selector=\"tour_date\" data_tourid=\"{0}\" data_leavedate=\"{1}\">{1}</a></span>", tourInfo.TourId, tourInfo.LDate.ToString("yyyy-MM-dd"));
            }

            return s;
        }
        #endregion
    }
}
