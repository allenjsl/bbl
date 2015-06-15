﻿
-- 2012-09-07  开始
if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tbl_TrafficPrices') and o.name = 'FK_TBL_TRAFFICPRICES_REFERENCE_TBL_TRAFFIC')
alter table tbl_TrafficPrices
   drop constraint FK_TBL_TRAFFICPRICES_REFERENCE_TBL_TRAFFIC
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tbl_Travel') and o.name = 'FK_TBL_TRAVEL_REFERENCE_TBL_TRAFFIC')
alter table tbl_Travel
   drop constraint FK_TBL_TRAVEL_REFERENCE_TBL_TRAFFIC
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tbl_Traffic')
            and   type = 'U')
   drop table tbl_Traffic
go

/*==============================================================*/
/* Table: tbl_Traffic                                           */
/*==============================================================*/
create table tbl_Traffic (
   TrafficId            int                  identity,
   TrafficName          nvarchar(255)        not null,
   TrafficDays          int                  not null default 0,
   ChildPrices          money                not null default 0,
   Status               tinyint              not null default 0,
   CompanyId            int                  not null,
   Operater             nvarchar(255)        not null,
   OperaterId           int                  not null default 0,
   IsDelete             char(1)              not null default '0',
   InsueTime            datetime             not null default getdate(),
   constraint PK_TBL_TRAFFIC primary key (TrafficId)
)
go




if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tbl_TrafficPrices') and o.name = 'FK_TBL_TRAFFICPRICES_REFERENCE_TBL_TRAFFIC')
alter table tbl_TrafficPrices
   drop constraint FK_TBL_TRAFFICPRICES_REFERENCE_TBL_TRAFFIC
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tbl_TrafficPrices')
            and   type = 'U')
   drop table tbl_TrafficPrices
go

/*==============================================================*/
/* Table: tbl_TrafficPrices                                     */
/* 交通管理价格明细表											*/
/*==============================================================*/
create table tbl_TrafficPrices (
   PricesID             char(36)             not null,
   TrafficId            int                  null,
   SDateTime            datetime             not null default getdate(),
   TicketPrices         money                not null default 0,
   TicketNums           int                  not null default 0,
   Status               tinyint              not null default 0,
   InsueTime            datetime             not null default getdate(),
   constraint PK_TBL_TRAFFICPRICES primary key (PricesID)
)
go

alter table tbl_TrafficPrices
   add constraint FK_TBL_TRAFFICPRICES_REFERENCE_TBL_TRAFFIC foreign key (TrafficId)
      references tbl_Traffic (TrafficId)
go



if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tbl_Travel') and o.name = 'FK_TBL_TRAVEL_REFERENCE_TBL_TRAFFIC')
alter table tbl_Travel
   drop constraint FK_TBL_TRAVEL_REFERENCE_TBL_TRAFFIC
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tbl_Travel')
            and   type = 'U')
   drop table tbl_Travel
go

/*==============================================================*/
/* Table: tbl_Travel                                            */
/*==============================================================*/
create table tbl_Travel (
   TravelId             int                  identity,
   TrafficId            int                  null,
   SerialNum            int                  not null default 0,
   TrafficType          tinyint              not null default 0,
   LProvince            int                  not null default 0,
   LCity                int                  not null default 0,
   RProvince            int                  not null default 0,
   RCity                int                  not null default 0,
   FilghtNum            nvarchar(255)        not null,
   FlightCompany        tinyint              not null default 0,
   LTime                nvarchar(255)        not null,
   RTime                nvarchar(255)        not null,
   IsStop               char(1)              not null,
   Space                tinyint              not null,
   AirPlaneType         nvarchar(255)        null,
   IntervalDays         int                  not null,
   CompanyId            int                  not null default 0,
   Operater             nvarchar(255)        not null,
   OperaterID           int                  not null default 0,
   InsueTime            datetime             null default getdate(),
   constraint PK_TBL_TRAVEL primary key (TravelId)
)
go

alter table tbl_Travel
   add constraint FK_TBL_TRAVEL_REFERENCE_TBL_TRAFFIC foreign key (TrafficId)
      references tbl_Traffic (TrafficId)
go







if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tbl_TourTraffic') and o.name = 'FK_TBL_TOURTRAFFIC_REFERENCE_TBL_TOUR')
alter table tbl_TourTraffic
   drop constraint FK_TBL_TOURTRAFFIC_REFERENCE_TBL_TOUR
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tbl_TourTraffic')
            and   type = 'U')
   drop table tbl_TourTraffic
go

/*==============================================================*/
/* Table: tbl_TourTraffic										*/
/* 团队交通子表													*/
/*==============================================================*/
create table tbl_TourTraffic (
   TourId               char(36)             not null,
   TrafficId            int                  not null,
   constraint PK_TBL_TOURTRAFFIC primary key (TourId, TrafficId)
)
go

alter table tbl_TourTraffic
   add constraint FK_TBL_TOURTRAFFIC_REFERENCE_TBL_TOUR foreign key (TourId)
      references tbl_Tour (TourId)
go




if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tbl_TourOrderTraffic') and o.name = 'FK_TBL_TOURORDERTRAFFIC_REFERENCE_TBL_TOURORDER')
alter table tbl_TourOrderTraffic
   drop constraint FK_TBL_TOURORDERTRAFFIC_REFERENCE_TBL_TOURORDER
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tbl_TourOrderTraffic')
            and   type = 'U')
   drop table tbl_TourOrderTraffic
go

/*==============================================================*/
/* Table: tbl_TourOrderTraffic                                  */
/* 订单交通信息表												*/
/*==============================================================*/
create table tbl_TourOrderTraffic (
   OrderId              char(36)             not null,
   TrafficId            int                  not null,
   constraint PK_TBL_TOURORDERTRAFFIC primary key (OrderId, TrafficId)
)
go

alter table tbl_TourOrderTraffic
   add constraint FK_TBL_TOURORDERTRAFFIC_REFERENCE_TBL_TOURORDER foreign key (OrderId)
      references tbl_TourOrder (ID)
go





--机票申请表增加  订单编号

GO
ALTER TABLE dbo.tbl_PlanTicketOut ADD
	OrderId char(36) NOT NULL CONSTRAINT DF_tbl_PlanTicketOut_OrderId DEFAULT ''
GO
DECLARE @v sql_variant 
SET @v = N'订单编号'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_PlanTicketOut', N'COLUMN', N'OrderId'
GO


--=====================================
--描述：添加交通
--创建人：李晓欢
--时间：2012-09-10
--=====================================
create PROC [dbo].[proc_Traffic_Add]
(
@TrafficName NVARCHAR(255), --交通名称
@ChildPrices MONEY, --儿童价
@CompanyId INT, --公司编号
@Operater NVARCHAR(255),  --操作员
@OperaterId INT,  --操作人编号
@InsueTime DATETIME,  --插入时间
@result INT OUTPUT   --输出参数
)
AS
BEGIN
 	 DECLARE @error INT 
 	 SET @error=0
 	 INSERT INTO tbl_Traffic(TrafficName,TrafficDays,ChildPrices,[Status],CompanyId,Operater,OperaterId,IsDelete,InsueTime)
 	 VALUES(@TrafficName, 0,@ChildPrices,0,@CompanyId,@Operater,@OperaterId,0,@InsueTime)
 	 SET @error=@error+@@error
 	 IF(@error<>0)
 	    BEGIN
 	    	SET @result=0
 	    	RETURN @result
 	    END
 	 ELSE
 	 	BEGIN
 	 		SET @result=1
 	 		RETURN @result
 	 	END
END
GO

--=====================================
--描述：修改交通
--创建人：李晓欢
--时间：2012-09-10
--=====================================
create PROC [dbo].[proc_Traffic_Update]
(
@TrafficID CHAR(36),--交通编号
@TrafficName NVARCHAR(255), --交通名称
@ChildPrices MONEY, --儿童价
@CompanyId INT, --公司编号
@Operater NVARCHAR(255),  --操作员
@OperaterId INT,  --操作人编号
@InsueTime DATETIME,  --插入时间
@result INT OUTPUT   --输出参数
)
AS
BEGIN
 	 DECLARE @error INT 
 	 SET @error=0
 	 UPDATE tbl_traffic SET TrafficName=@TrafficName,ChildPrices=@ChildPrices,CompanyId=@CompanyId,Operater=@Operater,OperaterId=@OperaterId,InsueTime=@InsueTime WHERE TrafficId=@TrafficID
 	 SET @error=@error+@@error
 	 IF(@error<>0)
 	    BEGIN
 	    	SET @result=0
 	    	RETURN @result
 	    END
 	 ELSE
 	 	BEGIN
 	 		SET @result=1
 	 		RETURN @result
 	 	END
END
GO


--=====================================
--描述：添加交通行程
--创建人：李晓欢
--时间：2012-09-11
--=====================================
create PROC [dbo].[proc_Travel_Add]
@TrafficId INT,
@SerialNum INT,
@TrafficType TINYINT,
@LProvince INT,
@LCity INT,
@RProvince INT,
@RCity INT,
@FilghtNum NVARCHAR(255),
@FlightCompany TINYINT,
@LTime NVARCHAR(255),
@RTime NVARCHAR(255),
@IsStop CHAR(1),
@Space TINYINT,
@AirPlaneType NVARCHAR(255),
@IntervalDays INT,
@CompanyId INT,
@Operater NVARCHAR(255),
@OperaterID INT,
@InsueTime DATETIME,
@result INT OUTPUT
AS
BEGIN
  	DECLARE @error INT
  	SET @error=0
  	INSERT INTO tbl_Travel(TrafficId,SerialNum,TrafficType,LProvince,LCity,RProvince,RCity,FilghtNum,FlightCompany,LTime,RTime,IsStop,[Space],AirPlaneType,IntervalDays,CompanyId,Operater,OperaterID,InsueTime)
  	VALUES(@TrafficId,@SerialNum,@TrafficType,@LProvince,@LCity,@RProvince,@RCity,@FilghtNum,@FlightCompany,@LTime,@RTime,@IsStop,@Space,@AirPlaneType,
  	@IntervalDays,@CompanyId,@Operater,@OperaterID,@InsueTime )
  	SET @error=@error+@@ERROR
  	UPDATE tbl_Traffic	SET TrafficDays =(SELECT SUM(isnull(IntervalDays,0)) FROM tbl_Travel WHERE tbl_Travel.TrafficId=tbl_Traffic.TrafficId)  WHERE TrafficId=@TrafficId
  	SET @error=@error+@@ERROR
  	IF(@error<>0)
  	  BEGIN
  	  	SET @result=0
  	  	RETURN @result
  	  END
  	 ELSE
  	 	BEGIN
  	     SET @result=1
  	     RETURN @result
  	 	END
END
GO

--=====================================
--描述：修改交通行程
--创建人：李晓欢
--时间：2012-09-11
--=====================================
create PROC [dbo].[proc_Travel_Update]
@TravelId INT,--行程编号
@TrafficId INT, --交通编号
@SerialNum INT, --序列号
@TrafficType TINYINT, --交通类型
@LProvince INT, --出发省份
@LCity INT, --出发城市
@RProvince INT, --抵达省份
@RCity INT, --抵达城市
@FilghtNum NVARCHAR(255), --航班号
@FlightCompany TINYINT, --航空公司
@LTime NVARCHAR(255), --出发时间
@RTime NVARCHAR(255), --抵达时间
@IsStop CHAR(1), --是否停靠
@Space TINYINT, --舱位
@AirPlaneType NVARCHAR(255), --机型
@IntervalDays INT, --间隔天数
@CompanyId INT, --公司编号
@Operater NVARCHAR(255), --操作员
@OperaterID INT, --操作员编号 
@InsueTime DATETIME, --插入时间
@result INT OUTPUT --输出参数 0失败 1成功
AS
BEGIN
  	DECLARE @error INT
  	SET @error=0
  	UPDATE tbl_Travel 	SET  		
  		TrafficId = @TrafficId,
  		SerialNum = @SerialNum,
  		TrafficType = @TrafficType,
  		LProvince = @LProvince,
  		LCity = @LCity,
  		RProvince = @RProvince,
  		RCity = @RCity,
  		FilghtNum = @FilghtNum,
  		FlightCompany = @FlightCompany,
  		LTime = @LTime,
  		RTime = @RTime,
  		IsStop = @IsStop,
  		[Space] = @Space,
  		AirPlaneType = @AirPlaneType,
  		IntervalDays = @IntervalDays,
  		CompanyId = @CompanyId,
  		Operater = @Operater,
  		OperaterID = @OperaterID,
  		InsueTime = @InsueTime WHERE TravelId=@TravelId
  	SET @error=@error+@@ERROR
  	UPDATE tbl_Traffic	SET TrafficDays =(SELECT SUM(isnull(IntervalDays,0)) FROM tbl_Travel WHERE tbl_Travel.TrafficId=tbl_Traffic.TrafficId)  WHERE TrafficId=@TrafficId
  	SET @error=@error+@@ERROR
  	IF(@error<>0)
  	  BEGIN
  	  	SET @result=0
  	  	RETURN @result
  	  END
  	 ELSE
  	 	BEGIN
  	     SET @result=1
  	     RETURN @result
  	 	END
  END
  
  
  go
  
  
  
GO
-- =============================================
-- Author:		汪奇志
-- Create date: 2011-01-21
-- Description:	写入散拼计划、团队计划、单项服务信息
-- History:
-- 1.2011-04-18 增加地接报价人数、单价及我社报价人数、单价
-- 2.2011-06-08 团队计划订单增加返佣
-- 3.2011-07-11 团队计划增加人数和价格信息，删除SelfUnitPriceAmount
-- 4.2011-12-20 增加对销售员（用于人次统计）字段的处理
-- 5.2012-04-09 汪奇志 增加最近出团的处理
-- 6.2012-08-24 汪奇志 订单游客信息表增加团队编号的写入
-- 7.2012-09-10 周文超 增加团队交通信息
-- =============================================
ALTER PROCEDURE [dbo].[proc_Tour_InsertTourInfo]
	@TourId CHAR(36),--团队编号
	@TourCode NVARCHAR(50),--团号
	@TourType TINYINT,--团队类型
	@Status TINYINT,--团队状态
	@TourDays INT,--团队天数
	@LDate DATETIME,--出团日期
	@AreaId INT,--线路区域编号
	@RouteName NVARCHAR(255),--线路名称
	@PlanPeopleNumber INT,--计划人数
	@TicketStatus TINYINT,--机票状态
	@ReleaseType CHAR(1),--发布类型
	@OperatorId INT,--发布人编号
	@CreateTime DATETIME,--发布时间
	@CompanyId INT,--公司编号
	@LTraffic NVARCHAR(255),--出发交通 去程航班/时间
	@RTraffic NVARCHAR(255),--返程交通 回程航班/时间
	@Gather NVARCHAR(255),--集合方式
	@RouteId INT=0,--引用的线路编号
	@LocalAgencys NVARCHAR(MAX)=NULL,--团队地接社信息 XML:<ROOT><Info AgencyId="INT地接社编号" Name="地接社名称" LicenseNo="许可证号" Telephone="联系电话" ContacterName="联系人" /></ROOT>
	@Attachs NVARCHAR(MAX)=NULL,--团队附件 XML:<ROOT><Info FilePath="" Name="" /></ROOT>
	@Plans NVARCHAR(MAX)=NULL,--标准发布团队行程信息 XML:<ROOT><Info Interval="区间" Vehicle="交通工具" Hotel="住宿" Dinner="用餐" Plan="行程内容" FilePath="附件路径" /></ROOT>
	@QuickPrivate NVARCHAR(MAX)=NULL,--快速发布团队专有信息 XML:<ROOT><Info Plan="团队行程" Service="服务标准" Remark="备注" /></ROOT>
	@Service NVARCHAR(MAX)=NULL,--包含项目信息(散拼计划包含项目、团队计划价格组成、单项服务) XML:<ROOT><Info Type="INT项目类型" Service="接待标准或单项服务具体要求" LocalPrice="MONEY地接报价" SelfPrice="MONEY我社报价" Remark="备注" LocalPeopleNumber="地接报价人数" LocalUnitPrice="地接报价单价" SelfPeopleNumber="我社报价人数" SelfUnitPrice="我社报价单价" /></ROOT>
	@NormalPrivate NVARCHAR(MAX)=NULL,--标准发布专有信息(不含项目等) XML:<ROOT><Info Key="键" Value="值" /></ROOT>
	@Childrens NVARCHAR(MAX)=NULL,--散拼计划子团信息 XML:<ROOT><Info TourId="团队编号" LDate="出团日期" TourCode="团号" /></ROOT>
	@TourPrice NVARCHAR(MAX)=NULL,--散拼计划报价信息 XML:<ROOT><Info AdultPrice="MONEY成人价" ChildrenPrice="MONEY儿童价" StandardId="报价标准编号" LevelId="客户等级编号" /></ROOT>
	@ChildrenCreateRule NVARCHAR(MAX)=NULL,--散拼计划子团建团规则 XML:<ROOT><Info Type="规则类型" SDate="起始日期" EDate="结束日期" Cycle="周期" /></ROOT>
	@SinglePrivate NVARCHAR(MAX)=NULL,--单项服务专有信息 XML:<ROOT><Info OrderId="订单编号" SellerId="销售员编号" SellerName="销售员姓名" TotalAmount="合计收入金额" GrossProfit="团队毛利" BuyerCId="客户单位编号" BuyerCName="客户单位名称" ContactName="联系人" ContactTelephone="联系电话" Mobile="联系手机" TotalOutAmount="合计支出金额" /></ROOT>
	@PlansSingle NVARCHAR(MAX)=NULL,--单项服务供应商安排信息 XML:<ROOT><Info PlanId="安排编号" ServiceType="项目类型" SupplierId="供应商编号" SupplierName="供应商名称" Arrange="具体安排" Amount="结算费用" Remark="备注" /></ROOT>
	@TeamPrivate NVARCHAR(MAX)=NULL,--团队计划专有信息 XML:<ROOT><Info OrderId="订单编号" SellerId="销售员编号" SellerName="销售员姓名" TotalAmount="合计金额" BuyerCId="客户单位编号" BuyerCName="客户单位名称" ContactName="联系人" ContactTelephone="联系电话" Mobile="联系手机" QuoteId="报价编号" /></ROOT>
	@CustomerDisplayType TINYINT=0,--游客信息体现类型
	@Customers NVARCHAR(MAX)=NULL,--订单游客信息 XML:<ROOT><Info TravelerId="游客编号" TravelerName="游客姓名" IdType="证件类型" IdNo="证件号" Gender="性别" TravelerType="类型(成人/儿童)" ContactTel="联系电话" TF_XiangMu="特服项目" TF_NeiRong="特服内容" TF_ZengJian="1:增加 0:减少" TF_FeiYong="特服费用" /></ROOT>	
	@CustomerFilePath NVARCHAR(255)=NULL,--订单游客附件
	@Result INT OUTPUT,--操作结果 正值：成功 负值或0：失败
	@CoordinatorId INT=0,--计调员编号,
	@GatheringTime NVARCHAR(MAX)=NULL,--集合时间
	@GatheringPlace NVARCHAR(MAX)=NULL,--集合地点
	@GatheringSign NVARCHAR(MAX)=NULL,--集合标志
	@SentPeoples NVARCHAR(MAX)=NULL,--送团人信息集合 XML:<ROOT><Info UID="送团人编号" /></ROOT>
	@TourCityId INT=0,--出港城市编号
	@TourTeamUnit NVARCHAR(MAX)=NULL--团队计划增加人数和价格信息XML:<ROOT><Info NumberCr="成人数" NumberEt="儿童数" NumberQp="全陪数" UnitAmountCr="成人单价" UnitAmountEt="儿童单价" UnitAmountQp="全陪单价" /></ROOT>
	,@TourTraffic NVARCHAR(max) = NULL  --团队交通信息XML:<ROOT><Info TrafficId="交通编号" /></ROOT>
