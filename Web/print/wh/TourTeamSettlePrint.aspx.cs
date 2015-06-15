using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.print.wh
{
    public partial class TourTeamSettlePrint : Eyousoft.Common.Page.BackPage
    {
        protected int servicesCount = 0;
        protected int allCount = 0;
        /// <summary>
        /// 订单号
        /// </summary>
        protected string orderId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string tourId = Utils.GetQueryStringValue("tourId");
            orderId = Utils.GetQueryStringValue("orderId");
            if (tourId != "")
            {
                DataInit(tourId);
            }
        }

        /// <summary>
        /// 页面初始化方法
        /// </summary>
        /// <param name="travelId">行程单ID</param>
        protected void DataInit(string tourId)
        {
            //声明bll对象
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            //声明团队计划对象
            EyouSoft.Model.TourStructure.TourBaseInfo model = bll.GetTourInfo(tourId);
            if (model != null && model.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
            {
                EyouSoft.Model.TourStructure.TourTeamInfo teamModel = (EyouSoft.Model.TourStructure.TourTeamInfo)model;
                //收款单位
                this.txtBuyerCName.Text = teamModel.BuyerCName;

                EyouSoft.Model.CompanyStructure.CustomerInfo cusModel = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomerModel(model.BuyerCId);
                if (cusModel != null)
                {
                    //收款人
                    txtContact.Text = cusModel.ContactName;
                    //电话
                    txtPbone.Text = cusModel.Phone;
                    //传真
                    txtFax.Text = cusModel.Fax;
                }
                //线路
                this.txtAreaName.Text = teamModel.RouteName;
                //人数
                this.txtCount.Text = teamModel.PlanPeopleNumber.ToString();
                //日期
                this.txtDataTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

                //价格组成
                if (teamModel.Services != null && teamModel.Services.Count > 0)
                {
                    this.servicesCount = teamModel.Services.Count;
                    this.rptProject.DataSource = teamModel.Services;
                    this.rptProject.DataBind();

                }


                //收入
                IList<EyouSoft.Model.TourStructure.OrderFinanceExpense> fristList = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo).GetOrderList(SiteUserInfo.CompanyID, tourId);
                if (fristList != null && fristList.Count > 0)
                {
                    allCount += fristList.Count;
                    this.rptFrist.DataSource = fristList;
                    this.rptFrist.DataBind();

                }

                //其它收入
                EyouSoft.Model.FinanceStructure.OtherCostQuery otherCostQueryModel = new EyouSoft.Model.FinanceStructure.OtherCostQuery();
                otherCostQueryModel.TourId = tourId;
                IList<EyouSoft.Model.FinanceStructure.OtherIncomeInfo> secoundList = new EyouSoft.BLL.FinanceStructure.OtherCost(SiteUserInfo).GetOtherIncomeList(otherCostQueryModel);
                if (secoundList != null && secoundList.Count > 0)
                {
                    allCount += secoundList.Count;
                    this.rptSecound.DataSource = secoundList;
                    this.rptSecound.DataBind();

                }

                //支出
                IList<EyouSoft.Model.PlanStructure.PaymentList> thirdList = new EyouSoft.BLL.PlanStruture.TravelAgency().GetSettleList(tourId);
                if (thirdList != null && thirdList.Count > 0)
                {
                    allCount += thirdList.Count;
                    this.rptThird.DataSource = thirdList;
                    this.rptThird.DataBind();


                }

                //其它支出
                IList<EyouSoft.Model.FinanceStructure.OtherOutInfo> forthList = new EyouSoft.BLL.FinanceStructure.OtherCost(SiteUserInfo).GetOtherOutList(otherCostQueryModel);
                if (forthList != null && forthList.Count > 0)
                {
                    allCount += forthList.Count;
                    this.rptForth.DataSource = forthList;
                    this.rptForth.DataBind();

                }
            }
        }
        /// <summary>
        /// 计算总价
        /// </summary>
        /// <param name="PersonalPrice"></param>
        /// <param name="AdultNumber"></param>
        /// <param name="ChildPrice"></param>
        /// <param name="ChildNumber"></param>
        /// <returns></returns>
        protected string sum(object PersonalPrice, object AdultNumber, object ChildPrice, object ChildNumber)
        {
            return (Utils.GetDecimal(PersonalPrice.ToString()) * Utils.GetInt(AdultNumber.ToString()) + Utils.GetDecimal(ChildPrice.ToString()) * Utils.GetInt(ChildNumber.ToString())).ToString("￥#,##0.00");
        }
    }
}
