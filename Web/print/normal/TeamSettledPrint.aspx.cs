using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using EyouSoft.Common;

namespace Web.print.normal
{
    /// <summary>
    /// 芭比来-团队结算单
    /// 创建人：戴银柱
    /// </summary>
    public partial class TeamSettledPrint : Eyousoft.Common.Page.BackPage
    {

        /// <summary>
        /// 旅客 TD 数
        /// </summary>
        protected int rptCustomerTDCount = 0;

        /// <summary>
        /// 机票 TD 数
        /// </summary>
        protected int rptTicketPriceTDCount = 0;

        /// <summary>
        /// 地接 TD 数
        /// </summary>
        protected int rptDjPriceTDCount = 0;
        /// <summary>
        /// 订单ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected string orderId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string tourId = Utils.GetQueryStringValue("tourId");
                orderId = Utils.GetQueryStringValue("orderId");
                if (tourId != "")
                {
                    DataInit(tourId);
                }
            }
        }

        /// <summary>
        /// 页面初始化方法
        /// </summary>
        /// <param name="tourId"></param>
        private void DataInit(string tourId)
        {
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            EyouSoft.Model.TourStructure.TourBaseInfo model = bll.GetTourInfo(tourId);

            EyouSoft.Model.TourStructure.TourInfo infoModel = null;
            EyouSoft.Model.TourStructure.TourTeamInfo teamModel = null;

            if (model != null && model.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划)
            {
                infoModel = (EyouSoft.Model.TourStructure.TourInfo)model;

            }
            else
            {
                teamModel = (EyouSoft.Model.TourStructure.TourTeamInfo)model;
            }

            this.lblTourCode.Text = model.TourCode;
            this.lblAreaName.Text = model.RouteName;
            this.lblBenginDate.Text = model.LDate.ToString("yyyy-MM-dd");
            this.lblEndDate.Text = model.RDate.ToString("yyyy-MM-dd");
            #region 结算价
            if (EyouSoft.Model.EnumType.TourStructure.TourType.团队计划 == model.TourType)
            {

                TourQuotePrint priantTQP = new TourQuotePrint();
                string number = string.Empty;
                string money = string.Empty;
                priantTQP.getPepoleNum(SiteUserInfo.CompanyID, model.PlanPeopleNumber, teamModel.TourTeamUnit, ref number, ref money);
                this.lblCount.Text = number;
            }
            #endregion
            //this.lblCount.Text = model.PlanPeopleNumber.ToString();
            this.lblTeamPrice.Text = Utils.FilterEndOfTheZeroString(model.TotalIncome.ToString("0.00"));

            this.lblMeter.Text = "";
            //票务
            this.lblTicket.Text = "";
            //团 利 润 
            this.lblProfit.Text = Utils.FilterEndOfTheZeroString((model.TotalIncome + model.TotalOtherIncome - model.TotalExpenses - model.TotalOtherExpenses - model.DistributionAmount).ToString("0.00"));
            //备注
            this.lblRemarks.Text = "";

            //绑定旅客信息
            List<EyouSoft.Model.TourStructure.TourOrder> lorder = new List<EyouSoft.Model.TourStructure.TourOrder>();
            EyouSoft.Model.TourStructure.TourOrder customerList = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo).GetOrderModel