AS
BEGIN
	DECLARE @hdoc INT
	DECLARE @errorcount INT
	
	SET @Result=0
	SET @errorcount=0

	DECLARE @CompanyName NVARCHAR(255)--公司名称
	DECLARE @OperatorName NVARCHAR(255)--操作人姓名
	DECLARE @OperatorTelephone NVARCHAR(255)--操作人电话
	DECLARE @OperatorMobile NVARCHAR(255)--操作人手机
	DECLARE @OperatorFax NVARCHAR(255)--操作人传真
	DECLARE @DepartmentId INT--操作员部门编号	
	SELECT @CompanyName=[CompanyName] FROM [tbl_CompanyInfo] WHERE Id=@CompanyId
	SELECT @OperatorName=[ContactName],@OperatorTelephone=[ContactTel],@OperatorMobile=[ContactMobile],@OperatorFax=[ContactFax] FROM [tbl_COmpanyUser] WHERE Id=@OperatorId
	SELECT @DepartmentId=[DepartId] FROM [tbl_CompanyUser] WHERE Id=@OperatorId	

	BEGIN TRAN
	IF(@TourType=0)--散拼计划
	BEGIN
		--表变量存放此次写入的团队(模板团及子团)
		DECLARE @tmptbl TABLE(TourId CHAR(36))
		--模板团信息
		INSERT INTO [tbl_Tour]([TourId],[TemplateId],[TourType],[LeaveDate],[RDate]
			,[AreaId],[Status],[IsCostConfirm],[IsReview],[TicketStatus]
			,[TourCode],[RouteId],[RouteName],[TourDays],[PlanPeopleNumber]
			,[VirtualPeopleNumber],[ReleaseType],[LTraffic],[RTraffic],[Gather]
			,[CreateTime],[OperatorId],[SellerId],[CompanyId],[TotalAmount]
			,[LastTime],[TotalIncome],[TotalExpenses],[TotalOtherIncome],[TotalOtherExpenses]
			,[DistributionAmount],[GrossProfit],[IsSettleInCome],[IsSettleOut],[IsDelete]
			,[EndDateTime],[TotalOutAmount],[GatheringTime],[GatheringPlace],[GatheringSign])
		VALUES(@TourId,'',@TourType,GETDATE(),DATEADD(DAY,@TourDays-1,GETDATE())
			,@AreaId,@Status,'0','0',@TicketStatus
			,@TourId,@RouteId,@RouteName,@TourDays,@PlanPeopleNumber
			,0,@ReleaseType,@LTraffic,@RTraffic,@Gather
			,GETDATE(),@OperatorId,0,@CompanyId,0
			,GETDATE(),0,0,0,0
			,0,0,'0','0','0'
			,NULL,0,@GatheringTime,@GatheringPlace,@GatheringSign)
		SET @errorcount=@errorcount+@@ERROR
		INSERT INTO @tmptbl(TourId) VALUES (@TourId)
		IF(@errorcount=0)--子团信息
		BEGIN
			DECLARE @tmpTrafficHodc INT --预控人数从当天的交通的总机票数来
			EXECUTE sp_xml_preparedocument @tmpTrafficHodc OUTPUT,@TourTraffic
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@Childrens
			INSERT INTO [tbl_Tour]([TourId],[TemplateId],[TourType],[LeaveDate],[RDate]
				,[AreaId],[Status],[IsCostConfirm],[IsReview],[TicketStatus]
				,[TourCode],[RouteId],[RouteName],[TourDays],[PlanPeopleNumber]
				,[VirtualPeopleNumber],[ReleaseType],[LTraffic],[RTraffic],[Gather]
				,[CreateTime],[OperatorId],[SellerId],[CompanyId],[TotalAmount]
				,[LastTime],[TotalIncome],[TotalExpenses],[TotalOtherIncome],[TotalOtherExpenses]
				,[DistributionAmount],[GrossProfit],[IsSettleInCome],[IsSettleOut],[IsDelete]
				,[EndDateTime],[TotalOutAmount],[GatheringTime],[GatheringPlace],[GatheringSign])
			SELECT A.TourId,@TourId,@TourType,A.LDate,DATEADD(DAY,@TourDays-1,A.LDate)
				,@AreaId,@Status,'0','0',@TicketStatus
				,A.TourCode,@RouteId,@RouteName,@TourDays
				,ISNULL((SELECT SUM(d.TicketNums) FROM OPENXML(@tmpTrafficHodc,'/ROOT/Info') WITH(TrafficId INT) AS c INNER JOIN tbl_TrafficPrices AS d ON d.TrafficId = c.TrafficId AND DATEDIFF(dd,d.SDateTime,A.LDate) = 0 AND d.[Status] = 0),0) - ISNULL((SELECT SUM(to1.PeopleNumber - to1.LeaguePepoleNum) FROM tbl_TourOrder to1 WHERE to1.IsDelete = '0' AND to1.OrderState IN (1,2,5) AND DATEDIFF(dd,to1.LeaveDate,A.LDate) = 0 AND EXISTS (SELECT 1 FROM tbl_TourOrderTraffic tot WHERE tot.OrderId = to1.ID AND tot.TrafficId IN (SELECT TrafficId FROM OPENXML(@tmpTrafficHodc,'/ROOT/Info') WITH(TrafficId INT) AS c))),0)
				,0,@ReleaseType,@LTraffic,@RTraffic,@Gather
				,GETDATE(),@OperatorId,0,@CompanyId,0
				,GETDATE(),0,0,0,0
				,0,0,'0','0','0'
				,NULL,0,@GatheringTime,@GatheringPlace,@GatheringSign
			FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(TourId CHAR(36),LDate DATETIME,TourCode NVARCHAR(255)) AS A
			SET @errorcount=@errorcount+@@ERROR
			INSERT INTO @tmptbl(TourId) SELECT A.TourId FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(TourId CHAR(36)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
			EXECUTE sp_xml_removedocument @tmpTrafficHodc
		END
		IF(@errorcount=0)--报价信息
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@TourPrice
			INSERT INTO [tbl_TourPrice]([TourId],[Standard],[LevelId],[AdultPrice],[ChildPrice])
			SELECT B.TourId,A.StandardId,A.LevelId,A.AdultPrice,A.ChildrenPrice FROM OPENXML(@hdoc,'/ROOT/Info') 
			WITH(AdultPrice MONEY,ChildrenPrice MONEY,StandardId INT,LevelId INT) AS A ,@tmptbl AS B WHERE A.LevelId>0 AND A.StandardId>0
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0)--团队操作员计调员信息
		BEGIN
			/*INSERT INTO [tbl_TourOperator]([TourId],[OperatorId]) SELECT B.TourId,A.UserId FROM (SELECT C.UserId FROM tbl_UserArea AS C WHERE C.AreaId=@AreaId AND EXISTS(SELECT 1 FROM tbl_CompanyUser AS D WHERE D.Id=C.UserId AND D.UserType=2)) AS A,@tmptbl AS B */
			INSERT INTO [tbl_TourOperator]([TourId],[OperatorId]) SELECT B.TourId,@CoordinatorId FROM @tmptbl AS B
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0 AND @ChildrenCreateRule IS NOT NULL)--建团规则
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@ChildrenCreateRule
			INSERT INTO [tbl_TourCreateRule]([TourId],[RuleType],[SDate],[EDate],[Cycle])
			SELECT B.TourId,A.[Type],A.SDate,A.EDate,A.Cycle FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH([Type] TINYINT,SDate DATETIME,EDate DATETIME,Cycle NVARCHAR(255)) AS A,@tmptbl AS B
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @LocalAgencys IS NOT NULL)--地接社信息
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@LocalAgencys
			INSERT INTO [tbl_TourLocalAgency]([TourId],[AgencyId],[Name],[LicenseNo],[Telephone],[ContacterName])
			SELECT B.TourId,A.AgencyId,A.[Name],A.LicenseNo,A.Telephone,A.ContacterName FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(AgencyId INT,[Name] NVARCHAR(255),LicenseNo NVARCHAR(255),Telephone NVARCHAR(255),ContacterName NVARCHAR(255)) AS A,@tmptbl AS B
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @Attachs IS NOT NULL)--附件信息
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@Attachs
			INSERT INTO [tbl_TourAttach]([TourId],[FilePath],[Name])
			SELECT B.TourId,A.FilePath,A.[Name] FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(FilePath NVARCHAR(255),[Name] NVARCHAR(255)) AS A,@tmptbl AS B
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @ReleaseType='0')--标准发布行程信息
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@Plans
			INSERT INTO [tbl_TourPlan]([TourId],[Interval],[Vehicle],[Hotel],[Dinner],[Plan],[FilePath])
			SELECT B.TourId,A.Interval,A.Vehicle,A.Hotel,A.Dinner,A.[Plan],A.FilePath FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(Interval NVARCHAR(255),Vehicle NVARCHAR(255),Hotel NVARCHAR(255),Dinner NVARCHAR(255),[Plan] NVARCHAR(MAX),FilePath NVARCHAR(255)) AS A,@tmptbl AS B
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @ReleaseType='1')--快速发布行程信息等
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@QuickPrivate
			INSERT INTO [tbl_TourQuickPlan]([TourId],[Plan],[Service],[Remark])
			SELECT B.TourId,A.[Plan],A.[Service],A.Remark FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH([Plan] NVARCHAR(MAX),[Service] NVARCHAR(MAX),Remark NVARCHAR(MAX)) AS A,@tmptbl AS B
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @ReleaseType='0' AND @Service IS NOT NULL)--标准发布包含项目
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@Service
			INSERT INTO [tbl_TourService]([TourId],[Key],[Value])
			SELECT B.TourId,dbo.fn_GetTourServiceKey(A.[Type]),A.[Service] FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH([Type] TINYINT,[Service] NVARCHAR(MAX)) AS A,@tmptbl AS B
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc			
		END		
		IF(@errorcount=0 AND @ReleaseType='0' AND @NormalPrivate IS NOT NULL)--标准发布不含项目等
		BEGIN			
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@NormalPrivate			
			INSERT INTO [tbl_TourService]([TourId],[Key],[Value])
			SELECT B.TourId,A.[Key],A.[Value] FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH([Key] NVARCHAR(50),[Value] NVARCHAR(MAX)) AS A,@tmptbl AS B			
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END		
		IF(@errorcount=0 AND @SentPeoples IS NOT NULL)--送团人信息
		BEGIN
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@SentPeoples
			INSERT INTO [tbl_TourSentTask]([TourId],[OperatorId])
			SELECT B.TourId,A.[UID] FROM OPENXML(@hdoc,'/ROOT/Info') WITH([UID] INT) AS A,@tmptbl AS B
			SET @errorcount=@errorcount+@@ERROR
			EXEC sp_xml_removedocument @hdoc
		END	
		IF(@errorcount=0 AND @TourCityId IS NOT NULL AND @TourCityId>0)--出港城市
		BEGIN
			INSERT INTO [tbl_TourCity]([TourId],[CityId]) SELECT A.TourId,@TourCityId FROM @tmptbl AS A
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0)--是否出团
		BEGIN
			UPDATE [tbl_Tour] SET IsLeave='1' WHERE TemplateId>'' AND LeaveDate<=GETDATE() AND TourId IN(SELECT TourId FROM @tmptbl)
			SET @errorcount=@errorcount+@@ERROR			
			IF(@errorcount=0)--标记团队状态为行程途中
			BEGIN
				UPDATE tbl_Tour SET Status=2 WHERE TourType IN(0,1) AND [Status] IN(0,1,3) AND DATEDIFF(DAY,GETDATE(),[LeaveDate])<=0 AND DATEDIFF(DAY,GETDATE(),[RDate])>0 AND TourId IN(SELECT TourId FROM @tmptbl)
				SET @errorcount=@errorcount+@@ERROR
			END			
			IF(@errorcount=0)--标记团队状态为回团报账
			BEGIN
				UPDATE tbl_Tour SET Status=3 WHERE TourType IN(0,1) AND [Status] IN(0,1,2) AND DATEDIFF(DAY,GETDATE(),[RDate])<0 AND TourId IN(SELECT TourId FROM @tmptbl)
				SET @errorcount=@errorcount+@@ERROR
			END
			IF(@errorcount=0)--标记最近出团
			BEGIN
				EXECUTE proc_Tour_IsRecently @TourId=@TourId,@IsT=N'1'
			END
		END
		IF @errorcount = 0 AND @TourTraffic IS NOT NULL --团队交通信息
		BEGIN
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@TourTraffic
			INSERT INTO tbl_TourTraffic (TourId,TrafficId)
			SELECT B.TourId,A.TrafficId FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(TrafficId int) AS A,@tmptbl AS B			
			SET @errorcount=@errorcount+@@ERROR
			EXEC sp_xml_removedocument @hdoc
		END
	END

	
	IF(@TourType=1)--团队计划
	BEGIN
		DECLARE @SellerId INT--销售员编号
		DECLARE @SellerName NVARCHAR(255)--销售员姓名
		DECLARE @CommissionType	TINYINT--返佣类型
		DECLARE @CommissionPrice MONEY--返佣金额（单价）
		DECLARE @IncomeAmount MONEY--订单收入金额
		--预控人数从当天的交通的总机票数来
		DECLARE @tmpPlanPeopleNumber INT
		DECLARE @YiShiYong int
		DECLARE @tmpTrafficHodcTeam INT
		EXECUTE sp_xml_preparedocument @tmpTrafficHodcTeam OUTPUT,@TourTraffic
		SELECT @tmpPlanPeopleNumber = isnull(SUM(d.TicketNums),0) FROM OPENXML(@tmpTrafficHodcTeam,'/ROOT/Info') WITH(TrafficId INT) AS c INNER JOIN tbl_TrafficPrices AS d ON d.TrafficId = c.TrafficId AND DATEDIFF(dd,d.SDateTime,@LDate) = 0 AND d.[Status] = 0
		SET @YiShiYong = ISNULL((SELECT SUM(to1.PeopleNumber - to1.LeaguePepoleNum) FROM tbl_TourOrder to1 WHERE to1.IsDelete = '0' AND to1.OrderState IN (1,2,5) AND DATEDIFF(dd,to1.LeaveDate,@LDate) = 0 AND EXISTS (SELECT 1 FROM tbl_TourOrderTraffic tot WHERE tot.OrderId = to1.ID AND tot.TrafficId IN (SELECT TrafficId FROM OPENXML(@tmpTrafficHodcTeam,'/ROOT/Info') WITH(TrafficId INT) AS c))),0)
		--当天总票数 - 当天已使用的票数
		IF @PlanPeopleNumber > (@tmpPlanPeopleNumber - @YiShiYong) 
		BEGIN
			ROLLBACK TRAN
			SET @Result=-1
			RETURN
		END
		
		EXECUTE sp_xml_removedocument @tmpTrafficHodcTeam
		--团队信息		
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@TeamPrivate
		SELECT @SellerId=SaleId,@SellerName=Saler,@CommissionType=[CommissionType],@CommissionPrice=[CommissionCount] FROM [tbl_Customer] WHERE Id=(SELECT A.BuyerCId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(BuyerCId INT) AS A)
		SELECT @IncomeAmount=TotalAmount FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TotalAmount MONEY) AS A
		IF(@CommissionType<>1) SET @CommissionType=2

		IF(@CommissionType=1)--现返时从订单金额中减去返佣金额
		BEGIN
			SET @IncomeAmount=@IncomeAmount-@PlanPeopleNumber*@CommissionPrice
		END

		INSERT INTO [tbl_Tour]([TourId],[TemplateId],[TourType],[LeaveDate],[RDate]
			,[AreaId],[Status],[IsCostConfirm],[IsReview],[TicketStatus]
			,[TourCode],[RouteId],[RouteName],[TourDays],[PlanPeopleNumber]
			,[VirtualPeopleNumber],[ReleaseType],[LTraffic],[RTraffic],[Gather]
			,[CreateTime],[OperatorId],[SellerId],[CompanyId],[TotalAmount]
			,[LastTime],[TotalIncome],[TotalExpenses],[TotalOtherIncome],[TotalOtherExpenses]
			,[DistributionAmount],[GrossProfit],[IsSettleInCome],[IsSettleOut],[IsDelete]
			,[EndDateTime],[TotalOutAmount],[QuoteId],[GatheringTime],[GatheringPlace]
			,[GatheringSign])
		SELECT @TourId,@TourId,@TourType,@LDate,DATEADD(DAY,@TourDays-1,@LDate)
			,@AreaId,@Status,'0','0',@TicketStatus
			,@TourCode,@RouteId,@RouteName,@TourDays,@PlanPeopleNumber
			,@PlanPeopleNumber,@ReleaseType,@LTraffic,@RTraffic,@Gather
			,GETDATE(),@OperatorId,@SellerId,@CompanyId,A.TotalAmount
			,GETDATE(),@IncomeAmount,0,0,0
			,0,0,'0','0','0'
			,NULL,0,A.QuoteId,@GatheringTime,@GatheringPlace
			,@GatheringSign
		FROM OPENXML(@hdoc,'/ROOT/Info')
		WITH(SellerId INT,TotalAmount MONEY,QuoteId INT) AS A
		SET @errorcount=@errorcount+@@ERROR
		EXECUTE sp_xml_removedocument @hdoc
		IF(@errorcount=0)--团队操作员计调员信息
		BEGIN
			/*INSERT INTO [tbl_TourOperator]([TourId],[OperatorId]) SELECT @TourId,A.UserId FROM (SELECT C.UserId FROM tbl_UserArea AS C WHERE C.AreaId=@AreaId AND EXISTS(SELECT 1 FROM tbl_CompanyUser AS D WHERE D.Id=C.UserId AND D.UserType=2)) AS A*/
			INSERT INTO [tbl_TourOperator]([TourId],[OperatorId])VALUES(@TourId,@CoordinatorId)
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0 AND @LocalAgencys IS NOT NULL)--地接社信息
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@LocalAgencys
			INSERT INTO [tbl_TourLocalAgency]([TourId],[AgencyId],[Name],[LicenseNo],[Telephone],[ContacterName])
			SELECT @TourId,A.AgencyId,A.[Name],A.LicenseNo,A.Telephone,A.ContacterName FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(AgencyId INT,[Name] NVARCHAR(255),LicenseNo NVARCHAR(255),Telephone NVARCHAR(255),ContacterName NVARCHAR(255)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @Attachs IS NOT NULL)--附件信息
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@Attachs
			INSERT INTO [tbl_TourAttach]([TourId],[FilePath],[Name])
			SELECT @TourId,A.FilePath,A.[Name] FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(FilePath NVARCHAR(255),[Name] NVARCHAR(255)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @ReleaseType='0')--标准发布行程信息
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@Plans
			INSERT INTO [tbl_TourPlan]([TourId],[Interval],[Vehicle],[Hotel],[Dinner],[Plan],[FilePath])
			SELECT @TourId,A.Interval,A.Vehicle,A.Hotel,A.Dinner,A.[Plan],A.FilePath FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(Interval NVARCHAR(255),Vehicle NVARCHAR(255),Hotel NVARCHAR(255),Dinner NVARCHAR(255),[Plan] NVARCHAR(MAX),FilePath NVARCHAR(255)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @ReleaseType='1')--快速发布行程信息等
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@QuickPrivate
			INSERT INTO [tbl_TourQuickPlan]([TourId],[Plan],[Service],[Remark])
			SELECT @TourId,A.[Plan],A.[Service],A.Remark FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH([Plan] NVARCHAR(MAX),[Service] NVARCHAR(MAX),Remark NVARCHAR(MAX)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @NormalPrivate IS NOT NULL)--标准发布不含项目等
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@NormalPrivate
			INSERT INTO [tbl_TourService]([TourId],[Key],[Value])
			SELECT @TourId,A.[Key],A.[Value] FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH([Key] NVARCHAR(50),[Value] NVARCHAR(MAX)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @Service IS NOT NULL)--价格组成
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@Service
			INSERT INTO [tbl_TourTeamPrice]([TourId],[ItemType],[Standard],[LocalPrice],[MyPrice],[Requirement],[Remark],[LocalPeopleNumber],[LocalUnitPrice],[SelfPeopleNumber],[SelfUnitPrice])
			SELECT @TourId,A.[Type],A.[Service],A.LocalPrice,A.SelfPrice,NULL,NULL,[LocalPeopleNumber],[LocalUnitPrice],[SelfPeopleNumber],[SelfUnitPrice] FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH([Type] TINYINT,[Service] NVARCHAR(MAX),LocalPrice MONEY,SelfPrice MONEY,[LocalPeopleNumber] INT,[LocalUnitPrice] MONEY,[SelfPeopleNumber] INT,[SelfUnitPrice] MONEY) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		
		DECLARE @ChildPrice MONEY
		DECLARE @PersonalPrice MONEY
		DECLARE @AdultNumber INT
		DECLARE @ChildNumber INT 
		DECLARE @NumberQP INT
		SET @ChildPrice = 0
		SET @PersonalPrice = 0
		SET @AdultNumber = 0
		SET @ChildNumber = 0
		SET @NumberQP = 0
		if @errorcount = 0 and @TourTeamUnit is not null  --团队计划增加人数和价格信息
		begin
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@TourTeamUnit
			select @AdultNumber = a.NumberCr,@ChildNumber = a.NumberEt,@NumberQP = a.NumberQp
				,@PersonalPrice = a.UnitAmountCr,@ChildPrice = a.UnitAmountEt from openxml(@hdoc,'/ROOT/Info')
			with(NumberCr int,NumberEt int,NumberQp int
				,UnitAmountCr money,UnitAmountEt money) as a
			EXEC sp_xml_removedocument @hdoc
		END
		SET @AdultNumber = ISNULL(@AdultNumber,0) + ISNULL(@NumberQP,0)
		IF @AdultNumber = 0 AND @ChildNumber = 0
		BEGIN
			SET @AdultNumber = @PlanPeopleNumber
		END
		--生成订单	
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@TeamPrivate		
		INSERT INTO [tbl_TourOrder]([ID],[OrderNo],[TourId],[AreaId],[RouteId]
			,[RouteName],[TourNo],[TourClassId],[LeaveDate],[LeaveTraffic]
			,[ReturnTraffic],[OrderType],[OrderState],[BuyCompanyID],[BuyCompanyName]
			,[ContactName],[ContactTel],[ContactMobile],[ContactFax],[SalerName]
			,[SalerId],[OperatorName],[OperatorID],[PriceStandId],[PersonalPrice]
			,[ChildPrice],[MarketPrice],[AdultNumber],[ChildNumber],[MarketNumber]
			,[PeopleNumber],[OtherPrice],[SaveSeatDate],[OperatorContent],[SpecialContent]
			,[SumPrice],[SellCompanyName],[SellCompanyId],[LastDate],[LastOperatorID]
			,[SuccessTime],[FinanceAddExpense],[FinanceRedExpense],[FinanceSum],[FinanceRemark]
			,[HasCheckMoney],[NotCheckMoney],[IssueTime],[IsDelete],[ViewOperatorId]
			,[CustomerDisplayType],[CustomerFilePath],[LeaguePepoleNum],[TourDays],[CommissionType]
			,[CommissionPrice],PerTimeSellerId)
		SELECT A.OrderId,dbo.fn_TourOrder_CreateOrderNo(),@TourId,@AreaId,@RouteId
			,@RouteName,@TourCode,@TourType,@LDate,@LTraffic
			,@RTraffic,2,5,A.BuyerCId,A.BuyerCName
			,@OperatorName,@OperatorTelephone,@OperatorMobile,@OperatorFax,@SellerName
			,@SellerId,@OperatorName,@OperatorId,0,@PersonalPrice
			,@ChildPrice,0,@AdultNumber,@ChildNumber,0
			,@PlanPeopleNumber,0,GETDATE(),'',''
			,@IncomeAmount,@CompanyName,@CompanyId,GETDATE(),@OperatorId
			,NULL,0,0,A.TotalAmount,''
			,0,0,GETDATE(),'0',@OperatorId
			,@CustomerDisplayType,@CustomerFilePath,0,@TourDays,@CommissionType
			,@CommissionPrice,@OperatorId
		FROM OPENXML(@hdoc,'/ROOT/Info')
		WITH(OrderId CHAR(36),SellerId INT,SellerName NVARCHAR(255),TotalAmount MONEY,BuyerCId INT,BuyerCName NVARCHAR(255),ContactName NVARCHAR(100),ContactTelephone NVARCHAR(50)) AS A
		SET @errorcount=@errorcount+@@ERROR				
		IF(@errorcount=0)--写收入
		BEGIN
			INSERT INTO [tbl_StatAllIncome]([CompanyId],[TourId],[AreaId],[TourType],[ItemId]
				,[ItemType],[Amount],[AddAmount],[ReduceAmount],[TotalAmount]
				,[OperatorId],[DepartmentId],[CreateTime],[HasCheckAmount],[NotCheckAmount]
				,[IsDelete])
			SELECT @CompanyId,@TourId,@AreaId,@TourType,A.OrderId
				,1,@IncomeAmount,0,0,@IncomeAmount
				,@SellerId,(SELECT [DepartId] FROM [tbl_CompanyUser] WHERE [Id]=@SellerId),GETDATE(),0,0
				,'0'
			FROM OPENXML(@hdoc,'/ROOT/Info') 
			WITH(OrderId CHAR(36),TotalAmount MONEY) AS A
			SET @errorcount=@errorcount+@@ERROR
		END
		EXECUTE sp_xml_removedocument @hdoc
		IF(@errorcount=0 AND @SentPeoples IS NOT NULL)--送团人信息
		BEGIN
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@SentPeoples
			INSERT INTO [tbl_TourSentTask]([TourId],[OperatorId])
			SELECT @TourId,A.[UID] FROM OPENXML(@hdoc,'/ROOT/Info') WITH([UID] INT) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXEC sp_xml_removedocument @hdoc
		END	
		IF(@errorcount=0 AND @TourCityId IS NOT NULL AND @TourCityId>0)--出港城市
		BEGIN
			INSERT INTO [tbl_TourCity]([TourId],[CityId]) VALUES(@TourId,@TourCityId)
			SET @errorcount=@errorcount+@@ERROR
		END
		if @errorcount = 0 and @TourTeamUnit is not null  --团队计划增加人数和价格信息
		begin
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@TourTeamUnit
			INSERT INTO [tbl_TourTeamUnit] ([TourId],[NumberCR],[NumberET],[NumberQP]
				,[UnitAmountCR],[UnitAmountET],[UnitAmountQP])
			select @TourId,a.NumberCr,a.NumberEt,a.NumberQp
				,a.UnitAmountCr,a.UnitAmountEt,a.UnitAmountQp from openxml(@hdoc,'/ROOT/Info')
			with(NumberCr int,NumberEt int,NumberQp int
				,UnitAmountCr money,UnitAmountEt money,UnitAmountQp money) as a
			SET @errorcount=@errorcount + @@ERROR
			EXEC sp_xml_removedocument @hdoc
		end
		IF(@errorcount=0)--是否出团
		BEGIN
			UPDATE [tbl_Tour] SET IsLeave='1' WHERE TourId=@TourId AND LeaveDate<=GETDATE()
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)--标记团队状态为行程途中
			BEGIN
				UPDATE tbl_Tour SET Status=2 WHERE TourType IN(0,1) AND [Status] IN(0,1,3) AND DATEDIFF(DAY,GETDATE(),[LeaveDate])<=0 AND DATEDIFF(DAY,GETDATE(),[RDate])>0 AND TourId=@TourId
				SET @errorcount=@errorcount+@@ERROR
			END			
			IF(@errorcount=0)--标记团队状态为回团报账
			BEGIN
				UPDATE tbl_Tour SET Status=3 WHERE TourType IN(0,1) AND [Status] IN(0,1,2) AND DATEDIFF(DAY,GETDATE(),[RDate])<0 AND TourId=@TourId
				SET @errorcount=@errorcount+@@ERROR
			END
		END
		IF @errorcount = 0 AND @TourTraffic IS NOT NULL  --团队交通信息
		BEGIN
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@TourTraffic
			INSERT INTO tbl_TourTraffic (TourId,TrafficId)
			SELECT @TourId,a.TrafficId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TrafficId INT) AS a
			SET @errorcount=@errorcount+@@ERROR
			EXEC sp_xml_removedocument @hdoc
		END
		IF @errorcount = 0 AND @TeamPrivate IS NOT NULL   --订单交通信息
		BEGIN
			DECLARE @tmphdoc INT
			DECLARE @tmpOrderId CHAR(36)
			EXEC sp_xml_preparedocument @tmphdoc OUTPUT, @TeamPrivate
			SELECT @tmpOrderId = OrderId FROM OPENXML(@tmphdoc,'/ROOT/Info') WITH(OrderId CHAR(36)) AS b
			EXEC sp_xml_removedocument @tmphdoc
			IF @tmpOrderId IS NOT NULL 
			BEGIN
				EXEC sp_xml_preparedocument @hdoc OUTPUT,@TourTraffic
				INSERT INTO tbl_TourOrderTraffic (OrderId,TrafficId)
				SELECT TOP 1 @tmpOrderId,a.TrafficId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TrafficId INT) AS a
				SET @errorcount=@errorcount+@@ERROR
				EXEC sp_xml_removedocument @hdoc
			END
		END
	END

	IF(@TourType=2)--单项服务
	BEGIN
		--团队信息
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@SinglePrivate
		INSERT INTO [tbl_Tour]([TourId],[TemplateId],[TourType],[LeaveDate],[RDate]
			,[AreaId],[Status],[IsCostConfirm],[IsReview],[TicketStatus]
			,[TourCode],[RouteId],[RouteName],[TourDays],[PlanPeopleNumber]
			,[VirtualPeopleNumber],[ReleaseType],[LTraffic],[RTraffic],[Gather]
			,[CreateTime],[OperatorId],[SellerId],[CompanyId],[TotalAmount]
			,[LastTime],[TotalIncome],[TotalExpenses],[TotalOtherIncome],[TotalOtherExpenses]
			,[DistributionAmount],[GrossProfit],[IsSettleInCome],[IsSettleOut],[IsDelete]
			,[EndDateTime],[TotalOutAmount])
		SELECT @TourId,@TourId,@TourType,@LDate,@LDate
			,@AreaId,@Status,'0','0',@TicketStatus
			,@TourCode,@RouteId,@RouteName,@TourDays,@PlanPeopleNumber
			,@PlanPeopleNumber,@ReleaseType,@LTraffic,@RTraffic,@Gather
			,GETDATE(),@OperatorId,A.SellerId,@CompanyId,A.TotalAmount
			,GETDATE(),A.TotalAmount,0,0,0
			,0,A.GrossProfit,'0','0','0'
			,NULL,A.TotalOutAmount
		FROM OPENXML(@hdoc,'/ROOT/Info')
		WITH(SellerId INT,TotalAmount MONEY,GrossProfit MONEY,TotalOutAmount MONEY) AS A
		SET @errorcount=@errorcount+@@ERROR
		EXECUTE sp_xml_removedocument @hdoc
		IF(@errorcount=0 AND @Service IS NOT NULL)--价格组成
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@Service
			INSERT INTO [tbl_TourTeamPrice]([TourId],[ItemType],[Standard],[LocalPrice],[MyPrice],[Requirement],[Remark])
			SELECT @TourId,A.[Type],NULL,A.LocalPrice,A.SelfPrice,A.[Service],A.Remark FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH([Type] TINYINT,LocalPrice MONEY,SelfPrice MONEY,[Service] NVARCHAR(MAX),Remark NVARCHAR(MAX)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		--供应商安排
		IF(@errorcount=0 AND @PlansSingle IS NOT NULL)
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@PlansSingle
			INSERT INTO [tbl_PlanSingle]([PlanId],[TourId],[ServiceType],[SupplierId],[SupplierName]
				,[Arrange],[Amount],[Remark],[OperatorId],[CreateTime]
				,[AddAmount],[ReduceAmount],[TotalAmount],[FRemark])
			SELECT A.PlanId,@TourId,A.ServiceType,A.SupplierId,A.SupplierName
				,A.Arrange,A.Amount,A.Remark,@OperatorId,GETDATE()
				,0,0,A.Amount,''
			FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(PlanId CHAR(36),ServiceType TINYINT,SupplierId INT,SupplierName NVARCHAR(255),Arrange NVARCHAR(MAX),Amount MONEY,Remark NVARCHAR(MAX)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @PlansSingle IS NOT NULL)--写支出
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@PlansSingle
			INSERT INTO [tbl_StatAllOut]([CompanyId],[TourId],[AreaId],[TourType],[ItemId]
				,[ItemType],[Amount],[AddAmount],[ReduceAmount],[TotalAmount]
				,[OperatorId],[DepartmentId],[CreateTime],[HasCheckAmount],[NotCheckAmount]
				,[IsDelete],[SupplierId],[SupplierName])
			SELECT @CompanyId,@TourId,@AreaId,@TourType,A.PlanId
				,4,A.Amount,0,0,A.Amount
				,@OperatorId,@DepartmentId,GETDATE(),0,0
				,'0',A.SupplierId,A.SupplierName
			FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(PlanId CHAR(36),Amount MONEY,SupplierId INT,SupplierName NVARCHAR(255)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		--生成订单
		DECLARE @OrderId CHAR(36) --订单编号
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@SinglePrivate
		SELECT @OrderId=OrderId FROM OPENXML(@hdoc,'/ROOT/Info')WITH(OrderId CHAR(36))
		INSERT INTO [tbl_TourOrder]([ID],[OrderNo],[TourId],[AreaId],[RouteId]
			,[RouteName],[TourNo],[TourClassId],[LeaveDate],[LeaveTraffic]
			,[ReturnTraffic],[OrderType],[OrderState],[BuyCompanyID],[BuyCompanyName]
			,[ContactName],[ContactTel],[ContactMobile],[ContactFax],[SalerName]
			,[SalerId],[OperatorName],[OperatorID],[PriceStandId],[PersonalPrice]
			,[ChildPrice],[MarketPrice],[AdultNumber],[ChildNumber],[MarketNumber]
			,[PeopleNumber],[OtherPrice],[SaveSeatDate],[OperatorContent],[SpecialContent]
			,[SumPrice],[SellCompanyName],[SellCompanyId],[LastDate],[LastOperatorID]
			,[SuccessTime],[FinanceAddExpense],[FinanceRedExpense],[FinanceSum],[FinanceRemark]
			,[HasCheckMoney],[NotCheckMoney],[IssueTime],[IsDelete],[ViewOperatorId]
			,[CustomerDisplayType],[CustomerFilePath],[LeaguePepoleNum],[TourDays],[PerTimeSellerId])
		SELECT A.OrderId,dbo.fn_TourOrder_CreateOrderNo(),@TourId,@AreaId,@RouteId
			,@RouteName,@TourCode,@TourType,@LDate,@LTraffic
			,@RTraffic,3,5,A.BuyerCId,A.BuyerCName
			,A.ContactName,A.ContactTelephone,A.ContactMobile,'',A.SellerName
			,A.SellerId,@OperatorName,@OperatorId,0,0
			,0,0,@PlanPeopleNumber,0,0
			,@PlanPeopleNumber,0,GETDATE(),'',''
			,A.TotalAmount,@CompanyName,@CompanyId,GETDATE(),@OperatorId
			,NULL,0,0,A.TotalAmount,''
			,0,0,GETDATE(),'0',@OperatorId
			,@CustomerDisplayType,@CustomerFilePath,0,@TourDays,@OperatorId
		FROM OPENXML(@hdoc,'/ROOT/Info')
		WITH(OrderId CHAR(36),SellerId INT,SellerName NVARCHAR(255),TotalAmount MONEY,BuyerCId INT,BuyerCName NVARCHAR(255),ContactName NVARCHAR(100),ContactTelephone NVARCHAR(50),ContactMobile NVARCHAR(50)) AS A
		SET @errorcount=@errorcount+@@ERROR
		IF(@errorcount=0)--写收入
		BEGIN
			INSERT INTO [tbl_StatAllIncome]([CompanyId],[TourId],[AreaId],[TourType],[ItemId]
				,[ItemType],[Amount],[AddAmount],[ReduceAmount],[TotalAmount]
				,[OperatorId],[DepartmentId],[CreateTime],[HasCheckAmount],[NotCheckAmount]
				,[IsDelete])
			SELECT @CompanyId,@TourId,@AreaId,@TourType,A.OrderId
				,1,A.TotalAmount,0,0,A.TotalAmount
				,A.SellerId,(SELECT [DepartId] FROM [tbl_CompanyUser] WHERE [Id]=A.SellerId),GETDATE(),0,0
				,'0'
			FROM OPENXML(@hdoc,'/ROOT/Info') 
			WITH(OrderId CHAR(36),TotalAmount MONEY,SellerId INT) AS A
			SET @errorcount=@errorcount+@@ERROR
		END
		EXECUTE sp_xml_removedocument @hdoc	
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@Customers	
		IF(@errorcount=0 AND @Customers IS NOT NULL)--订单游客信息
		BEGIN			
			INSERT INTO [tbl_TourOrderCustomer]([ID],[OrderId],[CompanyID],[VisitorName],[CradType]
				,[CradNumber],[Sex],[VisitorType],[ContactTel],[IssueTime]
				,[CustomerStatus],[TourId])
			SELECT A.TravelerId,@OrderId,@CompanyId,A.TravelerName,A.IdType
				,A.IdNo,A.Gender,A.TravelerType,A.ContactTel,GETDATE()
				,0,@TourId FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(TravelerId CHAR(36),TravelerName NVARCHAR(100),IdType TINYINT,IdNo NVARCHAR(255),Gender TINYINT,TravelerType TINYINT,ContactTel NVARCHAR(100)) AS A
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0 AND @Customers IS NOT NULL)--游客特服信息
		BEGIN
			INSERT INTO [tbl_CustomerSpecialService]([CustormerId],[ProjectName],[ServiceDetail],[IsAdd],[Fee],[IssueTime])
			SELECT A.TravelerId,A.TF_XiangMu,A.TF_NeiRong,A.TF_ZengJian,A.TF_FeiYong,GETDATE() FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(TravelerId CHAR(36),TF_XiangMu NVARCHAR(255),TF_NeiRong NVARCHAR(500),TF_ZengJian TINYINT,TF_FeiYong MONEY) AS A
			SET @errorcount=@errorcount+@@ERROR
		END
		EXECUTE sp_xml_removedocument @hdoc		
	END

	IF(@errorcount>0)
	BEGIN
		ROLLBACK TRAN
		SET @Result=0
	END
	ELSE
	BEGIN		
		COMMIT TRAN
		SET @Result=1
	END
END



go




GO
-- =============================================
-- Author:		汪奇志
-- Create date: 2011-01-23
-- Description:	更新散拼计划、团队计划、单项服务信息
-- History:
-- 1.2011-07-11 团队计划增加人数和价格信息，删除SelfUnitPriceAmount
-- 2.2012-04-09 汪奇志 增加最近出团处理
-- 3.2012-07-30 汪奇志 修改计划同步修改订单的线路区域信息
-- 4.2012-08-24 汪奇志 订单游客信息表增加团队编号的写入
-- 5.2012-09-11 周文超 增加团队交通的修改
-- =============================================
ALTER PROCEDURE [dbo].[proc_Tour_UpdateTourInfo]
	@TourId CHAR(36),--团队编号
	@TourCode NVARCHAR(50),--团号
	@TourType TINYINT,--团队类型
	@TourDays INT,--团队天数
	@LDate DATETIME,--出团日期
	@AreaId INT,--线路区域编号
	@RouteName NVARCHAR(255),--线路名称
	@PlanPeopleNumber INT,--计划人数
	@ReleaseType CHAR(1),--发布类型
	@OperatorId INT,--发布人编号
	@CompanyId INT,--公司编号
	@LTraffic NVARCHAR(255),--出发交通 去程航班/时间
	@RTraffic NVARCHAR(255),--返程交通 回程航班/时间
	@Gather NVARCHAR(255),--集合方式
	@RouteId INT=0,--引用的线路编号
	@LocalAgencys NVARCHAR(MAX)=NULL,--团队地接社信息 XML:<ROOT><Info AgencyId="INT地接社编号" Name="地接社名称" LicenseNo="许可证号" Telephone="联系电话" ContacterName="联系人" /></ROOT>
	@Attachs NVARCHAR(MAX)=NULL,--团队附件 XML:<ROOT><Info FilePath="" Name="" /></ROOT>
	@Plans NVARCHAR(MAX)=NULL,--标准发布团队行程信息 XML:<ROOT><Info Interval="区间" Vehicle="交通工具" Hotel="住宿" Dinner="用餐" Plan="行程内容" FilePath="附件路径" /></ROOT>
	@QuickPrivate NVARCHAR(MAX)=NULL,--快速发布团队专有信息 XML:<ROOT><Info Plan="团队行程" Service="服务标准" Remark="备注" /></ROOT>
	@Service NVARCHAR(MAX)=NULL,--包含项目信息(散拼计划包含项目、团队计划价格组成、单项服务) XML:<ROOT><Info Type="INT项目类型" Service="接待标准或单项服务具体要求" LocalPrice="MONEY地接报价" SelfPrice="MONEY我社报价" Remark="备注" LocalPeopleNumber="地接报价人数" LocalUnitPrice="地接报价单价" SelfPeopleNumber="我社报价人数" SelfUnitPrice="我社报价单价" /></ROOT>
	@NormalPrivate NVARCHAR(MAX)=NULL,--标准发布专有信息(不含项目等) XML:<ROOT><Info Key="键" Value="值" /></ROOT>
	@TourPrice NVARCHAR(MAX)=NULL,--散拼计划报价信息 XML:<ROOT><Info AdultPrice="MONEY成人价" ChildrenPrice="MONEY儿童价" StandardId="报价标准编号" LevelId="客户等级编号" /></ROOT>
	@SinglePrivate NVARCHAR(MAX)=NULL,--单项服务专有信息 XML:<ROOT><Info OrderId="订单编号" SellerId="销售员编号" SellerName="销售员姓名" TotalAmount="合计收入金额" GrossProfit="团队毛利" BuyerCId="客户单位编号" BuyerCName="客户单位名称" ContactName="联系人" ContactTelephone="联系电话" ContactMobile="联系手机" TotalOutAmount="合计支出金额" /></ROOT>
	@PlansSingle NVARCHAR(MAX)=NULL,--单项服务供应商安排信息 XML:<ROOT><Info PlanId="安排编号" ServiceType="项目类型" SupplierId="供应商编号" SupplierName="供应商名称" Arrange="具体安排" Amount="结算费用" Remark="备注" /></ROOT>
	@TeamPrivate NVARCHAR(MAX)=NULL,--团队计划专有信息 XML:<ROOT><Info OrderId="订单编号" SellerId="销售员编号" SellerName="销售员姓名" TotalAmount="合计金额" BuyerCId="客户单位编号" BuyerCName="客户单位名称" ContactName="联系人" ContactTelephone="联系电话" ContactMobile="联系手机" /></ROOT>
	@CustomerDisplayType TINYINT=0,--游客信息体现类型
	@Customers NVARCHAR(MAX)=NULL,--订单游客信息 XML:<ROOT><Info TravelerId="游客编号" TravelerName="游客姓名" IdType="证件类型" IdNo="证件号" Gender="性别" TravelerType="类型(成人/儿童)" ContactTel="联系电话" TF_XiangMu="特服项目" TF_NeiRong="特服内容" TF_ZengJian="1:增加 0:减少" TF_FeiYong="特服费用" /></ROOT>	
	@CustomerFilePath NVARCHAR(255)=NULL,--订单游客附件
	@Result INT OUTPUT,--操作结果 正值：成功 负值或0：失败
	@OrderId CHAR(36) OUTPUT,--订单编号（仅针对团队计划及单项服务）
	@CoordinatorId INT=0,--计调员编号
	@GatheringTime NVARCHAR(MAX)=NULL,--集合时间
	@GatheringPlace NVARCHAR(MAX)=NULL,--集合地点
	@GatheringSign NVARCHAR(MAX)=NULL,--集合标志
	@SentPeoples NVARCHAR(MAX)=NULL,--送团人信息集合 XML:<ROOT><Info UID="送团人编号" /></ROOT>
	@TourCityId INT=0,--出港城市编号
	@TourTeamUnit NVARCHAR(MAX)=NULL--团队计划增加人数和价格信息XML:<ROOT><Info NumberCr="成人数" NumberEt="儿童数" NumberQp="全陪数" UnitAmountCr="成人单价" UnitAmountEt="儿童单价" UnitAmountQp="全陪单价" /></ROOT>
	,@TourTraffic NVARCHAR(max) = NULL  --团队交通信息XML:<ROOT><Info TrafficId="交通编号" /></ROOT>
AS
BEGIN
	DECLARE @hdoc INT
	DECLARE @errorcount INT
	
	SET @Result=0
	SET @errorcount=0
	SET @OrderId=''

	DECLARE @CompanyName NVARCHAR(255)--公司名称
	DECLARE @OperatorName NVARCHAR(255)--操作人姓名
	DECLARE @OperatorTelephone NVARCHAR(255)--操作人电话
	DECLARE @OperatorMobile NVARCHAR(255)--操作人手机
	DECLARE @OperatorFax NVARCHAR(255)--操作人传真
	DECLARE @DepartmentId INT--操作员部门编号	
	DECLARE @CRouteId INT--修改前线路编号
	--预控人数从当天的交通的总机票数来
	DECLARE @tmpTrafficHodcTeam INT
	DECLARE @tmpPlanPeopleNumber INT
	DECLARE @YiShiYong INT
	DECLARE @OrderPeopleNum INT 
	
	IF @PlanPeopleNumber IS NULL 
	BEGIN
		SET @PlanPeopleNumber = 0
	END
	
	EXECUTE sp_xml_preparedocument @tmpTrafficHodcTeam OUTPUT,@TourTraffic
	SELECT @tmpPlanPeopleNumber = isnull(SUM(d.TicketNums),0) FROM OPENXML(@tmpTrafficHodcTeam,'/ROOT/Info') WITH(TrafficId INT) AS c INNER JOIN tbl_TrafficPrices AS d ON d.TrafficId = c.TrafficId AND DATEDIFF(dd,d.SDateTime,@LDate) = 0 AND d.[Status] = 0
	SET @OrderPeopleNum = ISNULL((SELECT SUM(to1.PeopleNumber - to1.LeaguePepoleNum) FROM tbl_TourOrder to1 WHERE to1.IsDelete = '0' AND to1.OrderState IN (1,2,5) AND to1.TourId = @TourId),0)
	SET @YiShiYong = ISNULL((SELECT SUM(to1.PeopleNumber - to1.LeaguePepoleNum) FROM tbl_TourOrder to1 WHERE to1.IsDelete = '0' AND to1.OrderState IN (1,2,5) AND DATEDIFF(dd,to1.LeaveDate,@LDate) = 0 AND EXISTS (SELECT 1 FROM tbl_TourOrderTraffic tot WHERE tot.OrderId = to1.ID AND tot.TrafficId IN (SELECT TrafficId FROM OPENXML(@tmpTrafficHodcTeam,'/ROOT/Info') WITH(TrafficId INT) AS c))),0)
	EXECUTE sp_xml_removedocument @tmpTrafficHodcTeam	
	
	IF  @TourType = 1 AND @PlanPeopleNumber <= 0
	BEGIN
		SET @Result= -1
		RETURN
	END
	IF @TourType = 1 AND @PlanPeopleNumber < @OrderPeopleNum
	BEGIN
		SET @Result= -2
		RETURN
	END	
	IF @TourType = 0
	BEGIN
		SET @PlanPeopleNumber = @OrderPeopleNum + @tmpPlanPeopleNumber - @YiShiYong
	END

	SELECT @CompanyName=[CompanyName] FROM [tbl_CompanyInfo] WHERE Id=@CompanyId
	SELECT @OperatorName=[ContactName],@OperatorTelephone=[ContactTel],@OperatorMobile=[ContactMobile],@OperatorFax=[ContactFax] FROM [tbl_COmpanyUser] WHERE Id=@OperatorId
	SELECT @DepartmentId=[DepartId] FROM [tbl_CompanyUser] WHERE Id=@OperatorId	
	SELECT @CRouteId=[RouteId] FROM [tbl_Tour] WHERE [TourId]=@TourId

	BEGIN TRAN
	IF(@TourType=0)--散拼计划
	BEGIN
		--计划信息
		UPDATE [tbl_Tour] SET [LeaveDate] = @LDate
			,[RDate] = DATEADD(DAY,@TourDays-1,@LDate)
			,[AreaId] = @AreaId
			,[TourCode] = @TourCode
			,[RouteId] = @RouteId
			,[RouteName] = @RouteName
			,[TourDays] = @TourDays
			,[PlanPeopleNumber] = @PlanPeopleNumber
			,[LTraffic] = @LTraffic
			,[RTraffic] = @RTraffic
			,[Gather] = @Gather
			,[OperatorId] = @OperatorId
			,[LastTime] = GETDATE()
			,[GatheringTime]=@GatheringTime
			,[GatheringPlace]=@GatheringPlace
			,[GatheringSign]=@GatheringSign
		WHERE [TourId]=@TourId
		SET @errorcount=@errorcount+@@ERROR
		IF(@errorcount=0)--报价信息
		BEGIN			
			DELETE FROM [tbl_TourPrice] WHERE [TourId]=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				EXEC sp_xml_preparedocument @hdoc OUTPUT,@TourPrice
				INSERT INTO [tbl_TourPrice]([TourId],[Standard],[LevelId],[AdultPrice],[ChildPrice])
				SELECT @TourId,A.StandardId,A.LevelId,A.AdultPrice,A.ChildrenPrice FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH(AdultPrice MONEY,ChildrenPrice MONEY,StandardId INT,LevelId INT) AS A WHERE A.LevelId>0 AND A.StandardId>0
				SET @errorcount=@errorcount+@@ERROR
				EXEC sp_xml_removedocument @hdoc
			END			
		END
		IF(@errorcount=0)--团队操作员计调员信息
		BEGIN
			DELETE FROM [tbl_TourOperator] WHERE [TourId]=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				/*INSERT INTO [tbl_TourOperator]([TourId],[OperatorId]) SELECT @TourId,A.UserId FROM (SELECT C.UserId FROM tbl_UserArea AS C WHERE C.AreaId=@AreaId AND EXISTS(SELECT 1 FROM tbl_CompanyUser AS D WHERE D.Id=C.UserId AND D.UserType=2)) AS A*/
				INSERT INTO [tbl_TourOperator]([TourId],[OperatorId]) VALUES (@TourId,@CoordinatorId)
				SET @errorcount=@errorcount+@@ERROR
			END			
		END		
		IF(@errorcount=0 AND @LocalAgencys IS NOT NULL)--地接社信息
		BEGIN
			DELETE FROM [tbl_TourLocalAgency] WHERE TourId=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				EXEC sp_xml_preparedocument @hdoc OUTPUT,@LocalAgencys
				INSERT INTO [tbl_TourLocalAgency]([TourId],[AgencyId],[Name],[LicenseNo],[Telephone],[ContacterName])
				SELECT @TourId,A.AgencyId,A.[Name],A.LicenseNo,A.Telephone,A.ContacterName FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH(AgencyId INT,[Name] NVARCHAR(255),LicenseNo NVARCHAR(255),Telephone NVARCHAR(255),ContacterName NVARCHAR(255)) AS A
				SET @errorcount=@errorcount+@@ERROR
				EXEC sp_xml_removedocument @hdoc
			END
		END
		IF(@errorcount=0 AND @Attachs IS NOT NULL)--附件信息
		BEGIN
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@Attachs
			--删除的附件
			INSERT INTO [tbl_SysDeletedFileQue]([FilePath]) SELECT A.[FilePath] FROM [tbl_TourAttach] AS A WHERE A.TourId=@TourId AND A.[FilePath] NOT IN(SELECT B.FilePath FROM OPENXML(@hdoc,'/ROOT/Info') WITH(FilePath NVARCHAR(255))AS B)
			DELETE FROM [tbl_TourAttach] WHERE TourId=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN				
				INSERT INTO [tbl_TourAttach]([TourId],[FilePath],[Name])
				SELECT @TourId,A.FilePath,A.[Name] FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH(FilePath NVARCHAR(255),[Name] NVARCHAR(255)) AS A
				SET @errorcount=@errorcount+@@ERROR
				
			END
			EXEC sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @ReleaseType='0')--标准发布行程信息
		BEGIN
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@Plans
			--删除的附件
			INSERT INTO [tbl_SysDeletedFileQue]([FilePath]) SELECT A.[FilePath] FROM [tbl_TourPlan] AS A WHERE A.TourId=@TourId AND A.[FilePath] NOT IN(SELECT B.FilePath FROM OPENXML(@hdoc,'/ROOT/Info') WITH(FilePath NVARCHAR(255))AS B)
			DELETE FROM [tbl_TourPlan] WHERE TourId=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				INSERT INTO [tbl_TourPlan]([TourId],[Interval],[Vehicle],[Hotel],[Dinner],[Plan],[FilePath])
				SELECT @TourId,A.Interval,A.Vehicle,A.Hotel,A.Dinner,A.[Plan],A.FilePath FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH(Interval NVARCHAR(255),Vehicle NVARCHAR(255),Hotel NVARCHAR(255),Dinner NVARCHAR(255),[Plan] NVARCHAR(MAX),FilePath NVARCHAR(255)) AS A
				SET @errorcount=@errorcount+@@ERROR
			END
			EXEC sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @ReleaseType='1')--快速发布行程信息等
		BEGIN
			DELETE FROM [tbl_TourQuickPlan] WHERE TourId=@TourId
			SET @errorcount=@errorcount+@@ERROR
			BEGIN
				EXEC sp_xml_preparedocument @hdoc OUTPUT,@QuickPrivate
				INSERT INTO [tbl_TourQuickPlan]([TourId],[Plan],[Service],[Remark])
				SELECT @TourId,A.[Plan],A.[Service],A.Remark FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH([Plan] NVARCHAR(MAX),[Service] NVARCHAR(MAX),Remark NVARCHAR(MAX)) AS A
				SET @errorcount=@errorcount+@@ERROR
				EXEC sp_xml_removedocument @hdoc
			END
		END
		IF(@errorcount=0 AND @ReleaseType='0' AND @Service IS NOT NULL)--标准发布包含项目
		BEGIN
			DELETE FROM [tbl_TourService] WHERE TourId=@TourId AND [Key] IN('DiJie','JiuDian','YongCan','JingDian','BaoXian','DaJiaoTong','DaoFu','GouWu','XiaoJiaoTong','QiTa')
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				EXEC sp_xml_preparedocument @hdoc OUTPUT,@Service
				INSERT INTO [tbl_TourService]([TourId],[Key],[Value])
				SELECT @TourId,dbo.fn_GetTourServiceKey(A.[Type]),A.[Service] FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH([Type] TINYINT,[Service] NVARCHAR(MAX)) AS A
				SET @errorcount=@errorcount+@@ERROR
				EXEC sp_xml_removedocument @hdoc
			END
		END		
		IF(@errorcount=0 AND @ReleaseType='0' AND @NormalPrivate IS NOT NULL)--标准发布不含项目等
		BEGIN			
			DELETE FROM [tbl_TourService] WHERE TourId=@TourId AND [Key] IN('BuHanXiangMu','ErTongAnPai','GouWuAnPai','NeiBuXingXi','WenXinTiXing','ZhuYiShiXiang','ZiFeiXIangMu')
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				EXEC sp_xml_preparedocument @hdoc OUTPUT,@NormalPrivate			
				INSERT INTO [tbl_TourService]([TourId],[Key],[Value])
				SELECT @TourId,A.[Key],A.[Value] FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH([Key] NVARCHAR(50),[Value] NVARCHAR(MAX)) AS A
				SET @errorcount=@errorcount+@@ERROR
				EXEC sp_xml_removedocument @hdoc
			END
		END	
		IF(@errorcount=0 AND @SentPeoples IS NOT NULL)--送团人信息
		BEGIN
			DELETE FROM [tbl_TourSentTask] WHERE [TourId]=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				EXEC sp_xml_preparedocument @hdoc OUTPUT,@SentPeoples
				INSERT INTO [tbl_TourSentTask]([TourId],[OperatorId])
				SELECT @TourId,A.[UID] FROM OPENXML(@hdoc,'/ROOT/Info') WITH([UID] INT) AS A
				SET @errorcount=@errorcount+@@ERROR
				EXEC sp_xml_removedocument @hdoc
			END
		END	
		IF(@errorcount=0 AND @TourCityId IS NOT NULL AND @TourCityId>0)--出港城市
		BEGIN
			DELETE FROM [tbl_TourCity] WHERE [TourId]=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				INSERT INTO [tbl_TourCity]([TourId],[CityId]) VALUES(@TourId,@TourCityId)
				SET @errorcount=@errorcount+@@ERROR
			END
		END
		IF(@errorcount=0)--是否出团
		BEGIN
			UPDATE [tbl_Tour] SET IsLeave='1' WHERE TourId=@TourId AND LeaveDate<=GETDATE()
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0)--订单表团号处理
		BEGIN
			UPDATE [tbl_TourOrder] SET [TourNo]=@TourCode WHERE [TourId]=@TourId
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0)--团队状态维护
		BEGIN
			IF(@errorcount=0)
			BEGIN
				UPDATE [tbl_Tour] SET [Status]=1  WHERE [Status]=0 AND PlanPeopleNumber=(SELECT ISNULL(SUM(PeopleNumber-LeaguePepoleNum),0) FROM tbl_TourOrder WHERE TourId = @TourId AND IsDelete='0' AND OrderState NOT IN(3,4))	and TourId=@TourId 
				SET @errorcount=@errorcount+@@ERROR
			END
			IF(@errorcount=0)
			BEGIN
				UPDATE [tbl_Tour] SET [Status]=0  WHERE [Status]=1 AND PlanPeopleNumber>(SELECT ISNULL(SUM(PeopleNumber-LeaguePepoleNum),0) FROM tbl_TourOrder WHERE TourId = @TourId AND IsDelete='0' AND OrderState NOT IN(3,4))	and TourId=@TourId 
				SET @errorcount=@errorcount+@@ERROR
			END
			--标记团队状态为行程途中
			UPDATE tbl_Tour SET Status=2 WHERE TourType IN(0,1) AND [Status] IN(0,1,3) AND DATEDIFF(DAY,GETDATE(),[LeaveDate])<=0 AND DATEDIFF(DAY,GETDATE(),[RDate])>0 AND TourId=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)--标记团队状态为回团报账
			BEGIN
				UPDATE tbl_Tour SET Status=3 WHERE TourType IN(0,1) AND [Status] IN(0,1,2) AND DATEDIFF(DAY,GETDATE(),[RDate])<0 AND TourId=@TourId
				SET @errorcount=@errorcount+@@ERROR
			END
			IF(@errorcount=0)
			BEGIN
				UPDATE [tbl_Tour] SET [Status]=2 WHERE [TourId]=@TourId AND [Status] =3 AND DATEDIFF(DAY,GETDATE(),[RDate])>0
				SET @errorcount=@errorcount+@@ERROR
			END
			IF(@errorcount=0)--标记最近出团
			BEGIN
				EXECUTE proc_Tour_IsRecently @TourId=@TourId,@IsT=N'0'
			END
		END

		IF(@errorcount=0)--同步订单表线路区域
		BEGIN
			UPDATE tbl_TourOrder SET AreaId=@AreaId WHERE TourId=@TourId
		END
		IF @errorcount = 0 AND @TourTraffic IS NOT NULL  --团队交通信息
		BEGIN
			DELETE FROM tbl_TourTraffic WHERE TourId = @TourId
			SET @errorcount=@errorcount+@@ERROR
			IF @errorcount = 0
			BEGIN
				EXEC sp_xml_preparedocument @hdoc OUTPUT,@TourTraffic
				INSERT INTO tbl_TourTraffic (TourId,TrafficId)
				SELECT @TourId,TrafficId FROM OPENXML(@hdoc,'/ROOT/Info') WITH (TrafficId INT)
				SET @errorcount=@errorcount+@@ERROR
				EXEC sp_xml_removedocument @hdoc	
			END
		END
	END


	IF(@TourType=1)--团队计划
	BEGIN
		DECLARE @SellerId INT--销售员编号
		DECLARE @SellerName NVARCHAR(255)--销售员姓名
		--团队信息
		EXEC sp_xml_preparedocument @hdoc OUTPUT,@TeamPrivate
		SELECT @SellerId=SaleId,@SellerName=Saler FROM [tbl_Customer] WHERE Id=(SELECT A.BuyerCId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(BuyerCId INT) AS A)
		UPDATE [tbl_Tour] SET [LeaveDate] = @LDate
			,[RDate] = DATEADD(DAY,@TourDays-1,@LDate)
			,[AreaId] = @AreaId
			,[TourCode] = @TourCode
			,[RouteId] = @RouteId
			,[RouteName] = @RouteName
			,[TourDays] = @TourDays
			,[PlanPeopleNumber] = @PlanPeopleNumber
			,[LTraffic] = @LTraffic
			,[RTraffic] = @RTraffic
			,[Gather] = @Gather
			,[OperatorId] = @OperatorId
			,[SellerId] = @SellerId
			,[CompanyId] = @CompanyId
			,[TotalAmount] = A.TotalAmount
			,[TotalInCome]=A.TotalAmount
			,[LastTime] = GETDATE()
			,[GatheringTime]=@GatheringTime
			,[GatheringPlace]=@GatheringPlace
			,[GatheringSign]=@GatheringSign
		FROM [tbl_Tour] AS B,(SELECT SellerId,TotalAmount FROM OPENXML(@hdoc,'/ROOT/Info')
		WITH(SellerId INT,TotalAmount MONEY)) A
		WHERE B.[TourId]=@TourId
		SET @errorcount=@errorcount+@@ERROR
		EXEC sp_xml_removedocument @hdoc
		IF(@errorcount=0)--团队操作员计调员信息
		BEGIN
			DELETE FROM [tbl_TourOperator] WHERE TourId=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				/*INSERT INTO [tbl_TourOperator]([TourId],[OperatorId]) SELECT @TourId,A.UserId FROM (SELECT C.UserId FROM tbl_UserArea AS C WHERE C.AreaId=@AreaId AND EXISTS(SELECT 1 FROM tbl_CompanyUser AS D WHERE D.Id=C.UserId AND D.UserType=2)) AS A*/
				INSERT INTO [tbl_TourOperator]([TourId],[OperatorId]) VALUES (@TourId,@CoordinatorId)
				SET @errorcount=@errorcount+@@ERROR
			END
		END
		IF(@errorcount=0 AND @LocalAgencys IS NOT NULL)--地接社信息
		BEGIN
			DELETE FROM [tbl_TourLocalAgency] WHERE TourId=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				EXEC sp_xml_preparedocument @hdoc OUTPUT,@LocalAgencys
				INSERT INTO [tbl_TourLocalAgency]([TourId],[AgencyId],[Name],[LicenseNo],[Telephone],[ContacterName])
				SELECT @TourId,A.AgencyId,A.[Name],A.LicenseNo,A.Telephone,A.ContacterName FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH(AgencyId INT,[Name] NVARCHAR(255),LicenseNo NVARCHAR(255),Telephone NVARCHAR(255),ContacterName NVARCHAR(255)) AS A
				SET @errorcount=@errorcount+@@ERROR
				EXEC sp_xml_removedocument @hdoc
			END
		END
		IF(@errorcount=0 AND @Attachs IS NOT NULL)--附件信息
		BEGIN
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@Attachs
			--删除的附件
			INSERT INTO [tbl_SysDeletedFileQue]([FilePath]) SELECT A.[FilePath] FROM [tbl_TourAttach] AS A WHERE A.TourId=@TourId AND A.[FilePath] NOT IN(SELECT B.FilePath FROM OPENXML(@hdoc,'/ROOT/Info') WITH(FilePath NVARCHAR(255)) AS B)
			DELETE FROM [tbl_TourAttach] WHERE TourId=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				INSERT INTO [tbl_TourAttach]([TourId],[FilePath],[Name])
				SELECT @TourId,A.FilePath,A.[Name] FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH(FilePath NVARCHAR(255),[Name] NVARCHAR(255)) AS A
				SET @errorcount=@errorcount+@@ERROR
			END
			EXEC sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @ReleaseType='0')--标准发布行程信息
		BEGIN
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@Plans
			INSERT INTO [tbl_SysDeletedFileQue]([FilePath]) SELECT A.[FilePath] FROM [tbl_TourPlan] AS A WHERE A.TourId=@TourId AND A.[FilePath] NOT IN(SELECT B.FilePath FROM OPENXML(@hdoc,'/ROOT/Info') WITH(FilePath NVARCHAR(255)) AS B)
			DELETE FROM [tbl_TourPlan] WHERE TourId=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				INSERT INTO [tbl_TourPlan]([TourId],[Interval],[Vehicle],[Hotel],[Dinner],[Plan],[FilePath])
				SELECT @TourId,A.Interval,A.Vehicle,A.Hotel,A.Dinner,A.[Plan],A.FilePath FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH(Interval NVARCHAR(255),Vehicle NVARCHAR(255),Hotel NVARCHAR(255),Dinner NVARCHAR(255),[Plan] NVARCHAR(MAX),FilePath NVARCHAR(255)) AS A
				SET @errorcount=@errorcount+@@ERROR
			END
			EXEC sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @ReleaseType='1')--快速发布行程信息等
		BEGIN
			DELETE FROM [tbl_TourQuickPlan] WHERE TourId=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				EXEC sp_xml_preparedocument @hdoc OUTPUT,@QuickPrivate
				INSERT INTO [tbl_TourQuickPlan]([TourId],[Plan],[Service],[Remark])
				SELECT @TourId,A.[Plan],A.[Service],A.Remark FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH([Plan] NVARCHAR(MAX),[Service] NVARCHAR(MAX),Remark NVARCHAR(MAX)) AS A
				SET @errorcount=@errorcount+@@ERROR
				EXEC sp_xml_removedocument @hdoc
			END
		END
		IF(@errorcount=0 AND @NormalPrivate IS NOT NULL)--标准发布不含项目等
		BEGIN
			DELETE FROM [tbl_TourService] WHERE TourId=@TourId AND [Key] IN('BuHanXiangMu','ErTongAnPai','GouWuAnPai','NeiBuXingXi','WenXinTiXing','ZhuYiShiXiang','ZiFeiXIangMu')
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				EXEC sp_xml_preparedocument @hdoc OUTPUT,@NormalPrivate
				INSERT INTO [tbl_TourService]([TourId],[Key],[Value])
				SELECT @TourId,A.[Key],A.[Value] FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH([Key] NVARCHAR(50),[Value] NVARCHAR(MAX)) AS A
				SET @errorcount=@errorcount+@@ERROR
				EXEC sp_xml_removedocument @hdoc
			END
		END
		IF(@errorcount=0 AND @Service IS NOT NULL)--价格组成
		BEGIN
			DELETE FROM [tbl_TourTeamPrice] WHERE TourId=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				EXEC sp_xml_preparedocument @hdoc OUTPUT,@Service
				INSERT INTO [tbl_TourTeamPrice]([TourId],[ItemType],[Standard],[LocalPrice],[MyPrice],[Requirement],[Remark],[LocalPeopleNumber],[LocalUnitPrice],[SelfPeopleNumber],[SelfUnitPrice])
				SELECT @TourId,A.[Type],A.[Service],A.LocalPrice,A.SelfPrice,NULL,NULL,[LocalPeopleNumber],[LocalUnitPrice],[SelfPeopleNumber],[SelfUnitPrice] FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH([Type] TINYINT,[Service] NVARCHAR(MAX),LocalPrice MONEY,SelfPrice MONEY,[LocalPeopleNumber] INT,[LocalUnitPrice] MONEY,[SelfPeopleNumber] INT,[SelfUnitPrice] MONEY) AS A
				SET @errorcount=@errorcount+@@ERROR
				EXEC sp_xml_removedocument @hdoc
			END
		END
		
		DECLARE @ChildPrice MONEY
		DECLARE @PersonalPrice MONEY
		DECLARE @AdultNumber INT
		DECLARE @ChildNumber INT 
		DECLARE @NumberQP INT
		SET @ChildPrice = 0
		SET @PersonalPrice = 0
		SET @AdultNumber = 0
		SET @ChildNumber = 0
		SET @NumberQP = 0
		if @errorcount = 0 and @TourTeamUnit is not null  --团队计划增加人数和价格信息
		begin
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@TourTeamUnit
			select @AdultNumber = a.NumberCr,@ChildNumber = a.NumberEt,@NumberQP = a.NumberQp
				,@PersonalPrice = a.UnitAmountCr,@ChildPrice = a.UnitAmountEt from openxml(@hdoc,'/ROOT/Info')
			with(NumberCr int,NumberEt int,NumberQp int
				,UnitAmountCr money,UnitAmountEt money) as a
			EXEC sp_xml_removedocument @hdoc
		END
		SET @AdultNumber = ISNULL(@AdultNumber,0) + ISNULL(@NumberQP,0)
		IF @AdultNumber = 0 AND @ChildNumber = 0
		BEGIN
			SET @AdultNumber = @PlanPeopleNumber
		END
		
		--订单
		SELECT @OrderId=Id FROM tbl_TourOrder WHERE TourId=@TourId
		EXEC sp_xml_preparedocument @hdoc OUTPUT,@TeamPrivate
		UPDATE [tbl_TourOrder] SET [AreaId] = @AreaId
			,[RouteId] = @RouteId
			,[RouteName] = @RouteName
			,[TourNo] = @TourCode
			,[LeaveDate] = @LDate
			,[LeaveTraffic] = @LTraffic
			,[ReturnTraffic] = @RTraffic
			,[BuyCompanyID] = A.BuyerCId
			,[BuyCompanyName] = A.BuyerCName			
			,[SalerName] = @SellerName
			,[SalerId] = @SellerId
			,[OperatorName] = @OperatorName
			,[OperatorID] = @OperatorId
			,[PeopleNumber] = @PlanPeopleNumber
			,[SumPrice] = A.TotalAmount
			,[SellCompanyName] = @CompanyName
			,[SellCompanyId] = @CompanyId
			,[LastDate] = GETDATE()
			,[LastOperatorID] = @OperatorId
			,[FinanceSum] = A.TotalAmount+FinanceAddExpense-FinanceRedExpense
			,[ViewOperatorId] = @OperatorId
			,[TourDays] = @TourDays			
			,[AdultNumber]=@AdultNumber
			,[ChildNumber] = @ChildNumber
			,[PersonalPrice] = @PersonalPrice
			,[ChildPrice] = @ChildPrice
		FROM [tbl_TourOrder] AS B,(
			SELECT OrderId,BuyerCId,BuyerCName,ContactName,ContactTelephone,SellerName,SellerId,TotalAmount FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(OrderId CHAR(36),SellerId INT,SellerName NVARCHAR(255),TotalAmount MONEY,BuyerCId INT,BuyerCName NVARCHAR(255),ContactName NVARCHAR(100),ContactTelephone NVARCHAR(50))
		) A
		WHERE B.Id=@OrderId
		SET @errorcount=@errorcount+@@ERROR
		EXEC sp_xml_removedocument @hdoc
		IF(@errorcount=0 AND @SentPeoples IS NOT NULL)--送团人信息
		BEGIN
			DELETE FROM [tbl_TourSentTask] WHERE [TourId]=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				EXEC sp_xml_preparedocument @hdoc OUTPUT,@SentPeoples
				INSERT INTO [tbl_TourSentTask]([TourId],[OperatorId])
				SELECT @TourId,A.[UID] FROM OPENXML(@hdoc,'/ROOT/Info') WITH([UID] INT) AS A
				SET @errorcount=@errorcount+@@ERROR
				EXEC sp_xml_removedocument @hdoc
			END
		END	
		IF(@errorcount=0 AND @TourCityId IS NOT NULL AND @TourCityId>0)--出港城市
		BEGIN
			DELETE FROM [tbl_TourCity] WHERE [TourId]=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				INSERT INTO [tbl_TourCity]([TourId],[CityId]) VALUES(@TourId,@TourCityId)
				SET @errorcount=@errorcount+@@ERROR
			END
		END
		if @errorcount = 0 and @TourTeamUnit is not null  --团队计划增加人数和价格信息
		begin
			DELETE FROM [tbl_TourTeamUnit] WHERE [TourId]=@TourId
			SET @errorcount = @errorcount+@@ERROR
			IF @errorcount = 0
			BEGIN
				EXEC sp_xml_preparedocument @hdoc OUTPUT,@TourTeamUnit
				INSERT INTO [tbl_TourTeamUnit] ([TourId],[NumberCR],[NumberET],[NumberQP]
					,[UnitAmountCR],[UnitAmountET],[UnitAmountQP])
				select @TourId,a.NumberCr,a.NumberEt,a.NumberQp
					,a.UnitAmountCr,a.UnitAmountEt,a.UnitAmountQp from openxml(@hdoc,'/ROOT/Info')
				with(NumberCr int,NumberEt int,NumberQp int
					,UnitAmountCr money,UnitAmountEt money,UnitAmountQp money) as a
				SET @errorcount=@errorcount + @@ERROR
				EXEC sp_xml_removedocument @hdoc
			end
		end
		IF(@errorcount=0)--是否出团
		BEGIN
			UPDATE [tbl_Tour] SET IsLeave='1' WHERE TourId=@TourId AND LeaveDate<=GETDATE()
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0)--团队状态维护
		BEGIN
			--标记团队状态为行程途中
			UPDATE tbl_Tour SET Status=2 WHERE TourType IN(0,1) AND [Status] IN(0,1,3) AND DATEDIFF(DAY,GETDATE(),[LeaveDate])<=0 AND DATEDIFF(DAY,GETDATE(),[RDate])>0 AND TourId=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)--标记团队状态为回团报账
			BEGIN
				UPDATE tbl_Tour SET Status=3 WHERE TourType IN(0,1) AND [Status] IN(0,1,2) AND DATEDIFF(DAY,GETDATE(),[RDate])<0 AND TourId=@TourId
				SET @errorcount=@errorcount+@@ERROR
			END
			IF(@errorcount=0)
			BEGIN
				UPDATE [tbl_Tour] SET [Status]=2 WHERE [TourId]=@TourId AND [Status] =3 AND DATEDIFF(DAY,GETDATE(),[RDate])>0
				SET @errorcount=@errorcount+@@ERROR
			END
		END
		IF @errorcount = 0 AND @TourTraffic IS NOT NULL  --团队交通信息
		BEGIN
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@TourTraffic
			--团队交通信息
			DELETE FROM tbl_TourTraffic WHERE TourId = @TourId
			SET @errorcount=@errorcount+@@ERROR
			IF @errorcount = 0
			BEGIN
				INSERT INTO tbl_TourTraffic (TourId,TrafficId)
				SELECT @TourId,TrafficId FROM OPENXML(@hdoc,'/ROOT/Info') WITH (TrafficId INT)
				SET @errorcount=@errorcount+@@ERROR	
			END
			--订单交通信息
			DELETE FROM tbl_TourOrderTraffic WHERE OrderId = @OrderId
			SET @errorcount=@errorcount+@@ERROR
			IF @errorcount = 0
			BEGIN
				INSERT INTO tbl_TourOrderTraffic (OrderId,TrafficId)
				SELECT TOP 1 @OrderId,TrafficId FROM OPENXML(@hdoc,'/ROOT/Info') WITH (TrafficId INT)
				SET @errorcount=@errorcount+@@ERROR
			END
			EXEC sp_xml_removedocument @hdoc
		END
	END


	IF(@TourType=2)--单项服务
	BEGIN
		--团队信息
		EXEC sp_xml_preparedocument @hdoc OUTPUT,@SinglePrivate
		UPDATE [tbl_Tour] SET [OperatorId] = @OperatorId
			,[SellerId] = A.SellerId
			,[TotalAmount] = A.TotalAmount
			,[LastTime] = GETDATE()
			,[GrossProfit] = A.GrossProfit
			,[TotalOutAmount]=A.TotalOutAmount
			,[TourCode]=@TourCode
			,[PlanPeopleNumber]=@PlanPeopleNumber
			,[LeaveDate]=@LDate
			,[RDate]=@LDate
		FROM tbl_Tour AS B,(SELECT SellerId,TotalAmount,GrossProfit,TotalOutAmount FROM OPENXML(@hdoc,'/ROOT/Info')
		WITH(SellerId INT,TotalAmount MONEY,GrossProfit MONEY,TotalOutAmount MONEY)) A
		WHERE B.TourId=@TourId
		SET @errorcount=@errorcount+@@ERROR
		EXEC sp_xml_removedocument @hdoc
		IF(@errorcount=0 AND @Service IS NOT NULL)--价格组成
		BEGIN
			DELETE FROM [tbl_TourTeamPrice] WHERE TourId=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				EXEC sp_xml_preparedocument @hdoc OUTPUT,@Service
				INSERT INTO [tbl_TourTeamPrice]([TourId],[ItemType],[Standard],[LocalPrice],[MyPrice],[Requirement],[Remark])
				SELECT @TourId,A.[Type],NULL,A.LocalPrice,A.SelfPrice,A.[Service],A.Remark FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH([Type] TINYINT,LocalPrice MONEY,SelfPrice MONEY,[Service] NVARCHAR(MAX),Remark NVARCHAR(MAX)) AS A
				SET @errorcount=@errorcount+@@ERROR
				EXEC sp_xml_removedocument @hdoc
			END
		END
		--订单
		SELECT @OrderId=[Id] FROM [tbl_TourOrder] WHERE [TourId]=@TourId
		EXEC sp_xml_preparedocument @hdoc OUTPUT,@SinglePrivate		
		UPDATE [tbl_TourOrder] SET [AreaId] = @AreaId
			,[RouteId] = @RouteId
			,[RouteName] = @RouteName
			,[TourNo] = @TourCode
			,[LeaveTraffic] = @LTraffic
			,[ReturnTraffic] = @RTraffic
			,[BuyCompanyID] = A.BuyerCId
			,[BuyCompanyName] = A.BuyerCName
			,[SalerName] = A.SellerName
			,[SalerId] = A.SellerId
			,[OperatorName] = @OperatorName
			,[OperatorID] = @OperatorId
			,[PeopleNumber] = @PlanPeopleNumber
			,[SumPrice] = A.TotalAmount
			,[SellCompanyName] = @CompanyName
			,[SellCompanyId] = @CompanyId
			,[LastDate] = GETDATE()
			,[LastOperatorID] = @OperatorId
			,[FinanceSum] = A.TotalAmount+FinanceAddExpense-FinanceRedExpense
			,[ViewOperatorId] = @OperatorId
			,[CustomerDisplayType] = @CustomerDisplayType
			,[CustomerFilePath] = @CustomerFilePath
			,[TourDays] = @TourDays
			,[ContactName]=A.ContactName
			,[ContactTel]=A.ContactTelephone
			,[ContactMobile]=A.ContactMobile			
			,[AdultNumber]=@PlanPeopleNumber
			,[LeaveDate]=@LDate
		FROM tbl_TourOrder AS B,(
			SELECT OrderId,SellerId,SellerName,TotalAmount,BuyerCId,BuyerCName,ContactName,ContactTelephone,ContactMobile FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(OrderId CHAR(36),SellerId INT,SellerName NVARCHAR(255),TotalAmount MONEY,BuyerCId INT,BuyerCName NVARCHAR(255),ContactName NVARCHAR(100),ContactTelephone NVARCHAR(50),ContactMobile NVARCHAR(50))
		) A
		WHERE B.Id=@OrderId
		SET @errorcount=@errorcount+@@ERROR
		EXEC sp_xml_removedocument @hdoc
		--供应商安排处理
		EXEC sp_xml_preparedocument @hdoc OUTPUT,@PlansSingle		
		IF(@errorcount=0 AND @PlansSingle IS NOT NULL)--删除要删除的供应商安排
		BEGIN
			--删除对应的供应商安排
			DELETE FROM [tbl_PlanSingle] WHERE [TourId]=@TourId AND [PlanId] NOT IN(SELECT A.PlanId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(PlanId CHAR(36)) AS A)
			SET @errorcount=@errorcount+@@ERROR
			--删除统计分析支出对账单对应的安排
			DELETE FROM [tbl_StatAllOut] WHERE [TourId]=@TourId AND [ItemType]=4 AND [ItemId] NOT IN(SELECT A.PlanId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(PlanId CHAR(36)) AS A)
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0 AND @PlansSingle IS NOT NULL)--修改要更新的供应商安排
		BEGIN
			UPDATE [tbl_PlanSingle] SET [ServiceType] = C.ServiceType
				,[SupplierId] = C.SupplierId
				,[SupplierName] = C.SupplierName
				,[Arrange] = C.Arrange
				,[Amount] = C.Amount
				,[Remark] = C.Remark
				,[OperatorId] = @OperatorId
			FROM [tbl_PlanSingle] AS B INNER JOIN(
				SELECT A.PlanId,A.ServiceType,A.SupplierId,A.SupplierName,A.Arrange,A.Amount,A.Remark
				FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH(PlanId CHAR(36),ServiceType TINYINT,SupplierId INT,SupplierName NVARCHAR(255),Arrange NVARCHAR(MAX),Amount MONEY,Remark NVARCHAR(MAX)) AS A
			)C ON B.PlanId=C.PlanId		
			SET @errorcount=@errorcount+@@ERROR	
			UPDATE [tbl_StatAllOut] SET [Amount]=C.Amount
			FROM [tbl_StatAllOut] AS B INNER JOIN(
				SELECT A.PlanId,A.Amount
				FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH(PlanId CHAR(36),Amount MONEY) AS A
			)C ON B.[TourId]=@TourId AND B.[ItemType]=4 AND B.[ItemId]=C.PlanId
			SET @errorcount=@errorcount+@@ERROR
			UPDATE [tbl_StatAllOut] SET [TotalAmount]=B.[Amount]+B.AddAmount-B.ReduceAmount
			FROM [tbl_StatAllOut] AS B INNER JOIN(
				SELECT A.PlanId
				FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH(PlanId CHAR(36)) AS A
			)C ON B.[TourId]=@TourId AND B.[ItemType]=4 AND B.[ItemId]=C.PlanId
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0 AND @PlansSingle IS NOT NULL)--写入要新增的供应商安排
		BEGIN
			INSERT INTO [tbl_PlanSingle]([PlanId],[TourId],[ServiceType],[SupplierId],[SupplierName]
				,[Arrange],[Amount],[Remark],[OperatorId],[CreateTime]
				,[AddAmount],[ReduceAmount],[TotalAmount],[FRemark])
			SELECT A.PlanId,@TourId,A.ServiceType,A.SupplierId,A.SupplierName
				,A.Arrange,A.Amount,A.Remark,@OperatorId,GETDATE()
				,0,0,A.Amount,''
			FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(PlanId CHAR(36),ServiceType TINYINT,SupplierId INT,SupplierName NVARCHAR(255),Arrange NVARCHAR(MAX),Amount MONEY,Remark NVARCHAR(MAX)) AS A
			WHERE NOT EXISTS(SELECT 1 FROM [tbl_PlanSingle] WHERE TourId=@TourId AND [PlanId]=A.PlanId)


			SET @errorcount=@errorcount+@@ERROR	
		END
		IF(@errorcount=0 AND @PlansSingle IS NOT NULL)--写新增的供应商安排支出
		BEGIN
			INSERT INTO [tbl_StatAllOut]([CompanyId],[TourId],[AreaId],[TourType],[ItemId]
				,[ItemType],[Amount],[AddAmount],[ReduceAmount],[TotalAmount]
				,[OperatorId],[DepartmentId],[CreateTime],[HasCheckAmount],[NotCheckAmount]
				,[IsDelete],[SupplierId],[SupplierName])
			SELECT @CompanyId,@TourId,@AreaId,@TourType,A.PlanId
				,4,A.Amount,0,0,A.Amount
				,@OperatorId,@DepartmentId,GETDATE(),0,0
				,'0',A.SupplierId,A.SupplierName
			FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(PlanId CHAR(36),Amount MONEY,SupplierId INT,SupplierName NVARCHAR(255)) AS A
			WHERE NOT EXISTS(SELECT 1 FROM [tbl_StatAllOut] WHERE TourId=@TourId AND [ItemId]=A.PlanId AND ItemType=4)
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0 AND @PlansSingle IS NOT NULL)--更新支出合计金额信息
		BEGIN
			UPDATE [tbl_PlanSingle] SET [TotalAmount]=[Amount]+[AddAmount]-[ReduceAmount] WHERE [TourId]=@TourId AND [PlanId] IN(SELECT A.PlanId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(PlanId CHAR(36)) AS A)
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN 
				UPDATE [tbl_StatAllOut] SET [TotalAmount]=[Amount]+[AddAmount]-[ReduceAmount] WHERE [TourId]=@TourId AND [ItemType]=4 AND [ItemId] IN(SELECT A.PlanId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(PlanId CHAR(36)) AS A)
			END
		END
		EXEC sp_xml_removedocument @hdoc
		--游客处理
		EXEC sp_xml_preparedocument @hdoc OUTPUT,@Customers
		IF(@errorcount=0)--删除要删除的游客信息
		BEGIN
			DELETE FROM [tbl_TourOrderCustomer] WHERE [OrderId]=@OrderId AND [Id] NOT IN(SELECT A.TravelerId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TravelerId CHAR(36)) AS A)
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0 AND @Customers IS NOT NULL)--更新要修改的游客信息
		BEGIN
			UPDATE [tbl_TourOrderCustomer] SET [VisitorName] = C.TravelerName
				,[CradType] = C.IdType
				,[CradNumber] = C.IdNo
				,[Sex] = C.Gender
				,[VisitorType] = C.TravelerType
				,[ContactTel] = C.ContactTel
			FROM [tbl_TourOrderCustomer] AS B INNER JOIN(
				SELECT A.TravelerId,A.TravelerName,A.IdType,A.IdNo,A.Gender,A.TravelerType,A.ContactTel FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH(TravelerId CHAR(36),TravelerName NVARCHAR(100),IdType TINYINT,IdNo NVARCHAR(255),Gender TINYINT,TravelerType TINYINT,ContactTel NVARCHAR(100)) AS A
			)C ON B.Id=C.TravelerId
			WHERE B.OrderId=@OrderId
		END
		IF(@errorcount=0 AND @Customers IS NOT NULL)--写入要新增的游客信息
		BEGIN			
			INSERT INTO [tbl_TourOrderCustomer]([ID],[OrderId],[CompanyID],[VisitorName],[CradType]
				,[CradNumber],[Sex],[VisitorType],[ContactTel],[IssueTime]
				,[CustomerStatus],[TourId])
			SELECT A.TravelerId,@OrderId,@CompanyId,A.TravelerName,A.IdType
				,A.IdNo,A.Gender,A.TravelerType,A.ContactTel,GETDATE()
				,0,@TourId FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(TravelerId CHAR(36),TravelerName NVARCHAR(100),IdType TINYINT,IdNo NVARCHAR(255),Gender TINYINT,TravelerType TINYINT,ContactTel NVARCHAR(100)) AS A
			WHERE NOT EXISTS(SELECT 1 FROM [tbl_TourOrderCustomer] WHERE OrderId=@OrderId AND [Id]=A.TravelerId)
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0 AND @Customers IS NOT NULL)--特服信息
		BEGIN
			DELETE FROM tbl_CustomerSpecialService WHERE CustormerId IN(SELECT A.TravelerId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TravelerId CHAR(36)) AS A)
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				INSERT INTO [tbl_CustomerSpecialService]([CustormerId],[ProjectName],[ServiceDetail],[IsAdd],[Fee],[IssueTime])
				SELECT A.TravelerId,A.TF_XiangMu,A.TF_NeiRong,A.TF_ZengJian,A.TF_FeiYong,GETDATE() FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH(TravelerId CHAR(36),TF_XiangMu NVARCHAR(255),TF_NeiRong NVARCHAR(500),TF_ZengJian TINYINT,TF_FeiYong MONEY) AS A
				SET @errorcount=@errorcount+@@ERROR
			END
		END
		EXEC sp_xml_removedocument @hdoc
	END
	
	IF(@errorcount=0 AND @CRouteId>0)--线路上团数维护
	BEGIN
		UPDATE [tbl_Route] SET [TourCount]=(SELECT COUNT(*) FROM [tbl_Tour] WHERE [RouteId]=@CRouteId AND TemplateId<>'' AND [IsDelete]='0') WHERE [Id]=@CRouteId
	END
	IF(@errorcount=0 AND @RouteId>0)
	BEGIN
		UPDATE [tbl_Route] SET [TourCount]=(SELECT COUNT(*) FROM [tbl_Tour] WHERE [RouteId]=@RouteId AND TemplateId<>'' AND [IsDelete]='0') WHERE [Id]=@RouteId
	END

	IF(@errorcount>0)
	BEGIN
		ROLLBACK TRAN
		SET @Result=0
	END
	ELSE
	BEGIN		
		COMMIT TRAN
		SET @Result=1
	END
