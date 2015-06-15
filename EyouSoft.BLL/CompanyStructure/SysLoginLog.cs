using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.CompanyStructure
{
    /// <summary>
    /// 系统登录日志BLL
    /// Author xuqh 2011-01-22
    /// </summary>
    public class SysLoginLog
    {
        private readonly EyouSoft.IDAL.CompanyStructure.ISysLoginLog Dal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.CompanyStructure.ISysLoginLog>();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">系统登陆日志实体</param>
        /// <returns></returns>
        public bool Add(EyouSoft.Model.CompanyStructure.SysLoginLog model)
        {
            return Dal.Add(model);
        }
        /// <summary>
        /// 获取登录日志实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>操作日志实体</returns>
        public EyouSoft.Model.CompanyStructure.SysLoginLog GetModel(string id)
        {
            return Dal.GetModel(id);
        }
    }
}
