#region 命名空间
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data.Common;
using System.Data;
#endregion

namespace EyouSoft.DAL.StatisticStructure
{
    /// <summary>
    /// 销售统计分析 区域销售统计分析
    /// </summary>
    /// 陈志仁  开发时间:2012-02-27
    public class SoldStatistic : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.StatisticStructure.ISoldStatistic
    {
        private readonly EyouSoft.Data.EyouSoftTBL dcDal = null;
        private readonly Database _db = null;
        private const string sqlAreaStat = "select CityId, CityName,SaleId,Saler from tbl_Customer where CompanyId =1 AND CityId > 0  group by CityId,CityName,SaleId,Saler";
        /// <summary>
        /// 构造函数
        /// </summary>
        public SoldStatistic()
        {
            _db = this.SystemStore;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this.SystemStore.ConnectionString);
        }
        #region 销售统计分析
        /// <summary>
        /// 按指定条件获取客户资料信息集合
        /// </summary>
        /// <param name="companyId">公司（专线）编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seachInfo">查询条件业务实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CustomerInfo> GetCustomers(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.CompanyStructure.MCustomerSoldStatSearchInfo seachInfo)
        {
            IList<EyouSoft.Model.CompanyStructure.CustomerInfo> items = new List<EyouSoft.Model.CompanyStructure.CustomerInfo>();

            StringBuilder cmdQuery = new StringBuilder();
            string tableName = String.Format("select Id,ProvinceName,CityId,CityName,[Name],Saler,SaleId,ContactName,Phone,Mobile,Fax,CompanyId,IsNull((select sum(O.PeopleNumber-O.LeaguePepoleNum) from tbl_TourOrder AS O INNER JOIN tbl_Tour AS T ON O.TourId = T.TourId where T.CompanyId = {0} AND (T.TourType in (0,1)) And T.IsDelete= 0 And O.IsDelete = 0 AND O.BuyCompanyID=tbl_Customer.Id AND O.OrderState=5),0) AS TradeNum,IsNull((select sum(O.PeopleNumber-O.LeaguePepoleNum) from tbl_TourOrder AS O INNER JOIN tbl_Tour AS T ON O.TourId = T.TourId where T.CompanyId = {0} AND (T.TourType=0) AND T.IsDelete= 0 And O.IsDelete = 0 AND O.BuyCompanyID=tbl_Customer.Id AND O.OrderState=5),0) AS TradeTourNum from tbl_Customer WHERE Companyid={0} AND IsDelete='0'", companyId);
            bool IsGroupBy = false;
            string orderByString = "TradeNum DESC";
            string fields = "*";

            #region 拼接查询条件
            cmdQuery.Append(" (1=1)");
            if (seachInfo != null)
            {
                //出团日期
                if (seachInfo.TourStartDate.HasValue || seachInfo.TourEndDate.HasValue || seachInfo.AreaId.HasValue)
                {
                    //出团日期查询
                    string tmpTourLeaveDateSearchPara = String.Empty;
                    if (seachInfo.TourStartDate.HasValue)
                    {
                        tmpTourLeaveDateSearchPara = " and (T.LeaveDate > '" + seachInfo.TourStartDate.Value.AddDays(-1) + "') ";
                    }
                    if (seachInfo.TourEndDate.HasValue)
                    {
                        tmpTourLeaveDateSearchPara += " and (T.LeaveDate < '" + seachInfo.TourEndDate.Value.AddDays(1) + "') ";

                    }
                    if (seachInfo.AreaId.HasValue)
                    {
                        tmpTourLeaveDateSearchPara += " AND (T.AreaId=" + seachInfo.AreaId.Value + ") ";
                    }
                    tableName = String.Format("select Id,ProvinceName,CityId,CityName,[Name],Saler,SaleId,ContactName,Phone,Mobile,Fax,CompanyId,IsNull((SELECT sum(O.PeopleNumber-O.LeaguePepoleNum) FROM tbl_TourOrder AS O INNER JOIN tbl_Tour AS T ON O.TourId = T.TourId WHERE T.CompanyId = {0} AND (T.TourType in (0,1)) AND T.IsDelete= 0 And O.IsDelete = 0 AND O.BuyCompanyID=tbl_Customer.Id AND O.OrderState=5 {1}),0) as TradeNum,IsNull((SELECT sum(O.PeopleNumber-O.LeaguePepoleNum) FROM tbl_TourOrder AS O INNER JOIN tbl_Tour AS T ON O.TourId = T.TourId WHERE T.CompanyId = {0} AND (T.TourType=0) AND T.IsDelete= 0 And O.IsDelete = 0 AND O.BuyCompanyID=tbl_Customer.Id AND O.OrderState=5 {2}),0) AS TradeTourNum from tbl_Customer WHERE Companyid={0} AND IsDelete='0'", companyId, tmpTourLeaveDateSearchPara, tmpTourLeaveDateSearchPara);
                    cmdQuery.Append(" AND (Id in (select O.BuyCompanyID From tbl_TourOrder O,tbl_Tour T WHERE O.TourId = T.TourId " + tmpTourLeaveDateSearchPara + "))");

                }
                if (seachInfo.CityIdList != null && seachInfo.CityIdList.Length > 0)
                {
                    string CityIdList = "";
                    for (int i = 0; i < seachInfo.CityIdList.Length; i++)
                        CityIdList = CityIdList + seachInfo.CityIdList[i].ToString() + ',';
                    CityIdList = CityIdList.Trim(',');
                    cmdQuery.AppendFormat(" AND CityId in ({0}) ", CityIdList);
                }
                if (seachInfo.CustomerId.HasValue)
                {
                    cmdQuery.AppendFormat(" AND Id = {0} ", seachInfo.CustomerId.Value);
                }
                if (!string.IsNullOrEmpty(seachInfo.SellerName))
                {
                    cmdQuery.AppendFormat(" AND Saler LIKE '%{0}%' ", seachInfo.SellerName);
                }
                if (seachInfo.SellerIds != null && seachInfo.SellerIds.Length > 0)
                {
                    cmdQuery.AppendFormat(" AND SaleId IN({0}) ", EyouSoft.Toolkit.Utils.GetSqlIdStrByArray(seachInfo.SellerIds));
                }
                if (!string.IsNullOrEmpty(seachInfo.Mobile))
                {
                    cmdQuery.AppendFormat(" AND Mobile LIKE '%{0}%' ", seachInfo.Mobile);
                }
                if (seachInfo.DeptIds != null && seachInfo.DeptIds.Length > 0 && (seachInfo.SellerIds == null || seachInfo.SellerIds.Length == 0))
                {
                    cmdQuery.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_CompanyUser WHERE Id=SaleId AND DepartId IN({0}) ) ", EyouSoft.Toolkit.Utils.GetSqlIdStrByArray(seachInfo.DeptIds));
                }
                //交易数比较
                switch (seachInfo.SearchCompare)
                {
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.大于等于:
                        cmdQuery.AppendFormat(" AND TradeNum > {0} ", seachInfo.SendTourPeopleNumber - 1);
                        break;
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.等于:
                        cmdQuery.AppendFormat(" AND TradeNum = {0} ", seachInfo.SendTourPeopleNumber);
                        break;
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.小于等于:
                        cmdQuery.AppendFormat(" AND TradeNum < {0} ", seachInfo.SendTourPeopleNumber + 1);
                        break;
                }

                switch (seachInfo.XSTJFXOrderByType)
                {
                    case 0:
                        orderByString = "TradeNum DESC";
                        break;
                    case 1:
                        orderByString = "TradeNum ASC";
                        break;
                    case 2:
                        orderByString = "TradeTourNum DESC";
                        break;
                    case 3:
                        orderByString = "TradeTourNum ASC";
                        break;
                    case 4:
                        orderByString = "TradeNum-TradeTourNum DESC";
                        break;
                    case 5:
                        orderByString = "TradeNum-TradeTourNum ASC";
                        break;
                    default:
                        orderByString = "TradeNum DESC";
                        break;
                }
            }

            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReaderBySqlTbl(this._db, pageSize, pageIndex, ref recordCount, tableName, fields, cmdQuery.ToString(), orderByString, IsGroupBy))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.CompanyStructure.CustomerInfo item = new EyouSoft.Model.CompanyStructure.CustomerInfo();
                    item.CityName = rdr.IsDBNull(rdr.GetOrdinal("CityName")) ? "" : rdr["CityName"].ToString();
                    item.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    item.ContactName = rdr.IsDBNull(rdr.GetOrdinal("ContactName")) ? "" : rdr["ContactName"].ToString();
                    item.Fax = rdr.IsDBNull(rdr.GetOrdinal("Fax")) ? "" : rdr["Fax"].ToString();
                    item.Id = rdr.GetInt32(rdr.GetOrdinal("Id"));
                    item.Mobile = rdr.IsDBNull(rdr.GetOrdinal("Mobile")) ? "" : rdr["Mobile"].ToString();
                    item.Name = rdr.IsDBNull(rdr.GetOrdinal("Name")) ? "" : rdr["Name"].ToString();
                    item.Phone = rdr.IsDBNull(rdr.GetOrdinal("Phone")) ? "" : rdr["Phone"].ToString();
                    item.ProvinceName = rdr.IsDBNull(rdr.GetOrdinal("ProvinceName")) ? "" : rdr["ProvinceName"].ToString();
                    item.SaleId = rdr.GetInt32(rdr.GetOrdinal("SaleId"));
                    item.Saler = rdr.IsDBNull(rdr.GetOrdinal("Saler")) ? "" : rdr["Saler"].ToString();
                    item.TradeTourNum = rdr.GetInt32(rdr.GetOrdinal("TradeTourNum"));
                    item.TradeNum = rdr.GetInt32(rdr.GetOrdinal("TradeNum"));
                    items.Add(item);
                }
            }
            return items;
        }
        /// <summary>
        /// 按指定条件获取 人数合计信息
        /// </summary>
        /// <param name="companyId">公司（专线）编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="seachInfo">查询条件业务实体</param>
        /// <returns></returns>
        public Dictionary<string, int> GetSoldStatSumInfo(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.CompanyStructure.MCustomerSoldStatSearchInfo seachInfo)
        {
            Dictionary<string, int> SoldStat = new Dictionary<string, int>();
            StringBuilder cmdQuery = new StringBuilder();
            string strSql = "select IsNUll(sum(t.TradeNum),0) as TradeNum,IsNull(sum(t.TradeTourNum),0) as TradeTourNum from (";
            string tableName = String.Format("select IsNull((select sum(O.PeopleNumber-O.LeaguePepoleNum) from tbl_TourOrder AS O INNER JOIN tbl_Tour AS T ON O.TourId = T.TourId where T.CompanyId = {0} AND (T.TourType in (0,1)) And T.IsDelete= 0 And O.IsDelete = 0 AND O.BuyCompanyID=tbl_Customer.Id AND O.OrderState=5),0) AS TradeNum,IsNull((select sum(O.PeopleNumber-O.LeaguePepoleNum) from tbl_TourOrder AS O INNER JOIN tbl_Tour AS T ON O.TourId = T.TourId where T.CompanyId = {0} AND (T.TourType=0) AND T.IsDelete= 0 And O.IsDelete = 0 AND O.BuyCompanyID=tbl_Customer.Id AND O.OrderState=5),0) AS TradeTourNum from tbl_Customer WHERE Companyid={0} AND IsDelete='0'", companyId);
            #region 拼接查询条件
            if (seachInfo != null)
            {
                //出团日期
                if (seachInfo.TourStartDate.HasValue || seachInfo.TourEndDate.HasValue || seachInfo.AreaId.HasValue)
                {
                    //出团日期查询
                    string tmpTourLeaveDateSearchPara = String.Empty;
                    if (seachInfo.TourStartDate.HasValue)
                    {
                        tmpTourLeaveDateSearchPara = " and (T.LeaveDate > '" + seachInfo.TourStartDate.Value.AddDays(-1) + "') ";
                    }
                    if (seachInfo.TourEndDate.HasValue)
                    {
                        tmpTourLeaveDateSearchPara += " and (T.LeaveDate < '" + seachInfo.TourEndDate.Value.AddDays(1) + "') ";

                    }
                    if (seachInfo.AreaId.HasValue)
                    {
                        tmpTourLeaveDateSearchPara += " and (T.AreaId = " + seachInfo.AreaId + ") ";
                    }
                    tableName = String.Format("select Id,ProvinceName,CityId,CityName,[Name],Saler,SaleId,ContactName,Phone,Mobile,Fax,CompanyId,IsNull((SELECT sum(O.PeopleNumber-O.LeaguePepoleNum) FROM tbl_TourOrder AS O INNER JOIN tbl_Tour AS T ON O.TourId = T.TourId WHERE T.CompanyId = {0} AND (T.TourType in (0,1)) AND T.IsDelete= 0 And O.IsDelete = 0 AND O.BuyCompanyID=tbl_Customer.Id AND O.OrderState=5 {1}),0) as TradeNum,IsNull((SELECT sum(O.PeopleNumber-O.LeaguePepoleNum) FROM tbl_TourOrder AS O INNER JOIN tbl_Tour AS T ON O.TourId = T.TourId WHERE T.CompanyId = {0} AND (T.TourType=0) AND T.IsDelete= 0 And O.IsDelete = 0 AND O.BuyCompanyID=tbl_Customer.Id AND O.OrderState=5 {2}),0) AS TradeTourNum from tbl_Customer WHERE Companyid={0} AND IsDelete='0'", companyId, tmpTourLeaveDateSearchPara, tmpTourLeaveDateSearchPara);
                    cmdQuery.Append(" AND (Id in (select O.BuyCompanyID From tbl_TourOrder O,tbl_Tour T WHERE O.TourId = T.TourId " + tmpTourLeaveDateSearchPara + "))");

                }
                if (seachInfo.CityIdList != null && seachInfo.CityIdList.Length > 0)
                {
                    string CityIdList = "";
                    for (int i = 0; i < seachInfo.CityIdList.Length; i++)
                        CityIdList = CityIdList + seachInfo.CityIdList[i].ToString() + ',';
                    CityIdList = CityIdList.Trim(',');
                    cmdQuery.AppendFormat(" AND CityId in ({0}) ", CityIdList);
                }
                if (seachInfo.CustomerId.HasValue)
                {
                    cmdQuery.AppendFormat(" AND Id = {0} ", seachInfo.CustomerId.Value);
                }
                if (!string.IsNullOrEmpty(seachInfo.SellerName))
                {
                    cmdQuery.AppendFormat(" AND Saler LIKE '%{0}%' ", seachInfo.SellerName);
                }
                if (seachInfo.SellerIds != null && seachInfo.SellerIds.Length > 0)
                {
                    cmdQuery.AppendFormat(" AND SaleId IN({0}) ", EyouSoft.Toolkit.Utils.GetSqlIdStrByArray(seachInfo.SellerIds));
                }
                if (!string.IsNullOrEmpty(seachInfo.Mobile))
                {
                    cmdQuery.AppendFormat(" AND Mobile LIKE '%{0}%' ", seachInfo.Mobile);
                }
                if (seachInfo.DeptIds != null && seachInfo.DeptIds.Length > 0 && (seachInfo.SellerIds == null || seachInfo.SellerIds.Length == 0))
                {
                    cmdQuery.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_CompanyUser WHERE Id=tbl_Customer.SaleId AND DepartId IN({0}) ) ", EyouSoft.Toolkit.Utils.GetSqlIdStrByArray(seachInfo.DeptIds));
                }
            }

            #endregion
            strSql = strSql + tableName + cmdQuery.ToString() + ") as t";
            //交易数比较
            switch (seachInfo.SearchCompare)
            {
                case EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.大于等于:
                    strSql = strSql + String.Format(" WHERE t.TradeNum > {0} ", seachInfo.SendTourPeopleNumber - 1);
                    break;
                case EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.等于:
                    strSql = strSql + String.Format(" WHERE t.TradeNum = {0} ", seachInfo.SendTourPeopleNumber);
                    break;
                case EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.小于等于:
                    strSql = strSql + String.Format(" WHERE t.TradeNum < {0} ", seachInfo.SendTourPeopleNumber + 1);
                    break;
            }
            DbCommand cmd = this._db.GetSqlStringCommand(strSql);
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    SoldStat.Add("TradeNum", rdr.GetInt32(rdr.GetOrdinal("TradeNum")));
                    SoldStat.Add("TradeTourNum", rdr.GetInt32(rdr.GetOrdinal("TradeTourNum")));
                }
            }
            return SoldStat;
        }
        #endregion
        #region 区域销售统计分析
        /// <summary>
        /// 按指定条件获取客户资料信息集合
        /// </summary>
        /// <param name="companyId">公司（专线）编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.AreaDepartStat> getAreaStatList(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.StatisticStructure.AreaSoldStatSearch searchInfo)
        {
            IList<EyouSoft.Model.StatisticStructure.AreaDepartStat> items = new List<EyouSoft.Model.StatisticStructure.AreaDepartStat>();

            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_Customer";
            string primaryKey = "CityId";
            string orderByString = "CityId";
            string fields = "CityId, CityName,SaleId,Saler";
            cmdQuery.AppendFormat("CompanyId ={0} AND IsDelete='0' AND CityId > 0", companyId);
            if (!String.IsNullOrEmpty(searchInfo.SalerIds))
            {
                cmdQuery.AppendFormat(" AND SaleId IN ({0})", searchInfo.SalerIds);
            }
            if (!String.IsNullOrEmpty(searchInfo.CityIds))
            {
                cmdQuery.AppendFormat(" AND CityId IN ({0})", searchInfo.CityIds);
            }
            cmdQuery.Append(" group by CityId,CityName,SaleId,Saler ");
            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields, cmdQuery.ToString(), orderByString, true))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.StatisticStructure.AreaDepartStat item = new EyouSoft.Model.StatisticStructure.AreaDepartStat();
                    item.CityId = rdr.GetInt32(rdr.GetOrdinal("CityId"));
                    item.CityName = rdr.IsDBNull(rdr.GetOrdinal("CityName")) ? "" : rdr["CityName"].ToString();
                    item.SaleId = rdr.GetInt32(rdr.GetOrdinal("SaleId"));
                    item.Saler = rdr.IsDBNull(rdr.GetOrdinal("Saler")) ? "" : rdr["Saler"].ToString();
                    items.Add(item);
                }
            }
            return items;
        }
        /// <summary>
        /// 取得有订单信息的部门名称列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="startTime"> 开始时间(出团时间)</param>
        /// <param name="entTime">结束时间(出团时间)</param>
        /// <param name="tourType">团队类型</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.AreaStatDepartList> getDepartList(int companyId, DateTime? startTime, DateTime? entTime
            , Model.EnumType.TourStructure.TourType? tourType)
        {
            IList<EyouSoft.Model.StatisticStructure.AreaStatDepartList> items = new List<EyouSoft.Model.StatisticStructure.AreaStatDepartList>();
            StringBuilder cmdQuery = new StringBuilder();
            cmdQuery.AppendFormat("select U.DepartId,U.DepartName from tbl_CompanyUser U,tbl_TourOrder O where U.Id = O.OperatorId AND U.CompanyId = {0} AND U.IsDelete = '0' AND U.DepartId > 0 AND O.SellCompanyId = {0} AND O.OrderState not in (3,4) AND O.IsDelete = '0'", companyId);
            //出团时间
            if (startTime.HasValue || entTime.HasValue)
            {
                if (startTime.HasValue)
                    cmdQuery.AppendFormat(" AND O.LeaveDate > '{0}' ", startTime.Value.ToShortDateString());
                if (entTime.HasValue)
                    cmdQuery.AppendFormat(" AND O.LeaveDate < '{0}' ", entTime.Value.ToShortDateString());
            }
            if (tourType.HasValue)
            {
                cmdQuery.AppendFormat(" and O.TourClassId = {0} ", (int)tourType.Value);
            }
            cmdQuery.Append(" group by U.DepartId,U.DepartName");
            DbCommand dc = this._db.GetSqlStringCommand(cmdQuery.ToString());
            using (IDataReader rdr = DbHelper.ExecuteReader(dc, this._db))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.StatisticStructure.AreaStatDepartList item = new EyouSoft.Model.StatisticStructure.AreaStatDepartList();
                    item.DepartId = rdr.GetInt32(rdr.GetOrdinal("DepartId"));
                    item.DepartName = rdr.IsDBNull(rdr.GetOrdinal("DepartName")) ? "" : rdr["DepartName"].ToString();
                    items.Add(item);
                }
            }
            return items;
        }
        /// <summary>
        /// 取得指定年月某个城市某个部门的人数统计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="startTime">开始时间(出团时间)</param>
        /// <param name="entTime">结束时间(出团时间)</param>
        /// <param name="cityId">城市编号</param>
        /// <param name="DeptId">部门编号</param>
        /// <param name="salerId">责任销售编号</param>
        /// <param name="tourType">团队类型</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.AreaDepartStatInfo> getDepartStatList(int companyId, DateTime? startTime
            , DateTime? entTime, int cityId, int salerId, Model.EnumType.TourStructure.TourType? tourType)
        {
            IList<EyouSoft.Model.StatisticStructure.AreaDepartStatInfo> items = new List<EyouSoft.Model.StatisticStructure.AreaDepartStatInfo>();
            StringBuilder cmdQuery = new StringBuilder();
            cmdQuery.AppendFormat("select U.DepartId,sum(O.PeopleNumber-O.LeaguePepoleNum) AS TourNumber from tbl_TourOrder O,tbl_CompanyUser U where O.PerTimeSellerId=U.Id AND O.SellCompanyId = {0} AND O.OrderState not in (3,4) AND O.IsDelete = '0'", companyId);
            //出团时间
            if (startTime.HasValue || entTime.HasValue)
            {
                if (startTime.HasValue)
                    cmdQuery.AppendFormat(" AND O.LeaveDate > '{0}' ", startTime.Value.ToShortDateString());
                if (entTime.HasValue)
                    cmdQuery.AppendFormat(" AND O.LeaveDate < '{0}' ", entTime.Value.ToShortDateString());
            }
            if (tourType.HasValue)
            {
                cmdQuery.AppendFormat(" and O.TourClassId = {0} ", (int)tourType.Value);
            }
            //区域
            cmdQuery.AppendFormat(" AND BuyCompanyID IN (select Id from tbl_Customer where CompanyId = {0} AND CityId = {1} AND SaleId={2})", companyId, cityId, salerId);
            //用户未删除且已指定部门
            cmdQuery.Append(" AND U.IsDelete = '0' AND U.DepartId > 0 ");
            cmdQuery.Append(" group by U.DepartId");
            DbCommand dc = this._db.GetSqlStringCommand(cmdQuery.ToString());
            using (IDataReader rdr = DbHelper.ExecuteReader(dc, this._db))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.StatisticStructure.AreaDepartStatInfo item = new EyouSoft.Model.StatisticStructure.AreaDepartStatInfo();
                    item.DepartId = rdr.GetInt32(rdr.GetOrdinal("DepartId"));
                    item.TradeNumber = rdr.GetInt32(rdr.GetOrdinal("TourNumber"));
                    items.Add(item);
                }
            }
            return items;
        }

        /// <summary>
        /// 获取区域销售统计合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="startTime">开始时间(出团时间)</param>
        /// <param name="entTime">结束时间(出团时间)</param>
        /// <param name="deptId">部门编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="tourType">团队类型</param>
        /// <param name="heJi">合计数</param>
        public void GetQuYuXiaoShouTongJiHeJi(int companyId, DateTime? startTime, DateTime? entTime, int deptId
            , Model.StatisticStructure.AreaSoldStatSearch searchInfo, Model.EnumType.TourStructure.TourType? tourType
            , out int heJi)
        {
            heJi = 0;
            StringBuilder cmdText = new StringBuilder();
            cmdText.Append(" SELECT SUM(PeopleNumber-LeaguePepoleNum) FROM tbl_TourOrder AS A ");
            cmdText.AppendFormat(" INNER JOIN tbl_Customer AS B ON A.BuyCompanyId=B.Id AND B.CompanyId={0} ", companyId);
            if (searchInfo != null)
            {
                if (!string.IsNullOrEmpty(searchInfo.CityIds))
                {
                    cmdText.AppendFormat(" AND B.CityId IN({0})", searchInfo.CityIds);
                }
                if (!string.IsNullOrEmpty(searchInfo.SalerIds))
                {
                    cmdText.AppendFormat(" AND B.SaleId IN({0}) ", searchInfo.SalerIds);
                }
            }
            cmdText.AppendFormat(" AND B.CityId>0 ");
            cmdText.AppendFormat(" INNER JOIN tbl_CompanyUser AS C ON C.Id=A.PerTimeSellerId AND C.DepartId={0} ", deptId);
            cmdText.Append(" WHERE A.OrderState NOT IN(3,4) AND A.IsDelete='0' ");
            cmdText.AppendFormat(" AND A.SellCompanyId={0} ", companyId);
            //出团时间
            if (startTime.HasValue || entTime.HasValue)
            {
                if (startTime.HasValue)
                    cmdText.AppendFormat(" AND A.LeaveDate > '{0}' ", startTime.Value.ToShortDateString());
                if (entTime.HasValue)
                    cmdText.AppendFormat(" AND A.LeaveDate < '{0}' ", entTime.Value.ToShortDateString());
            }
            if (tourType.HasValue)
            {
                cmdText.AppendFormat(" and A.TourClassId = {0} ", (int)tourType.Value);
            }

            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) heJi = rdr.GetInt32(0);
                }
            }

        }
        #endregion
    }
}
