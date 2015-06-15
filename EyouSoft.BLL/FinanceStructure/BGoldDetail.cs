using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.FinanceStructure
{
    /// <summary>
    /// 收入、支出增加减少费用明细信息业务逻辑类
    /// </summary>
    /// Author:汪奇志 2011-04-29
    public class BGoldDetail
    {
        private readonly EyouSoft.IDAL.FinanceStructure.IGoldDetail dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.FinanceStructure.IGoldDetail>();

        #region constructure
        /// <summary>
        /// default constructure
        /// </summary>
        public BGoldDetail() { }
        #endregion

        #region private members
        /// <summary>
        /// 获取收入、支出增加减少费用明细信息集合
        /// </summary>
        /// <param name="itemType">项目类型</param>
        /// <param name="itemId">项目编号</param>
        /// <returns></returns>
        private IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> GetDetails(EyouSoft.Model.EnumType.FinanceStructure.GoldType itemType, string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return null;

            return dal.GetDetails(itemType, itemId);
        }
        #endregion

        #region public members
        /// <summary>
        /// 设置收入、支出增加减少费用明细信息集合，1成功 其它失败
        /// </summary>
        /// <param name="details">收入、支出增加减少费用明细信息集合</param>
        /// <returns></returns>
        internal int SetDetais(IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> details)
        {
            if (details == null || details.Count < 1) return 0;

            foreach (var item in details)
            {
                if (string.IsNullOrEmpty(item.DetailId)) item.DetailId = Guid.NewGuid().ToString();
            }

            return dal.SetDetais(details);
        }

        /// <summary>
        /// 获取订单收入增加减少费用明细信息集合
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> GetDetailsByOrderIncome(string orderId)
        {
            return this.GetDetails(EyouSoft.Model.EnumType.FinanceStructure.GoldType.订单收入, orderId);
        }

        /// <summary>
        /// 获取杂费收入增加减少费用明细信息集合
        /// </summary>
        /// <param name="otheerIncomeId">杂费收入编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> GetDetailsByOtherIncome(string otheerIncomeId)
        {
            return this.GetDetails(EyouSoft.Model.EnumType.FinanceStructure.GoldType.杂费收入, otheerIncomeId);
        }

        /// <summary>
        /// 获取地接计调安排支出增加减少费用明细信息集合
        /// </summary>
        /// <param name="localAgencyPlanId">地接计调安排编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> GetDetailsByLocalAgencyOut(string localAgencyPlanId)
        {
            return this.GetDetails(EyouSoft.Model.EnumType.FinanceStructure.GoldType.地接支出, localAgencyPlanId);
        }

        /// <summary>
        /// 获取机票计调安排支出增加减少费用明细信息集合
        /// </summary>
        /// <param name="ticketPlanId">机票计调安排编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> GetDetailsByTicketOut(string ticketPlanId)
        {
            return this.GetDetails(EyouSoft.Model.EnumType.FinanceStructure.GoldType.机票支出, ticketPlanId);
        }

        /// <summary>
        /// 获取杂费支出增加减少费用明细信息集合
        /// </summary>
        /// <param name="otherOutId">杂费支出编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> GetDetailsByOtherOut(string otherOutId)
        {
            return this.GetDetails(EyouSoft.Model.EnumType.FinanceStructure.GoldType.杂费支出, otherOutId);
        }

        /// <summary>
        /// 获取单项服务供应商安排增加减少费用明细信息集合
        /// </summary>
        /// <param name="singlePlanId">单项服务供应商安排编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.MGoldDetailInfo> GetDetailsBySingleOut(string singlePlanId)
        {
            return this.GetDetails(EyouSoft.Model.EnumType.FinanceStructure.GoldType.单项供应商安排, singlePlanId);
        }
        #endregion
    }
}
