/************************************************************
 * 模块名称：客户关系管理-销售统计数据实现
 * 功能说明：客户关系管理-销售统计数据实现
 * 创建人：周文超  2011-4-18 16:34:45
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
    /// 客户关系管理-销售统计数据实现
    /// </summary>
    public class DSalesStatistics : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.StatisticStructure.ISalesStatistics
    {
        private readonly Database _db = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DSalesStatistics()
        {
            _db = base.SystemStore;
        }

        #region ISalesStatistics 成员

        /// <summary>
        /// 获取销售统计列表
        /// </summary>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="QueryModel">查询实体</param>
        /// <param name="QueryModel">查询实体</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.MSalesStatistics> GetSalesStatistics(int PageSize, int PageIndex
            , ref int RecordCount, EyouSoft.Model.StatisticStructure.MQuerySalesStatistics QueryModel,string us)
        {
            if (QueryModel == null || QueryModel.CompanyId <= 0)
                return null;

            IList<EyouSoft.Model.StatisticStructure.MSalesStatistics> list = null;
            EyouSoft.Model.StatisticStructure.MSalesStatistics model = null;
            string strFile = " * ";
            string strPK = "ID";
            string strTableName = "View_SalesStatistics";
            string strOrderBy = " LeaveDate asc ";
            string strWhere = this.GetSqlWhere(QueryModel, us);
            switch (QueryModel.OrderIndex)
            {
                case 0:
                    strOrderBy = " LeaveDate asc ";
                    break;
                case 1:
                    strOrderBy = " LeaveDate desc ";
                    break;
            }

            using (IDataReader dr = DbHelper.ExecuteReader(this._db, PageSize, PageIndex, ref RecordCount, strTableName, strPK, strFile, strWhere, strOrderBy))
            {
                list = new List<EyouSoft.Model.StatisticStructure.MSalesStatistics>();

                while (dr.Read())
                {
                    model = new EyouSoft.Model.StatisticStructure.MSalesStatistics();

                    if (!dr.IsDBNull(dr.GetOrdinal("ID")))
                        model.OrderId = dr.GetString(dr.GetOrdinal("ID"));
                    if (!dr.IsDBNull(dr.GetOrdinal("LeaveDate")))
                        model.LeaveDate = dr.GetDateTime(dr.GetOrdinal("LeaveDate"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TourClassId")) && !dr.IsDBNull(dr.GetOrdinal("AreaName")))
                    {
                        if ((Model.EnumType.TourStructure.TourType)dr.GetByte(dr.GetOrdinal("TourClassId")) == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务)
                        {

                            model.AreaName = this.GetPlanSingleAreaName(dr.GetString(dr.GetOrdinal("AreaName")));
                        }
                        else
                            model.AreaName = dr.GetString(dr.GetOrdinal("AreaName"));
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("RouteName")))
                        model.RouteName = dr.GetString(dr.GetOrdinal("RouteName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Logistics")))
                        model.Logistics = this.GetOperatorListByXML(dr.GetString(dr.GetOrdinal("Logistics")), "OperatorId", "OperatorName");
                    if (!dr.IsDBNull(dr.GetOrdinal("BuyCompanyName")))
                        model.CustomerName = dr.GetString(dr.GetOrdinal("BuyCompanyName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("FinanceSum")))
                        model.TotalAmount = dr.GetDecimal(dr.GetOrdinal("FinanceSum"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PeopleNumber")))
                        model.PeopleNum = dr.GetInt32(dr.GetOrdinal("PeopleNumber"));
                    //if (!dr.IsDBNull(dr.GetOrdinal("OperatorName")))
                    //    model.OperatorName = dr.GetString(dr.GetOrdinal("OperatorName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("BuyerContactName")))
                        model.OperatorName = dr.GetString(dr.GetOrdinal("BuyerContactName"));
                    //if (!dr.IsDBNull(dr.GetOrdinal("DepartName")))
                    //    model.DepartName = dr.GetString(dr.GetOrdinal("DepartName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ProvinceAndCity")))
                        model.ProvinceAndCity = dr.GetString(dr.GetOrdinal("ProvinceAndCity"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SalerId")))
                        model.SalerId = dr.GetInt32(dr.GetOrdinal("SalerId"));
                    //if (!dr.IsDBNull(dr.GetOrdinal("SalerName")))
                    //    model.SalerName = dr.GetString(dr.GetOrdinal("SalerName"));

                    string xml = dr["PerTimeSeller"].ToString();
                    if (!string.IsNullOrEmpty(xml))
                    {
                        XElement xroot = XElement.Parse(xml);
                        var xrow = Utils.GetXElement(xroot, "row");
                        model.SalerName = Utils.GetXAttributeValue(xrow, "ContactName");
                        model.DepartName = Utils.GetXAttributeValue(xrow, "DepartName");
                    }

                    model.ZeRenSelllerName = dr["SalerName"].ToString();
                    model.KeHuDanWeiId = dr.GetInt32(dr.GetOrdinal("BuyCompanyId"));

                    list.Add(model);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取销售统计的汇总信息
        /// </summary>
        /// <param name="AllPeopleNum">总人数</param>
        /// <param name="AllFinanceSum">总金额</param>
        /// <param name="QueryModel">查询实体</param>
        /// <param name="us">用户信息集合</param>
        public void GetSumSalesStatistics(ref int AllPeopleNum, ref decimal AllFinanceSum
            , Model.StatisticStructure.MQuerySalesStatistics QueryModel,string us)
        {
            if (QueryModel == null || QueryModel.CompanyId <= 0)
                return ;

            string strSql = " select sum(PeopleNumber) as PeopleNumber,sum(FinanceSum) as FinanceSum from [View_SalesStatistics] ";
            string strWhere = this.GetSqlWhere(QueryModel, us);
            if (!string.IsNullOrEmpty(strWhere))
                strSql += " where 1 = 1 and " + strWhere;

            DbCommand dc = this._db.GetSqlStringCommand(strSql);

            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                if (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                        AllPeopleNum = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        AllFinanceSum = dr.GetDecimal(1);
                }
            }
        }

        #endregion

        #region 私有函数

        /// <summary>
        /// 根据查询实体生成SqlWhere语句
        /// </summary>
        /// <param name="QueryModel">查询实体</param>
        /// <param name="us">用户信息集合</param>
        /// <returns>SqlWhere语句</returns>
        private string GetSqlWhere(EyouSoft.Model.StatisticStructure.MQuerySalesStatistics QueryModel,string us)
        {
            if (QueryModel == null || QueryModel.CompanyId <= 0)
                return string.Empty;

            string strIds = string.Empty;
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" SellCompanyId = {0} ", QueryModel.CompanyId);

            if (QueryModel.LeaveDateStart.HasValue)
                strWhere.AppendFormat(" and datediff(dd,'{0}',LeaveDate) >= 0 ", QueryModel.LeaveDateStart.Value.ToShortDateString());
            if (QueryModel.LeaveDateEnd.HasValue)
                strWhere.AppendFormat(" and datediff(dd,LeaveDate,'{0}') >= 0 ", QueryModel.LeaveDateEnd.Value.ToShortDateString());

            //线路区域
            strIds = this.GetSqlIdStrByArray(QueryModel.AreaIds);
            if (!string.IsNullOrEmpty(strIds))
                strWhere.AppendFormat(" and AreaId in ({0}) ", strIds);

            //责任计调
            //strIds = this.GetSqlIdStrByArray(QueryModel.LogisticIds);
            //if (!string.IsNullOrEmpty(strIds))
            //    strWhere.AppendFormat(" and TourId in (select TourId from tbl_TourOperator where tbl_TourOperator.OperatorId in ({0})) ", strIds);

            //人次统计销售员
            strIds = this.GetSqlIdStrByArray(QueryModel.LogisticIds);
            if (!string.IsNullOrEmpty(strIds))
                strWhere.AppendFormat(" and PerTimeSellerId in ({0}) ", strIds);

            if (QueryModel.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单)
                strWhere.AppendFormat(" and OrderState = {0} ", (int)Model.EnumType.TourStructure.OrderState.已成交);
            else if (QueryModel.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计有效订单)
                strWhere.AppendFormat(" and OrderState in ({0},{1},{2}) ", (int)Model.EnumType.TourStructure.OrderState.未处理, (int)Model.EnumType.TourStructure.OrderState.已成交, (int)Model.EnumType.TourStructure.OrderState.已留位);

            if (QueryModel.CustomerId > 0)
                strWhere.AppendFormat(" and BuyCompanyID = {0} ", QueryModel.CustomerId);
            //责任销售
            strIds = this.GetSqlIdStrByArray(QueryModel.SalesClerkIds);
            if (!string.IsNullOrEmpty(strIds))
                strWhere.AppendFormat(" and SalerId in ({0}) ", strIds);

            //对方操作员
            strIds = this.GetSqlIdStrByArray(QueryModel.OperatorId);
            if (!string.IsNullOrEmpty(strIds))
                strWhere.AppendFormat(" and BuyerContactId in ({0}) ", strIds);

            /*//所属地区
            strIds = this.GetSqlIdStrByArray(QueryModel.CityIds);
            if (!string.IsNullOrEmpty(strIds))
                strWhere.AppendFormat(" and BuyCompanyID in (select Id from tbl_Customer where tbl_Customer.IsDelete = '0' and tbl_Customer.CityId in ({0})) ", strIds);*/

            if ((QueryModel.CityIds != null && QueryModel.CityIds.Length > 0)
                || (QueryModel.ProvinceIds != null && QueryModel.ProvinceIds.Length > 0))
            {
                strWhere.AppendFormat(" AND BuyCompanyId IN(SELECT Id FROM tbl_Customer AS A WHERE A.IsDelete='0' AND CompanyId ={0} ", QueryModel.CompanyId);

                if (QueryModel.CityIds != null && QueryModel.CityIds.Length > 0)
                {
                    strWhere.AppendFormat(" AND A.CityId IN({0}) ", Utils.GetSqlIdStrByArray(QueryModel.CityIds));
                }

                if (QueryModel.ProvinceIds != null && QueryModel.ProvinceIds.Length > 0)
                {
                    strWhere.AppendFormat(" AND A.ProviceId IN({0}) ", Utils.GetSqlIdStrByArray(QueryModel.ProvinceIds));
                }

                strWhere.Append(" ) ");
            }

            //组团社名称
            if (!string.IsNullOrEmpty(QueryModel.CustomerName))
                strWhere.AppendFormat(" and BuyCompanyName like '%{0}%' ", QueryModel.CustomerName);

            if (!string.IsNullOrEmpty(us))
            {
                strWhere.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_Tour AS A WHERE A.TourId=View_SalesStatistics.TourId AND A.OperatorId IN({0})) ", us);
            }

            if (QueryModel.ShangDanSDate.HasValue)
            {
                strWhere.AppendFormat(" AND IssueTime>='{0}' ", QueryModel.ShangDanSDate.Value);
            }

            if (QueryModel.ShangDanEDate.HasValue)
            {
                strWhere.AppendFormat(" AND IssueTime<='{0}' ", QueryModel.ShangDanEDate.Value.AddDays(1).AddMilliseconds(-1));
            }

            return strWhere.ToString();
        }

        /// <summary>
        /// 根据整型数组生成半角逗号分割的Sql字符串
        /// </summary>
        /// <param name="arrIds">整型数组</param>
        /// <returns>半角逗号分割的Sql字符串</returns>
        private string GetSqlIdStrByArray(int[] arrIds)
        {
            if (arrIds == null || arrIds.Length <= 0)
                return string.Empty;

            string strTmp = string.Empty;
            foreach (int i in arrIds)
            {
                if (i <= 0)
                    continue;

                strTmp += i.ToString() + ",";
            }
            strTmp = strTmp.Trim(',');

            return strTmp;
        }

        /// <summary>
        /// 根据Xml生成人员实体集合
        /// </summary>
        /// <param name="OperatorXML">人员xml</param>
        /// <param name="IdFiledName">Id属性名称</param>
        /// <param name="NameFiledName">Name属性名称</param>
        /// <returns>人员实体集合</returns>
        private IList<Model.StatisticStructure.StatisticOperator> GetOperatorListByXML(string OperatorXML, string IdFiledName
            , string NameFiledName)
        {
            if (string.IsNullOrEmpty(OperatorXML) || string.IsNullOrEmpty(IdFiledName) || string.IsNullOrEmpty(NameFiledName))
                return null;

            XElement xRoot = XElement.Parse(OperatorXML);
            var xRows = Utils.GetXElements(xRoot, "row");
            if (xRows == null || xRows.Count() <= 0)
                return null;

            IList<Model.StatisticStructure.StatisticOperator> tmpList = new List<Model.StatisticStructure.StatisticOperator>();
            Model.StatisticStructure.StatisticOperator tmpModel = null;
            foreach (var t in xRows)
            {
                if (t == null)
                    continue;

                tmpModel = new EyouSoft.Model.StatisticStructure.StatisticOperator();
                tmpModel.OperatorId = Utils.GetInt(Utils.GetXAttributeValue(t, IdFiledName));
                tmpModel.OperatorName = Utils.GetXAttributeValue(t, NameFiledName);

                tmpList.Add(tmpModel);
            }

            return tmpList;
        }

        /// <summary>
        /// 根据单项服务类别XML生成名称字符串
        /// </summary>
        /// <param name="AreaNameXML">单项服务类别XML</param>
        /// <returns>单项服务类别名称字符串</returns>
        private string GetPlanSingleAreaName(string AreaNameXML)
        {
            if (string.IsNullOrEmpty(AreaNameXML))
                return string.Empty;

            XElement xRoot = XElement.Parse(AreaNameXML);
            if (xRoot == null)
                return string.Empty;

            var xRows = Utils.GetXElements(xRoot, "row");
            if (xRows == null || xRows.Count() <= 0)
                return string.Empty;

            string strAreaName = string.Empty;
            foreach (var t in xRows)
            {
                if (t == null)
                    continue;

                strAreaName += ((Model.EnumType.TourStructure.ServiceType)Utils.GetInt(Utils.GetXAttributeValue(t, "ServiceType"))).ToString() + ",";
            }

            strAreaName = strAreaName.Trim(',');
            return strAreaName;
        }

        #endregion
    }
}
