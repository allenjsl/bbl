/************************************************************
 * 模块名称：客户关系管理-销售统计业务逻辑
 * 功能说明：客户关系管理-销售统计业务逻辑
 * 创建人：周文超  2011-4-18 16:24:22
 * *********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.StatisticStructure
{
    /// <summary>
    /// 客户关系管理-销售统计业务逻辑
    /// </summary>
    public class BSalesStatistics : EyouSoft.BLL.BLLBase
    {
        private readonly IDAL.StatisticStructure.ISalesStatistics dal = Component.Factory.ComponentFactory.CreateDAL<IDAL.StatisticStructure.ISalesStatistics>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public BSalesStatistics() { }

        /// <summary>
        /// constructor with specified initial value
        /// </summary>
        /// <param name="uinfo">当前登录用户信息</param>
        public BSalesStatistics(EyouSoft.SSOComponent.Entity.UserInfo uinfo)
        {
            if (uinfo != null)
            {
                base.DepartIds = uinfo.Departs;
                base.CompanyId = uinfo.CompanyID;
            }
        }

        /// <summary>
        /// 获取销售统计列表
        /// </summary>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="QueryModel">查询实体</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.MSalesStatistics> GetSalesStatistics(int PageSize, int PageIndex
            , ref int RecordCount, Model.StatisticStructure.MQuerySalesStatistics QueryModel)
        {
            if (QueryModel == null || QueryModel.CompanyId <= 0)
                return null;

            return dal.GetSalesStatistics(PageSize, PageIndex, ref RecordCount, QueryModel, HaveUserIds);
        }

        /// <summary>
        /// 获取销售统计的汇总信息
        /// </summary>
        /// <param name="AllPeopleNum">总人数</param>
        /// <param name="AllFinanceSum">总金额</param>
        /// <param name="QueryModel">查询实体</param>
        public void GetSumSalesStatistics(ref int AllPeopleNum, ref decimal AllFinanceSum
            , Model.StatisticStructure.MQuerySalesStatistics QueryModel)
        {
            if (QueryModel == null || QueryModel.CompanyId <= 0)
                return ;

            dal.GetSumSalesStatistics(ref AllPeopleNum, ref AllFinanceSum, QueryModel, HaveUserIds);
        }
    }
}
