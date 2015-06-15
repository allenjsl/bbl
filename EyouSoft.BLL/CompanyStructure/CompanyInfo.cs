using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 专线商公司信息BLL
    /// </summary>
    /// 创建人：luofx 2011-01-17
    public class CompanyInfo : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.CompanyStructure.ICompanyInfo idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.ICompanyInfo>();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompanyInfo()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompanyInfo(int[] DepartmentIds)
        {
            base.DepartIds = DepartmentIds;
        }
        #endregion 构造函数

        #region 公共方法
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">公司信息实体</param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.CompanyStructure.CompanyInfo model)
        {
            return idal.Add(model);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">公司信息实体</param>
        /// <returns></returns>
        public bool Update(EyouSoft.Model.CompanyStructure.CompanyInfo model)
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
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_公司信息.ToString() + "修改了公司信息数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "修改公司信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_公司信息;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
            }
            #endregion
            return IsTrue;   
        }
        /// <summary>
        /// 获取公司信息实体
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SystemId">系统编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CompanyInfo GetModel(int CompanyId, int SystemId)
        {
            return idal.GetModel(CompanyId, SystemId);
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
