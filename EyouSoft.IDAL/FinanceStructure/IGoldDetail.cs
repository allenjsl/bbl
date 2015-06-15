using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.FinanceStructure
{
    /// <summary>
    /// 收入、支出增加减少费用明细信息数据访问接口
    /// </summary>
    /// Author:汪奇志 2011-04-29
    public interface IGoldDetail
    {
        /// <summary>
        /// 设置收入、支出增加减少费用明细信息集合，1成功 其它失败
        /// </summary>
        /// <param name="details">收入、支出增加减少费用明细信息集合</param>
        /// <returns></returns>
        int SetDetais(IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> details);
        /// <summary>
        /// 获取收入、支出增加减少费用明细信息集合
        /// </summary>
        /// <param name="itemType">项目类型</param>
        /// <param name="itemId">项目编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> GetDetails(EyouSoft.Model.EnumType.FinanceStructure.GoldType itemType, string itemId);
    }
}
