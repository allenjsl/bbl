using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.TourStructure
{
    /// <summary>
    /// 描述：询价报价业务类
    /// 修改记录：
    /// 1.2010-3-18　PM 曹胡生　创建
    /// </summary>
    public class LineInquireQuoteInfo : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.TourStructure.ILineInquireQuoteInfo dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.TourStructure.ILineInquireQuoteInfo>();

        #region 构造函数

        public LineInquireQuoteInfo()
        {
        }
        /// <summary>
        /// 用当前用户信息构造函数
        /// </summary>
        /// <param name="uinfo">当前登录用户信息</param>
        public LineInquireQuoteInfo(EyouSoft.SSOComponent.Entity.UserInfo uinfo)
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
        /// 获取询价列表
        /// </summary>
        /// <param name="companyId">公司编号（专线：专线公司编号，组团：组团公司编号）</param>
        /// <param name="pageSize">每页显示的记录数</param>
        /// <param name="pageIndex">返回第几页</param>
        /// <param name="recordCount">返回的记录数</param>
        /// <param name="isZhuTuan">专线：False,组团：True</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LineInquireQuoteInfo> GetInquireList(int companyId, int pageSize, int pageIndex, ref int recordCount, bool isZhuTuan, EyouSoft.Model.TourStructure.LineInquireQuoteSearchInfo SearchInfo)
        {
            return dal.GetInquireList(companyId, pageSize, pageIndex, ref recordCount, isZhuTuan,SearchInfo);
        }
       /// <summary>
        /// 获取询价报价实体
       /// </summary>
       /// <param name="Id">主键编号</param>
       /// <param name="CompanyId">专线公司编号</param>
       /// <param name="CustomerId">组团公司编号</param>
       /// <param name="isZhuTuan">是否组团端</param>
       /// <returns></returns>
        public EyouSoft.Model.TourStructure.LineInquireQuoteInfo GetQuoteModel(int Id, int CompanyId, int CustomerId, bool isZhuTuan)
        {
            return dal.GetQuoteModel(Id, CompanyId, CustomerId,isZhuTuan?1:0);
        }

        /// <summary>
        /// 组团端添加一个询价
        /// </summary>
        /// <param name="LineInquireQuoteInfo"></param>
        /// <returns></returns>
        public bool AddInquire(EyouSoft.Model.TourStructure.LineInquireQuoteInfo LineInquireQuoteInfo)
        {
            if (LineInquireQuoteInfo.CompanyId == 0 || LineInquireQuoteInfo.CustomerId == 0) return false;
            LineInquireQuoteInfo.QuoteState = EyouSoft.Model.EnumType.TourStructure.QuoteState.未处理;
            if (dal.AddInquire(LineInquireQuoteInfo))
            {
                #region LGWR
                //EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                //logInfo.CompanyId = 0;
                //logInfo.DepatId = 0;
                //logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                //logInfo.EventIp = string.Empty;
                //logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_组团社询价.ToString() + "添加了一条询价";
                //logInfo.EventTime = DateTime.Now;
                //logInfo.EventTitle = "添加询价";
                //logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_组团社询价;
                //logInfo.OperatorId = 0;
                //this.Logwr(logInfo);
                return true;
                #endregion
            }
            return false;
        }

        /// <summary>
        /// 修改一个询价
        /// </summary>
        /// <param name="LineInquireQuoteInfo"></param>
        /// <returns></returns>
        public bool UpdateInquire(EyouSoft.Model.TourStructure.LineInquireQuoteInfo LineInquireQuoteInfo)
        {
            if (LineInquireQuoteInfo.CompanyId == 0 || LineInquireQuoteInfo.Id==0) return false;
            //LineInquireQuoteInfo.QuoteState = EyouSoft.Model.EnumType.TourStructure.QuoteState.未处理;
            if (dal.UpdateInquire(LineInquireQuoteInfo))
            {
                #region LGWR
                //EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                //logInfo.CompanyId = 0;
                //logInfo.DepatId = 0;
                //logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                //logInfo.EventIp = string.Empty;
                //logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_组团社询价.ToString() + "修改了一条询价";
                //logInfo.EventTime = DateTime.Now;
                //logInfo.EventTitle = "修改询价";
                //logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_组团社询价;
                //logInfo.OperatorId = 0;
                //this.Logwr(logInfo);
                return true;
                #endregion
            }
            return false;
        }

        /// <summary>
        /// 专线端修改报价
        /// </summary>
        /// <param name="LineInquireQuoteInfo"></param>
        /// <returns></returns>
        public bool UpdateQuote(EyouSoft.Model.TourStructure.LineInquireQuoteInfo LineInquireQuoteInfo)
        {
            if (LineInquireQuoteInfo.CompanyId == 0 || LineInquireQuoteInfo.Id == 0) return false;
            LineInquireQuoteInfo.QuoteState = EyouSoft.Model.EnumType.TourStructure.QuoteState.已处理;
            if (dal.UpdateQuote(LineInquireQuoteInfo))
            {
                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_组团社询价.ToString() + "添加了一条报价，编号："+LineInquireQuoteInfo.Id;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "添加报价";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_组团社询价;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                return true;
                #endregion
            }
            return false;
        }

        /// <summary>
        /// 专线端添加报价并生成团队计划
        /// </summary>
        /// <param name="info">团队计划列表信息业务实体</param> 
        /// <param name="LineInquireQuoteInfo">线路询价报价实体类</param>
        /// <returns></returns>
        public bool AddQuote(EyouSoft.Model.TourStructure.TourTeamInfo info,EyouSoft.Model.TourStructure.LineInquireQuoteInfo LineInquireQuoteInfo)
        {
            if (LineInquireQuoteInfo.CompanyId == 0 || LineInquireQuoteInfo.Id == 0) return false;

            if (QuoteSuccessInsertPlan(LineInquireQuoteInfo.Id, info))
            {
                //同步组团询价游客信息到团队计划订单
                dal.SyncQuoteTravellerToTourTeamOrder(LineInquireQuoteInfo.Id, info.TourId);

                LineInquireQuoteInfo.QuoteState = EyouSoft.Model.EnumType.TourStructure.QuoteState.已成功;
                if (dal.UpdateQuote(LineInquireQuoteInfo))
                {
                    #region LGWR
                    EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                    logInfo.CompanyId = 0;
                    logInfo.DepatId = 0;
                    logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                    logInfo.EventIp = string.Empty;
                    logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_组团社询价.ToString() + "添加了一条报价，编号：" + LineInquireQuoteInfo.Id;
                    logInfo.EventTime = DateTime.Now;
                    logInfo.EventTitle = "添加报价";
                    logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_组团社询价;
                    logInfo.OperatorId = 0;
                    this.Logwr(logInfo);
                    return true;
                    #endregion
                }
            }
            return false;
        }

        /// <summary>
        /// 专线端报价成功后生成快速发布计划
        /// </summary>
        /// <param name="QuoteId">报价编号</param>
        /// <param name="info">团队计划信息实体</param>
        /// <returns></returns>
        private bool QuoteSuccessInsertPlan(int QuoteId, EyouSoft.Model.TourStructure.TourTeamInfo info)
        {
            Tour TourBll = new Tour();
            //插团队计划
            if (TourBll.InsertTeamTourInfo(info) > 0)
            {
                return dal.QuoteAddPlanNo(QuoteId, info.TourId,info.OperatorId);
            }
            return false;
        }
        /// <summary>
        /// 删除询价报价记录
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool DelInquire(int CompanyId, params int[] Ids)
        {
            if (Ids == null || Ids.Length <= 0)
                return false;
            string strIds = string.Empty;
            foreach (int str in Ids)
            {
                strIds += str.ToString().Trim() + ",";
            }
            strIds = strIds.Trim(',');
            if (dal.DelInquire(CompanyId,strIds))
            {
                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_组团社询价.ToString() + "删除了一条报价，编号：" + strIds;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "删除报价";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_组团社询价;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                return true;
                #endregion
            }
            return false;
        }

        /// <summary>
        /// 获取组团社询价列表合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="renShuHeJi">人数合计</param>
        public void GetInquireListHeJi(int companyId, EyouSoft.Model.TourStructure.LineInquireQuoteSearchInfo searchInfo, out int renShuHeJi)
        {
            renShuHeJi = 0;
            if (companyId < 1) return;

            dal.GetInquireListHeJi(companyId, searchInfo, out renShuHeJi);
        }
    }
}
