using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{ 
    /// <summary>
    /// 公司产品数据层接口
    /// </summary>
    /// 鲁功源 2011-01-21
    public interface ICompanyBrand
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">公司产品实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(EyouSoft.Model.CompanyStructure.CompanyBrand model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">公司产品实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Update(EyouSoft.Model.CompanyStructure.CompanyBrand model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">主键编号集合</param>
        /// <returns>true:成功 false:失败</returns>
        bool Delete(params int[] Ids);
        /// <summary>
        /// 获取公司产品实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.CompanyBrand GetModel(int Id);
        /// <summary>
        /// 分页获取公司产品列表
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>公司产品列表</returns>
        IList<EyouSoft.Model.CompanyStructure.CompanyBrand> GetList(int PageSize,int PageIndex,ref int RecordCount,int CompanyId);

        /// <summary>
        /// 根据公司ID获取公司品牌信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.CompanyBrand> GetBrandByCompanyId(int companyId);
    }
}
