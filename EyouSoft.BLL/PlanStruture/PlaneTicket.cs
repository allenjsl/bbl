using System;
using System.Collections.Generic;
using System.Text;
using EyouSoft.Model.PlanStructure;

namespace EyouSoft.BLL.PlanStruture
{
    /// <summary>
    /// 机票相关操作方法
    /// autor:李焕超 datetime:2011-1-19
    /// </summary>
    public class PlaneTicket : EyouSoft.BLL.BLLBase
    {
        EyouSoft.IDAL.PlanStruture.IPlaneTicket dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.PlanStruture.IPlaneTicket>();

        #region constructor
        /// <summary>
        /// 普通构造函数
        /// </summary>
        public PlaneTicket()
        {

        }

        /// <summary>
        /// 组织机构控制构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public PlaneTicket(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }
        #endregion

        #region public members
        /// <summary>
        /// 申请机票
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool addTicketOutListModel(TicketOutListInfo model)
        {
            if (string.IsNullOrEmpty(model.TourId))
                return false;
            model.State = EyouSoft.Model.EnumType.PlanStructure.TicketState.机票申请;
            model.TicketOutId = Guid.NewGuid().ToString();
            EyouSoft.BLL.TourStructure.Tour bllTour = new EyouSoft.BLL.TourStructure.Tour();
            EyouSoft.Model.TourStructure.TourBaseInfo modelTour = bllTour.GetTourInfo(model.TourId);
            if (modelTour != null)
                model.Saler = modelTour.SellerName;
            if (dal.addTicketOutListModel(model))
            {

                //重新计算团队支出
                EyouSoft.BLL.UtilityStructure.Utility u = new EyouSoft.BLL.UtilityStructure.Utility();
                IList<EyouSoft.Model.StatisticStructure.ItemIdAndType> iList = new List<EyouSoft.Model.StatisticStructure.ItemIdAndType>();
                iList.Add(new EyouSoft.Model.StatisticStructure.ItemIdAndType() { ItemId = model.TicketOutId, ItemType = EyouSoft.Model.EnumType.StatisticStructure.PaidType.机票支出 });
                //价格维护
                u.CalculationTourOut(model.TourId, iList);
                u.CalculationTourSettleStatus(model.TourId);

                //加日志
                AddSysLog("新增");
                //维护团队状态
                EyouSoft.BLL.TourStructure.Tour tourDal = new EyouSoft.BLL.TourStructure.Tour();
                tourDal.SetTourTicketStatus(model.TourId, model.State);
                return true;

            }
            return false;
        }

        /// <summary>
        /// 获取申请机票
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        public TicketOutListInfo GetTicketOutListModel(string TicketOutId)
        {
            return dal.GetTicketModel(TicketOutId);
        }

        /// <summary>
        /// 返回退订机票实体
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.CustomerRefund GetRefundTicketModel(int TicketId)
        {
            string refundId = dal.GetRefundTicketModel(TicketId);
            if (!string.IsNullOrEmpty(refundId))
            {
                EyouSoft.BLL.TourStructure.TourOrder bll = new EyouSoft.BLL.TourStructure.TourOrder();
                EyouSoft.Model.TourStructure.CustomerRefund model = new EyouSoft.Model.TourStructure.CustomerRefund();
                model = bll.GetCustomerRefund(refundId);
                return model;
            }
            else
            { return null; }

        }

