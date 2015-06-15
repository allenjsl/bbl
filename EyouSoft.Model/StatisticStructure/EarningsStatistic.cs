
/*   Author:周文超 2011-01-21    */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.Model.StatisticStructure
{
    #region 利润统计基类

    /// <summary>
    /// 利润统计基类
    /// </summary>
    public class EarningsStatisticBase
    {
        /// <summary>
        /// 团队数
        /// </summary>
        public int TourNum { get; set; }

        /// <summary>
        /// 团队人数（所有有效订单的人数和）
        /// </summary>
        public int TourPeopleNum { get; set; }

        /// <summary>
        /// 总收入
        /// </summary>
        public decimal GrossIncome { get; set; }

        /// <summary>
        /// 总支出
        /// </summary>
        public decimal GrossOut { get; set; }

        /// <summary>
        /// 团队毛利
        /// </summary>
        /// 总收入-总支出
        public decimal TourGross
        {
            get;
            set;
            //get
            //{
            //    return this.GrossIncome - this.GrossOut;
            //}
        }

        /// <summary>
        /// 人均毛利
        /// </summary>
        /// 团队毛利/团队人数
        public decimal PeopleGross
        {
            get
            {
                if (this.TourPeopleNum > 0)
                    return this.TourGross / this.TourPeopleNum;
                else
                    return 0;
            }
        }

        /// <summary>
        /// 利润分配
        /// </summary>
        public decimal TourShare { get; set; }

        /// <summary>
        /// 公司利润
        /// </summary>
        /// 团队毛利-利润分配
        public decimal CompanyShare
        {
            get
            {
                return this.TourGross - this.TourShare;
            }
        }

        /// <summary>
        /// 利润率
        /// </summary>
        public string LiRunLv
        {
            get
            {
                decimal _lirunlv = 0;

                if (GrossIncome > 0)
                {
                    _lirunlv = CompanyShare / GrossIncome;
                }

                return string.Format("{0:F2}%", _lirunlv * 100);
            }
        }
    }

    #endregion

    #region 利润--区域统计

    /// <summary>
    /// 利润--区域统计
    /// </summary>
    public class EarningsAreaStatistic : EarningsStatisticBase
    {
        /// <summary>
        /// 线路区域Id
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// 线路区域名称
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 计调员
        /// </summary>
        public IList<StatisticOperator> Logistics { get; set; }
    }

    #endregion

    #region 利润--部门统计

    /// <summary>
    /// 利润--部门统计
    /// </summary>
    public class EarningsDepartStatistic : EarningsStatisticBase
    {
        /// <summary>
        /// 部门Id
        /// </summary>
        public int DepartId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName { get; set; }

        /// <summary>
        /// 销售员
        /// </summary>
        public IList<StatisticOperator> SalesClerk { get; set; }
    }

    #endregion

    #region 利润--类型统计

    /// <summary>
    /// 利润--类型统计
    /// </summary>
    public class EarningsTypeStatistic : EarningsStatisticBase
    {
        /// <summary>
        /// 团队类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType TourType { get; set; }

        /// <summary>
        /// 销售员
        /// </summary>
        public IList<StatisticOperator> SalesClerk { get; set; }
    }

    #endregion

    #region 利润--时间统计

    /// <summary>
    /// 利润--时间统计
    /// </summary>
    public class EarningsTimeStatistic : EarningsStatisticBase
    {
        /// <summary>
        /// 年份
        /// </summary>
        public int CurrYear { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int CurrMonth { get; set; }
    }

    #endregion

    #region 利润统计查询实体

    /// <summary>
    /// 利润统计查询实体
    /// </summary>
    public class QueryEarningsStatistic
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 团队类型
        /// </summary>
        public EyouSoft.Model.EnumType.TourStructure.TourType? TourType { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public int[] DepartIds { get; set; }

        /// <summary>
        /// 销售员
        /// </summary>
        public int[] SaleIds { get; set; }

        /// <summary>
        /// 出团时间起
        /// </summary>
        public DateTime? LeaveDateStart { get; set; }

        /// <summary>
        /// 出团时间止
        /// </summary>
        public DateTime? LeaveDateEnd { get; set; }

        /// <summary>
        /// 核算日期起
        /// </summary>
        public DateTime? CheckDateStart { get; set; }

        /// <summary>
        /// 核算日期止
        /// </summary>
        public DateTime? CheckDateEnd { get; set; }

        /// <summary>
        /// 线路区域Id
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int CurrYear { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int CurrMonth { get; set; }

        /// <summary>
        /// 排序索引   0/1：线路区域升/降序；2/3：部门升/降序；4/5：团队类型升/降序；6/7：当前月升/降序
        /// </summary>
        public int OrderIndex { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public string TourCode { get; set; }
        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 统计订单方式
        /// </summary>
        public Model.EnumType.CompanyStructure.ComputeOrderType ComputeOrderType { get; set; }
    }

    #endregion

    #region 团队利润统计
    /// <summary>
    /// 团队利润统计信息业务实体
    /// </summary>
    public class MTuanDuiLiRunTongJiInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MTuanDuiLiRunTongJiInfo() { }

        /// <summary>
        /// 销售员姓名
        /// </summary>
        public string XiaoShouYuanName { get; set; }
        /// <summary>
        /// 团队数
        /// </summary>
        public int TuanDuiShu { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public int RenShu { get; set; }
        /// <summary>
        /// 收入金额
        /// </summary>
        public decimal ShouRuJinE { get; set; }
        /// <summary>
        /// 支出金额
        /// </summary>
        public decimal ZhiChuJInE { get; set; }
        /// <summary>
        /// 利润分配金额
        /// </summary>
        public decimal LiRunFenPeiJinE { get; set; }
        /// <summary>
        /// 利润
        /// </summary>
        public decimal LiRun { get { return ShouRuJinE - ZhiChuJInE - LiRunFenPeiJinE; } }
        /// <summary>
        /// 人均利润
        /// </summary>
        public decimal RenJunLiRun
        {
            get
            {
                if (RenShu <= 0) return 0;
                return LiRun / RenShu;
            }
        }
    }

    /// <summary>
    /// 团队利润统计查询信息业务实体
    /// </summary>
    public class MTuanDuiLiRunTongJinSearchInfo
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MTuanDuiLiRunTongJinSearchInfo() { }

        /// <summary>
        /// 出团起始时间
        /// </summary>
        public DateTime? CTSTime { get; set; }
        /// <summary>
        /// 出团截止时间
        /// </summary>
        public DateTime? CTETime { get; set; }
        /// <summary>
        /// 部门编号集合
        /// </summary>
        public int[] DeptIds { get; set; }
        /// <summary>
        /// 销售员编号集合
        /// </summary>
        public int[] XiaoShouYuanIds { get; set; }
        /// <summary>
        /// 线路区域
        /// </summary>
        public int? AreaId { get; set; }
        /// <summary>
        /// 客户单位所在省份编号集合
        /// </summary>
        public int[] ProvinceIds { get; set; }
        /// <summary>
        /// 客户单位所在城市集合
        /// </summary>
        public int[] CityIds { get; set; }
    }
    #endregion
}
