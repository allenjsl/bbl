using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using EyouSoft.Model.PlanStructure;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using EyouSoft.Toolkit;

namespace EyouSoft.DAL.PlanStruture
{
    /// <summary>
    /// 地接社相关操作方法
    /// autor:李焕超 datetime:2011-1-19
    /// </summary>
    public class TravelAgency : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.PlanStruture.ITravelAgency
    {
        private readonly Database _db = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        public TravelAgency()
        {
            _db = this.SystemStore;
        }
        /// <summary>
        /// 安排地接社
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool RanguageTravel(LocalTravelAgencyInfo model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_PlanTravelAgency_Add");

            this._db.AddInParameter(cmd, "ID", DbType.AnsiStringFixedLength, model.ID);
            this._db.AddInParameter(cmd, "AddAmount", DbType.Decimal, model.AddAmount);
            this._db.AddInParameter(cmd, "Commission", DbType.Decimal, model.Commission);
            this._db.AddInParameter(cmd, "Contacter", DbType.String, model.Contacter);
            this._db.AddInParameter(cmd, "ContactTel", DbType.String, model.ContactTel);
            this._db.AddInParameter(cmd, "DeliverTime", DbType.DateTime, model.DeliverTime);
            this._db.AddInParameter(cmd, "Fee", DbType.Decimal, model.Fee);
            this._db.AddInParameter(cmd, "LocalTravelAgency", DbType.String, model.LocalTravelAgency);
            this._db.AddInParameter(cmd, "Notice", DbType.String, model.Notice);
            this._db.AddInParameter(cmd, "Operator", DbType.String, model.Operator);
            this._db.AddInParameter(cmd, "OperatorID", DbType.Int32, model.OperatorID);
            this._db.AddInParameter(cmd, "PayType", DbType.AnsiStringFixedLength, (int)model.PayType);
            this._db.AddInParameter(cmd, "ReceiveTime", DbType.DateTime, model.ReceiveTime);
            this._db.AddInParameter(cmd, "ReduceAmount", DbType.Decimal, model.ReduceAmount);
            this._db.AddInParameter(cmd, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(cmd, "Settlement", DbType.Decimal, model.Settlement);
            this._db.AddInParameter(cmd, "TotalAmount", DbType.Decimal, model.TotalAmount);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, model.CompanyId);
            this._db.AddInParameter(cmd, "TourId", DbType.String, model.TourId);
            this._db.AddInParameter(cmd, "TravelAgencyID", DbType.Int32, model.TravelAgencyID);
            this._db.AddInParameter(cmd, "PriceInfoListXML", DbType.String, CreatePriceInfoXML(model.TravelAgencyPriceInfoList));
            this._db.AddInParameter(cmd, "PlanInfoListXML", DbType.String, CreateTourPlanInfoXML(model.LocalAgencyTourPlanInfoList));
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);

            DbHelper.RunProcedure(cmd, this._db);
            object ob = this._db.GetParameterValue(cmd, "Result");
            return int.Parse(ob.ToString()) > 0 ? true : false;

        }

