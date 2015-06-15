using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-规章制度IDAL
    /// 创建人：luofx 2011-01-19
    /// </summary>
    public interface IRuleInfo
    {
        /// <summary>
        /// 获取规章制度实体信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">主键Id</param>
        /// <returns></returns>
        EyouSoft.Model.AdminCenterStructure.RuleInfo GetModel(int CompanyId, int Id);
        /// <summary>
        /// 获取规章制度信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="RuleNo">规章制度编号</param>
        /// <param name="Title">规章制度标题</param>      
        /// <returns></returns>
        IList<EyouSoft.Model.AdminCenterStructure.RuleInfo> GetList(int PageSize, int PageIndex, ref int RecordCount,int CompanyId, string RuleNo, string Title);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">合同信息实体</param>
        /// <returns></returns>
        bool Add(EyouSoft.Model.AdminCenterStructure.RuleInfo model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">合同信息实体</param>
        /// <returns></returns>
        bool Update(EyouSoft.Model.AdminCenterStructure.RuleInfo model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">规章制度编号</param>
        /// <returns></returns>
        bool Delete(int CompanyId, int Id);
    }
}