        /// <summary>
        /// 修改机票申请
        /// </summary>
        /// <param name="TicketModel"></param>
        /// <returns></returns>
        public bool UpdateTicketOutListModel(TicketOutListInfo TicketModel)
        {
            if (TicketModel.State == EyouSoft.Model.EnumType.PlanStructure.TicketState.None) return false;

            //TicketModel.State = EyouSoft.Model.EnumType.PlanStructure.TicketState.机票申请;
            string registerId;
            if (dal.ToTicketOut(TicketModel, out registerId))
            {
                EyouSoft.BLL.UtilityStructure.Utility u = new EyouSoft.BLL.UtilityStructure.Utility();
                IList<EyouSoft.Model.StatisticStructure.ItemIdAndType> iList = new List<EyouSoft.Model.StatisticStructure.ItemIdAndType>();
                iList.Add(new EyouSoft.Model.StatisticStructure.ItemIdAndType() { ItemId = TicketModel.TicketOutId, ItemType = EyouSoft.Model.EnumType.StatisticStructure.PaidType.机票支出 });
                //价格维护
                u.CalculationTourOut(TicketModel.TourId, iList);
                u.CalculationTourSettleStatus(TicketModel.TourId);
                //日志
                AddSysLog("修改");

                //维护团队状态
                EyouSoft.BLL.TourStructure.Tour tourDal = new EyouSoft.BLL.TourStructure.Tour();
                tourDal.SetTourTicketStatus(TicketModel.TourId, TicketModel.State);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 出票
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        public bool ToTicketOut(TicketOutListInfo TicketModel)
        {
            if (TicketModel == null || TicketModel.TicketOfficeId <= 0)
                return false;

            TicketModel.State = EyouSoft.Model.EnumType.PlanStructure.TicketState.已出票;
            TicketModel.TicketOutTime = DateTime.Now;
            string registerId;
            if (dal.ToTicketOut(TicketModel, out registerId))
            {
                if (TicketModel.TicketOfficeId > 0)
                {
                    //维护供应商交易次数
                    IDAL.UtilityStructure.IUtility idal = Component.Factory.ComponentFactory.CreateDAL<IDAL.UtilityStructure.IUtility>();
                    idal.ServerTradeCount(TicketModel.TicketOfficeId);

                    //维护团队状态
                    EyouSoft.BLL.TourStructure.Tour tourDal = new EyouSoft.BLL.TourStructure.Tour();
                    tourDal.SetTourTicketStatus(TicketModel.TourId, TicketModel.State);

                    //价格维护 支出明细已在存储过程中完成
                    EyouSoft.BLL.UtilityStructure.Utility u = new EyouSoft.BLL.UtilityStructure.Utility();
                    IList<EyouSoft.Model.StatisticStructure.ItemIdAndType> iList = new List<EyouSoft.Model.StatisticStructure.ItemIdAndType>();
                    iList.Add(new EyouSoft.Model.StatisticStructure.ItemIdAndType() { ItemId = TicketModel.TicketOutId, ItemType = EyouSoft.Model.EnumType.StatisticStructure.PaidType.机票支出 });
                    u.CalculationTourOut(TicketModel.TourId, iList);

                    if (!string.IsNullOrEmpty(registerId))
                    {
                        u.CalculationCheckedOut(registerId);
                    }

                    u.CalculationTourSettleStatus(TicketModel.TourId);

                    //日志
                    AddSysLog("出票");
                }
                return true;
            }
            return false;
        }

        /*/// <summary>
        /// 退票
        /// </summary>
        /// <param name="TicketId"></param>
        /// <param name="State"></param>
        /// <param name="ReturnMoney"></param>
        /// <returns></returns>
        public bool ReturnTicketOut(EyouSoft.Model.PlanStructure.TicketRefundModel model)
        {
            if (dal.ReturnTicketOut(model))
            {
                //日志
                AddSysLog("退票");
                //维护供应商交易次数
                EyouSoft.Model.PlanStructure.TicketOutListInfo ticketModel = new TicketOutListInfo();
                ticketModel = GetTicketModel(GetTicketOutId(int.Parse(model.TicketId)));
                if (ticketModel != null)
                {
                    //维护供应商交易次数
                    IDAL.UtilityStructure.IUtility idal = Component.Factory.ComponentFactory.CreateDAL<IDAL.UtilityStructure.IUtility>();
                    idal.ServerTradeCount(ticketModel.TicketOfficeId);

                    //维护团队机票状态
                    EyouSoft.BLL.TourStructure.Tour tourDal = new EyouSoft.BLL.TourStructure.Tour();
                    tourDal.SetTourTicketStatus(ticketModel.TourId, model.State);

                    //维护团队收入、杂费收入
                    if (model.State == EyouSoft.Model.EnumType.PlanStructure.TicketState.已退票)
                    {
                        new BLL.UtilityStructure.Utility().CalculationTourIncome(ticketModel.TourId, null);
                    }
                }
                return true;
            }
            return false;
        }*/

        /// <summary>
        /// 通过TicketId获取ticketOutId
        /// </summary>
        /// <param name="TicketId">TicketId</param>
        /// <returns></returns>
        public string GetTicketOutId(int TicketId)
        {
            return dal.GetTicketOutId(TicketId);
        }

        /// <summary>
        /// 返回机票实体
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        public TicketOutListInfo GetTicketModel(string TicketId)
        {
            return dal.GetTicketModel(TicketId);
        }

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
        /// <returns></returns>
        public IList<EyouSoft.Model.PlanStructure.TicketInfo> GetTicketList(int PageSize, int PageIndex, int CompanyID, ref int RecordCount, ref int PageCount)
        {
            return dal.GetTicketList(PageSize, PageIndex, CompanyID, ref RecordCount, ref PageCount, null);
        }

        /// <summary>
        /// 财务管理机票审核列表
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyID"></param>
        /// <param name="strWhere"></param>
        /// <param name="filedOrder"></param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PlanStructure.TicketInfo> GetCheckedTicketList(int PageSize, int PageIndex, int CompanyID, ref int RecordCount, ref int PageCount)
        {
            return dal.GetCheckedTicketList(PageSize, PageIndex, CompanyID, ref RecordCount, ref PageCount, this.HaveUserIds);
        }

        /*
        /// <summary>
        /// 散拼计划-申请机票列表 --如果不输入CompanyId请输入 “空”
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyID"></param>
        /// <param name="strWhere"></param>
        /// <param name="filedOrder"></param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PlanStructure.TicketListModel> GetTicketList(int CompanyID, string TourId)
        { return dal.GetTicketList(CompanyID, TourId); }
        */

        /*/// <summary>
        /// 机票统计函数
        /// </summary>
        /// <param name="SearchModel"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PlanStructure.TicketStatisticsModel> GetTicketCount(TicketStatisticsSearchModel SearchModel)
        {

            if (SearchModel.DepartmentId != null && SearchModel.DepartmentId.Length > 0)
            {
                int[] items = GetDepartmentUsers(SearchModel.DepartmentId, SearchModel.CompanyId);
                if (SearchModel.OperatorId == null || SearchModel.OperatorId.Length <= 0)
                {
                    int[] temp = new int[items.Length];
                    int i = 0;
                    foreach (int item in items)
                    {
                        temp[i++] = item;
                    }
                    SearchModel.OperatorId = temp;
                }
            }

            return dal.GetTicketCount(SearchModel, this.HaveUserIds);
        }*/

        /// <summary>
        /// 财务审核机票
        /// </summary>
        /// <param name="TicketId"></param>
        ///  <param name="ReviewRemark">账务审核备注</param>
        /// <returns></returns>
        public bool CheckTicket(string TicketId, string ReviewRemark)
        {
            if (dal.CheckTicket(TicketId, ReviewRemark))
            {
                //日志
                AddSysLog("审核");
                //维护团队机票状态
                TicketOutListInfo model = GetTicketModel(TicketId);
                if (model != null)
                {
                    EyouSoft.BLL.TourStructure.Tour tourDal = new EyouSoft.BLL.TourStructure.Tour();
                    tourDal.SetTourTicketStatus(model.TourId, model.State);
                }
                return true;
            }
            return false;

        }

        /// <summary>
        /// 机票搜索
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="SearchModel"></param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        public IList<TicketInfo> SearchTicketOut(int PageSize, int PageIndex, EyouSoft.Model.PlanStructure.TicketSearchModel SearchModel, ref int RecordCount, ref int PageCount)
        {
            return dal.SearchTicketOut(PageSize, PageIndex, SearchModel, ref RecordCount, ref PageCount, null);
        }

        /// <summary>
        /// 已订机票游客
        /// </summary>
        /// <param name="TourId"></param>
        /// <returns></returns>
        public IList<string> CustomerList(string TourId)
        {
            return dal.CustomerList(TourId);
        }

        /// <summary>
        /// 已出票的操作员名字
        /// </summary>
        /// <param name="TourId"></param>
        /// <returns></returns>
        public IList<string> CustomerOperatorList(string TourId)
        {
            return dal.CustomerOperatorList(TourId);
        }

        /// <summary>
        /// 删除一次申请机票
        /// </summary>
        /// <param name="TicketOutId"></param>
        /// <returns></returns>
        public int DeleteTicket(string TicketOutId)
        {
            TicketOutListInfo model = GetTicketModel(TicketOutId);

            if (dal.DeleteTicket(TicketOutId) > 0)
            {
                //日志
                AddSysLog("删除");
                //维护团队机票状态

                if (model != null)
                {
                    EyouSoft.BLL.TourStructure.Tour tourDal = new EyouSoft.BLL.TourStructure.Tour();
                    tourDal.SetTourTicketStatus(model.TourId, model.State);

                    //价格维护
                    EyouSoft.BLL.UtilityStructure.Utility u = new EyouSoft.BLL.UtilityStructure.Utility();
                    IList<EyouSoft.Model.StatisticStructure.ItemIdAndType> iList = new List<EyouSoft.Model.StatisticStructure.ItemIdAndType>();
                    iList.Add(new EyouSoft.Model.StatisticStructure.ItemIdAndType() { ItemId = model.TicketOutId, ItemType = EyouSoft.Model.EnumType.StatisticStructure.PaidType.机票支出 });
                    u.CalculationTourOut(model.TourId, iList);

                    //维护供应商交易次数
                    IDAL.UtilityStructure.IUtility idal = Component.Factory.ComponentFactory.CreateDAL<IDAL.UtilityStructure.IUtility>();
                    idal.ServerTradeCount(model.TicketOfficeId);
                }
                return 1;

            }
            return 0;
        }

        /// <summary>
        /// 获取出票统计出票量明细
        /// </summary>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="model">查询实体</param>
        /// <returns></returns>
        public IList<Model.PlanStructure.TicketOutStatisticInfo> GetTicketOutStatisticList(int PageSize, int PageIndex
            , ref int RecordCount, Model.StatisticStructure.QueryTicketOutStatisti model)
        {
            if (model == null)
                return null;

            return dal.GetTicketOutStatisticList(PageSize, PageIndex, ref RecordCount, model, null);
        }

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
        public int UNCheckTicket(string TicketId)
        {
            TicketOutListInfo ticketInfo = GetTicketModel(TicketId);

            if (string.IsNullOrEmpty(TicketId)) return 0;

            int iRow = dal.UNCheckTicket(TicketId);
            if (iRow == 1)
            {
                if (ticketInfo != null)
                {
                    new EyouSoft.BLL.TourStructure.Tour().SetTourTicketStatus(ticketInfo.TourId, EyouSoft.Model.EnumType.PlanStructure.TicketState.None);
                }

                AddSysLog("取消审核");
            }

            return iRow;
        }

        /// <summary>
        /// 获取计划机票申请信息集合
        /// </summary>
        /// <param name="companyId">公司(专线)编号</param>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public IList<MLBTicketApplyInfo> GetTicketApplys(int companyId, string tourId)
        {
            return dal.GetTicketApplys(companyId, tourId);
        }

        /// <summary>
        /// 机票管理退票
        /// </summary>
        /// <param name="refundTicketInfo">机票退票信息业务实体</param>
        /// <returns></returns>
        public bool RefundTicket(MRefundTicketInfo refundTicketInfo)
        {
            bool result = false;
            if (refundTicketInfo == null) return result;
            if (refundTicketInfo.TicketListId < 1) return result;
            if (refundTicketInfo.OperatorId < 1) return result;

            result = dal.RefundTicket(refundTicketInfo) > 0 ? true : false;

            #region stat.
            if (result)
            {
                if (refundTicketInfo.Status == EyouSoft.Model.EnumType.PlanStructure.TicketRefundSate.已退票
                    && !string.IsNullOrEmpty(refundTicketInfo.TourId))
                {
                    EyouSoft.BLL.UtilityStructure.Utility utilitybll = new EyouSoft.BLL.UtilityStructure.Utility();
                    utilitybll.CalculationTourIncome(refundTicketInfo.TourId, null);
                    utilitybll = null;
                }
            }
            #endregion

            return result;
        }

        /// <summary>
        /// 获取游客退票航段信息集体
        /// </summary>
        /// <param name="refundId">退票编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PlanStructure.TicketFlight> GetRefundTicketFlights(string refundId)
        {
            if (string.IsNullOrEmpty(refundId)) return null;

            return dal.GetRefundTicketFlights(refundId);
        }

        /// <summary>
        /// 获取财务机票审核实体信息
        /// </summary>
        /// <param name="ticketId">机票申请Id</param>
        /// <returns>财务机票审核实体信息</returns>
        public MCheckTicketInfo GetMCheckTicket(string ticketId)
        {
            if (string.IsNullOrEmpty(ticketId))
                return null;

            return dal.GetMCheckTicket(ticketId);
        }

        /// <summary>
        /// 取消出票(删除支出明细，支付登记明细，更改机票状态为未出票)
        /// </summary>
        /// <param name="planId">机票安排编号</param>
        /// <returns></returns>
        public int CancelTicket(string planId)
        {
            if (string.IsNullOrEmpty(planId)) return 0;
            var info = GetTicketModel(planId);
            if (info == null || string.IsNullOrEmpty(info.TourId)) return -1;

            dal.CancelTicket(info.TourId, planId);

            //维护团队状态
            EyouSoft.BLL.TourStructure.Tour tourbll = new EyouSoft.BLL.TourStructure.Tour();
            tourbll.SetTourTicketStatus(info.TourId, EyouSoft.Model.EnumType.PlanStructure.TicketState.None);

            //数据维护
            EyouSoft.BLL.UtilityStructure.Utility u = new EyouSoft.BLL.UtilityStructure.Utility();
            IList<EyouSoft.Model.StatisticStructure.ItemIdAndType> iList = new List<EyouSoft.Model.StatisticStructure.ItemIdAndType>();
            iList.Add(new EyouSoft.Model.StatisticStructure.ItemIdAndType() { ItemId = planId, ItemType = EyouSoft.Model.EnumType.StatisticStructure.PaidType.机票支出 });
            u.CalculationTourOut(info.TourId, new List<EyouSoft.Model.StatisticStructure.ItemIdAndType>() { new EyouSoft.Model.StatisticStructure.ItemIdAndType() { ItemId = planId, ItemType = EyouSoft.Model.EnumType.StatisticStructure.PaidType.机票支出 } });
            u.CalculationTourSettleStatus(info.TourId);

            AddSysLog("取消出票");

            return 1;
        }

        /// <summary>
        /// 取消退票，返回1成功，其它失败
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="jiPiaoGuanLiId">机票管理编号</param>
        /// <returns></returns>
        public int QuXiaoTuiPiao(int companyId, int jiPiaoGuanLiId)
        {
            if (companyId < 1 || jiPiaoGuanLiId < 1) return 0;

            if (dal.QuXiaoTuiPiao(companyId, jiPiaoGuanLiId) == 1)
            {

                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_机票管理.ToString() + "取消退票，退票编号为：" + jiPiaoGuanLiId + "。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "取消退票";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_机票管理;
                logInfo.OperatorId = 0;
                new EyouSoft.BLL.CompanyStructure.SysHandleLogs().Add(logInfo);

                return 1;
            }

            return 0;
        }

        /// <summary>
        /// 取消机票审核，1:成功，-1:非审核通过状态下的机票申请不存在取消审核操作，-2:团队已提交财务不可取消审核
        /// </summary>
        /// <param name="ticketId">机票申请编号</param>
        /// <returns></returns>
        public int QuXiaoShenHe(string ticketId)
        {
            if (string.IsNullOrEmpty(ticketId)) return 0;

            string tourId = string.Empty;
            int dalRetCode = dal.QuXiaoShenHe(ticketId, out tourId);

            if (dalRetCode == 1)
            {
                new EyouSoft.BLL.TourStructure.Tour().SetTourTicketStatus(tourId, EyouSoft.Model.EnumType.PlanStructure.TicketState.None);

                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_机票审核.ToString() + "取消机票审核，机票申请编号为：" + ticketId + "。";
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "取消机票审核";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_机票审核;
                logInfo.OperatorId = 0;

                new EyouSoft.BLL.CompanyStructure.SysHandleLogs().Add(logInfo);
            }

            return dalRetCode;
        }

        /// <summary>
        /// 根据订单编号获取机票申请信息（因为一个订单只有一条机票申请信息）
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public TicketOutListInfo GetTicketOutInfoByOrderId(string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return null;

            return dal.GetTicketOutInfoByOrderId(orderId);
        }

        #endregion

        #region private members
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private bool AddSysLog(string type)
        {
            EyouSoft.BLL.CompanyStructure.SysHandleLogs sysLong = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();
            EyouSoft.Model.CompanyStructure.SysHandleLogs sysModel = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            sysModel.EventMessage = System.DateTime.Now.ToString() + "{0}在机票管理" + type + "了出票";
            sysModel.EventTitle = type + " 机票管理";
            sysModel.ID = Guid.NewGuid().ToString();
            sysModel.EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode;
            sysModel.ModuleId = Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_机票管理;

            return sysLong.Add(sysModel);
        }
        /*/// <summary>
        /// 获取部门人员
        /// </summary>
        /// <returns></returns>
        private int[] GetDepartmentUsers(int[] DepartId, int CompanyId)
        {
            EyouSoft.BLL.CompanyStructure.CompanyUser dalUser = new EyouSoft.BLL.CompanyStructure.CompanyUser();
            IList<int> items = new List<int>();
            foreach (int m in DepartId)
            {
                IList<int> users = GetUsers(CompanyId, m);
                foreach (int u in users)
                {
                    items.Add(u);
                }
            }
            int[] user = new int[items.Count];
            int i = 0;
            foreach (int n in items)
            {
                user[i++] = n;
            }
            return user;
        }*/
        #endregion
    }
}
