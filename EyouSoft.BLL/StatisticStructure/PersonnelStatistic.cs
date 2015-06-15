using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.StatisticStructure
{
    /// <summary>
    /// 员工业绩统计业务实体
    /// </summary>
    /// 周文超 2011-01-21
    public class PersonnelStatistic : EyouSoft.BLL.BLLBase
    {
        private readonly IDAL.StatisticStructure.IPersonnelStatistic idal = Component.Factory.ComponentFactory.CreateDAL<IDAL.StatisticStructure.IPersonnelStatistic>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public PersonnelStatistic(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }

        /// <summary>
        /// 获取员工业绩-收入统计
        /// </summary>
        /// <param name="model">员工业绩统计查询实体</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.PersonnelIncomeStatistic> GetPersonnelIncomeStatistic(Model.StatisticStructure.QueryPersonnelStatistic model)
        {
            return idal.GetPersonnelIncomeStatistic(model, base.HaveUserIds);
        }

        /// <summary>
        /// 获取员工业绩-利润统计
        /// </summary>
        /// <param name="model">员工业绩统计查询实体</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.PersonnelProfitStatistic> GetPersonnelProfitStatistic(Model.StatisticStructure.QueryPersonnelStatistic model)
        {
            return idal.GetPersonnelProfitStatistic(model, base.HaveUserIds);
        }
    }
}
