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
using System.Drawing;
namespace Web.StatisticAnalysis.OutlayAccount
{
    /// <summary>
    /// 页面功能：支出对账单--部门统计
    /// </summary>
    /// 刘咏梅 2011-01-14
    public partial class OutDepartmentStatisticList : BackPage
    {
        #region Private Members
        protected int PageSize = 20;
        protected int PageIndex = 1;
        protected int RecordCount = 0;
        protected string TourType = string.Empty;//团队类型
        protected string OperatorName = string.Empty;//操作员
        protected string OperatorId = string.Empty;//操作员ID
        protected string RouteAreaName = string.Empty;//线路区域名称
        protected DateTime? StartDate = null;//出团开始日期
        protected DateTime? EndDate = null;//出团结束日期
        protected string CompanyName = string.Empty;    //供应商
        protected string isAccount = string.Empty;  //是否结清
        protected bool res = false;                 //标示总计的价格是否有超链接
        IList<EyouSoft.Model.StatisticStructure.StatAllOutList> list = null;
        #endregion

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SelectOperator.Title = "操作员：";

            #region 权限验证
            if (!CheckGrant(TravelPermission.统计分析_支出对账单_支出对账单栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.统计分析_支出对账单_支出对账单栏目, true);
                return;
            }
            #endregion

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
                InitOutDepartStaticlist();
            }
            #endregion

            #region 导出报表请求
            if (Utils.GetInt(Request.QueryString["isExport"], 0) == 1)
            {
                if (list != null && list.Count != 0)
                {
                    ToExcel(this.crp_PrintOutDepartList, list);
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
        private void InitOutDepartStaticlist()
        {
            #region 获取表单值
            PageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            RouteAreaName = Utils.GetQueryStringValue("RouteArea");
            OperatorName = Utils.GetQueryStringValue("OperatorName");
            OperatorId = Utils.GetQueryStringValue("OperatorId");
            StartDate = Utils.GetDateTimeNullable(Request.QueryString["StartDate"]);
            EndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("EndDate"));
            TourType = Utils.GetQueryStringValue("TourType");
            CompanyName = Utils.GetQueryStringValue("CompanyName");
            isAccount = Utils.GetQueryStringValue("IsAccount");
            #endregion

            EyouSoft.BLL.StatisticStructure.StatAllOut OutlayBll = new EyouSoft.BLL.StatisticStructure.StatAllOut(SiteUserInfo);

            #region 查询实体赋值
            EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic SearchModel = new EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic();
            SearchModel.LeaveDateStart = StartDate;
            SearchModel.LeaveDateEnd = EndDate;
            SearchModel.CompanyId = this.SiteUserInfo.CompanyID;
            SearchModel.SaleIds = Utils.GetIntArray(OperatorId, ",");
            if (RouteAreaName != "" && RouteAreaName != "0")
            {
                SearchModel.AreaId = int.Parse(RouteAreaName);
            }

            if (TourType != "" && TourType != "-1")
            {
                SearchModel.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)int.Parse(TourType);
            }
            SearchModel.BuyCompanyName = CompanyName;
            #endregion

            #region 表单初始化
            this.txtStartDate.Value = StartDate.HasValue ? StartDate.Value.ToString("yyyy-MM-dd") : new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy-MM-dd");
            this.txtEndDate.Value = EndDate.HasValue ? EndDate.Value.ToString("yyyy-MM-dd") : new DateTime(DateTime.Now.Year, 12, 31).ToString("yyyy-MM-dd");
            if (TourType != "")
            {
                this.TourTypeList1.TourType = int.Parse(TourType);
            }
            if (RouteAreaName != "")
            {
                this.RouteAreaList1.RouteAreaId = int.Parse(RouteAreaName);
            }
            this.SelectOperator.OperName = OperatorName;
            this.SelectOperator.OperId = OperatorId;
            this.txt_com.Value = CompanyName;
            if (isAccount == "-1")
            {
                SearchModel.IsAccount = null;
            }
            else if (isAccount == "0")
            {
                SearchModel.IsAccount = true;
            }
            else if (isAccount == "1")
            {
                SearchModel.IsAccount = false;
            }
            BindSum(SearchModel);
            if (isAccount != "")
            {
                this.drpIsAccount.SelectedValue = isAccount;
            }
            #endregion

            //调用查询方法 获取列表
            list = OutlayBll.GetList(PageSize, PageIndex, ref RecordCount, SearchModel);
            if (Utils.GetInt(Request.QueryString["IsCartogram"], 0) == 1)//异步请求统计图时不执行列表绑定
                return;

            #region 绑定列表
            if (list != null && list.Count != 0)
            {
                this.tbl_ExportPage.Visible = true;
                this.crp_OutDepartStatiList.DataSource = list;
                this.crp_OutDepartStatiList.DataBind();
                BindPage();//绑定分页
            }
            else
            {
                this.tbl_ExportPage.Visible = false;
                this.crp_OutDepartStatiList.EmptyText = "<tr bgcolor='#e3f1fc'><td colspan='4' id=\"EmptyData\" height='50px' align='center'>暂时没有数据！</td></tr>";
            }
            #endregion
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
        private void ToExcel(System.Web.UI.WebControls.Repeater ctl, IList<EyouSoft.Model.StatisticStructure.StatAllOutList> PerAreaList)
        {
            ctl.Visible = true;
            ctl.DataSource = PerAreaList;
            ctl.DataBind();

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=IncAreaStaticFile.xls");

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

        #region 异步生成统计图Flash的XML
        private void GetCartogramFlashXml()
        {
            #region 构造XML数据
            StringBuilder strXml = new StringBuilder();
            if (list != null && list.Count != 0)
            {
                strXml.AppendFormat(@"<graph formatNumber='0' formatNumberScale='0' decimalPrecision='0'  xAxisName='操作员' yAxisName=''  yaxisminvalue='' yaxismaxvalue='' limitsDecimalPrecision='0' divLineDecimalPrecision=''>");
                StringBuilder strCategory = new StringBuilder("<categories>");
                StringBuilder strDataSet = new StringBuilder("<dataset seriesname='总支出' color='#AFD8F8' showValue='1'>");
                StringBuilder strDataSet1 = new StringBuilder("<dataset seriesname='已付' color='#FF8E46' showValue='1'>");
                StringBuilder strDataSet2 = new StringBuilder("<dataset seriesname='未付' color='#87CEFA' showValue='1'>");
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].OperatorName == null || string.IsNullOrEmpty(list[i].OperatorName.ToString()))
                        continue;
                    strCategory.AppendFormat(@"<category name='" + list[i].OperatorName.ToString() + "' hoverText='" + list[i].OperatorName.ToString() + "'/>");
                    //总支出
                    strDataSet.AppendFormat(@"<set value='{0}' />", Utils.GetDecimal(list[i].TotalAmount.ToString()));
                    //已付
                    strDataSet1.AppendFormat(@"<set value='{0}' />", Utils.GetDecimal(list[i].PaidAmount.ToString()));
                    //未付
                    strDataSet2.AppendFormat(@"<set value='{0}' link='{1}' />", Utils.GetDecimal(list[i].NotPaidAmount.ToString()), "javascript:GetUnpayList(" + list[i].OperatorId + ");");

                }
                strCategory.Append("</categories>");
                strDataSet.Append("</dataset>");
                strDataSet1.Append("</dataset>");
                strDataSet2.Append("</dataset>");
                strXml.Append(strCategory.ToString());
                strXml.Append(strDataSet.ToString());
                strXml.Append(strDataSet1.ToString());
                strXml.Append(strDataSet2.ToString());
                strXml.Append(@"</graph>");
            }
            #endregion

