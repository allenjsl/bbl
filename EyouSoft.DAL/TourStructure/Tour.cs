/*Author:汪奇志 2011-01-24*/
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

namespace EyouSoft.DAL.TourStructure
{
    /// <summary>
    /// 计划中心数据访问类
    /// </summary>
    /// Author:汪奇志 2011-01-24
    public class Tour : EyouSoft.Toolkit.DAL.DALBase, EyouSoft.IDAL.TourStructure.ITour
    {
        #region constructor
        /// <summary>
        /// database
        /// </summary>
        private Database _db = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public Tour()
        {
            this._db = base.SystemStore;
        }
        #endregion

        #region static constants
        //static constants
        private const string DEFAULT_XML_DOC = "<ROOT></ROOT>";
        //private const string SQL_UPDATE_Delete = "UPDATE [tbl_Tour] SET [IsDelete]=@V WHERE TourId=@TourId;UPDATE [tbl_TourOrder] SET [IsDelete]=@V WHERE TourId=@TourId;UPDATE [tbl_StatAllIncome] SET  [IsDelete]=@V WHERE TourId=@TourId;UPDATE [tbl_StatAllOut] SET  [IsDelete]=@V WHERE TourId=@TourId;";
        private const string SQL_SELECT_GetTourInfo = "SELECT * FROM [tbl_Tour] WHERE TourId=@TourId";
        private const string SQL_SELECT_GetPlansSingle = "SELECT * FROM tbl_PlanSingle WHERE TourId=@TourId";
        private const string SQL_SELECT_GetSingleOrTeamOrderInfo = "SELECT [Id],[OrderNo],[BuyCompanyID],[BuyCompanyName],[ContactName],[ContactTel],[SalerId],[SalerName],[ContactMobile],[CustomerDisplayType],[CustomerFilePath] FROM [tbl_TourOrder] WHERE TourId=@TourId";
        private const string SQL_SELECT_GetSingleTravellers = "SELECT A.*,B.CustormerId,B.ProjectName,B.ServiceDetail,B.IsAdd,B.Fee,B.IssueTime AS BIssueTime FROM tbl_TourOrderCustomer AS A LEFT OUTER JOIN tbl_CustomerSpecialService AS B ON A.Id=B.CustormerId WHERE A.OrderId=@OrderId";
        private const string SQL_SELECT_GetTourQuickPrivateInfo = "SELECT * FROM [tbl_TourQuickPlan] WHERE TourId=@TourId";
        private const string SQL_SELECT_TourNormalPrivateInfo = "SELECT * FROM [tbl_TourService] WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetTourJourneys = "SELECT * FROM [tbl_TourPlan] WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetTourAttachs = "SELECT * FROM [tbl_TourAttach] WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetTourLocalAgencys = "SELECT * FROM [tbl_TourLocalAgency] WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetTourTeamOrSingleServices = "SELECT * FROM [tbl_TourTeamPrice] WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetCustomerInfo = "SELECT ContactName,Phone,Mobile,Fax  FROM [tbl_Customer] WHERE Id=@CID";
        private const string SQL_UPDATE_SetStatus = "UPDATE [tbl_Tour] SET Status=@Status,EndDateTime=(CASE @Status WHEN 5 THEN GETDATE() ELSE NULL END) WHERE [TourId]=@TourId;";
        private const string SQL_UPDATE_SetIsCostConfirm = "UPDATE [tbl_Tour] SET [IsCostConfirm]=@IsCostConfirm WHERE [TourId]=@TourId";
        private const string SQL_UPDATE_SetIsReview = "UPDATE [tbl_Tour] SET [IsReview]=@IsReview WHERE [TourId]=@TourId";
        private const string SQL_UPDATE_SetTourTicketStatus = "UPDATE [tbl_Tour] SET [TicketStatus]=@Status WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetTourRealityPeopleNumber = "SELECT SUM([PeopleNumber]-[LeaguePepoleNum]) AS Number FROM [tbl_TourOrder] WHERE TourId=@TourId AND IsDelete='0' AND OrderState NOT IN(3,4)";
        private const string SQL_UPDATE_SetTourVirtualPeopleNumbe = "UPDATE [tbl_Tour] SET VirtualPeopleNumber=CASE WHEN ([PlanPeopleNumber]-@Expression)<0 THEN [PlanPeopleNumber] ELSE @Expression END WHERE TourId=@TourId";
        private const string SQL_DELETE_DeleteTourAttach = "DELETE FROM [tbl_TourAttach] WHERE [AttachId]=@AttachId AND [TourId]=@TourId";
        private const string SQL_SELECT_GetPriceStandards = "SELECT A.*,B.[LevType],B.[CustomStandName] AS LevName,C.[PriceStandName] As StandardName FROM [tbl_TourPrice] AS A LEFT OUTER JOIN [tbl_CompanyCustomStand] AS B ON A.[LevelId]=B.[Id] LEFT OUTER JOIN [tbl_CompanyPriceStand] AS C ON A.[Standard]=C.[Id]  WHERE A.[TourId]=@TourId";
        private const string SQL_SELECT_GetIsReview = "SELECT [IsReview] FROM [tbl_Tour] WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetIsCostConfirm = "SELECT [IsCostConfirm] FROM [tbl_Tour] WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetIsExistsExpense = "SELECT COUNT(*) AS Expression FROM [tbl_StatAllOut] WHERE TourId=@TourId AND IsDelete='0' AND ItemType IN(1,2,4) ";
        private const string SQL_SELECT_GetTourExpense = "SELECT SUM([TotalAmount]) AS TotalAmount,SUM([HasCheckAmount]) AS PaidAmount FROM [tbl_StatAllOut] WHERE TourId=@TourId AND IsDelete='0'";
        private const string SQL_SELECT_GetTourCoordinators = "SELECT B.[ContactName],B.[ContactTel],A.[OperatorId],B.[ContactMobile] FROM [tbl_TourOperator] AS A INNER JOIN [tbl_CompanyUser] AS B ON A.[OperatorId]=B.[Id] WHERE A.[TourId]=@TourId";
        private const string SQL_SELECT_GetSingleOutRegAmount = "SELECT SUM([PaymentAmount]) AS Amount FROM [tbl_FinancialPayInfo] WHERE [ReceiveId]=@PlanId AND [ReceiveType]=0";
        private const string SQL_SELECT_GetSingleSuppliers = "SELECT SupplierId FROM [tbl_PlanSingle] WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetTourSuppliers = "SELECT [TicketOfficeId] AS SupplierId FROM [tbl_PlanTicketOut] WHERE [TourId]=@TourId UNION SELECT [TravelAgencyID] AS SupplierId FROM [tbl_PlanLocalAgency] WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetTourCustoemrs = "SELECT [BuyCompanyID] FROM [tbl_TourOrder] WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetTourPeopleNumberShiShou = " SELECT SUM(PeopleNumber-LeaguePepoleNum) AS ShiShou  FROM [tbl_TourOrder] WHERE [TourId]=@TourId AND [OrderState] IN(1,2,5) AND [IsDelete]='0' ";
        private const string SQL_SELECT_GetTourSentPeoples = "SELECT A.[OperatorId],B.[ContactName] FROM [tbl_TourSentTask] AS A INNER JOIN [tbl_CompanyUser] AS B ON A.[OperatorId]=B.[Id] WHERE A.[TourId]=@TourId";
        private const string SQL_SELECT_GetTourGuides = "SELECT [Guide] FROM [tbl_TourSendGuide] WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetTourCitys = "SELECT [CityId] FROM [tbl_TourCity] WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetRouteSTSCollectInfo = "SELECT SUM(A.[TotalIncome]) AS IncomeAmount,SUM(A.[TotalExpenses]) AS OutAmount,(SELECT SUM(B.[PeopleNumber]-B.[LeaguePepoleNum]) FROM [tbl_TourOrder] AS B WHERE B.TourId IN(SELECT C.[TourId] FROM [tbl_Tour] AS C WHERE C.[CompanyId]=@CompanyId AND C.[RouteId]=@RouteId AND C.[IsDelete]='0') AND B.[IsDelete]='0' AND B.[OrderState] NOT IN(3,4)) AS ShiShou FROM [tbl_Tour] AS A WHERE A.[CompanyId]=@CompanyId AND A.[RouteId]=@RouteId AND A.[IsDelete]='0'";
        private const string SQL_SELECT_GetRouteSKSCollectInfo = "SELECT SUM([AdultNumber]) AS AdultNumber,SUM([ChildNumber]) AS ChildrenNumber FROM [tbl_TourOrder] WHERE [IsDelete]='0' AND [RouteId]=@RouteId AND [OrderState] IN(1,2,5)";
        private const string SQL_SELECT_GetSinglePlanInfo = "SELECT * FROM tbl_PlanSingle WHERE PlanId=@PlanId";
        private const string SQL_SELECT_GetTourType = "SELECT [TourType] FROM [tbl_Tour] WHERE [TourId]=@TourId";
        private const string SQL_SELECT_GetTourRemindTravelAgencyInfo = "SELECT A.[OrderState],B.[Id],B.[Name],B.[ContactName],B.[Phone],B.[Mobile],B.[Fax] FROM [tbl_TourOrder] AS A INNER JOIN [tbl_Customer] AS B ON A.[BuyCompanyId]=B.Id WHERE A.[TourId]=@TourId AND A.[IsDelete]='0' AND A.[OrderState] IN(2,5)";
        private const string SQL_SELECT_GetTourTeamUnit = @" SELECT [NumberCR],[NumberET],[NumberQP],[UnitAmountCR],[UnitAmountET],[UnitAmountQP] FROM [tbl_TourTeamUnit] where TourId = @TourId ";
        const string SQL_SELECT_GetShengYuRenShu = "SELECT PlanPeopleNumber,(SELECT SUM(PeopleNumber-LeaguePepoleNum) FROM tbl_TourOrder WHERE IsDelete='0' AND OrderState NOT IN(3,4) AND TourId=@TourId) FROM tbl_Tour WHERE TourId=@TourId";
        #endregion

