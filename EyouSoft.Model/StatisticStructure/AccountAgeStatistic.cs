using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.StatisticStructure
{
    #region 帐龄分析统计实体

    /// <summary>
    /// 帐龄分析统计实体
    /// </summary>
    public class AccountAgeStatistic
    {
        /// <summary>
        /// 销售员信息
        /// </summary>
        public StatisticOperator SalesClerk { get; set; }

        /// <summary>
        /// 拖欠款总额
        /// </summary>
        public decimal ArrearageSum { get; set; }

        /// <summary>
        /// 最长拖欠时间
        /// </summary>
        public DateTime MaxArrearageTime { get; set; }
    }

    #endregion

    #region 帐龄分析统计查询实体

    /// <summary>
    /// 帐龄分析统计查询实体
    /// </summary>
    public class QueryAccountAgeStatistic
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 供应商编号集合
        /// </summary>
        public int[] BuyCompanyId { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string BuyCompanyName { get; set; }
        
        /// <summary>
        /// 团队类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType? TourType { get; set; }

        /// <summary>
        /// 销售员
        /// </summary>
        public int[] SaleIds { get; set; }

        /// <summary>
        /// 线路区域Id
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// 出团时间起
        /// </summary>
        public DateTime? LeaveDateStart { get; set; }

        /// <summary>
        /// 出团时间止
        /// </summary>
        public DateTime? LeaveDateEnd { get; set; }

        /// <summary>
        /// 统计订单方式
        /// </summary>
        public Model.EnumType.CompanyStructure.ComputeOrderType ComputeOrderType { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        public int OrderIndex { get; set; }

        /// <summary>
        /// 是否已结清（收入、支出对账单用。已结清True；未结清False；null不作为条件）
        /// </summary>
        public bool? IsAccount { get; set; }
    }

    #endregion

    #region 账龄分析-按客户单位统计实体
    /// <summary>
    /// 账龄分析-按客户单位统计实体
    /// </summary>
    /// 汪奇志 2012-02-24
    public class ZhangLingAnKeHuDanWei
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public ZhangLingAnKeHuDanWei() { }

        /// <summary>
        /// 客户单位编号
        /// </summary>
        public int KeHuId { get; set; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string KeHuName { get; set; }
        /// <summary>
        /// 未收款
        /// </summary>
        public decimal WeiShouKuan { get; set; }
        /// <summary>
        /// 最早未收款下单时间
        /// </summary>
        public DateTime EarlyTime { get; set; }
    }
    #endregion

    #region 账龄分析-按客户单位统计查询实体
    /// <summary>
    /// 账龄分析-按客户单位统计查询实体
    /// </summary>
    public class ZhangLingAnKeHuDanWeiChaXun
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public ZhangLingAnKeHuDanWeiChaXun() { }

        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string KeHuName { get; set; }
        /// <summary>
        /// 排序方式 0.未收款降序 1.未收款升序 2.最早未收款下单时间降序 3.最早未收款下单时间升序
        /// </summary>
        public int? SortType { get; set; }
        /// <summary>
        /// 出团起始时间
        /// </summary>
        public DateTime? LSDate { get; set; }
        /// <summary>
        /// 出团截止时间
        /// </summary>
        public DateTime? LEDate { get; set; }
    }
    #endregion

    #region 支出账龄分析信息业务实体
    /// <summary>
    /// 支出账龄分析信息业务实体
    /// </summary>
    /// 汪奇志 2012-02-28
    public class FXZhiChuZhangLingInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public FXZhiChuZhangLingInfo() { }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string GongYingShang { get; set; }
        /// <summary>
        /// 交易总额
        /// </summary>
        public decimal JiaoYiZongE { get; set; }
        /// <summary>
        /// 欠款总额
        /// </summary>
        public decimal QianKuanZongE { get; set; }
        /// <summary>
        /// 最早拖欠时间
        /// </summary>
        public DateTime EarlyTime { get; set; }
        /// <summary>
        /// 供应商类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.SupplierType GongYingShangLeiXing { get; set; }
    }
    #endregion

    #region 支出账龄分析查询信息业务实体
    /// <summary>
    /// 支出账龄分析查询信息业务实体
    /// </summary>
    /// 汪奇志 2012-02-28
    public class FXZhiChuZhangLingChaXunInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public FXZhiChuZhangLingChaXunInfo() { }

        /// <summary>
        /// 出团起始时间
        /// </summary>
        public DateTime? LSDate { get; set; }
        /// <summary>
        /// 出团截止时间
        /// </summary>
        public DateTime? LEDate { get; set; }
        /// <summary>
        /// 计划操作员编号集合
        /// </summary>
        public int[] OperatorIds { get; set; }
        /// <summary>
        /// 订单操作员所在部门编号集合
        /// </summary>
        public int[] OperatorDepartIds { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string GongYingShang { get; set; }
        /// <summary>
        /// 排序类型 0：欠款总额DESC，1：欠款总额ASC，2：最早拖欠时间DESC，3：最早拖欠时间ASC，4：交易总额DESC，5：交易总额ASC
        /// </summary>
        public int SortType { get; set; }
        /// <summary>
        /// 供应商类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.SupplierType? GongYingShangLeiXing { get; set; }
    }
    #endregion
}
