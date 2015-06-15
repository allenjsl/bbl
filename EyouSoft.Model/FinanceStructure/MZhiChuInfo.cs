//汪奇志 2012-08-26 团款支出相关信息业务实体
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.FinanceStructure
{
    #region 财务管理-团款支出-按计调项显示支出列表信息业务实体
    /// <summary>
    /// 财务管理-团款支出-按计调项显示支出列表信息业务实体
    /// </summary>
    public class MLBJiDiaoZhiChuInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MLBJiDiaoZhiChuInfo() { }

        /// <summary>
        /// 计调安排编号
        /// </summary>
        public string AnPaiId { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 出团时间
        /// </summary>
        public DateTime CTTime { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 团队类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType TourType { get; set; }
        /// <summary>
        /// 支出类别
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.SupplierType ZhiChuLeiBie { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string GYSName { get; set; }
        /// <summary>
        /// 支出金额
        /// </summary>
        public decimal ZhiChuJinE { get; set; }
        /// <summary>
        /// 已支付金额
        /// </summary>
        public decimal YiZhiFuJinE { get; set; }
        /// <summary>
        /// 已登记金额
        /// </summary>
        public decimal YiDengJiJinE { get; set; }
        /// <summary>
        /// 未支付金额
        /// </summary>
        public decimal WeiZhiFuJinE { get { return ZhiChuJinE - YiZhiFuJinE; } }
        /// <summary>
        /// 未登记金额
        /// </summary>
        public decimal WeiDengJiJinE { get { return ZhiChuJinE - YiDengJiJinE; } }
        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 供应商编号
        /// </summary>
        public int GYSId { get; set; }
    }

    /// <summary>
    /// 财务管理-团款支出-按计调项显示支出列表查询信息业务实体
    /// </summary>
    public class MLBJiDiaoZhiChuSearchInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MLBJiDiaoZhiChuSearchInfo() { }

        /// <summary>
        /// 团队类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType? TourType { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 出团起始时间
        /// </summary>
        public DateTime? CTSTime { get; set; }
        /// <summary>
        /// 出团截止时间
        /// </summary>
        public DateTime? CTETime { get; set; }
        /// <summary>
        /// 支出类别
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.SupplierType? ZhiChuLeiBie { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string GYSName { get; set; }
    }
    #endregion

    #region 财务管理-团款支出-按计调类型批量支出登记信息业务实体
    /// <summary>
    /// 财务管理-团款支出-按计调类型批量支出登记信息业务实体
    /// </summary>
    public class MZhiChuPiLiangDengJiInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MZhiChuPiLiangDengJiInfo() { }

        /// <summary>
        /// 付款时间
        /// </summary>
        public DateTime FKTime { get; set; }
        /// <summary>
        /// 付款人编号
        /// </summary>
        public int FKRenId { get; set; }
        /// <summary>
        /// 付款人姓名
        /// </summary>
        public string FKRenName { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.RefundType FKFangShi { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string BeiZhu { get; set; }
        /// <summary>
        /// 计调安排编号集合
        /// </summary>
        public string[] AnPaiIds { get; set; }
    }
    #endregion
}
