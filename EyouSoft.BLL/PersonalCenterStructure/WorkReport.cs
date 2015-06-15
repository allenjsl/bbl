using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.PersonalCenterStructure
{
    /// <summary>
    /// 工作汇报业务层
    /// </summary>
    /// 鲁功源  2011-01-20
    public class WorkReport : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.PersonalCenterStructure.IWorkReport idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.PersonalCenterStructure.IWorkReport>();
        private readonly int userId=0;
        /// <summary>
        /// 系统操作日志逻辑
        /// </summary>
        private readonly BLL.CompanyStructure.SysHandleLogs HandleLogsBll = new EyouSoft.BLL.CompanyStructure.SysHandleLogs();

        #region 构造函数
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public WorkReport() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public WorkReport(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
                userId = UserModel.ID;
            }
        }
        #endregion

        #region WorkReport 成员
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">工作汇报实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.PersonalCenterStructure.WorkReport model)
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
                           EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_工作交流.ToString() + "新增了工作交流！编号为：" + model.ReportId,
                           EventTitle = "新增" + Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_工作交流.ToString() + "数据"
                       });
            }
            return Result;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">工作汇报实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Update(EyouSoft.Model.PersonalCenterStructure.WorkReport model)
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
                           EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_工作交流.ToString() + "修改了工作交流！编号为：" + model.ReportId,
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
        /// 设置审核状态
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <param name="Status">状态</param>
        /// <param name="CheckRemark">审核备注</param>
        /// <returns>true:成功 false:失败</returns>
        public bool SetChecked(int Id, EyouSoft.Model.EnumType.PersonalCenterStructure.CheckState Status, string CheckRemark)
        {
            if (Id <= 0)
                return false;
            bool Result= idal.SetChecked(Id, Status, CheckRemark);
            if (Result)
            {
                HandleLogsBll.Add(
                       new EyouSoft.Model.CompanyStructure.SysHandleLogs()
                       {
                           ModuleId = EyouSoft.Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_工作交流,
                           EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode,
                           EventMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "{0}在" + Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_工作交流.ToString() + "设置了工作交流的状态为：" + Status .ToString()+ "！编号为：" + Id.ToString(),
                           EventTitle = "设置了" + Model.EnumType.CompanyStructure.SysPermissionClass.个人中心_工作交流.ToString() + "的状态"
                       });
            }
            return Result;
        }
        /// <summary>
        /// 获取工作汇报实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns>工作汇报实体</returns>
        public EyouSoft.Model.PersonalCenterStructure.WorkReport GetModel(int Id)
        {
            if (Id <= 0)
                return null;
            return idal.GetModel(Id);
        }
        /// <summary>
        /// 分页工作交流集合
        /// </summary>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="QueryInfo">工作汇报查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.WorkReport> GetList(int pageSize, int pageIndex, ref int RecordCount, EyouSoft.Model.PersonalCenterStructure.QueryWorkReport QueryInfo)
        {
            return idal.GetList(pageSize, pageIndex, ref RecordCount, CompanyId, userId, QueryInfo);
        }

        #endregion
    }
}
