using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyouSoft.IDAL.FinanceStructure
{
    /// <summary>
    /// 杂费收入、支出数据访问类
    /// </summary>
    /// 周文超  2011-01-19
    public interface IOtherCost
    {
        /// <summary>
        /// 添加杂费收入信息
        /// </summary>
        /// <param name="list">杂费收入信息实体集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        int AddOtherIncome(IList<Model.FinanceStructure.OtherIncomeInfo> list);

        /// <summary>
        /// 添加杂费支出信息
        /// </summary>
        /// <param name="list">杂费支出信息实体集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        int AddOtherOut(IList<Model.FinanceStructure.OtherOutInfo> list);

        /// <summary>
        /// 修改杂费收入信息（只修改增加、减少、小计、备注）
        /// </summary>
        /// <param name="list">杂费收入信息集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        int UpdateOtherIncome(IList<Model.FinanceStructure.OtherIncomeInfo> list);

        /// <summary>
        /// 修改杂费收入信息（修改所有值）
        /// </summary>
        /// <param name="model">杂费收入信息集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        int UpdateOtherIncome(Model.FinanceStructure.OtherIncomeInfo model);

        /// <summary>
        /// 修改杂费支出信息（只修改增加、减少、小计、备注）
        /// </summary>
        /// <param name="list">支出信息集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        int UpdateOtherOut(IList<Model.FinanceStructure.OtherOutInfo> list);

        /// <summary>
        /// 修改杂费支出信息（修改所有值）
        /// </summary>
        /// <param name="model">支出信息集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        int UpdateOtherOut(Model.FinanceStructure.OtherOutInfo model);

        /// <summary>
        /// 删除杂费（收入或者支出）
        /// </summary>
        /// <param name="OtherCostIds">杂费Id集合</param>
        /// <returns>返回1表示成功，其他失败</returns>
        int DeleteOtherCost(string[] OtherCostIds);

        /// <summary>
        /// 获取杂费收入信息
        /// </summary>
        /// <param name="OtherIncomeId">杂费收入信息Id</param>
        /// <returns>杂费收入信息</returns>
        Model.FinanceStructure.OtherIncomeInfo GetOtherIncomeInfo(string OtherIncomeId);

        /// <summary>
        /// 获取杂费支出信息
        /// </summary>
        /// <param name="OtherOutId">杂费支出信息Id</param>
        /// <returns>杂费支出信息</returns>
        Model.FinanceStructure.OtherOutInfo GetOtherOutInfo(string OtherOutId);

        /// <summary>
        /// 获取杂费收入信息
        /// </summary>
        /// <param name="model">杂费信息查询实体</param>
        /// <returns>杂费收入信息集合</returns>
        IList<Model.FinanceStructure.OtherIncomeInfo> GetOtherIncomeList(Model.FinanceStructure.OtherCostQuery model);

        /// <summary>
        /// 获取杂费支出信息
        /// </summary>
        /// <param name="model">杂费信息查询实体</param>
        /// <returns>杂费支出信息集合</returns>
        IList<Model.FinanceStructure.OtherOutInfo> GetOtherOutList(Model.FinanceStructure.OtherCostQuery model);

        /// <summary>
        /// 获取杂费收入信息
        /// </summary>
        /// <param name="model">杂费信息查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns>杂费收入信息集合</returns>
        IList<EyouSoft.Model.FinanceStructure.OtherIncomeInfo> GetOtherIncomeList(EyouSoft.Model.FinanceStructure.OtherCostQuery model, string HaveUserIds, int PageSize, int PageIndex, ref int RecordCount);

        /// <summary>
        /// 获取杂费支出信息
        /// </summary>
        /// <param name="model">杂费信息查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns>杂费支出信息集合</returns>
        IList<EyouSoft.Model.FinanceStructure.OtherOutInfo> GetOtherOutList(EyouSoft.Model.FinanceStructure.OtherCostQuery model, string HaveUserIds, int PageSize, int PageIndex, ref int RecordCount);

        /// <summary>
        /// 获取杂费收入信息合计
        /// </summary>
        /// <param name="model">杂费信息查询实体</param>
        /// <param name="haveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="isIncome">收入 or 支出 （1：收入；0：支出）</param>
        /// <param name="totalAmount">总合计金额</param>
        void GetOtherIncomeList(Model.FinanceStructure.OtherCostQuery model, string haveUserIds, bool isIncome
            , ref decimal totalAmount);

        /// <summary>
        /// 设置杂费收入（支出）的状态
        /// </summary>
        /// <param name="status">支出（收入）状态</param>
        /// <param name="otherCostId">杂费收入（支出）Id</param>
        /// <returns>1：操作成功；其他失败</returns>
        int SetOtherCostStatus(bool status, params string[] otherCostId);

    }
}
