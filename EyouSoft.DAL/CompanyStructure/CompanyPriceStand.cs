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
    /// 公司报价等级DAL
    /// Author xuqh 2011-01-22
    /// </summary>
    public class CompanyPriceStand : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.ICompanyPriceStand
    {
        #region static constants
        private const string SQL_SELECT_ISEXISTS = "select count(*) from tbl_CompanyPriceStand where PriceStandName = @PriceStandName and CompanyId = @CompanyId and Id != @Id";
        private const string SQL_INSERT_CompanyPriceStand = "insert into tbl_CompanyPriceStand (PriceStandName,CompanyId,OperatorId,IsDelete,IsSystem) values(@PriceStandName,@CompanyId,@OperatorId,'0','0')";
        private const string SQL_UPDATE_CompanyPriceStand = "update tbl_CompanyPriceStand set PriceStandName = @PriceStandName where Id = @Id";
        private const string SQL_SELECT_CompanyPriceStand = "select Id,PriceStandName,CompanyId,OperatorId,IssueTime,IsDelete,IsSystem from tbl_CompanyPriceStand where Id = @Id";
        private const string SQL_DELETE_CompanyPriceStand = "update tbl_CompanyPriceStand set IsDelete = '1' ";
        private const string SQL_GetPriceStandByCompanyId = "select Id,PriceStandName,CompanyId,OperatorId,IssueTime,IsDelete,IsSystem from tbl_CompanyPriceStand where IsDelete = '0' and CompanyId = @CompanyId";
        private readonly Database _db = null;
        #endregion

        #region 构造函数
        public CompanyPriceStand()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region ICompanyPriceStand 成员

        /// <summary>
        /// 验证是否已经存在同名的报价等级
        /// </summary>
        /// <param name="PriceStandName">报价等级名称</param>
        /// <param name="Id">主键编号</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>true:存在 false:不存在</returns>
        public bool IsExists(string PriceStandName, int Id, int CompanyId)
        {
            bool isExists = false;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_ISEXISTS);

            this._db.AddInParameter(cmd, "PriceStandName", DbType.String, PriceStandName);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, Id);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, CompanyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    isExists = rdr.GetInt32(0) > 0 ? true : false;
                }
            }

            return isExists;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">报价等级实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.CompanyPriceStand model)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_CompanyPriceStand);

            this._db.AddInParameter(cmd, "PriceStandName", DbType.String, model.PriceStandName);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);
            //this._db.AddInParameter(cmd, "IsDelete", DbType.String, model.IsDelete == true ? "1" : "0");

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">报价等级实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.CompanyStructure.CompanyPriceStand model)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_UPDATE_CompanyPriceStand);
            this._db.AddInParameter(cmd, "PriceStandName", DbType.String, model.PriceStandName);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, model.Id);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取报价等级实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CompanyPriceStand GetModel(int Id)
        {
            EyouSoft.Model.CompanyStructure.CompanyPriceStand companyPriceModel = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_CompanyPriceStand);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, Id);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    companyPriceModel = new EyouSoft.Model.CompanyStructure.CompanyPriceStand();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Id")))
                        companyPriceModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("PriceStandName")))
                        companyPriceModel.PriceStandName = rdr.GetString(rdr.GetOrdinal("PriceStandName"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("CompanyId")))
                        companyPriceModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("OperatorId")))
                        companyPriceModel.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("IssueTime")))
                        companyPriceModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("IsDelete")))
                        companyPriceModel.IsDelete = rdr[rdr.GetOrdinal("IsDelete")].ToString() == "1" ? true : false;
                    if (!rdr.IsDBNull(rdr.GetOrdinal("IsSystem")))
                        companyPriceModel.IsSystem = rdr[rdr.GetOrdinal("IsSystem")].ToString() == "1" ? true : false;
                }
            }

            return companyPriceModel;
        }

        /// <summary>
        /// 删除报价等级
        /// </summary>
        /// <param name="Ids">主键编号</param>
        /// <returns>true成功 false失败</returns>
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

            DbCommand dc = _db.GetSqlStringCommand(SQL_DELETE_CompanyPriceStand + " where IsSystem = '0' and Id in (" + strIds + ");");
            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(dc, _db) > 0 ? true:false;
        }


        /// <summary>
        /// 分页获取公司报价等级集合
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>公司报价等级集合</returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyPriceStand> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId)
        {
            IList<EyouSoft.Model.CompanyStructure.CompanyPriceStand> totals = new List<EyouSoft.Model.CompanyStructure.CompanyPriceStand>();

            string tableName = "tbl_CompanyPriceStand";
            string primaryKey = "Id";
            string orderByString = "IssueTime DESC";
            string fields = " Id, PriceStandName, CompanyId, OperatorId, IssueTime,IsDelete,IsSystem";

            StringBuilder cmdQuery = new StringBuilder(" IsDelete='0' ");
            cmdQuery.AppendFormat(" and CompanyId='{0}' ", CompanyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(this._db, PageSize, PageIndex, ref RecordCount, tableName, primaryKey, fields, cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.CompanyStructure.CompanyPriceStand companyPriceStandInfo = new EyouSoft.Model.CompanyStructure.CompanyPriceStand();

                    if (!rdr.IsDBNull(rdr.GetOrdinal("Id")))
                        companyPriceStandInfo.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("PriceStandName")))
                        companyPriceStandInfo.PriceStandName = rdr.GetString(rdr.GetOrdinal("PriceStandName"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("CompanyId")))
                        companyPriceStandInfo.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("OperatorId")))
                        companyPriceStandInfo.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("IssueTime")))
                        companyPriceStandInfo.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("IsDelete")))
                        companyPriceStandInfo.IsDelete = rdr[rdr.GetOrdinal("IsDelete")].ToString() == "1" ? true : false;
                    if (!rdr.IsDBNull(rdr.GetOrdinal("IsSystem")))
                        companyPriceStandInfo.IsSystem = rdr[rdr.GetOrdinal("IsSystem")].ToString() == "1" ? true : false;

                    totals.Add(companyPriceStandInfo);
                }
            }

            return totals;
        }

        /// <summary>
        /// 获取某公司所有报价等级信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyPriceStand> GetPriceStandByCompanyId(int companyId)
        {
            IList<EyouSoft.Model.CompanyStructure.CompanyPriceStand> totals = new List<EyouSoft.Model.CompanyStructure.CompanyPriceStand>();
            EyouSoft.Model.CompanyStructure.CompanyPriceStand model = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_GetPriceStandByCompanyId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    model = new EyouSoft.Model.CompanyStructure.CompanyPriceStand();
                    model.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    model.PriceStandName = rdr.GetString(rdr.GetOrdinal("PriceStandName"));
                    model.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    model.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    model.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    model.IsDelete = Convert.ToBoolean(rdr.GetOrdinal("IsDelete"));
                    model.IsSystem = Convert.ToBoolean(rdr.GetOrdinal("IsSystem"));
                    totals.Add(model);
                }
            }

            return totals;
        }

        /// <summary>
        /// 判断报价标准或客户等级是否被使用过
        /// </summary>
        /// <param name="id">报价标准或客户等级</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="typeId">1是标价标准 0是客户等级</param>
        /// <returns></returns>
        public bool IsUsed(int id, int companyId, int typeId)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_JudgeCustomOrPriceStand");
            this._db.AddInParameter(cmd, "Id", DbType.Int32, id);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);
            this._db.AddInParameter(cmd, "IsPriceStand", DbType.Int32, typeId);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            EyouSoft.Toolkit.DAL.DbHelper.RunProcedure(cmd, this._db);
            object obj = this._db.GetParameterValue(cmd, "Result");
            return Convert.ToInt32(obj) > 0 ? true : false;
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 是否系统默认
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private string IsSysDefault(string ids)
        {
            string str = string.Empty;
            string isSysSql = string.Format("select stuff((select ',' + IsSystem from tbl_CompanyPriceStand  where Id in ({0}) and IsDelete = '0' for xml path('')),1,1,'') as IsSys",
                ids);
            DbCommand cmd = this._db.GetSqlStringCommand(isSysSql);
            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    str = rdr.IsDBNull(rdr.GetOrdinal("IsSys")) ? "" : rdr.GetString(rdr.GetOrdinal("IsSys"));
                }
            }
            return str;
        }
        #endregion
    }
}
