using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-培训计划IDAL
    /// 创建人：luofx 2011-01-19
    /// </summary>
    public interface ITrainPlan
    {
        /// <summary>
        /// 获取培训计划实体信息
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        EyouSoft.Model.AdminCenterStructure.TrainPlan GetModel(int CompanyId, int Id);
        /// <summary>
        /// 获取培训计划信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>    
        /// <param name="UserId">当前用户编号</param> 
        /// <param name="DepartmentId">部门编号</param> 
        /// <returns></returns>
        IList<EyouSoft.Model.AdminCenterStructure.TrainPlan> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, int UserId, int DepartmentId);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">培训计划实体</param>
        /// <returns></returns>
        bool Add(EyouSoft.Model.AdminCenterStructure.TrainPlan model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">培训计划实体</param>
        /// <returns></returns>
        bool Update(EyouSoft.Model.AdminCenterStructure.TrainPlan model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        bool Delete(int CompanyId, int Id);
    }
}
