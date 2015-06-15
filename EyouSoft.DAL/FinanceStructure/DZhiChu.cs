//汪奇志 2012-08-26
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
    /// 团款支出相关数据访问类
    /// </summary>
    public class DZhiChu : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.FinanceStructure.IZhiChu
    {
        #region constructor
        /// <summary>
        /// database
        /// </summary>
        private Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public DZhiChu()
        {
            _db = SystemStore;
        }
        #endregion

        #region static constants
        //static constants

        #endregion

        #region private members
        /// <summary>
        ///  获取财务管理-团款支出-按计调项显示支出列表查询条件SQL
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        string SQLWhere_GetJiDiaoZhiChuLB(EyouSoft.Model.FinanceStructure.MLBJiDiaoZhiChuSearchInfo searchInfo)
        {
            if (searchInfo == null) return string.Empty;
            StringBuilder s = new StringBuilder();

            if (searchInfo.CTETime.HasValue)
            {
                s.AppendFormat(" AND LeaveDate<'{0}' ", searchInfo.CTETime.Value.AddDays(1));
            }
            if (searchInfo.CTSTime.HasValue)
            {
                s.AppendFormat(" AND LeaveDate>'{0}' ", searchInfo.CTSTime.Value.AddDays(-1));
            }
            if (!string.IsNullOrEmpty(searchInfo.GYSName))
            {
                s.AppendFormat(" AND GYSName LIKE '%{0}%' ", searchInfo.GYSName);
            }
            if (!string.IsNullOrEmpty(searchInfo.TourCode))
            {
                s.AppendFormat(" AND TourCode LIKE '%{0}%' ", searchInfo.TourCode);
            }
            if (searchInfo.TourType.HasValue)
            {
                s.AppendFormat(" AND TourType={0} ", (int)searchInfo.TourType.Value);
            }
            if (searchInfo.ZhiChuLeiBie.HasValue)
            {
                s.AppendFormat(" AND ZhiChuLeiBie={0} ", (int)searchInfo.ZhiChuLeiBie.Value);
            }

            return s.ToString();
        }
        #endregion

        #region EyouSoft.IDAL.FinanceStructure.IZhiChu 成员
        /// <summary>
        /// 获取财务管理-团款支出-按计调项显示支出列表
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.MLBJiDiaoZhiChuInfo> GetJiDiaoZhiChuLB(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.FinanceStructure.MLBJiDiaoZhiChuSearchInfo searchInfo)
        {
            IList<EyouSoft.Model.FinanceStructure.MLBJiDiaoZhiChuInfo> items = new List<EyouSoft.Model.FinanceStructure.MLBJiDiaoZhiChuInfo>();

            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "view_FinJiDiaoZhiChu";
            string primaryKey = "AnPaiId";
            string orderByString = "AnPaiTime DESC";
            string fields = "*";

            #region SQL
            cmdQuery.AppendFormat(" CompanyId={0} ", companyId);
            cmdQuery.Append(SQLWhere_GetJiDiaoZhiChuLB(searchInfo));
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(_db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    var item = new EyouSoft.Model.FinanceStructure.MLBJiDiaoZhiChuInfo();

                    item.AnPaiId = rdr.GetString(rdr.GetOrdinal("AnPaiId"));
                    item.CTTime = rdr.GetDateTime(rdr.GetOrdinal("LeaveDate"));
                    item.GYSName = rdr["GYSName"].ToString();
                    item.RouteName = rdr["RouteName"].ToString();
                    item.TourCode = rdr["TourCode"].ToString();
                    item.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)rdr.GetByte(rdr.GetOrdinal("TourType"));
                    item.YiDengJiJinE = rdr.GetDecimal(rdr.GetOrdinal("YiDengJiJinE"));
                    item.YiZhiFuJinE = rdr.GetDecimal(rdr.GetOrdinal("YiZhiFuJinE"));
                    item.ZhiChuJinE = rdr.GetDecimal(rdr.GetOrdinal("ZhiChuJinE"));
                    item.ZhiChuLeiBie = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)rdr.GetInt32(rdr.GetOrdinal("ZhiChuLeiBie"));
                    item.TourId = rdr.GetString(rdr.GetOrdinal("TourId"));
                    item.GYSId = rdr.GetInt32(rdr.GetOrdinal("GYSId"));

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取财务管理-团款支出-按计调项显示支出列表合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="zhiChuJinE">支出金额</param>
        /// <param name="yiDengJiJinE">已登记金额</param>
        /// <param name="yiZhiFuJinE">已支付金额</param>
        public void GetJiDiaoZhiChuLBHeJi(int companyId, EyouSoft.Model.FinanceStructure.MLBJiDiaoZhiChuSearchInfo searchInfo, out decimal zhiChuJinE, out decimal yiDengJiJinE, out decimal yiZhiFuJinE)
        {
            zhiChuJinE = 0; yiDengJiJinE = 0; yiZhiFuJinE = 0;

            StringBuilder cmdText = new StringBuilder();

            #region SQL
            cmdText.Append(" SELECT SUM(ZhiChuJinE) AS ZhiChuJinE, ");
            cmdText.Append(" SUM(YiDengJiJinE) AS YiDengJiJinE, ");
            cmdText.Append(" SUM(YiZhiFuJinE) AS YiZhiFuJinE ");
            cmdText.Append(" FROM view_FinJiDiaoZhiChu ");
            cmdText.AppendFormat(" WHERE CompanyId={0} ", companyId);
            cmdText.Append(SQLWhere_GetJiDiaoZhiChuLB(searchInfo));
            #endregion

            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) zhiChuJinE = rdr.GetDecimal(0);
                    if (!rdr.IsDBNull(1)) yiDengJiJinE = rdr.GetDecimal(1);
                    if (!rdr.IsDBNull(2)) yiZhiFuJinE = rdr.GetDecimal(2);
                }
            }
        }

        /// <summary>
        /// 财务管理-团款支出-按计调类型批量支出登记，返回1成功，其它失败
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="operatorId">操作人编号</param>
        /// <param name="info">登记信息业务实体</param>
        /// <returns></returns>
        public int PiLiangDengJi(int companyId, int operatorId, EyouSoft.Model.FinanceStructure.MZhiChuPiLiangDengJiInfo info)
        {
            //计调安排编号XML:<root><info anpaiid="计调安排编号" /></root>
            if (info == null || info.AnPaiIds == null || info.AnPaiIds.Length < 1) return 0;
            StringBuilder xml = new StringBuilder();
            xml.Append("<root>");
            foreach (var s in info.AnPaiIds)
            {
                xml.AppendFormat("<info anpaiid=\"{0}\" />", s);
            }
            xml.Append("</root>");

            DbCommand cmd = _db.GetStoredProcCommand("proc_FinPiLiangDengJiZhiChu");
            _db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);
            _db.AddInParameter(cmd, "OperatorId", DbType.Int32, operatorId);
            _db.AddInParameter(cmd, "FKTime", DbType.DateTime, info.FKTime);
            _db.AddInParameter(cmd, "FKRenId", DbType.Int32, info.FKRenId);
            _db.AddInParameter(cmd, "FKRenName", DbType.String, info.FKRenName);
            _db.AddInParameter(cmd, "FKFangShi", DbType.Byte, info.FKFangShi);
            _db.AddInParameter(cmd, "BeiZhu", DbType.String, info.BeiZhu);
            _db.AddInParameter(cmd, "AnPaiIdsXML", DbType.String, xml.ToString());
            _db.AddOutParameter(cmd, "RetCode", DbType.Int32, 4);

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

            return Convert.ToInt32(this._db.GetParameterValue(cmd, "RetCode"));
        }

        #endregion
    }
}
