using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using System.Text;
using System.IO;
using EyouSoft.Common;
using Common.Enum;

namespace Web.StatisticAnalysis.ProfitStatistic
{
    /// <summary>
    /// 页面功能：利润统计--类型统计
    /// Author：liuym
    /// Date:2011-01-20
    /// </summary>
    public partial class ProTypeStatisticList : BackPage
    {
        #region Private Members
        protected int PageSize = 20;
        protected int PageIndex = 1;
        protected int RecordCount = 0;
        protected string DeapartName = string.Empty;//部门名称      
        protected string DepartId = string.Empty;//部门Id 
        protected DateTime? StartDate = null;//开始日期
        protected DateTime? EndDate = null;//结束日期
        protected DateTime? CheckSDate = null;//核算开始时间
        protected DateTime? CheckEDate = null;//核算结束时间
        protected string OperatorName = string.Empty;//操作员
        protected string OperatorId = string.Empty;//操作员ID
        protected IList<EyouSoft.Model.StatisticStructure.EarningsTypeStatistic> ProTypeStaList = new List<EyouSoft.Model.StatisticStructure.EarningsTypeStatistic>();//利润统计-类型统计列表
        public string URL = string.Empty;
        #endregion

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 权限验证
            if (!CheckGrant(TravelPermission.统计分析_利润统计_利润统计栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.统计分析_利润统计_利润统计栏目, true);
                return;
            }
            #endregion
            #region 获取团队数URL
            EyouSoft.BLL.CompanyStructure.CompanySetting setBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting setModel = setBll.GetSetting(SiteUserInfo.CompanyID);
            URL = setModel.ProfitStatTourPagePath;
            #endregion


            #region 根据URL参数初始化PageSize
            if ((Request.QueryString["isAll"] != null && Utils.InputText(Request.QueryString["isAll"]) == "1")
                || (Request.QueryString["isExport"] != null && Utils.InputText(Request.QueryString["isExport"]) == "1"))
            {
                PageSize = int.MaxValue - 1;
            }
            #endregion

            #region 初始化数据
            if (!IsPostBack)
            {
                InitProAreaStaticlist();
            }
            #endregion

            #region 导出报表请求
            if (Request.QueryString["isExport"] != null && Utils.InputText(Request.QueryString["isExport"]) == "1")
            {
                if (ProTypeStaList != null && ProTypeStaList.Count != 0)
                {
                    ToExcel(this.crp_printGetProTypeList, ProTypeStaList);
                }
            }
            #endregion

            #region 统计图异步请求
            if (Utils.GetInt(Utils.GetQueryStringValue("IsCartogram"), 0) > 0)
            {
                GetCartogramFlashXml();
            }
            #endregion
        }
        #endregion

