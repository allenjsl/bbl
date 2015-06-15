using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.EnumType.SysStructure
{
    #region 域名类型
    /// <summary>
    /// 域名类型
    /// </summary>
    public enum DomainType
    {
        /// <summary>
        /// 专线入口
        /// </summary>
        专线入口=0,
        /// <summary>
        /// 同行入口
        /// </summary>
        同行入口
    }
    #endregion

    #region 同行平台模板
    /// <summary>
    /// 同行平台模板
    /// </summary>
    public enum SiteTemplate
    {
        /// <summary>
        /// none
        /// </summary>
        None = 0,
        /// <summary>
        /// 模板一
        /// </summary>
        模板一 = 1
    }
    #endregion
}
