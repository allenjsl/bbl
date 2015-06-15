using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;

namespace Web.print.wh
{
    /// <summary>
    /// 望海-团队确认单
    /// 功能：显示确认单
    /// 创建人：戴银柱
    /// </summary>
    public partial class TourConfirmPrint : BackPage
    {
        public int ci = 1;
        public int xi = 0;
        public int mi = 0;
        public int li = 0;
        protected DateTime? xcTime = null;
        protected string tourId = null;
        public int visitorRowsCount = 0;
        public int sListRowsCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //取Id
                tourId = Utils.GetQueryStringValue("tourid");
                //判断是否存在Id
                InitRouteInfo();
                //绑定数据
                BindXC();
                //绑定游客列表
                InitVisitorList();
            }
        }
        public string getDate(int day)
        {
            return xcTime.Value.AddDays(day).ToString("MM-dd");
        }

        public string getEat(string eat)
        {
            string returnValue = "";
            if (eat.Contains("2"))
            {
                returnValue += "早，";
            }
            if (eat.Contains("3"))
            {
                returnValue += "中，";
            }
            if (eat.Contains("4"))
            {
                returnValue += "晚，";
            }
            return returnValue.TrimEnd('，');
        }
        /// <summary>
        /// 判断是否有传值
        /// </summary>
        protected void InitRouteInfo()
        {
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour();

            EyouSoft.Model.TourStructure.TourTeamInfo tourInfo = bll.GetTourInfo(tourId) as EyouSoft.Model.TourStructure.TourTeamInfo;

            if (tourInfo == null)
            {
                Response.Clear();
                Response.Write("未找到信息");
                Response.End();
            }

        }

        /// <summary>
        /// 初始化游客信息列表
        /// </summary>
        protected void InitVisitorList()
        {
            EyouSoft.BLL.TourStructure.TourOrder order = new EyouSoft.BLL.TourStructure.TourOrder(base.SiteUserInfo);

            IList<EyouSoft.Model.TourStructure.TourOrderCustomer> list = order.GetTravellers(tourId).Where(x => x.CustomerStatus == EyouSoft.Model.EnumType.TourStructure.CustomerStatus.正常).ToList(); 

            if (list != null && list.Count > 0)
            {
                rptVisitorList.DataSource = list;
                rptVisitorList.DataBind();

                visitorRowsCount = list.Count;
            }

        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        void BindXC()
        {
            tabxcquick.Visible = false;
            tabProject.Visible = false;
            tabProject20.Visible = false;
            //声明bll对象
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            //声明散拼计划对象
            EyouSoft.Model.TourStructure.TourTeamInfo model = (EyouSoft.Model.TourStructure.TourTeamInfo)bll.GetTourInfo(Utils.GetQueryStringValue("tourId"));
            if (model != null)
            {

                #region 标准，快速 公有数据
                xcTime = model.LDate;
                lblGoTraffic.Text = model.LTraffic;
                lblEndTraffic.Text = model.RTraffic;
                lblTid.Text = model.TourCode;
                #endregion
                #region 组团社联系信息
                this.lblSetMan.Text = model.BuyerCName;
                EyouSoft.Model.CompanyStructure.CustomerInfo cusModel = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomerModel(model.BuyerCId);
                if (cusModel != null)
                {
                    //组团 联系人电话
                    this.lblSetPhone.Text = cusModel.Phone;
                    this.lblSetPhone2.Text = cusModel.Phone;
                    //组团联系人Fax
                    this.lblSetFAX.Text = cusModel.Fax;
                    this.lblSetMan.Text = cusModel.Name;
                    this.lblSetMan2.Text = cusModel.Name;
                    this.lblSetName.Text = cusModel.ContactName;

                }
                #endregion

                #region 专线
                EyouSoft.Model.CompanyStructure.ContactPersonInfo userInfoModel = new EyouSoft.BLL.CompanyStructure.CompanyUser().GetUserBasicInfo(model.OperatorId);
                if (userInfoModel != null)
                {
                    ////主要联系人电话
                    //this.lblGetMan.Text = userInfoModel.ContactTel;
                    ////专线FAX
                    //this.lblGetFAX.Text = userInfoModel.ContactFax;

                    EyouSoft.Model.CompanyStructure.CompanyInfo companyModel = new EyouSoft.BLL.CompanyStructure.CompanyInfo().GetModel(model.CompanyId, SiteUserInfo.SysId);
                    if (companyModel != null)
                    {
                        this.lblGetPhone.Text = companyModel.ContactTel;
                        this.lblGetPhone2.Text = companyModel.ContactTel;
                        this.lblGetMan.Text = companyModel.CompanyName;
                        this.lblGetMan2.Text = companyModel.CompanyName;
                        this.lblGetName.Text = companyModel.ContactName;
                        this.lblGetFAX.Text = companyModel.ContactFax;

                    }
                }
                #endregion

                if (model.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                {
                    #region 标准发布
                    if (model.TourNormalInfo != null)
                    {

                        this.lblLineName.Text = model.RouteName == "" ? "" : model.RouteName;
                        this.lblDaySum.Text = model.TourDays.ToString() == "" ? "0" : model.TourDays.ToString();
                        this.lblGoTraffic.Text = model.LTraffic;
                        this.lblEndTraffic.Text = model.RTraffic;
                        this.lblManSum.Text = model.PlanPeopleNumber.ToString();

                        if (model.TourNormalInfo.Plans != null && model.TourNormalInfo.Plans.Count > 0)
                        {
                            xc_list.DataSource = model.TourNormalInfo.Plans;
                            xc_list.DataBind();
                        }

                        lblBuHanXiangMu.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.BuHanXiangMu);
                        lblZiFeiXIangMu.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.ZiFeiXIangMu);
                        lblErTongAnPai.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.ErTongAnPai);
                        lblGouWuAnPai.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.GouWuAnPai);
                        lblZhuYiShiXiang.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourNormalInfo.ZhuYiShiXiang);
                        lblTotalAmount.Text = model.TotalAmount.ToString("#0.00") + "元";
                        lblGatheringPlace.Text = model.GatheringPlace;
                        lblGatheringTime.Text = model.GatheringTime;

                        if (model.Services != null && model.Services.Count > 0)
                        {
                            rpt_sList.DataSource = model.Services;
                            rpt_sList.DataBind();
                            sListRowsCount = model.Services.Count;
                        }

                        tabProject.Visible = true;
                        tabProject20.Visible = true;
                    }
                    #endregion


                }
                else
                {
                    #region 快速发布
                    tabxcquick.Visible = true;
                    this.lblLineName.Text = model.RouteName == "" ? "" : model.RouteName;
                    this.lblDaySum.Text = model.TourDays.ToString() == "" ? "0" : model.TourDays.ToString();
                    this.lblGoTraffic.Text = model.LTraffic;
                    this.lblEndTraffic.Text = model.RTraffic;
                    this.lblManSum.Text = model.PlanPeopleNumber.ToString();
                    if (model.TourQuickInfo != null)
                    {
                        lblKs.Text = EyouSoft.Common.Function.StringValidate.TextToHtml(model.TourQuickInfo.Remark);
                        lblQuickPlan.Text = model.TourQuickInfo.QuickPlan;
                    }
                    #endregion
                }
            }
            else
            {
                Response.Clear();
                Response.Write("未找到信息");
                Response.End();
            }
        }

    }
}
