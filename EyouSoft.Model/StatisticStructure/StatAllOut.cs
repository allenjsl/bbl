using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.StatisticStructure
{
    #region 所有支出明细实体
    /// <summary>
    /// 所有支出明细实体基类
    /// </summary>
    /// 鲁功源 2011-01-23
    public class StatAllOut
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public StatAllOut() { }
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
        public EyouSoft.Model.EnumType.StatisticStructure.PaidType ItemType
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
        /// 已确认支付金额
        /// </summary>
        public decimal CheckAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 未确认支付金额
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
        /// <summary>
        /// 供应商编号
        /// </summary>
        public int SupplierId
        {
            get;
            set;
        }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get;
            set;
        }
        #endregion
    }
    #endregion

    #region 所有支出明细列表实体
    public class StatAllOutList
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public StatAllOutList() { }
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
        /// 合计应付金额
        /// </summary>
        public decimal TotalAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 已付金额
        /// </summary>
        public decimal PaidAmount
        {
            get;
            set;
        }
        /// <summary>
        /// 未付金额
        /// </summary>
        public decimal NotPaidAmount
        {
            get
            {
                return TotalAmount - PaidAmount;
            }
        }
        #endregion
    }
    #endregion

    #region 支出项目编号、类型实体

    /// <summary>
    /// 支出项目编号、类型实体
    /// </summary>
    public class ItemIdAndType
    {
        /// <summary>
        /// 支出项目编号
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 支出项目类型
        /// </summary>
        public EyouSoft.Model.EnumType.StatisticStructure.PaidType ItemType { get; set; }
    }

    #endregion
}
