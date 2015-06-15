using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using EyouSoft.Common;
using Eyousoft.Common.Page;

namespace Web
{
    /// <summary>
    /// 机票审核查看页面
    /// 修改记录：
    /// 1、2011-01-12 曹胡生 创建
    /// </summary>
    public partial class JiPiaoAuditShow : BackPage
    {
        protected EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType? config_Agency;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                onInit();

                #region 获得该公司的代理费计算方式
                config_Agency = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetAgencyFee(SiteUserInfo.CompanyID);
                switch (config_Agency)
                {
                    case EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式一:
                        {
                            this.lblMsgFrist.Text = "(票面价+税/机建)*人数+代理费)";
                            this.lblMsgSecond.Text = "(票面价+税/机建)*人数+代理费)";
                        } break;
                    case EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式二:
                        {
                            this.lblMsgFrist.Text = "(票面价+税/机建)*人数-代理费)";
                            this.lblMsgSecond.Text = "(票面价+税/机建)*人数-代理费)";
                        } break;
                    case EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式三:
                        {
                            this.lblMsgFrist.Text = "(票面价*百分比+机建燃油)*人数+其它费用";
                            this.lblMsgSecond.Text = "(票面价*百分比+机建燃油)*人数+其它费用";
                        } break;
                    default:
                        {
                            this.lblMsgFrist.Text = "(票面价+税/机建)*人数+代理费)";
                            this.lblMsgSecond.Text = "(票面价+税/机建)*人数+代理费)";
                        } break;
                }
                hideDoType.Value = config_Agency.HasValue ? config_Agency.Value.ToString() : "";
                #endregion
            }

            if (CheckGrant(global::Common.Enum.TravelPermission.机票管理_机票管理_取消出票)) lbtnCancelTicket.Visible = true;
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        private void onInit()
        {
            BindPayType();
            //证件类型绑定
            CusCardTypeBind();
            //航空公司绑定
            AirCompanyTypeBind();
            string id = Utils.GetQueryStringValue("id");
            if (id != "")
            {
                EyouSoft.BLL.PlanStruture.PlaneTicket bll = new EyouSoft.BLL.PlanStruture.PlaneTicket();
                EyouSoft.Model.PlanStructure.TicketOutListInfo model = bll.GetTicketOutListModel(id);
                if (model != null)
                {
                    #region 航班
                    if (model.TicketFlightList != null && model.TicketFlightList.Count > 0)
                    {
                        RepAirList.DataSource = model.TicketFlightList;
                        RepAirList.DataBind();
                    }
                    #endregion

                    #region 名单
                    if (model.CustomerInfoList != null && model.CustomerInfoList.Count > 0)
                    {
                        this.RepCusList.DataSource = model.CustomerInfoList;
                        this.RepCusList.DataBind();
                    }
                    #endregion

                    #region 票款
                    if (model.TicketKindInfoList != null && model.TicketKindInfoList.Count > 0)
                    {
                        foreach (EyouSoft.Model.PlanStructure.TicketKindInfo TicketKindInfo in model.TicketKindInfoList)
                        {
                            #region 成人
                            if (TicketKindInfo.TicketType == EyouSoft.Model.EnumType.PlanStructure.KindType.成人)
                            {
                                //票面价
                                this.txtAdultPrice.Value = FilterEndOfTheZeroDecimal(TicketKindInfo.Price);
                                //税/机建
                                this.txtAdultShui.Value = FilterEndOfTheZeroDecimal(TicketKindInfo.OilFee);
                                //人数
                                this.txtAdultCount.Value = TicketKindInfo.PeopleCount.ToString();
                                //代理费
                                this.txtAdultProxyPrice.Value = FilterEndOfTheZeroDecimal(TicketKindInfo.AgencyPrice);
                                //票款
                                this.txtAdultSum.Value = FilterEndOfTheZeroDecimal(TicketKindInfo.TotalMoney);
                                //百分比
                                txt_Percent.Value = FilterEndOfTheZeroDecimal(TicketKindInfo.Discount);
                                txt_DaiLiFei.Value = FilterEndOfTheZeroDecimal(TicketKindInfo.OtherPrice);
                            }
                            #endregion

                            #region 儿童
                            if (TicketKindInfo.TicketType == EyouSoft.Model.EnumType.PlanStructure.KindType.儿童)
                            {
                                //票面价
                                this.txtChildPrice.Value = FilterEndOfTheZeroDecimal(TicketKindInfo.Price);
                                //税/机建
                                this.txtChildShui.Value = FilterEndOfTheZeroDecimal(TicketKindInfo.OilFee);
                                //人数
                                this.txtChildCount.Value = TicketKindInfo.PeopleCount.ToString();
                                //代理费
                                this.txtChildProxyPrice.Value = FilterEndOfTheZeroDecimal(TicketKindInfo.AgencyPrice);
                                //票款
                                this.txtChildSum.Value = FilterEndOfTheZeroDecimal(TicketKindInfo.TotalMoney);
                                txt_Percent2.Value = FilterEndOfTheZeroDecimal(TicketKindInfo.Discount);
                                txt_DaiLiFei2.Value = FilterEndOfTheZeroDecimal(TicketKindInfo.OtherPrice);
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region 基本信息
                    //总费用
                    this.txtSumMoney.Value = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(model.TotalAmount.ToString()).ToString("0.00"));
                    //支付方式
                    this.ddlPayType.SelectedValue = model.PayType.ToString();
                    //订票须知
                    this.txtOrderPiaoMust.Value = model.Notice;
                    //PNR
                    this.txtPNR.Value = model.PNR;
                    //售票处
                    this.txtSalePlace.Value = model.TicketOffice;
                    //票号
                    this.txtPiaoHao.Value = model.TicketNum;
                    //备注
                    this.txtMemo.Value = model.Remark;
                    if (model.PayType.ToString() != "0" && model.PayType.ToString() != "")
                    {
                        this.ddlPayType.Items.FindByText(model.PayType.ToString()).Selected = true;
                    }
                    //订单ID
                    this.hdOrderID.Value = model.OrderId;
                    //团号ID
                    this.hdTourID.Value = model.TourId;
                    #endregion
                }
                bll = null;
                model = null;
            }
        }

        //绑定支付方式
        private void BindPayType()
        {
            System.Collections.Generic.List<EnumObj> list = EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.RefundType));
            this.ddlPayType.DataTextField = "Text";
            this.ddlPayType.DataValueField = "Value";
            this.ddlPayType.DataSource = list;
            this.ddlPayType.DataBind();
            this.ddlPayType.Items.Insert(0, "请选择");
        }

