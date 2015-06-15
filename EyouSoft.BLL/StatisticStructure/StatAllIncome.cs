using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.StatisticStructure
{
    /// <summary>
    /// 所有收入明细业务层
    /// </summary>
    /// 鲁功源 2011-01-23
    public class StatAllIncome : BLLBase
    {
        private readonly IDAL.StatisticStructure.IStatAllIncome idal = Component.Factory.ComponentFactory.CreateDAL<IDAL.StatisticStructure.IStatAllIncome>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public StatAllIncome()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userModel">当前登录用户信息</param>
        public StatAllIncome(EyouSoft.SSOComponent.Entity.UserInfo userModel)
        {
            if (userModel != null)
            {
                DepartIds = userModel.Departs;
                CompanyId = userModel.CompanyID;
            }
        }

        /// <summary>
        /// 分页获取收入明细列表
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="queryInfo">查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.StatAllIncomeList> GetList(int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic queryInfo)
        {
            return idal.GetList(PageSize, PageIndex, ref RecordCount, queryInfo, HaveUserIds);
        }
        /// <summary>
        /// 添加收入明细
        /// </summary>
        /// <param name="model">收入明细实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.StatisticStructure.StatAllIncome model)
        {
            if (model == null)
                return false;

            return idal.Add(model);
        }
        /// <summary>
        /// 批量添加收入明细
        /// </summary>
        /// <param name="list">收入明细列表</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(IList<EyouSoft.Model.StatisticStructure.StatAllIncome> list)
        {
            if (list == null || list.Count == 0)
                return false;
            return idal.Add(list);
        }
        /// <summary>
        /// 删除收入明细
        /// </summary>
        /// <param name="list">收入项目编号、类型集合</param>
        /// <returns></returns>
        public bool Delete(IList<EyouSoft.Model.StatisticStructure.IncomeItemIdAndType> list)
        {
            if (list == null || list.Count == 0)
                return false;
            return idal.Delete(list);
        }

        /// <summary>
        /// 获取收入对帐单的汇总信息
        /// </summary>
        /// <param name="queryInfo">查询实体</param>
        /// <param name="TotalAmount">总收入</param>
        /// <param name="HasCheckAmount">已收</param>
        public void GetAllTotalAmount(EyouSoft.Model.StatisticStructure.QueryAccountAgeStatistic queryInfo
            , ref decimal TotalAmount, ref decimal HasCheckAmount)
        {
            if (queryInfo == null || queryInfo.CompanyId <= 0)
                return;

            idal.GetAllTotalAmount(queryInfo, ref TotalAmount, ref HasCheckAmount, HaveUserIds);
        }
    }
}