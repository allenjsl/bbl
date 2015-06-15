//汪奇志 2012-08-24 团款收入相关信息业务实体
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.FinanceStructure
{
    #region 财务管理-团款收入-批量审核列表信息业务实体
    /// <summary>
    /// 财务管理-团款收入-批量审核列表信息业务实体
    /// </summary>
    public class MLBShouKuanShenHeInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MLBShouKuanShenHeInfo() { }

        /// <summary>
        /// 收款登记编号
        /// </summary>
        public string SKDengJiId { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string KeHuMingCheng { get; set; }
        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime SKRiQi { get; set; }
        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal SKJinE { get; set; }
        /// <summary>
        /// 收款方式
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.RefundType SKFangShi { get; set; }
        /// <summary>
        /// 是否审核
        /// </summary>
        public bool SKStatus { get; set; }
        /// <summary>
        /// 收款备注
        /// </summary>
        public string SKBeiZhu { get; set; }
        /// <summary>
        /// 收款人姓名
        /// </summary>
        public string SKRenName { get; set; }
    }

    /// <summary>
    /// 财务管理-团款收入-批量审核列表查询信息业务实体
    /// </summary>
    public class MLBShouKuanShenHeSearchInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MLBShouKuanShenHeSearchInfo() { }

        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string KeHuMingCheng { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 出团起始时间
        /// </summary>
        public DateTime? CTSTime { get; set; }
        /// <summary>
        /// 出团截止时间
        /// </summary>
        public DateTime? CTETime { get; set; }
        /// <summary>
        /// 收款起始时间
        /// </summary>
        public DateTime? SKSTime { get; set; }
        /// <summary>
        /// 收款截止时间
        /// </summary>
        public DateTime? SKETime { get; set; }
        /// <summary>
        /// 收款状态
        /// </summary>
        public bool? SKStatus { get; set; }
        /// <summary>
        /// 统计订单方式
        /// </summary>
        public Model.EnumType.CompanyStructure.ComputeOrderType TongJiDingDanFangShi { get; set; }
    }
    #endregion
}
