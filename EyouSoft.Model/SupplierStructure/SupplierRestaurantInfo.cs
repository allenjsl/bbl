/*Author:汪奇志 2011-03-08*/
using System;
using System.Collections.Generic;

namespace EyouSoft.Model.SupplierStructure
{
    /// <summary>
    /// 供应商餐馆信息业务实体
    /// </summary>
    /// Author:汪奇志 2011-03-08
    public class SupplierRestaurantInfo:EyouSoft.Model.CompanyStructure.CompanySupplier
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public SupplierRestaurantInfo() { }

        /// <summary>
        /// 菜系
        /// </summary>
        public string Cuisine { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string Introduce { get; set; }
        /// <summary>
        /// 导游词
        /// </summary>
        public string TourGuide { get; set; }
    }

    #region 供应商餐馆查询信息业务实体
    /// <summary>
    /// 供应商餐馆查询信息业务实体
    /// </summary>
    /// Author:汪奇志 2011-03-08
    public class SupplierRestaurantSearchInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public SupplierRestaurantSearchInfo() { }

        /// <summary>
        /// 省份编号
        /// </summary>
        public int? ProvinceId { get; set; }
        /// <summary>
        /// 城市编号
        /// </summary>
        public int? CityId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 菜系
        /// </summary>
        public string Cuisine { get; set; }
    }
    #endregion
}
