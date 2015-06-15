using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EyouSoft.Model.EnumType;
using EyouSoft.Model.PlanStructure;
using EyouSoft.Model.TourStructure;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using EyouSoft.Toolkit;
using System.Data;
using System.Data.Common;
using System.Xml.Linq;


namespace EyouSoft.DAL.PlanStruture
{
    /// <summary>
    /// 机票相关操作方法
    /// autor:李焕超 datetime:2011-1-19
    /// </summary>
    public class PlaneTicket : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.PlanStruture.IPlaneTicket
    {
        #region static constants
        //static constants
        /// <summary>
        /// 根据退票编号获取退票航段信息集合
        /// </summary>
        private const string SQL_SELECT_GetRefundTicketFlights = @"SELECT B.* FROM tbl_CustomerRefundFlight AS A INNER JOIN tbl_PlanTicketFlight AS B
ON A.FlightId=B.Id
WHERE A.RefundId=@RefundId";
        /// <summary>
        /// 根据退票编号获取出票时间
        /// </summary>
        private const string SQL_SELECT_GetTicketOutTimeByReturnTicketId = "SELECT C.[TicketOutTime],C.[Id] AS TicketOutId FROM [tbl_PlanTicketOut] AS C WHERE C.[Id] IN(SELECT B.[TicketId] FROM [tbl_CustomerRefundFlight] AS A INNER JOIN [tbl_PlanTicketFlight] AS B ON A.[FlightId]=B.[Id] WHERE A.[RefundId]=@ReturnId)";
        #endregion

        #region constructor
        private readonly Database _db = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        public PlaneTicket()
        {
            _db = this.SystemStore;
        }
        #endregion

        #region private members
        /// <summary>
        /// 机票管理列表
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyId"></param>
        /// <param name="strWhere"></param>
        /// <param name="field"></param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        private IList<TicketInfo> GetList(int PageSize, int PageIndex, int CompanyId, string strWhere, string field, ref int RecordCount, ref int PageCount)
        {
            List<EyouSoft.Model.PlanStructure.TicketInfo> TicketInfoList = new List<EyouSoft.Model.PlanStructure.TicketInfo>();
            string cmdQuery = string.Format(" companyId={0} {1}", CompanyId, strWhere);
            string tableName = "tbl_PlanTicket";
            string primaryKey = "Id";
            using (IDataReader rdr = EyouSoft.Toolkit.DAL.DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, tableName, primaryKey, field, cmdQuery, "State asc,RegisterTime desc"))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.PlanStructure.TicketInfo ticketModel = new TicketInfo();
                    ticketModel.Id = rdr.GetInt32(rdr.GetOrdinal("Id")); ;
                    ticketModel.Saler = rdr.IsDBNull(rdr.GetOrdinal("Saler")) ? "" : rdr.GetString(rdr.GetOrdinal("Saler"));
                    ticketModel.RefundId = rdr.GetString(rdr.GetOrdinal("RefundId"));
                    ticketModel.RegisterTime = rdr.IsDBNull(rdr.GetOrdinal("LeaveDate")) ? DateTime.Now : rdr.GetDateTime(rdr.GetOrdinal("LeaveDate"));
                    ticketModel.RouteName = rdr.IsDBNull(rdr.GetOrdinal("RouteName")) ? "" : rdr.GetString(rdr.GetOrdinal("RouteName"));// rdr.GetOrdinal("CommissionType");
                    ticketModel.State = rdr.IsDBNull(rdr.GetOrdinal("State")) ? (EyouSoft.Model.EnumType.PlanStructure.TicketState.机票申请) : (EyouSoft.Model.EnumType.PlanStructure.TicketState)Enum.Parse(typeof(EyouSoft.Model.EnumType.PlanStructure.TicketState), rdr.GetByte(rdr.GetOrdinal("State")).ToString()); ;
                    ticketModel.TicketType = rdr.IsDBNull(rdr.GetOrdinal("TicketType")) ? EyouSoft.Model.EnumType.PlanStructure.TicketType.订单申请机票 : (EyouSoft.Model.EnumType.PlanStructure.TicketType)Enum.Parse(typeof(EyouSoft.Model.EnumType.PlanStructure.TicketType), rdr.GetByte(rdr.GetOrdinal("TicketType")).ToString());
                    ticketModel.TourId = rdr.GetString(rdr.GetOrdinal("TourId"));
                    ticketModel.TourNum = rdr.IsDBNull(rdr.GetOrdinal("tourcode")) ? "" : rdr.GetString(rdr.GetOrdinal("tourcode"));
                    ticketModel.TicketFlights = this.GetFlegMent(rdr.IsDBNull(rdr.GetOrdinal("FligthSegment")) ? "" : rdr.GetString(rdr.GetOrdinal("FligthSegment")));
                    ticketModel.Operator = rdr.IsDBNull(rdr.GetOrdinal("Operator")) ? "" : rdr.GetString(rdr.GetOrdinal("Operator"));
                    // 出票时间 add by zhengzy 2011-10-28
                    if (!rdr.IsDBNull(rdr.GetOrdinal("TicketOutTime")))
                    {
                        ticketModel.TicketOutTime = rdr.GetDateTime(rdr.GetOrdinal("TicketOutTime"));
                    }
                    if (ticketModel.TicketType == EyouSoft.Model.EnumType.PlanStructure.TicketType.订单退票)
                    {
                        ticketModel.TicketFlights = this.GetReturnTicketFlights(ticketModel.RefundId);
                        ticketModel.TicketOutTime = GetTicketOutTimeByReturnTicketId(ticketModel.RefundId);
                    }
                    //查找销售
                    string tourid = ticketModel.TourId;
                    EyouSoft.DAL.TourStructure.TourOrder torder = new EyouSoft.DAL.TourStructure.TourOrder();
                    IList<EyouSoft.Model.StatisticStructure.StatisticOperator> salerList = new List<EyouSoft.Model.StatisticStructure.StatisticOperator>();
                    salerList = torder.GetSalerInfo(tourid);
                    torder = new EyouSoft.DAL.TourStructure.TourOrder();
                    string strSaler = "";
                    if (salerList != null && salerList.Count > 0)
                    {
                        foreach (EyouSoft.Model.StatisticStructure.StatisticOperator s in salerList)
                        {
                            strSaler += s.OperatorName + "，";
                        }
                    }
                    ticketModel.Saler = strSaler.TrimEnd(new char[] { '，' });
                    if (!rdr.IsDBNull(rdr.GetOrdinal("VerifyTime")))
                        ticketModel.VerifyTime = rdr.GetDateTime(rdr.GetOrdinal("VerifyTime"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("OrderInfo")))
                        this.GetOrderInfoByXml(rdr.GetString(rdr.GetOrdinal("OrderInfo")), ref ticketModel);

