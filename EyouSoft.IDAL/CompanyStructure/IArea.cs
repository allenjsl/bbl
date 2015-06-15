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
    public interface IArea
    {
        /// <summary>
        /// 验证是否已经存在同名的线路区域
        /// </summary>
        /// <param name="AreaName">线路区域名称</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>true:存在 false:不存在</returns>
        bool IsExists(string AreaName, int CompanyId,int Id);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">线路区域实体</param>
        /// <param name="userIds">用户编号</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(EyouSoft.Model.CompanyStructure.Area model, string[] userIds);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">线路区域实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Update(EyouSoft.Model.CompanyStructure.Area model, string[] userIds);
        /// <summary>
        /// 获取线路区域实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        EyouSoft.Model.CompanyStructure.Area GetModel(int Id);
        /// <summary>
        /// 删除线路区域集合
        /// </summary>
        /// <param name="Ids">主键编号</param>
        /// <returns>true:成功 false:失败</returns>
        bool Delete(params int[] Ids);

        /// <summary>
        /// 删除线路区域用户关联信息
        /// </summary>
        /// <param name="AreaIds"></param>
        /// <returns></returns>
        bool DeleteUserArea(params int[] AreaIds);

        /// <summary>
        /// 线路区域是否发布过
        /// </summary>
        /// <param name="areaId">线路ID</param>
        /// <param name="companyId">公司ID</param>
        /// <returns>true发布过 false没发布过 </returns>
        bool IsAreaPublish(int areaId, int companyId);

        /// <summary>
        /// 分页获取公司线路区域集合
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>公司线路区域集合</returns>
        IList<EyouSoft.Model.CompanyStructure.Area> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId);

        /// <summary>
        /// 获取线路区域集合
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.Area> GetAreaList(int userId);
        /// <summary>
        /// 根据当前登录用户获取同级及下级部门人员的线路区域信息集合
        /// </summary>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.Area> GetAreas(string us);

        /// <summary>
        /// 获取当前公司的所有线路区域信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.Area> GetAreaByCompanyId(int companyId);
        /// <summary>
        /// 获取指定公司线路区域排序信息(最小及最大排序号)
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="min">最小排序号</param>
        /// <param name="max">最大排序号</param>
        void GetAreaSortId(int companyId, out int min, out int max);
    }
}
