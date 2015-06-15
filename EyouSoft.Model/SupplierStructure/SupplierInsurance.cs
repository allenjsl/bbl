using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SupplierStructure
{
    /// <summary>
    /// 保险供应商信息实体
    /// 创建人：luofx 2011-03-8
    /// </summary>    
    public class SupplierInsurance : EyouSoft.Model.CompanyStructure.SupplierBasic
    {
        /// <summary>
        /// 合作协议
        /// </summary>
        public string AgreementFile { get; set; }
    }   
}
