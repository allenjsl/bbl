/*   Author:周文超 2011-01-21    */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.StatisticStructure
{
    #region 支出对账单基类

    /// <summary>
    /// 支出对账单基类
    /// </summary>
    public class ExpenseStatisticBase
    {
        /// <summary>
        /// 计调员Id
        /// </summary>
        public int LogisticsId { get; set; }

        /// <summary>
        /// 计调员名称
        /// </summary>
        public string LogisticsName { get; set; }

        /// <summary>
        /// 总支出
        /// </summary>
        public decimal ExpenseSum { get; set; }

        /// <summary>
        /// 已付
        /// </summary>
        public decimal CheckPay { get; set; }

        /// <summary>
        /// 未付
        /// </summary>
        public decimal NoCheckPay 
        {
            get 
            {
                return this.ExpenseSum - this.CheckPay;
            }
        }
    }

    #endregion

    #region 支出对账-区域统计实体

    /// <summary>
    /// 支出对账-区域统计实体
    /// </summary>
    public class ExpenseAreaStatistic : ExpenseStatisticBase
    {
        /// <summary>
        /// 线路区域Id
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// 线路区域名称
        /// </summary>
        public string AreaName { get; set; }
    }

    #endregion

    #region 支出对账-部门统计实体

    /// <summary>
    /// 支出对账-部门统计实体
    /// </summary>
    public class ExpenseDepartStatistic : ExpenseStatisticBase
    {
        /// <summary>
        /// 部门Id
        /// </summary>
        public int DepartId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName { get; set; }
    }

    #endregion

    #region 支出对账单查询实体

    /// <summary>
    /// 支出对账单查询实体
    /// </summary>
    public class QueryExpenseStatistic
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 团队类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType? TourType { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public int[] DepartIds { get; set; }

        /// <summary>
        /// 计调员
        /// </summary>
        public int[] SaleIds { get; set; }

        /// <summary>
        /// 线路区域Id
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int OrderIndex { get; set; }
    }

    #endregion
}
