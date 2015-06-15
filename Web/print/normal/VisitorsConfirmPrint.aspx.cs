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

namespace Web.print.normal
{
    /// <summary>
    /// 芭比来-散客确认单 田想兵
    /// </summary>
    public partial class VisitorsConfirmPrint : Eyousoft.Common.Page.BackPage
    {
        protected DateTime? LeaveDate = null;
        public int ci = 1;
        private string td_class = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCusList();

                string tourId = Utils.GetQueryStringValue("tourId");
                //声明bll对象
                EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
                //声明团队计划对象
                EyouSoft.Model.TourStructure.TourInfo model = (EyouSoft.Model.TourStructure.TourInfo)bll.GetTourInfo(tourId);
                if (model != null)
                {
                    lt_routName.Text = model.RouteName;

                    #region 专线
                    EyouSoft.Model.CompanyStructure.ContactPersonInfo userInfoModel = new EyouSoft.BLL.CompanyStructure.CompanyUser().GetUserBasicInfo(model.OperatorId);
                    if (userInfoModel != null)
                    {
                        txt_Ldate.Value = model.LDate.ToString("yyyy-MM-dd");
                        txt_lxr2.Value = userInfoModel.ContactName;
                        //主要联系人电话
                        this.txt_tel2.Value = userInfoModel.ContactTel;
                        //专线FAX
                        this.txt_fax2.Value = userInfoModel.ContactFax;
                        EyouSoft.Model.CompanyStructure.CompanyInfo companyModel = new EyouSoft.BLL.CompanyStructure.CompanyInfo().GetModel(model.CompanyId, SiteUserInfo.SysId);
                        if (companyModel != null)
                        {
                            txt_fax2.Value = companyModel.ContactFax;
                            txt_tel2.Value = companyModel.ContactTel;
                            txt_lxr2.Value = companyModel.ContactName;
                            this.txtyfJbr.Value = companyModel.ContactName;
                        }
                    }
                    #endregion

                    txt_HCHB.Value = model.RTraffic;
                    txt_QCHB.Value = model.LTraffic;
                    if (model.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                    {
                        txt_jhDate.Value = model.GatheringTime;
                        //txt_str.Value = model.SentPeoples.FirstOrDefault().OperatorName;
                        EyouSoft.Model.TourStructure.TourSentPeopleInfo sentModel = model.SentPeoples.FirstOrDefault();
                        if (sentModel != null)
                            txt_str.Value = sentModel.OperatorName;
                        txt_jhdd.Value = model.GatheringPlace;
                        txt_jhBZ.Value = model.GatheringSign;
                        xc_Normarl.Visible = true;
                        tb_service_normal.Visible = true;
                        //出团日期
                        LeaveDate = model.LDate;
                        this.rptTravel.DataSource = model.TourNormalInfo.Plans;
                        this.rptTravel.DataBind();  //不含项目
                        this.lblNoProject.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.BuHanXiangMu);
                        //购物安排
                        this.lblBuy.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.GouWuAnPai);
                        //儿童安排
                        this.lblChildPlan.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.ErTongAnPai);
                        //自费项目
                        this.lblSelfProject.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.ZiFeiXIangMu);
                        //注意事项
                        this.lblNote.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.ZhuYiShiXiang);
                        //温馨提示
                        this.lblTips.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.WenXinTiXing);
                        //行程安排
                        this.rptTravel.DataSource = model.TourNormalInfo.Plans;
                        this.rptTravel.DataBind();
                        //包含项目
                        this.rptProject.DataSource = model.TourNormalInfo.Services;
                        this.rptProject.DataBind();
                    }
                    else
                    {
                        tb_service_quick.Visible = true;
                        xc_quick.Visible = true;
                        litTravel.Text = model.TourQuickInfo.QuickPlan;
                        litService.Text = model.TourQuickInfo.Service;
                    }
                }
            }
        }
        protected string GetDinnerByValue(string dinner)
        {
            string str = "";
            if (dinner.Trim().Length > 0)
            {
                string[] list = dinner.Split(',');
                if (list.Contains("2"))
                {
                    str += "早,";
                }
                if (list.Contains("3"))
                {
                    str += "中,";
                }
                if (list.Contains("4"))
                {
                    str += "晚,";
                }
            }
            str = str.TrimEnd(',');
            return str;
        }
        /// <summary>
        /// 根据出团日期 获得每一天
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected string GetDateByIndex(int index)
        {
            if (LeaveDate != null)
            {
                DateTime dateTime = Convert.ToDateTime(LeaveDate);
                return dateTime.AddDays(index).ToString("yyyy-MM-dd");
            }
            else
            {
                return "";
            }
        }
        void BindCusList()
        {
            string tourId = Utils.GetQueryStringValue("tourId");
            string orderId = Utils.GetQueryStringValue("orderId");
            if (tourId != "")
            {
                EyouSoft.BLL.TourStructure.TourOrder orderbll = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
                if (orderId != "")
                {
                    EyouSoft.Model.TourStructure.TourOrder orderModel = orderbll.GetOrderModel(CurrentUserCompanyID, orderId);
                    if (orderModel != null)
                    {

                        
                        txt_RouteName.Value = orderModel.RouteName;
                        int companyId = orderModel.BuyCompanyID;

                        EyouSoft.Model.CompanyStructure.CustomerInfo cusModel = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomerModel(companyId);
                        if (cusModel != null)
                        {
                            txt_tel.Value = cusModel.Phone;
                            txt_fax.Value = cusModel.Fax;
                            txt_lxr.Value = cusModel.ContactName;
                            txtJbr.Value = cusModel.ContactName; 
                            txt_TeamName.Value = cusModel.Name;
                            txt_jiafan.Value = cusModel.Name;
                        }
                        txt_large.Value = EyouSoft.Common.Function.StringValidate.ConvertNumAmtToChinese(orderModel.SumPrice);
                        txt_Price.Value = orderModel.SumPrice.ToString("###,##0.00");
                        txt_PepoleNum.Value = orderModel.AdultNumber + "/成人数" + "+" + orderModel.ChildNumber + "/儿童数";
                        IList<EyouSoft.Model.TourStructure.TourOrderCustomer> cusList = orderModel.CustomerList.Where(x => x.CustomerStatus == EyouSoft.Model.EnumType.TourStructure.CustomerStatus.正常).ToList();
                        cus_list.DataSource = cusList;
                        cus_list.DataBind();
                        if (cusList.Count > 0)
                        {
                            txt_first.Value = cusList[0].ContactTel;
                        }
                    }
                }
                else
                {
                    IList<EyouSoft.Model.TourStructure.TourOrderCustomer> cusList = orderbll.GetTravellers(tourId).Where(x => x.CustomerStatus == EyouSoft.Model.EnumType.TourStructure.CustomerStatus.正常).ToList(); ;

                    cus_list.DataSource = cusList;
                    cus_list.DataBind();
                    if (cusList.Count > 0)
                    {
                        txt_first.Value = cusList[0].ContactTel;
                    }
                }
            }
        }
    }
}
