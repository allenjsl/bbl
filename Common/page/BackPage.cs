using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.SSOComponent.Entity;
using EyouSoft.Security.Membership;
using Common.Enum;
using System.Web.UI.HtmlControls;
namespace Eyousoft.Common.Page
{
    public enum PageType
    {
        general,
        boxyPage
    }
    public class BackPage:System.Web.UI.Page
    {
        /// <summary>
        /// 设置页面类型
        /// </summary>
        public PageType PageType = PageType.general;

        /// <summary>
        /// 页面请求类型，是浏览器正常请求还是Ajax请求
        /// </summary>
        private bool isAjaxConnect = false;

        private bool _IsLogin = false;
        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsLogin
        {
            get
            {
                return _IsLogin;
            }
        }

        private EyouSoft.SSOComponent.Entity.UserInfo _userInfo = null;
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo SiteUserInfo
        {
            get
            {
                return _userInfo;
            }
        }
        /// <summary>
        /// 获取当前用户所在的公司ID
        /// </summary>
        public int CurrentUserCompanyID
        {
            get
            {
                return _userInfo.CompanyID;
            }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            AddMetaContentType();

            //获取页面请求 类型
            string urlType = EyouSoft.Common.Utils.GetQueryStringValue("urltype");
            if (urlType == "pageajax")
            {
                isAjaxConnect = true;
            }

            //初始化用户信息
            UserInfo userInfo = null;
            _IsLogin = EyouSoft.Security.Membership.UserProvider.IsUserLogin(out userInfo);
            _userInfo = userInfo;

            if (!_IsLogin)//没有登录
            {
                //判断页面请求类型
                if (isAjaxConnect)//是Ajax请求
                {
                    Response.Clear();
                    Response.Write("{Islogin:false}");
                    Response.End();
                }
                else//普通浏览器请求
                {
                    if (this.PageType == PageType.general)
                    {
                        UserProvider.RedirectLogin(Request.Url.ToString());
                    }
                    else
                    {
                        //UserProvider.RedirectMinLoginPage(Request.Url.ToString());
                        Response.Clear();
                        Response.Write("<script type='text/javascript'>");
                        Response.Write("if(window.parent.Boxy==undefined||window.parent.Boxy==null){");
                        Response.Write("window.location.href='" + UserProvider.Url_Login + "';");
                        Response.Write("}else{");
                        Response.Write("window.location.href='" + UserProvider.Url_MinLogin + "?returnurl=" + Server.UrlEncode(Request.Url.ToString()) + "';");
                        Response.Write("}");
                        Response.Write("</script>");
                        Response.End();
                    }
                }
            }
            else//已登录
            {
                //判断当前登录用户类型，如果用户类型不是专线用户，则判断当前页面是否是共享使用
                if (_userInfo.UserType != EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.专线用户)//不是
                {
                    if (_userInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.组团用户)//如果是组团用户
                    {
                        //判断当前页面是否属于 专线用户和组团用户共用页面
                        bool isShared = EyouSoft.Common.Utils.SharedByTowUsers_URLS.Contains(Request.Url.AbsolutePath.ToLower());
                        if (isShared == false)//当前页面不共享
                        {
                            if (this.PageType == PageType.general)
                            {
                                Response.Redirect("/GroupEnd/Default.aspx", true);
                            }
                            else
                            {
                                string script = "<script>window.top.location.href ='/GroupEnd/Default.aspx';</script> ";
                                Response.Clear();
                                Response.Write(script);
                                Response.End();
                            }
                        }
                    }
                    else if (_userInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.地接用户)
                    {
                        //判断当前页面是否属于 专线用户和地接用户共用页面
                        bool isShared = EyouSoft.Common.Utils.SharedByAreaConectAndBackUser_URLS.Contains(Request.Url.AbsolutePath.ToLower());
                        if (isShared == false)//当前页面不共享
                        {
                            //跳转到地接用户的 个人中心 地接安排 页面
                            if (this.PageType == PageType.general)
                            {
                                Response.Redirect("/UserCenter/DjArrangeMengs/TravelAgencyList.aspx", true);
                            }
                            else
                            {
                                string script = "<script type='text/javascript'>window.top.location.href ='/UserCenter/DjArrangeMengs/TravelAgencyList.aspx';</script> ";
                                Response.Clear();
                                Response.Write(script);
                                Response.End();
                            }
                        }
                    }
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //给页面Title加上 当前用户的公司名称结尾
            if (this.Header != null)
            {
                this.Title += "_"+_userInfo.CompanyName;
            }
            
        }

        /// <summary>
        /// 判断当前用户是否有权限
        /// </summary>
        /// <param name="permissionId">权限ID</param>
        /// <returns></returns>
        public bool CheckGrant(TravelPermission permission)
        {
            if (_userInfo != null)
            {
                return _userInfo.PermissionList.Contains((int)permission);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 添加Content-Type Meta标记到页面头部
        /// </summary>
        protected virtual void AddMetaContentType()
        {
            HtmlMeta meta = new HtmlMeta();
            //meta.HttpEquiv = "content-type";
            //meta.Content = Response.ContentType + "; charset=" + Response.ContentEncoding.HeaderName;
            meta.Attributes["charset"] = Response.ContentEncoding.HeaderName;
            //Page.Header.Controls.Add(meta);
        }

        /// <summary>
        /// register alert script
        /// </summary>
        /// <param name="s">msg</param>
        protected void RegisterAlertScript(string s)
        {
            this.RegisterScript(string.Format("alert('{0}');", s));
        }

        /// <summary>
        /// register alert and redirect script
        /// </summary>
        /// <param name="s"></param>
        /// <param name="url">IsNullOrEmpty(url)=true page reload</param>
        protected void RegisterAlertAndRedirectScript(string s, string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                this.RegisterScript(string.Format("alert('{0}');window.location.href='{1}';", s, url));
            }
            else
            {
                this.RegisterScript(string.Format("alert('{0}');window.location.href=window.location.href;", s));
            }
        }

        /// <summary>
        /// register alert and reload script
        /// </summary>
        /// <param name="s"></param>
        protected void RegisterAlertAndReloadScript(string s)
        {
            RegisterAlertAndRedirectScript(s, null);
        }

        /// <summary>
        /// register scripts
        /// </summary>
        /// <param name="script"></param>
        protected void RegisterScript(string script)
        {
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), script, true);
        }

        /// <summary>
        /// response to xls
        /// </summary>
        /// <param name="s">response</param>
        protected void ResponseToXls(string s)
        {
            ResponseToXls(s, System.Text.Encoding.Default);
        }

        /// <summary>
        /// response to xls
        /// </summary>
        /// <param name="s">response</param>
        protected void ResponseToXls(string s,Encoding encoding)
        {
            Response.Clear();
            Response.ContentEncoding = encoding;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Write(s.ToString());
            Response.End();
        }

        /// <summary>
        /// Response.Clear();Response.Write(s);Response.End();
        /// </summary>
        /// <param name="s"></param>
        protected void RCWE(string s)
        {
            Response.Clear();
            Response.Write(s);
            Response.End();
        }
    }
}
