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
using EyouSoft.Model.StatisticStructure;
using Common.Enum;
namespace Web.StatisticAnalysis.ProfitStatistic
{
    /// <summary>
    /// 页面功能：利润统计--按区域统计
    /// Author:Liuym
    /// Date:2011-01-20
    /// </summary>
    public partial class ProAreaStatisticList : BackPage
    {
        #region Private Members
        protected int PageSize = 20;
        protected int PageIndex = 1;
        protected int RecordCount = 0;
        protected int TourType;//团队类型
        protected DateTime? LeaveStartDate = null;//出团开始日期
        protected DateTime? LeaveEndDate = null;//出团结束日期
        protected DateTime? CheckStartTime = null;//开始核算日期
        protected DateTime? CheckEndTime = null;//结束核算日期
        IList<EyouSoft.Model.StatisticStructure.EarningsAreaStatistic> list = null;
        public string URL = string.Empty;

        /// <summary>
        /// 查询-操作人部门
        /// </summary>
        protected string DeptIds = string.Empty;
        /// <summary>
        /// 查询-操作人
        /// </summary>
        protected string OperatorIds = string.Empty;

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

            #region  根据Url参数，设置显示记录数
            if ((Request.QueryString["isAll"] != null && Utils.InputText(Request.QueryString["isAll"]) == "1")
                || (Request.QueryString["isExport"] != null && Utils.InputText(Request.QueryString["isExport"]) == "1")
                || Utils.GetInt(Utils.GetQueryStringValue("IsCartogram"), 0) > 0)
            {
                PageSize = int.MaxValue - 1;
            }
            #endregion

            #region 获取团队数URL
            EyouSoft.BLL.CompanyStructure.CompanySetting setBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting setModel = setBll.GetSetting(SiteUserInfo.CompanyID);
            URL = setModel.ProfitStatTourPagePath;
            #endregion

            #region 初始化数据
            if (!IsPostBack)
            {
                InitProAreaStaticlist();
            }
            #endregion

            #region 导出报表
            if (Utils.GetQueryStringValue("isExport") == "1")
            {
                if (list != null && list.Count != 0)
                {
                    ToExcel(this.crp_PrintGetProAreaList, list);
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

        #region  区域统计数据
        private void InitProAreaStaticlist()
        {
            #region 获取参数
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            LeaveStartDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveStartDate"), new DateTime(DateTime.Now.Year, 1, 1));
            LeaveEndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveEndDate"), new DateTime(DateTime.Now.Year, 12, 31));
            CheckStartTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("CheckStartDate"));
            CheckEndTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("CheckEndDate"));
            TourType = Utils.GetInt(Utils.GetQueryStringValue("TourType"), -1);
            DeptIds = Utils.GetQueryStringValue("deptids");
            OperatorIds = Utils.GetQueryStringValue("operatorids");
            #endregion

            #region 实体赋值
            EyouSoft.BLL.StatisticStructure.EarningsStatistic EarningBll = new EyouSoft.BLL.StatisticStructure.EarningsStatistic(this.SiteUserInfo);
            EyouSoft.Model.StatisticStructure.QueryEarningsStatistic model = new EyouSoft.Model.StatisticStructure.QueryEarningsStatistic();
            model.OrderIndex = 0;
            model.LeaveDateStart = LeaveStartDate;
            model.LeaveDateEnd = LeaveEndDate;
            model.CheckDateStart = CheckStartTime;
            model.CheckDateEnd = CheckEndTime;
            model.CompanyId = this.SiteUserInfo.CompanyID;
            //设置查询model的ComputeOrderType值
            EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? computerOrderType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(SiteUserInfo.CompanyID);
            if (computerOrderType.HasValue)
            {
                model.ComputeOrderType = computerOrderType.Value;
            }
            TourTypeList1.TourType = TourType;
            if (TourType != -1)
            {
                model.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)TourType;
            }
            model.DepartIds = Utils.GetIntArray(DeptIds, ",");
            model.SaleIds = Utils.GetIntArray(OperatorIds, ",");
            #endregion

            #region 初始化表单元素
            this.txtCheckStartDate.Value = CheckStartTime.HasValue ? CheckStartTime.Value.ToString("yyyy-MM-dd") : ""; ;
            this.txtCheckEndDate.Value = CheckEndTime.HasValue ? CheckEndTime.Value.ToString("yyyy-MM-dd") : "";
            this.txtLeaveStartDate.Value = LeaveStartDate.HasValue ? LeaveStartDate.Value.ToString("yyyy-MM-dd") : "";
            this.txtLeaveEndDate.Value = LeaveEndDate.HasValue ? LeaveEndDate.Value.ToString("yyyy-MM-dd") : "";

