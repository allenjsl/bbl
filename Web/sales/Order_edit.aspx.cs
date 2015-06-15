using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using EyouSoft.Common;
using Eyousoft.Common.Page;
using System.Collections.Generic;
using EyouSoft.Model.TourStructure;

namespace Web.sales
{
    /// <summary>
    /// 订单详细编辑页面
    /// 修改记录：
    /// 1、2011-01-12 曹胡生 创建
    /// </summary>
    /// 修改记录：
    /// 2、2011-05-30 柴逸宁
    /// 修改备注：增加销售员、计调员字段
    /// 修改记录
    /// 3.2011-06-08 田想兵
    /// 修改备注：增加返佣和组团操作员
    /// 修改记录：
    /// 4.2011-6-30 柴逸宁
    /// 修改备注：添加游客列表序号
    /// 5.2011-7-5 柴逸宁
    /// 修改备注：在“不受理”状态，取消所有操作的可见
    /// 6.2011-7-13 柴逸宁
    /// 修改备注：添加各种合计，与详细人数，根据配置
    public partial class Order_edit : BackPage
    {
        protected int manNum, childNum, isXianFan;
        protected decimal tuituansunshi;
        //游客列表
        protected string cusHtml = "";
        //结算价列表
        protected string price = "";
        //订单ID
        private string OrderID = "";
        //本订单价格标准客户等级
        protected string PriceStandId = "";
        protected string CustomerLevId = "";
        protected bool IsRequiredTourCode = false;
        /// <summary>
        /// 是否单项服务
        /// </summary>
        protected bool isDXFW = true;
        //散拼计划报价标准
        System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> PriceList = null;
        //客户等级列表
        System.Collections.Generic.IList<EyouSoft.Model.CompanyStructure.CustomStand> CustomStandList = null;
        //报价标准列表
        System.Collections.Generic.IList<EyouSoft.Model.CompanyStructure.CompanyPriceStand> CompanyPriceStandList = null;
        /// <summary>
        /// 组团端操作人
        /// </summary>
        protected int BuyerContactId = 0;
        /// <summary>
        /// 返佣类型
        /// </summary>
        protected int CommissionType = 0;
        /// <summary>
        /// 佣金
        /// </summary>
        protected decimal CommissionPrice = 0;
        /// <summary>
        /// 组团操作人姓名
        /// </summary>
        protected string BuyerContactName = "";
        protected string strTraffic = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.销售管理_订单中心_订单修改))
            {
                Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.销售管理_订单中心_订单修改, false);
                return;
            }
            OrderID = EyouSoft.Common.Utils.GetQueryStringValue("id");
            if (!IsPostBack)
            {
                onInit();
            }

            #region 获取客户单位的责任销售
            if (Utils.GetQueryStringValue("act") == "getSeller")
            {
                Response.Clear();
                int comId = Utils.GetInt(Utils.GetQueryStringValue("comId"));
                EyouSoft.BLL.CompanyStructure.Customer custBll = new EyouSoft.BLL.CompanyStructure.Customer();
                EyouSoft.Model.CompanyStructure.CustomerInfo cusModel = custBll.GetCustomerModel(comId);
                if (cusModel != null)
                {
                    string saler = cusModel.Saler == null || cusModel.Saler == "" ? "暂无销售" : cusModel.Saler;
                    System.Text.StringBuilder jsonNameAndId = new System.Text.StringBuilder();
                    string userList = "";
                    jsonNameAndId.Append("[");
                    foreach (var vc in cusModel.CustomerContactList)
                    {
                        jsonNameAndId.Append("{\"Name\":\"" + vc.Name + "\",\"ID\":" + vc.ID + "},");
                    }
                    userList = jsonNameAndId.ToString().TrimEnd(',');
                    userList += "]";
                    Response.Write("[{\"saler\":\"" + saler + "\",\"salerid\":\"" + cusModel.SaleId + "\",cusList:" + userList + ",\"CommissionType\":\"" + ((int)cusModel.CommissionType).ToString() + "\",CommissionCount:" + Utils.FilterEndOfTheZeroDecimal(cusModel.CommissionCount) + "}]");

                }
                Response.End();
            }
            #endregion
        }

        /// <summary>
        /// 关联交通
        /// </summary>
        /// <param name="selTrafficId">选择的交通编号</param>
        /// <returns></returns>
        protected string GetSelectTraffic(int selTrafficId)
        {
            EyouSoft.BLL.PlanStruture.PlanTrffic BLL = new EyouSoft.BLL.PlanStruture.PlanTrffic();
            EyouSoft.Model.PlanStructure.TrafficSearch searchmodel = new EyouSoft.Model.PlanStructure.TrafficSearch();
            searchmodel.TourId = Utils.GetQueryStringValue("tourId");
            IList<EyouSoft.Model.PlanStructure.TrafficInfo> list = BLL.GetTrafficList(searchmodel, SiteUserInfo.CompanyID);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("<option value='' data-price='' data-shengyu='0'>请选择</option>");
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.TrafficId == selTrafficId)
                    {
                        sb.AppendFormat("<option value='{0}' selected='selected'  data-shengyu='0'>{1}</option>", item.TrafficId, item.TrafficName);
                    }
                    else
                    {
                        sb.AppendFormat("<option value='{0}'  data-shengyu='0'>{1}</option>", item.TrafficId, item.TrafficName);
                    }
                }
            }
            return sb.ToString();
        }


        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        //确认提交
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            #region 判断是否提交财务
            EyouSoft.Model.TourStructure.TourBaseInfo m = new EyouSoft.BLL.TourStructure.Tour().GetTourInfo(Utils.GetQueryStringValue("tourid"));
            if (m != null)
            {
                if (!Utils.PlanIsUpdateOrDelete(m.Status.ToString()))
                {
                    Response.Write("<script>alert('该团已提交财务，不能对它操作!');location.href=location.href;</script>");
                    return;
                }
            }
            #endregion
            OrderModify(false, DateTime.Now);
        }

        // 订单详细数据初始化
        private void onInit()
        {
            #region 订单数据初化
            if (OrderID != "")
            {
                EyouSoft.BLL.PlanStruture.PlaneTicket BLL = new EyouSoft.BLL.PlanStruture.PlaneTicket();
                EyouSoft.Model.PlanStructure.TicketOutListInfo TicketOutModel = BLL.GetTicketOutInfoByOrderId(OrderID);
                if (TicketOutModel != null)
                {
                    lbtnSubmit.Visible = false;
                    lbtnSeats.Visible = false;
                    LinkButton1.Visible = false;
                }
                new EyouSoft.BLL.TourStructure.TourOrder().GetDingDanTuiTuanRenShu(OrderID, out manNum, out childNum, out tuituansunshi);
                hd_manNum.Value = manNum.ToString();
                hd_childNum.Value = childNum.ToString();
                hd_tuituan.Value = tuituansunshi.ToString();
                EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder();
                EyouSoft.Model.TourStructure.TourOrder TourOrderModel = TourOrderBll.GetOrderModel(CurrentUserCompanyID, OrderID);
                hd_tourtype.Value = ((int)TourOrderModel.TourClassId).ToString();
                hd_xianfan.Value = ((int)TourOrderModel.CommissionType).ToString();
                System.Text.StringBuilder stringPrice = new System.Text.StringBuilder();
                if (TourOrderModel != null)
                {
                    EyouSoft.BLL.CompanyStructure.Customer custBll = new EyouSoft.BLL.CompanyStructure.Customer();
                    EyouSoft.Model.CompanyStructure.CustomerInfo cusModel = custBll.GetCustomerModel(TourOrderModel.BuyCompanyID);
                    if (cusModel != null)
                    {
                        IsRequiredTourCode = cusModel.IsRequiredTourCode;
                    }
                    //关联交通
                    strTraffic = GetSelectTraffic(TourOrderModel.OrderTrafficId);

                    #region 绑定全部计调  以注释
                    ////获取团队编号
                    //string tourid = TourOrderModel.TourId;
                    ////实例化计划中心BLL
                    //EyouSoft.BLL.TourStructure.Tour tBLL = new EyouSoft.BLL.TourStructure.Tour();
                    ////实例化Model
                    //EyouSoft.Model.TourStructure.TourBaseInfo tModel = new EyouSoft.Model.TourStructure.TourBaseInfo();
                    ////根据团队编号获取计划信息实体
                    //tModel = tBLL.GetTourInfo(tourid);
                    //if (tModel.AreaId != 0)
                    //{
                    //    EyouSoft.Model.CompanyStructure.Area aModel = new EyouSoft.BLL.CompanyStructure.Area().GetModel(tModel.AreaId);
                    //    if (aModel.AreaUserList != null)
                    //    {
                    //        IList<EyouSoft.Model.CompanyStructure.UserArea> list = aModel.AreaUserList;
                    //        ddlJDList.DataSource = list;
                    //        ddlJDList.DataBind();
                    //    }
                    //}
                    #endregion

                    #region 显示计调和销售
                    //获取团队编号
                    string tourid = TourOrderModel.TourId;
                    //订单状态
                    bool status = false;
                    if (tourid != null)
                    {
                        //实例化计划中心BLL
                        EyouSoft.BLL.TourStructure.Tour tBLL = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
                        //定义计调输出
                        string jdName = string.Empty;
                        //判定订单类型
                        switch ((int)TourOrderModel.TourClassId)
                        {
                            case (int)EyouSoft.Model.EnumType.TourStructure.TourType.团队计划:
                                EyouSoft.Model.TourStructure.TourTeamInfo ttModel = (EyouSoft.Model.TourStructure.TourTeamInfo)tBLL.GetTourInfo(tourid);
                                jdName = ttModel.Coordinator.Name;
                                break;
                            case (int)EyouSoft.Model.EnumType.TourStructure.TourType.单项服务:
                                //单项服务不显示计调
                                isDXFW = false;
                                break;
                            case (int)EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划:
                                EyouSoft.Model.TourStructure.TourInfo tModel = (EyouSoft.Model.TourStructure.TourInfo)tBLL.GetTourInfo(tourid);
                                jdName = tModel.Coordinator.Name;
                                status = (
                                    tModel.Status == EyouSoft.Model.EnumType.TourStructure.TourStatus.财务核算
                                    || tModel.Status == EyouSoft.Model.EnumType.TourStructure.TourStatus.核算结束);
                                break;
                        }
                        if (isDXFW)
                        {
                            //计调员赋值
                            lblJDY.Text = jdName;
                        }
                    }

                    //销售员赋值
                    lblXSY.Text = TourOrderModel.SalerName == null || TourOrderModel.SalerName.Trim() == "" ? "暂无销售" : TourOrderModel.SalerName.Trim();
                    hidSalerID.Value = TourOrderModel.SalerId.ToString();
                    hidSalerName.Value = TourOrderModel.SalerName;
                    #endregion
                    #region 配置
                    EyouSoft.BLL.CompanyStructure.CompanySetting setBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();//初始化bll

                    EyouSoft.Model.CompanyStructure.CompanyFieldSetting set = setBll.GetSetting(CurrentUserCompanyID);//配置实体
                    //游客信息是否必填
                    hd_IsRequiredTraveller.Value = set.IsRequiredTraveller.ToString();
                    //总结价配置
                    //int TeamNumberOfPeople = (int)setBll.GetTeamNumberOfPeople(SiteUserInfo.CompanyID);
                    int TeamNumberOfPeople = (int)set.TeamNumberOfPeople;
                    #endregion
                    #region 取消订单
                    if (TourOrderModel.TourClassId == EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划 && !status)
                    {
                        LinkButton1.Visible = true;
                    }
                    else
                        LinkButton1.Visible = false;
                    #endregion
                    if (TourOrderModel.OrderState == EyouSoft.Model.EnumType.TourStructure.OrderState.不受理)
                    {
                        tableroot.Visible = false;
                    }
                    #region 结算价绑定
                    //只有散拼有报价标准，单项服务与团队计划都没有报价标准
                    if (TourOrderModel.TourClassId == EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划)
                    {
                        EyouSoft.BLL.CompanyStructure.CompanyPriceStand CompanyPriceStandBll = new EyouSoft.BLL.CompanyStructure.CompanyPriceStand();
                        EyouSoft.BLL.CompanyStructure.CompanyCustomStand CompanyCustomStandBll = new EyouSoft.BLL.CompanyStructure.CompanyCustomStand();
                        EyouSoft.BLL.TourStructure.Tour tourBLl = new EyouSoft.BLL.TourStructure.Tour();
                        PriceList = tourBLl.GetPriceStandards(TourOrderModel.TourId);
                        CompanyPriceStandList = CompanyPriceStandBll.GetPriceStandByCompanyId(SiteUserInfo.CompanyID);
                        CustomStandList = CompanyCustomStandBll.GetCustomStandByCompanyId(SiteUserInfo.CompanyID);
                        for (int j = 0; j < CompanyPriceStandList.Count; j++)
                        {
                            if (j == 0)
                            {
                                stringPrice.Append("<tr><td align=\"center\">报价标准</td>");
                                for (int i = 0; i < CustomStandList.Count; i++)
                                {
                                    EyouSoft.Model.CompanyStructure.CustomStand CustomStand = CustomStandList[i];
                                    stringPrice.AppendFormat("<td><div style=\"text-align: center;\">{0}</div></td>", CustomStand.CustomStandName);
                                }
                                stringPrice.Append("</tr>");
                            }
                            EyouSoft.Model.CompanyStructure.CompanyPriceStand CompanyPriceStand = CompanyPriceStandList[j];
                            stringPrice.Append("<tr>");
                            stringPrice.AppendFormat("<td><div class=\"divPrice\" val=\"{0}\">", CompanyPriceStand.Id);
                            stringPrice.AppendFormat("<div style=\"text-align: center;\">{0}</div></td>", CompanyPriceStand.PriceStandName);
                            for (int i = 0; i < CustomStandList.Count; i++)
                            {
                                EyouSoft.Model.CompanyStructure.CustomStand CustomStand = CustomStandList[i];
                                stringPrice.Append("<td>");

                                stringPrice.AppendFormat("<input type=\"radio\" name=\"radio\" id=\"radio{0}\" value=\"{0}\" />", CustomStand.Id);
                                stringPrice.AppendFormat("成人价：<span name=\"sp_cr_price\">{0}</span><input type='hidden' name='hid_cr_price' value='{0}'/>", FilterEndOfTheZeroDecimal(GetPriceBy(CompanyPriceStand.Id, CustomStand.Id, true)));
                                stringPrice.AppendFormat("儿童价：<span name=\"sp_et_price\">{0}</span><input type='hidden' name='hid_et_price' value='{0}'/>", FilterEndOfTheZeroDecimal(GetPriceBy(CompanyPriceStand.Id, CustomStand.Id, false)));
                            }
                            stringPrice.Append("</td></tr>");
                        }
                        //人数（成人）
                        this.txtDdultCount.Text = TourOrderModel.AdultNumber.ToString();
                        //人数（儿童）
                        this.txtChildCount.Text = TourOrderModel.ChildNumber.ToString();

                    }
                    //如果该订单是团队计划订单，则不显示成人数与儿童数，显示总人数
                    else if (TourOrderModel.TourClassId == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
                    {
                        this.TeamPersonNumMaax.Visible = true;
                        this.lblTeamPersonNum.Enabled = false;
                        this.SanPingPersonNum.Visible = false;
                        if (TeamNumberOfPeople == (int)EyouSoft.Model.EnumType.CompanyStructure.TeamNumberOfPeople.PartNumber)
                        {
                            TeamPersonNumMin.Visible = false;

                            //结算价html
                            decimal unitAmountCr = 0;
                            decimal unitAmountEt = 0;
                            decimal unitAmountQp = 0;
                            int numberCr = 0;
                            int numberEt = 0;
                            int numberQp = 0;
                            if (TourOrderModel.TourTeamUnit != null)
                            {
                                unitAmountCr = TourOrderModel.TourTeamUnit.UnitAmountCr;//成人单价合计
                                unitAmountEt = TourOrderModel.TourTeamUnit.UnitAmountEt;//儿童单价合计
                                unitAmountQp = TourOrderModel.TourTeamUnit.UnitAmountQp;//全陪单价合计
                                numberCr = TourOrderModel.TourTeamUnit.NumberCr;//成人数
                                numberEt = TourOrderModel.TourTeamUnit.NumberEt;//儿童数
                                numberQp = TourOrderModel.TourTeamUnit.NumberQp;//全陪数
                            }
                            //人数（成人）
                            this.txtNumberCr.Text = numberCr.ToString();
                            //人数（儿童）
                            this.txtNumberEt.Text = numberEt.ToString();
                            //全陪数
                            this.txtNumberQp.Text = numberQp.ToString();
                            stringPrice.AppendFormat("<tr><td> &nbsp;成人单价合计：<input type=\"text\" name=\"UnitAmountCr\"  value=\"{0}\" onchange=\"groupSum()\" /></td>", unitAmountCr.ToString("0.00"));
                            stringPrice.AppendFormat("<td> &nbsp;儿童单价合计：<input type=\"text\" name=\"UnitAmountEt\"  value=\"{0}\" onchange=\"groupSum()\" /></td>", unitAmountEt.ToString("0.00"));
                            stringPrice.AppendFormat("<td> &nbsp;全陪单价合计：<input type=\"text\" name=\"UnitAmountQp\"  value=\"{0}\" onchange=\"groupSum()\" /></td></tr>", unitAmountQp.ToString("0.00"));
                        }
                        else
                        {
                            TeamPersonNumCount.Visible = false;
                        }
                    }


                    price = stringPrice.ToString();
                    PriceStandId = TourOrderModel.PriceStandId.ToString();
                    CustomerLevId = TourOrderModel.CustomerLevId.ToString();
                    #endregion
                    #region 订单基本数据
                    //线路名称
                    this.lblLineName.Text = TourOrderModel.RouteName;
                    //出团日期
                    this.hid_ChuTuanDate.Value = TourOrderModel.LeaveDate.ToShortDateString();
                    this.lblChuTuanDate.Text = TourOrderModel.LeaveDate.ToString("yyyy-MM-dd");
                    //当前空位
                    //this.lblCurFreePosi.Text = TourOrderModel.RemainNum.ToString();


                    //声明bll对象
                    EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
                    //声明散拼计划对象
                    EyouSoft.Model.TourStructure.TourBaseInfo model = bll.GetTourInfo(TourOrderModel.TourId);

                    this.lblCurFreePosi.Text = bll.GetShengYuRenShu(TourOrderModel.TourId).ToString();

                    if (model != null)
                    {
                        //出发交通
                        this.lblChuFanTra.Text = model.LTraffic;
                        //返回交通
                        this.lblBackTra.Text = model.RTraffic;
                    }
                    //联系人
                    this.txtContactName.Text = TourOrderModel.ContactName;
                    //电话
                    this.txtContactPhone.Text = TourOrderModel.ContactTel;
                    //手机
                    this.txtContactMobile.Text = TourOrderModel.ContactMobile;
                    //传真
                    this.txtContactFax.Text = TourOrderModel.ContactFax;
                    //特殊要求说明
                    this.txtSpecialRe.Text = TourOrderModel.SpecialContent;
                    //操作留言
                    this.txtOperMes.Text = TourOrderModel.OperatorContent;
                    //团队计划时显示总人数
                    this.lblTeamPersonNum.Text = TourOrderModel.PeopleNumber.ToString();
                    //总金额
                    this.txtTotalMoney.Text = Utils.FilterEndOfTheZeroDecimal(TourOrderModel.SumPrice);
                    //报价等级编号
                    this.hd_PriceStandId.Value = TourOrderModel.PriceStandId.ToString();
                    //客户等级编号
                    this.hd_LevelID.Value = TourOrderModel.CustomerLevId.ToString();
                    //成人价
                    this.hd_cr_price.Value = TourOrderModel.PersonalPrice.ToString();
                    //儿童价
                    this.hd_rt_price.Value = TourOrderModel.ChildPrice.ToString();
                    //线路ID
                    this.hd_lineID.Value = TourOrderModel.RouteId.ToString();
                    //购买公司ID
                    this.hd_BuyCompanyId.Value = TourOrderModel.BuyCompanyID.ToString();
                    //购买公司名称
                    txtGroupsName.Text = TourOrderModel.BuyCompanyName;
                    //团号ID
                    this.TourID.Value = TourOrderModel.TourId;
                    //团队或散拼
                    this.hd_TeamOrSanPing.Value = ((int)TourOrderModel.TourClassId).ToString();
                    //增减费用
                    if (TourOrderModel.AmountPlus != null)
                    {
                        this.addmoney.Text = Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(TourOrderModel.AmountPlus.AddAmount.ToString()).ToString("0.00"));
                        this.delmoney.Text = Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(TourOrderModel.AmountPlus.ReduceAmount.ToString()).ToString("0.00"));
                        this.remark.Value = TourOrderModel.AmountPlus.Remark;
                    }
                    if (!string.IsNullOrEmpty(TourOrderModel.CustomerFilePath))
                    {
                        this.hykCusFile.NavigateUrl = TourOrderModel.CustomerFilePath;
                        this.hykCusFile.Visible = true;
                    }


                    #region 返佣和对方操作员
                    BuyerContactId = TourOrderModel.BuyerContactId;
                    CommissionType = (int)TourOrderModel.CommissionType;
                    CommissionPrice = TourOrderModel.CommissionPrice;
                    BuyerContactName = TourOrderModel.BuyerContactName;
                    #endregion

                    txtBuyerTourCode.Value = TourOrderModel.BuyerTourCode;

                    #endregion
                    #region 订单游客数据
                    System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourOrderCustomer> curList = TourOrderModel.CustomerList;
                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    if (curList != null && curList.Count > 0)
                    {
                        for (int i = 0; i < curList.Count; i++)
                        {
                            if (curList[i].VisitorType == EyouSoft.Model.EnumType.TourStructure.VisitorType.成人)
                            {
                                stringBuilder.AppendFormat("<tr itemtype=\"{0}\">", "adult");
                            }
                            else if (curList[i].VisitorType == EyouSoft.Model.EnumType.TourStructure.VisitorType.儿童)
                            {
                                stringBuilder.AppendFormat("<tr itemtype=\"{0}\">", "child");
                            }
                            else
                            {
                                stringBuilder.AppendFormat("<tr itemtype=\"{0}\">", "other");
                            }
                            stringBuilder.AppendFormat("<td style=\"width: 5%\" bgcolor=\"#e3f1fc\" index=\"{0}\" align=\"center\">{0}</td><td height=\"25\" bgcolor=\"#e3f1fc\" align=\"center\">", i + 1);
                            stringBuilder.AppendFormat("<input type=\"text\" class=\"searchinput\" id=\"cusName\" MaxLength=\"50\" valid=\"required\" errmsg=\"请填写姓名!\" name=\"cusName\" value=\"{0}\" /></td>", curList[i].VisitorName);
                            stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");

                            #region 游客类型
                            if (curList[i].VisitorType == EyouSoft.Model.EnumType.TourStructure.VisitorType.成人)
                            {
                                stringBuilder.Append("<select disabled=\"disabled\" title=\"请选择\" id=\"cusType\" name=\"cusType\">");
                                stringBuilder.Append("<option value=\"\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\" selected=\"selected\">成人</option>");
                                stringBuilder.Append("<option value=\"2\">儿童</option>");
                                stringBuilder.Append(" </select>");
                            }
                            //儿童
                            else if (curList[i].VisitorType == EyouSoft.Model.EnumType.TourStructure.VisitorType.儿童)
                            {
                                stringBuilder.Append("<select disabled=\"disabled\" title=\"请选择\"  id=\"cusType\" name=\"cusType\">");
                                stringBuilder.Append("<option value=\"\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\" >成人</option>");
                                stringBuilder.Append("<option value=\"2\" selected=\"selected\">儿童</option>");
                                stringBuilder.Append(" </select>");
                            }
                            //其它
                            else
                            {
                                stringBuilder.Append("<select id=\"cusType\" title=\"请选择\"  name=\"cusType\">");
                                stringBuilder.Append("<option value=\"\"  selected=\"selected\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\" >成人</option>");
                                stringBuilder.Append("<option value=\"2\">儿童</option>");
                                stringBuilder.Append(" </select>");
                            }
                            #endregion

                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");

                            #region 游客证件类型
                            switch (curList[i].CradType)
                            {
                                case EyouSoft.Model.EnumType.TourStructure.CradType.身份证:
                                    {
                                        stringBuilder.Append("<select id=\"cusCardType\" name=\"cusCardType\">");
                                        stringBuilder.Append("<option value=\"0\">请选择证件</option>");
                                        stringBuilder.Append("<option value=\"1\" selected=\"selected\">身份证</option>");
                                        stringBuilder.Append("<option value=\"2\">护照</option>");
                                        stringBuilder.Append("<option value=\"3\">军官证</option>");
                                        stringBuilder.Append("<option value=\"4\">台胞证</option>");
                                        stringBuilder.Append("<option value=\"5\">港澳通行证</option>");
                                        stringBuilder.Append("<option value=\"6\">户口本</option>");
                                        stringBuilder.Append("</select>");
                                        break;
                                    }
                                case EyouSoft.Model.EnumType.TourStructure.CradType.护照:
                                    {
                                        stringBuilder.Append("<select id=\"cusCardType\" name=\"cusCardType\">");
                                        stringBuilder.Append("<option value=\"0\">请选择证件</option>");
                                        stringBuilder.Append("<option value=\"1\">身份证</option>");
                                        stringBuilder.Append("<option value=\"2\" selected=\"selected\">护照</option>");
                                        stringBuilder.Append("<option value=\"3\">军官证</option>");
                                        stringBuilder.Append("<option value=\"4\">台胞证</option>");
                                        stringBuilder.Append("<option value=\"5\">港澳通行证</option>");
                                        stringBuilder.Append("<option value=\"6\">户口本</option>");
                                        stringBuilder.Append("</select>");
                                        break;
                                    }
                                case EyouSoft.Model.EnumType.TourStructure.CradType.军官证:
                                    {
                                        stringBuilder.Append("<select id=\"cusCardType\" name=\"cusCardType\">");
                                        stringBuilder.Append("<option value=\"0\">请选择证件</option>");
                                        stringBuilder.Append("<option value=\"1\">身份证</option>");
                                        stringBuilder.Append("<option value=\"2\">护照</option>");
                                        stringBuilder.Append("<option value=\"3\" selected=\"selected\">军官证</option>");
                                        stringBuilder.Append("<option value=\"4\">台胞证</option>");
                                        stringBuilder.Append("<option value=\"5\">港澳通行证</option>");
                                        stringBuilder.Append("<option value=\"6\">户口本</option>");
                                        stringBuilder.Append("</select>");
                                        break;
                                    }
                                case EyouSoft.Model.EnumType.TourStructure.CradType.台胞证:
                                    {
                                        stringBuilder.Append("<select id=\"cusCardType\" name=\"cusCardType\">");
                                        stringBuilder.Append("<option value=\"0\">请选择证件</option>");
                                        stringBuilder.Append("<option value=\"1\">身份证</option>");
                                        stringBuilder.Append("<option value=\"2\">护照</option>");
                                        stringBuilder.Append("<option value=\"3\">军官证</option>");
                                        stringBuilder.Append("<option value=\"4\" selected=\"selected\">台胞证</option>");
                                        stringBuilder.Append("<option value=\"5\">港澳通行证</option>");
                                        stringBuilder.Append("<option value=\"6\">户口本</option>");
                                        stringBuilder.Append("</select>");
                                        break;
                                    }
                                case EyouSoft.Model.EnumType.TourStructure.CradType.港澳通行证:
                                    {
                                        stringBuilder.Append("<select id=\"cusCardType\" name=\"cusCardType\">");
                                        stringBuilder.Append("<option value=\"0\">请选择证件</option>");
                                        stringBuilder.Append("<option value=\"1\">身份证</option>");
                                        stringBuilder.Append("<option value=\"2\">护照</option>");
                                        stringBuilder.Append("<option value=\"3\">军官证</option>");
                                        stringBuilder.Append("<option value=\"4\">台胞证</option>");
                                        stringBuilder.Append("<option value=\"5\" selected=\"selected\">港澳通行证</option>");
                                        stringBuilder.Append("<option value=\"6\">户口本</option>");
                                        stringBuilder.Append("</select>");
                                        break;
                                    }
                                case EyouSoft.Model.EnumType.TourStructure.CradType.户口本:
                                    {
                                        stringBuilder.Append("<select id=\"cusCardType\" name=\"cusCardType\">");
                                        stringBuilder.Append("<option value=\"0\">请选择证件</option>");
                                        stringBuilder.Append("<option value=\"1\">身份证</option>");
                                        stringBuilder.Append("<option value=\"2\">护照</option>");
                                        stringBuilder.Append("<option value=\"3\">军官证</option>");
                                        stringBuilder.Append("<option value=\"4\">台胞证</option>");
                                        stringBuilder.Append("<option value=\"5\" >港澳通行证</option>");
                                        stringBuilder.Append("<option value=\"6\" selected=\"selected\">户口本</option>");
                                        stringBuilder.Append("</select>");
                                        break;
                                    }
                                default:
                                    {
                                        stringBuilder.Append("<select id=\"cusCardType\" name=\"cusCardType\">");
                                        stringBuilder.Append("<option value=\"0\" selected=\"selected\">请选择证件</option>");
                                        stringBuilder.Append("<option value=\"1\" >身份证</option>");
                                        stringBuilder.Append("<option value=\"2\">护照</option>");
                                        stringBuilder.Append("<option value=\"3\">军官证</option>");
                                        stringBuilder.Append("<option value=\"4\">台胞证</option>");
                                        stringBuilder.Append("<option value=\"5\">港澳通行证</option>");
                                        stringBuilder.Append("<option value=\"6\">户口本</option>");
                                        stringBuilder.Append("</select>");
                                        break;
                                    }
                            }
                            #endregion

                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");
                            stringBuilder.AppendFormat("<input type=\"text\" class=\"searchinput searchinput02\" id=\"cusCardNo\" MaxLength=\"150\" onblur='getSex(this)' name=\"cusCardNo\" value=\"{0}\">", curList[i].CradNumber);
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");

                            #region 游客性别
                            switch (curList[i].Sex)
                            {
                                case EyouSoft.Model.EnumType.CompanyStructure.Sex.男:
                                    {
                                        stringBuilder.Append("<select class='ddlSex' id=\"cusSex\" name=\"cusSex\">");
                                        stringBuilder.Append("<option value=\"0\">请选择</option>");
                                        stringBuilder.Append("<option value=\"1\" selected=\"selected\">男</option>");
                                        stringBuilder.Append("<option value=\"2\">女</option>");
                                        stringBuilder.Append("</select>");
                                        break;
                                    }
                                case EyouSoft.Model.EnumType.CompanyStructure.Sex.女:
                                    {
                                        stringBuilder.Append("<select class='ddlSex' id=\"cusSex\" name=\"cusSex\">");
                                        stringBuilder.Append("<option value=\"0\">请选择</option>");
                                        stringBuilder.Append("<option value=\"1\">男</option>");
                                        stringBuilder.Append("<option value=\"2\" selected=\"selected\">女</option>");
                                        stringBuilder.Append("</select>");
                                        break;
                                    }
                                default:
                                    {
                                        stringBuilder.Append("<select class='ddlSex' id=\"cusSex\" name=\"cusSex\">");
                                        stringBuilder.Append("<option value=\"0\" selected=\"selected\">请选择</option>");
                                        stringBuilder.Append("<option value=\"1\">男</option>");
                                        stringBuilder.Append("<option value=\"2\">女</option>");
                                        break;
                                    }
                            }

                            #endregion

                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");
                            stringBuilder.AppendFormat("<input type=\"text\" class=\"searchinput\" id=\"cusPhone\" MaxLength=\"50\" name=\"cusPhone\" value=\"{0}\">", curList[i].ContactTel);
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\"  width=\"6%\">");
                            if (curList[i].SpecialServiceInfo != null)
                            {
                                string str = string.Format("txtItem={0}&txtServiceContent={1}&txtCost={2}&ddlOperate={3}", curList[i].SpecialServiceInfo.ProjectName, curList[i].SpecialServiceInfo.ServiceDetail, curList[i].SpecialServiceInfo.Fee, (curList[i].SpecialServiceInfo.IsAdd.ToString() == "true" ? "1" : "0"));
                                stringBuilder.AppendFormat("<input id=\"spe{0}\" type=\"hidden\" name=\"specive\" value=\"{1}\" />", curList[i].ID, str);
                            }
                            else
                            {
                                stringBuilder.AppendFormat("<input id=\"spe{0}\" type=\"hidden\" name=\"specive\" value=\"\" />", curList[i].ID);
                            }
                            stringBuilder.AppendFormat("<a sign=\"speService\" href=\"javascript:void(0)\" onclick=\"OrderEdit.OpenSpecive('spe{0}')\">特服</a></td>", curList[i].ID);
                            stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\" width=\"15%\">");
                            stringBuilder.AppendFormat("<input type=\"hidden\" name=\"cusID\" value=\"{0}\" />", curList[i].ID);
                            stringBuilder.Append("<a sign=\"add\" href=\"javascript:void(0)\" onclick=\"OrderEdit.AddCus()\">添加</a>&nbsp;");
                            stringBuilder.Append("<input type=\"hidden\" name=\"cusState\" value=\"EDIT\" />");
                            string msg = "";
                            if (TourOrderBll.IsDoDelete(curList[i].ID, ref msg))
                            {
                                stringBuilder.Append("<a sign=\"del\" href=\"javascript:void(0)\" onclick=\"OrderEdit.DelCus($(this))\">删除</a></td></tr>");
                            }
                            else
                            {
                                stringBuilder.AppendFormat("<span>{0}</span>", msg);
                            }
                        }
                    }
                    cusHtml = stringBuilder.ToString();
                    if (TourOrderModel.TourClassId == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
                    {
                        txtTotalMoney.Enabled = false;
                        txtDdultCount.Enabled = false;
                        txtChildCount.Enabled = false;
                        this.lbtnSeats.Visible = false;
                    }
                    TourOrderBll = null;
                    TourOrderModel = null;
                    #endregion
                }
            }
            #endregion
        }

        //文件上传
        private string uploadFile()
        {
            string filePath = Server.MapPath("~/uploadFiles/VisitorInfoFile");
            string fileName = "";
            if (EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files[0], "VisitorInfoFile", out filePath, out fileName))
            {
                return filePath;
            }
            else
            {
                return "";
            }
        }

        //输出成功消息
        private void printSuccMsg(string msg)
        {
            EyouSoft.Common.Function.MessageBox.ResponseScript(this, string.Format(";alert('{0}');window.parent.Boxy.getIframeDialog('{1}').hide();{2}", msg, Utils.GetQueryStringValue("iframeId"), "window.parent.location.reload();"));
        }
        //输出失败消息
        private void printFaiMsg(string msg)
        {
            EyouSoft.Common.Function.MessageBox.Show(this, msg);
            onInit();
        }

        //判断数据输入是否正确
        private bool checkData(ref string msg)
        {
            if (Utils.GetInt(this.txtChildCount.Text, -1) == -1)
            {
                this.txtChildCount.Text = "0";
                return true;
            }
            if (Utils.GetInt(this.txtDdultCount.Text, -1) == -1)
            {
                this.txtDdultCount.Text = "0";
                return true;
            }

            if (Utils.GetDecimal(this.txtTotalMoney.Text, -1) == -1)
            {
                msg = "总金额输入有误";
                return false;
            }
            return true;
        }

        //过滤小数点后的多余0
        public string FilterEndOfTheZeroDecimal(object o)
        {
            return Utils.FilterEndOfTheZeroString(o.ToString());
        }

        //同意留位
        protected void btnYes_Click(object sender, EventArgs e)
        {

            #region 判断是否提交财务
            EyouSoft.Model.TourStructure.TourBaseInfo m = new EyouSoft.BLL.TourStructure.Tour().GetTourInfo(Utils.GetQueryStringValue("tourid"));
            if (m != null)
            {
                if (!Utils.PlanIsUpdateOrDelete(m.Status.ToString()))
                {
                    Response.Write("<script>alert('该团已提交财务，不能对它操作!');location.href=location.href;</script>");
                    return;
                }
            }
            #endregion
            //点同意留位，状态为已留位
            EyouSoft.BLL.CompanyStructure.CompanySetting setBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting setModel = setBll.GetSetting(SiteUserInfo.CompanyID);
            //留位时间
            DateTime seatDate = Utils.GetDateTime(txtEndTime.Text);
            if (setModel != null)
            {
                if (seatDate != DateTime.MinValue)
                {
                    if (seatDate > DateTime.Now.AddMinutes(Convert.ToDouble(setModel.ReservationTime)))
                    {
                        printFaiMsg(string.Format("留位时间最长到{0}", DateTime.Now.AddMinutes(setModel.ReservationTime)));
                        onInit();
                        return;
                    }
                }
                else
                {
                    printFaiMsg("留位时间输入错误!");
                    onInit();
                    return;
                }
            }
            EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder();
            EyouSoft.Model.TourStructure.TourOrder TourOrderModel = TourOrderBll.GetOrderModel(CurrentUserCompanyID, OrderID);
            if (TourOrderModel != null)
            {
                OrderModify(true, seatDate);
            }
            onInit();
            TourOrderBll = null;
            TourOrderModel = null;
        }

        //订单修改
        private void OrderModify(bool IsSeat, DateTime seatDate)
        {
            string msg = "";
            EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
            EyouSoft.Model.TourStructure.TourOrder TourOrderModel = new EyouSoft.Model.TourStructure.TourOrder();
            System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourOrderCustomer> curList = new System.Collections.Generic.List<EyouSoft.Model.TourStructure.TourOrderCustomer>();
            if (IsSeat)
            {
                TourOrderModel.OrderState = EyouSoft.Model.EnumType.TourStructure.OrderState.已留位;
                TourOrderModel.SaveSeatDate = seatDate;
            }
            else
            {
                TourOrderModel.OrderState = EyouSoft.Model.EnumType.TourStructure.OrderState.已成交;
            }
            if (checkData(ref msg))
            {
                int cusLength = Utils.GetFormValues("cusName").Length;
                for (int i = 0; i < cusLength; i++)
                {
                    //游客id
                    string cusID = Utils.GetFormValues("cusID")[i];
                    //游客姓名
                    string cusName = Utils.GetFormValues("cusName")[i];

                    if (cusName == "")
                    {
                        continue;
                    }
                    //游客证件名称
                    int cusCardType = Utils.GetInt(Utils.GetFormValues("cusCardType")[i]);
                    //游客证件号码
                    string cusCardNo = Utils.GetFormValues("cusCardNo")[i];
                    //游客性别
                    int cusSex = Utils.GetInt(Utils.GetFormValues("cusSex")[i]);
                    //游客联系电话
                    string cusPhone = Utils.GetFormValues("cusPhone")[i];
                    //游客类型
                    int cusType = Utils.GetInt(Utils.GetFormValues("cusType")[i]);
                    EyouSoft.Model.TourStructure.TourOrderCustomer cusModel = new EyouSoft.Model.TourStructure.TourOrderCustomer();
                    EyouSoft.Model.TourStructure.CustomerSpecialService specModel = new EyouSoft.Model.TourStructure.CustomerSpecialService();
                    if (string.IsNullOrEmpty(cusID))
                    {
                        cusModel.ID = System.Guid.NewGuid().ToString();
                    }
                    else
                    {
                        cusModel.ID = cusID;
                    }
                    cusModel.OrderId = TourOrderModel.ID;
                    cusModel.CompanyID = SiteUserInfo.CompanyID;
                    //特服
                    string specive = Utils.GetFormValues("specive")[i];
                    if (!string.IsNullOrEmpty(specive))
                    {
                        if (specive.LastIndexOf(',') + 1 == specive.Length)
                        {
                            specive = specive.Substring(0, specive.Length - 1);
                        }
                        specModel.CustormerId = cusModel.ID;
                        specModel.ProjectName = Utils.GetFromQueryStringByKey(specive, "txtItem");
                        specModel.ServiceDetail = Utils.GetFromQueryStringByKey(specive, "txtServiceContent");
                        specModel.IsAdd = Utils.GetFromQueryStringByKey(specive, "ddlOperate") == "1" ? true : false;
                        specModel.Fee = Utils.GetDecimal(Utils.GetFromQueryStringByKey(specive, "txtCost"));
                        //特服项目名不能为空，否则不添加该条特服信息
                        if (!string.IsNullOrEmpty(specModel.ProjectName))
                        {
                            cusModel.SpecialServiceInfo = specModel;
                        }
                    }
                    //添加到游客列表                   
                    cusModel.VisitorName = cusName;
                    if (Utils.GetFormValues("cusState")[i] == "DEL")
                    {
                        cusModel.IsDelete = true;
                    }
                    else
                    {
                        cusModel.IsDelete = false;
                    }
                    #region 游客证件类型
                    cusModel.CradType = (EyouSoft.Model.EnumType.TourStructure.CradType)cusCardType;
                    #endregion
                    cusModel.CradNumber = cusCardNo;
                    cusModel.ContactTel = cusPhone;
                    #region 游客类型
                    if (cusType == 1)
                    {
                        cusModel.VisitorType = EyouSoft.Model.EnumType.TourStructure.VisitorType.成人;
                    }
                    //儿童
                    else if (cusType == 2)
                    {
                        cusModel.VisitorType = EyouSoft.Model.EnumType.TourStructure.VisitorType.儿童;
                    }
                    //其它
                    else
                    {
                        printFaiMsg("请选择游客类型");
                        return;
                    }
                    #endregion
                    #region 性别
                    if (cusSex == 1)
                    {
                        cusModel.Sex = EyouSoft.Model.EnumType.CompanyStructure.Sex.男;
                    }
                    else if (cusSex == 2)
                    {
                        cusModel.Sex = EyouSoft.Model.EnumType.CompanyStructure.Sex.女;
                    }
                    else
                    {
                        cusModel.Sex = EyouSoft.Model.EnumType.CompanyStructure.Sex.未知;
                    }
                    #endregion
                    //添加到游客列表中
                    curList.Add(cusModel);
                }
                //特殊要求说明
                string specialRe = this.txtSpecialRe.Text;
                //操作留言
                string operMes = this.txtOperMes.Text;
                //总金额
                decimal totalMoney = Utils.GetDecimal(this.txtTotalMoney.Text);
                //报价标准编号
                PriceStandId = Utils.GetFormValue("hd_PriceStandId");
                //客户等级编号
                CustomerLevId = Utils.GetFormValue("hd_LevelID");
                //成人价
                decimal adultP = Utils.GetDecimal(Utils.GetFormValue("hd_cr_price"));
                //儿童价
                decimal childP = Utils.GetDecimal(Utils.GetFormValue("hd_rt_price"));

                TourOrderModel.ID = OrderID;
                TourOrderModel.SellCompanyId = this.SiteUserInfo.CompanyID;
                TourOrderModel.TourId = this.TourID.Value;
                TourOrderModel.SpecialContent = specialRe;
                TourOrderModel.OperatorContent = operMes;
                TourOrderModel.LastDate = DateTime.Now;
                TourOrderModel.LastOperatorID = SiteUserInfo.ID;
                TourOrderModel.SumPrice = totalMoney;
                if (EyouSoft.Model.EnumType.TourStructure.TourType.团队计划 == (EyouSoft.Model.EnumType.TourStructure.TourType)Utils.GetInt(this.hd_TeamOrSanPing.Value))
                {
                    TourOrderModel.TourClassId = EyouSoft.Model.EnumType.TourStructure.TourType.团队计划;
                    EyouSoft.BLL.CompanyStructure.CompanySetting setBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();//初始化bll
                    //获取配置
                    int TeamNumberOfPeople = (int)setBll.GetTeamNumberOfPeople(SiteUserInfo.CompanyID);
                    //仅总人数
                    if (TeamNumberOfPeople == (int)EyouSoft.Model.EnumType.CompanyStructure.TeamNumberOfPeople.OnlyTotalNumber)
                    {
                        TourOrderModel.ChildNumber = Utils.GetInt(Utils.GetFormValue(this.txtChildCount.UniqueID));
                        //团队计划 成人数 = 总人数
                        TourOrderModel.AdultNumber = Utils.GetInt(Utils.GetFormValue(this.lblTeamPersonNum.UniqueID));
                        TourOrderModel.PeopleNumber = Utils.GetInt(Utils.GetFormValue(this.lblTeamPersonNum.UniqueID));
                    }
                    else
                    {
                        msg = string.Empty;
                        if (Utils.GetIntSign(txtNumberCr.Text) < 0)
                        {
                            msg += "请输入正确的成人数！ \\n";
                        }
                        if (Utils.GetIntSign(txtNumberEt.Text) < 0)
                        {
                            msg += "请输入正确的儿童数！ \\n";
                        }
                        if (Utils.GetIntSign(txtNumberQp.Text) < 0)
                        {
                            msg += "请输入正确的全陪数！ \\n";
                        }
                        if (Utils.GetDecimal(Utils.GetFormValue("UnitAmountCr"), -1) < 0)
                        {
                            msg += "请输入正确的成人单价合计！ \\n";
                        }
                        if (Utils.GetDecimal(Utils.GetFormValue("UnitAmountEt"), -1) < 0)
                        {
                            msg += "请输入正确的儿童单价合计！ \\n";
                        }
                        if (Utils.GetDecimal(Utils.GetFormValue("UnitAmountQp"), -1) < 0)
                        {
                            msg += "请输入正确的全陪单价合计！ \\n";
                        }
                        if (msg.Length > 0)
                        {
                            TourOrderModel = null;
                            curList = null;
                            printFaiMsg(msg);
                            return;
                        }
                        else
                        {
                            MTourTeamUnitInfo model = new MTourTeamUnitInfo();
                            model.NumberCr = Utils.GetInt(txtNumberCr.Text);
                            model.NumberEt = Utils.GetInt(txtNumberEt.Text);
                            model.NumberQp = Utils.GetInt(txtNumberQp.Text);
                            model.UnitAmountCr = Utils.GetDecimal(Utils.GetFormValue("UnitAmountCr"));
                            model.UnitAmountEt = Utils.GetDecimal(Utils.GetFormValue("UnitAmountEt"));
                            model.UnitAmountQp = Utils.GetDecimal(Utils.GetFormValue("UnitAmountQp"));
                            TourOrderModel.TourTeamUnit = model;
                            TourOrderModel.PeopleNumber = model.NumberCr + model.NumberEt + model.NumberQp;
                        }

                    }
                }
                else
                {
                    TourOrderModel.ChildNumber = Utils.GetInt(Utils.GetFormValue(this.txtChildCount.UniqueID));
                    TourOrderModel.AdultNumber = Utils.GetInt(Utils.GetFormValue(this.txtDdultCount.UniqueID));
                    TourOrderModel.PeopleNumber = TourOrderModel.ChildNumber + TourOrderModel.AdultNumber;
                }
                TourOrderModel.ContactName = this.txtContactName.Text;
                TourOrderModel.ContactTel = this.txtContactPhone.Text;
                TourOrderModel.ContactMobile = this.txtContactMobile.Text;
                TourOrderModel.ContactFax = this.txtContactFax.Text;
                TourOrderModel.PersonalPrice = adultP;
                TourOrderModel.ChildPrice = childP;
                TourOrderModel.PriceStandId = Utils.GetInt(PriceStandId);
                TourOrderModel.CustomerLevId = Utils.GetInt(CustomerLevId);
                TourOrderModel.SaveSeatDate = seatDate;
                TourOrderModel.LastDate = DateTime.Now;
                TourOrderModel.LastOperatorID = SiteUserInfo.ID;
                TourOrderModel.SuccessTime = DateTime.Now;
                TourOrderModel.RouteId = Utils.GetInt(this.hd_lineID.Value);
                TourOrderModel.CustomerFilePath = uploadFile();
                TourOrderModel.CustomerDisplayType = String.IsNullOrEmpty(TourOrderModel.CustomerFilePath) ? EyouSoft.Model.EnumType.TourStructure.CustomerDisplayType.输入方式 : EyouSoft.Model.EnumType.TourStructure.CustomerDisplayType.附件方式;
                TourOrderModel.CustomerList = curList;
                TourOrderModel.AmountPlus = new EyouSoft.Model.TourStructure.TourOrderAmountPlusInfo();
                TourOrderModel.AmountPlus.AddAmount = Utils.GetDecimal(this.addmoney.Text);
                TourOrderModel.AmountPlus.ReduceAmount = Utils.GetDecimal(this.delmoney.Text);
                TourOrderModel.AmountPlus.Remark = this.remark.Value;
                #region 返佣和对方操作员
                TourOrderModel.BuyerContactId = Utils.GetInt(Utils.GetFormValue("otherOprator"));
                TourOrderModel.BuyerContactName = Utils.InputText(Utils.GetFormValue("hd_orderOprator"));
                TourOrderModel.CommissionType = (EyouSoft.Model.EnumType.CompanyStructure.CommissionType)Utils.GetInt(Utils.GetFormValue("hd_rebateType"));
                TourOrderModel.CommissionPrice = Utils.GetDecimal(Utils.GetFormValue("txt_Rebate"));
                #endregion
                //组团社的责任销售员编号
                TourOrderModel.SalerId = Utils.GetInt(Utils.GetFormValue("hidSalerID"));
                //组团社的责任销售员名字
                TourOrderModel.SalerName = Utils.GetFormValue("hidSalerName");
                //购买公司编号
                TourOrderModel.BuyCompanyID = Utils.GetInt(Utils.GetFormValue("hd_BuyCompanyId"));
                //购买公司名称
                TourOrderModel.BuyCompanyName = Utils.GetFormValue("txtGroupsName");
                TourOrderModel.BuyerTourCode = Utils.GetFormValue(txtBuyerTourCode.ClientID);
                TourOrderModel.OrderTrafficId = Utils.GetInt(Utils.GetFormValue("selectTraffic"));
                switch (TourOrderBll.Update(TourOrderModel))
                {
                    case 0:
                        {
                            TourOrderModel = null;
                            curList = null;
                            printFaiMsg("修改失败");
                            break;
                        }
                    case 1:
                        {
                            TourOrderModel = null;
                            curList = null;
                            printSuccMsg("修改成功");
                            break;
                        }
                    case 2:
                        {
                            TourOrderModel = null;
                            curList = null;
                            printFaiMsg("该团队的订单总人数+当前订单人数大于团队计划人数总和");
                            break;
                        }
                    case 3:
                        {
                            TourOrderModel = null;
                            curList = null;
                            printFaiMsg("该客户所欠金额大于最高欠款金额");
                            break;
                        }
                    case 4:
                        {
                            TourOrderModel = null;
                            curList = null;
                            printFaiMsg("订单人数加上交通出团日期当天已使用票数大于交通出团日期当天人数不允许修改");
                            break;
                        }
                }
            }
            else
            {
                TourOrderModel = null;
                curList = null;
                printFaiMsg(msg);
            }
        }

        /// <summary>
        /// 描述：根据报价标准ID与客户等级ID获得相应价格
        /// </summary>
        /// <param name="StandId">报价标准ID</param>
        /// <param name="LevelId">客户等级ID</param>
        /// <param name="CorD">成人或儿童</param>
        /// <returns></returns>
        public decimal GetPriceBy(int StandId, int LevelId, bool CorD)
        {
            decimal price = 0;
            for (int i = 0; i < PriceList.Count; i++)
            {
                EyouSoft.Model.TourStructure.TourPriceStandardInfo PriceStand = PriceList[i];
                if (PriceStand.StandardId == StandId)
                {
                    foreach (EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo CompanyPriceStand in PriceList[i].CustomerLevels)
                    {
                        if (CompanyPriceStand.LevelId == LevelId)
                        {
                            price = CorD ? CompanyPriceStand.AdultPrice : CompanyPriceStand.ChildrenPrice;
                        }
                    }
                }
            }
            return price;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            #region 判断是否提交财务
            EyouSoft.Model.TourStructure.TourBaseInfo m = new EyouSoft.BLL.TourStructure.Tour().GetTourInfo(Utils.GetQueryStringValue("tourid"));
            if (m != null)
            {
                if (!Utils.PlanIsUpdateOrDelete(m.Status.ToString()))
                {
                    Response.Write("<script>alert('该团已提交财务，不能对它操作!');location.href=location.href;</script>");
                    return;
                }
            }
            #endregion
            EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
            EyouSoft.Model.TourStructure.TourOrder odermodel = TourOrderBll.GetOrderModel(CurrentUserCompanyID, OrderID);
            bool b = TourOrderBll.CancelOrder(CurrentUserCompanyID, OrderID, odermodel.TourId);
            if (b)
            {
                printSuccMsg("修改成功");
            }
            else
            {

                printFaiMsg("修改失败");
            }

        }
    }
}
