using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 专线商公司角色信息BLL
    /// </summary>
    /// 创建人：luofx 2011-01-17
    public class SysRoleManage : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.CompanyStructure.ISysRoleManage idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.ISysRoleManage>();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public SysRoleManage()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public SysRoleManage(int[] DepartmentIds)
        {
            base.DepartIds = DepartmentIds;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 获取公司角色信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.SysRoleManage> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId)
        {
            return idal.GetList(PageSize,PageIndex,ref RecordCount,CompanyId);
        }
        /// <summary>
        /// 获取角色信息实体
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">角色编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.SysRoleManage GetModel(int CompanyId, int Id)
        {
            return idal.GetModel(CompanyId,Id);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">角色信息实体</param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.CompanyStructure.SysRoleManage model)
        {
            if (model != null && model.RoleName == "管理员") return false;

            bool IsTrue = false;
            IsTrue = idal.Add(model);
            #region LGWR
            if (IsTrue)
            {
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_角色管理.ToString() + "新增了角色管理信息数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "新增角色管理信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_角色管理;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
            }
            #endregion
            return IsTrue;               
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">角色信息实体</param>
        /// <returns></returns>
        public bool Update(EyouSoft.Model.CompanyStructure.SysRoleManage model)
        {
            bool IsTrue = false;
            IsTrue = idal.Update(model);
            #region LGWR
            if (IsTrue)
            {
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_角色管理.ToString() + "修改了角色管理信息数据,编号为："+model.Id;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "修改角色管理信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_角色管理;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
            }
            #endregion
            return IsTrue;                
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="RoleId">角色ID</param>
        /// <returns></returns>
        public bool Delete(int CompanyId,params int[] RoleId)
        {
            bool IsTrue = false;
            IsTrue = idal.Delete(CompanyId, RoleId);
            #region LGWR
            if (IsTrue)
            {
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_角色管理.ToString() + "删除了角色管理信息数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "删除角色管理信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_角色管理;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
            }
            #endregion
            return IsTrue;              
        }
        #endregion 公共方法

        #region 私有方法
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logInfo">日志信息</param>
        private void Logwr(EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo)
        {
            EyouSoft.BLL.CompanyStructure.SysHandleLogs logbll = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();
            logbll.Add(logInfo);
            logbll = null;
        }
        #endregion
    }
}
