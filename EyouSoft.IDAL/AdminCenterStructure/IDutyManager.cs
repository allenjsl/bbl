using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-职务管理IDAL
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public interface IDutyManager
    {
        /// <summary>
        /// 获取职务信息实体
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="DutyId">职务编号</param>
        /// <returns></returns>
        EyouSoft.Model.AdminCenterStructure.DutyManager GetModel(int CompanyId, int DutyId);
        /// <summary>
        /// 获取公司职务信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.AdminCenterStructure.DutyManager> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId);
          /// <summary>
        /// 获取所有职务信息（职务名称和ID值）
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.AdminCenterStructure.DutyManager> GetList(int CompanyId);
        /// <summary>
        /// 判断是否已经有用户已经使用该职务
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="DutyId">职务编号</param>
        /// <returns></returns>
        bool IsHasBeenUsed(int CompanyId, int RoleId);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">公司账户信息实体</param>
        /// <returns>0:失败，1：成功，-1：职务名称重复</returns>
        int Add(EyouSoft.Model.AdminCenterStructure.DutyManager model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">公司账户信息实体</param>
        /// <returns>0:失败，1：成功，-1：职务名称重复</returns>
        int Update(EyouSoft.Model.AdminCenterStructure.DutyManager model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        bool Delete(int CompanyId, int DutyId);
    }
}
