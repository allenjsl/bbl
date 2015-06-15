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
    /// 公司客户等级DAL
    /// Author xuqh 2011-01-22
    /// </summary>
    public class CompanyCustomStand : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.CompanyStructure.ICompanyCustomStand
    {
        #region static constants
        private const string SQL_SELECT_ISEXISTS = "select count(*) from tbl_CompanyCustomStand where CustomStandName = @CustomStandName and CompanyId = @CompanyId and Id != @Id";
        private const string SQL_INSERT_CustomStand = "insert into tbl_CompanyCustomStand (CustomStandName,CompanyId,OperatorId,IsSystem,LevType,IsDelete,SalerCommision,LogisticsCommision) values(@CustomStandName,@CompanyId,@OperatorId,@IsSystem,@LevType,'0',@SalerCommision,@LogisticsCommision)";
        private const string SQL_UPDATE_CustomStand = "update tbl_CompanyCustomStand set CustomStandName = @CustomStandName,LevType=@LevType,SalerCommision = @SalerCommision,LogisticsCommision = @LogisticsCommision where Id = @Id";
        private const string SQL_SELECT_CustomStand = "select Id,CustomStandName,CompanyId,OperatorId,IssueTime,IsSystem,LevType,IsDelete,SalerCommision,LogisticsCommision from tbl_CompanyCustomStand where Id = @Id";
        private const string SQL_DELETE_CustomStand = "update tbl_CompanyCustomStand set IsDelete = '1' ";
        private const string SQL_GetCustomStandByCompanyId = "select Id,CustomStandName,CompanyId,OperatorId,IssueTime,IsSystem,LevType,IsDelete,SalerCommision,LogisticsCommision from tbl_CompanyCustomStand where CompanyId = @CompanyId and IsDelete = '0'";
        private readonly Database _db = null;
        #endregion

        #region 构造函数
        public CompanyCustomStand()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region ICompanyCustomStand 成员

        /// <summary>
        /// 验证是否已经存在同名的客户等级
        /// </summary>
        /// <param name="CustomStandName">客户等级名称</param>
        /// <param name="Id">主键编号</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>true:存在 false:不存在</returns>
        public bool IsExists(string CustomStandName, int Id, int CompanyId)
        {
            bool isExists = false;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_ISEXISTS);

            this._db.AddInParameter(cmd, "CustomStandName", DbType.String, CustomStandName);
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
        /// <param name="model">客户等级实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.CustomStand model)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_INSERT_CustomStand);

            this._db.AddInParameter(cmd, "CustomStandName", DbType.String, model.CustomStandName);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(cmd, "IsSystem", DbType.String, model.IsSystem == true ? "1" : "0");
            this._db.AddInParameter(cmd, "LevType", DbType.Byte, (int)model.LevType);
            this._db.AddInParameter(cmd, "SalerCommision", DbType.Decimal, model.SalerCommision);
            this._db.AddInParameter(cmd, "LogisticsCommision", DbType.Decimal, model.LogisticsCommision);
            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">客户等级实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.CompanyStructure.CustomStand model)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_UPDATE_CustomStand);
            this._db.AddInParameter(cmd, "CustomStandName", DbType.String, model.CustomStandName);
            this._db.AddInParameter(cmd, "LevType", DbType.Byte, (int)model.LevType);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, model.Id);
            this._db.AddInParameter(cmd, "SalerCommision", DbType.Decimal, model.SalerCommision);
            this._db.AddInParameter(cmd, "LogisticsCommision", DbType.Decimal, model.LogisticsCommision);

            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取客户等级实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CustomStand GetModel(int Id)
        {
            EyouSoft.Model.CompanyStructure.CustomStand customStandModel = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_CustomStand);
            this._db.AddInParameter(cmd, "Id", DbType.Int32, Id);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    customStandModel = new EyouSoft.Model.CompanyStructure.CustomStand();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Id")))
                        customStandModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("CustomStandName")))
                        customStandModel.CustomStandName = rdr.GetString(rdr.GetOrdinal("CustomStandName"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("CompanyId")))
                        customStandModel.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("OperatorId")))
                        customStandModel.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("LevType")))
                        customStandModel.LevType = (EyouSoft.Model.EnumType.CompanyStructure.CustomLevType)int.Parse(rdr[rdr.GetOrdinal("LevType")].ToString());
                    if (!rdr.IsDBNull(rdr.GetOrdinal("IssueTime")))
                        customStandModel.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("IsSystem")))
                        customStandModel.IsSystem = rdr[(rdr.GetOrdinal("IsSystem"))].ToString() == "1" ? true : false;
                    if (!rdr.IsDBNull(rdr.GetOrdinal("IsDelete")))
                        customStandModel.IsDelete = rdr[rdr.GetOrdinal("IsDelete")].ToString() == "1" ? true : false;
                    if (!rdr.IsDBNull(rdr.GetOrdinal("SalerCommision")))
                        customStandModel.SalerCommision = rdr.GetDecimal(rdr.GetOrdinal("SalerCommision"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("LogisticsCommision")))
                        customStandModel.LogisticsCommision = rdr.GetDecimal(rdr.GetOrdinal("LogisticsCommision"));
                }
            }

            return customStandModel;
        }

        /// <summary>
        /// 删除客户等级
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

            DbCommand dc = _db.GetSqlStringCommand(SQL_DELETE_CustomStand + " where IsSystem='0' and Id in (" + strIds + ");");
            return EyouSoft.Toolkit.DAL.DbHelper.ExecuteSql(dc, _db) > 0 ? true:false;
        }

        /// <summary>
        /// 分页获取公司客户等级集合
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>公司客户等级集合</returns>
        public IList<EyouSoft.Model.CompanyStructure.CustomStand> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId)
        {
            IList<EyouSoft.Model.CompanyStructure.CustomStand> totals = new List<EyouSoft.Model.CompanyStructure.CustomStand>();

            string tableName = "tbl_CompanyCustomStand";
            string primaryKey = "Id";
            string orderByString = "IssueTime DESC";
            string fields = " Id,CustomStandName,LevType,CompanyId,OperatorId,IssueTime,IsSystem,IsDelete,SalerCommision,LogisticsCommision ";

            StringBuilder cmdQuery = new StringBuilder(" IsDelete='0' ");
            cmdQuery.AppendFormat(" and CompanyId='{0}' ", CompanyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(this._db, PageSize, PageIndex, ref RecordCount, tableName, primaryKey, fields, cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.CompanyStructure.CustomStand customStandInfo = new EyouSoft.Model.CompanyStructure.CustomStand();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Id")))
                        customStandInfo.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("CustomStandName")))
                        customStandInfo.CustomStandName = rdr.GetString(rdr.GetOrdinal("CustomStandName"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("CompanyId")))
                        customStandInfo.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("OperatorId")))
                        customStandInfo.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("LevType")))
                        customStandInfo.LevType = (EyouSoft.Model.EnumType.CompanyStructure.CustomLevType)int.Parse(rdr[rdr.GetOrdinal("LevType")].ToString());
                    if (!rdr.IsDBNull(rdr.GetOrdinal("IssueTime")))
                        customStandInfo.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("IsSystem")))
                        customStandInfo.IsSystem = rdr[(rdr.GetOrdinal("IsSystem"))].ToString() == "1" ? true : false;
                    if (!rdr.IsDBNull(rdr.GetOrdinal("IsDelete")))
                        customStandInfo.IsDelete = rdr[rdr.GetOrdinal("IsDelete")].ToString() == "1" ? true : false;
                    if (!rdr.IsDBNull(rdr.GetOrdinal("SalerCommision")))
                        customStandInfo.SalerCommision = rdr.GetDecimal(rdr.GetOrdinal("SalerCommision"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("LogisticsCommision")))
                        customStandInfo.LogisticsCommision = rdr.GetDecimal(rdr.GetOrdinal("LogisticsCommision"));

                    totals.Add(customStandInfo);
                }
            }

            return totals;
        }

        /// <summary>
        /// 根据公司编号获取客户等级信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CustomStand> GetCustomStandByCompanyId(int companyId)
        {
            IList<EyouSoft.Model.CompanyStructure.CustomStand> totals = new List<EyouSoft.Model.CompanyStructure.CustomStand>();
            EyouSoft.Model.CompanyStructure.CustomStand model = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_GetCustomStandByCompanyId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);

            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    model = new EyouSoft.Model.CompanyStructure.CustomStand();
                    model.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    model.CustomStandName = rdr.GetString(rdr.GetOrdinal("CustomStandName"));
                    model.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    model.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                    model.LevType = (EyouSoft.Model.EnumType.CompanyStructure.CustomLevType)int.Parse(rdr[rdr.GetOrdinal("LevType")].ToString());
                    model.IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    model.IsSystem = rdr[(rdr.GetOrdinal("IsSystem"))].ToString() == "1" ? true : false;
                    model.IsDelete = rdr[rdr.GetOrdinal("IsDelete")].ToString() == "1" ? true : false;
                    if (!rdr.IsDBNull(rdr.GetOrdinal("SalerCommision")))
                        model.SalerCommision = rdr.GetDecimal(rdr.GetOrdinal("SalerCommision"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("LogisticsCommision")))
                        model.LogisticsCommision = rdr.GetDecimal(rdr.GetOrdinal("LogisticsCommision"));

                    totals.Add(model);
                }
            }

            return totals;
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
            string isSysSql = string.Format("select stuff((select ',' + IsSystem from tbl_CompanyCustomStand  where Id in ({0}) and IsDelete = '0' for xml path('')),1,1,'') as IsSys",
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
