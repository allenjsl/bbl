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
    /// 支出明细数据访问
    /// </summary>
    /// 2011-01-23
    public class OutRegister : EyouSoft.Toolkit.DAL.DALBase, IDAL.FinanceStructure.IOutRegister
    {
        #region 私有变量

        private readonly Database _db = null;

        /// <summary>
        /// 支出明细插入
        /// </summary>
        private const string Sql_OutRegister_Insert = @" INSERT INTO [tbl_FinancialPayInfo] ([Id],[CompanyID],[ItemId],[ItemType],[ReceiveId],[ReceiveType],[ReceiveCompanyId],[ReceiveCompanyName],[PaymentDate],[StaffNo],[StaffName],[PaymentAmount],[PaymentType],[IsBill],[BillAmount],[BillNo],[Remark],[IsChecked],[OperatorID],[IssueTime],[CheckerId],[IsPay]) VALUES (@Id,@CompanyID,@ItemId,@ItemType,@ReceiveId,@ReceiveType,@ReceiveCompanyId,@ReceiveCompanyName,@PaymentDate,@StaffNo,@StaffName,@PaymentAmount,@PaymentType,@IsBill,@BillAmount,@BillNo,@Remark,@IsChecked,@OperatorID,@IssueTime,@CheckerId,@IsPay) ";
        /// <summary>
        /// 支出明细修改
        /// </summary>
        private const string Sql_OutRegister_Update = @" UPDATE [tbl_FinancialPayInfo] SET [PaymentDate] = @PaymentDate,[StaffNo] = @StaffNo,[StaffName] = @StaffName,[PaymentAmount] = @PaymentAmount,[PaymentType] = @PaymentType,[IsBill] = @IsBill,[BillAmount] = @BillAmount,[BillNo] = @BillNo,[Remark] = @Remark WHERE Id = @Id ";
        /// <summary>
        /// 支出明细设置审批状态
        /// </summary>
        private const string Sql_OutRegister_SetCheck = @" UPDATE [tbl_FinancialPayInfo] SET [IsChecked] = @IsChecked,[CheckerId] = @CheckerId WHERE Id ";
        /// <summary>
        /// 支出明细设置支付状态
        /// </summary>
        private const string Sql_OutRegister_SetPay = @" UPDATE [tbl_FinancialPayInfo] SET [IsPay] = @IsPay WHERE Id ";
        /// <summary>
        /// 支出明细查询
        /// </summary>
        private const string Sql_OutRegister_Select = @" SELECT [Id],[CompanyID],[ItemId],[ItemType],[ReceiveId],[ReceiveType],[ReceiveCompanyId],[ReceiveCompanyName],[PaymentDate],[StaffNo],[StaffName],[PaymentAmount],[PaymentType],[IsBill],[BillAmount],[BillNo],[Remark],[IsChecked],[OperatorID],[IssueTime],[CheckerId],[IsPay] FROM [tbl_FinancialPayInfo] ";
        

        /// <summary>
        /// 构造函数
        /// </summary>
        public OutRegister()
        {
            _db = base.SystemStore;
        }

        #endregion

        #region IOutRegister 成员

        /// <summary>
        /// 添加一条支出明细
        /// </summary>
        /// <param name="model">支出明细实体</param>
        /// <returns>返回1成功，其他失败</returns>
        public int AddOutRegister(EyouSoft.Model.FinanceStructure.OutRegisterInfo model)
        {
            DbCommand dc = _db.GetSqlStringCommand(Sql_OutRegister_Insert);
            model.RegisterId = Guid.NewGuid().ToString();
            _db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, model.RegisterId);
            _db.AddInParameter(dc, "CompanyID", DbType.Int32, model.CompanyId);
            _db.AddInParameter(dc, "ItemId", DbType.AnsiStringFixedLength, model.ItemId);
            _db.AddInParameter(dc, "ItemType", DbType.Byte, (int)model.RegisterType);
            _db.AddInParameter(dc, "ReceiveId", DbType.AnsiStringFixedLength, model.ReceiveId);
            _db.AddInParameter(dc, "ReceiveType", DbType.Byte, (int)model.ReceiveType);
            _db.AddInParameter(dc, "ReceiveCompanyId", DbType.Int32, model.ReceiveCompanyId);
            _db.AddInParameter(dc, "ReceiveCompanyName", DbType.String, model.ReceiveCompanyName);
            _db.AddInParameter(dc, "PaymentDate", DbType.DateTime, model.PaymentDate);
            _db.AddInParameter(dc, "StaffNo", DbType.Int32, model.StaffNo);
            _db.AddInParameter(dc, "StaffName", DbType.String, model.StaffName);
            _db.AddInParameter(dc, "PaymentAmount", DbType.Decimal, model.PaymentAmount);
            _db.AddInParameter(dc, "PaymentType", DbType.Byte, (int)model.PaymentType);
            _db.AddInParameter(dc, "IsBill", DbType.AnsiStringFixedLength, model.IsBill ? "1" : "0");
            _db.AddInParameter(dc, "BillAmount", DbType.Decimal, model.BillAmount);
            _db.AddInParameter(dc, "BillNo", DbType.String, model.BillNo);
            _db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            _db.AddInParameter(dc, "IsChecked", DbType.AnsiStringFixedLength, model.IsChecked ? "1" : "0");
            _db.AddInParameter(dc, "OperatorID", DbType.Int32, model.OperatorId);
            _db.AddInParameter(dc, "IssueTime", DbType.DateTime, model.IssueTime);
            _db.AddInParameter(dc, "CheckerId", DbType.Int32, model.CheckerId);
            _db.AddInParameter(dc, "IsPay", DbType.AnsiStringFixedLength, model.IsPay ? "1" : "0");

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 修改支出明细
        /// </summary>
        /// <param name="model">支出明细实体</param>
        /// <returns>返回1成功，其他失败</returns>
        public int UpdateOutRegister(EyouSoft.Model.FinanceStructure.OutRegisterInfo model)
        {
            DbCommand dc = _db.GetSqlStringCommand(Sql_OutRegister_Update);
            _db.AddInParameter(dc, "PaymentDate", DbType.DateTime, model.PaymentDate);
            _db.AddInParameter(dc, "StaffNo", DbType.Int32, model.StaffNo);
            _db.AddInParameter(dc, "StaffName", DbType.String, model.StaffName);
            _db.AddInParameter(dc, "PaymentAmount", DbType.Decimal, model.PaymentAmount);
            _db.AddInParameter(dc, "PaymentType", DbType.Byte, (int)model.PaymentType);
            _db.AddInParameter(dc, "IsBill", DbType.AnsiStringFixedLength, model.IsBill ? "1" : "0");
            _db.AddInParameter(dc, "BillAmount", DbType.Decimal, model.BillAmount);
            _db.AddInParameter(dc, "BillNo", DbType.String, model.BillNo);
            _db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            _db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, model.RegisterId);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 将某支出明细设置为已审批状态
        /// </summary>
        /// <param name="IsChecked">是否审批</param>
        /// <param name="CheckerId">审批人Id</param>
        /// <param name="OutRegisterIds">支出明细Id集合</param>
        /// <returns>返回1成功，其他失败</returns>
        public int SetCheckedState(bool IsChecked, int CheckerId, params string[] OutRegisterIds)
        {
            if (OutRegisterIds == null || OutRegisterIds.Length <= 0 || CheckerId <= 0)
                return 0;

            string strSql = Sql_OutRegister_SetCheck + " in ( ";
            foreach (string str in OutRegisterIds)
            {
                strSql += "'" + str + "',";
            }
            strSql = strSql.Trim(',');
            strSql += "); ";
            
            DbCommand dc = _db.GetSqlStringCommand(strSql);
            _db.AddInParameter(dc, "IsChecked", DbType.AnsiStringFixedLength, IsChecked ? "1" : "0");
            _db.AddInParameter(dc, "CheckerId", DbType.Int32, CheckerId);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 将某支出明细设置为已支付
        /// </summary>
        /// <param name="IsPay">是否支付</param>
        /// <param name="OutRegisterIds">支出明细Id集合</param>
        /// <returns>返回1成功，其他失败</returns>
        public int SetIsPay(bool IsPay, params string[] OutRegisterIds)
        {
            if (OutRegisterIds == null || OutRegisterIds.Length <= 0)
                return 0;

            string strSql = Sql_OutRegister_SetPay + " in ( ";
            foreach (string str in OutRegisterIds)
            {
                strSql += "'" + str + "',";
            }
            strSql = strSql.Trim(',');
            strSql += "); ";

            DbCommand dc = _db.GetSqlStringCommand(strSql);
            _db.AddInParameter(dc, "IsPay", DbType.AnsiStringFixedLength, IsPay ? "1" : "0");

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 获取支出登记明细
        /// </summary>
        /// <param name="model">支出登记查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.OutRegisterInfo> GetOutRegisterList(EyouSoft.Model.FinanceStructure.QueryOutRegisterInfo model)
        {
            IList<EyouSoft.Model.FinanceStructure.OutRegisterInfo> list = new List<EyouSoft.Model.FinanceStructure.OutRegisterInfo>();
            string strOrder = string.Empty;
            DbCommand dc = _db.GetSqlStringCommand(Sql_OutRegister_Select + " where " + this.GetSqlWhere(model, string.Empty, ref strOrder) + (string.IsNullOrEmpty(strOrder) ? string.Empty : " order by " + strOrder));

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                EyouSoft.Model.FinanceStructure.OutRegisterInfo tmpModel = null;
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.FinanceStructure.OutRegisterInfo();

                    #region 实体赋值

                    tmpModel.RegisterId = dr["Id"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("CompanyID")))
                        tmpModel.CompanyId = int.Parse(dr["CompanyID"].ToString());
                    tmpModel.ItemId = dr["ItemId"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("ItemType")))
                        tmpModel.RegisterType = (Model.EnumType.FinanceStructure.OutRegisterType)int.Parse(dr["ItemType"].ToString());
                    tmpModel.ReceiveId = dr["ReceiveId"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("ReceiveType")))
                        tmpModel.ReceiveType = (Model.EnumType.FinanceStructure.OutPlanType)int.Parse(dr["ReceiveType"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("ReceiveCompanyId")))
                        tmpModel.ReceiveCompanyId = int.Parse(dr["ReceiveCompanyId"].ToString());
                    tmpModel.ReceiveCompanyName = dr["ReceiveCompanyName"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("PaymentDate")))
                        tmpModel.PaymentDate = DateTime.Parse(dr["PaymentDate"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("StaffNo")))
                        tmpModel.StaffNo = int.Parse(dr["StaffNo"].ToString());
                    tmpModel.StaffName = dr["StaffName"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("PaymentAmount")))
                        tmpModel.PaymentAmount = decimal.Parse(dr["PaymentAmount"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("PaymentType")))
                        tmpModel.PaymentType = (Model.EnumType.TourStructure.RefundType)int.Parse(dr["PaymentType"].ToString());
                    if (!string.IsNullOrEmpty(dr["IsBill"].ToString()) && (dr["IsBill"].ToString() == "1" || dr["IsBill"].ToString().ToLower() == "true"))
                        tmpModel.IsBill = true;
                    if (!dr.IsDBNull(dr.GetOrdinal("BillAmount")))
                        tmpModel.BillAmount = decimal.Parse(dr["BillAmount"].ToString());
                    tmpModel.BillNo = dr["BillNo"].ToString();
                    tmpModel.Remark = dr["Remark"].ToString();
                    if (!string.IsNullOrEmpty(dr["IsChecked"].ToString()) && (dr["IsChecked"].ToString() == "1" || dr["IsChecked"].ToString().ToLower() == "true"))
                        tmpModel.IsChecked = true;
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorID")))
                        tmpModel.OperatorId = int.Parse(dr["OperatorID"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                        tmpModel.IssueTime = DateTime.Parse(dr["IssueTime"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("CheckerId")))
                        tmpModel.CheckerId = int.Parse(dr["CheckerId"].ToString());
                    if (!string.IsNullOrEmpty(dr["IsPay"].ToString()) && (dr["IsPay"].ToString() == "1" || dr["IsPay"].ToString().ToLower() == "true"))
                        tmpModel.IsPay = true;

                    #endregion

                    list.Add(tmpModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取支出登记明细
        /// </summary>
        /// <param name="model">支出登记查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="PageSize">每页条数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.OutRegisterInfo> GetOutRegisterList(EyouSoft.Model.FinanceStructure.QueryOutRegisterInfo model, string HaveUserIds, int PageSize, int PageIndex, ref int RecordCount)
        {
            IList<EyouSoft.Model.FinanceStructure.OutRegisterInfo> list = new List<EyouSoft.Model.FinanceStructure.OutRegisterInfo>();
            string strFiles = @" [Id],[CompanyID],[ItemId],[ItemType],[ReceiveId],[ReceiveType],[ReceiveCompanyId],[ReceiveCompanyName],[PaymentDate],[StaffNo],[StaffName],[PaymentAmount],[PaymentType],[IsBill],[BillAmount],[BillNo],[Remark],[IsChecked],[OperatorID],[IssueTime],[CheckerId],[IsPay],
(case when [ItemType] = 0 then
   (select TourCode,RouteName from tbl_tour where tourid = [ItemId] for xml auto,root('root'))
 else
	''
 end
) as TourInfo ";
            string strOrder = string.Empty;
            string strWhere = this.GetSqlWhere(model, HaveUserIds, ref strOrder);

            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, "tbl_FinancialPayInfo"
                , "Id", strFiles, strWhere, strOrder))
            {
                EyouSoft.Model.FinanceStructure.OutRegisterInfo tmpModel = null;
                System.Xml.XmlDocument xml = null;
                System.Xml.XmlNodeList xmlNodeList = null;
                while (dr.Read())
                {
                    tmpModel = new EyouSoft.Model.FinanceStructure.OutRegisterInfo();

                    #region 实体赋值

                    tmpModel.RegisterId = dr["Id"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("CompanyID")))
                        tmpModel.CompanyId = int.Parse(dr["CompanyID"].ToString());
                    tmpModel.ItemId = dr["ItemId"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("ItemType")))
                        tmpModel.RegisterType = (Model.EnumType.FinanceStructure.OutRegisterType)int.Parse(dr["ItemType"].ToString());
                    tmpModel.ReceiveId = dr["ReceiveId"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("ReceiveType")))
                        tmpModel.ReceiveType = (Model.EnumType.FinanceStructure.OutPlanType)int.Parse(dr["ReceiveType"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("ReceiveCompanyId")))
                        tmpModel.ReceiveCompanyId = int.Parse(dr["ReceiveCompanyId"].ToString());
                    tmpModel.ReceiveCompanyName = dr["ReceiveCompanyName"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("PaymentDate")))
                        tmpModel.PaymentDate = DateTime.Parse(dr["PaymentDate"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("StaffNo")))
                        tmpModel.StaffNo = int.Parse(dr["StaffNo"].ToString());
                    tmpModel.StaffName = dr["StaffName"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("PaymentAmount")))
                        tmpModel.PaymentAmount = decimal.Parse(dr["PaymentAmount"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("PaymentType")))
                        tmpModel.PaymentType = (Model.EnumType.TourStructure.RefundType)int.Parse(dr["PaymentType"].ToString());
                    if (!string.IsNullOrEmpty(dr["IsBill"].ToString()) && (dr["IsBill"].ToString() == "1" || dr["IsBill"].ToString().ToLower() == "true"))
                        tmpModel.IsBill = true;
                    if (!dr.IsDBNull(dr.GetOrdinal("BillAmount")))
                        tmpModel.BillAmount = decimal.Parse(dr["BillAmount"].ToString());
                    tmpModel.BillNo = dr["BillNo"].ToString();
                    tmpModel.Remark = dr["Remark"].ToString();
                    if (!string.IsNullOrEmpty(dr["IsChecked"].ToString()) && (dr["IsChecked"].ToString() == "1" || dr["IsChecked"].ToString().ToLower() == "true"))
                        tmpModel.IsChecked = true;
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorID")))
                        tmpModel.OperatorId = int.Parse(dr["OperatorID"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                        tmpModel.IssueTime = DateTime.Parse(dr["IssueTime"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("CheckerId")))
                        tmpModel.CheckerId = int.Parse(dr["CheckerId"].ToString());
                    if (!string.IsNullOrEmpty(dr["IsPay"].ToString()) && (dr["IsPay"].ToString() == "1" || dr["IsPay"].ToString().ToLower() == "true"))
                        tmpModel.IsPay = true;

                    if (!dr.IsDBNull(dr.GetOrdinal("TourInfo")))
                    {
                        xml = new System.Xml.XmlDocument();
                        xml.LoadXml(dr.GetString(dr.GetOrdinal("TourInfo")));
                        xmlNodeList = xml.GetElementsByTagName("tbl_tour");
                        if (xmlNodeList != null && xmlNodeList.Count > 0)
                        {
                            if (xmlNodeList[0].Attributes["TourCode"] != null && !string.IsNullOrEmpty(xmlNodeList[0].Attributes["TourCode"].Value))
                                tmpModel.TourCode = xmlNodeList[0].Attributes["TourCode"].Value;
                            if (xmlNodeList[0].Attributes["RouteName"] != null && !string.IsNullOrEmpty(xmlNodeList[0].Attributes["RouteName"].Value))
                                tmpModel.RouteName = xmlNodeList[0].Attributes["RouteName"].Value;
                        }
                    }

                    #endregion

                    list.Add(tmpModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取计调项目支出已登记金额
        /// </summary>
        /// <param name="registerId">登记编号 不为null时计调项目支出已登记金额不含自身金额 为null时计调项目支出已登记金额</param>
        /// <param name="planId">计调项目编号</param>
        /// <param name="planType">计调项目类型</param>
        /// <param name="expenseAmount">计调项目支出金额</param>
        /// <returns></returns>
        public decimal GetExpenseRegisterAmount(string registerId, string planId, Model.EnumType.FinanceStructure.OutPlanType planType, out decimal expenseAmount)
        {
            expenseAmount = 0;
            decimal registerAmount = 0;

            DbCommand cmd = this._db.GetStoredProcCommand("proc_Financial_GetExpenseRegisterAmount");            
            this._db.AddInParameter(cmd, "PlanId", DbType.AnsiStringFixedLength, planId);
            this._db.AddInParameter(cmd, "PlanType", DbType.Byte, planType);
            if (!string.IsNullOrEmpty(registerId))
            {
                this._db.AddInParameter(cmd, "RegisterId", DbType.AnsiStringFixedLength, registerId);
            }
            else
            {
                this._db.AddInParameter(cmd, "RegisterId", DbType.AnsiStringFixedLength, DBNull.Value);
            }

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    expenseAmount = rdr.IsDBNull(rdr.GetOrdinal("ExpenseAmount")) ? 0 : rdr.GetDecimal(rdr.GetOrdinal("ExpenseAmount"));
                    registerAmount = rdr.IsDBNull(rdr.GetOrdinal("RegisterAmount")) ? 0 : rdr.GetDecimal(rdr.GetOrdinal("RegisterAmount"));
                }
            }

            return registerAmount;
        }

        /// <summary>
        /// 获取支出登记明细金额合计
        /// </summary>
        /// <param name="model">支出登记查询实体</param>
        /// <param name="haveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="paymentAmount">付款金额</param>
        public void GetOutRegisterList(Model.FinanceStructure.QueryOutRegisterInfo model, string haveUserIds, ref decimal paymentAmount)
        {
            if(model == null)
                return;

            var strSql = new StringBuilder();
            string strOrder = string.Empty;
            strSql.Append(" select sum(PaymentAmount) from tbl_FinancialPayInfo where ");
            strSql.Append(GetSqlWhere(model, haveUserIds, ref strOrder));

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            using (IDataReader dr = DbHelper.ExecuteReader(dc,_db))
            {
                if(dr.Read())
                {
                    if (!dr.IsDBNull(0))
                        paymentAmount = dr.GetDecimal(0);
                }
            }
        }

        #endregion

        #region 私有函数

        /// <summary>
        /// 根据查询实体生成SqlWhere子句
        /// </summary>
        /// <param name="model">查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <param name="strOrder">排序语句</param>
        /// <returns>SqlWhere子句</returns>
        private string GetSqlWhere(EyouSoft.Model.FinanceStructure.QueryOutRegisterInfo model, string HaveUserIds
            , ref string strOrder)
        {
            if (model == null)
                return string.Empty;

            StringBuilder strSqlWhere = new StringBuilder(" 1 = 1 ");
            if (model.CompanyId > 0)
                strSqlWhere.AppendFormat(" and CompanyID = {0} ", model.CompanyId);
            if (!string.IsNullOrEmpty(model.ItemId))
                strSqlWhere.AppendFormat(" and ItemId = '{0}' ", model.ItemId);
            if (model.RegisterType.HasValue)
                strSqlWhere.AppendFormat(" and ItemType = {0} ", (int)model.RegisterType);
            if (!string.IsNullOrEmpty(model.ReceiveId))
                strSqlWhere.AppendFormat(" and ReceiveId = '{0}' ", model.ReceiveId);
            if (model.ReceiveType.HasValue)
                strSqlWhere.AppendFormat(" and ReceiveType = {0} ", (int)model.ReceiveType);
            if (!string.IsNullOrEmpty(model.TourNo))
                strSqlWhere.AppendFormat(" and (ItemType = 0 and ItemId in (select TourId from tbl_Tour where TourCode like '%{0}%')) ", model.TourNo);
            /*if (model.OperatorId > 0)
                strSqlWhere.AppendFormat(" and StaffNo = {0} ", model.OperatorId);*/
            if (model.StartTime.HasValue)
                strSqlWhere.AppendFormat(" and DATEDIFF(dd,PaymentDate,'{0}') <= 0 ", model.StartTime.Value.ToShortDateString());
            if (model.EndTime.HasValue)
                strSqlWhere.AppendFormat(" and DATEDIFF(dd,PaymentDate,'{0}') >= 0 ", model.EndTime.Value.ToShortDateString());
            if (!string.IsNullOrEmpty(HaveUserIds))
            {
                //strSqlWhere.AppendFormat(" and OperatorID in ({0}) ", HaveUserIds);
                strSqlWhere.AppendFormat(" AND EXISTS (SELECT 1 FROM tbl_Tour AS A WHERE A.TourId=tbl_FinancialPayInfo.ItemId AND A.OperatorId IN({0}) ) ", HaveUserIds);
            }
            if (model.ReceiveCompanyId > 0)
                strSqlWhere.AppendFormat(" and ReceiveCompanyId = {0} ", model.ReceiveCompanyId);
            if (!string.IsNullOrEmpty(model.ReceiveCompanyName))
                strSqlWhere.AppendFormat(" and ReceiveCompanyName like '%{0}%' ", model.ReceiveCompanyName);
            if (model.OperatorId > 0)
                strSqlWhere.AppendFormat(" AND OperatorID={0} ", model.OperatorId);
            if (model.FTime.HasValue)
                strSqlWhere.AppendFormat(" AND DATEDIFF(DAY,'{0}',PaymentDate)=0 ", model.FTime.Value);
            if (model.RegisterStatus.HasValue)
            {
                switch (model.RegisterStatus.Value)
                {
                    case 1:
                        strSqlWhere.Append(" AND IsChecked='0' ");
                        break;
                    case 2:
                        strSqlWhere.Append(" AND IsChecked='1' AND IsPay='0' ");
                        break;
                }
            }

            switch (model.OrderIndex)
            {
                case 0:
                    strOrder = " PaymentDate asc ";
                    break;
                case 1:
                    strOrder = " PaymentDate desc ";
                    break;
            }


            return strSqlWhere.ToString();
        }

        #endregion
    }
}