        #region private members
        #region create xml
        /// <summary>
        /// 创建团队地接社信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateLocalAgencysXML(IList<EyouSoft.Model.TourStructure.TourLocalAgencyInfo> items)
        {
            //XML:<ROOT><Info AgencyId="INT" Name="" LicenseNo="" Telephone="" ContacterName="" /></ROOT>
            if (items == null || items.Count < 1) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                xmlDoc.AppendFormat("<Info AgencyId=\"{0}\" Name=\"{1}\" LicenseNo=\"{2}\" Telephone=\"{3}\" ContacterName=\"{4}\" />", item.AgencyId
                    , Utils.ReplaceXmlSpecialCharacter(item.Name)
                    , Utils.ReplaceXmlSpecialCharacter(item.LicenseNo)
                    , Utils.ReplaceXmlSpecialCharacter(item.Telephone)
                    , Utils.ReplaceXmlSpecialCharacter(item.ContacterName));
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建团队附件信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateAttachsXML(IList<EyouSoft.Model.TourStructure.TourAttachInfo> items)
        {
            //<ROOT><Info FilePath="" Name="" /></ROOT>
            if (items == null || items.Count < 1) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                xmlDoc.AppendFormat("<Info FilePath=\"{0}\" Name=\"{1}\" />", item.FilePath, item.Name);
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }
        /// <summary>
        /// 创建标准发布计划行程信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreatePlansXML(IList<EyouSoft.Model.TourStructure.TourPlanInfo> items)
        {
            //XML:<ROOT><Info Interval="区间" Vehicle="交通工具" Hotel="住宿" Dinner="用餐" Plan="行程内容" FilePath="附件路径" /></ROOT>
            if (items == null || items.Count < 1) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                xmlDoc.AppendFormat("<Info Interval=\"{0}\" Vehicle=\"{1}\" Hotel=\"{2}\" Dinner=\"{3}\" Plan=\"{4}\" FilePath=\"{5}\" />", Utils.ReplaceXmlSpecialCharacter(item.Interval)
                    , Utils.ReplaceXmlSpecialCharacter(item.Vehicle)
                    , Utils.ReplaceXmlSpecialCharacter(item.Hotel)
                    , Utils.ReplaceXmlSpecialCharacter(item.Dinner)
                    , Utils.ReplaceXmlSpecialCharacter(item.Plan)
                    , item.FilePath);
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建快速发布计划专有信息XMLDocument
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private string CreateQuickPrivateXML(EyouSoft.Model.TourStructure.TourQuickPrivateInfo info)
        {
            //XML:<ROOT><Info Plan="团队行程" Service="服务标准" Remark="备注" /></ROOT>
            if (info == null) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            xmlDoc.AppendFormat("<Info Plan=\"{0}\" Service=\"{1}\" Remark=\"{2}\" />", Utils.ReplaceXmlSpecialCharacter(info.QuickPlan)
                , Utils.ReplaceXmlSpecialCharacter(info.Service)
                , Utils.ReplaceXmlSpecialCharacter(info.Remark));
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建包含项目信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateServiceXML(IList<EyouSoft.Model.TourStructure.TourServiceInfo> items)
        {
            //XML:<ROOT><Info Type="INT项目类型" Service="接待标准或单项服务具体要求" LocalPrice="MONEY地接报价" SelfPrice="MONEY我社报价" Remark="备注" /></ROOT>
            if (items == null || items.Count < 1) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                xmlDoc.AppendFormat("<Info Type=\"{0}\" Service=\"{1}\" LocalPrice=\"{2}\" SelfPrice=\"{3}\" Remark=\"{4}\" />", (int)item.ServiceType
                   , Utils.ReplaceXmlSpecialCharacter(item.Service)
                   , 0
                   , 0
                   , "");
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建包含项目信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateServiceXML(IList<EyouSoft.Model.TourStructure.TourTeamServiceInfo> items)
        {
            //XML:<ROOT><Info Type="INT项目类型" Service="接待标准或单项服务具体要求" LocalPrice="MONEY地接报价" SelfPrice="MONEY我社报价" Remark="备注" LocalPeopleNumber="地接报价人数" LocalUnitPrice="地接报价单价" SelfPeopleNumber="我社报价人数" SelfUnitPrice="我社报价单价" /></ROOT>
            if (items == null || items.Count < 1) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                xmlDoc.AppendFormat("<Info Type=\"{0}\" Service=\"{1}\" LocalPrice=\"{2}\" SelfPrice=\"{3}\" Remark=\"{4}\" LocalPeopleNumber=\"{5}\" LocalUnitPrice=\"{6}\" SelfPeopleNumber=\"{7}\" SelfUnitPrice=\"{8}\" />", (int)item.ServiceType
                   , Utils.ReplaceXmlSpecialCharacter(item.Service)
                   , item.LocalPrice
                   , item.SelfPrice
                   , ""
                   , item.LocalPeopleNumber
                   , item.LocalUnitPrice
                   , item.SelfPeopleNumber
                   , item.SelfUnitPrice);
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建包含项目信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateServiceXML(IList<EyouSoft.Model.TourStructure.TourSingleServiceInfo> items)
        {
            //XML:<ROOT><Info Type="INT项目类型" Service="接待标准或单项服务具体要求" LocalPrice="MONEY地接报价" SelfPrice="MONEY我社报价" Remark="备注" /></ROOT>
            if (items == null || items.Count < 1) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                xmlDoc.AppendFormat("<Info Type=\"{0}\" Service=\"{1}\" LocalPrice=\"{2}\" SelfPrice=\"{3}\" Remark=\"{4}\" />", (int)item.ServiceType
                   , Utils.ReplaceXmlSpecialCharacter(item.Requirement)
                   , 0
                   , item.SelfPrice
                   , Utils.ReplaceXmlSpecialCharacter(item.Remark));
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建标准发布专有信息(不含项目等)XMLDocument
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private string CreateNormalPrivateXML(EyouSoft.Model.TourStructure.TourTeamNormalPrivateInfo info)
        {
            //XML:<ROOT><Info Key="键" Value="值" /></ROOT>
            if (info == null) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "BuHanXiangMu", Utils.ReplaceXmlSpecialCharacter(info.BuHanXiangMu));
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "ErTongAnPai", Utils.ReplaceXmlSpecialCharacter(info.ErTongAnPai));
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "GouWuAnPai", Utils.ReplaceXmlSpecialCharacter(info.GouWuAnPai));
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "NeiBuXingXi", Utils.ReplaceXmlSpecialCharacter(info.NeiBuXingXi));
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "WenXinTiXing", Utils.ReplaceXmlSpecialCharacter(info.WenXinTiXing));
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "ZhuYiShiXiang", Utils.ReplaceXmlSpecialCharacter(info.ZhuYiShiXiang));
            xmlDoc.AppendFormat("<Info Key=\"{0}\" Value=\"{1}\" />", "ZiFeiXIangMu", Utils.ReplaceXmlSpecialCharacter(info.ZiFeiXIangMu));
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建散拼计划子团信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateChildrensXML(IList<EyouSoft.Model.TourStructure.TourChildrenInfo> items)
        {
            //XML:<ROOT><Info TourId="团队编号" LDate="出团日期" TourCode="团号"  /></ROOT>
            if (items == null || items.Count < 1) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item.ChildrenId)) item.ChildrenId = Guid.NewGuid().ToString();
                xmlDoc.AppendFormat("<Info TourId=\"{0}\" LDate=\"{1}\" TourCode=\"{2}\"  />", item.ChildrenId
                   , item.LDate
                   , Utils.ReplaceXmlSpecialCharacter(item.TourCode));
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建散拼计划报价信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateTourPriceXML(IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> items)
        {
            //XML:<ROOT><Info AdultPrice="MONEY成人价" ChildrenPrice="MONEY儿童价" StandId="报价标准编号" LevelId="客户等级编号" /></ROOT>
            if (items == null || items.Count < 1) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                foreach (var tmp in item.CustomerLevels)
                {
                    xmlDoc.AppendFormat("<Info AdultPrice=\"{0}\" ChildrenPrice=\"{1}\" StandardId=\"{2}\" LevelId=\"{3}\" />", tmp.AdultPrice
                        , tmp.ChildrenPrice
                        , item.StandardId
                        , tmp.LevelId);
                }
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建散拼计划子团建团规则XMLDocument
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private string CreateChildrenCreateRuleXML(EyouSoft.Model.TourStructure.TourCreateRuleInfo info)
        {
            //XML:<ROOT><Info Type="规则类型" SDate="起始日期" EDate="结束日期" Cycle="周期" /></ROOT>
            if (info == null) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            xmlDoc.AppendFormat("<Info Type=\"{0}\" SDate=\"{1}\" EDate=\"{2}\" Cycle=\"{3}\" />", (int)info.Rule
                , info.SDate
                , info.EDate
                , info.Cycle);
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建团队计划专有信息XMLDocument
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private string CreateTeamPrivateXML(EyouSoft.Model.TourStructure.TourTeamInfo info)
        {
            //团队计划专有信息 XML:<ROOT><Info OrderId="订单编号" SellerId="销售员编号" SellerName="销售员姓名" TotalAmount="合计金额" BuyerCId="客户单位编号" BuyerCName="客户单位名称" ContactName="联系人" ContactTelephone="联系电话" ContactMobile="联系手机"  QuoteId="报价编号" SelfUnitPriceAmount="我社报价单价合计金额" /></ROOT>
            if (info == null) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            xmlDoc.AppendFormat("<Info OrderId=\"{0}\" SellerId=\"{1}\" SellerName=\"{2}\" TotalAmount=\"{3}\" BuyerCId=\"{4}\" BuyerCName=\"{5}\" ContactName=\"{6}\" ContactTelephone=\"{7}\" ContactMobile=\"{8}\" QuoteId=\"{9}\" />"
                , info.OrderId
                , info.SellerId
                , ""
                , info.TotalAmount
                , info.BuyerCId
                , Utils.ReplaceXmlSpecialCharacter(info.BuyerCName)
                , ""
                , ""
                , ""
                , info.QuoteId);
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建单项服务专有信息XMLDocument
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private string CreateSinglePrivateXML(EyouSoft.Model.TourStructure.TourSingleInfo info)
        {
            //XML:<ROOT><Info OrderId="订单编号" SellerId="销售员编号" SellerName="销售员姓名" TotalAmount="合计金额" GrossProfit="团队毛利" BuyerCId="客户单位编号" BuyerCName="客户单位名称" ContactName="联系人" ContactTelephone="联系电话" ContactMobile="联系电话" TotalOutAmount="合计支出金额" /></ROOT>
            if (info == null) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            xmlDoc.AppendFormat("<Info OrderId=\"{0}\" SellerId=\"{1}\" SellerName=\"{2}\" TotalAmount=\"{3}\"  GrossProfit=\"{4}\" BuyerCId=\"{5}\" BuyerCName=\"{6}\" ContactName=\"{7}\" ContactTelephone=\"{8}\" ContactMobile=\"{9}\" TotalOutAmount=\"{10}\" />", info.OrderId
                , info.SellerId
                , Utils.ReplaceXmlSpecialCharacter(info.SellerName)
                , info.TotalAmount
                , info.GrossProfit
                , info.BuyerCId
                , Utils.ReplaceXmlSpecialCharacter(info.BuyerCName)
                , Utils.ReplaceXmlSpecialCharacter(info.ContacterName)
                , Utils.ReplaceXmlSpecialCharacter(info.ContacterTelephone)
                , Utils.ReplaceXmlSpecialCharacter(info.ContacterMobile)
                , info.TotalOutAmount);
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建订单游客信息XMLDocument
        /// </summary>
        /// <returns></returns>
        private string CreateTourOrderCustomersXML(IList<EyouSoft.Model.TourStructure.TourOrderCustomer> items)
        {
            //XML:<ROOT><Info TravelerId="游客编号" TravelerName="游客姓名" IdType="证件类型" IdNo="证件号" Gender="性别" TravelerType="类型(成人/儿童)" ContactTel="联系电话" TF_XiangMu="特服项目" TF_NeiRong="特服内容" TF_ZengJian="1:增加 0:减少" TF_FeiYong="特服费用" /></ROOT>	
            if (items == null || items.Count < 1) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item.ID)) item.ID = Guid.NewGuid().ToString();

                item.SpecialServiceInfo = item.SpecialServiceInfo ?? new EyouSoft.Model.TourStructure.CustomerSpecialService();

                xmlDoc.AppendFormat("<Info TravelerId=\"{0}\" TravelerName=\"{1}\" IdType=\"{2}\" IdNo=\"{3}\" Gender=\"{4}\" TravelerType=\"{5}\" ContactTel=\"{6}\" TF_XiangMu=\"{7}\" TF_NeiRong=\"{8}\" TF_ZengJian=\"{9}\" TF_FeiYong=\"{10}\" />", item.ID
                    , Utils.ReplaceXmlSpecialCharacter(item.VisitorName)
                    , (int)item.CradType
                    , Utils.ReplaceXmlSpecialCharacter(item.CradNumber)
                    , (int)item.Sex
                    , (int)item.VisitorType
                    , Utils.ReplaceXmlSpecialCharacter(item.ContactTel)
                    , Utils.ReplaceXmlSpecialCharacter(item.SpecialServiceInfo.ProjectName)
                    , Utils.ReplaceXmlSpecialCharacter(item.SpecialServiceInfo.ServiceDetail)
                    , item.SpecialServiceInfo.IsAdd ? 1 : 0
                    , item.SpecialServiceInfo.Fee);
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建单项服务供应商安排信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreatePlansSingleXML(IList<EyouSoft.Model.TourStructure.PlanSingleInfo> items)
        {
            //@XML:<ROOT><Info PlanId="安排编号" ServiceType="项目类型" SupplierId="供应商编号" SupplierName="供应商名称" Arrange="具体安排" Amount="结算费用" Remark="备注" /></ROOT>
            if (items == null || items.Count < 1) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                if (string.IsNullOrEmpty(item.PlanId)) item.PlanId = Guid.NewGuid().ToString();

                xmlDoc.AppendFormat("<Info PlanId=\"{0}\" ServiceType=\"{1}\" SupplierId=\"{2}\" SupplierName=\"{3}\" Arrange=\"{4}\" Amount=\"{5}\" Remark=\"{6}\" />", item.PlanId
                    , (int)item.ServiceType
                    , item.SupplierId
                    , Utils.ReplaceXmlSpecialCharacter(item.SupplierName)
                    , Utils.ReplaceXmlSpecialCharacter(item.Arrange)
                    , item.Amount
                    , Utils.ReplaceXmlSpecialCharacter(item.Remark));
            }
            xmlDoc.Append("</ROOT>");

            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建生成团号XMLDocument
        /// </summary>
        /// <param name="ldates"></param>
        /// <returns></returns>
        private string CreateAutoTourCodesXML(params DateTime[] ldates)
        {
            //XML:<ROOT><Info Date="出团日期" SDate="出团日期字符串(出团日期为2008-08-08字符串格式为20080808)" /></ROOT>
            if (ldates == null || ldates.Length < 1) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var date in ldates)
            {
                xmlDoc.AppendFormat("<Info Date=\"{0}\" SDate=\"{1}\" />", date.ToString(), date.ToString("yyyyMMdd"));
            }
            xmlDoc.Append("</ROOT>");
            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建送团人信息XMLDocument
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private string CreateSentPeoplesXML(IList<EyouSoft.Model.TourStructure.TourSentPeopleInfo> items)
        {
            //XML:<ROOT><Info PID="送团人编号" /></ROOT>
            if (items == null || items.Count < 1) return DEFAULT_XML_DOC;

            StringBuilder xmlDoc = new StringBuilder();
            xmlDoc.Append("<ROOT>");
            foreach (var item in items)
            {
                xmlDoc.AppendFormat("<Info UID=\"{0}\" />", item.OperatorId);
            }
            xmlDoc.Append("</ROOT>");
            return xmlDoc.ToString();
        }

        /// <summary>
        /// 创建团队计划人数及单价信息XMLDocument
        /// </summary>
        /// <param name="info">团队计划人数及单价信息实体</param>
        /// <returns></returns>
        private string CreateTourTeamUnitXml(Model.TourStructure.MTourTeamUnitInfo info)
        {
            if (info == null || info.NumberCr <= 0)
                return DEFAULT_XML_DOC;

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

        /// <summary>
        /// 生成团队交通SqlXml
        /// </summary>
        /// <param name="list">团队交通集合</param>
        /// <returns>团队交通SqlXml</returns>
        private string CreateTourTraffic(IList<int> list)
        {
            if (list == null || !list.Any()) return DEFAULT_XML_DOC;
            var xml = new StringBuilder();
            xml.Append("<ROOT>");
            foreach (var i in list)
            {
                xml.AppendFormat("<Info TrafficId=\"{0}\" ", i);
                xml.Append(" />");
            }
            xml.Append("</ROOT>");
            return xml.ToString();
        }

        #endregion

        /// <summary>
        /// 写入散拼计划，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">散拼计划信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        private int InsertTourInfo(EyouSoft.Model.TourStructure.TourInfo info)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Tour_InsertTourInfo");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, info.TourId);
            this._db.AddInParameter(cmd, "TourCode", DbType.String, "");
            this._db.AddInParameter(cmd, "TourType", DbType.Byte, info.TourType);
            this._db.AddInParameter(cmd, "Status", DbType.Byte, info.Status);
            this._db.AddInParameter(cmd, "TourDays", DbType.Int32, info.TourDays);
            this._db.AddInParameter(cmd, "LDate", DbType.DateTime, DateTime.Now);
            this._db.AddInParameter(cmd, "AreaId", DbType.Int32, info.AreaId);
            this._db.AddInParameter(cmd, "RouteName", DbType.String, info.RouteName);
            this._db.AddInParameter(cmd, "PlanPeopleNumber", DbType.Int32, info.PlanPeopleNumber);
            this._db.AddInParameter(cmd, "TicketStatus", DbType.Byte, info.TicketStatus);
            this._db.AddInParameter(cmd, "ReleaseType", DbType.AnsiStringFixedLength, (int)info.ReleaseType);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, info.OperatorId);
            this._db.AddInParameter(cmd, "CreateTime", DbType.DateTime, DateTime.Now);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, info.CompanyId);
            this._db.AddInParameter(cmd, "LTraffic", DbType.String, info.LTraffic);
            this._db.AddInParameter(cmd, "RTraffic", DbType.String, info.RTraffic);
            this._db.AddInParameter(cmd, "Gather", DbType.String, info.Gather);
            this._db.AddInParameter(cmd, "RouteId", DbType.Int32, info.RouteId);
            this._db.AddInParameter(cmd, "LocalAgencys", DbType.String, this.CreateLocalAgencysXML(info.LocalAgencys));
            this._db.AddInParameter(cmd, "Attachs", DbType.String, this.CreateAttachsXML(info.Attachs));

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                this._db.AddInParameter(cmd, "Plans", DbType.String, this.CreatePlansXML(info.TourNormalInfo.Plans));
            else
                this._db.AddInParameter(cmd, "Plans", DbType.String, DBNull.Value);

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick)
                this._db.AddInParameter(cmd, "QuickPrivate", DbType.String, this.CreateQuickPrivateXML(info.TourQuickInfo));
            else
                this._db.AddInParameter(cmd, "QuickPrivate", DbType.String, DBNull.Value);

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                this._db.AddInParameter(cmd, "Service", DbType.String, this.CreateServiceXML(info.TourNormalInfo.Services));
            else
                this._db.AddInParameter(cmd, "Service", DbType.String, DBNull.Value);

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                this._db.AddInParameter(cmd, "NormalPrivate", DbType.String, this.CreateNormalPrivateXML(info.TourNormalInfo));
            else
                this._db.AddInParameter(cmd, "NormalPrivate", DbType.String, DBNull.Value);

            this._db.AddInParameter(cmd, "Childrens", DbType.String, this.CreateChildrensXML(info.Childrens));
            this._db.AddInParameter(cmd, "TourPrice", DbType.String, this.CreateTourPriceXML(info.PriceStandards));
            this._db.AddInParameter(cmd, "ChildrenCreateRule", DbType.String, this.CreateChildrenCreateRuleXML(info.CreateRule));
            this._db.AddInParameter(cmd, "SinglePrivate", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "PlansSingle", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "TeamPrivate", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "CustomerDisplayType", DbType.Byte, 0);
            this._db.AddInParameter(cmd, "Customers", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "CustomerFilePath", DbType.String, DBNull.Value);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            this._db.AddInParameter(cmd, "CoordinatorId", DbType.Int32, info.Coordinator.CoordinatorId);
            this._db.AddInParameter(cmd, "GatheringTime", DbType.String, info.GatheringTime);
            this._db.AddInParameter(cmd, "GatheringPlace", DbType.String, info.GatheringPlace);
            this._db.AddInParameter(cmd, "GatheringSign", DbType.String, info.GatheringSign);
            this._db.AddInParameter(cmd, "SentPeoples", DbType.String, this.CreateSentPeoplesXML(info.SentPeoples));
            this._db.AddInParameter(cmd, "TourCityId", DbType.Int32, info.TourCityId);
            this._db.AddInParameter(cmd, "TourTraffic", DbType.String, this.CreateTourTraffic(info.TourTraffic));

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

        /// <summary>
        /// 写入团队计划，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">团队计划信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        private int InsertTeamTourInfo(EyouSoft.Model.TourStructure.TourTeamInfo info)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Tour_InsertTourInfo");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, info.TourId);
            this._db.AddInParameter(cmd, "TourCode", DbType.String, info.TourCode);
            this._db.AddInParameter(cmd, "TourType", DbType.Byte, info.TourType);
            this._db.AddInParameter(cmd, "Status", DbType.Byte, info.Status);
            this._db.AddInParameter(cmd, "TourDays", DbType.Int32, info.TourDays);
            this._db.AddInParameter(cmd, "LDate", DbType.DateTime, info.LDate);
            this._db.AddInParameter(cmd, "AreaId", DbType.Int32, info.AreaId);
            this._db.AddInParameter(cmd, "RouteName", DbType.String, info.RouteName);
            this._db.AddInParameter(cmd, "PlanPeopleNumber", DbType.Int32, info.PlanPeopleNumber);
            this._db.AddInParameter(cmd, "TicketStatus", DbType.Byte, info.TicketStatus);
            this._db.AddInParameter(cmd, "ReleaseType", DbType.AnsiStringFixedLength, (int)info.ReleaseType);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, info.OperatorId);
            this._db.AddInParameter(cmd, "CreateTime", DbType.DateTime, DateTime.Now);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, info.CompanyId);
            this._db.AddInParameter(cmd, "LTraffic", DbType.String, info.LTraffic);
            this._db.AddInParameter(cmd, "RTraffic", DbType.String, info.RTraffic);
            this._db.AddInParameter(cmd, "Gather", DbType.String, info.Gather);
            this._db.AddInParameter(cmd, "RouteId", DbType.Int32, info.RouteId);
            this._db.AddInParameter(cmd, "LocalAgencys", DbType.String, this.CreateLocalAgencysXML(info.LocalAgencys));
            this._db.AddInParameter(cmd, "Attachs", DbType.String, this.CreateAttachsXML(info.Attachs));

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                this._db.AddInParameter(cmd, "Plans", DbType.String, this.CreatePlansXML(info.TourNormalInfo.Plans));
            else
                this._db.AddInParameter(cmd, "Plans", DbType.String, DBNull.Value);

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick)
                this._db.AddInParameter(cmd, "QuickPrivate", DbType.String, this.CreateQuickPrivateXML(info.TourQuickInfo));
            else
                this._db.AddInParameter(cmd, "QuickPrivate", DbType.String, DBNull.Value);

            this._db.AddInParameter(cmd, "Service", DbType.String, this.CreateServiceXML(info.Services));

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                this._db.AddInParameter(cmd, "NormalPrivate", DbType.String, this.CreateNormalPrivateXML(info.TourNormalInfo));
            else
                this._db.AddInParameter(cmd, "NormalPrivate", DbType.String, DBNull.Value);

            this._db.AddInParameter(cmd, "Childrens", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "TourPrice", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "ChildrenCreateRule", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "SinglePrivate", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "PlansSingle", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "TeamPrivate", DbType.String, this.CreateTeamPrivateXML(info));
            this._db.AddInParameter(cmd, "CustomerDisplayType", DbType.Byte, 0);
            this._db.AddInParameter(cmd, "Customers", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "CustomerFilePath", DbType.String, DBNull.Value);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            this._db.AddInParameter(cmd, "CoordinatorId", DbType.Int32, info.Coordinator.CoordinatorId);
            this._db.AddInParameter(cmd, "GatheringTime", DbType.String, info.GatheringTime);
            this._db.AddInParameter(cmd, "GatheringPlace", DbType.String, info.GatheringPlace);
            this._db.AddInParameter(cmd, "GatheringSign", DbType.String, info.GatheringSign);
            this._db.AddInParameter(cmd, "SentPeoples", DbType.String, this.CreateSentPeoplesXML(info.SentPeoples));
            this._db.AddInParameter(cmd, "TourCityId", DbType.Int32, info.TourCityId);
            _db.AddInParameter(cmd, "TourTeamUnit", DbType.String, CreateTourTeamUnitXml(info.TourTeamUnit));
            this._db.AddInParameter(cmd, "TourTraffic", DbType.String, this.CreateTourTraffic(info.TourTraffic));

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

        /// <summary>
        /// 写入单项服务，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">单项服务信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        private int InsertSingleTourInfo(EyouSoft.Model.TourStructure.TourSingleInfo info)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Tour_InsertTourInfo");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, info.TourId);
            this._db.AddInParameter(cmd, "TourCode", DbType.String, info.TourCode);
            this._db.AddInParameter(cmd, "TourType", DbType.Byte, info.TourType);
            this._db.AddInParameter(cmd, "Status", DbType.Byte, info.Status);
            this._db.AddInParameter(cmd, "TourDays", DbType.Int32, info.TourDays);
            this._db.AddInParameter(cmd, "LDate", DbType.DateTime, info.LDate);
            this._db.AddInParameter(cmd, "AreaId", DbType.Int32, info.AreaId);
            this._db.AddInParameter(cmd, "RouteName", DbType.String, info.RouteName);
            this._db.AddInParameter(cmd, "PlanPeopleNumber", DbType.Int32, info.PlanPeopleNumber);
            this._db.AddInParameter(cmd, "TicketStatus", DbType.Byte, info.TicketStatus);
            this._db.AddInParameter(cmd, "ReleaseType", DbType.AnsiStringFixedLength, (int)info.ReleaseType);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, info.OperatorId);
            this._db.AddInParameter(cmd, "CreateTime", DbType.DateTime, DateTime.Now);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, info.CompanyId);
            this._db.AddInParameter(cmd, "LTraffic", DbType.String, info.LTraffic);
            this._db.AddInParameter(cmd, "RTraffic", DbType.String, info.RTraffic);
            this._db.AddInParameter(cmd, "Gather", DbType.String, info.Gather);
            this._db.AddInParameter(cmd, "RouteId", DbType.Int32, info.RouteId);
            this._db.AddInParameter(cmd, "LocalAgencys", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "Attachs", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "Plans", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "QuickPrivate", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "Service", DbType.String, this.CreateServiceXML(info.Services));
            this._db.AddInParameter(cmd, "NormalPrivate", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "Childrens", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "TourPrice", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "ChildrenCreateRule", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "SinglePrivate", DbType.String, this.CreateSinglePrivateXML(info));
            this._db.AddInParameter(cmd, "PlansSingle", DbType.String, this.CreatePlansSingleXML(info.Plans));
            this._db.AddInParameter(cmd, "TeamPrivate", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "CustomerDisplayType", DbType.Byte, info.CustomerDisplayType);
            this._db.AddInParameter(cmd, "Customers", DbType.String, this.CreateTourOrderCustomersXML(info.Customers));
            this._db.AddInParameter(cmd, "CustomerFilePath", DbType.String, info.CustomerFilePath);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            this._db.AddInParameter(cmd, "CoordinatorId", DbType.Int32, DBNull.Value);
            this._db.AddInParameter(cmd, "GatheringTime", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "GatheringPlace", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "GatheringSign", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "SentPeoples", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "TourCityId", DbType.Int32, DBNull.Value);

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

        /// <summary>
        /// 修改散拼计划，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">散拼计划信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        private int UpdateTourInfo(EyouSoft.Model.TourStructure.TourInfo info)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Tour_UpdateTourInfo");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, info.TourId);
            this._db.AddInParameter(cmd, "TourCode", DbType.String, info.TourCode);
            this._db.AddInParameter(cmd, "TourType", DbType.Byte, info.TourType);
            this._db.AddInParameter(cmd, "TourDays", DbType.Int32, info.TourDays);
            this._db.AddInParameter(cmd, "LDate", DbType.DateTime, info.LDate);
            this._db.AddInParameter(cmd, "AreaId", DbType.Int32, info.AreaId);
            this._db.AddInParameter(cmd, "RouteName", DbType.String, info.RouteName);
            this._db.AddInParameter(cmd, "PlanPeopleNumber", DbType.Int32, info.PlanPeopleNumber);
            this._db.AddInParameter(cmd, "ReleaseType", DbType.AnsiStringFixedLength, (int)info.ReleaseType);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, info.OperatorId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, info.CompanyId);
            this._db.AddInParameter(cmd, "LTraffic", DbType.String, info.LTraffic);
            this._db.AddInParameter(cmd, "RTraffic", DbType.String, info.RTraffic);
            this._db.AddInParameter(cmd, "Gather", DbType.String, info.Gather);
            this._db.AddInParameter(cmd, "RouteId", DbType.Int32, info.RouteId);
            this._db.AddInParameter(cmd, "LocalAgencys", DbType.String, this.CreateLocalAgencysXML(info.LocalAgencys));
            this._db.AddInParameter(cmd, "Attachs", DbType.String, this.CreateAttachsXML(info.Attachs));

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                this._db.AddInParameter(cmd, "Plans", DbType.String, this.CreatePlansXML(info.TourNormalInfo.Plans));
            else
                this._db.AddInParameter(cmd, "Plans", DbType.String, DBNull.Value);

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick)
                this._db.AddInParameter(cmd, "QuickPrivate", DbType.String, this.CreateQuickPrivateXML(info.TourQuickInfo));
            else
                this._db.AddInParameter(cmd, "QuickPrivate", DbType.String, DBNull.Value);

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                this._db.AddInParameter(cmd, "Service", DbType.String, this.CreateServiceXML(info.TourNormalInfo.Services));
            else
                this._db.AddInParameter(cmd, "Service", DbType.String, DBNull.Value);

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                this._db.AddInParameter(cmd, "NormalPrivate", DbType.String, this.CreateNormalPrivateXML(info.TourNormalInfo));
            else
                this._db.AddInParameter(cmd, "NormalPrivate", DbType.String, DBNull.Value);

            this._db.AddInParameter(cmd, "TourPrice", DbType.String, this.CreateTourPriceXML(info.PriceStandards));
            this._db.AddInParameter(cmd, "SinglePrivate", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "PlansSingle", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "TeamPrivate", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "CustomerDisplayType", DbType.Byte, 0);
            this._db.AddInParameter(cmd, "Customers", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "CustomerFilePath", DbType.String, DBNull.Value);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            this._db.AddOutParameter(cmd, "OrderId", DbType.AnsiStringFixedLength, 36);
            this._db.AddInParameter(cmd, "CoordinatorId", DbType.Int32, info.Coordinator.CoordinatorId);
            this._db.AddInParameter(cmd, "GatheringTime", DbType.String, info.GatheringTime);
            this._db.AddInParameter(cmd, "GatheringPlace", DbType.String, info.GatheringPlace);
            this._db.AddInParameter(cmd, "GatheringSign", DbType.String, info.GatheringSign);
            this._db.AddInParameter(cmd, "SentPeoples", DbType.String, this.CreateSentPeoplesXML(info.SentPeoples));
            this._db.AddInParameter(cmd, "TourCityId", DbType.Int32, info.TourCityId);
            this._db.AddInParameter(cmd, "TourTraffic", DbType.String, this.CreateTourTraffic(info.TourTraffic));

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

        /// <summary>
        /// 修改团队计划，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">团队计划信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        private int UpdateTeamTourInfo(EyouSoft.Model.TourStructure.TourTeamInfo info)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Tour_UpdateTourInfo");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, info.TourId);
            this._db.AddInParameter(cmd, "TourCode", DbType.String, info.TourCode);
            this._db.AddInParameter(cmd, "TourType", DbType.Byte, info.TourType);
            this._db.AddInParameter(cmd, "TourDays", DbType.Int32, info.TourDays);
            this._db.AddInParameter(cmd, "LDate", DbType.DateTime, info.LDate);
            this._db.AddInParameter(cmd, "AreaId", DbType.Int32, info.AreaId);
            this._db.AddInParameter(cmd, "RouteName", DbType.String, info.RouteName);
            this._db.AddInParameter(cmd, "PlanPeopleNumber", DbType.Int32, info.PlanPeopleNumber);
            this._db.AddInParameter(cmd, "ReleaseType", DbType.AnsiStringFixedLength, (int)info.ReleaseType);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, info.OperatorId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, info.CompanyId);
            this._db.AddInParameter(cmd, "LTraffic", DbType.String, info.LTraffic);
            this._db.AddInParameter(cmd, "RTraffic", DbType.String, info.RTraffic);
            this._db.AddInParameter(cmd, "Gather", DbType.String, info.Gather);
            this._db.AddInParameter(cmd, "RouteId", DbType.Int32, info.RouteId);
            this._db.AddInParameter(cmd, "LocalAgencys", DbType.String, this.CreateLocalAgencysXML(info.LocalAgencys));
            this._db.AddInParameter(cmd, "Attachs", DbType.String, this.CreateAttachsXML(info.Attachs));

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                this._db.AddInParameter(cmd, "Plans", DbType.String, this.CreatePlansXML(info.TourNormalInfo.Plans));
            else
                this._db.AddInParameter(cmd, "Plans", DbType.String, DBNull.Value);

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick)
                this._db.AddInParameter(cmd, "QuickPrivate", DbType.String, this.CreateQuickPrivateXML(info.TourQuickInfo));
            else
                this._db.AddInParameter(cmd, "QuickPrivate", DbType.String, DBNull.Value);

            this._db.AddInParameter(cmd, "Service", DbType.String, this.CreateServiceXML(info.Services));

            if (info.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Normal)
                this._db.AddInParameter(cmd, "NormalPrivate", DbType.String, this.CreateNormalPrivateXML(info.TourNormalInfo));
            else
                this._db.AddInParameter(cmd, "NormalPrivate", DbType.String, DBNull.Value);

            this._db.AddInParameter(cmd, "TourPrice", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "SinglePrivate", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "PlansSingle", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "TeamPrivate", DbType.String, this.CreateTeamPrivateXML(info));
            this._db.AddInParameter(cmd, "CustomerDisplayType", DbType.Byte, 0);
            this._db.AddInParameter(cmd, "Customers", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "CustomerFilePath", DbType.String, DBNull.Value);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            this._db.AddOutParameter(cmd, "OrderId", DbType.AnsiStringFixedLength, 36);
            this._db.AddInParameter(cmd, "CoordinatorId", DbType.Int32, info.Coordinator.CoordinatorId);
            this._db.AddInParameter(cmd, "GatheringTime", DbType.String, info.GatheringTime);
            this._db.AddInParameter(cmd, "GatheringPlace", DbType.String, info.GatheringPlace);
            this._db.AddInParameter(cmd, "GatheringSign", DbType.String, info.GatheringSign);
            this._db.AddInParameter(cmd, "SentPeoples", DbType.String, this.CreateSentPeoplesXML(info.SentPeoples));
            this._db.AddInParameter(cmd, "TourCityId", DbType.Int32, info.TourCityId);
            _db.AddInParameter(cmd, "TourTeamUnit", DbType.String, CreateTourTeamUnitXml(info.TourTeamUnit));
            this._db.AddInParameter(cmd, "TourTraffic", DbType.String, this.CreateTourTraffic(info.TourTraffic));

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

            info.OrderId = this._db.GetParameterValue(cmd, "OrderId").ToString();

            return Convert.ToInt32(this._db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 修改单项服务，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">单项服务信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        private int UpdateSingleTourInfo(EyouSoft.Model.TourStructure.TourSingleInfo info)
        {
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Tour_UpdateTourInfo");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, info.TourId);
            this._db.AddInParameter(cmd, "TourCode", DbType.String, info.TourCode);
            this._db.AddInParameter(cmd, "TourType", DbType.Byte, info.TourType);
            this._db.AddInParameter(cmd, "TourDays", DbType.Int32, info.TourDays);
            this._db.AddInParameter(cmd, "LDate", DbType.DateTime, info.LDate);
            this._db.AddInParameter(cmd, "AreaId", DbType.Int32, info.AreaId);
            this._db.AddInParameter(cmd, "RouteName", DbType.String, info.RouteName);
            this._db.AddInParameter(cmd, "PlanPeopleNumber", DbType.Int32, info.PlanPeopleNumber);
            this._db.AddInParameter(cmd, "ReleaseType", DbType.AnsiStringFixedLength, (int)info.ReleaseType);
            this._db.AddInParameter(cmd, "OperatorId", DbType.Int32, info.OperatorId);
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, info.CompanyId);
            this._db.AddInParameter(cmd, "LTraffic", DbType.String, info.LTraffic);
            this._db.AddInParameter(cmd, "RTraffic", DbType.String, info.RTraffic);
            this._db.AddInParameter(cmd, "Gather", DbType.String, info.Gather);
            this._db.AddInParameter(cmd, "RouteId", DbType.Int32, info.RouteId);
            this._db.AddInParameter(cmd, "LocalAgencys", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "Attachs", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "Plans", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "QuickPrivate", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "Service", DbType.String, this.CreateServiceXML(info.Services));
            this._db.AddInParameter(cmd, "NormalPrivate", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "TourPrice", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "SinglePrivate", DbType.String, this.CreateSinglePrivateXML(info));
            this._db.AddInParameter(cmd, "PlansSingle", DbType.String, this.CreatePlansSingleXML(info.Plans));
            this._db.AddInParameter(cmd, "TeamPrivate", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "CustomerDisplayType", DbType.Byte, info.CustomerDisplayType);
            this._db.AddInParameter(cmd, "Customers", DbType.String, this.CreateTourOrderCustomersXML(info.Customers));
            this._db.AddInParameter(cmd, "CustomerFilePath", DbType.String, info.CustomerFilePath);
            this._db.AddOutParameter(cmd, "Result", DbType.Int32, 4);
            this._db.AddOutParameter(cmd, "OrderId", DbType.AnsiStringFixedLength, 36);
            this._db.AddInParameter(cmd, "CoordinatorId", DbType.Int32, DBNull.Value);
            this._db.AddInParameter(cmd, "GatheringTime", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "GatheringPlace", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "GatheringSign", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "SentPeoples", DbType.String, DBNull.Value);
            this._db.AddInParameter(cmd, "TourCityId", DbType.Int32, DBNull.Value);

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

            info.OrderId = this._db.GetParameterValue(cmd, "OrderId").ToString();

            return Convert.ToInt32(this._db.GetParameterValue(cmd, "Result"));
        }

        /// <summary>
        /// 获取单项服务供应商安排信息集合
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.TourStructure.PlanSingleInfo> GetPlansSingle(string tourId)
        {
            IList<EyouSoft.Model.TourStructure.PlanSingleInfo> items = new List<EyouSoft.Model.TourStructure.PlanSingleInfo>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetPlansSingle);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.TourStructure.PlanSingleInfo()
                    {
                        AddAmount = rdr.GetDecimal(rdr.GetOrdinal("AddAmount")),
                        Amount = rdr.GetDecimal(rdr.GetOrdinal("Amount")),
                        Arrange = rdr["Arrange"].ToString(),
                        CreateTime = rdr.GetDateTime(rdr.GetOrdinal("CreateTime")),
                        FRemark = rdr["FRemark"].ToString(),
                        OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                        PlanId = rdr.GetString(rdr.GetOrdinal("PlanId")),
                        ReduceAmount = rdr.GetDecimal(rdr.GetOrdinal("ReduceAmount")),
                        Remark = rdr["Remark"].ToString(),
                        ServiceType = (EyouSoft.Model.EnumType.TourStructure.ServiceType)rdr.GetByte(rdr.GetOrdinal("ServiceType")),
                        SupplierId = rdr.GetInt32(rdr.GetOrdinal("SupplierId")),
                        SupplierName = rdr["SupplierName"].ToString(),
                        TotalAmount = rdr.GetDecimal(rdr.GetOrdinal("TotalAmount")),
                        PaidAmount = rdr.GetDecimal(rdr.GetOrdinal("PaidAmount")),
                    });
                }
            }

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    item.RegAmount = this.GetSingleOutRegAmount(item.PlanId);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取团队计划或单项服务订单信息
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private EyouSoft.Model.TourStructure.TourOrder GetSingleOrTeamOrderInfo(string tourId)
        {
            EyouSoft.Model.TourStructure.TourOrder info = null;

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetSingleOrTeamOrderInfo);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    info = new EyouSoft.Model.TourStructure.TourOrder()
                    {
                        ID = rdr.GetString(rdr.GetOrdinal("Id")),
                        SalerName = rdr["SalerName"].ToString(),
                        SalerId = rdr.GetInt32(rdr.GetOrdinal("SalerId")),
                        BuyCompanyID = rdr.GetInt32(rdr.GetOrdinal("BuyCompanyID")),
                        BuyCompanyName = rdr["BuyCompanyName"].ToString(),
                        OrderNo = rdr["OrderNo"].ToString(),
                        ContactName = rdr["ContactName"].ToString(),
                        ContactTel = rdr["ContactTel"].ToString(),
                        ContactMobile = rdr["ContactMobile"].ToString(),
                        CustomerDisplayType = (EyouSoft.Model.EnumType.TourStructure.CustomerDisplayType)rdr.GetByte(rdr.GetOrdinal("CustomerDisplayType")),
                        CustomerFilePath = rdr["CustomerFilePath"].ToString()
                    };
                }
            }

            return info;
        }

        /// <summary>
        /// 获取单项服务游客信息集合
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.TourStructure.TourOrderCustomer> GetSingleTravellers(string orderId)
        {
            IList<EyouSoft.Model.TourStructure.TourOrderCustomer> items = new List<EyouSoft.Model.TourStructure.TourOrderCustomer>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetSingleTravellers);
            this._db.AddInParameter(cmd, "OrderId", DbType.AnsiStringFixedLength, orderId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.TourStructure.CustomerSpecialService specialServiceInfo = null;

                    if (!rdr.IsDBNull(rdr.GetOrdinal("CustormerId")))
                    {
                        specialServiceInfo = new EyouSoft.Model.TourStructure.CustomerSpecialService()
                        {
                            CustormerId = rdr.GetString(rdr.GetOrdinal("Id")),
                            Fee = rdr.GetDecimal(rdr.GetOrdinal("Fee")),
                            IsAdd = this.GetBoolean(rdr["IsAdd"].ToString()),
                            IssueTime = rdr.GetDateTime(rdr.GetOrdinal("BIssueTime")),
                            ProjectName = rdr["ProjectName"].ToString(),
                            ServiceDetail = rdr["ServiceDetail"].ToString(),
                        };
                    }

                    items.Add(new EyouSoft.Model.TourStructure.TourOrderCustomer()
                    {
                        CompanyID = rdr.GetInt32(rdr.GetOrdinal("CompanyId")),
                        ContactTel = rdr["ContactTel"].ToString(),
                        CradNumber = rdr["CradNumber"].ToString(),
                        CradType = (EyouSoft.Model.EnumType.TourStructure.CradType)rdr.GetByte(rdr.GetOrdinal("CradType")),
                        CustomerStatus = (EyouSoft.Model.EnumType.TourStructure.CustomerStatus)rdr.GetByte(rdr.GetOrdinal("CustomerStatus")),
                        ID = rdr.GetString(rdr.GetOrdinal("Id")),
                        IssueTime = rdr.GetDateTime(rdr.GetOrdinal("IssueTime")),
                        LeagueInfo = null,
                        OrderId = orderId,
                        Sex = (EyouSoft.Model.EnumType.CompanyStructure.Sex)rdr.GetByte(rdr.GetOrdinal("Sex")),
                        SpecialServiceInfo = specialServiceInfo,
                        VisitorName = rdr["VisitorName"].ToString(),
                        VisitorType = (EyouSoft.Model.EnumType.TourStructure.VisitorType)rdr.GetByte(rdr.GetOrdinal("VisitorType"))
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 获取快速发布计划专有信息业务实体
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private EyouSoft.Model.TourStructure.TourQuickPrivateInfo GetTourQuickPrivateInfo(string tourId)
        {
            EyouSoft.Model.TourStructure.TourQuickPrivateInfo info = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourQuickPrivateInfo);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    info = new EyouSoft.Model.TourStructure.TourQuickPrivateInfo()
                    {
                        QuickPlan = rdr["Plan"].ToString(),
                        Remark = rdr["Remark"].ToString(),
                        Service = rdr["Service"].ToString()
                    };
                }
            }

            return info;
        }

        /// <summary>
        /// 获取标准发布计划行程信息集合
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.TourStructure.TourPlanInfo> GetTourJourneys(string tourId)
        {
            IList<EyouSoft.Model.TourStructure.TourPlanInfo> items = new List<EyouSoft.Model.TourStructure.TourPlanInfo>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourJourneys);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.TourStructure.TourPlanInfo()
                    {
                        Dinner = rdr["Dinner"].ToString(),
                        FilePath = rdr["FilePath"].ToString(),
                        Hotel = rdr["Hotel"].ToString(),
                        Interval = rdr["Interval"].ToString(),
                        Plan = rdr["Plan"].ToString(),
                        Vehicle = rdr["Vehicle"].ToString()
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 获取标准发布散拼计划、团队计划专有信息业务实体
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private EyouSoft.Model.TourStructure.TourNormalPrivateInfo GetTourNormalPrivateInfo(string tourId)
        {
            EyouSoft.Model.TourStructure.TourNormalPrivateInfo info = null;
            IList<EyouSoft.Model.TourStructure.TourServiceInfo> services = null;

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_TourNormalPrivateInfo);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                info = new EyouSoft.Model.TourStructure.TourNormalPrivateInfo();
                services = new List<EyouSoft.Model.TourStructure.TourServiceInfo>();

                while (rdr.Read())
                {
                    switch (rdr.GetString(rdr.GetOrdinal("Key")))
                    {
                        case "DiJie":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.地接
                            });
                            break;
                        case "JiuDian":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.酒店
                            });
                            break;
                        case "YongCan":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.用餐
                            });
                            break;
                        case "XiaoJiaoTong":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.小交通
                            });
                            break;
                        case "DaJiaoTong":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.大交通
                            });
                            break;
                        case "JingDian":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.景点
                            });
                            break;
                        case "GouWu":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.购物
                            });
                            break;
                        case "BaoXian":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.保险
                            });
                            break;
                        case "QiTa":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.其它
                            });
                            break;
                        case "DaoFu":
                            services.Add(new EyouSoft.Model.TourStructure.TourServiceInfo()
                            {
                                Service = rdr["Value"].ToString(),
                                ServiceType = EyouSoft.Model.EnumType.TourStructure.ServiceType.导服
                            });
                            break;
                        case "BuHanXiangMu":
                            info.BuHanXiangMu = rdr["Value"].ToString();
                            break;
                        case "GouWuAnPai":
                            info.GouWuAnPai = rdr["Value"].ToString();
                            break;
                        case "ErTongAnPai":
                            info.ErTongAnPai = rdr["Value"].ToString();
                            break;
                        case "ZiFeiXIangMu":
                            info.ZiFeiXIangMu = rdr["Value"].ToString();
                            break;
                        case "ZhuYiShiXiang":
                            info.ZhuYiShiXiang = rdr["Value"].ToString();
                            break;
                        case "WenXinTiXing":
                            info.WenXinTiXing = rdr["Value"].ToString();
                            break;
                        case "NeiBuXingXi":
                            info.NeiBuXingXi = rdr["Value"].ToString();
                            break;
                    }
                }
            }

            if (info != null)
            {
                info.Services = services;
                info.Plans = this.GetTourJourneys(tourId);
            }

            return info;
        }

        /// <summary>
        /// 获取团队附件信息集合
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.TourStructure.TourAttachInfo> GetTourAttachs(string tourId)
        {
            IList<EyouSoft.Model.TourStructure.TourAttachInfo> items = new List<EyouSoft.Model.TourStructure.TourAttachInfo>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourAttachs);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.TourStructure.TourAttachInfo()
                    {
                        FilePath = rdr["FilePath"].ToString(),
                        Name = rdr["Name"].ToString()
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 获取团队计划包含项目信息集合
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.TourStructure.TourTeamServiceInfo> GetTourTeamServices(string tourId)
        {
            IList<EyouSoft.Model.TourStructure.TourTeamServiceInfo> items = new List<EyouSoft.Model.TourStructure.TourTeamServiceInfo>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourTeamOrSingleServices);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.TourStructure.TourTeamServiceInfo()
                    {
                        LocalPrice = rdr.GetDecimal(rdr.GetOrdinal("LocalPrice")),
                        SelfPrice = rdr.GetDecimal(rdr.GetOrdinal("MyPrice")),
                        Service = rdr["Standard"].ToString(),
                        ServiceType = (EyouSoft.Model.EnumType.TourStructure.ServiceType)rdr.GetByte(rdr.GetOrdinal("ItemType")),
                        LocalPeopleNumber = rdr.GetInt32(rdr.GetOrdinal("LocalPeopleNumber")),
                        LocalUnitPrice = rdr.GetDecimal(rdr.GetOrdinal("LocalUnitPrice")),
                        SelfPeopleNumber = rdr.GetInt32(rdr.GetOrdinal("SelfPeopleNumber")),
                        SelfUnitPrice = rdr.GetDecimal(rdr.GetOrdinal("SelfUnitPrice"))
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 获取单项服务具体要求信息集合
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.TourStructure.TourSingleServiceInfo> GetTourSingleServices(string tourId)
        {
            IList<EyouSoft.Model.TourStructure.TourSingleServiceInfo> items = new List<EyouSoft.Model.TourStructure.TourSingleServiceInfo>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourTeamOrSingleServices);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.TourStructure.TourSingleServiceInfo()
                    {
                        SelfPrice = rdr.GetDecimal(rdr.GetOrdinal("MyPrice")),
                        Remark = rdr["Remark"].ToString(),
                        Requirement = rdr["Requirement"].ToString(),
                        ServiceType = (EyouSoft.Model.EnumType.TourStructure.ServiceType)rdr.GetByte(rdr.GetOrdinal("ItemType"))
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 获取客户主要联系人信息
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        private EyouSoft.Model.CompanyStructure.CustomerInfo GetCustomerInfo(int cid)
        {
            EyouSoft.Model.CompanyStructure.CustomerInfo info = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetCustomerInfo);
            this._db.AddInParameter(cmd, "CID", DbType.Int32, cid);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    info = new EyouSoft.Model.CompanyStructure.CustomerInfo()
                    {
                        ContactName = rdr["ContactName"].ToString(),
                        Phone = rdr["Phone"].ToString(),
                        Fax = rdr["Fax"].ToString(),
                        Mobile = rdr["Mobile"].ToString()
                    };
                }
            }

            return info;
        }

        /// <summary>
        /// 获取团队所有订单实收人数
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private int GetTourRealityPeopleNumber(string tourId)
        {
            int peopleNumber = 0;

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourRealityPeopleNumber);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) peopleNumber = rdr.GetInt32(0);
                }
            }

            return peopleNumber;
        }

        /// <summary>
        /// 获取单项服务支出已登记金额
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        private decimal GetSingleOutRegAmount(string planId)
        {
            decimal amount = 0;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetSingleOutRegAmount);
            this._db.AddInParameter(cmd, "PlanId", DbType.AnsiStringFixedLength, planId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    amount = rdr.IsDBNull(rdr.GetOrdinal("Amount")) ? 0 : rdr.GetDecimal(rdr.GetOrdinal("Amount"));
                }
            }

            return amount;
        }

        /// <summary>
        /// 获取计划实际收客人数
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private int GetTourPeopleNumberShiShou(string tourId)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourPeopleNumberShiShou);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    return rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
                }
            }

            return 0;
        }

        /// <summary>
        /// 获取利润统计团队数订单信息集合
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.TourStructure.LBLYTJTourOrderInfo> GetLYTJTourOrdersByXml(string xml, int[] salers)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<EyouSoft.Model.TourStructure.LBLYTJTourOrderInfo> items = new List<EyouSoft.Model.TourStructure.LBLYTJTourOrderInfo>();

            XElement xRoot = XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");

            foreach (var xRow in xRows)
            {
                var status = (EyouSoft.Model.EnumType.TourStructure.OrderState)Utils.GetInt(Utils.GetXAttributeValue(xRow, "OrderState"));
                //订单状态筛选
                if (status != EyouSoft.Model.EnumType.TourStructure.OrderState.已成交) continue;
                //销售员筛选
                if (salers != null && salers.Length > 0 && !salers.Contains(Utils.GetInt(Utils.GetXAttributeValue(xRow, "SalerId")))) continue;

                items.Add(new EyouSoft.Model.TourStructure.LBLYTJTourOrderInfo()
                {
                    AdultNumber = Utils.GetInt(Utils.GetXAttributeValue(xRow, "AdultNumber")),
                    BuyCompanyName = Utils.GetXAttributeValue(xRow, "BuyCompanyName"),
                    ChildrenNumber = Utils.GetInt(Utils.GetXAttributeValue(xRow, "ChildNumber")),
                    PeopleNumber = Utils.GetInt(Utils.GetXAttributeValue(xRow, "PeopleNumber")),
                    ReceivedAmount = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "HasCheckMoney")),
                    TotalAmount = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "FinanceSum")),
                });
            }



            return items;
        }

        /// <summary>
        /// 获取利润统计团队数供应商安排信息集合
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="serviceType">计调项目类型</param>
        /// <returns></returns>
        private IList<EyouSoft.Model.TourStructure.LBLYTJTourPlanInfo> GetLYTJTourPlansByXml(string xml, EyouSoft.Model.EnumType.TourStructure.ServiceType? serviceType)
        {
            if (string.IsNullOrEmpty(xml)) return null;
            IList<EyouSoft.Model.TourStructure.LBLYTJTourPlanInfo> items = new List<EyouSoft.Model.TourStructure.LBLYTJTourPlanInfo>();

            XElement xRoot = XElement.Parse(xml);
            var xRows = Utils.GetXElements(xRoot, "row");

            foreach (var xRow in xRows)
            {
                EyouSoft.Model.TourStructure.LBLYTJTourPlanInfo item = new EyouSoft.Model.TourStructure.LBLYTJTourPlanInfo();
                item.PaidAmount = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "PaidAmount"));
                item.SupplierName = Utils.GetXAttributeValue(xRow, "SupplierName");
                item.TotalAmount = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "TotalAmount"));
                item.ServiceType = serviceType.HasValue ? serviceType.Value : (EyouSoft.Model.EnumType.TourStructure.ServiceType)Utils.GetInt(Utils.GetXAttributeValue(xRow, "ServiceType"));

                if (serviceType.HasValue && serviceType.Value == EyouSoft.Model.EnumType.TourStructure.ServiceType.大交通)
                {
                    item.PeopleNumber = Utils.GetInt(Utils.GetXAttributeValue(xRow, "PeopleCount"));
                }

                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// 获取团队/散拼计划计调员信息
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private EyouSoft.Model.TourStructure.TourCoordinatorInfo GetTourCoordinator(string tourId)
        {
            EyouSoft.Model.TourStructure.TourCoordinatorInfo info = null;
            var items = this.GetTourCoordinators(tourId);
            if (items != null && items.Count > 0) info = items[0];
            return info;
        }

        /// <summary>
        /// 获取计划出港城市信息
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private int GetTourCity(string tourId)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourCitys);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    return rdr.IsDBNull(0) ? 0 : rdr.GetInt32(0);
                }
            }

            return 0;
        }

        /// <summary>
        /// 获取出回团提醒组团社信息业务实体
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private IList<EyouSoft.Model.PersonalCenterStructure.MTourRemindTravelAgencyInfo> GetTourRemindTravelAgencyInfo(string tourId)
        {
            IList<EyouSoft.Model.PersonalCenterStructure.MTourRemindTravelAgencyInfo> items = new List<EyouSoft.Model.PersonalCenterStructure.MTourRemindTravelAgencyInfo>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourRemindTravelAgencyInfo);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.PersonalCenterStructure.MTourRemindTravelAgencyInfo()
                    {
                        AgencyId = rdr.GetInt32(rdr.GetOrdinal("Id")),
                        AgencyName = rdr["Name"].ToString(),
                        ContactName = rdr["ContactName"].ToString(),
                        Fax = rdr["Fax"].ToString(),
                        Mobile = rdr["Mobile"].ToString(),
                        Telephone = rdr["Phone"].ToString()
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 获取团队交通信息
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private IList<int> GetTourTraffic(string tourId)
        {
            if (string.IsNullOrEmpty(tourId)) return null;

            var strSql = new StringBuilder();
            strSql.Append(" select TourId,TrafficId from tbl_TourTraffic where TourId = @TourId; ");
            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "TourId", DbType.AnsiStringFixedLength, tourId);

            IList<int> list = new List<int>();
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                while (dr.Read())
                {
                    if (!dr.IsDBNull(dr.GetOrdinal("TrafficId")))
                        list.Add(dr.GetInt32(dr.GetOrdinal("TrafficId")));
                }
            }

            return list;
        }

        #endregion

        #region EyouSoft.IDAL.TourStructure.ITour Members
        /// <summary>
        /// 写入计划信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">计划信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int InsertTourInfo(EyouSoft.Model.TourStructure.TourBaseInfo info)
        {
            int returnValue = 0;

            switch (info.TourType)
            {
                case EyouSoft.Model.EnumType.TourStructure.TourType.单项服务:
                    returnValue = this.InsertSingleTourInfo((EyouSoft.Model.TourStructure.TourSingleInfo)info);
                    break;
                case EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划:
                    returnValue = this.InsertTourInfo((EyouSoft.Model.TourStructure.TourInfo)info);
                    break;
                case EyouSoft.Model.EnumType.TourStructure.TourType.团队计划:
                    returnValue = this.InsertTeamTourInfo((EyouSoft.Model.TourStructure.TourTeamInfo)info);
                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// 修改计划信息，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="info">计划信息业务实体</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int UpdateTourInfo(EyouSoft.Model.TourStructure.TourBaseInfo info)
        {
            int returnValue = 0;

            switch (info.TourType)
            {
                case EyouSoft.Model.EnumType.TourStructure.TourType.单项服务:
                    returnValue = this.UpdateSingleTourInfo((EyouSoft.Model.TourStructure.TourSingleInfo)info);
                    break;
                case EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划:
                    returnValue = this.UpdateTourInfo((EyouSoft.Model.TourStructure.TourInfo)info);
                    break;
                case EyouSoft.Model.EnumType.TourStructure.TourType.团队计划:
                    returnValue = this.UpdateTeamTourInfo((EyouSoft.Model.TourStructure.TourTeamInfo)info);
                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// 删除计划信息
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <returns></returns>
        public bool Delete(string tourId)
        {
            //DbCommand cmd = this._db.GetSqlStringCommand(SQL_UPDATE_Delete);
            //this._db.AddInParameter(cmd, "V", DbType.AnsiStringFixedLength, "1");
            //this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);
            //return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;

            DbCommand dc = this._db.GetStoredProcCommand("proc_Tour_UpdateIsDelete");
            this._db.AddInParameter(dc, "TourId", DbType.AnsiStringFixedLength, tourId);
            this._db.AddOutParameter(dc, "ErrorValue", DbType.Int32, 4);

            DbHelper.RunProcedure(dc, this._db);
            object obj = this._db.GetParameterValue(dc, "ErrorValue");
            if (obj.Equals(null))
                return false;
            else
                return int.Parse(obj.ToString()) > 0 ? true : false;
        }

        /// <summary>
        /// 获取散拼计划信息集合（专线端）
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourBaseInfo> GetTours(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info, string us)
        {
            IList<EyouSoft.Model.TourStructure.TourBaseInfo> items = new List<EyouSoft.Model.TourStructure.TourBaseInfo>();
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_Tour";
            string primaryKey = "TourId";
            string orderByString = "IsLeave ASC,LeaveDate ASC";
            StringBuilder fields = new StringBuilder();

            #region fields
            fields.Append("TourId,LeaveDate,TourCode,Status,IsCostConfirm,IsReview,TicketStatus,RouteName,PlanPeopleNumber,VirtualPeopleNumber,ReleaseType,TourDays,RouteStatus,HandStatus ");
            fields.Append(",(SELECT SUM(PeopleNumber) FROM [tbl_TourOrder] WHERE TourId=tbl_Tour.TourId AND IsDelete='0' AND OrderState=2) AS LiuWei");
            fields.Append(",(SELECT SUM(PeopleNumber) FROM [tbl_TourOrder] WHERE TourId=tbl_Tour.TourId AND IsDelete='0' AND OrderState=1) AS WeiChuLi");
            fields.Append(",(SELECT SUM(PeopleNumber-LeaguePepoleNum) FROM [tbl_TourOrder] WHERE TourId=tbl_Tour.TourId AND IsDelete='0' AND OrderState IN(1,2,5)) AS ShiShou");
            #endregion

            #region 拼接查询条件
            cmdQuery.AppendFormat(" CompanyId={0} ", companyId);
            cmdQuery.Append(" AND IsDelete='0' AND TemplateId>'' ");
            cmdQuery.AppendFormat(" AND TourType={0} ", (int)EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划);
            info = info ?? new EyouSoft.Model.TourStructure.TourSearchInfo();
            if (!string.IsNullOrEmpty(us))
            {
                cmdQuery.AppendFormat(" AND OperatorId IN({0}) ", us);
            }
            if (info.EDate.HasValue)
            {
                cmdQuery.AppendFormat(" AND LeaveDate<='{0}' ", info.EDate.Value);
            }
            if (info.FDate.HasValue)
            {
                cmdQuery.AppendFormat(" AND LeaveDate='{0}' ", info.FDate.Value);
            }
            if (info.OrderStatus.HasValue || (info.Sellers != null && info.Sellers.Length > 0))
            {
                cmdQuery.Append(" AND EXISTS(SELECT 1 FROM tbl_TourOrder WHERE TourId=tbl_Tour.TourId ");

                if (info.OrderStatus.HasValue)
                {
                    cmdQuery.AppendFormat(" AND OrderState={0} ", (int)info.OrderStatus.Value);
                }

                if (info.Sellers != null && info.Sellers.Length > 0)
                {
                    cmdQuery.AppendFormat(" AND SalerId IN({0}) ", Utils.GetSqlIdStrByArray(info.Sellers));
                }

                cmdQuery.Append(" ) ");
            }
            if (!string.IsNullOrEmpty(info.RouteName))
            {
                cmdQuery.AppendFormat(" AND RouteName LIKE '%{0}%' ", info.RouteName);
            }
            if (info.SDate.HasValue)
            {
                cmdQuery.AppendFormat(" AND LeaveDate>='{0}' ", info.SDate.Value);
            }
            if (!string.IsNullOrEmpty(info.TourCode))
            {
                cmdQuery.AppendFormat(" AND TourCode LIKE '%{0}%' ", info.TourCode);
            }
            if (info.TourDays.HasValue)
            {
                cmdQuery.AppendFormat(" AND TourDays={0} ", info.TourDays.Value);
            }
            if (info.TourStatus.HasValue)
            {
                cmdQuery.AppendFormat(" AND Status={0} ", (int)info.TourStatus.Value);
            }
            if (info.AreaId.HasValue)
            {
                cmdQuery.AppendFormat(" AND AreaId={0} ", info.AreaId.Value);
            }
            if (!string.IsNullOrEmpty(info.TourId))
            {
                cmdQuery.AppendFormat(" AND TourId='{0}' ", info.TourId);
            }
            if (info.Areas != null && info.Areas.Length > 0)
            {
                cmdQuery.Append(" AND AreaId IN( ");
                cmdQuery.AppendFormat(" {0} ", info.Areas[0]);
                for (int i = 1; i < info.Areas.Length; i++)
                {
                    cmdQuery.AppendFormat(",{0}", info.Areas[i]);
                }
                cmdQuery.Append(" ) ");
            }
            if (info.Coordinators != null && info.Coordinators.Length > 0)
            {
                cmdQuery.AppendFormat(" AND EXISTS (SELECT 1 FROM [tbl_TourOperator] AS A WHERE A.TourId=tbl_Tour.TourId AND A.OperatorId IN({0})) ", Utils.GetSqlIdStrByArray(info.Coordinators));
            }
            if (info.TourCityId.HasValue)
            {
                cmdQuery.AppendFormat(" AND EXISTS (SELECT 1 FROM tbl_TourCity AS A WHERE A.TourId = tbl_Tour.TourId AND A.CityId = {0})", info.TourCityId.Value);
            }
            if (!string.IsNullOrEmpty(info.YouKeName))
            {
                cmdQuery.AppendFormat(" AND EXISTS(SELECT 1 FROM [tbl_TourOrderCustomer] AS A WHERE A.TourId=tbl_Tour.TourId AND A.[VisitorName] LIKE '%{0}%') ", info.YouKeName);
            }
            #endregion

            #region 排序
            if (info != null && info.SortType.HasValue)
            {
                switch (info.SortType.Value)
                {
                    case EyouSoft.Model.EnumType.TourStructure.TourSortType.出团日期降序:
                        orderByString = "LeaveDate DESC";
                        break;
                    case EyouSoft.Model.EnumType.TourStructure.TourSortType.出团日期升序:
                        orderByString = "LeaveDate ASC";
                        break;
                    case EyouSoft.Model.EnumType.TourStructure.TourSortType.默认:
                        orderByString = "IsLeave ASC,LeaveDate ASC";
                        break;
                    case EyouSoft.Model.EnumType.TourStructure.TourSortType.收客状态降序:
                        orderByString = "Status DESC";
                        break;
                    case EyouSoft.Model.EnumType.TourStructure.TourSortType.收客状态升序:
                        orderByString = "Status ASC";
                        break;
                }
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.TourStructure.TourBaseInfo item = new EyouSoft.Model.TourStructure.TourBaseInfo();

                    item.TourId = rdr.GetString(rdr.GetOrdinal("TourId"));
                    item.LDate = rdr.GetDateTime(rdr.GetOrdinal("LeaveDate"));
                    item.TourCode = rdr["TourCode"].ToString();
                    item.Status = (EyouSoft.Model.EnumType.TourStructure.TourStatus)rdr.GetByte(rdr.GetOrdinal("Status"));
                    item.IsCostConfirm = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsCostConfirm")));
                    item.IsReview = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsReview")));
                    item.TicketStatus = (EyouSoft.Model.EnumType.PlanStructure.TicketState)rdr.GetByte(rdr.GetOrdinal("TicketStatus"));
                    item.RouteName = rdr["RouteName"].ToString();
                    item.PlanPeopleNumber = rdr.GetInt32(rdr.GetOrdinal("PlanPeopleNumber"));
                    item.VirtualPeopleNumber = rdr.GetInt32(rdr.GetOrdinal("VirtualPeopleNumber"));
                    item.ReleaseType = (EyouSoft.Model.EnumType.TourStructure.ReleaseType)int.Parse(rdr.GetString(rdr.GetOrdinal("ReleaseType")));
                    item.PeopleNumberLiuWei = rdr.IsDBNull(rdr.GetOrdinal("LiuWei")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("LiuWei"));
                    item.PeopleNumberWeiChuLi = rdr.IsDBNull(rdr.GetOrdinal("WeiChuLi")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("WeiChuLi"));
                    item.PeopleNumberShiShou = rdr.IsDBNull(rdr.GetOrdinal("ShiShou")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("ShiShou"));
                    item.TourDays = rdr.GetInt32(rdr.GetOrdinal("TourDays"));

                    item.TourRouteStatus = rdr.IsDBNull(rdr.GetOrdinal("RouteStatus"))
                                               ? Model.EnumType.TourStructure.TourRouteStatus.无
                                               : (Model.EnumType.TourStructure.TourRouteStatus)rdr.GetByte(rdr.GetOrdinal("RouteStatus"));
                    item.HandStatus = rdr.IsDBNull(rdr.GetOrdinal("HandStatus"))
                                               ? Model.EnumType.TourStructure.HandStatus.无
                                               : (Model.EnumType.TourStructure.HandStatus)rdr.GetByte(rdr.GetOrdinal("HandStatus"));

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取团队计划信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBTeamTourInfo> GetToursTeam(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info, string us)
        {
            IList<EyouSoft.Model.TourStructure.LBTeamTourInfo> items = new List<EyouSoft.Model.TourStructure.LBTeamTourInfo>();
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_Tour";
            string primaryKey = "TourId";
            string orderByString = "IsLeave ASC,LeaveDate ASC";
            StringBuilder fields = new StringBuilder();

            #region fields
            fields.Append("TourId,LeaveDate,TourCode,Status,IsCostConfirm,IsReview,TicketStatus,RouteName,PlanPeopleNumber,ReleaseType,TourDays,TotalIncome,TotalExpenses,TotalOtherIncome,TotalOtherExpenses,DistributionAmount,(SELECT SUM(LeaguePepoleNum) FROM tbl_TourOrder WHERE TourId=tbl_Tour.Tourid) AS TuiTuanRenShu");
            #endregion

            #region 拼接查询条件
            cmdQuery.AppendFormat(" CompanyId={0} ", companyId);
            cmdQuery.Append(" AND IsDelete='0' ");
            cmdQuery.AppendFormat(" AND TourType={0} ", (int)EyouSoft.Model.EnumType.TourStructure.TourType.团队计划);
            info = info ?? new EyouSoft.Model.TourStructure.TourSearchInfo();
            if (!string.IsNullOrEmpty(us))
            {
                cmdQuery.AppendFormat(" AND OperatorId IN({0}) ", us);
            }
            if (info.EDate.HasValue)
            {
                cmdQuery.AppendFormat(" AND LeaveDate<='{0}' ", info.EDate.Value);
            }
            if (info.FDate.HasValue)
            {
                cmdQuery.AppendFormat(" AND LeaveDate='{0}' ", info.FDate.Value);
            }
            if (info.OrderStatus.HasValue || (info.Sellers != null && info.Sellers.Length > 0))
            {
                cmdQuery.Append(" AND EXISTS(SELECT 1 FROM tbl_TourOrder WHERE TourId=tbl_Tour.TourId ");
                if (info.OrderStatus.HasValue)
                {
                    cmdQuery.AppendFormat(" AND OrderState={0} ", (int)info.OrderStatus.Value);
                }
                if (info.Sellers != null && info.Sellers.Length > 0)
                {
                    cmdQuery.AppendFormat(" AND SalerId IN({0}) ", Utils.GetSqlIdStrByArray(info.Sellers));
                }
                cmdQuery.Append(" ) ");
            }
            if (!string.IsNullOrEmpty(info.RouteName))
            {
                cmdQuery.AppendFormat(" AND RouteName LIKE '%{0}%' ", info.RouteName);
            }
            if (info.SDate.HasValue)
            {
                cmdQuery.AppendFormat(" AND LeaveDate>='{0}' ", info.SDate.Value);
            }
            if (!string.IsNullOrEmpty(info.TourCode))
            {
                cmdQuery.AppendFormat(" AND TourCode LIKE '%{0}%' ", info.TourCode);
            }
            if (info.TourDays.HasValue)
            {
                cmdQuery.AppendFormat(" AND TourDays={0} ", info.TourDays.Value);
            }
            if (info.TourStatus.HasValue)
            {
                cmdQuery.AppendFormat(" AND Status={0} ", (int)info.TourStatus.Value);
            }
            if (info.AreaId.HasValue)
            {
                cmdQuery.AppendFormat(" AND AreaId={0} ", info.AreaId.Value);
            }
            if (!string.IsNullOrEmpty(info.TourId))
            {
                cmdQuery.AppendFormat(" AND TourId='{0}' ", info.TourId);
            }
            if (info.Coordinators != null && info.Coordinators.Length > 0)
            {
                cmdQuery.AppendFormat(" AND EXISTS (SELECT 1 FROM [tbl_TourOperator] AS A WHERE A.TourId=tbl_Tour.TourId AND A.OperatorId IN({0})) ", Utils.GetSqlIdStrByArray(info.Coordinators));
            }
            if (!string.IsNullOrEmpty(info.YouKeName))
            {
                cmdQuery.AppendFormat(" AND EXISTS(SELECT 1 FROM [tbl_TourOrderCustomer] AS A WHERE A.TourId=tbl_Tour.TourId AND A.[VisitorName] LIKE '%{0}%') ", info.YouKeName);
            }
            #endregion

            #region 排序
            if (info != null && info.SortType.HasValue)
            {
                switch (info.SortType.Value)
                {
                    case EyouSoft.Model.EnumType.TourStructure.TourSortType.出团日期降序:
                        orderByString = "LeaveDate DESC";
                        break;
                    case EyouSoft.Model.EnumType.TourStructure.TourSortType.出团日期升序:
                        orderByString = "LeaveDate ASC";
                        break;
                    case EyouSoft.Model.EnumType.TourStructure.TourSortType.默认:
                        orderByString = "IsLeave ASC,LeaveDate ASC";
                        break;
                    case EyouSoft.Model.EnumType.TourStructure.TourSortType.收客状态降序:
                        orderByString = "Status DESC";
                        break;
                    case EyouSoft.Model.EnumType.TourStructure.TourSortType.收客状态升序:
                        orderByString = "Status ASC";
                        break;
                }
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.TourStructure.LBTeamTourInfo item = new EyouSoft.Model.TourStructure.LBTeamTourInfo();

                    item.TourId = rdr.GetString(rdr.GetOrdinal("TourId"));
                    item.LDate = rdr.GetDateTime(rdr.GetOrdinal("LeaveDate"));
                    item.TourCode = rdr["TourCode"].ToString();
                    item.Status = (EyouSoft.Model.EnumType.TourStructure.TourStatus)rdr.GetByte(rdr.GetOrdinal("Status"));
                    item.IsCostConfirm = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsCostConfirm")));
                    item.IsReview = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsReview")));
                    item.TicketStatus = (EyouSoft.Model.EnumType.PlanStructure.TicketState)rdr.GetByte(rdr.GetOrdinal("TicketStatus"));
                    item.RouteName = rdr["RouteName"].ToString();
                    item.PlanPeopleNumber = rdr.GetInt32(rdr.GetOrdinal("PlanPeopleNumber")) - rdr.GetInt32(rdr.GetOrdinal("TuiTuanRenShu"));
                    item.ReleaseType = (EyouSoft.Model.EnumType.TourStructure.ReleaseType)int.Parse(rdr.GetString(rdr.GetOrdinal("ReleaseType")));
                    item.TourDays = rdr.GetInt32(rdr.GetOrdinal("TourDays"));
                    item.TotalIncome = rdr.GetDecimal(rdr.GetOrdinal("TotalIncome"));
                    item.TotalExpenses = rdr.GetDecimal(rdr.GetOrdinal("TotalExpenses"));
                    item.TotalOtherIncome = rdr.GetDecimal(rdr.GetOrdinal("TotalOtherIncome"));
                    item.TotalOtherExpenses = rdr.GetDecimal(rdr.GetOrdinal("TotalOtherExpenses"));
                    item.DistributionAmount = rdr.GetDecimal(rdr.GetOrdinal("DistributionAmount"));

                    EyouSoft.Model.TourStructure.TourOrder orderInfo = this.GetSingleOrTeamOrderInfo(item.TourId);
                    EyouSoft.Model.CompanyStructure.CustomerInfo customerInfo = this.GetCustomerInfo(orderInfo.BuyCompanyID);
                    if (orderInfo != null)
                    {
                        item.BuyerCId = orderInfo.BuyCompanyID;
                        item.BuyerCName = orderInfo.BuyCompanyName;
                        item.OrderId = orderInfo.ID;
                    }
                    if (customerInfo != null)
                    {
                        item.BuyerContacterName = customerInfo.ContactName;
                        item.BuyerContacterTelephone = customerInfo.Phone;
                    }

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取单项服务信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBSingleTourInfo> GetToursSingle(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSingleSearchInfo info, string us)
        {
            IList<EyouSoft.Model.TourStructure.LBSingleTourInfo> items = new List<EyouSoft.Model.TourStructure.LBSingleTourInfo>();
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_Tour";
            string primaryKey = "TourId";
            string orderByString = "IsLeave ASC,LeaveDate ASC";
            StringBuilder fields = new StringBuilder();

            #region fields
            fields.Append("TourId,Status,TourCode");
            fields.Append(",(SELECT ContactName FROM tbl_CompanyUser WHERE Id=tbl_Tour.OperatorId ) AS OperatorName ");
            #endregion

            #region 拼接查询条件
            cmdQuery.AppendFormat(" CompanyId={0} ", companyId);
            cmdQuery.Append(" AND IsDelete='0' ");
            cmdQuery.AppendFormat(" AND TourType={0} ", (int)EyouSoft.Model.EnumType.TourStructure.TourType.单项服务);
            info = info ?? new EyouSoft.Model.TourStructure.TourSingleSearchInfo();
            if (!string.IsNullOrEmpty(us))
            {
                cmdQuery.AppendFormat(" AND OperatorId IN({0}) ", us);
            }
            if (info.BuyerCId.HasValue || !string.IsNullOrEmpty(info.OrderCode) || info.OrderETime.HasValue || info.OrderFTime.HasValue || info.OrderSTime.HasValue || !string.IsNullOrEmpty(info.BuyerCName))
            {
                cmdQuery.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_TourOrder WHERE TourId=tbl_Tour.TourId ");

                if (info.BuyerCId.HasValue)
                {
                    cmdQuery.AppendFormat(" AND BuyCompanyID={0} ", info.BuyerCId.Value);
                }
                if (!string.IsNullOrEmpty(info.OrderCode))
                {
                    cmdQuery.AppendFormat(" AND OrderNo='{0}' ", info.OrderCode);
                }
                if (info.OrderETime.HasValue)
                {
                    //cmdQuery.AppendFormat(" AND IssueTime<='{0}' ", info.OrderETime.Value);
                    cmdQuery.AppendFormat(" AND DATEDIFF(DAY,IssueTime,'{0}')>=0 ", info.OrderETime.Value);
                }
                if (info.OrderFTime.HasValue)
                {
                    //cmdQuery.AppendFormat(" AND IssueTime='{0}' ", info.OrderFTime.Value);
                    cmdQuery.AppendFormat(" AND DATEDIFF(DAY,IssueTime,'{0}')=0 ", info.OrderFTime.Value);
                }
                if (info.OrderSTime.HasValue)
                {
                    //cmdQuery.AppendFormat(" AND IssueTime>='{0}' ", info.OrderSTime.Value);
                    cmdQuery.AppendFormat(" AND DATEDIFF(DAY,IssueTime,'{0}')<=0 ", info.OrderSTime.Value);
                }
                if (!string.IsNullOrEmpty(info.BuyerCName))
                {
                    cmdQuery.AppendFormat(" AND BuyCompanyName LIKE '%{0}%' ", info.BuyerCName);
                }

                cmdQuery.Append(" ) ");
            }
            if (info.OperatorId != null && info.OperatorId.Length > 0)
            {
                cmdQuery.AppendFormat(" AND OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(info.OperatorId));
            }
            if (!string.IsNullOrEmpty(info.TourCode))
            {
                cmdQuery.AppendFormat(" AND TourCode LIKE '%{0}%' ", info.TourCode);
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.TourStructure.LBSingleTourInfo item = new EyouSoft.Model.TourStructure.LBSingleTourInfo();

                    item.TourId = rdr.GetString(rdr.GetOrdinal("TourId"));
                    item.Status = (EyouSoft.Model.EnumType.TourStructure.TourStatus)rdr.GetByte(rdr.GetOrdinal("Status"));
                    item.OperatorName = rdr["OperatorName"].ToString();
                    item.TourCode = rdr["TourCode"].ToString();

                    EyouSoft.Model.TourStructure.TourOrder orderInfo = this.GetSingleOrTeamOrderInfo(item.TourId);
                    if (orderInfo != null)
                    {
                        item.OrderNo = orderInfo.OrderNo;
                        item.BuyerCName = orderInfo.BuyCompanyName;
                        item.BuyerContacterName = orderInfo.ContactName;
                        item.BuyerContacterTelephone = orderInfo.ContactTel;
                        item.BuyerId = orderInfo.BuyCompanyID;
                    }
                    /*EyouSoft.Model.CompanyStructure.CustomerInfo customerInfo = this.GetCustomerInfo(orderInfo.BuyCompanyID);
                    if (customerInfo != null)
                    {
                        item.BuyerCName = orderInfo.BuyCompanyName;
                        item.BuyerContacterName = customerInfo.ContactName;
                        item.BuyerContacterTelephone = customerInfo.Phone;
                    }*/
                    items.Add(item);
                }
            }

            #region 服务类别
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    StringBuilder s = new StringBuilder();
                    var plans = this.GetPlansSingle(item.TourId);

                    if (plans != null && plans.Count > 0)
                    {
                        s.Append(plans[0].ServiceType.ToString());

                        for (var i = 1; i < plans.Count; i++)
                        {
                            s.AppendFormat(",{0}", plans[i].ServiceType.ToString());
                        }
                    }

                    item.Services = s.ToString();
                }
            }
            #endregion

            return items;
        }

        /// <summary>
        /// 获取待核算计划信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBAccountingTourInfo> GetToursNotAccounting(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info, string us)
        {
            IList<EyouSoft.Model.TourStructure.LBAccountingTourInfo> items = new List<EyouSoft.Model.TourStructure.LBAccountingTourInfo>();
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_Tour";
            string primaryKey = "TourId";
            string orderByString = "IsLeave ASC,LeaveDate ASC";
            StringBuilder fields = new StringBuilder();

            #region fields
            fields.Append("TourId,TourType,LeaveDate,TourCode,Status,IsCostConfirm,IsReview,TicketStatus,RouteName,ReleaseType,TotalIncome,TotalExpenses,DistributionAmount,GrossProfit");
            fields.Append(",(SELECT ContactName FROM tbl_CompanyUser WHERE Id=tbl_Tour.OperatorId ) AS OperatorName ");
            fields.Append(",(SELECT SUM(AdultNumber-LeaguePepoleNum) FROM tbl_TourOrder WHERE TourId=tbl_Tour.TourId AND OrderState NOT IN(3,4) AND IsDelete='0') AS AdultNumber");
            fields.Append(",(SELECT SUM(ChildNumber) FROM tbl_TourOrder WHERE TourId=tbl_Tour.TourId AND OrderState NOT IN(3,4) AND IsDelete='0') AS ChildrenNumber");
            fields.Append(" ,TotalOtherIncome,TotalOtherExpenses ");
            #endregion

            #region 拼接查询条件
            cmdQuery.AppendFormat(" CompanyId={0} ", companyId);
            cmdQuery.Append(" AND IsDelete='0' ");
            cmdQuery.AppendFormat(" AND Status={0} ", (int)EyouSoft.Model.EnumType.TourStructure.TourStatus.财务核算);
            info = info ?? new EyouSoft.Model.TourStructure.TourSearchInfo();
            if (!string.IsNullOrEmpty(us))
            {
                cmdQuery.AppendFormat(" AND OperatorId IN({0}) ", us);
            }
            if (info.EDate.HasValue)
            {
                cmdQuery.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')>=0 ", info.EDate.Value);
            }
            if (info.FDate.HasValue)
            {
                cmdQuery.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')=0 ", info.FDate.Value);
            }
            if (info.OrderStatus.HasValue)
            {
                cmdQuery.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_TourOrder WHERE TourId=tbl_Tour.TourId AND OrderState={0}) ", (int)info.OrderStatus.Value);
            }
            if (!string.IsNullOrEmpty(info.RouteName))
            {
                cmdQuery.AppendFormat(" AND RouteName LIKE '%{0}%' ", info.RouteName);
            }
            if (info.SDate.HasValue)
            {
                cmdQuery.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')<=0 ", info.SDate.Value);
            }
            if (!string.IsNullOrEmpty(info.TourCode))
            {
                cmdQuery.AppendFormat(" AND TourCode LIKE '%{0}%' ", info.TourCode);
            }
            if (info.TourDays.HasValue)
            {
                cmdQuery.AppendFormat(" AND TourDays={0} ", info.TourDays.Value);
            }
            if (info.TourStatus.HasValue)
            {
                cmdQuery.AppendFormat(" AND Status={0} ", (int)info.TourStatus.Value);
            }
            if (info.AreaId.HasValue)
            {
                cmdQuery.AppendFormat(" AND AreaId={0} ", info.AreaId.Value);
            }
            if (!string.IsNullOrEmpty(info.TourId))
            {
                cmdQuery.AppendFormat(" AND TourId='{0}' ", info.TourId);
            }

            if (info.OperatorIds != null && info.OperatorIds.Length > 0)
            {
                cmdQuery.AppendFormat(" AND OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(info.OperatorIds));
            }
            if (info.OperatorDepartIds != null && info.OperatorDepartIds.Length > 0)
            {
                cmdQuery.AppendFormat(" AND OperatorId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByArray(info.OperatorDepartIds));
            }

            if (info.TourType.HasValue)
            {
                cmdQuery.AppendFormat(" AND TourType={0} ", (int)info.TourType.Value);
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.TourStructure.LBAccountingTourInfo item = new EyouSoft.Model.TourStructure.LBAccountingTourInfo();

                    item.AdultNumber = rdr.IsDBNull(rdr.GetOrdinal("AdultNumber")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("AdultNumber"));
                    item.ChildrenNumber = rdr.IsDBNull(rdr.GetOrdinal("ChildrenNumber")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("ChildrenNumber"));
                    item.DistributionAmount = rdr.GetDecimal(rdr.GetOrdinal("DistributionAmount"));
                    item.InComeAmount = rdr.GetDecimal(rdr.GetOrdinal("TotalIncome")) + rdr.GetDecimal(rdr.GetOrdinal("TotalOtherIncome"));
                    item.LDate = rdr.GetDateTime(rdr.GetOrdinal("LeaveDate"));
                    item.OperatorName = rdr["OperatorName"].ToString();
                    item.OutAmount = rdr.GetDecimal(rdr.GetOrdinal("TotalExpenses")) + rdr.GetDecimal(rdr.GetOrdinal("TotalOtherExpenses"));
                    item.ReleaseType = (EyouSoft.Model.EnumType.TourStructure.ReleaseType)int.Parse(rdr.GetString(rdr.GetOrdinal("ReleaseType")));
                    item.RouteName = rdr["RouteName"].ToString();
                    item.TourCode = rdr["TourCode"].ToString();
                    item.TourId = rdr.GetString(rdr.GetOrdinal("TourId"));
                    item.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)rdr.GetByte(rdr.GetOrdinal("TourType"));

                    items.Add(item);
                }
            }

            #region 单项服务处理
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    if (item.TourType != EyouSoft.Model.EnumType.TourStructure.TourType.单项服务) continue;

                    StringBuilder s = new StringBuilder();
                    var plans = this.GetPlansSingle(item.TourId);

                    if (plans != null && plans.Count > 0)
                    {
                        s.Append(plans[0].ServiceType.ToString());

                        for (var i = 1; i < plans.Count; i++)
                        {
                            s.AppendFormat(",{0}", plans[i].ServiceType.ToString());
                        }
                    }

                    item.SingleServices = s.ToString();
                }
            }
            #endregion

            return items;
        }

        /// <summary>
        /// 获取已核算计划信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBAccountingTourInfo> GetToursAccounting(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info, string us)
        {
            IList<EyouSoft.Model.TourStructure.LBAccountingTourInfo> items = new List<EyouSoft.Model.TourStructure.LBAccountingTourInfo>();
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_Tour";
            string primaryKey = "TourId";
            string orderByString = "IsLeave ASC,LeaveDate ASC";
            StringBuilder fields = new StringBuilder();

            #region fields
            fields.Append("TourId,TourType,LeaveDate,TourCode,Status,IsCostConfirm,IsReview,TicketStatus,RouteName,ReleaseType,TotalIncome,TotalExpenses,DistributionAmount,GrossProfit");
            fields.Append(",(SELECT ContactName FROM tbl_CompanyUser WHERE Id=tbl_Tour.OperatorId ) AS OperatorName ");
            fields.Append(",(SELECT SUM(AdultNumber-LeaguePepoleNum) FROM tbl_TourOrder WHERE TourId=tbl_Tour.TourId AND OrderState NOT IN(3,4) AND IsDelete='0') AS AdultNumber");
            fields.Append(",(SELECT SUM(ChildNumber) FROM tbl_TourOrder WHERE TourId=tbl_Tour.TourId AND OrderState NOT IN(3,4) AND IsDelete='0') AS ChildrenNumber");
            fields.Append(" ,TotalOtherIncome,TotalOtherExpenses ");
            #endregion

            #region 拼接查询条件
            cmdQuery.AppendFormat(" CompanyId={0} ", companyId);
            cmdQuery.Append(" AND IsDelete='0' ");
            cmdQuery.AppendFormat(" AND Status={0} ", (int)EyouSoft.Model.EnumType.TourStructure.TourStatus.核算结束);
            info = info ?? new EyouSoft.Model.TourStructure.TourSearchInfo();
            if (!string.IsNullOrEmpty(us))
            {
                cmdQuery.AppendFormat(" AND OperatorId IN({0}) ", us);
            }
            if (info.EDate.HasValue)
            {
                cmdQuery.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')>=0 ", info.EDate.Value);
            }
            if (info.FDate.HasValue)
            {
                cmdQuery.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')=0 ", info.FDate.Value);
            }
            if (info.OrderStatus.HasValue)
            {
                cmdQuery.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_TourOrder WHERE TourId=tbl_Tour.TourId AND OrderState={0}) ", (int)info.OrderStatus.Value);
            }
            if (!string.IsNullOrEmpty(info.RouteName))
            {
                cmdQuery.AppendFormat(" AND RouteName LIKE '%{0}%' ", info.RouteName);
            }
            if (info.SDate.HasValue)
            {
                cmdQuery.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')<=0 ", info.SDate.Value);
            }
            if (!string.IsNullOrEmpty(info.TourCode))
            {
                cmdQuery.AppendFormat(" AND TourCode LIKE '%{0}%' ", info.TourCode);
            }
            if (info.TourDays.HasValue)
            {
                cmdQuery.AppendFormat(" AND TourDays={0} ", info.TourDays.Value);
            }
            if (info.TourStatus.HasValue)
            {
                cmdQuery.AppendFormat(" AND Status={0} ", (int)info.TourStatus.Value);
            }
            if (info.AreaId.HasValue)
            {
                cmdQuery.AppendFormat(" AND AreaId={0} ", info.AreaId.Value);
            }
            if (!string.IsNullOrEmpty(info.TourId))
            {
                cmdQuery.AppendFormat(" AND TourId='{0}' ", info.TourId);
            }
            if (info.OperatorIds != null && info.OperatorIds.Length > 0)
            {
                cmdQuery.AppendFormat(" AND OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(info.OperatorIds));
            }
            if (info.OperatorDepartIds != null && info.OperatorDepartIds.Length > 0)
            {
                cmdQuery.AppendFormat(" AND OperatorId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByArray(info.OperatorDepartIds));
            }
            if (info.TourType.HasValue)
            {
                cmdQuery.AppendFormat(" AND TourType={0} ", (int)info.TourType.Value);
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.TourStructure.LBAccountingTourInfo item = new EyouSoft.Model.TourStructure.LBAccountingTourInfo();

                    item.AdultNumber = rdr.IsDBNull(rdr.GetOrdinal("AdultNumber")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("AdultNumber"));
                    item.ChildrenNumber = rdr.IsDBNull(rdr.GetOrdinal("ChildrenNumber")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("ChildrenNumber"));
                    item.DistributionAmount = rdr.GetDecimal(rdr.GetOrdinal("DistributionAmount"));
                    item.InComeAmount = rdr.GetDecimal(rdr.GetOrdinal("TotalIncome")) + rdr.GetDecimal(rdr.GetOrdinal("TotalOtherIncome"));
                    item.LDate = rdr.GetDateTime(rdr.GetOrdinal("LeaveDate"));
                    item.OperatorName = rdr["OperatorName"].ToString();
                    item.OutAmount = rdr.GetDecimal(rdr.GetOrdinal("TotalExpenses")) + rdr.GetDecimal(rdr.GetOrdinal("TotalOtherExpenses"));
                    item.ReleaseType = (EyouSoft.Model.EnumType.TourStructure.ReleaseType)int.Parse(rdr.GetString(rdr.GetOrdinal("ReleaseType")));
                    item.RouteName = rdr["RouteName"].ToString();
                    item.TourCode = rdr["TourCode"].ToString();
                    item.TourId = rdr.GetString(rdr.GetOrdinal("TourId"));
                    item.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)rdr.GetByte(rdr.GetOrdinal("TourType"));

                    items.Add(item);
                }
            }

            #region 单项服务处理
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    if (item.TourType != EyouSoft.Model.EnumType.TourStructure.TourType.单项服务) continue;

                    StringBuilder s = new StringBuilder();
                    var plans = this.GetPlansSingle(item.TourId);

                    if (plans != null && plans.Count > 0)
                    {
                        s.Append(plans[0].ServiceType.ToString());

                        for (var i = 1; i < plans.Count; i++)
                        {
                            s.AppendFormat(",{0}", plans[i].ServiceType.ToString());
                        }
                    }

                    item.SingleServices = s.ToString();
                }
            }
            #endregion

            return items;
        }

        /// <summary>
        /// 获取计划信息业务实体
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.TourBaseInfo GetTourInfo(string tourId)
        {
            EyouSoft.Model.EnumType.TourStructure.TourType tourType = EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourInfo);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            EyouSoft.Model.TourStructure.TourInfo tourInfo = null;
            EyouSoft.Model.TourStructure.TourTeamInfo teamInfo = null;
            EyouSoft.Model.TourStructure.TourSingleInfo singleInfo = null;
            EyouSoft.Model.TourStructure.TourBaseInfo info = null;

            #region 基本信息
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    tourType = (EyouSoft.Model.EnumType.TourStructure.TourType)rdr.GetByte(rdr.GetOrdinal("TourType"));

                    switch (tourType)
                    {
                        #region 单项服务基本信息
                        case EyouSoft.Model.EnumType.TourStructure.TourType.单项服务:
                            singleInfo = new EyouSoft.Model.TourStructure.TourSingleInfo();
                            singleInfo.AreaId = rdr.GetInt32(rdr.GetOrdinal("AreaId"));
                            singleInfo.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                            singleInfo.CreateTime = rdr.GetDateTime(rdr.GetOrdinal("CreateTime"));
                            singleInfo.DistributionAmount = rdr.GetDecimal(rdr.GetOrdinal("DistributionAmount"));
                            singleInfo.Gather = rdr["Gather"].ToString();
                            singleInfo.GrossProfit = rdr.GetDecimal(rdr.GetOrdinal("GrossProfit"));
                            singleInfo.IsCostConfirm = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsCostConfirm")));
                            singleInfo.IsReview = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsReview")));
                            singleInfo.LDate = rdr.GetDateTime(rdr.GetOrdinal("LeaveDate"));
                            singleInfo.LTraffic = rdr["LTraffic"].ToString();
                            singleInfo.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                            singleInfo.PlanPeopleNumber = rdr.GetInt32(rdr.GetOrdinal("PlanPeopleNumber"));
                            singleInfo.ReleaseType = (EyouSoft.Model.EnumType.TourStructure.ReleaseType)int.Parse(rdr.GetString(rdr.GetOrdinal("ReleaseType")));
                            singleInfo.RouteId = rdr.GetInt32(rdr.GetOrdinal("RouteId"));
                            singleInfo.RouteName = rdr["RouteName"].ToString();
                            singleInfo.RTraffic = rdr["RTraffic"].ToString();
                            singleInfo.Status = (EyouSoft.Model.EnumType.TourStructure.TourStatus)rdr.GetByte(rdr.GetOrdinal("Status"));
                            singleInfo.TicketStatus = (EyouSoft.Model.EnumType.PlanStructure.TicketState)rdr.GetByte(rdr.GetOrdinal("TicketStatus"));
                            singleInfo.TotalExpenses = rdr.GetDecimal(rdr.GetOrdinal("TotalExpenses"));
                            singleInfo.TotalIncome = rdr.GetDecimal(rdr.GetOrdinal("TotalIncome"));
                            singleInfo.TotalOtherExpenses = rdr.GetDecimal(rdr.GetOrdinal("TotalOtherExpenses"));
                            singleInfo.TotalOtherIncome = rdr.GetDecimal(rdr.GetOrdinal("TotalOtherIncome"));
                            singleInfo.TourCode = rdr["TourCode"].ToString();
                            singleInfo.TourDays = rdr.GetInt32(rdr.GetOrdinal("TourDays"));
                            singleInfo.TourId = tourId;
                            singleInfo.TourType = tourType;
                            singleInfo.TotalAmount = rdr.GetDecimal(rdr.GetOrdinal("TotalAmount"));
                            singleInfo.TotalOutAmount = rdr.GetDecimal(rdr.GetOrdinal("TotalOutAmount"));
                            singleInfo.SellerId = rdr.GetInt32(rdr.GetOrdinal("SellerId"));
                            break;
                        #endregion

                        #region 散拼计划基本信息
                        case EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划:
                            tourInfo = new EyouSoft.Model.TourStructure.TourInfo();
                            tourInfo.AreaId = rdr.GetInt32(rdr.GetOrdinal("AreaId"));
                            tourInfo.Attachs = null;
                            tourInfo.Childrens = null;
                            tourInfo.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                            tourInfo.CreateRule = null;
                            tourInfo.CreateTime = rdr.GetDateTime(rdr.GetOrdinal("CreateTime"));
                            tourInfo.DistributionAmount = rdr.GetDecimal(rdr.GetOrdinal("DistributionAmount"));
                            tourInfo.Gather = rdr["Gather"].ToString();
                            tourInfo.GrossProfit = rdr.GetDecimal(rdr.GetOrdinal("GrossProfit"));
                            tourInfo.IsCostConfirm = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsCostConfirm")));
                            tourInfo.IsReview = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsReview")));
                            tourInfo.LDate = rdr.GetDateTime(rdr.GetOrdinal("LeaveDate"));
                            tourInfo.LocalAgencys = null;
                            tourInfo.LTraffic = rdr["LTraffic"].ToString();
                            tourInfo.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                            tourInfo.PlanPeopleNumber = rdr.GetInt32(rdr.GetOrdinal("PlanPeopleNumber"));
                            tourInfo.PriceStandards = null;
                            tourInfo.ReleaseType = (EyouSoft.Model.EnumType.TourStructure.ReleaseType)int.Parse(rdr.GetString(rdr.GetOrdinal("ReleaseType")));
                            tourInfo.RouteId = rdr.GetInt32(rdr.GetOrdinal("RouteId"));
                            tourInfo.RouteName = rdr["RouteName"].ToString();
                            tourInfo.RTraffic = rdr["RTraffic"].ToString();
                            tourInfo.Status = (EyouSoft.Model.EnumType.TourStructure.TourStatus)rdr.GetByte(rdr.GetOrdinal("Status"));
                            tourInfo.TicketStatus = (EyouSoft.Model.EnumType.PlanStructure.TicketState)rdr.GetByte(rdr.GetOrdinal("TicketStatus"));
                            tourInfo.TotalExpenses = rdr.GetDecimal(rdr.GetOrdinal("TotalExpenses"));
                            tourInfo.TotalIncome = rdr.GetDecimal(rdr.GetOrdinal("TotalIncome"));
                            tourInfo.TotalOtherExpenses = rdr.GetDecimal(rdr.GetOrdinal("TotalOtherExpenses"));
                            tourInfo.TotalOtherIncome = rdr.GetDecimal(rdr.GetOrdinal("TotalOtherIncome"));
                            tourInfo.TourCode = rdr["TourCode"].ToString();
                            tourInfo.TourDays = rdr.GetInt32(rdr.GetOrdinal("TourDays"));
                            tourInfo.TourId = tourId;
                            tourInfo.TourNormalInfo = null;
                            tourInfo.TourQuickInfo = null;
                            tourInfo.TourType = tourType;
                            tourInfo.VirtualPeopleNumber = rdr.GetInt32(rdr.GetOrdinal("VirtualPeopleNumber"));
                            tourInfo.GatheringTime = rdr["GatheringTime"].ToString();
                            tourInfo.GatheringPlace = rdr["GatheringPlace"].ToString();
                            tourInfo.GatheringSign = rdr["GatheringSign"].ToString();
                            tourInfo.TourRouteStatus = rdr.IsDBNull(rdr.GetOrdinal("RouteStatus"))
                                                           ? Model.EnumType.TourStructure.TourRouteStatus.无
                                                           : (Model.EnumType.TourStructure.TourRouteStatus)
                                                             rdr.GetByte(rdr.GetOrdinal("RouteStatus"));
                            tourInfo.HandStatus = rdr.IsDBNull(rdr.GetOrdinal("HandStatus"))
                                                           ? Model.EnumType.TourStructure.HandStatus.无
                                                           : (Model.EnumType.TourStructure.HandStatus)
                                                             rdr.GetByte(rdr.GetOrdinal("HandStatus"));

                            break;
                        #endregion

                        #region 团队计划基本信息
                        case EyouSoft.Model.EnumType.TourStructure.TourType.团队计划:
                            teamInfo = new EyouSoft.Model.TourStructure.TourTeamInfo();
                            teamInfo.AreaId = rdr.GetInt32(rdr.GetOrdinal("AreaId"));
                            teamInfo.Attachs = null;
                            teamInfo.CompanyId = rdr.GetInt32(rdr.GetOrdinal("CompanyId"));
                            teamInfo.CreateTime = rdr.GetDateTime(rdr.GetOrdinal("CreateTime"));
                            teamInfo.DistributionAmount = rdr.GetDecimal(rdr.GetOrdinal("DistributionAmount"));
                            teamInfo.Gather = rdr["Gather"].ToString();
                            teamInfo.GrossProfit = rdr.GetDecimal(rdr.GetOrdinal("GrossProfit"));
                            teamInfo.IsCostConfirm = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsCostConfirm")));
                            teamInfo.IsReview = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsReview")));
                            teamInfo.LDate = rdr.GetDateTime(rdr.GetOrdinal("LeaveDate"));
                            teamInfo.LocalAgencys = null;
                            teamInfo.LTraffic = rdr["LTraffic"].ToString();
                            teamInfo.OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId"));
                            teamInfo.PlanPeopleNumber = rdr.GetInt32(rdr.GetOrdinal("PlanPeopleNumber"));
                            teamInfo.ReleaseType = (EyouSoft.Model.EnumType.TourStructure.ReleaseType)int.Parse(rdr.GetString(rdr.GetOrdinal("ReleaseType")));
                            teamInfo.RouteId = rdr.GetInt32(rdr.GetOrdinal("RouteId"));
                            teamInfo.RouteName = rdr["RouteName"].ToString();
                            teamInfo.RTraffic = rdr["RTraffic"].ToString();
                            teamInfo.Status = (EyouSoft.Model.EnumType.TourStructure.TourStatus)rdr.GetByte(rdr.GetOrdinal("Status"));
                            teamInfo.TicketStatus = (EyouSoft.Model.EnumType.PlanStructure.TicketState)rdr.GetByte(rdr.GetOrdinal("TicketStatus"));
                            teamInfo.TotalExpenses = rdr.GetDecimal(rdr.GetOrdinal("TotalExpenses"));
                            teamInfo.TotalIncome = rdr.GetDecimal(rdr.GetOrdinal("TotalIncome"));
                            teamInfo.TotalOtherExpenses = rdr.GetDecimal(rdr.GetOrdinal("TotalOtherExpenses"));
                            teamInfo.TotalOtherIncome = rdr.GetDecimal(rdr.GetOrdinal("TotalOtherIncome"));
                            teamInfo.TourCode = rdr["TourCode"].ToString();
                            teamInfo.TourDays = rdr.GetInt32(rdr.GetOrdinal("TourDays"));
                            teamInfo.TourId = tourId;
                            teamInfo.TourNormalInfo = null;
                            teamInfo.TourQuickInfo = null;
                            teamInfo.TourType = tourType;
                            teamInfo.TotalAmount = rdr.GetDecimal(rdr.GetOrdinal("TotalAmount"));
                            teamInfo.SellerId = rdr.GetInt32(rdr.GetOrdinal("SellerId"));
                            teamInfo.QuoteId = rdr.GetInt32(rdr.GetOrdinal("QuoteId"));
                            teamInfo.GatheringTime = rdr["GatheringTime"].ToString();
                            teamInfo.GatheringPlace = rdr["GatheringPlace"].ToString();
                            teamInfo.GatheringSign = rdr["GatheringSign"].ToString();
                            break;
                        #endregion
                    }
                }
            }
            #endregion

            #region 验证
            if (tourType == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务 && singleInfo == null)
            {
                return null;
            }

            if (tourType == EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划 && tourInfo == null)
            {
                return null;
            }

            if (tourType == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划 && teamInfo == null)
            {
                return null;
            }
            #endregion

            #region 单项服务部分
            if (tourType == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务)
            {
                EyouSoft.Model.TourStructure.TourOrder orderInfo = this.GetSingleOrTeamOrderInfo(tourId);

                singleInfo.Services = this.GetTourSingleServices(tourId);
                singleInfo.Plans = this.GetPlansSingle(tourId);
                if (orderInfo != null)
                {
                    singleInfo.BuyerCId = orderInfo.BuyCompanyID;
                    singleInfo.BuyerCName = orderInfo.BuyCompanyName;
                    singleInfo.ContacterName = orderInfo.ContactName;
                    singleInfo.ContacterTelephone = orderInfo.ContactTel;
                    singleInfo.ContacterMobile = orderInfo.ContactMobile;
                    singleInfo.SellerId = orderInfo.SalerId;
                    singleInfo.SellerName = orderInfo.SalerName;
                    singleInfo.OrderId = orderInfo.ID;
                    singleInfo.CustomerDisplayType = orderInfo.CustomerDisplayType;
                    singleInfo.CustomerFilePath = orderInfo.CustomerFilePath;
                    singleInfo.OrderCode = orderInfo.OrderNo;
                }
                singleInfo.Customers = this.GetSingleTravellers(orderInfo.ID);
            }
            #endregion

            #region 散拼计划部分
            if (tourType == EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划)
            {
                tourInfo.LocalAgencys = this.GetTourLocalAgencys(tourId);
                tourInfo.Attachs = this.GetTourAttachs(tourId);
                tourInfo.PriceStandards = this.GetPriceStandards(tourId);
                tourInfo.PeopleNumberShiShou = this.GetTourPeopleNumberShiShou(tourInfo.TourId);

                if (tourInfo.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick)
                {
                    tourInfo.TourQuickInfo = this.GetTourQuickPrivateInfo(tourId);
                }
                else
                {
                    tourInfo.TourNormalInfo = this.GetTourNormalPrivateInfo(tourId);
                }
                tourInfo.Coordinator = this.GetTourCoordinator(tourInfo.TourId);
                tourInfo.SentPeoples = this.GetTourSentPeoples(tourInfo.TourId);
                tourInfo.TourCityId = this.GetTourCity(tourInfo.TourId);
                tourInfo.TourTraffic = this.GetTourTraffic(tourInfo.TourId);
            }
            #endregion

            #region  团队计划部分
            if (tourType == EyouSoft.Model.EnumType.TourStructure.TourType.团队计划)
            {
                EyouSoft.Model.TourStructure.TourOrder orderInfo = this.GetSingleOrTeamOrderInfo(tourId);

                if (orderInfo != null)
                {
                    teamInfo.BuyerCId = orderInfo.BuyCompanyID;
                    teamInfo.BuyerCName = orderInfo.BuyCompanyName;
                    teamInfo.OrderId = orderInfo.ID;
                }
                teamInfo.Attachs = this.GetTourAttachs(tourId);
                teamInfo.LocalAgencys = this.GetTourLocalAgencys(tourId);
                teamInfo.Services = this.GetTourTeamServices(tourId);

                if (teamInfo.ReleaseType == EyouSoft.Model.EnumType.TourStructure.ReleaseType.Quick)
                {
                    teamInfo.TourQuickInfo = this.GetTourQuickPrivateInfo(tourId);
                }
                else
                {
                    teamInfo.TourNormalInfo = this.GetTourNormalPrivateInfo(tourId);
                }
                teamInfo.Coordinator = this.GetTourCoordinator(teamInfo.TourId);
                teamInfo.SentPeoples = this.GetTourSentPeoples(teamInfo.TourId);
                teamInfo.TourCityId = this.GetTourCity(teamInfo.TourId);
                teamInfo.TourTeamUnit = GetTourTeamUnit(teamInfo.TourId);
                teamInfo.TourTraffic = this.GetTourTraffic(teamInfo.TourId);
            }
            #endregion

            #region 返回
            switch (tourType)
            {
                case EyouSoft.Model.EnumType.TourStructure.TourType.单项服务: info = singleInfo; break;
                case EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划: info = tourInfo; break;
                case EyouSoft.Model.EnumType.TourStructure.TourType.团队计划: info = teamInfo; break;
            }

            return info;
            #endregion
        }

        /// <summary>
        /// 设置团队状态，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="status">状态</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int SetStatus(string tourId, EyouSoft.Model.EnumType.TourStructure.TourStatus status)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_UPDATE_SetStatus);
            this._db.AddInParameter(cmd, "Status", DbType.Byte, status);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 设置团队成本确认状态，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="isCostConfirm">状态</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int SetIsCostConfirm(string tourId, bool isCostConfirm)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_UPDATE_SetIsCostConfirm);
            this._db.AddInParameter(cmd, "IsCostConfirm", DbType.AnsiStringFixedLength, isCostConfirm ? "1" : "0");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 设置团队复核状态，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="isReview">状态</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int SetIsReview(string tourId, bool isReview)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_UPDATE_SetIsReview);
            this._db.AddInParameter(cmd, "IsReview", DbType.AnsiStringFixedLength, isReview ? "1" : "0");
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 设置团队机票状态，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="status">状态</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int SetTourTicketStatus(string tourId, EyouSoft.Model.EnumType.PlanStructure.TicketState status)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_UPDATE_SetTourTicketStatus);
            this._db.AddInParameter(cmd, "Status", DbType.Byte, status);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 设置团队虚拟实收人数，正值时成功，负值或0时失败
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="expression">人数的数值表达式</param>
        /// <returns>正值：成功 负值或0：失败</returns>
        public int SetTourVirtualPeopleNumber(string tourId, int expression)
        {
            int realityPeopleNumber = this.GetTourRealityPeopleNumber(tourId);

            if (expression < realityPeopleNumber) expression = realityPeopleNumber;

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_UPDATE_SetTourVirtualPeopleNumbe);
            this._db.AddInParameter(cmd, "Expression", DbType.Int32, expression);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 删除团队附件信息
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <param name="attachId">附件编号</param>
        /// <returns></returns>
        public bool DeleteTourAttach(string tourId, int attachId)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_DELETE_DeleteTourAttach);
            this._db.AddInParameter(cmd, "AttachId", DbType.Int32, attachId);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /// <summary>
        /// 生成团号
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="ldate">出团日期</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourChildrenInfo> CreateAutoTourCodes(int companyId, params DateTime[] ldates)
        {
            IList<EyouSoft.Model.TourStructure.TourChildrenInfo> items = new List<EyouSoft.Model.TourStructure.TourChildrenInfo>();
            DbCommand cmd = this._db.GetStoredProcCommand("proc_Tour_CreateAutoTourCodes");
            this._db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);
            this._db.AddInParameter(cmd, "LDates", DbType.String, this.CreateAutoTourCodesXML(ldates));

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.TourStructure.TourChildrenInfo()
                    {
                        LDate = rdr.GetDateTime(rdr.GetOrdinal("LDate")),
                        TourCode = rdr["TourCode"].ToString()
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 团号重复验证，返回重复的团号集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="tourId">团队编号 注:新增验证时传string.Empty或null</param>
        /// <param name="tourCodes">团号</param>
        /// <returns></returns>
        public IList<string> ExistsTourCodes(int companyId, string tourId, params string[] tourCodes)
        {
            IList<string> items = new List<string>();
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("SELECT [TourCode] FROM [tbl_Tour] WHERE CompanyId={0} ", companyId);
            if (!string.IsNullOrEmpty(tourId))
            {
                cmdText.AppendFormat(" AND [TourId]<>'{0}' ", tourId);
            }

            if (tourCodes != null && tourCodes.Length > 0)
            {
                cmdText.Append(" AND [TourCode] IN( ");
                cmdText.AppendFormat(" '{0}' ", tourCodes[0]);
                for (int i = 1; i < tourCodes.Length; i++)
                {
                    cmdText.AppendFormat(" ,'{0}' ", tourCodes[0]);
                }
                cmdText.Append(" ) ");
            }

            DbCommand cmd = this._db.GetSqlStringCommand(cmdText.ToString());

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(rdr["TourCode"].ToString());
                }
            }

            return items;
        }

        /// <summary>
        /// 获取计划信息集合(按线路编号)
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="routeId">线路编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourBaseInfo> GetToursByRouteId(int companyId, int pageSize, int pageIndex, ref int recordCount, int routeId)
        {
            IList<EyouSoft.Model.TourStructure.TourBaseInfo> items = new List<EyouSoft.Model.TourStructure.TourBaseInfo>();
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_Tour";
            string primaryKey = "TourId";
            string orderByString = "LeaveDate ASC";
            StringBuilder fields = new StringBuilder();

            #region fields
            fields.Append("TourId,TourCode,LeaveDate,RouteName,TourDays,TotalIncome,TotalExpenses");
            fields.Append(",(SELECT SUM(PeopleNumber-LeaguePepoleNum) FROM [tbl_TourOrder] WHERE TourId=tbl_Tour.TourId AND IsDelete='0' AND OrderState NOT IN(3,4)) AS ShiShou");
            #endregion

            cmdQuery.AppendFormat(" CompanyId={0} ", companyId);
            cmdQuery.AppendFormat(" AND RouteId={0} ", routeId);
            cmdQuery.AppendFormat(" AND TemplateId>'' ");
            cmdQuery.Append(" AND IsDelete='0' ");

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.TourStructure.TourBaseInfo item = new EyouSoft.Model.TourStructure.TourBaseInfo();
                    item.TourId = rdr.GetString(rdr.GetOrdinal("TourId"));
                    item.TourCode = rdr["TourCode"].ToString();
                    item.LDate = rdr.GetDateTime(rdr.GetOrdinal("LeaveDate"));
                    item.RouteName = rdr["RouteName"].ToString();
                    item.TourDays = rdr.GetInt32(rdr.GetOrdinal("TourDays"));
                    item.TotalIncome = rdr.GetDecimal(rdr.GetOrdinal("TotalIncome"));
                    item.TotalExpenses = rdr.GetDecimal(rdr.GetOrdinal("TotalExpenses"));
                    item.PeopleNumberShiShou = rdr.IsDBNull(rdr.GetOrdinal("ShiShou")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("ShiShou"));
                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取散拼计划报价信息集合
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourPriceStandardInfo> GetPriceStandards(string tourId)
        {
            List<EyouSoft.Model.TourStructure.TourPriceStandardInfo> items = new List<EyouSoft.Model.TourStructure.TourPriceStandardInfo>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetPriceStandards);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    //从后向前查找报价等级
                    EyouSoft.Model.TourStructure.TourPriceStandardInfo item = items.FindLast((EyouSoft.Model.TourStructure.TourPriceStandardInfo tmp) =>
                    {
                        if (rdr.GetInt32(rdr.GetOrdinal("Standard")) == tmp.StandardId)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    });

                    item = item ?? new EyouSoft.Model.TourStructure.TourPriceStandardInfo();

                    EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo levelInfo = new EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo();
                    levelInfo.AdultPrice = rdr.GetDecimal(rdr.GetOrdinal("AdultPrice"));
                    levelInfo.ChildrenPrice = rdr.GetDecimal(rdr.GetOrdinal("ChildPrice"));
                    levelInfo.LevelId = rdr.GetInt32(rdr.GetOrdinal("LevelId"));
                    if (rdr.IsDBNull(rdr.GetOrdinal("LevType")))
                    {
                        levelInfo.LevelType = EyouSoft.Model.EnumType.CompanyStructure.CustomLevType.其他;
                    }
                    else
                    {
                        levelInfo.LevelType = (EyouSoft.Model.EnumType.CompanyStructure.CustomLevType)rdr.GetByte(rdr.GetOrdinal("LevType"));
                    }
                    levelInfo.LevelName = rdr["LevName"].ToString();

                    if (item.StandardId == 0)
                    {
                        item.StandardId = rdr.GetInt32(rdr.GetOrdinal("Standard"));
                        item.CustomerLevels = new List<EyouSoft.Model.TourStructure.TourPriceCustomerLevelInfo>();
                        item.CustomerLevels.Add(levelInfo);
                        item.StandardName = rdr["StandardName"].ToString();

                        items.Add(item);
                    }
                    else
                    {
                        item.CustomerLevels.Add(levelInfo);
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// 获取计划列表（组团端）
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="tourDisplayType">团队展示方式</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBZTTours> GetToursZTD(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info, EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType tourDisplayType)
        {
            if (companyId <= 0)
                return null;

            IList<EyouSoft.Model.TourStructure.LBZTTours> list = new List<EyouSoft.Model.TourStructure.LBZTTours>();
            StringBuilder strFiles = new StringBuilder();
            strFiles.Append(" TourId,TourType,ReleaseType,TourCode,RouteName,LeaveDate,PlanPeopleNumber,VirtualPeopleNumber,RouteStatus,HandStatus,(SELECT ContactName,ContactTel,ContactMobile,QQ FROM tbl_CompanyUser AS A WHERE A.Id=tbl_Tour.OperatorId FOR XML RAW,ROOT('root')) AS Contacter ");
            strFiles.Append(",(SELECT SUM(PeopleNumber-LeaguePepoleNum) FROM [tbl_TourOrder] WHERE TourId=tbl_Tour.TourId AND IsDelete='0' AND OrderState IN(1,2,5)) AS ShiShou");
            string strOrder = @"LeaveDate ASC";
            StringBuilder strWhere = new StringBuilder();

            strWhere.AppendFormat(" TemplateId > '' and IsDelete = '0' and LeaveDate > getdate() and TourType = 0 and HandStatus=0 and Status=0 and CompanyId = {0} ", companyId);

            if (tourDisplayType == EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType.子母团)
            {
                strWhere.Append(" AND IsRecently='1' ");
            }

            if (info != null)
            {
                if (info.AreaId.HasValue)
                {
                    strWhere.AppendFormat(" and AreaId = {0} ", info.AreaId);
                }
                else if (info.Areas != null && info.Areas.Length > 0)
                {
                    string strIds = string.Empty;
                    foreach (int i in info.Areas)
                    {
                        if (i <= 0)
                            continue;

                        strIds += i.ToString() + ",";
                    }
                    strIds = strIds.Trim(',');

                    if (!string.IsNullOrEmpty(strIds))
                    {
                        strWhere.AppendFormat(" and AreaId in ({0}) ", strIds);
                    }
                }

                if (info.EDate.HasValue)
                {
                    strWhere.AppendFormat(" and datediff(dd,LeaveDate,'{0}') >= 0 ", info.EDate.Value.ToShortDateString());
                }

                if (info.SDate.HasValue)
                {
                    strWhere.AppendFormat(" and datediff(dd,'{0}',LeaveDate) >= 0 ", info.SDate.Value.ToShortDateString());
                }

                if (!string.IsNullOrEmpty(info.RouteName))
                {
                    strWhere.AppendFormat(" and RouteName like '%{0}%' ", info.RouteName);
                }

                if (!string.IsNullOrEmpty(info.TourId))
                {
                    strWhere.AppendFormat(" AND TourId ='{0}' ", info.TourId);
                }

                if (info.TourCityId.HasValue)
                {
                    strWhere.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_TourCity AS A WHERE A.TourId=tbl_Tour.TourId AND A.CityId={0})", info.TourCityId.Value);
                }
            }

            using (IDataReader dr = DbHelper.ExecuteReader(_db, pageSize, pageIndex, ref recordCount, "tbl_Tour", "TourId"
                , strFiles.ToString(), strWhere.ToString(), strOrder))
            {
                EyouSoft.Model.TourStructure.LBZTTours tModel = null;
                while (dr.Read())
                {
                    tModel = new EyouSoft.Model.TourStructure.LBZTTours();

                    tModel.TourId = dr["TourId"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("ReleaseType")))
                        tModel.ReleaseType = (EyouSoft.Model.EnumType.TourStructure.ReleaseType)int.Parse(dr["ReleaseType"].ToString());
                    tModel.TourCode = dr["TourCode"].ToString();
                    tModel.RouteName = dr["RouteName"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("LeaveDate")))
                        tModel.LDate = dr.GetDateTime(dr.GetOrdinal("LeaveDate"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PlanPeopleNumber")))
                        tModel.PlanPeopleNumber = dr.GetInt32(dr.GetOrdinal("PlanPeopleNumber"));
                    if (!dr.IsDBNull(dr.GetOrdinal("VirtualPeopleNumber")))
                        tModel.VirtualPeopleNumber = dr.GetInt32(dr.GetOrdinal("VirtualPeopleNumber"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ShiShou")))
                        tModel.PeopleNumberShiShou = dr.GetInt32(dr.GetOrdinal("ShiShou"));
                    if (!dr.IsDBNull(dr.GetOrdinal("RouteStatus")))
                        tModel.TourRouteStatus =
                            (Model.EnumType.TourStructure.TourRouteStatus)dr.GetByte(dr.GetOrdinal("RouteStatus"));
                    if (!dr.IsDBNull(dr.GetOrdinal("HandStatus")))
                        tModel.HandStatus =
                            (Model.EnumType.TourStructure.HandStatus)dr.GetByte(dr.GetOrdinal("HandStatus"));

                    if (!dr.IsDBNull(dr.GetOrdinal("Contacter")) && !string.IsNullOrEmpty(dr["Contacter"].ToString()))//发布人信息处理
                    {
                        XElement xRoot = XElement.Parse(dr["Contacter"].ToString());
                        XElement xRow = Utils.GetXElement(xRoot, "row");
                        tModel.ContacterMobile = Utils.GetXAttributeValue(xRow, "ContactMobile");
                        tModel.ContacterName = Utils.GetXAttributeValue(xRow, "ContactName");
                        tModel.ContacterQQ = Utils.GetXAttributeValue(xRow, "QQ");
                        tModel.ContacterTelephone = Utils.GetXAttributeValue(xRow, "ContactTel");
                    }

                    list.Add(tModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取计划列表（供应商交易次数，仅地接）
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="gysId">供应商编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBGYSTours> GetToursGYSDiJie(int companyId, int pageSize, int pageIndex, ref int recordCount, int gysId, EyouSoft.Model.TourStructure.MTimesSummaryDiJieSearchInfo searchInfo)
        {
            if (companyId <= 0)
                return null;

            IList<EyouSoft.Model.TourStructure.LBGYSTours> list = new List<EyouSoft.Model.TourStructure.LBGYSTours>();
            StringBuilder strFiles = new StringBuilder(" TourId,TourType,ReleaseType,TourCode,RouteName,LeaveDate,PlanPeopleNumber ");
            //订单实收
            strFiles.Append(",(select sum(PeopleNumber - LeaguePepoleNum) from tbl_TourOrder as tro where tro.TourId = tbl_tour.TourId and tro.IsDelete = '0' and OrderState not in (3,4)) as RealityPeopleNumber ");
            //团队计调员名称
            strFiles.Append(",(select ContactName from tbl_CompanyUser where tbl_CompanyUser.Id in (select OperatorId from tbl_TourOperator where tbl_TourOperator.TourId = tbl_tour.TourId) for xml auto,root('root')) as PlanNames ");
            //返利、结算金额 --团队、散拼计划地接安排
            strFiles.AppendFormat(",(select sum(Commission) as Commission,sum(TotalAmount) as TotalAmount,SUM(PayAmount) AS PayAmount from tbl_PlanLocalAgency where tbl_PlanLocalAgency.TourId = tbl_tour.TourId AND TravelAgencyID={0} FOR XML RAW,ROOT('root')) as AmountXML ", gysId);
            //单项服务地接安排
            strFiles.AppendFormat(",(SELECT SUM(A.TotalAmount) AS TotalAmount,SUM(A.PaidAmount) AS PayAmount FROM tbl_PlanSingle AS A WHERE A.TourId=tbl_Tour.TourId AND A.SupplierId={0} AND A.ServiceType={1} FOR XML  RAW,ROOT('root')) AS SingleAmountXML", gysId, (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.地接);
            string strOrder = @"LeaveDate ASC";
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" TemplateId > '' and IsDelete = '0' and CompanyId = {0} ", companyId);
            if (gysId > 0)
            {
                strWhere.Append(" AND TourId IN ( ");
                strWhere.AppendFormat(" SELECT A.TourId from tbl_PlanLocalAgency AS A WHERE A.TravelAgencyID = {0} ", gysId);
                if (searchInfo != null && searchInfo.PayStatus.HasValue)
                {
                    switch (searchInfo.PayStatus.Value)
                    {
                        case 1:
                            strWhere.AppendFormat(" AND A.TotalAmount=A.PayAmount ");
                            break;
                        case 2:
                            strWhere.AppendFormat(" AND A.TotalAmount<>A.PayAmount ");
                            break;
                    }
                }
                strWhere.AppendFormat(" UNION ALL SELECT A.TourId FROM tbl_PlanSingle AS A WHERE A.SupplierId={0} ", gysId);
                if (searchInfo != null && searchInfo.PayStatus.HasValue)
                {
                    switch (searchInfo.PayStatus.Value)
                    {
                        case 1:
                            strWhere.AppendFormat(" AND A.TotalAmount=A.PaidAmount ");
                            break;
                        case 2:
                            strWhere.AppendFormat(" AND A.TotalAmount<>A.PaidAmount ");
                            break;
                    }
                }
                strWhere.Append(" ) ");
            }
            if (searchInfo != null)
            {
                if (searchInfo.SDate.HasValue)
                {
                    strWhere.AppendFormat(" AND LeaveDate>='{0}' ", searchInfo.SDate.Value);
                }
                if (searchInfo.EDate.HasValue)
                {
                    strWhere.AppendFormat(" AND LeaveDate<='{0}' ", searchInfo.EDate.Value);
                }
            }

            using (IDataReader dr = DbHelper.ExecuteReader(_db, pageSize, pageIndex, ref recordCount, "tbl_Tour", "TourId"
                , strFiles.ToString(), strWhere.ToString(), strOrder))
            {
                EyouSoft.Model.TourStructure.LBGYSTours tModel = null;
                System.Xml.XmlAttributeCollection attList = null;
                System.Xml.XmlDocument xml = null;
                System.Xml.XmlNodeList xmlNodeList = null;
                while (dr.Read())
                {
                    tModel = new EyouSoft.Model.TourStructure.LBGYSTours();

                    tModel.TourId = dr["TourId"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("TourType")))
                        tModel.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)int.Parse(dr["TourType"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("ReleaseType")))
                        tModel.ReleaseType = (EyouSoft.Model.EnumType.TourStructure.ReleaseType)int.Parse(dr["ReleaseType"].ToString());
                    tModel.TourCode = dr["TourCode"].ToString();
                    tModel.RouteName = dr["RouteName"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("LeaveDate")))
                        tModel.LDate = dr.GetDateTime(dr.GetOrdinal("LeaveDate"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PlanPeopleNumber")))
                        tModel.PlanPeopleNumber = dr.GetInt32(dr.GetOrdinal("PlanPeopleNumber"));
                    if (!dr.IsDBNull(dr.GetOrdinal("RealityPeopleNumber")))
                        tModel.RealityPeopleNumber = dr.GetInt32(dr.GetOrdinal("RealityPeopleNumber"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PlanNames")) && !string.IsNullOrEmpty(dr["PlanNames"].ToString()))
                    {
                        xml = new System.Xml.XmlDocument();
                        xml.LoadXml(dr["PlanNames"].ToString());
                        xmlNodeList = xml.GetElementsByTagName("tbl_CompanyUser");
                        if (xmlNodeList != null && xmlNodeList.Count > 0)
                        {
                            string str = string.Empty;
                            foreach (System.Xml.XmlNode node in xmlNodeList)
                            {
                                attList = node.Attributes;
                                if (attList["ContactName"] != null)
                                {
                                    if (!string.IsNullOrEmpty(attList["ContactName"].Value))
                                        str += attList["ContactName"].Value + ",";
                                }
                            }
                            str = str.Trim(',');
                            tModel.PlanNames = str;
                        }
                    }

                    string amountXML = dr["AmountXML"].ToString();
                    if (!string.IsNullOrEmpty(amountXML) && amountXML != "<root><row/></root>" && tModel.TourType != EyouSoft.Model.EnumType.TourStructure.TourType.单项服务)
                    {
                        XElement xRoot = XElement.Parse(amountXML);
                        XElement xRow = Utils.GetXElement(xRoot, "row");

                        tModel.CommissionAmount = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "Commission"));
                        tModel.SettlementAmount = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "TotalAmount"));
                        tModel.PayAmount = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "PayAmount"));
                    }

                    amountXML = dr["SingleAmountXML"].ToString();
                    if (!string.IsNullOrEmpty(amountXML) && amountXML != "<root><row/></root>" && tModel.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务)
                    {
                        XElement xRoot = XElement.Parse(amountXML);
                        XElement xRow = Utils.GetXElement(xRoot, "row");

                        tModel.SettlementAmount = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "TotalAmount"));
                        tModel.PayAmount = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "PayAmount"));
                    }

                    list.Add(tModel);
                }
            }

            return list;
        }
        /// <summary>
        /// 获取计划列表（供应商交易次数，仅票务）
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="gysId">供应商编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.JPGYSTours> GetToursGYSJiPiao(int companyId, int pageSize, int pageIndex, ref int recordCount, int gysId)
        {
            return GetToursGYSJiPiao(companyId, pageSize, pageIndex, ref recordCount, gysId, null);
        }
        /// <summary>
        /// 获取计划列表（供应商交易次数，仅票务）
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="gysId">供应商编号</param>
        /// <param name="searchInfo">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.JPGYSTours> GetToursGYSJiPiao(int companyId, int pageSize, int pageIndex, ref int recordCount, int gysId, EyouSoft.Model.TourStructure.MTimesSummaryDiJieSearchInfo searchInfo)
        {
            if (companyId <= 0)
                return null;

            IList<EyouSoft.Model.TourStructure.JPGYSTours> list = new List<EyouSoft.Model.TourStructure.JPGYSTours>();
            StringBuilder strFiles = new StringBuilder(" TourId,TourCode,RouteName,LeaveDate,TourType ");
            //团队计调员名称
            strFiles.Append(",(select ContactName from tbl_CompanyUser where tbl_CompanyUser.Id in (select OperatorId from tbl_TourOperator where tbl_TourOperator.TourId = tbl_tour.TourId) for xml auto,root('root')) as PlanNames ");
            //总费用
            strFiles.AppendFormat(",(select sum(TotalAmount) AS TotalAmount,SUM(PayAmount) AS PayAmount from tbl_PlanTicketOut where tbl_PlanTicketOut.TourId = tbl_tour.tourId AND tbl_PlanTicketOut.TicketOfficeId={0} FOR XML RAW,ROOT('root')) as AmountXML ", gysId);
            //单项服务
            strFiles.AppendFormat(",(SELECT SUM(A.TotalAmount) AS TotalAmount,SUM(A.PaidAmount) AS PayAmount FROM tbl_PlanSingle AS A WHERE A.TourId=tbl_Tour.TourId AND A.SupplierId={0} AND A.ServiceType={1} FOR XML RAW,ROOT('root') ) AS SingleAmountXML", gysId, (int)EyouSoft.Model.EnumType.TourStructure.ServiceType.大交通);

            //人数、票款、代理费
            strFiles.AppendFormat(",(select sum(PeopleCount) as PeopleCount,sum(TotalMoney) as TotalMoney,sum(AgencyPrice) as AgencyPrice from tbl_PlanTicketKind where TicketId in (select ID from tbl_PlanTicketOut where tbl_PlanTicketOut.TourId = tbl_tour.tourId AND tbl_PlanTicketOut.TicketOfficeId={0}) for xml auto,root('root')) as TicketKind ", gysId);
            //票号、航段、出票时间
            strFiles.AppendFormat(",(select tbl_PlanTicketOut.TicketNum,tbl_PlanTicketFlight.FligthSegment,convert(varchar(19),tbl_PlanTicketOut.TicketOutTime,120) TicketOutTime from tbl_PlanTicketFlight inner join tbl_PlanTicketOut on tbl_PlanTicketFlight.TicketId = tbl_PlanTicketOut.ID and tbl_PlanTicketOut.TourId = tbl_tour.tourId AND tbl_PlanTicketOut.TicketOfficeId={0} for xml raw,root) as TicketFlight ", gysId);
            string strOrder = @"LeaveDate ASC";
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" TemplateId > '' and IsDelete = '0' and CompanyId = {0} ", companyId);
            if (gysId > 0)
            {
                if (searchInfo != null)
                {
                    strWhere.AppendFormat(" and TourId in (select distinct TourId from tbl_PlanTicketOut AS A where A.TicketOfficeId = {0} ", gysId);
                    if (searchInfo.PayStatus.HasValue)
                    {
                        switch (searchInfo.PayStatus.Value)
                        {
                            case 1:
                                strWhere.AppendFormat(" AND A.TotalAmount=A.PayAmount ");
                                break;
                            case 2:
                                strWhere.AppendFormat(" AND A.TotalAmount<>A.PayAmount ");
                                break;
                        }
                    }
                    strWhere.AppendFormat(" UNION ALL select tourid from tbl_plansingle AS A where A.supplierid={0} ", gysId);
                    if (searchInfo.PayStatus.HasValue)
                    {
                        switch (searchInfo.PayStatus.Value)
                        {
                            case 1:
                                strWhere.AppendFormat(" AND A.TotalAmount=A.PaidAmount ");
                                break;
                            case 2:
                                strWhere.AppendFormat(" AND A.TotalAmount<>A.PaidAmount ");
                                break;
                        }
                    }
                    strWhere.Append(" ) ");
                    if (searchInfo.SDate.HasValue)
                    {
                        strWhere.AppendFormat(" AND LeaveDate>='{0}' ", searchInfo.SDate.Value);
                    }
                    if (searchInfo.EDate.HasValue)
                    {
                        strWhere.AppendFormat(" AND LeaveDate<='{0}' ", searchInfo.EDate.Value);
                    }
                }
                else
                {
                    strWhere.AppendFormat(" and TourId in (select distinct TourId from tbl_PlanTicketOut where tbl_PlanTicketOut.TicketOfficeId = {0} UNION ALL select tourid from tbl_plansingle where supplierid={0}) ", gysId);
                }
            }

            using (IDataReader dr = DbHelper.ExecuteReader(_db, pageSize, pageIndex, ref recordCount, "tbl_Tour", "TourId"
                , strFiles.ToString(), strWhere.ToString(), strOrder))
            {
                EyouSoft.Model.TourStructure.JPGYSTours tModel = null;
                System.Xml.XmlAttributeCollection attList = null;
                System.Xml.XmlDocument xml = null;
                System.Xml.XmlNodeList xmlNodeList = null;
                while (dr.Read())
                {
                    tModel = new EyouSoft.Model.TourStructure.JPGYSTours();

                    EyouSoft.Model.EnumType.TourStructure.TourType tourType = (EyouSoft.Model.EnumType.TourStructure.TourType)dr.GetByte(dr.GetOrdinal("TourType"));

                    tModel.TourId = dr["TourId"].ToString();
                    tModel.TourCode = dr["TourCode"].ToString();
                    tModel.RouteName = dr["RouteName"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("LeaveDate")))
                        tModel.LDate = dr.GetDateTime(dr.GetOrdinal("LeaveDate"));

                    string amountXML = dr["AmountXML"].ToString();
                    if (!string.IsNullOrEmpty(amountXML) && amountXML != "<root><row/></root>" && tourType != EyouSoft.Model.EnumType.TourStructure.TourType.单项服务)
                    {
                        XElement xRoot = XElement.Parse(amountXML);
                        XElement xRow = Utils.GetXElement(xRoot, "row");

                        tModel.TotalAmount = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "TotalAmount"));
                        tModel.PayAmount = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "PayAmount"));
                    }

                    amountXML = dr["SingleAmountXML"].ToString();
                    if (!string.IsNullOrEmpty(amountXML) && amountXML != "<root><row/></root>" && tourType == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务)
                    {
                        XElement xRoot = XElement.Parse(amountXML);
                        XElement xRow = Utils.GetXElement(xRoot, "row");

                        tModel.TotalAmount = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "TotalAmount"));
                        tModel.PayAmount = Utils.GetDecimal(Utils.GetXAttributeValue(xRow, "PayAmount"));
                    }

                    if (!dr.IsDBNull(dr.GetOrdinal("PlanNames")) && !string.IsNullOrEmpty(dr["PlanNames"].ToString()))
                    {
                        xml = new System.Xml.XmlDocument();
                        xml.LoadXml(dr["PlanNames"].ToString());
                        xmlNodeList = xml.GetElementsByTagName("tbl_CompanyUser");
                        if (xmlNodeList != null && xmlNodeList.Count > 0)
                        {
                            string str = string.Empty;
                            foreach (System.Xml.XmlNode node in xmlNodeList)
                            {
                                attList = node.Attributes;
                                if (attList["ContactName"] != null)
                                {
                                    if (!string.IsNullOrEmpty(attList["ContactName"].Value))
                                        str += attList["ContactName"].Value + ",";
                                }
                            }
                            str = str.Trim(',');
                            tModel.PlanNames = str;
                        }
                    }
                    if (!dr.IsDBNull(dr.GetOrdinal("TicketKind")) && !string.IsNullOrEmpty(dr["TicketKind"].ToString()))
                    {
                        xml = new System.Xml.XmlDocument();
                        xml.LoadXml(dr["TicketKind"].ToString());
                        xmlNodeList = xml.GetElementsByTagName("tbl_PlanTicketKind");
                        if (xmlNodeList != null && xmlNodeList.Count > 0)
                        {
                            attList = xmlNodeList[0].Attributes;
                            if (attList["PeopleCount"] != null)
                            {
                                if (!string.IsNullOrEmpty(attList["PeopleCount"].Value))
                                    tModel.PeopleNum = int.Parse(attList["PeopleCount"].Value);
                            }
                            if (attList["TotalMoney"] != null)
                            {
                                if (!string.IsNullOrEmpty(attList["TotalMoney"].Value))
                                    tModel.TicketPrice = decimal.Parse(attList["TotalMoney"].Value);
                            }
                            if (attList["AgencyPrice"] != null)
                            {
                                if (!string.IsNullOrEmpty(attList["AgencyPrice"].Value))
                                    tModel.AgencyPrice = decimal.Parse(attList["AgencyPrice"].Value);
                            }
                        }
                    }

                    // 航班明细
                    if (!dr.IsDBNull(dr.GetOrdinal("TicketFlight")) && !string.IsNullOrEmpty(dr["TicketFlight"].ToString()))
                    {
                        xml = new System.Xml.XmlDocument();
                        xml.LoadXml(dr["TicketFlight"].ToString());
                        xmlNodeList = xml.GetElementsByTagName("row");
                        if (xmlNodeList != null && xmlNodeList.Count > 0)
                        {
                            tModel.TicketFligth = new List<TicketFligth>();
                            foreach (var cls in from object t in xmlNodeList select new TicketFligth())
                            {
                                attList = xmlNodeList[0].Attributes;
                                if (attList["TicketNum"] != null)
                                {
                                    if (!string.IsNullOrEmpty(attList["TicketNum"].Value))
                                        cls.TicketNum = attList["TicketNum"].Value;
                                }
                                if (attList["FligthSegment"] != null)
                                {
                                    if (!string.IsNullOrEmpty(attList["FligthSegment"].Value))
                                        cls.FligthSegment = attList["FligthSegment"].Value;
                                }
                                if (attList["TicketOutTime"] != null)
                                {
                                    if (!string.IsNullOrEmpty(attList["TicketOutTime"].Value))
                                        cls.TicketOutTime = Utils.GetDateTime(attList["TicketOutTime"].Value);
                                }
                                tModel.TicketFligth.Add(cls);
                            }
                        }
                    }

                    list.Add(tModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取计划列表（利润统计团队数）
        /// </summary>
        /// <param name="copanyId">公司编号</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBLYTJTours> GetToursLYTJ(int companyId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.StatisticStructure.QueryEarningsStatistic info, string us)
        {
            if (companyId <= 0)
                return null;

            IList<EyouSoft.Model.TourStructure.LBLYTJTours> list = new List<EyouSoft.Model.TourStructure.LBLYTJTours>();
            string strFiles = @" TourId,TourType,ReleaseType,TourCode,RouteName,leaveDate,TotalAllIncome,TotalAllExpenses,TourPeopleNum,Logistics,Orders,PKID,PlanTickets,PlanAgencys,PlanSingles,ReimburseAmount ";
            string strOrder = @"LeaveDate ASC";
            StringBuilder strWhere = new StringBuilder();
            string strIds = string.Empty;
            strWhere.AppendFormat(" CompanyId = {0} ", companyId);
            if (info.TourType.HasValue)
                strWhere.AppendFormat(" and TourType = {0} ", (int)info.TourType.Value);
            /*if (info.DepartIds != null && info.DepartIds.Length > 0)
            {
                strIds = string.Empty;
                foreach (int i in info.DepartIds)
                {
                    if (i <= 0)
                        continue;

                    strIds += i.ToString() + ",";
                }
                strIds = strIds.Trim(',');

                if (!string.IsNullOrEmpty(strIds))
                    strWhere.AppendFormat(" and DepartId in ({0}) ", strIds);
            }
            if (info.SaleIds != null && info.SaleIds.Length > 0)
            {
                strIds = string.Empty;
                foreach (int i in info.SaleIds)
                {
                    if (i <= 0)
                        continue;

                    strIds += i.ToString() + ",";
                }
                strIds = strIds.Trim(',');
                if (!string.IsNullOrEmpty(strIds))
                    strWhere.AppendFormat(" and TourId in (select distinct tbl_TourOrder.TourId from tbl_TourOrder where SalerId in ({0})) ", strIds);
            }*/
            if (info.DepartIds != null && info.DepartIds.Length > 0)
            {
                strWhere.AppendFormat(" AND DepartId IN({0}) ", Utils.GetSqlIdStrByArray(info.DepartIds));
            }
            if (info.SaleIds != null && info.SaleIds.Length > 0)
            {
                strWhere.AppendFormat(" AND OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(info.SaleIds));
            }

            if (info.LeaveDateStart.HasValue)
                strWhere.AppendFormat(" and datediff(dd,'{0}',LeaveDate) >= 0 ", info.LeaveDateStart.Value.ToShortDateString());
            if (info.LeaveDateEnd.HasValue)
                strWhere.AppendFormat(" and datediff(dd,LeaveDate,'{0}') >= 0 ", info.LeaveDateEnd.Value.ToShortDateString());
            if (info.CheckDateStart.HasValue)
                strWhere.AppendFormat(" and datediff(dd,'{0}',EndDateTime) >= 0 ", info.CheckDateStart.Value.ToShortDateString());
            if (info.CheckDateEnd.HasValue)
                strWhere.AppendFormat(" and datediff(dd,EndDateTime,'{0}') >= 0 ", info.CheckDateEnd.Value.ToShortDateString());
            if (info.AreaId > 0)
                strWhere.AppendFormat(" and AreaId = {0} ", info.AreaId);
            if (info.CurrYear > 0 && info.CurrMonth > 0)
                strWhere.AppendFormat(" AND YEAR(LeaveDate)={0} AND MONTH(LeaveDate) = {1}  ", info.CurrYear, info.CurrMonth);
            if (!string.IsNullOrEmpty(info.TourCode))
                strWhere.AppendFormat(" AND TourCode LIKE '%{0}%' ", info.TourCode);
            if (!string.IsNullOrEmpty(info.RouteName))
                strWhere.AppendFormat(" AND RoteName LIKE '%{0}%' ", info.RouteName);
            if (!string.IsNullOrEmpty(us))
                strWhere.AppendFormat(" AND OperatorId IN({0}) ", us);

            using (IDataReader dr = DbHelper.ExecuteReader(_db, pageSize, pageIndex, ref recordCount
                , "View_EarningsStatistic", "TourId", strFiles, strWhere.ToString(), strOrder))
            {
                EyouSoft.Model.TourStructure.LBLYTJTours tModel = null;
                //System.Xml.XmlAttributeCollection attList = null;
                //System.Xml.XmlDocument xml = null;
                //System.Xml.XmlNodeList xmlNodeList = null;
                while (dr.Read())
                {
                    tModel = new EyouSoft.Model.TourStructure.LBLYTJTours();

                    tModel.TourId = dr["TourId"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("TourType")))
                        tModel.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)int.Parse(dr["TourType"].ToString());
                    if (!dr.IsDBNull(dr.GetOrdinal("ReleaseType")))
                        tModel.ReleaseType = (EyouSoft.Model.EnumType.TourStructure.ReleaseType)int.Parse(dr["ReleaseType"].ToString());
                    tModel.TourCode = dr["TourCode"].ToString();
                    tModel.RouteName = dr["RouteName"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("LeaveDate")))
                        tModel.LDate = dr.GetDateTime(dr.GetOrdinal("LeaveDate"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TotalAllIncome")))
                        tModel.IncomeAmount = dr.GetDecimal(dr.GetOrdinal("TotalAllIncome"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TotalAllExpenses")))
                        tModel.OutAmount = dr.GetDecimal(dr.GetOrdinal("TotalAllExpenses"));
                    if (!dr.IsDBNull(dr.GetOrdinal("TourPeopleNum")))
                        tModel.RealityPeopleNumber = dr.GetInt32(dr.GetOrdinal("TourPeopleNum"));

                    /*if (!dr.IsDBNull(dr.GetOrdinal("Logistics")) && !string.IsNullOrEmpty(dr["Logistics"].ToString()))
                    {
                        xml = new System.Xml.XmlDocument();
                        xml.LoadXml(dr["Logistics"].ToString());
                        xmlNodeList = xml.GetElementsByTagName("tbl_CompanyUser");
                        if (xmlNodeList != null && xmlNodeList.Count > 0)
                        {
                            string str = string.Empty;
                            foreach (System.Xml.XmlNode node in xmlNodeList)
                            {
                                attList = node.Attributes;
                                if (attList["ContactName"] != null)
                                {
                                    if (!string.IsNullOrEmpty(attList["ContactName"].Value))
                                        str += attList["ContactName"].Value + ",";
                                }
                            }
                            str = str.Trim(',');
                            tModel.PlanNames = str;
                        }                        
                    }*/

                    string xml = dr["Logistics"].ToString();
                    if (!string.IsNullOrEmpty(xml))
                    {
                        XElement xroot = XElement.Parse(xml);
                        if (xroot != null)
                        {
                            var xrows = Utils.GetXElements(xroot, "row");
                            StringBuilder names = new StringBuilder();
                            foreach (var xrow in xrows)
                            {
                                names.Append(Utils.GetXAttributeValue(xrow, "ContactName"));
                                names.Append(",");
                            }

                            if (!string.IsNullOrEmpty(names.ToString()))
                            {
                                tModel.PlanNames = names.ToString().Trim(',');
                            }
                        }
                    }

                    tModel.PKID = dr.GetInt32(dr.GetOrdinal("PKID"));
                    tModel.ReimburseAmount = dr.IsDBNull(dr.GetOrdinal("ReimburseAmount")) ? 0 : dr.GetDecimal(dr.GetOrdinal("ReimburseAmount"));
                    tModel.Orders = this.GetLYTJTourOrdersByXml(dr["Orders"].ToString(), info.SaleIds);
                    tModel.PlanTickets = this.GetLYTJTourPlansByXml(dr["PlanTickets"].ToString(), EyouSoft.Model.EnumType.TourStructure.ServiceType.大交通);
                    tModel.PlanAgencys = this.GetLYTJTourPlansByXml(dr["PlanAgencys"].ToString(), EyouSoft.Model.EnumType.TourStructure.ServiceType.地接);
                    tModel.PlanSingles = this.GetLYTJTourPlansByXml(dr["PlanSingles"].ToString(), null);

                    list.Add(tModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取计划列表（组团端首页）
        /// </summary>
        /// <param name="companyId">专线公司编号</param>
        /// <param name="areaId">线路区域编号</param>
        /// <param name="expression">返回指定行数的数值表达式</param>
        /// <param name="tourDisplayType">团队展示方式</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBZTTours> GetToursZTDSY(int companyId, int areaId, int expression, EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType tourDisplayType)
        {
            IList<EyouSoft.Model.TourStructure.LBZTTours> list = new List<EyouSoft.Model.TourStructure.LBZTTours>();
            StringBuilder strSql = new StringBuilder(" select ");
            if (expression > 0)
                strSql.AppendFormat(" top {0} ", expression);
            strSql.AppendFormat(" TourId,TourType,ReleaseType,TourCode,RouteName,LeaveDate,PlanPeopleNumber,VirtualPeopleNumber,(SELECT SUM(PeopleNumber-LeaguePepoleNum) FROM [tbl_TourOrder] WHERE TourId=tbl_Tour.TourId AND IsDelete='0' AND OrderState IN(1,2,5)) AS ShiShou from tbl_Tour where TemplateId > '' and IsDelete = '0' and LeaveDate > getdate() and TourType = 0 and Status=0 and CompanyId = {0} ", companyId);
            if (areaId > 0)
                strSql.AppendFormat(" AND AreaId = {0} ", areaId);

            if (tourDisplayType == EyouSoft.Model.EnumType.CompanyStructure.TourDisplayType.子母团)
            {
                strSql.Append(" AND IsRecently='1' ");
            }

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                EyouSoft.Model.TourStructure.LBZTTours tModel = null;
                while (dr.Read())
                {
                    tModel = new EyouSoft.Model.TourStructure.LBZTTours();

                    tModel.TourId = dr["TourId"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("ReleaseType")))
                        tModel.ReleaseType = (EyouSoft.Model.EnumType.TourStructure.ReleaseType)int.Parse(dr["ReleaseType"].ToString());
                    tModel.TourCode = dr["TourCode"].ToString();
                    tModel.RouteName = dr["RouteName"].ToString();
                    if (!dr.IsDBNull(dr.GetOrdinal("LeaveDate")))
                        tModel.LDate = dr.GetDateTime(dr.GetOrdinal("LeaveDate"));
                    if (!dr.IsDBNull(dr.GetOrdinal("PlanPeopleNumber")))
                        tModel.PlanPeopleNumber = dr.GetInt32(dr.GetOrdinal("PlanPeopleNumber"));
                    if (!dr.IsDBNull(dr.GetOrdinal("VirtualPeopleNumber")))
                        tModel.VirtualPeopleNumber = dr.GetInt32(dr.GetOrdinal("VirtualPeopleNumber"));
                    if (!dr.IsDBNull(dr.GetOrdinal("ShiShou"))) tModel.PeopleNumberShiShou = dr.GetInt32(dr.GetOrdinal("ShiShou"));

                    list.Add(tModel);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取团队复核状态
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <returns></returns>
        public bool GetIsReview(string tourId)
        {
            bool b = false;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetIsReview);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    b = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsReview")));
                }
            }

            return b;
        }

        /// <summary>
        /// 获取团队成本确认状态
        /// </summary>
        /// <param name="tourId">团队编号</param>
        /// <returns></returns>
        public bool GetIsCostConfirm(string tourId)
        {
            bool b = false;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetIsCostConfirm);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    b = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsCostConfirm")));
                }
            }

            return b;
        }

        /// <summary>
        /// 分页获取出团提醒
        /// </summary>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="HaveUserIds">拥有的用户Id集合</param>
        /// <param name="RemindDays">提前X天</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.LeaveTourRemind> GetLeaveTourRemind(int pageSize, int pageIndex, ref int recordCount, int CompanyId, string HaveUserIds, int RemindDays)
        {
            if (CompanyId <= 0 || RemindDays <= 0)
                return null;

            IList<EyouSoft.Model.PersonalCenterStructure.LeaveTourRemind> list = new List<EyouSoft.Model.PersonalCenterStructure.LeaveTourRemind>();
            StringBuilder strFiles = new StringBuilder();
            strFiles.Append(" TourId,RouteName,LeaveDate,TourCode ");
            strFiles.Append(" ,(select sum(PeopleNumber - LeaguePepoleNum) from tbl_TourOrder as tro where tro.TourId = tbl_tour.TourId and tro.IsDelete = '0' and OrderState not in (3,4)) as RealityPeopleNumber ");
            strFiles.Append(" ,(select ContactName from tbl_CompanyUser where tbl_CompanyUser.Id in (select OperatorId from tbl_TourOperator where tbl_TourOperator.TourId = tbl_tour.TourId) and tbl_CompanyUser.IsDelete = '0' for xml auto,root('root')) as PlanNames ");
            string strOrder = @" LeaveDate ASC ";
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" TemplateId > '' and IsDelete = '0' AND DATEDIFF(dd,GETDATE(),LeaveDate)>=0 AND DATEDIFF(dd,GETDATE(),LeaveDate) <= {0} and TourType in (0,1) and CompanyId = {1} ", RemindDays, CompanyId);
            if (!string.IsNullOrEmpty(HaveUserIds))
                strWhere.AppendFormat(" and OperatorId in ({0}) ", HaveUserIds);

            using (IDataReader dr = DbHelper.ExecuteReader(_db, pageSize, pageIndex, ref recordCount, "tbl_Tour", "TourId"
                , strFiles.ToString(), strWhere.ToString(), strOrder))
            {
                EyouSoft.Model.PersonalCenterStructure.LeaveTourRemind tModel = null;
                System.Xml.XmlDocument xml = null;
                System.Xml.XmlNodeList xmlNodeList = null;
                while (dr.Read())
                {
                    tModel = new EyouSoft.Model.PersonalCenterStructure.LeaveTourRemind();

                    if (!dr.IsDBNull(0))
                        tModel.TourId = dr.GetString(0);
                    if (!dr.IsDBNull(1))
                        tModel.RouteName = dr.GetString(1);
                    if (!dr.IsDBNull(2))
                        tModel.LeaveDate = dr.GetDateTime(2);
                    if (!dr.IsDBNull(3))
                        tModel.TourCode = dr.GetString(3);
                    if (!dr.IsDBNull(4))
                        tModel.PeopleCount = dr.GetInt32(4);
                    string strNames = string.Empty;
                    if (!dr.IsDBNull(5))
                    {
                        xml = new System.Xml.XmlDocument();
                        xml.LoadXml(dr.GetString(5));
                        xmlNodeList = xml.GetElementsByTagName("tbl_CompanyUser");
                        if (xmlNodeList != null && xmlNodeList.Count > 0)
                        {
                            foreach (System.Xml.XmlNode t in xmlNodeList)
                            {
                                if (t == null || t.Attributes == null || t.Attributes.Count <= 0)
                                    continue;

                                if (t.Attributes["ContactName"] != null && !string.IsNullOrEmpty(t.Attributes["ContactName"].Value))
                                {
                                    if (!strNames.Contains(t.Attributes["ContactName"].Value))
                                        strNames += t.Attributes["ContactName"].Value + ",";
                                }
                            }
                        }
                    }
                    tModel.JobName = strNames.Trim(',');

                    list.Add(tModel);
                }
            }

            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    item.AgencyInfo = this.GetTourRemindTravelAgencyInfo(item.TourId);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取出团提醒数量
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="HaveUserIds">拥有的用户Id集合</param>
        /// <param name="RemindDays">提前X天</param>
        /// <returns></returns>
        public int GetLeaveTourRemind(int CompanyId, string HaveUserIds, int RemindDays)
        {
            if (CompanyId <= 0 || RemindDays <= 0)
                return 0;

            StringBuilder strSql = new StringBuilder(" select count(*) from tbl_tour ");
            strSql.AppendFormat(" where TemplateId > '' and IsDelete = '0' AND DATEDIFF(dd,GETDATE(),LeaveDate)>=0 AND DATEDIFF(dd,GETDATE(),LeaveDate) <= {0} and TourType in (0,1) and CompanyId = {1} ", RemindDays, CompanyId);
            if (!string.IsNullOrEmpty(HaveUserIds))
                strSql.AppendFormat(" and OperatorId in ({0}) ", HaveUserIds);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            object obj = DbHelper.GetSingle(dc, _db);
            if (obj.Equals(null))
                return 0;
            else
                return int.Parse(obj.ToString());
        }

        /// <summary>
        /// 分页获取回团提醒
        /// </summary>
        /// <param name="pageSize">每页显示数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="HaveUserIds">拥有的用户Id集合</param>
        /// <param name="RemindDays">提前X天</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.PersonalCenterStructure.BackTourRemind> GetBackTourRemind(int pageSize, int pageIndex, ref int recordCount, int CompanyId, string HaveUserIds, int RemindDays)
        {
            if (CompanyId <= 0 || RemindDays <= 0)
                return null;

            IList<EyouSoft.Model.PersonalCenterStructure.BackTourRemind> list = new List<EyouSoft.Model.PersonalCenterStructure.BackTourRemind>();
            StringBuilder strFiles = new StringBuilder();
            strFiles.Append(" TourId,RouteName,RDate,TourCode ");
            strFiles.Append(" ,(select sum(PeopleNumber - LeaguePepoleNum) from tbl_TourOrder as tro where tro.TourId = tbl_tour.TourId and tro.IsDelete = '0' and OrderState not in (3,4)) as RealityPeopleNumber ");
            strFiles.Append(" ,(select ContactName from tbl_CompanyUser where tbl_CompanyUser.Id in (select OperatorId from tbl_TourOperator where tbl_TourOperator.TourId = tbl_tour.TourId) and tbl_CompanyUser.IsDelete = '0' for xml auto,root('root')) as PlanNames ");
            string strOrder = @" LeaveDate ASC ";
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" TemplateId > '' and IsDelete = '0' AND DATEDIFF(dd,GETDATE(),RDate)>=0 AND DATEDIFF(dd,GETDATE(),RDate) <= {0} and TourType in (0,1) and CompanyId = {1} ", RemindDays, CompanyId);
            if (!string.IsNullOrEmpty(HaveUserIds))
                strWhere.AppendFormat(" and OperatorId in ({0}) ", HaveUserIds);

            using (IDataReader dr = DbHelper.ExecuteReader(_db, pageSize, pageIndex, ref recordCount, "tbl_Tour", "TourId"
                , strFiles.ToString(), strWhere.ToString(), strOrder))
            {
                EyouSoft.Model.PersonalCenterStructure.BackTourRemind tModel = null;
                System.Xml.XmlDocument xml = null;
                System.Xml.XmlNodeList xmlNodeList = null;
                while (dr.Read())
                {
                    tModel = new EyouSoft.Model.PersonalCenterStructure.BackTourRemind();

                    if (!dr.IsDBNull(0))
                        tModel.TourId = dr.GetString(0);
                    if (!dr.IsDBNull(1))
                        tModel.RouteName = dr.GetString(1);
                    if (!dr.IsDBNull(2))
                        tModel.BackDate = dr.GetDateTime(2);
                    if (!dr.IsDBNull(3))
                        tModel.TourCode = dr.GetString(3);
                    if (!dr.IsDBNull(4))
                        tModel.PeopleCount = dr.GetInt32(4);
                    string strNames = string.Empty;
                    if (!dr.IsDBNull(5))
                    {
                        xml = new System.Xml.XmlDocument();
                        xml.LoadXml(dr.GetString(5));
                        xmlNodeList = xml.GetElementsByTagName("tbl_CompanyUser");
                        if (xmlNodeList != null && xmlNodeList.Count > 0)
                        {
                            foreach (System.Xml.XmlNode t in xmlNodeList)
                            {
                                if (t == null || t.Attributes == null || t.Attributes.Count <= 0)
                                    continue;

                                if (t.Attributes["ContactName"] != null && !string.IsNullOrEmpty(t.Attributes["ContactName"].Value))
                                {
                                    if (!strNames.Contains(t.Attributes["ContactName"].Value))
                                        strNames += t.Attributes["ContactName"].Value + ",";
                                }
                            }
                        }
                    }
                    tModel.JobName = strNames.Trim(',');

                    list.Add(tModel);
                }
            }

            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    item.AgencyInfo = this.GetTourRemindTravelAgencyInfo(item.TourId);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取回团提醒数量
        /// </summary>
        /// <param name="CompanyId">公司编号</param>
        /// <param name="HaveUserIds">拥有的用户Id集合</param>
        /// <param name="RemindDays">提前X天</param>
        /// <returns></returns>
        public int GetBackTourRemind(int CompanyId, string HaveUserIds, int RemindDays)
        {
            if (CompanyId <= 0 || RemindDays <= 0)
                return 0;

            StringBuilder strSql = new StringBuilder(" select count(*) from tbl_tour ");
            strSql.AppendFormat(" where TemplateId > '' and IsDelete = '0' AND DATEDIFF(dd,GETDATE(),RDate)>=0 AND DATEDIFF(dd,GETDATE(),RDate) <= {0}  and TourType in (0,1) and CompanyId = {1} ", RemindDays, CompanyId);
            if (!string.IsNullOrEmpty(HaveUserIds))
                strSql.AppendFormat(" and OperatorId in ({0}) ", HaveUserIds);

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            object obj = DbHelper.GetSingle(dc, _db);
            if (obj.Equals(null))
                return 0;
            else
                return int.Parse(obj.ToString());
        }

        /// <summary>
        /// 判断计划是否存在支出
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public bool GetIsExistsExpense(string tourId)
        {
            bool b = false;

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetIsExistsExpense);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    b = rdr.GetInt32(0) > 0 ? true : false;
                }
            }

            return b;
        }

        /// <summary>
        /// 获取团款支出信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="isSettleOut">支出是否已结清</param>
        /// <param name="pageSize">页记录数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBTKZCTourInfo> GetToursTKZC(int companyId, bool isSettleOut, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchTKZCInfo info, string us)
        {
            IList<EyouSoft.Model.TourStructure.LBTKZCTourInfo> items = new List<EyouSoft.Model.TourStructure.LBTKZCTourInfo>();
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_Tour";
            string primaryKey = "TourId";
            string orderByString = "LeaveDate ASC";
            StringBuilder fields = new StringBuilder();

            #region fields
            fields.Append("TourId,LeaveDate,TourCode,IsCostConfirm,ReleaseType,RouteName,TourType");
            #endregion

            #region 拼接查询条件
            cmdQuery.AppendFormat(" CompanyId={0} ", companyId);
            cmdQuery.AppendFormat(" AND IsDelete='0' AND TemplateId>'' ");
            cmdQuery.AppendFormat(" AND IsSettleOut='{0}' ", isSettleOut ? "1" : "0");
            info = info ?? new EyouSoft.Model.TourStructure.TourSearchTKZCInfo();

            if (info.EDate.HasValue)
            {
                cmdQuery.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')>=0 ", info.EDate.Value);
            }
            if (info.FDate.HasValue)
            {
                cmdQuery.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')=0 ", info.FDate.Value);
            }
            if (info.PaymentFTime.HasValue || info.PaymentSTime.HasValue || info.PaymentETime.HasValue)
            {
                cmdQuery.Append(" AND EXISTS(SELECT 1 FROM tbl_FinancialPayInfo WHERE ");
                cmdQuery.AppendFormat(" ItemType={0} ", (int)EyouSoft.Model.EnumType.FinanceStructure.OutRegisterType.团队);
                cmdQuery.AppendFormat(" AND ItemId=tbl_Tour.TourId ");
                if (info.PaymentFTime.HasValue)
                    cmdQuery.AppendFormat(" AND DATEDIFF(DAY,PaymentDate,'{0}')=0 ", info.PaymentFTime.Value);
                if (info.PaymentSTime.HasValue)
                    cmdQuery.AppendFormat(" AND PaymentDate>='{0}' ", info.PaymentSTime.Value);
                if (info.PaymentETime.HasValue)
                    cmdQuery.AppendFormat(" AND PaymentDate<='{0}' ", info.PaymentETime.Value);
                cmdQuery.Append(" ) ");
            }
            if (info.SDate.HasValue)
            {
                cmdQuery.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')<=0 ", info.SDate.Value);
            }

            /*if (!string.IsNullOrEmpty(info.SupplierCName)||info.SupplierCType.HasValue)
            {
                cmdQuery.Append(" AND EXISTS(SELECT 1 FROM tbl_StatAllOut WHERE TourId=tbl_Tour.TourId ");
                if (!string.IsNullOrEmpty(info.SupplierCName))
                {
                    cmdQuery.AppendFormat(" AND SupplierName LIKE '%{0}%' ", info.SupplierCName);
                }
                if (info.SupplierCType.HasValue)
                {
                    cmdQuery.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_CompanySupplier WHERE Id=tbl_StatAllOut.SupplierId AND SupplierType={0}) ", (int)info.SupplierCType.Value);
                }
                cmdQuery.Append(" ) ");
            }*/
            cmdQuery.Append(" AND EXISTS(SELECT 1 FROM tbl_StatAllOut WHERE TourId=tbl_Tour.TourId AND IsDelete='0' AND ItemType<>3 ");
            if (!string.IsNullOrEmpty(info.SupplierCName))
            {
                cmdQuery.AppendFormat(" AND SupplierName LIKE '%{0}%' ", info.SupplierCName);
            }
            if (info.SupplierCType.HasValue)
            {
                cmdQuery.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_CompanySupplier WHERE Id=tbl_StatAllOut.SupplierId AND SupplierType={0}) ", (int)info.SupplierCType.Value);
            }
            cmdQuery.Append(" ) ");

            if (!string.IsNullOrEmpty(info.TourCode))
            {
                cmdQuery.AppendFormat(" AND TourCode LIKE '%{0}%' ", info.TourCode);
            }
            if (info.TourType.HasValue)
            {
                cmdQuery.AppendFormat(" AND TourType={0} ", (int)info.TourType.Value);
            }
            if (!string.IsNullOrEmpty(us))
            {
                cmdQuery.AppendFormat(" AND OperatorId IN({0}) ", us);
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    var item = new EyouSoft.Model.TourStructure.LBTKZCTourInfo()
                    {
                        IsCostConfirm = this.GetBoolean(rdr.GetString(rdr.GetOrdinal("IsCostConfirm"))),
                        LDate = rdr.GetDateTime(rdr.GetOrdinal("LeaveDate")),
                        RouteName = rdr["RouteName"].ToString(),
                        TourCode = rdr["TourCode"].ToString(),
                        TourId = rdr.GetString(rdr.GetOrdinal("TourId")),
                        TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)rdr.GetByte(rdr.GetOrdinal("TourType"))
                    };

                    if (item.TourType == EyouSoft.Model.EnumType.TourStructure.TourType.单项服务)
                    {
                        /*var orderinfo = this.GetSingleOrTeamOrderInfo(item.TourId);
                        if (orderinfo != null) { item.TourCode = orderinfo.OrderNo; }
                        orderinfo = null;*/
                        item.RouteName = "单项服务";
                    }

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取单项服务支出信息
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <param name="totalAmount">支出金额</param>
        /// <param name="unpaidAmount">未付金额</param>
        /// <param name="searchInfo">查询信息</param>
        public void GetTourExpense(string tourId, out decimal totalAmount, out decimal unpaidAmount, EyouSoft.Model.PlanStructure.MExpendSearchInfo searchInfo)
        {
            totalAmount = 0;
            unpaidAmount = 0;

            string s = string.Empty;
            if (searchInfo != null)
            {
                if (string.IsNullOrEmpty(searchInfo.SupplierName))
                {
                    s += string.Format(" AND [SupplierName] LIKE '%{0}%' ", searchInfo.SupplierName);
                }
            }

            DbCommand cmd = this._db.GetSqlStringCommand(string.Format(SQL_SELECT_GetTourExpense, s));
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(rdr.GetOrdinal("TotalAmount")))
                    {
                        totalAmount = rdr.GetDecimal(rdr.GetOrdinal("TotalAmount"));
                    }
                    if (!rdr.IsDBNull(rdr.GetOrdinal("PaidAmount")))
                    {
                        unpaidAmount = totalAmount - rdr.GetDecimal(rdr.GetOrdinal("PaidAmount"));
                    }
                }
            }

            return;
        }

        /// <summary>
        /// 获取计划地接社信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourLocalAgencyInfo> GetTourLocalAgencys(string tourId)
        {
            IList<EyouSoft.Model.TourStructure.TourLocalAgencyInfo> items = new List<EyouSoft.Model.TourStructure.TourLocalAgencyInfo>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourLocalAgencys);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.TourStructure.TourLocalAgencyInfo()
                    {
                        AgencyId = rdr.GetInt32(rdr.GetOrdinal("AgencyId")),
                        LicenseNo = rdr["LicenseNo"].ToString(),
                        Name = rdr["Name"].ToString(),
                        Telephone = rdr["Telephone"].ToString(),
                        ContacterName = rdr["ContacterName"].ToString()
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 获取计划计调员信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourCoordinatorInfo> GetTourCoordinators(string tourId)
        {
            IList<EyouSoft.Model.TourStructure.TourCoordinatorInfo> items = new List<EyouSoft.Model.TourStructure.TourCoordinatorInfo>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourCoordinators);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.TourStructure.TourCoordinatorInfo()
                    {
                        CoordinatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                        Name = rdr["ContactName"].ToString(),
                        Telephone = rdr["ContactTel"].ToString(),
                        Mobile = rdr["ContactMobile"].ToString()
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 获取单项服务供应商安排的供应商编号信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public IList<int> GetSingleSuppliers(string tourId)
        {
            IList<int> items = new List<int>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetSingleSuppliers);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(rdr.GetInt32(rdr.GetOrdinal("SupplierId")));
                }
            }

            return items;
        }

        /// <summary>
        /// 获取团队计划及散拼计划供应商编号信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public IList<int> GetTourSuppliers(string tourId)
        {
            IList<int> items = new List<int>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourSuppliers);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(rdr.GetInt32(rdr.GetOrdinal("SupplierId")));
                }
            }

            return items;
        }

        /// <summary>
        /// 获取团队计划、散拼计划、单项服务客户单位信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public IList<int> GetTourCustomers(string tourId)
        {
            IList<int> items = new List<int>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourCustoemrs);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(rdr.GetInt32(rdr.GetOrdinal("BuyCompanyID")));
                }
            }

            return items;
        }

        /// <summary>
        /// 获取计划送团人信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public IList<Model.TourStructure.TourSentPeopleInfo> GetTourSentPeoples(string tourId)
        {
            var items = new List<Model.TourStructure.TourSentPeopleInfo>();

            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourSentPeoples);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(new EyouSoft.Model.TourStructure.TourSentPeopleInfo()
                    {
                        OperatorId = rdr.GetInt32(0),
                        OperatorName = rdr[1].ToString()
                    });
                }
            }

            return items;
        }

        /// <summary>
        /// 设置计划导游及集合标志信息
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <param name="guides">导游信息集合</param>
        /// <param name="gatheringSign">集合标志</param>
        /// <returns></returns>
        public bool SetTourGuides(string tourId, IList<string> guides, string gatheringSign)
        {
            DbCommand cmd = this._db.GetSqlStringCommand("SELECT 1");
            StringBuilder cmdText = new StringBuilder();
            cmdText.Append("UPDATE [tbl_Tour] SET [GatheringSign]=@GatheringSign WHERE [TourId]=@TourId;");
            this._db.AddInParameter(cmd, "GatheringSign", DbType.String, gatheringSign);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            if (guides != null && guides.Count > 0)
            {
                cmdText.Append(" DELETE FROM [tbl_TourSendGuide] WHERE [TourId]=@TourId;  ");

                int i = 0;
                foreach (var guide in guides)
                {
                    cmdText.AppendFormat("INSERT INTO [tbl_TourSendGuide]([TourId],[Guide]) VALUES(@TourId,@Guide{0});", i.ToString());
                    this._db.AddInParameter(cmd, string.Format("Guide{0}", i.ToString()), DbType.String, guide);
                    i++;
                }
            }

            cmd.CommandText = cmdText.ToString();

            return DbHelper.ExecuteSql(cmd, this._db) > 0 ? true : false;
        }

        /*/// <summary>
        /// 根据用户编号获取其运送团的信息
        /// </summary>
        /// <param name="UserID">用户编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourSentTask> GetMySendTourInfo(int pageSize, int pageIndex, ref int recordCount, int[] UserID, EyouSoft.Model.TourStructure.TourSentTaskSearch TourSentTaskSearch)
        {
            string UserIDs = string.Empty;
            foreach (int i in UserID)
            {
                UserIDs += i.ToString() + ",";
            }
            UserIDs = UserIDs.Trim(',');
            return GetMySendTourInfo(pageSize,pageIndex,ref recordCount,UserIDs,TourSentTaskSearch);
        }*/
        /// <summary>
        /// 根据用户编号获取其运送团的信息
        /// </summary>
        /// <param name="UserID">用户编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourSentTask> GetMySendTourInfo(int pageSize, int pageIndex, ref int recordCount, string UserIDs, EyouSoft.Model.TourStructure.TourSentTaskSearch TourSentTaskSearch)
        {
            IList<EyouSoft.Model.TourStructure.TourSentTask> items = new List<EyouSoft.Model.TourStructure.TourSentTask>();
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_Tour";
            string primaryKey = "Id";
            string orderByString = "CreateTime DESC";
            StringBuilder fields = new StringBuilder();
            #region 要查询的字段
            fields.Append("*");
            #endregion
            #region 拼接查询条件

            //cmdQuery.AppendFormat(" TourId in(select TourId from tbl_TourSentTask where OperatorId in({0})) and IsDelete=0", UserIDs);
            cmdQuery.AppendFormat(" 1=1 AND IsDelete='0' AND TourType IN({0},{1}) AND TemplateId>'' ", (int)EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划, (int)EyouSoft.Model.EnumType.TourStructure.TourType.团队计划);
            cmdQuery.AppendFormat(" AND(OperatorId IN({0}) OR EXISTS(SELECT 1 FROM tbl_TourSentTask AS A WHERE A.TourId=tbl_Tour.TourId AND A.OperatorId IN({0}) )) ", UserIDs);
            cmdQuery.AppendFormat(" AND CompanyId={0} ", TourSentTaskSearch.CompanyId);
            if (TourSentTaskSearch != null)
            {
                if (!String.IsNullOrEmpty(TourSentTaskSearch.RouteName))
                {
                    cmdQuery.AppendFormat(" and RouteName like '%{0}%'", TourSentTaskSearch.RouteName);

                }
                if (TourSentTaskSearch.LDate.HasValue)
                {
                    cmdQuery.AppendFormat(" and LeaveDate>='{0}'", TourSentTaskSearch.LDate.Value);
                }
                if (TourSentTaskSearch.LEDate.HasValue)
                {
                    cmdQuery.AppendFormat(" and LeaveDate<='{0}'", TourSentTaskSearch.LEDate.Value.AddDays(1).AddMilliseconds(-1));
                }
            }
            #endregion
            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.TourStructure.TourSentTask item = new EyouSoft.Model.TourStructure.TourSentTask();
                    item.TourId = rdr.GetString(rdr.GetOrdinal("TourId"));
                    item.LDate = rdr.IsDBNull(rdr.GetOrdinal("LeaveDate")) ? System.DateTime.Now : rdr.GetDateTime(rdr.GetOrdinal("LeaveDate"));
                    item.RouteName = rdr["RouteName"].ToString();
                    item.GatheringTime = rdr["GatheringTime"].ToString();
                    item.LTraffic = rdr["LTraffic"].ToString();
                    item.RTraffic = rdr["RTraffic"].ToString();
                    item.PlanPeopleNumber = rdr.IsDBNull(rdr.GetOrdinal("PlanPeopleNumber")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("PlanPeopleNumber"));
                    item.TourCoordinatorInfo = GetTourCoordinators(item.TourId);
                    items.Add(item);
                }
            }
            return items;
        }

        /*/// <summary>
        /// 根据用户编号获取其运送团的信息
        /// </summary>
        /// <param name="UserID">用户编号</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.TourSentTask> GetMySendTourInfo(int[] UserID)
        {
            IList<EyouSoft.Model.TourStructure.TourSentTask> items = new List<EyouSoft.Model.TourStructure.TourSentTask>();
            StringBuilder cmdQuery = new StringBuilder();
            string UserIDs = string.Empty;
            foreach (int i in UserID)
            {
                UserIDs += i.ToString() + ",";
            }
            UserIDs = UserIDs.Trim(',');
            cmdQuery.AppendFormat(" select * from tbl_Tour where TourId in(select TourId from tbl_TourSentTask where OperatorId in({0})) ", UserIDs);
            DbCommand cmd = this._db.GetSqlStringCommand(cmdQuery.ToString());
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.TourStructure.TourSentTask item = new EyouSoft.Model.TourStructure.TourSentTask();
                    item.TourId = rdr.GetString(rdr.GetOrdinal("TourId"));
                    item.LDate = rdr.IsDBNull(rdr.GetOrdinal("LeaveDate")) ? System.DateTime.Now : rdr.GetDateTime(rdr.GetOrdinal("LeaveDate"));
                    item.RouteName = rdr["RouteName"].ToString();
                    item.GatheringTime = rdr["GatheringTime"].ToString();
                    item.LTraffic = rdr["LTraffic"].ToString();
                    item.RTraffic = rdr["RTraffic"].ToString();
                    item.PlanPeopleNumber = rdr.IsDBNull(rdr.GetOrdinal("PlanPeopleNumber")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("PlanPeopleNumber"));
                    item.TourCoordinatorInfo = GetTourCoordinators(item.TourId);
                    items.Add(item);
                }
            }
            return items;
        }*/

        /// <summary>
        /// 获取计划导游信息集合
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public IList<string> GetTourGuides(string tourId)
        {
            IList<string> items = new List<string>();
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourGuides);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                while (rdr.Read())
                {
                    items.Add(rdr[0].ToString());
                }
            }

            return items;
        }

        /// <summary>
        /// 获取个人中心地接安排计划信息集合
        /// </summary>
        /// <param name="companyId">公司(专线)编号</param>
        /// <param name="supplierId">公司(供应商地接)编号</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="info">查询信息</param>
        /// <returns></returns>
        public IList<EyouSoft.Model.TourStructure.LBDJAPTourInfo> GetToursDJAP(int companyId, int supplierId, int pageSize, int pageIndex, ref int recordCount, EyouSoft.Model.TourStructure.TourSearchInfo info)
        {
            IList<EyouSoft.Model.TourStructure.LBDJAPTourInfo> items = new List<EyouSoft.Model.TourStructure.LBDJAPTourInfo>();
            StringBuilder cmdQuery = new StringBuilder();
            string tableName = "tbl_Tour";
            string primaryKey = "TourId";
            string orderByString = "LeaveDate ASC";
            StringBuilder fields = new StringBuilder();

            #region fields
            fields.Append("TourId,RouteName,LeaveDate,TourDays,PlanPeopleNumber,ReleaseType,LTraffic,RTraffic,TourType");
            fields.Append(" ,(SELECT B.ContactName,B.ContactTel,B.ContactMobile,B.QQ FROM tbl_TourOperator AS A INNER JOIN tbl_CompanyUser AS B ON A.OperatorId=B.Id WHERE A.TourId=tbl_Tour.TourId FOR XML RAW,ROOT('root')) AS JDYXML");
            #endregion

            #region 拼接查询条件
            cmdQuery.AppendFormat(" CompanyId={0} ", companyId);
            cmdQuery.Append(" AND IsDelete='0' AND TemplateId>'' ");
            cmdQuery.AppendFormat(" AND TourType IN({0},{1})", (int)EyouSoft.Model.EnumType.TourStructure.TourType.散拼计划, (int)EyouSoft.Model.EnumType.TourStructure.TourType.团队计划);

            cmdQuery.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_PlanLocalAgency AS A WHERE A.TourId=tbl_Tour.TourId AND A.TravelAgencyID={0}) ", supplierId);

            info = info ?? new EyouSoft.Model.TourStructure.TourSearchInfo();
            if (info.EDate.HasValue)
            {
                cmdQuery.AppendFormat(" AND LeaveDate<='{0}' ", info.EDate.Value);
            }
            if (info.FDate.HasValue)
            {
                cmdQuery.AppendFormat(" AND LeaveDate='{0}' ", info.FDate.Value);
            }
            if (!string.IsNullOrEmpty(info.RouteName))
            {
                cmdQuery.AppendFormat(" AND RouteName LIKE '%{0}%' ", info.RouteName);
            }
            if (info.SDate.HasValue)
            {
                cmdQuery.AppendFormat(" AND LeaveDate>='{0}' ", info.SDate.Value);
            }
            if (info.TourDays.HasValue)
            {
                cmdQuery.AppendFormat(" AND TourDays={0} ", info.TourDays.Value);
            }
            #endregion

            #region 排序
            if (info != null && info.SortType.HasValue)
            {
                switch (info.SortType.Value)
                {
                    case EyouSoft.Model.EnumType.TourStructure.TourSortType.出团日期降序:
                        orderByString = "LeaveDate DESC";
                        break;
                    case EyouSoft.Model.EnumType.TourStructure.TourSortType.出团日期升序:
                        orderByString = "LeaveDate ASC";
                        break;
                    case EyouSoft.Model.EnumType.TourStructure.TourSortType.默认:
                        orderByString = "IsLeave ASC,LeaveDate ASC";
                        break;
                    case EyouSoft.Model.EnumType.TourStructure.TourSortType.收客状态降序:
                        orderByString = "Status DESC";
                        break;
                    case EyouSoft.Model.EnumType.TourStructure.TourSortType.收客状态升序:
                        orderByString = "Status ASC";
                        break;
                }
            }
            #endregion

            using (IDataReader rdr = DbHelper.ExecuteReader(this._db, pageSize, pageIndex, ref recordCount, tableName, primaryKey, fields.ToString(), cmdQuery.ToString(), orderByString))
            {
                while (rdr.Read())
                {
                    EyouSoft.Model.TourStructure.LBDJAPTourInfo item = new EyouSoft.Model.TourStructure.LBDJAPTourInfo();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("LeaveDate")))
                        item.LDate = Utils.GetDateTime(rdr["LeaveDate"].ToString());
                    if (!rdr.IsDBNull(rdr.GetOrdinal("LTraffic")))
                        item.LTrafic = rdr["LTraffic"].ToString();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("PlanPeopleNumber")))
                        item.PlanPeopleNumber = rdr.GetInt32(rdr.GetOrdinal("PlanPeopleNumber"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("ReleaseType")))
                        item.ReleaseType = (EyouSoft.Model.EnumType.TourStructure.ReleaseType)int.Parse(rdr.GetString(rdr.GetOrdinal("ReleaseType")));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("RouteName")))
                        item.RouteName = rdr["RouteName"].ToString();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("RTraffic")))
                        item.RTrafic = rdr["RTraffic"].ToString();
                    if (!rdr.IsDBNull(rdr.GetOrdinal("TourDays")))
                        item.TourDays = rdr.GetInt32(rdr.GetOrdinal("TourDays"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("TourId")))
                        item.TourId = rdr.GetString(rdr.GetOrdinal("TourId"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("TourType")))
                        item.TourType = (EyouSoft.Model.EnumType.TourStructure.TourType)rdr.GetByte(rdr.GetOrdinal("TourType"));

                    #region 计划计调员信息
                    string jdyXML = rdr["JDYXML"].ToString();
                    if (!string.IsNullOrEmpty(jdyXML))
                    {
                        XElement xRoot = XElement.Parse(jdyXML);
                        XElement xRow = Utils.GetXElement(xRoot, "row");
                        item.JDYContacterName = Utils.GetXAttributeValue(xRow, "ContactName");
                        item.JDYMobile = Utils.GetXAttributeValue(xRow, "ContactMobile");
                        item.JDYQQ = Utils.GetXAttributeValue(xRow, "QQ");
                        item.JDYTelephone = Utils.GetXAttributeValue(xRow, "ContactTel");
                    }
                    #endregion

                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// 获取线路上团数汇总信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="routeId">线路编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.MRouteSTSCollectInfo GetRouteSTSCollectInfo(int companyId, int routeId)
        {
            EyouSoft.Model.TourStructure.MRouteSTSCollectInfo info = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetRouteSTSCollectInfo);
            this._db.AddInParameter(cmd, "CompanyId", DbType.AnsiStringFixedLength, companyId);
            this._db.AddInParameter(cmd, "RouteId", DbType.Int32, routeId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    info = new EyouSoft.Model.TourStructure.MRouteSTSCollectInfo()
                    {
                        IncomeAmount = rdr.IsDBNull(rdr.GetOrdinal("IncomeAmount")) ? 0 : rdr.GetDecimal(rdr.GetOrdinal("IncomeAmount")),
                        OutAmount = rdr.IsDBNull(rdr.GetOrdinal("OutAmount")) ? 0 : rdr.GetDecimal(rdr.GetOrdinal("OutAmount")),
                        PeopleNumberShiShou = rdr.IsDBNull(rdr.GetOrdinal("ShiShou")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("ShiShou")),
                    };
                }
            }

            return info;
        }

        /// <summary>
        /// 获取线路收客数汇总信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="routeId">线路编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.MRouteSKSCollectInfo GetRouteSKSCollectInfo(int companyId, int routeId)
        {
            EyouSoft.Model.TourStructure.MRouteSKSCollectInfo info = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetRouteSKSCollectInfo);
            this._db.AddInParameter(cmd, "RouteId", DbType.Int32, routeId);


            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    info = new EyouSoft.Model.TourStructure.MRouteSKSCollectInfo()
                    {
                        AdultNumber = rdr.IsDBNull(rdr.GetOrdinal("AdultNumber")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("AdultNumber")),
                        ChildrenNumber = rdr.IsDBNull(rdr.GetOrdinal("ChildrenNumber")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("ChildrenNumber"))
                    };
                }
            }

            return info;
        }

        /// <summary>
        /// 获取单项服务供应商安排信息业务实体
        /// </summary>
        /// <param name="planId">供应商安排编号</param>
        /// <returns></returns>
        public EyouSoft.Model.TourStructure.PlanSingleInfo GetSinglePlanInfo(string planId)
        {
            EyouSoft.Model.TourStructure.PlanSingleInfo info = null;
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetSinglePlanInfo);
            this._db.AddInParameter(cmd, "PlanId", DbType.AnsiStringFixedLength, planId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    info = new EyouSoft.Model.TourStructure.PlanSingleInfo()
                    {
                        AddAmount = rdr.GetDecimal(rdr.GetOrdinal("AddAmount")),
                        Amount = rdr.GetDecimal(rdr.GetOrdinal("Amount")),
                        Arrange = rdr["Arrange"].ToString(),
                        CreateTime = rdr.GetDateTime(rdr.GetOrdinal("CreateTime")),
                        FRemark = rdr["FRemark"].ToString(),
                        OperatorId = rdr.GetInt32(rdr.GetOrdinal("OperatorId")),
                        PlanId = rdr.GetString(rdr.GetOrdinal("PlanId")),
                        ReduceAmount = rdr.GetDecimal(rdr.GetOrdinal("ReduceAmount")),
                        Remark = rdr["Remark"].ToString(),
                        ServiceType = (EyouSoft.Model.EnumType.TourStructure.ServiceType)rdr.GetByte(rdr.GetOrdinal("ServiceType")),
                        SupplierId = rdr.GetInt32(rdr.GetOrdinal("SupplierId")),
                        SupplierName = rdr["SupplierName"].ToString(),
                        TotalAmount = rdr.GetDecimal(rdr.GetOrdinal("TotalAmount")),
                        PaidAmount = rdr.GetDecimal(rdr.GetOrdinal("PaidAmount")),
                    };
                }
            }

            return info;
        }

        /// <summary>
        /// 获取待核算计划信息金额合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <param name="income">总收入</param>
        /// <param name="expenditure">总支出</param>
        /// <param name="payOff">总利润分配</param>
        /// <param name="adultNumber">总成人数</param>
        /// <param name="childNumber">总儿童人数</param>
        public void GetToursNotAccounting(int companyId, Model.TourStructure.TourSearchInfo info, string us
            , ref decimal income, ref decimal expenditure, ref decimal payOff, ref int adultNumber, ref int childNumber)
        {
            var strSql = new StringBuilder();
            strSql.Append(" select sum(AllIncome),sum(AllExpenses),sum(DistributionAmount),sum(AdultNumber),sum(ChildrenNumber) ,sum(GrossProfit) from ");
            strSql.Append("(");
            strSql.Append("select (TotalIncome+TotalOtherIncome) AS AllIncome,(TotalExpenses+TotalOtherExpenses) AS AllExpenses,DistributionAmount,GrossProfit ");
            strSql.Append(",(SELECT SUM(AdultNumber-LeaguePepoleNum) FROM tbl_TourOrder WHERE TourId=tbl_Tour.TourId AND OrderState NOT IN(3,4) AND IsDelete='0') AS AdultNumber ");
            strSql.Append(",(SELECT SUM(ChildNumber) FROM tbl_TourOrder WHERE TourId=tbl_Tour.TourId AND OrderState NOT IN(3,4) AND IsDelete='0') AS ChildrenNumber ");
            strSql.Append(" from tbl_Tour where ");

            #region 拼接查询条件

            strSql.AppendFormat(" CompanyId={0} ", companyId);
            strSql.Append(" AND IsDelete='0' ");
            strSql.AppendFormat(" AND Status={0} ", (int)Model.EnumType.TourStructure.TourStatus.财务核算);
            info = info ?? new Model.TourStructure.TourSearchInfo();
            if (!string.IsNullOrEmpty(us))
            {
                strSql.AppendFormat(" AND OperatorId IN({0}) ", us);
            }
            if (info.EDate.HasValue)
            {
                strSql.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')>=0 ", info.EDate.Value);
            }
            if (info.FDate.HasValue)
            {
                strSql.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')=0 ", info.FDate.Value);
            }
            if (info.OrderStatus.HasValue)
            {
                strSql.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_TourOrder WHERE TourId=tbl_Tour.TourId AND OrderState={0}) ", (int)info.OrderStatus.Value);
            }
            if (!string.IsNullOrEmpty(info.RouteName))
            {
                strSql.AppendFormat(" AND RouteName LIKE '%{0}%' ", info.RouteName);
            }
            if (info.SDate.HasValue)
            {
                strSql.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')<=0 ", info.SDate.Value);
            }
            if (!string.IsNullOrEmpty(info.TourCode))
            {
                strSql.AppendFormat(" AND TourCode LIKE '%{0}%' ", info.TourCode);
            }
            if (info.TourDays.HasValue)
            {
                strSql.AppendFormat(" AND TourDays={0} ", info.TourDays.Value);
            }
            if (info.TourStatus.HasValue)
            {
                strSql.AppendFormat(" AND Status={0} ", (int)info.TourStatus.Value);
            }
            if (info.AreaId.HasValue)
            {
                strSql.AppendFormat(" AND AreaId={0} ", info.AreaId.Value);
            }
            if (!string.IsNullOrEmpty(info.TourId))
            {
                strSql.AppendFormat(" AND TourId='{0}' ", info.TourId);
            }
            if (info.OperatorIds != null && info.OperatorIds.Length > 0)
            {
                strSql.AppendFormat(" AND OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(info.OperatorIds));
            }
            if (info.OperatorDepartIds != null && info.OperatorDepartIds.Length > 0)
            {
                strSql.AppendFormat(" AND OperatorId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByArray(info.OperatorDepartIds));
            }
            if (info.TourType.HasValue)
            {
                strSql.AppendFormat(" AND TourType={0} ", (int)info.TourType.Value);
            }
            #endregion

            strSql.Append(" ) as a ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                        income = dr.GetDecimal(0);
                    if (!dr.IsDBNull(1))
                        expenditure = dr.GetDecimal(1);
                    if (!dr.IsDBNull(2))
                        payOff = dr.GetDecimal(2);
                    if (!dr.IsDBNull(3))
                        adultNumber = dr.GetInt32(3);
                    if (!dr.IsDBNull(4))
                        childNumber = dr.GetInt32(4);
                }
            }
        }

        /// <summary>
        /// 获取已核算计划信息金额合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <param name="income">总收入</param>
        /// <param name="expenditure">总支出</param>
        /// <param name="payOff">总利润分配</param>
        /// <param name="adultNumber">总成人数</param>
        /// <param name="childNumber">总儿童人数</param>
        public void GetToursAccounting(int companyId, Model.TourStructure.TourSearchInfo info, string us
            , ref decimal income, ref decimal expenditure, ref decimal payOff, ref int adultNumber, ref int childNumber)
        {
            var strSql = new StringBuilder();
            strSql.Append(" select sum(AllIncome),sum(AllExpenses),sum(DistributionAmount),sum(AdultNumber),sum(ChildrenNumber) ,sum(GrossProfit) from ");
            strSql.Append(" (");
            strSql.Append("select (TotalIncome+TotalOtherIncome) AS AllIncome,(TotalExpenses+TotalOtherExpenses) AS AllExpenses,DistributionAmount,GrossProfit ");
            strSql.Append(",(SELECT SUM(AdultNumber-LeaguePepoleNum) FROM tbl_TourOrder WHERE TourId=tbl_Tour.TourId AND OrderState NOT IN(3,4) AND IsDelete='0') AS AdultNumber ");
            strSql.Append(",(SELECT SUM(ChildNumber) FROM tbl_TourOrder WHERE TourId=tbl_Tour.TourId AND OrderState NOT IN(3,4) AND IsDelete='0') AS ChildrenNumber ");
            strSql.Append(" from tbl_Tour where ");

            #region 拼接查询条件

            strSql.AppendFormat(" CompanyId={0} ", companyId);
            strSql.Append(" AND IsDelete='0' ");
            strSql.AppendFormat(" AND Status={0} ", (int)Model.EnumType.TourStructure.TourStatus.核算结束);
            info = info ?? new EyouSoft.Model.TourStructure.TourSearchInfo();
            if (!string.IsNullOrEmpty(us))
            {
                strSql.AppendFormat(" AND OperatorId IN({0}) ", us);
            }
            if (info.EDate.HasValue)
            {
                strSql.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')>=0 ", info.EDate.Value);
            }
            if (info.FDate.HasValue)
            {
                strSql.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')=0 ", info.FDate.Value);
            }
            if (info.OrderStatus.HasValue)
            {
                strSql.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_TourOrder WHERE TourId=tbl_Tour.TourId AND OrderState={0}) ", (int)info.OrderStatus.Value);
            }
            if (!string.IsNullOrEmpty(info.RouteName))
            {
                strSql.AppendFormat(" AND RouteName LIKE '%{0}%' ", info.RouteName);
            }
            if (info.SDate.HasValue)
            {
                strSql.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')<=0 ", info.SDate.Value);
            }
            if (!string.IsNullOrEmpty(info.TourCode))
            {
                strSql.AppendFormat(" AND TourCode LIKE '%{0}%' ", info.TourCode);
            }
            if (info.TourDays.HasValue)
            {
                strSql.AppendFormat(" AND TourDays={0} ", info.TourDays.Value);
            }
            if (info.TourStatus.HasValue)
            {
                strSql.AppendFormat(" AND Status={0} ", (int)info.TourStatus.Value);
            }
            if (info.AreaId.HasValue)
            {
                strSql.AppendFormat(" AND AreaId={0} ", info.AreaId.Value);
            }
            if (!string.IsNullOrEmpty(info.TourId))
            {
                strSql.AppendFormat(" AND TourId='{0}' ", info.TourId);
            }
            if (info.OperatorIds != null && info.OperatorIds.Length > 0)
            {
                strSql.AppendFormat(" AND OperatorId IN({0}) ", Utils.GetSqlIdStrByArray(info.OperatorIds));
            }
            if (info.OperatorDepartIds != null && info.OperatorDepartIds.Length > 0)
            {
                strSql.AppendFormat(" AND OperatorId IN(SELECT Id FROM tbl_CompanyUser WHERE DepartId IN({0})) ", Utils.GetSqlIdStrByArray(info.OperatorDepartIds));
            }
            if (info.TourType.HasValue)
            {
                strSql.AppendFormat(" AND TourType={0} ", (int)info.TourType.Value);
            }
            #endregion

            strSql.Append(" ) as a ");

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                        income = dr.GetDecimal(0);
                    if (!dr.IsDBNull(1))
                        expenditure = dr.GetDecimal(1);
                    if (!dr.IsDBNull(2))
                        payOff = dr.GetDecimal(2);
                    if (!dr.IsDBNull(3))
                        adultNumber = dr.GetInt32(3);
                    if (!dr.IsDBNull(4))
                        childNumber = dr.GetInt32(4);
                }
            }
        }

        /// <summary>
        /// 获取团款支出信息集合金额合计
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="isSettleOut">支出是否已结清</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <param name="singleAmount">单项服务支出金额</param>
        /// <param name="singleHasAmount">单项服务支出已付金额</param>
        /// <param name="ticketAmount">机票支出金额</param>
        /// <param name="ticketHasAmount">机票支出已付金额</param>
        /// <param name="travelAgencyAmount">地接支出金额</param>
        /// <param name="travelAgencyHasAmount">地接支出已付金额</param>
        /// <returns></returns>
        public void GetToursTKZC(int companyId, bool isSettleOut, Model.TourStructure.TourSearchTKZCInfo info, string us
                          , ref decimal travelAgencyAmount, ref decimal travelAgencyHasAmount
                          , ref decimal ticketAmount, ref decimal ticketHasAmount
                          , ref decimal singleAmount, ref decimal singleHasAmount)
        {
            var strSql = new StringBuilder();
            var strTourWhere = new StringBuilder();
            var strAgencyWhere = new StringBuilder();

            #region 拼接查询条件

            strTourWhere.AppendFormat(" CompanyId = {0} ", companyId);
            strTourWhere.AppendFormat(" AND IsDelete = '0' AND TemplateId > '' ");
            strTourWhere.AppendFormat(" AND IsSettleOut = '{0}' ", isSettleOut ? "1" : "0");
            info = info ?? new Model.TourStructure.TourSearchTKZCInfo();

            if (info.EDate.HasValue)
                strTourWhere.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')>=0 ", info.EDate.Value);
            if (info.FDate.HasValue)
                strTourWhere.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')=0 ", info.FDate.Value);
            if (info.PaymentFTime.HasValue || info.PaymentSTime.HasValue || info.PaymentETime.HasValue)
            {
                strTourWhere.Append(" AND EXISTS(SELECT 1 FROM tbl_FinancialPayInfo WHERE ");
                strTourWhere.AppendFormat(" ItemType={0} ", (int)Model.EnumType.FinanceStructure.OutRegisterType.团队);
                strTourWhere.AppendFormat(" AND ItemId=tbl_Tour.TourId ");
                if (info.PaymentFTime.HasValue)
                    strTourWhere.AppendFormat(" AND DATEDIFF(DAY,PaymentDate,'{0}')=0 ", info.PaymentFTime.Value);
                if (info.PaymentSTime.HasValue)
                    strTourWhere.AppendFormat(" AND PaymentDate>='{0}' ", info.PaymentSTime.Value);
                if (info.PaymentETime.HasValue)
                    strTourWhere.AppendFormat(" AND PaymentDate<='{0}' ", info.PaymentETime.Value);
                strTourWhere.Append(" ) ");
            }
            if (info.SDate.HasValue)
                strTourWhere.AppendFormat(" AND DATEDIFF(DAY,LeaveDate,'{0}')<=0 ", info.SDate.Value);

            strTourWhere.Append(" AND EXISTS (SELECT 1 FROM tbl_StatAllOut WHERE TourId=tbl_Tour.TourId AND IsDelete='0' AND ItemType<>3 ");
            if (!string.IsNullOrEmpty(info.SupplierCName))
                strTourWhere.AppendFormat(" AND SupplierName LIKE '%{0}%' ", info.SupplierCName);
            if (info.SupplierCType.HasValue)
                strTourWhere.AppendFormat(" AND EXISTS(SELECT 1 FROM tbl_CompanySupplier WHERE Id=tbl_StatAllOut.SupplierId AND SupplierType={0}) ", (int)info.SupplierCType.Value);

            strTourWhere.Append(" ) ");

            if (!string.IsNullOrEmpty(info.TourCode))
                strTourWhere.AppendFormat(" AND TourCode LIKE '%{0}%' ", info.TourCode);
            if (info.TourType.HasValue)
                strTourWhere.AppendFormat(" AND TourType={0} ", (int)info.TourType.Value);
            if (!string.IsNullOrEmpty(us))
                strTourWhere.AppendFormat(" AND OperatorId IN({0}) ", us);

            strAgencyWhere.Append(" AND EXISTS(SELECT 1 FROM tbl_StatAllOut WHERE 1=1 {0} ");
            strAgencyWhere.AppendFormat(" AND CompanyId={0} ", companyId);
            strAgencyWhere.Append(" AND IsDelete='0' ");
            if (!string.IsNullOrEmpty(info.SupplierCName))
            {
                strAgencyWhere.AppendFormat(" AND SupplierName LIKE '%{0}%' ", info.SupplierCName);
            }
            if (info.SupplierCType.HasValue)
            {
                switch (info.SupplierCType.Value)
                {
                    case EyouSoft.Model.EnumType.CompanyStructure.SupplierType.地接:
                        strAgencyWhere.AppendFormat(" AND ItemType IN(1,4) ");
                        break;

                    case EyouSoft.Model.EnumType.CompanyStructure.SupplierType.票务:
                        strAgencyWhere.AppendFormat(" AND ItemType IN(2,4) ");
                        break;
                }
            }
            strAgencyWhere.Append(")");
            #endregion

            //地接
            strSql.Append(" SELECT sum([TotalAmount]),sum([PayAmount]) ");
            strSql.AppendFormat(" FROM [tbl_PlanLocalAgency] where TourId in (select TourId from tbl_Tour where {0}) {1}; "
                                , strTourWhere.ToString(), string.Format(strAgencyWhere.ToString(), " AND tbl_StatAllOut.ItemId=tbl_PlanLocalAgency.Id "));

            //机票
            strSql.Append(" SELECT sum([TotalAmount]),sum([PayAmount]) ");
            strSql.AppendFormat(
                " FROM [tbl_PlanTicketOut] where State = {1} and TourId in (select TourId from tbl_Tour where {0} ) {2}; ",
                strTourWhere.ToString(), (int)Model.EnumType.PlanStructure.TicketState.已出票, string.Format(strAgencyWhere.ToString(), " AND tbl_StatAllOut.ItemId=tbl_PlanTicketOut.Id "));

            //单项服务
            strSql.Append(" SELECT sum([TotalAmount]),sum([PaidAmount]) ");
            strSql.AppendFormat(" FROM [tbl_PlanSingle] where TourId in (select TourId from tbl_Tour where {0} ) {1}; "
                                , strTourWhere.ToString(), string.Format(strAgencyWhere.ToString(), " AND tbl_StatAllOut.ItemId=tbl_PlanSingle.PlanId "));

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());

            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                        travelAgencyAmount = dr.GetDecimal(0);
                    if (!dr.IsDBNull(1))
                        travelAgencyHasAmount = dr.GetDecimal(1);
                }

                dr.NextResult();

                if (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                        ticketAmount = dr.GetDecimal(0);
                    if (!dr.IsDBNull(1))
                        ticketHasAmount = dr.GetDecimal(1);
                }

                dr.NextResult();

                if (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                        singleAmount = dr.GetDecimal(0);
                    if (!dr.IsDBNull(1))
                        singleHasAmount = dr.GetDecimal(1);
                }
            }
        }

        /// <summary>
        /// 获取计划类型
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public EyouSoft.Model.EnumType.TourStructure.TourType? GetTourType(string tourId)
        {
            DbCommand cmd = this._db.GetSqlStringCommand(SQL_SELECT_GetTourType);
            this._db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, this._db))
            {
                if (rdr.Read())
                {
                    return (EyouSoft.Model.EnumType.TourStructure.TourType)rdr.GetByte(0);
                }
            }

            return null;
        }

        /// <summary>
        /// 根据团队Id获取团队计划人数和价格信息
        /// </summary>
        /// <param name="tourId">团队Id</param>
        /// <returns>团队计划人数和价格信息</returns>
        public MTourTeamUnitInfo GetTourTeamUnit(string tourId)
        {
            if (string.IsNullOrEmpty(tourId))
                return null;

            DbCommand dc = _db.GetSqlStringCommand(SQL_SELECT_GetTourTeamUnit);
            _db.AddInParameter(dc, "TourId", DbType.AnsiStringFixedLength, tourId);
            using (IDataReader dr = DbHelper.ExecuteReader(dc, _db))
            {
                if (dr.Read())
                {
                    return new MTourTeamUnitInfo
                               {
                                   NumberCr =
                                       dr.IsDBNull(dr.GetOrdinal("NumberCR"))
                                           ? 0
                                           : dr.GetInt32(dr.GetOrdinal("NumberCR")),
                                   NumberEt =
                                       dr.IsDBNull(dr.GetOrdinal("NumberET"))
                                           ? 0
                                           : dr.GetInt32(dr.GetOrdinal("NumberET")),
                                   NumberQp =
                                       dr.IsDBNull(dr.GetOrdinal("NumberQP"))
                                           ? 0
                                           : dr.GetInt32(dr.GetOrdinal("NumberQP")),
                                   UnitAmountCr =
                                       dr.IsDBNull(dr.GetOrdinal("UnitAmountCR"))
                                           ? 0
                                           : dr.GetDecimal(dr.GetOrdinal("UnitAmountCR")),
                                   UnitAmountEt =
                                       dr.IsDBNull(dr.GetOrdinal("UnitAmountET"))
                                           ? 0
                                           : dr.GetDecimal(dr.GetOrdinal("UnitAmountET")),
                                   UnitAmountQp =
                                       dr.IsDBNull(dr.GetOrdinal("UnitAmountQP"))
                                           ? 0
                                           : dr.GetDecimal(dr.GetOrdinal("UnitAmountQP"))
                               };
                }
            }

            return null;
        }

        /// <summary>
        /// 设置团队推广状态
        /// </summary>
        /// <param name="tourRouteStatus">团队推广状态</param>
        /// <param name="touId">团队Id</param>
        /// <returns>1:成功；其他失败</returns>
        public int SetTourRouteStatus(Model.EnumType.TourStructure.TourRouteStatus tourRouteStatus, params string[] touId)
        {
            if (touId == null || touId.Length <= 0)
                return 0;

            var strSql = new StringBuilder(" UPDATE [tbl_Tour] SET [RouteStatus] = @RouteStatus WHERE ");
            if (touId.Length == 1)
                strSql.AppendFormat(" TourId = '{0}' ", touId[0]);
            else
            {
                string strTourIds = touId.Aggregate(string.Empty, (current, strId) => current + "'" + strId + "'" + ",");
                strTourIds = strTourIds.TrimEnd(',');
                strSql.AppendFormat(" TourId in ({0}) ", strTourIds);
            }

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "RouteStatus", DbType.Byte, (int)tourRouteStatus);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 设置计划的手动设置状态
        /// </summary>
        /// <param name="handStatus">计划的手动设置状态</param>
        /// <param name="touId">团队Id</param>
        /// <returns>1:成功；其他失败</returns>
        public int SetHandStatus(Model.EnumType.TourStructure.HandStatus handStatus, params string[] touId)
        {
            if (touId == null || touId.Length <= 0)
                return 0;

            var strSql = new StringBuilder(" UPDATE [tbl_Tour] SET [HandStatus] = @HandStatus WHERE ");
            if (touId.Length == 1)
                strSql.AppendFormat(" TourId = '{0}' ", touId[0]);
            else
            {
                string strTourIds = touId.Aggregate(string.Empty, (current, strId) => current + "'" + strId + "'" + ",");
                strTourIds = strTourIds.TrimEnd(',');
                strSql.AppendFormat(" TourId in ({0}) ", strTourIds);
            }

            DbCommand dc = _db.GetSqlStringCommand(strSql.ToString());
            _db.AddInParameter(dc, "HandStatus", DbType.Byte, (int)handStatus);

            return DbHelper.ExecuteSql(dc, _db) > 0 ? 1 : 0;
        }

        /// <summary>
        /// 根据子系统公司编号与出团日期，删除计划数据
        /// </summary>
        /// <param name="SysID">子系统公司编号</param>
        /// <param name="LDateStart">出团日期开始</param>
        /// <param name="LDateEnd">出团日期结束</param>
        /// <returns></returns>
        public IList<string> ClearTour(int SysID, DateTime LDateStart, DateTime LDateEnd)
        {
            IList<string> list = new List<string>();
            DbCommand dc = this._db.GetStoredProcCommand("proc_Tour_ClearData");
            this._db.AddInParameter(dc, "CompanyID", DbType.Int32, SysID);
            this._db.AddInParameter(dc, "LDateStart", DbType.DateTime, LDateStart);
            this._db.AddInParameter(dc, "LDateEnd", DbType.DateTime, LDateEnd);
            using (IDataReader dr = DbHelper.ExecuteReader(dc, this._db))
            {
                while (dr.Read())
                {
                    list.Add(dr.GetString(dr.GetOrdinal("TourId")));
                }
            }
            return list;
        }

        /// <summary>
        /// 获取指定计划编号所属母团下子团信息集合
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="tourId">计划编号</param>
        /// <param name="sLDate">出团起始时间</param>
        /// <param name="eLDate">出团截止时间</param>
        /// <returns></returns>
        public IList<MRiLiTourInfo> GetToursRiLi(int companyId, string tourId, DateTime? sLDate, DateTime? eLDate)
        {
            IList<MRiLiTourInfo> items = new List<MRiLiTourInfo>();
            DbCommand cmd = _db.GetSqlStringCommand("SELECT 1");
            StringBuilder cmdText = new StringBuilder();
            cmdText.Append(" SELECT TourId,LeaveDate,PlanPeopleNumber ");
            cmdText.Append(" ,(SELECT SUM(B.PeopleNumber-B.LeaguePepoleNum) FROM [tbl_TourOrder] AS B WHERE B.TourId=A.TourId AND B.IsDelete='0' AND B.OrderState IN(1,2,5)) AS ShiShou ");
            cmdText.Append(" FROM tbl_Tour AS A WHERE A.CompanyId=@CompanyId  ");
            if (sLDate.HasValue)
            {
                cmdText.Append(" AND A.LeaveDate>=@SLDate ");
                _db.AddInParameter(cmd, "SLDate", DbType.DateTime, sLDate.Value);
            }
            if (eLDate.HasValue)
            {
                cmdText.Append(" AND A.LeaveDate<@ELDate ");
                _db.AddInParameter(cmd, "ELDate", DbType.DateTime, eLDate.Value);
            }
            cmdText.Append(" AND A.IsDelete = '0' AND A.LeaveDate > GETDATE() AND A.TourType = 0 AND A.HandStatus=0 and A.Status=0 ");
            cmdText.Append(" AND TemplateId IN(SELECT C.TemplateId FROM tbl_Tour AS C WHERE C.TourId=@TourId) ");


            _db.AddInParameter(cmd, "CompanyId", DbType.Int32, companyId);
            _db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            cmd.CommandText = cmdText.ToString();

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                while (rdr.Read())
                {
                    var item = new MRiLiTourInfo();

                    item.JiaGeCR = 0;
                    item.LDate = rdr.GetDateTime(rdr.GetOrdinal("LeaveDate"));
                    item.RenShuJiHua = rdr.GetInt32(rdr.GetOrdinal("PlanPeopleNumber"));
                    if (!rdr.IsDBNull(rdr.GetOrdinal("ShiShou"))) item.RenShuShiShou = rdr.GetInt32(rdr.GetOrdinal("ShiShou"));
                    item.TourId = rdr.GetString(rdr.GetOrdinal("TourId"));

                    items.Add(item);
                }

            }

            return items;
        }

        /// <summary>
        /// 获取计划剩余人数
        /// </summary>
        /// <param name="tourId">计划编号</param>
        /// <returns></returns>
        public int GetShengYuRenShu(string tourId)
        {
            DbCommand cmd = _db.GetSqlStringCommand(SQL_SELECT_GetShengYuRenShu);
            _db.AddInParameter(cmd, "TourId", DbType.AnsiStringFixedLength, tourId);

            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    if (rdr.IsDBNull(1))
                    {
                        return rdr.GetInt32(0);
                    }

                    return rdr.GetInt32(0) - rdr.GetInt32(1);
                }
            }

            return 0;
        }

        /// <summary>
        /// 获取团队计划列表合计信息
        /// </summary>
        /// <param name="companyId">公司编号</param>
        /// <param name="info">查询信息</param>
        /// <param name="us">用户信息集合</param>
        /// <returns></returns>
        public void GetToursTeamHeJi(int companyId, EyouSoft.Model.TourStructure.TourSearchInfo info, string us, out int renShuHeJi, out decimal tuanKuanJinEHeJi)
        {
            renShuHeJi = 0;
            tuanKuanJinEHeJi = 0;

            StringBuilder cmdText = new StringBuilder();

            #region SQL
            cmdText.Append(" SELECT SUM(TotalIncome) AS TuanKuanJinEHeJi,SUM(RenShu) AS RenShuHeJi ");
            cmdText.Append(" FROM ( ");

            cmdText.AppendFormat("SELECT TotalIncome ,ISNULL((SELECT SUM(A.PeopleNumber-A.LeaguePepoleNum) FROM tbl_TourOrder AS A WHERE A.TourId=tbl_Tour.TourId),0) AS RenShu ");
            cmdText.Append(" FROM tbl_Tour ");
            cmdText.AppendFormat(" WHERE CompanyId={0} ", companyId);
            cmdText.Append(" AND IsDelete='0' ");
            cmdText.AppendFormat(" AND TourType={0} ", (int)EyouSoft.Model.EnumType.TourStructure.TourType.团队计划);
            info = info ?? new EyouSoft.Model.TourStructure.TourSearchInfo();
            if (!string.IsNullOrEmpty(us))
            {
                cmdText.AppendFormat(" AND OperatorId IN({0}) ", us);
            }
            if (info.EDate.HasValue)
            {
                cmdText.AppendFormat(" AND LeaveDate<='{0}' ", info.EDate.Value);
            }
            if (info.FDate.HasValue)
            {
                cmdText.AppendFormat(" AND LeaveDate='{0}' ", info.FDate.Value);
            }
            if (info.OrderStatus.HasValue || (info.Sellers != null && info.Sellers.Length > 0))
            {
                cmdText.Append(" AND EXISTS(SELECT 1 FROM tbl_TourOrder WHERE TourId=tbl_Tour.TourId ");
                if (info.OrderStatus.HasValue)
                {
                    cmdText.AppendFormat(" AND OrderState={0} ", (int)info.OrderStatus.Value);
                }
                if (info.Sellers != null && info.Sellers.Length > 0)
                {
                    cmdText.AppendFormat(" AND SalerId IN({0}) ", Utils.GetSqlIdStrByArray(info.Sellers));
                }
                cmdText.Append(" ) ");
            }
            if (!string.IsNullOrEmpty(info.RouteName))
            {
                cmdText.AppendFormat(" AND RouteName LIKE '%{0}%' ", info.RouteName);
            }
            if (info.SDate.HasValue)
            {
                cmdText.AppendFormat(" AND LeaveDate>='{0}' ", info.SDate.Value);
            }
            if (!string.IsNullOrEmpty(info.TourCode))
            {
                cmdText.AppendFormat(" AND TourCode LIKE '%{0}%' ", info.TourCode);
            }
            if (info.TourDays.HasValue)
            {
                cmdText.AppendFormat(" AND TourDays={0} ", info.TourDays.Value);
            }
            if (info.TourStatus.HasValue)
            {
                cmdText.AppendFormat(" AND Status={0} ", (int)info.TourStatus.Value);
            }
            if (info.AreaId.HasValue)
            {
                cmdText.AppendFormat(" AND AreaId={0} ", info.AreaId.Value);
            }
            if (!string.IsNullOrEmpty(info.TourId))
            {
                cmdText.AppendFormat(" AND TourId='{0}' ", info.TourId);
            }
            if (info.Coordinators != null && info.Coordinators.Length > 0)
            {
                cmdText.AppendFormat(" AND EXISTS (SELECT 1 FROM [tbl_TourOperator] AS A WHERE A.TourId=tbl_Tour.TourId AND A.OperatorId IN({0})) ", Utils.GetSqlIdStrByArray(info.Coordinators));
            }
            if (!string.IsNullOrEmpty(info.YouKeName))
            {
                cmdText.AppendFormat(" AND EXISTS(SELECT 1 FROM [tbl_TourOrderCustomer] AS A WHERE A.TourId=tbl_Tour.TourId AND A.[VisitorName] LIKE '%{0}%') ", info.YouKeName);
            }

            cmdText.Append(" )T ");
            #endregion

            DbCommand cmd = _db.GetSqlStringCommand(cmdText.ToString());
            using (IDataReader rdr = DbHelper.ExecuteReader(cmd, _db))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0)) tuanKuanJinEHeJi = rdr.GetDecimal(0);
                    if (!rdr.IsDBNull(1)) renShuHeJi = rdr.GetInt32(1);
                }
            }
        }

        #endregion
    }
}
