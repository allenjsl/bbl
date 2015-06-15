using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace EyouSoft.BLL.FinanceStructure
{
    /// <summary>
    /// 杂费收入、支出业务逻辑
    /// </summary>
    /// 周文超   2011-01-21
    public class OtherCost : EyouSoft.BLL.BLLBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public OtherCost(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }

        private readonly EyouSoft.IDAL.FinanceStructure.IOtherCost idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.FinanceStructure.IOtherCost>();
        /// <summary>
        /// 统一维护业务逻辑
        /// </summary>
        private readonly BLL.UtilityStructure.Utility UtilityBll = new EyouSoft.BLL.UtilityStructure.Utility();
        /// <summary>
        /// 统计分析所有收入明细
        /// </summary>
        private readonly BLL.StatisticStructure.StatAllIncome IncomeBLL = new EyouSoft.BLL.StatisticStructure.StatAllIncome();
        /// <summary>
        /// 统计分析所有支出明细
        /// </summary>
        private readonly BLL.StatisticStructure.StatAllOut OutBLL = new EyouSoft.BLL.StatisticStructure.StatAllOut();
        /// <summary>
        /// 系统操作日志逻辑
        /// </summary>
        private readonly BLL.CompanyStructure.SysHandleLogs HandleLogsBll = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();

        #region 杂费收入方法

        /// <summary>
        /// 添加杂费收入信息
        /// </summary>
        /// <param name="model">杂费收入信息实体</param>
        /// <param name="modelRefund">如果要同时写一条收款明细，请构造此实体；不需赋值null</param>
        /// <returns>返回1表示成功，其他失败</returns>
        public int AddOtherIncome(EyouSoft.Model.FinanceStructure.OtherIncomeInfo model)
        {
            if (model == null)
                return 0;

            IList<EyouSoft.Model.FinanceStructure.OtherIncomeInfo> list = new List<EyouSoft.Model.FinanceStructure.OtherIncomeInfo>();
            list.Add(model);

            return AddOtherIncome(list);
        }

        /// <summary>
        /// 批量添加杂费收入信息
        /// </summary>
        /// <param name="list">杂费收入信息实体集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        public int AddOtherIncome(IList<EyouSoft.Model.FinanceStructure.OtherIncomeInfo> list)
        {
            if (list == null || list.Count <= 0)
                return 0;

            int Result = 0;
            Result = idal.AddOtherIncome(list);
            if (Result == 1)
            {
                IList<EyouSoft.Model.StatisticStructure.StatAllIncome> listStat = new List<EyouSoft.Model.StatisticStructure.StatAllIncome>();
                EyouSoft.Model.StatisticStructure.StatAllIncome tmodel = null;
                foreach (EyouSoft.Model.FinanceStructure.OtherIncomeInfo t in list)
                {
                    tmodel = new EyouSoft.Model.StatisticStructure.StatAllIncome();
                    tmodel.AddAmount = t.AddAmount;
                    tmodel.Amount = t.Amount;
                    tmodel.AreaId = 0;
                    tmodel.CheckAmount = 0;
                    tmodel.CompanyId = t.CompanyId;
                    tmodel.CreateTime = DateTime.Now;
                    tmodel.DepartmentId = 0;
                    tmodel.ItemId = t.IncomeId;
                    tmodel.ItemType = EyouSoft.Model.EnumType.TourStructure.ItemType.团款其他收入;
                    tmodel.NotCheckAmount = 0;
                    tmodel.OperatorId = t.OperatorId;
                    tmodel.OperatorName = string.Empty;
                    tmodel.ReduceAmount = t.ReduceAmount;
                    tmodel.TotalAmount = t.TotalAmount;
                    tmodel.TourId = t.TourId;
                    tmodel.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)255;

                    listStat.Add(tmodel);
                }

                IncomeBLL.Add(listStat);

                #region stat.
                string statTourId = string.Empty;
                IList<Model.StatisticStructure.IncomeItemIdAndType> statItems = new List<Model.StatisticStructure.IncomeItemIdAndType>();
                foreach (EyouSoft.Model.FinanceStructure.OtherIncomeInfo item in list)
                {
                    if (string.IsNullOrEmpty(statTourId))
                    {
                        statTourId = item.TourId;
                    }
                    statItems.Add(new EyouSoft.Model.StatisticStructure.IncomeItemIdAndType()
                    {
                        ItemId = item.IncomeId,
                        ItemType = EyouSoft.Model.EnumType.TourStructure.ItemType.团款其他收入
                    });
                }
                UtilityBll.CalculationTourIncome(statTourId, statItems);
                #endregion

                HandleLogsBll.Add(
                    new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                    {
                        ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费收入,
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费收入.ToString() + "新增了杂费收入数据！",
                        EventTitle = "新增" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费收入.ToString() + "数据"
                    });
            }

            return Result;
        }

        /// <summary>
        /// 获取杂费收入信息
        /// </summary>
        /// <param name="model">杂费信息查询实体</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns>杂费收入信息集合</returns>
        public IList<EyouSoft.Model.FinanceStructure.OtherIncomeInfo> GetOtherIncomeList(EyouSoft.Model.FinanceStructure.OtherCostQuery model, int PageSize, int PageIndex, ref int RecordCount)
        {
            return idal.GetOtherIncomeList(model, base.HaveUserIds, PageSize, PageIndex, ref RecordCount);
        }

        /// <summary>
        /// 获取杂费收入信息
        /// </summary>
        /// <param name="model">杂费信息查询实体</param>
        /// <returns>杂费收入信息集合</returns>
        public IList<EyouSoft.Model.FinanceStructure.OtherIncomeInfo> GetOtherIncomeList(EyouSoft.Model.FinanceStructure.OtherCostQuery model)
        {
            return idal.GetOtherIncomeList(model);
        }

        /// <summary>
        /// 获取杂费收入信息金额合计
        /// </summary>
        /// <param name="model">杂费信息查询实体</param>
        /// <param name="totalAmount">总合计金额</param>
        public void GetOtherIncomeList(Model.FinanceStructure.OtherCostQuery model, ref decimal totalAmount)
        {
            if (model == null)
                return;

            idal.GetOtherIncomeList(model, HaveUserIds, true, ref totalAmount);
        }

        /// <summary>
        /// 获取杂费收入信息
        /// </summary>
        /// <param name="OtherIncomeId">杂费收入信息Id</param>
        /// <returns>杂费收入信息</returns>
        public EyouSoft.Model.FinanceStructure.OtherIncomeInfo GetOtherIncomeInfo(string OtherIncomeId)
        {
            if (string.IsNullOrEmpty(OtherIncomeId))
                return null;

            return idal.GetOtherIncomeInfo(OtherIncomeId);
        }

        /// <summary>
        /// 修改杂费收入信息（只修改增加、减少、小计、备注）
        /// </summary>
        /// <param name="list">杂费收入信息集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        public int UpdateOtherIncome(IList<EyouSoft.Model.FinanceStructure.OtherIncomeInfo> list)
        {
            if (list == null || list.Count <= 0)
                return 0;

            int Result = 0;
            Result = idal.UpdateOtherIncome(list);
            if (Result == 1)
            {
                string strTourId = string.Empty;
                IList<Model.StatisticStructure.IncomeItemIdAndType> listItem = new List<Model.StatisticStructure.IncomeItemIdAndType>();
                foreach (EyouSoft.Model.FinanceStructure.OtherIncomeInfo t in list)
                {
                    if (string.IsNullOrEmpty(strTourId))
                        strTourId = t.TourId;
                    listItem.Add(new EyouSoft.Model.StatisticStructure.IncomeItemIdAndType() { ItemId = t.IncomeId, ItemType = EyouSoft.Model.EnumType.TourStructure.ItemType.团款其他收入 });
                }
                UtilityBll.CalculationTourIncome(strTourId, listItem);

                HandleLogsBll.Add(
                    new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                    {
                        ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费收入,
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费收入.ToString() + "把增加费用、减少费用、小计、备注修改了！",
                        EventTitle = "修改" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费收入.ToString() + "数据"
                    });
            }

            return Result;
        }

        /// <summary>
        /// 修改杂费收入信息（修改所有值）
        /// </summary>
        /// <param name="list">杂费收入信息集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        public int UpdateOtherIncome(EyouSoft.Model.FinanceStructure.OtherIncomeInfo model)
        {
            if (model == null)
                return 0;

            int Result = 0;
            Result = idal.UpdateOtherIncome(model);
            if (Result == 1)
            {
                IList<Model.StatisticStructure.IncomeItemIdAndType> list = new List<Model.StatisticStructure.IncomeItemIdAndType>();
                list.Add(new EyouSoft.Model.StatisticStructure.IncomeItemIdAndType() { ItemId = model.IncomeId, ItemType = EyouSoft.Model.EnumType.TourStructure.ItemType.团款其他收入 });
                UtilityBll.CalculationTourIncome(model.TourId, list);

                HandleLogsBll.Add(
                    new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                    {
                        ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费收入,
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费收入.ToString() + "修改了杂费收入数据！编号为：" + model.IncomeId,
                        EventTitle = "修改" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费收入.ToString() + "数据"
                    });
            }

            return Result;
        }

        /// <summary>
        /// 删除杂费收入
        /// </summary>
        /// <param name="TourId">团队Id</param>
        /// <param name="OtherIncomeIds">杂费收入Id集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        public int DeleteOtherIncome(string TourId, string[] OtherIncomeIds)
        {
            if (OtherIncomeIds == null || OtherIncomeIds.Length <= 0)
                return 0;

            int Result = 0;
            Result = idal.DeleteOtherCost(OtherIncomeIds);
            if (Result == 1)
            {
                IList<Model.StatisticStructure.IncomeItemIdAndType> list = new List<Model.StatisticStructure.IncomeItemIdAndType>();
                foreach (string str in OtherIncomeIds)
                {
                    if (string.IsNullOrEmpty(str))
                        continue;

                    list.Add(new EyouSoft.Model.StatisticStructure.IncomeItemIdAndType() { ItemId = str, ItemType = EyouSoft.Model.EnumType.TourStructure.ItemType.团款其他收入 });
                }

                IncomeBLL.Delete(list);

                if (!string.IsNullOrEmpty(TourId))
                    UtilityBll.CalculationTourIncome(TourId, null);

                HandleLogsBll.Add(
                    new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                    {
                        ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费收入,
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费收入.ToString() + "删除了杂费收入数据！",
                        EventTitle = "删除" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费收入.ToString() + "数据"
                    });
            }

            return Result;
        }

        #endregion

        #region 杂费支出方法

        /// <summary>
        /// 添加杂费支出信息
        /// </summary>
        /// <param name="model">杂费支出信息实体</param>
        /// <returns>返回1表示成功，其他失败</returns>
        public int AddOtherOut(EyouSoft.Model.FinanceStructure.OtherOutInfo model)
        {
            if (model == null)
                return 0;

            IList<EyouSoft.Model.FinanceStructure.OtherOutInfo> list = new List<EyouSoft.Model.FinanceStructure.OtherOutInfo>();
            list.Add(model);

            return AddOtherOut(list);
        }

        /// <summary>
        /// 添加杂费支出信息
        /// </summary>
        /// <param name="model">杂费支出信息实体</param>
        /// <returns>返回1表示成功，其他失败</returns>
        public int AddOtherOut(IList<EyouSoft.Model.FinanceStructure.OtherOutInfo> list)
        {
            if (list == null || list.Count <= 0)
                return 0;

            int Result = 0;
            Result = idal.AddOtherOut(list);

            if (Result == 1)
            {
                IList<EyouSoft.Model.StatisticStructure.StatAllOut> listStat = new List<EyouSoft.Model.StatisticStructure.StatAllOut>();
                EyouSoft.Model.StatisticStructure.StatAllOut tmodel = null;
                foreach (EyouSoft.Model.FinanceStructure.OtherOutInfo t in list)
                {
                    tmodel = new EyouSoft.Model.StatisticStructure.StatAllOut();
                    tmodel.AddAmount = t.AddAmount;
                    tmodel.Amount = t.Amount;
                    tmodel.AreaId = 0;
                    tmodel.CheckAmount = 0;
                    tmodel.CompanyId = t.CompanyId;
                    tmodel.CreateTime = DateTime.Now;
                    tmodel.DepartmentId = 0;
                    tmodel.ItemId = t.OutId;
                    tmodel.ItemType = EyouSoft.Model.EnumType.StatisticStructure.PaidType.杂费支出;
                    tmodel.NotCheckAmount = 0;
                    tmodel.OperatorId = t.OperatorId;
                    tmodel.ReduceAmount = t.ReduceAmount;
                    tmodel.TotalAmount = t.TotalAmount;
                    tmodel.TourId = t.TourId;
                    tmodel.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)255;
                    tmodel.SupplierId = 0;
                    tmodel.SupplierName = string.Empty;

                    listStat.Add(tmodel);
                }

                OutBLL.Add(listStat);

                #region stat.
                string statTourId = string.Empty;
                IList<Model.StatisticStructure.ItemIdAndType> statItems = new List<Model.StatisticStructure.ItemIdAndType>();
                foreach (EyouSoft.Model.FinanceStructure.OtherOutInfo t in list)
                {
                    if (string.IsNullOrEmpty(statTourId))
                    {
                        statTourId = t.TourId;
                    }
                    statItems.Add(new EyouSoft.Model.StatisticStructure.ItemIdAndType()
                    {
                        ItemId = t.OutId,
                        ItemType = EyouSoft.Model.EnumType.StatisticStructure.PaidType.杂费支出
                    });
                }
                UtilityBll.CalculationTourOut(statTourId, statItems);
                #endregion

                HandleLogsBll.Add(
                    new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                    {
                        ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费支出,
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费支出.ToString() + "新增了杂费支出数据！",
                        EventTitle = "新增" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费支出.ToString() + "数据"
                    });
            }

            return Result;
        }

        /// <summary>
        /// 修改杂费支出信息（只修改增加、减少、小计、备注）
        /// </summary>
        /// <param name="list">支出信息集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        public int UpdateOtherOut(IList<EyouSoft.Model.FinanceStructure.OtherOutInfo> list)
        {
            if (list == null || list.Count <= 0)
                return 0;

            int Result = 0;
            Result = idal.UpdateOtherOut(list);
            if (Result == 1)
            {
                string strTourId = string.Empty;
                IList<Model.StatisticStructure.ItemIdAndType> listItem = new List<Model.StatisticStructure.ItemIdAndType>();
                foreach (EyouSoft.Model.FinanceStructure.OtherOutInfo t in list)
                {
                    if (string.IsNullOrEmpty(strTourId))
                        strTourId = t.TourId;
                    listItem.Add(new EyouSoft.Model.StatisticStructure.ItemIdAndType() { ItemId = t.OutId, ItemType = EyouSoft.Model.EnumType.StatisticStructure.PaidType.杂费支出 });
                }
                UtilityBll.CalculationTourOut(strTourId, listItem);

                HandleLogsBll.Add(
                    new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                    {
                        ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费支出,
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费支出.ToString() + "把增加费用、减少费用、小计、备注修改了！",
                        EventTitle = "修改" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费支出.ToString() + "数据"
                    });
            }

            return Result;
        }

        /// <summary>
        /// 修改杂费支出信息（修改所有值）
        /// </summary>
        /// <param name="list">支出信息</param>
        /// <returns>返回1表示成功，其他失败</returns>
        public int UpdateOtherOut(EyouSoft.Model.FinanceStructure.OtherOutInfo model)
        {
            if (model == null)
                return 0;

            int Result = 0;
            Result = idal.UpdateOtherOut(model);
            if (Result == 1)
            {
                IList<Model.StatisticStructure.ItemIdAndType> list = new List<Model.StatisticStructure.ItemIdAndType>();
                list.Add(new EyouSoft.Model.StatisticStructure.ItemIdAndType() { ItemId = model.OutId, ItemType = EyouSoft.Model.EnumType.StatisticStructure.PaidType.杂费支出 });
                UtilityBll.CalculationTourOut(model.TourId, list);

                HandleLogsBll.Add(
                    new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                    {
                        ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费支出,
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费支出.ToString() + "修改了杂费支出数据！编号为：" + model.OutId,
                        EventTitle = "修改" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费支出.ToString() + "数据"
                    });
            }

            return Result;
        }

        /// <summary>
        /// 获取杂费支出信息
        /// </summary>
        /// <param name="model">杂费信息查询实体</param>
        /// <returns>杂费支出信息集合</returns>
        public IList<EyouSoft.Model.FinanceStructure.OtherOutInfo> GetOtherOutList(EyouSoft.Model.FinanceStructure.OtherCostQuery model)
        {
            return idal.GetOtherOutList(model);
        }

        /// <summary>
        /// 获取杂费支出信息
        /// </summary>
        /// <param name="model">杂费信息查询实体</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns>杂费支出信息集合</returns>
        public IList<EyouSoft.Model.FinanceStructure.OtherOutInfo> GetOtherOutList(EyouSoft.Model.FinanceStructure.OtherCostQuery model, int PageSize, int PageIndex, ref int RecordCount)
        {
            return idal.GetOtherOutList(model, base.HaveUserIds, PageSize, PageIndex, ref RecordCount);
        }

        /// <summary>
        /// 获取杂费支出信息金额合计
        /// </summary>
        /// <param name="model">杂费信息查询实体</param>
        /// <param name="totalAmount">总合计金额</param>
        public void GetOtherOutList(Model.FinanceStructure.OtherCostQuery model, ref decimal totalAmount)
        {
            if (model == null)
                return;

            idal.GetOtherIncomeList(model, HaveUserIds, false, ref totalAmount);
        }


        /// <summary>
        /// 获取杂费支出信息
        /// </summary>
        /// <param name="OtherOutId">杂费支出信息Id</param>
        /// <returns>杂费支出信息</returns>
        public EyouSoft.Model.FinanceStructure.OtherOutInfo GetOtherOutInfo(string OtherOutId)
        {
            if (string.IsNullOrEmpty(OtherOutId))
                return null;

            return idal.GetOtherOutInfo(OtherOutId);
        }

        /// <summary>
        /// 删除杂费支出
        /// </summary>
        /// <param name="TourId">团队Id</param>
        /// <param name="OtherOutIds">支出收入Id集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        public int DeleteOtherOut(string TourId, string[] OtherOutIds)
        {
            if (OtherOutIds == null || OtherOutIds.Length <= 0)
                return 0;

            int Result = 0;
            Result = idal.DeleteOtherCost(OtherOutIds);
            if (Result == 1)
            {
                IList<Model.StatisticStructure.ItemIdAndType> list = new List<Model.StatisticStructure.ItemIdAndType>();
                foreach (string str in OtherOutIds)
                {
                    if (string.IsNullOrEmpty(str))
                        continue;

                    list.Add(new EyouSoft.Model.StatisticStructure.ItemIdAndType() { ItemId = str, ItemType = EyouSoft.Model.EnumType.StatisticStructure.PaidType.杂费支出 });
                }

                OutBLL.Delete(list);
                if (!string.IsNullOrEmpty(TourId))
                    UtilityBll.CalculationTourOut(TourId, null);

                HandleLogsBll.Add(
                    new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                    {
                        ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费支出,
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费支出.ToString() + "删除了杂费支出数据！",
                        EventTitle = "删除" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_杂费支出.ToString() + "数据"
                    });
            }

            return Result;
        }

        #endregion

        /// <summary>
        /// 设置杂费收入（支出）的状态
        /// </summary>
        /// <param name="status">支出（收入）状态</param>
        /// <param name="otherCostId">杂费收入（支出）Id</param>
        /// <returns>1：操作成功；其他失败</returns>
        public int SetOtherCostStatus(bool status, params string[] otherCostId)
        {
            if (otherCostId == null || otherCostId.Length <= 0)
                return 0;

            return idal.SetOtherCostStatus(status, otherCostId);
        }
    }
}
