using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace EyouSoft.BLL.PersonalCenterStructure
{
    /// <summary>
    /// 个人中心-工作计划业务层
    /// </summary>
    /// 鲁功源 2011-01-20
    public class WorkPlan: EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.PersonalCenterStructure.IWorkPlan idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.PersonalCenterStructure.IWorkPlan>();
        /// <summary>
        /// 系统操作日志逻辑
        /// </summary>
        private readonly BLL.CompanyStructure.SysHandleLogs HandleLogsBll = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();

        #region 构造函数
          /// <summary>
        /// 默认构造函数
        /// </summary>
        public WorkPlan() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public WorkPlan(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }
        #endregion

        #region WorkPlan 成员
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">工作计划实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.PersonalCenterStructure.WorkPlan model)
        {
            if (model == null)
                return false;
            bool Result = idal.Add(model);
            if (Result)
            {
                HandleLogsBll.Add(
                       new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                       {
                           ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_工作交流,
                           EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                           EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_工作交流.ToString() + "新增了工作交流！编号为：" + model.PlanId,
                           EventTitle = "新增" + Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_工作交流.ToString() + "数据"
                       });
            }
            return Result;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">工作计划实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.PersonalCenterStructure.WorkPlan model)
        {
            if (model == null)
                return false;
            bool Result = idal.Update(model);
            if (Result)
            {
                HandleLogsBll.Add(
                       new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                       {
                           ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_工作交流,
                           EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                           EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_工作交流.ToString() + "修改了工作交流！编号为：" + model.PlanId,
                           EventTitle = "修改" + Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_工作交流.ToString() + "数据"
                       });
            }
            return Result;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">主键集合</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Delete(params string[] Ids)
        {
            if (Ids == null || Ids.Length == 0)
                return false;
            bool Result = idal.Delete(Ids);
            if (Result)
            {
                string strIDs = string.Empty;
                foreach (var item in Ids)
                {
                    strIDs += item + ",";
                }
                HandleLogsBll.Add(
                       new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                       {
                           ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_工作交流,
                           EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                           EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_工作交流.ToString() + "删除了工作交流！编号为：" + strIDs.TrimEnd(','),
                           EventTitle = "删除" + Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_工作交流.ToString() + "数据"
                       });
            }
            return Result;
        }
        /// <summary>
        /// 获取交流专区实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns>交流专区实体</returns>
        public EyouSoft.Model.PersonalCenterStructure.WorkPlan GetModel(int Id)
        {
            if (Id <= 0)
                return null;
            return idal.GetModel(Id,base.HaveUserIds);
        }
        /// <summary>
        /// 分页工作交流集合
        /// </summary>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号 =0返回所有</param>
        /// <param name="OperatorId">操作人编号 =0返回所有</param>
        /// <param name="QueryInfo">工作计划查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.WorkPlan> GetList(int pageSize, int pageIndex, ref int RecordCount, int CompanyId, int OperatorId,EyouSoft.Model.PersonalCenterStructure.QueryWorkPlan QueryInfo)
        {
            return idal.GetList(pageSize, pageIndex, ref RecordCount, CompanyId, OperatorId,QueryInfo);
        }

        #endregion
    }
}
