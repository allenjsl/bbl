//汪奇志 2012-08-17
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
    /// 发票管理数据访问类
    /// </summary>
    public class DFaPiao : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.FinanceStructure.IFaPiao
    {
        #region constructor
        /// <summary>
        /// database
        /// </summary>
        private Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public DFaPiao()
        {
            _db = SystemStore;
        }
        #endregion

        #region static constants
        //static constants
        const string SQL_INSERT_Insert = "INSERT INTO [tbl_FinFaPiao]([Id],[CompanyId],[CrmId],[RiQi],[JinE],[PiaoHao],[KaiPiaoRenId],[KaiPiaoRen],[BeiZhu],[CaoZuoRenId],[IssueTime],[IsDelete]) VALUES (@Id,@CompanyId,@CrmId,@RiQi,@JinE,@PiaoHao,@KaiPiaoRenId,@KaiPiaoRen,@BeiZhu,@CaoZuoRenId,@IssueTime,@IsDelete)";
        const string SQL_UPDATE_Update = "UPDATE [tbl_FinFaPiao] SET [RiQi]=@RiQi,[JinE]=@JinE,[PiaoHao]=@PiaoHao,[KaiPiaoRenId]=@KaiPiaoRenId,[KaiPiaoRen]=@KaiPiaoRen,[BeiZhu]=@BeiZhu WHERE [Id]=@Id";
        const string SQL_UPDATE_Delete = "UPDATE [tbl_FinFaPiao] SET [IsDelete]='1' WHERE [Id]=@Id";
        const string SQL_SELECT_GetInfo = "SELECT * FROM [tbl_FinFaPiao] WHERE [Id]=@Id";
        #endregion

        #region EyouSoft.IDAL.FinanceStructure.IFaPiao 成员
        /// <summary>
        /// 登记开票信息，操作成功返回开票登记编号，失败返回0。
        /// </summary>
        /// <param name="info">开票信息业务实体</param>
        /// <returns></returns>
        public int Insert(EyouSoft.Model.FinanceStructure.MFaPiaoInfo info)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_INSERT_Insert);

            _db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, info.Id);
            _db.AddInParameter(cmd, "CompanyId", DbType.Int32, info.CompanyId);
            _db.AddInParameter(cmd, "CrmId", DbType.Int32, info.CrmId);
            _db.AddInParameter(cmd, "RiQi", DbType.DateTime, info.RiQi);
            _db.AddInParameter(cmd, "JinE", DbType.Decimal, info.JinE);
            _db.AddInParameter(cmd, "PiaoHao", DbType.String, info.PiaoHao);
            _db.AddInParameter(cmd, "KaiPiaoRenId", DbType.Int32, info.KaiPiaoRenId);
            _db.AddInParameter(cmd, "KaiPiaoRen", DbType.String, info.KaiPiaoRen);
            _db.AddInParameter(cmd, "BeiZhu", DbType.String, info.BeiZhu);
            _db.AddInParameter(cmd, "CaoZuoRenId", DbType.Int32, info.CaoZuoRenId);
            _db.AddInParameter(cmd, "IssueTime", DbType.DateTime, DateTime.Now);
            _db.AddInParameter(cmd, "IsDelete", DbType.AnsiStringFixedLength, "0");

            return DbHelper.ExecuteSql(cmd, _db) == 1 ? 1 : -1;
        }

        /// <summary>
        /// 修改开票信息，操作成功返回1，负值失败。
        /// </summary>
        /// <param name="info">开票信息业务实体</param>
        /// <returns></returns>
        public int Update(EyouSoft.Model.FinanceStructure.MFaPiaoInfo info)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_UPDATE_Update);

            _db.AddInParameter(cmd, "RiQi", DbType.DateTime, info.RiQi);
            _db.AddInParameter(cmd, "JinE", DbType.Decimal, info.JinE);
            _db.AddInParameter(cmd, "PiaoHao", DbType.String, info.PiaoHao);
            _db.AddInParameter(cmd, "KaiPiaoRenId", DbType.Int32, info.KaiPiaoRenId);
            _db.AddInParameter(cmd, "KaiPiaoRen", DbType.String, info.KaiPiaoRen);
            _db.AddInParameter(cmd, "BeiZhu", DbType.String, info.BeiZhu);
            _db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, info.Id);

            return DbHelper.ExecuteSql(cmd, _db) == 1 ? 1 : -1;
        }

        /// <summary>
        /// 删除开票信息，操作成功返回1，负值失败。
        /// </summary>
        /// <param name="faPiaoId">发票登记编号</param>
        /// <returns></returns>
        public int Delete(string faPiaoId)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_UPDATE_Delete);
            _db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, faPiaoId);

            return DbHelper.ExecuteSql(cmd, _db) == 1 ? 1 : -1;
        }

        /// <summary>
        /// 获取发票登记信息业务实体
        /// </summary>
        /// <param name="faPiaoId">发票登记编号</param>
        /// <returns></returns>
        public EyouSoft.Model.FinanceStructure.MFaPiaoInfo GetInfo(string faPiaoId)
        {
            EyouSoft.Model.FinanceStructure.MFaPiaoInfo info = null;
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_GetInfo);
            _db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, faPiaoId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    info = new EyouSoft.Model.FinanceStructure.MFaPiaoInfo();

                    info.BeiZhu = rdr["BeiZhu"].ToString();
                    info.CaoZuoRenId = rdr.GetInt32(rdr.GetOrdinal("CaoZuoRenId"));
                    info.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    info.CrmId = rdr.GetInt32(rdr.GetOrdinal("CrmId"));
                    info.Id = faPiaoId;
                    info.JinE = rdr.GetDecimal(rdr.GetOrdinal("JinE"));
                    info.KaiPiaoRen = rdr["KaiPiaoRen"].ToString();
                    info.KaiPiaoRenId = rdr.GetInt32(rdr.GetOrdinal("KaiPiaoRenId"));
                    info.PiaoHao = rdr["PiaoHao"].ToString();
                    info.RiQi = rdr.GetDateTime(rdr.GetOrdinal("RiQi"));
                }
            }

            return info;
        }

        /// <summary>
        /// 获取发票管理列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="searchInfo">查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.MFaPiaoGuanLiInfo> GetFaPiaoGuanLis(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.FinanceStructure.MFaPiaoGuanLiSearchInfo searchInfo)
        {
            IList<EyouSoft.Model.FinanceStructure.MFaPiaoGuanLiInfo> items = new List<EyouSoft.Model.FinanceStructure.MFaPiaoGuanLiInfo>();

            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_Customer";
            string primaryKey = "Id";
            string orderByString = "IssueTime DESC";
            StringBuilder fields = new StringBuilder();

            #region fields
            fields.Append(" Id,Name ");

            //交易金额
            fields.Append(" ,(SELECT SUM(A.FinanceSum) FROM tbl_TourOrder AS A WHERE A.BuyCompanyId=tbl_Customer.Id AND A.IsDelete='0' AND A.OrderState=5 ");
            if (searchInfo != null)
            {
                if (searchInfo.CTETime.HasValue)
                {
                    fields.AppendFormat(" AND A.LeaveDate<'{0}' ", searchInfo.CTETime.Value.AddDays(1));
                }
                if (searchInfo.CTSTime.HasValue)
                {
                    fields.AppendFormat(" AND A.LeaveDate>'{0}' ", searchInfo.CTSTime.Value.AddDays(-1));
                }
            }
            fields.Append(" ) AS JiaoYiJinE ");

            //开票金额
            fields.Append(" ,(SELECT SUM(A.JinE) FROM tbl_FinFaPiao AS A WHERE A.CrmId=tbl_Customer.Id AND A.IsDelete='0' ");
            if (searchInfo != null)
            {
                if (searchInfo.KPETime.HasValue)
                {
                    fields.AppendFormat(" AND A.RiQi<'{0}' ", searchInfo.KPETime.Value.AddDays(1));
                }
                if (!string.IsNullOrEmpty(searchInfo.KPRen))
                {
                    fields.AppendFormat(" AND A.KaiPiaoRen LIKE '%{0}%' ", searchInfo.KPRen);
                }
                if (searchInfo.KPSTime.HasValue)
                {
                    fields.AppendFormat(" AND A.RiQi>'{0}' ", searchInfo.KPSTime.Value.AddDays(-1));
                }
            }
            fields.Append(" ) AS KaiPiaoJinE ");
            #endregion

            #region 拼接查询条件
            cmdQuery.AppendFormat(" CompanyId={0} AND IsDelete='0' ", companyId);
            if (searchInfo != null)
            {
                if (!string.IsNullOrEmpty(searchInfo.CrmName))
                {
                    cmdQuery.AppendFormat(" AND Name LIKE '%{0}%' ", searchInfo.CrmName);
                }
            }           
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                int _index_Id = rdr.GetOrdinal("Id");
                int _index_Name = rdr.GetOrdinal("Name");
                int _index_JiaoYiJinE = rdr.GetOrdinal("JiaoYiJinE");
                int _index_KaiPiaoJinE = rdr.GetOrdinal("KaiPiaoJinE");

                while (rdr.Read())
                {
                    var item =new  EyouSoft.Model.FinanceStructure.MFaPiaoGuanLiInfo();

                    item.CrmId = rdr.GetInt32(_index_Id);
                    item.CrmName = rdr[_index_Name].ToString();

                    if (!rdr.IsDBNull(_index_JiaoYiJinE)) item.JiaoYiJinE = rdr.GetDecimal(_index_JiaoYiJinE);
                    if (!rdr.IsDBNull(_index_KaiPiaoJinE)) item.KaiPiaoJinE = rdr.GetDecimal(_index_KaiPiaoJinE);

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取发票管理列表合计信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询实体</param>
        /// <param name="jiaoYiJinE">交易金额合计</param>
        /// <param name="kaiPiaoJinE">开票金额合计</param>
        public void GetFaPiaoGuanLisHeJi(int companyId, EyouSoft.Model.FinanceStructure.MFaPiaoGuanLiSearchInfo searchInfo, out decimal jiaoYiJinE, out decimal kaiPiaoJinE)
        {            
            StringBuilder cmdText = new StringBuilder();
            jiaoYiJinE = 0;
            kaiPiaoJinE = 0;
        
            #region SQL 
            //cmdtext:SELECT SUM(C.F1),SUM(C.F2) FROM (SELECT (交易金额查询) AS F1,(开票金额查询) AS F2 FROM A )C
            cmdText.Append("SELECT SUM(JiaoYiJinE) AS JiaoYiJinE,SUM(KaiPiaoJinE) AS KaiPiaoJinE FROM (");

            cmdText.AppendFormat(" SELECT ");

            cmdText.Append(" (SELECT ISNULL(SUM(B.FinanceSum),0) FROM tbl_TourOrder AS B WHERE B.BuyCompanyId=A.Id AND B.IsDelete='0' AND B.OrderState=5 ");
            if (searchInfo != null)
            {
                if (searchInfo.CTETime.HasValue)
                {
                    cmdText.AppendFormat(" AND B.LeaveDate<'{0}' ", searchInfo.CTETime.Value.AddDays(1));
                }
                if (searchInfo.CTSTime.HasValue)
                {
                    cmdText.AppendFormat(" AND B.LeaveDate>'{0}' ", searchInfo.CTSTime.Value.AddDays(-1));
                }
            }
            cmdText.Append(" ) AS JiaoYiJinE ");

            cmdText.Append(" ,(SELECT ISNULL(SUM(B.JinE),0) FROM tbl_FinFaPiao AS B WHERE B.CrmId=A.Id AND B.IsDelete='0' ");
            if (searchInfo != null)
            {
                if (searchInfo.KPETime.HasValue)
                {
                    cmdText.AppendFormat(" AND B.RiQi<'{0}' ", searchInfo.KPETime.Value.AddDays(1));
                }
                if (!string.IsNullOrEmpty(searchInfo.KPRen))
                {
                    cmdText.AppendFormat(" AND B.KaiPiaoRen LIKE '%{0}%' ", searchInfo.KPRen);
                }
                if (searchInfo.KPSTime.HasValue)
                {
                    cmdText.AppendFormat(" AND B.RiQi>'{0}' ", searchInfo.KPSTime.Value.AddDays(-1));
                }
            }
            cmdText.Append(" ) AS KaiPiaoJinE ");

            cmdText.AppendFormat(" FROM tbl_Customer AS A ");
            cmdText.AppendFormat(" WHERE A.CompanyId={0} AND A.IsDelete='0' ", companyId);
            if (searchInfo != null)
            {
                if (!string.IsNullOrEmpty(searchInfo.CrmName))
                {
                    cmdText.AppendFormat(" AND A.Name LIKE '%{0}%' ", searchInfo.CrmName);
                }
            }

            cmdText.Append(")C");
            #endregion

            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) jiaoYiJinE = rdr.GetDecimal(0);
                    if (!rdr.IsDBNull(1)) kaiPiaoJinE = rdr.GetDecimal(1);
                }
            }
        }

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
        public IList<EyouSoft.Model.FinanceStructure.MFaPiaoInfo> GetFaPiaos(int companyId, int crmId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.FinanceStructure.MFaPiaoSearchInfo searchInfo)
        {
            IList<EyouSoft.Model.FinanceStructure.MFaPiaoInfo> items = new List<EyouSoft.Model.FinanceStructure.MFaPiaoInfo>();

            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_FinFaPiao";
            string primaryKey = "Id";
            string orderByString = "RiQi DESC";
            string fields = "*";

            #region SQL
            cmdQuery.AppendFormat(" IsDelete='0' AND CompanyId={0} AND CrmId={1} ", companyId, crmId);

            if (searchInfo != null)
            {
                if (searchInfo.KPETime.HasValue)
                {
                    cmdQuery.AppendFormat(" AND RiQi<'{0}' ", searchInfo.KPETime.Value.AddDays(1));
                }
                if (!string.IsNullOrEmpty(searchInfo.KPRen))
                {
                    cmdQuery.AppendFormat(" AND KaiPiaoRen LIKE '%{0}%' ", searchInfo.KPRen);
                }
                if (searchInfo.KPSTime.HasValue)
                {
                    cmdQuery.AppendFormat(" AND RiQi>'{0}' ", searchInfo.KPSTime.Value.AddDays(-1));
                }
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    var item = new EyouSoft.Model.FinanceStructure.MFaPiaoInfo();

                    item.BeiZhu = rdr["BeiZhu"].ToString();
                    item.CaoZuoRenId = rdr.GetInt32(rdr.GetOrdinal("CaoZuoRenId"));
                    item.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                    item.CrmId = rdr.GetInt32(rdr.GetOrdinal("CrmId"));
                    item.Id = rdr.GetString(rdr.GetOrdinal("Id"));
                    item.JinE = rdr.GetDecimal(rdr.GetOrdinal("JinE"));
                    item.KaiPiaoRen = rdr["KaiPiaoRen"].ToString();
                    item.KaiPiaoRenId = rdr.GetInt32(rdr.GetOrdinal("KaiPiaoRenId"));
                    item.PiaoHao = rdr["PiaoHao"].ToString();
                    item.RiQi = rdr.GetDateTime(rdr.GetOrdinal("RiQi"));

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取发票已登记列表合计信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="crmId">客户单位编号</param>
        /// <param name="searchInfo">查询实体</param>
        /// <param name="kaiPiaoJinE">开票金额合计</param>
        public void GetFaPiaosHeJi(int companyId, int crmId, EyouSoft.Model.FinanceStructure.MFaPiaoSearchInfo searchInfo, out decimal kaiPiaoJinE)
        {
            kaiPiaoJinE = 0;
            StringBuilder cmdText = new StringBuilder();

            #region SQL
            cmdText.Append(" SELECT SUM(JinE) AS KaiPiaoJinE FROM tbl_FinFaPiao WHERE IsDelete='0'  ");
            cmdText.AppendFormat(" AND CompanyId={0} ", companyId);
            cmdText.AppendFormat(" AND CrmId={0} ", crmId);

            if (searchInfo != null)
            {
                if (searchInfo.KPETime.HasValue)
                {
                    cmdText.AppendFormat(" AND RiQi<'{0}' ", searchInfo.KPETime.Value.AddDays(1));
                }
                if (!string.IsNullOrEmpty(searchInfo.KPRen))
                {
                    cmdText.AppendFormat(" AND KaiPiaoRen LIKE '%{0}%' ", searchInfo.KPRen);
                }
                if (searchInfo.KPSTime.HasValue)
                {
                    cmdText.AppendFormat(" AND RiQi>'{0}' ", searchInfo.KPSTime.Value.AddDays(-1));
                }
            }
            #endregion

            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) kaiPiaoJinE = rdr.GetDecimal(0);
                }
            }
        }

        #endregion
    }
}