        //证件类型绑定
        private void CusCardTypeBind()
        {
            string cardType = "";
            System.Collections.Generic.List<EnumObj> list = EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.CradType));
            if (list != null && list.Count > 0)
            {
                cardType += "{value:\"-1\",text:\"--请选择--\"}|";
                for (int i = 0; i < list.Count; i++)
                {
                    cardType += "{value:\"" + list[i].Value + "\",text:\"" + list[i].Text + "\"}|";
                }
                cardType = cardType.TrimEnd('|');
            }
            this.hd_cardType.Value = cardType;
        }

        //航空公司绑定
        private void AirCompanyTypeBind()
        {
            string airType = "";
            System.Collections.Generic.List<EnumObj> list = EyouSoft.Common.EnumObj.GetList(typeof(EyouSoft.Model.EnumType.PlanStructure.FlightCompany));
            if (list != null && list.Count > 0)
            {
                airType += "{value:\"-1\",text:\"--请选择--\"}|";
                for (int i = 0; i < list.Count; i++)
                {
                    airType += "{value:\"" + list[i].Value + "\",text:\"" + list[i].Text + "\"}|";
                }
                airType = airType.TrimEnd('|');
            }
            this.hd_airCompany.Value = airType;
        }


        //过滤小数点后的多余0
        public string FilterEndOfTheZeroDecimal(object o)
        {
            return Utils.FilterEndOfTheZeroString(o.ToString());
        }

        /// <summary>
        /// 取消出票按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnCancelTicket_Click(object sender, EventArgs e)
        {
            string planId = Utils.GetQueryStringValue("id");

            EyouSoft.BLL.PlanStruture.PlaneTicket bll = new EyouSoft.BLL.PlanStruture.PlaneTicket();

            if (bll.CancelTicket(planId) == 1)
            {
                Utils.ShowMsgAndCloseBoxy("取消机票成功", Utils.GetQueryStringValue("iframeId"), true);
            }

            bll = null;
        }
    }
}
