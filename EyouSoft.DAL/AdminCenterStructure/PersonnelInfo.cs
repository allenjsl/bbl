using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data.Common;

namespace EyouSoft.DAL.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-人事档案DAL
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class PersonnelInfo : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.AdminCenterStructure.IPersonnelInfo
    {
        private readonly EyouSoft.Data.EyouSoftTBL dcDal = null;
        private readonly Database _db = null;
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public PersonnelInfo()
        {
            _db = this.SystemStore;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);
        }
        #endregion

        #region 实现接口公共方法
        /// <summary>
        /// 获取认识档案信息实体
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="PersonId">职员编号</param>
        /// <returns></returns>
        public EyouSoft.Model.AdminCenterStructure.PersonnelInfo GetModel(int CompanyId, int PersonId)
        {
            EyouSoft.Model.AdminCenterStructure.PersonnelInfo model = null;
            model = (from item in dcDal.PersonnelInfo
                     where item.CompanyId == CompanyId && item.Id == PersonId
                     select new EyouSoft.Model.AdminCenterStructure.PersonnelInfo
                     {
                         Id = item.Id,
                         ArchiveNo = item.ArchiveNo,
                         BirthDate = item.BirthDate,
                         Birthplace = item.Birthplace,
                         CardId = item.CardId,
                         CompanyId = item.CompanyId,
                         ContactAddress = item.ContactAddress,
                         ContactMobile = item.ContactMobile,
                         ContactSex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.Sex), item.ContactSex.Value.ToString()),
                         ContactTel = item.ContactTel,
                         DepartmentId = item.DepartmentId,
                         DutyId = item.DutyId,
                         Email = item.Email,
                         EntryDate = item.EntryDate,
                         IsLeave = item.IsLeave == 1 ? true : false,
                         IsMarried = item.IsMarried == 1 ? true : false,
                         LeaveDate = item.LeaveDate,
                         MSN = item.Msn,
                         National = item.National,
                         PersonalType = (EyouSoft.Model.EnumType.AdminCenterStructure.PersonalType)Enum.Parse(typeof(EyouSoft.Model.EnumType.AdminCenterStructure.PersonalType), item.PersonalType.Value.ToString()),
                         PhotoPath = item.PhotoPath,
                         Politic = item.Politic,
                         QQ = item.Qq,
                         Remark = item.Remark,
                         ServiceYear = item.ServiceYear,
                         UserName = item.UserName,
                         SchoolList = (from School in dcDal.SchoolInfo  //学历信息
                                       where School.PersonId == item.Id
                                       select new EyouSoft.Model.AdminCenterStructure.SchoolInfo
                                       {
                                           Id = School.Id,
                                           Degree = (EyouSoft.Model.EnumType.AdminCenterStructure.DegreeType)Enum.Parse(typeof(EyouSoft.Model.EnumType.AdminCenterStructure.DegreeType), School.Degree.ToString()),
                                           EndDate = School.EndDate,
                                           Professional = School.Professional,
                                           Remark = School.Remark,
                                           SchoolName = School.SchoolName,
                                           StartDate = School.StartDate,
                                           StudyStatus = Convert.ToBoolean(School.StudyStatus),
                                       }).ToList<EyouSoft.Model.AdminCenterStructure.SchoolInfo>(),
                         HistoryList = (from History in dcDal.PersonalHistory  //履历信息
                                        where History.PersonId == item.Id
                                        select new EyouSoft.Model.AdminCenterStructure.PersonalHistory
                                        {
                                            Id = History.Id,
                                            EndDate = History.EndDate,
                                            Remark = History.Remark,
                                            StartDate = History.StartDate,
                                            TakeUp = History.TakeUp,
                                            WorkPlace = History.WorkPlace,
                                            WorkUnit = History.WorkUnit
                                        }).ToList<EyouSoft.Model.AdminCenterStructure.PersonalHistory>()
                     }).FirstOrDefault();
            return model;
        }
        /// <summary>
        /// 获取人事档案列表信息
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="ReCordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> GetList(int PageSize, int PageIndex, ref int ReCordCount, int CompanyId, EyouSoft.Model.AdminCenterStructure.PersonnelSearchInfo SearchInfo)
        {
            IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> ResultList = null;
            #region SQL处理
            string tableName = "tbl_PersonnelInfo";
            string identityColumnName = "Id";
            string orderByString = " IssueTime DESC";
            StringBuilder fields = new StringBuilder();
            fields.Append("[Id],[ArchiveNo],[BirthDate],[UserName],[ContactSex],MSN,QQ,EntryDate,DepartmentId,");
            fields.AppendFormat("(SELECT [Id],[DepartName] FROM tbl_CompanyDepartment a WHERE a.[id] IN(select [value] from  dbo.fn_split([tbl_PersonnelInfo].[DepartmentId],',')) AND [CompanyId]={0} FOR XML Raw,Root('Department'))AS [DepartmentXML],", CompanyId);
            fields.AppendFormat("(SELECT [JobName] FROM [tbl_DutyManager] b WHERE b.[id]=[tbl_PersonnelInfo].[dutyid] AND [CompanyId]={0}) AS [DutyName],", CompanyId);
            fields.Append("(SELECT DATEDIFF(YEAR,ISNULL(EntryDate,getdate()),getdate())) AS [WorkYear],[ContactTel],[ContactMobile],[Email]");
            StringBuilder query = new StringBuilder();
            query.AppendFormat("[CompanyId]={0}", CompanyId);
            if (!string.IsNullOrEmpty(SearchInfo.ArchiveNo))
            {
                query.AppendFormat(" AND ArchiveNo like'%{0}%'", SearchInfo.ArchiveNo);
            }
            if (!string.IsNullOrEmpty(SearchInfo.UserName))
            {
                query.AppendFormat(" AND UserName like'%{0}%'", SearchInfo.UserName);
            }
            if (SearchInfo.IsLeave.HasValue)
            {
                query.AppendFormat(" AND IsLeave={0}", SearchInfo.IsLeave == true ? 1 : 0);
            }
            if (SearchInfo.IsMarried.HasValue)
            {
                query.AppendFormat(" AND IsMarried={0}", SearchInfo.IsMarried == true ? 1 : 0);
            }
            if (SearchInfo.DutyId.HasValue)
            {
                query.AppendFormat(" AND DutyId={0}", SearchInfo.DutyId);
            }
            if (SearchInfo.BirthDateFrom.HasValue)
            {
                query.AppendFormat(" AND DATEDIFF(DAY,'{0}',BirthDate)>=0", SearchInfo.BirthDateFrom);
            }
            if (SearchInfo.BirthDateTo.HasValue)
            {
                query.AppendFormat(" AND DATEDIFF(DAY,BirthDate,'{0}')>=0", SearchInfo.BirthDateTo);
            }
            if (SearchInfo.PersonalType.HasValue && ((int)SearchInfo.PersonalType) >= 0)
            {
                query.AppendFormat(" AND PersonalType={0}", (int)SearchInfo.PersonalType);
            }
            if (SearchInfo.ContactSex.HasValue && ((int)SearchInfo.ContactSex) > 0)
            {
                query.AppendFormat(" AND ContactSex={0}", (int)SearchInfo.ContactSex);
            }
            if (SearchInfo.WorkYearFrom.HasValue && SearchInfo.WorkYearFrom > 0)
            {
                query.AppendFormat(" AND DATEDIFF(DAY,EntryDate,getdate())>={0}", SearchInfo.WorkYearFrom * 365);
            }
            if (SearchInfo.WorkYearTo.HasValue && SearchInfo.WorkYearTo > 0)
            {
                query.AppendFormat(" AND DATEDIFF(DAY,EntryDate,getdate())<={0}", SearchInfo.WorkYearTo * 365);
            }
            #endregion
            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref ReCordCount, tableName, identityColumnName, fields.ToString(), query.ToString(), orderByString))
            {
                ResultList = new List<EyouSoft.Model.AdminCenterStructure.PersonnelInfo>();
                while (dr.Read())
                {
                    EyouSoft.Model.AdminCenterStructure.PersonnelInfo model = new EyouSoft.Model.AdminCenterStructure.PersonnelInfo()
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        ArchiveNo = dr.IsDBNull(dr.GetOrdinal("ArchiveNo")) ? "" : dr.GetString(dr.GetOrdinal("ArchiveNo")),
                        MSN = dr.IsDBNull(dr.GetOrdinal("MSN")) ? "" : dr.GetString(dr.GetOrdinal("MSN")),
                        QQ = dr.IsDBNull(dr.GetOrdinal("QQ")) ? "" : dr.GetString(dr.GetOrdinal("QQ")),
                        UserName = dr.IsDBNull(dr.GetOrdinal("UserName")) ? "" : dr.GetString(dr.GetOrdinal("UserName")),
                        ContactSex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.Sex), dr.GetInt32(dr.GetOrdinal("ContactSex")).ToString()),
                        DepartmentList = this.GetDepartmentList(dr["DepartmentXML"].ToString()),
                        DepartmentId = dr["DepartmentId"].ToString(),
                        DutyName = dr.IsDBNull(dr.GetOrdinal("DutyName")) ? "" : dr.GetString(dr.GetOrdinal("DutyName")),
                        WorkYear = dr.IsDBNull(dr.GetOrdinal("WorkYear")) ? 0 : Convert.ToInt32(dr["WorkYear"].ToString()),
                        ContactTel = dr.IsDBNull(dr.GetOrdinal("ContactTel")) ? "" : dr.GetString(dr.GetOrdinal("ContactTel")),
                        ContactMobile = dr.IsDBNull(dr.GetOrdinal("ContactMobile")) ? "" : dr.GetString(dr.GetOrdinal("ContactMobile")),
                        Email = dr.IsDBNull(dr.GetOrdinal("Email")) ? "" : dr.GetString(dr.GetOrdinal("Email"))
                    };
                    if (dr.IsDBNull(dr.GetOrdinal("EntryDate")))
                    {
                        model.EntryDate = null;
                    }
                    else
                    {
                        model.EntryDate = dr.GetDateTime(dr.GetOrdinal("EntryDate"));
                    }
                    if (dr.IsDBNull(dr.GetOrdinal("BirthDate")))
                    {
                        model.BirthDate = null;
                    }
                    else
                    {
                        model.BirthDate = dr.GetDateTime(dr.GetOrdinal("BirthDate"));
                    }
                    ResultList.Add(model);
                    model = null;
                }
            };
            return ResultList;
        }
        /// <summary>
        /// 获取通讯录信息
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="ReCordCount"></param>
        /// <param name="CompanyId"></param>
        /// <param name="UserName">姓名</param>
        /// <param name="DepartmentId">部门编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> GetList(int PageSize, int PageIndex, ref int ReCordCount, int CompanyId, string UserName, int? DepartmentId)
        {
            IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> ResultList = null;
            #region SQL处理
            string tableName = "tbl_PersonnelInfo";
            string identityColumnName = "Id";
            StringBuilder fields = new StringBuilder();
            fields.Append("[Id],[UserName],[ContactTel],ContactSex,[ContactMobile],[Email],[QQ],[MSN],");
            fields.AppendFormat("(SELECT [Id],[DepartName] FROM tbl_CompanyDepartment a WHERE a.[id] IN(select [value] from  dbo.fn_split([tbl_PersonnelInfo].[DepartmentId],',')) AND [CompanyId]={0} FOR XML Raw,Root('Department'))AS [DepartmentXML] ", CompanyId);
            StringBuilder query = new StringBuilder();
            query.AppendFormat("[CompanyId]={0} AND IsLeave=0", CompanyId);
            if (!string.IsNullOrEmpty(UserName))
            {
                query.AppendFormat(" AND UserName like'%{0}%'", UserName);
            }
            if (DepartmentId.HasValue && DepartmentId > 0)
            {
                query.AppendFormat(" AND EXISTS(SELECT 1 FROM dbo.fn_split(DepartmentId,',') WHERE [VALUE]='{0}')", DepartmentId);
            }
            string orderByString = " IssueTime DESC";
            #endregion
            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref ReCordCount, tableName, identityColumnName, fields.ToString(), query.ToString(), orderByString))
            {
                ResultList = new List<EyouSoft.Model.AdminCenterStructure.PersonnelInfo>();
                while (dr.Read())
                {
                    EyouSoft.Model.AdminCenterStructure.PersonnelInfo model = new EyouSoft.Model.AdminCenterStructure.PersonnelInfo()
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        UserName = dr.IsDBNull(dr.GetOrdinal("UserName")) ? "" : dr.GetString(dr.GetOrdinal("UserName")),
                        ContactSex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.Sex), dr.GetInt32(dr.GetOrdinal("ContactSex")).ToString()),
                        DepartmentList = this.GetDepartmentList(dr["DepartmentXML"].ToString()),
                        ContactTel = dr.IsDBNull(dr.GetOrdinal("ContactTel")) ? "" : dr.GetString(dr.GetOrdinal("ContactTel")),
                        ContactMobile = dr.IsDBNull(dr.GetOrdinal("ContactMobile")) ? "" : dr.GetString(dr.GetOrdinal("ContactMobile")),
                        Email = dr.IsDBNull(dr.GetOrdinal("Email")) ? "" : dr.GetString(dr.GetOrdinal("Email")),
                        QQ = dr.IsDBNull(dr.GetOrdinal("QQ")) ? "" : dr.GetString(dr.GetOrdinal("QQ")),
                        MSN = dr.IsDBNull(dr.GetOrdinal("MSN")) ? "" : dr.GetString(dr.GetOrdinal("MSN"))
                    };
                    ResultList.Add(model);
                    model = null;
                }
            };
            return ResultList;
        }
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model">职工档案信息实体</param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.AdminCenterStructure.PersonnelInfo model)
        {
            bool IsTrue = false;
            EyouSoft.Data.PersonnelInfo DataModel = new EyouSoft.Data.PersonnelInfo();
            InputModelValue(DataModel, model);
            DataModel.IssueTime = DateTime.Now;

            #region 学历处理
            if (model.SchoolList != null && model.SchoolList.Count > 0)
            {
                ((List<EyouSoft.Model.AdminCenterStructure.SchoolInfo>)model.SchoolList).ForEach(item =>
                {
                    EyouSoft.Data.SchoolInfo DataSchoolModel = new EyouSoft.Data.SchoolInfo();
                    DataSchoolModel.Degree = Convert.ToByte((int)item.Degree);
                    DataSchoolModel.EndDate = item.EndDate;
                    DataSchoolModel.PersonId = model.Id;
                    DataSchoolModel.Professional = item.Professional;
                    DataSchoolModel.Remark = item.Remark;
                    DataSchoolModel.SchoolName = item.Remark;
                    DataSchoolModel.StartDate = item.StartDate;
                    DataSchoolModel.StudyStatus = (byte)(item.StudyStatus ? 1 : 0);
                    DataModel.PersonSchoolInfoList.Add(DataSchoolModel);
                    DataSchoolModel = null;
                });
            }
            #endregion 学历处理

            #region 履历处理
            if (model.HistoryList != null && model.HistoryList.Count > 0)
            {
                ((List<EyouSoft.Model.AdminCenterStructure.PersonalHistory>)model.HistoryList).ForEach(item =>
                {
                    EyouSoft.Data.PersonalHistory DataHistoryModel = new EyouSoft.Data.PersonalHistory();

                    DataHistoryModel.EndDate = item.EndDate;
                    DataHistoryModel.PersonId = model.Id;
                    DataHistoryModel.Remark = item.Remark;
                    DataHistoryModel.StartDate = item.StartDate;
                    DataHistoryModel.TakeUp = item.TakeUp;
                    DataHistoryModel.WorkPlace = item.WorkPlace;
                    DataHistoryModel.WorkUnit = item.WorkUnit;
                    DataModel.PersonPersonalHistoryList.Add(DataHistoryModel);
                    DataHistoryModel = null;
                });
            }
            #endregion 学历处理

            dcDal.PersonnelInfo.InsertOnSubmit(DataModel);
            dcDal.SubmitChanges();
            if (dcDal.ChangeConflicts.Count == 0)
            {
                IsTrue = true;
            }
            DataModel = null;
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
            EyouSoft.Data.PersonnelInfo DataModel = dcDal.PersonnelInfo.FirstOrDefault(item =>
                item.Id == model.Id && item.CompanyId == model.CompanyId
            );
            if (DataModel != null)
            {
                //实体赋值
                InputModelValue(DataModel, model);

                #region 学历处理
                if (model.SchoolList != null && model.SchoolList.Count > 0)
                {
                    if (this.SchoolDelete(model.Id))
                    {
                        ((List<EyouSoft.Model.AdminCenterStructure.SchoolInfo>)model.SchoolList).ForEach(item =>
                        {
                            EyouSoft.Data.SchoolInfo DataSchoolModel = new EyouSoft.Data.SchoolInfo();
                            DataSchoolModel.Degree = Convert.ToByte((int)item.Degree);
                            DataSchoolModel.EndDate = item.EndDate;
                            DataSchoolModel.PersonId = model.Id;
                            DataSchoolModel.Professional = item.Professional;
                            DataSchoolModel.Remark = item.Remark;
                            DataSchoolModel.SchoolName = item.SchoolName;
                            DataSchoolModel.StartDate = item.StartDate;
                            DataSchoolModel.StudyStatus = (byte)(item.StudyStatus ? 1 : 0);
                            DataModel.PersonSchoolInfoList.Add(DataSchoolModel);
                            DataSchoolModel = null;
                        });
                    }
                }
                else
                {
                    this.SchoolDelete(model.Id);
                }
                #endregion 学历处理

                #region 履历处理
                if (model.HistoryList != null && model.HistoryList.Count > 0)
                {
                    if (this.HistoryDelete(model.Id))
                    {
                        ((List<EyouSoft.Model.AdminCenterStructure.PersonalHistory>)model.HistoryList).ForEach(item =>
                        {
                            EyouSoft.Data.PersonalHistory DataHistoryModel = new EyouSoft.Data.PersonalHistory();

                            DataHistoryModel.EndDate = item.EndDate;
                            DataHistoryModel.PersonId = model.Id;
                            DataHistoryModel.Remark = item.Remark;
                            DataHistoryModel.StartDate = item.StartDate;
                            DataHistoryModel.TakeUp = item.TakeUp;
                            DataHistoryModel.WorkPlace = item.WorkPlace;
                            DataHistoryModel.WorkUnit = item.WorkUnit;
                            DataModel.PersonPersonalHistoryList.Add(DataHistoryModel);
                            DataHistoryModel = null;
                        });
                    }
                }
                else
                {
                    this.HistoryDelete(model.Id);
                }
                #endregion 学历处理
                dcDal.SubmitChanges();
            }
            if (dcDal.ChangeConflicts.Count == 0)
            {
                IsTrue = true;
            }
            DataModel = null;
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
            //bool IsTrue = false;
            //IEnumerable<EyouSoft.Data.PersonnelInfo> Lists = from item in dcDal.PersonnelInfo
            //                                                 where item.CompanyId == CompanyId && PersonId.Contains(item.Id)
            //                                                 select item;
            //if (Lists != null)
            //{
            //    if (this.SchoolDelete(PersonId) && this.HistoryDelete(PersonId))
            //    {
            //        dcDal.PersonnelInfo.DeleteAllOnSubmit<EyouSoft.Data.PersonnelInfo>(Lists);
            //        dcDal.SubmitChanges();
            //        if (dcDal.ChangeConflicts.Count == 0)
            //        {
            //            IsTrue = true;
            //        }
            //    }
            //    Lists = null;
            //}
            //return IsTrue;

            string cmdText = string.Format("DELETE FROM tbl_AttendanceInfo WHERE StaffNo IN({0}) AND CompanyId={1};DELETE FROM tbl_SchoolInfo WHERE PersonId IN({0});DELETE FROM tbl_PersonalHistory WHERE PersonId IN({0});DELETE FROM tbl_PersonnelInfo WHERE Id  IN({0}) AND CompanyId={1};", EyouSoft.Toolkit.Utils.GetSqlIdStrByArray(PersonId), CompanyId);
            DbCommand cmd = _db.GetSqlStringCommand(cmdText);

            return DbHelper.ExecuteSql(cmd, _db) > 0;
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
            IList<EyouSoft.Model.AdminCenterStructure.MWageInfo> items = new List<EyouSoft.Model.AdminCenterStructure.MWageInfo>();
            DbCommand cmd = _db.GetSqlStringCommand("SELECT * FROM tbl_Wage WHERE CompanyId=@CompanyId AND [Year]=@Year AND [Month]=@Month");
            _db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);
            _db.AddInParameter(cmd, "Year", DbType.Int32, year);
            _db.AddInParameter(cmd, "Month", DbType.Byte, month);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.AdminCenterStructure.MWageInfo()
                    {
                        BingJia = rdr.GetDecimal(rdr.GetOrdinal("BingJia")),
                        ChiDao = rdr.GetDecimal(rdr.GetOrdinal("ChiDao")),
                        CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        FanBu = rdr.GetDecimal(rdr.GetOrdinal("FanBu")),
                        GangWeiGongZi = rdr.GetDecimal(rdr.GetOrdinal("GangWeiGongZi")),
                        HuaBu = rdr.GetDecimal(rdr.GetOrdinal("HuaBu")),
                        IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime")),
                        JiaBanFei = rdr.GetDecimal(rdr.GetOrdinal("JiaBanFei")),
                        JiangJin = rdr.GetDecimal(rdr.GetOrdinal("JiangJin")),
                        Month = rdr.GetByte(rdr.GetOrdinal("Month")),
                        OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                        QianKuan = rdr.GetDecimal(rdr.GetOrdinal("QianKuan")),
                        QuanQinJiang = rdr.GetDecimal(rdr.GetOrdinal("QuanQinJiang")),
                        RegId = rdr.GetInt32(rdr.GetOrdinal("Id")),
                        SheBao = rdr.GetDecimal(rdr.GetOrdinal("SheBao")),
                        ShiJia = rdr.GetDecimal(rdr.GetOrdinal("ShiJia")),
                        ShiFaGongZi=rdr.GetDecimal(rdr.GetOrdinal("ShiFaGongZi")),
                        WuXian = rdr.GetDecimal(rdr.GetOrdinal("WuXian")),
                        XingMing = rdr["XingMing"].ToString(),
                        XingZhengFaKuan = rdr.GetDecimal(rdr.GetOrdinal("XingZhengFaKuan")),
                        Year = rdr.GetInt32(rdr.GetOrdinal("Year")),
                        YeWuFaKuan = rdr.GetDecimal(rdr.GetOrdinal("YeWuFaKuan")),
                        YingFaGongZi=rdr.GetDecimal(rdr.GetOrdinal("YingFaGongZi")),
                        YouBu = rdr.GetDecimal(rdr.GetOrdinal("YouBu")),
                        ZhiWei = rdr["ZhiWei"].ToString()
                    });
                }
            }

            return items;
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
            DbCommand cmd = _db.GetStoredProcCommand("proc_Wage_Set");
            _db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);
            _db.AddInParameter(cmd, "Year", DbType.Int32, year);
            _db.AddInParameter(cmd, "Month", DbType.Byte, month);
            _db.AddInParameter(cmd, "OperatorId", DbType.Int32, operatorId);

            StringBuilder xml = new StringBuilder();

            #region create xml
            xml.Append("<ROOT>");
            if (wages != null && wages.Count > 0)
            {
                foreach (var item in wages)
                {
                    xml.AppendFormat("<Info ");
                    xml.AppendFormat(" ZhiWei=\"{0}\" ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(item.ZhiWei));
                    xml.AppendFormat(" XingMing=\"{0}\" ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(item.XingMing));
                    xml.AppendFormat(" GangWeiGongZi=\"{0}\" ", item.GangWeiGongZi);
                    xml.AppendFormat(" QuanQinJiang=\"{0}\" ", item.QuanQinJiang);
                    xml.AppendFormat(" JiangJin=\"{0}\" ", item.JiangJin);
                    xml.AppendFormat(" FanBu=\"{0}\" ", item.FanBu);
                    xml.AppendFormat(" HuaBu=\"{0}\" ", item.HuaBu);
                    xml.AppendFormat(" YouBu=\"{0}\" ", item.YouBu);
                    xml.AppendFormat(" JiaBanFei=\"{0}\" ", item.JiaBanFei);
                    xml.AppendFormat(" ChiDao=\"{0}\" ", item.ChiDao);
                    xml.AppendFormat(" BingJia=\"{0}\" ", item.BingJia);
                    xml.AppendFormat(" ShiJia=\"{0}\" ", item.ShiJia);
                    xml.AppendFormat(" XingZhengFaKuan=\"{0}\" ", item.XingZhengFaKuan);
                    xml.AppendFormat(" YeWuFaKuan=\"{0}\" ", item.YeWuFaKuan);
                    xml.AppendFormat(" QianKuan=\"{0}\" ", item.QianKuan);
                    xml.AppendFormat(" YingFaGongZi=\"{0}\" ", item.YingFaGongZi);
                    xml.AppendFormat(" WuXian=\"{0}\" ", item.WuXian);
                    xml.AppendFormat(" SheBao=\"{0}\" ", item.SheBao);
                    xml.AppendFormat(" ShiFaGongZi=\"{0}\" ", item.ShiFaGongZi);
                    xml.Append("/>");
                }                
            }
            xml.Append("</ROOT>");
            #endregion

            _db.AddInParameter(cmd, "WagesXML", DbType.String, xml.ToString());
            _db.AddOutParameter(cmd, "Result", DbType.Int32, 4);

            int sqlExceptionCode = 0;

            try
            {
                DbHelper.RunProcedure(cmd, this._db);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                sqlExceptionCode = 0 - e.Number;
            }

            if (sqlExceptionCode < 0) return false;

            return Convert.ToInt32(this._db.GetParameterValue(cmd, "Result")) == 1 ? true : false;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 生成部门集合List
        /// </summary>
        /// <param name="DepartMentXml">要分析的XML字符串</param>
        /// <returns></returns>
        private IList<EyouSoft.Model.CompanyStructure.Department> GetDepartmentList(string DepartMentXml)
        {
            if (string.IsNullOrEmpty(DepartMentXml)) return null;
            IList<EyouSoft.Model.CompanyStructure.Department> ResultList = null;
            XElement root = XElement.Parse(DepartMentXml);
            var xRow = root.Elements("row");
            ResultList = new List<EyouSoft.Model.CompanyStructure.Department>();
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
        /// 删除某员工的学历信息
        /// </summary>
        /// <param name="PersonId">员工编号</param>
        /// <returns></returns>
        private bool SchoolDelete(params int[] PersonId)
        {
            bool IsTrue = false;
            IEnumerable<EyouSoft.Data.SchoolInfo> Lists = from item in dcDal.SchoolInfo
                                                          where PersonId.Contains(item.PersonId)
                                                          select item;
            dcDal.SchoolInfo.DeleteAllOnSubmit<EyouSoft.Data.SchoolInfo>(Lists);
            dcDal.SubmitChanges();
            if (dcDal.ChangeConflicts.Count == 0)
            {
                IsTrue = true;
            }
            Lists = null;
            return IsTrue;
        }
        /// <summary>
        /// 删除某员工的学历信息
        /// </summary>
        /// <param name="CompanyId">员工编号</param>
        /// <returns></returns>
        private bool HistoryDelete(params int[] PersonId)
        {
            bool IsTrue = false;
            IEnumerable<EyouSoft.Data.PersonalHistory> Lists = from item in dcDal.PersonalHistory
                                                               where PersonId.Contains(item.PersonId)
                                                               select item;
            if (Lists != null)
            {
                dcDal.PersonalHistory.DeleteAllOnSubmit<EyouSoft.Data.PersonalHistory>(Lists);
                dcDal.SubmitChanges();
                if (dcDal.ChangeConflicts.Count == 0)
                {
                    IsTrue = true;
                }
                Lists = null;
            }
            return IsTrue;
        }
        /// <summary>
        /// 给职员个人档案信息实体赋值
        /// </summary>
        /// <param name="DataModel">Linq端的实体</param>
        /// <param name="model">职员个人档案信息实体</param>
        private void InputModelValue(EyouSoft.Data.PersonnelInfo DataModel, EyouSoft.Model.AdminCenterStructure.PersonnelInfo model)
        {
            DataModel.ArchiveNo = model.ArchiveNo;
            DataModel.BirthDate = model.BirthDate;
            DataModel.Birthplace = model.Birthplace;
            DataModel.CardId = model.CardId;
            DataModel.CompanyId = model.CompanyId;
            DataModel.ContactAddress = model.ContactAddress;
            DataModel.ContactMobile = model.ContactMobile;
            DataModel.ContactSex = (int)model.ContactSex;
            DataModel.ContactTel = model.ContactTel;
            DataModel.DepartmentId = model.DepartmentId;
            DataModel.DutyId = model.DutyId;
            DataModel.Email = model.Email;
            DataModel.EntryDate = model.EntryDate;
            DataModel.IsLeave = model.IsLeave ? 1 : 0;
            DataModel.IsMarried = model.IsMarried ? 1 : 0;
            DataModel.IssueTime = System.DateTime.Now;
            DataModel.LeaveDate = model.LeaveDate;
            DataModel.Msn = model.MSN;
            DataModel.National = model.National;
            DataModel.OperatorId = model.OperatorId;
            DataModel.PersonalType = (int)model.PersonalType;
            DataModel.PhotoPath = model.PhotoPath;
            DataModel.Politic = model.Politic;
            DataModel.Qq = model.QQ;
            DataModel.Remark = model.Remark;
            DataModel.ServiceYear = model.ServiceYear;
            DataModel.UserName = model.UserName;
        }
        #endregion 私有方法
    }
}
