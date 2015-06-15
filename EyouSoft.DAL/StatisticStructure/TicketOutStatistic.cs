using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using EyouSoft.Toolkit;
using System.Data;
using System.Xml.Linq;
using System.Text;
using System.Data.Common;

namespace EyouSoft.DAL.StatisticStructure
{
    /// <summary>
    /// 出票统计数据访问
    /// </summary>
    /// 周文超 2011-03-18
    public class TicketOutStatistic : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.StatisticStructure.ITicketOutStatistic
    {
        /// <summary>
        /// 数据库
        /// </summary>
        private readonly Database _db = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TicketOutStatistic()
        {
            _db = base.SystemStore;
        }

        #region ITicketOutStatistic 成员

        /// <summary>
        /// 获取出票--售票处统计
        /// </summary>
        /// <param name="model">出票统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.TicketOutStatisticOffice> GetTicketOutStatisticOffice(EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti model, string HaveUserIds)
        {
            if (model == null)
                return null;

            IList<EyouSoft.Model.StatisticStructure.TicketOutStatisticOffice> list = new List<EyouSoft.Model.StatisticStructure.TicketOutStatisticOffice>();
            EyouSoft.Model.StatisticStructure.TicketOutStatisticOffice tmpModel = null;
            var strSql = new StringBuilder();
            var strWhere = new StringBuilder();

            #region  Sql

            strWhere.AppendFormat(" A.[State] = {0} ", (int)Model.EnumType.PlanStructure.TicketState.已出票);
            //供应商被删除，按售票处统计不统计，其他统计
            strWhere.Append(" and exists (select 1 from tbl_CompanySupplier where tbl_CompanySupplier.Id = A.TicketOfficeId and tbl_CompanySupplier.IsDelete = '0') ");
            if (model.CompanyId > 0)
                strWhere.AppendFormat(" and A.CompanyID = {0} ", model.CompanyId);
            if ((model.DepartIds != null && model.DepartIds.Length > 0) || !string.IsNullOrEmpty(model.DepartName))
            {
                strWhere.Append(" and exists (select 1 from tbl_CompanyUser where tbl_CompanyUser.ID = A.RegisterOperatorId ");
                if (model.DepartIds != null && model.DepartIds.Length > 0)
                {
                    string strIds = GetIdsByIdArr(model.DepartIds);
                    if (!string.IsNullOrEmpty(strIds))
                        strWhere.AppendFormat(" and tbl_CompanyUser.DepartId in ({0}) ", strIds);
                }
                if (!string.IsNullOrEmpty(model.DepartName))
                    strWhere.AppendFormat(" and tbl_CompanyUser.DepartName like '%{0}%' ", model.DepartName);

                strWhere.Append(" ) ");
            }
            if (model.OfficeId > 0)
                strWhere.AppendFormat(" and A.TicketOfficeId = {0} ", model.OfficeId);
            if (!string.IsNullOrEmpty(model.OfficeName))
                strWhere.AppendFormat(" and A.TicketOffice like '%{0}%' ", model.OfficeName);
            if (model.StartTicketOutTime.HasValue)
                strWhere.AppendFormat(" and datediff(dd,'{0}',A.TicketOutTime) >= 0 ", model.StartTicketOutTime.Value);
            if (model.EndTicketOutTime.HasValue)
                strWhere.AppendFormat(" and datediff(dd,A.TicketOutTime,'{0}') >= 0 ", model.EndTicketOutTime.Value);
            if (!string.IsNullOrEmpty(HaveUserIds) || model.LeaveDateStart.HasValue || model.LeaveDateEnd.HasValue)
            {
                strWhere.Append(" and exists (select 1 from tbl_Tour as T where T.TourId = A.TourId ");

                if (!string.IsNullOrEmpty(HaveUserIds))
                    strWhere.AppendFormat(" and T.OperatorId in ({0}) ", HaveUserIds);
                if (model.LeaveDateStart.HasValue)
                    strWhere.AppendFormat(" and T.LeaveDate >= '{0}' ", model.LeaveDateStart.Value);
                if (model.LeaveDateEnd.HasValue)
                    strWhere.AppendFormat(" and T.LeaveDate <= '{0}' ", model.LeaveDateEnd.Value);

                strWhere.Append(" ) ");
            }
            if (model.AirLineIds != null && model.AirLineIds.Length > 0)
            {
                string strAirIds = GetIdsByIdArr(model.AirLineIds);
                if (!string.IsNullOrEmpty(strAirIds))
                    strWhere.AppendFormat(
                        " and exists (select 1 from tbl_planticketflight as C where C.ticketid = A.Id and C.AireLine in ({0})) ",
                        strAirIds);
            }

            strSql.Append(" select ticketofficeid,TicketOffice,sum(peoplecount) as peoplecount,sum(TotalAmount) as TotalAmount ");
            strSql.Append(" ,sum(PayAmount) as PayAmount ");
            strSql.Append(" from ( ");
            strSql.Append(" select ticketofficeid,TotalAmount,PayAmount ");
            strSql.Append(" ,(select UnitName from tbl_CompanySupplier where Id = A.TicketOfficeId) as TicketOffice  ");
            strSql.Append(
                " ,(select isnull(SUM(peoplecount),0) from tbl_planticketkind AS B where A.id = B.ticketid ) as peoplecount ");
            strSql.Append(" from tbl_planticketout  AS A  ");
            strSql.AppendFormat(" where {0} ", strWhere.ToString());
            strSql.Append(" ) as pto ");
            strSql.Append(" group by ticketofficeid,TicketOffice ");

            switch (model.OrderIndex)
            {
                case 0:
                    strSql.Append(" order by TicketOfficeId asc ");
                    break;
                case 1:
                    strSql.Append(" order by TicketOfficeId desc ");
                    break;
                default:
                    strSql.Append(" order by TicketOfficeId asc ");
                    break;
            }

            #endregion

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            #region 实体赋值

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.StatisticStructure.TicketOutStatisticOffice();

                    if (!dr.IsDBNull(0))
                        tmpModel.OfficeId = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        tmpModel.OfficeName = dr.GetString(1);
                    if (!dr.IsDBNull(2))
                        tmpModel.TicketOutNum = dr.GetInt32(2);
                    if (!dr.IsDBNull(3))
                        tmpModel.TotalAmount = dr.GetDecimal(3);
                    if (!dr.IsDBNull(4))
                        tmpModel.PayAmount = dr.GetDecimal(4);

                    list.Add(tmpModel);
                }
            }

