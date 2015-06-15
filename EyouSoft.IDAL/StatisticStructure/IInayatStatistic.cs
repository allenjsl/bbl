using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.StatisticStructure
{
    /// <summary>
    /// 人次统计数据接口
    /// </summary>
    /// 周文超 2011-01-21
    public interface IInayatStatistic
    {
        /// <summary>
        /// 获取人次-区域统计
        /// </summary>
        /// <param name="model">人次统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        IList<Model.StatisticStructure.InayaAreatStatistic> GetInayaAreatStatistic(
            Model.StatisticStructure.QueryInayatStatistic model, string HaveUserIds);

        /// <summary>
        /// 获取人次-部门统计
        /// </summary>
        /// <param name="model">人次统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        IList<Model.StatisticStructure.InayaDepartStatistic> GetInayaDepartStatistic(
            Model.StatisticStructure.QueryInayatStatistic model, string HaveUserIds);

        /// <summary>
        /// 获取人次-时间统计
        /// </summary>
        /// <param name="model">人次统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        IList<Model.StatisticStructure.InayaTimeStatistic> GetInayaTimeStatistic(
            Model.StatisticStructure.QueryInayatStatistic model, string HaveUserIds);
    }
}
