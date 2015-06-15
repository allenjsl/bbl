using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.StatisticStructure
{
    /// <summary>
    /// 所有收入明细数据层接口
    /// </summary>
    /// 鲁功源 2011-01-23
    public interface IStatAllIncome
    {
        /// <summary>
        /// 分页获取收入明细列表
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="queryInfo">查询实体</param>
        /// <param name="haveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        IList<EyouSoft.Model.StatisticStructure.StatAllIncomeList> GetList(int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic queryInfo, string haveUserIds);
        /// <summary>
        /// 添加所有收入明细
        /// </summary>
        /// <param name="model">所有收入明细实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(EyouSoft.Model.StatisticStructure.StatAllIncome model);
        /// <summary>
        /// 批量添加所有收入明细
        /// </summary>
        /// <param name="list">收入明细列表</param>
        /// <returns></returns>
        bool Add(IList<EyouSoft.Model.StatisticStructure.StatAllIncome> list);
        /// <summary>
        /// 删除收入明细
        /// </summary>
        /// <param name="list">收入项目编号、类型集合</param>
        /// <returns></returns>
        bool Delete(IList<EyouSoft.Model.StatisticStructure.IncomeItemIdAndType> list);

        /// <summary>
        /// 获取收入对帐单的汇总信息
        /// </summary>
        /// <param name="queryInfo">查询实体</param>
        /// <param name="TotalAmount">总收入</param>
        /// <param name="HasCheckAmount">已收</param>
        /// <param name="haveUserIds">用户Id集合，半角逗号分割</param>
        void GetAllTotalAmount(EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic queryInfo
            , ref decimal TotalAmount, ref decimal HasCheckAmount, string haveUserIds);
    }
}
