using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.TourStructure
{
    /// <summary>
    /// 描述:报价附件接口类
    /// 修改记录:
    /// 1. 2010-03-17 PM 曹胡生 创建
    /// </summary>
    public interface IQuoteAttach
    {
        /// <summary>
        /// 获得报价附件列表
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize">每页显示的记录数</param>
        /// <param name="pageIndex">返回第几页</param>
        /// <param name="recordCount">返回的记录数</param>
        /// <param name="QuoteAttach">留言搜索实体</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.QuoteAttach> GetQuoteList(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.QuoteAttach QuoteAttach);

        /// <summary>
        /// 根据报价编号,获得报价附件信息
        /// </summary>
        /// <param name="QuoteId">报价编号</param>
        /// <returns></returns>
        EyouSoft.Model.TourStructure.QuoteAttach GetQuoteInfo(int QuoteId);

        /// <summary>
        /// 添加报价附件信息
        /// </summary>
        /// <param name="QuoteAttach"></param>
        /// <returns></returns>
        bool AddQuote(EyouSoft.Model.TourStructure.QuoteAttach QuoteAttach);

        /// <summary>
        /// 更新报价附件信息
        /// </summary>
        /// <param name="QuoteAttach"></param>
        /// <returns></returns>
        bool UpdateQuote(EyouSoft.Model.TourStructure.QuoteAttach QuoteAttach);

        /// <summary>
        ///根据报价编号, 删除报价附件信息
        /// </summary>
        /// <param name="QuoteId"></param>
        /// <returns></returns>
        bool DeleteQuote(string QuoteId);
    }
}
