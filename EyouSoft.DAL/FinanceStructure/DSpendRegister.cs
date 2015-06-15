/************************************************************
 * 模块名称：支出销帐数据访问
 * 功能说明：支出销帐数据访问
 * 创建人：周文超  2011-4-28 15:55:28
 * *********************************************************/

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
    /// 支出销帐数据访问
    /// </summary>
    public class DSpendRegister : Toolkit.DAL.DALBase, IDAL.FinanceStructure.ISpendRegister
    {
        #region 私有成员

        /// <summary>
        /// 数据库链接对象
        /// </summary>
        private readonly Database _db = null;

        /// <summary>
        /// 新增支出登帐Sql
        /// </summary>
        private const string Sql_SpendRegister_Insert = @" INSERT INTO [tbl_SpendRegister]
           ([CompanyId]
           ,[PayTime]
           ,[Amount]
           ,[PayType]
           ,[SupplierId]
           ,[SupplierName]
           ,[Realname]
           ,[Telephone]
           ,[Remark]
           ,[RegisterTime]
           ,[OperatorId]
           ,[OffAmount])
     VALUES
           (@CompanyId
           ,@PayTime
           ,@Amount
           ,@PayType
           ,@SupplierId
           ,@SupplierName
           ,@Realname
           ,@Telephone
           ,@Remark
           ,@RegisterTime
           ,@OperatorId
           ,@OffAmount); ";

        /// <summary>
        /// 根据查询实体生成SqlWhere语句
        /// </summary>
        /// <param name="model">查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="strOrder">排序语句</param>
        /// <returns>SqlWhere语句</returns>
        private string GetSqlWhere(EyouSoft.Model.FinanceStructure.MQuerySpendRegister model, string HaveUserIds
            , ref string strOrder)
        {
            if (model == null || model.CompanyId <= 0)
                return string.Empty;

            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" CompanyId = {0} ", model.CompanyId);
            if (model.StartTime.HasValue)
                strWhere.AppendFormat(" and datediff(dd,'{0}',PayTime) >= 0 ", model.StartTime.Value.ToShortDateString());
            if (model.EndTime.HasValue)
                strWhere.AppendFormat(" and datediff(dd,PayTime,'{0}') >= 0 ", model.EndTime.Value.ToShortDateString());
            if (!string.IsNullOrEmpty(HaveUserIds))
                strWhere.AppendFormat(" and OperatorId in ({0}) ", HaveUserIds);
            if (model.PayType.HasValue)
                strWhere.AppendFormat(" and PayType = {0} ", (int)model.PayType.Value);
            if (model.SupplierId > 0)
                strWhere.AppendFormat(" and SupplierId = {0} ", model.SupplierId);

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
        private string GetCancelRegistXML(IList<EyouSoft.Model.FinanceStructure.MSpendRegisterDetail> list)
        {
            if (list == null || list.Count <= 0)
                return string.Empty;

            StringBuilder strSqlRoot = new StringBuilder();
            strSqlRoot.Append("<ROOT>");
            foreach (EyouSoft.Model.FinanceStructure.MSpendRegisterDetail t in list)
            {
                if (t == null)
                    continue;

                strSqlRoot.AppendFormat("<CancelRegist Amount = '{0}' ItemId = '{1}' ItemType = '{2}' "
                    , t.Amount, EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(t.ItemId), (int)t.ItemType);
                strSqlRoot.Append(" />");
            }
            strSqlRoot.Append("</ROOT>");

            return strSqlRoot.ToString();
        }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public DSpendRegister()
        {
            _db = this.SystemStore;
        }

        #region ISpendRegister 成员

        /// <summary>
        /// 新增支出登帐信息
        /// </summary>
        /// <param name="model">支出登帐信息实体</param>
        /// <returns>返回1成功，其他失败</returns>
        public int AddSpendRegister(EyouSoft.Model.FinanceStructure.MSpendRegister model)
        {
            if (model == null)
                return 0;

            DbCommand dc = _db.GetSqlStringCommand(Sql_SpendRegister_Insert);
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, model.CompanyId);
            _db.AddInParameter(dc, "PayTime", DbType.DateTime, model.PayTime);
            _db.AddInParameter(dc, "PayType", DbType.Byte, (int)model.PayType);
            _db.AddInParameter(dc, "Amount", DbType.Decimal, model.Amount);
            _db.AddInParameter(dc, "SupplierId", DbType.Int32, model.SupplierId);
            _db.AddInParameter(dc, "SupplierName", DbType.String, model.SupplierName);
            _db.AddInParameter(dc, "Realname", DbType.String, model.Realname);
            _db.AddInParameter(dc, "Telephone", DbType.String, model.Telephone);
            _db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            _db.AddInParameter(dc, "RegisterTime", DbType.DateTime, model.RegisterTime);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, model.OperatorId);
            _db.AddInParameter(dc, "OffAmount", DbType.Decimal, model.OffAmount);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 获取支出登帐信息
        /// </summary>
        /// <param name="model">支出登帐信息查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.MSpendRegister> GetList(EyouSoft.Model.FinanceStructure.MQuerySpendRegister model, string HaveUserIds, int PageSize, int PageIndex, ref int RecordCount)
        {
            if (model == null || model.CompanyId <= 0)
                return null;

            IList<EyouSoft.Model.FinanceStructure.MSpendRegister> list = new List<EyouSoft.Model.FinanceStructure.MSpendRegister>();
            string strFiles = " [RegisterId],[CompanyId],[PayTime],[Amount],[PayType],[SupplierId],[SupplierName],[Realname],[Telephone],[Remark],[RegisterTime],[OperatorId],[OffAmount] ";
            string strOrder = string.Empty;
            string strWhere = this.GetSqlWhere(model, HaveUserIds, ref strOrder);

            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, "tbl_CashierRegister", "ID", strFiles, strWhere, strOrder))
            {
                EyouSoft.Model.FinanceStructure.MSpendRegister tmpModel = null;
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.FinanceStructure.MSpendRegister();

                    if (!dr.IsDBNull(dr.GetOrdinal("RegisterId")))
                        tmpModel.RegisterId = dr.GetInt32(dr.GetOrdinal("RegisterId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CompanyId")))
                        tmpModel.CompanyId = dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PayTime")))
                        tmpModel.PayTime = dr.GetDateTime(dr.GetOrdinal("PayTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Amount")))
                        tmpModel.Amount = dr.GetDecimal(dr.GetOrdinal("Amount"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PayType")))
                        tmpModel.PayType = (EyouSoft.Model.EnumType.TourStructure.RefundType)dr.GetInt32(dr.GetOrdinal("PayType"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SupplierId")))
                        tmpModel.SupplierId = dr.GetInt32(dr.GetOrdinal("SupplierId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("SupplierName")))
                        tmpModel.SupplierName = dr.GetString(dr.GetOrdinal("SupplierName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Realname")))
                        tmpModel.Realname = dr.GetString(dr.GetOrdinal("Realname"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Telephone")))
                        tmpModel.Telephone = dr.GetString(dr.GetOrdinal("Telephone"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Remark")))
                        tmpModel.Remark = dr.GetString(dr.GetOrdinal("Remark"));
                    if (!dr.IsDBNull(dr.GetOrdinal("RegisterTime")))
                        tmpModel.RegisterTime = dr.GetDateTime(dr.GetOrdinal("RegisterTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorId")))
                        tmpModel.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("OffAmount")))
                        tmpModel.OffAmount = dr.GetDecimal(dr.GetOrdinal("OffAmount"));

                    list.Add(tmpModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 支出销帐
        /// </summary>
        /// <param name="RegistId">支出登帐Id</param>
        /// <param name="OperatorId">当前操作员Id</param>
        /// <param name="OperatorName">当前操作员名称</param>
        /// <param name="list">支出销帐实体集合</param>
        /// <returns>返回1成功，其他失败</returns>
        public int SpendRegisterDetail(int RegistId, int OperatorId, string OperatorName, IList<EyouSoft.Model.FinanceStructure.MSpendRegisterDetail> list)
        {
            if (RegistId <= 0 || OperatorId <= 0 || list == null || list.Count <= 0)
                return 0;

            DbCommand dc = _db.GetStoredProcCommand("proc_SpendRegister_CancelRegist");
            _db.AddInParameter(dc, "RegistId", DbType.Int32, RegistId);
            _db.AddInParameter(dc, "OperatorId", DbType.Int32, OperatorId);
            _db.AddInParameter(dc, "OperatorName", DbType.String, OperatorName);
            _db.AddInParameter(dc, "CancelRegistXML", DbType.String, this.GetCancelRegistXML(list));
            _db.AddOutParameter(dc, "ErrorValue", DbType.Int32, 4);

            DbHelper.RunProcedure(dc, _db);
            object obj = _db.GetParameterValue(dc, "ErrorValue");
            if (obj.Equals(null))
                return 0;
            else
                return int.Parse(obj.ToString());
        }

        /// <summary>
        /// 批量登记付款
        /// </summary>
        /// <param name="info">付款批量登记信息业务实体</param>
        /// <returns></returns>
        public int BatchRegisterExpense(EyouSoft.Model.FinanceStructure.MBatchRegisterExpenseInfo info)
        {
            //<ROOT><Info TourId="计划编号" /></ROOT>
            StringBuilder xml = new StringBuilder();

            xml.Append("<ROOT>");
            foreach (var id in info.TourIds)
            {
                xml.AppendFormat("<Info TourId=\"{0}\" />", id);
            }
            xml.Append("</ROOT>");

            DbCommand cmd = this._db.GetStoredProcCommand("proc_BatchRegisterExpense");
            this._db.AddInParameter(cmd, "PaymentTime", DbType.DateTime, info.PaymentTime);
            this._db.AddInParameter(cmd, "PaymentType", DbType.Byte, info.PaymentType);
            this._db.AddInParameter(cmd, "PayerId", DbType.Int32, info.PayerId);
            this._db.AddInParameter(cmd, "Payer", DbType.String, info.Payer);
            this._db.AddInParameter(cmd, "Remark", DbType.String, info.Remark);            
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, info.OperatorId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, info.CompanyId);
            this._db.AddInParameter(cmd, "TourIdsXML", DbType.String, xml.ToString());
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            this._db.AddInParameter(cmd, "GysName", DbType.String, info.SearchGYSName);
            if (info.SearchGYSType.HasValue)
            {
                _db.AddInParameter(cmd, "GysType", DbType.Byte, info.SearchGYSType.Value);
            }
            else
            {
                _db.AddInParameter(cmd, "GysType", DbType.Byte, 255);
            }
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

        /*/// <summary>
        /// 批量审批付款
        /// </summary>
        /// <param name="operatorId">操作人编号</param>
        /// <param name="registerIds">付款登记编号集合</param>
        /// <returns></returns>
        public int BatchApprovalExpense(int operatorId, IList<string> registerIds)
        {
            throw new NotImplementedException("未实现。");
        }

        /// <summary>
        /// 批量支付付款
        /// </summary>
        /// <param name="operatorId">操作人编号</param>
        /// <param name="registerIds">付款登记编号集合</param>
        /// <returns></returns>
        public int BatchPayExpense(int operatorId, IList<string> registerIds)
        {
            throw new NotImplementedException("未实现。");
        }*/

        #endregion
    }
}
