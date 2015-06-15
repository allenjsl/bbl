using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 公司部门信息数据层接口
    /// </summary>
    /// 鲁功源 2011-01-21
    public interface IDepartment
    {
        /// <summary>
        /// 添加部门信息
        /// </summary>
        /// <param name="model">部门实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(EyouSoft.Model.CompanyStructure.Department model);
        /// <summary>
        /// 修改部门信息
        /// </summary>
        /// <param name="model">部门实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Update(EyouSoft.Model.CompanyStructure.Department model);
        /// <summary>
        /// 获取公司部门实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.Department GetModel(int Id);
        /// <summary>
        /// 删除部门信息
        /// </summary>
        /// <param name="Id">主键集合</param>
        /// <returns>true:成功 false:失败</returns>
        bool Delete(int Id);

        /// <summary>
        /// 获取公司的所有部门信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="ParentDepartId">父级部门编号</param>
        /// <returns>部门信息集合</returns>
        IList<EyouSoft.Model.CompanyStructure.Department> GetList(int CompanyId, int ParentDepartId);

        /// <summary>
        /// 获取所有部门信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.Department> GetAllDept(int CompanyId);

        /// <summary>
        /// 根据用户编号和公司编号获取公司打印文件实体(如果当前用户部门获取到的实体为NULL,则获取总部的)
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.CompanyPrintTemplate GetDeptPrint(int userId, int companyId);

        /// <summary>
        /// 判断当前登录用户是否为发布者的上级部门
        /// </summary>
        /// <param name="pubId">登录者ID</param>
        /// <param name="curId">当前用户ID</param>
        /// <returns></returns>
        int JudgePermission(int pubId, int curId);

        /// <summary>
        /// 判断是否有下级部门
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns></returns>
        bool HasChildDept(int id);

        /// <summary>
        /// 判断该部门下是否有员工
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns></returns>
        bool HasDeptUser(int id, int companyId);

    }
}
