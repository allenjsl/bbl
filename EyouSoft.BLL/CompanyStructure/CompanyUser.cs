using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 用户信息BLL
    /// Author 2011-01-22
    /// </summary>
    public class CompanyUser:EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.CompanyStructure.ICompanyUser Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.ICompanyUser>();
        private readonly BLL.CompanyStructure.SysHandleLogs handleLogsBll = new BLL.CompanyStructure.SysHandleLogs();

        #region 构造函数
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CompanyUser() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public CompanyUser(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }
        #endregion

        #region private members
        /// <summary>
        /// 移除公司部门缓存
        /// </summary>
        /// <param name="companyId">公司编号</param>
        private void RemoveDepartmentCache(int companyId)
        {
            EyouSoft.BLL.CompanyStructure.Department departmentbll = new EyouSoft.BLL.CompanyStructure.Department();
            departmentbll.RemoveDepartmentCache(companyId);
            departmentbll = null;
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
            model.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_组织机构.ToString() + (flag ? actionName : actionName + "失败") + "了部门人员数据";
            model.EventTitle = (flag ? actionName : actionName + "失败") + Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_组织机构.ToString() + "数据";

            return model;
        }
        #endregion

        #region public members
        /// <summary>
        /// 判断E-MAIL是否已存在
        /// </summary>
        /// <param name="email">email地址</param>
        /// <param name="userId">当前修改Email的用户ID</param>
        /// <returns></returns>
        public bool IsExistsEmail(string email, int userId)
        {
            return Dal.IsExistsEmail(email, userId);
        }

        /// <summary>
        /// 判断用户名是否已存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="CompanyId">当前公司编号</param>
        /// <returns></returns>
        public bool IsExists(int id,string userName, int CompanyId)
        {
            return Dal.IsExists(id,userName, CompanyId);
        }

        /* 暂不公开 汪奇志
        /// <summary>
        /// 真实删除用户信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="userIdList">用户ID列表</param>
        /// <returns></returns>
        public bool Delete(int companyId,params string[] userIdList)
        {
            return Dal.Delete(userIdList);
        }*/

        /// <summary>
        /// 移除用户(即虚拟删除用户)
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="userIdList">用户ID列表</param>
        /// <returns></returns>
        public bool Remove(int companyId,params string[] userIdList)
        {
            bool dalResult = Dal.Remove(userIdList);

            if (dalResult)
            {
                this.RemoveDepartmentCache(companyId);
            }

            handleLogsBll.Add(AddLogs("删除", dalResult));
            return dalResult;
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="model">用户信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.CompanyUser model)
        {
            bool dalResult = Dal.Add(model);

            if (dalResult)
            {
                this.RemoveDepartmentCache(model.CompanyId);
            }

            handleLogsBll.Add(AddLogs("添加", dalResult));
            return dalResult;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model">用户信息实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.CompanyStructure.CompanyUser model)
        {
            bool dalResult = Dal.Update(model);

            if (dalResult)
            {
                this.RemoveDepartmentCache(model.CompanyId);

                //修改密码
                UpdatePassWord(model.ID, model.PassWordInfo);
            }

            handleLogsBll.Add(AddLogs("修改", dalResult));

            return dalResult;
        }

        /// <summary>
        /// 根据用户编号获取用户信息
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <returns>用户实体</returns>
        public EyouSoft.Model.CompanyStructure.CompanyUser GetUserInfo(int UserId)
        {
            return Dal.GetUserInfo(UserId);
        }
        /// <summary>
        /// 根据用户编号获取用户基本信息
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.ContactPersonInfo GetUserBasicInfo(int UserId)
        {
            return Dal.GetUserBasicInfo(UserId);
        }
        /// <summary>
        /// 根据用户名密码获取用户信息
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CompanyUser GetUserInfo(string UserName, string Pwd)
        {
            return Dal.GetUserInfo(UserName, Pwd);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="password">密码实体类</param>
        /// <returns></returns>
        public bool UpdatePassWord(int id, EyouSoft.Model.CompanyStructure.PassWord password)
        {
            return Dal.UpdatePassWord(id, password);
        }

        /// <summary>
        /// 获得管理员实体信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CompanyUser GetAdminModel(int companyId)
        {
            return Dal.GetAdminModel(companyId);
        }

        /// <summary>
        /// 获取指定公司下的所有帐号用户详细信息列表[注;不包括总帐号]
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总的记录数</param>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetList(int companyId, int pageSize, int pageIndex, ref int recordCount,int userId)
        {
            return Dal.GetList(companyId, pageSize, pageIndex, ref recordCount,userId);
        }

        /// <summary>
        /// 获取某公司下所有用户信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总的记录数</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetUserInfo(int companyId, int pageSize, int pageIndex, ref int recordCount)
        {
            return Dal.GetList(companyId, pageSize, pageIndex, ref recordCount, 0);
        }

        /// <summary>
        /// 状态设置
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="isEnable">是否停用,true:停用,false:启用</param>
        /// <returns></returns>
        public bool SetEnable(int id, bool isEnable)
        {
            return Dal.SetEnable(id, isEnable);
        }

        /// <summary>
        /// 根据当前用户组织架构信息分页获取用户列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="CurrUserId">当前用户编号</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetList(int companyId, int pageSize, int pageIndex, ref int recordCount)
        {
            return Dal.GetList(companyId, pageSize, pageIndex, ref recordCount);
        }
        /// <summary>
        /// 根据当前用户组织架构信息获取所有用户列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetList(int companyId)
        {
            return Dal.GetList(companyId, base.HaveUserIds);
        }

        /// <summary>
        /// 根据部门ID数组获取所有用户ID数组
        /// </summary>
        /// <param name="departIds"></param>
        /// <returns></returns>
        public int[] GetUserIdsByDepartIds(int[] departIds)
        {
            return Dal.GetUserIdsByDepartIds(departIds);
        }

        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="RoleId">角色编号</param>
        /// <param name="PermissionList">权限集合</param>
        /// <returns>是否成功</returns>
        public bool SetPermission(int UserId, int RoleId, string[] PermissionList)
        {
            return Dal.SetPermission(UserId, RoleId, PermissionList);
        }

        /*/// <summary>
        /// 根据部门编号获取同级及下级部门所有用户信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="departmentId">部门编号</param>
        /// <returns></returns>
        public IList<int> GetUsers(int companyId,int departmentId)
        {
            IList<int> us = null;
            //IDictionary<部门编号,IList<用户编号>>
            IDictionary<int, IList<int>> items = (IDictionary<int, IList<int>>)EyouSoft.Cache.Facade.EyouSoftCache.GetCache(string.Format(EyouSoft.Cache.Tag.Company.CompanyDepartment, companyId));

            if (items == null || items.Count < 1)
            {
                us = Dal.GetUsers(departmentId);

                items = new Dictionary<int, IList<int>>();
                items.Add(departmentId, us);

                EyouSoft.Cache.Facade.EyouSoftCache.Add(string.Format(EyouSoft.Cache.Tag.Company.CompanyDepartment, companyId), items);
            }
            else
            {
                if (items.ContainsKey(departmentId))
                {
                    us = items[departmentId];
                }
                else
                {
                    us = Dal.GetUsers(departmentId);
                    items.Add(departmentId, us);

                    EyouSoft.Cache.Facade.EyouSoftCache.Add(string.Format(EyouSoft.Cache.Tag.Company.CompanyDepartment, companyId), items);
                }
            }

            return us;
        }*/

        /// <summary>
        /// 获取该公司所有员工信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetCompanyUser(int companyId)
        {
            return Dal.GetCompanyUser(companyId);
        }

        /// <summary>
        /// 更新组团用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateZuTuan(EyouSoft.Model.CompanyStructure.CompanyUser model)
        {
            return Dal.UpdateZuTuan(model);
        }
        /// <summary>
        /// 根据组团端用户编号分页获取相应的线路区域计调员
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="TourUserId">组团用户编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.ContactPersonInfo> GetAreaJobsByTourUserID(int PageSize, int PageIndex, ref int RecordCount, int TourUserId)
        {
            if (TourUserId <= 0)
                return null;
            return Dal.GetAreaJobsByTourUserID(PageIndex, PageSize, ref RecordCount, TourUserId);
        }

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
        public IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetListByContactName(int pageSize, int pageIndex, ref int recordCount, int deptId, string contactName)
        {
            return Dal.GetListByContactName(pageSize, pageIndex, ref recordCount, deptId, contactName, base.CompanyId);
        }


        /// <summary>
        /// 获取公司用户信息
        /// </summary>
        /// <param name="QueryModel">查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyUser> GetCompanyUsers(Model.CompanyStructure.QueryCompanyUser QueryModel)
        {
            if (QueryModel == null || QueryModel.CompanyId <= 0)
                return null;

            return Dal.GetCompanyUsers(QueryModel);
        }

        #endregion
    }
}
