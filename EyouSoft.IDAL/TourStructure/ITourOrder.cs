using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.TourStructure
{
    /// <summary>
    /// 销售管理-订单相关处理IDAL
    /// 创建人：luofx 2011-01-19
    /// </summary>
    public interface ITourOrder
    {
        #region 订单信息维护
        /// <summary>
        /// 获取订单实体信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        EyouSoft.Model.TourStructure.TourOrder GetOrderModel(int CompanyId, string OrderId);
        /// <summary>
        /// 根据团队Id获取订单Id
        /// </summary>
        /// <param name="TourId">团队编号</param>
        /// <returns></returns>
        string GetOrderIdByTourId(string TourId);
        /// <summary>
        /// 获取订单中心订单信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号(组团社时传组团社的公司编号，反之专线公司编号)</param>
        /// <param name="SearchInfo">订单中心查询实体</param>
        /// <param name="HaveUserIds">根据部门Id生成的用户Id字符串</param>IsTour
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, EyouSoft.Model.TourStructure.OrderCenterSearchInfo SearchModel, string HaveUserIds);

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
        IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, int CashierRegisterId, int BuyCompanyId, string HaveUserIds);
        /// <summary>
        /// 获取团队结算订单信息集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="TourId">团队编号</param>
        /// <param name="HaveUserIds">根据部门Id生成的用户Id字符串</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.OrderFinanceExpense> GetOrderList(int CompanyId, string TourId, string HaveUserIds);

        /// <summary>
        /// 统计分析-员工业绩订单详情
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="SearchInfo">搜索条件实体</param>
        /// <param name="haveUserIds">用户Ids</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderList(int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.StatisticStructure.QueryPersonnelStatistic SearchInfo, string haveUserIds);

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
        IList<EyouSoft.Model.TourStructure.TourOrder> GetTourOrderList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, EyouSoft.Model.TourStructure.TourOrderSearchInfo SearchModel);
        /// <summary>
        /// 获取财务管理应收帐款信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="SearchInfo">财务订单帐款搜索实体</param>
        /// <param name="IsReceived">是否已结清</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourAboutAccountInfo> GetTourReciveAccountList(int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.TourStructure.OrderAboutAccountSearchInfo SearchInfo, bool? IsReceived, string us);

        /// <summary>
        /// 获取订单信息集合
        /// --综合方法
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">查询实体</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderList(int PageSize, int PageIndex, ref int RecordCount,
                                                                   EyouSoft.Model.TourStructure.SearchInfoForDAL
                                                                       SearchInfo);
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
        IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderListByBuyCompanyId(int PageSize, int PageIndex, ref int RecordCount, int companyId, int buyCompanyId, int sellerId, EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo searchInfo);
        /// <summary>
        /// 根据客户公司编号获取未结清帐款的订单信息集合(汇总)
        /// </summary>
        /// <param name="buyCompanyId">客户单位编号</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询实体</param>
        /// <param name="sellerId">销售员编号</param>
        /// <param name="daiShouKuanHeJi">待收款金额汇总</param>
        /// <returns></returns>
        void GetOrderListByBuyCompanyId(int companyId, int buyCompanyId, int sellerId, EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo searchInfo, out decimal daiShouKuanHeJi);
        /// <summary>
        /// 根据据团队编号获取订单信息集合        
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="TourId">团队编号</param>        
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderList(int CompanyId, string TourId);
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
        int AddOrder(EyouSoft.Model.TourStructure.TourOrder model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">订单信息实体</param>
        /// <returns>
        /// 0:失败；
        /// 1:成功；
        /// 2：该团队的订单总人数+当前订单人数大于团队计划人数总和；
        /// 3：该客户所欠金额大于最高欠款金额；
        /// </returns>
        int Update(EyouSoft.Model.TourStructure.TourOrder model);
        /// <summary>
        /// 修改订单增加/减少费用信息
        /// </summary>
        /// <param name="Lists">团队结算订单信息集合</param>
        /// <returns></returns>
        bool UpdateFinanceExpense(IList<EyouSoft.Model.TourStructure.OrderFinanceExpense> Lists);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="OrderId">订单编号</param>
        /// <param name="OrderId">公司编号</param>
        /// <returns></returns>
        bool Delete(string OrderId, int CompanyId);
        /// <summary>
        /// 创建订单号
        /// </summary>
        /// <returns>返回订单号</returns>
        string CreateOrderNo();
        #endregion 订单信息维护

        #region 销售收款/退款
        /// <summary>
        /// 获取收退款信息
        /// </summary>
        /// <param name="OrderId">订单编号</param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="IsRecive">true=收款，false=退款</param>
        /// <returns></returns>
        IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> GetReceiveRefund(string OrderId, int CompanyId, bool IsRecive);
        /// <summary>
        /// 增加销售收款/退款
        /// </summary>
        /// <param name="lists">收入收退款登记明细信息集合</param>
        /// <param name="OrderId">订单编号</param>
        /// <param name="IsRecive">是否收款(true:收款;false:退款)</param>
        /// <returns></returns>
        bool AddReceiveRefund(IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> lists, string OrderId, bool IsRecive);
        /// <summary>
        /// 修改销售收款/退款
        /// </summary>
        /// <param name="model">收入收退款登记明细信息实体</param>
        /// <param name="OrderId">订单编号</param>
        /// <param name="IsRecive">是否收款(true:收款;false:退款)</param>
        /// <returns></returns>
        bool UpdateReceiveRefund(IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> Lists, string OrderId, bool IsRecive);
        /// <summary>
        /// 删除销售收款/退款
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="CompanyId">收款（退款）编号(主键)</param>
        /// <returns></returns>
        bool DeleteReceiveRefund(int CompanyId, string Id);
        /// <summary>
        /// 销售收款/退款审核
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="CompanyId">收款（退款）编号(主键)</param>
        /// <returns></returns>
        bool CheckReceiveRefund(int CompanyId, string Id);
        #endregion 销售收款/退款

        #region 游客基本信息
        /// <summary>
        /// 获取订单游客信息集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="OrderId">订单编号</param>
        /// <param name="CusType">0:所有，1:退票，2：退团</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourOrderCustomer> GetCustomerList(int CompanyId, string OrderId, int CusType);
        /// <summary>
        /// 获取计划下所有游客信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourOrderCustomer> GetTravellers(string tourId);
        /// <summary>
        /// 获取单个游客是否可删除以及其票务状态
        /// </summary>
        /// <param name="CustomerId">游客编号</param>
        /// <param name="Msg">返回的信息</param>
        /// <returns></returns>
        bool IsDoDelete(string CustomerId, ref string Msg);
        /// <summary>
        /// 增加游客信息
        /// </summary>
        /// <param name="CusList">订单游客信息集合</param>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        bool AddCustomerList(IList<EyouSoft.Model.TourStructure.TourOrderCustomer> CusList, string OrderId);
        /// <summary>
        /// 修改游客信息
        /// </summary>
        /// <param name="CusList">订单游客信息集合</param>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        bool UpdateCustomerList(IList<EyouSoft.Model.TourStructure.TourOrderCustomer> CusList, string OrderId);
        #endregion 游客基本信息

        #region 游客退团
        /// <summary>
        /// 新增退团
        /// </summary>
        /// <param name="model">游客退团信息实体</param>
        /// <param name="tourId">团队Id，返回参数</param>
        /// <param name="orderId">订单Id，返回参数</param>
        /// <returns></returns>
        bool AddLeague(Model.TourStructure.CustomerLeague model, ref string tourId, ref string orderId);
        /// <summary>
        /// 获取退团信息实体
        /// </summary>
        /// <param name="CustomerId">游客编号</param>
        /// <returns></returns>
        EyouSoft.Model.TourStructure.CustomerLeague GetLeague(string CustomerId);
        /// <summary>
        /// 修改退团
        /// </summary>
        /// <param name="model">游客退团信息实体</param>
        /// <param name="tourId">团队Id，返回参数</param>
        /// <param name="orderId">订单Id，返回参数</param>
        /// <returns></returns>
        bool UpdateLeague(Model.TourStructure.CustomerLeague model, ref string tourId, ref string orderId);

        #endregion 游客退团

        #region 游客退票
        /// <summary>
        /// 获取退票信息实体
        /// </summary>
        /// <param name="Id">退票编号</param>
        /// <returns></returns>
        EyouSoft.Model.TourStructure.CustomerRefund GetCustomerRefund(string Id);

        /// <summary>
        /// 修改退票
        /// </summary>
        /// <param name="model">游客退票信息实体</param>
        /// <param name="TourId">团队编号</param>
        /// <returns></returns>
        bool UpdateCustomerRefund(EyouSoft.Model.TourStructure.CustomerRefund model, ref string TourId);
        /// <summary>
        /// 退票
        /// </summary>
        /// <param name="model">游客退票信息实体</param>
        /// <param name="TourId">团队编号</param>
        /// <returns></returns>
        bool AddCustomerRefund(EyouSoft.Model.TourStructure.CustomerRefund model, ref string TourId);

        /// <summary>
        /// 获取游客的所有航段信息
        /// </summary>
        /// <param name="CustomerId">游客Id</param>
        /// <returns>游客的所有航段信息</returns>
        Model.TourStructure.MCustomerAllFlight GetCustomerAllFlight(string CustomerId);

        #endregion 游客退票

        #region 游客特服
        /// <summary>
        /// 获取特服信息
        /// </summary>
        /// <param name="CustomerId">游客编号</param>
        /// <returns></returns>
        EyouSoft.Model.TourStructure.CustomerSpecialService GetSpecialService(string CustomerId);
        /// <summary>
        /// 增加特服
        /// </summary>
        /// <param name="model">游客特服实体</param>
        /// <returns></returns>
        bool AddSpecialService(EyouSoft.Model.TourStructure.CustomerSpecialService model);
        /// <summary>
        /// 修改特服
        /// </summary>
        /// <param name="model">游客特服实体</param>
        /// <returns></returns>
        bool UpdateSpecialService(EyouSoft.Model.TourStructure.CustomerSpecialService model);
        #endregion 游客特服

        #region 其他方法
        /// <summary>
        /// 根据团队编号获取销售员信息集合
        /// </summary>
        /// <param name="TourId">团队编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.StatisticStructure.StatisticOperator> GetSalerInfo(string TourId);
        /// <summary>
        /// 根据收退款id获取订单Id
        /// </summary>
        /// <param name="RefundId">收退款id</param>
        /// <returns></returns>
        string GetOrderIdByRefundId(string RefundId);

        /// <summary>
        /// 数组转换成字符串已逗号隔开
        /// </summary>
        /// <param name="item">要转换的数组</param>        
        /// <returns></returns>
        string ConvertIntArrayTostring(int[] Item);
        /// <summary>
        /// 获取计调信息集合
        /// </summary>
        /// <param name="TourId">团队编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourOperator> GetOperatorList(string TourId);
        /// <summary>
        /// 根据订单状态获取组团公司各订单的数量
        /// </summary>
        /// <param name="BuyCompanyId">组团公司编号</param>
        /// <returns></returns>
        int GetOrderCountByBuyCompanyId(int BuyCompanyId, EyouSoft.Model.EnumType.TourStructure.OrderState? OrderState);
        /// <summary>
        /// 根据订单编号获取该订单组团公司的联系信息
        /// </summary>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.CustomerInfo GetCustomerInfo(string OrderId);

        #endregion

        #region public member

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
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.LBOrderStatusTourOrderInfo> GetOrdersByOrderStatus(int companyId, string tourId, EyouSoft.Model.EnumType.TourStructure.OrderState orderStatus, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.OrderCenterSearchInfo queryInfo, string us);

        /// <summary>
        /// 取消订单（订单更改成不受理）
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        bool CancelOrder(string orderId);

        /// <summary>
        /// 根据订单编号获取可退票游客信息集合
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourOrderCustomer> GetCanRefundTicketTravellers(string orderId);

        /// <summary>
        /// 汇总符合条件的订单的总收入，已收金额
        /// </summary>
        /// <param name="dalSearchModel">查询实体</param>
        /// <param name="financeSum">总收入</param>
        /// <param name="hasCheckMoney">已收</param>
        void GetFinanceSumByOrder(Model.TourStructure.SearchInfoForDAL dalSearchModel, ref decimal financeSum
                                  , ref decimal hasCheckMoney);

        /// <summary>
        /// 获取统计信息订单列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="searchInfo">查询实体</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourOrder> GetStatisticOrderList(int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.SearchInfoForDAL searchInfo);

        /// <summary>
        /// 获取财务管理应收帐款信息金额合计
        /// </summary>
        /// <param name="searchInfo">财务订单帐款搜索实体</param>
        /// <param name="isReceived">是否已结清</param>
        /// <param name="us">用户信息集合</param>
        /// <param name="peopleNumber">总人数</param>
        /// <param name="financeSum">应收账款</param>
        /// <param name="hasCheckMoney">已收账款</param>
        /// <param name="notCheckMoney">未审核账款</param>
        /// <param name="NotCheckTuiMoney">未审核退款</param>  
        void GetTourReciveAccountList(Model.TourStructure.OrderAboutAccountSearchInfo searchInfo, bool? isReceived
                                      , string us, ref int peopleNumber, ref decimal financeSum,
                                      ref decimal hasCheckMoney, ref decimal notCheckMoney, ref decimal NotCheckTuiMoney);
        /// <summary>
        /// 获取销售收款合计信息业务实体
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        EyouSoft.Model.TourStructure.MSaleReceivableSummaryInfo GetSaleReceivableSummary(int companyId, EyouSoft.Model.TourStructure.SalerSearchInfo searchInfo, string us);

        /// <summary>
        /// 机票审核根据团队Id获取订单信息
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="tourId">团队Id</param>
        /// <returns>返回订单部分信息列表</returns>
        IList<Model.TourStructure.OrderByCheckTicket> GetOrderByCheckTicket(int companyId, string tourId);

        /// <summary>
        /// 机票审核根据团队Id获取订单信息
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="orderId">订单Id</param>
        /// <returns>返回订单部分信息列表</returns>
        IList<Model.TourStructure.OrderByCheckTicket> GetOrderByCheckTicketByOrderId(int companyId, string orderId);

        /// <summary>
        /// 根据订单状态获取专线公司各订单状态的订单的数量
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="orderState">订单状态</param>
        /// <returns></returns>
        int GetOrderCountByCompanyId(int companyId, Model.EnumType.TourStructure.OrderState? orderState);
        /// <summary>
        /// 根据订单编号获取订单对方操作员信息
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.CustomerContactInfo GetOrderOtherSideContactInfo(string orderId);
        /// <summary>
        /// 取消收款审核
        /// </summary>
        /// <param name="companyId">收款登记所在公司编号</param>
        /// <param name="orderId">收款登记的订单编号</param>
        /// <param name="checkedId">要取消收款审核的收款登记编号</param>
        /// <returns></returns>
        bool CancelIncomeChecked(int companyId, string orderId, string checkedId);
        /// <summary>
        /// 取消退款审核
        /// </summary>
        /// <param name="companyId">退款登记所在公司编号</param>
        /// <param name="orderId">退款登记的订单编号</param>
        /// <param name="checkedId">要取消退款审核的退款登记编号</param>
        /// <returns></returns>
        bool CancelIncomeTuiChecked(int companyId, string orderId, string checkedId);
        /// <summary>
        /// 客户关系交易明细汇总
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="keHuId">客户单位编号</param>
        /// <param name="renCi">人次合计</param>
        /// <param name="jiaoYiJinE">交易金额合计</param>
        /// <param name="yiShouJinE">已收金额合计</param>
        /// <param name="chaXun">查询信息</param>
        void GetKeHuJiaoYiMingXiHuiZong(int companyId, int keHuId, out int renCi, out decimal jiaoYiJinE, out decimal yiShouJinE, EyouSoft.Model.CompanyStructure.KeHuJiaoYiMingXiChaXunInfo chaXun);
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
        IList<EyouSoft.Model.CompanyStructure.KeHuJiaoYiMingXiInfo> GetKeHuJiaoYiMingXi(int pageSize, int pageIndex, ref int recordCount, int companyId, int keHuId, EyouSoft.Model.CompanyStructure.KeHuJiaoYiMingXiChaXunInfo chaXun);
        /// <summary>
        /// 获取订单提醒集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        IList<Model.TourStructure.MDingDanTiXingInfo> GetDingDanTiXing(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.MDingDanTiXingInfo searchInfo, string us);
        /// <summary>
        /// 获取订单退团人数及退团损失
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="tuiTuanChengRenShu">退团成人数</param>
        /// <param name="tuiTuanErTongShu">退团儿童数</param>
        /// <param name="tuiTuanSunShiJinE">退团损失金额</param>
        void GetDingDanTuiTuanRenShu(string orderId, out int tuiTuanChengRenShu, out int tuiTuanErTongShu, out decimal tuiTuanSunShiJinE);

        #endregion

        #region 送机计划表

        /// <summary>
        /// 获取送机计划表
        /// </summary>
        /// <param name="trafficId">交通编号</param>
        /// <param name="leaveDate">出团日期</param>
        /// <returns></returns>
        IList<Model.TourStructure.SongJiJiHuaBiao> GetSongJiJiHuaBiao(int trafficId, DateTime leaveDate);

        #endregion
    }
}
