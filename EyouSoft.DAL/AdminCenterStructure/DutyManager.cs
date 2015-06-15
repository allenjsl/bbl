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
    /// 行政中心-职务管理DAL
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class DutyManager : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.AdminCenterStructure.IDutyManager
    {
        private EyouSoft.Data.EyouSoftTBL dcDal = null;
        private readonly Database _db = null;
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public DutyManager()
        {
            _db = this.SystemStore;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);
        }
        #endregion

        #region 实现接口公共方法
        /// <summary>
        /// 获取职务信息实体
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="DutyId">职务编号</param>
        /// <returns></returns>
        public EyouSoft.Model.AdminCenterStructure.DutyManager GetModel(int CompanyId, int DutyId)
        {
            EyouSoft.Model.AdminCenterStructure.DutyManager model = null;
            model = (from item in dcDal.DutyManager
                     where item.CompanyId == CompanyId && item.Id == DutyId
                     select new EyouSoft.Model.AdminCenterStructure.DutyManager
                     {
                         Id = item.Id,
                         Help = item.Help,
                         JobName = item.JobName,
                         Remark = item.Remark,
                         Requirement = item.Requirement
                     }).FirstOrDefault();
            return model;
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
            IList<EyouSoft.Model.AdminCenterStructure.DutyManager> ResultList = null;
            string tableName = "tbl_DutyManager";
            string identityColumnName = "Id";
            string fields = "[Id],[Help],[JobName],[Remark],[Requirement]";
            string query = string.Format("[CompanyId]={0}", CompanyId);
            string orderByString = " IssueTime DESC";
            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, tableName, identityColumnName, fields, query, orderByString))
            {
                ResultList = new List<EyouSoft.Model.AdminCenterStructure.DutyManager>();
                while (dr.Read())
                {
                    EyouSoft.Model.AdminCenterStructure.DutyManager model = new EyouSoft.Model.AdminCenterStructure.DutyManager()
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        Help = dr.IsDBNull(dr.GetOrdinal("Help")) ? "" : dr.GetString(dr.GetOrdinal("Help")),
                        JobName = dr.GetString(dr.GetOrdinal("JobName")),
                        Remark = dr.IsDBNull(dr.GetOrdinal("Remark")) ? "" : dr.GetString(dr.GetOrdinal("Remark")),
                        Requirement = dr.IsDBNull(dr.GetOrdinal("Requirement")) ? "" : dr.GetString(dr.GetOrdinal("Requirement"))
                    };
                    ResultList.Add(model);
                    model = null;
                }
            };
            return ResultList;
        }
        /// <summary>
        /// 获取所有职务信息（职务名称和ID值）
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.DutyManager> GetList(int CompanyId)
        {
            IList<EyouSoft.Model.AdminCenterStructure.DutyManager> ResultList = null;
            string StrSql = string.Format("SELECT [ID],[JobName] FROM [tbl_DutyManager] WHERE [CompanyId]={0} Order BY IssueTime DESC", CompanyId);
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                ResultList = new List<EyouSoft.Model.AdminCenterStructure.DutyManager>();
                while (dr.Read())
                {
                    EyouSoft.Model.AdminCenterStructure.DutyManager model = new EyouSoft.Model.AdminCenterStructure.DutyManager()
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        JobName = dr.GetString(dr.GetOrdinal("JobName")),
                    };
                    ResultList.Add(model);
                    model = null;
                }
            };
            return ResultList;
        }
        /// <summary>
        /// 判断是否已经有用户已经使用该职务
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="DutyId">职务编号</param>
        /// <returns></returns>
        public bool IsHasBeenUsed(int CompanyId, int DutyId)
        {
            bool IsTrue = false;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);
            EyouSoft.Data.PersonnelInfo DataModel = dcDal.PersonnelInfo.FirstOrDefault(item =>
                    item.CompanyId == CompanyId && item.DutyId == DutyId
            );
            if (DataModel != null)
            {
                IsTrue = true;
            }
            return IsTrue;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">公司账户信息实体</param>
        /// <returns>0:失败，1：成功，-1：职务名称重复</returns>
        public int Add(EyouSoft.Model.AdminCenterStructure.DutyManager model)
        {
            int EffectedCount = 0;
            if (this.IsExists(model.JobName, 0, model.CompanyId))
            {
                EffectedCount = -1;
            }
            else
            {
                EyouSoft.Data.DutyManager DutyModel = new EyouSoft.Data.DutyManager()
                {
                    CompanyId = model.CompanyId,
                    Help = model.Help,
                    IssueTime = System.DateTime.Now,
                    JobName = model.JobName,
                    OperatorId = model.OperatorId,
                    Remark = model.Remark,
                    Requirement = model.Requirement
                };
                dcDal.DutyManager.InsertOnSubmit(DutyModel);
                dcDal.SubmitChanges();
                EffectedCount = 1;
                DutyModel = null;
            }
            return EffectedCount;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">公司账户信息实体</param>
        /// <returns></returns>
        public int Update(EyouSoft.Model.AdminCenterStructure.DutyManager model)
        {
            int EffectedCount = 0;
            if (this.IsExists(model.JobName, model.Id, model.CompanyId))
            {
                EffectedCount = -1;
            }
            else
            {
                EyouSoft.Data.DutyManager DataModel = dcDal.DutyManager.FirstOrDefault(item =>
                   item.Id == model.Id && item.CompanyId == model.CompanyId
                );
                if (DataModel != null)
                {
                    DataModel.CompanyId = model.CompanyId;
                    DataModel.Help = model.Help;
                    DataModel.IssueTime = System.DateTime.Now;
                    DataModel.JobName = model.JobName;
                    DataModel.OperatorId = model.OperatorId;
                    DataModel.Remark = model.Remark;
                    DataModel.Requirement = model.Requirement;
                    dcDal.SubmitChanges();
                    EffectedCount = 1;
                }
                DataModel = null;
            }
            return EffectedCount;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public bool Delete(int CompanyId, int DutyId)
        {
            bool IsTrue = false;
            EyouSoft.Data.DutyManager DutyModel = dcDal.DutyManager.FirstOrDefault(item =>
                item.Id == DutyId && item.CompanyId == CompanyId
            );
            if (DutyModel != null)
            {
                dcDal.DutyManager.DeleteOnSubmit(DutyModel);
                dcDal.SubmitChanges();
                if (dcDal.ChangeConflicts.Count == 0)
                {
                    IsTrue = true;
                }
                DutyModel = null;
            }
            return IsTrue;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 判断是否存在重复的职务名称
        /// </summary>
        /// <param name="JobName">职务名称</param>
        /// <param name="Id">职务ID</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        private bool IsExists(string JobName, int Id,int companyId)
        {
            int Count = 0;
            string StrSql = " SELECT COUNT(1) FROM tbl_DutyManager WHERE JobName=@JobName AND CompanyId=@CompanyId ";
            if (Id > 0)
            {
                StrSql += " AND [ID]<>@Id";
            }
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            if (Id > 0)
            {
                this._db.AddInParameter(dc, "Id", DbType.Int32, Id);
            }
            this._db.AddInParameter(dc, "JobName", DbType.String, JobName);
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, companyId);
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                while (dr.Read())
                {
                    Count = Convert.ToInt32(dr[0].ToString());
                }
            }
            return Count > 0 ? true : false;
        }
        #endregion 私有方法
    }
}
