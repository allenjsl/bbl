using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EyouSoft.Common;

namespace Web.GroupEnd.Orders
{
    /// <summary>
    /// 描述:组团端订单修改页面
    /// 修改记录:
    /// 1. 2011-02-25 AM 曹胡生 创建
    /// </summary>
    public partial class OrderEidt : Eyousoft.Common.Page.FrontPage
    {
        //游客列表
        protected string cusHtml = "";
        //订单ID
        private string OrderID = "";
        //本订单价格标准客户等级
        protected string PriceStandId = "";
        protected string CustomerLevId = "";
        //结算价列表
        protected string price = "";

        //散拼计划报价标准
        System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> PriceList = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            OrderID = EyouSoft.Common.Utils.GetQueryStringValue("OrderID");
            if (!IsPostBack)
            {
                onInit();
            }
        }

        //设置页面为弹窗
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = Eyousoft.Common.Page.PageType.boxyPage;
        }

        //确认提交
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            string msg = "";
            EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder();
            EyouSoft.Model.TourStructure.TourOrder TourOrderModel = new EyouSoft.Model.TourStructure.TourOrder();
            System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourOrderCustomer> curList = new System.Collections.Generic.List<EyouSoft.Model.TourStructure.TourOrderCustomer>();

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
                    cusModel.OrderId = OrderID;
                    cusModel.CompanyID = this.SiteUserInfo.CompanyID;
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
                        specModel.Fee = Utils.GetDecimal(Utils.GetFromQueryStringByKey(specive,
"txtCost"));
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
                        //doultCount++;
                    }
                    //儿童
                    else if (cusType == 2)
                    {
                        cusModel.VisitorType = EyouSoft.Model.EnumType.TourStructure.VisitorType.儿童;
                        //childCount++;
                    }
                    //其它
                    else
                    {
                        //cusModel.VisitorType = EyouSoft.Model.EnumType.TourStructure.VisitorType.未知;
                        printFaiMsg("请选择游客类型");
                        Response.End();
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
                TourOrderModel.TourId = this.TourID.Value;
                TourOrderModel.SellCompanyId = this.SiteUserInfo.CompanyID;
                TourOrderModel.ContactName = this.txtContactName.Text;
                TourOrderModel.ContactTel = this.txtContactPhone.Text;
                TourOrderModel.ContactMobile = this.txtContactMobile.Text;
                TourOrderModel.ContactFax = this.txtContactFax.Text;
                TourOrderModel.SaveSeatDate = DateTime.Now;
                TourOrderModel.LastDate = DateTime.Now;
                TourOrderModel.SpecialContent = specialRe;
                TourOrderModel.OperatorContent = operMes;
                TourOrderModel.SumPrice = totalMoney;
                TourOrderModel.ChildNumber = Utils.GetInt(this.txtChildCount.Text);
                TourOrderModel.AdultNumber = Utils.GetInt(this.txtDdultCount.Text);
                TourOrderModel.PeopleNumber = TourOrderModel.ChildNumber + TourOrderModel.AdultNumber;
                TourOrderModel.PersonalPrice = adultP;
                TourOrderModel.ChildPrice = childP;
                TourOrderModel.BuyCompanyID = Utils.GetInt(this.hd_BuyCompanyId.Value);
                TourOrderModel.PriceStandId = Utils.GetInt(PriceStandId);
                TourOrderModel.CustomerLevId = Utils.GetInt(CustomerLevId);
                TourOrderModel.OrderState = EyouSoft.Model.EnumType.TourStructure.OrderState.未处理;
                TourOrderModel.RouteId = Utils.GetInt(this.hd_lineID.Value);
                TourOrderModel.CustomerFilePath = uploadFile();
                //是否组团端报名
                TourOrderModel.IsTourOrderEdit = true;
                TourOrderModel.CustomerDisplayType = String.IsNullOrEmpty(TourOrderModel.CustomerFilePath) ? EyouSoft.Model.EnumType.TourStructure.CustomerDisplayType.输入方式 : EyouSoft.Model.EnumType.TourStructure.CustomerDisplayType.附件方式;
                TourOrderModel.CustomerList = curList;
                TourOrderModel.BuyCompanyName = SiteUserInfo.TourCompany.CompanyName;
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
                }
            }
            else
            {
                printFaiMsg(msg);
            }
            TourOrderModel = null;
            curList = null;
            onInit();
        }

        // 订单详细数据初始化
        private void onInit()
        {
            #region 订单数据初化
            if (OrderID != "")
            {
                EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder();
                EyouSoft.Model.TourStructure.TourOrder TourOrderModel = TourOrderBll.GetOrderModel(SiteUserInfo.CompanyID, OrderID);
                System.Text.StringBuilder stringPrice = new System.Text.StringBuilder();
                if (TourOrderModel != null)
                {
                    if (TourOrderModel.OrderType == EyouSoft.Model.EnumType.TourStructure.OrderType.组团下单)
                    {
                        //&& TourOrderModel.BuyCompanyID == SiteUserInfo.TourCompany.TourCompanyId
                        if (TourOrderModel.OrderState == EyouSoft.Model.EnumType.TourStructure.OrderState.未处理)
                        {
                            this.lbtnSubmit.Visible = true;
                        }
                    }
                    #region 结算价绑定
                    //只有散拼有报价标准，单项服务与团队计划都没有报价标准
                    if (TourOrderModel.TourClassId == EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划)
                    {
                        EyouSoft.BLL.TourStructure.Tour tourBLl = new EyouSoft.BLL.TourStructure.Tour();
                        PriceList = tourBLl.GetPriceStandards(TourOrderModel.TourId);
                        for (int i = 0; i < PriceList.Count; i++)
                        {
                            for (int j = 0; j < PriceList[i].CustomerLevels.Count; j++)
                            {
                                if (PriceList[i].CustomerLevels[j].LevelId == SiteUserInfo.TourCompany.CustomerLevel)
                                {
                                    stringPrice.Append("<tr>");
                                    stringPrice.AppendFormat("<td ><input type=\"radio\" name=\"radio\" id=\"radio{0}\" value=\"{0}\"/>{1}</td>", PriceList[i].StandardId, PriceList[i].StandardName);
                                    stringPrice.Append("<td>");
                                    stringPrice.AppendFormat("&nbsp;&nbsp;成人价：<span name=\"sp_cr_price\">{0}</span>&nbsp;&nbsp;", Utils.FilterEndOfTheZeroDecimal(PriceList[i].CustomerLevels[j].AdultPrice));
                                    stringPrice.AppendFormat("儿童价：<span name=\"sp_et_price\">{0}</span>", Utils.FilterEndOfTheZeroDecimal(PriceList[i].CustomerLevels[j].ChildrenPrice));
                                    stringPrice.Append("</td></tr>");
                                    break;
                                }
                            }
                        }
                        price = stringPrice.ToString();
                    }
                    //如果该订单是团队计划订单，则不显示成人数与儿童数，显示总人数
                    else if (TourOrderModel.TourClassId == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
                    {
                        this.SanPingPersonNum.Visible = false;
                        this.lblTeamPersonNum.Visible = true;
                        this.lblTeamPersonNum.Enabled = false;
                    }
                    PriceStandId = TourOrderModel.PriceStandId.ToString();
                    CustomerLevId = TourOrderModel.CustomerLevId.ToString();

                    #endregion
                    #region 订单基本数据
                    //线路名称
                    this.lblLineName.Text = TourOrderModel.RouteName;
                    //出团日期
                    this.lblChuTuanDate.Text = TourOrderModel.LeaveDate.ToString("yyyy-MM-dd");
                    //当前空位
                    this.lblCurFreePosi.Text = TourOrderModel.RemainNum.ToString();
                    //出发交通
                    this.lblChuFanTra.Text = TourOrderModel.LeaveTraffic;
                    //返回交通
                    this.lblBackTra.Text = TourOrderModel.ReturnTraffic;
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
                    //人数（成人）
                    this.txtDdultCount.Text = TourOrderModel.AdultNumber.ToString();
                    //人数（儿童）
                    this.txtChildCount.Text = TourOrderModel.ChildNumber.ToString();
                    //总金额
                    this.txtTotalMoney.Text = EyouSoft.Common.Utils.FilterEndOfTheZeroString(EyouSoft.Common.Utils.GetDecimal(TourOrderModel.SumPrice.ToString()).ToString("0.00"));
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
                    //总人数
                    this.lblTeamPersonNum.Text = TourOrderModel.PeopleNumber.ToString();
                    //团队编号
                    this.TourID.Value = TourOrderModel.TourId;
                    //留位时间
                    if (EyouSoft.Model.EnumType.TourStructure.OrderState.已留位 == TourOrderModel.OrderState)
                    {
                        this.lblLeaveState.Visible = true;
                        this.lblLeave.Visible = true;
                        this.lblLeave.Text = "留位到" + TourOrderModel.SaveSeatDate.ToString("yyyy-MM-dd hh:mm");
                    }
                    else
                    {
                        this.lblLeave.Visible = false;
                        this.lblLeaveState.Visible = false;
                    }
                    if (!string.IsNullOrEmpty(TourOrderModel.CustomerFilePath))
                    {
                        this.hykCusFile.NavigateUrl = TourOrderModel.CustomerFilePath;
                        this.hykCusFile.Visible = true;
                    }
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
                            stringBuilder.AppendFormat("<td style=\"width: 5%\" bgcolor=\"#e3f1fc\" index=\"{0}\" align=\"center\">{0}</td>", i + 1);
                            stringBuilder.Append("<td height=\"25\" bgcolor=\"#e3f1fc\" align=\"center\">");
                            stringBuilder.AppendFormat("<input type=\"text\" class=\"searchinput\" id=\"cusName\" MaxLength=\"50\" valid=\"required\" errmsg=\"请填写姓名!\"  name=\"cusName\" value=\"{0}\" /></td>", curList[i].VisitorName);
                            stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");

                            #region 游客类型
                            if (curList[i].VisitorType == EyouSoft.Model.EnumType.TourStructure.VisitorType.成人)
                            {
                                stringBuilder.Append("<select  id=\"cusType\" title=\"请选择\" valid=\"required\" errmsg=\"请选择类型!\"  name=\"cusType\">");
                                stringBuilder.Append("<option value=\"\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\" selected=\"selected\">成人</option>");
                                stringBuilder.Append("<option value=\"2\">儿童</option>");
                                stringBuilder.Append(" </select>");
                            }
                            //儿童
                            else if (curList[i].VisitorType == EyouSoft.Model.EnumType.TourStructure.VisitorType.儿童)
                            {
                                stringBuilder.Append("<select  title=\"请选择\" valid=\"required\" errmsg=\"请选择类型!\" id=\"cusType\" name=\"cusType\">");
                                stringBuilder.Append("<option value=\"\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\" >成人</option>");
                                stringBuilder.Append("<option value=\"2\" selected=\"selected\">儿童</option>");
                                stringBuilder.Append(" </select>");
                            }
                            //其它
                            else
                            {
                                stringBuilder.Append("<select  id=\"cusType\" title=\"请选择\" valid=\"required\" errmsg=\"请选择类型!\" name=\"cusType\">");
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
                                        stringBuilder.Append("<option value=\"5\">港澳通行证</option>");
                                        stringBuilder.Append("<option value=\"6\" selected=\"selected\">户口本</option>");
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
                            stringBuilder.AppendFormat("<input type=\"text\" class=\"searchinput searchinput02\" id=\"cusCardNo\" onblur='getSex(this)' MaxLength=\"150\" name=\"cusCardNo\" value=\"{0}\">", curList[i].CradNumber);
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");

                            #region 游客性别
                            switch (curList[i].Sex)
                            {
                                case EyouSoft.Model.EnumType.CompanyStructure.Sex.男:
                                    {
                                        stringBuilder.Append("<select id=\"cusSex\" class='ddlSex' name=\"cusSex\">");
                                        stringBuilder.Append("<option value=\"0\">请选择</option>");
                                        stringBuilder.Append("<option value=\"1\" selected=\"selected\">男</option>");
                                        stringBuilder.Append("<option value=\"2\">女</option>");
                                        stringBuilder.Append("</select>");
                                        break;
                                    }
                                case EyouSoft.Model.EnumType.CompanyStructure.Sex.女:
                                    {
                                        stringBuilder.Append("<select id=\"cusSex\" class='ddlSex' name=\"cusSex\">");
                                        stringBuilder.Append("<option value=\"0\">请选择</option>");
                                        stringBuilder.Append("<option value=\"1\">男</option>");
                                        stringBuilder.Append("<option value=\"2\" selected=\"selected\">女</option>");
                                        stringBuilder.Append("</select>");
                                        break;
                                    }
                                default:
                                    {
                                        stringBuilder.Append("<select id=\"cusSex\" class='ddlSex' name=\"cusSex\">");
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
                            stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\" width=\"12%\">");
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

    }
}
