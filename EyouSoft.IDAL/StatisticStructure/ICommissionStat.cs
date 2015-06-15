using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.StatisticStructure
{
    /// <summary>
    /// 返佣统计数据接口
    /// </summary>
    /// 周文超 2011-06-08
    public interface ICommissionStat
    {
        /// <summary>
        /// 获取客户返佣统计信息
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="queryModel">查询实体</param>
        /// <returns></returns>
        IList<Model.StatisticStructure.MCommissionStatInfo> GetCommissions(int pageSize, int pageIndex, ref int recordCount, int companyId, Model.StatisticStructure.MCommissionStatSeachInfo queryModel);
        /// <summary>
        /// 获取返佣明细信息集合
        /// </summary>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="buyerCompanyId">客户单位编号（组团）</param>
        /// <param name="buyerContactId">客户单位联系人编号</param>
        /// <param name="commissionType">返佣类型</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.StatisticStructure.MCommissionDetailInfo> GetCommissionDetails(int pageSize, int pageIndex, ref int recordCount
            , int companyId, int buyerCompanyId, int buyerContactId, EyouSoft.Model.EnumType.CompanyStructure.CommissionType commissionType
            , EyouSoft.Model.StatisticStructure.MCommissionStatSeachInfo searchInfo);
        /// <summary>
        /// 支付返佣金额，返回1成功，其它失败
        /// </summary>
        /// <param name="info">支付信息业务实体</param>
        /// <returns></returns>
        int PayCommission(EyouSoft.Model.StatisticStructure.MPayCommissionInfo info);
    }
}
