using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Security.Membership
{
    /// <summary>
    /// utility
    /// </summary>
    /// Author:汪奇志 2010-06-30
    public class Utility
    {
        /// <summary>
        /// 获取当前登录用户的公司编号
        /// </summary>
        /// <returns></returns>
        public int GetCurrentUserCompanyId()
        {
            EyouSoft.SSOComponent.Entity.UserInfo userInfo = UserProvider.GetUser();//new EyouSoft.Security.Membership.UserProvider().GetUser();

            if (userInfo != null)
            {
                return userInfo.CompanyID;
            }

            return 0;
        }

    }
}
