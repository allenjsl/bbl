--计调安排机票信息表增加账务审核备注字段 2011-08-12
GO
ALTER TABLE dbo.tbl_PlanTicketOut ADD
	ReviewRemark nvarchar(MAX) NULL
GO
DECLARE @v sql_variant 
SET @v = N'账务审核备注'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_PlanTicketOut', N'COLUMN', N'ReviewRemark'
GO

-- =============================================
-- Author:		李焕超
-- Create date: 2011-03-01
-- Description:	机票申请修改、出票
-- History:
-- 1.2011-03-30 汪奇志 调整过程书写格式
-- 2.2011-04-21 汪奇志 机票票款增加百分比、其它费用
-- 3.2011-08-15 汪奇志 完成出票机票款自动结清处理(自动结清按系统配置)
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
)
AS
BEGIN
	DECLARE @hdoc INT
	DECLARE @errorcount INT
	SET @errorcount=0
	BEGIN TRANSACTION addTicket

	--机票申请信息
	UPDATE [tbl_PlanTicketOut] SET [PNR] = @PNR,[Total] = @Total,[Notice] = @Notice,[TicketOffice] = @TicketOffice,[TicketOfficeId] = @TicketOfficeId
		,[TicketNum] = @TicketNum,[PayType] = @PayType,[Remark] = @Remark,[State] = @State,[Operator] = @Operator
		,[OperateID] = @OperateID,[TotalAmount] = @Total+[AddAmount]-[ReduceAmount],[TicketOutTime] = @TicketOutTime
	WHERE id=@TicketId
	SET @errorcount=@errorcount+@@ERROR

	IF(@errorcount=0 AND @OutCustomerXML IS NOT NULL AND LEN(@OutCustomerXML)>0)--机票申请与游客对应关系处理
	BEGIN
        DELETE FROM [tbl_PlanTicketOutCustomer] WHERE TicketOutId=@TicketId
        SET @errorcount=@errorcount+@@ERROR
		IF(@errorcount=0)
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@OutCustomerXML
			INSERT INTO [tbl_PlanTicketOutCustomer]([TicketOutId] ,[UserId])
			SELECT  @TicketId,A.TravellerId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TravellerId char(36)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
	END

	IF(@errorcount=0 AND LEN(@CustomerXML)>10)--游客信息处理
	BEGIN
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@CustomerXML
		UPDATE tbl_TourOrderCustomer SET VisitorName=A.VisitorName,CradType=A.CradType,CradNumber=A.CradNumber
		FROM tbl_TourOrderCustomer AS B INNER JOIN (SELECT VisitorName,CradType,CradNumber,TravellerId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(VisitorName varchar(250),CradNumber varchar(250),CradType int,TravellerId char(36))) AS A
		ON B.Id=A.TravellerId
		SET @errorcount=@errorcount+@@ERROR
		EXECUTE sp_xml_removedocument @hdoc
	END 

	IF(@errorcount=0 AND @FlightListXML IS NOT NULL AND LEN(@FlightListXML)>0)--航段信息
    BEGIN
        DELETE FROM [tbl_PlanTicketFlight] WHERE TicketId=@TicketId
		SET @errorcount=@errorcount+@@ERROR
		IF(@errorcount=0)
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@FlightListXML
			INSERT INTO  [tbl_PlanTicketFlight]([FligthSegment],[DepartureTime],[AireLine],[Discount],[TicketId],[TicketTime])
			SELECT A.FligthSegment,A.DepartureTime,A.AireLine,A.Discount,@TicketId,A.TicketTime   FROM OPENXML(@hdoc,'/ROOT/Info') 
			WITH(FligthSegment nvarchar(50),DepartureTime datetime,AireLine tinyint,Discount decimal,TicketTime varchar(200)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		 END
	END

	IF(@errorcount=0 AND @TicketKindXML IS NOT NULL AND LEN(@TicketKindXML)>0)--票款信息处理
	BEGIN
        DELETE FROM [tbl_PlanTicketKind] WHERE TicketId=@TicketId
        SET @errorcount=@errorcount+@@ERROR
		IF(@errorcount=0)
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@TicketKindXML
			INSERT INTO [tbl_PlanTicketKind]([TicketId],[Price],[OilFee],[PeopleCount],[AgencyPrice],[TotalMoney],[TicketType])
			SELECT  @TicketId,A.Price,A.OilFee,A.PeopleCount,A.AgencyPrice,A.TotalMoney,A.TicketType FROM OPENXML(@hdoc,'/ROOT/Info') 
			WITH( Price money,OilFee money,PeopleCount int,AgencyPrice  money,TotalMoney money,TicketType tinyint) AS A
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
GO
--以上日志已更新


--2011-09-02
--机票票款百分比字段数据类型更改
alter table tbl_PlanTicketKind 
alter column [Discount] decimal(10,6) not null
go
--机票航班折扣字段数据类型更改
alter table tbl_PlanTicketFlight
alter column [Discount] decimal(10,6) not null
go

-- =============================================
-- Author:		<李焕超,>
-- Create date: <Create 2011-1-21,,>
-- Description:	<申请机票,,>
-- History:
-- 1.2011-04-21 汪奇志 重新整理过程及机票票款增加百分比、其它费用
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

	SET @errorcount=0
	BEGIN TRANSACTION

	INSERT INTO [tbl_PlanTicketOut]([id],[PNR],[Total],[Notice],[TicketOffice]
		,[TicketOfficeId],[TicketNum],[PayType],[Remark],[TourId]
		,[State],[Operator],[OperateID],[CompanyID],[AddAmount]
		,[ReduceAmount],[TotalAmount],[FRemark],RegisterOperatorId)
	VALUES(@TicketId,@PNR,@Total,@Notice,@TicketOffice
		,@TicketOfficeId,@TicketNum,@PayType,@Remark,@TourId
		,@State,@Operator,@OperateID,@CompanyID,0
		,0,@Total,'',@RegisterOperatorId)
	SET @errorcount=@errorcount+@@ERROR

	IF(@errorcount=0 AND @OutCustomerXML IS NOT NULL AND LEN(@OutCustomerXML)>0)--写入机票申请与游客对应关系
	BEGIN
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@OutCustomerXML
		INSERT INTO [tbl_PlanTicketOutCustomer]([TicketOutId] ,[UserId])
		SELECT @TicketId,A.TravellerId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TravellerId CHAR(36)) AS A
		SET @errorcount=@errorcount+@@ERROR
		EXECUTE sp_xml_removedocument @hdoc
	END

	IF(@errorcount=0 AND @CustomerXML IS NOT NULL AND LEN(@CustomerXML)>0)--更新游客信息
	BEGIN
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@CustomerXML
		UPDATE [tbl_TourOrderCustomer] SET VisitorName=B.VisitorName,CradNumber=B.CradNumber,CradType=B.CradType
		FROM [tbl_TourOrderCustomer] AS A INNER JOIN(SELECT VisitorName,CradNumber,CradType,TravellerId FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(VisitorName nvarchar(250),CradNumber nvarchar(250),CradType int,TravellerId char(36))
		) AS B
		ON A.Id=B.TravellerId
		SET @errorcount=@errorcount+@@ERROR
		EXECUTE sp_xml_removedocument @hdoc
	END

	IF(@errorcount=0 AND @FlightListXML IS NOT NULL AND LEN(@FlightListXML)>0)--航段信息
	BEGIN
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@FlightListXML
        INSERT INTO  [tbl_PlanTicketFlight]([FligthSegment],[DepartureTime],[AireLine],[Discount],[TicketId],[TicketTime])
		SELECT A.FligthSegment,A.DepartureTime,A.AireLine,A.Discount,@TicketId,A.TicketTime FROM OPENXML(@hdoc,'/ROOT/Info') 
		WITH(FligthSegment nvarchar(50),DepartureTime datetime,AireLine tinyint,Discount decimal(10,6),TicketTime varchar(100)) AS A
		SET @errorcount=@errorcount+@@ERROR
		EXECUTE sp_xml_removedocument @hdoc
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
GO

