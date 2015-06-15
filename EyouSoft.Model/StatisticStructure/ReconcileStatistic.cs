/*   Author:周文超 2011-01-21    */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.StatisticStructure
{
    #region 收入对账单基类

    /// <summary>
    /// 收入对账单基类
    /// </summary>
    public class ReconcileStatisticBase
    {
        /// <summary>
        /// 销售员Id
        /// </summary>
        public int SaleId { get; set; }

        /// <summary>
        /// 销售员
        /// </summary>
        public string SalesClerk { get; set; }

        /// <summary>
        /// 总收入
        /// </summary>
        public decimal GrossIncome { get; set; }

        /// <summary>
        /// 已收
        /// </summary>
        public decimal CheckIncome { get; set; }

        /// <summary>
        /// 未收
        /// </summary>
        public decimal NoCheckIncome 
        {
            get
            {
                return this.GrossIncome - this.CheckIncome;
            }
        }

    }

    #endregion

    #region 收入对账-区域统计实体

    /// <summary>
    /// 收入对账-区域统计实体
    /// </summary>
    public class ReconcileAreaStatistic : ReconcileStatisticBase
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

    #region 收入对账-部门统计实体

    /// <summary>
    /// 收入对账-部门统计实体
    /// </summary>
    public class ReconcileDepartStatistic : ReconcileStatisticBase
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

    #region 收入对账单查询实体

    /// <summary>
    /// 收入对账单查询实体
    /// </summary>
    public class QueryReconcileStatistic
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
        /// 销售员
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
