using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 系统操作日志BLL
    /// Author xuqh 2011-01-22
    /// </summary>
    public class SysHandleLogs
    {
        private readonly EyouSoft.IDAL.CompanyStructure.ISysHandleLogs Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.ISysHandleLogs>();

        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        private readonly EyouSoft.SSOComponent.Entity.UserInfo CurrUserInfo = EyouSoft.Security.Membership.UserProvider.GetUser();

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="model">系统操作日志实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.CompanyStructure.SysHandleLogs model)
        {
            if (model == null)
                return false;

            //model.EventCode = Model.CompanyStructure.SysHandleLogsNO.EventCode;
            model.EventIp = Toolkit.Utils.GetRemoteIP();
            model.EventTime = DateTime.Now;
            if (CurrUserInfo != null)
            {
                model.OperatorId = CurrUserInfo.ID;
                model.CompanyId = CurrUserInfo.CompanyID;
                model.DepatId = CurrUserInfo.DepartId;
                if (!string.IsNullOrEmpty(model.EventMessage) && model.EventMessage.Contains("{0}") && CurrUserInfo.ContactInfo != null)
                    model.EventMessage = string.Format(model.EventMessage, CurrUserInfo.ContactInfo.ContactName);
            }

            return Dal.Add(model);
        }

        /// <summary>
        /// 获取操作日志实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>操作日志实体</returns>
        public EyouSoft.Model.CompanyStructure.SysHandleLogs GetModel(string id)
        {
            return Dal.GetModel(id);
        }

        /// <summary>
        /// 分页获取操作日志列表
        /// </summary>
        /// <param name="pageSize">每页现实条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="model">系统操作日志查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.SysHandleLogs> GetList(int pageSize, int pageIndex, ref int RecordCount, EyouSoft.Model.CompanyStructure.QueryHandleLog model)
        {
            return Dal.GetList(pageSize, pageIndex, ref RecordCount, model);
        }
    }
}
