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
using Common.Enum;
using System.Drawing;
namespace Web.StatisticAnalysis.IncomeAccount
{
    /// <summary>
    /// 页面功能：统计分析-收入对账单--部门统计
    /// Author:liuym
    /// Date:2011-1-19
    /// </summary>
    public partial class IncDepartmentStatisticList : BackPage
    {
        #region Private Members
        protected int PageSize = 20;
        protected int PageIndex = 1;
        protected int RecordCount = 0;
        protected string TourType = string.Empty;//团队类型
        protected string RouteArea = string.Empty;//线路区域
        protected string Salser = string.Empty;//销售员    
        protected string SalserID = string.Empty;//销售员ID
        protected DateTime? LeaveTourStarDate = null;//出团开始日期
        protected DateTime? LeaveTourEndDate = null;//出团结束日期
        protected string CompanyName = string.Empty;//单位
        protected string isAccount = string.Empty;  //是否结清
        protected bool res = false;                 //标示价格是否有超链接
        IList<EyouSoft.Model.StatisticStructure.StatAllIncomeList> IncomeDepartList = null;//收入对账部门统计列表
        #endregion

        #region 页面初始化事件
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 权限验证
            if (!CheckGrant(TravelPermission.统计分析_收入对账单_收入对账单栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.统计分析_收入对账单_收入对账单栏目, true);
                return;
            }
            #endregion

            #region 根据URL参数初始化PageSize
            if (Utils.GetInt(Request.QueryString["isAll"], 0) == 1
                || Utils.GetInt(Request.QueryString["isExport"], 0) == 1
                || Utils.GetInt(Request.QueryString["IsCartogram"], 0) == 1)
            {
                PageSize = int.MaxValue - 1;
            }
            #endregion

            #region 初始化数据
            if (!IsPostBack)
            {
                InitIncDepartAreaStaticlist();
            }
            #endregion

