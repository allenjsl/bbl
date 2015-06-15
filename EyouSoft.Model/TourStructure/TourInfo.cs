/*Author:汪奇志 2011-01-18*/
using System;
using System.Collections.Generic;

namespace EyouSoft.Model.TourStructure
{
    #region 散拼计划、团队计划基础信息业务实体
    /// <summary>
    /// 散拼计划、团队计划基础信息业务实体
    /// </summary>
    public class TourBaseInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourBaseInfo() { }

        /// <summary>
        /// 团队编号(注：散拼计划写入时为模板团编号)
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 团队类型
        /// </summary>
        public EnumType.TourStructure.TourType TourType { get; set; }
        /// <summary>
        /// 团队状态
        /// </summary>
        public EnumType.TourStructure.TourStatus Status { get; set; }
        /// <summary>
        /// 团队天数
        /// </summary>
        public int TourDays { get; set; }
        /// <summary>
        /// 出团时间[单项服务委托日期]
        /// </summary>
        public DateTime LDate { get; set; }
        /// <summary>
        /// 回团时间
        /// </summary>
        public DateTime RDate { get { return this.LDate.AddDays(this.TourDays - 1); } }
        /// <summary>
        /// 线路区域编号
        /// </summary>
        public int AreaId { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 计划人数
        /// </summary>
        public int PlanPeopleNumber { get; set; }
        /// <summary>
        /// 机票状态
        /// </summary>
        public EnumType.PlanStructure.TicketState TicketStatus { get; set; }
        /// <summary>
        /// 是否成本确认
        /// </summary>
        public bool IsCostConfirm { get; set; }
        /// <summary>
        /// 是否团队复核
        /// </summary>
        public bool IsReview { get; set; }
        /// <summary>
        /// 团队发布类型
        /// </summary>
        public EnumType.TourStructure.ReleaseType ReleaseType { get; set; }
        /// <summary>
        /// 发布人编号
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 团队订单总收入
        /// </summary>
        public decimal TotalIncome { get; set; }
        /// <summary>
        /// 团队计调总支出
        /// </summary>
        public decimal TotalExpenses { get; set; }
        /// <summary>
        /// 团队杂费总收入
        /// </summary>
        public decimal TotalOtherIncome { get; set; }
        /// <summary>
        /// 团队杂费总支出
        /// </summary>
        public decimal TotalOtherExpenses { get; set; }
        /// <summary>
        /// 利润分配总金额
        /// </summary>
        public decimal DistributionAmount { get; set; }
        /// <summary>
        /// 团队毛利
        /// </summary>
        public decimal GrossProfit { get; set; }
        /// <summary>
        /// 去程航班/时间
        /// </summary>
        public string LTraffic { get; set; }
        /// <summary>
        /// 回程航班/时间
        /// </summary>
        public string RTraffic { get; set; }
        /// <summary>
        /// 集合方式
        /// </summary>
        public string Gather { get; set; }
        /// <summary>
        /// 引用的线路编号
        /// </summary>
        public int RouteId { get; set; }
        /// <summary>
        /// 原引用的线路编号(修改计划信息时赋该计划原来引用的线路编号)
        /// </summary>
        public int ORouteId { get; set; }
        /// <summary>
        /// 虚拟实收
        /// </summary>
        public int VirtualPeopleNumber { get; set; }
        /// <summary>
        /// 订单留位人数
        /// </summary>
        public int PeopleNumberLiuWei { get; set; }
        /// <summary>
        /// 订单实收人数
        /// </summary>
        public int PeopleNumberShiShou { get; set; }
        /// <summary>
        /// 订单未处理人数
        /// </summary>
        public int PeopleNumberWeiChuLi { get; set; }
        /// <summary>
        /// 客户单位编号(仅针团队计划或单项服务)
        /// </summary>
        public int BuyerCId { get; set; }
        /// <summary>
        /// 客户单位名称(仅针团队计划或单项服务)
        /// </summary>
        public string BuyerCName { get; set; }
        /// <summary>
        /// 销售员编号(仅针团队计划或单项服务)
        /// </summary>
        public int SellerId { get; set; }
        /// <summary>
        /// 订单编号(仅针团队计划或单项服务)
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 销售员姓名(仅针团队计划或单项服务)
        /// </summary>
        public string SellerName { get; set; }

        /// <summary>
        /// 团队推广状态
        /// </summary>
        public EnumType.TourStructure.TourRouteStatus TourRouteStatus { get; set; }

        /// <summary>
        /// 手动设置的计划状态
        /// </summary>
        public EnumType.TourStructure.HandStatus HandStatus { get; set; }

        /// <summary>
        /// 团队交通列表
        /// </summary>
        public IList<int> TourTraffic { get; set; }

    }
    #endregion

    #region 团队地接社信息业务实体
    /// <summary>
    /// 团队地接社信息业务实体
    /// </summary>
    public class TourLocalAgencyInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourLocalAgencyInfo() { }

        /// <summary>
        /// 地接社编号
        /// </summary>
        public int AgencyId { get; set; }
        /// <summary>
        /// 地接社名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 许可证号
        /// </summary>
        public string LicenseNo { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContacterName { get; set; }
    }
    #endregion

    #region 团队附件信息业务实体
    /// <summary>
    /// 团队附件信息业务实体
    /// </summary>
    public class TourAttachInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourAttachInfo() { }

