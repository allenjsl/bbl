using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.PersonalCenterStructure
{

    #region 个人中心-收款提醒实体
    /// <summary>
    /// 个人中心-收款提醒实体
    /// </summary>
    /// 鲁功源 2011-01-30
    public class ReceiptRemind
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReceiptRemind() { }

        #region 属性

        /// <summary>
        /// 客户Id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 欠款单位名称
        /// </summary>
        public string CustomerName
        {
            get;
            set;
        }
        /// <summary>
        /// 联系人名称（主要联系人）
        /// </summary>
        public string ContactName
        {
            get;
            set;
        }
        /// <summary>
        /// 联系电话（主要联系人电话）
        /// </summary>
        public string ContactTel
        {
            get;
            set;
        }
        /// <summary>
        /// 欠款总额（总所有未收款）
        /// </summary>
        public decimal ArrearCash
        {
            get;
            set;
        }
        /// <summary>
        /// 销售员姓名（所有接过这家客户订单的销售）
        /// </summary>
        public string SalerName
        {
            get;
            set;
        }
        /// <summary>
        /// 销售员编号，一个客户单位一个销售员一行时是销售员编号，其它时为0
        /// </summary>
        public int SellerId { get; set; }
        #endregion
    }
    #endregion

    #region 个人中心-付款提醒实体
    /// <summary>
    /// 个人中心-付款提醒实体
    /// </summary>
    public class PayRemind
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PayRemind() { }

        #region 属性

        /// <summary>
        /// 供应商编号
        /// </summary>
        public int SupplierId { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get;
            set;
        }

        /// <summary>
        /// 供应商类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.SupplierType SupplierType { get; set; }

        /// <summary>
        /// 联系人名称（供应商第一个联系人）
        /// </summary>
        public string ContactName
        {
            get;
            set;
        }

        /// <summary>
        /// 联系电话（供应商第一个联系电话）
        /// </summary>
        public string ContactTel
        {
            get;
            set;
        }

        /// <summary>
        /// 欠款总额（汇总所有未收款）
        /// </summary>
        public decimal PayCash
        {
            get;
            set;
        }

        /// <summary>
        /// 计调员姓名（所有安排过这家供应商的计调）
        /// </summary>
        public string JobName
        {
            get;
            set;
        }

        #endregion
    }
    #endregion

    #region 个人中心-出团提醒实体
    /// <summary>
    /// 个人中心-出团提醒实体
    /// </summary>
    /// 鲁功源 2011-01-30
    public class LeaveTourRemind
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LeaveTourRemind() { }

        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId
        {
            get;
            set;
        }

        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode
        {
            get;
            set;
        }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName
        {
            get;
            set;
        }
        /// <summary>
        /// 出团时间
        /// </summary>
        public DateTime LeaveDate
        {
            get;
            set;
        }
        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleCount
        {
            get;
            set;
        }
        /// <summary>
        /// 计调员姓名
        /// </summary>
        public string JobName
        {
            get;
            set;
        }

        /// <summary>
        /// 客户单位信息集合
        /// </summary>
        public IList<MTourRemindTravelAgencyInfo> AgencyInfo { get; set; }

    }
    #endregion

    #region 个人中心-回团提醒实体
    /// <summary>
    /// 个人中心-回团提醒实体
    /// </summary>
    public class BackTourRemind
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BackTourRemind() { }

        /// <summary>
        /// 团队编号
        /// </summary>
        public string TourId
        {
            get;
            set;
        }

        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode
        {
            get;
            set;
        }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName
        {
            get;
            set;
        }
        /// <summary>
        /// 出团时间
        /// </summary>
        public DateTime BackDate
        {
            get;
            set;
        }
        /// <summary>
        /// 人数
        /// </summary>
        public int PeopleCount
        {
            get;
            set;
        }
        /// <summary>
        /// 计调员姓名
        /// </summary>
        public string JobName
        {
            get;
            set;
        }

        /// <summary>
        /// 客户单位信息集合
        /// </summary>
        public IList<MTourRemindTravelAgencyInfo> AgencyInfo { get; set; }
    }
    #endregion

    #region 个人中心-劳动合同到期提醒实体
    /// <summary>
    /// 个人中心-劳动合同到期提醒实体
    /// </summary>
    public class ContractReminder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ContractReminder() { }

        #region 属性
        /// <summary>
        /// 员工编号
        /// </summary>
        public string StaffNo
        {
            get;
            set;
        }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string StaffName
        {
            get;
            set;
        }
        /// <summary>
        /// 签订时间
        /// </summary>
        public DateTime SignDate
        {
            get;
            set;
        }
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime ExpireDate
        {
            get;
            set;
        }
        #endregion

    }

    /// <summary>
    /// 个人中心-公告通知实体
    /// </summary>
    public class NoticeNews
    {
        /// <summary>
        /// 消息编号
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int ClickNum
        {
            get;
            set;
        }

        /// <summary>
        /// 发布人
        /// </summary>
        public string OperateName
        {
            get;
            set;
        }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? IssueTime
        {
            get;
            set;
        }
    }
    #endregion

    #region 出回团提醒组团社信息业务实体
    /// <summary>
    /// 出回团提醒组团社信息业务实体
    /// </summary>
    public class MTourRemindTravelAgencyInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MTourRemindTravelAgencyInfo() { }

        /// <summary>
        /// 客户单位编号
        /// </summary>
        public int AgencyId { get; set; }
        /// <summary>
        /// 客户单位名称
        /// </summary>
        public string AgencyName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 联系手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 联系传真
        /// </summary>
        public string Fax { get; set; }

    }
    #endregion

    #region 个人中心-收款提醒查询实体
    /// <summary>
    /// 个人中心-收款提醒查询实体
    /// </summary>
    public class ReceiptRemindSearchInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public ReceiptRemindSearchInfo() { }
        /// <summary>
        /// 客源单位
        /// </summary>
        public string QianKuanDanWei { get; set; }
        /// <summary>
        /// 出团起始时间
        /// </summary>
        public DateTime? LSDate { get; set; }
        /// <summary>
        /// 出团截止时间
        /// </summary>
        public DateTime? LEDate { get; set; }
        /// <summary>
        /// 计划操作员编号集合
        /// </summary>
        public int[] OperatorIds { get; set; }
        /// <summary>
        /// 计划操作员所在部门编号集合
        /// </summary>
        public int[] OperatorDepartIds { get; set; }
    }
    #endregion

    #region 个人中心-付款提醒查询实体
    /// <summary>
    /// FuKuanTiXingChaXun
    /// </summary>
    /// 汪奇志 2012-02-23
    public class FuKuanTiXingChaXun
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public FuKuanTiXingChaXun() { }

        /// <summary>
        /// 出团起始时间
        /// </summary>
        public DateTime? LSDate { get; set; }
        /// <summary>
        /// 出团截止时间
        /// </summary>
        public DateTime? LEDate { get; set; }
        /// <summary>
        /// 计划操作员编号集合
        /// </summary>
        public int[] OperatorIds { get; set; }
        /// <summary>
        /// 计划操作员所在部门编号集合
        /// </summary>
        public int[] OperatorDepartIds { get; set; }
        /// <summary>
        /// 收款单位
        /// </summary>
        public string ShouKuanDanWei { get; set; }
    }
    #endregion
}
