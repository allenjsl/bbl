using System;
using System.Collections;
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
using System.Collections.Generic;

namespace Web.print.wh
{
    /// <summary>
    /// 望海-散客确认单
    /// </summary>
    public partial class VisitorsConfirmPrint : Eyousoft.Common.Page.BackPage
    {

        public int ci = 1;
        public DateTime? xcTime = null;
        public string FR = "";
        public string FR_TEL = "";
        public string FR_FAX = "";
        public int visitorRowsCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindXC();
            }
        }

        /// <summary>
        /// 根据天数 获得日期
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public string getDate(int day)
        {
            return xcTime.Value.AddDays(day).ToString("MM-dd");
        }

        /// <summary>
        /// 获得对应显示文字
        /// </summary>
        /// <param name="eat"></param>
        /// <returns></returns>
        public string getEat(string eat)
        {
            string returnValue = "";
            if (eat.Contains("2"))
            {
                returnValue += "早";
            }
            if (eat.Contains("3"))
            {
                returnValue += "中";
            }
            if (eat.Contains("4"))
            {
                returnValue += "晚";
            }
            return returnValue;
        }


        /// <summary>
        /// 初始化方法
        /// </summary>
        void BindXC()
        {
            string tourId = Utils.GetQueryStringValue("tourId");
            if (tourId == "")
            {
                Response.Write("未找到信息!");
                return;
            }

            //声明bll对象
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            //声明散拼计划对象
            EyouSoft.Model.TourStructure.TourInfo model = (EyouSoft.Model.TourStructure.TourInfo)bll.GetTourInfo(tourId);
            if (model != null)
            {
                //设置出团时间
                xcTime = model.LDate;
                //发送专线
                this.txtFromCompany.Text = SiteUserInfo.CompanyName;
                this.txtCompany.Text = SiteUserInfo.CompanyName;
                if (SiteUserInfo.ContactInfo != null)
                {
                    //联系人姓名
                    this.txtFromName.Text = SiteUserInfo.ContactInfo.ContactName;
                    //联系人传真
                    this.txtFromFax.Text = SiteUserInfo.ContactInfo.ContactFax;
                    //联系人电话
                    this.txtFromTel.Text = SiteUserInfo.ContactInfo.ContactTel;
                    this.txtContactTel.Text = SiteUserInfo.ContactInfo.ContactTel;
                }

                //线路名称
                this.txtAreaName.Text = model.RouteName;
                //天数
                this.txtDayCount.Text = model.TourDays.ToString();
                //出发交通
                this.txtLTraffic.Text = model.LTraffic;
                //返程交通
                this.txtRTraffic.Text = model.RTraffic;
                //人数
                //this.txtPeopleCount.Text = model.PlanPeopleNumber.ToString();
                //设置游客信息
                IList<EyouSoft.Model.TourStructure.TourOrderCustomer> customerList = new EyouSoft.BLL.TourStructure.TourOrder().GetTravellers(tourId).Where(x => x.CustomerStatus == EyouSoft.Model.EnumType.TourStructure.CustomerStatus.正常).ToList();
                ;
                if (customerList != null && customerList.Count > 0)
                {
                    visitorRowsCount = customerList.Count;
                    //绑定旅客列表
                    this.rptCustomerList.DataSource = customerList;
                    this.rptCustomerList.DataBind();
                }
                //获取订单信息
                EyouSoft.Model.TourStructure.TourOrder orderModel = new EyouSoft.BLL.TourStructure.TourOrder().GetOrderModel(SiteUserInfo.CompanyID, Utils.GetQueryStringValue("orderId"));
                int buyCompanyID = orderModel.BuyCompanyID;
                #region 接收方信息
                EyouSoft.Model.CompanyStructure.CustomerInfo cModel = new EyouSoft.Model.CompanyStructure.CustomerInfo();
                EyouSoft.BLL.CompanyStructure.Customer cBLL = new EyouSoft.BLL.CompanyStructure.Customer();
                cModel = cBLL.GetCustomerModel(buyCompanyID);
                txtToCompany.Text = cModel.Name;
                txtToName.Text = cModel.ContactName;
                txtToFax.Text = cModel.Fax;
                txtToTel.Text = cModel.Phone;
                #endregion
                //标准版
                if (model.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                {
                    //隐藏快速版行程安排
                    this.tblXCFast.Visible = false;
                    //隐藏快速版服务标准
                    this.tblFastService.Visible = false;

                    if (model.SentPeoples != null && model.SentPeoples.Count > 0)
                    {
                        //送团人信息
                        for (int i = 0; i < model.SentPeoples.Count; i++)
                        {
                            EyouSoft.Model.CompanyStructure.CompanyUser SendUserInfo = new EyouSoft.BLL.CompanyStructure.CompanyUser().GetUserInfo(model.SentPeoples[i].OperatorId);
                            if (SendUserInfo != null)
                            {
                                //送团人
                                this.txtSendMan.Text += SendUserInfo.PersonInfo.ContactName == null ? "" : SendUserInfo.PersonInfo.ContactName + ",";
                                //送团电话
                                this.txtSendTel.Text += SendUserInfo.PersonInfo == null ? "" : SendUserInfo.PersonInfo.ContactTel + ",";
                            }

                        }
                        this.txtSendMan.Text = this.txtSendMan.Text.TrimEnd(',');
                        this.txtSendTel.Text = this.txtSendTel.Text.TrimEnd(',');


                    }
                    if (model.TourNormalInfo != null)
                    {
                        //行程安排
                        this.xc_list.DataSource = model.TourNormalInfo.Plans;
                        this.xc_list.DataBind();
                        //包含项目
                        this.rptProject.DataSource = model.TourNormalInfo.Services;
                        this.rptProject.DataBind();
                        //不包含项目
                        this.lblNoPriject.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.BuHanXiangMu);
                        //儿童安排
                        this.lblChildren.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.ErTongAnPai);

                        //购物安排
                        this.lblShop.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.GouWuAnPai);
                        //赠送项目
                        this.lblZiFei.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.ZiFeiXIangMu);
                        //注意事项
                        this.lblZhuyi.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.ZhuYiShiXiang);
                        //温馨提醒
                        this.lblTiXing.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.WenXinTiXing);
                    }
                    //获得报价说明
                    if (Utils.GetQueryStringValue("orderId") != "")
                    {
                        //EyouSoft.Model.TourStructure.TourOrder orderModel = new EyouSoft.BLL.TourStructure.TourOrder().GetOrderModel(SiteUserInfo.CompanyID, Utils.GetQueryStringValue("orderId"));
                        if (orderModel != null)
                        {
                            //成人价格
                            this.lblManPrice.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(orderModel.PersonalPrice.ToString("0.00"));
                            //成人数
                            this.lblManCount.Text = orderModel.AdultNumber.ToString();
                            //儿童价格
                            this.lblChildPrice.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(orderModel.ChildPrice.ToString("0.00"));
                            //儿童数
                            this.lblChildCount.Text = orderModel.ChildNumber.ToString();
                            this.txtPeopleCount.Text = (orderModel.AdultNumber + orderModel.ChildNumber).ToString();
                            //总价格
                            this.lblAllPrice.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(orderModel.SumPrice.ToString("0.00"));
                        }
                        else
                        {
                            //成人价格
                            this.lblManPrice.Text = "0";
                            //成人数
                            this.lblManCount.Text = "0";
                            //儿童价格
                            this.lblChildPrice.Text = "0";
                            //儿童数
                            this.lblChildCount.Text = "0";
                            //总价格
                            this.lblAllPrice.Text = "0";
                        }
                    }
                    else
                    {
                        //成人价格
                        this.lblManPrice.Text = "0";
                        //成人数
                        this.lblManCount.Text = "0";
                        //儿童价格
                        this.lblChildPrice.Text = "0";
                        //儿童数
                        this.lblChildCount.Text = "0";
                        //总价格
                        this.lblAllPrice.Text = "0";
                    }




                }
                //快速版
                else
                {

                    this.txtPeopleCount.Text = (orderModel.AdultNumber + orderModel.ChildNumber).ToString();
                    //隐藏标准版 行程安排
                    this.tblXingCheng.Visible = false;
                    //隐藏标准版 服务
                    this.tblNoService.Visible = false;
                    if (model.TourQuickInfo != null)
                    {
                        //设置行程安排
                        this.lclXingCheng.Text = model.TourQuickInfo.QuickPlan;
                        //设置服务标准
                        this.lclService.Text = model.TourQuickInfo.Service;
                    }


                }
            }


            EyouSoft.Model.CompanyStructure.CompanyInfo infoModel = new EyouSoft.BLL.CompanyStructure.CompanyInfo().GetModel(SiteUserInfo.CompanyID, SiteUserInfo.SysId);
            if (infoModel != null)
            {
                EyouSoft.Model.CompanyStructure.CompanyAccount account = null;//公司账户
                if (infoModel.CompanyAccountList != null && infoModel.CompanyAccountList.Count > 0)
                {
                    account = infoModel.CompanyAccountList[0];

                    this.lblCompanyName.Text = SiteUserInfo.CompanyName;
                    this.lblBankName.Text = account.BankName;
                    this.lblBankNum.Text = account.BankNo;
                    this.lblBankUserName.Text = account.AccountName;
                }
            }

        }



    }
}
