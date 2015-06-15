using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.TourStructure
{
    #region 订单信息实体
    /// <summary>
    /// 订单信息实体
    /// 创建人：luofx 2011-01-19
    /// </summary>
    public class TourOrder
    {
        #region Model
        /// <summary>
        /// 主键ID 
        /// </summary>
        public string ID { set; get; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { set; get; }
        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { set; get; }
        /// <summary>
        /// 线路区域Id
        /// </summary>
        public int AreaId { set; get; }
        /// <summary>
        /// 线路Id
        /// </summary>
        public int RouteId { set; get; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { set; get; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo { set; get; }
        /// <summary>
        /// 团队类型(散客，团队,单项服务)
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType TourClassId { set; get; }
        /// <summary>
        /// 出团时间
        /// </summary>
        public DateTime LeaveDate { set; get; }
        /// <summary>
        /// 团队天数
        /// </summary>
        public int Tourdays { set; get; }
        /// <summary>
        /// 出发交通
        /// </summary>
        public string LeaveTraffic { set; get; }
        /// <summary>
        /// 返程交通
        /// </summary>
        public string ReturnTraffic { set; get; }
        /// <summary>
        /// 订单来源  
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.OrderType OrderType { set; get; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.OrderState OrderState { set; get; }
        /// <summary>
        /// 预订单位ID
        /// </summary>
        public int BuyCompanyID { set; get; }
        /// <summary>
        /// 预订单位名称
        /// </summary>
        public string BuyCompanyName { set; get; }
        /// <summary>
        /// 下单人（组团下单，专线下单）
        /// </summary>
        public string ContactName { set; get; }
        /// <summary>
        /// 下单人联系电话
        /// </summary>
        public string ContactTel { set; get; }
        /// <summary>
        /// 下单人联系手机
        /// </summary>
        public string ContactMobile { set; get; }
        /// <summary>
        /// 下单人传真
        /// </summary>
        public string ContactFax { set; get; }
        /// <summary>
        /// 销售员名字
        /// </summary>
        public string SalerName { set; get; }
        /// <summary>
        /// 销售员Id
        /// </summary>
        public int SalerId { set; get; }
        /// <summary>
        /// 下单人名字
        /// </summary>
        public string OperatorName { set; get; }
        /// <summary>
        /// 下单人ID
        /// </summary>
        public int OperatorID { set; get; }
        /// <summary>
        /// 价格等级编号
        /// </summary>
        public int PriceStandId { set; get; }
        /// <summary>
        /// 成人价
        /// </summary>
        public decimal PersonalPrice { set; get; }
        /// <summary>
        /// 儿童价
        /// </summary>
        public decimal ChildPrice { set; get; }
        /// <summary>
        /// 单房差
        /// </summary>
        public decimal MarketPrice { set; get; }
        /// <summary>
        /// 成人数
        /// </summary>
        public int AdultNumber { set; get; }
        /// <summary>
        /// 儿童数
        /// </summary>
        public int ChildNumber { set; get; }
        /// <summary>
        /// 单房差数
        /// </summary>
        public int MarketNumber { set; get; }
        /// <summary>
        /// 总人数
        /// </summary>
        public int PeopleNumber { set; get; }
        /// <summary>
        /// 退团人数
        /// </summary>
        public int LeaguePepoleNum { set; get; }
        /// <summary>
        /// 其它费用
        /// </summary>
        public decimal OtherPrice { set; get; }
        /// <summary>
        /// 留位时间
        /// </summary>
        public DateTime SaveSeatDate { set; get; }
        /// <summary>
        /// 操作留言
        /// </summary>
        public string OperatorContent { set; get; }
        /// <summary>
        /// 特别要求
        /// </summary>
        public string SpecialContent { set; get; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal SumPrice { set; get; }
        /// <summary>
        /// 专线公司名称
        /// </summary>
        public string SellCompanyName { set; get; }
        /// <summary>
        /// 专线公司编号
        /// </summary>
        public int SellCompanyId { set; get; }
        /// <summary>
        /// 最后修改时间 默认getdate()
        /// </summary>
        public DateTime LastDate { set; get; }
        /// <summary>
        /// 最后操作人ID
        /// </summary>
        public int LastOperatorID { set; get; }
        /// <summary>
        /// 交易成功时间
        /// </summary>
        public DateTime? SuccessTime { set; get; }
        /// <summary>
        /// 已收已审核帐款
        /// </summary>
        public decimal HasCheckMoney { set; get; }
        /// <summary>
        /// 已收未审核帐款
        /// </summary>
        public decimal NotCheckMoney { set; get; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { set; get; }
        /// <summary>
        /// 是否删除(已删除=true,未删除=false)
        /// </summary>
        public bool IsDelete { set; get; }
        /// <summary>
        /// 操作人或发布计划人(组团下单及团队订单时存放发布计划人 专线报名时存放当前登录人)用于浏览权限控制
        /// </summary>
        public int ViewOperatorId { set; get; }
        /// <summary>
        /// 游客信息体现类型（附件方式、输入方式）
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.CustomerDisplayType CustomerDisplayType { set; get; }
        /// <summary>
        /// 游客信息附件
        /// </summary>
        public string CustomerFilePath { set; get; }
        /// <summary>
        /// 客户等级编号
        /// </summary>
        public int CustomerLevId { set; get; }
        /// <summary>
        /// 财务小计
        /// </summary>
        public decimal FinanceSum { set; get; }
        #endregion Model

        #region 扩展
        /// <summary>
        /// 团队发布类型
        /// </summary>
        public EnumType.TourStructure.ReleaseType ReleaseType { get; set; }
        /// <summary>
        /// 游客信息集合
        /// </summary>
        public IList<EyouSoft.Model.TourStructure.TourOrderCustomer> CustomerList { set; get; }
        /// <summary>
        /// 剩余人数
        /// </summary>
        public int RemainNum { set; get; }
        /// <summary>
        /// 未收金额
        /// </summary>
        public decimal NotReceived { set; get; }
        /// <summary>
        /// 团队计调信息集合
        /// </summary>
        public IList<EyouSoft.Model.TourStructure.TourOperator> OperatorList { set; get; }
        /// <summary>
        /// 是否组团端操作【组团端修改/新增 订单时=true】
        /// </summary>
        public bool IsTourOrderEdit { set; get; }
        /// <summary>
        /// 订单金额增加减少费用信息
        /// </summary>
        public TourOrderAmountPlusInfo AmountPlus { get; set; }
        #endregion 扩展

        /// <summary>
        /// 组团联系人编号
        /// </summary>
        public int BuyerContactId { get; set; }
        /// <summary>
        /// 组团联系人姓名
        /// </summary>
        public string BuyerContactName { get; set; }
        /// <summary>
        /// 返佣类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.CommissionType CommissionType { get; set; }
        /// <summary>
        /// 返佣金额(单价)
        /// </summary>
        public decimal CommissionPrice { get; set; }

        /// <summary>
        /// 团队计划订单人数价格信息
        /// </summary>
        public MTourTeamUnitInfo TourTeamUnit { get; set; }
        /// <summary>
        /// 组团社团号
        /// </summary>
        public string BuyerTourCode { get; set; }

        /// <summary>
        /// 订单关联交通编号
        /// </summary>
        public int OrderTrafficId { get; set; }

        /// <summary>
        /// 此订单是否有机票申请(用户列表判断能否修改、申请机票 用)；有机票申请则订单不能修改，不能申请机票
        /// </summary>
        public bool IsExtsisTicket { get; set; }
    }
    #endregion

    #region 团队计调信息实体
    /// <summary>
    /// 团队计调信息实体
    /// </summary>
    public class TourOperator
    {
        /// <summary>
        /// 计调员编号
        /// </summary>
        public int OperatorId { set; get; }
        /// <summary>
        /// 基调员名称
        /// </summary>
        public string OperatorName { set; get; }
        /// <summary>
        /// 基调员姓名
        /// </summary>
        public string ContactName { set; get; }
        /// <summary>
        /// 电话
        /// </summary>
        public string ContactTel { set; get; }
        /// <summary>
        /// 联系手机
        /// </summary>
        public string ContactMobile { set; get; }

    }
    #endregion

    #region 团队结算订单实体
    /// <summary>
    /// 团队结算订单实体
    /// </summary>
    public class OrderFinanceExpense
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { set; get; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { set; get; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string CompanyName { set; get; }
        /// <summary>
        /// 财务增加费用
        /// </summary>
        public decimal FinanceAddExpense { set; get; }
        /// <summary>
        /// 财务减少费用
        /// </summary>
        public decimal FinanceRedExpense { set; get; }
        /// <summary>
        /// 财务小计
        /// </summary>
        public decimal FinanceSum { set; get; }
        /// <summary>
        /// 财务备注
        /// </summary>
        public string FinanceRemark { set; get; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal SumPrice { set; get; }
        /// <summary>
        /// 增加减少费用信息集合，string.IsNullOrEmpty(ItemId)==true视为新增操作 ==false视为修改操作
        /// </summary>
        public IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> GoldDetails { get; set; }
    }
    #endregion

    #region 订单中心查询实体
    /// <summary>
    /// 订单中心查询实体
    /// </summary>
    public class OrderCenterSearchInfo
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.OrderState? OrderState { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo { get; set; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 出团日期开始
        /// </summary>
        public DateTime? LeaveDateFrom { get; set; }
        /// <summary>
        /// 出团日期结束
        /// </summary>
        public DateTime? LeaveDateTo { get; set; }
        /// <summary>
        /// 下单日期开始
        /// </summary>
        public DateTime? CreateDateFrom { get; set; }
        /// <summary>
        /// 下单日期结束
        /// </summary>
        public DateTime? CreateDateTo { get; set; }
        /// <summary>
        /// 操作员Id数组
        /// </summary>
        public int[] OperatorId { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }
    }
    #endregion

    #region 组团端订单查询实体
    /// <summary>
    /// 组团端订单查询实体
    /// </summary>
    public class TourOrderSearchInfo
    {
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 线路区域编号
        /// </summary>
        public int? AreaId { get; set; }
        /// <summary>
        /// 出团日期开始
        /// </summary>
        public DateTime? LeaveDateFrom { get; set; }
        /// <summary>
        /// 出团日期结束
        /// </summary>
        public DateTime? LeaveDateTo { get; set; }
        /// <summary>
        /// 下单日期开始
        /// </summary>
        public DateTime? CreateDateFrom { get; set; }
        /// <summary>
        /// 下单日期结束
        /// </summary>
        public DateTime? CreateDateTo { get; set; }
    }
    #endregion

    #region 组团端-财务管理查询实体
    /// <summary>
    /// 组团端-财务管理查询实体
    /// </summary>
    public class TourFinanceSearchInfo
    {
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo { get; set; }
        /// <summary>
        /// 出团日期开始
        /// </summary>
        public DateTime? LeaveDateFrom { get; set; }
        /// <summary>
        /// 出团日期结束
        /// </summary>
        public DateTime? LeaveDateTo { get; set; }
        /// <summary>
        /// 操作员名称
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 销售员名称
        /// </summary>
        public string SalerName { get; set; }
        /// <summary>
        /// 是否结清
        /// </summary>
        public bool IsSettle { get; set; }

    }
    #endregion

    #region 销售收款查询实体
    /// <summary>
    /// 销售收款查询实体
    /// </summary>
    public class SalerSearchInfo
    {
        /// <summary>
        /// 出团起始时间
        /// </summary>
        public DateTime? SDate { get; set; }
        /// <summary>
        /// 出团截止时间
        /// </summary>
        public DateTime? EDate { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo { get; set; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 统计订单的方式
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? ComputeOrderType { get; set; }
        /// <summary>
        /// 销售员Id
        /// </summary>
        public int[] SalerId { get; set; }
        /// <summary>
        /// 订单操作员Id
        /// </summary>
        public int[] OrderOperatorId { get; set; }

        /// <summary>
        /// 帐款是否结清
        /// </summary>
        public bool? IsSettle { get; set; }

    }
    #endregion

    #region 统计分析-收入对账单（账龄分析表）搜索实体
    /// <summary>
    /// 统计分析-收入对账单（账龄分析表）搜索实体
    /// </summary>
    public class SearchInfo
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { set; get; }
        /// <summary>
        /// 团队类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType? TourType { set; get; }
        /// <summary>
        /// 线路区域
        /// </summary>
        public int? AreaId { set; get; }
        /// <summary>
        /// 销售员
        /// </summary>
        public int? SalerId { set; get; }
        /// <summary>
        /// 出团日期开始
        /// </summary>
        public DateTime? LeaveDateFrom { set; get; }
        /// <summary>
        /// 出团日期结束
        /// </summary>
        public DateTime? LeaveDateTo { set; get; }

        /// <summary>
        /// 订单统计方式
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType? ComputeOrderType { get; set; }

        /// <summary>
        /// 客户单位Id
        /// </summary>
        public int BuyCompanyId { get; set; }

        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string BuyCompanyName { get; set; }

        /// <summary>
        /// 帐款是否结清
        /// </summary>
        public bool? IsSettle { get; set; }
    }
    #endregion

    #region DAL端类似方法统一实体
    /// <summary>
    /// DAL端类似方法统一实体
    /// </summary>
    public class SearchInfoForDAL
    {
        /// <summary>
        /// 所属公司编号
        /// </summary>
        public int? SellCompanyId { set; get; }
        /// <summary>
        /// 组团公司编号
        /// </summary>
        public int? BuyCompanyId { set; get; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo { get; set; }
        /// <summary>
        /// 团队类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType? TourType { set; get; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 线路编号
        /// </summary>
        public int? RouteId { get; set; }
        /// <summary>
        /// 线路区域
        /// </summary>
        public int? AreaId { set; get; }
        /// <summary>
        /// 操作员编号
        /// </summary>
        public int[] OperatorId { get; set; }
        /// <summary>
        /// 销售员编号
        /// </summary>
        public int[] SalerId { get; set; }
        /// <summary>
        /// 操作员名称
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 销售员名称
        /// </summary>
        public string SalerName { get; set; }
        /// <summary>
        /// 出团日期开始
        /// </summary>
        public DateTime? LeaveDateFrom { get; set; }
        /// <summary>
        /// 出团日期结束
        /// </summary>
        public DateTime? LeaveDateTo { get; set; }
        /// <summary>
        /// 下单日期开始
        /// </summary>
        public DateTime? CreateDateFrom { get; set; }
        /// <summary>
        /// 下单日期结束
        /// </summary>
        public DateTime? CreateDateTo { get; set; }
        /// <summary>
        /// 帐款是否结清
        /// </summary>
        public bool? IsSettle { get; set; }
        /// <summary>
        /// 组织架构用户编号（逗号分隔）
        /// </summary>
        public string HaveUserIds { get; set; }
        /// <summary>
        /// 订单状态数组
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.OrderState[] OrderState { get; set; }
    }
    #endregion

    #region 订单金额增加减少费用信息业务实体
    /// <summary>
    /// 订单金额增加减少费用信息业务实体
    /// </summary>
    public class TourOrderAmountPlusInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TourOrderAmountPlusInfo() { }

        /// <summary>
        /// 增加费用
        /// </summary>
        public decimal AddAmount { get; set; }
        /// <summary>
        /// 减少费用
        /// </summary>
        public decimal ReduceAmount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
    #endregion

    #region 订单状态链接列表信息业务实体
    /// <summary>
    /// 订单状态链接列表信息业务实体
    /// </summary>
    /// Author:汪奇志 2011-03-21
    public class LBOrderStatusTourOrderInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public LBOrderStatusTourOrderInfo() { }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 计划编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime LDate { get; set; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string BuyerCompanyName { get; set; }
        /// <summary>
        /// 客户单位联系人
        /// </summary>
        public string BuyerContacterName { get; set; }
        /// <summary>
        /// 客户单位联系人电话
        /// </summary>
        public string BuyerContacterTelephone { get; set; }
        /// <summary>
        /// 操作员姓名
        /// </summary>
        public string OperatorName { get; set; }
    }

    #endregion

    #region 销售收款合计信息业务实体
    /// <summary>
    /// 销售收款合计信息业务实体
    /// </summary>
    /// 汪奇志 2011-07-04
    public class MSaleReceivableSummaryInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MSaleReceivableSummaryInfo() { }

        /// <summary>
        /// 总金额(团队核算后金额)
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleNumber { get; set; }
        /// <summary>
        /// 已收金额
        /// </summary>
        public decimal ReceivedAmount { get; set; }
        /// <summary>
        /// 未审核金额
        /// </summary>
        public decimal UnauditedAmount { get; set; }
        /// <summary>
        /// 未收金额
        /// </summary>
        public decimal NotReceivedAmount { get { return this.TotalAmount - this.ReceivedAmount; } }
    }
    #endregion

    #region 机票审核订单信息实体

    /// <summary>
    /// 机票审核订单信息实体
    /// </summary>
    public class OrderByCheckTicket
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 组团名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 订单人数
        /// </summary>
        public int OrderPeople { get; set; }

        /// <summary>
        /// 订单金额(财务小计)
        /// </summary>
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 已收已审核帐款
        /// </summary>
        public decimal HasCheckMoney { set; get; }

        /// <summary>
        /// 未收
        /// </summary>
        public decimal NotMoney
        {
            get { return OrderAmount - HasCheckMoney; }
        }
    }

    #endregion

    #region 订单提醒列表信息业务实体
    /// <summary>
    /// 订单提醒列表信息业务实体
    /// </summary>
    /// 汪奇志 2012-04-09
    public class MDingDanTiXingInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MDingDanTiXingInfo() { }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 订单叼
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 出团时间
        /// </summary>
        public DateTime LDate { get; set; }
        /// <summary>
        /// 客户单位编号
        /// </summary>
        public int KeHuBianHao { get; set; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string KeHuMingCheng { get; set; }
        /// <summary>
        /// 订单人数
        /// </summary>
        public int RenShu { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal JinE { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime XiaDanShiJian { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.OrderState OrderStatus { get; set; }
        /// <summary>
        /// 留位时间
        /// </summary>
        public DateTime LiuWeiShiJian { get; set; }
    }

    /// <summary>
    /// 订单提醒列表查询信息业务实体
    /// </summary>
    /// 汪奇志 2012-04-09
    public class MDingDanTiXingSearchInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MDingDanTiXingSearchInfo() { }
    }
    #endregion

    #region 送机计划表实体

    /// <summary>
    /// 送机计划表实体
    /// </summary>
    public class SongJiJiHuaBiao
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 团队类型
        /// </summary>
        public EnumType.TourStructure.TourType TourType { get; set; }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 组团社编号
        /// </summary>
        public int BuyCompanyId { get; set; }

        /// <summary>
        /// 组团社名称
        /// </summary>
        public string BuyCompanyName { get; set; }

        /// <summary>
        /// 成人数
        /// </summary>
        public int ChenRenShu { get; set; }

        /// <summary>
        /// 儿童数
        /// </summary>
        public int ErTongShu { get; set; }

        /// <summary>
        /// 全陪数
        /// </summary>
        public int QuanPeiShu { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public EyouSoft.Model.TourStructure.TourOrderAmountPlusInfo TourOrderAmountPlusInfo { get; set; }

        /// <summary>
        /// 游客信息集合
        /// </summary>
        public IList<TourOrderCustomer> OrderCustomers { set; get; }
    }

    #endregion

}