END



go



GO

-- =============================================
-- Author:		luofx
-- Create date: 2011-01-24
-- Description:	新增订单
-- History:
-- 1.增加组团联系人信息、返佣信息 汪奇志 2011-06-08
-- 2.修正人数验证（减去已退团人数）
-- 3.2011-12-20 汪奇志 增加对销售员（用于人次统计）字段的处理
-- 4.2012-03-01 汪奇志 增加组团社团号
-- 5.2012-09-11 周文超 增加订单交通信息
-- =============================================
ALTER PROCEDURE [dbo].[proc_TourOrder_Insert]	
	@OrderID CHAR(36),                --订单编号
	@TourId CHAR(36),	             --团队编号
	@AreaId INT,			             --线路区域编号
	@RouteId INT,		             --线路编号
	@RouteName NVARCHAR(500),         --线路名称
	@TourNo NVARCHAR(100),	         --团号
	@TourClassId TINYINT,	         --团队类型(散客，团队)
	@LeaveDate DATETIME,              --出团日期
	@TourDays INT,                    --团队天数
	@LeaveTraffic NVARCHAR(250),      --出发交通
	@ReturnTraffic NVARCHAR(250),     --返程交通
	@OrderType TINYINT,               --订单来源  0:组团下单；1：代客预定
	@OrderState TINYINT,              --订单状态
	@BuyCompanyID INT,	             --购买公司编号
	@BuyCompanyName NVARCHAR(250),    --购买公司名称
	@ContactName NVARCHAR(250),	     --联系人
	@ContactTel NVARCHAR(250),	     --联系人电话
	@ContactMobile NVARCHAR(100),     --联系人手机
	@ContactFax NVARCHAR(100),        --传真  
	@OperatorName NVARCHAR(100),      --下单人姓名
	@OperatorID INT,                  --下单人编号
	@PriceStandId INT,                --报价等级编号
	@PersonalPrice MONEY,             --成人价
	@ChildPrice MONEY,                --儿童价
	@MarketPrice MONEY,               --单房差
	@AdultNumber INT,                 --成人数
	@ChildNumber INT,                 --儿童数
	@MarketNumber INT,                --单房差数
	@PeopleNumber INT,                --总人数
	@OtherPrice MONEY,                --其他费用
	@SaveSeatDate DATETIME,           --留位日期
	@OperatorContent NVARCHAR(1000),  --操作说明
	@SpecialContent NVARCHAR(1000),   --特殊留言
	@SumPrice MONEY,                  --订单中金额
	@SellCompanyName NVARCHAR(100),   --专线公司名称
	@SellCompanyId INT,               --专线公司编号
	@LastDate DATETIME,               --最后修改日期
	@LastOperatorID INT,              --最后修改人
	@ViewOperatorId INT,              --操作人或发布计划人(组团下单及团队订单时存放发布计划人 专线报名时存放当前登录人)
	@CustomerDisplayType TINYINT,     --游客信息体现类型（附件方式、输入方式）
	@CustomerFilePath NVARCHAR(500),  --游客信息附件
	@CustomerLevId INT,               --客户等级编号
	@IsTourOrderEdit INT,             --1：来自组团端的订单修改，0：来自专线端的订单修改
	@Result INT OUTPUT,				 --回传参数:0:失败；1:成功;2：该团队的订单总人数+当前订单人数大于团队计划人数总和；3：该客户所欠金额大于最高欠款金额；
	@TourCustomerXML NVARCHAR(MAX),   --游客XML详细信息:<ROOT><TourOrderCustomer ID="" CompanyId="" OrderId="" VisitorName="" CradType="" CradNumber="" Sex="" VisitorType"" ContactTel="" ProjectName="" ServiceDetail="" IsAdd="" Fee=""/></ROOT> 
	@AmountPlus NVARCHAR(MAX)=NULL--订单金额增加减少费用信息 XML:<ROOT><Info AddAmount="增加费用" ReduceAmount="减少费用" Remark="备注" /></ROOT>
	,@BuyerContactId INT--组团联系人编号
	,@BuyerContactName NVARCHAR(50)--组团联系人姓名
	,@CommissionType TINYINT--返佣类型
	,@CommissionPrice MONEY--返佣金额
	,@BuyerTourCode NVARCHAR(255)--组团社团号
	,@OrderTraffic INT --订单交通编号
