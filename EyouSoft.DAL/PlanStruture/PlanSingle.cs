//Author:汪奇志 2010-02-17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.PlanStruture
{
    /// <summary>
    /// 单项服务计调安排数据访问类
    /// </summary>
    /// Author:汪奇志 2010-02-17
    public class PlanSingle:EyouSoft.Toolkit.DAL.DALBase,EyouSoft.IDAL.PlanStruture.IPlanSingle
    {
        #region constructor
        /// <summary>
        /// database
        /// </summary>
        private Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public PlanSingle()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region static constants
        //static constants
        private const string DEFAULT_XML_DOC = "<ROOT></ROOT>";
        private const string SQL_SELECT_GetSingleExpense = "SELECT SUM([TotalAmount]) AS TotalAmount,SUM([HasCheckAmount]) AS PaidAmount FROM [tbl_StatAllOut] WHERE TourId=@TourId";
        #endregion

        #region private members
        /// <summary>
        /// 创建单项服务支出团队核算信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateSetSingleAccountingXML(IList<EyouSoft.Model.TourStructure.PlanSingleInfo> items)
        {
            //XML:<ROOT><Info PlanId="安排编号" Amount="结算费用" AddAmount="增加费用" ReduceAmount="减少费用" FRemark="财务备注" /></ROOT>
            if (items == null || items.Count < 1) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                xmlDoc.AppendFormat("<Info PlanId=\"{0}\" Amount=\"{1}\" AddAmount=\"{2}\" ReduceAmount=\"{3}\" FRemark=\"{4}\" />", item.PlanId
                    , item.Amount
                    , item.AddAmount
                    , item.ReduceAmount
                    , Utils.ReplaceXmlSpecialCharacter(item.FRemark));
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }
        #endregion

        #region IPlanSingle 成员
        /// <summary>
        /// 获取单项服务支出信息
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <param name="totalAmount">支出金额</param>
        /// <param name="paidAmount">未付金额</param>
        public void GetSingleExpense(string tourId, out decimal totalAmount, out decimal unpaidAmount)
        {
            totalAmount = 0;
            unpaidAmount = 0;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetSingleExpense);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(rdr.GetOrdinal("TotalAmount")))
                    {
                        totalAmount = rdr.GetDecimal(rdr.GetOrdinal("TotalAmount"));
                    }
                    if (!rdr.IsDBNull(rdr.GetOrdinal("PaidAmount")))
                    {
                        unpaidAmount = totalAmount - rdr.GetDecimal(rdr.GetOrdinal("PaidAmount"));
                    }
                }
            }

            return;
        }

        /// <summary>
        /// 单项服务支出团队核算，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <param name="plans">单项服务支出信息集合</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int SetSingleAccounting(string tourId, IList<EyouSoft.Model.TourStructure.PlanSingleInfo> plans)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_PlanSingle_SetSingleAccounting");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);
            this._db.AddInParameter(cmd, "PlansSingle", DbType.String, this.CreateSetSingleAccountingXML(plans));
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);

            int sqlExceptionCode = 0;

            try
            {
                DbHelper.RunProcedure(cmd, this._db);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                sqlExceptionCode = 0 - e.Number;
            }

            if (sqlExceptionCode < 0) return sqlExceptionCode;

            return Convert.ToInt32(this._db.GetParameterValue(cmd, "Result"));
        }
        #endregion
    }
}
