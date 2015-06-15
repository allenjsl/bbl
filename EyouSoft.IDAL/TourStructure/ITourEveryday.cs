using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.TourStructure
{
    /// <summary>
    /// 散客天天发计划数据访问类接口
    /// </summary>
    /// Author：汪奇志 2011-03-18
    public interface ITourEveryday
    {
        /// <summary>
        /// 写入散客天天发计划信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">散客天天发计划信息业务实体</param>
        /// <returns></returns>
        int InsertTourEverydayInfo(EyouSoft.Model.TourStructure.TourEverydayInfo info);
        /// <summary>
        /// 更新散客天天发计划信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">散客天天发计划信息业务实体</param>
        /// <returns></returns>
        int UpdateTourEverydayInfo(EyouSoft.Model.TourStructure.TourEverydayInfo info);
        /// <summary>
        /// 删除散客天天发计划信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">天天发计划编号</param>
        /// <returns></returns>
        int DeleteTourEverydayInfo(string tourId);
        /// <summary>
        /// 获取天天发计划报价信息集合
        /// </summary>
        /// <param name="tourId">天天发计划编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> GetTourEverydayPriceStandards(string tourId);
        /// <summary>
        /// 获取散客天天发计划信息业务实体
        /// </summary>
        /// <param name="tourId">天天发计划编号</param>
        /// <returns></returns>
        EyouSoft.Model.TourStructure.TourEverydayInfo GetTourEverydayInfo(string tourId);
        /// <summary>
        /// 获取散客天天发计划信息集合
        /// </summary>
        /// <param name="companyId">公司（专线）编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="queryInfo">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.LBTourEverydayInfo> GetTourEverydays(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourEverydaySearchInfo queryInfo);
        /// <summary>
        /// 申请散客天天发计划，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">散客天天发计划申请信息业务实体</param>
        /// <returns></returns>
        int ApplyTourEveryday(EyouSoft.Model.TourStructure.TourEverydayApplyInfo info);
        /// <summary>
        /// 获取散客天天发计划申请信息业务实体
        /// </summary>
        /// <param name="applyId">申请编号</param>
        /// <returns></returns>
        EyouSoft.Model.TourStructure.TourEverydayApplyInfo GetTourEverydayApplyInfo(string applyId);        
        /// <summary>
        /// 获取散客天天发计划处理申请信息集合
        /// </summary>
        /// <param name="companyId">公司（专线）编号</param>
        /// <param name="tourId">天天发计划编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="handleStatus">处理状态</param>
        /// <param name="queryInfo">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.LBTourEverydayHandleInfo> GetTourEverydayHandleApplys(int companyId, string tourId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.EnumType.TourStructure.TourEverydayHandleStatus? handleStatus, EyouSoft.Model.TourStructure.TourEverydayHandleApplySearchInfo queryInfo);
        /// <summary>
        /// 散客天天发计划生成散拼计划后续工作(生成订单、更新申请状态、天天发计划已处理数量维护、未处理数量维护等)，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">散拼计划计划编号</param>
        /// <param name="everydayTourId">天天发计划编号</param>
        /// <param name="everydayTourApplyId">天天发计划申请编号</param>
        /// <param name="buyerCompanyId">客户单位编号</param>
        /// <returns></returns>
        int BuildTourFollow(string tourId, string everydayTourId, string everydayTourApplyId, out int buyerCompanyId);
    }
}
