using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-培训计划BLL
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class TrainPlan : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.AdminCenterStructure.ITrainPlan idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.AdminCenterStructure.ITrainPlan>();
        
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public TrainPlan()
        {
        }
              /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public TrainPlan(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }
        #endregion 

        #region 公共方法
        /// <summary>
        /// 获取培训计划实体信息
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.AdminCenterStructure.TrainPlan GetModel(int CompanyId, int Id)
        {
            return idal.GetModel(CompanyId,Id);
        }
        /// <summary>
        /// 获取培训计划信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>    
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.TrainPlan> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, int UserId, int DepartmentId)
        {
            return idal.GetList(PageSize, PageIndex, ref RecordCount, CompanyId, UserId, DepartmentId);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">培训计划实体</param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.AdminCenterStructure.TrainPlan model)
        {
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
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_培训计划.ToString() + "新增了培训计划信息数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "新增培训计划信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_培训计划;
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
        /// <param name="model">培训计划实体</param>
        /// <returns></returns>
        public bool Update(EyouSoft.Model.AdminCenterStructure.TrainPlan model)
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
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_培训计划.ToString() + "修改了培训计划信息数据，编号为：" + model.Id;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "修改培训计划信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_培训计划;
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
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public bool Delete(int CompanyId, int Id)
        {
            bool IsTrue = false;
            IsTrue = idal.Delete(CompanyId, Id);
            #region LGWR
            if (IsTrue)
            {
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_培训计划.ToString() + "删除了培训计划信息数据，编号为：" + Id;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "删除培训计划信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_培训计划;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
            }
            #endregion
            return IsTrue;      
        }
        #endregion

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
