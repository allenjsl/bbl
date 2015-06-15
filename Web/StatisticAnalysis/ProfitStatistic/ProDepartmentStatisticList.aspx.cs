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
    /// 页面功能：利润统计--部门统计
    /// Author:liuym
    /// Date:2011-01-20
    /// </summary>
    public partial class ProDepartmentStatisticList : BackPage
    {
        #region Private Members
        protected int PageSize = 20;
        protected int PageIndex = 1;
        protected int RecordCount = 0;
        protected int TourType;//团队类型      
        protected DateTime? LeaveTourStartDate = null;//出团开始日期
        protected DateTime? LeaveTourEndDate = null;//出团结束日期
        protected DateTime? CheckStartDate = null;//核算开始日期
        protected DateTime? CheckEndDate = null;//核算结束日期
        IList<EyouSoft.Model.StatisticStructure.EarningsDepartStatistic> ProfitList = null;
        public string URL = string.Empty;
        protected int AreaId = 0;
        /// <summary>
        /// 查询-操作人部门
        /// </summary>
        protected string DeptIds = string.Empty;
        /// <summary>
        /// 查询-操作人
        /// </summary>
        protected string OperatorIds = string.Empty;
        #endregion

        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 权限验证
            if (!CheckGrant(TravelPermission.统计分析_利润统计_利润统计栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.统计分析_利润统计_利润统计栏目, true);
                return;
            }
            #endregion

            #region 根据Url参数，设置显示记录数
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
                InitProDepartStaticlist();
            }
            #endregion

            #region 导出报表请求
            if (Request.QueryString["isExport"] != null && Utils.InputText(Request.QueryString["isExport"]) == "1")
            {
                if (ProfitList != null && ProfitList.Count != 0)
                {
                    ToExcel(this.crp_PrintGetProDepartList, ProfitList);
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
        private void InitProDepartStaticlist()
        {
            #region 查询参数
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            LeaveTourStartDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveTourStartDate"), new DateTime(DateTime.Now.Year, 1, 1));
            LeaveTourEndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveTourEndDate"), new DateTime(DateTime.Now.Year, 12, 31));
            CheckStartDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("CheckStartTime"));
            CheckEndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("CheckEndTime"));
            TourType = Utils.GetInt(Utils.GetQueryStringValue("TourType"), -1);
            AreaId = Utils.GetInt(Utils.GetQueryStringValue("areaid"));
            DeptIds = Utils.GetQueryStringValue("deptids");
            OperatorIds = Utils.GetQueryStringValue("operatorids");
            #endregion

            #region 成员变量
            EyouSoft.Model.StatisticStructure.QueryEarningsStatistic model = new EyouSoft.Model.StatisticStructure.QueryEarningsStatistic();
            EyouSoft.BLL.StatisticStructure.EarningsStatistic EarningBll = new EyouSoft.BLL.StatisticStructure.EarningsStatistic(this.SiteUserInfo);
            #endregion

            #region 查询实体赋值
            model.CheckDateStart = CheckStartDate;
            model.CheckDateEnd = CheckEndDate;
            model.LeaveDateStart = LeaveTourStartDate;
            model.LeaveDateEnd = LeaveTourEndDate;
            model.CompanyId = this.SiteUserInfo.CompanyID;
            model.OrderIndex = 2;
            //设置查询model的ComputeOrderType值
            EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? computerOrderType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(SiteUserInfo.CompanyID);
            if (computerOrderType.HasValue)
            {
                model.ComputeOrderType = computerOrderType.Value;
            }
            TourTypeList1.TourType = TourType;
            if (TourType > -1)
            {
                model.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)TourType;
            }
            model.AreaId = AreaId;

            model.DepartIds = Utils.GetIntArray(DeptIds, ",");
            model.SaleIds = Utils.GetIntArray(OperatorIds, ",");
            #endregion

            #region 表单初始化
            this.txtCheckStartDate.Value = CheckStartDate.HasValue ? CheckStartDate.Value.ToString("yyyy-MM-dd") : ""; ;
            this.txtCheckEndDate.Value = CheckEndDate.HasValue ? CheckEndDate.Value.ToString("yyyy-MM-dd") : "";
            this.txtLeaveTourStartDate.Value = LeaveTourStartDate.HasValue ? LeaveTourStartDate.Value.ToString("yyyy-MM-dd") : "";
            this.txtLeaveTourEndDate.Value = LeaveTourEndDate.HasValue ? LeaveTourEndDate.Value.ToString("yyyy-MM-dd") : "";

            UCSelectDepartment1.GetDepartmentName = Utils.GetQueryStringValue("deptnames");
            UCSelectDepartment1.GetDepartId = DeptIds;
            SelectOperator.OperName = Utils.GetQueryStringValue("operatornames");
            SelectOperator.OperId = OperatorIds;

            RouteAreaList1.RouteAreaId = AreaId;
            #endregion

            IList<EyouSoft.Model.StatisticStructure.EarningsDepartStatistic> statisticList = new List<EyouSoft.Model.StatisticStructure.EarningsDepartStatistic>();
            statisticList = EarningBll.GetEarningsDepartStatistic(model);
            #region 统计 by 田想兵 3.9
            int teamNum = 0;
            decimal inMoney = 0;
            decimal outMoney = 0;
            decimal teamGross = 0;
            decimal pepoleGross = 0;
            decimal profitallot = 0;
            decimal comProfit = 0;
            int peopleNum = 0;
            int peopleSum = 0;
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
                peopleSum += v.TourPeopleNum;
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
            lt_peopleSum.Text = peopleSum.ToString();
            if (inMoney != 0)
            {
                lt_lirunlv.Text = (Convert.ToDouble(comProfit) / Convert.ToDouble(inMoney)).ToString("0.00%");
            }
            else
            {
                lt_lirunlv.Text = "0.00%";
            }
            #endregion
            ProfitList = EyouSoft.Common.Function.SelfExportPage.GetList<EyouSoft.Model.StatisticStructure.EarningsDepartStatistic>(PageIndex, PageSize, out RecordCount, statisticList);
            if (Utils.GetInt(Utils.GetQueryStringValue("IsCartogram"), 0) > 0) //标识为统计图异步请求
                return;

            if (ProfitList != null && ProfitList.Count != 0)
            {
                this.tbl_ExportPage.Visible = true;
                this.crp_GetProDepartList.DataSource = ProfitList;
                this.crp_GetProDepartList.DataBind();
                BindPage();//绑定分页
            }
            else
            {
                this.tbl_ExportPage.Visible = false;
                this.crp_GetProDepartList.EmptyText = "<tr><td colspan='9' id=\"EmptyData\" bgcolor='#e3f1fc' height='50px' align='center'>暂时没有数据！</td></tr>";
            }
            //释放资源
            model = null;
            EarningBll = null;

            RegisterScript(string.Format("var recordCount={0};", RecordCount));
        }
        #endregion

        #region 获取销售员
        public string GetProfSalsers(IList<StatisticOperator> list)
        {
            return Utils.GetListConverToStr(list);
        }
        #endregion

        #region 获取销售员ID
        public string GetProfSalerId(IList<StatisticOperator> list)
        {
            string SalerId = string.Empty;
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    SalerId += SalerId + list[i].OperatorId + ",";
                }
            }
            return SalerId;
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
        private void ToExcel(System.Web.UI.WebControls.Repeater ctl, IList<EyouSoft.Model.StatisticStructure.EarningsDepartStatistic> ProfitList)
        {
            ctl.Visible = true;
            ctl.DataSource = ProfitList;
            ctl.DataBind();

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=ProDepartStaticFile.xls");

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

        #region 线路区域统计图Flash
        public void GetCartogramFlashXml()
        {
            StringBuilder strXml = new StringBuilder();
            if (ProfitList != null && ProfitList.Count != 0)
            {
                strXml.AppendFormat(@"<graph xAxisName='部门' formatNumber='0' formatNumberScale='0' decimalPrecision='0'  yAxisName=''  yaxisminvalue='0' yaxismaxvalue='' limitsDecimalPrecision='0' divLineDecimalPrecision=''>");
                StringBuilder strCategory = new StringBuilder("<categories>");
                //StringBuilder strDataSet = new StringBuilder("<dataset seriesname='团量' color='#AFD8F8' showValue='1'>");
                StringBuilder strDataSet1 = new StringBuilder("<dataset seriesname='收入' color='#FF8E46' showValue='1'>");
                StringBuilder strDataSet2 = new StringBuilder("<dataset seriesname='支出' color='#87CEFA' showValue='1'>");
                StringBuilder strDataSet3 = new StringBuilder("<dataset seriesname='利润' color='#8470FF ' showValue='1'>");
                for (int i = 0; i < ProfitList.Count; i++)
                {
                    if (ProfitList[i].DepartName == null || string.IsNullOrEmpty(ProfitList[i].DepartName))
                        continue;
                    //部门名称
                    strCategory.AppendFormat(@"<category name='" + ProfitList[i].DepartName.ToString() + "' hoverText='" + ProfitList[i].DepartName.ToString() + "'/>");
                    //团量
                    //strDataSet.AppendFormat(@"<set value='{0}' link='{1}' />", ProfitList[i].TourNum.ToString(), "javascript:GetProTourNumList();");
                    //收入
                    strDataSet1.AppendFormat(@"<set value='{0}' />", (ProfitList[i].GrossIncome/10000).ToString("0.00"));
                    //支出
                    strDataSet2.AppendFormat(@"<set value='{0}' />", (ProfitList[i].GrossOut/10000).ToString("0.00"));
                    //利润
                    strDataSet3.AppendFormat(@"<set value='{0}' />", (ProfitList[i].TourGross/10000).ToString("0.00"));
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
    }
}