-- =============================================
-- Author:		李焕超
-- Create date: 2011-03-01
-- Description:	机票申请修改、出票
-- History:
-- 1.2011-03-30 汪奇志 调整过程书写格式
-- 2.2011-04-21 汪奇志 机票票款增加百分比、其它费用
-- 3.2011-08-15 汪奇志 完成出票机票款自动结清处理(自动结清按系统配置)
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
)
AS
BEGIN
	DECLARE @hdoc INT
	DECLARE @errorcount INT
	SET @errorcount=0
	BEGIN TRANSACTION addTicket

	--机票申请信息
	UPDATE [tbl_PlanTicketOut] SET [PNR] = @PNR,[Total] = @Total,[Notice] = @Notice,[TicketOffice] = @TicketOffice,[TicketOfficeId] = @TicketOfficeId
		,[TicketNum] = @TicketNum,[PayType] = @PayType,[Remark] = @Remark,[State] = @State,[Operator] = @Operator
		,[OperateID] = @OperateID,[TotalAmount] = @Total+[AddAmount]-[ReduceAmount],[TicketOutTime] = @TicketOutTime
	WHERE id=@TicketId
	SET @errorcount=@errorcount+@@ERROR

	IF(@errorcount=0 AND @OutCustomerXML IS NOT NULL AND LEN(@OutCustomerXML)>0)--机票申请与游客对应关系处理
	BEGIN
        DELETE FROM [tbl_PlanTicketOutCustomer] WHERE TicketOutId=@TicketId
        SET @errorcount=@errorcount+@@ERROR
		IF(@errorcount=0)
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@OutCustomerXML
			INSERT INTO [tbl_PlanTicketOutCustomer]([TicketOutId] ,[UserId])
			SELECT  @TicketId,A.TravellerId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TravellerId char(36)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
	END

	IF(@errorcount=0 AND LEN(@CustomerXML)>10)--游客信息处理
	BEGIN
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@CustomerXML
		UPDATE tbl_TourOrderCustomer SET VisitorName=A.VisitorName,CradType=A.CradType,CradNumber=A.CradNumber
		FROM tbl_TourOrderCustomer AS B INNER JOIN (SELECT VisitorName,CradType,CradNumber,TravellerId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(VisitorName varchar(250),CradNumber varchar(250),CradType int,TravellerId char(36))) AS A
		ON B.Id=A.TravellerId
		SET @errorcount=@errorcount+@@ERROR
		EXECUTE sp_xml_removedocument @hdoc
	END 

	IF(@errorcount=0 AND @FlightListXML IS NOT NULL AND LEN(@FlightListXML)>0)--航段信息
    BEGIN
        DELETE FROM [tbl_PlanTicketFlight] WHERE TicketId=@TicketId
		SET @errorcount=@errorcount+@@ERROR
		IF(@errorcount=0)
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@FlightListXML
			INSERT INTO  [tbl_PlanTicketFlight]([FligthSegment],[DepartureTime],[AireLine],[Discount],[TicketId],[TicketTime])
			SELECT A.FligthSegment,A.DepartureTime,A.AireLine,A.Discount,@TicketId,A.TicketTime   FROM OPENXML(@hdoc,'/ROOT/Info') 
			WITH(FligthSegment nvarchar(50),DepartureTime datetime,AireLine tinyint,Discount decimal(10,6),TicketTime varchar(200)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		 END
	END

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
GO


--http://www.tatop.com/ManageWorkflow/TaskShow.aspx?taskid=181 中旅体育
--人事工资表
CREATE TABLE [dbo].[tbl_Wage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Year] [int] NULL,
	[Month] [tinyint] NULL,
	[OperatorId] [int] NOT NULL CONSTRAINT [DF__tbl_Wage__Operat__158603F9]  DEFAULT ((0)),
	[IssueTime] [datetime] NOT NULL CONSTRAINT [DF__tbl_Wage__IssueT__167A2832]  DEFAULT (getdate()),
	[ZhiWei] [nvarchar](255) NULL,
	[XingMing] [nvarchar](255) NULL,
	[GangWeiGongZi] [money] NOT NULL CONSTRAINT [DF__tbl_Wage__GangWe__176E4C6B]  DEFAULT ((0)),
	[QuanQinJiang] [money] NOT NULL CONSTRAINT [DF__tbl_Wage__QuanQi__186270A4]  DEFAULT ((0)),
	[JiangJin] [money] NOT NULL CONSTRAINT [DF__tbl_Wage__JiangJ__195694DD]  DEFAULT ((0)),
	[FanBu] [money] NOT NULL CONSTRAINT [DF__tbl_Wage__FanBu__1A4AB916]  DEFAULT ((0)),
	[HuaBu] [money] NOT NULL CONSTRAINT [DF__tbl_Wage__HuaBu__1B3EDD4F]  DEFAULT ((0)),
	[YouBu] [money] NOT NULL CONSTRAINT [DF__tbl_Wage__YouBu__1C330188]  DEFAULT ((0)),
	[JiaBanFei] [money] NOT NULL CONSTRAINT [DF__tbl_Wage__JiaBan__1D2725C1]  DEFAULT ((0)),
	[ChiDao] [money] NOT NULL CONSTRAINT [DF__tbl_Wage__ChiDao__1E1B49FA]  DEFAULT ((0)),
	[BingJia] [money] NOT NULL CONSTRAINT [DF__tbl_Wage__BingJi__1F0F6E33]  DEFAULT ((0)),
	[ShiJia] [money] NOT NULL CONSTRAINT [DF__tbl_Wage__ShiJia__2003926C]  DEFAULT ((0)),
	[XingZhengFaKuan] [money] NOT NULL CONSTRAINT [DF__tbl_Wage__XingZh__20F7B6A5]  DEFAULT ((0)),
	[YeWuFaKuan] [money] NOT NULL CONSTRAINT [DF__tbl_Wage__YeWuFa__21EBDADE]  DEFAULT ((0)),
	[QianKuan] [money] NOT NULL CONSTRAINT [DF__tbl_Wage__QianKu__22DFFF17]  DEFAULT ((0)),
	[YingFaGongZi] [money] NOT NULL CONSTRAINT [DF__tbl_Wage__YingFa__23D42350]  DEFAULT ((0)),
	[WuXian] [money] NOT NULL CONSTRAINT [DF__tbl_Wage__WuXian__24C84789]  DEFAULT ((0)),
	[SheBao] [money] NOT NULL CONSTRAINT [DF__tbl_Wage__SheBao__25BC6BC2]  DEFAULT ((0)),
	[ShiFaGongZi] [money] NOT NULL CONSTRAINT [DF__tbl_Wage__ShiFaG__26B08FFB]  DEFAULT ((0)),
 CONSTRAINT [PK_TBL_WAGE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登记编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公司编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'年份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'Year'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'月份' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'Month'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作人编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'OperatorId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'IssueTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'ZhiWei'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'XingMing'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'岗位工资' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'GangWeiGongZi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'全勤奖' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'QuanQinJiang'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'奖金' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'JiangJin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'饭补' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'FanBu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'话补' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'HuaBu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'油补' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'YouBu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'加班费' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'JiaBanFei'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'迟到' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'ChiDao'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'病假' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'BingJia'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'事假' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'ShiJia'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'行政罚款' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'XingZhengFaKuan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'业务罚款' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'YeWuFaKuan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'欠款' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'QianKuan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应发工资' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'YingFaGongZi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'五险' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'WuXian'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'社保' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'SheBao'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'实发工资' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'ShiFaGongZi'
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-10-19
-- Description:	按年月设置人事工资信息
-- =============================================
CREATE PROCEDURE [dbo].[proc_Wage_Set]
	@CompanyId INT,--公司编号
	@Year INT,--年份
	@Month TINYINT,--月份
	@OperatorId INT,--操作员编号
	@WagesXML NVARCHAR(MAX),--XML:<ROOT><Info ... /></ROOT>
	@Result INT OUTPUT--操作结果 1:成功
AS
BEGIN
	DECLARE @errorcount INT
	DECLARE @hdoc INT

	SET @errorcount=0
	SET @Result=1

	BEGIN TRAN
	DELETE FROM tbl_Wage WHERE CompanyId=@CompanyId AND [Year]=@Year AND [Month]=@Month
	SET @errorcount=@errorcount+@@ERROR

	IF(@errorcount=0 AND @WagesXML IS NOT NULL AND LEN(@WagesXML)>0 AND @WagesXML<>'<ROOT></ROOT>')
	BEGIN
		EXEC sp_xml_preparedocument @hdoc OUTPUT,@WagesXML
		INSERT INTO [tbl_Wage]([CompanyId],[Year],[Month],[OperatorId],[IssueTime]
			,[ZhiWei],[XingMing],[GangWeiGongZi],[QuanQinJiang],[JiangJin]
			,[FanBu],[HuaBu],[YouBu],[JiaBanFei],[ChiDao]
			,[BingJia],[ShiJia],[XingZhengFaKuan],[YeWuFaKuan],[QianKuan]
			,[YingFaGongZi],[WuXian],[SheBao],[ShiFaGongZi])
		SELECT @CompanyId,@Year,@Month,@OperatorId,GETDATE()
			,[ZhiWei],[XingMing],[GangWeiGongZi],[QuanQinJiang],[JiangJin]
			,[FanBu],[HuaBu],[YouBu],[JiaBanFei],[ChiDao]
			,[BingJia],[ShiJia],[XingZhengFaKuan],[YeWuFaKuan],[QianKuan]
			,[YingFaGongZi],[WuXian],[SheBao],[ShiFaGongZi]
		FROM OPENXML(@hdoc,'/ROOT/Info') 
		WITH([ZhiWei] NVARCHAR(255),[XingMing] NVARCHAR(255),[GangWeiGongZi] MONEY,[QuanQinJiang] MONEY,[JiangJin] MONEY
			,[FanBu] MONEY,[HuaBu] MONEY,[YouBu] MONEY,[JiaBanFei] MONEY,[ChiDao] MONEY
			,[BingJia] MONEY,[ShiJia] MONEY,[XingZhengFaKuan] MONEY,[YeWuFaKuan] MONEY,[QianKuan] MONEY
			,[YingFaGongZi] MONEY,[WuXian] MONEY,[SheBao] MONEY,[ShiFaGongZi] MONEY)		
		SET @errorcount=@errorcount+@@ERROR
		EXEC sp_xml_removedocument @hdoc
	END	

	IF(@errorcount>0)
	BEGIN
		ROLLBACK TRAN
		SET @Result=0
		RETURN -1
	END

	COMMIT TRAN
	SET @Result=1
END
GO
--http://www.tatop.com/ManageWorkflow/TaskShow.aspx?taskid=181 中旅体育



--(20111026)bbl.txt
-- =============================================
-- Author:		周文超
-- Create date: 2011-04-19
-- Description:	机票管理--退票统计视图
-- History:
-- 1.2011-06-14 增加团队发布人列 [TourOperatorId]
-- 2.2011-10-28 增加机票票号[TicketNum]
-- =============================================
ALTER VIEW [dbo].[View_RefundStatistics]
AS
select a.Id,a.CustormerId,a.OperatorName,a.IssueTime,a.RefundAmount
,b.VisitorName
,c.TourNo,c.RouteName,c.LeaveDate,c.BuyCompanyName,c.BuyerContactName as OrderOperatorName,c.SellCompanyId,c.BuyCompanyID
,(select sum(TotalMoney) 
	from tbl_PlanTicketKind 
	where tbl_PlanTicketKind.TicketId in (select TicketOutId 
											from tbl_PlanTicketOutCustomer 
											where tbl_PlanTicketOutCustomer.UserId = a.CustormerId)
 ) as TotalAmount
,(select ID,FligthSegment,DepartureTime,AireLine,Discount,TicketId,TicketTime,TicketNum=(SELECT tbl_PlanTicketOut.TicketNum FROM tbl_PlanTicketOut WHERE tbl_PlanTicketOut.ID=tbl_PlanTicketFlight.TicketId)
	from tbl_PlanTicketFlight 
	where tbl_PlanTicketFlight.ID in (select tbl_CustomerRefundFlight.FlightId 
										from tbl_CustomerRefundFlight 
										where tbl_CustomerRefundFlight.CustomerId = a.CustormerId 
											and tbl_CustomerRefundFlight.RefundId = a.Id) 
 for xml raw,root('root')) as FligthInfo
,D.OperatorId AS TourOperatorId
from tbl_CustomerRefund as a 
inner join tbl_TourOrderCustomer as b 
on a.CustormerId = b.Id 
inner join tbl_TourOrder as c 
on c.ID = b.OrderId 
INNER JOIN tbl_Tour AS D
ON c.TourId=D.TourId
GO
/*
pages:
/Web/caiwuguanli/TeamClear.aspx
/Web/caiwuguanli/srtuankuan_list.aspx
/Web/caiwuguanli/waitkuan.aspx
/Web/jipiao/JiPiao_List.aspx
/Web/jipiao/JiPiao_TuiList.aspx
/Web/SupplierControl/AreaConnect/AreaConnect.aspx
/Web/SupplierControl/AreaConnect/TradeSituation.aspx
/Web/SupplierControl/TicketService/TicketService.aspx
/Web/SupplierControl/TicketService/TradeSituation.aspx
/Web/sanping/SanPing_JiPiaoAdd.aspx
/Web/sanping/VisitorList.aspx
*/
--(20111026)bbl.txt


-- =============================================
-- Author:		
-- Create date: 
-- Description:	
-- History:
-- 1.2011-12-02 汪奇志 付款提醒增加单项服务的未付提醒
-- =============================================
ALTER VIEW [dbo].[View_PayRemind_GetList]
AS
SELECT     Id, UnitName, CompanyId,
                          (SELECT     TOP 1 ContactName, ContactTel
                            FROM          tbl_SupplierContact AS tsc
                            WHERE      tsc.SupplierId = tcs.Id
                            ORDER BY tsc.Id ASC FOR xml auto, root('root')) AS ContactInfo, /*(CASE WHEN SupplierType = 1 THEN
                          (SELECT     isnull(sum(TotalAmount - PayAmount), 0)
                            FROM          tbl_PlanLocalAgency AS tpa
                            WHERE      tpa.CompanyId = tcs.CompanyId AND tpa.TravelAgencyID = tcs.Id) WHEN SupplierType = 2 THEN
                          (SELECT     isnull(sum(TotalAmount - PayAmount), 0)
                            FROM          tbl_PlanTicketOut AS tpto
                            WHERE      tpto.CompanyID = tcs.CompanyId AND tpto.TicketOfficeId = tcs.Id) ELSE 0 END)*/0 AS TotalAmount, 
                      /*(CASE WHEN SupplierType = 1 THEN
                          (SELECT     Operator
                            FROM          tbl_PlanLocalAgency AS tpa
                            WHERE      tpa.CompanyId = tcs.CompanyId AND tpa.TravelAgencyID = tcs.Id FOR xml auto, root('root')) WHEN SupplierType = 2 THEN
                          (SELECT     Operator
                            FROM          tbl_PlanTicketOut AS tpto
                            WHERE      tpto.CompanyID = tcs.CompanyId AND tpto.TicketOfficeId = tcs.Id FOR xml auto, root('root')) ELSE '' END)*/'' AS OperatorInfo 
	,SupplierType
	,(SELECT ISNULL(SUM(TotalAmount-PayAmount),0) FROM tbl_PlanLocalAgency AS A WHERE A.CompanyId=tcs.CompanyId AND A.TravelAgencyId=tcs.Id)--地接支出
	+(SELECT ISNULL(SUM(TotalAmount-PayAmount),0) FROM tbl_PlanTicketOut AS A WHERE A.CompanyId=tcs.CompanyId AND A.TicketOfficeId=tcs.Id AND A.State=3)--机票支出
	+(SELECT ISNULL(SUM(TotalAmount-PaidAmount),0) FROM tbl_PlanSingle AS A WHERE A.SupplierId =tcs.Id) --单项支出
	AS Amount
	,(SELECT Operator AS [Name] FROM tbl_PlanLocalAgency AS A WHERE A.CompanyId=tcs.CompanyId AND A.TravelAgencyId=tcs.Id FOR XML RAW,ROOT('root')) AS PlanerXmlDJ--地接计调
	,(SELECT Operator AS [Name] FROM tbl_PlanTicketOut AS A WHERE A.CompanyId=tcs.CompanyId AND A.TicketOfficeId=tcs.Id AND A.State=3 FOR XML RAW,ROOT('root')) AS PlanerXmlJP--机票计调
	,(SELECT B.ContactName AS [Name] FROM tbl_PlanSingle AS A INNER JOIN tbl_CompanyUser AS B ON A.OperatorId=B.Id WHERE A.SupplierId=tcs.Id FOR XML RAW,ROOT('root')) AS PlanerXmlDX--单项计调
FROM         tbl_CompanySupplier AS tcs
WHERE     IsDelete = '0'
GO


--安排地接地接社需知及备注不能输入太多字符BUG
go
alter table tbl_PlanLocalAgency 
alter column [Notice] nvarchar(max)
alter table tbl_PlanLocalAgency 
alter column [Remark] nvarchar(max)
go

-- =============================================
-- Author:		<Author,,李焕超>
-- Create date: <Create Date,,>
-- Description:	<Description,安排地接,>
-- =============================================
ALTER PROCEDURE [dbo].[proc_PlanTravelAgency_Add] 
(
            @ID char(36),
            @AddAmount money,---增加费用
            @Commission money,---返利
            @Contacter nvarchar(50),---联系人
            @ContactTel nvarchar(50),--联系人电话
            @DeliverTime datetime,---送团时间
            @Fee money,--团款
            @LocalTravelAgency nvarchar(250),---地接社
            @Notice nvarchar(max),---地接社需知
            @Operator nvarchar(50),---操作员
            @OperatorID int,----操作员编号
            @PayType tinyint,---支付方式
            @ReceiveTime datetime,---接团时间
            @ReduceAmount money,----减少金额
            @Remark nvarchar(max),--备注
            @Settlement money,---结算金额
            @TotalAmount money,--总金额
            @TourId char(36),---团号
            @TravelAgencyID int,--地接社编号
            @PriceInfoListXML nvarchar(max),---价格组成</root><PriceInfo PriceType=项目类型 ID= PeopleCount= Price=  Remark= Title=  PriceTpyeId=项目编号 /></root>
            @PlanInfoListXML nvarchar(max),---接待行程<root><TourPlanInfo Days=第几天 /></root>
            @CompanyId  int,
            @Result int output ---返回是否成功

         
)
AS
BEGIN
	set @Result=0
	declare @hdoc int
    begin transaction addTravel ---开始事务
   -----------插入地接社信息
    begin 
    INSERT INTO  [tbl_PlanLocalAgency]
           ([ID]
           ,[TourId]
           ,[LocalTravelAgency]
           ,[TravelAgencyID]
           ,[Contacter]
           ,[ContactTel]
           ,[ReceiveTime]
           ,[DeliverTime]
           ,[Fee]
           ,[Commission]
           ,[Settlement]
           ,[PayType]
           ,[Notice]
           ,[Remark]
           ,[OperatorID]
           ,[Operator]
           ,[OperateTime]
           ,[AddAmount]
           ,[ReduceAmount]
           ,[TotalAmount]
            ,[CompanyId])
     VALUES
        (  @ID
           ,@TourId
           ,@LocalTravelAgency
           ,@TravelAgencyID
           ,@Contacter
           ,@ContactTel
           ,@ReceiveTime
           ,@DeliverTime
           ,@Fee
           ,@Commission
           ,@Settlement
           ,@PayType
           ,@Notice
           ,@Remark
           ,@OperatorID
           ,@Operator
           ,getdate()
           ,@AddAmount
           ,@ReduceAmount
           ,@TotalAmount
           ,@CompanyId  
     )
    end 
    if @@error>0
 ------出错处理
    begin
      rollback transaction addTravel 
      set @Result=0
      return @Result 
    end

---------写入价格组成
 if(@PriceInfoListXML !='' and @PriceInfoListXML is not null)
 begin
        EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@PriceInfoListXML
        INSERT INTO [tbl_PlanLocalAgencyPrice]
           ([PriceTpyeId]
           ,[Title]
           ,[Price]
           ,[Remark]
           ,[PeopleCount]
           ,[ReferenceID]
           ,[PriceType])
        select 
           [PriceTpyeId]
           ,[Title]
           ,[Price]
           ,[Remark]
           ,[PeopleCount]
           ,@ID
           ,[PriceType]
         from  OPENXML(@hdoc,'/ROOT/PriceInfo')
         with(PriceTpyeId tinyint,Title nvarchar(50), Price money, Remark nvarchar(1000), PeopleCount int, ReferenceID char(36), PriceType tinyint)
         EXECUTE sp_xml_removedocument @hdoc
         
         if(@@error>0)
          begin
            EXECUTE sp_xml_removedocument @hdoc
            set @Result=0
            rollback transaction addTravel
            return @Result
          end
 end

---------写入行程安排
 if(@PlanInfoListXML !='' and @PlanInfoListXML is not null and len(@ID)>0)
 begin
  EXECUTE SP_XML_PREPAREDOCUMENT @hdoc output,@PlanInfoListXML
  INSERT INTO [tbl_PlanLocalAgencyTourPlan]
           ([LocalTravelId]
           ,[Days])
  select @ID,Days from openxml(@hdoc,'/ROOT/TourPlanInfo')
  WITH(Days tinyint)
  execute sp_xml_removedocument @hdoc
------出错判断  
  if(@@error>0)
  begin
   set @Result =0
   execute sp_xml_removedocument @hdoc
   rollback transaction addTravel
   return @Result
  end
 end

 ----无错返回
commit transaction addTravel
set @Result =1

return @Result

END
GO

-- =============================================
-- Author:		<Author,,李焕超>
-- Create date: <Create Date,2011-02-23,>
-- Description:	<Description,修改安排地接,>
-- =============================================
ALTER PROCEDURE [dbo].[proc_PlanTravelAgency_Update]
(
            @ID char(36),
            @AddAmount money,---增加费用
            @Commission money,---返利
            @Contacter nvarchar(50),---联系人
            @ContactTel nvarchar(50),--联系人电话
            @DeliverTime datetime,---送团时间
            @Fee money,--团款
            @LocalTravelAgency nvarchar(250),---地接社
            @Notice nvarchar(max),---地接社需知
            @OperateTime datetime, ---操作时间
            @Operator nvarchar(50),---操作员
            @OperatorID int,----操作员编号
            @PayType tinyint,---支付方式
            @ReceiveTime datetime,---接团时间
            @ReduceAmount money,----减少金额
            @Remark nvarchar(max),--备注
            @Settlement money,---结算金额
            @TotalAmount money,--总金额
            @TourId char(36),---团号
            @TravelAgencyID int,--地接社编号
            @PriceInfoListXML nvarchar(max),---价格组成</root><PriceInfo PriceType=项目类型 ID= PeopleCount= Price=  Remark= Title=  PriceTpyeId=项目编号 /></root>
            @PlanInfoListXML nvarchar(max),---接待行程<root><TourPlanInfo Days=第几天 /></root>
            @Result int output ---返回是否成功

         
)
AS
BEGIN
	declare @hdoc int
    begin transaction addTravel ---开始事务
   -----------插入地接社信息
    begin 
    declare @TempAddAmount money
    declare @TempReduceAmount money
    set @TempAddAmount =0
    set @TempReduceAmount =0
    select @TempAddAmount=AddAmount, @TempReduceAmount=ReduceAmount from  tbl_PlanLocalAgency where id=@ID
    update  [tbl_PlanLocalAgency] set 
           [TourId]=@TourId
           ,[LocalTravelAgency]=@LocalTravelAgency
           ,[TravelAgencyID]=@TravelAgencyID
           ,[Contacter]=@Contacter
           ,[ContactTel]=@ContactTel
           ,[ReceiveTime]=@ReceiveTime
           ,[DeliverTime]=@DeliverTime
           ,[Fee]=@Fee
           ,[Commission]=@Commission
           ,[Settlement]=@Settlement
           ,[PayType]=@PayType
           ,[Notice]=@Notice
           ,[Remark]=@Remark
           ,[OperatorID]=@OperatorID
           ,[Operator]=@Operator
           ,[OperateTime]=@OperateTime
           ,[TotalAmount]=(@Settlement +@TempAddAmount -@TempReduceAmount) where id=@ID
     
    end 
    if @@error>0
 ------出错处理
    begin
      rollback transaction addTravel 
      set @Result=0
      return @Result 
    end

---------写入价格组成
 if(@PriceInfoListXML !='' and @PriceInfoListXML is not null)
 begin
        delete from [tbl_PlanLocalAgencyPrice] where [ReferenceID]=@ID
        EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@PriceInfoListXML
        INSERT INTO [tbl_PlanLocalAgencyPrice]
           (
            [PriceTpyeId]
           ,[Title]
           ,[Price]
           ,[Remark]
           ,[PeopleCount]
           ,[ReferenceID]
           ,[PriceType])
        select 
            [PriceTpyeId]
           ,[Title]
           ,[Price]
           ,[Remark]
           ,[PeopleCount]
           ,@ID
           ,PriceType
         from  OPENXML(@hdoc,'/ROOT/PriceInfo')
         with(PriceType tinyint,Title nvarchar(50), Price money, Remark nvarchar(1000), PeopleCount int, PriceTpyeId tinyint)
         EXECUTE sp_xml_removedocument @hdoc
         
         if(@@error>0)
          begin
            EXECUTE sp_xml_removedocument @hdoc
            set @Result=0
            rollback transaction addTravel
            return @Result
          end
 end

---------写入行程安排
 if(@PlanInfoListXML !='' and @PlanInfoListXML is not null)
 begin
  delete from [tbl_PlanLocalAgencyTourPlan] where [LocalTravelId]=@ID
  EXECUTE SP_XML_PREPAREDOCUMENT @hdoc output,@PlanInfoListXML
  INSERT INTO [tbl_PlanLocalAgencyTourPlan]
           ([LocalTravelId]
           ,[Days])
  select @ID,Days from openxml(@hdoc,'/ROOT/TourPlanInfo')
  WITH(LocalTravelId char(36),Days tinyint)
  execute sp_xml_removedocument @hdoc
------出错判断  
  if(@@error>0)
  begin
   set @Result =0
   execute sp_xml_removedocument @hdoc
   rollback transaction addTravel
   return @Result
  end
 end

 ----无错返回
commit transaction addTravel
set @Result =1
return @Result

END
GO
--安排地接地接社需知及备注不能输入太多字符BUG


--删除团队时团款支出登记同时删除
-- =============================================
-- Author:		周文超
-- Create date: 2011-04-25
-- Description:	删除团队信息（虚拟删除）
-- Error：0参数错误
-- Success：1删除成功
-- History:
-- 1.2011-12-16 汪奇志 增加团款支出登记的删除
-- =============================================
ALTER PROCEDURE [dbo].[proc_Tour_UpdateIsDelete]
	@TourId char(36),		--团队Id
	@ErrorValue int output   --错误代码
AS
BEGIN
	/*
	1、更新团队表Isdelete
	2、订单表
	3、所有收入，支出表
	4、计调表（地接、票务）
	5、维护线路的上团数、收客数
	*/

	if @TourId is null or len(@TourId) <> 36
	begin
		set @ErrorValue = 0
		return
	end

	declare @errorcount int  --错误记数器
	declare @TourCount int --团队数
	declare @CustomerCount int  --游客数
	declare @RouteId int   --线路Id
	set @TourCount = 0
	set @CustomerCount = 0
	set @errorcount = 0

	begin tran UpdateTour
	
	--1、更新团队表Isdelete
	UPDATE [tbl_Tour] SET [IsDelete] = '1' WHERE TourId = @TourId;
	SET @errorcount = @errorcount + @@ERROR

	--订单表
	UPDATE [tbl_TourOrder] SET [IsDelete] = '1' WHERE TourId = @TourId;
	SET @errorcount = @errorcount + @@ERROR
	
	--订单游客退票航段信息
	delete from tbl_CustomerRefundFlight where CustomerId in (select ID from tbl_TourOrderCustomer where tbl_TourOrderCustomer.OrderId in (select ID from tbl_TourOrder where tbl_TourOrder.TourId = @TourId))
	SET @errorcount = @errorcount + @@ERROR

	--订单游客退票信息
	delete from tbl_CustomerRefund where CustormerId in (select ID from tbl_TourOrderCustomer where tbl_TourOrderCustomer.OrderId in (select ID from tbl_TourOrder where tbl_TourOrder.TourId = @TourId))
	SET @errorcount = @errorcount + @@ERROR

	--团款收入信息
	delete from tbl_ReceiveRefund where tbl_ReceiveRefund.ItemType = 1 and tbl_ReceiveRefund.ItemId in (select ID from tbl_TourOrder where tbl_TourOrder.TourId = @TourId)
	SET @errorcount = @errorcount + @@ERROR

	--团款支出登记
	DELETE FROM [tbl_FinancialPayInfo] WHERE [ItemId]=@TourId
	SET @errorcount=@errorcount+@@ERROR

	--团款其他收入信息
	delete from tbl_ReceiveRefund where tbl_ReceiveRefund.ItemType = 2 and tbl_ReceiveRefund.ItemId  = @TourId
	SET @errorcount = @errorcount + @@ERROR
	delete from tbl_TourOtherCost where TourId = @TourId
	SET @errorcount = @errorcount + @@ERROR

	--所有收入，支出表
	UPDATE [tbl_StatAllIncome] SET  [IsDelete] = '1' WHERE TourId = @TourId;
	SET @errorcount = @errorcount + @@ERROR
	
	UPDATE [tbl_StatAllOut] SET  [IsDelete] = '1' WHERE TourId = @TourId;
	SET @errorcount = @errorcount + @@ERROR

	--计调表（地接、票务）
	delete from tbl_PlanTicket where TourId =  @TourId
	SET @errorcount = @errorcount + @@ERROR
	delete from tbl_PlanTicketOutCustomer where TicketOutId in (select ID from tbl_PlanTicketOut where TourId = @TourId)
	SET @errorcount = @errorcount + @@ERROR
	delete from tbl_PlanTicketFlight where TicketId in (select ID from tbl_PlanTicketOut where TourId = @TourId)
	SET @errorcount = @errorcount + @@ERROR
	delete from tbl_PlanTicketKind where TicketId in (select ID from tbl_PlanTicketOut where TourId = @TourId)
	SET @errorcount = @errorcount + @@ERROR
	delete from tbl_PlanTicketOut where TourId = @TourId
	SET @errorcount = @errorcount + @@ERROR
	

	delete from tbl_PlanLocalAgencyPrice where ReferenceID in (select ID from tbl_PlanLocalAgency where TourId = @TourId)
	SET @errorcount = @errorcount + @@ERROR
	delete from tbl_PlanLocalAgencyTourPlan where LocalTravelId in (select ID from tbl_PlanLocalAgency where TourId = @TourId)
	SET @errorcount = @errorcount + @@ERROR
	delete from tbl_PlanLocalAgency where TourId = @TourId
	SET @errorcount = @errorcount + @@ERROR

	--线路Id赋值
	select @RouteId = RouteId from tbl_Tour where TourId = @TourId
	--上团数赋值
	select @TourCount = count(*) from tbl_Tour as a where a.RouteId = @RouteId and a.IsDelete = '0' and a.TemplateId <> ''
	--收客数赋值
	select @CustomerCount = sum(PeopleNumber) from tbl_TourOrder as a where a.TourId in (select b.TourId from tbl_Tour as b where b.RouteId =@RouteId and b.IsDelete = '0') and a.IsDelete = '0' and a.OrderState not in (3,4)
	--维护线路区域的上团数、收客数
	update tbl_Route set TourCount = isnull(@TourCount,0),VisitorCount = isnull(@CustomerCount,0) where ID = @RouteId
	SET @errorcount = @errorcount + @@ERROR

	IF @ErrorCount > 0 
	BEGIN
		SET @ErrorValue = -1;
		ROLLBACK TRAN UpdateTour
	end	
	ELSE
	BEGIN
		SET @ErrorValue = 1;
		COMMIT TRAN UpdateTour
	end

END
GO
--删除团队时团款支出登记同时删除


--2011-12-16 汪奇志 修改分页存储过程
--存储过程分页,在SqlDataReader中使用
--history:
--1.修改SET @strSQL中的表名的别名为@TableName  2010-5-27  zhangzy
ALTER PROCEDURE [dbo].[T_StocPage]
(    
    @PageSize     int = 10,           -- 页尺寸
    @PageIndex    int = 1,            -- 页码
    @TableName      nvarchar(255),       -- 表名
    @IdentityColumnName      nvarchar(255)='AAA',       -- 主键字段名 
    @FieldsList	  nvarchar(2000),	      --列名
    @FieldSearchKey     nvarchar(2000) = '', -- 查询条件 (注意: 不要加 where)
    @OrderString      nvarchar(1000),       -- 排序  
    @IsGroupBy   char(1) = '0'    --条件是否分组
)
AS
declare @RecordCount int	      --记录总数[返回]
declare @PageCount int
declare @strSQL   varchar(4000)       -- 主语句
declare @strTmp   nvarchar(2000)        -- 临时变量
declare @PageLowerBound int	--当前页面第一条记录的位置序数
declare @PageUpperBound int		--当前页面最后一条记录位置序数
SET @PageCount = 0
------------------------排序条件--------------------------
if @OrderString IS NOT NULL AND @OrderString != ''--是否存在查询条件
	SET @OrderString = 'ORDER BY '+@OrderString
ELSE
	SET @OrderString = ''
------------------------查询条件--------------------------
if @FieldSearchKey IS NOT NULL AND @FieldSearchKey != ''--是否存在查询条件
	IF @IsGroupBy IS NOT NULL AND @IsGroupBy = '1'
		SET @strTmp = ' select @RecordCount=count(*) from (SELECT COUNT(*) AS C FROM [' + @TableName + ']'+' WHERE ' + @FieldSearchKey + ') as t '
	else
		SET @strTmp = 'SELECT @RecordCount=COUNT(*) FROM [' + @TableName + ']'+' WHERE ' + @FieldSearchKey
ELSE
	SET @strTmp = 'SELECT @RecordCount=COUNT(*) FROM [' + @TableName + ']'

EXECUTE sp_executesql  @strTmp,N'@RecordCount int OUTPUT',@RecordCount OUTPUT
SET @PageCount=ceiling(@RecordCount*1.0/@PageSize)
---------------------------计算总计录数结束----------------------------------
if @PageIndex >@PageCount
	SET @PageIndex = @PageCount
if @PageCount=0
	SET @PageIndex = 1
if @PageIndex<=0
	SET @PageIndex = 1
--判断记录数是否大于总记录数
if (@PageIndex-1)*@PageSize>@RecordCount
begin
	SET @PageIndex= @PageCount
end
if @FieldSearchKey IS NOT NULL AND @FieldSearchKey != ''--是否存在查询条件
	SET @strTmp = N' WHERE ' + @FieldSearchKey
ELSE
	SET @strTmp = N''

--当前页面第一条记录的位置序数
SET @PageLowerBound=(@PageIndex-1)* @PageSize +1
--当前页面最后一条记录的位置序数
SET @PageUpperBound = @PageLowerBound + @PageSize -1
--设置返回记录的SQL语句
---------------------------ROW_NUMBER 分页---------------------
--SET @strSQL = 'SELECT * FROM ( SELECT  ' + @FieldsList + ',ROW_NUMBER() OVER(' + @OrderString + ') AS RowNumber FROM [' + @TableName + '] ' + @strTmp + ' ) '+@TableName+' WHERE RowNumber BETWEEN ' + Cast(@PageLowerBound as nvarchar) + ' AND ' + Cast(@PageUpperBound as nvarchar)+' ' + @OrderString
SET @strSQL = 'SELECT t120.* FROM ( SELECT  ' + @FieldsList + ',ROW_NUMBER() OVER(' + @OrderString + ') AS RowNumber FROM [' + @TableName + '] ' + @strTmp + ' ) AS t120 WHERE RowNumber BETWEEN ' + Cast(@PageLowerBound as nvarchar) + ' AND ' + Cast(@PageUpperBound as nvarchar)+' ORDER BY RowNumber ASC'
select @RecordCount as RecordCount
PRINT @strSQL

EXECUTE(@strSQL)
GO
--2011-12-16 汪奇志 修改分页存储过程


--2011-12-19 汪奇志 机票申请表增加账务审核时间
GO
ALTER TABLE dbo.tbl_PlanTicketOut ADD
	VerifyTime datetime NULL
GO
DECLARE @v sql_variant 
SET @v = N'财务审核时间'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_PlanTicketOut', N'COLUMN', N'VerifyTime'
GO
GO
--2011-12-19 汪奇志 机票申请退票表增加账务审核时间
ALTER TABLE dbo.tbl_PlanTicket ADD
	VerifyTime datetime NULL
GO
DECLARE @v sql_variant 
SET @v = N'财务审核时间(关联类型为申请机票时记录该申请财务审核时间)'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_PlanTicket', N'COLUMN', N'VerifyTime'
GO


--销售员（用于人次统计）修改开始
--2011-12-19 汪奇志 订单表增加字段：销售员（用于人次统计）
GO
ALTER TABLE dbo.tbl_TourOrder ADD
	PerTimeSellerId int NOT NULL CONSTRAINT DF_tbl_TourOrder_PerTimeSellerId DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'销售员（用于人次统计）
客户单位自行下单记录客户单位的责任销售
专线后台下单记录操作人'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOrder', N'COLUMN', N'PerTimeSellerId'
GO
--数据处理 销售员（用于人次统计）客户单位自行下单记录客户单位的责任销售，专线后台下单记录操作人
UPDATE tbl_TourOrder SET PerTimeSellerId=SalerId WHERE OrderType=0
UPDATE tbl_TourOrder SET PerTimeSellerId=OperatorId WHERE OrderType IN(1,2,3)
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
				,A.TourCode,@RouteId,@RouteName,@TourDays,@PlanPeopleNumber
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
		END
	END

	
	IF(@TourType=1)--团队计划
	BEGIN
		DECLARE @SellerId INT--销售员编号
		DECLARE @SellerName NVARCHAR(255)--销售员姓名
		DECLARE @CommissionType	TINYINT--返佣类型
		DECLARE @CommissionPrice MONEY--返佣金额（单价）
		DECLARE @IncomeAmount MONEY--订单收入金额
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
			,@SellerId,@OperatorName,@OperatorId,0,0
			,0,0,@PlanPeopleNumber,0,0
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
				,[CustomerStatus])
			SELECT A.TravelerId,@OrderId,@CompanyId,A.TravelerName,A.IdType
				,A.IdNo,A.Gender,A.TravelerType,A.ContactTel,GETDATE()
				,0 FROM OPENXML(@hdoc,'/ROOT/Info')
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
GO

-- =============================================
-- Author:		luofx
-- Create date: 2011-01-24
-- Description:	新增订单
-- History:
-- 1.增加组团联系人信息、返佣信息 汪奇志 2011-06-08
-- 2.修正人数验证（减去已退团人数）
-- 3.2011-12-20 汪奇志 增加对销售员（用于人次统计）字段的处理
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
	SET @ErrorCount=0
	SET @Result=1
BEGIN
	DECLARE @PerTimeSellerId INT--销售员（用于人次统计）
	SET @PerTimeSellerId=@OperatorID
	--判断订单人数是否大于剩余人数
	SELECT @VirtualPeopleNumber=VirtualPeopleNumber,@PlanPeopleNumber=PlanPeopleNumber FROM tbl_Tour WHERE TourId=@TourId  AND IsDelete=0
	SELECT @ShiShu = ISNULL(SUM(PeopleNumber - LeaguePepoleNum),0) FROM tbl_TourOrder WHERE IsDelete=0 AND TourId=@TourId AND OrderState IN(1,2,5)
	
	--该团所有订单人数大于团队计划人数，则不允许修改
	if @PlanPeopleNumber - @ShiShu - @PeopleNumber < 0
	begin
		SET @Result=2
	end
	
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
				,CommissionType,CommissionPrice,PerTimeSellerId)
			VALUES(@OrderID,@OrderNO,@TourId,@AreaId,@RouteId,@RouteName,@TourNo,@TourClassId
				,@LeaveDate,@TourDays,@LeaveTraffic,@ReturnTraffic,@OrderType,@OrderState,@BuyCompanyID
				,@BuyCompanyName,@ContactName,@ContactTel,@ContactMobile,@ContactFax,@SalerName
				,@SalerId,@OperatorName, @OperatorID,@PriceStandId,@PersonalPrice
				,@ChildPrice,@MarketPrice,@AdultNumber,@ChildNumber,@MarketNumber
				,@PeopleNumber,@OtherPrice,@SaveSeatDate,@OperatorContent
				,@SpecialContent,@SumPrice,@SellCompanyName,@SellCompanyId,@LastDate
				,@LastOperatorID,getdate(),@ViewOperatorId,@CustomerDisplayType,@CustomerFilePath
				,@CustomerLevId,@SumPrice,getdate(),@BuyerContactId,@BuyerContactName
				,@CommissionType,@CommissionPrice,@PerTimeSellerId)
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
GO
-- =============================================
-- Author:		周文超
-- Create date: 2010-01-21
-- Description:	人次统计基本视图
-- 周文超 2011-06-20增加出团日期列
-- 2011-12-20 汪奇志 人次统计销售员改用人次统计销售员列
-- =============================================
ALTER VIEW [dbo].[View_InayatStatistic]
AS
select 
DepartId,DepartName,
PerTimeSellerId AS SalerId,
tbl_CompanyUser.ContactName as SalerName,
ViewOperatorId,
SellCompanyId,AreaId,tbl_TourOrder.IssueTime,tbl_TourOrder.IsDelete,TourClassId,OrderState,LeaveDate,
(PeopleNumber - LeaguePepoleNum) as PeopleCount,
((PeopleNumber - LeaguePepoleNum) * cast(TourDays as decimal)) as PeopleDays
,(select OperatorId from tbl_Tour where tbl_Tour.TourId = tbl_TourOrder.TourId) as TourOperatorId
from tbl_TourOrder left join tbl_CompanyUser on tbl_CompanyUser.Id = tbl_TourOrder.PerTimeSellerId
where tbl_TourOrder.isdelete = '0' and tbl_TourOrder.IsDelete = '0' and tbl_TourOrder.OrderState not in (3,4) AND tbl_TourOrder.PerTimeSellerId>0
GO
--销售员（用于人次统计）修改结束
--以上脚本已更新至服务器 2011-12-20

