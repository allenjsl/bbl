using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SiteStructure
{
    /// <summary>
    /// 描述：同行平台 机票政策 接口类
    /// 修改记录：
    /// 1. 2011-03-28 PM 曹胡生 创建
    /// </summary>
    public interface ITicketPolicy
    {
        /// <summary>
        /// 获得当前公司的机票政策
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        IList<EyouSoft.Model.SiteStructure.TicketPolicy> GetTicketPolicy(int CompanyId, int pageSize, int pageIndex, ref int recordCount);

        /// <summary>
        /// 获得机票政策实体信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        EyouSoft.Model.SiteStructure.TicketPolicy GetTicketPolicy(int Id,int companyId);

        /// <summary>
        /// 添加机票政策
        /// </summary>
        /// <param name="TicketPolicy"></param>
        /// <returns></returns>
        bool AddTicketPolicy(EyouSoft.Model.SiteStructure.TicketPolicy TicketPolicy);

        /// <summary>
        /// 修改机票政策
        /// </summary>
        /// <param name="TicketPolicy"></param>
        /// <returns></returns>
        bool UpdateTicketPolicy(EyouSoft.Model.SiteStructure.TicketPolicy TicketPolicy);

        /// <summary>
        /// 删除机票政策
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool DelTicketPolicy(int Id,int companyId);
    }
}
