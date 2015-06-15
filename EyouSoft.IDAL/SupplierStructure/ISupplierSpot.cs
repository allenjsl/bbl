using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.SupplierStructure
{
    /// <summary>
    /// 供应商-景点数据层接口
    /// </summary>
    /// 鲁功源  2011-03-08
    public interface ISupplierSpot
    {
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list">供应商景点实体集合</param>
        /// <returns></returns>
        bool Add(IList<EyouSoft.Model.SupplierStructure.SupplierSpot> list);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">供应商景点实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Update(EyouSoft.Model.SupplierStructure.SupplierSpot model);
        /// <summary>
        /// 获取供应商景点实体
        /// </summary>
        /// <param name="Id">景点主键编号</param>
        /// <param name="model">景点实体</param>
        void GetModel(int Id, ref EyouSoft.Model.SupplierStructure.SupplierSpot model);
        /// <summary>
        /// 分页获取供应商景点
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页索引</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">所属公司编号</param>
        /// <param name="query">供应商查询实体</param>
        /// <returns>供应商景点列表</returns>
        IList<EyouSoft.Model.SupplierStructure.SupplierSpot> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, EyouSoft.Model.SupplierStructure.SupplierQuery query);

        /// <summary>
        /// 获取供应商景点信息
        /// </summary>
        /// <param name="TopNum">top条数</param>
        /// <param name="CompanyId">所属公司编号</param>
        /// <param name="query">供应商查询实体</param>
        /// <returns></returns>
        IList<EyouSoft.Model.SupplierStructure.SupplierSpot> GetList(int TopNum, int CompanyId
            , EyouSoft.Model.SupplierStructure.SupplierQuery query);
    }
}
