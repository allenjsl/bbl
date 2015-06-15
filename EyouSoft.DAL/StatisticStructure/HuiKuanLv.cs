//回款率分析数据访问类 汪奇志 2012-02-27
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using EyouSoft.Model.TourStructure;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit;
using EyouSoft.Toolkit.DAL;
using System.Xml.Linq;

namespace EyouSoft.DAL.StatisticStructure
{
    /// <summary>
    /// 回款率分析数据访问类
    /// </summary>
    /// 汪奇志 2012-02-27
    public class HuiKuanLv : EyouSoft.Toolkit.DAL.DALBase,EyouSoft.IDAL.StatisticStructure.IHuiKuanLv
    {
        #region constructor
        /// <summary>
        /// database
        /// </summary>
        private Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public HuiKuanLv()
        {
            this._db = SystemStore;
        }
        #endregion

        #region static constants
        //static constants

        #endregion

        #region public members
        /// <summary>
        /// 获取回款率分析信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="tongJiShiJian">统计时间</param>
        /// <param name="chaXun">查询信息</param>
        /// <returns></returns>
        public EyouSoft.Model.StatisticStructure.HuiKuanLvInfo GetHuiKuanLv(int companyId, DateTime tongJiShiJian, EyouSoft.Model.StatisticStructure.HuiKuanLvChaXunInfo chaXun)
        {
            StringBuilder cmdText = new StringBuilder();
            EyouSoft.Model.StatisticStructure.HuiKuanLvInfo info = null;

            #region SQL
            cmdText.Append("SELECT SUM(YingShouKuan) AS YingShouKuan,SUM(YiShouKuan) AS YiShouKuan,SUM(YiTuiKuan) AS YiTuiKuan FROM(");
            cmdText.Append(" SELECT A.FinanceSum AS YingShouKuan ");
            cmdText.AppendFormat(" ,(SELECT ISNULL(SUM(RefundMoney),0) FROM tbl_ReceiveRefund AS B WHERE B.ItemId=A.Id AND B.ItemType=1 AND B.IsReceive=1 {0} AND B.IssueTime<='{1}') AS YiShouKuan ", chaXun.SFBHWeiShenHe ? "" : " AND B.IsCheck=1 ", tongJiShiJian);
            cmdText.AppendFormat(" ,(SELECT ISNULL(SUM(RefundMoney),0) FROM tbl_ReceiveRefund AS B WHERE B.ItemId=A.Id AND B.ItemType=1 AND B.IsReceive=0 AND B.IsCheck=1 AND B.IssueTime<='{0}' ) AS YiTuiKuan ", tongJiShiJian);
            cmdText.AppendFormat(" FROM tbl_TourOrder AS A WHERE IsDelete='0' AND SellCompanyId={0} ", companyId);

            if (!string.IsNullOrEmpty(chaXun.KeHuDanWei))
            {
                cmdText.AppendFormat(" AND BuyCompanyName LIKE '%{0}%' ", chaXun.KeHuDanWei);
            }
            if (chaXun.OperatorIds != null && chaXun.OperatorIds.Length > 0)
            {
                cmdText.AppendFormat(" AND PerTimeSellerId IN({0}) ", Utils.GetSqlIdStrByList(chaXun.OperatorIds));
            }
            if (chaXun.OperatorDepartIds != null && chaXun.OperatorDepartIds.Length > 0)
            {
                cmdText.AppendFormat(" AND PerTimeSellerId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByList(chaXun.OperatorDepartIds));
            }
            if (chaXun.LSDate.HasValue || chaXun.LEDate.HasValue)
            {
                cmdText.Append(" AND EXISTS(SELECT 1 FROM tbl_Tour AS C WHERE C.TourId=A.TourId ");
                if (chaXun.LSDate.HasValue)
                {
                    cmdText.AppendFormat(" AND C.LeaveDate>='{0}' ", chaXun.LSDate.Value);
                }
                if (chaXun.LEDate.HasValue)
                {
                    cmdText.AppendFormat(" AND C.LeaveDate<='{0}' ", chaXun.LEDate.Value);
                }
                cmdText.Append(" ) ");
            }

            if (chaXun.TongJiDingDanFangShi.HasValue)
            {
                switch (chaXun.TongJiDingDanFangShi.Value)
                {
                    case EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单:
                        cmdText.AppendFormat(" AND OrderState = {0} ", (int)Model.EnumType.TourStructure.OrderState.已成交);
                        break;
                    case EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计有效订单:
                        cmdText.AppendFormat(" AND OrderState IN({0},{1},{2}) ", (int)Model.EnumType.TourStructure.OrderState.未处理, (int)Model.EnumType.TourStructure.OrderState.已成交, (int)Model.EnumType.TourStructure.OrderState.已留位);
                        break;
                }
            }

            cmdText.Append(" ) D ");
            #endregion

            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());


            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    decimal yiTuiKuan = 0;
                    info = new EyouSoft.Model.StatisticStructure.HuiKuanLvInfo();
                    info.TongJiShiJian = tongJiShiJian;
                    info.YingShouKuan = rdr.IsDBNull(rdr.GetOrdinal("YingShouKuan")) ? 0 : rdr.GetDecimal(rdr.GetOrdinal("YingShouKuan"));
                    info.YiShouKuan = rdr.IsDBNull(rdr.GetOrdinal("YiShouKuan")) ? 0 : rdr.GetDecimal(rdr.GetOrdinal("YiShouKuan"));
                    yiTuiKuan = rdr.IsDBNull(rdr.GetOrdinal("YiTuiKuan")) ? 0 : rdr.GetDecimal(rdr.GetOrdinal("YiTuiKuan"));
                    info.YiShouKuan = info.YiShouKuan - yiTuiKuan;

                }
            }

            return info;
        }
        #endregion
    }
}
