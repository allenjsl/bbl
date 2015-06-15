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
    /// 页面功能：利润统计--按时间统计
    /// Author:liuym
    /// Date:2011-1-20
    /// </summary>
    public partial class ProTimeStatisticList : BackPage
    {
        #region Private Members
        protected int PageSize = 20;
        protected int PageIndex = 1;
        protected int RecordCount = 0;
        protected string TourType = string.Empty;//团队类型      
        protected string RouteAreaName = string.Empty;//线路区域
        protected string DepartName = string.Empty;//部门名称
        protected string DepartId = string.Empty;//部门Id 
        protected string OperatorName = string.Empty;//操作员
        protected string OperatorId = string.Empty;//操作员ID
        protected DateTime? leaveTourStartDate = null;//出团开始日期
        protected DateTime? leaveTourEndDate = null;//出团结束日期
        protected DateTime? checkStartDate = null;
        protected DateTime? checkEndDate = null;
        protected bool IsExport = false;//是否导出
        IList<EyouSoft.Model.StatisticStructure.EarningsTimeStatistic> list = null;
        public string URL = string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 权限验证
            if (!CheckGrant(TravelPermission.统计分析_利润统计_利润统计栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.统计分析_利润统计_利润统计栏目, true);
                return;
            }
            #endregion

            #region 根据Url参数，来设置PageSize
            string IsPrint = Request.QueryString["isAll"];
            string isExportValue = Request.QueryString["isExport"];
            if ((IsPrint != "" && IsPrint == "1") || (isExportValue != "" && isExportValue == "1"))
            {
                //设置显示的记录为最大值
                PageSize = int.MaxValue - 1;
            }
            #endregion

            //是否导出
            if (isExportValue != "" && isExportValue == "1")
            {
                IsExport = true;
            }

            #region 获取团队数URL
            EyouSoft.BLL.CompanyStructure.CompanySetting setBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting setModel = setBll.GetSetting(SiteUserInfo.CompanyID);
            URL = setModel.ProfitStatTourPagePath;
            #endregion

            #region 初始化数据
            if (!IsPostBack)
            {
                InitProTimeStaticlist();
            }
            #endregion

            #region 导出
            if ((list != null && list.Count != 0) && IsExport)
            {
                ToExcel(this.crp_PrintProTimeStaList, list);
            }
            #endregion
        }

        #region  区域统计数据
        private void InitProTimeStaticlist()
        {
            #region 获取参数
            PageIndex = Utils.GetInt(Request.QueryString["Page"], 1);
            RouteAreaName = Utils.GetQueryStringValue("RouteAreaName");
            leaveTourStartDate = Utils.GetDateTimeNullable(Request.QueryString["leaveTourStarDate"], new DateTime(DateTime.Now.Year, 1, 1));
            leaveTourEndDate = Utils.GetDateTimeNullable(Request.QueryString["leaveTourEndDate"], new DateTime(DateTime.Now.Year, 12, 31));
            checkStartDate = Utils.GetDateTimeNullable(Request.QueryString["checkStartDate"]);
            checkEndDate = Utils.GetDateTimeNullable(Request.QueryString["checkEndDate"]);
            TourType = Utils.GetQueryStringValue("TourType");
            OperatorName = Utils.GetQueryStringValue("OperatorName");
            OperatorId = Utils.GetQueryStringValue("OperatorId");
            DepartName = Utils.GetQueryStringValue("deptnames");
            DepartId = Utils.GetQueryStringValue("deptids");
            #endregion

            #region 初始化表单值
            this.txtcheckStartDate.Value = checkStartDate.HasValue ? checkStartDate.Value.ToString("yyyy-MM-dd") : "";
            this.txtcheckEndDate.Value = checkEndDate.HasValue ? checkEndDate.Value.ToString("yyyy-MM-dd") : "";
            this.txtLeaveStarDate.Value = leaveTourStartDate.HasValue ? leaveTourStartDate.Value.ToString("yyyy-MM-dd") : "";
            this.txtLeaveEndDate.Value = leaveTourEndDate.HasValue ? leaveTourEndDate.Value.ToString("yyyy-MM-dd") : "";
            if (TourType != "")//团队类型
            {
                this.TourTypeList1.TourType = int.Parse(TourType);
            }
            if (RouteAreaName != "")//线路区域名称
            {
                this.RouteAreaList1.RouteAreaId = int.Parse(RouteAreaName);
            }
            this.SelectOperator.OperName = OperatorName;//销售员
            this.SelectOperator.OperId = OperatorId;
            this.UCSelectDepartment1.GetDepartmentName = DepartName;//部门
            this.UCSelectDepartment1.GetDepartId = DepartId;

            #endregion

            #region 调用底层方法 Model赋值
            EyouSoft.Model.StatisticStructure.QueryEarningsStatistic model = new EyouSoft.Model.StatisticStructure.QueryEarningsStatistic();
            EyouSoft.BLL.StatisticStructure.EarningsStatistic EarningBll = new EyouSoft.BLL.StatisticStructure.EarningsStatistic(this.SiteUserInfo);
            if (RouteAreaName != "" && RouteAreaName != "0")
            {
                model.AreaId = int.Parse(RouteAreaName);
            }
            if (TourType != "" && TourType != "-1")
            {
                model.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)int.Parse(TourType);
            }
            model.OrderIndex = 6;
            model.LeaveDateStart = leaveTourStartDate;
            model.LeaveDateEnd = leaveTourEndDate;
            //设置查询model的ComputeOrderType值
            EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? computerOrderType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(SiteUserInfo.CompanyID);
            if (computerOrderType.HasValue)
            {
                model.ComputeOrderType = computerOrderType.Value;
            }

            if (OperatorId != "")
            {
                model.SaleIds = Utils.GetIntArray(OperatorId, ",");
            }
            if (DepartId != "")
            {
                model.DepartIds = Utils.GetIntArray(DepartId, ",");
            }
            model.CheckDateStart = checkStartDate;
            model.CheckDateEnd = checkEndDate;

            IList<EyouSoft.Model.StatisticStructure.EarningsTimeStatistic> statisticList = new List<EyouSoft.Model.StatisticStructure.EarningsTimeStatistic>();

            model.CompanyId = CurrentUserCompanyID;

            statisticList = EarningBll.GetEarningsTimeStatistic(model);
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
            list = EyouSoft.Common.Function.SelfExportPage.GetList<EyouSoft.Model.StatisticStructure.EarningsTimeStatistic>(PageIndex, PageSize, out RecordCount, statisticList);
            #endregion

            if (list != null && list.Count != 0)
            {
                this.tbl_ExportPage.Visible = true;
                this.crp_GetProTimeList.DataSource = list;
                this.crp_GetProTimeList.DataBind();
                BindPage();//绑定分页
            }
            else
            {
                this.tbl_ExportPage.Visible = false;
                this.crp_GetProTimeList.EmptyText = "<tr><td colspan='8' id=\"EmptyData\" bgcolor='#e3f1fc' height='50px' align='center'>暂时没有数据！</td></tr>";
            }
            //释放资源
            model = null;
            EarningBll = null;

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
        private void ToExcel(System.Web.UI.WebControls.Repeater ctl, IList<EyouSoft.Model.StatisticStructure.EarningsTimeStatistic> PerAreaList)
        {
            ctl.Visible = true;
            ctl.DataSource = PerAreaList;
            ctl.DataBind();

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=ProTimeStaticFile.xls");

            HttpContext.Current.Response.Charset = "UTF-8";

            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;

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

        #region 利润统计-按时间统计-图Flash
        public string GetCartogramFlashXml()
        {
            if (list != null && list.Count != 0)
            {
                StringBuilder strXml = new StringBuilder();
                strXml.AppendFormat(@"<graph xAxisName='月份' formatNumber='0' formatNumberScale='0' decimalPrecision='0'  yAxisName=''  yaxisminvalue='0' yaxismaxvalue='' limitsDecimalPrecision='0' divLineDecimalPrecision=''>");
                StringBuilder strCategory = new StringBuilder("<categories>");
                //StringBuilder strDataSet = new StringBuilder("<dataset seriesname='团量' color='#AFD8F8' showValue='1'>");
                StringBuilder strDataSet1 = new StringBuilder("<dataset seriesname='收入' color='#FF8E46' showValue='1'>");
                StringBuilder strDataSet2 = new StringBuilder("<dataset seriesname='支出' color='#87CEFA' showValue='1'>");
                StringBuilder strDataSet3 = new StringBuilder("<dataset seriesname='利润' color='#8470FF' showValue='1'>");
                for (int i = 0; i < list.Count; i++)
                {
                    strCategory.AppendFormat(@"<category name='" + list[i].CurrMonth.ToString() + "' hoverText='" + list[i].CurrMonth.ToString() + "'/>");
                    //团量
                    //strDataSet.AppendFormat(@"<set value='{0}' link='{1}' />", list[i].TourNum.ToString(), "javascript:GetProTourNumList();");
                    //收入
                    strDataSet1.AppendFormat(@"<set value='{0}' />", (list[i].GrossIncome / 10000).ToString("0.00"));
                    //支出
                    strDataSet2.AppendFormat(@"<set value='{0}' />", (list[i].GrossOut / 10000).ToString("0.00"));
                    //利润
                    strDataSet3.AppendFormat(@"<set value='{0}' />", (list[i].CompanyShare / 10000).ToString("0.00"));
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
                return strXml.ToString();
            }
            else
                return "";
        }
        #endregion
    }
}
