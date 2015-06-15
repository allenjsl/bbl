using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.FinanceStructure
{
    /// <summary>
    /// 支出明细业务逻辑
    /// </summary>
    /// 周文超 2011-01-21
    public class OutRegister : EyouSoft.BLL.BLLBase
    {
        private readonly IDAL.FinanceStructure.IOutRegister idal = Component.Factory.ComponentFactory.CreateDAL<IDAL.FinanceStructure.IOutRegister>();
        /// <summary>
        /// 统一维护业务逻辑
        /// </summary>
        private readonly BLL.UtilityStructure.Utility UtilityBll = new EyouSoft.BLL.UtilityStructure.Utility();
        /// <summary>
        /// 系统操作日志逻辑
        /// </summary>
        private readonly BLL.CompanyStructure.SysHandleLogs HandleLogsBll = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public OutRegister(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }

        #region 成员

        /// <summary>
        /// 添加一条支出明细
        /// </summary>
        /// <param name="model">支出明细实体</param>
        /// <returns>返回1成功，其他失败</returns>
        public int AddOutRegister(EyouSoft.Model.FinanceStructure.OutRegisterInfo model)
        {
            if (model == null)
                return 0;
            //计调项目总支出
            decimal expenseAmount = 0;
            //计调项目支出已登记金额 registerAmount 已登记金额不含自身 
            decimal registerAmount = this.GetExpenseRegisterAmount(model.RegisterId, model.ReceiveId, model.ReceiveType, out expenseAmount);
            //登记金额验证：计调项目支出所有登记金额不能大于计调项目支出金额
            if (expenseAmount < registerAmount + model.PaymentAmount) return -1;

            int Result = idal.AddOutRegister(model);
            if (Result == 1)
            {
                HandleLogsBll.Add(
                    new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                    {
                        ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款支出,
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款支出.ToString() + "新增了付款数据！编号为：" + model.RegisterId,
                        EventTitle = "新增" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款支出.ToString() + "数据"
                    });
            }

            return Result;
        }

        /// <summary>
        /// 修改支出明细
        /// </summary>
        /// <param name="model">支出明细实体</param>
        /// <returns>返回1成功，其他失败</returns>
        public int UpdateOutRegister(EyouSoft.Model.FinanceStructure.OutRegisterInfo model)
        {
            if (model == null)
                return 0;

            //计调项目总支出
            decimal expenseAmount = 0;
            //计调项目支出已登记金额
            decimal registerAmount = this.GetExpenseRegisterAmount(model.RegisterId, model.ReceiveId, model.ReceiveType, out expenseAmount);
            //登记金额验证：计调项目支出所有登记金额不能大于计调项目支出金额
            if (expenseAmount < registerAmount + model.PaymentAmount) return -1;

            int Result = idal.UpdateOutRegister(model);
            if (Result == 1)
            {
                HandleLogsBll.Add(
                    new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                    {
                        ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款支出,
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款支出.ToString() + "修改了付款数据！编号为：" + model.RegisterId,
                        EventTitle = "修改" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款支出.ToString() + "数据"
                    });
            }

            return Result;
        }

        /// <summary>
        /// 将某支出明细设置为已审批状态
        /// </summary>
        /// <param name="IsChecked">是否审批</param>
        /// <param name="CheckerId">审批人Id</param>
        /// <param name="OutRegisterIds">支出明细Id集合</param>
        /// <returns>返回1成功，其他失败</returns>
        public int SetCheckedState(bool IsChecked, int CheckerId, params string[] OutRegisterIds)
        {
            if (OutRegisterIds == null || OutRegisterIds.Length <= 0 || CheckerId <= 0)
                return 0;

            int Result = idal.SetCheckedState(IsChecked, CheckerId, OutRegisterIds);
            if (Result == 1)
            {
                HandleLogsBll.Add(
                    new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                    {
                        ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款支出,
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款支出.ToString() + "把审批状态修改为" + (IsChecked ? "已审批" : "未审批") + "！编号为：" + this.GetStrIdsByArrIds(OutRegisterIds),
                        EventTitle = "修改" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款支出.ToString() + "数据"
                    });
            }

            return Result;
        }

        /// <summary>
        /// 将某支出明细设置为已支付
        /// </summary>
        /// <param name="IsPay">是否支付</param>
        /// <param name="OutRegisterIds">支出明细Id集合</param>
        /// <returns>返回1成功，其他失败</returns>
        public int SetIsPay(bool IsPay, params string[] OutRegisterIds)
        {
            if (OutRegisterIds == null || OutRegisterIds.Length <= 0)
                return 0;

            int Result = idal.SetIsPay(IsPay, OutRegisterIds);
            if (Result == 1)
            {
                UtilityBll.CalculationCheckedOut(OutRegisterIds);
                UtilityBll.CalculationCashFlow(base.CompanyId, DateTime.Now);

                HandleLogsBll.Add(
                    new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                    {
                        ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款支出,
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款支出.ToString() + "把支付状态修改为" + (IsPay ? "已支付" : "未支付") + "！编号为：" + this.GetStrIdsByArrIds(OutRegisterIds),
                        EventTitle = "修改" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款支出.ToString() + "数据"
                    });
            }

            return Result;
        }

        /// <summary>
        /// 获取支出登记明细
        /// </summary>
        /// <param name="model">支出登记查询实体</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.OutRegisterInfo> GetOutRegisterList(EyouSoft.Model.FinanceStructure.QueryOutRegisterInfo model, int PageSize, int PageIndex, ref int RecordCount)
        {
            return idal.GetOutRegisterList(model, base.HaveUserIds, PageSize, PageIndex, ref RecordCount);
        }

        /// <summary>
        /// 获取支出登记明细
        /// </summary>
        /// <param name="model">支出登记查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.OutRegisterInfo> GetOutRegisterList(EyouSoft.Model.FinanceStructure.QueryOutRegisterInfo model)
        {
            return idal.GetOutRegisterList(model);
        }

        /// <summary>
        /// 获取计调项目支出已登记金额
        /// </summary>
        /// <param name="registerId">登记编号 不为null时计调项目支出已登记金额不含自身金额 为null时计调项目支出已登记金额</param>
        /// <param name="planId">计调项目编号</param>
        /// <param name="planType">计调项目类型</param>
        /// <param name="expenseAmount">计调项目支出金额</param>
        /// <returns></returns>
        public decimal GetExpenseRegisterAmount(string registerId, string planId, Model.EnumType.FinanceStructure.OutPlanType planType, out decimal expenseAmount)
        {
            return idal.GetExpenseRegisterAmount(registerId, planId, planType, out expenseAmount);
        }

        /// <summary>
        /// 获取支出登记明细金额合计
        /// </summary>
        /// <param name="model">支出登记查询实体</param>
        /// <param name="paymentAmount">付款金额</param>
        public void GetOutRegisterList(Model.FinanceStructure.QueryOutRegisterInfo model, ref decimal paymentAmount)
        {
            if (model == null)
                return;

            idal.GetOutRegisterList(model, HaveUserIds, ref paymentAmount);
        }

        #endregion

        #region 私有函数

        /// <summary>
        /// 将数组Id集合变为以半角逗号分割的Id集合
        /// </summary>
        /// <param name="OutRegisterIds">数组Id集合</param>
        /// <returns></returns>
        private string GetStrIdsByArrIds(params string[] OutRegisterIds)
        {
            if (OutRegisterIds == null || OutRegisterIds.Length <= 0)
                return string.Empty;

            string strReturn = string.Empty;
            foreach (string str in OutRegisterIds)
            {
                strReturn += str + ",";
            }
            strReturn = strReturn.Trim(',');

            return strReturn;
        }

        #endregion
    }
}
