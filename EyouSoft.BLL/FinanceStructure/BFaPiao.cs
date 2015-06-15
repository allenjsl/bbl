//汪奇志 2012-08-17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.FinanceStructure
{    
    /// <summary>
    /// 发票管理业务逻辑类
    /// </summary>
    public class BFaPiao
    {
        readonly EyouSoft.IDAL.FinanceStructure.IFaPiao dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.FinanceStructure.IFaPiao>();

        #region constructure
        /// <summary>
        /// default constructure
        /// </summary>
        public BFaPiao() { }
        #endregion

        #region public members
        /// <summary>
        /// 登记开票信息，操作成功返回1
        /// </summary>
        /// <param name="info">开票信息业务实体</param>
        /// <returns></returns>
        public int Insert(EyouSoft.Model.FinanceStructure.MFaPiaoInfo info)
        {
            if (info == null || info.CompanyId < 1 || info.CrmId < 1) return 0;

            info.Id = Guid.NewGuid().ToString();

            int dalRetCode = dal.Insert(info);

            if (dalRetCode == 1)
            {
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_发票管理.ToString() + "新增开票登记，登记编号为：" + info.Id;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "新增开票登记";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_发票管理;
                logInfo.OperatorId = 0;

                new EyouSoft.BLL.CompanyStructure.SysHandleLogs().Add(logInfo);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 修改开票信息，操作成功返回1
        /// </summary>
        /// <param name="info">开票信息业务实体</param>
        /// <returns></returns>
        public int Update(EyouSoft.Model.FinanceStructure.MFaPiaoInfo info)
        {
            if (info == null || string.IsNullOrEmpty(info.Id)) return 0;

            int dalRetCode = dal.Update(info);

            if (dalRetCode == 1)
            {
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_发票管理.ToString() + "修改开票登记，登记编号为：" + info.Id;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "修改开票登记";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_发票管理;
                logInfo.OperatorId = 0;

                new EyouSoft.BLL.CompanyStructure.SysHandleLogs().Add(logInfo);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 删除开票信息，操作成功返回1
        /// </summary>
        /// <param name="faPiaoId">发票登记编号</param>
        /// <returns></returns>
        public int Delete(string faPiaoId)
        {
            if (string.IsNullOrEmpty(faPiaoId)) return 0;

            int dalRetCode = dal.Delete(faPiaoId);

            if (dalRetCode == 1)
            {
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_发票管理.ToString() + "删除开票登记，登记编号为：" + faPiaoId;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "删除开票登记";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_发票管理;
                logInfo.OperatorId = 0;

                new EyouSoft.BLL.CompanyStructure.SysHandleLogs().Add(logInfo);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 获取发票登记信息业务实体
        /// </summary>
        /// <param name="faPiaoId">发票登记编号</param>
        /// <returns></returns>
        public EyouSoft.Model.FinanceStructure.MFaPiaoInfo GetInfo(string faPiaoId)
        {
            return dal.GetInfo(faPiaoId);
        }

        /// <summary>
        /// 获取发票管理列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="searchInfo">查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.MFaPiaoGuanLiInfo> GetFaPiaoGuanLis(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.FinanceStructure.MFaPiaoGuanLiSearchInfo searchInfo)
        {
            if (companyId < 1) return null;

            return dal.GetFaPiaoGuanLis(companyId, pageSize, pageIndex, ref recordCount, searchInfo);
        }

        /// <summary>
        /// 获取发票管理列表合计信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询实体</param>
        /// <param name="jiaoYiJinE">交易金额合计</param>
        /// <param name="kaiPiaoJinE">开票金额合计</param>
        public void GetFaPiaoGuanLisHeJi(int companyId, EyouSoft.Model.FinanceStructure.MFaPiaoGuanLiSearchInfo searchInfo, out decimal jiaoYiJinE, out decimal kaiPiaoJinE)
        {
            jiaoYiJinE = 0;
            kaiPiaoJinE = 0;
            if (companyId < 1) return;

            dal.GetFaPiaoGuanLisHeJi(companyId, searchInfo, out jiaoYiJinE, out kaiPiaoJinE);
        }

        /// <summary>
        /// 获取发票已登记列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="crmId">客户单位编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.MFaPiaoInfo> GetFaPiaos(int companyId, int crmId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.FinanceStructure.MFaPiaoSearchInfo searchInfo)
        {
            if (companyId < 1 || crmId < 1) return null;

            return dal.GetFaPiaos(companyId, crmId, pageSize, pageIndex, ref recordCount, searchInfo);
        }

        /// <summary>
        /// 获取发票已登记列表合计信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="crmId">客户单位编号</param>
        /// <param name="searchInfo">查询实体</param>
        /// <param name="kaiPiaoJinE">开票金额合计</param>
        public void GetFaPiaosHeJi(int companyId, int crmId, EyouSoft.Model.FinanceStructure.MFaPiaoSearchInfo searchInfo, out decimal kaiPiaoJinE)
        {
            kaiPiaoJinE = 0;
            if (companyId < 1 || crmId < 1) return;

            dal.GetFaPiaosHeJi(companyId, crmId, searchInfo, out kaiPiaoJinE);
        }
        #endregion
    }
}
