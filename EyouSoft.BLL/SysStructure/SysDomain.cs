using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.SysStructure
{
    /// <summary>
    /// 系统域名业务逻辑
    /// </summary>
    public class SystemDomain
    {
        private readonly EyouSoft.IDAL.SysStructure.ISystemDomain dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.SysStructure.ISystemDomain>();

        #region public members
        /// <summary>
        /// 获取域名信息
        /// </summary>
        /// <param name="domain">域名</param>
        /// <returns></returns>
        public EyouSoft.Model.SysStructure.SystemDomain GetDomain(string domain)
        {
           
            EyouSoft.Model.SysStructure.SystemDomain model = (EyouSoft.Model.SysStructure.SystemDomain)
                EyouSoft.Cache.Facade.EyouSoftCache.GetCache(EyouSoft.Cache.Tag.System.SystemDomain + domain);
            if (model == null)
            {
                model = dal.GetDomain(domain);
                if(model!=null)
                    EyouSoft.Cache.Facade.EyouSoftCache.Add(EyouSoft.Cache.Tag.System.SystemDomain + domain, model, DateTime.Now.AddHours(2));
            }

            return model;
        }

        /// <summary>
        /// 验证域名是否重复，返回重复的域名信息集合
        /// </summary>
        /// <param name="domains">域名信息集合</param>
        /// <param name="sysId">系统编号 HasValue时排除该系统原有域名</param>
        /// <returns></returns>
        public IList<string> IsExistsDomains(IList<string> domains, int? sysId)
        {
            if (domains == null || domains.Count < 0) return null;

            for (int i = 0; i < domains.Count;i++ )
            {
                domains[i] = domains[i].Replace("http://", "");
            }

            return dal.IsExistsDomains(domains, sysId);
        }

        /// <summary>
        /// 获取域名信息集合
        /// </summary>
        /// <param name="sysId">系统编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SysStructure.SystemDomain> GetDomains(int? sysId)
        {
            return dal.GetDomains(sysId);
        }
        #endregion
    }
}
