using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SupplierStructure
{
    #region 地接供应商交易情况合计信息业务实体
    /// <summary>
    /// 地接供应商交易情况合计信息业务实体
    /// </summary>
    /// 汪奇志 2011-06-20
    public class MTimesSummaryDiJieInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MTimesSummaryDiJieInfo() { }

        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleNumber { get; set; }
        /// <summary>
        /// 返利
        /// </summary>
        public decimal CommAmount { get; set; }
        /// <summary>
        /// 结算费用
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 已支付金额
        /// </summary>
        public decimal PayAmount { get; set; }
        /// <summary>
        /// 未支付金额
        /// </summary>
        public decimal NotPayAmount { get { return this.TotalAmount - this.PayAmount; } }
    }
    #endregion

    #region 票务供应商交易情况合计信息业务实体
    /// <summary>
    /// 票务供应商交易情况合计信息业务实体
    /// </summary>
    /// 汪奇志 2011-06-20
    public class MTimesSummaryJiPiaoInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MTimesSummaryJiPiaoInfo() { }

        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleNumber { get; set; }
        /// <summary>
        /// 票款
        /// </summary>
        public decimal TicketAmount { get; set; }
        /// <summary>
        /// 代理费
        /// </summary>
        public decimal AgencyAmount { get; set; }
        /// <summary>
        /// 总费用
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 已支付金额
        /// </summary>
        public decimal PayAmount { get; set; }
        /// <summary>
        /// 未支付金额
        /// </summary>
        public decimal NotPayAmount { get { return this.TotalAmount - this.PayAmount; } }
    }
    #endregion
}
