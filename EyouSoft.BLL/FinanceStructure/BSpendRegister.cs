/************************************************************
 * 模块名称：支出销帐业务逻辑
 * 功能说明：支出销帐业务逻辑
 * 创建人：周文超  2011-4-29 11:13:13
 * *********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.FinanceStructure
{
    /// <summary>
    /// 支出销帐业务逻辑
    /// </summary>
    public class BSpendRegister : EyouSoft.BLL.BLLBase
    {
        private readonly IDAL.FinanceStructure.ISpendRegister dal = Component.Factory.ComponentFactory.CreateDAL<IDAL.FinanceStructure.ISpendRegister>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public BSpendRegister() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public BSpendRegister(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }

        /// <summary>
        /// 新增支出登帐信息
        /// </summary>
        /// <param name="model">支出登帐信息实体</param>
        /// <returns>返回1成功，其他失败</returns>
        public int AddSpendRegister(EyouSoft.Model.FinanceStructure.MSpendRegister model)
        {
            if (model == null)
                return 0;

            return dal.AddSpendRegister(model);
        }

        /// <summary>
        /// 获取支出登帐信息
        /// </summary>
        /// <param name="model">支出登帐信息查询实体</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.MSpendRegister> GetList(EyouSoft.Model.FinanceStructure.MQuerySpendRegister model, int PageSize, int PageIndex, ref int RecordCount)
        {
            if (model == null || model.CompanyId <= 0)
                return null;

            return dal.GetList(model, base.HaveUserIds, PageSize, PageIndex, ref RecordCount);
        }

        /// <summary>
        /// 支出销帐
        /// </summary>
        /// <param name="RegistId">支出登帐Id</param>
        /// <param name="OperatorId">当前操作员Id</param>
        /// <param name="OperatorName">当前操作员名称</param>
        /// <param name="list">支出销帐实体集合</param>
        /// <returns>返回1成功，其他失败</returns>
        public int SpendRegisterDetail(int RegistId, int OperatorId, string OperatorName, IList<EyouSoft.Model.FinanceStructure.MSpendRegisterDetail> list)
        {
            if (RegistId <= 0 || OperatorId <= 0 || list == null || list.Count <= 0)
                return 0;

            return dal.SpendRegisterDetail(RegistId, OperatorId, OperatorName, list);
        }

        /// <summary>
        /// 批量登记付款
        /// </summary>
        /// <param name="info">付款批量登记信息业务实体</param>
        /// <returns></returns>
        public int BatchRegisterExpense(EyouSoft.Model.FinanceStructure.MBatchRegisterExpenseInfo info)
        {
            if (info == null
                || info.OperatorId <= 0
                || info.CompanyId <= 0
                || info.PaymentTime == DateTime.MinValue
                || info.TourIds == null
                || info.TourIds.Count < 1)
            {
                throw new System.Exception("请传递正确的参数。");
            }

            int dalRetCode = dal.BatchRegisterExpense(info);

            #region LGWR
            if (dalRetCode == 1)
            {
                StringBuilder s=new StringBuilder();
                s.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款支出.ToString() + "做批量付款登记");

                s.Append("，登记的团队编号为:");
                foreach (var tourId in info.TourIds)
                {
                    s.Append(tourId + "、");
                }

                s.Append("，供应商查询条件为：" + info.SearchGYSName+"。");

                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = s.ToString();
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "批量付款登记";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款支出;
                logInfo.OperatorId = 0;

                new EyouSoft.BLL.CompanyStructure.SysHandleLogs().Add(logInfo);
            }
            #endregion

            return dalRetCode;
        }

        /// <summary>
        /// 批量审批付款
        /// </summary>
        /// <param name="operatorId">操作人编号</param>
        /// <param name="registerIds">付款登记编号集合</param>
        /// <returns></returns>
        public int BatchApprovalExpense(int operatorId, IList<string> registerIds)
        {
            if (operatorId <= 0 || registerIds == null || registerIds.Count < 1)
            {
                throw new System.Exception("请传递正确的参数。");
            }

            string[] ids = new string[registerIds.Count];

            for (var i = 0; i < registerIds.Count;i++ )
            {
                ids[i] = registerIds[i];
            }

            return new EyouSoft.BLL.FinanceStructure.OutRegister(null).SetCheckedState(true, operatorId, ids);
        }

        /// <summary>
        /// 批量支付付款
        /// </summary>
        /// <param name="operatorId">操作人编号</param>
        /// <param name="registerIds">付款登记编号集合</param>
        /// <returns></returns>
        public int BatchPayExpense(int operatorId, IList<string> registerIds)
        {
            if (operatorId <= 0 || registerIds == null || registerIds.Count < 1)
            {
                throw new System.Exception("请传递正确的参数。");
            }

            string[] ids = new string[registerIds.Count];

            for (var i = 0; i < registerIds.Count; i++)
            {
                ids[i] = registerIds[i];
            }

            return new EyouSoft.BLL.FinanceStructure.OutRegister(null).SetIsPay(true, ids);
        }
    }
}
