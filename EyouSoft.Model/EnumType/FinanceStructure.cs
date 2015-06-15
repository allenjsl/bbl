using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.EnumType
{
    /// <summary>
    /// 财务管理-相关枚举
    /// </summary>
    public class FinanceStructure
    {
        /// <summary>
        /// 杂费类型枚举
        /// </summary>
        public enum CostType
        {
            /// <summary>
            /// 杂费收入
            /// </summary>
            收入 = 0,
            /// <summary>
            /// 杂费支出
            /// </summary>
            支出 = 1
        }

        /// <summary>
        /// 收入费用类型
        /// </summary>
        public enum OutRegisterType
        {
            /// <summary>
            /// 团队
            /// </summary>
            团队 = 0,
            /// <summary>
            /// 杂费
            /// </summary>
            杂费 = 1
        }

        /// <summary>
        /// 支出明细计调项类型
        /// </summary>
        public enum OutPlanType
        {
            /// <summary>
            /// 单项服务供应商安排
            /// </summary>
            单项服务供应商安排 = 0,
            /// <summary>
            /// 地接
            /// </summary>
            地接 = 1,

            /// <summary>
            /// 票务
            /// </summary>
            票务 = 2
        }

        /// <summary>
        /// 收入、支出增加减少费用明细信息关系类型
        /// </summary>
        public enum GoldType
        {
            /// <summary>
            /// 订单收入 ItemId=订单编号
            /// </summary>
            订单收入=1,
            /// <summary>
            /// 杂费收入 ItemId=杂费收入编号
            /// </summary>
            杂费收入,
            /// <summary>
            /// 地接支出 ItemId=地接计调安排编号
            /// </summary>
            地接支出,
            /// <summary>
            /// 机票支出 ItemId=机票计调安排编号
            /// </summary>
            机票支出,
            /// <summary>
            /// 杂费支出 ItemId=杂费支出编号
            /// </summary>
            杂费支出,
            /// <summary>
            /// 单项供应商安排 ItemId=单项服务供应商安排编号
            /// </summary>
            单项供应商安排
        }

        /// <summary>
        /// 查询操作符
        /// </summary>
        public enum QueryOperator
        {
            /// <summary>
            /// none
            /// </summary>
            None=0,
            /// <summary>
            /// =
            /// </summary>
            等于,
            /// <summary>
            /// &lt;=
            /// </summary>
            小于等于,
            /// <summary>
            /// &gt;=
            /// </summary>
            大于等于
        }

        /// <summary>
        /// 查询金额类型
        /// </summary>
        public enum QueryAmountType
        {
            /// <summary>
            /// None
            /// </summary>
            None=0,
            /// <summary>
            /// 应收款
            /// </summary>
            应收款,
            /// <summary>
            /// 已收款
            /// </summary>
            已收款,
            /// <summary>
            /// 未收款
            /// </summary>
            未收款,
            /// <summary>
            /// 已收待审核
            /// </summary>
            已收待审核,
            /// <summary>
            /// 退款待审核
            /// </summary>
            退款待审核
        }
    }
}
