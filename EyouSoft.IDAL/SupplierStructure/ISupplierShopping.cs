using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SupplierStructure
{
    /// <summary>
    /// 供应商购物操作接口
    /// author:xuqh 2011-03-08
    /// </summary>
    public interface ISupplierShopping
    {
        /// <summary>
        /// 添加供应商-购物信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="supplierId">供应商基本信息ID</param>
        /// <returns></returns>
        bool AddShopping(EyouSoft.Model.SupplierStructure.SupplierShopping model,int supplierId);

        /// <summary>
        /// 修改供应商购物信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="supplierId">供应商基本信息ID</param>
        /// <returns></returns>
        bool UpdateShopping(EyouSoft.Model.SupplierStructure.SupplierShopping model, int supplierId);

        /// <summary>
        /// 删除供应商购物信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteShopping(params int[] ids);

        /// <summary>
        /// 获取供应商购物实体
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        EyouSoft.Model.SupplierStructure.SupplierShopping GetModel(int id);

        /// <summary>
        /// 批量导入数据
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        bool ImportExcelData(List<EyouSoft.Model.SupplierStructure.SupplierShopping> ls);

        /// <summary>
        /// 分页获取供应商购物信息
        /// </summary>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">起始页码</param>
        /// <param name="recordCount">总数</param>
        /// <param name="supplierType">供应商类型</param>
        /// <param name="provinceId">省份ID</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="shopName">商店名称</param>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        IList<EyouSoft.Model.SupplierStructure.SupplierShopping> GetList(int pageSize, int pageIndex, ref int recordCount,int companyId, EyouSoft.Model.EnumType.CompanyStructure.SupplierType supplierType,EyouSoft.Model.SupplierStructure.SupplierQuery queryModel);
    }
}
