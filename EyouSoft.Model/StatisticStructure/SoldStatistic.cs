using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.StatisticStructure
{
    #region 主列表
    /// <summary>
    /// 统计主列表
    /// </summary>
    public class AreaDepartStat
    {
        /// <summary>
        /// 区域编号
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 销售编号
        /// </summary>
        public int SaleId { get; set; }
        /// <summary>
        /// 销售员
        /// </summary>
        public string Saler { get; set; }

    }
    #endregion
    #region 部门列表
    /// <summary>
    /// 部门列表
    /// </summary>
    public class AreaStatDepartList
    {
        /// <summary>
        /// 部门编号
        /// </summary>
        public int DepartId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName { get; set; }

    }
    /// <summary>
    /// 部门统计数列表
    /// </summary>
    public class AreaDepartStatInfo 
    {
        /// <summary>
        /// 部门编号
        /// </summary>
        public int DepartId { get; set; }
        /// <summary>
        /// 统计的人数
        /// </summary>
        public int TradeNumber { get; set; }
    }
    #endregion
    #region 查询实体
    public class AreaSoldStatSearch {
        /// <summary>
        /// 销售员
        /// </summary>
        public string SalerIds { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string CityIds { get; set; }
    }
    #endregion
}
