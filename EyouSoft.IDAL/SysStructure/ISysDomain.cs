using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SysStructure
{
    /// <summary>
    /// 公司域名数据访问接口
    /// </summary>
    public interface ISystemDomain
    {
        /// <summary>
        /// 获取域名信息
        /// </summary>
        /// <param name="domain">域名</param>
        /// <returns></returns>
        EyouSoft.Model.SysStructure.SystemDomain GetDomain(string domain);
        /// <summary>
        /// 验证域名是否重复，返回重复的域名信息集合
        /// </summary>
        /// <param name="domains">域名信息集合</param>
        /// <param name="sysId">系统编号 HasValue时排除该系统原有域名</param>
        /// <returns></returns>
        IList<string> IsExistsDomains(IList<string> domains, int? sysId);
        /// <summary>
        /// 获取域名信息集合
        /// </summary>
        /// <param name="sysId">系统编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.SysStructure.SystemDomain> GetDomains(int? sysId);
    }
}
