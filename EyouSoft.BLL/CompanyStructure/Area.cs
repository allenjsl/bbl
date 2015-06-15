using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 线路区域BLL
    /// Author:xuqh 2011-01-21 
    /// </summary>
    public class Area: EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.CompanyStructure.IArea Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.IArea>();

        private readonly BLL.CompanyStructure.SysHandleLogs handleLogsBll = new BLL.CompanyStructure.SysHandleLogs();

        #region constructure
        /// <summary>
        /// default constructure
        /// </summary>
        public Area() { }

        /// <summary>
        /// constructor with specified initial value
        /// </summary>
        /// <param name="uinfo">当前登录用户信息</param>
        public Area(EyouSoft.SSOComponent.Entity.UserInfo uinfo)
        {
            if (uinfo != null)
            {
                base.DepartIds = uinfo.Departs;
                base.CompanyId = uinfo.CompanyID;
            }
        }

        /// <summary>
        /// constructor with specified initial value
        /// </summary>
        /// <param name="uinfo">当前登录用户信息</param>
        /// <param name="isEnableOrganizations">是否启用组织机构</param>
        public Area(EyouSoft.SSOComponent.Entity.UserInfo uinfo, bool isEnableOrganizations)
        {
            base.IsEnable = isEnableOrganizations;

            if (uinfo != null)
            {
                base.DepartIds = uinfo.Departs;
                base.CompanyId = uinfo.CompanyID;
            }
        }
        #endregion

        #region 成员方法

        /// <summary>
        /// 验证是否已经存在同名的线路区域
        /// </summary>
        /// <param name="AreaName">线路区域名称</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>true:存在 false:不存在</returns>
        public bool IsExists(string AreaName, int CompanyId,int Id)
        {
            return Dal.IsExists(AreaName, CompanyId,Id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">线路区域实体</param>
        /// <param name="userIds">用户编号</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.Area model, string[] userIds)
        {
            bool result = false;
            result = Dal.Add(model,userIds);

            handleLogsBll.Add(AddLogs("新增",result));

            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">线路区域实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.CompanyStructure.Area model, string[] userIds)
        {
            bool result = false;
            result = Dal.Update(model,userIds);

            handleLogsBll.Add(AddLogs("修改", result));

            return result;
        }

        /// <summary>
        /// 获取线路区域实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.Area GetModel(int Id)
        {
            return Dal.GetModel(Id);
        }

        /// <summary>
        /// 删除线路区域集合
        /// </summary>
        /// <param name="Ids">主键编号</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(params int[] Ids)
        {
            bool result = false;
            result = Dal.Delete(Ids);
            if (result)
                Dal.DeleteUserArea(Ids);
            handleLogsBll.Add(AddLogs("删除", result));

            return result;
        }

        /// <summary>
        /// 线路区域是否发布过
        /// </summary>
        /// <param name="areaId">线路ID</param>
        /// <param name="companyId">公司ID</param>
        /// <returns>true发布过 false没发布过 </returns>
        public bool IsAreaPublish(int areaId, int companyId)
        {
            return Dal.IsAreaPublish(areaId, companyId);
        }

        /// <summary>
        /// 分页获取公司线路区域集合
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>公司线路区域集合</returns>
        public IList<EyouSoft.Model.CompanyStructure.Area> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId)
        {
            return Dal.GetList(PageSize, PageIndex, ref RecordCount, CompanyId);
        }

        /// <summary>
        /// 获取线路区域集合
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.Area> GetAreaList(int userId)
        {
            return Dal.GetAreaList(userId);
        }

        /// <summary>
        /// 根据当前登录用户获取同级及下级部门人员的线路区域信息集合
        /// </summary>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.Area> GetAreas()
        {
            return Dal.GetAreas(this.HaveUserIds);
        }

        /// <summary>
        /// 获取当前公司的所有线路区域信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.Area> GetAreaByCompanyId(int companyId)
        {
            return Dal.GetAreaByCompanyId(companyId);
        }

        /// <summary>
        /// 获取指定公司线路区域排序信息(最小及最大排序号)
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="min">最小排序号</param>
        /// <param name="max">最大排序号</param>
        public void GetAreaSortId(int companyId, out int min, out int max)
        {
            min = 0; max = 0;
            if (companyId < 1) return;
            Dal.GetAreaSortId(companyId, out min, out max);
        }
        #endregion

        /// <summary>
        /// 添加日志记录
        /// </summary>
        /// <param name="areaModel">日志操作实体</param>
        /// <param name="actionName">操作名称</param>
        /// <param name="flag">操作状态</param>
        /// <returns></returns>
        private EyouSoft.Model.CompanyStructure.SysHandleLogs AddLogs(string actionName, bool flag)
        {
            EyouSoft.Model.CompanyStructure.SysHandleLogs model = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            model.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_基础设置;
            model.EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode;
            model.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_基础设置.ToString() + (flag ? actionName : actionName + "失败") + "了线路区域数据";
            model.EventTitle = (flag ? actionName : actionName + "失败") + Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_基础设置.ToString() + "数据";

            return model;
        }
    }
}
