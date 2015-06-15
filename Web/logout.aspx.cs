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
using EyouSoft.Security.Membership;
using EyouSoft.SSOComponent.Entity;
using EyouSoft.Common;

namespace Web
{
    /// <summary>
    /// 张新兵，2011-01-30
    /// 用于用户的 安全退出
    /// </summary>
    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string loginUrl = string.Empty;

            //初始化用户信息,根据用户信息获取登录URL
            UserInfo userInfo = null;
            bool _IsLogin = EyouSoft.Security.Membership.UserProvider.IsUserLogin(out userInfo);

            if (_IsLogin == true)
            {
                loginUrl = GetLoginUrl(userInfo.SysId, userInfo.UserType);
            }

            UserProvider.UserLogout(loginUrl);
        }

        #region private members
        /// <summary>
        /// 根据系统ID和用户类型，获取用户退出后要跳转的页面地址
        /// </summary>
        /// <param name="systemId">系统ID</param>
        /// <param name="userType">用户类型</param>
        /// <returns></returns>
        private string GetLoginUrl(int systemId, EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType userType)
        {
            string loginUrl = string.Empty;
            string zxurl = string.Empty;
            var items = new EyouSoft.BLL.SysStructure.SystemDomain().GetDomains(systemId);

            EyouSoft.Model.EnumType.SysStructure.DomainType domainType = EyouSoft.Model.EnumType.SysStructure.DomainType.专线入口;
            switch (userType)
            {
                case EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.地接用户: break;
                case EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.专线用户: break;
                case EyouSoft.Model.EnumType.CompanyStructure.CompanyUserType.组团用户: domainType = EyouSoft.Model.EnumType.SysStructure.DomainType.同行入口; break;
            }

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    if (!string.IsNullOrEmpty(loginUrl)) break;
                    if (string.IsNullOrEmpty(zxurl) && item.DomainType == EyouSoft.Model.EnumType.SysStructure.DomainType.专线入口)
                    {
                        zxurl = "http://" + item.Domain + item.Url;
                    }

                    if (string.IsNullOrEmpty(loginUrl) && item.DomainType == domainType)
                    {
                        loginUrl = "http://" + item.Domain + item.Url;
                    }
                }
            }

            if (string.IsNullOrEmpty(loginUrl)) loginUrl = zxurl;

            if (string.IsNullOrEmpty(loginUrl)) loginUrl = "/login.aspx";

            return loginUrl;
        }
        #endregion
    }
}
