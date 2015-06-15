/************************************************************
 * 模块名称：系统信息实体
 * 功能说明：总后台开新系统
 * 创建人：周文超  2011-4-15 10:11:41
 * *********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SysStructure
{
    #region 系统信息业务实体基类
    /// <summary>
    /// 系统信息业务实体基类
    /// </summary>
    [Serializable]
    public class MSysBaseInfo
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SystemId { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 系统开通模块集合 BIG(FIRST)
        /// </summary>
        public int[] ModuleIds { get; set; }

        /// <summary>
        /// 系统开通栏目集合 SMALL(SECOND)
        /// </summary>
        public int[] PartIds { get; set; }

        /// <summary>
        /// 系统权限集合 (THIRD)
        /// </summary>
        public int[] PermissionIds { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
    #endregion

    #region 系统信息业务实体
    /// <summary>
    /// 系统信息业务实体
    /// </summary>
    [Serializable]
    public class MSysInfo : MSysBaseInfo
    {
        /// <summary>
        /// 系统域名信息
        /// </summary>
        public IList<SystemDomain> Domains { get; set; }
        /// <summary>
        /// 公司信息
        /// </summary>
        public Model.CompanyStructure.CompanyInfo CompanyInfo { get; set; }
        /// <summary>
        /// 公司总部部门信息
        /// </summary>
        public Model.CompanyStructure.Department DepartmentInfo { get; set; }
        /// <summary>
        /// 管理员信息
        /// </summary>
        public Model.CompanyStructure.CompanyUser AdminInfo { get; set; }
        /// <summary>
        /// 公司配置信息
        /// </summary>
        public Model.CompanyStructure.CompanyFieldSetting Setting { get; set; }
    }
    #endregion

    #region WebMaster系统列表信息业务实体
    /// <summary>
    /// WebMaster系统列表信息业务实体
    /// </summary>
    [Serializable]
    public class MLBSysInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MLBSysInfo() { }

        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysId { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SysName { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 公司联系人
        /// </summary>
        public string Realname { get; set; }
        /// <summary>
        /// 公司联系电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 公司联系传真
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 管理员账号编号
        /// </summary>
        public int AdminUserId { get; set; }
        /// <summary>
        /// 管理员账号用户名
        /// </summary>
        public string AdminUsername { get; set; }
        /// <summary>
        /// 管理员账号密码
        /// </summary>
        public string AdminPassword { get; set; }
        /// <summary>
        /// 系统创建时间
        /// </summary>
        public DateTime SysCreateTime { get; set; }
        /// <summary>
        /// 系统域名信息集合
        /// </summary>
        public IList<EyouSoft.Model.SysStructure.SystemDomain> Domains { get; set; }
    }
    #endregion

    #region 系统查询信息业务实体
    /// <summary>
    /// 系统查询信息业务实体
    /// </summary>
    [Serializable]
    public class MSysSearchInfo
    {

    }
    #endregion
}
