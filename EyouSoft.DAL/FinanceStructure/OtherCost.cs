using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data;

namespace EyouSoft.DAL.FinanceStructure
{
    /// <summary>
    /// 杂费收入数据访问类
    /// </summary>
    /// 周文超 2011-01-20
    public class OtherCost : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.FinanceStructure.IOtherCost
    {
        #region 私有变量

        private readonly Database _db = null;

        private const string Sql_OtherCost_Update = @" UPDATE [tbl_TourOtherCost]
   SET [IncreaseCost] = {0}
      ,[ReduceCost] = {1}
      ,[SumCost] = {2}
      ,[Remark] = '{3}'
      where [Id] = '{4}'; ";
        private const string Sql_OtherCost_UpdateAll = @" UPDATE [tbl_TourOtherCost]
   SET [CustromCName] = @CustromCName
      ,[CustromCId] = @CustromCId
      ,[ProceedItem] = @ProceedItem
      ,[Proceed] = @Proceed
      ,[IncreaseCost] = @IncreaseCost
      ,[ReduceCost] = @ReduceCost
      ,[SumCost] = @SumCost
      ,[Remark] = @Remark
      ,[PayTime] = @PayTime
      ,[Payee] = @Payee
      ,[PayType] = @PayType
      ,[Status] = @Status
 WHERE Id = @Id ";
        private const string Sql_OtherCost_Delete = @" DELETE FROM [tbl_TourOtherCost] ";
        private const string Sql_OtherCost_Select = @" SELECT 
      [Id]
      ,[CompanyId]
      ,[TourId]
      ,[CostType]
      ,[CustromCName]
      ,[CustromCId]
      ,[ProceedItem]
      ,[Proceed]
      ,[IncreaseCost]
      ,[ReduceCost]
      ,[SumCost]
      ,[Remark]
      ,[OperatorId]
      ,[CreateTime]
      ,[PayTime]
      ,[Payee]
      ,[PayType]
      ,[Status]
  FROM [tbl_TourOtherCost] ";
        private const string Sql_OtherCost_Insert = @" INSERT INTO [tbl_TourOtherCost] ([Id],[CompanyId],[TourId],[CostType],[CustromCName],[CustromCId],[ProceedItem],[Proceed],[IncreaseCost],[ReduceCost],[SumCost],[Remark],[OperatorId],[CreateTime],[PayTime],[Payee],[PayType],[Status]) VALUES ('{0}',{1},'{2}',{3},'{4}',{5},'{6}',{7},{8},{9},{10},'{11}',{12},'{13}','{14}','{15}',{16},'{17}'); ";

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public OtherCost()
        {
            _db = this.SystemStore;
        }

        #endregion

        #region IOtherCost 成员

