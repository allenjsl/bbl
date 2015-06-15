using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.StatisticStructure
{
    /// <summary>
    /// 收入对账单业务逻辑
    /// </summary>
    /// 周文超 2011-01-21
    public class ReconcileStatistic : EyouSoft.BLL.BLLBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public ReconcileStatistic(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }

        /// <summary>
        /// 获取收入对账-区域统计
        /// </summary>
        /// <param name="model">收入对账单查询实体</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.ReconcileAreaStatistic> GetReconcileAreaStatistic(Model.StatisticStructure.QueryReconcileStatistic model, int PageSize, int PageIndex, ref int RecordCount)
        {
            throw new NotImplementedException("没有实现！");
        }

        /// <summary>
        /// 获取收入对账-部门统计
        /// </summary>
        /// <param name="model">收入对账单查询实体</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.ReconcileDepartStatistic> GetReconcileDepartStatistic(Model.StatisticStructure.QueryReconcileStatistic model, int PageSize, int PageIndex, ref int RecordCount)
        {
            throw new NotImplementedException("没有实现！");
        }
    }
}
