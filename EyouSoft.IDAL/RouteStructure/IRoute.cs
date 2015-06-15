/*Author:汪奇志 2011-01-21*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.RouteStructure
{
    /// <summary>
    /// 线路数据访问类接口
    /// </summary>
    /// Author:汪奇志 2011-01-21
    public interface IRoute
    {
        /// <summary>
        /// 写入线路库信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">线路信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        int InsertRouteInfo(EyouSoft.Model.RouteStructure.RouteInfo info);
        /// <summary>
        /// 更新线路库信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">线路信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        int UpdateRouteInfo(EyouSoft.Model.RouteStructure.RouteInfo info);
        /// <summary>
        /// 删除线路信息
        /// </summary>
        /// <param name="routeIds">线路编号集合</param>
        /// <returns></returns>
        bool Delete(string routeIds);
        /// <summary>
        /// 获取线路信息
        /// </summary>
        /// <param name="routeId">线路编号</param>
        /// <returns></returns>
        EyouSoft.Model.RouteStructure.RouteInfo GetRouteInfo(int routeId);
        /// <summary>
        /// 获取线路信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <param name="ReleaseType">线路发布类型</param>
        /// <returns></returns>
        IList<EyouSoft.Model.RouteStructure.RouteBaseInfo> GetRoutes(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.RouteStructure.RouteSearchInfo info, string us, EyouSoft.Model.EnumType.TourStructure.ReleaseType? ReleaseType);
    }
}
