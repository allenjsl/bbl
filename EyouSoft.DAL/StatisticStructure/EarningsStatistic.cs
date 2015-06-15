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
    /// 利润统计数据访问
    /// </summary>
    /// 周文超 2011-01-24
    public class EarningsStatistic : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.StatisticStructure.IEarningsStatistic
    {
        private readonly Database _db = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EarningsStatistic()
        {
            _db = base.SystemStore;
        }

        #region IEarningsStatistic 成员

        /// <summary>
        /// 获取利润--区域统计
        /// </summary>
        /// <param name="model">利润统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.EarningsAreaStatistic> GetEarningsAreaStatistic(EyouSoft.Model.StatisticStructure.QueryEarningsStatistic model, string HaveUserIds)
        {
            IList<EyouSoft.Model.StatisticStructure.EarningsAreaStatistic> list = new List<EyouSoft.Model.StatisticStructure.EarningsAreaStatistic>();
            string strOrder = string.Empty;
            string strWhere = this.GetSqlWhere(model, HaveUserIds, ref strOrder);
            StringBuilder strSql = new StringBuilder(" select ");
            strSql.Append(" AreaId,AreaName, ");
            strSql.Append(" count(TourId) as TourCount, ");
            strSql.Append(" sum(TotalAllExpenses) as TotalAllExpenses, ");
            strSql.Append(" 0 as GrossProfit, ");
            strSql.Append(" sum(DistributionAmount) as DistributionAmount, ");
            strSql.AppendFormat(" (select Logistics from View_EarningsStatistic as tmpVes where tmpVes.AreaId = ves.AreaId {0} for xml auto,root('root')) as Logistics, ", strWhere.Replace("ves.", "tmpVes."));
            strSql.AppendFormat(" (select Orders from View_EarningsStatistic as tmpVes where tmpVes.AreaId = ves.AreaId and len(Orders) > 1 {0} for xml auto,root('root')) as Orders, ", strWhere.Replace("ves.", "tmpVes."));
            strSql.Append(" Sum(TotalOtherIncome) as TotalOtherIncome");
            strSql.Append(" from View_EarningsStatistic as ves ");
            strSql.AppendFormat(" where IsDelete = '0' ");
            strSql.AppendFormat(" {0} ", strWhere);
            strSql.Append(" group by AreaId,AreaName ");
            if (!string.IsNullOrEmpty(strOrder))
                strSql.AppendFormat(" order by {0} ", strOrder);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                EyouSoft.Model.StatisticStructure.EarningsAreaStatistic tmpModel = null;
                decimal FinanceSum = 0;
                int PeopleNumber = 0;
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.StatisticStructure.EarningsAreaStatistic();

                    if (!dr.IsDBNull(0))
                        tmpModel.AreaId = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        tmpModel.AreaName = dr.GetString(1);
                    if (!dr.IsDBNull(2))
                        tmpModel.TourNum = dr.GetInt32(2);
                    if (!dr.IsDBNull(3))
                        tmpModel.GrossOut = dr.GetDecimal(3);
                    if (!dr.IsDBNull(4))
                        tmpModel.TourGross = 0;//dr.GetDecimal(4);
                    if (!dr.IsDBNull(5))
                        tmpModel.TourShare = dr.GetDecimal(5);
                    if (!dr.IsDBNull(6))
                        tmpModel.Logistics = this.GetStatisticOperatorByXML(dr.GetString(6));

                    FinanceSum = 0;
                    PeopleNumber = 0;
                    IList<Model.StatisticStructure.StatisticOperator> tmp = null;
                    if (!dr.IsDBNull(7))
                        this.GetOrderInfoByXML(dr.GetString(7), model, ref FinanceSum, ref PeopleNumber, ref tmp, false);

                    tmpModel.GrossIncome = FinanceSum;  //此处只计算团款收入
                    tmpModel.TourPeopleNum = PeopleNumber;
                    if (!dr.IsDBNull(8))
                        tmpModel.GrossIncome += dr.GetDecimal(8); //此处加上杂费收入

                    //团队毛利=总收入-总支出
                    tmpModel.TourGross = tmpModel.GrossIncome - tmpModel.GrossOut;


                    list.Add(tmpModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取利润--部门统计
        /// </summary>
        /// <param name="model">利润统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.EarningsDepartStatistic> GetEarningsDepartStatistic(EyouSoft.Model.StatisticStructure.QueryEarningsStatistic model, string HaveUserIds)
        {
            IList<EyouSoft.Model.StatisticStructure.EarningsDepartStatistic> list = new List<EyouSoft.Model.StatisticStructure.EarningsDepartStatistic>();
            string strOrder = string.Empty;
            string strWhere = this.GetSqlWhere(model, HaveUserIds, ref strOrder);
            StringBuilder strSql = new StringBuilder(" select ");
            strSql.Append(" DepartId,");
            strSql.Append(" (select tbl_CompanyDepartment.DepartName from tbl_CompanyDepartment where tbl_CompanyDepartment.Id = DepartId) as DepartName, ");
            strSql.Append(" count(TourId) as TourCount, ");
            strSql.Append(" sum(TotalAllExpenses) as TotalAllExpenses, ");
            strSql.Append(" 0 as GrossProfit, ");
            strSql.Append(" sum(DistributionAmount) as DistributionAmount, ");
            strSql.AppendFormat(" (select Orders from View_EarningsStatistic as tmpVes where tmpVes.DepartId = ves.DepartId and len(Orders) > 1 {0} for xml auto,root('root')) as Orders, ", strWhere.Replace("ves.", "tmpVes."));
            strSql.Append(" Sum(TotalOtherIncome) as TotalOtherIncome");
            strSql.Append(" from View_EarningsStatistic as ves ");
            strSql.AppendFormat(" where IsDelete = '0' ");
            strSql.AppendFormat(" {0} ", strWhere);
            strSql.Append(" group by DepartId ");
            if (!string.IsNullOrEmpty(strOrder))
                strSql.AppendFormat(" order by {0} ", strOrder);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                EyouSoft.Model.StatisticStructure.EarningsDepartStatistic tmpModel = null;
                decimal FinanceSum = 0;
                int PeopleNumber = 0;
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.StatisticStructure.EarningsDepartStatistic();
                    if (!dr.IsDBNull(0))
                        tmpModel.DepartId = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        tmpModel.DepartName = dr.GetString(1);
                    if (!dr.IsDBNull(2))
                        tmpModel.TourNum = dr.GetInt32(2);
                    if (!dr.IsDBNull(3))
                        tmpModel.GrossOut = dr.GetDecimal(3);
                    if (!dr.IsDBNull(4))
                        tmpModel.TourGross = 0;//dr.GetDecimal(4);
                    if (!dr.IsDBNull(5))
                        tmpModel.TourShare = dr.GetDecimal(5);

                    FinanceSum = 0;
                    PeopleNumber = 0;
                    IList<Model.StatisticStructure.StatisticOperator> tmp = null;
                    if (!dr.IsDBNull(6))
                        this.GetOrderInfoByXML(dr.GetString(6), model, ref FinanceSum, ref PeopleNumber, ref tmp, true);

                    tmpModel.SalesClerk = tmp;
                    tmpModel.GrossIncome = FinanceSum;  //此处只计算团款收入
                    tmpModel.TourPeopleNum = PeopleNumber;
                    if (!dr.IsDBNull(7))
                        tmpModel.GrossIncome += dr.GetDecimal(7); //此处加上杂费收入

                    //团队毛利=总收入-总支出
                    tmpModel.TourGross = tmpModel.GrossIncome - tmpModel.GrossOut;

                    list.Add(tmpModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取利润--类型统计
        /// </summary>
        /// <param name="model">利润统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.EarningsTypeStatistic> GetEarningsTypeStatistic(EyouSoft.Model.StatisticStructure.QueryEarningsStatistic model, string HaveUserIds)
        {
            IList<EyouSoft.Model.StatisticStructure.EarningsTypeStatistic> list = new List<EyouSoft.Model.StatisticStructure.EarningsTypeStatistic>();
            string strOrder = string.Empty;
            string strWhere = this.GetSqlWhere(model, HaveUserIds, ref strOrder);
            StringBuilder strSql = new StringBuilder(" select ");
            strSql.Append(" TourType, ");
            strSql.Append(" count(TourId) as TourCount, ");
            strSql.Append(" sum(TotalAllExpenses) as TotalAllExpenses, ");
            strSql.Append(" 0 as GrossProfit, ");
            strSql.Append(" sum(DistributionAmount) as DistributionAmount, ");
            strSql.AppendFormat(" (select Orders from View_EarningsStatistic as tmpVes where tmpVes.TourType = ves.TourType and len(Orders) > 1 {0} for xml auto,root('root')) as Orders, ", strWhere.Replace("ves.", "tmpVes."));
            strSql.Append(" Sum(TotalOtherIncome) as TotalOtherIncome");
            strSql.Append(" from View_EarningsStatistic as ves ");
            strSql.AppendFormat(" where IsDelete = '0' ");
            strSql.AppendFormat(" {0} ", strWhere);
            strSql.Append(" group by TourType ");
            if (!string.IsNullOrEmpty(strOrder))
                strSql.AppendFormat(" order by {0} ", strOrder);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                EyouSoft.Model.StatisticStructure.EarningsTypeStatistic tmpModel = null;
                decimal FinanceSum = 0;
                int PeopleNumber = 0;
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.StatisticStructure.EarningsTypeStatistic();
                    if (!dr.IsDBNull(0))
                        tmpModel.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)dr.GetByte(0);
                    if (!dr.IsDBNull(1))
                        tmpModel.TourNum = dr.GetInt32(1);
                    if (!dr.IsDBNull(2))
                        tmpModel.GrossOut = dr.GetDecimal(2);
                    if (!dr.IsDBNull(3))
                        tmpModel.TourGross = 0;//dr.GetDecimal(3);
                    if (!dr.IsDBNull(4))
                        tmpModel.TourShare = dr.GetDecimal(4);

                    FinanceSum = 0;
                    PeopleNumber = 0;
                    IList<Model.StatisticStructure.StatisticOperator> tmp = null;
                    if (!dr.IsDBNull(5))
                        this.GetOrderInfoByXML(dr.GetString(5), model, ref FinanceSum, ref PeopleNumber, ref tmp, true);

                    tmpModel.SalesClerk = tmp;
                    tmpModel.GrossIncome = FinanceSum;  //此处只计算团款收入
                    tmpModel.TourPeopleNum = PeopleNumber;
                    if (!dr.IsDBNull(6))
                        tmpModel.GrossIncome += dr.GetDecimal(6); //此处加上杂费收入

                    //团队毛利=总收入-总支出
                    tmpModel.TourGross = tmpModel.GrossIncome - tmpModel.GrossOut;

                    list.Add(tmpModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取利润--时间统计
        /// </summary>
        /// <param name="model">利润统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.EarningsTimeStatistic> GetEarningsTimeStatistic(EyouSoft.Model.StatisticStructure.QueryEarningsStatistic model, string HaveUserIds)
        {
            IList<EyouSoft.Model.StatisticStructure.EarningsTimeStatistic> list = new List<EyouSoft.Model.StatisticStructure.EarningsTimeStatistic>();
            string strOrder = string.Empty;
            string strWhere = this.GetSqlWhere(model, HaveUserIds, ref strOrder);
            StringBuilder strSql = new StringBuilder(" select ");
            strSql.Append(" year(LeaveDate) as CurrYear,month(LeaveDate) as CurrMonth, ");
            strSql.Append(" count(TourId) as TourCount, ");
            strSql.Append(" sum(TotalAllExpenses) as TotalAllExpenses, ");
            strSql.Append(" 0 as GrossProfit, ");
            strSql.Append(" sum(DistributionAmount) as DistributionAmount, ");
            strSql.AppendFormat(" (select Orders from View_EarningsStatistic as tmpVes where year(tmpVes.LeaveDate) = year(ves.LeaveDate) and month(tmpVes.LeaveDate) = month(ves.LeaveDate) and len(Orders) > 1 {0} for xml auto,root('root')) as Orders, ", strWhere.Replace("ves.", "tmpVes."));
            strSql.Append(" Sum(TotalOtherIncome) as TotalOtherIncome");
            strSql.Append(" from View_EarningsStatistic as ves ");
            strSql.AppendFormat(" where IsDelete = '0' ");
            strSql.AppendFormat(" {0} ", strWhere);
            strSql.Append(" group by year(LeaveDate),month(LeaveDate) ");
            if (!string.IsNullOrEmpty(strOrder))
                strSql.AppendFormat(" order by {0} ", strOrder);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                EyouSoft.Model.StatisticStructure.EarningsTimeStatistic tmpModel = null;
                decimal FinanceSum = 0;
                int PeopleNumber = 0;
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.StatisticStructure.EarningsTimeStatistic();
                    if (!dr.IsDBNull(0))
                        tmpModel.CurrYear = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        tmpModel.CurrMonth = dr.GetInt32(1);
                    if (!dr.IsDBNull(2))
                        tmpModel.TourNum = dr.GetInt32(2);
                    if (!dr.IsDBNull(3))
                        tmpModel.GrossOut = dr.GetDecimal(3);
                    if (!dr.IsDBNull(4))
                        tmpModel.TourGross = 0;//dr.GetDecimal(4);
                    if (!dr.IsDBNull(5))
                        tmpModel.TourShare = dr.GetDecimal(5);

                    FinanceSum = 0;
                    PeopleNumber = 0;
                    IList<Model.StatisticStructure.StatisticOperator> tmp = null;
                    if (!dr.IsDBNull(6))
                        this.GetOrderInfoByXML(dr.GetString(6), model, ref FinanceSum, ref PeopleNumber, ref tmp, false);

                    tmpModel.GrossIncome = FinanceSum;  //此处只计算团款收入
                    tmpModel.TourPeopleNum = PeopleNumber;
                    if (!dr.IsDBNull(7))
                        tmpModel.GrossIncome += dr.GetDecimal(7); //此处加上杂费收入

                    //团队毛利=总收入-总支出
                    tmpModel.TourGross = tmpModel.GrossIncome - tmpModel.GrossOut;

                    list.Add(tmpModel);
                }
            }

            return list;
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
            throw new System.Exception("未实现。");
        }
        #endregion

        #region 私有函数

        /// <summary>
        /// 根据查询实体生成Where子句
        /// </summary>
        /// <param name="model">查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="strOrder">排序语句</param>
        /// <returns>Where子句</returns>
        private string GetSqlWhere(EyouSoft.Model.StatisticStructure.QueryEarningsStatistic model, string HaveUserIds
            , ref string strOrder)
        {
            if (model == null)
                return string.Empty;

            StringBuilder strSqlWhere = new StringBuilder();
            string strIds = string.Empty;
            if (model.CompanyId > 0)
                strSqlWhere.AppendFormat(" and ves.CompanyId = {0} ", model.CompanyId);
            if (model.TourType.HasValue)
                strSqlWhere.AppendFormat(" and ves.TourType = {0} ", (int)model.TourType.Value);
            if (model.DepartIds != null && model.DepartIds.Length > 0)
            {
                strIds = string.Empty;
                foreach (int i in model.DepartIds)
                {
                    if (i <= 0)
                        continue;

                    strIds += i.ToString() + ",";
                }
                strIds = strIds.Trim(',');

                if (!string.IsNullOrEmpty(strIds))
                    strSqlWhere.AppendFormat(" and ves.DepartId in ({0}) ", strIds);
            }
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
                    strSqlWhere.AppendFormat(" and ves.TourId in (select distinct tbl_TourOrder.TourId from tbl_TourOrder where SalerId in ({0})) ", strIds);
            }
            if (!string.IsNullOrEmpty(HaveUserIds))
                strSqlWhere.AppendFormat(" and ves.OperatorId in ({0}) ", HaveUserIds);
            if (model.LeaveDateStart.HasValue)
                strSqlWhere.AppendFormat(" and datediff(dd,'{0}',ves.LeaveDate) >= 0 ", model.LeaveDateStart.Value.ToShortDateString());
            if (model.LeaveDateEnd.HasValue)
                strSqlWhere.AppendFormat(" and datediff(dd,ves.LeaveDate,'{0}') >= 0 ", model.LeaveDateEnd.Value.ToShortDateString());
            if (model.CheckDateStart.HasValue)
                strSqlWhere.AppendFormat(" and datediff(dd,'{0}',ves.EndDateTime) >= 0 ", model.CheckDateStart.Value.ToShortDateString());
            if (model.CheckDateEnd.HasValue)
                strSqlWhere.AppendFormat(" and datediff(dd,ves.EndDateTime,'{0}') >= 0 ", model.CheckDateEnd.Value.ToShortDateString());
            if (model.AreaId > 0)
                strSqlWhere.AppendFormat(" and ves.AreaId = {0} ", model.AreaId);

            switch (model.OrderIndex)
            {
                case 0:
                    strOrder = " AreaId asc ";
                    break;
                case 1:
                    strOrder = " AreaId desc ";
                    break;
                case 2:
                    strOrder = " DepartId asc ";
                    break;
                case 3:
                    strOrder = " DepartId desc ";
                    break;
                case 4:
                    strOrder = " TourType asc ";
                    break;
                case 5:
                    strOrder = " TourType desc ";
                    break;
                case 6:
                    strOrder = " CurrYear asc,CurrMonth asc ";
                    break;
                case 7:
                    strOrder = " CurrYear desc,CurrMonth desc ";
                    break;
            }

            return strSqlWhere.ToString();
        }

        /// <summary>
        /// 根据XML获取计调员信息
        /// </summary>
        /// <param name="strXML"></param>
        /// <returns></returns>
        private IList<Model.StatisticStructure.StatisticOperator> GetStatisticOperatorByXML(string strXML)
        {
            if (string.IsNullOrEmpty(strXML))
                return null;

            XElement xRoot = XElement.Parse(strXML);
            var xRows = Utils.GetXElements(xRoot, "tmpVes");
            if (xRows == null || xRows.Count() <= 0)
                return null;

            IList<Model.StatisticStructure.StatisticOperator> tmpList = new List<Model.StatisticStructure.StatisticOperator>();
            Model.StatisticStructure.StatisticOperator tmpModel = null;
            foreach (var t in xRows)
            {
                string strTmpXML = Utils.GetXAttributeValue(t, "Logistics");
                if (string.IsNullOrEmpty(strTmpXML))
                    continue;

                var TmpXRows = Utils.GetXElements(XElement.Parse(strTmpXML), "row");
                if (TmpXRows == null || TmpXRows.Count() <= 0)
                    continue;

                foreach (var tmp in TmpXRows)
                {
                    tmpModel = new EyouSoft.Model.StatisticStructure.StatisticOperator();
                    tmpModel.OperatorId = Utils.GetInt(Utils.GetXAttributeValue(tmp, "Id"));
                    tmpModel.OperatorName = Utils.GetXAttributeValue(tmp, "ContactName");
                    tmpList.Add(tmpModel);
                }
            }

            return tmpList == null ? tmpList : tmpList.Distinct(new DistinctByStatisticOperator()).ToList();
        }

        /// <summary>
        /// 根据订单XML获取订单部分信息
        /// </summary>
        /// <param name="strOrderXML">订单XML</param>
        /// <param name="model">查询实体</param>
        /// <param name="TotalAllIncome">团队总收入</param>
        /// <param name="TourPeopleNum">团队人数</param>
        /// <param name="tmpList">销售员集合</param>
        /// <param name="IsIList">是否为集合赋值</param>
        private void GetOrderInfoByXML(string strOrderXML, EyouSoft.Model.StatisticStructure.QueryEarningsStatistic model
            , ref decimal TotalAllIncome, ref int TourPeopleNum
            , ref IList<Model.StatisticStructure.StatisticOperator> tmpList, bool IsIList)
        {
            if (string.IsNullOrEmpty(strOrderXML))
                return;

            XElement xRoot = XElement.Parse(strOrderXML);
            var xRows = Utils.GetXElements(xRoot, "tmpVes");
            if (xRows == null || xRows.Count() <= 0)
                return;

            TotalAllIncome = 0;
            TourPeopleNum = 0;
            if (tmpList == null && IsIList)
                tmpList = new List<Model.StatisticStructure.StatisticOperator>();

            Model.StatisticStructure.StatisticOperator tmpModel = null;
            foreach (var t in xRows)
            {
                string strTmpXML = Utils.GetXAttributeValue(t, "Orders");
                if (string.IsNullOrEmpty(strTmpXML))
                    continue;

                var TmpXRows = Utils.GetXElements(XElement.Parse(strTmpXML), "row");
                if (TmpXRows == null || TmpXRows.Count() <= 0)
                    continue;

                foreach (var tmp in TmpXRows)
                {
                    int SaleId = Utils.GetInt(Utils.GetXAttributeValue(tmp, "SalerId"));
                    int OrderState = Utils.GetInt(Utils.GetXAttributeValue(tmp, "OrderState"));
                    if (model != null)
                    {
                        //销售员查询
                        if (SaleId > 0 && model.SaleIds != null && model.SaleIds.Length > 0 && !model.SaleIds.Contains(SaleId))
                        {
                            continue;
                        }
                        //订单统计方式
                        if (model.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计有效订单)
                        {
                            if (OrderState == (int)Model.EnumType.TourStructure.OrderState.不受理 || OrderState == (int)Model.EnumType.TourStructure.OrderState.留位过期)
                                continue;
                        }
                        else if (model.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单 && OrderState != (int)Model.EnumType.TourStructure.OrderState.已成交)
                        {
                            continue;
                        }
                    }

                    TourPeopleNum += (Utils.GetInt(Utils.GetXAttributeValue(tmp, "PeopleNumber")) - Utils.GetInt(Utils.GetXAttributeValue(tmp, "LeaguePepoleNum")));
                    TotalAllIncome += Utils.GetDecimal(Utils.GetXAttributeValue(tmp, "FinanceSum"));

                    if (!IsIList)
                        continue;

                    tmpModel = new EyouSoft.Model.StatisticStructure.StatisticOperator();
                    tmpModel.OperatorId = Utils.GetInt(Utils.GetXAttributeValue(tmp, "SalerId"));
                    tmpModel.OperatorName = Utils.GetXAttributeValue(tmp, "SalerName");
                    tmpList.Add(tmpModel);
                }
            }

            if (tmpList != null)
                tmpList = tmpList.Distinct(new DistinctByStatisticOperator()).ToList();
        }

        #endregion
    }
}
