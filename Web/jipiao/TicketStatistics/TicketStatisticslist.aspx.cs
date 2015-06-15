using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.jipiao.TicketStatistics
{
    /// <summary>
    /// 出票统计 按售票处统计
    /// 李晓欢 2011.3.28
    /// </summary>
    public partial class TicketStatisticslist : Eyousoft.Common.Page.BackPage
    {
        #region Private Mebers
        protected int PageSize = 15;  //每页显示的记录
        protected int PageIndex = 1;  //页码
        protected int RecordCount = 0; //总记录数
        protected int CompanyID = 1;//当前登录的公司编号
        protected int lenght = 0; //列表count
        #endregion

        #region 查询条件定义

        protected int AirlinesValue = 0;
        protected DateTime? Datetime;
        protected DateTime? ticketendtime;
        private DateTime? LeaDateS;
        private DateTime? LeaDateE;

        #endregion

        protected EyouSoft.BLL.StatisticStructure.TicketOutStatistic TicketBll = null;
        EyouSoft.Model.StatisticStructure.TicketOutStatisticOffice ticketmodel = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            ticketmodel = new EyouSoft.Model.StatisticStructure.TicketOutStatisticOffice();
            TicketBll = new EyouSoft.BLL.StatisticStructure.TicketOutStatistic(SiteUserInfo);

            //权限判断
            if (!CheckGrant(global::Common.Enum.TravelPermission.机票管理_出票统计_栏目))
            {
                EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.机票管理_出票统计_栏目, false);
                return;
            }
            if (!IsPostBack)
            {
                BindAirlines();
                BindList();
                if (EyouSoft.Common.Utils.GetQueryStringValue("action") == "toexcel")
                {
                    CreateExcel("TicketOutNum" + DateTime.Now.ToShortDateString());
                }
            }
        }

        #region 绑定航空公司
        protected void BindAirlines()
        {
            this.Airlineslist.Items.Clear();
            List<EyouSoft.Common.EnumObj> Airlines = EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.PlanStructure.FlightCompany));
            Airlineslist.DataSource = Airlines;
            Airlineslist.DataValueField = "Value";
            Airlineslist.DataTextField = "Text";
            Airlineslist.DataBind();
            this.Airlineslist.Items.Insert(0, new ListItem("--请选择航空公司--", "-1"));
        }
        #endregion

        protected void BindList()
        {
            //分页
            PageIndex = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("Page"), 1);
            AirlinesValue = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("AirlinesValue"));
            string DepartMent = EyouSoft.Common.Utils.GetQueryStringValue("DepartMents");
            string DepartId = EyouSoft.Common.Utils.GetQueryStringValue("DepartIds");
            Datetime = EyouSoft.Common.Utils.GetDateTimeNullable(EyouSoft.Common.Utils.GetQueryStringValue("DateTime"));
            ticketendtime = EyouSoft.Common.Utils.GetDateTimeNullable(EyouSoft.Common.Utils.GetQueryStringValue("endtime"));
            LeaDateS = EyouSoft.Common.Utils.GetDateTimeNullable(EyouSoft.Common.Utils.GetQueryStringValue("leaDateS"));
            LeaDateE = EyouSoft.Common.Utils.GetDateTimeNullable(EyouSoft.Common.Utils.GetQueryStringValue("leaDateE"));
            EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti QueryTicket = new EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti();
            QueryTicket.CompanyId = SiteUserInfo.CompanyID;
            if (AirlinesValue > 0)
            {
                QueryTicket.AirLineIds = new int[1] { AirlinesValue };
                if (this.Airlineslist.Items.FindByValue(AirlinesValue.ToString()) != null)
                {
                    this.Airlineslist.Items.FindByValue(AirlinesValue.ToString()).Selected = true;
                }
            }
            QueryTicket.DepartIds = JiPiao_TuiList.GetIntArrByStr(DepartId);
            QueryTicket.EndTicketOutTime = ticketendtime;
            QueryTicket.StartTicketOutTime = Datetime;
            QueryTicket.LeaveDateStart = LeaDateS;
            QueryTicket.LeaveDateEnd = LeaDateE;
            IList<EyouSoft.Model.StatisticStructure.TicketOutStatisticOffice> ticketlist = TicketBll.GetTicketOutStatisticOffice(QueryTicket);
            if (ticketlist != null && ticketlist.Count > 0)
            {
                this.prtticketlist.DataSource = EyouSoft.Common.Function.SelfExportPage.GetList<EyouSoft.Model.StatisticStructure.TicketOutStatisticOffice>(PageIndex, PageSize, out RecordCount, ticketlist);
                this.prtticketlist.DataBind();
                BindPage();
                lenght = ticketlist.Count;

                #region 设置总计
                //总票数
                this.lblAllTickets.Text = ticketlist.Sum(p => p.TicketOutNum).ToString();
                //应付机票款 
                this.lblNeedMoney.Text = ticketlist.Sum(p => p.TotalAmount).ToString("c2");
                //已付机票款 
                this.lblOverMoney.Text = ticketlist.Sum(p => p.PayAmount).ToString("c2");
                //未付机票款
                this.lblNoMoney.Text = ticketlist.Sum(p => p.UnPaidAmount).ToString("c2");
                #endregion

                this.lblMsg.Visible = false;
            }
            else
            {
                this.lblMsg.Visible = true;
                this.ExportPageInfo1.Visible = false;
            }

            #region 设置查询值

            UCselectDepart.GetDepartmentName = DepartMent;
            UCselectDepart.GetDepartId = DepartId;
            if (QueryTicket.LeaveDateStart.HasValue)
                txtLeaveDateStart.Text = QueryTicket.LeaveDateStart.Value.ToShortDateString();
            if (QueryTicket.LeaveDateEnd.HasValue)
                txtLeaveDateEnd.Text = QueryTicket.LeaveDateEnd.Value.ToShortDateString();

            #endregion

        }

        #region 导出Excel
        /// <summary>
        /// 导出Excel
        /// </summary>
        public void CreateExcel(string FileName)
        {
            AirlinesValue = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("AirlinesValue"));
            string DepartId = EyouSoft.Common.Utils.GetQueryStringValue("DepartIds");
            Datetime = EyouSoft.Common.Utils.GetDateTimeNullable(EyouSoft.Common.Utils.GetQueryStringValue("DateTime"));
            ticketendtime = EyouSoft.Common.Utils.GetDateTimeNullable(EyouSoft.Common.Utils.GetQueryStringValue("endtime"));
            LeaDateS = EyouSoft.Common.Utils.GetDateTimeNullable(EyouSoft.Common.Utils.GetQueryStringValue("leaDateS"));
            LeaDateE = EyouSoft.Common.Utils.GetDateTimeNullable(EyouSoft.Common.Utils.GetQueryStringValue("leaDateE"));

            //列表数据绑定
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";

            EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti QueryTicket = new EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti();
            QueryTicket.CompanyId = SiteUserInfo.CompanyID;
            if (AirlinesValue > 0)
                QueryTicket.AirLineIds = new int[1] { AirlinesValue };
            QueryTicket.DepartIds = JiPiao_TuiList.GetIntArrByStr(DepartId);
            QueryTicket.EndTicketOutTime = ticketendtime;
            QueryTicket.StartTicketOutTime = Datetime;
            QueryTicket.LeaveDateStart = LeaDateS;
            QueryTicket.LeaveDateEnd = LeaDateE;
            IList<EyouSoft.Model.StatisticStructure.TicketOutStatisticOffice> ticketlist = ticketlist = TicketBll.GetTicketOutStatisticOffice(QueryTicket);
            if (ticketlist != null && ticketlist.Count > 0)
            {
                //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\n", "售票处", "出票量", "应付机票款", "已付机票款", "未付机票款");
                foreach (EyouSoft.Model.StatisticStructure.TicketOutStatisticOffice sh in ticketlist)
                {
                    sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\n", sh.OfficeName, sh.TicketOutNum, "￥" + sh.TotalAmount, "￥" + sh.PayAmount, "￥" + sh.UnPaidAmount);
                }
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\n", "总计", ticketlist.Sum(p => p.TicketOutNum).ToString(), "￥" + ticketlist.Sum(p => p.TotalAmount).ToString(), "￥" + ticketlist.Sum(p => p.PayAmount).ToString(), "￥" + ticketlist.Sum(p => p.UnPaidAmount).ToString());
                Response.Write(sb.ToString());
                Response.End();
            }
           
        }
        #endregion


        #region 绑定分页
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
