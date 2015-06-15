using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 专线商公司账户信息DAL
    /// </summary>
    /// 创建人：luofx 2011-01-17
    public class CompanyInfo : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.ICompanyInfo
    {
        /// <summary>
        /// Dbml
        /// </summary>
        private EyouSoft.Data.EyouSoftTBL dcDal = null;
        private readonly Database _db = null;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompanyInfo()
        {
            _db = this.SystemStore;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);
        }
        #endregion 构造函数

        #region 实现接口公共方法
        /// <summary>
        /// 获取公司信息实体
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SystemId">系统编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CompanyInfo GetModel(int CompanyId, int SystemId)
        {
            IList<EyouSoft.Model.CompanyStructure.CompanyAccount> CompanyAccountList = null;
            EyouSoft.Model.CompanyStructure.CompanyInfo model = null;
            string StrSql = string.Format("SELECT * FROM tbl_CompanyInfo where id={0} AND SystemId={1}  SELECT * FROM tbl_CompanyAccount WHERE CompanyId={0}", CompanyId, SystemId);
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                if (dr.Read())
                {
                    model = new EyouSoft.Model.CompanyStructure.CompanyInfo()
                    {
                        CompanyAddress = dr.IsDBNull(dr.GetOrdinal("CompanyAddress")) ? "" : dr.GetString(dr.GetOrdinal("CompanyAddress")),
                        CompanyEnglishName = dr.IsDBNull(dr.GetOrdinal("CompanyEnglishName")) ? "" : dr.GetString(dr.GetOrdinal("CompanyEnglishName")),
                        CompanyName = dr.IsDBNull(dr.GetOrdinal("CompanyName")) ? "" : dr.GetString(dr.GetOrdinal("CompanyName")),
                        CompanySiteUrl = dr.IsDBNull(dr.GetOrdinal("CompanySiteUrl")) ? "" : dr.GetString(dr.GetOrdinal("CompanySiteUrl")),
                        CompanyType = dr.IsDBNull(dr.GetOrdinal("CompanyType")) ? "" : dr.GetString(dr.GetOrdinal("CompanyType")),
                        CompanyZip = dr.IsDBNull(dr.GetOrdinal("CompanyZip")) ? "" : dr.GetString(dr.GetOrdinal("CompanyZip")),
                        ContactFax = dr.IsDBNull(dr.GetOrdinal("ContactFax")) ? "" : dr.GetString(dr.GetOrdinal("ContactFax")),
                        ContactMobile = dr.IsDBNull(dr.GetOrdinal("ContactMobile")) ? "" : dr.GetString(dr.GetOrdinal("ContactMobile")),
                        ContactName = dr.IsDBNull(dr.GetOrdinal("ContactName")) ? "" : dr.GetString(dr.GetOrdinal("ContactName")),
                        ContactTel = dr.IsDBNull(dr.GetOrdinal("ContactTel")) ? "" : dr.GetString(dr.GetOrdinal("ContactTel")),
                        License = dr.IsDBNull(dr.GetOrdinal("License")) ? "" : dr.GetString(dr.GetOrdinal("License")),
                        SystemId = dr.GetInt32(dr.GetOrdinal("SystemId")),
                        Id = dr.GetInt32(dr.GetOrdinal("Id")),
                        IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"))
                    };
                    if (dr.NextResult())
                    {
                        CompanyAccountList = new List<EyouSoft.Model.CompanyStructure.CompanyAccount>();
                        while (dr.Read())
                        {
                            EyouSoft.Model.CompanyStructure.CompanyAccount Account = new EyouSoft.Model.CompanyStructure.CompanyAccount()
                            {
                                AccountName = dr.IsDBNull(dr.GetOrdinal("AccountName")) ? "" : dr.GetString(dr.GetOrdinal("AccountName")),
                                BankName = dr.IsDBNull(dr.GetOrdinal("BankName")) ? "" : dr.GetString(dr.GetOrdinal("BankName")),
                                BankNo = dr.IsDBNull(dr.GetOrdinal("BankNo")) ? "" : dr.GetString(dr.GetOrdinal("BankNo")),
                                CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId")),
                            };
                            CompanyAccountList.Add(Account);                            
                            Account = null;
                        }
                        model.CompanyAccountList = CompanyAccountList;
                    }
                }
            }
            return model;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">公司信息实体</param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.CompanyStructure.CompanyInfo model)
        {
            bool IsTrue = false;
            EyouSoft.Data.CompanyInfo DataModel = new EyouSoft.Data.CompanyInfo();
            DataModel.CompanyEnglishName = model.CompanyEnglishName;
            DataModel.CompanyName = model.CompanyName;
            DataModel.CompanySiteUrl = model.CompanySiteUrl;
            DataModel.CompanyType = model.CompanyType;
            DataModel.CompanyZip = model.CompanyZip;
            DataModel.ContactFax = model.ContactFax;
            DataModel.ContactMobile = model.ContactMobile;
            DataModel.ContactName = model.ContactName;
            DataModel.ContactTel = model.ContactTel;
            DataModel.License = model.License;
            DataModel.SystemId = model.SystemId;
            DataModel.IssueTime = System.DateTime.Now;
            if (model.CompanyAccountList != null && model.CompanyAccountList.Count > 0)
            {
                ((List<EyouSoft.Model.CompanyStructure.CompanyAccount>)model.CompanyAccountList).ForEach(item =>
                {
                    EyouSoft.Data.CompanyAccount DataAccountModel = new EyouSoft.Data.CompanyAccount();
                    DataAccountModel.AccountName = item.AccountName;
                    DataAccountModel.BankName = item.BankName;
                    DataAccountModel.BankNo = item.BankNo;
                    DataAccountModel.CompanyId = model.Id;
                    DataModel.CompanyCompanyAccountList.Add(DataAccountModel);
                    DataAccountModel = null;
                });
            }
            dcDal.CompanyInfo.InsertOnSubmit(DataModel);
            dcDal.SubmitChanges();
            if (dcDal.ChangeConflicts.Count == 0)
            {
                IsTrue = true;
            }
            return IsTrue;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">公司信息实体</param>
        /// <returns></returns>
        public bool Update(EyouSoft.Model.CompanyStructure.CompanyInfo model)
        {
            bool IsTrue = false;
            string CompanyAccountXML = CreateCompanyAccountXML(model.CompanyAccountList);
            DbCommand dc = this._db.GetStoredProcCommand("proc_CompanyInfo_Update");
            this._db.AddInParameter(dc, "Id", DbType.Int32, model.Id);
            this._db.AddInParameter(dc, "SystemId", DbType.Int32, model.SystemId);
            this._db.AddInParameter(dc, "CompanyEnglishName", DbType.String, model.CompanyEnglishName);
            this._db.AddInParameter(dc, "CompanyName", DbType.String, model.CompanyName);
            this._db.AddInParameter(dc, "CompanySiteUrl", DbType.String, model.CompanySiteUrl);
            this._db.AddInParameter(dc, "CompanyType", DbType.String, model.CompanyType);
            this._db.AddInParameter(dc, "CompanyZip", DbType.String, model.CompanyZip);
            this._db.AddInParameter(dc, "ContactFax", DbType.AnsiString, model.ContactFax);
            this._db.AddInParameter(dc, "ContactMobile", DbType.AnsiString, model.ContactMobile);
            this._db.AddInParameter(dc, "ContactName", DbType.String, model.ContactName);
            this._db.AddInParameter(dc, "ContactTel", DbType.AnsiString, model.ContactTel);
            this._db.AddInParameter(dc, "License", DbType.String, model.License);
            this._db.AddInParameter(dc, "CompanyAddress", DbType.String, model.CompanyAddress);
            this._db.AddInParameter(dc, "CompanyAccountXML", DbType.String, CompanyAccountXML);
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, this._db);
            object Result = this._db.GetParameterValue(dc, "Result");
            if (!Result.Equals(null))
            {
                IsTrue = int.Parse(Result.ToString()) > 0 ? true : false;
            }
            return IsTrue;
        }
        /// <summary>
        /// 公司账户新增
        /// </summary>
        /// <param name="model">公司账户信息实体</param>
        /// <returns></returns>
        public bool AccountAdd(EyouSoft.Model.CompanyStructure.CompanyAccount model)
        {
            bool IsTrue = false;
            EyouSoft.Data.CompanyAccount AcountModel = new EyouSoft.Data.CompanyAccount()
            {
                AccountName = model.AccountName,
                BankName = model.BankName,
                BankNo = model.BankNo,
                CompanyId = model.CompanyId,
            };
            dcDal.CompanyAccount.InsertOnSubmit(AcountModel);
            dcDal.SubmitChanges();
            if (dcDal.ChangeConflicts.Count == 0)
            {
                IsTrue = true;
            }
            AcountModel = null;
            return IsTrue;
        }
        /// <summary>
        /// 公司账户修改
        /// </summary>
        /// <param name="model">公司账户信息实体</param>
        /// <returns></returns>
        public bool AccountUpdate(EyouSoft.Model.CompanyStructure.CompanyAccount model)
        {
            bool IsTrue = false;
            EyouSoft.Data.CompanyAccount DataModel = dcDal.CompanyAccount.FirstOrDefault(item =>
               item.Id == model.Id && item.CompanyId == model.CompanyId
            );
            if (DataModel != null)
            {
                DataModel.AccountName = model.AccountName;
                DataModel.BankName = model.BankName;
                DataModel.BankNo = model.BankNo;
                dcDal.SubmitChanges();
                IsTrue = true;
            }
            DataModel = null;
            return IsTrue;
        }
        #endregion 实现接口公共方法

        #region 私有方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AccountList">公司账户信息实体</param>
        /// <returns></returns>
        private string CreateCompanyAccountXML(IList<EyouSoft.Model.CompanyStructure.CompanyAccount> AccountList)
        {
            if (AccountList == null) return "";
            StringBuilder CompanyAccountXML = new StringBuilder();
            CompanyAccountXML.Append("<ROOT>");//CompanyAccoun
            foreach (EyouSoft.Model.CompanyStructure.CompanyAccount item in AccountList)
            {
                CompanyAccountXML.AppendFormat("<CompanyAccoun AccountName=\"{0}\"", item.AccountName);
                CompanyAccountXML.AppendFormat(" BankName=\"{0}\"", item.BankName);
                CompanyAccountXML.AppendFormat(" BankNo=\"{0}\"", item.BankNo);
                CompanyAccountXML.AppendFormat(" CompanyId=\"{0}\" />", item.CompanyId);
            }
            CompanyAccountXML.Append("</ROOT>");
            return CompanyAccountXML.ToString();
        }
        /// <summary>
        /// 公司账户删除
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        private bool AccountDelete(int CompanyId)
        {
            bool IsTrue = false;
            IEnumerable<EyouSoft.Data.CompanyAccount> AccountLists = from item in dcDal.CompanyAccount
                                                                     where item.CompanyId == CompanyId
                                                                     select item;
            if (AccountLists != null)
            {
                dcDal.CompanyAccount.DeleteAllOnSubmit<EyouSoft.Data.CompanyAccount>(AccountLists);
                dcDal.SubmitChanges();
                if (dcDal.ChangeConflicts.Count == 0)
                {
                    IsTrue = true;
                }
                AccountLists = null;
            }
            return IsTrue;
        }
        #endregion 私有方法
    }
}
