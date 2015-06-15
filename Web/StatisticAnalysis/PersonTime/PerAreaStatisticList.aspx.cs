using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using System.IO;
using EyouSoft.Common;
using System.Text;
using EyouSoft.Model.StatisticStructure;
using Common.Enum;

namespace Web.StatisticAnalysis.PersonTime
{
    /// <summary>
    /// 页面功能：人次统计--区域统计
    /// Author:liuym
    /// Date:2011-01-14
    /// </summary>
    public partial class PerAreaStatisticList : BackPage
    {
        #region Protected Members    
        protected int PageSize = 20;//每页显示的记录
        protected int RecordCount = 0;//总记录条数
        protected int PageIndex = 1;//当前页
        protected string Salser = string.Empty;//销售员
        protected string SalserID = string.Empty;//销售员ID
        protected string DepartmentName = string.Empty;//部门名称
        protected string DepartId = string.Empty;//部门ID
        protected DateTime? OrderStarTime = null;//开始时间
        protected DateTime? OrderEndTime = null;//结束时间
        protected IList<EyouSoft.Model.StatisticStructure.InayaAreatStatistic> PerAreaStaList = null;
        protected int AllPopleNum = 0; //合计人数
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
                //初始化列表数据
                InitPerAreaStaticlist();
            }
            #endregion

            #region 导出报表请求
            if ((PerAreaStaList != null && PerAreaStaList.Count != 0) && Utils.GetInt(Request.QueryString["isExport"], 0) == 1)
            {
                ToExcel(this.crp_PrintPerAreaStaList, PerAreaStaList);
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
        private void InitPerAreaStaticlist()
        {
            #region 获取参数
            DepartmentName = Utils.GetQueryStringValue("DepartName");
            DepartId = Utils.GetQueryStringValue("DepartId");
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            OrderStarTime = Utils.GetDateTimeNullable(Request.QueryString["StartTime"]);
            OrderEndTime = Utils.GetDateTimeNullable(Request.QueryString["EndTime"]);
            Salser = Utils.GetQueryStringValue("Salser");
            SalserID = Utils.GetQueryStringValue("SalserId");
            #endregion

            #region 查询Model赋值
            EyouSoft.BLL.StatisticStructure.InayatStatistic InayaStaBLL = new EyouSoft.BLL.StatisticStructure.InayatStatistic(this.SiteUserInfo);
            EyouSoft.Model.StatisticStructure.QueryInayatStatistic QueryModel = new EyouSoft.Model.StatisticStructure.QueryInayatStatistic();//查询实体
            QueryModel.OrderIndex = 0;
            #region 按出团时间查询 by txb 2011.6.20
            QueryModel.LeaveDateStart = OrderStarTime;
            QueryModel.LeaveDateEnd = OrderEndTime;
            #endregion
            QueryModel.CompanyId = this.SiteUserInfo.CompanyID;//当前用户公司ID
            //设置查询model的ComputeOrderType值
            EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? computerOrderType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(SiteUserInfo.CompanyID);
            if (computerOrderType.HasValue)
            {
                QueryModel.ComputeOrderType =computerOrderType.Value;
            }
            if (SalserID != "")
            {
                QueryModel.SaleIds = Utils.GetIntArray(SalserID, ",");
            }
            if (DepartId != "")
            {
                QueryModel.DepartIds = Utils.GetIntArray(DepartId, ",");
            }

            QueryModel.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType?)Utils.GetEnumValue(typeof(EyouSoft.Model.EnumType.TourStructure.TourType), Utils.GetQueryStringValue("tourtype"), null);
            #endregion

            #region 初始化表单
            this.txtAreaStarTime.Value = OrderStarTime.HasValue ? OrderStarTime.Value.ToString("yyyy-MM-dd") : "";
            this.txtAreaEndTime.Value = OrderEndTime.HasValue ? OrderEndTime.Value.ToString("yyyy-MM-dd") : "";
            this.UCselectDepart.GetDepartmentName = DepartmentName;
            this.UCselectDepart.GetDepartId = DepartId;
            this.selectSals.OperName = Salser;
            this.selectSals.OperId = SalserID;
            #endregion

            #region 获取列表数据

            PerAreaStaList = InayaStaBLL.GetInayaAreatStatistic(QueryModel);
            //计算合计人数
            SumAllPeopleNum(PerAreaStaList);
            PerAreaStaList = EyouSoft.Common.Function.SelfExportPage.GetList<EyouSoft.Model.StatisticStructure.InayaAreatStatistic>(PageIndex, PageSize, out RecordCount, PerAreaStaList);
            if (Utils.GetInt(Request.QueryString["IsCartogram"], 0) == 1)
                return;
            if (PerAreaStaList != null && PerAreaStaList.Count != 0)
            {
                this.tbl_ExportPage.Visible = true;
                this.crp_PerAreaStaList.DataSource = PerAreaStaList;
                this.crp_PerAreaStaList.DataBind();
                BindPage();//绑定分页
            }
            else
            {
                this.tbl_ExportPage.Visible = false;
                this.crp_PerAreaStaList.EmptyText = "<tr bgcolor='#e3f1fc'><td id=\"EmptyData\" colspan='5' height='50px' align='center'>暂时没有数据！</td></tr>";
            }
            #endregion

            #region 释放资源
            InayaStaBLL = null;
            QueryModel = null;
            #endregion
        }


        #endregion

        #region 获取计调员
        public string GetAccouter(IList<StatisticOperator> list)
        {
            if (list != null && list.Count > 0)
                return Utils.GetListConverToStr(list);
            else
                return "";
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
        private void ToExcel(System.Web.UI.WebControls.Repeater ctl, IList<EyouSoft.Model.StatisticStructure.InayaAreatStatistic> PerAreaList)
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

        #region 线路区域统计图Flash
        private  void GetCartogramFlashXml()
        {
            #region 构造XML数据
            StringBuilder strXml = new StringBuilder();
            if (PerAreaStaList != null && PerAreaStaList.Count > 0)
            {
                strXml.Append(@"<graph formatNumber='0' formatNumberScale='0' decimalPrecision='0'  xAxisName='线路区域' font-size='20px' yAxisName='人'  yaxisminvalue='0' yaxismaxvalue='0' limitsDecimalPrecision='0' divLineDecimalPrecision='0'>");
                StringBuilder strCategory = new StringBuilder("<categories>");
                StringBuilder strDataSet = new StringBuilder("<dataset seriesname='人数' color='#AFD8F8' showValue='1'>");

                for (int i = 0; i < PerAreaStaList.Count; i++)
                {
                    if (PerAreaStaList[i].AreaName!=null && !string.IsNullOrEmpty(PerAreaStaList[i].AreaName))
                    {
                        //线路区域
                        strCategory.Append(@"<category name='" + PerAreaStaList[i].AreaName.ToString() + "' hoverText='" + PerAreaStaList[i].AreaName.ToString() + "'/>");
                        //人数
                        strDataSet.Append(@"<set value='" + int.Parse(PerAreaStaList[i].PeopleCount.ToString()) + "' />");
                    }                   
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
        private void SumAllPeopleNum(IList<InayaAreatStatistic> list)
        {
            if (list == null || list.Count <= 0) return;
            
            foreach (var inayaAreatStatistic in list)
            {
                if (inayaAreatStatistic == null) continue;

                AllPopleNum += inayaAreatStatistic.PeopleCount;
                TuanDuiShuHeJi += inayaAreatStatistic.TuanDuiShu;
            }
        }

        #endregion
    }
}
