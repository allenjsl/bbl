using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.StatisticStructure
{
    #region 统计分析-现金流量分析实体
    /// <summary>
    /// 统计分析-现金流量分析实体
    /// </summary>
    /// 鲁功源 2011-01-21
    public class CompanyCash
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompanyCash() { }
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
        /// 现金储备
        /// </summary>
        public decimal CashReserve
        {
            get;
            set;
        }
        /// <summary>
        /// 现金收入
        /// </summary>
        public decimal CashIn
        {
            get;
            set;
        }
        /// <summary>
        /// 现金支出
        /// </summary>
        public decimal CashOut
        {
            get;
            set;
        }
        /// <summary>
        /// 当前现金量
        /// </summary>
        public decimal CurrCash
        {
            get 
            {
                return this.CashIn + this.CashReserve - this.CashOut;
            }
        }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime IssueTime
        {
            get;
            set;
        }
        #endregion
    }
    #endregion

    #region 统计分析-现金流量分析查询实体
    /// <summary>
    /// 统计分析-现金流量分析查询实体
    /// </summary>
    public class QueryCompanyCash
    {
        /// <summary>
        /// 统计类型[按日，按月]
        /// </summary>
        public EyouSoft.Model.EnumType.StatisticStructure.StatisticType StatisticType
        {
            get;
            set;
        }
        /// <summary>
        /// 查询日期
        /// </summary>
        public DateTime? QueryDate
        {
            get;
            set;
        }
        /// <summary>
        /// 排序索引[1:升序 0:降序]
        /// </summary>
        public int OrderIndex
        {
            get;
            set;
        }
    }
    #endregion
}
