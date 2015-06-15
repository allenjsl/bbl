using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.Model.PlanStructure;

namespace EyouSoft.BLL.PlanStruture
{
    /// <summary>
    /// 地接社相关操作方法
    /// autor:李焕超 datetime:2011-1-19
    /// </summary>
    public class TravelAgency : BLLBase
    {
        EyouSoft.IDAL.PlanStruture.ITravelAgency dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.PlanStruture.ITravelAgency>();

        #region constructor
        /// <summary>
        /// 构造函数
        /// </summary>
        public TravelAgency()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userModel">当前登录用户信息</param>
        public TravelAgency(EyouSoft.SSOComponent.Entity.UserInfo userModel)
        {
            if (userModel != null)
            {
                base.DepartIds = userModel.Departs;
                base.CompanyId = userModel.CompanyID;
            }
        }
        #endregion

        #region private members
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
        #endregion

        #region public members
        /// <summary>
        /// 安排地接
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddTravel(LocalTravelAgencyInfo model)
        {

            string id = Guid.NewGuid().ToString();
            model.ID = id;
            model.TotalAmount = model.Settlement;
            if (dal.RanguageTravel(model))
            {   
                //添加支出明细
                AddStatAllOut(model);

                //重新计算团队支出
                EyouSoft.BLL.UtilityStructure.Utility u = new EyouSoft.BLL.UtilityStructure.Utility();
                IList<EyouSoft.Model.StatisticStructure.ItemIdAndType> iList = new List<EyouSoft.Model.StatisticStructure.ItemIdAndType>();
                iList.Add(new EyouSoft.Model.StatisticStructure.ItemIdAndType() { ItemId = model.ID, ItemType = EyouSoft.Model.EnumType.StatisticStructure.PaidType.地接支出 });
                //价格维护
                u.CalculationTourOut(model.TourId, iList);

                //加日志
                //AddSysLog("新增");
                
                //维护地接社交易数量
                if (model.TravelAgencyID > 0)
                {
                    u.ServerTradeCount(model.TravelAgencyID);
                }

                u.CalculationTourSettleStatus(model.TourId);

                #region LGWR
                EyouSoft.Model.EnumType.TourStructure.TourType? tourType = new EyouSoft.BLL.TourStructure.Tour().GetTourType(model.TourId);
                Model.EnumType.CompanyStructure.SysPermissionClass mokuai = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散拼计划;
                if (tourType != null && tourType == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
                {
                    mokuai = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_团队计划;
                }

                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + mokuai.ToString() + "新增了地接安排，安排编号：" + model.ID + "，计划编号为：" + model.TourId;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "新增地接安排";
                logInfo.ModuleId = mokuai;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                #endregion

                return true;
            }

            return false;
        }

        /// <summary>
        /// 返回地接
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public LocalTravelAgencyInfo GetTravelModel(string ID)
        {
            return dal.GetTravelModel(ID);
        }

        /// <summary>
        /// 修改地接
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public bool UpdateTravelAgency(EyouSoft.Model.PlanStructure.LocalTravelAgencyInfo Model)
        {
            if (dal.UpdateTravelModel(Model))
            {
                //重新计算团队支出
                EyouSoft.BLL.UtilityStructure.Utility u = new EyouSoft.BLL.UtilityStructure.Utility();
                IList<EyouSoft.Model.StatisticStructure.ItemIdAndType> iList = new List<EyouSoft.Model.StatisticStructure.ItemIdAndType>();
                iList.Add(new EyouSoft.Model.StatisticStructure.ItemIdAndType() { ItemId = Model.ID, ItemType = EyouSoft.Model.EnumType.StatisticStructure.PaidType.地接支出 });
                //价格维护
                u.CalculationTourOut(Model.TourId, iList);
                //加日志
                //AddSysLog("修改");

                //维护地接社交易数量
                if (Model.TravelAgencyID > 0)
                {
                    EyouSoft.Model.PlanStructure.LocalTravelAgencyInfo tempModel = GetTravelModel(Model.ID);
                    if (tempModel != null)
                        u.ServerTradeCount(new int[] { Model.TravelAgencyID, tempModel.TravelAgencyID });
                }

                u.CalculationTourSettleStatus(Model.TourId);

                #region LGWR
                EyouSoft.Model.EnumType.TourStructure.TourType? tourType = new EyouSoft.BLL.TourStructure.Tour().GetTourType(Model.TourId);
                Model.EnumType.CompanyStructure.SysPermissionClass mokuai = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散拼计划;
                if (tourType != null && tourType == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
                {
                    mokuai = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_团队计划;
                }

                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + mokuai.ToString() + "修改了地接安排，安排编号：" + Model.ID + "，计划编号为：" + Model.TourId;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "修改地接安排";
                logInfo.ModuleId = mokuai;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                #endregion

                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除地接
        /// </summary>
        /// <param name="TravelId">安排地接编号</param>
        /// <returns></returns>
        public bool DelTravelAgency(string TravelId)
        {
            EyouSoft.Model.PlanStructure.LocalTravelAgencyInfo modelTravel = GetTravelModel(TravelId);
            if (dal.DeletTravelModel(TravelId))
            //加日志
            //AddSysLog("删除");
            //维护地接社交易数量
            if (modelTravel != null)
            {
                EyouSoft.BLL.CompanyStructure.CompanySupplier bllSuplier = new EyouSoft.BLL.CompanyStructure.CompanySupplier();// EyouSoft.BLL.CompanyStructure.CompanySupplier();
                EyouSoft.Model.CompanyStructure.CompanySupplier Model = bllSuplier.GetModel(modelTravel.TravelAgencyID, modelTravel.CompanyId);

                if (Model != null && Model.Id > 0)
                {
                    EyouSoft.BLL.UtilityStructure.Utility idal = new EyouSoft.BLL.UtilityStructure.Utility();
                    idal.ServerTradeCount(new int[] { Model.Id });
                }

                //重新计算团队支出
                EyouSoft.BLL.UtilityStructure.Utility u = new EyouSoft.BLL.UtilityStructure.Utility();
                //价格维护
                u.CalculationTourOut(modelTravel.TourId, null);

                #region LGWR
                EyouSoft.Model.EnumType.TourStructure.TourType? tourType = new EyouSoft.BLL.TourStructure.Tour().GetTourType(modelTravel.TourId);
                Model.EnumType.CompanyStructure.SysPermissionClass mokuai = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散拼计划;
                if (tourType != null && tourType == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
                {
                    mokuai = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_团队计划;
                }

                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + mokuai.ToString() + "删除了地接安排，安排编号：" + modelTravel.ID + "，计划编号为：" + modelTravel.TourId;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "删除地接安排";
                logInfo.ModuleId = mokuai;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                #endregion

                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取某个业务员支出统计列表
        /// </summary>
        /// <param name="SalerId"></param>
        /// <param name="Date"></param>
        /// <returns></returns>
        public IList<PersonalStatics> GetStaticsList(EyouSoft.Model.StatisticStructure.QueryPersonnelStatistic SearchModel)
        {
            return dal.GetStaticsList(SearchModel, HaveUserIds);
        }

        /// <summary>
        /// 获取地接列表
        /// </summary>
        /// <param name="CompanyId">团队编号</param>
        /// <returns></returns>
        public IList<LocalTravelAgencyInfo> GetTravelList(int CompanyId)
        { return dal.GetTravelList(CompanyId); }

        /// <summary>
        /// 获取某团地接列表
        /// </summary>
        /// <param name="TravelId">团队编号</param>
        /// <returns></returns>
        public IList<LocalTravelAgencyInfo> GetList(string TourId)
        { return dal.GetList(TourId); }

        /// <summary>
        /// 根据团号列出机票地接社支出信息
        /// </summary>
        /// <param name="TourId">团队编号</param>
        /// <returns></returns>
        public IList<PaymentList> GetSettleList(string TourId)
        {
            return GetSettleList(TourId, null);

        }

        /// <summary>
        /// 团款支出-应付账款-支付项目-明细
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<PaymentList> GetSettleList(string tourId, MExpendSearchInfo searchInfo)
        {
            return dal.GetSettleList(tourId, searchInfo);
        }

        /*/// <summary>
        /// 修改机票地接社支出金额
        /// </summary>
        /// <param name="TravelId">团队编号</param>
        /// <returns></returns>
        public bool UpdateSettle(PaymentList Model)
        {
            if (dal.UpdateSettle(Model))
            { //加日志
                AddSysLog("修改机票地接社支出金额");
                return true;
            }
            return false;
        }*/

        /// <summary>
        /// 修改机票地接社支出金额
        /// </summary>
        /// <param name="ModelList">更改实体</param>
        /// <param name="TourId">团队编号</param>
        /// <returns></returns>
        public bool UpdateSettle(IList<PaymentList> ModelList, string TourId)
        {
            foreach (PaymentList model in ModelList)
            {
                dal.UpdateSettle(model);
            }
            //加日志
            //AddSysLog("修改机票地接社支出金额");
            EyouSoft.BLL.UtilityStructure.Utility u = new EyouSoft.BLL.UtilityStructure.Utility();
            u.CalculationTourOut(TourId, null);
            u.CalculationTourSettleStatus(TourId);

            EyouSoft.Model.EnumType.TourStructure.TourType? tourType = new EyouSoft.BLL.TourStructure.Tour().GetTourType(TourId);
            Model.EnumType.CompanyStructure.SysPermissionClass mokuai = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散拼计划;
            if (tourType != null && tourType == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
            {
                mokuai = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_团队计划;
            }

            foreach (var item in ModelList)
            {
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + mokuai.ToString() + "团队核算修改了" + item.SupplierType.ToString() + "支出金额，支出项目编号为：" + item.Id + "，计划编号为：" + TourId;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "团队核算修改支出金额";
                logInfo.ModuleId = mokuai;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
            }

            return true;
        }
        /// <summary>
        /// 添加支出明细
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private bool AddStatAllOut(LocalTravelAgencyInfo model)
        {
            EyouSoft.Model.EnumType.TourStructure.TourType? tourType = new EyouSoft.BLL.TourStructure.Tour().GetTourType(model.TourId);

            EyouSoft.BLL.StatisticStructure.StatAllOut statics = new EyouSoft.BLL.StatisticStructure.StatAllOut();
            EyouSoft.Model.StatisticStructure.StatAllOut staModel = new EyouSoft.Model.StatisticStructure.StatAllOut();
            staModel.AddAmount = model.AddAmount;
            staModel.Amount = 0;
            staModel.CheckAmount = 0;
            staModel.CompanyId = model.CompanyId;
            staModel.CreateTime = System.DateTime.Now;
            staModel.ItemId = model.ID;
            staModel.ItemType = EyouSoft.Model.EnumType.StatisticStructure.PaidType.地接支出;
            staModel.NotCheckAmount = 0;
            staModel.OperatorId = model.OperatorID;
            staModel.TotalAmount = model.Settlement;
            staModel.TourId = model.TourId;
            staModel.SupplierName = model.LocalTravelAgency;
            staModel.SupplierId = model.TravelAgencyID;
            staModel.TourType = tourType != null ? tourType.Value : EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划;

            return statics.Add(staModel);
        }

        /*
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private bool AddSysLog(string type)
        {
            EyouSoft.BLL.CompanyStructure.SysHandleLogs sysLong = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();
            EyouSoft.Model.CompanyStructure.SysHandleLogs sysModel = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            sysModel.EventMessage = System.DateTime.Now.ToString() + "{0}在安排地接" + type + "了数据";
            sysModel.EventTitle = type + " 安排地接 数据";
            sysModel.ID = Guid.NewGuid().ToString();
            sysModel.EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode;
            sysModel.ModuleId = Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_机票管理;

            return sysLong.Add(sysModel);
        } */
        
        /// <summary>
        /// 获取付款提醒查看明细信息集合
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="ServerId"></param>
        /// <param name="Type">1代表地接，2代表票务</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<ArrearInfo> GetFuKuanTiXingMingXi(int CompanyId, int ServerId, int ServerType, int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.PersonalCenterStructure.FuKuanTiXingChaXun searchInfo)
        {
            return dal.GetFuKuanTiXingMingXi(CompanyId, ServerId, ServerType, PageSize, PageIndex, ref  RecordCount, searchInfo);
        }

        /// <summary>
        /// 获取付款提醒查看明细信息汇总
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="supplierId">供应商编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="weiFuHeJi">未付金额合计</param>
        /// <returns></returns>
        public void GetFuKuanTiXingMingXi(int companyId, int supplierId, EyouSoft.Model.PersonalCenterStructure.FuKuanTiXingChaXun searchInfo, out decimal weiFuHeJi)
        {
            dal.GetFuKuanTiXingMingXi(companyId, supplierId, searchInfo, out weiFuHeJi);
        }


        /// <summary>
        /// 统计中心-获取欠款列表（总支出；已付；未付）(统计分析-支出对账单总支出、已付、未付明细列表)
        /// </summary>
        /// <param name="SearchModel"></param>
        /// <returns></returns>
        public IList<ArrearInfo> GetArrear(EyouSoft.Model.PlanStructure.ArrearSearchInfo SearchModel)
        {
            if (SearchModel != null)
            {
                SearchModel = new ArrearSearchInfo()
                {
                    AreaId = SearchModel.AreaId,
                    CompanyId = SearchModel.CompanyId,
                    IsAccount = SearchModel.IsAccount,
                    LeaveDate1 = SearchModel.LeaveDate1,
                    LeaveDate2 = SearchModel.LeaveDate2,
                    OperateId = SearchModel.OperateId,
                    SalerId = SearchModel.SalerId,
                    SeachType = SearchModel.SeachType,
                    SupplierId = SearchModel.SupplierId,
                    SupplierName = SearchModel.SupplierName,
                    TourType = SearchModel.TourType
                };

                if (!SearchModel.LeaveDate2.HasValue)
                {
                    SearchModel.LeaveDate2 = new DateTime(DateTime.Now.Year + 1, 1, 1).AddMilliseconds(-1);
                }
                else
                {
                    SearchModel.LeaveDate2 = SearchModel.LeaveDate2.Value.AddDays(1).AddMilliseconds(-1);
                }

                if (!SearchModel.LeaveDate1.HasValue)
                {
                    SearchModel.LeaveDate1 = new DateTime(DateTime.Now.Year, 1, 1);
                }
            }

            return dal.GetArrear(SearchModel, HaveUserIds);
        }

        /// <summary>
        /// 统计中心-获取欠款金额汇总信息(统计分析-支出对账单总支出、已付、未付明细列表合计)
        /// </summary>
        /// <param name="searchModel">查询实体</param>
        /// <param name="totalAmount">总支出</param>
        /// <param name="arrear">未付</param>
        /// <returns></returns>
        public void GetArrearAllMoeny(ArrearSearchInfo searchModel, ref decimal totalAmount, ref decimal arrear)
        {
            if (searchModel == null)
                return;

            searchModel = new ArrearSearchInfo()
            {
                AreaId = searchModel.AreaId,
                CompanyId = searchModel.CompanyId,
                IsAccount = searchModel.IsAccount,
                LeaveDate1 = searchModel.LeaveDate1,
                LeaveDate2 = searchModel.LeaveDate2,
                OperateId = searchModel.OperateId,
                SalerId = searchModel.SalerId,
                SeachType = searchModel.SeachType,
                SupplierId = searchModel.SupplierId,
                SupplierName = searchModel.SupplierName,
                TourType = searchModel.TourType                
            };

            if (!searchModel.LeaveDate2.HasValue)
            {
                searchModel.LeaveDate2 = new DateTime(DateTime.Now.Year + 1, 1, 1).AddMilliseconds(-1);
            }
            else
            {
                searchModel.LeaveDate2 = searchModel.LeaveDate2.Value.AddDays(1).AddMilliseconds(-1);
            }

            if (!searchModel.LeaveDate1.HasValue)
            {
                searchModel.LeaveDate1 = new DateTime(DateTime.Now.Year, 1, 1);
            }

            dal.GetArrearAllMoeny(searchModel, HaveUserIds, ref totalAmount, ref arrear);
        }
        #endregion

    }
}
