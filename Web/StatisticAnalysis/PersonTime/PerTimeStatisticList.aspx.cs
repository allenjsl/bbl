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
namespace Web.StatisticAnalysis.PersonTime
{
    /// <summary>
    /// 页面功能：按时间统计
    /// Author:Liuym
    /// Date:2011-1-18
    /// </summary>
    public partial class PerTimeStatisticList : BackPage
    {
        #region Private Members
        protected int PageSize = 20;//每页显示的记录
        protected int RecordCount = 0;//总记录条数
        protected int PageIndex = 1;//当前页
        protected bool IsExport = false;//是否导出
        protected string Salser = string.Empty;//销售员
        protected string SalserID = string.Empty;//销售员ID
        protected string DepartName = string.Empty;//部门名称
        protected string DepartId = string.Empty;//部门编号
        protected string RouteAreaId = string.Empty;//线路区域Id    
        IList<EyouSoft.Model.StatisticStructure.InayaTimeStatistic> InayaTimeList = null;
        protected int AllPopleNum;   //合计人数

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
                InitPerTimeStaticlist();
            }
            #endregion

            #region 导出报表请求
            if (Utils.GetInt(Request.QueryString["isExport"], 0) == 1)
            {
                IsExport = true;
                if (InayaTimeList != null && InayaTimeList.Count != 0)
                {
                    ToExcel(this.crp_PrintPerTimeStaList, InayaTimeList);
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

        #region  区域统计数据
        private void InitPerTimeStaticlist()
        {
            #region URL参数检测、查询实体赋值
            EyouSoft.BLL.StatisticStructure.InayatStatistic InayaStaBLL = new EyouSoft.BLL.StatisticStructure.InayatStatistic(this.SiteUserInfo);
            DepartName = Utils.GetQueryStringValue("DepartName");//部门名称
            DepartId = Utils.GetQueryStringValue("DepartId");//部门ID
            PageIndex = Utils.GetInt(Request.QueryString["Page"], 1);
            RouteAreaId = Utils.GetQueryStringValue("RouteAreaId");
            Salser = Utils.GetQueryStringValue("Salser");
            SalserID = Utils.GetQueryStringValue("SalserId");

            EyouSoft.Model.StatisticStructure.QueryInayatStatistic QueryModel = new EyouSoft.Model.StatisticStructure.QueryInayatStatistic();
            QueryModel.CompanyId = CurrentUserCompanyID;
            QueryModel.OrderIndex = 4;
            //设置查询model的ComputeOrderType值
            EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? computerOrderType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(SiteUserInfo.CompanyID);
            if (computerOrderType.HasValue)
            {
                QueryModel.ComputeOrderType = computerOrderType.Value;
            }
            if (SalserID != "")
            {
                QueryModel.SaleIds = Utils.GetIntArray(SalserID, ",");
            }
            if (DepartId != "")
            {
                QueryModel.DepartIds = Utils.GetIntArray(DepartId, ",");
            }
            if (RouteAreaId != "" && RouteAreaId != "0")
            {
                QueryModel.AreaId = int.Parse(RouteAreaId);
            }

            QueryModel.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType?)Utils.GetEnumValue(typeof(EyouSoft.Model.EnumType.TourStructure.TourType), Utils.GetQueryStringValue("tourtype"), null);
            #endregion

            #region 初始化表单
            if (RouteAreaId != "")
            {
                this.RouteAreaList1.RouteAreaId = int.Parse(RouteAreaId);
            }
            this.UCSelectDepartment1.GetDepartmentName = DepartName;
            this.UCSelectDepartment1.GetDepartId = DepartId;
            this.SelectSalser.OperName = Salser;
            this.SelectSalser.OperId = SalserID;
            #endregion

            InayaTimeList = InayaStaBLL.GetInayaTimeStatistic(QueryModel);
            SumAllPeopleNum(InayaTimeList);
            InayaTimeList = EyouSoft.Common.Function.SelfExportPage.GetList<EyouSoft.Model.StatisticStructure.InayaTimeStatistic>(PageIndex, PageSize, out RecordCount, InayaTimeList);
            if (Utils.GetInt(Request.QueryString["IsCartogram"], 0) == 1) //异步请求统计图时不执行列表绑定
                return;

            #region 列表绑定
            if (InayaTimeList != null && InayaTimeList.Count != 0)
            {
                this.tblExporPageSelect.Visible = true;
                this.crp_PerTimeStaList.DataSource = InayaTimeList;
                this.crp_PerTimeStaList.DataBind();
                BindPage();//绑定分页
            }
            else
            {
                this.tblExporPageSelect.Visible = false;
                this.crp_PerTimeStaList.EmptyText = "<tr><td height='50px' bgColor='#e3f1fc' colspan='3' id=\"EmptyData\" height='50px' align='center'>暂时没有数据！</td></tr>";
            }
            #endregion
            QueryModel = null;
            InayaStaBLL = null;


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
        private void ToExcel(System.Web.UI.WebControls.Repeater ctl, IList<EyouSoft.Model.StatisticStructure.InayaTimeStatistic> PerAreaList)
        {
            ctl.Visible = true;
            ctl.DataSource = PerAreaList;
            ctl.DataBind();

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=PerAreaStaticFile.xls");

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

        #region 异步生成统计图Flash的XML
        private void GetCartogramFlashXml()
        {
            #region 构造XML
            StringBuilder strXml = new StringBuilder();
            if (InayaTimeList != null && InayaTimeList.Count > 0)
            {
                strXml.Append(@"<graph formatNumber='0' formatNumberScale='0' decimalPrecision='0'  xAxisName='月份' yAxisName='' yaxisminvalue='0' yaxismaxvalue='' limitsDecimalPrecision='0' divLineDecimalPrecision='0'>");
                StringBuilder strCategory = new StringBuilder("<categories>");
                StringBuilder strDataSet = new StringBuilder("<dataset seriesname='人数' color='#AFD8F8' showValue='1'>");
                StringBuilder strDataSet1 = new StringBuilder("<dataset seriesname='人天数' color='#FF8E46' showValue='1'>");
                for (int i = 0; i < InayaTimeList.Count; i++)
                {
                    if (InayaTimeList[i].CurrMonth == null || string.IsNullOrEmpty(InayaTimeList[i].CurrMonth.ToString()))
                        continue;
                    //月份---Y轴
                    strCategory.Append(@"<category name='" + InayaTimeList[i].CurrMonth.ToString() + "' hoverText='" + InayaTimeList[i].CurrMonth.ToString() + "'/>");
                    //人数
                    strDataSet.Append(@"<set value='" + InayaTimeList[i].PeopleCount.ToString() + "' />");
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
        private void SumAllPeopleNum(IList<InayaTimeStatistic> list)
        {
            if (list == null || list.Count <= 0) return;

            foreach (var inayaTimeStatistic in list)
            {
                if (inayaTimeStatistic == null) continue;
                AllPopleNum += inayaTimeStatistic.PeopleCount;
                TuanDuiShuHeJi += inayaTimeStatistic.TuanDuiShu;
            }
        }

        #endregion
    }
}
