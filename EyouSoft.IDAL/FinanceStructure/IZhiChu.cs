//汪奇志 2012-08-26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.FinanceStructure
{
    /// <summary>
    /// 团款支出相关数据访问类接口
    /// </summary>
    public interface IZhiChu
    {
        /// <summary>
        /// 获取财务管理-团款支出-按计调项显示支出列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        IList<EyouSoft.Model.FinanceStructure.MLBJiDiaoZhiChuInfo> GetJiDiaoZhiChuLB(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.FinanceStructure.MLBJiDiaoZhiChuSearchInfo searchInfo);
        /// <summary>
        /// 获取财务管理-团款支出-按计调项显示支出列表合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="zhiChuJinE">支出金额</param>
        /// <param name="yiDengJiJinE">已登记金额</param>
        /// <param name="yiZhiFuJinE">已支付金额</param>
        void GetJiDiaoZhiChuLBHeJi(int companyId, EyouSoft.Model.FinanceStructure.MLBJiDiaoZhiChuSearchInfo searchInfo, out decimal zhiChuJinE, out decimal yiDengJiJinE, out decimal yiZhiFuJinE);
        /// <summary>
        /// 财务管理-团款支出-按计调类型批量支出登记，返回1成功，其它失败
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="operatorId">操作人编号</param>
        /// <param name="info">登记信息业务实体</param>
        /// <returns></returns>
        int PiLiangDengJi(int companyId, int operatorId, EyouSoft.Model.FinanceStructure.MZhiChuPiLiangDengJiInfo info);
    }
}