--订单游客名单排列顺序BUG BEGIN 汪奇志 2012-01-12
--订单游客信息表增加自增列
ALTER TABLE dbo.tbl_TourOrderCustomer ADD
	IdentityId int NOT NULL IDENTITY (1, 1)
GO
DECLARE @v sql_variant 
SET @v = N'自增编号'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOrderCustomer', N'COLUMN', N'IdentityId'
GO

-- =============================================
-- Author:		luofx
-- Create date: 2011-01-23
-- Description:	团队计划-修改(增加)游客信息   
-- History:
-- 1.汪奇志 2012-01-12 临时表字段[Vid]变更成标识列
-- =============================================
ALTER PROCEDURE [dbo].[proc_TourOrderCustomer_Update]
	@TourCustomerXML NVARCHAR(MAX),  --收退款XML详细信息
	@OrderId CHAR(36),	   --订单编号
	@Result INT OUTPUT     --回传参数
AS
DECLARE @ErrorCount INT   --错误累计参数
DECLARE @hdoc INT         --XML使用参数
DECLARE @i INT
DECLARE @MAXID INT				   --暂存表最大vid
DECLARE @ID CHAR(36)			   --游客编号
DECLARE @CompanyId INT             --公司编号
DECLARE @VisitorName NVARCHAR(250) --游客姓名
DECLARE @CradType INT			   --证件类型
DECLARE @CradNumber NVARCHAR(250)  --证件号
DECLARE @Sex INT				   --性别
DECLARE @VisitorType INT		   --游客类型
DECLARE @ContactTel NVARCHAR(250)  --电话号码
--特服
DECLARE	@ProjectName NVARCHAR(1000)--项目名称
DECLARE	@ServiceDetail NVARCHAR(1000)--服务内容
DECLARE	@IsAdd TINYINT --增/减
DECLARE	@Fee MONEY --费用
DECLARE @IsDelete INT
SET @ErrorCount=0
SET @Result=0
SET @i=1
BEGIN
	CREATE TABLE #TmpTourCustomer_Update(
	   [Vid] INT NOT NULL IDENTITY (1, 1),
	   [ID] CHAR(36),				--游客编号
	   [CompanyId] INT,             --公司编号
	   [OrderId] CHAR(36),          --订单编号
	   [VisitorName] NVARCHAR(250), --游客姓名
	   [CradType] INT,				--证件类型
	   [CradNumber] NVARCHAR(250),	--证件号
	   [Sex] INT,					--性别
	   [VisitorType] INT,			--游客类型
	   [ContactTel] NVARCHAR(250),	--电话号码
	   [ProjectName] NVARCHAR(1000),--项目名称
	   [ServiceDetail] NVARCHAR(1000),--服务内容
	   [IsAdd] TINYINT, --增/减
	   [IsDelete] INT,
	   [Fee] MONEY --费用
	)
	BEGIN TRANSACTION TourCustomer_Update	
		--插入暂存表信息
		EXEC sp_xml_preparedocument @hdoc OUTPUT, @TourCustomerXML
			INSERT INTO #TmpTourCustomer_Update(/*[Vid],*/[ID],CompanyId,OrderId,VisitorName,CradType,
				CradNumber,Sex,VisitorType,ContactTel,ProjectName,ServiceDetail,IsAdd,Fee,[IsDelete])
				SELECT /*row_number() OVER(ORDER BY [ID] DESC) AS RowNumber,*/[ID],CompanyId,OrderId=@OrderId,VisitorName,CradType,
					CradNumber,Sex,VisitorType,ContactTel,ProjectName,ServiceDetail,IsAdd,Fee,[IsDelete] FROM 
				OPENXML(@hdoc,N'/ROOT/TourOrderCustomer') 
					WITH([Vid] INT,[ID] CHAR(36),CompanyId INT,OrderId CHAR(36),VisitorName NVARCHAR(250),CradType INT,
						CradNumber NVARCHAR(250),Sex INT,VisitorType INT,ContactTel NVARCHAR(250)
						,ProjectName NVARCHAR(250),ServiceDetail NVARCHAR(1000),IsAdd TINYINT,Fee MONEY,[IsDelete] INT)
			SET @ErrorCount = @ErrorCount + @@ERROR			
		EXEC sp_xml_removedocument @hdoc
		SELECT @MAXID=MAX(Vid) FROM #TmpTourCustomer_Update
		WHILE(@i<=@MAXID AND @ErrorCount=0)
			BEGIN
				--给变量赋值
				SELECT @ID=[id],@CompanyId=CompanyId,@OrderId=OrderId,@VisitorName=VisitorName,@CradType=CradType,@CradNumber=CradNumber,
					@Sex=Sex,@VisitorType=VisitorType,@ContactTel=ContactTel,@ProjectName=ProjectName,
					@ServiceDetail=ServiceDetail,@IsAdd=IsAdd,@Fee=Fee,@IsDelete=[IsDelete] FROM #TmpTourCustomer_Update WHERE [Vid]=@i									
				print @IsDelete
				IF(@IsDelete=1)--当是需要删除的游客时
					BEGIN	
						IF(NOT EXISTS(SELECT CustormerId FROM [tbl_CustomerRefund] WHERE CustormerId=@ID))	
							BEGIN			    
								DELETE FROM [tbl_CustomerSpecialService] WHERE CustormerId=@ID
								DELETE FROM [tbl_TourOrderCustomer] WHERE [id]=@ID
							END
					END
				ELSE
					BEGIN
						--游客维护
						IF(EXISTS(SELECT 1 FROM [tbl_TourOrderCustomer] WHERE [id]=@ID))
							BEGIN
								UPDATE [tbl_TourOrderCustomer] SET [VisitorName]=@VisitorName,
									[CradType]=@CradType,[CradNumber]=@CradNumber,[Sex]=@Sex,VisitorType=@VisitorType,[ContactTel]=@ContactTel
								WHERE [id]=@ID
								SET @ErrorCount = @ErrorCount + @@ERROR	
								IF(LEN(@ProjectName)>0 AND LEN(@ID)>0)
									BEGIN					
										UPDATE 	[tbl_CustomerSpecialService] SET ProjectName=@ProjectName,ServiceDetail=@ServiceDetail,
											IsAdd=IsAdd,@Fee=@Fee WHERE CustormerId=@ID
										SET @ErrorCount = @ErrorCount + @@ERROR
									END
							END
						ELSE
							BEGIN
								INSERT INTO [tbl_TourOrderCustomer](id,[CompanyId],[OrderId],[VisitorName],
									[CradType],[CradNumber],[Sex],[VisitorType],[ContactTel])
								VALUES(@ID,@CompanyId,@OrderId,@VisitorName,
									@CradType,@CradNumber,@Sex,@VisitorType,@ContactTel)
								SET @ErrorCount = @ErrorCount + @@ERROR																
							END
						--特服维护
						
						IF(LEN(@ProjectName)>0)
							BEGIN
								IF(EXISTS(SELECT 1 FROM [tbl_CustomerSpecialService] WHERE CustormerId=@ID))
									BEGIN								
										UPDATE [tbl_CustomerSpecialService] SET ProjectName=@ProjectName,ServiceDetail=@ServiceDetail,IsAdd=@IsAdd,Fee=@Fee	
											WHERE CustormerId=@ID										
									END
								ELSE
									BEGIN
										INSERT INTO [tbl_CustomerSpecialService](CustormerId,ProjectName,ServiceDetail,IsAdd,Fee)
											VALUES(@ID,@ProjectName,@ServiceDetail,@IsAdd,@Fee)
									END
							END		
					END
				SET @i = @i + 1
				SET @ErrorCount = @ErrorCount + @@ERROR
			END				
		IF(@ErrorCount>0)
			BEGIN
				SET @Result=0
				ROLLBACK TRANSACTION TourCustomer_Update
			END
	COMMIT TRANSACTION TourCustomer_Update
	DROP TABLE #TmpTourCustomer_Update
	SET @Result=1
	RETURN  @Result
