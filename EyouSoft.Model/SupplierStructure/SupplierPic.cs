using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SupplierStructure
{
    /// <summary>
    /// 供应商图片信息实体
    /// </summary>
    /// lugy 2011-03-08
    public class SupplierPic
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SupplierPic() { }

        #region 属性
        /// <summary>
        /// 主键编号
        /// </summary>
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// 供应商编号
        /// </summary>
        public int SupplierId
        {
            get;
            set;
        }
        /// <summary>
        /// 图片名称
        /// </summary>
        public string PicName
        {
            get;
            set;
        }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string PicPath
        {
            get;
            set;
        }
        #endregion
    }
}
