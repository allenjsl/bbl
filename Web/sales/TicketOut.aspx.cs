using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eyousoft.Common.Page;
using EyouSoft.Common;

namespace Web.sales
{
    public partial class TicketOut : BackPage
    {
        /// <summary>
        /// 计算公式
        /// </summary>
        protected EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType? config_Agency;
        /// <summary>
        /// 总金额
        /// </summary>
        protected string Total = string.Empty;
        /// <summary>
        /// 支付方式选项
        /// </summary>
        protected string selectOptions = string.Empty;

        public string piaomianjia = string.Empty,
            shui = string.Empty,
            pepoleNum = string.Empty,
            DaiLiFei = string.Empty,
            piaokuan = string.Empty,
            piaomianjia2 = string.Empty,
            shui2 = string.Empty,
            pepoleNum2 = string.Empty,
            DaiLiFei2 = string.Empty,
            piaokuan2 = string.Empty,
            OtherMoney = string.Empty,
            Percent = string.Empty,
            OtherMoney2 = string.Empty,
            Percent2 = string.Empty,
            PNR = string.Empty,
            Remark = string.Empty,
            Notice = string.Empty,
            orderId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            config_Agency = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetAgencyFee(SiteUserInfo.CompanyID);
            if (!IsPostBack)
            {
                #region 获得该公司的代理费计算方式

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
            PageInit();

        }

