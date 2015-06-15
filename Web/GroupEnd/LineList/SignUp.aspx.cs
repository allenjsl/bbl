using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;
using Eyousoft.Common.Page;
using System.Text.RegularExpressions;

namespace Web.GroupEnd
{
    /// <summary>
    /// 订单报名
    /// 李晓欢 2011-01-28
    /// 修改：田想兵 2011-06-17
    /// 修改内容：提交财务和不允许报名及留位操作
    /// </summary>
    public partial class SignUp : Eyousoft.Common.Page.FrontPage
    {
        #region 变量
        //本订单价格标准客户等级
        protected string PriceStandId = "";
        protected string CustomerLevId = "";
        //结算价列表
        protected string price = "";

        //散拼计划报价标准
        System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> PriceList = null;
        EyouSoft.Model.TourStructure.TourInfo model = null;

        //线路区域计调员集合
        protected string coordinator = "";
        #endregion
        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadVisitors1.CurrentPageIframeId = Request.QueryString["iframeId"];
            if (!this.Page.IsPostBack)
            {
                BindPriceList();
                BindAreaInfo();
                GetBackMoney();

            }
        }
        #endregion

        #region 获得组团公司返佣金额配置信息
        protected void GetBackMoney()
        {
            EyouSoft.Model.CompanyStructure.CustomerInfo companyModel = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomerModel(SiteUserInfo.TourCompany.TourCompanyId);
            if (companyModel != null)
            {
                this.lblBackMoney.Text = companyModel.CommissionCount.ToString("f2");
                this.hideBackMoney.Value = companyModel.CommissionCount.ToString("f2");
                this.hideBackType.Value = ((int)companyModel.CommissionType).ToString();
            }
        }


        #endregion


        #region 绑定结算价
        protected void BindPriceList()
        {
            System.Text.StringBuilder stringPrice = new System.Text.StringBuilder();
            string TourId = Utils.GetQueryStringValue("tourId");
            EyouSoft.BLL.TourStructure.Tour tourBLl = new EyouSoft.BLL.TourStructure.Tour();
            PriceList = tourBLl.GetPriceStandards(TourId);
            for (int i = 0; i < PriceList.Count; i++)
            {
                for (int j = 0; j < PriceList[i].CustomerLevels.Count; j++)
                {
                    if (PriceList[i].CustomerLevels[j].LevelId == SiteUserInfo.TourCompany.CustomerLevel)
                    {
                        stringPrice.Append("<tr>");
                        stringPrice.AppendFormat("<td><input type=\"radio\" name=\"radio\" class='radio_select' id=\"radio{0}\" value=\"{0}\"/>{1}&nbsp;&nbsp;</td>", PriceList[i].StandardId, PriceList[i].StandardName);
                        stringPrice.Append("<td>");
                        stringPrice.AppendFormat("成人价：<span name=\"sp_cr_price\">{0}</span>&nbsp;&nbsp;", Utils.FilterEndOfTheZeroDecimal(PriceList[i].CustomerLevels[j].AdultPrice));
                        stringPrice.AppendFormat("儿童价：<span name=\"sp_et_price\">{0}</span>", Utils.FilterEndOfTheZeroDecimal(PriceList[i].CustomerLevels[j].ChildrenPrice));
                        stringPrice.Append("</td></tr>");
                        break;
                    }
                }
            }
            price = stringPrice.ToString();
        }
        #endregion

