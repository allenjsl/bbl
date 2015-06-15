/************************************************************
 * 模块名称：客户关系管理销售统计相关业务实体
 * 功能说明：客户关系管理销售统计相关业务实体
 * 创建人：周文超  2011-4-18 15:42:16
 * *********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.StatisticStructure
{
    #region 客户关系管理-销售统计实体

    /// <summary>
    /// 客户关系管理-销售统计实体
    /// </summary>
    public class MSalesStatistics
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime LeaveDate { get; set; }

        /// <summary>
        /// 组团社名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 线路区域名称
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 金额（订单财务小计）
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleNum { get; set; }

        /// <summary>
        /// 责任销售编号
        /// </summary>
        public int SalerId { get; set; }

        /// <summary>
        /// 人次统计销售员姓名
        /// </summary>
        public string SalerName { get; set; }

        /// <summary>
        /// 责任计调
        /// </summary>
        public IList<Model.StatisticStructure.StatisticOperator> Logistics { get; set; }

        /// <summary>
        /// 对方操作员
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 人次统计销售员部门名称
        /// </summary>
        public string DepartName { get; set; }

        /// <summary>
        /// 所属地区
        /// </summary>
        public string ProvinceAndCity { get; set; }
        /// <summary>
        /// 责任销售姓名
        /// </summary>
        public string ZeRenSelllerName { get; set; }
        /// <summary>
        /// 客户单位编号
        /// </summary>
        public int KeHuDanWeiId { get; set; }
    }

    #endregion

    #region 客户关系管理-销售统计查询实体

    /// <summary>
    /// 客户关系管理-销售统计查询实体
    /// </summary>
    public class MQuerySalesStatistics
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 出团日期开始
        /// </summary>
        public DateTime? LeaveDateStart { get; set; }
        /// <summary>
        /// 出团日期结束
        /// </summary>
        public DateTime? LeaveDateEnd { get; set; }
        /// <summary>
        /// 线路区域Id
        /// </summary>
        public int[] AreaIds { get; set; }
        /// <summary>
        /// 组团社Id
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 责任销售
        /// </summary>
        public int[] SalesClerkIds { get; set; }
        /// <summary>
        /// 人次统计销售员编号集合
        /// </summary>
        public int[] LogisticIds { get; set; }
        /// <summary>
        /// 对方操作员
        /// </summary>
        public int[] OperatorId { get; set; }
        /// <summary>
        /// 所属地区
        /// </summary>
        public int[] CityIds { get; set; }
        /// <summary>
        /// 统计订单方式
        /// </summary>
        public Model.EnumType.CompanyStructure.ComputeOrderType ComputeOrderType { get; set; }
        /// <summary>
        /// 排序索引   0/1出团日志升/降序；
        /// </summary>
        public int OrderIndex { get; set; }
        /// <summary>
        /// 组团社名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 客户单位所在省份
        /// </summary>
        public int[] ProvinceIds { get; set; }

        /// <summary>
        /// 上单起始时间
        /// </summary>
        public DateTime? ShangDanSDate { get; set; }
        /// <summary>
        /// 上单截止时间
        /// </summary>
        public DateTime? ShangDanEDate { get; set; }
    }

    #endregion
}