(SiteUserInfo.CompanyID, orderId);
            if (customerList != null)
            {
                EyouSoft.BLL.CompanyStructure.Customer cinfo = new EyouSoft.BLL.CompanyStructure.Customer();
                EyouSoft.Model.CompanyStructure.CustomerInfo cm = cinfo.GetCustomerModel(customerList.BuyCompanyID);
                if (cm != null)
                {
                    customerList.BuyCompanyName = cm.Name;
                }
                lorder.Add(customerList);
            }
            this.rptCustomer.DataSource = lorder;
            this.rptCustomer.DataBind();
            //获得该计划下的所有地接的集合
            EyouSoft.BLL.PlanStruture.TravelAgency travelBll = new EyouSoft.BLL.PlanStruture.TravelAgency();
            IList<EyouSoft.Model.PlanStructure.LocalTravelAgencyInfo> travelList = travelBll.GetList(tourId);
            if (travelList != null && travelList.Count > 0)
            {
                rptDjPriceTDCount = travelList.Count * 2;
            }
            this.rptDjPrice.DataSource = travelList;
            this.rptDjPrice.DataBind();
            //添加地接备注
            if (travelList != null && travelList.Count > 0)
            {
                for (int i = 0; i < travelList.Count; i++)
                {
                    this.lblRemarks.Text += travelList[i].Remark + "<br />";
                }
            }
            //机票费列表
            EyouSoft.BLL.PlanStruture.PlaneTicket ticketBll = new EyouSoft.BLL.PlanStruture.PlaneTicket();
            IList<EyouSoft.Model.PlanStructure.MLBTicketApplyInfo> ticketList = ticketBll.GetTicketApplys(CurrentUserCompanyID, tourId);
            if (ticketList != null && ticketList.Count > 0)
            {
                rptTicketPriceTDCount = ticketList.Count * 3;
            }
            this.rptTicketPrice.DataSource = ticketList;
            this.rptTicketPrice.DataBind();
            //添加机票备注
            if (ticketList != null && ticketList.Count > 0)
            {
                for (int i = 0; i < ticketList.Count; i++)
                {
                    this.lblRemarks.Text += ticketList[i].Remark;
                }
            }
            //获得计调信息
            IList<string> coordList = new EyouSoft.BLL.TourStructure.Tour().GetTourCoordinators(tourId);
            if (coordList != null && coordList.Count > 0)
            {
                for (int i = 0; i < coordList.Count; i++)
                {
                    this.lblMeter.Text += coordList[i] + "<br />";
                }
            }
            //获得票务信息
            IList<string> operatorList = new EyouSoft.BLL.PlanStruture.PlaneTicket().CustomerOperatorList(tourId);
            if (operatorList != null && operatorList.Count > 0)
            {
                for (int i = 0; i < operatorList.Count; i++)
                {
                    this.lblTicket.Text += operatorList[i] + "<br />";
                }
            }
        }


        /// <summary>
        /// 获得机票款 人数
        /// </summary>
        /// <param name="fundAdult"></param>
        /// <param name="fundChildren"></param>
        /// <returns></returns>
        protected string GetPeopleCount(object fundAdult, object fundChildren)
        {
            //总人数 
            int pCount = 0;
            if (fundAdult != null)
            {
                pCount = ((EyouSoft.Model.PlanStructure.TicketKindInfo)fundAdult).PeopleCount;
            }
            if (fundChildren != null)
            {
                pCount = pCount + ((EyouSoft.Model.PlanStructure.TicketKindInfo)fundChildren).PeopleCount;
            }
            return pCount.ToString();
        }

        protected string GetDiscount(object list)
        {
            string str = "";
            IList<EyouSoft.Model.PlanStructure.TicketFlight> ticketFlightList = (List<EyouSoft.Model.PlanStructure.TicketFlight>)list;
            if (ticketFlightList != null && ticketFlightList.Count > 0)
            {
                for (int i = 0; i < ticketFlightList.Count; i++)
                {
                    if (i == 0)
                    {
                        str += Utils.FilterEndOfTheZeroString(ticketFlightList[i].Discount.ToString("0.00")) + "%";
                    }
                    else
                    {
                        str += "/ " + Utils.FilterEndOfTheZeroString(ticketFlightList[i].Discount.ToString("0.00")) + "%";
                    }
                }
            }

            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected string GetCustomerHtmlByList(IList<EyouSoft.Model.TourStructure.TourOrderCustomer> list)
        {
            string htmlStr = "";
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].CustomerStatus == EyouSoft.Model.EnumType.TourStructure.CustomerStatus.正常)
                    {
                        if ((i + 1) != list.Count)
                        {
                            htmlStr += " <tr><td class=\"td_b_border\" width=\"73px\">" + list[i].VisitorName + "</td></tr>";
                        }
                        else
                        {
                            htmlStr += " <tr><td  width=\"73px\">" + list[i].VisitorName + "</td></tr>";
                        }
                    }
                }
            }
            return htmlStr;
        }
    }

}
