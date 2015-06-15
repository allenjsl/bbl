using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using EyouSoft.Toolkit;
using System.Data;
using EyouSoft.Toolkit.DAL;
using System.Xml.Linq;

namespace EyouSoft.DAL.SupplierStructure
{
    /// <summary>
    /// 供应商酒店DAL
    /// author:xuqh 2011-03-08
    /// </summary>
    public class SupplierShopping : EyouSoft.Toolkit.DAL.DALBase,EyouSoft.IDAL.SupplierStructure.ISupplierShopping
    {
        private const string SQL_Add = "insert into tbl_SupplierShopping (Id,SaleProduct,GuideWord) values(@Id,@SaleProduct,@GuideWord);update tbl_CompanySupplier set AgreementFile = @AgreementFile where Id = @Id";
        private const string SQL_Update = "update tbl_SupplierShopping set SaleProduct = @SaleProduct,GuideWord = @GuideWord where Id = @Id;update tbl_CompanySupplier set AgreementFile = @AgreementFile where Id = @Id";

        private readonly Database _db = null;

        #region 构造函数
        public SupplierShopping()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region ICompanyShopping 成员

        /// <summary>
        /// 添加供应商-购物信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="supplierId">供应商基本信息ID</param>
        /// <returns></returns>
        public bool AddShopping(EyouSoft.Model.SupplierStructure.SupplierShopping model,int supplierId)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_Add);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, supplierId);
            this._db.AddInParameter(cmd, "SaleProduct", DbType.String, model.SaleProduct);
            this._db.AddInParameter(cmd, "GuideWord", DbType.String, model.GuideWord);
            this._db.AddInParameter(cmd, "AgreementFile", DbType.String, model.AgreementFile);
            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 修改供应商购物信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="supplierId">供应商基本信息ID</param>
        /// <returns></returns>
        public bool UpdateShopping(EyouSoft.Model.SupplierStructure.SupplierShopping model,int supplierId)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_Update);
            this._db.AddInParameter(cmd, "SaleProduct", DbType.String, model.SaleProduct);
            this._db.AddInParameter(cmd, "GuideWord", DbType.String, model.GuideWord);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, supplierId);
            this._db.AddInParameter(cmd, "AgreementFile", DbType.String, model.AgreementFile);
            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 删除供应商购物信息
        /// </summary>
        /// <param name="ids">ID数组</param>
        /// <returns></returns>
        public bool DeleteShopping(params int[] ids)
        {
            DbCommand cmd = this._db.GetSqlStringCommand("update tbl_CompanySupplier set IsDelete = '1' where Id in (" + ConvertToString(ids) + ")");
            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, _db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取供应商购物实体
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public EyouSoft.Model.SupplierStructure.SupplierShopping GetModel(int id)
        {
            EyouSoft.Model.SupplierStructure.SupplierShopping model = null;
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Supplier_GetShopping");
            this._db.AddInParameter(cmd, "Id", DbType.Int32, id);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    #region 供应商-购物信息
                    model = new EyouSoft.Model.SupplierStructure.SupplierShopping();
                    model.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    model.ProvinceId = rdr.GetInt32(rdr.GetOrdinal("ProvinceId"));
                    model.ProvinceName = rdr.IsDBNull(rdr.GetOrdinal("ProvinceName")) ? "" : rdr.GetString(rdr.GetOrdinal("ProvinceName"));
                    model.CityId = rdr.GetInt32(rdr.GetOrdinal("CityId"));
                    model.CityName = rdr.IsDBNull(rdr.GetOrdinal("CityName")) ? "" : rdr.GetString(rdr.GetOrdinal("GuideWord"));
                    model.UnitName = rdr.GetString(rdr.GetOrdinal("UnitName"));
                    model.SupplierType = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.SupplierType), rdr.GetByte(rdr.GetOrdinal("SupplierType")).ToString());
                    model.UnitAddress = rdr.IsDBNull(rdr.GetOrdinal("UnitAddress")) ? "" : rdr.GetString(rdr.GetOrdinal("UnitAddress"));
                    //model.c = rdr.GetInt32(rdr.GetOrdinal("Commission"));
                    model.AgreementFile = rdr.IsDBNull(rdr.GetOrdinal("AgreementFile")) ? "" : rdr.GetString(rdr.GetOrdinal("AgreementFile"));
                    model.TradeNum = rdr.GetInt32(rdr.GetOrdinal("TradeNum"));
                    model.Remark = rdr.IsDBNull(rdr.GetOrdinal("Remark")) ? "" : rdr.GetString(rdr.GetOrdinal("Remark"));
                    model.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    model.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    model.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    model.IsDelete = Convert.ToBoolean(rdr.GetOrdinal("IsDelete"));
                    model.SaleProduct = rdr.IsDBNull(rdr.GetOrdinal("SaleProduct")) ? "" : rdr.GetString(rdr.GetOrdinal("SaleProduct"));
                    model.GuideWord = rdr.IsDBNull(rdr.GetOrdinal("GuideWord")) ? "" : rdr.GetString(rdr.GetOrdinal("GuideWord"));
                    #endregion

                    #region 供应商联系人信息
                    rdr.NextResult();
                    IList<EyouSoft.Model.CompanyStructure.SupplierContact> ls = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();

                    while(rdr.Read())
                    {
                        EyouSoft.Model.CompanyStructure.SupplierContact contactModel = new EyouSoft.Model.CompanyStructure.SupplierContact();
                        contactModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                        contactModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                        contactModel.SupplierId = rdr.GetInt32(rdr.GetOrdinal("SupplierId"));
                        contactModel.SupplierType = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.SupplierType), rdr.GetByte(rdr.GetOrdinal("SupplierType")).ToString());
                        contactModel.ContactName = rdr.IsDBNull(rdr.GetOrdinal("ContactName")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactName"));
                        contactModel.JobTitle = rdr.IsDBNull(rdr.GetOrdinal("JobTitle")) ? "" : rdr.GetString(rdr.GetOrdinal("JobTitle"));
                        contactModel.ContactFax = rdr.IsDBNull(rdr.GetOrdinal("ContactFax")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactFax"));
                        contactModel.ContactTel = rdr.IsDBNull(rdr.GetOrdinal("ContactTel")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactTel"));
                        contactModel.ContactMobile = rdr.IsDBNull(rdr.GetOrdinal("ContactMobile")) ? "" : rdr.GetString(rdr.GetOrdinal("ContactMobile"));
                        contactModel.QQ = rdr.IsDBNull(rdr.GetOrdinal("QQ")) ? "" : rdr.GetString(rdr.GetOrdinal("QQ"));
                        contactModel.Email = rdr.IsDBNull(rdr.GetOrdinal("Email")) ? "" : rdr.GetString(rdr.GetOrdinal("Email"));
                        ls.Add(contactModel);
                        contactModel = null;
                    }
                    #endregion

                    model.SupplierContact = ls;
                }
            }

            return model;
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="ls">数据集合</param>
        /// <returns></returns>
        public bool ImportExcelData(List<EyouSoft.Model.SupplierStructure.SupplierShopping> ls)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("begin ");
            sql.Append("declare @supplierId int set @supplierId = 0 ");

            foreach (EyouSoft.Model.SupplierStructure.SupplierShopping item in ls)
            {
                sql.AppendFormat(" insert into tbl_CompanySupplier(ProvinceId,ProvinceName,CityId,CityName,UnitName,"
                                +" SupplierType,UnitAddress,AgreementFile,TradeNum,"
		                        +" Remark,CompanyId,OperatorId,IssueTime,IsDelete)"
                                +" values({0},'{1}',{2},'{3}','{4}',"
                                +" {5},'{6}','{7}',{8},'{9}',"
	                            +" {10},{11},'{12}',{13});",
                                item.ProvinceId,item.ProvinceName,item.CityId,item.CityName,item.UnitName,
                                (byte)item.SupplierType,item.UnitAddress,item.AgreementFile,item.TradeNum,item.Remark,
                                item.CompanyId,item.OperatorId,item.IssueTime,item.IsDelete ? "1":"0");
                sql.Append(" select @supplierId = @@identity");

                if (item.SupplierContact != null)
                {
                    foreach (EyouSoft.Model.CompanyStructure.SupplierContact contact in item.SupplierContact)
                    {
                        sql.AppendFormat(" insert into tbl_SupplierContact(CompanyId,SupplierId,SupplierType,ContactName "
                                        +",JobTitle,ContactFax,ContactTel,ContactMobile,QQ,Email)"
                                        +" values({0},@supplierId,{1},'{2}'"
                                        +",'{3}','{4}','{5}','{6}','{7}','{8}' );",
                                        contact.CompanyId,(byte)contact.SupplierType,contact.ContactName,contact.JobTitle,contact.ContactFax,
                                        contact.ContactTel,contact.ContactMobile,contact.QQ,contact.Email);
                    }
                }

                sql.AppendFormat(" insert into tbl_SupplierShopping(Id,SaleProduct,GuideWord)VALUES(@supplierId,'{0}','{1}');",
                                item.SaleProduct,item.GuideWord);
                sql.Append(" set @supplierId = 0");
            }
            sql.Append(" end");

            if (string.IsNullOrEmpty(sql.ToString()))
                return true;

            DbCommand cmd = this._db.GetSqlStringCommand(sql.ToString());
            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSqlTrans(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 分页获取供应商购物信息
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="pageIndex">起始页</param>
        /// <param name="recordCount">总数</param>
        /// <param name="supplierType">供应商类型</param>
        /// <param name="queryModel">查询实体</param>
        public IList<EyouSoft.Model.SupplierStructure.SupplierShopping> GetList(int pageSize, int pageIndex, ref int recordCount,int companyId, EyouSoft.Model.EnumType.CompanyStructure.SupplierType supplierType, EyouSoft.Model.SupplierStructure.SupplierQuery queryModel)
        {
            IList<EyouSoft.Model.SupplierStructure.SupplierShopping> ls = new List<EyouSoft.Model.SupplierStructure.SupplierShopping>();
            string tableName = "tbl_CompanySupplier";
            StringBuilder fields = new StringBuilder();
            fields.Append(" Id,UnitAddress,ProvinceName,CityName,");
            fields.Append(" (select top 1 ContactName,ContactTel,ContactFax from tbl_SupplierContact a where a.SupplierId = tbl_CompanySupplier.[Id] for xml raw,root('root')) as ContactXML,");
            fields.Append(" (select top 1 SaleProduct from tbl_SupplierShopping b where b.Id = tbl_CompanySupplier.[Id] for xml raw,root('root')) as ShoppingXML,");
            fields.Append(" UnitName,TradeNum ");
            string primaryKey = "Id";
            string orderbyStr = "IssueTime desc";
            StringBuilder strWhere = new StringBuilder(" IsDelete='0' ");
            strWhere.AppendFormat(" and SupplierType={0}", Convert.ToByte((int)supplierType));
            strWhere.AppendFormat(" and CompanyId = {0}", companyId);

            if (queryModel != null)
            {
                if (queryModel.ProvinceId > 0)
                    strWhere.AppendFormat(" and ProvinceId={0}", queryModel.ProvinceId);
                if (queryModel.CityId > 0)
                    strWhere.AppendFormat(" and CityId={0}", queryModel.CityId);
                if (!string.IsNullOrEmpty(queryModel.UnitName))
                    strWhere.AppendFormat(" and UnitName like '%{0}%'", queryModel.UnitName);
            }

            using (IDataReader dr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), strWhere.ToString(), orderbyStr))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.SupplierStructure.SupplierShopping model = new EyouSoft.Model.SupplierStructure.SupplierShopping();
                    model.Id = dr.GetInt32(dr.GetOrdinal("Id"));
                    model.ProvinceName = dr.IsDBNull(dr.GetOrdinal("ProvinceName")) ? "" : dr.GetString(dr.GetOrdinal("ProvinceName"));
                    model.CityName = dr.IsDBNull(dr.GetOrdinal("CityName")) ? "" : dr.GetString(dr.GetOrdinal("CityName"));
                    model.UnitAddress = dr.IsDBNull(dr.GetOrdinal("UnitAddress")) ? "" : dr.GetOrdinal("UnitAddress").ToString();
                    model.UnitName = dr.GetString(dr.GetOrdinal("UnitName"));
                    model.TradeNum = dr.GetInt32(dr.GetOrdinal("TradeNum"));
                    model.SupplierContact = GetContactList(dr.IsDBNull(dr.GetOrdinal("ContactXML")) ? "" : dr.GetString(dr.GetOrdinal("ContactXML")));
                    model.SaleProduct = GetShoppingList(dr.IsDBNull(dr.GetOrdinal("ShoppingXML")) ? "" : dr.GetString(dr.GetOrdinal("ShoppingXML")));
                    ls.Add(model);
                    model = null;
                }
            }

            return ls;
        }

        #endregion

        #region 私人方法
        /// <summary>
        /// 转XML格式
        /// </summary>
        /// <param name="ContactXML"></param>
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
        /// 转XML格式
        /// </summary>
        /// <param name="ContactXML"></param>
        /// <returns></returns>
        private string GetShoppingList(string ContactXML)
        {
            string saleProduct = string.Empty;
            if (string.IsNullOrEmpty(ContactXML))
                return "";
            XElement root = XElement.Parse(ContactXML);
            var xRow = root.Elements("row");
            foreach (var tmp in xRow)
            {
                saleProduct = tmp.Attribute("SaleProduct") == null ? "" : tmp.Attribute("SaleProduct").Value;
            }
            return saleProduct;
        }

        /// <summary>
        /// 构造联系人XML信息
        /// </summary>
        /// <param name="supplierContact"></param>
        /// <returns></returns>
        private string CreateContactXML(IList<EyouSoft.Model.CompanyStructure.SupplierContact> supplierContact)
        {
            if (supplierContact == null || supplierContact.Count == 0)
                return "";
            StringBuilder strXml = new StringBuilder();
            strXml.Append("<ROOT>");
            foreach (EyouSoft.Model.CompanyStructure.SupplierContact model in supplierContact)
            {
                strXml.AppendFormat("<ContactInfo ContactName=\"{0}\" JobTitle=\"{1}\" ContactFax=\"{2}\" ContactTel=\"{3}\" ContactMobile=\"{4}\" QQ=\"{5}\" Email=\"{6}\" />",
                    Utils.ReplaceXmlSpecialCharacter(model.ContactName.ToString()),
                    Utils.ReplaceXmlSpecialCharacter(model.JobTitle.ToString()),
                    Utils.ReplaceXmlSpecialCharacter(model.ContactFax.ToString()),
                    Utils.ReplaceXmlSpecialCharacter(model.ContactTel.ToString()),
                    Utils.ReplaceXmlSpecialCharacter(model.ContactMobile.ToString()),
                    Utils.ReplaceXmlSpecialCharacter(model.QQ.ToString()),
                    Utils.ReplaceXmlSpecialCharacter(model.Email.ToString()));
            }
            strXml.Append("</ROOT>");
            return strXml.ToString();
        }

        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        private string ConvertToString(params int[] Ids)
        {
            string strIds = string.Empty;
            foreach (int str in Ids)
            {
                strIds += "'" + str.ToString().Trim() + "',";
            }
            strIds = strIds.Trim(',');
            return strIds;
        }
        #endregion
    }
}
