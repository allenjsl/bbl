using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using System.Data;
using System.Data.Common;
using EyouSoft.Toolkit;

namespace EyouSoft.DAL.RouteStructure
{
    /// <summary>
    /// 报价数据访问类
    /// </summary>
    public class Quote : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.RouteStructure.IQuote
    {
        #region 私有变量
        private readonly Database _db = null;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public Quote()
        {
            _db = this.SystemStore;
        }
        #endregion

        #region IQuote 成员
        /// <summary>
        /// 写入团队计划报价信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">团队计划报价信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int InsertTourTeamQuote(EyouSoft.Model.RouteStructure.QuoteTeamInfo info)
        {
            DbCommand dc = this._db.GetStoredProcCommand("proc_RouteQuote_InsertQuoteInfo");
            this._db.AddInParameter(dc, "AdultNum", DbType.Int32, info.AdultNum);
            this._db.AddInParameter(dc, "ChildNum", DbType.Int32, info.ChildNum);
            this._db.AddInParameter(dc, "ContactName", DbType.String, info.ContactName);
            this._db.AddInParameter(dc, "ContactTel", DbType.String, info.ContactTel);
            this._db.AddInParameter(dc, "LocalQuoteSum", DbType.Decimal, info.LocalQuoteSum);
            this._db.AddInParameter(dc, "OperatorId", DbType.Int32, info.OperatorId);
            this._db.AddInParameter(dc, "PeopleNum", DbType.Int32, info.PeopleNum);
            this._db.AddInParameter(dc, "QuoteUnitsId", DbType.Int32, info.QuoteUnitsId);
            this._db.AddInParameter(dc, "QuoteUnitsName", DbType.String, info.QuoteUnitsName);
            this._db.AddInParameter(dc, "Remark", DbType.String, info.Remark);
            this._db.AddInParameter(dc, "RouteId", DbType.Int32, info.RouteId);
            this._db.AddInParameter(dc, "MyQuoteSum", DbType.Decimal, info.SelfQuoteSum);
            this._db.AddInParameter(dc, "TicketAgio", DbType.Decimal, info.TicketAgio);
            this._db.AddInParameter(dc, "TmpLeaveDate", DbType.DateTime, info.TmpLeaveDate);
            this._db.AddInParameter(dc, "QuoteAskXML", DbType.String, this.CreateQuoteAskXML(info.Requirements));
            this._db.AddInParameter(dc, "QuoteListXML", DbType.String, this.CreateQuoteListXML(info.Services));
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, this._db);
            object obj = this._db.GetParameterValue(dc, "Result");
            if (obj == null)
                return 0;
            return int.Parse(obj.ToString());
        }
        /// <summary>
        /// 更新团队计划报价信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">团队计划报价信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int UpdateTourTeamQuote(EyouSoft.Model.RouteStructure.QuoteTeamInfo info)
        {
            DbCommand dc = this._db.GetStoredProcCommand("proc_RouteQuote_UpdateQuoteInfo");
            this._db.AddInParameter(dc, "Id", DbType.Int32, info.QuoteId);
            this._db.AddInParameter(dc, "AdultNum", DbType.Int32, info.AdultNum);
            this._db.AddInParameter(dc, "ChildNum", DbType.Int32, info.ChildNum);
            this._db.AddInParameter(dc, "ContactName", DbType.String, info.ContactName);
            this._db.AddInParameter(dc, "ContactTel", DbType.String, info.ContactTel);
            this._db.AddInParameter(dc, "LocalQuoteSum", DbType.Decimal, info.LocalQuoteSum);
            this._db.AddInParameter(dc, "OperatorId", DbType.Int32, info.OperatorId);
            this._db.AddInParameter(dc, "PeopleNum", DbType.Int32, info.PeopleNum);
            this._db.AddInParameter(dc, "QuoteUnitsId", DbType.Int32, info.QuoteUnitsId);
            this._db.AddInParameter(dc, "QuoteUnitsName", DbType.String, info.QuoteUnitsName);
            this._db.AddInParameter(dc, "Remark", DbType.String, info.Remark);
            this._db.AddInParameter(dc, "RouteId", DbType.Int32, info.RouteId);
            this._db.AddInParameter(dc, "MyQuoteSum", DbType.Decimal, info.SelfQuoteSum);
            this._db.AddInParameter(dc, "TicketAgio", DbType.Decimal, info.TicketAgio);
            this._db.AddInParameter(dc, "TmpLeaveDate", DbType.DateTime, info.TmpLeaveDate);
            this._db.AddInParameter(dc, "QuoteAskXML", DbType.String, this.CreateQuoteAskXML(info.Requirements));
            this._db.AddInParameter(dc, "QuoteListXML", DbType.String, this.CreateQuoteListXML(info.Services));
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, this._db);
            object obj = this._db.GetParameterValue(dc, "Result");
            if (obj == null)
                return 0;
            return int.Parse(obj.ToString());
        }
        /// <summary>
        /// 删除团队计划报价信息
        /// </summary>
        /// <param name="quoteId">报价编号</param>
        /// <returns></returns>
        public bool DeleteTourTeamQuote(int quoteId)
        {
            DbCommand dc = this._db.GetStoredProcCommand("proc_RouteQuote_DeleteQuoteInfo");
            this._db.AddInParameter(dc, "Id", DbType.Int32, quoteId);
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, this._db);
            object obj = this._db.GetParameterValue(dc, "Result");
            if (obj == null)
                return false;
            return int.Parse(obj.ToString()) > 0 ? true : false;
        }
        /// <summary>
        /// 获取团队计划报价信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="routeId">线路编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.RouteStructure.QuoteTeamInfo> GetQuotesTeam(int companyId, int routeId, int pageSize, int pageIndex, ref int recordCount)
        {
            IList<EyouSoft.Model.RouteStructure.QuoteTeamInfo> list = new List<EyouSoft.Model.RouteStructure.QuoteTeamInfo>();
            string tableName = "tbl_RouteQuote";
            string fields = "Id,QuoteUnitsName,ContactName,ContactTel,TmpLeaveDate,AdultNum,ChildNum,PeopleNum,LocalQuoteSum,MyQuoteSum,TicketAgio,Remark,OperatorId,IssueTime,RouteId";
            string primaryKey = "Id";
            string orderStrBy = " IssueTime desc ";
            StringBuilder strWhere = new StringBuilder();
            if (routeId > 0)
                strWhere.AppendFormat(" RouteId={0} ", routeId);
            using (IDataReader dr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields, strWhere.ToString(), orderStrBy))
            {
                while (dr.Read())
                {
                    EyouSoft.Model.RouteStructure.QuoteTeamInfo model = new EyouSoft.Model.RouteStructure.QuoteTeamInfo();
                    if (!dr.IsDBNull(dr.GetOrdinal("AdultNum")))
                        model.AdultNum = dr.GetInt32(dr.GetOrdinal("AdultNum"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ChildNum")))
                        model.ChildNum = dr.GetInt32(dr.GetOrdinal("ChildNum"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactName")))
                        model.ContactName = dr.GetString(dr.GetOrdinal("ContactName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactTel")))
                        model.ContactTel = dr.GetString(dr.GetOrdinal("ContactTel"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                        model.CreateTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("LocalQuoteSum")))
                        model.LocalQuoteSum = dr.GetDecimal(dr.GetOrdinal("LocalQuoteSum"));
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorId")))
                        model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PeopleNum")))
                        model.PeopleNum = dr.GetInt32(dr.GetOrdinal("PeopleNum"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Id")))
                        model.QuoteId = dr.GetInt32(dr.GetOrdinal("Id"));
                    if (!dr.IsDBNull(dr.GetOrdinal("QuoteUnitsName")))
                        model.QuoteUnitsName = dr.GetString(dr.GetOrdinal("QuoteUnitsName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Remark")))
                        model.Remark = dr.GetString(dr.GetOrdinal("Remark"));
                    if (!dr.IsDBNull(dr.GetOrdinal("RouteId")))
                        model.RouteId = dr.GetInt32(dr.GetOrdinal("RouteId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("MyQuoteSum")))
                        model.SelfQuoteSum = dr.GetDecimal(dr.GetOrdinal("MyQuoteSum"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TicketAgio")))
                        model.TicketAgio = dr.GetDecimal(dr.GetOrdinal("TicketAgio"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TmpLeaveDate")))
                        model.TmpLeaveDate = dr.GetDateTime(dr.GetOrdinal("TmpLeaveDate"));
                    list.Add(model);
                    model = null;
                }
            }
            return list;
        }
        /// <summary>
        /// 获取线路报价信息实体
        /// </summary>
        /// <param name="QuoteId">线路报价信息编号</param>
        /// <returns></returns>
        public EyouSoft.Model.RouteStructure.QuoteTeamInfo GetQuoteInfo(int QuoteId)
        {
            EyouSoft.Model.RouteStructure.QuoteTeamInfo model = null;
            DbCommand dc = this._db.GetStoredProcCommand("proc_RouteQuote_SelectQuoteInfo");
            this._db.AddInParameter(dc, "Id", DbType.Int32, QuoteId);
            using (IDataReader dr = DbHelper.RunReaderProcedure(dc, this._db))
            {
                if (dr.Read())
                {
                    model = new EyouSoft.Model.RouteStructure.QuoteTeamInfo();

                    #region 线路报价基本信息
                    if (!dr.IsDBNull(dr.GetOrdinal("AdultNum")))
                        model.AdultNum = dr.GetInt32(dr.GetOrdinal("AdultNum"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ChildNum")))
                        model.ChildNum = dr.GetInt32(dr.GetOrdinal("ChildNum"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactName")))
                        model.ContactName = dr.GetString(dr.GetOrdinal("ContactName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactTel")))
                        model.ContactTel = dr.GetString(dr.GetOrdinal("ContactTel"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                        model.CreateTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("LocalQuoteSum")))
                        model.LocalQuoteSum = dr.GetDecimal(dr.GetOrdinal("LocalQuoteSum"));
                    if (!dr.IsDBNull(dr.GetOrdinal("OperatorId")))
                        model.OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PeopleNum")))
                        model.PeopleNum = dr.GetInt32(dr.GetOrdinal("PeopleNum"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Id")))
                        model.QuoteId = dr.GetInt32(dr.GetOrdinal("Id"));
                    if (!dr.IsDBNull(dr.GetOrdinal("QuoteUnitsName")))
                        model.QuoteUnitsName = dr.GetString(dr.GetOrdinal("QuoteUnitsName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Remark")))
                        model.Remark = dr.GetString(dr.GetOrdinal("Remark"));
                    if (!dr.IsDBNull(dr.GetOrdinal("RouteId")))
                        model.RouteId = dr.GetInt32(dr.GetOrdinal("RouteId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("MyQuoteSum")))
                        model.SelfQuoteSum = dr.GetDecimal(dr.GetOrdinal("MyQuoteSum"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TicketAgio")))
                        model.TicketAgio = dr.GetDecimal(dr.GetOrdinal("TicketAgio"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TmpLeaveDate")))
                        model.TmpLeaveDate = dr.GetDateTime(dr.GetOrdinal("TmpLeaveDate"));
                    if (!dr.IsDBNull(dr.GetOrdinal("QuoteUnitsId")))
                        model.QuoteUnitsId = dr.GetInt32(dr.GetOrdinal("QuoteUnitsId"));
                    #endregion

                    #region 线路报价客户要求信息
                    IList<EyouSoft.Model.TourStructure.TourServiceInfo> ServiceList = new List<EyouSoft.Model.TourStructure.TourServiceInfo>();
                    dr.NextResult();
                    while (dr.Read())
                    {
                        EyouSoft.Model.TourStructure.TourServiceInfo ServiceModel = new EyouSoft.Model.TourStructure.TourServiceInfo();
                        if (!dr.IsDBNull(dr.GetOrdinal("ConcreteAsk")))
                            ServiceModel.Service = dr[dr.GetOrdinal("ConcreteAsk")].ToString();
                        if (!dr.IsDBNull(dr.GetOrdinal("ItemType")))
                            ServiceModel.ServiceType = (EyouSoft.Model.EnumType.TourStructure.ServiceType)int.Parse(dr[dr.GetOrdinal("ItemType")].ToString());
                        ServiceList.Add(ServiceModel);
                        ServiceModel = null;
                    }
                    model.Requirements = ServiceList;
                    #endregion

                    #region 线路报价明细信息
                    dr.NextResult();
                    IList<EyouSoft.Model.TourStructure.TourTeamServiceInfo> TeamService = new List<EyouSoft.Model.TourStructure.TourTeamServiceInfo>();
                    while (dr.Read())
                    {
                        EyouSoft.Model.TourStructure.TourTeamServiceInfo TeamServiceModel = new EyouSoft.Model.TourStructure.TourTeamServiceInfo();
                        if (!dr.IsDBNull(dr.GetOrdinal("LocalQuote")))
                            TeamServiceModel.LocalPrice = dr.GetDecimal(dr.GetOrdinal("LocalQuote"));
                        if (!dr.IsDBNull(dr.GetOrdinal("MyQuote")))
                            TeamServiceModel.SelfPrice = dr.GetDecimal(dr.GetOrdinal("MyQuote"));
                        if (!dr.IsDBNull(dr.GetOrdinal("Reception")))
                            TeamServiceModel.Service = dr.GetString(dr.GetOrdinal("Reception"));
                        if (!dr.IsDBNull(dr.GetOrdinal("ItemId")))
                            TeamServiceModel.ServiceType = (EyouSoft.Model.EnumType.TourStructure.ServiceType)int.Parse(dr[dr.GetOrdinal("ItemId")].ToString());
                        TeamServiceModel.LocalPeopleNumber = dr.GetInt32(dr.GetOrdinal("LocalPeopleNumber"));
                        TeamServiceModel.LocalUnitPrice = dr.GetDecimal(dr.GetOrdinal("LocalUnitPrice"));
                        TeamServiceModel.SelfPeopleNumber = dr.GetInt32(dr.GetOrdinal("SelfPeopleNumber"));
                        TeamServiceModel.SelfUnitPrice = dr.GetDecimal(dr.GetOrdinal("SelfUnitPrice"));
                        TeamService.Add(TeamServiceModel);
                        TeamServiceModel = null;
                    }
                    model.Services = TeamService;
                    #endregion
                }
            }
            return model;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 创建线路报价客户要求XML
        /// </summary>
        /// <param name="list">线路报价客户要求集合</param>
        /// <returns></returns>
        private string CreateQuoteAskXML(IList<EyouSoft.Model.TourStructure.TourServiceInfo> list)
        {
            if (list == null || list.Count < 1) return string.Empty;

            StringBuilder strXml = new StringBuilder();
            strXml.Append("<ROOT>");
            foreach (var item in list)
            {
                strXml.AppendFormat("<QuoteAskInfo ItemType=\"{0}\" ConcreteAsk=\"{1}\" />", (int)item.ServiceType, Utils.ReplaceXmlSpecialCharacter(item.Service));
            }
            strXml.Append("</ROOT>");
            return strXml.ToString();
        }
        /// <summary>
        /// 创建线路包含项目信息XML
        /// </summary>
        /// <param name="list">线路包含项目集合</param>
        /// <returns></returns>
        private string CreateQuoteListXML(IList<EyouSoft.Model.TourStructure.TourTeamServiceInfo> list)
        {
            //XML:<ROOT><QuoteInfo ItemId="项目类型" Reception="接待标准" LocalQuote="地接报价" MyQuote="我社报价" LocalPeopleNumber="地接报价人数" LocalUnitPrice="地接报价单价" SelfPeopleNumber="我社报价人数" SelfUnitPrice="我社报价单价" /></ROOT>
            if (list == null || list.Count < 1) return string.Empty;
            StringBuilder strXml = new StringBuilder();
            strXml.Append("<ROOT>");
            foreach (var item in list)
            {
                strXml.AppendFormat("<QuoteInfo ItemId=\"{0}\" Reception=\"{1}\" LocalQuote=\"{2}\" MyQuote=\"{3}\" LocalPeopleNumber=\"{4}\" LocalUnitPrice=\"{5}\" SelfPeopleNumber=\"{6}\"  SelfUnitPrice=\"{7}\"  />", (int)item.ServiceType
                    , Utils.ReplaceXmlSpecialCharacter(item.Service)
                    , item.LocalPrice
                    , item.SelfPrice
                    , item.LocalPeopleNumber
                    , item.LocalUnitPrice
                    , item.SelfPeopleNumber
                    , item.SelfUnitPrice);
            }
            strXml.Append("</ROOT>");
            return strXml.ToString();
        }
        #endregion
    }
}
