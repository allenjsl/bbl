using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using Common.Enum;

namespace Web.StatisticAnalysis.Ageanalysis
{
    /// <summary>
    /// 页面功能：账龄分析表--部门统计
    /// Author:liuym
    /// Date:2011-1-21
    /// </summary>
    public partial class AgeDepartmentStatisticList : BackPage
    {
        #region Private Members
        protected int PageIndex = 1;
        protected int PageSize = 20;
        protected int RecordCount = 0;
        protected string TourType = string.Empty;//团队类型
        protected string RouteAreaName = string.Empty;//线路区域
        protected string Salser = string.Empty;//销售员
        protected string SalerId = string.Empty;//销售员ID
        protected DateTime? StartDate = null;//出团开始日期
        protected DateTime? EndDate = null;//出团结束日期
        IList<EyouSoft.Model.StatisticStructure.AccountAgeStatistic> list = null;
        #endregion

        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SelectSalers.Title = "销售员：";

            #region 权限验证
            
            if (!CheckGrant(TravelPermission.统计分析_帐龄分析_帐龄分析栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.统计分析_帐龄分析_帐龄分析栏目, true);
                return;
            }

            #endregion

            #region 根据URL参数设置PageSize
            if (Utils.GetInt(Request.QueryString["isAll"],0) == 1)
            {
                PageSize = int.MaxValue-1;
            }
            #endregion

            if (!IsPostBack)
            {
                InitAgeAreaStaList();
            }

            #region 导出报表请求
            if (Utils.GetQueryStringValue("isExport") == "1")
            {
                if (list != null && list.Count != 0)
                {
                    ToExcel(this.crp_PrintGetAgeDepartStaList, list);
                }
                else
                {
                    Page.RegisterStartupScript("提示", Utils.ShowMsg("暂无数据,无法执行导出！"));
                }
            }
            #endregion

        }
        #endregion

        #region 初始化账龄分析表
        private void InitAgeAreaStaList()
        {
            #region 获取参数
            PageIndex = Utils.GetInt(Request.QueryString["Page"],1);
            TourType = Utils.GetQueryStringValue("tourType");
            RouteAreaName = Utils.GetQueryStringValue("routeAreaName");
            Salser = Utils.GetQueryStringValue("Salser");
            SalerId = Utils.GetQueryStringValue("SalserId");
            StartDate = Utils.GetDateTimeNullable(Request.QueryString["startDate"], new DateTime(DateTime.Now.Year, 1, 1));
            EndDate = Utils.GetDateTimeNullable(Request.QueryString["endDate"], new DateTime(DateTime.Now.Year, 12, 31));
            #endregion

            //调用底层方法 
            EyouSoft.BLL.StatisticStructure.AccountAgeStatistic AgeStaBll = new EyouSoft.BLL.StatisticStructure.AccountAgeStatistic(this.SiteUserInfo);
            EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic model = new EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic();
            
            #region 查询实体赋值
            if (TourType != "" && TourType != "-1")
            {
                model.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)int.Parse(TourType);
            }
            model.LeaveDateStart = StartDate;
            model.LeaveDateEnd = EndDate;
            if (RouteAreaName != "" && RouteAreaName != "0")
            {
                model.AreaId = int.Parse(RouteAreaName);
            }
            if (SalerId != "")
            {
                model.SaleIds = Utils.GetIntArray(SalerId, ",");
            }
            #endregion

            #region 初始化表单赋值   

            txtStartDate.Value = StartDate.HasValue ? StartDate.Value.ToString("yyyy-MM-dd") : "";
            txtEndDate.Value = EndDate.HasValue ? EndDate.Value.ToString("yyyy-MM-dd") : "";
            if(TourType!="")
            {
                this.TourTypeList1.TourType = int.Parse(TourType);
            }
            if(RouteAreaName!="")
            {
                this.RouteAreaList1.RouteAreaId = int.Parse(RouteAreaName);
            }
            this.SelectSalers.OperName = Salser;
            this.SelectSalers.OperId = SalerId;
            #endregion

            #region 绑定列表
            list = EyouSoft.Common.Function.SelfExportPage.GetList<EyouSoft.Model.StatisticStructure.AccountAgeStatistic>(PageIndex, PageSize, out RecordCount, AgeStaBll.GetAccountAgeStatistic(model));
            if (list != null && list.Count != 0)
            {
                this.tbl_ExportPage.Visible = true;
                this.crp_GetAgeDepartStaList.DataSource = list;
                this.crp_GetAgeDepartStaList.DataBind();
                BindPage();
            }
            else
            {
                this.tbl_ExportPage.Visible = false;
                this.crp_GetAgeDepartStaList.EmptyText = "<tr bgcolor='#e3f1fc'><td id=\"EmptyData\" colspan='3' height='50px' align='center'>暂时没有数据！</td></tr>";
            }
            #endregion

        }
      

        #endregion

        #region 计算拖欠时间
        protected string GetMaxDay(string Time)
        {
            string Resutl = string.Empty;   
            DateTime? Date = Utils.GetDateTimeNullable(Time);
            if (Date.HasValue)
            {
                TimeSpan ts = DateTime.Now - Date.Value;
                if (ts.Days > 0)
                    Resutl += ts.Days.ToString() + "天";
                if (ts.Hours > 0)
                    Resutl += ts.Hours.ToString() + "时";
                if(ts.Minutes > 0)
                    Resutl += ts.Minutes.ToString() + "分";
            }
            return Resutl;
        }
        #endregion

        #region 导出Excel
        private void ToExcel(System.Web.UI.WebControls.Repeater ctl, IList<EyouSoft.Model.StatisticStructure.AccountAgeStatistic> PerAreaList)
        {
            ctl.Visible = true;
            ctl.DataSource = PerAreaList;
            ctl.DataBind();

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=AgeDepartStaticDepart.xls");

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
    }
}
