using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.UtilityStructure
{
    /// <summary>
    /// 统一维护的方法
    /// Create:luofx  Date:2011-01-23
    /// </summary>
    public interface IUtility
    {
        /// <summary>
        /// 修改线路库线路团队收客数，上团数
        /// </summary>
        /// <param name="RouteId">线路编号</param>
        /// <param name="type">团队上团数和收客数修改类型</param>
        /// <returns></returns>
        bool UpdateRouteSomething(int[] RouteId, EyouSoft.Model.EnumType.TourStructure.UpdateTourType type);
        ///// <summary>
        ///// 维护团队订单数据
        ///// </summary>
        ///// <param name="ItemId">项目编号(订单编号，团队编号)</param>
        ///// <param name="type">类型</param>
        ///// <returns></returns>
        //bool UpdateOrderAndTourSomething(string ItemId, EyouSoft.Model.EnumType.TourStructure.ItemType type);
        /// <summary>
        /// 1:增加（审核）收款之后更新订单已收未审核金额和已收已审核金额
        /// 2:更新统计分析[所有收入明细表([tbl_StatAllIncome])]已收未审核金额和已收已审核金额
        ///	3:团队的收入是否已结清
        /// </summary>
        /// <param name="IsReceive">是否收款（true=收款，false=退款）</param>
        /// <param name="OrderIds">订单编号</param>
        /// <returns></returns>
        bool UpdateCheckMoney(bool IsReceive,params string[] OrderIds);
        /// <summary>
        /// 1：修改团队收入
        /// 2：团款收入时才插入或更新统计分析[所有收入明细表([tbl_StatAllIncome])]数据
        /// </summary>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        bool UpdateTotalIncome(string OrderId);

        /// <summary>
        /// 重新计算团队收入相关字段，所有模块收入相关项、
        /// </summary>
        /// <param name="TourId">团队Id</param>
        /// <param name="list">收入项目编号、类型实体</param>
        /// <returns>返回1成功，其他失败</returns>
        int CalculationTourIncome(string TourId, IList<Model.StatisticStructure.IncomeItemIdAndType> list);

        /// <summary>
        /// 重新计算团队支出相关项、所有支出相关项、
        /// </summary>
        /// <param name="TourId">团队Id，传值string.Empty不重新计算团队的各种支出</param>
        /// <param name="list">支出项目编号、类型实体集合</param>
        /// <returns>返回1成功，其他失败</returns>
        int CalculationTourOut(string TourId, IList<Model.StatisticStructure.ItemIdAndType> list);

        /// <summary>
        /// 重新计算所有模块的已确认支付金额
        /// </summary>
        /// <param name="FinancialPayIds">支出登记明细Id集合</param>
        /// <returns>返回1成功，其他失败</returns>
        int CalculationCheckedOut(params string[] FinancialPayIds);

        /// <summary>
        /// 重新计算团队的利润分配
        /// </summary>
        /// <param name="TourId">团队Id</param>
        /// <returns>返回1成功，其他失败</returns>
        int CalculationTourProfitShare(string TourId);

        /// <summary>
        /// 重新计算现金流量某一天的收入、支出
        /// </summary>
        /// <param name="CompanyId">要计算的公司Id</param>
        /// <param name="CurrDate">要计算哪一天</param>
        /// <returns>返回1成功，其他失败</returns>
        int CalculationCashFlow(int CompanyId, DateTime CurrDate);

        /// <summary>
        /// 维护组团公司与专线公司的交易次数
        /// </summary>
        /// <param name="BuyCompanyId">组团公司编号</param>
        /// <returns></returns>
        bool UpdateTradeNum(int BuyCompanyId);

        /// <summary>
        /// 计算供应商交易数量
        /// </summary>
        /// <param name="ServerId">供应商编号</param>
        /// <returns></returns>
        bool ServerTradeCount(int ServerId);

        /// <summary>
        /// 维护计划款项结清状态，返回1成功，其它失败
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        int CalculationTourSettleStatus(string tourId);
    }
}
