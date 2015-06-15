using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data;

namespace EyouSoft.DAL.FinanceStructure
{
    /// <summary>
    /// 出纳登帐数据访问
    /// </summary>
    /// 周文超 2011-01-21
    public class CashierRegister : Toolkit.DAL.DALBase, IDAL.FinanceStructure.ICashierRegister
    {
        #region 私有变量

        private readonly Database _db = null;

        /// <summary>
        /// 插入Sql语句
        /// </summary>
        private const string Sql_CashierRegister_Insert = @" INSERT INTO [tbl_CashierRegister]
           ([PaymentTime]
           ,[PaymentCount]
           ,[PaymentBank]
           ,[Customer]
           ,[CustomerID]
           ,[Contacter]
           ,[ContactTel]
           ,[Remark]
           ,[CompanyID]
           ,[RegisterTime]
           ,[OperatorId]
           ,[TotalAmount])
     VALUES
           (@PaymentTime
           ,@PaymentCount
           ,@PaymentBank
           ,@Customer
           ,@CustomerID
           ,@Contacter
           ,@ContactTel
           ,@Remark
           ,@CompanyID
           ,@RegisterTime
           ,@OperatorId
           ,@TotalAmount) ";

        /// <summary>
        /// 构造函数
        /// </summary>
        public CashierRegister()
        {
            _db = this.SystemStore;
        }

        #endregion

        #region ICashierRegister 成员

