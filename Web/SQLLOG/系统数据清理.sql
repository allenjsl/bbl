--芭比莱系统数据清理，所有删除操作为真删除，执行操作前请备份数据库数据，运行需谨慎
--清理所有业务数据：计划、天天发计划、组团询价、计调安排、账务、现金流量、供应商、客户关系、系统日志等
--汪奇志 2011-05-03

DECLARE @CompanyId INT--公司编号
SET @CompanyId=0

BEGIN TRAN
--计划相关
DELETE FROM tbl_TourOperator WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId)
DELETE FROM tbl_TourLocalAgency WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId)
DELETE FROM tbl_TourPrice WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId)
DELETE FROM tbl_TourTeamPrice WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId)
DELETE FROM tbl_TourQuickPlan WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId)
DELETE FROM tbl_TourPlan WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId)
DELETE FROM tbl_TourCity WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId)
DELETE FROM tbl_GoldDetail WHERE ItemType=6 AND ItemId IN(SELECT PlanId FROM tbl_PlanSingle WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId))
DELETE FROM tbl_PlanSingle WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId)
DELETE FROM tbl_TourSentTask WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId)
DELETE FROM tbl_TourSendGuide WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId)
DELETE FROM tbl_TourService WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId)
DELETE FROM tbl_TourAttach WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId)
DELETE FROM tbl_TourCreateRule WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId)
--订单相关
DELETE FROM tbl_CustomerLeague WHERE CustormerId IN (SELECT Id FROM tbl_TourOrderCustomer WHERE OrderId IN(SELECT Id FROM tbl_TourOrder WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId)))
DELETE FROM tbl_CustomerSpecialService WHERE CustormerId IN (SELECT Id FROM tbl_TourOrderCustomer WHERE OrderId IN(SELECT Id FROM tbl_TourOrder WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId)))
DELETE FROM tbl_CustomerRefundFlight WHERE RefundId IN(SELECT Id FROM tbl_CustomerRefund WHERE CustormerId IN (SELECT Id FROM tbl_TourOrderCustomer WHERE OrderId IN(SELECT Id FROM tbl_TourOrder WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId))))
DELETE FROM tbl_CustomerRefund WHERE CustormerId IN (SELECT Id FROM tbl_TourOrderCustomer WHERE OrderId IN(SELECT Id FROM tbl_TourOrder WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId)))
DELETE FROM tbl_TourOrderCustomer WHERE OrderId IN(SELECT Id FROM tbl_TourOrder WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId))
DELETE FROM tbl_TourOrderPlus WHERE OrderId IN(SELECT Id FROM tbl_TourOrder WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId))
--收退款明细
DELETE FROM tbl_ReceiveRefund WHERE CompanyId=@CompanyId
--收入支出增加减少费用明细信息表
DELETE FROM tbl_GoldDetail WHERE ItemType=1 AND ItemId IN(SELECT Id FROM tbl_TourOrder WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId))
DELETE FROM tbl_TourOrder WHERE TourId IN(SELECT TourId FROM tbl_Tour WHERE CompanyId=@CompanyId)

--计划信息
DELETE FROM tbl_Tour WHERE CompanyId=@CompanyId

--计调安排
DELETE FROM tbl_PlanTicketOutCustomer WHERE TicketOutId IN(SELECT ID FROM tbl_PlanTicketOut WHERE CompanyId=@CompanyId)
DELETE FROM tbl_PlanTicketFlight WHERE TicketId IN(SELECT ID FROM tbl_PlanTicketOut WHERE CompanyId=@CompanyId)
DELETE FROM tbl_PlanTicketKind WHERE TicketId IN(SELECT ID FROM tbl_PlanTicketOut WHERE CompanyId=@CompanyId)
DELETE FROM tbl_PlanTicket WHERE CompanyId=@CompanyId
DELETE FROM tbl_GoldDetail WHERE ItemType=4 AND ItemId IN(SELECT Id FROM tbl_PlanTicketOut WHERE CompanyId=@CompanyId)
DELETE FROM tbl_PlanTicketOut WHERE CompanyId=@CompanyId

