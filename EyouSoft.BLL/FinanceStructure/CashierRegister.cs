using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.FinanceStructure
{
    /// <summary>
    /// 出纳登帐业务逻辑
    /// </summary>
    /// 周文超 2011-01-21
    public class CashierRegister : EyouSoft.BLL.BLLBase
    {
        private readonly IDAL.FinanceStructure.ICashierRegister idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<IDAL.FinanceStructure.ICashierRegister>();

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
        public CashierRegister(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }

        /// <summary>
        /// 新增出纳登帐信息
        /// </summary>
        /// <param name="model">出纳登帐信息实体</param>
        /// <returns>返回1成功，其他失败</returns>
        public int AddCashierRegister(Model.FinanceStructure.CashierRegisterInfo model)
        {
            if (model == null)
                return 0;

            int Result = idal.AddCashierRegister(model);
            if (Result == 1)
            {
                HandleLogsBll.Add(
                    new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                    {
                        ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_出纳登帐,
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_出纳登帐.ToString() + "新增了出纳登帐数据！编号为：" + model.RegisterId,
                        EventTitle = "新增" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_出纳登帐.ToString() + "数据"
                    });
            }

            return Result;
        }

        /// <summary>
        /// 获取出纳登帐信息
        /// </summary>
        /// <param name="model">出纳登帐信息查询实体</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns></returns>
        public IList<Model.FinanceStructure.CashierRegisterInfo> GetList(Model.FinanceStructure.QueryCashierRegisterInfo model
            , int PageSize, int PageIndex, ref int RecordCount)
        {
            return idal.GetList(model, base.HaveUserIds, PageSize, PageIndex, ref RecordCount);
        }

        /// <summary>
        /// 获取出纳登帐信息金额合计
        /// </summary>
        /// <param name="model">出纳登帐信息查询实体</param>
        /// <param name="paymentCount">到款银行</param>
        /// <param name="totalAmount">已销账金额</param>
        public void GetCashierRegisterMoney(Model.FinanceStructure.QueryCashierRegisterInfo model
            , ref decimal paymentCount, ref decimal totalAmount)
        {
            if (model == null)
                return;

            idal.GetCashierRegisterMoney(model, HaveUserIds, ref paymentCount, ref totalAmount);
        }

        /// <summary>
        /// 销账
        /// </summary>
        /// <param name="RegistId">出纳登帐Id</param>
        /// <param name="OperatorId">当前操作员Id</param>
        /// <param name="OperatorName">当前操作员名称</param>
        /// <param name="list">销帐实体集合</param>
        /// <returns>返回1成功，其他失败</returns>
        public int CancelRegist(int RegistId, int OperatorId, string OperatorName
            , IList<EyouSoft.Model.FinanceStructure.CancelRegistInfo> list)
        {
            if (RegistId <= 0 || list == null || list.Count <= 0)
                return 0;

            int Result = idal.CancelRegist(RegistId, OperatorId, OperatorName, list);
            if (Result == 1)
            {
                string[] OrderIds = new string[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    OrderIds[i] = list[i].OrderId;
                }
                UtilityBll.UpdateCheckMoney(true, OrderIds);
                HandleLogsBll.Add(
                    new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                    {
                        ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_出纳登帐,
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_出纳登帐.ToString() + "新增了销账数据！编号为：" + RegistId,
                        EventTitle = "销账" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_出纳登帐.ToString() + "数据"
                    });
            }

            return Result;
        }
    }
}
