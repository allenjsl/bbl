using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 张新兵 ，2011-02-14
    /// 管理网站使用的Cookie Key
    /// </summary>
    public class CookieKeyManage
    {
        /// <summary>
        /// 作为存储用户最后登录时间的KEY.
        /// 存储的时间格式为：year-month-day-hour-minutes-seconds.
        /// </summary>
        public const string LAST_LOGIN_TIME_COOKIE_KEY = "lastlogintime";
    }
}
