/************************************************************
 * 模块名称：退票统计相关实体
 * 功能说明：
 * 创建人：周文超  2011-4-21 16:57:24
 * *********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.StatisticStructure
{
    #region 退票统计信息实体

    /// <summary>
    /// 退票统计信息实体
    /// </summary>
    public class MRefundStatistic
    {
        /// <summary>
        /// 退票信息编号
        /// </summary>
        public string RefundId { get; set; }

        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo { get; set; }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime LeaveDate { get; set; }

        /// <summary>
        /// 游客编号
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// 退票人（订单游客姓名）
        /// </summary>
        public string RefundName { get; set; }

        /// <summary>
        /// 已退航段信息
        /// </summary>
        public IList<Model.PlanStructure.TicketFlight> RefundFlight { get; set; }

        /// <summary>
        /// 票款（机票申请时的总费用）
        /// </summary>
        public decimal TicketPrice { get; set; }

        /// <summary>
        /// 我社操作人（专线退票保存人）
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 组团社名称
        /// </summary>
        public string BuyCompanyName { get; set; }

        /// <summary>
        /// 对方操作人（订单下单人）
        /// </summary>
        public string BuyOrderOperatorName { get; set; }

        /// <summary>
        /// 退回金额
        /// </summary>
        public decimal RefundAmount { get; set; }
    }

    #endregion

    #region 退票统计查询实体

    /// <summary>
    /// 退票统计查询实体
    /// </summary>
    public class MQueryRefundStatistic
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo { get; set; }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime? LeaveDateStart { get; set; }

        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime? LeaveDateEnd { get; set; }

        /// <summary>
        /// 航段
        /// </summary>
        public string FligthSegment { get; set; }

        /// <summary>
        /// 航空公司
        /// </summary>
        public Model.EnumType.PlanStructure.FlightCompany? AireLine { get; set; }

        /// <summary>
        /// 组团社名称
        /// </summary>
        public string BuyCompanyName { get; set; }

        /// <summary>
        /// 组团社编号
        /// </summary>
        public int BuyCompanyId { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        public int[] DepIds { get; set; }

        /// <summary>
        /// 我社操作员编号
        /// </summary>
        public int[] OperatorIds { get; set; }

        /// <summary>
        /// 排序索引（0/1团号升/降序；2/3退票时间升/降序）
        /// </summary>
        public int OrderIndex { get; set; }

    }

    #endregion
}