AS
	DECLARE @ErrorCount INT              --错误累计参数
	DECLARE @hdoc INT                    --XML使用参数
	DECLARE @TmpResult INT               --临时暂存返回数
	DECLARE @OrderNO VARCHAR(100)        --订单号
	DECLARE @SalerName NVARCHAR(100)         --销售人姓名
	DECLARE @SalerId INT                     --销售人ID
	DECLARE @MaxDebts MONEY              --最高欠款金额
	DECLARE @PeopleNumberCount INT       --团队订单总人数:已成交人数+已留位订单人数
	DECLARE @PlanPeopleNumber INT        --团队人数
	DECLARE @VirtualPeopleNumber INT     --虚拟实收人数
	DECLARE @RealVirtualPeopleNumber INT     --用作修改虚拟实收的临时参数
	DECLARE @ShiShu INT --实际订单人数
	DECLARE @TrafficPeopleNum INT --交通当天的票数
	DECLARE @TrafficYiShiYong INT --交通当天已使用的票数
	SET @ErrorCount=0
	SET @Result=1
BEGIN
	DECLARE @PerTimeSellerId INT--销售员（用于人次统计）
	SET @PerTimeSellerId=@OperatorID
	--判断订单人数是否大于剩余人数
	SELECT @VirtualPeopleNumber=VirtualPeopleNumber,@PlanPeopleNumber=PlanPeopleNumber FROM tbl_Tour WHERE TourId=@TourId  AND IsDelete=0
	SELECT @ShiShu = ISNULL(SUM(PeopleNumber - LeaguePepoleNum),0) FROM tbl_TourOrder WHERE IsDelete=0 AND TourId=@TourId AND OrderState IN(1,2,5)
	--交通出团日期当天的人数
	SELECT @TrafficPeopleNum = TicketNums FROM tbl_TrafficPrices WHERE TrafficId = @OrderTraffic AND DATEDIFF(dd,SDateTime,@LeaveDate) = 0 AND [Status] = 0
	IF @TrafficPeopleNum IS NULL
	BEGIN
		SET @TrafficPeopleNum = 0
	END
	--交通出团日期当天已使用的票数
	SELECT @TrafficYiShiYong = ISNULL(SUM(PeopleNumber - LeaguePepoleNum),0) FROM tbl_TourOrder AS a WHERE a.IsDelete='0' AND a.OrderState IN(1,2,5) AND DATEDIFF(dd,a.LeaveDate,@LeaveDate) = 0 AND EXISTS (SELECT 1 FROM tbl_TourOrderTraffic AS b WHERE b.OrderId = a.ID AND b.TrafficId = @OrderTraffic)
	
	--订单人数 + 交通出团日期当天已使用票数 大于 交通出团日期当天的人数  不允许修改
	IF @PeopleNumber + @TrafficYiShiYong > @TrafficPeopleNum
	BEGIN
		SET @Result=4
		RETURN @Result
	END
	
	--该团所有订单人数大于团队计划人数，则不允许修改
	if @PlanPeopleNumber - @ShiShu - @PeopleNumber < 0
	begin
		SET @Result=2
		RETURN @Result
	END
	
	
	--留位和成交时，验证是否达到该客户的最高欠款金额 
	IF(@OrderState=2 OR @OrderState=5)
	BEGIN 
		SELECT @MaxDebts=MaxDebts FROM tbl_Customer WHERE [ID]=@BuyCompanyID	 
		--该客户的最高欠款金额-本订单金额<已成交+已留位的订单的 财务小计-已收已审核金额 的总和
		IF((@MaxDebts-@SumPrice)<(SELECT SUM(FinanceSum-HasCheckMoney) FROM tbl_TourOrder WHERE BuyCompanyID=@BuyCompanyID  AND IsDelete=0 AND TourId=@TourId AND (OrderState=5 OR OrderState=2)))
		BEGIN
			SET @Result=3
		END
		ELSE--更新统计分析所有收入明细表
		BEGIN			
			EXEC [proc_StatAllIncome_AboutDelete] @OrderID,0
		END
	END
	ELSE--不受理时，更新统计分析所有收入明细表
	BEGIN		
		EXEC [proc_StatAllIncome_AboutDelete] @OrderID,1			
	END
	BEGIN TRANSACTION TourOrder_Insert		
	IF(@Result=1)
	BEGIN
		IF((@BuyerContactName IS NULL OR LEN(@BuyerContactName)<1) AND @BuyerContactId>0)
		BEGIN
			SELECT @BuyerContactName=[Name] FROM [tbl_CustomerContactInfo] WHERE [Id]=@BuyerContactId
		END
		--生成订单号
		SET @OrderNO=dbo.fn_TourOrder_CreateOrderNo()
		--查找责任销售
		SELECT @SalerId=SaleId,@SalerName=Saler FROM tbl_Customer WHERE ID=@BuyCompanyID  AND IsDelete=0
		IF(@OrderType=0)
		BEGIN
			SET @PerTimeSellerId=@SalerId
		END
		--插入订单信息
		INSERT INTO [tbl_TourOrder] 
			([ID],[OrderNo],[TourId],[AreaId],[RouteId],[RouteName],[TourNo],[TourClassId]
				,[LeaveDate],[TourDays],[LeaveTraffic],[ReturnTraffic],[OrderType],[OrderState],[BuyCompanyID]
				,[BuyCompanyName],[ContactName],[ContactTel],[ContactMobile],[ContactFax],[SalerName]
				,[SalerId],[OperatorName],[OperatorID],[PriceStandId],[PersonalPrice]
				,[ChildPrice],[MarketPrice],[AdultNumber],[ChildNumber],[MarketNumber]
				,[PeopleNumber],[OtherPrice],[SaveSeatDate],[OperatorContent]
				,[SpecialContent],[SumPrice],[SellCompanyName],[SellCompanyId],[LastDate]
				,[LastOperatorID],[IssueTime],[ViewOperatorId],[CustomerDisplayType],[CustomerFilePath]
				,CustomerLevId,FinanceSum,SuccessTime,BuyerContactId,BuyerContactName
				,CommissionType,CommissionPrice,PerTimeSellerId,BuyerTourCode)
			VALUES(@OrderID,@OrderNO,@TourId,@AreaId,@RouteId,@RouteName,@TourNo,@TourClassId
				,@LeaveDate,@TourDays,@LeaveTraffic,@ReturnTraffic,@OrderType,@OrderState,@BuyCompanyID
				,@BuyCompanyName,@ContactName,@ContactTel,@ContactMobile,@ContactFax,@SalerName
				,@SalerId,@OperatorName, @OperatorID,@PriceStandId,@PersonalPrice
				,@ChildPrice,@MarketPrice,@AdultNumber,@ChildNumber,@MarketNumber
				,@PeopleNumber,@OtherPrice,@SaveSeatDate,@OperatorContent
				,@SpecialContent,@SumPrice,@SellCompanyName,@SellCompanyId,@LastDate
				,@LastOperatorID,getdate(),@ViewOperatorId,@CustomerDisplayType,@CustomerFilePath
				,@CustomerLevId,@SumPrice,getdate(),@BuyerContactId,@BuyerContactName
				,@CommissionType,@CommissionPrice,@PerTimeSellerId,@BuyerTourCode)
		SET @ErrorCount = @ErrorCount + @@ERROR	
		IF(LEN(@TourCustomerXML)>0 AND @ErrorCount=0)--插入游客信息
		BEGIN				
			EXECUTE  dbo.proc_TourOrderCustomer_Insert @TourCustomerXML,@OrderID,@Result OUTPUT	
			SET @ErrorCount = @ErrorCount + @@ERROR							
		END
		IF(@ErrorCount=0 AND @AmountPlus IS NOT NULL AND LEN(@AmountPlus)>0)--订单金额增加减少费用信息
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@AmountPlus
			INSERT INTO [tbl_TourOrderPlus]([OrderId],[AddAmount],[ReduceAmount],[Remark])
			SELECT @OrderId,A.AddAmount,A.ReduceAmount,A.Remark FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(AddAmount MONEY,ReduceAmount MONEY,Remark NVARCHAR(MAX)) AS A
			EXECUTE sp_xml_removedocument @hdoc
		END
		--订单交通信息
		IF @ErrorCount = 0
		BEGIN
			INSERT INTO tbl_TourOrderTraffic (OrderId,TrafficId) VALUES (@OrderID,@OrderTraffic)
			SET @ErrorCount = @ErrorCount + @@ERROR
		END
		
		--维护团队实收数=已有虚拟实收人数+订单总人数				
		UPDATE tbl_Tour SET VirtualPeopleNumber=@VirtualPeopleNumber+@PeopleNumber WHERE TourId=@TourId  AND IsDelete=0
		SET @ErrorCount = @ErrorCount + @@ERROR	
	END
	IF(@ErrorCount>0)
	BEGIN
		SET @Result=0
		ROLLBACK TRANSACTION TourOrder_Insert
	END
	COMMIT TRANSACTION TourOrder_Insert
	--更新是否收客收满
	UPDATE [tbl_Tour] SET [Status]=1  WHERE [Status]=0  AND IsDelete='0' AND PlanPeopleNumber=(SELECT ISNULL(SUM(PeopleNumber-LeaguePepoleNum),0) FROM tbl_TourOrder WHERE TourId=@TourId  AND IsDelete='0' AND OrderState NOT IN(3,4)) and TourId=@TourId 				
	RETURN  @Result
