using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using System.IO;
using EyouSoft.Common;
using EyouSoft.Model.StatisticStructure;
using Common.Enum;
namespace Web.StatisticAnalysis.EmployeeAchievementsTime
{
    /// <summary>
    /// 页面功能：统计分析--按收入统计
    /// Author:liuym
    /// Date:2011-1-18
    /// </summary>
    public partial class EmpIncomeStatisticList : BackPage
    {
        #region Private Members
        protected int PageSize = 20;
        protected int PageIndex = 1;
        protected int RecordCount = 0;
        protected DateTime? LeaveTourStartDate = null;//出团开始日期
        protected DateTime? LeaveTourEndDate = null;//出团结束日期
        protected DateTime? SignBillStartDate = null;//签单开始日期
        protected DateTime? SignBillEndDate = null;//签单结束日期
        protected string Salser = string.Empty;//销售员
        protected string SalserId = string.Empty;//销售员ID
        protected string Accounter = string.Empty;//计调员
        protected string DepartName = string.Empty;//部门名称
        protected string DepartId = string.Empty;//部门ID
        protected string AccounterId = string.Empty;//计调员ID
        protected string RoyaltyRatio = string.Empty;//提成比例
        IList<EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic> EmpIncomList = null;
        #endregion

        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 权限验证
            if (!CheckGrant(TravelPermission.统计分析_员工业绩_员工业绩栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.统计分析_员工业绩_员工业绩栏目, true);
                return;
            }
            #endregion


            if (!IsPostBack)
            {
                //初始化数据
                InitEmpIncomeStaticlist();
            }

            #region 导出
            if (Request.QueryString["isExport"] != null && Utils.InputText(Request.QueryString["isExport"]) == "1")
            {
                if (EmpIncomList != null && EmpIncomList.Count != 0)
                {
                    ToExcel(this.crp_PrintEmpIncomeList, EmpIncomList);
                }
            }
            #endregion
            if (Request.QueryString["isAll"] != null && Utils.InputText(Request.QueryString["isAll"]) == "1")
            {
                PageSize = int.MaxValue;
            }
        }
        #endregion

