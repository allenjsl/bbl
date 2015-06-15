using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Eyousoft.Common.Page;
using System.Linq;
using System.Collections.Generic;

namespace Web.jipiao
{
    /// <summary>
    /// 机票管理出票页面
    /// 修改记录：
    /// 1、2011-01-13 曹胡生　创建
    /// </summary>
    public partial class JiPiao_chupiao : BackPage
    {
        /// <summary>
        /// 计算公式
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType? config_Agency;

        /// <summary>
        /// 配置游客勾选
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.TicketTravellerCheckedType TicketTraveller;
        public System.Collections.Generic.IList<string> CustomerList = null;
        public EyouSoft.Model.PlanStructure.TicketOutListInfo modelinfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.机票管理_机票管理_出票操作))
            {
                EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.机票管理_机票管理_出票操作, false);
                return;
            }
            if (!IsPostBack)
            {
                #region 游客申请机票配置
                TicketTraveller = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetTicketTravellerCheckedType(SiteUserInfo.CompanyID);
                #endregion
                //初始化表单数据
                onInit();
                #region 获得该公司的代理费计算方式
                config_Agency = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetAgencyFee(SiteUserInfo.CompanyID);

                if (config_Agency.HasValue)
                {
                    switch (config_Agency.Value)
                    {
                        case EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式一:
                            this.lblMsgFrist.Text = "(票面价+税/机建)*人数+代理费)";
                            this.lblMsgSecond.Text = "(票面价+税/机建)*人数+代理费)";
                            break;
                        case EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式二:
                            this.lblMsgFrist.Text = "(票面价+税/机建)*人数-代理费)";
                            this.lblMsgSecond.Text = "(票面价+税/机建)*人数-代理费)";
                            break;
                        case EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType.公式三:
                            this.lblMsgFrist.Text = "(票面价*百分比+机建燃油）*人数+其它费用)";
                            this.lblMsgSecond.Text = "(票面价*百分比+机建燃油）*人数+其它费用)";
                            break;
                        default:
                            this.lblMsgFrist.Text = "(票面价+税/机建)*人数+代理费)";
                            this.lblMsgSecond.Text = "(票面价+税/机建)*人数+代理费)";
                            break;
                    }
                    this.hideDoType.Value = ((int)config_Agency.Value).ToString();
                }
                #endregion
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

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        protected void lbtnSave_Click1(object sender, EventArgs e)
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
            string msg = "";
            string id = Utils.GetQueryStringValue("id");
            if (checkData(ref msg))
            {
                EyouSoft.BLL.PlanStruture.PlaneTicket bll = new EyouSoft.BLL.PlanStruture.PlaneTicket();
                EyouSoft.Model.PlanStructure.TicketOutListInfo model = new EyouSoft.Model.PlanStructure.TicketOutListInfo();
                #region 航班，名单，票款
                #region 航班
                //航班列表长度
                int ariLength = Utils.GetFormValues("txtAirTime").Length;
                //客户列表长度
                int cusLength = Utils.GetFormValues("txtCusName").Length;
                System.Collections.Generic.IList<EyouSoft.Model.PlanStructure.TicketFlight> TicketFlightList = new
                    System.Collections.Generic.List<EyouSoft.Model.PlanStructure.TicketFlight>();
                for (int i = 0; i < ariLength; i++)
                {
                    EyouSoft.Model.PlanStructure.TicketFlight cusModel = new EyouSoft.Model.PlanStructure.TicketFlight();
                    //航班日期
                    string txtAirTime = Utils.GetFormValues("txtAirTime")[i];
                    //航班线路
                    string selAirLine = Utils.GetFormValues("selAirLine")[i];
                    //航空公司
                    string selAirCompany = Utils.GetFormValues("SelAirCompany")[i];
                    //折扣
                    string txtZheKo = Utils.GetFormValues("txtZheKo")[i];
                    //航班日期
                    cusModel.DepartureTime = Utils.GetDateTime(txtAirTime);
                    //航班线路
                    cusModel.FligthSegment = selAirLine;
                    //航空公司
                    cusModel.AireLine = (EyouSoft.Model.EnumType.PlanStructure.FlightCompany)(Utils.GetInt(selAirCompany));
                    //折扣
                    cusModel.Discount = Utils.GetDecimal(txtZheKo);
                    cusModel.TicketTime = Utils.GetFormValues("txt_hbh_date")[i];
                    //添加到航班列表
                    TicketFlightList.Add(cusModel);
                    cusModel = null;
                }
                //航班列表
                model.TicketFlightList = TicketFlightList;
                #endregion
                #region 名单
                System.Collections.Generic.IList<EyouSoft.Model.PlanStructure.TicketOutCustomerInfo> TicketOutCustomerInfoList = new
    System.Collections.Generic.List<EyouSoft.Model.PlanStructure.TicketOutCustomerInfo>();
                for (int i = 0; i < cusLength; i++)
                {
                    EyouSoft.Model.PlanStructure.TicketOutCustomerInfo cusModel = new EyouSoft.Model.PlanStructure.TicketOutCustomerInfo();
                    //是否勾选
                    string cusID = "";
                    try
                    {
                        cusID = Utils.GetFormValues("chkOper")[i];
                    }
                    catch
                    {
                        continue;
                    }
                    //客户姓名
                    string txtCusName = Utils.GetFormValues("txtCusName")[i];
                    //客户证件类型
                    string selCardType = Utils.GetFormValues("SelCardType")[i];
                    //证件号码
                    string txtCardNumber = Utils.GetFormValues("txtCardNumber")[i];
                    cusModel.TicketOutId = id;
                    cusModel.UserId = cusID;
                    //添加至客户列表
                    TicketOutCustomerInfoList.Add(cusModel);
                    cusModel = null;
                }
                //客户列表对应关系
                model.TicketOutCustomerInfoList = TicketOutCustomerInfoList;
                #endregion
                #region 票款
                System.Collections.Generic.IList<EyouSoft.Model.PlanStructure.TicketKindInfo> TicketKindInfoList = new
    System.Collections.Generic.List<EyouSoft.Model.PlanStructure.TicketKindInfo>();
                EyouSoft.Model.PlanStructure.TicketKindInfo TicketKindInfoModel = new EyouSoft.Model.PlanStructure.TicketKindInfo();
                //成人票面价
                TicketKindInfoModel.Price = Utils.GetDecimal(this.txtAdultPrice.Value);
                //成人税/机建
                TicketKindInfoModel.OilFee = Utils.GetDecimal(this.txtAdultShui.Value);
                //成人人数
                TicketKindInfoModel.PeopleCount = Utils.GetInt(this.txtAdultCount.Value);
                //成人代理费
                TicketKindInfoModel.AgencyPrice = Utils.GetDecimal(this.txtAdultProxyPrice.Value);
                //成人票款
                TicketKindInfoModel.TotalMoney = Utils.GetDecimal(this.txtAdultSum.Value);
                //其它费用
                TicketKindInfoModel.OtherPrice = Utils.GetDecimal(this.txt_OtherMoney.Value);
                //百分比
                TicketKindInfoModel.Discount = Utils.GetDecimal(this.txt_Percent.Value, 100) / 100;
                //票款类型
                TicketKindInfoModel.TicketType = EyouSoft.Model.EnumType.PlanStructure.KindType.成人;
                TicketKindInfoModel.TicketId = id;

                TicketKindInfoList.Add(TicketKindInfoModel);

                EyouSoft.Model.PlanStructure.TicketKindInfo TicketKindInfoModel1 = new EyouSoft.Model.PlanStructure.TicketKindInfo();
                //儿童票面价
                TicketKindInfoModel1.Price = Utils.GetDecimal(this.txtChildPrice.Value);
                //儿童税/机建
                TicketKindInfoModel1.OilFee = Utils.GetDecimal(this.txtChildShui.Value);
                //儿童人数
                TicketKindInfoModel1.PeopleCount = Utils.GetInt(this.txtChildCount.Value);
                //儿童代理费
                TicketKindInfoModel1.AgencyPrice = Utils.GetDecimal(this.txtChildProxyPrice.Value);
                //儿童票款
                TicketKindInfoModel1.TotalMoney = Utils.GetDecimal(this.txtChildSum.Value);
                //其它费用
                TicketKindInfoModel1.OtherPrice = Utils.GetDecimal(this.txt_OtherMoneyChild.Value);
                //百分比
                TicketKindInfoModel1.Discount = Utils.GetDecimal(this.txt_PercentChild.Value, 100) / 100;
                //票款类型
                TicketKindInfoModel1.TicketType = EyouSoft.Model.EnumType.PlanStructure.KindType.儿童;
                TicketKindInfoModel1.TicketId = id;

                TicketKindInfoList.Add(TicketKindInfoModel1);

                TicketKindInfoModel1 = null;
                TicketKindInfoModel = null;
                model.TicketKindInfoList = TicketKindInfoList;
                #endregion
                #endregion
                #region 基本信息
                //总费用
                model.Total = EyouSoft.Common.Utils.GetDecimal(this.txtSumMoney.Value);
                //支付方式
                model.PayType = (EyouSoft.Model.EnumType.TourStructure.RefundType)EyouSoft.Common.Utils.GetInt(Utils.GetFormValue("ddlPayType"));
                //
                model.OperateID = SiteUserInfo.ID;
                model.CompanyID = SiteUserInfo.CompanyID;
                model.Operator = SiteUserInfo.ContactInfo.ContactName;
                model.RefundId = id;
                //出票表ID
                model.TicketOutId = id;
                //订单ID
                model.OrderId = this.hdOrderID.Value;
                //团号ID
                model.TourId = this.hdTourID.Value;
                //订票须知
                model.Notice = this.txtOrderPiaoMust.Value;
                //PNR
                model.PNR = this.txtPNR.Value;
                //售票处
                model.TicketOffice = this.txtSalePlace.Value;
                model.TicketOfficeId = Utils.GetInt(this.hd_PiaoWuSuppId.Value);
                //票号
                model.TicketNum = this.txtPiaoHao.Value;
                //备注
                model.Remark = this.txtMemo.Value;
                //线路名称
                model.RouteName = this.hd_LineName.Value;

                #endregion
                if (this.IsOk.Checked)
                {
                    model.State = EyouSoft.Model.EnumType.PlanStructure.TicketState.已出票;
                    if (bll.ToTicketOut(model))
                    {
                        TicketFlightList = null;
                        TicketKindInfoList = null;
                        TicketOutCustomerInfoList = null;
                        printSuccMsg("保存成功!");
                    }
                    else
                    {
                        TicketFlightList = null;
                        TicketKindInfoList = null;
                        TicketOutCustomerInfoList = null;
                        printFaiMsg("保存失败");
                    }
                }
                else
                {
                    model.State = EyouSoft.Model.EnumType.PlanStructure.TicketState.审核通过;
                    if (bll.UpdateTicketOutListModel(model))
                    {
                        TicketFlightList = null;
                        TicketKindInfoList = null;
                        TicketOutCustomerInfoList = null;
                        printSuccMsg("修改成功!");
                    }
                    else
                    {
                        TicketFlightList = null;
                        TicketKindInfoList = null;
                        TicketOutCustomerInfoList = null;
                        printFaiMsg("修改失败!");
                    }
                }

                model = null;
            }
            else
            {
                onInit();
                printFaiMsg(msg);
            }
        }

        //判断数据输入是否正确
        private bool checkData(ref string msg)
        {
 
            if (Utils.GetDecimal(this.txtSumMoney.Value, -1) == -1)
            {
                msg = "总金额输入有误!";
                return false;
            }
            if ((Utils.GetInt(this.txtAdultCount.Value) + Utils.GetInt(this.txtChildCount.Value)) <= 0)
            {
                msg = "成人数与儿童数相加必须大于0!";
                return false;
            }
            return true;
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

                    this.hideTourId.Value = model.TourId;

                    EyouSoft.BLL.TourStructure.TourOrder orderbll = new EyouSoft.BLL.TourStructure.TourOrder(SiteUserInfo);
                    System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourOrderCustomer> cusromerList = orderbll.GetTravellers(model.TourId);
                    if (cusromerList != null && cusromerList.Count > 0)
                    {
                        this.RepCusList.DataSource = cusromerList;
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
                                this.txtAdultPrice.Value = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(TicketKindInfo.Price.ToString()).ToString("0.00"));
                                //税/机建
                                this.txtAdultShui.Value = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(TicketKindInfo.OilFee.ToString()).ToString("0.00"));
                                //人数
                                this.txtAdultCount.Value = TicketKindInfo.PeopleCount.ToString();
                                //代理费
                                this.txtAdultProxyPrice.Value = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(TicketKindInfo.AgencyPrice.ToString()).ToString("0.00"));
                                //百分比
                                this.txt_Percent.Value = EyouSoft.Common.Utils.FilterEndOfTheZeroString((TicketKindInfo.Discount).ToString("0.00"));
                                //其它费用
                                this.txt_OtherMoney.Value = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(TicketKindInfo.OtherPrice.ToString()).ToString("0.00"));
                                //票款
                                this.txtAdultSum.Value = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(TicketKindInfo.TotalMoney.ToString()).ToString("0.00"));
                            }
                            #endregion

                            #region 儿童
                            if (TicketKindInfo.TicketType == EyouSoft.Model.EnumType.PlanStructure.KindType.儿童)
                            {
                                //票面价
                                this.txtChildPrice.Value = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(TicketKindInfo.Price.ToString()).ToString("0.00"));
                                //税/机建
                                this.txtChildShui.Value = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(TicketKindInfo.OilFee.ToString()).ToString("0.00"));
                                //人数
                                this.txtChildCount.Value = TicketKindInfo.PeopleCount.ToString();
                                //代理费
                                this.txtChildProxyPrice.Value = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(TicketKindInfo.AgencyPrice.ToString()).ToString("0.00"));
                                //百分比
                                this.txt_PercentChild.Value = EyouSoft.Common.Utils.FilterEndOfTheZeroString((TicketKindInfo.Discount).ToString("0.00"));
                                //其它费用
                                this.txt_OtherMoneyChild.Value = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(TicketKindInfo.OtherPrice.ToString()).ToString("0.00"));
                                //票款
                                this.txtChildSum.Value = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(TicketKindInfo.TotalMoney.ToString()).ToString("0.00"));
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region 基本信息
                    //总费用
                    this.txtSumMoney.Value = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(model.Total.ToString()).ToString("0.00"));
                    //支付方式
                    this.ddlPayType.SelectedValue = model.PayType.ToString();
                    //订票须知
                    this.txtOrderPiaoMust.Value = model.Notice;
                    //PNR
                    this.txtPNR.Value = model.PNR;
                    //售票处
                    this.txtSalePlace.Value = model.TicketOffice;
                    //供应商编号
                    this.hd_PiaoWuSuppId.Value = model.TicketOfficeId.ToString();
                    //票号
                    this.txtPiaoHao.Value = model.TicketNum;
                    //备注
                    this.txtMemo.Value = model.Remark;
                    //支付方式
                    if (model.PayType.ToString() != "0" && model.PayType.ToString() != "")
                    {
                        this.ddlPayType.Items.FindByText(model.PayType.ToString()).Selected = true;
                    }
                    //订单ID
                    this.hdOrderID.Value = model.OrderId;
                    //团号ID
                    this.hdTourID.Value = model.TourId;
                    //线路名称
                    this.hd_LineName.Value = model.RouteName;
                    #endregion
                }
                bll = null;
                model = null;
            }
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
                airType += "{value:\"\",text:\"请选择\"}|";
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
        /// 该游客是否可以申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string isHave(string id, List<int> ApplyFlights, List<int> RefundFlights)
        {
            EyouSoft.BLL.PlanStruture.PlaneTicket bll = new EyouSoft.BLL.PlanStruture.PlaneTicket();
            string strvalue = "";
            #region 原来的判断
            if (CustomerList == null)
                CustomerList = bll.CustomerList(Utils.GetQueryStringValue("tourId"));
            if (CustomerList != null)
            {
                if (CustomerList.Contains(id))
                {
                    string iD = Utils.GetString(Request.QueryString["id"], "");
                    if (id != "" && modelinfo == null)
                    {
                        modelinfo = bll.GetTicketOutListModel(iD);
                    }
                    if (modelinfo != null)
                    {
                        if (modelinfo.CustomerInfoList.Where(x => x.ID == id).Count() > 0)
                        {
                            strvalue = "checked='true'";
                        }
                        else
                        {
                            ////////判断是否可选
                            //strvalue = "disabled=\"disabled\"";

                            #region 配置判断
                            if (TicketTraveller == EyouSoft.Model.EnumType.CompanyStructure.TicketTravellerCheckedType.None)
                            {
                                strvalue = "";
                            }
                            else
                            {
                                int istuipiao = 0, enumtuipiao = 0;//1有退票，2全部退票，0没有退票
                                if (ApplyFlights != null)
                                {
                                    if (RefundFlights != null)
                                    {
                                        foreach (var v in ApplyFlights)
                                        {
                                            var l = RefundFlights.Where(x => x == v);
                                            if (l.Count() > 0)
                                            {
                                                istuipiao++;
                                            }
                                        }
                                    }
                                    if (istuipiao > 0)
                                    {
                                        enumtuipiao = 1;
                                    }
                                    if (istuipiao == ApplyFlights.Count)
                                    {
                                        enumtuipiao = 2;
                                    }
                                    if (TicketTraveller == EyouSoft.Model.EnumType.CompanyStructure.TicketTravellerCheckedType.LeastOne)
                                    {
                                        if (enumtuipiao == 0)
                                        {
                                            strvalue += " disabled=\"disabled\"";
                                        }
                                    }
                                    if (TicketTraveller == EyouSoft.Model.EnumType.CompanyStructure.TicketTravellerCheckedType.All)
                                    {
                                        if (enumtuipiao != 2)
                                        {
                                            strvalue += " disabled=\"disabled\"";
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                    else
                    {
                        //strvalue = "disabled=\"disabled\"";
                        #region 配置判断
                        if (TicketTraveller == EyouSoft.Model.EnumType.CompanyStructure.TicketTravellerCheckedType.None)
                        {
                            strvalue = "";
                        }
                        else
                        {
                            int istuipiao = 0, enumtuipiao = 0;//1有退票，2全部退票，0没有退票
                            if (ApplyFlights != null)
                            {
                                if (RefundFlights != null)
                                {
                                    foreach (var v in ApplyFlights)
                                    {
                                        var l = RefundFlights.Where(x => x == v);
                                        if (l.Count() > 0)
                                        {
                                            istuipiao++;
                                        }
                                    }
                                }
                                if (istuipiao > 0)
                                {
                                    enumtuipiao = 1;
                                }
                                if (istuipiao == ApplyFlights.Count)
                                {
                                    enumtuipiao = 2;
                                }
                                if (TicketTraveller == EyouSoft.Model.EnumType.CompanyStructure.TicketTravellerCheckedType.LeastOne)
                                {
                                    if (enumtuipiao == 0)
                                    {
                                        strvalue += " disabled=\"disabled\"";
                                    }
                                }
                                if (TicketTraveller == EyouSoft.Model.EnumType.CompanyStructure.TicketTravellerCheckedType.All)
                                {
                                    if (enumtuipiao != 2)
                                    {
                                        strvalue += " disabled=\"disabled\"";
                                    }
                                }
                            }
                        }

                        #endregion
                    }
                }
                else
                {
                    strvalue = "";
                }
            }
            else
            {
                strvalue = "";
            }
            #endregion
            if (id == Utils.GetQueryStringValue("cid"))
            {
                strvalue += " checked=\"true\"";
            }
            return strvalue;
        }
    }
}
