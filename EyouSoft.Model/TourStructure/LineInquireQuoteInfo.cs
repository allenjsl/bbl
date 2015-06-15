using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.TourStructure
{
    #region 询价报价实体类
    /// <summary>
    /// 描述:线路询价报价实体类
    /// </summary>    
    /// 1. 2010-3-18 AM　曹胡生  创建
    public class LineInquireQuoteInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public LineInquireQuoteInfo() { }

        /// <summary>
        /// 询价报价编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 专线公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 线路编号
        /// </summary>
        public int RouteId { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 询价单位编号
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 询价单位名称
        /// </summary>
        public string CustomerName { get; set; }
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
        public DateTime? LeaveDate { get; set; }
        /// <summary>
        /// 成人数
        /// </summary>
        public int AdultNumber { get; set; }
        /// <summary>
        /// 儿童数
        /// </summary>
        public int ChildNumber { get; set; }
        /// <summary>
        /// 总人数
        /// </summary>
        public int PeopleNum { get; set; }
        /// <summary>
        /// 特殊要求说明
        /// </summary>
        public string SpecialClaim { get; set; }
        /// <summary>
        /// 机票折扣
        /// </summary>
        public decimal TicketAgio { get; set; }
        /// <summary>
        /// 询价日期
        /// </summary>
        public DateTime? IssueTime { get; set; }
        /// <summary>
        /// 询价状态
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.QuoteState QuoteState { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 生成的计划编号
        /// </summary>
        public string BuildTourId { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 行程要求
        /// </summary>
        public XingChengMust XingCheng { get; set; }
        /// <summary>
        /// 价格组成信息集合
        /// </summary>
        public IList<TourStructure.TourTeamServiceInfo> Services { get; set; }
        /// <summary>
        /// 客人要求信息集合
        /// </summary>
        public IList<TourStructure.TourServiceInfo> Requirements { get; set; }
        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 游客信息
        /// </summary>
        public TourEverydayApplyTravellerInfo Traveller { get; set; }
    }    
    #endregion

    #region 组团询价查询信息业务实体
    /// <summary>
    /// 组团询价查询信息业务实体
    /// </summary>
    public class LineInquireQuoteSearchInfo
    {
        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 天数
        /// </summary>
        public int DayNum { get; set; }
        /// <summary>
        /// 出团开始时间
        /// </summary>
        public DateTime? SDate { get; set; }
        /// <summary>
        /// 出团结束时间
        /// </summary>
        public DateTime? EDate { get; set; }
        /// <summary>
        /// 询团起始时间
        /// </summary>
        public DateTime? XunTuanSTime { get; set; }
        /// <summary>
        /// 询团截止时间
        /// </summary>
        public DateTime? XunTuanETime { get; set; }
    }
    #endregion

    #region 行程要求信息业务实体
    /// <summary>
    /// 行程要求信息业务实体
    /// </summary>
    public class XingChengMust
    {
        public XingChengMust()
        {
        }
        /// <summary>
        /// 询价报价编号
        /// </summary>
        public int QuoteId
        {
            get;
            set;
        }
        /// <summary>
        /// 行程要求
        /// </summary>
        public string QuotePlan
        {
            get;
            set;
        }
        /// <summary>
        /// 行程附件名称
        /// </summary>
        public string PlanAccessoryName
        {
            get;
            set;
        }
        /// <summary>
        /// 行程附件
        /// </summary>
        public string PlanAccessory
        {
            get;
            set;
        }
    }
    #endregion
}
