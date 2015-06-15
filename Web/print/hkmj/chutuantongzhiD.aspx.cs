using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.print.hkmj
{
    public partial class chutuantongzhiD : Eyousoft.Common.Page.BackPage
    {
        protected string TravelplanStr = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string tourId = Utils.GetQueryStringValue("tourId");
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
        protected void DataInit(string tourId)
        {
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            EyouSoft.Model.TourStructure.TourBaseInfo model = bll.GetTourInfo(tourId);
            if (model != null)
            {
                if (model.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划)
                {
                    EyouSoft.Model.TourStructure.TourInfo tModel = (EyouSoft.Model.TourStructure.TourInfo)model;
                    if (tModel != null)
                    {
                        //线路名称
                        this.litRouteName.Text = tModel.RouteName;

                        EyouSoft.Model.TourStructure.TourOrder order = new EyouSoft.BLL.TourStructure.TourOrder().GetOrderModel(CurrentUserCompanyID, Utils.GetQueryStringValue("orderId"));
                        if (order != null)
                        {
                            //组团社
                            this.litBuyCompanyName.Text = order.BuyCompanyName;
                            //组团社联系人
                            this.litCustomName.Text = order.ContactName;
                            this.litTel.Text = order.ContactTel;
                            this.litFax.Text = order.ContactFax;
                            //游客信息
                            string strVistors = string.Empty;
                            if (order.CustomerList != null && order.CustomerList.Count > 0)
                            {
                                for (int i = 0; i < order.CustomerList.Count; i++)
                                {
                                    strVistors += order.CustomerList[i].VisitorName + ",";
                                }
                                this.litVisitorsList.Text = strVistors.TrimEnd(',');
                            }
                        }


                        //机场送团
                        if (tModel.SentPeoples != null && tModel.SentPeoples.Count > 0)
                        {
                            string strSongTuanName = string.Empty;
                            for (int i = 0; i < tModel.SentPeoples.Count; i++)
                            {
                                strSongTuanName += tModel.SentPeoples[i].OperatorName + ",";
                            }
                            this.litSongTuanName.Text = strSongTuanName.TrimEnd(',');

                            //手机
                            EyouSoft.Model.CompanyStructure.CompanyUser userModel = new EyouSoft.BLL.CompanyStructure.CompanyUser().GetUserInfo(tModel.SentPeoples[0].OperatorId);
                            if (userModel != null)
                            {
                                this.litSongTuanPhone.Text = userModel.PersonInfo.ContactMobile;
                            }
                        }

                        //去程航班/时间
                        this.litLFilght.Text = tModel.LTraffic;

                        //返程航班/时间
                        this.litRFilght.Text = tModel.RTraffic;
                        //集合地点
                        this.litJihePlace.Text = tModel.GatheringPlace;
                        if (tModel.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                        {                            
                            this.litQplan.Visible = false;
                            this.pnequick.Visible = false;
                            if (tModel.TourNormalInfo != null)
                            {
                                //注意事项
                                this.litZhuYiShiXiang.Text = tModel.TourNormalInfo.ZhuYiShiXiang;
                                //温馨提醒
                                this.litWenXinTiXing.Text = tModel.TourNormalInfo.WenXinTiXing;
                                //行程安排
                                if (tModel.TourNormalInfo.Plans != null && tModel.TourNormalInfo.Plans.Count > 0)
                                {
                                    for (int i = 0; i < tModel.TourNormalInfo.Plans.Count; i++)
                                    {
                                        TravelplanStr += "<tr><td height=\"25\" align=\"center\" bgcolor=\"#FFFFFF\">" + tModel.LDate.AddDays(i).ToString("yyyy-MM-dd") + "</td><td align=\"left\" bgcolor=\"#FFFFFF\">" + tModel.TourNormalInfo.Plans[i].Plan + "</td><td align=\"center\" bgcolor=\"#FFFFFF\">" + (tModel.TourNormalInfo.Plans[i].Dinner.Contains("2") ? "早," : "") + "" + (tModel.TourNormalInfo.Plans[i].Dinner.Contains("3") ? "中," : "") + "" + (tModel.TourNormalInfo.Plans[i].Dinner.Contains("4") ? "晚," : "") + "</td><td align=\"center\" bgcolor=\"#FFFFFF\">" + tModel.TourNormalInfo.Plans[i].Hotel + "</td></tr>";
                                    }

                                }
                            }
                        }
                        else
                        {
                            this.pnetab.Visible = false;
                            this.tabView.Attributes.Add("style","border: 1px #000000 solid");
                            this.pneStand.Visible = false;
                            if (tModel.TourQuickInfo != null)
                            {
                                this.litQplan.Text = tModel.TourQuickInfo.QuickPlan;
                                this.litServerStand.Text = tModel.TourQuickInfo.Service;
                            }
                        }
                    }
                }
                if (model.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
                {
                    EyouSoft.Model.TourStructure.TourTeamInfo ttModel = (EyouSoft.Model.TourStructure.TourTeamInfo)model;
                    if (ttModel != null)
                    {
                        //线路名称
                        this.litRouteName.Text = ttModel.RouteName;
                        //组团社
                        this.litBuyCompanyName.Text = ttModel.BuyerCName;
                        //组团社联系人
                        EyouSoft.Model.CompanyStructure.CustomerInfo CustomerInfo = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomerModel(ttModel.BuyerCId);
                        if (CustomerInfo != null)
                        {
                            this.litCustomName.Text = CustomerInfo.ContactName;
                            this.litTel.Text = CustomerInfo.Phone;
                            this.litFax.Text = CustomerInfo.Fax;
                        }

                        EyouSoft.Model.TourStructure.TourOrder order = new EyouSoft.BLL.TourStructure.TourOrder().GetOrderModel(CurrentUserCompanyID, Utils.GetQueryStringValue("orderId"));
                        if (order != null)
                        {
                            //组团社
                            this.litBuyCompanyName.Text = order.BuyCompanyName;
                            //组团社联系人
                            this.litCustomName.Text = order.ContactName;
                            this.litTel.Text = order.ContactTel;
                            this.litFax.Text = order.ContactFax;
                            //游客信息
                            string strVistors = string.Empty;
                            if (order.CustomerList != null && order.CustomerList.Count > 0)
                            {
                                for (int i = 0; i < order.CustomerList.Count; i++)
                                {
                                    strVistors += order.CustomerList[i].VisitorName + ",";
                                }
                                this.litVisitorsList.Text = strVistors.TrimEnd(',');
                            }
                        }

                        //机场送团
                        if (ttModel.SentPeoples != null && ttModel.SentPeoples.Count > 0)
                        {
                            string strSongTuanName = string.Empty;
                            for (int i = 0; i < ttModel.SentPeoples.Count; i++)
                            {
                                strSongTuanName += ttModel.SentPeoples[i].OperatorName + ",";
                            }
                            this.litSongTuanName.Text = strSongTuanName.TrimEnd(',');

                            //手机
                            EyouSoft.Model.CompanyStructure.CompanyUser userModel = new EyouSoft.BLL.CompanyStructure.CompanyUser().GetUserInfo(ttModel.SentPeoples[0].OperatorId);
                            if (userModel != null)
                            {
                                this.litSongTuanPhone.Text = userModel.PersonInfo.ContactMobile;
                            }
                        }

                        //去程航班/时间
                        this.litLFilght.Text = ttModel.LTraffic;

                        //返程航班/时间
                        this.litRFilght.Text = ttModel.RTraffic;

                        //集合地点
                        this.litJihePlace.Text = ttModel.GatheringPlace;
                        if (ttModel.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                        {
                            this.litQplan.Visible = false;
                            this.pnequick.Visible = false;
                            if (ttModel.TourNormalInfo != null)
                            {
                                //注意事项
                                this.litZhuYiShiXiang.Text = ttModel.TourNormalInfo.ZhuYiShiXiang;
                                //温馨提醒
                                this.litWenXinTiXing.Text = ttModel.TourNormalInfo.WenXinTiXing;
                                //行程安排
                                if (ttModel.TourNormalInfo.Plans != null && ttModel.TourNormalInfo.Plans.Count > 0)
                                {
                                    for (int i = 0; i < ttModel.TourNormalInfo.Plans.Count; i++)
                                    {
                                        TravelplanStr += "<tr><td height=\"25\" align=\"center\" bgcolor=\"#FFFFFF\">" + ttModel.LDate.AddDays(i).ToString("yyyy-MM-dd") + "</td><td align=\"left\" bgcolor=\"#FFFFFF\">" + ttModel.TourNormalInfo.Plans[i].Plan + "</td><td align=\"center\" bgcolor=\"#FFFFFF\">" + (ttModel.TourNormalInfo.Plans[i].Dinner.Contains("2") ? "早," : "") + "" + (ttModel.TourNormalInfo.Plans[i].Dinner.Contains("3") ? "中," : "") + "" + (ttModel.TourNormalInfo.Plans[i].Dinner.Contains("4") ? "晚," : "") + "</td><td align=\"center\" bgcolor=\"#FFFFFF\">" + ttModel.TourNormalInfo.Plans[i].Hotel + "</td></tr>";
                                    }

                                }
                            }
                        }
                        else
                        {
                            this.pnetab.Visible = false;
                            this.pneStand.Visible = false;
                            this.tabView.Attributes.Add("style", "border: 1px #000000 solid");
                            if (ttModel.TourQuickInfo != null)
                            {
                                this.litQplan.Text = ttModel.TourQuickInfo.QuickPlan;
                                this.litServerStand.Text = ttModel.TourQuickInfo.Service;
                            }
                        }
                    }
                }

            }
        }
    }
}
