using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SupplierStructure
{
    public class SupplierOther : EyouSoft.Model.CompanyStructure.SupplierBasic
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SupplierOther() { }

        #region 属性
        /// <summary>
        /// 合作协议
        /// </summary>
        public string AgreementFile { get; set; }

        #endregion

    }
}
