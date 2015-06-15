using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Cache.Tag
{
    /// <summary>
    /// 系统缓存标签
    /// </summary>
    public static class System
    {
        public const string SystemUser = "BBL_PLATFORM/SYSTEM/USER/";
        public const string SystemDomain = "BBL_PLATFORM/SYSTEM_DOMAIN/";

    }
    
    /// <summary>
    /// 公司缓存标签
    /// </summary>
    public static class Company
    {
        /// <summary>
        /// 登录用户
        /// </summary>
        public const string CompanyUser = "BBL_PLATFORM/COMPANY/{0}/USER/{1}";
        /// <summary>
        /// 公司部门
        /// </summary>
        public const string CompanyDepartment = "BBL_PLATFORM/COMPANY/{0}/Department";
        /// <summary>
        /// 公司配置
        /// </summary>
        public const string CompanyConfig = "BBL_PLATFORM/COMPANY/{0}/Config";
    }
}
