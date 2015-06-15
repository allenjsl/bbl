using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.TourStructure
{
    /// <summary>
    /// 游客退票信息实体
    /// 创建人：luofx 2011-01-19
    /// </summary>
    public class CustomerRefund
    {
        #region Model      
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { set; get; }
        /// <summary>
        /// 所属游客编号
        /// </summary>
        public string CustormerId { set; get; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { set; get; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo { set; get; }
        /// <summary>
        /// 操作员名字
        /// </summary>
        public string OperatorName { set; get; }
        /// <summary>
        /// 操作员ID
        /// </summary>
        public int OperatorID { set; get; }
        
        /// <summary>
        /// 退回金额
        /// </summary>
        public decimal RefundAmount { set; get; }
        /// <summary>
        /// 退票状态(未退票，已退票)
        /// </summary>
        public Model.EnumType.PlanStructure.TicketRefundSate IsRefund { set; get; }
        /// <summary>
        /// 退票需知
        /// </summary>
        public string RefundNote { set; get; }
        /// <summary>
        /// 退票时间
        /// </summary>
        public DateTime IssueTime { set; get; }

        /// <summary>
        /// 游客姓名
        /// </summary>
        public string VisitorName { set; get; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.CradType CradType { set; get; }
        /// <summary>
        /// 证件编号
        /// </summary>
        public string CradNumber { set; get; }

        /// <summary>
        /// 游客已退航段信息集合
        /// </summary>
        public IList<MCustomerRefundFlight> CustomerRefundFlights { get; set; }

        #endregion Model
    }

    /// <summary>
    /// 游客退票已退航段信息
    /// </summary>
    [Serializable]
    public class MCustomerRefundFlight
    {
        /// <summary>
        /// 游客退票信息编号
        /// </summary>
        public string RefundId { get; set; }

        /// <summary>
        /// 游客已退航段编号
        /// </summary>
        public int FlightId { get; set; }

        /// <summary>
        /// 游客编号
        /// </summary>
        public string CustomerId { set; get; }
    }

    /// <summary>
    /// 游客所有航段信息
    /// </summary>
    [Serializable]
    public class MCustomerAllFlight
    {
        /// <summary>
        /// 游客已退票航段集合
        /// </summary>
        public IList<MCustomerRefundFlight> RefundFlights { get; set; }

        /// <summary>
        /// 游客机票申请时的所有航段集合
        /// </summary>
        public IList<Model.PlanStructure.MTicketFlightAndState> TicketFlights { get; set; }
    }
}
