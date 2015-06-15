using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 公司省份数据访问接口
    /// </summary>
    /// 周文超   2011-01-19
    public interface ICompanyProvince
    {
        /// <summary>
        /// 获取某公司的所有省份
        /// </summary>
        /// <param name="CompanyId">公司Id</param>
        /// <returns></returns>
        IList<Model.CompanyStructure.Province> GetListByCompanyId(int CompanyId);

        /// <summary>
        /// 获取省份信息
        /// </summary>
        /// <param name="ProvinceId">省份Id</param>
        /// <returns></returns>
        Model.CompanyStructure.Province GetModel(int ProvinceId);
    }
}