        /// <summary>
        /// 设置杂费收入（支出）的状态
        /// </summary>
        /// <param name="status">支出（收入）状态</param>
        /// <param name="otherCostId">杂费收入（支出）Id</param>
        /// <returns>1：操作成功；其他失败</returns>
        public int SetOtherCostStatus(bool status, params string[] otherCostId)
        {
            if (otherCostId == null || otherCostId.Length <= 0)
                return 0;

            var strSql = new StringBuilder();
            strSql.AppendFormat(" UPDATE [tbl_TourOtherCost] SET [Status] = '{0}' where ", status ? "1" : "0");
            if (otherCostId.Length == 1)
                strSql.AppendFormat(" ID = '{0}' ", otherCostId[0]);
            else
            {
                string strTmp = otherCostId.Aggregate(string.Empty, (current, s) => current + "'" + s + "'" + ",");
                strTmp = strTmp.TrimEnd(',');
                strSql.AppendFormat(" ID in ({0}) ", strTmp);
            }
            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 添加杂费收入信息
        /// </summary>
        /// <param name="list">杂费收入信息实体集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        public int AddOtherIncome(IList<Model.FinanceStructure.OtherIncomeInfo> list)
        {
            if (list == null || list.Count <= 0)
                return 0;

            StringBuilder strSql = new StringBuilder();
            foreach (Model.FinanceStructure.OtherIncomeInfo t in list)
            {
                if (t == null)
                    continue;

                t.IncomeId = Guid.NewGuid().ToString();
                strSql.AppendFormat(Sql_OtherCost_Insert, t.IncomeId, t.CompanyId, t.TourId
                                    , (int)EyouSoft.Model.EnumType.FinanceStructure.CostType.收入, t.CustromCName,
                                    t.CustromCId, t.Item
                                    , t.Amount, t.AddAmount, t.ReduceAmount, t.TotalAmount, t.Remark, t.OperatorId,
                                    DateTime.Now, t.PayTime
                                    , t.Payee, (int)t.PayType, t.Status ? "1" : "0");
            }

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 添加杂费支出信息
        /// </summary>
        /// <param name="list">杂费支出信息实体集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        public int AddOtherOut(IList<Model.FinanceStructure.OtherOutInfo> list)
        {
            if (list == null || list.Count <= 0)
                return 0;

            StringBuilder strSql = new StringBuilder();
            foreach (Model.FinanceStructure.OtherOutInfo t in list)
            {
                if (t == null)
                    continue;

                t.OutId = Guid.NewGuid().ToString();
                strSql.AppendFormat(Sql_OtherCost_Insert, t.OutId, t.CompanyId, t.TourId
                    , (int)EyouSoft.Model.EnumType.FinanceStructure.CostType.支出, t.CustromCName, t.CustromCId, t.Item
                    , t.Amount, t.AddAmount, t.ReduceAmount, t.TotalAmount, t.Remark, t.OperatorId, DateTime.Now, t.PayTime
                    , t.Payee, (int)t.PayType, t.Status ? "1" : "0");
            }

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 修改杂费收入信息（只修改增加、减少、小计、备注）
        /// </summary>
        /// <param name="list">杂费收入信息集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        public int UpdateOtherIncome(IList<EyouSoft.Model.FinanceStructure.OtherIncomeInfo> list)
        {
            if (list == null || list.Count <= 0)
                return 0;

            StringBuilder strSql = new StringBuilder();
            foreach (EyouSoft.Model.FinanceStructure.OtherIncomeInfo tmp in list)
            {
                strSql.AppendFormat(Sql_OtherCost_Update, tmp.AddAmount, tmp.ReduceAmount, tmp.TotalAmount, tmp.Remark, tmp.IncomeId);
            }
            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 修改杂费收入信息（修改所有值）
        /// </summary>
        /// <param name="model">杂费收入信息集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        public int UpdateOtherIncome(Model.FinanceStructure.OtherIncomeInfo model)
        {
            if (model == null)
                return 0;

            DbCommand dc = _db.GetSqlStringCommand(Sql_OtherCost_UpdateAll);
            _db.AddInParameter(dc, "CustromCName", DbType.String, model.CustromCName);
            _db.AddInParameter(dc, "CustromCId", DbType.Int32, model.CustromCId);
            _db.AddInParameter(dc, "ProceedItem", DbType.String, model.Item);
            _db.AddInParameter(dc, "Proceed", DbType.Decimal, model.Amount);
            _db.AddInParameter(dc, "IncreaseCost", DbType.Decimal, model.AddAmount);
            _db.AddInParameter(dc, "ReduceCost", DbType.Decimal, model.ReduceAmount);
            _db.AddInParameter(dc, "SumCost", DbType.Decimal, model.TotalAmount);
            _db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            _db.AddInParameter(dc, "PayTime", DbType.DateTime, model.PayTime);
            _db.AddInParameter(dc, "Payee", DbType.String, model.Payee);
            _db.AddInParameter(dc, "PayType", DbType.Byte, (int)model.PayType);
            _db.AddInParameter(dc, "Status", DbType.AnsiStringFixedLength, model.Status ? "1" : "0");
            _db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, model.IncomeId);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 修改杂费支出信息（只修改增加、减少、小计、备注）
        /// </summary>
        /// <param name="list">支出信息集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        public int UpdateOtherOut(IList<EyouSoft.Model.FinanceStructure.OtherOutInfo> list)
        {
            if (list == null || list.Count <= 0)
                return 0;

            StringBuilder strSql = new StringBuilder();
            foreach (EyouSoft.Model.FinanceStructure.OtherOutInfo tmp in list)
            {
                strSql.AppendFormat(Sql_OtherCost_Update, tmp.AddAmount, tmp.ReduceAmount, tmp.TotalAmount, tmp.Remark, tmp.OutId);
            }
            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 修改杂费支出信息（修改所有值）
        /// </summary>
        /// <param name="model">支出信息集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        public int UpdateOtherOut(Model.FinanceStructure.OtherOutInfo model)
        {
            if (model == null)
                return 0;

            DbCommand dc = _db.GetSqlStringCommand(Sql_OtherCost_UpdateAll);
            _db.AddInParameter(dc, "CustromCName", DbType.String, model.CustromCName);
            _db.AddInParameter(dc, "CustromCId", DbType.Int32, model.CustromCId);
            _db.AddInParameter(dc, "ProceedItem", DbType.String, model.Item);
            _db.AddInParameter(dc, "Proceed", DbType.Decimal, model.Amount);
            _db.AddInParameter(dc, "IncreaseCost", DbType.Decimal, model.AddAmount);
            _db.AddInParameter(dc, "ReduceCost", DbType.Decimal, model.ReduceAmount);
            _db.AddInParameter(dc, "SumCost", DbType.Decimal, model.TotalAmount);
            _db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            _db.AddInParameter(dc, "PayTime", DbType.DateTime, model.PayTime);
            _db.AddInParameter(dc, "Payee", DbType.String, model.Payee);
            _db.AddInParameter(dc, "PayType", DbType.Byte, (int)model.PayType);
            _db.AddInParameter(dc, "Status", DbType.AnsiStringFixedLength, model.Status ? "1" : "0");
            _db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, model.OutId);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 删除杂费（收入或者支出）
        /// </summary>
        /// <param name="OtherCostIds">杂费Id集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        public int DeleteOtherCost(string[] OtherCostIds)
        {
            if (OtherCostIds == null || OtherCostIds.Length <= 0)
                return 0;

            string strIds = string.Empty;
            foreach (string str in OtherCostIds)
            {
                if (string.IsNullOrEmpty(str))
                    continue;

                strIds += "'" + str.Trim() + "',";
            }
            strIds = strIds.Trim(',');

            DbCommand dc = _db.GetSqlStringCommand(Sql_OtherCost_Delete + " where Id in (" + strIds + ");");
            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 获取杂费收入信息
        /// </summary>
        /// <param name="OtherIncomeId">杂费收入信息Id</param>
        /// <returns>杂费收入信息</returns>
        public EyouSoft.Model.FinanceStructure.OtherIncomeInfo GetOtherIncomeInfo(string OtherIncomeId)
        {
            if (string.IsNullOrEmpty(OtherIncomeId))
                return null;

            DbCommand dc = _db.GetSqlStringCommand(Sql_OtherCost_Select + " where Id = @Id ");
            _db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, OtherIncomeId);

            EyouSoft.Model.FinanceStructure.OtherIncomeInfo rModel = new EyouSoft.Model.FinanceStructure.OtherIncomeInfo();
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    rModel.IncomeId = dr["Id"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("CompanyId")))
                        rModel.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    rModel.TourId = dr["TourId"].ToString();
                    rModel.CustromCName = dr["CustromCName"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("CustromCId")))
                        rModel.CustromCId = dr.GetInt32(dr.GetOrdinal("CustromCId"));
                    rModel.Item = dr["ProceedItem"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("Proceed")))
                        rModel.Amount = dr.GetDecimal(dr.GetOrdinal("Proceed"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IncreaseCost")))
                        rModel.AddAmount = dr.GetDecimal(dr.GetOrdinal("IncreaseCost"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ReduceCost")))
                        rModel.ReduceAmount = dr.GetDecimal(dr.GetOrdinal("ReduceCost"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SumCost")))
                        rModel.TotalAmount = dr.GetDecimal(dr.GetOrdinal("SumCost"));
                    rModel.Remark = dr["Remark"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorId")))
                        rModel.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CreateTime")))
                        rModel.CreateTime = dr.GetDateTime(dr.GetOrdinal("CreateTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PayTime")))
                        rModel.PayTime = dr.GetDateTime(dr.GetOrdinal("PayTime"));
                    rModel.Payee = dr["Payee"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("PayType")))
                        rModel.PayType = (EyouSoft.Model.EnumType.TourStructure.RefundType)dr.GetByte(dr.GetOrdinal("PayType"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Status")))
                    {
                        if (dr.GetString(dr.GetOrdinal("Status")) == "1" || dr.GetString(dr.GetOrdinal("Status")).ToLower() == "true")
                            rModel.Status = true;
                        else
                            rModel.Status = false;
                    }
                }
            }

            return rModel;
        }

        /// <summary>
        /// 获取杂费支出信息
        /// </summary>
        /// <param name="OtherOutId">杂费支出信息Id</param>
        /// <returns>杂费支出信息</returns>
        public EyouSoft.Model.FinanceStructure.OtherOutInfo GetOtherOutInfo(string OtherOutId)
        {
            if (string.IsNullOrEmpty(OtherOutId))
                return null;

            DbCommand dc = _db.GetSqlStringCommand(Sql_OtherCost_Select + " where Id = @Id ");
            _db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, OtherOutId);

            EyouSoft.Model.FinanceStructure.OtherOutInfo rModel = new EyouSoft.Model.FinanceStructure.OtherOutInfo();
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    rModel.OutId = dr["Id"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("CompanyId")))
                        rModel.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    rModel.TourId = dr["TourId"].ToString();
                    rModel.CustromCName = dr["CustromCName"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("CustromCId")))
                        rModel.CustromCId = dr.GetInt32(dr.GetOrdinal("CustromCId"));
                    rModel.Item = dr["ProceedItem"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("Proceed")))
                        rModel.Amount = dr.GetDecimal(dr.GetOrdinal("Proceed"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IncreaseCost")))
                        rModel.AddAmount = dr.GetDecimal(dr.GetOrdinal("IncreaseCost"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ReduceCost")))
                        rModel.ReduceAmount = dr.GetDecimal(dr.GetOrdinal("ReduceCost"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SumCost")))
                        rModel.TotalAmount = dr.GetDecimal(dr.GetOrdinal("SumCost"));
                    rModel.Remark = dr["Remark"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorId")))
                        rModel.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CreateTime")))
                        rModel.CreateTime = dr.GetDateTime(dr.GetOrdinal("CreateTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PayTime")))
                        rModel.PayTime = dr.GetDateTime(dr.GetOrdinal("PayTime"));
                    rModel.Payee = dr["Payee"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("PayType")))
                        rModel.PayType = (EyouSoft.Model.EnumType.TourStructure.RefundType)dr.GetByte(dr.GetOrdinal("PayType"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Status")))
                    {
                        if (dr.GetString(dr.GetOrdinal("Status")) == "1" || dr.GetString(dr.GetOrdinal("Status")).ToLower() == "true")
                            rModel.Status = true;
                        else
                            rModel.Status = false;
                    }
                }
            }

            return rModel;
        }

        /// <summary>
        /// 获取杂费收入信息
        /// </summary>
        /// <param name="model">杂费信息查询实体</param>
        /// <returns>杂费收入信息集合</returns>
        public IList<EyouSoft.Model.FinanceStructure.OtherIncomeInfo> GetOtherIncomeList(EyouSoft.Model.FinanceStructure.OtherCostQuery model)
        {
            IList<EyouSoft.Model.FinanceStructure.OtherIncomeInfo> list = new List<EyouSoft.Model.FinanceStructure.OtherIncomeInfo>();
            StringBuilder strSql = new StringBuilder(Sql_OtherCost_Select);
            string strOrder = string.Empty;
            strSql.AppendFormat(" where {0} {1} ", GetSqlWhereByQueryModel(model, string.Empty, EyouSoft.Model.EnumType.FinanceStructure.CostType.收入, ref strOrder), string.IsNullOrEmpty(strOrder) ? string.Empty : " order by " + strOrder);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                EyouSoft.Model.FinanceStructure.OtherIncomeInfo rModel = null;
                while (dr.Read())
                {
                    rModel = new EyouSoft.Model.FinanceStructure.OtherIncomeInfo();
                    rModel.IncomeId = dr["Id"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("CompanyId")))
                        rModel.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    rModel.TourId = dr["TourId"].ToString();
                    rModel.CustromCName = dr["CustromCName"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("CustromCId")))
                        rModel.CustromCId = dr.GetInt32(dr.GetOrdinal("CustromCId"));
                    rModel.Item = dr["ProceedItem"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("Proceed")))
                        rModel.Amount = dr.GetDecimal(dr.GetOrdinal("Proceed"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IncreaseCost")))
                        rModel.AddAmount = dr.GetDecimal(dr.GetOrdinal("IncreaseCost"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ReduceCost")))
                        rModel.ReduceAmount = dr.GetDecimal(dr.GetOrdinal("ReduceCost"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SumCost")))
                        rModel.TotalAmount = dr.GetDecimal(dr.GetOrdinal("SumCost"));
                    rModel.Remark = dr["Remark"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorId")))
                        rModel.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CreateTime")))
                        rModel.CreateTime = dr.GetDateTime(dr.GetOrdinal("CreateTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PayTime")))
                        rModel.PayTime = dr.GetDateTime(dr.GetOrdinal("PayTime"));
                    rModel.Payee = dr["Payee"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("PayType")))
                        rModel.PayType = (EyouSoft.Model.EnumType.TourStructure.RefundType)dr.GetByte(dr.GetOrdinal("PayType"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Status")))
                    {
                        if (dr.GetString(dr.GetOrdinal("Status")) == "1" || dr.GetString(dr.GetOrdinal("Status")).ToLower() == "true")
                            rModel.Status = true;
                        else
                            rModel.Status = false;
                    }

                    list.Add(rModel);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取杂费支出信息
        /// </summary>
        /// <param name="model">杂费信息查询实体</param>
        /// <returns>杂费支出信息集合</returns>
        public IList<EyouSoft.Model.FinanceStructure.OtherOutInfo> GetOtherOutList(EyouSoft.Model.FinanceStructure.OtherCostQuery model)
        {
            IList<EyouSoft.Model.FinanceStructure.OtherOutInfo> list = new List<EyouSoft.Model.FinanceStructure.OtherOutInfo>();
            StringBuilder strSql = new StringBuilder(Sql_OtherCost_Select);
            string strOrder = string.Empty;
            strSql.AppendFormat(" where {0} {1} ", GetSqlWhereByQueryModel(model, string.Empty, EyouSoft.Model.EnumType.FinanceStructure.CostType.支出, ref strOrder), string.IsNullOrEmpty(strOrder) ? string.Empty : " order by " + strOrder);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                EyouSoft.Model.FinanceStructure.OtherOutInfo rModel = null;
                while (dr.Read())
                {
                    rModel = new EyouSoft.Model.FinanceStructure.OtherOutInfo();
                    rModel.OutId = dr["Id"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("CompanyId")))
                        rModel.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    rModel.TourId = dr["TourId"].ToString();
                    rModel.CustromCName = dr["CustromCName"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("CustromCId")))
                        rModel.CustromCId = dr.GetInt32(dr.GetOrdinal("CustromCId"));
                    rModel.Item = dr["ProceedItem"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("Proceed")))
                        rModel.Amount = dr.GetDecimal(dr.GetOrdinal("Proceed"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IncreaseCost")))
                        rModel.AddAmount = dr.GetDecimal(dr.GetOrdinal("IncreaseCost"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ReduceCost")))
                        rModel.ReduceAmount = dr.GetDecimal(dr.GetOrdinal("ReduceCost"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SumCost")))
                        rModel.TotalAmount = dr.GetDecimal(dr.GetOrdinal("SumCost"));
                    rModel.Remark = dr["Remark"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorId")))
                        rModel.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CreateTime")))
                        rModel.CreateTime = dr.GetDateTime(dr.GetOrdinal("CreateTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PayTime")))
                        rModel.PayTime = dr.GetDateTime(dr.GetOrdinal("PayTime"));
                    rModel.Payee = dr["Payee"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("PayType")))
                        rModel.PayType = (EyouSoft.Model.EnumType.TourStructure.RefundType)dr.GetByte(dr.GetOrdinal("PayType"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Status")))
                    {
                        if (dr.GetString(dr.GetOrdinal("Status")) == "1" || dr.GetString(dr.GetOrdinal("Status")).ToLower() == "true")
                            rModel.Status = true;
                        else
                            rModel.Status = false;
                    }

                    list.Add(rModel);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取杂费收入信息
        /// </summary>
        /// <param name="model">杂费信息查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns>杂费收入信息集合</returns>
        public IList<EyouSoft.Model.FinanceStructure.OtherIncomeInfo> GetOtherIncomeList(EyouSoft.Model.FinanceStructure.OtherCostQuery model, string HaveUserIds, int PageSize, int PageIndex, ref int RecordCount)
        {
            IList<EyouSoft.Model.FinanceStructure.OtherIncomeInfo> list = new List<EyouSoft.Model.FinanceStructure.OtherIncomeInfo>();
            string strFiles = " [Id],[CompanyId],[TourId],[CostType],[CustromCName],[CustromCId],[ProceedItem],[Proceed],[IncreaseCost],[ReduceCost],[SumCost],[Remark],[OperatorId],[CreateTime],[PayTime],[Payee],[PayType],[Status] ";
            string strOrder = string.Empty;
            string strWhere = this.GetSqlWhereByQueryModel(model, HaveUserIds
                , EyouSoft.Model.EnumType.FinanceStructure.CostType.收入, ref strOrder);

            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, "tbl_TourOtherCost"
                , "Id", strFiles, strWhere, strOrder))
            {
                EyouSoft.Model.FinanceStructure.OtherIncomeInfo tmpModel = null;
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.FinanceStructure.OtherIncomeInfo();
                    tmpModel.IncomeId = dr["Id"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("CompanyId")))
                        tmpModel.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    tmpModel.TourId = dr["TourId"].ToString();
                    tmpModel.CustromCName = dr["CustromCName"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("CustromCId")))
                        tmpModel.CustromCId = dr.GetInt32(dr.GetOrdinal("CustromCId"));
                    tmpModel.Item = dr["ProceedItem"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("Proceed")))
                        tmpModel.Amount = dr.GetDecimal(dr.GetOrdinal("Proceed"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IncreaseCost")))
                        tmpModel.AddAmount = dr.GetDecimal(dr.GetOrdinal("IncreaseCost"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ReduceCost")))
                        tmpModel.ReduceAmount = dr.GetDecimal(dr.GetOrdinal("ReduceCost"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SumCost")))
                        tmpModel.TotalAmount = dr.GetDecimal(dr.GetOrdinal("SumCost"));
                    tmpModel.Remark = dr["Remark"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorId")))
                        tmpModel.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CreateTime")))
                        tmpModel.CreateTime = dr.GetDateTime(dr.GetOrdinal("CreateTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PayTime")))
                        tmpModel.PayTime = dr.GetDateTime(dr.GetOrdinal("PayTime"));
                    tmpModel.Payee = dr["Payee"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("PayType")))
                        tmpModel.PayType = (EyouSoft.Model.EnumType.TourStructure.RefundType)dr.GetByte(dr.GetOrdinal("PayType"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Status")))
                    {
                        if (dr.GetString(dr.GetOrdinal("Status")) == "1" || dr.GetString(dr.GetOrdinal("Status")).ToLower() == "true")
                            tmpModel.Status = true;
                        else
                            tmpModel.Status = false;
                    }

                    list.Add(tmpModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取杂费支出信息
        /// </summary>
        /// <param name="model">杂费信息查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns>杂费支出信息集合</returns>
        public IList<EyouSoft.Model.FinanceStructure.OtherOutInfo> GetOtherOutList(EyouSoft.Model.FinanceStructure.OtherCostQuery model, string HaveUserIds, int PageSize, int PageIndex, ref int RecordCount)
        {
            IList<EyouSoft.Model.FinanceStructure.OtherOutInfo> list = new List<EyouSoft.Model.FinanceStructure.OtherOutInfo>();
            string strFiles = " [Id],[CompanyId],[TourId],[CostType],[CustromCName],[CustromCId],[ProceedItem],[Proceed],[IncreaseCost],[ReduceCost],[SumCost],[Remark],[OperatorId],[CreateTime],[PayTime],[Payee],[PayType],[Status] ";
            string strOrder = string.Empty;
            string strWhere = this.GetSqlWhereByQueryModel(model, HaveUserIds
                , EyouSoft.Model.EnumType.FinanceStructure.CostType.支出, ref strOrder);

            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, "tbl_TourOtherCost"
                , "Id", strFiles, strWhere, strOrder))
            {
                EyouSoft.Model.FinanceStructure.OtherOutInfo tmpModel = null;
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.FinanceStructure.OtherOutInfo();
                    tmpModel.OutId = dr["Id"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("CompanyId")))
                        tmpModel.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    tmpModel.TourId = dr["TourId"].ToString();
                    tmpModel.CustromCName = dr["CustromCName"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("CustromCId")))
                        tmpModel.CustromCId = dr.GetInt32(dr.GetOrdinal("CustromCId"));
                    tmpModel.Item = dr["ProceedItem"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("Proceed")))
                        tmpModel.Amount = dr.GetDecimal(dr.GetOrdinal("Proceed"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IncreaseCost")))
                        tmpModel.AddAmount = dr.GetDecimal(dr.GetOrdinal("IncreaseCost"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ReduceCost")))
                        tmpModel.ReduceAmount = dr.GetDecimal(dr.GetOrdinal("ReduceCost"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SumCost")))
                        tmpModel.TotalAmount = dr.GetDecimal(dr.GetOrdinal("SumCost"));
                    tmpModel.Remark = dr["Remark"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorId")))
                        tmpModel.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CreateTime")))
                        tmpModel.CreateTime = dr.GetDateTime(dr.GetOrdinal("CreateTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PayTime")))
                        tmpModel.PayTime = dr.GetDateTime(dr.GetOrdinal("PayTime"));
                    tmpModel.Payee = dr["Payee"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("PayType")))
                        tmpModel.PayType = (EyouSoft.Model.EnumType.TourStructure.RefundType)dr.GetByte(dr.GetOrdinal("PayType"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Status")))
                    {
                        if (dr.GetString(dr.GetOrdinal("Status")) == "1" || dr.GetString(dr.GetOrdinal("Status")).ToLower() == "true")
                            tmpModel.Status = true;
                        else
                            tmpModel.Status = false;
                    }

                    list.Add(tmpModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取杂费收入信息合计
        /// </summary>
        /// <param name="model">杂费信息查询实体</param>
        /// <param name="haveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="isIncome">收入 or 支出 （1：收入；0：支出）</param>
        /// <param name="totalAmount">总合计金额</param>
        public void GetOtherIncomeList(Model.FinanceStructure.OtherCostQuery model, string haveUserIds, bool isIncome
            , ref decimal totalAmount)
        {
            if (model == null)
                return;

            string strOrder = string.Empty;
            var strSql = new StringBuilder();
            strSql.Append(" select sum(SumCost) from tbl_TourOtherCost ");
            string strWhere = GetSqlWhereByQueryModel(model, haveUserIds
                , isIncome ? Model.EnumType.FinanceStructure.CostType.收入 : Model.EnumType.FinanceStructure.CostType.支出
                , ref strOrder);
            if (!string.IsNullOrEmpty(strWhere))
                strSql.AppendFormat(" where {0} ", strWhere);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                        totalAmount = dr.GetDecimal(0);
                }
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 根据查询实体生成SqlWhere子句
        /// </summary>
        /// <param name="model">查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="strOrder">排序语句</param>
        /// <returns>SqlWhere子句</returns>
        private string GetSqlWhereByQueryModel(EyouSoft.Model.FinanceStructure.OtherCostQuery model, string HaveUserIds
            , EyouSoft.Model.EnumType.FinanceStructure.CostType CostType, ref string strOrder)
        {
            if (model == null)
                return string.Empty;

            StringBuilder strSqlWhere = new StringBuilder("");
            strSqlWhere.AppendFormat(" CostType = {0} ", (int)CostType);
            if (model.CompanyId > 0)
                strSqlWhere.AppendFormat(" and CompanyId = {0} ", model.CompanyId);
            if (!string.IsNullOrEmpty(model.TourId))
                strSqlWhere.AppendFormat(" and TourId = '{0}' ", model.TourId);
            if (!string.IsNullOrEmpty(model.ItemName))
                strSqlWhere.AppendFormat(" and ProceedItem like '%{0}%' ", model.ItemName);
            if (!string.IsNullOrEmpty(model.Payee))
                strSqlWhere.AppendFormat(" and Payee like '%{0}%' ", model.Payee);
            if (model.StartTime.HasValue)
                strSqlWhere.AppendFormat(" and datediff(dd,'{0}',PayTime) >= 0 ", model.StartTime.Value.ToShortDateString());
            if (model.EndTime.HasValue)
                strSqlWhere.AppendFormat(" and datediff(dd,PayTime,'{0}') >= 0 ", model.EndTime.Value.ToShortDateString());
            if (!string.IsNullOrEmpty(HaveUserIds))
            {
                //strSqlWhere.AppendFormat(" and OperatorId in ({0}) ", HaveUserIds);
                strSqlWhere.AppendFormat(" and ( (OperatorId in ({0}) AND TourId='') OR (TourId>'' AND EXISTS(SELECT 1 FROM tbl_Tour AS A WHERE A.TourId=tbl_TourOtherCost.TourId AND A.OperatorId IN({0})) ) ) ", HaveUserIds);
            }
            if (!string.IsNullOrEmpty(model.TourCode) || model.LEDate.HasValue || model.LSDate.HasValue)
            {
                strSqlWhere.Append(" AND TourId>'' ");
                strSqlWhere.Append(" AND EXISTS(SELECT 1 FROM tbl_Tour AS A WHERE A.TourId=tbl_TourOtherCost.TourId ");
                if (!string.IsNullOrEmpty(model.TourCode))
                {
                    strSqlWhere.AppendFormat(" AND A.TourCode LIKE '%{0}%' ", model.TourCode);
                }
                if (model.LEDate.HasValue)
                {
                    strSqlWhere.AppendFormat(" AND A.LeaveDate<'{0}' ", model.LEDate.Value.AddDays(1));
                }
                if (model.LSDate.HasValue)
                {
                    strSqlWhere.AppendFormat(" AND A.LeaveDate>='{0}' ", model.LSDate.Value);
                }
                strSqlWhere.Append(" ) ");
                //strSqlWhere.AppendFormat(" AND TourId>'' AND EXISTS(SELECT 1 FROM tbl_Tour AS A WHERE A.TourId=tbl_TourOtherCost.TourId AND A.TourCode LIKE '%{0}%') ", model.TourCode);
            }

            switch (model.OrderIndex)
            {
                case 0:
                    strOrder = " CreateTime asc ";
                    break;
                case 1:
                    strOrder = " CreateTime desc ";
                    break;
            }

            return strSqlWhere.ToString();
        }

        #endregion
    }
}
