using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.FinanceStructure
{
    /// <summary>
    /// 团队利润分配业务逻辑
    /// </summary>
    /// 周文超 2011-01-21
    public class TourProfitSharer : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.FinanceStructure.ITourProfitShareInfo idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.FinanceStructure.ITourProfitShareInfo>();
        /// <summary>
        /// 统一维护业务逻辑
        /// </summary>
        private readonly BLL.UtilityStructure.Utility UtilityBll = new EyouSoft.BLL.UtilityStructure.Utility();
        /// <summary>
        /// 系统操作日志逻辑
        /// </summary>
        private readonly BLL.CompanyStructure.SysHandleLogs HandleLogsBll = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public TourProfitSharer(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }

        #region 成员

        /// <summary>
        /// 添加团队利润分配
        /// </summary>
        /// <param name="list">团队利润分配实体集合</param>
        /// <returns>返回1成功，其他失败</returns>
        public int AddTourShare(IList<EyouSoft.Model.FinanceStructure.TourProfitShareInfo> list)
        {
            if (list == null || list.Count == 0)
                return 0;

            int Result = idal.Add(list);
            if (Result == 1)
            {
                UtilityBll.CalculationTourProfitShare(list[0].TourId);

                HandleLogsBll.Add(
                    new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                    {
                        ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团队核算,
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团队核算.ToString() + "新增了团队利润分配数据！",
                        EventTitle = "新增" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团队核算.ToString() + "数据"
                    });
            }

            return Result;
        }

        /// <summary>
        /// 添加团队利润分配
        /// </summary>
        /// <param name="list">团队利润分配实体</param>
        /// <returns>返回1成功，其他失败</returns>
        public int AddTourShare(EyouSoft.Model.FinanceStructure.TourProfitShareInfo model)
        {
            if (model == null)
                return 0;

            IList<EyouSoft.Model.FinanceStructure.TourProfitShareInfo> list = new List<EyouSoft.Model.FinanceStructure.TourProfitShareInfo>();
            list.Add(model);

            return AddTourShare(list);
        }

        /// <summary>
        /// 修改团队利润分配
        /// </summary>
        /// <param name="list">团队利润分配实体集合</param>
        /// <returns>返回1成功，其他失败</returns>
        public int UpdateTourShare(IList<EyouSoft.Model.FinanceStructure.TourProfitShareInfo> list)
        {
            if (list == null || list.Count == 0)
                return 0;
            int addResult = AddTourShare(list.Where(item => string.IsNullOrEmpty(item.ShareId)).ToList());
            int editResult = idal.Update(list.Where(item => !string.IsNullOrEmpty(item.ShareId)).ToList());
            int Result = (addResult > 0 || editResult > 0) ? 1 : 0;
            if (Result == 1)
            {
                UtilityBll.CalculationTourProfitShare(list[0].TourId);

                HandleLogsBll.Add(
                    new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                    {
                        ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团队核算,
                        EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                        EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团队核算.ToString() + "修改了团队利润分配数据！",
                        EventTitle = "修改" + Model.EnumType.CompanyStructure.SysPermissionClass.财务管理_团队核算.ToString() + "数据"
                    });
            }

            return Result;
        }

        /// <summary>
        /// 修改团队利润分配
        /// </summary>
        /// <param name="list">团队利润分配实体</param>
        /// <returns>返回1成功，其他失败</returns>
        public int UpdateTourShare(EyouSoft.Model.FinanceStructure.TourProfitShareInfo model)
        {
            if (model == null)
                return 0;

            IList<EyouSoft.Model.FinanceStructure.TourProfitShareInfo> list = new List<EyouSoft.Model.FinanceStructure.TourProfitShareInfo>();
            list.Add(model);

            return UpdateTourShare(model);
        }

        /// <summary>
        /// 获取团队利润分配实体
        /// </summary>
        /// <param name="TourShareId">团队利润分配Id</param>
        /// <returns></returns>
        public EyouSoft.Model.FinanceStructure.TourProfitShareInfo GetModel(string TourShareId)
        {
            if (string.IsNullOrEmpty(TourShareId))
                return null;
            return idal.GetModel(TourShareId);
        }

        /// <summary>
        /// 获取团队的所有利润分配
        /// </summary>
        /// <param name="TourId"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.TourProfitShareInfo> GetTourShareList(string TourId)
        {
            if (string.IsNullOrEmpty(TourId))
                return null;
            return idal.GetList(TourId);
        }

        #endregion
    }
}
