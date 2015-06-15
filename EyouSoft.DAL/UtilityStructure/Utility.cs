using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;

namespace EyouSoft.DAL.UtilityStructure
{
    /// <summary>
    /// 统一维护的方法
    /// Create:luofx  Date:2011-01-23
    /// </summary>
    public class Utility : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.UtilityStructure.IUtility
    {
        private readonly Database _db = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        public Utility()
        {
            _db = this.SystemStore;
        }
        /// <summary>
        /// 修改线路库线路团队收客数，上团数
        /// </summary>
        /// <param name="RouteId">线路编号</param>
        /// <param name="type">团队上团数和收客数修改类型</param>
        /// <returns></returns>
        public bool UpdateRouteSomething(int[] RouteId, EyouSoft.Model.EnumType.TourStructure.UpdateTourType type)
        {
            bool IsTrue = false;
            EyouSoft.DAL.TourStructure.TourOrder dal = new EyouSoft.DAL.TourStructure.TourOrder();
            string RouteIds = dal.ConvertIntArrayTostring(RouteId);
            DbCommand dc = this._db.GetStoredProcCommand("proc_UpdateTourCountOrVisitorCount");
            this._db.AddInParameter(dc, "RouteId", DbType.AnsiString, RouteIds);
            this._db.AddInParameter(dc, "UpdateTourType", DbType.Int32, (int)type);
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, _db);
            object Result = this._db.GetParameterValue(dc, "Result");
            dal = null;
            if (!Result.Equals(null))
            {
                IsTrue = int.Parse(Result.ToString()) > 0 ? true : false;
            }
            return IsTrue;
        }
        /// <summary>
        /// 1:更新团队总收入
        /// 2：团款收入时才插入或更新统计分析[所有收入明细表([tbl_StatAllIncome])]数据
        /// </summary>
        /// <returns></returns>
        public bool UpdateTotalIncome(string OrderId)
        {
            bool IsTrue = false;
            DbCommand dc = this._db.GetStoredProcCommand("proc_UpdateTotalIncome");
            this._db.AddInParameter(dc, "OrderId", DbType.AnsiStringFixedLength, OrderId);
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, _db);
            object Result = this._db.GetParameterValue(dc, "Result");
            if (!Result.Equals(null))
            {
                IsTrue = int.Parse(Result.ToString()) > 0 ? true : false;
            }
            return IsTrue;
        }

        /// <summary>
        /// 1:增加（审核）收款之后更新订单已收未审核金额和已收已审核金额
        /// 2:更新统计分析[所有收入明细表([tbl_StatAllIncome])]已收未审核金额和已收已审核金额
        ///	3:团队的收入是否已结清
        /// </summary>
        /// <param name="IsReceive">是否收款（true=收款，false=退款）</param>
        /// <param name="OrderIds">订单编号</param>
        /// <returns></returns>
        public bool UpdateCheckMoney(bool IsReceive, params string[] OrderIds)
        {
            bool IsTrue = false;
            foreach (string id in OrderIds)
            {
                DbCommand dc = this._db.GetStoredProcCommand("proc_UpdateCheckMoney");
                this._db.AddInParameter(dc, "OrderId", DbType.AnsiString, id);
                this._db.AddInParameter(dc, "IsReceive", DbType.Int32, IsReceive ? 1 : 0);
                this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
                DbHelper.RunProcedure(dc, _db);
                object Result = this._db.GetParameterValue(dc, "Result");
                if (!Result.Equals(null))
                {
                    IsTrue = int.Parse(Result.ToString()) > 0 ? true : false;
                }
            }
            return IsTrue;
        }
        ///// <summary>
        ///// 维护团队,订单数据
        ///// </summary>
        ///// <param name="ItemId">项目编号(订单编号，团队编号)</param>
        ///// <param name="type">类型</param>
        ///// <returns></returns>
        //public bool UpdateOrderAndTourSomething(string ItemId, EyouSoft.Model.EnumType.TourStructure.ItemType type)
        //{
        //    DbCommand dc = this._db.GetStoredProcCommand("proc_UpdateOrderAndTour");
        //    this._db.AddInParameter(dc, "ItemId", DbType.AnsiStringFixedLength, ItemId);
        //    this._db.AddInParameter(dc, "ItemType", DbType.Int32, (int)type);
        //    this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
        //    return DbHelper.ExecuteSql(dc, this._db) > 0 ? true : false;
        //}

        /// <summary>
        /// 重新计算团队收入相关字段，所有模块收入相关项、
        /// </summary>
        /// <param name="TourId">团队Id</param>
        /// <param name="list">收入项目编号、类型实体</param>
        /// <returns>返回1成功，其他失败</returns>
        public int CalculationTourIncome(string TourId, IList<Model.StatisticStructure.IncomeItemIdAndType> list)
        {
            if (string.IsNullOrEmpty(TourId) && (list == null || list.Count <= 0))
                return 0;

            DbCommand dc = _db.GetStoredProcCommand("proc_Utility_CalculationTourIncome");
            _db.AddInParameter(dc, "TourId", DbType.AnsiStringFixedLength, TourId);
            _db.AddInParameter(dc, "IncomeItemIdAndTypeXML", DbType.String, GetItemIdAndTypeXML(list));
            _db.AddOutParameter(dc, "ErrorValue", DbType.Int32, 4);

            DbHelper.RunProcedure(dc, _db);

            object obj = _db.GetParameterValue(dc, "ErrorValue");
            if (obj.Equals(null))
                return 0;
            else
                return int.Parse(obj.ToString());
        }

        /// <summary>
        /// 重新计算团队支出相关项、所有支出相关项、
        /// </summary>
        /// <param name="TourId">团队Id，传值string.Empty不重新计算团队的各种支出</param>
        /// <param name="list">支出项目编号、类型实体集合</param>
        /// <returns>返回1成功，其他失败</returns>
        public int CalculationTourOut(string TourId, IList<Model.StatisticStructure.ItemIdAndType> list)
        {
            if (string.IsNullOrEmpty(TourId) && (list == null || list.Count <= 0))
                return 0;

            DbCommand dc = _db.GetStoredProcCommand("proc_Utility_CalculationTourOut");
            _db.AddInParameter(dc, "TourId", DbType.AnsiStringFixedLength, TourId);
            _db.AddInParameter(dc, "ItemIdAndTypeXML", DbType.String, GetItemIdAndTypeXML(list));
            _db.AddOutParameter(dc, "ErrorValue", DbType.Int32, 4);

            DbHelper.RunProcedure(dc, _db);

            object obj = _db.GetParameterValue(dc, "ErrorValue");
            if (obj.Equals(null))
                return 0;
            else
                return int.Parse(obj.ToString());
        }

        /// <summary>
        /// 重新计算所有模块的已确认支付金额
        /// </summary>
        /// <param name="FinancialPayIds">支出登记明细Id集合</param>
        /// <returns>返回1成功，其他失败</returns>
        public int CalculationCheckedOut(params string[] FinancialPayIds)
        {
            if (FinancialPayIds == null || FinancialPayIds.Length <= 0)
                return 0;

            string strIds = string.Empty;
            foreach (string str in FinancialPayIds)
            {
                strIds += "'" + str + "',";
            }
            strIds = strIds.Trim(',');

            DbCommand dc = _db.GetStoredProcCommand("proc_Utility_CalculationCheckedOut");
            _db.AddInParameter(dc, "FinancialPayId", DbType.String, strIds);
            _db.AddOutParameter(dc, "ErrorValue", DbType.Int32, 4);

            DbHelper.RunProcedure(dc, _db);

            object obj = _db.GetParameterValue(dc, "ErrorValue");
            if (obj.Equals(null))
                return 0;
            else
                return int.Parse(obj.ToString());
        }

        /// <summary>
        /// 重新计算团队的利润分配
        /// </summary>
        /// <param name="TourId">团队Id</param>
        /// <returns>返回1成功，其他失败</returns>
        public int CalculationTourProfitShare(string TourId)
        {
            if (string.IsNullOrEmpty(TourId))
                return 0;

            string strSql = string.Format(" update tbl_Tour set DistributionAmount = (select isnull(sum(ShareCost),0) from tbl_TourShare where TourId = '{0}') where TourId = '{0}' ", TourId);
            DbCommand dc = _db.GetSqlStringCommand(strSql);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 重新计算现金流量某一天的收入、支出
        /// </summary>
        /// <param name="CompanyId">要计算的公司Id</param>
        /// <param name="CurrDate">要计算哪一天</param>
        /// <returns>返回1成功，其他失败</returns>
        public int CalculationCashFlow(int CompanyId, DateTime CurrDate)
        {
            if (CompanyId <= 0)
                return 0;

            DbCommand dc = _db.GetStoredProcCommand("proc_Utility_CalculationCashFlow");
            _db.AddInParameter(dc, "CompanyId", DbType.Int32, CompanyId);
            _db.AddInParameter(dc, "CurrDate", DbType.DateTime, CurrDate);
            _db.AddOutParameter(dc, "ErrorValue", DbType.Int32, 4);

            DbHelper.RunProcedure(dc, _db);

            object obj = _db.GetParameterValue(dc, "ErrorValue");
            if (obj.Equals(null))
                return 0;
            else
                return int.Parse(obj.ToString());
        }
        /// <summary>
        /// 维护组团公司与专线公司的交易次数
        /// </summary>
        /// <param name="BuyCompanyId">组团公司编号</param>
        /// <returns></returns>
        public bool UpdateTradeNum(int BuyCompanyId)
        {
            string StrSql = "UPDATE tbl_Customer SET TradeNum=(SELECT COUNT(1) FROM tbl_TourOrder WHERE BuyCompanyID=@BuyCompanyID AND OrderState=5 AND IsDelete=0) WHERE ID=@BuyCompanyID";
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            this._db.AddInParameter(dc, "BuyCompanyID", DbType.Int32, BuyCompanyId);
            return DbHelper.ExecuteSql(dc,this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 计算地接社或机票供应商交易数量
        /// </summary>
        /// <param name="ServerId">供应商编号</param>
        /// <returns></returns>
        public bool ServerTradeCount(int ServerId)
        {
            if (ServerId < 0)
                return false;

            DbCommand cmd = this._db.GetStoredProcCommand("proc_ServerTradeCount");
            this._db.AddInParameter(cmd, "ServerId", DbType.Int32,ServerId);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(cmd, this._db);
            object o = this._db.GetParameterValue(cmd, "Result");
            return int.Parse(o.ToString()) > 0 ? true : false;

        }

        /// <summary>
        /// 维护计划款项结清状态，返回1成功，其它失败
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public int CalculationTourSettleStatus(string tourId)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Utility_CalculationTourSettleStatus");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);
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

        #region 私有函数

        /// <summary>
        /// 生成支出项目编号、类型XML
        /// </summary>
        /// <param name="list">支出项目编号、类型实体集合</param>
        /// <returns>XML</returns>
        private string GetItemIdAndTypeXML(IList<Model.StatisticStructure.ItemIdAndType> list)
        {
            if (list == null || list.Count <= 0)
                return string.Empty;

            StringBuilder strXML = new StringBuilder();
            strXML.Append("<ROOT>");
            foreach (Model.StatisticStructure.ItemIdAndType t in list)
            {
                if (t == null)
                    continue;

                strXML.AppendFormat("<ItemIdAndType ItemId='{0}' ItemType='{1}' ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(t.ItemId), (int)t.ItemType);
                strXML.Append(" />");
            }
            strXML.Append("</ROOT>");

            return strXML.ToString();
        }

        /// <summary>
        /// 生成收入项目编号、类型XML
        /// </summary>
        /// <param name="list">收入项目编号、类型实体集合</param>
        /// <returns>XML</returns>
        private string GetItemIdAndTypeXML(IList<Model.StatisticStructure.IncomeItemIdAndType> list)
        {
            if (list == null || list.Count <= 0)
                return string.Empty;

            StringBuilder strXML = new StringBuilder();
            strXML.Append("<ROOT>");
            foreach (Model.StatisticStructure.IncomeItemIdAndType t in list)
            {
                if (t == null)
                    continue;

                strXML.AppendFormat("<IncomeItemIdAndType ItemId='{0}' ItemType='{1}' ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(t.ItemId), (int)t.ItemType);
                strXML.Append(" />");
            }
            strXML.Append("</ROOT>");

            return strXML.ToString();
        }

        /// <summary>
        /// 数组转化成字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string ArrayToString(string[] str)
        {
            StringBuilder st = new StringBuilder();
            foreach (string s in str)
                st.AppendFormat("{0},", s);

            return st.ToString().TrimEnd(new char[] { ',' });
        }
        #endregion
    }
}
