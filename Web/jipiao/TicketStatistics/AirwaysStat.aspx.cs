using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Eyousoft.Common.Page;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

using Common.Enum;

namespace Web.jipiao.TicketStatistics
{
    /// <summary>
    /// 按航空公司统计
    /// by 田想兵 3.28
    /// </summary>
    public partial class AirwaysStat : BackPage
    {
        #region 分页变量
        protected int pageSize = 15;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion
        #region 统计
        protected string lblAllTickets = null;
        protected string lblNeedMoney = null;
        protected string lblOverMoney = null;
        protected string lblNoMoney = null;
        #endregion
        IList<EyouSoft.Model.StatisticStructure.TicketOutStatisticAirLine> list = null;//未收集合
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!CheckGrant(TravelPermission.机票管理_机票管理_出票统计))
                {
                    Utils.ResponseNoPermit(TravelPermission.机票管理_机票管理_出票统计, false);
                }
                Bind();
                #region 导出报表请求
                if (Utils.GetInt(Request.QueryString["isExport"], 0) == 1)
                {
                    var bll = new EyouSoft.BLL.StatisticStructure.TicketOutStatistic(SiteUserInfo);
                    var model = new EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti();
                    model.OfficeName = Utils.GetQueryStringValue("OfficeName");
                    model.CompanyId = SiteUserInfo.CompanyID;
                    model.DepartName = Utils.GetQueryStringValue("DepartMents");
                    string strDepIds = Utils.GetQueryStringValue("DepartIds");
                    model.DepartIds = JiPiao_TuiList.GetIntArrByStr(strDepIds);
                    model.StartTicketOutTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("DateTime"));
                    model.EndTicketOutTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("endtime"));
                    model.LeaveDateStart = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("leaDateS"));
                    model.LeaveDateEnd = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("leaDateE"));
                    list = bll.GetTicketOutStatisticAirLine(model);
                    if (list != null && list.Count != 0)
                    {
                        ToExcel(this.rpt_list, list);
                    }
                }
                #endregion
            }
        }

        #region 导出Excel
        private void ToExcel(ControlLibrary.CustomRepeater ctl, IList<EyouSoft.Model.StatisticStructure.TicketOutStatisticAirLine> PerAreaList)
        {
            ctl.Visible = true;
            ctl.DataSource = PerAreaList;
            ctl.DataBind();

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=airways" + DateTime.Now.ToShortDateString() + ".xls");

            HttpContext.Current.Response.Charset = "UTF-8";

            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;

            HttpContext.Current.Response.ContentType = "application/ms-excel";

            ctl.Page.EnableViewState = false;
            StringWriter sw = new StringWriter();

            sw.WriteLine("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
            sw.WriteLine("<head>");
            sw.WriteLine("<!--[if gte mso 9]>");
            sw.WriteLine("<xml>");
            sw.WriteLine(" <x:ExcelWorkbook>");
            sw.WriteLine("  <x:ExcelWorksheets>");
            sw.WriteLine("   <x:ExcelWorksheet>");
            sw.WriteLine("    <x:Name>按航空公司统计</x:Name>");
            sw.WriteLine("    <x:WorksheetOptions>");
            sw.WriteLine("      <x:Print>");
            sw.WriteLine("       <x:ValidPrinterInfo />");
            sw.WriteLine("      </x:Print>");
            sw.WriteLine("    </x:WorksheetOptions>");
            sw.WriteLine("   </x:ExcelWorksheet>");
            sw.WriteLine("  </x:ExcelWorksheets>");
            sw.WriteLine("</x:ExcelWorkbook>");
            sw.WriteLine("</xml>");
            sw.WriteLine("<![endif]-->");
            sw.WriteLine("</head>");
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
            ctl.RenderControl(hw);
            ctl.Visible = false;
            string str = sw.ToString();
            str = Regex.Replace(str, "</?(a|A)(.*?>|>)", " ");



            HttpContext.Current.Response.Write(str);
            HttpContext.Current.Response.End();
        }
        #endregion
        void Bind()
        {
            pageIndex = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);

            EyouSoft.BLL.StatisticStructure.TicketOutStatistic bll = new EyouSoft.BLL.StatisticStructure.TicketOutStatistic(SiteUserInfo);
            EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti model = new EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti();
            model.OfficeName = Utils.GetQueryStringValue("OfficeName");
            model.CompanyId = SiteUserInfo.CompanyID;
            model.DepartName = Utils.GetQueryStringValue("DepartMents");
            string strDepIds = Utils.GetQueryStringValue("DepartIds");
            model.DepartIds = JiPiao_TuiList.GetIntArrByStr(strDepIds);
            model.StartTicketOutTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("DateTime"));
            model.EndTicketOutTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("endtime"));
            model.LeaveDateStart = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("leaDateS"));
            model.LeaveDateEnd = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("leaDateE"));
            list = bll.GetTicketOutStatisticAirLine(model);
            if (list != null && list.Count > 0)
            {
                recordCount = list.Count;
                rpt_list.DataSource = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                rpt_list.DataBind();

                #region 设置总计
                //总票数
                this.lblAllTickets = list.Sum(p => p.TicketOutNum).ToString();
                //应付机票款 
                this.lblNeedMoney = list.Sum(p => p.TotalAmount).ToString("￥#,###0.00");
                //已付机票款 
                this.lblOverMoney = list.Sum(p => p.PayAmount).ToString("￥#,###0.00");
                //未付机票款
                this.lblNoMoney = list.Sum(p => p.UnPaidAmount).ToString("￥#,###0.00");
                #endregion


                #region 设置分页
                ExportPageInfo1.intPageSize = pageSize;
                ExportPageInfo1.intRecordCount = recordCount;
                ExportPageInfo1.PageLinkURL = Request.Path + "?";
                ExportPageInfo1.UrlParams = Request.QueryString;
                ExportPageInfo1.CurrencyPage =pageIndex;
                #endregion
            }
            else
            {
                //没有数据隐藏控件
                ExportPageInfo1.Visible = false;
                
            }

            UCselectDepart.GetDepartmentName = model.DepartName;
            UCselectDepart.GetDepartId = strDepIds;
            txt_date.Value = model.StartTicketOutTime.HasValue ? model.StartTicketOutTime.Value.ToString("yyyy-MM-dd") : "";
            txt_endDate.Value = model.EndTicketOutTime.HasValue ? model.EndTicketOutTime.Value.ToString("yyyy-MM-dd") : "";
            txt_spq.Value = model.OfficeName;
            txtLeaveDateStart.Text = model.LeaveDateStart.HasValue ? model.LeaveDateStart.Value.ToString("yyyy-MM-dd") : string.Empty;
            txtLeaveDateEnd.Text = model.LeaveDateEnd.HasValue ? model.LeaveDateEnd.Value.ToString("yyyy-MM-dd") : string.Empty;
        }
    }
}