END
GO

-- =============================================
-- Author:		luofx
-- Create date: 2011-01-23
-- Description:	团队计划-增加游客信息
-- History:
-- 1.汪奇志 2012-01-12 临时表字段[Vid]变更成标识列
-- =============================================
ALTER PROCEDURE [dbo].[proc_TourOrderCustomer_Insert]
	@TourCustomerXML NVARCHAR(MAX),  --游客XML详细信息:<ROOT><TourOrderCustomer ID="" CompanyId="" OrderId="" VisitorName="" CradType="" CradNumber="" Sex="" VisitorType"" ContactTel="" ProjectName="" ServiceDetail="" IsAdd="" Fee=""/></ROOT>
	@OrderId CHAR(36),	             --订单编号
	@Result INT OUTPUT               --回传参数
AS
DECLARE @ErrorCount INT            --错误累计参数
DECLARE @hdoc INT                  --XML使用参数
DECLARE @MAXID INT				   --暂存表最大vid
DECLARE @i INT
DECLARE @ID CHAR(36)			   --游客编号(新增时必须有值传入)
DECLARE @CompanyId INT             --公司编号
DECLARE @VisitorName NVARCHAR(250) --游客姓名
DECLARE @CradType INT			   --证件类型
DECLARE @CradNumber NVARCHAR(250)  --证件号
DECLARE @Sex INT				   --性别
DECLARE @VisitorType INT		   --游客类型
DECLARE @ContactTel NVARCHAR(250)  --电话号码
--特服
DECLARE	@ProjectName NVARCHAR(1000)  --项目名称
DECLARE	@ServiceDetail NVARCHAR(1000)--服务内容
DECLARE	@IsAdd TINYINT               --增/减
DECLARE	@Fee MONEY                   --费用
SET @ErrorCount=0
SET @Result=0
SET @i=1
BEGIN
	--创建订单游客暂存表
	CREATE TABLE #TmpTourCustomer_Insert(
	   [Vid] INT NOT NULL IDENTITY (1, 1),                   --临时主键
	   [ID] CHAR(36),				--游客编号
	   [CompanyId] INT,             --公司编号
	   [OrderId] CHAR(36),          --订单编号
	   [VisitorName] NVARCHAR(250), --游客姓名
	   [CradType] INT,				--证件类型
	   [CradNumber] NVARCHAR(250),	--证件号
	   [Sex] INT,					--性别
	   [VisitorType] INT,			--游客类型
	   [ContactTel] NVARCHAR(250),	--电话号码
	   [ProjectName] NVARCHAR(1000),--项目名称
	   [ServiceDetail] NVARCHAR(1000),--服务内容
	   [IsAdd] TINYINT, --增/减
	   [Fee] MONEY --费用
	)
	BEGIN TRANSACTION TourCustomer_Insert	
		--将游客XML数据插入到订单游客暂存表	
		EXEC sp_xml_preparedocument @hdoc OUTPUT, @TourCustomerXML
			INSERT INTO #TmpTourCustomer_Insert(/*[Vid],*/[ID],CompanyId,OrderId,VisitorName,CradType,
				CradNumber,Sex,VisitorType,ContactTel,ProjectName,ServiceDetail,IsAdd,Fee)
				SELECT /*row_number() OVER(ORDER BY [ID] DESC) AS RowNumber,*/[ID],CompanyId,OrderId=@OrderId,VisitorName,CradType,
					CradNumber,Sex,VisitorType,ContactTel,ProjectName,ServiceDetail,IsAdd,Fee FROM 
				OPENXML(@hdoc,N'/ROOT/TourOrderCustomer') 
					WITH([ID] CHAR(36),CompanyId INT,OrderId CHAR(36),VisitorName NVARCHAR(250),CradType INT,
						CradNumber NVARCHAR(250),Sex INT,VisitorType INT,ContactTel NVARCHAR(250)
						,ProjectName NVARCHAR(250),ServiceDetail NVARCHAR(1000),IsAdd TINYINT,Fee MONEY)
			SET @ErrorCount = @ErrorCount + @@ERROR			
		EXEC sp_xml_removedocument @hdoc
		--获取最大临时展出表的主键值
		SELECT @MAXID=MAX(vid) FROM #TmpTourCustomer_Insert	
		WHILE(@i<=@MAXID AND @ErrorCount=0)
			BEGIN
				--给变量赋值
				SELECT @ID=[id],@CompanyId=CompanyId,@OrderId=OrderId,@VisitorName=VisitorName,@CradType=CradType,@CradNumber=CradNumber,
					@Sex=Sex,@VisitorType=VisitorType,@ContactTel=ContactTel,@ProjectName=ProjectName,
					@ServiceDetail=ServiceDetail,@IsAdd=IsAdd,@Fee=Fee FROM #TmpTourCustomer_Insert WHERE [Vid]=@i									
				--插入游客数据到游客表中
				INSERT INTO [tbl_TourOrderCustomer](id,[CompanyId],[OrderId],[VisitorName],
					[CradType],[CradNumber],[Sex],[VisitorType],[ContactTel])
					VALUES(@ID,@CompanyId,@OrderId,@VisitorName,
						@CradType,@CradNumber,@Sex,@VisitorType,@ContactTel)
				SET @ErrorCount = @ErrorCount + @@ERROR
				print @ID
				--插入游客特服数据
				IF(LEN(@ProjectName)>0)
					BEGIN
						INSERT INTO [tbl_CustomerSpecialService](CustormerId,ProjectName,ServiceDetail,IsAdd,Fee)
							VALUES(@ID,@ProjectName,@ServiceDetail,@IsAdd,@Fee)										
					END
				SET @i = @i + 1
				SET @ErrorCount = @ErrorCount + @@ERROR
			END
	    --判断是否需要回滚			
		IF(@ErrorCount>0)
			BEGIN
				SET @Result=0
				ROLLBACK TRANSACTION TourCustomer_Insert
			END
	COMMIT TRANSACTION TourCustomer_Insert
	DROP TABLE #TmpTourCustomer_Insert
	SET @Result=1
	RETURN  @Result
