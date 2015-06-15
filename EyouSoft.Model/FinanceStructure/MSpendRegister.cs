/************************************************************
 * 模块名称：支出销帐相关实体
 * 功能说明：支出销帐相关实体
 * 创建人：周文超  2011-4-28 15:22:15
 * *********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.FinanceStructure
{
    #region 支出登账实体

    /// <summary>
    /// 支出登账实体
    /// </summary>
    public class MSpendRegister
    {
        /// <summary>
        /// 支出登帐编号
        /// </summary>
        public int RegisterId { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 支出时间
        /// </summary>
        public DateTime PayTime { get; set; }

        /// <summary>
        /// 支出金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 支出方式
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.RefundType PayType { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        public int SupplierId { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 供应商联系人姓名
        /// </summary>
        public string Realname { get; set; }

        /// <summary>
        /// 供应商联系人电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 操作员编号
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 已销账金额
        /// </summary>
        public decimal OffAmount { get; set; }
    }

    #endregion

    #region 支出销帐实体

    /// <summary>
    /// 支出销帐实体
    /// </summary>
    public class MSpendRegisterDetail
    {
        /// <summary>
        /// 销帐金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 支出项目编号
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 支出项目类型
        /// </summary>
        public Model.EnumType.StatisticStructure.PaidType ItemType { get; set; }

    }

    #endregion

    #region 支出登帐查询实体

    /// <summary>
    /// 支出登帐查询实体
    /// </summary>
    public class MQuerySpendRegister
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        public int SupplierId { get; set; }

        /// <summary>
        /// 支出方式
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.RefundType? PayType { get; set; }

        /// <summary>
        /// 支出开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 支出结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 排序索引  0/1：到款时间升/降序
        /// </summary>
        public int OrderIndex { get; set; }
    }

    #endregion

    #region 付款批量登记信息业务实体
    /// <summary>
    /// 付款批量登记信息业务实体
    /// </summary>
    public class MBatchRegisterExpenseInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MBatchRegisterExpenseInfo() { }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PaymentTime { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.RefundType PaymentType { get; set; }
        /// <summary>
        /// 付款人编号
        /// </summary>
        public int PayerId { get; set; }
        /// <summary>
        /// 付款人姓名
        /// </summary>
        public string Payer { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /*/// <summary>
        /// 支出类型，为null时关联所有支出
        /// </summary>
        public EyouSoft.Model.EnumType.FinanceStructure.OutPlanType? ExpenseType { get; set; }*/
        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 计划编号集合
        /// </summary>
        public IList<string> TourIds { get; set; }
        /// <summary>
        /// 查询-供应商名称
        /// </summary>
        public string SearchGYSName { get; set; }
        /// <summary>
        /// 查询-供应商类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.SupplierType? SearchGYSType { get; set; }
    }
    #endregion
}
