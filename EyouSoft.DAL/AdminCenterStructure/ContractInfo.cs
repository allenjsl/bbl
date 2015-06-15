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
    /// 行政中心-合同管理DAL
    /// 创建人：luofx 2011-01-18
    /// </summary>
    public class ContractInfo : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.AdminCenterStructure.IContractInfo
    {
        private readonly EyouSoft.Data.EyouSoftTBL dcDal = null;
        private readonly Database _db = null;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public ContractInfo()
        {
            _db = this.SystemStore;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);
        }
        #endregion

        #region 实现接口公共方法
        /// <summary>
        /// 获取合同管理实体信息
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.AdminCenterStructure.ContractInfo GetModel(int CompanyId, int Id)
        {
            EyouSoft.Model.AdminCenterStructure.ContractInfo model = null;
            model = (from item in dcDal.ContractInfo
                     where item.CompanyId == CompanyId && item.Id == Id
                     select new EyouSoft.Model.AdminCenterStructure.ContractInfo
                     {
                         Id = item.Id,
                         BeginDate = item.BeginDate,
                         EndDate = item.EndDate,
                         ContractStatus = (EyouSoft.Model.EnumType.AdminCenterStructure.ContractStatus)Enum.Parse(typeof(EyouSoft.Model.EnumType.AdminCenterStructure.ContractStatus), item.ContractStatus.ToString()),
                         StaffName = item.StaffName,
                         Remark = item.Remark,
                         StaffNo = item.StaffNo
                     }).FirstOrDefault();
            return model;
        }
        /// <summary>
        /// 获取合同信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">合同管理查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.AdminCenterStructure.ContractInfo> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, EyouSoft.Model.AdminCenterStructure.ContractSearchInfo SearchInfo)
        {
            IList<EyouSoft.Model.AdminCenterStructure.ContractInfo> ResultList = null;
            string tableName = "tbl_ContractInfo";
            string identityColumnName = "Id";
            string fields = " [Id],[StaffNo],[StaffName],[BeginDate],[EndDate],[ContractStatus],[Remark] ";
            string query = string.Format("[CompanyId]={0}", CompanyId);
            if (!string.IsNullOrEmpty(SearchInfo.StaffNo))
            {
                query = query + string.Format("AND StaffNo LIKE '%{0}%' ", SearchInfo.StaffNo);
            }
            if (!string.IsNullOrEmpty(SearchInfo.StaffName))
            {
                query = query + string.Format("AND StaffName LIKE '%{0}%' ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(SearchInfo.StaffName));
            }
            if (SearchInfo.BeginFrom.HasValue)
            {
                query = query + string.Format(" AND DATEDIFF(DAY,'{0}',BeginDate)>=0", SearchInfo.BeginFrom);
            }
            if (SearchInfo.BeginTo.HasValue)
            {
                query = query + string.Format(" AND DATEDIFF(DAY,BeginDate,'{0}')>=0", SearchInfo.BeginTo);
            }
            if (SearchInfo.EndFrom.HasValue)
            {
                query = query + string.Format(" AND DATEDIFF(DAY,'{0}',EndDate)>=0", SearchInfo.EndFrom);
            }
            if (SearchInfo.EndTo.HasValue)
            {
                query = query + string.Format(" AND DATEDIFF(DAY,EndDate,'{0}')>=0", SearchInfo.EndTo);
            }
            string orderByString = " IssueTime DESC";            
            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, tableName, identityColumnName, fields, query, orderByString))
            {
                ResultList = new List<EyouSoft.Model.AdminCenterStructure.ContractInfo>();
                while (dr.Read())
                {
                    EyouSoft.Model.AdminCenterStructure.ContractInfo model = new EyouSoft.Model.AdminCenterStructure.ContractInfo()
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        BeginDate = dr.GetDateTime(dr.GetOrdinal("BeginDate")),
                        EndDate = dr.GetDateTime(dr.GetOrdinal("EndDate")),
                        ContractStatus = (EyouSoft.Model.EnumType.AdminCenterStructure.ContractStatus)Enum.Parse(typeof(EyouSoft.Model.EnumType.AdminCenterStructure.ContractStatus), dr.GetInt32(dr.GetOrdinal("ContractStatus")).ToString()),
                        Remark = dr.IsDBNull(dr.GetOrdinal("Remark")) ? "" : dr.GetString(dr.GetOrdinal("Remark")),
                        StaffName = dr.IsDBNull(dr.GetOrdinal("StaffName")) ? "" : dr.GetString(dr.GetOrdinal("StaffName")),
                        StaffNo = dr.GetString(dr.GetOrdinal("StaffNo"))
                    };
                    ResultList.Add(model);
                    model = null;
                }
            };
            return ResultList;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">合同信息实体</param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.AdminCenterStructure.ContractInfo model)
        {
            bool IsTrue = false;
            EyouSoft.Data.ContractInfo DataModel = new EyouSoft.Data.ContractInfo()
            {
                BeginDate = model.BeginDate,
                EndDate = model.EndDate,
                ContractStatus = (int)model.ContractStatus,
                StaffName = model.StaffName,
                Remark = model.Remark,
                StaffNo = model.StaffNo,
                CompanyId = model.CompanyId,
                OperatorId = model.OperatorId,
                IssueTime = System.DateTime.Now
            };
            dcDal.ContractInfo.InsertOnSubmit(DataModel);
            dcDal.SubmitChanges();
            IsTrue = true;
            DataModel = null;
            return IsTrue;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">合同信息实体</param>
        /// <returns></returns>
        public bool Update(EyouSoft.Model.AdminCenterStructure.ContractInfo model)
        {
            bool IsTrue = false;
            EyouSoft.Data.ContractInfo DataModel = dcDal.ContractInfo.FirstOrDefault(item =>
               item.Id == model.Id && item.CompanyId == model.CompanyId
            );
            if (DataModel != null)
            {
                DataModel.BeginDate = model.BeginDate;
                DataModel.EndDate = model.EndDate;
                DataModel.OperatorId = model.OperatorId;
                DataModel.Remark = model.Remark;
                DataModel.StaffName = model.StaffName;
                DataModel.StaffNo = model.StaffNo;
                DataModel.ContractStatus = (int)model.ContractStatus;
                dcDal.SubmitChanges();
                IsTrue = true;
            }
            DataModel = null;
            return IsTrue;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">合同信息编号</param>
        /// <returns></returns>
        public bool Delete(int CompanyId, int Id)
        {
            bool IsTrue = false;
            EyouSoft.Data.ContractInfo DataModel = dcDal.ContractInfo.FirstOrDefault(item => item.Id == Id && item.CompanyId == CompanyId);
            if (DataModel != null)
            {
                dcDal.ContractInfo.DeleteOnSubmit(DataModel);
                dcDal.SubmitChanges();
                if (dcDal.ChangeConflicts.Count == 0)
                {
                    IsTrue = true;
                }
                DataModel = null;
            }
            return IsTrue;
        }

        /// <summary>
        /// 获取合同提醒列表
        /// </summary>
        /// <param name="remindDay">提前几天提醒</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.ContractReminder> GetContractRemind(int PageSize, int PageIndex, ref int RecordCount, int remindDay, int companyId)
        {
            IList<EyouSoft.Model.PersonalCenterStructure.ContractReminder> ls = new List<EyouSoft.Model.PersonalCenterStructure.ContractReminder>();
            EyouSoft.Model.PersonalCenterStructure.ContractReminder model = null;
            string tableName = "tbl_ContractInfo";
            string identityColumnName = "Id";
            string fields = " Id,CompanyId,StaffNo,StaffName,BeginDate,EndDate ";
            string query = string.Format("CompanyId={0} and datediff(day,getdate(),EndDate) <= {1} and datediff(day,getdate(),EndDate) >= 0", companyId, remindDay);
            string orderByString = " Id ASC";

            using (IDataReader rdr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, tableName, identityColumnName, fields, query, orderByString))
            {
                while (rdr.Read())
                {
                    model = new EyouSoft.Model.PersonalCenterStructure.ContractReminder();
                    model.StaffNo = rdr.IsDBNull(rdr.GetOrdinal("StaffNo")) ? "" : rdr.GetString(rdr.GetOrdinal("StaffNo"));
                    model.StaffName = rdr.IsDBNull(rdr.GetOrdinal("StaffName")) ? "" : rdr.GetString(rdr.GetOrdinal("StaffName"));
                    model.SignDate = rdr.GetDateTime(rdr.GetOrdinal("BeginDate"));
                    model.ExpireDate = rdr.GetDateTime(rdr.GetOrdinal("EndDate"));
                    ls.Add(model);
                    model = null;
                }
            };
            return ls;
        }

        /// <summary>
        /// 获取合同到期总人数
        /// </summary>
        /// <param name="remindDay">到期天数</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public int GetTotalContractRemind(int remindDay, int companyId)
        {
            StringBuilder SQL_GetTotal = new StringBuilder();
            SQL_GetTotal.AppendFormat("select count(1) from tbl_ContractInfo where datediff(day,getdate(),EndDate) <= {0} and CompanyId = {1}", remindDay, companyId);

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_GetTotal.ToString());

            object obj = DbHelper.GetSingle(cmd, _db);
            if (obj.Equals(null))
                return 0;
            else
                return int.Parse(obj.ToString());
        }
        #endregion
    }
}
