using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SupplierStructure
{
    /// <summary>
    /// 供应商车队实体
    /// </summary>
    /// mk 2011-03-08
    public class SupplierCarTeam : EyouSoft.Model.CompanyStructure.SupplierBasic
    {
        public IList<SupplierCarInfo> CarsInfo { get; set; }
    }

    /// <summary>
    /// 供应商车队车辆信息实体
    /// </summary>
    /// mk 2011-03-08
    public class SupplierCarInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SupplierCarInfo() { }


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
        public int PrivaderId
        {
            get;
            set;
        }

        /// <summary>
        /// 车型
        /// </summary>
        public string CarType
        {
            get;
            set;
        }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNumber
        {
            get;
            set;
        }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string Image
        {
            get;
            set;
        }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price
        {
            get;
            set;
        }

        /// <summary>
        /// 司机名称
        /// </summary>
        public string DriverName
        {
            get;
            set;
        }

        /// <summary>
        /// 司机电话
        /// </summary>
        public string DriverPhone
        {
            get;
            set;
        }

        /// <summary>
        /// 导游词
        /// </summary>
        public string GuideWord
        {
            get;
            set;
        }

        #endregion
    }

    #region 供应商车队查询信息业务实体
    /// <summary>
    /// 供应商车队查询信息业务实体
    /// </summary>
    /// Author:mk 2011-03-08
    public class SupplierCarTeamSearchInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public SupplierCarTeamSearchInfo() { }

        /// <summary>
        /// 省份编号
        /// </summary>
        public int? ProvinceId { get; set; }
        /// <summary>
        /// 城市编号
        /// </summary>
        public int? CityId { get; set; }
        /// <summary>
        /// 车队名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 车型
        /// </summary>
        public string CarType { get; set; }
    }
    #endregion
}