DELETE FROM tbl_PlanLocalAgencyPrice WHERE ReferenceID IN(SELECT Id FROM tbl_PlanLocalAgency WHERE CompanyId=@CompanyId)
DELETE FROM tbl_PlanLocalAgencyTourPlan WHERE LocalTravelId IN(SELECT Id FROM tbl_PlanLocalAgency WHERE CompanyId=@CompanyId)
DELETE FROM tbl_GoldDetail WHERE ItemType=3 AND ItemId IN(SELECT Id FROM tbl_PlanLocalAgency WHERE CompanyId=@CompanyId)
DELETE FROM tbl_PlanLocalAgency WHERE CompanyId=@CompanyId

--天天发计划
DELETE FROM tbl_TourEverydayPlan WHERE TourId IN(SELECT TourId FROM tbl_TourEveryday WHERE CompanyId=@CompanyId)
DELETE FROM tbl_TourEverydayQuickPlan WHERE TourId IN(SELECT TourId FROM tbl_TourEveryday WHERE CompanyId=@CompanyId)
DELETE FROM tbl_TourEverydayService WHERE TourId IN(SELECT TourId FROM tbl_TourEveryday WHERE CompanyId=@CompanyId)
DELETE FROM tbl_TourEverydayPrice WHERE TourId IN(SELECT TourId FROM tbl_TourEveryday WHERE CompanyId=@CompanyId)
DELETE FROM tbl_TourEverydayAttach WHERE TourId IN(SELECT TourId FROM tbl_TourEveryday WHERE CompanyId=@CompanyId)
DELETE FROM tbl_TourEverydayApplyTraveller WHERE ApplyId IN(SELECT ApplyId FROM tbl_TourEverydayApply WHERE TourId IN(SELECT TourId FROM tbl_TourEveryday WHERE CompanyId=@CompanyId))
DELETE FROM tbl_TourEverydayApply WHERE TourId IN(SELECT TourId FROM tbl_TourEveryday WHERE CompanyId=@CompanyId)
DELETE FROM tbl_TourEveryday WHERE CompanyId=@CompanyId

--组团询价
DELETE FROM tbl_Quote WHERE CompanyId=@CompanyId
DELETE FROM tbl_CustomerQuoteAsk WHERE QuoteId IN (SELECT Id FROM tbl_CustomerQuote WHERE CompanyId=@CompanyId)
DELETE FROM tbl_CustomerQuoteClaim WHERE QuoteId IN (SELECT Id FROM tbl_CustomerQuote WHERE CompanyId=@CompanyId)
DELETE FROM tbl_CustomerQuotePrice WHERE QuoteId IN (SELECT Id FROM tbl_CustomerQuote WHERE CompanyId=@CompanyId)
DELETE FROM tbl_CustomerQuote WHERE CompanyId=@CompanyId

--杂费收入支出信息
--收入支出增加减少费用明细信息表
DELETE FROM tbl_GoldDetail WHERE ItemType IN(2,5) AND ItemId IN(SELECT Id FROM tbl_TourOtherCost WHERE CompanyId=@CompanyId)
DELETE FROM tbl_TourOtherCost WHERE CompanyId=@CompanyId
--支出登记明细
DELETE FROM tbl_FinancialPayInfo WHERE CompanyId=@CompanyId
--利润分配
DELETE FROM tbl_TourShare WHERE CompanyId=@CompanyId
--出纳收入登账
DELETE FROM tbl_EraseAccount WHERE RegistId IN(SELECT Id FROM tbl_CashierRegister WHERE CompanyId=@CompanyId)
DELETE FROM tbl_CashierRegister WHERE CompanyId=@CompanyId
--出纳支出登账
DELETE FROM tbl_SpendRegisterDetail WHERE RegisterId IN(SELECT RegisterId FROM tbl_SpendRegister WHERE CompanyId=@CompanyId)
DELETE FROM tbl_SpendRegister WHERE CompanyId=@CompanyId

--现金流量 
DELETE FROM tbl_CompanyCash WHERE CompanyId=@CompanyId
DELETE FROM tbl_CompanyCashFlow WHERE CompanyId=@CompanyId

--收入支出信息
DELETE FROM tbl_StatAllIncome WHERE CompanyId=@CompanyId
DELETE FROM tbl_StatAllOut WHERE CompanyId=@CompanyId

--更新供应商交易次数
UPDATE tbl_CompanySupplier SET TradeNum=0 WHERE CompanyId=@CompanyId
--更新线路库团队数及收客数
UPDATE tbl_Route SET TourCount=0,VisitorCount=0 WHERE CompanyId=@CompanyId

