using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using EyouSoft.Common;
using Eyousoft.Common.Page;
using System.Collections.Generic;

namespace Web.jipiao
{
    /// <summary>
    /// 机票列表页面
    /// 修改记录：
    /// 1、2011-01-12 曹胡生 创建
    /// </summary>
    public partial class JiPiaoList : Eyousoft.Common.Page.BackPage
    {
        #region 分页参数
        protected int pageSize = 20;
        protected int pageIndex = 1;
        int recordCount = 0;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.机票管理_机票管理_栏目))
            {
                EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.机票管理_机票管理_栏目, false);
                return;
            }
            if (!IsPostBack)
            {
                onInit();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.general;
        }

        //机票列表绑定
        private void onInit()
        {
            #region 获取付款申请单
            EyouSoft.BLL.CompanyStructure.CompanySetting CompanyBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            string printPath = CompanyBll.GetPrintPath(SiteUserInfo.CompanyID, EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType.付款申请单);
            if (!string.IsNullOrEmpty(printPath))
            {
                hypJiPiao.Visible = true;
                hypJiPiao.NavigateUrl = printPath;
            }
            else
            {
                hypJiPiao.Visible = false;
            }
            #endregion
            //计调员姓名
            string operName = Utils.GetQueryStringValue("operName").Trim();
            //计调员ID
            string[] operID = Utils.GetQueryStringValue("oper").Split(',');
            //日期
            string inputDate = Utils.GetQueryStringValue("inputDate").Trim();
            //航段
            string inputGoLine = Utils.GetQueryStringValue("inputGoLine").Trim();
            //操作人ID
            string[] operIds = Utils.GetQueryStringValue("oper").Split(',');
            //开始时间
            string timeStart = Utils.GetQueryStringValue("timeStart");
            //截止日期
            string timeEnd = Utils.GetQueryStringValue("timeEnd");

            ///团号
            string tourCode = Utils.GetQueryStringValue("tourCode");
            //机票状态
            EyouSoft.Model.EnumType.PlanStructure.TicketState ticketState;
            if (Utils.GetQueryStringValue("ticketState") != null && Utils.GetQueryStringValue("ticketState") != "")
            {
                ticketState = (EyouSoft.Model.EnumType.PlanStructure.TicketState)(Convert.ToUInt32(Utils.GetQueryStringValue("ticketState")));

            }
            else
            {
                ticketState = EyouSoft.Model.EnumType.PlanStructure.TicketState.None;
            }
            this.selectOperator1.OperId = Utils.GetQueryStringValue("oper");
            this.selectOperator1.OperName = operName;

            DateTime? lSDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("lsdate"));
            DateTime? lEDate = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("ledate"));

            ListBind(operID, operName, inputDate, inputGoLine, timeStart, timeEnd, ticketState, tourCode, lSDate, lEDate);
        }

        //列表初始化
        protected void ListBind(string[] operID, string operName, string inputDate, string inputGoLine, string timeStart, string timeEnd, EyouSoft.Model.EnumType.PlanStructure.TicketState state, string tourCode, DateTime? lSDate, DateTime? lEDate)
        {
            int pageCount = 1;
            pageIndex = Utils.GetInt(Utils.GetQueryStringValue("Page"), 1);
            EyouSoft.BLL.PlanStruture.PlaneTicket PlaneTicketBll = new EyouSoft.BLL.PlanStruture.PlaneTicket(SiteUserInfo);
            EyouSoft.Model.PlanStructure.TicketSearchModel SearchModel = new EyouSoft.Model.PlanStructure.TicketSearchModel();
            System.Collections.Generic.IList<EyouSoft.Model.PlanStructure.TicketInfo> Ilist = null;
            if (!(string.IsNullOrEmpty(operName)
               && string.IsNullOrEmpty(inputDate)
               && string.IsNullOrEmpty(inputGoLine)
               && string.IsNullOrEmpty(timeStart)
               && string.IsNullOrEmpty(timeEnd)
               && Convert.ToInt32(state) == 0
               && string.IsNullOrEmpty(tourCode)
               && !lSDate.HasValue
               && !lEDate.HasValue))
            {
                SearchModel.TicketListOrFinancialList = 2;
                SearchModel.CompanyId = SiteUserInfo.CompanyID;
                SearchModel.Operator = operName;
                SearchModel.DepartureTime = inputDate;
                SearchModel.FligthSegment = inputGoLine;
                SearchModel.TourCode = tourCode;
                if (timeStart != "")
                {
                    SearchModel.AirTimeStart = Convert.ToDateTime(timeStart);
                }
                if (timeEnd != "")
                {
                    SearchModel.AirTimeEnd = Convert.ToDateTime(timeEnd);

                }
                SearchModel.TicketState = state;
                SearchModel.LSDate = lSDate;
                SearchModel.LEDate = lEDate;

                //根据条件搜索相关记录
                Ilist = PlaneTicketBll.SearchTicketOut(pageSize, pageIndex, SearchModel, ref recordCount, ref pageCount);
            }
            else
            {
                Ilist = PlaneTicketBll.GetTicketList(pageSize, pageIndex, SiteUserInfo.CompanyID, ref recordCount, ref pageCount);
            }
            if (Ilist != null && Ilist.Count > 0)
            {
                this.repList.DataSource = Ilist;
                this.repList.DataBind();
            }
            else
            {
                this.ExporPageInfoSelect1.Visible = false;
                this.repList.EmptyText = "<tr><td height='30px' bgcolor='#e3f1fc' colspan='12' align='center'>暂时没有数据！</td></tr>";
            }
            PlaneTicketBll = null;
            SearchModel = null;
            BindPage();
            this.txtTimeStart.Value = timeStart;
            this.txtTimeEnd.Value = timeEnd;
            if (this.dpTicketState.Items.FindByValue(Convert.ToInt32(state).ToString()) != null)
            {
                this.dpTicketState.Items.FindByValue(Convert.ToInt32(state).ToString()).Selected = true;
            }

        }

        // 分页控件绑定
        protected void BindPage()
        {
            this.ExporPageInfoSelect1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"].ToString() + "?";
            this.ExporPageInfoSelect1.UrlParams = Request.QueryString;
            this.ExporPageInfoSelect1.intPageSize = pageSize;
            this.ExporPageInfoSelect1.CurrencyPage = pageIndex;
            this.ExporPageInfoSelect1.intRecordCount = recordCount;
        }

        protected void repList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            EyouSoft.Model.PlanStructure.TicketInfo model = (EyouSoft.Model.PlanStructure.TicketInfo)e.Item.DataItem;
            System.Web.UI.HtmlControls.HtmlAnchor ChuPiao = e.Item.FindControl("ChuPiao") as System.Web.UI.HtmlControls.HtmlAnchor;
            System.Web.UI.HtmlControls.HtmlAnchor TuiPiao = e.Item.FindControl("TuiPiao") as System.Web.UI.HtmlControls.HtmlAnchor;
            System.Web.UI.HtmlControls.HtmlAnchor Show = e.Item.FindControl("Show") as System.Web.UI.HtmlControls.HtmlAnchor;

            if (model.TicketType == EyouSoft.Model.EnumType.PlanStructure.TicketType.订单退票)
            {
                Show.Visible = false;
                TuiPiao.Visible = true;
                TuiPiao.Attributes.Add("goUrl", "JiPiao_tuipiao.aspx?id=" + model.Id.ToString());
                ChuPiao.Visible = false;
            }
            if (model.State == EyouSoft.Model.EnumType.PlanStructure.TicketState.审核通过)
            {
                ChuPiao.Visible = true;
                TuiPiao.Visible = false;
                ChuPiao.Attributes.Add("goUrl", "JiPiao_chupiao.aspx?tourid=" + model.TourId + "&id=" + model.RefundId);
                Show.Visible = false;
            }
            Show.Attributes.Add("goUrl", "/caiwuguanli/JiPiaoAuditShow.aspx?id=" + model.RefundId);
        }

        /// <summary>
        /// 获得机票的当前状态
        /// </summary>
        /// <returns></returns>
        protected string GetTicketState(object ticketType, object state)
        {
            string ticketState = "";

            if ((EyouSoft.Model.EnumType.PlanStructure.TicketState)state == EyouSoft.Model.EnumType.PlanStructure.TicketState.机票申请 && (EyouSoft.Model.EnumType.PlanStructure.TicketType)ticketType == EyouSoft.Model.EnumType.PlanStructure.TicketType.订单退票)
            {
                ticketState = "退票申请";
            }
            else
            {
                ticketState = ((EyouSoft.Model.EnumType.PlanStructure.TicketState)state).ToString();
            }
            return ticketState;
        }

        protected string GetColor(string state)
        {
            string html = "";
            switch (state)
            {
                case "已退票":
                    html = "<tr bgcolor='#B465F'>";
                    break;
                case "已出票":
                    html = "<tr bgcolor='#6699FF'>";
                    break;
                default:
                    html = "<tr bgcolor='#BDDCF4'>";
                    break;
            }
            return html;
        }

        /// <summary>
        /// 获得机票的 时间、航段 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected string GetHtmlByList(object list, DateTime RegisterTime)
        {
            string str = "";
            if (list == null)
            {
                str = "<tr><td height=\"30\" width=\"110px\" style=\"border-right:solid 1px #fff;border-bottom:solid 0px #fff;\" >" + RegisterTime.ToString("yyyy-MM-dd") + "</td> <td style=\"border-bottom:solid 0px #fff\" width=\"100px\"></td></tr>";
            }
            else
            {
                IList<EyouSoft.Model.PlanStructure.TicketFlight> refundFlightList = (List<EyouSoft.Model.PlanStructure.TicketFlight>)list;
                if (refundFlightList != null && refundFlightList.Count > 0)
                {
                    for (int i = 0; i < refundFlightList.Count; i++)
                    {
                        if (i == (refundFlightList.Count - 1))
                        {
                            str += "<tr><td height=\"30\" width=\"110px\" style=\"border-right:solid 1px #fff;border-bottom:solid 0px #fff;\" >" + refundFlightList[i].DepartureTime.ToString("yyyy-MM-dd") + "</td> <td style=\"border-bottom:solid 0px #fff\" width=\"100px\">" + refundFlightList[i].FligthSegment + "</td></tr>";
                        }
                        else
                        {
                            str += "<tr><td height=\"30\" width=\"110px\" style=\"border-right:solid 1px #fff;border-bottom:solid 1px #fff;\" >" + refundFlightList[i].DepartureTime.ToString("yyyy-MM-dd") + "</td> <td style=\"border-bottom:solid 1px #fff\" width=\"100px\">" + refundFlightList[i].FligthSegment + "</td></tr>";
                        }
                    }

                }
                else
                {
                    str += "<tr><td height=\"30\" width=\"110px\"  class=\"FlightTD\"></td> <td class=\"FlightTD\"  width=\"100px\"></td></tr>";
                }

            }
            return str;
        }
    }
}
