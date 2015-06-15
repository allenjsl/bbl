using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.StatisticStructure
{
    /// <summary>
    /// 出票统计业务逻辑
    /// </summary>
    /// 周文超 2011-03-18
    public class TicketOutStatistic : EyouSoft.BLL.BLLBase
    {
        private readonly IDAL.StatisticStructure.ITicketOutStatistic dal = Component.Factory.ComponentFactory.CreateDAL<IDAL.StatisticStructure.ITicketOutStatistic>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public TicketOutStatistic(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }

        /// <summary>
        /// 获取出票--售票处统计
        /// </summary>
        /// <param name="model">出票售票处统计实体</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.TicketOutStatisticOffice> GetTicketOutStatisticOffice(Model.StatisticStructure.QueryTicketOutStatisti model)
        {
            if (model == null)
                return null;

            return dal.GetTicketOutStatisticOffice(model, null);
        }

        /// <summary>
        /// 获取出票--航空公司统计
        /// </summary>
        /// <param name="model">出票航空公司统计实体</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.TicketOutStatisticAirLine> GetTicketOutStatisticAirLine(Model.StatisticStructure.QueryTicketOutStatisti model)
        {
            if (model == null)
                return null;

            return dal.GetTicketOutStatisticAirLine(model, null);
        }

        /// <summary>
        /// 获取出票--部门统计
        /// </summary>
        /// <param name="model">出票部门统计实体</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.TicketOutStatisticDepart> GetTicketOutStatisticDepart(Model.StatisticStructure.QueryTicketOutStatisti model)
        {
            if (model == null)
                return null;

            return dal.GetTicketOutStatisticDepart(model, null);
        }

        /// <summary>
        /// 获取出票--日期统计
        /// </summary>
        /// <param name="model">出票日期统计实体</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.TicketOutStatisticTime> GetTicketOutStatisticTime(Model.StatisticStructure.QueryTicketOutStatisti model)
        {
            if (model == null)
                return null;

            return dal.GetTicketOutStatisticTime(model, null);
        }
    }
}
