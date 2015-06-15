using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SiteStructure
{
    /// <summary>
    /// 描述：同行平台 基础设置 接口类
    /// 修改记录：
    /// 1. 2011-03-28 PM 曹胡生 创建
    /// </summary>
    public interface ISiteBasicConfig
    {
        /// <summary>
        /// 修改基础设置信息
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        bool UpdateSiteInfo(EyouSoft.Model.SiteStructure.SiteBasicConfig SiteBasicConfig);

        /// <summary>
        /// 获得公司基础设置信息
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        EyouSoft.Model.SiteStructure.SiteBasicConfig GetSiteBasicConfig(int CompanyId);
    }
}
