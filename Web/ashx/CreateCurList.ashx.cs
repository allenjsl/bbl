using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

using EyouSoft.SSOComponent.Entity;

namespace Web.ashx
{
    /// <summary>
    /// 描述：生成游客列表
    /// 修改记录：
    /// 2010-1-20 曹胡生 创建
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CreateCurList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            UserInfo userInfo = null;
            bool _IsLogin = EyouSoft.Security.Membership.UserProvider.IsUserLogin(out userInfo);
            if (!_IsLogin)
            {
                return;
            }

            string[] data = EyouSoft.Common.Utils.GetFormValue("postArray").Split(',');
            string type = EyouSoft.Common.Utils.GetQueryStringValue("type");
            int trCount = EyouSoft.Common.Utils.GetInt(EyouSoft.Common.Utils.GetQueryStringValue("trCount"));
            string[,] curArray = new string[data.Length / 6, 6];
            for (int i = 0; i < data.Length / 6; i++)
            {
                int k = i * 6;
                int t = 0;
                for (int j = k; j < k + 6; j++)
                {
                    curArray[i, t++] = data[j];
                }
            }
            #region 订单游客数据
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            for (int i = 0; i < data.Length / 6; i++)
            {
                if (curArray[i, 1] == "1" || curArray[i, 1] == "成人")
                {
                    stringBuilder.AppendFormat("<tr itemtype=\"adult\">");
                }
                else if (curArray[i, 1] == "2" || curArray[i, 1] == "儿童")
                {
                    stringBuilder.AppendFormat("<tr itemtype=\"child\">");
                }
                else
                {
                    stringBuilder.AppendFormat("<tr itemtype=\"other\">");
                }
                if (type == "order")
                {

                    stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\" index=\"" + (i + 1 + trCount) + "\" style=\"width: 5%\">" + (i + 1 + trCount) + "</td>");

                }
                stringBuilder.Append("<td height=\"25\" bgcolor=\"#e3f1fc\" align=\"center\">");
                stringBuilder.AppendFormat("<input type=\"text\" class=\"searchinput\" id=\"cusName\" valid=\"required\" errmsg=\"请填写姓名!\" name=\"cusName\" MaxLength=\"50\" value=\"{0}\" /></td>", curArray[i, 0].ToString());

                stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");
                //成人
                if (curArray[i, 1] == "1" || curArray[i, 1] == "成人")
                {
                    stringBuilder.Append("<select  title=\"请选择\" valid=\"required\" errmsg=\"请选择类型!\" id=\"cusType\" name=\"cusType\">");
                    stringBuilder.Append("<option value=\"\" >请选择</option>");
                    stringBuilder.Append("<option value=\"1\" selected=\"selected\">成人</option>");
                    stringBuilder.Append("<option value=\"2\">儿童</option>");
                    stringBuilder.Append(" </select>");
                }
                else if (curArray[i, 1] == "2" || curArray[i, 1] == "儿童")
                {
                    stringBuilder.Append("<select  title=\"请选择\" valid=\"required\" errmsg=\"请选择类型!\" id=\"cusType\" name=\"cusType\">");
                    stringBuilder.Append("<option value=\"\" >请选择</option>");
                    stringBuilder.Append("<option value=\"1\" >成人</option>");
                    stringBuilder.Append("<option value=\"2\" selected=\"selected\">儿童</option>");
                    stringBuilder.Append(" </select>");
                }
                else
                {
                    stringBuilder.Append("<select id=\"cusType\" title=\"请选择\" valid=\"required\" errmsg=\"请选择类型!\" name=\"cusType\">");
                    stringBuilder.Append("<option value=\"\" selected=\"selected\">请选择</option>");
                    stringBuilder.Append("<option value=\"1\" >成人</option>");
                    stringBuilder.Append("<option value=\"2\" >儿童</option>");
                    stringBuilder.Append(" </select>");
                }
                stringBuilder.Append("</td>");

                stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");

                #region 游客证件类型
                switch (curArray[i, 2])
                {
                    case "1":
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
                    case "身份证":
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
                    case "2":
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
                    case "护照":
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
                    case "3":
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
                    case "军官证":
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
                    case "4":
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
                    case "台胞证":
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
                    case "5":
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
                    case "港澳通行证":
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
                stringBuilder.AppendFormat("<input type=\"text\" class=\"searchinput searchinput02\" id=\"cusCardNo\" MaxLength=\"150\" name=\"cusCardNo\" value=\"{0}\">", curArray[i, 3]);
                stringBuilder.Append("</td>");

                stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");
                //男
                if (curArray[i, 4] == "1" || curArray[i, 4] == "男")
                {
                    stringBuilder.Append("<select id=\"cusSex\" name=\"cusSex\">");
                    stringBuilder.Append("<option value=\"0\">请选择</option>");
                    stringBuilder.Append("<option value=\"1\" selected=\"selected\">男</option>");
                    stringBuilder.Append("<option value=\"2\">女</option>");
                    stringBuilder.Append("</select>");
                }
                //女
                else if ((curArray[i, 4] == "2") || (curArray[i, 4] == "女"))
                {
                    stringBuilder.Append("<select id=\"cusSex\" name=\"cusSex\">");
                    stringBuilder.Append("<option value=\"0\">请选择</option>");
                    stringBuilder.Append("<option value=\"1\">男</option>");
                    stringBuilder.Append("<option value=\"2\" selected=\"selected\">女</option>");
                    stringBuilder.Append("</select>");
                }
                else
                {
                    stringBuilder.Append("<select id=\"cusSex\" name=\"cusSex\">");
                    stringBuilder.Append("<option value=\"0\" selected=\"selected\">请选择</option>");
                    stringBuilder.Append("<option value=\"1\">男</option>");
                    stringBuilder.Append("<option value=\"2\">女</option>");
                    stringBuilder.Append("</select>");
                }
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\">");
                stringBuilder.AppendFormat("<input type=\"text\" class=\"searchinput\" id=\"cusPhone\" name=\"cusPhone\" MaxLength=\"50\" value=\"{0}\">", curArray[i, 5]);
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\" width=\"6%\">");
                stringBuilder.AppendFormat("<input id=\"{0}\" type=\"hidden\" name=\"specive\" value=\"\" /><a sign=\"speService\"  href=\"javascript:void(0)\" onclick=\"OrderEdit.OpenSpecive('{0}')\">特服</a>", DateTime.Now.Ticks + i);
                stringBuilder.Append("<td bgcolor=\"#e3f1fc\" align=\"center\" width=\"12%\">");
                stringBuilder.Append("<input type=\"hidden\" name=\"cusID\" value=\"\" />");
                stringBuilder.Append("<a sign=\"add\" href=\"javascript:void(0)\" onclick=\"OrderEdit.AddCus()\">添加</a>&nbsp;");
                stringBuilder.Append("<input type=\"hidden\" name=\"cusState\" value=\"ADD\" />");
                stringBuilder.Append("<a sign=\"del\" href=\"javascript:void(0)\" onclick=\"OrderEdit.DelCus($(this))\">删除</a></td></tr>");
            }

            #endregion
            context.Response.ContentType = "text/plain";
            context.Response.Write(stringBuilder.ToString());
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
