using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 公司部门信息BLL
    /// Author 2011-01-22
    /// </summary>
    public class Department
    {
        private readonly EyouSoft.IDAL.CompanyStructure.IDepartment Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.IDepartment>();
        private readonly BLL.CompanyStructure.SysHandleLogs handleLogsBll = new BLL.CompanyStructure.SysHandleLogs();

        #region private members
        /// <summary>
        /// 移除公司部门缓存
        /// </summary>
        /// <param name="companyId">公司编号</param>
        internal void RemoveDepartmentCache(int companyId)
        {
            EyouSoft.Cache.Facade.EyouSoftCache.Remove(string.Format(EyouSoft.Cache.Tag.Company.CompanyDepartment, companyId));
        }

        /// <summary>
        /// 添加日志记录
        /// </summary>
        /// <param name="actionName">操作名称</param>
        /// <param name="flag">操作状态</param>
        /// <returns></returns>
        private EyouSoft.Model.CompanyStructure.SysHandleLogs AddLogs(string actionName, bool flag)
        {
            EyouSoft.Model.CompanyStructure.SysHandleLogs model = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            model.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_组织机构;
            model.EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode;
            model.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_组织机构.ToString() + (flag ? actionName : actionName + "失败") + "了部门名称数据";
            model.EventTitle = (flag ? actionName : actionName + "失败") + Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_组织机构.ToString() + "数据";

            return model;
        }
        #endregion

        #region public members
        /// <summary>
        /// 添加部门信息
        /// </summary>
        /// <param name="model">部门实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.Department model)
        {
            bool dalResult=Dal.Add(model);

            if (dalResult)
            {
                this.RemoveDepartmentCache(model.CompanyId);
            }

            handleLogsBll.Add(AddLogs("添加", dalResult));
            return dalResult;
        }

        /// <summary>
        /// 修改部门信息
        /// </summary>
        /// <param name="model">部门实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.CompanyStructure.Department model)
        {
            bool dalResult = Dal.Update(model);

            if (dalResult)
            {
                this.RemoveDepartmentCache(model.CompanyId);
            }

            handleLogsBll.Add(AddLogs("修改", dalResult));
            return Dal.Update(model);
        }

        /// <summary>
        /// 获取公司部门实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.Department GetModel(int Id)
        {
            return Dal.GetModel(Id);
        }

        /// <summary>
        /// 删除部门信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="Id">部门编号</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(int companyId,int Id)
        {
            bool dalResult = Dal.Delete(Id);

            if (dalResult)
            {
                this.RemoveDepartmentCache(companyId);
            }

            handleLogsBll.Add(AddLogs("删除", dalResult));
            return dalResult;
        }

        /// <summary>
        /// 获取上级部门信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="ParentDepartId">父级部门编号</param>
        /// <returns>部门信息集合</returns>
        public IList<EyouSoft.Model.CompanyStructure.Department> GetList(int CompanyId, int ParentDepartId)
        {
            return Dal.GetList(CompanyId, ParentDepartId);
        }

        /// <summary>
        /// 获取所有部门信息
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.Department> GetAllDept(int CompanyId)
        {
            return Dal.GetAllDept(CompanyId);
        }

        /// <summary>
        /// 根据用户编号和公司编号获取公司打印文件实体(如果当前用户部门获取到的实体为NULL,则获取总部的)
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CompanyPrintTemplate GetDeptPrint(int userId, int companyId)
        {
            return Dal.GetDeptPrint(userId, companyId);
        }

        /// <summary>
        /// 判断当前登录用户是否为发布者的上级部门
        /// </summary>
        /// <param name="pubId">发布者ID</param>
        /// <param name="curId">当前用户ID</param>
        /// <returns>0不是上级部门，1是上级部门(包括上上..级),2是总部</returns>
        public int JudgePermission(int pubId, int curId)
        {
            return Dal.JudgePermission(pubId, curId);
        }

        /// <summary>
        /// 判断是否有下级部门
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns></returns>
        public bool HasChildDept(int id)
        {
            return Dal.HasChildDept(id);
        }

        /// <summary>
        /// 判断该部门下是否有员工
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        public bool HasDeptUser(int id, int companyId)
        {
            return Dal.HasDeptUser(id, companyId);
        }
        #endregion
    }
}
