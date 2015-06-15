using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.SSOComponent.Entity;
using EyouSoft.Security.Membership;

namespace Eyousoft.Common.Page
{
    public class FrontPage:System.Web.UI.Page
    {
        /// <summary>
        /// 设置页面类型
        /// </summary>
        public PageType PageType = PageType.general;

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

        /// <summary>
        /// 页面请求类型，是浏览器正常请求还是Ajax请求
        /// </summary>
        private bool isAjaxConnect = false;

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
        /// 获取当前用户所在的组团公司ID
        /// </summary>
        public int CurrentUserCompanyID
        {
            get
            {
                return _userInfo.TourCompany.TourCompanyId;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

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
                        UserProvider.RedirectMinLoginPage(Request.Url.ToString());
                    }
                }
            }
            else//已登录
            {

                //判断当前页面是否属于 专线用户和组团用户共用页面
                bool isShared = EyouSoft.Common.Utils.SharedByTowUsers_URLS.Contains(Request.Url.AbsolutePath.ToLower())
                    && (_userInfo.ContactInfo.UserType != EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.地接用户);

                if (isShared == false)//不共用
                {
                    //判断当前登录用户是否是组团用户，
                    //如果不是如果是专线用户则跳回到专线首页
                    //如果是地接用户 则跳转到地接用户的首页
                    if (_userInfo.ContactInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.专线用户)
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
                    else if (_userInfo.ContactInfo.UserType == EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.地接用户)
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

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //给页面Title加上 当前用户的公司名称结尾
            if (this.Header != null)
            {
                this.Title += "_组团_" + _userInfo.CompanyName;
            }
        }
    }
}
