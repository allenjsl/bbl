using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Eyousoft.Common.Page;

namespace Web.caiwuguanli
{
    /// <summary>
    /// 财务管理 机票审核
    /// 李晓欢
    /// 2011-05-12
    /// </summary>
    public partial class JipiaoValidate :BackPage
    {
        protected EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType? config_Agency;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_机票审核_财务审核))
                {
                    Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_机票审核_财务审核, false);
                }

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
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        //过滤小数点后的多余0
        public string FilterEndOfTheZeroDecimal(object o)
        {
            return Utils.FilterEndOfTheZeroString(o.ToString());
        }

        #region //航空公司绑定
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
#endregion

        #region
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
        #endregion

        #region 机票信息初始化
        private void onInit()
        {
            //证件类型绑定
            CusCardTypeBind();
            //航空公司绑定
            AirCompanyTypeBind();
            string id = Utils.GetQueryStringValue("id");
            if (id != "")
            {
                //机票申请编号
                this.hidRefundId.Value = id;
                
                EyouSoft.BLL.PlanStruture.PlaneTicket bll = new EyouSoft.BLL.PlanStruture.PlaneTicket();                                               
                EyouSoft.Model.PlanStructure.MCheckTicketInfo CheckTicket = bll.GetMCheckTicket(id);     
                
                if (CheckTicket != null)
                {
                    #region 航班
                    if (CheckTicket.TicketFlightList != null && CheckTicket.TicketFlightList.Count > 0)
                    {
                        RepAirList.DataSource = CheckTicket.TicketFlightList;
                        RepAirList.DataBind();
                    }
                    #endregion
                    
                    #region 机票状态
                    EyouSoft.Model.EnumType.PlanStructure.TicketState ticketState = CheckTicket.TicketState;
                    switch (ticketState)
                    {
                        case EyouSoft.Model.EnumType.PlanStructure.TicketState.机票申请:
                            {
                                this.PanClose.Visible = false;
                                this.Panquxiao.Visible = false;
                                this.PanShenhe.Visible = true;
                            }
                            break;
                        case EyouSoft.Model.EnumType.PlanStructure.TicketState.审核通过:
                            {
                                this.PanShenhe.Visible = false;
                                this.Panquxiao.Visible = true;
                                this.PanClose.Visible = false;
                            }
                            break;
                        case EyouSoft.Model.EnumType.PlanStructure.TicketState.已出票:
                            {
                                this.PanClose.Visible = true;
                                this.Panquxiao.Visible = false;
                                this.PanShenhe.Visible = false;
                            }
                            break;
                        default:
                            break;
                    }
                    #endregion

                    #region 名单
                    if (CheckTicket.CustomerList != null && CheckTicket.CustomerList.Count > 0)
                    {
                        this.RepCusList.DataSource = CheckTicket.CustomerList;
                        this.RepCusList.DataBind();
                    }
                    #endregion

                    #region 票款
                    if (CheckTicket.TicketKindList != null && CheckTicket.TicketKindList.Count > 0)
                    {
                        foreach (EyouSoft.Model.PlanStructure.TicketKindInfo TicketKindInfo in CheckTicket.TicketKindList)
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

                    #region  订单信息
                    EyouSoft.Model.PlanStructure.TicketOutListInfo TicketOutListInfo = new EyouSoft.BLL.PlanStruture.PlaneTicket().GetTicketModel(id);
                    if (TicketOutListInfo != null)
                    {
                        IList<EyouSoft.Model.TourStructure.OrderByCheckTicket> list = new EyouSoft.BLL.TourStructure.TourOrder().GetOrderByCheckTicketByOrderId(SiteUserInfo.CompanyID, TicketOutListInfo.OrderId);
                        if (list != null && list.Count > 0)
                        {
                            this.TicketOrderlist.DataSource = list;
                            this.TicketOrderlist.DataBind();
                        }
                    }
                    
                    #endregion
                    //总费用
                    this.txtSumMoney.Value = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(CheckTicket.TotalAmount.ToString()).ToString("0.00"));
                   
                    //财务审核备注
                    txt_remark.Text = CheckTicket.ReviewRemark;
                }
                CheckTicket = null;
                bll = null;
            }
        }
        #endregion

        #region 取消审核
        protected void lbtnquxiao_Click(object sender, EventArgs e)
        {
            string Ticketid = Utils.GetQueryStringValue("id");
            EyouSoft.BLL.PlanStruture.PlaneTicket PlaneTicket= new EyouSoft.BLL.PlanStruture.PlaneTicket();
            if (Ticketid != "" && !string.IsNullOrEmpty(Ticketid))
            {
                int Result = PlaneTicket.QuXiaoShenHe(Ticketid);
                //1:成功，-1:非审核通过状态下的机票申请不存在取消审核操作，-2:团队已提交财务不可取消审核
                switch (Result)
                {
                    case 1:
                        { Response.Write("<script>alert('已经取消审核!');window.parent.location.href='/caiwuguanli/JiPiaoAudit.aspx'</script>"); }
                        break;
                    case -1:
                        { Response.Write("<script>alert('非审核通过状态下的机票申请不存在取消审核操作');parent.Boxy.getIframeDialog('" + Request.QueryString["iframeid"] + "').hide();</script>"); }
                        break;
                    case -2:
                        { Response.Write("<script>alert('该计划已提交财务,不能取消审核!');parent.Boxy.getIframeDialog('" + Request.QueryString["iframeid"] + "').hide();</script>"); }
                        break;
                }
            }                      
        }
        #endregion
    }
}
