using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.TourStructure
{
    /// <summary>
    /// 散客天天发计划业务逻辑类
    /// </summary>
    /// Author:汪奇志 2011-03-18
    public class TourEveryday : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.TourStructure.ITourEveryday dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.TourStructure.ITourEveryday>();

        #region constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public TourEveryday() { }

        /// <summary>
        /// constructor with specified initial value
        /// </summary>
        /// <param name="uinfo">当前登录用户信息</param>
        public TourEveryday(EyouSoft.SSOComponent.Entity.UserInfo uinfo)
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
        /// <param name="isZTD">是否启用组织机构</param>
        public TourEveryday(EyouSoft.SSOComponent.Entity.UserInfo uinfo, bool isEnableOrganizations)
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
        /// 获取散客天天发计划处理申请信息价格处理，价格为申请时所选中的报价标准、客户等级的价格
        /// </summary>
        /// <param name="items"></param>
        private void HandleTourEverydayHandleApplysPrice(IList<EyouSoft.Model.TourStructure.LBTourEverydayHandleInfo> items)
        {
            //价格处理
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    var priceStandards = dal.GetTourEverydayPriceStandards(item.TourId);
                    if (priceStandards == null || priceStandards.Count < 1) continue;
                    foreach (var priceStandard in priceStandards)
                    {
                        if (priceStandard.StandardId == item.StandardId)
                        {
                            if (priceStandard.CustomerLevels == null || priceStandard.CustomerLevels.Count < 1) continue;
                            foreach (var level in priceStandard.CustomerLevels)
                            {
                                if (level.LevelId == item.LevelId)
                                {
                                    item.AdultPrice = level.AdultPrice;
                                    item.ChildrenPrice = level.ChildrenPrice;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

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
        /// 写入散客天天发计划信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">散客天天发计划信息业务实体</param>
        /// <returns></returns>
        public int InsertTourEverydayInfo(EyouSoft.Model.TourStructure.TourEverydayInfo info)
        {
            if (info == null) return 0;
            if (info.CompanyId < 0) return -1;
            if (info.AreaId < 1) return -2;
            if (info.TourDays < 1) return -3;
            if (info.OperatorId < 1) return -4;
            if (info.PriceStandards == null) return -5;

            info.TourId = Guid.NewGuid().ToString();
            info.CreateTime = DateTime.Now;

            int dalExceptionCode=dal.InsertTourEverydayInfo(info);

            if (dalExceptionCode <= 0) return dalExceptionCode;

            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散客天天发.ToString() + "新增了散客天天发计划数据，计划编号为：" + info.TourId;
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "添加散客天天发计划";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散客天天发;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            #endregion

            return dalExceptionCode;            
        }

        /// <summary>
        /// 更新散客天天发计划信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">散客天天发计划信息业务实体</param>
        /// <returns></returns>
        public int UpdateTourEverydayInfo(EyouSoft.Model.TourStructure.TourEverydayInfo info)
        {
            if (info == null) return 0;
            if (string.IsNullOrEmpty(info.TourId)) return -1;
            if (info.AreaId < 1) return -2;
            if (info.TourDays < 1) return -3;
            if (info.OperatorId < 1) return -4;
            if (info.PriceStandards == null) return -5;

            int dalExceptionCode = dal.UpdateTourEverydayInfo(info);
            if (dalExceptionCode <= 0) return dalExceptionCode;

            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散客天天发.ToString() + "修改了散客天天发计划数据，计划编号为：" + info.TourId;
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "修改散客天天发计划";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散客天天发;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            #endregion

            return dalExceptionCode;            
        }

        /// <summary>
        /// 删除散客天天发计划信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">天天发计划编号</param>
        /// <returns></returns>
        public int DeleteTourEverydayInfo(string tourId)
        {
            if (string.IsNullOrEmpty(tourId)) return 0;
            int dalExceptionCode = dal.DeleteTourEverydayInfo(tourId);

            if (dalExceptionCode <= 0) return dalExceptionCode;

            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散客天天发.ToString() + "删除了散客天天发计划数据，计划编号为：" +tourId;
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "删除散客天天发计划";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散客天天发;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            #endregion

            return dalExceptionCode;
        }

        /// <summary>
        /// 获取散客天天发计划信息业务实体
        /// </summary>
        /// <param name="tourId">天天发计划编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.TourEverydayInfo GetTourEverydayInfo(string tourId)
        {
            if (string.IsNullOrEmpty(tourId)) return null;
            return dal.GetTourEverydayInfo(tourId);
        }

        /// <summary>
        /// 获取散客天天发计划信息集合
        /// </summary>
        /// <param name="companyId">公司（专线）编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="queryInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBTourEverydayInfo> GetTourEverydays(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourEverydaySearchInfo queryInfo)
        {
            if (companyId < 1) return null;

            var items = dal.GetTourEverydays(companyId, pageSize, pageIndex, ref recordCount, queryInfo);

            #region 价格处理
            //价格处理 取第一个报价等级的门市及同行成人儿童价
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    var priceStandards = dal.GetTourEverydayPriceStandards(item.TourId);
                    if (priceStandards != null && priceStandards.Count > 0)
                    {
                        var price = priceStandards[0];

                        if (price.CustomerLevels != null && price.CustomerLevels.Count > 0)
                        {
                            foreach (var tmp in price.CustomerLevels)
                            {
                                if (tmp.LevelType == EyouSoft.Model.EnumType.CompanyStructure.CustomLevType.门市)
                                {
                                    item.MSAdultPrice = tmp.AdultPrice;
                                    item.MSChildrePricen = tmp.ChildrenPrice;
                                }

                                if (tmp.LevelType == EyouSoft.Model.EnumType.CompanyStructure.CustomLevType.同行)
                                {
                                    item.THAdultPrice = tmp.AdultPrice;
                                    item.THChildrenPrice = tmp.ChildrenPrice;
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            return items;
        }

        /// <summary>
        /// 申请散客天天发计划，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">散客天天发计划申请信息业务实体</param>
        /// <returns></returns>
        public int ApplyTourEveryday(EyouSoft.Model.TourStructure.TourEverydayApplyInfo info)
        {
            if (info == null) return 0;
            if (info.ApplyCompanyId < 1) return -1;
            if (info.LDate == DateTime.MinValue) return -2;
            if (info.LevelId < 1) return -3;
            if (info.OperatorId < 1) return -4;
            if (info.StandardId < 1) return -5;
            if (string.IsNullOrEmpty(info.TourId)) return -6;
            if (info.PeopleNumber < 1) return -7;

            info.ApplyId = Guid.NewGuid().ToString();
            if (info.Traveller != null && info.Traveller.Travellers != null && info.Traveller.Travellers.Count > 0)
            {
                foreach (var item in info.Traveller.Travellers)
                {
                    item.ID = Guid.NewGuid().ToString();
                }
            }

            return dal.ApplyTourEveryday(info);
        }

        /// <summary>
        /// 获取散客天天发计划申请信息业务实体
        /// </summary>
        /// <param name="applyId">申请编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.TourEverydayApplyInfo GetTourEverydayApplyInfo(string applyId)
        {
            if (string.IsNullOrEmpty(applyId)) return null;

            return dal.GetTourEverydayApplyInfo(applyId);
        }

        /// <summary>
        /// 获取散客天天发计划申请游客信息业务实体
        /// </summary>
        /// <param name="applyId">申请编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.TourEverydayApplyTravellerInfo GetTourEverydayApplyTravellerInfo(string applyId)
        {
            var info = this.GetTourEverydayApplyInfo(applyId);
            if (info != null)
            {
                return info.Traveller;
            }

            return null;
        }

        /// <summary>
        /// 获取散客天天发计划已处理申请信息集合
        /// </summary>
        /// <param name="companyId">公司（专线）编号</param>
        /// <param name="tourId">天天发计划编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBTourEverydayHandleInfo> GetTourEverydayHandleApplys(int companyId, string tourId, int pageSize, int pageIndex, ref int recordCount)
        {
            if (companyId < 0 || string.IsNullOrEmpty(tourId)) return null;

            var items = dal.GetTourEverydayHandleApplys(companyId, tourId, pageSize, pageIndex, ref recordCount, EyouSoft.Model.EnumType.TourStructure.TourEverydayHandleStatus.已处理, null);

            this.HandleTourEverydayHandleApplysPrice(items);

            return items;
        }

        /// <summary>
        /// 获取散客天天发计划未处理申请信息集合
        /// </summary>
        /// <param name="companyId">公司（专线）编号</param>
        /// <param name="tourId">天天发计划编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBTourEverydayHandleInfo> GetTourEverydayUntreatedApplys(int companyId, string tourId, int pageSize, int pageIndex, ref int recordCount)
        {
            if (companyId < 0 || string.IsNullOrEmpty(tourId)) return null;

            var items = dal.GetTourEverydayHandleApplys(companyId, tourId, pageSize, pageIndex, ref recordCount, EyouSoft.Model.EnumType.TourStructure.TourEverydayHandleStatus.未处理, null);

            this.HandleTourEverydayHandleApplysPrice(items);

            return items;
        }

        /// <summary>
        /// 散客天天发计划生成散拼计划，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourInfo">散拼计划信息业务实体</param>
        /// <param name="everydayTourId">天天发计划编号</param>
        /// <param name="everydayTourApplyId">天天发计划申请编号</param>
        /// <returns></returns>
        public int BuildTour(EyouSoft.Model.TourStructure.TourInfo tourInfo, string everydayTourId, string everydayTourApplyId)
        {
            int insertTourResult = 0, dalExceptionCode = 0, buyerCompanyId = 0;
            string buildTourId=string.Empty;

            EyouSoft.BLL.TourStructure.Tour tourbll = new Tour();
            insertTourResult = tourbll.InsertTourInfo(tourInfo);
            tourbll = null;

            if (insertTourResult > 0)
            {
                buildTourId = tourInfo.Childrens[0].ChildrenId;

                dalExceptionCode = dal.BuildTourFollow(buildTourId, everydayTourId, everydayTourApplyId, out buyerCompanyId);
            }
            else
            {
                return -109;
            }

            if (dalExceptionCode <= 0) return dalExceptionCode;

            #region stat.
            EyouSoft.BLL.UtilityStructure.Utility utilitybll = new EyouSoft.BLL.UtilityStructure.Utility();
            utilitybll.UpdateTradeNum(new int[] { buyerCompanyId });
            utilitybll = null;
            #endregion

            #region LGWR
            EyouSoft.Model.CompanyStructure.SysHandleLogs logInfo = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            logInfo.CompanyId = 0;
            logInfo.DepatId = 0;
            logInfo.EventCode = EyouSoft.Model.CompanyStructure.SysHandleLogsNO.EventCode;
            logInfo.EventIp = string.Empty;
            logInfo.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散客天天发.ToString() + "生成计划，生成的散拼计划编号为：" + buildTourId + "，天天发计划申请编号为：" + everydayTourApplyId + "，天天发计划编号为：" + everydayTourId;
            logInfo.EventTime = DateTime.Now;
            logInfo.EventTitle = "散客天天发计划生成散拼计划";
            logInfo.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.散拼计划_散客天天发;
            logInfo.OperatorId = 0;
            this.Logwr(logInfo);
            #endregion

            return dalExceptionCode;
        }
        #endregion
    }
}
