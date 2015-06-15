using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SysStructure
{
    /// <summary>
    /// 管理后台系统管理数据访问接口
    /// </summary>
    /// Author:汪奇志 2011-04-23
    public interface ISys
    {
        /// <summary>
        /// 创建子系统，返回1成功，其它失败
        /// </summary>
        /// <param name="sysInfo">EyouSoft.Model.SysStructure.MSysInfo</param>
        /// <returns></returns>
        int CreateSys(EyouSoft.Model.SysStructure.MSysInfo sysInfo);
        /// <summary>
        /// 修改子系统，返回1成功，其它失败
        /// </summary>
        /// <param name="sysInfo"></param>
        /// <returns></returns>
        int UpdateSys(EyouSoft.Model.SysStructure.MSysInfo sysInfo);
        /// <summary>
        /// 获取系统信息，仅取WEBMASTER修改子系统时使用的数据
        /// </summary>
        /// <param name="SystemId">系统编号</param>
        /// <returns></returns>
        EyouSoft.Model.SysStructure.MSysInfo GetSysInfo(int sysId);
        /// <summary>
        /// 获取所有子系统信息集合
        /// </summary>
        /// <returns></returns>
        IList<Model.SysStructure.MLBSysInfo> GetSyss(Model.SysStructure.MSysSearchInfo searchInfo);
        /// <summary>
        /// 根据系统编号获取公司编号
        /// </summary>
        /// <param name="sysId">系统编号</param>
        /// <returns></returns>
        int GetCompanyIdBySysId(int sysId);
        /// <summary>
        /// 根据公司编号获取公司管理员编号
        /// </summary>
        /// <param name="compayId">公司编号</param>
        /// <returns></returns>
        int GetAdminIdByCompanyId(int compayId);
        /// <summary>
        /// 根据公司编号获取公司总部部门编号
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        int GetHeadOfficeIdByCompanyId(int companyId);
        /// <summary>
        /// 更新管理员账号信息，密码为空时不修改密码
        /// </summary>
        /// <param name="webmasterInfo">EyouSoft.Model.SysStructure.MWebmasterInfo</param>
        /// <returns></returns>
        bool UpdateWebmasterInfo(EyouSoft.Model.SysStructure.MWebmasterInfo webmasterInfo);
    }
}
