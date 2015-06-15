/*   Author:周文超 2011-01-21    */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.StatisticStructure
{
    #region 员工业绩统计基类

    /// <summary>
    /// 员工业绩统计基类
    /// </summary>
    public class PersonnelStatisticBase
    {
        /// <summary>
        /// 部门Id
        /// </summary>
        public int DepartId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName { get; set; }

        /// <summary>
        /// 计调员
        /// </summary>
        public IList<StatisticOperator> Logistics { get; set; }

        /// <summary>
        /// 销售员
        /// </summary>
        public StatisticOperator SalesClerk { get; set; }

        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleNum { get; set; }

        /// <summary>
        /// 收入
        /// </summary>
        public decimal Income { get; set; }
    }

    #endregion

    #region 员工业绩-收入统计

    /// <summary>
    /// 员工业绩-收入统计
    /// </summary>
    public class PersonnelIncomeStatistic : PersonnelStatisticBase
    {
        
    }

    #endregion

    #region 员工业绩-利润统计

    /// <summary>
    /// 员工业绩-利润统计
    /// </summary>
    public class PersonnelProfitStatistic : PersonnelStatisticBase
    {
        /// <summary>
        /// 支出
        /// </summary>
        public decimal OutMoney { get; set; }

        /// <summary>
        /// 利润分配
        /// </summary>
        public decimal ShareMoney { get; set; }
    }

    #endregion

    #region 员工业绩统计查询实体

    /// <summary>
    /// 员工业绩统计查询实体
    /// </summary>
    public class QueryPersonnelStatistic
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 出团时间起
        /// </summary>
        public DateTime? LeaveDateStart { get; set; }

        /// <summary>
        /// 出团时间止
        /// </summary>
        public DateTime? LeaveDateEnd { get; set; }

        /// <summary>
        /// 签单日期起
        /// </summary>
        public DateTime? CheckDateStart { get; set; }

        /// <summary>
        /// 签单日期止
        /// </summary>
        public DateTime? CheckDateEnd { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public int[] DepartIds { get; set; }

        /// <summary>
        /// 销售员
        /// </summary>
        public int[] SaleIds { get; set; }

        /// <summary>
        /// 计调员
        /// </summary>
        public int[] LogisticsIds { get; set; }

        /// <summary>
        /// 统计订单方式
        /// </summary>
        public Model.EnumType.CompanyStructure.ComputeOrderType ComputeOrderType { get; set; }

        /// <summary>
        /// 排序索引 0/1：销售员Id升/降
        /// </summary>
        public int OrderIndex { get; set; }
    }

    #endregion
}
