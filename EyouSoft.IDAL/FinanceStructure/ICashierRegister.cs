using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.FinanceStructure
{
    /// <summary>
    /// 出纳登帐数据接口
    /// </summary>
    /// 周文超 2011-01-21
    public interface ICashierRegister
    {
        /// <summary>
        /// 新增出纳登帐信息
        /// </summary>
        /// <param name="model">出纳登帐信息实体</param>
        /// <returns>返回1成功，其他失败</returns>
        int AddCashierRegister(Model.FinanceStructure.CashierRegisterInfo model);

        /// <summary>
        /// 获取出纳登帐信息
        /// </summary>
        /// <param name="model">出纳登帐信息查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns></returns>
        IList<EyouSoft.Model.FinanceStructure.CashierRegisterInfo> GetList(EyouSoft.Model.FinanceStructure.QueryCashierRegisterInfo model, string HaveUserIds, int PageSize, int PageIndex, ref int RecordCount);

        /// <summary>
        /// 获取出纳登帐信息金额合计
        /// </summary>
        /// <param name="model">出纳登帐信息查询实体</param>
        /// <param name="haveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="paymentCount">到款银行</param>
        /// <param name="totalAmount">已销账金额</param>
        void GetCashierRegisterMoney(Model.FinanceStructure.QueryCashierRegisterInfo model, string haveUserIds
            , ref decimal paymentCount, ref decimal totalAmount);

        /// <summary>
        /// 销帐
        /// </summary>
        /// <param name="RegistId">出纳登帐Id</param>
        /// <param name="OperatorId">当前操作员Id</param>
        /// <param name="OperatorName">当前操作员名称</param>
        /// <param name="list">销帐实体集合</param>
        /// <returns>返回1成功，其他失败</returns>
        int CancelRegist(int RegistId, int OperatorId, string OperatorName
            , IList<EyouSoft.Model.FinanceStructure.CancelRegistInfo> list);
    }
}
