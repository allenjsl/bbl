using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 公司客户等级数据层接口
    /// </summary>
    /// 鲁功源 2011-01-21
    public interface ICompanyCustomStand
    {
        /// <summary>
        /// 验证是否已经存在同名的客户等级
        /// </summary>
        /// <param name="CustomStandName">客户等级名称</param>
        /// <param name="Id">主键编号</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>true:存在 false:不存在</returns>
        bool IsExists(string CustomStandName, int Id, int CompanyId);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">客户等级实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(EyouSoft.Model.CompanyStructure.CustomStand model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">客户等级实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Update(EyouSoft.Model.CompanyStructure.CustomStand model);
        /// <summary>
        /// 获取客户等级实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.CustomStand GetModel(int Id);
        /// <summary>
        /// 删除客户等级
        /// </summary>
        /// <param name="Ids">主键编号</param>
        /// <returns>true:成功 false:失败</returns>
        bool Delete(params int[] Ids);
        /// <summary>
        /// 分页获取公司客户等级集合
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>公司客户等级集合</returns>
        IList<EyouSoft.Model.CompanyStructure.CustomStand> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId);

        /// <summary>
        /// 根据公司编号获取客户等级信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.CustomStand> GetCustomStandByCompanyId(int companyId);
    }
}
