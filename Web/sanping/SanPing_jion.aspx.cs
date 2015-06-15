/// ////////////////////////////////////////////////////////////////////////////////
/// 版权所有：Copyright (C) 杭州易诺科技 2011
/// 模块名称：SanPing_jion.aspx.cs
/// 模块编号： 
/// 文件名称：F:\myProject\巴比来\Web\sanping\SanPing_jion.aspx.cs
/// 功    能： 散拼报名
/// 作    者：田想兵
/// 创建时间：2011-1-12 16:09:32
/// 修改时间：
/// 公    司：杭州易诺科技 
/// 产    品：巴比来 
/// ////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using System.Data;
using System.Text.RegularExpressions;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text;

namespace Web.sanping
{
    /// <summary>
    /// 散拼报名
    /// 创建人：田想兵 日期:2011-1-12
    /// 修改：田想兵 日期:2011-6-8
    /// 修改内容：添加返佣和对方操作员
    /// </summary>
    /// 修改人：柴逸宁
    /// 修改时间：2011-7-4
    /// 修改备注：添加游客信息提交验证
    public partial class SanPing_jion : Eyousoft.Common.Page.BackPage
    {
        #region Private Members
        protected string EditId = string.Empty;//修改ID
        EyouSoft.Model.TourStructure.TourInfo model = null;
        protected string strTraffic = string.Empty;
        #endregion
        IList<EyouSoft.Model.CompanyStructure.CustomStand> list = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadVisitors1.CurrentPageIframeId = Utils.GetQueryStringValue("iframeId");// Request.QueryString["iframeId"];
            if (!IsPostBack)
            {
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
                        StringBuilder jsonNameAndId = new StringBuilder();
                        string userList = "";
                        jsonNameAndId.Append("[");
                        foreach (var vc in cusModel.CustomerContactList)
                        {
                            jsonNameAndId.Append("{\"Name\":\"" + vc.Name + "\",\"ID\":" + vc.ID + "},");
                        }
                        userList = jsonNameAndId.ToString().TrimEnd(',');
                        userList += "]";
                        Response.Write("[{\"saler\":\"" + saler + "\",cusList:" + userList + ",\"CommissionType\":\"" + ((int)cusModel.CommissionType).ToString() + "\",CommissionCount:" + Utils.FilterEndOfTheZeroDecimal(cusModel.CommissionCount) + "}]");
                    }
                    Response.End();
                }


                #endregion

                BindXlInfo();
                BindPireList();
                strTraffic = GetSelectTraffic(-1);
                #region //配置留位时间读取

