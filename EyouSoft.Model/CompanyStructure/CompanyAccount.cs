using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 专线公司账户信息实体
    /// </summary>
    /// 创建人：luofx 2011-01-17
    public class CompanyAccount
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 账户姓名
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 开户银行行名称
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 帐号
        /// </summary>
        public string BankNo { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }
    }
}