        /// <summary>
        /// 新增出纳登帐信息
        /// </summary>
        /// <param name="model">出纳登帐信息实体</param>
        /// <returns>返回1成功，其他失败</returns>
        public int AddCashierRegister(EyouSoft.Model.FinanceStructure.CashierRegisterInfo model)
        {
            if (model == null)
                return 0;

            DbCommand dc = _db.GetSqlStringCommand(Sql_CashierRegister_Insert);
            _db.AddInParameter(dc, "PaymentTime", DbType.DateTime, model.PaymentTime);
            _db.AddInParameter(dc, "PaymentCount", DbType.Decimal, model.PaymentCount);
            _db.AddInParameter(dc, "PaymentBank", DbType.String, model.PaymentBank);
            _db.AddInParameter(dc, "Customer", DbType.String, model.CustomerName);
            _db.AddInParameter(dc, "CustomerID", DbType.Int32, model.CustomerId);
            _db.AddInParameter(dc, "Contacter", DbType.String, model.Contacter);
            _db.AddInParameter(dc, "ContactTel", DbType.String, model.ContactTel);
            _db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            _db.AddInParameter(dc, "CompanyID", DbType.Int32, model.CompanyId);
            _db.AddInParameter(dc, "RegisterTime", DbType.DateTime, model.CreateTime);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            _db.AddInParameter(dc, "TotalAmount", DbType.Decimal, model.TotalAmount);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 获取出纳登帐信息
        /// </summary>
        /// <param name="model">出纳登帐信息查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.CashierRegisterInfo> GetList(EyouSoft.Model.FinanceStructure.QueryCashierRegisterInfo model, string HaveUserIds, int PageSize, int PageIndex, ref int RecordCount)
        {
            IList<EyouSoft.Model.FinanceStructure.CashierRegisterInfo> list = new List<EyouSoft.Model.FinanceStructure.CashierRegisterInfo>();
            string strFiles = " [ID],[PaymentTime],[PaymentCount],[PaymentBank],[Customer],[CustomerID],[Contacter],[ContactTel],[Remark],[CompanyID],[RegisterTime],[OperatorId],[TotalAmount] ";
            string strOrder = string.Empty;
            string strWhere = this.GetSqlWhere(model, HaveUserIds, ref strOrder);

            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, "tbl_CashierRegister", "ID", strFiles, strWhere, strOrder))
            {
                EyouSoft.Model.FinanceStructure.CashierRegisterInfo tmpModel = null;
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.FinanceStructure.CashierRegisterInfo();
                    if (!dr.IsDBNull(dr.GetOrdinal("ID")))
                        tmpModel.RegisterId = int.Parse(dr["ID"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("PaymentTime")))
                        tmpModel.PaymentTime = DateTime.Parse(dr["PaymentTime"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("PaymentCount")))
                        tmpModel.PaymentCount = decimal.Parse(dr["PaymentCount"].ToString());
                    tmpModel.PaymentBank = dr["PaymentBank"].ToString();
                    tmpModel.CustomerName = dr["Customer"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("CustomerID")))
                        tmpModel.CustomerId = int.Parse(dr["CustomerID"].ToString());
                    tmpModel.Contacter = dr["Contacter"].ToString();
                    tmpModel.ContactTel = dr["ContactTel"].ToString();
                    tmpModel.Remark = dr["Remark"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("CompanyID")))
                        tmpModel.CompanyId = int.Parse(dr["CompanyID"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("RegisterTime")))
                        tmpModel.CreateTime = DateTime.Parse(dr["RegisterTime"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorId")))
                        tmpModel.OperatorId = int.Parse(dr["OperatorId"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("TotalAmount")))
                        tmpModel.TotalAmount = decimal.Parse(dr["TotalAmount"].ToString());

                    list.Add(tmpModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取出纳登帐信息金额合计
        /// </summary>
        /// <param name="model">出纳登帐信息查询实体</param>
        /// <param name="haveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="paymentCount">到款银行</param>
        /// <param name="totalAmount">已销账金额</param>
        public void GetCashierRegisterMoney(Model.FinanceStructure.QueryCashierRegisterInfo model, string haveUserIds
            , ref decimal paymentCount, ref decimal totalAmount)
        {
            if(model == null)
                return;

            string strOrder = string.Empty;
            string strWhere = GetSqlWhere(model, haveUserIds, ref strOrder);

            var strSql = new StringBuilder();
            strSql.Append(" select sum(PaymentCount),sum(TotalAmount) from tbl_CashierRegister ");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.AppendFormat(" where {0} ", strWhere);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            using (IDataReader dr = DbHelper.ExecuteReader(dc,_db))
            {
                if(dr.Read())
                {
                    if (!dr.IsDBNull(0))
                        paymentCount = dr.GetDecimal(0);
                    if (!dr.IsDBNull(1))
                        totalAmount = dr.GetDecimal(1);
                }
            }
        }

        /// <summary>
        /// 销帐
        /// </summary>
        /// <param name="RegistId">出纳登帐Id</param>
        /// <param name="OperatorId">当前操作员Id</param>
        /// <param name="OperatorName">当前操作员名称</param>
        /// <param name="list">销帐实体集合</param>
        /// <returns>返回1成功，其他失败</returns>
        public int CancelRegist(int RegistId, int OperatorId, string OperatorName
            , IList<EyouSoft.Model.FinanceStructure.CancelRegistInfo> list)
        {
            DbCommand dc = _db.GetStoredProcCommand("proc_CashierRegister_CancelRegist");
            _db.AddInParameter(dc, "RegistId", DbType.Int32, RegistId);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, OperatorId);
            _db.AddInParameter(dc, "OperatorName", DbType.String, OperatorName);
            _db.AddInParameter(dc, "EraseAccountXML", DbType.String, this.GetCancelRegistXML(list));
            _db.AddOutParameter(dc, "ErrorValue", DbType.Int32, 4);

            DbHelper.RunProcedure(dc, _db);
            object obj = _db.GetParameterValue(dc, "ErrorValue");
            if (obj.Equals(null))
                return 0;
            else
                return int.Parse(obj.ToString());
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 根据查询实体获取SqlWhere条件
        /// </summary>
        /// <param name="model">查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="strOrder">排序语句</param>
        /// <returns>SqlWhere条件</returns>
        private string GetSqlWhere(EyouSoft.Model.FinanceStructure.QueryCashierRegisterInfo model, string HaveUserIds
            , ref string strOrder)
        {
            if (model == null)
                return string.Empty;

            StringBuilder strWhere = new StringBuilder(" 1 = 1 ");
            if (model.CompanyId > 0)
                strWhere.AppendFormat(" and CompanyID = {0} ", model.CompanyId);
            if (model.CustomerId > 0)
                strWhere.AppendFormat(" and CustomerID = {0} ", model.CustomerId);
            if (!string.IsNullOrEmpty(model.PaymentBank))
                strWhere.AppendFormat(" and PaymentBank like '%{0}%' ", model.PaymentBank);
            if (model.StartTime.HasValue)
                strWhere.AppendFormat(" and datediff(dd,'{0}',PaymentTime) >= 0 ", model.StartTime.Value.ToShortDateString());
            if (model.EndTime.HasValue)
                strWhere.AppendFormat(" and datediff(dd,PaymentTime,'{0}') >= 0 ", model.EndTime.Value.ToShortDateString());
            if (!string.IsNullOrEmpty(HaveUserIds))
                strWhere.AppendFormat(" and OperatorId in ({0}) ", HaveUserIds);

            switch (model.OrderIndex)
            {
                case 0:
                    strOrder = " PaymentTime asc ";
                    break;
                case 1:
                    strOrder = " PaymentTime desc ";
                    break;
            }

            return strWhere.ToString();
        }

        /// <summary>
        /// 将销帐实体集合生成XML
        /// </summary>
        /// <param name="list">销帐实体集合</param>
        /// <returns>销帐实体集合XML</returns>
        private string GetCancelRegistXML(IList<EyouSoft.Model.FinanceStructure.CancelRegistInfo> list)
        {
            if (list == null || list.Count <= 0)
                return string.Empty;

            StringBuilder strSqlRoot = new StringBuilder();
            strSqlRoot.Append("<ROOT>");
            foreach (EyouSoft.Model.FinanceStructure.CancelRegistInfo t in list)
            {
                if (t == null)
                    continue;

                strSqlRoot.AppendFormat("<EraseAccount_Add OrderId='{0}'  EraseMoney='{1}'", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(t.OrderId), t.Money);
                strSqlRoot.Append(" />");
            }
            strSqlRoot.Append("</ROOT>");

            return strSqlRoot.ToString();
        }

        #endregion
    }
}
