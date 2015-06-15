using System;
using System.Data;
using System.Data.Common;
using EyouSoft.Toolkit.DAL;
using System.Collections.Generic;
using EyouSoft.Toolkit;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;

namespace EyouSoft.DAL.TourStructure
{
    /// <summary>
    /// 描述：询价报价数据类
    /// 修改记录：
    /// 1.2010-3-18　PM 曹胡生　创建
    /// </summary>
    public class LineInquireQuoteInfo : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.TourStructure.ILineInquireQuoteInfo
    {
        #region static constants
        //static constants
        private const string DEFAULT_XML_DOC = "<ROOT></ROOT>";
        #endregion

        private readonly Database DB = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        public LineInquireQuoteInfo()
        {
            DB = this.SystemStore;
        }

        #region 私有方法
        /// <summary>
        /// 创建询价客户要求XML
        /// </summary>
        /// <param name="list">询价客户要求集合</param>
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
        /// 创建询价行程要求XML
        /// </summary>
        /// <param name="list">询价行程要求集合</param>
        /// <returns></returns>
        private string CreateXingChengAskXML(EyouSoft.Model.TourStructure.XingChengMust XingChengMust)
        {
            if (XingChengMust == null) return string.Empty;
            StringBuilder strXml = new StringBuilder();
            strXml.Append("<ROOT>");
            strXml.AppendFormat("<XingCheng QuotePlan=\"{0}\" PlanAccessoryName=\"{1}\" PlanAccessory=\"{2}\" />",
            Utils.ReplaceXmlSpecialCharacter(XingChengMust.QuotePlan), Utils.ReplaceXmlSpecialCharacter(XingChengMust.PlanAccessoryName),
            Utils.ReplaceXmlSpecialCharacter(XingChengMust.PlanAccessory));
            strXml.Append("</ROOT>");
            return strXml.ToString();
        }
        /// <summary>
        /// 创建报价价格组成XML
        /// </summary>
        /// <param name="list">报价价格组成集合</param>
        /// <returns></returns>
        private string CreateQuoteListXML(IList<EyouSoft.Model.TourStructure.TourTeamServiceInfo> list)
        {
            //XML:<ROOT><QuoteInfo QuoteId="询价编号" ItemId="项目类型" Reception="接待标准" LocalQuote="地接报价" MyQuote="我社报价" LocalPeopleNumber="地接报价人数" LocalUnitPrice="地接报价单价" SelfPeopleNumber="我社报价人数" SelfUnitPrice="我社报价单价" /></ROOT>
            if (list == null || list.Count < 1) return string.Empty;
            StringBuilder strXml = new StringBuilder();
            strXml.Append("<ROOT>");
            foreach (var item in list)
            {
                strXml.AppendFormat("<QuoteInfo ItemId=\"{0}\" Reception=\"{1}\" LocalQuote=\"{2}\" MyQuote=\"{3}\" LocalPeopleNumber=\"{4}\" LocalUnitPrice=\"{5}\" SelfPeopleNumber=\"{6}\" SelfUnitPrice=\"{7}\" />", (int)item.ServiceType
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

        /// <summary>
        /// 创建游客信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateQuoteTravellersXML(IList<EyouSoft.Model.TourStructure.TourOrderCustomer> items)
        {
            //XML:<ROOT><Info TravellerId="游客编号" TravellerName="游客姓名" CertificateType="证件类型" CertificateCode="证件号码" Gender="性别" TravellerType="游客类型" Telephone="电话" SpecialServiceName="特服项目" SpecialServiceDetail="特服内容" SpecialServiceIsAdd="特服增/减" SpecialServiceFee="特服费用" /></ROOT>
            if (items == null && items.Count < 1) return DEFAULT_XML_DOC;
            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item.ID)) item.ID = Guid.NewGuid().ToString();

                xmlDoc.AppendFormat("<Info TravellerId=\"{0}\" TravellerName=\"{1}\" CertificateType=\"{2}\" CertificateCode=\"{3}\" Gender=\"{4}\" TravellerType=\"{5}\" Telephone=\"{6}\" SpecialServiceName=\"{7}\" SpecialServiceDetail=\"{8}\" SpecialServiceIsAdd=\"{9}\" SpecialServiceFee=\"{10}\" />", item.ID
                    , Utils.ReplaceXmlSpecialCharacter(item.VisitorName)
                    , (int)item.CradType
                    , Utils.ReplaceXmlSpecialCharacter(item.CradNumber)
                    , (int)item.Sex
                    , (int)item.VisitorType
                    , Utils.ReplaceXmlSpecialCharacter(item.ContactTel)
                    , item.SpecialServiceInfo != null ? Utils.ReplaceXmlSpecialCharacter(item.SpecialServiceInfo.ProjectName) : ""
                    , item.SpecialServiceInfo != null ? Utils.ReplaceXmlSpecialCharacter(item.SpecialServiceInfo.ServiceDetail) : ""
                    , item.SpecialServiceInfo != null ? (item.SpecialServiceInfo.IsAdd ? 1 : 0) : 0
                    , item.SpecialServiceInfo != null ? item.SpecialServiceInfo.Fee : 0);
            }
            xmlDoc.Append("</ROOT>");
            return xmlDoc.ToString();
        }
        #endregion

