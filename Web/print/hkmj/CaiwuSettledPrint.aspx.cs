using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.print.hkmj
{
    public partial class CaiwuSettledPrint : Eyousoft.Common.Page.BackPage
    {
        //组团社名称
        protected string buyCompanyName = string.Empty;
        //组团社主要联系人
        protected string buyCompanyContentName = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string tourId = EyouSoft.Common.Utils.GetQueryStringValue("Tourid");
                if (!string.IsNullOrEmpty(tourId))
                {
                    DataInit(tourId);
                }
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="tourId"></param>
        protected void DataInit(string tourId)
        {
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            EyouSoft.Model.TourStructure.TourBaseInfo model = bll.GetTourInfo(tourId);

            EyouSoft.Model.TourStructure.TourInfo infoModel = null;
            EyouSoft.Model.TourStructure.TourTeamInfo teamModel = null;

            if (model != null && model.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划)
            {
                infoModel = (EyouSoft.Model.TourStructure.TourInfo)model;
                if (infoModel != null)
                {
                    //线路名称
                    this.litRouteName.Text = infoModel.RouteName;
                    this.litLdate.Text = infoModel.LDate.ToString("yyyy-MM-dd");
                    //专线主要联系人
                    this.litContectName.Value = this.SiteUserInfo.ContactInfo.ContactName;
                    this.litQuerenDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                }

                EyouSoft.Model.TourStructure.TourOrder customerList = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo).GetOrderModel(SiteUserInfo.CompanyID, EyouSoft.Common.Utils.GetQueryStringValue("orderId"));
                if (customerList != null)
                {
                    //组团社
                    buyCompanyName = customerList.BuyCompanyName;
                    //组团社联系人
                    buyCompanyContentName = customerList.BuyerContactName;
                    this.litbuyCompanyName.Text = customerList.BuyCompanyName;

                    this.litPeopleCount.Text = customerList.PeopleNumber.ToString();
                    this.litCostDetail.Text = (customerList.AdultNumber) + "*" + (customerList.PersonalPrice.ToString("0.00")) + " + " + (customerList.ChildNumber) + " * " + (customerList.ChildPrice.ToString("0.00"));
                    this.litCostSum.Text = customerList.SumPrice.ToString("0.00");
                    string vistors = string.Empty;
                    if (customerList.CustomerList != null && customerList.CustomerList.Count > 0)
                    {
                        customerList.CustomerList = customerList.CustomerList.Where(p => p.CustomerStatus == EyouSoft.Model.EnumType.TourStructure.CustomerStatus.正常).ToList();
                        if (customerList.CustomerList != null && customerList.CustomerList.Count > 0)
                        {
                            for (int i = 0; i < customerList.CustomerList.Count; i++)
                            {
                                vistors += customerList.CustomerList[i].VisitorName + ",";
                            }
                        }
                    }
                    this.litVistors.Text = vistors.TrimEnd(',');
                    //备注
                    if (customerList.AmountPlus != null)
                    {
                        this.litremark.Text = customerList.AmountPlus.Remark;
                    }
                }
            }
            else
            {
                teamModel = (EyouSoft.Model.TourStructure.TourTeamInfo)model;
                if (teamModel != null)
                {
                    //组团社
                    buyCompanyName = teamModel.BuyerCName;

                    this.litbuyCompanyName.Text = teamModel.BuyerCName;

                    //组团社联系人
                    EyouSoft.Model.CompanyStructure.CustomerInfo CustomerInfo = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomerModel(teamModel.BuyerCId);
                    if (CustomerInfo != null)
                    {
                        if (CustomerInfo.CustomerContactList != null && CustomerInfo.CustomerContactList.Count > 0)
                        {
                            buyCompanyContentName = CustomerInfo.CustomerContactList[0].Name;
                        }
                    }
                    this.litRouteName.Text = teamModel.RouteName;
                    this.litLdate.Text = teamModel.LDate.ToString("yyyy-MM-dd");
                    //专线主要联系人
                    this.litContectName.Value = this.SiteUserInfo.ContactInfo.ContactName;
                    this.litQuerenDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                }

                EyouSoft.Model.TourStructure.TourOrder customerList = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo).GetOrderModel
    (SiteUserInfo.CompanyID, EyouSoft.Common.Utils.GetQueryStringValue("orderId"));
                if (customerList != null)
                {
                    this.litPeopleCount.Text = customerList.PeopleNumber.ToString();

                    if (customerList.TourTeamUnit != null)
                    {
                        this.litCostDetail.Text = customerList.TourTeamUnit.UnitAmountCr.ToString("0.00") + "+" + customerList.TourTeamUnit.UnitAmountEt.ToString("0.00") + "+" + customerList.TourTeamUnit.UnitAmountQp.ToString("0.00");
                    }
                    this.litCostSum.Text = customerList.SumPrice.ToString("0.00");

                    string vistors = string.Empty;
                    if (customerList.CustomerList != null && customerList.CustomerList.Count > 0)
                    {
                        customerList.CustomerList = customerList.CustomerList.Where(p => p.CustomerStatus == EyouSoft.Model.EnumType.TourStructure.CustomerStatus.正常).ToList();
                        if (customerList.CustomerList != null && customerList.CustomerList.Count > 0)
                        {
                            for (int i = 0; i < customerList.CustomerList.Count; i++)
                            {
                                vistors += customerList.CustomerList[i].VisitorName + ",";
                            }
                        }
                    }
                    this.litVistors.Text = vistors.TrimEnd(',');
                    //备注
                    if (customerList.AmountPlus != null)
                    {
                        this.litremark.Text = customerList.AmountPlus.Remark;
                    }
                }
            }


            //公司账户信息
            EyouSoft.Model.CompanyStructure.CompanyInfo info = new EyouSoft.BLL.CompanyStructure.CompanyInfo().GetModel(CurrentUserCompanyID, CurrentUserCompanyID);
            if (info != null)
            {
                if (info.CompanyAccountList != null && info.CompanyAccountList.Count > 0)
                {
                    if (string.IsNullOrEmpty(info.CompanyAccountList[0].AccountName))
                    {
                        this.trAccountNameView.Visible = false;
                    }
                    else
                    {
                        this.lithuming.Text = info.CompanyAccountList[0].AccountName;
                    }
                    if (string.IsNullOrEmpty(info.CompanyAccountList[0].BankName) && string.IsNullOrEmpty(info.CompanyAccountList[0].BankNo))
                    {
                        this.trBankView.Visible = false;
                    }
                    else
                    {
                        this.litkaihuhang.Text = info.CompanyAccountList[0].BankName;
                        this.litzhanghao.Text = info.CompanyAccountList[0].BankNo;
                    }
                }
                else
                {
                    this.trBankView.Visible = false;
                    this.trAccountNameView.Visible = false;
                }
            }

        }

    }
}
