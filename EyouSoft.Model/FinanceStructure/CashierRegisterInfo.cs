/*Author:汪奇志 2011-01-19*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.FinanceStructure
{
    #region 出纳登账信息业务实体
    /// <summary>
    /// 出纳登账信息业务实体
    /// </summary>
    public class CashierRegisterInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public CashierRegisterInfo() { }

        /// <summary>
        /// 登记编号
        /// </summary>
        public int RegisterId { get; set; }
        /// <summary>
        /// 到款时间
        /// </summary>
        public DateTime PaymentTime { get; set; }
        /// <summary>
        /// 到款金额
        /// </summary>
        public decimal PaymentCount { get; set; }
        /// <summary>
        /// 到款银行
        /// </summary>
        public string PaymentBank { get; set; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 客户单位编号
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacter { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactTel { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 登记时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 登记人编号
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 已销账金额
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
    #endregion

    #region 出纳登帐查询实体

    /// <summary>
    /// 出纳登帐查询实体
    /// </summary>
    public class QueryCashierRegisterInfo
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 客户单位编号
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 到款银行
        /// </summary>
        public string PaymentBank { get; set; }

        /// <summary>
        /// 到款开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 到款结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 排序索引  0/1：到款时间升/降序
        /// </summary>
        public int OrderIndex { get; set; }
    }

    #endregion

    #region 销帐实体

    /// <summary>
    /// 销帐实体
    /// </summary>
    public class CancelRegistInfo
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }
    }

    #endregion
}