            #endregion

            return list;
        }

        /// <summary>
        /// 获取出票--航空公司统计
        /// </summary>
        /// <param name="model">出票统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.TicketOutStatisticAirLine> GetTicketOutStatisticAirLine(EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti model, string HaveUserIds)
        {
            if (model == null)
                return null;

            IList<EyouSoft.Model.StatisticStructure.TicketOutStatisticAirLine> list = new List<EyouSoft.Model.StatisticStructure.TicketOutStatisticAirLine>();
            EyouSoft.Model.StatisticStructure.TicketOutStatisticAirLine tmpModel = null;
            StringBuilder strSql = new StringBuilder();
            StringBuilder strWhere = new StringBuilder();


            #region Sql

            strWhere.AppendFormat(" C.[State] = {0} ", (int)Model.EnumType.PlanStructure.TicketState.已出票);
            if (model.CompanyId > 0)
                strWhere.AppendFormat(" and C.CompanyID = {0} ", model.CompanyId);
            if ((model.DepartIds != null && model.DepartIds.Length > 0) || !string.IsNullOrEmpty(model.DepartName))
            {
                strWhere.Append(" and exists (select 1 from tbl_CompanyUser where tbl_CompanyUser.ID = C.RegisterOperatorId ");
                if (model.DepartIds != null && model.DepartIds.Length > 0)
                {
                    string strIds = GetIdsByIdArr(model.DepartIds);
                    if (!string.IsNullOrEmpty(strIds))
                        strWhere.AppendFormat(" and tbl_CompanyUser.DepartId in ({0}) ", strIds);
                }
                if (!string.IsNullOrEmpty(model.DepartName))
                    strWhere.AppendFormat(" and tbl_CompanyUser.DepartName like '%{0}%' ", model.DepartName);

                strWhere.Append(" ) ");
            }
            if (model.OfficeId > 0)
                strWhere.AppendFormat(" and C.TicketOfficeId = {0} ", model.OfficeId);
            if (!string.IsNullOrEmpty(model.OfficeName))
                strWhere.AppendFormat(" and C.TicketOffice like '%{0}%' ", model.OfficeName);
            if (model.StartTicketOutTime.HasValue)
                strWhere.AppendFormat(" and datediff(dd,'{0}',C.TicketOutTime) >= 0 ", model.StartTicketOutTime.Value);
            if (model.EndTicketOutTime.HasValue)
                strWhere.AppendFormat(" and datediff(dd,C.TicketOutTime,'{0}') >= 0 ", model.EndTicketOutTime.Value);
            if (model.AirLineIds != null && model.AirLineIds.Length > 0)
            {
                string strAirIds = GetIdsByIdArr(model.AirLineIds);
                if (!string.IsNullOrEmpty(strAirIds))
                    strWhere.AppendFormat(" and B.AireLine in ({0}) ", strAirIds);
            }
            if (!string.IsNullOrEmpty(HaveUserIds) || model.LeaveDateStart.HasValue || model.LeaveDateEnd.HasValue)
            {
                strWhere.Append(" and exists (select 1 from tbl_Tour as T where T.TourId = C.TourId ");

                if (!string.IsNullOrEmpty(HaveUserIds))
                    strWhere.AppendFormat(" and T.OperatorId in ({0}) ", HaveUserIds);
                if (model.LeaveDateStart.HasValue)
                    strWhere.AppendFormat(" and T.LeaveDate >= '{0}' ", model.LeaveDateStart.Value);
                if (model.LeaveDateEnd.HasValue)
                    strWhere.AppendFormat(" and T.LeaveDate <= '{0}' ", model.LeaveDateEnd.Value);

                strWhere.Append(" ) ");
            }

            /*
             * 说明：按照航空公司统计，如果一个机票申请有两个航段的话，会导致机票款和人数统计多次
             * 即分别统计到不同的航空公司头上；所以统计出两个值,有case语句的是将机票款和人数统计到机票申请的第一个航段的航空公司上面
             * 没有case语句的是重复统计的值（每个航段的航空公司都加了机票申请的机票款和人数）
             */
            strSql.Append(" select AireLine ");
            strSql.Append(" ,Sum(case rownumber when 1 then TotalAmount else 0 end) as TotalAmount ");
            strSql.Append(" ,Sum(case rownumber when 1 then PayAmount else 0 end) as PayAmount ");
            strSql.Append(" ,Sum(case rownumber when 1 then peoplecount else 0 end) as peoplecount ");
            strSql.Append(" ,sum(TotalAmount) as TotalAmountAll ");
            strSql.Append(" ,sum(PayAmount) as PayAmount ");
            strSql.Append(" ,sum(peoplecount) as peoplecount ");
            strSql.Append(" from ( ");
            strSql.Append(" select AireLine,C.ID,C.TotalAmount,C.PayAmount ");
            strSql.Append(" ,ROW_NUMBER() OVER(partition by C.ID order by C.ID asc) as rownumber ");
            strSql.Append(" ,(select isnull(SUM(peoplecount),0) from tbl_planticketkind AS B where C.id = B.ticketid ) as peoplecount ");
            strSql.Append(" from tbl_PlanTicketFlight as B inner join tbl_planticketout as C on B.ticketid = C.Id ");
            strSql.AppendFormat(" where {0} ", strWhere.ToString());
            strSql.Append(" ) as A  ");
            strSql.Append(" group by AireLine ");

            #endregion

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            #region 实体赋值

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.StatisticStructure.TicketOutStatisticAirLine();

                    if (!dr.IsDBNull(0))
                        tmpModel.AirLineId = dr.GetByte(0);
                    if (!dr.IsDBNull(1))
                        tmpModel.TotalAmount = dr.GetDecimal(1);
                    if (!dr.IsDBNull(2))
                        tmpModel.PayAmount = dr.GetDecimal(2);
                    if (!dr.IsDBNull(3))
                        tmpModel.TicketOutNum = dr.GetInt32(3);

                    list.Add(tmpModel);
                }
            }

            #endregion

            return list;
        }

        /// <summary>
        /// 获取出票--部门统计
        /// </summary>
        /// <param name="model">出票统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.TicketOutStatisticDepart> GetTicketOutStatisticDepart(EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti model, string HaveUserIds)
        {
            if (model == null)
                return null;

            IList<EyouSoft.Model.StatisticStructure.TicketOutStatisticDepart> list = new List<EyouSoft.Model.StatisticStructure.TicketOutStatisticDepart>();
            EyouSoft.Model.StatisticStructure.TicketOutStatisticDepart tmpModel = null;
            StringBuilder strSql = new StringBuilder();
            StringBuilder strWhere = new StringBuilder("");
            strWhere.AppendFormat(" pto.[State] = {0} ", (int)Model.EnumType.PlanStructure.TicketState.已出票);

            #region SqlWhere

            if (model.CompanyId > 0)
                strWhere.AppendFormat(" and pto.CompanyID = {0} ", model.CompanyId);
            if (model.DepartIds != null && model.DepartIds.Length > 0)
                strWhere.AppendFormat(" and pto.DepartId in ({0}) ", this.GetIdsByIdArr(model.DepartIds));
            if (!string.IsNullOrEmpty(model.DepartName))
                strWhere.AppendFormat(" and pto.DepartName like '%{0}%' ", model.DepartName);
            if (model.OfficeId > 0)
                strWhere.AppendFormat(" and pto.TicketOfficeId = {0} ", model.OfficeId);
            if (!string.IsNullOrEmpty(model.OfficeName))
                strWhere.AppendFormat(" and pto.TicketOffice like '%{0}%' ", model.OfficeName);
            if (model.StartTicketOutTime.HasValue)
                strWhere.AppendFormat(" and datediff(dd,'{0}',pto.TicketOutTime) >= 0 ", model.StartTicketOutTime.Value);
            if (model.EndTicketOutTime.HasValue)
                strWhere.AppendFormat(" and datediff(dd,pto.TicketOutTime,'{0}') >= 0 ", model.EndTicketOutTime.Value);
            if (model.AirLineIds != null && model.AirLineIds.Length > 0)
            {
                strWhere.AppendFormat(" and exists (select 1 from tbl_PlanTicketFlight where tbl_PlanTicketFlight.TicketId in (select distinct ID from View_TicketOutStatisticDepart where View_TicketOutStatisticDepart.DepartId = pto.DepartId and {0}) ", strWhere.ToString().Replace("pto.", "View_TicketOutStatisticDepart."));
                if (model.AirLineIds != null && model.AirLineIds.Length > 0)
                    strWhere.AppendFormat(" and tbl_PlanTicketFlight.AireLine in ({0}) ", this.GetIdsByIdArr(model.AirLineIds));
                strWhere.Append(") ");
            }
            if (model.LeaveDateStart.HasValue || model.LeaveDateEnd.HasValue || (!string.IsNullOrEmpty(HaveUserIds)))
            {
                strWhere.Append(" and exists (select 1 from tbl_Tour where tbl_Tour.TourId = pto.TourId ");
                if (!string.IsNullOrEmpty(HaveUserIds))
                    strWhere.AppendFormat(" and tbl_Tour.OperatorId in ({0}) ", HaveUserIds);
                if (model.LeaveDateStart.HasValue)
                    strWhere.AppendFormat(" and tbl_Tour.LeaveDate >= '{0}' ", model.LeaveDateStart.Value);
                if (model.LeaveDateEnd.HasValue)
                    strWhere.AppendFormat(" and tbl_Tour.LeaveDate <= '{0}' ", model.LeaveDateEnd.Value);

                strWhere.Append(" ) ");
            }

            #endregion

            #region Sql

            strSql.Append(" select DepartId,DepartName ");
            strSql.Append(" ,sum(PeopleCount) as PeopleCount,sum(TotalAmount) as TotalAmount,sum(PayAmount) as PayAmount ");
            strSql.Append(" from View_TicketOutStatisticDepart as pto ");
            strSql.AppendFormat(" where {0} ", strWhere.ToString());
            strSql.Append(" group by DepartId,DepartName ");

            switch (model.OrderIndex)
            {
                case 4:
                    strSql.Append(" order by DepartId asc ");
                    break;
                case 5:
                    strSql.Append(" order by DepartId desc ");
                    break;
                default:
                    strSql.Append(" order by DepartId asc ");
                    break;
            }

            #endregion

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            #region 实体赋值

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.StatisticStructure.TicketOutStatisticDepart();

                    if (!dr.IsDBNull(0))
                        tmpModel.DepartId = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        tmpModel.DepartName = dr.GetString(1);
                    if (!dr.IsDBNull(2))
                        tmpModel.TicketOutNum = dr.GetInt32(2);
                    if (!dr.IsDBNull(3))
                        tmpModel.TotalAmount = dr.GetDecimal(3);
                    if (!dr.IsDBNull(4))
                        tmpModel.PayAmount = dr.GetDecimal(4);

                    list.Add(tmpModel);
                }
            }

            #endregion

            return list;
        }

        /// <summary>
        /// 获取出票--日期统计
        /// </summary>
        /// <param name="model">出票统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.TicketOutStatisticTime> GetTicketOutStatisticTime(EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti model, string HaveUserIds)
        {
            if (model == null)
                return null;

            IList<EyouSoft.Model.StatisticStructure.TicketOutStatisticTime> list = new List<EyouSoft.Model.StatisticStructure.TicketOutStatisticTime>();
            EyouSoft.Model.StatisticStructure.TicketOutStatisticTime tmpModel = null;
            StringBuilder strSql = new StringBuilder();
            StringBuilder strWhere = new StringBuilder("");
            strWhere.AppendFormat(" pto.[State] = {0} ", (int)Model.EnumType.PlanStructure.TicketState.已出票);

            #region SqlWhere

            if (model.CompanyId > 0)
                strWhere.AppendFormat(" and pto.CompanyID = {0} ", model.CompanyId);
            if (model.DepartIds != null && model.DepartIds.Length > 0)
                strWhere.AppendFormat(" and pto.DepartId in ({0}) ", this.GetIdsByIdArr(model.DepartIds));
            if (!string.IsNullOrEmpty(model.DepartName))
                strWhere.AppendFormat(" and pto.DepartName like '%{0}%' ", model.DepartName);
            if (model.OfficeId > 0)
                strWhere.AppendFormat(" and pto.TicketOfficeId = {0} ", model.OfficeId);
            if (!string.IsNullOrEmpty(model.OfficeName))
                strWhere.AppendFormat(" and pto.TicketOffice like '%{0}%' ", model.OfficeName);
            if (model.StartTicketOutTime.HasValue)
                strWhere.AppendFormat(" and datediff(dd,'{0}',pto.TicketOutTime) >= 0 ", model.StartTicketOutTime.Value);
            if (model.EndTicketOutTime.HasValue)
                strWhere.AppendFormat(" and datediff(dd,pto.TicketOutTime,'{0}') >= 0 ", model.EndTicketOutTime.Value);
            if (model.AirLineIds != null && model.AirLineIds.Length > 0)
            {
                strWhere.AppendFormat(" and exists (select 1 from tbl_PlanTicketFlight where tbl_PlanTicketFlight.TicketId in (select distinct ID from View_TicketOutStatisticDepart where year(View_TicketOutStatisticDepart.TicketOutTime) = year(pto.TicketOutTime) and month(View_TicketOutStatisticDepart.TicketOutTime) = month(pto.TicketOutTime) and {0}) ", strWhere.ToString().Replace("pto.", "View_TicketOutStatisticDepart."));
                if (model.AirLineIds != null && model.AirLineIds.Length > 0)
                    strWhere.AppendFormat(" and tbl_PlanTicketFlight.AireLine in ({0}) ", this.GetIdsByIdArr(model.AirLineIds));
                strWhere.Append(") ");
            }
            if (model.LeaveDateStart.HasValue || model.LeaveDateEnd.HasValue || (!string.IsNullOrEmpty(HaveUserIds)))
            {
                strWhere.Append(" and exists (select 1 from tbl_Tour where tbl_Tour.TourId = pto.TourId ");
                if (!string.IsNullOrEmpty(HaveUserIds))
                    strWhere.AppendFormat(" and tbl_Tour.OperatorId in ({0}) ", HaveUserIds);
                if (model.LeaveDateStart.HasValue)
                    strWhere.AppendFormat(" and tbl_Tour.LeaveDate >= '{0}' ", model.LeaveDateStart.Value);
                if (model.LeaveDateEnd.HasValue)
                    strWhere.AppendFormat(" and tbl_Tour.LeaveDate <= '{0}' ", model.LeaveDateEnd.Value);

                strWhere.Append(" ) ");
            }

            #endregion

            #region Sql

            strSql.Append(" select year(TicketOutTime) as CurrYear,month(TicketOutTime) as CurrMonth ");
            strSql.Append(" ,sum(PeopleCount) as PeopleCount,sum(TotalAmount) as TotalAmount,sum(PayAmount) as PayAmount ");
            strSql.Append(" from View_TicketOutStatisticDepart as pto ");
            strSql.AppendFormat(" where {0} ", strWhere.ToString());
            strSql.Append(" group by year(TicketOutTime),month(TicketOutTime) ");

            switch (model.OrderIndex)
            {
                case 4:
                    strSql.Append(" order by CurrYear asc,CurrMonth asc ");
                    break;
                case 5:
                    strSql.Append(" order by CurrYear desc,CurrMonth desc ");
                    break;
                default:
                    strSql.Append(" order by CurrYear asc,CurrMonth asc ");
                    break;
            }

            #endregion

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            #region 实体赋值

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.StatisticStructure.TicketOutStatisticTime();

                    if (!dr.IsDBNull(0))
                        tmpModel.CurrYear = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        tmpModel.CurrMonth = dr.GetInt32(1);
                    if (!dr.IsDBNull(2))
                        tmpModel.TicketOutNum = dr.GetInt32(2);
                    if (!dr.IsDBNull(3))
                        tmpModel.TotalAmount = dr.GetDecimal(3);
                    if (!dr.IsDBNull(4))
                        tmpModel.PayAmount = dr.GetDecimal(4);

                    list.Add(tmpModel);
                }
            }

            #endregion

            return list;
        }

        #endregion

        #region 私有成员

        ///// <summary>
        ///// 根据查询实体生成Sqlwhere子句
        ///// </summary>
        ///// <param name="model">查询实体</param>
        ///// <param name="HaveUserIds">用户Id集合，用半角逗号分割</param>
        ///// <param name="strOrderBy">排序Sql语句</param>
        ///// <returns>Sqlwhere子句</returns>
        //private string GetSqlWhere(EyouSoft.Model.StatisticStructure.QueryTicketOutStatisti model, string HaveUserIds
        //    , ref string strOrderBy)
        //{
        //    StringBuilder strWhere = new StringBuilder(" [State] = 3 ");
        //    if (model == null)
        //        return strWhere.ToString();

        //    if (model.CompanyId > 0)
        //        strWhere.AppendFormat(" and CompanyID = {0} ", model.CompanyId);
        //    if (!string.IsNullOrEmpty(HaveUserIds))
        //        strWhere.AppendFormat(" and RegisterOperatorId in {0} ", HaveUserIds);
        //    if ((model.DepartIds != null && model.DepartIds.Length > 0) || !string.IsNullOrEmpty(model.DepartName))
        //    {
        //        string strTmpWhere = " and exists (select 1 from tbl_CompanyUser where tbl_CompanyUser.ID = pto.RegisterOperatorId ";
        //        if (model.DepartIds != null && model.DepartIds.Length > 0)
        //            strTmpWhere += string.Format(" and tbl_CompanyUser.DepartId in ({0}) ", this.GetIdsByIdArr(model.DepartIds));
        //        if (!string.IsNullOrEmpty(model.DepartName))
        //            strTmpWhere += string.Format(" and tbl_CompanyUser.DepartName like '%{0}%' ", model.DepartName);

        //        strTmpWhere += " ) ";

        //        strWhere.Append(strTmpWhere);
        //    }
        //    if (model.OfficeId > 0)
        //        strWhere.AppendFormat(" and TicketOfficeId = {0} ", model.OfficeId);
        //    if (!string.IsNullOrEmpty(model.OfficeName))
        //        strWhere.AppendFormat(" and TicketOffice like '%{0}%' ", model.OfficeName);
        //    if (model.StartTicketOutTime.HasValue)
        //        strWhere.AppendFormat(" and datediff(dd,'{0}',TicketOutTime) >= 0 ", model.StartTicketOutTime.Value);
        //    if (model.EndTicketOutTime.HasValue)
        //        strWhere.AppendFormat(" and datediff(dd,TicketOutTime,'{0}') >= 0 ", model.EndTicketOutTime.Value);
        //    if (model.AirLineIds != null && model.AirLineIds.Length > 0)
        //        strWhere.AppendFormat("", this.GetIdsByIdArr(model.AirLineIds));

        //    return strWhere.ToString();
        //}

        /// <summary>
        /// 根据Id数组生成半角逗号分割的Id字符串
        /// </summary>
        /// <param name="IdArr">Id数组</param>
        /// <returns>半角逗号分割的Id字符串</returns>
        private string GetIdsByIdArr(int[] IdArr)
        {
            if (IdArr == null || IdArr.Length <= 0)
                return string.Empty;

            string strIds = string.Empty;
            foreach (int i in IdArr)
            {
                if (i <= 0)
                    continue;

                strIds += i.ToString() + ",";
            }
            strIds = strIds.Trim(',');

            return strIds;
        }

        #endregion
    }
}
