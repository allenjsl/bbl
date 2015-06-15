using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.SupplierStructure
{
    /// <summary>
    /// 供应商购物BLL
    /// author:xuqh 2011-03-08
    /// </summary>
    public class SupplierShopping
    {
        private readonly EyouSoft.IDAL.SupplierStructure.ISupplierShopping Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.SupplierStructure.ISupplierShopping>();

        private readonly BLL.CompanyStructure.SysHandleLogs handleLogsBll = new BLL.CompanyStructure.SysHandleLogs();
        private readonly BLL.CompanyStructure.SupplierBaseHandle supplierBase = new EyouSoft.BLL.CompanyStructure.SupplierBaseHandle();

        #region 成员方法
        /// <summary>
        /// 添加供应商购物信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddShopping(EyouSoft.Model.SupplierStructure.SupplierShopping model)
        {
            int supplierId = supplierBase.AddSupplierBase(model);
            return Dal.AddShopping(model,supplierId);
        }

        /// <summary>
        /// 修改供应商购物信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateShopping(EyouSoft.Model.SupplierStructure.SupplierShopping model)
        {
            int successId = supplierBase.UpdateSupplierBase(model);

            if (successId == 1)
                return Dal.UpdateShopping(model, model.Id);
            else
                return false;
        }

        /// <summary>
        /// 删除供应商购物信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteShopping(params int[] ids)
        {
            return supplierBase.DeleteSupplierBase(ids);
        }

        /// <summary>
        /// 获取供应商购物实体
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public EyouSoft.Model.SupplierStructure.SupplierShopping GetModel(int id)
        {
            return Dal.GetModel(id);
        }

        /// <summary>
        /// 批量导入数据
        /// </summary>
        /// <param name="ls"></param>
        /// <returns></returns>
        public bool ImportExcelData(List<EyouSoft.Model.SupplierStructure.SupplierShopping> ls)
        {
            return Dal.ImportExcelData(ls);
        }

        /// <summary>
        /// 分页获取供应商购物信息
        /// </summary>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">起始页码</param>
        /// <param name="recordCount">总数</param>
        /// <param name="supplierType">供应商类型</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="shopName">商店名称</param>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SupplierStructure.SupplierShopping> GetList(int pageSize, int pageIndex, ref int recordCount,int companyId, EyouSoft.Model.EnumType.CompanyStructure.SupplierType supplierType, EyouSoft.Model.SupplierStructure.SupplierQuery queryModel)
        {
            return Dal.GetList(pageSize, pageIndex, ref recordCount, companyId, supplierType, queryModel);
        }
        #endregion
    }
}
