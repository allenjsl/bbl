using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.CompanyStructure
{
    /// <summary>
    /// 供应商联系人
    /// 创建人：xuqh 2011-01-19
    /// </summary>
    public class SupplierContact
    {
        #region Model
        /// <summary>
        /// 联系人编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 公司编号
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        public int SupplierId { get; set; }

        /// <summary>
        /// 供应商类型
        /// </summary>
        public EyouSoft.Model.EnumType.CompanyStructure.SupplierType SupplierType { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string ContactFax { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactTel { get; set; }

        /// <summary>
        /// 联系人手机
        /// </summary>
        public string ContactMobile { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 地接用户帐户信息(组团公司编号为地接供应商编号)
        /// </summary>
        public EyouSoft.Model.CompanyStructure.UserAccount UserAccount { get; set; }

        #endregion
    }
}