END
GO
--订单游客名单排列顺序BUG END 2012-01-12 已更新至服务器

--销售统计订单人数减去退团人数 2012-01-13 汪奇志
-- =============================================
-- Author:		周文超
-- Create date: 2011-04-19
-- Description:	客户关系管理-销售统计视图
-- History:
-- 1.2012-01-13 汪奇志 订单人数减去退团人数
-- =============================================
ALTER VIEW [dbo].[View_SalesStatistics]
AS
select ID,TourId,TourClassId,LeaveDate,SellCompanyId,AreaId,BuyCompanyId,BuyCompanyName,SalerId,SalerName,FinanceSum,(PeopleNumber-LeaguePepoleNum) AS PeopleNumber,OrderState,OperatorID
,(select ProvinceName + '  ' + CityName from tbl_Customer where tbl_Customer.ID = tbl_TourOrder.BuyCompanyID) as ProvinceAndCity  --组团社所在区域
,(case when tbl_TourOrder.OrderType = 0 then tbl_TourOrder.OperatorName else '' end) as OperatorName  --操作员名称
,(select top 1 DepartmentId from tbl_CustomerContactInfo where tbl_CustomerContactInfo.UserId = tbl_TourOrder.OperatorID) as DepartName  --操作员部门
,(case when TourClassId = 2 then '单项服务' else RouteName end) as RouteName   --线路名称
,(case when TourClassId = 2 then (select ServiceType from tbl_PlanSingle where tbl_PlanSingle.TourId = tbl_TourOrder.TourId for xml raw,root('root')) else (select AreaName from tbl_Area where tbl_Area.Id = tbl_TourOrder.AreaId) end) as AreaName   --线路区域名称
,(select OperatorId,(select ContactName from tbl_CompanyUser where tbl_CompanyUser.ID = tbl_TourOperator.OperatorId) as OperatorName from tbl_TourOperator where tbl_TourOperator.TourId = tbl_TourOrder.TourId for xml raw,root('root')) as Logistics  --计调员
from tbl_TourOrder
where tbl_TourOrder.IsDelete = '0' and tbl_TourOrder.OrderState not in (3,4)
GO
--销售统计订单人数减去退团人数 2012-01-13 汪奇志


