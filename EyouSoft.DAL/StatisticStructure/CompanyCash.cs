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
    /// 统计分析-现金流量列表数据层
    /// </summary>
    /// 鲁功源 2011-01-22
    public class CompanyCash : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.StatisticStructure.ICompanyCash
    {
        #region 变量
        private Database _db = null;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompanyCash()
        {
            _db = this.SystemStore;
        }
        #endregion


        #region ICompanyCash 成员
        /// <summary>
        /// 获取统计分析-现金流量列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="queryInfo">现金流量查询实体</param>
        /// <returns>现金流量列表</returns>
        public IList<EyouSoft.Model.StatisticStructure.CompanyCash> GetList(int companyId, EyouSoft.Model.StatisticStructure.QueryCompanyCash queryInfo)
        {
            IList<EyouSoft.Model.StatisticStructure.CompanyCash> list = new List<EyouSoft.Model.StatisticStructure.CompanyCash>();
            
            #region 构造sql语句
            StringBuilder strSql = new StringBuilder();

            strSql.Append(" SELECT A.* ");
            if (queryInfo.StatisticType == EyouSoft.Model.EnumType.StatisticStructure.StatisticType.按月统计)
            {
                strSql.AppendFormat(" ,(SELECT CashReserve FROM [tbl_CompanyCash] WHERE [IssueTime]=A.[IssueTime] AND CompanyId={0}) AS MonthCashReserve ", companyId);
            }
            strSql.Append(" FROM  ( ");

            #region tbl_A
            strSql.Append("SELECT SUM([CashReserve]) AS CashReserve");
            strSql.Append(" ,SUM(CashIn) AS CashIn");
            strSql.Append(" ,SUM(CashOut) AS CashOut");
            strSql.Append(" ,MAX([IssueTime]) AS IssueTime ");
            strSql.Append(" FROM tbl_CompanyCash WHERE YEAR([IssueTime])=YEAR(GETDATE())");
            if (queryInfo.StatisticType == EyouSoft.Model.EnumType.StatisticStructure.StatisticType.按日统计)
            {
                strSql.Append(" AND MONTH([IssueTime])=MONTH(GETDATE()) ");
            }
            strSql.AppendFormat(" AND CompanyId={0} ", companyId);

            if (queryInfo.StatisticType == EyouSoft.Model.EnumType.StatisticStructure.StatisticType.按月统计)
            {
                strSql.Append(" GROUP BY MONTH([IssueTime]) ");
            }
            else
            {
                strSql.Append(" GROUP BY [IssueTime] ");
            }
            #endregion

            strSql.Append(" ) A ");

            if (queryInfo.StatisticType == EyouSoft.Model.EnumType.StatisticStructure.StatisticType.按月统计)
            {
                strSql.Append(" ORDER BY MONTH(A.[IssueTime]) ");
            }
            else
            {
                strSql.Append(" ORDER BY A.[IssueTime] ");
            }
            if (queryInfo.OrderIndex == 0)
            {
                strSql.Append(" ASC ");
            }
            else
            {
                strSql.Append(" DESC ");
            }
            #endregion

            DbCommand dc=this._db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.StatisticStructure.CompanyCash model = new EyouSoft.Model.StatisticStructure.CompanyCash();
                    if (!dr.IsDBNull(dr.GetOrdinal("CashIn")))
                        model.CashIn = dr.GetDecimal(dr.GetOrdinal("CashIn"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CashOut")))
                        model.CashOut = dr.GetDecimal(dr.GetOrdinal("CashOut"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CashReserve")))
                        model.CashReserve = dr.GetDecimal(dr.GetOrdinal("CashReserve"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                        model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    if (queryInfo.StatisticType == EyouSoft.Model.EnumType.StatisticStructure.StatisticType.按月统计)
                        model.CashReserve = dr.GetDecimal(dr.GetOrdinal("MonthCashReserve"));
                    list.Add(model);
                    model = null;
                }
            }
            return list;
        }

        #endregion
    }
}
