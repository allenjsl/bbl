using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SupplierStructure
{
    /// <summary>
    /// 酒店供应商信息数据访问类接口
    /// </summary>
    /// Author:汪奇志 2011-03-08
    public interface ISupplierHotel
    {
        /// <summary>
        /// 写入酒店供应商附加信息
        /// </summary>
        /// <param name="info">酒店供应商信息业务实体</param>
        /// <returns></returns>
        bool InsertHotelPertain(EyouSoft.Model.SupplierStructure.SupplierHotelInfo info);
        /// <summary>
        /// 更新酒店供应商附加信息
        /// </summary>
        /// <param name="info">酒店供应商信息业务实体</param>
        /// <returns></returns>
        bool UpdateHotelPertain(EyouSoft.Model.SupplierStructure.SupplierHotelInfo info);
        /// <summary>
        /// 写入酒店供应商房型信息集合
        /// </summary>
        /// <param name="roomTypes">房型信息集合</param>
        /// <returns></returns>
        bool InsertRoomTypes(int supplierId, IList<EyouSoft.Model.SupplierStructure.SupplierHotelRoomTypeInfo> roomTypes);
        /// <summary>
        /// 删除酒店供应商房型信息
        /// </summary>
        /// <param name="supplierId">供应商编号</param>
        /// <returns></returns>
        bool DeleteRoomType(int supplierId);
        /// <summary>
        /// 获取酒店供应商附加信息(含房型信息集合，不含供应商基本信息)
        /// </summary>
        /// <param name="supplierId">供应商编号</param>
        /// <returns></returns>
        EyouSoft.Model.SupplierStructure.SupplierHotelInfo GetHotelPertainInfo(int supplierId);
        /// <summary>
        /// 获取酒店供应商信息集合
        /// </summary>
        /// <param name="companyId">公司(专线)编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.SupplierStructure.SupplierHotelInfo> GetHotels(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.SupplierStructure.SupplierHotelSearchInfo info);

        /// <summary>
        /// 获取酒店供应商信息集合(供应商基本信息，图片信息，房型信息。同行登录口适用)
        /// </summary>
        /// <param name="companyId">公司(专线)编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.SupplierStructure.SupplierHotelInfo> GetSiteHotels(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.SupplierStructure.SupplierHotelSearchInfo info);
    }
}
