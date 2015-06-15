using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.StatisticStructure
{
    /// <summary>
    /// 统计分析-现金流量列表数据层接口
    /// </summary>
    /// 鲁功源 2011-01-22
    public interface ICompanyCash
    {
        /// <summary>
        /// 获取统计分析-现金流量列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="queryInfo">现金流量查询实体</param>
        /// <returns>现金流量列表</returns>
        IList<EyouSoft.Model.StatisticStructure.CompanyCash> GetList(int companyId,EyouSoft.Model.StatisticStructure.QueryCompanyCash queryInfo);
    }
}
