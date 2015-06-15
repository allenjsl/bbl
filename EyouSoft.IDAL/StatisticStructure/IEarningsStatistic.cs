using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.StatisticStructure
{
    /// <summary>
    /// 利润统计数据接口
    /// </summary>
    /// 周文超 2011-01-24
    public interface IEarningsStatistic
    {
        /// <summary>
        /// 获取利润--区域统计
        /// </summary>
        /// <param name="model">利润统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        IList<Model.StatisticStructure.EarningsAreaStatistic> GetEarningsAreaStatistic(Model.StatisticStructure.QueryEarningsStatistic model, string HaveUserIds);

        /// <summary>
        /// 获取利润--部门统计
        /// </summary>
        /// <param name="model">利润统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        IList<Model.StatisticStructure.EarningsDepartStatistic> GetEarningsDepartStatistic(Model.StatisticStructure.QueryEarningsStatistic model, string HaveUserIds);

        /// <summary>
        /// 获取利润--类型统计
        /// </summary>
        /// <param name="model">利润统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        IList<Model.StatisticStructure.EarningsTypeStatistic> GetEarningsTypeStatistic(Model.StatisticStructure.QueryEarningsStatistic model, string HaveUserIds);

        /// <summary>
        /// 获取利润--时间统计
        /// </summary>
        /// <param name="model">利润统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        IList<Model.StatisticStructure.EarningsTimeStatistic> GetEarningsTimeStatistic(Model.StatisticStructure.QueryEarningsStatistic model, string HaveUserIds);

        /// <summary>
        /// 获取团队利润统计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="us">用户Id集合，半角逗号间隔</param>
        /// <returns></returns>
        IList<EyouSoft.Model.StatisticStructure.MTuanDuiLiRunTongJiInfo> GetTuanDuiLiRunTongJi(int companyId, EyouSoft.Model.StatisticStructure.MTuanDuiLiRunTongJinSearchInfo searchInfo, string us);
    }
}
