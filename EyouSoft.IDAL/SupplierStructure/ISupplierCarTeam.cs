using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.IDAL.CompanyStructure;

namespace EyouSoft.IDAL.SupplierStructure
{
    /// <summary>
    /// 车队供应商信息数据访问类接口
    /// </summary>
    /// Author:毛坤 2011-03-08
    public interface ISupplierCarTeam
    {
        /// <summary>
        /// 添加供应商车队信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddCarTeamAttach(EyouSoft.Model.SupplierStructure.SupplierCarTeam model);

        /// <summary>
        /// 修改供应商车队信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateCarTeamAttach(EyouSoft.Model.SupplierStructure.SupplierCarTeam model);

        /// <summary>
        /// 插入车队供应商的车辆信息集合
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="carsInfo"></param>
        /// <returns></returns>
        bool InsertCars(int supplierId, IList<EyouSoft.Model.SupplierStructure.SupplierCarInfo> carsInfo);

        /// <summary>
        /// 删除车队供应商车辆信息
        /// </summary>
        /// <param name="supplierId">供应商编号</param>
        /// <returns></returns>
        bool DeleteCars(int supplierId);

        /// <summary>
        /// 获取车队供应商信息(含车辆信息集合,不含供应商基本信息)
        /// </summary>
        /// <param name="supplierId">供应商编号</param>
        /// <returns></returns>
        EyouSoft.Model.SupplierStructure.SupplierCarTeam GetCarTeamAttachInfo(int supplierId);

        /// <summary>
        /// 获取车队供应商信息集合
        /// </summary>
        /// <param name="companyId">车队供应商编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.SupplierStructure.SupplierCarTeam> GetCarTeams(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.SupplierStructure.SupplierCarTeamSearchInfo info);
    }
}
