using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.StatisticStructure
{
    /// <summary>
    /// 统计分析-现金流量明细数据层接口
    /// </summary>
    /// 鲁功源 2011-01-22
    public interface ICompanyCashFlow
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">现金流量明细实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(EyouSoft.Model.StatisticStructure.CompanyCashFlow model);
    }
}
