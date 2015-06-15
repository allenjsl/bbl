using System.Collections.Generic;
using EyouSoft.Model.StatisticStructure;
using System;

namespace EyouSoft.BLL.StatisticStructure
{
    /// <summary>
    /// 返佣统计业务逻辑
    /// </summary>
    /// 周文超 2011-06-08
    public class BCommissionStat
    {
        private readonly IDAL.StatisticStructure.ICommissionStat _dal = Component.Factory.ComponentFactory.CreateDAL<IDAL.StatisticStructure.ICommissionStat>();

        #region private members
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logInfo">日志信息</param>
        private void Logwr(EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo)
        {
            EyouSoft.BLL.CompanyStructure.SysHandleLogs logbll = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();
            logbll.Add(logInfo);
            logbll = null;
        }
        #endregion

        #region public members
        /// <summary>
        /// 获取客户返佣统计信息
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="queryModel">查询实体</param>
        /// <returns></returns>
        public IList<MCommissionStatInfo> GetCommissions(int pageSize, int pageIndex, ref int recordCount, int companyId, MCommissionStatSeachInfo queryModel)
        {
            if(companyId <= 0)
                return null;

            return _dal.GetCommissions(pageSize, pageIndex, ref recordCount, companyId, queryModel);
        }

        /// <summary>
        /// 获取返佣明细信息集合
        /// </summary>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="buyerCompanyId">客户单位编号（组团）</param>
        /// <param name="buyerContactId">客户单位联系人编号</param>
        /// <param name="commissionType">返佣类型</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.MCommissionDetailInfo> GetCommissionDetails(int pageSize, int pageIndex, ref int recordCount
            , int companyId, int buyerCompanyId, int buyerContactId, EyouSoft.Model.EnumType.CompanyStructure.CommissionType commissionType
            , EyouSoft.Model.StatisticStructure.MCommissionStatSeachInfo searchInfo)
        {
            if (companyId < 1 || buyerCompanyId < 1 || buyerContactId < 1) { throw new System.Exception("方法参数(companyId,buyerCompanyId,buyerContactId都要赋值)传递不正确。"); }

            return _dal.GetCommissionDetails(pageSize, pageIndex, ref recordCount
                , companyId, buyerCompanyId, buyerContactId, commissionType
                , searchInfo);
        }

        /// <summary>
        /// 支付返佣金额，返回1成功，其它失败
        /// </summary>
        /// <param name="info">支付信息业务实体</param>
        /// <returns></returns>
        /// <remarks>支出明细已完成</remarks>
        public int PayCommission(EyouSoft.Model.StatisticStructure.MPayCommissionInfo info)
        {
            if (info == null || info.BuyerCompanyId < 1 || info.CompanyId < 1 || string.IsNullOrEmpty(info.OrderId)
                || string.IsNullOrEmpty(info.TourId) || info.PayerId < 1) return 0;

            if (info.PayTime == DateTime.MinValue)
            {
                info.PayTime = DateTime.Now;
            }

            int dalExceptionCode = _dal.PayCommission(info);

            if (dalExceptionCode != 1) return dalExceptionCode;

            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.客户关系管理_返佣统计.ToString() + "支付返佣，支付金额为：" + info.Amount.ToString("C2") + "，订单编号为：" + info.OrderId;
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "支付返佣";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.客户关系管理_返佣统计;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            #endregion

            return dalExceptionCode;
        }
        #endregion
    }
}
