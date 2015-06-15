using System.Collections.Generic;
using System.Text;
using EyouSoft.Model.StatisticStructure;
using EyouSoft.Toolkit.DAL;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using EyouSoft.Toolkit;
using System;
using System.Data.Common;

namespace EyouSoft.DAL.StatisticStructure
{
    /// <summary>
    /// 返佣统计数据访问
    /// </summary>
    /// 周文超 2011-06-08
    public class DCommissionStat : DALBase, IDAL.StatisticStructure.ICommissionStat
    {
        #region 变量

        private readonly Database _db;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public DCommissionStat()
        {
            _db = SystemStore;
        }

        #endregion

        #region ICommissionStat 成员

        /// <summary>
        /// 获取客户返佣统计信息
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="queryModel">查询实体</param>
        /// <returns></returns>
        public IList<MCommissionStatInfo> GetCommissions(int pageSize, int pageIndex, ref int recordCount, int companyId, MCommissionStatSeachInfo queryModel)
        {
            if (companyId <= 0)
                return null;

            IList<MCommissionStatInfo> list = new List<MCommissionStatInfo>();
            const string strTableName = "tbl_tourorder";
            const string strPk = "BuyerContactId";
            string strOrderBy = string.Empty;
            string strSqlWhere = GetSqlWhereByQueryModel(companyId, queryModel, ref strOrderBy);
            strSqlWhere += " group by BuyCompanyID,BuyerContactId  ";
            var strFiled = new StringBuilder(" BuyCompanyID,BuyerContactId ");
            strFiled.Append(@" ,(select [Name] from tbl_Customer as b where b.ID = tbl_tourorder.BuyCompanyID) as BuyCompanyName ");
            strFiled.Append(@" ,(select [Name] from tbl_CustomerContactInfo as c where c.ID = tbl_tourorder.BuyerContactId) as BuyerContactName ");
            strFiled.Append(@" ,sum(case when CommissionType = 1 then CommissionPrice * PeopleNumber else 0 end) as BeforeAmount ");
            strFiled.Append(@" ,sum(case when CommissionType = 2 then CommissionPrice * PeopleNumber else 0 end) as AfterAmount ");

            using (IDataReader dr = DbHelper.ExecuteReader(_db, pageSize, pageIndex, ref recordCount, strTableName, strPk, strFiled.ToString(), strSqlWhere, strOrderBy, true))
            {
                while (dr.Read())
                {
                    var model = new MCommissionStatInfo
                                    {
                                        CustomerId = dr.IsDBNull(0) ? 0 : dr.GetInt32(0),
                                        ContactId = dr.IsDBNull(1) ? 0 : dr.GetInt32(1),
                                        CustomerName = dr.IsDBNull(2) ? string.Empty : dr.GetString(2),
                                        ContactName = dr.IsDBNull(3) ? string.Empty : dr.GetString(3),
                                        BeforeAmount = dr.IsDBNull(4) ? 0 : dr.GetDecimal(4),
                                        AfterAmount = dr.IsDBNull(5) ? 0 : dr.GetDecimal(5)
                                    };

                    list.Add(model);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取返佣明细信息集合
        /// </summary>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="buyerCompanyId">客户单位编号（组团）</param>
        /// <param name="buyerContactId">客户单位联系人编号</param>
        /// <param name="commissionType">返佣类型</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.MCommissionDetailInfo> GetCommissionDetails(int pageSize, int pageIndex, ref int recordCount
            , int companyId, int buyerCompanyId, int buyerContactId, EyouSoft.Model.EnumType.CompanyStructure.CommissionType commissionType
            , EyouSoft.Model.StatisticStructure.MCommissionStatSeachInfo searchInfo)
        {
            IList<EyouSoft.Model.StatisticStructure.MCommissionDetailInfo> items = new List<EyouSoft.Model.StatisticStructure.MCommissionDetailInfo>();
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_TourOrder";
            string primaryKey = "Id";
            string orderByString = "IssueTime DESC";
            string fields = "TourId,RouteName,TourNo,ID,OrderNo,AdultNumber,ChildNumber,SumPrice,LastOperatorID,CommissionType,CommissionPrice,CommissionStatus,BuyCompanyID,BuyCompanyName,BuyerContactId,BuyerContactName,(SELECT [ContactName] FROM [tbl_CompanyUser] AS A WHERE A.[Id]=tbl_TourOrder.LastOperatorID) AS OperatorName";

            cmdQuery.AppendFormat(" SellCompanyId={0} ", companyId);
            cmdQuery.AppendFormat(" AND BuyCompanyID={0} ", buyerCompanyId);
            cmdQuery.AppendFormat(" AND BuyerContactId={0} ", buyerContactId);
            cmdQuery.AppendFormat(" AND CommissionType={0} ", (int)commissionType);
            cmdQuery.AppendFormat(" AND OrderState not in ({0},{1}) ", (int)Model.EnumType.TourStructure.OrderState.不受理, (int)Model.EnumType.TourStructure.OrderState.留位过期);
            cmdQuery.Append(" AND IsDelete='0' ");

            if (searchInfo != null)
            {
                if (searchInfo.LeaveDateStart.HasValue)
                {
                    cmdQuery.AppendFormat(" AND LeaveDate>='{0}' ", searchInfo.LeaveDateStart.Value);
                }
                if (searchInfo.LeaveDateEnd.HasValue)
                {
                    cmdQuery.AppendFormat(" AND LeaveDate<='{0}' ", searchInfo.LeaveDateEnd.Value.AddDays(1).AddMilliseconds(-1));
                }
                if (searchInfo.OperatorId!=null&&searchInfo.OperatorId.Length>0)
                {
                    cmdQuery.AppendFormat(" AND LastOperatorID IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorId));
                }
                if (searchInfo.OrderDateStart.HasValue)
                {
                    cmdQuery.AppendFormat(" AND IssueTime>='{0}' ", searchInfo.OrderDateStart.Value);
                }
                if (searchInfo.OrderDateEnd.HasValue)
                {
                    cmdQuery.AppendFormat(" AND IssueTime<='{0}' ", searchInfo.OrderDateEnd.Value.AddDays(1).AddMilliseconds(-1));
                }
            }

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields, cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    items.Add(new MCommissionDetailInfo()
                    {
                        AdultNumber = rdr.GetInt32(rdr.GetOrdinal("AdultNumber")),
                        ChildrenNumber = rdr.GetInt32(rdr.GetOrdinal("ChildNumber")),
                        CommissionPrice = rdr.GetDecimal(rdr.GetOrdinal("CommissionPrice")),
                        IsPaid = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("CommissionStatus"))),
                        OperatorId = rdr.GetInt32(rdr.GetOrdinal("LastOperatorID")),
                        OperatorName = rdr["OperatorName"].ToString(),
                        OrderAmount = rdr.GetDecimal(rdr.GetOrdinal("SumPrice")),
                        OrderId = rdr.GetString(rdr.GetOrdinal("Id")),
                        OrderNo = rdr.GetString(rdr.GetOrdinal("OrderNo")),
                        RouteName = rdr["RouteName"].ToString(),
                        TourCode = rdr.GetString(rdr.GetOrdinal("TourNo")),
                        TourId = rdr.GetString(rdr.GetOrdinal("TourId"))
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 支付返佣金额，返回1成功，其它失败
        /// </summary>
        /// <param name="info">支付信息业务实体</param>
        /// <returns></returns>
        public int PayCommission(EyouSoft.Model.StatisticStructure.MPayCommissionInfo info)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_OtherCost_PayCommission");
            this._db.AddInParameter(cmd, "CostId", DbType.AnsiStringFixedLength, Guid.NewGuid().ToString());
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, info.TourId);
            this._db.AddInParameter(cmd, "OrderId", DbType.AnsiStringFixedLength, info.OrderId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, info.CompanyId);
            this._db.AddInParameter(cmd, "BuyerCompanyId", DbType.Int32, info.BuyerCompanyId);
            this._db.AddInParameter(cmd, "BuyerCompanyName", DbType.String, info.BuyerCompanyName);
            this._db.AddInParameter(cmd, "Amount", DbType.Decimal, info.Amount);
            this._db.AddInParameter(cmd, "PayerId", DbType.Int32, info.PayerId);
            this._db.AddInParameter(cmd, "PayTime", DbType.DateTime, info.PayTime);
            this._db.AddInParameter(cmd, "PayType", DbType.Byte, info.PayType);
            this._db.AddInParameter(cmd, "CostType", DbType.Byte, info.CostType);
            this._db.AddInParameter(cmd, "CostName", DbType.String, info.CostName);
            this._db.AddInParameter(cmd, "ItemType", DbType.String, info.ItemType);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);

            int sqlExceptionCode = 0;

            try
            {
                DbHelper.RunProcedure(cmd, this._db);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                sqlExceptionCode = 0 - e.Number;
            }

            if (sqlExceptionCode < 0) return sqlExceptionCode;

            return Convert.ToInt32(this._db.GetParameterValue(cmd, "Result"));
        }
        #endregion

        #region 私有函数

        /// <summary>
        /// 根据查询实体返回SqlWhere子句
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="queryModel">查询实体</param>
        /// <param name="strOrderBy">排序子句</param>
        /// <returns>SqlWhere子句</returns>
        private string GetSqlWhereByQueryModel(int companyId, MCommissionStatSeachInfo queryModel, ref string strOrderBy)
        {
            var strWhere = new StringBuilder(" BuyCompanyID > 0 and BuyerContactId > 0 and IsDelete = '0' ");
            strWhere.AppendFormat(" and SellCompanyId = {0} ", companyId);
            strWhere.AppendFormat(" and OrderState not in ({0},{1}) ", (int)Model.EnumType.TourStructure.OrderState.不受理,
                                  (int)Model.EnumType.TourStructure.OrderState.留位过期);

            if (queryModel == null)
                return strWhere.ToString();

            if (queryModel.ContactId != null && queryModel.ContactId.Length > 0)
                strWhere.AppendFormat(" and BuyerContactId in ({0}) ", Utils.GetSqlIdStrByArray(queryModel.ContactId));
            if (!string.IsNullOrEmpty(queryModel.ContactName))
                strWhere.AppendFormat(" and BuyerContactName like '%{0}%' ", queryModel.ContactName);
            if (queryModel.CustomerId != null && queryModel.CustomerId.Length > 0)
                strWhere.AppendFormat(" and BuyCompanyID in ({0}) ", Utils.GetSqlIdStrByArray(queryModel.CustomerId));
            if (!string.IsNullOrEmpty(queryModel.CustomerName))
                strWhere.AppendFormat(" and BuyCompanyName like '%{0}%' ", queryModel.CustomerName);
            if (queryModel.OperatorId != null && queryModel.OperatorId.Length > 0)
                strWhere.AppendFormat(" and LastOperatorID in ({0}) ", Utils.GetSqlIdStrByArray(queryModel.OperatorId));
            if(queryModel.LeaveDateStart.HasValue)
                strWhere.AppendFormat(" and datediff(dd,'{0}',LeaveDate) >= 0 ", queryModel.LeaveDateStart.Value.ToShortDateString());
            if (queryModel.LeaveDateEnd.HasValue)
                strWhere.AppendFormat(" and datediff(dd,LeaveDate,'{0}') >= 0 ", queryModel.LeaveDateEnd.Value.ToShortDateString());
            if (queryModel.OrderDateStart.HasValue)
                strWhere.AppendFormat(" and datediff(dd,'{0}',IssueTime) >= 0 ", queryModel.OrderDateStart.Value.ToShortDateString());
            if (queryModel.OrderDateEnd.HasValue)
                strWhere.AppendFormat(" and datediff(dd,IssueTime,'{0}') >= 0 ", queryModel.OrderDateEnd.Value.ToShortDateString());

            switch (queryModel.OrderByIndex)
            {
                case 0:
                    strOrderBy = " BuyCompanyID asc, BuyerContactId asc ";
                    break;
                case 1:
                    strOrderBy = " BuyCompanyID desc, BuyerContactId desc ";
                    break;
                default:
                    strOrderBy = " BuyCompanyID desc, BuyerContactId desc ";
                    break;
            }

            return strWhere.ToString();
        }

        #endregion
    }
}
