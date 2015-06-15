using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 公司报价等级数据层接口
    /// </summary>
    /// 鲁功源 2011-01-21
    public interface ICompanyPriceStand
    {
        /// <summary>
        /// 验证是否已经存在同名的报价等级
        /// </summary>
        /// <param name="PriceStandName">报价等级名称</param>
        /// <param name="Id">主键编号</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>true:存在 false:不存在</returns>
        bool IsExists(string PriceStandName, int Id, int CompanyId);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">报价等级实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(EyouSoft.Model.CompanyStructure.CompanyPriceStand model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">报价等级实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Update(EyouSoft.Model.CompanyStructure.CompanyPriceStand model);
        /// <summary>
        /// 获取报价等级实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.CompanyPriceStand GetModel(int Id);
        /// <summary>
        /// 删除报价等级
        /// </summary>
        /// <param name="Ids">主键编号</param>
        /// <returns>true:成功 false:失败</returns>
        bool Delete(params int[] Ids);
        /// <summary>
        /// 分页获取公司报价等级集合
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>公司报价等级集合</returns>
        IList<EyouSoft.Model.CompanyStructure.CompanyPriceStand> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId);

        /// <summary>
        /// 获取某公司所有报价等级信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.CompanyPriceStand> GetPriceStandByCompanyId(int companyId);

        /// <summary>
        /// 判断报价标准或客户等级是否被使用过
        /// </summary>
        /// <param name="id">报价标准或客户等级</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="typeId">1是标价标准 0是客户等级</param>
        /// <returns></returns>
        bool IsUsed(int id, int companyId, int typeId);
    }
}
