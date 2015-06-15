using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.FinanceStructure
{
    /// <summary>
    /// 支出明细数据接口
    /// </summary>
    /// 2011-01-23
    public interface IOutRegister
    {
        /// <summary>
        /// 添加一条支出明细
        /// </summary>
        /// <param name="model">支出明细实体</param>
        /// <returns>返回1成功，其他失败</returns>
        int AddOutRegister(EyouSoft.Model.FinanceStructure.OutRegisterInfo model);

        /// <summary>
        /// 修改支出明细
        /// </summary>
        /// <param name="model">支出明细实体</param>
        /// <returns>返回1成功，其他失败</returns>
        int UpdateOutRegister(EyouSoft.Model.FinanceStructure.OutRegisterInfo model);

        /// <summary>
        /// 将某支出明细设置为已审批状态
        /// </summary>
        /// <param name="IsChecked">是否审批</param>
        /// <param name="CheckerId">审批人Id</param>
        /// <param name="OutRegisterIds">支出明细Id集合</param>
        /// <returns>返回1成功，其他失败</returns>
        int SetCheckedState(bool IsChecked, int CheckerId, params string[] OutRegisterIds);

        /// <summary>
        /// 将某支出明细设置为已支付
        /// </summary>
        /// <param name="IsPay">是否支付</param>
        /// <param name="OutRegisterIds">支出明细Id集合</param>
        /// <returns>返回1成功，其他失败</returns>
        int SetIsPay(bool IsPay, params string[] OutRegisterIds);

        /// <summary>
        /// 获取支出登记明细
        /// </summary>
        /// <param name="model">支出登记查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns></returns>
        IList<EyouSoft.Model.FinanceStructure.OutRegisterInfo> GetOutRegisterList(EyouSoft.Model.FinanceStructure.QueryOutRegisterInfo model, string HaveUserIds, int PageSize, int PageIndex, ref int RecordCount);

        /// <summary>
        /// 获取支出登记明细
        /// </summary>
        /// <param name="model">支出登记查询实体</param>
        /// <returns></returns>
        IList<EyouSoft.Model.FinanceStructure.OutRegisterInfo> GetOutRegisterList(EyouSoft.Model.FinanceStructure.QueryOutRegisterInfo model);
        /// <summary>
        /// 获取计调项目支出已登记金额
        /// </summary>
        /// <param name="registerId">登记编号 不为null时计调项目支出已登记金额不含自身金额 为null时计调项目支出已登记金额</param>
        /// <param name="planId">计调项目编号</param>
        /// <param name="planType">计调项目类型</param>
        /// <param name="expenseAmount">计调项目支出金额</param>
        /// <returns></returns>
        decimal GetExpenseRegisterAmount(string registerId, string planId, Model.EnumType.FinanceStructure.OutPlanType planType, out decimal expenseAmount);

        /// <summary>
        /// 获取支出登记明细金额合计
        /// </summary>
        /// <param name="model">支出登记查询实体</param>
        /// <param name="haveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="paymentAmount">付款金额</param>
        void GetOutRegisterList(Model.FinanceStructure.QueryOutRegisterInfo model, string haveUserIds, ref decimal paymentAmount);
    }
}
