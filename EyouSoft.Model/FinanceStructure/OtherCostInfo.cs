/*Author:汪奇志 2011-01-19*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.FinanceStructure
{
    #region 杂费收入信息业务实体
    /// <summary>
    /// 杂费收入信息业务实体
    /// </summary>
    public class OtherIncomeInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public OtherIncomeInfo() { }

        /// <summary>
        /// 收入编号
        /// </summary>
        public string IncomeId { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string CustromCName { get; set; }
        /// <summary>
        /// 客户单位编号
        /// </summary>
        public int CustromCId { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        public string Item { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 增加费用
        /// </summary>
        public decimal AddAmount { get; set; }
        /// <summary>
        /// 减少费用
        /// </summary>
        public decimal ReduceAmount { get; set; }
        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 收/付款日期
        /// </summary>
        public DateTime PayTime { get; set; }

        /// <summary>
        /// 收/付款人
        /// </summary>
        public string Payee { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public Model.EnumType.TourStructure.RefundType PayType { get; set; }

        /// <summary>
        /// 增加减少费用信息集合，string.IsNullOrEmpty(ItemId)==true视为新增操作 ==false视为修改操作
        /// </summary>
        public IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> GoldDetails { get; set; }

        /// <summary>
        /// 是否已收（已收：1；未收：0）
        /// </summary>
        public bool Status { get; set; }
    }
    #endregion

    #region 杂费支出信息业务实体
    /// <summary>
    /// 杂费支出信息业务实体
    /// </summary>
    public class OtherOutInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public OtherOutInfo() { }

        /// <summary>
        /// 支出编号
        /// </summary>
        public string OutId { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string CustromCName { get; set; }
        /// <summary>
        /// 客户单位编号
        /// </summary>
        public int CustromCId { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        public string Item { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 增加费用
        /// </summary>
        public decimal AddAmount { get; set; }
        /// <summary>
        /// 减少费用
        /// </summary>
        public decimal ReduceAmount { get; set; }
        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 收/付款日期
        /// </summary>
        public DateTime PayTime { get; set; }
        /// <summary>
        /// 收/付款人
        /// </summary>
        public string Payee { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public Model.EnumType.TourStructure.RefundType PayType { get; set; }
        /// <summary>
        /// 增加减少费用信息集合，string.IsNullOrEmpty(ItemId)==true视为新增操作 ==false视为修改操作
        /// </summary>
        public IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> GoldDetails { get; set; }

        /// <summary>
        /// 是否已付（已付：1；未付：0）
        /// </summary>
        public bool Status { get; set; }
    }
    #endregion

    #region 团队核算修改杂费明细实体

    /// <summary>
    /// 团队核算修改杂费明细实体
    /// </summary>
    public class UpdateOtherCostInfo
    {
        /// <summary>
        /// 杂费收入支出Id
        /// </summary>
        public string OtherCostId { get; set; }

        /// <summary>
        /// 增加费用
        /// </summary>
        public decimal IncreaseCost { get; set; }

        /// <summary>
        /// 减少费用
        /// </summary>
        public decimal ReduceCost { get; set; }

        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal SumCost { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    #endregion

    #region 杂费收入、支出信息查询实体

    /// <summary>
    /// 杂费收入、支出信息查询实体
    /// </summary>
    public class OtherCostQuery
    {
        /// <summary>
        /// 专线公司Id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 团队Id
        /// </summary>
        public string TourId { get; set; }

        /// <summary>
        /// 项目名称（收入、支出）
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 收/付款人
        /// </summary>
        public string Payee { get; set; }

        /// <summary>
        /// 收/付款开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 收/付款结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 排序索引（0/1：操作时间升/降序）
        /// </summary>
        public int OrderIndex { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 出团起始时间
        /// </summary>
        public DateTime? LSDate { get;set; }
        /// <summary>
        /// 出团截止时间
        /// </summary>
        public DateTime? LEDate { get; set; }
    }

    #endregion

    #region 支出登记明细信息业务实体
    /// <summary>
    /// 支出登记明细信息业务实体
    /// </summary>
    public class OutRegisterInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public OutRegisterInfo() { }

        /// <summary>
        /// 支出明细Id
        /// </summary>
        public string RegisterId { get; set; }

        /// <summary>
        /// 所属公司ID
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public Model.EnumType.FinanceStructure.OutRegisterType RegisterType { get; set; }

        /// <summary>
        /// 计调项目编号
        /// </summary>
        public string ReceiveId { get; set; }

        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 计调项目类型
        /// </summary>
        public Model.EnumType.FinanceStructure.OutPlanType ReceiveType { get; set; }

        /// <summary>
        /// 收款单位编号
        /// </summary>
        public int ReceiveCompanyId { get; set; }

        /// <summary>
        /// 收款单位名称
        /// </summary>
        public string ReceiveCompanyName { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// 付款人编号
        /// </summary>
        public int StaffNo { get; set; }

        /// <summary>
        /// 付款人名称
        /// </summary>
        public string StaffName { get; set; }

        /// <summary>
        /// 付款金额
        /// </summary>
        public decimal PaymentAmount { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public Model.EnumType.TourStructure.RefundType PaymentType { get; set; }

        /// <summary>
        /// 是否开票(默认0)
        /// </summary>
        public bool IsBill { get; set; }

        /// <summary>
        /// 开票金额
        /// </summary>
        public decimal BillAmount { get; set; }

        /// <summary>
        /// 发票收据号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// 操作员ID
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { get; set; }

        /// <summary>
        /// 审核人编号
        /// </summary>
        public int CheckerId { get; set; }

        /// <summary>
        /// 是否支付
        /// </summary>
        public bool IsPay { get; set; }
    }
    #endregion

    #region 支出明细查询实体

    /// <summary>
    /// 支出明细查询实体
    /// </summary>
    public class QueryOutRegisterInfo
    {
        /// <summary>
        /// 所属公司Id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public Model.EnumType.FinanceStructure.OutRegisterType? RegisterType { get; set; }

        /// <summary>
        /// 计调项目编号
        /// </summary>
        public string ReceiveId { get; set; }

        /// <summary>
        /// 计调项目类型
        /// </summary>
        public Model.EnumType.FinanceStructure.OutPlanType? ReceiveType { get; set; }

        /// <summary>
        /// 收款单位编号
        /// </summary>
        public int ReceiveCompanyId { get; set; }

        /// <summary>
        /// 收款单位名称
        /// </summary>
        public string ReceiveCompanyName { get; set; }

        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo { get; set; }

        /// <summary>
        /// 申请人Id
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 申请开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 申请结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 申请固定时间
        /// </summary>
        public DateTime? FTime { get; set; }

        /// <summary>
        /// 排序索引 0/1：付款日期升/降序
        /// </summary>
        public int OrderIndex { get; set; }
        /// <summary>
        /// 支出登记状态 1:未审批 2:未支付
        /// </summary>
        public int? RegisterStatus { get; set; }
    }

    #endregion

    #region 收入收退款登记明细信息实体
    /// <summary>
    /// 收入收退款登记明细信息实体
    /// </summary>
    /// 创建人：luofx 2011-01-19
    public class ReceiveRefund
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { set; get; }
        /// <summary>
        /// 所属公司ID
        /// </summary>
        public int CompanyID { set; get; }
        /// <summary>
        /// 付款单位编号
        /// </summary>
        public int? PayCompanyId { set; get; }
        /// <summary>
        /// 付款单位名称
        /// </summary>
        public string PayCompanyName { set; get; }
        /// <summary>
        /// 关联编号（订单编号，团款其他收入支出编号）
        /// </summary>
        public string ItemId { set; get; }
        /// <summary>
        /// 关联类型（订单编号，团款其他收入支出编号）
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.ItemType ItemType { set; get; }
        /// <summary>
        /// 收款（退款）日期
        /// </summary>
        public DateTime? RefundDate { set; get; }
        /// <summary>
        /// 收款（退款）人编号(员工编号)
        /// </summary>
        public int StaffNo { set; get; }
        /// <summary>
        /// 收款（退款）人
        /// </summary>
        public string StaffName { set; get; }
        /// <summary>
        /// 收款（退款）金额
        /// </summary>
        public decimal RefundMoney { set; get; }
        /// <summary>
        /// 收款（退款）方式
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.RefundType RefundType { set; get; }
        /// <summary>
        /// 是否开票(true=开票，false=不开票)
        /// </summary>
        public bool IsBill { set; get; }
        /// <summary>
        /// 开票金额
        /// </summary>
        public decimal BillAmount { set; get; }
        /// <summary>
        /// 发票收据号
        /// </summary>
        //public string BillNo { set; get; }
        /// <summary>
        /// 收款（团队收入）/退款(true=收款/收入，false=退款)
        /// </summary>
        public bool IsReceive { set; get; }
        /// <summary>
        /// 是否审核(true=审核，false=未审核)
        /// </summary>
        public bool IsCheck { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// 操作员ID
        /// </summary>
        public int OperatorID { set; get; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? IssueTime { set; get; }
        /// <summary>
        /// 审核人Id
        /// </summary>
        public int? CheckerId { set; get; }
    }
    #endregion

}