--销售分析与销售统计数据统计不一致调整 2012-01-30 汪奇志

-- =============================================
-- Author:		周文超
-- Create date: 2011-04-19
-- Description:	客户关系管理-销售统计视图
-- History:
-- 1.2012-01-13 汪奇志 订单人数减去退团人数
-- 2.2012-01-30 汪奇志 增加人次统计销售员编号及姓名列、对方操作员编号及姓名列
-- =============================================
ALTER VIEW [dbo].[View_SalesStatistics]
AS
select ID,TourId,TourClassId,LeaveDate,SellCompanyId,AreaId,BuyCompanyId,BuyCompanyName,SalerId,SalerName,FinanceSum,(PeopleNumber-LeaguePepoleNum) AS PeopleNumber,OrderState,OperatorID
,(select ProvinceName + '  ' + CityName from tbl_Customer where tbl_Customer.ID = tbl_TourOrder.BuyCompanyID) as ProvinceAndCity  --组团社所在区域
,(case when tbl_TourOrder.OrderType = 0 then tbl_TourOrder.OperatorName else '' end) as OperatorName  --操作员名称
,(select top 1 DepartmentId from tbl_CustomerContactInfo where tbl_CustomerContactInfo.UserId = tbl_TourOrder.OperatorID) as DepartName  --操作员部门
,(case when TourClassId = 2 then '单项服务' else RouteName end) as RouteName   --线路名称
,(case when TourClassId = 2 then (select ServiceType from tbl_PlanSingle where tbl_PlanSingle.TourId = tbl_TourOrder.TourId for xml raw,root('root')) else (select AreaName from tbl_Area where tbl_Area.Id = tbl_TourOrder.AreaId) end) as AreaName   --线路区域名称
,(select OperatorId,(select ContactName from tbl_CompanyUser where tbl_CompanyUser.ID = tbl_TourOperator.OperatorId) as OperatorName from tbl_TourOperator where tbl_TourOperator.TourId = tbl_TourOrder.TourId for xml raw,root('root')) as Logistics  --计调员
,PerTimeSellerId--人次统计销售员编号
,(SELECT ContactName,DepartName FROM tbl_CompanyUser WHERE Id=tbl_TourOrder.PerTimeSellerId FOR XML RAW,ROOT('root')) AS PerTimeSeller--人次统计销售员姓名
,BuyerContactId--对方操作员编号
,BuyerContactName--对方操作员姓名
from tbl_TourOrder
where tbl_TourOrder.IsDelete = '0' and tbl_TourOrder.OrderState not in (3,4)
GO

