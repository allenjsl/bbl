using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.TourStructure
{
    /// <summary>
    /// 描述：询价报价接口类
    /// 修改记录：
    /// 1.2010-3-18　PM 曹胡生　创建
    /// </summary>
    public interface ILineInquireQuoteInfo
    {
        /// <summary>
        /// 获取组团端询价列表
        /// </summary>
        /// <param name="companyId">公司编号（专线：专线公司编号，组团：组团公司编号）</param>
        /// <param name="pageSize">每页显示的记录数</param>
        /// <param name="pageIndex">返回第几页</param>
        /// <param name="recordCount">返回的记录数</param>
        /// <param name="isZhuTuan">专线：False,组团：True</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.LineInquireQuoteInfo> GetInquireList(int companyId, int pageSize, int pageIndex, ref int recordCount, bool isZhuTuan, EyouSoft.Model.TourStructure.LineInquireQuoteSearchInfo SearchInfo);

        /// <summary>
        /// 获取询价报价实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <param name="CompanyId">专线公司编号</param>
        /// <param name="CustomerId">组团公司编号</param>
        /// <param name="isZhuTuan">是否组团端,1是，0不是</param>
        /// <returns></returns>
        EyouSoft.Model.TourStructure.LineInquireQuoteInfo GetQuoteModel(int Id, int CompanyId, int CustomerId, int isZhuTuan);

        /// <summary>
        /// 组团端添加一个询价
        /// </summary>
        /// <param name="LineInquireQuoteInfo"></param>
        /// <returns></returns>
        bool AddInquire(EyouSoft.Model.TourStructure.LineInquireQuoteInfo LineInquireQuoteInfo);

        /// <summary>
        /// 修改一个询价
        /// </summary>
        /// <param name="LineInquireQuoteInfo"></param>
        /// <returns></returns>
        bool UpdateInquire(EyouSoft.Model.TourStructure.LineInquireQuoteInfo LineInquireQuoteInfo);

        /// <summary>
        /// 专线端修改报价
        /// </summary>
        /// <param name="LineInquireQuoteInfo"></param>
        /// <returns></returns>
        bool UpdateQuote(EyouSoft.Model.TourStructure.LineInquireQuoteInfo LineInquireQuoteInfo);

        //  /// <summary>
        ///// 专线端添加报价
        ///// </summary>
        ///// <param name="LineInquireQuoteInfo"></param>
        ///// <returns></returns>
        //bool AddQuote(EyouSoft.Model.TourStructure.LineInquireQuoteInfo LineInquireQuoteInfo);
        
         /// <summary>
        /// 专线端生成快速发布版后更新询价信息表计划编号
        /// </summary>
        /// <param name="QuoteId">询价报价编号</param>
        /// <param name="TourId">生成的团队编号</param>
        /// <returns></returns>
        bool QuoteAddPlanNo(int QuoteId, string TourId, int OperatorId);
        
        /// <summary>
        /// 删除询价报价记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool DelInquire(int CompanyId, string Ids);

        /// <summary>
        /// 同步组团询价游客信息到团队计划订单
        /// </summary>
        /// <param name="quoteId">组团询价编号</param>
        /// <param name="tourId">生成的团队计划编号</param>
        /// <returns></returns>
        bool SyncQuoteTravellerToTourTeamOrder(int quoteId, string tourId);

        /// <summary>
        /// 获取组团社询价列表合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="renShuHeJi">人数合计</param>
        void GetInquireListHeJi(int companyId, EyouSoft.Model.TourStructure.LineInquireQuoteSearchInfo searchInfo, out int renShuHeJi);
    }
}
