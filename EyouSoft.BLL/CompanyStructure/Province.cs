using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 省份管理BLL
    /// Author xuqh 2011-01-24
    /// </summary>
    public class Province
    {
        private readonly EyouSoft.IDAL.CompanyStructure.IProvince Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.IProvince>();
        private readonly BLL.CompanyStructure.SysHandleLogs handleLogsBll = new BLL.CompanyStructure.SysHandleLogs();

        #region 成员方法
        /// <summary>
        /// 验证省份名是否已经存在
        /// </summary>
        /// <param name="provinceName">省份名称</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="provinceId">身份编号</param>
        /// <returns>true:已存在 false:不存在</returns>
        public bool IsExists(string provinceName, int companyId,int provinceId)
        {
            return Dal.IsExists(provinceName, companyId,provinceId);
        }


        /// <summary>
        /// 添加省份
        /// </summary>
        /// <param name="model">省份实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.Province model)
        {
            bool result = false;
            result = Dal.Add(model);
            handleLogsBll.Add(AddLogs("添加", result));

            return result;
        }

        /// <summary>
        /// 修改省份
        /// </summary>
        /// <param name="model">省份实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.CompanyStructure.Province model)
        {
            bool result = false;
            result = Dal.Update(model);
            handleLogsBll.Add(AddLogs("修改", result));

            return result;
        }

        /// <summary>
        /// 获取省份实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.Province GetModel(int Id)
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
        /// 获取指定公司的省份集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>省份集合</returns>
        public IList<EyouSoft.Model.CompanyStructure.Province> GetList(int CompanyId)
        {
            return Dal.GetList(CompanyId);
        }

        /// <summary>
        /// 获取某个公司所有省份的信息包括城市
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.Province> GetProvinceInfo(int CompanyId)
        {
            return Dal.GetProvinceInfo(CompanyId);
        }

        /// <summary>
        /// 获取有常用城市的省份列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.Province> GetHasFavCityProvince(int companyId)
        {
            return Dal.GetHasFavCityProvince(companyId);
        }
        #endregion

        /// <summary>
        /// 添加日志记录
        /// </summary>
        /// <param name="actionName">操作名称</param>
        /// <param name="flag">操作状态</param>
        /// <returns></returns>
        private EyouSoft.Model.CompanyStructure.SysHandleLogs AddLogs(string actionName, bool flag)
        {
            EyouSoft.Model.CompanyStructure.SysHandleLogs model = new EyouSoft.Model.CompanyStructure.SysHandleLogs();
            model.ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_基础设置;
            model.EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode;
            model.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_基础设置.ToString() + (flag ? actionName : actionName + "失败") + "了省份数据";
            model.EventTitle = (flag ? actionName : actionName + "失败") + Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_基础设置.ToString() + "数据";

            return model;
        }
    }
}
