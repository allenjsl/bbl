using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.StatisticStructure
{
    /// <summary>
    /// 支出对账单业务逻辑
    /// </summary>
    /// 2011-01-21
    public class ExpenseStatistic : EyouSoft.BLL.BLLBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public ExpenseStatistic(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }

        /// <summary>
        /// 获取支出对账-区域统计
        /// </summary>
        /// <param name="model">支出对账单查询实体</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.ExpenseAreaStatistic> GetExpenseAreaStatistic(Model.StatisticStructure.QueryExpenseStatistic model, int PageSize, int PageIndex, ref int RecordCount)
        {
            throw new NotImplementedException("没有实现！");
        }

        /// <summary>
        /// 获取支出对账-部门统计
        /// </summary>
        /// <param name="model">支出对账单查询实体</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.ExpenseDepartStatistic> GetExpenseDepartStatistic(Model.StatisticStructure.QueryExpenseStatistic model, int PageSize, int PageIndex, ref int RecordCount)
        {
            throw new NotImplementedException("没有实现！");
        }
    }
}
