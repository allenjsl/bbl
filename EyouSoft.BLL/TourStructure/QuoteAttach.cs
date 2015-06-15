using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.TourStructure
{
    /// <summary>
    /// 描述:报价附件业务类
    /// 修改记录:
    /// 1. 2011-03-17 PM 曹胡生 创建
    /// </summary>
    public class QuoteAttach : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.TourStructure.IQuoteAttach dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.TourStructure.IQuoteAttach>();

        #region 构造函数

        public QuoteAttach()
        {
        }
        /// <summary>
        /// 用当前用户信息构造函数
        /// </summary>
        /// <param name="uinfo">当前登录用户信息</param>
        public QuoteAttach(EyouSoft.SSOComponent.Entity.UserInfo uinfo)
        {
            if (uinfo != null)
            {
                base.DepartIds = uinfo.Departs;
                base.CompanyId = uinfo.CompanyID;
            }
        }
        #endregion

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

        /// <summary>
        /// 获得报价附件列表
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize">每页显示的记录数</param>
        /// <param name="pageIndex">返回第几页</param>
        /// <param name="recordCount">返回的记录数</param>
        /// <param name="QuoteAttach">搜索实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.QuoteAttach> GetQuoteList(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.QuoteAttach QuoteAttach)
        {
            return dal.GetQuoteList(companyId, pageSize, pageIndex, ref recordCount, QuoteAttach);
        }

        /// <summary>
        /// 根据报价编号,获得报价附件信息
        /// </summary>
        /// <param name="QuoteId">报价编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.QuoteAttach GetQuoteInfo(int QuoteId)
        {
            return dal.GetQuoteInfo(QuoteId);
        }

        /// <summary>
        /// 添加报价附件信息
        /// </summary>
        /// <param name="QuoteAttach"></param>
        /// <returns></returns>
        public bool AddQuote(EyouSoft.Model.TourStructure.QuoteAttach QuoteAttach)
        {
            if (dal.AddQuote(QuoteAttach))
            {
                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_上传报价.ToString() + "上传了一个报价，上传标题：" + QuoteAttach.FileName;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "上传报价";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_上传报价;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                return true;
                #endregion
            }
            return false;
        }

        /// <summary>
        /// 更新报价附件信息
        /// </summary>
        /// <param name="QuoteAttach"></param>
        /// <returns></returns>
        public bool UpdateQuote(EyouSoft.Model.TourStructure.QuoteAttach QuoteAttach)
        {
            if (dal.UpdateQuote(QuoteAttach))
            {
                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_上传报价.ToString() + "更改了一个报价附件，标题：" + QuoteAttach.FileName;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "更改报价附件";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_上传报价;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                return true;
                #endregion
            }
            return false;
        }

        /// <summary>
        ///根据报价编号, 删除报价附件信息
        /// </summary>
        /// <param name="QuoteId"></param>
        /// <returns></returns>
        public bool DeleteQuote(params int[] QuoteId)
        {
            if (QuoteId == null || QuoteId.Length <= 0)
                return false;
            string strIds = string.Empty;
            foreach (int str in QuoteId)
            {
                strIds += str.ToString().Trim() + ",";
            }
            strIds = strIds.Trim(',');
            if (dal.DeleteQuote(strIds))
            {
                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_上传报价.ToString() + "删除了一个报价附件，编号：" + QuoteId.ToString();
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "删除报价附件";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_上传报价;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                return true;
                #endregion
            }
            return false;
        }
    }
}