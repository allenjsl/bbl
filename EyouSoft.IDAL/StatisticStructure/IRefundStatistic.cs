/************************************************************
 * 模块名称：退票统计数据访问接口
 * 功能说明：
 * 创建人：周文超  2011-4-22 9:56:07
 * *********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.StatisticStructure
{
    /// <summary>
    /// 退票统计数据访问接口
    /// </summary>
    public interface IRefundStatistic
    {
        /// <summary>
        /// 获取退票统计
        /// </summary>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="model">查询实体</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        IList<Model.StatisticStructure.MRefundStatistic> GetRefundStatistic(int PageSize, int PageIndex
            , ref int RecordCount, Model.StatisticStructure.MQueryRefundStatistic model,string us);

        /// <summary>
        /// 获取退票统计合计信息
        /// </summary>
        /// <param name="refundAmount">退回金额合计</param>
        /// <param name="model">查询实体</param>
        /// <param name="us">用户信息集合</param>
        void GetSumRefundStatistic(ref decimal refundAmount, Model.StatisticStructure.MQueryRefundStatistic model,
                                   string us);
    }
}
