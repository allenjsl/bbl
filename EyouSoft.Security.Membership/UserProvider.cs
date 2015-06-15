using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.Common;
using EyouSoft.SSOComponent;
using EyouSoft.SSOComponent.Entity;
using System.Web;
using EyouSoft.Toolkit;

/*
 * 用户信息相关操作
 * 蒋胜蓝  2010-6-1
 * 
 */

namespace EyouSoft.Security.Membership
{
    /// <summary>
    /// 凭据类型
    /// </summary>
    enum TicketType
    {
        /// <summary>
        /// 用户
        /// </summary>
        UserCookieName,
        /// <summary>
        /// 管理员
        /// </summary>
        MasterCookieName
    }

    /// <summary>
    /// 用户账号操作
    /// 1.系统登陆、退出
    /// 2.权限验证
    /// 3.用户更新
    /// </summary>
    public class UserProvider
    {
        private static object synchObject = new object();
        //定期更新用于加密SSO票据的密匙和初始化向量.
        private static string _DesKey = "12$#@!#@5trfdewsfr54321234edw%$#";
        private static string _DesIV = "!54~14874&%@+-)#";
        /// <summary>
        /// 加解密对象
        /// </summary>
        static EyouSoft.Common.DataProtection.HashCrypto _hash = new EyouSoft.Common.DataProtection.HashCrypto();
        /// <summary>
        /// COOKIE有效时间
        /// </summary>
        const int CookieExpireTime = 12;
        /// <summary>
        /// 凭据有效时间
        /// </summary>
        const int TicketExpireTime = 12;
        /// <summary>
        /// 用户COOKIE名称
        /// </summary>
        const string UserCookieName = "pfUser";
        /// <summary>
        /// 管理员COOKIE名称
        /// </summary>
        const string MasterCookieName = "pfmUser";
        /// <summary>
        /// 运营后台登录COOKIE NAME
        /// </summary>
        public const string LoginMasterCookie_SSO = "WEBMASTER_SSO";
        public const string LoginMasterCookie_UserName = "WEBMASTER_U";
        /// <summary>
        /// 用户登录COOKIES NAME
        /// </summary>
        public const string LoginCookie_SSO = "TYPT_SSO";
        public const string LoginCookie_UserName = "TYPE_U";
        /// <summary>
        /// 该值记录用户最后一次登录所用的用户名
        /// </summary>
        public const string LoginCookie_LongTimeUserName = "TYPE_LTU";
        /// <summary>
        /// IMFrame用户登录COOKIES NAME
        /// </summary>
        public const string IMFrameLoginCookie_SSO = "IMTYPE_SSO";
        public const string IMFrameLoginCookie_UserName = "IMTYPE_U";
        /// <summary>
        /// 登录key有效时间量
        /// </summary>
        private const int ExpiresMinutes = 360;
        /// <summary>
        /// Cookie_LongTimeUserName的有效时间
        /// </summary>
        private const int ExpiresDays_LongTimeUserName = 30; 
        /// <summary>
        /// 登录页面URL
        /// </summary>
        public static string Url_Login
        {
            get
            {
                return "/login.aspx";
            }
        }
        /// <summary>
        /// 快速登录页面URL
        /// </summary>
        public static string Url_MinLogin
        {
            get
            {
                return "/slogin.aspx";
            }
        }
        /// <summary>
        /// 运营后台登录页面
        /// </summary>
        public static string Url_YunYingLogin
        {
            get
            {
                return "/Login.aspx";
            }
        }
        /// <summary>
        /// 公司编号
        /// </summary>
        public static int CompanyId
        {
            get
            {
                int _CompanyId = 0;

                EyouSoft.Model.SysStructure.SystemDomain model = new EyouSoft.SSOComponent.UserLogin().GetDomain(System.Web.HttpContext.Current.Request.Url.Host.ToLower());
                if (model != null)
                    _CompanyId = model.CompanyId;

                return _CompanyId;
            }
        }

        private const string KEY = "EyouSoft.Common.UserManage";

        public string RedirectUrl = "";

        static UserProvider()
        {
            _hash.Key = _DesKey;
            _hash.IV = _DesIV;            
        }

