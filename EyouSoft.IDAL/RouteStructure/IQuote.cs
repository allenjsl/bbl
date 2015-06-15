/*Author:汪奇志 2011-01-21*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.RouteStructure
{
    /// <summary>
    /// 报价数据访问类接口
    /// </summary>
    /// Author:汪奇志 2011-01-21
    public interface IQuote
    {
        /// <summary>
        /// 写入团队计划报价信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">团队计划报价信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        int InsertTourTeamQuote(EyouSoft.Model.RouteStructure.QuoteTeamInfo info);
        /// <summary>
        /// 更新团队计划报价信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">团队计划报价信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        int UpdateTourTeamQuote(EyouSoft.Model.RouteStructure.QuoteTeamInfo info);
        /// <summary>
        /// 删除团队计划报价信息
        /// </summary>
        /// <param name="quoteId">报价编号</param>
        /// <returns></returns>
        bool DeleteTourTeamQuote(int quoteId);
        /// <summary>
        /// 获取团队计划报价信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="routeId">线路编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        IList<EyouSoft.Model.RouteStructure.QuoteTeamInfo> GetQuotesTeam(int companyId, int routeId, int pageSize, int pageIndex, ref int recordCount);
        /// <summary>
        /// 获取线路报价信息实体
        /// </summary>
        /// <param name="QuoteId">线路报价信息编号</param>
        /// <returns></returns>
        EyouSoft.Model.RouteStructure.QuoteTeamInfo GetQuoteInfo(int QuoteId);
    }
}