            Response.Clear();
            Response.Write(strXml.ToString());
            Response.End();
        }
        #endregion

        #region 绑定总计数据
        protected void BindSum(EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic model)
        {
            Decimal total = 0; //总收入
            Decimal ys = 0;    //应收
            Decimal ws = 0;    //未收
            //BLL声明
            EyouSoft.BLL.StatisticStructure.StatAllOut outBLL = new EyouSoft.BLL.StatisticStructure.StatAllOut(SiteUserInfo);
            //获取总计数据
            outBLL.GetAllTotalAmount(model, ref total, ref ys);
            if (total != 0)
            {
                res = true;
            }
            //计算未收金额
            ws = total - ys;
            //总金额赋值_已结清
            this.sum_total_account.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(total.ToString()).ToString("0.00"));
            //总金额赋值_所有
            this.sum_total_noaccount.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(total.ToString()).ToString("0.00"));
            //应收金额赋值_已结清
            this.sum_ys_account.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(ys.ToString()).ToString("0.00"));
            //应收金额赋值_所有
            this.sum_ys_noaccount.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(ys.ToString()).ToString("0.00"));
            //未收金额赋值_已结清
            this.sum_ws_account.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(ws.ToString()).ToString("0.00"));
            //未收金额赋值_所有
            this.sum_ws_noaccount.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(ws.ToString()).ToString("0.00"));
        }
        #endregion
    }
}