        protected void PageInit()
        {
            IList<EnumObj> list = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.TourStructure.RefundType));
            if (list != null && list.Count > 0)
            {
                this.ddl_PayType.DataSource = list;
                this.ddl_PayType.DataTextField = "Text";
                this.ddl_PayType.DataValueField = "Value";
                this.ddl_PayType.DataBind();
            }
            orderId = Utils.GetQueryStringValue("id");
            if (!string.IsNullOrEmpty(orderId))
            {
                EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder();
                EyouSoft.Model.TourStructure.TourOrder TourOrderModel = TourOrderBll.GetOrderModel(CurrentUserCompanyID, orderId);
                if (TourOrderModel != null)
                {
                    pepoleNum = TourOrderModel.AdultNumber.ToString();
                    pepoleNum2 = TourOrderModel.ChildNumber.ToString();
                    hidRouteName.Value = TourOrderModel.RouteName;
                    hidOrderId.Value = TourOrderModel.ID;
                }

                EyouSoft.BLL.PlanStruture.PlaneTicket BLL = new EyouSoft.BLL.PlanStruture.PlaneTicket();
                EyouSoft.Model.PlanStructure.TicketOutListInfo model = BLL.GetTicketOutInfoByOrderId(orderId);
                hidIsExtsisTicket.Value = model != null ? "1" : "0";
                if (model != null)
                {
                    piaomianjia = Utils.FilterEndOfTheZeroDecimal(model.TicketKindInfoList.FirstOrDefault().Price);
                    shui = Utils.FilterEndOfTheZeroDecimal(model.TicketKindInfoList.FirstOrDefault().OilFee);
                    pepoleNum = model.TicketKindInfoList.FirstOrDefault().PeopleCount.ToString();
                    DaiLiFei = Utils.FilterEndOfTheZeroDecimal(model.TicketKindInfoList.FirstOrDefault().AgencyPrice);
                    Percent = Utils.FilterEndOfTheZeroDecimal(model.TicketKindInfoList.FirstOrDefault().Discount);
                    OtherMoney = Utils.FilterEndOfTheZeroDecimal(model.TicketKindInfoList.FirstOrDefault().OtherPrice);
                    piaokuan = Utils.FilterEndOfTheZeroDecimal(model.TicketKindInfoList.FirstOrDefault().TotalMoney);
                    if (model.TicketKindInfoList.Count > 1)
                    {
                        piaomianjia2 = Utils.FilterEndOfTheZeroDecimal(model.TicketKindInfoList[1].Price);
                        shui2 = Utils.FilterEndOfTheZeroDecimal(model.TicketKindInfoList[1].OilFee);
                        pepoleNum2 = model.TicketKindInfoList[1].PeopleCount.ToString();
                        DaiLiFei2 = Utils.FilterEndOfTheZeroDecimal(model.TicketKindInfoList[1].AgencyPrice);
                        Percent2 = Utils.FilterEndOfTheZeroDecimal(model.TicketKindInfoList[1].Discount);
                        OtherMoney2 = Utils.FilterEndOfTheZeroDecimal(model.TicketKindInfoList[1].OtherPrice);
                        piaokuan2 = Utils.FilterEndOfTheZeroDecimal(model.TicketKindInfoList[1].TotalMoney);
                    }

                    Total = Utils.FilterEndOfTheZeroDecimal(model.Total);
                    PNR = model.PNR;
                    ddl_PayType.SelectedIndex = (int)model.PayType;
                    Remark = model.Remark;
                    Notice = model.Notice;
                    this.hideId.Value = model.TicketOutId;
                    if ((int)model.State >= 2)
                    {
                        lbtn_submit.Visible = false;
                    }
                }
            }
        }


        /// <summary>
        /// 提交事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtn_submit_Click(object sender, EventArgs e)
        {
            EyouSoft.Model.PlanStructure.TicketOutListInfo model = new EyouSoft.Model.PlanStructure.TicketOutListInfo();
            model.TourId = Utils.GetQueryStringValue("tourId");
            model.CompanyID = this.SiteUserInfo.CompanyID;
            model.OperateID = SiteUserInfo.ID;
            if (SiteUserInfo.ContactInfo != null)
            {
                model.Operator = SiteUserInfo.ContactInfo.ContactName;
            }
            model.OrderId = orderId;
            #region 成人儿童票款
            IList<EyouSoft.Model.PlanStructure.TicketKindInfo> ltki = new
                    List<EyouSoft.Model.PlanStructure.TicketKindInfo>();
            ////成人票款
            EyouSoft.Model.PlanStructure.TicketKindInfo tki = new EyouSoft.Model.PlanStructure.TicketKindInfo();
            tki.OilFee = Utils.GetDecimal(Utils.GetFormValue("txt_shui"));
            tki.PeopleCount = Utils.GetInt(Utils.GetFormValue("txt_pepoleNum"));
            tki.Price = Utils.GetDecimal(Utils.GetFormValue("txt_piaomianjia"));
            tki.TotalMoney = Utils.GetDecimal(Utils.GetFormValue("txt_piaokuan"));
            tki.TicketType = EyouSoft.Model.EnumType.PlanStructure.KindType.成人;
            if (config_Agency.HasValue && EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式三 == config_Agency.Value)
            {
                tki.OtherPrice = Utils.GetDecimal(Utils.GetFormValue("txt_DaiLiFei"));
                tki.Discount = Utils.GetDecimal(Utils.GetFormValue("txt_Percent"), 100) / 100;
                tki.AgencyPrice = 0;
            }
            else
            {
                tki.OtherPrice = 0;
                tki.Discount = 1;
                tki.AgencyPrice = Utils.GetDecimal(Utils.GetFormValue("txt_DaiLiFei"));
            }
            ltki.Add(tki);
            ///////儿童票款
            EyouSoft.Model.PlanStructure.TicketKindInfo tkirt = new EyouSoft.Model.PlanStructure.TicketKindInfo();
            tkirt.AgencyPrice = Utils.GetDecimal(Utils.GetFormValue("txt_DaiLiFei2"));
            tkirt.OilFee = Utils.GetDecimal(Utils.GetFormValue("txt_shui2"));
            tkirt.PeopleCount = Utils.GetInt(Utils.GetFormValue("txt_pepoleNum2"));
            tkirt.Price = Utils.GetDecimal(Utils.GetFormValue("txt_piaomianjia2"));
            tkirt.TotalMoney = Utils.GetDecimal(Utils.GetFormValue("txt_piaokuan2"));
            tkirt.TicketType = EyouSoft.Model.EnumType.PlanStructure.KindType.儿童;
            tkirt.OtherPrice = Utils.GetDecimal(Utils.GetFormValue("txt_DaiLiFei2"));
            tkirt.Discount = Utils.GetDecimal(Utils.GetFormValue("txt_Percent2"), 100) / 100;
            ltki.Add(tkirt);
            #endregion
            model.TicketKindInfoList = ltki;
            model.PNR = Utils.GetFormValue("txt_PNR");
            model.Total = Utils.GetDecimal(Utils.GetFormValue("txt_SumPrice"));
            model.PayType = (EyouSoft.Model.EnumType.TourStructure.RefundType)Utils.GetInt(Utils.GetFormValue(ddl_PayType.UniqueID));
            model.Notice = Utils.GetFormValue("txt_Order");
            model.Remark = Utils.GetFormValue("txt_Remark");
            model.RouteName = Utils.GetFormValue("hidRouteName");
            model.OrderId = Utils.GetFormValue("hidOrderId");
            model.CustomerInfoList = null;
            model.State = EyouSoft.Model.EnumType.PlanStructure.TicketState.机票申请;
            model.TicketType = EyouSoft.Model.EnumType.PlanStructure.TicketType.团队申请机票;
            model.RegisterOperatorId = SiteUserInfo.ID;
            model.TicketOutTime = DateTime.Now;

            EyouSoft.BLL.PlanStruture.PlaneTicket bll = new EyouSoft.BLL.PlanStruture.PlaneTicket();

            if (Utils.GetFormValue("hidIsExtsisTicket") == "1")
            {
                //修改
                model.TicketOutId = this.hideId.Value;

                if (bll.UpdateTicketOutListModel(model))
                {
                    EyouSoft.Common.Function.MessageBox.ResponseScript(this.Page, string.Format("alert('修改成功');window.parent.Boxy.getIframeDialog('{0}').hide();window.parent.location.reload();", Utils.GetQueryStringValue("iframeId")));

                }
                else
                {
                    EyouSoft.Common.Function.MessageBox.ResponseScript(this.Page, string.Format("alert('修改失败');"));

                }
            }
            else
            {
                //添加
                if (bll.addTicketOutListModel(model))
                {
                    EyouSoft.Common.Function.MessageBox.ResponseScript(this.Page, string.Format("alert('添加成功');window.parent.Boxy.getIframeDialog('{0}').hide();window.parent.location.reload();", Utils.GetQueryStringValue("iframeId")));
                }
                else
                {
                    EyouSoft.Common.Function.MessageBox.ResponseScript(this.Page, string.Format("alert('添加成功')"));
                }

            }


        }
    }
}