        #region  区域统计数据
        private void InitProAreaStaticlist()
        {
            #region 页面参数初始化
            PageIndex = Utils.GetInt(Request.QueryString["Page"], 1);
            DeapartName = Utils.GetQueryStringValue("departName");
            OperatorName = Utils.GetQueryStringValue("OperatorName");
            OperatorId = Utils.GetQueryStringValue("OperatorId");
            StartDate = Utils.GetDateTimeNullable(Request.QueryString["StartDate"], new DateTime(DateTime.Now.Year, 1, 1));
            EndDate = Utils.GetDateTimeNullable(Request.QueryString["EndDate"], new DateTime(DateTime.Now.Year, 12, 31));
            DepartId = Utils.GetQueryStringValue("DepartmentId");
            CheckSDate = Utils.GetDateTimeNullable(Request.QueryString["CheckSDate"]);
            CheckEDate = Utils.GetDateTimeNullable(Request.QueryString["CheckEDate"]);
            #endregion

            #region 表单控件初始化
            UCSelectDepartment1.GetDepartmentName = DeapartName;
            UCSelectDepartment1.GetDepartId = DepartId;
            SelectOperator.OperName = OperatorName;
            SelectOperator.OperId = OperatorId;
            txtStartDate.Value = StartDate.HasValue ? StartDate.Value.ToString("yyyy-MM-dd") : "";
            txtEndDate.Value = EndDate.HasValue ? EndDate.Value.ToString("yyyy-MM-dd") : "";
            txtCheckStartTime.Value = CheckSDate.HasValue ? CheckSDate.Value.ToString("yyyy-MM-dd") : "";
            txtCheckEndTime.Value = CheckEDate.HasValue ? CheckEDate.Value.ToString("yyyy-MM-dd") : "";
            #endregion

            #region 调用底层方法 查询Model赋值
            EyouSoft.Model.StatisticStructure.QueryEarningsStatistic model = new EyouSoft.Model.StatisticStructure.QueryEarningsStatistic();
            EyouSoft.BLL.StatisticStructure.EarningsStatistic EarningBll = new EyouSoft.BLL.StatisticStructure.EarningsStatistic(this.SiteUserInfo);
            model.CompanyId = base.SiteUserInfo.CompanyID;
            model.OrderIndex = 4;
            model.LeaveDateStart = StartDate;
            model.LeaveDateEnd = EndDate;
            if (!string.IsNullOrEmpty(OperatorId))
            {
                model.SaleIds = Utils.GetIntArray(OperatorId, ",");
            }
            if (!string.IsNullOrEmpty(DepartId))
            {
                model.DepartIds = Utils.GetIntArray(DepartId, ",");
            }
            //设置查询model的ComputeOrderType值
            EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? computerOrderType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(SiteUserInfo.CompanyID);
            if (computerOrderType.HasValue)
            {
                model.ComputeOrderType = computerOrderType.Value;
            }
            model.CheckDateStart = CheckSDate;
            model.CheckDateEnd = CheckEDate;

            IList<EyouSoft.Model.StatisticStructure.EarningsTypeStatistic> statisticList = new List<EyouSoft.Model.StatisticStructure.EarningsTypeStatistic>();
            statisticList = EarningBll.GetEarningsTypeStatistic(model);
            #region 统计 by 田想兵 3.9
            int teamNum = 0;
            decimal inMoney = 0;
            decimal outMoney = 0;
            decimal teamGross = 0;
            decimal pepoleGross = 0;
            decimal profitallot = 0;
            decimal comProfit = 0;
            int peopleNum = 0;
            int propleSum = 0;
            foreach (var v in statisticList)
            {
                peopleNum += v.TourPeopleNum;
                teamNum += v.TourNum;
                inMoney += v.GrossIncome;
                outMoney += v.GrossOut;
                teamGross += v.TourGross;
                //pepoleGross += v.PeopleGross;
                profitallot += v.TourShare;
                comProfit += v.CompanyShare;
                propleSum += v.TourPeopleNum;
            }
            if (peopleNum == 0)
                pepoleGross = 0;
            else
                pepoleGross = teamGross / peopleNum;
            lt_teamNum.Text = teamNum.ToString();
            lt_InMoney.Text = inMoney.ToString("###,##0.00");
            lt_outMoney.Text = outMoney.ToString("###,##0.00");
            lt_teamgross_profit.Text = teamGross.ToString("###,##0.00");
            lt_pepolegross_profit.Text = pepoleGross.ToString("###,##0.00");
            lt_profitallot.Text = profitallot.ToString("###,##0.00");
            lt_comProfit.Text = comProfit.ToString("###,##0.00");
            lt_peopleSum.Text = propleSum.ToString();
            if (inMoney != 0)
            {
                lt_lirunlv.Text = (Convert.ToDouble(comProfit) / Convert.ToDouble(inMoney)).ToString("0.00%");
            }
            else
            {
                lt_lirunlv.Text = "0.00%";
            }
            #endregion
            ProTypeStaList = EyouSoft.Common.Function.SelfExportPage.GetList<EyouSoft.Model.StatisticStructure.EarningsTypeStatistic>(PageIndex, PageSize, out RecordCount, EarningBll.GetEarningsTypeStatistic(model));
            #endregion

            #region 数据源绑定
            if (ProTypeStaList != null && ProTypeStaList.Count != 0)
            {
                this.crp_GetProTypeList.DataSource = ProTypeStaList;
                this.crp_GetProTypeList.DataBind();
                BindPage();//绑定分页
            }
            else
            {
                this.tbl_ExportPage.Visible = false;
                this.crp_GetProTypeList.EmptyText = "<tr><td colspan='9' id=\"EmptyData\" bgcolor='#e3f1fc' height='50px' align='center'>暂时没有数据！</td></tr>";
            }
            #endregion

            RegisterScript(string.Format("var recordCount={0};", RecordCount));
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
        private void ToExcel(System.Web.UI.WebControls.Repeater ctl, IList<EyouSoft.Model.StatisticStructure.EarningsTypeStatistic> PerAreaList)
        {
            ctl.Visible = true;
            ctl.DataSource = PerAreaList;
            ctl.DataBind();

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=ProAreaStaticFile.xls");

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

        #region 利润统计按类型统计图Flash
        public void GetCartogramFlashXml()
        {
            StringBuilder strXml = new StringBuilder();
            if (ProTypeStaList != null && ProTypeStaList.Count != 0)
            {
                strXml.AppendFormat(@"<graph xAxisName='类型' formatNumber='0' formatNumberScale='0' decimalPrecision='0' yAxisName=''  yaxisminvalue='0' yaxismaxvalue='' limitsDecimalPrecision='0' divLineDecimalPrecision=''>");
                StringBuilder strCategory = new StringBuilder("<categories>");
                //StringBuilder strDataSet = new StringBuilder("<dataset seriesname='团量' color='#AFD8F8' showValue='1'>");
                StringBuilder strDataSet1 = new StringBuilder("<dataset seriesname='收入' color='#FF8E46' showValue='1'>");
                StringBuilder strDataSet2 = new StringBuilder("<dataset seriesname='支出' color='#87CEFA' showValue='1'>");
                StringBuilder strDataSet3 = new StringBuilder("<dataset seriesname='利润' color='#8470FF ' showValue='1'>");
                for (int i = 0; i < ProTypeStaList.Count; i++)
                {
                    //团队类型
                    strCategory.AppendFormat(@"<category name='" + ProTypeStaList[i].TourType.ToString() + "' hoverText='" + ProTypeStaList[i].TourType.ToString() + "'/>");
                    //团量
                   // strDataSet.AppendFormat(@"<set value='{0}' link='{1}' />", ProTypeStaList[i].TourNum.ToString(), "javascript:GetTourNumList();");
                    //收入
                    strDataSet1.AppendFormat(@"<set value='{0}' />", (ProTypeStaList[i].GrossIncome / 10000).ToString("0.00"));
                    //支出
                    strDataSet2.AppendFormat(@"<set value='{0}' />", (ProTypeStaList[i].GrossOut / 10000).ToString("0.00"));
                    //利润
                    strDataSet3.AppendFormat(@"<set value='{0}' />", (ProTypeStaList[i].TourGross / 10000).ToString("0.00"));
                }
                strCategory.Append("</categories>");
                //strDataSet.Append("</dataset>");
                strDataSet1.Append("</dataset>");
                strDataSet2.Append("</dataset>");
                strDataSet3.Append("</dataset>");
                strXml.Append(strCategory.ToString());
                //strXml.Append(strDataSet.ToString());
                strXml.Append(strDataSet1.ToString());
                strXml.Append(strDataSet2.ToString());
                strXml.Append(strDataSet3.ToString());
                strXml.Append(@"</graph>");
            }
            Response.Clear();
            Response.Write(strXml.ToString());
            Response.End();
        }
        #endregion

        #region 获取销售员集合
        protected string GetSalers(IList<EyouSoft.Model.StatisticStructure.StatisticOperator> SalesClerk)
        {
            return Utils.GetListConverToStr(SalesClerk);
        }
        #endregion

    }

}
