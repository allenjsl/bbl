using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 专线商角色管理IDAL
    /// </summary>
    /// 创建人：luofx 2011-01-17
    public interface ISysRoleManage
    {
        /// <summary>
        /// 获取公司职务信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.SysRoleManage> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId);
        /// <summary>
        /// 获取角色信息实体
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">角色编号</param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.SysRoleManage GetModel(int CompanyId, int Id);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">角色信息实体</param>
        /// <returns></returns>
        bool Add(EyouSoft.Model.CompanyStructure.SysRoleManage model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">角色信息实体</param>
        /// <returns></returns>
        bool Update(EyouSoft.Model.CompanyStructure.SysRoleManage model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="RoleId">角色ID</param>
        /// <returns></returns>
        bool Delete(int CompanyId, params int[] RoleId);
    }
}
