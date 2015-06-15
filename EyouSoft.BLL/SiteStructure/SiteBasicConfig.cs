using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.SiteStructure
{
    /// <summary>
    /// 描述：同行平台 基础设置 业务类
    /// 修改记录：
    /// 1. 2011-03-28 PM 曹胡生 创建
    /// </summary>
    public class SiteBasicConfig : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.SiteStructure.ISiteBasicConfig idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.SiteStructure.ISiteBasicConfig>();
       
        /// <summary>
        /// 修改基础设置信息
        /// </summary>
        /// <returns></returns>
        public bool UpdateSiteInfo(EyouSoft.Model.SiteStructure.SiteBasicConfig SiteBasicConfig)
        {
            if (SiteBasicConfig == null || SiteBasicConfig.CompanyId == 0) return false;
            return idal.UpdateSiteInfo(SiteBasicConfig);
        }

        /// <summary>
        /// 获得公司基础设置信息
        /// </summary>
        /// <returns></returns>
        public EyouSoft.Model.SiteStructure.SiteBasicConfig GetSiteBasicConfig(int CompanyId)
        {
            if (CompanyId == 0) return null;
            return idal.GetSiteBasicConfig(CompanyId);
        }
    }
}
