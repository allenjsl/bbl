using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.StatisticStructure
{
    /// <summary>
    /// 所有支出明细业务层
    /// </summary>
    /// 鲁功源 2011-01-23
    public class StatAllOut : BLLBase
    {
        private readonly IDAL.StatisticStructure.IStatAllOut idal = Component.Factory.ComponentFactory.CreateDAL<IDAL.StatisticStructure.IStatAllOut>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public StatAllOut()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public StatAllOut(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                DepartIds = UserModel.Departs;
                CompanyId = UserModel.CompanyID;
            }
        }

        /// <summary>
        /// 分页获取支出明细列表(统计分析-支出对账单列表)
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="queryInfo">查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.StatAllOutList> GetList(int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic queryInfo)
        {
            DateTime? _StartDate = null;
            DateTime? _EndDate = null;

            if (queryInfo != null)
            {
                _StartDate = queryInfo.LeaveDateStart;
                _EndDate = queryInfo.LeaveDateEnd;

                if (!queryInfo.LeaveDateEnd.HasValue)
                {
                    queryInfo.LeaveDateEnd = new DateTime(DateTime.Now.Year + 1, 1, 1).AddMilliseconds(-1);
                }
                else
                {
                    queryInfo.LeaveDateEnd = queryInfo.LeaveDateEnd.Value.AddDays(1).AddMilliseconds(-1);
                }

                if (!queryInfo.LeaveDateStart.HasValue)
                {
                    queryInfo.LeaveDateStart = new DateTime(DateTime.Now.Year, 1, 1);
                }
            }

            var items = idal.GetList(PageSize, PageIndex, ref RecordCount, queryInfo, HaveUserIds);

            queryInfo.LeaveDateEnd = _EndDate;
            queryInfo.LeaveDateStart = _StartDate;

            return items;
        }
        /// <summary>
        /// 添加支出明细
        /// </summary>
        /// <param name="model">支出明细实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.StatisticStructure.StatAllOut model)
        {
            return idal.Add(model);
        }
        /// <summary>
        /// 批量添加支出明细
        /// </summary>
        /// <param name="list">支出明细列表</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(IList<EyouSoft.Model.StatisticStructure.StatAllOut> list)
        {
            if (list == null || list.Count == 0)
                return false;
            return idal.Add(list);
        }
        /// <summary>
        /// 删除支出明细
        /// </summary>
        /// <param name="list">支出项目编号、类型集合</param>
        /// <returns></returns>
        public bool Delete(IList<EyouSoft.Model.StatisticStructure.ItemIdAndType> list)
        {
            if (list == null || list.Count == 0)
                return false;
            return idal.Delete(list);
        }

        /// <summary>
        /// 获取支出对帐单的汇总信息(统计分析-支出对账单列表合计)
        /// </summary>
        /// <param name="queryInfo">查询实体</param>
        /// <param name="TotalAmount">总支出</param>
        /// <param name="HasCheckAmount">已付</param>
        public void GetAllTotalAmount(EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic queryInfo
            , ref decimal TotalAmount, ref decimal HasCheckAmount)
        {
            if (queryInfo == null || queryInfo.CompanyId <= 0)
                return;

            DateTime? _StartDate = null;
            DateTime? _EndDate = null;

            _EndDate = queryInfo.LeaveDateEnd;
            _StartDate = queryInfo.LeaveDateStart;

            if (!queryInfo.LeaveDateEnd.HasValue)
            {
                queryInfo.LeaveDateEnd = new DateTime(DateTime.Now.Year + 1, 1, 1).AddMilliseconds(-1);
            }
            else
            {
                queryInfo.LeaveDateEnd = queryInfo.LeaveDateEnd.Value.AddDays(1).AddMilliseconds(-1);
            }

            if (!queryInfo.LeaveDateStart.HasValue)
            {
                queryInfo.LeaveDateStart = new DateTime(DateTime.Now.Year, 1, 1);
            }

            idal.GetAllTotalAmount(queryInfo, ref TotalAmount, ref HasCheckAmount, HaveUserIds);

            queryInfo.LeaveDateEnd = _EndDate;
            queryInfo.LeaveDateStart = _StartDate;
        }
    }
}
