using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data.Common;
using System.Data;

namespace EyouSoft.DAL.StatisticStructure
{
    /// <summary>
    /// 所有支出明细数据层接口
    /// </summary>
    /// 鲁功源 2011-01-23
    public class StatAllOut : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.StatisticStructure.IStatAllOut
    {
        #region 变量
        private string Sql_StatAllOut_Delete = "Update tbl_StatAllOut set IsDelete='1' where ItemType={0} and ItemId='{1}';";
        private string Sql_StatAllOut_Add = "if NOT EXISTS (select 1 from tbl_StatAllOut where ItemId='{4}' and ItemType={5} and CompanyId={0})	begin	insert into tbl_StatAllOut([CompanyId],[TourId],[AreaId],[TourType],[ItemId],[ItemType],[Amount],[AddAmount],[ReduceAmount],[TotalAmount],[OperatorId],[DepartmentId],[HasCheckAmount],[NotCheckAmount],[SupplierId],[SupplierName])	values({0},'{1}',{2},{3},'{4}',{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},'{15}') end ;";
        private Database _db = null;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public StatAllOut()
        {
            _db = this.SystemStore;
        }
        #endregion

        #region IStatAllOut 成员
        /// <summary>
        /// 分页获取支出明细列表(统计分析-支出对账单列表)
        /// </summary>
        /// <param name="PageSize">每页现实条数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="queryInfo">查询实体</param>
        /// <param name="haveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.StatAllOutList> GetList(int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic queryInfo, string haveUserIds)
        {
            if (queryInfo == null || queryInfo.CompanyId <= 0)
                return null;

            IList<EyouSoft.Model.StatisticStructure.StatAllOutList> list = new List<EyouSoft.Model.StatisticStructure.StatAllOutList>();

            #region 构造分页存储过程参数

            string tableName = "tbl_StatAllOut";
            string primaryKey = "Id";
            string orderByStr = string.Empty;
            StringBuilder strfield = new StringBuilder();
            strfield.Append("OperatorId,");
            strfield.Append("sum(TotalAmount) as TotalAmount,");
            strfield.Append("sum(HasCheckAmount) as PaidAmount,");
            strfield.Append("(select ContactName from tbl_CompanyUser where id=tbl_StatAllOut.OperatorId) as OperatorName");
            string strWhere = this.GetSqlWhereByQuery(queryInfo, haveUserIds, ref orderByStr);

            #endregion

            using (IDataReader dr = DbHelper.ExecuteReaderIsGroupBy(this._db, PageSize, PageIndex, ref RecordCount, tableName, primaryKey, strfield.ToString(), strWhere, orderByStr, true))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.StatisticStructure.StatAllOutList model = new EyouSoft.Model.StatisticStructure.StatAllOutList();
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorId")))
                        model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorName")))
                        model.OperatorName = dr[dr.GetOrdinal("OperatorName")].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("PaidAmount")))
                        model.PaidAmount = dr.GetDecimal(dr.GetOrdinal("PaidAmount"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TotalAmount")))
                        model.TotalAmount = dr.GetDecimal(dr.GetOrdinal("TotalAmount"));
                    list.Add(model);
                    model = null;
                }
            }
            return list;
        }
        /// <summary>
        /// 添加所有支出明细
        /// </summary>
        /// <param name="model">所有支出明细实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.StatisticStructure.StatAllOut model)
        {
            DbCommand dc = this._db.GetStoredProcCommand("proc_StatAllOut_Add");
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(dc, "SupplierId", DbType.Int32, model.SupplierId);
            this._db.AddInParameter(dc, "SupplierName", DbType.String, model.SupplierName);
            this._db.AddInParameter(dc, "AddAmount", DbType.Decimal, model.AddAmount);
            this._db.AddInParameter(dc, "Amount", DbType.Decimal, model.Amount);
            this._db.AddInParameter(dc, "AreaId", DbType.Int32, model.AreaId);
            this._db.AddInParameter(dc, "HasCheckAmount", DbType.Decimal, model.CheckAmount);
            this._db.AddInParameter(dc, "DepartmentId", DbType.Int32, model.DepartmentId);
            this._db.AddInParameter(dc, "ItemId", DbType.String, model.ItemId);
            this._db.AddInParameter(dc, "ItemType", DbType.Byte, (int)model.ItemType);
            this._db.AddInParameter(dc, "NotCheckAmount", DbType.Decimal, model.NotCheckAmount);
            this._db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            this._db.AddInParameter(dc, "ReduceAmount", DbType.Decimal, model.ReduceAmount);
            this._db.AddInParameter(dc, "TotalAmount", DbType.Decimal, model.TotalAmount);
            this._db.AddInParameter(dc, "TourId", DbType.String, model.TourId);
            this._db.AddInParameter(dc, "TourType", DbType.Byte, (int)model.TourType);
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, this._db);
            object obj = this._db.GetParameterValue(dc, "Result");
            return int.Parse(obj.ToString()) > 0 ? true : false;
        }
        /// <summary>
        /// 批量添加所有支出明细
        /// </summary>
        /// <param name="list">支出明细列表</param>
        /// <returns></returns>
        public bool Add(IList<EyouSoft.Model.StatisticStructure.StatAllOut> list)
        {
            StringBuilder strSql = new StringBuilder();
            foreach (var item in list)
            {
                strSql.AppendFormat(Sql_StatAllOut_Add, item.CompanyId, item.TourId, item.AreaId, (int)item.TourType,
                    item.ItemId, (int)item.ItemType, item.Amount, item.AddAmount, item.ReduceAmount, item.TotalAmount,
                    item.OperatorId, item.DepartmentId, item.CheckAmount, item.NotCheckAmount, item.SupplierId, item.SupplierName);
            }
            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());
            return DbHelper.ExecuteSqlTrans(dc, this._db) > 0 ? true : false;
        }
        /// <summary>
        /// 删除支出明细
        /// </summary>
        /// <param name="list">支出项目编号、类型集合</param>
        /// <returns></returns>
        public bool Delete(IList<EyouSoft.Model.StatisticStructure.ItemIdAndType> list)
        {

            StringBuilder strSql = new StringBuilder();
            foreach (var item in list)
            {
                strSql.AppendFormat(Sql_StatAllOut_Delete, (int)item.ItemType, item.ItemId);
            }
            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());
            return DbHelper.ExecuteSqlTrans(dc, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取支出对帐单的汇总信息(统计分析-支出对账单列表合计)
        /// </summary>
        /// <param name="queryInfo">查询实体</param>
        /// <param name="totalAmount">总支出</param>
        /// <param name="hasCheckAmount">已付</param>
        /// <param name="haveUserIds">用户Id集合，半角逗号分割</param>
        public void GetAllTotalAmount(EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic queryInfo
            , ref decimal totalAmount, ref decimal hasCheckAmount, string haveUserIds)
        {
            if (queryInfo == null || queryInfo.CompanyId <= 0)
                return;

            string orderByStr = string.Empty;
            string Query = this.GetSqlWhereByQuery(queryInfo, haveUserIds, ref orderByStr);
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select sum(a.TotalAmount) as AllTotalAmount,sum(a.HasCheckAmount) as AllHasCheckAmount from (select sum(TotalAmount) as TotalAmount,sum(HasCheckAmount) as HasCheckAmount from tbl_StatAllOut ");
            strSql.Append(" where " + Query);
            strSql.Append(" ) as a ");

            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());

            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                if (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                        totalAmount = dr.GetDecimal(0);
                    if (!dr.IsDBNull(1))
                        hasCheckAmount = dr.GetDecimal(1);
                }
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 根据查询条件生成SqlWhere语句
        /// </summary>
        /// <param name="queryInfo">查询条件实体</param>
        /// <param name="orderByStr">排序字符串</param>
        /// <param name="haveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns>SqlWhere语句</returns>
        private string GetSqlWhereByQuery(EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic queryInfo, string haveUserIds, ref string orderByStr)
        {
            StringBuilder strWhere = new StringBuilder(" (ItemType=1 OR ItemType=2 OR ItemType=4) AND IsDelete='0' ");
            if (queryInfo != null)
            {
                orderByStr = queryInfo.OrderIndex == 0 ? " OperatorId asc" : " OperatorId desc ";

                //StringBuilder strAreaSql = new StringBuilder("select areaid from tbl_StatAllOut where  IsDelete='0' ");
                if (queryInfo.AreaId > 0)
                    strWhere.AppendFormat(" and areaid={0} ", queryInfo.AreaId);
                if (queryInfo.CompanyId > 0)
                    strWhere.AppendFormat(" and CompanyId={0} ", queryInfo.CompanyId);
                if (queryInfo.SaleIds != null && queryInfo.SaleIds.Length > 0)
                {
                    string SaleIds = string.Empty;
                    foreach (int i in queryInfo.SaleIds)
                    {
                        if (i > 0)
                            SaleIds += i.ToString() + ",";
                    }
                    SaleIds = SaleIds.TrimEnd(',');
                    strWhere.AppendFormat(" and OperatorId in({0})", SaleIds);
                }
                if (queryInfo.TourType.HasValue)
                    strWhere.AppendFormat(" and TourType={0}", (int)queryInfo.TourType.Value);

                if (queryInfo.BuyCompanyId != null && queryInfo.BuyCompanyId.Length > 0)
                {
                    string strBuyCompanyIds = string.Empty;
                    if (queryInfo.BuyCompanyId != null && queryInfo.BuyCompanyId.Length > 0)
                    {
                        foreach (int i in queryInfo.BuyCompanyId)
                        {
                            if (i > 0)
                                strBuyCompanyIds += i.ToString() + ",";
                        }
                    }
                    strBuyCompanyIds = strBuyCompanyIds.Trim(',');
                    if (!string.IsNullOrEmpty(strBuyCompanyIds))
                        strWhere.AppendFormat(" and SupplierId in ({0}) ", strBuyCompanyIds);
                }
                if (!string.IsNullOrEmpty(queryInfo.BuyCompanyName))
                    strWhere.AppendFormat(" and SupplierName like '%{0}%' ", queryInfo.BuyCompanyName);

                if (queryInfo.IsAccount.HasValue)
                {
                    if (queryInfo.IsAccount.Value)
                        strWhere.Append(" and TotalAmount <> 0 and TotalAmount = HasCheckAmount ");// + NotCheckAmount
                    else
                        strWhere.Append(" and TotalAmount <> 0 and TotalAmount <> HasCheckAmount ");// + NotCheckAmount
                }

                /*if (!string.IsNullOrEmpty(haveUserIds))
                    strWhere.AppendFormat(@" and (TourId is null and len(TourId) <> 36 and OperatorId in ({0}) or (TourId is not null and len(TourId) = 36 and TourId in (select TourId from tbl_Tour where tbl_Tour.OperatorId in ({0})))) ", haveUserIds);*/

                //strWhere.AppendFormat(" and areaid in({0})", strAreaSql.ToString());

                if (queryInfo.LeaveDateStart.HasValue||queryInfo.LeaveDateEnd.HasValue)
                {
                    strWhere.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_Tour AS A WHERE 1=1 AND A.TourId=tbl_StatAllOut.TourId ");
                    if (queryInfo.LeaveDateStart.HasValue)
                    {
                        strWhere.AppendFormat(" AND A.LeaveDate>='{0}' ", queryInfo.LeaveDateStart.Value);
                    }

                    if (queryInfo.LeaveDateEnd.HasValue)
                    {
                        strWhere.AppendFormat(" AND A.LeaveDate<='{0}' ", queryInfo.LeaveDateEnd.Value);
                    }
                    strWhere.AppendFormat(" ) ");
                }
            }
            strWhere.Append(" group by OperatorId  ");

            return strWhere.ToString();
        }

        #endregion
    }
}
