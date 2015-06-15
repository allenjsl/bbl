using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-考勤管理BLL
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class AttendanceInfo : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.AdminCenterStructure.IAttendanceInfo idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.AdminCenterStructure.IAttendanceInfo>();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AttendanceInfo()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public AttendanceInfo(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
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
        /// 按考勤时间获取单个员工考勤信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">考勤编号</param>        
        /// <returns></returns>
        public EyouSoft.Model.AdminCenterStructure.AttendanceInfo GetModel(int CompanyId, string Id)
        {
            return idal.GetModel(CompanyId, Id);
        }
        /// <summary>
        /// 获取某年月的考勤概况
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="StaffNo">员工编号</param>
        /// <param name="Year">年份</param>
        /// <param name="Month">月份</param>
        /// <returns>考勤概况实体</returns>
        public EyouSoft.Model.AdminCenterStructure.AttendanceAbout GetAttendanceAbout(int CompanyId, int StaffNo, int Year, int Month)
        {
            return idal.GetAttendanceAbout(CompanyId, StaffNo, Year, Month);
        }
        /// <summary>
        /// 获取考勤管理集合信息
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="ArchiveNo ">员工号</param>
        /// <param name="StaffName">员工名字</param>
        /// <param name="DepartmentId">部门ID</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.AttendanceAbout> GetList(int PageSize, int PageIndex, ref int RecordCount, string ArchiveNo, string StaffName, int DepartmentId, int CompanyId)
        {
            return idal.GetList(PageSize, PageIndex, ref RecordCount, ArchiveNo, StaffName, DepartmentId, CompanyId);
        }
        /// <summary>
        /// 按考勤时间获取单个员工考勤信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="StaffNo">员工编号</param>
        /// <param name="AddDate">考勤的时间</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo> GetList(int CompanyId, int StaffNo, DateTime AddDate)
        {
            return idal.GetList(CompanyId, StaffNo, AddDate);
        }
        /// <summary>
        /// 按年份，月份获取员工考勤列表
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">考勤查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> GetList(int CompanyId, EyouSoft.Model.AdminCenterStructure.SearchInfo SearchInfo)
        {
            if (!SearchInfo.Year.HasValue || SearchInfo.Year.Value == 0)
            {
                SearchInfo.Year = DateTime.Now.Year;
            }
            if (!SearchInfo.Month.HasValue || SearchInfo.Month.Value == 0)
            {
                SearchInfo.Month = DateTime.Now.Month;
            }
            return idal.GetList(CompanyId, SearchInfo);
        }
        /// <summary>
        /// 按部门获取考勤汇总信息集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">考勤查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.AttendanceByDepartment> GetAttendanceByDepartmentList(int CompanyId, EyouSoft.Model.AdminCenterStructure.SearchInfo SearchInfo)
        {
            if (!SearchInfo.Year.HasValue || SearchInfo.Year.Value == 0)
            {
                SearchInfo.Year = DateTime.Now.Year;
            }
            if (!SearchInfo.Month.HasValue || SearchInfo.Month.Value == 0)
            {
                SearchInfo.Month = DateTime.Now.Month;
            }
            return idal.GetAttendanceByDepartmentList(CompanyId,SearchInfo);
        }
        /// <summary>
        /// 考勤新增
        /// </summary>
        /// <param name="model">考勤管理信息实体</param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.AdminCenterStructure.AttendanceInfo model)
        {
            bool IsTrue = false;
            IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo> lists = new List<EyouSoft.Model.AdminCenterStructure.AttendanceInfo>();
            lists.Add(model);
            IsTrue = this.Add(lists);
            lists = null;
            return IsTrue;
        }
        /// <summary>
        /// 考勤批量新增
        /// </summary>
        /// <param name="lists">考勤管理信息集合</param>
        /// <returns></returns>
        public bool Add(IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo> lists)
        {
            bool IsTrue = false;
            IsTrue = idal.Add(lists);
            #region LGWR
            if (IsTrue)
            {
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_考勤管理.ToString() + "新增了考勤管理数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "新增考勤";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_考勤管理;
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
        /// <param name="CompanyId">公司编号</param>
        /// <param name="lists">考勤管理信息集合</param>
        /// <param name="AddDate">考勤时间</param>
        /// <returns></returns>
        public bool Update(int CompanyId, IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo> lists, DateTime AddDate)
        {
            bool IsTrue = false;
            IsTrue = idal.Update(CompanyId, lists, AddDate);
            #region LGWR
            if (IsTrue)
            {
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_考勤管理.ToString() + "修改了考勤管理数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "修改考勤信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_考勤管理;
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
        /// <param name="Id">考勤编号</param>
        /// <returns></returns>
        public bool Delete(int CompanyId, string Id)
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
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_考勤管理.ToString() + "删除了考勤管理数据，编号为：" + Id;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "删除考勤信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.行政中心_考勤管理;
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
