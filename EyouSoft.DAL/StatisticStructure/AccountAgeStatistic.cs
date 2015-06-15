using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data;
using EyouSoft.Toolkit;

namespace EyouSoft.DAL.StatisticStructure
{
    /// <summary>
    /// 帐龄分析统计数据访问
    /// </summary>
    /// 周文超 2011-01-25
    public class AccountAgeStatistic : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.StatisticStructure.IAccountAgeStatistic
    {
        #region constructor
        private readonly Database _db = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AccountAgeStatistic()
        {
            _db = base.SystemStore;
        }
        #endregion

        #region private members

        /// <summary>
        /// 根据查询实体生成Where子句
        /// </summary>
        /// <param name="model">查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="strOrder">排序语句</param>
        /// <returns>Where子句</returns>
        private string GetSqlWhere(EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic model, string HaveUserIds
            , ref string strOrder)
        {
            if (model == null)
                return string.Empty;

            StringBuilder strSqlWhere = new StringBuilder();
            string strIds = string.Empty;
            if (model.CompanyId > 0)
                strSqlWhere.AppendFormat(" and SellCompanyId = {0} ", model.CompanyId);
            if (model.TourType.HasValue)
                strSqlWhere.AppendFormat(" and TourClassId = {0} ", (int)model.TourType.Value);
            if (model.SaleIds != null && model.SaleIds.Length > 0)
            {
                strIds = string.Empty;
                foreach (int i in model.SaleIds)
                {
                    if (i <= 0)
                        continue;

                    strIds += i.ToString() + ",";
                }
                strIds = strIds.Trim(',');
                if (!string.IsNullOrEmpty(strIds))
                    strSqlWhere.AppendFormat(" and SalerId in ({0}) ", strIds);
            }
            if (!string.IsNullOrEmpty(HaveUserIds))
                strSqlWhere.AppendFormat(" and ViewOperatorId in ({0}) ", HaveUserIds);
            if (model.LeaveDateStart.HasValue)
                strSqlWhere.AppendFormat(" and LeaveDate>='{0}' ", model.LeaveDateStart.Value);
            if (model.LeaveDateEnd.HasValue)
                strSqlWhere.AppendFormat(" and LeaveDate<='{0}' ", model.LeaveDateEnd.Value.AddDays(1).AddMilliseconds(-1));
            if (model.AreaId > 0)
                strSqlWhere.AppendFormat(" and AreaId = {0} ", model.AreaId);

            switch (model.OrderIndex)
            {
                case 0:
                    strOrder = " SalerId asc ";
                    break;
                case 1:
                    strOrder = " SalerId desc ";
                    break;
            }

            return strSqlWhere.ToString();
        }

        #endregion

        #region IAccountAgeStatistic 成员

        /// <summary>
        /// 获取帐龄分析统计
        /// </summary>
        /// <param name="model">帐龄分析查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.AccountAgeStatistic> GetAccountAgeStatistic(EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic model, string HaveUserIds)
        {
            IList<EyouSoft.Model.StatisticStructure.AccountAgeStatistic> list = new List<EyouSoft.Model.StatisticStructure.AccountAgeStatistic>();
            string strOrder = string.Empty;
            StringBuilder strSql = new StringBuilder(" select ");
            strSql.Append(" SalerId, ");
            strSql.Append(" (select ContactName from tbl_CompanyUser where tbl_CompanyUser.id = tbl_TourOrder.SalerId and tbl_CompanyUser.isdelete = '0') as SalerName, ");
            strSql.Append(" sum(FinanceSum - HasCheckMoney) as ArrearageSum, ");
            strSql.Append(" min(IssueTime) as MaxArrearageTime ");
            strSql.Append(" from tbl_TourOrder ");
            //strSql.AppendFormat(" where IsDelete = '0' and (FinanceSum - HasCheckMoney - NotCheckMoney) > 0 and SalerId > 0 and exists (select 1 from tbl_CompanyUser where tbl_CompanyUser.isdelete = '0') ");
            strSql.AppendFormat(" where IsDelete = '0' and (FinanceSum - HasCheckMoney) <> 0 and SalerId > 0 ");
            strSql.AppendFormat(" {0} ", this.GetSqlWhere(model, HaveUserIds, ref strOrder));
            strSql.Append(" group by SalerId ");
            if (!string.IsNullOrEmpty(strOrder))
                strSql.AppendFormat(" order by {0} ", strOrder);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                EyouSoft.Model.StatisticStructure.AccountAgeStatistic tmpModel = null;
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.StatisticStructure.AccountAgeStatistic();
                    if (!dr.IsDBNull(0))
                    {
                        tmpModel.SalesClerk = new EyouSoft.Model.StatisticStructure.StatisticOperator();
                        tmpModel.SalesClerk.OperatorId = dr.GetInt32(0);
                        if (!dr.IsDBNull(1))
                            tmpModel.SalesClerk.OperatorName = dr.GetString(1);
                    }
                    if (!dr.IsDBNull(2))
                        tmpModel.ArrearageSum = dr.GetDecimal(2);
                    if (!dr.IsDBNull(3))
                        tmpModel.MaxArrearageTime = dr.GetDateTime(3);

                    list.Add(tmpModel);
                }
            }

