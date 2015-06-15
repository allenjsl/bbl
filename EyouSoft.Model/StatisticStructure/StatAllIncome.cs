using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.StatisticStructure
{
    #region 所有收入明细实体基类
    /// <summary>
    /// 所有收入明细实体基类
    /// </summary>
    /// 鲁功源 2011-01-23
    public class StatAllIncome
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public StatAllIncome() { }
        #endregion

        #region 成员属性
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
        /// 团队编号
        /// </summary>
        public string TourId
        {
            get;
            set;
        }
        /// <summary>
        /// 线路区域编号
        /// </summary>
        public int AreaId
        {
            get;
            set;
        }
        /// <summary>
        /// 团队类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType TourType
        {
            get;
            set;
        }
        /// <summary>
        /// 项目编号
        /// </summary>
        public string ItemId
        {
            get;
            set;
        }
        /// <summary>
        /// 项目类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.ItemType ItemType
        {
            get;
            set;
        }
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 增加费用
        /// </summary>
        public decimal AddAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 减少费用
        /// </summary>
        public decimal ReduceAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal TotalAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 销售员编号
        /// </summary>
        public int OperatorId
        {
            get;
            set;
        }
        /// <summary>
        /// 部门编号
        /// </summary>
        public int DepartmentId
        {
            get;
            set;
        }
        /// <summary>
        /// 已收已审核金额
        /// </summary>
        public decimal CheckAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 已收未审核金额
        /// </summary>
        public decimal NotCheckAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get;
            set;
        }
        #endregion

        #region 附加属性
        /// <summary>
        /// 销售员名称[应用端不需赋值]
        /// </summary>
        public string OperatorName
        {
            get;
            set;
        }
        #endregion
    }
    #endregion

    #region 所有收入明细列表实体
    /// <summary>
    /// 所有收入明细列表实体
    /// </summary>
    public class StatAllIncomeList
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public StatAllIncomeList() { }
        #endregion

        #region 属性
        /// <summary>
        /// 销售员编号
        /// </summary>
        public int OperatorId
        {
            get;
            set;
        }
        /// <summary>
        /// 销售员名称
        /// </summary>
        public string OperatorName
        {
            get;
            set;
        }
        /// <summary>
        /// 合计应收金额
        /// </summary>
        public decimal TotalAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 已收金额
        /// </summary>
        public decimal AccountAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 未收金额
        /// </summary>
        public decimal NotAccountAmount
        {
            get
            {
                return TotalAmount - AccountAmount;
            }
        }
        #endregion
    }
    #endregion

    #region 收入项目编号、类型实体

    /// <summary>
    /// 收入项目编号、类型实体
    /// </summary>
    public class IncomeItemIdAndType
    {
        /// <summary>
        /// 收入项目编号
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 收入项目类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.ItemType ItemType { get; set; }
    }

    #endregion 
}