END



go




GO
-- =============================================
-- Author:		luofx
-- Create date: 2011-01-24
-- Description:	修改订单
-- History:
-- 1.增加组团联系人信息、返佣信息 汪奇志 2011-06-08
-- 2.增加修改团队计划订单同步修改团队计划信息，修正人数验证（减去已退团人数）
-- 3.修改客户单位(BuyCompanyName,BuyCompanyID,SalerId,SalerName,CommissionType)  曹胡生 2012-02-02
-- 4.2012-03-01 汪奇志 增加组团社团号
-- 5.2012-07-18 汪奇志 人数判断时减去当前订单的退团人数
-- 6.2012-09-11 周文超 增加订单交通信息 
-- =============================================
ALTER PROCEDURE [dbo].[proc_TourOrder_Update]	
	@OrderID CHAR(36),                --订单编号
	@TourId CHAR(36),	             --团队编号
	@OrderState TINYINT,              --订单状态
	@BuyCompanyID INT,	             --购买公司编号
	@BuyCompanyName nvarchar(250),	 --购买公司名称
	@SalerId int,					--责任销售员编号
	@SalerName nvarchar(100),		--责任销售员名称
	@PriceStandId INT,                --报价等级编号
	@PersonalPrice MONEY,             --成人价
	@ChildPrice MONEY,                --儿童价
	@MarketPrice MONEY,               --单房差
	@AdultNumber INT,                 --成人数
	@ChildNumber INT,                 --儿童数
	@MarketNumber INT,                --单房差数
	@PeopleNumber INT,                --总人数
	@OtherPrice Decimal(10,4),        --其他费用
	@SaveSeatDate DATETIME,           --留位日期
	@OperatorContent NVARCHAR(1000),  --操作说明
	@SpecialContent NVARCHAR(1000),   --特殊留言
	@SumPrice MONEY,          --订单中金额
	@LastDate DATETIME,               --最后修改日期
	@LastOperatorID INT,              --最后修改人
	@CustomerDisplayType TINYINT,     --游客信息体现类型（附件方式、输入方式）
	@CustomerFilePath NVARCHAR(500),  --游客信息附件
	@CustomerLevId INT,               --客户等级编号
	@ContactName NVARCHAR(250),	     --联系人
	@ContactTel NVARCHAR(250),	     --联系人电话
	@ContactMobile NVARCHAR(100),     --联系人手机
	@ContactFax NVARCHAR(100),        --传真  
	@IsTourOrderEdit INT,             --1：来自组团端的订单修改，0：来自专线端的订单修改
	@TourCustomerXML NVARCHAR(MAX),   --游客XML详细信息:<ROOT><TourOrderCustomer ID="" CompanyId="" OrderId="" VisitorName="" CradType="" CradNumber="" Sex="" VisitorType"" ContactTel="" ProjectName="" ServiceDetail="" IsAdd="" Fee=""/></ROOT> 
	@Result INT OUTPUT,			 --回传参数:0:失败；1:成功;2：该团队的订单总人数+当前订单人数大于团队计划人数总和；3：该客户所欠金额大于最高欠款金额；
	@AmountPlus NVARCHAR(MAX)=NULL--订单金额增加减少费用信息 XML:<ROOT><Info AddAmount="增加费用" ReduceAmount="减少费用" Remark="备注" /></ROOT>
	,@BuyerContactId INT--组团联系人编号
	,@BuyerContactName NVARCHAR(50)--组团联系人姓名
	,@CommissionPrice MONEY--返佣金额
	,@CommissionType TINYINT--返佣类型
	,@TourTeamUnit nvarchar(max)=NULL--团队计划增加人数和价格信息XML:<ROOT><Info NumberCr="成人数" NumberEt="儿童数" NumberQp="全陪数" UnitAmountCr="成人单价" UnitAmountEt="儿童单价" UnitAmountQp="全陪单价" /></ROOT>
	,@BuyerTourCode NVARCHAR(255)--组团社团号
	,@OrderTraffic INT --订单交通编号