        /// <summary>
        /// 附件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 附件名称
        /// </summary>
        public string Name { get; set; }
    }
    #endregion

    #region 标准发布团队行程信息业务实体
    /// <summary>
    /// 标准发布团队行程信息业务实体
    /// </summary>
    public class TourPlanInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourPlanInfo() { }

        /// <summary>
        /// 行程区间
        /// </summary>
        public string Interval { get; set; }
        /// <summary>
        /// 交通工具
        /// </summary>
        public string Vehicle { get; set; }
        /// <summary>
        /// 住宿
        /// </summary>
        public string Hotel { get; set; }
        /// <summary>
        /// 用餐
        /// </summary>
        public string Dinner { get; set; }
        /// <summary>
        /// 行程内容
        /// </summary>
        public string Plan { get; set; }
        /// <summary>
        /// 行程附件
        /// </summary>
        public string FilePath { get; set; }
    }
    #endregion

    #region 快速发布计划专有信息业务实体
    /// <summary>
    /// 快速发布计划专有信息业务实体
    /// </summary>
    public class TourQuickPrivateInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourQuickPrivateInfo() { }

        /// <summary>
        /// 行程内容
        /// </summary>
        public string QuickPlan { get; set; }
        /// <summary>
        /// 服务标准
        /// </summary>
        public string Service { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
    #endregion

    #region 散拼计划包含项目信息业务实体
    /// <summary>
    /// 散拼计划包含项目信息业务实体
    /// </summary>
    public class TourServiceInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourServiceInfo() { }

        /// <summary>
        /// 自动编号
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public EnumType.TourStructure.ServiceType ServiceType { get; set; }
        /// <summary>
        /// 接待标准(单项服务具体要求时无此项)
        /// </summary>
        public string Service { get; set; }
    }
    #endregion

    #region 团队计划包含项目信息业务实体
    /// <summary>
    /// 团队计划包含项目信息业务实体
    /// </summary>
    public class TourTeamServiceInfo : TourServiceInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourTeamServiceInfo() { }

        /// <summary>
        /// 地接报价合计
        /// </summary>
        public decimal LocalPrice { get; set; }
        /// <summary>
        /// 我社报价合计
        /// </summary>
        public decimal SelfPrice { get; set; }
        /// <summary>
        /// 地接报价人数
        /// </summary>
        public int LocalPeopleNumber { get; set; }
        /// <summary>
        /// 地接报价单价
        /// </summary>
        public decimal LocalUnitPrice { get; set; }
        /// <summary>
        /// 我社报价人数
        /// </summary>
        public int SelfPeopleNumber { get; set; }
        /// <summary>
        /// 我社报价单价
        /// </summary>
        public decimal SelfUnitPrice { get; set; }
    }
    #endregion

    #region 单项服务具体要求信息业务实体
    /// <summary>
    /// 单项服务具体要求信息业务实体
    /// </summary>
    public class TourSingleServiceInfo : TourServiceInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourSingleServiceInfo() { }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal SelfPrice { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 具休要求
        /// </summary>
        public string Requirement { get; set; }

    }
    #endregion

    #region 标准发布团队计划专有信息业务实体
    /// <summary>
    /// 标准发布团队计划专有信息业务实体
    /// </summary>
    public class TourTeamNormalPrivateInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourTeamNormalPrivateInfo() { }

        /// <summary>
        /// 行程信息集合
        /// </summary>
        public IList<TourPlanInfo> Plans { get; set; }
        /// <summary>
        /// 不含项目
        /// </summary>
        public string BuHanXiangMu { get; set; }
        /// <summary>
        /// 购物安排
        /// </summary>
        public string GouWuAnPai { get; set; }
        /// <summary>
        /// 儿童安排
        /// </summary>
        public string ErTongAnPai { get; set; }
        /// <summary>
        /// 自费项目
        /// </summary>
        public string ZiFeiXIangMu { get; set; }
        /// <summary>
        /// 注意事项
        /// </summary>
        public string ZhuYiShiXiang { get; set; }
        /// <summary>
        /// 温鑫提醒
        /// </summary>
        public string WenXinTiXing { get; set; }
        /// <summary>
        /// 内部信息
        /// </summary>
        public string NeiBuXingXi { get; set; }
    }
    #endregion

    #region 标准发布散拼计划专有信息业务实体
    /// <summary>
    /// 标准发布散拼计划专有信息业务实体
    /// </summary>
    public class TourNormalPrivateInfo : TourTeamNormalPrivateInfo
    {
        /// <summary>
        /// 包含项目信息集合
        /// </summary>
        public IList<TourServiceInfo> Services { get; set; }
    }
    #endregion

    #region 散拼计划报价客户等级信息业务实体
    /// <summary>
    /// 散拼计划报价客户等级信息业务实体
    /// </summary>
    public class TourPriceCustomerLevelInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourPriceCustomerLevelInfo() { }

        /// <summary>
        /// 成人价
        /// </summary>
        public decimal AdultPrice { get; set; }
        /// <summary>
        /// 儿童价
        /// </summary>
        public decimal ChildrenPrice { get; set; }
        /// <summary>
        /// 客户等级编号
        /// </summary>
        public int LevelId { get; set; }
        /// <summary>
        /// 客户等级名称
        /// </summary>
        public string LevelName { get; set; }
        /// <summary>
        /// 客户等级类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.CustomLevType LevelType { get; set; }
    }
    #endregion

    #region 散拼计划报价报价等级信息业务实体
    /// <summary>
    /// 散拼计划报价报价等级信息业务实体
    /// </summary>
    public class TourPriceStandardInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourPriceStandardInfo() { }

        /// <summary>
        /// 报价等级编号
        /// </summary>
        public int StandardId { get; set; }
        /// <summary>
        /// 报价等级名称
        /// </summary>
        public string StandardName { get; set; }
        /// <summary>
        /// 客户等级信息集合
        /// </summary>
        public IList<TourPriceCustomerLevelInfo> CustomerLevels { get; set; }
    }
    #endregion

    #region 散拼计划建团规则信息业务实体
    /// <summary>
    /// 散拼计划建团规则信息业务实体
    /// </summary>
    public class TourCreateRuleInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourCreateRuleInfo() { }

        /// <summary>
        /// 建团规则
        /// </summary>
        public EnumType.TourStructure.CreateTourRule Rule { get; set; }
        /// <summary>
        /// 起始日期
        /// </summary>
        public DateTime? SDate { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime? EDate { get; set; }
        /// <summary>
        /// 周期 按周生成：DayOfWeek间用","间隔，按天生成：为间隔的天数，按日期生成：NULL。
        /// </summary>
        public string Cycle { get; set; }
    }
    #endregion

    #region 散拼计划子团信息业务实体
    /// <summary>
    /// 散拼计划子团信息业务实体
    /// </summary>
    public class TourChildrenInfo
    {
        /// <summary>
        /// default constructure
        /// </summary>
        public TourChildrenInfo() { }

        /// <summary>
        /// 子团编号
        /// </summary>
        public string ChildrenId { get; set; }
        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime LDate { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
    }
    #endregion

    #region 散拼计划团队信息业务实体
    /// <summary>
    /// 散拼计划团队信息业务实体
    /// </summary>
    public class TourInfo : TourBaseInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourInfo() { }

        /// <summary>
        /// 团队地接社信息集合
        /// </summary>
        public IList<TourLocalAgencyInfo> LocalAgencys { get; set; }
        /// <summary>
        /// 团队附件信息集合
        /// </summary>
        public IList<TourAttachInfo> Attachs { get; set; }
        /// <summary>
        /// 快速发布团队专有信息
        /// </summary>
        public TourQuickPrivateInfo TourQuickInfo { get; set; }
        /// <summary>
        /// 标准发布团队专有信息
        /// </summary>
        public TourNormalPrivateInfo TourNormalInfo { get; set; }
        /// <summary>
        /// 报价等级信息集合
        /// </summary>
        public IList<TourPriceStandardInfo> PriceStandards { get; set; }
        /// <summary>
        /// 建团规则
        /// </summary>
        public TourCreateRuleInfo CreateRule { get; set; }
        /// <summary>
        /// 子团信息集合 团队本身为子团信息时=null
        /// </summary>
        public IList<TourChildrenInfo> Childrens { get; set; }
        /// <summary>
        /// 计调员信息
        /// </summary>
        public TourCoordinatorInfo Coordinator { get; set; }
        /// <summary>
        /// 集合时间
        /// </summary>
        public string GatheringTime { get; set; }
        /// <summary>
        /// 集合地点
        /// </summary>
        public string GatheringPlace { get; set; }
        /// <summary>
        /// 集合标志
        /// </summary>
        public string GatheringSign { get; set; }
        /// <summary>
        /// 送团人信息集合
        /// </summary>
        public IList<TourSentPeopleInfo> SentPeoples { get; set; }
        /// <summary>
        /// 出港城市编号
        /// </summary>
        public int TourCityId { get; set; }
    }
    #endregion

    #region 团队计划团队信息业务实体
    /// <summary>
    /// 团队计划团队信息业务实体
    /// </summary>
    public class TourTeamInfo : TourBaseInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourTeamInfo() { }

        /// <summary>
        /// 团队地接社信息集合
        /// </summary>
        public IList<TourLocalAgencyInfo> LocalAgencys { get; set; }
        /// <summary>
        /// 团队附件信息集合
        /// </summary>
        public IList<TourAttachInfo> Attachs { get; set; }
        /// <summary>
        /// 快速发布团队专有信息
        /// </summary>
        public TourQuickPrivateInfo TourQuickInfo { get; set; }
        /// <summary>
        /// 标准发布团队专有信息
        /// </summary>
        public TourTeamNormalPrivateInfo TourNormalInfo { get; set; }
        /// <summary>
        /// 包含项目信息集合
        /// </summary>
        public IList<TourTeamServiceInfo> Services { get; set; }
        /// <summary>
        /// 合计收入金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 报价编号 从线路产品库我要报价报价完成时生成的团队计划有该报价的编号
        /// </summary>
        public int QuoteId { get; set; }
        /// <summary>
        /// 原客户单位编号(修改计划信息时赋该计划原来的客户单位)
        /// </summary>
        public int OBuyerCId { get; set; }
        /// <summary>
        /// 计调员信息
        /// </summary>
        public TourCoordinatorInfo Coordinator { get; set; }
        /// <summary>
        /// 集合时间
        /// </summary>
        public string GatheringTime { get; set; }
        /// <summary>
        /// 集合地点
        /// </summary>
        public string GatheringPlace { get; set; }
        /// <summary>
        /// 集合标志
        /// </summary>
        public string GatheringSign { get; set; }
        /// <summary>
        /// 送团人信息集合
        /// </summary>
        public IList<TourSentPeopleInfo> SentPeoples { get; set; }
        /// <summary>
        /// 出港城市编号
        /// </summary>
        public int TourCityId { get; set; }

        /// <summary>
        /// 团队计划人数及单价信息
        /// </summary>
        public MTourTeamUnitInfo TourTeamUnit { get; set; }
    }
    #endregion

    #region 单项服务供应商安排信息业务实体
    /// <summary>
    /// 单项服务供应商安排信息业务实体
    /// </summary>
    public class PlanSingleInfo
    {
        /// <summary>
        /// default constructure
        /// </summary>
        public PlanSingleInfo() { }

        /// <summary>
        /// 安排编号
        /// </summary>
        public string PlanId { get; set; }
        /// <summary>
        /// 安排项目类型
        /// </summary>
        public EnumType.TourStructure.ServiceType ServiceType { get; set; }
        /// <summary>
        /// 供应商编号
        /// </summary>
        public int SupplierId { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 具体安排
        /// </summary>
        public string Arrange { get; set; }
        /// <summary>
        /// 结算费用
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 操作员编号
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 增加费用
        /// </summary>
        public decimal AddAmount { get; set; }
        /// <summary>
        /// 减少费用
        /// </summary>
        public decimal ReduceAmount { get; set; }
        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 财务备注
        /// </summary>
        public string FRemark { get; set; }
        /// <summary>
        /// 已支付金额
        /// </summary>
        public decimal PaidAmount { get; set; }
        /// <summary>
        /// 已登记金额
        /// </summary>
        public decimal RegAmount { get; set; }
        /// <summary>
        /// 增加减少费用信息集合，string.IsNullOrEmpty(ItemId)==true视为新增操作 ==false视为修改操作
        /// </summary>
        public IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> GoldDetails { get; set; }
    }
    #endregion

    #region 单项服务团队信息业务实体
    /// <summary>
    /// 单项服务团队信息业务实体
    /// </summary>
    public class TourSingleInfo : TourBaseInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourSingleInfo() { }

        /// <summary>
        /// 客户要求信息集合
        /// </summary>
        public IList<TourSingleServiceInfo> Services { get; set; }
        /// <summary>
        /// 合计收入金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 供应商安排信息集合
        /// </summary>
        public IList<PlanSingleInfo> Plans { get; set; }
        /// <summary>
        /// 合计支出金额
        /// </summary>
        public decimal TotalOutAmount { get; set; }
        /// <summary>
        /// 游客信息体现类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.CustomerDisplayType CustomerDisplayType { set; get; }
        /// <summary>
        /// 游客信息集合
        /// </summary>
        public IList<EyouSoft.Model.TourStructure.TourOrderCustomer> Customers { set; get; }
        /// <summary>
        /// 游客信息附件
        /// </summary>
        public string CustomerFilePath { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContacterName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContacterTelephone { get; set; }
        /// <summary>
        /// 联系手机
        /// </summary>
        public string ContacterMobile { get; set; }
        /// <summary>
        /// 原客户单位编号(修改计划信息时赋该计划原来的客户单位)
        /// </summary>
        public int OBuyerCId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderCode { get; set; }
    }
    #endregion

    #region 散拼计划团队计划查询信息业务实体
    /// <summary>
    /// 散拼计划团队计划查询信息业务实体
    /// </summary>
    public class TourSearchInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourSearchInfo() { }

        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 团队天数
        /// </summary>
        public int? TourDays { get; set; }
        /// <summary>
        /// 出团起始时间
        /// </summary>
        public DateTime? SDate { get; set; }
        /// <summary>
        /// 出团截止时间
        /// </summary>
        public DateTime? EDate { get; set; }
        /// <summary>
        /// 固定出团时间
        /// </summary>
        public DateTime? FDate { get; set; }
        /// <summary>
        /// 团队状态
        /// </summary>
        public EnumType.TourStructure.TourStatus? TourStatus { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public EnumType.TourStructure.OrderState? OrderStatus { get; set; }
        /// <summary>
        /// 线路区域编号
        /// </summary>
        public int? AreaId { get; set; }
        /// <summary>
        /// 线路区域集合
        /// </summary>
        public int[] Areas { get; set; }
        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public EnumType.TourStructure.TourSortType? SortType { get; set; }
        /// <summary>
        /// 出港城市编号
        /// </summary>
        public int? TourCityId { get; set; }
        /// <summary>
        /// 销售员集合
        /// </summary>
        public int[] Sellers { get; set; }
        /// <summary>
        /// 计调员集合
        /// </summary>
        public int[] Coordinators { get; set; }
        /// <summary>
        /// 操作员编号集合
        /// </summary>
        public int[] OperatorIds { get; set; }
        /// <summary>
        /// 操作员所在部门编号集合
        /// </summary>
        public int[] OperatorDepartIds { get; set; }
        /// <summary>
        /// 计划类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType? TourType { get; set; }
        /// <summary>
        /// 游客姓名
        /// </summary>
        public string YouKeName { get; set; }
    }
    #endregion

    #region 单项服务查询信息业务实体
    /// <summary>
    /// 单项服务查询信息业务实体
    /// </summary>
    public class TourSingleSearchInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourSingleSearchInfo() { }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 下单起始时间
        /// </summary>
        public DateTime? OrderSTime { get; set; }
        /// <summary>
        /// 下单截止时间
        /// </summary>
        public DateTime? OrderETime { get; set; }
        /// <summary>
        /// 下单固定时间
        /// </summary>
        public DateTime? OrderFTime { get; set; }
        /// <summary>
        /// 操作员编号集合
        /// </summary>
        public int[] OperatorId { get; set; }
        /// <summary>
        /// 客户单位编号
        /// </summary>
        public int? BuyerCId { get; set; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string BuyerCName { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
    }
    #endregion

    #region 单项服务列表信息业务实体
    /// <summary>
    /// 单项服务列表信息业务实体
    /// </summary>
    public class LBSingleTourInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public LBSingleTourInfo() { }

        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 团队状态
        /// </summary>
        public EnumType.TourStructure.TourStatus Status { get; set; }
        /// <summary>
        /// 操作员姓名
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 客户单位Id
        /// </summary>
        public int BuyerId { get; set; }

        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string BuyerCName { get; set; }
        /// <summary>
        /// 客户单位联系人
        /// </summary>
        public string BuyerContacterName { get; set; }
        /// <summary>
        /// 客户单位联系人电话
        /// </summary>
        public string BuyerContacterTelephone { get; set; }
        /// <summary>
        /// 服务类别
        /// </summary>
        public string Services { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
    }
    #endregion

    #region 团队计划列表信息业务实体
    /// <summary>
    /// 团队计划列表信息业务实体
    /// </summary>
    public class LBTeamTourInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public LBTeamTourInfo() { }

        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime LDate { get; set; }
        /// <summary>
        /// 团队状态
        /// </summary>
        public EnumType.TourStructure.TourStatus Status { get; set; }
        /// <summary>
        /// 是否成本确认
        /// </summary>
        public bool IsCostConfirm { get; set; }
        /// <summary>
        /// 是否团队复核
        /// </summary>
        public bool IsReview { get; set; }
        /// <summary>
        /// 机票状态
        /// </summary>
        public EnumType.PlanStructure.TicketState TicketStatus { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public int PlanPeopleNumber { get; set; }
        /// <summary>
        /// 发布类型
        /// </summary>
        public EnumType.TourStructure.ReleaseType ReleaseType { get; set; }
        /// <summary>
        /// 天数
        /// </summary>
        public int TourDays { get; set; }
        /// <summary>
        /// 订单总收入
        /// </summary>
        public decimal TotalIncome { get; set; }
        /// <summary>
        /// 计调总支出
        /// </summary>
        public decimal TotalExpenses { get; set; }
        /// <summary>
        /// 杂费总收入
        /// </summary>
        public decimal TotalOtherIncome { get; set; }
        /// <summary>
        /// 杂费总支出
        /// </summary>
        public decimal TotalOtherExpenses { get; set; }
        /// <summary>
        /// 利润分配总金额
        /// </summary>
        public decimal DistributionAmount { get; set; }
        /// <summary>
        /// 客户单位编号
        /// </summary>
        public int BuyerCId { get; set; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string BuyerCName { get; set; }
        /// <summary>
        /// 客户单位联系人
        /// </summary>
        public string BuyerContacterName { get; set; }
        /// <summary>
        /// 客户单位联系人电话
        /// </summary>
        public string BuyerContacterTelephone { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }
    }
    #endregion

    #region 团队核算列表信息业务实体
    /// <summary>
    /// 团队核算列表信息业务实体
    /// </summary>
    public class LBAccountingTourInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public LBAccountingTourInfo() { }

        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
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
        /// 操作人姓名
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 收入金额
        /// </summary>
        public decimal InComeAmount { get; set; }
        /// <summary>
        /// 支出金额
        /// </summary>
        public decimal OutAmount { get; set; }
        /// <summary>
        /// 毛利
        /// </summary>
        public decimal GrossProfit { get { return this.InComeAmount - this.OutAmount; } }
        /// <summary>
        /// 利润分配金额
        /// </summary>
        public decimal DistributionAmount { get; set; }
        /// <summary>
        /// 纯利
        /// </summary>
        public decimal NetProfit { get { return this.GrossProfit - this.DistributionAmount; } }
        /// <summary>
        /// 计划类型
        /// </summary>
        public EnumType.TourStructure.TourType TourType { get; set; }
        /// <summary>
        /// 计划发布类型
        /// </summary>
        public EnumType.TourStructure.ReleaseType ReleaseType { get; set; }
        /// <summary>
        /// 单项服务服务类别
        /// </summary>
        public string SingleServices { get; set; }
        /// <summary>
        /// 人均毛利
        /// </summary>
        public decimal RenJunMaoLi
        {
            get
            {
                int renshu = AdultNumber + ChildrenNumber;
                if (renshu == 0) return 0;
                return GrossProfit / renshu;
            }
        }
        /// <summary>
        /// 毛利率
        /// </summary>
        public decimal MaoLiLv
        {
            get
            {
                if (InComeAmount <= 0) return 0;
                return GrossProfit / InComeAmount;
            }
        }

        /// <summary>
        /// 团队类型标记
        /// </summary>
        public string TourTypeMark
        {
            get
            {
                string s = string.Empty;
                switch (TourType)
                {
                    case EyouSoft.Model.EnumType.TourStructure.TourType.单项服务: s = "(单)"; break;
                    case EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划: s = "(散)"; break;
                    case EyouSoft.Model.EnumType.TourStructure.TourType.团队计划: s = "(团)"; break;
                }
                return s;
            }
        }
    }
    #endregion

    #region 组团端计划列表信息业务实体
    /// <summary>
    /// 组团端计划列表信息业务实体
    /// </summary>
    public class LBZTTours
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public LBZTTours() { }

        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 团队类型
        /// </summary>
        public EnumType.TourStructure.TourType TourType { get { return EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划; } }
        /// <summary>
        /// 发布类型
        /// </summary>
        public EnumType.TourStructure.ReleaseType ReleaseType { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime LDate { get; set; }
        /// <summary>
        /// 计划人数
        /// </summary>
        public int PlanPeopleNumber { get; set; }
        /// <summary>
        /// 虚拟实收人数
        /// </summary>
        public int VirtualPeopleNumber { get; set; }
        /// <summary>
        /// 剩余人数
        /// </summary>
        public int RemainPeopleNumber { get { return this.PlanPeopleNumber - this.PeopleNumberShiShou; } }
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
        /// 负责人QQ
        /// </summary>
        public string ContacterQQ { get; set; }
        /// <summary>
        /// 订单实收人数（含留位、未处理、已成交订单）
        /// </summary>
        public int PeopleNumberShiShou { get; set; }

        /// <summary>
        /// 团队推广状态
        /// </summary>
        public EnumType.TourStructure.TourRouteStatus TourRouteStatus { get; set; }

        /// <summary>
        /// 手动设置的计划状态
        /// </summary>
        public EnumType.TourStructure.HandStatus HandStatus { get; set; } 
    }
    #endregion

    #region 供应商（地接社）交易次数计划列表信息业务实体
    /// <summary>
    /// 供应商（地接社）交易次数计划列表信息业务实体
    /// </summary>
    public class LBGYSTours
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public LBGYSTours() { }

        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 团队类型
        /// </summary>
        public EnumType.TourStructure.TourType TourType { get; set; }
        /// <summary>
        /// 发布类型
        /// </summary>
        public EnumType.TourStructure.ReleaseType ReleaseType { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime LDate { get; set; }
        /// <summary>
        /// 计划人数
        /// </summary>
        public int PlanPeopleNumber { get; set; }
        /// <summary>
        /// 订单实收人数
        /// </summary>
        public int RealityPeopleNumber { get; set; }
        /// <summary>
        /// 计调员
        /// </summary>
        public string PlanNames { get; set; }
        /// <summary>
        /// 返利
        /// </summary>
        public decimal CommissionAmount { get; set; }
        /// <summary>
        /// 结算费用
        /// </summary>
        public decimal SettlementAmount { get; set; }
        /// <summary>
        /// 已支付金额
        /// </summary>
        public decimal PayAmount { get; set; }
        /// <summary>
        /// 未支付金额
        /// </summary>
        public decimal NotPayAmount { get { return this.SettlementAmount - this.PayAmount; } }

    }
    #endregion

    #region 利润统计团队数计划列表信息业务实体
    /// <summary>
    /// 利润统计团队数计划列表信息业务实体
    /// </summary>
    public class LBLYTJTours
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public LBLYTJTours() { }

        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 团队类型
        /// </summary>
        public EnumType.TourStructure.TourType TourType { get; set; }
        /// <summary>
        /// 发布类型
        /// </summary>
        public EnumType.TourStructure.ReleaseType ReleaseType { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime LDate { get; set; }
        /// <summary>
        /// 订单实收人数
        /// </summary>
        public int RealityPeopleNumber { get; set; }
        /// <summary>
        /// 计调员
        /// </summary>
        public string PlanNames { get; set; }
        /// <summary>
        /// 收入金额
        /// </summary>
        public decimal IncomeAmount { get; set; }
        /// <summary>
        /// 支出金额
        /// </summary>
        public decimal OutAmount { get; set; }
        /// <summary>
        /// 毛利
        /// </summary>
        public decimal GrossProfit { get { return this.IncomeAmount - this.OutAmount; } }

        /// <summary>
        /// 联字号
        /// </summary>
        public int PKID { get; set; }
        /// <summary>
        /// 报销金额
        /// </summary>
        public decimal ReimburseAmount { get; set; }
        /// <summary>
        /// 订单信息集合
        /// </summary>
        public IList<LBLYTJTourOrderInfo> Orders { get; set; }
        /// <summary>
        /// 机票安排信息集合(非单项服务)
        /// </summary>
        public IList<LBLYTJTourPlanInfo> PlanTickets { get; set; }
        /// <summary>
        /// 地接安排信息集合(非单项服务)
        /// </summary>
        public IList<LBLYTJTourPlanInfo> PlanAgencys { get; set; }
        /// <summary>
        /// 单项服务供应商信息安排集合(单项服务)
        /// </summary>
        public IList<LBLYTJTourPlanInfo> PlanSingles { get; set; }
    }

    /// <summary>
    /// 利润统计团队数计划列表订单信息业务实体
    /// </summary>
    public class LBLYTJTourOrderInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public LBLYTJTourOrderInfo() { }

        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string BuyCompanyName { get; set; }
        /// <summary>
        /// 成人数
        /// </summary>
        public int AdultNumber { get; set; }
        /// <summary>
        /// 儿童数
        /// </summary>
        public int ChildrenNumber { get; set; }
        /// <summary>
        /// 总人数
        /// </summary>
        public int PeopleNumber { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 已收金额
        /// </summary>
        public decimal ReceivedAmount { get; set; }
        /// <summary>
        /// 未收金额
        /// </summary>
        public decimal UnReceivedAmount { get { return this.TotalAmount - this.ReceivedAmount; } }
    }

    /// <summary>
    /// 利润统计团队数计划列表计调安排信息业务实体
    /// </summary>
    public class LBLYTJTourPlanInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public LBLYTJTourPlanInfo() { }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 已付金额
        /// </summary>
        public decimal PaidAmount { get; set; }
        /// <summary>
        /// 未付金额
        /// </summary>
        public decimal UnPaidAmount { get { return this.TotalAmount - this.PaidAmount; } }
        /// <summary>
        /// 项目类型
        /// </summary>
        public EnumType.TourStructure.ServiceType ServiceType { get; set; }
        /// <summary>
        /// 人数(仅限机票安排)
        /// </summary>
        public int PeopleNumber { get; set; }
    }
    #endregion

    #region 供应商（票务）交易次数计划列表信息业务实体

    /// <summary>
    /// 供应商（票务）交易次数计划列表信息业务实体
    /// </summary>
    public class JPGYSTours
    {
        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }

        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime LDate { get; set; }

        /// <summary>
        /// 计调员
        /// </summary>
        public string PlanNames { get; set; }

        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleNum { get; set; }

        /// <summary>
        /// 票款
        /// </summary>
        public decimal TicketPrice { get; set; }

        /// <summary>
        /// 代理费
        /// </summary>
        public decimal AgencyPrice { get; set; }

        /// <summary>
        /// 总费用
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 已支付金额
        /// </summary>
        public decimal PayAmount { get; set; }
        /// <summary>
        /// 未支付金额
        /// </summary>
        public decimal NotPayAmount { get { return this.TotalAmount - this.PayAmount; } }

        /// <summary>
        /// 航班明细
        /// </summary>
        public IList<TicketFligth> TicketFligth { get; set; }
    }

    #endregion

    /// <summary>
    /// 航班明细
    /// </summary>
    public class TicketFligth
    {
        /// <summary>
        /// 票号
        /// </summary>
        public string TicketNum { get; set; }

        /// <summary>
        /// 航段
        /// </summary>
        public string FligthSegment { get; set; }

        /// <summary>
        /// 出票时间
        /// </summary>
        public DateTime TicketOutTime { get; set; }
    }

    #region 团款支出列表信息业务实体
    /// <summary>
    /// 团款支出列表信息业务实体
    /// </summary>
    public class LBTKZCTourInfo
    {
        /// <summary>
        /// default constructor 
        /// </summary>
        public LBTKZCTourInfo() { }

        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 团号/单项服务订单号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime? LDate { get; set; }
        /// <summary>
        /// 团队类型
        /// </summary>
        public EnumType.TourStructure.TourType TourType { get; set; }
        /// <summary>
        /// 是否成本确认
        /// </summary>
        public bool IsCostConfirm { get; set; }
    }
    #endregion

    #region 团款支出查询信息业务实体
    /// <summary>
    /// 团款支出查询信息业务实体
    /// </summary>
    public class TourSearchTKZCInfo
    {
        /// <summary>
        /// default constructor 
        /// </summary>
        public TourSearchTKZCInfo() { }

        /// <summary>
        /// 团队类型
        /// </summary>
        public EnumType.TourStructure.TourType? TourType { get; set; }
        /// <summary>
        /// 团号/单项服务订单号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierCName { get; set; }
        /// <summary>
        /// 供应商类型
        /// </summary>
        public EnumType.CompanyStructure.SupplierType? SupplierCType { get; set; }
        /// <summary>
        /// 出团起始时间
        /// </summary>
        public DateTime? SDate { get; set; }
        /// <summary>
        /// 出团截止时间
        /// </summary>
        public DateTime? EDate { get; set; }
        /// <summary>
        /// 固定出团时间
        /// </summary>
        public DateTime? FDate { get; set; }
        /// <summary>
        /// 付款固定时间
        /// </summary>
        public DateTime? PaymentFTime { get; set; }
        /// <summary>
        /// 付款起始时间
        /// </summary>
        public DateTime? PaymentSTime { get; set; }
        /// <summary>
        /// 付款截止时间
        /// </summary>
        public DateTime? PaymentETime { get; set; }
    }
    #endregion

    #region 散拼/团队计划计调员信息业务实体
    /// <summary>
    /// 散拼/团队计划计调员信息业务实体
    /// </summary>
    public class TourCoordinatorInfo
    {
        /// <summary>
        /// default constructor 
        /// </summary>
        public TourCoordinatorInfo() { }

        /// <summary>
        /// 计调员编号
        /// </summary>
        public int CoordinatorId { get; set; }
        /// <summary>
        /// 计调员姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 计调员联系电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 计调员联系手机
        /// </summary>
        public string Mobile { get; set; }
    }
    #endregion

    #region 计划送团人信息业务实体
    /// <summary>
    /// 计划送团人信息业务实体
    /// </summary>
    public class TourSentPeopleInfo
    {
        /// <summary>
        /// default constructor 
        /// </summary>
        public TourSentPeopleInfo() { }

        /// <summary>
        /// 送团人编号
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 送团人姓名
        /// </summary>
        public string OperatorName { get; set; }
    }
    #endregion

    #region 个人中心-地接安排计划信息业务实体
    /// <summary>
    /// 个人中心-地接安排计划信息业务实体
    /// </summary>
    public class LBDJAPTourInfo
    {
        /// <summary>
        /// default constructor 
        /// </summary>
        public LBDJAPTourInfo() { }

        /// <summary>
        /// 计划编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime LDate { get; set; }
        /// <summary>
        /// 计划天数
        /// </summary>
        public int TourDays { get; set; }
        /// <summary>
        /// 计划人数
        /// </summary>
        public int PlanPeopleNumber { get; set; }
        /// <summary>
        /// 去程航班/时间
        /// </summary>
        public string LTrafic { get; set; }
        /// <summary>
        /// 回程航班/时间
        /// </summary>
        public string RTrafic { get; set; }
        /// <summary>
        /// 计划类型
        /// </summary>
        public EnumType.TourStructure.TourType TourType { get; set; }
        /// <summary>
        /// 计划发布类型
        /// </summary>
        public EnumType.TourStructure.ReleaseType ReleaseType { get; set; }
        /// <summary>
        /// 计调员姓名
        /// </summary>
        public string JDYContacterName { get; set; }
        /// <summary>
        /// 计调员电话
        /// </summary>
        public string JDYTelephone { get; set; }
        /// <summary>
        /// 计调员手机
        /// </summary>
        public string JDYMobile { get; set; }
        /// <summary>
        /// 计调员QQ
        /// </summary>
        public string JDYQQ { get; set; }
    }
    #endregion

    #region 线路上团数汇总信息业务实体
    /// <summary>
    /// 线路上团数汇总信息业务实体
    /// </summary>
    public class MRouteSTSCollectInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MRouteSTSCollectInfo() { }

        /// <summary>
        /// 实收人数
        /// </summary>
        public int PeopleNumberShiShou { get; set; }
        /// <summary>
        /// 收入
        /// </summary>
        public decimal IncomeAmount { get; set; }
        /// <summary>
        /// 支出
        /// </summary>
        public decimal OutAmount { get; set; }
        /// <summary>
        /// 毛利
        /// </summary>
        public decimal GrossProfit { get { return this.IncomeAmount - this.OutAmount; } }
    }
    #endregion

    #region 线路收客数汇总信息业务实体

    /// <summary>
    /// 线路收客数汇总信息业务实体
    /// </summary>
    public class MRouteSKSCollectInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MRouteSKSCollectInfo() { }

        /// <summary>
        /// 成人数
        /// </summary>
        public int AdultNumber { get; set; }
        /// <summary>
        /// 儿童数
        /// </summary>
        public int ChildrenNumber { get; set; }
    }

    #endregion

    #region 团队计划人数及单价信息

    /// <summary>
    /// 团队计划人数及单价信息
    /// </summary>
    public class MTourTeamUnitInfo
    {
        /// <summary>
        /// 成人数
        /// </summary>
        public int NumberCr { get; set; }

        /// <summary>
        /// 儿童数
        /// </summary>
        public int NumberEt { get; set; }

        /// <summary>
        /// 全陪数
        /// </summary>
        public int NumberQp { get; set; }

        /// <summary>
        /// 成人单价合计
        /// </summary>
        public decimal UnitAmountCr { get; set; }

        /// <summary>
        /// 儿童单价合计
        /// </summary>
        public decimal UnitAmountEt { get; set; }

        /// <summary>
        /// 全陪单价合计
        /// </summary>
        public decimal UnitAmountQp { get; set; }
    }

    #endregion

    #region 地接供应商交易次数明细查询信息业务实体
    /// <summary>
    /// 地接供应商交易次数明细查询信息业务实体
    /// </summary>
    public class MTimesSummaryDiJieSearchInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MTimesSummaryDiJieSearchInfo() { }

        /// <summary>
        /// 出团起始时间
        /// </summary>
        public DateTime? SDate { get; set; }
        /// <summary>
        /// 出团截止时间
        /// </summary>
        public DateTime? EDate { get; set; }
        /// <summary>
        /// 付款状态 1:已结清 2:未结清
        /// </summary>
        public int? PayStatus { get; set; }
    }
    #endregion

    #region 计划信息业务实体（用于子母团展示团队信息时的日历）
    /// <summary>
    /// 计划信息业务实体（用于子母团展示团队信息时的日历）
    /// </summary>
    /// 汪奇志 2012-04-06
    public class MRiLiTourInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MRiLiTourInfo() { }

        /// <summary>
        /// 计划编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 计划人数
        /// </summary>
        public int RenShuJiHua { get; set; }
        /// <summary>
        /// 实收人数
        /// </summary>
        public int RenShuShiShou { get; set; }
        /// <summary>
        /// 剩余人数
        /// </summary>
        public int RenShuShengYu { get { return RenShuJiHua - RenShuShiShou; } }
        /// <summary>
        /// 成人价格
        /// </summary>
        public decimal JiaGeCR { get; set; }
        /// <summary>
        /// 出团时间
        /// </summary>
        public DateTime LDate { get; set; }
    }
    #endregion
}
