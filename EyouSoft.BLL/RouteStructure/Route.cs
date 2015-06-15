/*Author:汪奇志 2011-01-21*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.RouteStructure
{
    /// <summary>
    /// 线路库业务逻辑类
    /// </summary>
    /// Author:汪奇志 2011-01-21
    public class Route:EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.RouteStructure.IRoute dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.RouteStructure.IRoute>();
        /// <summary>
        /// 系统操作日志逻辑
        /// </summary>
        private readonly BLL.CompanyStructure.SysHandleLogs HandleLogsBll = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();

        #region constructure
        /// <summary>
        /// default constructure
        /// </summary>
        public Route() { }

        /// <summary>
        /// constructor with specified initial value
        /// </summary>
        /// <param name="uinfo">login user info</param>
        public Route(EyouSoft.SSOComponent.Entity.UserInfo uinfo)
        {
            if (uinfo != null)
            {
                base.DepartIds = uinfo.Departs;
                base.CompanyId = uinfo.CompanyID;
            }
        }
        #endregion

        #region 成员方法
        /// <summary>
        /// 写入线路库信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">线路信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int InsertRouteInfo(EyouSoft.Model.RouteStructure.RouteInfo info)
        {
            if (info == null)
                return 0;
            int Result= dal.InsertRouteInfo(info);
            if (Result>0)
            {
                HandleLogsBll.Add(
                       new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                       {
                           ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.线路产品库_线路产品库,
                           EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                           EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.线路产品库_线路产品库.ToString() + "新增线路！编号为：" + info.RouteId,
                           EventTitle = "新增" + Model.EnumType.CompanyStructure.SysPermissionClass.线路产品库_线路产品库.ToString() + "数据"
                       });
            }
            return Result;
        }

        /// <summary>
        /// 更新线路库信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">线路信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int UpdateRouteInfo(EyouSoft.Model.RouteStructure.RouteInfo info)
        {
            if (info == null)
                return 0;
            int Result= dal.UpdateRouteInfo(info);
            if (Result > 0)
            {
                HandleLogsBll.Add(
                       new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                       {
                           ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.线路产品库_线路产品库,
                           EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                           EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.线路产品库_线路产品库.ToString() + "修改线路！编号为：" + info.RouteId,
                           EventTitle = "修改" + Model.EnumType.CompanyStructure.SysPermissionClass.线路产品库_线路产品库.ToString() + "数据"
                       });
            }
            return Result;
        }

        /// <summary>
        /// 删除线路信息
        /// </summary>
        /// <param name="routeId">线路编号</param>
        /// <returns></returns>
        public bool Delete(params int[] routeId)
        {
            if (routeId==null || routeId.Length==0)
                return false;
            string RouteIds = string.Empty;
            foreach (var item in routeId)
            {
                RouteIds += item.ToString() + ",";
            }
            RouteIds = RouteIds.TrimEnd(',');
            bool Result = dal.Delete(RouteIds);
            if (Result)
            {
                HandleLogsBll.Add(
                       new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                       {
                           ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.线路产品库_线路产品库,
                           EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                           EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.线路产品库_线路产品库.ToString() + "删除线路！编号为：" + RouteIds,
                           EventTitle = "删除" + Model.EnumType.CompanyStructure.SysPermissionClass.线路产品库_线路产品库.ToString() + "数据"
                       });
            }
            return Result;
        }

        /// <summary>
        /// 获取线路信息
        /// </summary>
        /// <param name="routeId">线路编号</param>
        /// <returns></returns>
        public EyouSoft.Model.RouteStructure.RouteInfo GetRouteInfo(int routeId)
        {
            if (routeId <= 0)
                return null;
            return dal.GetRouteInfo(routeId);
        }

        /// <summary>
        /// 获取所有线路信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.RouteStructure.RouteBaseInfo> GetRoutes(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.RouteStructure.RouteSearchInfo info)
        {
            return dal.GetRoutes(companyId, pageSize, pageIndex, ref recordCount, info, base.HaveUserIds, null);
        }
        /// <summary>
        /// 获取快速版线路信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.RouteStructure.RouteBaseInfo> GetQuickRoutes(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.RouteStructure.RouteSearchInfo info)
        {
            return dal.GetRoutes(companyId, pageSize, pageIndex, ref recordCount, info, base.HaveUserIds, EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick);
        }
        /// <summary>
        /// 获取标准版线路信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.RouteStructure.RouteBaseInfo> GetNormalRoutes(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.RouteStructure.RouteSearchInfo info)
        {
            return dal.GetRoutes(companyId, pageSize, pageIndex, ref recordCount, info, base.HaveUserIds, EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal);
        }
        #endregion
    }
}
