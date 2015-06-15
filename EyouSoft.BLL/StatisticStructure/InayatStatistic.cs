using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.StatisticStructure
{
    /// <summary>
    /// 人次统计业务实体
    /// </summary>
    /// 周文超 2011-01-21
    public class InayatStatistic : EyouSoft.BLL.BLLBase
    {
        private readonly IDAL.StatisticStructure.IInayatStatistic idal = Component.Factory.ComponentFactory.CreateDAL<IDAL.StatisticStructure.IInayatStatistic>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public InayatStatistic(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }

        /// <summary>
        /// 获取人次-区域统计
        /// </summary>
        /// <param name="model">人次统计查询实体</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.InayaAreatStatistic> GetInayaAreatStatistic(
            Model.StatisticStructure.QueryInayatStatistic model)
        {
            return idal.GetInayaAreatStatistic(model, base.HaveUserIds);
        }

        /// <summary>
        /// 获取人次-部门统计
        /// </summary>
        /// <param name="model">人次统计查询实体</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.InayaDepartStatistic> GetInayaDepartStatistic(
            Model.StatisticStructure.QueryInayatStatistic model)
        {
            return idal.GetInayaDepartStatistic(model, base.HaveUserIds);
        }

        /// <summary>
        /// 获取人次-时间统计
        /// </summary>
        /// <param name="model">人次统计查询实体</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.InayaTimeStatistic> GetInayaTimeStatistic(
            Model.StatisticStructure.QueryInayatStatistic model)
        {
            return idal.GetInayaTimeStatistic(model, base.HaveUserIds);
        }
    }
}
