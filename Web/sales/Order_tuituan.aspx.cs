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

using Eyousoft.Common.Page;

namespace Web.sales
{
    /// <summary>
    /// 订单退团页面
    /// 修改记录：
    /// 1、2011-01-12 曹胡生 创建
    /// </summary>
    public partial class Order_tuituan : BackPage
    {
        protected string cusHtml = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //判断权限
            if (!CheckGrant(global::Common.Enum.TravelPermission.销售管理_订单中心_退团操作))
            {
                EyouSoft.Common.Utils.ResponseNoPermit(global::Common.Enum.TravelPermission.销售管理_订单中心_退团操作, false);
                return;
            }
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

        // 订单游客信息初始化
        private void onInit()
        {
            //订单ID
            string orderID = EyouSoft.Common.Utils.GetQueryStringValue("id").ToString().Trim();
            string tourNo = EyouSoft.Common.Utils.GetQueryStringValue("tourNo");
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            EyouSoft.BLL.TourStructure.TourOrder TourOrderBll = new EyouSoft.BLL.TourStructure.TourOrder();
            System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourOrderCustomer> cusList = TourOrderBll.GetCustomerList(CurrentUserCompanyID, orderID.ToString());
            #region 游客退团列表绑定
            if (cusList != null && cusList.Count > 0)
            {
                int i = 0;
                foreach (EyouSoft.Model.TourStructure.TourOrderCustomer cusModel in cusList)
                {
                    i++;
                    if (cusModel != null)
                    {
                        stringBuilder.AppendFormat("<tr><td style=\"width: 5%\" bgcolor=\"#e3f1fc\" index=\"{0}\" align=\"center\">{0}</td><td height=\"25\" bgcolor=\"#e3f1fc\" align=\"center\">", i);
                        stringBuilder.AppendFormat("<input type=\"text\" class=\"searchinput\" id=\"cusName\" name=\"cusName\" value=\"{0}\"></td>", cusModel.VisitorName);

                        #region 游客类型
                        switch (cusModel.VisitorType)
                        {
                            case EyouSoft.Model.EnumType.TourStructure.VisitorType.成人:
                                stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");
                                stringBuilder.Append("<select disabled=\"disabled\"  name=\"cusType\" >");
                                stringBuilder.Append("<option value=\"0\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\" selected=selected>成人</option>");
                                stringBuilder.Append("<option value=\"2\">儿童</option>");
                                stringBuilder.Append("</select></td>");
                                break;
                            case EyouSoft.Model.EnumType.TourStructure.VisitorType.儿童:
                                stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");
                                stringBuilder.Append("<select disabled=\"disabled\" name=\"cusType\" >");
                                stringBuilder.Append("<option value=\"0\">请选择</option>");
                                stringBuilder.Append("<option value=\"1\">成人</option>");
                                stringBuilder.Append("<option value=\"2\" selected=selected>儿童</option>");
                                stringBuilder.Append("</select></td>");
                                break;
                            default:
                                stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");
                                stringBuilder.Append("<select disabled=\"disabled\" name=\"cusType\" >");
                                stringBuilder.Append("<option value=\"0\" selected=selected>请选择</option>");
                                stringBuilder.Append("<option value=\"1\">成人</option>");
                                stringBuilder.Append("<option value=\"2\"儿童</option>");
                                stringBuilder.Append("</select></td>");
                                break;
                        }
                        #endregion

                        #region 证件类型
                        stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");
                        switch (cusModel.CradType)
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
                        stringBuilder.Append("</td>");
                        #endregion

                        stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");
                        stringBuilder.AppendFormat("<input type=\"text\" class=\"searchinput searchinput02\" id=\"card\" name=\"card\" value=\"{0}\"></td>", cusModel.CradNumber);

                        #region 性别类型
                        stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");

                        switch (cusModel.Sex)
                        {
                            case EyouSoft.Model.EnumType.CompanyStructure.Sex.男:
                                {
                                    stringBuilder.Append("<select id=\"cusSex\" name=\"cusSex\">");
                                    stringBuilder.Append("<option value=\"0\">请选择</option>");
                                    stringBuilder.Append("<option value=\"1\" selected=\"selected\">男</option>");
                                    stringBuilder.Append("<option value=\"2\">女</option>");
                                    stringBuilder.Append("</select>");
                                    break;
                                }
                            case EyouSoft.Model.EnumType.CompanyStructure.Sex.女:
                                {
                                    stringBuilder.Append("<select id=\"cusSex\" name=\"cusSex\">");
                                    stringBuilder.Append("<option value=\"0\">请选择</option>");
                                    stringBuilder.Append("<option value=\"1\">男</option>");
                                    stringBuilder.Append("<option value=\"2\" selected=\"selected\">女</option>");
                                    stringBuilder.Append("</select>");
                                    break;
                                }
                            default:
                                {
                                    stringBuilder.Append("<select id=\"cusSex\" name=\"cusSex\">");
                                    stringBuilder.Append("<option value=\"0\" selected=\"selected\">请选择</option>");
                                    stringBuilder.Append("<option value=\"1\">男</option>");
                                    stringBuilder.Append("<option value=\"2\">女</option>");
                                    break;
                                }
                        }
                        stringBuilder.Append("</td>");
                        #endregion

                        stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");
                        stringBuilder.AppendFormat("<input type=\"text\" class=\"searchinput\" id=\"phone\" name=\"phone\" value=\"{0}\">", cusModel.ContactTel);
                        stringBuilder.Append("</td>");
                        stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");

                        if (cusModel.CustomerStatus == EyouSoft.Model.EnumType.TourStructure.CustomerStatus.正常)
                        {
                            stringBuilder.AppendFormat("<a sign=\"tuituan\" href=\"Tuituan.aspx?cusID={0}&tourNo={1}\">退团</a>", cusModel.ID, tourNo);

                        }//已退团
                        else if (cusModel.CustomerStatus == EyouSoft.Model.EnumType.TourStructure.CustomerStatus.已退团)
                        {
                            stringBuilder.AppendFormat("<a sign=\"tuituan\" href=\"Tuituan.aspx?cusID={0}&tourNo={1}\">已退团</a>", cusModel.ID, tourNo);
                        }
                        stringBuilder.Append("</td </tr>");
                    }
                }
            }
            #endregion

            cusHtml = stringBuilder.ToString();

            #region 释放对象
            TourOrderBll = null;
            cusList = null;
            stringBuilder = null;
            #endregion
        }
    }
}
