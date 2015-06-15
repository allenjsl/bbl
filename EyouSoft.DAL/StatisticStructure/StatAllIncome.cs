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
    /// 所有收入明细数据层
    /// </summary>
    /// 鲁功源 2011-01-23
    public class StatAllIncome : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.StatisticStructure.IStatAllIncome
    {
        #region 变量
        private string Sql_StatAllIncome_Delete = "Update tbl_StatAllIncome set IsDelete='1' where ItemType=0 and ItemId='{1}' ";
        private string Sql_StatAllIncome_Add = "if NOT EXISTS (select 1 from tbl_StatAllIncome where ItemId='{4}' and ItemType={5} and CompanyId={0})	begin	insert into tbl_StatAllIncome([CompanyId],[TourId],[AreaId],[TourType],[ItemId],[ItemType],[Amount],[AddAmount],[ReduceAmount],[TotalAmount],[OperatorId],[DepartmentId],[HasCheckAmount],[NotCheckAmount]) values({0},'{1}',{2},{3},'{4}',{5},{6},{7},{8},{9},{10},{11},{12},{13}) end ;";
        private Database _db = null;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public StatAllIncome()
        {
            _db = this.SystemStore;
        }
        #endregion

        #region IStatAllIncome 成员
        /// <summary>
        /// 分页获取收入明细列表
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="queryInfo">查询实体</param>
        /// <param name="haveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.StatAllIncomeList> GetList(int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic queryInfo, string haveUserIds)
        {
            if (queryInfo == null || queryInfo.CompanyId <= 0)
                return null;

            IList<EyouSoft.Model.StatisticStructure.StatAllIncomeList> list = new List<EyouSoft.Model.StatisticStructure.StatAllIncomeList>();

            #region 构造分页存储过程参数

            string tableName = "view_StatAllIncomeStatement";
            string primaryKey = "salerid";
            string orderByStr = string.Empty;
            StringBuilder strfield = new StringBuilder();
            strfield.Append(" sum(totalamount) as TotalAmount,sum(hascheckamount) as AccountAmount,salerid,(select contactname from tbl_companyuser as a where a.id=view_StatAllIncomeStatement.salerid) as SalerName");
            string Query = this.GetSqlWhereByQuery(queryInfo, haveUserIds, ref orderByStr);

            #endregion

            using (IDataReader dr = DbHelper.ExecuteReaderIsGroupBy(this._db, PageSize, PageIndex, ref RecordCount, tableName, primaryKey, strfield.ToString(), Query, orderByStr, true))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.StatisticStructure.StatAllIncomeList model = new EyouSoft.Model.StatisticStructure.StatAllIncomeList();
                    if (!dr.IsDBNull(dr.GetOrdinal("SalerId")))
                        model.OperatorId = dr.GetInt32(dr.GetOrdinal("SalerId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SalerName")))
                        model.OperatorName = dr[dr.GetOrdinal("SalerName")].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("AccountAmount")))
                        model.AccountAmount = dr.GetDecimal(dr.GetOrdinal("AccountAmount"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TotalAmount")))
                        model.TotalAmount = dr.GetDecimal(dr.GetOrdinal("TotalAmount"));
                    list.Add(model);
                    model = null;
                }
            }
            return list;
        }
        /// <summary>
        /// 添加所有收入明细
        /// </summary>
        /// <param name="model">所有收入明细实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.StatisticStructure.StatAllIncome model)
        {
            DbCommand dc = this._db.GetStoredProcCommand("proc_StatAllIncome_Add");
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(dc, "AddAmount", DbType.Decimal, model.AddAmount);
            this._db.AddInParameter(dc, "Amount", DbType.Decimal, model.Amount);
            this._db.AddInParameter(dc, "AreaId", DbType.Int32, model.AreaId);
            this._db.AddInParameter(dc, "CheckAmount", DbType.Decimal, model.CheckAmount);
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
        /// 批量添加所有收入明细
        /// </summary>
        /// <param name="list">收入明细列表</param>
        /// <returns></returns>
        public bool Add(IList<EyouSoft.Model.StatisticStructure.StatAllIncome> list)
        {
            StringBuilder strSql = new StringBuilder();
            foreach (var item in list)
            {
                strSql.AppendFormat(Sql_StatAllIncome_Add, item.CompanyId, item.TourId, item.AreaId, (int)item.TourType,
                    item.ItemId, (int)item.ItemType, item.Amount, item.AddAmount, item.ReduceAmount, item.TotalAmount,
                    item.OperatorId, item.DepartmentId, item.CheckAmount, item.NotCheckAmount);
            }
            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());
            return DbHelper.ExecuteSqlTrans(dc, this._db) > 0 ? true : false;
        }
        /// <summary>
        /// 删除支出明细
        /// </summary>
        /// <param name="list">支出项目编号、类型集合</param>
        /// <returns></returns>
        public bool Delete(IList<EyouSoft.Model.StatisticStructure.IncomeItemIdAndType> list)
        {
            StringBuilder strSql = new StringBuilder();
            foreach (var item in list)
            {
                strSql.AppendFormat(Sql_StatAllIncome_Delete, (int)item.ItemType, item.ItemId);
            }
            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());
            return DbHelper.ExecuteSqlTrans(dc, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取收入对帐单的汇总信息
        /// </summary>
        /// <param name="queryInfo">查询实体</param>
        /// <param name="TotalAmount">总收入</param>
        /// <param name="HasCheckAmount">已收</param>
        /// <param name="haveUserIds">用户Id集合，半角逗号分割</param>
        public void GetAllTotalAmount(EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic queryInfo
            , ref decimal TotalAmount, ref decimal HasCheckAmount, string haveUserIds)
        {
            if (queryInfo == null || queryInfo.CompanyId <= 0)
                return;

            string orderByStr = string.Empty;
            string Query = this.GetSqlWhereByQuery(queryInfo, haveUserIds, ref orderByStr);
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select sum(a.TotalAmount) as AllTotalAmount,sum(a.AccountAmount) as AllAccountAmount from (select sum(totalamount) as TotalAmount,sum(hascheckamount) as AccountAmount from view_StatAllIncomeStatement ");
            if (queryInfo == null || queryInfo.CompanyId <= 0)
                strSql.Append(Query);
            else
                strSql.Append(" where " + Query);
            strSql.Append(" ) as a ");

            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());

            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                if (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                        TotalAmount = dr.GetDecimal(0);
                    if (!dr.IsDBNull(1))
                        HasCheckAmount = dr.GetDecimal(1);
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
        private string GetSqlWhereByQuery(EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic queryInfo
            , string haveUserIds, ref string orderByStr)
        {
            if (queryInfo == null || queryInfo.CompanyId <= 0)
                return " group by salerid ";

            StringBuilder Query = new StringBuilder();
            Query.AppendFormat(" CompanyId={0} ", queryInfo.CompanyId);

            orderByStr = queryInfo.OrderIndex == 0 ? " salerid asc" : " salerid desc ";

            EyouSoft.DAL.TourStructure.TourOrder ConvertDal = new EyouSoft.DAL.TourStructure.TourOrder();
            if (queryInfo.AreaId > 0)
                Query.AppendFormat(" AND areaid={0} ", queryInfo.AreaId);
            if (queryInfo.SaleIds != null && queryInfo.SaleIds.Length > 0)
            {
                string salerIds = ConvertDal.ConvertIntArrayTostring(queryInfo.SaleIds);
                Query.AppendFormat(" AND SalerId in({0})", salerIds);
            }
            if (queryInfo.BuyCompanyId != null && queryInfo.BuyCompanyId.Length > 0)
            {
                string BuyCompanyIds = ConvertDal.ConvertIntArrayTostring(queryInfo.BuyCompanyId);
                Query.AppendFormat(" AND BuyCompanyId in({0})", BuyCompanyIds);
            }
            if (!string.IsNullOrEmpty(queryInfo.BuyCompanyName))
            {
                Query.AppendFormat(" AND BuyCompanyName LIKE '%{0}%' ", queryInfo.BuyCompanyName);
            }
            if (queryInfo.TourType.HasValue)
                Query.AppendFormat(" AND TourType={0}", (int)queryInfo.TourType.Value);
            if (queryInfo.LeaveDateStart.HasValue)
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,'{0}',LeaveDate)>=0", queryInfo.LeaveDateStart);
            }
            if (queryInfo.LeaveDateEnd.HasValue)
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')>=0", queryInfo.LeaveDateEnd);
            }

            if (queryInfo.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计有效订单)
                Query.AppendFormat(" AND OrderState in ({0},{1},{2})", (int)Model.EnumType.TourStructure.OrderState.未处理, (int)Model.EnumType.TourStructure.OrderState.已成交, (int)Model.EnumType.TourStructure.OrderState.已留位);
            else if (queryInfo.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单)
                Query.AppendFormat(" AND OrderState = {0} ", (int)Model.EnumType.TourStructure.OrderState.已成交);

            if (queryInfo.IsAccount.HasValue)
            {
                if (queryInfo.IsAccount.Value)
                    Query.Append(" and TotalAmount <> 0 and TotalAmount = HasCheckAmount ");// + NotCheckAmount
                else
                    Query.Append(" and TotalAmount <> 0 and TotalAmount <> HasCheckAmount ");//+ + NotCheckAmount
            }

            if (!string.IsNullOrEmpty(haveUserIds))
                Query.AppendFormat(" and TourOperatorId in ({0}) ", haveUserIds);

            Query.Append(" group by salerid  ");

            return Query.ToString();
        }

        #endregion
    }
}
