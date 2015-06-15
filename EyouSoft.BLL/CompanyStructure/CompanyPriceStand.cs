using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 公司报价等级BLL
    /// Author:xuqh 2011-01-21
    /// </summary>
    public class CompanyPriceStand
    {
        private readonly EyouSoft.IDAL.CompanyStructure.ICompanyPriceStand Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.ICompanyPriceStand>();
        private readonly BLL.CompanyStructure.SysHandleLogs handleLogsBll = new BLL.CompanyStructure.SysHandleLogs();

        #region 成员方法

        /// <summary>
        /// 验证是否已经存在同名的报价等级
        /// </summary>
        /// <param name="PriceStandName">报价等级名称</param>
        /// <param name="Id">主键编号</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>true:存在 false:不存在</returns>
        public bool IsExists(string PriceStandName, int Id, int CompanyId)
        {
            return Dal.IsExists(PriceStandName, Id, CompanyId);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">报价等级实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.CompanyPriceStand model)
        {
            bool result = false;
            result = Dal.Add(model);
            handleLogsBll.Add(AddLogs("添加", result));

            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">报价等级实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.CompanyStructure.CompanyPriceStand model)
        {
            bool result = false;
            result = Dal.Update(model);
            handleLogsBll.Add(AddLogs("修改", result));

            return result;
        }

        /// <summary>
        /// 获取报价等级实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CompanyPriceStand GetModel(int Id)
        {
            return Dal.GetModel(Id);
        }

        /// <summary>
        /// 删除报价等级
        /// </summary>
        /// <param name="Ids">主键编号</param>
        /// <returns>大于0:成功 等于0:失败 -1为系统默认</returns>
        public bool Delete(params int[] Ids)
        {
            bool result = false;
            result = Dal.Delete(Ids);
            handleLogsBll.Add(AddLogs("删除", result));

            return result;
        }

        /// <summary>
        /// 分页获取公司报价等级集合
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <returns>公司报价等级集合</returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyPriceStand> GetList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId)
        {
            return Dal.GetList(PageSize, PageIndex, ref RecordCount, CompanyId);
        }

        /// <summary>
        /// 获取某公司所有报价等级信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.CompanyPriceStand> GetPriceStandByCompanyId(int companyId)
        {
            return Dal.GetPriceStandByCompanyId(companyId);
        }

        /// <summary>
        /// 判断报价标准或客户等级是否被使用过
        /// </summary>
        /// <param name="id">报价标准或客户等级</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="typeId">1是标价标准 0是客户等级</param>
        /// <returns></returns>
        public bool IsUsed(int id, int companyId, int typeId)
        {
            return Dal.IsUsed(id, companyId, typeId);
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
            model.EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_基础设置.ToString() + (flag ? actionName : actionName + "失败") + "了公司报价等级数据";
            model.EventTitle = (flag ? actionName : actionName + "失败") + Model.EnumType.CompanyStructure.SysPermissionClass.系统设置_基础设置.ToString() + "数据";

            return model;
        }
    }
}
