using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.StatisticStructure
{
    /// <summary>
    /// 回款率分析数据访问类接口
    /// </summary>
    /// 汪奇志 2012-02-27
    public interface IHuiKuanLv
    {
        /// <summary>
        /// 获取回款率分析信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="tongJiShiJian">统计时间</param>
        /// <param name="chaXun">查询信息</param>
        /// <returns></returns>
        EyouSoft.Model.StatisticStructure.HuiKuanLvInfo GetHuiKuanLv(int companyId, DateTime tongJiShiJian, EyouSoft.Model.StatisticStructure.HuiKuanLvChaXunInfo chaXun);
    }
}
