using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 专线商公司账户信息IDAL
    /// </summary>
    /// 创建人：luofx 2011-01-17
    /// 修改：xuqh 2011-01-22 Add Method
    public interface ICompanyInfo
    {
        /// <summary>
        /// 获取公司信息实体
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SystemId">系统编号</param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.CompanyInfo GetModel(int CompanyId, int SystemId);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">公司信息实体</param>
        /// <returns></returns>
        bool Update(EyouSoft.Model.CompanyStructure.CompanyInfo model);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">公司信息实体</param>
        /// <returns></returns>
        bool Add(EyouSoft.Model.CompanyStructure.CompanyInfo model);
    }
}
