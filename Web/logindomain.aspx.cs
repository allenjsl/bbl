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
using EyouSoft.SSOComponent.Entity;
using Common;

namespace Web
{
    /// <summary>
    /// 创建人：张新兵 ，创建时间：2011-01-15
    /// 创建内容：处理登录请求
    /// </summary>
    public partial class logindomain : System.Web.UI.Page
    {
        /// <summary>
        /// 根据当前URL的域名信息 ，获取对应的公司ID
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private int GetCompanyIdByHost(string host)
        {
            EyouSoft.BLL.SysStructure.SystemDomain bll = new EyouSoft.BLL.SysStructure.SystemDomain();
            EyouSoft.Model.SysStructure.SystemDomain domain = bll.GetDomain(host);

            int companyId = 0;

            if (domain != null)
            {
                companyId = domain.CompanyId;
            }

            return companyId;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string u = Utils.InputText(Request.QueryString["u"]);
            string p = Utils.InputText(Request.QueryString["p"]);
            string vc = Utils.InputText(Request.QueryString["vc"]);
            string callback = Utils.InputText(Request.QueryString["callback"]);

            int companyId = GetCompanyIdByHost(Request.Url.Host.ToLower());

            if (companyId == 0)
            {
                Response.Clear();
                Response.Write(";" + callback + "({m:'error url'});");
                Response.End();
            }

            bool isUserValid = false;
            UserInfo userInfo=null;

            string token = EyouSoft.Security.Membership.UserProvider.GenerateSSOToken(u);

            EyouSoft.Security.Membership.UserProvider userProvider = new EyouSoft.Security.Membership.UserProvider();

            isUserValid=userProvider.UserLogin(companyId, u, p, token, out userInfo);

            if (!isUserValid)
            {
                Response.Clear();
                Response.Write(";" + callback + "({m:'用户名或密码不正确'});");
                Response.End();
            }
            else
            {
                if (userInfo != null)
                {
                    if (userInfo.IsEnable == false)
                    {
                        Response.Clear();
                        Response.Write(";" + callback + "({m:'您的账户已停用或已过期，请联系管理员'});");
                        Response.End();
                    }
                }
            }


            EyouSoft.Security.Membership.UserProvider.GenerateUserLoginCookies(token, u);
            Utils.SetCookie(CookieKeyManage.LAST_LOGIN_TIME_COOKIE_KEY, DateTime.Now.ToString("yyyy-M-d-H-m-s"));
            //GenerateLastLoginTimeCookie();

            string html = "";
            if (userInfo.ContactInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.组团用户)
            {
                html = "1";
            }
            else if (userInfo.ContactInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.专线用户)
            {
                html = "2";
            }
            else
            {
                html = "3";
            }

            Response.Clear();
            Response.Write(";" + callback + "({h:" + html + "});");
            Response.End();
        }
    }
}
