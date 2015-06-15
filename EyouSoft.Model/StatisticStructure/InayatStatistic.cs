
/*   Author:周文超 2011-01-21    */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.StatisticStructure
{
    #region 人员类（销售员、计调员）

    /// <summary>
    /// 销售员/计调员
    /// </summary>
    public class StatisticOperator
    {
        /// <summary>
        /// 人员Id
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 人员名称
        /// </summary>
        public string OperatorName { get; set; }
    }

    #endregion

    #region 人次统计基类

    /// <summary>
    /// 人次统计基类
    /// </summary>
    public class InayatStatisticBase
    {
        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleCount { get; set; }

        /// <summary>
        /// 人天数
        /// </summary>
        public decimal PeopleDays { get; set; }
        /// <summary>
        /// 团队数
        /// </summary>
        public int TuanDuiShu { get; set; }
    }

    #endregion

    #region 人次--区域统计实体

    /// <summary>
    /// 人次--区域统计实体
    /// </summary>
    public class InayaAreatStatistic : InayatStatisticBase
    {
        /// <summary>
        /// 线路区域Id
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// 线路区域名称
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 计调员
        /// </summary>
        public IList<StatisticOperator> Logistics { get; set; }

        /// <summary>
        /// 销售员
        /// </summary>
        public IList<StatisticOperator> SalesClerk { get; set; }
        
    }

    #endregion

    #region 人次--部门统计实体

    /// <summary>
    /// 人次--部门统计实体
    /// </summary>
    public class InayaDepartStatistic : InayatStatisticBase
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
        /// 销售员
        /// </summary>
        public IList<StatisticOperator> SalesClerk { get; set; }

    }

    #endregion

    #region 人次--时间统计实体

    /// <summary>
    /// 人次--时间统计实体
    /// </summary>
    public class InayaTimeStatistic : InayatStatisticBase
    {
        /// <summary>
        /// 年份
        /// </summary>
        public int CurrYear { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int CurrMonth { get; set; }
    }

    #endregion

    #region 人次统计查询实体

    /// <summary>
    /// 人次统计查询实体
    /// </summary>
    public class QueryInayatStatistic
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 线路区域Id
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public int[] DepartIds { get; set; }

        /// <summary>
        /// 销售员
        /// </summary>
        public int[] SaleIds { get; set; }

        /// <summary>
        /// 下单开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 下单结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 出团时间-始
        /// </summary>
        public DateTime? LeaveDateStart { get; set; }

        /// <summary>
        /// 出团时间-止
        /// </summary>
        public DateTime? LeaveDateEnd { get; set; }

        /// <summary>
        /// 统计订单方式
        /// </summary>
        public Model.EnumType.CompanyStructure.ComputeOrderType ComputeOrderType { get; set; }

        /// <summary>
        /// 排序索引 0/1：线路区域Id升/降序；2/3：部门Id升/降序；4/5月份升/降序；
        /// </summary>
        public int OrderIndex { get; set; }
        /// <summary>
        /// 计划类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType? TourType { get; set; }
    }

    #endregion

}
