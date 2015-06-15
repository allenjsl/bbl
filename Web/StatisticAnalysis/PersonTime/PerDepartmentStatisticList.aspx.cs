using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using System.Text;
using System.IO;
using EyouSoft.Model.StatisticStructure;
using Common.Enum;
namespace Web.StatisticAnalysis.PersonTime
{
    /// <summary>
    /// 页面功能：按部门统计
    /// Author:liuym
    /// Date:2011-1-18
    /// </summary>
    public partial class PerDepartmentStatisticList : BackPage
    {
        #region Private Members
        protected int PageSize = 20;//每页显示的记录
        protected int RecordCount = 0;//总记录条数
        protected int PageIndex = 1;//当前页
        protected bool IsExport = false;//是否导出
        protected string RouteAreaId = string.Empty;//线路ID
        protected string Salser = string.Empty;//销售员
        protected string SalserID = string.Empty;//销售员ID
        protected DateTime? OrderStartTime = null;//开始下订单时间
        protected DateTime? OrderEndTime = null;//结束下订单时间
        IList<EyouSoft.Model.StatisticStructure.InayaDepartStatistic> InayaDepartList = null;
        protected int AllPopleNum = 0;   //合计人数
        protected string TourTypeSearchOptionHTML = string.Empty;//计划类型查询下拉菜单option
        protected int TuanDuiShuHeJi = 0;//团队数合计
        #endregion

        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 权限验证
            if (!CheckGrant(TravelPermission.统计分析_人次统计_人次统计栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.统计分析_人次统计_人次统计栏目, true);
                return;
            }
            #endregion

            TourTypeSearchOptionHTML = Utils.GetTourTypeSearchOptionHTML(CurrentUserCompanyID, Utils.GetQueryStringValue("tourtype"), false);

            #region 根据URL参数初始化PageSize
            if (Utils.GetInt(Request.QueryString["isAll"], 0) == 1
                || Utils.GetInt(Request.QueryString["isExport"], 0) == 1
                || Utils.GetInt(Request.QueryString["IsCartogram"], 0) == 1)
            {
                PageSize = int.MaxValue - 1; //-1为了避免存储过程报错
            }
            #endregion

            #region 初始化数据源
            if (!IsPostBack)
            {
                BindPerDepartmentStaticList();
            }
            #endregion

            #region 导出报表请求
            if (Utils.GetInt(Request.QueryString["isExport"], 0) == 1)
            {
                if (InayaDepartList != null && InayaDepartList.Count != 0)
                {
                    ToExcel(this.crp_PrintPerDepartStaList, InayaDepartList);
                }
            }
            #endregion

            #region 统计图异步请求
            if (Utils.GetInt(Request.QueryString["IsCartogram"], 0) == 1)
            {
                GetCartogramFlashXml();
            }
            #endregion
        }
        #endregion

        #region 导出Excel
        private void ToExcel(System.Web.UI.WebControls.Repeater ctl, IList<EyouSoft.Model.StatisticStructure.InayaDepartStatistic> PerAreaList)
        {
            ctl.Visible = true;
            ctl.DataSource = PerAreaList;
            ctl.DataBind();

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=PerDepartmentStaticFile.xls");

            HttpContext.Current.Response.Charset = "UTF-8";

            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;

            HttpContext.Current.Response.ContentType = "application/ms-excel";

            ctl.Page.EnableViewState = false;
            this.ExportPageInfo1.Visible = false;
            StringWriter sw = new StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
            ctl.RenderControl(hw);
            ctl.Visible = false;
            HttpContext.Current.Response.Write(sw.ToString());
            HttpContext.Current.Response.End();
        }
        #endregion

