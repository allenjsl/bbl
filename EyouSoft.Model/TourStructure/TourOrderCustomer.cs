using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.TourStructure
{
    #region 订单游客信息实体
    /// <summary>
    /// 订单游客信息实体
    /// </summary>
    /// 创建人：luofx 2011-01-19
    public class TourOrderCustomer
    {
        /// <summary>
        /// 游客编号
        /// </summary>
        public string ID { set; get; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { set; get; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyID { set; get; }
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
        /// 游客性别
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.Sex Sex { set; get; }
        /// <summary>
        /// 游客类型 
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.VisitorType VisitorType { set; get; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactTel { set; get; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { set; get; }
        /// <summary>
        /// 游客退团状态
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.CustomerStatus CustomerStatus { set; get; }
        /// <summary>
        /// 是否删除 
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 游客退团信息
        /// </summary>
        public EyouSoft.Model.TourStructure.CustomerLeague LeagueInfo { set; get; }        
        /// <summary>
        /// 游客特服信息
        /// </summary>
        public EyouSoft.Model.TourStructure.CustomerSpecialService SpecialServiceInfo { set; get; }
        /// <summary>
        /// 申请的航段信息集合
        /// </summary>
        public IList<int> ApplyFlights { get; set; }
        /// <summary>
        /// 退的航段信息集合
        /// </summary>
        public IList<int> RefundFlights { get; set; }
    }
    #endregion

    #region 游客机票申请航段信息业务实体
    /// <summary>
    /// 游客机票申请航段信息业务实体
    /// </summary>
    /// Author:汪奇志 2011-05-03
    public class MTicketApplyFlightInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MTicketApplyFlightInfo() { }
        /// <summary>
        /// 航段编号
        /// </summary>
        public int FlightId { get; set; }
        /// <summary>
        /// 机票申请状态
        /// </summary>
        public int Status { get; set; }
    }
    #endregion
}
