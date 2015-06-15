using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;

namespace Web.StatisticAnalysis.Ageanalysis
{
    /// <summary>
    ///页面功能： 拖欠款总金额
    ///Author:liuym
    ///Date:2011-1-21
    /// </summary>
    public partial class GetArrearageList : BackPage
    {
        #region Private Attributes
        protected int PageSize = 20;
        protected int PageIndex = 1;
        protected int RecordCount = 0;
        protected DateTime? StartDate = null;//出团开始时间
        protected DateTime? EndDate = null;//出团结束时间
        protected string TourType = string.Empty;//团队类型
        protected string RouteAreaId = string.Empty;//线路ID
        protected string SalserId = string.Empty;//销售员ID
        IList<EyouSoft.Model.TourStructure.TourOrder> list = null;
        #endregion

        /// <summary>
        /// OnPreInit事件 设置模式窗体属性
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetTotalArrearage();
            }
        }

        #region 拖欠款总额列表 
        private void GetTotalArrearage()
        {
            PageIndex = Utils.GetInt(Request.QueryString["Page"],1);
            StartDate =Utils.GetDateTimeNullable(Utils.GetQueryStringValue("startDate"));
            EndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("endDate"));
            RouteAreaId = Utils.GetQueryStringValue("routeAreaName");
            TourType = Utils.GetQueryStringValue("tourType");
            SalserId = Utils.GetQueryStringValue("SalerId");
            EyouSoft.BLL.TourStructure.TourOrder TourOrderBLL = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
            EyouSoft.Model.TourStructure.SearchInfo SearchModel = new EyouSoft.Model.TourStructure.SearchInfo();
            SearchModel.LeaveDateFrom = StartDate;
            SearchModel.LeaveDateTo = EndDate;
            if (RouteAreaId != "" && RouteAreaId != "0")
            {
                SearchModel.AreaId = int.Parse(RouteAreaId);
            }
            SearchModel.SalerId = int.Parse(SalserId);
            if (TourType != "" && TourType != "-1")
            {
                SearchModel.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)int.Parse(TourType);
            }
            //TODO:有问题
            list = TourOrderBLL.GetOrderList(PageSize, PageIndex, ref RecordCount, SearchModel);
            if (list != null && list.Count != 0)
            {
                this.crp_GetArrenarageList.DataSource = list;
                this.crp_GetArrenarageList.DataBind();
                this.tbl_ExportPage.Visible = true;
                BindPage();
            }
            else
            {
                this.tbl_ExportPage.Visible = false;
                this.crp_GetArrenarageList.EmptyText = "<tr bgcolor='#e3f1fc'><td colspan='9' height='50px' align='center'>暂时没有数据！</td></tr>";
            }
        }
        #endregion

        /// <summary>
        /// 设置分页
        /// </summary>
        private void BindPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams = Request.QueryString;
            this.ExportPageInfo1.intPageSize = PageSize;
            this.ExportPageInfo1.CurrencyPage = PageIndex;
            this.ExportPageInfo1.intRecordCount = RecordCount;
        }
    }
}