--线路库、线路报价信息
DELETE FROM tbl_RouteFile WHERE RouteId IN(SELECT ID FROM tbl_Route WHERE CompanyId=@CompanyId)
DELETE FROM tbl_RouteFastPlan WHERE RouteId IN(SELECT ID FROM tbl_Route WHERE CompanyId=@CompanyId)
DELETE FROM tbl_RouteStandardPlan WHERE RouteId IN(SELECT ID FROM tbl_Route WHERE CompanyId=@CompanyId)
DELETE FROM tbl_RouteService WHERE RouteId IN(SELECT ID FROM tbl_Route WHERE CompanyId=@CompanyId)
--线路报价
DELETE FROM tbl_RouteQuoteAsk WHERE QuoteId IN(SELECT Id FROM tbl_RouteQuote WHERE RouteId IN(SELECT ID FROM tbl_Route WHERE CompanyId=@CompanyId))
DELETE FROM tbl_RouteQuoteList WHERE QuoteId IN(SELECT Id FROM tbl_RouteQuote WHERE RouteId IN(SELECT ID FROM tbl_Route WHERE CompanyId=@CompanyId))
DELETE FROM tbl_RouteQuote WHERE RouteId IN(SELECT ID FROM tbl_Route WHERE CompanyId=@CompanyId)
DELETE FROM tbl_Route WHERE CompanyId=@CompanyId

--供应商信息
DELETE FROM tbl_SupplierContact WHERE SupplierId IN(SELECT Id FROM tbl_CompanySupplier WHERE CompanyId=@CompanyId)
DELETE FROM tbl_SupplierAccessory WHERE SupplierId IN(SELECT Id FROM tbl_CompanySupplier WHERE CompanyId=@CompanyId)
DELETE FROM tbl_SupplierSpot WHERE SupplierId IN(SELECT Id FROM tbl_CompanySupplier WHERE CompanyId=@CompanyId)
DELETE FROM tbl_SupplierCarInfo WHERE PrivaderId IN(SELECT Id FROM tbl_CompanySupplier WHERE CompanyId=@CompanyId)
DELETE FROM tbl_SupplierHotelRoomType WHERE SupplierId IN(SELECT Id FROM tbl_CompanySupplier WHERE CompanyId=@CompanyId)
DELETE FROM tbl_SupplierHotel WHERE SupplierId IN(SELECT Id FROM tbl_CompanySupplier WHERE CompanyId=@CompanyId)
DELETE FROM tbl_SupplierRestaurant WHERE SupplierId IN(SELECT Id FROM tbl_CompanySupplier WHERE CompanyId=@CompanyId)
DELETE FROM tbl_SupplierShopping WHERE Id IN(SELECT Id FROM tbl_CompanySupplier WHERE CompanyId=@CompanyId)
DELETE FROM tbl_CompanySupplier WHERE CompanyId=@CompanyId

--客户关系
--客户回访
DELETE FROM tbl_CustomerCallBackResult WHERE CustomerCareforId IN(SELECT Id FROM tbl_CustomerCallBack WHERE CompanyId=@CompanyId)
DELETE FROM tbl_CustomerCallBack WHERE CompanyId=@CompanyId
--客户营销活动策划
DELETE FROM tbl_CustomerMarketing WHERE CompanyId=@CompanyId
--客户关怀
DELETE FROM tbl_CustomerCarefor WHERE CompanyId=@CompanyId
--留言
DELETE FROM tbl_MessageReply WHERE MessageId IN(SELECT MessageId FROM tbl_Message WHERE CompanyId=@CompanyId)
DELETE FROM tbl_Message WHERE CompanyId=@CompanyId
--客户联系人
DELETE FROM tbl_CustomerContactInfo WHERE CustomerId IN(SELECT Id FROM tbl_Customer WHERE CompanyId=@CompanyId)
DELETE FROM tbl_Customer WHERE CompanyId=@CompanyId

--系统日志
--登录日志
DELETE FROM tbl_SysLoginLog WHERE CompanyId=@CompanyId
--操作日志
DELETE FROM tbl_SysHandleLogs WHERE CompanyId=@CompanyId

COMMIT TRAN
GO

--修改公司管理员账号密码
--明文：000000
--MD5值：670b14728ad9902aecba32e22fa4f6bd
--DECLARE @CompanyId INT
--SET @CompanyId=0
--UPDATE tbl_CompanyUser SET [Password]='000000',MD5Password='670b14728ad9902aecba32e22fa4f6bd' WHERE IsAdmin='1' AND CompanyId=@CompanyId
--GO
