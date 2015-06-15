using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.SSOComponent.Entity;
using Eyousoft.Common.Page;
using System.Web.UI.HtmlControls;
using EyouSoft.Security.Membership;

namespace Eyousoft.Common.Page
{
    /// <summary>
    /// 创建人：张新兵，2011-4-6
    /// 地接用户基类
    /// </summary>
    public class AreaConnectPage : System.Web.UI.Page
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

                //判断当前登录用户 是否是 地接用户，如果不是地接用户 则跳转到对应用户类型的相应首页
                if (_userInfo.ContactInfo.UserType != EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.地接用户)
                {
                    if (_userInfo.ContactInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.组团用户)
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
                    else if (_userInfo.ContactInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.专线用户)
                    {
                        if (this.PageType == PageType.general)
                        {
                            Response.Redirect("/sanping/default.aspx", true);
                        }
                        else
                        {
                            string script = "<script type='text/javascript'>window.top.location.href ='/sanping/default.aspx';</script> ";
                            Response.Clear();
                            Response.Write(script);
                            Response.End();
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
                this.Title += "_" + _userInfo.CompanyName;
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
    }
}
