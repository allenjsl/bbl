/************************************************************
 * 模块名称：支出销帐数据接口
 * 功能说明：支出销帐数据接口
 * 创建人：周文超  2011-4-28 15:50:27
 * *********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.FinanceStructure
{
    /// <summary>
    /// 支出登帐数据接口
    /// </summary>
    public interface ISpendRegister
    {
        /// <summary>
        /// 新增支出登帐信息
        /// </summary>
        /// <param name="model">支出登帐信息实体</param>
        /// <returns>返回1成功，其他失败</returns>
        int AddSpendRegister(Model.FinanceStructure.MSpendRegister model);

        /// <summary>
        /// 获取支出登帐信息
        /// </summary>
        /// <param name="model">支出登帐信息查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns></returns>
        IList<EyouSoft.Model.FinanceStructure.MSpendRegister> GetList(EyouSoft.Model.FinanceStructure.MQuerySpendRegister model, string HaveUserIds, int PageSize, int PageIndex, ref int RecordCount);

        /// <summary>
        /// 支出销帐
        /// </summary>
        /// <param name="RegistId">支出登帐Id</param>
        /// <param name="OperatorId">当前操作员Id</param>
        /// <param name="OperatorName">当前操作员名称</param>
        /// <param name="list">支出销帐实体集合</param>
        /// <returns>返回1成功，其他失败</returns>
        int SpendRegisterDetail(int RegistId, int OperatorId, string OperatorName
            , IList<EyouSoft.Model.FinanceStructure.MSpendRegisterDetail> list);

        /// <summary>
        /// 批量登记付款
        /// </summary>
        /// <param name="info">付款批量登记信息业务实体</param>
        /// <returns></returns>
        int BatchRegisterExpense(EyouSoft.Model.FinanceStructure.MBatchRegisterExpenseInfo info);
        /*/// <summary>
        /// 批量审批付款
        /// </summary>
        /// <param name="operatorId">操作人编号</param>
        /// <param name="registerIds">付款登记编号集合</param>
        /// <returns></returns>
        int BatchApprovalExpense(int operatorId, IList<string> registerIds);
        /// <summary>
        /// 批量支付付款
        /// </summary>
        /// <param name="operatorId">操作人编号</param>
        /// <param name="registerIds">付款登记编号集合</param>
        /// <returns></returns>
        int BatchPayExpense(int operatorId, IList<string> registerIds);*/
    }
}
