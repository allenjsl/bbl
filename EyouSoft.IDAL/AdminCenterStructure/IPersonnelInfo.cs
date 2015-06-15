using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.AdminCenterStructure
{
    /// <summary>
    /// 行政中心-人事档案IDAL
    /// 创建人：luofx 2011-01-17
    /// </summary>
    public interface IPersonnelInfo
    {
        /// <summary>
        /// 获取认识档案信息实体
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="PersonId">职员编号</param>
        /// <returns></returns>
        EyouSoft.Model.AdminCenterStructure.PersonnelInfo GetModel(int CompanyId, int PersonId);
        /// <summary>
        /// 获取人事档案列表信息
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="ReCordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">认识档案搜索实体</param>
        /// <returns></returns>
        IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> GetList(int PageSize, int PageIndex, ref int ReCordCount, int CompanyId, EyouSoft.Model.AdminCenterStructure.PersonnelSearchInfo SearchInfo);
        /// <summary>
        /// 获取通讯录信息
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="ReCordCount"></param>
        /// <param name="CompanyId"></param>
        /// <param name="UserName">姓名</param>
        /// <param name="DepartmentId">部门编号</param>
        /// <returns></returns>
        IList<EyouSoft.Model.AdminCenterStructure.PersonnelInfo> GetList(int PageSize, int PageIndex, ref int ReCordCount, int CompanyId,string UserName,int? DepartmentId);
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model">职工档案信息实体</param>
        /// <returns></returns>
        bool Add(EyouSoft.Model.AdminCenterStructure.PersonnelInfo model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">职工档案信息实体</param>
        /// <returns></returns>
        bool Update(EyouSoft.Model.AdminCenterStructure.PersonnelInfo model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="PersonId">员工编号</param>
        /// <returns></returns>
        bool Delete(int CompanyId, params int[] PersonId);
        /// <summary>
        /// 获取人事工资信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <param name="query">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.AdminCenterStructure.MWageInfo> GetWages(int companyId, int year, int month, EyouSoft.Model.AdminCenterStructure.MWageSearchInfo query);
        /// <summary>
        /// 按年月设置人事工资信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <param name="operatorId">操作人编号</param>
        /// <param name="wages">人事工资信息集合</param>
        /// <returns></returns>
        bool SetWages(int companyId, int year, int month, int operatorId, IList<EyouSoft.Model.AdminCenterStructure.MWageInfo> wages);
    }
}
