/*Author:汪奇志 2011-01-19*/
using System;
using System.Collections.Generic;

namespace EyouSoft.Model.RouteStructure
{
    #region 线路基本信息业务实体
    /// <summary>
    /// 线路基本信息业务实体
    /// </summary>
    public class RouteBaseInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public RouteBaseInfo() { }

        /// <summary>
        /// 线路编号
        /// </summary>
        public int RouteId { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 发布人编号
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 发布人姓名
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 线路区域编号
        /// </summary>
        public int AreaId { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 线路天数
        /// </summary>
        public int RouteDays { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 发布类型
        /// </summary>
        public EnumType.TourStructure.ReleaseType ReleaseType { get; set; }
        /// <summary>
        /// 上团数
        /// </summary>
        public int TourCount { get; set; }
        /// <summary>
        /// 收客数
        /// </summary>
        public int VisitorCount { get; set; }
        /// <summary>
        /// 线路描述
        /// </summary>
        public string RouteDepict { get; set; }

        #region 附件属性
        /// <summary>
        /// 线路区域名称
        /// </summary>
        public string AreaName
        {
            get;
            set;
        }
        #endregion

    }
    #endregion

    #region 线路信息业务实体
    /// <summary>
    /// 线路信息业务实体
    /// </summary>
    public class RouteInfo:RouteBaseInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public RouteInfo() { }

        /// <summary>
        /// 附件信息集合
        /// </summary>
        public IList<TourStructure.TourAttachInfo> Attachs { get; set; }
        /// <summary>
        /// 快速发布线路专有信息
        /// </summary>
        public TourStructure.TourQuickPrivateInfo RouteQuickInfo { get; set; }
        /// <summary>
        /// 标准发布线路专有信息
        /// </summary>
        public TourStructure.TourNormalPrivateInfo RouteNormalInfo { get; set; }
    }
    #endregion    

     #region 线路查询信息业务实体
    /// <summary>
    /// 线路查询信息业务实体
    /// </summary>
    public class RouteSearchInfo
    {
        /// <summary>
        /// 线路区域编号
        /// </summary>
        public int? AreaId { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 发布起始日期
        /// </summary>
        public DateTime? RSDate { get; set; }
        /// <summary>
        /// 发布截止日期
        /// </summary>
        public DateTime? REDate { get; set; }
        /// <summary>
        /// 线路天数
        /// </summary>
        public int? RouteDays { get; set; }
        /// <summary>
        /// 发布人编号
        /// </summary>
        public int? ROperatorId { get; set; }
        /// <summary>
        /// 发布人姓名
        /// </summary>
        public string OperatorName { get; set; }
    }
     #endregion
}
