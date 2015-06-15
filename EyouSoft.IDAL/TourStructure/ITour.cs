/*Author:汪奇志 2011-01-20*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.TourStructure
{
    /// <summary>
    /// 计划中心数据访问类接口
    /// </summary>
    /// Author:汪奇志 2011-01-20
    public interface ITour
    {
        /// <summary>
        /// 写入计划信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">计划信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        int InsertTourInfo(EyouSoft.Model.TourStructure.TourBaseInfo info);
        /// <summary>
        /// 修改计划信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">计划信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        int UpdateTourInfo(EyouSoft.Model.TourStructure.TourBaseInfo info);
        /// <summary>
        /// 删除计划信息
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <returns></returns>
        bool Delete(string tourId);
        /// <summary>
        /// 获取散拼计划信息集合（专线端）
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourBaseInfo> GetTours(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info, string us);
        /// <summary>
        /// 获取团队计划信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.LBTeamTourInfo> GetToursTeam(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info, string us);
        /// <summary>
        /// 获取单项服务信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.LBSingleTourInfo> GetToursSingle(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSingleSearchInfo info, string us);
        /// <summary>
        /// 获取待核算计划信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.LBAccountingTourInfo> GetToursNotAccounting(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info, string us);
        /// <summary>
        /// 获取已核算计划信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.LBAccountingTourInfo> GetToursAccounting(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info, string us);
        /// <summary>
        /// 获取计划信息业务实体
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <returns></returns>
        EyouSoft.Model.TourStructure.TourBaseInfo GetTourInfo(string tourId);
        /// <summary>
        /// 设置团队状态，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="status">状态</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        int SetStatus(string tourId, EyouSoft.Model.EnumType.TourStructure.TourStatus status);
        /// <summary>
        /// 设置团队成本确认状态，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="isCostConfirm">状态</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        int SetIsCostConfirm(string tourId, bool isCostConfirm);
        /// <summary>
        /// 设置团队复核状态，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="isReview">状态</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        int SetIsReview(string tourId, bool isReview);
        /// <summary>
        /// 设置团队机票状态，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="status">状态</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        int SetTourTicketStatus(string tourId, EyouSoft.Model.EnumType.PlanStructure.TicketState status);
        /// <summary>
        /// 设置团队虚拟实收人数，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="expression">人数的数值表达式</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        int SetTourVirtualPeopleNumber(string tourId, int expression);
        /// <summary>
        /// 删除团队附件信息
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="attachId">附件编号</param>
        /// <returns></returns>
        bool DeleteTourAttach(string tourId, int attachId);
        /// <summary>
        /// 生成团号
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="ldate">出团日期</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourChildrenInfo> CreateAutoTourCodes(int companyId, params DateTime[] ldates);
        /// <summary>
        /// 团号重复验证，返回重复的团号集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="tourId">团队编号 注:新增验证时传string.Empty或null</param>
        /// <param name="tourCodes">团号</param>
        /// <returns></returns>
        IList<string> ExistsTourCodes(int companyId, string tourId, params string[] tourCodes);
        /// <summary>
        /// 获取计划信息集合(按线路编号)
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="routeId">线路编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourBaseInfo> GetToursByRouteId(int companyId, int pageSize, int pageIndex, ref int recordCount, int routeId);
        /// <summary>
        /// 获取散拼计划报价信息集合
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> GetPriceStandards(string tourId);
        /// <summary>
        /// 获取计划列表（组团端）
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="tourDisplayType">团队展示方式</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.LBZTTours> GetToursZTD(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info, EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType tourDisplayType);
        /// <summary>
        /// 获取计划列表（供应商交易次数，仅地接）
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="gysId">供应商编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.LBGYSTours> GetToursGYSDiJie(int companyId, int pageSize, int pageIndex, ref int recordCount, int gysId,EyouSoft.Model.TourStructure.MTimesSummaryDiJieSearchInfo searchInfo);
        /// <summary>
        /// 获取计划列表（供应商交易次数，仅票务）
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="gysId">供应商编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.JPGYSTours> GetToursGYSJiPiao(int companyId, int pageSize, int pageIndex, ref int recordCount, int gysId);
        /// <summary>
        /// 获取计划列表（供应商交易次数，仅票务）
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="gysId">供应商编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.JPGYSTours> GetToursGYSJiPiao(int companyId, int pageSize, int pageIndex, ref int recordCount, int gysId, EyouSoft.Model.TourStructure.MTimesSummaryDiJieSearchInfo searchInfo);
        /// <summary>
        /// 获取计划列表（利润统计团队数）
        /// </summary>
        /// <param name="copanyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.LBLYTJTours> GetToursLYTJ(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.StatisticStructure.QueryEarningsStatistic info, string us);
        /// <summary>
        /// 获取计划列表（组团端首页）
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="areaId">线路区域编号</param>
        /// <param name="expression">返回指定行数的数值表达式</param>
        /// <param name="tourDisplayType">团队展示方式</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.LBZTTours> GetToursZTDSY(int companyId, int areaId, int expression, EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType tourDisplayType);
        /// <summary>
        /// 获取团队复核状态
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <returns></returns>
        bool GetIsReview(string tourId);
        /// <summary>
        /// 获取团队成本确认状态
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <returns></returns>
        bool GetIsCostConfirm(string tourId);
        /// <summary>
        /// 分页获取出团提醒
        /// </summary>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="HaveUserIds">拥有的用户Id集合</param>
        /// <param name="RemindDays">提前X天</param>
        /// <returns></returns>
        IList<EyouSoft.Model.PersonalCenterStructure.LeaveTourRemind> GetLeaveTourRemind(int pageSize, int pageIndex, ref int recordCount, int CompanyId, string HaveUserIds, int RemindDays);
        /// <summary>
        /// 分页获取回团提醒
        /// </summary>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="HaveUserIds">拥有的用户Id集合</param>
        /// <param name="RemindDays">提前X天</param>
        /// <returns></returns>
        IList<EyouSoft.Model.PersonalCenterStructure.BackTourRemind> GetBackTourRemind(int pageSize, int pageIndex, ref int recordCount, int CompanyId, string HaveUserIds, int RemindDays);
        /// <summary>
        /// 获取出团提醒数量
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="HaveUserIds">拥有的用户Id集合</param>
        /// <param name="RemindDays">提前X天</param>
        /// <returns></returns>
        int GetLeaveTourRemind(int CompanyId, string HaveUserIds, int RemindDays);
        /// <summary>
        /// 获取回团提醒数量
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="HaveUserIds">拥有的用户Id集合</param>
        /// <param name="RemindDays">提前X天</param>
        /// <returns></returns>
        int GetBackTourRemind(int CompanyId, string HaveUserIds, int RemindDays);
        /// <summary>
        /// 判断计划是否存在支出
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        bool GetIsExistsExpense(string tourId);
        /// <summary>
        /// 获取团款支出信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="isSettleOut">支出是否已结清</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.LBTKZCTourInfo> GetToursTKZC(int companyId, bool isSettleOut, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchTKZCInfo info, string us);
        /// <summary>
        /// 获取单项服务支出信息
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <param name="totalAmount">支出金额</param>
        /// <param name="unpaidAmount">未付金额</param>
        /// <param name="searchInfo">查询信息</param>
        void GetTourExpense(string tourId, out decimal totalAmount, out decimal unpaidAmount,EyouSoft.Model.PlanStructure.MExpendSearchInfo searchInfo);
        /// <summary>
        /// 获取计划地接社信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourLocalAgencyInfo> GetTourLocalAgencys(string tourId);
        /// <summary>
        /// 获取计划计调员信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourCoordinatorInfo> GetTourCoordinators(string tourId);
        /// <summary>
        /// 获取单项服务供应商安排的供应商编号信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        IList<int> GetSingleSuppliers(string tourId);       
        /// <summary>
        /// 获取团队计划及散拼计划供应商编号信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        IList<int> GetTourSuppliers(string tourId);
        /// <summary>
        /// 获取团队计划、散拼计划、单项服务客户单位信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        IList<int> GetTourCustomers(string tourId);
        /// <summary>
        /// 获取计划送团人信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        IList<Model.TourStructure.TourSentPeopleInfo> GetTourSentPeoples(string tourId);
        /// <summary>
        /// 设置计划导游信息集合及集合标志信息
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <param name="guides">导游信息集合</param>
        /// <param name="gatheringSign">集合标志</param>
        /// <returns></returns>
        bool SetTourGuides(string tourId, IList<string> guides, string gatheringSign);
        /*/// <summary>
        /// 根据用户编号获取其运送团的信息
        /// </summary>
        /// <param name="UserID">用户编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourSentTask> GetMySendTourInfo(int pageSize, int pageIndex, ref int recordCount, int[] UserID, EyouSoft.Model.TourStructure.TourSentTaskSearch TourSentTaskSearch);*/
        /// <summary>
        /// 根据用户编号获取其运送团的信息
        /// </summary>
        /// <param name="UserID">用户编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourSentTask> GetMySendTourInfo(int pageSize, int pageIndex, ref int recordCount, string UserIDs, EyouSoft.Model.TourStructure.TourSentTaskSearch TourSentTaskSearch);
        /*/// <summary>
        /// 根据用户编号获取其运送团的信息
        /// </summary>
        /// <param name="UserID">用户编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.TourSentTask> GetMySendTourInfo(int[] UserID);*/
        /// <summary>
        /// 获取计划导游信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        IList<string> GetTourGuides(string tourId);
        /// <summary>
        /// 获取个人中心地接安排计划信息集合
        /// </summary>
        /// <param name="companyId">公司(专线)编号</param>
        /// <param name="supplierId">公司(供应商地接)编号</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.LBDJAPTourInfo> GetToursDJAP(int companyId, int supplierId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info);
        /// <summary>
        /// 获取线路上团数汇总信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="routeId">线路编号</param>
        /// <returns></returns>
        EyouSoft.Model.TourStructure.MRouteSTSCollectInfo GetRouteSTSCollectInfo(int companyId, int routeId);
        /// <summary>
        /// 获取线路收客数汇总信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="routeId">线路编号</param>
        /// <returns></returns>
        EyouSoft.Model.TourStructure.MRouteSKSCollectInfo GetRouteSKSCollectInfo(int companyId, int routeId);
        /// <summary>
        /// 获取单项服务供应商安排信息业务实体
        /// </summary>
        /// <param name="planId">供应商安排编号</param>
        /// <returns></returns>
        EyouSoft.Model.TourStructure.PlanSingleInfo GetSinglePlanInfo(string planId);

        /// <summary>
        /// 获取待核算计划信息金额合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <param name="income">总收入</param>
        /// <param name="expenditure">总支出</param>
        /// <param name="payOff">总利润分配</param>
        /// <param name="adultNumber">总成人数</param>
        /// <param name="childNumber">总儿童人数</param>
        void GetToursNotAccounting(int companyId, Model.TourStructure.TourSearchInfo info, string us,
                                          ref decimal income, ref decimal expenditure, ref decimal payOff,
                                          ref int adultNumber, ref int childNumber);

        /// <summary>
        /// 获取待核算计划信息金额合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <param name="income">总收入</param>
        /// <param name="expenditure">总支出</param>
        /// <param name="payOff">总利润分配</param>
        /// <param name="adultNumber">总成人数</param>
        /// <param name="childNumber">总儿童人数</param>
        void GetToursAccounting(int companyId, Model.TourStructure.TourSearchInfo info, string us,
                                          ref decimal income, ref decimal expenditure, ref decimal payOff,
                                          ref int adultNumber, ref int childNumber);

        /// <summary>
        /// 获取团款支出信息集合金额合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="isSettleOut">支出是否已结清</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <param name="singleAmount">单项服务支出金额</param>
        /// <param name="singleHasAmount">单项服务支出已付金额</param>
        /// <param name="ticketAmount">机票支出金额</param>
        /// <param name="ticketHasAmount">机票支出已付金额</param>
        /// <param name="travelAgencyAmount">地接支出金额</param>
        /// <param name="travelAgencyHasAmount">地接支出已付金额</param>
        /// <returns></returns>
        void GetToursTKZC(int companyId, bool isSettleOut, Model.TourStructure.TourSearchTKZCInfo info, string us
                          , ref decimal travelAgencyAmount, ref decimal travelAgencyHasAmount
                          , ref decimal ticketAmount, ref decimal ticketHasAmount
                          , ref decimal singleAmount, ref decimal singleHasAmount);
        /// <summary>
        /// 获取计划类型
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        EyouSoft.Model.EnumType.TourStructure.TourType? GetTourType(string tourId);

        /// <summary>
        /// 根据团队Id获取团队计划人数和价格信息
        /// </summary>
        /// <param name="tourId">团队Id</param>
        /// <returns>团队计划人数和价格信息</returns>
        Model.TourStructure.MTourTeamUnitInfo GetTourTeamUnit(string tourId);

        /// <summary>
        /// 设置团队推广状态
        /// </summary>
        /// <param name="tourRouteStatus">团队推广状态</param>
        /// <param name="touId">团队Id</param>
        /// <returns>1:成功；其他失败</returns>
        int SetTourRouteStatus(Model.EnumType.TourStructure.TourRouteStatus tourRouteStatus, params string[] touId);

        /// <summary>
        /// 设置计划的手动设置状态
        /// </summary>
        /// <param name="handStatus">计划的手动设置状态</param>
        /// <param name="touId">团队Id</param>
        /// <returns>1:成功；其他失败</returns>
        int SetHandStatus(Model.EnumType.TourStructure.HandStatus handStatus, params string[] touId);

        /// <summary>
        /// 根据子系统公司编号与出团日期，删除计划数据
        /// </summary>
        /// <param name="SysID">子系统公司编号</param>
        /// <param name="LDateStart">出团日期开始</param>
        /// <param name="LDateEnd">出团日期结束</param>
        /// <returns>IList</returns>
        IList<string> ClearTour(int SysID, DateTime LDateStart, DateTime LDateEnd);

        /// <summary>
        /// 获取指定计划编号所属母团下子团信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="tourId">计划编号</param>
        /// <param name="sLDate">出团起始时间</param>
        /// <param name="eLDate">出团截止时间</param>
        /// <returns></returns>
        IList<EyouSoft.Model.TourStructure.MRiLiTourInfo> GetToursRiLi(int companyId, string tourId, DateTime? sLDate, DateTime? eLDate);
        /// <summary>
        /// 获取计划剩余人数
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        int GetShengYuRenShu(string tourId);
        /// <summary>
        /// 获取团队计划列表合计信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        void GetToursTeamHeJi(int companyId, EyouSoft.Model.TourStructure.TourSearchInfo info, string us, out int renShuHeJi, out decimal tuanKuanJinEHeJi);
    }
}
