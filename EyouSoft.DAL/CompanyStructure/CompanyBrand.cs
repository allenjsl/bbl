using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace EyouSoft.DAL.CompanyStructure
{
    /// <summary>
    /// 公司品牌DAL
    /// Author xuqh 2011-01-22
    /// </summary>
    public class CompanyBrand : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.ICompanyBrand
    {
        #region static constants
        private const string SQL_INSERT_CompanyBrand = "insert into tbl_CompanyBrands (BrandName,Logo1,Logo2,OperatorId,CompanyId,IsDelete) values(@BrandName,@Logo1,@Logo2,@OperatorId,@CompanyId,@IsDelete)";
        //private const string SQL_UPDATE_CompanyBrand = "update tbl_CompanyBrands set BrandName = @BrandName,Logo1=@Logo1,Logo2=@Logo2 where Id = @Id";
        private const string SQL_UPDATE_CompanyBrand = "update tbl_CompanyBrands ";
        private const string SQL_SELECT_CompanyBrand = "select Id,BrandName,Logo1,Logo2,OperatorId,IssueTime,CompanyId,IsDelete from tbl_CompanyBrands where Id = @Id";
        private const string SQL_DELETE_CompanyBrand = "update tbl_CompanyBrands set IsDelete = '1' ";
        private const string SQL_GetBrandByCompanyId = "select Id,BrandName,Logo1,Logo2,OperatorId,IssueTime,CompanyId,IsDelete from tbl_CompanyBrands where CompanyId = @CompanyId and IsDelete = '0'";
        private readonly Database _db = null;
        #endregion

        #region 构造函数
        public CompanyBrand()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region ICompanyBrand 成员

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">公司产品实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.CompanyBrand model)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_CompanyBrand);

            this._db.AddInParameter(cmd, "BrandName", DbType.String, model.BrandName);
            this._db.AddInParameter(cmd, "Logo1", DbType.String, model.Logo1);
            this._db.AddInParameter(cmd, "Logo2", DbType.String, model.Logo2);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "IsDelete", DbType.String, model.IsDelete == true ? "1" : "0");

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">公司产品实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.CompanyStructure.CompanyBrand model)
        {
            if (model == null || model.Id <= 0)
                return false;

            DbCommand cmd = null;
            StringBuilder updateStr = new StringBuilder();
            updateStr.AppendFormat(" set Logo1 = '{0}' ", string.IsNullOrEmpty(model.Logo1) ? string.Empty : model.Logo1);
            updateStr.AppendFormat(" ,Logo2 = '{0}' ", string.IsNullOrEmpty(model.Logo2) ? string.Empty : model.Logo2);
            updateStr.AppendFormat(" ,BrandName = '{0}' ", string.IsNullOrEmpty(model.BrandName) ? string.Empty : model.BrandName);
            updateStr.AppendFormat(" where Id = {0} ", model.Id);

            cmd = this._db.GetSqlStringCommand(SQL_UPDATE_CompanyBrand + updateStr);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">主键编号集合</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(params int[] Ids)
        {
            if (Ids == null || Ids.Length <= 0)
                return false;

            string strIds = string.Empty;
            foreach (int str in Ids)
            {
                strIds += "'" + str.ToString().Trim() + "',";
            }
            strIds = strIds.Trim(',');

            DbCommand dc = _db.GetSqlStringCommand(SQL_DELETE_CompanyBrand + " where Id in (" + strIds + ");");
            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(dc, _db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取公司产品实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CompanyBrand GetModel(int Id)
        {
            EyouSoft.Model.CompanyStructure.CompanyBrand companyBrandModel = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_CompanyBrand);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, Id);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    companyBrandModel = new EyouSoft.Model.CompanyStructure.CompanyBrand();
                    companyBrandModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    companyBrandModel.BrandName = rdr.GetString(rdr.GetOrdinal("BrandName"));
                    companyBrandModel.Logo1 = rdr.IsDBNull(rdr.GetOrdinal("Logo1")) ? "" : rdr.GetString(rdr.GetOrdinal("Logo1"));
                    companyBrandModel.Logo2 = rdr.IsDBNull(rdr.GetOrdinal("Logo2")) ? "" : rdr.GetString(rdr.GetOrdinal("Logo2"));
                    companyBrandModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    companyBrandModel.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    companyBrandModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    companyBrandModel.IsDelete = Convert.ToBoolean(rdr.GetOrdinal("IsDelete"));
                }
            }

            return companyBrandModel;
        }

        /// <summary>
        /// 分页获取公司产品列表
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>公司产品列表</returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyBrand> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId)
        {
            IList<EyouSoft.Model.CompanyStructure.CompanyBrand> totals = new List<EyouSoft.Model.CompanyStructure.CompanyBrand>();

            string tableName = "tbl_CompanyBrands";
            string primaryKey = "Id";
            string orderByString = "IssueTime DESC";
            string fields = " Id,BrandName,Logo1,Logo2,OperatorId,IssueTime,CompanyId,IsDelete";

            StringBuilder cmdQuery = new StringBuilder(" IsDelete='0' ");
            cmdQuery.AppendFormat(" and CompanyId='{0}' ", CompanyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(this._db, PageSize, PageIndex, ref RecordCount, tableName, primaryKey, fields, cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.CompanyStructure.CompanyBrand companyBrandInfo = new EyouSoft.Model.CompanyStructure.CompanyBrand();

                    companyBrandInfo.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    companyBrandInfo.BrandName = rdr.GetString(rdr.GetOrdinal("BrandName"));
                    companyBrandInfo.Logo1 = rdr.IsDBNull(rdr.GetOrdinal("Logo1")) ? "" : rdr.GetString(rdr.GetOrdinal("Logo1"));
                    companyBrandInfo.Logo2 = rdr.IsDBNull(rdr.GetOrdinal("Logo2")) ? "" : rdr.GetString(rdr.GetOrdinal("Logo2"));
                    companyBrandInfo.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    companyBrandInfo.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    companyBrandInfo.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    companyBrandInfo.IsDelete = Convert.ToBoolean(rdr.GetOrdinal("IsDelete"));

                    totals.Add(companyBrandInfo);
                }
            }

            return totals;
        }

        /// <summary>
        /// 根据公司ID获取公司品牌信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyBrand> GetBrandByCompanyId(int companyId)
        {
            IList<EyouSoft.Model.CompanyStructure.CompanyBrand> totals = new List<EyouSoft.Model.CompanyStructure.CompanyBrand>();
            EyouSoft.Model.CompanyStructure.CompanyBrand model = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_GetBrandByCompanyId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    model = new EyouSoft.Model.CompanyStructure.CompanyBrand();
                    model.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    model.BrandName = rdr.GetString(rdr.GetOrdinal("BrandName"));
                    model.Logo1 = rdr.IsDBNull(rdr.GetOrdinal("Logo1")) ? "" : rdr.GetString(rdr.GetOrdinal("Logo1"));
                    model.Logo2 = rdr.IsDBNull(rdr.GetOrdinal("Logo2")) ? "" : rdr.GetString(rdr.GetOrdinal("Logo2"));
                    model.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    model.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    model.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    model.IsDelete = Convert.ToBoolean(rdr.GetOrdinal("IsDelete"));
                    totals.Add(model);
                }
            }

            return totals;
        }

        #endregion
    }
}
