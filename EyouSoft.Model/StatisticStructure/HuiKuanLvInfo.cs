using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.StatisticStructure
{
    #region 回款率分析信息业务实体
    /// <summary>
    /// 回款率分析信息业务实体
    /// </summary>
    /// 汪奇志 2012-02-27
    public class HuiKuanLvInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public HuiKuanLvInfo() { }

        /// <summary>
        /// 应收款
        /// </summary>
        public decimal YingShouKuan { get; set; }
        /// <summary>
        /// 已收款
        /// </summary>
        public decimal YiShouKuan { get; set; }
        /// <summary>
        /// 回款率
        /// </summary>
        public decimal HuiKuanLv
        {
            get
            {
                if (YingShouKuan == 0)
                {
                    return 1;
                }

                return YiShouKuan / YingShouKuan;
            }
        }
        /// <summary>
        /// 统计时间
        /// </summary>
        public DateTime TongJiShiJian { get; set; }
        /// <summary>
        /// 回款率百分比形式
        /// </summary>
        public string HuiKuanLvBaiFenBi { get { return (HuiKuanLv * 100).ToString("F2") + "%"; } }
        /// <summary>
        /// 未收款
        /// </summary>
        public decimal WeiShouKuan { get { return YingShouKuan - YiShouKuan; } }
    }
    #endregion

    #region 回款率分析查询信息业务实体
    /// <summary>
    /// 回款率分析查询信息业务实体
    /// </summary>
    /// 汪奇志 2012-02-27
    public class HuiKuanLvChaXunInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public HuiKuanLvChaXunInfo() { }

        /// <summary>
        /// 出团起始时间
        /// </summary>
        public DateTime? LSDate { get; set; }
        /// <summary>
        /// 出团截止时间
        /// </summary>
        public DateTime? LEDate { get; set; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string KeHuDanWei { get; set; }
        /// <summary>
        /// 订单销售员编号集合
        /// </summary>
        public int[] OperatorIds { get; set; }
        /// <summary>
        /// 订单销售员所在部门编号集合
        /// </summary>
        public int[] OperatorDepartIds { get; set; }
        /// <summary>
        /// 已收款是否包含未审核的收款登记
        /// </summary>
        public bool SFBHWeiShenHe { get; set; }
        /// <summary>
        /// 统计订单方式
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? TongJiDingDanFangShi { get; set; }
    }
    #endregion
}