AS
	DECLARE @ErrorCount INT   --错误累计参数
	DECLARE @hdoc INT         --XML使用参数
	DECLARE @TmpResult INT    --临时暂存返回数
	DECLARE @OrderNO VARCHAR(100) --订单号
	DECLARE @MaxDebts MONEY       --最高欠款金额
	DECLARE @PeopleNumberCount INT--团队订单总人数:已成交人数+已留位订单人数(本身未包括)
	DECLARE @PlanPeopleNumber INT --团队人数
	DECLARE @OldPeopleNumber INT  --订单原有人数
	DECLARE @VirtualPeopleNumber INT  --虚拟实数
	DECLARE @RealVirtualPeopleNumber INT     --用作修改虚拟实收的临时参数
	DECLARE @FinanceSum MONEY --财务小计
	DECLARE @ShiShu INT --实际订单人数
	declare @TourClassId tinyint   --团队类型
	DECLARE @SBuyCompanyID INT--之前订单的组团社公司编号
	DECLARE @TrafficPeopleNum INT --交通当天的票数
	DECLARE @TrafficYiShiYong INT --交通当天已使用的票数
	DECLARE @LeaveDate DATETIME   --出团时间
	SET @ErrorCount=0
	SET @Result=1
BEGIN	
	DECLARE @TuiTuanRenShu INT--当前订单退团人数
	--获取之前订单的组团社公司编号
	SELECT @SBuyCompanyID=BuyCompanyID,@TuiTuanRenShu=LeaguePepoleNum FROM [tbl_TourOrder] WHERE [ID]=@OrderID
	--判断订单人数是否大于剩余人数
    SELECT @OldPeopleNumber = ISNULL(PeopleNumber,0),@TourId=TourId,@TourClassId = TourClassId, @LeaveDate = LeaveDate
    FROM tbl_TourOrder WHERE ID=@OrderID AND IsDelete=0
	--未处理订单人数+已留位订单人数+已成交订单人数 - 已退团人数
	SELECT @PeopleNumberCount = ISNULL(SUM(PeopleNumber - LeaguePepoleNum),0) FROM tbl_TourOrder WHERE id <> @OrderID AND TourId = @TourId AND (OrderState=1 OR OrderState=2 OR OrderState=5)  AND IsDelete = '0'	
	SELECT @VirtualPeopleNumber = ISNULL(VirtualPeopleNumber,0),@PlanPeopleNumber = ISNULL(PlanPeopleNumber,0) FROM tbl_Tour WHERE TourId = @TourId  AND IsDelete=0
	SELECT @ShiShu = ISNULL(SUM(PeopleNumber - LeaguePepoleNum),0) FROM tbl_TourOrder WHERE IsDelete=0 AND TourId=@TourId AND (OrderState=1 OR OrderState=2 OR OrderState=5)
	--交通出团日期当天的人数
	SELECT @TrafficPeopleNum = TicketNums FROM tbl_TrafficPrices WHERE TrafficId = @OrderTraffic AND SDateTime = (SELECT LeaveDate FROM tbl_Tour WHERE tbl_Tour.TourId = @TourId) AND [Status] = 0
	IF @TrafficPeopleNum IS NULL
	BEGIN
		SET @TrafficPeopleNum = 0
	END
	--交通出团日期当天已使用的票数
	SELECT @TrafficYiShiYong = ISNULL(SUM(PeopleNumber - LeaguePepoleNum),0) FROM tbl_TourOrder AS a WHERE a.IsDelete='0' AND a.OrderState IN(1,2,5) AND DATEDIFF(dd,a.LeaveDate,@LeaveDate) = 0 AND EXISTS (SELECT 1 FROM tbl_TourOrderTraffic AS b WHERE b.OrderId = a.ID AND b.TrafficId = @OrderTraffic) AND a.ID <> @OrderID

	--订单人数 + 交通出团日期当天已使用票数 大于 交通出团日期当天的人数  不允许修改
	IF @PeopleNumber + @TrafficYiShiYong > @TrafficPeopleNum
	BEGIN
		SET @Result=4
		RETURN @Result
	END

	--该团所有订单人数大于团队计划人数，则不允许修改
	if @PlanPeopleNumber < @PeopleNumberCount + @PeopleNumber-@TuiTuanRenShu
	begin
		SET @Result=2
		RETURN @Result
	end
	
	--未处理,留位和成交时，验证是否达到该客户的最高欠款金额 
	IF(@OrderState=1 OR @OrderState=2 OR @OrderState=5)
	BEGIN 			
		SELECT @MaxDebts=MaxDebts FROM tbl_Customer WHERE [ID]=@BuyCompanyID	 
		--该客户的最高欠款金额-本订单金额<已成交+已留位的订单的 财务小计-已收已审核金额 的总和(本身订单除外)
		IF((@MaxDebts-@SumPrice)<(SELECT SUM(FinanceSum-HasCheckMoney) FROM tbl_TourOrder WHERE id<>@OrderID  AND IsDelete=0 AND BuyCompanyID=@BuyCompanyID AND TourId=@TourId AND (OrderState=5 OR OrderState=2)))
		BEGIN
			SET @Result=3
		END
		ELSE--更新统计分析所有收入明细表
		BEGIN			
			EXEC [proc_StatAllIncome_AboutDelete] @OrderID,0
		END
	END
	ELSE--更新统计分析所有收入明细表
	BEGIN
		EXEC [proc_StatAllIncome_AboutDelete] @OrderID,1			
	END
	BEGIN TRANSACTION TourOrder_Update			
	IF(@Result=1)
	BEGIN
		--DECLARE @CommissionType TINYINT --返佣类型
		DECLARE @CommissionIsPaid CHAR(1)--返佣金额是否支付
		
		IF((@BuyerContactName IS NULL OR LEN(@BuyerContactName)<1) AND @BuyerContactId>0)
		BEGIN
			SELECT @BuyerContactName=[Name] FROM [tbl_CustomerContactInfo] WHERE [Id]=@BuyerContactId
		END
		--重新计算财务小计:新的订单金额+增加费用+减少费用@CommissionType=[CommissionType],
		SELECT @FinanceSum=@SumPrice+FinanceAddExpense-FinanceRedExpense,@CommissionIsPaid=[CommissionStatus] FROM [tbl_TourOrder] a WHERE a.[ID]=@OrderID				
		--修改订单信息
		UPDATE [tbl_TourOrder] SET			
				[OrderState]=@OrderState,[PriceStandId]=@PriceStandId,[PersonalPrice]=@PersonalPrice
				,[ChildPrice]=@ChildPrice,[MarketPrice]=@MarketPrice,[AdultNumber]=@AdultNumber
				,[ChildNumber]=@ChildNumber,[MarketNumber]=@MarketNumber,[PeopleNumber]=@PeopleNumber
				,[OtherPrice]=@OtherPrice,[SaveSeatDate]=@SaveSeatDate,[OperatorContent]=@OperatorContent
				,[SpecialContent]=@SpecialContent,[SumPrice]=@SumPrice,[LastDate]=@LastDate,[LastOperatorID]=@LastOperatorID
				,[CustomerDisplayType]=@CustomerDisplayType,[CustomerFilePath]=@CustomerFilePath,CustomerLevId=@CustomerLevId
				,FinanceSum=@FinanceSum,ContactName=@ContactName,ContactTel=@ContactTel
				,ContactMobile=@ContactMobile,ContactFax=@ContactFax,SuccessTime=GETDATE()
				,[BuyerContactId]=@BuyerContactId,[BuyerContactName]=@BuyerContactName,[CommissionPrice]=@CommissionPrice
				,BuyCompanyName=@BuyCompanyName,SalerId=@SalerId,SalerName=@SalerName,CommissionType=@CommissionType,BuyCompanyID=@BuyCompanyID,BuyerTourCode=@BuyerTourCode
		WHERE [ID]=@OrderID		
		SET @ErrorCount = @ErrorCount + @@ERROR	
		--维护团队实收数=已有虚拟实收人数+订单总人数
		SET @RealVirtualPeopleNumber = @VirtualPeopleNumber + @PeopleNumber - @OldPeopleNumber
		IF(@VirtualPeopleNumber+@PeopleNumber-@OldPeopleNumber)<(@PeopleNumberCount+@PeopleNumber)
		BEGIN
			SET @RealVirtualPeopleNumber=@PeopleNumberCount+@PeopleNumber
		END
		--留位和确认成交时，维护团队虚拟实收数
		IF(@OrderState=1 OR @OrderState=2 OR @OrderState=5)
		BEGIN						
			UPDATE tbl_Tour SET VirtualPeopleNumber=@RealVirtualPeopleNumber WHERE TourId=@TourId  AND IsDelete=0
		END
		ELSE
		BEGIN
			SET @RealVirtualPeopleNumber=@RealVirtualPeopleNumber-@PeopleNumber
			UPDATE tbl_Tour SET VirtualPeopleNumber=@RealVirtualPeopleNumber WHERE TourId=@TourId  AND IsDelete=0
		END								
		SET @ErrorCount = @ErrorCount + @@ERROR	           				
		IF(@ErrorCount=0 AND LEN(@TourCustomerXML)>0)
		BEGIN				
			--游客数据维护
			EXECUTE  dbo.proc_TourOrderCustomer_Update @TourCustomerXML,@OrderID,@Result OUTPUT	
			IF(@TmpResult<>1)
			BEGIN
				SET @ErrorCount = @ErrorCount + 1	
			END
		END
		IF(@ErrorCount=0 AND @AmountPlus IS NOT NULL AND LEN(@AmountPlus)>0)--订单金额增加减少费用信息
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@AmountPlus
			DELETE FROM [tbl_TourOrderPlus] WHERE [OrderId]=@OrderId
			SET @ErrorCount = @ErrorCount +@@ERROR
			IF(@ErrorCount=0)
			BEGIN
				INSERT INTO [tbl_TourOrderPlus]([OrderId],[AddAmount],[ReduceAmount],[Remark])
				SELECT @OrderId,A.AddAmount,A.ReduceAmount,A.Remark FROM OPENXML(@hdoc,'/ROOT/Info')
				WITH(AddAmount MONEY,ReduceAmount MONEY,Remark NVARCHAR(MAX)) AS A
			END
			EXECUTE sp_xml_removedocument @hdoc
		END

		IF(@CommissionType=2 AND @CommissionIsPaid='1')--返佣类型为后返且返佣金额已支付修改其它支出信息
		BEGIN
			DECLARE @OhterOutAmount MONEY--其它支出金额
			DECLARE @CostId CHAR(36)--杂费支出编号
			SELECT @CostId=[Id] FROM [tbl_TourOtherCost] WHERE [ItemType]=2 AND [ItemId]=@OrderId
			SET @OhterOutAmount=(@AdultNumber+@ChildNumber)*@CommissionPrice
			--更新杂费支出
			UPDATE [tbl_TourOtherCost] SET [Proceed]=@OhterOutAmount,SumCost=@OhterOutAmount+[IncreaseCost]-[ReduceCost] WHERE [Id]=@CostId
			--更新支出明细
			UPDATE [tbl_StatAllOut] SET [Amount]=@OhterOutAmount,[TotalAmount]=@OhterOutAmount+[AddAmount]-[ReduceAmount]
			WHERE [ItemId]=@CostId AND [ItemType]=3
		END

		--团队计划订单时，修改订单同步修改团队计划
		if @TourClassId = 1
		begin
			if @TourTeamUnit is not null and len(@TourTeamUnit) > 1
			begin
				DELETE FROM [tbl_TourTeamUnit] WHERE [TourId] = @TourId --删除原来的数据
				
				EXEC sp_xml_preparedocument @hdoc OUTPUT,@TourTeamUnit
				INSERT INTO [tbl_TourTeamUnit] ([TourId],[NumberCR],[NumberET],[NumberQP]
					,[UnitAmountCR],[UnitAmountET],[UnitAmountQP])
				select @TourId,a.NumberCr,a.NumberEt,a.NumberQp
					,a.UnitAmountCr,a.UnitAmountEt,a.UnitAmountQp from openxml(@hdoc,'/ROOT/Info')
				with(NumberCr int,NumberEt int,NumberQp int
					,UnitAmountCr money,UnitAmountEt money,UnitAmountQp money) as a
				SET @errorcount=@errorcount + @@ERROR
				EXEC sp_xml_removedocument @hdoc
			end
		END
		
		--订单交通信息
		IF @ErrorCount = 0
		BEGIN
			DELETE FROM tbl_TourOrderTraffic WHERE OrderId = @OrderID
			SET @ErrorCount = @ErrorCount + @@ERROR
			IF @ErrorCount = 0
			BEGIN
				INSERT INTO tbl_TourOrderTraffic (OrderId,TrafficId) VALUES (@OrderID,@OrderTraffic)
				SET @ErrorCount = @ErrorCount + @@ERROR
			END
		END
	END
	--更新组团社交易次数
	--如果当前选择组团社与之前订单的组团社不一致，则要把之前订单的组团社交易次数减1
	IF(@SBuyCompanyID<>@BuyCompanyID)
	BEGIN
		UPDATE [tbl_Customer] SET TradeNum=TradeNum-1 WHERE ID=@SBuyCompanyID
		SET @errorcount=@errorcount + @@ERROR
	END
	IF(@ErrorCount>0)
	BEGIN
		SET @Result=0
		ROLLBACK TRANSACTION TourOrder_Update
	END
	COMMIT TRANSACTION TourOrder_Update
	--更新是否收客收满
	UPDATE [tbl_Tour] SET [Status]=1  WHERE [Status]=0 AND PlanPeopleNumber=(SELECT ISNULL(SUM(PeopleNumber-LeaguePepoleNum),0) FROM tbl_TourOrder WHERE TourId=(SELECT TourId FROM tbl_TourOrder WHERE [id]=@OrderID) AND IsDelete='0' AND OrderState NOT IN(3,4))	and TourId=@TourId 						
	RETURN  @Result
