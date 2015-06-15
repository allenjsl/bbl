using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.TourStructure
{
    /// <summary>
    /// 销售管理-订单信息BLL
    /// 创建人：luofx 2011-01-17
    /// </summary>
    /// 邵权江 2012-1-6 重写方法GetOrderListAboutTourCompany()
    public class TourOrder : EyouSoft.BLL.BLLBase
    {
        #region private readonly IDAL
        private readonly EyouSoft.IDAL.TourStructure.ITourOrder dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.TourStructure.ITourOrder>();
        /// <summary>
        /// 统一维护方法DAL
        /// </summary>
        private readonly EyouSoft.BLL.UtilityStructure.Utility utilityBll = new EyouSoft.BLL.UtilityStructure.Utility();
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public TourOrder()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public TourOrder(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }
        #endregion 构造函数

        #region 私有方法
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logInfo">日志信息</param>
        private void Logwr(EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo)
        {
            EyouSoft.BLL.CompanyStructure.SysHandleLogs logbll = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();
            logbll.Add(logInfo);
            logbll = null;
        }
        #endregion 私有方法

        #region 订单信息维护

        /// <summary>
        /// 获取订单实体信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        public Model.TourStructure.TourOrder GetOrderModel(int CompanyId, string OrderId)
        {
            Model.TourStructure.TourOrder tmpModel = dal.GetOrderModel(CompanyId, OrderId);

            if (tmpModel != null && tmpModel.TourClassId == Model.EnumType.TourStructure.TourType.团队计划)
                tmpModel.TourTeamUnit = new Tour().GetTourTeamUnit(tmpModel.TourId);

            return tmpModel;
        }
        /// <summary>
        /// 获取订单中心订单信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">订单中心查询实体</param>
        /// <param name="HaveUserIds">根据部门Id生成的用户Id字符串</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, EyouSoft.Model.TourStructure.OrderCenterSearchInfo SearchModel)
        {
            string HaveUserIds = base.HaveUserIds;
            return dal.GetOrderList(PageSize, PageIndex, ref  RecordCount, CompanyId, SearchModel, HaveUserIds);
        }
        /// <summary>
        /// 获取组团端订单中心订单信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">订单中心查询实体</param>
        /// <param name="HaveUserIds">根据部门Id生成的用户Id字符串</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetTourOrderList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, EyouSoft.Model.TourStructure.TourOrderSearchInfo SearchModel)
        {
            IList<EyouSoft.Model.TourStructure.TourOrder> ResultList = dal.GetTourOrderList(PageSize, PageIndex, ref  RecordCount, CompanyId, SearchModel);
            foreach (EyouSoft.Model.TourStructure.TourOrder item in ResultList)
            {
                item.OperatorList = dal.GetOperatorList(item.TourId);
            }
            return ResultList;
        }
        /// <summary>
        /// 获取销售收款订单信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">订单中心查询实体</param>        
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, EyouSoft.Model.TourStructure.SalerSearchInfo SearchInfo)
        {
            EyouSoft.Model.TourStructure.SearchInfoForDAL DALSearchModel = new EyouSoft.Model.TourStructure.SearchInfoForDAL()
            {
                RouteName = SearchInfo.RouteName,
                TourNo = SearchInfo.TourNo,
                CompanyName = SearchInfo.CompanyName,
                SalerId = SearchInfo.SalerId,
                OrderNo = SearchInfo.OrderNo,
                OrderState = SearchInfo.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单 ? new EyouSoft.Model.EnumType.TourStructure.OrderState[] { EyouSoft.Model.EnumType.TourStructure.OrderState.已成交 } : new EyouSoft.Model.EnumType.TourStructure.OrderState[] { EyouSoft.Model.EnumType.TourStructure.OrderState.未处理, EyouSoft.Model.EnumType.TourStructure.OrderState.已成交, EyouSoft.Model.EnumType.TourStructure.OrderState.已留位 },
                OperatorName = string.Empty,
                SalerName = string.Empty,
                LeaveDateFrom = SearchInfo.SDate,
                LeaveDateTo = SearchInfo.EDate,
                OperatorId = SearchInfo.OrderOperatorId,
                AreaId = null,
                IsSettle = SearchInfo.IsSettle,
                RouteId = null,
                BuyCompanyId = null,
                CreateDateFrom = null,
                CreateDateTo = null,
                SellCompanyId = null,
                TourType = null,
                HaveUserIds = base.HaveUserIds
            };
            IList<EyouSoft.Model.TourStructure.TourOrder> ResultList = dal.GetOrderList(PageSize, PageIndex,
                                                                                        ref RecordCount, DALSearchModel);
            DALSearchModel = null;
            return ResultList;
        }
        /// <summary>
        /// 获取组团端-财务管理订单信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>        
        /// <param name="SearchInfo">查询实体</param>        
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetTourOrderFinanceList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, EyouSoft.Model.TourStructure.TourFinanceSearchInfo SearchInfo)
        {
            EyouSoft.Model.TourStructure.SearchInfoForDAL DALSearchModel = new EyouSoft.Model.TourStructure.SearchInfoForDAL()
            {
                RouteName = SearchInfo.RouteName,
                TourNo = SearchInfo.TourNo,
                LeaveDateFrom = SearchInfo.LeaveDateFrom,
                LeaveDateTo = SearchInfo.LeaveDateTo,
                OperatorName = SearchInfo.OperatorName,
                SalerName = SearchInfo.SalerName,
                OperatorId = null,
                SalerId = null,
                AreaId = null,
                IsSettle = null,
                BuyCompanyId = CompanyId,
                CompanyName = string.Empty,
                RouteId = null,
                CreateDateFrom = null,
                CreateDateTo = null,
                OrderNo = string.Empty,
                SellCompanyId = null,
                TourType = null,
                HaveUserIds = string.Empty
            };
            IList<EyouSoft.Model.TourStructure.TourOrder> ResultList = dal.GetOrderList(PageSize, PageIndex,
                                                                                        ref RecordCount, DALSearchModel);
            //获取计调信息
            foreach (EyouSoft.Model.TourStructure.TourOrder item in ResultList)
            {
                item.OperatorList = dal.GetOperatorList(item.TourId);
            }
            DALSearchModel = null;
            return ResultList;
        }
        /// <summary>
        /// 根据线路id获取所有相关订单信息集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="RouteId">线路编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderListByRouteId(int PageSize, int PageIndex, ref int RecordCount, int RouteId)
        {
            if (RouteId <= 0)
            {
                return null;
            }
            EyouSoft.Model.TourStructure.SearchInfoForDAL DALSearchModel = new EyouSoft.Model.TourStructure.SearchInfoForDAL()
            {
                HaveUserIds = string.Empty,
                RouteId = RouteId,
                OrderState = new EyouSoft.Model.EnumType.TourStructure.OrderState[] { EyouSoft.Model.EnumType.TourStructure.OrderState.未处理, EyouSoft.Model.EnumType.TourStructure.OrderState.已成交, EyouSoft.Model.EnumType.TourStructure.OrderState.已留位 },
                OperatorName = string.Empty,
                SalerName = string.Empty,
                SalerId = null,
                LeaveDateFrom = null,
                LeaveDateTo = null,
                AreaId = null,
                SellCompanyId = null,
                TourType = null,
                RouteName = string.Empty,
                TourNo = string.Empty,
                CompanyName = string.Empty,
                OrderNo = string.Empty,
                OperatorId = null,
                IsSettle = null,
                BuyCompanyId = null,
                CreateDateFrom = null,
                CreateDateTo = null,
            };
            IList<EyouSoft.Model.TourStructure.TourOrder> ResultList = dal.GetOrderList(PageSize, PageIndex,
                                                                                        ref RecordCount, DALSearchModel);
            DALSearchModel = null;
            return ResultList;
        }
        /// <summary>
        /// 统计分析（收入对账单-未收金额订单信息列表或账龄分析表-拖欠金额订单信息列表）
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="SearchInfo">搜索条件实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderList(int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.TourStructure.SearchInfo SearchInfo)
        {
            EyouSoft.Model.TourStructure.SearchInfoForDAL DALSearchModel = new EyouSoft.Model.TourStructure.SearchInfoForDAL()
            {
                SalerId = SearchInfo.SalerId.HasValue ? new int[] { SearchInfo.SalerId.Value } : null,
                LeaveDateFrom = SearchInfo.LeaveDateFrom,
                LeaveDateTo = SearchInfo.LeaveDateTo,
                AreaId = SearchInfo.AreaId,
                SellCompanyId = SearchInfo.CompanyId,
                TourType = SearchInfo.TourType,
                OrderState = new EyouSoft.Model.EnumType.TourStructure.OrderState[] { EyouSoft.Model.EnumType.TourStructure.OrderState.未处理, EyouSoft.Model.EnumType.TourStructure.OrderState.已成交, EyouSoft.Model.EnumType.TourStructure.OrderState.已留位 },
                OperatorName = string.Empty,
                SalerName = string.Empty,
                RouteName = string.Empty,
                TourNo = string.Empty,
                CompanyName = SearchInfo.BuyCompanyName,
                OrderNo = string.Empty,
                OperatorId = null,
                RouteId = null,
                IsSettle = SearchInfo.IsSettle,
                BuyCompanyId = SearchInfo.BuyCompanyId,
                CreateDateFrom = null,
                CreateDateTo = null,
                HaveUserIds = HaveUserIds
            };

            #region  订单状态查询

            if (SearchInfo != null && SearchInfo.ComputeOrderType.HasValue)
            {
                if (SearchInfo.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单)
                    DALSearchModel.OrderState = new EyouSoft.Model.EnumType.TourStructure.OrderState[] { EyouSoft.Model.EnumType.TourStructure.OrderState.已成交 };
                else if (SearchInfo.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计有效订单)
                    DALSearchModel.OrderState = new EyouSoft.Model.EnumType.TourStructure.OrderState[] { EyouSoft.Model.EnumType.TourStructure.OrderState.未处理, EyouSoft.Model.EnumType.TourStructure.OrderState.已成交, EyouSoft.Model.EnumType.TourStructure.OrderState.已留位 };
            }

            #endregion

            IList<EyouSoft.Model.TourStructure.TourOrder> ResultList = dal.GetStatisticOrderList(PageSize, PageIndex,
                                                                                        ref RecordCount, DALSearchModel);
            DALSearchModel = null;
            return ResultList;
        }
        /// <summary>
        /// 根据出纳登记编号获取相关订单列表数据
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="CashierRegisterId">出纳登记Id</param>
        /// <param name="BuyCompanyId">组团公司Id，不大于0不不作为条件</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, int CashierRegisterId, int BuyCompanyId)
        {
            string HaveUserIds = base.HaveUserIds;
            return dal.GetOrderList(PageSize, PageIndex, ref RecordCount, CompanyId, CashierRegisterId, BuyCompanyId
                , HaveUserIds);
        }
        /// <summary>
        /// 获取团队结算订单信息集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="TourId">团队编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.OrderFinanceExpense> GetOrderList(int CompanyId, string TourId)
        {
            string HaveUserIds = base.HaveUserIds;
            return dal.GetOrderList(CompanyId, TourId, HaveUserIds);
        }
        /// <summary>
        /// 统计分析-员工业绩订单详情
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="SearchInfo">搜索条件实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderList(int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.StatisticStructure.QueryPersonnelStatistic SearchInfo)
        {
            return dal.GetOrderList(PageSize, PageIndex, ref RecordCount, SearchInfo, HaveUserIds);
        }
        /// <summary>
        /// 根据客户公司编号获取未结清帐款的订单信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="buyCompanyId">客户单位编号</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询实体</param>
        /// <param name="sellerId">销售员编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderListByBuyCompanyId(int PageSize, int PageIndex, ref int RecordCount, int companyId, int buyCompanyId, int sellerId, EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo searchInfo)
        {
            return dal.GetOrderListByBuyCompanyId(PageSize, PageIndex, ref RecordCount, companyId, buyCompanyId, sellerId, searchInfo);
        }

        /// <summary>
        /// 根据客户公司编号获取未结清帐款的订单信息集合(汇总)
        /// </summary>
        /// <param name="buyCompanyId">客户单位编号</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询实体</param>
        /// <param name="sellerId">销售员编号</param>
        /// <param name="daiShouKuanHeJi">待收款金额汇总</param>
        /// <returns></returns>
        public void GetOrderListByBuyCompanyId(int companyId, int buyCompanyId, int sellerId, EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo searchInfo, out decimal daiShouKuanHeJi)
        {
            dal.GetOrderListByBuyCompanyId(companyId, buyCompanyId, sellerId, searchInfo, out daiShouKuanHeJi);
        }

        /*/// <summary>
        /// 根据客户公司编号获取已成交订单信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="BuyCompanyId">客户公司编号</param>
        /// <param name="CompanyId">所属公司编号</param>
        /// <param name="SDate">出团开始时间</param>
        /// <param name="LDate">出团结束时间</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderListAboutTourCompany(int PageSize, int PageIndex, ref int RecordCount, int BuyCompanyId, int CompanyId, DateTime? SDate, DateTime? LDate)
        {
            return GetOrderListAboutTourCompany(PageSize,PageIndex,ref RecordCount,BuyCompanyId,CompanyId,SDate,LDate,EyouSoft.Model.EnumType.TourStructure.TourType.None);
        }*/
        /*/// <summary>
        /// 根据客户公司编号获取已成交订单信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="BuyCompanyId">客户公司编号</param>
        /// <param name="CompanyId">所属公司编号</param>
        /// <param name="SDate">出团开始时间</param>
        /// <param name="LDate">出团结束时间</param>
        /// <param name="TourType">团队类型</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderListAboutTourCompany(int PageSize, int PageIndex, ref int RecordCount, int BuyCompanyId, int CompanyId, DateTime? SDate, DateTime? LDate,EyouSoft.Model.EnumType.TourStructure.TourType TourType)
        {
            EyouSoft.Model.TourStructure.SearchInfoForDAL DALSearchModel = new EyouSoft.Model.TourStructure.SearchInfoForDAL()
            {
                BuyCompanyId = BuyCompanyId,
                SellCompanyId = CompanyId,
                OrderState = new EyouSoft.Model.EnumType.TourStructure.OrderState[] { EyouSoft.Model.EnumType.TourStructure.OrderState.已成交 },
                OperatorName = string.Empty,
                SalerName = string.Empty,
                SalerId = null,
                LeaveDateFrom = SDate,
                LeaveDateTo = LDate,
                AreaId = null,                
                TourType = null,
                RouteName = string.Empty,
                TourNo = string.Empty,
                CompanyName = string.Empty,
                OrderNo = string.Empty,
                OperatorId = null,
                RouteId = null,
                IsSettle = null,
                CreateDateFrom = null,
                CreateDateTo = null,
                HaveUserIds = string.Empty
            };
            if (TourType != EyouSoft.Model.EnumType.TourStructure.TourType.None)
                DALSearchModel.TourType = TourType;
            IList<EyouSoft.Model.TourStructure.TourOrder> ResultList = dal.GetOrderList(PageSize, PageIndex,
                                                                                        ref RecordCount, DALSearchModel);
            DALSearchModel = null;
            return ResultList;
        }*/
        /*/// <summary>
        /// 根据客户公司编号获取已成交订单信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="BuyCompanyId">客户公司编号</param>
        /// <param name="CompanyId">所属公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderListAboutTourCompany(int PageSize, int PageIndex, ref int RecordCount, int BuyCompanyId, int CompanyId)
        {
            return GetOrderListAboutTourCompany(PageSize, PageIndex, ref RecordCount, BuyCompanyId, CompanyId, null, null);
            EyouSoft.Model.TourStructure.SearchInfoForDAL DALSearchModel = new EyouSoft.Model.TourStructure.SearchInfoForDAL()
            {
                BuyCompanyId = BuyCompanyId,
                SellCompanyId = CompanyId,
                OrderState = new EyouSoft.Model.EnumType.TourStructure.OrderState[] { EyouSoft.Model.EnumType.TourStructure.OrderState.已成交 },
                OperatorName = string.Empty,
                SalerName = string.Empty,
                SalerId = null,
                LeaveDateFrom = null,
                LeaveDateTo = null,
                AreaId = null,
                TourType = null,
                RouteName = string.Empty,
                TourNo = string.Empty,
                CompanyName = string.Empty,
                OrderNo = string.Empty,
                OperatorId = null,
                RouteId = null,
                IsSettle = null,
                CreateDateFrom = null,
                CreateDateTo = null,
                HaveUserIds = string.Empty
            };
            IList<EyouSoft.Model.TourStructure.TourOrder> ResultList = dal.GetOrderList(PageSize, PageIndex,
                                                                                        ref RecordCount, DALSearchModel);
            DALSearchModel = null;
            return ResultList;
        }*/
        /// <summary>
        /// 获取财务管理-团款收入-应收帐款信息集合
        /// </summary>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourAboutAccountInfo> GetTourReciveAccountList(int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.TourStructure.OrderAboutAccountSearchInfo SearchInfo)
        {
            return dal.GetTourReciveAccountList(PageSize, PageIndex, ref RecordCount, SearchInfo, null, this.HaveUserIds);
        }

        /// <summary>
        /// 获取财务管理-团款收入-应收帐款信息金额合计
        /// </summary>
        /// <param name="searchInfo">财务订单帐款搜索实体</param>
        /// <param name="peopleNumber">总人数</param>
        /// <param name="financeSum">应收账款</param>
        /// <param name="hasCheckMoney">已收账款</param>
        /// <param name="notCheckMoney">未审核账款</param>
        /// <param name="NotCheckTuiMoney">未审核退款</param>  
        public void GetTourReciveAccountList(Model.TourStructure.OrderAboutAccountSearchInfo searchInfo
            , ref int peopleNumber, ref decimal financeSum, ref decimal hasCheckMoney, ref decimal notCheckMoney, ref decimal NotCheckTuiMoney)
        {
            if (searchInfo == null)
                return;

            dal.GetTourReciveAccountList(searchInfo, null, HaveUserIds, ref peopleNumber, ref financeSum,
                                         ref hasCheckMoney, ref notCheckMoney, ref NotCheckTuiMoney);
        }

        /// <summary>
        /// 获取财务管理-团款收入-已结清帐款信息集合
        /// </summary>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourAboutAccountInfo> GetTourHasReciveAccountList(int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.TourStructure.OrderAboutAccountSearchInfo SearchInfo)
        {
            return dal.GetTourReciveAccountList(PageSize, PageIndex, ref RecordCount, SearchInfo, true, this.HaveUserIds);
        }

        /// <summary>
        /// 获取财务管理-团款收入-已结清帐款信息金额合计
        /// </summary>
        /// <param name="searchInfo">财务订单帐款搜索实体</param>
        /// <param name="peopleNumber">总人数</param>
        /// <param name="financeSum">应收账款</param>
        /// <param name="hasCheckMoney">已收账款</param>
        /// <param name="notCheckMoney">未审核账款</param>
        public void GetTourHasReciveAccountList(Model.TourStructure.OrderAboutAccountSearchInfo searchInfo
            , ref int peopleNumber, ref decimal financeSum, ref decimal hasCheckMoney, ref decimal notCheckMoney)
        {
            decimal temp = 0;
            if (searchInfo == null)
                return;

            dal.GetTourReciveAccountList(searchInfo, true, HaveUserIds, ref peopleNumber, ref financeSum,
                                         ref hasCheckMoney, ref notCheckMoney, ref temp);
        }

        /// <summary>
        /// 根据据团队编号获取订单信息集合(打印结算明细单)        
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="TourId">团队编号</param>      
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderCustomerListByTourId(int CompanyId, string TourId)
        {
            IList<EyouSoft.Model.TourStructure.TourOrder> ResultList = dal.GetOrderList(CompanyId, TourId);
            foreach (EyouSoft.Model.TourStructure.TourOrder item in ResultList)
            {
                item.CustomerList = this.GetLeagueList(CompanyId, item.ID);
            }
            return ResultList;
        }

        /// <summary>
        /// 新增订单
        /// </summary>
        /// <param name="model">订单信息实体</param>
        /// <returns>
        /// 0:失败；
        /// 1:成功；
        /// 2：该团队的订单总人数+当前订单人数大于团队计划人数总和；
        /// 3：该客户所欠金额大于最高欠款金额；
        /// </returns>
        public int AddOrder(EyouSoft.Model.TourStructure.TourOrder model)
        {
            if (model.PeopleNumber <= 0) return 0;

            if (model.OrderType == EyouSoft.Model.EnumType.TourStructure.OrderType.代客预定)
            {
                model.LastOperatorID = model.OperatorID;
            }

            int RowCount = dal.AddOrder(model);
            if (RowCount == 1)
            {
                utilityBll.UpdateRouteSomething(new int[] { model.RouteId }, EyouSoft.Model.EnumType.TourStructure.UpdateTourType.修改收客数);
                utilityBll.UpdateTotalIncome(model.ID);
                if (model.OrderState == EyouSoft.Model.EnumType.TourStructure.OrderState.已成交)
                {
                    utilityBll.UpdateTradeNum(new int[] { model.BuyCompanyID });
                }
                utilityBll.CalculationTourSettleStatus(model.TourId);

                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心.ToString() + "新增了订单数据,团队类型为：" + model.TourClassId + "，订单编号为：" + model.ID;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "新增订单";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
                #endregion
            }
            return RowCount;
        }
        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="model">订单信息实体</param>
        /// <returns>
        /// 0:失败；
        /// 1:成功；
        /// 2：该团队的订单总人数+当前订单人数大于团队计划人数总和；
        /// 3：该客户所欠金额大于最高欠款金额；
        /// </returns>
        public int Update(EyouSoft.Model.TourStructure.TourOrder model)
        {
            if (model.PeopleNumber <= 0) return 0;

            if (model.TourClassId == Model.EnumType.TourStructure.TourType.团队计划 && new CompanyStructure.CompanySetting().GetTeamNumberOfPeople(model.SellCompanyId) == Model.EnumType.CompanyStructure.TeamNumberOfPeople.PartNumber)
            {
                if (model.TourTeamUnit == null || model.TourTeamUnit.NumberCr <= 0)
                    return 0;

                model.AdultNumber = model.TourTeamUnit.NumberCr;
                model.ChildNumber = model.TourTeamUnit.NumberEt;
                model.PersonalPrice = model.TourTeamUnit.UnitAmountCr;
                model.ChildPrice = model.TourTeamUnit.UnitAmountEt;
                model.PeopleNumber = model.TourTeamUnit.NumberCr + model.TourTeamUnit.NumberEt +
                                     model.TourTeamUnit.NumberQp;
            }
            else
            {
                model.TourTeamUnit = null;
            }

            int RowCount = dal.Update(model);
            if (RowCount == 1)
            {
                utilityBll.UpdateRouteSomething(new int[] { model.RouteId }, EyouSoft.Model.EnumType.TourStructure.UpdateTourType.修改收客数);
                utilityBll.UpdateTotalIncome(model.ID);
                if (model.OrderState == EyouSoft.Model.EnumType.TourStructure.OrderState.已成交)
                {
                    utilityBll.UpdateTradeNum(new int[] { model.BuyCompanyID });
                }
                utilityBll.CalculationTourSettleStatus(model.TourId);
                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心.ToString() + "修改了订单数据,团队类型为：" + model.TourClassId + "，订单编号为：" + model.ID;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "修改订单";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
                #endregion
            }
            return RowCount;
        }
        /// <summary>
        /// 修改订单增加/减少费用信息
        /// </summary>
        /// <param name="Lists">团队结算订单信息集合</param>
        /// <param name="TourId">团队编号</param>
        /// <returns></returns>
        public bool UpdateFinanceExpense(IList<EyouSoft.Model.TourStructure.OrderFinanceExpense> Lists, string TourId)
        {
            bool IsTrue = false;
            if (string.IsNullOrEmpty(TourId)) return IsTrue;
            IsTrue = dal.UpdateFinanceExpense(Lists);
            if (IsTrue)
            {
                IList<Model.StatisticStructure.IncomeItemIdAndType> IncomeItemlist = new List<Model.StatisticStructure.IncomeItemIdAndType>();
                foreach (EyouSoft.Model.TourStructure.OrderFinanceExpense item in Lists)
                {
                    Model.StatisticStructure.IncomeItemIdAndType IncomeItem = new EyouSoft.Model.StatisticStructure.IncomeItemIdAndType()
                    {
                        ItemId = item.OrderId,
                        ItemType = EyouSoft.Model.EnumType.TourStructure.ItemType.订单
                    };
                    IncomeItemlist.Add(IncomeItem);
                    IncomeItem = null;
                    utilityBll.UpdateCheckMoney(true, item.OrderId);
                }
                utilityBll.CalculationTourIncome(TourId, IncomeItemlist);
                utilityBll.CalculationTourSettleStatus(TourId);
                IncomeItemlist = null;
                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团队核算.ToString() + "修改订单增加/减少费用数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "修改订单增加,减少费用信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团队核算;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
                #endregion
            }
            return IsTrue;
        }
        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="OrderId">订单编号</param>
        /// <param name="OrderId">公司编号</param>
        /// <returns></returns>
        public bool Delete(string OrderId, int CompanyId, int RouteId)
        {
            bool IsTrue = dal.Delete(OrderId, CompanyId);
            if (IsTrue)
            {
                utilityBll.UpdateRouteSomething(new int[] { RouteId }, EyouSoft.Model.EnumType.TourStructure.UpdateTourType.修改收客数);
                utilityBll.UpdateTotalIncome(OrderId);
                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心.ToString() + "删除订单数据,订单编号为：" + OrderId;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "删除订单";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
                #endregion
            }
            return IsTrue;
        }
        /// <summary>
        /// 取消订单（订单更改成不受理）
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="OrderId">订单编号</param>
        /// <param name="TourId">团队编号</param>
        /// <returns></returns>
        public bool CancelOrder(int CompanyId, string OrderId, string TourId)
        {
            bool IsTrue = false;
            IsTrue = dal.CancelOrder(OrderId);
            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心.ToString() + "修改订单数据,订单编号为：" + OrderId + "的订单状态修改为" + EyouSoft.Model.EnumType.TourStructure.OrderState.不受理;
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "修改订单";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            logInfo = null;
            #endregion
            return IsTrue;
        }

        /// <summary>
        /// 创建订单号
        /// </summary>
        /// <returns>返回订单号</returns>
        public string CreateOrderNo()
        {
            return dal.CreateOrderNo();
        }
        #endregion 订单信息维护

        #region 游客基本信息

        /// <summary>
        /// 获取计划下所有游客信息集合（包含申请的航段信息集合、退的航段信息集合）
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrderCustomer> GetTravellers(string tourId)
        {
            return dal.GetTravellers(tourId);
        }
        /// <summary>
        /// 获取游客集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrderCustomer> GetCustomerList(int CompanyId, string OrderId)
        {
            return dal.GetCustomerList(CompanyId, OrderId, 0);
        }
        /// <summary>
        /// 增加游客信息
        /// </summary>
        /// <param name="CusList">订单游客信息集合</param>
        /// <returns></returns>
        public bool AddCustomerList(IList<EyouSoft.Model.TourStructure.TourOrderCustomer> CusList, string OrderId)
        {
            return dal.AddCustomerList(CusList, OrderId);
        }
        /// <summary>
        /// 修改游客信息
        /// </summary>
        /// <param name="CusList">订单游客信息集合</param>
        /// <returns></returns>
        public bool UpdateCustomerList(IList<EyouSoft.Model.TourStructure.TourOrderCustomer> CusList, string OrderId)
        {
            return dal.UpdateCustomerList(CusList, OrderId);
        }
        /// <summary>
        /// 获取单个游客是否可删除以及其票务状态
        /// </summary>
        /// <param name="CustomerId">游客编号</param>
        /// <param name="Msg">返回的信息</param>
        /// <returns></returns>
        public bool IsDoDelete(string CustomerId, ref string Msg)
        {
            return dal.IsDoDelete(CustomerId, ref Msg);
        }
        #endregion 游客基本信息

        #region 销售收款/退款
        /// <summary>
        /// 获取收款信息集合
        /// </summary>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> GetReceive(string OrderId)
        {
            return dal.GetReceiveRefund(OrderId, base.CompanyId, true);
        }
        /// <summary>
        /// 获取退款信息集合
        /// </summary>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> GetRefund(string OrderId)
        {
            return dal.GetReceiveRefund(OrderId, base.CompanyId, false);
        }
        /// <summary>
        /// 增加收款
        /// </summary>
        /// <param name="model">收入收退款登记明细信息实体</param>
        /// <param name="OrderId">订单编号</param>
        /// <param name="LogRefundType">日志区别收款/退款操作源头</param>
        /// <returns></returns>
        public bool AddReceive(EyouSoft.Model.FinanceStructure.ReceiveRefund model, string OrderId, EyouSoft.Model.EnumType.TourStructure.LogRefundSource LogRefundType)
        {
            bool IsTrue = false;
            IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> lists = new List<EyouSoft.Model.FinanceStructure.ReceiveRefund>();
            lists.Add(model);
            IsTrue = this.AddReceive(lists, OrderId, LogRefundType);
            lists = null;
            return IsTrue;
        }
        /// <summary>
        /// 批量增加收款
        /// </summary>
        /// <param name="lists">收入收退款登记明细信息集合</param>
        /// <param name="OrderId">订单编号</param>
        /// <param name="LogRefundType">日志区别收款/退款操作源头</param>
        /// <returns></returns>
        public bool AddReceive(IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> lists, string OrderId, EyouSoft.Model.EnumType.TourStructure.LogRefundSource LogRefundType)
        {
            bool IsTrue = false;
            IsTrue = dal.AddReceiveRefund(lists, OrderId, true);
            if (IsTrue)
            {
                utilityBll.UpdateCheckMoney(true, OrderId);
                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + LogRefundType.ToString() + "增加收款数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "增加收款信息";
                logInfo.ModuleId = (EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass), LogRefundType.ToString());
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
                #endregion
            }
            return IsTrue;
        }
        /// <summary>
        /// 增加退款
        /// </summary>
        /// <param name="model">收入收退款登记明细信息实体</param>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        public bool AddRefund(EyouSoft.Model.FinanceStructure.ReceiveRefund model, string OrderId, EyouSoft.Model.EnumType.TourStructure.LogRefundSource LogRefundType)
        {
            bool IsTrue = false;
            IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> lists = new List<EyouSoft.Model.FinanceStructure.ReceiveRefund>();
            lists.Add(model);
            IsTrue = this.AddRefund(lists, OrderId, LogRefundType);
            lists = null;
            return IsTrue;
        }
        /// <summary>
        /// 批量增加退款
        /// </summary>
        /// <param name="lists">收入收退款登记明细信息集合</param>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        public bool AddRefund(IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> lists, string OrderId, EyouSoft.Model.EnumType.TourStructure.LogRefundSource LogRefundType)
        {
            bool IsTrue = false;
            IsTrue = dal.AddReceiveRefund(lists, OrderId, false);
            if (IsTrue)
            {
                utilityBll.UpdateCheckMoney(false, OrderId);
                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + LogRefundType.ToString() + "增加退款数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "增加退款信息";
                logInfo.ModuleId = (EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass), LogRefundType.ToString());
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
                #endregion
            }
            return IsTrue;
        }
        /// <summary>
        /// 修改收款
        /// </summary>
        /// <param name="model">收入收退款登记明细信息实体</param>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        public bool UpdateReceive(EyouSoft.Model.FinanceStructure.ReceiveRefund model, string OrderId, EyouSoft.Model.EnumType.TourStructure.LogRefundSource LogRefundType)
        {
            bool IsTrue = false;
            IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> lists = new List<EyouSoft.Model.FinanceStructure.ReceiveRefund>();
            lists.Add(model);
            IsTrue = this.UpdateReceive(lists, OrderId, LogRefundType);
            lists = null;
            return IsTrue;
        }
        /// <summary>
        /// 批量修改收款
        /// </summary>
        /// <param name="lists">收入收退款登记明细信息集合</param>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        public bool UpdateReceive(IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> lists, string OrderId, EyouSoft.Model.EnumType.TourStructure.LogRefundSource LogRefundType)
        {
            bool IsTrue = false;
            IsTrue = dal.UpdateReceiveRefund(lists, OrderId, true);
            if (IsTrue)
            {
                utilityBll.UpdateCheckMoney(true, OrderId);

                #region stat.
                if (lists != null && lists.Count > 0)
                {
                    utilityBll.CalculationCashFlow(lists[0].CompanyID, DateTime.Today);
                }
                #endregion

                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + LogRefundType.ToString() + "修改收款数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "修改收款信息";
                logInfo.ModuleId = (EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass), LogRefundType.ToString());
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
                #endregion
            }
            return IsTrue;
        }
        /// <summary>
        /// 修改退款
        /// </summary>
        /// <param name="model">收入收退款登记明细信息实体</param>
        /// <param name="OrderId">订单编号</param>
        /// <param name="LogRefundType">区分退款/收款实体</param>
        /// <returns></returns>
        public bool UpdateRefund(EyouSoft.Model.FinanceStructure.ReceiveRefund model, string OrderId, EyouSoft.Model.EnumType.TourStructure.LogRefundSource LogRefundType)
        {
            bool IsTrue = false;
            IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> lists = new List<EyouSoft.Model.FinanceStructure.ReceiveRefund>();
            lists.Add(model);
            IsTrue = this.UpdateRefund(lists, OrderId, LogRefundType);
            lists = null;
            return IsTrue;
        }
        /// <summary>
        /// 批量修改退款
        /// </summary>
        /// <param name="lists">收入收退款登记明细信息实体</param>
        /// <param name="OrderId">订单编号</param>
        /// <param name="LogRefundType">区分退款/收款实体</param>
        /// <returns></returns>
        public bool UpdateRefund(IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> lists, string OrderId, EyouSoft.Model.EnumType.TourStructure.LogRefundSource LogRefundType)
        {
            bool IsTrue = false;
            IsTrue = dal.UpdateReceiveRefund(lists, OrderId, false);
            if (IsTrue)
            {
                utilityBll.UpdateCheckMoney(false, OrderId);
                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + LogRefundType.ToString() + "增加退款数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "修改退款信息";
                logInfo.ModuleId = (EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass), LogRefundType.ToString());
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
                #endregion
            }
            return IsTrue;
        }
        /// <summary>
        /// 删除退款
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="CompanyId">收款（退款）编号(主键)</param>
        /// <param name="OrderId">订单编号</param>
        /// <param name="LogRefundType">区分退款/收款实体</param>
        /// <returns></returns>
        public bool DeleteRefund(int CompanyId, string Id, string OrderId, EyouSoft.Model.EnumType.TourStructure.LogRefundSource LogRefundType)
        {
            bool IsTrue = false;
            IsTrue = dal.DeleteReceiveRefund(CompanyId, Id);
            if (IsTrue)
            {
                utilityBll.UpdateCheckMoney(false, OrderId);
                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + LogRefundType.ToString() + "删除退款数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "删除退款信息";
                logInfo.ModuleId = (EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass), LogRefundType.ToString());
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
                #endregion
            }
            return IsTrue;
        }
        /// <summary>
        /// 删除销售收款
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        ///  <param name="CompanyId">收款编号(主键)</param>
        /// <param name="OrderId">订单编号</param>
        /// <param name="LogRefundType">区分退款/收款实体</param>        
        /// <returns></returns>
        public bool DeleteReceive(int CompanyId, string Id, string OrderId, EyouSoft.Model.EnumType.TourStructure.LogRefundSource LogRefundType)
        {
            bool IsTrue = false;
            IsTrue = dal.DeleteReceiveRefund(CompanyId, Id);
            if (IsTrue)
            {
                utilityBll.UpdateCheckMoney(true, OrderId);
                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + LogRefundType.ToString() + "删除收款数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "删除收款信息";
                logInfo.ModuleId = (EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass), LogRefundType.ToString());
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
                #endregion
            }
            return IsTrue;
        }
        /// <summary>
        /// 删除销售收款
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        ///  <param name="CompanyId">退款编号(主键)</param>
        /// <param name="OrderId">订单编号</param>            
        /// <returns></returns>
        public bool CheckReceive(int CompanyId, string Id, string OrderId)
        {
            bool IsTrue = false;
            IsTrue = dal.CheckReceiveRefund(CompanyId, Id);
            if (IsTrue)
            {
                utilityBll.UpdateCheckMoney(true, OrderId);
                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款收入.ToString() + "审核收款数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "审核收款信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款收入;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
                #endregion
            }
            return IsTrue;
        }
        /// <summary>
        /// 审核退款
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">收款（退款）编号(主键)</param>
        /// <param name="OrderId">订单编号</param>    
        /// <returns></returns>
        public bool CheckRefund(int CompanyId, string Id, string OrderId)
        {
            bool IsTrue = false;
            IsTrue = dal.CheckReceiveRefund(CompanyId, Id);
            if (IsTrue)
            {
                utilityBll.UpdateCheckMoney(false, OrderId);
                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款收入.ToString() + "审核退款数据。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "审核退款信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款收入;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                logInfo = null;
                #endregion
            }
            return IsTrue;
        }
        #endregion 销售收款/退款

        #region 游客退票
        ///// <summary>
        ///// 获取可退票游客集合
        ///// </summary>
        ///// <param name="CompanyId">公司编号</param>
        ///// <param name="OrderId">订单编号</param>
        ///// <returns></returns>
        //public IList<EyouSoft.Model.TourStructure.TourOrderCustomer> GetCustomerRefundList(int CompanyId, string OrderId)
        //{
        //    return dal.GetCustomerList(CompanyId, OrderId, 1);
        //}

        /// <summary>
        /// 获取退票信息实体
        /// </summary>
        /// <param name="Id">退票编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.CustomerRefund GetCustomerRefund(string Id)
        {
            return dal.GetCustomerRefund(Id);
        }
        /// <summary>
        /// 修改退票
        /// </summary>
        /// <param name="model">游客退票信息实体</param>
        /// <param name="TourId">团队编号</param>
        /// <returns></returns>
        public bool UpdateCustomerRefund(EyouSoft.Model.TourStructure.CustomerRefund model, ref string TourId)
        {
            bool IsTrue = false;
            IsTrue = dal.UpdateCustomerRefund(model, ref TourId);
            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心.ToString() + "修改订单数据,使游客编号为" + model.CustormerId + "进行了修改退票操作。";
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "修改订单游客退票";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            logInfo = null;
            #endregion
            return IsTrue;
        }
        /// <summary>
        /// 新增退票
        /// </summary>
        /// <param name="model">游客退票信息实体</param>
        /// <param name="TourId">团队编号</param>
        /// <returns></returns>
        public bool AddCustomerRefund(EyouSoft.Model.TourStructure.CustomerRefund model, ref string TourId)
        {
            bool IsTrue = false;
            IsTrue = dal.AddCustomerRefund(model, ref TourId);
            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心.ToString() + "修改订单数据,为游客编号为" + model.CustormerId + "进行了退票操作。";
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "新增游客退票";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            logInfo = null;
            #endregion
            return IsTrue;
        }

        /// <summary>
        /// 获取游客的所有航段信息
        /// </summary>
        /// <param name="CustomerId">游客Id</param>
        /// <returns>游客的所有航段信息</returns>
        public Model.TourStructure.MCustomerAllFlight GetCustomerAllFlight(string CustomerId)
        {
            if (string.IsNullOrEmpty(CustomerId))
                return null;

            return dal.GetCustomerAllFlight(CustomerId);
        }

        #endregion 游客退票

        #region 游客退团
        /// <summary>
        /// 获取未退团游客集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrderCustomer> GetLeagueList(int CompanyId, string OrderId)
        {
            return dal.GetCustomerList(CompanyId, OrderId, 2);
        }
        /// <summary>
        /// 获取退团信息实体
        /// </summary>
        /// <param name="CustomerId">游客编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.CustomerLeague GetLeague(string CustomerId)
        {
            return dal.GetLeague(CustomerId);
        }
        /// <summary>
        /// 退团
        /// </summary>
        /// <param name="model">游客退团信息实体</param>
        /// <returns></returns>
        public bool AddLeague(EyouSoft.Model.TourStructure.CustomerLeague model)
        {
            bool IsTrue = false;
            string strTourId = string.Empty, strOrderId = string.Empty;
            IsTrue = dal.AddLeague(model, ref strTourId, ref strOrderId);
            if (IsTrue)
            {
                if (!string.IsNullOrEmpty(strOrderId))
                    utilityBll.UpdateTotalIncome(strOrderId);
                if (!string.IsNullOrEmpty(strTourId))
                    utilityBll.CalculationTourSettleStatus(strTourId);
            }

            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心.ToString() + "修改订单数据,使游客编号为" + model.CustormerId + "进行了退团操作。";
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "新增订单游客退团";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            logInfo = null;
            #endregion
            return IsTrue;
        }
        /// <summary>
        /// 修改退团
        /// </summary>
        /// <param name="model">游客退团信息实体</param>
        /// <returns></returns>
        public bool UpdateLeague(EyouSoft.Model.TourStructure.CustomerLeague model)
        {
            bool IsTrue = false;
            string strTourId = string.Empty, strOrderId = string.Empty;
            IsTrue = dal.UpdateLeague(model, ref strTourId, ref strOrderId);
            if (IsTrue)
            {
                if (!string.IsNullOrEmpty(strOrderId))
                    utilityBll.UpdateTotalIncome(strOrderId);
                if (!string.IsNullOrEmpty(strTourId))
                    utilityBll.CalculationTourSettleStatus(strTourId);
            }
            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心.ToString() + "修改订单数据,使游客编号为" + model.CustormerId + "进行了修改退团操作。";
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "修改订单游客退团";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            logInfo = null;
            #endregion
            return IsTrue;
        }
        #endregion 游客退团

        #region 游客特服
        /// <summary>
        /// 增加特服
        /// </summary>
        /// <param name="model">游客特服实体</param>
        /// <returns></returns>
        public bool AddSpecialService(EyouSoft.Model.TourStructure.CustomerSpecialService model)
        {
            bool IsTrue = false;
            IsTrue = dal.AddSpecialService(model);
            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心.ToString() + "修改订单数据,为游客编号为" + model.CustormerId + "进行了新增游客特服操作。";
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "新增游客特服";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            logInfo = null;
            #endregion
            return IsTrue;
        }
        /// <summary>
        /// 获取特服信息
        /// </summary>
        /// <param name="CustomerId">游客编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.CustomerSpecialService GetSpecialService(string CustomerId)
        {
            return dal.GetSpecialService(CustomerId);
        }
        /// <summary>
        /// 修改特服
        /// </summary>
        /// <param name="model">游客特服实体</param>
        /// <returns></returns>
        public bool UpdateSpecialService(EyouSoft.Model.TourStructure.CustomerSpecialService model)
        {
            bool IsTrue = false;
            IsTrue = dal.UpdateSpecialService(model);
            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心.ToString() + "修改订单数据,为游客编号为" + model.CustormerId + "进行了修改游客特服操作。";
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "修改游客特服";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.销售管理_订单中心;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            logInfo = null;
            #endregion
            return IsTrue;
        }
        #endregion 游客特服

        #region 其他方法
        /// <summary>
        /// 根据团队编号获取销售员信息集合
        /// </summary>
        /// <param name="TourId">团队编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.StatisticOperator> GetSalerInfo(string TourId)
        {
            return dal.GetSalerInfo(TourId);
        }
        /// <summary>
        /// 根据订单编号获取该订单组团公司的联系信息
        /// </summary>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CustomerInfo GetCustomerInfo(string OrderId)
        {
            return dal.GetCustomerInfo(OrderId);
        }
        /// <summary>
        /// 根据团队计划Id获取订单Id        
        /// </summary>
        /// <returns></returns>
        //public string GetOrderIdByTourId(string TourId)
        //{
        //    return dal.GetOrderIdByTourId(TourId);
        //}
        #endregion

        #region public members
        /// <summary>
        /// 根据订单状态获取订单信息集合
        /// </summary>
        /// <param name="companyId">公司(专线公司)编号</param>
        /// <param name="tourId">计划编号</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>        
        /// <param name="queryInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBOrderStatusTourOrderInfo> GetOrdersByOrderStatus(int companyId, string tourId, EyouSoft.Model.EnumType.TourStructure.OrderState orderStatus, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.OrderCenterSearchInfo queryInfo)
        {
            return dal.GetOrdersByOrderStatus(companyId, tourId, orderStatus, pageSize, pageIndex, ref recordCount, queryInfo, this.HaveUserIds);
        }

        /// <summary>
        /// 根据订单编号获取可退票游客信息集合
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrderCustomer> GetCanRefundTicketTravellers(string orderId)
        {
            return dal.GetCanRefundTicketTravellers(orderId);
        }

        /// <summary>
        /// 汇总符合条件的订单的总收入，已收金额
        /// </summary>
        /// <param name="searchInfo">查询实体</param>
        /// <param name="financeSum">总收入</param>
        /// <param name="hasCheckMoney">已收</param>
        public void GetFinanceSumByOrder(Model.TourStructure.SearchInfo searchInfo, ref decimal financeSum
            , ref decimal hasCheckMoney)
        {
            if (searchInfo == null)
                return;

            Model.TourStructure.SearchInfoForDAL dalSearchModel = new Model.TourStructure.SearchInfoForDAL()
            {
                SalerId = searchInfo.SalerId.HasValue ? new int[] { searchInfo.SalerId.Value } : null,
                LeaveDateFrom = searchInfo.LeaveDateFrom,
                LeaveDateTo = searchInfo.LeaveDateTo,
                AreaId = searchInfo.AreaId,
                SellCompanyId = searchInfo.CompanyId,
                TourType = searchInfo.TourType,
                OrderState = new EyouSoft.Model.EnumType.TourStructure.OrderState[] { EyouSoft.Model.EnumType.TourStructure.OrderState.未处理, EyouSoft.Model.EnumType.TourStructure.OrderState.已成交, EyouSoft.Model.EnumType.TourStructure.OrderState.已留位 },
                OperatorName = string.Empty,
                SalerName = string.Empty,
                RouteName = string.Empty,
                TourNo = string.Empty,
                CompanyName = searchInfo.BuyCompanyName,
                OrderNo = string.Empty,
                OperatorId = null,
                RouteId = null,
                IsSettle = searchInfo.IsSettle,
                BuyCompanyId = searchInfo.BuyCompanyId,
                CreateDateFrom = null,
                CreateDateTo = null,
                HaveUserIds = HaveUserIds
            };

            #region  订单状态查询

            if (searchInfo.ComputeOrderType.HasValue)
            {
                if (searchInfo.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单)
                    dalSearchModel.OrderState = new EyouSoft.Model.EnumType.TourStructure.OrderState[] { EyouSoft.Model.EnumType.TourStructure.OrderState.已成交 };
                else if (searchInfo.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计有效订单)
                    dalSearchModel.OrderState = new EyouSoft.Model.EnumType.TourStructure.OrderState[] { EyouSoft.Model.EnumType.TourStructure.OrderState.未处理, EyouSoft.Model.EnumType.TourStructure.OrderState.已成交, EyouSoft.Model.EnumType.TourStructure.OrderState.已留位 };
            }

            #endregion

            dal.GetFinanceSumByOrder(dalSearchModel, ref financeSum, ref hasCheckMoney);
        }

        /// <summary>
        /// 获取销售收款合计信息业务实体
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.MSaleReceivableSummaryInfo GetSaleReceivableSummary(int companyId, EyouSoft.Model.TourStructure.SalerSearchInfo searchInfo)
        {
            if (companyId <= 0 || searchInfo == null) return null;

            return dal.GetSaleReceivableSummary(companyId, searchInfo, this.HaveUserIds);
        }

        /// <summary>
        /// 机票审核根据团队Id获取订单信息
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="tourId">团队Id</param>
        /// <returns>返回订单部分信息列表</returns>
        public IList<Model.TourStructure.OrderByCheckTicket> GetOrderByCheckTicket(int companyId, string tourId)
        {
            if (companyId <= 0)
                return null;

            return dal.GetOrderByCheckTicket(companyId, tourId);
        }

        /// <summary>
        /// 机票审核根据团队Id获取订单信息
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="orderId">订单Id</param>
        /// <returns>返回订单部分信息列表</returns>
        public IList<Model.TourStructure.OrderByCheckTicket> GetOrderByCheckTicketByOrderId(int companyId, string orderId)
        {
            if (companyId <= 0)
                return null;

            return dal.GetOrderByCheckTicketByOrderId(companyId, orderId);
        }

        /// <summary>
        /// 根据订单编号获取订单对方操作员信息
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CustomerContactInfo GetOrderOtherSideContactInfo(string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return null;

            return dal.GetOrderOtherSideContactInfo(orderId);
        }

        /// <summary>
        /// 取消收款审核
        /// </summary>
        /// <param name="companyId">收款登记所在公司编号</param>
        /// <param name="orderId">收款登记的订单编号</param>
        /// <param name="checkedId">要取消收款审核的收款登记编号</param>
        /// <returns></returns>
        public bool CancelIncomeChecked(int companyId, string orderId, string checkedId)
        {
            if (companyId < 1 || string.IsNullOrEmpty(checkedId) || string.IsNullOrEmpty(orderId)) return false;

            if (dal.CancelIncomeChecked(companyId, orderId, checkedId))
            {
                var utilityBll = new EyouSoft.BLL.UtilityStructure.Utility();
                utilityBll.UpdateCheckMoney(true, orderId);
                utilityBll.CalculationCashFlow(companyId, DateTime.Today);
                utilityBll = null;

                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款收入.ToString() + "取消收款审核，收款登记编号为：" + checkedId;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "取消了收款审核";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款收入;
                logInfo.OperatorId = 0;
                new EyouSoft.BLL.CompanyStructure.SysHandleLogs().Add(logInfo);
                #endregion

                return true;
            }

            return false;
        }

        /// <summary>
        /// 取消退款审核
        /// </summary>
        /// <param name="companyId">收款登记所在公司编号</param>
        /// <param name="orderId">退款登记的订单编号</param>
        /// <param name="checkedId">要取消退款审核的退款登记编号</param>
        /// <returns></returns>
        public bool CancelIncomeTuiChecked(int companyId, string orderId, string checkedId)
        {
            if (companyId < 1 || string.IsNullOrEmpty(checkedId) || string.IsNullOrEmpty(orderId)) return false;

            if (dal.CancelIncomeTuiChecked(companyId, orderId, checkedId))
            {
                new EyouSoft.BLL.UtilityStructure.Utility().UpdateCheckMoney(true, orderId);

                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款收入.ToString() + "取消退款审核，退款登记编号为：" + checkedId;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "取消了退款审核";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款收入;
                logInfo.OperatorId = 0;
                new EyouSoft.BLL.CompanyStructure.SysHandleLogs().Add(logInfo);
                #endregion

                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取客户关系交易明细汇总
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="keHuId">客户单位编号</param>
        /// <param name="renCi">人次合计</param>
        /// <param name="jiaoYiJinE">交易金额合计</param>
        /// <param name="yiShouJinE">已收金额合计</param>
        /// <param name="chaXun">查询信息</param>
        public void GetKeHuJiaoYiMingXiHuiZong(int companyId, int keHuId, out int renCi, out decimal jiaoYiJinE, out decimal yiShouJinE, EyouSoft.Model.CompanyStructure.KeHuJiaoYiMingXiChaXunInfo chaXun)
        {
            renCi = 0;
            jiaoYiJinE = 0;
            yiShouJinE = 0;
            if (companyId < 1 || keHuId < 1) return;

            dal.GetKeHuJiaoYiMingXiHuiZong(companyId, keHuId, out renCi, out jiaoYiJinE, out yiShouJinE, chaXun);
        }

        /// <summary>
        /// 获取客户关系交易明细
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="keHuId">客户编号</param>
        /// <param name="chaXun">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.KeHuJiaoYiMingXiInfo> GetKeHuJiaoYiMingXi(int pageSize, int pageIndex, ref int recordCount, int companyId, int keHuId, EyouSoft.Model.CompanyStructure.KeHuJiaoYiMingXiChaXunInfo chaXun)
        {
            if (companyId < 1 || keHuId < 1) return null;

            return dal.GetKeHuJiaoYiMingXi(pageSize, pageIndex, ref recordCount, companyId, keHuId, chaXun);
        }

        /// <summary>
        /// 获取订单提醒集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<Model.TourStructure.MDingDanTiXingInfo> GetDingDanTiXing(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.MDingDanTiXingInfo searchInfo)
        {
            return dal.GetDingDanTiXing(companyId, pageSize, pageIndex, ref recordCount, searchInfo, HaveUserIds);
        }


        /// <summary>
        /// 获取订单退团人数及退团损失
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="tuiTuanChengRenShu">退团成人数</param>
        /// <param name="tuiTuanErTongShu">退团儿童数</param>
        /// <param name="tuiTuanSunShiJinE">退团损失金额</param>
        public void GetDingDanTuiTuanRenShu(string orderId, out int tuiTuanChengRenShu, out int tuiTuanErTongShu, out decimal tuiTuanSunShiJinE)
        {
            dal.GetDingDanTuiTuanRenShu(orderId, out tuiTuanChengRenShu, out tuiTuanErTongShu, out tuiTuanSunShiJinE);
        }
        #endregion

        #region 送机计划表

        /// <summary>
        /// 获取送机计划表
        /// </summary>
        /// <param name="trafficId">交通编号</param>
        /// <param name="leaveDate">出团日期</param>
        /// <returns></returns>
        public IList<Model.TourStructure.SongJiJiHuaBiao> GetSongJiJiHuaBiao(int trafficId, DateTime leaveDate)
        {
            if (trafficId <= 0) return null;

            return dal.GetSongJiJiHuaBiao(trafficId, leaveDate);
        }

        #endregion
    }
}
