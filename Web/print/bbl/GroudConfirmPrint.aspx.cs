using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Eyousoft.Common.Page;

namespace Web.print.bbl
{
    /// <summary>
    /// 地接确认单
    /// by 田想兵 20`11.5.12
    /// </summary>
    public partial class GroudConfirmPrint : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 出团时间
        /// </summary>
        protected DateTime? LeaveDate = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
            }
        }
        void Bind()
        {
            //声明bll对象
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            string tourId = Utils.GetQueryStringValue("tourId");
            if (tourId == "")
            {
                Response.Write("未找到信息!");
                return;
            }


            //声明团队计划对象
            EyouSoft.Model.TourStructure.TourBaseInfo basemodel = bll.GetTourInfo(tourId);
            LeaveDate = basemodel.LDate;
            lt_LDate.Text = basemodel.LDate.Month.ToString() + "月" + basemodel.LDate.Day.ToString() + "日" + basemodel.TourType.ToString();
            lt_TourName.Text = basemodel.RouteName;
            #region 组团端
            EyouSoft.Model.CompanyStructure.CustomerInfo cusModel = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomerModel(basemodel.BuyerCId);
            if (cusModel != null)
            {
                txt_to_fax.Text = cusModel.Fax;
                txt_to_name.Text = cusModel.Name;
                txt_to_tel.Text = cusModel.Phone;
                txt_to_user.Text = cusModel.ContactName;
            }
            #endregion
            #region 专线
            EyouSoft.Model.CompanyStructure.ContactPersonInfo userInfoModel = new EyouSoft.BLL.CompanyStructure.CompanyUser().GetUserBasicInfo(basemodel.OperatorId);
            if (userInfoModel != null)
            {
                //主要联系人电话
                this.txt_fr_tel.Text = userInfoModel.ContactTel;
                //专线FAX
                this.txt_fr_fax.Text = userInfoModel.ContactFax;

                EyouSoft.Model.CompanyStructure.CompanyInfo companyModel = new EyouSoft.BLL.CompanyStructure.CompanyInfo().GetModel(basemodel.CompanyId, SiteUserInfo.SysId);
                if (companyModel != null)
                {
                    this.txt_fr_user.Text = companyModel.ContactName;
                    txt_fr_name.Text = companyModel.CompanyName;
                    txt_fr_tel.Text = companyModel.ContactTel;
                    txt_fr_tel.Text = companyModel.ContactFax;
                }
            }
            #endregion
            if (basemodel.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划)
            {
                EyouSoft.Model.TourStructure.TourInfo model = (EyouSoft.Model.TourStructure.TourInfo)basemodel;
                if (model.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                {
                    tb_normal.Visible = true;
                    rptTravel.DataSource = model.TourNormalInfo.Plans;
                    rptTravel.DataBind();
                    rptProject.DataSource = model.TourNormalInfo.Services;
                    rptProject.DataBind();
                    div_noproject.Visible = true;
                    lblNoProject.Text = TextToHtml(model.TourNormalInfo.BuHanXiangMu);
                    div_notice.Visible = true;
                    lt_notice.Text = TextToHtml(model.TourNormalInfo.ZhuYiShiXiang);
                }
                else
                {
                    tb_normal.Visible = false;
                    tb_quick.Visible = true;
                    tb_quickService.Visible = true;
                    lb_Quick.Text = model.TourQuickInfo.QuickPlan;
                    lt_service.Text = model.TourQuickInfo.Service;
                }
            }
            else
                if (basemodel.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
                {
                    EyouSoft.Model.TourStructure.TourTeamInfo model = (EyouSoft.Model.TourStructure.TourTeamInfo)basemodel;
                    if (model.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                    {
                        tb_normal.Visible = true;
                        if (model.TourNormalInfo != null)
                        {
                            rptTravel.DataSource = model.TourNormalInfo.Plans;
                            rptTravel.DataBind();
                            rptProject.DataSource = model.Services;
                            rptProject.DataBind();
                            div_noproject.Visible = true;
                            lblNoProject.Text = TextToHtml(model.TourNormalInfo.BuHanXiangMu);
                            div_notice.Visible = true;
                            lt_notice.Text = TextToHtml(model.TourNormalInfo.ZhuYiShiXiang);
                        }
                    }
                    else
                    {
                        if (model.TourQuickInfo != null)
                        {
                            tb_normal.Visible = false;
                            tb_quick.Visible = true;
                            tb_quickService.Visible = true;
                            lb_Quick.Text = model.TourQuickInfo.QuickPlan;
                            lt_service.Text = model.TourQuickInfo.Service;
                        }
                    }
                    /*#region 结算价
                    TourQuotePrint priantTQP = new TourQuotePrint();
                    string number = string.Empty;
                    string money = string.Empty;
                    priantTQP.getPepoleNum(SiteUserInfo.CompanyID,model.PlanPeopleNumber, model.TourTeamUnit, ref number, ref money);
                    this.txt_Money.Text = money;
                    #endregion*/
                }
                else if (basemodel.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务)
                {
                    p_title.Visible = false;
                    d_title.Visible = false;
                    Title = "单项服务确认单";
                    tb_single.Visible = true;
                    tb_normal.Visible = false;
                    EyouSoft.Model.TourStructure.TourSingleInfo model = (EyouSoft.Model.TourStructure.TourSingleInfo)basemodel;
                    rpt_single.DataSource = model.Plans;
                    rpt_single.DataBind();
                    txt_Money.Text = model.TotalAmount.ToString("#,##0.00");
                }


            if (basemodel.TourType != EyouSoft.Model.EnumType.TourStructure.TourType.单项服务)
            {
                phJieSuanJiaGe.Visible = true;
                EyouSoft.BLL.PlanStruture.TravelAgency planbll = new EyouSoft.BLL.PlanStruture.TravelAgency();
                IList<EyouSoft.Model.PlanStructure.LocalTravelAgencyInfo> agencys = planbll.GetList(tourId);
                planbll = null;

                if (agencys != null && agencys.Count > 0)
                {
                    System.Text.StringBuilder s = new System.Text.StringBuilder();

                    foreach (var agency in agencys)
                    {
                        s.AppendFormat("单位名称：{0} 结算费用：{1}    ", agency.LocalTravelAgency, Utils.FilterEndOfTheZeroDecimal(agency.Settlement));
                    }

                    txt_Money.Text = s.ToString();
                }
            }
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

        protected string TextToHtml(string str)
        {
            return EyouSoft.Common.Function.StringValidate.TextToHtml(str);
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
    }
}
