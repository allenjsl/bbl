using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SysStructure
{
    /// <summary>
    /// 权限操作接口
    /// Author xuqh 2011-01-23
    /// </summary>
    public interface IPermission
    {
        /// <summary>
        /// 获取某个角色的所有权限
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        int[] GetRolePermission(int roleId, int CompanyId);

        /// <summary>
        /// 获取某个公司的所有权限
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        int[] GetCompanyPermission(int companyId);

        /// <summary>
        /// 获取某个系统的所有权限
        /// </summary>
        /// <param name="SysId"></param>
        /// <returns></returns>
        IList<EyouSoft.Model.SysStructure.PermissionCategory> GetAllPermission(int SysId);

        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="permissionId">权限编号</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="permissionName">权限名称</param>
        /// <returns></returns>
        bool SetPermission(int roleId,int permissionId, int companyId, string permissionName);

        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="PermissionList">权限数组</param>
        /// <returns></returns>
        bool SetRolePermission(int roleId, int companyId, string[] PermissionList);
        /// <summary>
        /// 获取系统所有栏目、子栏目、权限信息
        /// </summary>
        /// <returns></returns>
        IList<EyouSoft.Model.SysStructure.PermissionCategory> GetSysPermissions();
        /// <summary>
        /// 添加系统权限基础数据，返回添加的编号
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="firstId">第一级编号</param>
        /// <param name="secondId">第二级编号</param>
        /// <returns></returns>
        int InsertSysPermission(string name, int? firstId, int? secondId);
    }
}
