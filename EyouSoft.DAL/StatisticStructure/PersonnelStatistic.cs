using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data;

namespace EyouSoft.DAL.StatisticStructure
{
    /// <summary>
    /// 员工业绩统计数据访问
    /// </summary>
    /// 周文超 2011-01-24
    public class PersonnelStatistic : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.StatisticStructure.IPersonnelStatistic
    {
        private readonly Database _db = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PersonnelStatistic()
        {
            _db = base.SystemStore;
        }

        #region IPersonnelStatistic 成员

        /// <summary>
        /// 获取员工业绩-收入统计
        /// </summary>
        /// <param name="model">员工业绩统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic> GetPersonnelIncomeStatistic(EyouSoft.Model.StatisticStructure.QueryPersonnelStatistic model, string HaveUserIds)
        {
            IList<EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic> list = new List<EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic>();
            string strOrder = string.Empty;
            string strTourWhere = string.Empty;
            StringBuilder strSql = new StringBuilder(" select ");
            strSql.Append(" SalerId, ");
            strSql.Append(" (select ContactName from tbl_CompanyUser where tbl_CompanyUser.id = tro.SalerId and tbl_CompanyUser.isdelete = '0') as SalerName, ");
            strSql.Append(" sum(PeopleNumber - LeaguePepoleNum) as PeopleCount, ");
            strSql.Append(" sum(FinanceSum) as FinanceSum, ");
            strSql.Append(" (select DepartId,DepartName from tbl_CompanyUser where Id = SalerId and isdelete = '0' for xml auto,root('root')) as SalerInfo, ");
            strSql.Append(" (select Id,ContactName from tbl_CompanyUser where id in (select distinct OperatorId from tbl_TourOperator where TourId in (select distinct TourId from tbl_TourOrder as tmpTro where tmpTro.salerId = tro.salerId and tmpTro.isdelete = '0')) and tbl_CompanyUser.isdelete = '0' for xml auto,root('root')) as Logistics ");
            strSql.Append(" from tbl_TourOrder as tro ");
            strSql.AppendFormat(" where IsDelete = '0' and SalerId > 0 and exists (select 1 from tbl_CompanyUser where tbl_CompanyUser.isdelete = '0' and tbl_CompanyUser.Id = SalerId) ");
            strSql.AppendFormat(" {0} ", this.GetSqlWhere(model, HaveUserIds, ref strOrder, ref strTourWhere));
            strSql.Append(" group by SalerId ");
            if (!string.IsNullOrEmpty(strOrder))
                strSql.AppendFormat(" order by {0} ", strOrder);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic tmpModel = null;
                System.Xml.XmlAttributeCollection attList = null;
                System.Xml.XmlDocument xml = null;
                System.Xml.XmlNodeList xmlNodeList = null;
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.StatisticStructure.PersonnelIncomeStatistic();
                    if (!dr.IsDBNull(0))
                    {
                        tmpModel.SalesClerk = new EyouSoft.Model.StatisticStructure.StatisticOperator();
                        tmpModel.SalesClerk.OperatorId = dr.GetInt32(0);
                        if (!dr.IsDBNull(1))
                            tmpModel.SalesClerk.OperatorName = dr.GetString(1);
                    }
                    if (!dr.IsDBNull(2))
                        tmpModel.PeopleNum = dr.GetInt32(2);
                    if (!dr.IsDBNull(3))
                        tmpModel.Income = dr.GetDecimal(3);
                    if (!dr.IsDBNull(4))
                    {
                        xml = new System.Xml.XmlDocument();
                        xml.LoadXml(dr.GetString(4));
                        xmlNodeList = xml.GetElementsByTagName("tbl_CompanyUser");
                        if (xmlNodeList != null && xmlNodeList.Count > 0)
                        {
                            attList = xmlNodeList[0].Attributes;
                            if (attList["DepartId"] != null)
                                tmpModel.DepartId = int.Parse(attList["DepartId"].Value);
                            if (attList["DepartName"] != null)
                                tmpModel.DepartName = attList["DepartName"].Value;
                        }
                    }
                    tmpModel.Logistics = new InayatStatistic().GetStatisticOperator(dr["Logistics"].ToString(), "tbl_CompanyUser", "Id", "ContactName");

                    list.Add(tmpModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取员工业绩-利润统计
        /// </summary>
        /// <param name="model">员工业绩统计查询实体</param>
        /// <param name="haveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.PersonnelProfitStatistic> GetPersonnelProfitStatistic(EyouSoft.Model.StatisticStructure.QueryPersonnelStatistic model, string haveUserIds)
        {
            IList<EyouSoft.Model.StatisticStructure.PersonnelProfitStatistic> list = new List<EyouSoft.Model.StatisticStructure.PersonnelProfitStatistic>();
            string strOrder = string.Empty;
            string strTourWhere = string.Empty;
            string strWhere = this.GetSqlWhere(model, haveUserIds, ref strOrder, ref strTourWhere);
            StringBuilder strSql = new StringBuilder(" select ");
            strSql.Append(" SalerId, ");
            strSql.Append(" (select ContactName from tbl_CompanyUser where tbl_CompanyUser.id = tro.SalerId and tbl_CompanyUser.isdelete = '0') as SalerName, ");
            strSql.Append(" sum(PeopleNumber - LeaguePepoleNum) as PeopleCount, ");
            strSql.Append(" sum(FinanceSum) as FinanceSum, ");
            strSql.Append(" (select DepartId,DepartName from tbl_CompanyUser where Id = SalerId  and isdelete = '0' for xml auto,root('root')) as SalerInfo, ");
            strSql.Append(" (select Id,ContactName from tbl_CompanyUser where id in (select distinct OperatorId from tbl_TourOperator where TourId in (select distinct TourId from tbl_TourOrder as tmpTro where tmpTro.salerId = tro.salerId and tmpTro.isdelete = '0')) and tbl_CompanyUser.isdelete = '0' for xml auto,root('root')) as Logistics, ");
            strSql.AppendFormat(" (select sum(TotalExpenses) from tbl_Tour where tbl_Tour.TourId in (select distinct TourId from tbl_TourOrder as tmpTro where tmpTro.salerId = tro.salerId and tmpTro.isdelete = '0') and tbl_Tour.isdelete = '0' {0} ) as TotalExpenses, ", strTourWhere);
            strSql.AppendFormat(" (select sum(DistributionAmount) from tbl_Tour where tbl_Tour.TourId in (select distinct TourId from tbl_TourOrder as tmpTro where tmpTro.salerId = tro.salerId and tmpTro.isdelete = '0') and tbl_Tour.isdelete = '0' {0} ) as DistributionAmount ", strTourWhere);
            strSql.Append(" from tbl_TourOrder as tro ");
            strSql.AppendFormat(" where IsDelete = '0' and SalerId > 0 and exists (select 1 from tbl_CompanyUser where tbl_CompanyUser.isdelete = '0' and tbl_CompanyUser.Id = SalerId) ");
            strSql.AppendFormat(" {0} ", strWhere);
            strSql.Append(" group by SalerId ");
            if (!string.IsNullOrEmpty(strOrder))
                strSql.AppendFormat(" order by {0} ", strOrder);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                System.Xml.XmlAttributeCollection attList = null;
                System.Xml.XmlDocument xml = null;
                System.Xml.XmlNodeList xmlNodeList = null;
                EyouSoft.Model.StatisticStructure.PersonnelProfitStatistic tmpModel = null;
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.StatisticStructure.PersonnelProfitStatistic();
                    if (!dr.IsDBNull(0))
                    {
                        tmpModel.SalesClerk = new EyouSoft.Model.StatisticStructure.StatisticOperator();
                        tmpModel.SalesClerk.OperatorId = dr.GetInt32(0);
                        if (!dr.IsDBNull(1))
                            tmpModel.SalesClerk.OperatorName = dr.GetString(1);
                    }
                    if (!dr.IsDBNull(2))
                        tmpModel.PeopleNum = dr.GetInt32(2);
                    if (!dr.IsDBNull(3))
                        tmpModel.Income = dr.GetDecimal(3);
                    if (!dr.IsDBNull(4))
                    {
                        xml = new System.Xml.XmlDocument();
                        xml.LoadXml(dr.GetString(4));
                        xmlNodeList = xml.GetElementsByTagName("tbl_CompanyUser");
                        if (xmlNodeList != null && xmlNodeList.Count > 0)
                        {
                            attList = xmlNodeList[0].Attributes;
                            if (attList["DepartId"] != null)
                                tmpModel.DepartId = int.Parse(attList["DepartId"].Value);
                            if (attList["DepartName"] != null)
                                tmpModel.DepartName = attList["DepartName"].Value;
                        }
                    }
                    tmpModel.Logistics = new InayatStatistic().GetStatisticOperator(dr["Logistics"].ToString(), "tbl_CompanyUser", "Id", "ContactName");
                    if (!dr.IsDBNull(6))
                        tmpModel.OutMoney = dr.GetDecimal(6);
                    if (!dr.IsDBNull(7))
                        tmpModel.ShareMoney = dr.GetDecimal(7);

                    list.Add(tmpModel);
                }
            }

            return list;
        }

