//汪奇志 2012-08-17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.FinanceStructure
{
    /// <summary>
    /// 发票管理数据访问类接口
    /// </summary>
    public interface IFaPiao
    {
        /// <summary>
        /// 登记开票信息，操作成功返回1，负值失败。
        /// </summary>
        /// <param name="info">开票信息业务实体</param>
        /// <returns></returns>
        int Insert(EyouSoft.Model.FinanceStructure.MFaPiaoInfo info);
        /// <summary>
        /// 修改开票信息，操作成功返回1，负值失败。
        /// </summary>
        /// <param name="info">开票信息业务实体</param>
        /// <returns></returns>
        int Update(EyouSoft.Model.FinanceStructure.MFaPiaoInfo info);
        /// <summary>
        /// 删除开票信息，操作成功返回1，负值失败。
        /// </summary>
        /// <param name="faPiaoId">发票登记编号</param>
        /// <returns></returns>
        int Delete(string faPiaoId);
        /// <summary>
        /// 获取发票登记信息业务实体
        /// </summary>
        /// <param name="faPiaoId">发票登记编号</param>
        /// <returns></returns>
        EyouSoft.Model.FinanceStructure.MFaPiaoInfo GetInfo(string faPiaoId);
        /// <summary>
        /// 获取发票管理列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="searchInfo">查询实体</param>
        /// <returns></returns>
        IList<EyouSoft.Model.FinanceStructure.MFaPiaoGuanLiInfo> GetFaPiaoGuanLis(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.FinanceStructure.MFaPiaoGuanLiSearchInfo searchInfo);
        /// <summary>
        /// 获取发票管理列表合计信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询实体</param>
        /// <param name="jiaoYiJinE">交易金额合计</param>
        /// <param name="kaiPiaoJinE">开票金额合计</param>
        void GetFaPiaoGuanLisHeJi(int companyId, EyouSoft.Model.FinanceStructure.MFaPiaoGuanLiSearchInfo searchInfo, out decimal jiaoYiJinE, out decimal kaiPiaoJinE);
        /// <summary>
        /// 获取发票已登记列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="crmId">客户单位编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.FinanceStructure.MFaPiaoInfo> GetFaPiaos(int companyId, int crmId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.FinanceStructure.MFaPiaoSearchInfo searchInfo);
        /// <summary>
        /// 获取发票已登记列表合计信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="crmId">客户单位编号</param>
        /// <param name="searchInfo">查询实体</param>
        /// <param name="kaiPiaoJinE">开票金额合计</param>
        void GetFaPiaosHeJi(int companyId, int crmId, EyouSoft.Model.FinanceStructure.MFaPiaoSearchInfo searchInfo, out decimal kaiPiaoJinE);
    }
}
