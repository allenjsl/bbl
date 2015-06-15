using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-职务管理BLL
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class DutyManager : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.AdminCenterStructure.IDutyManager idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.AdminCenterStructure.IDutyManager>();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public DutyManager()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public DutyManager(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }
        #endregion 构造函数

        #region 公共方法
        /// <summary>
        /// 获取职务信息实体
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="DutyId">职务编号</param>
        /// <returns></returns>
        public EyouSoft.Model.AdminCenterStructure.DutyManager GetModel(int CompanyId, int DutyId)
        {
            return idal.GetModel(CompanyId, DutyId);
        }
        /// <summary>
        /// 获取公司职务信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.DutyManager> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId)
        {
            return idal.GetList(PageSize, PageIndex, ref RecordCount, CompanyId);
        }
        /// <summary>
        /// 获取所有职务信息（职务名称和ID值）
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.DutyManager> GetList(int CompanyId)
        {
            return idal.GetList(CompanyId);
        }
        /// <summary>
        /// 判断是否已经有用户已经使用该职务
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="DutyId">职务编号</param>
        /// <returns></returns>
        public bool IsHasBeenUsed(int CompanyId, int DutyId)
        {
            return idal.IsHasBeenUsed(CompanyId, DutyId);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">公司账户信息实体</param>
        /// <returns>0:失败，1：成功，-1：职务名称重复</returns>
        public int Add(EyouSoft.Model.AdminCenterStructure.DutyManager model)
        {
            int RowCount = 0;
            RowCount = idal.Add(model);
            #region LGWR
            if (RowCount>0)
            {
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_职务管理.ToString() + "新增了职务信息数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "新增职务";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_职务管理;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
            }
            #endregion
            return RowCount;                
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">公司账户信息实体</param>
        /// <returns>0:失败，1：成功，-1：职务名称重复</returns>
        public int Update(EyouSoft.Model.AdminCenterStructure.DutyManager model)
        {
            int RowCount = 0;
            RowCount = idal.Update(model);
            #region LGWR
            if (RowCount>0)
            {
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_职务管理.ToString() + "修改了职务信息数据，编号为："+model.Id;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "修改职务信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_职务管理;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
            }
            #endregion
            return RowCount;                 
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="DutyId">职务编号</param>
        /// <returns></returns>
        public bool Delete(int CompanyId, int DutyId)
        {
            bool IsTrue = false;
            IsTrue = idal.Delete(CompanyId, DutyId);
            #region LGWR
            if (IsTrue)
            {
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_职务管理.ToString() + "删除了职务信息数据，编号为：" + DutyId;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "删除职务信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_职务管理;
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
