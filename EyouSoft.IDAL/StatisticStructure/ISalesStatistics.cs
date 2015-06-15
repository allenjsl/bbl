/************************************************************
 * 模块名称：客户关系管理-销售统计数据接口
 * 功能说明：客户关系管理-销售统计数据接口
 * 创建人：周文超  2011-4-18 16:32:14
 * *********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.StatisticStructure
{
    /// <summary>
    /// 客户关系管理-销售统计数据接口
    /// </summary>
    public interface ISalesStatistics
    {
        /// <summary>
        /// 获取销售统计列表
        /// </summary>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="QueryModel">查询实体</param>
        /// <param name="QueryModel">查询实体</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        IList<Model.StatisticStructure.MSalesStatistics> GetSalesStatistics(int PageSize, int PageIndex
            , ref int RecordCount, Model.StatisticStructure.MQuerySalesStatistics QueryModel, string us);

        /// <summary>
        /// 获取销售统计的汇总信息
        /// </summary>
        /// <param name="AllPeopleNum">总人数</param>
        /// <param name="AllFinanceSum">总金额</param>
        /// <param name="QueryModel">查询实体</param>
        /// <param name="us">用户信息集合</param>
        void GetSumSalesStatistics(ref int AllPeopleNum, ref decimal AllFinanceSum
            , Model.StatisticStructure.MQuerySalesStatistics QueryModel,string us);
    }
}
