using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using System.IO;
using EyouSoft.Common;
using Common.Enum;

namespace Web.StatisticAnalysis.CashFlow
{
    /// <summary>
    /// 页面功能：现金流量表--按月统计
    /// Author:liuym
    /// Date:2011-01-21
    /// </summary>
    public partial class CasMonthStatisticList : BackPage
    {
        #region Public Members
        IList<EyouSoft.Model.StatisticStructure.CompanyCash> list = null;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 权限验证
            if (!CheckGrant(TravelPermission.统计分析_现金流量_现金流量栏目))
            {
                Utils.ResponseNoPermit(TravelPermission.统计分析_现金流量_现金流量栏目, true);
                return;
            }
            #endregion

            if (!IsPostBack)
            {
                GetCashDayStaList();
            }
            if (Utils.GetQueryStringValue("isExport") == "1")
            {
                if (list != null && list.Count != 0)
                {
                    ToExcel(this.crp_PrintGetCashMonthStaList, list);
                }
                else
                {
                    Page.RegisterStartupScript("提示", Utils.ShowMsg("暂无数据,无法执行导出！"));
                }
            }
        }

        #region 现金流量表统计
        private void GetCashDayStaList()
        {
            EyouSoft.BLL.StatisticStructure.CompanyCash CashBLL = new EyouSoft.BLL.StatisticStructure.CompanyCash(this.SiteUserInfo);
            list = CashBLL.GetListGroupByMonth(this.SiteUserInfo.CompanyID);
            if (list != null && list.Count != 0)
            {
                //this.tbl_ExportPage.Visible = true;
                this.crp_GetCashMonthStaList.DataSource = list;
                this.crp_GetCashMonthStaList.DataBind();
            }
            else
            {
                //this.tbl_ExportPage.Visible = false;
                this.crp_GetCashMonthStaList.EmptyText = "<tr bgColor='#e3f1fc'><td colspan='5' id=\"EmptyData\" height='50px' align='center'>暂时没有数据！</td></tr>";
            }
            //释放资源
            CashBLL = null;
        }
        #endregion


        #region 导出Excel
        private void ToExcel(System.Web.UI.WebControls.Repeater ctl, IList<EyouSoft.Model.StatisticStructure.CompanyCash> PerAreaList)
        {
            ctl.Visible = true;
            ctl.DataSource = PerAreaList;
            ctl.DataBind();

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=CashMonthSta.xls");

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
    }
}
