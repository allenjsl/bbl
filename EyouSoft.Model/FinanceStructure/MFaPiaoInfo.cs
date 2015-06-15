//发票管理相关信息业务实体 汪奇志 2012-08-17
using System;

namespace EyouSoft.Model.FinanceStructure
{
    #region 发票登记信息业务实体
    /// <summary>
    /// 发票登记信息业务实体
    /// </summary>
    public class MFaPiaoInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MFaPiaoInfo() { }

        /// <summary>
        /// 登记编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 客户单位编号
        /// </summary>
        public int CrmId { get; set; }
        /// <summary>
        /// 开票日期
        /// </summary>
        public DateTime RiQi { get; set; }
        /// <summary>
        /// 开票金额
        /// </summary>
        public decimal JinE { get; set; }
        /// <summary>
        /// 票号
        /// </summary>
        public string PiaoHao { get; set; }
        /// <summary>
        /// 开票人编号
        /// </summary>
        public int KaiPiaoRenId { get; set; }
        /// <summary>
        /// 开票人姓名
        /// </summary>
        public string KaiPiaoRen { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string BeiZhu { get; set; }
        /// <summary>
        /// 操作人编号
        /// </summary>
        public int CaoZuoRenId { get; set; }
    }
    #endregion

    #region 发票登记查询信息业务实体
    /// <summary>
    /// 发票登记查询信息业务实体
    /// </summary>
    public class MFaPiaoSearchInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MFaPiaoSearchInfo() { }

        /// <summary>
        /// 开票起始时间
        /// </summary>
        public DateTime? KPSTime { get; set; }
        /// <summary>
        /// 开票截止时间
        /// </summary>
        public DateTime? KPETime { get; set; }
        /// <summary>
        /// 开票人姓名
        /// </summary>
        public string KPRen { get; set; }
    }
    #endregion

    #region 财务管理-发票管理列表信息业务实体
    /// <summary>
    /// 财务管理-发票管理列表信息业务实体
    /// </summary>
    public class MFaPiaoGuanLiInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MFaPiaoGuanLiInfo() { }

        /// <summary>
        /// 客户单位编号
        /// </summary>
        public int CrmId { get; set; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string CrmName { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal JiaoYiJinE { get; set; }
        /// <summary>
        /// 开票金额
        /// </summary>
        public decimal KaiPiaoJinE { get; set; }
        /// <summary>
        /// 未开票金额
        /// </summary>
        public decimal WeiKaiPiaoJinE { get { return JiaoYiJinE - KaiPiaoJinE; } }
    }
    #endregion

    #region 财务管理-发票管理列表查询信息业务实体
    /// <summary>
    /// 财务管理-发票管理列表查询信息业务实体
    /// </summary>
    public class MFaPiaoGuanLiSearchInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MFaPiaoGuanLiSearchInfo() { }

        /// <summary>
        /// 出团起始时间
        /// </summary>
        public DateTime? CTSTime { get; set; }
        /// <summary>
        /// 出团截止时间
        /// </summary>
        public DateTime? CTETime { get; set; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string CrmName { get; set; }
        /// <summary>
        /// 开票起始时间
        /// </summary>
        public DateTime? KPSTime { get; set; }
        /// <summary>
        /// 开票截止时间
        /// </summary>
        public DateTime? KPETime { get; set; }
        /// <summary>
        /// 开票人姓名
        /// </summary>
        public string KPRen { get; set; }
    }
    #endregion
}
