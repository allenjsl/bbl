using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 公司品牌管理BLL
    /// Author xuqh 2011-01-22
    /// </summary>
    public class CompanyBrand
    {
        private readonly EyouSoft.IDAL.CompanyStructure.ICompanyBrand Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.ICompanyBrand>();
        private readonly BLL.CompanyStructure.SysHandleLogs handleLogsBll = new BLL.CompanyStructure.SysHandleLogs();
        
        #region 成员方法
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">公司产品实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.CompanyBrand model)
        {
            bool result = false;
            result = Dal.Add(model);
            handleLogsBll.Add(AddLogs("新增", result));

            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">公司产品实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.CompanyStructure.CompanyBrand model)
        {
            bool result = false;
            result = Dal.Update(model);
            handleLogsBll.Add(AddLogs("修改", result));

            return result;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">主键编号集合</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(params int[] Ids)
        {
            bool result = false;
            result = Dal.Delete(Ids);
            handleLogsBll.Add(AddLogs("删除", result));

            return result;
        }
        /// <summary>
        /// 获取公司产品实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CompanyBrand GetModel(int Id)
        {
            return Dal.GetModel(Id);
        }

        /// <summary>
        /// 分页获取公司产品列表
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>公司产品列表</returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyBrand> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId)
        {
            return Dal.GetList(PageSize, PageIndex, ref RecordCount, CompanyId);
        }

        /// <summary>
        /// 根据公司ID获取公司品牌信息
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyBrand> GetBrandByCompanyId(int companyId)
        {
            return Dal.GetBrandByCompanyId(companyId);
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
            model.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_基础设置.ToString() + (flag ? actionName : actionName + "失败") + "了品牌管理数据";
            model.EventTitle = (flag ? actionName : actionName + "失败") + Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_基础设置.ToString() + "数据";

            return model;
        }
    }
}