                EyouSoft.BLL.CompanyStructure.CompanySetting setBll = new EyouSoft.BLL.CompanyStructure.CompanySetting();//初始化bll
                EyouSoft.Model.CompanyStructure.CompanyFieldSetting set = null;//配置实体
                set = setBll.GetSetting(CurrentUserCompanyID);
                txtEndTime.Attributes["onfocus"] = "WdatePicker({errDealMode:1,minDate:'" + DateTime.Now.ToString() + "',maxDate:'" + DateTime.Now.AddMinutes(set.ReservationTime).ToString() + "',dateFmt:'yyyy/MM/dd HH:mm',alwaysUseStartDate:true});";
                hd_waitTime.Value = set.ReservationTime.ToString();
                hd_IsRequiredTraveller.Value = set.IsRequiredTraveller.ToString();
                #endregion
            }
            #region 获取关联交通成本价
            if (Utils.GetQueryStringValue("act") == "getPrice")
            {
                GetPrice();
            }
            #endregion
        }

        /// <summary>
        /// 获取成本价
        /// </summary>
        protected void GetPrice()
        {
            DateTime dt = Utils.GetDateTime(Utils.GetQueryStringValue("startDate"));
            int trafficId = Utils.GetInt(Utils.GetQueryStringValue("trafficId"));
            EyouSoft.BLL.PlanStruture.PlanTrffic BLL = new EyouSoft.BLL.PlanStruture.PlanTrffic();
            EyouSoft.Model.PlanStructure.TrafficPricesInfo model = BLL.GetTrafficPriceModel(trafficId, dt);
            if (model != null)
            {
                Response.Clear();
                Response.Write(string.Format("{{\"result\":\"{0}\",\"shengyu\":\"{1}\"}}", Utils.FilterEndOfTheZeroDecimal(model.TicketPrices), model.ShengYu.ToString()));
                Response.End();
            }
            else
            {
                Response.Clear();
                Response.Write(string.Format("{{\"result\":\"{0}\",\"shengyu\":\"{1}\"}}", 0, 0));
                Response.End();
            }
        }

        /// <summary>
        /// 价格列表
        /// </summary>
        void BindPireList()
        {

            EyouSoft.BLL.TourStructure.Tour bl = new EyouSoft.BLL.TourStructure.Tour();
            IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> list = bl.GetPriceStandards(Request["tourId"]);

            rpt_price.DataSource = list;
            rpt_price.DataBind();
        }
        /// <summary>
        /// 绑定线路信息
        /// </summary>
        void BindXlInfo()
        {
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour();
            model = (EyouSoft.Model.TourStructure.TourInfo)bll.GetTourInfo(Request.QueryString["tourId"]);
            if (model != null)
            {
                lt_xianluName.Text = model.RouteName;
                lt_teamName.Text = model.TourCode;
                lt_startDate.Text = model.LDate.ToShortDateString();
                hid_StartDate.Value = model.LDate.ToShortDateString();
                lt_startBus.Text = model.LTraffic;
                lt_backBus.Text = model.RTraffic;
                //lt_shengyu.Text = (model.PlanPeopleNumber - model.PeopleNumberShiShou).ToString();//-model.PeopleNumberWeiChuLi-model.PeopleNumberLiuWei).ToString();
                lbshengyu.Text = (model.PlanPeopleNumber - model.PeopleNumberShiShou).ToString();
                lt_phone.Text = SiteUserInfo.ContactInfo.ContactTel;
                lb_username.Text = SiteUserInfo.ContactInfo.ContactName;
                lt_tel.Text = SiteUserInfo.ContactInfo.ContactMobile;
                lt_fax.Text = SiteUserInfo.ContactInfo.ContactFax;
                //lt_seller.Text = model.SellerName;
                lt_oprator.Text = model.Coordinator.Name;
            }

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
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<option value='' data-price='' data-shengyu='0'>请选择</option>");
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.TrafficId == selTrafficId)
                    {
                        sb.AppendFormat("<option value='{0}' selected='selected' data-price='' data-shengyu='0'>{1}</option>", item.TrafficId, item.TrafficName);
                    }
                    else
                    {
                        sb.AppendFormat("<option value='{0}' data-price='' data-shengyu='0'>{1}</option>", item.TrafficId, item.TrafficName);
                    }
                }
            }
            return sb.ToString();
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            model = (EyouSoft.Model.TourStructure.TourInfo)bll.GetTourInfo(Request.QueryString["tourId"]);

            if (model != null)
            {
                if (!Utils.PlanIsUpdateOrDelete(model.Status.ToString()))
                {
                    Response.Write("<script>alert('该团已提交财务，不能对它操作!');location.href=location.href;</script>");
                    return;
                }
            }
            //结算标准 
            string jsbj = Request.Form["radio"];
            int cr_num = Utils.GetInt(txt_crNum.Value);
            int et_num = Utils.GetInt(txt_rtNum.Value);
            decimal sum_money = Utils.GetDecimal(txt_sumMoney.Value);
            string special = txt_Special.Value;

            //散拼信息
            //EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour();
            //model = (EyouSoft.Model.TourStructure.TourInfo)bll.GetTourInfo(Request.QueryString["tourId"]);

            //model.PriceStandards

            EyouSoft.BLL.TourStructure.TourOrder orderbll = new EyouSoft.BLL.TourStructure.TourOrder();
            EyouSoft.Model.TourStructure.TourOrder ordermodel = new EyouSoft.Model.TourStructure.TourOrder();
            int crNum = Utils.GetInt(txt_crNum.Value);
            if (crNum == 0)
            {
                EyouSoft.Common.Function.MessageBox.Show(this.Page, "请填写成人数!");
                BindXlInfo();
                BindPireList();
                return;
            }
            ordermodel.AdultNumber = crNum;
            ordermodel.AreaId = model.AreaId;
            int teamId = Utils.GetInt(Utils.GetFormValue("hd_teamId"));
            if (teamId == 0)
            {
                EyouSoft.Common.Function.MessageBox.Show(this.Page, "请选择组团社!");
                BindXlInfo();
                BindPireList();
                return;
                //Response.Write("<script>alert('请选择组团社');location.href=location.href;</script>");
            }

            ordermodel.BuyCompanyID = teamId;
            ordermodel.BuyCompanyName = Utils.GetFormValue("txt_teamName");

            ordermodel.ChildNumber = Utils.GetInt(txt_rtNum.Value);
            ordermodel.ChildPrice = Utils.GetDecimal(Utils.GetFormValue("hd_rt_price"));
            int cusLevel = Utils.GetInt(Utils.GetFormValue("hd_cuslevel"));
            if (cusLevel == 0)
            {
                EyouSoft.Common.Function.MessageBox.Show(this.Page, "请选择客户等级!");
                BindXlInfo();
                BindPireList();
                return;
            }
            ordermodel.CustomerLevId = cusLevel;
            IList<EyouSoft.Model.TourStructure.TourOrderCustomer> cus_list = new List<EyouSoft.Model.TourStructure.TourOrderCustomer>();
            string[] cus_arr = Utils.GetFormValues("txtVisitorName");
            string[] uri = Utils.GetFormValues("tefu");
            string[] cradType = Utils.GetFormValues("ddlCardType");
            decimal orderprice = 0;
            for (int k = 0; k < cus_arr.Length; k++)
            {
                if (cus_arr[k] == "")
                {
                    break;
                }
                EyouSoft.Model.TourStructure.TourOrderCustomer item = new EyouSoft.Model.TourStructure.TourOrderCustomer();
                item.VisitorName = cus_arr[k];
                item.VisitorType = Utils.GetFormValues("ddlVisitorType")[k] == "1" ? EyouSoft.Model.EnumType.TourStructure.VisitorType.成人 : EyouSoft.Model.EnumType.TourStructure.VisitorType.儿童;
                //switch (Utils.GetFormValues("ddlCardType")[k])
                //{
                //    case "1":
                //        {
                //            item.CradType = EyouSoft.Model.EnumType.TourStructure.CradType.身份证;
                //        } break;
                //    case "2":
                //        {
                //            item.CradType = EyouSoft.Model.EnumType.TourStructure.CradType.护照;
                //        } break;
                //    case "3":
                //        {
                //            item.CradType = EyouSoft.Model.EnumType.TourStructure.CradType.军官证;
                //        } break;
                //    case "4":
                //        {
                //            item.CradType = EyouSoft.Model.EnumType.TourStructure.CradType.台胞证;
                //        } break;
                //    case "5":
                //        {
                //            item.CradType = EyouSoft.Model.EnumType.TourStructure.CradType.港澳通行证;
                //        } break;
                //    default:
                //        {
                //            item.CradType = EyouSoft.Model.EnumType.TourStructure.CradType.未知;
                //        } break;
                //}
                item.CradType = (EyouSoft.Model.EnumType.TourStructure.CradType)Utils.GetInt(cradType[k]);
                item.CradNumber = Utils.GetFormValues("txtCardNo")[k];
                switch (Utils.GetFormValues("ddlSex")[k])
                {
                    case "2":
                        {
                            item.Sex = EyouSoft.Model.EnumType.CompanyStructure.Sex.男;
                        } break;
                    case "1":
                        {
                            item.Sex = EyouSoft.Model.EnumType.CompanyStructure.Sex.女;
                        } break;
                    default:
                        {
                            item.Sex = EyouSoft.Model.EnumType.CompanyStructure.Sex.未知;
                        } break;
                }
                item.ContactTel = Utils.GetFormValues("txtContactTel")[k];
                EyouSoft.Model.TourStructure.CustomerSpecialService css = new EyouSoft.Model.TourStructure.CustomerSpecialService();

                css.Fee = Utils.GetDecimal(Utils.GetFromQueryStringByKey(uri[k], "txtCost"));
                css.IsAdd = Utils.GetFromQueryStringByKey(uri[k], "ddlOperate") == "0" ? true : false;
                css.IssueTime = DateTime.Now;
                css.ProjectName = Utils.GetFromQueryStringByKey(uri[k], "txtItem");
                css.ServiceDetail = Utils.GetFromQueryStringByKey(uri[k], "txtServiceContent");
                item.SpecialServiceInfo = css;
                item.IssueTime = DateTime.Now;
                item.CompanyID = CurrentUserCompanyID;
                cus_list.Add(item);
                orderprice += Utils.GetDecimal(Utils.GetFromQueryStringByKey(uri[k], "txtCost"));
            }
            ordermodel.IssueTime = DateTime.Now;
            ordermodel.OperatorContent = Utils.GetFormValue("txt_actMsg");
            ordermodel.SpecialContent = Utils.GetFormValue("txt_Special");
            ordermodel.CustomerList = cus_list;
            ordermodel.LastDate = DateTime.Now;
            ordermodel.LeaveDate = model.LDate;
            ordermodel.PriceStandId = Utils.GetInt(Utils.GetFormValue("hd_level"));

            ordermodel.PersonalPrice = Utils.GetDecimal(Utils.GetFormValue("hd_cr_price"));
            ordermodel.ChildPrice = Utils.GetDecimal(Utils.GetFormValue("hd_rt_price"));
            ordermodel.ID = Guid.NewGuid().ToString();
            ordermodel.CustomerList = cus_list;
            ordermodel.TourId = model.TourId;
            ordermodel.OrderType = EyouSoft.Model.EnumType.TourStructure.OrderType.代客预定;
            ordermodel.SellCompanyId = CurrentUserCompanyID;
            ordermodel.SellCompanyName = SiteUserInfo.CompanyName;
            ordermodel.ViewOperatorId = SiteUserInfo.ID;
            ordermodel.TourNo = model.TourCode;
            ordermodel.Tourdays = model.TourDays;
            ordermodel.TourClassId = EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划;
            ordermodel.OperatorID = SiteUserInfo.ID;
            ordermodel.OperatorName = SiteUserInfo.ContactInfo.ContactName;
            ordermodel.OrderState = EyouSoft.Model.EnumType.TourStructure.OrderState.未处理;
            ordermodel.OtherPrice = orderprice;
            ordermodel.RouteId = model.RouteId;
            ordermodel.RouteName = model.RouteName;
            ordermodel.SumPrice = Utils.GetDecimal(Utils.GetFormValue(txt_sumMoney.UniqueID)) + orderprice;
            ordermodel.PeopleNumber = Utils.GetInt(Utils.GetFormValue("txt_crNum")) + Utils.GetInt(Utils.GetFormValue("txt_rtNum"));
            ordermodel.ContactTel = lt_phone.Text;
            ordermodel.ContactName = lb_username.Text;
            ordermodel.ContactMobile = lt_tel.Text;
            ordermodel.LeaveTraffic = lt_startBus.Text;
            ordermodel.ReturnTraffic = lt_backBus.Text;
            ordermodel.ContactFax = lt_fax.Text;
            EyouSoft.BLL.CompanyStructure.Customer csbll = new EyouSoft.BLL.CompanyStructure.Customer();
            EyouSoft.Model.CompanyStructure.CustomerInfo cusmodel = csbll.GetCustomerModel(Utils.GetInt(Utils.GetFormValue("hd_teamId")));//
            EyouSoft.Model.TourStructure.TourOrderAmountPlusInfo am = new EyouSoft.Model.TourStructure.TourOrderAmountPlusInfo();
            am.AddAmount = Utils.GetDecimal(Utils.GetFormValue("txt_addmoney"));//增加费用
            am.ReduceAmount = Utils.GetDecimal(Utils.GetFormValue("txt_minusmoney"));//减少费用
            am.Remark = Utils.GetFormValue("txt_remark");//备注
            ordermodel.AmountPlus = am;
            if (cusmodel != null)
            {
                ordermodel.SalerId = cusmodel.SaleId;
                ordermodel.SalerName = cusmodel.Saler;
            }

            #region 返佣和对方操作员
            ordermodel.BuyerContactId = Utils.GetInt(Utils.GetFormValue("otherOprator"));
            ordermodel.BuyerContactName = Utils.InputText(Utils.GetFormValue("hd_orderOprator"));
            ordermodel.CommissionType = (EyouSoft.Model.EnumType.CompanyStructure.CommissionType)Utils.GetInt(Utils.GetFormValue("hd_rebateType"));
            ordermodel.CommissionPrice = Utils.GetDecimal(Utils.GetFormValue("txt_Rebate"));
            #endregion

            switch (btn.CommandName)
            {
                ///提交
                case "submit":
                    {
                        ordermodel.SaveSeatDate = DateTime.Now;
                        ordermodel.OrderState = EyouSoft.Model.EnumType.TourStructure.OrderState.未处理;
                    } break;
                case "Reservations":
                    {
                        ordermodel.OrderState = EyouSoft.Model.EnumType.TourStructure.OrderState.已留位;
                        ordermodel.SaveSeatDate = Utils.GetDateTime(txtEndTime.Text, DateTime.Now);
                    } break;//留位

            }
            string fileAtt = "";
            string oldfileAtt = "";

            if (EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files[0], "VisitorInfoFile", out fileAtt, out oldfileAtt))
            {
                ordermodel.CustomerFilePath = fileAtt;
            }
            else
            {
                EyouSoft.Common.Function.MessageBox.Show(this.Page, "上传附件失败！");
                return;
            }

            ordermodel.BuyerTourCode = Utils.GetFormValue(txtBuyerTourCode.ClientID);
            ordermodel.OrderTrafficId = Utils.GetInt(Utils.GetFormValue("selectTraffic"));

            /// 0:失败；
            /// 1:成功；
            /// 2：该团队的订单总人数+当前订单人数大于团队计划人数总和；
            /// 3：该客户所欠金额大于最高欠款金额；
            int i = orderbll.AddOrder(ordermodel);
            switch (i)
            {
                case 1:
                    {
                        Response.Write("<script>alert('提交成功');parent.Boxy.getIframeDialog('" + Request.QueryString["iframeid"] + "').hide();parent.location.href=parent.location.href;</script>");
                    } break;
                case 2: { Response.Write("<script>alert('该团队的订单总人数+当前订单人数大于团队计划人数总和');location.href=location.href;</script>"); } break;
                case 3: { Response.Write("<script>alert('该客户所欠金额大于最高欠款金额');location.href=location.href;</script>"); } break;
                case 4:
                    {
                        Response.Write("<script>alert('订单人数加上交通出团日期当天已使用票数大于交通出团日期当天人数，添加失败！');location.href=location.href;</script>");
                        break;
                    }
                default: { Response.Write("<script>alert('添加失败!');location.href=location.href;</script>"); } break;
            }
        }
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }

        protected void rpt_price_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (list == null)
            {
                list = new List<EyouSoft.Model.CompanyStructure.CustomStand>();
                EyouSoft.BLL.CompanyStructure.CompanyCustomStand bll = new EyouSoft.BLL.CompanyStructure.CompanyCustomStand();
                int kkk = 0;
                list = bll.GetList(100, 1, ref kkk, CurrentUserCompanyID);
            }
            EyouSoft.Model.TourStructure.TourPriceStandardInfo mm = e.Item.DataItem as EyouSoft.Model.TourStructure.TourPriceStandardInfo;
            Repeater rpt = e.Item.FindControl("rpt_list") as Repeater;
            if (model != null)
            {
                //IList<EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo> plist = model.PriceStandards[e.Item.ItemIndex].CustomerLevels;


                EyouSoft.BLL.TourStructure.Tour tour = new EyouSoft.BLL.TourStructure.Tour();
                IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> listStand = ((EyouSoft.Model.TourStructure.TourInfo)tour.GetTourInfo(Request.QueryString["tourId"])).PriceStandards;
                IList<EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo> plist = listStand[e.Item.ItemIndex].CustomerLevels;
                //for (int j = 0; j < ilist.Count; j++)
                //{
                //    //if(listStand[i].CustomerLevels[i].LevelId == list.sel)
                //    if (list != null)
                //    {
                //        var vn = list.Where(x => x.Id == ilist[j].LevelId).FirstOrDefault();
                //        if (vn != null)
                //            ilist[j].LevelName = vn.CustomStandName;
                //    }
                //}
                for (int i = 0; i < listStand.Count; i++)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        //if(listStand[i].CustomerLevels[i].LevelId == list.sel)
                        if (listStand[i].CustomerLevels.Count > j)
                        {
                            var vn = list.Where(x => x.Id == listStand[i].CustomerLevels[j].LevelId).FirstOrDefault();
                            if (vn != null)
                                listStand[i].CustomerLevels[j].LevelName = vn.CustomStandName;
                            else
                                listStand[i].CustomerLevels.RemoveAt(j);
                        }
                        var xn = listStand[i].CustomerLevels.Where(x => x.LevelId == list[j].Id).FirstOrDefault();
                        if (xn == null)
                            listStand[i].CustomerLevels.Add(new EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo() { LevelId = list[j].Id, LevelType = list[j].LevType, LevelName = list[j].CustomStandName, AdultPrice = 0, ChildrenPrice = 0 });
                    }
                }
                rpt.DataSource = plist;
                rpt.DataBind();
            }
        }

    }
}
