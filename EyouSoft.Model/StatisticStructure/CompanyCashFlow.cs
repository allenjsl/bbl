using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.StatisticStructure
{
    /// <summary>
    /// 统计分析-现金流量明细实体
    /// </summary>
    /// 鲁功源 2011-01-21
    public class CompanyCashFlow
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompanyCashFlow() { }
        #endregion

        #region 属性
        /// <summary>
        /// 编号
        /// </summary>
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal CashReserve
        {
            get;
            set;
        }
        /// <summary>
        /// 操作人
        /// </summary>
        public int OperatorId
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime
        {
            get;
            set;
        }
        /// <summary>
        /// 现金流量类型
        /// </summary>
        public EyouSoft.Model.EnumType.StatisticStructure.CashType CashType
        {
            get;
            set;
        }
        #endregion
    }
}
