using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.PlanStruture
{
    /// <summary>
    /// 交通管理数据访问类
    /// </summary>
    /// Author:李晓欢 2012-09-10
    public class PlanTrffic : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.PlanStruture.IPlanTrffic
    {
        #region constructor
        /// <summary>
        /// database
        /// </summary>
        private readonly Database _db = null;
        /// <summary>
        /// default constructor
        /// </summary>
        public PlanTrffic()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region 私有变量
        private const string SQL_TRAFFIC_DELETE = "UPDATE  tbl_Traffic SET IsDelete='1' WHERE TrafficId IN({0})";

        private const string SQL_TRAFFIC_UPDATESTATUS = " UPDATE tbl_Traffic SET [Status]=@Status WHERE TrafficId=@TrafficId ";

        private const string SQL_TRAFFIC_GETTRFFICMODEL = "SELECT TrafficId,TrafficName,TrafficDays,ChildPrices,[Status],IsDelete FROM tbl_Traffic WHERE TrafficId=@TrafficId";

        private const string SQL_TRAVEL_GETTRAVELINFO = "SELECT t.TravelId,t.TrafficId,t.SerialNum,t.TrafficType,t.LProvince,t.LCity,t.RProvince,t.RCity,t.FilghtNum,t.FlightCompany,t.LTime,t.RTime,t.IsStop,t.[Space],t.AirPlaneType,t.IntervalDays,t.CompanyId,t.Operater,t.OperaterID,t.InsueTime FROM tbl_Travel t WHERE t.TravelId=@TravelId";

        private const string SQL_TRAVEL_DELETE = " DELETE FROM tbl_Travel WHERE TravelId IN({0});UPDATE tbl_Traffic SET TrafficDays = ISNULL( (SELECT SUM(isnull(IntervalDays,0)) FROM tbl_Travel WHERE tbl_Travel.TrafficId=tbl_Traffic.TrafficId),0) WHERE TrafficId=@TrafficId ";

        private const string SQL_TRAFFIC_ADDPRICES = "IF EXISTS(SELECT 1 FROM tbl_TrafficPrices WHERE SDateTime=@SDateTime AND TrafficId=@TrafficId) DELETE FROM tbl_TrafficPrices WHERE SDateTime=@SDateTime AND TrafficId=@TrafficId ; INSERT INTO tbl_TrafficPrices(PricesID,TrafficId,SDateTime,TicketPrices,TicketNums,[Status],InsueTime) VALUES (@PricesID,@TrafficId,@SDateTime,@TicketPrices,@TicketNums,@Status,@InsueTime)";

        private const string SQL_TRAFFIC_UPDATEPRICES = "UPDATE tbl_TrafficPrices SET SDateTime=@SDateTime, TicketPrices=@TicketPrices,TicketNums=@TicketNums,Status=@Status, InsueTime=@InsueTime WHERE SDateTime=@SDateTime AND TrafficId=@TrafficId";

        private const string SQL_TRAFFIC_UPDATEPRICESSTATUS = "UPDATE tbl_TrafficPrices SET Status=@Status WHERE PricesID=@PricesID AND TrafficId=@TrafficId ";

        private const string SQL_TRAFFIC_GETPRICESINFO = "SELECT * FROM tbl_TrafficPrices WHERE PricesID=@PricesID AND TrafficId=@TrafficId";

        #endregion

        #region IPlanTrffic 交通成员

        /// <summary>
        /// 添加交通
        /// </summary>
        /// <param name="trfficModel">交通实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool AddPlanTraffic(EyouSoft.Model.PlanStructure.TrafficInfo trafficModel)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Traffic_Add");
            this._db.AddInParameter(cmd, "TrafficName", DbType.String, trafficModel.TrafficName);
            this._db.AddInParameter(cmd, "ChildPrices", DbType.Decimal, trafficModel.ChildPrices);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, trafficModel.CompanyId);
            this._db.AddInParameter(cmd, "Operater", DbType.String, trafficModel.Operater);
            this._db.AddInParameter(cmd, "OperaterId", DbType.Int32, trafficModel.OperaterId);
            this._db.AddInParameter(cmd, "InsueTime", DbType.DateTime, trafficModel.InsueTime);
            this._db.AddOutParameter(cmd, "result", DbType.Int32, 4);
            DbHelper.RunProcedure(cmd, this._db);
            return Convert.ToInt32(this._db.GetParameterValue(cmd, "result")) > 0 ? true : false;
        }

        /// <summary>
        /// 修改交通
        /// </summary>
        /// <param name="trfficModel">交通实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool UpdatePlanTraffic(EyouSoft.Model.PlanStructure.TrafficInfo trafficModel)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Traffic_Update");
            this._db.AddInParameter(cmd, "TrafficID", DbType.StringFixedLength, trafficModel.TrafficId);
            this._db.AddInParameter(cmd, "TrafficName", DbType.String, trafficModel.TrafficName);
            this._db.AddInParameter(cmd, "ChildPrices", DbType.Decimal, trafficModel.ChildPrices);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, trafficModel.CompanyId);
            this._db.AddInParameter(cmd, "Operater", DbType.String, trafficModel.Operater);
            this._db.AddInParameter(cmd, "OperaterId", DbType.Int32, trafficModel.OperaterId);
            this._db.AddInParameter(cmd, "InsueTime", DbType.DateTime, trafficModel.InsueTime);
            this._db.AddOutParameter(cmd, "result", DbType.Int32, 4);
            DbHelper.RunProcedure(cmd, this._db);
            return Convert.ToInt32(this._db.GetParameterValue(cmd, "result")) > 0 ? true : false;
        }

        /// <summary>
        /// 删除交通
        /// </summary>
        /// <param name="trfficIds">交通编号集合</param>
        /// <returns>true:成功 false:失败</returns>
        public bool DeletePlanTraffic(string trafficIds)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(string.Format(SQL_TRAFFIC_DELETE, trafficIds));
            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }


        /// <summary>
        /// 获取交通集合列表
        /// </summary>
        /// <param name="trffIcName">交通名称</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="CompanyID">公司编号</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns>所有交通集合</returns>
        public IList<EyouSoft.Model.PlanStructure.TrafficInfo> GetTrafficList(EyouSoft.Model.PlanStructure.TrafficSearch SearchModel, int PageSize, int PageIndex, int CompanyID, ref int RecordCount)
        {
            IList<EyouSoft.Model.PlanStructure.TrafficInfo> trafficList = new List<EyouSoft.Model.PlanStructure.TrafficInfo>();
            #region 过程分页参数设置
            string tabName = "tbl_Traffic";
            string fields = " TrafficId,TrafficName,[TrafficDays],ChildPrices,[Status],CompanyId,Operater,OperaterId,InsueTime,IsDelete";
            string primaryKey = "TrafficId";
            string orderStrBy = "InsueTime desc";
            StringBuilder strwhere = new StringBuilder();
            strwhere.AppendFormat(" IsDelete='0' AND CompanyId={0}  ", CompanyID);
            if (SearchModel != null && !string.IsNullOrEmpty(SearchModel.TrafficName))
            {
                strwhere.AppendFormat(" and TrafficName like '%{0}%'  ", SearchModel.TrafficName);
            }
            using (IDataReader dr = DbHelper.ExecuteReader(this._db, PageSize, PageIndex, ref RecordCount, tabName, primaryKey, fields, strwhere.ToString(), orderStrBy))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.PlanStructure.TrafficInfo trfficModel = new EyouSoft.Model.PlanStructure.TrafficInfo();
                    trfficModel.ChildPrices = !dr.IsDBNull(dr.GetOrdinal("ChildPrices")) ? dr.GetDecimal(dr.GetOrdinal("ChildPrices")) : 0;
                    trfficModel.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    trfficModel.InsueTime = dr.GetDateTime(dr.GetOrdinal("InsueTime"));
                    trfficModel.IsDelete = dr.GetString(dr.GetOrdinal("IsDelete")) == "0" ? true : false;
                    trfficModel.Operater = dr.GetString(dr.GetOrdinal("Operater"));
                    trfficModel.OperaterId = dr.GetInt32(dr.GetOrdinal("OperaterId"));
                    trfficModel.Status = (EyouSoft.Model.EnumType.PlanStructure.TrafficStatus)int.Parse(dr[dr.GetOrdinal("Status")].ToString());
                    trfficModel.TrafficDays = !dr.IsDBNull(dr.GetOrdinal("TrafficDays")) ? dr.GetInt32(dr.GetOrdinal("TrafficDays")) : 0;
                    trfficModel.TrafficId = dr.IsDBNull(dr.GetOrdinal("TrafficId")) ? 0 : dr.GetInt32(dr.GetOrdinal("TrafficId"));
                    trfficModel.TrafficName = dr["TrafficName"].ToString();
                    trafficList.Add(trfficModel);
                }
            }
            #endregion
            return trafficList;
        }

        /// <summary>
        /// 获取交通集合列表
        /// </summary>
        /// <param name="SearchModel">查询实体</param>
        /// <param name="CompanyID">公司编号</param>
        /// <returns>所有交通集合</returns>
        public IList<EyouSoft.Model.PlanStructure.TrafficInfo> GetTrafficList(EyouSoft.Model.PlanStructure.TrafficSearch SearchModel, int CompanyID)
        {
            IList<EyouSoft.Model.PlanStructure.TrafficInfo> trafficList = new List<EyouSoft.Model.PlanStructure.TrafficInfo>();
            #region 过程分页参数设置

            var strSql = new StringBuilder();
            strSql.Append(" SELECT TrafficId,TrafficName,ChildPrices,[Status],CompanyId,Operater,OperaterId,InsueTime,IsDelete ");

            strSql.Append(
                " ,(SELECT tbl_Travel.TravelId,tbl_Travel.TrafficId,tbl_Travel.SerialNum, tbl_Travel.FilghtNum ");
            strSql.Append(
                " , tbl_Travel.FlightCompany, tbl_Travel.AirPlaneType, tbl_Travel.IntervalDays, tbl_Travel.TrafficType ");
            strSql.Append(" , tbl_Travel.[Space],(SELECT ProvinceName FROM tbl_CompanyProvince ");
            strSql.Append(" WHERE tbl_CompanyProvince.Id=tbl_Travel.LProvince) AS LProvinceName ");
            strSql.Append(" ,(SELECT CityName FROM tbl_CompanyCity  WHERE tbl_CompanyCity.Id=tbl_Travel.LCity  ");
            strSql.Append(" AND tbl_CompanyCity.ProvinceId=tbl_Travel.LProvince) AS LCityName ");
            strSql.Append(
                " ,(SELECT ProvinceName FROM tbl_CompanyProvince  WHERE tbl_CompanyProvince.Id=tbl_Travel.RProvince) AS RProvinceName ");
            strSql.Append(" ,(SELECT CityName FROM tbl_CompanyCity  WHERE tbl_CompanyCity.Id=tbl_Travel.RCity  ");
            strSql.Append(" AND tbl_CompanyCity.ProvinceId=tbl_Travel.RProvince) AS RCityName  ");
            strSql.Append(
                " FROM  tbl_Travel WHERE tbl_Travel.TrafficId=tbl_Traffic.TrafficId FOR XML RAW,ROOT ) AS travelList ");

            strSql.Append(" FROM tbl_Traffic WHERE IsDelete='0' and CompanyId = @CompanyId and Status='0' ");
            if (SearchModel != null)
            {
                if (!string.IsNullOrEmpty(SearchModel.TrafficName))
                    strSql.AppendFormat(" and TrafficName like '%{0}%' ", SearchModel.TrafficName);
                if (!string.IsNullOrEmpty(SearchModel.TourId))
                {
                    strSql.AppendFormat(
                        " and EXISTS (SELECT 1 FROM tbl_TourTraffic tt WHERE tt.TrafficId = tbl_Traffic.TrafficId AND tt.TourId = '{0}') ",
                        SearchModel.TourId);
                }
            }

            DbCommand cmd = this._db.GetSqlStringCommand(strSql.ToString());
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, CompanyID);
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.PlanStructure.TrafficInfo trfficModel = new EyouSoft.Model.PlanStructure.TrafficInfo();
                    trfficModel.ChildPrices = !dr.IsDBNull(dr.GetOrdinal("ChildPrices")) ? dr.GetDecimal(dr.GetOrdinal("ChildPrices")) : 0;
                    trfficModel.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    trfficModel.InsueTime = dr.GetDateTime(dr.GetOrdinal("InsueTime"));
                    trfficModel.IsDelete = dr.GetString(dr.GetOrdinal("IsDelete")) == "0" ? true : false;
                    trfficModel.Operater = dr.GetString(dr.GetOrdinal("Operater"));
                    trfficModel.OperaterId = dr.GetInt32(dr.GetOrdinal("OperaterId"));
                    trfficModel.Status = (EyouSoft.Model.EnumType.PlanStructure.TrafficStatus)int.Parse(dr[dr.GetOrdinal("Status")].ToString());
                    trfficModel.TrafficId = dr.IsDBNull(dr.GetOrdinal("TrafficId")) ? 0 : dr.GetInt32(dr.GetOrdinal("TrafficId"));
                    trfficModel.TrafficName = dr["TrafficName"].ToString();
                    trfficModel.travelList = dr.IsDBNull(dr.GetOrdinal("travelList")) ? null : GetTravelListForXML(dr.GetString(dr.GetOrdinal("travelList")));
                    trafficList.Add(trfficModel);
                }
            }
            #endregion
            return trafficList;
        }

        /// <summary>
        /// 设置交通状态
        /// </summary>
        /// <param name="trffiCID">交通编号</param>
        /// <param name="status">当前状态</param>
        /// <returns>状态设置</returns>
        public bool UpdateChangeSatus(int trafficID, EyouSoft.Model.EnumType.PlanStructure.TrafficStatus status)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_TRAFFIC_UPDATESTATUS);
            this._db.AddInParameter(cmd, "Status", DbType.Byte, status);
            this._db.AddInParameter(cmd, "TrafficId", DbType.Int32, trafficID);
            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取交通实体
        /// </summary>
        /// <param name="trfficID">交通编号</param>
        /// <returns>返回交通实体</returns>
        public EyouSoft.Model.PlanStructure.TrafficInfo GetTrafficModel(int trafficID)
        {
            EyouSoft.Model.PlanStructure.TrafficInfo trfficModel = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_TRAFFIC_GETTRFFICMODEL);
            this._db.AddInParameter(cmd, "TrafficId", DbType.Int32, trafficID);
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {
                    trfficModel = new EyouSoft.Model.PlanStructure.TrafficInfo()
                    {
                        ChildPrices = dr.IsDBNull(dr.GetOrdinal("ChildPrices")) ? 0 : dr.GetDecimal(dr.GetOrdinal("ChildPrices")),
                        TrafficId = dr.IsDBNull(dr.GetOrdinal("TrafficId")) ? 0 : dr.GetInt32(dr.GetOrdinal("TrafficId")),
                        TrafficName = dr["TrafficName"].ToString(),
                        TrafficDays = dr.IsDBNull(dr.GetOrdinal("TrafficDays")) ? 0 : dr.GetInt32(dr.GetOrdinal("TrafficDays")),
                        Status = (EyouSoft.Model.EnumType.PlanStructure.TrafficStatus)int.Parse(dr[dr.GetOrdinal("Status")].ToString()),
                        IsDelete = dr.IsDBNull(dr.GetOrdinal("IsDelete")) ? false : dr.GetString(dr.GetOrdinal("IsDelete")) == "0" ? true : false
                    };
                }
            }
            return trfficModel;
        }

        /// <summary>
        /// 获取交通出票统计信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="search">查询实体</param>
        /// <returns></returns>
        public IList<Model.PlanStructure.JiaoTongChuPiao> GetJiaoTongChuPiao(int companyId
            , Model.PlanStructure.JiaoTongChuPiaoSearch search)
        {
            IList<Model.PlanStructure.JiaoTongChuPiao> list = null;
            if (companyId <= 0) return list;

            var strSql = new StringBuilder();
            //出团时间查询
            string strLeaveDate = string.Empty;
            //出票时间查询
            string strShenQingDate = string.Empty;
            strSql.Append(" SELECT t.TrafficId,t.TrafficName  ");

            strSql.Append(" ,( ");
            strSql.Append(" SELECT isnull(SUM(to1.PeopleNumber - to1.LeaguePepoleNum),0) FROM tbl_TourOrder to1  ");
            strSql.Append(" WHERE to1.IsDelete = '0' @ ");
            strSql.Append(
                " AND EXISTS (SELECT 1 FROM tbl_TourOrderTraffic tot WHERE tot.OrderId = to1.ID AND tot.TrafficId = t.TrafficId) ");
            strSql.AppendFormat(
                " AND EXISTS (SELECT 1 FROM tbl_PlanTicketOut pto WHERE pto.OrderId = to1.ID AND pto.[State] = {0} $) ",
                (int)Model.EnumType.PlanStructure.TicketState.已出票);
            strSql.Append(" ) AS ChuPiaoShu ");

            strSql.Append(" ,(");
            strSql.Append(" SELECT ISNULL(SUM(ptk.AgencyPrice),0) FROM  tbl_PlanTicketKind ptk  WHERE  ");

            strSql.AppendFormat("  EXISTS (SELECT 1 FROM tbl_PlanTicketOut pto WHERE pto.ID = ptk.TicketId AND pto.[State]={0} AND pto.OrderId IN ", (int)Model.EnumType.PlanStructure.TicketState.已出票);
            strSql.Append(" ( ");
            strSql.Append("  SELECT to1.ID FROM tbl_TourOrder to1 WHERE to1.IsDelete = '0'   ");
            strSql.Append("  AND EXISTS (SELECT 1 FROM tbl_TourOrderTraffic tot WHERE tot.OrderId = to1.ID AND tot.TrafficId = t.TrafficId $ ))) ");       
            strSql.Append(" ) AS DaiLiFei");

            strSql.AppendFormat(
                " FROM tbl_Traffic t WHERE t.IsDelete = '0' and t.[Status] = 0 and CompanyId = {0} ", companyId);
            if (search != null)
            {
                if (search.StartTime.HasValue)
                {
                    strLeaveDate += string.Format(
                        " and datediff(dd,'{0}',to1.LeaveDate) >= 0 ", search.StartTime.Value.ToShortDateString());
                }
                if (search.EndTime.HasValue)
                {
                    strLeaveDate += string.Format(
                        " and datediff(dd,'{0}',to1.LeaveDate) <= 0 ", search.EndTime.Value.ToShortDateString());
                }
            }
            strSql.Append(" ORDER BY t.InsueTime ");

            DbCommand dc =
                _db.GetSqlStringCommand(strSql.ToString().Replace("@", strLeaveDate).Replace("$", strLeaveDate));
            list = new List<Model.PlanStructure.JiaoTongChuPiao>();
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                while (dr.Read())
                {
                    var model = new Model.PlanStructure.JiaoTongChuPiao();
                    if (!dr.IsDBNull(dr.GetOrdinal("TrafficId")))
                        model.TrafficId = dr.GetInt32(dr.GetOrdinal("TrafficId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TrafficName")))
                        model.TrafficName = dr.GetString(dr.GetOrdinal("TrafficName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ChuPiaoShu")))
                        model.ChuPiaoShu = dr.GetInt32(dr.GetOrdinal("ChuPiaoShu"));
                    if (!dr.IsDBNull(dr.GetOrdinal("DaiLiFei")))
                        model.AgencyPrice = dr.GetDecimal(dr.GetOrdinal("DaiLiFei"));
                    list.Add(model);
                }
            }

            return list;
        }

        #endregion

        #region 交通行程成员
        /// <summary>
        /// 新增交通行程
        /// </summary>
        /// <param name="travelModel">行程实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool AddPlanTravel(EyouSoft.Model.PlanStructure.TravelInfo travelModel)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Travel_Add");
            this._db.AddInParameter(cmd, "TrafficId", DbType.Int32, travelModel.TrafficId);
            this._db.AddInParameter(cmd, "SerialNum", DbType.Int32, travelModel.SerialNum);
            this._db.AddInParameter(cmd, "TrafficType", DbType.Byte, Convert.ToInt32(travelModel.TrafficType));
            this._db.AddInParameter(cmd, "LProvince", DbType.Int32, travelModel.LProvince);
            this._db.AddInParameter(cmd, "LCity", DbType.Int32, travelModel.LCity);
            this._db.AddInParameter(cmd, "RProvince", DbType.Int32, travelModel.RProvince);
            this._db.AddInParameter(cmd, "RCity", DbType.Int32, travelModel.RCity);
            this._db.AddInParameter(cmd, "FilghtNum", DbType.String, travelModel.FilghtNum);
            this._db.AddInParameter(cmd, "FlightCompany", DbType.Byte, Convert.ToInt32(travelModel.FlightCompany));
            this._db.AddInParameter(cmd, "LTime", DbType.String, travelModel.LTime);
            this._db.AddInParameter(cmd, "RTime", DbType.String, travelModel.RTime);
            this._db.AddInParameter(cmd, "IsStop", DbType.Boolean, travelModel.IsStop == true ? 0 : 1);
            this._db.AddInParameter(cmd, "Space", DbType.Byte, Convert.ToInt32(travelModel.Space));
            this._db.AddInParameter(cmd, "AirPlaneType", DbType.String, travelModel.AirPlaneType);
            this._db.AddInParameter(cmd, "IntervalDays", DbType.Int32, travelModel.IntervalDays);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, travelModel.CompanyId);
            this._db.AddInParameter(cmd, "Operater", DbType.String, travelModel.Operater);
            this._db.AddInParameter(cmd, "OperaterID", DbType.Int32, travelModel.OperaterID);
            this._db.AddInParameter(cmd, "InsueTime", DbType.DateTime, travelModel.InsueTime);
            this._db.AddOutParameter(cmd, "result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, this._db);
            return Convert.ToInt32(this._db.GetParameterValue(cmd, "result")) > 0 ? true : false;
        }

        /// <summary>
        /// 获取交通行程
        /// </summary>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">总页数</param>
        /// <param name="CompanyID">公司编号</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="trfficID">交通编号</param>
        /// <returns>交通行程</returns>
        public IList<EyouSoft.Model.PlanStructure.TravelInfo> GetTravelList(int PageSize, int PageIndex, int CompanyID, ref int RecordCount, int trafficID)
        {
            IList<EyouSoft.Model.PlanStructure.TravelInfo> list = new List<EyouSoft.Model.PlanStructure.TravelInfo>();
            string tabName = "tbl_Travel";
            string fields = "tbl_Travel.TravelId,tbl_Travel.TrafficId,tbl_Travel.SerialNum,tbl_Travel.TrafficType,tbl_Travel.LProvince,tbl_Travel.LCity,tbl_Travel.RProvince,tbl_Travel.RCity,tbl_Travel.FilghtNum,tbl_Travel.FlightCompany,tbl_Travel.LTime,tbl_Travel.RTime,tbl_Travel.IsStop,tbl_Travel.[Space],tbl_Travel.AirPlaneType,tbl_Travel.IntervalDays,tbl_Travel.CompanyId,tbl_Travel.InsueTime,tbl_Travel.Operater,tbl_Travel.OperaterID,(SELECT CityName FROM tbl_CompanyCity  WHERE tbl_CompanyCity.Id=tbl_Travel.LCity AND tbl_CompanyCity.ProvinceId=tbl_Travel.LProvince) AS LCityName,(SELECT CityName FROM tbl_CompanyCity  WHERE tbl_CompanyCity.Id=tbl_Travel.RCity AND tbl_CompanyCity.ProvinceId=tbl_Travel.RProvince) AS RCityName";
            string primaryKey = " TravelId ";
            string orderByStr = " InsueTime DESC ";
            StringBuilder sqlWhere = new StringBuilder();
            sqlWhere.AppendFormat(" tbl_Travel.TrafficId={0} ", trafficID);
            using (IDataReader dr = DbHelper.ExecuteReader(this._db, PageSize, PageIndex, ref RecordCount, tabName, primaryKey, fields, sqlWhere.ToString(), orderByStr))
            {
                EyouSoft.Model.PlanStructure.TravelInfo model = null;
                while (dr.Read())
                {
                    model = new EyouSoft.Model.PlanStructure.TravelInfo()
                    {
                        AirPlaneType = !dr.IsDBNull(dr.GetOrdinal("AirPlaneType")) ? dr.GetString(dr.GetOrdinal("AirPlaneType")) : string.Empty,
                        CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId")),
                        FilghtNum = !dr.IsDBNull(dr.GetOrdinal("FilghtNum")) ? dr.GetString(dr.GetOrdinal("FilghtNum")) : string.Empty,
                        FlightCompany = (EyouSoft.Model.EnumType.PlanStructure.FlightCompany)Convert.ToInt32(dr[dr.GetOrdinal("FlightCompany")].ToString()),
                        InsueTime = dr.GetDateTime(dr.GetOrdinal("InsueTime")),
                        IntervalDays = !dr.IsDBNull(dr.GetOrdinal("IntervalDays")) ? dr.GetInt32(dr.GetOrdinal("IntervalDays")) : 0,
                        IsStop = dr.IsDBNull(dr.GetOrdinal("IsStop")) ? false : dr.GetString(dr.GetOrdinal("IsStop")) == "0" ? true : false,
                        TravelId = dr.GetInt32(dr.GetOrdinal("TravelId")),
                        TrafficType = (EyouSoft.Model.EnumType.PlanStructure.TrafficType)Convert.ToInt32(dr[dr.GetOrdinal("TrafficType")].ToString()),
                        TrafficId = Convert.ToInt32(dr["TrafficId"].ToString()),
                        LCity = dr.GetInt32(dr.GetOrdinal("LCity")),
                        LCityName = !dr.IsDBNull(dr.GetOrdinal("LCityName")) ? dr.GetString(dr.GetOrdinal("LCityName")) : string.Empty,
                        LProvince = dr.GetInt32(dr.GetOrdinal("LProvince")),
                        LTime = !dr.IsDBNull(dr.GetOrdinal("LTime")) ? dr.GetString(dr.GetOrdinal("LTime")) : string.Empty,
                        Operater = dr.GetString(dr.GetOrdinal("Operater")),
                        OperaterID = dr.GetInt32(dr.GetOrdinal("OperaterID")),
                        RCity = dr.GetInt32(dr.GetOrdinal("RCity")),
                        RCityName = !dr.IsDBNull(dr.GetOrdinal("RCityName")) ? dr.GetString(dr.GetOrdinal("RCityName")) : string.Empty,
                        RProvince = dr.GetInt32(dr.GetOrdinal("RProvince")),
                        RTime = !dr.IsDBNull(dr.GetOrdinal("RTime")) ? dr.GetString(dr.GetOrdinal("RTime")) : string.Empty,
                        SerialNum = dr.GetInt32(dr.GetOrdinal("SerialNum")),
                        Space = (EyouSoft.Model.EnumType.PlanStructure.Space)Convert.ToInt32(dr[dr.GetOrdinal("Space")].ToString())
                    };
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 修改交通行程
        /// </summary>
        /// <param name="travelModel">行程实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool UpdatePlanTravel(EyouSoft.Model.PlanStructure.TravelInfo travelModel)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Travel_Update");
            this._db.AddInParameter(cmd, "TravelId", DbType.Int32, travelModel.TravelId);
            this._db.AddInParameter(cmd, "TrafficId", DbType.Int32, travelModel.TrafficId);
            this._db.AddInParameter(cmd, "SerialNum", DbType.Int32, travelModel.SerialNum);
            this._db.AddInParameter(cmd, "TrafficType", DbType.Byte, Convert.ToInt32(travelModel.TrafficType));
            this._db.AddInParameter(cmd, "LProvince", DbType.Int32, travelModel.LProvince);
            this._db.AddInParameter(cmd, "LCity", DbType.Int32, travelModel.LCity);
            this._db.AddInParameter(cmd, "RProvince", DbType.Int32, travelModel.RProvince);
            this._db.AddInParameter(cmd, "RCity", DbType.Int32, travelModel.RCity);
            this._db.AddInParameter(cmd, "FilghtNum", DbType.String, travelModel.FilghtNum);
            this._db.AddInParameter(cmd, "FlightCompany", DbType.Byte, Convert.ToInt32(travelModel.FlightCompany));
            this._db.AddInParameter(cmd, "LTime", DbType.String, travelModel.LTime);
            this._db.AddInParameter(cmd, "RTime", DbType.String, travelModel.RTime);
            this._db.AddInParameter(cmd, "IsStop", DbType.Boolean, travelModel.IsStop == true ? 0 : 1);
            this._db.AddInParameter(cmd, "Space", DbType.Byte, Convert.ToInt32(travelModel.Space));
            this._db.AddInParameter(cmd, "AirPlaneType", DbType.String, travelModel.AirPlaneType);
            this._db.AddInParameter(cmd, "IntervalDays", DbType.Int32, travelModel.IntervalDays);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, travelModel.CompanyId);
            this._db.AddInParameter(cmd, "Operater", DbType.String, travelModel.Operater);
            this._db.AddInParameter(cmd, "OperaterID", DbType.Int32, travelModel.OperaterID);
            this._db.AddInParameter(cmd, "InsueTime", DbType.DateTime, travelModel.InsueTime);
            this._db.AddOutParameter(cmd, "result", DbType.Int32, 4);
            DbHelper.RunProcedureWithResult(cmd, this._db);
            return Convert.ToInt32(this._db.GetParameterValue(cmd, "result")) > 0 ? true : false;
        }


        /// <summary>
        /// 获取行程实体
        /// </summary>
        /// <param name="travelID">行程编号</param>
        /// <returns></returns>
        public EyouSoft.Model.PlanStructure.TravelInfo GettravelModel(int travelID)
        {
            EyouSoft.Model.PlanStructure.TravelInfo travelModel = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_TRAVEL_GETTRAVELINFO);
            this._db.AddInParameter(cmd, "TravelId", DbType.Int32, travelID);
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {
                    travelModel = new EyouSoft.Model.PlanStructure.TravelInfo();
                    travelModel.TravelId = dr.GetInt32(dr.GetOrdinal("TravelId"));
                    travelModel.TrafficId = dr.GetInt32(dr.GetOrdinal("TrafficId"));
                    travelModel.SerialNum = !dr.IsDBNull(dr.GetInt32(dr.GetOrdinal("SerialNum"))) ? dr.GetInt32(dr.GetOrdinal("SerialNum")) : 0;
                    travelModel.TrafficType = (EyouSoft.Model.EnumType.PlanStructure.TrafficType)Convert.ToInt32(dr[dr.GetOrdinal("TrafficType")].ToString());
                    travelModel.LProvince = !dr.IsDBNull(dr.GetOrdinal("LProvince")) ? dr.GetInt32(dr.GetOrdinal("LProvince")) : 0;
                    travelModel.LCity = !dr.IsDBNull(dr.GetOrdinal("LCity")) ? dr.GetInt32(dr.GetOrdinal("LCity")) : 0;
                    travelModel.RProvince = dr.IsDBNull(dr.GetOrdinal("RProvince")) ? 0 : dr.GetInt32(dr.GetOrdinal("RProvince"));
                    travelModel.RCity = !dr.IsDBNull(dr.GetOrdinal("RCity")) ? dr.GetInt32(dr.GetOrdinal("RCity")) : 0;
                    travelModel.FilghtNum = !dr.IsDBNull(dr.GetOrdinal("FilghtNum")) ? dr.GetString(dr.GetOrdinal("FilghtNum")) : string.Empty;
                    travelModel.FlightCompany = (EyouSoft.Model.EnumType.PlanStructure.FlightCompany)Convert.ToInt32(dr[dr.GetOrdinal("FlightCompany")].ToString());
                    travelModel.LTime = !dr.IsDBNull(dr.GetOrdinal("LTime")) ? dr.GetString(dr.GetOrdinal("LTime")) : string.Empty;
                    travelModel.RTime = !dr.IsDBNull(dr.GetOrdinal("RTime")) ? dr.GetString(dr.GetOrdinal("RTime")) : string.Empty;
                    travelModel.IsStop = dr.GetString(dr.GetOrdinal("IsStop")) == "1" ? false : true;
                    travelModel.AirPlaneType = !dr.IsDBNull(dr.GetOrdinal("AirPlaneType")) ? dr.GetString(dr.GetOrdinal("AirPlaneType")) : string.Empty;
                    travelModel.IntervalDays = !dr.IsDBNull(dr.GetInt32(dr.GetOrdinal("IntervalDays"))) ? dr.GetInt32(dr.GetOrdinal("IntervalDays")) : 0;
                    travelModel.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    travelModel.Operater = dr.GetString(dr.GetOrdinal("Operater"));
                    travelModel.OperaterID = dr.GetInt32(dr.GetOrdinal("OperaterID"));
                    travelModel.InsueTime = dr.GetDateTime(dr.GetOrdinal("InsueTime"));
                    travelModel.Space = (EyouSoft.Model.EnumType.PlanStructure.Space)Convert.ToInt32(dr[dr.GetOrdinal("Space")].ToString());
                }
            }
            return travelModel;
        }

        /// <summary>
        /// 删除交通行程
        /// </summary>
        /// <param name="travelIds">交通行程编号集合</param>
        /// <param name="trfficIds">交通编号</param>
        /// <returns>true:成功 false:失败</returns>
        public bool DeletePlanTravel(string travelIds, int trafficId)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(string.Format(SQL_TRAVEL_DELETE, travelIds));
            this._db.AddInParameter(cmd, "TrafficId", DbType.Int32, trafficId);
            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }


        #endregion

        #region 价格成员
        /// <summary>
        /// 获取价格实体
        /// </summary>
        /// <param name="trffIcID">交通编号</param>
        /// <param name="LTourDate">出团时间</param>
        /// <returns>返回价格实体</returns>
        public EyouSoft.Model.PlanStructure.TrafficPricesInfo GetTrafficPriceModel(int traffIcID, DateTime LTourDate)
        {
            EyouSoft.Model.PlanStructure.TrafficPricesInfo model = null;
            var strSql = new StringBuilder();
            strSql.Append(" SELECT PricesID,InsueTime,SDateTime, TrafficId,TicketPrices,TicketNums,[Status] ");
            strSql.AppendFormat(
                ",(SELECT SUM(to1.PeopleNumber - to1.LeaguePepoleNum) FROM tbl_TourOrder to1 WHERE to1.IsDelete = '0' AND EXISTS(SELECT 1 FROM tbl_TourOrderTraffic tot WHERE tot.OrderId = to1.ID AND tot.TrafficId = tbl_TrafficPrices.TrafficId) AND DATEDIFF(dd,to1.LeaveDate,tbl_TrafficPrices.SDateTime) = 0 and to1.OrderState IN({0},{1},{2})) as YiShiYong ",
                (int)Model.EnumType.TourStructure.OrderState.未处理,
                (int)Model.EnumType.TourStructure.OrderState.已成交,
                (int)Model.EnumType.TourStructure.OrderState.已留位);
            strSql.Append(" FROM tbl_TrafficPrices ");
            strSql.Append(" WHERE TrafficId=@TrafficId AND DateDiff(dd,SDateTime,@LeaveDate)=0 ");
            strSql.AppendFormat(" and [Status] = {0} ", (int)Model.EnumType.PlanStructure.TicketStatus.正常);
            DbCommand cmd = this._db.GetSqlStringCommand(strSql.ToString());
            this._db.AddInParameter(cmd, "TrafficId", DbType.Int32, traffIcID);
            this._db.AddInParameter(cmd, "LeaveDate", DbType.DateTime, LTourDate);
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (dr.Read())
                {
                    model = new EyouSoft.Model.PlanStructure.TrafficPricesInfo()
                    {
                        PricesID = dr["PricesID"].ToString(),
                        Status = (EyouSoft.Model.EnumType.PlanStructure.TicketStatus)int.Parse(dr[dr.GetOrdinal("Status")].ToString()),
                        InsueTime = dr.GetDateTime(dr.GetOrdinal("InsueTime")),
                        SDateTime = dr.GetDateTime(dr.GetOrdinal("SDateTime")),
                        TicketNums = !dr.IsDBNull(dr.GetOrdinal("TicketNums")) ? dr.GetInt32(dr.GetOrdinal("TicketNums")) : 0,
                        TicketPrices = !dr.IsDBNull(dr.GetOrdinal("TicketPrices")) ? dr.GetDecimal(dr.GetOrdinal("TicketPrices")) : 0,
                        TrafficId = dr.GetInt32(dr.GetOrdinal("TrafficId")),
                        YiShiYong = dr.IsDBNull(dr.GetOrdinal("YiShiYong")) ? 0 : dr.GetInt32(dr.GetOrdinal("YiShiYong"))
                    };
                }
            }
            return model;
        }

        /// <summary>
        /// 根据交通编号出团时间获取价格集合
        /// </summary>
        /// <param name="trafficId">交通编号集合</param>
        /// <param name="LTourDate">出团时间</param>
        /// <returns>返回价格集合</returns>
        public IList<EyouSoft.Model.PlanStructure.TrafficPricesInfo> GetTrafficPriceList(DateTime LTourDate, params int[] trafficId)
        {
            IList<EyouSoft.Model.PlanStructure.TrafficPricesInfo> list = null;
            EyouSoft.Model.PlanStructure.TrafficPricesInfo model = null;
            var strSql = new StringBuilder();
            strSql.Append(" SELECT PricesID,InsueTime,SDateTime, TrafficId,TicketPrices,TicketNums,[Status] ");
            strSql.AppendFormat(
                ",(SELECT SUM(to1.PeopleNumber - to1.LeaguePepoleNum) FROM tbl_TourOrder to1 WHERE to1.IsDelete = '0' AND EXISTS(SELECT 1 FROM tbl_TourOrderTraffic tot WHERE tot.OrderId = to1.ID AND tot.TrafficId = tbl_TrafficPrices.TrafficId) AND DATEDIFF(dd,to1.LeaveDate,tbl_TrafficPrices.SDateTime) = 0 and to1.OrderState IN({0},{1},{2})) as YiShiYong ",
                (int)Model.EnumType.TourStructure.OrderState.未处理,
                (int)Model.EnumType.TourStructure.OrderState.已成交,
                (int)Model.EnumType.TourStructure.OrderState.已留位);
            strSql.Append(" FROM tbl_TrafficPrices ");
            strSql.AppendFormat(" WHERE TrafficId in ({0}) AND DateDiff(dd,SDateTime,@LeaveDate)=0 ", trafficId);
            strSql.AppendFormat(" and [Status] = {0} ", (int)Model.EnumType.PlanStructure.TicketStatus.正常);
            DbCommand cmd = this._db.GetSqlStringCommand(strSql.ToString());
            this._db.AddInParameter(cmd, "LeaveDate", DbType.DateTime, LTourDate);
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                list = new List<EyouSoft.Model.PlanStructure.TrafficPricesInfo>();
                while (dr.Read())
                {
                    model = new EyouSoft.Model.PlanStructure.TrafficPricesInfo()
                    {
                        PricesID = dr["PricesID"].ToString(),
                        Status = (EyouSoft.Model.EnumType.PlanStructure.TicketStatus)int.Parse(dr[dr.GetOrdinal("Status")].ToString()),
                        InsueTime = dr.GetDateTime(dr.GetOrdinal("InsueTime")),
                        SDateTime = dr.GetDateTime(dr.GetOrdinal("SDateTime")),
                        TicketNums = !dr.IsDBNull(dr.GetOrdinal("TicketNums")) ? dr.GetInt32(dr.GetOrdinal("TicketNums")) : 0,
                        TicketPrices = !dr.IsDBNull(dr.GetOrdinal("TicketPrices")) ? dr.GetDecimal(dr.GetOrdinal("TicketPrices")) : 0,
                        TrafficId = dr.GetInt32(dr.GetOrdinal("TrafficId")),
                        YiShiYong = dr.IsDBNull(dr.GetOrdinal("YiShiYong")) ? 0 : dr.GetInt32(dr.GetOrdinal("YiShiYong"))
                    };
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 添加价格
        /// </summary>
        /// <param name="pricemodel">价格实体</param>
        /// <returns>true 成功  false 失败</returns>
        public bool AddTrafficPrice(EyouSoft.Model.PlanStructure.TrafficPricesInfo pricemodel)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_TRAFFIC_ADDPRICES);
            this._db.AddInParameter(cmd, "TrafficId", DbType.Int32, pricemodel.TrafficId);
            this._db.AddInParameter(cmd, "PricesID", DbType.AnsiStringFixedLength, pricemodel.PricesID);
            this._db.AddInParameter(cmd, "SDateTime", DbType.DateTime, pricemodel.SDateTime);
            this._db.AddInParameter(cmd, "TicketPrices", DbType.Decimal, pricemodel.TicketPrices);
            this._db.AddInParameter(cmd, "TicketNums", DbType.Int32, pricemodel.TicketNums);
            this._db.AddInParameter(cmd, "Status", DbType.Byte, Convert.ToInt32(pricemodel.Status));
            this._db.AddInParameter(cmd, "InsueTime", DbType.DateTime, pricemodel.InsueTime);
            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 修改价格
        /// </summary>
        /// <param name="pricemodel">价格实体</param>
        /// <returns>true 成功 false 失败</returns>
        public bool UpdateTrafficPrice(EyouSoft.Model.PlanStructure.TrafficPricesInfo pricemodel)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_TRAFFIC_UPDATEPRICES);
            this._db.AddInParameter(cmd, "TrafficId", DbType.Int32, pricemodel.TrafficId);
            this._db.AddInParameter(cmd, "SDateTime", DbType.DateTime, pricemodel.SDateTime);
            this._db.AddInParameter(cmd, "TicketPrices", DbType.Decimal, pricemodel.TicketPrices);
            this._db.AddInParameter(cmd, "TicketNums", DbType.Int32, pricemodel.TicketNums);
            this._db.AddInParameter(cmd, "Status", DbType.Byte, Convert.ToInt32(pricemodel.Status));
            this._db.AddInParameter(cmd, "InsueTime", DbType.DateTime, pricemodel.InsueTime);
            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取价格列表
        /// </summary>
        /// <param name="TrafficId">交通编号</param>
        /// <returns>价格集合</returns>
        public IList<EyouSoft.Model.PlanStructure.TrafficPricesInfo> GetPricesList(int trafficID)
        {
            IList<EyouSoft.Model.PlanStructure.TrafficPricesInfo> priceslist = null;
            EyouSoft.Model.PlanStructure.TrafficPricesInfo pricesmodel = null;
            StringBuilder sqlWhere = new StringBuilder();
            sqlWhere.AppendFormat(" SELECT * ");
            sqlWhere.AppendFormat(" ,(SELECT SUM(to1.PeopleNumber - to1.LeaguePepoleNum) FROM tbl_TourOrder to1 WHERE to1.IsDelete = '0' AND EXISTS(SELECT 1 FROM tbl_TourOrderTraffic tot WHERE tot.OrderId = to1.ID AND tot.TrafficId = tbl_TrafficPrices.TrafficId) AND DATEDIFF(dd,to1.LeaveDate,tbl_TrafficPrices.SDateTime) = 0 and to1.OrderState IN({0},{1},{2})) as YiShiYong ",
                (int)Model.EnumType.TourStructure.OrderState.未处理,
                (int)Model.EnumType.TourStructure.OrderState.已成交,
                (int)Model.EnumType.TourStructure.OrderState.已留位);
            sqlWhere.AppendFormat(" FROM tbl_TrafficPrices WHERE TrafficId = {0} ORDER BY InsueTime DESC ", trafficID);
            DbCommand cmd = this._db.GetSqlStringCommand(sqlWhere.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {
                priceslist = new List<EyouSoft.Model.PlanStructure.TrafficPricesInfo>();
                while (dr.Read())
                {
                    pricesmodel = new EyouSoft.Model.PlanStructure.TrafficPricesInfo()
                    {
                        InsueTime = dr.GetDateTime(dr.GetOrdinal("InsueTime")),
                        PricesID = !dr.IsDBNull(dr.GetOrdinal("PricesID")) ? dr.GetString(dr.GetOrdinal("PricesID")) : string.Empty,
                        SDateTime = dr.GetDateTime(dr.GetOrdinal("SDateTime")),
                        Status = (EyouSoft.Model.EnumType.PlanStructure.TicketStatus)int.Parse(dr[dr.GetOrdinal("Status")].ToString()),
                        TrafficId = Convert.ToInt32(dr["TrafficId"].ToString()),
                        TicketNums = !dr.IsDBNull(dr.GetOrdinal("TicketNums")) ? dr.GetInt32(dr.GetOrdinal("TicketNums")) : 0,
                        TicketPrices = !dr.IsDBNull(dr.GetOrdinal("TicketPrices")) ? dr.GetDecimal(dr.GetOrdinal("TicketPrices")) : 0,
                        YiShiYong = dr.IsDBNull(dr.GetOrdinal("YiShiYong")) ? 0 : dr.GetInt32(dr.GetOrdinal("YiShiYong"))
                    };
                    priceslist.Add(pricesmodel);
                }
            }
            return priceslist;
        }


        /// <summary>
        /// 交通某一天的价格修改后同步更新影响到的团队的信息(预控人数)
        /// </summary>
        /// <param name="trafficId">交通编号</param>
        public void TongBuGengXinTuanDui(int trafficId)
        {
            if (trafficId <= 0) return;

            var strSql = new StringBuilder();
            strSql.Append(" UPDATE tbl_Tour SET PlanPeopleNumber = ");
            strSql.Append(
                " IsNull((SELECT SUM(d.TicketNums) FROM  tbl_TrafficPrices AS d where d.TrafficId IN (SELECT tt.TrafficId FROM tbl_TourTraffic tt WHERE tt.TourId = tbl_Tour.TourId) AND DATEDIFF(dd,d.SDateTime,tbl_Tour.LeaveDate) = 0 AND d.[Status] = @TicketStatus),0) ");
            strSql.Append(
                " WHERE tbl_Tour.TourId IN (SELECT DISTINCT tt.TourId FROM tbl_TourTraffic tt WHERE tt.TrafficId = @TrafficId) ");
            strSql.Append(" and tbl_Tour.TourType = @TourType ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "TrafficId", DbType.Int32, trafficId);
            _db.AddInParameter(dc, "TicketStatus", DbType.Byte, (int)Model.EnumType.PlanStructure.TicketStatus.正常);
            _db.AddInParameter(dc, "TourType", DbType.Byte, (int)Model.EnumType.TourStructure.TourType.散拼计划);
            DbHelper.ExecuteSql(dc, _db);
        }

        /// <summary>
        /// 更新票状态
        /// </summary>
        /// <param name="pricesID">价格编号</param>
        ///  <param name="trafficId">交通编号</param>
        /// <param name="status">票状态</param>
        /// <returns>true 成功 false 失败</returns>
        public bool UpdatePricesStatus(string pricesID, int trafficId, EyouSoft.Model.EnumType.PlanStructure.TicketStatus status)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_TRAFFIC_UPDATEPRICESSTATUS);
            this._db.AddInParameter(cmd, "PricesID", DbType.String, pricesID);
            this._db.AddInParameter(cmd, "Status", DbType.Byte, (int)status);
            this._db.AddInParameter(cmd, "TrafficId", DbType.Int32, trafficId);
            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 获取价格实体
        /// </summary>
        /// <param name="pricesId">价格编号</param>
        /// <param name="trafficId">交通编号</param>
        /// <returns>价格实体</returns>
        public EyouSoft.Model.PlanStructure.TrafficPricesInfo GetTrafficPrices(string pricesId, int trafficId)
        {
            EyouSoft.Model.PlanStructure.TrafficPricesInfo model = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_TRAFFIC_GETPRICESINFO);
            this._db.AddInParameter(cmd, "PricesID", DbType.String, pricesId);
            this._db.AddInParameter(cmd, "TrafficId", DbType.Int32, trafficId);
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, this._db))
            {

                while (dr.Read())
                {
                    model = new EyouSoft.Model.PlanStructure.TrafficPricesInfo()
                    {
                        TrafficId = dr.IsDBNull(dr.GetOrdinal("TrafficId")) ? 0 : dr.GetInt32(dr.GetOrdinal("TrafficId")),
                        TicketPrices = !dr.IsDBNull(dr.GetOrdinal("TicketPrices")) ? dr.GetDecimal(dr.GetOrdinal("TicketPrices")) : 0,
                        TicketNums = !dr.IsDBNull(dr.GetOrdinal("TicketNums")) ? dr.GetInt32(dr.GetOrdinal("TicketNums")) : 0,
                        InsueTime = dr.GetDateTime(dr.GetOrdinal("InsueTime")),
                        PricesID = dr["PricesID"].ToString(),
                        Status = (EyouSoft.Model.EnumType.PlanStructure.TicketStatus)int.Parse(dr[dr.GetOrdinal("Status")].ToString()),
                        SDateTime = dr.GetDateTime(dr.GetOrdinal("SDateTime"))
                    };
                }
            }
            return model;
        }

        #endregion

        #region xmlConvertList 私有方法
        /// <summary>
        /// 获取交通行程
        /// </summary>
        /// <returns></returns>
        private IList<EyouSoft.Model.PlanStructure.TravelInfo> GetTravelListForXML(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<EyouSoft.Model.PlanStructure.TravelInfo> list = new List<EyouSoft.Model.PlanStructure.TravelInfo>();
            EyouSoft.Model.PlanStructure.TravelInfo item = null;
            System.Xml.Linq.XElement xRoot = System.Xml.Linq.XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                item = new EyouSoft.Model.PlanStructure.TravelInfo
                {
                    FilghtNum = Utils.GetXAttributeValue(xRow, "FilghtNum"),
                    FlightCompany = (EyouSoft.Model.EnumType.PlanStructure.FlightCompany)Convert.ToInt32(Utils.GetXAttributeValue(xRow, "FlightCompany")),
                    AirPlaneType = Utils.GetXAttributeValue(xRow, "AirPlaneType"),
                    IntervalDays = Convert.ToInt32(Utils.GetXAttributeValue(xRow, "IntervalDays")),
                    TravelId = Utils.GetInt(Utils.GetXAttributeValue(xRow, "TravelId")),
                    TrafficId = Utils.GetInt(Utils.GetXAttributeValue(xRow, "TrafficId")),
                    TrafficType = (EyouSoft.Model.EnumType.PlanStructure.TrafficType)Convert.ToInt32(Utils.GetXAttributeValue(xRow, "TrafficType")),
                    Space = (EyouSoft.Model.EnumType.PlanStructure.Space)Convert.ToInt32(Utils.GetXAttributeValue(xRow, "Space")),
                    SerialNum = Convert.ToInt32(Utils.GetXAttributeValue(xRow, "SerialNum")),
                    LProvinceName = Utils.GetXAttributeValue(xRow, "LProvinceName"),
                    LCityName = Utils.GetXAttributeValue(xRow, "LCityName"),
                    RProvinceName = Utils.GetXAttributeValue(xRow, "RProvinceName"),
                    RCityName = Utils.GetXAttributeValue(xRow, "RCityName")
                };
                list.Add(item);
            }
            return list;
        }
        #endregion
    }
}
