/************************************************************
 * 模块名称：退票统计数据访问
 * 功能说明：
 * 创建人：周文超  2011-4-22 9:53:25
 * *********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using EyouSoft.Toolkit;
using System.Data;
using System.Xml.Linq;

namespace EyouSoft.DAL.StatisticStructure
{
    /// <summary>
    /// 退票统计数据访问
    /// </summary>
    public class DRefundStatistic : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.StatisticStructure.IRefundStatistic
    {
        private readonly Database _db = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DRefundStatistic()
        {
            _db = base.SystemStore;
        }

        #region IRefundStatistic 成员

        /// <summary>
        /// 获取退票统计
        /// </summary>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="model">查询实体</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.MRefundStatistic> GetRefundStatistic(int PageSize, int PageIndex, ref int RecordCount
            , EyouSoft.Model.StatisticStructure.MQueryRefundStatistic model, string us)
        {
            if (model == null || model.CompanyId <= 0)
                return null;

            IList<EyouSoft.Model.StatisticStructure.MRefundStatistic> list = null;
            EyouSoft.Model.StatisticStructure.MRefundStatistic TMPModel = null;
            string strFile = " Id,CustormerId,OperatorName,VisitorName,TourNo,RouteName,LeaveDate,BuyCompanyName,OrderOperatorName,TotalAmount,FligthInfo,RefundAmount ";
            string strPK = "Id";
            string strTableName = "View_RefundStatistics";
            string strOrderBy = " TourNo asc ";
            string strWhere = this.GetSqlWhereByQueryModel(model, us);
            switch (model.OrderIndex)
            {
                case 0:
                    strOrderBy = " TourNo asc ";
                    break;
                case 1:
                    strOrderBy = " TourNo desc ";
                    break;
                case 2:
                    strOrderBy = " IssueTime asc ";
                    break;
                case 3:
                    strOrderBy = " IssueTime desc ";
                    break;
            }

            using (IDataReader dr = DbHelper.ExecuteReader(this._db, PageSize, PageIndex, ref RecordCount, strTableName, strPK, strFile, strWhere, strOrderBy))
            {
                list = new List<EyouSoft.Model.StatisticStructure.MRefundStatistic>();

                while (dr.Read())
                {
                    //Id,CustormerId,OperatorName,VisitorName,TourNo,RouteName,LeaveDate,BuyCompanyName,OrderOperatorName,TotalAmount,FligthInfo,RefundAmount
                    TMPModel = new EyouSoft.Model.StatisticStructure.MRefundStatistic();
                    if (!dr.IsDBNull(0))
                        TMPModel.RefundId = dr.GetString(0);
                    if (!dr.IsDBNull(1))
                        TMPModel.CustomerId = dr.GetString(1);
                    if (!dr.IsDBNull(2))
                        TMPModel.OperatorName = dr.GetString(2);
                    if (!dr.IsDBNull(3))
                        TMPModel.RefundName = dr.GetString(3);
                    if (!dr.IsDBNull(4))
                        TMPModel.TourNo = dr.GetString(4);
                    if (!dr.IsDBNull(5))
                        TMPModel.RouteName = dr.GetString(5);
                    if (!dr.IsDBNull(6))
                        TMPModel.LeaveDate = dr.GetDateTime(6);
                    if (!dr.IsDBNull(7))
                        TMPModel.BuyCompanyName = dr.GetString(7);
                    if (!dr.IsDBNull(8))
                        TMPModel.BuyOrderOperatorName = dr.GetString(8);
                    if (!dr.IsDBNull(9))
                        TMPModel.TicketPrice = dr.GetDecimal(9);
                    if (!dr.IsDBNull(10))
                        TMPModel.RefundFlight = this.GetTicketFlightXML(dr.GetString(10));
                    if (!dr.IsDBNull(11))
                        TMPModel.RefundAmount = dr.GetDecimal(11);

                    list.Add(TMPModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取退票统计合计信息
        /// </summary>
        /// <param name="refundAmount">退回金额合计</param>
        /// <param name="model">查询实体</param>
        /// <param name="us">用户信息集合</param>
        public void GetSumRefundStatistic(ref decimal refundAmount, Model.StatisticStructure.MQueryRefundStatistic model, string us)
        {
            refundAmount = 0M;
            var strSql = new StringBuilder();
            strSql.Append(" select sum(RefundAmount) from View_RefundStatistics ");
            string strWhere = GetSqlWhereByQueryModel(model, us);
            if (!string.IsNullOrEmpty(strWhere))
                strSql.AppendFormat(" where {0} ", strWhere);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    refundAmount = dr.IsDBNull(0) ? 0M : dr.GetDecimal(0);
                }
            }
        }

        #endregion

        #region  私有方法

        /// <summary>
        /// 根据查询实体生成SqlWhere语句
        /// </summary>
        /// <param name="model">查询实体</param>
        /// <param name="us">用户信息集合</param>
        /// <returns>SqlWhere语句</returns>
        private string GetSqlWhereByQueryModel(EyouSoft.Model.StatisticStructure.MQueryRefundStatistic model, string us)
        {
            if (model == null || model.CompanyId <= 0)
                return string.Empty;

            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" SellCompanyId = {0} ", model.CompanyId);
            if (!string.IsNullOrEmpty(model.TourNo))
                strWhere.AppendFormat(" and TourNo like '%{0}%' ", model.TourNo);
            if (!string.IsNullOrEmpty(model.RouteName))
                strWhere.AppendFormat(" and RouteName like '%{0}%' ", model.RouteName);
            if (model.LeaveDateStart.HasValue)
                strWhere.AppendFormat(" and datediff(dd,'{0}',LeaveDate) >= 0 ", model.LeaveDateStart.Value.ToShortDateString());
            if (model.LeaveDateEnd.HasValue)
                strWhere.AppendFormat(" and datediff(dd,LeaveDate,'{0}') >= 0 ", model.LeaveDateEnd.Value.ToShortDateString());
            if (!string.IsNullOrEmpty(model.FligthSegment) || model.AireLine.HasValue)
            {
                strWhere.Append(" and exists (select 1 from tbl_CustomerRefundFlight where tbl_CustomerRefundFlight.RefundId = ID and exists (select 1 from tbl_PlanTicketFlight where tbl_PlanTicketFlight.ID = tbl_CustomerRefundFlight.FlightId ");
                if (!string.IsNullOrEmpty(model.FligthSegment))
                    strWhere.AppendFormat(" and tbl_PlanTicketFlight.FligthSegment like '%{0}%' ", model.FligthSegment);
                if (model.AireLine.HasValue)
                    strWhere.AppendFormat(" and tbl_PlanTicketFlight.AireLine = {0} ", (int)model.AireLine.Value);

                strWhere.Append(" )) ");
            }
            if (!string.IsNullOrEmpty(model.BuyCompanyName))
                strWhere.AppendFormat(" and BuyCompanyName like '%{0}%' ", model.BuyCompanyName);
            if (model.BuyCompanyId > 0)
                strWhere.AppendFormat(" and BuyCompanyID = {0} ", model.BuyCompanyId);
            if (!string.IsNullOrEmpty(us))
            {
                strWhere.AppendFormat(" AND TourOperatorId IN({0}) ", us);
            }
            var opIds = new List<int>();
            if (model.OperatorIds != null && model.OperatorIds.Length > 0)
            {
                foreach (var i in model.OperatorIds)
                {
                    if (i <= 0)
                        continue;

                    if (opIds.Contains(i))
                        continue;

                    opIds.Add(i);
                }
            }
            if (model.DepIds != null && model.DepIds.Length > 0)
            {
                int[] userIds = new DAL.CompanyStructure.CompanyUser().GetUserIdsByDepartIds(model.DepIds);
                if (userIds != null && userIds.Length > 0)
                {
                    foreach (var i in userIds)
                    {
                        if (i <= 0)
                            continue;

                        if (opIds.Contains(i))
                            continue;

                        opIds.Add(i);
                    }
                }
            }
            if (opIds != null && opIds.Count > 0)
            {
                string strIds = string.Empty;
                foreach (int i in opIds)
                {
                    if (i <= 0)
                        continue;

                    strIds += i + ",";
                }
                strIds = strIds.Trim(',');
                if (!string.IsNullOrEmpty(strIds))
                    strWhere.AppendFormat(" and OperatorId in ({0}) ", strIds);
            }

            return strWhere.ToString();
        }

        /// <summary>
        /// 根据退票航段信息XML生成集合
        /// </summary>
        /// <param name="TicketFlightXML">退票航段信息XML</param>
        /// <returns>退票航段信息集合</returns>
        private IList<Model.PlanStructure.TicketFlight> GetTicketFlightXML(string TicketFlightXML)
        {
            if (string.IsNullOrEmpty(TicketFlightXML))
                return null;

            XElement xRoot = XElement.Parse(TicketFlightXML);
            var xRows = Utils.GetXElements(xRoot, "row");
            if (xRows == null || xRows.Count() <= 0)
                return null;

            IList<Model.PlanStructure.TicketFlight> list = new List<Model.PlanStructure.TicketFlight>();
            Model.PlanStructure.TicketFlight tm = null;
            foreach (var t in xRows)
            {
                if (t == null)
                    continue;

                tm = new EyouSoft.Model.PlanStructure.TicketFlight();
                tm.ID = Utils.GetInt(Utils.GetXAttributeValue(t, "ID"));
                tm.FligthSegment = Utils.GetXAttributeValue(t, "FligthSegment");
                tm.DepartureTime = Utils.GetDateTime(Utils.GetXAttributeValue(t, "DepartureTime"));
                tm.AireLine = (EyouSoft.Model.EnumType.PlanStructure.FlightCompany)Utils.GetInt(Utils.GetXAttributeValue(t, "AireLine"));
                tm.Discount = Utils.GetDecimal(Utils.GetXAttributeValue(t, "Discount"));
                tm.TicketId = Utils.GetXAttributeValue(t, "TicketId");
                tm.TicketTime = Utils.GetXAttributeValue(t, "TicketTime");
                // 票号 add by zhengzy 2011-10-28
                tm.TicketNum = Utils.GetXAttributeValue(t, "TicketNum");

                list.Add(tm);
            }

            return list;
        }

        #endregion
    }
}
