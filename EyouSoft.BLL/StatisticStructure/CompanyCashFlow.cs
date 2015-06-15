using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.StatisticStructure
{
    /// <summary>
    /// 统计分析-现金流量明细业务逻辑层
    /// </summary>
    /// 鲁功源 2011-01-22
    public class CompanyCashFlow : EyouSoft.BLL.BLLBase
    {
        private readonly EyouSoft.IDAL.StatisticStructure.ICompanyCashFlow idal = EyouSoft.Component.Factory.ComponentFactory.CreateDAL<EyouSoft.IDAL.StatisticStructure.ICompanyCashFlow>();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public CompanyCashFlow(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
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
        /// 添加现金流量明细
        /// </summary>
        /// <param name="model">现金流量明细实体</param>
        /// <returns>true:成功 false:失败</returns>
        public bool Add(EyouSoft.Model.StatisticStructure.CompanyCashFlow model)
        {
            if (model == null)
                return false;
            return idal.Add(model);
        }
        #endregion
    }
}
