using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.PlanStruture
{
    /// <summary>
    /// 单项服务计调安排数据访问类接口
    /// </summary>
    /// Author:汪奇志 2010-02-17
    public interface IPlanSingle
    {
        /*/// <summary>
        /// 获取单项服务支出信息
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <param name="totalAmount">支出金额</param>
        /// <param name="paidAmount">未付金额</param>
        void GetSingleExpense(string tourId, out decimal totalAmount, out decimal unpaidAmount);*/
        /// <summary>
        /// 单项服务支出团队核算，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <param name="plans">单项服务支出信息集合</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        int SetSingleAccounting(string tourId, IList<EyouSoft.Model.TourStructure.PlanSingleInfo> plans);
    }
}
