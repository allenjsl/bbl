using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-合同管理信息实体
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public class ContractInfo
    {
        #region Model
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { set; get; }
        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId { set; get; }
        /// <summary>
        /// 员工号
        /// </summary>
        public string StaffNo { set; get; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string StaffName { set; get; }
        /// <summary>
        /// 签订时间
        /// </summary>
        public DateTime BeginDate { set; get; }
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime EndDate { set; get; }
        /// <summary>
        /// 状态（未到期，到期未处理，到期已处理）
        /// </summary>
        public EyouSoft.Model.EnumType.AdminCenterStructure.ContractStatus ContractStatus { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// 操作人编号
        /// </summary>
        public int OperatorId { set; get; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime IssueTime { set; get; }
        #endregion Model

    }
    /// <summary>
    /// 合同管理查询实体
    /// </summary>
    public class ContractSearchInfo
    {
        /// <summary>
        /// 员工号
        /// </summary>
        public string StaffNo { set; get; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string StaffName { set; get; }
        /// <summary>
        /// 合同起始时间开始
        /// </summary>
        public DateTime? BeginFrom { set; get; }
        /// <summary>
        /// 合同起始时间结束
        /// </summary>
        public DateTime? BeginTo{ set; get; }
        /// <summary>
        /// 合同结束时间开始
        /// </summary>
        public DateTime? EndFrom { set; get; }
        /// <summary>
        /// 合同结束时间结束
        /// </summary>
        public DateTime? EndTo { set; get; }
    }
}
