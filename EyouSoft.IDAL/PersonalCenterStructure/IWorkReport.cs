using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.PersonalCenterStructure
{
    /// <summary>
    /// 工作汇报数据层接口
    /// </summary>
    /// 鲁功源  2011-01-17
    public interface IWorkReport
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">工作汇报实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(EyouSoft.Model.PersonalCenterStructure.WorkReport model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">工作汇报实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Update(EyouSoft.Model.PersonalCenterStructure.WorkReport model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">主键集合</param>
        /// <returns>true:成功 false:失败</returns>
        bool Delete(params string[] Ids);
        /// <summary>
        /// 设置审核状态
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <param name="Status">状态</param>
        /// <param name="CheckRemark">审核备注</param>
        /// <returns>true:成功 false:失败</returns>
        bool SetChecked(int Id, EyouSoft.Model.EnumType.PersonalCenterStructure.CheckState Status, string CheckRemark);
        /// <summary>
        /// 获取工作汇报实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns>工作汇报实体</returns>
        EyouSoft.Model.PersonalCenterStructure.WorkReport GetModel(int Id);
        /// <summary>
        /// 分页获取工作汇报列表
        /// </summary>
        /// <param name="pageSize">每页现实条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号 =0返回所有</param>
        /// <param name="OperatorId">操作人编号</param>
        /// <param name="QueryInfo">工作汇报查询实体</param>
        /// <returns></returns>
        IList<EyouSoft.Model.PersonalCenterStructure.WorkReport> GetList(int pageSize, int pageIndex, ref int RecordCount, int CompanyId, int OperatorId, EyouSoft.Model.PersonalCenterStructure.QueryWorkReport QueryInfo);
    }
}