        /// <summary>
        /// 返回地接社实体
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public LocalTravelAgencyInfo GetTravelModel(string ID)
        {
            LocalTravelAgencyInfo model = null;
            string sql = "select * from tbl_PlanLocalAgency where id=@TravelId";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "TravelId", DbType.AnsiStringFixedLength, ID);
            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rd.Read())
                {
                    model = new LocalTravelAgencyInfo();
                    model.AddAmount = rd.GetDecimal(rd.GetOrdinal("AddAmount"));
                    model.Commission = rd.GetDecimal(rd.GetOrdinal("Commission"));
                    model.Contacter = rd.IsDBNull(rd.GetOrdinal("Contacter")) ? "" : rd.GetString(rd.GetOrdinal("Contacter"));
                    model.ContactTel = rd.IsDBNull(rd.GetOrdinal("ContactTel")) ? "" : rd.GetString(rd.GetOrdinal("ContactTel"));
                    model.DeliverTime = rd.GetDateTime(rd.GetOrdinal("DeliverTime"));
                    model.Fee = rd.GetDecimal(rd.GetOrdinal("Fee"));
                    model.FRemark = rd.IsDBNull(rd.GetOrdinal("FRemark")) ? "" : rd.GetString(rd.GetOrdinal("FRemark"));
                    model.ID = rd.GetString(rd.GetOrdinal("ID"));
                    model.LocalTravelAgency = rd.IsDBNull(rd.GetOrdinal("LocalTravelAgency")) ? "" : rd.GetString(rd.GetOrdinal("LocalTravelAgency"));
                    model.Notice = rd.IsDBNull(rd.GetOrdinal("Notice")) ? "" : rd.GetString(rd.GetOrdinal("Notice"));
                    model.OperateTime = rd.GetDateTime(rd.GetOrdinal("OperateTime"));
                    model.Operator = rd.IsDBNull(rd.GetOrdinal("Operator")) ? "" : rd.GetString(rd.GetOrdinal("Operator"));
                    model.OperatorID = rd.GetInt32(rd.GetOrdinal("OperatorID"));
                    model.PayType = (EyouSoft.Model.EnumType.TourStructure.RefundType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.RefundType), rd.GetByte(rd.GetOrdinal("PayType")).ToString());
                    model.ReceiveTime = rd.GetDateTime(rd.GetOrdinal("ReceiveTime"));
                    model.ReduceAmount = rd.GetDecimal(rd.GetOrdinal("ReduceAmount"));
                    model.Remark = rd.IsDBNull(rd.GetOrdinal("Remark")) ? "" : rd.GetString(rd.GetOrdinal("Remark"));
                    model.Settlement = rd.GetDecimal(rd.GetOrdinal("Settlement"));
                    model.TotalAmount = rd.GetDecimal(rd.GetOrdinal("TotalAmount"));
                    model.TourId = rd.GetString(rd.GetOrdinal("TourId"));
                    model.TravelAgencyID = rd.GetInt32(rd.GetOrdinal("TravelAgencyID"));
                    //价格组成
                    model.TravelAgencyPriceInfoList = GetTravelPriceList(model.ID);
                    //接待行程
                    model.LocalAgencyTourPlanInfoList = GetTravelPlanList(model.ID);

                }

            }
            return model;
        }

        /// <summary>
        /// 修改地接
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool UpdateTravelModel(LocalTravelAgencyInfo model)
        {
            if (string.IsNullOrEmpty(model.ID))
                return false;
            DbCommand cmd = this._db.GetStoredProcCommand("proc_PlanTravelAgency_Update");
            this._db.AddInParameter(cmd, "ID", DbType.AnsiStringFixedLength, model.ID);
            this._db.AddInParameter(cmd, "AddAmount", DbType.Decimal, model.AddAmount);
            this._db.AddInParameter(cmd, "Commission", DbType.Decimal, model.Commission);
            this._db.AddInParameter(cmd, "Contacter", DbType.String, model.Contacter);
            this._db.AddInParameter(cmd, "ContactTel", DbType.String, model.ContactTel);
            this._db.AddInParameter(cmd, "DeliverTime", DbType.DateTime, model.DeliverTime);
            this._db.AddInParameter(cmd, "Fee", DbType.Decimal, model.Fee);
            this._db.AddInParameter(cmd, "LocalTravelAgency", DbType.String, model.LocalTravelAgency);
            this._db.AddInParameter(cmd, "Notice", DbType.String, model.Notice);
            this._db.AddInParameter(cmd, "OperateTime", DbType.DateTime, model.OperateTime);
            this._db.AddInParameter(cmd, "Operator", DbType.String, model.Operator);
            this._db.AddInParameter(cmd, "OperatorID", DbType.Int32, model.OperatorID);
            this._db.AddInParameter(cmd, "PayType", DbType.AnsiStringFixedLength, (int)model.PayType);
            this._db.AddInParameter(cmd, "ReceiveTime", DbType.DateTime, model.ReceiveTime);
            this._db.AddInParameter(cmd, "ReduceAmount", DbType.Decimal, model.ReduceAmount);
            this._db.AddInParameter(cmd, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(cmd, "Settlement", DbType.Decimal, model.Settlement);
            this._db.AddInParameter(cmd, "TotalAmount", DbType.Decimal, model.TotalAmount);
            this._db.AddInParameter(cmd, "TourId", DbType.String, model.TourId);
            this._db.AddInParameter(cmd, "TravelAgencyID", DbType.Int32, model.TravelAgencyID);
            this._db.AddInParameter(cmd, "PriceInfoListXML", DbType.String, CreatePriceInfoXML(model.TravelAgencyPriceInfoList));
            this._db.AddInParameter(cmd, "PlanInfoListXML", DbType.String, CreateTourPlanInfoXML(model.LocalAgencyTourPlanInfoList));
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(cmd, this._db);
            object ob = this._db.GetParameterValue(cmd, "Result");
            return int.Parse(ob.ToString()) > 0 ? true : false;
        }

        /// <summary>
        /// 删除地接
        /// </summary>
        /// <param name="TravelId">安排地接编号</param>
        /// <returns></returns>
        public bool DeletTravelModel(string TravelId)
        {
            if (string.IsNullOrEmpty(TravelId))
                return false;
            DbCommand cmd = this._db.GetStoredProcCommand("proc_PlanTravelAgency_Del");
            this._db.AddInParameter(cmd, "TravelId", DbType.AnsiStringFixedLength, TravelId);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(cmd, this._db);
            object ob = this._db.GetParameterValue(cmd, "Result");
            return int.Parse(ob.ToString()) > 0 ? true : false;
        }

        /// <summary>
        /// 获取某团地接列表
        /// </summary>
        /// <param name="TravelId">团队编号</param>
        /// <returns></returns>
        public IList<LocalTravelAgencyInfo> GetList(string TourId)
        {
            if (string.IsNullOrEmpty(TourId))
                return null;

            return GetTravelListWhere(string.Format(" and TourId='{0}' ", TourId));
        }

        /// <summary>
        /// 获取地接列表
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        public IList<LocalTravelAgencyInfo> GetTravelList(int CompanyId)
        {
            return GetTravelListWhere(string.Format(" and companyid={0}", CompanyId.ToString()));
        }

        /// <summary>
        /// 获取某个业务员支出统计列表
        /// </summary>
        /// <param name="SearchModel">查询实体</param>
        /// <param name="haveUserIds">用户Ids</param>
        /// <returns></returns>
        public IList<PersonalStatics> GetStaticsList(EyouSoft.Model.StatisticStructure.QueryPersonnelStatistic SearchModel
            , string haveUserIds)
        {
            IList<PersonalStatics> saticsList = new List<PersonalStatics>();
            PersonalStatics model = null;
            StringBuilder sql = new StringBuilder();
            #region sql
            sql.AppendFormat(" select * from View_TravelAndTicketArrear  where companyid={0}", SearchModel.CompanyId);
            if (!string.IsNullOrEmpty(SearchModel.LeaveDateStart.ToString()))
            {
                sql.AppendFormat(" and datediff(dd,LeaveDate,'{0}')<0 ", SearchModel.LeaveDateStart);
            }
            if (!string.IsNullOrEmpty(SearchModel.LeaveDateEnd.ToString()))
            {
                sql.AppendFormat(" and datediff(dd,LeaveDate,'{0}')>=0 ", SearchModel.LeaveDateEnd);
            }
            if (!string.IsNullOrEmpty(SearchModel.CheckDateStart.ToString()))
            {
                sql.AppendFormat(" and  datediff(dd,RegTime,'{0}')<0 ", SearchModel.CheckDateStart.ToString());
            }
            if (!string.IsNullOrEmpty(SearchModel.CheckDateEnd.ToString()))
            {
                sql.AppendFormat(" and  datediff(dd,RegTime,'{0}')>=0 ", SearchModel.CheckDateEnd.ToString());
            }
            if (SearchModel.LogisticsIds != null && SearchModel.LogisticsIds.Count() > 0)
            {
                string sid = "";
                foreach (int i in SearchModel.LogisticsIds)
                    sid += i + ",";
                sql.AppendFormat(" and OperateID in({0})", sid.Substring(0, (sid.Length - 1)));
            }
            if (SearchModel.SaleIds != null && SearchModel.SaleIds.Count() > 0)
            {
                string salerid = "";
                foreach (int i in SearchModel.SaleIds)
                    salerid += i + ",";
                sql.AppendFormat("  AND TourId IN(SELECT [TourId] FROM [tbl_TourOrder] WHERE [SalerId] IN({0}) AND [IsDelete]='0')", salerid.Substring(0, (salerid.Length - 1)));
            }
            if (!string.IsNullOrEmpty(haveUserIds))
                sql.AppendFormat(" and TourOperatorId in ({0}) ", haveUserIds);

            #endregion
            DbCommand cmd = this._db.GetSqlStringCommand(sql.ToString());
            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rd.Read())
                {
                    model = new PersonalStatics();
                    model.LeaveDate = rd.IsDBNull(rd.GetOrdinal("leavedate")) ? DateTime.Parse("2000-1-1") : rd.GetDateTime(rd.GetOrdinal("leavedate"));
                    model.PeopleCount = rd.IsDBNull(rd.GetOrdinal("PeopleCount")) ? 0 : rd.GetInt32(rd.GetOrdinal("PeopleCount"));
                    model.RouteName = rd.IsDBNull(rd.GetOrdinal("RouteName")) ? "" : rd.GetString(rd.GetOrdinal("RouteName"));
                    model.SellCompanyName = rd.IsDBNull(rd.GetOrdinal("Supplier")) ? "" : rd.GetString(rd.GetOrdinal("Supplier"));
                    model.Total = rd.IsDBNull(rd.GetOrdinal("TotalAmount")) ? 0 : rd.GetDecimal(rd.GetOrdinal("TotalAmount"));
                    model.TourNo = rd.IsDBNull(rd.GetOrdinal("TourCode")) ? "" : rd.GetString(rd.GetOrdinal("TourCode"));

                    saticsList.Add(model);
                }
            }
            return saticsList;

        }

        /// <summary>
        /// 根据团号列出机票地接社支付信息 ---供应商类型地接(1) 或机票(2)
        /// </summary>
        /// <param name="TourId">团队编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<PaymentList> GetSettleList(string TourId, MExpendSearchInfo searchInfo)
        {
            IList<PaymentList> payMentList = new List<PaymentList>();
            PaymentList model = null;
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT [ID],[TourId] ,[AddAmount],[ReduceAmount],[TotalAmount],[FRemark],[Settlement] as PayAmount,PayAmount as PayedAmount ,1 as SupplierType,LocalTravelAgency as SuplierName,TravelAgencyID AS SupplierId ");
            sql.Append(" FROM [tbl_PlanLocalAgency] where TourId=@TourId {0}");
            sql.Append(" union");
            sql.Append(" SELECT [ID],[TourId] ,[AddAmount],[ReduceAmount],[TotalAmount],[FRemark],[Total] as PayAmount,PayAmount as PayedAmount,2 as SupplierType,TicketOffice as SuplierName,TicketOfficeId AS SupplierId ");
            sql.Append(" FROM [tbl_PlanTicketOut]  where state=3 and TourId=@TourId {1}");

            string cmdText = sql.ToString();
            string sdijie = string.Empty;
            string sjipiao = string.Empty;

            if (searchInfo != null)
            {                
                if (!string.IsNullOrEmpty(searchInfo.SupplierName))
                {
                    sdijie += string.Format(" AND LocalTravelAgency LIKE '%{0}%' ", searchInfo.SupplierName);
                    sjipiao += string.Format(" AND TicketOffice LIKE '%{0}%' ", searchInfo.SupplierName);
                }
                if (searchInfo.SupplierType.HasValue)
                {
                    if (searchInfo.SupplierType.Value == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接)
                    {
                        sjipiao += " AND 1=0 ";
                    }

                    if (searchInfo.SupplierType.Value == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务)
                    {
                        sdijie += " AND 1=0 ";
                    }
                }
            }

            cmdText = string.Format(cmdText, sdijie, sjipiao);

            DbCommand cmd = this._db.GetSqlStringCommand(cmdText);
            this._db.AddInParameter(cmd, "TourId", DbType.StringFixedLength, TourId);
            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rd.Read())
                {
                    model = new PaymentList();
                    model.AddAmount = rd.IsDBNull(rd.GetOrdinal("AddAmount")) ? 0 : rd.GetDecimal(rd.GetOrdinal("AddAmount"));
                    model.FRemark = rd.IsDBNull(rd.GetOrdinal("FRemark")) ? "" : rd.GetString(rd.GetOrdinal("FRemark"));
                    model.Id = rd.IsDBNull(rd.GetOrdinal("Id")) ? "0" : rd.GetString(rd.GetOrdinal("Id"));
                    model.PayAmount = rd.IsDBNull(rd.GetOrdinal("PayAmount")) ? 0 : rd.GetDecimal(rd.GetOrdinal("PayAmount"));
                    model.ReduceAmount = rd.IsDBNull(rd.GetOrdinal("ReduceAmount")) ? 0 : rd.GetDecimal(rd.GetOrdinal("ReduceAmount"));
                    model.SuplierName = rd.IsDBNull(rd.GetOrdinal("SuplierName")) ? "" : rd.GetString(rd.GetOrdinal("SuplierName"));
                    model.SupplierType = (EyouSoft.Model.EnumType.CompanyStructure.SupplierType)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.SupplierType), rd.GetInt32(rd.GetOrdinal("SupplierType")).ToString());
                    model.TotalAmount = rd.IsDBNull(rd.GetOrdinal("TotalAmount")) ? 0 : rd.GetDecimal(rd.GetOrdinal("TotalAmount"));
                    model.PayedAmount = rd.IsDBNull(rd.GetOrdinal("PayedAmount")) ? 0 : rd.GetDecimal(rd.GetOrdinal("PayedAmount"));
                    model.SupplierId = rd.IsDBNull(rd.GetOrdinal("SupplierId")) ? 0 : rd.GetInt32(rd.GetOrdinal("SupplierId"));
                    payMentList.Add(model);
                }
            }
            return payMentList;
        }

        /// <summary>
        /// 修改机票地接社支出金额
        /// </summary>
        /// <param name="TravelId">团队编号</param>
        /// <returns></returns>
        public bool UpdateSettle(PaymentList Model)
        {
            if (string.IsNullOrEmpty(Model.Id) || Model.Id == "0")
                return false;
            if (Model.SupplierType != EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接 && Model.SupplierType != EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务)
                return false;

            string sql = "";
            if (Model.SupplierType == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接)
                sql = " update tbl_PlanLocalAgency set AddAmount=@AddAmount ,ReduceAmount=@ReduceAmount,FRemark=@FRemark,TotalAmount=@TotalAmount where id=@id";

            if (Model.SupplierType == EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务)
                sql = " update tbl_PlanTicketOut set AddAmount=@AddAmount ,ReduceAmount=@ReduceAmount,FRemark=@FRemark,TotalAmount=@TotalAmount where id=@id";

            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "AddAmount", DbType.Decimal, Model.AddAmount);
            this._db.AddInParameter(cmd, "ReduceAmount", DbType.Decimal, Model.ReduceAmount);
            this._db.AddInParameter(cmd, "FRemark", DbType.String, Model.FRemark);
            this._db.AddInParameter(cmd, "TotalAmount", DbType.Decimal, Model.TotalAmount);
            this._db.AddInParameter(cmd, "id", DbType.StringFixedLength, Model.Id);
            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;

        }

        /// <summary>
        /// 获取付款提醒查看明细信息集合
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="ServerId"></param>
        /// <param name="Type">1代表地接，2代表票务</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<ArrearInfo> GetFuKuanTiXingMingXi(int CompanyId, int ServerId, int ServerType, int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.PersonalCenterStructure.FuKuanTiXingChaXun searchInfo)
        {
            string tableName = "View_TravelAndTicketArrear";
            string field = " * ";
            string orderBy = " leaveDate ";
            //string sqlWhere = string.Format(" arrear <>0 and companyid={0} and serverid={1} and ServerType={2}", CompanyId, ServerId, ServerType);
            StringBuilder sqlWhere = new StringBuilder();
            sqlWhere.AppendFormat(" arrear <>0 and companyid={0} and serverid={1}", CompanyId, ServerId);

            if (searchInfo != null)
            {
                if (searchInfo.LEDate.HasValue
                    || searchInfo.LSDate.HasValue
                    || (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    || (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0))
                {
                    if (searchInfo.LSDate.HasValue)
                    {
                        sqlWhere.AppendFormat(" AND LeaveDate>='{0}' ", searchInfo.LSDate.Value);
                    }
                    if (searchInfo.LEDate.HasValue)
                    {
                        sqlWhere.AppendFormat(" AND LeaveDate<='{0}' ", searchInfo.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0)
                    {
                        sqlWhere.AppendFormat(" AND TourOperatorId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorIds));
                    }
                    if (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    {
                        sqlWhere.AppendFormat(" AND TourOperatorId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorDepartIds));
                    }
                }
            }

            IList<ArrearInfo> ArrearList = new List<ArrearInfo>();
            ArrearInfo model = null;
            using (IDataReader rd = DbHelper.ExecuteReader(this._db, PageSize, PageIndex, ref RecordCount, tableName, " tourid ", field, sqlWhere.ToString(), orderBy))
            {
                while (rd.Read())
                {
                    model = new ArrearInfo();
                    model.Arrear = rd.GetDecimal(rd.GetOrdinal("Arrear"));
                    model.LeaveDate = rd.IsDBNull(rd.GetOrdinal("LeaveDate")) ? DateTime.Parse("2000-1-1") : rd.GetDateTime(rd.GetOrdinal("LeaveDate"));
                    model.RouteName = rd.IsDBNull(rd.GetOrdinal("RouteName")) ? "" : rd.GetString(rd.GetOrdinal("RouteName"));
                    model.TotalAmount = rd.GetDecimal(rd.GetOrdinal("TotalAmount")); ;
                    model.TourCode = rd.IsDBNull(rd.GetOrdinal("TourCode")) ? "" : rd.GetString(rd.GetOrdinal("TourCode"));
                    model.TourId = rd.IsDBNull(rd.GetOrdinal("TourId")) ? "" : rd.GetString(rd.GetOrdinal("TourId"));
                    ArrearList.Add(model);
                }
            }
            return ArrearList;
        }

        /// <summary>
        /// 获取付款提醒查看明细信息汇总
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="supplierId">供应商编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="weiFuHeJi">未付金额合计</param>
        /// <returns></returns>
        public void GetFuKuanTiXingMingXi(int companyId, int supplierId, EyouSoft.Model.PersonalCenterStructure.FuKuanTiXingChaXun searchInfo, out decimal weiFuHeJi)
        {
            weiFuHeJi = 0;

            #region SQL
            StringBuilder cmdText = new StringBuilder();
            cmdText.Append("SELECT SUM(Arrear) AS WeiFuHeJi FROM View_TravelAndTicketArrear WHERE ");
            cmdText.AppendFormat(" arrear <>0 and companyid={0} and serverid={1}", companyId, supplierId);

            if (searchInfo != null)
            {
                if (searchInfo.LEDate.HasValue
                    || searchInfo.LSDate.HasValue
                    || (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    || (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0))
                {
                    if (searchInfo.LSDate.HasValue)
                    {
                        cmdText.AppendFormat(" AND LeaveDate>='{0}' ", searchInfo.LSDate.Value);
                    }
                    if (searchInfo.LEDate.HasValue)
                    {
                        cmdText.AppendFormat(" AND LeaveDate<='{0}' ", searchInfo.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0)
                    {
                        cmdText.AppendFormat(" AND TourOperatorId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorIds));
                    }
                    if (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    {
                        cmdText.AppendFormat(" AND TourOperatorId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorDepartIds));
                    }
                }
            }
            #endregion

            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) weiFuHeJi = rdr.GetDecimal(0);
                }
            }
        }

        /// <summary>
        /// 统计中心-获取欠款列表（总支出；已付；未付）(统计分析-支出对账单总支出、已付、未付明细列表)
        /// </summary>
        /// <param name="searchModel">查询实体</param>
        /// <param name="haveUserIds">用户Ids</param>
        /// <returns></returns>
        public IList<ArrearInfo> GetArrear(EyouSoft.Model.PlanStructure.ArrearSearchInfo searchModel, string haveUserIds)
        {
            if (searchModel.CompanyId <= 0)
                return null;

            StringBuilder searchCon = new StringBuilder();
            searchCon.AppendFormat("select * from View_TravelAndTicketArrear  where companyid={0} ", searchModel.CompanyId);
            if (searchModel.SeachType == 1) //总支出
                searchCon.Append(" and TotalAmount <> 0 ");
            if (searchModel.SeachType == 2)  //已付
                searchCon.Append(" and PayAmount <> 0 ");
            if (searchModel.SeachType == 3) //未付
                searchCon.Append(" and arrear <> 0 ");
            if (searchModel.LeaveDate1 != null)
                searchCon.AppendFormat(" and LeaveDate>='{0}' ", searchModel.LeaveDate1.Value);
            if (searchModel.LeaveDate1 != null)
                searchCon.AppendFormat(" and LeaveDate<='{0}' ", searchModel.LeaveDate2.Value);
            if (searchModel.OperateId > 0)
                searchCon.AppendFormat(" and operateid={0}", searchModel.OperateId);
            if (searchModel.SalerId > 0)
                searchCon.AppendFormat(" and salerid={0}", searchModel.SalerId);
            if (searchModel.AreaId > 0)
                searchCon.AppendFormat(" and areaid={0}", searchModel.AreaId);
            if (searchModel.TourType > 0)
                searchCon.AppendFormat(" and TourType={0}", searchModel.TourType);
            if (searchModel.SupplierId != null && searchModel.SupplierId.Length > 0)
            {
                string strIds = string.Empty;
                foreach (int i in searchModel.SupplierId)
                {
                    if (i > 0)
                        strIds += i.ToString() + ",";
                }
                strIds = strIds.Trim(',');
                if (!string.IsNullOrEmpty(strIds))
                    searchCon.AppendFormat(" and ServerId in ({0})", strIds);
            }
            if (!string.IsNullOrEmpty(searchModel.SupplierName))
                searchCon.AppendFormat(" and Supplier like '%{0}%' ", searchModel.SupplierName);
            if (searchModel.IsAccount.HasValue)
                searchCon.Append(searchModel.IsAccount.Value ? " and Arrear = 0 " : " and Arrear <> 0 ");
            /*if (!string.IsNullOrEmpty(haveUserIds))
                searchCon.AppendFormat(" and TourOperatorId in ({0}) ", haveUserIds);*/

            DbCommand cmd = this._db.GetSqlStringCommand(searchCon.ToString());
            IList<ArrearInfo> ArrearList = new List<ArrearInfo>();
            ArrearInfo model = null;
            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rd.Read())
                {
                    model = new ArrearInfo();
                    model.Arrear = rd.GetDecimal(rd.GetOrdinal("Arrear"));
                    model.LeaveDate = rd.IsDBNull(rd.GetOrdinal("LeaveDate")) ? DateTime.MinValue : rd.GetDateTime(rd.GetOrdinal("LeaveDate"));
                    model.RouteName = rd.IsDBNull(rd.GetOrdinal("RouteName")) ? "" : rd.GetString(rd.GetOrdinal("RouteName"));
                    model.TotalAmount = rd.GetDecimal(rd.GetOrdinal("TotalAmount")); ;
                    model.TourCode = rd.IsDBNull(rd.GetOrdinal("TourCode")) ? "" : rd.GetString(rd.GetOrdinal("TourCode"));
                    model.TourId = rd.IsDBNull(rd.GetOrdinal("TourId")) ? "" : rd.GetString(rd.GetOrdinal("TourId"));
                    model.Supplier = rd.IsDBNull(rd.GetOrdinal("Supplier")) ? "" : rd.GetString(rd.GetOrdinal("Supplier"));
                    model.Operator = rd.IsDBNull(rd.GetOrdinal("Operator")) ? "" : rd.GetString(rd.GetOrdinal("Operator"));
                    model.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)rd.GetByte(rd.GetOrdinal("TourType"));
                    ArrearList.Add(model);
                }
            }

            return ArrearList;
        }

        /// <summary>
        /// 统计中心-获取欠款金额汇总信息(统计分析-支出对账单总支出、已付、未付明细列表合计)
        /// </summary>
        /// <param name="searchModel">查询实体</param>
        /// <param name="haveUserIds">用户Ids</param>
        /// <param name="totalAmount">总支出</param>
        /// <param name="arrear">未付</param>
        /// <returns></returns>
        public void GetArrearAllMoeny(ArrearSearchInfo searchModel, string haveUserIds, ref decimal totalAmount, ref decimal arrear)
        {
            if (searchModel.CompanyId <= 0)
                return;

            totalAmount = 0;
            arrear = 0;
            var searchCon = new StringBuilder();
            searchCon.AppendFormat("select TotalAmount,Arrear from View_TravelAndTicketArrear  where companyid={0} ", searchModel.CompanyId);
            if (searchModel.SeachType == 1) //总支出
                searchCon.Append(" and TotalAmount <> 0 ");
            if (searchModel.SeachType == 2)  //已付
                searchCon.Append(" and PayAmount <> 0 ");
            if (searchModel.SeachType == 3) //未付
                searchCon.Append(" and arrear <> 0 ");
            if (searchModel.LeaveDate1 != null)
                searchCon.AppendFormat(" and leaveDate>='{0}' ", searchModel.LeaveDate1.Value);
            if (searchModel.LeaveDate1 != null)
                searchCon.AppendFormat(" and leaveDate<='{0}' ", searchModel.LeaveDate2.Value);
            if (searchModel.OperateId > 0)
                searchCon.AppendFormat(" and operateid={0}", searchModel.OperateId);
            if (searchModel.SalerId > 0)
                searchCon.AppendFormat(" and salerid={0}", searchModel.SalerId);
            if (searchModel.AreaId > 0)
                searchCon.AppendFormat(" and areaid={0}", searchModel.AreaId);
            if (searchModel.TourType > 0)
                searchCon.AppendFormat(" and TourType={0}", searchModel.TourType);
            if (searchModel.SupplierId != null && searchModel.SupplierId.Length > 0)
            {
                string strIds = string.Empty;
                foreach (int i in searchModel.SupplierId)
                {
                    if (i > 0)
                        strIds += i.ToString() + ",";
                }
                strIds = strIds.Trim(',');
                if (!string.IsNullOrEmpty(strIds))
                    searchCon.AppendFormat(" and ServerId in ({0})", strIds);
            }
            if (!string.IsNullOrEmpty(searchModel.SupplierName))
                searchCon.AppendFormat(" and Supplier like '%{0}%' ", searchModel.SupplierName);
            if (searchModel.IsAccount.HasValue)
                searchCon.Append(searchModel.IsAccount.Value ? " and Arrear = 0 " : " and Arrear <> 0 ");
            /*if (!string.IsNullOrEmpty(haveUserIds))
                searchCon.AppendFormat(" and TourOperatorId in ({0}) ", haveUserIds);*/

            DbCommand cmd = this._db.GetSqlStringCommand(searchCon.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(cmd, _db))
            {
                while (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                        totalAmount += dr.GetDecimal(0);
                    if (!dr.IsDBNull(1))
                        arrear += dr.GetDecimal(1);
                }
            }
        }

        #region 私有方法
        /// <summary>
        /// 创建价格组成对象XML
        /// </summary>
        /// <returns></returns>
        private string CreatePriceInfoXML(IList<EyouSoft.Model.PlanStructure.TravelAgencyPriceInfo> list)
        {
            if (list != null && list.Count > 0)
            {
                StringBuilder strXML = new StringBuilder("<ROOT>");
                foreach (EyouSoft.Model.PlanStructure.TravelAgencyPriceInfo model in list)
                {
                    //if (!string.IsNullOrEmpty(model.Title) || Enum.IsDefined(typeof(EyouSoft.Model.EnumType.TourStructure.ServiceType), model.PriceTpyeId))
                    if ((model.PriceTpyeAorB == EyouSoft.Model.EnumType.CompanyStructure.PriceComponent.统一报价 /*&& !string.IsNullOrEmpty(model.Title)*/)
                        || (model.PriceTpyeAorB == EyouSoft.Model.EnumType.CompanyStructure.PriceComponent.分项报价 && Enum.IsDefined(typeof(EyouSoft.Model.EnumType.TourStructure.ServiceType), model.PriceTpyeId)))
                        strXML.AppendFormat("<PriceInfo PriceType=\"{0}\" ID=\"{1}\" PeopleCount=\"{2}\" Price=\"{3}\"  Remark=\"{4}\" Title=\"{5}\"  PriceTpyeId=\"{6}\" />", (byte)model.PriceTpyeAorB, model.ID, model.PeopleCount, model.Price, model.Remark, model.Title, (byte)model.PriceTpyeId);
                }
                strXML.Append("</ROOT>");
                return strXML.ToString();
            }
            return "";
        }
        /// <summary>
        /// 创建接待行程XML
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string CreateTourPlanInfoXML(IList<EyouSoft.Model.PlanStructure.LocalAgencyTourPlanInfo> list)
        {
            if (list != null && list.Count > 0)
            {
                StringBuilder strXML = new StringBuilder("<ROOT>");
                foreach (EyouSoft.Model.PlanStructure.LocalAgencyTourPlanInfo model in list)
                {
                    if (model.Days > 0)
                        strXML.AppendFormat("<TourPlanInfo Days=\"{0}\" />", model.Days);
                }
                strXML.Append("</ROOT>");
                return strXML.ToString();
            }
            return "";
        }
        /// <summary>
        /// 地接社价格组成
        /// </summary>
        /// <param name="TravelId"></param>
        /// <returns></returns>
        private IList<TravelAgencyPriceInfo> GetTravelPriceList(string TravelId)
        {
            IList<TravelAgencyPriceInfo> priceList = new List<TravelAgencyPriceInfo>();
            TravelAgencyPriceInfo model = null;
            string sql = "select * from tbl_PlanLocalAgencyPrice where ReferenceID=@TravelId";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "TravelId", DbType.String, TravelId);
            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rd.Read())
                {
                    model = new TravelAgencyPriceInfo();
                    model.ID = rd.GetInt32(rd.GetOrdinal("ID"));
                    model.PeopleCount = rd.GetInt32(rd.GetOrdinal("PeopleCount"));
                    model.Price = rd.GetDecimal(rd.GetOrdinal("Price"));
                    model.PriceTpyeAorB = (EyouSoft.Model.EnumType.CompanyStructure.PriceComponent)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.PriceComponent), rd.GetByte(rd.GetOrdinal("PriceType")).ToString());
                    //  model.PriceTypeA = (EyouSoft.Model.EnumType.PlanStructure.TravelPriceAType)Enum.Parse(typeof(EyouSoft.Model.EnumType.PlanStructure.TravelPriceAType), rd.GetString(rd.GetOrdinal("PriceTypeA")));
                    model.PriceTpyeId = (EyouSoft.Model.EnumType.TourStructure.ServiceType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.ServiceType), rd.GetByte(rd.GetOrdinal("PriceTpyeId")).ToString());
                    model.ReferenceID = rd.GetString(rd.GetOrdinal("ReferenceID"));
                    model.Remark = rd.GetString(rd.GetOrdinal("Remark"));
                    model.Title = rd.GetString(rd.GetOrdinal("Title"));
                    priceList.Add(model);
                }

            }
            return priceList;
        }
        /// <summary>
        /// 返回地接社接待行程
        /// </summary>
        /// <param name="TravelId"></param>
        /// <returns></returns>
        private IList<LocalAgencyTourPlanInfo> GetTravelPlanList(string TravelId)
        {
            IList<LocalAgencyTourPlanInfo> palnList = new List<LocalAgencyTourPlanInfo>();
            LocalAgencyTourPlanInfo model = null;
            string sql = "select * from tbl_PlanLocalAgencyTourPlan where LocalTravelId=@TravelId";
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "TravelId", DbType.String, TravelId);
            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rd.Read())
                {
                    model = new LocalAgencyTourPlanInfo();
                    model.LocalTravelId = rd.IsDBNull(rd.GetOrdinal("LocalTravelId")) ? "0" : rd.GetString(rd.GetOrdinal("LocalTravelId"));
                    model.Days = (int)rd.GetByte(rd.GetOrdinal("Days"));
                    palnList.Add(model);
                }

            }
            return palnList;
        }

        /// <summary>
        /// 根据条件获取地接列表
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <returns></returns>
        private IList<LocalTravelAgencyInfo> GetTravelListWhere(string Where)
        {
            if (string.IsNullOrEmpty(Where))
                return null;

            IList<LocalTravelAgencyInfo> agencyList = new List<LocalTravelAgencyInfo>();
            LocalTravelAgencyInfo model = null;
            string sql = "  select * from tbl_PlanLocalAgency where 1=1 " + Where;
            DbCommand cmd = this._db.GetSqlStringCommand(sql);

            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rd.Read())
                {
                    model = new LocalTravelAgencyInfo();
                    model.Commission = rd.IsDBNull(rd.GetOrdinal("Commission")) ? 0 : rd.GetDecimal(rd.GetOrdinal("Commission"));
                    model.DeliverTime = rd.IsDBNull(rd.GetOrdinal("DeliverTime")) ? DateTime.Parse("2011-01-29") : rd.GetDateTime(rd.GetOrdinal("DeliverTime"));
                    model.ReceiveTime = rd.IsDBNull(rd.GetOrdinal("ReceiveTime")) ? DateTime.Parse("2011-01-29") : rd.GetDateTime(rd.GetOrdinal("ReceiveTime"));
                    model.Fee = rd.GetDecimal(rd.GetOrdinal("Fee"));
                    model.ID = rd.GetString(rd.GetOrdinal("ID"));
                    model.LocalTravelAgency = rd.IsDBNull(rd.GetOrdinal("LocalTravelAgency")) ? "" : rd.GetString(rd.GetOrdinal("LocalTravelAgency"));
                    model.TravelAgencyID = rd.GetInt32(rd.GetOrdinal("TravelAgencyID"));
                    model.Settlement = rd.GetDecimal(rd.GetOrdinal("Settlement"));
                    model.PayType = (EyouSoft.Model.EnumType.TourStructure.RefundType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.RefundType), rd.GetByte(rd.GetOrdinal("PayType")).ToString());

                    agencyList.Add(model);
                }
            }
            return agencyList;
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
