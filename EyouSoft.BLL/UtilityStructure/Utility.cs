using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.UtilityStructure
{
    /// <summary>
    /// 统一维护的方法
    /// Create:luofx  Date:2011-01-23
    /// </summary>
    public class Utility
    {
        private readonly IDAL.UtilityStructure.IUtility idal = Component.Factory.ComponentFactory.CreateDAL<IDAL.UtilityStructure.IUtility>();

        /// <summary>
        /// 修改线路库线路团队收客数，上团数
        /// </summary>
        /// <param name="RouteId">线路编号</param>
        /// <param name="type">团队上团数和收客数修改类型</param>
        /// <returns></returns>
        public bool UpdateRouteSomething(int[] RouteId, EyouSoft.Model.EnumType.TourStructure.UpdateTourType type)
        {
            if (RouteId == null || RouteId.Length <= 0)
                return false;
            int[] newRouteId = RouteId.Where<int>(p=>p>0).ToArray<int>();
            return idal.UpdateRouteSomething(newRouteId, type);
        }

        /// <summary>
        /// 1:更新团队总收入
        /// 2：团款收入时才插入或更新统计分析[所有收入明细表([tbl_StatAllIncome])]数据
        /// </summary>
        /// <returns></returns>
        public bool UpdateTotalIncome(string OrderId)
        {
            if (string.IsNullOrEmpty(OrderId))
                return false;

            return idal.UpdateTotalIncome(OrderId);
        }

        /// <summary>
        /// 1:增加（审核）收款之后更新订单已收未审核金额和已收已审核金额
        /// 2:更新统计分析[所有收入明细表([tbl_StatAllIncome])]已收未审核金额和已收已审核金额
        ///	3:团队的收入是否已结清
        /// </summary>
        /// <param name="IsReceive">是否收款（true=收款，false=退款）</param>
        /// <param name="OrderIds">订单编号</param>
        /// <returns></returns>
        public bool UpdateCheckMoney(bool IsReceive, params string[] OrderIds)
        {
            if (OrderIds == null || OrderIds.Length == 0)
                return false;

            return idal.UpdateCheckMoney(IsReceive, OrderIds);
        }

        /// <summary>
        /// 重新计算团队收入相关字段，所有模块收入相关项、
        /// </summary>
        /// <param name="TourId">团队Id</param>
        /// <param name="list">收入项目编号、类型实体</param>
        /// <returns>返回1成功，其他失败</returns>
        public int CalculationTourIncome(string TourId, IList<Model.StatisticStructure.IncomeItemIdAndType> list)
        {
            if (string.IsNullOrEmpty(TourId) && (list == null || list.Count <= 0))
                return 0;

            return idal.CalculationTourIncome(TourId, list);
        }

        /// <summary>
        /// 重新计算团队支出相关项、所有支出相关项、
        /// </summary>
        /// <param name="TourId">团队Id，传值string.Empty不重新计算团队的各种支出</param>
        /// <param name="list">支出项目编号、类型实体集合</param>
        /// <returns>返回1成功，其他失败</returns>
        public int CalculationTourOut(string TourId, IList<Model.StatisticStructure.ItemIdAndType> list)
        {
            if (string.IsNullOrEmpty(TourId) && (list == null || list.Count <= 0))
                return 0;

            return idal.CalculationTourOut(TourId, list);
        }



        /// <summary>
        /// 重新计算所有模块的已确认支付金额
        /// </summary>
        /// <param name="FinancialPayIds">支出登记明细Id集合</param>
        /// <returns>返回1成功，其他失败</returns>
        public int CalculationCheckedOut(params string[] FinancialPayIds)
        {
            if (FinancialPayIds == null || FinancialPayIds.Length <= 0)
                return 0;

            return idal.CalculationCheckedOut(FinancialPayIds);
        }

        /// <summary>
        /// 重新计算团队的利润分配
        /// </summary>
        /// <param name="TourId">团队Id</param>
        /// <returns>返回1成功，其他失败</returns>
        public int CalculationTourProfitShare(string TourId)
        {
            if (string.IsNullOrEmpty(TourId))
                return 0;

            return idal.CalculationTourProfitShare(TourId);
        }

        /// <summary>
        /// 重新计算现金流量某一天的收入、支出
        /// </summary>
        /// <param name="CompanyId">要计算的公司Id</param>
        /// <param name="CurrDate">要计算哪一天</param>
        /// <returns>返回1成功，其他失败</returns>
        public int CalculationCashFlow(int CompanyId, DateTime CurrDate)
        {
            if (CompanyId <= 0)
                return 0;

            return idal.CalculationCashFlow(CompanyId, CurrDate);
        }
        /// <summary>
        /// 维护组团公司与专线公司的交易次数
        /// </summary>
        /// <param name="BuyCompanyId">组团公司编号</param>
        /// <returns></returns>
        public void UpdateTradeNum(int[] BuyCompanyId)
        {
            foreach (int item in BuyCompanyId)
            {
                if (item > 0)
                {
                    idal.UpdateTradeNum(item);
                }
            }            
        }

        /// <summary>
        /// 计算供应商交易数量
        /// </summary>
        /// <param name="ServerId">供应商编号数组</param>
        /// <returns></returns>
        public bool ServerTradeCount(int[] ServerIds)
        {
            if (ServerIds == null || ServerIds.Length < 0)
                return false;
            foreach (int i in ServerIds)
                ServerTradeCount(i);
            return true;
        }

        /// <summary>
        /// 计算供应商交易数量
        /// </summary>
        /// <param name="ServerId">供应商编号</param>
        /// <returns></returns>
        public bool ServerTradeCount(int ServerId)
        {
            return idal.ServerTradeCount(ServerId);
        }

        /// <summary>
        /// 维护计划款项结清状态，返回1成功，其它失败
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public int CalculationTourSettleStatus(string tourId)
        {
            if (string.IsNullOrEmpty(tourId)) return 0;

            return idal.CalculationTourSettleStatus(tourId);
        }
    }
}