        #region 弹窗设置
        /// <summary>
        /// OnPreInit事件 设置模式窗体属性
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }
        #endregion

        #region 绑定线路信息
        protected void BindAreaInfo()
        {
            //计划中心业务逻辑类
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour();
            //计划团队信息实体类
            string TourId = Utils.GetQueryStringValue("tourId");
            if (TourId != null && TourId != "")
            {
                model = (EyouSoft.Model.TourStructure.TourInfo)bll.GetTourInfo(TourId);
                if (model != null)
                {
                    
                    this.lt_xianluName.Text = model.RouteName;
                    this.lt_teamCode.Text = model.TourCode;
                    this.lt_startDate.Text = model.LDate.ToShortDateString();
                    this.lt_shengyu.Text = (model.PlanPeopleNumber - model.VirtualPeopleNumber).ToString();
                    //计调员
                    this.litCoordinatorId.Text = model.Coordinator.Name.ToString();
                    //计调员编号
                    this.hidCoordinatorId.Value = model.Coordinator.CoordinatorId.ToString();

                    #region 线路区域
                    int Areaid = model.AreaId;
                    EyouSoft.BLL.CompanyStructure.Area AreaBll = new EyouSoft.BLL.CompanyStructure.Area();
                    EyouSoft.Model.CompanyStructure.Area Area = new EyouSoft.Model.CompanyStructure.Area();
                    Area = AreaBll.GetModel(Areaid);

                    //计调员
                    if (Area != null)
                    {
                        this.Area.Text = Area.AreaName;
                    }
                    #endregion

                    //销售员
                    EyouSoft.Model.CompanyStructure.CustomerInfo Customer = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomerModel(SiteUserInfo.TourCompany.TourCompanyId);
                    if (Customer != null)
                    {
                        this.litseller.Text = Customer.Saler;
                    }

                }
            }
            //初始化联系人
            this.txtContactName.Text = SiteUserInfo.ContactInfo.ContactName;
            //电话
            this.txtContactPhone.Text = SiteUserInfo.ContactInfo.ContactTel;
            //手机
            this.txtContactMobile.Text = SiteUserInfo.ContactInfo.ContactMobile;
            //传真
            this.txtContactFax.Text = SiteUserInfo.ContactInfo.ContactFax;

        }
        #endregion

        #region 确认提交信息
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Save(true, DateTime.Now);
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="orderState">true=非同意留位操作</param>
        /// <param name="saveSeatDate">留位时间</param>

        private void Save(bool state, DateTime saveSeatDate)
        {
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            string TourId = Utils.GetQueryStringValue("tourId");
            model = (EyouSoft.Model.TourStructure.TourInfo)bll.GetTourInfo(TourId);
            #region 提交财务后不允许操作 by  txb 6.17
            if (model != null)
            {
                if (!Utils.PlanIsUpdateOrDelete(model.Status.ToString()))
                {
                    Response.Write("<script>alert('该团已提交财务，不能对它操作!');location.href=location.href;</script>");
                    return;
                }
            }
            #endregion
            //订单信息业务逻辑层
            EyouSoft.BLL.TourStructure.TourOrder orderbll = new EyouSoft.BLL.TourStructure.TourOrder();
            //订单信息实体类
            EyouSoft.Model.TourStructure.TourOrder ordermodel = new EyouSoft.Model.TourStructure.TourOrder();
            //线路区域编号
            ordermodel.AreaId = model.AreaId;
            //儿童数
            ordermodel.ChildNumber = Utils.GetInt(Utils.GetFormValue(txtChildCount.UniqueID));
            //儿童价
            ordermodel.ChildPrice = Utils.GetDecimal(Utils.GetFormValue("hd_rt_price"));
            //成人数
            ordermodel.AdultNumber = Utils.GetInt(Utils.GetFormValue(txtDdultCount.UniqueID));
            //成人价
            ordermodel.PersonalPrice = Utils.GetDecimal(Utils.GetFormValue("hd_cr_price"));
            //预定人姓名
            ordermodel.ContactName = this.txtContactName.Text;
            //预订人电话
            ordermodel.ContactTel = this.txtContactPhone.Text;
            //手机
            ordermodel.ContactMobile = this.txtContactMobile.Text;
            //传真
            ordermodel.ContactFax = this.txtContactFax.Text;
            //客户等级编号
            ordermodel.CustomerLevId = SiteUserInfo.TourCompany.CustomerLevel;
            //报价等级编号
            ordermodel.PriceStandId = Utils.GetInt(Utils.GetFormValue(hd_PriceStandId.UniqueID));
            //下单时间
            ordermodel.IssueTime = DateTime.Now;
            //游客特别要求
            ordermodel.SpecialContent = Utils.GetFormValue("txt_Special");
            //组团社编号
            ordermodel.BuyCompanyID = SiteUserInfo.TourCompany.TourCompanyId;
            //是否组团端报名
            ordermodel.IsTourOrderEdit = true;
            //组团社单位名称
            EyouSoft.Model.CompanyStructure.CustomerInfo customInfo =
                new EyouSoft.BLL.CompanyStructure.Customer().GetCustomerModel(SiteUserInfo.TourCompany.TourCompanyId);
            if (customInfo != null)
            {
                ordermodel.BuyCompanyName = customInfo.Name;
                //销售员
                ordermodel.SalerId = customInfo.SaleId;
                ordermodel.SalerName = customInfo.Saler;
            }

            //下单人联系手机
            //ordermodel.ContactMobile = SiteUserInfo.ContactInfo.ContactMobile;
            //下单人传真
            //ordermodel.ContactFax = SiteUserInfo.ContactInfo.ContactFax;

            //计调员
            ordermodel.OperatorList = new List<EyouSoft.Model.TourStructure.TourOperator>();
            EyouSoft.Model.TourStructure.TourOperator TourOperator = new EyouSoft.Model.TourStructure.TourOperator();
            TourOperator.ContactName = model.Coordinator.Name.ToString();
            TourOperator.OperatorId = model.Coordinator.CoordinatorId;
            ordermodel.OperatorList.Add(TourOperator);

            #region 根据团队计划获取线路编号
            EyouSoft.BLL.TourStructure.Tour bllTour = new EyouSoft.BLL.TourStructure.Tour();
            //计划团队信息实体类
            string TourID = Utils.GetQueryStringValue("tourId");
            if (TourID != null && TourID != "")
            {
                model = (EyouSoft.Model.TourStructure.TourInfo)bllTour.GetTourInfo(TourID);
                if (model != null)
                {
                    ordermodel.RouteId = model.RouteId;
                    ordermodel.ViewOperatorId = model.OperatorId;
                }
            }
            #endregion

            //获取团队类型编号
            ordermodel.TourClassId = EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划;

            //订单游客信息业务逻辑
            IList<EyouSoft.Model.TourStructure.TourOrderCustomer> cus_list = new List<EyouSoft.Model.TourStructure.TourOrderCustomer>();
            //获取所有的游客姓名
            string[] cus_arr = Utils.GetFormValues("txtVisitorName");
            //获取特服信息
            string[] uri = Utils.GetFormValues("tefu");
            string[] cardType = Utils.GetFormValues("ddlCardType");
            decimal orderprice = 0;
            for (int k = 0; k < cus_arr.Length; k++)
            {
                if (Utils.GetFormValues("txtVisitorName")[k] == "")
                {
                    break;
                }
                //订单游客信息实体
                EyouSoft.Model.TourStructure.TourOrderCustomer item = new EyouSoft.Model.TourStructure.TourOrderCustomer();
                item.VisitorName = Utils.GetFormValues("txtVisitorName")[k];
                //游客类型
                item.VisitorType = Utils.GetFormValues("ddlVisitorType")[k] == "1" ? EyouSoft.Model.EnumType.TourStructure.VisitorType.成人 : EyouSoft.Model.EnumType.TourStructure.VisitorType.儿童;
                item.CradType = (EyouSoft.Model.EnumType.TourStructure.CradType)Utils.GetInt(cardType[k]);
                //游客证件号码
                item.CradNumber = Utils.GetFormValues("txtCardNo")[k];
                #region 游客性别
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
                #endregion

                //游客联系电话
                item.ContactTel = Utils.GetFormValues("txtContactTel")[k];

                //游客特服信息实体类
                EyouSoft.Model.TourStructure.CustomerSpecialService css = new EyouSoft.Model.TourStructure.CustomerSpecialService();
                //特服费用
                css.Fee = Utils.GetDecimal(Utils.GetFromQueryStringByKey(uri[k], "txtCost"));
                //增加费用还是减少费用
                css.IsAdd = Utils.GetFromQueryStringByKey(uri[k], "ddlOperate") == "0" ? true : false;
                css.IssueTime = DateTime.Now;
                //项目
                css.ProjectName = Utils.GetFromQueryStringByKey(uri[k], "txtItem");
                //服务内容
                css.ServiceDetail = Utils.GetFromQueryStringByKey(uri[k], "txtServiceContent");
                //游客特服信息
                item.SpecialServiceInfo = css;
                item.IssueTime = DateTime.Now;
                item.CompanyID = SiteUserInfo.CompanyID;

                //游客所有的信息添加到订单
                cus_list.Add(item);
                orderprice += Utils.GetDecimal(Utils.GetFromQueryStringByKey(uri[k], "txtCost"));
            }

            //游客信息集合
            ordermodel.CustomerList = cus_list;
            //返佣金额
            ordermodel.CommissionPrice = Utils.GetDecimal(this.hideBackMoney.Value);
            //返佣类型
            ordermodel.CommissionType = (EyouSoft.Model.EnumType.CompanyStructure.CommissionType)Utils.GetInt(this.hideBackType.Value);
            ordermodel.BuyerContactId = new EyouSoft.BLL.CompanyStructure.Customer().GetContactId
(SiteUserInfo.ID);
            ordermodel.BuyerContactName = SiteUserInfo.ContactInfo.ContactName;
            ordermodel.LastDate = DateTime.Now;
            ordermodel.LeaveDate = model.LDate;
            ordermodel.SaveSeatDate = saveSeatDate;
            ordermodel.PersonalPrice = Utils.GetDecimal(Utils.GetFormValue("hd_cr_price"));
            ordermodel.ChildPrice = Utils.GetDecimal(Utils.GetFormValue("hd_rt_price"));
            ordermodel.ID = Guid.NewGuid().ToString();
            ordermodel.TourId = model.TourId;
            ordermodel.OrderType = EyouSoft.Model.EnumType.TourStructure.OrderType.组团下单;
            ordermodel.SellCompanyId = SiteUserInfo.CompanyID;
            ordermodel.SellCompanyName = this.SiteUserInfo.CompanyName;

            ordermodel.TourNo = model.TourCode;
            ordermodel.Tourdays = model.TourDays;
            ordermodel.TourClassId = EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划;
            ordermodel.OperatorID = SiteUserInfo.ID;
            ordermodel.OperatorName = SiteUserInfo.ContactInfo.ContactName;
            ordermodel.OrderState = state ? EyouSoft.Model.EnumType.TourStructure.OrderState.未处理 : EyouSoft.Model.EnumType.TourStructure.OrderState.已留位;
            ordermodel.OtherPrice = orderprice;
            ordermodel.RouteId = model.RouteId;
            ordermodel.RouteName = model.RouteName;
            ordermodel.OtherPrice = orderprice;
            ordermodel.SumPrice = Utils.GetDecimal(Utils.GetFormValue("txtTotalMoney").ToString());
            ordermodel.PeopleNumber = Utils.GetInt(Utils.GetFormValue(txtDdultCount.UniqueID)) + Utils.GetInt(Utils.GetFormValue(txtChildCount.UniqueID));

            #region 游客附件信息
            string fileAtt = "";
            string oldfileAtt = "";
            string VisitorInfoFile = "/uploadFiles/zutuanFile/";
            if (EyouSoft.Common.Function.UploadFile.FileUpLoad(Request.Files[0], VisitorInfoFile, out fileAtt, out oldfileAtt))
            {
                ordermodel.CustomerFilePath = fileAtt;
            }
            else
            {
                EyouSoft.Common.Function.MessageBox.Show(this.Page, "上传附件失败！");
                return;
            }
            #endregion
            //提交订单
            /// 0:失败；
            /// 1:成功；
            /// 2：该团队的订单总人数+当前订单人数大于团队计划人数总和；
            /// 3：该客户所欠金额大于最高欠款金额；
            int i = orderbll.AddOrder(ordermodel);
            switch (i)
            {
                case 1:
                    {
                        Utils.ShowMsgAndCloseBoxy("提交成功", Request.QueryString["iframeid"], true);
                    } break;
                case 2: { Response.Write("<script>alert('该团队的订单总人数+当前订单人数大于团队计划人数总和');location.href=location.href;</script>"); } break;
                case 3: { Response.Write("<script>alert('该客户所欠金额大于最高欠款金额');location.href=location.href;</script>"); } break;
                default: { Response.Write("<script>alert('添加失败!');location.href=location.href;</script>"); } break;
            }

            orderbll = null;
            ordermodel = null;
            cus_list = null;
        }
        #endregion

        #region 同意留位
        //同意留位
        protected void btnYes_Click(object sender, EventArgs e)
        {
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

                        return;
                    }
                }
                else
                {
                    printFaiMsg("留位时间输入错误!");

                    return;
                }
            }
            EyouSoft.BLL.TourStructure.Tour bll = new EyouSoft.BLL.TourStructure.Tour(SiteUserInfo);
            string TourId = Utils.GetQueryStringValue("tourId");
            model = (EyouSoft.Model.TourStructure.TourInfo)bll.GetTourInfo(TourId);

            #region 提交财务后不允许操作 by  txb 6.17
            if (model != null)
            {
                if (!Utils.PlanIsUpdateOrDelete(model.Status.ToString()))
                {
                    Response.Write("<script>alert('该团已提交财务，不能对它操作!');location.href=location.href;</script>");
                    return;
                }
            }
            #endregion
            if (model != null)
            {
                //EyouSoft.Model.EnumType.TourStructure.OrderState.已留位
                Save(false, seatDate);
            }

            bll = null;
            model = null;
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
            BindPriceList();
            BindAreaInfo();

        }
        #endregion

    }
}
