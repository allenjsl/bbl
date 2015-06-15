using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.SiteStructure
{
    /// <summary>
    /// 描述：同行平台 机票政策 业务类
    /// 修改记录：
    /// 1. 2011-03-28 PM 曹胡生 创建
    /// </summary>
    public class TicketPolicy : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.SiteStructure.ITicketPolicy idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.SiteStructure.ITicketPolicy>();
        /// <summary>
        /// 获得当前公司的机票政策
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SiteStructure.TicketPolicy> GetTicketPolicy(int CompanyId, int pageSize, int pageIndex, ref int recordCount)
        {
            if(CompanyId==0)return null;
            return idal.GetTicketPolicy(CompanyId, pageSize, pageIndex, ref recordCount);
        }

        /// <summary>
        /// 获得机票政策实体信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EyouSoft.Model.SiteStructure.TicketPolicy GetTicketPolicy(int Id,int CompanyId)
        {
            if(Id==0 || CompanyId==0)return null;
            return idal.GetTicketPolicy(Id,CompanyId);
        }

        /// <summary>
        /// 添加机票政策
        /// </summary>
        /// <param name="TicketPolicy"></param>
        /// <returns></returns>
        public bool AddTicketPolicy(EyouSoft.Model.SiteStructure.TicketPolicy TicketPolicy)
        {
            if(TicketPolicy==null || TicketPolicy.CompanyId==0) return false;
            return idal.AddTicketPolicy(TicketPolicy);
        }

        /// <summary>
        /// 修改机票政策
        /// </summary>
        /// <param name="TicketPolicy"></param>
        /// <returns></returns>
        public bool UpdateTicketPolicy(EyouSoft.Model.SiteStructure.TicketPolicy TicketPolicy)
        {
            if (TicketPolicy == null || TicketPolicy.CompanyId == 0 || TicketPolicy.Id==0) return false;
            return idal.UpdateTicketPolicy(TicketPolicy);
        }

        /// <summary>
        /// 删除机票政策
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DelTicketPolicy(int Id,int CompanyId)
        {
           if(Id==0 || CompanyId==0) return false;
           return idal.DelTicketPolicy(Id,CompanyId);
        }
    }
}
