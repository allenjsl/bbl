//Author:汪奇志 2010-02-17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.PlanStruture
{
    /// <summary>
    /// 单项服务计调安排业务逻辑类
    /// </summary>
    /// Author:汪奇志 2010-02-17
    public class PlanSingle
    {
        private readonly EyouSoft.IDAL.PlanStruture.IPlanSingle dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.PlanStruture.IPlanSingle>();

        #region constructure
        /// <summary>
        /// default constructor
        /// </summary>
        public PlanSingle() { }
        #endregion

        #region public members
        /*/// <summary>
        /// 获取单项服务支出信息
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <param name="totalAmount">支出金额</param>
        /// <param name="paidAmount">未付金额</param>
        public void GetSingleExpense(string tourId, out decimal totalAmount, out decimal unpaidAmount)
        {
            dal.GetSingleExpense(tourId, out totalAmount, out unpaidAmount);
        }*/

        /// <summary>
        /// 单项服务支出团队核算，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <param name="plans">单项服务支出信息集合</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int SetSingleAccounting(string tourId, IList<EyouSoft.Model.TourStructure.PlanSingleInfo> plans)
        {
            int dalExceptionCode = dal.SetSingleAccounting(tourId, plans);

            #region stat.
            if (dalExceptionCode == 1)
            {
                EyouSoft.BLL.UtilityStructure.Utility utilitybll = new EyouSoft.BLL.UtilityStructure.Utility();
                utilitybll.CalculationTourIncome(tourId, null);
                utilitybll.CalculationTourOut(tourId, null);
                utilitybll.CalculationTourSettleStatus(tourId);
                utilitybll = null;
            }
            #endregion

            return dalExceptionCode;
        }
        #endregion
    }
}
