using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.TourStructure
{
    /// <summary>
    /// 财务管理-应收账款（已收帐款）团队信息实体
    /// Create:luofx   Date:2011-01-129
    /// </summary>
    public class TourAboutAccountInfo
    {
        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo { get; set; }
        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime LeaveDate { get; set; }
        /// <summary>
        /// 团队状态
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourStatus TourStatus { get; set; }
        /// <summary>
        /// 团队类型：散拼计划、团队计划、单项服务
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType TourType { get; set; }
        /// <summary>
        /// 订单信息集合
        /// </summary>
        public IList<OrderAboutAccount> OrderAccountList { get; set; }
    }
    /// <summary>
    /// 财务管理-应收账款（已收帐款）订单相关实体
    /// Create:luofx   Date:2011-01-129
    /// </summary>
    public class OrderAboutAccount
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public int PepoleNum { get; set; }
        /// <summary>
        /// 客源单位名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 销售员
        /// </summary>
        public string SalerName { get; set; }
        /// <summary>
        /// 应收账款
        /// </summary>
        public decimal FinanceSum { get; set; }
        /// <summary>
        /// 待审核帐款
        /// </summary>
        public decimal NotCheckMoney { get; set; }
        /// <summary>
        /// 已收帐款
        /// </summary>
        public decimal HasCheckMoney { get; set; }
        /// <summary>
        /// 未收帐款
        /// </summary>
        public decimal NotReciveMoney { get; set; }
        /// <summary>
        /// 未审核退款
        /// </summary>
        public decimal NotCheckTuiMoney { get; set; }
        /// <summary>
        /// 订单增加减少费用信息
        /// </summary>
        public TourOrderAmountPlusInfo OrderAmountPlusInfo { get; set; }
        /// <summary>
        /// 同行操作人姓名
        /// </summary>
        public string BuyerContactName { get; set; }
        /// <summary>
        /// 对方团号
        /// </summary>
        public string BuyerTourCode { get; set; }

    }
    /// <summary>
    /// 财务订单帐款搜索实体
    /// </summary>
    public class OrderAboutAccountSearchInfo
    {
        /// <summary>
        /// 当前登录人公司ID
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 出团日期开始
        /// </summary>
        public DateTime? LeaveDateFrom { get; set; }
        /// <summary>
        /// 出团日期结束
        /// </summary>
        public DateTime? LeaveDateTo { get; set; }
        /// <summary>
        /// 客源单位名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 销售员Id
        /// </summary>
        public int[] SalerId { get; set; }
        /// <summary>
        /// 登记时间开始
        /// </summary>
        public DateTime? CreateDateFrom { get; set; }
        /// <summary>
        /// 登记时间结束
        /// </summary>
        public DateTime? CreateDateTo { get; set; }

        /// <summary>
        /// 统计订单方式
        /// </summary>
        public Model.EnumType.CompanyStructure.ComputeOrderType ComputeOrderType { get; set; }
        /// <summary>
        /// 收款登记状态 1:收款未审批 2.退款待审
        /// </summary>
        public int? RegisterStatus { get; set; }
        /// <summary>
        /// 款项操作符查询-查询类型
        /// </summary>
        public EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType? QueryAmountType { get; set; }
        /// <summary>
        /// 款项操作符查询-操作符
        /// </summary>
        public EyouSoft.Model.EnumType.FinanceStructure.QueryOperator? QueryAmountOperator { get; set; }
        /// <summary>
        /// 款项操作符查询-操作数
        /// </summary>
        public decimal? QueryAmount { get; set; }
        /// <summary>
        /// 排序类型 0:出团时间升序 1:出团时间降序
        /// </summary>
        public int SortType { get; set; }
        /// <summary>
        /// 计划类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType? TourType { get; set; }
    }
}
