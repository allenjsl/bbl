using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.Model.PlanStructure;

namespace EyouSoft.IDAL.PlanStruture
{
    /// <summary>
    /// 机票相关操作方法接口
    /// autor :李焕超  datetime:2011-1-19
    /// </summary>
    public interface IPlaneTicket
    {
        /// <summary>
        /// 申请机票
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool addTicketOutListModel(TicketOutListInfo model);

        /// <summary>
        /// 出票/修改机票
        /// </summary>
        /// <param name="TicketId"></param>
        /// <param name="registerId">完成出票机票款自动结清付款登记编号 为空时未做付款登记</param>
        /// <returns></returns>
        bool ToTicketOut(TicketOutListInfo TicketModel, out string registerId);

        /// <summary>
        /// 退票
        /// </summary>
        /// <param name="TicketId"></param>
        /// <param name="State"></param>
        /// <param name="ReturnMoney"></param>
        /// <returns></returns>
        bool ReturnTicketOut(EyouSoft.Model.PlanStructure.TicketRefundModel model);

        /// <summary>
        /// 返回申请机票实体
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        TicketOutListInfo GetTicketModel(string TicketId);

        /// <summary>
        /// 返回退订机票实体
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        string GetRefundTicketModel(int TicketId);

        /// <summary>
        /// 机票管理列表
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyID"></param>
        /// <param name="strWhere"></param>
        /// <param name="filedOrder"></param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        IList<EyouSoft.Model.PlanStructure.TicketInfo> GetTicketList(int PageSize, int PageIndex, int CompanyID, ref int RecordCount, ref int PageCount, string us);

        /// <summary>
        /// 财务管理机票审核列表
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyID"></param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        IList<EyouSoft.Model.PlanStructure.TicketInfo> GetCheckedTicketList(int PageSize, int PageIndex, int CompanyID, ref int RecordCount, ref int PageCount, string us);

        /// <summary>
        /// 搜索函数
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyId"></param>
        /// <param name="Operator"></param>
        /// <param name="DepartureTime"></param>
        /// <param name="FligthSegment"></param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        IList<TicketInfo> SearchTicketOut(int PageSize, int PageIndex, TicketSearchModel SearchModel, ref int RecordCount, ref int PageCount, string us);

        /*
        /// <summary>
        /// 散拼计划-申请机票列表
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyID"></param>
        /// <param name="strWhere"></param>
        /// <param name="filedOrder"></param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        IList<EyouSoft.Model.PlanStructure.TicketListModel> GetTicketList(int CompanyID, string TourId);*/

        /*/// <summary>
        /// 机票统计函数
        /// </summary>
        /// <param name="SearchModel"></param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        IList<EyouSoft.Model.PlanStructure.TicketStatisticsModel> GetTicketCount(TicketStatisticsSearchModel SearchModel, string us);*/

        /// <summary>
        /// 财务审核机票
        /// </summary>
        /// <param name="TicketId"></param>
        /// <param name="ReviewRemark">账务审核备注</param> 
        /// <returns></returns>
        bool CheckTicket(string TicketId, string ReviewRemark);

        /// <summary>
        /// 已订机票游客
        /// </summary>
        /// <param name="TourId"></param>
        /// <returns></returns>
        IList<string> CustomerList(string TourId);

        /// <summary>
        /// 已出票的操作员名字
        /// </summary>
        /// <param name="TourId"></param>
        /// <returns></returns>
        IList<string> CustomerOperatorList(string TourId);

        /// <summary>
        /// 删除一次申请机票
        /// </summary>
        /// <param name="TicketOutId"></param>
        /// <returns></returns>
        int DeleteTicket(string TicketOutId);

        /// <summary>
        /// 通过TicketId获取ticketOutId
        /// </summary>
        /// <param name="TicketId">TicketId</param>
        /// <returns></returns>
        string GetTicketOutId(int TicketId);

        /// <summary>
        /// 获取出票统计出票量明细
        /// </summary>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="model">查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        IList<Model.PlanStructure.TicketOutStatisticInfo> GetTicketOutStatisticList(int PageSize, int PageIndex
            , ref int RecordCount, Model.StatisticStructure.QueryTicketOutStatisti model, string HaveUserIds);

        /// <summary>
        /// 财务取消审核
        /// </summary>
        /// <param name="TicketId">机票申请Id</param>
        /// <returns>
        /// 1成功取消；
        /// 0参数错误；
        /// -1未找到对应的机票申请；
        /// -2团队状态或者机票申请状态不允许取消审核；
        /// -3取消审核过程中发生错误；
        /// </returns>
        int UNCheckTicket(string TicketId);

        /// <summary>
        /// 获取计划机票申请信息集合
        /// </summary>
        /// <param name="companyId">公司(专线)编号</param>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        IList<MLBTicketApplyInfo> GetTicketApplys(int companyId, string tourId);
        /// <summary>
        /// 机票管理退票，已退票时同时写杂费收入及收入明细，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="refundTicketInfo">机票退票信息业务实体</param>
        /// <returns></returns>
        int RefundTicket(MRefundTicketInfo refundTicketInfo);
        /// <summary>
        /// 获取游客退票航段信息集体
        /// </summary>
        /// <param name="refundId">退票编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.PlanStructure.TicketFlight> GetRefundTicketFlights(string refundId);

        /// <summary>
        /// 获取财务机票审核实体信息
        /// </summary>
        /// <param name="ticketId">机票申请Id</param>
        /// <returns>财务机票审核实体信息</returns>
        MCheckTicketInfo GetMCheckTicket(string ticketId);
        /// <summary>
        /// 取消出票(删除支出明细，支付登记明细，更改机票状态为未出票)
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <param name="planId">机票安排编号</param>
        /// <returns></returns>
        int CancelTicket(string tourId, string planId);

        /// <summary>
        /// 取消退票，返回1成功，其它失败
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="jiPiaoGuanLiId">机票管理编号</param>
        /// <returns></returns>
        int QuXiaoTuiPiao(int companyId, int jiPiaoGuanLiId);

        /// <summary>
        /// 取消机票审核，1:成功，-1:非审核通过状态下的机票申请不存在取消审核操作，-2:团队已提交财务不可取消审核
        /// </summary>
        /// <param name="ticketId">机票申请编号</param>
        /// <param name="tourId">团队编号</param>
        /// <returns></returns>
        int QuXiaoShenHe(string ticketId, out string tourId);

        /// <summary>
        /// 根据订单编号获取机票申请信息（因为一个订单只有一条机票申请信息）
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        TicketOutListInfo GetTicketOutInfoByOrderId(string orderId);
    }

}
