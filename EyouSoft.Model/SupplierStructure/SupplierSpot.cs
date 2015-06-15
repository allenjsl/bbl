using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.SupplierStructure
{
    /// <summary>
    /// 供应商景点[景点]
    /// </summary>
    /// lugy 2011-03-08
    public class SupplierSpot : EyouSoft.Model.CompanyStructure.SupplierBasic
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SupplierSpot() { }

        #region 属性
        /// <summary>
        /// 合作协议
        /// </summary>
        public string AgreementFile 
        { 
            get; 
            set; 
        }
        /// <summary>
        /// 政策
        /// </summary>
        public string UnitPolicy 
        {
            get; 
            set; 
        }
        /// <summary>
        /// 星级
        /// </summary>
        public EyouSoft.Model.EnumType.SupplierStructure.ScenicSpotStar Start
        {
            get;
            set;
        }
        /// <summary>
        /// 导游词
        /// </summary>
        public string TourGuide
        {
            get;
            set;
        }
        /// <summary>
        /// 团队价
        /// </summary>
        public decimal TeamPrice
        {
            get;
            set;
        }
        /// <summary>
        /// 散客价
        /// </summary>
        public decimal TravelerPrice
        {
            get;
            set;
        }
        #endregion

    }
}
