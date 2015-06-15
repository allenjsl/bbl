using System;
using System.Collections.Generic;

namespace EyouSoft.Model.PlanStructure
{
    #region LocalTravelAgencyInfo 安排地接社
    /// <summary>
    /// 安排地接社
    /// autor:李焕超 date:2011-1-17
    /// </summary>
    [Serializable]
    public class LocalTravelAgencyInfo
    {
        public LocalTravelAgencyInfo()
        { }
        #region Model
        private string _id;
        private string _tourid;
        private string _localtravelagency;
        private int _travelagencyid;
        private string _contacter;
        private string _contactTel;
        private DateTime _receivetime;
        private DateTime _delivertime;
        private decimal _fee;
        private decimal _commission;
        private decimal _settlement;
        private EyouSoft.Model.EnumType.TourStructure.RefundType _paytype;
        private string _notice;
        private string _remark;
        private int _operatorid;
        private string _operator;
        private DateTime _operatetime;
        /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId
        {
            set { _tourid = value; }
            get { return _tourid; }
        }
        /// <summary>
        /// 地接社名称
        /// </summary>
        public string LocalTravelAgency
        {
            set { _localtravelagency = value; }
            get { return _localtravelagency; }
        }
        /// <summary>
        /// 地接社名称编号
        /// </summary>
        public int TravelAgencyID
        {
            set { _travelagencyid = value; }
            get { return _travelagencyid; }
        }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacter
        {
            set { _contacter = value; }
            get { return _contacter; }
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactTel
        {
            set { _contactTel = value; }
            get { return _contactTel; }
        }
        /// <summary>
        /// 接团时间
        /// </summary>
        public DateTime ReceiveTime
        {
            set { _receivetime = value; }
            get { return _receivetime; }
        }
        /// <summary>
        /// 送团时间
        /// </summary>
        public DateTime DeliverTime
        {
            set { _delivertime = value; }
            get { return _delivertime; }
        }
        /// <summary>
        /// 团款
        /// </summary>
        public decimal Fee
        {
            set { _fee = value; }
            get { return _fee; }
        }
        /// <summary>
        /// 返利
        /// </summary>
        public decimal Commission
        {
            set { _commission = value; }
            get { return _commission; }
        }
        /// <summary>
        /// 结算费用
        /// </summary>
        public decimal Settlement
        {
            set { _settlement = value; }
            get { return _settlement; }
        }
        /// <summary>
        /// 支付方式
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.RefundType PayType
        {
            set { _paytype = value; }
            get { return _paytype; }
        }
        /// <summary>
        /// 地接社需知
        /// </summary>
        public string Notice
        {
            set { _notice = value; }
            get { return _notice; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 操作员编号
        /// </summary>
        public int OperatorID
        {
            set { _operatorid = value; }
            get { return _operatorid; }
        }
        /// <summary>
        /// 操作员
        /// </summary>
        public string Operator
        {
            set { _operator = value; }
            get { return _operator; }
        }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime
        {
            set { _operatetime = value; }
            get { return _operatetime; }
        }
        //增加费用
        public decimal AddAmount
        { get; set; }
        //减少费用
        public decimal ReduceAmount
        { set; get; }
        //合计金额
        public decimal TotalAmount
        { set; get; }
        //财务备注
        public string FRemark
        { set; get; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId
        { set; get; }

        public IList<TravelAgencyPriceInfo> TravelAgencyPriceInfoList
        {
            set;
            get;
        }
        public IList<LocalAgencyTourPlanInfo> LocalAgencyTourPlanInfoList
        {
            set;
            get;
        }
        #endregion Model

    }
    #endregion

    #region TravelAgencyPriceInfo 安排地接价格组成
    /// <summary>
    /// 安排地接价格组成
    /// </summary>
    [Serializable]
    public class TravelAgencyPriceInfo
    {
        public TravelAgencyPriceInfo()
        { }
        #region Model
        private int _id;
        private EyouSoft.Model.EnumType.CompanyStructure.PriceComponent _pricetpyeAorB;
        private string _title;
        private decimal _price;
        private string _remark;
        private int _peoplecount;
        private string _referenceid;
        private EyouSoft.Model.EnumType.TourStructure.ServiceType _PriceTpyeId;

        /// <summary>
        /// 编号
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 价格项目类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.PriceComponent PriceTpyeAorB
        {
            set { _pricetpyeAorB = value; }
            get { return _pricetpyeAorB; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price
        {
            set { _price = value; }
            get { return _price; }
        }
        /// <summary>
        /// 要求
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleCount
        {
            set { _peoplecount = value; }
            get { return _peoplecount; }
        }
        /// <summary>
        /// 安排地接旅行社的关联编号
        /// </summary>
        public string ReferenceID
        {
            set { _referenceid = value; }
            get { return _referenceid; }
        }
        /// <summary>
        /// 价格项目编号
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.ServiceType PriceTpyeId
        {
            set { _PriceTpyeId = value; }
            get { return _PriceTpyeId; }
        }
        #endregion Model

    }
    #endregion

    #region LocalAgencyTourPlanInfo 地接社团队行程关系表
    /// <summary>
    /// 地接社团队行程关系表
    /// </summary>
    [Serializable]
    public class LocalAgencyTourPlanInfo
    {
        public LocalAgencyTourPlanInfo()
        { }
        #region Model
        private string _localtravelid;
        private int _days;
        /// <summary>
        /// 地接安排编号
        /// </summary>
        public string LocalTravelId
        {
            set { _localtravelid = value; }
            get { return _localtravelid; }
        }
        /// <summary>
        /// 团队行程第几天
        /// </summary>
        public int Days
        {
            set { _days = value; }
            get { return _days; }
        }
        #endregion Model

    }
    #endregion

    #region 利润统计支出 2011-1-25 
    [Serializable]
    public class PersonalStatics
    {
       /// <summary>
        /// 线路名称
       /// </summary>
        public string RouteName
        { set; get; }

        /// <summary>
        /// 团号
        /// </summary>
        public string TourNo
        { set; get; }

       /// <summary>
       /// 出团日期
       /// </summary>
        public DateTime LeaveDate
        { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public String SellCompanyName
        { get; set; }

        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleCount
        { get; set; }

        /// <summary>
        ///金额
        /// </summary>
        public decimal Total
        { get; set; }
    
  
    }
    #endregion

    #region 团队散拼支出列表 2011-1-26
    [Serializable]
    public class PaymentList
    {
        /// <summary>
        /// 计调项目编号
        /// </summary>
        public string Id { set; get; }
        /// <summary>
        /// 供应商编号
        /// </summary>
        public int SupplierId { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string SuplierName { set; get; }
        /// <summary>
        /// 支出金额
        /// </summary>
        public decimal PayAmount { set; get; }
        /// <summary>
        /// 已支付金额
        /// </summary>
        public decimal PayedAmount { set; get; }
        /// <summary>
        /// 增加金额
        /// </summary>
        public decimal AddAmount { get; set; }
        /// <summary>
        /// 减少金额
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
        /// 项目类型 地接或机票
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.SupplierType SupplierType { set; get; }
        /// <summary>
        /// 增加减少费用信息集合，string.IsNullOrEmpty(ItemId)==true视为新增操作 ==false视为修改操作
        /// </summary>
        public IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> GoldDetails { get; set; }
    }   
    #endregion

    #region 地接机票应付款类 2011-2-15
    [Serializable]
    public class ArrearInfo
    {
        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId
        { set; get; }

        /// <summary>
        /// 团号
        /// </summary>
        public string  TourCode
        { set; get; }

        /// <summary>
        /// 出团日期
        /// </summary>
        public DateTime LeaveDate
        { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalAmount
        { get; set; }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName
        { get; set; }

        /// <summary>
        ///欠款金额
        /// </summary>
        public decimal Arrear
        { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier
        { set; get; }

        /// <summary>
        /// 操作员
        /// </summary>
        public string Operator
        { set; get; }
        /// <summary>
        /// 计划类型
        /// </summary>
        public EnumType.TourStructure.TourType TourType { get; set; }

    }
    #endregion

    #region 统计分析--地接机票应付款搜索条件类 2011-2-15
    [Serializable]
    public class ArrearSearchInfo
    {

        /// <summary>
        /// 团队类型
        /// </summary>
        public int TourType
        { set; get; }

        /// <summary>
        /// 出团日期开始
        /// </summary>
        public DateTime? LeaveDate1
        { get; set; }

        /// <summary>
        /// 出团日期结束
        /// </summary>
        public DateTime? LeaveDate2
        { get; set; }

        /// <summary>
        /// 线路区域编号
        /// </summary>
        public int AreaId
        { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        public int OperateId
        { set; get; }

        /// <summary>
        /// 销售员
        /// </summary>
        public int SalerId
        { set; get; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId
        { set; get; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public int[] SupplierId { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 查询类型 1：查询总支出；2：查询已付；3：查询未付
        /// </summary>
        public int SeachType { get; set; }

        /// <summary>
        /// 是否已结清（已结清True；未结清False；null不作为条件）
        /// </summary>
        public bool? IsAccount { get; set; }

    }
    #endregion

    #region 团款支出-应付账款-支付项目查询业务实体
    /// <summary>
    /// 团款支出-应付账款-支付项目查询业务实体
    /// </summary>
    /// 汪奇志 2012-02-02
    public class MExpendSearchInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MExpendSearchInfo() { }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 供应商类别
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.SupplierType? SupplierType { get; set; }

    }
    #endregion
}
