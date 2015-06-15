using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SupplierStructure
{
    /// <summary>
    /// 保险供应商IDAL
    /// 创建人：luofx 2011-03-8
    /// </summary>  
    public interface ISupplierInsurance
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">保险供应商实体</param>
        /// <returns></returns>
        bool Add(EyouSoft.Model.SupplierStructure.SupplierInsurance model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">保险供应商实体</param>
        /// <returns></returns>
        bool Update(EyouSoft.Model.SupplierStructure.SupplierInsurance model);

        /// <summary>
        /// 获取所属保险供应商信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="SearchInfo">搜索实体</param>
        /// <returns></returns>
        IList<EyouSoft.Model.SupplierStructure.SupplierInsurance> GetList(int PageSize, int PageIndex, ref int RecordCount,int CompanyId, EyouSoft.Model.SupplierStructure.SupplierQuery SearchInfo);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="Id">保险供应商编号（主键）</param>
        /// <param name="CompanyId">所属公司编号</param>
        /// <returns></returns>
        EyouSoft.Model.SupplierStructure.SupplierInsurance GetModel(int Id, int CompanyId);
    }
}
