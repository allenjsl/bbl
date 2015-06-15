using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.FinanceStructure
{
    /// <summary>
    /// 财务管理-团队利润分配数据层接口
    /// </summary>
    /// 鲁功源 2011-01-22
    public interface ITourProfitShareInfo
    {
        /// <summary>
        /// 添加团队利润分配
        /// </summary>
        /// <param name="list">团队利润分配集合</param>
        /// <returns>1：成功 其它:失败</returns>
        int Add(IList<EyouSoft.Model.FinanceStructure.TourProfitShareInfo> list);
        /// <summary>
        /// 修改团队利润分配
        /// </summary>
        /// <param name="list">团队利润分配集合</param>
        /// <returns>1：成功 其它:失败</returns>
        int Update(IList<EyouSoft.Model.FinanceStructure.TourProfitShareInfo> list);
        /// <summary>
        /// 获取指定团队下的所有团队利润分配列表
        /// </summary>
        /// <param name="TourId">团队编号</param>
        /// <returns>团队利润分配列表</returns>
        IList<EyouSoft.Model.FinanceStructure.TourProfitShareInfo> GetList(string TourId);
        /// <summary>
        /// 获取团队利润分配实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns>团队利润分配实体</returns>
        EyouSoft.Model.FinanceStructure.TourProfitShareInfo GetModel(string Id);
    }
}
