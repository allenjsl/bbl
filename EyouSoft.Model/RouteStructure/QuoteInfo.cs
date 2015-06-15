/*Author:汪奇志 2011-01-19*/
using System;
using System.Collections.Generic;

namespace EyouSoft.Model.RouteStructure
{
    #region 团队计划报价信息业务实体
    /// <summary>
    /// 团队计划报价信息业务实体
    /// </summary>
    public class QuoteTeamInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public QuoteTeamInfo() { }

        /// <summary>
        /// 报价编号
        /// </summary>
        public int QuoteId { get; set; }
        /// <summary>
        /// 线路编号
        /// </summary>
        public int RouteId { get; set; }
        /// <summary>
        /// 询价单位编号
        /// </summary>
        public int QuoteUnitsId { get; set; }
        /// <summary>
        /// 询价单位名称
        /// </summary>
        public string QuoteUnitsName { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactTel { get; set; }
        /// <summary>
        /// 预计出团时间
        /// </summary>
        public DateTime TmpLeaveDate { get; set; }
        /// <summary>
        /// 成人数
        /// </summary>
        public int AdultNum { get; set; }
        /// <summary>
        /// 儿童数
        /// </summary>
        public int ChildNum { get; set; }
        /// <summary>
        /// 总人数
        /// </summary>
        public int PeopleNum { get; set; }
        /// <summary>
        /// 地接报价总金额
        /// </summary>
        public decimal LocalQuoteSum { get; set; }
        /// <summary>
        /// 我社报价总金额
        /// </summary>
        public decimal SelfQuoteSum { get; set; }
        /// <summary>
        /// 机票折扣
        /// </summary>
        public decimal TicketAgio { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 报价人编号
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 报价时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 包含项目信息集合
        /// </summary>
        public IList<TourStructure.TourTeamServiceInfo> Services { get; set; }
        /// <summary>
        /// 客户要求信息集合
        /// </summary>
        public IList<TourStructure.TourServiceInfo> Requirements { get; set; }
    }
    #endregion
}
