using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.StatisticStructure
{
    #region 客户关系返佣统计信息业务实体
    /// <summary>
    /// 客户关系返佣统计信息业务实体
    /// </summary>
    /// Author：汪奇志 2011-06-08
    public class MCommissionStatInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MCommissionStatInfo() { }

        /// <summary>
        /// 客户单位编号
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 客户单位操作员编号
        /// </summary>
        public int ContactId { get; set; }
        /// <summary>
        /// 客户单位操作员姓名
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 返佣金额
        /// </summary>
        public decimal CommissionAmount { get { return this.BeforeAmount + this.AfterAmount; } }
        /// <summary>
        /// 现返金额
        /// </summary>
        public decimal BeforeAmount { get; set; }
        /// <summary>
        /// 后返金额
        /// </summary>
        public decimal AfterAmount { get; set; }
    }
    #endregion

    #region 客户关系返佣明细信息业务实体
    /// <summary>
    /// 客户关系返佣明细信息业务实体
    /// </summary>
    /// Author:汪奇志 2011-06-07
    public class MCommissionDetailInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MCommissionDetailInfo() { }

        /// <summary>
        /// 计划编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 成人数
        /// </summary>
        public int AdultNumber { get; set; }
        /// <summary>
        /// 儿童数
        /// </summary>
        public int ChildrenNumber { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal OrderAmount { get; set; }
        /// <summary>
        /// 操作员编号
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 操作员姓名
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 返佣单价
        /// </summary>
        public decimal CommissionPrice { get; set; }
        /// <summary>
        /// 返佣金额
        /// </summary>
        public decimal CommissionAmount { get { return (this.AdultNumber + this.ChildrenNumber) * this.CommissionPrice; } }
        /// <summary>
        /// 是否支付
        /// </summary>
        public bool IsPaid { get; set; }
    }
    #endregion

    #region 客户关系返佣统计查询信息业务实体
    /// <summary>
    /// 客户关系返佣统计查询信息业务实体
    /// </summary>
    public class MCommissionStatSeachInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MCommissionStatSeachInfo() { }

        /// <summary>
        /// 客户单位
        /// </summary>
        public int[] CustomerId { get; set; }

        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 对方操作员
        /// </summary>
        public int[] ContactId { get; set; }

        /// <summary>
        /// 对方操作员名称
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 出团时间-始
        /// </summary>
        public DateTime? LeaveDateStart { get; set; }

        /// <summary>
        /// 出团时间-止
        /// </summary>
        public DateTime? LeaveDateEnd { get; set; }

        /// <summary>
        /// 下单时间-始
        /// </summary>
        public DateTime? OrderDateStart { get; set; }

        /// <summary>
        /// 下单时间-止
        /// </summary>
        public DateTime? OrderDateEnd { get; set; }

        /// <summary>
        /// 我社操作员Id
        /// </summary>
        public int[] OperatorId { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int OrderByIndex { get; set; }
    }
    #endregion

    #region 返佣支付信息业务实体
    /// <summary>
    /// 返佣支付信息业务实体
    /// </summary>
    /// Auhtor:汪奇志 2011-06-07
    public class MPayCommissionInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MPayCommissionInfo() { }

        /// <summary>
        /// 计划编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }        
        /// <summary>
        /// 公司编号（专线）
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 客户单位编号
        /// </summary>
        public int BuyerCompanyId { get; set; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string BuyerCompanyName { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 支付人编号
        /// </summary>
        public int PayerId { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PayTime { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.RefundType PayType { get { return EyouSoft.Model.EnumType.TourStructure.RefundType.财务现付; } }
        /// <summary>
        /// 杂费支出类型
        /// </summary>
        public EyouSoft.Model.EnumType.FinanceStructure.CostType CostType { get { return EyouSoft.Model.EnumType.FinanceStructure.CostType.支出; } }
        /// <summary>
        /// 支出项目
        /// </summary>
        public string CostName { get { return "支付返佣"; } }
        /// <summary>
        /// 项目类型
        /// </summary>
        public int ItemType { get { return 2; } }
    }
    #endregion
}
