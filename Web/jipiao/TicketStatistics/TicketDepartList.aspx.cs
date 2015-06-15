using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;
using System.Text;

namespace Web.jipiao.TicketStatistics
{
    public partial class TicketDepartList : BackPage
    {
        #region 分页变量
        protected int pageSize = 15;
        protected int pageIndex = 1;
        protected int recordCount;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckGrant(global::Common.Enum.TravelPermission.机票管理_出票统计_栏目))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.机票管理_出票统计_栏目, true);

            }
            string cat = Utils.GetQueryStringValue("cat");

            if (!IsPostBack)
            {
                switch (cat)
                {
                    case "toexcel":

                        ToExcel("TicketDepartList" + DateTime.Now.ToShortDateString());
                        break;

                }
                AirDataBind(-1);
                Bind();
            }
        }

        //绑定数据
        protected void Bind()
        {
            pageIndex = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);

            EyouSoft.BLL.StatisticStructure.TicketOutStatistic ticketBll = new EyouSoft.BLL.StatisticStructure.TicketOutStatistic(SiteUserInfo);
            EyouSoft.Model.StatisticStructure.TicketOutStatisticDepart ticketDepart = new EyouSoft.Model.StatisticStructure.TicketOutStatisticDepart();
            EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti queryTicket = new EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti();
            int tmpAirLineId = Utils.GetInt(Utils.GetQueryStringValue("areaId"), -1);
            if (tmpAirLineId > 0)
            {
                queryTicket.AirLineIds = new int[1] { tmpAirLineId };
            }
            queryTicket.CompanyId = SiteUserInfo.CompanyID;
            queryTicket.OfficeName = Utils.GetQueryStringValue("OfficeName");
            queryTicket.StartTicketOutTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("DateTime"));
            queryTicket.EndTicketOutTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("endtime"));
            queryTicket.LeaveDateStart = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("leaDateS"));
            queryTicket.LeaveDateEnd = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("leaDateE"));
            IList<EyouSoft.Model.StatisticStructure.TicketOutStatisticDepart> list = ticketBll.GetTicketOutStatisticDepart(queryTicket);
            if (list != null && list.Count > 0)
            {
                recordCount = list.Count;
                retList.DataSource = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                this.retList.DataBind();
                BindPage();
                this.lblMsg.Visible = false;


                #region 设置总计
                //总票数
                this.lblAllTickets.Text = list.Sum(p => p.TicketOutNum).ToString();
                //应付机票款 
                this.lblNeedMoney.Text = list.Sum(p => p.TotalAmount).ToString("c2");
                //已付机票款 
                this.lblOverMoney.Text = list.Sum(p => p.PayAmount).ToString("c2");
                //未付机票款
                this.lblNoMoney.Text = list.Sum(p => p.UnPaidAmount).ToString("c2");
                #endregion


            }
            else
            {
                this.lblMsg.Visible = true;
                this.ExportPageInfo1.Visible = false;
            }
            this.ddlAirLineIds.SelectedValue = tmpAirLineId.ToString();
            if (queryTicket.StartTicketOutTime.HasValue)
                this.txt_date.Value = queryTicket.StartTicketOutTime.Value.ToShortDateString();
            if (queryTicket.EndTicketOutTime.HasValue)
                this.txt_endDate.Value = queryTicket.EndTicketOutTime.Value.ToShortDateString();
            if (queryTicket.LeaveDateStart.HasValue)
                txtLeaveDateStart.Text = queryTicket.LeaveDateStart.Value.ToShortDateString();
            if (queryTicket.LeaveDateEnd.HasValue)
                txtLeaveDateEnd.Text = queryTicket.LeaveDateEnd.Value.ToShortDateString();
            this.txt_spq.Value = queryTicket.OfficeName;

        }

        //导出
        protected void ToExcel(string title)
        {
            EyouSoft.BLL.StatisticStructure.TicketOutStatistic ticketBll = new EyouSoft.BLL.StatisticStructure.TicketOutStatistic(SiteUserInfo);
            IList<EyouSoft.Model.StatisticStructure.TicketOutStatisticDepart> list = null;
            EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti searchInfo = new EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti();
            int tmpAirLineId = Utils.GetInt(Utils.GetQueryStringValue("areaId"), -1);
            if (tmpAirLineId > 0)
            {
                searchInfo.AirLineIds = new int[1] { tmpAirLineId };
            }
            searchInfo.CompanyId = SiteUserInfo.CompanyID;
            searchInfo.OfficeName = Utils.GetQueryStringValue("OfficeName");
            searchInfo.StartTicketOutTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("DateTime"));
            searchInfo.EndTicketOutTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("endTime"));
            searchInfo.LeaveDateStart = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("leaDateS"));
            searchInfo.LeaveDateEnd = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("leaDateE"));
            //用gerList方法取得总记录的条数
            list = ticketBll.GetTicketOutStatisticDepart(searchInfo);
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + title + ".xls");
            Response.ContentEncoding = System.Text.Encoding.Default;
            Response.ContentType = "application/ms-excel";

            //取得数据表各列标题，各标题之间以\t分割，最后一个列标题后加回车符
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\n", "部门", "出票量", "应付机票款", "已付机票款", "未付机票款");
            foreach (EyouSoft.Model.StatisticStructure.TicketOutStatisticDepart ticket in list)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\n",
                   ticket.DepartName,
                    ticket.TicketOutNum,
                    "￥" + ticket.TotalAmount,
                    "￥" + ticket.PayAmount,
                    "￥" + ticket.UnPaidAmount);
            }
            sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\n", "总计", list.Sum(p => p.TicketOutNum).ToString(), list.Sum(p => p.TotalAmount).ToString("c2"), list.Sum(p => p.PayAmount).ToString("c2"), list.Sum(p => p.UnPaidAmount).ToString("c2"));
            Response.Write(sb.ToString());
            Response.End();
        }



        #region 绑定分页控件
        /// <summary>
        /// 绑定分页控件
        /// </summary>
        protected void BindPage()
        {
            ExportPageInfo1.intPageSize = pageSize;
            ExportPageInfo1.intRecordCount = recordCount;
            ExportPageInfo1.PageLinkURL = Request.Path + "?";
            ExportPageInfo1.UrlParams = Request.QueryString;
            ExportPageInfo1.CurrencyPage = EyouSoft.Common.Utils.GetInt(Request.QueryString["page"], 1);
        }
        #endregion

        //绑定航空公司
        protected void AirDataBind(int value)
        {
            IList<EnumObj> proList = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.PlanStructure.FlightCompany));
            ddlAirLineIds.DataSource = proList;
            ddlAirLineIds.DataBind();
            ListItem item = new ListItem();
            item.Value = "-1";
            item.Text = "--请选择--";
            this.ddlAirLineIds.Items.Insert(0, item);
            this.ddlAirLineIds.SelectedIndex = value;
        }
    }
}