        public UserProvider()
        {
                        
        }

        #region 票据
        /// <summary>
        /// 根据指定的用户名生成票据
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public static string GenerateSSOToken(string username)
        {
            //时间戳,密匙,UserName=新字符串;
            string date = DateTime.Now.AddMinutes(ExpiresMinutes).ToString("yyyy-MM-dd HH:mm:ss");
            string token = string.Format("{0};{1};{2}", date, KEY, username);
            //SSO票据=DES加密(新字符串);
            token = _hash.DESEncrypt(token);
            return token;
        }

        /// <summary>
        /// SSO票据是否有效
        /// </summary>
        /// <param name="ssoToken">票据</param>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public static bool IsSSOTokenValid(string ssoToken, string username)
        {
            //字符串=DES解密(SSO票据)
            string token = _hash.DeDESEncrypt(ssoToken);
            string[] arr = token.Split(';');
            if (arr.Length == 3)
            {
                DateTime date;
                string user = arr[2], key = arr[1];
                if (DateTime.TryParse(arr[0], out date)
                    && user.Equals(username, StringComparison.Ordinal)
                    && key == KEY)
                {
                    //时间戳在有效时间内。
                    if (date > DateTime.Now)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 编码票据
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string EncodeSSOToken(string token)
        {
            try
            {
                return HttpServerUtility.UrlTokenEncode(System.Text.Encoding.UTF8.GetBytes(token));
            }
            catch (System.Exception e)
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 解码票据   
        /// </summary>
        /// <param name="encodeedToken"></param>
        /// <returns></returns>
        public static string DecodeSSOToken(string encodeedToken)
        {
            try
            {
                return System.Text.Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(encodeedToken));
            }
            catch (System.Exception e)
            {
                return string.Empty;
            }
        }
        #endregion

        #region COOKIES
        /// <summary>
        /// 设置用户登录COOKIES
        /// </summary>
        /// <param name="ssotoken">票据</param>
        /// <param name="username">票据对应用户名</param>
        public static void GenerateUserLoginCookies(string ssotoken, string username)
        {
            //zxb 20100906
            //修改内容：修改平台用户的登录有效时 为 浏览器进程

            ssotoken = EncodeSSOToken(ssotoken);//HttpUtility.UrlEncode(ssotoken, System.Text.Encoding.UTF8);
            string domainSuffix = Utils.GetDomainSuffix(Utils.AbsoluteWebRoot);

            HttpResponse response = HttpContext.Current.Response;

            //移除COOKIES
            response.Cookies.Remove(LoginCookie_SSO);
            response.Cookies.Remove(LoginCookie_UserName);
            response.Cookies.Remove(LoginCookie_LongTimeUserName);
            //Remove SSO Cookie.
            response.Cookies[LoginCookie_SSO].Expires = DateTime.Now.AddDays(-1);
            //if (!String.IsNullOrEmpty(domainSuffix))
            //{
            //    response.Cookies[LoginCookie_SSO].Domain = domainSuffix;
            //}
            //Remove UserName Cookie.
            response.Cookies[LoginCookie_UserName].Expires = DateTime.Now.AddDays(-1);
            //if (!String.IsNullOrEmpty(domainSuffix))
            //{
            //    response.Cookies[LoginCookie_UserName].Domain = domainSuffix;
            //}
            //Remove LongTimeUserName Cookie.
            response.Cookies[LoginCookie_LongTimeUserName].Expires = DateTime.Now.AddDays(-1);
            //if (!String.IsNullOrEmpty(domainSuffix))
            //{
            //    response.Cookies[LoginCookie_LongTimeUserName].Domain = domainSuffix;
            //}

            //DateTime expiresDate = DateTime.Now.AddMinutes(ExpiresMinutes);

            //Set New expiresDate of the LoginCookie_LongTimeUserName.
            DateTime expiresDateOfLTU = DateTime.Now.AddDays(ExpiresDays_LongTimeUserName);

            //Add New Cookies.
            //SSO Cookie.
            System.Web.HttpCookie cookies = new HttpCookie(LoginCookie_SSO);
            cookies.Value = ssotoken;
            //if (!String.IsNullOrEmpty(domainSuffix))
            //{
            //    cookies.Domain = domainSuffix;
            //}
            cookies.HttpOnly = true;
            //cookies.Expires = expiresDate;
            HttpContext.Current.Response.AppendCookie(cookies);

            //UserName Cookie.
            cookies = new HttpCookie(LoginCookie_UserName);
            cookies.Value = HttpUtility.UrlEncode(username, System.Text.Encoding.UTF8);
            //if (!String.IsNullOrEmpty(domainSuffix))
            //{
            //    cookies.Domain = domainSuffix;
            //}
            cookies.HttpOnly = true;
            //cookies.Expires = expiresDate;
            HttpContext.Current.Response.AppendCookie(cookies);

            //LongTimeUserName_Cookie.
            cookies = new HttpCookie(LoginCookie_LongTimeUserName);
            cookies.Value = HttpUtility.UrlEncode(username, System.Text.Encoding.UTF8);
            //if (!String.IsNullOrEmpty(domainSuffix))
            //{
            //    cookies.Domain = domainSuffix;
            //}
            cookies.HttpOnly = true;
            cookies.Expires = expiresDateOfLTU;
            HttpContext.Current.Response.AppendCookie(cookies);
        }
        private static string GetCookie_SSO()
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request.Cookies[LoginCookie_SSO] == null)
            {
                return string.Empty;
            }
            else
            {
                return DecodeSSOToken(request.Cookies[LoginCookie_SSO].Value);//HttpUtility.UrlDecode(request.Cookies[LoginCookie_SSO].Value, System.Text.Encoding.UTF8);
            }
        }
        private static string GetCookie_U()
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request.Cookies[LoginCookie_UserName] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpUtility.UrlDecode(request.Cookies[LoginCookie_UserName].Value,System.Text.Encoding.UTF8);
            }
        }

