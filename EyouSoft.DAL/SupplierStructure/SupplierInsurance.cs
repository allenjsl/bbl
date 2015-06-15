using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.SupplierStructure
{
    /// <summary>
    /// 保险供应商DAL
    /// 创建人：luofx 2011-03-8
    /// </summary>    
    public class SupplierInsurance : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.SupplierStructure.ISupplierInsurance
    {
        private readonly Database _db = null;

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public SupplierInsurance()
        {
            _db = this.SystemStore;
        }
        #endregion

        #region 接口实现方法
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">保险供应商实体</param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.SupplierStructure.SupplierInsurance model)
        {
            bool IsTrue = false;
            string SupplierInsuranceXML = CreateSupplierInsuranceXML(model.SupplierContact);
            DbCommand dc = this._db.GetStoredProcCommand("proc_SupplierInsurance_Insert");
            this._db.AddInParameter(dc, "AgreementFile", DbType.String, model.AgreementFile);
            this._db.AddInParameter(dc, "CityId", DbType.Int32, model.CityId);
            this._db.AddInParameter(dc, "CityName", DbType.String, model.CityName);
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(dc, "ProvinceId", DbType.Int32, model.ProvinceId);
            this._db.AddInParameter(dc, "ProvinceName", DbType.String, model.ProvinceName);
            this._db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(dc, "SupplierType", DbType.Byte, (int)model.SupplierType);
            this._db.AddInParameter(dc, "UnitAddress", DbType.String, model.UnitAddress);
            this._db.AddInParameter(dc, "UnitName", DbType.String, model.UnitName);                
            this._db.AddInParameter(dc, "SupplierInsuranceXML", DbType.String, SupplierInsuranceXML);
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
        /// 修改
        /// </summary>
        /// <param name="model">保险供应商实体</param>
        /// <returns></returns>
        public bool Update(EyouSoft.Model.SupplierStructure.SupplierInsurance model)
        {
            bool IsTrue = false;
            string SupplierInsuranceXML = CreateSupplierInsuranceXML(model.SupplierContact);
            DbCommand dc = this._db.GetStoredProcCommand("proc_SupplierInsurance_Update");
            this._db.AddInParameter(dc, "Id", DbType.Int32, model.Id);
            this._db.AddInParameter(dc, "AgreementFile", DbType.String, model.AgreementFile);
            this._db.AddInParameter(dc, "CityId", DbType.Int32, model.CityId);
            this._db.AddInParameter(dc, "CityName", DbType.String, model.CityName);
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(dc, "ProvinceId", DbType.Int32, model.ProvinceId);
            this._db.AddInParameter(dc, "ProvinceName", DbType.String, model.ProvinceName);
            this._db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(dc, "SupplierType", DbType.Byte, (int)model.SupplierType);
            this._db.AddInParameter(dc, "UnitAddress", DbType.String, model.UnitAddress);
            this._db.AddInParameter(dc, "UnitName", DbType.String, model.UnitName);
            this._db.AddInParameter(dc, "SupplierInsuranceXML", DbType.String, SupplierInsuranceXML);
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
        /// 获取所属保险供应商信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">搜索实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SupplierStructure.SupplierInsurance> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, EyouSoft.Model.SupplierStructure.SupplierQuery SearchInfo)
        {
            IList<EyouSoft.Model.SupplierStructure.SupplierInsurance> ResultList = null;
            StringBuilder fields = new StringBuilder();
            fields.Append(" id,ProvinceId,ProvinceName,CityId,CityName,UnitName,SupplierType,TradeNum,");
            fields.Append("UnitAddress,Commission,AgreementFile,Remark,UnitPolicy,CompanyId,OperatorId,");
            fields.Append("IssueTime,(SELECT * FROM tbl_SupplierContact WHERE SupplierId=tbl_CompanySupplier.ID ");
            fields.Append("FOR XML RAW,ROOT('ROOT')) AS SupplierInsuranceXML ");
            string TableName = "tbl_CompanySupplier";
            string orderByString = " [IssueTime] DESC";
            string identityColumnName = "id";
            StringBuilder Query = new StringBuilder();
            Query.AppendFormat(" CompanyId={0} AND IsDelete=0 AND SupplierType=8", CompanyId);
            if (!string.IsNullOrEmpty(SearchInfo.UnitName))
            {
                Query.AppendFormat(" AND UnitName LIKE '%{0}%'", SearchInfo.UnitName);
            }
            using (IDataReader dr = DbHelper.ExecuteReader(this._db, PageSize, PageIndex, ref RecordCount, TableName, identityColumnName, fields.ToString(), Query.ToString(), orderByString))
            {
                ResultList = new List<EyouSoft.Model.SupplierStructure.SupplierInsurance>();
                while (dr.Read())
                {
                    EyouSoft.Model.SupplierStructure.SupplierInsurance model = new EyouSoft.Model.SupplierStructure.SupplierInsurance()
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("id")),
                        AgreementFile = dr.IsDBNull(dr.GetOrdinal("AgreementFile")) ? "" : dr.GetString(dr.GetOrdinal("AgreementFile")),
                        CityId = dr.IsDBNull(dr.GetOrdinal("CityId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CityId")),
                        CompanyId = dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                        OperatorId = dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                        ProvinceId = dr.IsDBNull(dr.GetOrdinal("ProvinceId")) ? 0 : dr.GetInt32(dr.GetOrdinal("ProvinceId")),
                        TradeNum = dr.IsDBNull(dr.GetOrdinal("TradeNum")) ? 0 : dr.GetInt32(dr.GetOrdinal("TradeNum")),
                        UnitName = dr.IsDBNull(dr.GetOrdinal("UnitName")) ? "" : dr.GetString(dr.GetOrdinal("UnitName")),
                        CityName = dr.IsDBNull(dr.GetOrdinal("CityName")) ? "" : dr.GetString(dr.GetOrdinal("CityName")),
                        ProvinceName = dr.IsDBNull(dr.GetOrdinal("ProvinceName")) ? "" : dr.GetString(dr.GetOrdinal("ProvinceName")),
                        Remark = dr.IsDBNull(dr.GetOrdinal("Remark")) ? "" : dr.GetString(dr.GetOrdinal("Remark")),
                        UnitAddress = dr.IsDBNull(dr.GetOrdinal("UnitAddress")) ? "" : dr.GetString(dr.GetOrdinal("UnitAddress")),
                        IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime")),
                        SupplierType = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.SupplierType), dr.GetByte(dr.GetOrdinal("SupplierType")).ToString())
                    };
                    if (!dr.IsDBNull(dr.GetOrdinal("SupplierInsuranceXML")))
                    {
                        model.SupplierContact = this.GetContactInfo(dr.GetString(dr.GetOrdinal("SupplierInsuranceXML")));
                    }
                    ResultList.Add(model);
                    model = null;
                }
            }
            return ResultList;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="Id">保险供应商编号（主键）</param>
        /// <param name="CompanyId">所属公司编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SupplierStructure.SupplierInsurance GetModel(int Id, int CompanyId)
        {
            EyouSoft.Model.SupplierStructure.SupplierInsurance model = null;
            StringBuilder StrSql = new StringBuilder();
            StrSql.Append(" SELECT id,ProvinceId,ProvinceName,CityId,CityName,UnitName,SupplierType,");
            StrSql.Append(" UnitAddress,Commission,AgreementFile,Remark,UnitPolicy,CompanyId,OperatorId,");
            StrSql.Append(" IssueTime,(SELECT * FROM tbl_SupplierContact WHERE SupplierId=a.ID ");
            StrSql.Append(" FOR XML RAW,ROOT('ROOT')) AS SupplierInsuranceXML");
            StrSql.AppendFormat(" FROM tbl_CompanySupplier a WHERE CompanyId={0} AND id={1} AND SupplierType=8 ", CompanyId, Id);
            DbCommand dc = this._db.GetSqlStringCommand(StrSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                if (dr.Read())
                {
                    model = new EyouSoft.Model.SupplierStructure.SupplierInsurance()
                    {
                        Id = dr.GetInt32(dr.GetOrdinal("id")),
                        AgreementFile = dr.IsDBNull(dr.GetOrdinal("AgreementFile")) ? "" : dr.GetString(dr.GetOrdinal("AgreementFile")),
                        CityId = dr.IsDBNull(dr.GetOrdinal("CityId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CityId")),
                        CompanyId = dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId")),
                        OperatorId = dr.IsDBNull(dr.GetOrdinal("OperatorId")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorId")),
                        ProvinceId = dr.IsDBNull(dr.GetOrdinal("ProvinceId")) ? 0 : dr.GetInt32(dr.GetOrdinal("ProvinceId")),
                        //TradeNum = dr.IsDBNull(dr.GetOrdinal("TradeNum")) ? 0 : dr.GetInt32(dr.GetOrdinal("TradeNum")),
                        UnitName = dr.IsDBNull(dr.GetOrdinal("UnitName")) ? "" : dr.GetString(dr.GetOrdinal("UnitName")),
                        CityName = dr.IsDBNull(dr.GetOrdinal("CityName")) ? "" : dr.GetString(dr.GetOrdinal("CityName")),
                        ProvinceName = dr.IsDBNull(dr.GetOrdinal("ProvinceName")) ? "" : dr.GetString(dr.GetOrdinal("ProvinceName")),
                        Remark = dr.IsDBNull(dr.GetOrdinal("Remark")) ? "" : dr.GetString(dr.GetOrdinal("Remark")),
                        UnitAddress = dr.IsDBNull(dr.GetOrdinal("UnitAddress")) ? "" : dr.GetString(dr.GetOrdinal("UnitAddress")),
                        IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime")),
                        SupplierType = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.SupplierType), dr.GetByte(dr.GetOrdinal("SupplierType")).ToString())
                    };
                    if (!dr.IsDBNull(dr.GetOrdinal("SupplierInsuranceXML")))
                    {
                        model.SupplierContact = this.GetContactInfo(dr.GetString(dr.GetOrdinal("SupplierInsuranceXML")));
                    }
                }
            }
            return model;
        }

        #endregion 接口实现方法

        #region 私有方法
        /// <summary>
        /// 解析供应商联系人XML
        /// </summary>
        /// <param name="SupplierInsuranceXML">供应商联系人XML</param>
        /// <returns></returns>
        private IList<EyouSoft.Model.CompanyStructure.SupplierContact> GetContactInfo(string SupplierInsuranceXML)
        {
            IList<EyouSoft.Model.CompanyStructure.SupplierContact> ResultList = null;
            if (!string.IsNullOrEmpty(SupplierInsuranceXML))
            {
                ResultList = new List<EyouSoft.Model.CompanyStructure.SupplierContact>();
                XElement root = XElement.Parse(SupplierInsuranceXML);
                var xRow = root.Elements("row");
                foreach (var tmp1 in xRow)
                {
                    EyouSoft.Model.CompanyStructure.SupplierContact model = new EyouSoft.Model.CompanyStructure.SupplierContact()
                    {
                        CompanyId = int.Parse(tmp1.Attribute("CompanyId").Value),
                        ContactFax = tmp1.Attribute("ContactFax").Value,
                        ContactMobile = tmp1.Attribute("ContactMobile").Value,
                        ContactName = tmp1.Attribute("ContactName").Value,
                        ContactTel = tmp1.Attribute("ContactTel").Value,
                        Email = tmp1.Attribute("Email").Value,
                        JobTitle = tmp1.Attribute("JobTitle").Value,
                        QQ = tmp1.Attribute("QQ").Value,
                        SupplierId = int.Parse(tmp1.Attribute("SupplierId").Value),
                        SupplierType = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.SupplierType), tmp1.Attribute("SupplierType").Value)
                    };
                    ResultList.Add(model);
                    model = null;
                }
            }
            return ResultList;
        }
        /// <summary>
        /// 生成供应商联系人XML
        /// </summary>
        /// <param name="SupplierContact"></param>
        /// <returns></returns>
        private string CreateSupplierInsuranceXML(IList<EyouSoft.Model.CompanyStructure.SupplierContact> SupplierContact)
        {
            StringBuilder StrBuild = new StringBuilder("");
            if (SupplierContact != null && SupplierContact.Count > 0)
            {
                StrBuild.Append("<ROOT>");
                foreach (EyouSoft.Model.CompanyStructure.SupplierContact model in SupplierContact)
                {
                    StrBuild.AppendFormat("<SupplierContact id=\"{0}\" CompanyId=\"{1}\"", model.Id, model.CompanyId);
                    StrBuild.AppendFormat(" ContactFax=\"{0}\" ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(model.ContactFax));
                    StrBuild.AppendFormat(" ContactMobile=\"{0}\" ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(model.ContactMobile));
                    StrBuild.AppendFormat(" ContactName=\"{0}\" ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(model.ContactName));
                    StrBuild.AppendFormat(" ContactTel=\"{0}\" ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(model.ContactTel));
                    StrBuild.AppendFormat(" Email=\"{0}\" ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(model.Email));
                    StrBuild.AppendFormat(" JobTitle=\"{0}\" ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(model.JobTitle));
                    StrBuild.AppendFormat(" QQ=\"{0}\" ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(model.QQ));
                    StrBuild.AppendFormat(" SupplierId=\"{0}\" ", model.SupplierId);
                    StrBuild.AppendFormat(" SupplierType=\"{0}\" />", (int)model.SupplierType);
                }
                StrBuild.Append("</ROOT>");
            }
            return StrBuild.ToString();
        }

        #endregion
    }
}
