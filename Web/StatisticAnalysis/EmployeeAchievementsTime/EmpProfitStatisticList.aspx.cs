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
    /// 页面功能：统计分析--按利润统计
    /// Author:Liuym
    /// Date:2011-1-18
    /// </summary>
    public partial class EmpProfitStatisticList : BackPage
    {
        #region Private Members
        protected int PageSize = 20;
        protected int PageIndex = 1;
        protected int RecordCount = 0;
        protected DateTime? LeaveStartTourDate = null;//出团开始日期
        protected DateTime? SignBillStartDate = null;//签单开始日期
        protected DateTime? LeaveEndTourDate = null;//出团结束日期
        protected DateTime? SignBillEndDate = null;//签单结束日期
        protected string DepartName = string.Empty;//部门名称
        protected string DepartId = string.Empty;//部门ID
        protected string Salser = string.Empty;//销售员
        protected string SalserId = string.Empty;//销售员ID
        protected string Accounter = string.Empty;//计调员
        protected string AccounterId = string.Empty;//计调员ID
        IList<EyouSoft.Model.StatisticStructure.PersonnelProfitStatistic> EmpProfitStaList = null;
        #endregion

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
                InitEmpProfitStaticlist();
            }
            //导出
            if (Request.QueryString["isExport"] != null && Utils.InputText(Request.QueryString["isExport"]) == "1")
            {
                if (EmpProfitStaList != null && EmpProfitStaList.Count != 0)
                {
                    ToExcel(this.crp_PrintEmpProfitList, EmpProfitStaList);
                }
            }           
        }

        #region  初始化收入统计
        private void InitEmpProfitStaticlist()
        {
            #region 获取参数
            DepartName = Utils.GetQueryStringValue("DepartName");
            DepartId = Utils.GetQueryStringValue("DepartId");
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            Accounter = Utils.GetQueryStringValue("Accounter");
            AccounterId = Utils.GetQueryStringValue("AccounterId");
            LeaveStartTourDate = Utils.GetDateTimeNullable(Request.QueryString["LeaveTourStartDate"]) ;           
            LeaveEndTourDate = Utils.GetDateTimeNullable(Request.QueryString["LeaveTourEndDate"]);
            SignBillStartDate = Utils.GetDateTimeNullable(Request.QueryString["SignBillStartDate"]) ;
            SignBillStartDate = Utils.GetDateTimeNullable(Request.QueryString["SignBillEndDate"]);
            Salser = Utils.GetQueryStringValue("Salser");
            SalserId = Utils.GetQueryStringValue("SalserId");
            #endregion 

            #region 实体赋值
            EyouSoft.Model.StatisticStructure.QueryPersonnelStatistic model = new EyouSoft.Model.StatisticStructure.QueryPersonnelStatistic();
            model.CompanyId = SiteUserInfo.CompanyID;
            model.LeaveDateStart = LeaveStartTourDate;
            model.LeaveDateEnd = LeaveEndTourDate;
            model.CheckDateStart = SignBillStartDate;
            model.CheckDateEnd = SignBillEndDate;
            model.DepartIds = Utils.GetIntArray(DepartId,",");
            //设置查询model的ComputeOrderType值
            EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? computerOrderType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(SiteUserInfo.CompanyID);
            if (computerOrderType.HasValue)
            {
                model.ComputeOrderType = computerOrderType.Value;
            }
            if(AccounterId!="")
            {
                model.LogisticsIds = Utils.GetIntArray(AccounterId,",");
            }
            if(SalserId!="")
            {
                model.SaleIds = Utils.GetIntArray(SalserId,",");
            }
            model.OrderIndex = 1;
            #endregion

            #region 初始化表单
            this.txtLeaveTourStartDate.Value = LeaveStartTourDate.HasValue ? LeaveStartTourDate.Value.ToString("yyyy-MM-dd") : "";
            this.txtLeaveTourEndDate.Value = LeaveEndTourDate.HasValue ? LeaveEndTourDate.Value.ToString("yyyy-MM-dd") : "";
            this.txtSignBillStartDate.Value = SignBillStartDate.HasValue ? SignBillStartDate.Value.ToString("yyyy-MM-dd") : "";
            this.txtSignBillEndDate.Value = SignBillEndDate.HasValue ? SignBillEndDate.Value.ToString("yyyy-MM-dd") : "";
            this.UCSelectDepartment.GetDepartmentName = DepartName;
            this.UCSelectDepartment.GetDepartId = DepartId;
            SelectSalser.OperName = Salser;
            SelectSalser.OperId = SalserId;
            SelectAccounter.OperName = Accounter;
            SelectAccounter.OperId = AccounterId;
            #endregion 

            EyouSoft.BLL.StatisticStructure.PersonnelStatistic EmployeeAchiveBLL = new EyouSoft.BLL.StatisticStructure.PersonnelStatistic(this.SiteUserInfo);
            //底层方法
            if (Request.QueryString["isAll"] != null && Utils.InputText(Request.QueryString["isAll"]) == "1")
            {
                PageSize = int.MaxValue-1;
            }
            EmpProfitStaList = EyouSoft.Common.Function.SelfExportPage.GetList<EyouSoft.Model.StatisticStructure.PersonnelProfitStatistic>(PageIndex, PageSize, out RecordCount, EmployeeAchiveBLL.GetPersonnelProfitStatistic(model));
            if (EmpProfitStaList != null && EmpProfitStaList.Count != 0)
            {
                this.tbl_ExPageEmpProfit.Visible = true;
                this.crp_EmpProfitList.DataSource = EmpProfitStaList;
                this.crp_EmpProfitList.DataBind();
                BindPage();//绑定分页
            }
            else
            {
                this.tbl_ExPageEmpProfit.Visible = false;
                this.crp_EmpProfitList.EmptyText = "<tr bgcolor='#e3f1fc'><td colspan='7' height='50px' id=\"EmptyData\" align='center'>暂时没有数据！</td></tr>";
            }
            model = null;
            EmployeeAchiveBLL = null;
        }
        #endregion

        #region 计调员
        public string GetEmpProfitLogistics(IList<StatisticOperator> list)
        {
            return Utils.GetListConverToStr(list);
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
        private void ToExcel(System.Web.UI.WebControls.Repeater ctl, IList<EyouSoft.Model.StatisticStructure.PersonnelProfitStatistic> PerAreaList)
        {
            ctl.Visible = true;
            ctl.DataSource = PerAreaList;
            ctl.DataBind();

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=EmpProfitStaticFile.xls");

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