        #region  初始化收入统计
        private void InitEmpIncomeStaticlist()
        {
            EyouSoft.BLL.StatisticStructure.PersonnelStatistic EmployeeAchiveBLL = new EyouSoft.BLL.StatisticStructure.PersonnelStatistic(this.SiteUserInfo);
            //获取当月开始时间 本月的第一天
            DateTime? CurrStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //当月结束时间 本月的最后一天
            DateTime? CurrEndTime = CurrStartTime.Value.AddMonths(1).AddDays(-1);
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            Accounter = Utils.GetQueryStringValue("Accounter");
            AccounterId = Utils.GetQueryStringValue("AccounterId");
            LeaveTourStartDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveTourStartDate")) == null ? CurrStartTime : Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveTourStartDate"));
            LeaveTourEndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveTourEndDate")) == null ? CurrEndTime : Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveTourEndDate"));
            SignBillStartDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("SignBillStartDate")) == null ? CurrStartTime : Utils.GetDateTimeNullable(Utils.GetQueryStringValue(("SignBillStartDate")));
            SignBillEndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("SignBillEndDate")) == null ? CurrEndTime : Utils.GetDateTimeNullable(Utils.GetQueryStringValue("SignBillEndDate"));
            Salser = Utils.GetQueryStringValue("Salser");
            DepartName = Utils.GetQueryStringValue("DepartName");
            DepartId = Utils.GetQueryStringValue("DepartId");
            RoyaltyRatio = Utils.GetQueryStringValue("RoyaltyRatio");
            SalserId = Utils.GetQueryStringValue("SalserId");
            EyouSoft.Model.StatisticStructure.QueryPersonnelStatistic model = new EyouSoft.Model.StatisticStructure.QueryPersonnelStatistic();
            model.LeaveDateStart = LeaveTourStartDate;
            model.LeaveDateEnd = LeaveTourEndDate;
            model.CheckDateStart = SignBillStartDate;
            model.CheckDateEnd = SignBillEndDate;
            model.CompanyId = this.CurrentUserCompanyID;
            model.DepartIds = Utils.GetIntArray(DepartId, ",");
            //设置查询model的ComputeOrderType值
            EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? computerOrderType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(SiteUserInfo.CompanyID);
            if (computerOrderType.HasValue)
            {
                model.ComputeOrderType = computerOrderType.Value;
            }
            if (AccounterId != "")
            {
                model.LogisticsIds = Utils.GetIntArray(AccounterId, ",");
            }
            if (SalserId != "")
            {
                model.SaleIds = Utils.GetIntArray(SalserId, ",");
            }
            model.OrderIndex = 0;
            #region 初始化文本框
            this.txtLeaveTourStartDate.Value = LeaveTourStartDate.HasValue ? LeaveTourStartDate.Value.ToString("yyyy-MM-dd") : "";
            this.txtLeaveTourEndDate.Value = LeaveTourEndDate.HasValue ? LeaveTourEndDate.Value.ToString("yyyy-MM-dd") : "";
            this.txtSignBillStartDate.Value = SignBillStartDate.HasValue ? SignBillStartDate.Value.ToString("yyyy-MM-dd") : "";
            this.txtSignBillEndDate.Value = SignBillEndDate.HasValue ? SignBillEndDate.Value.ToString("yyyy-MM-dd") : "";
            this.txtRoyaltyRatio.Value = RoyaltyRatio;
            this.UCSelectDepartment.GetDepartmentName = DepartName;
            this.UCSelectDepartment.GetDepartId = DepartId;
            SelectSalser.OperName = Salser;
            SelectSalser.OperId = SalserId;
            SelectAccounter.OperName = Accounter;
            SelectAccounter.OperId = AccounterId;
            #endregion
            if (Request.QueryString["isExport"] != null && Utils.InputText(Request.QueryString["isExport"]) == "1")
                PageSize = int.MaxValue - 1;

            EmpIncomList = EyouSoft.Common.Function.SelfExportPage.GetList<EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic>(PageIndex, PageSize, out RecordCount, EmployeeAchiveBLL.GetPersonnelIncomeStatistic(model));
            if (EmpIncomList != null && EmpIncomList.Count != 0)
            {
                this.tbl_ExPageEmp.Visible = true;
                this.crp_EmpIncomeList.DataSource = EmpIncomList;
                this.crp_EmpIncomeList.DataBind();
                BindPage();//绑定分页
            }
            else
            {
                this.tbl_ExPageEmp.Visible = false;
                this.crp_EmpIncomeList.EmptyText = "<tr bgcolor=\"#e3f1fc\"><td colspan='6' id=\"EmptyData\" align='center'>暂时没有数据！</td></tr>";
            }
            //释放资源
            EmployeeAchiveBLL = null;
            model = null;
        }
        #endregion

        #region 获取计调员
        public string GetAccouter(IList<StatisticOperator> list)
        {
            return Utils.GetListConverToStr(list);
        }
        #endregion

        #region 提成
        /// <summary>
        /// 收入*提成比例
        /// </summary>
        /// <param name="EmplyeeIncome"></param>
        /// <returns></returns>
        public string GetRoyalty(string EmplyeeIncome)
        {
            string returnVal = string.Empty;
            RoyaltyRatio = Utils.GetQueryStringValue("RoyaltyRatio");
            if (RoyaltyRatio != "" && EmplyeeIncome != "")
            {
                decimal price = decimal.Parse(EmplyeeIncome) * decimal.Parse(RoyaltyRatio);
                returnVal = "￥" + Convert.ToString(Utils.FilterEndOfTheZeroDecimal(price));
            }
            return returnVal;
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

        #region 导出Excel
        private void ToExcel(System.Web.UI.WebControls.Repeater ctl, IList<EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic> EmpIncomList)
        {
            ctl.Visible = true;
            ctl.DataSource = EmpIncomList;
            ctl.DataBind();

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=PerAreaStaticFile.xls");

            HttpContext.Current.Response.Charset = "UTF-8";

            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;

            HttpContext.Current.Response.ContentType = "application/ms-excel";

            ctl.Page.EnableViewState = false;
            StringWriter sw = new StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
            ctl.RenderControl(hw);
            ctl.Visible = false;
            HttpContext.Current.Response.Write(sw.ToString());
            HttpContext.Current.Response.End();
        }
        #endregion


    }
}
