using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using EyouSoft.Toolkit.DAL;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using EyouSoft.Toolkit;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 供应商管理-地接DAL
    /// 创建人：xuqh 2011-01-19
    /// </summary>
    public class CompanySupplier : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.ICompanySupplier
    {
        #region static constants
        //static constants
        private const string SQL_SELECT_GetTimesSummaryDiJie = "SELECT SUM([PeopleNumber] - [LeaguePepoleNum]) AS ShiShou FROM [tbl_TourOrder] WHERE [OrderState] NOT IN(3,4) AND [IsDelete]='0' AND [TourId] IN(SELECT [TourId] FROM [tbl_PlanLocalAgency] WHERE [TravelAgencyID] = @GYSID UNION ALL SELECT [TourId] FROM [tbl_PlanSingle] WHERE [SupplierId]=@GYSID);SELECT SUM([Commission]) AS CommAmount,SUM([TotalAmount]) AS TotalAmount,SUM([PayAmount]) AS PayAmount FROM [tbl_PlanLocalAgency] WHERE [TravelAgencyID]=@GYSID;SELECT SUM([TotalAmount]) AS TotalAmount,SUM([PaidAmount]) AS PayAmount FROM [tbl_PlanSingle] WHERE [SupplierId]=@GYSID AND [ServiceType]=@ServiceType AND EXISTS(SELECT 1 FROM [tbl_Tour] AS A WHERE A.[TourId]=[tbl_PlanSingle].[TourId] AND A.[IsDelete]='0')";
        private const string SQL_SELECT_GetTimesSummaryJiPiao = "SELECT SUM([PeopleCount]) AS PeopleNumber,SUM([TotalMoney]) AS TicketAmount,SUM([AgencyPrice]) AS AgencyAmount FROM [tbl_PlanTicketKind] WHERE [TicketId] IN (SELECT [ID] FROM [tbl_PlanTicketOut] WHERE [TicketOfficeId]=@GYSID);SELECT SUM([TotalAmount]) AS TotalAmount,SUM([PayAmount]) AS PayAmount FROM [tbl_PlanTicketOut] WHERE [TicketOfficeId]=@GYSID;SELECT SUM([TotalAmount]) AS TotalAmount,SUM([PaidAmount]) AS PayAmount FROM [tbl_PlanSingle] WHERE [SupplierId]=@GYSID AND [ServiceType]=@ServiceType AND EXISTS(SELECT 1 FROM [tbl_Tour] AS A WHERE A.[TourId]=[tbl_PlanSingle].[TourId] AND A.[IsDelete]='0')";
        #endregion

        private readonly EyouSoft.Data.EyouSoftTBL dcDal = null;
        private readonly Database _db = null;

        #region 构造函数
        public CompanySupplier()
        {
            this._db = base.SystemStore;
            this.dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);
        }
        #endregion

        #region ICompanySupplier 成员

        /// <summary>
        /// 删除供应商联系人
        /// </summary>
        /// <param name="supplierId">供应商联系人ID</param>
        /// <returns></returns>
        public bool DeleteSupplierContact(int supplierId)
        {

            bool IsTrue = false;
            IEnumerable<EyouSoft.Data.SupplierContact> entity = dcDal.SupplierContact.Where(d => d.Id == supplierId);
            if (entity != null)
            {
                dcDal.SupplierContact.DeleteAllOnSubmit<EyouSoft.Data.SupplierContact>(entity);
                dcDal.SubmitChanges();
                if (dcDal.ChangeConflicts.Count == 0)
                {
                    IsTrue = true;
                }
            }
            return IsTrue;
        }

        /// <summary>
        /// 添加供应商信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddSupplierInfo(EyouSoft.Model.CompanyStructure.CompanySupplier model)
        {
            #region *

            //bool IsTrue = false;

            //EyouSoft.Data.CompanySupplier DataModel = new EyouSoft.Data.CompanySupplier();
            //InitDataModel(DataModel, model);

            ////添加供应商联系人信息
            //if (model.SupplierContact != null && model.SupplierContact.Count > 0)
            //{
            //    ((List<EyouSoft.Model.CompanyStructure.SupplierContact>)model.SupplierContact).ForEach(item =>
            //        {
            //            EyouSoft.Data.SupplierContact supplierContact = new EyouSoft.Data.SupplierContact();
            //            supplierContact.CompanyId = model.CompanyId;
            //            supplierContact.SupplierType = (byte)item.SupplierType;
            //            supplierContact.ContactName = item.ContactName;
            //            supplierContact.JobTitle = item.JobTitle;
            //            supplierContact.ContactFax = item.ContactFax;
            //            supplierContact.ContactTel = item.ContactTel;
            //            supplierContact.ContactMobile = item.ContactMobile;
            //            supplierContact.Qq = item.QQ;
            //            supplierContact.Email = item.Email;
            //            DataModel.SupplierSupplierContactList.Add(supplierContact);
            //            supplierContact = null;
            //        }
            //    );
            //}

            //dcDal.CompanySupplier.InsertOnSubmit(DataModel);
            //dcDal.SubmitChanges();

            //if (dcDal.ChangeConflicts.Count == 0)
            //{
            //    IsTrue = true;
            //}
            //DataModel = null;
            //return IsTrue;

            #endregion

            if (model == null)
                return false;

            DbCommand dc = this._db.GetStoredProcCommand("proc_CompanySupplier_Add");
            this._db.AddInParameter(dc, "ProvinceId", DbType.Int32, model.ProvinceId);
            this._db.AddInParameter(dc, "ProvinceName", DbType.String, model.ProvinceName);
            this._db.AddInParameter(dc, "CityId", DbType.Int32, model.CityId);
            this._db.AddInParameter(dc, "CityName", DbType.String, model.CityName);
            this._db.AddInParameter(dc, "UnitName", DbType.String, model.UnitName);
            this._db.AddInParameter(dc, "SupplierType", DbType.Byte, (int)model.SupplierType);
            this._db.AddInParameter(dc, "UnitAddress", DbType.String, model.UnitAddress);
            this._db.AddInParameter(dc, "Commission", DbType.Decimal, model.Commission);
            this._db.AddInParameter(dc, "AgreementFile", DbType.String, model.AgreementFile);
            this._db.AddInParameter(dc, "TradeNum", DbType.Int32, model.TradeNum);
            this._db.AddInParameter(dc, "UnitPolicy", DbType.String, model.UnitPolicy);
            this._db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(dc, "SupplierContactXML", DbType.String, this.GetSupplierContactXMLByModel(model.SupplierContact));
            this._db.AddOutParameter(dc, "ErrorValue", DbType.Int32, 4);
            this._db.AddInParameter(dc, "LicenseKey", DbType.String, model.LicenseKey);

            DbHelper.RunProcedure(dc, this._db);
            object obj = this._db.GetParameterValue(dc, "ErrorValue");
            if (obj.Equals(null))
                return false;
            else
                return int.Parse(obj.ToString()) == 1 ? true : false;
        }

        /// <summary>
        /// 修改供应商信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateSupplerInfo(EyouSoft.Model.CompanyStructure.CompanySupplier model)
        {
            #region *

            //EyouSoft.Data.CompanySupplier supplier = dcDal.CompanySupplier.FirstOrDefault(item => item.Id == model.Id && item.CompanyId == model.CompanyId);

            //if (supplier != null)
            //{
            //    InitDataModel(supplier, model);

            //    //if(supplier.SupplierSupplierContactList != null && )

            //    //修改联系人信息 如果不是导入的数据
            //    if (supplier.SupplierSupplierContactList != null)
            //    {
            //        if (this.SupplierDelete(model.Id))
            //        {
            //            ((List<EyouSoft.Model.CompanyStructure.SupplierContact>)model.SupplierContact).ForEach(item =>
            //                {
            //                    #region 赋值
            //                    EyouSoft.Data.SupplierContact dataContact = new EyouSoft.Data.SupplierContact();
            //                    //contact.Id = item.Id;
            //                    dataContact.CompanyId = model.CompanyId;
            //                    dataContact.SupplierId = model.Id;
            //                    dataContact.SupplierType = (byte)item.SupplierType;
            //                    dataContact.ContactName = item.ContactName;
            //                    dataContact.JobTitle = item.JobTitle;
            //                    dataContact.ContactFax = item.ContactFax;
            //                    dataContact.ContactTel = item.ContactTel;
            //                    dataContact.ContactMobile = item.ContactMobile;
            //                    dataContact.Qq = item.QQ;
            //                    dataContact.Email = item.Email;
            //                    supplier.SupplierSupplierContactList.Add(dataContact);
            //                    dataContact = null;
            //                    #endregion
            //                }
            //            );
            //        }
            //    }
            //    else
            //    {
            //        ((List<EyouSoft.Model.CompanyStructure.SupplierContact>)model.SupplierContact).ForEach(item =>
            //        {
            //            #region 赋值
            //            EyouSoft.Data.SupplierContact dataContact = new EyouSoft.Data.SupplierContact();
            //            //contact.Id = item.Id;
            //            dataContact.CompanyId = model.CompanyId;
            //            dataContact.SupplierId = model.Id;
            //            dataContact.SupplierType = (byte)item.SupplierType;
            //            dataContact.ContactName = item.ContactName;
            //            dataContact.JobTitle = item.JobTitle;
            //            dataContact.ContactFax = item.ContactFax;
            //            dataContact.ContactTel = item.ContactTel;
            //            dataContact.ContactMobile = item.ContactMobile;
            //            dataContact.Qq = item.QQ;
            //            dataContact.Email = item.Email;
            //            supplier.SupplierSupplierContactList.Add(dataContact);
            //            dataContact = null;
            //            #endregion
            //        });
            //    }

            //    dcDal.SubmitChanges();
            //}

            //return dcDal.ChangeConflicts.Count == 0 ? true : false;

            #endregion

            if (model == null || model.Id <= 0)
                return false;

            DbCommand dc = this._db.GetStoredProcCommand("proc_CompanySupplier_Update");
            this._db.AddInParameter(dc, "Id", DbType.Int32, model.Id);
            this._db.AddInParameter(dc, "ProvinceId", DbType.Int32, model.ProvinceId);
            this._db.AddInParameter(dc, "ProvinceName", DbType.String, model.ProvinceName);
            this._db.AddInParameter(dc, "CityId", DbType.Int32, model.CityId);
            this._db.AddInParameter(dc, "CityName", DbType.String, model.CityName);
            this._db.AddInParameter(dc, "UnitName", DbType.String, model.UnitName);
            this._db.AddInParameter(dc, "SupplierType", DbType.Byte, (int)model.SupplierType);
            this._db.AddInParameter(dc, "UnitAddress", DbType.String, model.UnitAddress);
            this._db.AddInParameter(dc, "Commission", DbType.Decimal, model.Commission);
            this._db.AddInParameter(dc, "AgreementFile", DbType.String, model.AgreementFile);
            this._db.AddInParameter(dc, "TradeNum", DbType.Int32, model.TradeNum);
            this._db.AddInParameter(dc, "UnitPolicy", DbType.String, model.UnitPolicy);
            this._db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(dc, "IsDelete", DbType.AnsiStringFixedLength, model.IsDelete ? "1" : "0");
            this._db.AddInParameter(dc, "SupplierContactXML", DbType.String, this.GetSupplierContactXMLByModel(model.SupplierContact));
            this._db.AddOutParameter(dc, "ErrorValue", DbType.Int32, 4);
            this._db.AddInParameter(dc, "LicenseKey", DbType.String, model.LicenseKey);

            DbHelper.RunProcedure(dc, this._db);
            object obj = this._db.GetParameterValue(dc, "ErrorValue");
            if (obj.Equals(null))
                return false;
            else
                return int.Parse(obj.ToString()) == 1 ? true : false;
        }

        /// <summary>
        /// 删除供应商信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteSupplierInfo(params int[] id)
        {
            bool IsTrue = false;

            var companySupplier = dcDal.CompanySupplier.Where(item => id.Contains(item.Id));

            foreach (var item in companySupplier)
            {
                item.IsDelete = "1";
            }

            dcDal.SubmitChanges();
            if (dcDal.ChangeConflicts.Count == 0)
            {
                IsTrue = true;
            }
            return IsTrue;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CompanySupplier GetModel(int Id, int companyId)
        {
            #region *

            //EyouSoft.Data.CompanySupplier supplierModel = dcDal.CompanySupplier.FirstOrDefault(item => item.Id == Id && item.CompanyId == companyId);
            ////EyouSoft.Data.SupplierContact supplierContact = supplierModel.SupplierSupplierContactList.First();
            //IList<EyouSoft.Model.CompanyStructure.SupplierContact> ls = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();

            //if (supplierModel != null)
            //{
            //    foreach (var item in supplierModel.SupplierSupplierContactList)
            //    {
            //        EyouSoft.Model.CompanyStructure.SupplierContact contact = new EyouSoft.Model.CompanyStructure.SupplierContact();
            //        contact.Id = item.Id;
            //        contact.CompanyId = item.CompanyId;
            //        contact.SupplierId = item.SupplierId;
            //        contact.SupplierType = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)item.SupplierType;
            //        contact.ContactName = item.ContactName;
            //        contact.JobTitle = item.JobTitle;
            //        contact.ContactFax = item.ContactFax;
            //        contact.ContactTel = item.ContactTel;
            //        contact.ContactMobile = item.ContactMobile;
            //        contact.QQ = item.Qq;
            //        contact.Email = item.Email;
            //        contact.UserAccount = new EyouSoft.Model.CompanyStructure.UserAccount();
            //        contact.UserAccount.ID = item.UserId;
            //        ls.Add(contact);
            //    }
            //}

            //if (supplierModel == null)
            //    return null;

            //return new EyouSoft.Model.CompanyStructure.CompanySupplier()
            //{
            //    #region 实体赋值
            //    Id = supplierModel.Id,
            //    ProvinceId = supplierModel.ProvinceId,
            //    ProvinceName = supplierModel.ProvinceName,
            //    CityId = supplierModel.CityId,
            //    CityName = supplierModel.CityName,
            //    UnitName = supplierModel.UnitName,
            //    SupplierType = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)supplierModel.SupplierType,
            //    UnitAddress = supplierModel.UnitAddress,
            //    Commission = supplierModel.Commission,
            //    AgreementFile = supplierModel.AgreementFile,
            //    TradeNum = supplierModel.TradeNum,
            //    UnitPolicy = supplierModel.UnitPolicy,
            //    Remark = supplierModel.Remark,
            //    CompanyId = supplierModel.CompanyId,
            //    OperatorId = supplierModel.OperatorId,
            //    IssueTime = DateTime.Now,
            //    IsDelete = Convert.ToBoolean(Convert.ToInt32(supplierModel.IsDelete)) ? true : false,
            //    SupplierContact = ls
            //    #endregion
            //};

            #endregion

            if (Id <= 0 || companyId <= 0)
                return null;

            EyouSoft.Model.CompanyStructure.CompanySupplier model = new EyouSoft.Model.CompanyStructure.CompanySupplier();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select [Id],[ProvinceId],[ProvinceName],[CityId],[CityName],[UnitName],[SupplierType],[UnitAddress],[Commission],[AgreementFile],[TradeNum],[UnitPolicy],[Remark],[CompanyId],[OperatorId],[IssueTime],[IsDelete],[LicenseKey] from tbl_CompanySupplier where Id = @Id and CompanyId = @CompanyId;");
            strSql.Append(" select [Id],[CompanyId],[SupplierId],[SupplierType],[ContactName],[JobTitle],[ContactFax],[ContactTel],[ContactMobile],[QQ],[Email],(select ID,UserName,[Password],MD5Password,CompanyId,TourCompanyId from tbl_CompanyUser where tbl_CompanyUser.ID = tbl_SupplierContact.UserId for xml raw,root('root')) as UserInfo from tbl_SupplierContact where SupplierId = @Id and CompanyId = @CompanyId;");
            strSql.Append(" select [Id],[SupplierId],[PicName],[PicPath] from tbl_SupplierAccessory where SupplierId = @Id;");

            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());
            this._db.AddInParameter(dc, "Id", DbType.Int32, Id);
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, companyId);

            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                if (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                        model.Id = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        model.ProvinceId = dr.GetInt32(1);
                    if (!dr.IsDBNull(2))
                        model.ProvinceName = dr.GetString(2);
                    if (!dr.IsDBNull(3))
                        model.CityId = dr.GetInt32(3);
                    if (!dr.IsDBNull(4))
                        model.CityName = dr.GetString(4);
                    if (!dr.IsDBNull(5))
                        model.UnitName = dr.GetString(5);
                    if (!dr.IsDBNull(6))
                        model.SupplierType = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)dr.GetByte(6);
                    if (!dr.IsDBNull(7))
                        model.UnitAddress = dr.GetString(7);
                    if (!dr.IsDBNull(8))
                        model.Commission = dr.GetDecimal(8);
                    if (!dr.IsDBNull(9))
                        model.AgreementFile = dr.GetString(9);
                    if (!dr.IsDBNull(10))
                        model.TradeNum = dr.GetInt32(10);
                    if (!dr.IsDBNull(11))
                        model.UnitPolicy = dr.GetString(11);
                    if (!dr.IsDBNull(12))
                        model.Remark = dr.GetString(12);
                    if (!dr.IsDBNull(13))
                        model.CompanyId = dr.GetInt32(13);
                    if (!dr.IsDBNull(14))
                        model.OperatorId = dr.GetInt32(14);
                    if (!dr.IsDBNull(15))
                        model.IssueTime = dr.GetDateTime(15);
                    if (!dr.IsDBNull(16) && (dr.GetString(16) == "1" || dr.GetString(16).ToLower() == "true"))
                        model.IsDelete = true;
                    if (!dr.IsDBNull(17))
                        model.LicenseKey = dr[17].ToString();
                }

                dr.NextResult();
                model.SupplierContact = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
                EyouSoft.Model.CompanyStructure.SupplierContact SCModel = null;
                while (dr.Read())
                {
                    SCModel = new EyouSoft.Model.CompanyStructure.SupplierContact();
                    //[Id],[CompanyId],[SupplierId],[SupplierType],[ContactName],[JobTitle],[ContactFax],[ContactTel],[ContactMobile],[QQ],[Email],[UserId]
                    if (!dr.IsDBNull(0))
                        SCModel.Id = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        SCModel.CompanyId = dr.GetInt32(1);
                    if (!dr.IsDBNull(2))
                        SCModel.SupplierId = dr.GetInt32(2);
                    if (!dr.IsDBNull(3))
                        SCModel.SupplierType = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)dr.GetByte(3);
                    if (!dr.IsDBNull(4))
                        SCModel.ContactName = dr.GetString(4);
                    if (!dr.IsDBNull(5))
                        SCModel.JobTitle = dr.GetString(5);
                    if (!dr.IsDBNull(6))
                        SCModel.ContactFax = dr.GetString(6);
                    if (!dr.IsDBNull(7))
                        SCModel.ContactTel = dr.GetString(7);
                    if (!dr.IsDBNull(8))
                        SCModel.ContactMobile = dr.GetString(8);
                    if (!dr.IsDBNull(9))
                        SCModel.QQ = dr.GetString(9);
                    if (!dr.IsDBNull(10))
                        SCModel.Email = dr.GetString(10);
                    if (!dr.IsDBNull(11))
                    {
                        XElement xRoot = XElement.Parse(dr.GetString(11));
                        if (xRoot != null)
                        {
                            var xRows = Utils.GetXElements(xRoot, "row");
                            if (xRows != null && xRows.Count() > 0)
                            {
                                SCModel.UserAccount = new EyouSoft.Model.CompanyStructure.UserAccount();
                                SCModel.UserAccount.PassWordInfo = new EyouSoft.Model.CompanyStructure.PassWord();
                                foreach (var t in xRows)
                                {
                                    SCModel.UserAccount.ID = Utils.GetInt(Utils.GetXAttributeValue(t, "ID"));
                                    SCModel.UserAccount.CompanyId = Utils.GetInt(Utils.GetXAttributeValue(t, "CompanyId"));
                                    SCModel.UserAccount.TourCompanyId = Utils.GetInt(Utils.GetXAttributeValue(t, "TourCompanyId"));
                                    SCModel.UserAccount.UserName = Utils.GetXAttributeValue(t, "UserName");
                                    SCModel.UserAccount.PassWordInfo.NoEncryptPassword = Utils.GetXAttributeValue(t, "Password");
                                    break;
                                }
                            }
                        }
                    }

                    model.SupplierContact.Add(SCModel);
                }

                dr.NextResult();
                model.SupplierPic = new List<EyouSoft.Model.SupplierStructure.SupplierPic>();
                EyouSoft.Model.SupplierStructure.SupplierPic SPModel = null;
                while (dr.Read())
                {
                    SPModel = new EyouSoft.Model.SupplierStructure.SupplierPic();
                    //[Id],[SupplierId],[PicName],[PicPath]
                    if (!dr.IsDBNull(0))
                        SPModel.Id = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        SPModel.SupplierId = dr.GetInt32(1);
                    if (!dr.IsDBNull(2))
                        SPModel.PicName = dr.GetString(2);
                    if (!dr.IsDBNull(3))
                        SPModel.PicPath = dr.GetString(3);

                    model.SupplierPic.Add(SPModel);
                }
            }

            return model;
        }

        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="RecordCount">总数</param>
        /// <param name="Id">供应商编号</param>
        /// <param name="supplierType">单位类型：地接0-票务1</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanySupplier> GetList(int pageSize, int pageIndex, ref int RecordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType supplierType, int ProvinceId, int CityId, string UnitName, int companyId)
        {
            IList<EyouSoft.Model.CompanyStructure.CompanySupplier> ls = new List<EyouSoft.Model.CompanyStructure.CompanySupplier>();
            string tableName = "tbl_CompanySupplier";
            StringBuilder fields = new StringBuilder();
            fields.Append(" Id,UnitAddress,UnitName,ProvinceName,CityName,");
            fields.Append(" (select top 1 ContactName,ContactTel,ContactFax from tbl_SupplierContact a where a.SupplierId = tbl_CompanySupplier.[Id] for xml raw,root('root')) as ContactXML,");
            fields.Append(" TradeNum,UnitPolicy ");
            fields.Append(",LicenseKey");
            string primaryKey = "Id";
            string orderbyStr = "IssueTime desc";
            StringBuilder strWhere = new StringBuilder(" IsDelete='0' ");
            strWhere.AppendFormat(" and SupplierType={0}", Convert.ToByte((int)supplierType));
            strWhere.AppendFormat(" and CompanyId = {0}", companyId);

            if (ProvinceId > 0)
                strWhere.AppendFormat(" and ProvinceId={0}", ProvinceId);
            if (CityId > 0)
                strWhere.AppendFormat(" and CityId={0}", CityId);
            if (!string.IsNullOrEmpty(UnitName))
                strWhere.AppendFormat(" and UnitName like '%{0}%'", UnitName);

            using (IDataReader dr = DbHelper.ExecuteReader(base.SystemStore, pageSize, pageIndex, ref RecordCount, tableName, primaryKey, fields.ToString(), strWhere.ToString(), orderbyStr))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.CompanyStructure.CompanySupplier model = new EyouSoft.Model.CompanyStructure.CompanySupplier();
                    model.Id = dr.GetInt32(dr.GetOrdinal("Id"));
                    model.UnitAddress = dr.IsDBNull(dr.GetOrdinal("UnitAddress")) ? "" : dr.GetOrdinal("UnitAddress").ToString();
                    model.UnitName = dr.GetString(dr.GetOrdinal("UnitName"));
                    model.ProvinceName = dr.IsDBNull(dr.GetOrdinal("ProvinceName")) ? "" : dr.GetString(dr.GetOrdinal("ProvinceName"));
                    model.CityName = dr.IsDBNull(dr.GetOrdinal("CityName")) ? "" : dr.GetString(dr.GetOrdinal("CityName"));
                    model.TradeNum = dr.GetInt32(dr.GetOrdinal("TradeNum"));
                    model.UnitPolicy = dr.IsDBNull(dr.GetOrdinal("UnitPolicy")) ? "" : dr[dr.GetOrdinal("UnitPolicy")].ToString();
                    model.SupplierContact = GetContactList(dr.IsDBNull(dr.GetOrdinal("ContactXML")) ? "" : dr.GetString(dr.GetOrdinal("ContactXML")));
                    if (!dr.IsDBNull(dr.GetOrdinal("LicenseKey")))
                    {
                        model.LicenseKey = dr["LicenseKey"].ToString();
                    }
                    ls.Add(model);
                    model = null;
                }
            }

            return ls;
        }

        /// <summary>
        /// 分页获取供应商-其它列表
        /// 新加 2011-3-9
        /// </summary>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="RecordCount">总数</param>
        /// <param name="Id">供应商编号</param>
        /// <param name="supplierType">单位类型：地接0-票务1</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SupplierStructure.SupplierOther> GetOtherList(int pageSize, int pageIndex, ref int RecordCount, EyouSoft.Model.EnumType.CompanyStructure.SupplierType supplierType, string UnitName, int companyId)
        {
            IList<EyouSoft.Model.SupplierStructure.SupplierOther> ls = new List<EyouSoft.Model.SupplierStructure.SupplierOther>();
            string tableName = "tbl_CompanySupplier";
            StringBuilder fields = new StringBuilder();
            fields.Append(" Id,UnitAddress,UnitName,ProvinceName,CityName,");
            fields.Append(" (select top 1 ContactName,ContactTel,ContactFax from tbl_SupplierContact a where a.SupplierId = tbl_CompanySupplier.[Id] for xml raw,root('root')) as ContactXML,");
            fields.Append(" TradeNum,UnitPolicy ");
            string primaryKey = "Id";
            string orderbyStr = "IssueTime desc";
            StringBuilder strWhere = new StringBuilder(" IsDelete='0' ");
            strWhere.AppendFormat(" and SupplierType={0}", Convert.ToByte((int)supplierType));
            strWhere.AppendFormat(" and CompanyId = {0}", companyId);

            if (!string.IsNullOrEmpty(UnitName))
                strWhere.AppendFormat(" and UnitName like '%{0}%'", UnitName);

            using (IDataReader dr = DbHelper.ExecuteReader(base.SystemStore, pageSize, pageIndex, ref RecordCount, tableName, primaryKey, fields.ToString(), strWhere.ToString(), orderbyStr))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.SupplierStructure.SupplierOther model = new EyouSoft.Model.SupplierStructure.SupplierOther();
                    model.Id = dr.GetInt32(dr.GetOrdinal("Id"));
                    model.UnitAddress = dr.IsDBNull(dr.GetOrdinal("UnitAddress")) ? "" : dr.GetOrdinal("UnitAddress").ToString();
                    model.UnitName = dr.GetString(dr.GetOrdinal("UnitName"));
                    model.ProvinceName = dr.IsDBNull(dr.GetOrdinal("ProvinceName")) ? "" : dr.GetString(dr.GetOrdinal("ProvinceName"));
                    model.CityName = dr.IsDBNull(dr.GetOrdinal("CityName")) ? "" : dr.GetString(dr.GetOrdinal("CityName"));
                    model.TradeNum = dr.GetInt32(dr.GetOrdinal("TradeNum"));
                    model.SupplierContact = GetContactList(dr.IsDBNull(dr.GetOrdinal("ContactXML")) ? "" : dr.GetString(dr.GetOrdinal("ContactXML")));
                    ls.Add(model);
                    model = null;
                }
            }

            return ls;
        }

        /// <summary>
        /// 数据批量插入到数据库
        /// </summary>
        /// <param name="ls">数据集(来自文件)</param>
        /// <returns></returns>
        public bool ImportExcelData(List<EyouSoft.Model.CompanyStructure.CompanySupplier> ls)
        {
            StringBuilder sqlStr = new StringBuilder();

            foreach (EyouSoft.Model.CompanyStructure.CompanySupplier item in ls)
            {
                int pId = GetProvinceId(item.ProvinceName);
                int cId = GetCityId(item.CityName);

                if (pId != 0 && cId != 0)
                {
                    sqlStr.AppendFormat("insert into tbl_CompanySupplier(ProvinceId,ProvinceName,CityId,CityName,UnitName,"
                                        + "SupplierType,UnitAddress,Commission,AgreementFile,TradeNum,UnitPolicy,Remark,"
                                        + "CompanyId,OperatorId,IssueTime,IsDelete) values({0},'{1}',{2},'{3}','{4}',"
                                        + "{5},'{6}',{7},'{8}',{9},'{10}',"
                                        + "'{11}',{12},{13},'{14}',{15});", pId,
                                        item.ProvinceName, cId, item.CityName, item.UnitName, (byte)item.SupplierType,
                                        item.UnitAddress, item.Commission, item.AgreementFile, item.TradeNum, item.UnitPolicy,
                                        item.Remark, item.CompanyId, item.OperatorId, item.IssueTime, item.IsDelete ? "1" : "0");
                }
            }

            if (string.IsNullOrEmpty(sqlStr.ToString()))
                return true;

            DbCommand cmd = this._db.GetSqlStringCommand(sqlStr.ToString());
            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSqlTrans(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 批量导入供应商-其它信息
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public bool ImportOtherData(List<EyouSoft.Model.SupplierStructure.SupplierOther> ls)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("begin ");
            sql.Append("declare @supplierId int set @supplierId = 0 ");

            foreach (EyouSoft.Model.SupplierStructure.SupplierOther item in ls)
            {
                sql.AppendFormat(" insert into tbl_CompanySupplier(ProvinceId,ProvinceName,CityId,CityName,UnitName,"
                                + " SupplierType,UnitAddress,AgreementFile,TradeNum,"
                                + " Remark,CompanyId,OperatorId,IssueTime,IsDelete)"
                                + " values({0},'{1}',{2},'{3}','{4}',"
                                + " {5},'{6}','{7}',{8},'{9}',"
                                + " {10},{11},'{12}',{13});",
                                item.ProvinceId, item.ProvinceName, item.CityId, item.CityName, item.UnitName,
                                (byte)item.SupplierType, item.UnitAddress, item.AgreementFile, item.TradeNum, item.Remark,
                                item.CompanyId, item.OperatorId, item.IssueTime, item.IsDelete ? "1" : "0");
                sql.Append(" select @supplierId = @@identity");

                if (item.SupplierContact != null)
                {
                    foreach (EyouSoft.Model.CompanyStructure.SupplierContact contact in item.SupplierContact)
                    {
                        sql.AppendFormat(" insert into tbl_SupplierContact(CompanyId,SupplierId,SupplierType,ContactName "
                                        + ",JobTitle,ContactFax,ContactTel,ContactMobile,QQ,Email)"
                                        + " values({0},@supplierId,{1},'{2}'"
                                        + ",'{3}','{4}','{5}','{6}','{7}','{8}' );",
                                        contact.CompanyId, (byte)contact.SupplierType, contact.ContactName, contact.JobTitle, contact.ContactFax,
                                        contact.ContactTel, contact.ContactMobile, contact.QQ, contact.Email);
                    }
                }

                sql.Append(" set @supplierId = 0");
            }
            sql.Append(" end");

            if (string.IsNullOrEmpty(sql.ToString()))
                return true;

            DbCommand cmd = this._db.GetSqlStringCommand(sql.ToString());
            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSqlTrans(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 分页获取付款提醒
        /// </summary>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="searchInfo">查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.PayRemind> GetPayRemind(int pageSize, int pageIndex, ref int recordCount, int CompanyId, EyouSoft.Model.PersonalCenterStructure.FuKuanTiXingChaXun searchInfo)
        {
            if (CompanyId <= 0)
                return null;

            IList<EyouSoft.Model.PersonalCenterStructure.PayRemind> list = new List<EyouSoft.Model.PersonalCenterStructure.PayRemind>();
            StringBuilder strFiles = new StringBuilder();
            strFiles.AppendFormat(" Id,UnitName,ContactInfo,Amount,OperatorInfo,SupplierType,PlanerXmlDJ,PlanerXmlJP,PlanerXmlDX ");

            bool hasTourSearch = false;
            if (searchInfo != null)
            {
                if (searchInfo.LEDate.HasValue
                    || searchInfo.LSDate.HasValue
                    || (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    || (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0))
                {
                    hasTourSearch = true;
                    StringBuilder tourSearchWhere = new StringBuilder();

                    tourSearchWhere.Append(" AND EXISTS(SELECT 1 FROM tbl_Tour AS A WHERE 1=1 AND IsDelete='0' AND A.TourId={KEY}");

                    if (searchInfo.LSDate.HasValue)
                    {
                        tourSearchWhere.AppendFormat(" AND A.LeaveDate>='{0}' ", searchInfo.LSDate.Value);
                    }
                    if (searchInfo.LEDate.HasValue)
                    {
                        tourSearchWhere.AppendFormat(" AND A.LeaveDate<='{0}' ", searchInfo.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0)
                    {
                        tourSearchWhere.AppendFormat(" AND A.OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorIds));
                    }
                    if (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    {
                        tourSearchWhere.AppendFormat(" AND A.OperatorId IN(SELECT E.Id FROM tbl_CompanyUser AS E WHERE E.DepartId IN({0})) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorDepartIds));
                    }
                    tourSearchWhere.Append(")");


                    strFiles.Append(",(");

                    strFiles.AppendFormat("SELECT SUM(TotalAmount-HasCheckAmount) FROM tbl_StatAllOut WHERE IsDelete='0' AND SupplierId=View_PayRemind_GetList.Id AND TourId IN(SELECT B.TourId FROM tbl_PlanLocalAgency AS B WHERE B.TravelAgencyID=View_PayRemind_GetList.Id {0} UNION SELECT C.TourId FROM tbl_PlanTicketOut AS C WHERE C.State=3 AND C.TicketOfficeId=View_PayRemind_GetList.Id {1} UNION SELECT D.TourId FROM tbl_PlanSingle AS D WHERE D.SupplierId=View_PayRemind_GetList.Id {2}) "
                        , tourSearchWhere.ToString().Replace("{KEY}", "B.TourId ")
                        , tourSearchWhere.ToString().Replace("{KEY}", "C.TourId ")
                        , tourSearchWhere.ToString().Replace("{KEY}", "D.TourId "));

                    strFiles.Append(") AS TourSearchAmount");

                }
                else
                {
                    strFiles.Append(",0 AS TourSearchAmount");
                }
            }
            else
            {
                strFiles.Append(",0 AS TourSearchAmount");
            }

            string strOrder = @" Amount desc ";
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" Amount > 0 and CompanyId = {0} ", CompanyId);

            #region SQL WHERE
            if (searchInfo != null)
            {
                if (searchInfo.LEDate.HasValue
                    || searchInfo.LSDate.HasValue
                    || (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    || (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0))
                {
                    strWhere.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_Tour AS A WHERE IsDelete='0' AND TourId IN(SELECT B.TourId FROM tbl_PlanLocalAgency AS B WHERE B.TravelAgencyID=View_PayRemind_GetList.Id UNION SELECT C.TourId FROM tbl_PlanTicketOut AS C WHERE C.State=3 AND C.TicketOfficeId=View_PayRemind_GetList.Id UNION SELECT D.TourId FROM tbl_PlanSingle AS D WHERE D.SupplierId=View_PayRemind_GetList.Id ) ");

                    if (searchInfo.LSDate.HasValue)
                    {
                        strWhere.AppendFormat(" AND A.LeaveDate>='{0}' ", searchInfo.LSDate.Value);
                    }
                    if (searchInfo.LEDate.HasValue)
                    {
                        strWhere.AppendFormat(" AND A.LeaveDate<='{0}' ", searchInfo.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0)
                    {
                        strWhere.AppendFormat(" AND A.OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorIds));
                    }
                    if (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    {
                        strWhere.AppendFormat(" AND A.OperatorId IN(SELECT E.Id FROM tbl_CompanyUser AS E WHERE E.DepartId IN({0})) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorDepartIds));
                    }

                    strWhere.AppendFormat(" ) ");
                }

                if (!string.IsNullOrEmpty(searchInfo.ShouKuanDanWei))
                {
                    strWhere.AppendFormat(" AND UnitName LIKE '%{0}%' ", searchInfo.ShouKuanDanWei);
                }
            }
            #endregion

            using (IDataReader dr = DbHelper.ExecuteReader(_db, pageSize, pageIndex, ref recordCount
                , "View_PayRemind_GetList", "Id", strFiles.ToString(), strWhere.ToString(), strOrder))
            {
                EyouSoft.Model.PersonalCenterStructure.PayRemind tModel = null;
                System.Xml.XmlDocument xml = null;
                System.Xml.XmlNodeList xmlNodeList = null;
                while (dr.Read())
                {
                    tModel = new EyouSoft.Model.PersonalCenterStructure.PayRemind();
                    if (!dr.IsDBNull(0))
                        tModel.SupplierId = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        tModel.SupplierName = dr.GetString(1);
                    if (!dr.IsDBNull(5))
                        tModel.SupplierType = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.SupplierType), dr.GetByte(dr.GetOrdinal("SupplierType")).ToString());
                    if (!dr.IsDBNull(2))
                    {
                        xml = new System.Xml.XmlDocument();
                        xml.LoadXml(dr.GetString(2));
                        xmlNodeList = xml.GetElementsByTagName("tsc");
                        if (xmlNodeList != null && xmlNodeList.Count > 0)
                        {
                            if (xmlNodeList[0].Attributes["ContactName"] != null)
                                tModel.ContactName = xmlNodeList[0].Attributes["ContactName"].Value;
                            if (xmlNodeList[0].Attributes["ContactTel"] != null)
                                tModel.ContactTel = xmlNodeList[0].Attributes["ContactTel"].Value;
                        }
                    }
                    if (!dr.IsDBNull(3))
                        tModel.PayCash = dr.GetDecimal(3);
                    /*if (!dr.IsDBNull(4)&&!string.IsNullOrEmpty(dr[4].ToString()))
                    {
                        string strPlanNames = string.Empty;
                        xml = new System.Xml.XmlDocument();
                        xml.LoadXml(dr.GetString(4));
                        xmlNodeList = xml.GetElementsByTagName("tpa");
                        if (xmlNodeList != null && xmlNodeList.Count > 0)
                        {
                            foreach (System.Xml.XmlNode t in xmlNodeList)
                            {
                                if (t == null || t.Attributes == null || t.Attributes.Count <= 0)
                                    continue;

                                if (t.Attributes["Operator"] != null && !string.IsNullOrEmpty(t.Attributes["Operator"].Value))
                                {
                                    if (!strPlanNames.Contains(t.Attributes["Operator"].Value))
                                    {
                                        strPlanNames += t.Attributes["Operator"].Value + ",";
                                    }
                                }
                            }
                        }
                        xmlNodeList = xml.GetElementsByTagName("tpto");
                        if (xmlNodeList != null && xmlNodeList.Count > 0)
                        {
                            foreach (System.Xml.XmlNode t in xmlNodeList)
                            {
                                if (t == null || t.Attributes == null || t.Attributes.Count <= 0)
                                    continue;

                                if (t.Attributes["Operator"] != null && !string.IsNullOrEmpty(t.Attributes["Operator"].Value))
                                {
                                    if (!strPlanNames.Contains(t.Attributes["Operator"].Value))
                                    {
                                        strPlanNames += t.Attributes["Operator"].Value + ",";
                                    }
                                }
                            }
                        }
                        strPlanNames = strPlanNames.Trim(',');
                        tModel.JobName = strPlanNames;
                    }*/

                    IList<string> planers = new List<string>();
                    planers = ParseRemindPlanerXML(planers, dr["PlanerXmlDJ"].ToString());
                    planers = ParseRemindPlanerXML(planers, dr["PlanerXmlJP"].ToString());
                    planers = ParseRemindPlanerXML(planers, dr["PlanerXmlDX"].ToString());
                    StringBuilder planer = new StringBuilder();

                    if (planers.Count > 0)
                    {
                        foreach (var s in planers)
                        {
                            planer.Append(s);
                            planer.Append(",");
                        }
                    }

                    tModel.JobName = planer.ToString().TrimEnd(',');

                    if (hasTourSearch) tModel.PayCash = dr.IsDBNull(dr.GetOrdinal("TourSearchAmount")) ? 0 : dr.GetDecimal(dr.GetOrdinal("TourSearchAmount"));

                    list.Add(tModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取付款提醒未付款合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询实体</param>
        /// <param name="weiFuHeJi">未付款合计</param>
        /// <returns></returns>
        public void GetPayRemind(int companyId, EyouSoft.Model.PersonalCenterStructure.FuKuanTiXingChaXun searchInfo, out decimal weiFuHeJi)
        {
            weiFuHeJi = 0;
            StringBuilder cmdText = new StringBuilder();

            cmdText.Append("SELECT SUM(Amount) AS Amount,SUM(TourSearchAmount) AS TourSearchAmount FROM ( ");

            cmdText.Append("SELECT Amount");
            bool hasTourSearch = false;
            if (searchInfo != null)
            {
                if (searchInfo.LEDate.HasValue
                    || searchInfo.LSDate.HasValue
                    || (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    || (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0))
                {
                    hasTourSearch = true;
                    StringBuilder tourSearchWhere = new StringBuilder();

                    tourSearchWhere.Append(" AND EXISTS(SELECT 1 FROM tbl_Tour AS A WHERE 1=1 AND A.IsDelete='0' AND A.TourId={KEY}");

                    if (searchInfo.LSDate.HasValue)
                    {
                        tourSearchWhere.AppendFormat(" AND A.LeaveDate>='{0}' ", searchInfo.LSDate.Value);
                    }
                    if (searchInfo.LEDate.HasValue)
                    {
                        tourSearchWhere.AppendFormat(" AND A.LeaveDate<='{0}' ", searchInfo.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0)
                    {
                        tourSearchWhere.AppendFormat(" AND A.OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorIds));
                    }
                    if (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    {
                        tourSearchWhere.AppendFormat(" AND A.OperatorId IN(SELECT E.Id FROM tbl_CompanyUser AS E WHERE E.DepartId IN({0})) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorDepartIds));
                    }
                    tourSearchWhere.Append(")");


                    cmdText.Append(",(");

                    cmdText.AppendFormat("SELECT SUM(TotalAmount-HasCheckAmount) FROM tbl_StatAllOut WHERE IsDelete='0' AND SupplierId=View_PayRemind_GetList.Id AND TourId IN(SELECT B.TourId FROM tbl_PlanLocalAgency AS B WHERE B.TravelAgencyID=View_PayRemind_GetList.Id {0} UNION SELECT C.TourId FROM tbl_PlanTicketOut AS C WHERE C.State=3 AND C.TicketOfficeId=View_PayRemind_GetList.Id {1} UNION SELECT D.TourId FROM tbl_PlanSingle AS D WHERE D.SupplierId=View_PayRemind_GetList.Id {2}) "
                        , tourSearchWhere.ToString().Replace("{KEY}", "B.TourId ")
                        , tourSearchWhere.ToString().Replace("{KEY}", "C.TourId ")
                        , tourSearchWhere.ToString().Replace("{KEY}", "D.TourId "));

                    cmdText.Append(") AS TourSearchAmount");

                }
                else
                {
                    cmdText.Append(",0 AS TourSearchAmount");
                }
            }
            else
            {
                cmdText.Append(",0 AS TourSearchAmount");
            }

            cmdText.Append(" FROM View_PayRemind_GetList ");
            cmdText.AppendFormat(" WHERE Amount > 0 and CompanyId = {0} ", companyId);

            if (searchInfo != null)
            {
                if (searchInfo.LEDate.HasValue
                    || searchInfo.LSDate.HasValue
                    || (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    || (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0))
                {
                    cmdText.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_Tour AS A WHERE IsDelete='0' AND TourId IN(SELECT B.TourId FROM tbl_PlanLocalAgency AS B WHERE B.TravelAgencyID=View_PayRemind_GetList.Id UNION SELECT C.TourId FROM tbl_PlanTicketOut AS C WHERE C.State=3 AND C.TicketOfficeId=View_PayRemind_GetList.Id UNION SELECT D.TourId FROM tbl_PlanSingle AS D WHERE D.SupplierId=View_PayRemind_GetList.Id ) ");

                    if (searchInfo.LSDate.HasValue)
                    {
                        cmdText.AppendFormat(" AND A.LeaveDate>='{0}' ", searchInfo.LSDate.Value);
                    }
                    if (searchInfo.LEDate.HasValue)
                    {
                        cmdText.AppendFormat(" AND A.LeaveDate<='{0}' ", searchInfo.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0)
                    {
                        cmdText.AppendFormat(" AND A.OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorIds));
                    }
                    if (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    {
                        cmdText.AppendFormat(" AND A.OperatorId IN(SELECT E.Id FROM tbl_CompanyUser AS E WHERE E.DepartId IN({0})) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorDepartIds));
                    }

                    cmdText.AppendFormat(" ) ");
                }

                if (!string.IsNullOrEmpty(searchInfo.ShouKuanDanWei))
                {
                    cmdText.AppendFormat(" AND UnitName LIKE '%{0}%' ", searchInfo.ShouKuanDanWei);
                }
            }

            cmdText.Append(" ) t556464");

            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    weiFuHeJi = rdr.IsDBNull(rdr.GetOrdinal("Amount")) ? 0 : rdr.GetDecimal(rdr.GetOrdinal("Amount"));
                    if (hasTourSearch) weiFuHeJi = rdr.IsDBNull(rdr.GetOrdinal("TourSearchAmount")) ? 0 : rdr.GetDecimal(rdr.GetOrdinal("TourSearchAmount"));
                }
            }
        }

        /// <summary>
        /// 获取付款提醒数量
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public int GetPayRemind(int CompanyId)
        {
            if (CompanyId <= 0)
                return 0;

            StringBuilder strSql = new StringBuilder(" select count(Id) from View_PayRemind_GetList ");
            strSql.AppendFormat(" where TotalAmount > 0 and CompanyId = {0} ", CompanyId);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            object obj = DbHelper.GetSingle(dc, _db);
            if (obj.Equals(null))
                return 0;
            else
                return int.Parse(obj.ToString());
        }

        /// <summary>
        /// 获取供应商列表交易次数合计
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="type">供应商类型</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public int GetTimesGYSSummary(int companyId, EyouSoft.Model.EnumType.CompanyStructure.SupplierType type, EyouSoft.Model.CompanyStructure.MSupplierSearchInfo searchInfo)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.Append(" SELECT SUM([TradeNum]) FROM [tbl_CompanySupplier] WHERE [IsDelete]='0'  ");
            cmdText.AppendFormat(" AND [CompanyId]={0} ", companyId);
            cmdText.AppendFormat(" AND [SupplierType]={0} ", (int)type);

            if (searchInfo != null)
            {
                if (searchInfo.CityId.HasValue && searchInfo.CityId.Value > 0)
                {
                    cmdText.AppendFormat(" AND [CityId]={0} ", searchInfo.CityId.Value);
                }
                if (searchInfo.ProvinceId.HasValue && searchInfo.ProvinceId.Value > 0)
                {
                    cmdText.AppendFormat(" AND [ProvinceId]={0} ", searchInfo.ProvinceId.Value);
                }
                if (!string.IsNullOrEmpty(searchInfo.Name))
                {
                    cmdText.AppendFormat(" AND UnitName LIKE '%{0}%' ", searchInfo.Name);
                }
            }

            DbCommand cmd = this._db.GetSqlStringCommand(cmdText.ToString());

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) return rdr.GetInt32(0);
                }
            }

            return 0;
        }

        /// <summary>
        /// 获取地接供应商交易情况合计信息
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="gysId">供应商编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public EyouSoft.Model.SupplierStructure.MTimesSummaryDiJieInfo GetTimesSummaryDiJie(int companyId, int gysId, EyouSoft.Model.TourStructure.MTimesSummaryDiJieSearchInfo searchInfo)
        {
            EyouSoft.Model.SupplierStructure.MTimesSummaryDiJieInfo info = new EyouSoft.Model.SupplierStructure.MTimesSummaryDiJieInfo();
            var sql = new StringBuilder();
            // 实收人数
            sql.Append(" SELECT");
            sql.Append(" 	SUM([PeopleNumber] - [LeaguePepoleNum]) AS ShiShou");
            sql.Append(" FROM [tbl_TourOrder]");
            sql.Append(" WHERE [OrderState] NOT IN(3,4)");
            sql.Append(" 	AND [IsDelete]='0'");
            sql.Append(" 	AND [TourId] IN(SELECT A.TourId");
            sql.Append(" 					FROM");
            sql.Append(" 						(SELECT [TourId]");
            sql.Append(" 						FROM [tbl_PlanLocalAgency]");
            sql.Append(" 						WHERE [TravelAgencyID] = @GYSID");
            if (searchInfo != null && searchInfo.PayStatus.HasValue)
            {
                switch (searchInfo.PayStatus.Value)
                {
                    case 1:
                        sql.Append(" 						AND TotalAmount=PayAmount");
                        break;
                    case 2:
                        sql.Append(" 						AND TotalAmount<>PayAmount");
                        break;
                }
            }
            sql.Append(" 						UNION ALL");
            sql.Append(" 						SELECT [TourId]");
            sql.Append(" 						FROM [tbl_PlanSingle]");
            sql.Append(" 						WHERE [SupplierId]=@GYSID");
            if (searchInfo != null && searchInfo.PayStatus.HasValue)
            {
                switch (searchInfo.PayStatus.Value)
                {
                    case 1:
                        sql.Append(" 						AND TotalAmount=PaidAmount");
                        break;
                    case 2:
                        sql.Append(" 						AND TotalAmount<>PaidAmount");
                        break;
                }
            }
            sql.Append(" 						) AS A");
            if (searchInfo != null && (searchInfo.SDate.HasValue || searchInfo.EDate.HasValue))
            {
                sql.Append(" 					WHERE (SELECT COUNT(*) ");
                sql.Append(" 						   FROM tbl_Tour AS B");
                sql.Append(" 						   WHERE");
                if (searchInfo.SDate.HasValue)
                {
                    sql.Append(" 						   B.LeaveDate >= @SDate AND");
                }
                if (searchInfo.EDate.HasValue)
                {
                    sql.Append(" 						   B.LeaveDate <= @EDate AND");
                }
                sql.Append(" 						   B.TourId = A.TourId)>0");
            }
            sql.Append(" 						   );");
            // 地接支出合计
            sql.Append(" SELECT");
            sql.Append(" 	SUM([Commission]) AS CommAmount");
            sql.Append(" 	,SUM([TotalAmount]) AS TotalAmount");
            sql.Append(" 	,SUM([PayAmount]) AS PayAmount");
            sql.Append(" FROM [tbl_PlanLocalAgency]");
            sql.Append(" WHERE [TravelAgencyID]=@GYSID");
            if (searchInfo != null && searchInfo.PayStatus.HasValue)
            {
                switch (searchInfo.PayStatus.Value)
                {
                    case 1:
                        sql.Append(" 	AND TotalAmount=PayAmount");
                        break;
                    case 2:
                        sql.Append(" 	AND TotalAmount<>PayAmount");
                        break;
                }
            }
            if (searchInfo != null && (searchInfo.SDate.HasValue || searchInfo.EDate.HasValue))
            {
                sql.Append(" 	AND (SELECT COUNT(*)");
                sql.Append(" 		 FROM tbl_Tour AS A");
                sql.Append(" 		 WHERE");
                if (searchInfo.SDate.HasValue)
                {
                    sql.Append(" 			A.LeaveDate >= @SDate AND");
                }
                if (searchInfo.EDate.HasValue)
                {
                    sql.Append(" 			A.LeaveDate <= @EDate AND");
                }
                sql.Append(" 			A.TourId = tbl_PlanLocalAgency.TourId)>0;");
            }
            // 单项支出合计
            sql.Append(" SELECT");
            sql.Append(" 	SUM([TotalAmount]) AS TotalAmount");
            sql.Append(" 	,SUM([PaidAmount]) AS PayAmount");
            sql.Append(" FROM [tbl_PlanSingle]");
            sql.Append(" WHERE [SupplierId]=@GYSID");
            sql.Append(" 	AND [ServiceType]=@ServiceType");
            sql.Append(" 	AND EXISTS(SELECT 1");
            sql.Append(" 				FROM [tbl_Tour] AS A");
            sql.Append(" 				WHERE A.[TourId]=[tbl_PlanSingle].[TourId] AND A.[IsDelete]='0')");
            if (searchInfo != null && searchInfo.PayStatus.HasValue)
            {
                switch (searchInfo.PayStatus.Value)
                {
                    case 1:
                        sql.Append(" 	AND TotalAmount=PaidAmount");
                        break;
                    case 2:
            sql.Append(" 	AND TotalAmount<>PaidAmount");
                        break;
                }
            }
            if (searchInfo != null && (searchInfo.SDate.HasValue || searchInfo.EDate.HasValue))
            {
                sql.Append(" 	AND (SELECT COUNT(*)");
                sql.Append(" 		 FROM tbl_Tour AS A");
                sql.Append(" 		 WHERE");
                if (searchInfo.SDate.HasValue)
                {
                    sql.Append(" 			A.LeaveDate >= @SDate AND");
                }
                if (searchInfo.EDate.HasValue)
                {
                    sql.Append(" 			A.LeaveDate <= @EDate AND");
                }
                sql.Append(" 			A.TourId = tbl_PlanSingle.TourId)>0;");
            }

            DbCommand cmd = this._db.GetSqlStringCommand(sql.ToString());
            //DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTimesSummaryDiJie);
            this._db.AddInParameter(cmd, "GYSID", DbType.Int32, gysId);
            this._db.AddInParameter(cmd, "ServiceType", DbType.Byte, EyouSoft.Model.EnumType.TourStructure.ServiceType.地接);
            if (searchInfo != null && searchInfo.SDate.HasValue)
            {
                this._db.AddInParameter(cmd, "SDate", DbType.DateTime, searchInfo.SDate);
            }
            if (searchInfo != null && searchInfo.EDate.HasValue)
            {
                this._db.AddInParameter(cmd, "EDate", DbType.DateTime, searchInfo.EDate);
            }

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) info.PeopleNumber = rdr.GetInt32(0);
                }
                rdr.NextResult();
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) info.CommAmount = rdr.GetDecimal(0);
                    if (!rdr.IsDBNull(1)) info.TotalAmount = rdr.GetDecimal(1);
                    if (!rdr.IsDBNull(2)) info.PayAmount = rdr.GetDecimal(2);
                }
                rdr.NextResult();
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) info.TotalAmount = info.TotalAmount + rdr.GetDecimal(0);
                    if (!rdr.IsDBNull(1)) info.PayAmount = info.PayAmount + rdr.GetDecimal(1);
                }
            }

            return info;
        }
        /// <summary>
        /// 获取票务供应商交易情况合计信息
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="gysId">供应商编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SupplierStructure.MTimesSummaryJiPiaoInfo GetTimesSummaryJiPiao(int companyId, int gysId)
        {
            return GetTimesSummaryJiPiao(companyId, gysId, null);
        }
        /// <summary>
        /// 获取票务供应商交易情况合计信息
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="gysId">供应商编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public EyouSoft.Model.SupplierStructure.MTimesSummaryJiPiaoInfo GetTimesSummaryJiPiao(int companyId, int gysId, EyouSoft.Model.TourStructure.MTimesSummaryDiJieSearchInfo searchInfo)
        {
            EyouSoft.Model.SupplierStructure.MTimesSummaryJiPiaoInfo info = new EyouSoft.Model.SupplierStructure.MTimesSummaryJiPiaoInfo();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT SUM([PeopleCount]) AS PeopleNumber,SUM([TotalMoney]) AS TicketAmount,SUM([AgencyPrice]) AS AgencyAmount FROM [tbl_PlanTicketKind] WHERE [TicketId] IN (SELECT O.[ID] FROM [tbl_PlanTicketOut] O,[tbl_Tour]T WHERE O.TourId = T.TourId And O.[TicketOfficeId]=@GYSID ");
            #region 查询信息
            if (searchInfo != null)
            {
                if (searchInfo.PayStatus.HasValue)
                {
                    switch (searchInfo.PayStatus)
                    {
                        case 1:
                            strSql.Append(" AND O.TotalAmount=O.PayAmount");
                            break;
                        case 2:
                            strSql.Append(" AND O.TotalAmount<>O.PayAmount");
                            break;
                    }
                }
                if (searchInfo.SDate.HasValue)
                {
                    strSql.Append(" AND T.LeaveDate >= @SDate");
                }
                if (searchInfo.EDate.HasValue)
                {
                    strSql.Append(" AND T.LeaveDate <= @EDate");
                }
            }
#endregion
            strSql.Append(" And T.TemplateId > '' and T.IsDelete = '0' and T.CompanyId = @CompanyId );");

            strSql.Append("SELECT SUM(O.[TotalAmount]) AS TotalAmount,SUM(O.[PayAmount]) AS PayAmount FROM [tbl_PlanTicketOut] O,[tbl_Tour]T WHERE O.TourId = T.TourId And O.[TicketOfficeId]=@GYSID ");
            #region 查询信息
            if (searchInfo != null)
            {
                if (searchInfo.PayStatus.HasValue)
                {
                    switch (searchInfo.PayStatus)
                    {
                        case 1:
                            strSql.Append(" AND O.TotalAmount=O.PayAmount");
                            break;
                        case 2:
                            strSql.Append(" AND O.TotalAmount<>O.PayAmount");
                            break;
                    }
                }
                if (searchInfo.SDate.HasValue)
                {
                    strSql.Append(" AND T.LeaveDate >= @SDate");
                }
                if (searchInfo.EDate.HasValue)
                {
                    strSql.Append(" AND T.LeaveDate <= @EDate");
                }
            }
            #endregion
            strSql.Append(" And T.TemplateId > '' and T.IsDelete = '0' and T.CompanyId = @CompanyId;");

            strSql.Append("SELECT SUM(O.[TotalAmount]) AS TotalAmount,SUM(O.[PaidAmount]) AS PayAmount FROM [tbl_PlanSingle] O,[tbl_Tour] T WHERE O.TourId=T.TourId AND O.[SupplierId]=@GYSID AND O.[ServiceType]=@ServiceType ");
            #region 查询信息
            if (searchInfo != null)
            {
                if (searchInfo.PayStatus.HasValue)
                {
                    switch (searchInfo.PayStatus)
                    {
                        case 1:
                            strSql.Append(" AND O.TotalAmount=O.PaidAmount");
                            break;
                        case 2:
                            strSql.Append(" AND O.TotalAmount<>O.PaidAmount");
                            break;
                    }
                }
                if (searchInfo.SDate.HasValue)
                {
                    strSql.Append(" AND T.LeaveDate >= @SDate");
                }
                if (searchInfo.EDate.HasValue)
                {
                    strSql.Append(" AND T.LeaveDate <= @EDate");
                }
            }
            #endregion
            strSql.Append(" And T.TemplateId > '' and T.IsDelete = '0' and T.CompanyId = @CompanyId;");
            DbCommand cmd = this._db.GetSqlStringCommand(strSql.ToString());
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);
            this._db.AddInParameter(cmd, "GYSID", DbType.Int32, gysId);
            this._db.AddInParameter(cmd, "ServiceType", DbType.Byte, EyouSoft.Model.EnumType.TourStructure.ServiceType.大交通);
            if (searchInfo != null && searchInfo.SDate.HasValue)
            {
                this._db.AddInParameter(cmd, "SDate", DbType.DateTime, searchInfo.SDate);
            }
            if (searchInfo != null && searchInfo.EDate.HasValue)
            {
                this._db.AddInParameter(cmd, "EDate", DbType.DateTime, searchInfo.EDate);
            }
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) info.PeopleNumber = rdr.GetInt32(0);
                    if (!rdr.IsDBNull(1)) info.TicketAmount = rdr.GetDecimal(1);
                    if (!rdr.IsDBNull(2)) info.AgencyAmount = rdr.GetDecimal(2);
                }
                rdr.NextResult();
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) info.TotalAmount = rdr.GetDecimal(0);
                    if (!rdr.IsDBNull(1)) info.PayAmount = rdr.GetDecimal(1);
                }
                rdr.NextResult();
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) info.TotalAmount = info.TotalAmount + rdr.GetDecimal(0);
                    if (!rdr.IsDBNull(1)) info.PayAmount = info.PayAmount + rdr.GetDecimal(1);
                }
            }
            return info;
        }
        #endregion

        #region 私有成员

        /// <summary>
        /// 根据供应商联系人信息生成XML
        /// </summary>
        /// <param name="list">供应商联系人信息集合</param>
        /// <returns>生SqlXML</returns>
        private string GetSupplierContactXMLByModel(IList<EyouSoft.Model.CompanyStructure.SupplierContact> list)
        {
            if (list == null || list.Count <= 0)
                return string.Empty;

            StringBuilder strXml = new StringBuilder("<ROOT>");
            foreach (var t in list)
            {
                if (t == null)
                    continue;

                //Id,ContactName,JobTitle,ContactFax,ContactTel,ContactMobile,QQ,Email,UserId,UserName,[Password],MD5Password
                strXml.AppendFormat("<CustomerInfo Id = \"{0}\" ContactName = \"{1}\" JobTitle = \"{2}\" ContactFax = \"{3}\" ContactTel = \"{4}\" ContactMobile = \"{5}\" QQ = \"{6}\" Email = \"{7}\" ", t.Id, Utils.ReplaceXmlSpecialCharacter(t.ContactName), Utils.ReplaceXmlSpecialCharacter(t.JobTitle), Utils.ReplaceXmlSpecialCharacter(t.ContactFax), Utils.ReplaceXmlSpecialCharacter(t.ContactTel), Utils.ReplaceXmlSpecialCharacter(t.ContactMobile), Utils.ReplaceXmlSpecialCharacter(t.QQ), Utils.ReplaceXmlSpecialCharacter(t.Email));

                if (t.UserAccount == null || t.UserAccount.PassWordInfo == null)
                    strXml.Append("UserId = \"0\" UserName = \"\" Password = \"\" MD5Password = \"\" ");
                else
                    strXml.AppendFormat("UserId = \"{0}\" UserName = \"{1}\" Password = \"{2}\" MD5Password = \"{3}\" ", t.UserAccount.ID, Utils.ReplaceXmlSpecialCharacter(t.UserAccount.UserName), Utils.ReplaceXmlSpecialCharacter(t.UserAccount.PassWordInfo.NoEncryptPassword), Utils.ReplaceXmlSpecialCharacter(t.UserAccount.PassWordInfo.MD5Password));

                strXml.Append(" />");
            }
            strXml.Append("</ROOT>");

            return strXml.ToString();
        }

        /// <summary>
        /// 转XML格式
        /// </summary>
        /// <param name="ContactXML">XML</param>
        /// <returns></returns>
        private IList<EyouSoft.Model.CompanyStructure.SupplierContact> GetContactList(string ContactXML)
        {
            if (string.IsNullOrEmpty(ContactXML))
                return null;
            IList<EyouSoft.Model.CompanyStructure.SupplierContact> ResultList = null;
            XElement root = XElement.Parse(ContactXML);
            var xRow = root.Elements("row");
            foreach (var tmp in xRow)
            {

                ResultList = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
                EyouSoft.Model.CompanyStructure.SupplierContact model = new EyouSoft.Model.CompanyStructure.SupplierContact()
                {
                    ContactName = tmp.Attribute("ContactName").Value,
                    ContactTel = tmp.Attribute("ContactTel").Value,
                    ContactFax = tmp.Attribute("ContactFax") == null ? "" : tmp.Attribute("ContactFax").Value
                };
                ResultList.Add(model);
                model = null;
            }
            return ResultList;
        }

        /// <summary>
        /// 初始化供应商模型数据
        /// </summary>
        /// <param name="dataModel"></param>
        /// <param name="model"></param>
        private void InitDataModel(EyouSoft.Data.CompanySupplier dataModel, EyouSoft.Model.CompanyStructure.CompanySupplier model)
        {
            dataModel.Id = model.Id;
            dataModel.ProvinceId = model.ProvinceId;
            dataModel.ProvinceName = model.ProvinceName;
            dataModel.CityId = model.CityId;
            dataModel.CityName = model.CityName;
            dataModel.UnitName = model.UnitName;
            dataModel.SupplierType = Convert.ToByte((int)model.SupplierType);
            dataModel.UnitAddress = model.UnitAddress;
            dataModel.Commission = model.Commission;
            dataModel.AgreementFile = model.AgreementFile;
            dataModel.TradeNum = model.TradeNum;
            dataModel.UnitPolicy = model.UnitPolicy;
            dataModel.Remark = model.Remark;
            dataModel.CompanyId = model.CompanyId;
            dataModel.OperatorId = model.OperatorId;
            dataModel.IssueTime = DateTime.Now;
            dataModel.IsDelete = model.IsDelete ? "1" : "0";
        }

        /// <summary>
        /// 删除某供应商所有联系人信息
        /// </summary>
        /// <param name="supplierId">供应商编号</param>
        /// <returns></returns>
        private bool SupplierDelete(int supplierId)
        {
            bool IsTrue = false;
            IEnumerable<EyouSoft.Data.SupplierContact> Lists = from item in dcDal.SupplierContact
                                                               where item.SupplierId == supplierId
                                                               select item;
            dcDal.SupplierContact.DeleteAllOnSubmit<EyouSoft.Data.SupplierContact>(Lists);
            dcDal.SubmitChanges();
            if (dcDal.ChangeConflicts.Count == 0)
            {
                IsTrue = true;
            }
            Lists = null;
            return IsTrue;
        }

        /// <summary>
        /// 获取省份编号
        /// </summary>
        /// <param name="provinceName">省份名称</param>
        /// <returns></returns>
        private int GetProvinceId(string provinceName)
        {
            int provinceId = 0;
            string SQL_GetProvinceId = "select Id from tbl_CompanyProvince where ProvinceName = @ProvinceName";

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_GetProvinceId);
            this._db.AddInParameter(cmd, "ProvinceName", DbType.String, provinceName);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    provinceId = rdr.IsDBNull(rdr.GetOrdinal("Id")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("Id"));
                }
            }
            return provinceId;
        }

        /// <summary>
        /// 获取城市编号
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <returns></returns>
        private int GetCityId(string cityName)
        {
            int cityId = 0;
            string SQL_GetCityId = "select Id from tbl_CompanyCity where CityName = @CityName";

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_GetCityId);
            this._db.AddInParameter(cmd, "CityName", DbType.String, cityName);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    cityId = rdr.IsDBNull(rdr.GetOrdinal("Id")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("Id"));
                }
            }
            return cityId;
        }

        /// <summary>
        /// parse remind planer xml
        /// </summary>
        /// <param name="planer">planers</param>
        /// <param name="xml">planer xml</param>
        private IList<string> ParseRemindPlanerXML(IList<string> planers, string xml)
        {
            if (string.IsNullOrEmpty(xml)) return planers;
            if (planers == null) planers = new List<string>();

            XElement xroot = XElement.Parse(xml);
            var xraws = Utils.GetXElements(xroot, "row");
            if (xraws != null && xraws.Count() > 0)
            {
                foreach (var xraw in xraws)
                {
                    string name = Utils.GetXAttributeValue(xraw, "Name");
                    if (!string.IsNullOrEmpty(name) && !planers.Contains(name))
                    {
                        planers.Add(name);
                    }
                }
            }

            return planers;
        }
        #endregion
    }

    /// <summary>
    /// 供应商基本信息维护
    /// </summary>
    public class SupplierBaseHandle : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.ISupplierBaseHandle
    {
        private readonly Database _db = null;

        #region 静态Sql语句

        /// <summary>
        /// 供应商基本信息插入Sql
        /// </summary>
        private const string Sql_SupplierBase_Insert = @" INSERT INTO [tbl_CompanySupplier]
           ([ProvinceId]
           ,[ProvinceName]
           ,[CityId]
           ,[CityName]
           ,[UnitName]
           ,[SupplierType]
           ,[UnitAddress]
           ,[TradeNum]
           ,[Remark]
           ,[CompanyId]
           ,[OperatorId]
           ,[IssueTime]
           ,[IsDelete]
           ,[LicenseKey])
     VALUES
           (@ProvinceId
           ,@ProvinceName
           ,@CityId
           ,@CityName
           ,@UnitName
           ,@SupplierType
           ,@UnitAddress
           ,@TradeNum
           ,@Remark
           ,@CompanyId
           ,@OperatorId
           ,@IssueTime
           ,@IsDelete
           ,@LicenseKey);select @@IDENTITY; ";

        /// <summary>
        /// 供应商基本信息修改Sql
        /// </summary>
        private const string Sql_SupplierBase_Update = @" UPDATE [tbl_CompanySupplier]
   SET [ProvinceId] = @ProvinceId
      ,[ProvinceName] = @ProvinceName
      ,[CityId] = @CityId
      ,[CityName] = @CityName
      ,[UnitName] = @UnitName
      ,[UnitAddress] = @UnitAddress
      ,[TradeNum] = @TradeNum
      ,[Remark] = @Remark
      ,[LicenseKey]=@LicenseKey
 WHERE Id = @Id ";

        /// <summary>
        /// 设置供应商基本信息删除状态Sql
        /// </summary>
        private const string Sql_SupplierBase_UpdateIsDelete = @" UPDATE [tbl_CompanySupplier]
   SET [IsDelete] = @IsDelete WHERE Id in ({0}); ";

        /// <summary>
        /// 供应商基本信查询Sql
        /// </summary>
        private const string Sql_SupplierBase_Select = @" SELECT [Id]
      ,[ProvinceId]
      ,[ProvinceName]
      ,[CityId]
      ,[CityName]
      ,[UnitName]
      ,[SupplierType]
      ,[UnitAddress]
      ,[TradeNum]
      ,[Remark]
      ,[CompanyId]
      ,[OperatorId]
      ,[IssueTime]
      ,[IsDelete]
      ,[LicenseKey]
  FROM [tbl_CompanySupplier] ";

        /// <summary>
        /// 供应商联系人删除Sql
        /// </summary>
        public const string Sql_SupplierContact_Delete = @" DELETE FROM [tbl_SupplierContact] WHERE SupplierId = {0}; ";

        /// <summary>
        /// 供应商联系人插入Sql
        /// </summary>
        private const string Sql_SupplierContact_Insert = @" INSERT INTO [tbl_SupplierContact]
        ([CompanyId]
           ,[SupplierId]
           ,[SupplierType]
           ,[ContactName]
           ,[JobTitle]
           ,[ContactFax]
           ,[ContactTel]
           ,[ContactMobile]
           ,[QQ]
           ,[Email])
     VALUES
           ({0}
           ,{1}
           ,{2}
           ,'{3}'
           ,'{4}'
           ,'{5}'
           ,'{6}'
           ,'{7}'
           ,'{8}'
           ,'{9}'); ";

        /// <summary>
        /// 供应商附件删除Sql
        /// </summary>
        public const string Sql_SupplierPic_Delete = @" DELETE FROM [tbl_SupplierAccessory] WHERE SupplierId = {0}; ";

        /// <summary>
        /// 供应商附件插入Sql
        /// </summary>
        public const string Sql_SupplierPic_Insert = @" INSERT INTO [tbl_SupplierAccessory]
           ([SupplierId]
           ,[PicName]
           ,[PicPath])
     VALUES
           ({0}
           ,'{1}'
           ,'{2}'); ";

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public SupplierBaseHandle()
        {
            this._db = base.SystemStore;
        }

        #endregion

        #region ISupplierBaseHandle 成员

        /// <summary>
        /// 添加供应商基本信息（不含联系人，附件）
        /// </summary>
        /// <param name="model">供应商基本信息实体</param>
        /// <returns>供应商基本信息Id</returns>
        public int AddSupplierBase(EyouSoft.Model.CompanyStructure.SupplierBasic model)
        {
            if (model == null)
                return 0;

            DbCommand dc = _db.GetSqlStringCommand(Sql_SupplierBase_Insert);
            _db.AddInParameter(dc, "ProvinceId", DbType.Int32, model.ProvinceId);
            _db.AddInParameter(dc, "ProvinceName", DbType.String, model.ProvinceName);
            _db.AddInParameter(dc, "CityId", DbType.Int32, model.CityId);
            _db.AddInParameter(dc, "CityName", DbType.String, model.CityName);
            _db.AddInParameter(dc, "UnitName", DbType.String, model.UnitName);
            _db.AddInParameter(dc, "SupplierType", DbType.Byte, (int)model.SupplierType);
            _db.AddInParameter(dc, "UnitAddress", DbType.String, model.UnitAddress);
            _db.AddInParameter(dc, "TradeNum", DbType.Int32, model.TradeNum);
            _db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            _db.AddInParameter(dc, "IssueTime", DbType.DateTime, DateTime.Now);
            _db.AddInParameter(dc, "IsDelete", DbType.AnsiStringFixedLength, "0");
            _db.AddInParameter(dc, "LicenseKey", DbType.String, model.LicenseKey);

            object obj = DbHelper.GetSingle(dc, _db);
            if (obj.Equals(null))
                return 0;
            else
                return Convert.ToInt32(obj);
        }

        /// <summary>
        ///  修改供应商基本信息（不含联系人，附件）
        /// </summary>
        /// <param name="model">供应商基本信息实体</param>
        /// <returns>返回1成功；其他失败</returns>
        public int UpdateSupplierBase(EyouSoft.Model.CompanyStructure.SupplierBasic model)
        {
            if (model == null)
                return 0;

            DbCommand dc = _db.GetSqlStringCommand(Sql_SupplierBase_Update);
            _db.AddInParameter(dc, "ProvinceId", DbType.Int32, model.ProvinceId);
            _db.AddInParameter(dc, "ProvinceName", DbType.String, model.ProvinceName);
            _db.AddInParameter(dc, "CityId", DbType.Int32, model.CityId);
            _db.AddInParameter(dc, "CityName", DbType.String, model.CityName);
            _db.AddInParameter(dc, "UnitName", DbType.String, model.UnitName);
            _db.AddInParameter(dc, "UnitAddress", DbType.String, model.UnitAddress);
            _db.AddInParameter(dc, "TradeNum", DbType.Int32, model.TradeNum);
            _db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            _db.AddInParameter(dc, "Id", DbType.Int32, model.Id);
            _db.AddInParameter(dc, "LicenseKey", DbType.String, model.LicenseKey);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 删除供应商基本信息
        /// </summary>
        /// <param name="IsDelete">删除状态</param>
        /// <param name="SupplierBaseIds">供应商Id集合</param>
        /// <returns></returns>
        public bool DeleteSupplierBase(bool IsDelete, params int[] SupplierBaseIds)
        {
            if (SupplierBaseIds == null || SupplierBaseIds.Length <= 0)
                return false;

            string strSql = Sql_SupplierBase_UpdateIsDelete;
            string strIds = string.Empty;
            foreach (int i in SupplierBaseIds)
            {
                if (i > 0)
                    strIds += i.ToString() + ",";
            }
            strIds = strIds.Trim(',');
            if (string.IsNullOrEmpty(strIds))
                return false;

            strSql = string.Format(strSql, strIds);
            DbCommand dc = _db.GetSqlStringCommand(strSql);
            _db.AddInParameter(dc, "IsDelete", DbType.AnsiStringFixedLength, IsDelete ? "1" : "0");

            return DbHelper.ExecuteSql(dc, _db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取供应商基本信息
        /// </summary>
        /// <param name="SupplierBaseId">供应商基本信息Id</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.SupplierBasic GetSupplierBase(int SupplierBaseId)
        {
            if (SupplierBaseId <= 0)
                return null;

            EyouSoft.Model.CompanyStructure.SupplierBasic model = new EyouSoft.Model.CompanyStructure.SupplierBasic();
            model.SupplierContact = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
            model.SupplierPic = new List<EyouSoft.Model.SupplierStructure.SupplierPic>();
            EyouSoft.Model.CompanyStructure.SupplierContact ContactModel = null;
            EyouSoft.Model.SupplierStructure.SupplierPic PicModel = null;
            string strSql = Sql_SupplierBase_Select + string.Format(" where Id = {0}; ", SupplierBaseId);
            strSql += string.Format(" SELECT [Id],[CompanyId],[SupplierId],[SupplierType],[ContactName],[JobTitle],[ContactFax],[ContactTel],[ContactMobile],[QQ],[Email] FROM [tbl_SupplierContact] where SupplierId = {0}; ", SupplierBaseId);
            strSql += string.Format(" SELECT [Id],[SupplierId],[PicName],[PicPath] FROM [tbl_SupplierAccessory] where SupplierId = {0}; ", SupplierBaseId);

            DbCommand dc = _db.GetSqlStringCommand(strSql);

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                #region 基本信息

                if (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                        model.Id = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        model.ProvinceId = dr.GetInt32(1);
                    if (!dr.IsDBNull(2))
                        model.ProvinceName = dr.GetString(2);
                    if (!dr.IsDBNull(3))
                        model.CityId = dr.GetInt32(3);
                    if (!dr.IsDBNull(4))
                        model.CityName = dr.GetString(4);
                    if (!dr.IsDBNull(5))
                        model.UnitName = dr.GetString(5);
                    if (!dr.IsDBNull(6))
                        model.SupplierType = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)Convert.ToInt32(dr.GetByte(6));
                    if (!dr.IsDBNull(7))
                        model.UnitAddress = dr.GetString(7);
                    if (!dr.IsDBNull(8))
                        model.TradeNum = dr.GetInt32(8);
                    if (!dr.IsDBNull(9))
                        model.Remark = dr.GetString(9);
                    if (!dr.IsDBNull(10))
                        model.CompanyId = dr.GetInt32(10);
                    if (!dr.IsDBNull(11))
                        model.OperatorId = dr.GetInt32(11);
                    if (!dr.IsDBNull(12))
                        model.IssueTime = dr.GetDateTime(12);
                    if (!dr.IsDBNull(13))
                    {
                        this.GetBoolean(dr.GetString(13));
                    }
                    if (!dr.IsDBNull(14))
                    {
                        model.LicenseKey = dr["LicenseKey"].ToString();                       
                    }
                }
                #endregion

                #region 联系人信息

                dr.NextResult();
                while (dr.Read())
                {
                    ContactModel = new EyouSoft.Model.CompanyStructure.SupplierContact();
                    if (!dr.IsDBNull(0))
                        ContactModel.Id = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        ContactModel.CompanyId = dr.GetInt32(1);
                    if (!dr.IsDBNull(2))
                        ContactModel.SupplierId = dr.GetInt32(2);
                    if (!dr.IsDBNull(3))
                        ContactModel.SupplierType = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)Convert.ToInt32(dr.GetByte(3));
                    if (!dr.IsDBNull(4))
                        ContactModel.ContactName = dr.GetString(4);
                    if (!dr.IsDBNull(5))
                        ContactModel.JobTitle = dr.GetString(5);
                    if (!dr.IsDBNull(6))
                        ContactModel.ContactFax = dr.GetString(6);
                    if (!dr.IsDBNull(7))
                        ContactModel.ContactTel = dr.GetString(7);
                    if (!dr.IsDBNull(8))
                        ContactModel.ContactMobile = dr.GetString(8);
                    if (!dr.IsDBNull(9))
                        ContactModel.QQ = dr.GetString(9);
                    if (!dr.IsDBNull(10))
                        ContactModel.Email = dr.GetString(10);

                    model.SupplierContact.Add(ContactModel);
                }

                #endregion

                #region 附件信息

                dr.NextResult();
                while (dr.Read())
                {
                    PicModel = new EyouSoft.Model.SupplierStructure.SupplierPic();
                    if (!dr.IsDBNull(0))
                        PicModel.Id = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        PicModel.SupplierId = dr.GetInt32(1);
                    if (!dr.IsDBNull(2))
                        PicModel.PicName = dr.GetString(2);
                    if (!dr.IsDBNull(3))
                        PicModel.PicPath = dr.GetString(3);

                    model.SupplierPic.Add(PicModel);
                }

                #endregion
            }

            return model;
        }

        /// <summary>
        /// 查询供应商基本信息
        /// </summary>
        /// <param name="CompanyId">专线Id</param>
        /// <param name="SeachModel">供应商查询实体</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.SupplierBasic> GetSupplierBaseList(int CompanyId
            , EyouSoft.Model.SupplierStructure.SupplierQuery SeachModel, int PageSize, int PageIndex, ref int RecordCount)
        {

            IList<EyouSoft.Model.CompanyStructure.SupplierBasic> list = new List<EyouSoft.Model.CompanyStructure.SupplierBasic>();
            EyouSoft.Model.CompanyStructure.CompanySupplier model = null;
            string tableName = "tbl_CompanySupplier";
            StringBuilder fields = new StringBuilder();
            fields.Append(" Id,UnitAddress,UnitName,ProvinceName,CityName,");
            fields.Append(" (select top 1 ContactName,ContactTel,ContactFax from tbl_SupplierContact a where a.SupplierId = tbl_CompanySupplier.[Id] for xml raw,root('root')) as ContactXML,");
            fields.Append(" TradeNum,UnitPolicy ");
            fields.Append(",LicenseKey");
            string primaryKey = "Id";
            string orderbyStr = "IssueTime desc";
            StringBuilder strWhere = new StringBuilder(" IsDelete='0' ");
            strWhere.AppendFormat(" and CompanyId = {0} ", CompanyId);
            if (SeachModel != null)
            {
                if (SeachModel.ProvinceId > 0)
                    strWhere.AppendFormat(" and ProvinceId = {0} ", SeachModel.ProvinceId);
                if (!string.IsNullOrEmpty(SeachModel.ProvinceName))
                    strWhere.AppendFormat(" and ProvinceName like '%{0}%' ", SeachModel.ProvinceName);
                if (SeachModel.CityId > 0)
                    strWhere.AppendFormat(" and CityId = {0} ", SeachModel.CityId);
                if (!string.IsNullOrEmpty(SeachModel.CityName))
                    strWhere.AppendFormat(" and CityName like '%{0}%' ", SeachModel.CityName);
                if (!string.IsNullOrEmpty(SeachModel.UnitName))
                    strWhere.AppendFormat(" and UnitName like '%{0}%' ", SeachModel.UnitName);
            }

            using (IDataReader dr = DbHelper.ExecuteReader(base.SystemStore, PageSize, PageIndex, ref RecordCount, tableName, primaryKey, fields.ToString(), strWhere.ToString(), orderbyStr))
            {
                while (dr.Read())
                {
                    model = new EyouSoft.Model.CompanyStructure.CompanySupplier();
                    model.Id = dr.GetInt32(dr.GetOrdinal("Id"));
                    model.UnitAddress = dr.IsDBNull(dr.GetOrdinal("UnitAddress")) ? "" : dr.GetOrdinal("UnitAddress").ToString();
                    model.UnitName = dr.GetString(dr.GetOrdinal("UnitName"));
                    model.ProvinceName = dr.IsDBNull(dr.GetOrdinal("ProvinceName")) ? "" : dr.GetString(dr.GetOrdinal("ProvinceName"));
                    model.CityName = dr.IsDBNull(dr.GetOrdinal("CityName")) ? "" : dr.GetString(dr.GetOrdinal("CityName"));
                    model.TradeNum = dr.GetInt32(dr.GetOrdinal("TradeNum"));
                    model.UnitPolicy = dr.IsDBNull(dr.GetOrdinal("UnitPolicy")) ? "" : dr[dr.GetOrdinal("UnitPolicy")].ToString();
                    model.SupplierContact = GetContactList(dr.IsDBNull(dr.GetOrdinal("ContactXML")) ? "" : dr.GetString(dr.GetOrdinal("ContactXML")));
                    if (!dr.IsDBNull(dr.GetOrdinal("LicenseKey")))
                    {
                        model.LicenseKey = dr["LicenseKey"].ToString();
                    }

                    list.Add(model);
                }
            }

            return list;
        }

        /// <summary>
        /// 设置供应商联系人（先删除在添加模式）
        /// </summary>
        /// <param name="SupplierBasicId">供应商编号（小于等于0则取集合内的供应商编号）</param>
        /// <param name="list">供应商联系人集合</param>
        /// <returns>返回1成功；其他失败</returns>
        public int SetSupplierContact(int SupplierBasicId, IList<EyouSoft.Model.CompanyStructure.SupplierContact> list)
        {
            StringBuilder strSql = new StringBuilder();
            if (SupplierBasicId > 0)
                strSql.AppendFormat(Sql_SupplierContact_Delete, SupplierBasicId);

            if (list != null && list.Count > 0)
            {
                IList<int> OldSupplierBasicId = new List<int>();
                foreach (EyouSoft.Model.CompanyStructure.SupplierContact t in list)
                {
                    if (t == null)
                        continue;

                    if (SupplierBasicId > 0)
                        strSql.AppendFormat(Sql_SupplierContact_Insert, t.CompanyId, SupplierBasicId, (int)t.SupplierType
                            , t.ContactName, t.JobTitle, t.ContactFax, t.ContactTel, t.ContactMobile, t.QQ, t.Email);
                    else
                    {
                        if (OldSupplierBasicId.Contains(t.SupplierId))
                            strSql.AppendFormat(Sql_SupplierContact_Insert, t.CompanyId, t.SupplierId, (int)t.SupplierType
                            , t.ContactName, t.JobTitle, t.ContactFax, t.ContactTel, t.ContactMobile, t.QQ, t.Email);
                        else
                        {
                            strSql.AppendFormat(Sql_SupplierContact_Delete, t.SupplierId);
                            strSql.AppendFormat(Sql_SupplierContact_Insert, t.CompanyId, t.SupplierId, (int)t.SupplierType
                            , t.ContactName, t.JobTitle, t.ContactFax, t.ContactTel, t.ContactMobile, t.QQ, t.Email);
                        }

                        OldSupplierBasicId.Add(t.SupplierId);
                    }
                }
                OldSupplierBasicId = null;
            }

            if (string.IsNullOrEmpty(strSql.ToString()))
                return 0;

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 设置供应商附件（先删除在添加模式）
        /// </summary>
        /// <param name="SupplierBasicId">供应商编号（小于等于0则取集合内的供应商编号）</param>
        /// <param name="list">供应商附件集合</param>
        /// <returns>返回1成功；其他失败</returns>
        public int SetSupplierAccessory(int SupplierBasicId, IList<EyouSoft.Model.SupplierStructure.SupplierPic> list)
        {
            StringBuilder strSql = new StringBuilder();
            if (SupplierBasicId > 0)
                strSql.AppendFormat(Sql_SupplierPic_Delete, SupplierBasicId);

            if (list != null && list.Count > 0)
            {
                IList<int> OldSupplierBasicId = new List<int>();
                foreach (EyouSoft.Model.SupplierStructure.SupplierPic t in list)
                {
                    if (t == null)
                        continue;

                    if (SupplierBasicId > 0)
                        strSql.AppendFormat(Sql_SupplierPic_Insert, SupplierBasicId, t.PicName, t.PicPath);
                    else
                    {
                        if (OldSupplierBasicId.Contains(t.SupplierId))
                            strSql.AppendFormat(Sql_SupplierPic_Insert, t.SupplierId, t.PicName, t.PicPath);
                        else
                        {
                            strSql.AppendFormat(Sql_SupplierPic_Delete, t.SupplierId);
                            strSql.AppendFormat(Sql_SupplierPic_Insert, t.SupplierId, t.PicName, t.PicPath);
                        }

                        OldSupplierBasicId.Add(t.SupplierId);
                    }
                }
                OldSupplierBasicId = null;
            }

            if (string.IsNullOrEmpty(strSql.ToString()))
                return 0;

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        #endregion

        #region 私有函数

        /// <summary>
        /// 转XML格式
        /// </summary>
        /// <param name="ContactXML">XML</param>
        /// <returns></returns>
        private IList<EyouSoft.Model.CompanyStructure.SupplierContact> GetContactList(string ContactXML)
        {
            if (string.IsNullOrEmpty(ContactXML))
                return null;
            IList<EyouSoft.Model.CompanyStructure.SupplierContact> ResultList = null;
            XElement root = XElement.Parse(ContactXML);
            if (root == null)
                return null;
            var xRow = root.Elements("row");
            if (xRow == null)
                return null;
            foreach (var tmp in xRow)
            {
                if (tmp == null)
                    continue;

                ResultList = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
                EyouSoft.Model.CompanyStructure.SupplierContact model = new EyouSoft.Model.CompanyStructure.SupplierContact()
                {
                    ContactName = tmp.Attribute("ContactName") == null ? string.Empty : tmp.Attribute("ContactName").Value,
                    ContactTel = tmp.Attribute("ContactTel") == null ? string.Empty : tmp.Attribute("ContactTel").Value,
                    ContactFax = tmp.Attribute("ContactFax") == null ? string.Empty : tmp.Attribute("ContactFax").Value
                };
                ResultList.Add(model);
                model = null;
            }
            return ResultList;
        }        
        #endregion
    }
}
