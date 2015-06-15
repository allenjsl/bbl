using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.StatisticStructure
{
    /// <summary>
    /// 员工业绩统计数据接口
    /// </summary>
    /// 周文超 2011-01-24
    public interface IPersonnelStatistic
    {
        /// <summary>
        /// 获取员工业绩-收入统计
        /// </summary>
        /// <param name="model">员工业绩统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        IList<Model.StatisticStructure.PersonnelIncomeStatistic> GetPersonnelIncomeStatistic(Model.StatisticStructure.QueryPersonnelStatistic model, string HaveUserIds);

        /// <summary>
        /// 获取员工业绩-利润统计
        /// </summary>
        /// <param name="model">员工业绩统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        IList<Model.StatisticStructure.PersonnelProfitStatistic> GetPersonnelProfitStatistic(Model.StatisticStructure.QueryPersonnelStatistic model, string HaveUserIds);
    }
}
