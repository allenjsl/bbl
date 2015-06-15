/*Author:汪奇志 2011-03-08*/
using System;
using System.Collections.Generic;

namespace EyouSoft.Model.SupplierStructure
{
    #region 供应商酒店信息业务实体
    /// <summary>
    /// 供应商酒店信息业务实体
    /// </summary>
    /// Author:汪奇志 2011-03-08
    public class SupplierHotelInfo : EyouSoft.Model.CompanyStructure.SupplierBasic
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public SupplierHotelInfo() { }

        /// <summary>
        /// 星级
        /// </summary>
        public EyouSoft.Model.EnumType.SupplierStructure.HotelStar Star { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        public string Introduce { get; set; }
        /// <summary>
        /// 导游词
        /// </summary>
        public string TourGuide { get; set; }
        /// <summary>
        /// 房型信息业务实体
        /// </summary>
        public IList<SupplierHotelRoomTypeInfo> RoomTypes { get; set; }
    }
    #endregion

    #region 供应商酒店房型信息业务实体
    /// <summary>
    /// 供应商酒店房型信息业务实体
    /// </summary>
    /// Author:汪奇志 2011-03-08
    public class SupplierHotelRoomTypeInfo
    {
        /// <summary>
        /// 房型编号
        /// </summary>
        public int RoomTypeId { get; set; }
        /// <summary>
        /// 房型名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 前台销售价
        /// </summary>
        public decimal SellingPrice { get; set; }
        /// <summary>
        /// 结算价
        /// </summary>
        public decimal AccountingPrice { get; set; }
        /// <summary>
        /// 是否含早
        /// </summary>
        public bool IsBreakfast { get; set; }
    }
    #endregion

    #region 供应商酒店查询信息业务实体
    /// <summary>
    /// 供应商酒店查询信息业务实体
    /// </summary>
    /// Author:汪奇志 2011-03-08
    public class SupplierHotelSearchInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public SupplierHotelSearchInfo() { }

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
        /// 星级
        /// </summary>
        public EyouSoft.Model.EnumType.SupplierStructure.HotelStar? Star { get; set; }
    }
    #endregion
}
