using System;
using System.Collections.Generic;
namespace EyouSoft.Model.CompanyStructure
{
    #region 客户资料信息业务实体
    /// <summary>
    /// 实体类tbl_Customer，客户资料
    /// autor:李焕超
    /// creat time :2011-1-17
    /// </summary>
    [Serializable]
    public class CustomerInfo
    {
        bool _IsEnable = true;

        /// <summary>
        /// default constructor
        /// </summary>
        public CustomerInfo() { }

        /// <summary>
        /// 
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// 省份编号
        /// </summary>
        public int ProviceId { set; get; }
        /// <summary>
        /// 省份名称
        /// </summary>
        public string ProvinceName { set; get; }
        /// <summary>
        /// 城市编号
        /// </summary>
        public int CityId { set; get; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { set; get; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 许可证
        /// </summary>
        public string Licence { set; get; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Adress { set; get; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string PostalCode { set; get; }
        /// <summary>
        /// 银行账号
        /// </summary>
        public string BankAccount { set; get; }
        /// <summary>
        /// 最高欠款
        /// </summary>
        public decimal MaxDebts { set; get; }
        /// <summary>
        /// 预存款
        /// </summary>
        public decimal PreDeposit { set; get; }
        /// <summary>
        /// 返佣金额
        /// </summary>
        public decimal CommissionCount { set; get; }
        /// <summary>
        /// 责任销售编号
        /// </summary>
        public int SaleId { set; get; }
        /// <summary>
        /// 责任销售
        /// </summary>
        public string Saler { set; get; }
        /// <summary>
        /// 客户等级
        /// </summary>
        public int CustomerLev { set; get; }
        /// <summary>
        /// 客户等级名称
        /// </summary>
        public string LevelName { get; set; }
        /// <summary>
        /// 返佣类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.CommissionType CommissionType { set; get; }
        /// <summary>
        /// 所属公司
        /// </summary>
        public int CompanyId { set; get; }
        /// <summary>
        /// 是否启用 1:启用  0:停用
        /// </summary>
        public bool IsEnable { set { this._IsEnable = value; } get { return this._IsEnable; } }
        /// <summary>
        /// 主要联系人姓名
        /// </summary>
        public string ContactName { set; get; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { set; get; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { set; get; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
        private int _tradeTourNum = 0;
        /// <summary>
        /// 团队合计人数
        /// </summary>
        public int TradeTourNum { set { _tradeTourNum = value; } get { return _tradeTourNum; } }
        /// <summary>
        /// 交易次数
        /// </summary>
        public int TradeNum { set; get; }
        /// <summary>
        /// 满意度
        /// </summary>
        public decimal SatNum { set; get; }
        /// <summary>
        /// 投诉数量
        /// </summary>
        public int ComplaintNum { set; get; }
        /// <summary>
        /// 添加人
        /// </summary>
        public int OperatorId { set; get; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { set; get; }
        /// <summary>
        /// 打印页眉
        /// </summary>
        public string PageHeadFile { set; get; }
        /// <summary>
        /// 打印页脚
        /// </summary>
        public string PageFootFile { set; get; }
        /// <summary>
        /// 打印模版
        /// </summary>
        public string TemplateFile { set; get; }
        /// <summary>
        /// 客户公章
        /// </summary>
        public string CustomerStamp { set; get; }
        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDelete { set; get; }
        /// <summary>
        /// 联系人
        /// </summary>
        public IList<EyouSoft.Model.CompanyStructure.CustomerContactInfo> CustomerContactList { set; get; }
        public int BrandId { set; get; }
        public EyouSoft.Model.CompanyStructure.CompanyBrand CompanyBrand { set; get; }
        /// <summary>
        /// 结算日期
        /// </summary>
        /// <remarks>用于控制收款提醒，根据结算日类型来确定，按月：在每月X号前不提醒，X号开始提醒，X=0时该客户有欠款就提醒，按周：每周的星期X后的4天</remarks>
        public int AccountDay { get; set; }
        /// <summary>
        /// 结算方式
        /// </summary>
        public string AccountWay { get; set; }
        /// <summary>
        /// 结算日类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.AccountDayType AccountDayType { get; set; }
        /// <summary>
        /// logo
        /// </summary>
        public string FilePathLogo { get; set; }
        /// <summary>
        /// 结算方式
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.KHJieSuanType JieSuanType { get; set; }
        /// <summary>
        /// 对方团号是否必填
        /// </summary>
        public bool IsRequiredTourCode { get; set; }
    }
    #endregion

    #region 客户资料配置信息业务实体
    /// <summary>
    /// 客户资料配置信息
    /// </summary>
    [Serializable]
    public class CustomerConfig
    {
       /// <summary>
       /// 编号
       /// </summary>
        public int Id
        {
            set;
            get;

        }

        /// <summary>
        /// 打印页眉
        /// </summary>
        public string PageHeadFile
        {
            set;
            get;
        }
        /// <summary>
        /// 打印页脚
        /// </summary>
        public string PageFootFile
        {
            set;
            get;
        }
        /// <summary>
        /// 打印模版
        /// </summary>
        public string TemplateFile
        {
            set;
            get;
        }
        /// <summary>
        /// 客户公章
        /// </summary>
        public string CustomerStamp
        {
            set;
            get;
        }
        /// <summary>
        /// logo
        /// </summary>
        public string FilePathLogo { get; set; }
    }
    #endregion

    #region 客户资料查询信息业务实体    
    /// <summary>
    /// 客户资料查询信息业务实体
    /// </summary>
    /// Author:汪奇志 2011-04-18
    public class MCustomerSeachInfo
    {
        EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField _orderByField = EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField.创建时间;
        EyouSoft.Model.EnumType.CompanyStructure.OrderByType _orderByType = EyouSoft.Model.EnumType.CompanyStructure.OrderByType.DESC;


        /// <summary>
        /// default constructor
        /// </summary>
        public MCustomerSeachInfo() { }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactName { get;set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactTelephone { get; set; }
        /// <summary>
        /// 省份编号
        /// </summary>
        public int? ProvinceId { get; set; }
        /// <summary>
        /// 城市编号
        /// </summary>
        public int? CityId { get; set; }
        /// <summary>
        /// 责任销售姓名
        /// </summary>
        public string SellerName { get; set; }
        /// <summary>
        /// 责任销售编号集合
        /// </summary>
        public int[] SellerIds { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.CustomerOrderByField OrderByField
        {
            get { return _orderByField; }
            set { _orderByField = value; }
        }
        /// <summary>
        /// 排序方式
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.OrderByType OrderByType
        {
            get { return _orderByType; }
            set { _orderByType = value; }
        }
        /// <summary>
        /// 联系手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 城市编号集合
        /// </summary>
        public int[] CityIdList { get; set; }
        /// <summary>
        /// 省份编号集合
        /// </summary>
        public int[] ProvinceIds { get; set; }
    }
    /// <summary>
    /// 组团社 销售统计查询实体
    /// </summary>
    public class MCustomerSoldStatSearchInfo : MCustomerSeachInfo
    {
        private EyouSoft.Model.EnumType.FinanceStructure.QueryOperator searchCompare = EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.None;
        /// <summary>
        /// 客户单位编号
        /// </summary>
        public int? CustomerId {get;set;}        
        /// <summary>
        /// 出团起始时间
        /// </summary>
        public DateTime? TourStartDate { get; set; }
        /// <summary>
        /// 出团截止时间
        /// </summary>
        public DateTime? TourEndDate { get; set; }
        /// <summary>
        /// 下单起始时间
        /// </summary>
        public DateTime? OrderStartDate { get; set; }
        /// <summary>
        /// 下单截止时间
        /// </summary>
        public DateTime? OrderEndDate { get; set; }
        /// <summary>
        /// 比较方式 默认无,0:不进行比较 1:大于 2:小于
        /// </summary>
        public EyouSoft.Model.EnumType.FinanceStructure.QueryOperator SearchCompare { get { return searchCompare; } set { searchCompare = value; } }
        /// <summary>
        /// 送团人数
        /// </summary>
        public int? SendTourPeopleNumber { get; set; }
        /// <summary>
        /// 线路区域编号
        /// </summary>
        public int? AreaId { get; set; }
        /// <summary>
        /// 部门编号
        /// </summary>
        public int[] DeptIds { get; set; }
        /// <summary>
        /// 销售统计分析排序方式 0：合计人数DESC，1：合计人数ASC，2：散客人数DESC，3：散客人数ASC，4：团队人数DESC，5：团队人数ASC
        /// </summary>
        public int XSTJFXOrderByType { get; set; }
    }
    #endregion

    #region 客户关系交易明细业务实体
    /// <summary>
    /// 客户关系交易明细业务实体
    /// </summary>
    /// 汪奇志 2012-03-01
    public class KeHuJiaoYiMingXiInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public KeHuJiaoYiMingXiInfo() { }
        // 	 	 	 	 	 	 	
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 对方团号
        /// </summary>
        public string BuyerTourCode { get; set; }
        /// <summary>
        /// 报团人次
        /// </summary>
        public int RenCi { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal JiaoYiJInE { get; set; }
        /// <summary>
        /// 已收金额
        /// </summary>
        public decimal YiShouJinE { get; set; }
        /// <summary>
        /// 未收金额
        /// </summary>
        public decimal WeiShouJinE { get { return JiaoYiJInE - YiShouJinE; } }
        /// <summary>
        /// 同行操作员
        /// </summary>
        public string BuyerContactName { get; set; }
    }
    #endregion

    #region 客户关系交易明细查询业务实体
    /// <summary>
    /// 客户关系交易明细查询业务实体
    /// </summary>
    /// 汪奇志 2012-03-01
    public class KeHuJiaoYiMingXiChaXunInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public KeHuJiaoYiMingXiChaXunInfo() { }

        /*/// <summary>
        /// 团队类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType? TourType { get; set; }*/
        /// <summary>
        /// 出团起始时间
        /// </summary>
        public DateTime? LSDate { get; set; }
        /// <summary>
        /// 出团截止时间
        /// </summary>
        public DateTime? LEDate { get; set; }
        /// <summary>
        /// 款项操作符查询-查询类型
        /// </summary>
        public EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType? QueryAmountType { get; set; }
        /// <summary>
        /// 款项操作符查询-操作符
        /// </summary>
        public EyouSoft.Model.EnumType.FinanceStructure.QueryOperator? QueryAmountOperator { get; set; }
        /// <summary>
        /// 款项操作符查询-操作数
        /// </summary>
        public decimal? QueryAmount { get; set; }
        /// <summary>
        /// 团队类型集合
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType[] TourTypes { get; set; }
        /// <summary>
        /// 线路区域编号
        /// </summary>
        public int? AreaId { get; set; }
    }
    #endregion
}


