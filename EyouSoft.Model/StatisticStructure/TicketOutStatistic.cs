
/*   Author:周文超 2011-03-18    */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.StatisticStructure
{

    #region 出票统计基类

    /// <summary>
    /// 出票统计基类
    /// </summary>
    public class TicketOutStatisticBase
    {
        /// <summary>
        /// 出票量
        /// </summary>
        public int TicketOutNum { get; set; }

        /// <summary>
        /// 应付机票款
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 已付机票款
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 未付机票款
        /// </summary>
        public decimal UnPaidAmount
        {
            get { return this.TotalAmount - this.PayAmount; }
        }
    }

    #endregion

    #region 出票--售票处统计

    /// <summary>
    /// 出票--售票处统计
    /// </summary>
    public class TicketOutStatisticOffice : TicketOutStatisticBase
    {
        /// <summary>
        /// 售票处（机票供应商）Id
        /// </summary>
        public int OfficeId { get; set; }

        /// <summary>
        /// 售票处（机票供应商）名称
        /// </summary>
        public string OfficeName { get; set; }
    }

    #endregion

    #region 出票--航空公司统计

    /// <summary>
    /// 出票--航空公司统计
    /// </summary>
    public class TicketOutStatisticAirLine : TicketOutStatisticBase
    {
        /// <summary>
        /// 航空公司Id
        /// </summary>
        public int AirLineId { get; set; }

        /// <summary>
        /// 航空公司名称
        /// </summary>
        public string AirLineName
        {
            get
            {
                return ((Model.EnumType.PlanStructure.FlightCompany)this.AirLineId).ToString();
            }
        }
    }

    #endregion

    #region 出票--部门统计

    /// <summary>
    /// 出票--部门统计
    /// </summary>
    public class TicketOutStatisticDepart : TicketOutStatisticBase
    {
        /// <summary>
        /// 部门id
        /// </summary>
        public int DepartId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName { get; set; }
    }

    #endregion

    #region 出票--日期统计

    /// <summary>
    /// 出票--日期统计
    /// </summary>
    public class TicketOutStatisticTime : TicketOutStatisticBase
    {
        /// <summary>
        /// 当前年
        /// </summary>
        public int CurrYear { get; set; }

        /// <summary>
        /// 当前月
        /// </summary>
        public int CurrMonth { get; set; }
    }

    #endregion

    #region 出票统计查询实体

    /// <summary>
    /// 出票统计查询实体
    /// </summary>
    public class QueryTicketOutStatisti
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 航空公司Id
        /// </summary>
        public int[] AirLineIds { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        public int[] DepartIds { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName { get; set; }

        /// <summary>
        /// 售票处（机票供应商）Id
        /// </summary>
        public int OfficeId { get; set; }

        /// <summary>
        /// 售票处（机票供应商）名称
        /// </summary>
        public string OfficeName { get; set; }

        /// <summary>
        /// 出票开始日期
        /// </summary>
        public DateTime? StartTicketOutTime { get; set; }

        /// <summary>
        /// 出票结束日期
        /// </summary>
        public DateTime? EndTicketOutTime { get; set; }

        /// <summary>
        /// 出团开始日期
        /// </summary>
        public DateTime? LeaveDateStart { get; set; }

        /// <summary>
        /// 出团结束日期
        /// </summary>
        public DateTime? LeaveDateEnd { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int OrderIndex { get; set; }
    }

    #endregion
}
