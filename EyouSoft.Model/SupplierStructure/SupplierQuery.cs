using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SupplierStructure
{
    /// <summary>
    /// 供应商查询实体
    /// </summary>
    /// 鲁功源 2011-03-08
    public class SupplierQuery
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SupplierQuery() { }

        #region 属性
        /// <summary>
        /// 省份编号
        /// </summary>
        public int ProvinceId
        {
            get;
            set;
        }
        /// <summary>
        /// 省份名称
        /// </summary>
        public string ProvinceName
        {
            get;
            set;
        }
        /// <summary>
        /// 城市编号
        /// </summary>
        public int CityId
        {
            get;
            set;
        }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName
        {
            get;
            set;
        }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName
        {
            get;
            set;
        }

        /// <summary>
        /// 星级
        /// </summary>
        public EyouSoft.Model.EnumType.SupplierStructure.ScenicSpotStar? Start
        {
            get;
            set;
        } 

        #endregion

    }
}
