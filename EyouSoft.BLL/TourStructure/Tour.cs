/*Author:汪奇志 2011-01-18*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace EyouSoft.BLL.TourStructure
{
    /// <summary>
    /// 计划中心业务逻辑类
    /// </summary>
    /// Author:汪奇志 2011-01-20
    public class Tour : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.TourStructure.ITour dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.TourStructure.ITour>();

        #region constructure
        /// <summary>
        /// default constructure
        /// </summary>
        public Tour() { }

        /// <summary>
        /// constructor with specified initial value
        /// </summary>
        /// <param name="uinfo">当前登录用户信息</param>
        public Tour(EyouSoft.SSOComponent.Entity.UserInfo uinfo)
        {
            if (uinfo != null)
            {
                base.DepartIds = uinfo.Departs;
                base.CompanyId = uinfo.CompanyID;
            }
        }

        /// <summary>
        /// constructor with specified initial value
        /// </summary>
        /// <param name="uinfo">当前登录用户信息</param>
        /// <param name="isEnableOrganizations">是否启用组织机构</param>
        public Tour(EyouSoft.SSOComponent.Entity.UserInfo uinfo, bool isEnableOrganizations)
        {
            base.IsEnable = isEnableOrganizations;

            if (uinfo != null)
            {
                base.DepartIds = uinfo.Departs;
                base.CompanyId = uinfo.CompanyID;
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

        /// <summary>
        /// 获取公司配置信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        private EyouSoft.Model.CompanyStructure.CompanyFieldSetting GetCompanyConfig(int companyId)
        {
            EyouSoft.BLL.CompanyStructure.CompanySetting bll = new EyouSoft.BLL.CompanyStructure.CompanySetting();
            EyouSoft.Model.CompanyStructure.CompanyFieldSetting config = bll.GetSetting(companyId);
            bll = null;

            return config;
        }
        #endregion

        #region 成员方法
        /// <summary>
        /// 写入散拼计划，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">散拼计划信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int InsertTourInfo(EyouSoft.Model.TourStructure.TourInfo info)
        {
            info.TourId = Guid.NewGuid().ToString();
            info.TourType = EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划;

            #region  create children tours tourid and is exists tour codes
            bool isExistsTourCodes = false;

            if (info.Childrens != null && info.Childrens.Count > 0)
            {
                int i = 0;
                string[] tourCodes = new string[info.Childrens.Count];

                foreach (var children in info.Childrens)
                {
                    tourCodes[i++] = children.TourCode;
                    children.ChildrenId = Guid.NewGuid().ToString();
                }

                IList<string> existsTourCodes = this.ExistsTourCodes(info.CompanyId, null, tourCodes);

                if (existsTourCodes != null && existsTourCodes.Count > 0)
                {
                    isExistsTourCodes = true;
                }
            }

            if (isExistsTourCodes) return -1;
            #endregion

            if (info.Coordinator == null) return -4;

            int dalExceptionCode = dal.InsertTourInfo(info);

            if (dalExceptionCode == -2601) return -1;
            if (dalExceptionCode <= 0) return dalExceptionCode;

            #region stat.
            EyouSoft.BLL.UtilityStructure.Utility utilitybll = new EyouSoft.BLL.UtilityStructure.Utility();
            utilitybll.UpdateRouteSomething(new int[] { info.RouteId }, EyouSoft.Model.EnumType.TourStructure.UpdateTourType.修改上团数);
            utilitybll = null;
            #endregion

            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散拼计划.ToString() + "新增了散拼计划数据，计划编号为：" + info.TourId;
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "添加散拼计划";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散拼计划;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            #endregion

            return dalExceptionCode;
        }

        /// <summary>
        /// 写入团队计划，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">团队计划信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int InsertTeamTourInfo(EyouSoft.Model.TourStructure.TourTeamInfo info)
        {
            info.TourId = Guid.NewGuid().ToString();
            info.OrderId = Guid.NewGuid().ToString();
            info.TourType = EyouSoft.Model.EnumType.TourStructure.TourType.团队计划;

            if (info.BuyerCId < 1) return -3;

            if (string.IsNullOrEmpty(info.TourCode))
            {
                var items = this.CreateAutoTourCodes(info.CompanyId, info.LDate);
                if (items != null && items.Count > 0)
                {
                    info.TourCode = items[0].TourCode;
                }
            }

            #region is exists tour codes
            bool isExistsTourCodes = false;
            IList<string> existsTourCodes = this.ExistsTourCodes(info.CompanyId, null, info.TourCode);
            if (existsTourCodes != null && existsTourCodes.Count > 0)
            {
                isExistsTourCodes = true;
            }
            if (isExistsTourCodes) return -1;
            #endregion

            if (info.Coordinator == null) return -4;

            if (new CompanyStructure.CompanySetting().GetTeamNumberOfPeople(info.CompanyId) == Model.EnumType.CompanyStructure.TeamNumberOfPeople.PartNumber)
            {
                if (info.TourTeamUnit == null || info.TourTeamUnit.NumberCr <= 0)
                    return -5;

                info.PlanPeopleNumber = info.TourTeamUnit.NumberCr + info.TourTeamUnit.NumberEt +
                                        info.TourTeamUnit.NumberQp;
            }
            else
                info.TourTeamUnit = null;

            int dalExceptionCode = dal.InsertTourInfo(info);

            if (dalExceptionCode == -2601) return -1;
            if (dalExceptionCode <= 0) return dalExceptionCode;

            #region stat.
            EyouSoft.BLL.UtilityStructure.Utility utilitybll = new EyouSoft.BLL.UtilityStructure.Utility();
            utilitybll.UpdateRouteSomething(new int[] { info.RouteId }, EyouSoft.Model.EnumType.TourStructure.UpdateTourType.修改上团数和收客数);
            utilitybll.UpdateTradeNum(new int[] { info.BuyerCId });
            utilitybll = null;
            //统计分析收入明细已在存储过程中完成
            #endregion

            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_团队计划.ToString() + "新增了团队计划数据，计划编号为：" + info.TourId;
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "添加团队计划";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_团队计划;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            #endregion

            return dalExceptionCode;
        }

        /// <summary>
        /// 写入单项服务，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">单项服务信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int InsertSingleTourInfo(EyouSoft.Model.TourStructure.TourSingleInfo info)
        {
            info.TourId = Guid.NewGuid().ToString();
            info.OrderId = Guid.NewGuid().ToString();
            info.TourType = EyouSoft.Model.EnumType.TourStructure.TourType.单项服务;
            if (info.BuyerCId < 1) return -3;
            if (info.SellerId < 1) return -4;
            if (string.IsNullOrEmpty(info.TourCode)) return -5;
            if (info.LDate == DateTime.MinValue) return -6;

            #region is exists tour codes
            bool isExistsTourCodes = false;
            IList<string> existsTourCodes = this.ExistsTourCodes(info.CompanyId, null, info.TourCode);
            if (existsTourCodes != null && existsTourCodes.Count > 0)
            {
                isExistsTourCodes = true;
            }
            if (isExistsTourCodes) return -1;
            #endregion

            #region 游客编号处理
            if (info.Customers != null && info.Customers.Count > 0)
            {
                foreach (var traveller in info.Customers)
                {
                    traveller.ID = Guid.NewGuid().ToString();
                }
            }
            #endregion

            int dalExceptionCode = dal.InsertTourInfo(info);

            if (dalExceptionCode == -2601) return -1;
            if (dalExceptionCode <= 0) return dalExceptionCode;

            #region stat.
            //统计分析收入明细已在存储过程中完成
            //统计分析支出明细已在存储过程中完成
            EyouSoft.BLL.UtilityStructure.Utility utilitybll = new EyouSoft.BLL.UtilityStructure.Utility();
            var outStats = new List<Model.StatisticStructure.ItemIdAndType>();
            if (info.Plans != null && info.Plans.Count > 0)
            {
                foreach (var plan in info.Plans)
                {
                    outStats.Add(new EyouSoft.Model.StatisticStructure.ItemIdAndType()
                    {
                        ItemId = plan.PlanId,
                        ItemType = EyouSoft.Model.EnumType.StatisticStructure.PaidType.单项服务供应商安排
                    });
                }
            }
            utilitybll.CalculationTourOut(info.TourId, outStats);
            utilitybll.UpdateTradeNum(new int[] { info.BuyerCId });
            utilitybll.CalculationTourIncome(info.TourId, null);
            if (info.Plans != null && info.Plans.Count > 0)
            {
                foreach (var item in info.Plans)
                {
                    utilitybll.ServerTradeCount(item.SupplierId);
                }
            }
            utilitybll = null;
            #endregion

            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.单项服务_单项服务.ToString() + "新增了单项服务数据，单项服务编号为：" + info.TourId;
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "添加单项服务";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.单项服务_单项服务;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            #endregion

            return dalExceptionCode;
        }

        /// <summary>
        /// 修改散拼计划，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">散拼计划信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int UpdateTourInfo(EyouSoft.Model.TourStructure.TourInfo info)
        {
            if (info == null || string.IsNullOrEmpty(info.TourId)) return -2;
            info.TourType = EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划;

            #region is exists tour codes
            bool isExistsTourCodes = false;
            IList<string> existsTourCodes = this.ExistsTourCodes(info.CompanyId, info.TourId, info.TourCode);
            if (existsTourCodes != null && existsTourCodes.Count > 0)
            {
                isExistsTourCodes = true;
            }
            if (isExistsTourCodes) return -1;
            #endregion

            int dalExceptionCode = dal.UpdateTourInfo(info);

            if (dalExceptionCode == -2601) return -1;
            if (dalExceptionCode <= 0) return dalExceptionCode;

            #region stat.
            EyouSoft.BLL.UtilityStructure.Utility utilitybll = new EyouSoft.BLL.UtilityStructure.Utility();
            utilitybll.UpdateRouteSomething(new int[] { info.RouteId, info.ORouteId }, EyouSoft.Model.EnumType.TourStructure.UpdateTourType.修改上团数);
            utilitybll = null;
            #endregion

            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散拼计划.ToString() + "修改了散拼计划数据，计划编号为：" + info.TourId;
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "修改散拼计划";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散拼计划;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            #endregion

            return dalExceptionCode;
        }

        /// <summary>
        /// 修改团队计划，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">团队计划信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int UpdateTeamTourInfo(EyouSoft.Model.TourStructure.TourTeamInfo info)
        {
            if (info == null || string.IsNullOrEmpty(info.TourId)) return -2;
            info.TourType = EyouSoft.Model.EnumType.TourStructure.TourType.团队计划;

            if (info.BuyerCId < 1) return -3;

            #region is exists tour codes
            bool isExistsTourCodes = false;
            IList<string> existsTourCodes = this.ExistsTourCodes(info.CompanyId, info.TourId, info.TourCode);
            if (existsTourCodes != null && existsTourCodes.Count > 0)
            {
                isExistsTourCodes = true;
            }
            if (isExistsTourCodes) return -1;
            #endregion

            if (new CompanyStructure.CompanySetting().GetTeamNumberOfPeople(info.CompanyId) == Model.EnumType.CompanyStructure.TeamNumberOfPeople.PartNumber)
            {
                if (info.TourTeamUnit == null || info.TourTeamUnit.NumberCr <= 0)
                    return -5;

                info.PlanPeopleNumber = info.TourTeamUnit.NumberCr + info.TourTeamUnit.NumberEt +
                                        info.TourTeamUnit.NumberQp;
            }
            else
                info.TourTeamUnit = null;

            int dalExceptionCode = dal.UpdateTourInfo(info);

            if (dalExceptionCode == -2601) return -1;
            if (dalExceptionCode <= 0) return dalExceptionCode;

            #region stat.
            EyouSoft.BLL.UtilityStructure.Utility utilitybll = new EyouSoft.BLL.UtilityStructure.Utility();
            utilitybll.UpdateRouteSomething(new int[] { info.RouteId, info.ORouteId }, EyouSoft.Model.EnumType.TourStructure.UpdateTourType.修改上团数和收客数);
            var incomeStats = new List<Model.StatisticStructure.IncomeItemIdAndType>();
            incomeStats.Add(new EyouSoft.Model.StatisticStructure.IncomeItemIdAndType()
            {
                ItemId = info.OrderId,
                ItemType = EyouSoft.Model.EnumType.TourStructure.ItemType.订单
            });
            utilitybll.CalculationTourIncome(info.TourId, incomeStats);
            utilitybll.UpdateTradeNum(new int[] { info.BuyerCId, info.OBuyerCId });
            utilitybll.CalculationTourSettleStatus(info.TourId);
            utilitybll = null;
            #endregion

            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_团队计划.ToString() + "修改了团队计划数据，把计划总金额修改成" + info.TotalAmount + "，计划编号为：" + info.TourId;
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "修改团队计划";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.团队计划_团队计划;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            #endregion

            return dalExceptionCode;
        }

        /// <summary>
        /// 修改单项服务，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">单项服务信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int UpdateSingleTourInfo(EyouSoft.Model.TourStructure.TourSingleInfo info)
        {
            if (info == null && string.IsNullOrEmpty(info.TourId)) return -2;
            info.TourType = EyouSoft.Model.EnumType.TourStructure.TourType.单项服务;

            if (info.BuyerCId < 1) return -3;
            if (info.SellerId < 1) return -4;
            if (string.IsNullOrEmpty(info.TourCode)) return -5;
            if (info.LDate == DateTime.MinValue) return -6;

            #region is exists tour codes
            bool isExistsTourCodes = false;
            IList<string> existsTourCodes = this.ExistsTourCodes(info.CompanyId, info.TourId, info.TourCode);
            if (existsTourCodes != null && existsTourCodes.Count > 0)
            {
                isExistsTourCodes = true;
            }
            if (isExistsTourCodes) return -1;
            #endregion

            #region 游客信息处理
            if (info.Customers != null && info.Customers.Count > 0)
            {
                foreach (var traveller in info.Customers)
                {
                    if (string.IsNullOrEmpty(traveller.ID))
                    {
                        traveller.ID = Guid.NewGuid().ToString();
                    }
                }
            }
            #endregion

            //原供应商安排信息集合 用于交易次数维护
            IList<int> oSuppliers = dal.GetSingleSuppliers(info.TourId);

            int dalExceptionCode = dal.UpdateTourInfo(info);

            if (dalExceptionCode == -2601) return -1;
            if (dalExceptionCode <= 0) return dalExceptionCode;

            #region stat.
            EyouSoft.BLL.UtilityStructure.Utility utilitybll = new EyouSoft.BLL.UtilityStructure.Utility();

            var incomeStats = new List<Model.StatisticStructure.IncomeItemIdAndType>();
            incomeStats.Add(new EyouSoft.Model.StatisticStructure.IncomeItemIdAndType()
            {
                ItemId = info.OrderId,
                ItemType = EyouSoft.Model.EnumType.TourStructure.ItemType.订单
            });
            utilitybll.CalculationTourIncome(info.TourId, incomeStats);

            var outStats = new List<Model.StatisticStructure.ItemIdAndType>();
            if (info.Plans != null && info.Plans.Count > 0)
            {
                foreach (var plan in info.Plans)
                {
                    outStats.Add(new EyouSoft.Model.StatisticStructure.ItemIdAndType()
                    {
                        ItemId = plan.PlanId,
                        ItemType = EyouSoft.Model.EnumType.StatisticStructure.PaidType.单项服务供应商安排
                    });
                }
            }
            utilitybll.CalculationTourOut(info.TourId, outStats);
            utilitybll.UpdateTradeNum(new int[] { info.BuyerCId, info.OBuyerCId });

            if (oSuppliers != null && oSuppliers.Count > 0)
            {
                foreach (var item in oSuppliers)
                {
                    utilitybll.ServerTradeCount(item);
                }
            }
            if (info.Plans != null && info.Plans.Count > 0)
            {
                foreach (var item in info.Plans)
                {
                    utilitybll.ServerTradeCount(item.SupplierId);
                }
            }

            utilitybll.CalculationTourSettleStatus(info.TourId);
            utilitybll = null;
            //统计分析支出明细已在存储过程中完成（含删除的供应商安排及新增的供应商安排）
            #endregion

            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.单项服务_单项服务.ToString() + "修改了单项服务数据，把收入总金额修改成" + info.TotalAmount + "，支出总金额修改成" + info.TotalOutAmount + "，计划编号为：" + info.TourId;
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "修改单项服务";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.单项服务_单项服务;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            #endregion

            return dalExceptionCode;
        }

        /// <summary>
        /// 删除计划信息
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <returns></returns>
        public bool Delete(string tourId)
        {
            if (string.IsNullOrEmpty(tourId)) return false;

            //计划供应商信息集合 用于交易次数维护
            IList<int> suppliers = dal.GetTourSuppliers(tourId);
            //单项服务供应商信息集合 用于交易次数维护
            IList<int> singleSuppliers = dal.GetSingleSuppliers(tourId);
            //计划客户单位信息集合 用于交易次数维护
            IList<int> customers = dal.GetTourCustomers(tourId);

            bool dalExceptionCode = dal.Delete(tourId);

            if (dalExceptionCode)
            {
                #region stat.
                EyouSoft.BLL.UtilityStructure.Utility utilitybll = new EyouSoft.BLL.UtilityStructure.Utility();
                if (suppliers != null && suppliers.Count > 0)
                {
                    foreach (var item in suppliers)
                    {
                        utilitybll.ServerTradeCount(item);
                    }
                }

                if (singleSuppliers != null && singleSuppliers.Count > 0)
                {
                    foreach (var item in singleSuppliers)
                    {
                        utilitybll.ServerTradeCount(item);
                    }
                }

                if (customers != null && customers.Count > 0)
                {
                    int customerCount = customers.Count;
                    int[] c = new int[customerCount];
                    for (int i = 0; i < customerCount; i++)
                    {
                        c[i] = customers[i];
                    }
                    utilitybll.UpdateTradeNum(c);
                }

                utilitybll = null;
                #endregion

                #region LGWR
                EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
                logInfo.CompanyId = 0;
                logInfo.DepatId = 0;
                logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
                logInfo.EventIp = string.Empty;
                logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散拼计划.ToString() + "删除了计划信息，计划编号为：" + tourId;
                logInfo.EventTime = DateTime.Now;
                logInfo.EventTitle = "删除计划信息";
                logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散拼计划;
                logInfo.OperatorId = 0;
                this.Logwr(logInfo);
                #endregion
            }

            return dalExceptionCode;
        }

        /// <summary>
        /// 获取散拼计划信息集合(专线端)
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourBaseInfo> GetTours(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info)
        {
            info = info ?? new EyouSoft.Model.TourStructure.TourSearchInfo();
            //取公司配置的列表显示控制配置
            info = info ?? new EyouSoft.Model.TourStructure.TourSearchInfo();
            if (!(info.SDate.HasValue || info.EDate.HasValue))
            {
                EyouSoft.Model.CompanyStructure.CompanyFieldSetting config = this.GetCompanyConfig(companyId);
                info.SDate = DateTime.Now.AddMonths(-config.DisplayBeforeMonth);
                info.EDate = DateTime.Now.AddMonths(config.DisplayAfterMonth);
            }

            return dal.GetTours(companyId, pageSize, pageIndex, ref recordCount, info, this.HaveUserIds);
        }

        /// <summary>
        /// 获取团队计划信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBTeamTourInfo> GetToursTeam(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info)
        {
            info = info ?? new EyouSoft.Model.TourStructure.TourSearchInfo();
            //取公司配置的列表显示控制配置
            info = info ?? new EyouSoft.Model.TourStructure.TourSearchInfo();
            if (!(info.SDate.HasValue || info.EDate.HasValue))
            {
                EyouSoft.Model.CompanyStructure.CompanyFieldSetting config = this.GetCompanyConfig(companyId);
                info.SDate = DateTime.Now.AddMonths(-config.DisplayBeforeMonth);
                info.EDate = DateTime.Now.AddMonths(config.DisplayAfterMonth);
            }

            return dal.GetToursTeam(companyId, pageSize, pageIndex, ref recordCount, info, this.HaveUserIds);
        }

        /// <summary>
        /// 获取单项服务信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBSingleTourInfo> GetToursSingle(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSingleSearchInfo info)
        {
            info = info ?? new EyouSoft.Model.TourStructure.TourSingleSearchInfo();

            return dal.GetToursSingle(companyId, pageSize, pageIndex, ref recordCount, info, this.HaveUserIds);
        }

        /// <summary>
        /// 获取待核算计划信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBAccountingTourInfo> GetToursNotAccounting(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info)
        {
            //取公司配置的列表显示控制配置
            info = info ?? new EyouSoft.Model.TourStructure.TourSearchInfo();
            if (!(info.SDate.HasValue || info.EDate.HasValue))
            {
                EyouSoft.Model.CompanyStructure.CompanyFieldSetting config = this.GetCompanyConfig(companyId);
                info.SDate = DateTime.Now.AddMonths(-config.DisplayBeforeMonth);
                info.EDate = DateTime.Now.AddMonths(config.DisplayAfterMonth);
            }

            return dal.GetToursNotAccounting(companyId, pageSize, pageIndex, ref recordCount, info, this.HaveUserIds);
        }

        /// <summary>
        /// 获取待核算计划信息金额合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="info">查询信息</param>
        /// <param name="income">总收入</param>
        /// <param name="expenditure">总支出</param>
        /// <param name="payOff">总利润分配</param>
        /// <param name="adultNumber">总成人数</param>
        /// <param name="childNumber">总儿童人数</param>
        public void GetToursNotAccounting(int companyId, Model.TourStructure.TourSearchInfo info, ref decimal income, ref decimal expenditure, ref decimal payOff, ref int adultNumber, ref int childNumber)
        {
            //取公司配置的列表显示控制配置
            info = info ?? new EyouSoft.Model.TourStructure.TourSearchInfo();
            if (!(info.SDate.HasValue || info.EDate.HasValue))
            {
                Model.CompanyStructure.CompanyFieldSetting config = GetCompanyConfig(companyId);
                info.SDate = DateTime.Now.AddMonths(-config.DisplayBeforeMonth);
                info.EDate = DateTime.Now.AddMonths(config.DisplayAfterMonth);
            }

            dal.GetToursNotAccounting(companyId, info, HaveUserIds, ref income, ref expenditure, ref payOff,
                                      ref adultNumber, ref childNumber);
        }

        /// <summary>
        /// 获取已核算计划信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBAccountingTourInfo> GetToursAccounting(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info)
        {
            //取公司配置的列表显示控制配置
            info = info ?? new EyouSoft.Model.TourStructure.TourSearchInfo();
            if (!(info.SDate.HasValue || info.EDate.HasValue))
            {
                EyouSoft.Model.CompanyStructure.CompanyFieldSetting config = this.GetCompanyConfig(companyId);
                info.SDate = DateTime.Now.AddMonths(-config.DisplayBeforeMonth);
                info.EDate = DateTime.Now.AddMonths(config.DisplayAfterMonth);
            }

            return dal.GetToursAccounting(companyId, pageSize, pageIndex, ref recordCount, info, this.HaveUserIds);
        }

        /// <summary>
        /// 获取已核算计划信息金额合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="info">查询信息</param>
        /// <param name="income">总收入</param>
        /// <param name="expenditure">总支出</param>
        /// <param name="payOff">总利润分配</param>
        /// <param name="adultNumber">总成人数</param>
        /// <param name="childNumber">总儿童人数</param>
        public void GetToursAccounting(int companyId, Model.TourStructure.TourSearchInfo info, ref decimal income
            , ref decimal expenditure, ref decimal payOff, ref int adultNumber, ref int childNumber)
        {
            //取公司配置的列表显示控制配置
            info = info ?? new EyouSoft.Model.TourStructure.TourSearchInfo();
            if (!(info.SDate.HasValue || info.EDate.HasValue))
            {
                EyouSoft.Model.CompanyStructure.CompanyFieldSetting config = this.GetCompanyConfig(companyId);
                info.SDate = DateTime.Now.AddMonths(-config.DisplayBeforeMonth);
                info.EDate = DateTime.Now.AddMonths(config.DisplayAfterMonth);
            }

            dal.GetToursAccounting(companyId, info, HaveUserIds, ref income, ref expenditure, ref payOff,
                                      ref adultNumber, ref childNumber);
        }

        /// <summary>
        /// 获取计划信息业务实体
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.TourBaseInfo GetTourInfo(string tourId)
        {
            return dal.GetTourInfo(tourId);
        }

        /// <summary>
        /// 设置团队状态，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="status">状态</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int SetStatus(string tourId, EyouSoft.Model.EnumType.TourStructure.TourStatus status)
        {
            int result = 0;

            if (status == EyouSoft.Model.EnumType.TourStructure.TourStatus.核算结束)//核算结束条件：已成本确认、已团队复核
            {
                if (dal.GetIsExistsExpense(tourId) && !dal.GetIsCostConfirm(tourId)) return -1;
                if (!dal.GetIsReview(tourId)) return -2;
            }

            result = dal.SetStatus(tourId, status);

            if (result <= 0) return 0;

            if (status != EyouSoft.Model.EnumType.TourStructure.TourStatus.财务核算
                && status != EyouSoft.Model.EnumType.TourStructure.TourStatus.核算结束)
            {
                result = this.SetIsReview(tourId, false);
                if (result <= 0) return 0;
            }

            #region /**/
            /*using (TransactionScope tran = new TransactionScope())
            {
                result = dal.SetStatus(tourId, status);

                if (result <= 0) return 0;

                if (status != EyouSoft.Model.EnumType.TourStructure.TourStatus.财务核算 
                    && status != EyouSoft.Model.EnumType.TourStructure.TourStatus.核算结束)
                {
                    result = this.SetIsReview(tourId, false);
                    if (result <= 0) return 0;
                }

                tran.Complete();
            }*/
            #endregion

            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散拼计划.ToString() + "修改了计划状态，计划状态修改为" + status.ToString() + "，计划编号为：" + tourId;
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "修改计划状态";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散拼计划;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            #endregion

            return 1;
        }

        /// <summary>
        /// 设置团队成本确认状态，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="isCostConfirm">状态</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int SetIsCostConfirm(string tourId, bool isCostConfirm)
        {
            int dalExceptionCode = dal.SetIsCostConfirm(tourId, isCostConfirm);
            if (dalExceptionCode <= 0) return dalExceptionCode;

            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款支出.ToString() + "修改了成本确认状态，成本确认状态修改成" + isCostConfirm.ToString() + "，计划编号为：" + tourId;
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "修改成本确认状态";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团款支出;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            #endregion

            return dalExceptionCode;
        }

        /// <summary>
        /// 设置团队复核状态，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="isReview">状态</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int SetIsReview(string tourId, bool isReview)
        {
            int dalExceptionCode = dal.SetIsReview(tourId, isReview);
            if (dalExceptionCode <= 0) return dalExceptionCode;

            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团队核算.ToString() + "修改了团队复核状态，复核状态修改成" + isReview.ToString() + "，计划编号为：" + tourId;
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "修改团队复核状态";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团队核算;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            #endregion

            return dalExceptionCode;
        }

        /// <summary>
        /// 设置团队机票状态，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="status">状态</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int SetTourTicketStatus(string tourId, EyouSoft.Model.EnumType.PlanStructure.TicketState status)
        {
            EyouSoft.BLL.PlanStruture.PlaneTicket planTicketBLL = new EyouSoft.BLL.PlanStruture.PlaneTicket();

            IList<EyouSoft.Model.PlanStructure.MLBTicketApplyInfo> items = planTicketBLL.GetTicketApplys(0, tourId);
            status = EyouSoft.Model.EnumType.PlanStructure.TicketState.None;

            #region 机票状态优先级处理
            //机票状态优先级：机票申请；审核通过；已出票；None
            if (items != null && items.Count > 0)
            {
                var ienumerable = items.Where(item => item.Status == EyouSoft.Model.EnumType.PlanStructure.TicketState.机票申请);

                if (ienumerable != null && ienumerable.Count() > 0)
                {
                    status = EyouSoft.Model.EnumType.PlanStructure.TicketState.机票申请;
                }
                else
                {
                    ienumerable = items.Where(item => item.Status == EyouSoft.Model.EnumType.PlanStructure.TicketState.审核通过);
                    if (ienumerable != null && ienumerable.Count() > 0)
                    {
                        status = EyouSoft.Model.EnumType.PlanStructure.TicketState.审核通过;
                    }
                    else
                    {
                        ienumerable = items.Where(item => item.Status == EyouSoft.Model.EnumType.PlanStructure.TicketState.已出票);

                        if (ienumerable != null && ienumerable.Count() > 0)
                        {
                            status = EyouSoft.Model.EnumType.PlanStructure.TicketState.已出票;
                        }
                    }
                }
            }
            else
            {
                status = EyouSoft.Model.EnumType.PlanStructure.TicketState.None;
            }
            #endregion

            return dal.SetTourTicketStatus(tourId, status);
        }

        /// <summary>
        /// 设置团队虚拟实收人数，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="expression">人数的数值表达式</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int SetTourVirtualPeopleNumber(string tourId, int expression)
        {
            if (expression < 0) expression = 0;
            int dalExceptionCode = dal.SetTourVirtualPeopleNumber(tourId, expression);
            if (dalExceptionCode <= 0) return dalExceptionCode;

            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散拼计划.ToString() + "修改了虚拟实收人数，虚拟实收人数修改成" + expression + "，计划编号为：" + tourId;
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "修改计划虚拟实收人数";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散拼计划;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            #endregion

            return dalExceptionCode;
        }

        /// <summary>
        /// 生成团号
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="ldate">出团日期</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourChildrenInfo> CreateAutoTourCodes(int companyId, params DateTime[] ldates)
        {
            if (companyId < 1 || ldates == null || ldates.Length < 1) return null;

            return dal.CreateAutoTourCodes(companyId, ldates);
        }

        /// <summary>
        /// 团号重复验证，返回重复的团号集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="tourId">团队编号 注:新增验证时传string.Empty或null</param>
        /// <param name="tourCodes">团号</param>
        /// <returns></returns>
        public IList<string> ExistsTourCodes(int companyId, string tourId, params string[] tourCodes)
        {
            if (companyId < 1 || tourCodes == null || tourCodes.Length < 1) return null;

            return dal.ExistsTourCodes(companyId, tourId, tourCodes);
        }

        /// <summary>
        /// 获取计划信息集合(按线路编号)
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="routeId">线路编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourBaseInfo> GetToursByRouteId(int companyId, int pageSize, int pageIndex, ref int recordCount, int routeId)
        {
            if (routeId == 0) return null;

            return dal.GetToursByRouteId(companyId, pageSize, pageIndex, ref recordCount, routeId);
        }

        /// <summary>
        /// 获取散拼计划报价信息集合
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> GetPriceStandards(string tourId)
        {
            return dal.GetPriceStandards(tourId);
        }

        /// <summary>
        /// 获取计划列表（组团端）
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <param name="tourDisplayType">团队展示方式</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBZTTours> GetToursZTD(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info, EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType tourDisplayType)
        {
            if (companyId <= 0) return null;

            if (info != null && (info.SDate.HasValue || info.EDate.HasValue || !string.IsNullOrEmpty(info.TourId))) tourDisplayType = EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType.明细团;

            return dal.GetToursZTD(companyId, pageSize, pageIndex, ref recordCount, info, tourDisplayType);
        }

        /// <summary>
        /// 获取计划列表（供应商交易次数，仅地接）
        /// </summary>
        /// <param name="companyId">专线公司编号</param> 
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="gysId">供应商编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBGYSTours> GetToursGYS(int companyId, int pageSize, int pageIndex, ref int recordCount, int gysId)
        {
            return GetToursGYS(companyId, pageSize, pageIndex, ref recordCount, gysId, null);
        }

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
        public IList<EyouSoft.Model.TourStructure.LBGYSTours> GetToursGYS(int companyId, int pageSize, int pageIndex, ref int recordCount, int gysId, EyouSoft.Model.TourStructure.MTimesSummaryDiJieSearchInfo searchInfo)
        {
            if (companyId <= 0)
                return null;

            return dal.GetToursGYSDiJie(companyId, pageSize, pageIndex, ref recordCount, gysId, searchInfo);
        }

        /// <summary>
        /// 获取计划列表（供应商交易次数，仅票务）
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="gysId">供应商编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.JPGYSTours> GetToursJPGYS(int companyId, int pageSize, int pageIndex, ref int recordCount, int gysId)
        {
            if (companyId <= 0)
                return null;

            return dal.GetToursGYSJiPiao(companyId, pageSize, pageIndex, ref recordCount, gysId);
        }
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
        public IList<EyouSoft.Model.TourStructure.JPGYSTours> GetToursJPGYS(int companyId, int pageSize, int pageIndex, ref int recordCount, int gysId, EyouSoft.Model.TourStructure.MTimesSummaryDiJieSearchInfo searchInfo)
        {
            if (companyId <= 0)
                return null;

            return dal.GetToursGYSJiPiao(companyId, pageSize, pageIndex, ref recordCount,gysId, searchInfo);
        }
        /// <summary>
        /// 获取计划列表（利润统计团队数）
        /// </summary>
        /// <param name="copanyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBLYTJTours> GetToursLYTJ(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.StatisticStructure.QueryEarningsStatistic info)
        {
            if (companyId <= 0)
                return null;

            return dal.GetToursLYTJ(companyId, pageSize, pageIndex, ref recordCount, info, this.HaveUserIds);
        }

        /// <summary>
        /// 获取计划列表（组团端首页）
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="areaId">线路区域编号</param>
        /// <param name="expression">返回指定行数的数值表达式</param>
        /// <param name="tourDisplayType">团队展示方式</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBZTTours> GetToursZTDSY(int companyId, int areaId, int expression, EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType tourDisplayType)
        {
            if (companyId <= 0) return null;

            return dal.GetToursZTDSY(companyId, areaId, expression, tourDisplayType);
        }

        /// <summary>
        /// 获取团款支出信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="isSettleOut">支出是否已结清</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBTKZCTourInfo> GetToursTKZC(int companyId, bool isSettleOut, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchTKZCInfo info)
        {
            return dal.GetToursTKZC(companyId, isSettleOut, pageSize, pageIndex, ref recordCount, info, this.HaveUserIds);
        }

        /// <summary>
        /// 获取团款支出信息集合金额合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="isSettleOut">支出是否已结清</param>
        /// <param name="info">查询信息</param>
        /// <param name="singleAmount">单项服务支出金额</param>
        /// <param name="singleHasAmount">单项服务支出已付金额</param>
        /// <param name="ticketAmount">机票支出金额</param>
        /// <param name="ticketHasAmount">机票支出已付金额</param>
        /// <param name="travelAgencyAmount">地接支出金额</param>
        /// <param name="travelAgencyHasAmount">地接支出已付金额</param>
        /// <returns></returns>
        public void GetToursTKZC(int companyId, bool isSettleOut, Model.TourStructure.TourSearchTKZCInfo info
                          , ref decimal travelAgencyAmount, ref decimal travelAgencyHasAmount
                          , ref decimal ticketAmount, ref decimal ticketHasAmount
                          , ref decimal singleAmount, ref decimal singleHasAmount)
        {
            dal.GetToursTKZC(companyId, isSettleOut, info, HaveUserIds
                             , ref travelAgencyAmount, ref travelAgencyHasAmount
                             , ref ticketAmount, ref ticketHasAmount
                             , ref singleAmount, ref singleHasAmount);
        }

        /// <summary>
        /// 获取计划单项服务支出信息
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <param name="totalAmount">支出金额</param>
        /// <param name="unpaidAmount">未付金额</param>
        /// <param name="searchInfo">查询信息</param>
        public void GetTourExpense(string tourId, out decimal totalAmount, out decimal unpaidAmount, EyouSoft.Model.PlanStructure.MExpendSearchInfo searchInfo)
        {
            dal.GetTourExpense(tourId, out totalAmount, out unpaidAmount, searchInfo);
        }
        /// <summary>
        /// 获取计划地接社信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourLocalAgencyInfo> GetTourLocalAgencys(string tourId)
        {
            return dal.GetTourLocalAgencys(tourId);
        }

        /// <summary>
        /// 根据客户等级获取计划报价(门市价、同行价)信息，默认取多个报价标准的第一行
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <param name="levelId">客户等级编号</param>
        /// <param name="marketPrice">门市价格</param>
        /// <param name="levelPrice">客户等级对应的价格</param>
        /// <returns></returns>
        /// <remarks>若未匹配到对应的客户等级,同行价等于门市价</remarks>
        public void GetTourPrice(string tourId, int levelId, out decimal marketPrice, out decimal levelPrice)
        {
            marketPrice = 0;
            levelPrice = 0;
            bool isMatch = false;
            var items = this.GetPriceStandards(tourId);

            if (items != null && items.Count > 0)
            {
                var item = items[0];
                if (item.CustomerLevels != null && item.CustomerLevels.Count > 0)
                {
                    foreach (var level in item.CustomerLevels)
                    {
                        //取出来的客户等级去匹配当前登录的客户等级
                        if (level.LevelId == levelId)
                        {
                            levelPrice = level.AdultPrice;
                            isMatch = true;
                        }

                        if (level.LevelType == EyouSoft.Model.EnumType.CompanyStructure.CustomLevType.门市)
                        {
                            marketPrice = level.AdultPrice;

                            if (!isMatch)
                            {
                                levelPrice = level.AdultPrice;
                            }
                        }
                    }
                }
            }

            return;
        }

        /// <summary>
        /// 获取计划报价门市价信息，默认取多个报价标准的第一行
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <param name="adultPrice">门市成人价</param>
        /// <param name="childrenPrice">门市儿童价</param>
        /// <returns></returns>
        public void GetTourMarketPrice(string tourId, out decimal adultPrice, out decimal childrenPrice)
        {
            adultPrice = 0;
            childrenPrice = 0;

            var items = this.GetPriceStandards(tourId);
            if (items != null && items.Count > 0)
            {
                var item = items[0];
                if (item.CustomerLevels != null && item.CustomerLevels.Count > 0)
                {
                    foreach (var level in item.CustomerLevels)
                    {
                        if (level.LevelType == EyouSoft.Model.EnumType.CompanyStructure.CustomLevType.门市)
                        {
                            adultPrice = level.AdultPrice;
                            childrenPrice = level.ChildrenPrice;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取计划计调员信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public IList<string> GetTourCoordinators(string tourId)
        {
            IList<string> items = new List<string>();
            var tmps = dal.GetTourCoordinators(tourId);

            if (tmps != null && tmps.Count > 0)
            {
                foreach (var tmp in tmps)
                {
                    items.Add(tmp.Name);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取团队/散拼计划计调员信息
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.TourCoordinatorInfo GetTourCoordinator(string tourId)
        {
            EyouSoft.Model.TourStructure.TourCoordinatorInfo info = null;
            var items = dal.GetTourCoordinators(tourId);
            if (items != null && items.Count > 0) info = items[0];
            return info;
        }

        /// <summary>
        /// 获取计划送团人信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public IList<Model.TourStructure.TourSentPeopleInfo> GetTourSentPeoples(string tourId)
        {
            if (string.IsNullOrEmpty(tourId)) return null;
            return dal.GetTourSentPeoples(tourId);
        }

        /// <summary>
        /// 设置计划导游及集合标志信息
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <param name="guides">导游信息集合</param>
        /// <param name="gatheringSign">集合标志</param>
        /// <returns></returns>
        public bool SetTourGuides(string tourId, IList<string> guides, string gatheringSign)
        {
            if (string.IsNullOrEmpty(tourId)) return false;
            return dal.SetTourGuides(tourId, guides, gatheringSign);
        }

        /// <summary>
        /// 根据用户编号获取其运送团的信息
        /// </summary>
        /// <param name="UserID">用户编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourSentTask> GetMySendTourInfo(int pageSize, int pageIndex, ref int recordCount, int[] UserID, EyouSoft.Model.TourStructure.TourSentTaskSearch TourSentTaskSearch)
        {
            if (UserID != null && UserID.Length > 0)
            {
                string UserIdList = base.HaveUserIds;
                return dal.GetMySendTourInfo(pageSize, pageIndex, ref recordCount, UserIdList, TourSentTaskSearch);
            }
            return null;
        }

        /*/// <summary>
        /// 根据用户编号获取其运送团的信息
        /// </summary>
        /// <param name="UserID">用户编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourSentTask> GetMySendTourInfo(int[] UserID)
        {
            if (UserID != null && UserID.Length > 0)
            {
                return dal.GetMySendTourInfo(UserID);
            }
            return null;
        }*/

        /// <summary>
        /// 获取计划导游信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public IList<string> GetTourGuides(string tourId)
        {
            if (string.IsNullOrEmpty(tourId)) return null;

            return dal.GetTourGuides(tourId);
        }

        /// <summary>
        /// 获取网站热点线路计划信息集合
        /// </summary>
        /// <param name="companyId">公司（专线）编号</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <param name="tourDisplayType">团队展示方式</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBZTTours> GetToursSiteHot(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info, EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType tourDisplayType)
        {
            return this.GetToursZTD(companyId, pageSize, pageIndex, ref recordCount, info, tourDisplayType);
        }

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
        public IList<EyouSoft.Model.TourStructure.LBDJAPTourInfo> GetToursDJAP(int companyId, int supplierId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info)
        {
            if (companyId < 1 || supplierId < 1) return null;

            return dal.GetToursDJAP(companyId, supplierId, pageSize, pageIndex, ref recordCount, info);
        }

        /// <summary>
        /// 获取线路上团数汇总信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="routeId">线路编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.MRouteSTSCollectInfo GetRouteSTSCollectInfo(int companyId, int routeId)
        {
            if (companyId < 1 || routeId < 1) return null;

            return dal.GetRouteSTSCollectInfo(companyId, routeId);
        }

        /// <summary>
        /// 获取线路收客数汇总信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="routeId">线路编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.MRouteSKSCollectInfo GetRouteSKSCollectInfo(int companyId, int routeId)
        {
            if (companyId < 1 || routeId < 1) return null;

            return dal.GetRouteSKSCollectInfo(companyId, routeId);
        }

        /// <summary>
        /// 获取单项服务供应商安排信息业务实体
        /// </summary>
        /// <param name="planId">供应商安排编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.PlanSingleInfo GetSinglePlanInfo(string planId)
        {
            if (string.IsNullOrEmpty(planId)) return null;

            return dal.GetSinglePlanInfo(planId);
        }

        /// <summary>
        /// 获取计划类型
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public EyouSoft.Model.EnumType.TourStructure.TourType? GetTourType(string tourId)
        {
            if (string.IsNullOrEmpty(tourId)) return null;

            return dal.GetTourType(tourId);
        }

        /// <summary>
        /// 根据团队Id获取团队计划人数和价格信息
        /// </summary>
        /// <param name="tourId">团队Id</param>
        /// <returns>团队计划人数和价格信息</returns>
        public Model.TourStructure.MTourTeamUnitInfo GetTourTeamUnit(string tourId)
        {
            if (string.IsNullOrEmpty(tourId))
                return null;

            return dal.GetTourTeamUnit(tourId);
        }

        /// <summary>
        /// 设置团队推广状态
        /// </summary>
        /// <param name="tourRouteStatus">团队推广状态</param>
        /// <param name="touId">团队Id</param>
        /// <returns>1:成功；其他失败</returns>
        public int SetTourRouteStatus(Model.EnumType.TourStructure.TourRouteStatus tourRouteStatus, params string[] touId)
        {
            if (touId == null || touId.Length <= 0)
                return 0;

            return dal.SetTourRouteStatus(tourRouteStatus, touId);
        }

        /// <summary>
        /// 设置计划的手动设置状态
        /// </summary>
        /// <param name="handStatus">计划的手动设置状态</param>
        /// <param name="touId">团队Id</param>
        /// <returns>1:成功；其他失败</returns>
        public int SetHandStatus(Model.EnumType.TourStructure.HandStatus handStatus, params string[] touId)
        {
            if (touId == null || touId.Length <= 0)
                return 0;

            return dal.SetHandStatus(handStatus, touId);
        }

        /// <summary>
        /// 根据子系统公司编号与出团日期，删除计划数据
        /// </summary>
        /// <param name="SysID">子系统公司编号</param>
        /// <param name="LDateStart">出团日期开始</param>
        /// <param name="LDateEnd">出团日期结束</param>
        /// <param name="str"></param>
        /// <returns></returns>
        public void ClearTour(int SysID, DateTime LDateStart, DateTime LDateEnd, ref string str)
        {
            IList<string> list = null;
            if (!(SysID.Equals(0)))
                list = dal.ClearTour(SysID, LDateStart, LDateEnd);
            if (list != null && list.Count > 0)
            {
                foreach (string s in list)
                {
                    if (!Delete(s))
                    {
                        str += s + ",";
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定计划编号所属母团下子团信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="tourId">计划编号</param>
        /// <param name="sLDate">出团起始时间</param>
        /// <param name="eLDate">出团截止时间</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.MRiLiTourInfo> GetToursRiLi(int companyId, string tourId, DateTime sLDate, DateTime eLDate)
        {
            if (companyId == 0 || string.IsNullOrEmpty(tourId)) return null;

            if (sLDate == DateTime.MinValue || sLDate == DateTime.MaxValue) sLDate = DateTime.Now;
            if (eLDate == DateTime.MinValue || eLDate == DateTime.MaxValue) eLDate = sLDate.AddMonths(1);

            var items= dal.GetToursRiLi(companyId, tourId, sLDate, eLDate);

            return items;
        }

        /// <summary>
        /// 获取计划剩余人数
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public int GetShengYuRenShu(string tourId)
        {
            if (string.IsNullOrEmpty(tourId)) return 0;

            return dal.GetShengYuRenShu(tourId);
        }

        /// <summary>
        /// 获取团队计划列表合计信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public void GetToursTeamHeJi(int companyId, EyouSoft.Model.TourStructure.TourSearchInfo info, out int renShuHeJi, out decimal tuanKuanJinEHeJi)
        {
            renShuHeJi = 0; tuanKuanJinEHeJi = 0;

            if (companyId < 1) return;

            info = info ?? new EyouSoft.Model.TourStructure.TourSearchInfo();
            //取公司配置的列表显示控制配置
            info = info ?? new EyouSoft.Model.TourStructure.TourSearchInfo();
            if (!(info.SDate.HasValue || info.EDate.HasValue))
            {
                EyouSoft.Model.CompanyStructure.CompanyFieldSetting config = this.GetCompanyConfig(companyId);
                info.SDate = DateTime.Now.AddMonths(-config.DisplayBeforeMonth);
                info.EDate = DateTime.Now.AddMonths(config.DisplayAfterMonth);
            }

            dal.GetToursTeamHeJi(companyId, info, this.HaveUserIds, out renShuHeJi, out tuanKuanJinEHeJi);
        }
        #endregion
    }
}
