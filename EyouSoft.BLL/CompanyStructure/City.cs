using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 城市管理BLL
    /// Author: xuqh 2011-01-21
    /// </summary>
    public class City
    {
        private readonly EyouSoft.IDAL.CompanyStructure.ICity Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.ICity>();
        private readonly BLL.CompanyStructure.SysHandleLogs handleLogsBll = new BLL.CompanyStructure.SysHandleLogs();

        #region 成员方法

        /// <summary>
        /// 验证城市名是否已经存在
        /// </summary>
        /// <param name="cityName">城市名称</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="companyId">城市编号</param>
        /// <returns>true:已存在 false:不存在</returns>
        public bool IsExists(string cityName, int companyId,int cityId)
        {
            return Dal.IsExists(cityName, companyId,cityId);
        }

        /// <summary>
        /// 添加城市
        /// </summary>
        /// <param name="model">城市实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.City model)
        {
            bool result = false;
            result = Dal.Add(model);
            handleLogsBll.Add(AddLogs("新增", result));

            return result;
        }

        /// <summary>
        /// 修改城市
        /// </summary>
        /// <param name="model">城市实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.CompanyStructure.City model)
        {
            bool result = false;
            result = Dal.Update(model);
            handleLogsBll.Add(AddLogs("修改", result));

            return result;
        }

        /// <summary>
        /// 获取城市实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.City GetModel(int Id)
        {
            return Dal.GetModel(Id);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">主键集合</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(params int[] Ids)
        {
            bool result = false;
            result = Dal.Delete(Ids);
            handleLogsBll.Add(AddLogs("删除", result));

            return result;
        }

        /// <summary>
        /// 设置是否常用
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <param name="IsFav">是否常用</param>
        /// <returns>true:成功 false:失败</returns>
        public bool SetFav(int id, bool IsFav)
        {
            return Dal.SetFav(id, IsFav);
        }

        /// <summary>
        /// 获取城市集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="ProvinceId">省份编号</param>
        /// <param name="IsFav">是否常用城市 =null返回全部</param>
        /// <returns>城市集合</returns>
        public IList<EyouSoft.Model.CompanyStructure.City> GetList(int CompanyId, int ProvinceId, bool? IsFav)
        {
            return Dal.GetList(CompanyId, ProvinceId, IsFav);
        }

        /// <summary>
        /// 获取城市集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="ProvinceId">省份编号</param>
        /// <param name="IsFav">是否常用城市 =null返回全部</param>
        /// <returns>城市集合</returns>
        public IList<EyouSoft.Model.CompanyStructure.City> GetList(int CompanyId, int? ProvinceId, bool? IsFav)
        {
            if (CompanyId <= 0)
                return null;

            return Dal.GetList(CompanyId, ProvinceId, IsFav);
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
            model.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_基础设置.ToString() + (flag ? actionName : actionName + "失败") + "了城市数据";
            model.EventTitle = (flag ? actionName : actionName + "失败") + Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_基础设置.ToString() + "数据";

            return model;
        }
    }
}
