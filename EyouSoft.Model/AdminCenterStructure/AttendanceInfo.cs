using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-考勤管理信息实体
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class AttendanceInfo
    {
        #region Model

        /// <summary>
        /// 编号
        /// </summary>
        public string Id { set; get; }
        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId { set; get; }
        /// <summary>
        /// 员工编号
        /// </summary>
        public int? StaffNo { set; get; }
        /// <summary>
        /// 员工号(人事档案的档案编号)
        /// </summary>
        public string ArchiveNo { set; get; }
        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId { set; get; }
        /// <summary>
        /// 考勤状况(准点，迟到，早退，旷工，休假，外出，出团，请假，加班)
        /// </summary>
        public EyouSoft.Model.EnumType.AdminCenterStructure.WorkStatus WorkStatus { set; get; }
        /// <summary>
        /// 考勤时间
        /// </summary>
        public DateTime AddDate { set; get; }
        /// <summary>
        /// 请假原因
        /// </summary>
        public string Reason { set; get; }
        /// <summary>
        /// 请假（加班）时间起始
        /// </summary>
        public DateTime? BeginDate { set; get; }
        /// <summary>
        /// 请假（加班）时间结束
        /// </summary>
        public DateTime? EndDate { set; get; }
        /// <summary>
        /// 请假天数（加班时数）
        /// </summary>
        public decimal OutTime { set; get; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { set; get; }
        #endregion Model

    }
    /// <summary>
    /// 考勤汇总信息实体
    /// </summary>
    public class AttendanceByDepartment
    {
        /// <summary>
        /// 部门编号
        /// </summary>
        public int DepartmentId { set; get; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { set; get; }
        /// <summary>
        /// 人员信息
        /// </summary>
        public IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> PersonList { set; get; }
    }
    /// <summary>
    /// 考勤列表实体（当月考勤概况）
    /// </summary>
    public class AttendanceAbout
    {
        /// <summary>
        /// 员工号(人事档案的档案编号)
        /// </summary>
        public string ArchiveNo { set; get; }
        /// <summary>
        /// 员工编号
        /// </summary>
        public int? StaffNo { set; get; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string StaffName { set; get; }
        /// <summary>
        /// 所属部门
        /// </summary>
        public IList<EyouSoft.Model.CompanyStructure.Department> DepartmentList { set; get; }
        /// <summary>
        /// 准点（单位：天）
        /// </summary>
        public int Punctuality { set; get; }
        /// <summary>
        /// 迟到（单位：天）
        /// </summary>
        public int Late { set; get; }
        /// <summary>
        /// 早退（单位：天）
        /// </summary>
        public int LeaveEarly { set; get; }
        /// <summary>
        /// 旷工（单位：天）
        /// </summary>
        public int Absenteeism { set; get; }
        /// <summary>
        /// 休假（单位：天）
        /// </summary>
        public int Vacation { set; get; }
        /// <summary>
        /// 外出（单位：天）
        /// </summary>
        public int Out { set; get; }
        /// <summary>
        /// 出团（单位：天）
        /// </summary>
        public int Group { set; get; }
        /// <summary>
        /// 请假（单位：天）
        /// </summary>
        public decimal AskLeave { set; get; }
        /// <summary>
        /// 加班（单位：小时）
        /// </summary>
        public decimal OverTime { set; get; }
    }
    /// <summary>
    /// 考勤查询实体
    /// </summary>
    public class SearchInfo
    {
        /// <summary>
        /// 年份(为null或小于等于0时，取当前年份值)
        /// </summary>
        public int? Year { set; get; }
        /// <summary>
        /// 月份(为null或小于等于0时，取当前月份值)
        /// </summary>
        public int? Month { set; get; }
        /// <summary>
        /// 部门Id(为null或小于等于0时，不作条件)
        /// </summary>
        public int? DepartMentId { set; get; }
        /// <summary>
        /// 员工号(人事档案的档案编号)
        /// (为null或空时，不作条件)
        /// </summary>
        public string ArchiveNo { set; get; }
        /// <summary>
        /// 员工姓名(为null或""时，不作条件)
        /// </summary>
        public string StaffName { set; get; }
    }
}
