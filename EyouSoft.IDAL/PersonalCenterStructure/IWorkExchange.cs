using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.PersonalCenterStructure
{
    /// <summary>
    /// 个人中心-交流专区数据层接口
    /// </summary>
    /// 鲁功源 2011-01-17
    public interface IWorkExchange
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">交流专区实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Add(EyouSoft.Model.PersonalCenterStructure.WorkExchange model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">交流专区实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool Update(EyouSoft.Model.PersonalCenterStructure.WorkExchange model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids">主键编号</param>
        /// <returns>true:成功 false:失败</returns>
        bool Delete(params string[] Ids);
        /// <summary>
        /// 获取交流专区实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <returns>交流专区实体</returns>
        EyouSoft.Model.PersonalCenterStructure.WorkExchange GetModel(int Id);
        /// <summary>
        /// 分页获取交流专区列表
        /// </summary>
        /// <param name="pageSize">每页现实条数</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="CompanyId">公司编号 =0返回所有</param>
        /// <param name="OperatorId">操作人编号 =0返回所有</param>
        /// <returns>交流专区列表</returns>
        IList<EyouSoft.Model.PersonalCenterStructure.WorkExchange> GetList(int pageSize, int pageIndex, ref int RecordCount,int CompanyId,int OperatorId);
        /// <summary>
        /// 更新浏览次数
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool SetClicks(int Id);
        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="model">交流专区回复实体</param>
        /// <returns>true:成功 false:失败</returns>
        bool AddReply(EyouSoft.Model.PersonalCenterStructure.WorkExchangeReply model);
    }
}