--销售分析与销售统计数据统计不一致调整 2012-01-30 汪奇志  已更新 2012-01-30

--修改订单允许修改组团社信息 2012-02-02 曹胡生
-- =============================================
-- Author:		luofx
-- Create date: 2011-01-24
-- Description:	修改订单
-- History:
-- 1.增加组团联系人信息、返佣信息 汪奇志 2011-06-08
-- 2.增加修改团队计划订单同步修改团队计划信息，修正人数验证（减去已退团人数）
-- 3.修改客户单位(BuyCompanyName,BuyCompanyID,SalerId,SalerName,CommissionType)  曹胡生 2012-02-02
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
	SET @ErrorCount=0
	SET @Result=1
BEGIN	
	--获取之前订单的组团社公司编号
	SELECT @SBuyCompanyID=BuyCompanyID FROM [tbl_TourOrder] WHERE [ID]=@OrderID
	--判断订单人数是否大于剩余人数
    SELECT @OldPeopleNumber = ISNULL(PeopleNumber,0),@TourId=TourId,@TourClassId = TourClassId FROM tbl_TourOrder WHERE ID=@OrderID AND IsDelete=0
	--未处理订单人数+已留位订单人数+已成交订单人数 - 已退团人数
	SELECT @PeopleNumberCount = ISNULL(SUM(PeopleNumber - LeaguePepoleNum),0) FROM tbl_TourOrder WHERE id <> @OrderID AND TourId = @TourId AND (OrderState=1 OR OrderState=2 OR OrderState=5)  AND IsDelete = '0'	
	SELECT @VirtualPeopleNumber = ISNULL(VirtualPeopleNumber,0),@PlanPeopleNumber = ISNULL(PlanPeopleNumber,0) FROM tbl_Tour WHERE TourId = @TourId  AND IsDelete=0
	SELECT @ShiShu = ISNULL(SUM(PeopleNumber - LeaguePepoleNum),0) FROM tbl_TourOrder WHERE IsDelete=0 AND TourId=@TourId AND (OrderState=1 OR OrderState=2 OR OrderState=5)

	--该团所有订单人数大于团队计划人数，则不允许修改
	if @PlanPeopleNumber < @PeopleNumberCount + @PeopleNumber
	begin
		SET @Result=2
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
				,BuyCompanyName=@BuyCompanyName,SalerId=@SalerId,SalerName=@SalerName,CommissionType=@CommissionType,BuyCompanyID=@BuyCompanyID
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
		end
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
GO
--修改订单允许修改组团社信息 已更新  2012-02-02


