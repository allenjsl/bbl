using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-考勤管理IDAL
    /// 创建人：luofx 2011-01-18
    /// </summary>
    public interface IAttendanceInfo
    {
        /// <summary>
        /// 根据考勤编号获取单个考勤信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">考勤编号</param>
        /// <returns></returns>
        EyouSoft.Model.AdminCenterStructure.AttendanceInfo GetModel(int CompanyId, string Id);
        /// <summary>
        /// 获取某年月的考勤概况
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="StaffNo">员工编号</param>
        /// <param name="Year">年份</param>
        /// <param name="Month">月份</param>
        /// <returns>考勤概况实体</returns>
        EyouSoft.Model.AdminCenterStructure.AttendanceAbout GetAttendanceAbout(int CompanyId, int StaffNo, int Year, int Month);
        /// <summary>
        /// 按考勤时间获取单个员工考勤信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="StaffNo">员工编号</param>
        /// <param name="AddDate">考勤的时间</param>
        /// <returns></returns>
        IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo> GetList(int CompanyId, int StaffNo, DateTime AddDate);
        /// <summary>
        /// 获取考勤管理集合信息
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="ArchiveNo">员工号(为空时不作条件)</param>
        /// <param name="StaffName">员工名字(为空时不作条件)</param>
        /// <param name="DepartmentId">部门ID(为0时取所有)</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.AdminCenterStructure.AttendanceAbout> GetList(int PageSize, int PageIndex, ref int RecordCount, string ArchiveNo, string StaffName, int DepartmentId, int CompanyId);
        /// <summary>
        /// 按年份，月份获取员工考勤列表
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">考勤查询试题</param>
        /// <returns></returns>
        IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> GetList(int CompanyId, EyouSoft.Model.AdminCenterStructure.SearchInfo SearchInfo);
        /// <summary>
        /// 按部门获取考勤汇总信息集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">考勤查询实体</param>
        /// <returns></returns>
        IList<EyouSoft.Model.AdminCenterStructure.AttendanceByDepartment> GetAttendanceByDepartmentList(int CompanyId, EyouSoft.Model.AdminCenterStructure.SearchInfo SearchInfo);
        /// <summary>
        /// 考勤批量新增
        /// </summary>
        /// <param name="lists">考勤管理信息集合</param>
        /// <returns></returns>
        bool Add(IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo> lists);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="lists">考勤管理信息集合</param>
        /// <param name="AddDate">考勤时间</param>
        /// <returns></returns>
        bool Update(int CompanyId, IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo> lists, DateTime AddDate);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        bool Delete(int CompanyId, string Id);
    }
}
