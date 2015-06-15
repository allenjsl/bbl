using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 用户信息数据层接口
    /// </summary>
    /// 鲁功源 2011-01-20
    public interface ICompanyUser
    {
        /// <summary>
        /// 判断E-MAIL是否已存在
        /// </summary>
        /// <param name="email">email地址</param>
        /// <param name="userId">当前修改Email的用户ID</param>
        /// <returns></returns>
        bool IsExistsEmail(string email, int userId);
        /// <summary>
        /// 判断用户名是否已存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="CompanyId">当前公司编号</param>
        /// <param name="Id">编号</param>
        /// <returns></returns>
        bool IsExists(int id,string userName,int CompanyId);
        /// <summary>
        /// 真实删除用户信息
        /// </summary>
        /// <param name="userIdList">用户ID列表</param>
        /// <returns></returns>
        bool Delete(params string[] userIdList);
        /// <summary>
        /// 移除用户(即虚拟删除用户)
        /// </summary>
        /// <param name="userIdList">用户ID列表</param>
        /// <returns></returns>
        bool Remove(params string[] userIdList);
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="model">用户信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(EyouSoft.Model.CompanyStructure.CompanyUser model);
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model">用户信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Update(EyouSoft.Model.CompanyStructure.CompanyUser model);
        /// <summary>
        /// 根据用户编号获取用户信息
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <returns>用户实体</returns>
        EyouSoft.Model.CompanyStructure.CompanyUser GetUserInfo(int UserId);
        /// <summary>
        /// 根据用户名密码获取用户信息
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.CompanyUser GetUserInfo(string UserName, string Pwd);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="password">密码实体类</param>
        /// <returns></returns>
        bool UpdatePassWord(int id, EyouSoft.Model.CompanyStructure.PassWord password);
        /// <summary>
        /// 获得管理员实体信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.CompanyUser GetAdminModel(int companyId);
        /// <summary>
        /// 获取指定公司下的所有帐号用户详细信息列表[注;不包括总帐号]
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总的记录数</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetList(int companyId, int pageSize, int pageIndex, ref int recordCount,int userId);
        /// <summary>
        /// 根据当前用户组织架构信息分页获取用户列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="CurrUserId">当前用户编号</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetList(int companyId, int pageSize, int pageIndex, ref int recordCount);
        /// <summary>
        /// 根据当前用户组织架构信息获取所有用户列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="CurrUserId">当前用户编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetList(int companyId, string CurrUserId);
        /// <summary>
        /// 状态设置
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="isEnable">是否停用,true:停用,false:启用</param>
        /// <returns></returns>
        bool SetEnable(int id, bool isEnable);

        /// <summary>
        /// 根据部门ID数组获取所有用户ID数组
        /// </summary>
        /// <param name="departIds"></param>
        /// <returns></returns>
        int[] GetUserIdsByDepartIds(int[] departIds);
        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="RoleId">角色编号</param>
        /// <param name="PermissionList">权限集合</param>
        /// <returns>是否成功</returns>
        bool SetPermission(int UserId, int RoleId, string[] PermissionList);

        /// <summary>
        /// 根据用户编号获取用户基本信息
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.ContactPersonInfo GetUserBasicInfo(int UserId);

        /// <summary>
        /// 根据部门编号获取同级及下级部门所有用户信息集合
        /// </summary>
        /// <param name="departmentId">部门编号</param>
        /// <returns></returns>
        IList<int> GetUsers(int departmentId);

        /// <summary>
        /// 获取该公司所有员工信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetCompanyUser(int companyId);

        /// <summary>
        /// 更新组团用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateZuTuan(EyouSoft.Model.CompanyStructure.CompanyUser model);
        /// <summary>
        /// 根据组团端用户编号分页获取相应的线路区域计调员
        /// </summary>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="TourUserId">组团用户编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.ContactPersonInfo> GetAreaJobsByTourUserID(int PageIndex, int PageSize, ref int RecordCount, int TourUserId);
    
        /// <summary>
        /// 根据联系人名称获取人员信息
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">起始页码</param>
        /// <param name="recordCount">记录总数</param>
        /// <param name="deptId">部门ID</param>
        /// <param name="contactName">联系人名称 模糊查询</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetListByContactName(int pageSize, int pageIndex, ref int recordCount, int deptId, string contactName, int companyId);

        /// <summary>
        /// 获取公司用户信息
        /// </summary>
        /// <param name="QueryModel">查询实体</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetCompanyUsers(Model.CompanyStructure.QueryCompanyUser QueryModel);
    }
}
