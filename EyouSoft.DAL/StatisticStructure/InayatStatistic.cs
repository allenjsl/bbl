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
    /// 人次统计数据访问
    /// </summary>
    /// 周文超 2011-01-21
    public class InayatStatistic : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.StatisticStructure.IInayatStatistic
    {
        private readonly Database _db = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public InayatStatistic()
        {
            _db = base.SystemStore;
        }

        #region IInayatStatistic 成员

        /// <summary>
        /// 获取人次-区域统计
        /// </summary>
        /// <param name="model">人次统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.InayaAreatStatistic> GetInayaAreatStatistic(EyouSoft.Model.StatisticStructure.QueryInayatStatistic model, string HaveUserIds)
        {
            IList<EyouSoft.Model.StatisticStructure.InayaAreatStatistic> list = new List<EyouSoft.Model.StatisticStructure.InayaAreatStatistic>();
            string strOrder = string.Empty;
            string View_InayatStatisticWhere = this.GetSqlWhere(model, HaveUserIds, ref strOrder);

            StringBuilder strSql = new StringBuilder(" select ");
            strSql.Append(" AreaId,");
            strSql.Append(" sum(PeopleCount) as PeopleCount,");
            strSql.Append(" sum(PeopleDays) as PeopleDays,");
            strSql.Append(" (select AreaName from tbl_Area where Id = AreaId) as AreaName,");
            strSql.Append(" (select SalerId,SalerName from View_InayatStatistic as tmpTro where tmpTro.AreaId = tro.AreaId for xml auto,root('root')) as SalerInfo,");
            strSql.Append(" (select Id,ContactName from tbl_CompanyUser where id in (select distinct OperatorId from tbl_TourOperator where TourId in (select distinct TourId from tbl_Tour where tbl_Tour.AreaId = tro.AreaId)) and tbl_CompanyUser.isdelete = '0' for xml auto,root('root')) as Logistics ");
            strSql.AppendFormat(" ,(SELECT COUNT(*) FROM tbl_Tour AS A WHERE A.TourId IN(SELECT TourId FROM View_InayatStatistic AS B WHERE B.IsDelete = '0' AND B.TourClassId in ({0},{1})  {2} AND B.AreaId=tro.AreaId)) AS TuanDunShu ", (int)EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划, (int)EyouSoft.Model.EnumType.TourStructure.TourType.团队计划, View_InayatStatisticWhere);
            strSql.Append(" from View_InayatStatistic as tro ");
            strSql.AppendFormat(" where IsDelete = '0' and TourClassId in ({0},{1}) ", (int)EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划, (int)EyouSoft.Model.EnumType.TourStructure.TourType.团队计划);
            strSql.AppendFormat(" {0} ", View_InayatStatisticWhere);
            strSql.Append(" group by tro.AreaId ");
            if (!string.IsNullOrEmpty(strOrder))
                strSql.AppendFormat(" order by {0} ", strOrder);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                EyouSoft.Model.StatisticStructure.InayaAreatStatistic tmpModel = null;
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.StatisticStructure.InayaAreatStatistic();
                    if (!dr.IsDBNull(0))
                        tmpModel.AreaId = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        tmpModel.PeopleCount = dr.GetInt32(1);
                    if (!dr.IsDBNull(2))
                        tmpModel.PeopleDays = dr.GetDecimal(2);
                    if (!dr.IsDBNull(3))
                        tmpModel.AreaName = dr.GetString(3);
                    tmpModel.SalesClerk = this.GetStatisticOperator(dr["SalerInfo"].ToString(), "tmpTro", "SalerId", "SalerName");
                    tmpModel.Logistics = this.GetStatisticOperator(dr["Logistics"].ToString(), "tbl_CompanyUser", "Id", "ContactName");
                    tmpModel.TuanDuiShu = dr.GetInt32(6);

                    list.Add(tmpModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取人次-部门统计
        /// </summary>
        /// <param name="model">人次统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.InayaDepartStatistic> GetInayaDepartStatistic(EyouSoft.Model.StatisticStructure.QueryInayatStatistic model, string HaveUserIds)
        {
            IList<EyouSoft.Model.StatisticStructure.InayaDepartStatistic> list = new List<EyouSoft.Model.StatisticStructure.InayaDepartStatistic>();
            string strOrder = string.Empty;
            string strWhere = this.GetSqlWhere(model, HaveUserIds, ref strOrder);            

            StringBuilder strSql = new StringBuilder(" select ");
            strSql.Append(" DepartId, ");
            strSql.Append(" (select tbl_CompanyDepartment.DepartName from tbl_CompanyDepartment where tbl_CompanyDepartment.Id = DepartId) as DepartName, ");
            strSql.Append(" sum(PeopleCount) as PeopleCount, ");
            strSql.Append(" sum(PeopleDays) as PeopleDays, ");
            strSql.AppendFormat(" (select SalerId,SalerName from View_InayatStatistic as tmpView where tmpView.DepartId = vids.DepartId {0} for xml auto,root('root')) as SalerInfo ", strWhere);
            strSql.AppendFormat(" ,(SELECT COUNT(*) FROM tbl_Tour AS A WHERE A.TourId IN(SELECT TourId FROM View_InayatStatistic AS B WHERE B.IsDelete = '0' AND B.TourClassId in ({0},{1})  {2} AND B.DepartId=vids.DepartId)) AS TuanDunShu ", (int)EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划, (int)EyouSoft.Model.EnumType.TourStructure.TourType.团队计划, strWhere);
            strSql.Append(" from View_InayatStatistic as vids ");
            strSql.AppendFormat(" where IsDelete = '0' and TourClassId in ({0},{1}) ", (int)EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划, (int)EyouSoft.Model.EnumType.TourStructure.TourType.团队计划);
            strSql.AppendFormat(" {0} ", strWhere);
            strSql.Append(" group by DepartId ");
            if (!string.IsNullOrEmpty(strOrder))
                strSql.AppendFormat(" order by {0} ", strOrder);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                EyouSoft.Model.StatisticStructure.InayaDepartStatistic tmpModel = null;
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.StatisticStructure.InayaDepartStatistic();
                    if (!dr.IsDBNull(0))
                        tmpModel.DepartId = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        tmpModel.DepartName = dr.GetString(1);
                    if (!dr.IsDBNull(2))
                        tmpModel.PeopleCount = dr.GetInt32(2);
                    if (!dr.IsDBNull(3))
                        tmpModel.PeopleDays = dr.GetDecimal(3);

                    tmpModel.SalesClerk = this.GetStatisticOperator(dr["SalerInfo"].ToString(), "tmpView", "SalerId", "SalerName");

                    tmpModel.TuanDuiShu = dr.GetInt32(5);

                    list.Add(tmpModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取人次-时间统计
        /// </summary>
        /// <param name="model">人次统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.InayaTimeStatistic> GetInayaTimeStatistic(EyouSoft.Model.StatisticStructure.QueryInayatStatistic model, string HaveUserIds)
        {
            IList<EyouSoft.Model.StatisticStructure.InayaTimeStatistic> list = new List<EyouSoft.Model.StatisticStructure.InayaTimeStatistic>();
            string strOrder = string.Empty;
            string strWhere=this.GetSqlWhere(model, HaveUserIds, ref strOrder);
            StringBuilder strSql = new StringBuilder(" select ");
            strSql.Append(" month(LeaveDate) as CurrMonth, ");
            strSql.Append(" sum(PeopleCount) as PeopleCount, ");
            strSql.Append(" sum(PeopleDays) as PeopleDays");
            strSql.Append(" ,year(LeaveDate) as CurrYear ");
            strSql.AppendFormat(" ,(SELECT COUNT(*) FROM tbl_Tour AS A WHERE A.TourId IN(SELECT TourId FROM View_InayatStatistic AS B WHERE B.IsDelete = '0' AND B.TourClassId in ({0},{1})  {2} AND Year(B.LeaveDate)=YEAR(vids.LeaveDate) AND MONTH(B.LeaveDate)=MONTH(vids.LeaveDate) )) AS TuanDunShu ", (int)EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划, (int)EyouSoft.Model.EnumType.TourStructure.TourType.团队计划, strWhere);
            strSql.Append(" from View_InayatStatistic as vids ");
            strSql.AppendFormat(" where IsDelete = '0' and TourClassId in ({0},{1}) ", (int)EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划, (int)EyouSoft.Model.EnumType.TourStructure.TourType.团队计划);
            strSql.AppendFormat(" {0} ", strWhere);
            strSql.Append(" group by year(LeaveDate),month(LeaveDate) ");
            if (!string.IsNullOrEmpty(strOrder))
                strSql.AppendFormat(" order by {0} ", strOrder);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                EyouSoft.Model.StatisticStructure.InayaTimeStatistic tmpModel = null;
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.StatisticStructure.InayaTimeStatistic();
                    if (!dr.IsDBNull(0))
                        tmpModel.CurrMonth = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        tmpModel.PeopleCount = dr.GetInt32(1);
                    if (!dr.IsDBNull(2))
                        tmpModel.PeopleDays = dr.GetDecimal(2);
                    if (!dr.IsDBNull(3))
                        tmpModel.CurrYear = dr.GetInt32(3);

                    tmpModel.TuanDuiShu = dr.GetInt32(4);

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
        /// <returns>Where子句</returns>
        private string GetSqlWhere(EyouSoft.Model.StatisticStructure.QueryInayatStatistic model, string HaveUserIds
            , ref string strOrder)
        {
            if (model == null)
                return string.Empty;

            StringBuilder strSqlWhere = new StringBuilder();
            if (model.CompanyId > 0)
                strSqlWhere.AppendFormat(" and SellCompanyId = {0} ", model.CompanyId);
            if (model.AreaId > 0)
                strSqlWhere.AppendFormat(" and AreaId = {0} ", model.AreaId);

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
                strSqlWhere.AppendFormat(" and TourOperatorId in ({0}) ", HaveUserIds);
            if (model.StartTime.HasValue)
                strSqlWhere.AppendFormat(" and datediff(dd,'{0}',IssueTime) >= 0 ", model.StartTime.Value.ToShortDateString());
            if (model.EndTime.HasValue)
                strSqlWhere.AppendFormat(" and datediff(dd,IssueTime,'{0}') >= 0 ", model.EndTime.Value.ToShortDateString());

            if (model.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计有效订单)
                strSqlWhere.AppendFormat(" and OrderState in ({0},{1},{2}) ", (int)Model.EnumType.TourStructure.OrderState.未处理, (int)Model.EnumType.TourStructure.OrderState.已成交, (int)Model.EnumType.TourStructure.OrderState.已留位);
            else if (model.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单)
                strSqlWhere.AppendFormat(" and OrderState = {0} ", (int)Model.EnumType.TourStructure.OrderState.已成交);

            if (model.LeaveDateStart.HasValue)
                strSqlWhere.AppendFormat(" and datediff(dd,'{0}',LeaveDate) >= 0 ", model.LeaveDateStart.Value.ToShortDateString());
            if (model.LeaveDateEnd.HasValue)
                strSqlWhere.AppendFormat(" and datediff(dd,LeaveDate,'{0}') >= 0 ", model.LeaveDateEnd.Value.ToShortDateString());

            if (model.TourType.HasValue)
            {
                strSqlWhere.AppendFormat(" AND TourClassId={0} ", (int)model.TourType.Value);
            }


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
                    strOrder = " CurrMonth asc ";
                    break;
                case 5:
                    strOrder = " CurrMonth desc ";
                    break;
            }

            return strSqlWhere.ToString();
        }

        /// <summary>
        /// 解析SqlXML获取人员信息
        /// </summary>
        /// <param name="strXML">SqlXML</param>
        /// <param name="strTableName">表名称</param>
        /// <param name="strIdValue">Id字段名称</param>
        /// <param name="strNameValue">Name字段名称</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.StatisticOperator> GetStatisticOperator(string strXML, string strTableName, string strIdValue, string strNameValue)
        {
            if (string.IsNullOrEmpty(strXML) || string.IsNullOrEmpty(strTableName) || string.IsNullOrEmpty(strIdValue) || string.IsNullOrEmpty(strNameValue))
                return null;

            System.Xml.XmlAttributeCollection attList = null;
            System.Xml.XmlDocument xml = null;
            System.Xml.XmlNodeList xmlNodeList = null;
            xml = new System.Xml.XmlDocument();
            xml.LoadXml(strXML);
            xmlNodeList = xml.GetElementsByTagName(strTableName);
            if (xmlNodeList == null || xmlNodeList.Count <= 0)
                return null;

            List<Model.StatisticStructure.StatisticOperator> list = new List<Model.StatisticStructure.StatisticOperator>();
            Model.StatisticStructure.StatisticOperator model = null;
            foreach (System.Xml.XmlNode node in xmlNodeList)
            {
                attList = node.Attributes;
                if (attList != null && attList.Count > 0)
                {
                    model = new EyouSoft.Model.StatisticStructure.StatisticOperator();
                    if (attList[strIdValue] != null && !string.IsNullOrEmpty(attList[strIdValue].Value))
                        model.OperatorId = int.Parse(attList[strIdValue].Value);
                    if (attList[strNameValue] != null)
                        model.OperatorName = attList[strNameValue].Value;

                    list.Add(model);
                }
            }
            if (list.Count > 0)
                list = list.Distinct(new DistinctByStatisticOperator()).ToList();

            xml = null;
            attList = null;
            xmlNodeList = null;

            return list;
        }

        #endregion
    }

    /// <summary>
    /// 人员信息去重复类
    /// </summary>
    public class DistinctByStatisticOperator : IEqualityComparer<Model.StatisticStructure.StatisticOperator>
    {
        public bool Equals(Model.StatisticStructure.StatisticOperator x, Model.StatisticStructure.StatisticOperator y)
        {
            if (x.OperatorId == y.OperatorId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(Model.StatisticStructure.StatisticOperator obj)
        {
            return 0;
        }
    }
}
