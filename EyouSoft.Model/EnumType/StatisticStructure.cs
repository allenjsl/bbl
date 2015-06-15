using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.EnumType
{
    /// <summary>
    /// 统计分析-现金流量枚举
    /// </summary>
    /// 鲁功源 2011-01-21
    public class StatisticStructure
    {
        /// <summary>
        /// 现金流量类型
        /// </summary>
        public enum CashType
        { 
            /// <summary>
            /// 收入
            /// </summary>
            收入=1,
            /// <summary>
            /// 支出
            /// </summary>
            支出=2,
            /// <summary>
            /// 现金储备
            /// </summary>
            现金储备=3
        }
        /// <summary>
        /// 现金流量统计类型
        /// </summary>
        public enum StatisticType
        { 
            /// <summary>
            /// 按日统计
            /// </summary>
            按日统计=1,
            /// <summary>
            /// 按月统计
            /// </summary>
            按月统计=2,
            /// <summary>
            /// 按区域统计
            /// </summary>
            按区域统计=3,
            /// <summary>
            /// 按部门统计
            /// </summary>
            按部门统计=4,
            /// <summary>
            /// 按时间统计
            /// </summary>
            按时间统计=5,
            /// <summary>
            /// 按收入统计
            /// </summary>
            按收入统计=6,
            /// <summary>
            /// 按利润统计
            /// </summary>
            按利润统计=7,
            /// <summary>
            /// 按类型统计
            /// </summary>
            按类型统计=8
        }
        /// <summary>
        /// 支出类型
        /// </summary>
        public enum PaidType
        { 
            /// <summary>
            /// 地接支出
            /// </summary>
            地接支出=1,
            /// <summary>
            /// 机票支出
            /// </summary>
            机票支出=2,
            /// <summary>
            /// 杂费支出
            /// </summary>
            杂费支出=3,
            /// <summary>
            /// 单项服务供应商安排
            /// </summary>
            单项服务供应商安排=4
        }
    }
}
