using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.StatisticStructure
{
    /// <summary>
    /// 出票统计接口
    /// </summary>
    /// 周文超 2011-03-18
    public interface ITicketOutStatistic
    {
        /// <summary>
        /// 获取出票--售票处统计
        /// </summary>
        /// <param name="model">出票统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        IList<Model.StatisticStructure.TicketOutStatisticOffice> GetTicketOutStatisticOffice(Model.StatisticStructure.QueryTicketOutStatisti model, string HaveUserIds);

        /// <summary>
        /// 获取出票--航空公司统计
        /// </summary>
        /// <param name="model">出票统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        IList<Model.StatisticStructure.TicketOutStatisticAirLine> GetTicketOutStatisticAirLine(Model.StatisticStructure.QueryTicketOutStatisti model, string HaveUserIds);

        /// <summary>
        /// 获取出票--部门统计
        /// </summary>
        /// <param name="model">出票统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        IList<Model.StatisticStructure.TicketOutStatisticDepart> GetTicketOutStatisticDepart(Model.StatisticStructure.QueryTicketOutStatisti model, string HaveUserIds);

        /// <summary>
        /// 获取出票--日期统计
        /// </summary>
        /// <param name="model">出票统计查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        IList<Model.StatisticStructure.TicketOutStatisticTime> GetTicketOutStatisticTime(Model.StatisticStructure.QueryTicketOutStatisti model, string HaveUserIds);
    }
}
