using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.PlanStruture
{
    /// <summary>
    /// 交通管理业务逻辑类
    /// </summary>
    /// Author:李晓欢 2012-09-10
    public class PlanTrffic : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.PlanStruture.IPlanTrffic Idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.PlanStruture.IPlanTrffic>();
        /// <summary>
        /// 系统操作日志逻辑
        /// </summary>
        private readonly BLL.CompanyStructure.SysHandleLogs HandleLogsBll = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();

        #region constructure
        /// <summary>
        /// default constructure
        /// </summary>
        public PlanTrffic() { }

        /// <summary>
        /// constructor with specified initial value
        /// </summary>
        /// <param name="uinfo">login user info</param>
        public PlanTrffic(EyouSoft.SSOComponent.Entity.UserInfo uinfo)
        {
            if (uinfo != null)
            {
                base.DepartIds = uinfo.Departs;
                base.CompanyId = uinfo.CompanyID;
            }
        }
        #endregion

        #region 成员
        /// <summary>
        /// 添加交通
        /// </summary>
        /// <param name="trfficModel">交通实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool AddPlanTraffic(EyouSoft.Model.PlanStructure.TrafficInfo trafficModel)
        {
            if (trafficModel == null) return false;
            bool result = Idal.AddPlanTraffic(trafficModel);
            if (result)
            {
                HandleLogsBll.Add(
                     new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                     {
                         ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理,
                         EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                         EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理.ToString() + "新增交通！编号为：" + trafficModel.TrafficId,
                         EventTitle = "新增" + Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理.ToString() + "数据"
                     });
            }
            return result;
        }

        /// <summary>
        /// 修改交通
        /// </summary>
        /// <param name="trfficModel">交通实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool UpdatePlanTraffic(EyouSoft.Model.PlanStructure.TrafficInfo trafficModel)
        {
            if (trafficModel == null) return false;
            bool result = Idal.UpdatePlanTraffic(trafficModel);
            if (result)
            {
                HandleLogsBll.Add(
                       new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                       {
                           ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理,
                           EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                           EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理.ToString() + "修改交通！编号为：" + trafficModel.TrafficId,
                           EventTitle = "修改" + Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理.ToString() + "数据"
                       });
            }
            return result;
        }

        /// <summary>
        /// 删除交通
        /// </summary>
        /// <param name="trfficIds">交通编号集合</param>
        /// <returns>true:成功 false:失败</returns>
        public bool deletePlanTraffic(params int[] trafficIds)
        {
            if (trafficIds == null || trafficIds.Length == 0)
                return false;
            string tfIds = string.Empty;
            foreach (var itemID in trafficIds)
            {
                tfIds += Convert.ToInt32(itemID) + ",";
            }
            tfIds = tfIds.TrimEnd(',');
            bool Result = Idal.DeletePlanTraffic(tfIds);
            if (Result)
            {
                HandleLogsBll.Add(
                       new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                       {
                           ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理,
                           EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                           EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理.ToString() + "删除交通！编号为：" + tfIds,
                           EventTitle = "删除" + Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理.ToString() + "数据"
                       });
            }
            return Result;
        }

        /// <summary>
        /// 获取交通集合列表
        /// </summary>
        /// <param name="trffIcName">交通名称</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">当前页</param>
        /// <param name="CompanyID">公司编号</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="us">用户信息集合</param>
        /// <returns>所有交通集合</returns>
        public IList<EyouSoft.Model.PlanStructure.TrafficInfo> GetTrafficList(EyouSoft.Model.PlanStructure.TrafficSearch SearchModel, int PageSize, int PageIndex, int CompanyID, ref int RecordCount)
        {
            return Idal.GetTrafficList(SearchModel, PageSize, PageIndex, CompanyID, ref RecordCount);
        }


        /// <summary>
        /// 设置交通状态
        /// </summary>
        /// <param name="trffiCID">交通编号</param>
        /// <param name="status">当前状态</param>
        /// <returns>状态设置</returns>
        public bool UpdateChangeSatus(int traffiCID, EyouSoft.Model.EnumType.PlanStructure.TrafficStatus status)
        {
            return Idal.UpdateChangeSatus(traffiCID, status);
        }

        /// <summary>
        /// 获取交通实体
        /// </summary>
        /// <param name="trfficID">交通编号</param>
        /// <returns>返回交通实体</returns>
        public EyouSoft.Model.PlanStructure.TrafficInfo GetTrafficModel(int trafficID)
        {
            return Idal.GetTrafficModel(trafficID);
        }

        /// <summary>
        /// 获取交通集合列表
        /// </summary>
        /// <param name="SearchModel">查询实体</param>
        /// <param name="CompanyID">公司编号</param>
        /// <param name="us">用户信息集合</param>
        /// <returns>所有交通集合</returns>
        public IList<EyouSoft.Model.PlanStructure.TrafficInfo> GetTrafficList(EyouSoft.Model.PlanStructure.TrafficSearch SearchModel, int CompanyID)
        {
            return Idal.GetTrafficList(SearchModel, CompanyID);
        }

        /// <summary>
        /// 添加行程
        /// </summary>
        /// <param name="travelModel">行程实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool AddPlanTravel(EyouSoft.Model.PlanStructure.TravelInfo travelModel)
        {
            if (travelModel == null) return false;
            bool result = Idal.AddPlanTravel(travelModel);
            if (result)
            {
                HandleLogsBll.Add(
                     new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                     {
                         ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理,
                         EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                         EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理.ToString() + "新增行程！编号为：" + travelModel.TravelId,
                         EventTitle = "新增" + Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理.ToString() + "数据"
                     });
            }
            return result;
        }

        /// <summary>
        /// 获取交通行程
        /// </summary>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">总页数</param>
        /// <param name="CompanyID">公司编号</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="trfficID">交通编号</param>
        /// <returns>交通行程</returns>
        public IList<EyouSoft.Model.PlanStructure.TravelInfo> GettravelList(int PageSize, int PageIndex, int CompanyID, ref int RecordCount, int trafficID)
        {
            return Idal.GetTravelList(PageSize, PageIndex, CompanyId, ref RecordCount, trafficID);
        }

        /// <summary>
        /// 修改交通行程
        /// </summary>
        /// <param name="travelModel">行程实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool UpdatePlanTravel(EyouSoft.Model.PlanStructure.TravelInfo travelModel)
        {
            if (travelModel == null) return false;
            bool result = Idal.UpdatePlanTravel(travelModel);
            if (result)
            {
                HandleLogsBll.Add(
                     new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                     {
                         ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理,
                         EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                         EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理.ToString() + "修改行程！编号为：" + travelModel.TravelId,
                         EventTitle = "修改" + Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理.ToString() + "数据"
                     });
            }
            return result;
        }

        /// <summary>
        /// 获取行程实体
        /// </summary>
        /// <param name="travelID">行程编号</param>
        /// <returns></returns>
        public EyouSoft.Model.PlanStructure.TravelInfo GettravelModel(int travelID)
        {
            return Idal.GettravelModel(travelID);
        }

        /// <summary>
        /// 删除交通行程
        /// </summary>
        /// <param name="trfficIds">交通行程编号集合</param>
        /// <param name="trfficIds">交通编号</param>
        /// <returns>true:成功 false:失败</returns>
        public bool DeletePlanTravel(int trafficId, params int[] travelIds)
        {
            if (travelIds == null && travelIds.Length <= 0) return false;
            string ids = string.Empty;
            foreach (var item in travelIds)
            {
                ids += Convert.ToInt32(item) + ",";
            }
            ids = ids.TrimEnd(',');

            bool Result = Idal.DeletePlanTravel(ids, trafficId);
            if (Result)
            {
                HandleLogsBll.Add(
                       new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                       {
                           ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理,
                           EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                           EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理.ToString() + "删除交通行程！编号为：" + ids,
                           EventTitle = "删除" + Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理.ToString() + "数据"
                       });
            }
            return Result;
        }


        /// <summary>
        /// 获取交通出票统计信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="search">查询实体</param>
        /// <returns></returns>
        public IList<Model.PlanStructure.JiaoTongChuPiao> GetJiaoTongChuPiao(int companyId
            , Model.PlanStructure.JiaoTongChuPiaoSearch search)
        {
            if (companyId <= 0) return null;

            return Idal.GetJiaoTongChuPiao(companyId, search);
        }

        #endregion

        #region 价格成员
        /// <summary>
        /// 获取价格实体
        /// </summary>
        /// <param name="trffIcID">交通编号</param>
        /// <param name="LTourDate">出团时间</param>
        /// <returns>返回价格实体</returns>
        public EyouSoft.Model.PlanStructure.TrafficPricesInfo GetTrafficPriceModel(int traffIcID, DateTime LTourDate)
        {
            return Idal.GetTrafficPriceModel(traffIcID, LTourDate);
        }

        /// <summary>
        /// 根据交通编号出团时间获取价格集合
        /// </summary>
        /// <param name="trafficId">交通编号集合</param>
        /// <param name="LTourDate">出团时间</param>
        /// <returns>返回价格集合</returns>
        public IList<EyouSoft.Model.PlanStructure.TrafficPricesInfo> GetTrafficPriceList(DateTime LTourDate, params int[] trafficId)
        {
            if (trafficId == null && trafficId.Length <= 0) return null;
            //string ids = string.Empty;
            //foreach (var item in trafficId)
            //{
            //    ids += Convert.ToInt32(item) + ",";
            //}
            //ids = ids.TrimEnd(',');
            return Idal.GetTrafficPriceList(LTourDate, trafficId);
        }

        /// <summary>
        /// 添加价格
        /// </summary>
        /// <param name="pricemodel">价格实体</param>
        /// <returns>true 成功  false 失败</returns>
        public bool AddTrafficPrice(EyouSoft.Model.PlanStructure.TrafficPricesInfo pricemodel)
        {
            if (pricemodel == null) return false;
            pricemodel.PricesID = Guid.NewGuid().ToString();
            bool result = Idal.AddTrafficPrice(pricemodel);
            if (result)
            {
                this.TongBuGengXinTuanDui(pricemodel.TrafficId);
                HandleLogsBll.Add(
                     new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                     {
                         ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理,
                         EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                         EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理.ToString() + "添加交通价格！编号为：" + pricemodel.PricesID,
                         EventTitle = "修改" + Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理.ToString() + "数据"
                     });
            }
            return result;
        }

        /// <summary>
        /// 修改价格
        /// </summary>
        /// <param name="pricemodel">价格实体</param>
        /// <returns>true 成功 false 失败</returns>
        public bool UpdatetrafficPrice(EyouSoft.Model.PlanStructure.TrafficPricesInfo pricemodel)
        {
            if (pricemodel == null) return false;
            bool result = Idal.UpdateTrafficPrice(pricemodel);
            if (result)
            {
                this.TongBuGengXinTuanDui(pricemodel.TrafficId);
                HandleLogsBll.Add(
                     new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                     {
                         ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理,
                         EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                         EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理.ToString() + "更新交通价格！编号为：" + pricemodel.PricesID,
                         EventTitle = "修改" + Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理.ToString() + "数据"
                     });
            }
            return result;
        }

        /// <summary>
        /// 获取价格列表
        /// </summary>
        /// <param name="trfficId">交通编号</param>
        /// <returns>价格集合</returns>
        public IList<EyouSoft.Model.PlanStructure.TrafficPricesInfo> GetPricesList(int trafficId)
        {
            return Idal.GetPricesList(trafficId);
        }

        /// <summary>
        /// 获取价格实体
        /// </summary>
        /// <param name="pricesId">价格编号</param>
        /// <param name="trafficId">交通编号</param>
        /// <returns>价格实体</returns>
        public EyouSoft.Model.PlanStructure.TrafficPricesInfo GetTrafficPrices(string pricesId, int trafficId)
        {
            return Idal.GetTrafficPrices(pricesId, trafficId);
        }

        /// <summary>
        /// 交通某一天的价格修改后同步更新影响到的团队的信息(预控人数)
        /// </summary>
        /// <param name="trafficId">交通编号</param>
        public void TongBuGengXinTuanDui(int trafficId)
        {
            if (trafficId <= 0) return;

            Idal.TongBuGengXinTuanDui(trafficId);
        }

        /// <summary>
        /// 更新票状态
        /// </summary>
        /// <param name="pricesID">价格编号</param>
        ///  <param name="trafficId">交通编号</param>
        /// <param name="status">票状态</param>
        /// <returns>true 成功 false 失败</returns>
        public bool UpdatePricesStatus(string pricesID, int trafficId, EyouSoft.Model.EnumType.PlanStructure.TicketStatus status)
        {
            if (string.IsNullOrEmpty(pricesID)) return false;
            bool result = Idal.UpdatePricesStatus(pricesID, trafficId, status);
            if (result)
            {
                this.TongBuGengXinTuanDui(trafficId);
                HandleLogsBll.Add(
                     new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                     {
                         ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理,
                         EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                         EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理.ToString() + "更新价格状态！编号为：" + pricesID,
                         EventTitle = "修改" + Model.EnumType.CompanyStructure.SysPermissionClass.机票管理_交通管理.ToString() + "数据"
                     });
            }
            return result;
        }

        #endregion
    }
}
