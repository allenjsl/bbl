using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.SysStructure
{
    /// <summary>
    /// 权限操作BLL
    /// xuqh 2011-01-24
    /// </summary>
    public class Permission
    {
        private readonly EyouSoft.IDAL.SysStructure.IPermission dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.SysStructure.IPermission>();

        /// <summary>
        /// 获取某个角色的所有权限
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public int[] GetRolePermission(int roleId, int CompanyId)
        {
            return dal.GetRolePermission(roleId, CompanyId);
        }

        /// <summary>
        /// 获取某个公司的所有权限
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public int[] GetCompanyPermission(int companyId)
        {
            return dal.GetCompanyPermission(companyId);
        }

        /// <summary>
        /// 获取某个系统的所有权限
        /// </summary>
        /// <param name="SysId"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SysStructure.PermissionCategory> GetAllPermission(int SysId)
        {
            return dal.GetAllPermission(SysId);
        }

        /// <summary>
        /// 设置权限(此方法为设置单个权限)
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="permissionId">权限编号</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="permissionName">权限名称</param>
        /// <returns></returns>
        public bool SetPermission(int roleId, int permissionId, int companyId, string permissionName)
        {
            return dal.SetPermission(roleId, permissionId, companyId, permissionName);
        }

        /// <summary>
        /// 设置角色权限(此方法批量设置权限)
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="companyId">公司ID</param>
        /// <param name="PermissionList">权限数组</param>
        /// <returns></returns>
        public bool SetRolePermission(int roleId, int companyId, string[] PermissionList)
        {
            return dal.SetRolePermission(roleId, companyId, PermissionList);
        }

        /// <summary>
        /// 获取系统所有栏目、子栏目、权限信息
        /// </summary>
        /// <returns></returns>
        public IList<EyouSoft.Model.SysStructure.PermissionCategory> GetSysPermissions()
        {
            return dal.GetSysPermissions();
        }

        /// <summary>
        /// 添加第一级栏目
        /// </summary>
        /// <param name="name">栏目名称</param>
        /// <returns></returns>
        public int InsertFirstSysPermission(string name)
        {
            if (string.IsNullOrEmpty(name)) return 0;
            return dal.InsertSysPermission(name, null, null);
        }

        /// <summary>
        /// 添加第二级栏目
        /// </summary>
        /// <param name="name">栏目名称</param>
        /// <param name="firstId">第一级栏目编号</param>
        /// <returns></returns>
        public int InsertSecondSysPermission(string name, int firstId)
        {
            if (firstId < 1 || string.IsNullOrEmpty(name)) return 0;
            return dal.InsertSysPermission(name, firstId, null);
        }

        /// <summary>
        /// 添加第三级栏目（权限）
        /// </summary>
        /// <param name="name">栏目名称</param>
        /// <param name="firstId">第一级栏目编号</param>
        /// <param name="secondId">第二级栏目编号</param>
        /// <returns></returns>
        public int InsertThirdSysPermission(string name, int firstId, int secondId)
        {
            if (firstId < 1 || secondId < 1 || string.IsNullOrEmpty(name)) return 0;
            return dal.InsertSysPermission(name, firstId, secondId);
        }
    }
}
