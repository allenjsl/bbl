//汪奇志 2012-08-24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.FinanceStructure
{
    /// <summary>
    /// 团款收入相关业务逻辑类
    /// </summary>
    public class BShouRu
    {
        readonly EyouSoft.IDAL.FinanceStructure.IShouRu dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.FinanceStructure.IShouRu>();

        #region constructure
        /// <summary>
        /// default constructure
        /// </summary>
        public BShouRu() { }
        #endregion

        #region public members
        /// <summary>
        /// 获取财务管理-团款收入-批量收款审核列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.MLBShouKuanShenHeInfo> GetShouKuanShenHeLB(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.FinanceStructure.MLBShouKuanShenHeSearchInfo searchInfo)
        {
            if (companyId < 1) return null;

            return dal.GetShouKuanShenHeLB(companyId, pageSize, pageIndex, ref recordCount, searchInfo);
        }

        /// <summary>
        /// 获取财务管理-团款收入-批量收款审核列表合计
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="searchInfo"></param>
        /// <param name="skJinEHeJi">收款金额合计</param>
        public void GetShouKuanShenHeLBHeJi(int companyId, EyouSoft.Model.FinanceStructure.MLBShouKuanShenHeSearchInfo searchInfo,out decimal skJinEHeJi)
        {
            skJinEHeJi = 0;
            if (companyId < 1) return;

            dal.GetShouKuanShenHeLBHeJi(companyId, searchInfo, out skJinEHeJi);
        }

        /// <summary>
        /// 审核收款，返回1成功，其它失败
        /// </summary>
        /// <param name="dengJiId">登记编号</param>
        /// <param name="orderId">订单编号</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="operatorId">审核人编号</param>
        /// <returns></returns>
        public int ShenHeShouKuan(string dengJiId, string orderId, int companyId,int operatorId)
        {
            if (string.IsNullOrEmpty(dengJiId) || string.IsNullOrEmpty(orderId) || companyId < 1 || operatorId < 1) return 0;

            int dalRetCode = dal.ShenHeShouKuan(dengJiId, operatorId);

            if (dalRetCode == 1)
            {
                EyouSoft.BLL.UtilityStructure.Utility utilityBll = new EyouSoft.BLL.UtilityStructure.Utility();
                utilityBll.UpdateCheckMoney(true, orderId);
                utilityBll.CalculationCashFlow(companyId, DateTime.Today);
                utilityBll = null;

                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款收入.ToString() + "审核收款，收款登记编号为：" + dengJiId;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "审核收款";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款收入;
                logInfo.OperatorId = 0;
                new EyouSoft.BLL.CompanyStructure.SysHandleLogs().Add(logInfo);
                #endregion
            }

            return dalRetCode;
        }
        #endregion
    }
}