END


go




GO

-- =============================================
-- Author:		<李焕超,>
-- Create date: <Create 2011-1-21,,>
-- Description:	<申请机票,,>
-- History:
-- 1.2011-04-21 汪奇志 重新整理过程及机票票款增加百分比、其它费用
-- 2.2012-03-03 汪奇志 增加机票管理及机票计调安排信息的关联 以方便查询
-- =============================================
ALTER PROCEDURE [dbo].[proc_TicketOut_Insert]
(
	@TicketId char(36) ,--出票编号,PlanTicketOut表编号	
	@TourId char(36) ,--团号
	@OrderId char(36),--订单号
	@RouteName nvarchar(255),--线路名称
	@PNR nvarchar(max),--PNR
	@Total money, --总费用
	@Notice nvarchar(250),--订票需知
	@TicketOffice nvarchar(50),--机票供应商
	@TicketOfficeId int,--机票供应商编号
	@Saler nvarchar(50),--销售员
	@TicketNum nvarchar(50),--票号
	@PayType nvarchar(50),--支付方式
	@Remark nvarchar(250),--备注
	@TicketType tinyint,--机票类型（团队申请机票、订单退票，订单申请机票）
	@State tinyint,--状态
	@Operator nvarchar(250),--操作人员
	@OperateID int,--操作人员编号
	@CompanyID int,--公司编号
	@RegisterOperatorId  int,--机票申请人编号
	@CustomerXML nvarchar(max),--游客修改信息XML:<ROOT><Info VisitorName="游客姓名" CradNumber="证件号码" CradType="证件类型" TravellerId="游客编号" /></ROOT>
	@FlightListXML nvarchar(max),--机票航班信息XML:<ROOT><Info FligthSegment="航段" DepartureTime="出港时间" AireLine="航空公司" Discount="折扣" TicketTime="时间" /></ROOT>
	@TicketKindXML nvarchar(max),--票款类型 XML:<ROOT><Info Price="票面价" OilFee="税/机建" PeopleCount="人数" AgencyPrice="代理费" TotalMoney="票款" TicketType="票种" Discount="百分比" OtherPrice="其它费用" /></ROOT>
	@OutCustomerXML nvarchar(max),--机票申请与游客对应关系 XML:<ROOT><Info TravellerId="游客编号" /></ROOT>
	@Result int output --输出参数
)
AS
BEGIN
	DECLARE @errorcount INT
	DECLARE @hdoc INT
	
	IF @TicketId IS NULL OR LEN(@TicketId) <> 36 OR @TourId IS NULL OR LEN(@TourId) <> 36 OR @OrderId IS NULL OR LEN(@OrderId) <> 36 
	BEGIN
		SET @Result = -2
		return
	END
		

	SET @errorcount=0
	BEGIN TRANSACTION

	INSERT INTO [tbl_PlanTicketOut]([id],[PNR],[Total],[Notice],[TicketOffice]
		,[TicketOfficeId],[TicketNum],[PayType],[Remark],[TourId]
		,[State],[Operator],[OperateID],[CompanyID],[AddAmount]
		,[ReduceAmount],[TotalAmount],[FRemark],RegisterOperatorId,OrderId)
	VALUES(@TicketId,@PNR,@Total,@Notice,@TicketOffice
		,@TicketOfficeId,@TicketNum,@PayType,@Remark,@TourId
		,@State,@Operator,@OperateID,@CompanyID,0
		,0,@Total,'',@RegisterOperatorId,@OrderId)
	SET @errorcount=@errorcount+@@ERROR