            UCSelectDepartment1.GetDepartmentName = Utils.GetQueryStringValue("deptnames");
            UCSelectDepartment1.GetDepartId = DeptIds;
            SelectOperator.OperName = Utils.GetQueryStringValue("operatornames");
            SelectOperator.OperId = OperatorIds;

            #endregion
            IList<EyouSoft.Model.StatisticStructure.EarningsAreaStatistic> statisticList = new List<EyouSoft.Model.StatisticStructure.EarningsAreaStatistic>();
            statisticList = EarningBll.GetEarningsAreaStatistic(model);
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
            list = EyouSoft.Common.Function.SelfExportPage.GetList<EyouSoft.Model.StatisticStructure.EarningsAreaStatistic>(PageIndex, PageSize, out RecordCount, statisticList);
            if (Utils.GetInt(Utils.GetQueryStringValue("IsCartogram"), 0) > 0) //标识为统计图异步请求
                return;
            if (list != null && list.Count != 0)
            {
                this.tbl_ExportPage.Visible = true;
                this.crp_GetProAreaList.DataSource = list;
                this.crp_GetProAreaList.DataBind();
                BindPage();//绑定分页
            }
            else
            {
                this.tbl_ExportPage.Visible = false;
                this.crp_GetProAreaList.EmptyText = "<tr><td id=\"EmptyData\" colspan='9' bgcolor='#e3f1fc' height='50px' align='center'>暂时没有数据！</td></tr>";
            }

            RegisterScript(string.Format("var recordCount={0};", RecordCount));

            model = null;
            EarningBll = null;
        }
        #endregion

        #region 获取计调员
        public string GetAccounter(IList<StatisticOperator> SalesClerk)
        {
            return Utils.GetListConverToStr(SalesClerk);
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
        private void ToExcel(System.Web.UI.WebControls.Repeater ctl, IList<EyouSoft.Model.StatisticStructure.EarningsAreaStatistic> PerAreaList)
        {
            ctl.Visible = true;
            ctl.DataSource = PerAreaList;
            ctl.DataBind();

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=ProAreaStaticFile.xls");
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

        #region 线路区域统计图Flash
        public void GetCartogramFlashXml()
        {
            #region 构造XML数据
            StringBuilder strXml = new StringBuilder();
            if (list != null && list.Count != 0)
            {
                strXml.AppendFormat(@"<graph xAxisName='线路区域'  formatNumber='0'  formatNumberScale='0' decimalPrecision='0' yAxisName='' yaxisminvalue='0' yaxismaxvalue='' limitsDecimalPrecision='0' divLineDecimalPrecision=''>");

                StringBuilder strCategory = new StringBuilder("<categories>");
                //StringBuilder strDataSet = new StringBuilder("<dataset seriesname='团量' color='#AFD8F8' showValue='1'>");
                StringBuilder strDataSet1 = new StringBuilder("<dataset seriesname='收入' color='#FF8E46' showValue='1'>");
                StringBuilder strDataSet2 = new StringBuilder("<dataset seriesname='支出' color='#87CEFA' showValue='1'>");
                StringBuilder strDataSet3 = new StringBuilder("<dataset seriesname='利润' color='#8470FF' showValue='1'>");
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].AreaName == null || string.IsNullOrEmpty(list[i].AreaName))
                        continue;
                    //线路名称
                    strCategory.AppendFormat(@"<category name='" + list[i].AreaName.ToString() + "' hoverText='" + list[i].AreaName.ToString() + "'/>");
                    //团量
                    //strDataSet.AppendFormat(@"<set value='{0}' link='{1}' />", list[i].TourNum.ToString(), "javascript:GetproTourLRTJ();");
                    //收入
                    strDataSet1.AppendFormat(@"<set value='{0}' />", (list[i].GrossIncome/10000).ToString("0.00"));
                    //支出
                    strDataSet2.AppendFormat(@"<set value='{0}' />", (list[i].GrossOut/10000).ToString("0.00"));

                    //利润
                    strDataSet3.AppendFormat(@"<set value='{0}' />", (list[i].CompanyShare/10000).ToString("0.00"));
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
            #endregion
            Response.Clear();
            Response.Write(strXml.ToString());
            Response.End();
        }
        #endregion
    }
}