        #region 绑定按部门统计列表显示
        private void BindPerDepartmentStaticList()
        {
            EyouSoft.BLL.StatisticStructure.InayatStatistic InayaStaBLL = new EyouSoft.BLL.StatisticStructure.InayatStatistic(this.SiteUserInfo);

            #region URL参数检测、查询实体赋值
            ////获取当月开始时间 本月的第一天
            //DateTime? CurrStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ////当月结束时间 本月的最后一天
            //DateTime? CurrEndTime = CurrStartTime.Value.AddMonths(1).AddDays(-1);
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            Salser = Utils.GetQueryStringValue("Salser");
            SalserID = Utils.GetQueryStringValue("SalserId");
            OrderStartTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("OrderStartTime"));
            OrderEndTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("OrderEndTime"));
            RouteAreaId = Utils.GetQueryStringValue("RouteAreaId");
            EyouSoft.Model.StatisticStructure.QueryInayatStatistic QueryInayatStaticModel = new EyouSoft.Model.StatisticStructure.QueryInayatStatistic();
            //设置查询model的ComputeOrderType值
            EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? computerOrderType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(SiteUserInfo.CompanyID);
            if (computerOrderType.HasValue)
            {
                QueryInayatStaticModel.ComputeOrderType = computerOrderType.Value;
            }
            if (SalserID != "")
            {
                QueryInayatStaticModel.SaleIds = Utils.GetIntArray(SalserID, ",");
            }
            if (RouteAreaId != "" && RouteAreaId != "0")
            {
                QueryInayatStaticModel.AreaId = int.Parse(RouteAreaId);
            }
            QueryInayatStaticModel.OrderIndex = 2;
            QueryInayatStaticModel.CompanyId = CurrentUserCompanyID;
            #region 按出团时间查询 by txb 2011.6.20
            QueryInayatStaticModel.LeaveDateStart = OrderStartTime;
            QueryInayatStaticModel.LeaveDateEnd = OrderEndTime;
            #endregion
            QueryInayatStaticModel.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType?)Utils.GetEnumValue(typeof(EyouSoft.Model.EnumType.TourStructure.TourType), Utils.GetQueryStringValue("tourtype"), null);
            #endregion

            #region 表单控件初始化
            this.txtOrderStarTime.Value = OrderStartTime.HasValue ? OrderStartTime.Value.ToString("yyyy-MM-dd") : "";
            this.txtOrderEndTime.Value = OrderEndTime.HasValue ? OrderEndTime.Value.ToString("yyyy-MM-dd") : "";
            if (RouteAreaId != "")
            {
                this.RouteAreaList1.RouteAreaId = int.Parse(RouteAreaId);
            }
            this.SelectSalser.OperName = Salser;
            this.SelectSalser.OperId = SalserID;
            #endregion

            InayaDepartList = InayaStaBLL.GetInayaDepartStatistic(QueryInayatStaticModel);
            SumAllPeopleNum(InayaDepartList);
            InayaDepartList = EyouSoft.Common.Function.SelfExportPage.GetList<EyouSoft.Model.StatisticStructure.InayaDepartStatistic>(PageIndex, PageSize, out RecordCount, InayaDepartList);
            if (Utils.GetInt(Request.QueryString["IsCartogram"], 0) == 1) //统计图异步请求时无须绑定列表
                return;

            #region 列表绑定
            if (InayaDepartList != null && InayaDepartList.Count != 0)
            {
                if (Request.QueryString["isExport"] != null && Utils.InputText(Request.QueryString["isExport"]) == "1")
                    this.tbl_PageInfo.Visible = false;
                else
                    this.tbl_PageInfo.Visible = true;
                this.crp_PerDepartStaList.DataSource = InayaDepartList;
                this.crp_PerDepartStaList.DataBind();
                BindPage();//绑定分页
            }
            else
            {
                this.tbl_PageInfo.Visible = false;
                this.crp_PerDepartStaList.EmptyText = "<tr bgcolor='#e3f1fc'><td height='50px' colspan='4' id=\"EmptyData\"  height='50px' align='center'>暂时没有数据！</td></tr>";
            }
            #endregion

        }
        #endregion

        #region 获取销售员
        public string GetSalsers(IList<StatisticOperator> list)
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

        #region 部门统计图Flash
        private void GetCartogramFlashXml()
        {
            #region XML构造
            StringBuilder strXml = new StringBuilder();
            if (InayaDepartList != null && InayaDepartList.Count > 0)
            {
                strXml.Append(@"<graph formatNumber='0' formatNumberScale='0' decimalPrecision='0'  xAxisName='部门' yAxisName=''  yaxisminvalue='0' yaxismaxvalue='' limitsDecimalPrecision='0' divLineDecimalPrecision='0'>");
                StringBuilder strCategory = new StringBuilder("<categories>");
                StringBuilder strDataSet = new StringBuilder("<dataset seriesname='人数' color='#AFD8F8' showValue='1'>");

                for (int i = 0; i < InayaDepartList.Count; i++)
                {
                    string deparName = InayaDepartList[i].DepartName == null ? null : InayaDepartList[i].DepartName.ToString();
                    int peopleCount = InayaDepartList[i].PeopleCount == 0 ? 0 : InayaDepartList[i].PeopleCount;
                    //int peopleDay = InayaDepartList[i].PeopleDays == 0 ? 0 : InayaDepartList[i].PeopleDays;
                    strCategory.Append(@"<category name='" + deparName + "' hoverText='" + deparName + "'/>");
                    //人数
                    strDataSet.Append(@"<set value='" + peopleCount + "' />");
                }
                strCategory.Append("</categories>");
                strDataSet.Append("</dataset>");
                strXml.Append(strCategory.ToString());
                strXml.Append(strDataSet.ToString());
                strXml.Append(@"</graph>");
            }
            #endregion

            Response.Clear();
            Response.Write(strXml.ToString());
            Response.End();
        }
        #endregion

        #region 计算合计人数

        /// <summary>
        /// 计算合计人数
        /// </summary>
        /// <param name="list">人次-区域统计实体集合</param>
        private void SumAllPeopleNum(IList<InayaDepartStatistic> list)
        {
            if (list == null || list.Count <= 0)  return;
            
            foreach (var inayaDepartStatistic in list)
            {
                if (inayaDepartStatistic == null) continue;

                AllPopleNum += inayaDepartStatistic.PeopleCount;
                TuanDuiShuHeJi += inayaDepartStatistic.TuanDuiShu;
            }
        }

        #endregion
    }
}
