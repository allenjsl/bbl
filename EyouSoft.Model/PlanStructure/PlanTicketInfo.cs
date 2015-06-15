/*autor:李焕超 date:2011-1-17*/
using System;
using System.Collections.Generic;
using EyouSoft.Model.TourStructure;

namespace EyouSoft.Model.PlanStructure
{
    #region TicketOutListInfo 出票列表及查询联合的机票大实体
    /// <summary>
    /// 出票列表及查询联合的机票大实体
    /// </summary> 
    [Serializable]
    public class TicketOutListInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TicketOutListInfo() { }

        /// <summary>
        /// 出票表ID
        /// </summary>
        public string TicketOutId { get; set; }
        /// <summary>
        /// 机票列表Id
        /// </summary>
        public int TicketOutListId { get; set; }
        /// <summary>
        ///团号 
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        ///订单号 
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 订单退票明细Id
        /// </summary>
        public string RefundId { get; set; }
        /// <summary>
        /// 机票类型（团队申请机票、订单退票，订单申请机票）
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TicketType TicketType { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        ///  PNR
        /// </summary>
        public string PNR { set; get; }
        /// <summary>
        /// 总费用
        /// </summary>
        public decimal Total { set; get; }
        /// <summary>
        /// 订票需知
        /// </summary>
        public string Notice { set; get; }
        /// <summary>
        /// 机票供应商
        /// </summary>
        public string TicketOffice { set; get; }
        /// <summary>
        /// 机票供应商编号
        /// </summary>
        public int TicketOfficeId { set; get; }
        /// <summary>
        /// 销售员
        /// </summary>
        public string Saler { set; get; }
        /// <summary>
        /// 票号
        /// </summary>
        public string TicketNum { set; get; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.RefundType PayType { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// 状态
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TicketState State { set; get; }
        /// <summary>
        /// 退票状态
        /// </summary>
        //public EyouSoft.Model.EnumType.PlanStructure.TicketRefundSate RefundSate { set; get; }
        /// <summary>
        /// 操作人员
        /// </summary>
        public string Operator { set; get; }
        /// <summary>
        /// 操作人员编号
        /// </summary>
        public int OperateID { set; get; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyID { set; get; }
        /// <summary>
        /// 增加费用
        /// </summary>
        public decimal AddAmount { get; set; }
        /// <summary>
        /// 减少费用
        /// </summary>
        public decimal ReduceAmount { set; get; }
        /// <summary>
        /// 退还费用
        /// </summary>
        //public decimal ReturnAmount { set; get; }
        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal TotalAmount { set; get; }
        /// <summary>
        /// 财务备注
        /// </summary>
        public string FRemark { set; get; }
        /// <summary>
        /// 机票申请与游客对应关系
        /// </summary>
        public IList<TicketOutCustomerInfo> TicketOutCustomerInfoList { set; get; }
        /// <summary>
        /// 申请机票游客列表
        /// </summary>
        public IList<EyouSoft.Model.TourStructure.TourOrderCustomer> CustomerInfoList { get; set; }
        /// <summary>
        /// 机票航班信息
        /// </summary>
        public IList<TicketFlight> TicketFlightList { get; set; }
        /// <summary>
        /// 机票票款信息
        /// </summary>
        public IList<TicketKindInfo> TicketKindInfoList { get; set; }
        /// <summary>
        /// 机票申请人编号
        /// </summary>
        public int RegisterOperatorId { get; set; }
        /// <summary>
        /// 出票时间
        /// </summary>
        public DateTime? TicketOutTime { get; set; }
        /// <summary>
        /// 账务审核备注
        /// </summary>
        public string ReviewRemark
        {
            get;
            set;
        }

    }
    #endregion

    #region 出票信息
    [Serializable]
    /// <summary>
    /// 出票信息
    /// </summary>
    public class TicketOutInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TicketOutInfo() { }

        /// <summary>
        /// 编号
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// PNR
        /// </summary>
        public string PNR { get; set; }
        /// <summary>
        /// 总费用
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// 订票需知
        /// </summary>
        public string Notice { get; set; }
        /// <summary>
        /// 机票供应商
        /// </summary>
        public string TicketOffice { get; set; }
        /// <summary>
        /// 机票供应商编号
        /// </summary>
        public int TicketOfficeId { get; set; }
        /// <summary>
        /// 票号
        /// </summary>
        public string TicketNum { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.RefundType PayType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourID { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TicketState State { get; set; }
        /// <summary>
        /// 操作人员
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 操作人员编号
        /// </summary>
        public int OperateID { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyID { get; set; }
        //增加费用
        public decimal AddAmount { get; set; }
        /// <summary>
        /// 减少费用
        /// </summary>
        public decimal ReduceAmount { set; get; }
        //合计金额
        public decimal TotalAmount { set; get; }
        /// <summary>
        /// 财务备注
        /// </summary>
        public string FRemark { set; get; }
        /// <summary>
        /// 已支付金额
        /// </summary>
        public decimal PayAmount { set; get; }
        /// <summary>
        /// 机票申请与游客对应关系
        /// </summary>
        public IList<TicketOutCustomerInfo> TicketOutCustomerInfoList { get; set; }
        /// <summary>
        /// 机票航班信息
        /// </summary>
        public IList<TicketFlight> TicketFlightList { get; set; }
        /// <summary>
        /// 机票票款信息
        /// </summary>
        public IList<TicketKindInfo> TicketKindInfoList { get; set; }
        /// <summary>
        /// 申请机票游客列表
        /// </summary>
        public IList<EyouSoft.Model.TourStructure.TourOrderCustomer> CustomerInfoList { get; set; }
    }
    #endregion

    #region 实体类-机票申请与游客对应关系
    /// <summary>
    /// 实体类-机票申请与游客对应关系
    /// </summary>
    [Serializable]
    public class TicketOutCustomerInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TicketOutCustomerInfo() { }

        /// <summary>
        /// 申请编号
        /// </summary>
        public string TicketOutId { get; set; }
        /// <summary>
        /// 游客编号
        /// </summary>
        public string UserId { get; set; }
    }
    #endregion

    #region 机票航班信息业务实体
    /// <summary>
    /// 机票航班信息业务实体
    /// </summary>
    [Serializable]
    public class TicketFlight
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TicketFlight() { }

        /// <summary>
        /// 编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 航段
        /// </summary>
        public string FligthSegment { get; set; }
        /// <summary>
        /// 出港时间
        /// </summary>
        public DateTime DepartureTime { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public Model.EnumType.PlanStructure.FlightCompany AireLine { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// 出票编号
        /// </summary>
        public string TicketId { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string TicketTime { get; set; }

        /// <summary>
        /// 票号
        /// </summary>
        public string TicketNum { get; set; }
    }
    #endregion

    #region 机票票款信息业务实体
    /// <summary>
    /// 机票票款信息业务实体
    /// </summary>
    [Serializable]
    public class TicketKindInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TicketKindInfo() { }

        /// <summary>
        /// 票款自动编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 出票编号
        /// </summary>
        public string TicketId { get; set; }
        /// <summary>
        /// 票面价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 税/机建
        /// </summary>
        public decimal OilFee { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleCount { get; set; }
        /// <summary>
        /// 代理费
        /// </summary>
        public decimal AgencyPrice { get; set; }
        /// <summary>
        /// 票款
        /// </summary>
        public decimal TotalMoney { get; set; }
        /// <summary>
        /// 票种（儿童或成人）
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.KindType TicketType { get; set; }
        /// <summary>
        /// 百分比（折扣）
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// 其它费用
        /// </summary>
        public decimal OtherPrice { get; set; }
    }
    #endregion

    #region 机票管理 TicketInfo

    /// <summary>
    /// 机票管理
    /// </summary>
    /// autor:李焕超 date:2011-1-17
    [Serializable]
    public class TicketInfo
    {
        public TicketInfo() { }

        /// <summary>
        /// 编号
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// 团队Id
        /// </summary>
        public string TourId { set; get; }
        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderId { set; get; }
        /// <summary>
        /// 订单退票明细Id
        /// </summary>
        public string RefundId { set; get; }
        /// <summary>
        /// 机票类型（团队申请机票、订单退票，订单申请机票）
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TicketType TicketType { set; get; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourNum { set; get; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { set; get; }
        /// <summary>
        /// 销售员
        /// </summary>
        public string Saler { set; get; }
        /// <summary>
        /// 计调员
        /// </summary>
        public string Operator { set; get; }
        /// <summary>
        /// 计调员编号
        /// </summary>
        public int OperatorId { set; get; }
        /// <summary>
        /// 记录添加时间
        /// </summary>
        public DateTime RegisterTime { set; get; }
        /// <summary>
        /// 航段
        /// </summary>
        public IList<TicketFlight> TicketFlights { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TicketState State { set; get; }

        /// <summary>
        /// 出票时间
        /// </summary>
        public DateTime TicketOutTime { get; set; }
        /// <summary>
        /// 财务审核时间
        /// </summary>
        public DateTime? VerifyTime { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 客户单位编号(购买单位编号)
        /// </summary>
        public int BuyCompanyId { get; set; }

        /// <summary>
        /// 客户单位名称(购买单位名称)
        /// </summary>
        public string BuyCompanyName { get; set; }
    }

    #endregion Model

    #region 机票搜索参数
    [Serializable]
    public class TicketSearchModel
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TicketSearchModel() { }

        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { set; get; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { set; get; }
        /// <summary>
        /// 航班号/时间（航班信息） 	
        /// </summary>
        public string DepartureTime { set; get; }
        /// <summary>
        /// 航段（航班信息）
        /// </summary>
        public string FligthSegment { set; get; }
        /// <summary>
        /// 用于财务管理模块，或机票管理模块 1代表财务审核，2代表机票管理
        /// </summary>
        public int TicketListOrFinancialList { set; get; }

        /// <summary>
        /// 机票状态
        /// </summary>
        public EnumType.PlanStructure.TicketState? TicketState { get; set; }

        /// <summary>
        /// 日期-始（航班信息）
        /// </summary>
        public DateTime? AirTimeStart { get; set; }
        /// <summary>
        /// 日期-止（航班信息）
        /// </summary>
        public DateTime? AirTimeEnd { get; set; }

        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 审核起始时间
        /// </summary>
        public DateTime? SVerifyTime { get; set; }
        /// <summary>
        /// 审核截止时间
        /// </summary>
        public DateTime? EVerifyTime { get; set; }
        /// <summary>
        /// 出团起始时间
        /// </summary>
        public DateTime? LSDate { get; set; }
        /// <summary>
        /// 出团截止时间
        /// </summary>
        public DateTime? LEDate { get; set; }
    }

    #endregion

    #region 机票统计信息业务实体
    /// <summary>
    /// 机票统计信息业务实体
    /// </summary>
    [Serializable]
    public class TicketStatisticsModel
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public TicketStatisticsModel() { }
        /// <summary>
        /// 出票数/退票数
        /// </summary>
        public int TicketCount { set; get; }
        /// <summary>
        /// 应退金额
        /// </summary>
        public decimal NeedReturnAmount { set; get; }
        /// <summary>
        /// 已退金额
        /// </summary>
        public decimal ReturnedAmount { set; get; }
        /// <summary>
        /// 成交金额
        /// </summary>
        public decimal TotalAmount { set; get; }
        /// <summary>
        /// 月份
        /// </summary>
        public int Mon { set; get; }
        /// <summary>
        /// 年
        /// </summary>
        public int Years { set; get; }
    }

    #endregion

    #region 机票统计搜索参数信息业务实体
    /*/// <summary>
    /// 机票统计搜索参数信息业务实体
    /// </summary>
    [Serializable]
    public class TicketStatisticsSearchModel
    {
        public TicketStatisticsSearchModel() { }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { set; get; }
        /// <summary>
        /// 团队类型（散客，团队）1团队，2散客
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType? TicketType { set; get; }
        /// <summary>
        /// 线路区域
        /// </summary>
        public int AreaRouteId { set; get; }
        /// <summary>
        /// 部门
        /// </summary>
        public int[] DepartmentId { set; get; }
        /// <summary>
        /// 人员
        /// </summary>
        public int[] OperatorId { set; get; }
        /// <summary>
        /// 机票状态
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TicketState TicketState { set; get; }
        /// <summary>
        /// 出团日期搜索开始
        /// </summary>
        public DateTime? LeaveDateBegin { set; get; }
        /// <summary>
        /// 出团日期搜索结束
        /// </summary>
        public DateTime? LeaveDateEnd { set; get; }
    }*/

    #endregion

    #region 机票退票参数类
    /// <summary>
    /// 机票退票参数类
    /// </summary>
    [Serializable]
    public class TicketRefundModel
    {
        public TicketRefundModel() { }
        /// <summary>
        /// 订票编号
        /// </summary>
        public string TicketId { set; get; }
        /// <summary>
        /// 退还金额
        /// </summary>
        public decimal ReturnMoney { set; get; }
        /// <summary>
        /// 状态
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TicketState State { set; get; }
        /// <summary>
        /// 须知及备注
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// 退票游客
        /// </summary>
        public IList<EyouSoft.Model.TourStructure.TourOrderCustomer> TourOrderCustomerList { set; get; }
    }

    #endregion

    #region 出票统计明细实体
    /// <summary>
    /// 出票统计明细实体
    /// 出票统计出票量详细页使用此实体
    /// </summary>
    /// 周文超   2011-03-22
    public class TicketOutStatisticInfo
    {
        /// <summary>
        /// PNR
        /// </summary>
        public string PNR { get; set; }
        /// <summary>
        /// 票号
        /// </summary>
        public string TicketNum { get; set; }
        /// <summary>
        /// 票款金额
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 出票人数
        /// </summary>
        public int PeopleCount { get; set; }
        /// <summary>
        /// 机票航班信息集合
        /// </summary>
        public IList<TicketFlight> TicketFlightList { get; set; }
        /// <summary>
        /// 机票价格信息集合
        /// </summary>
        public IList<TicketKindInfo> TicketKindList { get; set; }
    }

    #endregion

    #region 机票申请列表信息业务实体
    /// <summary>
    /// 机票申请列表信息业务实体
    /// </summary>
    /// Author:汪奇志 2011-04-19
    public class MLBTicketApplyInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MLBTicketApplyInfo() { }

        /// <summary>
        /// 机票申请编号[计调安排编号]
        /// </summary>
        public string ApplyId { get; set; }
        /// <summary>
        /// 机票状态
        /// </summary>
        public EnumType.PlanStructure.TicketState Status { get; set; }
        /// <summary>
        /// 计划编号
        /// </summary>
        public string TourId { get; set; }
        /// <summary>
        /// 航段信息集合
        /// </summary>
        public IList<TicketFlight> TicketFlights { get; set; }
        /// <summary>
        /// 成人票款信息
        /// </summary>
        public TicketKindInfo FundAdult { get; set; }
        /// <summary>
        /// 儿童票款信息业务实体
        /// </summary>
        public TicketKindInfo FundChildren { get; set; }
        /// <summary>
        /// 总票款(增加减少费用前)
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 增加费用
        /// </summary>
        public decimal AddAmount { get; set; }
        /// <summary>
        /// 减少费用
        /// </summary>
        public decimal ReduceAmount { get; set; }
        /// <summary>
        /// 合计金额(增加减少费用后)
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// PNR
        /// </summary>
        public string PNR { get; set; }
        /// <summary>
        /// 机票申请时间
        /// </summary>
        public DateTime ApplyTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
    #endregion

    #region 机票退票信息业务实体，用于机票管理退票操作
    /// <summary>
    /// 机票退票信息业务实体，用于机票管理退票操作
    /// </summary>
    public class MRefundTicketInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MRefundTicketInfo() { }

        /// <summary>
        /// 机票管理列表编号
        /// </summary>
        public int TicketListId { get; set; }
        /// <summary>
        /// 退回金额
        /// </summary>
        public decimal RefundAmount { get; set; }
        /// <summary>
        /// 退回状态
        /// </summary>
        public EnumType.PlanStructure.TicketRefundSate Status { get; set; }
        /// <summary>
        /// 退票需知
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 操作员编号
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// 退票所在申请的计划编号
        /// </summary>
        public string TourId { get; set; }
    }
    #endregion

    #region 机票航班信息业务实体（含机票申请状态）

    /// <summary>
    /// 机票航班信息业务实体（含机票申请状态）
    /// </summary>
    public class MTicketFlightAndState : TicketFlight
    {
        /// <summary>
        /// 机票申请状态
        /// </summary>
        public EyouSoft.Model.EnumType.PlanStructure.TicketState Status { get; set; }
    }

    #endregion

    #region 财务管理机票审核实体

    /// <summary>
    /// 财务管理机票审核实体
    /// </summary>
    [Serializable]
    public class MCheckTicketInfo
    {
        /// <summary>
        /// 机票申请Id
        /// </summary>
        public string TicketId { get; set; }

        /// <summary>
        /// 机票状态
        /// </summary>
        public EnumType.PlanStructure.TicketState TicketState { get; set; }

        /// <summary>
        /// 机票航班信息
        /// </summary>
        public IList<TicketFlight> TicketFlightList { get; set; }
        /// <summary>
        /// 机票票款信息
        /// </summary>
        public IList<TicketKindInfo> TicketKindList { get; set; }
        /// <summary>
        /// 申请机票游客列表
        /// </summary>
        public IList<TourOrderCustomer> CustomerList { get; set; }

        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal TotalAmount { set; get; }

        /// <summary>
        /// 客户单位
        /// </summary>
        public string CustomerNames { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal OrderTotalAmount { get; set; }

        /// <summary>
        /// 已收金额
        /// </summary>
        public decimal HasOrderAmount { get; set; }

        /// <summary>
        /// 出票人数
        /// </summary>
        public int TicketNum { get; set; }

        /// <summary>
        /// 未收金额
        /// </summary>
        public decimal NoOrderAmount
        {
            get { return OrderTotalAmount - HasOrderAmount; }
        }

        /// <summary>
        /// 账务审核备注
        /// </summary>
        public string ReviewRemark
        {
            get;
            set;
        }
    }

    #endregion
}
