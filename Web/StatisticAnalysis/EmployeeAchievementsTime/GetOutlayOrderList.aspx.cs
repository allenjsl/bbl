using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using EyouSoft.Model.PlanStructure;

namespace Web.StatisticAnalysis.EmployeeAchievementsTime
{
    /// <summary>
    /// 员工业绩表--利润统计，支出明细列表
    /// </summary>
    /// liuym 2011-1-25
    public partial class GetOutlayOrderList : BackPage
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
        protected int PageSize = 10;
        protected int PageIndex = 1;
        protected int RecordCount = 0;
        protected DateTime? LeaveTourStartDate = null;//开始出团日期
        protected DateTime? LeaveTourEndDate = null;//结束出团日期
        protected DateTime? CheckOrderStartDate = null;//开始签单日期
        protected DateTime? CheckOrderEndDate = null;//结束签单日期
        protected string DepartId= string.Empty;//部门ID
        protected string SalserId= string.Empty;//销售员ID
        protected string AccounterId = string.Empty;//计调员ID
        IList<PersonalStatics> list = null;
        #endregion

        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                //初始化支出列表
                InitEmpProfitStaList();
            }
        }
        #endregion

        #region 初始化按利润统计
        private void InitEmpProfitStaList()
        {
            ////获取当月开始时间 本月的第一天
            //DateTime? CurrStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ////当月结束时间 本月的最后一天
            //DateTime? CurrEndTime = CurrStartTime.Value.AddMonths(1).AddDays(-1);
            #region 获取参数
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            SalserId = EyouSoft.Common.Utils.GetQueryStringValue("SalserId");
            DepartId = Utils.GetQueryStringValue("DepartId");
            AccounterId = Utils.GetQueryStringValue("AccounterId");
            LeaveTourStartDate = Utils.GetDateTimeNullable(Request.QueryString["LeaveTourStartDate"]);
            LeaveTourEndDate = Utils.GetDateTimeNullable(Request.QueryString["LeaveTourEndDate"]);
            CheckOrderStartDate = Utils.GetDateTimeNullable(Request.QueryString["SignBillStartDate"]) ;
            CheckOrderEndDate = Utils.GetDateTimeNullable(Request.QueryString["SignBillEndDate"]);
 
            #endregion

            #region 查询Model赋值
            EyouSoft.Model.StatisticStructure.QueryPersonnelStatistic model = new EyouSoft.Model.StatisticStructure.QueryPersonnelStatistic();
            model.CheckDateStart = CheckOrderStartDate;
            model.CheckDateEnd = CheckOrderEndDate;
            model.LeaveDateStart = LeaveTourStartDate;
            model.LeaveDateEnd = LeaveTourEndDate;
            if (AccounterId != ""&&AccounterId!="0")
            {
                model.LogisticsIds = Utils.GetIntArray(AccounterId, ",");
            }
            if(SalserId!=""&&SalserId!="0")
            {
                model.SaleIds = Utils.GetIntArray(SalserId, ",");
            }
            if(DepartId!=""&&DepartId!="0")
            {
                model.DepartIds = Utils.GetIntArray(DepartId,",");
            }
            model.CompanyId = this.SiteUserInfo.CompanyID;
            
            #endregion
            //调用底层方法
            EyouSoft.BLL.PlanStruture.TravelAgency TraveAnBLL = new EyouSoft.BLL.PlanStruture.TravelAgency(SiteUserInfo);
            list = EyouSoft.Common.Function.SelfExportPage.GetList<PersonalStatics>(PageIndex,PageSize,out RecordCount,TraveAnBLL.GetStaticsList(model));
            if(list!=null&&list.Count>0)
            {
                this.tbl_ExPageEmp.Visible = true;
                this.crp_GetEmpTourList.DataSource = list;
                this.crp_GetEmpTourList.DataBind();
                 BindPage();//调用分页
            }
            else
            {
                this.tbl_ExPageEmp.Visible=false;
                this.crp_GetEmpTourList.EmptyText = "<tr  bgcolor='#e3f1fc'><td colspan='6' height='50px' align='center'>暂时没有数据！</td></tr>";
            }
            TraveAnBLL = null;
            list = null;
            model = null;
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
