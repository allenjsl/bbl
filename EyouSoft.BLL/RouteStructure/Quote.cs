/*Author:汪奇志 2011-01-21*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.RouteStructure
{
    /// <summary>
    /// 报价业务逻辑类
    /// </summary>
    /// Author:汪奇志 2011-01-21
    public class Quote : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.RouteStructure.IQuote dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.RouteStructure.IQuote>();
        /// <summary>
        /// 系统操作日志逻辑
        /// </summary>
        private readonly BLL.CompanyStructure.SysHandleLogs HandleLogsBll = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();

        #region constructure
        /// <summary>
        /// default constructure
        /// </summary>
        public Quote() { }

        /// <summary>
        /// constructor with specified initial value
        /// </summary>
        /// <param name="uinfo">login user info</param>
        public Quote(EyouSoft.SSOComponent.Entity.UserInfo uinfo)
        {
            if (uinfo != null)
            {
                base.DepartIds = uinfo.Departs;
                base.CompanyId = uinfo.CompanyID;
            }
        }
        #endregion

        #region 成员方法
        /// <summary>
        /// 写入团队计划报价信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">团队计划报价信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int InsertTourTeamQuote(EyouSoft.Model.RouteStructure.QuoteTeamInfo info)
        {
            if (info == null)
                return 0;
            int Result= dal.InsertTourTeamQuote(info);
            if (Result > 0)
            {
                HandleLogsBll.Add(
                       new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                       {
                           ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.线路产品库_线路产品库,
                           EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                           EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.线路产品库_线路产品库.ToString() + "新增线路报价！编号为：" + Result.ToString(),
                           EventTitle = "新增" + Model.EnumType.CompanyStructure.SysPermissionClass.线路产品库_线路产品库.ToString() + "报价数据"
                       });
            }
            return Result;
        }        

        /// <summary>
        /// 更新团队计划报价信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">团队计划报价信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int UpdateTourTeamQuote(EyouSoft.Model.RouteStructure.QuoteTeamInfo info)
        {
            if (info == null)
                return 0;
            int Result= dal.UpdateTourTeamQuote(info);
            if (Result > 0)
            {
                HandleLogsBll.Add(
                       new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                       {
                           ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.线路产品库_线路产品库,
                           EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                           EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.线路产品库_线路产品库.ToString() + "修改线路报价！编号为：" + info.QuoteId,
                           EventTitle = "修改" + Model.EnumType.CompanyStructure.SysPermissionClass.线路产品库_线路产品库.ToString() + "报价数据"
                       });
            }
            return Result;
        }

        /// <summary>
        /// 删除团队计划报价信息
        /// </summary>
        /// <param name="quoteId">报价编号</param>
        /// <returns></returns>
        public bool DeleteTourTeamQuote(int quoteId)
        {
            if (quoteId <= 0)
                return false;
            bool Result=dal.DeleteTourTeamQuote(quoteId);
            if (Result)
            {
                HandleLogsBll.Add(
                       new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                       {
                           ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.线路产品库_线路产品库,
                           EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                           EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.线路产品库_线路产品库.ToString() + "删除线路报价！编号为：" + quoteId,
                           EventTitle = "修改" + Model.EnumType.CompanyStructure.SysPermissionClass.线路产品库_线路产品库.ToString() + "报价数据"
                       });
            }
            return Result;
        }

        /// <summary>
        /// 获取团队计划报价信息集合
        /// </summary> 
        /// <param name="companyId">公司编号</param>
        /// <param name="routeId">线路编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns> 
        public IList<EyouSoft.Model.RouteStructure.QuoteTeamInfo> GetQuotesTeam(int companyId, int routeId, int pageSize, int pageIndex, ref int recordCount)
        {
            return dal.GetQuotesTeam(companyId, routeId, pageSize, pageIndex, ref recordCount);
        }

        /// <summary>
        /// 获取团队计划报价信息业务实体
        /// </summary>
        /// <param name="quoteId">报价编号</param>
        /// <returns></returns>
        public EyouSoft.Model.RouteStructure.QuoteTeamInfo GetQuoteInfo(int quoteId)
        {
            if (quoteId <= 0)
                return null;
            return dal.GetQuoteInfo(quoteId);
        }
        #endregion
    }
}
