using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
/////支出审批 by 田想兵 3.9
namespace Web.caiwuguanli
{
    /// <summary>
    /// 支出审批 by 田想兵 3.9
    /// </summary>
    public partial class waitkuanPass : Eyousoft.Common.Page.BackPage
    {
        /// <summary>
        /// 页面变量I
        /// </summary>
        public int i = 0;
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 审批、取消审批、支付、取消支付
            if (Utils.GetFormValue("requestType") == "ajax_cancel")
            {
                Response.Clear();
                if (SetRegisterStatus())
                {
                    Response.Write("0");
                }
                else
                {
                    Response.Write("-1");
                }
                Response.End();
            }
            #endregion

            if (!IsPostBack)
            {
                //权限判断
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_栏目))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款支出_栏目, false);
                }
                EyouSoft.Model.TourStructure.TourBaseInfo tourInfo = new EyouSoft.Model.TourStructure.TourBaseInfo();
                EyouSoft.BLL.TourStructure.Tour bllTour = new EyouSoft.BLL.TourStructure.Tour();
                tourInfo = bllTour.GetTourInfo(Utils.GetQueryStringValue("tourid"));
                if (tourInfo != null)
                {
                    lt_teamNum.Text = tourInfo.TourCode;
                    lt_xianluName.Text = tourInfo.RouteName;
                    lt_days.Text = tourInfo.TourDays.ToString();
                    lt_pepoleNum.Text = tourInfo.PeopleNumberShiShou.ToString();
                    lt_LevelDate.Text = tourInfo.LDate.ToString("yyyy-MM-dd");
                    //lt_seller.Text = tourInfo.SellerName;
                    //lt_oprator.Text=tourInfo.OperatorId
                    //EyouSoft.Model.CompanyStructure.ContactPersonInfo modelUser = new EyouSoft.BLL.CompanyStructure.CompanyUser(SiteUserInfo).GetUserBasicInfo(tourInfo.OperatorId);
                    //if (modelUser != null)
                    //{
                    //    lt_oprator.Text = modelUser.ContactName;
                    //}

                    EyouSoft.BLL.TourStructure.TourOrder orderbll = new EyouSoft.BLL.TourStructure.TourOrder();
                    EyouSoft.Model.TourStructure.TourOrder ordermodel = new EyouSoft.Model.TourStructure.TourOrder();

                    IList<EyouSoft.Model.StatisticStructure.StatisticOperator> listSeller = orderbll.GetSalerInfo(tourInfo.TourId);
                    IList<string> listOprator = bllTour.GetTourCoordinators(tourInfo.TourId);

                    string sellers = "";
                    string oprator = "";
                    foreach (var v in listSeller)
                    {
                        sellers += v.OperatorName + ",";
                    }
                    if (sellers.Length > 0)
                    {
                        sellers = sellers.TrimEnd(',');
                    }
                    foreach (var v in listOprator)
                    {
                        oprator += v + ",";
                    }
                    if (oprator.Length > 0)
                    {
                        oprator = oprator.TrimEnd(',');
                    }
                    lt_seller.Text = sellers;
                    lt_oprator.Text = oprator;
                }
                string planId = Utils.GetQueryStringValue("tourid");
                #region 列表绑定
                //收入
                this.rpt_list1.DataSource = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo).GetOrderList(SiteUserInfo.CompanyID, planId);
                this.rpt_list1.DataBind();

                //其它收入
                EyouSoft.Model.FinanceStructure.OtherCostQuery otherCostQueryModel = new EyouSoft.Model.FinanceStructure.OtherCostQuery();
                otherCostQueryModel.TourId = planId;
                this.rpt_list2.DataSource = new EyouSoft.BLL.FinanceStructure.OtherCost(SiteUserInfo).GetOtherIncomeList(otherCostQueryModel);
                this.rpt_list2.DataBind();

                //其它支出
                this.rpt_list3.DataSource = new EyouSoft.BLL.FinanceStructure.OtherCost(SiteUserInfo).GetOtherOutList(otherCostQueryModel);
                this.rpt_list3.DataBind();

                //利润分配
                this.rpt_list4.DataSource = new EyouSoft.BLL.FinanceStructure.TourProfitSharer(SiteUserInfo).GetTourShareList(planId);
                this.rpt_list4.DataBind();

                //
                rpt_list5.DataSource = new EyouSoft.BLL.FinanceStructure.OutRegister(SiteUserInfo).GetOutRegisterList(new EyouSoft.Model.FinanceStructure.QueryOutRegisterInfo { ReceiveId = Utils.GetQueryStringValue("ReceiveId"), ItemId= planId });
                rpt_list5.DataBind();

                rpt_list1.EmptyText = "<tr><td id=\"EmptyData\" colspan='7' bgcolor='#e3f1fc' height='50px' align='center'>暂时没有数据！</td></tr>";
                rpt_list2.EmptyText = "<tr><td id=\"EmptyData\" colspan='6' bgcolor='#e3f1fc' height='50px' align='center'>暂时没有数据！</td></tr>";
                rpt_list3.EmptyText = "<tr><td id=\"EmptyData\" colspan='6' bgcolor='#e3f1fc' height='50px' align='center'>暂时没有数据！</td></tr>";
                rpt_list4.EmptyText = "<tr><td id=\"EmptyData\" colspan='5' bgcolor='#e3f1fc' height='50px' align='center'>暂时没有数据！</td></tr>";

                #endregion
            }
        }
        /// <summary>
        /// 获取操作状态URL
        /// </summary>
        /// <param name="ischeck"></param>
        /// <param name="ispay"></param>
        /// <param name="id"></param>
        /// <param name="tourid"></param>
        /// <param name="ReceiveId"></param>
        /// <returns></returns>
        public string getUrl(bool ischeck, bool ispay, string id, string tourid, string ReceiveId)
        {
            if (!ispay)
            {
                if (ischeck)
                {
                    string s = string.Empty;//"已审核";

                    if (CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_取消审批))
                    {
                        s += string.Format("&nbsp;&nbsp;<a href=\"javascript:void(0)\" onclick=\"cancel(1,'{0}')\">取消审批</a>", id);
                    }

                    if (CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_财务支付))
                    {
                        s += string.Format("&nbsp;&nbsp;<a href=\"javascript:void(0)\" onclick=\"cancel(4,'{0}')\">确认支付</a>", id);
                    }

                    return s;
                }
                else
                {
                    //return "<a class=\"pass\" target=\"_top\" href=\"waitkuan.aspx?act=pass&id=" + id + "&tourid=" + tourid + "&ReceiveId=" + ReceiveId + "\">审批</a>";
                    string s = string.Empty;
                    if (CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_付款审批))
                    {
                        s = string.Format("<a href=\"javascript:void(0)\" onclick=\"cancel(3,'{0}')\">确认审批</a>", id);
                    }

                    return s;
                }
            }
            else
            {
                string s = string.Empty;//"已支付";

                if (CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_取消支付))
                {
                    s += string.Format("&nbsp;&nbsp;<a href=\"javascript:void(0)\" onclick=\"cancel(2,'{0}')\">取消支付</a>", id);
                }

                return s;
            }
        }
        /// <summary>
        /// 弹出窗
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.PageType = Eyousoft.Common.Page.PageType.boxyPage;
            base.OnPreInit(e);
        }

        /// <summary>
        /// 审批、取消审批、支付、取消支付
        /// </summary>
        /// <returns></returns>
        private bool SetRegisterStatus()
        {
            int requestTypeId = Utils.GetInt(Utils.GetFormValue("requestTypeId"));
            string registerId = Utils.GetFormValue("registerId");
            string planId = Utils.GetFormValue("planId");
            string tourId = Utils.GetFormValue("tourId");
            bool retCode = false;

            if (requestTypeId < 1 || string.IsNullOrEmpty(registerId) || string.IsNullOrEmpty(planId) || string.IsNullOrEmpty(tourId)) return false;

            switch (requestTypeId)
            {
                case 1://取消审批
                    if (CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_取消审批))
                        retCode = new EyouSoft.BLL.FinanceStructure.OutRegister(SiteUserInfo).SetCheckedState(false, SiteUserInfo.ID, registerId) == 1;
                    break;
                case 2://取消支付
                    if (CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_取消支付))
                        retCode = new EyouSoft.BLL.FinanceStructure.OutRegister(SiteUserInfo).SetIsPay(false, registerId) == 1;
                    break;
                case 3://审批
                    if (CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_付款审批))
                        retCode = new EyouSoft.BLL.FinanceStructure.OutRegister(SiteUserInfo).SetCheckedState(true, SiteUserInfo.ID, registerId) == 1;
                    break;
                case 4://支付
                    if (CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款支出_财务支付))
                        retCode = new EyouSoft.BLL.FinanceStructure.OutRegister(SiteUserInfo).SetIsPay(true, registerId) == 1;
                    break;
            }

            return retCode;
        }
    }
}
