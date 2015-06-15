using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using EyouSoft.Model.TourStructure;
using Microsoft.Practices.EnterpriseLibrary.Data;
using EyouSoft.Toolkit.DAL;
using EyouSoft.Toolkit;

namespace EyouSoft.DAL.TourStructure
{
    /// <summary>
    /// 销售管理-订单相关处理DAL
    /// 创建人：luofx 2011-01-19
    /// </summary>
    /// --------------------
    /// 2011-06-17 周文超  增加  GetStatisticOrderList方法
    public class TourOrder : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.TourStructure.ITourOrder
    {
        private EyouSoft.Data.EyouSoftTBL dcDal = null;
        private readonly Database _db = null;

        #region static constants
        //static constants
        private const string SQL_SELECT_GetOrderAmountPlus = "SELECT * FROM [tbl_TourOrderPlus] WHERE [OrderId]=@OrderId";
        private const string SQL_UPDATE_CancelOrder = "UPDATE [tbl_TourOrder] SET [OrderState]=@Status WHERE [Id]=@OrderId;UPDATE [tbl_Tour] SET [VirtualPeopleNumber]=ISNULL((SELECT SUM([PeopleNumber]-[LeaguePepoleNum]) FROM [tbl_TourOrder] WHERE [TourId]=@TourId AND [IsDelete]='0' AND [OrderState] IN(1,3,5)),0) WHERE [TourId]=@TourId;UPDATE [tbl_StatAllIncome] SET [IsDelete]='1' WHERE [ItemId]=@OrderId AND [ItemType]=@ItemType ";
        /// <summary>
        /// 根据订单编号获取可退票游客信息集合commandText
        /// </summary>
        private const string SQL_SELECT_GetCanRefundTicketTravellers = @"SELECT A.[ID],A.[VisitorName],A.[CradType],A.[CradNumber],A.[Sex],A.[VisitorType],A.[ContactTel] FROM [tbl_TourOrderCustomer]AS A
WHERE A.[OrderId]=@OrderId AND EXISTS(SELECT 1 FROM [tbl_PlanTicketOutCustomer] AS B INNER JOIN [tbl_PlanTicketOut] AS C ON B.[TicketOutId]=C.[Id] AND C.[State]=3 WHERE B.[UserId]=A.[Id])";

        /// <summary>
        /// 根据计划编号获取计划下所有订单游客信息集体
        /// </summary>
        private const string SQL_SELECT_GetTravellers = @"SELECT A.[ID],A.[VisitorName],A.[CradType],A.[CradNumber],A.[Sex],A.[VisitorType],A.[ContactTel]
,(SELECT B.[ID] FROM [tbl_PlanTicketFlight] AS B WHERE B.[TicketId] IN(SELECT C.[TicketOutId] FROM [tbl_PlanTicketOutCustomer] AS C WHERE C.[UserId]=A.[Id]) FOR XML RAW,ROOT('root')) AS ApplyFlights
,(SELECT B.[FlightId] FROM [tbl_CustomerRefundFlight] AS B WHERE B.[CustomerId]=A.[Id] FOR XML RAW,ROOT('root')) AS RefundFlights,A.[CustomerStatus],A.OrderId
FROM [tbl_TourOrderCustomer] AS A
WHERE A.OrderId IN(SELECT B.[Id] FROM [tbl_TourOrder] AS B WHERE B.[TourId]=@TourId AND B.[IsDelete]='0' AND B.[OrderState] IN(1,2,5)) ORDER BY A.[IdentityId]";
        /*/// <summary>
        /// 根据计划编号获取计划下所有订单游客信息集体,申请的航段信息含申请机票状态
        /// </summary>
        private const string SQL_SELECT_GetTravellers = @"SELECT A.[ID],A.[VisitorName],A.[CradType],A.[CradNumber],A.[Sex],A.[VisitorType],A.[ContactTel]
,(SELECT B.[ID],D.State FROM [tbl_PlanTicketFlight] AS B INNER JOIN [tbl_PlanTicketOut] AS D 
	ON B.TicketId=D.Id
	WHERE B.[TicketId] IN(SELECT C.[TicketOutId] FROM [tbl_PlanTicketOutCustomer] AS C WHERE C.[UserId]=A.[Id]) FOR XML RAW,ROOT('root')) AS ApplyFlights
,(SELECT B.[FlightId] FROM [tbl_CustomerRefundFlight] AS B WHERE B.[CustomerId]=A.[Id] FOR XML RAW,ROOT('root')) AS RefundFlights
FROM [tbl_TourOrderCustomer]AS A
WHERE A.OrderId IN(SELECT B.[Id] FROM [tbl_TourOrder] AS B WHERE B.[TourId]=@TourId)";*/

        const string SQL_SELECT_GetOrderOtherSideContactInfo = "SELECT B.* FROM tbl_TourOrder AS A INNER JOIN [tbl_CustomerContactInfo] AS B ON A.[BuyerContactId] = B.[Id] WHERE A.[Id]=@OrderId";
        const string SQL_UPDATE_CancelIncomeChecked = "UPDATE [tbl_ReceiveRefund] SET [IsCheck]=0 WHERE [Id]=@Id AND [CompanyID]=@CompanyId AND [ItemId]=@OrderId";
        const string SQL_UPDATE_CancelIncomeTuiChecked = "UPDATE [tbl_ReceiveRefund] SET [IsCheck]=0 WHERE [Id]=@Id AND [CompanyID]=@CompanyId AND [ItemId]=@OrderId";
        const string SQL_SELECT_GetDingDanTuiTuanRenShu = "SELECT A.LeaguePepoleNum AS TuiTuanZongRenShu,(SELECT COUNT(*) FROM tbl_TourOrderCustomer AS B WHERE B.OrderId=A.Id AND B.CustomerStatus=1 AND VisitorType=2) AS TuiTuanErTongShu ,(SELECT SUM(RefundAmount) FROM tbl_CustomerLeague AS B WHERE B.CustormerId IN(SELECT ID FROM tbl_TourOrderCustomer AS C WHERE C.OrderId=A.Id)) AS TuiTuanSunShiJinE FROM tbl_TourOrder AS A WHERE A.Id=@OrderId";
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public TourOrder()
        {
            _db = this.SystemStore;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 根据游客编号获取团号
        /// </summary>
        /// <param name="CustomerId">游客编号</param>
        /// <returns></returns>
        private string GetTourNo(string CustomerId)
        {
            string TourNo = string.Empty;
            string StrSql = "SELECT TourNo FROM tbl_TourOrder Where id=(SELECT OrderId From tbl_TourOrderCustomer WHERE id=@CustomerId)";
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            this._db.AddInParameter(dc, "CustomerId", DbType.AnsiStringFixedLength, CustomerId);
            TourNo = DbHelper.GetSingle(dc, this._db).ToString();
            return TourNo;
        }
        /// <summary>
        /// 设置订单联系人信息
        /// </summary>
        /// <param name="model"></param>
        private void InputContactValue(EyouSoft.Model.TourStructure.TourOrder model, string ContactInfoXML)
        {
            XElement root = XElement.Parse(ContactInfoXML);
            var xRow = root.Element("row");
            if (xRow != null)
            {
                model.ContactMobile = xRow.Attribute("ContactMobile").Value.ToString();
                model.ContactName = xRow.Attribute("ContactName").Value.ToString();
                model.ContactTel = xRow.Attribute("ContactTel").Value.ToString();
            }

        }
        /// <summary>
        /// 根据团队编号获取订单信息集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="TourId">团队编号</param>
        /// <param name="model">帐款信息实体</param>
        /// <param name="IsReceived">是否已结清</param>
        /// <param name="SqlWhere">订单查询条件</param>
        /// <returns></returns>
        private IList<EyouSoft.Model.TourStructure.OrderAboutAccount> GetOrderAboutAccountList(int CompanyId, string TourId, EyouSoft.Model.TourStructure.TourAboutAccountInfo model, bool? IsReceived, string SqlWhere)
        {
            IList<EyouSoft.Model.TourStructure.OrderAboutAccount> OrderAboutList = null;
            EyouSoft.Model.TourStructure.OrderAboutAccount OrderAbout = null;
            string StrSql = string.Format("SELECT [id],RouteName,TourNo,LeaveDate,OrderNo,PeopleNumber,BuyCompanyName,SalerName,FinanceSum,NotCheckMoney,HasCheckMoney,TourClassId,(SELECT Status FROM tbl_Tour WHERE TourId=tbl_TourOrder.TourId) AS TourStatus,FinanceAddExpense,FinanceRedExpense,FinanceRemark,(SELECT SUM(RefundMoney) FROM tbl_ReceiveRefund WHERE ItemId=tbl_TourOrder.ID AND IsReceive=0 AND IsCheck=0) AS NotCheckTuiMoney,BuyerContactName,LeaguePepoleNum,BuyerTourCode FROM tbl_TourOrder WHERE TourId=@TourId AND SellCompanyId={0}  AND IsDelete=0", CompanyId);
            if (!string.IsNullOrEmpty(SqlWhere))
            {
                StrSql += SqlWhere;
            }
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            this._db.AddInParameter(dc, "TourId", DbType.AnsiStringFixedLength, TourId);
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                OrderAboutList = new List<EyouSoft.Model.TourStructure.OrderAboutAccount>();
                while (dr.Read())
                {
                    model.RouteName = dr.IsDBNull(dr.GetOrdinal("RouteName")) ? "" : dr.GetString(dr.GetOrdinal("RouteName"));
                    model.LeaveDate = dr.GetDateTime(dr.GetOrdinal("LeaveDate"));
                    model.TourNo = dr.IsDBNull(dr.GetOrdinal("TourNo")) ? "" : dr.GetString(dr.GetOrdinal("TourNo"));
                    model.TourStatus = (EyouSoft.Model.EnumType.TourStructure.TourStatus)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.TourStatus), dr.GetByte(dr.GetOrdinal("TourStatus")).ToString());
                    model.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.TourType), dr.GetByte(dr.GetOrdinal("TourClassId")).ToString());
                    OrderAbout = new EyouSoft.Model.TourStructure.OrderAboutAccount()
                    {
                        TourId = TourId,
                        CompanyName = dr.IsDBNull(dr.GetOrdinal("BuyCompanyName")) ? "" : dr.GetString(dr.GetOrdinal("BuyCompanyName")),
                        FinanceSum = dr.IsDBNull(dr.GetOrdinal("FinanceSum")) ? 0 : dr.GetDecimal(dr.GetOrdinal("FinanceSum")),
                        HasCheckMoney = dr.IsDBNull(dr.GetOrdinal("HasCheckMoney")) ? 0 : dr.GetDecimal(dr.GetOrdinal("HasCheckMoney")),
                        NotCheckMoney = dr.IsDBNull(dr.GetOrdinal("NotCheckMoney")) ? 0 : dr.GetDecimal(dr.GetOrdinal("NotCheckMoney")),
                        OrderId = dr.GetString(dr.GetOrdinal("id")),
                        OrderNo = dr.IsDBNull(dr.GetOrdinal("OrderNo")) ? "" : dr.GetString(dr.GetOrdinal("OrderNo")),
                        PepoleNum = dr.GetInt32(dr.GetOrdinal("PeopleNumber")) - dr.GetInt32(dr.GetOrdinal("LeaguePepoleNum")),
                        SalerName = dr.IsDBNull(dr.GetOrdinal("SalerName")) ? "" : dr.GetString(dr.GetOrdinal("SalerName")),
                        NotCheckTuiMoney = dr.IsDBNull(dr.GetOrdinal("NotCheckTuiMoney")) ? 0 : dr.GetDecimal(dr.GetOrdinal("NotCheckTuiMoney")),
                        BuyerContactName = dr["BuyerContactName"].ToString(),
                        BuyerTourCode = dr["BuyerTourCode"].ToString()
                    };
                    OrderAbout.NotReciveMoney = OrderAbout.FinanceSum - OrderAbout.HasCheckMoney;
                    OrderAbout.OrderAmountPlusInfo = new TourOrderAmountPlusInfo
                    {
                        AddAmount =
                            dr.IsDBNull(dr.GetOrdinal("FinanceAddExpense"))
                                ? 0
                                : dr.GetDecimal(dr.GetOrdinal("FinanceAddExpense")),
                        ReduceAmount =
                            dr.IsDBNull(dr.GetOrdinal("FinanceRedExpense"))
                                ? 0
                                : dr.GetDecimal(dr.GetOrdinal("FinanceRedExpense")),
                        Remark =
                            dr.IsDBNull(dr.GetOrdinal("FinanceRemark"))
                                ? string.Empty
                                : dr.GetString(dr.GetOrdinal("FinanceRemark"))
                    };

