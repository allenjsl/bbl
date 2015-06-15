using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Common.Enum;
using EyouSoft.SSOComponent.Entity;

namespace Web.UserControl
{
    /// <summary>
    /// 机票申请
    /// by txb
    /// </summary>
    public partial class jipiaoUpdate : System.Web.UI.UserControl
    {
        /// <summary>
        /// 当前用户名
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 当前用户ID
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 当前公司ID
        /// </summary>
        public int cur_companyId { get; set; }
        /// <summary>
        /// 计算公式
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType? config_Agency;
        /// <summary>
        /// 配置游客勾选
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.TicketTravellerCheckedType TicketTraveller;

        /// <summary>
        /// 是否弹窗 bool小写
        /// </summary>
        public string isWindow { get; set; }
        /// <summary>
        /// 游客列表
        /// </summary>
        protected IList<EyouSoft.Model.TourStructure.TourOrderCustomer> cusList;
        public int count = 0;
        public EyouSoft.Model.PlanStructure.TicketOutListInfo modelinfo;
        public int iii = 0;
        public string piaomianjia = "", shui = "", pepoleNum = "", DaiLiFei = "", piaokuan = "", piaomianjia2 = "", shui2 = "", pepoleNum2 = "", DaiLiFei2 = "", piaokuan2 = ""
            , Png = "", Total = "", paytype = "", Notice = "", Remark = "", OtherMoney = "", Percent = "", OtherMoney2 = "", Percent2 = "";
        public IList<string> CustomerList = null;
        /// <summary>
        /// 时间配置
        /// </summary>
        protected EyouSoft.Model.EnumType.CompanyStructure.TicketOfficeFillTime toft;
        protected void Page_Load(object sender, EventArgs e)
        {
            config_Agency = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetAgencyFee(cur_companyId);
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
                #region 游客申请机票配置
                TicketTraveller = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetTicketTravellerCheckedType(cur_companyId);
                #endregion
                toft = new EyouSoft.BLL.CompanyStructure.CompanySetting().GetTicketOfficeFillTime(cur_companyId);
                BindInfo();
                DdlAreaListInit();

            }
        }
        /// <summary>
        /// 绑定信息
        /// </summary>
        void BindInfo()
        {

            //EyouSoft.Model.PlanStructure.TicketOutListInfo model = bll.GetTicketOutListModel(Request.QueryString["id"]);
            //;
            EyouSoft.BLL.PlanStruture.PlaneTicket bll = new EyouSoft.BLL.PlanStruture.PlaneTicket();

            UserInfo userInfo = null;
            bool _IsLogin = EyouSoft.Security.Membership.UserProvider.IsUserLogin(out userInfo);
            EyouSoft.BLL.TourStructure.TourOrder orderbll = new EyouSoft.BLL.TourStructure.TourOrder(userInfo);
            //string orderId = orderbll.GetOrderIdByTourId(Request.QueryString["tourId"]);
            if (Utils.GetQueryStringValue("tourId") != "")
            {
                string tourId = Utils.GetQueryStringValue("tourId");
                this.hideTourId.Value = tourId;
                
                CustomerList = bll.CustomerList(Utils.GetQueryStringValue("tourId"));

                cusList = orderbll.GetTravellers(tourId);//.Where(x => x.CustomerStatus == EyouSoft.Model.EnumType.TourStructure.CustomerStatus.正常).ToList();
                for (int i = cusList.Count-1; i >=0 ;i--)
                {
                    if (!CustomerList.Contains(cusList[i].ID))
                    {
                        if (cusList[i].CustomerStatus == EyouSoft.Model.EnumType.TourStructure.CustomerStatus.已退团)
                        {
                            cusList.RemoveAt(i);
                        }
                    }
                }
                rpt_list.DataSource = cusList;
                count = 10;
                rpt_list.DataBind();

            }

            if (Utils.GetString(Request.QueryString["id"], "") != "")
            {
                this.hideId.Value = Utils.GetString(Request.QueryString["id"], "");

                modelinfo = bll.GetTicketOutListModel(Request.QueryString["id"]);
                if (modelinfo != null)
                {
                    rpt_hangbang.DataSource = modelinfo.TicketFlightList;
                    rpt_hangbang.DataBind();
                    if (modelinfo.TicketKindInfoList != null)
                    {
                        piaomianjia = Utils.FilterEndOfTheZeroDecimal(modelinfo.TicketKindInfoList.FirstOrDefault().Price);
                        shui = Utils.FilterEndOfTheZeroDecimal(modelinfo.TicketKindInfoList.FirstOrDefault().OilFee);
                        pepoleNum = modelinfo.TicketKindInfoList.FirstOrDefault().PeopleCount.ToString();
                        DaiLiFei = Utils.FilterEndOfTheZeroDecimal(modelinfo.TicketKindInfoList.FirstOrDefault().AgencyPrice);
                        piaokuan = Utils.FilterEndOfTheZeroDecimal(modelinfo.TicketKindInfoList.FirstOrDefault().TotalMoney);

                        OtherMoney = Utils.FilterEndOfTheZeroDecimal(modelinfo.TicketKindInfoList.FirstOrDefault().OtherPrice);
                        Percent = Utils.FilterEndOfTheZeroDecimal(modelinfo.TicketKindInfoList.FirstOrDefault().Discount * 100);
                        if (modelinfo.TicketKindInfoList.Count > 1)
                        {
                            piaomianjia2 = Utils.FilterEndOfTheZeroDecimal(modelinfo.TicketKindInfoList[1].Price);
                            shui2 = Utils.FilterEndOfTheZeroDecimal(modelinfo.TicketKindInfoList[1].OilFee);
                            pepoleNum2 = modelinfo.TicketKindInfoList[1].PeopleCount.ToString();
                            DaiLiFei2 = Utils.FilterEndOfTheZeroDecimal(modelinfo.TicketKindInfoList[1].AgencyPrice);
                            piaokuan2 = Utils.FilterEndOfTheZeroDecimal(modelinfo.TicketKindInfoList[1].TotalMoney);
                            OtherMoney2 = Utils.FilterEndOfTheZeroDecimal(modelinfo.TicketKindInfoList[1].OtherPrice);
                            Percent2 = Utils.FilterEndOfTheZeroDecimal(modelinfo.TicketKindInfoList[1].Discount * 100);
                        }
                    }
                    Png = modelinfo.PNR;
                    Total = Utils.FilterEndOfTheZeroDecimal(modelinfo.Total);
                    paytype = modelinfo.PayType.ToString();
                    Notice = modelinfo.Notice;
                    Remark = modelinfo.Remark;

                    //售票处
                    this.txtSalePlace.Value = modelinfo.TicketOffice;
                    this.hd_PiaoWuSuppId.Value = modelinfo.TicketOfficeId.ToString();
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
            //    DateTime date = DateTime.Parse(txt_date.Value);
            //    string leg = txt_Leg.Value;
            //    string gotime = txt_gotime.Value;
            //    string backLeg = txt_backLeg.Value;
            //    string backTime = txt_backTime.Value;
            //    string company = ddl_Com.SelectedValue;
            //    string PNG = txt_PNG.Value;
            //    decimal PiaoKuang = decimal.Parse(txt_PiaoKuang.Value);
            //    decimal dailifei = decimal.Parse( txt_DaiLiFei.Value);
            //    int pepoleNum = int.Parse( txt_Pepole.Value );
            //    decimal sumPrice = decimal.Parse(txt_SumPrice.Value);
            //    string Order = txt_Order.Value;
            //    for (int i = 0; i < Utils.GetFormValues("txt_code").Length; i++)
            //    { 
            //        //do something...
            //    }
            EyouSoft.Model.TourStructure.TourBaseInfo mb = new EyouSoft.BLL.TourStructure.Tour().GetTourInfo(Utils.GetQueryStringValue("tourid"));
             if (mb != null)
             {
                 if (!Utils.PlanIsUpdateOrDelete(mb.Status.ToString()))
                 {
                     Response.Write("<script>alert('该团已提交财务，不能对它操作!');location.href=location.href;</script>");
                     return;
                 }
             }
            UserInfo userInfo = null;
            bool _IsLogin = EyouSoft.Security.Membership.UserProvider.IsUserLogin(out userInfo);
            EyouSoft.BLL.PlanStruture.PlaneTicket bll = new EyouSoft.BLL.PlanStruture.PlaneTicket();
            EyouSoft.Model.PlanStructure.TicketOutListInfo model = new EyouSoft.Model.PlanStructure.TicketOutListInfo();
            if (this.hideId.Value != "")
            {
                model = bll.GetTicketOutListModel(this.hideId.Value);
            }
            if (model != null)
            {
                model.CompanyID = cur_companyId;
                model.Notice = Utils.GetFormValue("txt_Order");
                model.OperateID = userInfo.ID;
                model.Operator = userInfo.ContactInfo.ContactName;
                model.PayType = (EyouSoft.Model.EnumType.TourStructure.RefundType)Utils.GetInt(Utils.GetFormValue("sel_paytype"));
                model.PNR = Utils.GetFormValue("txt_Pnr");
                model.Remark = Utils.GetFormValue("textRemark");
                model.State = EyouSoft.Model.EnumType.PlanStructure.TicketState.机票申请;
                model.TourId = this.hideTourId.Value;

                IList<EyouSoft.Model.PlanStructure.TicketFlight> ltf = new
                    List<EyouSoft.Model.PlanStructure.TicketFlight>();
                IList<EyouSoft.Model.PlanStructure.TicketKindInfo> ltki = new
                    List<EyouSoft.Model.PlanStructure.TicketKindInfo>();
                for (int i = 0; i < Utils.GetFormValues("txt_date").Length; i++)
                {
                    EyouSoft.Model.PlanStructure.TicketFlight tf = new EyouSoft.Model.PlanStructure.TicketFlight();
                    tf.TicketTime = Utils.GetFormValues("txt_time")[i];
                    tf.AireLine = (EyouSoft.Model.EnumType.PlanStructure.FlightCompany)(Utils.GetInt(Utils.GetFormValues("sel_com")[i]));
                    tf.DepartureTime = Utils.GetDateTime(Utils.GetFormValues("txt_date")[i]);
                    tf.Discount = Utils.GetDecimal(Utils.GetFormValues("txt_Discount")[i]);
                    tf.FligthSegment = Utils.GetFormValues("sel_Flight")[i];
                    ltf.Add(tf);
                }

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
                ////
                model.Total = Utils.GetDecimal(Utils.GetFormValue("txt_SumPrice"));
                model.TicketKindInfoList = ltki;
                model.TicketFlightList = ltf;
                EyouSoft.BLL.RouteStructure.Route routbll = new EyouSoft.BLL.RouteStructure.Route(userInfo);
                EyouSoft.BLL.TourStructure.Tour tourbll = new EyouSoft.BLL.TourStructure.Tour(userInfo);

                model.RouteName = tourbll.GetTourInfo(Utils.GetQueryStringValue("tourId")).RouteName;

                IList<EyouSoft.Model.PlanStructure.TicketOutCustomerInfo> listorder = new List<EyouSoft.Model.PlanStructure.TicketOutCustomerInfo>();
                if (Utils.GetFormValues("chk_md").Length == 0)
                {
                    Utils.Show("请选择游客!");
                    return;
                }
                IList<EyouSoft.Model.TourStructure.TourOrderCustomer> listcus = new
                List<EyouSoft.Model.TourStructure.TourOrderCustomer>();
                //for (int i = 0; i < Utils.GetFormValues("chk_md").Length; i++)
                //{
                //    EyouSoft.Model.PlanStructure.TicketOutCustomerInfo m = new EyouSoft.Model.PlanStructure.TicketOutCustomerInfo();
                //    EyouSoft.Model.TourStructure.TourOrderCustomer c = new EyouSoft.Model.TourStructure.TourOrderCustomer();

                //    c.ID = Utils.GetFormValues("chk_md")[i];
                //    c.VisitorName = Utils.GetFormValues("txt_name")[i];
                //    c.CradType = (EyouSoft.Model.EnumType.TourStructure.CradType)Utils.GetInt(Utils.GetFormValues("sel_cred")[i]);
                //    c.CradNumber = Utils.GetFormValues("txt_code")[i];
                //    m.UserId = Utils.GetFormValues("chk_md")[i];
                //    listorder.Add(m);
                //    listcus.Add(c);
                //}
                for (int i = 0; i < Utils.GetFormValues("chk_md").Length; i++)
                {
                    EyouSoft.Model.PlanStructure.TicketOutCustomerInfo m = new EyouSoft.Model.PlanStructure.TicketOutCustomerInfo();
                    EyouSoft.Model.TourStructure.TourOrderCustomer c = new EyouSoft.Model.TourStructure.TourOrderCustomer();
                    c.ID = Utils.GetFormValues("chk_md")[i];
                    for (int j = 0; j < Utils.GetFormValues("txt_name").Length; j++)
                    {
                        if (c.ID == Utils.GetFormValues("hd_uid")[j])
                        {
                            c.VisitorName = Utils.GetFormValues("txt_name")[j];
                            c.CradType = (EyouSoft.Model.EnumType.TourStructure.CradType)Utils.GetInt(Utils.GetFormValues("sel_cred")[j]);
                            c.CradNumber = Utils.GetFormValues("txt_code")[j];
                            break;
                        }
                    }
                    m.UserId = Utils.GetFormValues("chk_md")[i];
                    listorder.Add(m);
                    listcus.Add(c);
                }
                model.CustomerInfoList = listcus;
                model.TicketOutCustomerInfoList = listorder;

                //if (Utils.GetInt(Utils.GetQueryStringValue("type")) == 1)
                //{
                //    ///散拼计划机票计调
                //    model.TicketType = EyouSoft.Model.EnumType.PlanStructure.TicketType.订单申请机票;
                //}
                //else
                //{
                    ///团队计划机票计调
                    model.TicketType = EyouSoft.Model.EnumType.PlanStructure.TicketType.团队申请机票;
                //}
                //model.TicketOutId = "0";
                    model.RegisterOperatorId = userInfo.ID;
                //售票处
                model.TicketOffice = this.txtSalePlace.Value;
                model.TicketOfficeId = Utils.GetInt(this.hd_PiaoWuSuppId.Value);
                if (this.hideId.Value != "")
                {
                    //修改
                    model.TicketOutId = this.hideId.Value;

                    if (bll.UpdateTicketOutListModel(model))
                    {
                        Utils.ShowAndRedirect("修改成功!", Request.Url.ToString());
                    }
                    else
                    {
                        Utils.ShowAndRedirect("修改失败!", Request.Url.ToString());
                    }

                }
                else
                {
                    //添加
                    if (bll.addTicketOutListModel(model))
                    {
                        Utils.ShowAndRedirect("添加成功!", Request.Url.ToString());
                    }
                    else
                    {
                        Utils.ShowAndRedirect("添加失败!", Request.Url.ToString());
                    }
                }
            }
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

        /// <summary>
        /// 航空公司
        /// </summary>
        protected void DdlAreaListInit()
        {
            string project = "";
            IList<EnumObj> proList = EnumObj.GetList(typeof(EyouSoft.Model.EnumType.PlanStructure.FlightCompany));
            if (proList != null && proList.Count > 0)
            {
                project += "{value:\"\",text:\"--请选择--\"}|";
                for (int i = 0; i < proList.Count; i++)
                {
                    project += "{value:\"" + proList[i].Value + "\",text:\"" + proList[i].Text + "\"}|";
                }
                project = project.TrimEnd('|');
            }
            this.hideAreaList.Value = project;

            //if (this.SetList != null && this.SetList.Count > 0)
            //{
            //    this.hidePrice.Value = this.SetList.Count.ToString();
            //    this.rptList.DataSource = this.SetList;
            //    this.rptList.DataBind();

            //}
        }
    }
}