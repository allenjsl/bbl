using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using EyouSoft.Model.EnumType;

namespace Web.StatisticAnalysis.EmployeeAchievementsTime
{
    /// <summary>
    /// 页面功能：员工业绩表--人数、收入订单明细列表
    /// Author:liuym
    /// Date:2011-1-22
    /// </summary>
    public partial class GetEmpTourList : BackPage
    {
        /// <summary>
        /// OnPreInit事件 设置模式窗体属性
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }
        #region Private Members
        protected int PageIndex = 0;
        protected int PageSize = 10;
        protected int RecordCount = 0;
        protected string SalersId = string.Empty;//销售员
        protected string DepartId = string.Empty;//部门
        protected string AccounterId = string.Empty;//计调
        protected DateTime? LeaveTourStartDate = null;//离团开始日期
        protected DateTime? LeaveTourEndDate = null;// 离团结束日期
        protected DateTime? SignBillStarDate = null;//签单开始日期
        protected DateTime? SignBillEndDate = null;//签单结束日期
        protected string StaticType = "0";//统计类型：0 收入统计；1利润统计
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                InitEmpTourOrderList();
            }
        }

        #region 初始化
        private void InitEmpTourOrderList()
        {
            StaticType = Utils.GetQueryStringValue("Type");
            //获取当月开始时间 本月的第一天
            DateTime? CurrStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //当月结束时间 本月的最后一天
            DateTime? CurrEndTime = CurrStartTime.Value.AddMonths(1).AddDays(-1);
            IList<EyouSoft.Model.TourStructure.TourOrder> list = null;
            EyouSoft.BLL.TourStructure.TourOrder tourOrderBLL = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
            EyouSoft.Model.StatisticStructure.QueryPersonnelStatistic searchModel = new EyouSoft.Model.StatisticStructure.QueryPersonnelStatistic();
            //分页
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            SalersId = Utils.GetQueryStringValue("SalserId");
            DepartId = Utils.GetQueryStringValue("DepartId");
            AccounterId = Utils.GetQueryStringValue("AccounterId");
            if (StaticType == "0")
            {
                LeaveTourStartDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveTourStartDate")) == null ? CurrStartTime : Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveTourStartDate"));
                LeaveTourEndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveTourEndDate")) == null ? CurrEndTime : Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveTourEndDate"));
                SignBillStarDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("SignBillStartDate")) == null ? CurrStartTime : Utils.GetDateTimeNullable(Utils.GetQueryStringValue("SignBillStartDate")); ;
                SignBillEndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("SignBillEndDate")) == null ? CurrEndTime : Utils.GetDateTimeNullable(Utils.GetQueryStringValue("SignBillEndDate"));
            }
            else
            {
                LeaveTourStartDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveTourStartDate"));
                LeaveTourEndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveTourEndDate"));
                SignBillStarDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("SignBillStartDate"));
                SignBillEndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("SignBillEndDate"));
            }
            if(DepartId!="")
            {
                searchModel.DepartIds = Utils.GetIntArray(DepartId, ",");
            }
            if(SalersId!="")
            {
                searchModel.SaleIds = Utils.GetIntArray(SalersId, ",");
            }
            if (AccounterId != "")
            {
                searchModel.LogisticsIds = Utils.GetIntArray(AccounterId, ",");
            }
            searchModel.CompanyId = this.CurrentUserCompanyID;
            searchModel.LeaveDateStart = LeaveTourStartDate;
            searchModel.LeaveDateEnd = LeaveTourEndDate;
            searchModel.CheckDateStart = SignBillStarDate;
            searchModel.CheckDateEnd = SignBillEndDate;
            CompanyStructure.ComputeOrderType? computeOrder = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(SiteUserInfo.CompanyID);
            if (computeOrder.HasValue)
                searchModel.ComputeOrderType = computeOrder.Value;
            else
                searchModel.ComputeOrderType = CompanyStructure.ComputeOrderType.统计有效订单;
            list = tourOrderBLL.GetOrderList(PageSize, PageIndex, ref RecordCount, searchModel);
            if (list != null && list.Count != 0)
            {
                this.tbl_ExPageEmp.Visible = true;
                this.crp_GetEmpTourList.DataSource = list;
                this.crp_GetEmpTourList.DataBind();
                BindPage();
            }
            else
            {
                this.tbl_ExPageEmp.Visible = false;
                this.crp_GetEmpTourList.EmptyText = "<tr bgcolor='#e3f1fc'><td colspan='6' height='50px' align='center'>暂时没有数据！</td></tr>";
            }
            searchModel = null;
            list = null;
            tourOrderBLL = null;
        }
        #endregion

        #region 设置分页
        protected void BindPage()
        {
            this.ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExportPageInfo1.UrlParams = Request.QueryString;
            this.ExportPageInfo1.intPageSize = PageSize;
            this.ExportPageInfo1.CurrencyPage = PageIndex;
            this.ExportPageInfo1.intRecordCount = RecordCount;
        }
        #endregion
    }
}