                    TicketInfoList.Add(ticketModel);
                }
            }
            PageCount = RecordCount / PageSize + 1;
            return TicketInfoList;

        }

        /// <summary>
        /// 创建机票游客修改信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateTicketTravellerXML(IList<EyouSoft.Model.TourStructure.TourOrderCustomer> items)
        {
            //XML:<ROOT><Info VisitorName="游客姓名" CradNumber="证件号码" CradType="证件类型" TravellerId="游客编号" /></ROOT>
            if (items == null || items.Count < 1) return string.Empty;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                xmlDoc.AppendFormat("<Info  VisitorName=\"{0}\" CradNumber=\"{1}\" CradType=\"{2}\" TravellerId=\"{3}\"  />", item.VisitorName
                    , item.CradNumber
                    , (int)item.CradType
                    , item.ID);
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建机票申请与游客对应关系信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateTicketTravellerRelationXML(IList<EyouSoft.Model.PlanStructure.TicketOutCustomerInfo> items)
        {
            //XML:<ROOT><Info TravellerId="游客编号" /></ROOT>
            if (items == null || items.Count < 1) return string.Empty;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                xmlDoc.AppendFormat("<Info TravellerId=\"{0}\" />", item.UserId);
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建机票申请航段信息XMLDocument
        /// </summary>
        /// <param name="itmes"></param>
        /// <returns></returns>
        private string CreateTicketFlightXML(IList<EyouSoft.Model.PlanStructure.TicketFlight> items)
        {
            //XML:<ROOT><Info FligthSegment="航段" DepartureTime="出港时间" AireLine="航空公司" Discount="折扣" TicketTime="时间" /></ROOT>

            if (items == null || items.Count < 1) return string.Empty;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                xmlDoc.AppendFormat("<Info FligthSegment=\"{0}\" DepartureTime=\"{1}\" AireLine=\"{2}\" Discount=\"{3}\" TicketTime=\"{4}\"  />", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(item.FligthSegment)
                    , item.DepartureTime
                    , (int)item.AireLine
                    , item.Discount
                    , item.TicketTime);
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建机票申请票款信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateTicketFundXML(IList<EyouSoft.Model.PlanStructure.TicketKindInfo> items)
        {
            //XML:XML:<ROOT><Info Price="票面价" OilFee="税/机建" PeopleCount="人数" AgencyPrice="代理费" TotalMoney="票款" TicketType="票种" Discount="百分比" OtherPrice="其它费用" /></ROOT>
            if (items == null || items.Count < 1) return string.Empty;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                xmlDoc.AppendFormat("<Info Price=\"{0}\" OilFee=\"{1}\" PeopleCount=\"{2}\" AgencyPrice=\"{3}\" TotalMoney=\"{4}\" TicketType=\"{5}\" Discount=\"{6}\" OtherPrice=\"{7}\" />", item.Price
                    , item.OilFee
                    , item.PeopleCount
                    , item.AgencyPrice
                    , item.TotalMoney
                    , (byte)item.TicketType
                    , item.Discount
                    , item.OtherPrice);
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 返回机票航班列表
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        private IList<TicketFlight> GetFlightList(string TicketId)
        {
            IList<TicketFlight> FlightList = new List<TicketFlight>();
            TicketFlight model = null;
            StringBuilder sql = new StringBuilder();
            sql.Append(" select * from tbl_PlanTicketFlight where TicketId=@TicketId");
            DbCommand cmd = this._db.GetSqlStringCommand(sql.ToString());
            this._db.AddInParameter(cmd, "TicketId", DbType.StringFixedLength, TicketId);
            using (IDataReader rd = DbHelper.RunReaderProcedure(cmd, this._db))
            {
                while (rd.Read())
                {
                    model = new TicketFlight();
                    if (!rd.IsDBNull(rd.GetOrdinal("AireLine")))
                        model.AireLine = (Model.EnumType.PlanStructure.FlightCompany)rd.GetByte(rd.GetOrdinal("AireLine"));
                    model.DepartureTime = rd.GetDateTime(rd.GetOrdinal("DepartureTime"));
                    model.Discount = rd.GetDecimal(rd.GetOrdinal("Discount"));
                    model.FligthSegment = rd.IsDBNull(rd.GetOrdinal("FligthSegment")) ? "" : rd.GetString(rd.GetOrdinal("FligthSegment"));
                    model.TicketId = rd.GetString(rd.GetOrdinal("TicketId"));
                    model.ID = rd.GetInt32(rd.GetOrdinal("ID"));
                    model.TicketTime = rd.IsDBNull(rd.GetOrdinal("TicketTime")) ? "" : rd.GetString(rd.GetOrdinal("TicketTime"));
                    FlightList.Add(model);

                }
            }
            return FlightList;
        }

        /// <summary>
        /// 返回票款列表
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        private IList<TicketKindInfo> GetKindList(string TicketId)
        {
            IList<TicketKindInfo> kindList = new List<TicketKindInfo>();
            TicketKindInfo model = null;
            StringBuilder sql = new StringBuilder();
            sql.Append(" select * from tbl_PlanTicketKind where TicketId=@TicketId");
            DbCommand cmd = this._db.GetSqlStringCommand(sql.ToString());
            this._db.AddInParameter(cmd, "TicketId", DbType.StringFixedLength, TicketId);
            using (IDataReader rd = DbHelper.RunReaderProcedure(cmd, this._db))
            {
                while (rd.Read())
                {
                    model = new TicketKindInfo();
                    model.AgencyPrice = rd.GetDecimal(rd.GetOrdinal("AgencyPrice"));
                    model.ID = rd.GetInt32(rd.GetOrdinal("ID"));
                    model.OilFee = rd.GetDecimal(rd.GetOrdinal("OilFee"));
                    model.PeopleCount = rd.GetInt32(rd.GetOrdinal("PeopleCount"));
                    model.Price = rd.GetDecimal(rd.GetOrdinal("Price"));
                    model.TicketType = rd.IsDBNull(rd.GetOrdinal("TicketType")) ? (EyouSoft.Model.EnumType.PlanStructure.KindType)Enum.Parse(typeof(EyouSoft.Model.EnumType.PlanStructure.KindType), "0") : (EyouSoft.Model.EnumType.PlanStructure.KindType)Enum.Parse(typeof(EyouSoft.Model.EnumType.PlanStructure.KindType), rd.GetByte(rd.GetOrdinal("TicketType")).ToString());
                    model.TotalMoney = rd.GetDecimal(rd.GetOrdinal("TotalMoney"));
                    model.OtherPrice = rd.GetDecimal(rd.GetOrdinal("OtherPrice"));
                    model.Discount = rd.GetDecimal(rd.GetOrdinal("Discount"));
                    kindList.Add(model);
                }
            }
            return kindList;
        }

        /// <summary>
        /// 返回机票游客列表
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        private IList<TourOrderCustomer> GetCustomerList(string TicketId)
        {

            IList<TourOrderCustomer> kindList = new List<TourOrderCustomer>();
            TourOrderCustomer model = null;
            StringBuilder sql = new StringBuilder();
            sql.Append(" select b.* from tbl_PlanTicketOutCustomer a join tbl_TourOrderCustomer b on a.UserId = b.id where a.TicketOutId=@TicketId");
            DbCommand cmd = this._db.GetSqlStringCommand(sql.ToString());
            this._db.AddInParameter(cmd, "TicketId", DbType.StringFixedLength, TicketId);
            using (IDataReader rd = DbHelper.RunReaderProcedure(cmd, this._db))
            {
                while (rd.Read())
                {
                    model = new TourOrderCustomer();
                    model.ID = rd.GetString(rd.GetOrdinal("id"));
                    model.CradNumber = rd.IsDBNull(rd.GetOrdinal("CradNumber")) ? "" : rd.GetString(rd.GetOrdinal("CradNumber"));
                    model.CradType = rd.IsDBNull(rd.GetOrdinal("CradType")) ? (EyouSoft.Model.EnumType.TourStructure.CradType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.CradType), "0") : (EyouSoft.Model.EnumType.TourStructure.CradType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.CradType), rd.GetByte(rd.GetOrdinal("CradType")).ToString());
                    model.VisitorName = rd.IsDBNull(rd.GetOrdinal("VisitorName")) ? "" : rd.GetString(rd.GetOrdinal("VisitorName"));
                    kindList.Add(model);
                }
            }
            return kindList;
        }

        /// <summary>
        /// 返回退订机票游客列表
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        private IList<TourOrderCustomer> GetRefundCustomerList(string TicketId)
        {

            IList<TourOrderCustomer> kindList = new List<TourOrderCustomer>();
            TourOrderCustomer model = null;
            StringBuilder sql = new StringBuilder();
            sql.Append(" select b.* from tbl_PlanTicketOutCustomer a join tbl_TourOrderCustomer b on a.UserId = b.id join tbl_CustomerRefund c on b.id = c.CustormerId  where a.TicketOutId=@TicketId");
            DbCommand cmd = this._db.GetSqlStringCommand(sql.ToString());
            this._db.AddInParameter(cmd, "TicketId", DbType.StringFixedLength, TicketId);
            using (IDataReader rd = DbHelper.RunReaderProcedure(cmd, this._db))
            {
                while (rd.Read())
                {
                    model = new TourOrderCustomer();
                    model.ID = rd.GetString(rd.GetOrdinal("id"));
                    model.CradNumber = rd.IsDBNull(rd.GetOrdinal("CradNumber")) ? "" : rd.GetString(rd.GetOrdinal("CradNumber"));
                    model.CradType = rd.IsDBNull(rd.GetOrdinal("CradType")) ? (EyouSoft.Model.EnumType.TourStructure.CradType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.CradType), "0") : (EyouSoft.Model.EnumType.TourStructure.CradType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.CradType), rd.GetByte(rd.GetOrdinal("CradType")).ToString());
                    model.VisitorName = rd.IsDBNull(rd.GetOrdinal("VisitorName")) ? "" : rd.GetString(rd.GetOrdinal("VisitorName"));
                    kindList.Add(model);
                }
            }
            return kindList;
        }

        /// <summary>
        /// xml转化成实体
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private EyouSoft.Model.PlanStructure.TicketFlight TicketFlightXMLToModel(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return null;
            EyouSoft.Model.PlanStructure.TicketFlight model = new TicketFlight();
            XElement x = XElement.Parse(xml);
            XElement xChild = EyouSoft.Toolkit.Utils.GetXElement(x, "c");
            model.ID = string.IsNullOrEmpty(EyouSoft.Toolkit.Utils.GetXAttributeValue(xChild, "ID")) ? 0 : int.Parse(EyouSoft.Toolkit.Utils.GetXAttributeValue(xChild, "ID"));
            model.AireLine = (Model.EnumType.PlanStructure.FlightCompany)EyouSoft.Toolkit.Utils.GetInt(EyouSoft.Toolkit.Utils.GetXAttributeValue(xChild, "AireLine"));
            model.DepartureTime = string.IsNullOrEmpty(EyouSoft.Toolkit.Utils.GetXAttributeValue(xChild, "DepartureTime")) ? DateTime.Parse("2011-02-14") : DateTime.Parse(EyouSoft.Toolkit.Utils.GetXAttributeValue(xChild, "DepartureTime"));
            model.Discount = string.IsNullOrEmpty(EyouSoft.Toolkit.Utils.GetXAttributeValue(xChild, "Discount")) ? 0 : decimal.Parse(EyouSoft.Toolkit.Utils.GetXAttributeValue(xChild, "Discount"));
            model.FligthSegment = EyouSoft.Toolkit.Utils.GetXAttributeValue(xChild, "FligthSegment");
            model.TicketTime = EyouSoft.Toolkit.Utils.GetXAttributeValue(xChild, "TicketTime");
            return model;
        }

        /// <summary>
        /// xml转化成实体
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.PlanStructure.TicketKindInfo> TicketKindXMLToModelList(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return null;
            EyouSoft.Model.PlanStructure.TicketKindInfo model = null;
            IList<EyouSoft.Model.PlanStructure.TicketKindInfo> kindList = new List<EyouSoft.Model.PlanStructure.TicketKindInfo>();
            XElement x = XElement.Parse(xml);
            IEnumerable<XElement> xChildList = EyouSoft.Toolkit.Utils.GetXElements(x, "w");
            foreach (XElement xChild in xChildList)
            {
                model = new TicketKindInfo();
                model.TicketType = string.IsNullOrEmpty(EyouSoft.Toolkit.Utils.GetXAttributeValue(xChild, "TicketType")) ? EyouSoft.Model.EnumType.PlanStructure.KindType.成人 : (EyouSoft.Model.EnumType.PlanStructure.KindType)Enum.Parse(typeof(EyouSoft.Model.EnumType.PlanStructure.KindType), byte.Parse(EyouSoft.Toolkit.Utils.GetXAttributeValue(xChild, "TicketType")).ToString());
                model.AgencyPrice = string.IsNullOrEmpty(EyouSoft.Toolkit.Utils.GetXAttributeValue(xChild, "AgencyPrice")) ? 0 : decimal.Parse(EyouSoft.Toolkit.Utils.GetXAttributeValue(xChild, "AgencyPrice"));
                model.PeopleCount = string.IsNullOrEmpty(EyouSoft.Toolkit.Utils.GetXAttributeValue(xChild, "PeopleCount")) ? 0 : int.Parse(EyouSoft.Toolkit.Utils.GetXAttributeValue(xChild, "PeopleCount"));
                model.TotalMoney = string.IsNullOrEmpty(EyouSoft.Toolkit.Utils.GetXAttributeValue(xChild, "TotalMoney")) ? 0 : decimal.Parse(EyouSoft.Toolkit.Utils.GetXAttributeValue(xChild, "TotalMoney"));

                kindList.Add(model);
            }

            return kindList;
        }

        /// <summary>
        /// 数组转化成字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string ArrayToString(int[] str)
        {
            StringBuilder st = new StringBuilder();
            foreach (int s in str)
                st.AppendFormat("{0},", s);

            return st.ToString().TrimEnd(new char[] { ',' });
        }

        /// <summary>
        /// 根据退票信息Id获取退票的航段信息集合
        /// </summary>
        /// <param name="CustomerRefundId">退票信息Id</param>
        /// <returns>航段信息集合</returns>
        private IList<Model.PlanStructure.TicketFlight> GetReturnTicketFlights(string CustomerRefundId)
        {
            if (string.IsNullOrEmpty(CustomerRefundId))
                return null;

            IList<Model.PlanStructure.TicketFlight> list = new List<Model.PlanStructure.TicketFlight>();
            Model.PlanStructure.TicketFlight model = null;
            string strSql = " select ID,FligthSegment,DepartureTime,AireLine,Discount,TicketId,TicketTime from tbl_PlanTicketFlight where ID in (select FlightId from tbl_CustomerRefundFlight where RefundId = @RefundId) ";
            DbCommand dc = this._db.GetSqlStringCommand(strSql);
            this._db.AddInParameter(dc, "RefundId", DbType.AnsiStringFixedLength, CustomerRefundId);

            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                while (dr.Read())
                {
                    model = new TicketFlight();
                    if (!dr.IsDBNull(0))
                        model.ID = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        model.FligthSegment = dr.GetString(1);
                    if (!dr.IsDBNull(2))
                        model.DepartureTime = dr.GetDateTime(2);
                    if (!dr.IsDBNull(3))
                        model.AireLine = (EyouSoft.Model.EnumType.PlanStructure.FlightCompany)dr.GetByte(3);
                    if (!dr.IsDBNull(4))
                        model.Discount = dr.GetDecimal(4);
                    if (!dr.IsDBNull(5))
                        model.TicketId = dr.GetString(5);
                    if (!dr.IsDBNull(6))
                        model.TicketTime = dr.GetString(6);

                    list.Add(model);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取退票航段
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        private IList<Model.PlanStructure.TicketFlight> GetFlegMent(string FligthSegmentXML)
        {
            if (string.IsNullOrEmpty(FligthSegmentXML))
                return null;

            IList<Model.PlanStructure.TicketFlight> list = null;
            Model.PlanStructure.TicketFlight model = null;

            XElement xRoot = XElement.Parse(FligthSegmentXML);
            if (xRoot == null)
                return list;

            var xRows = Utils.GetXElements(xRoot, "row");
            if (xRows == null || xRows.Count() <= 0)
                return null;

            list = new List<Model.PlanStructure.TicketFlight>();
            foreach (var t in xRows)
            {
                if (t == null)
                    continue;

                model = new TicketFlight();

                model.ID = Utils.GetInt(Utils.GetXAttributeValue(t, "ID"));
                model.FligthSegment = Utils.GetXAttributeValue(t, "FligthSegment");
                model.DepartureTime = Utils.GetDateTime(Utils.GetXAttributeValue(t, "DepartureTime"));
                model.AireLine = (EyouSoft.Model.EnumType.PlanStructure.FlightCompany)Utils.GetInt(Utils.GetXAttributeValue(t, "AireLine"));
                model.Discount = Utils.GetDecimal(Utils.GetXAttributeValue(t, "Discount"));
                model.TicketId = Utils.GetXAttributeValue(t, "TicketId");
                model.TicketTime = Utils.GetXAttributeValue(t, "TicketTime");

                list.Add(model);
            }

            return list;
        }

        /// <summary>
        /// 根据Id数组生成半角逗号分割的Id字符串
        /// </summary>
        /// <param name="IdArr">Id数组</param>
        /// <returns>半角逗号分割的Id字符串</returns>
        private string GetIdsByIdArr(int[] IdArr)
        {
            if (IdArr == null || IdArr.Length <= 0)
                return string.Empty;

            string strIds = string.Empty;
            foreach (int i in IdArr)
            {
                if (i <= 0)
                    continue;

                strIds += i.ToString() + ",";
            }
            strIds = strIds.Trim(',');

            return strIds;
        }

        /// <summary>
        /// 根据机票款项信息FOR XML RAW,ROOT('root')字符串获取机票款项信息集合
        /// </summary>
        /// <param name="xml">[tbl_PlanTicketKind] FOR XML RAW,ROOT('root')字符串</param>
        /// <returns></returns>
        private IList<TicketKindInfo> ParseTicketFundsByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<TicketKindInfo> items = new List<TicketKindInfo>();

            XElement xRoot = XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");

            foreach (var xRow in xRows)
            {
                TicketKindInfo item = new TicketKindInfo();
                item.AgencyPrice = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "AgencyPrice"));
                item.Discount = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "Discount"));
                item.ID = Utils.GetInt(Utils.GetXAttributeValue(xRow, "ID"));
                item.OilFee = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "OilFee"));
                item.OtherPrice = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "OtherPrice"));
                item.PeopleCount = Utils.GetInt(Utils.GetXAttributeValue(xRow, "PeopleCount"));
                item.Price = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "Price"));
                item.TicketId = Utils.GetXAttributeValue(xRow, "TicketId");
                item.TicketType = (EyouSoft.Model.EnumType.PlanStructure.KindType)Utils.GetInt(Utils.GetXAttributeValue(xRow, "TicketType"));
                item.TotalMoney = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "TotalMoney"));

                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// 根据机票申请航段信息FOR XML RAW,ROOT('root')字符串获取机票申请航段信息集合
        /// </summary>
        /// <param name="xml">[tbl_PlanTicketFlight] FOR XML RAW,ROOT('root')字符串</param>
        /// <returns></returns>
        private IList<TicketFlight> ParseTicketFlightsByXml(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<TicketFlight> items = new List<TicketFlight>();

            XElement xRoot = XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");
            foreach (var xRow in xRows)
            {
                TicketFlight item = new TicketFlight();
                item.AireLine = (EyouSoft.Model.EnumType.PlanStructure.FlightCompany)Utils.GetInt(Utils.GetXAttributeValue(xRow, "AireLine"));
                item.DepartureTime = Utils.GetDateTime(Utils.GetXAttributeValue(xRow, "DepartureTime"));
                item.Discount = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "Discount"));
                item.FligthSegment = Utils.GetXAttributeValue(xRow, "FligthSegment");
                item.ID = Utils.GetInt(Utils.GetXAttributeValue(xRow, "ID"));
                item.TicketId = Utils.GetXAttributeValue(xRow, "TicketId");
                item.TicketTime = Utils.GetXAttributeValue(xRow, "TicketTime");
                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// 根据退票编号获取出票时间
        /// </summary>
        /// <param name="returnTicketId">退票编号</param>
        /// <returns></returns>
        private DateTime GetTicketOutTimeByReturnTicketId(string returnTicketId)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_GetTicketOutTimeByReturnTicketId);
            _db.AddInParameter(cmd, "ReturnId", DbType.AnsiStringFixedLength, returnTicketId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) return rdr.GetDateTime(0);
                }
            }

            return DateTime.Now;
        }

        /// <summary>
        /// 获取机票列表（包括审核和出票列表）的订单信息
        /// </summary>
        /// <param name="strXml">订单信息SqlXml</param>
        /// <param name="model">机票列表实体</param>
        private void GetOrderInfoByXml(string strXml, ref TicketInfo model)
        {
            if (string.IsNullOrEmpty(strXml) || model == null)
                return;

            var xRoot = XElement.Parse(strXml);
            var xRows = Utils.GetXElements(xRoot, "row");
            if (xRows == null || !xRows.Any()) return;
            foreach (var t in xRows)
            {
                if (t == null) continue;

                model.OrderNo = Utils.GetXAttributeValue(t, "OrderNo");
                model.BuyCompanyId = Utils.GetInt(Utils.GetXAttributeValue(t, "BuyCompanyID"));
                model.BuyCompanyName = Utils.GetXAttributeValue(t, "BuyCompanyName");

                break;
            }
        }

        #endregion

        #region public members
        /// <summary>
        /// 出票/修改机票
        /// </summary>
        /// <param name="TicketModel"></param>
        /// <param name="registerId">完成出票机票款自动结清付款登记编号 为空时未做付款登记</param>
        /// <returns></returns>
        public bool ToTicketOut(TicketOutListInfo model, out string registerId)
        {
            registerId = string.Empty;

            DbCommand dc = this._db.GetStoredProcCommand("proc_TicketOut_Update");
            this._db.AddInParameter(dc, "TicketId", DbType.AnsiStringFixedLength, model.TicketOutId);
            this._db.AddInParameter(dc, "TourId", DbType.AnsiStringFixedLength, model.TourId);
            this._db.AddInParameter(dc, "TicketType", DbType.Int16, model.TicketType);
            this._db.AddInParameter(dc, "RouteName", DbType.String, model.RouteName);
            this._db.AddInParameter(dc, "PNR", DbType.String, model.PNR);
            this._db.AddInParameter(dc, "Total", DbType.Decimal, model.Total);
            this._db.AddInParameter(dc, "Notice", DbType.String, model.Notice);
            this._db.AddInParameter(dc, "TicketOffice", DbType.String, model.TicketOffice);
            this._db.AddInParameter(dc, "TicketOfficeId", DbType.Int32, model.TicketOfficeId);
            this._db.AddInParameter(dc, "Saler", DbType.String, model.Saler);
            this._db.AddInParameter(dc, "TicketNum", DbType.String, model.TicketNum);
            this._db.AddInParameter(dc, "PayType", DbType.Int32, (int)model.PayType);
            this._db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(dc, "State", DbType.Byte, (byte)model.State);
            this._db.AddInParameter(dc, "Operator", DbType.String, model.Operator);
            this._db.AddInParameter(dc, "OperateID", DbType.Int32, model.OperateID);
            this._db.AddInParameter(dc, "CompanyID", DbType.Int32, model.CompanyID);
            if (model.TicketOutTime.HasValue)
            {
                this._db.AddInParameter(dc, "TicketOutTime", DbType.DateTime, model.TicketOutTime.Value);
            }
            else
            {
                this._db.AddInParameter(dc, "TicketOutTime", DbType.DateTime, DBNull.Value);
            }
            this._db.AddInParameter(dc, "CustomerXML", DbType.String, this.CreateTicketTravellerXML(model.CustomerInfoList));
            this._db.AddInParameter(dc, "OutCustomerXML", DbType.String, this.CreateTicketTravellerRelationXML(model.TicketOutCustomerInfoList));
            this._db.AddInParameter(dc, "FlightListXML", DbType.String, this.CreateTicketFlightXML(model.TicketFlightList));
            this._db.AddInParameter(dc, "TicketKindXML", DbType.String, this.CreateTicketFundXML(model.TicketKindInfoList));
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            this._db.AddOutParameter(dc, "TicketOutRegisterId", DbType.String, 36);
            this._db.AddInParameter(dc, "OrderId", DbType.AnsiStringFixedLength, model.OrderId);
            DbHelper.RunProcedure(dc, this._db);
            object ob = this._db.GetParameterValue(dc, "Result");

            if (int.Parse(ob.ToString()) > 0)
            {
                registerId = this._db.GetParameterValue(dc, "TicketOutRegisterId").ToString();
                return true;
            }

            return false;
        }

        /*
        /// <summary>
        /// 散拼计划-申请机票列表
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyID"></param>
        /// <param name="strWhere"></param>
        /// <param name="filedOrder"></param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PlanStructure.TicketListModel> GetTicketList(int CompanyID, string TourId)
        {
            if (CompanyID <= 0 && string.IsNullOrEmpty(TourId))
                return null;
            IList<EyouSoft.Model.PlanStructure.TicketListModel> ticketList = new List<EyouSoft.Model.PlanStructure.TicketListModel>();
            EyouSoft.Model.PlanStructure.TicketListModel model = null;

            StringBuilder StrSql = new StringBuilder();
            StrSql.Append(" select a.id as id,a.remark,a.total,a.state as state, a.pnr,(select * from tbl_PlanTicketKind as w where w.TicketId=a.id for xml auto,root('root')) as ticketKind,(select top 1 * from [tbl_PlanTicketFlight] as c where a.id=c.ticketid  for xml auto,root('root')) as flight");
            StrSql.Append(" from tbl_planticketout a   join view_ticketkind b on a.id=b.ticketid  ");
            #region 搜索
            StrSql.Append(" WHERE 1=1 ");
            if (CompanyID > 0)
                StrSql.AppendFormat(" and a.CompanyId={0} ", CompanyID);
            if (!string.IsNullOrEmpty(TourId))
                StrSql.AppendFormat(" and a.tourid='{0}'", TourId);
            #endregion
            DbCommand dc = this._db.GetSqlStringCommand(StrSql.ToString());
            using (IDataReader rd = DbHelper.ExecuteReader(dc, this._db))
            {
                while (rd.Read())
                {
                    TicketFlight fmodel = new TicketFlight();

                    model = new TicketListModel();
                    model.Id = rd.GetString(rd.GetOrdinal("Id"));
                    string flight = rd.IsDBNull(rd.GetOrdinal("flight")) ? string.Empty : rd.GetString(rd.GetOrdinal("flight"));
                    fmodel = TicketFlightXMLToModel(flight);
                    if (fmodel != null)
                    {
                        model.FligthSegment = fmodel.FligthSegment;
                        model.DepartureTime = fmodel.DepartureTime;
                        model.AireLine = fmodel.AireLine;
                        model.Discount = fmodel.Discount;
                        model.TicketTime = fmodel.TicketTime;
                    }

                    string kind = rd.IsDBNull(rd.GetOrdinal("ticketKind")) ? string.Empty : rd.GetString(rd.GetOrdinal("ticketKind"));
                    IList<TicketKindInfo> kindList = TicketKindXMLToModelList(kind);
                    if (kindList != null && kindList.Count > 0)
                    {
                        foreach (TicketKindInfo kindModel in kindList)
                        {
                            if (kindModel.TicketType == EyouSoft.Model.EnumType.PlanStructure.KindType.儿童)
                            {
                                model.ChidenAgencyMoney = kindModel.AgencyPrice;
                                model.ChidenTicketMoney = kindModel.TotalMoney;
                                model.ChidenCount = kindModel.PeopleCount;
                            }
                            else if (kindModel.TicketType == EyouSoft.Model.EnumType.PlanStructure.KindType.成人)
                            {
                                model.AgencyPrice = kindModel.AgencyPrice;
                                model.TotalMoney = kindModel.TotalMoney;
                                model.PeopleCount = kindModel.PeopleCount;
                            }
                        }
                    }
                    model.PNR = rd.IsDBNull(rd.GetOrdinal("PNR")) ? string.Empty : rd.GetString(rd.GetOrdinal("PNR"));
                    model.TotalAmount = rd.IsDBNull(rd.GetOrdinal("total")) ? 0 : rd.GetDecimal(rd.GetOrdinal("total"));
                    model.Remark = rd.IsDBNull(rd.GetOrdinal("Remark")) ? string.Empty : rd.GetString(rd.GetOrdinal("Remark"));
                    model.State = (EyouSoft.Model.EnumType.PlanStructure.TicketState)Enum.Parse(typeof(EyouSoft.Model.EnumType.PlanStructure.TicketState), rd.GetByte(rd.GetOrdinal("State")).ToString());

                    //添加到列表中
                    ticketList.Add(model);
                }
            }


            return ticketList;
        }*/

        /// <summary>
        /// 删除一次申请机票
        /// </summary>
        /// <param name="TicketOutId"></param>
        /// <returns></returns>
        public int DeleteTicket(string TicketOutId)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Ticket_Del");
            this._db.AddInParameter(cmd, "TicketOutId", DbType.StringFixedLength, TicketOutId);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(cmd, this._db);
            object v = this._db.GetParameterValue(cmd, "Result");
            return int.Parse(v.ToString());
        }

        /*/// <summary>
        /// 机票统计
        /// </summary>
        /// <param name="SearchModel"></param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PlanStructure.TicketStatisticsModel> GetTicketCount(TicketStatisticsSearchModel SearchModel, string us)
        {
            //判断公司编号是否正常
            if (SearchModel.CompanyId == 0)
                return null;

            if (SearchModel.TicketState == EyouSoft.Model.EnumType.PlanStructure.TicketState.已出票)
            {
                return GetOutTicketCount(SearchModel, us);
            }
            else if (SearchModel.TicketState == EyouSoft.Model.EnumType.PlanStructure.TicketState.已退票)
            {
                return GetReturnTicketCount(SearchModel, us);
            }
            else
            { return null; }
        }

        /// <summary>
        /// 出票统计
        /// </summary>
        /// <param name="SearchModel"></param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PlanStructure.TicketStatisticsModel> GetOutTicketCount(TicketStatisticsSearchModel SearchModel,string us)
        {
            //判断公司编号是否正常
            if (SearchModel.CompanyId == 0)
                return null;

            IList<EyouSoft.Model.PlanStructure.TicketStatisticsModel> statisList = new List<EyouSoft.Model.PlanStructure.TicketStatisticsModel>();
            EyouSoft.Model.PlanStructure.TicketStatisticsModel model = null;
            StringBuilder StrCmd = new StringBuilder();
            StrCmd.Append(" SELECT  sum(total) as total,month(leavedate) Mon,year(leavedate) as Years,sum(peoplecount) peoplecount,sum(ReturnAmount) as ReturnAmount from(");
            StrCmd.Append(" SELECT m.returnamount, g.leavedate,m.total,(select sum(peoplecount) from tbl_PlanTicketKind where tbl_PlanTicketKind.ticketid=m.ID)  as peoplecount  FROM tbl_PlanTicketout m ");
            StrCmd.Append(" join tbl_PlanTicket n on m.id=n.refundid and n.state=3 ");
            StrCmd.Append(" join tbl_tour g on m.tourid=g.tourid ");
            if (SearchModel.TicketType != null)
            {
                StrCmd.AppendFormat(" and g.TourType={0} ", (int)SearchModel.TicketType);
            }
            if (SearchModel.AreaRouteId > 0)
            {
                StrCmd.AppendFormat(" and g.areaid={0} ", SearchModel.AreaRouteId);
            }
            if ((SearchModel.OperatorId != null && SearchModel.OperatorId.Length > 0)||!string.IsNullOrEmpty(us))
            {
                StrCmd.Append("and g.tourid in(select tourid  from tbl_TourOperator where 1=1 ");
                if (SearchModel.OperatorId != null && SearchModel.OperatorId.Length > 0)
                {
                    StrCmd.AppendFormat(" AND  OperatorId IN({0})", Utils.GetSqlIdStrByArray(SearchModel.OperatorId));                    
                }
                if (!string.IsNullOrEmpty(us))
                {
                    StrCmd.AppendFormat(" AND OperatorId IN({0}) ", us);
                }
                StrCmd.Append(")");
                //StrCmd.AppendFormat(" and g.tourid in(select tourid  from tbl_TourOperator where  OperatorId in ({0}))", ArrayToString(SearchModel.OperatorId));
            }
            if (SearchModel.LeaveDateBegin != null)
            {
                StrCmd.AppendFormat(" and datediff(dd,'{0}',g.leavedate)>=0", SearchModel.LeaveDateBegin);
            }
            if (SearchModel.LeaveDateEnd != null)
            {
                StrCmd.AppendFormat(" and datediff(dd,g.leavedate,'{0}')>=0", SearchModel.LeaveDateEnd);
            }
            StrCmd.AppendFormat(" where   m.companyid={0}", SearchModel.CompanyId);
            StrCmd.Append(" ) as l  group by month(leavedate),year(leavedate)");

            DbCommand dc = this._db.GetSqlStringCommand(StrCmd.ToString());
            using (IDataReader rd = DbHelper.ExecuteReader(dc, this._db))
            {
                while (rd.Read())
                {
                    model = new TicketStatisticsModel();
                    model.TicketCount = rd.IsDBNull(rd.GetOrdinal("peoplecount")) ? 0 : rd.GetInt32(rd.GetOrdinal("peoplecount"));
                    model.TotalAmount = rd.IsDBNull(rd.GetOrdinal("total")) ? 0 : rd.GetDecimal(rd.GetOrdinal("total"));
                    model.Mon = rd.IsDBNull(rd.GetOrdinal("Mon")) ? 0 : rd.GetInt32(rd.GetOrdinal("Mon"));
                    model.Years = rd.IsDBNull(rd.GetOrdinal("Years")) ? 0 : rd.GetInt32(rd.GetOrdinal("Years"));

                    //添加到列表中
                    statisList.Add(model);
                }
            }
            return statisList;
        }

        /// <summary>
        ///退票统计
        /// </summary>
        /// <param name="SearchModel"></param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PlanStructure.TicketStatisticsModel> GetReturnTicketCount(TicketStatisticsSearchModel SearchModel, string us)
        {
            //判断公司编号是否正常
            if (SearchModel.CompanyId == 0)
                return null;

            IList<EyouSoft.Model.PlanStructure.TicketStatisticsModel> statisList = new List<EyouSoft.Model.PlanStructure.TicketStatisticsModel>();
            EyouSoft.Model.PlanStructure.TicketStatisticsModel model = null;
            #region 搜索条件
            StringBuilder sqlWhere = new StringBuilder();
            if (SearchModel.AreaRouteId != 0)
            {
                sqlWhere.AppendFormat(" and areaid={0}", SearchModel.AreaRouteId);
            }
            if (SearchModel.OperatorId != null && SearchModel.OperatorId.Length > 0)
            {
                sqlWhere.AppendFormat("and OperatorId in ({0}) ", ArrayToString(SearchModel.OperatorId));
            }
            if (SearchModel.TicketType != null)
            {
                sqlWhere.AppendFormat(" and TourType={0}", (int)SearchModel.TicketType);
            }
            if (SearchModel.LeaveDateBegin != null)
            {
                sqlWhere.AppendFormat(" and datediff(dd,'{0}',leavedate)>=0", SearchModel.LeaveDateBegin);
            }
            if (SearchModel.LeaveDateEnd != null)
            {
                sqlWhere.AppendFormat(" and datediff(dd,leavedate,'{0}')>=0", SearchModel.LeaveDateEnd);
            }
            #endregion

            #region sql
            StringBuilder StrSql = new StringBuilder();
            StrSql.Append(" select sum(tb.peoplecount) as peoplecount,sum(tb.ReturnAmount) as ReturnAmount,sum(tb.ReturnedAmount) as ReturnedAmount,month(tb.leavedate) as mon,year(tb.leavedate)as years from");
            StrSql.Append(" ( ");
            StrSql.Append(" select a.CustomerNum as peoplecount,0 as ReturnAmount,0 as  ReturnedAmount,d.leavedate as leavedate,d.areaid as areaid,a.OperatorId as OperatorId,d.TourType as TourType,b.companyid as companyid  ");
            StrSql.Append("  from tbl_CustomerRefund a join tbl_TourOrderCustomer b on a.CustormerId=b.id  join  tbl_TourOrder c on b.OrderId = c.id join tbl_tour d on c.TourId =d.tourid  ");
            StrSql.Append(" union all");
            StrSql.Append(" select 0 as peoplecount,0 as ReturnAmount,a.refundamount as  ReturnedAmount,d.leavedate as leavedate,d.areaid as areaid,a.OperatorId as OperatorId,d.TourType as TourType,b.companyid as companyid ");
            StrSql.Append("  from tbl_CustomerRefund a  join tbl_TourOrderCustomer b on a.CustormerId=b.id and a.IsRefund=1  join  tbl_TourOrder c on b.OrderId = c.id join tbl_tour d on c.TourId =d.tourid");
            StrSql.Append(" union all");
            StrSql.Append(" select 0 as peoplecount,a.refundamount as ReturnAmount,0 as  ReturnedAmount,d.leavedate as leavedate,d.areaid as areaid,a.OperatorId as OperatorId,d.TourType as TourType,b.companyid as companyid ");
            StrSql.Append("  from tbl_CustomerRefund a join tbl_TourOrderCustomer b on a.CustormerId=b.id and a.IsRefund=0  join  tbl_TourOrder c on b.OrderId = c.id join tbl_tour d on c.TourId =d.tourid ");
            StrSql.AppendFormat(" ) as tb where companyid={0} {1}  group by month(tb.leavedate),year(tb.leavedate)", SearchModel.CompanyId, sqlWhere.ToString());
            #endregion

            DbCommand dc = this._db.GetSqlStringCommand(StrSql.ToString());
            using (IDataReader rd = DbHelper.ExecuteReader(dc, this._db))
            {
                while (rd.Read())
                {
                    model = new TicketStatisticsModel();
                    model.TicketCount = rd.IsDBNull(rd.GetOrdinal("peoplecount")) ? 0 : rd.GetInt32(rd.GetOrdinal("peoplecount"));
                    //  model.TotalAmount = rd.IsDBNull(rd.GetOrdinal("total")) ? 0 : rd.GetDecimal(rd.GetOrdinal("total"));
                    model.Mon = rd.IsDBNull(rd.GetOrdinal("Mon")) ? 0 : rd.GetInt32(rd.GetOrdinal("Mon"));
                    model.Years = rd.IsDBNull(rd.GetOrdinal("Years")) ? 0 : rd.GetInt32(rd.GetOrdinal("Years"));
                    model.NeedReturnAmount = rd.IsDBNull(rd.GetOrdinal("ReturnAmount")) ? 0 : rd.GetDecimal(rd.GetOrdinal("ReturnAmount"));
                    model.ReturnedAmount = rd.IsDBNull(rd.GetOrdinal("ReturnedAmount")) ? 0 : rd.GetDecimal(rd.GetOrdinal("ReturnedAmount")); ;
                    //添加到列表中
                    statisList.Add(model);
                }
            }
            return statisList;
        }*/

        /// <summary>
        /// 财务审核机票
        /// </summary>
        /// <param name="TicketId"></param>
        /// <param name="ReviewRemark">账务审核备注</param>
        /// <returns></returns>
        public bool CheckTicket(string TicketId, string ReviewRemark)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.Append("UPDATE [tbl_Planticket] SET [State]=@Status,VerifyTime=@VerifyTime WHERE [RefundId]=@TicketId");
            cmdText.Append(";UPDATE [tbl_PlanTicketOut] SET [State]=@Status,[ReviewRemark]=@ReviewRemark,VerifyTime=@VerifyTime WHERE [Id]=@TicketId");
            DbCommand cmd = this._db.GetSqlStringCommand(cmdText.ToString());
            this._db.AddInParameter(cmd, "TicketId", DbType.AnsiStringFixedLength, TicketId);
            this._db.AddInParameter(cmd, "Status", DbType.Byte, EyouSoft.Model.EnumType.PlanStructure.TicketState.审核通过);
            this._db.AddInParameter(cmd, "ReviewRemark", DbType.String, ReviewRemark);
            this._db.AddInParameter(cmd, "VerifyTime", DbType.DateTime, DateTime.Now);
            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 财务取消审核
        /// </summary>
        /// <param name="TicketId">机票申请Id</param>
        /// <returns>
        /// 1成功取消；
        /// 0参数错误；
        /// -1未找到对应的机票申请；
        /// -2团队状态或者机票申请状态不允许取消审核；
        /// -3取消审核过程中发生错误；
        /// </returns>
        public int UNCheckTicket(string TicketId)
        {
            if (string.IsNullOrEmpty(TicketId))
                return 0;

            DbCommand dc = this._db.GetStoredProcCommand("proc_TicketOut_UNChecked");
            this._db.AddInParameter(dc, "TicketId", DbType.AnsiStringFixedLength, TicketId.Trim());
            this._db.AddOutParameter(dc, "ErrorValue", DbType.Int32, 4);

            DbHelper.RunProcedure(dc, this._db);
            object obj = this._db.GetParameterValue(dc, "ErrorValue");
            if (obj.Equals(null))
                return 0;
            else
                return int.Parse(obj.ToString());
        }

        /// <summary>
        /// 返回机票实体
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        public TicketOutListInfo GetTicketModel(string TicketId)
        {
            TicketOutListInfo model = null;
            #region sql
            StringBuilder sql = new StringBuilder(" select a.[ID] TicketOutId,a.[PNR],a.[Total],a.[Notice],a.[TicketOffice],a.[TicketOfficeId],a.[TicketNum],a.[PayType],a.[Remark],a.[TourId],a.[Operator],a.[OperateID],a.[CompanyID],a.[AddAmount],a.[ReduceAmount],a.[TotalAmount],a.[FRemark],a.[PayAmount],a.[ReturnAmount],a.[RegisterTime],a.[ReviewRemark]");
            sql.Append(" ,b.[Id] as TicketOutListId,b.[TourId],a.[OrderId],b.[RefundId],b.[TicketType],b.[RouteName],b.[Saler],b.[OperatorId],b.[Operator] ,b.[State]");
            sql.Append(" from tbl_PlanticketOut a join tbl_PlanTicket b on a.id=b.RefundId where a.id=@ticketId");
            DbCommand cmd = this._db.GetSqlStringCommand(sql.ToString());
            //  DbCommand cmd = this._db.GetStoredProcCommand("proc_Ticket_GetModel");
            this._db.AddInParameter(cmd, "ticketId", DbType.StringFixedLength, TicketId);
            #endregion
            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rd.Read())
                {
                    model = new TicketOutListInfo();
                    model.TicketOutId = rd.GetString(rd.GetOrdinal("TicketOutId"));
                    model.AddAmount = rd.GetDecimal(rd.GetOrdinal("AddAmount"));
                    model.CompanyID = rd.GetInt32(rd.GetOrdinal("CompanyID"));
                    model.FRemark = rd.IsDBNull(rd.GetOrdinal("FRemark")) ? "" : rd.GetString(rd.GetOrdinal("FRemark"));
                    model.Notice = rd.IsDBNull(rd.GetOrdinal("Notice")) ? "" : rd.GetString(rd.GetOrdinal("Notice"));
                    model.OperateID = rd.GetInt32(rd.GetOrdinal("OperateID"));
                    model.Operator = rd.IsDBNull(rd.GetOrdinal("Operator")) ? "" : rd.GetString(rd.GetOrdinal("Operator"));
                    model.PayType = (EyouSoft.Model.EnumType.TourStructure.RefundType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.RefundType), rd.GetInt32(rd.GetOrdinal("PayType")).ToString());
                    model.PNR = rd.IsDBNull(rd.GetOrdinal("PNR")) ? "" : rd.GetString(rd.GetOrdinal("PNR"));
                    model.ReduceAmount = rd.GetDecimal(rd.GetOrdinal("ReduceAmount"));
                    model.Remark = rd.IsDBNull(rd.GetOrdinal("Remark")) ? "" : rd.GetString(rd.GetOrdinal("Remark"));
                    model.State = (EyouSoft.Model.EnumType.PlanStructure.TicketState)Enum.Parse(typeof(EyouSoft.Model.EnumType.PlanStructure.TicketState), rd.GetByte(rd.GetOrdinal("State")).ToString());
                    model.TicketNum = rd.IsDBNull(rd.GetOrdinal("TicketNum")) ? "" : rd.GetString(rd.GetOrdinal("TicketNum"));
                    model.TicketOffice = rd.IsDBNull(rd.GetOrdinal("TicketOffice")) ? "" : rd.GetString(rd.GetOrdinal("TicketOffice"));
                    model.TicketOfficeId = rd.GetInt32(rd.GetOrdinal("TicketOfficeId"));
                    model.TotalAmount = rd.GetDecimal(rd.GetOrdinal("TotalAmount"));
                    model.TourId = rd.GetString(rd.GetOrdinal("TourID"));
                    model.OrderId = rd.IsDBNull(rd.GetOrdinal("OrderId")) ? "" : rd.GetString(rd.GetOrdinal("OrderId"));
                    model.RefundId = rd.IsDBNull(rd.GetOrdinal("RefundId")) ? "" : rd.GetString(rd.GetOrdinal("RefundId"));
                    model.RouteName = rd.IsDBNull(rd.GetOrdinal("RouteName")) ? "" : rd.GetString(rd.GetOrdinal("RouteName"));
                    model.Saler = rd.IsDBNull(rd.GetOrdinal("Saler")) ? "" : rd.GetString(rd.GetOrdinal("Saler"));
                    model.TicketOutListId = rd.GetInt32(rd.GetOrdinal("TicketOutListId"));
                    model.TicketType = (EyouSoft.Model.EnumType.PlanStructure.TicketType)Enum.Parse(typeof(EyouSoft.Model.EnumType.PlanStructure.TicketType), rd.GetByte(rd.GetOrdinal("TicketType")).ToString());
                    model.Total = rd.GetDecimal(rd.GetOrdinal("Total"));
                    //游客
                    model.CustomerInfoList = GetCustomerList(model.TicketOutId);
                    //票款
                    model.TicketKindInfoList = GetKindList(model.TicketOutId);
                    //航段
                    model.TicketFlightList = GetFlightList(model.TicketOutId);
                    //账务审核备注
                    model.ReviewRemark = rd["ReviewRemark"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 返回ticketOutId
        /// </summary>
        /// <param name="TicketId"></param>
        /// <returns></returns>
        public string GetRefundTicketModel(int TicketId)
        {
            string refundId = "";
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Ticket_GetReturnModel");
            this._db.AddInParameter(cmd, "TicketId", DbType.Int32, TicketId);
            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rd.Read())
                {
                    refundId = rd.GetString(0);
                }
            }
            return refundId;
        }

        /// <summary>
        /// 申请机票
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool addTicketOutListModel(TicketOutListInfo model)
        {
            DbCommand dc = this._db.GetStoredProcCommand("proc_TicketOut_Insert");
            this._db.AddInParameter(dc, "TicketId", DbType.String, model.TicketOutId);
            this._db.AddInParameter(dc, "TourId", DbType.AnsiStringFixedLength, model.TourId);
            this._db.AddInParameter(dc, "OrderId", DbType.AnsiStringFixedLength, string.IsNullOrEmpty(model.OrderId) ? "0" : model.OrderId);
            this._db.AddInParameter(dc, "TicketType", DbType.Int16, (Int16)model.TicketType);
            this._db.AddInParameter(dc, "RouteName", DbType.String, model.RouteName);
            this._db.AddInParameter(dc, "PNR", DbType.String, model.PNR);
            this._db.AddInParameter(dc, "Total", DbType.Decimal, model.Total);
            this._db.AddInParameter(dc, "Notice", DbType.String, model.Notice);
            this._db.AddInParameter(dc, "TicketOffice", DbType.String, model.TicketOffice);
            this._db.AddInParameter(dc, "TicketOfficeId", DbType.Int32, model.TicketOfficeId);
            this._db.AddInParameter(dc, "Saler", DbType.String, model.Saler);
            this._db.AddInParameter(dc, "TicketNum", DbType.String, model.TicketNum);
            this._db.AddInParameter(dc, "PayType", DbType.Int32, (int)model.PayType);
            this._db.AddInParameter(dc, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(dc, "State", DbType.Byte, (byte)model.State);
            this._db.AddInParameter(dc, "Operator", DbType.String, model.Operator);
            this._db.AddInParameter(dc, "OperateID", DbType.Int32, model.OperateID);
            this._db.AddInParameter(dc, "CompanyID", DbType.Int32, model.CompanyID);
            this._db.AddInParameter(dc, "CustomerXML", DbType.String, this.CreateTicketTravellerXML(model.CustomerInfoList));
            this._db.AddInParameter(dc, "OutCustomerXML", DbType.String, this.CreateTicketTravellerRelationXML(model.TicketOutCustomerInfoList));
            this._db.AddInParameter(dc, "FlightListXML", DbType.String, this.CreateTicketFlightXML(model.TicketFlightList));
            this._db.AddInParameter(dc, "TicketKindXML", DbType.String, this.CreateTicketFundXML(model.TicketKindInfoList));
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            this._db.AddInParameter(dc, "RegisterOperatorId", DbType.Int32, model.RegisterOperatorId);
            DbHelper.RunProcedure(dc, this._db);
            object obj = this._db.GetParameterValue(dc, "Result");
            return int.Parse(obj.ToString()) > 0 ? true : false;
        }

        /// <summary>
        /// 退票
        /// </summary>
        /// <param name="TicketId"></param>
        /// <param name="State"></param>
        /// <param name="ReturnMoney"></param>
        /// <returns></returns>
        public bool ReturnTicketOut(EyouSoft.Model.PlanStructure.TicketRefundModel model)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Ticket_Return");
            this._db.AddInParameter(cmd, "State", DbType.Byte, model.State);
            this._db.AddInParameter(cmd, "Remark", DbType.String, model.Remark);
            this._db.AddInParameter(cmd, "ReturnMoney", DbType.Decimal, model.ReturnMoney);
            this._db.AddInParameter(cmd, "ticketId", DbType.StringFixedLength, model.TicketId);
            //this._db.AddInParameter(cmd, "TourOrderCustomer", DbType.String, CreateCustomerXML(model.TourOrderCustomerList));
            this._db.AddInParameter(cmd, "TourOrderCustomer", DbType.String, string.Empty);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(cmd, this._db);
            object o = this._db.GetParameterValue(cmd, "Result");

            return int.Parse(o.ToString()) > 0 ? true : false;
        }

        /// <summary>
        /// 机票管理列表
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyID"></param>
        /// <param name="strWhere"></param>
        /// <param name="filedOrder"></param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PlanStructure.TicketInfo> GetTicketList(int PageSize, int PageIndex, int CompanyID, ref int RecordCount, ref int PageCount, string us)
        {
            #region field
            StringBuilder field = new StringBuilder();
            field.Append("[Id],[TourId],[OrderId],[RefundId],[TicketType] ,[RouteName],[State]");
            field.Append(",(select top 1 tbl_CompanyUser.ContactName from tbl_tour join tbl_CompanyUser on tbl_tour.SellerId=tbl_CompanyUser.id  where tbl_tour.tourid=tbl_PlanTicket.tourid ) as saler ");
            field.Append(",(select top 1 tbl_CompanyUser.ContactName from tbl_tour join tbl_TourOperator on tbl_tour.tourid=tbl_TourOperator.tourid join tbl_CompanyUser on tbl_TourOperator.operatorid=tbl_CompanyUser.id  where tbl_tour.tourid=tbl_PlanTicket.tourid ) as operator ");
            field.Append(",(select top 1 [LeaveDate] from tbl_tour where tbl_tour.tourid=tbl_PlanTicket.tourid ) as LeaveDate ");
            field.Append(",(select top 1 [tourcode] from tbl_tour where tbl_tour.tourid=tbl_PlanTicket.tourid ) as tourcode ");
            field.Append(",(select * from tbl_PlanTicketFlight where tbl_PlanTicketFlight.TicketId=convert(char(36),tbl_PlanTicket.refundid) for xml raw,root('root') ) as FligthSegment  ");
            // 出票时间 add by zhengzy 2011-10-28
            field.Append(",(select TicketOutTime from tbl_PlanTicketOut where tbl_PlanTicketOut.ID=convert(char(36),tbl_PlanTicket.refundid )) as TicketOutTime  ");
            field.Append(" ,VerifyTime ");
            field.Append(
                " ,(select OrderNo,BuyCompanyID,BuyCompanyName from tbl_TourOrder where tbl_TourOrder.ID = tbl_PlanTicket.OrderId for xml raw,root('Root')) as OrderInfo ");
            #endregion
            string strWhere = string.Format(" and ([state] IN({0},{1},{2}) or TicketType=3) AND EXISTS(SELECT 1 FROM tbl_Tour AS A WHERE A.TourId=tbl_PlanTicket.TourId) ", (byte)EyouSoft.Model.EnumType.PlanStructure.TicketState.审核通过
                //string strWhere = string.Format(" and ([state] IN({0},{1},{2}) or TicketType=3) AND EXISTS(SELECT 1 FROM tbl_Tour AS A WHERE A.TourId=tbl_PlanTicket.TourId AND A.OperatorId IN({3})) ", (byte)EyouSoft.Model.EnumType.PlanStructure.TicketState.审核通过
                , (byte)EyouSoft.Model.EnumType.PlanStructure.TicketState.已出票
                , (byte)EyouSoft.Model.EnumType.PlanStructure.TicketState.已退票
                , us);

            return GetList(PageSize, PageIndex, CompanyID, strWhere, field.ToString(), ref RecordCount, ref PageCount);

        }

        /// <summary>
        /// 财务管理机票审核列表
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="CompanyID"></param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PlanStructure.TicketInfo> GetCheckedTicketList(int PageSize, int PageIndex, int CompanyID, ref int RecordCount, ref int PageCount, string us)
        {
            #region field
            StringBuilder field = new StringBuilder();
            field.Append("[Id],[TourId],[OrderId],[RefundId],[TicketType] ,[RouteName],[State]");
            field.Append(",(select top 1 tbl_CompanyUser.ContactName from tbl_tour join tbl_CompanyUser on tbl_tour.SellerId=tbl_CompanyUser.id  where tbl_tour.tourid=tbl_PlanTicket.tourid ) as saler ");
            field.Append(",(select top 1 tbl_CompanyUser.ContactName from tbl_tour join tbl_TourOperator on tbl_tour.tourid=tbl_TourOperator.tourid join tbl_CompanyUser on tbl_TourOperator.operatorid=tbl_CompanyUser.id  where tbl_tour.tourid=tbl_PlanTicket.tourid ) as operator ");
            field.Append(",(select top 1 LeaveDate from tbl_tour where tbl_tour.tourid=tbl_PlanTicket.tourid ) as LeaveDate ");
            field.Append(",(select top 1 tbl_tour.tourcode from tbl_tour where tbl_tour.tourid=tbl_PlanTicket.tourid ) as tourcode ");
            field.Append(",(select * from tbl_PlanTicketFlight where tbl_PlanTicketFlight.TicketId=convert(char(36),tbl_PlanTicket.refundid ) for xml raw,root('root') ) as FligthSegment  ");
            // 出票时间 add by zhengzy 2011-10-28
            field.Append(",(select TicketOutTime from tbl_PlanTicketOut where tbl_PlanTicketOut.ID=convert(char(36),tbl_PlanTicket.refundid )) as TicketOutTime  ");
            field.Append(" ,VerifyTime ");
            field.Append(
                " ,(select OrderNo,BuyCompanyID,BuyCompanyName from tbl_TourOrder where tbl_TourOrder.ID = tbl_PlanTicket.OrderId for xml raw,root('Root')) as OrderInfo ");
            #endregion
            string strWhere = string.Format(" and TicketType <> 3  AND EXISTS (SELECT 1 FROM tbl_Tour AS A WHERE A.TourId=tbl_PlanTicket.TourId AND A.OperatorId IN({0}))", us);
            return GetList(PageSize, PageIndex, CompanyID, strWhere, field.ToString(), ref RecordCount, ref PageCount);
        }

        /// <summary>
        /// 机票管理搜索函数
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="SearchModel"></param>
        /// <param name="RecordCount"></param>
        /// <param name="PageCount"></param>
        /// <returns></returns>
        public IList<TicketInfo> SearchTicketOut(int PageSize, int PageIndex, TicketSearchModel SearchModel, ref int RecordCount, ref int PageCount, string us)
        {
            #region field
            StringBuilder field = new StringBuilder();
            field.Append("[Id],[TourId],[OrderId],[RefundId],[TicketType] ,[RouteName],[State]");
            field.Append(",(select top 1 tbl_CompanyUser.ContactName from tbl_tour a join tbl_CompanyUser on a.SellerId=tbl_CompanyUser.id  where a.tourid=tbl_PlanTicket.tourid ) as saler ");
            field.Append(",(select top 1 tbl_CompanyUser.ContactName from tbl_tour b join tbl_TourOperator on b.tourid=tbl_TourOperator.tourid join tbl_CompanyUser on tbl_TourOperator.operatorid=tbl_CompanyUser.id  where b.tourid=tbl_PlanTicket.tourid ) as operator ");
            field.Append(",(select top 1 LeaveDate from tbl_tour c  where c.tourid=tbl_PlanTicket.tourid ) as LeaveDate ");
            field.Append(",(select top 1 d.tourcode from tbl_tour d where d.tourid=tbl_PlanTicket.tourid ) as tourcode ");
            field.Append(",(select * from tbl_PlanTicketFlight where tbl_PlanTicketFlight.TicketId=convert(char(36),tbl_PlanTicket.refundid ) for xml raw,root('root') ) as FligthSegment  ");
            // 出票时间 add by zhengzy 2011-10-28
            field.Append(",(select TicketOutTime from tbl_PlanTicketOut where tbl_PlanTicketOut.ID=convert(char(36),tbl_PlanTicket.refundid )) as TicketOutTime  ");
            field.Append(" ,VerifyTime ");
            field.Append(
                " ,(select OrderNo,BuyCompanyID,BuyCompanyName from tbl_TourOrder where tbl_TourOrder.ID = tbl_PlanTicket.OrderId for xml raw,root('Root')) as OrderInfo ");
            #endregion
            #region 拼接where
            StringBuilder strWhere = new StringBuilder("");
            if (SearchModel.TicketListOrFinancialList != 0)
            {
                if (SearchModel.TicketListOrFinancialList == 1)
                {
                    strWhere.Append(" and TicketType <>3 ");
                }
                else if (SearchModel.TicketListOrFinancialList == 2)
                {
                    strWhere.AppendFormat(" and ([state] IN({0},{1},{2}) or TicketType=3) ", (byte)EyouSoft.Model.EnumType.PlanStructure.TicketState.审核通过
                        , (byte)EyouSoft.Model.EnumType.PlanStructure.TicketState.已出票
                        , (byte)EyouSoft.Model.EnumType.PlanStructure.TicketState.已退票);
                }
            }
            if (SearchModel.TicketState.HasValue && SearchModel.TicketState.Value != PlanStructure.TicketState.None)
                strWhere.AppendFormat(" and [State] = {0} ", (int)SearchModel.TicketState.Value);
            if (!string.IsNullOrEmpty(SearchModel.Operator))
            {

                string[] str = SearchModel.Operator.Replace('，', ',').Split(new char[] { ',' });
                StringBuilder so = new StringBuilder();
                foreach (string s in str)
                    so.AppendFormat("'{0}',", s);
                strWhere.AppendFormat(" and id in(select h.id   from tbl_tour e  join tbl_TourOperator on e.tourid=tbl_TourOperator.tourid join tbl_CompanyUser on tbl_TourOperator.operatorid=tbl_CompanyUser.id  join tbl_planticket h  on e.tourid=h.tourid where tbl_CompanyUser.ContactName  in({0}))  ", so.ToString().TrimEnd(new char[] { ',' }));
            }

            /*汪奇志 2012-03-03 注释 if (!string.IsNullOrEmpty(SearchModel.DepartureTime.ToString()))
                strWhere.AppendFormat(" and id in(select id  from tbl_tour f  join tbl_planticket g on f.tourid=g.tourid  where datediff(dd,'{0}',f.leavedate)=0)", SearchModel.DepartureTime);

            if (!string.IsNullOrEmpty(SearchModel.FligthSegment) || SearchModel.AirTimeStart.HasValue || SearchModel.AirTimeEnd.HasValue)
            {
                strWhere.Append("  and id in (select i.id from tbl_PlanTicketFlight join tbl_planticket i on tbl_PlanTicketFlight.TicketId=convert(char(36),i.refundid )  where 1 = 1 ");
                if (!string.IsNullOrEmpty(SearchModel.FligthSegment))
                    strWhere.AppendFormat(" and FligthSegment like '%{0}%' ", SearchModel.FligthSegment);
                if (SearchModel.AirTimeStart.HasValue)
                    strWhere.AppendFormat(" and datediff(dd,'{0}',DepartureTime) >= 0 ", SearchModel.AirTimeStart.Value);
                if (SearchModel.AirTimeEnd.HasValue)
                    strWhere.AppendFormat(" and datediff(dd,DepartureTime,'{0}') >= 0 ", SearchModel.AirTimeEnd.Value);

                strWhere.Append(" ) ");
            }
             团号查询条件 add by zhengzy 2010-10-28
            if (!string.IsNullOrEmpty(SearchModel.TourCode))
            {
                strWhere.AppendFormat(" and (select count(*) from tbl_tour where tbl_tour.TourCode like '%{0}%' and tbl_tour.TourId = tbl_PlanTicket.TourId)>0 ", SearchModel.TourCode);
            }
            if (!string.IsNullOrEmpty(us))
            {
                strWhere.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_Tour AS A WHERE A.TourId=tbl_PlanTicket.TourId AND A.OperatorId IN({0})) ", us);
            }*/

            if (SearchModel.SVerifyTime.HasValue)
            {
                strWhere.AppendFormat(" AND VerifyTime >='{0}' ", SearchModel.SVerifyTime.Value);
            }
            if (SearchModel.EVerifyTime.HasValue)
            {
                strWhere.AppendFormat(" AND VerifyTime <='{0}' ", SearchModel.EVerifyTime.Value.AddDays(1).AddMilliseconds(-1));
            }

            if (SearchModel.LSDate.HasValue || SearchModel.LEDate.HasValue
                || !string.IsNullOrEmpty(us) || !string.IsNullOrEmpty(SearchModel.TourCode))//针对计划的查询
            {
                strWhere.Append(" AND Id IN(SELECT A.JiPiaoGuanLiId FROM tbl_PlanTicketPlanId AS A INNER JOIN tbl_PlanTicketOut AS B ON A.PlanId=B.Id WHERE 1=1 ");

                strWhere.Append(" AND EXISTS(SELECT 1 FROM tbl_Tour WHERE TourId=B.TourId ");
                if (SearchModel.LSDate.HasValue)
                {
                    strWhere.AppendFormat(" AND LeaveDate>='{0}' ", SearchModel.LSDate.Value);
                }
                if (SearchModel.LEDate.HasValue)
                {
                    strWhere.AppendFormat(" AND LeaveDate<='{0}' ", SearchModel.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                }
                if (!string.IsNullOrEmpty(us))
                {
                    strWhere.AppendFormat(" AND OperatorId IN({0}) ", us);
                }
                if (!string.IsNullOrEmpty(SearchModel.TourCode))
                {
                    strWhere.AppendFormat(" AND TourCode LIKE '%{0}%' ", SearchModel.TourCode);
                }

                strWhere.Append(" ) ");
                strWhere.Append(" ) ");
            }

            if (!string.IsNullOrEmpty(SearchModel.FligthSegment)
                || SearchModel.AirTimeStart.HasValue
                || SearchModel.AirTimeEnd.HasValue
                || !string.IsNullOrEmpty(SearchModel.DepartureTime))//针对航段的查询
            {
                strWhere.Append(" AND Id IN(SELECT A.JiPiaoGuanLiId FROM tbl_PlanTicketPlanId AS A INNER JOIN tbl_PlanTicketFlight AS B ON A.PlanId=B.TicketId WHERE 1=1 ");

                if (!string.IsNullOrEmpty(SearchModel.FligthSegment))
                {
                    strWhere.AppendFormat(" AND B.FligthSegment LIKE '%{0}%' ", SearchModel.FligthSegment);
                }

                if (SearchModel.AirTimeStart.HasValue)
                {
                    strWhere.AppendFormat(" AND B.DepartureTime>='{0}' ", SearchModel.AirTimeStart.Value);
                }

                if (SearchModel.AirTimeEnd.HasValue)
                {
                    strWhere.AppendFormat(" AND B.DepartureTime<='{0}' ", SearchModel.AirTimeEnd.Value.AddDays(1).AddMilliseconds(-1));
                }

                if (!string.IsNullOrEmpty(SearchModel.DepartureTime))
                {
                    strWhere.AppendFormat(" AND B.TicketTime LIKE '%{0}%' ", SearchModel.DepartureTime);
                }

                strWhere.Append(")");
            }

            #endregion
            return GetList(PageSize, PageIndex, SearchModel.CompanyId, strWhere.ToString(), field.ToString(), ref RecordCount, ref PageCount);

        }

        /// <summary>
        /// 已订机票游客
        /// </summary>
        /// <param name="TourId"></param>
        /// <returns></returns>
        public IList<string> CustomerList(string TourId)
        {
            string sql = string.Format("select b.UserId  from  tbl_PlanTicketOut a join tbl_PlanTicketOutCustomer b on a.id=b.TicketOutId  WHERE a.tourid=@TourId AND IsApplyRefund='0' ");
            IList<string> customerList = new List<string>();
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "TourId", DbType.StringFixedLength, TourId);
            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {
                string customer = "0";
                while (rd.Read())
                {
                    customer = rd.GetString(rd.GetOrdinal("UserId"));
                    customerList.Add(customer);
                }
            }
            return customerList;
        }

        /// <summary>
        /// 已出票的操作员名字
        /// </summary>
        /// <param name="TourId"></param>
        /// <returns></returns>
        public IList<string> CustomerOperatorList(string TourId)
        {
            string sql = string.Format("select a.operator  from  tbl_PlanTicket a  where State={0} and a.tourid=@TourId", (int)EyouSoft.Model.EnumType.PlanStructure.TicketState.已出票);
            IList<string> operatorNameList = new List<string>();
            DbCommand cmd = this._db.GetSqlStringCommand(sql);
            this._db.AddInParameter(cmd, "TourId", DbType.StringFixedLength, TourId);
            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {
                string operatorName = "";
                while (rd.Read())
                {
                    operatorName = rd.IsDBNull(rd.GetOrdinal("operator")) ? "" : rd.GetString(rd.GetOrdinal("operator"));
                    operatorNameList.Add(operatorName);
                }
            }
            return operatorNameList;
        }

        /// <summary>
        /// 通过TicketId获取ticketOutId
        /// </summary>
        /// <param name="TicketId">TicketId</param>
        /// <returns></returns>
        public string GetTicketOutId(int TicketId)
        {
            string ticketOutId = "";
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Ticket_GetTicketOutId");
            this._db.AddInParameter(cmd, "TicketId", DbType.Int32, TicketId);
            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {

                if (rd.Read())
                {
                    ticketOutId = rd.GetString(rd.GetOrdinal("TicketOutId"));
                    return ticketOutId;

                }
            }
            return ticketOutId;
        }

        /// <summary>
        /// 获取出票统计出票量明细
        /// </summary>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="PageIndex">当前页数</param>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="model">查询实体</param>
        /// <param name="HaveUserIds">用户Id集合，半角逗号分割</param>
        /// <returns></returns>
        public IList<Model.PlanStructure.TicketOutStatisticInfo> GetTicketOutStatisticList(int PageSize, int PageIndex
            , ref int RecordCount, Model.StatisticStructure.QueryTicketOutStatisti model, string HaveUserIds)
        {
            if (model == null)
                return null;

            IList<Model.PlanStructure.TicketOutStatisticInfo> list = new List<Model.PlanStructure.TicketOutStatisticInfo>();
            Model.PlanStructure.TicketOutStatisticInfo tmpModel = null;
            XElement xRoot = null;
            IEnumerable<XElement> xRows = null;
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" [State] = {0} ", (int)Model.EnumType.PlanStructure.TicketState.已出票);
            string tableName = "View_TicketOutStatisticDepart";
            string primaryKey = "ID";
            StringBuilder strFile = new StringBuilder(" PNR,TicketNum,TotalAmount,AireLine,TicketKind ");

            #region SqlWhere

            if (model.CompanyId > 0)
                strWhere.AppendFormat(" and CompanyID = {0} ", model.CompanyId);
            if (!string.IsNullOrEmpty(HaveUserIds))
                strWhere.AppendFormat(" and RegisterOperatorId in ({0}) ", HaveUserIds);
            if (model.DepartIds != null && model.DepartIds.Length > 0)
                strWhere.AppendFormat(" and DepartId in ({0}) ", this.GetIdsByIdArr(model.DepartIds));
            if (!string.IsNullOrEmpty(model.DepartName))
                strWhere.AppendFormat(" and DepartName like '%{0}%' ", model.DepartName);
            if (model.OfficeId > 0)
                strWhere.AppendFormat(" and TicketOfficeId = {0} ", model.OfficeId);
            if (!string.IsNullOrEmpty(model.OfficeName))
                strWhere.AppendFormat(" and TicketOffice like '%{0}%' ", model.OfficeName);
            if (model.StartTicketOutTime.HasValue)
                strWhere.AppendFormat(" and datediff(dd,'{0}',TicketOutTime) >= 0 ", model.StartTicketOutTime.Value);
            if (model.EndTicketOutTime.HasValue)
                strWhere.AppendFormat(" and datediff(dd,TicketOutTime,'{0}') >= 0 ", model.EndTicketOutTime.Value);
            if (model.LeaveDateStart.HasValue || model.LeaveDateEnd.HasValue)
            {
                strWhere.Append(" and exists (select 1 from tbl_Tour as a where a.TourId = View_TicketOutStatisticDepart.TourId ");
                if (model.LeaveDateStart.HasValue)
                    strWhere.AppendFormat(" and a.LeaveDate >= '{0}' ", model.LeaveDateStart.Value);
                if (model.LeaveDateEnd.HasValue)
                    strWhere.AppendFormat(" and a.LeaveDate <= '{0}' ", model.LeaveDateEnd.Value);

                strWhere.Append(" ) ");

            }

            if (model.AirLineIds != null && model.AirLineIds.Length > 0)
            {
                string strIds = GetIdsByIdArr(model.AirLineIds);
                if (!string.IsNullOrEmpty(strIds))
                {
                    strWhere.Append(" and exists (select 1 from tbl_PlanTicketFlight where tbl_PlanTicketFlight.TicketId = View_TicketOutStatisticDepart.ID ");
                    strWhere.AppendFormat(" and tbl_PlanTicketFlight.AireLine in ({0}) ", strIds);
                    strWhere.Append(") ");
                }
            }

            #endregion

            #region  实体赋值

            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, tableName, primaryKey
                , strFile.ToString(), strWhere.ToString(), "TicketOutTime desc"))
            {
                while (dr.Read())
                {
                    tmpModel = new TicketOutStatisticInfo();

                    if (!dr.IsDBNull(0))
                        tmpModel.PNR = dr.GetString(0);
                    if (!dr.IsDBNull(1))
                        tmpModel.TicketNum = dr.GetString(1);
                    if (!dr.IsDBNull(2))
                        tmpModel.TotalAmount = dr.GetDecimal(2);
                    if (!dr.IsDBNull(3))
                    {
                        tmpModel.TicketFlightList = new List<TicketFlight>();
                        TicketFlight TFModel = null;
                        xRoot = XElement.Parse(dr.GetString(3));
                        if (xRoot != null)
                        {
                            xRows = Utils.GetXElements(xRoot, "row");
                            if (xRows != null && xRows.Count() > 0)
                            {
                                foreach (var t in xRows)
                                {
                                    TFModel = new TicketFlight();

                                    TFModel.AireLine = (EyouSoft.Model.EnumType.PlanStructure.FlightCompany)Utils.GetInt(Utils.GetXAttributeValue(t, "AireLine"));
                                    TFModel.FligthSegment = Utils.GetXAttributeValue(t, "FligthSegment");
                                    TFModel.DepartureTime = Utils.GetDateTime(Utils.GetXAttributeValue(t, "DepartureTime"), DateTime.MinValue);
                                    TFModel.TicketTime = Utils.GetXAttributeValue(t, "TicketTime");
                                    TFModel.Discount = Utils.GetDecimal(Utils.GetXAttributeValue(t, "Discount"));

                                    tmpModel.TicketFlightList.Add(TFModel);
                                }
                            }
                        }
                    }
                    if (!dr.IsDBNull(4))
                    {
                        tmpModel.TicketKindList = new List<TicketKindInfo>();
                        TicketKindInfo TKModel = null;
                        xRoot = XElement.Parse(dr.GetString(4));
                        if (xRoot != null)
                        {
                            xRows = Utils.GetXElements(xRoot, "row");
                            if (xRows != null && xRows.Count() > 0)
                            {
                                foreach (var t in xRows)
                                {
                                    TKModel = new TicketKindInfo();

                                    TKModel.Price = Utils.GetDecimal(Utils.GetXAttributeValue(t, "Price"));
                                    TKModel.OilFee = Utils.GetDecimal(Utils.GetXAttributeValue(t, "OilFee"));
                                    TKModel.AgencyPrice = Utils.GetDecimal(Utils.GetXAttributeValue(t, "AgencyPrice"));
                                    TKModel.PeopleCount = Utils.GetInt(Utils.GetXAttributeValue(t, "PeopleCount"));
                                    TKModel.TotalMoney = Utils.GetDecimal(Utils.GetXAttributeValue(t, "TotalMoney"));
                                    TKModel.TicketType = (EyouSoft.Model.EnumType.PlanStructure.KindType)Utils.GetInt(Utils.GetXAttributeValue(t, "TicketType"));

                                    //出票量累加
                                    tmpModel.PeopleCount += TKModel.PeopleCount;

                                    tmpModel.TicketKindList.Add(TKModel);
                                }
                            }
                        }
                    }

                    list.Add(tmpModel);
                }
            }

            #endregion

            return list;
        }

        /// <summary>
        /// 获取计划机票申请信息集合
        /// </summary>
        /// <param name="companyId">公司(专线)编号</param>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public IList<MLBTicketApplyInfo> GetTicketApplys(int companyId, string tourId)
        {
            IList<MLBTicketApplyInfo> items = new List<MLBTicketApplyInfo>();
            DbCommand cmd = this._db.GetSqlStringCommand("SELECT 1");
            StringBuilder cmdText = new StringBuilder();
            cmdText.Append(" SELECT A.[Id],A.[TourId],A.[State],A.[PNR],A.[Total],A.[AddAmount],A.[ReduceAmount],A.[TotalAmount],A.[RegisterTime],A.[Remark] ");
            cmdText.Append(" ,(SELECT * FROM [tbl_PlanTicketFlight] AS B WHERE B.[TicketId]=A.Id FOR XML RAW,ROOT('root')) AS Flights ");
            cmdText.Append(" ,(SELECT * FROM [tbl_PlanTicketKind] AS B WHERE B.[TicketId]=A.Id FOR XML RAW,ROOT('root')) AS Funds ");
            cmdText.Append(" FROM [tbl_PlanTicketOut] AS A WHERE 1=1 ");

            if (companyId > 0)
            {
                cmdText.Append(" AND A.[CompanyID]=@CompanyId ");
                this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);
            }

            if (!string.IsNullOrEmpty(tourId))
            {
                cmdText.Append(" AND A.[TourId]=@TourId ");
                this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);
            }

            cmd.CommandText = cmdText.ToString();

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    TicketKindInfo fundAdult = new TicketKindInfo();
                    TicketKindInfo fundChildren = new TicketKindInfo();
                    IList<TicketKindInfo> funds = ParseTicketFundsByXml(rdr["Funds"].ToString());

                    if (funds != null && funds.Count > 0)
                    {
                        foreach (var fund in funds)
                        {
                            if (fund.TicketType == EyouSoft.Model.EnumType.PlanStructure.KindType.成人) fundAdult = fund;
                            if (fund.TicketType == EyouSoft.Model.EnumType.PlanStructure.KindType.儿童) fundChildren = fund;
                        }
                    }

                    items.Add(new MLBTicketApplyInfo()
                    {
                        AddAmount = rdr.GetDecimal(rdr.GetOrdinal("AddAmount")),
                        Amount = rdr.GetDecimal(rdr.GetOrdinal("Total")),
                        ApplyId = rdr.GetString(rdr.GetOrdinal("Id")),
                        ApplyTime = rdr.GetDateTime(rdr.GetOrdinal("RegisterTime")),
                        FundAdult = fundAdult,
                        FundChildren = fundChildren,
                        PNR = rdr["PNR"].ToString(),
                        ReduceAmount = rdr.GetDecimal(rdr.GetOrdinal("ReduceAmount")),
                        Remark = rdr["Remark"].ToString(),
                        Status = (EyouSoft.Model.EnumType.PlanStructure.TicketState)rdr.GetByte(rdr.GetOrdinal("State")),
                        TicketFlights = ParseTicketFlightsByXml(rdr["Flights"].ToString()),
                        TotalAmount = rdr.GetDecimal(rdr.GetOrdinal("TotalAmount")),
                        TourId = rdr.GetString(rdr.GetOrdinal("TourId"))
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 机票管理退票，已退票时同时写杂费收入及收入明细，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="refundTicketInfo">机票退票信息业务实体</param>
        /// <returns></returns>
        public int RefundTicket(MRefundTicketInfo refundTicketInfo)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Ticket_Return");
            this._db.AddInParameter(cmd, "TicketListId", DbType.Int32, refundTicketInfo.TicketListId);
            this._db.AddInParameter(cmd, "RefundAmount", DbType.Decimal, refundTicketInfo.RefundAmount);
            this._db.AddInParameter(cmd, "Status", DbType.Byte, refundTicketInfo.Status);
            this._db.AddInParameter(cmd, "Remark", DbType.String, refundTicketInfo.Remark);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, refundTicketInfo.OperatorId);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            this._db.AddOutParameter(cmd, "TourId", DbType.String, 36);
            DbHelper.RunProcedure(cmd, this._db);
            object objResult = this._db.GetParameterValue(cmd, "Result");
            object objTourId = this._db.GetParameterValue(cmd, "TourId");

            refundTicketInfo.TourId = objTourId.ToString();

            return int.Parse(objResult.ToString());
        }

        /// <summary>
        /// 获取游客退票航段信息集体
        /// </summary>
        /// <param name="refundId">退票编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PlanStructure.TicketFlight> GetRefundTicketFlights(string refundId)
        {
            IList<EyouSoft.Model.PlanStructure.TicketFlight> items = new List<EyouSoft.Model.PlanStructure.TicketFlight>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetRefundTicketFlights);
            this._db.AddInParameter(cmd, "RefundId", DbType.AnsiStringFixedLength, refundId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(new TicketFlight()
                    {
                        AireLine = (EyouSoft.Model.EnumType.PlanStructure.FlightCompany)rdr.GetByte(rdr.GetOrdinal("AireLine")),
                        DepartureTime = rdr.GetDateTime(rdr.GetOrdinal("DepartureTime")),
                        Discount = rdr.GetDecimal(rdr.GetOrdinal("Discount")),
                        FligthSegment = rdr["FligthSegment"].ToString(),
                        ID = rdr.GetInt32(rdr.GetOrdinal("Id")),
                        TicketId = rdr.GetString(rdr.GetOrdinal("TicketId")),
                        TicketTime = rdr["TicketTime"].ToString()
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 获取财务机票审核实体信息
        /// </summary>
        /// <param name="ticketId">机票申请Id</param>
        /// <returns>财务机票审核实体信息</returns>
        public MCheckTicketInfo GetMCheckTicket(string ticketId)
        {
            MCheckTicketInfo model = null;
            if (string.IsNullOrEmpty(ticketId))
                return model;

            var strSql = new StringBuilder();
            strSql.Append("select ID,TotalAmount,[State],[ReviewRemark] from tbl_PlanTicketOut where ID = @ticketId; ");
            strSql.Append("select * from tbl_TourOrderCustomer where ID in (select UserId from tbl_PlanTicketOutCustomer where TicketOutId = @ticketId); ");
            strSql.Append("select * from tbl_PlanTicketFlight where TicketId = @ticketId; ");
            strSql.Append("select * from tbl_PlanTicketKind where TicketId = @ticketId; ");
            strSql.Append("select BuyCompanyName,FinanceSum,HasCheckMoney,NotCheckMoney from tbl_TourOrder where IsDelete = '0' and OrderState not in (3,4) and ID in (select distinct OrderId from tbl_TourOrderCustomer where ID in (select distinct UserId from tbl_PlanTicketOutCustomer where TicketOutId = @ticketId));");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "ticketId", DbType.AnsiStringFixedLength, ticketId);

            using (var dr = DbHelper.ExecuteReader(dc, _db))
            {
                model = new MCheckTicketInfo();

                #region 机票申请信息

                while (dr.Read())  //机票申请信息
                {
                    if (!dr.IsDBNull(0))
                        model.TicketId = dr.GetString(0);
                    if (!dr.IsDBNull(1))
                        model.TotalAmount = dr.GetDecimal(1);
                    if (!dr.IsDBNull(2))
                        model.TicketState = (PlanStructure.TicketState)dr.GetByte(2);
                    model.ReviewRemark = dr["ReviewRemark"].ToString();
                }

                #endregion

                #region 机票申请游客信息

                dr.NextResult();
                model.CustomerList = new List<TourOrderCustomer>();
                TourOrderCustomer customerModel;
                while (dr.Read())  //机票申请游客信息
                {
                    customerModel = new TourOrderCustomer();
                    if (!dr.IsDBNull(dr.GetOrdinal("ID")))
                        customerModel.ID = dr.GetString(dr.GetOrdinal("ID"));
                    if (!dr.IsDBNull(dr.GetOrdinal("OrderId")))
                        customerModel.OrderId = dr.GetString(dr.GetOrdinal("OrderId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CompanyID")))
                        customerModel.CompanyID = dr.GetInt32(dr.GetOrdinal("CompanyID"));
                    if (!dr.IsDBNull(dr.GetOrdinal("VisitorName")))
                        customerModel.VisitorName = dr.GetString(dr.GetOrdinal("VisitorName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CradType")))
                        customerModel.CradType = (Model.EnumType.TourStructure.CradType)dr.GetByte(dr.GetOrdinal("CradType"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CradNumber")))
                        customerModel.CradNumber = dr.GetString(dr.GetOrdinal("CradNumber"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Sex")))
                        customerModel.Sex = (Model.EnumType.CompanyStructure.Sex)dr.GetByte(dr.GetOrdinal("Sex"));
                    if (!dr.IsDBNull(dr.GetOrdinal("VisitorType")))
                        customerModel.VisitorType = (Model.EnumType.TourStructure.VisitorType)dr.GetByte(dr.GetOrdinal("VisitorType"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactTel")))
                        customerModel.ContactTel = dr.GetString(dr.GetOrdinal("ContactTel"));
                    if (!dr.IsDBNull(dr.GetOrdinal("IssueTime")))
                        customerModel.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("CustomerStatus")))
                        customerModel.CustomerStatus = (Model.EnumType.TourStructure.CustomerStatus)dr.GetByte(dr.GetOrdinal("CustomerStatus"));

                    model.CustomerList.Add(customerModel);
                }

                #endregion

                #region 机票航段信息

                dr.NextResult();
                model.TicketFlightList = new List<TicketFlight>();
                TicketFlight flightModel;
                while (dr.Read())
                {
                    flightModel = new TicketFlight();
                    if (!dr.IsDBNull(dr.GetOrdinal("ID")))
                        flightModel.ID = dr.GetInt32(dr.GetOrdinal("ID"));
                    if (!dr.IsDBNull(dr.GetOrdinal("FligthSegment")))
                        flightModel.FligthSegment = dr.GetString(dr.GetOrdinal("FligthSegment"));
                    if (!dr.IsDBNull(dr.GetOrdinal("DepartureTime")))
                        flightModel.DepartureTime = dr.GetDateTime(dr.GetOrdinal("DepartureTime"));
                    if (!dr.IsDBNull(dr.GetOrdinal("AireLine")))
                        flightModel.AireLine = (PlanStructure.FlightCompany)dr.GetByte(dr.GetOrdinal("AireLine"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Discount")))
                        flightModel.Discount = dr.GetDecimal(dr.GetOrdinal("Discount"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TicketId")))
                        flightModel.TicketId = dr.GetString(dr.GetOrdinal("TicketId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TicketTime")))
                        flightModel.TicketTime = dr.GetString(dr.GetOrdinal("TicketTime"));

                    model.TicketFlightList.Add(flightModel);
                }

                #endregion

                #region 机票票款信息

                dr.NextResult();
                model.TicketKindList = new List<TicketKindInfo>();
                model.TicketNum = 0;
                TicketKindInfo kindModel;
                while (dr.Read())
                {
                    kindModel = new TicketKindInfo();
                    if (!dr.IsDBNull(dr.GetOrdinal("ID")))
                        kindModel.ID = dr.GetInt32(dr.GetOrdinal("ID"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TicketId")))
                        kindModel.TicketId = dr.GetString(dr.GetOrdinal("TicketId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Price")))
                        kindModel.Price = dr.GetDecimal(dr.GetOrdinal("Price"));
                    if (!dr.IsDBNull(dr.GetOrdinal("OilFee")))
                        kindModel.OilFee = dr.GetDecimal(dr.GetOrdinal("OilFee"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PeopleCount")))
                    {
                        kindModel.PeopleCount = dr.GetInt32(dr.GetOrdinal("PeopleCount"));
                        model.TicketNum += kindModel.PeopleCount;
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("AgencyPrice")))
                        kindModel.AgencyPrice = dr.GetDecimal(dr.GetOrdinal("AgencyPrice"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TotalMoney")))
                        kindModel.TotalMoney = dr.GetDecimal(dr.GetOrdinal("TotalMoney"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TicketType")))
                        kindModel.TicketType = (PlanStructure.KindType)dr.GetByte(dr.GetOrdinal("TicketType"));
                    if (!dr.IsDBNull(dr.GetOrdinal("Discount")))
                        kindModel.Discount = dr.GetDecimal(dr.GetOrdinal("Discount"));
                    if (!dr.IsDBNull(dr.GetOrdinal("OtherPrice")))
                        kindModel.OtherPrice = dr.GetDecimal(dr.GetOrdinal("OtherPrice"));

                    model.TicketKindList.Add(kindModel);
                }

                #endregion

                #region 机票申请所含游客所在的订单信息

                dr.NextResult();
                model.CustomerNames = string.Empty;
                model.OrderTotalAmount = 0;
                model.HasOrderAmount = 0;
                while (dr.Read())
                {
                    //BuyCompanyName,FinanceSum,HasCheckMoney,NotCheckMoney
                    if (!dr.IsDBNull(0))
                        model.CustomerNames += dr.GetString(0) + ",";
                    if (!dr.IsDBNull(1))
                        model.OrderTotalAmount += dr.GetDecimal(1);
                    if (!dr.IsDBNull(2))
                        model.HasOrderAmount += dr.GetDecimal(2);
                }

                model.CustomerNames = model.CustomerNames.Trim(',');

                #endregion
            }

            return model;
        }

        /// <summary>
        /// 取消出票(删除支出明细，支付登记明细，更改机票状态为未出票)
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <param name="planId">机票安排编号</param>
        /// <returns></returns>
        public int CancelTicket(string tourId, string planId)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.Append("DECLARE @tmp_tbl_flightid TABLE(FID INT)");
            cmdText.Append(";DECLARE @tmp_tbl_returnid TABLE(RID CHAR(36))");
            cmdText.Append(";INSERT INTO @tmp_tbl_flightid(FID)SELECT [Id] FROM [tbl_PlanTicketFlight] WHERE TicketId=@PlanId");
            cmdText.Append(";INSERT INTO @tmp_tbl_returnid(RID)SELECT [RefundId] FROM [tbl_CustomerRefundFlight] WHERE FlightId IN(SELECT FID FROM @tmp_tbl_flightid)");
            cmdText.Append(";DELETE FROM [tbl_CustomerRefundFlight] WHERE FlightId IN(SELECT FID FROM @tmp_tbl_flightid)");
            cmdText.Append(";DELETE FROM [tbl_CustomerRefund] WHERE Id IN(SELECT RID FROM @tmp_tbl_returnid)");
            cmdText.Append(";DELETE FROM [tbl_PlanTicketPlanId] WHERE [PlanId]=@PlanId AND [JiPiaoGuanLiId] IN(SELECT [Id] FROM [tbl_PlanTicket] WHERE [RefundId] IN(SELECT [RID] FROM @tmp_tbl_returnid) AND [TicketType]=@TicketType)");
            cmdText.Append(";DELETE FROM [tbl_PlanTicket] WHERE RefundId IN(SELECT RID FROM @tmp_tbl_returnid) AND [TicketType]=@TicketType");
            cmdText.Append(";DELETE FROM [tbl_StatAllOut] WHERE [TourId]=@TourId AND [ItemType]=@StatPaidType AND [ItemId]=@PlanId");
            cmdText.Append(";DELETE FROM [tbl_FinancialPayInfo] WHERE [ItemId]=@TourId AND [ItemType]=@OutRegisterType AND [ReceiveType]=@PlanType AND [ReceiveId]=@PlanId");
            cmdText.Append(";UPDATE [tbl_PlanTicketOut] SET [State]=@TicketStatus WHERE [ID]=@PlanId AND TourId=@TourId ");
            cmdText.Append(";UPDATE [tbl_PlanTicketOut] SET [PayAmount] = (SELECT ISNULL(SUM([PaymentAmount]),0) FROM [tbl_FinancialPayInfo] WHERE [ItemId]=@TourId AND [ItemType]=@OutRegisterType AND [ReceiveType]=@PlanType AND [ReceiveId]=@PlanId AND [IsChecked] = '1' AND [IsPay] = '1') WHERE [ID]=@PlanId AND TourId=@TourId");
            cmdText.Append(";UPDATE [tbl_PlanTicket] SET [State]=@TicketStatus WHERE [TourId]=@TourId AND [RefundId]=@PlanId");
            //cmdText.Append(";DELETE FROM tbl_PlanTicketPlanId WHERE PlanId=@PlanId");

            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());
            _db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);
            _db.AddInParameter(cmd, "StatPaidType", DbType.Byte, EyouSoft.Model.EnumType.StatisticStructure.PaidType.机票支出);
            _db.AddInParameter(cmd, "PlanId", DbType.AnsiStringFixedLength, planId);
            _db.AddInParameter(cmd, "OutRegisterType", DbType.Byte, Model.EnumType.FinanceStructure.OutRegisterType.团队);
            _db.AddInParameter(cmd, "PlanType", DbType.Byte, Model.EnumType.FinanceStructure.OutPlanType.票务);
            _db.AddInParameter(cmd, "TicketStatus", DbType.Byte, Model.EnumType.PlanStructure.TicketState.审核通过);
            _db.AddInParameter(cmd, "TicketType", DbType.Byte, Model.EnumType.PlanStructure.TicketType.订单退票);

            DbHelper.ExecuteSql(cmd, _db);

            return 1;
        }

        /// <summary>
        /// 取消退票，返回1成功，其它失败
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="jiPiaoGuanLiId">机票管理编号</param>
        /// <returns></returns>
        public int QuXiaoTuiPiao(int companyId, int jiPiaoGuanLiId)
        {
            DbCommand cmd = _db.GetStoredProcCommand("proc_PlanTicketQuXiaoTuiPiao");
            _db.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
            _db.AddInParameter(cmd, "@JiPiaoGuanLiId", DbType.Int32, jiPiaoGuanLiId);
            _db.AddOutParameter(cmd, "@RetCode", DbType.Int32, 4);

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

        /// <summary>
        /// 取消机票审核，1:成功，-1:非审核通过状态下的机票申请不存在取消审核操作，-2:团队已提交财务不可取消审核
        /// </summary>
        /// <param name="ticketId">机票申请编号</param>
        /// <param name="tourId">团队编号</param>
        /// <returns></returns>
        public int QuXiaoShenHe(string ticketId, out string tourId)
        {
            tourId = string.Empty;

            DbCommand cmd = _db.GetStoredProcCommand("proc_PlanTicketQuXiaoShenHe");
            _db.AddInParameter(cmd, "TicketId", DbType.String, ticketId);
            _db.AddOutParameter(cmd, "RetCode", DbType.Int32, 4);
            _db.AddOutParameter(cmd, "TourId", DbType.AnsiStringFixedLength, 36);

            int sqlExceptionCode = 0;

            try
            {
                DbHelper.RunProcedure(cmd, _db);
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                sqlExceptionCode = 0 - e.Number;
            }

            if (sqlExceptionCode < 0) return sqlExceptionCode;

            object objTourId = _db.GetParameterValue(cmd, "TourId");
            object objRetCode = _db.GetParameterValue(cmd, "RetCode");

            if (objTourId != null)
            {
                tourId = objTourId.ToString();
            }

            if (objRetCode != null)
            {
                return Convert.ToInt32(objRetCode);
            }

            return 0;
        }

        /// <summary>
        /// 根据订单编号获取机票申请信息（因为一个订单只有一条机票申请信息）
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public TicketOutListInfo GetTicketOutInfoByOrderId(string orderId)
        {
            TicketOutListInfo model = null;
            #region sql
            var sql = new StringBuilder(" select a.[ID] TicketOutId,a.[PNR],a.[Total],a.[Notice],a.[TicketOffice],a.[TicketOfficeId],a.[TicketNum],a.[PayType],a.[Remark],a.[TourId],a.[Operator],a.[OperateID],a.[CompanyID],a.[AddAmount],a.[ReduceAmount],a.[TotalAmount],a.[FRemark],a.[PayAmount],a.[ReturnAmount],a.[RegisterTime],a.[ReviewRemark]");
            sql.Append(" ,b.[Id] as TicketOutListId,b.[TourId],a.[OrderId],b.[RefundId],b.[TicketType],b.[RouteName],b.[Saler],b.[OperatorId],b.[Operator] ,b.[State]");
            sql.Append(" from tbl_PlanticketOut a join tbl_PlanTicket b on a.id=b.RefundId where a.OrderId=@OrderId");
            DbCommand cmd = this._db.GetSqlStringCommand(sql.ToString());
            this._db.AddInParameter(cmd, "OrderId", DbType.AnsiStringFixedLength, orderId);
            #endregion
            using (IDataReader rd = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rd.Read())
                {
                    model = new TicketOutListInfo();
                    model.TicketOutId = rd.GetString(rd.GetOrdinal("TicketOutId"));
                    model.AddAmount = rd.GetDecimal(rd.GetOrdinal("AddAmount"));
                    model.CompanyID = rd.GetInt32(rd.GetOrdinal("CompanyID"));
                    model.FRemark = rd.IsDBNull(rd.GetOrdinal("FRemark")) ? "" : rd.GetString(rd.GetOrdinal("FRemark"));
                    model.Notice = rd.IsDBNull(rd.GetOrdinal("Notice")) ? "" : rd.GetString(rd.GetOrdinal("Notice"));
                    model.OperateID = rd.GetInt32(rd.GetOrdinal("OperateID"));
                    model.Operator = rd.IsDBNull(rd.GetOrdinal("Operator")) ? "" : rd.GetString(rd.GetOrdinal("Operator"));
                    model.PayType = (EyouSoft.Model.EnumType.TourStructure.RefundType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.RefundType), rd.GetInt32(rd.GetOrdinal("PayType")).ToString());
                    model.PNR = rd.IsDBNull(rd.GetOrdinal("PNR")) ? "" : rd.GetString(rd.GetOrdinal("PNR"));
                    model.ReduceAmount = rd.GetDecimal(rd.GetOrdinal("ReduceAmount"));
                    model.Remark = rd.IsDBNull(rd.GetOrdinal("Remark")) ? "" : rd.GetString(rd.GetOrdinal("Remark"));
                    model.State = (EyouSoft.Model.EnumType.PlanStructure.TicketState)Enum.Parse(typeof(EyouSoft.Model.EnumType.PlanStructure.TicketState), rd.GetByte(rd.GetOrdinal("State")).ToString());
                    model.TicketNum = rd.IsDBNull(rd.GetOrdinal("TicketNum")) ? "" : rd.GetString(rd.GetOrdinal("TicketNum"));
                    model.TicketOffice = rd.IsDBNull(rd.GetOrdinal("TicketOffice")) ? "" : rd.GetString(rd.GetOrdinal("TicketOffice"));
                    model.TicketOfficeId = rd.GetInt32(rd.GetOrdinal("TicketOfficeId"));
                    model.TotalAmount = rd.GetDecimal(rd.GetOrdinal("TotalAmount"));
                    model.TourId = rd.GetString(rd.GetOrdinal("TourID"));
                    model.OrderId = rd.IsDBNull(rd.GetOrdinal("OrderId")) ? "" : rd.GetString(rd.GetOrdinal("OrderId"));
                    model.RefundId = rd.IsDBNull(rd.GetOrdinal("RefundId")) ? "" : rd.GetString(rd.GetOrdinal("RefundId"));
                    model.RouteName = rd.IsDBNull(rd.GetOrdinal("RouteName")) ? "" : rd.GetString(rd.GetOrdinal("RouteName"));
                    model.Saler = rd.IsDBNull(rd.GetOrdinal("Saler")) ? "" : rd.GetString(rd.GetOrdinal("Saler"));
                    model.TicketOutListId = rd.GetInt32(rd.GetOrdinal("TicketOutListId"));
                    model.TicketType = (EyouSoft.Model.EnumType.PlanStructure.TicketType)Enum.Parse(typeof(EyouSoft.Model.EnumType.PlanStructure.TicketType), rd.GetByte(rd.GetOrdinal("TicketType")).ToString());
                    model.Total = rd.GetDecimal(rd.GetOrdinal("Total"));
                    //游客
                    model.CustomerInfoList = GetCustomerList(model.TicketOutId);
                    //票款
                    model.TicketKindInfoList = GetKindList(model.TicketOutId);
                    //航段
                    model.TicketFlightList = GetFlightList(model.TicketOutId);
                    //账务审核备注
                    model.ReviewRemark = rd["ReviewRemark"].ToString();
                }
            }
            return model;
        }

        #endregion
    }
}