        #endregion

        #region 私有函数

        /// <summary>
        /// 根据查询实体生成Where子句
        /// </summary>
        /// <param name="model">查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="strOrder">排序语句</param>
        /// <param name="strTourWhere">团队SqlWhere</param>
        /// <returns>Where子句</returns>
        private string GetSqlWhere(EyouSoft.Model.StatisticStructure.QueryPersonnelStatistic model, string HaveUserIds
            , ref string strOrder, ref string strTourWhere)
        {
            if (model == null)
                return string.Empty;

            StringBuilder strSqlWhere = new StringBuilder();
            StringBuilder strTmpTourWhere = new StringBuilder();
            if (model.CompanyId > 0)
            {
                strSqlWhere.AppendFormat(" and SellCompanyId = {0} ", model.CompanyId);
                strTmpTourWhere.AppendFormat(" and CompanyId = {0} ", model.CompanyId);
            }
            if (model.LeaveDateStart.HasValue)
            {
                strSqlWhere.AppendFormat(" and datediff(dd,'{0}',LeaveDate) >= 0 ", model.LeaveDateStart.Value.ToShortDateString());
                strTmpTourWhere.AppendFormat(" and datediff(dd,'{0}',LeaveDate) >= 0 ", model.LeaveDateStart.Value.ToShortDateString());
            }
            if (model.LeaveDateEnd.HasValue)
            {
                strSqlWhere.AppendFormat(" and datediff(dd,LeaveDate,'{0}') >= 0 ", model.LeaveDateEnd.Value.ToShortDateString());
                strTmpTourWhere.AppendFormat(" and datediff(dd,LeaveDate,'{0}') >= 0 ", model.LeaveDateEnd.Value.ToShortDateString());
            }
            if (model.CheckDateStart.HasValue)
                strSqlWhere.AppendFormat(" and datediff(dd,'{0}',IssueTime) >= 0 ", model.CheckDateStart.Value.ToShortDateString());
            if (model.CheckDateEnd.HasValue)
                strSqlWhere.AppendFormat(" and datediff(dd,IssueTime,'{0}') >= 0 ", model.CheckDateEnd.Value.ToShortDateString());

            IList<int> SaleIds = new List<int>();
            if (model.SaleIds != null && model.SaleIds.Length > 0)
            {
                foreach (int i in model.SaleIds)
                {
                    if (i <= 0)
                        continue;

                    if (SaleIds.Contains(i))
                        continue;

                    SaleIds.Add(i);
                }
            }
            if (model.DepartIds != null && model.DepartIds.Length > 0)
            {
                int[] UserIds = new DAL.CompanyStructure.CompanyUser().GetUserIdsByDepartIds(model.DepartIds);
                if (UserIds != null && UserIds.Length > 0)
                {
                    foreach (int i in UserIds)
                    {
                        if (i <= 0)
                            continue;

                        if (SaleIds.Contains(i))
                            continue;

                        SaleIds.Add(i);
                    }
                }
            }

            if (SaleIds != null && SaleIds.Count > 0)
            {
                string strIds = string.Empty;
                foreach (int i in SaleIds)
                {
                    if (i <= 0)
                        continue;

                    strIds += i.ToString() + ",";
                }
                strIds = strIds.Trim(',');
                if (!string.IsNullOrEmpty(strIds))
                    strSqlWhere.AppendFormat(" and SalerId in ({0}) ", strIds);
            }
            //if (!string.IsNullOrEmpty(HaveUserIds))
            //    strSqlWhere.AppendFormat(" and ViewOperatorId in ({0}) ", HaveUserIds);
            if (!string.IsNullOrEmpty(HaveUserIds))
            {
                strSqlWhere.AppendFormat(
                    " and exists (select 1 from tbl_Tour where tbl_Tour.TourId = tro.TourId and tbl_Tour.OperatorId in ({0})) ",
                    HaveUserIds);

                strTmpTourWhere.AppendFormat(" and OperatorId in ({0}) ", HaveUserIds);
            }
            if (model.LogisticsIds != null && model.LogisticsIds.Length > 0)
            {
                string strIds = string.Empty;
                foreach (int i in model.LogisticsIds)
                {
                    strIds += i.ToString() + ",";
                }
                strIds = strIds.Trim(',');
                if (!string.IsNullOrEmpty(strIds))
                    strSqlWhere.AppendFormat(" and TourId in (select TourId from tbl_TourOperator where OperatorId in ({0})) ", strIds);

                strTmpTourWhere.AppendFormat(" and TourId in (select TourId from tbl_TourOperator where OperatorId in ({0})) ", strIds);
            }

            if (model.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计有效订单)
                strSqlWhere.AppendFormat(" and OrderState in ({0},{1},{2}) ", (int)Model.EnumType.TourStructure.OrderState.未处理, (int)Model.EnumType.TourStructure.OrderState.已成交, (int)Model.EnumType.TourStructure.OrderState.已留位);
            else if (model.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单)
                strSqlWhere.AppendFormat(" and OrderState = {0} ", (int)Model.EnumType.TourStructure.OrderState.已成交);

            switch (model.OrderIndex)
            {
                case 0:
                    strOrder = " SalerId asc ";
                    break;
                case 1:
                    strOrder = " SalerId desc ";
                    break;
            }

            strTourWhere = strTmpTourWhere.ToString();

            return strSqlWhere.ToString();
        }

        #endregion
    }
}
