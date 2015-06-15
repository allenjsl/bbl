using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using EyouSoft.Common;
using Eyousoft.Common.Page;

namespace Web.sales
{
    /// <summary>
    /// 销售列表查看页面
    /// 修改记录：
    /// 1、2011-01-12 曹胡生 创建
    /// </summary>
    public partial class Sale_show : Eyousoft.Common.Page.BackPage
    {
        #region 分页参数
        protected int pageSize = 20;
        protected int pageIndex = 1;
        //int recordCount;
        #endregion
        //游客列表
        protected string cusHtml = "";
        //订单ID
        private string OrderID = "";
        //结算价列表
        protected string price = "";

        //本订单价格标准客户等级
        protected string PriceStandId = "";
        protected string CustomerLevId = "";

        //散拼计划报价标准
        System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> PriceList = null;
        //客户等级列表
        System.Collections.Generic.IList<EyouSoft.Model.CompanyStructure.CustomStand> CustomStandList = null;
        //报价标准列表
        System.Collections.Generic.IList<EyouSoft.Model.CompanyStructure.CompanyPriceStand> CompanyPriceStandList = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.GetQueryStringValue("act") == "show") {
                //判断权限
                if (!CheckGrant(global::Common.Enum.TravelPermission.财务管理_团款收入_栏目))
                {
                    EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.财务管理_团款收入_栏目, false);
                    return;
                }
            }
            else
            {
                //判断权限
                if (!CheckGrant(global::Common.Enum.TravelPermission.销售管理_销售收款_栏目))
                {
                    EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.销售管理_销售收款_栏目, false);
                    return;
                }
            }
            OrderID = EyouSoft.Common.Utils.GetQueryStringValue("OrderID");
            if (!IsPostBack)
            {
                onInit();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageType = PageType.boxyPage;
        }

        //销售列表详细数据初始化
        private void onInit()
        {
            if (OrderID != "")
            {
                EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder();
                EyouSoft.Model.TourStructure.TourOrder TourOrderModel = TourOrderBll.GetOrderModel(CurrentUserCompanyID, OrderID);
                System.Text.StringBuilder stringPrice = new System.Text.StringBuilder();

                if (TourOrderModel != null)
                {
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
                                stringPrice.AppendFormat("成人价：<span name=\"sp_cr_price\">{0}</span>", FilterEndOfTheZeroDecimal(GetPriceBy(CompanyPriceStand.Id, CustomStand.Id, true)));
                                stringPrice.AppendFormat("儿童价：<span name=\"sp_et_price\">{0}</span>", FilterEndOfTheZeroDecimal(GetPriceBy(CompanyPriceStand.Id, CustomStand.Id, false)));
                            }
                            stringPrice.Append("</td></tr>");
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
                    this.lblContactName.Text = TourOrderModel.ContactName;
                    //电话
                    this.lblContactPhone.Text = TourOrderModel.ContactTel;
                    //手机
                    this.lblContactMobile.Text = TourOrderModel.ContactMobile;
                    //传真
                    this.lblContactFax.Text = TourOrderModel.ContactFax;
                    //特殊要求说明
                    this.txtSpecialRe.Text = TourOrderModel.SpecialContent;
                    //操作留言
                    this.txtOperMes.Text = TourOrderModel.OperatorContent;
                    //人数（成人）
                    this.txtDdultCount.Text = TourOrderModel.AdultNumber.ToString();
                    //人数（儿童）
                    this.txtChildCount.Text = TourOrderModel.ChildNumber.ToString();
                    //总金额
                    this.txtTotalMoney.Text = Utils.FilterEndOfTheZeroString(TourOrderModel.SumPrice.ToString());
                    //总人数
                    this.lblTeamPersonNum.Text = TourOrderModel.PeopleNumber.ToString();
                    if (!string.IsNullOrEmpty(TourOrderModel.CustomerFilePath))
                    {
                        this.hykCusFile.NavigateUrl = TourOrderModel.CustomerFilePath;
                        this.hykCusFile.Visible = true;
                    }

                    ltrBuyerTourCode.Text = TourOrderModel.BuyerTourCode;
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
                            stringBuilder.AppendFormat("<input type=\"text\" class=\"searchinput\" id=\"cusName\" name=\"cusName\" value=\"{0}\" /></td>", curList[i].VisitorName);
                            stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");

                            #region 游客类型
                            if (curList[i].VisitorType == EyouSoft.Model.EnumType.TourStructure.VisitorType.成人)
                            {
                                stringBuilder.Append("<select disabled=\"disabled\" id=\"cusType\" name=\"cusType\">");
                                stringBuilder.Append("<option value=\"0\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\" selected=\"selected\">成人</option>");
                                stringBuilder.Append("<option value=\"2\">儿童</option>");
                                stringBuilder.Append(" </select>");
                            }
                            //儿童
                            else if (curList[i].VisitorType == EyouSoft.Model.EnumType.TourStructure.VisitorType.儿童)
                            {
                                stringBuilder.Append("<select disabled=\"disabled\" id=\"cusType\" name=\"cusType\">");
                                stringBuilder.Append("<option value=\"0\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\" >成人</option>");
                                stringBuilder.Append("<option value=\"2\" selected=\"selected\">儿童</option>");
                                stringBuilder.Append(" </select>");
                            }
                            //其它
                            else
                            {
                                stringBuilder.Append("<select disabled=\"disabled\" id=\"cusType\" name=\"cusType\">");
                                stringBuilder.Append("<option value=\"0\"  selected=\"selected\">请选择</option>");
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
                                        stringBuilder.Append("</select>");
                                        break;
                                    }
                            }
                            #endregion

                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");
                            stringBuilder.AppendFormat("<input type=\"text\" class=\"searchinput searchinput02\" id=\"cusCardNo\" onblur='getSex(this)' name=\"cusCardNo\" value=\"{0}\">", curList[i].CradNumber);
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
                            stringBuilder.AppendFormat("<input type=\"text\" class=\"searchinput\" id=\"cusPhone\" name=\"cusPhone\" value=\"{0}\">", curList[i].ContactTel);
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
                            stringBuilder.AppendFormat("<a sign=\"speService\" href=\"javascript:void(0)\" onclick=\"OrderEdit.OpenSpecive('spe{0}',$(this))\">特服</a></td>", curList[i].ID);
                            stringBuilder.Append("</tr>");
                        }
                    }
                    cusHtml = stringBuilder.ToString();
                    #endregion                    

                    if (TourOrderModel.BuyerContactId > 0)
                    {
                        var buyerContactInfo = new EyouSoft.BLL.CompanyStructure.Customer().GetCustomerContactModel(TourOrderModel.BuyerContactId);
                        if (buyerContactInfo != null)
                        {
                            this.ltrBuyerContact.Text = string.Format("姓名:{0}&nbsp;&nbsp;电话:{1}&nbsp;&nbsp;手机:{2}&nbsp;&nbsp;QQ:{3}", buyerContactInfo.Name
                                , buyerContactInfo.Tel
                                , buyerContactInfo.Mobile
                                , buyerContactInfo.qq);
                            buyerContactInfo = null;
                        }
                    }
                }

                TourOrderBll = null;
                TourOrderModel = null;
            }
        }

        //过滤小数点后的多余0
        public string FilterEndOfTheZeroDecimal(object o)
        {
            return Utils.FilterEndOfTheZeroString(o.ToString());
        }

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
    }
}