        #region EyouSoft.IDAL.TourStructure.ILineInquireQuoteInfo members
        /// <summary>
        /// 获取询价列表
        /// </summary>
        /// <param name="companyId">公司编号（专线：专线公司编号，组团：组团公司编号）</param>
        /// <param name="pageSize">每页显示的记录数</param>
        /// <param name="pageIndex">返回第几页</param>
        /// <param name="recordCount">返回的记录数</param>
        /// <param name="isZhuTuan">专线：False,组团：True</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LineInquireQuoteInfo> GetInquireList(int companyId, int pageSize, int pageIndex, ref int recordCount, bool isZhuTuan, EyouSoft.Model.TourStructure.LineInquireQuoteSearchInfo SearchInfo)
        {
            IList<EyouSoft.Model.TourStructure.LineInquireQuoteInfo> items = new List<EyouSoft.Model.TourStructure.LineInquireQuoteInfo>();
            EyouSoft.Model.TourStructure.LineInquireQuoteInfo item = null;
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_CustomerQuote";
            string primaryKey = "Id";
            string orderByString = "IssueTime DESC";
            StringBuilder fields = new StringBuilder();
            #region 要查询的字段
            fields.Append(" Id,LeaveDate,RouteName,CustomerId,AdultNumber+ChildNumber AS PeopleNum,(select name from tbl_Customer where Id=tbl_CustomerQuote.CustomerId) as CustomerName,ContactName,ContactTel,QuoteState,");
            fields.Append("(select TourCode from tbl_Tour where TourId=tbl_CustomerQuote.BuildTourId) as TourCode,");
            //fields.Append("(select RouteName from tbl_Tour where TourId=tbl_CustomerQuote.BuildTourId) as RouteName,");
            fields.Append("(select TourDays from tbl_Tour where TourId=tbl_CustomerQuote.BuildTourId) as TourDays");
            //fields.Append("(select LeaveDate from tbl_Tour where TourId=tbl_CustomerQuote.BuildTourId) as ChuTuanDate");
            fields.Append(" ,IssueTime ");
            #endregion
            #region 拼接查询条件
            if (isZhuTuan)
            {
                cmdQuery.AppendFormat(" CustomerId={0} AND IsDelete=0", companyId);
            }
            else
            {
                cmdQuery.AppendFormat(" CompanyId={0} AND IsDelete=0", companyId);
            }
            if (SearchInfo != null)
            {
                if (!string.IsNullOrEmpty(SearchInfo.TourNo) || SearchInfo.DayNum != 0)
                {
                    cmdQuery.Append(" AND EXISTS(SELECT 1 FROM tbl_Tour AS A WHERE A.TourId=tbl_CustomerQuote.BuildTourId ");

                    if (!string.IsNullOrEmpty(SearchInfo.TourNo))
                    {
                        cmdQuery.AppendFormat(" AND A.TourCode LIKE '%{0}%' ", SearchInfo.TourNo);
                    }
                    if (SearchInfo.DayNum != 0)
                    {
                        cmdQuery.AppendFormat(" AND A.TourDays={0} ", SearchInfo.DayNum);
                    }

                    cmdQuery.Append(" ) ");
                }
                if (!String.IsNullOrEmpty(SearchInfo.RouteName))
                {
                    cmdQuery.AppendFormat(" AND RouteName like  '%{0}%'", SearchInfo.RouteName);
                }
                if (SearchInfo.SDate.HasValue && SearchInfo.SDate != DateTime.MinValue)
                {
                    cmdQuery.AppendFormat(" AND LeaveDate>'{0}' ", SearchInfo.SDate.Value.AddDays(-1));
                }
                if (SearchInfo.EDate.HasValue && SearchInfo.EDate != DateTime.MinValue)
                {
                    cmdQuery.AppendFormat(" AND LeaveDate<'{0}' ", SearchInfo.EDate.Value.AddDays(1));
                }                
                if (SearchInfo.XunTuanETime.HasValue)
                {
                    cmdQuery.AppendFormat(" AND IssueTime<'{0}' ", SearchInfo.XunTuanETime.Value.AddDays(1));
                }
                if (SearchInfo.XunTuanSTime.HasValue)
                {
                    cmdQuery.AppendFormat(" AND IssueTime>'{0}' ", SearchInfo.XunTuanSTime.Value);
                }
            }
            #endregion
            using (IDataReader rdr = DbHelper.ExecuteReader(this.DB, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    item = new EyouSoft.Model.TourStructure.LineInquireQuoteInfo()
                    {
                        Id = rdr.GetInt32(rdr.GetOrdinal("Id")),
                        CustomerId = rdr.GetInt32(rdr.GetOrdinal("CustomerId")),
                        RouteName = rdr["RouteName"].ToString(),
                        LeaveDate = rdr.IsDBNull(rdr.GetOrdinal("LeaveDate")) ? System.DateTime.Now : rdr.GetDateTime(rdr.GetOrdinal("LeaveDate")),
                        PeopleNum = rdr.IsDBNull(rdr.GetOrdinal("PeopleNum")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("PeopleNum")),
                        CustomerName = rdr["CustomerName"].ToString(),
                        ContactName = rdr["ContactName"].ToString(),
                        ContactTel = rdr["ContactTel"].ToString(),
                        QuoteState = (EyouSoft.Model.EnumType.TourStructure.QuoteState)rdr.GetByte(rdr.GetOrdinal("QuoteState")),
                        IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"))
                    };
                    items.Add(item);
                }
            }
            return items;
        }

