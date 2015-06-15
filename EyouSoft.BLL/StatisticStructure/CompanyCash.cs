using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.StatisticStructure
{
    /// <summary>
    /// 统计分析-现金流量业务逻辑层
    /// </summary>
    /// 鲁功源 2011-01-22
    public class CompanyCash : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.StatisticStructure.ICompanyCash idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.StatisticStructure.ICompanyCash>();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public CompanyCash(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }
        #endregion

        #region 成员方法
        /// <summary>
        /// 分页获取按天统计的现金流量列表
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.CompanyCash> GetListGroupByDay(int CompanyId)
        {
            EyouSoft.Model.StatisticStructure.QueryCompanyCash QueryInfo = new EyouSoft.Model.StatisticStructure.QueryCompanyCash();
            QueryInfo.OrderIndex = 1;
            QueryInfo.QueryDate = DateTime.Now;
            QueryInfo.StatisticType = EyouSoft.Model.EnumType.StatisticStructure.StatisticType.按日统计;
            return idal.GetList(CompanyId, QueryInfo);
        }
        /// <summary>
        /// 分页获取按月统计的现金流量列表
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.CompanyCash> GetListGroupByMonth(int CompanyId)
        {
            EyouSoft.Model.StatisticStructure.QueryCompanyCash QueryInfo = new EyouSoft.Model.StatisticStructure.QueryCompanyCash();
            QueryInfo.OrderIndex = 1;
            QueryInfo.QueryDate = DateTime.Now;
            QueryInfo.StatisticType = EyouSoft.Model.EnumType.StatisticStructure.StatisticType.按月统计;
            return idal.GetList(CompanyId, QueryInfo);
        }
        #endregion
    }
}
