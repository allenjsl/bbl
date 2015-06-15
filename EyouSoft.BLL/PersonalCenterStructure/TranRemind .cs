using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.PersonalCenterStructure
{
    /// <summary>
    /// 个人中心-事务提醒
    /// </summary>
    /// 鲁功源 2011-01-30
    public class TranRemind : BLLBase
    {
        private readonly EyouSoft.IDAL.AdminCenterStructure.IContractInfo Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.AdminCenterStructure.IContractInfo>();
        private readonly IDAL.TourStructure.ITour idalTour = Component.Factory.ComponentFactory.CreateDAL<IDAL.TourStructure.ITour>();
        private readonly IDAL.CompanyStructure.ICompanySupplier idalSupplier = Component.Factory.ComponentFactory.CreateDAL<IDAL.CompanyStructure.ICompanySupplier>();
        private readonly IDAL.CompanyStructure.ICustomer idalCustomer = Component.Factory.ComponentFactory.CreateDAL<IDAL.CompanyStructure.ICustomer>();
        private readonly IDAL.TourStructure.ITourOrder idalOrder = Component.Factory.ComponentFactory.CreateDAL<IDAL.TourStructure.ITourOrder>();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public TranRemind()
        {
            base.IsEnable = false;
        }

        /// <summary>
        /// 带组织机构构造函数
        /// </summary>
        /// <param name="uinfo">当前用户信息</param>
        public TranRemind(EyouSoft.SSOComponent.Entity.UserInfo uinfo)
        {
            if (uinfo != null)
            {
                base.DepartIds = uinfo.Departs;
                base.CompanyId = uinfo.CompanyID;
            }
        }

        #endregion

        /// <summary>
        /// 获取合同到期N天列表
        /// </summary>
        /// <param name="PageSize">每页显示数</param>
        /// <param name="PageIndex">起始页码</param>
        /// <param name="RecordCount">总数</param>
        /// <param name="remindDay">到期提醒天数</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.ContractReminder> GetContractRemind(int PageSize, int PageIndex, ref int RecordCount, int remindDay, int companyId)
        {
            return Dal.GetContractRemind(PageSize, PageIndex, ref RecordCount, remindDay, companyId);
        }
        /// <summary>
        /// 分页获取收款提醒
        /// </summary>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="receiptRemindType">收款提醒配置</param>
        /// <param name="sellerId">销售员编号 收款提醒配置成EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType.OnlySeller时不可为空</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.ReceiptRemind> GetReceiptRemind(int pageSize, int pageIndex, ref int recordCount, int CompanyId
            , EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType receiptRemindType, int? sellerId
            ,EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo searchInfo)
        {
            if (CompanyId <= 0) return null;
            if (receiptRemindType == EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType.OnlySeller && !sellerId.HasValue) return null;

            //结算日之前不提醒
            DateTime today = DateTime.Today;
            int accountDay = today.Day;
            if (DateTime.DaysInMonth(today.Year, today.Month) == today.Day)
            {
                accountDay = 31;
            }

            //结算星期几开始往后提醒4天
            int[] accountDayWeek = null;
            DayOfWeek dayOfWeek = today.DayOfWeek;

            switch (dayOfWeek)
            {
                case DayOfWeek.Monday: accountDayWeek = new int[] { 0, 5, 6, 7, 1 }; break;
                case DayOfWeek.Tuesday: accountDayWeek = new int[] { 0, 6, 7, 1, 2 }; break;
                case DayOfWeek.Wednesday: accountDayWeek = new int[] { 0, 7, 1, 2, 3 }; break;
                case DayOfWeek.Thursday: accountDayWeek = new int[] { 0, 1, 2, 3, 4 }; break;
                case DayOfWeek.Friday: accountDayWeek = new int[] { 0, 2, 3, 4, 5 }; break;
                case DayOfWeek.Saturday: accountDayWeek = new int[] { 0, 3, 4, 5, 6 }; break;
                case DayOfWeek.Sunday: accountDayWeek = new int[] { 0, 4, 5, 6, 7 }; break;
            }

            string sellers = string.Empty;
            if (receiptRemindType == EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType.OnlySeller && sellerId.HasValue)
            {
                sellers = sellerId.ToString();
            }

            if (!string.IsNullOrEmpty(sellers))
            {
                return idalCustomer.GetReceiptRemindByOrderSeller(pageSize, pageIndex, ref recordCount, CompanyId, accountDay, accountDayWeek, sellers, searchInfo);
            }

            return idalCustomer.GetReceiptRemind(pageSize, pageIndex, ref recordCount, CompanyId, accountDay, accountDayWeek, searchInfo);
        }

        /// <summary>
        /// 获取收款提醒合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="receiptRemindType">收款提醒配置</param>
        /// <param name="sellerId">销售员编号 收款提醒配置成EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType.OnlySeller时不可为空</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public void GetReceiptRemind(int companyId, EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType receiptRemindType, int? sellerId
            , EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo searchInfo, out decimal daiShouKuanHeJi)
        {
            daiShouKuanHeJi = 0;

            if (CompanyId <= 0) return;
            if (receiptRemindType == EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType.OnlySeller && !sellerId.HasValue) return;

            //结算日之前不提醒
            DateTime today = DateTime.Today;
            int accountDay = today.Day;
            if (DateTime.DaysInMonth(today.Year, today.Month) == today.Day)
            {
                accountDay = 31;
            }

            //结算星期几开始往后提醒4天
            int[] accountDayWeek = null;
            DayOfWeek dayOfWeek = today.DayOfWeek;

            switch (dayOfWeek)
            {
                case DayOfWeek.Monday: accountDayWeek = new int[] { 0, 5, 6, 7, 1 }; break;
                case DayOfWeek.Tuesday: accountDayWeek = new int[] { 0, 6, 7, 1, 2 }; break;
                case DayOfWeek.Wednesday: accountDayWeek = new int[] { 0, 7, 1, 2, 3 }; break;
                case DayOfWeek.Thursday: accountDayWeek = new int[] { 0, 1, 2, 3, 4 }; break;
                case DayOfWeek.Friday: accountDayWeek = new int[] { 0, 2, 3, 4, 5 }; break;
                case DayOfWeek.Saturday: accountDayWeek = new int[] { 0, 3, 4, 5, 6 }; break;
                case DayOfWeek.Sunday: accountDayWeek = new int[] { 0, 4, 5, 6, 7 }; break;
            }

            string sellers = string.Empty;
            if (receiptRemindType == EyouSoft.Model.EnumType.CompanyStructure.ReceiptRemindType.OnlySeller && sellerId.HasValue)
            {
                sellers = sellerId.ToString();
            }

            if (!string.IsNullOrEmpty(sellers))
            {
                idalCustomer.GetReceiptRemindByOrderSeller(CompanyId, accountDay, accountDayWeek, sellers, searchInfo,out daiShouKuanHeJi);
                return;
            }

            idalCustomer.GetReceiptRemind(CompanyId, accountDay, accountDayWeek, searchInfo,out daiShouKuanHeJi);
        }

        /*/// <summary>
        /// 获取收款提醒数量
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public int GetReceiptRemind(int CompanyId)
        {
            if (CompanyId <= 0)
                return 0;

            return idalCustomer.GetReceiptRemind(CompanyId);
        }*/

        /// <summary>
        /// 分页获取付款提醒
        /// </summary>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="searchInfo">查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.PayRemind> GetPayRemind(int pageSize, int pageIndex, ref int recordCount, int CompanyId, EyouSoft.Model.PersonalCenterStructure.FuKuanTiXingChaXun searchInfo)
        {
            if (CompanyId <= 0)
                return null;

            return idalSupplier.GetPayRemind(pageSize, pageIndex, ref recordCount, CompanyId, searchInfo);
        }

        /// <summary>
        /// 获取付款提醒未付款合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询实体</param>
        /// <param name="weiFuHeJi">未付款合计</param>
        /// <returns></returns>
        public void GetPayRemind(int companyId, EyouSoft.Model.PersonalCenterStructure.FuKuanTiXingChaXun searchInfo, out decimal weiFuHeJi)
        {
            idalSupplier.GetPayRemind(companyId, searchInfo, out weiFuHeJi);
        }

        /// <summary>
        /// 获取付款提醒数量
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public int GetPayRemind(int CompanyId)
        {
            if (CompanyId <= 0)
                return 0;

            return idalSupplier.GetPayRemind(CompanyId);
        }

        /// <summary>
        /// 分页获取出团提醒
        /// </summary>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="RemindDays">提前X天</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.LeaveTourRemind> GetLeaveTourRemind(int pageSize, int pageIndex, ref int recordCount, int CompanyId, int RemindDays)
        {
            if (CompanyId <= 0 || RemindDays <= 0)
                return null;

            return idalTour.GetLeaveTourRemind(pageSize, pageIndex, ref recordCount, CompanyId, base.HaveUserIds, RemindDays);
        }

        /// <summary>
        /// 获取出团提醒数量
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="RemindDays">提前X天</param>
        /// <returns></returns>
        public int GetLeaveTourRemind(int CompanyId, int RemindDays)
        {
            if (CompanyId <= 0 || RemindDays <= 0)
                return 0;

            return idalTour.GetLeaveTourRemind(CompanyId, base.HaveUserIds, RemindDays);
        }

        /// <summary>
        /// 分页获取回团提醒
        /// </summary>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="RemindDays">提前X天</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.BackTourRemind> GetBackTourRemind(int pageSize, int pageIndex, ref int recordCount, int CompanyId, int RemindDays)
        {
            if (CompanyId <= 0 || RemindDays <= 0)
                return null;

            return idalTour.GetBackTourRemind(pageSize, pageIndex, ref recordCount, CompanyId, base.HaveUserIds, RemindDays);
        }

        /// <summary>
        /// 获取回团提醒数量
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="RemindDays">提前X天</param>
        /// <returns></returns>
        public int GetBackTourRemind(int CompanyId, int RemindDays)
        {
            if (CompanyId <= 0 || RemindDays <= 0)
                return 0;

            return idalTour.GetBackTourRemind(CompanyId, base.HaveUserIds, RemindDays);
        }

        /// <summary>
        /// 获取合同到期总人数
        /// </summary>
        /// <param name="remindDay">到期天数</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public int GetTotalContractRemind(int remindDay, int companyId)
        {
            return Dal.GetTotalContractRemind(remindDay, companyId);
        }
        /// <summary>
        /// 根据组团公司编号获取已留位订单数
        /// </summary>
        /// <param name="BuyCompanyId">组团公司编号</param>
        /// <returns></returns>
        public int GetTotalOrderCountByBuyCompanyId(int BuyCompanyId)
        {
            return idalOrder.GetOrderCountByBuyCompanyId(BuyCompanyId, EyouSoft.Model.EnumType.TourStructure.OrderState.已留位);
        }

        /// <summary>
        /// 获取专线未处理的订单数量
        /// </summary>
        /// <param name="companyId">专线公司Id</param>
        /// <returns>未处理的订单数量</returns>
        public int GetNoHandleOrderCountByCompanyId(int companyId)
        {
            return idalOrder.GetOrderCountByCompanyId(companyId, Model.EnumType.TourStructure.OrderState.未处理);
        }

        /*/// <summary>
        /// 获取专线未处理的订单列表
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">专线公司Id</param>
        /// <returns></returns>
        public IList<Model.TourStructure.TourOrder> GetNoHandleOrderCountByCompanyId(int pageSize, int pageIndex
            , ref int recordCount, int companyId)
        {
            var dAlSearchModel = new Model.TourStructure.SearchInfoForDAL
            {
                BuyCompanyId = null,
                SellCompanyId = CompanyId,
                OrderState = new[] { Model.EnumType.TourStructure.OrderState.未处理 },
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

            return idalOrder.GetOrderList(pageSize, pageIndex, ref recordCount, dAlSearchModel);
        }   */
    }
}
