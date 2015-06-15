using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.Model.CompanyStructure;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using EyouSoft.Toolkit.DAL;
using EyouSoft.Toolkit;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 客户关系管理DAL
    /// </summary>
    /// 创建人：李焕超 2011-01-17
    public class Customer : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.ICustomer
    {
        private readonly EyouSoft.Data.EyouSoftTBL dcDal = null;
        private readonly Database _db = null;

        #region static constants
        //static constants
        const string SQL_SELECT_GetContactId = "SELECT [Id] FROM [tbl_CustomerContactInfo] WHERE [UserId]=@UID ";
        const string SQL_SELECT_GetCustomerDebt = "SELECT [MaxDebts] FROM [tbl_Customer] WHERE [Id]=@CustomerId;SELECT SUM([FinanceSum]-[HasCheckMoney]) FROM [tbl_TourOrder] WHERE [BuyCompanyId]=@CustomerId AND IsDelete='0' AND [OrderState] IN(2,5)";
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public Customer()
        {
            _db = this.SystemStore;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this.SystemStore.ConnectionString);
        }

        //客户资料方法
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsCustomer(int Id)
        {
            EyouSoft.Data.Customer c = dcDal.Customer.AsQueryable().FirstOrDefault(item => item.Id == Id);
            if (c != null)
                return true;
            return false;

        }
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int AddCustomer(EyouSoft.Model.CompanyStructure.CustomerInfo model)
        {
            if (model == null)
                return 0;

            DbCommand dc = this._db.GetStoredProcCommand("proc_Customer_Insert");
            this._db.AddInParameter(dc, "Adress", DbType.String, model.Adress);
            this._db.AddInParameter(dc, "BankAccount", DbType.String, model.BankAccount);
            this._db.AddInParameter(dc, "CityId", DbType.Int32, model.CityId);
            this._db.AddInParameter(dc, "CityName", DbType.String, model.CityName);
            this._db.AddInParameter(dc, "CommissionCount", DbType.Decimal, model.CommissionCount);
            this._db.AddInParameter(dc, "CommissionType", DbType.Int32, (int)model.CommissionType);
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(dc, "ContactName", DbType.String, model.ContactName);
            this._db.AddInParameter(dc, "CustomerLev", DbType.Int32, model.CustomerLev);
            this._db.AddInParameter(dc, "Fax", DbType.String, model.Fax);
            this._db.AddInParameter(dc, "Licence", DbType.String, model.Licence);
            this._db.AddInParameter(dc, "MaxDebts", DbType.Decimal, model.MaxDebts);
            this._db.AddInParameter(dc, "Mobile", DbType.String, model.Mobile);
            this._db.AddInParameter(dc, "Name", DbType.String, model.Name);
            this._db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(dc, "Phone", DbType.String, model.Phone);
            this._db.AddInParameter(dc, "PostalCode", DbType.String, model.PostalCode);
            this._db.AddInParameter(dc, "PreDeposit", DbType.Decimal, model.PreDeposit);
            this._db.AddInParameter(dc, "ProviceId", DbType.Int32, model.ProviceId);
            this._db.AddInParameter(dc, "ProvinceName", DbType.String, model.ProvinceName);
            this._db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(dc, "SaleId", DbType.Int32, model.SaleId);
            this._db.AddInParameter(dc, "BrandId", DbType.Int32, model.BrandId);
            this._db.AddInParameter(dc, "Saler", DbType.String, model.Saler);
            this._db.AddInParameter(dc, "CustomerInfoXML", DbType.String, CreateCustomerXML(model.CustomerContactList));
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            this._db.AddInParameter(dc, "AccountDay", DbType.Byte, model.AccountDay);
            this._db.AddInParameter(dc, "IsEnable", DbType.AnsiStringFixedLength, model.IsEnable ? "1" : "0");
            this._db.AddInParameter(dc, "AccountWay", DbType.String, model.AccountWay);
            this._db.AddInParameter(dc, "AccountDayType", DbType.Byte, model.AccountDayType);
            this._db.AddInParameter(dc, "JieSuanType", DbType.Byte, model.JieSuanType);
            this._db.AddInParameter(dc, "IsRequiredTourCode", DbType.AnsiStringFixedLength, model.IsRequiredTourCode ? "1" : "0");
            DbHelper.RunProcedure(dc, this._db);
            object ob = this._db.GetParameterValue(dc, "Result");
            return int.Parse(ob.ToString());

        }

        /// <summary>
        ///  增加多条数据
        /// </summary>
        public bool AddCustomerMore(IList<EyouSoft.Model.CompanyStructure.CustomerInfo> modelList)
        {
            foreach (EyouSoft.Model.CompanyStructure.CustomerInfo model in modelList)
            {
                AddCustomer(model);
            }
            return true;
        }

        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int UpdateCustomer(EyouSoft.Model.CompanyStructure.CustomerInfo model)
        {
            DbCommand dc = this._db.GetStoredProcCommand("proc_Customer_Update");
            this._db.AddInParameter(dc, "CustomerId", DbType.Int32, model.Id);
            this._db.AddInParameter(dc, "Adress", DbType.String, model.Adress);
            this._db.AddInParameter(dc, "BankAccount", DbType.String, model.BankAccount);
            this._db.AddInParameter(dc, "CityId", DbType.Int32, model.CityId);
            this._db.AddInParameter(dc, "CityName", DbType.String, model.CityName);
            this._db.AddInParameter(dc, "CommissionCount", DbType.Decimal, model.CommissionCount);
            this._db.AddInParameter(dc, "CommissionType", DbType.Int32, (int)model.CommissionType);
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(dc, "ContactName", DbType.String, model.ContactName);
            this._db.AddInParameter(dc, "CustomerLev", DbType.Int32, model.CustomerLev);
            this._db.AddInParameter(dc, "Fax", DbType.String, model.Fax);
            this._db.AddInParameter(dc, "Licence", DbType.String, model.Licence);
            this._db.AddInParameter(dc, "MaxDebts", DbType.Decimal, model.MaxDebts);
            this._db.AddInParameter(dc, "Mobile", DbType.String, model.Mobile);
            this._db.AddInParameter(dc, "Name", DbType.String, model.Name);
            this._db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(dc, "Phone", DbType.String, model.Phone);
            this._db.AddInParameter(dc, "PostalCode", DbType.String, model.PostalCode);
            this._db.AddInParameter(dc, "PreDeposit", DbType.Decimal, model.PreDeposit);
            this._db.AddInParameter(dc, "ProviceId", DbType.Int32, model.ProviceId);
            this._db.AddInParameter(dc, "ProvinceName", DbType.String, model.ProvinceName);
            this._db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(dc, "SaleId", DbType.Int32, model.SaleId);
            this._db.AddInParameter(dc, "BrandId", DbType.Int32, model.BrandId);
            this._db.AddInParameter(dc, "Saler", DbType.String, model.Saler);
            this._db.AddInParameter(dc, "IsEnable", DbType.String, model.IsEnable ? 1 : 0);
            this._db.AddInParameter(dc, "IsDelete", DbType.String, model.IsDelete ? 1 : 0);
            this._db.AddInParameter(dc, "IssueTime", DbType.DateTime, model.IssueTime);
            this._db.AddInParameter(dc, "CustomerInfoXML", DbType.String, CreateCustomerXML(model.CustomerContactList));
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            this._db.AddInParameter(dc, "AccountDay", DbType.Byte, model.AccountDay);
            this._db.AddInParameter(dc, "AccountWay", DbType.String, model.AccountWay);
            this._db.AddInParameter(dc, "AccountDayType", DbType.Byte, model.AccountDayType);
            this._db.AddInParameter(dc, "JieSuanType", DbType.Byte, model.JieSuanType);
            this._db.AddInParameter(dc, "IsRequiredTourCode", DbType.AnsiStringFixedLength, model.IsRequiredTourCode ? "1" : "0");
            DbHelper.RunProcedure(dc, this._db);
            object ob = this._db.GetParameterValue(dc, "Result");
            return int.Parse(ob.ToString());

        }

        /// <summary>
        ///  更新一条数据
        /// </summary>
        public bool UpdateSampleCustomer(EyouSoft.Model.CompanyStructure.CustomerInfo model)
        {
            StringBuilder sql = new StringBuilder("UPDATE tbl_Customer SET [ProviceId] = ProviceId,[ProvinceName] = @ProvinceName,[CityId] = @CityId,[CityName] = @CityName ,[Name] = @Name");
            sql.Append(" ,[Licence] = @Licence ,[Adress] = @Adress,[PostalCode] = @PostalCode,[BankAccount] = @BankAccount");
            sql.Append(" ,[CommissionCount] = @CommissionCount,[ContactName] = @ContactName,[Phone] = @Phone,[Mobile] = @Mobile,[Fax] = @Fax,[Remark] = @Remark");
            sql.Append(" WHERE  id=@CustomerId");


            DbCommand dc = this._db.GetSqlStringCommand(sql.ToString());
            this._db.AddInParameter(dc, "CustomerId", DbType.Int32, model.Id);
            this._db.AddInParameter(dc, "Adress", DbType.String, model.Adress);
            this._db.AddInParameter(dc, "BankAccount", DbType.String, model.BankAccount);
            this._db.AddInParameter(dc, "CityId", DbType.Int32, model.CityId);
            this._db.AddInParameter(dc, "CityName", DbType.String, model.CityName);
            this._db.AddInParameter(dc, "CommissionCount", DbType.Decimal, model.CommissionCount);
            this._db.AddInParameter(dc, "ContactName", DbType.String, model.ContactName);
            this._db.AddInParameter(dc, "Fax", DbType.String, model.Fax);
            this._db.AddInParameter(dc, "Licence", DbType.String, model.Licence);
            this._db.AddInParameter(dc, "Mobile", DbType.String, model.Mobile);
            this._db.AddInParameter(dc, "Name", DbType.String, model.Name);
            this._db.AddInParameter(dc, "Phone", DbType.String, model.Phone);
            this._db.AddInParameter(dc, "PostalCode", DbType.String, model.PostalCode);
            this._db.AddInParameter(dc, "ProviceId", DbType.Int32, model.ProviceId);
            this._db.AddInParameter(dc, "ProvinceName", DbType.String, model.ProvinceName);
            this._db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            return DbHelper.ExecuteSql(dc, this._db) > 0 ? true : false;

        }

        /// <summary>
        ///  更新组团端客户资料配置管理数据
        /// </summary>
        public bool UpdateSampleCustomerConfig(EyouSoft.Model.CompanyStructure.CustomerConfig model)
        {
            StringBuilder sql = new StringBuilder("UPDATE tbl_Customer SET  CustomerStamp=@CustomerStamp, PageFootFile=@PageFootFile, PageHeadFile=@PageHeadFile, TemplateFile=@TemplateFile,FilePathLogo=@FilePathLogo");
            sql.Append(" WHERE  id=@CustomerId");
            DbCommand dc = this._db.GetSqlStringCommand(sql.ToString());
            this._db.AddInParameter(dc, "CustomerId", DbType.Int32, model.Id);
            this._db.AddInParameter(dc, "CustomerStamp", DbType.String, model.CustomerStamp);
            this._db.AddInParameter(dc, "PageFootFile", DbType.String, model.PageFootFile);
            this._db.AddInParameter(dc, "PageHeadFile", DbType.String, model.PageHeadFile);
            this._db.AddInParameter(dc, "TemplateFile", DbType.String, model.TemplateFile);
            this._db.AddInParameter(dc, "FilePathLogo", DbType.String, model.FilePathLogo);
            return DbHelper.ExecuteSql(dc, this._db) > 0 ? true : false;

        }

        /// <summary>
        /// 更新当前登录游客的电话和手机
        /// </summary>
        /// <param name="Model">只赋值编号，电话和手机</param>
        /// <returns></returns>
        public bool UpDateSampleCompanyUserInfo(EyouSoft.Model.CompanyStructure.CompanyUser Model)
        {
            if (Model == null || Model.PersonInfo == null)
                return false;
            DbCommand cmd = this._db.GetStoredProcCommand("UpdateCompanyUserInfo");
            this._db.AddInParameter(cmd, "UserId", DbType.Int32, Model.ID);
            this._db.AddInParameter(cmd, "Tel", DbType.String, Model.PersonInfo.ContactTel);
            this._db.AddInParameter(cmd, "Mobile", DbType.String, Model.PersonInfo.ContactMobile);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(cmd, this._db);
            object o = this._db.GetParameterValue(cmd, "Result");
            return int.Parse(o.ToString()) > 0 ? true : false;

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteCustomer(int Id)
        {
            string sql = "update tbl_customer set IsDelete=1 where id=@customerId;update tbl_companyuser set isdelete=1 where tourcompanyid=@customerId ";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "customerId", DbType.Int32, Id);
            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public EyouSoft.Model.CompanyStructure.CustomerInfo GetCustomerModel(int Id)
        {
            string sql = " select * from tbl_customer where id=@customerId  select * from tbl_CompanyBrands where id in(select BrandId from tbl_customer where id=@customerId)";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "customerId", DbType.Int32, Id);
            EyouSoft.Model.CompanyStructure.CustomerInfo customerModel = null;
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    customerModel = new CustomerInfo();
                    customerModel.Adress = rdr.IsDBNull(rdr.GetOrdinal("Adress")) ? "" : rdr["Adress"].ToString();
                    customerModel.BankAccount = rdr.IsDBNull(rdr.GetOrdinal("BankAccount")) ? "" : rdr["BankAccount"].ToString();
                    customerModel.CityId = rdr.GetInt32(rdr.GetOrdinal("CityId"));
                    customerModel.CityName = rdr.IsDBNull(rdr.GetOrdinal("CityName")) ? "" : rdr["CityName"].ToString();
                    customerModel.CommissionCount = rdr.GetDecimal(rdr.GetOrdinal("CommissionCount"));
                    customerModel.CommissionType = rdr.IsDBNull(rdr.GetOrdinal("CommissionType")) ? (EyouSoft.Model.EnumType.CompanyStructure.CommissionType.未确定) : (EyouSoft.Model.EnumType.CompanyStructure.CommissionType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.CommissionType), rdr.GetByte(rdr.GetOrdinal("CommissionType")).ToString());// rdr.GetOrdinal("CommissionType");
                    customerModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    customerModel.ComplaintNum = rdr.GetInt32(rdr.GetOrdinal("ComplaintNum"));
                    customerModel.ContactName = rdr.IsDBNull(rdr.GetOrdinal("ContactName")) ? "" : rdr["ContactName"].ToString();
                    customerModel.CustomerLev = rdr.GetInt32(rdr.GetOrdinal("CustomerLev"));
                    customerModel.CustomerStamp = rdr.IsDBNull(rdr.GetOrdinal("CustomerStamp")) ? "" : rdr["CustomerStamp"].ToString();
                    customerModel.Fax = rdr.IsDBNull(rdr.GetOrdinal("Fax")) ? "" : rdr["Fax"].ToString();
                    customerModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    customerModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    customerModel.Licence = rdr.IsDBNull(rdr.GetOrdinal("Licence")) ? "" : rdr["Licence"].ToString();
                    customerModel.MaxDebts = rdr.GetDecimal(rdr.GetOrdinal("MaxDebts"));
                    customerModel.Mobile = rdr.IsDBNull(rdr.GetOrdinal("Mobile")) ? "" : rdr["Mobile"].ToString();
                    customerModel.Name = rdr.IsDBNull(rdr.GetOrdinal("Name")) ? "" : rdr["Name"].ToString();
                    customerModel.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    customerModel.PageFootFile = rdr.IsDBNull(rdr.GetOrdinal("PageFootFile")) ? "" : rdr["PageFootFile"].ToString();
                    customerModel.PageHeadFile = rdr.IsDBNull(rdr.GetOrdinal("PageHeadFile")) ? "" : rdr["PageHeadFile"].ToString();
                    customerModel.Phone = rdr.IsDBNull(rdr.GetOrdinal("Phone")) ? "" : rdr["Phone"].ToString();
                    customerModel.PostalCode = rdr.IsDBNull(rdr.GetOrdinal("PostalCode")) ? "" : rdr["PostalCode"].ToString();
                    customerModel.PreDeposit = rdr.GetDecimal(rdr.GetOrdinal("PreDeposit"));
                    customerModel.ProviceId = rdr.GetInt32(rdr.GetOrdinal("ProviceId"));
                    customerModel.ProvinceName = rdr.IsDBNull(rdr.GetOrdinal("ProvinceName")) ? "" : rdr["ProvinceName"].ToString();
                    customerModel.Remark = rdr.IsDBNull(rdr.GetOrdinal("Remark")) ? "" : rdr["Remark"].ToString();
                    customerModel.SaleId = rdr.GetInt32(rdr.GetOrdinal("SaleId"));
                    customerModel.Saler = rdr.IsDBNull(rdr.GetOrdinal("Saler")) ? "" : rdr["Saler"].ToString();
                    customerModel.SatNum = rdr.GetDecimal(rdr.GetOrdinal("SatNum"));
                    customerModel.TemplateFile = rdr.IsDBNull(rdr.GetOrdinal("TemplateFile")) ? "" : rdr["TemplateFile"].ToString();
                    customerModel.TradeNum = rdr.GetInt32(rdr.GetOrdinal("TradeNum"));
                    customerModel.IsEnable = rdr.GetString(rdr.GetOrdinal("IsEnable")) == "1" ? true : false;
                    customerModel.BrandId = rdr.GetInt32(rdr.GetOrdinal("BrandId"));
                    customerModel.AccountDay = rdr.GetByte(rdr.GetOrdinal("AccountDay"));
                    customerModel.AccountWay = rdr["AccountWay"].ToString();
                    customerModel.AccountDayType = (EyouSoft.Model.EnumType.CompanyStructure.AccountDayType)rdr.GetByte(rdr.GetOrdinal("AccountDayType"));
                    customerModel.FilePathLogo = rdr["FilePathLogo"].ToString();
                    customerModel.JieSuanType = (EyouSoft.Model.EnumType.CompanyStructure.KHJieSuanType)rdr.GetByte(rdr.GetOrdinal("JieSuanType"));
                    customerModel.IsRequiredTourCode = GetBoolean(rdr.GetString(rdr.GetOrdinal("IsRequiredTourCode")));

                    customerModel.CustomerContactList = GetCustomerContactList(Id);
                    if (rdr.NextResult())
                    {
                        EyouSoft.Model.CompanyStructure.CompanyBrand comModel = null;
                        if (rdr.Read())
                        {
                            comModel = new EyouSoft.Model.CompanyStructure.CompanyBrand();

                            comModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                            comModel.BrandName = rdr.IsDBNull(rdr.GetOrdinal("BrandName")) ? "" : rdr["BrandName"].ToString();
                            comModel.Logo1 = rdr.IsDBNull(rdr.GetOrdinal("Logo1")) ? "" : rdr.GetString(rdr.GetOrdinal("Logo1"));
                            comModel.Logo2 = rdr.IsDBNull(rdr.GetOrdinal("Logo2")) ? "" : rdr.GetString(rdr.GetOrdinal("Logo2"));
                            comModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                            comModel.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                            comModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                            comModel.IsDelete = Convert.ToBoolean(rdr.GetOrdinal("IsDelete"));

                        }
                        customerModel.CompanyBrand = comModel;
                    }

                }
            }
            return customerModel;
        }

        /// <summary>
        /// 得到客户的配置信息对象实体
        /// </summary>
        public EyouSoft.Model.CompanyStructure.CustomerConfig GetCustomerConfigModel(int Id)
        {
            string sql = " select * from tbl_customer where id=@customerId ";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "customerId", DbType.Int32, Id);
            EyouSoft.Model.CompanyStructure.CustomerConfig customerModel = null;
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    customerModel = new CustomerConfig();
                    customerModel.Id = rdr.GetInt32(rdr.GetOrdinal("id"));
                    customerModel.PageFootFile = rdr.IsDBNull(rdr.GetOrdinal("PageFootFile")) ? "" : rdr["PageFootFile"].ToString();
                    customerModel.PageHeadFile = rdr.IsDBNull(rdr.GetOrdinal("PageHeadFile")) ? "" : rdr["PageHeadFile"].ToString();
                    customerModel.CustomerStamp = rdr.IsDBNull(rdr.GetOrdinal("CustomerStamp")) ? "" : rdr["CustomerStamp"].ToString();
                    customerModel.TemplateFile = rdr.IsDBNull(rdr.GetOrdinal("TemplateFile")) ? "" : rdr["TemplateFile"].ToString();
                    customerModel.FilePathLogo = rdr["FilePathLogo"].ToString();
                }
            }
            return customerModel;
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public bool StopIt(int CustomerId)
        {
            StringBuilder sql = new StringBuilder(" update tbl_Customer set isEnable=0 where id=@CustomerId  ");
            sql.Append(" update tbl_companyuser set isenable=0 where tourcompanyid=@CustomerId");

            DbCommand cmd = this._db.GetSqlStringCommand(sql.ToString());
            this._db.AddInParameter(cmd, "CustomerId", DbType.Int32, CustomerId);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;

        }

        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public bool StartId(int CustomerId)
        {
            StringBuilder sql = new StringBuilder(" update tbl_Customer set isEnable=1 where id=@CustomerId ");
            sql.Append(" update tbl_companyuser set isenable=1 where tourcompanyid=@CustomerId");
            DbCommand cmd = this._db.GetSqlStringCommand(sql.ToString());
            this._db.AddInParameter(cmd, "CustomerId", DbType.Int32, CustomerId);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }
        #endregion  成员方法

        //客户联系人方法
        #region  成员方法

        // /// <summary>
        // /// 是否存在该记录
        // /// </summary>
        public bool ExistsCustomerContact(int Id)
        {
            EyouSoft.Data.CustomerContactInfo c = dcDal.CustomerContactInfo.FirstOrDefault(item => item.Id == Id);
            if (c != null)
                return true;
            return false;
        }

        /// <summary>
        /// 删除某客户的所有联系人
        /// </summary>
        public bool DeleteCustomerContact(int CustomerId)
        {
            string sql = " delete from tbl_CustomerContact  where CustomerId=@CustomerId ";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "CustomerId", DbType.Int32, CustomerId);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 删除联系人
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool DeleteCustomerContacter(params string[] Ids)
        {
            string sql = "UPDATE tbl_CompanyUser SET IsDelete='1' WHERE [Id] IN(SELECT [UserId] FROM tbl_CustomerContactInfo WHERE Id IN({0}) AND [UserId]>0) ;delete from tbl_CustomerContactInfo where id in({0})";
            StringBuilder str = new StringBuilder("");
            if (Ids != null && Ids.Count() > 0)
            {
                foreach (string s in Ids)
                {
                    if (!string.IsNullOrEmpty(s))
                        str.AppendFormat("{0},", s);
                    //  DeleteCustomerContact(int.Parse(s)); 
                }
                string temp = string.Format(sql, str.ToString().TrimEnd(','));
                return DbHelper.ExecuteSql(this._db.GetSqlStringCommand(temp), this._db) > 0 ? true : false;
            }
            return false;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public EyouSoft.Model.CompanyStructure.CustomerContactInfo GetCustomerContactModel(int Id)
        {
            string sql = "select * from  tbl_CustomerContactInfo where id=@id ";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "id", DbType.Int32, Id);
            CustomerContactInfo model = null;
            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rd.Read())
                {
                    model = new CustomerContactInfo();
                    model.BirthDay = rd.IsDBNull(rd.GetOrdinal("BirthDay")) ? DateTime.Parse("2011-01-30") : rd.GetDateTime(rd.GetOrdinal("BirthDay"));
                    model.CustomerId = rd.IsDBNull(rd.GetOrdinal("CustomerId")) ? 0 : rd.GetInt32(rd.GetOrdinal("CustomerId"));
                    model.Department = rd.IsDBNull(rd.GetOrdinal("DepartmentId")) ? "0" : rd.GetString(rd.GetOrdinal("DepartmentId"));
                    model.Email = rd.IsDBNull(rd.GetOrdinal("Email")) ? "" : rd.GetString(rd.GetOrdinal("Email"));
                    model.Hobby = rd.IsDBNull(rd.GetOrdinal("Hobby")) ? "" : rd.GetString(rd.GetOrdinal("Hobby"));
                    model.ID = rd.IsDBNull(rd.GetOrdinal("ID")) ? 0 : rd.GetInt32(rd.GetOrdinal("ID"));
                    model.Job = rd.IsDBNull(rd.GetOrdinal("JobId")) ? "0" : rd.GetString(rd.GetOrdinal("JobId"));
                    model.Mobile = rd.IsDBNull(rd.GetOrdinal("Mobile")) ? "" : rd.GetString(rd.GetOrdinal("Mobile"));
                    model.Name = rd.IsDBNull(rd.GetOrdinal("Name")) ? "" : rd.GetString(rd.GetOrdinal("Name"));
                    model.Remark = rd.IsDBNull(rd.GetOrdinal("Remark")) ? "" : rd.GetString(rd.GetOrdinal("Remark"));
                    model.Sex = rd.IsDBNull(rd.GetOrdinal("Sex")) ? "" : rd.GetString(rd.GetOrdinal("Sex")) == "0" ? "男" : "女";
                    model.Spetialty = rd.IsDBNull(rd.GetOrdinal("Spetialty")) ? "" : rd.GetString(rd.GetOrdinal("Spetialty"));
                    model.Tel = rd.IsDBNull(rd.GetOrdinal("Tel")) ? "" : rd.GetString(rd.GetOrdinal("Tel"));
                    model.qq = rd.IsDBNull(rd.GetOrdinal("qq")) ? "" : rd.GetString(rd.GetOrdinal("qq"));
                    model.UserId = rd.IsDBNull(rd.GetOrdinal("UserId")) ? 0 : rd.GetInt32(rd.GetOrdinal("UserId"));
                    model.Fax = rd.IsDBNull(rd.GetOrdinal("Fax")) ? string.Empty : rd.GetString(rd.GetOrdinal("Fax"));
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public IList<EyouSoft.Model.CompanyStructure.CustomerContactInfo> GetCustomerContactList(int CustomerId)
        {
            string sql = "select a.*,b.username,b.Password,b.MD5Password  from  tbl_CustomerContactInfo  a left join tbl_companyuser b on a.userid=b.id  where a.CustomerId= @CustomerId ";
            List<EyouSoft.Model.CompanyStructure.CustomerContactInfo> CustomerContactList = new List<EyouSoft.Model.CompanyStructure.CustomerContactInfo>();
            EyouSoft.Model.CompanyStructure.CustomerContactInfo model = null;
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "CustomerId", DbType.Int32, CustomerId);
            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {
                //int i = 0;
                while (rd.Read())
                {
                    model = new CustomerContactInfo();
                    model.BirthDay = rd.IsDBNull(rd.GetOrdinal("BirthDay")) ? DateTime.Parse("2011-01-30") : rd.GetDateTime(rd.GetOrdinal("BirthDay"));
                    model.CustomerId = rd.IsDBNull(rd.GetOrdinal("CustomerId")) ? 0 : rd.GetInt32(rd.GetOrdinal("CustomerId"));
                    model.Department = rd.IsDBNull(rd.GetOrdinal("DepartmentId")) ? "0" : rd.GetString(rd.GetOrdinal("DepartmentId"));
                    model.Email = rd.IsDBNull(rd.GetOrdinal("Email")) ? "" : rd.GetString(rd.GetOrdinal("Email"));
                    model.Hobby = rd.IsDBNull(rd.GetOrdinal("Hobby")) ? "" : rd.GetString(rd.GetOrdinal("Hobby"));
                    model.ID = rd.IsDBNull(rd.GetOrdinal("ID")) ? 0 : rd.GetInt32(rd.GetOrdinal("ID"));
                    model.Job = rd.IsDBNull(rd.GetOrdinal("JobId")) ? "0" : rd.GetString(rd.GetOrdinal("JobId"));
                    model.Mobile = rd.IsDBNull(rd.GetOrdinal("Mobile")) ? "" : rd.GetString(rd.GetOrdinal("Mobile"));
                    model.Name = rd.IsDBNull(rd.GetOrdinal("Name")) ? "" : rd.GetString(rd.GetOrdinal("Name"));
                    model.Remark = rd.IsDBNull(rd.GetOrdinal("Remark")) ? "" : rd.GetString(rd.GetOrdinal("Remark"));
                    model.Sex = rd.IsDBNull(rd.GetOrdinal("Sex")) ? "" : rd.GetString(rd.GetOrdinal("Sex")) == "0" ? "男" : "女";
                    model.Spetialty = rd.IsDBNull(rd.GetOrdinal("Spetialty")) ? "" : rd.GetString(rd.GetOrdinal("Spetialty"));
                    model.Tel = rd.IsDBNull(rd.GetOrdinal("Tel")) ? "" : rd.GetString(rd.GetOrdinal("Tel"));
                    model.qq = rd.IsDBNull(rd.GetOrdinal("qq")) ? "" : rd.GetString(rd.GetOrdinal("qq"));
                    model.UserId = rd.IsDBNull(rd.GetOrdinal("UserId")) ? 0 : rd.GetInt32(rd.GetOrdinal("UserId"));
                    model.Fax = rd.IsDBNull(rd.GetOrdinal("Fax")) ? string.Empty : rd.GetString(rd.GetOrdinal("Fax"));
                    model.AreaIds = "";
                    if (model.UserId != 0)
                    {
                        //获取用户名密码
                        EyouSoft.Model.CompanyStructure.UserAccount userAccout = new UserAccount();
                        userAccout.UserName = rd.IsDBNull(rd.GetOrdinal("username")) ? "" : rd.GetString(rd.GetOrdinal("username"));
                        userAccout.PassWordInfo = new EyouSoft.Model.CompanyStructure.PassWord() { NoEncryptPassword = rd.IsDBNull(rd.GetOrdinal("Password")) ? "" : rd.GetString(rd.GetOrdinal("Password")) };
                        model.UserAccount = userAccout;
                        //获取区域
                        model.AreaIds = GetAreaList(model.UserId)[0];
                        model.AreaNames = GetAreaList(model.UserId)[1];

                    }
                    //i++;
                    CustomerContactList.Add(model);
                }
            }
            return CustomerContactList;
        }


        #endregion  成员方法

        //客户回访方法
        #region  成员方法
        /// <summary>
        ///  增加一条数据
        /// </summary>
        public bool AddCustomerCallBack(EyouSoft.Model.CompanyStructure.CustomerCallBackInfo model)
        {
            if (model == null)
                return false;
            try
            {
                DbCommand dc = this._db.GetStoredProcCommand("proc_CustomerCallBack_Add");
                this._db.AddInParameter(dc, "CallBacker", DbType.String, model.CallBacker);
                this._db.AddInParameter(dc, "CallBackerId", DbType.Int32, model.CallBackerId);
                this._db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
                this._db.AddInParameter(dc, "CustomerId", DbType.Int32, model.CustomerId);
                this._db.AddInParameter(dc, "CustomerName", DbType.String, model.CustomerName);
                this._db.AddInParameter(dc, "CustomerUser", DbType.String, model.CustomerUser);
                this._db.AddInParameter(dc, "IsCallBack", DbType.Byte, (byte)model.IsCallBack);
                this._db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
                double r = 0;
                if (model.CustomerCallBackResultInfoList != null && model.CustomerCallBackResultInfoList.Count > 0)
                {
                    if (model.CustomerCallBackResultInfoList[0].Car == 1)
                        r += 1;
                    if (model.CustomerCallBackResultInfoList[0].Guide == 1)
                        r += 1;
                    if (model.CustomerCallBackResultInfoList[0].Hotel == 1)
                        r += 1;
                    if (model.CustomerCallBackResultInfoList[0].Journey == 1)
                        r += 1;
                    if (model.CustomerCallBackResultInfoList[0].meals == 1)
                        r += 1;
                    if (model.CustomerCallBackResultInfoList[0].Shopping == 1)
                        r += 1;
                    if (model.CustomerCallBackResultInfoList[0].Spot == 1)
                        r += 1;

                    r = double.Parse((r / 7).ToString("00.00"));
                }
                this._db.AddInParameter(dc, "CallResult", DbType.Double, r);
                this._db.AddInParameter(dc, "CallBackResultXML", DbType.String, CreateCustomerCallResultXML(model.CustomerCallBackResultInfoList));
                this._db.AddOutParameter(dc, "ZResult", DbType.Int32, 4);
                DbHelper.RunProcedure(dc, this._db);
                object ob = this._db.GetParameterValue(dc, "ZResult");
                return int.Parse(ob.ToString()) > 1 ? false : true;
            }
            catch { return false; }
        }

        /// <summary>
        ///  更新一条数据
        /// </summary>
        public bool UpdateCustomerCallBack(EyouSoft.Model.CompanyStructure.CustomerCallBackInfo model)
        {
            if (model == null)
                return false;

            string sql = " update tbl_CustomerCallBack set CallBacker=@CallBacker, CallBackerId=@CallBackerId, CompanyId=@CompanyId, CustomerId=@CustomerId, CustomerName=@CustomerName, CustomerUser=@CustomerUser, IsCallBack=@IsCallBack, Remark=@Remark,Time=@Time where id=@id";
            DbCommand dc = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(dc, "id", DbType.Int32, model.ID);
            this._db.AddInParameter(dc, "CallBacker", DbType.String, model.CallBacker);
            this._db.AddInParameter(dc, "CallBackerId", DbType.Int32, model.CallBackerId);
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(dc, "CustomerId", DbType.Int32, model.CustomerId);
            this._db.AddInParameter(dc, "CustomerName", DbType.String, model.CustomerName);
            this._db.AddInParameter(dc, "CustomerUser", DbType.String, model.CustomerUser);
            this._db.AddInParameter(dc, "IsCallBack", DbType.Byte, (byte)model.IsCallBack);

            if (string.IsNullOrEmpty(model.Time.ToString()))
                model.Time = DateTime.Now;
            this._db.AddInParameter(dc, "Time", DbType.DateTime, model.Time);
            this._db.AddInParameter(dc, "Remark", DbType.String, model.Remark);

            return DbHelper.ExecuteSql(dc, this._db) > 0 ? true : false;

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteCustomerCallBack(int Id)
        {

            DbCommand cmd = this._db.GetStoredProcCommand("proc_CustomerCallBack_Delete");
            this._db.AddInParameter(cmd, "id", DbType.Int32, Id);
            this._db.AddOutParameter(cmd, "result", DbType.Int32, 4);
            DbHelper.RunProcedure(cmd, this._db);
            object o = this._db.GetParameterValue(cmd, "result");
            return int.Parse(o.ToString()) > 0 ? true : false;
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        public bool DeleteCustomerCallBackMore(params string[] Ids)
        {
            bool r = true;
            string strIds = string.Empty;
            foreach (string id in Ids)
            {
                r = r & DeleteCustomerCallBack(int.Parse(id));
                //strIds += id + ",";
            }
            //   DbCommand cmd = this._db.GetSqlStringCommand(string.Format("delete from tbl_CustomerCallBackResult where CustomerCareforId in({0})  delete from tbl_CustomerCallBack where id in({0})  update tbl_customer set ComplaintNum=ComplaintNum - 1 where id in ({0})", strIds.TrimEnd(',')));
            // return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
            return r;

        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public EyouSoft.Model.CompanyStructure.CustomerCallBackInfo GetCustomerCallBackModel(int Id)
        {
            string sql = " select * from tbl_CustomerCallBack where id=@id";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "id", DbType.Int32, Id);
            CustomerCallBackInfo model = null;
            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rd.Read())
                {
                    model = new CustomerCallBackInfo();
                    model.ID = rd.GetInt32(rd.GetOrdinal("ID"));
                    model.CallBacker = rd.IsDBNull(rd.GetOrdinal("CallBacker")) ? "" : rd.GetString(rd.GetOrdinal("CallBacker"));
                    model.CallBackerId = rd.GetInt32(rd.GetOrdinal("CallBackerId"));
                    model.CompanyId = rd.GetInt32(rd.GetOrdinal("CompanyId"));
                    model.CustomerId = rd.GetInt32(rd.GetOrdinal("CustomerId"));
                    model.CustomerName = rd.IsDBNull(rd.GetOrdinal("CustomerName")) ? "" : rd.GetString(rd.GetOrdinal("CustomerName"));
                    model.CustomerUser = rd.IsDBNull(rd.GetOrdinal("CustomerUser")) ? "" : rd.GetString(rd.GetOrdinal("CustomerUser"));
                    model.IsCallBack = rd.GetString(rd.GetOrdinal("IsCallBack")) == "0" ? EyouSoft.Model.EnumType.CompanyStructure.CallBackType.回访 : (EyouSoft.Model.EnumType.CompanyStructure.CallBackType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.CallBackType), rd.GetString(rd.GetOrdinal("IsCallBack")).ToString());
                    model.Remark = rd.IsDBNull(rd.GetOrdinal("Remark")) ? "" : rd.GetString(rd.GetOrdinal("Remark"));
                    model.Result = rd.GetDecimal(rd.GetOrdinal("Result"));
                    model.Time = rd.IsDBNull(rd.GetOrdinal("Time")) ? DateTime.Parse("2000-01-01") : rd.GetDateTime(rd.GetOrdinal("Time"));
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public IList<EyouSoft.Model.CompanyStructure.CustomerCallBackInfo> GetCustomerCallBackList(EyouSoft.Model.EnumType.CompanyStructure.CallBackType CallType, int PageSize, int PageIndex, int CompanyId, string strWhere, string filedOrder, ref int RecordCount, ref int PageCount)
        {
            List<EyouSoft.Model.CompanyStructure.CustomerCallBackInfo> CallBackInfoList = new List<EyouSoft.Model.CompanyStructure.CustomerCallBackInfo>();
            string cmdQuery = string.Format("IsCallBack={2} and Companyid={0} {1}", CompanyId, strWhere, (int)CallType);
            string tableName = "tbl_CustomerCallBack";
            string primaryKey = "Id";
            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, tableName, primaryKey, filedOrder, cmdQuery, " RegTime desc"))
            {
                EyouSoft.Model.CompanyStructure.CustomerCallBackInfo Model;
                while (rdr.Read())
                {
                    Model = new CustomerCallBackInfo();
                    Model.CallBacker = rdr["CallBacker"].ToString();
                    Model.CallBackerId = rdr.GetInt32(rdr.GetOrdinal("CallBackerId"));
                    Model.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    Model.CustomerId = rdr.GetInt32(rdr.GetOrdinal("CustomerId"));
                    Model.CustomerName = rdr["CustomerName"].ToString();
                    Model.CustomerUser = rdr["CustomerUser"].ToString();
                    Model.ID = rdr.GetInt32(rdr.GetOrdinal("ID"));
                    Model.IsCallBack = (EyouSoft.Model.EnumType.CompanyStructure.CallBackType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.CallBackType), rdr["IsCallBack"].ToString());
                    Model.Remark = rdr["Remark"].ToString();
                    Model.Result = rdr.GetDecimal(rdr.GetOrdinal("Result"));
                    Model.Time = rdr.GetDateTime(rdr.GetOrdinal("Time"));
                    CallBackInfoList.Add(Model);

                }
            }
            PageCount = RecordCount / PageSize + 1;
            return CallBackInfoList;
        }


        #endregion  成员方法

        //客户回访结果方法
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsCustomerCallbackResult(int Id)
        { return false; }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public bool UpdateCustomerCallbackResult(EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo model)
        {
            if (model != null)
            {
                DbCommand cmd = this._db.GetStoredProcCommand("proc_CustomerCallResult_Update");
                this._db.AddInParameter(cmd, "id", DbType.Int32, model.ID);
                this._db.AddInParameter(cmd, "Car", DbType.Byte, model.Car);
                this._db.AddInParameter(cmd, "CustomerCareforId", DbType.Int32, model.CustomerCareforId);
                this._db.AddInParameter(cmd, "DepartureTime", DbType.DateTime, model.DepartureTime);
                this._db.AddInParameter(cmd, "Guide", DbType.Byte, model.Guide);
                this._db.AddInParameter(cmd, "Hotel", DbType.Byte, model.Hotel);
                this._db.AddInParameter(cmd, "Journey", DbType.Byte, model.Journey);
                this._db.AddInParameter(cmd, "meals", DbType.Byte, model.meals);
                this._db.AddInParameter(cmd, "Remark", DbType.String, model.Remark);
                this._db.AddInParameter(cmd, "RouteID", DbType.Int32, model.RouteID);
                this._db.AddInParameter(cmd, "RouteName", DbType.String, model.RouteName);
                this._db.AddInParameter(cmd, "Shopping", DbType.Byte, model.Shopping);
                this._db.AddInParameter(cmd, "Spot", DbType.Byte, model.Spot);
                double r = 0;

                if (model.Car == 1)
                    r += 1;
                if (model.Guide == 1)
                    r += 1;
                if (model.Hotel == 1)
                    r += 1;
                if (model.Journey == 1)
                    r += 1;
                if (model.meals == 1)
                    r += 1;
                if (model.Shopping == 1)
                    r += 1;
                if (model.Spot == 1)
                    r += 1;

                r = double.Parse((r / 7).ToString("00.00"));

                this._db.AddInParameter(cmd, "CallResult", DbType.Decimal, r);
                this._db.AddOutParameter(cmd, "Zresult", DbType.Int32, 4);
                DbHelper.RunProcedure(cmd, this._db);
                object o = this._db.GetParameterValue(cmd, "Zresult");
                return int.Parse(o.ToString()) > 0 ? true : false;
            }
            return false;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteCustomerCallbackResult(int CareForId)
        {

            IEnumerable<EyouSoft.Data.CustomerCallBackResult> dataList = dcDal.CustomerCallBackResult.Where(item => item.CustomerCareforId == CareForId);
            foreach (EyouSoft.Data.CustomerCallBackResult model in dataList)
            {
                dcDal.CustomerCallBackResult.DeleteOnSubmit(model);
            }
            dcDal.SubmitChanges();
            return true;

        }

        /// <summary>
        /// 通过回访编号得到一个对象实体
        /// </summary>
        public EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo GetCustomerCallbackResultModel(int CareForId)
        {
            CustomerCallBackResultInfo model = null;
            string sql = string.Format(" select * from tbl_CustomerCallBackResult where id={0}", CareForId);
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rd.Read())
                {
                    model = new CustomerCallBackResultInfo();
                    model.Car = rd.GetByte(rd.GetOrdinal("Car"));
                    model.CustomerCareforId = rd.GetInt32(rd.GetOrdinal("CustomerCareforId"));
                    model.DepartureTime = rd.IsDBNull(rd.GetOrdinal("DepartureTime")) ? DateTime.Parse("2011-02-14") : rd.GetDateTime(rd.GetOrdinal("DepartureTime"));
                    model.Guide = rd.GetByte(rd.GetOrdinal("Guide"));
                    model.Hotel = rd.GetByte(rd.GetOrdinal("Hotel"));
                    model.ID = rd.GetInt32(rd.GetOrdinal("CustomerCareforId"));
                    model.Journey = rd.GetByte(rd.GetOrdinal("Journey"));
                    model.meals = rd.GetByte(rd.GetOrdinal("meals"));
                    model.Remark = rd["Remark"].ToString();
                    model.RouteID = rd.GetInt32(rd.GetOrdinal("CustomerCareforId")); ;
                    model.RouteName = rd["RouteName"].ToString();
                    model.Shopping = rd.GetByte(rd.GetOrdinal("Shopping"));
                    model.Spot = rd.GetByte(rd.GetOrdinal("Spot"));
                }
            }

            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public IList<EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo> GetCustomerCallbackResultList(int CallBackId)
        {
            IList<EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo> callResultList = new List<EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo>();
            EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo model = null;
            string sql = string.Format(" select * from tbl_CustomerCallBackResult where CustomerCareforId={0}", CallBackId);
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rd.Read())
                {
                    model = new CustomerCallBackResultInfo();
                    model.Car = rd.GetByte(rd.GetOrdinal("Car"));
                    model.CustomerCareforId = rd.GetInt32(rd.GetOrdinal("CustomerCareforId"));
                    model.DepartureTime = rd.IsDBNull(rd.GetOrdinal("DepartureTime")) ? DateTime.Parse("2011-02-14") : rd.GetDateTime(rd.GetOrdinal("DepartureTime"));
                    model.Guide = rd.GetByte(rd.GetOrdinal("Guide"));
                    model.Hotel = rd.GetByte(rd.GetOrdinal("Hotel"));
                    model.ID = rd.GetInt32(rd.GetOrdinal("CustomerCareforId"));
                    model.Journey = rd.GetByte(rd.GetOrdinal("Journey"));
                    model.meals = rd.GetByte(rd.GetOrdinal("meals"));
                    model.Remark = rd["Remark"].ToString();
                    model.RouteID = rd.GetInt32(rd.GetOrdinal("CustomerCareforId")); ;
                    model.RouteName = rd["RouteName"].ToString();
                    model.Shopping = rd.GetByte(rd.GetOrdinal("Shopping"));
                    model.Spot = rd.GetByte(rd.GetOrdinal("Spot"));
                    callResultList.Add(model);

                }
            }
            return callResultList;
        }



        #endregion  成员方法

        //客户营销活动方法
        #region  成员方法
        //给data赋值
        public void InitalMarketData(EyouSoft.Data.CustomerMarketing dataModel, CustomerMarketingInfo model)
        {
            dataModel.CompanyId = model.CompanyId;
            dataModel.Content = model.Content;
            dataModel.Effect = model.Effect;
            dataModel.Id = model.Id;
            dataModel.IssueTime = model.IssueTime;
            dataModel.OperatorId = model.OperatorId;
            dataModel.Participant = model.Participant;
            dataModel.Sponsor = model.Sponsor;
            dataModel.State = model.State;
            dataModel.Theme = model.Theme;
            dataModel.Time = model.Time;
        }
        //给model 赋值
        public void InitalMarketModel(CustomerMarketingInfo model, EyouSoft.Data.CustomerMarketing dataModel)
        {
            model.CompanyId = dataModel.CompanyId;
            model.Content = dataModel.Content;
            model.Effect = dataModel.Effect;
            model.Id = dataModel.Id;
            model.IssueTime = dataModel.IssueTime;
            model.OperatorId = dataModel.OperatorId;
            model.Participant = dataModel.Participant;
            model.Sponsor = dataModel.Sponsor;
            model.State = dataModel.State;
            model.Theme = dataModel.Theme;
            model.Time = dataModel.Time;
        }

        /// <summary>
        ///  增加一条数据
        /// </summary>
        public bool AddCustomerMarketing(EyouSoft.Model.CompanyStructure.CustomerMarketingInfo model)
        {
            if (model == null)
                return false;
            EyouSoft.Data.CustomerMarketing dataModel = new EyouSoft.Data.CustomerMarketing();
            if (dataModel != null)
            {
                InitalMarketData(dataModel, model);
                dcDal.CustomerMarketing.InsertOnSubmit(dataModel);
                dcDal.SubmitChanges();
                return true;
            }
            return false;

        }

        /// <summary>
        ///  更新一条数据
        /// </summary>
        public bool UpdateCustomerMarketing(EyouSoft.Model.CompanyStructure.CustomerMarketingInfo model)
        {
            EyouSoft.Data.CustomerMarketing dataModel = dcDal.CustomerMarketing.FirstOrDefault(item => item.Id == model.Id);
            if (dataModel != null)
            {
                InitalMarketData(dataModel, model);
                dcDal.SubmitChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteCustomerMarketing(int Id)
        {

            EyouSoft.Data.CustomerMarketing dataModel = dcDal.CustomerMarketing.FirstOrDefault(item => item.Id == Id);
            if (dataModel != null)
            {
                dcDal.CustomerMarketing.DeleteOnSubmit(dataModel);
                return true;
            }
            return false;

        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        public bool DeleteCustomerMarketingS(int[] Id)
        {
            if (Id.Length < 1)
                return false;

            StringBuilder str = new StringBuilder("");
            foreach (int i in Id)
            { str.AppendFormat("{0},", i); }
            string sql = string.Format(" delete from tbl_CustomerMarketing where id in({0})", str.ToString().Substring(0, (str.Length - 1)));
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;

        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public EyouSoft.Model.CompanyStructure.CustomerMarketingInfo GetCustomerMarketingModel(int Id)
        {
            CustomerMarketingInfo model = new CustomerMarketingInfo();
            EyouSoft.Data.CustomerMarketing dataModel = dcDal.CustomerMarketing.FirstOrDefault(item => item.Id == Id);
            InitalMarketModel(model, dataModel);
            dataModel = null;
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public IList<EyouSoft.Model.CompanyStructure.CustomerMarketingInfo> GetCustomerMarketingList(int PageSize, int PageIndex, int CompanyID, string strWhere, string filedOrder, ref int RecordCount, ref int PageCount)
        {
            List<EyouSoft.Model.CompanyStructure.CustomerMarketingInfo> MarketInfoList = new List<EyouSoft.Model.CompanyStructure.CustomerMarketingInfo>();
            string cmdQuery = string.Format(" Companyid={0} {1}", CompanyID, strWhere);
            string tableName = "tbl_CustomerMarketing";
            string primaryKey = "Id";
            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, tableName, primaryKey, filedOrder, cmdQuery, "IssueTime desc"))
            {
                EyouSoft.Model.CompanyStructure.CustomerMarketingInfo Model;
                while (rdr.Read())
                {
                    Model = new CustomerMarketingInfo();
                    Model.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    Model.Content = rdr["Content"].ToString();
                    Model.Effect = rdr["Effect"].ToString();
                    Model.Id = rdr.GetInt32(rdr.GetOrdinal("Id")); ;
                    Model.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    Model.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")); ;
                    Model.Participant = rdr["Participant"].ToString();
                    Model.Sponsor = rdr["Sponsor"].ToString();
                    Model.State = rdr.GetByte(rdr.GetOrdinal("State"));
                    Model.Theme = rdr["Theme"].ToString();
                    Model.Time = rdr.GetDateTime(rdr.GetOrdinal("Time")); ;

                    MarketInfoList.Add(Model);

                }
            }
            PageCount = RecordCount / PageSize + 1;
            return MarketInfoList;
        }



        #endregion  成员方法

        #region  私有方法
        /// <summary>
        /// 创建机票用户对象XML
        /// </summary>
        /// <returns></returns>
        private string CreateCustomerXML(IList<EyouSoft.Model.CompanyStructure.CustomerContactInfo> list)
        {
            if (list != null && list.Count > 0)
            {
                StringBuilder strXML = new StringBuilder("<ROOT>");
                foreach (EyouSoft.Model.CompanyStructure.CustomerContactInfo model in list)
                {
                    strXML.AppendFormat("<CustomerInfo ");
                    if (model.UserAccount != null && !string.IsNullOrEmpty(model.UserAccount.UserName) && !string.IsNullOrEmpty(model.UserAccount.PassWordInfo.NoEncryptPassword))
                    {
                        strXML.AppendFormat(" AreaIds=\"{0}\"  username=\"{1}\" Pwd=\"{2}\" Sex=\"{3}\" ContactName=\"{4}\" Mobile=\"{5}\" qq=\"{6}\" BirthDay=\"{7}\" Email=\"{8}\"    Job=\"{9}\"  Department=\"{10}\"   ContactId=\"{11}\"  Md5=\"{12}\" Tel=\"{13}\" Fax=\"{14}\" />", model.AreaIds, model.UserAccount.UserName, model.UserAccount.PassWordInfo.NoEncryptPassword, model.Sex, model.Name, model.Mobile, model.qq, model.BirthDay, model.Email, model.Job, model.Department, model.ID, model.UserAccount.PassWordInfo.MD5Password, model.Tel, model.Fax);
                    }
                    else
                    {
                        strXML.AppendFormat(" AreaIds=\"{0}\" username=\"{1}\" Pwd=\"{2}\"  Sex=\"{3}\" ContactName=\"{4}\" Mobile=\"{5}\" qq=\"{6}\" BirthDay=\"{7}\" Email=\"{8}\" Job=\"{9}\" Department=\"{10}\"  ContactId=\"{11}\" Md5=\"{12}\" Tel=\"{13}\"  Fax=\"{14}\" />", model.AreaIds, "", "", model.Sex, model.Name, model.Mobile, model.qq, model.BirthDay, model.Email, model.Job, model.Department, model.ID, "", model.Tel, model.Fax);
                    }
                }
                strXML.Append("</ROOT>");
                return strXML.ToString();
            }
            return "";
        }

        private string CreateCustomerCallResultXML(IList<EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo> list)
        {
            if (list != null && list.Count > 0)
            {
                StringBuilder strXML = new StringBuilder("<ROOT>");
                foreach (EyouSoft.Model.CompanyStructure.CustomerCallBackResultInfo model in list)
                {
                    if (string.IsNullOrEmpty(model.DepartureTime.ToString()))
                        model.DepartureTime = DateTime.Now;
                    strXML.AppendFormat("<CallBackResult ");
                    strXML.AppendFormat(" RouteID=\"{0}\" RouteName=\"{1}\" DepartureTime=\"{2}\" Journey=\"{3}\" meals=\"{4}\" Hotel=\"{5}\" Spot=\"{6}\" Guide=\"{7}\" Shopping=\"{8}\" Car=\"{9}\" Remark=\"{10}\" />"
                    , model.RouteID, model.RouteName, model.DepartureTime, model.Journey, model.meals, model.Hotel, model.Spot, model.Guide, model.Shopping, model.Car, model.Remark);

                }
                strXML.Append("</ROOT>");
                return strXML.ToString();
            }
            return "";
        }

        /// <summary>
        /// 返回区域id和名称 id=str[0],name=str[1]
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        private string[] GetAreaList(int UserId)
        {
            string[] str = new string[] { "", "" };
            EyouSoft.DAL.CompanyStructure.Area area = new Area();
            IList<EyouSoft.Model.CompanyStructure.Area> arealist = area.GetAreaList(UserId);
            StringBuilder strId = new StringBuilder("");
            StringBuilder strName = new StringBuilder("");
            if (arealist != null && arealist.Count > 0)
            {
                foreach (EyouSoft.Model.CompanyStructure.Area a in arealist)
                {
                    strId.AppendFormat("{0},", a.Id);
                    strName.AppendFormat("{0},", a.AreaName);
                }
            }
            str[0] = strId.ToString().TrimEnd(',');
            str[1] = strName.ToString().TrimEnd(',');
            return str;
        }
        #endregion

        #region 收款提醒方法

        /// <summary>
        /// 分页获取收款提醒
        /// </summary>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="accountDay">结算日</param>
        /// <param name="accountDayWeek">结算星期数组</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.ReceiptRemind> GetReceiptRemind(int pageSize, int pageIndex, ref int recordCount, int CompanyId, int accountDay, int[] accountDayWeek, EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo searchInfo)
        {
            if (CompanyId <= 0)
                return null;

            IList<EyouSoft.Model.PersonalCenterStructure.ReceiptRemind> list = new List<EyouSoft.Model.PersonalCenterStructure.ReceiptRemind>();

            string strOrder = @" ArrearCash desc ";
            string fields = "*";
            string cmdQuery = "SearchTourDaiShouKuanHeJi>0";

            StringBuilder tableSQL = new StringBuilder();

            #region SQL
            tableSQL.Append(" SELECT Id,[Name],ContactName,Phone,ArrearCash,SalerInfo ");

            bool hasSearchTourDaiShouKuanHeJi = false;
            if (searchInfo != null)
            {
                if (searchInfo.LEDate.HasValue
                    || searchInfo.LSDate.HasValue
                    || (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    || (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0))
                {
                    hasSearchTourDaiShouKuanHeJi = true;
                    strOrder = "SearchTourDaiShouKuanHeJi DESC";
                    tableSQL.Append(" ,(SELECT SUM(A.FinanceSum-A.HasCheckMoney) FROM tbl_TourOrder AS A WHERE A.BuyCompanyId=View_ReceiptRemind_GetList.Id AND A.[IsDelete]='0' AND A.[OrderState] NOT IN(3,4) AND TourId IN(SELECT TourId FROM tbl_Tour AS B WHERE 1=1 ");
                    if (searchInfo.LSDate.HasValue)
                    {
                        tableSQL.AppendFormat(" AND B.LeaveDate>='{0}' ", searchInfo.LSDate.Value);
                    }
                    if (searchInfo.LEDate.HasValue)
                    {
                        tableSQL.AppendFormat(" AND B.LeaveDate<='{0}' ", searchInfo.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0)
                    {
                        tableSQL.AppendFormat(" AND B.OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorIds));
                    }
                    if (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    {
                        tableSQL.AppendFormat(" AND B.OperatorId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorDepartIds));
                    }
                    tableSQL.Append(" )) AS SearchTourDaiShouKuanHeJi ");
                }
                else
                {
                    tableSQL.Append(",0.0001 AS SearchTourDaiShouKuanHeJi");
                }
            }
            else
            {
                tableSQL.Append(",0.0001 AS SearchTourDaiShouKuanHeJi");
            }

            tableSQL.Append(" FROM View_ReceiptRemind_GetList ");

            tableSQL.AppendFormat(" WHERE ArrearCash > 0 and CompanyId = {0} AND ( (AccountDay<={1} AND AccountDayType={2}) OR (AccountDay IN({3}) AND AccountDayType={4})) ", CompanyId
                , accountDay
                , (int)EyouSoft.Model.EnumType.CompanyStructure.AccountDayType.Month
                , EyouSoft.Toolkit.Utils.GetSqlIdStrByArray(accountDayWeek)
                , (int)EyouSoft.Model.EnumType.CompanyStructure.AccountDayType.Week);

            if (searchInfo != null)
            {
                if (!string.IsNullOrEmpty(searchInfo.QianKuanDanWei))
                {
                    tableSQL.AppendFormat(" AND Name LIKE '%{0}%' ", searchInfo.QianKuanDanWei);
                }

                if (searchInfo.LEDate.HasValue
                    || searchInfo.LSDate.HasValue
                    || (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    || (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0))
                {
                    tableSQL.AppendFormat(" AND EXISTS( ");
                    tableSQL.AppendFormat(" SELECT 1 FROM tbl_Tour AS B WHERE B.TourId IN(SELECT TourId FROM tbl_TourOrder AS A WHERE A.BuyCompanyID=View_ReceiptRemind_GetList.Id AND A.[IsDelete]='0' AND A.[OrderState] NOT IN(3,4)) ");

                    if (searchInfo.LSDate.HasValue)
                    {
                        tableSQL.AppendFormat(" AND B.LeaveDate>='{0}' ", searchInfo.LSDate.Value);
                    }
                    if (searchInfo.LEDate.HasValue)
                    {
                        tableSQL.AppendFormat(" AND B.LeaveDate<='{0}' ", searchInfo.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0)
                    {
                        tableSQL.AppendFormat(" AND B.OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorIds));
                    }
                    if (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    {
                        tableSQL.AppendFormat(" AND B.OperatorId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorDepartIds));
                    }

                    tableSQL.Append(" ) ");
                }
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReaderBySqlTbl(_db, pageSize, pageIndex, ref recordCount, tableSQL.ToString(), fields, cmdQuery, strOrder, false))
            {
                EyouSoft.Model.PersonalCenterStructure.ReceiptRemind tModel = null;
                System.Xml.XmlDocument xml = null;
                System.Xml.XmlNodeList xmlNodeList = null;
                while (rdr.Read())
                {
                    tModel = new EyouSoft.Model.PersonalCenterStructure.ReceiptRemind();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Id"))) tModel.CustomerId = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    tModel.CustomerName = rdr["Name"].ToString();
                    tModel.ContactName = rdr["ContactName"].ToString();
                    tModel.ContactTel = rdr["Phone"].ToString();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("ArrearCash"))) tModel.ArrearCash = rdr.GetDecimal(rdr.GetOrdinal("ArrearCash"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("SalerInfo")))
                    {
                        string strPlanNames = string.Empty;
                        xml = new System.Xml.XmlDocument();
                        xml.LoadXml(rdr["SalerInfo"].ToString());
                        xmlNodeList = xml.GetElementsByTagName("tbl_TourOrder");
                        if (xmlNodeList != null && xmlNodeList.Count > 0)
                        {
                            foreach (System.Xml.XmlNode t in xmlNodeList)
                            {
                                if (t == null || t.Attributes == null || t.Attributes.Count <= 0)
                                    continue;

                                if (t.Attributes["SalerName"] != null && !string.IsNullOrEmpty(t.Attributes["SalerName"].Value))
                                {
                                    if (!strPlanNames.Contains(t.Attributes["SalerName"].Value))
                                    {
                                        strPlanNames += t.Attributes["SalerName"].Value + ",";
                                    }
                                }
                            }
                        }
                        strPlanNames = strPlanNames.Trim(',');
                        tModel.SalerName = strPlanNames;
                    }

                    if (hasSearchTourDaiShouKuanHeJi)
                    {
                        tModel.ArrearCash = rdr.IsDBNull(rdr.GetOrdinal("SearchTourDaiShouKuanHeJi")) ? 0 : rdr.GetDecimal(rdr.GetOrdinal("SearchTourDaiShouKuanHeJi"));
                    }

                    list.Add(tModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取收款提醒合计-全部用户
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="accountDay">结算日</param>
        /// <param name="accountDayWeek">结算星期数组</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="daiShouKuanHeJi">待收款合计</param>
        /// <returns></returns>
        public void GetReceiptRemind(int companyId, int accountDay, int[] accountDayWeek, EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo searchInfo, out decimal daiShouKuanHeJi)
        {
            daiShouKuanHeJi = 0;
            StringBuilder cmdText = new StringBuilder();

            #region SQL
            cmdText.Append(" SELECT SUM(ArrearCash) AS DaiShouKuanHeJi,SUM(SearchTourDaiShouKuanHeJi) AS SearchTourDaiShouKuanHeJi FROM (");

            cmdText.Append("SELECT ArrearCash ");

            bool hasSearchTourDaiShouKuanHeJi = false;
            if (searchInfo != null)
            {
                if (searchInfo.LEDate.HasValue
                    || searchInfo.LSDate.HasValue
                    || (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    || (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0))
                {
                    hasSearchTourDaiShouKuanHeJi = true;
                    cmdText.Append(" ,(SELECT SUM(A.FinanceSum-A.HasCheckMoney) FROM tbl_TourOrder AS A WHERE A.BuyCompanyId=View_ReceiptRemind_GetList.Id AND A.[IsDelete]='0' AND A.[OrderState] NOT IN(3,4) AND TourId IN(SELECT TourId FROM tbl_Tour AS B WHERE 1=1 ");
                    if (searchInfo.LSDate.HasValue)
                    {
                        cmdText.AppendFormat(" AND B.LeaveDate>='{0}' ", searchInfo.LSDate.Value);
                    }
                    if (searchInfo.LEDate.HasValue)
                    {
                        cmdText.AppendFormat(" AND B.LeaveDate<='{0}' ", searchInfo.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0)
                    {
                        cmdText.AppendFormat(" AND B.OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorIds));
                    }
                    if (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    {
                        cmdText.AppendFormat(" AND B.OperatorId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorDepartIds));
                    }
                    cmdText.Append(" )) AS SearchTourDaiShouKuanHeJi ");
                }
                else
                {
                    cmdText.Append(" ,0 AS SearchTourDaiShouKuanHeJi ");
                }
            }
            else
            {
                cmdText.Append(" ,0 AS SearchTourDaiShouKuanHeJi ");
            }

            cmdText.Append(" FROM View_ReceiptRemind_GetList WHERE ");

            cmdText.AppendFormat(" ArrearCash > 0 and CompanyId = {0} AND ( (AccountDay<={1} AND AccountDayType={2}) OR (AccountDay IN({3}) AND AccountDayType={4})) ", companyId
                , accountDay
                , (int)EyouSoft.Model.EnumType.CompanyStructure.AccountDayType.Month
                , EyouSoft.Toolkit.Utils.GetSqlIdStrByArray(accountDayWeek)
                , (int)EyouSoft.Model.EnumType.CompanyStructure.AccountDayType.Week);

            if (searchInfo != null)
            {
                if (!string.IsNullOrEmpty(searchInfo.QianKuanDanWei))
                {
                    cmdText.AppendFormat(" AND Name LIKE '%{0}%' ", searchInfo.QianKuanDanWei);
                }

                if (searchInfo.LEDate.HasValue
                    || searchInfo.LSDate.HasValue
                    || (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    || (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0))
                {
                    cmdText.AppendFormat(" AND EXISTS( ");
                    cmdText.AppendFormat(" SELECT 1 FROM tbl_Tour AS B WHERE B.TourId IN(SELECT TourId FROM tbl_TourOrder AS A WHERE A.BuyCompanyID=View_ReceiptRemind_GetList.Id AND A.[IsDelete]='0' AND A.[OrderState] NOT IN(3,4)) ");

                    if (searchInfo.LSDate.HasValue)
                    {
                        cmdText.AppendFormat(" AND B.LeaveDate>='{0}' ", searchInfo.LSDate.Value);
                    }
                    if (searchInfo.LEDate.HasValue)
                    {
                        cmdText.AppendFormat(" AND B.LeaveDate<='{0}' ", searchInfo.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0)
                    {
                        cmdText.AppendFormat(" AND B.OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorIds));
                    }
                    if (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    {
                        cmdText.AppendFormat(" AND B.OperatorId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorDepartIds));
                    }

                    cmdText.Append(" ) ");
                }

            }
            cmdText.Append(")C");
            #endregion

            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) daiShouKuanHeJi = rdr.GetDecimal(0);
                    if (hasSearchTourDaiShouKuanHeJi) daiShouKuanHeJi = rdr.IsDBNull(1) ? 0 : rdr.GetDecimal(1);
                }
            }

        }

        /*/// <summary>
        /// 获取收款提醒数量
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public int GetReceiptRemind(int CompanyId)
        {
            if (CompanyId <= 0)
                return 0;

            StringBuilder strSql = new StringBuilder(" select count(Id) from View_ReceiptRemind_GetList ");
            strSql.AppendFormat(" where ArrearCash > 0 and CompanyId = {0} ", CompanyId);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            object obj = DbHelper.GetSingle(dc, _db);
            if (obj.Equals(null))
                return 0;
            else
                return int.Parse(obj.ToString());
        }*/

        #endregion

        /// <summary>
        /// 按指定条件获取客户资料信息集合
        /// </summary>
        /// <param name="companyId">公司（专线）编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seachInfo">查询条件业务实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CustomerInfo> GetCustomers(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.CompanyStructure.MCustomerSeachInfo seachInfo)
        {
            IList<EyouSoft.Model.CompanyStructure.CustomerInfo> items = new List<EyouSoft.Model.CompanyStructure.CustomerInfo>();

            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_Customer";
            string primaryKey = "Id";
            string orderByString = "IssueTime DESC";
            string fields = "*,(SELECT A.CustomStandName FROM tbl_CompanyCustomStand AS A WHERE A.Id=tbl_Customer.CustomerLev) AS LevelName";

            #region 拼接查询条件
            cmdQuery.AppendFormat(" Companyid={0} AND IsDelete='0' ", companyId);

            if (seachInfo != null)
            {
                if (seachInfo.CityId.HasValue)
                {
                    cmdQuery.AppendFormat(" AND CityId={0} ", seachInfo.CityId.Value);
                }
                if (!string.IsNullOrEmpty(seachInfo.ContactName))
                {
                    cmdQuery.AppendFormat(" AND ContactName LIKE '%{0}%' ", seachInfo.ContactName);
                }
                if (!string.IsNullOrEmpty(seachInfo.ContactTelephone))
                {
                    /*联系电话及手机查询
                    cmdQuery.AppendFormat(" AND (Phone LIKE '%{0}%' OR Mobile LIKE '%{0}%' OR EXISTS(SELECT 1 FROM tbl_CustomerContactInfo WHERE CustomerId=tbl_Customer.Id AND (Tel LIKE '%{0}%' OR Mobile LIKE '%{0}%'))) ", seachInfo.ContactTelephone);*/
                    //联系电话查询
                    cmdQuery.AppendFormat(" AND (Phone LIKE '%{0}%' OR EXISTS(SELECT 1 FROM tbl_CustomerContactInfo WHERE CustomerId=tbl_Customer.Id AND (Tel LIKE '%{0}%'))) ", seachInfo.ContactTelephone);
                }
                if (!string.IsNullOrEmpty(seachInfo.CustomerName))
                {
                    cmdQuery.AppendFormat(" AND Name LIKE '%{0}%' ", seachInfo.CustomerName);
                }
                if (seachInfo.ProvinceId.HasValue)
                {
                    cmdQuery.AppendFormat(" AND ProviceId={0} ", seachInfo.ProvinceId.Value);
                }
                if (!string.IsNullOrEmpty(seachInfo.SellerName))
                {
                    cmdQuery.AppendFormat(" AND Saler LIKE '%{0}%' ", seachInfo.SellerName);
                }
                if (seachInfo.SellerIds != null && seachInfo.SellerIds.Length > 0)
                {
                    cmdQuery.AppendFormat(" AND SaleId IN({0}) ", EyouSoft.Toolkit.Utils.GetSqlIdStrByArray(seachInfo.SellerIds));
                }
                if (!string.IsNullOrEmpty(seachInfo.Mobile))
                {
                    cmdQuery.AppendFormat(" AND Mobile LIKE '%{0}%' ", seachInfo.Mobile);
                }
                if (seachInfo.CityIdList != null && seachInfo.CityIdList.Length > 0)
                {
                    cmdQuery.AppendFormat(" AND CityId IN({0}) ", Utils.GetSqlIdStrByArray(seachInfo.CityIdList));
                }
                if (seachInfo.ProvinceIds != null && seachInfo.ProvinceIds.Length > 0)
                {
                    cmdQuery.AppendFormat(" AND ProviceId IN({0}) ", Utils.GetSqlIdStrByArray(seachInfo.ProvinceIds));
                }
            }

            #endregion

            #region 排序处理
            if (seachInfo != null)
            {
                switch (seachInfo.OrderByField)
                {
                    case EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.创建时间:
                        orderByString = " IssueTime ";
                        break;
                    case EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.地址:
                        orderByString = " Adress ";
                        break;
                    case EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.交易次数:
                        orderByString = " TradeNum ";
                        break;
                    case EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.结算方式:
                        orderByString = " AccountWay ";
                        break;
                    case EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.客户名称:
                        orderByString = " Name ";
                        break;
                    case EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.省份:
                        orderByString = " ProviceId ";
                        break;
                    case EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.责任销售:
                        orderByString = " Saler ";
                        break;
                    case EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.主要联系人传真:
                        orderByString = " Fax ";
                        break;
                    case EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.主要联系人电话:
                        orderByString = " Phone ";
                        break;
                    case EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.主要联系人名称:
                        orderByString = " ContactName ";
                        break;
                    case EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.主要联系人手机:
                        orderByString = " Mobile ";                        
                        break;
                }

                if (seachInfo.OrderByType == EyouSoft.Model.EnumType.CompanyStructure.OrderByType.DESC)
                {
                    orderByString += " DESC ";
                }
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields, cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.CompanyStructure.CustomerInfo item = new EyouSoft.Model.CompanyStructure.CustomerInfo();
                    item.Adress = rdr.IsDBNull(rdr.GetOrdinal("Adress")) ? "" : rdr["Adress"].ToString();
                    item.BankAccount = rdr.IsDBNull(rdr.GetOrdinal("BankAccount")) ? "" : rdr["BankAccount"].ToString();
                    item.CityId = rdr.GetInt32(rdr.GetOrdinal("CityId"));
                    item.CityName = rdr.IsDBNull(rdr.GetOrdinal("CityName")) ? "" : rdr["CityName"].ToString();
                    item.CommissionCount = rdr.GetDecimal(rdr.GetOrdinal("CommissionCount"));
                    item.CommissionType = rdr.IsDBNull(rdr.GetOrdinal("CommissionType")) ? (EyouSoft.Model.EnumType.CompanyStructure.CommissionType.未确定) : (EyouSoft.Model.EnumType.CompanyStructure.CommissionType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.CommissionType), rdr.GetByte(rdr.GetOrdinal("CommissionType")).ToString());
                    item.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    item.ComplaintNum = rdr.GetInt32(rdr.GetOrdinal("ComplaintNum"));
                    item.ContactName = rdr.IsDBNull(rdr.GetOrdinal("ContactName")) ? "" : rdr["ContactName"].ToString();
                    item.CustomerLev = rdr.GetInt32(rdr.GetOrdinal("CustomerLev"));
                    item.CustomerStamp = rdr.IsDBNull(rdr.GetOrdinal("CustomerStamp")) ? "" : rdr["CustomerStamp"].ToString();
                    item.Fax = rdr.IsDBNull(rdr.GetOrdinal("Fax")) ? "" : rdr["Fax"].ToString();
                    item.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    item.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    item.Licence = rdr.IsDBNull(rdr.GetOrdinal("Licence")) ? "" : rdr["Licence"].ToString();
                    item.MaxDebts = rdr.GetDecimal(rdr.GetOrdinal("MaxDebts"));
                    item.Mobile = rdr.IsDBNull(rdr.GetOrdinal("Mobile")) ? "" : rdr["Mobile"].ToString();
                    item.Name = rdr.IsDBNull(rdr.GetOrdinal("Name")) ? "" : rdr["Name"].ToString();
                    item.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    item.PageFootFile = rdr.IsDBNull(rdr.GetOrdinal("PageFootFile")) ? "" : rdr["PageFootFile"].ToString();
                    item.PageHeadFile = rdr.IsDBNull(rdr.GetOrdinal("PageHeadFile")) ? "" : rdr["PageHeadFile"].ToString();
                    item.Phone = rdr.IsDBNull(rdr.GetOrdinal("Phone")) ? "" : rdr["Phone"].ToString();
                    item.PostalCode = rdr.IsDBNull(rdr.GetOrdinal("PostalCode")) ? "" : rdr["PostalCode"].ToString();
                    item.PreDeposit = rdr.GetDecimal(rdr.GetOrdinal("PreDeposit"));
                    item.ProviceId = rdr.GetInt32(rdr.GetOrdinal("ProviceId"));
                    item.ProvinceName = rdr.IsDBNull(rdr.GetOrdinal("ProvinceName")) ? "" : rdr["ProvinceName"].ToString();
                    item.Remark = rdr.IsDBNull(rdr.GetOrdinal("Remark")) ? "" : rdr["Remark"].ToString();
                    item.SaleId = rdr.GetInt32(rdr.GetOrdinal("SaleId"));
                    item.Saler = rdr.IsDBNull(rdr.GetOrdinal("Saler")) ? "" : rdr["Saler"].ToString();
                    item.SatNum = rdr.GetDecimal(rdr.GetOrdinal("SatNum"));
                    item.TemplateFile = rdr.IsDBNull(rdr.GetOrdinal("TemplateFile")) ? "" : rdr["TemplateFile"].ToString();
                    item.TradeNum = rdr.GetInt32(rdr.GetOrdinal("TradeNum"));
                    item.IsEnable = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsEnable")));
                    item.LevelName = rdr["LevelName"].ToString();
                    item.AccountWay = rdr["AccountWay"].ToString();
                    item.JieSuanType = (EyouSoft.Model.EnumType.CompanyStructure.KHJieSuanType)rdr.GetByte(rdr.GetOrdinal("JieSuanType"));
                    item.IsRequiredTourCode = GetBoolean(rdr.GetString(rdr.GetOrdinal("IsRequiredTourCode")));

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取客户关系(组团)联系人编号
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public int GetContactId(int userId)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetContactId);
            this._db.AddInParameter(cmd, "UID", DbType.Int32, userId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    return rdr.GetInt32(0);
                }
            }

            return 0;
        }

        /// <summary>
        /// 分页获取收款提醒-按订单销售员
        /// </summary>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="accountDay">结算日</param>
        /// <param name="accountDayWeek">结算星期数组</param>
        /// <param name="sellers">销售员编号集合 多个编号间用“,”间隔</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.ReceiptRemind> GetReceiptRemindByOrderSeller(int pageSize, int pageIndex, ref int recordCount, int companyId, int accountDay, int[] accountDayWeek, string sellers, EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo searchInfo)
        {
            IList<EyouSoft.Model.PersonalCenterStructure.ReceiptRemind> items = new List<EyouSoft.Model.PersonalCenterStructure.ReceiptRemind>();
            string cmdQuery = "SearchTourDaiShouKuanHeJi>0";
            string orderByString = "NotPayAmount DESC";
            string fields = "*";

            StringBuilder tableSQL = new StringBuilder();

            #region SQL
            tableSQL.Append(" SELECT * ");
            bool hasSearchTourDaiShouKuanHeJi = false;
            if (searchInfo != null)
            {
                if (searchInfo.LEDate.HasValue
                    || searchInfo.LSDate.HasValue
                    || (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    || (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0))
                {
                    hasSearchTourDaiShouKuanHeJi = true;
                    orderByString = "SearchTourDaiShouKuanHeJi DESC";
                    tableSQL.Append(" ,(SELECT SUM(A.FinanceSum-A.HasCheckMoney) FROM tbl_TourOrder AS A WHERE A.BuyCompanyId=View_ReceiptRemind.BuyCompanyId AND A.SalerId=View_ReceiptRemind.SalerId AND A.[IsDelete]='0' AND A.[OrderState] NOT IN(3,4) AND TourId IN(SELECT TourId FROM tbl_Tour AS B WHERE 1=1 ");
                    if (searchInfo.LSDate.HasValue)
                    {
                        tableSQL.AppendFormat(" AND B.LeaveDate>='{0}' ", searchInfo.LSDate.Value);
                    }
                    if (searchInfo.LEDate.HasValue)
                    {
                        tableSQL.AppendFormat(" AND B.LeaveDate<='{0}' ", searchInfo.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0)
                    {
                        tableSQL.AppendFormat(" AND B.OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorIds));
                    }
                    if (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    {
                        tableSQL.AppendFormat(" AND B.OperatorId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorDepartIds));
                    }
                    tableSQL.Append(" )) AS SearchTourDaiShouKuanHeJi ");
                }
                else
                {
                    tableSQL.Append(",0.0001 AS SearchTourDaiShouKuanHeJi ");
                }
            }
            else
            {
                tableSQL.Append( ",0.0001 AS SearchTourDaiShouKuanHeJi ");
            }

            tableSQL.Append(" FROM View_ReceiptRemind WHERE 1=1 ");

            tableSQL.AppendFormat(" AND NotPayAmount>0 AND SellCompanyId={0} ", companyId);
            tableSQL.AppendFormat(" AND ( (AccountDay<={0} AND AccountDayType={1}) OR (AccountDay IN({2}) AND AccountDayType={3})) ", accountDay
                , (int)EyouSoft.Model.EnumType.CompanyStructure.AccountDayType.Month
                , EyouSoft.Toolkit.Utils.GetSqlIdStrByArray(accountDayWeek)
                , (int)EyouSoft.Model.EnumType.CompanyStructure.AccountDayType.Week);

            if (!string.IsNullOrEmpty(sellers))
            {
                tableSQL.AppendFormat(" AND SalerId IN({0}) ", sellers);
            }

            if (searchInfo != null)
            {
                if (!string.IsNullOrEmpty(searchInfo.QianKuanDanWei))
                {
                    tableSQL.AppendFormat(" AND Name LIKE '%{0}%' ", searchInfo.QianKuanDanWei);
                }

                if (searchInfo.LEDate.HasValue
                    || searchInfo.LSDate.HasValue
                    || (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    || (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0))
                {
                    tableSQL.AppendFormat(" AND EXISTS( ");
                    tableSQL.AppendFormat(" SELECT 1 FROM tbl_Tour AS B WHERE B.TourId IN(SELECT TourId FROM tbl_TourOrder AS A WHERE A.BuyCompanyID=View_ReceiptRemind.BuyCompanyId AND A.SalerId=View_ReceiptRemind.SalerId AND A.[IsDelete]='0' AND A.[OrderState] NOT IN(3,4)) ");

                    if (searchInfo.LSDate.HasValue)
                    {
                        tableSQL.AppendFormat(" AND B.LeaveDate>='{0}' ", searchInfo.LSDate.Value);
                    }
                    if (searchInfo.LEDate.HasValue)
                    {
                        tableSQL.AppendFormat(" AND B.LeaveDate<='{0}' ", searchInfo.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0)
                    {
                        tableSQL.AppendFormat(" AND B.OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorIds));
                    }
                    if (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    {
                        tableSQL.AppendFormat(" AND B.OperatorId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorDepartIds));
                    }

                    tableSQL.Append(" ) ");
                }
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReaderBySqlTbl(_db, pageSize, pageIndex, ref recordCount, tableSQL.ToString(), fields, cmdQuery, orderByString, false))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.PersonalCenterStructure.ReceiptRemind item = new EyouSoft.Model.PersonalCenterStructure.ReceiptRemind();

                    item.ArrearCash = rdr.IsDBNull(rdr.GetOrdinal("NotPayAmount")) ? 0 : rdr.GetDecimal(rdr.GetOrdinal("NotPayAmount"));
                    item.ContactName = rdr["ContactName"].ToString();
                    item.ContactTel = rdr["Phone"].ToString();
                    item.CustomerId = rdr.IsDBNull(rdr.GetOrdinal("BuyCompanyId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("BuyCompanyId"));
                    item.CustomerName = rdr["Name"].ToString();
                    item.SalerName = rdr["SalerName"].ToString();
                    item.SellerId = rdr.GetInt32(rdr.GetOrdinal("SalerId"));
                    if (hasSearchTourDaiShouKuanHeJi) item.ArrearCash = rdr.IsDBNull(rdr.GetOrdinal("SearchTourDaiShouKuanHeJi")) ? 0 : rdr.GetDecimal(rdr.GetOrdinal("SearchTourDaiShouKuanHeJi"));
                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取收款提醒合计-按订单销售员
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="accountDay">结算日</param>
        /// <param name="accountDayWeek">结算星期数组</param>
        /// <param name="sellers">销售员编号集合 多个编号间用“,”间隔</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="daiShouKuanHeji">待收款合计</param>
        /// <returns></returns>
        public void GetReceiptRemindByOrderSeller(int companyId, int accountDay, int[] accountDayWeek, string sellers, EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo searchInfo, out decimal daiShouKuanHeji)
        {
            daiShouKuanHeji = 0;
            StringBuilder cmdText = new StringBuilder();

            #region SQL
            cmdText.AppendFormat("SELECT SUM(NotPayAmount) AS DaiShouKuan,SUM(SearchTourDaiShouKuanHeJi) AS SearchTourDaiShouKuanHeJi FROM(");
            cmdText.AppendFormat(" SELECT NotPayAmount");

            bool hasSearchTourDaiShouKuanHeJi = false;
            if (searchInfo != null)
            {
                if (searchInfo.LEDate.HasValue
                    || searchInfo.LSDate.HasValue
                    || (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    || (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0))
                {
                    hasSearchTourDaiShouKuanHeJi = true;
                    cmdText.AppendFormat(" ,(SELECT SUM(A.FinanceSum-A.HasCheckMoney) FROM tbl_TourOrder AS A WHERE A.BuyCompanyId=View_ReceiptRemind.BuyCompanyId AND A.SalerId=View_ReceiptRemind.SalerId AND A.[IsDelete]='0' AND A.[OrderState] NOT IN(3,4) AND TourId IN(SELECT TourId FROM tbl_Tour AS B WHERE 1=1 ");
                    if (searchInfo.LSDate.HasValue)
                    {
                        cmdText.AppendFormat(" AND B.LeaveDate>='{0}' ", searchInfo.LSDate.Value);
                    }
                    if (searchInfo.LEDate.HasValue)
                    {
                        cmdText.AppendFormat(" AND B.LeaveDate<='{0}' ", searchInfo.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0)
                    {
                        cmdText.AppendFormat(" AND B.OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorIds));
                    }
                    if (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    {
                        cmdText.AppendFormat(" AND B.OperatorId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorDepartIds));
                    }
                    cmdText.Append(" )) AS SearchTourDaiShouKuanHeJi ");
                }
                else
                {
                    cmdText.Append(",0 AS SearchTourDaiShouKuanHeJi");
                }
            }
            else
            {
                cmdText.Append( ",0 AS SearchTourDaiShouKuanHeJi");
            }

            cmdText.AppendFormat(" FROM View_ReceiptRemind WHERE ");

            cmdText.AppendFormat(" NotPayAmount>0 AND SellCompanyId={0} ", companyId);
            cmdText.AppendFormat(" AND ( (AccountDay<={0} AND AccountDayType={1}) OR (AccountDay IN({2}) AND AccountDayType={3})) ", accountDay
                , (int)EyouSoft.Model.EnumType.CompanyStructure.AccountDayType.Month
                , EyouSoft.Toolkit.Utils.GetSqlIdStrByArray(accountDayWeek)
                , (int)EyouSoft.Model.EnumType.CompanyStructure.AccountDayType.Week);

            if (!string.IsNullOrEmpty(sellers))
            {
                cmdText.AppendFormat(" AND SalerId IN({0}) ", sellers);
            }

            if (searchInfo != null)
            {
                if (!string.IsNullOrEmpty(searchInfo.QianKuanDanWei))
                {
                    cmdText.AppendFormat(" AND Name LIKE '%{0}%' ", searchInfo.QianKuanDanWei);
                }

                if (searchInfo.LEDate.HasValue
                    || searchInfo.LSDate.HasValue
                    || (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    || (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0))
                {
                    cmdText.AppendFormat(" AND EXISTS( ");
                    cmdText.AppendFormat(" SELECT 1 FROM tbl_Tour AS B WHERE B.TourId IN(SELECT TourId FROM tbl_TourOrder AS A WHERE A.BuyCompanyID=View_ReceiptRemind.BuyCompanyId AND A.SalerId=View_ReceiptRemind.SalerId AND A.[IsDelete]='0' AND A.[OrderState] NOT IN(3,4)) ");

                    if (searchInfo.LSDate.HasValue)
                    {
                        cmdText.AppendFormat(" AND B.LeaveDate>='{0}' ", searchInfo.LSDate.Value);
                    }
                    if (searchInfo.LEDate.HasValue)
                    {
                        cmdText.AppendFormat(" AND B.LeaveDate<='{0}' ", searchInfo.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0)
                    {
                        cmdText.AppendFormat(" AND B.OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorIds));
                    }
                    if (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    {
                        cmdText.AppendFormat(" AND B.OperatorId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorDepartIds));
                    }

                    cmdText.Append(" ) ");
                }
            }

            cmdText.AppendFormat(")C");
            #endregion

            DbCommand cmd=_db.GetSqlStringCommand(cmdText.ToString());

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) daiShouKuanHeji = rdr.GetDecimal(0);
                    if (hasSearchTourDaiShouKuanHeJi) daiShouKuanHeji = rdr.IsDBNull(1) ? 0 : rdr.GetDecimal(1);
                }
            }
        }

        /// <summary>
        /// 批量更新客户资料责任销售
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="customers">客户资料编号集合</param>
        /// <param name="sellerId">责任销售员编号</param>
        /// <param name="sellerName">责任销售员姓名</param>
        /// <returns></returns>
        public bool BatchSpecifiedSeller(int companyId, IList<int> customers, int sellerId, string sellerName)
        {
            string cmdText = string.Format("UPDATE [tbl_Customer] SET [SaleId]={0},[Saler]='{1}' WHERE [Id] IN({2}) AND [CompanyId]={3}", sellerId
                , sellerName
                , EyouSoft.Toolkit.Utils.GetSqlIdStrByList(customers)
                , companyId);
            DbCommand cmd = _db.GetSqlStringCommand(cmdText);

            return DbHelper.ExecuteSql(cmd, _db) > 0 ? true : false;            
        }

        /// <summary>
        /// 获取组团社已欠款金额及最高欠款金额
        /// </summary>
        /// <param name="customerId">客户单位编号</param>
        /// <param name="debtAmount">已欠款金额</param>
        /// <param name="maxDebtAmount">最高欠款金额</param>
        public void GetCustomerDebt(int customerId, out decimal debtAmount, out decimal maxDebtAmount)
        {
            debtAmount = 0;
            maxDebtAmount = 0;
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_GetCustomerDebt);
            _db.AddInParameter(cmd, "CustomerId", DbType.Int32, customerId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) maxDebtAmount = rdr.GetDecimal(0);
                }

                rdr.NextResult();

                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) debtAmount = rdr.GetDecimal(0);
                }
            }
        }
    }
}
