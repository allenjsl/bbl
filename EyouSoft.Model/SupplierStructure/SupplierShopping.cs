using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SupplierStructure
{
    /// <summary>
    /// 供应商信息表[购物]
    /// 创建人：xuqh 2011-03-07
    /// </summary>
    public class SupplierShopping : EyouSoft.Model.CompanyStructure.SupplierBasic
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SupplierShopping() { }

        #region 属性
        /// <summary>
        /// 销售商品
        /// </summary>
        public string SaleProduct { get; set; }

        /// <summary>
        /// 导游词
        /// </summary>
        public string GuideWord { get; set; }

        /// <summary>
        /// 合作协议
        /// </summary>
        public string AgreementFile { get; set; }
        #endregion

    }
}
