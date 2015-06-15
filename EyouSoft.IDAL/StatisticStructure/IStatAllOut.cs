using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.StatisticStructure
{
    /// <summary>
    /// 所有支出明细数据层接口
    /// </summary>
    /// 鲁功源 2011-01-23
    public interface IStatAllOut
    {
        /// <summary>
        /// 分页获取支出明细列表
        /// </summary>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="queryInfo">查询实体</param>
        /// <param name="haveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        IList<EyouSoft.Model.StatisticStructure.StatAllOutList> GetList(int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic queryInfo, string haveUserIds);
        /// <summary>
        /// 添加所有支出明细
        /// </summary>
        /// <param name="model">所有支出明细实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(EyouSoft.Model.StatisticStructure.StatAllOut model);
        /// <summary>
        /// 批量添加所有支出明细
        /// </summary>
        /// <param name="list">支出明细列表</param>
        /// <returns></returns>
        bool Add(IList<EyouSoft.Model.StatisticStructure.StatAllOut> list);
        /// <summary>
        /// 删除支出明细
        /// </summary>
        /// <param name="list">支出项目编号、类型集合</param>
        /// <returns></returns>
        bool Delete(IList<EyouSoft.Model.StatisticStructure.ItemIdAndType> list);

        /// <summary>
        /// 获取支出对帐单的汇总信息
        /// </summary>
        /// <param name="queryInfo">查询实体</param>
        /// <param name="totalAmount">总支出</param>
        /// <param name="hasCheckAmount">已付</param>
        /// <param name="haveUserIds">用户Id集合，半角逗号分割</param>
        void GetAllTotalAmount(EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic queryInfo
            , ref decimal totalAmount, ref decimal hasCheckAmount, string haveUserIds);
    }
}