            return list;
        }

        /*/// <summary>
        /// 获取账龄分析-按客户单位统计
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWei> GetZhangLingAnKeHuDanWei(int pageSize, int pageIndex, ref int recordCount, int companyId, EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWeiChaXun searchInfo)
        {
            IList<EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWei> items = new List<EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWei>();
            string cmdQuery = string.Empty;
            string tableName = "view_ZhangLingAnKeHuDanWei";
            string primaryKey = "Id";
            string orderByString = "WeiShouKuan DESC";
            string fields = "*";

            #region SQL
            cmdQuery += string.Format("CompanyId={0} AND WeiShouKuan>0 ", companyId);

            if (searchInfo != null)
            {
                if (!string.IsNullOrEmpty(searchInfo.KeHuName))
                {
                    cmdQuery += string.Format(" AND Name LIKE '%{0}%' ",searchInfo.KeHuName);
                }

                if (searchInfo.SortType.HasValue)
                {
                    switch (searchInfo.SortType.Value)
                    {
                        case 0: orderByString = "WeiShouKuan DESC"; break;
                        case 1: orderByString = "WeiShouKuan ASC"; break;
                        case 2: orderByString = "EarlyTime DESC"; break;
                        case 3: orderByString = "EarlyTime ASC"; break;
                    }
                }
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields, cmdQuery, orderByString))
            {
                while (rdr.Read())
                {
                    var item = new EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWei();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("EarlyTime"))) item.EarlyTime = rdr.GetDateTime(rdr.GetOrdinal("EarlyTime"));
                    item.KeHuId = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    item.KeHuName = rdr["Name"].ToString();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("WeiShouKuan"))) item.WeiShouKuan = rdr.GetDecimal(rdr.GetOrdinal("WeiShouKuan"));

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取账龄分析-按客户单位统计合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="weiShouHeJi">未收款合计</param>
        public void GetZhangLingAnKeHuDanWei(int companyId, EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWeiChaXun searchInfo, out decimal weiShouHeJi)
        {
            weiShouHeJi = 0;
            string cmdText = string.Empty;

            #region SQL
            cmdText += string.Format("SELECT SUM(WeiShouKuan) AS WeiShouKuanHeJi  FROM view_ZhangLingAnKeHuDanWei WHERE CompanyId={0}", companyId);
            if (searchInfo != null)
            {
                cmdText += string.Format("AND Name LIKE '%{0}%'", searchInfo.KeHuName);
            }
            #endregion


            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    weiShouHeJi = rdr.IsDBNull(0) ? 0 : rdr.GetDecimal(0);
                }
            }
        }*/

        /// <summary>
        /// 获取账龄分析-按客户单位统计
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="us">用户编号集合 用,间隔</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWei> GetZhangLingAnKeHuDanWei(int pageSize, int pageIndex, ref int recordCount, int companyId, EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWeiChaXun searchInfo, string us)
        {
            IList<EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWei> items = new List<EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWei>();

            #region SQL
            StringBuilder table = new StringBuilder();

            table.Append(" SELECT BuyCompanyId AS KeHuId,SUM(FinanceSum-HasCheckMoney) AS WeiShouKuan,MIN(LeaveDate) AS EarlyTime ");
            table.AppendFormat(" FROM tbl_TourOrder WHERE IsDelete = '0' AND SellCompanyid={0} ", companyId);

            if (searchInfo != null||!string.IsNullOrEmpty(us))
            {
                if (searchInfo.LSDate.HasValue||searchInfo.LEDate.HasValue||!string.IsNullOrEmpty(us))
                {
                    table.Append(" AND EXISTS(SELECT 1 FROM tbl_Tour AS A WHERE A.TourId=tbl_TourOrder.TourId ");
                    if (searchInfo.LSDate.HasValue)
                    {
                        table.AppendFormat(" AND LeaveDate>='{0}' ", searchInfo.LSDate.Value);
                    }
                    if (searchInfo.LEDate.HasValue)
                    {
                        table.AppendFormat(" AND LeaveDate<='{0}' ", searchInfo.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (!string.IsNullOrEmpty(us))
                    {
                        table.AppendFormat(" AND OperatorId IN({0}) ", us);
                    }
                    table.Append(")");
                }
            }

            table.AppendFormat(" GROUP BY BuyCompanyId ");

            string fields = "*,(SELECT Name FROM tbl_Customer AS A WHERE A.Id=KeHuId) AS KeHuName";
            string query = " 1=1 ";
            if (searchInfo != null)
            {
                if (!string.IsNullOrEmpty(searchInfo.KeHuName))
                {
                    query += string.Format(" AND EXISTS(SELECT 1 FROM tbl_Customer WHERE Id=KeHuId AND Name LIKE '%{0}%') ", searchInfo.KeHuName);
                }
            }

            string orderByString = "WeiShouKuan DESC";

            if (searchInfo.SortType.HasValue)
            {
                switch (searchInfo.SortType.Value)
                {
                    case 0: orderByString = "WeiShouKuan DESC"; break;
                    case 1: orderByString = "WeiShouKuan ASC"; break;
                    case 2: orderByString = "EarlyTime DESC"; break;
                    case 3: orderByString = "EarlyTime ASC"; break;
                }
            }

            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReaderBySqlTbl(_db, pageSize, pageIndex, ref recordCount, table.ToString(), fields, query, orderByString, false))
            {
                while (rdr.Read())
                {
                    var item = new EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWei();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("EarlyTime"))) item.EarlyTime = rdr.GetDateTime(rdr.GetOrdinal("EarlyTime"));
                    item.KeHuId = rdr.GetInt32(rdr.GetOrdinal("KeHuId"));
                    item.KeHuName = rdr["KeHuName"].ToString();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("WeiShouKuan"))) item.WeiShouKuan = rdr.GetDecimal(rdr.GetOrdinal("WeiShouKuan"));

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取账龄分析-按客户单位统计合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="us">用户编号集合 用,间隔</param>
        /// <param name="weiShouHeJi">未收款合计</param>
        public void GetZhangLingAnKeHuDanWei(int companyId, EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWeiChaXun searchInfo, out decimal weiShouHeJi, string us)
        {
            weiShouHeJi = 0;
            StringBuilder cmdText = new StringBuilder();

            #region SQL
            cmdText.AppendFormat(" SELECT SUM(FinanceSum-HasCheckMoney) AS WeiShouHeJi FROM tbl_TourOrder WHERE IsDelete = '0' AND SellCompanyId={0}", companyId);
            if (searchInfo != null || !string.IsNullOrEmpty(us))
            {
                if (searchInfo.LSDate.HasValue || searchInfo.LEDate.HasValue || !string.IsNullOrEmpty(us))
                {
                    cmdText.Append(" AND EXISTS(SELECT 1 FROM tbl_Tour AS A WHERE A.TourId=tbl_TourOrder.TourId ");
                    if (searchInfo.LSDate.HasValue)
                    {
                        cmdText.AppendFormat(" AND LeaveDate>='{0}' ", searchInfo.LSDate.Value);
                    }
                    if (searchInfo.LEDate.HasValue)
                    {
                        cmdText.AppendFormat(" AND LeaveDate<='{0}' ", searchInfo.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (!string.IsNullOrEmpty(us))
                    {
                        cmdText.AppendFormat(" AND OperatorId IN({0}) ", us);
                    }
                    cmdText.Append(")");
                }
            }
            if (searchInfo != null)
            {
                if (!string.IsNullOrEmpty(searchInfo.KeHuName))
                {
                    cmdText.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_Customer WHERE Id=tbl_TourOrder.BuyCompanyId AND Name LIKE '%{0}%') ", searchInfo.KeHuName);
                }
            }
            #endregion


            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    weiShouHeJi = rdr.IsDBNull(0) ? 0 : rdr.GetDecimal(0);
                }
            }
        }

        /// <summary>
        /// 获取支出账龄分析（按供应商单位）
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="chaXun">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.FXZhiChuZhangLingInfo> GetZhiChuZhangLing(int pageSize, int pageIndex, ref int recordCount, int companyId, EyouSoft.Model.StatisticStructure.FXZhiChuZhangLingChaXunInfo chaXun)
        {
            IList<EyouSoft.Model.StatisticStructure.FXZhiChuZhangLingInfo> items = new List<EyouSoft.Model.StatisticStructure.FXZhiChuZhangLingInfo>();

            #region SQL
            StringBuilder _tbl = new StringBuilder();
            _tbl.Append(" SELECT A.Id,A.UnitName AS GongYingShang,A.SupplierType ");
            _tbl.Append(" ,(SELECT ISNULL(SUM(TotalAmount),0) FROM tbl_StatAllOut AS B WHERE B.SupplierId=A.Id AND B.IsDelete='0' {0}) AS JiaoYiZongE ");
            //_tbl.Append(" ,(SELECT ISNULL(SUM(HasCheckAmount),0) FROM tbl_StatAllOut AS B WHERE B.SupplierId=A.Id AND B.IsDelete='0') AS YiFuZongE ");
            _tbl.Append(" ,(SELECT ISNULL(SUM(TotalAmount-HasCheckAmount),0) FROM tbl_StatAllOut AS B WHERE B.SupplierId=A.Id AND B.IsDelete='0' {0}) AS WeiFuZongE ");
            _tbl.Append(" ,(SELECT ISNULL(MIN(CreateTime),GETDATE()) FROM tbl_StatAllOut AS B WHERE B.SupplierId=A.Id AND B.IsDelete='0' {0}) AS EarlyTime ");
            _tbl.AppendFormat(" FROM tbl_CompanySupplier AS A WHERE A.CompanyId={0} ",companyId);

            if (chaXun != null)
            {
                if (chaXun.GongYingShangLeiXing.HasValue)
                {
                    _tbl.AppendFormat(" AND A.SupplierType={0} ", (int)chaXun.GongYingShangLeiXing.Value);
                }

                if (!string.IsNullOrEmpty(chaXun.GongYingShang))
                {
                    _tbl.AppendFormat(" AND A.UnitName LIKE '%{0}%' ", chaXun.GongYingShang);
                }
            }

            string _tourQuery = string.Empty;

            if (chaXun != null)
            {
                if (chaXun.LEDate.HasValue
                    || chaXun.LSDate.HasValue
                    || (chaXun.OperatorDepartIds != null && chaXun.OperatorDepartIds.Length > 0)
                    || (chaXun.OperatorIds != null && chaXun.OperatorIds.Length > 0))
                {
                    _tourQuery += " AND EXISTS(SELECT 1 FROM tbl_Tour AS C WHERE C.TourId=B.TourId ";

                    if (chaXun.LEDate.HasValue)
                    {
                        _tourQuery += string.Format(" AND C.LeaveDate<='{0}' ", chaXun.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (chaXun.LSDate.HasValue)
                    {
                        _tourQuery += string.Format(" AND C.LeaveDate>='{0}' ", chaXun.LSDate.Value);
                    }
                    if (chaXun.OperatorDepartIds != null && chaXun.OperatorDepartIds.Length > 0)
                    {
                        _tourQuery += string.Format(" AND OperatorId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByArray(chaXun.OperatorDepartIds));
                    }
                    if (chaXun.OperatorIds != null && chaXun.OperatorIds.Length > 0)
                    {
                        _tourQuery += string.Format(" AND OperatorId IN({0}) ",Utils.GetSqlIdStrByArray(chaXun.OperatorIds));
                    }

                    _tourQuery += " ) ";
                }
            }

            string fields = "*";
            string query = " JiaoYiZongE > 0 ";//WeiFuZongE>0

            string orderByString = "WeiFuZongE DESC";

            if (chaXun != null)
            {
                switch (chaXun.SortType)
                {
                    case 1: orderByString = "WeiFuZongE ASC"; break;
                    case 2: orderByString = "EarlyTime DESC"; break;
                    case 3: orderByString = "EarlyTime ASC"; break;
                    case 4: orderByString = "JiaoYiZongE DESC"; break;
                    case 5: orderByString = "JiaoYiZongE ASC"; break;
                }
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReaderBySqlTbl(_db, pageSize, pageIndex, ref recordCount, string.Format(_tbl.ToString(),_tourQuery), fields, query, orderByString, false))
            {
                while (rdr.Read())
                {
                    var item = new EyouSoft.Model.StatisticStructure.FXZhiChuZhangLingInfo();

                    item.EarlyTime = rdr.GetDateTime(rdr.GetOrdinal("EarlyTime"));
                    item.GongYingShang = rdr["GongYingShang"].ToString();
                    item.JiaoYiZongE = rdr.GetDecimal(rdr.GetOrdinal("JiaoYiZongE"));
                    item.QianKuanZongE = rdr.GetDecimal(rdr.GetOrdinal("WeiFuZongE"));
                    item.GongYingShangLeiXing = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)rdr.GetByte(rdr.GetOrdinal("SupplierType"));

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取支出账龄分析合计信息（按供应商单位）
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="chaXun">查询信息</param>
        /// <param name="weiFuKuanHeJi">未付金额合计</param>
        /// <param name="zongJinEHeJi">交易总金额合计</param>
        public void GetZhiChuZhangLing(int companyId, EyouSoft.Model.StatisticStructure.FXZhiChuZhangLingChaXunInfo chaXun, out decimal weiFuKuanHeJi, out decimal zongJinEHeJi)
        {
            weiFuKuanHeJi = 0;
            zongJinEHeJi = 0;

            StringBuilder cmdText = new StringBuilder();

            cmdText.Append("SELECT SUM(WeiFuZongE) AS WeiFuZongEHeJi,SUM(ZongJinE) AS ZongJinEHeJi FROM ( ");
            cmdText.Append(" SELECT ");
            cmdText.Append(" (SELECT ISNULL(SUM(TotalAmount-HasCheckAmount),0) FROM tbl_StatAllOut AS B WHERE B.SupplierId=A.Id AND B.IsDelete='0' {0}) AS WeiFuZongE ");
            cmdText.Append(" ,(SELECT ISNULL(SUM(TotalAmount),0) FROM tbl_StatAllOut AS B WHERE B.SupplierId=A.Id AND B.IsDelete='0' {0}) AS ZongJinE");
            cmdText.AppendFormat(" FROM tbl_CompanySupplier AS A WHERE A.CompanyId={0} ", companyId);

            string _tourQuery = string.Empty;

            if (chaXun != null)
            {
                if (chaXun.LEDate.HasValue
                    || chaXun.LSDate.HasValue
                    || (chaXun.OperatorDepartIds != null && chaXun.OperatorDepartIds.Length > 0)
                    || (chaXun.OperatorIds != null && chaXun.OperatorIds.Length > 0))
                {
                    _tourQuery += " AND EXISTS(SELECT 1 FROM tbl_Tour AS C WHERE C.TourId=B.TourId ";

                    if (chaXun.LEDate.HasValue)
                    {
                        _tourQuery += string.Format(" AND C.LeaveDate<='{0}' ", chaXun.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (chaXun.LSDate.HasValue)
                    {
                        _tourQuery += string.Format(" AND C.LeaveDate>='{0}' ", chaXun.LSDate.Value);
                    }
                    if (chaXun.OperatorDepartIds != null && chaXun.OperatorDepartIds.Length > 0)
                    {
                        _tourQuery += string.Format(" AND OperatorId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByArray(chaXun.OperatorDepartIds));
                    }
                    if (chaXun.OperatorIds != null && chaXun.OperatorIds.Length > 0)
                    {
                        _tourQuery += string.Format(" AND OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(chaXun.OperatorIds));
                    }

                    _tourQuery += " ) ";
                }
            }

            if (chaXun != null)
            {
                if (chaXun.GongYingShangLeiXing.HasValue)
                {
                    cmdText.AppendFormat(" AND A.SupplierType={0} ", (int)chaXun.GongYingShangLeiXing.Value);
                }

                if (!string.IsNullOrEmpty(chaXun.GongYingShang))
                {
                    cmdText.AppendFormat(" AND A.UnitName LIKE '%{0}%' ", chaXun.GongYingShang);
                }
            }

            cmdText.Append(")D WHERE D.ZongJinE>0 ");

            DbCommand cmd = _db.GetSqlStringCommand(string.Format(cmdText.ToString(), _tourQuery));

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) weiFuKuanHeJi = rdr.GetDecimal(0);
                    if (!rdr.IsDBNull(1)) zongJinEHeJi = rdr.GetDecimal(1);
                }
            }

        }
        #endregion

    }
}
