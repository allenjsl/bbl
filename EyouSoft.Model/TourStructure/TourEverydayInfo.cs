using System;
using System.Collections.Generic;

namespace EyouSoft.Model.TourStructure
{
    #region 散客天天发计划信息业务实体
    /// <summary>
    /// 散客天天发计划信息业务实体
    /// </summary>
    /// Author:汪奇志 2011-03-17
    public class TourEverydayInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourEverydayInfo() { }

        /// <summary>
        /// 计划编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 线路区域编号
        /// </summary>
        public int AreaId { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 引用线路编号
        /// </summary>
        public int RouteId { get; set; }
        /// <summary>
        /// 计划天数
        /// </summary>
        public int TourDays { get; set; }
        /// <summary>
        /// 已处理数量
        /// </summary>
        public int HandleNumber { get; set; }
        /// <summary>
        /// 未处理数量
        /// </summary>
        public int UntreatedNumber { get; set; }
        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 计划附件信息集合
        /// </summary>
        public IList<TourAttachInfo> Attachs { get; set; }
        /// <summary>
        /// 报价等级信息集合
        /// </summary>
        public IList<TourPriceStandardInfo> PriceStandards { get; set; }
        /// <summary>
        /// 快速发布计划专有信息
        /// </summary>
        public TourQuickPrivateInfo TourQuickInfo { get; set; }
        /// <summary>
        /// 标准发布计划专有信息
        /// </summary>
        public TourNormalPrivateInfo TourNormalInfo { get; set; }
        /// <summary>
        /// 计划发布类型
        /// </summary>
        public EnumType.TourStructure.ReleaseType ReleaseType { get; set; }
    }
    #endregion

    #region 散客天天发计划申请信息业务实体
    /// <summary>
    /// 散客天天发计划申请信息业务实体
    /// </summary>
    /// Author:汪奇志 2011-03-18
    public class TourEverydayApplyInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourEverydayApplyInfo() { }

        /// <summary>
        /// 申请编号
        /// </summary>
        public string ApplyId { get; set; }
        /// <summary>
        /// 计划编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 申请公司编号
        /// </summary>
        public int ApplyCompanyId { get; set; }
        /// <summary>
        /// 发布计划公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime LDate { get; set; }
        /// <summary>
        /// 成人数
        /// </summary>
        public int AdultNumber { get; set; }
        /// <summary>
        /// 儿童数
        /// </summary>
        public int ChildrenNumber { get; set; }
        /// <summary>
        /// 人数(成人数+儿童数)
        /// </summary>
        public int PeopleNumber { get { return this.AdultNumber + this.ChildrenNumber; } }
        /// <summary>
        /// 报价标准编号
        /// </summary>
        public int StandardId { get; set; }
        /// <summary>
        /// 客户等级编号
        /// </summary>
        public int LevelId { get; set; }
        /// <summary>
        /// 特殊要求说明
        /// </summary>
        public string SpecialRequirement { get; set; }       
        /// <summary>
        /// 申请人编号
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyTime { get; set; }
        /// <summary>
        /// 散客天天发计划申请游客信息
        /// </summary>
        public TourEverydayApplyTravellerInfo Traveller { get; set; }
        /// <summary>
        /// 处理状态
        /// </summary>
        public EnumType.TourStructure.TourEverydayHandleStatus HandleStatus { get; set; }
        /// <summary>
        /// 处理人编号
        /// </summary>
        public int? HandleOperatorId { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? HandleTime { get; set; }
        /// <summary>
        /// 生成的计划编号
        /// </summary>
        public string BuildTourId { get; set; }
    }
    #endregion

    #region 散客天天发计划申请、组团询价游客信息业务实体
    /// <summary>
    /// 散客天天发计划申请、组团询价游客信息业务实体
    /// </summary>
    public class TourEverydayApplyTravellerInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourEverydayApplyTravellerInfo() { }

