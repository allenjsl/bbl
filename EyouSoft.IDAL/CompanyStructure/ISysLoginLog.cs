using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 系统登录日志
    /// </summary>
    /// 鲁功源 2010-01-21
    public interface ISysLoginLog
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">系统登陆日志实体</param>
        /// <returns></returns>
        bool Add(EyouSoft.Model.CompanyStructure.SysLoginLog model);
        /// <summary>
        /// 获取登录日志实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>操作日志实体</returns>
        EyouSoft.Model.CompanyStructure.SysLoginLog GetModel(string id);
    }
}
