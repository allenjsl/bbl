using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SupplierStructure
{
    /// <summary>
    /// 餐馆供应商数据类接口
    /// </summary>
    /// Author:汪奇志 2011-03-08
    public interface ISupplierRestaurant
    {
        /// <summary>
        /// 写入餐馆供应商附加信息
        /// </summary>
        /// <param name="info">餐馆供应商信息业务实体</param>
        /// <returns></returns>
        bool InsertRestaurantPertain(EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo info);
        /// <summary>
        /// 更新餐馆供应商附加信息
        /// </summary>
        /// <param name="info">餐馆供应商信息业务实体</param>
        /// <returns></returns>
        bool UpdateRestaurantPertain(EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo info);        
        /// <summary>
        /// 获取餐馆供应商附加信息(不含供应商基本信息)
        /// </summary>
        /// <param name="supplierId">供应商编号</param>
        /// <returns></returns>
        EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo GetRestaurantInfo(int supplierId);
        /// <summary>
        /// 获取餐馆供应商信息集合
        /// </summary>
        /// <param name="companyId">公司(专线)编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.SupplierStructure.SupplierRestaurantInfo> GetRestaurants(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.SupplierStructure.SupplierRestaurantSearchInfo info);
    }
}
