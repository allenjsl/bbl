using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.BLL.StatisticStructure
{
    /// <summary>
    /// 帐龄分析统计业务逻辑
    /// </summary>
    /// 周文超 2011-01-21
    public class AccountAgeStatistic : EyouSoft.BLL.BLLBase
    {
        private readonly IDAL.StatisticStructure.IAccountAgeStatistic dal = Component.Factory.ComponentFactory.CreateDAL<IDAL.StatisticStructure.IAccountAgeStatistic>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="UserModel">当前登录用户信息</param>
        public AccountAgeStatistic(EyouSoft.SSOComponent.Entity.UserInfo UserModel)
        {
            if (UserModel != null)
            {
                base.DepartIds = UserModel.Departs;
                base.CompanyId = UserModel.CompanyID;
            }
        }

        /// <summary>
        /// 获取帐龄分析统计
        /// </summary>
        /// <param name="model">帐龄分析查询实体</param>
        /// <returns></returns>
        public IList<Model.StatisticStructure.AccountAgeStatistic> GetAccountAgeStatistic(Model.StatisticStructure.QueryAccountAgeStatistic model)
        {
            return dal.GetAccountAgeStatistic(model, base.HaveUserIds);
        }

        /// <summary>
        /// 获取账龄分析-按客户单位统计
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWei> GetZhangLingAnKeHuDanWei(int pageSize, int pageIndex, ref int recordCount, int companyId, EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWeiChaXun searchInfo)
        {
            if (companyId < 1) return null;
            return dal.GetZhangLingAnKeHuDanWei(pageSize, pageIndex, ref recordCount, companyId, searchInfo,HaveUserIds);
        }

        /// <summary>
        /// 获取账龄分析-按客户单位统计合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="weiShouHeJi">未收款合计</param>
        public void GetZhangLingAnKeHuDanWei(int companyId, EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWeiChaXun searchInfo, out decimal weiShouHeJi)
        {
            weiShouHeJi = 0;
            if (companyId < 1) return;
            dal.GetZhangLingAnKeHuDanWei(companyId, searchInfo, out weiShouHeJi, HaveUserIds);
        }

        /// <summary>
        /// 获取支出账龄分析（按供应商单位）
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="chaXun">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.FXZhiChuZhangLingInfo> GetZhiChuZhangLing(int pageSize, int pageIndex, ref int recordCount, int companyId, EyouSoft.Model.StatisticStructure.FXZhiChuZhangLingChaXunInfo chaXun)
        {
            if (companyId < 1) return null;

            return dal.GetZhiChuZhangLing(pageSize, pageIndex, ref recordCount, companyId, chaXun);
        }

        /// <summary>
        /// 获取支出账龄分析合计信息（按供应商单位）
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="chaXun">查询信息</param>
        /// <param name="weiFuKuanHeJi">未付金额合计</param>
        /// <param name="zongJinEHeJi">交易总金额合计</param>
        public void GetZhiChuZhangLing(int companyId, EyouSoft.Model.StatisticStructure.FXZhiChuZhangLingChaXunInfo chaXun, out decimal weiFuKuanHeJi, out decimal zongJinEHeJi)
        {
            weiFuKuanHeJi = 0;
            zongJinEHeJi = 0;
            if (companyId < 1) return;

            dal.GetZhiChuZhangLing(companyId, chaXun, out weiFuKuanHeJi, out zongJinEHeJi);
        }

    }
}
