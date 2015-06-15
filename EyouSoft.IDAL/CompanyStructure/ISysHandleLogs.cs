using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.CompanyStructure
{
    /// <summary>
    /// 系统操作日志
    /// </summary>
    /// 鲁功源 2010-01-21
    public interface ISysHandleLogs
    {
        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="model">系统操作日志实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(EyouSoft.Model.CompanyStructure.SysHandleLogs model);
        /// <summary>
        /// 获取操作日志实体
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>操作日志实体</returns>
        EyouSoft.Model.CompanyStructure.SysHandleLogs GetModel(string id);
        /// <summary>
        /// 分页获取操作日志列表
        /// </summary>
        /// <param name="pageSize">每页现实条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="model">系统操作日志查询实体</param>
        /// <returns></returns>
        IList<EyouSoft.Model.CompanyStructure.SysHandleLogs> GetList(int pageSize,int pageIndex,ref int RecordCount, EyouSoft.Model.CompanyStructure.QueryHandleLog model);
    }
}