--按出团时间清理系统业务数据查询团队存储过程 开始
-- =============================================
-- Author:		<曹胡生>
-- Create date: <2012-02-03>
-- Description:	<删除计划业务数据>
-- =============================================
CREATE PROCEDURE [dbo].[proc_Tour_ClearData]
	@CompanyID int,--子系统公司编号
	@LDateStart datetime,--出团日期开始
	@LDateEnd datetime--出团日期结束
AS
BEGIN
	declare @DelTour table(TourId char(36))--要删除的团号
	declare @DelTemplateTuan table(TourId char(36)) --要删除的模板团
	declare @DelZiTuan table(TourId char(36),TemplateId char(36))--要删除的子团
	--要删除的子团
	insert into @DelZiTuan select TourId,TemplateId from tbl_tour where CompanyId=@CompanyID and
	DATEDIFF(DAY,@LDateStart,LeaveDate)>=0 and DATEDIFF(DAY,@LDateEnd,LeaveDate)<=0 and TemplateId<>'' and IsDelete=0
	
	insert into @DelTemplateTuan select TemplateId from @DelZiTuan group by TemplateId
	--将要删除的子团放入删除表中
	insert into @DelTour select TourId from @DelZiTuan
--	declare @ZiTourID char(36) --要删除的子团团号
--	DECLARE ZiTuanCursor CURSOR	FOR SELECT TourId FROM @DelZiTuan
--	OPEN ZiTuanCursor
--	FETCH NEXT FROM ZiTuanCursor INTO @ZiTourID
--	WHILE @@FETCH_STATUS =0
--	BEGIN
--		--exec proc_Tour_UpdateIsDelete @ZiTourID,@Result OUTPUT
--		insert into @DelTour values(@ZiTourID)
--		FETCH NEXT FROM  ZiTuanCursor INTO @ZiTourID
--	END	
--	--关闭游标
--	CLOSE ZiTuanCursor
--	--释放资源
--	DEALLOCATE ZiTuanCursor

	declare @TemplateTourID char(36)
	DECLARE TemplateTuanCursor CURSOR FOR SELECT TourId FROM @DelTemplateTuan
	OPEN TemplateTuanCursor
	FETCH NEXT FROM TemplateTuanCursor INTO @TemplateTourID
	WHILE @@FETCH_STATUS =0
	BEGIN
		--判断该模板下有没有子团，如果没有就删除该模板团
		if(not exists(select 1 from tbl_tour where TourId not in(select TourID from @DelZiTuan 
			where TemplateId=@TemplateTourID) and TemplateId=@TemplateTourID and CompanyId=@CompanyID and IsDelete=0))
		begin
			--由于单项业务与团队计划的模板团编号是自己，避免重复删除
			if(not exists(select 1 from @DelTour where TourId=@TemplateTourID))
			begin
				insert into @DelTour values(@TemplateTourID)
			end
			--exec proc_Tour_UpdateIsDelete @TemplateTourID,@Result OUTPUT
		end
		FETCH NEXT FROM TemplateTuanCursor INTO @TemplateTourID
	END	
	--关闭游标
	CLOSE TemplateTuanCursor
	--释放资源
	DEALLOCATE TemplateTuanCursor
	select * from @DelTour
END
GO
--按出团时间清理系统业务数据查询团队存储过程 结束 2012-02-09 更新至服务器

--修改批量登记BUG S

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-07-04
-- Description:	批量登记付款
-- @ExpenseType:(0:单项服务供应商安排，1:地接，2:票务，255:所有)
-- History:
-- 1.2012-03-13 汪奇志 更新机票付款登记的条件(未出票的不登记)
-- 2.2012-03-26 汪奇志 批量登记时针对对供应商的查询条件（供应商类型、供应商名称） 去除@ExpenseType，通过@GysType来操作
-- =============================================
ALTER PROCEDURE [dbo].[proc_BatchRegisterExpense]
	@PaymentTime DATETIME--支付时间
	,@PaymentType TINYINT--支付类型
	,@PayerId INT--付款人编号
	,@Payer NVARCHAR(50)--付款人姓名
	,@Remark NVARCHAR(255)--备注
	--,@ExpenseType TINYINT=255--支出类型，为255时关联所有支出
	,@OperatorId INT--操作员编号
	,@CompanyId INT--公司编号
	,@TourIdsXML NVARCHAR(MAX)--计划编号集合XML：<ROOT><Info TourId="计划编号" /></ROOT>
	,@Result INT OUTPUT--操作结果
	,@GysName NVARCHAR(255)=NULL
	,@GysType TINYINT=255--供应商类型，为255时关联所有供应商类型
AS
BEGIN
	SET @Result=0

	DECLARE @hdoc INT
	DECLARE @tmptbl_tour TABLE(TourId CHAR(36))
	DECLARE @tmptbl_zhichu TABLE(ZhiChuType TINYINT,ZhiChuLeiBie NVARCHAR(10),TourId CHAR(36),PlanId CHAR(36),Amount MONEY,GysId INT,GysName NVARCHAR(255))
	
	EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@TourIdsXML
	INSERT INTO @tmptbl_tour(TourId) SELECT A.TourId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TourId CHAR(36)) AS A
	EXECUTE sp_xml_removedocument @hdoc

	IF(@GysType=255)--单项支出
	BEGIN
		IF(@GysName IS NOT NULL AND LEN(@GysName)>0)
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuType,ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT 0,'单项',A.TourId,B.PlanId,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.PlanId AND [ReceiveType]=0)),B.SupplierId,B.SupplierName FROM @tmptbl_tour AS A INNER JOIN tbl_PlanSingle AS B
			ON A.TourId=B.TourId AND B.SupplierName LIKE'%'+@GysName+'%'
		END
		ELSE
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuType,ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT 0,'单项',A.TourId,B.PlanId,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.PlanId AND [ReceiveType]=0)),B.SupplierId,B.SupplierName FROM @tmptbl_tour AS A INNER JOIN tbl_PlanSingle AS B
			ON A.TourId=B.TourId
		END
	END

	IF(@GysType=1 OR @GysType=255)--地接支出
	BEGIN
		IF(@GysName IS NOT NULL AND LEN(@GysName)>0)
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuType,ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT 1,'地接',A.TourId,B.Id,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=1)),B.TravelAgencyID,B.LocalTravelAgency FROM @tmptbl_tour AS A INNER JOIN tbl_PlanLocalAgency AS B
			ON A.TourId=B.TourId AND B.LocalTravelAgency LIKE'%'+@GysName+'%'
		END
		ELSE
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuType,ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT 1,'地接',A.TourId,B.Id,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=1)),B.TravelAgencyID,B.LocalTravelAgency FROM @tmptbl_tour AS A INNER JOIN tbl_PlanLocalAgency AS B
			ON A.TourId=B.TourId
		END
	END

	IF(@GysType=2 OR @GysType=255)--机票支出
	BEGIN
		IF(@GysName IS NOT NULL AND LEN(@GysName)>0)
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuType,ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT 2,'机票',A.TourId,B.Id,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=2)),B.TicketOfficeId,B.TicketOffice FROM @tmptbl_tour AS A INNER JOIN tbl_PlanTicketOut AS B
			ON A.TourId=B.TourId AND B.State=3 AND B.TicketOffice LIKE'%'+@GysName+'%'
		END
		ELSE
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuType,ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT 2,'机票',A.TourId,B.Id,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=2)),B.TicketOfficeId,B.TicketOffice FROM @tmptbl_tour AS A INNER JOIN tbl_PlanTicketOut AS B
			ON A.TourId=B.TourId AND B.State=3
		END
	END

	--SELECT * FROM @tmptbl_zhichu

	INSERT INTO [tbl_FinancialPayInfo]([Id],[CompanyID],[ItemId],[ItemType],[ReceiveId]
		,[ReceiveType],[ReceiveCompanyId],[ReceiveCompanyName],[PaymentDate],[StaffNo]
		,[StaffName],[PaymentAmount],[PaymentType],[IsBill],[BillAmount]
		,[BillNo],[Remark],[IsChecked],[OperatorID],[IssueTime]
		,[CheckerId],[IsPay])
	SELECT NEWID(),@CompanyId,A.TourId,0,A.PlanId
		,A.ZhiChuType,A.GysId,A.GysName,@PaymentTime,@PayerId
		,@Payer,A.Amount,@PaymentType,'0',0
		,'',@Remark,'0',@OperatorId,GETDATE()
		,0,'0'
	FROM @tmptbl_zhichu AS A WHERE A.Amount<>0

	SET @Result=1
	RETURN @Result
END
GO
--修改批量登记BUG E


