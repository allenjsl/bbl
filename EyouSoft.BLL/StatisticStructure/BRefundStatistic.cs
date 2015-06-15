/************************************************************
 * 模块名称：退票统计业务逻辑
 * 功能说明：
 * 创建人：周文超  2011-4-21 17:14:11
 * *********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.StatisticStructure
{
    /// <summary>
    /// 退票统计业务逻辑
    /// </summary>
    public class BRefundStatistic : EyouSoft.BLL.BLLBase
    {
        private readonly IDAL.StatisticStructure.IRefundStatistic dal = Component.Factory.ComponentFactory.CreateDAL<IDAL.StatisticStructure.IRefundStatistic>();

        #region constructor
        /// <summary>
        /// 构造函数
        /// </summary>
        public BRefundStatistic() { }

        /// <summary>
        /// constructor with specified initial value
        /// </summary>
        /// <param name="uinfo">当前登录用户信息</param>
        public BRefundStatistic(EyouSoft.SSOComponent.Entity.UserInfo uinfo)
        {
            if (uinfo != null)
            {
                base.DepartIds = uinfo.Departs;
                base.CompanyId = uinfo.CompanyID;
            }
        }

        /// <summary>
        /// constructor with specified initial value
        /// </summary>
        /// <param name="uinfo">当前登录用户信息</param>
        /// <param name="isEnableOrganizations">是否启用组织机构</param>
        public BRefundStatistic(EyouSoft.SSOComponent.Entity.UserInfo uinfo, bool isEnableOrganizations)
        {
            base.IsEnable = isEnableOrganizations;

            if (uinfo != null)
            {
                base.DepartIds = uinfo.Departs;
                base.CompanyId = uinfo.CompanyID;
            }
        }

        #endregion

        /// <summary>
        /// 获取退票统计
        /// </summary>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="model">查询实体</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.MRefundStatistic> GetRefundStatistic(int PageSize, int PageIndex
            , ref int RecordCount, Model.StatisticStructure.MQueryRefundStatistic model)
        {
            if (model == null || model.CompanyId <= 0)
                return null;

            return dal.GetRefundStatistic(PageSize, PageIndex, ref RecordCount, model, HaveUserIds);
        }

        /// <summary>
        /// 获取退票统计合计信息
        /// </summary>
        /// <param name="refundAmount">退回金额合计</param>
        /// <param name="model">查询实体</param>
        public void GetSumRefundStatistic(ref decimal refundAmount, Model.StatisticStructure.MQueryRefundStatistic model)
        {
            refundAmount = 0M;
            if (model == null || model.CompanyId <= 0)
                return;

            dal.GetSumRefundStatistic(ref refundAmount, model, HaveUserIds);
        }
    }
}