                    OrderAboutList.Add(OrderAbout);

                }
            }
            return OrderAboutList;
        }
        /// <summary>
        /// 生成财务团队结算XML
        /// </summary>
        /// <param name="lists">团队结算订单实体</param>
        /// <returns></returns>
        private string CreateFinanceExpenseXML(IList<EyouSoft.Model.TourStructure.OrderFinanceExpense> lists)
        {
            if (lists == null) return "";
            StringBuilder StrBuild = new StringBuilder();
            StrBuild.Append("<ROOT>");
            foreach (EyouSoft.Model.TourStructure.OrderFinanceExpense model in lists)
            {
                StrBuild.AppendFormat("<OrderFinanceExpense OrderId=\"{0}\"", model.OrderId);
                StrBuild.AppendFormat(" FinanceAddExpense=\"{0}\" ", model.FinanceAddExpense);
                StrBuild.AppendFormat(" FinanceRedExpense=\"{0}\" ", model.FinanceRedExpense);
                StrBuild.AppendFormat(" FinanceSum=\"{0}\" ", model.FinanceSum);
                StrBuild.AppendFormat(" FinanceRemark=\"{0}\"  />", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(model.FinanceRemark));
            }
            StrBuild.Append("</ROOT>");
            return StrBuild.ToString();
        }
        /// <summary>
        /// 生成收退款的XML
        /// </summary>
        /// <param name="lists">收退款信息集合</param>
        /// <param name="IsRecive">1：收款，0：退款</param>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        private string CreateReceiveRefundXML(IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> lists, bool IsRecive, string OrderId)
        {
            if (lists == null) return "";
            StringBuilder StrBuild = new StringBuilder();
            StrBuild.Append("<ROOT>");
            foreach (EyouSoft.Model.FinanceStructure.ReceiveRefund model in lists)
            {
                StrBuild.AppendFormat("<ReceiveRefund id=\"{0}\" CompanyId=\"{1}\"", model.Id, model.CompanyID);
                StrBuild.AppendFormat(" PayCompanyId=\"{0}\" ", model.PayCompanyId);
                StrBuild.AppendFormat(" PayCompanyName=\"{0}\" ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(model.PayCompanyName));
                StrBuild.AppendFormat(" ItemId=\"{0}\" ", OrderId);
                StrBuild.AppendFormat(" ItemType=\"{0}\" ", (int)model.ItemType);
                StrBuild.AppendFormat(" RefundDate=\"{0}\" ", model.RefundDate);
                StrBuild.AppendFormat(" StaffNo=\"{0}\" ", model.StaffNo);
                StrBuild.AppendFormat(" StaffName=\"{0}\" ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(model.StaffName));
                StrBuild.AppendFormat(" RefundMoney=\"{0}\" ", model.RefundMoney);
                StrBuild.AppendFormat(" RefundType=\"{0}\" ", (int)model.RefundType);
                StrBuild.AppendFormat(" IsBill=\"{0}\" ", model.IsBill ? "1" : "0");
                StrBuild.AppendFormat(" BillAmount=\"{0}\" ", model.BillAmount);
                StrBuild.AppendFormat(" IsReceive=\"{0}\" ", IsRecive ? "1" : "0");
                StrBuild.AppendFormat(" IsCheck=\"{0}\" ", model.IsCheck ? "1" : "0");
                StrBuild.AppendFormat(" Remark=\"{0}\" ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(model.Remark));
                StrBuild.AppendFormat(" OperatorID=\"{0}\" ", model.OperatorID);
                StrBuild.AppendFormat(" CheckerId=\"{0}\" />", model.CheckerId);
            }
            StrBuild.Append("</ROOT>");
            return StrBuild.ToString();
        }
        /// <summary>
        /// 生成订单游客的XML
        /// </summary>
        /// <param name="lists"></param>
        /// <returns></returns>
        private string CreateTourOrderCustomerXML(IList<EyouSoft.Model.TourStructure.TourOrderCustomer> lists, string OrderId)
        {
            if (lists == null) return "";
            StringBuilder StrBuild = new StringBuilder();
            StrBuild.Append("<ROOT>");
            foreach (EyouSoft.Model.TourStructure.TourOrderCustomer model in lists)
            {
                string CustormerId = string.Empty;
                string ProjectName = "";
                string ServiceDetail = "";
                bool IsAdd = false;
                decimal Fee = 0;
                if (model.SpecialServiceInfo != null)
                {
                    ProjectName = EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(model.SpecialServiceInfo.ProjectName);
                    ServiceDetail = EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(model.SpecialServiceInfo.ServiceDetail);
                    IsAdd = model.SpecialServiceInfo.IsAdd;
                    Fee = model.SpecialServiceInfo.Fee;
                }
                StrBuild.AppendFormat("<TourOrderCustomer ID=\"{0}\" CompanyId=\"{1}\"", string.IsNullOrEmpty(model.ID) ? System.Guid.NewGuid().ToString() : model.ID, model.CompanyID);
                StrBuild.AppendFormat(" OrderId=\"{0}\" ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(OrderId));
                StrBuild.AppendFormat(" VisitorName=\"{0}\" ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(model.VisitorName));
                StrBuild.AppendFormat(" CradType=\"{0}\" ", (int)model.CradType);
                StrBuild.AppendFormat(" CradNumber=\"{0}\" ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(model.CradNumber));
                StrBuild.AppendFormat(" Sex=\"{0}\" ", (int)model.Sex);
                StrBuild.AppendFormat(" VisitorType=\"{0}\" ", (int)model.VisitorType);
                StrBuild.AppendFormat(" ContactTel=\"{0}\" ", EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(model.ContactTel));
                //StrBuild.AppendFormat(" CustormerId=\"{0}\" ", CustormerId);
                StrBuild.AppendFormat(" ProjectName=\"{0}\" ", ProjectName);
                StrBuild.AppendFormat(" ServiceDetail=\"{0}\" ", ServiceDetail);
                StrBuild.AppendFormat(" IsAdd=\"{0}\" ", IsAdd ? 1 : 0);
                StrBuild.AppendFormat(" IsDelete=\"{0}\"", model.IsDelete ? 1 : 0);
                StrBuild.AppendFormat(" Fee=\"{0}\" />", Fee);
            }
            StrBuild.Append("</ROOT>");
            return StrBuild.ToString();
        }

        /// <summary>
        /// 创建订单金额增加减少费用信息XMLDocument
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private string CreateOrderAmountPlusXML(EyouSoft.Model.TourStructure.TourOrderAmountPlusInfo info)
        {
            //XML:<ROOT><Info AddAmount="增加费用" ReduceAmount="减少费用" Remark="备注" /></ROOT>
            if (info == null) return string.Empty;
            StringBuilder xmlDoc = new StringBuilder();

            xmlDoc.Append("<ROOT>");
            xmlDoc.AppendFormat("<Info AddAmount=\"{0}\" ReduceAmount=\"{1}\" Remark=\"{2}\" />", info.AddAmount
                , info.ReduceAmount
                , EyouSoft.Toolkit.Utils.ReplaceXmlSpecialCharacter(info.Remark));
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 获取订单金额增加减少费用信息业务实体
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        private EyouSoft.Model.TourStructure.TourOrderAmountPlusInfo GetOrderAmountPlus(string orderId)
        {
            EyouSoft.Model.TourStructure.TourOrderAmountPlusInfo info = null;

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetOrderAmountPlus);
            this._db.AddInParameter(cmd, "OrderId", DbType.AnsiStringFixedLength, orderId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    info = new EyouSoft.Model.TourStructure.TourOrderAmountPlusInfo()
                    {
                        AddAmount = rdr.GetDecimal(rdr.GetOrdinal("AddAmount")),
                        ReduceAmount = rdr.GetDecimal(rdr.GetOrdinal("ReduceAmount")),
                        Remark = rdr["Remark"].ToString()
                    };
                }
            }

            return info;
        }

        /// <summary>
        /// 创建团队计划人数及单价信息XMLDocument
        /// </summary>
        /// <param name="info">团队计划人数及单价信息实体</param>
        /// <returns></returns>
        private string CreateTourTeamUnitXml(MTourTeamUnitInfo info)
        {
            if (info == null || info.NumberCr <= 0)
                return string.Empty;

            var xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            xmlDoc.AppendFormat("<Info NumberCr=\"{0}\" ", info.NumberCr);
            xmlDoc.AppendFormat(" NumberEt=\"{0}\" ", info.NumberEt);
            xmlDoc.AppendFormat(" NumberQp=\"{0}\" ", info.NumberQp);
            xmlDoc.AppendFormat(" UnitAmountCr=\"{0}\" ", info.UnitAmountCr);
            xmlDoc.AppendFormat(" UnitAmountEt=\"{0}\" ", info.UnitAmountEt);
            xmlDoc.AppendFormat(" UnitAmountQp=\"{0}\" ", info.UnitAmountQp);
            xmlDoc.Append(" />");
            xmlDoc.Append("</ROOT>");
            return xmlDoc.ToString();
        }

        #endregion 私有方法

        #region 订单信息
        /// <summary>
        /// 获取订单实体信息
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.TourOrder GetOrderModel(int CompanyId, string OrderId)
        {
            StringBuilder StrSql = new StringBuilder();
            EyouSoft.Model.TourStructure.TourOrder model = null;
            StrSql.Append("SELECT [ID],[RouteName],[RouteId],[TourNo],[TourId],TourClassId,[LeaveDate],[LeaveTraffic],[SalerId],[SalerName] ");
            StrSql.Append(",[ReturnTraffic],[OrderState],OrderType,[ContactName],[ContactTel],BuyCompanyID,BuyCompanyName");
            StrSql.Append(",[ContactMobile],[ContactFax],[PriceStandId],[PersonalPrice]");

            StrSql.Append(",[ChildPrice],[MarketPrice],[AdultNumber]");
            StrSql.Append(",[ChildNumber],[MarketNumber],[PeopleNumber],[OtherPrice]");
            StrSql.Append(",[SaveSeatDate],[OperatorContent],[SpecialContent],[SumPrice]");
            StrSql.Append(",[CustomerDisplayType],[CustomerFilePath],CustomerLevId");
            StrSql.Append(",(SELECT (b.PlanPeopleNumber-b.[VirtualPeopleNumber]) AS hasHum from [tbl_Tour] b WHERE b.[TourId]=a.[TourId])AS RemainNum ");
            StrSql.Append(" ,BuyerContactId,BuyerContactName,CommissionType,CommissionPrice,BuyerTourCode ");
            StrSql.AppendFormat(" FROM [dbo].[tbl_TourOrder] a WHERE [ID]='{0}' AND [SellCompanyId]='{1}' AND IsDelete='0'; ", OrderId, CompanyId);
            StrSql.AppendFormat(
                " select TrafficId from tbl_TourOrderTraffic where tbl_TourOrderTraffic.OrderId = '{0}'; ", OrderId);
            DbCommand dc = this.SystemStore.GetSqlStringCommand(StrSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                while (dr.Read())
                {
                    model = new EyouSoft.Model.TourStructure.TourOrder()
                    {
                        ID = dr.GetString(dr.GetOrdinal("id")),
                        RouteName = dr.IsDBNull(dr.GetOrdinal("RouteName")) ? "" : dr.GetString(dr.GetOrdinal("RouteName")),
                        RouteId = dr.IsDBNull(dr.GetOrdinal("RouteId")) ? 0 : dr.GetInt32(dr.GetOrdinal("RouteId")),
                        TourNo = dr.IsDBNull(dr.GetOrdinal("TourNo")) ? "" : dr.GetString(dr.GetOrdinal("TourNo")),
                        TourId = dr.IsDBNull(dr.GetOrdinal("TourId")) ? "" : dr.GetString(dr.GetOrdinal("TourId")),
                        LeaveDate = dr.GetDateTime(dr.GetOrdinal("LeaveDate")),
                        LeaveTraffic = dr.IsDBNull(dr.GetOrdinal("LeaveTraffic")) ? "" : dr.GetString(dr.GetOrdinal("LeaveTraffic")),
                        ReturnTraffic = dr.IsDBNull(dr.GetOrdinal("ReturnTraffic")) ? "" : dr.GetString(dr.GetOrdinal("ReturnTraffic")),
                        OrderState = (EyouSoft.Model.EnumType.TourStructure.OrderState)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.OrderState), dr.GetByte(dr.GetOrdinal("OrderState")).ToString()),
                        ContactName = dr.IsDBNull(dr.GetOrdinal("ContactName")) ? "" : dr.GetString(dr.GetOrdinal("ContactName")),
                        ContactTel = dr.IsDBNull(dr.GetOrdinal("ContactTel")) ? "" : dr.GetString(dr.GetOrdinal("ContactTel")),
                        ContactMobile = dr.IsDBNull(dr.GetOrdinal("ContactMobile")) ? "" : dr.GetString(dr.GetOrdinal("ContactMobile")),
                        ContactFax = dr.IsDBNull(dr.GetOrdinal("ContactFax")) ? "" : dr.GetString(dr.GetOrdinal("ContactFax")),
                        PriceStandId = dr.GetInt32(dr.GetOrdinal("PriceStandId")),
                        PersonalPrice = dr.IsDBNull(dr.GetOrdinal("PersonalPrice")) ? 0 : dr.GetDecimal(dr.GetOrdinal("PersonalPrice")),
                        ChildPrice = dr.IsDBNull(dr.GetOrdinal("ChildPrice")) ? 0 : dr.GetDecimal(dr.GetOrdinal("ChildPrice")),
                        MarketPrice = dr.IsDBNull(dr.GetOrdinal("MarketPrice")) ? 0 : dr.GetDecimal(dr.GetOrdinal("MarketPrice")),
                        AdultNumber = dr.IsDBNull(dr.GetOrdinal("AdultNumber")) ? 0 : dr.GetInt32(dr.GetOrdinal("AdultNumber")),
                        ChildNumber = dr.IsDBNull(dr.GetOrdinal("ChildNumber")) ? 0 : dr.GetInt32(dr.GetOrdinal("ChildNumber")),
                        MarketNumber = dr.IsDBNull(dr.GetOrdinal("MarketNumber")) ? 0 : dr.GetInt32(dr.GetOrdinal("MarketNumber")),
                        PeopleNumber = dr.IsDBNull(dr.GetOrdinal("PeopleNumber")) ? 0 : dr.GetInt32(dr.GetOrdinal("PeopleNumber")),
                        OtherPrice = dr.IsDBNull(dr.GetOrdinal("OtherPrice")) ? 0 : dr.GetDecimal(dr.GetOrdinal("OtherPrice")),
                        SaveSeatDate = dr.IsDBNull(dr.GetOrdinal("SaveSeatDate")) ? System.DateTime.Now : dr.GetDateTime(dr.GetOrdinal("SaveSeatDate")),
                        OperatorContent = dr.IsDBNull(dr.GetOrdinal("OperatorContent")) ? "" : dr.GetString(dr.GetOrdinal("OperatorContent")),
                        SpecialContent = dr.IsDBNull(dr.GetOrdinal("SpecialContent")) ? "" : dr.GetString(dr.GetOrdinal("SpecialContent")),
                        SumPrice = dr.IsDBNull(dr.GetOrdinal("SumPrice")) ? 0 : dr.GetDecimal(dr.GetOrdinal("SumPrice")),
                        CustomerDisplayType = (EyouSoft.Model.EnumType.TourStructure.CustomerDisplayType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.CustomerDisplayType), dr.GetByte(dr.GetOrdinal("CustomerDisplayType")).ToString()),
                        CustomerFilePath = dr.IsDBNull(dr.GetOrdinal("CustomerFilePath")) ? "" : dr.GetString(dr.GetOrdinal("CustomerFilePath")),
                        OrderType = (EyouSoft.Model.EnumType.TourStructure.OrderType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.OrderType), dr.GetByte(dr.GetOrdinal("OrderType")).ToString()),
                        TourClassId = (EyouSoft.Model.EnumType.TourStructure.TourType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.TourType), dr.GetByte(dr.GetOrdinal("TourClassId")).ToString()),
                        CustomerLevId = dr.IsDBNull(dr.GetOrdinal("CustomerLevId")) ? 0 : dr.GetInt32(dr.GetOrdinal("CustomerLevId")),
                        BuyCompanyID = dr.IsDBNull(dr.GetOrdinal("BuyCompanyID")) ? 0 : dr.GetInt32(dr.GetOrdinal("BuyCompanyID")),
                        BuyCompanyName = dr.IsDBNull(dr.GetOrdinal("BuyCompanyName")) ? "" : dr.GetString(dr.GetOrdinal("BuyCompanyName")),
                        RemainNum = dr.IsDBNull(dr.GetOrdinal("RemainNum")) ? 0 : dr.GetInt32(dr.GetOrdinal("RemainNum")),
                        SalerId = dr.IsDBNull(dr.GetOrdinal("SalerId")) ? 0 : dr.GetInt32(dr.GetOrdinal("SalerId")),
                        SalerName = dr.IsDBNull(dr.GetOrdinal("SalerName")) ? string.Empty : dr.GetString(dr.GetOrdinal("SalerName")),
                        BuyerContactId = dr.GetInt32(dr.GetOrdinal("BuyerContactId")),
                        BuyerContactName = dr["BuyerContactName"].ToString(),
                        CommissionType = (EyouSoft.Model.EnumType.CompanyStructure.CommissionType)dr.GetByte(dr.GetOrdinal("CommissionType")),
                        CommissionPrice = dr.GetDecimal(dr.GetOrdinal("CommissionPrice")),
                        BuyerTourCode = dr["BuyerTourCode"].ToString()
                    };
                }

                dr.NextResult();

                if (dr.Read())
                {
                    if (!dr.IsDBNull(dr.GetOrdinal("TrafficId")) && model != null)
                        model.OrderTrafficId = dr.GetInt32(dr.GetOrdinal("TrafficId"));
                }
            }

            if (model != null)
            {
                model.CustomerList = GetCustomerList(CompanyId, OrderId, 0);
                model.AmountPlus = this.GetOrderAmountPlus(model.ID);
            }

            return model;
        }
        /// <summary>
        /// 获取订单中心订单信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">订单中心查询实体</param>
        /// <param name="HaveUserIds">根据部门Id生成的用户Id字符串</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, EyouSoft.Model.TourStructure.OrderCenterSearchInfo SearchModel, string HaveUserIds)
        {
            EyouSoft.Model.TourStructure.TourOrder model = null;
            IList<EyouSoft.Model.TourStructure.TourOrder> ResultList = null;
            #region Sql相关处理
            StringBuilder fields = new StringBuilder();
            fields.Append(" [ID],[TourNo],TourId,[RouteName],[LeaveDate],[OrderNo],[OrderType],[OperatorName],[PeopleNumber],[BuyCompanyName],[OrderState],TourClassId,[BuyCompanyID] ");
            //fields.Append(",(CASE(tbl_TourOrder.[OrderType]) ");
            //fields.Append(" WHEN 0 THEN ");
            //fields.Append(" (SELECT ISNULL(ContactName,'') AS ContactName,ISNULL(ContactTel,'') AS ContactTel,ISNULL(ContactMobile,'') AS ContactMobile FROM tbl_CompanyUser WHERE ID=tbl_TourOrder.OperatorId FOR XML RAW,ROOT('ROOT'))");
            //fields.Append(" ELSE ");
            //fields.Append(" (SELECT ISNULL(ContactName,'') AS ContactName,ISNULL(Phone,'') AS ContactTel,ISNULL(mobile,'') AS ContactMobile FROM tbl_Customer WHERE ID=tbl_TourOrder.BuyCompanyID FOR XML RAW,ROOT('ROOT'))  ");
            //fields.Append(" END  ) AS ContactXML");
            fields.Append(",(SELECT ISNULL(ContactName,'') AS ContactName,ISNULL(Phone,'') AS ContactTel,ISNULL(mobile,'') AS ContactMobile FROM tbl_Customer WHERE ID=tbl_TourOrder.BuyCompanyID FOR XML RAW,ROOT('ROOT')) AS ContactXML ");
            fields.Append(",(SELECT ReleaseType FROM tbl_tour WHERE TourId=tbl_TourOrder.TourId ) AS ReleaseType");
            fields.Append(",SaveSeatDate");
            fields.Append(",LeaguePepoleNum");
            fields.Append(
                " ,(select count(*) from tbl_PlanTicketOut as b where b.OrderId = tbl_TourOrder.Id) as IsTicket ");
            string TableName = "tbl_TourOrder";
            string orderByString = " [IssueTime] DESC";
            string identityColumnName = "id";
            StringBuilder Query = new StringBuilder();
            Query.AppendFormat(" SellCompanyId={0}  AND IsDelete=0 AND (TourClassId=0 OR TourClassId=1)", CompanyId);
            if (!string.IsNullOrEmpty(HaveUserIds))
            {
                Query.AppendFormat(" AND ViewOperatorId IN({0}) ", HaveUserIds);
            }
            if (!string.IsNullOrEmpty(SearchModel.OrderNo))
            {
                Query.AppendFormat(" AND OrderNo LIKE '%{0}%'", SearchModel.OrderNo);
            }
            if (SearchModel.OrderState != null)
            {
                Query.AppendFormat(" AND OrderState={0}", (int)SearchModel.OrderState);
            }
            if (!string.IsNullOrEmpty(SearchModel.TourNo))
            {
                Query.AppendFormat(" AND TourNo LIKE '%{0}%'", SearchModel.TourNo);
            }
            if (!string.IsNullOrEmpty(SearchModel.CompanyName))
            {
                Query.AppendFormat(" AND BuyCompanyName LIKE '%{0}%'", SearchModel.CompanyName);
            }
            if (SearchModel.LeaveDateFrom.HasValue)
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,'{0}',LeaveDate)>=0", SearchModel.LeaveDateFrom);
            }
            if (SearchModel.LeaveDateTo.HasValue)
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')>=0", SearchModel.LeaveDateTo);
            }
            if (SearchModel.CreateDateFrom.HasValue)
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,'{0}',IssueTime)>=0", SearchModel.CreateDateFrom);
            }
            if (SearchModel.CreateDateTo.HasValue)
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,IssueTime,'{0}')>=0", SearchModel.CreateDateTo);
            }
            if (SearchModel.OperatorId != null && SearchModel.OperatorId.Length > 0)
            {
                Query.AppendFormat(" AND OperatorId in({0})", this.ConvertIntArrayTostring(SearchModel.OperatorId));
            }
            if (!string.IsNullOrEmpty(SearchModel.OrderId))
            {
                Query.AppendFormat(" AND Id='{0}' ", SearchModel.OrderId);
            }

            #endregion Sql相关处理
            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, TableName, identityColumnName, fields.ToString(), Query.ToString(), orderByString))
            {
                ResultList = new List<EyouSoft.Model.TourStructure.TourOrder>();
                while (dr.Read())
                {
                    model = new EyouSoft.Model.TourStructure.TourOrder()
                        {
                            ID = dr.GetString(dr.GetOrdinal("id")),
                            OrderNo = dr.GetString(dr.GetOrdinal("OrderNo")),
                            TourId = dr.IsDBNull(dr.GetOrdinal("TourId")) ? "" : dr.GetString(dr.GetOrdinal("TourId")),
                            RouteName =
                                dr.IsDBNull(dr.GetOrdinal("RouteName")) ? "" : dr.GetString(dr.GetOrdinal("RouteName")),
                            TourNo = dr.IsDBNull(dr.GetOrdinal("TourNo")) ? "" : dr.GetString(dr.GetOrdinal("TourNo")),
                            LeaveDate = dr.GetDateTime(dr.GetOrdinal("LeaveDate")),
                            PeopleNumber =
                                dr.IsDBNull(dr.GetOrdinal("PeopleNumber"))
                                    ? 0
                                    : dr.GetInt32(dr.GetOrdinal("PeopleNumber"))
                                      - dr.GetInt32(dr.GetOrdinal("LeaguePepoleNum")),
                            OrderState =
                                (EyouSoft.Model.EnumType.TourStructure.OrderState)
                                Enum.Parse(
                                    typeof(EyouSoft.Model.EnumType.TourStructure.OrderState),
                                    dr.GetByte(dr.GetOrdinal("OrderState")).ToString()),
                            OperatorName =
                                dr.IsDBNull(dr.GetOrdinal("OperatorName"))
                                    ? ""
                                    : dr.GetString(dr.GetOrdinal("OperatorName")),
                            BuyCompanyName =
                                dr.IsDBNull(dr.GetOrdinal("BuyCompanyName"))
                                    ? ""
                                    : dr.GetString(dr.GetOrdinal("BuyCompanyName")),
                            OrderType =
                                (EyouSoft.Model.EnumType.TourStructure.OrderType)
                                Enum.Parse(
                                    typeof(EyouSoft.Model.EnumType.TourStructure.OrderType),
                                    dr.GetByte(dr.GetOrdinal("OrderType")).ToString()),
                            TourClassId =
                                (EyouSoft.Model.EnumType.TourStructure.TourType)
                                Enum.Parse(
                                    typeof(EyouSoft.Model.EnumType.TourStructure.TourType),
                                    dr.GetByte(dr.GetOrdinal("TourClassId")).ToString()),
                            ReleaseType =
                                (EyouSoft.Model.EnumType.TourStructure.ReleaseType)
                                Enum.Parse(
                                    typeof(EyouSoft.Model.EnumType.TourStructure.ReleaseType),
                                    dr.GetString(dr.GetOrdinal("ReleaseType"))),
                            SaveSeatDate = dr.GetDateTime(dr.GetOrdinal("SaveSeatDate")),
                            BuyCompanyID =
                                dr.IsDBNull(dr.GetOrdinal("BuyCompanyID"))
                                    ? 0
                                    : dr.GetInt32(dr.GetOrdinal("BuyCompanyID")),
                            IsExtsisTicket =
                                dr.IsDBNull(dr.GetOrdinal("IsTicket"))
                                    ? false
                                    : (dr.GetInt32(dr.GetOrdinal("IsTicket")) > 0 ? true : false)
                        };
                    if (!dr.IsDBNull(dr.GetOrdinal("ContactXML")))
                    {
                        this.InputContactValue(model, dr.GetString(dr.GetOrdinal("ContactXML")));
                    }
                    ResultList.Add(model);
                    model = null;
                }
            }
            return ResultList;
        }

        /// <summary>
        /// 根据出纳登记编号获取相关订单列表数据
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="CashierRegisterId">出纳登记Id</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, int CashierRegisterId, int BuyCompanyId, string HaveUserIds)
        {
            EyouSoft.Model.TourStructure.TourOrder model = null;
            IList<EyouSoft.Model.TourStructure.TourOrder> ResultList = null;
            #region Sql相关处理
            StringBuilder fields = new StringBuilder();
            fields.Append(" [ID],[TourNo],TourId,[RouteName],[LeaveDate],[OrderNo],[BuyCompanyName]  ");
            fields.Append(",[PeopleNumber],[SalerName],[SumPrice],[HasCheckMoney],[NotCheckMoney],FinanceSum");
            string TableName = "tbl_TourOrder";
            string orderByString = " [IssueTime] DESC";
            string identityColumnName = "id";
            StringBuilder Query = new StringBuilder();
            Query.AppendFormat(" SellCompanyId={0}  AND IsDelete=0 AND (FinanceSum-HasCheckMoney-NotCheckMoney)>0 ", CompanyId);
            if (!string.IsNullOrEmpty(HaveUserIds))
            {
                Query.AppendFormat(" AND ViewOperatorId IN({0}) ", HaveUserIds);
            }
            if (BuyCompanyId > 0)
                Query.AppendFormat(" and BuyCompanyID = {0} ", BuyCompanyId);
            if (CashierRegisterId > 0)
            {
                Query.AppendFormat(" AND exists(SELECT CustomerId FROM tbl_CashierRegister WHERE Id={0} AND (CustomerId=tbl_TourOrder.BuyCompanyId OR CustomerId=0))", CashierRegisterId);
            }
            #endregion Sql相关处理
            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, TableName, identityColumnName, fields.ToString(), Query.ToString(), orderByString))
            {
                ResultList = new List<EyouSoft.Model.TourStructure.TourOrder>();
                while (dr.Read())
                {
                    model = new EyouSoft.Model.TourStructure.TourOrder();
                    model.ID = dr.GetString(dr.GetOrdinal("id"));
                    model.RouteName = dr.IsDBNull(dr.GetOrdinal("RouteName")) ? "" : dr.GetString(dr.GetOrdinal("RouteName"));
                    model.TourNo = dr.IsDBNull(dr.GetOrdinal("TourNo")) ? "" : dr.GetString(dr.GetOrdinal("TourNo"));
                    model.TourId = dr.IsDBNull(dr.GetOrdinal("TourId")) ? "" : dr.GetString(dr.GetOrdinal("TourId"));
                    model.OrderNo = dr.IsDBNull(dr.GetOrdinal("OrderNo")) ? "" : dr.GetString(dr.GetOrdinal("OrderNo"));
                    model.LeaveDate = dr.GetDateTime(dr.GetOrdinal("LeaveDate"));
                    model.PeopleNumber = dr.IsDBNull(dr.GetOrdinal("PeopleNumber")) ? 0 : dr.GetInt32(dr.GetOrdinal("PeopleNumber"));
                    model.SalerName = dr.IsDBNull(dr.GetOrdinal("SalerName")) ? "" : dr.GetString(dr.GetOrdinal("SalerName"));
                    model.SumPrice = dr.IsDBNull(dr.GetOrdinal("SumPrice")) ? 0 : dr.GetDecimal(dr.GetOrdinal("SumPrice"));
                    model.FinanceSum = dr.IsDBNull(dr.GetOrdinal("FinanceSum")) ? 0 : dr.GetDecimal(dr.GetOrdinal("FinanceSum"));
                    model.HasCheckMoney = dr.IsDBNull(dr.GetOrdinal("HasCheckMoney")) ? 0 : dr.GetDecimal(dr.GetOrdinal("HasCheckMoney"));
                    model.NotCheckMoney = dr.IsDBNull(dr.GetOrdinal("NotCheckMoney")) ? 0 : dr.GetDecimal(dr.GetOrdinal("NotCheckMoney"));
                    model.BuyCompanyName = dr.IsDBNull(dr.GetOrdinal("BuyCompanyName")) ? "" : dr.GetString(dr.GetOrdinal("BuyCompanyName"));
                    model.NotReceived = model.SumPrice - model.HasCheckMoney;
                    ResultList.Add(model);
                    model = null;
                }
            }
            return ResultList;
        }
        /// <summary>
        /// 获取团队结算订单信息集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="TourId">团队编号</param>
        /// <param name="HaveUserIds">根据部门Id生成的用户Id字符串</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.OrderFinanceExpense> GetOrderList(int CompanyId, string TourId, string HaveUserIds)
        {
            EyouSoft.Model.TourStructure.OrderFinanceExpense model = null;
            IList<EyouSoft.Model.TourStructure.OrderFinanceExpense> ResultList = null;
            #region Sql相关处理
            StringBuilder StrSql = new StringBuilder();
            StrSql.Append("SELECT [ID],OrderNo,[FinanceAddExpense],[FinanceRedExpense],[FinanceSum],[FinanceRemark],[SumPrice],[BuyCompanyName] AS CompanyName ");
            StrSql.Append(" FROM [tbl_TourOrder] WHERE ");
            StrSql.AppendFormat(" SellCompanyId={0}  AND IsDelete=0  AND (OrderState=1 OR OrderState=2 OR OrderState=5)", CompanyId);
            if (!string.IsNullOrEmpty(HaveUserIds))
            {
                StrSql.AppendFormat(" AND ViewOperatorId IN({0}) ", HaveUserIds);
            }

            if (!string.IsNullOrEmpty(TourId))
            {
                StrSql.AppendFormat(" AND TourId='{0}'", TourId);
            }
            StrSql.Append(" Order By [IssueTime] DESC ");
            #endregion Sql相关处理
            DbCommand dc = this._db.GetSqlStringCommand(StrSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                ResultList = new List<EyouSoft.Model.TourStructure.OrderFinanceExpense>();
                while (dr.Read())
                {
                    model = new EyouSoft.Model.TourStructure.OrderFinanceExpense()
                    {
                        OrderId = dr.GetString(dr.GetOrdinal("id")),
                        OrderNo = dr.GetString(dr.GetOrdinal("OrderNo")),
                        FinanceAddExpense = dr.IsDBNull(dr.GetOrdinal("FinanceAddExpense")) ? 0 : dr.GetDecimal(dr.GetOrdinal("FinanceAddExpense")),
                        FinanceRedExpense = dr.IsDBNull(dr.GetOrdinal("FinanceRedExpense")) ? 0 : dr.GetDecimal(dr.GetOrdinal("FinanceRedExpense")),
                        FinanceSum = dr.GetDecimal(dr.GetOrdinal("FinanceSum")),
                        FinanceRemark = dr.IsDBNull(dr.GetOrdinal("FinanceRemark")) ? "" : dr.GetString(dr.GetOrdinal("FinanceRemark")),
                        SumPrice = dr.IsDBNull(dr.GetOrdinal("SumPrice")) ? 0 : dr.GetDecimal(dr.GetOrdinal("SumPrice")),
                        CompanyName = dr.IsDBNull(dr.GetOrdinal("CompanyName")) ? "" : dr.GetString(dr.GetOrdinal("CompanyName"))
                    };
                    ResultList.Add(model);
                    model = null;
                }
            }
            return ResultList;
        }
        /// <summary>
        /// 统计分析-员工业绩订单详情
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="SearchInfo">搜索条件实体</param>
        /// <param name="haveUserIds">用户Ids</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderList(int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.StatisticStructure.QueryPersonnelStatistic SearchInfo, string haveUserIds)
        {
            EyouSoft.Model.TourStructure.TourOrder model = null;
            IList<EyouSoft.Model.TourStructure.TourOrder> ResultList = null;
            #region Sql相关处理
            string fields = "[ID],[TourNo],[RouteName],[LeaveDate],[OrderNo],[BuyCompanyName],[PeopleNumber],[SumPrice],FinanceSum,TourClassId";
            string TableName = "tbl_TourOrder";
            string orderByString = " [IssueTime] DESC";
            string identityColumnName = "id";
            StringBuilder Query = new StringBuilder();
            Query.AppendFormat(" [SellCompanyId] = {0}  AND IsDelete = '0' ", SearchInfo.CompanyId);
            if (SearchInfo.ComputeOrderType == Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单)
                Query.AppendFormat(" and OrderState = {0} ", (int)Model.EnumType.TourStructure.OrderState.已成交);
            else
                Query.AppendFormat(" and OrderState in ({0},{1},{2}) ", (int)Model.EnumType.TourStructure.OrderState.未处理,
                                   (int)Model.EnumType.TourStructure.OrderState.已成交,
                                   (int)Model.EnumType.TourStructure.OrderState.已留位);
            if (SearchInfo.LogisticsIds != null && SearchInfo.LogisticsIds.Length > 0)
            {
                Query.AppendFormat(" AND TourId IN(SELECT [TourId] FROM tbl_TourOperator WHERE OperatorId IN ({0}) ) ", this.ConvertIntArrayTostring(SearchInfo.LogisticsIds));
            }
            if (SearchInfo.DepartIds != null && SearchInfo.DepartIds.Length > 0)
            {
                Query.AppendFormat(" AND SalerId IN (SELECT [id] FROM tbl_CompanyUser WHERE  DepartId IN ({0}))", this.ConvertIntArrayTostring(SearchInfo.DepartIds));
            }
            if (SearchInfo.SaleIds != null && SearchInfo.SaleIds.Length > 0)
            {
                Query.AppendFormat(" AND SalerId IN ({0})", this.ConvertIntArrayTostring(SearchInfo.SaleIds));
            }
            if (SearchInfo.CheckDateStart.HasValue)
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,'{0}',IssueTime)>=0", SearchInfo.CheckDateStart);
            }
            if (SearchInfo.CheckDateEnd.HasValue)
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,IssueTime,'{0}')>=0", SearchInfo.CheckDateEnd);
            }
            if (SearchInfo.LeaveDateStart.HasValue)
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,'{0}',LeaveDate)>=0", SearchInfo.LeaveDateStart);
            }
            if (SearchInfo.LeaveDateEnd.HasValue)
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')>=0", SearchInfo.LeaveDateEnd);
            }

            if (!string.IsNullOrEmpty(haveUserIds))
            {
                Query.AppendFormat(
                    " and TourId in (select TourId from tbl_Tour where companyid = {0} AND IsDelete='0' AND TemplateId>'' and operatorId in ({1}) ",
                    SearchInfo.CompanyId, haveUserIds);
                if (SearchInfo.LeaveDateStart.HasValue)
                    Query.AppendFormat(" AND DATEDIFF(DAY,'{0}',LeaveDate)>=0 ", SearchInfo.LeaveDateStart);
                if (SearchInfo.LeaveDateEnd.HasValue)
                    Query.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')>=0 ", SearchInfo.LeaveDateEnd);

                Query.Append(" ) ");
            }


            #endregion Sql相关处理
            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, TableName, identityColumnName, fields, Query.ToString(), orderByString))
            {
                ResultList = new List<EyouSoft.Model.TourStructure.TourOrder>();
                while (dr.Read())
                {
                    model = new EyouSoft.Model.TourStructure.TourOrder();
                    model.ID = dr.GetString(dr.GetOrdinal("id"));
                    model.TourNo = dr.IsDBNull(dr.GetOrdinal("TourNo")) ? "" : dr.GetString(dr.GetOrdinal("TourNo"));
                    model.RouteName = dr.IsDBNull(dr.GetOrdinal("RouteName")) ? "" : dr.GetString(dr.GetOrdinal("RouteName"));
                    model.LeaveDate = dr.GetDateTime(dr.GetOrdinal("LeaveDate"));
                    model.OrderNo = dr.IsDBNull(dr.GetOrdinal("OrderNo")) ? "" : dr.GetString(dr.GetOrdinal("OrderNo"));
                    model.PeopleNumber = dr.IsDBNull(dr.GetOrdinal("PeopleNumber")) ? 0 : dr.GetInt32(dr.GetOrdinal("PeopleNumber"));
                    model.SumPrice = dr.IsDBNull(dr.GetOrdinal("SumPrice")) ? 0 : dr.GetDecimal(dr.GetOrdinal("SumPrice"));
                    model.FinanceSum = dr.IsDBNull(dr.GetOrdinal("FinanceSum")) ? 0 : dr.GetDecimal(dr.GetOrdinal("FinanceSum"));
                    model.BuyCompanyName = dr.IsDBNull(dr.GetOrdinal("BuyCompanyName")) ? "" : dr.GetString(dr.GetOrdinal("BuyCompanyName"));
                    model.TourClassId = (EyouSoft.Model.EnumType.TourStructure.TourType)dr.GetByte(dr.GetOrdinal("TourClassId"));
                    ResultList.Add(model);
                    model = null;
                }
            }
            return ResultList;
        }

        /// <summary>
        /// 获取组团端订单中心订单信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">订单中心查询实体</param>
        /// <param name="HaveUserIds">根据部门Id生成的用户Id字符串</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetTourOrderList(int PageSize, int PageIndex, ref int RecordCount, int CompanyId, EyouSoft.Model.TourStructure.TourOrderSearchInfo SearchModel)
        {
            EyouSoft.Model.TourStructure.TourOrder model = null;
            IList<EyouSoft.Model.TourStructure.TourOrder> ResultList = null;
            #region Sql相关处理
            StringBuilder fields = new StringBuilder();
            fields.Append(" [ID],[RouteName],TourId,[LeaveDate],[OrderNo],OrderType,IssueTime,AdultNumber,ChildNumber,[OrderState],BuyCompanyId,[PersonalPrice],[ChildPrice],[SaveSeatDate]");
            string TableName = "tbl_TourOrder";
            string orderByString = " [IssueTime] DESC";
            string identityColumnName = "id";
            StringBuilder Query = new StringBuilder();
            Query.AppendFormat(" BuyCompanyId={0} ", CompanyId);
            Query.Append(" AND IsDelete='0' AND (TourClassId=1 OR TourClassId=0)");
            if (!string.IsNullOrEmpty(SearchModel.OrderNo))
            {
                Query.AppendFormat(" AND OrderNo LIKE '%{0}%'", SearchModel.OrderNo);
            }
            if (!string.IsNullOrEmpty(SearchModel.RouteName))
            {
                Query.AppendFormat(" AND RouteName LIKE '%{0}%'", SearchModel.RouteName);
            }
            if (SearchModel.LeaveDateFrom.HasValue)
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,'{0}',LeaveDate)>=0", SearchModel.LeaveDateFrom);
            }
            if (SearchModel.LeaveDateTo.HasValue)
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')>=0", SearchModel.LeaveDateTo);
            }
            if (SearchModel.CreateDateFrom.HasValue)
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,'{0}',IssueTime)>=0", SearchModel.CreateDateFrom);
            }
            if (SearchModel.CreateDateTo.HasValue)
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,IssueTime,'{0}')>=0", SearchModel.CreateDateTo);
            }
            if (SearchModel.AreaId.HasValue && SearchModel.AreaId.Value > 0)
            {
                Query.AppendFormat(" AND AreaId={0}", SearchModel.AreaId.Value);
            }
            #endregion Sql相关处理
            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, TableName, identityColumnName, fields.ToString(), Query.ToString(), orderByString))
            {
                ResultList = new List<EyouSoft.Model.TourStructure.TourOrder>();
                while (dr.Read())
                {
                    model = new EyouSoft.Model.TourStructure.TourOrder()
                    {
                        ID = dr.GetString(dr.GetOrdinal("id")),
                        TourId = dr.IsDBNull(dr.GetOrdinal("TourId")) ? "" : dr.GetString(dr.GetOrdinal("TourId")),
                        OrderNo = dr.IsDBNull(dr.GetOrdinal("OrderNo")) ? "" : dr.GetString(dr.GetOrdinal("OrderNo")),
                        RouteName = dr.IsDBNull(dr.GetOrdinal("RouteName")) ? "" : dr.GetString(dr.GetOrdinal("RouteName")),
                        LeaveDate = dr.GetDateTime(dr.GetOrdinal("LeaveDate")),
                        AdultNumber = dr.IsDBNull(dr.GetOrdinal("AdultNumber")) ? 0 : dr.GetInt32(dr.GetOrdinal("AdultNumber")),
                        ChildNumber = dr.IsDBNull(dr.GetOrdinal("ChildNumber")) ? 0 : dr.GetInt32(dr.GetOrdinal("ChildNumber")),
                        OrderState = (EyouSoft.Model.EnumType.TourStructure.OrderState)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.OrderState), dr.GetByte(dr.GetOrdinal("OrderState")).ToString()),
                        BuyCompanyID = dr.IsDBNull(dr.GetOrdinal("BuyCompanyId")) ? 0 : dr.GetInt32(dr.GetOrdinal("BuyCompanyId")),
                        OrderType = (EyouSoft.Model.EnumType.TourStructure.OrderType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.OrderType), dr.GetByte(dr.GetOrdinal("OrderType")).ToString()),
                        PersonalPrice = dr.GetDecimal(dr.GetOrdinal("PersonalPrice")),
                        ChildPrice = dr.GetDecimal(dr.GetOrdinal("ChildPrice")),
                        IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime")),
                        SaveSeatDate = dr.IsDBNull(dr.GetOrdinal("SaveSeatDate")) ? System.DateTime.MinValue : dr.GetDateTime(dr.GetOrdinal("SaveSeatDate"))
                    };
                    model.PeopleNumber = model.AdultNumber + model.ChildNumber;
                    ResultList.Add(model);
                    model = null;
                }
            }
            return ResultList;
        }

        /// <summary>
        /// 获取财务管理-团款收入-信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="SearchInfo">财务订单帐款搜索实体</param>
        /// <param name="IsReceived">是否已结清</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourAboutAccountInfo> GetTourReciveAccountList(int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.TourStructure.OrderAboutAccountSearchInfo SearchInfo, bool? IsReceived, string us)
        {
            EyouSoft.Model.TourStructure.TourAboutAccountInfo model = null;
            IList<EyouSoft.Model.TourStructure.TourAboutAccountInfo> ResultList = null;
            #region Sql相关处理
            string fields = "distinct TourId,TourType";
            string TableName = "tbl_Tour";
            string orderByString = " [LeaveDate] ASC";
            string identityColumnName = "TourId";
            StringBuilder Query = new StringBuilder();
            StringBuilder strOrderQuery = new StringBuilder();

            Query.Append("  IsDelete='0' ");

            if (IsReceived.HasValue)
            {
                if (IsReceived.Value)
                {
                    Query.Append(" AND IsSettleInCome=1");
                }
                else
                {
                    Query.Append(" AND IsSettleInCome=0");
                }
            }

            Query.AppendFormat(" AND tourid in(SELECT tourid from tbl_tourorder WHERE SellCompanyId={0}", SearchInfo.CompanyId);
            if (!string.IsNullOrEmpty(SearchInfo.OrderNo))
            {
                Query.AppendFormat(" AND OrderNo LIKE '%{0}%'", SearchInfo.OrderNo);
                strOrderQuery.AppendFormat(" AND OrderNo LIKE '%{0}%'", SearchInfo.OrderNo);
            }
            if (!string.IsNullOrEmpty(SearchInfo.TourNo))
            {
                Query.AppendFormat(" AND TourNo LIKE '%{0}%'", SearchInfo.TourNo);
                strOrderQuery.AppendFormat(" AND TourNo LIKE '%{0}%'", SearchInfo.TourNo);
            }
            if (!string.IsNullOrEmpty(SearchInfo.CompanyName))
            {
                Query.AppendFormat(" AND BuyCompanyName LIKE '%{0}%'", SearchInfo.CompanyName);
                strOrderQuery.AppendFormat(" AND BuyCompanyName LIKE '%{0}%'", SearchInfo.CompanyName);
            }

            if (SearchInfo.LeaveDateFrom.HasValue)
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,'{0}',LeaveDate)>=0", SearchInfo.LeaveDateFrom);
                strOrderQuery.AppendFormat(" AND DATEDIFF(DAY,'{0}',LeaveDate)>=0", SearchInfo.LeaveDateFrom);
            }
            if (SearchInfo.LeaveDateTo.HasValue)
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')>=0", SearchInfo.LeaveDateTo);
                strOrderQuery.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')>=0", SearchInfo.LeaveDateTo);
            }
            /*if (SearchInfo.CreateDateFrom.HasValue)
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,'{0}',IssueTime)>=0", SearchInfo.CreateDateFrom);
                strOrderQuery.AppendFormat(" AND DATEDIFF(DAY,'{0}',IssueTime)>=0", SearchInfo.CreateDateFrom);
            }
            if (SearchInfo.CreateDateTo.HasValue)
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,IssueTime,'{0}')>=0", SearchInfo.CreateDateTo);
                strOrderQuery.AppendFormat(" AND DATEDIFF(DAY,IssueTime,'{0}')>=0", SearchInfo.CreateDateTo);
            }*/

            if (SearchInfo.CreateDateFrom.HasValue || SearchInfo.CreateDateTo.HasValue)
            {
                Query.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_ReceiveRefund WHERE ItemId=tbl_TourOrder.Id AND IsReceive=1 AND ItemType={0} ", (int)EyouSoft.Model.EnumType.TourStructure.ItemType.订单);
                strOrderQuery.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_ReceiveRefund WHERE ItemId=tbl_TourOrder.Id AND IsReceive=1 AND ItemType={0} ", (int)EyouSoft.Model.EnumType.TourStructure.ItemType.订单);
                if (SearchInfo.CreateDateFrom.HasValue)
                {
                    Query.AppendFormat(" AND RefundDate>='{0}' ", SearchInfo.CreateDateFrom.Value);
                    strOrderQuery.AppendFormat(" AND RefundDate>='{0}' ", SearchInfo.CreateDateFrom.Value);
                }
                if (SearchInfo.CreateDateTo.HasValue)
                {
                    Query.AppendFormat(" AND RefundDate<='{0}' ", SearchInfo.CreateDateTo.Value);
                    strOrderQuery.AppendFormat(" AND RefundDate<='{0}' ", SearchInfo.CreateDateTo.Value);
                }

                Query.Append(")");
                strOrderQuery.Append(")");
            }

            if (SearchInfo.SalerId != null && SearchInfo.SalerId.Length > 0)
            {
                Query.AppendFormat(" AND SalerId IN({0})", this.ConvertIntArrayTostring(SearchInfo.SalerId));
                strOrderQuery.AppendFormat(" AND SalerId IN({0})", this.ConvertIntArrayTostring(SearchInfo.SalerId));
            }
            if (SearchInfo.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计有效订单)
            {
                Query.AppendFormat(" AND OrderState IN({0},{1},{2}) ", (int)Model.EnumType.TourStructure.OrderState.未处理, (int)Model.EnumType.TourStructure.OrderState.已成交, (int)Model.EnumType.TourStructure.OrderState.已留位);
                strOrderQuery.AppendFormat(" AND OrderState IN({0},{1},{2}) ", (int)Model.EnumType.TourStructure.OrderState.未处理, (int)Model.EnumType.TourStructure.OrderState.已成交, (int)Model.EnumType.TourStructure.OrderState.已留位);
            }
            else if (SearchInfo.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单)
            {
                Query.AppendFormat(" AND OrderState = {0} ", (int)Model.EnumType.TourStructure.OrderState.已成交);
                strOrderQuery.AppendFormat(" AND OrderState = {0} ", (int)Model.EnumType.TourStructure.OrderState.已成交);
            }
            if (SearchInfo.RegisterStatus.HasValue)
            {
                string s = string.Empty;
                switch (SearchInfo.RegisterStatus.Value)
                {
                    case 1:
                        s = string.Format(" AND EXISTS(SELECT 1 FROM tbl_ReceiveRefund WHERE ItemId=tbl_TourOrder.Id AND IsCheck='0' AND ItemType={0} AND IsReceive=1) ", (int)EyouSoft.Model.EnumType.TourStructure.ItemType.订单);
                        Query.Append(s);
                        strOrderQuery.Append(s);
                        break;

                    case 2:
                        s = string.Format(" AND EXISTS(SELECT 1 FROM tbl_ReceiveRefund WHERE ItemId=tbl_TourOrder.Id AND IsCheck='0' AND ItemType={0} AND IsReceive=0) ", (int)EyouSoft.Model.EnumType.TourStructure.ItemType.订单);
                        Query.Append(s);
                        strOrderQuery.Append(s);
                        break;
                }
            }

            if (SearchInfo.QueryAmountType.HasValue && SearchInfo.QueryAmountOperator.HasValue && SearchInfo.QueryAmount.HasValue)
            {
                string _operator = string.Empty;
                switch (SearchInfo.QueryAmountOperator.Value)
                {
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.大于等于:
                        _operator = ">=";
                        break;
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.等于:
                        _operator = "=";
                        break;
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.小于等于:
                        _operator = "<=";
                        break;
                }

                string _fieldTour = string.Empty;
                string _fieldOrder = string.Empty;
                switch (SearchInfo.QueryAmountType.Value)
                {
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.未收款:
                        _fieldTour = _fieldOrder = "FinanceSum-HasCheckMoney";
                        break;
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.已收待审核:
                        _fieldTour = _fieldOrder = "NotCheckMoney";
                        break;
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.已收款:
                        _fieldTour = _fieldOrder = "HasCheckMoney";
                        break;
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.应收款:
                        _fieldTour = _fieldOrder = "FinanceSum";
                        break;
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.退款待审核:
                        _fieldTour = "(SELECT SUM(RefundMoney) FROM tbl_ReceiveRefund WHERE IsCheck='0' AND IsReceive=0 AND ItemId IN(SELECT Id FROM tbl_TourOrder WHERE TourId=tbl_tour.TourId))";
                        _fieldOrder = "(SELECT SUM(RefundMoney) FROM tbl_ReceiveRefund WHERE IsCheck='0' AND IsReceive=0 AND ItemId=tbl_TourOrder.Id)";
                        break;
                }

                if (!string.IsNullOrEmpty(_operator) && !string.IsNullOrEmpty(_fieldTour))
                {
                    Query.AppendFormat(" AND {0}{1}{2} ", _fieldTour, _operator, SearchInfo.QueryAmount);
                    strOrderQuery.AppendFormat(" AND {0}{1}{2} ", _fieldOrder, _operator, SearchInfo.QueryAmount);
                }

            }

            Query.Append(")");

            if (!string.IsNullOrEmpty(us))
            {
                Query.AppendFormat(" AND OperatorId IN({0}) ", us);
            }

            if (SearchInfo != null)
            {
                switch (SearchInfo.SortType)
                {
                    case 0: break;
                    case 1: orderByString = "LeaveDate DESC"; break;
                }
            }

            if (SearchInfo != null && SearchInfo.TourType.HasValue)
            {
                Query.AppendFormat(" AND TourType={0} ", (int)SearchInfo.TourType.Value);
                strOrderQuery.AppendFormat(" AND TourClassId={0} ", (int)SearchInfo.TourType.Value);
            }
            #endregion Sql相关处理
            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, TableName, identityColumnName, fields.ToString(), Query.ToString(), orderByString))
            {
                ResultList = new List<EyouSoft.Model.TourStructure.TourAboutAccountInfo>();
                while (dr.Read())
                {
                    string TourId = string.Empty;
                    model = new EyouSoft.Model.TourStructure.TourAboutAccountInfo();
                    TourId = dr.GetString(dr.GetOrdinal("TourId"));
                    model.TourId = TourId;
                    model.OrderAccountList = GetOrderAboutAccountList(SearchInfo.CompanyId, TourId, model, IsReceived, strOrderQuery.ToString());
                    model.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)dr.GetByte(dr.GetOrdinal("TourType"));
                    ResultList.Add(model);
                    model = null;
                }
            }
            return ResultList;
        }

        /// <summary>
        /// 获取订单信息集合
        ///   --综合方法
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="SearchInfo">查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderList(int PageSize, int PageIndex, ref int RecordCount, EyouSoft.Model.TourStructure.SearchInfoForDAL SearchInfo)
        {
            EyouSoft.Model.TourStructure.TourOrder model = null;
            IList<EyouSoft.Model.TourStructure.TourOrder> ResultList = null;
            #region Sql相关处理
            StringBuilder fields = new StringBuilder();
            fields.Append("[ID],[TourNo],[TourId],[RouteName],[LeaveDate],[OrderNo],[BuyCompanyName],TourClassId,[BuyCompanyID],");
            fields.Append("[SumPrice],[HasCheckMoney],[NotCheckMoney],[SalerName],[AdultNumber],");
            fields.Append(" [ChildNumber],FinanceSum,[IssueTime],[BuyerTourCode],[PeopleNumber],[LeaguePepoleNum] ");
            string TableName = "tbl_TourOrder";
            string orderByString = " [IssueTime] DESC";
            string identityColumnName = "id";
            StringBuilder Query = new StringBuilder();
            Query.Append(" IsDelete=0 ");
            if (SearchInfo.BuyCompanyId.HasValue && SearchInfo.BuyCompanyId > 0)//组团公司编号
            {
                Query.AppendFormat(" AND [BuyCompanyId]={0} ", SearchInfo.BuyCompanyId);
            }
            if (SearchInfo.SellCompanyId.HasValue && SearchInfo.SellCompanyId > 0)//专线公司编号
            {
                Query.AppendFormat(" AND [SellCompanyId]={0} ", SearchInfo.SellCompanyId);
            }
            if (!string.IsNullOrEmpty(SearchInfo.OrderNo))//订单号
            {
                Query.AppendFormat(" AND OrderNo LIKE '%{0}%'", SearchInfo.OrderNo);
            }
            if (!string.IsNullOrEmpty(SearchInfo.TourNo))//团号
            {
                Query.AppendFormat(" AND TourNo LIKE '%{0}%'", SearchInfo.TourNo);
            }
            if (SearchInfo.TourType.HasValue && SearchInfo.TourType.Value >= 0)//团队类型
            {
                Query.AppendFormat(" AND TourClassId={0}", (int)SearchInfo.TourType);
            }
            if (!string.IsNullOrEmpty(SearchInfo.CompanyName))//客户单位名称
            {
                Query.AppendFormat(" AND BuyCompanyName LIKE '%{0}%'", SearchInfo.CompanyName);
            }
            if (!string.IsNullOrEmpty(SearchInfo.RouteName))//线路名称
            {
                Query.AppendFormat(" AND RouteName LIKE '%{0}%'", SearchInfo.RouteName);
            }
            if (SearchInfo.AreaId.HasValue && SearchInfo.AreaId.Value > 0)//线路区域编号
            {
                Query.AppendFormat(" AND AreaId={0}", SearchInfo.AreaId);
            }
            if (SearchInfo.OperatorId != null && SearchInfo.OperatorId.Length > 0)//操作人编号
            {
                Query.AppendFormat(" AND OperatorId in({0})", this.ConvertIntArrayTostring(SearchInfo.OperatorId));
            }
            if (SearchInfo.SalerId != null && SearchInfo.SalerId.Length > 0)//销售员编号
            {
                Query.AppendFormat(" AND SalerId in({0})", this.ConvertIntArrayTostring(SearchInfo.SalerId));
            }
            if (!string.IsNullOrEmpty(SearchInfo.SalerName)) //销售员名称
            {
                Query.AppendFormat(" AND SalerName LIKE '%{0}%'", SearchInfo.SalerName);
            }
            if (!string.IsNullOrEmpty(SearchInfo.OperatorName)) //操作人名称
            {
                Query.AppendFormat(" AND OperatorName LIKE '%{0}%'", SearchInfo.OperatorName);
            }
            if (SearchInfo.LeaveDateFrom.HasValue)//出团日期 起始
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,'{0}',LeaveDate)>=0", SearchInfo.LeaveDateFrom);
            }
            if (SearchInfo.LeaveDateTo.HasValue)//出团日期 结束
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')>=0", SearchInfo.LeaveDateTo);
            }
            if (SearchInfo.CreateDateFrom.HasValue)//下单日期 起始
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,'{0}',IssueTime)>=0", SearchInfo.CreateDateFrom);
            }
            if (SearchInfo.CreateDateTo.HasValue)//下单日期 结束
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,IssueTime,'{0}')>=0", SearchInfo.CreateDateTo);
            }
            if (SearchInfo.RouteId.HasValue && SearchInfo.RouteId.Value > 0)//线路编号
            {
                Query.AppendFormat(" AND RouteId={0}", SearchInfo.RouteId);
            }
            //是否结清
            if (SearchInfo.IsSettle.HasValue)
            {
                if (SearchInfo.IsSettle == true)
                {
                    Query.Append(" AND HasCheckMoney=FinanceSum ");
                }
                else
                {
                    Query.Append(" AND FinanceSum<>HasCheckMoney ");
                }
            }
            if (!string.IsNullOrEmpty(SearchInfo.HaveUserIds)) //组织框架控制
            {
                Query.AppendFormat(" AND ViewOperatorId IN({0}) ", SearchInfo.HaveUserIds);
            }
            if (SearchInfo.TourType != null)
            {
                Query.AppendFormat(" AND TourId IN (select TourId from tbl_Tour where tourtype = {0}) ", (int)SearchInfo.TourType);
            }
            if (SearchInfo.OrderState != null && SearchInfo.OrderState.Length > 0)//订单状态
            {
                Query.Append(" AND (");
                for (int i = 0; i < SearchInfo.OrderState.Length; i++)
                {
                    if (i == 0)
                    {
                        Query.AppendFormat(" OrderState={0}", (int)SearchInfo.OrderState[i]);
                    }
                    else
                    {
                        Query.AppendFormat(" OR OrderState={0}", (int)SearchInfo.OrderState[i]);
                    }
                }
                Query.Append(" )");
            }
            #endregion Sql相关处理
            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, TableName, identityColumnName, fields.ToString(), Query.ToString(), orderByString))
            {
                ResultList = new List<EyouSoft.Model.TourStructure.TourOrder>();
                while (dr.Read())
                {
                    model = new EyouSoft.Model.TourStructure.TourOrder();
                    model.PeopleNumber = dr.IsDBNull(dr.GetOrdinal("PeopleNumber"))
                                             ? 0
                                             : dr.GetInt32(dr.GetOrdinal("PeopleNumber"));
                    model.LeaguePepoleNum = dr.IsDBNull(dr.GetOrdinal("LeaguePepoleNum"))
                                             ? 0
                                             : dr.GetInt32(dr.GetOrdinal("LeaguePepoleNum"));
                    model.ID = dr.GetString(dr.GetOrdinal("id"));
                    model.OrderNo = dr.IsDBNull(dr.GetOrdinal("OrderNo")) ? "" : dr.GetString(dr.GetOrdinal("OrderNo"));
                    model.TourNo = dr.IsDBNull(dr.GetOrdinal("TourNo")) ? "" : dr.GetString(dr.GetOrdinal("TourNo"));
                    model.TourId = dr.IsDBNull(dr.GetOrdinal("TourId")) ? "" : dr.GetString(dr.GetOrdinal("TourId"));
                    model.RouteName = dr.IsDBNull(dr.GetOrdinal("RouteName")) ? "" : dr.GetString(dr.GetOrdinal("RouteName"));
                    model.LeaveDate = dr.GetDateTime(dr.GetOrdinal("LeaveDate"));
                    model.BuyCompanyName = dr.IsDBNull(dr.GetOrdinal("BuyCompanyName")) ? "" : dr.GetString(dr.GetOrdinal("BuyCompanyName"));
                    model.SumPrice = dr.IsDBNull(dr.GetOrdinal("SumPrice")) ? 0 : dr.GetDecimal(dr.GetOrdinal("SumPrice"));
                    model.FinanceSum = dr.IsDBNull(dr.GetOrdinal("FinanceSum")) ? 0 : dr.GetDecimal(dr.GetOrdinal("FinanceSum"));
                    model.HasCheckMoney = dr.IsDBNull(dr.GetOrdinal("HasCheckMoney")) ? 0 : dr.GetDecimal(dr.GetOrdinal("HasCheckMoney"));
                    model.NotCheckMoney = dr.IsDBNull(dr.GetOrdinal("NotCheckMoney")) ? 0 : dr.GetDecimal(dr.GetOrdinal("NotCheckMoney"));
                    model.NotReceived = model.FinanceSum - model.HasCheckMoney;
                    model.SalerName = dr.IsDBNull(dr.GetOrdinal("SalerName")) ? "" : dr.GetString(dr.GetOrdinal("SalerName"));
                    model.AdultNumber = dr.IsDBNull(dr.GetOrdinal("AdultNumber")) ? 0 : dr.GetInt32(dr.GetOrdinal("AdultNumber"));
                    model.ChildNumber = dr.IsDBNull(dr.GetOrdinal("ChildNumber")) ? 0 : dr.GetInt32(dr.GetOrdinal("ChildNumber"));
                    model.PeopleNumber = model.PeopleNumber - model.LeaguePepoleNum;
                    model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    model.BuyCompanyID = dr.IsDBNull(dr.GetOrdinal("BuyCompanyID")) ? 0 : dr.GetInt32(dr.GetOrdinal("BuyCompanyID"));
                    model.TourClassId = (EyouSoft.Model.EnumType.TourStructure.TourType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.TourType), dr.GetByte(dr.GetOrdinal("TourClassId")).ToString());
                    model.BuyerTourCode = dr["BuyerTourCode"].ToString();
                    ResultList.Add(model);
                    model = null;
                }
            }
            return ResultList;
        }
        /// <summary>
        /// 根据客户公司编号获取未结清帐款的订单信息集合
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <param name="buyCompanyId">客户单位编号</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询实体</param>
        /// <param name="sellerId">销售员编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderListByBuyCompanyId(int PageSize, int PageIndex, ref int RecordCount, int companyId, int buyCompanyId, int sellerId, EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo searchInfo)
        {
            EyouSoft.Model.TourStructure.TourOrder model = null;
            IList<EyouSoft.Model.TourStructure.TourOrder> ResultList = null;
            #region Sql相关处理
            string fields = " ID,RouteName,LeaveDate,TourNo,FinanceSum,HasCheckMoney,NotCheckMoney";
            string TableName = "tbl_TourOrder";
            string orderByString = " IssueTime DESC";
            string identityColumnName = "id";
            StringBuilder query = new StringBuilder();
            query.AppendFormat(" BuyCompanyId={0}  AND IsDelete='0' AND SellCompanyId={1} AND OrderState NOT IN(3,4) ", buyCompanyId, companyId);

            if (sellerId > 0)
            {
                query.AppendFormat(" AND SalerId={0} ", sellerId);
            }

            if (searchInfo != null)
            {
                if (searchInfo.LEDate.HasValue
                    || searchInfo.LSDate.HasValue
                    || (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    || (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0))
                {
                    query.AppendFormat(" AND TourId IN( ");
                    query.AppendFormat(" SELECT TourId FROM tbl_Tour AS B WHERE B.TourId IN(SELECT TourId FROM tbl_TourOrder AS A WHERE A.BuyCompanyID={0} AND A.[IsDelete]='0' AND A.[OrderState] NOT IN(3,4)) ", buyCompanyId);

                    if (searchInfo.LSDate.HasValue)
                    {
                        query.AppendFormat(" AND B.LeaveDate>='{0}' ", searchInfo.LSDate.Value);
                    }
                    if (searchInfo.LEDate.HasValue)
                    {
                        query.AppendFormat(" AND B.LeaveDate<='{0}' ", searchInfo.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0)
                    {
                        query.AppendFormat(" AND B.OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorIds));
                    }
                    if (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    {
                        query.AppendFormat(" AND B.OperatorId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorDepartIds));
                    }

                    query.Append(" ) ");
                }
            }

            #endregion Sql相关处理
            using (IDataReader dr = DbHelper.ExecuteReader(_db, PageSize, PageIndex, ref RecordCount, TableName, identityColumnName, fields.ToString(), query.ToString(), orderByString))
            {
                ResultList = new List<EyouSoft.Model.TourStructure.TourOrder>();
                while (dr.Read())
                {
                    model = new EyouSoft.Model.TourStructure.TourOrder()
                    {
                        ID = dr.GetString(dr.GetOrdinal("id")),
                        RouteName = dr.IsDBNull(dr.GetOrdinal("RouteName")) ? "" : dr.GetString(dr.GetOrdinal("RouteName")),
                        LeaveDate = dr.GetDateTime(dr.GetOrdinal("LeaveDate")),
                        TourNo = dr.IsDBNull(dr.GetOrdinal("TourNo")) ? "" : dr.GetString(dr.GetOrdinal("TourNo")),
                        FinanceSum = dr.IsDBNull(dr.GetOrdinal("FinanceSum")) ? 0 : dr.GetDecimal(dr.GetOrdinal("FinanceSum")),
                        HasCheckMoney = dr.IsDBNull(dr.GetOrdinal("HasCheckMoney")) ? 0 : dr.GetDecimal(dr.GetOrdinal("HasCheckMoney")),
                        NotCheckMoney = dr.IsDBNull(dr.GetOrdinal("NotCheckMoney")) ? 0 : dr.GetDecimal(dr.GetOrdinal("NotCheckMoney")),
                    };
                    model.NotReceived = model.FinanceSum - model.HasCheckMoney;
                    ResultList.Add(model);
                    model = null;
                }
            }
            return ResultList;
        }

        /// <summary>
        /// 根据客户公司编号获取未结清帐款的订单信息集合(汇总)
        /// </summary>
        /// <param name="buyCompanyId">客户单位编号</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="searchInfo">查询实体</param>
        /// <param name="sellerId">销售员编号</param>
        /// <param name="daiShouKuanHeJi">待收款金额汇总</param>
        /// <returns></returns>
        public void GetOrderListByBuyCompanyId(int companyId, int buyCompanyId, int sellerId, EyouSoft.Model.PersonalCenterStructure.ReceiptRemindSearchInfo searchInfo, out decimal daiShouKuanHeJi)
        {
            daiShouKuanHeJi = 0;
            StringBuilder cmdText = new StringBuilder();
            cmdText.Append("SELECT SUM(FinanceSum) AS C1,SUM(HasCheckMoney) AS C2,SUM(NotCheckMoney) AS C3 FROM tbl_TourOrder WHERE ");
            cmdText.AppendFormat(" BuyCompanyId={0}  AND IsDelete='0' AND SellCompanyId={1} AND OrderState NOT IN(3,4) ", buyCompanyId, companyId);
            if (sellerId > 0)
            {
                cmdText.AppendFormat(" AND SalerId={0} ", sellerId);
            }

            if (searchInfo != null)
            {
                if (searchInfo.LEDate.HasValue
                    || searchInfo.LSDate.HasValue
                    || (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    || (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0))
                {
                    cmdText.AppendFormat(" AND TourId IN( ");
                    cmdText.AppendFormat(" SELECT TourId FROM tbl_Tour AS B WHERE B.TourId IN(SELECT TourId FROM tbl_TourOrder AS A WHERE A.BuyCompanyID={0} AND A.[IsDelete]='0' AND A.[OrderState] NOT IN(3,4)) ", buyCompanyId);

                    if (searchInfo.LSDate.HasValue)
                    {
                        cmdText.AppendFormat(" AND B.LeaveDate>='{0}' ", searchInfo.LSDate.Value);
                    }
                    if (searchInfo.LEDate.HasValue)
                    {
                        cmdText.AppendFormat(" AND B.LeaveDate<='{0}' ", searchInfo.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    if (searchInfo.OperatorIds != null && searchInfo.OperatorIds.Length > 0)
                    {
                        cmdText.AppendFormat(" AND B.OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorIds));
                    }
                    if (searchInfo.OperatorDepartIds != null && searchInfo.OperatorDepartIds.Length > 0)
                    {
                        cmdText.AppendFormat(" AND B.OperatorId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByArray(searchInfo.OperatorDepartIds));
                    }

                    cmdText.Append(" ) ");
                }
            }

            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    decimal c1 = 0;//合计应收
                    decimal c2 = 0;//确认已收

                    if (!rdr.IsDBNull(rdr.GetOrdinal("C1"))) c1 = rdr.GetDecimal(rdr.GetOrdinal("C1"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("C2"))) c2 = rdr.GetDecimal(rdr.GetOrdinal("C2"));

                    daiShouKuanHeJi = c1 - c2;
                }
            }

        }

        /// <summary>
        /// 根据据团队编号获取订单信息集合        
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="TourId">团队编号</param>     
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetOrderList(int CompanyId, string TourId)
        {
            EyouSoft.Model.TourStructure.TourOrder model = null;
            IList<EyouSoft.Model.TourStructure.TourOrder> ResultList = null;
            #region Sql相关处理
            StringBuilder Query = new StringBuilder();
            Query.Append("SELECT [ID],FinanceSum,HasCheckMoney,NotCheckMoney,OperatorContent,BuyCompanyName");
            Query.Append(",(SELECT [ContactName] FROM tbl_Customer  WHERE [id]=tbl_TourOrder.[BuyCompanyID]) AS [ContactName]");
            Query.Append(",(SELECT [Phone] FROM tbl_Customer  WHERE [id]=tbl_TourOrder.[BuyCompanyID]) AS [ContactTel]");
            Query.Append(",(SELECT [mobile] FROM tbl_Customer  WHERE [id]=tbl_TourOrder.[BuyCompanyID]) AS [ContactMobile]");
            Query.Append(",(SELECT Fax FROM tbl_Customer  WHERE [id]=tbl_TourOrder.[BuyCompanyID]) AS [ContactFax]");
            Query.AppendFormat(" FROM tbl_TourOrder WHERE SellCompanyId={0}  AND IsDelete=0  AND (OrderState=1 OR OrderState=2 OR OrderState=5) AND TourId='{1}' ", CompanyId, TourId);
            Query.Append(" ORDER BY [IssueTime] ASC");
            #endregion Sql相关处理
            DbCommand dc = this._db.GetSqlStringCommand(Query.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                ResultList = new List<EyouSoft.Model.TourStructure.TourOrder>();
                while (dr.Read())
                {
                    model = new EyouSoft.Model.TourStructure.TourOrder()
                    {
                        ID = dr.GetString(dr.GetOrdinal("id")),
                        FinanceSum = dr.IsDBNull(dr.GetOrdinal("FinanceSum")) ? 0 : dr.GetDecimal(dr.GetOrdinal("FinanceSum")),
                        HasCheckMoney = dr.IsDBNull(dr.GetOrdinal("HasCheckMoney")) ? 0 : dr.GetDecimal(dr.GetOrdinal("HasCheckMoney")),
                        NotCheckMoney = dr.IsDBNull(dr.GetOrdinal("NotCheckMoney")) ? 0 : dr.GetDecimal(dr.GetOrdinal("NotCheckMoney")),
                        OperatorContent = dr.IsDBNull(dr.GetOrdinal("OperatorContent")) ? "" : dr.GetString(dr.GetOrdinal("OperatorContent")),
                        BuyCompanyName = dr.IsDBNull(dr.GetOrdinal("BuyCompanyName")) ? "" : dr.GetString(dr.GetOrdinal("BuyCompanyName")),
                        ContactName = dr.IsDBNull(dr.GetOrdinal("ContactName")) ? "" : dr.GetString(dr.GetOrdinal("ContactName")),
                        ContactTel = dr.IsDBNull(dr.GetOrdinal("ContactTel")) ? "" : dr.GetString(dr.GetOrdinal("ContactTel")),
                        ContactMobile = dr.IsDBNull(dr.GetOrdinal("ContactMobile")) ? "" : dr.GetString(dr.GetOrdinal("ContactMobile")),
                        ContactFax = dr.IsDBNull(dr.GetOrdinal("ContactFax")) ? "" : dr.GetString(dr.GetOrdinal("ContactFax"))
                    };
                    model.NotReceived = model.FinanceSum - model.HasCheckMoney;
                    ResultList.Add(model);
                    model = null;
                }
            }
            return ResultList;
        }
        /// <summary>
        /// 新增订单
        /// </summary>
        /// <param name="model">订单信息实体</param>
        /// <returns>
        /// 0:失败；
        /// 1:成功；
        /// 2：该团队的订单总人数+当前订单人数大于团队计划人数总和；
        /// 3：该客户所欠金额大于最高欠款金额；
        /// </returns>
        public int AddOrder(EyouSoft.Model.TourStructure.TourOrder model)
        {
            string TourOrderCustomerXML = string.Empty;
            if (model.CustomerList != null)
            {
                TourOrderCustomerXML = this.CreateTourOrderCustomerXML(model.CustomerList, model.ID);
            }
            DbCommand dc = this._db.GetStoredProcCommand("proc_TourOrder_Insert");
            this._db.AddInParameter(dc, "OrderID", DbType.AnsiStringFixedLength, model.ID);
            this._db.AddInParameter(dc, "TourId", DbType.AnsiStringFixedLength, model.TourId);
            this._db.AddInParameter(dc, "AreaId", DbType.Int32, model.AreaId);
            this._db.AddInParameter(dc, "RouteId", DbType.Int32, model.RouteId);
            this._db.AddInParameter(dc, "RouteName", DbType.String, model.RouteName);
            this._db.AddInParameter(dc, "TourNo", DbType.String, model.TourNo);
            this._db.AddInParameter(dc, "TourClassId", DbType.Byte, model.TourClassId);
            this._db.AddInParameter(dc, "LeaveDate", DbType.DateTime, model.LeaveDate);
            this._db.AddInParameter(dc, "TourDays", DbType.Int32, model.Tourdays);
            this._db.AddInParameter(dc, "LeaveTraffic", DbType.String, model.LeaveTraffic);
            this._db.AddInParameter(dc, "ReturnTraffic", DbType.String, model.ReturnTraffic);
            this._db.AddInParameter(dc, "OrderType", DbType.Byte, (int)model.OrderType);
            this._db.AddInParameter(dc, "OrderState", DbType.Byte, (int)model.OrderState);
            this._db.AddInParameter(dc, "BuyCompanyID", DbType.Int32, model.BuyCompanyID);
            this._db.AddInParameter(dc, "BuyCompanyName", DbType.String, model.BuyCompanyName);
            this._db.AddInParameter(dc, "ContactName", DbType.String, model.ContactName);
            this._db.AddInParameter(dc, "ContactTel", DbType.String, model.ContactTel);
            this._db.AddInParameter(dc, "ContactMobile", DbType.String, model.ContactMobile);
            this._db.AddInParameter(dc, "ContactFax", DbType.String, model.ContactFax);
            //this._db.AddInParameter(dc, "SalerName", DbType.String, model.SalerName);
            //this._db.AddInParameter(dc, "SalerId", DbType.Int32, model.SalerId);
            this._db.AddInParameter(dc, "OperatorName", DbType.String, model.OperatorName);
            this._db.AddInParameter(dc, "OperatorID", DbType.Int32, model.OperatorID);
            this._db.AddInParameter(dc, "PriceStandId", DbType.Int32, model.PriceStandId);
            this._db.AddInParameter(dc, "ChildPrice", DbType.Currency, model.ChildPrice);
            this._db.AddInParameter(dc, "PersonalPrice", DbType.Currency, model.PersonalPrice);
            this._db.AddInParameter(dc, "MarketPrice", DbType.Currency, model.MarketPrice);
            this._db.AddInParameter(dc, "AdultNumber", DbType.Int32, model.AdultNumber);
            this._db.AddInParameter(dc, "ChildNumber", DbType.Int32, model.ChildNumber);
            this._db.AddInParameter(dc, "MarketNumber", DbType.Int32, model.MarketNumber);
            this._db.AddInParameter(dc, "PeopleNumber", DbType.Int32, model.PeopleNumber);
            this._db.AddInParameter(dc, "OtherPrice", DbType.Currency, model.OtherPrice);
            this._db.AddInParameter(dc, "SaveSeatDate", DbType.DateTime, model.SaveSeatDate);
            this._db.AddInParameter(dc, "OperatorContent", DbType.String, model.OperatorContent);
            this._db.AddInParameter(dc, "SpecialContent", DbType.String, model.SpecialContent);
            this._db.AddInParameter(dc, "SumPrice", DbType.Currency, model.SumPrice);
            this._db.AddInParameter(dc, "SellCompanyName", DbType.String, model.SellCompanyName);
            this._db.AddInParameter(dc, "SellCompanyId", DbType.Int32, model.SellCompanyId);
            this._db.AddInParameter(dc, "LastDate", DbType.DateTime, model.LastDate);
            this._db.AddInParameter(dc, "LastOperatorID", DbType.Int32, model.LastOperatorID);
            this._db.AddInParameter(dc, "ViewOperatorId", DbType.Int32, model.ViewOperatorId);
            this._db.AddInParameter(dc, "CustomerDisplayType", DbType.Byte, model.CustomerDisplayType);
            this._db.AddInParameter(dc, "CustomerFilePath", DbType.String, model.CustomerFilePath);
            this._db.AddInParameter(dc, "TourCustomerXML", DbType.String, TourOrderCustomerXML);
            this._db.AddInParameter(dc, "CustomerLevId", DbType.Int32, model.CustomerLevId);
            this._db.AddInParameter(dc, "IsTourOrderEdit", DbType.Int32, model.IsTourOrderEdit ? 1 : 0);
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            this._db.AddInParameter(dc, "AmountPlus", DbType.String, this.CreateOrderAmountPlusXML(model.AmountPlus));
            this._db.AddInParameter(dc, "BuyerContactId", DbType.Int32, model.BuyerContactId);
            this._db.AddInParameter(dc, "BuyerContactName", DbType.String, model.BuyerContactName);
            this._db.AddInParameter(dc, "CommissionType", DbType.Byte, model.CommissionType);
            this._db.AddInParameter(dc, "CommissionPrice", DbType.Decimal, model.CommissionPrice);
            _db.AddInParameter(dc, "BuyerTourCode", DbType.String, model.BuyerTourCode);
            _db.AddInParameter(dc, "OrderTraffic", DbType.Int32, model.OrderTrafficId);

            DbHelper.RunProcedure(dc, _db);
            object Result = this._db.GetParameterValue(dc, "Result");

            return int.Parse(Result.ToString());
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">订单信息实体</param>
        /// <returns>
        /// 0:失败；
        /// 1:成功；
        /// 2：该团队的订单总人数+当前订单人数大于团队计划人数总和；
        /// 3：该客户所欠金额大于最高欠款金额；
        /// </returns>
        public int Update(EyouSoft.Model.TourStructure.TourOrder model)
        {
            string TourOrderCustomerXML = string.Empty;
            if (model.CustomerList != null)
            {
                TourOrderCustomerXML = this.CreateTourOrderCustomerXML(model.CustomerList, model.ID);
            }
            DbCommand dc = this._db.GetStoredProcCommand("proc_TourOrder_Update");
            this._db.AddInParameter(dc, "OrderID", DbType.AnsiStringFixedLength, model.ID);
            this._db.AddInParameter(dc, "TourId", DbType.AnsiStringFixedLength, model.TourId);
            this._db.AddInParameter(dc, "OrderState", DbType.Byte, (int)model.OrderState);
            this._db.AddInParameter(dc, "PriceStandId", DbType.Int32, model.PriceStandId);
            this._db.AddInParameter(dc, "PersonalPrice", DbType.Currency, model.PersonalPrice);
            this._db.AddInParameter(dc, "ChildPrice", DbType.Currency, model.ChildPrice);
            this._db.AddInParameter(dc, "MarketPrice", DbType.Currency, model.MarketPrice);
            this._db.AddInParameter(dc, "AdultNumber", DbType.Int32, model.AdultNumber);
            this._db.AddInParameter(dc, "ChildNumber", DbType.Int32, model.ChildNumber);
            this._db.AddInParameter(dc, "MarketNumber", DbType.Int32, model.MarketNumber);
            this._db.AddInParameter(dc, "PeopleNumber", DbType.Int32, model.PeopleNumber);
            this._db.AddInParameter(dc, "OtherPrice", DbType.Currency, model.OtherPrice);
            this._db.AddInParameter(dc, "SaveSeatDate", DbType.DateTime, model.SaveSeatDate);
            this._db.AddInParameter(dc, "OperatorContent", DbType.String, model.OperatorContent);
            this._db.AddInParameter(dc, "SpecialContent", DbType.String, model.SpecialContent);
            this._db.AddInParameter(dc, "SumPrice", DbType.Currency, model.SumPrice);
            this._db.AddInParameter(dc, "LastDate", DbType.DateTime, model.LastDate);
            this._db.AddInParameter(dc, "LastOperatorID", DbType.Int32, model.LastOperatorID);
            this._db.AddInParameter(dc, "CustomerDisplayType", DbType.Byte, model.CustomerDisplayType);
            this._db.AddInParameter(dc, "CustomerFilePath", DbType.String, model.CustomerFilePath);
            this._db.AddInParameter(dc, "TourCustomerXML", DbType.String, TourOrderCustomerXML);
            this._db.AddInParameter(dc, "CustomerLevId", DbType.Int32, model.CustomerLevId);

            this._db.AddInParameter(dc, "ContactName", DbType.String, model.ContactName);
            this._db.AddInParameter(dc, "ContactMobile", DbType.String, model.ContactMobile);
            this._db.AddInParameter(dc, "ContactTel", DbType.String, model.ContactTel);
            this._db.AddInParameter(dc, "ContactFax", DbType.String, model.ContactFax);
            this._db.AddInParameter(dc, "IsTourOrderEdit", DbType.Int32, model.IsTourOrderEdit ? 1 : 0);
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            this._db.AddInParameter(dc, "AmountPlus", DbType.String, this.CreateOrderAmountPlusXML(model.AmountPlus));
            this._db.AddInParameter(dc, "BuyerContactId", DbType.Int32, model.BuyerContactId);
            this._db.AddInParameter(dc, "BuyerContactName", DbType.String, model.BuyerContactName);
            this._db.AddInParameter(dc, "CommissionPrice", DbType.Decimal, model.CommissionPrice);
            this._db.AddInParameter(dc, "CommissionType", DbType.Byte, model.CommissionType);

            this._db.AddInParameter(dc, "BuyCompanyID", DbType.Int32, model.BuyCompanyID);
            this._db.AddInParameter(dc, "BuyCompanyName", DbType.String, model.BuyCompanyName);
            this._db.AddInParameter(dc, "SalerName", DbType.String, model.SalerName);
            this._db.AddInParameter(dc, "SalerId", DbType.Int32, model.SalerId);

            _db.AddInParameter(dc, "TourTeamUnit", DbType.String, CreateTourTeamUnitXml(model.TourTeamUnit));
            _db.AddInParameter(dc, "BuyerTourCode", DbType.String, model.BuyerTourCode);
            _db.AddInParameter(dc, "OrderTraffic", DbType.Int32, model.OrderTrafficId);

            DbHelper.RunProcedure(dc, _db);
            object Result = this._db.GetParameterValue(dc, "Result");

            return int.Parse(Result.ToString());
        }
        /// <summary>
        /// 修改订单增加/减少费用信息
        /// </summary>
        /// <param name="Lists">团队结算订单信息集合</param>
        /// <returns></returns>
        public bool UpdateFinanceExpense(IList<EyouSoft.Model.TourStructure.OrderFinanceExpense> Lists)
        {
            bool IsTrue = false;
            string FinanceExpenseXML = this.CreateFinanceExpenseXML(Lists);
            DbCommand dc = this._db.GetStoredProcCommand("proc_TourOrder_UpdateFinanceExpense");
            this._db.AddInParameter(dc, "FinanceExpenseXML", DbType.String, FinanceExpenseXML);
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
        /// 删除
        /// </summary>
        /// <param name="OrderId">订单编号</param>
        /// <param name="OrderId">公司编号</param>
        /// <returns></returns>
        public bool Delete(string OrderId, int CompanyId)
        {
            /*string StrSql = "UPDATE [tbl_TourOrder] SET IsDelete=1 WHERE [ID]=@OrderId AND [SellCompanyId]=@CompanyId; ";
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            this._db.AddInParameter(dc, "OrderId", DbType.AnsiStringFixedLength, OrderId);
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, CompanyId);
            return DbHelper.ExecuteSql(dc, this._db) > 0 ? true : false;*/

            DbCommand cmd = this._db.GetStoredProcCommand("proc_TourOrder_Delete");
            this._db.AddInParameter(cmd, "OrderId", DbType.AnsiString, OrderId);

            DbHelper.RunProcedure(cmd, this._db);

            return true;
        }

        /// <summary>
        /// 创建订单号
        /// </summary>
        /// <returns>返回订单号</returns>
        public string CreateOrderNo()
        {
            string OrderNo = string.Empty;
            string StrSql = "select dbo.fn_TourOrder_CreateOrderNo()";
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            OrderNo = DbHelper.GetSingle(dc, this._db).ToString();
            return OrderNo;
        }

        #endregion 订单信息

        #region 销售收款/退款
        /// <summary>
        /// 获取收退款信息
        /// </summary>
        /// <param name="OrderId">订单编号</param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="IsRecive">true=收款，false=退款</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> GetReceiveRefund(string OrderId, int CompanyId, bool IsRecive)
        {

            dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);

            IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> lists = null;
            lists = (from item in dcDal.ReceiveRefund
                     where item.CompanyID == CompanyId && item.ItemId == OrderId && item.IsReceive == Convert.ToByte(IsRecive)
                     select new EyouSoft.Model.FinanceStructure.ReceiveRefund
                     {
                         Id = item.Id,
                         BillAmount = item.BillAmount,
                         IsBill = Convert.ToBoolean(item.IsBill),
                         ItemId = item.ItemId,
                         PayCompanyId = item.PayCompanyId,
                         PayCompanyName = item.PayCompanyName,
                         RefundDate = item.RefundDate,
                         RefundMoney = item.RefundMoney,
                         Remark = item.Remark,
                         StaffNo = item.StaffNo,
                         StaffName = item.StaffName,
                         IsCheck = Convert.ToBoolean(item.IsCheck),
                         RefundType = (EyouSoft.Model.EnumType.TourStructure.RefundType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.RefundType), item.RefundType.ToString())
                     }).ToList<EyouSoft.Model.FinanceStructure.ReceiveRefund>();



            return lists;
        }
        /// <summary>
        /// 增加销售收款/退款
        /// </summary>
        /// <param name="lists">收入收退款登记明细信息集合</param>
        /// <param name="IsRecive">是否收款(true:收款;false:退款)</param>
        /// <returns></returns>
        public bool AddReceiveRefund(IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> lists, string OrderId, bool IsRecive)
        {
            bool IsTrue = false;
            string ReceiveRefundXML = CreateReceiveRefundXML(lists, IsRecive, OrderId);
            DbCommand dc = this._db.GetStoredProcCommand("proc_ReceiveRefund_Insert");
            this._db.AddInParameter(dc, "ReceiveRefundXML", DbType.String, ReceiveRefundXML);
            this._db.AddInParameter(dc, "OrderId", DbType.AnsiStringFixedLength, OrderId);
            this._db.AddInParameter(dc, "IsRecive", DbType.Int32, IsRecive ? 1 : 0);
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
        /// 修改销售收款/退款
        /// </summary>
        /// <param name="model">收入收退款登记明细信息实体</param>
        /// <returns></returns>
        public bool UpdateReceiveRefund(IList<EyouSoft.Model.FinanceStructure.ReceiveRefund> lists, string OrderId, bool IsRecive)
        {
            bool IsTrue = false;
            string ReceiveRefundXML = CreateReceiveRefundXML(lists, IsRecive, OrderId);
            DbCommand dc = this._db.GetStoredProcCommand("proc_ReceiveRefund_Update");
            this._db.AddInParameter(dc, "ReceiveRefundXML", DbType.String, ReceiveRefundXML);
            this._db.AddInParameter(dc, "OrderId", DbType.AnsiStringFixedLength, OrderId);
            this._db.AddInParameter(dc, "IsRecive", DbType.Int32, IsRecive ? 1 : 0);
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
        /// 删除销售收款/退款
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="CompanyId">收款（退款）编号(主键)</param>
        /// <returns></returns>
        public bool DeleteReceiveRefund(int CompanyId, string Id)
        {
            bool IsTrue = false;
            DbCommand dc = this._db.GetStoredProcCommand("proc_ReceiveRefund_Delete");
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, CompanyId);
            this._db.AddInParameter(dc, "RefundId", DbType.AnsiStringFixedLength, Id);
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, this._db);
            object Result = this._db.GetParameterValue(dc, "Result");
            if (!Result.Equals(null))
            {
                IsTrue = int.Parse(Result.ToString()) > 0 ? true : false;
            }
            return IsTrue;
        }
        /// <summary>
        /// 销售收款/退款审核
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="Id">收款（退款）编号(主键)</param>
        /// <returns></returns>
        public bool CheckReceiveRefund(int CompanyId, string Id)
        {
            bool IsTrue = false;
            DbCommand dc = this._db.GetStoredProcCommand("proc_ReceiveRefund_Check");
            this._db.AddInParameter(dc, "CompanyId", DbType.Int32, CompanyId);
            this._db.AddInParameter(dc, "RefundId", DbType.AnsiStringFixedLength, Id);
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, this._db);
            object Result = this._db.GetParameterValue(dc, "Result");
            if (!Result.Equals(null))
            {
                IsTrue = int.Parse(Result.ToString()) > 0 ? true : false;
            }
            return IsTrue;
        }
        #endregion 销售收款/退款

        #region 游客基本信息
        /// <summary>
        /// 获取订单游客信息集合
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="OrderId">订单编号</param>
        /// <param name="CusType">0:所有，2：退团</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrderCustomer> GetCustomerList(int CompanyId, string OrderId, int CusType)
        {
            EyouSoft.Model.TourStructure.TourOrderCustomer model = null;
            IList<EyouSoft.Model.TourStructure.TourOrderCustomer> ResultList = null;
            #region Sql相关处理
            StringBuilder StrSql = new StringBuilder();
            StrSql.Append("SELECT [ID],CompanyID,CradType,OrderId,VisitorName,CradNumber,VisitorType,Sex,ContactTel,TicketStatus,CustomerStatus");
            StrSql.Append(" FROM [tbl_TourOrderCustomer] tt ");
            StrSql.AppendFormat("WHERE [CompanyId]={0} AND [OrderId]='{1}' ", CompanyId, OrderId);
            if (CusType == 2)
            {
                //未退团
                StrSql.Append(" AND CustomerStatus=0");
            }
            StrSql.Append(" Order By tt.[IdentityId] ASC ");
            #endregion Sql相关处理
            DbCommand dc = this._db.GetSqlStringCommand(StrSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                ResultList = new List<EyouSoft.Model.TourStructure.TourOrderCustomer>();
                while (dr.Read())
                {
                    model = new EyouSoft.Model.TourStructure.TourOrderCustomer()
                    {
                        ID = dr.GetString(dr.GetOrdinal("id")),
                        OrderId = dr.GetString(dr.GetOrdinal("OrderId")),
                        CompanyID = dr.GetInt32(dr.GetOrdinal("CompanyID")),
                        CradType = (EyouSoft.Model.EnumType.TourStructure.CradType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.CradType), dr.GetByte(dr.GetOrdinal("CradType")).ToString()),
                        VisitorName = dr.IsDBNull(dr.GetOrdinal("VisitorName")) ? "" : dr.GetString(dr.GetOrdinal("VisitorName")),
                        CradNumber = dr.IsDBNull(dr.GetOrdinal("CradNumber")) ? "" : dr.GetString(dr.GetOrdinal("CradNumber")),
                        VisitorType = (EyouSoft.Model.EnumType.TourStructure.VisitorType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.VisitorType), dr.GetByte(dr.GetOrdinal("VisitorType")).ToString()),
                        Sex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)Enum.Parse(typeof(EyouSoft.Model.EnumType.CompanyStructure.Sex), dr.GetByte(dr.GetOrdinal("Sex")).ToString()),
                        ContactTel = dr.IsDBNull(dr.GetOrdinal("ContactTel")) ? "" : dr.GetString(dr.GetOrdinal("ContactTel")),
                        CustomerStatus = (EyouSoft.Model.EnumType.TourStructure.CustomerStatus)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.CustomerStatus), dr.GetByte(dr.GetOrdinal("CustomerStatus")).ToString())
                    };
                    model.SpecialServiceInfo = this.GetSpecialService(model.ID);
                    ResultList.Add(model);
                    model = null;
                }
            }

            return ResultList;
        }
        /// <summary>
        /// 获取计划下所有游客信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrderCustomer> GetTravellers(string tourId)
        {
            IList<EyouSoft.Model.TourStructure.TourOrderCustomer> items = new List<EyouSoft.Model.TourStructure.TourOrderCustomer>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTravellers);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.TourStructure.TourOrderCustomer item = new EyouSoft.Model.TourStructure.TourOrderCustomer();
                    item.ID = rdr.GetString(rdr.GetOrdinal("Id"));
                    item.VisitorName = rdr["VisitorName"].ToString();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("CradType")))
                        item.CradType = (EyouSoft.Model.EnumType.TourStructure.CradType)rdr.GetByte(rdr.GetOrdinal("CradType"));
                    item.CradNumber = rdr["CradNumber"].ToString();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("Sex")))
                        item.Sex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)rdr.GetByte(rdr.GetOrdinal("Sex"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("VisitorType")))
                        item.VisitorType = (EyouSoft.Model.EnumType.TourStructure.VisitorType)rdr.GetByte(rdr.GetOrdinal("VisitorType"));
                    item.ContactTel = rdr["ContactTel"].ToString();

                    //申请的航段信息
                    string xml = rdr["ApplyFlights"].ToString();

                    if (!string.IsNullOrEmpty(xml))
                    {
                        item.ApplyFlights = new List<int>();
                        XElement xRoot = XElement.Parse(xml);
                        var xRows = Utils.GetXElements(xRoot, "row");
                        foreach (var xRow in xRows)
                        {
                            item.ApplyFlights.Add(Utils.GetInt(Utils.GetXAttributeValue(xRow, "ID")));
                        }
                    }

                    //退的航段信息
                    xml = rdr["RefundFlights"].ToString();
                    if (!string.IsNullOrEmpty(xml))
                    {
                        item.RefundFlights = new List<int>();
                        XElement xRoot = XElement.Parse(xml);
                        var xRows = Utils.GetXElements(xRoot, "row");
                        foreach (var xRow in xRows)
                        {
                            item.RefundFlights.Add(Utils.GetInt(Utils.GetXAttributeValue(xRow, "FlightId")));
                        }
                    }

                    item.CustomerStatus = (EyouSoft.Model.EnumType.TourStructure.CustomerStatus)rdr.GetByte(rdr.GetOrdinal("CustomerStatus"));
                    item.OrderId = rdr.GetString(rdr.GetOrdinal("OrderId"));

                    items.Add(item);
                }
            }


            return items;
        }
        /// <summary>
        /// 增加游客信息
        /// </summary>
        /// <param name="CusList">订单游客信息集合</param>
        /// <returns></returns>
        public bool AddCustomerList(IList<EyouSoft.Model.TourStructure.TourOrderCustomer> CusList, string OrderId)
        {
            bool IsTrue = false;
            string TourOrderCustomerXML = this.CreateTourOrderCustomerXML(CusList, OrderId);
            DbCommand dc = this._db.GetStoredProcCommand("proc_TourOrderCustomer_Insert");
            this._db.AddInParameter(dc, "TourCustomerXML", DbType.String, TourOrderCustomerXML);
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
        /// 修改游客信息
        /// </summary>
        /// <param name="CusList">订单游客信息集合</param>
        /// <returns></returns>
        public bool UpdateCustomerList(IList<EyouSoft.Model.TourStructure.TourOrderCustomer> CusList, string OrderId)
        {
            bool IsTrue = false;
            string TourOrderCustomerXML = this.CreateTourOrderCustomerXML(CusList, OrderId);
            DbCommand dc = this._db.GetStoredProcCommand("proc_TourOrderCustomer_Update");
            this._db.AddInParameter(dc, "TourCustomerXML", DbType.String, TourOrderCustomerXML);
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
        /// 获取单个游客是否可删除以及其票务状态
        /// </summary>
        /// <param name="CustomerId">游客编号</param>
        /// <param name="Msg">返回的信息</param>
        /// <returns></returns>
        public bool IsDoDelete(string CustomerId, ref string Msg)
        {
            bool IsTrue = false;
            DbCommand dc = this._db.GetStoredProcCommand("proc_CustomerStatus_Return");
            this._db.AddInParameter(dc, "CustomerId", DbType.AnsiStringFixedLength, CustomerId);
            this._db.AddOutParameter(dc, "StatusMsg", DbType.Int32, 4);
            DbHelper.RunProcedure(dc, _db);
            object Result = this._db.GetParameterValue(dc, "StatusMsg");
            if (!Result.Equals(null))
            {
                switch (int.Parse(Result.ToString()))
                {
                    case -2:
                        Msg = "已退团";
                        break;
                    case -1:
                        Msg = "退票申请";
                        break;
                    case 0:
                        Msg = string.Empty;
                        IsTrue = true;
                        break;
                    case 1:
                        Msg = "机票申请";
                        break;
                    case 2:
                        Msg = "未出票";
                        break;
                    case 3:
                        Msg = "已出票";
                        break;
                    case 4:
                        Msg = "已退票";
                        break;
                }
            }
            return IsTrue;
        }
        #endregion 游客基本信息

        #region 游客退团
        /// <summary>
        /// 获取退团信息实体
        /// </summary>
        /// <param name="CustomerId">游客编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.CustomerLeague GetLeague(string CustomerId)
        {
            dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);
            return (from item in dcDal.CustomerLeague
                    where item.CustormerId == CustomerId
                    select new EyouSoft.Model.TourStructure.CustomerLeague
                    {
                        CustormerId = item.CustormerId,
                        OperatorID = item.OperatorID,
                        OperatorName = item.OperatorName,
                        RefundAmount = item.RefundAmount,
                        RefundReason = item.RefundReason

                    }).FirstOrDefault();
        }
        /// <summary>
        /// 新增退团
        /// </summary>
        /// <param name="model">游客退团信息实体</param>
        /// <param name="tourId">团队Id，返回参数</param>
        /// <param name="orderId">订单Id，返回参数</param>
        /// <returns></returns>
        public bool AddLeague(CustomerLeague model, ref string tourId, ref string orderId)
        {
            bool isTrue = false;
            DbCommand dc = this._db.GetStoredProcCommand("proc_TourOrderCustomer_AddLeague");
            this._db.AddInParameter(dc, "CustormerId", DbType.AnsiStringFixedLength, model.CustormerId);
            this._db.AddInParameter(dc, "OperatorName", DbType.String, model.OperatorName);
            this._db.AddInParameter(dc, "OperatorID", DbType.Int32, model.OperatorID);
            this._db.AddInParameter(dc, "RefundReason", DbType.String, model.RefundReason);
            this._db.AddInParameter(dc, "RefundAmount", DbType.Currency, model.RefundAmount);
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            _db.AddInParameter(dc, "AddOrEdit", DbType.Int32, 0);
            _db.AddOutParameter(dc, "ReturnTourId", DbType.AnsiStringFixedLength, 36);
            _db.AddOutParameter(dc, "ReturnOrderId", DbType.AnsiStringFixedLength, 36);

            DbHelper.RunProcedure(dc, _db);

            if (_db.GetParameterValue(dc, "ReturnTourId") != null)
                tourId = _db.GetParameterValue(dc, "ReturnTourId").ToString();
            if (_db.GetParameterValue(dc, "ReturnOrderId") != null)
                orderId = _db.GetParameterValue(dc, "ReturnOrderId").ToString();

            object result = this._db.GetParameterValue(dc, "Result");
            if (!result.Equals(null))
            {
                isTrue = int.Parse(result.ToString()) > 0 ? true : false;
            }

            return isTrue;
        }
        /// <summary>
        /// 修改退团
        /// </summary>
        /// <param name="model">游客退团信息实体</param>
        /// <param name="tourId">团队Id，返回参数</param>
        /// <param name="orderId">订单Id，返回参数</param>
        /// <returns></returns>
        public bool UpdateLeague(CustomerLeague model, ref string tourId, ref string orderId)
        {
            bool isTrue = false;
            DbCommand dc = this._db.GetStoredProcCommand("proc_TourOrderCustomer_AddLeague");
            this._db.AddInParameter(dc, "CustormerId", DbType.AnsiStringFixedLength, model.CustormerId);
            this._db.AddInParameter(dc, "OperatorName", DbType.String, model.OperatorName);
            this._db.AddInParameter(dc, "OperatorID", DbType.Int32, model.OperatorID);
            this._db.AddInParameter(dc, "RefundReason", DbType.String, model.RefundReason);
            this._db.AddInParameter(dc, "RefundAmount", DbType.Currency, model.RefundAmount);
            this._db.AddOutParameter(dc, "Result", DbType.Int32, 4);
            _db.AddInParameter(dc, "AddOrEdit", DbType.Int32, 1);
            _db.AddOutParameter(dc, "ReturnTourId", DbType.AnsiStringFixedLength, 36);
            _db.AddOutParameter(dc, "ReturnOrderId", DbType.AnsiStringFixedLength, 36);

            DbHelper.RunProcedure(dc, _db);

            if (_db.GetParameterValue(dc, "ReturnTourId") != null)
                tourId = _db.GetParameterValue(dc, "ReturnTourId").ToString();
            if (_db.GetParameterValue(dc, "ReturnOrderId") != null)
                orderId = _db.GetParameterValue(dc, "ReturnOrderId").ToString();

            object result = this._db.GetParameterValue(dc, "Result");
            if (!result.Equals(null))
            {
                isTrue = int.Parse(result.ToString()) > 0 ? true : false;
            }

            return isTrue;
        }
        #endregion 游客退团

        #region 游客退票
        /// <summary>
        /// 获取退票信息实体
        /// </summary>
        /// <param name="Id">退票编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.CustomerRefund GetCustomerRefund(string Id)
        {
            EyouSoft.Model.TourStructure.CustomerRefund model = null;
            string TourNo = string.Empty;
            string StrSql = "SELECT a.*,b.VisitorName,b.CradType,b.CradNumber FROM tbl_CustomerRefund a Left join tbl_TourOrderCustomer b ON a.CustormerId=b.id Where a.id=@Id";
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            this._db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, Id);
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                while (dr.Read())
                {
                    model = new EyouSoft.Model.TourStructure.CustomerRefund()
                    {
                        CustormerId = dr.IsDBNull(dr.GetOrdinal("CustormerId")) ? "" : dr.GetString(dr.GetOrdinal("CustormerId")),
                        OperatorName = dr.IsDBNull(dr.GetOrdinal("OperatorName")) ? "" : dr.GetString(dr.GetOrdinal("OperatorName")),
                        RefundNote = dr.IsDBNull(dr.GetOrdinal("RefundNote")) ? "" : dr.GetString(dr.GetOrdinal("RefundNote")),
                        RouteName = dr.IsDBNull(dr.GetOrdinal("RouteName")) ? "" : dr.GetString(dr.GetOrdinal("RouteName")),
                        OperatorID = dr.IsDBNull(dr.GetOrdinal("OperatorID")) ? 0 : dr.GetInt32(dr.GetOrdinal("OperatorID")),
                        IsRefund = (EyouSoft.Model.EnumType.PlanStructure.TicketRefundSate)dr.GetByte(dr.GetOrdinal("IsRefund")),
                        IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime")),
                        RefundAmount = dr.IsDBNull(dr.GetOrdinal("RefundAmount")) ? 0 : dr.GetDecimal(dr.GetOrdinal("RefundAmount")),
                        Id = Id,
                        VisitorName = dr.IsDBNull(dr.GetOrdinal("VisitorName")) ? "" : dr.GetString(dr.GetOrdinal("VisitorName")),
                        CradNumber = dr.IsDBNull(dr.GetOrdinal("CradNumber")) ? "" : dr.GetString(dr.GetOrdinal("CradNumber")),
                        CradType = (EyouSoft.Model.EnumType.TourStructure.CradType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.CradType), dr.GetByte(dr.GetOrdinal("CradType")).ToString())
                    };
                    model.TourNo = GetTourNo(model.CustormerId);
                }
            }
            return model;
        }
        /// <summary>
        /// 新增退票
        /// </summary>
        /// <param name="model">游客退票信息实体</param>
        /// <param name="TourId">团队编号</param>
        /// <returns></returns>
        public bool AddCustomerRefund(EyouSoft.Model.TourStructure.CustomerRefund model, ref string TourId)
        {
            bool IsTrue = false;
            DbCommand dc = this._db.GetStoredProcCommand("proc_TourOrderCustomer_AddRefund");
            model.Id = Guid.NewGuid().ToString();
            this._db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, model.Id);
            this._db.AddInParameter(dc, "CustormerId", DbType.AnsiStringFixedLength, model.CustormerId);
            this._db.AddInParameter(dc, "RouteName", DbType.String, model.RouteName);
            this._db.AddInParameter(dc, "OperatorName", DbType.String, model.OperatorName);
            this._db.AddInParameter(dc, "OperatorID", DbType.Int32, model.OperatorID);
            this._db.AddInParameter(dc, "RefundAmount", DbType.Decimal, model.RefundAmount);
            this._db.AddInParameter(dc, "RefundNote", DbType.String, model.RefundNote);
            this._db.AddOutParameter(dc, "OutTourId", DbType.AnsiStringFixedLength, 36);
            this._db.AddInParameter(dc, "RefundFlightXML", DbType.String, this.GetRefundFlightXML(model));
            DbHelper.RunProcedure(dc, _db);
            object Result = this._db.GetParameterValue(dc, "OutTourId");
            if (!Result.Equals(null))
            {
                TourId = Result.ToString();
                IsTrue = TourId.Length > 0 ? true : false;
            }
            return IsTrue;
        }
        /// <summary>
        /// 修改退票
        /// </summary>
        /// <param name="model">游客退票信息实体</param>
        /// <param name="TourId">团队编号</param>
        /// <returns></returns>
        public bool UpdateCustomerRefund(EyouSoft.Model.TourStructure.CustomerRefund model, ref string TourId)
        {
            bool IsTrue = false;
            DbCommand dc = this._db.GetStoredProcCommand("proc_TourOrderCustomer_UpdateRefund");
            this._db.AddInParameter(dc, "Id", DbType.AnsiStringFixedLength, model.Id);
            this._db.AddInParameter(dc, "CustormerId", DbType.AnsiStringFixedLength, model.CustormerId);
            this._db.AddInParameter(dc, "RouteName", DbType.String, model.RouteName);
            this._db.AddInParameter(dc, "OperatorName", DbType.String, model.OperatorName);
            this._db.AddInParameter(dc, "OperatorID", DbType.Int32, model.OperatorID);
            this._db.AddInParameter(dc, "RefundAmount", DbType.Decimal, model.RefundAmount);
            this._db.AddInParameter(dc, "RefundNote", DbType.String, model.RefundNote);
            this._db.AddOutParameter(dc, "OutTourId", DbType.AnsiStringFixedLength, 36);
            this._db.AddInParameter(dc, "RefundFlightXML", DbType.String, this.GetRefundFlightXML(model));
            DbHelper.RunProcedure(dc, _db);
            object Result = this._db.GetParameterValue(dc, "OutTourId");
            if (!Result.Equals(null))
            {
                TourId = Result.ToString();
                IsTrue = TourId.Length > 0 ? true : false;
            }
            return IsTrue;
        }

        /// <summary>
        /// 获取游客的所有航段信息
        /// </summary>
        /// <param name="CustomerId">游客Id</param>
        /// <returns>游客的所有航段信息</returns>
        public Model.TourStructure.MCustomerAllFlight GetCustomerAllFlight(string CustomerId)
        {
            Model.TourStructure.MCustomerAllFlight model = null;
            if (string.IsNullOrEmpty(CustomerId))
                return model;

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select RefundId,FlightId,CustomerId from tbl_CustomerRefundFlight where CustomerId = @CustomerId; ");
            strSql.Append(" select ID,FligthSegment,DepartureTime,AireLine,Discount,TicketId,TicketTime,(select [State] from tbl_PlanTicketOut where tbl_PlanTicketOut.ID = tbl_PlanTicketFlight.TicketId) as TicketState from tbl_PlanTicketFlight where TicketId in (select TicketOutId from tbl_PlanTicketOutCustomer where UserId = @CustomerId); ");
            DbCommand dc = this._db.GetSqlStringCommand(strSql.ToString());
            this._db.AddInParameter(dc, "CustomerId", DbType.AnsiStringFixedLength, CustomerId);

            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                model = new EyouSoft.Model.TourStructure.MCustomerAllFlight();
                model.RefundFlights = new List<Model.TourStructure.MCustomerRefundFlight>();
                Model.TourStructure.MCustomerRefundFlight RFModel = null;
                model.TicketFlights = new List<Model.PlanStructure.MTicketFlightAndState>();
                Model.PlanStructure.MTicketFlightAndState TFModel = null;
                while (dr.Read())
                {
                    RFModel = new EyouSoft.Model.TourStructure.MCustomerRefundFlight();
                    if (!dr.IsDBNull(0))
                        RFModel.RefundId = dr.GetString(0);
                    if (!dr.IsDBNull(1))
                        RFModel.FlightId = dr.GetInt32(1);
                    if (!dr.IsDBNull(2))
                        RFModel.CustomerId = dr.GetString(2);

                    model.RefundFlights.Add(RFModel);
                }

                dr.NextResult();
                while (dr.Read())
                {
                    TFModel = new EyouSoft.Model.PlanStructure.MTicketFlightAndState();
                    if (!dr.IsDBNull(0))
                        TFModel.ID = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        TFModel.FligthSegment = dr.GetString(1);
                    if (!dr.IsDBNull(2))
                        TFModel.DepartureTime = dr.GetDateTime(2);
                    if (!dr.IsDBNull(3))
                        TFModel.AireLine = (EyouSoft.Model.EnumType.PlanStructure.FlightCompany)dr.GetByte(3);
                    if (!dr.IsDBNull(4))
                        TFModel.Discount = dr.GetDecimal(4);
                    if (!dr.IsDBNull(5))
                        TFModel.TicketId = dr.GetString(5);
                    if (!dr.IsDBNull(6))
                        TFModel.TicketTime = dr.GetString(6);
                    if (!dr.IsDBNull(7))
                        TFModel.Status = (EyouSoft.Model.EnumType.PlanStructure.TicketState)dr.GetByte(7);

                    model.TicketFlights.Add(TFModel);
                }
            }

            return model;
        }

        /// <summary>
        /// 根据游客已退航段信息集合生成SqlXML
        /// </summary>
        /// <param name="list">游客已退航段信息集合</param>
        /// <returns>SqlXML</returns>
        private string GetRefundFlightXML(EyouSoft.Model.TourStructure.CustomerRefund model)
        {
            if (model.CustomerRefundFlights == null || model.CustomerRefundFlights.Count <= 0 || model == null || string.IsNullOrEmpty(model.Id) || string.IsNullOrEmpty(model.CustormerId))
                return string.Empty;

            StringBuilder strXML = new StringBuilder("<ROOT>");

            foreach (var t in model.CustomerRefundFlights)
            {
                if (t == null)
                    continue;

                strXML.AppendFormat("<CustomerRefundFlight_Add RefundId = \"{0}\" FlightId = \"{1}\" CustomerId = \"{2}\" />", model.Id, t.FlightId, model.CustormerId);
            }

            strXML.Append("</ROOT>");

            return strXML.ToString();
        }

        #endregion 游客退票

        #region 游客特服
        /// <summary>
        /// 增加特服
        /// </summary>
        /// <param name="model">游客特服实体</param>
        /// <returns></returns>
        public bool AddSpecialService(EyouSoft.Model.TourStructure.CustomerSpecialService model)
        {
            string StrSql = "INSERT INTO [tbl_CustomerSpecialService] ([CustormerId],[ProjectName],[ServiceDetail],[IsAdd],[Fee],[IssueTime])VALUES(@CustormerId,@ProjectName,@ServiceDetail,@IsAdd,@Fee,@IssueTime) ";
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            this._db.AddInParameter(dc, "CustormerId", DbType.AnsiStringFixedLength, model.CustormerId);
            this._db.AddInParameter(dc, "ProjectName", DbType.String, model.ProjectName);
            this._db.AddInParameter(dc, "ServiceDetail", DbType.String, model.ServiceDetail);
            this._db.AddInParameter(dc, "IsAdd", DbType.Byte, model.IsAdd);
            this._db.AddInParameter(dc, "Fee", DbType.Decimal, model.Fee);
            this._db.AddInParameter(dc, "IssueTime", DbType.DateTime, System.DateTime.Now);
            return DbHelper.ExecuteSql(dc, this._db) > 0 ? true : false;
        }
        /// <summary>
        /// 获取特服信息
        /// </summary>
        /// <param name="CustomerId">游客编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.CustomerSpecialService GetSpecialService(string CustomerId)
        {
            EyouSoft.Model.TourStructure.CustomerSpecialService model = null;
            dcDal = new EyouSoft.Data.EyouSoftTBL(this._db.ConnectionString);
            EyouSoft.Data.CustomerSpecialService DataModel = dcDal.CustomerSpecialService.FirstOrDefault(item => item.CustormerId == CustomerId);
            if (DataModel != null)
            {
                model = new EyouSoft.Model.TourStructure.CustomerSpecialService()
                {
                    CustormerId = DataModel.CustormerId,
                    Fee = DataModel.Fee,
                    IsAdd = DataModel.IsAdd.ToString() == "1" ? true : false,
                    ProjectName = DataModel.ProjectName,
                    ServiceDetail = DataModel.ServiceDetail
                };
            }
            return model;
        }
        /// <summary>
        /// 修改特服
        /// </summary>
        /// <param name="model">游客特服实体</param>
        /// <returns></returns>
        public bool UpdateSpecialService(EyouSoft.Model.TourStructure.CustomerSpecialService model)
        {
            string StrSql = "UPDATE [tbl_CustomerSpecialService] SET [ProjectName]=@ProjectName,[ServiceDetail]=@ServiceDetail,[IsAdd]=@IsAdd,[Fee]=@Fee WHERE CustormerId=@CustormerId";
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            this._db.AddInParameter(dc, "CustormerId", DbType.AnsiStringFixedLength, model.CustormerId);
            this._db.AddInParameter(dc, "ProjectName", DbType.String, model.ProjectName);
            this._db.AddInParameter(dc, "ServiceDetail", DbType.String, model.ServiceDetail);
            this._db.AddInParameter(dc, "IsAdd", DbType.Byte, model.IsAdd);
            this._db.AddInParameter(dc, "Fee", DbType.Decimal, model.Fee);
            return DbHelper.ExecuteSql(dc, this._db) > 0 ? true : false;
        }
        #endregion 游客特服

        #region 其他方法
        /// <summary>
        /// 根据团队编号获取销售员信息集合
        /// </summary>
        /// <param name="TourId">团队编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.StatisticStructure.StatisticOperator> GetSalerInfo(string TourId)
        {
            IList<EyouSoft.Model.StatisticStructure.StatisticOperator> ResultList = null;
            string StrSql = "SELECT SalerId,SalerName FROM tbl_TourOrder WHERE TourId=@TourId";
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            this._db.AddInParameter(dc, "TourId", DbType.AnsiStringFixedLength, TourId);
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                ResultList = new List<EyouSoft.Model.StatisticStructure.StatisticOperator>();
                if (dr.Read())
                {
                    EyouSoft.Model.StatisticStructure.StatisticOperator model = new EyouSoft.Model.StatisticStructure.StatisticOperator()
                    {
                        OperatorId = dr.GetInt32(dr.GetOrdinal("SalerId")),
                        OperatorName = dr.IsDBNull(dr.GetOrdinal("SalerName")) ? "" : dr.GetString(dr.GetOrdinal("SalerName"))
                    };
                    ResultList.Add(model);
                    model = null;
                }
            }
            return ResultList;
        }
        /// <summary>
        /// 根据订单编号获取该订单组团公司的联系信息
        /// </summary>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CustomerInfo GetCustomerInfo(string OrderId)
        {
            string StrSql = "SELECT Name,ContactName,Phone,Mobile,Fax FROM tbl_Customer WHERE ID=(SELECT BuyCompanyId FROM tbl_TourOrder WHERE ID=@OrderId)";
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            this._db.AddInParameter(dc, "OrderId", DbType.AnsiStringFixedLength, OrderId);
            EyouSoft.Model.CompanyStructure.CustomerInfo model = null;
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                while (dr.Read())
                {
                    model = new EyouSoft.Model.CompanyStructure.CustomerInfo()
                    {
                        Name = dr.IsDBNull(dr.GetOrdinal("Name")) ? "" : dr.GetString(dr.GetOrdinal("Name")),
                        ContactName = dr.IsDBNull(dr.GetOrdinal("ContactName")) ? "" : dr.GetString(dr.GetOrdinal("ContactName")),
                        Phone = dr.IsDBNull(dr.GetOrdinal("Phone")) ? "" : dr.GetString(dr.GetOrdinal("Phone")),
                        Mobile = dr.IsDBNull(dr.GetOrdinal("Mobile")) ? "" : dr.GetString(dr.GetOrdinal("Mobile")),
                        Fax = dr.IsDBNull(dr.GetOrdinal("Fax")) ? "" : dr.GetString(dr.GetOrdinal("Fax")),
                    };
                }
            }
            return model;
        }
        /// <summary>
        /// 根据订单状态获取组团公司各订单状态的订单的数量
        /// </summary>
        /// <param name="BuyCompanyId">组团公司编号</param>
        /// <returns></returns>
        public int GetOrderCountByBuyCompanyId(int BuyCompanyId, EyouSoft.Model.EnumType.TourStructure.OrderState? OrderState)
        {
            int RowCount = 0;
            string StrSql = string.Format("SELECT COUNT(1) FROM tbl_TourOrder tt WHERE BuyCompanyId={0} ", BuyCompanyId);
            if (OrderState.HasValue)
                StrSql += string.Format("  AND OrderState={0} ", (int)OrderState);
            //if (OrderState.HasValue && OrderState.Value == EyouSoft.Model.EnumType.TourStructure.OrderState.已留位)
            //{
            //    StrSql += string.Format(" AND DATEDIFF(mi,tt.SaveSeatDate,getdate())>=CAST((SELECT TOP 1 FieldValue FROM tbl_CompanySetting WHERE Id=(SELECT TOP 1 CompanyId FROM tbl_Customer WHERE Id={0})) AS INT)", BuyCompanyId);
            //}
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            object Result = DbHelper.GetSingle(dc, this._db);
            if (Result != null)
            {
                RowCount = int.Parse(Result.ToString());
            }
            return RowCount;
        }
        /// <summary>
        /// 根据收退款id获取订单Id
        /// </summary>
        /// <param name="RefundId">收退款id</param>
        /// <returns></returns>
        public string GetOrderIdByRefundId(string RefundId)
        {
            return (from item in dcDal.ReceiveRefund where item.Id == RefundId select item.ItemId).FirstOrDefault().ToString();
        }
        /// <summary>
        /// 根据团队Id获取订单Id
        /// </summary>
        /// <returns></returns>
        public string GetOrderIdByTourId(string TourId)
        {
            string OrderId = string.Empty;
            string StrSql = "SELECT [id] FROM [tbl_TourOrder] WHERE [TourId]=@TourId ";
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            this._db.AddInParameter(dc, "TourId", DbType.AnsiStringFixedLength, TourId);
            OrderId = DbHelper.GetSingle(dc, this._db).ToString();
            return OrderId;
        }
        /// <summary>
        /// 数组转换成字符串已逗号隔开
        /// </summary>
        /// <param name="item">要转换的数组</param>        
        /// <returns></returns>
        public string ConvertIntArrayTostring(int[] Item)
        {
            string Result = string.Empty;
            foreach (int i in Item)
            {
                Result += i.ToString() + ",";
            }
            Result = Result.Trim(',');
            return Result;
        }
        /// <summary>
        /// 获取计调信息集合
        /// </summary>
        /// <param name="TourId">团队编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOperator> GetOperatorList(string TourId)
        {
            EyouSoft.Model.TourStructure.TourOperator model = null;
            IList<EyouSoft.Model.TourStructure.TourOperator> ResultList = null;
            #region Sql相关处理
            StringBuilder StrSql = new StringBuilder();
            StrSql.Append("SELECT a.OperatorId,b.UserName AS OperatorName,b.ContactTel,b.ContactMobile");
            StrSql.Append(",b.ContactName FROM [tbl_TourOperator] a left join tbl_CompanyUser b on a.OperatorId=b.Id");
            StrSql.AppendFormat(" WHERE a.[TourId]='{0}' ", TourId);
            #endregion Sql相关处理
            DbCommand dc = this._db.GetSqlStringCommand(StrSql.ToString());
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                ResultList = new List<EyouSoft.Model.TourStructure.TourOperator>();
                while (dr.Read())
                {
                    model = new EyouSoft.Model.TourStructure.TourOperator()
                    {
                        OperatorId = dr.GetInt32(dr.GetOrdinal("OperatorId")),
                        ContactName = dr.IsDBNull(dr.GetOrdinal("ContactName")) ? "" : dr.GetString(dr.GetOrdinal("ContactName")),
                        OperatorName = dr.IsDBNull(dr.GetOrdinal("OperatorName")) ? "" : dr.GetString(dr.GetOrdinal("OperatorName")),
                        ContactMobile = dr.IsDBNull(dr.GetOrdinal("ContactMobile")) ? "" : dr.GetString(dr.GetOrdinal("ContactMobile")),
                        ContactTel = dr.IsDBNull(dr.GetOrdinal("ContactTel")) ? "" : dr.GetString(dr.GetOrdinal("ContactTel"))
                    };
                    ResultList.Add(model);
                    model = null;
                }
            }
            return ResultList;
        }
        #endregion

        #region public members
        /// <summary>
        /// 根据订单状态获取订单信息集合
        /// </summary>
        /// <param name="companyId">公司(专线公司)编号</param>
        /// <param name="tourId">计划编号</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>        
        /// <param name="queryInfo">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBOrderStatusTourOrderInfo> GetOrdersByOrderStatus(int companyId, string tourId, EyouSoft.Model.EnumType.TourStructure.OrderState orderStatus, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.OrderCenterSearchInfo queryInfo, string us)
        {
            throw new NotImplementedException("有需要时实现!");
        }

        /// <summary>
        /// 取消订单（订单更改成不受理）
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public bool CancelOrder(string orderId)
        {
            if (orderId == null)
                return false;
            DbCommand cmd = this._db.GetStoredProcCommand("proc_TourOrder_CancelOrder");
            _db.AddInParameter(cmd, "OrderId", DbType.AnsiStringFixedLength, orderId);
            _db.AddOutParameter(cmd, "Result", DbType.Int32, 4);

            DbHelper.RunProcedure(cmd, _db);
            object obj = _db.GetParameterValue(cmd, "Result");
            if (!obj.Equals(null) && int.Parse(obj.ToString()) == 1)
                return true;

            return false;
        }

        /// <summary>
        /// 根据订单编号获取可退票游客信息集合
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrderCustomer> GetCanRefundTicketTravellers(string orderId)
        {
            IList<EyouSoft.Model.TourStructure.TourOrderCustomer> items = new List<EyouSoft.Model.TourStructure.TourOrderCustomer>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetCanRefundTicketTravellers);
            this._db.AddInParameter(cmd, "OrderId", DbType.AnsiStringFixedLength, orderId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.TourStructure.TourOrderCustomer item = new EyouSoft.Model.TourStructure.TourOrderCustomer();

                    item.ID = rdr.GetString(rdr.GetOrdinal("Id"));
                    item.VisitorName = rdr["VisitorName"].ToString();
                    item.CradType = (EyouSoft.Model.EnumType.TourStructure.CradType)rdr.GetByte(rdr.GetOrdinal("CradType"));
                    item.CradNumber = rdr["CradNumber"].ToString();
                    item.Sex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)rdr.GetByte(rdr.GetOrdinal("Sex"));
                    item.VisitorType = (EyouSoft.Model.EnumType.TourStructure.VisitorType)rdr.GetByte(rdr.GetOrdinal("VisitorType"));
                    item.ContactTel = rdr["ContactTel"].ToString();

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 汇总符合条件的订单的总收入，已收金额
        /// </summary>
        /// <param name="dalSearchModel">查询实体</param>
        /// <param name="financeSum">总收入</param>
        /// <param name="hasCheckMoney">已收</param>
        public void GetFinanceSumByOrder(Model.TourStructure.SearchInfoForDAL dalSearchModel, ref decimal financeSum
            , ref decimal hasCheckMoney)
        {
            if (dalSearchModel == null)
                return;

            financeSum = 0;
            hasCheckMoney = 0;
            string strSql = @"select [FinanceSum],[HasCheckMoney] from tbl_TourOrder ";

            #region Sql相关处理

            var query = new StringBuilder();
            StringBuilder strTourWhere = new StringBuilder();
            strTourWhere.Append(" IsDelete='0' AND TemplateId>'' ");
            query.Append(" where IsDelete = 0 ");
            if (dalSearchModel.BuyCompanyId.HasValue && dalSearchModel.BuyCompanyId > 0) //组团公司编号
                query.AppendFormat(" AND [BuyCompanyId]={0} ", dalSearchModel.BuyCompanyId);
            if (dalSearchModel.SellCompanyId.HasValue && dalSearchModel.SellCompanyId > 0)//专线公司编号
            {
                query.AppendFormat(" AND [SellCompanyId]={0} ", dalSearchModel.SellCompanyId);
                strTourWhere.AppendFormat(" and CompanyId = {0} ", dalSearchModel.SellCompanyId);
            }
            if (!string.IsNullOrEmpty(dalSearchModel.OrderNo))//订单号
            {
                query.AppendFormat(" AND OrderNo LIKE '%{0}%'", dalSearchModel.OrderNo);
            }
            if (!string.IsNullOrEmpty(dalSearchModel.TourNo))//团号
            {
                query.AppendFormat(" AND TourNo LIKE '%{0}%'", dalSearchModel.TourNo);
                strTourWhere.AppendFormat(" AND TourCode LIKE '%{0}%'", dalSearchModel.TourNo);
            }
            if (dalSearchModel.TourType.HasValue && dalSearchModel.TourType.Value >= 0)//团队类型
            {
                query.AppendFormat(" AND TourClassId={0}", (int)dalSearchModel.TourType);
                strTourWhere.AppendFormat(" AND TourType={0}", (int)dalSearchModel.TourType);
            }
            if (!string.IsNullOrEmpty(dalSearchModel.CompanyName))//客户单位名称
            {
                query.AppendFormat(" AND BuyCompanyName LIKE '%{0}%'", dalSearchModel.CompanyName);
            }
            if (!string.IsNullOrEmpty(dalSearchModel.RouteName))//线路名称
            {
                query.AppendFormat(" AND RouteName LIKE '%{0}%'", dalSearchModel.RouteName);
                strTourWhere.AppendFormat(" AND RouteName LIKE '%{0}%'", dalSearchModel.RouteName);
            }
            if (dalSearchModel.AreaId.HasValue && dalSearchModel.AreaId.Value > 0)//线路区域编号
            {
                query.AppendFormat(" AND AreaId={0}", dalSearchModel.AreaId);
                strTourWhere.AppendFormat(" AND AreaId={0}", dalSearchModel.AreaId);
            }
            if (dalSearchModel.OperatorId != null && dalSearchModel.OperatorId.Length > 0)//操作人编号
            {
                query.AppendFormat(" AND OperatorId in({0})", this.ConvertIntArrayTostring(dalSearchModel.OperatorId));
            }
            if (dalSearchModel.SalerId != null && dalSearchModel.SalerId.Length > 0)//销售员编号
            {
                query.AppendFormat(" AND SalerId in({0})", this.ConvertIntArrayTostring(dalSearchModel.SalerId));
            }
            if (!string.IsNullOrEmpty(dalSearchModel.SalerName)) //销售员名称
            {
                query.AppendFormat(" AND SalerName LIKE '%{0}%'", dalSearchModel.SalerName);
            }
            if (!string.IsNullOrEmpty(dalSearchModel.OperatorName)) //操作人名称
            {
                query.AppendFormat(" AND OperatorName LIKE '%{0}%'", dalSearchModel.OperatorName);
            }
            if (dalSearchModel.LeaveDateFrom.HasValue)//出团日期 起始
            {
                query.AppendFormat(" AND DATEDIFF(DAY,'{0}',LeaveDate)>=0", dalSearchModel.LeaveDateFrom);
                strTourWhere.AppendFormat(" AND DATEDIFF(DAY,'{0}',LeaveDate)>=0", dalSearchModel.LeaveDateFrom);
            }
            if (dalSearchModel.LeaveDateTo.HasValue)//出团日期 结束
            {
                query.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')>=0", dalSearchModel.LeaveDateTo);
                strTourWhere.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')>=0", dalSearchModel.LeaveDateTo);
            }
            if (dalSearchModel.CreateDateFrom.HasValue)//下单日期 起始
            {
                query.AppendFormat(" AND DATEDIFF(DAY,'{0}',IssueTime)>=0", dalSearchModel.CreateDateFrom);
            }
            if (dalSearchModel.CreateDateTo.HasValue)//下单日期 结束
            {
                query.AppendFormat(" AND DATEDIFF(DAY,IssueTime,'{0}')>=0", dalSearchModel.CreateDateTo);
            }
            if (dalSearchModel.RouteId.HasValue && dalSearchModel.RouteId.Value > 0)//线路编号
            {
                query.AppendFormat(" AND RouteId={0}", dalSearchModel.RouteId);
                strTourWhere.AppendFormat(" AND RouteId={0}", dalSearchModel.RouteId);
            }
            //是否结清
            if (dalSearchModel.IsSettle.HasValue)
            {
                if (dalSearchModel.IsSettle == true)
                {
                    query.Append(" AND HasCheckMoney=FinanceSum ");
                }
                else
                {
                    query.Append(" AND FinanceSum<>HasCheckMoney ");
                }
            }
            if (!string.IsNullOrEmpty(dalSearchModel.HaveUserIds)) //组织框架控制
            {
                query.AppendFormat(" and TourId in (select TourId from tbl_Tour where {0} and OperatorId in ({1})) ",
                                   strTourWhere.ToString(), dalSearchModel.HaveUserIds);
                //query.AppendFormat(" AND ViewOperatorId IN({0}) ", dalSearchModel.HaveUserIds);
            }
            if (dalSearchModel.OrderState != null && dalSearchModel.OrderState.Length > 0)//订单状态
            {
                query.Append(" AND (");
                for (int i = 0; i < dalSearchModel.OrderState.Length; i++)
                {
                    if (i == 0)
                    {
                        query.AppendFormat(" OrderState={0}", (int)dalSearchModel.OrderState[i]);
                    }
                    else
                    {
                        query.AppendFormat(" OR OrderState={0}", (int)dalSearchModel.OrderState[i]);
                    }
                }
                query.Append(" )");
            }

            #endregion Sql相关处理

            strSql += query.ToString();
            strSql += " order by [IssueTime] DESC ";

            DbCommand dc = _db.GetSqlStringCommand(strSql);
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                while (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                        financeSum += dr.GetDecimal(0);
                    if (!dr.IsDBNull(1))
                        hasCheckMoney += dr.GetDecimal(1);
                }
            }
        }

        /// <summary>
        /// 获取统计信息订单列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="searchInfo">查询实体</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourOrder> GetStatisticOrderList(int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.SearchInfoForDAL searchInfo)
        {
            EyouSoft.Model.TourStructure.TourOrder model = null;
            IList<EyouSoft.Model.TourStructure.TourOrder> ResultList = null;
            #region Sql相关处理
            StringBuilder fields = new StringBuilder();
            fields.Append("[ID],[TourNo],[TourId],[RouteName],[LeaveDate],[OrderNo],[BuyCompanyName],TourClassId,[BuyCompanyID],");
            fields.Append("[SumPrice],[HasCheckMoney],[NotCheckMoney],[SalerName],[AdultNumber],");
            fields.Append("[ChildNumber],FinanceSum,[IssueTime],[ContactTel]");
            string TableName = "tbl_TourOrder";
            string orderByString = " [IssueTime] DESC";
            string identityColumnName = "id";
            StringBuilder Query = new StringBuilder();
            StringBuilder strTourWhere = new StringBuilder();
            strTourWhere.Append(" IsDelete='0' AND TemplateId>'' ");
            Query.Append(" IsDelete=0 ");
            if (searchInfo.BuyCompanyId.HasValue && searchInfo.BuyCompanyId > 0)//组团公司编号
            {
                Query.AppendFormat(" AND [BuyCompanyId]={0} ", searchInfo.BuyCompanyId);
            }
            if (searchInfo.SellCompanyId.HasValue && searchInfo.SellCompanyId > 0)//专线公司编号
            {
                Query.AppendFormat(" AND [SellCompanyId]={0} ", searchInfo.SellCompanyId);
                strTourWhere.AppendFormat(" and CompanyId = {0} ", searchInfo.SellCompanyId);
            }
            if (!string.IsNullOrEmpty(searchInfo.OrderNo))//订单号
            {
                Query.AppendFormat(" AND OrderNo LIKE '%{0}%'", searchInfo.OrderNo);
            }
            if (!string.IsNullOrEmpty(searchInfo.TourNo))//团号
            {
                Query.AppendFormat(" AND TourNo LIKE '%{0}%'", searchInfo.TourNo);
                strTourWhere.AppendFormat(" AND TourCode LIKE '%{0}%'", searchInfo.TourNo);
            }
            if (searchInfo.TourType.HasValue && searchInfo.TourType.Value >= 0)//团队类型
            {
                Query.AppendFormat(" AND TourClassId={0}", (int)searchInfo.TourType);
                strTourWhere.AppendFormat(" AND TourType={0}", (int)searchInfo.TourType);
            }
            if (!string.IsNullOrEmpty(searchInfo.CompanyName))//客户单位名称
            {
                Query.AppendFormat(" AND BuyCompanyName LIKE '%{0}%'", searchInfo.CompanyName);
            }
            if (!string.IsNullOrEmpty(searchInfo.RouteName))//线路名称
            {
                Query.AppendFormat(" AND RouteName LIKE '%{0}%'", searchInfo.RouteName);
                strTourWhere.AppendFormat(" AND RouteName LIKE '%{0}%'", searchInfo.RouteName);
            }
            if (searchInfo.AreaId.HasValue && searchInfo.AreaId.Value > 0)//线路区域编号
            {
                Query.AppendFormat(" AND AreaId={0}", searchInfo.AreaId);
                strTourWhere.AppendFormat(" AND AreaId={0}", searchInfo.AreaId);
            }
            if (searchInfo.OperatorId != null && searchInfo.OperatorId.Length > 0)//操作人编号
            {
                Query.AppendFormat(" AND OperatorId in({0})", this.ConvertIntArrayTostring(searchInfo.OperatorId));
            }
            if (searchInfo.SalerId != null && searchInfo.SalerId.Length > 0)//销售员编号
            {
                Query.AppendFormat(" AND SalerId in({0})", this.ConvertIntArrayTostring(searchInfo.SalerId));
            }
            if (!string.IsNullOrEmpty(searchInfo.SalerName)) //销售员名称
            {
                Query.AppendFormat(" AND SalerName LIKE '%{0}%'", searchInfo.SalerName);
            }
            if (!string.IsNullOrEmpty(searchInfo.OperatorName)) //操作人名称
            {
                Query.AppendFormat(" AND OperatorName LIKE '%{0}%'", searchInfo.OperatorName);
            }
            if (searchInfo.LeaveDateFrom.HasValue)//出团日期 起始
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,'{0}',LeaveDate)>=0", searchInfo.LeaveDateFrom);
                strTourWhere.AppendFormat(" AND DATEDIFF(DAY,'{0}',LeaveDate)>=0", searchInfo.LeaveDateFrom);
            }
            if (searchInfo.LeaveDateTo.HasValue)//出团日期 结束
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')>=0", searchInfo.LeaveDateTo);
                strTourWhere.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')>=0", searchInfo.LeaveDateTo);
            }
            if (searchInfo.CreateDateFrom.HasValue)//下单日期 起始
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,'{0}',IssueTime)>=0", searchInfo.CreateDateFrom);
            }
            if (searchInfo.CreateDateTo.HasValue)//下单日期 结束
            {
                Query.AppendFormat(" AND DATEDIFF(DAY,IssueTime,'{0}')>=0", searchInfo.CreateDateTo);
            }
            if (searchInfo.RouteId.HasValue && searchInfo.RouteId.Value > 0)//线路编号
            {
                Query.AppendFormat(" AND RouteId={0}", searchInfo.RouteId);
                strTourWhere.AppendFormat(" AND RouteId={0}", searchInfo.RouteId);
            }
            //是否结清
            if (searchInfo.IsSettle.HasValue)
            {
                if (searchInfo.IsSettle == true)
                {
                    Query.Append(" AND HasCheckMoney=FinanceSum ");
                }
                else
                {
                    Query.Append(" AND FinanceSum<>HasCheckMoney ");
                }
            }
            if (!string.IsNullOrEmpty(searchInfo.HaveUserIds)) //组织框架控制
            {
                Query.AppendFormat(" and TourId in (select TourId from tbl_Tour where {0} and OperatorId in ({1})) ",
                                   strTourWhere.ToString(), searchInfo.HaveUserIds);
            }
            if (searchInfo.OrderState != null && searchInfo.OrderState.Length > 0)//订单状态
            {
                Query.Append(" AND (");
                for (int i = 0; i < searchInfo.OrderState.Length; i++)
                {
                    if (i == 0)
                    {
                        Query.AppendFormat(" OrderState={0}", (int)searchInfo.OrderState[i]);
                    }
                    else
                    {
                        Query.AppendFormat(" OR OrderState={0}", (int)searchInfo.OrderState[i]);
                    }
                }
                Query.Append(" )");
            }
            #endregion Sql相关处理
            using (IDataReader dr = DbHelper.ExecuteReader(_db, pageSize, pageIndex, ref recordCount, TableName, identityColumnName, fields.ToString(), Query.ToString(), orderByString))
            {
                ResultList = new List<EyouSoft.Model.TourStructure.TourOrder>();
                while (dr.Read())
                {
                    model = new EyouSoft.Model.TourStructure.TourOrder();
                    model.ID = dr.GetString(dr.GetOrdinal("id"));
                    model.OrderNo = dr.IsDBNull(dr.GetOrdinal("OrderNo")) ? "" : dr.GetString(dr.GetOrdinal("OrderNo"));
                    model.TourNo = dr.IsDBNull(dr.GetOrdinal("TourNo")) ? "" : dr.GetString(dr.GetOrdinal("TourNo"));
                    model.TourId = dr.IsDBNull(dr.GetOrdinal("TourId")) ? "" : dr.GetString(dr.GetOrdinal("TourId"));
                    model.RouteName = dr.IsDBNull(dr.GetOrdinal("RouteName")) ? "" : dr.GetString(dr.GetOrdinal("RouteName"));
                    model.LeaveDate = dr.GetDateTime(dr.GetOrdinal("LeaveDate"));
                    model.BuyCompanyName = dr.IsDBNull(dr.GetOrdinal("BuyCompanyName")) ? "" : dr.GetString(dr.GetOrdinal("BuyCompanyName"));
                    model.SumPrice = dr.IsDBNull(dr.GetOrdinal("SumPrice")) ? 0 : dr.GetDecimal(dr.GetOrdinal("SumPrice"));
                    model.FinanceSum = dr.IsDBNull(dr.GetOrdinal("FinanceSum")) ? 0 : dr.GetDecimal(dr.GetOrdinal("FinanceSum"));
                    model.HasCheckMoney = dr.IsDBNull(dr.GetOrdinal("HasCheckMoney")) ? 0 : dr.GetDecimal(dr.GetOrdinal("HasCheckMoney"));
                    model.NotCheckMoney = dr.IsDBNull(dr.GetOrdinal("NotCheckMoney")) ? 0 : dr.GetDecimal(dr.GetOrdinal("NotCheckMoney"));
                    model.NotReceived = model.FinanceSum - model.HasCheckMoney;
                    model.SalerName = dr.IsDBNull(dr.GetOrdinal("SalerName")) ? "" : dr.GetString(dr.GetOrdinal("SalerName"));
                    model.AdultNumber = dr.IsDBNull(dr.GetOrdinal("AdultNumber")) ? 0 : dr.GetInt32(dr.GetOrdinal("AdultNumber"));
                    model.ChildNumber = dr.IsDBNull(dr.GetOrdinal("ChildNumber")) ? 0 : dr.GetInt32(dr.GetOrdinal("ChildNumber"));
                    model.PeopleNumber = model.AdultNumber + model.ChildNumber;
                    model.IssueTime = dr.GetDateTime(dr.GetOrdinal("IssueTime"));
                    model.BuyCompanyID = dr.IsDBNull(dr.GetOrdinal("BuyCompanyID")) ? 0 : dr.GetInt32(dr.GetOrdinal("BuyCompanyID"));
                    model.TourClassId = (EyouSoft.Model.EnumType.TourStructure.TourType)Enum.Parse(typeof(EyouSoft.Model.EnumType.TourStructure.TourType), dr.GetByte(dr.GetOrdinal("TourClassId")).ToString());
                    model.ContactTel = dr["ContactTel"].ToString();
                    ResultList.Add(model);
                    model = null;
                }
            }
            return ResultList;
        }

        /// <summary>
        /// 获取财务管理应收帐款信息金额合计
        /// </summary>
        /// <param name="searchInfo">财务订单帐款搜索实体</param>
        /// <param name="isReceived">是否已结清</param>
        /// <param name="us">用户信息集合</param>
        /// <param name="peopleNumber">总人数</param>
        /// <param name="financeSum">应收账款</param>
        /// <param name="hasCheckMoney">已收账款</param>
        /// <param name="notCheckMoney">未审核账款</param>
        /// <param name="NotCheckTuiMoney">未审核退款</param> 
        public void GetTourReciveAccountList(Model.TourStructure.OrderAboutAccountSearchInfo searchInfo, bool? isReceived
            , string us, ref int peopleNumber, ref decimal financeSum, ref decimal hasCheckMoney, ref decimal notCheckMoney, ref decimal NotCheckTuiMoney)
        {

            if (searchInfo == null)
                return;

            #region Sql

            var strSql = new StringBuilder();
            var strTourWhere = new StringBuilder();

            strTourWhere.Append(" IsDelete = '0' and TemplateId > '' ");
            if (isReceived.HasValue)
            {
                strTourWhere.Append(isReceived.Value ? " AND IsSettleInCome = '1' " : " AND IsSettleInCome = '0' ");
            }
            if (!string.IsNullOrEmpty(us))
                strTourWhere.AppendFormat(" AND OperatorId IN ({0}) ", us);

            strSql.Append(" select sum(PeopleNumber-LeaguePepoleNum),sum(FinanceSum),sum(HasCheckMoney),sum(notCheckMoney),sum(RefundMoney) from tbl_TourOrder a left join (SELECT itemid,sum(RefundMoney) as RefundMoney FROM tbl_ReceiveRefund WHERE IsReceive=0 AND IsCheck=0 group by itemid) b on a.ID=b.ItemId where IsDelete = '0' ");
            strSql.AppendFormat(" and TourId in (select TourId from tbl_Tour where {0}) ", strTourWhere.ToString());

            if (!string.IsNullOrEmpty(searchInfo.OrderNo))
                strSql.AppendFormat(" AND OrderNo LIKE '%{0}%'", searchInfo.OrderNo);
            if (!string.IsNullOrEmpty(searchInfo.TourNo))
                strSql.AppendFormat(" AND TourNo LIKE '%{0}%'", searchInfo.TourNo);
            if (!string.IsNullOrEmpty(searchInfo.CompanyName))
                strSql.AppendFormat(" AND BuyCompanyName LIKE '%{0}%'", searchInfo.CompanyName);
            if (searchInfo.LeaveDateFrom.HasValue)
                strSql.AppendFormat(" AND DATEDIFF(DAY,'{0}',LeaveDate) >= 0", searchInfo.LeaveDateFrom);
            if (searchInfo.LeaveDateTo.HasValue)
                strSql.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}') >= 0", searchInfo.LeaveDateTo);
            /*if (searchInfo.CreateDateFrom.HasValue)
                strSql.AppendFormat(" AND DATEDIFF(DAY,'{0}',IssueTime) >= 0", searchInfo.CreateDateFrom);
            if (searchInfo.CreateDateTo.HasValue)
                strSql.AppendFormat(" AND DATEDIFF(DAY,IssueTime,'{0}') >= 0", searchInfo.CreateDateTo);*/

            if (searchInfo.CreateDateFrom.HasValue || searchInfo.CreateDateTo.HasValue)
            {
                strSql.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_ReceiveRefund WHERE ItemId=a.Id AND IsReceive=1 AND ItemType={0} ", (int)EyouSoft.Model.EnumType.TourStructure.ItemType.订单);
                if (searchInfo.CreateDateFrom.HasValue)
                {
                    strSql.AppendFormat(" AND RefundDate>='{0}' ", searchInfo.CreateDateFrom.Value);
                }
                if (searchInfo.CreateDateTo.HasValue)
                {
                    strSql.AppendFormat(" AND RefundDate<='{0}' ", searchInfo.CreateDateTo.Value);
                }

                strSql.Append(")");
            }

            if (searchInfo.SalerId != null && searchInfo.SalerId.Length > 0)
                strSql.AppendFormat(" AND SalerId IN({0})", this.ConvertIntArrayTostring(searchInfo.SalerId));
            if (searchInfo.ComputeOrderType == EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计有效订单)
                strSql.AppendFormat(" AND OrderState IN({0},{1},{2}) "
                    , (int)Model.EnumType.TourStructure.OrderState.未处理
                    , (int)Model.EnumType.TourStructure.OrderState.已成交
                    , (int)Model.EnumType.TourStructure.OrderState.已留位);
            else if (searchInfo.ComputeOrderType == Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单)
                strSql.AppendFormat(" AND OrderState = {0} ", (int)Model.EnumType.TourStructure.OrderState.已成交);
            if (searchInfo.RegisterStatus.HasValue)
            {
                switch (searchInfo.RegisterStatus.Value)
                {
                    case 1:
                        strSql.AppendFormat("AND EXISTS(SELECT 1 FROM tbl_ReceiveRefund WHERE ItemId=a.Id AND IsCheck='0' AND ItemType={0} AND IsReceive=1)", (int)EyouSoft.Model.EnumType.TourStructure.ItemType.订单);
                        break;
                    case 2:
                        strSql.AppendFormat("AND EXISTS(SELECT 1 FROM tbl_ReceiveRefund WHERE ItemId=a.Id AND IsCheck='0' AND ItemType={0} AND IsReceive=0)", (int)EyouSoft.Model.EnumType.TourStructure.ItemType.订单);
                        break;
                }
            }

            if (searchInfo.QueryAmountType.HasValue && searchInfo.QueryAmountOperator.HasValue && searchInfo.QueryAmount.HasValue)
            {
                string _operator = string.Empty;
                switch (searchInfo.QueryAmountOperator.Value)
                {
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.大于等于:
                        _operator = ">=";
                        break;
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.等于:
                        _operator = "=";
                        break;
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.小于等于:
                        _operator = "<=";
                        break;
                }

                string _field = string.Empty;
                switch (searchInfo.QueryAmountType.Value)
                {
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.未收款:
                        _field = "FinanceSum-HasCheckMoney";
                        break;
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.已收待审核:
                        _field = "NotCheckMoney";
                        break;
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.已收款:
                        _field = "HasCheckMoney";
                        break;
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.应收款:
                        _field = "FinanceSum";
                        break;
                    case EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.退款待审核:
                        _field = "(SELECT SUM(RefundMoney) FROM tbl_ReceiveRefund WHERE IsCheck='0' AND IsReceive=0 AND ItemId=a.Id)";
                        break;
                }

                if (!string.IsNullOrEmpty(_operator) && !string.IsNullOrEmpty(_field))
                {
                    strSql.AppendFormat(" AND {0}{1}{2} ", _field, _operator, searchInfo.QueryAmount);
                }

            }

            if (searchInfo != null && searchInfo.TourType.HasValue)
            {
                strSql.AppendFormat(" AND a.TourClassId={0} ", (int)searchInfo.TourType.Value);
            }

            #endregion

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                        peopleNumber = dr.GetInt32(0);
                    if (!dr.IsDBNull(1))
                        financeSum = dr.GetDecimal(1);
                    if (!dr.IsDBNull(2))
                        hasCheckMoney = dr.GetDecimal(2);
                    if (!dr.IsDBNull(3))
                        notCheckMoney = dr.GetDecimal(3);
                    if (!dr.IsDBNull(4))
                        NotCheckTuiMoney = dr.GetDecimal(4);
                }
            }
        }

        /// <summary>
        /// 获取销售收款合计信息业务实体
        /// </summary>
        /// <param name="companyId">公司编号（专线）</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.MSaleReceivableSummaryInfo GetSaleReceivableSummary(int companyId, EyouSoft.Model.TourStructure.SalerSearchInfo searchInfo, string us)
        {
            EyouSoft.Model.TourStructure.MSaleReceivableSummaryInfo info = null;
            StringBuilder cmdText = new StringBuilder();

            #region CommandText
            cmdText.Append(" SELECT ");
            cmdText.Append(" SUM([FinanceSum]) AS TotalAmount ");
            cmdText.Append(" ,SUM([PeopleNumber]-[LeaguePepoleNum]) AS PeopleNumber ");
            cmdText.Append(" ,SUM([HasCheckMoney]) AS ReceivedAmount ");
            cmdText.Append(" ,SUM([NotCheckMoney]) AS UnauditedAmount ");
            cmdText.Append(" FROM [tbl_TourOrder] ");
            cmdText.AppendFormat(" WHERE [SellCompanyId]={0} AND [IsDelete]='0' ", companyId);

            if (searchInfo != null)
            {
                if (!string.IsNullOrEmpty(searchInfo.CompanyName))
                {
                    cmdText.AppendFormat(" AND [BuyCompanyName] LIKE '%{0}%' ", searchInfo.CompanyName);
                }
                if (searchInfo.ComputeOrderType.HasValue)
                {
                    switch (searchInfo.ComputeOrderType.Value)
                    {
                        case EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计确认成交订单:
                            cmdText.AppendFormat(" AND [OrderState]={0} ", (int)EyouSoft.Model.EnumType.TourStructure.OrderState.已成交);
                            break;
                        case EyouSoft.Model.EnumType.CompanyStructure.ComputeOrderType.统计有效订单:
                            cmdText.AppendFormat(" AND [OrderState] NOT IN({0},{1}) ", (int)EyouSoft.Model.EnumType.TourStructure.OrderState.不受理, (int)EyouSoft.Model.EnumType.TourStructure.OrderState.留位过期);
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(searchInfo.OrderNo))
                {
                    cmdText.AppendFormat(" AND [OrderNo] LIKE '%{0}%' ", searchInfo.OrderNo);
                }
                if (!string.IsNullOrEmpty(searchInfo.RouteName))
                {
                    cmdText.AppendFormat(" AND [RouteName] LIKE '%{0}%' ", searchInfo.RouteName);
                }
                if (searchInfo.SalerId != null && searchInfo.SalerId.Length > 0)
                {
                    cmdText.AppendFormat(" AND [SalerId] IN({0}) ", Utils.GetSqlIdStrByArray(searchInfo.SalerId));
                }
                if (!string.IsNullOrEmpty(searchInfo.TourNo))
                {
                    cmdText.AppendFormat(" AND [TourNo] LIKE '%{0}%' ", searchInfo.TourNo);
                }
                if (searchInfo.SDate.HasValue)
                {
                    cmdText.AppendFormat(" AND LeaveDate>='{0}' ", searchInfo.SDate.Value);
                }
                if (searchInfo.EDate.HasValue)
                {
                    cmdText.AppendFormat(" AND LeaveDate<='{0}' ", searchInfo.EDate.Value);
                }
                if (searchInfo.OrderOperatorId != null && searchInfo.OrderOperatorId.Length > 0)
                {
                    cmdText.AppendFormat(" AND OperatorId in({0})", Utils.GetSqlIdStrByArray(searchInfo.OrderOperatorId));
                }
            }

            if (!string.IsNullOrEmpty(us))
            {
                cmdText.AppendFormat(" AND ViewOperatorId IN({0}) ", us);
            }
            #endregion

            DbCommand cmd = this._db.GetSqlStringCommand(cmdText.ToString());
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    info = new EyouSoft.Model.TourStructure.MSaleReceivableSummaryInfo();
                    if (!rdr.IsDBNull(0)) info.TotalAmount = rdr.GetDecimal(0);
                    if (!rdr.IsDBNull(1)) info.PeopleNumber = rdr.GetInt32(1);
                    if (!rdr.IsDBNull(2)) info.ReceivedAmount = rdr.GetDecimal(2);
                    if (!rdr.IsDBNull(3)) info.UnauditedAmount = rdr.GetDecimal(3);
                }
            }

            return info;
        }

        /// <summary>
        /// 机票审核根据团队Id获取订单信息
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="tourId">团队Id</param>
        /// <returns>返回订单部分信息列表</returns>
        public IList<OrderByCheckTicket> GetOrderByCheckTicket(int companyId, string tourId)
        {
            if (companyId <= 0)
                return null;

            IList<OrderByCheckTicket> list;

            var strSql = new StringBuilder();
            strSql.Append(@" select 
                             ID,BuyCompanyName,PeopleNumber,FinanceSum,HasCheckMoney
                             from tbl_tourorder
                             where IsDelete = '0' ");
            strSql.AppendFormat(" and SellCompanyId = {0} ", companyId);
            if (!string.IsNullOrEmpty(tourId))
                strSql.AppendFormat(" and TourId = '{0}' ", tourId);
            strSql.AppendFormat(" AND OrderState IN({0},{1}) ", (int)EyouSoft.Model.EnumType.TourStructure.OrderState.已成交, (int)EyouSoft.Model.EnumType.TourStructure.OrderState.已留位);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                list = new List<OrderByCheckTicket>();
                while (dr.Read())
                {
                    list.Add(new OrderByCheckTicket
                                 {
                                     OrderId = dr.IsDBNull(0) ? string.Empty : dr.GetString(0),
                                     CustomerName = dr.IsDBNull(1) ? string.Empty : dr.GetString(1),
                                     OrderPeople = dr.IsDBNull(2) ? 0 : dr.GetInt32(2),
                                     OrderAmount = dr.IsDBNull(3) ? 0 : dr.GetDecimal(3),
                                     HasCheckMoney = dr.IsDBNull(4) ? 0 : dr.GetDecimal(4)
                                 });
                }
            }

            return list;
        }

        /// <summary>
        /// 机票审核根据团队Id获取订单信息
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="orderId">订单Id</param>
        /// <returns>返回订单部分信息列表</returns>
        public IList<OrderByCheckTicket> GetOrderByCheckTicketByOrderId(int companyId, string orderId)
        {
            if (companyId <= 0)
                return null;

            IList<OrderByCheckTicket> list;

            var strSql = new StringBuilder();
            strSql.Append(@" select 
                             ID,BuyCompanyName,PeopleNumber,FinanceSum,HasCheckMoney
                             from tbl_tourorder
                             where IsDelete = '0' ");
            strSql.AppendFormat(" and SellCompanyId = {0} ", companyId);
            if (!string.IsNullOrEmpty(orderId))
                strSql.AppendFormat(" and ID = '{0}' ", orderId);
            strSql.AppendFormat(" AND OrderState IN({0},{1},{2}) ", (int)EyouSoft.Model.EnumType.TourStructure.OrderState.已成交, (int)EyouSoft.Model.EnumType.TourStructure.OrderState.已留位, (int)EyouSoft.Model.EnumType.TourStructure.OrderState.未处理);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                list = new List<OrderByCheckTicket>();
                while (dr.Read())
                {
                    list.Add(new OrderByCheckTicket
                    {
                        OrderId = dr.IsDBNull(0) ? string.Empty : dr.GetString(0),
                        CustomerName = dr.IsDBNull(1) ? string.Empty : dr.GetString(1),
                        OrderPeople = dr.IsDBNull(2) ? 0 : dr.GetInt32(2),
                        OrderAmount = dr.IsDBNull(3) ? 0 : dr.GetDecimal(3),
                        HasCheckMoney = dr.IsDBNull(4) ? 0 : dr.GetDecimal(4)
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// 根据订单状态获取专线公司各订单状态的订单的数量
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="orderState">订单状态</param>
        /// <returns></returns>
        public int GetOrderCountByCompanyId(int companyId, Model.EnumType.TourStructure.OrderState? orderState)
        {
            int RowCount = 0;
            string StrSql = string.Format("SELECT COUNT(1) FROM tbl_TourOrder tt WHERE SellCompanyId = {0} ", companyId);
            if (orderState.HasValue)
                StrSql += string.Format("  AND OrderState = {0} ", (int)orderState);
            //if (OrderState.HasValue && OrderState.Value == EyouSoft.Model.EnumType.TourStructure.OrderState.已留位)
            //{
            //    StrSql += string.Format(" AND DATEDIFF(mi,tt.SaveSeatDate,getdate())>=CAST((SELECT TOP 1 FieldValue FROM tbl_CompanySetting WHERE Id=(SELECT TOP 1 CompanyId FROM tbl_Customer WHERE Id={0})) AS INT)", BuyCompanyId);
            //}
            DbCommand dc = this._db.GetSqlStringCommand(StrSql);
            object Result = DbHelper.GetSingle(dc, this._db);
            if (Result != null)
            {
                RowCount = int.Parse(Result.ToString());
            }
            return RowCount;
        }

        /// <summary>
        /// 根据订单编号获取订单对方操作员信息
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public EyouSoft.Model.CompanyStructure.CustomerContactInfo GetOrderOtherSideContactInfo(string orderId)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_GetOrderOtherSideContactInfo);
            _db.AddInParameter(cmd, "OrderId", DbType.AnsiStringFixedLength, orderId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    return new EyouSoft.Model.CompanyStructure.CustomerContactInfo()
                    {
                        Name = rdr["Name"].ToString(),
                        Mobile = rdr["Mobile"].ToString()
                    };
                }
            }

            return null;
        }

        /// <summary>
        /// 取消收款审核
        /// </summary>
        /// <param name="companyId">收款登记所在公司编号</param>
        /// <param name="orderId">收款登记的订单编号</param>
        /// <param name="checkedId">要取消收款审核的收款登记编号</param>
        /// <returns></returns>
        public bool CancelIncomeChecked(int companyId, string orderId, string checkedId)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_UPDATE_CancelIncomeChecked);
            _db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, checkedId);
            _db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);
            _db.AddInParameter(cmd, "OrderId", DbType.AnsiStringFixedLength, orderId);


            return DbHelper.ExecuteSql(cmd, _db) == 1;
        }

        /// <summary>
        /// 取消退款审核
        /// </summary>
        /// <param name="companyId">退款登记所在公司编号</param>
        /// <param name="orderId">退款登记的订单编号</param>
        /// <param name="checkedId">要取消退款审核的退款登记编号</param>
        /// <returns></returns>
        public bool CancelIncomeTuiChecked(int companyId, string orderId, string checkedId)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_UPDATE_CancelIncomeTuiChecked);
            _db.AddInParameter(cmd, "Id", DbType.AnsiStringFixedLength, checkedId);
            _db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);
            _db.AddInParameter(cmd, "OrderId", DbType.AnsiStringFixedLength, orderId);


            return DbHelper.ExecuteSql(cmd, _db) == 1;
        }

        /// <summary>
        /// 客户关系交易明细汇总
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="keHuId">客户单位编号</param>
        /// <param name="renCi">人次合计</param>
        /// <param name="jiaoYiJinE">交易金额合计</param>
        /// <param name="yiShouJinE">已收金额合计</param>
        /// <param name="chaXun">查询信息</param>
        public void GetKeHuJiaoYiMingXiHuiZong(int companyId, int keHuId, out int renCi, out decimal jiaoYiJinE, out decimal yiShouJinE, EyouSoft.Model.CompanyStructure.KeHuJiaoYiMingXiChaXunInfo chaXun)
        {
            renCi = 0;
            jiaoYiJinE = 0;
            yiShouJinE = 0;
            StringBuilder cmdText = new StringBuilder();

            #region SQL
            cmdText.Append(" SELECT SUM(A.PeopleNumber-A.LeaguePepoleNum) AS RenCiHeJi,SUM(A.FinanceSum) AS JiaoYiJinEHeJi,SUM(A.HasCheckMoney) AS YiShouJinEHeJi ");
            cmdText.AppendFormat(" FROM tbl_TourOrder AS A WHERE A.IsDelete='0' AND A.SellCompanyId={0} AND A.BuyCompanyId={1} ", companyId, keHuId);
            cmdText.AppendFormat(" AND A.OrderState={0} ", (int)EyouSoft.Model.EnumType.TourStructure.OrderState.已成交);

            if (chaXun != null)
            {
                if (chaXun.LSDate.HasValue || chaXun.LEDate.HasValue || (chaXun.TourTypes != null && chaXun.TourTypes.Length > 0) || chaXun.AreaId.HasValue)
                {
                    cmdText.AppendFormat(" AND EXISTS (SELECT 1 FROM tbl_Tour AS B WHERE B.TourId=A.TourId AND B.IsDelete='0' ");

                    if (chaXun.LSDate.HasValue)
                    {
                        cmdText.AppendFormat(" AND B.LeaveDate>='{0}' ", chaXun.LSDate.Value);
                    }
                    if (chaXun.LEDate.HasValue)
                    {
                        cmdText.AppendFormat(" AND B.LeaveDate<='{0}' ", chaXun.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    /*if (chaXun.TourType.HasValue)
                    {
                        cmdText.AppendFormat(" AND B.TourType={0} ", (int)chaXun.TourType.Value);
                    }*/
                    if (chaXun.TourTypes != null && chaXun.TourTypes.Length > 0)
                    {
                        if (chaXun.TourTypes.Length == 1)
                        {
                            cmdText.AppendFormat(" AND B.TourType={0} ", (int)chaXun.TourTypes[0]);
                        }
                        else
                        {
                            cmdText.AppendFormat(" AND B.TourType IN({0} ", (int)chaXun.TourTypes[0]);
                            for (int i = 1; i < chaXun.TourTypes.Length; i++)
                            {
                                cmdText.AppendFormat(",{0}", (int)chaXun.TourTypes[i]);
                            }
                            cmdText.Append(" ) ");
                        }
                    }
                    if (chaXun.AreaId.HasValue)
                    {
                        cmdText.AppendFormat(" AND AreaId={0} ", chaXun.AreaId.Value);
                    }

                    cmdText.AppendFormat(" ) ");
                }

                if (chaXun.QueryAmountType.HasValue
                    && chaXun.QueryAmountType.HasValue
                    && chaXun.QueryAmount.HasValue
                    && chaXun.QueryAmountType != EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.None
                    && chaXun.QueryAmountOperator != EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.None)
                {
                    string _operator = string.Empty;
                    switch (chaXun.QueryAmountOperator.Value)
                    {
                        case EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.大于等于:
                            _operator = ">=";
                            break;
                        case EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.等于:
                            _operator = "=";
                            break;
                        case EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.小于等于:
                            _operator = "<=";
                            break;
                    }

                    string _field = string.Empty;
                    switch (chaXun.QueryAmountType.Value)
                    {
                        case EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.未收款:
                            _field = "FinanceSum-HasCheckMoney";
                            break;
                    }

                    if (!string.IsNullOrEmpty(_operator) && !string.IsNullOrEmpty(_field))
                    {
                        cmdText.AppendFormat(" AND {0}{1}{2} ", _field, _operator, chaXun.QueryAmount.Value);
                    }
                }
            }
            #endregion

            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(rdr.GetOrdinal("RenCiHeJi"))) renCi = rdr.GetInt32(rdr.GetOrdinal("RenCiHeJi"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("JiaoYiJinEHeJi"))) jiaoYiJinE = rdr.GetDecimal(rdr.GetOrdinal("JiaoYiJinEHeJi"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("YiShouJinEHeJi"))) yiShouJinE = rdr.GetDecimal(rdr.GetOrdinal("YiShouJinEHeJi"));
                }
            }
        }

        /// <summary>
        /// 获取客户关系交易明细
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="companyId">公司编号</param>
        /// <param name="keHuId">客户编号</param>
        /// <param name="chaXun">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.CompanyStructure.KeHuJiaoYiMingXiInfo> GetKeHuJiaoYiMingXi(int pageSize, int pageIndex, ref int recordCount, int companyId, int keHuId, EyouSoft.Model.CompanyStructure.KeHuJiaoYiMingXiChaXunInfo chaXun)
        {
            IList<EyouSoft.Model.CompanyStructure.KeHuJiaoYiMingXiInfo> items = new List<EyouSoft.Model.CompanyStructure.KeHuJiaoYiMingXiInfo>();
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_TourOrder";
            string primaryKey = "Id";
            string orderByString = "IssueTime DESC";
            string fields = "RouteName,TourNo,BuyerTourCode,PeopleNumber,FinanceSum,HasCheckMoney,OrderNo,BuyerContactName,LeaguePepoleNum";

            #region 查询
            cmdQuery.AppendFormat("IsDelete='0' AND SellCompanyId={0} AND BuyCompanyId={1}", companyId, keHuId);
            cmdQuery.AppendFormat(" AND OrderState={0} ", (int)EyouSoft.Model.EnumType.TourStructure.OrderState.已成交);
            if (chaXun != null)
            {
                if (chaXun.LSDate.HasValue || chaXun.LEDate.HasValue || (chaXun.TourTypes != null && chaXun.TourTypes.Length > 0) || chaXun.AreaId.HasValue)
                {
                    cmdQuery.AppendFormat(" AND EXISTS (SELECT 1 FROM tbl_Tour AS B WHERE B.TourId=tbl_TourOrder.TourId AND B.IsDelete='0' ");

                    if (chaXun.LSDate.HasValue)
                    {
                        cmdQuery.AppendFormat(" AND B.LeaveDate>='{0}' ", chaXun.LSDate.Value);
                    }
                    if (chaXun.LEDate.HasValue)
                    {
                        cmdQuery.AppendFormat(" AND B.LeaveDate<='{0}' ", chaXun.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                    }
                    /*if (chaXun.TourType.HasValue)
                    {
                        cmdQuery.AppendFormat(" AND B.TourType={0} ", (int)chaXun.TourType.Value);
                    }*/
                    if (chaXun.TourTypes != null && chaXun.TourTypes.Length > 0)
                    {
                        if (chaXun.TourTypes.Length == 1)
                        {
                            cmdQuery.AppendFormat(" AND B.TourType={0} ", (int)chaXun.TourTypes[0]);
                        }
                        else
                        {
                            cmdQuery.AppendFormat(" AND B.TourType IN({0} ", (int)chaXun.TourTypes[0]);
                            for (int i = 1; i < chaXun.TourTypes.Length; i++)
                            {
                                cmdQuery.AppendFormat(",{0}", (int)chaXun.TourTypes[i]);
                            }
                            cmdQuery.Append(" ) ");
                        }
                    }
                    if (chaXun.AreaId.HasValue)
                    {
                        cmdQuery.AppendFormat(" AND AreaId={0} ", chaXun.AreaId.Value);
                    }

                    cmdQuery.AppendFormat(" ) ");
                }

                if (chaXun.QueryAmountType.HasValue
                    && chaXun.QueryAmountType.HasValue
                    && chaXun.QueryAmount.HasValue
                    && chaXun.QueryAmountType != EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.None
                    && chaXun.QueryAmountOperator != EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.None)
                {
                    string _operator = string.Empty;
                    switch (chaXun.QueryAmountOperator.Value)
                    {
                        case EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.大于等于:
                            _operator = ">=";
                            break;
                        case EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.等于:
                            _operator = "=";
                            break;
                        case EyouSoft.Model.EnumType.FinanceStructure.QueryOperator.小于等于:
                            _operator = "<=";
                            break;
                    }

                    string _field = string.Empty;
                    switch (chaXun.QueryAmountType.Value)
                    {
                        case EyouSoft.Model.EnumType.FinanceStructure.QueryAmountType.未收款:
                            _field = "tbl_TourOrder.FinanceSum-tbl_TourOrder.HasCheckMoney";
                            break;
                    }

                    if (!string.IsNullOrEmpty(_operator) && !string.IsNullOrEmpty(_field))
                    {
                        cmdQuery.AppendFormat(" AND {0}{1}{2} ", _field, _operator, chaXun.QueryAmount.Value);
                    }
                }
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    var item = new EyouSoft.Model.CompanyStructure.KeHuJiaoYiMingXiInfo();
                    item.JiaoYiJInE = rdr.GetDecimal(rdr.GetOrdinal("FinanceSum"));
                    item.OrderCode = rdr["OrderNo"].ToString();
                    item.RenCi = rdr.GetInt32(rdr.GetOrdinal("PeopleNumber")) - rdr.GetInt32(rdr.GetOrdinal("LeaguePepoleNum"));
                    item.RouteName = rdr["RouteName"].ToString();
                    item.TourCode = rdr["TourNo"].ToString();
                    item.YiShouJinE = rdr.GetDecimal(rdr.GetOrdinal("HasCheckMoney"));
                    item.BuyerContactName = rdr["BuyerContactName"].ToString();
                    item.BuyerTourCode = rdr["BuyerTourCode"].ToString();
                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取订单提醒集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="searchInfo">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public IList<Model.TourStructure.MDingDanTiXingInfo> GetDingDanTiXing(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.MDingDanTiXingInfo searchInfo, string us)
        {
            IList<EyouSoft.Model.TourStructure.MDingDanTiXingInfo> items = new List<EyouSoft.Model.TourStructure.MDingDanTiXingInfo>();
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_TourOrder";
            string primaryKey = "Id";
            string orderByString = "IssueTime DESC";
            StringBuilder fields = new StringBuilder();

            #region fields
            fields.Append("Id,OrderNo,TourId,RouteName,TourNo,LeaveDate,BuyCompanyID,BuyCompanyName,PeopleNumber,SumPrice,IssueTime,OrderState,SaveSeatDate");
            #endregion

            #region 拼接查询条件
            cmdQuery.AppendFormat(" SellCompanyId={0} AND IsDelete='0' AND OrderState IN({1},{2}) ", companyId, (int)EyouSoft.Model.EnumType.TourStructure.OrderState.未处理, (int)EyouSoft.Model.EnumType.TourStructure.OrderState.已留位);
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    var item = new EyouSoft.Model.TourStructure.MDingDanTiXingInfo();

                    item.JinE = rdr.GetDecimal(rdr.GetOrdinal("SumPrice"));
                    item.KeHuBianHao = rdr.GetInt32(rdr.GetOrdinal("BuyCompanyID"));
                    item.KeHuMingCheng = rdr["BuyCompanyName"].ToString();
                    item.LDate = rdr.GetDateTime(rdr.GetOrdinal("LeaveDate"));
                    item.OrderCode = rdr.GetString(rdr.GetOrdinal("OrderNo"));
                    item.OrderId = rdr.GetString(rdr.GetOrdinal("Id"));
                    item.OrderStatus = (EyouSoft.Model.EnumType.TourStructure.OrderState)rdr.GetByte(rdr.GetOrdinal("OrderState"));
                    item.RenShu = rdr.GetInt32(rdr.GetOrdinal("PeopleNumber"));
                    item.RouteName = rdr["RouteName"].ToString();
                    item.TourCode = rdr.GetString(rdr.GetOrdinal("TourNo"));
                    item.TourId = rdr.GetString(rdr.GetOrdinal("TourId"));
                    item.XiaDanShiJian = rdr.GetDateTime(rdr.GetOrdinal("IssueTime"));
                    item.LiuWeiShiJian = rdr.GetDateTime(rdr.GetOrdinal("SaveSeatDate"));

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取订单退团人数及退团损失
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="tuiTuanChengRenShu">退团成人数</param>
        /// <param name="tuiTuanErTongShu">退团儿童数</param>
        /// <param name="tuiTuanSunShiJinE">退团损失金额</param>
        public void GetDingDanTuiTuanRenShu(string orderId, out int tuiTuanChengRenShu, out int tuiTuanErTongShu, out decimal tuiTuanSunShiJinE)
        {
            tuiTuanChengRenShu = 0;
            tuiTuanErTongShu = 0;
            tuiTuanSunShiJinE = 0;

            int tuiTuanZongRenShu = 0;

            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_GetDingDanTuiTuanRenShu);
            _db.AddInParameter(cmd, "OrderId", DbType.AnsiStringFixedLength, orderId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) tuiTuanZongRenShu = rdr.GetInt32(0);
                    if (!rdr.IsDBNull(1)) tuiTuanErTongShu = rdr.GetInt32(1);
                    if (!rdr.IsDBNull(2)) tuiTuanSunShiJinE = rdr.GetDecimal(2);
                }
            }

            tuiTuanChengRenShu = tuiTuanZongRenShu - tuiTuanErTongShu;
        }
        #endregion

        #region 送机计划表

        /// <summary>
        /// 获取送机计划表
        /// </summary>
        /// <param name="trafficId">交通编号</param>
        /// <param name="leaveDate">出团日期</param>
        /// <returns></returns>
        public IList<Model.TourStructure.SongJiJiHuaBiao> GetSongJiJiHuaBiao(int trafficId, DateTime leaveDate)
        {
            if (trafficId <= 0) return null;

            IList<Model.TourStructure.SongJiJiHuaBiao> list = null;
            var strSql = new StringBuilder();
            strSql.Append(" SELECT to1.Id,to1.TourClassId,to1.RouteName,to1.BuyCompanyID,to1.BuyCompanyName,to1.AdultNumber,to1.ChildNumber,to1.MarketNumber,to1.OperatorContent,to1.LeaguePepoleNum ");
            strSql.AppendFormat(
                " ,(SELECT * FROM tbl_TourOrderCustomer toc WHERE toc.OrderId = to1.ID AND toc.CustomerStatus = {0} AND toc.TicketStatus = {1} FOR XML RAW,ROOT('Root')) AS CustomerList ",
                (int)Model.EnumType.TourStructure.CustomerStatus.正常,
                (int)Model.EnumType.TourStructure.CustomerTicketStatus.正常);
            strSql.Append(" FROM tbl_TourOrder to1 ");
            strSql.Append(" where ");
            strSql.AppendFormat(
                " EXISTS (SELECT 1 FROM tbl_TourOrderTraffic tot WHERE tot.OrderId = to1.ID AND tot.TrafficId = {0}) ",
                trafficId);
            strSql.AppendFormat(" AND DATEDIFF(dd,to1.LeaveDate,'{0}') = 0 ", leaveDate.ToShortDateString());
            strSql.AppendFormat(" AND to1.OrderState={0} ", (int)EyouSoft.Model.EnumType.TourStructure.OrderState.已成交);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                list = new List<Model.TourStructure.SongJiJiHuaBiao>();
                while (dr.Read())
                {
                    var model = new Model.TourStructure.SongJiJiHuaBiao();
                    if (!dr.IsDBNull(dr.GetOrdinal("Id")))
                        model.OrderId = dr.GetString(dr.GetOrdinal("Id"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TourClassId")))
                        model.TourType = (Model.EnumType.TourStructure.TourType)dr.GetByte(dr.GetOrdinal("TourClassId"));
                    if (!dr.IsDBNull(dr.GetOrdinal("RouteName")))
                        model.RouteName = dr.GetString(dr.GetOrdinal("RouteName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("BuyCompanyID")))
                        model.BuyCompanyId = dr.GetInt32(dr.GetOrdinal("BuyCompanyID"));
                    if (!dr.IsDBNull(dr.GetOrdinal("BuyCompanyName")))
                        model.BuyCompanyName = dr.GetString(dr.GetOrdinal("BuyCompanyName"));
                    if (!dr.IsDBNull(dr.GetOrdinal("AdultNumber")))
                    {
                        if (dr.IsDBNull(dr.GetOrdinal("LeaguePepoleNum")))
                            model.ChenRenShu = dr.GetInt32(dr.GetOrdinal("AdultNumber"));
                        else
                        {
                            model.ChenRenShu = dr.GetInt32(dr.GetOrdinal("AdultNumber"))
                                               - dr.GetInt32(dr.GetOrdinal("LeaguePepoleNum"));
                        }
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("ChildNumber")))
                    {
                        model.ErTongShu = dr.GetInt32(dr.GetOrdinal("ChildNumber"));
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("MarketNumber")))
                        model.QuanPeiShu = dr.GetInt32(dr.GetOrdinal("MarketNumber"));

                    if (!dr.IsDBNull(dr.GetOrdinal("CustomerList")))
                        model.OrderCustomers = this.GetOrderCustomer(dr.GetString(dr.GetOrdinal("CustomerList")));

                    if (!string.IsNullOrEmpty(model.OrderId))
                    {
                        model.TourOrderAmountPlusInfo = this.GetOrderAmountPlus(model.OrderId);
                    }

                    list.Add(model);
                }

            }

            return list;
        }

        /// <summary>
        /// 根据订单游客
        /// </summary>
        /// <param name="strOrderCustomerXml">订单游客信息SqlXML</param>
        /// <returns></returns>
        private IList<Model.TourStructure.TourOrderCustomer> GetOrderCustomer(string strOrderCustomerXml)
        {
            IList<Model.TourStructure.TourOrderCustomer> list = null;
            if (string.IsNullOrEmpty(strOrderCustomerXml)) return list;

            var xRoot = XElement.Parse(strOrderCustomerXml);
            var xRows = Utils.GetXElements(xRoot, "row");
            if (xRows == null || !xRows.Any()) return list;

            list = new List<Model.TourStructure.TourOrderCustomer>();
            foreach (var t in xRows)
            {
                if (t == null) continue;

                var model = new Model.TourStructure.TourOrderCustomer
                    {
                        ID = Utils.GetXAttributeValue(t, "ID"),
                        OrderId = Utils.GetXAttributeValue(t, "OrderId"),
                        CompanyID = Utils.GetInt(Utils.GetXAttributeValue(t, "CompanyID")),
                        VisitorName = Utils.GetXAttributeValue(t, "VisitorName"),
                        CradType =
                            (Model.EnumType.TourStructure.CradType)Utils.GetInt(Utils.GetXAttributeValue(t, "CradType")),
                        CradNumber = Utils.GetXAttributeValue(t, "CradNumber"),
                        Sex = (Model.EnumType.CompanyStructure.Sex)Utils.GetInt(Utils.GetXAttributeValue(t, "Sex")),
                        VisitorType =
                            (Model.EnumType.TourStructure.VisitorType)
                            Utils.GetInt(Utils.GetXAttributeValue(t, "VisitorType")),
                        ContactTel = Utils.GetXAttributeValue(t, "ContactTel"),
                        IssueTime = Utils.GetDateTime(Utils.GetXAttributeValue(t, "IssueTime")),
                        CustomerStatus =
                            (Model.EnumType.TourStructure.CustomerStatus)
                            Utils.GetInt(Utils.GetXAttributeValue(t, "CustomerStatus"))
                    };


                list.Add(model);
            }

            return list;
        }

        #endregion
    }
}
