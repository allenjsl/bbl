using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Webmaster
{
    #region webmaster page base
    /// <summary>
    /// webmaster page base
    /// </summary>
    public class WebmasterPageBase:System.Web.UI.Page
    {
        /// <summary>
        /// 登录用户信息
        /// </summary>
        //EyouSoft.SSOComponent.Entity.MasterUserInfo UserInfo = null;

        protected override void OnInit(EventArgs e)
        {
            /*UserInfo = Utils.GetWebmaster();

            if (UserInfo == null)
            {
                Response.Redirect("/webmaster/login.aspx");
            }*/

            Utils.ValidateLogin();

            base.OnInit(e);
        }
    }
    #endregion

    #region webmaster utils
    /// <summary>
    /// webmaster utils
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// webmaster login session name
        /// </summary>
        public const string WebmasterLoginSessionName = "WLS";
        /// <summary>
        /// login page file path
        /// </summary>
        public const string WebmasterLoginPath = "/webmaster/login.aspx";

        /// <summary>
        /// set login webmaster
        /// </summary>
        /// <param name="webmasterInfo"></param>
        public static void SetWebmaster(EyouSoft.SSOComponent.Entity.MasterUserInfo webmasterInfo)
        {
            //HttpContext.Current.Session[Utils.WebmasterLoginSessionName] = webmasterInfo;
            EyouSoft.Security.Membership.UserProvider provider= new EyouSoft.Security.Membership.UserProvider();
            string token = EyouSoft.Security.Membership.UserProvider.GenerateSSOToken(webmasterInfo.Username);
            EyouSoft.Security.Membership.UserProvider.GenerateYunYingUserLoginCookies(token,webmasterInfo.Username);

        }

        /// <summary>
        /// get login webmaster
        /// </summary>
        /// <returns></returns>
        public static EyouSoft.SSOComponent.Entity.MasterUserInfo GetWebmaster()
        {
            //return (EyouSoft.SSOComponent.Entity.MasterUserInfo)HttpContext.Current.Session[Utils.WebmasterLoginSessionName];
            EyouSoft.Security.Membership.UserProvider provider = new EyouSoft.Security.Membership.UserProvider();
            var info=provider.GetMaster();
            provider = null;
            return info;
        }

        /// <summary>
        /// webmaster islogin
        /// </summary>
        /// <returns></returns>
        public static bool IsLogin()
        {
            var info = GetWebmaster();
            if (info == null) return false;
            return true;
        }

        /// <summary>
        /// validate webmaster login
        /// </summary>
        public static void ValidateLogin()
        {
            if (IsLogin())
            {
                return;
            }
            else
            {
                HttpContext.Current.Response.Redirect(WebmasterLoginPath);
            }
        }

        /// <summary>
        /// webmaster logout
        /// </summary>
        public static void Logout()
        {
            var webmasterInfo = GetWebmaster();

            if (webmasterInfo != null)
            {
                new EyouSoft.Security.Membership.UserProvider().MasterLogout(webmasterInfo.Username);
            }

            HttpContext.Current.Response.Redirect(WebmasterLoginPath);
        }
    }
    #endregion
}
