using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-合同管理IDAL
    /// 创建人：luofx 2011-01-18
    /// </summary>
    public interface IContractInfo
    {
        /// <summary>
        /// 获取合同管理实体信息
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        EyouSoft.Model.AdminCenterStructure.ContractInfo GetModel(int CompanyId, int Id);
        /// <summary>
        /// 获取合同信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">合同管理查询实体</param>
        /// <returns></returns>
        IList<EyouSoft.Model.AdminCenterStructure.ContractInfo> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, EyouSoft.Model.AdminCenterStructure.ContractSearchInfo SearchInfo);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">合同信息实体</param>
        /// <returns></returns>
        bool Add(EyouSoft.Model.AdminCenterStructure.ContractInfo model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">合同信息实体</param>
        /// <returns></returns>
        bool Update(EyouSoft.Model.AdminCenterStructure.ContractInfo model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">合同信息编号</param>
        /// <returns></returns>
        bool Delete(int CompanyId,int Id);

        /// <summary>
        /// 获取合同到期N天列表
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="remindDay">到期提醒天数</param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        IList<EyouSoft.Model.PersonalCenterStructure.ContractReminder> GetContractRemind(int PageSize, int PageIndex, ref int RecordCount, int remindDay, int companyId);
    
        /// <summary>
        /// 获取合同到期总人数
        /// </summary>
        /// <param name="remindDay">到期天数</param>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        int GetTotalContractRemind(int remindDay, int companyId);
    }
}