            #region 导出报表请求
            if (Utils.GetInt(Request.QueryString["isExport"], 0) == 1)
            {
                if (IncomeDepartList != null && IncomeDepartList.Count != 0)
                {
                    ToExcel(this.crp_PrintIncDepartStaList, IncomeDepartList);
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

        #region  初始化统计数据源
        private void InitIncDepartAreaStaticlist()
        {
            #region 获取参数
            PageIndex = Utils.GetInt(Request.QueryString["Page"], 1);
            TourType = Utils.GetQueryStringValue("TourType");
            RouteArea = Utils.GetQueryStringValue("RouteArea");
            Salser = Utils.GetQueryStringValue("Salser");
            SalserID = Utils.GetQueryStringValue("SalserId");
            LeaveTourStarDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveTourStartDate"));
            LeaveTourEndDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("LeaveTourEndDate"));
            CompanyName = Utils.GetQueryStringValue("CompanyName");
            isAccount = Utils.GetQueryStringValue("IsAccount");

            #endregion

            //私有变量
            EyouSoft.BLL.StatisticStructure.StatAllIncome IncomeBLL = new EyouSoft.BLL.StatisticStructure.StatAllIncome(SiteUserInfo);

            #region 实体赋值
            EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic model = new EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic();
            model.CompanyId = this.SiteUserInfo.CompanyID;
            model.SaleIds = Utils.GetIntArray(SalserID, ",");
            if (TourType != "" && TourType != "-1")
            {
                model.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)int.Parse(TourType);
            }
            if (RouteArea != "" && RouteArea != "0")
            {
                model.AreaId = int.Parse(RouteArea);
            }
            //设置查询model的ComputeOrderType值
            EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? computerOrderType = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetComputeOrderType(SiteUserInfo.CompanyID);
            if (computerOrderType.HasValue)
            {
                model.ComputeOrderType = computerOrderType.Value;
            }
            model.LeaveDateStart = LeaveTourStarDate;
            model.LeaveDateEnd = LeaveTourEndDate;
            model.BuyCompanyName = CompanyName;
            if (isAccount == "-1")
            {
                model.IsAccount = null;
            }
            else if (isAccount == "0")
            {
                model.IsAccount = true;
            }
            else if (isAccount == "1")
            {
                model.IsAccount = false;
            }
            BindSum(model);
            #endregion

            #region 表单初始化
            if (TourType != "")
            {
                this.TourTypeList1.TourType = int.Parse(TourType);
            }
            if (RouteArea != "")
            {
                this.RouteAreaList1.RouteAreaId = int.Parse(RouteArea);
            }
            this.SelectSalser.OperName = Salser;
            this.SelectSalser.OperId = SalserID;
            this.txtLeaveTourStarTime.Value = LeaveTourStarDate.HasValue ? LeaveTourStarDate.Value.ToString("yyyy-MM-dd") : "";
            this.txtLeaveTourEndTime.Value = LeaveTourEndDate.HasValue ? LeaveTourEndDate.Value.ToString("yyyy-MM-dd") : "";
            this.txt_com.Value = CompanyName;
            if (isAccount != "")
            {
                this.drpIsAccount.SelectedValue = isAccount;
            }

            #endregion

            //调用底层方法
            IncomeDepartList = IncomeBLL.GetList(PageSize, PageIndex, ref RecordCount, model);
            //如果为统计图异步请求时，不执行列表绑定
            if (Utils.GetInt(Request.QueryString["IsCartogram"], 0) == 1)
                return;

            #region 列表绑定
            if (IncomeDepartList != null && IncomeDepartList.Count != 0)
            {
                this.tbl_ExportPage.Visible = true;
                this.crp_IncDepartStaList.DataSource = IncomeDepartList;
                this.crp_IncDepartStaList.DataBind();
                BindPage();//绑定分页
            }
            else
            {
                this.tbl_ExportPage.Visible = false;
                this.crp_IncDepartStaList.EmptyText = "<tr bgcolor='#e3f1fc'><td colspan='4' id=\"EmptyData\" height='50px' align='center'>暂时没有数据！</td></tr>";
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
        private void ToExcel(System.Web.UI.WebControls.Repeater ctl, IList<EyouSoft.Model.StatisticStructure.StatAllIncomeList> IncomeDepartList)
        {
            ctl.Visible = true;
            ctl.DataSource = IncomeDepartList;
            ctl.DataBind();

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=IncAreaStaticFile.xls");

            HttpContext.Current.Response.Charset = "UTF-8";

            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;

            HttpContext.Current.Response.ContentType = "application/ms-excel";

            ctl.Page.EnableViewState = false;
            StringWriter sw = new StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
            ctl.RenderControl(hw);
            ctl.Page.EnableViewState = true;
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
            if (IncomeDepartList != null && IncomeDepartList.Count != 0)
            {
                strXml.Append(@"<graph formatNumber='0' formatNumberScale='0' decimalPrecision='0'  xAxisName='销售员' font-size='20px' yAxisName='' canvasBgColor='F6DFD9' canvasBaseColor='FE6E54' hovercapbgColor='FFECAA' hovercapborder='F47E00' divlinecolor='F47E00' yaxisminvalue='0' yaxismaxvalue='0' limitsDecimalPrecision='0' divLineDecimalPrecision='0'>");
                StringBuilder strCategory = new StringBuilder("<categories>");
                StringBuilder strDataSet = new StringBuilder("<dataset seriesname='总收入' color='F0807F' showValue='1'>");
                StringBuilder strDataSet1 = new StringBuilder("<dataset seriesname='已收' color='B22222' showValue='1'>");
                StringBuilder strDataSet2 = new StringBuilder("<dataset seriesname='未收' color='0080FF' showValue='1'>");

                for (int i = 0; i < IncomeDepartList.Count; i++)
                {
                    if (IncomeDepartList[i].OperatorName == null || string.IsNullOrEmpty(IncomeDepartList[i].OperatorName.ToString()))
                        continue;
                    strCategory.Append(@"<category name='" + IncomeDepartList[i].OperatorName.ToString() + "' hoverText='" + IncomeDepartList[i].OperatorName.ToString() + "'/>");
                    //总收入
                    strDataSet.Append(@"<set value='" + Utils.GetDecimal(IncomeDepartList[i].TotalAmount.ToString()) + "' />");
                    //已收
                    strDataSet1.Append(@"<set value='" + Utils.GetDecimal(IncomeDepartList[i].AccountAmount.ToString()) + "' />");
                    //未收
                    strDataSet2.AppendFormat(@"<set value='{0}' link='{1}' />", Utils.GetDecimal(IncomeDepartList[i].NotAccountAmount.ToString()), "javascript:GetUnCollectIncomeList(" + IncomeDepartList[i].OperatorId + ");");//未收入
                }
                strCategory.Append("</categories>");
                strDataSet.Append("</dataset>");
                strDataSet1.Append("</dataset>");
                strDataSet2.Append("</dataset>");
                strXml.Append(strCategory.ToString());
                strXml.Append(strDataSet.ToString());
                strXml.Append(strDataSet1.ToString());
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
            EyouSoft.BLL.StatisticStructure.StatAllIncome IncomeBLL = new EyouSoft.BLL.StatisticStructure.StatAllIncome(SiteUserInfo);
            //获取总计数据
            IncomeBLL.GetAllTotalAmount(model, ref total, ref ys);
            if (total != 0)
            {
                res = true;
            }
            //计算未收金额
            ws = total - ys;
            //总金额赋值
            this.sum_total.Text = "￥" + EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(total.ToString()).ToString("0.00"));
            //应收金额赋值
            this.sum_ys.Text = "￥" + EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(ys.ToString()).ToString("0.00"));
            //未收金额赋值_已结清
            this.sum_ws_account.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(ws.ToString()).ToString("0.00"));
            //未收金额赋值_所有
            this.sum_ws_noaccount.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(ws.ToString()).ToString("0.00"));
            //设置字体颜色
            this.sum_total.ForeColor = Color.Red;
            //设置字体颜色
            this.sum_ys.ForeColor = Color.Red;
        }
        #endregion
    }
}
