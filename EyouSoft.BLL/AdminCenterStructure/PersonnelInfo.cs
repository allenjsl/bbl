using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-人事档案BLL
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class PersonnelInfo : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.AdminCenterStructure.IPersonnelInfo idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.AdminCenterStructure.IPersonnelInfo>();
        
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public PersonnelInfo() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public PersonnelInfo(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
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
        /// 获取认识档案信息实体
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="PersonId">职员编号</param>
        /// <returns></returns>
        public EyouSoft.Model.AdminCenterStructure.PersonnelInfo GetModel(int CompanyId, int PersonId)
        {
            return idal.GetModel(CompanyId, PersonId);
        }
        /// <summary>
        /// 获取人事档案列表(内部通讯录)信息
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="ReCordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">认识档案搜索实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> GetList(int PageSize, int PageIndex, ref int ReCordCount, int CompanyId, EyouSoft.Model.AdminCenterStructure.PersonnelSearchInfo SearchInfo)
        {
            return idal.GetList(PageSize, PageIndex, ref ReCordCount, CompanyId, SearchInfo);
        }
        /// <summary>
        /// 获取通讯录信息
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="ReCordCount"></param>
        /// <param name="CompanyId"></param>
        /// <param name="UserName">姓名</param>
        /// <param name="DepartmentId">部门编号(null或0时取所有)</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> GetList(int PageSize, int PageIndex, ref int ReCordCount, int CompanyId, string UserName, int? DepartmentId)
        {
            return idal.GetList(PageSize, PageIndex, ref ReCordCount, CompanyId, UserName, DepartmentId);
        }
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model">职工档案信息实体</param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.AdminCenterStructure.PersonnelInfo model)
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
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_人事档案.ToString() + "新增了人事档案信息数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "新增人事档案信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_人事档案;
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
        /// <param name="model">职工档案信息实体</param>
        /// <returns></returns>
        public bool Update(EyouSoft.Model.AdminCenterStructure.PersonnelInfo model)
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
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_人事档案.ToString() + "修改了人事档案信息数据,编号为："+model.Id;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "修改人事档案信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_人事档案;
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
        /// <param name="PersonId">员工编号</param>
        /// <returns></returns>
        public bool Delete(int CompanyId, params int[] PersonId)
        {
            bool IsTrue = false;
            IsTrue = idal.Delete(CompanyId, PersonId);
            #region LGWR
            if (IsTrue)
            {
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_人事档案.ToString() + "删除了人事档案信息数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "删除人事档案信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_人事档案;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
            }
            #endregion
            return IsTrue;                 
        }

        /// <summary>
        /// 获取人事工资信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <param name="query">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.MWageInfo> GetWages(int companyId, int year, int month, EyouSoft.Model.AdminCenterStructure.MWageSearchInfo query)
        {
            if (companyId < 1 || year < 1900 || month < 1 || month > 12) return null;

            return idal.GetWages(companyId, year, month, query);
        }

        /// <summary>
        /// 按年月设置人事工资信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <param name="operatorId">操作人编号</param>
        /// <param name="wages">人事工资信息集合</param>
        /// <returns></returns>
        public bool SetWages(int companyId, int year, int month, int operatorId, IList<EyouSoft.Model.AdminCenterStructure.MWageInfo> wages)
        {
            if (companyId < 1 || year < 1900 || month < 1 || month > 12) return false;

            return idal.SetWages(companyId, year, month, operatorId, wages);
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
