//汪奇志 2012-08-24
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using EyouSoft.Model.TourStructure;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.FinanceStructure
{
    /// <summary>
    /// 团款收入相关数据访问类
    /// </summary>
    public class DShouRu : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.FinanceStructure.IShouRu
    {
        #region constructor
        /// <summary>
        /// database
        /// </summary>
        private Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public DShouRu()
        {
            _db = SystemStore;
        }
        #endregion

        #region static constants
        //static constants
        const string SQL_UPDATE_ShenHeShouKuan = "UPDATE [tbl_ReceiveRefund] SET [IsCheck]=1,[CheckerId]=@OperatorId,[ShenHeTime]=@ShenHeTime WHERE [Id]=@DengJiId AND [IsReceive]=1";
        #endregion

        #region private members
        /// <summary>
        /// 获取财务管理-团款收入-批量收款审核列表查询条件SQL
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        string SQLWhere_GetShouKuanShenHeLB(EyouSoft.Model.FinanceStructure.MLBShouKuanShenHeSearchInfo searchInfo)
        {
            StringBuilder cmdText = new StringBuilder();
            if (searchInfo != null)
            {
                if (searchInfo.CTETime.HasValue)
                {
                    cmdText.AppendFormat(" AND LeaveDate<'{0}' ", searchInfo.CTETime.Value.AddDays(1));
                }
                if (searchInfo.CTSTime.HasValue)
                {
                    cmdText.AppendFormat(" AND LeaveDate>'{0}' ", searchInfo.CTSTime.Value.AddDays(-1));
                }
                if (!string.IsNullOrEmpty(searchInfo.KeHuMingCheng))
                {
                    cmdText.AppendFormat(" AND BuyCompanyName LIKE '%{0}%' ", searchInfo.KeHuMingCheng);
                }
                if (!string.IsNullOrEmpty(searchInfo.OrderCode))
                {
                    cmdText.AppendFormat(" AND OrderNo LIKE '%{0}%' ", searchInfo.OrderCode);
                }
                if (searchInfo.SKETime.HasValue)
                {
                    cmdText.AppendFormat(" AND RefundDate<'{0}' ", searchInfo.SKETime.Value.AddDays(1));
                }
                if (searchInfo.SKStatus.HasValue)
                {
                    cmdText.AppendFormat(" AND IsCheck={0} ", searchInfo.SKStatus.Value ? "1" : "0");
                }
                if (searchInfo.SKSTime.HasValue)
                {
                    cmdText.AppendFormat(" AND RefundDate>'{0}' ", searchInfo.SKSTime.Value.AddDays(-1));
                }
                if (!string.IsNullOrEmpty(searchInfo.TourCode))
                {
                    cmdText.AppendFormat(" AND TourCode LIKE '%{0}%' ", searchInfo.TourCode);
                }
                if (searchInfo.TongJiDingDanFangShi == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计有效订单)
                {
                    cmdText.AppendFormat(" AND OrderState IN({0},{1},{2}) ", (int)Model.EnumType.TourStructure.OrderState.未处理, (int)Model.EnumType.TourStructure.OrderState.已成交, (int)Model.EnumType.TourStructure.OrderState.已留位);
                }
                else
                {
                    cmdText.AppendFormat(" AND OrderState = {0} ", (int)Model.EnumType.TourStructure.OrderState.已成交);
                }
            }

            return cmdText.ToString();
        }
        #endregion

        #region EyouSoft.IDAL.FinanceStructure.IShouRu 成员
        /// <summary>
        /// 获取财务管理-团款收入-批量收款审核列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.MLBShouKuanShenHeInfo> GetShouKuanShenHeLB(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.FinanceStructure.MLBShouKuanShenHeSearchInfo searchInfo)
        {
            IList<EyouSoft.Model.FinanceStructure.MLBShouKuanShenHeInfo> items = new List<EyouSoft.Model.FinanceStructure.MLBShouKuanShenHeInfo>();

            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "view_FinShouKuanDengJi";
            string primaryKey = "Id";
            string orderByString = "RefundDate DESC";
            string fields = "*";

            #region SQL
            cmdQuery.AppendFormat(" CompanyId={0} ", companyId);
            cmdQuery.Append(SQLWhere_GetShouKuanShenHeLB(searchInfo));
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(_db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    var item = new EyouSoft.Model.FinanceStructure.MLBShouKuanShenHeInfo();

                    item.KeHuMingCheng = rdr["BuyCompanyName"].ToString();
                    item.OrderCode = rdr["OrderNo"].ToString();
                    item.OrderId = rdr["OrderId"].ToString();
                    item.SKDengJiId = rdr["Id"].ToString();
                    item.SKFangShi = (EyouSoft.Model.EnumType.TourStructure.RefundType)rdr.GetByte(rdr.GetOrdinal("RefundType"));
                    item.SKJinE = rdr.GetDecimal(rdr.GetOrdinal("RefundMoney"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("RefundDate"))) item.SKRiQi = rdr.GetDateTime(rdr.GetOrdinal("RefundDate"));
                    item.SKStatus = rdr.GetByte(rdr.GetOrdinal("IsCheck")) == 1;
                    item.TourCode = rdr["TourCode"].ToString();
                    item.SKBeiZhu = rdr["Remark"].ToString();
                    item.SKRenName = rdr["StaffName"].ToString();

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取财务管理-团款收入-批量收款审核列表合计
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="searchInfo"></param>
        /// <param name="skJinEHeJi">收款金额合计</param>
        public void GetShouKuanShenHeLBHeJi(int companyId, EyouSoft.Model.FinanceStructure.MLBShouKuanShenHeSearchInfo searchInfo, out decimal skJinEHeJi)
        {
            skJinEHeJi = 0;

            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat(" SELECT SUM([RefundMoney]) AS SKJinEHeJi FROM view_FinShouKuanDengJi WHERE CompanyId={0} ", companyId);
            cmdText.Append(SQLWhere_GetShouKuanShenHeLB(searchInfo));

            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) skJinEHeJi = rdr.GetDecimal(0);
                }
            }
        }

        /// <summary>
        /// 审核收款，返回1成功，其它失败
        /// </summary>
        /// <param name="dengJiId">登记编号</param>
        /// <param name="operatorId">审核人编号</param>
        /// <returns></returns>
        public int ShenHeShouKuan(string dengJiId, int operatorId)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_UPDATE_ShenHeShouKuan);

            _db.AddInParameter(cmd, "OperatorId", DbType.Int32, operatorId);
            _db.AddInParameter(cmd, "DengJiId", DbType.AnsiStringFixedLength, dengJiId);
            _db.AddInParameter(cmd, "ShenHeTime", DbType.DateTime, DateTime.Now);

            return DbHelper.ExecuteSql(cmd, _db) == 1 ? 1 : -1;
        }
        #endregion
    }
}