        /// <summary>
        /// <summary>
        /// 获取询价报价实体
        /// </summary>
        /// <param name="Id">主键编号</param>
        /// <param name="CompanyId">专线公司编号</param>
        /// <param name="CustomerId">组团公司编号</param>
        /// <param name="isZhuTuan">是否组团端,1是，0不是</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.LineInquireQuoteInfo GetQuoteModel(int Id, int CompanyId, int CustomerId, int isZhuTuan)
        {
            EyouSoft.Model.TourStructure.LineInquireQuoteInfo model = null;
            DbCommand dc = this.DB.GetStoredProcCommand("proc_Tour_GetInquireQuote");
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, CompanyId);
            this.DB.AddInParameter(dc, "CustomerId", DbType.Int32, CustomerId);
            this.DB.AddInParameter(dc, "isZhuTuan", DbType.Int32, isZhuTuan);
            this.DB.AddInParameter(dc, "Id", DbType.Int32, Id);
            using (IDataReader dr = DbHelper.RunReaderProcedure(dc, this.DB))
            {
                if (dr.Read())
                {
                    #region 询价报价基本信息
                    model = new EyouSoft.Model.TourStructure.LineInquireQuoteInfo();
                    model.Id = dr.GetInt32(dr.GetOrdinal("Id"));
                    model.CompanyId = dr.IsDBNull(dr.GetOrdinal("CompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CompanyId"));
                    model.RouteId = dr.IsDBNull(dr.GetOrdinal("RouteId")) ? 0 : dr.GetInt32(dr.GetOrdinal("RouteId"));
                    model.RouteName = dr["RouteName"].ToString();
                    model.CustomerId = dr.IsDBNull(dr.GetOrdinal("CustomerId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CustomerId"));
                    model.CustomerName = dr["CustomerName"].ToString();
                    model.ContactName = dr["ContactName"].ToString();
                    model.ContactTel = dr["ContactTel"].ToString();
                    model.LeaveDate = dr.IsDBNull(dr.GetOrdinal("LeaveDate")) ? System.DateTime.Now : dr.GetDateTime(dr.GetOrdinal("LeaveDate"));
                    model.AdultNumber = dr.IsDBNull(dr.GetOrdinal("AdultNumber")) ? 0 : dr.GetInt32(dr.GetOrdinal("AdultNumber"));
                    model.ChildNumber = dr.IsDBNull(dr.GetOrdinal("ChildNumber")) ? 0 : dr.GetInt32(dr.GetOrdinal("ChildNumber"));
                    model.PeopleNum = model.AdultNumber + model.ChildNumber;
                    model.SpecialClaim = dr["SpecialClaim"].ToString();
                    model.TicketAgio = dr.IsDBNull(dr.GetOrdinal("TicketAgio")) ? 0 : dr.GetDecimal(dr.GetOrdinal("TicketAgio"));
                    model.IssueTime = dr.IsDBNull(dr.GetOrdinal("IssueTime")) ? System.DateTime.Now : dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    model.QuoteState = (EyouSoft.Model.EnumType.TourStructure.QuoteState)dr.GetByte(dr.GetOrdinal("QuoteState"));
                    model.Remark = dr["Remark"].ToString();
                    model.BuildTourId = dr["BuildTourId"].ToString();
                    model.TotalAmount = dr.GetDecimal(dr.GetOrdinal("TotalAmount"));
                    model.Traveller = new EyouSoft.Model.TourStructure.TourEverydayApplyTravellerInfo();
                    model.Traveller.TravellerDisplayType = (EyouSoft.Model.EnumType.TourStructure.CustomerDisplayType)dr.GetByte(dr.GetOrdinal("TravellerDisplayType"));
                    model.Traveller.TravellerFilePath = dr["TravellerFilePath"].ToString();
                    model.Traveller.Travellers = new List<EyouSoft.Model.TourStructure.TourOrderCustomer>();
                    #endregion
                    #region 行程要求
                    EyouSoft.Model.TourStructure.XingChengMust XingChengMust = null;
                    dr.NextResult();
                    if (dr.Read())
                    {
                        XingChengMust = new EyouSoft.Model.TourStructure.XingChengMust()
                        {
                            QuoteId = dr.IsDBNull(dr.GetOrdinal("QuoteId")) ? 0 : dr.GetInt32(dr.GetOrdinal("QuoteId")),
                            QuotePlan = dr["QuotePlan"].ToString(),
                            PlanAccessory = dr["PlanAccessory"].ToString(),
                            PlanAccessoryName = dr["PlanAccessoryName"].ToString()
                        };
                    }
                    model.XingCheng = XingChengMust;
                    #endregion
                    #region 客人要求信息集合
                    IList<EyouSoft.Model.TourStructure.TourServiceInfo> TourServiceInfoList = new List<EyouSoft.Model.TourStructure.TourServiceInfo>();
                    EyouSoft.Model.TourStructure.TourServiceInfo TourServiceInfo = null;
                    dr.NextResult();
                    while (dr.Read())
                    {
                        TourServiceInfo = new EyouSoft.Model.TourStructure.TourServiceInfo()
                        {
                            Service = dr["ConcreteAsk"].ToString(),
                            ServiceType = (EyouSoft.Model.EnumType.TourStructure.ServiceType)dr.GetInt32(dr.GetOrdinal("ItemType"))
                        };
                        TourServiceInfoList.Add(TourServiceInfo);
                    }
                    model.Requirements = TourServiceInfoList;
                    #endregion
                    #region 价格组成信息集合
                    IList<EyouSoft.Model.TourStructure.TourTeamServiceInfo> TourTeamServiceInfoList = new List<EyouSoft.Model.TourStructure.TourTeamServiceInfo>();
                    EyouSoft.Model.TourStructure.TourTeamServiceInfo TourTeamServiceInfo = null;
                    dr.NextResult();
                    while (dr.Read())
                    {
                        TourTeamServiceInfo = new EyouSoft.Model.TourStructure.TourTeamServiceInfo()
                        {
                            Service = dr["Reception"].ToString(),
                            LocalPrice = dr.IsDBNull(dr.GetOrdinal("LocalQuote")) ? 0 : dr.GetDecimal(dr.GetOrdinal("LocalQuote")),
                            SelfPrice = dr.IsDBNull(dr.GetOrdinal("MyQuote")) ? 0 : dr.GetDecimal(dr.GetOrdinal("MyQuote")),
                            ServiceType = (EyouSoft.Model.EnumType.TourStructure.ServiceType)dr.GetInt32(dr.GetOrdinal("ItemId")),
                            LocalPeopleNumber = dr.GetInt32(dr.GetOrdinal("LocalPeopleNumber")),
                            LocalUnitPrice = dr.GetDecimal(dr.GetOrdinal("LocalUnitPrice")),
                            SelfPeopleNumber = dr.GetInt32(dr.GetOrdinal("SelfPeopleNumber")),
                            SelfUnitPrice = dr.GetDecimal(dr.GetOrdinal("SelfUnitPrice"))
                        };
                        TourTeamServiceInfoList.Add(TourTeamServiceInfo);
                    }
                    model.Services = TourTeamServiceInfoList;
                    #endregion

                    #region 游客信息
                    if (dr.NextResult() && model != null)
                    {
                        while (dr.Read())
                        {
                            EyouSoft.Model.TourStructure.TourOrderCustomer traveller = new EyouSoft.Model.TourStructure.TourOrderCustomer()
                            {
                                ID = dr.GetString(dr.GetOrdinal("TravellerId")),
                                VisitorName = dr["TravellerName"].ToString(),
                                CradType = (EyouSoft.Model.EnumType.TourStructure.CradType)dr.GetByte(dr.GetOrdinal("CertificateType")),
                                CradNumber = dr["CertificateCode"].ToString(),
                                Sex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)dr.GetByte(dr.GetOrdinal("Gender")),
                                VisitorType = (EyouSoft.Model.EnumType.TourStructure.VisitorType)dr.GetByte(dr.GetOrdinal("TravellerType")),
                                ContactTel = dr["Telephone"].ToString(),                                
                            };
                            traveller.SpecialServiceInfo = new EyouSoft.Model.TourStructure.CustomerSpecialService()
                            {
                                CustormerId = traveller.ID,
                                Fee = !dr.IsDBNull(dr.GetOrdinal("Fee")) ? dr.GetDecimal(dr.GetOrdinal("Fee")) : 0,
                                IsAdd = !dr.IsDBNull(dr.GetOrdinal("IsAdd")) ? dr.GetByte(dr.GetOrdinal("IsAdd")) == 1 ? true : false : false,
                                ProjectName = dr["ServiceName"].ToString(),
                                ServiceDetail = dr["ServiceDetail"].ToString()
                            };

                            model.Traveller.Travellers.Add(traveller);
                        }
                    }
                    #endregion
                }
            }
            return model;
        }

        /// <summary>
        /// 组团端添加一个询价
        /// </summary>
        /// <param name="LineInquireQuoteInfo"></param>
        /// <returns></returns>
        public bool AddInquire(EyouSoft.Model.TourStructure.LineInquireQuoteInfo LineInquireQuoteInfo)
        {
            DbCommand dc = this.DB.GetStoredProcCommand("proc_Inquire_InsertInquire");
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, LineInquireQuoteInfo.CompanyId);
            this.DB.AddInParameter(dc, "RouteId", DbType.Int32, LineInquireQuoteInfo.RouteId);
            this.DB.AddInParameter(dc, "RouteName", DbType.String, LineInquireQuoteInfo.RouteName);
            this.DB.AddInParameter(dc, "CustomerName", DbType.String, LineInquireQuoteInfo.CustomerName);
            this.DB.AddInParameter(dc, "LeaveDate", DbType.DateTime, LineInquireQuoteInfo.LeaveDate);
            this.DB.AddInParameter(dc, "CustomerId", DbType.Int32, LineInquireQuoteInfo.CustomerId);
            this.DB.AddInParameter(dc, "ContactTel", DbType.String, LineInquireQuoteInfo.ContactTel);
            this.DB.AddInParameter(dc, "ContactName", DbType.String, LineInquireQuoteInfo.ContactName);
            this.DB.AddInParameter(dc, "AdultNumber", DbType.Int32, LineInquireQuoteInfo.PeopleNum);
            this.DB.AddInParameter(dc, "ChildNumber", DbType.Int32, LineInquireQuoteInfo.ChildNumber);
            this.DB.AddInParameter(dc, "SpecialClaim", DbType.String, LineInquireQuoteInfo.SpecialClaim);
            this.DB.AddInParameter(dc, "Remark", DbType.String, LineInquireQuoteInfo.Remark);
            this.DB.AddInParameter(dc, "QuoteState", DbType.Byte, (int)LineInquireQuoteInfo.QuoteState);
            this.DB.AddInParameter(dc, "XingCheng", DbType.String, CreateXingChengAskXML(LineInquireQuoteInfo.XingCheng));
            this.DB.AddInParameter(dc, "ASK", DbType.String, CreateQuoteAskXML(LineInquireQuoteInfo.Requirements));
            this.DB.AddOutParameter(dc, "Result", DbType.Int32, 4);

            if (LineInquireQuoteInfo.Traveller != null)
            {
                this.DB.AddInParameter(dc, "TravellerDisplayType", DbType.Byte, LineInquireQuoteInfo.Traveller.TravellerDisplayType);
                this.DB.AddInParameter(dc, "TravellerFilePath", DbType.String, LineInquireQuoteInfo.Traveller.TravellerFilePath);
                this.DB.AddInParameter(dc, "Travellers", DbType.String, this.CreateQuoteTravellersXML(LineInquireQuoteInfo.Traveller.Travellers));
            }
            else
            {
                this.DB.AddInParameter(dc, "TravellerDisplayType", DbType.Byte, EyouSoft.Model.EnumType.TourStructure.CustomerDisplayType.None);
                this.DB.AddInParameter(dc, "TravellerFilePath", DbType.String, DBNull.Value);
                this.DB.AddInParameter(dc, "Travellers", DbType.String, DBNull.Value);
            }

            DbHelper.RunProcedure(dc, DB);
            object Result = this.DB.GetParameterValue(dc, "Result");
            return int.Parse(Result.ToString()) > 0 ? true : false;
        }

        /// <summary>
        /// 修改一个询价
        /// </summary>
        /// <param name="LineInquireQuoteInfo"></param>
        /// <returns></returns>
        public bool UpdateInquire(EyouSoft.Model.TourStructure.LineInquireQuoteInfo LineInquireQuoteInfo)
        {
            DbCommand dc = this.DB.GetStoredProcCommand("proc_Inquire_UpdateInquire");
            this.DB.AddInParameter(dc, "Id", DbType.Int32, LineInquireQuoteInfo.Id);
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, LineInquireQuoteInfo.CompanyId);
            this.DB.AddInParameter(dc, "RouteId", DbType.Int32, LineInquireQuoteInfo.RouteId);
            this.DB.AddInParameter(dc, "CustomerName", DbType.String, LineInquireQuoteInfo.CustomerName);
            this.DB.AddInParameter(dc, "LeaveDate", DbType.DateTime, LineInquireQuoteInfo.LeaveDate);
            this.DB.AddInParameter(dc, "RouteName", DbType.String, LineInquireQuoteInfo.RouteName);
            this.DB.AddInParameter(dc, "CustomerId", DbType.Int32, LineInquireQuoteInfo.CustomerId);
            this.DB.AddInParameter(dc, "ContactTel", DbType.String, LineInquireQuoteInfo.ContactTel);
            this.DB.AddInParameter(dc, "ContactName", DbType.String, LineInquireQuoteInfo.ContactName);
            this.DB.AddInParameter(dc, "AdultNumber", DbType.Int32, LineInquireQuoteInfo.PeopleNum);
            this.DB.AddInParameter(dc, "ChildNumber", DbType.Int32, LineInquireQuoteInfo.ChildNumber);
            this.DB.AddInParameter(dc, "SpecialClaim", DbType.String, LineInquireQuoteInfo.SpecialClaim);
            this.DB.AddInParameter(dc, "Remark", DbType.String, LineInquireQuoteInfo.Remark);
            this.DB.AddInParameter(dc, "QuoteState", DbType.Byte, (int)LineInquireQuoteInfo.QuoteState);
            this.DB.AddInParameter(dc, "XingCheng", DbType.String, CreateXingChengAskXML(LineInquireQuoteInfo.XingCheng));
            this.DB.AddInParameter(dc, "QuoteInfo", DbType.String, CreateQuoteListXML(LineInquireQuoteInfo.Services));
            this.DB.AddInParameter(dc, "ASK", DbType.String, CreateQuoteAskXML(LineInquireQuoteInfo.Requirements));
            this.DB.AddOutParameter(dc, "Result", DbType.Int32, 4);
            if (LineInquireQuoteInfo.Traveller != null)
            {
                this.DB.AddInParameter(dc, "TravellerDisplayType", DbType.Byte, LineInquireQuoteInfo.Traveller.TravellerDisplayType);
                this.DB.AddInParameter(dc, "TravellerFilePath", DbType.String, LineInquireQuoteInfo.Traveller.TravellerFilePath);
                this.DB.AddInParameter(dc, "Travellers", DbType.String, this.CreateQuoteTravellersXML(LineInquireQuoteInfo.Traveller.Travellers));
            }
            else
            {
                this.DB.AddInParameter(dc, "TravellerDisplayType", DbType.Byte, EyouSoft.Model.EnumType.TourStructure.CustomerDisplayType.None);
                this.DB.AddInParameter(dc, "TravellerFilePath", DbType.String, DBNull.Value);
                this.DB.AddInParameter(dc, "Travellers", DbType.String, DBNull.Value);
            }
            DbHelper.RunProcedure(dc, DB);
            object Result = this.DB.GetParameterValue(dc, "Result");
            return int.Parse(Result.ToString()) > 0 ? true : false;
        }

        /// <summary>
        /// 专线端生成快速发布版后更新询价信息表计划编号
        /// </summary>
        /// <param name="QuoteId">询价报价编号</param>
        /// <param name="TourId">生成的团队编号</param>
        /// <returns></returns>
        public bool QuoteAddPlanNo(int QuoteId, string TourId, int OperatorId)
        {
            string SQL = String.Format("UPDATE tbl_CustomerQuote SET BuildTourId='{0}',BuileOperatorId={1},QuoteState={3},BuildTime=getDate() WHERE Id={2}", TourId, OperatorId, QuoteId, (int)EyouSoft.Model.EnumType.TourStructure.QuoteState.已成功);
            DbCommand dc = this.SystemStore.GetSqlStringCommand(SQL.ToString());
            return DbHelper.ExecuteSql(dc, this.DB) > 0 ? true : false;
        }

        /// <summary>
        /// 专线端修改报价
        /// </summary>
        /// <param name="LineInquireQuoteInfo"></param>
        /// <returns></returns>
        public bool UpdateQuote(EyouSoft.Model.TourStructure.LineInquireQuoteInfo LineInquireQuoteInfo)
        {
            DbCommand dc = this.DB.GetStoredProcCommand("proc_Inquire_UpdateQuote");
            this.DB.AddInParameter(dc, "CompanyId", DbType.Int32, LineInquireQuoteInfo.CompanyId);
            this.DB.AddInParameter(dc, "Id", DbType.Int32, LineInquireQuoteInfo.Id);
            this.DB.AddInParameter(dc, "RouteId", DbType.Int32, LineInquireQuoteInfo.RouteId);
            this.DB.AddInParameter(dc, "RouteName", DbType.String, LineInquireQuoteInfo.RouteName);
            this.DB.AddInParameter(dc, "ContactTel", DbType.String, LineInquireQuoteInfo.ContactTel);
            this.DB.AddInParameter(dc, "ContactName", DbType.String, LineInquireQuoteInfo.ContactName);
            this.DB.AddInParameter(dc, "LeaveDate", DbType.DateTime, LineInquireQuoteInfo.LeaveDate);
            this.DB.AddInParameter(dc, "AdultNumber", DbType.Int32, LineInquireQuoteInfo.PeopleNum);
            this.DB.AddInParameter(dc, "ChildNumber", DbType.Int32, LineInquireQuoteInfo.ChildNumber);
            this.DB.AddInParameter(dc, "SpecialClaim", DbType.String, LineInquireQuoteInfo.SpecialClaim);
            this.DB.AddInParameter(dc, "Remark", DbType.String, LineInquireQuoteInfo.Remark);

            this.DB.AddInParameter(dc, "TicketAgio", DbType.Decimal, LineInquireQuoteInfo.TicketAgio);
            this.DB.AddInParameter(dc, "QuoteState", DbType.Byte, (int)LineInquireQuoteInfo.QuoteState);
            this.DB.AddInParameter(dc, "XingCheng", DbType.String, CreateXingChengAskXML(LineInquireQuoteInfo.XingCheng));
            this.DB.AddInParameter(dc, "ASK", DbType.String, CreateQuoteAskXML(LineInquireQuoteInfo.Requirements));
            this.DB.AddInParameter(dc, "QuoteInfo", DbType.String, CreateQuoteListXML(LineInquireQuoteInfo.Services));
            this.DB.AddOutParameter(dc, "Result", DbType.Int32, 4);
            this.DB.AddInParameter(dc, "TotalAmount", DbType.Decimal, LineInquireQuoteInfo.TotalAmount);
            if (LineInquireQuoteInfo.Traveller != null)
            {
                this.DB.AddInParameter(dc, "TravellerDisplayType", DbType.Byte, LineInquireQuoteInfo.Traveller.TravellerDisplayType);
                this.DB.AddInParameter(dc, "TravellerFilePath", DbType.String, LineInquireQuoteInfo.Traveller.TravellerFilePath);
                this.DB.AddInParameter(dc, "Travellers", DbType.String, this.CreateQuoteTravellersXML(LineInquireQuoteInfo.Traveller.Travellers));
            }
            else
            {
                this.DB.AddInParameter(dc, "TravellerDisplayType", DbType.Byte, EyouSoft.Model.EnumType.TourStructure.CustomerDisplayType.None);
                this.DB.AddInParameter(dc, "TravellerFilePath", DbType.String, DBNull.Value);
                this.DB.AddInParameter(dc, "Travellers", DbType.String, DBNull.Value);
            }
            DbHelper.RunProcedure(dc, DB);
            object Result = this.DB.GetParameterValue(dc, "Result");
            return int.Parse(Result.ToString()) > 0 ? true : false;
        }

        /// <summary>
        /// 删除询价报价记录
        /// </summary>
        /// <param name="Id">询价报价编号</param>
        /// <param name="CompanyId">专线公司编号</param> 
        /// <returns></returns>
        public bool DelInquire(int CompanyId, string Ids)
        {
            string SQL = String.Format("UPDATE [tbl_CustomerQuote] SET [ISDELETE]=1 WHERE [CompanyId]={0} AND Id in ({1})",CompanyId,Ids);
            DbCommand dc = this.DB.GetSqlStringCommand(SQL);
            return DbHelper.ExecuteSql(dc, this.DB) > 0 ? true : false;
        }

        /// <summary>
        /// 同步组团询价游客信息到团队计划订单
        /// </summary>
        /// <param name="quoteId">组团询价编号</param>
        /// <param name="tourId">生成的团队计划编号</param>
        /// <returns></returns>
        public bool SyncQuoteTravellerToTourTeamOrder(int quoteId, string tourId)
        {
            DbCommand cmd = DB.GetStoredProcCommand("proc_SyncQuoteTravellerToTourTeamOrder");
            DB.AddInParameter(cmd, "QuoteId", DbType.Int32, quoteId);
            DB.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);
            DB.AddOutParameter(cmd, "Result", DbType.Int32, 4);

            int sqlExceptionCode = 0;

            try
            {
                DbHelper.RunProcedure(cmd, DB);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                sqlExceptionCode = 0 - e.Number;
            }

            if (sqlExceptionCode < 0) return false; ;

            return Convert.ToInt32(DB.GetParameterValue(cmd, "Result")) == 1 ? true : false;
        }

        /// <summary>
        /// 获取组团社询价列表合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="renShuHeJi">人数合计</param>
        public void GetInquireListHeJi(int companyId, EyouSoft.Model.TourStructure.LineInquireQuoteSearchInfo searchInfo, out int renShuHeJi)
        {
            renShuHeJi = 0;

            StringBuilder cmdText = new StringBuilder();

            #region SQL
            cmdText.Append(" SELECT SUM(AdultNumber+ChildNumber) AS RenShuHeJi FROM tbl_CustomerQuote ");
            cmdText.AppendFormat(" WHERE CompanyId={0} AND IsDelete='0' ", companyId);
            if (searchInfo != null)
            {
                if (!string.IsNullOrEmpty(searchInfo.TourNo) || searchInfo.DayNum != 0)
                {
                    cmdText.Append(" AND EXISTS(SELECT 1 FROM tbl_Tour AS A WHERE A.TourId=tbl_CustomerQuote.BuildTourId ");

                    if (!string.IsNullOrEmpty(searchInfo.TourNo))
                    {
                        cmdText.AppendFormat(" AND A.TourCode LIKE '%{0}%' ", searchInfo.TourNo);
                    }
                    if (searchInfo.DayNum != 0)
                    {
                        cmdText.AppendFormat(" AND A.TourDays={0} ", searchInfo.DayNum);
                    }

                    cmdText.Append(" ) ");
                }

                if (!String.IsNullOrEmpty(searchInfo.RouteName))
                {
                    cmdText.AppendFormat(" AND RouteName like  '%{0}%'", searchInfo.RouteName);
                }
                if (searchInfo.SDate.HasValue && searchInfo.SDate != DateTime.MinValue)
                {
                    cmdText.AppendFormat(" AND LeaveDate>'{0}' ", searchInfo.SDate.Value.AddDays(-1));
                }
                if (searchInfo.EDate.HasValue && searchInfo.EDate != DateTime.MinValue)
                {
                    cmdText.AppendFormat(" AND LeaveDate<'{0}' ", searchInfo.EDate.Value.AddDays(1));
                }
                if (searchInfo.XunTuanETime.HasValue)
                {
                    cmdText.AppendFormat(" AND IssueTime<'{0}' ", searchInfo.XunTuanETime.Value.AddDays(1));
                }
                if (searchInfo.XunTuanSTime.HasValue)
                {
                    cmdText.AppendFormat(" AND IssueTime>'{0}' ", searchInfo.XunTuanSTime.Value);
                }
            }
            #endregion

            DbCommand cmd = DB.GetSqlStringCommand(cmdText.ToString());

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, DB))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) renShuHeJi = rdr.GetInt32(0);
                }
            }
        }
        #endregion

    }
}