        /// <summary>
        /// 游客信息体现类型
        /// </summary>
        public EnumType.TourStructure.CustomerDisplayType TravellerDisplayType { set; get; }
        /// <summary>
        /// 游客信息集合
        /// </summary>
        public IList<EyouSoft.Model.TourStructure.TourOrderCustomer> Travellers { set; get; }
        /// <summary>
        /// 游客信息附件
        /// </summary>
        public string TravellerFilePath { get; set; }
    }
    #endregion

    #region 散客天天发计划列表信息业务实体
    /// <summary>
    /// 散客天天发计划列表信息业务实体
    /// </summary>
    /// Author:汪奇志 2011-03-18
    public class LBTourEverydayInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public LBTourEverydayInfo() { }

        /// <summary>
        /// 计划编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 计划发布类型
        /// </summary>
        public EnumType.TourStructure.ReleaseType ReleaseType { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 线路区域名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 计划天数
        /// </summary>
        public int TourDays { get; set; }
        /// <summary>
        /// 已处理数量
        /// </summary>
        public int HandleNumber { get; set; }
        /// <summary>
        /// 未处理数量
        /// </summary>
        public int UntreatedNumber { get; set; }
        /// <summary>
        /// 负责人姓名
        /// </summary>
        public string ContacterName { get; set; }
        /// <summary>
        /// 负责人电话
        /// </summary>
        public string ContacterTelephone { get; set; }
        /// <summary>
        /// 负责人手机
        /// </summary>
        public string ContacterMobile { get; set; }
        /// <summary>
        /// 负责人传真
        /// </summary>
        public string ContacterFax { get; set; }
        /// <summary>
        /// 负责人QQ
        /// </summary>
        public string ContacterQQ { get; set; }
        /// <summary>
        /// 负责人MSN
        /// </summary>
        public string ContacterMSN { get; set; }
        /// <summary>
        /// 负责人邮箱
        /// </summary>
        public string ContacterEmail { get; set; }
        /// <summary>
        /// 门市成人价
        /// </summary>
        public decimal MSAdultPrice { get; set; }
        /// <summary>
        /// 门市儿童价
        /// </summary>
        public decimal MSChildrePricen { get; set; }
        /// <summary>
        /// 同行成人价
        /// </summary>
        public decimal THAdultPrice { get; set; }
        /// <summary>
        /// 同行儿童价
        /// </summary>
        public decimal THChildrenPrice { get; set; }
        /// <summary>
        /// 计划附件信息集合
        /// </summary>
        public IList<TourAttachInfo> Attachs { get; set; }
    }
    #endregion

    #region 散客天天发计划查询信息业务实体
    /// <summary>
    /// 散客天天发计划查询信息业务实体
    /// </summary>
    /// Author:汪奇志 2011-03-18
    public class TourEverydaySearchInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourEverydaySearchInfo() { }

        /// <summary>
        /// 线路区域编号集合(注：组团端查询赋值为分配给组团用户的线路区域)
        /// </summary>
        public int[] Areas { get; set; }
        /// <summary>
        /// 线路区域编号
        /// </summary>
        public int? AreaId { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 计划天数
        /// </summary>
        public int? TourDays { get; set; }
    }
    #endregion

    #region 散客天天发计划处理列表信息业务实体
    /// <summary>
    /// 散客天天发计划处理列表信息业务实体
    /// </summary>
    /// Author:汪奇志 2011-03-18
    public class LBTourEverydayHandleInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public LBTourEverydayHandleInfo() { }

        /// <summary>
        /// 申请编号
        /// </summary>
        public string ApplyId { get; set; }
        /// <summary>
        /// 散客天天发计划编号
        /// </summary>
        public string TourId { get; set; }        
        /// <summary>
        /// 生成的散拼计划编号
        /// </summary>
        public string BuildTourId { get; set; }
        /// <summary>
        /// 生成的散拼计划团号
        /// </summary>
        public string BuildTourCode { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 申请成人数
        /// </summary>
        public int AdultNumber { get; set; }
        /// <summary>
        /// 申请儿童数
        /// </summary>
        public int ChildrenNumber { get; set; }
        /// <summary>
        /// 申请人数(申请成人数+申请儿童数)
        /// </summary>
        public int PeopleNumber { get { return this.AdultNumber + this.ChildrenNumber; } }
        /// <summary>
        /// 报价标准编号
        /// </summary>
        public int StandardId { get; set; }
        /// <summary>
        /// 客户等级编号
        /// </summary>
        public int LevelId { get; set; }
        /// <summary>
        /// 成人价格(对应申请时所选中的报价标准、客户等级的成人价)
        /// </summary>
        public decimal AdultPrice { get; set; }
        /// <summary>
        /// 儿童价格(对应申请时所选中的报价标准、客户等级的儿童价)
        /// </summary>
        public decimal ChildrenPrice { get; set; }
        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime LDate { get; set; }
        /// <summary>
        /// 申请公司（客户单位）名称
        /// </summary>
        public string ApplyCompanyName { get; set; }
        /// <summary>
        /// 申请人联系姓名
        /// </summary>
        public string ApplyContacterName { get; set; }
        /// <summary>
        /// 申请人联系电话
        /// </summary>
        public string ApplyContacterTelephone { get; set; }
        /// <summary>
        /// 计划发布类型
        /// </summary>
        public EnumType.TourStructure.ReleaseType ReleaseType { get; set; }
    }
    #endregion

    #region 散客天天发计划处理查询信息业务实体
    /// <summary>
    /// 散客天天发计划处理查询信息业务实体
    /// </summary>
    /// Author:汪奇志 2011-03-19
    public class TourEverydayHandleApplySearchInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourEverydayHandleApplySearchInfo() { }
    }
    #endregion
}
