using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.PlanStruture
{
    /// <summary>
    /// 交通管理相关操作方法接口
    /// autor:李晓欢 datetime:2012-09-10
    /// </summary>
    public interface IPlanTrffic
    {
        /// <summary>
        /// 添加交通
        /// </summary>
        /// <param name="trafficModel">交通实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool AddPlanTraffic(EyouSoft.Model.PlanStructure.TrafficInfo trafficModel);

        /// <summary>
        /// 修改交通
        /// </summary>
        /// <param name="trafficModel">交通实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool UpdatePlanTraffic(EyouSoft.Model.PlanStructure.TrafficInfo trafficModel);

        /// <summary>
        /// 删除交通
        /// </summary>
        /// <param name="trafficIds">交通编号集合</param>
        /// <returns>true:成功 false:失败</returns>
        bool DeletePlanTraffic(string trafficIds);

        /// <summary>
        /// 获取交通集合列表
        /// </summary>
        /// <param name="SearchModel">交通名称</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="CompanyID">公司编号</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="us">用户信息集合</param>
        /// <returns>所有交通集合</returns>
        IList<EyouSoft.Model.PlanStructure.TrafficInfo> GetTrafficList(EyouSoft.Model.PlanStructure.TrafficSearch SearchModel, int PageSize, int PageIndex, int CompanyID, ref int RecordCount);

        /// <summary>
        /// 获取交通集合列表
        /// </summary>
        /// <param name="SearchModel">查询实体</param>
        /// <param name="CompanyID">公司编号</param>
        /// <param name="us">用户信息集合</param>
        /// <returns>所有交通集合</returns>
        IList<EyouSoft.Model.PlanStructure.TrafficInfo> GetTrafficList(EyouSoft.Model.PlanStructure.TrafficSearch SearchModel, int CompanyID);


        /// <summary>
        /// 设置交通状态
        /// </summary>
        /// <param name="traffiCID">交通编号</param>
        /// <param name="status">当前状态</param>
        /// <returns>状态设置</returns>
        bool UpdateChangeSatus(int trafficID, EyouSoft.Model.EnumType.PlanStructure.TrafficStatus status);

        /// <summary>
        /// 获取交通实体
        /// </summary>
        /// <param name="trafficID">交通编号</param>
        /// <returns>返回交通实体</returns>
        EyouSoft.Model.PlanStructure.TrafficInfo GetTrafficModel(int trafficID);



        #region 交通行程成员
        /// <summary>
        /// 新增交通行程
        /// </summary>
        /// <param name="travelModel">行程实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool AddPlanTravel(EyouSoft.Model.PlanStructure.TravelInfo travelModel);

        /// <summary>
        /// 获取交通行程
        /// </summary>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">总页数</param>
        /// <param name="CompanyID">公司编号</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="trafficID">交通编号</param>
        /// <returns>交通行程</returns>
        IList<EyouSoft.Model.PlanStructure.TravelInfo> GetTravelList(int PageSize, int PageIndex, int CompanyID, ref int RecordCount, int trafficID);

        /// <summary>
        /// 修改交通行程
        /// </summary>
        /// <param name="travelModel">行程实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool UpdatePlanTravel(EyouSoft.Model.PlanStructure.TravelInfo travelModel);

        /// <summary>
        /// 获取行程实体
        /// </summary>
        /// <param name="travelID">行程编号</param>
        /// <returns></returns>
        EyouSoft.Model.PlanStructure.TravelInfo GettravelModel(int travelID);


        /// <summary>
        /// 删除交通行程
        /// </summary>
        /// <param name="travelIds">交通行程编号集合</param>
        /// <param name="trfficIds">交通编号</param>
        /// <returns>true:成功 false:失败</returns>
        bool DeletePlanTravel(string travelIds, int trafficId);

        /// <summary>
        /// 获取价格实体
        /// </summary>
        /// <param name="traffIcID">交通编号</param>
        /// <param name="LTourDate">出团时间</param>
        /// <returns>返回价格实体</returns>
        EyouSoft.Model.PlanStructure.TrafficPricesInfo GetTrafficPriceModel(int traffIcID, DateTime LTourDate);

        /// <summary>
        /// 根据交通编号出团时间获取价格集合
        /// </summary>
        /// <param name="trafficId">交通编号集合</param>
        /// <param name="LTourDate">出团时间</param>
        /// <returns>返回价格集合</returns>
        IList<EyouSoft.Model.PlanStructure.TrafficPricesInfo> GetTrafficPriceList(DateTime LTourDate, params int[] trafficId);

        /// <summary>
        /// 添加价格
        /// </summary>
        /// <param name="pricemodel">价格实体</param>
        /// <returns>true 成功  false 失败</returns>
        bool AddTrafficPrice(EyouSoft.Model.PlanStructure.TrafficPricesInfo pricemodel);

        /// <summary>
        /// 修改价格
        /// </summary>
        /// <param name="pricemodel">价格实体</param>
        /// <returns>true 成功 false 失败</returns>
        bool UpdateTrafficPrice(EyouSoft.Model.PlanStructure.TrafficPricesInfo pricemodel);


        /// <summary>
        /// 获取价格实体
        /// </summary>
        /// <param name="pricesId">价格编号</param>
        /// <param name="trafficId">交通编号</param>
        /// <returns>价格实体</returns>
        EyouSoft.Model.PlanStructure.TrafficPricesInfo GetTrafficPrices(string pricesId, int trafficId);

        /// <summary>
        /// 获取价格列表
        /// </summary>
        ///<param name="trfficID">交通编号</param>
        /// <returns>价格集合</returns>
        IList<EyouSoft.Model.PlanStructure.TrafficPricesInfo> GetPricesList(int trafficID);

        /// <summary>
        /// 交通某一天的价格修改后同步更新影响到的团队的信息(预控人数)
        /// </summary>
        /// <param name="trafficId">交通编号</param>
        void TongBuGengXinTuanDui(int trafficId);

        /// <summary>
        /// 更新票状态
        /// </summary>
        /// <param name="pricesID">价格编号</param>
        ///  <param name="trafficId">交通编号</param>
        /// <param name="status">票状态</param>
        /// <returns>true 成功 false 失败</returns>
        bool UpdatePricesStatus(string pricesID, int trafficId, EyouSoft.Model.EnumType.PlanStructure.TicketStatus status);

        /// <summary>
        /// 获取交通出票统计信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="search">查询实体</param>
        /// <returns></returns>
        IList<Model.PlanStructure.JiaoTongChuPiao> GetJiaoTongChuPiao(
            int companyId, Model.PlanStructure.JiaoTongChuPiaoSearch search);

        #endregion
    }
}