        /// <summary>
        /// 获取Cookie_LongTimeUserName的值，该值记录用户最后一次登录所用的用户名
        /// </summary>
        /// <returns></returns>
        public static string GetCookie_UserName()
        {
            //return GetCookie_U();
            HttpRequest request = HttpContext.Current.Request;
            if (request.Cookies[LoginCookie_LongTimeUserName] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpUtility.UrlDecode(request.Cookies[LoginCookie_LongTimeUserName].Value, System.Text.Encoding.UTF8);
            }
        }

        /// <summary>
        /// 设置运营后台用户登录COOKIES
        /// </summary>
        /// <param name="model"></param>
        public static void GenerateYunYingUserLoginCookies(string ssotoken, string username)
        {
            ssotoken = HttpUtility.UrlEncode(ssotoken, System.Text.Encoding.UTF8);
            string domainSuffix = Utils.GetDomainSuffix(Utils.AbsoluteWebRoot);

            HttpResponse response = HttpContext.Current.Response;

            //移除COOKIES
            response.Cookies.Remove(LoginMasterCookie_SSO);
            response.Cookies.Remove(LoginMasterCookie_UserName);
            response.Cookies[LoginMasterCookie_SSO].Expires = DateTime.Now.AddDays(-1);
            if (!String.IsNullOrEmpty(domainSuffix))
            {
                response.Cookies[LoginMasterCookie_SSO].Domain = domainSuffix;
            }
            response.Cookies[LoginMasterCookie_UserName].Expires = DateTime.Now.AddDays(-1);
            if (!String.IsNullOrEmpty(domainSuffix))
            {
                response.Cookies[LoginMasterCookie_UserName].Domain = domainSuffix;
            }

            DateTime expiresDate = DateTime.Now.AddMinutes(ExpiresMinutes);
            //添加新的Cookies
            System.Web.HttpCookie cookies = new HttpCookie(LoginMasterCookie_SSO);
            cookies.Value = ssotoken;
            if (!String.IsNullOrEmpty(domainSuffix))
            {
                cookies.Domain = domainSuffix;
            }
            cookies.HttpOnly = true;
            cookies.Expires = expiresDate;
            HttpContext.Current.Response.AppendCookie(cookies);

            cookies = new HttpCookie(LoginMasterCookie_UserName);
            cookies.Value = username;
            if (!String.IsNullOrEmpty(domainSuffix))
            {
                cookies.Domain = domainSuffix;
            }
            cookies.HttpOnly = true;
            cookies.Expires = expiresDate;
            HttpContext.Current.Response.AppendCookie(cookies);
        }
        /// <summary>
        /// 获取cookie中运营后台票据
        /// </summary>
        /// <returns></returns>
        private static string GetYunYingCookie_SSO()
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request.Cookies[LoginMasterCookie_SSO] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpUtility.UrlDecode(request.Cookies[LoginMasterCookie_SSO].Value, System.Text.Encoding.UTF8);
            }
        }
        /// <summary>
        /// 获取cookie中运营后台用户名
        /// </summary>
        /// <returns></returns>
        private static string GetYunYingCookie_U()
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request.Cookies[LoginMasterCookie_UserName] == null)
            {
                return string.Empty;
            }
            else
            {
                return request.Cookies[LoginMasterCookie_UserName].Value;
            }
        }

        /// <summary>
        /// 设置IMFrame用户登录COOKIES
        /// </summary>
        /// <param name="ssotoken"></param>
        /// <param name="username"></param>
        public static void GenerateIMFrameUserLoginCookies(string ssotoken, string username)
        {
            ssotoken = HttpUtility.UrlEncode(ssotoken, System.Text.Encoding.UTF8);
            string domainSuffix = Utils.GetDomainSuffix(Utils.AbsoluteWebRoot);

            HttpResponse response = HttpContext.Current.Response;

            //移除COOKIES
            //response.Cookies.Remove(IMFrameLoginCookie_SSO);
            //response.Cookies.Remove(IMFrameLoginCookie_UserName);
            //response.Cookies[IMFrameLoginCookie_SSO].Expires = DateTime.Now.AddDays(-1);
            //if (!String.IsNullOrEmpty(domainSuffix))
            //{
            //    response.Cookies[IMFrameLoginCookie_SSO].Domain = domainSuffix;
            //}
            //response.Cookies[IMFrameLoginCookie_UserName].Expires = DateTime.Now.AddDays(-1);
            //if (!String.IsNullOrEmpty(domainSuffix))
            //{
            //    response.Cookies[IMFrameLoginCookie_UserName].Domain = domainSuffix;
            //}

            //DateTime expiresDate = DateTime.Now.AddMinutes(ExpiresMinutes);
            //添加新的Cookies
            System.Web.HttpCookie cookies = new HttpCookie(IMFrameLoginCookie_SSO);
            cookies.Value = ssotoken;
            if (!String.IsNullOrEmpty(domainSuffix))
            {
                cookies.Domain = domainSuffix;
            }
            cookies.HttpOnly = true;
            //cookies.Expires = expiresDate;
            HttpContext.Current.Response.AppendCookie(cookies);

            cookies = new HttpCookie(IMFrameLoginCookie_UserName);
            cookies.Value = HttpUtility.UrlEncode(username, System.Text.Encoding.UTF8);
            if (!String.IsNullOrEmpty(domainSuffix))
            {
                cookies.Domain = domainSuffix;
            }
            cookies.HttpOnly = true;
            //cookies.Expires = expiresDate;
            HttpContext.Current.Response.AppendCookie(cookies);
        }

        /// <summary>
        /// 获取cookie中IMFrame票据
        /// </summary>
        /// <returns></returns>
        private static string GetIMFrameCookie_SSO()
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request.Cookies[IMFrameLoginCookie_SSO] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpUtility.UrlDecode(request.Cookies[IMFrameLoginCookie_SSO].Value, System.Text.Encoding.UTF8);
            }
        }
        /// <summary>
        /// 获取cookie中IMFrame用户名
        /// </summary>
        /// <returns></returns>
        private static string GetIMFrameCookie_U()
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request.Cookies[IMFrameLoginCookie_UserName] == null)
            {
                return string.Empty;
            }
            else
            {
                return HttpUtility.UrlDecode(request.Cookies[IMFrameLoginCookie_UserName].Value, System.Text.Encoding.UTF8);
            }
        }

        #endregion COOKIES

        #region 平台用户

        #region 登陆方法
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="UserName">用户名</param>
        /// <param name="PWD">密码</param>
        public bool UserLogin(int CompanyId, string UserName, string PWD, string Token, out EyouSoft.SSOComponent.Entity.UserInfo User)
        {
            User = new EyouSoft.SSOComponent.UserLogin().UserLoginAct(CompanyId, UserName, PWD, Token, PasswordType.MD5);
            if (User == null)
                return false;
            else
                return true;            
        }
        #endregion

        #region 平台用户 判断用户是否登录
        /// <summary>
        /// 判断用户是否登录
        /// </summary>
        /// <returns></returns>
        public static bool IsUserLogin(out UserInfo userInfo)
        {
            userInfo = null;
            HttpRequest request = HttpContext.Current.Request;
            //if (request.Cookies[LoginCookie_SSO] == null || request.Cookies[LoginCookie_UserName] == null)
            //{
            //    return false;
            //}
            string sso = GetCookie_SSO();//request.Cookies[LoginCookie_SSO].Value;
            string username = GetCookie_U();//request.Cookies[LoginCookie_UserName].Value;

            if (String.IsNullOrEmpty(sso) || String.IsNullOrEmpty(username))
            {
                return false;
            }

            //sso = HttpUtility.UrlDecode(sso, System.Text.Encoding.UTF8);

            bool isValid = false;
            try
            {
                isValid = IsSSOTokenValid(sso, username);
            }
            catch (System.Exception e)
            {
                isValid = false;
            }

            if (isValid == true)
            {
                userInfo = GetUser();
                if (userInfo == null)
                {
                    isValid = false;
                }
                else
                {
                    isValid = true;
                }
            }
            
            //userInfo = GetUser();

            return isValid;
        }
        #endregion 判断用户是否登录

        #region 平台用户 跳转到登录页面

        /// <summary>
        /// 跳转到登录页面
        /// </summary>
        public static void RedirectLogin()
        {
            HttpContext.Current.Response.Redirect(Url_Login, true);
        }
        /// <summary>
        /// 跳转到登录页面，并指定登录成功后要跳转的页面地址，及在登录页面要显示的信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="message"></param>
        public static void RedirectLogin(string url, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                HttpContext.Current.Response.Redirect(Url_Login + "?returnurl=" + HttpContext.Current.Server.UrlEncode(url) +
                    "&nlm=" + HttpContext.Current.Server.UrlEncode(message) + "&isShow=1");
            }
            else
            {
                RedirectLogin(url);
            }
        }
        /// <summary>
        /// 跳转到登录页面，并指定登录成功后要跳转的页面地址
        /// </summary>
        /// <param name="url">指定登录成功后要跳转的页面地址</param>
        public static void RedirectLogin(string url)
        {
            HttpContext.Current.Response.Redirect(Url_Login + "?returnurl=" + HttpContext.Current.Server.UrlEncode(url));
        }
        /// <summary>
        /// 跳转到登录页面，并指定要显示的信息。
        /// </summary>
        /// <param name="message"></param>
        public static void RedirectLoginAndShowMsg(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                HttpContext.Current.Response.Redirect(Url_Login + "?nlm=" + HttpContext.Current.Server.UrlEncode(message) + "&isShow=1");
            }
            else
            {
                RedirectLogin();
            }
        }

        /// <summary>
        /// 跳转到快速登录页面，一般用于弹窗内的登录跳转。
        /// </summary>
        /// <param name="url">登录成功后，要跳转的页面地址</param>
        public static void RedirectMinLoginPage(string url)
        {
            RedirectMinLoginPage(url, string.Empty);
        }

        /// <summary>
        /// 跳转到快速登录页面，一般用于弹窗内的登录跳转。
        /// </summary>
        /// <param name="url">登录成功后，要跳转的页面地址</param>
        /// <param name="message">要在登录页面 显示的信息</param>
        public static void RedirectMinLoginPage(string url, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                HttpContext.Current.Response.Redirect(Url_MinLogin + "?returnurl=" + HttpContext.Current.Server.UrlEncode(url) +
                    "&msg=" + HttpContext.Current.Server.UrlEncode(message),true);
            }
            else
            {
                HttpContext.Current.Response.Redirect(Url_MinLogin + "?returnurl=" + HttpContext.Current.Server.UrlEncode(url),true);
            }
        }

        /// <summary>
        /// 获取快速登录页面URL，一般用于javascript弹窗打开
        /// </summary>
        /// <param name="returnUrl">登录成功后，要跳转的页面地址</param>
        /// <param name="locationTarget">设置登录成功后的进行跳转的Target属性,默认为当前窗口，即弹窗内，_top,_parent,_self,_blank</param>
        /// <param name="message">要在登录页面 显示的信息</param>
        /// <returns></returns>
        public static string GetMinLoginPageUrl(string returnUrl, string locationTarget, string message)
        {
            string url = Url_MinLogin+"?t=1";
            if (!string.IsNullOrEmpty(returnUrl))
            {
                url += "&returnurl=" + HttpContext.Current.Server.UrlEncode(returnUrl);
            }
            if(!string.IsNullOrEmpty(locationTarget)){
                url += "&LocationTarget=" + locationTarget;
            }
            if (!string.IsNullOrEmpty(message))
            {
                url += "&msg=" + HttpContext.Current.Server.UrlEncode(message);
            }
            return url;
        }
        #endregion 跳转到登录页面

        #region 平台用户退出
        /// <summary>
        /// 用户退出
        /// </summary>
        public static void UserLogout()
        {
            Logout("");
            //EyouSoft.SSOComponent.Entity.UserInfo User = GetUser();
            //EyouSoft.SSOComponent.Entity.SSOResponse response = new EyouSoft.SSOComponent.Remote.UserLogin().UserLogout(User.ID, RedirectUrl);            
        }
        public static void UserLogout(string url)
        {
            Logout(url);
        }
        /// <summary>
        /// 退出当前用户，并跳转到指定的页面。
        /// </summary>
        /// <param name="url">指定退出后要跳转的页面地址</param>
        private static void Logout(string url)
        {
            new EyouSoft.SSOComponent.UserLogin().UserLogout(CompanyId, GetCookie_U());            

            string domainSuffix = Utils.GetDomainSuffix(Utils.AbsoluteWebRoot);

            System.Web.HttpCookie cookies = new HttpCookie(LoginCookie_SSO);
            //if (!String.IsNullOrEmpty(domainSuffix))
            //{
            //    cookies.Domain = domainSuffix;
            //}

            cookies.HttpOnly = true;
            cookies.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.AppendCookie(cookies);

            if (string.IsNullOrEmpty(url))
            {
                RedirectLogin();
            }
            else
            {
                HttpContext.Current.Response.Redirect(url, true);
            }
        }
        #endregion

        #region 获取、验证平台用户信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public static EyouSoft.SSOComponent.Entity.UserInfo GetUser()
        {
            return new EyouSoft.SSOComponent.UserLogin().GetUserInfo(CompanyId, GetCookie_U());
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public static EyouSoft.SSOComponent.Entity.UserInfo GetUser(string userName)
        {
            return new EyouSoft.SSOComponent.UserLogin().GetUserInfo(CompanyId, userName);
        }
        /// <summary>
        /// 更新用户信息
        /// </summary>
        public void UpdateUser(EyouSoft.SSOComponent.Entity.UserInfo User)
        {
            if (User != null)
            {
                new EyouSoft.SSOComponent.UserLogin().UpdateUserInfo(User);
            }
        }
        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="User">用户信息</param>
        /// <returns></returns>
        private bool ValidateUser()
        {
            string UID = GetCookie_U();
            if (!IsSSOTokenValid(GetCookie_SSO(), UID))
                return false;

            return true;
        }
        /// <summary>
        /// 验证用户权限
        /// </summary>
        /// <param name="PermissionID">权限号</param>
        /// <returns></returns>
        public bool CheckGrant(int PermissionID)
        {
            EyouSoft.SSOComponent.Entity.UserInfo User = GetUser();
            return User.PermissionList.Contains(PermissionID);
        }
        #endregion

        #endregion

        #region 运营后台用户

        #region 系统管理员
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="PWD">用户密码</param>
        /// <returns>管理员信息</returns>
        public MasterUserInfo MasterLogin(string UserName, string PWD)
        {
            MasterUserInfo MasterUser = new MaterLogin().MasterLogin(UserName, PWD);
            if (MasterUser == null)
                return null;
            return MasterUser;
        }
        /// <summary>
        /// 管理员退出
        /// </summary>
        /// <param name="UID">用户编号</param>
        /// <param name="PWD">用户密码</param>
        /// <returns></returns>
        public void MasterLogout(string UID)
        {
            new MaterLogin().MasterLogout(UID);

            string domainSuffix = Utils.GetDomainSuffix(Utils.AbsoluteWebRoot);

            System.Web.HttpCookie cookies = new HttpCookie(LoginMasterCookie_SSO);
            if (!String.IsNullOrEmpty(domainSuffix))
            {
                cookies.Domain = domainSuffix;
            }

            cookies.HttpOnly = true;
            cookies.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.AppendCookie(cookies);
        }
        /// <summary>
        /// 验证管理员
        /// </summary>
        /// <returns></returns>
        private bool ValidateMasterUser()
        {
            try
            {
                string UID = GetYunYingCookie_U();
                if (!IsSSOTokenValid(GetYunYingCookie_SSO(), UID))
                    return false;
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool ValidateMasterUserInFileSystem()
        {
            try
            {
                string UID = GetYunYingCookie_U();
                if (!IsSSOTokenValid(GetYunYingCookie_SSO(), UID))
                    return false;
            }
            catch
            {
                return false;
            }
            return true;

        }
        /// <summary>
        /// 验证管理员权限
        /// </summary>
        /// <param name="PermissionID">权限号</param>
        /// <returns></returns>
        public bool CheckMasterGrant(int PermissionID)
        {
            EyouSoft.SSOComponent.Entity.MasterUserInfo User = GetMaster();
            return User.Permissions.Contains(PermissionID);
        }
        /// <summary>
        /// 更新管理员信息
        /// </summary>
        /// <param name="User">用户编号</param>
        public void UpdateMasterInfo(MasterUserInfo User)
        {
            new MaterLogin().UpdateMasterInfo(User);
        }
        /// <summary>
        /// 获取管理员信息
        /// </summary>
        /// <returns>管理员信息</returns>
        public MasterUserInfo GetMaster()
        {              
            if (ValidateMasterUser())
            {
                MasterUserInfo MasterUser = new MaterLogin().GetMasterInfo(GetYunYingCookie_U());
                return MasterUser;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 运营后台 跳转到登录页面
        /// <summary>
        /// 跳转登录页面，在最顶端页面显示登录页
        /// </summary>
        public static void RedirectLoginOpenTopPageYunYing()
        {
            string script = "<script>window.top.location.href ='{0}';</script> ";
            HttpContext.Current.Response.Write(string.Format(script, Url_YunYingLogin));
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 跳转到登录页面，在最顶端页面显示登录页并指定登录成功后要跳转的页面地址
        /// </summary>
        /// <param name="url">指定登录成功后要跳转的页面地址</param>
        public static void RedirectLoginOpenTopPageYunYing(string url)
        {
            string script = "<script>window.top.location.href ='{0}';</script> ";
            HttpContext.Current.Response.Write(string.Format(script, Url_YunYingLogin + "?returnurl=" + HttpContext.Current.Server.UrlEncode(url)));
        }
        /// <summary>
        /// 跳转到登录页面，在最顶端页面显示登录页并指定登录成功后要跳转的页面地址，及在登录页面要显示的信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="message"></param>
        public static void RedirectLoginOpenTopPageYunYing(string url, string message)
        {
            string script = "<script>window.top.location.href ='{0}';</script> ";
            if (!string.IsNullOrEmpty(message))
            {
                HttpContext.Current.Response.Write(string.Format(script, Url_YunYingLogin + "?returnurl=" + HttpContext.Current.Server.UrlEncode(url) +
                    "&nlm=" + HttpContext.Current.Server.UrlEncode(message) + "&isShow=1"));
            }
            else
            {
                RedirectLoginOpenTopPageYunYing(url);
            }
        }
        #endregion

        #endregion
        
    }
}