--	IF(@errorcount=0 AND @OutCustomerXML IS NOT NULL AND LEN(@OutCustomerXML)>0)--写入机票申请与游客对应关系
--	BEGIN
--		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@OutCustomerXML
--		INSERT INTO [tbl_PlanTicketOutCustomer]([TicketOutId] ,[UserId])
--		SELECT @TicketId,A.TravellerId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TravellerId CHAR(36)) AS A
--		SET @errorcount=@errorcount+@@ERROR
--		EXECUTE sp_xml_removedocument @hdoc
--	END
	IF @errorcount = 0  --写入机票申请与游客对应关系
	BEGIN
		INSERT INTO [tbl_PlanTicketOutCustomer](TicketOutId,UserId)
		SELECT @TicketId,ID FROM tbl_TourOrderCustomer WHERE OrderId = @OrderId
		SET @errorcount=@errorcount+@@ERROR
	END

--	IF(@errorcount=0 AND @CustomerXML IS NOT NULL AND LEN(@CustomerXML)>0)--更新游客信息
--	BEGIN
--		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@CustomerXML
--		UPDATE [tbl_TourOrderCustomer] SET VisitorName=B.VisitorName,CradNumber=B.CradNumber,CradType=B.CradType
--		FROM [tbl_TourOrderCustomer] AS A INNER JOIN(SELECT VisitorName,CradNumber,CradType,TravellerId FROM OPENXML(@hdoc,'/ROOT/Info')
--			WITH(VisitorName nvarchar(250),CradNumber nvarchar(250),CradType int,TravellerId char(36))
--		) AS B
--		ON A.Id=B.TravellerId
--		SET @errorcount=@errorcount+@@ERROR
--		EXECUTE sp_xml_removedocument @hdoc
--	END

--	IF(@errorcount=0 AND @FlightListXML IS NOT NULL AND LEN(@FlightListXML)>0)--航段信息
--	BEGIN
--		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@FlightListXML
--        INSERT INTO  [tbl_PlanTicketFlight]([FligthSegment],[DepartureTime],[AireLine],[Discount],[TicketId],[TicketTime])
--		SELECT A.FligthSegment,A.DepartureTime,A.AireLine,A.Discount,@TicketId,A.TicketTime FROM OPENXML(@hdoc,'/ROOT/Info') 
--		WITH(FligthSegment nvarchar(50),DepartureTime datetime,AireLine tinyint,Discount decimal(10,6),TicketTime varchar(100)) AS A
--		SET @errorcount=@errorcount+@@ERROR
--		EXECUTE sp_xml_removedocument @hdoc
--	END
	IF @errorcount = 0
	BEGIN
		INSERT INTO  [tbl_PlanTicketFlight]([FligthSegment],[DepartureTime],[AireLine],[Discount],[TicketId],[TicketTime])
		SELECT 
		--ISNULL((SELECT ProvinceName FROM tbl_CompanyProvince AS sp WHERE sp.ID = a.LProvince),'') + 
		ISNULL((SELECT CityName FROM tbl_CompanyCity AS sc WHERE sc.ID = a.LCity),'') 
		+ '-' +
		--ISNULL((SELECT ProvinceName FROM tbl_CompanyProvince AS sp WHERE sp.ID = a.RProvince),'') + 
		ISNULL((SELECT CityName FROM tbl_CompanyCity AS sc WHERE sc.ID = a.RCity),''),
		(SELECT LeaveDate FROM tbl_Tour WHERE TourId = @TourId)
		,a.FlightCompany,0,@TicketId,a.FilghtNum + '/' + a.LTime
		FROM tbl_Travel AS a WHERE a.TrafficId = (SELECT TOP 1 tot.TrafficId FROM tbl_TourOrderTraffic tot WHERE tot.OrderId = @OrderId)
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount=0 AND @TicketKindXML IS NOT NULL AND LEN(@TicketKindXML)>0)--票款信息
	BEGIN
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@TicketKindXML
        INSERT INTO [tbl_PlanTicketKind]([TicketId],[Price],[OilFee],[PeopleCount],[AgencyPrice],[TotalMoney],[TicketType],[Discount],[OtherPrice])
		SELECT @TicketId,A.Price,A.OilFee,A.PeopleCount,A.AgencyPrice,A.TotalMoney,A.TicketType,A.Discount,A.OtherPrice FROM OPENXML(@hdoc,'/ROOT/Info') 
		WITH(Price money,OilFee money,PeopleCount int,AgencyPrice  money,TotalMoney money,TicketType tinyint,Discount DECIMAL(10,6),OtherPrice MONEY) AS A
		SET @errorcount=@errorcount+@@ERROR
		EXECUTE sp_xml_removedocument @hdoc
	END

	IF(@errorcount=0)--写入机票管理列表
	BEGIN		
		INSERT INTO [tbl_PlanTicket]([TourId],[OrderId],[RefundId],[TicketType],[RouteName],[Saler],[OperatorId],[RegisterTime],[State],[CompanyId])
		VALUES(@TourId,@OrderId,@TicketId,@TicketType,@RouteName,@Saler,@OperateID,GETDATE(),@State,@CompanyID)
		SET @errorcount=@errorcount+@@ERROR
		
		IF(@errorcount=0/* AND @State=3*/)--写机票管理及机票计调编号关联表
		BEGIN
			DECLARE @JiPiaoGuanLiId INT
			SELECT @JiPiaoGuanLiId=Id FROM tbl_PlanTicket WHERE RefundId=@TicketId
			DELETE FROM tbl_PlanTicketPlanId WHERE JiPiaoGuanLiId=@JiPiaoGuanLiId
			INSERT INTO tbl_PlanTicketPlanId(JiPiaoGuanLiId,PlanId)
			VALUES(@JiPiaoGuanLiId,@TicketId)
		END
	END

	IF(@errorcount>0)
	BEGIN
		ROLLBACK TRANSACTION
		SET @Result=0
		RETURN -1
	END

	COMMIT TRAN
	SET @Result=1
	RETURN 1
END



go




GO

-- =============================================
-- Author:		李焕超
-- Create date: 2011-03-01
-- Description:	机票申请修改、出票
-- History:
-- 1.2011-03-30 汪奇志 调整过程书写格式
-- 2.2011-04-21 汪奇志 机票票款增加百分比、其它费用
-- 3.2011-08-15 汪奇志 完成出票机票款自动结清处理(自动结清按系统配置)
-- 4.2012-03-02 汪奇志 增加机票管理及机票计调安排信息的关联 以方便查询
-- 5.2012-08-10 汪奇志 仅允许修改处于申请状态下的信息(修正修改机票申请与审核操作时间差导致审核过的申请被修改为申请状态的BUG)
-- 6.2012-09-12 周文超 修改机票申请（人数从订单出来、航段从交通出来）
-- =============================================
ALTER PROCEDURE [dbo].[proc_TicketOut_Update]
(   @TicketId char(36) ,--机票申请编号
    @TourId char(36),
	@RouteName nvarchar(255),--线路名称
    @PNR nvarchar(max),
    @Total money, --总费用
	@Notice nvarchar(250),--订票需知
	@TicketOffice nvarchar(50),--机票供应商
	@TicketOfficeId int,--机票供应商编号
	@Saler nvarchar(50),--销售员
	@TicketNum nvarchar(50),--票号
	@PayType nvarchar(50),--支付方式
	@Remark nvarchar(250),--备注
    @TicketType tinyint,--机票类型（团队申请机票、订单退票，订单申请机票）
    @State tinyint,--状态
	@Operator nvarchar(250),--操作人员
	@OperateID int,--操作人员编号
	@CompanyID int,--公司编号
	@TicketOutTime datetime,   --出票时间
	@CustomerXML nvarchar(max),--游客修改信息XML:<ROOT><Info VisitorName="游客姓名" CradNumber="证件号码" CradType="证件类型" TravellerId="游客编号" /></ROOT>
	@FlightListXML nvarchar(max),--机票航班信息XML:<ROOT><Info FligthSegment="航段" DepartureTime="出港时间" AireLine="航空公司" Discount="折扣" TicketTime="时间" /></ROOT>
	@TicketKindXML nvarchar(max),--票款类型 XML:<ROOT><Info Price="票面价" OilFee="税/机建" PeopleCount="人数" AgencyPrice="代理费" TotalMoney="票款" TicketType="票种" Discount="百分比" OtherPrice="其它费用" /></ROOT>
	@OutCustomerXML nvarchar(max),--机票申请与游客对应关系 XML:<ROOT><Info TravellerId="游客编号" /></ROOT>
	@Result int output --输出参数
	,@TicketOutRegisterId CHAR(36) OUTPUT--付款登记编号
	,@OrderId char(36)		--订单号
)
AS
BEGIN
	DECLARE @hdoc INT
	DECLARE @errorcount INT
	SET @errorcount=0

	IF(@State=1 AND NOT EXISTS(SELECT 1 FROM tbl_PlanTicketOut WHERE [Id]=@TicketId AND [State]=1))
	BEGIN
		SET @Result=-2
		RETURN -2
	END

	BEGIN TRANSACTION addTicket

	--机票申请信息
	UPDATE [tbl_PlanTicketOut] SET [PNR] = @PNR,[Total] = @Total,[Notice] = @Notice,[TicketOffice] = @TicketOffice,[TicketOfficeId] = @TicketOfficeId
		,[TicketNum] = @TicketNum,[PayType] = @PayType,[Remark] = @Remark,[State] = @State,[Operator] = @Operator
		,[OperateID] = @OperateID,[TotalAmount] = @Total+[AddAmount]-[ReduceAmount],[TicketOutTime] = @TicketOutTime
	WHERE id=@TicketId
	SET @errorcount=@errorcount+@@ERROR

--	IF(@errorcount=0 AND @OutCustomerXML IS NOT NULL AND LEN(@OutCustomerXML)>0)--机票申请与游客对应关系处理
--	BEGIN
--        DELETE FROM [tbl_PlanTicketOutCustomer] WHERE TicketOutId=@TicketId
--        SET @errorcount=@errorcount+@@ERROR
--		IF(@errorcount=0)
--		BEGIN
--			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@OutCustomerXML
--			INSERT INTO [tbl_PlanTicketOutCustomer]([TicketOutId] ,[UserId])
--			SELECT  @TicketId,A.TravellerId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TravellerId char(36)) AS A
--			SET @errorcount=@errorcount+@@ERROR
--			EXECUTE sp_xml_removedocument @hdoc
--		END
--	END

--	IF(@errorcount=0 AND LEN(@CustomerXML)>10)--游客信息处理
--	BEGIN
--		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@CustomerXML
--		UPDATE tbl_TourOrderCustomer SET VisitorName=A.VisitorName,CradType=A.CradType,CradNumber=A.CradNumber
--		FROM tbl_TourOrderCustomer AS B INNER JOIN (SELECT VisitorName,CradType,CradNumber,TravellerId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(VisitorName varchar(250),CradNumber varchar(250),CradType int,TravellerId char(36))) AS A
--		ON B.Id=A.TravellerId
--		SET @errorcount=@errorcount+@@ERROR
--		EXECUTE sp_xml_removedocument @hdoc
--	END 

--	IF(@errorcount=0 AND @FlightListXML IS NOT NULL AND LEN(@FlightListXML)>0)--航段信息
--    BEGIN
--        DELETE FROM [tbl_PlanTicketFlight] WHERE TicketId=@TicketId
--		SET @errorcount=@errorcount+@@ERROR
--		IF(@errorcount=0)
--		BEGIN
--			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@FlightListXML
--			INSERT INTO  [tbl_PlanTicketFlight]([FligthSegment],[DepartureTime],[AireLine],[Discount],[TicketId],[TicketTime])
--			SELECT A.FligthSegment,A.DepartureTime,A.AireLine,A.Discount,@TicketId,A.TicketTime   FROM OPENXML(@hdoc,'/ROOT/Info') 
--			WITH(FligthSegment nvarchar(50),DepartureTime datetime,AireLine tinyint,Discount decimal(10,6),TicketTime varchar(200)) AS A
--			SET @errorcount=@errorcount+@@ERROR
--			EXECUTE sp_xml_removedocument @hdoc
--		 END
--	END

	IF(@errorcount=0 AND @TicketKindXML IS NOT NULL AND LEN(@TicketKindXML)>0)--票款信息处理
	BEGIN
        DELETE FROM [tbl_PlanTicketKind] WHERE TicketId=@TicketId
        SET @errorcount=@errorcount+@@ERROR
		IF(@errorcount=0)
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@TicketKindXML
			INSERT INTO [tbl_PlanTicketKind]([TicketId],[Price],[OilFee],[PeopleCount],[AgencyPrice],[TotalMoney],[TicketType],[Discount],[OtherPrice])
			SELECT  @TicketId,A.Price,A.OilFee,A.PeopleCount,A.AgencyPrice,A.TotalMoney,A.TicketType,A.Discount,A.OtherPrice FROM OPENXML(@hdoc,'/ROOT/Info') 
			WITH( Price money,OilFee money,PeopleCount int,AgencyPrice  money,TotalMoney money,TicketType tinyint,Discount DECIMAL(10,6),OtherPrice MONEY) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
	END

	IF(@errorcount=0)--机票管理列表信息
	BEGIN
		UPDATE [tbl_PlanTicket] SET [TourId] = @TourId,[RouteName] = @RouteName,[Saler] = @Saler
			,[Operator] =@Operator,[OperatorId]=@OperateID,[RegisterTime] = GETDATE(),[State] = @State
		WHERE  RefundId=@TicketId
	END

	IF(@errorcount=0 AND @State=3 AND NOT EXISTS(SELECT 1 FROM [tbl_StatAllOut] WHERE ItemType=2 AND [ItemId]=@TicketId))--支出明细
	BEGIN
		DECLARE @AreaId INT--线路区域
		DECLARE @TourType TINYINT --计划类型
		DECLARE @DepartmentId INT--部门编号
		DECLARE @SupplierName NVARCHAR(255)
		
		SELECT @AreaId=AreaId,@TourType=TourType FROM [tbl_Tour] WHERE TourId=@TourId
		SELECT @DepartmentId=DepartId FROM tbl_CompanyUser WHERE Id=@OperateID
		SELECT @SupplierName=UnitName FROM tbl_CompanySupplier WHERE Id=@TicketOfficeId
	
		INSERT INTO [tbl_StatAllOut]([CompanyId],[TourId],[AreaId],[TourType],[ItemId]
			,[ItemType],[Amount],[AddAmount],[ReduceAmount],[TotalAmount]
			,[OperatorId],[DepartmentId],[CreateTime],[HasCheckAmount],[NotCheckAmount]
			,[IsDelete],[SupplierId],[SupplierName])
		SELECT @CompanyId,@TourId,@AreaId,@TourType,@TicketId
			,2,A.Total,A.AddAmount,A.ReduceAmount,A.TotalAmount
			,@OperateID,@DepartmentId,GETDATE(),0,0
			,'0',@TicketOfficeId,@SupplierName
		FROM tbl_PlanTicketOut AS A WHERE A.Id=@TicketId
	END

	IF(@errorcount=0 AND @State=3 AND EXISTS(SELECT 1 FROM tbl_CompanySetting WHERE Id=@CompanyId AND FieldName='IsTicketOutRegisterPayment' AND FieldValue='1'))--完成出票机票款自动结清处理(自动结清按系统配置),登记的付款默认已审核已支付
	BEGIN
		--ItemType(支出关联类型团队或杂费)=0团队 ReceiveType(计调项目类型)=2机票 PaymentType(支付方式)=2 账务现付
		DECLARE @Amount MONEY
		SELECT @Amount=[TotalAmount] FROM [tbl_PlanTicketOut] WHERE [Id]=@TicketId
		SET @TicketOutRegisterId=NEWID()
		INSERT INTO [tbl_FinancialPayInfo]([Id],[CompanyID],[ItemId],[ItemType],[ReceiveId]
			,[ReceiveType],[ReceiveCompanyId],[ReceiveCompanyName],[PaymentDate],[StaffNo]
			,[StaffName],[PaymentAmount],[PaymentType],[IsBill],[BillAmount]
			,[BillNo],[Remark],[IsChecked],[OperatorID],[IssueTime]
			,[CheckerId],[IsPay])
		VALUES(@TicketOutRegisterId,@CompanyId,@TourId,0,@TicketId
			,2,@TicketOfficeId,@TicketOffice,GETDATE(),@OperateID
			,@Operator,@Amount,2,0,0
			,'','','1',@OperateID,GETDATE()
			,@OperateID,'1')
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount=0/* AND @State=3*/)--写机票管理及机票计调编号关联表
	BEGIN
		DECLARE @JiPiaoGuanLiId INT
		SELECT @JiPiaoGuanLiId=Id FROM tbl_PlanTicket WHERE RefundId=@TicketId
		DELETE FROM tbl_PlanTicketPlanId WHERE JiPiaoGuanLiId=@JiPiaoGuanLiId
		INSERT INTO tbl_PlanTicketPlanId(JiPiaoGuanLiId,PlanId)
		VALUES(@JiPiaoGuanLiId,@TicketId)
	END

	IF(@errorcount>0)
	BEGIN
		ROLLBACK TRAN
		SET @Result=0
		RETURN -1
	END

	COMMIT TRAN
	SET @Result=1
	RETURN @Result
END


go