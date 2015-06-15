using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-考勤管理DAL
    /// 创建人：luofx 2011-01-18
    /// </summary>
    public class AttendanceInfo : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.AdminCenterStructure.IAttendanceInfo
    {
        private readonly EyouSoft.Data.EyouSoftTBL dcDal = null;
        private readonly Database _db = null;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AttendanceInfo()
        {
            _db = this.SystemStore;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);
        }
        #endregion

        #region 实现接口公共方法
        /// <summary>
        /// 根据考勤编号获取单个考勤信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">考勤编号</param>
        /// <returns></returns>
        public EyouSoft.Model.AdminCenterStructure.AttendanceInfo GetModel(int CompanyId, string Id)
        {
            EyouSoft.Model.AdminCenterStructure.AttendanceInfo model = null;
            model = (from item in dcDal.AttendanceInfo
                     where item.CompanyId == CompanyId && item.Id == Id
                     select new EyouSoft.Model.AdminCenterStructure.AttendanceInfo
                     {
                         Id = item.Id,
                         AddDate = item.AddDate,
                         BeginDate = item.BeginDate,
                         CompanyId = item.CompanyId,
                         EndDate = item.EndDate,
                         OperatorId = item.OperatorId,
                         Reason = item.Reason,
                         OutTime = item.OutTime,
                         WorkStatus = (EyouSoft.Model.EnumType.AdminCenterStructure.WorkStatus)Enum.Parse(typeof(EyouSoft.Model.EnumType.AdminCenterStructure.WorkStatus), item.WorkStatus.ToString())
                     }).FirstOrDefault();
            return model;
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
            EyouSoft.Model.AdminCenterStructure.AttendanceAbout model = null;
            DateTime AddDate = new DateTime(Year, Month, 1);
            StringBuilder StrSql = new StringBuilder();
            StrSql.Append("SELECT at.[id] AS [StaffNo], at.companyid, at.[DepartmentId], at.[UserName] AS [StaffName],");
            StrSql.AppendFormat(" (SELECT Count(1) FROM tbl_AttendanceInfo d WHERE d .[StaffNo] = at.[id] AND d .[WorkStatus] = 0 AND datediff(mm, '{0}', d .[AddDate]) = 0) AS Punctuality,", AddDate.ToShortDateString());
            StrSql.AppendFormat(" (SELECT Count(1) FROM tbl_AttendanceInfo e WHERE e.[StaffNo] = at.[id] AND e.[WorkStatus] = 1 AND datediff(mm, '{0}', e.[AddDate]) = 0) AS Late,", AddDate.ToShortDateString());
            StrSql.AppendFormat(" (SELECT Count(1) FROM tbl_AttendanceInfo f WHERE f.[StaffNo] = at.[id] AND f.[WorkStatus] = 2 AND datediff(mm, '{0}', f.[AddDate]) = 0) AS LeaveEarly,", AddDate.ToShortDateString());
            StrSql.AppendFormat(" (SELECT Count(1) FROM tbl_AttendanceInfo g WHERE g.[StaffNo] = at.[id] AND g.[WorkStatus] = 3 AND datediff(mm, '{0}', g.[AddDate]) = 0) AS Absenteeism,", AddDate.ToShortDateString());
            StrSql.AppendFormat(" (SELECT Count(1) FROM tbl_AttendanceInfo h WHERE h.[StaffNo] = at.[id] AND h.[WorkStatus] = 4 AND datediff(mm, '{0}', h.[AddDate]) = 0) AS Vacation,", AddDate.ToShortDateString());
            StrSql.AppendFormat(" (SELECT Count(1) FROM tbl_AttendanceInfo j WHERE j.[StaffNo] = at.[id] AND j.[WorkStatus] = 5 AND datediff(mm, '{0}', j.[AddDate]) = 0) AS Out,", AddDate.ToShortDateString());
            StrSql.AppendFormat(" (SELECT Count(1) FROM tbl_AttendanceInfo k WHERE k.[StaffNo] = at.[id] AND k.[WorkStatus] = 6 AND datediff(mm, '{0}', k.[AddDate]) = 0) AS [Group],", AddDate.ToShortDateString());
            StrSql.AppendFormat(" (SELECT Count(1) FROM tbl_AttendanceInfo l WHERE l.[StaffNo] = at.[id] AND l.[WorkStatus] = 7 AND datediff(mm, '{0}', l.[AddDate]) = 0) AS AskLeave,", AddDate.ToShortDateString());
            StrSql.AppendFormat(" (SELECT SUM(OutTime) FROM tbl_AttendanceInfo m WHERE m.[StaffNo] = at.[id] AND m.[WorkStatus] = 8 AND datediff(mm,'{0}', m.[AddDate]) = 0) AS OverTime", AddDate.ToShortDateString());
            StrSql.AppendFormat(" FROM [tbl_PersonnelInfo] AS at WHERE at.[IsLeave] = 0 AND Id={0}", StaffNo);
            DbCommand dc = this._db.GetSqlStringCommand(StrSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                while (dr.Read())
                {
                    model = new EyouSoft.Model.AdminCenterStructure.AttendanceAbout()
                    {
                        StaffName = dr.IsDBNull(dr.GetOrdinal("StaffName")) ? "" : dr.GetString(dr.GetOrdinal("StaffName")),
                        StaffNo = dr.IsDBNull(dr.GetOrdinal("StaffNo")) ? 0 : dr.GetInt32(dr.GetOrdinal("StaffNo")),
                        Absenteeism = dr.IsDBNull(dr.GetOrdinal("Absenteeism")) ? 0 : dr.GetInt32(dr.GetOrdinal("Absenteeism")),
                        Group = dr.IsDBNull(dr.GetOrdinal("Group")) ? 0 : dr.GetInt32(dr.GetOrdinal("Group")),
                        Late = dr.IsDBNull(dr.GetOrdinal("Late")) ? 0 : dr.GetInt32(dr.GetOrdinal("Late")),
                        LeaveEarly = dr.IsDBNull(dr.GetOrdinal("LeaveEarly")) ? 0 : dr.GetInt32(dr.GetOrdinal("LeaveEarly")),
                        Out = dr.IsDBNull(dr.GetOrdinal("Out")) ? 0 : dr.GetInt32(dr.GetOrdinal("Out")),
                        Punctuality = dr.IsDBNull(dr.GetOrdinal("Punctuality")) ? 0 : dr.GetInt32(dr.GetOrdinal("Punctuality")),
                        Vacation = dr.IsDBNull(dr.GetOrdinal("Vacation")) ? 0 : dr.GetInt32(dr.GetOrdinal("Vacation")),
                        AskLeave = dr.IsDBNull(dr.GetOrdinal("AskLeave")) ? 0 : dr.GetInt32(dr.GetOrdinal("AskLeave")),
                        OverTime = dr.IsDBNull(dr.GetOrdinal("OverTime")) ? 0 : dr.GetDecimal(dr.GetOrdinal("OverTime"))
                    };
                }
            };
            return model;
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
            return (from item in dcDal.AttendanceInfo
                    where item.CompanyId == CompanyId && item.StaffNo == StaffNo && item.AddDate == AddDate
                     orderby item.IssueTime descending
                    select new EyouSoft.Model.AdminCenterStructure.AttendanceInfo
                    {
                        Id = item.Id,
                        AddDate = item.AddDate,
                        BeginDate = item.BeginDate,
                        CompanyId = item.CompanyId,
                        EndDate = item.EndDate,
                        OperatorId = item.OperatorId,
                        Reason = item.Reason,
                        OutTime = item.OutTime,
                        WorkStatus = (EyouSoft.Model.EnumType.AdminCenterStructure.WorkStatus)Enum.Parse(typeof(EyouSoft.Model.EnumType.AdminCenterStructure.WorkStatus), item.WorkStatus.ToString())
                    }).ToList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo>();
        }
        /// <summary>
        /// 获取考勤管理集合信息
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="ArchiveNo">员工号(为空时不作条件)</param>
        /// <param name="StaffName">员工名字(为空时不作条件)</param>
        /// <param name="DepartmentId">部门ID(为0时取所有)</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.AttendanceAbout> GetList(int PageSize, int PageIndex, ref int RecordCount, string ArchiveNo, string StaffName, int DepartmentId, int CompanyId)
        {
            IList<EyouSoft.Model.AdminCenterStructure.AttendanceAbout> ResultList = null;
            string tableName = "view_AttendanceInfo";
            string identityColumnName = "StaffNo";
            string fields = "[StaffNo],ArchiveNo,[StaffName],[Punctuality],[Late],[LeaveEarly],[Absenteeism],[Vacation],[Out],[Group],[AskLeave],[OverTime],[DepartmentXML]";
            string query = string.Format("[CompanyId]={0}", CompanyId);
            if (!string.IsNullOrEmpty(StaffName))
            {
                query = query + string.Format(" AND [StaffName] LIKE '%{0}%'", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(StaffName));
            }
            if (!string.IsNullOrEmpty(ArchiveNo))
            {
                query = query + string.Format(" AND [ArchiveNo] LIKE '%{0}%'", ArchiveNo);
            }
            if (DepartmentId > 0)
            {
                query = query + string.Format(" AND EXISTS(SELECT 1 FROM dbo.fn_split(DepartmentId,',') WHERE [VALUE]='{0}') ", DepartmentId);
            }
            string orderByString = " [StaffNo] DESC";
            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, tableName, identityColumnName, fields, query, orderByString))
            {
                ResultList = new List<EyouSoft.Model.AdminCenterStructure.AttendanceAbout>();
                while (dr.Read())
                {
                    EyouSoft.Model.AdminCenterStructure.AttendanceAbout model = new EyouSoft.Model.AdminCenterStructure.AttendanceAbout()
                    {
                        StaffName = dr.IsDBNull(dr.GetOrdinal("StaffName")) ? "" : dr.GetString(dr.GetOrdinal("StaffName")),
                        StaffNo = dr.IsDBNull(dr.GetOrdinal("StaffNo")) ? 0 : dr.GetInt32(dr.GetOrdinal("StaffNo")),
                        ArchiveNo = dr.IsDBNull(dr.GetOrdinal("ArchiveNo")) ? "" : dr.GetString(dr.GetOrdinal("ArchiveNo")),
                        Absenteeism = dr.IsDBNull(dr.GetOrdinal("Absenteeism")) ? 0 : dr.GetInt32(dr.GetOrdinal("Absenteeism")),
                        Group = dr.IsDBNull(dr.GetOrdinal("Group")) ? 0 : dr.GetInt32(dr.GetOrdinal("Group")),
                        Late = dr.IsDBNull(dr.GetOrdinal("Late")) ? 0 : dr.GetInt32(dr.GetOrdinal("Late")),
                        LeaveEarly = dr.IsDBNull(dr.GetOrdinal("LeaveEarly")) ? 0 : dr.GetInt32(dr.GetOrdinal("LeaveEarly")),
                        Out = dr.IsDBNull(dr.GetOrdinal("Out")) ? 0 : dr.GetInt32(dr.GetOrdinal("Out")),
                        Punctuality = dr.IsDBNull(dr.GetOrdinal("Punctuality")) ? 0 : dr.GetInt32(dr.GetOrdinal("Punctuality")),
                        Vacation = dr.IsDBNull(dr.GetOrdinal("Vacation")) ? 0 : dr.GetInt32(dr.GetOrdinal("Vacation")),
                        AskLeave = dr.IsDBNull(dr.GetOrdinal("AskLeave")) ? 0 : dr.GetInt32(dr.GetOrdinal("AskLeave")),
                        OverTime = dr.IsDBNull(dr.GetOrdinal("OverTime")) ? 0 : dr.GetDecimal(dr.GetOrdinal("OverTime"))
                    };
                    model.DepartmentList = GetDepartmentList(dr["DepartmentXML"].ToString());
                    ResultList.Add(model);
                    model = null;
                }
            };
            return ResultList;
        }
        /// <summary>
        /// 按年份，月份获取员工考勤列表
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">考勤查询试题</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> GetList(int CompanyId, EyouSoft.Model.AdminCenterStructure.SearchInfo SearchInfo)
        {
            IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> ResultList = new List<EyouSoft.Model.AdminCenterStructure.PersonnelInfo>();
            DateTime SearchAddDate = new DateTime(SearchInfo.Year.Value, SearchInfo.Month.Value, 1);
            StringBuilder StrSql = new StringBuilder();
            StrSql.Append("SELECT Id,UserName,ArchiveNo,");
            StrSql.Append(" (SELECT Id,DepartName FROM  tbl_CompanyDepartment WHERE ID IN(SELECT [VALUE] from dbo.fn_split(a.DepartmentId,',')) FOR XML RAW,ROOT('ROOT'))AS DepartmentXML,");
            StrSql.AppendFormat(" (SELECT StaffNo,WorkStatus,AddDate FROM tbl_AttendanceInfo WHERE DATEDIFF(month,AddDate,'{0}')=0 FOR XML RAW,ROOT('ROOT')) AS AttendanceInfoXML", SearchAddDate.ToShortDateString());
            StrSql.AppendFormat(" FROM tbl_PersonnelInfo a WHERE CompanyId={0} ", CompanyId);
            if (SearchInfo.DepartMentId.HasValue && SearchInfo.DepartMentId.Value > 0)
            {
                StrSql.AppendFormat(" AND EXISTS(SELECT 1 FROM dbo.fn_split(DepartmentId,',') WHERE [VALUE]='{0}')", SearchInfo.DepartMentId);
            }
            if (!string.IsNullOrEmpty(SearchInfo.ArchiveNo))
            {
                StrSql.AppendFormat(" AND ArchiveNo LIKE '%{0}%'", SearchInfo.ArchiveNo);
            }
            if (!string.IsNullOrEmpty(SearchInfo.StaffName))
            {
                StrSql.AppendFormat(" AND UserName LIKE '%{0}%'", SearchInfo.StaffName);
            }
            StrSql.Append(" ORDER BY IssueTime DESC");
            DbCommand dc = this._db.GetSqlStringCommand(StrSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                ResultList = new List<EyouSoft.Model.AdminCenterStructure.PersonnelInfo>();
                while (dr.Read())
                {
                    EyouSoft.Model.AdminCenterStructure.PersonnelInfo model = new EyouSoft.Model.AdminCenterStructure.PersonnelInfo()
                    {
                        UserName = dr.IsDBNull(dr.GetOrdinal("UserName")) ? "" : dr.GetString(dr.GetOrdinal("UserName")),
                        Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                        ArchiveNo = dr.IsDBNull(dr.GetOrdinal("ArchiveNo")) ? "" : dr.GetString(dr.GetOrdinal("ArchiveNo")),
                        AttendanceList = this.GetAttendanceList(dr["AttendanceInfoXML"].ToString())
                    };
                    model.DepartmentList = GetDepartmentList(dr["DepartmentXML"].ToString());
                    ResultList.Add(model);
                    model = null;
                }
            };
            return ResultList;
        }
        /// <summary>
        /// 按部门获取考勤汇总信息集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">考勤查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.AttendanceByDepartment> GetAttendanceByDepartmentList(int CompanyId, EyouSoft.Model.AdminCenterStructure.SearchInfo SearchInfo)
        {
            IList<EyouSoft.Model.AdminCenterStructure.AttendanceByDepartment> ResultList = null;
            string StrSql = string.Format("SELECT ID,DepartName  FROM tbl_CompanyDepartment WHERE CompanyId={0} ", CompanyId);
            if (SearchInfo.DepartMentId.HasValue && SearchInfo.DepartMentId.Value > 0)
            {
                StrSql += string.Format(" AND ID={0}", SearchInfo.DepartMentId);
            }
            StrSql+=" ORDER BY IssueTime DESC";
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                ResultList = new List<EyouSoft.Model.AdminCenterStructure.AttendanceByDepartment>();
                while (dr.Read())
                {
                    EyouSoft.Model.AdminCenterStructure.AttendanceByDepartment model = new EyouSoft.Model.AdminCenterStructure.AttendanceByDepartment()
                    {
                        DepartmentName = dr.IsDBNull(dr.GetOrdinal("DepartName")) ? "" : dr.GetString(dr.GetOrdinal("DepartName")),
                        DepartmentId = dr.IsDBNull(dr.GetOrdinal("ID")) ? 0 : dr.GetInt32(dr.GetOrdinal("ID")),
                    };
                    model.PersonList = GetPersonnelList(CompanyId, model.DepartmentId, SearchInfo);                    
                    ResultList.Add(model);
                    model = null;
                }
            };

            return ResultList;
        }
        /// <summary>
        /// 考勤批量新增
        /// </summary>
        /// <param name="lists">考勤管理信息集合</param>
        /// <returns></returns>
        public bool Add(IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo> lists)
        {
            bool IsTrue = false;
            string AttendanceInfoXML = CreateAttendanceInfoXML(lists);
            DbCommand dc = this._db.GetStoredProcCommand("proc_AttendanceInfo_Insert");
            this._db.AddInParameter(dc, "AttendanceInfoXML", DbType.AnsiString, AttendanceInfoXML);
            this._db.AddInParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, this._db);
            object Result = this._db.GetParameterValue(dc, "Result");
            if (!Result.Equals(null))
            {
                IsTrue = int.Parse(Result.ToString()) > 0 ? true : false;
            }
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
            string AttendanceInfoXML = CreateAttendanceInfoXML(lists);
            DbCommand dc = this._db.GetStoredProcCommand("proc_AttendanceInfo_Update");
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, CompanyId);
            this._db.AddInParameter(dc, "AddDate", DbType.DateTime, AddDate);
            this._db.AddInParameter(dc, "AttendanceInfoXML", DbType.AnsiString, AttendanceInfoXML);
            this._db.AddInParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, this._db);
            object Result = this._db.GetParameterValue(dc, "Result");
            if (!Result.Equals(null))
            {
                IsTrue = int.Parse(Result.ToString()) > 0 ? true : false;
            }
            return IsTrue;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public bool Delete(int CompanyId, string Id)
        {
            bool IsTrue = false;
            EyouSoft.Data.AttendanceInfo DataModel = dcDal.AttendanceInfo.FirstOrDefault(item =>
                item.Id == Id && item.CompanyId == CompanyId
            );
            if (DataModel != null)
            {
                dcDal.AttendanceInfo.DeleteOnSubmit(DataModel);
                dcDal.SubmitChanges();
                if (dcDal.ChangeConflicts.Count == 0)
                {
                    IsTrue = true;
                }
                DataModel = null;
            }
            return IsTrue;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 生成考勤信息XML
        /// </summary>
        /// <param name="Lists">考勤信息集合</param>
        /// <returns></returns>
        private string CreateAttendanceInfoXML(IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo> Lists)
        {
            if (Lists == null) return "";
            StringBuilder StrBuild = new StringBuilder();
            StrBuild.Append("<ROOT>");
            foreach (EyouSoft.Model.AdminCenterStructure.AttendanceInfo model in Lists)
            {
                StrBuild.AppendFormat("<AttendanceInfo CompanyId=\"{0}\"", model.CompanyId);
                StrBuild.AppendFormat(" AddDate=\"{0}\" ", model.AddDate);
                StrBuild.AppendFormat(" OperatorId=\"{0}\" ", model.OperatorId);
                StrBuild.AppendFormat(" BeginDate=\"{0}\" ", model.BeginDate);
                StrBuild.AppendFormat(" EndDate=\"{0}\" ", model.EndDate);
                StrBuild.AppendFormat(" OutTime=\"{0}\" ", model.OutTime);
                StrBuild.AppendFormat(" Reason=\"{0}\" ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(model.Reason));
                StrBuild.AppendFormat(" StaffNo=\"{0}\" ", model.StaffNo);
                StrBuild.AppendFormat(" WorkStatus=\"{0}\" />", (int)model.WorkStatus);
            }
            StrBuild.Append("</ROOT>");
            return StrBuild.ToString();
        }

        /// <summary>
        /// 根据部门获取所有员工信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="DepartmentId">部门编号</param>
        /// <returns></returns>
        private IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> GetPersonnelList(int CompanyId, int DepartmentId, EyouSoft.Model.AdminCenterStructure.SearchInfo SearchInfo)
        {
            DateTime SearchAddDate = new DateTime(SearchInfo.Year.Value, SearchInfo.Month.Value, 1);
            IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> ResultList = null;
            string StrSql = string.Format("SELECT ID,UserName,ArchiveNo,(SELECT StaffNo,WorkStatus,AddDate FROM tbl_AttendanceInfo WHERE DATEDIFF(month,AddDate,'{0}')=0 FOR XML RAW,ROOT('ROOT')) AS AttendanceInfoXML  FROM tbl_PersonnelInfo WHERE CompanyId={1} AND EXISTS(SELECT 1 FROM dbo.fn_split(DepartmentId,',') WHERE [VALUE]='{2}')", SearchAddDate.ToShortDateString(), CompanyId, DepartmentId);
            if (!string.IsNullOrEmpty(SearchInfo.ArchiveNo))
            {
                StrSql+=string.Format(" AND ArchiveNo LIKE '%{0}%'", SearchInfo.ArchiveNo);
            }
            if (!string.IsNullOrEmpty(SearchInfo.StaffName))
            {
                StrSql+=string.Format(" AND UserName LIKE '%{0}%'", SearchInfo.StaffName);
            }
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                ResultList = new List<EyouSoft.Model.AdminCenterStructure.PersonnelInfo>();
                while (dr.Read())
                {
                    EyouSoft.Model.AdminCenterStructure.PersonnelInfo model = new EyouSoft.Model.AdminCenterStructure.PersonnelInfo()
                    {
                        UserName = dr.IsDBNull(dr.GetOrdinal("UserName")) ? "" : dr.GetString(dr.GetOrdinal("UserName")),
                        Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id")),
                        ArchiveNo = dr.IsDBNull(dr.GetOrdinal("ArchiveNo")) ? "" : dr.GetString(dr.GetOrdinal("ArchiveNo")),
                        AttendanceList = this.GetAttendanceList(dr["AttendanceInfoXML"].ToString())
                    };
                    ResultList.Add(model);
                    model = null;
                }
            };
            return ResultList;
        }
        /// <summary>
        /// 生成部门集合List
        /// </summary>
        /// <param name="DepartMentXml">要分析的XML字符串</param>
        /// <returns></returns>
        private IList<EyouSoft.Model.CompanyStructure.Department> GetDepartmentList(string DepartMentXml)
        {
            if (string.IsNullOrEmpty(DepartMentXml)) return null;
            IList<EyouSoft.Model.CompanyStructure.Department> ResultList = null;
            ResultList = new List<EyouSoft.Model.CompanyStructure.Department>();
            XElement root = XElement.Parse(DepartMentXml);
            var xRow = root.Elements("row");
            foreach (var tmp1 in xRow)
            {
                EyouSoft.Model.CompanyStructure.Department model = new EyouSoft.Model.CompanyStructure.Department()
                {
                    Id = int.Parse(tmp1.Attribute("Id").Value),
                    DepartName = tmp1.Attribute("DepartName").Value
                };
                ResultList.Add(model);
                model = null;
            }
            return ResultList;
        }
        /// <summary>
        /// 生成考勤信息集合List
        /// </summary>
        /// <param name="AttendanceXml">考勤信息XML</param>
        /// <returns></returns>
        private IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo> GetAttendanceList(string AttendanceXml)
        {
            if (string.IsNullOrEmpty(AttendanceXml)) return null;
            IList<EyouSoft.Model.AdminCenterStructure.AttendanceInfo> ResultList = null;
            ResultList = new List<EyouSoft.Model.AdminCenterStructure.AttendanceInfo>();
            XElement root = XElement.Parse(AttendanceXml);            
            IEnumerable<XElement> xRow =root.Elements("row");            
            foreach (XElement tmp1 in xRow)
            {
                EyouSoft.Model.AdminCenterStructure.AttendanceInfo model = new EyouSoft.Model.AdminCenterStructure.AttendanceInfo()
                {
                    AddDate = DateTime.Parse(tmp1.Attribute("AddDate").Value),
                    StaffNo = int.Parse(tmp1.Attribute("StaffNo").Value),
                    WorkStatus = (EyouSoft.Model.EnumType.AdminCenterStructure.WorkStatus)Enum.Parse(typeof(EyouSoft.Model.EnumType.AdminCenterStructure.WorkStatus), tmp1.Attribute("WorkStatus").Value)
                };
                ResultList.Add(model);
                model = null;
            }
            return ResultList;
        }
        #endregion
    }
}
