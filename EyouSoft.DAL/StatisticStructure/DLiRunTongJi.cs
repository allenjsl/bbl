//汪奇志 2012-07-17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using EyouSoft.Toolkit.DAL;
using EyouSoft.Toolkit;

namespace EyouSoft.DAL.StatisticStructure
{
    /// <summary>
    /// 利润统计数据访问类
    /// </summary>
    public class DLiRunTongJi : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.StatisticStructure.IEarningsStatistic
    {
        #region constructor
        /// <summary>
        /// database
        /// </summary>
        private Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public DLiRunTongJi()
        {
            _db = SystemStore;
        }
        #endregion

        #region private members
        /// <summary>
        /// 拼接查询条件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string GetSQL(EyouSoft.Model.StatisticStructure.QueryEarningsStatistic model)
        {
            StringBuilder cmdText = new StringBuilder();

            if (model.TourType.HasValue)
            {
                cmdText.AppendFormat(" AND A.TourType={0} ", (int)model.TourType.Value);
            }
            if (model.LeaveDateEnd.HasValue)
            {
                cmdText.AppendFormat(" AND A.LeaveDate<'{0}' ", model.LeaveDateEnd.Value.AddDays(1));
            }
            if (model.LeaveDateStart.HasValue)
            {
                cmdText.AppendFormat(" AND A.LeaveDate>'{0}' ", model.LeaveDateStart.Value.AddDays(-1));
            }
            if (model.CheckDateEnd.HasValue)
            {
                cmdText.AppendFormat(" AND A.EndDateTime<'{0}' ", model.CheckDateEnd.Value.AddDays(1));
            }
            if (model.CheckDateStart.HasValue)
            {
                cmdText.AppendFormat(" AND A.EndDateTime>'{0}' ", model.CheckDateStart.Value.AddDays(-1));
            }
            if (model.SaleIds != null && model.SaleIds.Length > 0)
            {
                cmdText.AppendFormat(" AND A.OperatorId IN({0}) ", EyouSoft.Toolkit.Utils.GetSqlIdStrByArray(model.SaleIds));
            }
            if (model.DepartIds != null && model.DepartIds.Length > 0)
            {
                cmdText.AppendFormat(" AND A.DepartId IN({0}) ", EyouSoft.Toolkit.Utils.GetSqlIdStrByArray(model.DepartIds));
            }
            if (model.AreaId > 0)
            {
                cmdText.AppendFormat(" AND A.AreaId={0} ", model.AreaId);
            }

            return cmdText.ToString();
        }
        #endregion

