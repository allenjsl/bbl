using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EyouSoft.Model.CompanyStructure
{
    #region 公司Hash配置实体[键：值形式]
    /// <summary>
    /// 公司Hash配置实体[键：值形式]
    /// </summary>
    /// 鲁功源 2011-01-18
    public class CompanySetting
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompanySetting() { }
        #endregion

        #region 属性
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId
        {
            get;
            set;
        }
        /// <summary>
        /// 字段名称[Key]
        /// </summary>
        public string FieldName
        {
            get;
            set;
        }
        /// <summary>
        /// 字段数值[Value]
        /// </summary>
        public string FieldValue
        {
            get;
            set;
        }
        #endregion

    }
    #endregion

    #region 公司配置实体
    /// <summary>
    /// 公司配置实体
    /// </summary>
    [Serializable]
    public class CompanyFieldSetting
    {
        private EyouSoft.Model.EnumType.CompanyStructure.TeamNumberOfPeople _teamNumberOfPeople = EyouSoft.Model.EnumType.CompanyStructure.TeamNumberOfPeople.OnlyTotalNumber;
        private EyouSoft.Model.EnumType.CompanyStructure.TicketOfficeFillTime _ticketOfficeFillTime = EyouSoft.Model.EnumType.CompanyStructure.TicketOfficeFillTime.Ticket;

        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// 公司 LOGO
        /// </summary>
        public string CompanyLogo { get; set; }
        /// <summary>
        /// 合同到期提前X天提醒
        /// </summary>
        public int ContractReminderDays { get; set; }
        /// <summary>
        /// 出团提前X天提醒
        /// </summary>
        public int LeaveTourReminderDays { get; set; }
        /// <summary>
        /// 回团提前X天提醒
        /// </summary>
        public int BackTourReminderDays { get; set; }
        /// <summary>
        /// 最长留位时间(分钟)
        /// </summary>
        public int ReservationTime { get; set; }
        /// <summary>
        /// 价格组成配置
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.PriceComponent PriceComponent { get; set; }
        /// <summary>
        /// 列表显示控制前X月
        /// </summary>
        public int DisplayBeforeMonth { get; set; }
        /// <summary>
        /// 列表显示控制后X月
        /// </summary>
        public int DisplayAfterMonth { get; set; }
        /// <summary>
        /// 公司打印模版集合
        /// </summary>
        public IList<PrintDocument> PrintDocument { get; set; }
        /// <summary>
        /// 公司打印文件
        /// </summary>
        public CompanyPrintTemplate CompanyPrintFile { get; set; }
        /// <summary>
        /// 机票票款计算公式
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.AgencyFeeType AgencyFeeInfo { get; set; }
        /// <summary>
        /// 统计分析利润统计团队数页面路径配置
        /// </summary>
        public string ProfitStatTourPagePath { get; set; }
        /// <summary>
        /// 统计订单的方式
        /// </summary>
        public Model.EnumType.CompanyStructure.ComputeOrderType ComputeOrderType { get; set; }
        /// <summary>
        /// 申请机票游客勾选配置
        /// </summary>
        public EnumType.CompanyStructure.TicketTravellerCheckedType TicketTravellerCheckedType { get; set; }
        /// <summary>
        /// 收款提醒配置
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType ReceiptRemindType { get; set; }
        /// <summary>
        /// 游客信息是否必填（仅控制专线端，组团端不受此限制），为真时姓名、证件号码、电话必录，证件号码和电话验证格式，且必须有一个游客信息
        /// </summary>
        public bool IsRequiredTraveller { get; set; }
        /// <summary>
        /// 团队计划人数配置
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.TeamNumberOfPeople TeamNumberOfPeople
        {
            get { return _teamNumberOfPeople; }
            set { _teamNumberOfPeople = value; }
        }
        /// <summary>
        /// 机票售票处填写时间
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.TicketOfficeFillTime TicketOfficeFillTime
        {
            get { return _ticketOfficeFillTime; }
            set { _ticketOfficeFillTime = value; }
        }
        /// <summary>
        /// 完成出票机票款是否自动结清
        /// </summary>
        public bool IsTicketOutRegisterPayment { get; set; }
        /// <summary>
        /// 回款率是否包含未审核的收款
        /// </summary>
        public bool HuiKuanLvSFBHWeiShenHe { get; set; }
        /// <summary>
        /// 同行平台-团队展示方式
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType SiteTourDisplayType { get; set; }
        /// <summary>
        /// 同行平台-模板编号
        /// </summary>
        public EyouSoft.Model.EnumType.SysStructure.SiteTemplate SiteTemplate { get; set; }
        /// <summary>
        /// 弹窗提醒间隔时间（单位：秒）
        /// </summary>
        public int TanChuangTiXingInterval { get; set; }

        //public object Clone()
        //{
        //    using (var stream = new MemoryStream())
        //    {
        //        var formatter = new BinaryFormatter();
        //        formatter.Serialize(stream, this);
        //        stream.Position = 0;
        //        var newObj = formatter.Deserialize(stream);
        //        return newObj;
        //    }
        //}

        /// <summary>
        /// 送团人编号
        /// </summary>
        public string SongTuanRenId { get; set; }
        /// <summary>
        /// 送团人姓名
        /// </summary>
        public string SongTuanRenName { get; set; }
        /// <summary>
        /// 集合地点
        /// </summary>
        public string JiHeDiDian { get; set; }
        /// <summary>
        /// 集合标志
        /// </summary>
        public string JiHeBiaoZhi { get; set; }
    }
    #endregion

    #region 公司打印模版配置实体
    [Serializable]
    public class PrintDocument
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public PrintDocument() { }
        #endregion

        #region 属性
        /// <summary>
        /// 打印模版类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.PrintTemplateType PrintTemplateType
        {
            get;
            set;
        }
        /// <summary>
        /// 打印模版文件路径
        /// </summary>
        public string PrintTemplate
        {
            get;
            set;
        }
        #endregion

    }
    #endregion

    #region 公司打印文件实体
    /// <summary>
    /// 公司打印文件实体
    /// </summary>
    /// 鲁功源 2011-02-14
    [Serializable]
    public class CompanyPrintTemplate
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CompanyPrintTemplate() { }
        #endregion

        #region 属性
        /// <summary>
        /// 打印页眉
        /// </summary>
        public string PageHeadFile
        {
            get;
            set;
        }
        /// <summary>
        /// 打印页脚
        /// </summary>
        public string PageFootFile
        {
            get;
            set;
        }
        /// <summary>
        /// 打印模版
        /// </summary>
        public string TemplateFile
        {
            get;
            set;
        }
        /// <summary>
        /// 部门公章
        /// </summary>
        public string DepartStamp
        {
            get;
            set;
        }
        #endregion
    }
    #endregion
}
