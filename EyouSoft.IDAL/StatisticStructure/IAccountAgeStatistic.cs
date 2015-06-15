using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.StatisticStructure
{
    /// <summary>
    /// 帐龄分析统计数据接口
    /// </summary>
    /// 周文超 2011-01-25
    public interface IAccountAgeStatistic
    {
        /// <summary>
        /// 获取帐龄分析统计
        /// </summary>
        /// <param name="model">帐龄分析查询实体</param>
        /// <returns></returns>
        IList<Model.StatisticStructure.AccountAgeStatistic> GetAccountAgeStatistic(Model.StatisticStructure.QueryAccountAgeStatistic model, string HaveUserIds);

        /*/// <summary>
        /// 获取账龄分析-按客户单位统计
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWei> GetZhangLingAnKeHuDanWei(int pageSize, int pageIndex, ref int recordCount, int companyId, EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWeiChaXun searchInfo);

        /// <summary>
        /// 获取账龄分析-按客户单位统计合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        void GetZhangLingAnKeHuDanWei(int companyId, EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWeiChaXun searchInfo, out decimal weiShouHeJi);*/

        /// <summary>
        /// 获取账龄分析-按客户单位统计
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="us">用户编号集合 用,间隔</param>
        /// <returns></returns>
        IList<EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWei> GetZhangLingAnKeHuDanWei(int pageSize, int pageIndex, ref int recordCount, int companyId, EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWeiChaXun searchInfo,string us);

        /// <summary>
        /// 获取账龄分析-按客户单位统计合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="us">用户编号集合 用,间隔</param>
        /// <param name="weiShouHeJi">未收款合计</param>
        void GetZhangLingAnKeHuDanWei(int companyId, EyouSoft.Model.StatisticStructure.ZhangLingAnKeHuDanWeiChaXun searchInfo, out decimal weiShouHeJi,string us);

        /// <summary>
        /// 获取支出账龄分析（按供应商单位）
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="chaXun">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.StatisticStructure.FXZhiChuZhangLingInfo> GetZhiChuZhangLing(int pageSize, int pageIndex, ref int recordCount, int companyId, EyouSoft.Model.StatisticStructure.FXZhiChuZhangLingChaXunInfo chaXun);
        /// <summary>
        /// 获取支出账龄分析合计信息（按供应商单位）
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="chaXun">查询信息</param>
        /// <param name="weiFuKuanHeJi">未付金额合计</param>
        /// <param name="zongJinEHeJi">交易总金额合计</param>
        void GetZhiChuZhangLing(int companyId, EyouSoft.Model.StatisticStructure.FXZhiChuZhangLingChaXunInfo chaXun, out decimal weiFuKuanHeJi, out decimal zongJinEHeJi);
    }
}
