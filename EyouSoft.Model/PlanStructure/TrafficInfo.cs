using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.PlanStructure
{
    #region 交通管理
    /// <summary>
    /// 交通管理实体
    /// autor:李晓欢 date:2012-09-07
    /// </summary>
    [Serializable]
    public class TrafficInfo
    {
        public TrafficInfo() { }
        #region model
        /// <summary>
        /// 交通编号
        /// </summary>
        public int TrafficId { get; set; }
        /// <summary>
        /// 交通名称
        /// </summary>
        public string TrafficName { get; set; }
        /// <summary>
        /// 天数
        /// </summary>
        public int TrafficDays { get; set; }
        /// <summary>
        /// 儿童价
        /// </summary>
        public decimal ChildPrices { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TrafficStatus Status { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operater { get; set; }
        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperaterId { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsueTime { get; set; }
        /// <summary>
        /// 交通行程
        /// </summary>
        public IList<EyouSoft.Model.PlanStructure.TravelInfo> travelList { get; set; }

        #endregion
    }
    #endregion

    #region 行程
    /// <summary>
    /// 交通行程实体
    /// </summary>
    [Serializable]
    public class TravelInfo
    {
        public TravelInfo() { }
        #region model
        /// <summary>
        /// 行程编号
        /// </summary>
        public int TravelId { get; set; }
        /// <summary>
        /// 交通编号
        /// </summary>
        public int TrafficId { get; set; }
        /// <summary>
        /// 序列
        /// </summary>
        public int SerialNum { get; set; }
        /// <summary>
        /// 交通类型
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TrafficType TrafficType { get; set; }
        /// <summary>
        /// 出发省份
        /// </summary>
        public int LProvince { get; set; }
        /// <summary>
        /// 出发省份名称
        /// </summary>
        public string LProvinceName { get; set; }
        /// <summary>
        /// 出发城市
        /// </summary>        
        public int LCity { get; set; }
        /// <summary>
        /// 出发城市名称
        /// </summary>
        public string LCityName { get; set; }
        /// <summary>
        /// 抵达省份
        /// </summary>
        public int RProvince { get; set; }
        public string RProvinceName { get; set; }
        /// <summary>
        /// 抵达城市
        /// </summary>
        public int RCity { get; set; }
        public string RCityName { get; set; }
        /// <summary>
        /// 航班号
        /// </summary>
        public string FilghtNum { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.FlightCompany FlightCompany { get; set; }
        /// <summary>
        /// 出发时间
        /// </summary>
        public string LTime { get; set; }
        /// <summary>
        /// 抵达时间
        /// </summary>
        public string RTime { get; set; }
        /// <summary>
        /// 是否停靠
        /// </summary>
        public bool IsStop { get; set; }
        /// <summary>
        /// 舱位
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.Space Space { get; set; }
        /// <summary>
        /// 机型
        /// </summary>
        public string AirPlaneType { get; set; }
        /// <summary>
        /// 间隔天数
        /// </summary>
        public int IntervalDays { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operater { get; set; }
        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperaterID { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsueTime { get; set; }
        #endregion
    }
    #endregion

    #region 价格
    /// <summary>
    /// 交通价格实体
    /// </summary>
    [Serializable]
    public class TrafficPricesInfo
    {
        public TrafficPricesInfo() { }
        #region model
        /// <summary>
        /// 价格编号
        /// </summary>
        public string PricesID { get; set; }
        /// <summary>
        /// 交通编号
        /// </summary>
        public int TrafficId { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime SDateTime { get; set; }
        /// <summary>
        /// 票单价
        /// </summary>
        public decimal TicketPrices { get; set; }
        /// <summary>
        /// 票总数
        /// </summary> 
        public int TicketNums { get; set; }

        /// <summary>
        /// 已使用票数
        /// </summary>
        public int YiShiYong { get; set; }

        /// <summary>
        /// 剩余票数
        /// </summary>
        public int ShengYu
        {
            get
            {
                return TicketNums - YiShiYong;
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TicketStatus Status { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime InsueTime { get; set; }
        #endregion
    }
    #endregion

    #region 交通管理查询实体

    /// <summary>
    /// 交通管理查询实体
    /// </summary>
    public class TrafficSearch
    {
        /// <summary>
        /// 交通名称
        /// </summary>
        public string TrafficName { get; set; }

        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }
    }

    #endregion

    #region 交通出票统计实体

    /// <summary>
    /// 交通出票统计实体
    /// </summary>
    public class JiaoTongChuPiao
    {
        /// <summary>
        /// 交通编号
        /// </summary>
        public int TrafficId { get; set; }

        /// <summary>
        /// 交通名称
        /// </summary>
        public string TrafficName { get; set; }

        /// <summary>
        /// 出票数
        /// </summary>
        public int ChuPiaoShu { get; set; }

        /// <summary>
        /// 代理费
        /// </summary>
        public decimal AgencyPrice { get; set; }
    }

    #endregion

    #region 交通出票统计查询实体

    /// <summary>
    /// 交通出票统计查询实体
    /// </summary>
    public class JiaoTongChuPiaoSearch
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }

    #endregion


}
