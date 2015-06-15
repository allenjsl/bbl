using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.StatisticStructure
{
    /// <summary>
    /// 利润统计业务逻辑
    /// </summary>
    /// 周文超 2011-01-21
    public class EarningsStatistic : EyouSoft.BLL.BLLBase
    {
        private readonly IDAL.StatisticStructure.IEarningsStatistic idal = Component.Factory.ComponentFactory.CreateDAL<IDAL.StatisticStructure.IEarningsStatistic>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public EarningsStatistic(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }

        /// <summary>
        /// 获取利润--区域统计
        /// </summary>
        /// <param name="model">利润统计查询实体</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.EarningsAreaStatistic> GetEarningsAreaStatistic(Model.StatisticStructure.QueryEarningsStatistic model)
        {
            if (model == null || model.CompanyId < 1) return null;

            return idal.GetEarningsAreaStatistic(model, base.HaveUserIds);
        }

        /// <summary>
        /// 获取利润--部门统计
        /// </summary>
        /// <param name="model">利润统计查询实体</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.EarningsDepartStatistic> GetEarningsDepartStatistic(Model.StatisticStructure.QueryEarningsStatistic model)
        {
            if (model == null || model.CompanyId < 1) return null;

            return idal.GetEarningsDepartStatistic(model, base.HaveUserIds);
        }

        /// <summary>
        /// 获取利润--类型统计
        /// </summary>
        /// <param name="model">利润统计查询实体</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.EarningsTypeStatistic> GetEarningsTypeStatistic(Model.StatisticStructure.QueryEarningsStatistic model)
        {
            if (model == null || model.CompanyId < 1) return null;

            return idal.GetEarningsTypeStatistic(model, base.HaveUserIds);
        }

        /// <summary>
        /// 获取利润--时间统计
        /// </summary>
        /// <param name="model">利润统计查询实体</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.EarningsTimeStatistic> GetEarningsTimeStatistic(Model.StatisticStructure.QueryEarningsStatistic model)
        {
            if (model == null || model.CompanyId < 1) return null;

            return idal.GetEarningsTimeStatistic(model, base.HaveUserIds);
        }

        /// <summary>
        /// 获取团队利润统计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="us">用户Id集合，半角逗号间隔</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.MTuanDuiLiRunTongJiInfo> GetTuanDuiLiRunTongJi(int companyId, EyouSoft.Model.StatisticStructure.MTuanDuiLiRunTongJinSearchInfo searchInfo, string us)
        {
            if (companyId < 1) return null;

            return idal.GetTuanDuiLiRunTongJi(companyId, searchInfo, null);
        }
    }
}
