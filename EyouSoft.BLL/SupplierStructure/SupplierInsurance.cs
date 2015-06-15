using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.SupplierStructure
{
    /// <summary>
    /// 
    /// </summary>
    public class SupplierInsurance
    {
        private readonly EyouSoft.IDAL.SupplierStructure.ISupplierInsurance idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.SupplierStructure.ISupplierInsurance>();
        private EyouSoft.BLL.CompanyStructure.SupplierBaseHandle bll = new EyouSoft.BLL.CompanyStructure.SupplierBaseHandle();
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">保险供应商实体</param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.SupplierStructure.SupplierInsurance model)
        {
            return idal.Add(model);
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="lists">保险供应商信息集合</param>
        /// <returns>0:失败，>0：返回值等于List集合的Count值时，却不成功，不等于时，部分成功</returns>
        public int AddList(IList<EyouSoft.Model.SupplierStructure.SupplierInsurance> lists)
        {
            int RowCount = 0;
            foreach (EyouSoft.Model.SupplierStructure.SupplierInsurance model in lists)
            {
                if (this.Add(model))
                {
                    RowCount++;
                }
            }
            return RowCount;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">保险供应商实体</param>
        /// <returns></returns>
        public bool Update(EyouSoft.Model.SupplierStructure.SupplierInsurance model)
        {
            return idal.Update(model);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id">保险供应商编号（主键）</param>
        /// <returns></returns>
        public bool Delete(int[] Id)
        {
            return bll.DeleteSupplierBase(Id);
        }
        /// <summary>
        /// 获取所属保险供应商信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">搜索实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.SupplierStructure.SupplierInsurance> GetList(int PageSize, int PageIndex, ref int RecordCount,int CompanyId, EyouSoft.Model.SupplierStructure.SupplierQuery SearchInfo)
        {
            return idal.GetList(PageSize, PageIndex, ref RecordCount,CompanyId, SearchInfo);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="Id">保险供应商编号（主键）</param>
        /// <param name="CompanyId">所属公司编号</param>
        /// <returns></returns>
        public EyouSoft.Model.SupplierStructure.SupplierInsurance GetModel(int Id, int CompanyId)
        {
            return idal.GetModel(Id, CompanyId);
        }
    }
}
