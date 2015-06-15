//汪奇志 2012-08-24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.FinanceStructure
{
    /// <summary>
    /// 团款收入相关数据访问类接口
    /// </summary>
    public interface IShouRu
    {
        /// <summary>
        /// 获取财务管理-团款收入-批量收款审核列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.FinanceStructure.MLBShouKuanShenHeInfo> GetShouKuanShenHeLB(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.FinanceStructure.MLBShouKuanShenHeSearchInfo searchInfo);

        /// <summary>
        /// 获取财务管理-团款收入-批量收款审核列表合计
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="searchInfo"></param>
        /// <param name="skJinEHeJi">收款金额合计</param>
        void GetShouKuanShenHeLBHeJi(int companyId, EyouSoft.Model.FinanceStructure.MLBShouKuanShenHeSearchInfo searchInfo, out decimal skJinEHeJi);

        /// <summary>
        /// 审核收款，返回1成功，其它失败
        /// </summary>
        /// <param name="dengJiId">登记编号</param>
        /// <param name="operatorId">审核人编号</param>
        /// <returns></returns>
        int ShenHeShouKuan(string dengJiId, int operatorId);
    }
}
