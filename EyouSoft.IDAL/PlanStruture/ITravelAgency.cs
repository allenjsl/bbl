using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.Model.PlanStructure;

namespace EyouSoft.IDAL.PlanStruture
{
    /// <summary>
    /// 地接社相关操作方法接口
    /// autor:李焕超 datetime:2011-1-19
    /// </summary>
    public interface ITravelAgency
    {
        /// <summary>
        /// 安排地接社
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool RanguageTravel(LocalTravelAgencyInfo model);

        /// <summary>
        /// 返回地接社
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        LocalTravelAgencyInfo GetTravelModel(string ID);

        /// <summary>
        /// 修改地接
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        bool UpdateTravelModel(LocalTravelAgencyInfo Model);

        /// <summary>
        /// 删除地接
        /// </summary>
        /// <param name="TravelId">安排地接编号</param>
        /// <returns></returns>
        bool DeletTravelModel(string TravelId);

        /// <summary>
        /// 获取某团地接列表
        /// </summary>
        /// <param name="TravelId">团队编号</param>
        /// <returns></returns>
        IList<LocalTravelAgencyInfo> GetList(string TourId);

        /// <summary>
        /// 获取地接列表
        /// </summary>
        /// <param name="CompanyId">团队编号</param>
        /// <returns></returns>
        IList<LocalTravelAgencyInfo> GetTravelList(int CompanyId);

        /// <summary>
        /// 获取某个业务员支出统计列表
        /// </summary>
        /// <param name="SearchModel">查询实体</param>
        /// <param name="haveUserIds">用户Ids</param>
        /// <returns></returns>
        IList<PersonalStatics> GetStaticsList(EyouSoft.Model.StatisticStructure.QueryPersonnelStatistic SearchModel, string haveUserIds);

        /// <summary>
        /// 根据团号列出机票地接社支付信息
        /// </summary>
        /// <param name="TourId">团队编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        IList<PaymentList> GetSettleList(string TourId, MExpendSearchInfo searchInfo);

        /// <summary>
        /// 修改机票地接社支出金额
        /// </summary>
        /// <param name="TravelId">团队编号</param>
        /// <returns></returns>
        bool UpdateSettle(PaymentList Model);

        /// <summary>
        /// 获取付款提醒查看明细信息集合
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="ServerId"></param>
        /// <param name="Type">1代表地接，2代表票务</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        IList<ArrearInfo> GetFuKuanTiXingMingXi(int CompanyId, int ServerId, int ServerType, int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.PersonalCenterStructure.FuKuanTiXingChaXun searchInfo);
        /// <summary>
        /// 获取付款提醒查看明细信息汇总
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="supplierId">供应商编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="weiFuHeJi">未付金额合计</param>
        /// <returns></returns>
        void GetFuKuanTiXingMingXi(int companyId, int supplierId, EyouSoft.Model.PersonalCenterStructure.FuKuanTiXingChaXun searchInfo, out decimal weiFuHeJi);

        /// <summary>
        /// 统计中心-获取欠款列表（总支出；已付；未付）
        /// </summary>
        /// <param name="searchModel">查询实体</param>
        /// <param name="haveUserIds">用户Ids</param>
        /// <returns></returns>
        IList<ArrearInfo> GetArrear(EyouSoft.Model.PlanStructure.ArrearSearchInfo searchModel, string haveUserIds);

        /// <summary>
        /// 统计中心-获取欠款金额汇总信息
        /// </summary>
        /// <param name="searchModel">查询实体</param>
        /// <param name="haveUserIds">用户Ids</param>
        /// <param name="totalAmount">总支出</param>
        /// <param name="arrear">未付</param>
        /// <returns></returns>
        void GetArrearAllMoeny(ArrearSearchInfo searchModel, string haveUserIds, ref decimal totalAmount
            , ref decimal arrear);

    }
}