        #region IEarningsStatistic 成员
        /// <summary>
        /// 获取利润--区域统计
        /// </summary>
        /// <param name="model">利润统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.EarningsAreaStatistic> GetEarningsAreaStatistic(EyouSoft.Model.StatisticStructure.QueryEarningsStatistic model, string HaveUserIds)
        {
            IList<EyouSoft.Model.StatisticStructure.EarningsAreaStatistic> items = new List<EyouSoft.Model.StatisticStructure.EarningsAreaStatistic>();
            StringBuilder cmdText = new StringBuilder();

            #region SQL
            cmdText.Append(" SELECT A.AreaId ");
            cmdText.Append(" ,A.AreaName ");
	        cmdText.Append(" ,COUNT(A.TourId) AS TuanDuiShu ");	        
	        cmdText.Append(" ,SUM(A.TotalAllExpenses) AS ZhiChuJinE ");
	        cmdText.Append(" ,SUM(A.DistributionAmount) AS LiRunFenPeiJinE ");

            if (model.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单)
            {
                cmdText.Append(" ,SUM(A.TotalOtherIncome+A.ChengJiaoJinE) AS ShouRuJinE ");
                cmdText.Append(" ,SUM(A.ChengJiaoRenShu) AS RenShu ");
            }
            else
            {
                cmdText.Append(" ,SUM(A.TotalOtherIncome+A.YouXiaoJinE) AS ShouRuJinE ");
                cmdText.Append(" ,SUM(A.YouXiaoRenShu) AS RenShu ");
            }
	        
            cmdText.Append(" FROM [View_EarningsStatistic] AS A ");
            cmdText.AppendFormat(" WHERE A.CompanyId={0} ", model.CompanyId);

            if (!string.IsNullOrEmpty(HaveUserIds))
            {
                cmdText.AppendFormat(" AND A.OperatorId IN({0}) ", HaveUserIds);
            }

            cmdText.Append(GetSQL(model));

            cmdText.Append(" GROUP BY A.AreaId,A.AreaName ");
            cmdText.Append(" ORDER BY A.AreaId ");

            if (model.OrderIndex == 1)
            {
                cmdText.Append("  DESC ");
            }
            #endregion

            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                //int index0 = rdr.GetOrdinal("AreaId");
                while (rdr.Read())
                {
                    var item = new EyouSoft.Model.StatisticStructure.EarningsAreaStatistic();

                    item.AreaId = rdr.GetInt32(rdr.GetOrdinal("AreaId"));
                    item.AreaName = rdr["AreaName"].ToString();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("ShouRuJinE"))) item.GrossIncome = rdr.GetDecimal(rdr.GetOrdinal("ShouRuJinE"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("ZhiChuJinE"))) item.GrossOut = rdr.GetDecimal(rdr.GetOrdinal("ZhiChuJinE"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("TuanDuiShu"))) item.TourNum = rdr.GetInt32(rdr.GetOrdinal("TuanDuiShu"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("LiRunFenPeiJinE"))) item.TourShare = rdr.GetDecimal(rdr.GetOrdinal("LiRunFenPeiJinE"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("RenShu"))) item.TourPeopleNum = rdr.GetInt32(rdr.GetOrdinal("RenShu"));
                    item.TourGross = item.GrossIncome - item.GrossOut;

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取利润--部门统计
        /// </summary>
        /// <param name="model">利润统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.EarningsDepartStatistic> GetEarningsDepartStatistic(EyouSoft.Model.StatisticStructure.QueryEarningsStatistic model, string HaveUserIds)
        {
            IList<EyouSoft.Model.StatisticStructure.EarningsDepartStatistic> items = new List<EyouSoft.Model.StatisticStructure.EarningsDepartStatistic>();

            StringBuilder cmdText = new StringBuilder();

            #region SQL
            cmdText.Append(" SELECT A.DepartId ");
            cmdText.Append(" ,(SELECT A1.DepartName FROM tbl_CompanyDepartment AS A1 WHERE A1.Id=A.DepartId) AS DeptName ");
            cmdText.Append(" ,COUNT(A.TourId) AS TuanDuiShu ");
            cmdText.Append(" ,SUM(A.TotalAllExpenses) AS ZhiChuJinE ");
            cmdText.Append(" ,SUM(A.DistributionAmount) AS LiRunFenPeiJinE ");

            if (model.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单)
            {
                cmdText.Append(" ,SUM(A.TotalOtherIncome+A.ChengJiaoJinE) AS ShouRuJinE ");
                cmdText.Append(" ,SUM(A.ChengJiaoRenShu) AS RenShu ");
            }
            else
            {
                cmdText.Append(" ,SUM(A.TotalOtherIncome+A.YouXiaoJinE) AS ShouRuJinE ");
                cmdText.Append(" ,SUM(A.YouXiaoRenShu) AS RenShu ");
            }

            cmdText.Append(" FROM [View_EarningsStatistic] AS A ");
            cmdText.AppendFormat(" WHERE A.CompanyId={0} ", model.CompanyId);

            if (!string.IsNullOrEmpty(HaveUserIds))
            {
                cmdText.AppendFormat(" AND A.OperatorId IN({0}) ", HaveUserIds);
            }

            cmdText.Append(GetSQL(model));

            cmdText.Append(" GROUP BY A.DepartId ");
            cmdText.Append(" ORDER BY A.DepartId ");

            if (model.OrderIndex == 3)
            {
                cmdText.Append("  DESC ");
            }
            #endregion

            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                while (rdr.Read())
                {
                    var item = new EyouSoft.Model.StatisticStructure.EarningsDepartStatistic();

                    if (!rdr.IsDBNull(rdr.GetOrdinal("DepartId"))) item.DepartId = rdr.GetInt32(rdr.GetOrdinal("DepartId"));
                    item.DepartName = rdr["DeptName"].ToString();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("ShouRuJinE"))) item.GrossIncome = rdr.GetDecimal(rdr.GetOrdinal("ShouRuJinE"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("ZhiChuJinE"))) item.GrossOut = rdr.GetDecimal(rdr.GetOrdinal("ZhiChuJinE"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("TuanDuiShu"))) item.TourNum = rdr.GetInt32(rdr.GetOrdinal("TuanDuiShu"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("LiRunFenPeiJinE"))) item.TourShare = rdr.GetDecimal(rdr.GetOrdinal("LiRunFenPeiJinE"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("RenShu"))) item.TourPeopleNum = rdr.GetInt32(rdr.GetOrdinal("RenShu"));
                    item.TourGross = item.GrossIncome - item.GrossOut;

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取利润--类型统计
        /// </summary>
        /// <param name="model">利润统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.EarningsTypeStatistic> GetEarningsTypeStatistic(EyouSoft.Model.StatisticStructure.QueryEarningsStatistic model, string HaveUserIds)
        {
            IList<EyouSoft.Model.StatisticStructure.EarningsTypeStatistic> items = new List<EyouSoft.Model.StatisticStructure.EarningsTypeStatistic>();

            StringBuilder cmdText = new StringBuilder();

            #region SQL
            cmdText.Append(" SELECT A.TourType ");
            cmdText.Append(" ,COUNT(A.TourId) AS TuanDuiShu ");
            cmdText.Append(" ,SUM(A.TotalAllExpenses) AS ZhiChuJinE ");
            cmdText.Append(" ,SUM(A.DistributionAmount) AS LiRunFenPeiJinE ");

            if (model.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单)
            {
                cmdText.Append(" ,SUM(A.TotalOtherIncome+A.ChengJiaoJinE) AS ShouRuJinE ");
                cmdText.Append(" ,SUM(A.ChengJiaoRenShu) AS RenShu ");
            }
            else
            {
                cmdText.Append(" ,SUM(A.TotalOtherIncome+A.YouXiaoJinE) AS ShouRuJinE ");
                cmdText.Append(" ,SUM(A.YouXiaoRenShu) AS RenShu ");
            }

            cmdText.Append(" FROM [View_EarningsStatistic] AS A ");
            cmdText.AppendFormat(" WHERE A.CompanyId={0} ", model.CompanyId);

            if (!string.IsNullOrEmpty(HaveUserIds))
            {
                cmdText.AppendFormat(" AND A.OperatorId IN({0}) ", HaveUserIds);
            }

            cmdText.Append(GetSQL(model));

            cmdText.Append(" GROUP BY A.TourType ");
            cmdText.Append(" ORDER BY A.TourType ");

            if (model.OrderIndex == 5)
            {
                cmdText.Append("  DESC ");
            }
            #endregion

            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                while (rdr.Read())
                {
                    var item = new EyouSoft.Model.StatisticStructure.EarningsTypeStatistic();

                    item.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)rdr.GetByte(rdr.GetOrdinal("TourType"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("ShouRuJinE"))) item.GrossIncome = rdr.GetDecimal(rdr.GetOrdinal("ShouRuJinE"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("ZhiChuJinE"))) item.GrossOut = rdr.GetDecimal(rdr.GetOrdinal("ZhiChuJinE"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("TuanDuiShu"))) item.TourNum = rdr.GetInt32(rdr.GetOrdinal("TuanDuiShu"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("LiRunFenPeiJinE"))) item.TourShare = rdr.GetDecimal(rdr.GetOrdinal("LiRunFenPeiJinE"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("RenShu"))) item.TourPeopleNum = rdr.GetInt32(rdr.GetOrdinal("RenShu"));
                    item.TourGross = item.GrossIncome - item.GrossOut;

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取利润--时间统计
        /// </summary>
        /// <param name="model">利润统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.EarningsTimeStatistic> GetEarningsTimeStatistic(EyouSoft.Model.StatisticStructure.QueryEarningsStatistic model, string HaveUserIds)
        {
            IList<EyouSoft.Model.StatisticStructure.EarningsTimeStatistic> items = new List<EyouSoft.Model.StatisticStructure.EarningsTimeStatistic>();

            StringBuilder cmdText = new StringBuilder();

            #region SQL
            cmdText.Append(" SELECT YEAR(A.LeaveDate) AS Year ");
            cmdText.Append(" ,MONTH(A.LeaveDate) AS Month ");
            cmdText.Append(" ,COUNT(A.TourId) AS TuanDuiShu ");
            cmdText.Append(" ,SUM(A.TotalAllExpenses) AS ZhiChuJinE ");
            cmdText.Append(" ,SUM(A.DistributionAmount) AS LiRunFenPeiJinE ");

            if (model.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单)
            {
                cmdText.Append(" ,SUM(A.TotalOtherIncome+A.ChengJiaoJinE) AS ShouRuJinE ");
                cmdText.Append(" ,SUM(A.ChengJiaoRenShu) AS RenShu ");
            }
            else
            {
                cmdText.Append(" ,SUM(A.TotalOtherIncome+A.YouXiaoJinE) AS ShouRuJinE ");
                cmdText.Append(" ,SUM(A.YouXiaoRenShu) AS RenShu ");
            }

            cmdText.Append(" FROM [View_EarningsStatistic] AS A ");
            cmdText.AppendFormat(" WHERE A.CompanyId={0} ", model.CompanyId);

            if (!string.IsNullOrEmpty(HaveUserIds))
            {
                cmdText.AppendFormat(" AND A.OperatorId IN({0}) ", HaveUserIds);
            }

            cmdText.Append(GetSQL(model));

            cmdText.Append(" GROUP BY YEAR(A.LeaveDate),MONTH(A.LeaveDate) ");

            if (model.OrderIndex == 6)
            {
                cmdText.Append(" ORDER BY Year ASC,Month ASC ");
            }
            else
            {
                cmdText.Append(" ORDER BY Year DESC,Month DESC ");
            }
            #endregion

            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                while (rdr.Read())
                {
                    var item = new EyouSoft.Model.StatisticStructure.EarningsTimeStatistic();

                    item.CurrMonth = rdr.GetInt32(rdr.GetOrdinal("Month"));
                    item.CurrYear = rdr.GetInt32(rdr.GetOrdinal("Year"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("ShouRuJinE"))) item.GrossIncome = rdr.GetDecimal(rdr.GetOrdinal("ShouRuJinE"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("ZhiChuJinE"))) item.GrossOut = rdr.GetDecimal(rdr.GetOrdinal("ZhiChuJinE"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("TuanDuiShu"))) item.TourNum = rdr.GetInt32(rdr.GetOrdinal("TuanDuiShu"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("LiRunFenPeiJinE"))) item.TourShare = rdr.GetDecimal(rdr.GetOrdinal("LiRunFenPeiJinE"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("RenShu"))) item.TourPeopleNum = rdr.GetInt32(rdr.GetOrdinal("RenShu"));
                    item.TourGross = item.GrossIncome - item.GrossOut;

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取团队利润统计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="us">用户Id集合，半角逗号间隔</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.MTuanDuiLiRunTongJiInfo> GetTuanDuiLiRunTongJi(int companyId, EyouSoft.Model.StatisticStructure.MTuanDuiLiRunTongJinSearchInfo searchInfo, string us)
        {
            IList<EyouSoft.Model.StatisticStructure.MTuanDuiLiRunTongJiInfo> items = new List<EyouSoft.Model.StatisticStructure.MTuanDuiLiRunTongJiInfo>();

            StringBuilder cmdText = new StringBuilder();

            #region SQL
            cmdText.Append(" SELECT SalerId ");
            cmdText.Append(" ,XiaoShouYuanName ");
            cmdText.Append(" ,SUM(TuanDuiShu) AS TuanDuiShu ");
            cmdText.Append(" ,SUM(RenShu) AS RenShu ");
            cmdText.Append(" ,SUM(TotalOtherIncome+FinanceSum) AS ShouRuJinE ");
            cmdText.Append(" ,SUM(ZhiChuJinE) AS ZhiChuJinE ");
            cmdText.Append(" ,SUM(DistributionAmount) AS LiRunFenPeiJinE ");
            cmdText.Append(" FROM view_TongJiTuanDuiLiRun ");
            cmdText.AppendFormat(" WHERE CompanyId={0} ", companyId);

            if (searchInfo != null)
            {
                if (searchInfo.AreaId.HasValue)
                {
                    cmdText.AppendFormat(" AND AreaId={0} ", searchInfo.AreaId.Value);
                }
                if (searchInfo.CityIds != null && searchInfo.CityIds.Length > 0)
                {
                    cmdText.AppendFormat(" AND CityId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.CityIds));
                }
                if (searchInfo.CTETime.HasValue)
                {
                    cmdText.AppendFormat(" AND LeaveDate<'{0}' ", searchInfo.CTETime.Value.AddDays(1));
                }
                if (searchInfo.CTSTime.HasValue)
                {
                    cmdText.AppendFormat(" AND LeaveDate>'{0}' ", searchInfo.CTSTime.Value.AddDays(-1));
                }
                if (searchInfo.DeptIds != null && searchInfo.DeptIds.Length > 0)
                {
                    cmdText.AppendFormat(" AND DepartId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.DeptIds));
                }
                if (searchInfo.ProvinceIds != null && searchInfo.ProvinceIds.Length > 0)
                {
                    cmdText.AppendFormat(" AND ProvinceId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.ProvinceIds));
                }
                if (searchInfo.XiaoShouYuanIds != null && searchInfo.XiaoShouYuanIds.Length > 0)
                {
                    cmdText.AppendFormat(" AND SalerId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.XiaoShouYuanIds));
                }
            }
            cmdText.Append(" GROUP BY SalerId,XiaoShouYuanName ORDER BY TuanDuiShu DESC ");
            #endregion

            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                while (rdr.Read())
                {
                    var item = new EyouSoft.Model.StatisticStructure.MTuanDuiLiRunTongJiInfo();

                    item.XiaoShouYuanName = rdr[1].ToString();
                    if (!rdr.IsDBNull(2)) item.TuanDuiShu = rdr.GetInt32(2);
                    if (!rdr.IsDBNull(3)) item.RenShu = rdr.GetInt32(3);
                    if (!rdr.IsDBNull(4)) item.ShouRuJinE = rdr.GetDecimal(4);
                    if (!rdr.IsDBNull(5)) item.ZhiChuJInE = rdr.GetDecimal(5);
                    if (!rdr.IsDBNull(6)) item.LiRunFenPeiJinE = rdr.GetDecimal(6);

                    items.Add(item);
                }
            }

            return items;
        }
        #endregion
    }
}
