--�Ƶ����Ż�Ʊ��Ϣ������������˱�ע�ֶ� 2011-08-12
GO
ALTER TABLE dbo.tbl_PlanTicketOut ADD
	ReviewRemark nvarchar(MAX) NULL
GO
DECLARE @v sql_variant 
SET @v = N'������˱�ע'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_PlanTicketOut', N'COLUMN', N'ReviewRemark'
GO

-- =============================================
-- Author:		�����
-- Create date: 2011-03-01
-- Description:	��Ʊ�����޸ġ���Ʊ
-- History:
-- 1.2011-03-30 ����־ ����������д��ʽ
-- 2.2011-04-21 ����־ ��ƱƱ�����Ӱٷֱȡ���������
-- 3.2011-08-15 ����־ ��ɳ�Ʊ��Ʊ���Զ����崦��(�Զ����尴ϵͳ����)
-- =============================================
ALTER PROCEDURE [dbo].[proc_TicketOut_Update]
(   @TicketId char(36) ,--��Ʊ������
    @TourId char(36),
	@RouteName nvarchar(255),--��·����
    @PNR nvarchar(max),
    @Total money, --�ܷ���
	@Notice nvarchar(250),--��Ʊ��֪
	@TicketOffice nvarchar(50),--��Ʊ��Ӧ��
	@TicketOfficeId int,--��Ʊ��Ӧ�̱��
	@Saler nvarchar(50),--����Ա
	@TicketNum nvarchar(50),--Ʊ��
	@PayType nvarchar(50),--֧����ʽ
	@Remark nvarchar(250),--��ע
    @TicketType tinyint,--��Ʊ���ͣ��Ŷ������Ʊ��������Ʊ�����������Ʊ��
    @State tinyint,--״̬
	@Operator nvarchar(250),--������Ա
	@OperateID int,--������Ա���
	@CompanyID int,--��˾���
	@TicketOutTime datetime,   --��Ʊʱ��
	@CustomerXML nvarchar(max),--�ο��޸���ϢXML:<ROOT><Info VisitorName="�ο�����" CradNumber="֤������" CradType="֤������" TravellerId="�οͱ��" /></ROOT>
	@FlightListXML nvarchar(max),--��Ʊ������ϢXML:<ROOT><Info FligthSegment="����" DepartureTime="����ʱ��" AireLine="���չ�˾" Discount="�ۿ�" TicketTime="ʱ��" /></ROOT>
	@TicketKindXML nvarchar(max),--Ʊ������ XML:<ROOT><Info Price="Ʊ���" OilFee="˰/����" PeopleCount="����" AgencyPrice="�����" TotalMoney="Ʊ��" TicketType="Ʊ��" Discount="�ٷֱ�" OtherPrice="��������" /></ROOT>
	@OutCustomerXML nvarchar(max),--��Ʊ�������οͶ�Ӧ��ϵ XML:<ROOT><Info TravellerId="�οͱ��" /></ROOT>
	@Result int output --�������
	,@TicketOutRegisterId CHAR(36) OUTPUT--����ǼǱ��
)
AS
BEGIN
	DECLARE @hdoc INT
	DECLARE @errorcount INT
	SET @errorcount=0
	BEGIN TRANSACTION addTicket

	--��Ʊ������Ϣ
	UPDATE [tbl_PlanTicketOut] SET [PNR] = @PNR,[Total] = @Total,[Notice] = @Notice,[TicketOffice] = @TicketOffice,[TicketOfficeId] = @TicketOfficeId
		,[TicketNum] = @TicketNum,[PayType] = @PayType,[Remark] = @Remark,[State] = @State,[Operator] = @Operator
		,[OperateID] = @OperateID,[TotalAmount] = @Total+[AddAmount]-[ReduceAmount],[TicketOutTime] = @TicketOutTime
	WHERE id=@TicketId
	SET @errorcount=@errorcount+@@ERROR

	IF(@errorcount=0 AND @OutCustomerXML IS NOT NULL AND LEN(@OutCustomerXML)>0)--��Ʊ�������οͶ�Ӧ��ϵ����
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

	IF(@errorcount=0 AND LEN(@CustomerXML)>10)--�ο���Ϣ����
	BEGIN
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@CustomerXML
		UPDATE tbl_TourOrderCustomer SET VisitorName=A.VisitorName,CradType=A.CradType,CradNumber=A.CradNumber
		FROM tbl_TourOrderCustomer AS B INNER JOIN (SELECT VisitorName,CradType,CradNumber,TravellerId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(VisitorName varchar(250),CradNumber varchar(250),CradType int,TravellerId char(36))) AS A
		ON B.Id=A.TravellerId
		SET @errorcount=@errorcount+@@ERROR
		EXECUTE sp_xml_removedocument @hdoc
	END 

	IF(@errorcount=0 AND @FlightListXML IS NOT NULL AND LEN(@FlightListXML)>0)--������Ϣ
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

	IF(@errorcount=0 AND @TicketKindXML IS NOT NULL AND LEN(@TicketKindXML)>0)--Ʊ����Ϣ����
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

	IF(@errorcount=0)--��Ʊ�����б���Ϣ
	BEGIN
		UPDATE [tbl_PlanTicket] SET [TourId] = @TourId,[RouteName] = @RouteName,[Saler] = @Saler
			,[Operator] =@Operator,[OperatorId]=@OperateID,[RegisterTime] = GETDATE(),[State] = @State
		WHERE  RefundId=@TicketId
	END

	IF(@errorcount=0 AND @State=3 AND NOT EXISTS(SELECT 1 FROM [tbl_StatAllOut] WHERE ItemType=2 AND [ItemId]=@TicketId))--֧����ϸ
	BEGIN
		DECLARE @AreaId INT--��·����
		DECLARE @TourType TINYINT --�ƻ�����
		DECLARE @DepartmentId INT--���ű��
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

	IF(@errorcount=0 AND @State=3 AND EXISTS(SELECT 1 FROM tbl_CompanySetting WHERE Id=@CompanyId AND FieldName='IsTicketOutRegisterPayment' AND FieldValue='1'))--��ɳ�Ʊ��Ʊ���Զ����崦��(�Զ����尴ϵͳ����),�Ǽǵĸ���Ĭ���������֧��
	BEGIN
		--ItemType(֧�����������Ŷӻ��ӷ�)=0�Ŷ� ReceiveType(�Ƶ���Ŀ����)=2��Ʊ PaymentType(֧����ʽ)=2 �����ָ�
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
--������־�Ѹ���


--2011-09-02
--��ƱƱ��ٷֱ��ֶ��������͸���
alter table tbl_PlanTicketKind 
alter column [Discount] decimal(10,6) not null
go
--��Ʊ�����ۿ��ֶ��������͸���
alter table tbl_PlanTicketFlight
alter column [Discount] decimal(10,6) not null
go

-- =============================================
-- Author:		<�����,>
-- Create date: <Create 2011-1-21,,>
-- Description:	<�����Ʊ,,>
-- History:
-- 1.2011-04-21 ����־ ����������̼���ƱƱ�����Ӱٷֱȡ���������
-- =============================================
ALTER PROCEDURE [dbo].[proc_TicketOut_Insert]
(
	@TicketId char(36) ,--��Ʊ���,PlanTicketOut����	
	@TourId char(36) ,--�ź�
	@OrderId char(36),--������
	@RouteName nvarchar(255),--��·����
	@PNR nvarchar(max),--PNR
	@Total money, --�ܷ���
	@Notice nvarchar(250),--��Ʊ��֪
	@TicketOffice nvarchar(50),--��Ʊ��Ӧ��
	@TicketOfficeId int,--��Ʊ��Ӧ�̱��
	@Saler nvarchar(50),--����Ա
	@TicketNum nvarchar(50),--Ʊ��
	@PayType nvarchar(50),--֧����ʽ
	@Remark nvarchar(250),--��ע
	@TicketType tinyint,--��Ʊ���ͣ��Ŷ������Ʊ��������Ʊ�����������Ʊ��
	@State tinyint,--״̬
	@Operator nvarchar(250),--������Ա
	@OperateID int,--������Ա���
	@CompanyID int,--��˾���
	@RegisterOperatorId  int,--��Ʊ�����˱��
	@CustomerXML nvarchar(max),--�ο��޸���ϢXML:<ROOT><Info VisitorName="�ο�����" CradNumber="֤������" CradType="֤������" TravellerId="�οͱ��" /></ROOT>
	@FlightListXML nvarchar(max),--��Ʊ������ϢXML:<ROOT><Info FligthSegment="����" DepartureTime="����ʱ��" AireLine="���չ�˾" Discount="�ۿ�" TicketTime="ʱ��" /></ROOT>
	@TicketKindXML nvarchar(max),--Ʊ������ XML:<ROOT><Info Price="Ʊ���" OilFee="˰/����" PeopleCount="����" AgencyPrice="�����" TotalMoney="Ʊ��" TicketType="Ʊ��" Discount="�ٷֱ�" OtherPrice="��������" /></ROOT>
	@OutCustomerXML nvarchar(max),--��Ʊ�������οͶ�Ӧ��ϵ XML:<ROOT><Info TravellerId="�οͱ��" /></ROOT>
	@Result int output --�������
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

	IF(@errorcount=0 AND @OutCustomerXML IS NOT NULL AND LEN(@OutCustomerXML)>0)--д���Ʊ�������οͶ�Ӧ��ϵ
	BEGIN
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@OutCustomerXML
		INSERT INTO [tbl_PlanTicketOutCustomer]([TicketOutId] ,[UserId])
		SELECT @TicketId,A.TravellerId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TravellerId CHAR(36)) AS A
		SET @errorcount=@errorcount+@@ERROR
		EXECUTE sp_xml_removedocument @hdoc
	END

	IF(@errorcount=0 AND @CustomerXML IS NOT NULL AND LEN(@CustomerXML)>0)--�����ο���Ϣ
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

	IF(@errorcount=0 AND @FlightListXML IS NOT NULL AND LEN(@FlightListXML)>0)--������Ϣ
	BEGIN
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@FlightListXML
        INSERT INTO  [tbl_PlanTicketFlight]([FligthSegment],[DepartureTime],[AireLine],[Discount],[TicketId],[TicketTime])
		SELECT A.FligthSegment,A.DepartureTime,A.AireLine,A.Discount,@TicketId,A.TicketTime FROM OPENXML(@hdoc,'/ROOT/Info') 
		WITH(FligthSegment nvarchar(50),DepartureTime datetime,AireLine tinyint,Discount decimal(10,6),TicketTime varchar(100)) AS A
		SET @errorcount=@errorcount+@@ERROR
		EXECUTE sp_xml_removedocument @hdoc
	END

	IF(@errorcount=0 AND @TicketKindXML IS NOT NULL AND LEN(@TicketKindXML)>0)--Ʊ����Ϣ
	BEGIN
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@TicketKindXML
        INSERT INTO [tbl_PlanTicketKind]([TicketId],[Price],[OilFee],[PeopleCount],[AgencyPrice],[TotalMoney],[TicketType],[Discount],[OtherPrice])
		SELECT @TicketId,A.Price,A.OilFee,A.PeopleCount,A.AgencyPrice,A.TotalMoney,A.TicketType,A.Discount,A.OtherPrice FROM OPENXML(@hdoc,'/ROOT/Info') 
		WITH(Price money,OilFee money,PeopleCount int,AgencyPrice  money,TotalMoney money,TicketType tinyint,Discount DECIMAL(10,6),OtherPrice MONEY) AS A
		SET @errorcount=@errorcount+@@ERROR
		EXECUTE sp_xml_removedocument @hdoc
	END

	IF(@errorcount=0)--д���Ʊ�����б�
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
-- Author:		�����
-- Create date: 2011-03-01
-- Description:	��Ʊ�����޸ġ���Ʊ
-- History:
-- 1.2011-03-30 ����־ ����������д��ʽ
-- 2.2011-04-21 ����־ ��ƱƱ�����Ӱٷֱȡ���������
-- 3.2011-08-15 ����־ ��ɳ�Ʊ��Ʊ���Զ����崦��(�Զ����尴ϵͳ����)
-- =============================================
ALTER PROCEDURE [dbo].[proc_TicketOut_Update]
(   @TicketId char(36) ,--��Ʊ������
    @TourId char(36),
	@RouteName nvarchar(255),--��·����
    @PNR nvarchar(max),
    @Total money, --�ܷ���
	@Notice nvarchar(250),--��Ʊ��֪
	@TicketOffice nvarchar(50),--��Ʊ��Ӧ��
	@TicketOfficeId int,--��Ʊ��Ӧ�̱��
	@Saler nvarchar(50),--����Ա
	@TicketNum nvarchar(50),--Ʊ��
	@PayType nvarchar(50),--֧����ʽ
	@Remark nvarchar(250),--��ע
    @TicketType tinyint,--��Ʊ���ͣ��Ŷ������Ʊ��������Ʊ�����������Ʊ��
    @State tinyint,--״̬
	@Operator nvarchar(250),--������Ա
	@OperateID int,--������Ա���
	@CompanyID int,--��˾���
	@TicketOutTime datetime,   --��Ʊʱ��
	@CustomerXML nvarchar(max),--�ο��޸���ϢXML:<ROOT><Info VisitorName="�ο�����" CradNumber="֤������" CradType="֤������" TravellerId="�οͱ��" /></ROOT>
	@FlightListXML nvarchar(max),--��Ʊ������ϢXML:<ROOT><Info FligthSegment="����" DepartureTime="����ʱ��" AireLine="���չ�˾" Discount="�ۿ�" TicketTime="ʱ��" /></ROOT>
	@TicketKindXML nvarchar(max),--Ʊ������ XML:<ROOT><Info Price="Ʊ���" OilFee="˰/����" PeopleCount="����" AgencyPrice="�����" TotalMoney="Ʊ��" TicketType="Ʊ��" Discount="�ٷֱ�" OtherPrice="��������" /></ROOT>
	@OutCustomerXML nvarchar(max),--��Ʊ�������οͶ�Ӧ��ϵ XML:<ROOT><Info TravellerId="�οͱ��" /></ROOT>
	@Result int output --�������
	,@TicketOutRegisterId CHAR(36) OUTPUT--����ǼǱ��
)
AS
BEGIN
	DECLARE @hdoc INT
	DECLARE @errorcount INT
	SET @errorcount=0
	BEGIN TRANSACTION addTicket

	--��Ʊ������Ϣ
	UPDATE [tbl_PlanTicketOut] SET [PNR] = @PNR,[Total] = @Total,[Notice] = @Notice,[TicketOffice] = @TicketOffice,[TicketOfficeId] = @TicketOfficeId
		,[TicketNum] = @TicketNum,[PayType] = @PayType,[Remark] = @Remark,[State] = @State,[Operator] = @Operator
		,[OperateID] = @OperateID,[TotalAmount] = @Total+[AddAmount]-[ReduceAmount],[TicketOutTime] = @TicketOutTime
	WHERE id=@TicketId
	SET @errorcount=@errorcount+@@ERROR

	IF(@errorcount=0 AND @OutCustomerXML IS NOT NULL AND LEN(@OutCustomerXML)>0)--��Ʊ�������οͶ�Ӧ��ϵ����
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

	IF(@errorcount=0 AND LEN(@CustomerXML)>10)--�ο���Ϣ����
	BEGIN
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@CustomerXML
		UPDATE tbl_TourOrderCustomer SET VisitorName=A.VisitorName,CradType=A.CradType,CradNumber=A.CradNumber
		FROM tbl_TourOrderCustomer AS B INNER JOIN (SELECT VisitorName,CradType,CradNumber,TravellerId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(VisitorName varchar(250),CradNumber varchar(250),CradType int,TravellerId char(36))) AS A
		ON B.Id=A.TravellerId
		SET @errorcount=@errorcount+@@ERROR
		EXECUTE sp_xml_removedocument @hdoc
	END 

	IF(@errorcount=0 AND @FlightListXML IS NOT NULL AND LEN(@FlightListXML)>0)--������Ϣ
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

	IF(@errorcount=0 AND @TicketKindXML IS NOT NULL AND LEN(@TicketKindXML)>0)--Ʊ����Ϣ����
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

	IF(@errorcount=0)--��Ʊ�����б���Ϣ
	BEGIN
		UPDATE [tbl_PlanTicket] SET [TourId] = @TourId,[RouteName] = @RouteName,[Saler] = @Saler
			,[Operator] =@Operator,[OperatorId]=@OperateID,[RegisterTime] = GETDATE(),[State] = @State
		WHERE  RefundId=@TicketId
	END

	IF(@errorcount=0 AND @State=3 AND NOT EXISTS(SELECT 1 FROM [tbl_StatAllOut] WHERE ItemType=2 AND [ItemId]=@TicketId))--֧����ϸ
	BEGIN
		DECLARE @AreaId INT--��·����
		DECLARE @TourType TINYINT --�ƻ�����
		DECLARE @DepartmentId INT--���ű��
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

	IF(@errorcount=0 AND @State=3 AND EXISTS(SELECT 1 FROM tbl_CompanySetting WHERE Id=@CompanyId AND FieldName='IsTicketOutRegisterPayment' AND FieldValue='1'))--��ɳ�Ʊ��Ʊ���Զ����崦��(�Զ����尴ϵͳ����),�Ǽǵĸ���Ĭ���������֧��
	BEGIN
		--ItemType(֧�����������Ŷӻ��ӷ�)=0�Ŷ� ReceiveType(�Ƶ���Ŀ����)=2��Ʊ PaymentType(֧����ʽ)=2 �����ָ�
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


--http://www.tatop.com/ManageWorkflow/TaskShow.aspx?taskid=181 ��������
--���¹��ʱ�
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ǼǱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��˾���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'Year'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�·�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'Month'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����˱��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'OperatorId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'IssueTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ְλ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'ZhiWei'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'XingMing'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��λ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'GangWeiGongZi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ȫ�ڽ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'QuanQinJiang'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'JiangJin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'FanBu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'HuaBu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ͳ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'YouBu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ӱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'JiaBanFei'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ٵ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'ChiDao'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'BingJia'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�¼�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'ShiJia'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'XingZhengFaKuan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ҵ�񷣿�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'YeWuFaKuan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ƿ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'QianKuan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ӧ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'YingFaGongZi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'WuXian'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�籣' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'SheBao'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ʵ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Wage', @level2type=N'COLUMN',@level2name=N'ShiFaGongZi'
GO

-- =============================================
-- Author:		����־
-- Create date: 2011-10-19
-- Description:	�������������¹�����Ϣ
-- =============================================
CREATE PROCEDURE [dbo].[proc_Wage_Set]
	@CompanyId INT,--��˾���
	@Year INT,--���
	@Month TINYINT,--�·�
	@OperatorId INT,--����Ա���
	@WagesXML NVARCHAR(MAX),--XML:<ROOT><Info ... /></ROOT>
	@Result INT OUTPUT--������� 1:�ɹ�
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
--http://www.tatop.com/ManageWorkflow/TaskShow.aspx?taskid=181 ��������



--(20111026)bbl.txt
-- =============================================
-- Author:		���ĳ�
-- Create date: 2011-04-19
-- Description:	��Ʊ����--��Ʊͳ����ͼ
-- History:
-- 1.2011-06-14 �����Ŷӷ������� [TourOperatorId]
-- 2.2011-10-28 ���ӻ�ƱƱ��[TicketNum]
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
-- 1.2011-12-02 ����־ �����������ӵ�������δ������
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
	,(SELECT ISNULL(SUM(TotalAmount-PayAmount),0) FROM tbl_PlanLocalAgency AS A WHERE A.CompanyId=tcs.CompanyId AND A.TravelAgencyId=tcs.Id)--�ؽ�֧��
	+(SELECT ISNULL(SUM(TotalAmount-PayAmount),0) FROM tbl_PlanTicketOut AS A WHERE A.CompanyId=tcs.CompanyId AND A.TicketOfficeId=tcs.Id AND A.State=3)--��Ʊ֧��
	+(SELECT ISNULL(SUM(TotalAmount-PaidAmount),0) FROM tbl_PlanSingle AS A WHERE A.SupplierId =tcs.Id) --����֧��
	AS Amount
	,(SELECT Operator AS [Name] FROM tbl_PlanLocalAgency AS A WHERE A.CompanyId=tcs.CompanyId AND A.TravelAgencyId=tcs.Id FOR XML RAW,ROOT('root')) AS PlanerXmlDJ--�ؽӼƵ�
	,(SELECT Operator AS [Name] FROM tbl_PlanTicketOut AS A WHERE A.CompanyId=tcs.CompanyId AND A.TicketOfficeId=tcs.Id AND A.State=3 FOR XML RAW,ROOT('root')) AS PlanerXmlJP--��Ʊ�Ƶ�
	,(SELECT B.ContactName AS [Name] FROM tbl_PlanSingle AS A INNER JOIN tbl_CompanyUser AS B ON A.OperatorId=B.Id WHERE A.SupplierId=tcs.Id FOR XML RAW,ROOT('root')) AS PlanerXmlDX--����Ƶ�
FROM         tbl_CompanySupplier AS tcs
WHERE     IsDelete = '0'
GO


--���ŵؽӵؽ�����֪����ע��������̫���ַ�BUG
go
alter table tbl_PlanLocalAgency 
alter column [Notice] nvarchar(max)
alter table tbl_PlanLocalAgency 
alter column [Remark] nvarchar(max)
go

-- =============================================
-- Author:		<Author,,�����>
-- Create date: <Create Date,,>
-- Description:	<Description,���ŵؽ�,>
-- =============================================
ALTER PROCEDURE [dbo].[proc_PlanTravelAgency_Add] 
(
            @ID char(36),
            @AddAmount money,---���ӷ���
            @Commission money,---����
            @Contacter nvarchar(50),---��ϵ��
            @ContactTel nvarchar(50),--��ϵ�˵绰
            @DeliverTime datetime,---����ʱ��
            @Fee money,--�ſ�
            @LocalTravelAgency nvarchar(250),---�ؽ���
            @Notice nvarchar(max),---�ؽ�����֪
            @Operator nvarchar(50),---����Ա
            @OperatorID int,----����Ա���
            @PayType tinyint,---֧����ʽ
            @ReceiveTime datetime,---����ʱ��
            @ReduceAmount money,----���ٽ��
            @Remark nvarchar(max),--��ע
            @Settlement money,---������
            @TotalAmount money,--�ܽ��
            @TourId char(36),---�ź�
            @TravelAgencyID int,--�ؽ�����
            @PriceInfoListXML nvarchar(max),---�۸����</root><PriceInfo PriceType=��Ŀ���� ID= PeopleCount= Price=  Remark= Title=  PriceTpyeId=��Ŀ��� /></root>
            @PlanInfoListXML nvarchar(max),---�Ӵ��г�<root><TourPlanInfo Days=�ڼ��� /></root>
            @CompanyId  int,
            @Result int output ---�����Ƿ�ɹ�

         
)
AS
BEGIN
	set @Result=0
	declare @hdoc int
    begin transaction addTravel ---��ʼ����
   -----------����ؽ�����Ϣ
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
 ------������
    begin
      rollback transaction addTravel 
      set @Result=0
      return @Result 
    end

---------д��۸����
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

---------д���г̰���
 if(@PlanInfoListXML !='' and @PlanInfoListXML is not null and len(@ID)>0)
 begin
  EXECUTE SP_XML_PREPAREDOCUMENT @hdoc output,@PlanInfoListXML
  INSERT INTO [tbl_PlanLocalAgencyTourPlan]
           ([LocalTravelId]
           ,[Days])
  select @ID,Days from openxml(@hdoc,'/ROOT/TourPlanInfo')
  WITH(Days tinyint)
  execute sp_xml_removedocument @hdoc
------�����ж�  
  if(@@error>0)
  begin
   set @Result =0
   execute sp_xml_removedocument @hdoc
   rollback transaction addTravel
   return @Result
  end
 end

 ----�޴���
commit transaction addTravel
set @Result =1

return @Result

END
GO

-- =============================================
-- Author:		<Author,,�����>
-- Create date: <Create Date,2011-02-23,>
-- Description:	<Description,�޸İ��ŵؽ�,>
-- =============================================
ALTER PROCEDURE [dbo].[proc_PlanTravelAgency_Update]
(
            @ID char(36),
            @AddAmount money,---���ӷ���
            @Commission money,---����
            @Contacter nvarchar(50),---��ϵ��
            @ContactTel nvarchar(50),--��ϵ�˵绰
            @DeliverTime datetime,---����ʱ��
            @Fee money,--�ſ�
            @LocalTravelAgency nvarchar(250),---�ؽ���
            @Notice nvarchar(max),---�ؽ�����֪
            @OperateTime datetime, ---����ʱ��
            @Operator nvarchar(50),---����Ա
            @OperatorID int,----����Ա���
            @PayType tinyint,---֧����ʽ
            @ReceiveTime datetime,---����ʱ��
            @ReduceAmount money,----���ٽ��
            @Remark nvarchar(max),--��ע
            @Settlement money,---������
            @TotalAmount money,--�ܽ��
            @TourId char(36),---�ź�
            @TravelAgencyID int,--�ؽ�����
            @PriceInfoListXML nvarchar(max),---�۸����</root><PriceInfo PriceType=��Ŀ���� ID= PeopleCount= Price=  Remark= Title=  PriceTpyeId=��Ŀ��� /></root>
            @PlanInfoListXML nvarchar(max),---�Ӵ��г�<root><TourPlanInfo Days=�ڼ��� /></root>
            @Result int output ---�����Ƿ�ɹ�

         
)
AS
BEGIN
	declare @hdoc int
    begin transaction addTravel ---��ʼ����
   -----------����ؽ�����Ϣ
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
 ------������
    begin
      rollback transaction addTravel 
      set @Result=0
      return @Result 
    end

---------д��۸����
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

---------д���г̰���
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
------�����ж�  
  if(@@error>0)
  begin
   set @Result =0
   execute sp_xml_removedocument @hdoc
   rollback transaction addTravel
   return @Result
  end
 end

 ----�޴���
commit transaction addTravel
set @Result =1
return @Result

END
GO
--���ŵؽӵؽ�����֪����ע��������̫���ַ�BUG


--ɾ���Ŷ�ʱ�ſ�֧���Ǽ�ͬʱɾ��
-- =============================================
-- Author:		���ĳ�
-- Create date: 2011-04-25
-- Description:	ɾ���Ŷ���Ϣ������ɾ����
-- Error��0��������
-- Success��1ɾ���ɹ�
-- History:
-- 1.2011-12-16 ����־ �����ſ�֧���Ǽǵ�ɾ��
-- =============================================
ALTER PROCEDURE [dbo].[proc_Tour_UpdateIsDelete]
	@TourId char(36),		--�Ŷ�Id
	@ErrorValue int output   --�������
AS
BEGIN
	/*
	1�������Ŷӱ�Isdelete
	2��������
	3���������룬֧����
	4���Ƶ����ؽӡ�Ʊ��
	5��ά����·�����������տ���
	*/

	if @TourId is null or len(@TourId) <> 36
	begin
		set @ErrorValue = 0
		return
	end

	declare @errorcount int  --���������
	declare @TourCount int --�Ŷ���
	declare @CustomerCount int  --�ο���
	declare @RouteId int   --��·Id
	set @TourCount = 0
	set @CustomerCount = 0
	set @errorcount = 0

	begin tran UpdateTour
	
	--1�������Ŷӱ�Isdelete
	UPDATE [tbl_Tour] SET [IsDelete] = '1' WHERE TourId = @TourId;
	SET @errorcount = @errorcount + @@ERROR

	--������
	UPDATE [tbl_TourOrder] SET [IsDelete] = '1' WHERE TourId = @TourId;
	SET @errorcount = @errorcount + @@ERROR
	
	--�����ο���Ʊ������Ϣ
	delete from tbl_CustomerRefundFlight where CustomerId in (select ID from tbl_TourOrderCustomer where tbl_TourOrderCustomer.OrderId in (select ID from tbl_TourOrder where tbl_TourOrder.TourId = @TourId))
	SET @errorcount = @errorcount + @@ERROR

	--�����ο���Ʊ��Ϣ
	delete from tbl_CustomerRefund where CustormerId in (select ID from tbl_TourOrderCustomer where tbl_TourOrderCustomer.OrderId in (select ID from tbl_TourOrder where tbl_TourOrder.TourId = @TourId))
	SET @errorcount = @errorcount + @@ERROR

	--�ſ�������Ϣ
	delete from tbl_ReceiveRefund where tbl_ReceiveRefund.ItemType = 1 and tbl_ReceiveRefund.ItemId in (select ID from tbl_TourOrder where tbl_TourOrder.TourId = @TourId)
	SET @errorcount = @errorcount + @@ERROR

	--�ſ�֧���Ǽ�
	DELETE FROM [tbl_FinancialPayInfo] WHERE [ItemId]=@TourId
	SET @errorcount=@errorcount+@@ERROR

	--�ſ�����������Ϣ
	delete from tbl_ReceiveRefund where tbl_ReceiveRefund.ItemType = 2 and tbl_ReceiveRefund.ItemId  = @TourId
	SET @errorcount = @errorcount + @@ERROR
	delete from tbl_TourOtherCost where TourId = @TourId
	SET @errorcount = @errorcount + @@ERROR

	--�������룬֧����
	UPDATE [tbl_StatAllIncome] SET  [IsDelete] = '1' WHERE TourId = @TourId;
	SET @errorcount = @errorcount + @@ERROR
	
	UPDATE [tbl_StatAllOut] SET  [IsDelete] = '1' WHERE TourId = @TourId;
	SET @errorcount = @errorcount + @@ERROR

	--�Ƶ����ؽӡ�Ʊ��
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

	--��·Id��ֵ
	select @RouteId = RouteId from tbl_Tour where TourId = @TourId
	--��������ֵ
	select @TourCount = count(*) from tbl_Tour as a where a.RouteId = @RouteId and a.IsDelete = '0' and a.TemplateId <> ''
	--�տ�����ֵ
	select @CustomerCount = sum(PeopleNumber) from tbl_TourOrder as a where a.TourId in (select b.TourId from tbl_Tour as b where b.RouteId =@RouteId and b.IsDelete = '0') and a.IsDelete = '0' and a.OrderState not in (3,4)
	--ά����·��������������տ���
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
--ɾ���Ŷ�ʱ�ſ�֧���Ǽ�ͬʱɾ��


--2011-12-16 ����־ �޸ķ�ҳ�洢����
--�洢���̷�ҳ,��SqlDataReader��ʹ��
--history:
--1.�޸�SET @strSQL�еı����ı���Ϊ@TableName  2010-5-27  zhangzy
ALTER PROCEDURE [dbo].[T_StocPage]
(    
    @PageSize     int = 10,           -- ҳ�ߴ�
    @PageIndex    int = 1,            -- ҳ��
    @TableName      nvarchar(255),       -- ����
    @IdentityColumnName      nvarchar(255)='AAA',       -- �����ֶ��� 
    @FieldsList	  nvarchar(2000),	      --����
    @FieldSearchKey     nvarchar(2000) = '', -- ��ѯ���� (ע��: ��Ҫ�� where)
    @OrderString      nvarchar(1000),       -- ����  
    @IsGroupBy   char(1) = '0'    --�����Ƿ����
)
AS
declare @RecordCount int	      --��¼����[����]
declare @PageCount int
declare @strSQL   varchar(4000)       -- �����
declare @strTmp   nvarchar(2000)        -- ��ʱ����
declare @PageLowerBound int	--��ǰҳ���һ����¼��λ������
declare @PageUpperBound int		--��ǰҳ�����һ����¼λ������
SET @PageCount = 0
------------------------��������--------------------------
if @OrderString IS NOT NULL AND @OrderString != ''--�Ƿ���ڲ�ѯ����
	SET @OrderString = 'ORDER BY '+@OrderString
ELSE
	SET @OrderString = ''
------------------------��ѯ����--------------------------
if @FieldSearchKey IS NOT NULL AND @FieldSearchKey != ''--�Ƿ���ڲ�ѯ����
	IF @IsGroupBy IS NOT NULL AND @IsGroupBy = '1'
		SET @strTmp = ' select @RecordCount=count(*) from (SELECT COUNT(*) AS C FROM [' + @TableName + ']'+' WHERE ' + @FieldSearchKey + ') as t '
	else
		SET @strTmp = 'SELECT @RecordCount=COUNT(*) FROM [' + @TableName + ']'+' WHERE ' + @FieldSearchKey
ELSE
	SET @strTmp = 'SELECT @RecordCount=COUNT(*) FROM [' + @TableName + ']'

EXECUTE sp_executesql  @strTmp,N'@RecordCount int OUTPUT',@RecordCount OUTPUT
SET @PageCount=ceiling(@RecordCount*1.0/@PageSize)
---------------------------�����ܼ�¼������----------------------------------
if @PageIndex >@PageCount
	SET @PageIndex = @PageCount
if @PageCount=0
	SET @PageIndex = 1
if @PageIndex<=0
	SET @PageIndex = 1
--�жϼ�¼���Ƿ�����ܼ�¼��
if (@PageIndex-1)*@PageSize>@RecordCount
begin
	SET @PageIndex= @PageCount
end
if @FieldSearchKey IS NOT NULL AND @FieldSearchKey != ''--�Ƿ���ڲ�ѯ����
	SET @strTmp = N' WHERE ' + @FieldSearchKey
ELSE
	SET @strTmp = N''

--��ǰҳ���һ����¼��λ������
SET @PageLowerBound=(@PageIndex-1)* @PageSize +1
--��ǰҳ�����һ����¼��λ������
SET @PageUpperBound = @PageLowerBound + @PageSize -1
--���÷��ؼ�¼��SQL���
---------------------------ROW_NUMBER ��ҳ---------------------
--SET @strSQL = 'SELECT * FROM ( SELECT  ' + @FieldsList + ',ROW_NUMBER() OVER(' + @OrderString + ') AS RowNumber FROM [' + @TableName + '] ' + @strTmp + ' ) '+@TableName+' WHERE RowNumber BETWEEN ' + Cast(@PageLowerBound as nvarchar) + ' AND ' + Cast(@PageUpperBound as nvarchar)+' ' + @OrderString
SET @strSQL = 'SELECT t120.* FROM ( SELECT  ' + @FieldsList + ',ROW_NUMBER() OVER(' + @OrderString + ') AS RowNumber FROM [' + @TableName + '] ' + @strTmp + ' ) AS t120 WHERE RowNumber BETWEEN ' + Cast(@PageLowerBound as nvarchar) + ' AND ' + Cast(@PageUpperBound as nvarchar)+' ORDER BY RowNumber ASC'
select @RecordCount as RecordCount
PRINT @strSQL

EXECUTE(@strSQL)
GO
--2011-12-16 ����־ �޸ķ�ҳ�洢����


--2011-12-19 ����־ ��Ʊ����������������ʱ��
GO
ALTER TABLE dbo.tbl_PlanTicketOut ADD
	VerifyTime datetime NULL
GO
DECLARE @v sql_variant 
SET @v = N'�������ʱ��'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_PlanTicketOut', N'COLUMN', N'VerifyTime'
GO
GO
--2011-12-19 ����־ ��Ʊ������Ʊ�������������ʱ��
ALTER TABLE dbo.tbl_PlanTicket ADD
	VerifyTime datetime NULL
GO
DECLARE @v sql_variant 
SET @v = N'�������ʱ��(��������Ϊ�����Ʊʱ��¼������������ʱ��)'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_PlanTicket', N'COLUMN', N'VerifyTime'
GO


--����Ա�������˴�ͳ�ƣ��޸Ŀ�ʼ
--2011-12-19 ����־ �����������ֶΣ�����Ա�������˴�ͳ�ƣ�
GO
ALTER TABLE dbo.tbl_TourOrder ADD
	PerTimeSellerId int NOT NULL CONSTRAINT DF_tbl_TourOrder_PerTimeSellerId DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'����Ա�������˴�ͳ�ƣ�
�ͻ���λ�����µ���¼�ͻ���λ����������
ר�ߺ�̨�µ���¼������'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOrder', N'COLUMN', N'PerTimeSellerId'
GO
--���ݴ��� ����Ա�������˴�ͳ�ƣ��ͻ���λ�����µ���¼�ͻ���λ���������ۣ�ר�ߺ�̨�µ���¼������
UPDATE tbl_TourOrder SET PerTimeSellerId=SalerId WHERE OrderType=0
UPDATE tbl_TourOrder SET PerTimeSellerId=OperatorId WHERE OrderType IN(1,2,3)
GO
-- =============================================
-- Author:		����־
-- Create date: 2011-01-21
-- Description:	д��ɢƴ�ƻ����ŶӼƻ������������Ϣ
-- History:
-- 1.2011-04-18 ���ӵؽӱ������������ۼ����籨������������
-- 2.2011-06-08 �ŶӼƻ��������ӷ�Ӷ
-- 3.2011-07-11 �ŶӼƻ����������ͼ۸���Ϣ��ɾ��SelfUnitPriceAmount
-- 4.2011-12-20 ���Ӷ�����Ա�������˴�ͳ�ƣ��ֶεĴ���
-- =============================================
ALTER PROCEDURE [dbo].[proc_Tour_InsertTourInfo]
	@TourId CHAR(36),--�Ŷӱ��
	@TourCode NVARCHAR(50),--�ź�
	@TourType TINYINT,--�Ŷ�����
	@Status TINYINT,--�Ŷ�״̬
	@TourDays INT,--�Ŷ�����
	@LDate DATETIME,--��������
	@AreaId INT,--��·������
	@RouteName NVARCHAR(255),--��·����
	@PlanPeopleNumber INT,--�ƻ�����
	@TicketStatus TINYINT,--��Ʊ״̬
	@ReleaseType CHAR(1),--��������
	@OperatorId INT,--�����˱��
	@CreateTime DATETIME,--����ʱ��
	@CompanyId INT,--��˾���
	@LTraffic NVARCHAR(255),--������ͨ ȥ�̺���/ʱ��
	@RTraffic NVARCHAR(255),--���̽�ͨ �س̺���/ʱ��
	@Gather NVARCHAR(255),--���Ϸ�ʽ
	@RouteId INT=0,--���õ���·���
	@LocalAgencys NVARCHAR(MAX)=NULL,--�Ŷӵؽ�����Ϣ XML:<ROOT><Info AgencyId="INT�ؽ�����" Name="�ؽ�������" LicenseNo="���֤��" Telephone="��ϵ�绰" ContacterName="��ϵ��" /></ROOT>
	@Attachs NVARCHAR(MAX)=NULL,--�ŶӸ��� XML:<ROOT><Info FilePath="" Name="" /></ROOT>
	@Plans NVARCHAR(MAX)=NULL,--��׼�����Ŷ��г���Ϣ XML:<ROOT><Info Interval="����" Vehicle="��ͨ����" Hotel="ס��" Dinner="�ò�" Plan="�г�����" FilePath="����·��" /></ROOT>
	@QuickPrivate NVARCHAR(MAX)=NULL,--���ٷ����Ŷ�ר����Ϣ XML:<ROOT><Info Plan="�Ŷ��г�" Service="�����׼" Remark="��ע" /></ROOT>
	@Service NVARCHAR(MAX)=NULL,--������Ŀ��Ϣ(ɢƴ�ƻ�������Ŀ���ŶӼƻ��۸���ɡ��������) XML:<ROOT><Info Type="INT��Ŀ����" Service="�Ӵ���׼����������Ҫ��" LocalPrice="MONEY�ؽӱ���" SelfPrice="MONEY���籨��" Remark="��ע" LocalPeopleNumber="�ؽӱ�������" LocalUnitPrice="�ؽӱ��۵���" SelfPeopleNumber="���籨������" SelfUnitPrice="���籨�۵���" /></ROOT>
	@NormalPrivate NVARCHAR(MAX)=NULL,--��׼����ר����Ϣ(������Ŀ��) XML:<ROOT><Info Key="��" Value="ֵ" /></ROOT>
	@Childrens NVARCHAR(MAX)=NULL,--ɢƴ�ƻ�������Ϣ XML:<ROOT><Info TourId="�Ŷӱ��" LDate="��������" TourCode="�ź�" /></ROOT>
	@TourPrice NVARCHAR(MAX)=NULL,--ɢƴ�ƻ�������Ϣ XML:<ROOT><Info AdultPrice="MONEY���˼�" ChildrenPrice="MONEY��ͯ��" StandardId="���۱�׼���" LevelId="�ͻ��ȼ����" /></ROOT>
	@ChildrenCreateRule NVARCHAR(MAX)=NULL,--ɢƴ�ƻ����Ž��Ź��� XML:<ROOT><Info Type="��������" SDate="��ʼ����" EDate="��������" Cycle="����" /></ROOT>
	@SinglePrivate NVARCHAR(MAX)=NULL,--�������ר����Ϣ XML:<ROOT><Info OrderId="�������" SellerId="����Ա���" SellerName="����Ա����" TotalAmount="�ϼ�������" GrossProfit="�Ŷ�ë��" BuyerCId="�ͻ���λ���" BuyerCName="�ͻ���λ����" ContactName="��ϵ��" ContactTelephone="��ϵ�绰" Mobile="��ϵ�ֻ�" TotalOutAmount="�ϼ�֧�����" /></ROOT>
	@PlansSingle NVARCHAR(MAX)=NULL,--�������Ӧ�̰�����Ϣ XML:<ROOT><Info PlanId="���ű��" ServiceType="��Ŀ����" SupplierId="��Ӧ�̱��" SupplierName="��Ӧ������" Arrange="���尲��" Amount="�������" Remark="��ע" /></ROOT>
	@TeamPrivate NVARCHAR(MAX)=NULL,--�ŶӼƻ�ר����Ϣ XML:<ROOT><Info OrderId="�������" SellerId="����Ա���" SellerName="����Ա����" TotalAmount="�ϼƽ��" BuyerCId="�ͻ���λ���" BuyerCName="�ͻ���λ����" ContactName="��ϵ��" ContactTelephone="��ϵ�绰" Mobile="��ϵ�ֻ�" QuoteId="���۱��" /></ROOT>
	@CustomerDisplayType TINYINT=0,--�ο���Ϣ��������
	@Customers NVARCHAR(MAX)=NULL,--�����ο���Ϣ XML:<ROOT><Info TravelerId="�οͱ��" TravelerName="�ο�����" IdType="֤������" IdNo="֤����" Gender="�Ա�" TravelerType="����(����/��ͯ)" ContactTel="��ϵ�绰" TF_XiangMu="�ط���Ŀ" TF_NeiRong="�ط�����" TF_ZengJian="1:���� 0:����" TF_FeiYong="�ط�����" /></ROOT>	
	@CustomerFilePath NVARCHAR(255)=NULL,--�����ο͸���
	@Result INT OUTPUT,--������� ��ֵ���ɹ� ��ֵ��0��ʧ��
	@CoordinatorId INT=0,--�Ƶ�Ա���,
	@GatheringTime NVARCHAR(MAX)=NULL,--����ʱ��
	@GatheringPlace NVARCHAR(MAX)=NULL,--���ϵص�
	@GatheringSign NVARCHAR(MAX)=NULL,--���ϱ�־
	@SentPeoples NVARCHAR(MAX)=NULL,--��������Ϣ���� XML:<ROOT><Info UID="�����˱��" /></ROOT>
	@TourCityId INT=0,--���۳��б��
	@TourTeamUnit NVARCHAR(MAX)=NULL--�ŶӼƻ����������ͼ۸���ϢXML:<ROOT><Info NumberCr="������" NumberEt="��ͯ��" NumberQp="ȫ����" UnitAmountCr="���˵���" UnitAmountEt="��ͯ����" UnitAmountQp="ȫ�㵥��" /></ROOT>
AS
BEGIN
	DECLARE @hdoc INT
	DECLARE @errorcount INT
	
	SET @Result=0
	SET @errorcount=0

	DECLARE @CompanyName NVARCHAR(255)--��˾����
	DECLARE @OperatorName NVARCHAR(255)--����������
	DECLARE @OperatorTelephone NVARCHAR(255)--�����˵绰
	DECLARE @OperatorMobile NVARCHAR(255)--�������ֻ�
	DECLARE @OperatorFax NVARCHAR(255)--�����˴���
	DECLARE @DepartmentId INT--����Ա���ű��	
	SELECT @CompanyName=[CompanyName] FROM [tbl_CompanyInfo] WHERE Id=@CompanyId
	SELECT @OperatorName=[ContactName],@OperatorTelephone=[ContactTel],@OperatorMobile=[ContactMobile],@OperatorFax=[ContactFax] FROM [tbl_COmpanyUser] WHERE Id=@OperatorId
	SELECT @DepartmentId=[DepartId] FROM [tbl_CompanyUser] WHERE Id=@OperatorId	

	BEGIN TRAN
	IF(@TourType=0)--ɢƴ�ƻ�
	BEGIN
		--�������Ŵ˴�д����Ŷ�(ģ���ż�����)
		DECLARE @tmptbl TABLE(TourId CHAR(36))
		--ģ������Ϣ
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
		IF(@errorcount=0)--������Ϣ
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
		IF(@errorcount=0)--������Ϣ
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@TourPrice
			INSERT INTO [tbl_TourPrice]([TourId],[Standard],[LevelId],[AdultPrice],[ChildPrice])
			SELECT B.TourId,A.StandardId,A.LevelId,A.AdultPrice,A.ChildrenPrice FROM OPENXML(@hdoc,'/ROOT/Info') 
			WITH(AdultPrice MONEY,ChildrenPrice MONEY,StandardId INT,LevelId INT) AS A ,@tmptbl AS B WHERE A.LevelId>0 AND A.StandardId>0
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0)--�ŶӲ���Ա�Ƶ�Ա��Ϣ
		BEGIN
			/*INSERT INTO [tbl_TourOperator]([TourId],[OperatorId]) SELECT B.TourId,A.UserId FROM (SELECT C.UserId FROM tbl_UserArea AS C WHERE C.AreaId=@AreaId AND EXISTS(SELECT 1 FROM tbl_CompanyUser AS D WHERE D.Id=C.UserId AND D.UserType=2)) AS A,@tmptbl AS B */
			INSERT INTO [tbl_TourOperator]([TourId],[OperatorId]) SELECT B.TourId,@CoordinatorId FROM @tmptbl AS B
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0 AND @ChildrenCreateRule IS NOT NULL)--���Ź���
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@ChildrenCreateRule
			INSERT INTO [tbl_TourCreateRule]([TourId],[RuleType],[SDate],[EDate],[Cycle])
			SELECT B.TourId,A.[Type],A.SDate,A.EDate,A.Cycle FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH([Type] TINYINT,SDate DATETIME,EDate DATETIME,Cycle NVARCHAR(255)) AS A,@tmptbl AS B
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @LocalAgencys IS NOT NULL)--�ؽ�����Ϣ
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@LocalAgencys
			INSERT INTO [tbl_TourLocalAgency]([TourId],[AgencyId],[Name],[LicenseNo],[Telephone],[ContacterName])
			SELECT B.TourId,A.AgencyId,A.[Name],A.LicenseNo,A.Telephone,A.ContacterName FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(AgencyId INT,[Name] NVARCHAR(255),LicenseNo NVARCHAR(255),Telephone NVARCHAR(255),ContacterName NVARCHAR(255)) AS A,@tmptbl AS B
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @Attachs IS NOT NULL)--������Ϣ
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@Attachs
			INSERT INTO [tbl_TourAttach]([TourId],[FilePath],[Name])
			SELECT B.TourId,A.FilePath,A.[Name] FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(FilePath NVARCHAR(255),[Name] NVARCHAR(255)) AS A,@tmptbl AS B
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @ReleaseType='0')--��׼�����г���Ϣ
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@Plans
			INSERT INTO [tbl_TourPlan]([TourId],[Interval],[Vehicle],[Hotel],[Dinner],[Plan],[FilePath])
			SELECT B.TourId,A.Interval,A.Vehicle,A.Hotel,A.Dinner,A.[Plan],A.FilePath FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(Interval NVARCHAR(255),Vehicle NVARCHAR(255),Hotel NVARCHAR(255),Dinner NVARCHAR(255),[Plan] NVARCHAR(MAX),FilePath NVARCHAR(255)) AS A,@tmptbl AS B
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @ReleaseType='1')--���ٷ����г���Ϣ��
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@QuickPrivate
			INSERT INTO [tbl_TourQuickPlan]([TourId],[Plan],[Service],[Remark])
			SELECT B.TourId,A.[Plan],A.[Service],A.Remark FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH([Plan] NVARCHAR(MAX),[Service] NVARCHAR(MAX),Remark NVARCHAR(MAX)) AS A,@tmptbl AS B
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @ReleaseType='0' AND @Service IS NOT NULL)--��׼����������Ŀ
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@Service
			INSERT INTO [tbl_TourService]([TourId],[Key],[Value])
			SELECT B.TourId,dbo.fn_GetTourServiceKey(A.[Type]),A.[Service] FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH([Type] TINYINT,[Service] NVARCHAR(MAX)) AS A,@tmptbl AS B
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc			
		END		
		IF(@errorcount=0 AND @ReleaseType='0' AND @NormalPrivate IS NOT NULL)--��׼����������Ŀ��
		BEGIN			
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@NormalPrivate			
			INSERT INTO [tbl_TourService]([TourId],[Key],[Value])
			SELECT B.TourId,A.[Key],A.[Value] FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH([Key] NVARCHAR(50),[Value] NVARCHAR(MAX)) AS A,@tmptbl AS B			
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END		
		IF(@errorcount=0 AND @SentPeoples IS NOT NULL)--��������Ϣ
		BEGIN
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@SentPeoples
			INSERT INTO [tbl_TourSentTask]([TourId],[OperatorId])
			SELECT B.TourId,A.[UID] FROM OPENXML(@hdoc,'/ROOT/Info') WITH([UID] INT) AS A,@tmptbl AS B
			SET @errorcount=@errorcount+@@ERROR
			EXEC sp_xml_removedocument @hdoc
		END	
		IF(@errorcount=0 AND @TourCityId IS NOT NULL AND @TourCityId>0)--���۳���
		BEGIN
			INSERT INTO [tbl_TourCity]([TourId],[CityId]) SELECT A.TourId,@TourCityId FROM @tmptbl AS A
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0)--�Ƿ����
		BEGIN
			UPDATE [tbl_Tour] SET IsLeave='1' WHERE TemplateId>'' AND LeaveDate<=GETDATE() AND TourId IN(SELECT TourId FROM @tmptbl)
			SET @errorcount=@errorcount+@@ERROR			
			IF(@errorcount=0)--����Ŷ�״̬Ϊ�г�;��
			BEGIN
				UPDATE tbl_Tour SET Status=2 WHERE TourType IN(0,1) AND [Status] IN(0,1,3) AND DATEDIFF(DAY,GETDATE(),[LeaveDate])<=0 AND DATEDIFF(DAY,GETDATE(),[RDate])>0 AND TourId IN(SELECT TourId FROM @tmptbl)
				SET @errorcount=@errorcount+@@ERROR
			END			
			IF(@errorcount=0)--����Ŷ�״̬Ϊ���ű���
			BEGIN
				UPDATE tbl_Tour SET Status=3 WHERE TourType IN(0,1) AND [Status] IN(0,1,2) AND DATEDIFF(DAY,GETDATE(),[RDate])<0 AND TourId IN(SELECT TourId FROM @tmptbl)
				SET @errorcount=@errorcount+@@ERROR
			END
		END
	END

	
	IF(@TourType=1)--�ŶӼƻ�
	BEGIN
		DECLARE @SellerId INT--����Ա���
		DECLARE @SellerName NVARCHAR(255)--����Ա����
		DECLARE @CommissionType	TINYINT--��Ӷ����
		DECLARE @CommissionPrice MONEY--��Ӷ�����ۣ�
		DECLARE @IncomeAmount MONEY--����������
		--�Ŷ���Ϣ		
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@TeamPrivate
		SELECT @SellerId=SaleId,@SellerName=Saler,@CommissionType=[CommissionType],@CommissionPrice=[CommissionCount] FROM [tbl_Customer] WHERE Id=(SELECT A.BuyerCId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(BuyerCId INT) AS A)
		SELECT @IncomeAmount=TotalAmount FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TotalAmount MONEY) AS A
		IF(@CommissionType<>1) SET @CommissionType=2

		IF(@CommissionType=1)--�ַ�ʱ�Ӷ�������м�ȥ��Ӷ���
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
		IF(@errorcount=0)--�ŶӲ���Ա�Ƶ�Ա��Ϣ
		BEGIN
			/*INSERT INTO [tbl_TourOperator]([TourId],[OperatorId]) SELECT @TourId,A.UserId FROM (SELECT C.UserId FROM tbl_UserArea AS C WHERE C.AreaId=@AreaId AND EXISTS(SELECT 1 FROM tbl_CompanyUser AS D WHERE D.Id=C.UserId AND D.UserType=2)) AS A*/
			INSERT INTO [tbl_TourOperator]([TourId],[OperatorId])VALUES(@TourId,@CoordinatorId)
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0 AND @LocalAgencys IS NOT NULL)--�ؽ�����Ϣ
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@LocalAgencys
			INSERT INTO [tbl_TourLocalAgency]([TourId],[AgencyId],[Name],[LicenseNo],[Telephone],[ContacterName])
			SELECT @TourId,A.AgencyId,A.[Name],A.LicenseNo,A.Telephone,A.ContacterName FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(AgencyId INT,[Name] NVARCHAR(255),LicenseNo NVARCHAR(255),Telephone NVARCHAR(255),ContacterName NVARCHAR(255)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @Attachs IS NOT NULL)--������Ϣ
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@Attachs
			INSERT INTO [tbl_TourAttach]([TourId],[FilePath],[Name])
			SELECT @TourId,A.FilePath,A.[Name] FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(FilePath NVARCHAR(255),[Name] NVARCHAR(255)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @ReleaseType='0')--��׼�����г���Ϣ
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@Plans
			INSERT INTO [tbl_TourPlan]([TourId],[Interval],[Vehicle],[Hotel],[Dinner],[Plan],[FilePath])
			SELECT @TourId,A.Interval,A.Vehicle,A.Hotel,A.Dinner,A.[Plan],A.FilePath FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(Interval NVARCHAR(255),Vehicle NVARCHAR(255),Hotel NVARCHAR(255),Dinner NVARCHAR(255),[Plan] NVARCHAR(MAX),FilePath NVARCHAR(255)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @ReleaseType='1')--���ٷ����г���Ϣ��
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@QuickPrivate
			INSERT INTO [tbl_TourQuickPlan]([TourId],[Plan],[Service],[Remark])
			SELECT @TourId,A.[Plan],A.[Service],A.Remark FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH([Plan] NVARCHAR(MAX),[Service] NVARCHAR(MAX),Remark NVARCHAR(MAX)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @NormalPrivate IS NOT NULL)--��׼����������Ŀ��
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@NormalPrivate
			INSERT INTO [tbl_TourService]([TourId],[Key],[Value])
			SELECT @TourId,A.[Key],A.[Value] FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH([Key] NVARCHAR(50),[Value] NVARCHAR(MAX)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		IF(@errorcount=0 AND @Service IS NOT NULL)--�۸����
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@Service
			INSERT INTO [tbl_TourTeamPrice]([TourId],[ItemType],[Standard],[LocalPrice],[MyPrice],[Requirement],[Remark],[LocalPeopleNumber],[LocalUnitPrice],[SelfPeopleNumber],[SelfUnitPrice])
			SELECT @TourId,A.[Type],A.[Service],A.LocalPrice,A.SelfPrice,NULL,NULL,[LocalPeopleNumber],[LocalUnitPrice],[SelfPeopleNumber],[SelfUnitPrice] FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH([Type] TINYINT,[Service] NVARCHAR(MAX),LocalPrice MONEY,SelfPrice MONEY,[LocalPeopleNumber] INT,[LocalUnitPrice] MONEY,[SelfPeopleNumber] INT,[SelfUnitPrice] MONEY) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		--���ɶ���	
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
		IF(@errorcount=0)--д����
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
		IF(@errorcount=0 AND @SentPeoples IS NOT NULL)--��������Ϣ
		BEGIN
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@SentPeoples
			INSERT INTO [tbl_TourSentTask]([TourId],[OperatorId])
			SELECT @TourId,A.[UID] FROM OPENXML(@hdoc,'/ROOT/Info') WITH([UID] INT) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXEC sp_xml_removedocument @hdoc
		END	
		IF(@errorcount=0 AND @TourCityId IS NOT NULL AND @TourCityId>0)--���۳���
		BEGIN
			INSERT INTO [tbl_TourCity]([TourId],[CityId]) VALUES(@TourId,@TourCityId)
			SET @errorcount=@errorcount+@@ERROR
		END
		if @errorcount = 0 and @TourTeamUnit is not null  --�ŶӼƻ����������ͼ۸���Ϣ
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
		IF(@errorcount=0)--�Ƿ����
		BEGIN
			UPDATE [tbl_Tour] SET IsLeave='1' WHERE TourId=@TourId AND LeaveDate<=GETDATE()
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)--����Ŷ�״̬Ϊ�г�;��
			BEGIN
				UPDATE tbl_Tour SET Status=2 WHERE TourType IN(0,1) AND [Status] IN(0,1,3) AND DATEDIFF(DAY,GETDATE(),[LeaveDate])<=0 AND DATEDIFF(DAY,GETDATE(),[RDate])>0 AND TourId=@TourId
				SET @errorcount=@errorcount+@@ERROR
			END			
			IF(@errorcount=0)--����Ŷ�״̬Ϊ���ű���
			BEGIN
				UPDATE tbl_Tour SET Status=3 WHERE TourType IN(0,1) AND [Status] IN(0,1,2) AND DATEDIFF(DAY,GETDATE(),[RDate])<0 AND TourId=@TourId
				SET @errorcount=@errorcount+@@ERROR
			END
		END
	END

	IF(@TourType=2)--�������
	BEGIN
		--�Ŷ���Ϣ
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
		IF(@errorcount=0 AND @Service IS NOT NULL)--�۸����
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@Service
			INSERT INTO [tbl_TourTeamPrice]([TourId],[ItemType],[Standard],[LocalPrice],[MyPrice],[Requirement],[Remark])
			SELECT @TourId,A.[Type],NULL,A.LocalPrice,A.SelfPrice,A.[Service],A.Remark FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH([Type] TINYINT,LocalPrice MONEY,SelfPrice MONEY,[Service] NVARCHAR(MAX),Remark NVARCHAR(MAX)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
		--��Ӧ�̰���
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
		IF(@errorcount=0 AND @PlansSingle IS NOT NULL)--д֧��
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
		--���ɶ���
		DECLARE @OrderId CHAR(36) --�������
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
		IF(@errorcount=0)--д����
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
		IF(@errorcount=0 AND @Customers IS NOT NULL)--�����ο���Ϣ
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
		IF(@errorcount=0 AND @Customers IS NOT NULL)--�ο��ط���Ϣ
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
-- Description:	��������
-- History:
-- 1.����������ϵ����Ϣ����Ӷ��Ϣ ����־ 2011-06-08
-- 2.����������֤����ȥ������������
-- 3.2011-12-20 ����־ ���Ӷ�����Ա�������˴�ͳ�ƣ��ֶεĴ���
-- =============================================
ALTER PROCEDURE [dbo].[proc_TourOrder_Insert]	
	@OrderID CHAR(36),                --�������
	@TourId CHAR(36),	             --�Ŷӱ��
	@AreaId INT,			             --��·������
	@RouteId INT,		             --��·���
	@RouteName NVARCHAR(500),         --��·����
	@TourNo NVARCHAR(100),	         --�ź�
	@TourClassId TINYINT,	         --�Ŷ�����(ɢ�ͣ��Ŷ�)
	@LeaveDate DATETIME,              --��������
	@TourDays INT,                    --�Ŷ�����
	@LeaveTraffic NVARCHAR(250),      --������ͨ
	@ReturnTraffic NVARCHAR(250),     --���̽�ͨ
	@OrderType TINYINT,               --������Դ  0:�����µ���1������Ԥ��
	@OrderState TINYINT,              --����״̬
	@BuyCompanyID INT,	             --����˾���
	@BuyCompanyName NVARCHAR(250),    --����˾����
	@ContactName NVARCHAR(250),	     --��ϵ��
	@ContactTel NVARCHAR(250),	     --��ϵ�˵绰
	@ContactMobile NVARCHAR(100),     --��ϵ���ֻ�
	@ContactFax NVARCHAR(100),        --����  
	@OperatorName NVARCHAR(100),      --�µ�������
	@OperatorID INT,                  --�µ��˱��
	@PriceStandId INT,                --���۵ȼ����
	@PersonalPrice MONEY,             --���˼�
	@ChildPrice MONEY,                --��ͯ��
	@MarketPrice MONEY,               --������
	@AdultNumber INT,                 --������
	@ChildNumber INT,                 --��ͯ��
	@MarketNumber INT,                --��������
	@PeopleNumber INT,                --������
	@OtherPrice MONEY,                --��������
	@SaveSeatDate DATETIME,           --��λ����
	@OperatorContent NVARCHAR(1000),  --����˵��
	@SpecialContent NVARCHAR(1000),   --��������
	@SumPrice MONEY,                  --�����н��
	@SellCompanyName NVARCHAR(100),   --ר�߹�˾����
	@SellCompanyId INT,               --ר�߹�˾���
	@LastDate DATETIME,               --����޸�����
	@LastOperatorID INT,              --����޸���
	@ViewOperatorId INT,              --�����˻򷢲��ƻ���(�����µ����ŶӶ���ʱ��ŷ����ƻ��� ר�߱���ʱ��ŵ�ǰ��¼��)
	@CustomerDisplayType TINYINT,     --�ο���Ϣ�������ͣ�������ʽ�����뷽ʽ��
	@CustomerFilePath NVARCHAR(500),  --�ο���Ϣ����
	@CustomerLevId INT,               --�ͻ��ȼ����
	@IsTourOrderEdit INT,             --1���������Ŷ˵Ķ����޸ģ�0������ר�߶˵Ķ����޸�
	@Result INT OUTPUT,				 --�ش�����:0:ʧ�ܣ�1:�ɹ�;2�����ŶӵĶ���������+��ǰ�������������ŶӼƻ������ܺͣ�3���ÿͻ���Ƿ���������Ƿ���
	@TourCustomerXML NVARCHAR(MAX),   --�ο�XML��ϸ��Ϣ:<ROOT><TourOrderCustomer ID="" CompanyId="" OrderId="" VisitorName="" CradType="" CradNumber="" Sex="" VisitorType"" ContactTel="" ProjectName="" ServiceDetail="" IsAdd="" Fee=""/></ROOT> 
	@AmountPlus NVARCHAR(MAX)=NULL--����������Ӽ��ٷ�����Ϣ XML:<ROOT><Info AddAmount="���ӷ���" ReduceAmount="���ٷ���" Remark="��ע" /></ROOT>
	,@BuyerContactId INT--������ϵ�˱��
	,@BuyerContactName NVARCHAR(50)--������ϵ������
	,@CommissionType TINYINT--��Ӷ����
	,@CommissionPrice MONEY--��Ӷ���
AS
	DECLARE @ErrorCount INT              --�����ۼƲ���
	DECLARE @hdoc INT                    --XMLʹ�ò���
	DECLARE @TmpResult INT               --��ʱ�ݴ淵����
	DECLARE @OrderNO VARCHAR(100)        --������
	DECLARE @SalerName NVARCHAR(100)         --����������
	DECLARE @SalerId INT                     --������ID
	DECLARE @MaxDebts MONEY              --���Ƿ����
	DECLARE @PeopleNumberCount INT       --�ŶӶ���������:�ѳɽ�����+����λ��������
	DECLARE @PlanPeopleNumber INT        --�Ŷ�����
	DECLARE @VirtualPeopleNumber INT     --����ʵ������
	DECLARE @RealVirtualPeopleNumber INT     --�����޸�����ʵ�յ���ʱ����
	DECLARE @ShiShu INT --ʵ�ʶ�������
	SET @ErrorCount=0
	SET @Result=1
BEGIN
	DECLARE @PerTimeSellerId INT--����Ա�������˴�ͳ�ƣ�
	SET @PerTimeSellerId=@OperatorID
	--�ж϶��������Ƿ����ʣ������
	SELECT @VirtualPeopleNumber=VirtualPeopleNumber,@PlanPeopleNumber=PlanPeopleNumber FROM tbl_Tour WHERE TourId=@TourId  AND IsDelete=0
	SELECT @ShiShu = ISNULL(SUM(PeopleNumber - LeaguePepoleNum),0) FROM tbl_TourOrder WHERE IsDelete=0 AND TourId=@TourId AND OrderState IN(1,2,5)
	
	--�������ж������������ŶӼƻ��������������޸�
	if @PlanPeopleNumber - @ShiShu - @PeopleNumber < 0
	begin
		SET @Result=2
	end
	
	--��λ�ͳɽ�ʱ����֤�Ƿ�ﵽ�ÿͻ������Ƿ���� 
	IF(@OrderState=2 OR @OrderState=5)
	BEGIN 
		SELECT @MaxDebts=MaxDebts FROM tbl_Customer WHERE [ID]=@BuyCompanyID	 
		--�ÿͻ������Ƿ����-���������<�ѳɽ�+����λ�Ķ����� ����С��-��������˽�� ���ܺ�
		IF((@MaxDebts-@SumPrice)<(SELECT SUM(FinanceSum-HasCheckMoney) FROM tbl_TourOrder WHERE BuyCompanyID=@BuyCompanyID  AND IsDelete=0 AND TourId=@TourId AND (OrderState=5 OR OrderState=2)))
		BEGIN
			SET @Result=3
		END
		ELSE--����ͳ�Ʒ�������������ϸ��
		BEGIN			
			EXEC [proc_StatAllIncome_AboutDelete] @OrderID,0
		END
	END
	ELSE--������ʱ������ͳ�Ʒ�������������ϸ��
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
		--���ɶ�����
		SET @OrderNO=dbo.fn_TourOrder_CreateOrderNo()
		--������������
		SELECT @SalerId=SaleId,@SalerName=Saler FROM tbl_Customer WHERE ID=@BuyCompanyID  AND IsDelete=0
		IF(@OrderType=0)
		BEGIN
			SET @PerTimeSellerId=@SalerId
		END
		--���붩����Ϣ
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
		IF(LEN(@TourCustomerXML)>0 AND @ErrorCount=0)--�����ο���Ϣ
		BEGIN				
			EXECUTE  dbo.proc_TourOrderCustomer_Insert @TourCustomerXML,@OrderID,@Result OUTPUT	
			SET @ErrorCount = @ErrorCount + @@ERROR							
		END
		IF(@ErrorCount=0 AND @AmountPlus IS NOT NULL AND LEN(@AmountPlus)>0)--����������Ӽ��ٷ�����Ϣ
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@AmountPlus
			INSERT INTO [tbl_TourOrderPlus]([OrderId],[AddAmount],[ReduceAmount],[Remark])
			SELECT @OrderId,A.AddAmount,A.ReduceAmount,A.Remark FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(AddAmount MONEY,ReduceAmount MONEY,Remark NVARCHAR(MAX)) AS A
			EXECUTE sp_xml_removedocument @hdoc
		END
		--ά���Ŷ�ʵ����=��������ʵ������+����������				
		UPDATE tbl_Tour SET VirtualPeopleNumber=@VirtualPeopleNumber+@PeopleNumber WHERE TourId=@TourId  AND IsDelete=0
		SET @ErrorCount = @ErrorCount + @@ERROR	
	END
	IF(@ErrorCount>0)
	BEGIN
		SET @Result=0
		ROLLBACK TRANSACTION TourOrder_Insert
	END
	COMMIT TRANSACTION TourOrder_Insert
	--�����Ƿ��տ�����
	UPDATE [tbl_Tour] SET [Status]=1  WHERE [Status]=0  AND IsDelete='0' AND PlanPeopleNumber=(SELECT ISNULL(SUM(PeopleNumber-LeaguePepoleNum),0) FROM tbl_TourOrder WHERE TourId=@TourId  AND IsDelete='0' AND OrderState NOT IN(3,4)) and TourId=@TourId 				
	RETURN  @Result
END
GO
-- =============================================
-- Author:		���ĳ�
-- Create date: 2010-01-21
-- Description:	�˴�ͳ�ƻ�����ͼ
-- ���ĳ� 2011-06-20���ӳ���������
-- 2011-12-20 ����־ �˴�ͳ������Ա�����˴�ͳ������Ա��
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
--����Ա�������˴�ͳ�ƣ��޸Ľ���
--���Ͻű��Ѹ����������� 2011-12-20

--�����ο���������˳��BUG BEGIN ����־ 2012-01-12
--�����ο���Ϣ������������
ALTER TABLE dbo.tbl_TourOrderCustomer ADD
	IdentityId int NOT NULL IDENTITY (1, 1)
GO
DECLARE @v sql_variant 
SET @v = N'�������'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOrderCustomer', N'COLUMN', N'IdentityId'
GO

-- =============================================
-- Author:		luofx
-- Create date: 2011-01-23
-- Description:	�ŶӼƻ�-�޸�(����)�ο���Ϣ   
-- History:
-- 1.����־ 2012-01-12 ��ʱ���ֶ�[Vid]����ɱ�ʶ��
-- =============================================
ALTER PROCEDURE [dbo].[proc_TourOrderCustomer_Update]
	@TourCustomerXML NVARCHAR(MAX),  --���˿�XML��ϸ��Ϣ
	@OrderId CHAR(36),	   --�������
	@Result INT OUTPUT     --�ش�����
AS
DECLARE @ErrorCount INT   --�����ۼƲ���
DECLARE @hdoc INT         --XMLʹ�ò���
DECLARE @i INT
DECLARE @MAXID INT				   --�ݴ�����vid
DECLARE @ID CHAR(36)			   --�οͱ��
DECLARE @CompanyId INT             --��˾���
DECLARE @VisitorName NVARCHAR(250) --�ο�����
DECLARE @CradType INT			   --֤������
DECLARE @CradNumber NVARCHAR(250)  --֤����
DECLARE @Sex INT				   --�Ա�
DECLARE @VisitorType INT		   --�ο�����
DECLARE @ContactTel NVARCHAR(250)  --�绰����
--�ط�
DECLARE	@ProjectName NVARCHAR(1000)--��Ŀ����
DECLARE	@ServiceDetail NVARCHAR(1000)--��������
DECLARE	@IsAdd TINYINT --��/��
DECLARE	@Fee MONEY --����
DECLARE @IsDelete INT
SET @ErrorCount=0
SET @Result=0
SET @i=1
BEGIN
	CREATE TABLE #TmpTourCustomer_Update(
	   [Vid] INT NOT NULL IDENTITY (1, 1),
	   [ID] CHAR(36),				--�οͱ��
	   [CompanyId] INT,             --��˾���
	   [OrderId] CHAR(36),          --�������
	   [VisitorName] NVARCHAR(250), --�ο�����
	   [CradType] INT,				--֤������
	   [CradNumber] NVARCHAR(250),	--֤����
	   [Sex] INT,					--�Ա�
	   [VisitorType] INT,			--�ο�����
	   [ContactTel] NVARCHAR(250),	--�绰����
	   [ProjectName] NVARCHAR(1000),--��Ŀ����
	   [ServiceDetail] NVARCHAR(1000),--��������
	   [IsAdd] TINYINT, --��/��
	   [IsDelete] INT,
	   [Fee] MONEY --����
	)
	BEGIN TRANSACTION TourCustomer_Update	
		--�����ݴ����Ϣ
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
				--��������ֵ
				SELECT @ID=[id],@CompanyId=CompanyId,@OrderId=OrderId,@VisitorName=VisitorName,@CradType=CradType,@CradNumber=CradNumber,
					@Sex=Sex,@VisitorType=VisitorType,@ContactTel=ContactTel,@ProjectName=ProjectName,
					@ServiceDetail=ServiceDetail,@IsAdd=IsAdd,@Fee=Fee,@IsDelete=[IsDelete] FROM #TmpTourCustomer_Update WHERE [Vid]=@i									
				print @IsDelete
				IF(@IsDelete=1)--������Ҫɾ�����ο�ʱ
					BEGIN	
						IF(NOT EXISTS(SELECT CustormerId FROM [tbl_CustomerRefund] WHERE CustormerId=@ID))	
							BEGIN			    
								DELETE FROM [tbl_CustomerSpecialService] WHERE CustormerId=@ID
								DELETE FROM [tbl_TourOrderCustomer] WHERE [id]=@ID
							END
					END
				ELSE
					BEGIN
						--�ο�ά��
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
						--�ط�ά��
						
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
-- Description:	�ŶӼƻ�-�����ο���Ϣ
-- History:
-- 1.����־ 2012-01-12 ��ʱ���ֶ�[Vid]����ɱ�ʶ��
-- =============================================
ALTER PROCEDURE [dbo].[proc_TourOrderCustomer_Insert]
	@TourCustomerXML NVARCHAR(MAX),  --�ο�XML��ϸ��Ϣ:<ROOT><TourOrderCustomer ID="" CompanyId="" OrderId="" VisitorName="" CradType="" CradNumber="" Sex="" VisitorType"" ContactTel="" ProjectName="" ServiceDetail="" IsAdd="" Fee=""/></ROOT>
	@OrderId CHAR(36),	             --�������
	@Result INT OUTPUT               --�ش�����
AS
DECLARE @ErrorCount INT            --�����ۼƲ���
DECLARE @hdoc INT                  --XMLʹ�ò���
DECLARE @MAXID INT				   --�ݴ�����vid
DECLARE @i INT
DECLARE @ID CHAR(36)			   --�οͱ��(����ʱ������ֵ����)
DECLARE @CompanyId INT             --��˾���
DECLARE @VisitorName NVARCHAR(250) --�ο�����
DECLARE @CradType INT			   --֤������
DECLARE @CradNumber NVARCHAR(250)  --֤����
DECLARE @Sex INT				   --�Ա�
DECLARE @VisitorType INT		   --�ο�����
DECLARE @ContactTel NVARCHAR(250)  --�绰����
--�ط�
DECLARE	@ProjectName NVARCHAR(1000)  --��Ŀ����
DECLARE	@ServiceDetail NVARCHAR(1000)--��������
DECLARE	@IsAdd TINYINT               --��/��
DECLARE	@Fee MONEY                   --����
SET @ErrorCount=0
SET @Result=0
SET @i=1
BEGIN
	--���������ο��ݴ��
	CREATE TABLE #TmpTourCustomer_Insert(
	   [Vid] INT NOT NULL IDENTITY (1, 1),                   --��ʱ����
	   [ID] CHAR(36),				--�οͱ��
	   [CompanyId] INT,             --��˾���
	   [OrderId] CHAR(36),          --�������
	   [VisitorName] NVARCHAR(250), --�ο�����
	   [CradType] INT,				--֤������
	   [CradNumber] NVARCHAR(250),	--֤����
	   [Sex] INT,					--�Ա�
	   [VisitorType] INT,			--�ο�����
	   [ContactTel] NVARCHAR(250),	--�绰����
	   [ProjectName] NVARCHAR(1000),--��Ŀ����
	   [ServiceDetail] NVARCHAR(1000),--��������
	   [IsAdd] TINYINT, --��/��
	   [Fee] MONEY --����
	)
	BEGIN TRANSACTION TourCustomer_Insert	
		--���ο�XML���ݲ��뵽�����ο��ݴ��	
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
		--��ȡ�����ʱչ���������ֵ
		SELECT @MAXID=MAX(vid) FROM #TmpTourCustomer_Insert	
		WHILE(@i<=@MAXID AND @ErrorCount=0)
			BEGIN
				--��������ֵ
				SELECT @ID=[id],@CompanyId=CompanyId,@OrderId=OrderId,@VisitorName=VisitorName,@CradType=CradType,@CradNumber=CradNumber,
					@Sex=Sex,@VisitorType=VisitorType,@ContactTel=ContactTel,@ProjectName=ProjectName,
					@ServiceDetail=ServiceDetail,@IsAdd=IsAdd,@Fee=Fee FROM #TmpTourCustomer_Insert WHERE [Vid]=@i									
				--�����ο����ݵ��οͱ���
				INSERT INTO [tbl_TourOrderCustomer](id,[CompanyId],[OrderId],[VisitorName],
					[CradType],[CradNumber],[Sex],[VisitorType],[ContactTel])
					VALUES(@ID,@CompanyId,@OrderId,@VisitorName,
						@CradType,@CradNumber,@Sex,@VisitorType,@ContactTel)
				SET @ErrorCount = @ErrorCount + @@ERROR
				print @ID
				--�����ο��ط�����
				IF(LEN(@ProjectName)>0)
					BEGIN
						INSERT INTO [tbl_CustomerSpecialService](CustormerId,ProjectName,ServiceDetail,IsAdd,Fee)
							VALUES(@ID,@ProjectName,@ServiceDetail,@IsAdd,@Fee)										
					END
				SET @i = @i + 1
				SET @ErrorCount = @ErrorCount + @@ERROR
			END
	    --�ж��Ƿ���Ҫ�ع�			
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
--�����ο���������˳��BUG END 2012-01-12 �Ѹ�����������

--����ͳ�ƶ���������ȥ�������� 2012-01-13 ����־
-- =============================================
-- Author:		���ĳ�
-- Create date: 2011-04-19
-- Description:	�ͻ���ϵ����-����ͳ����ͼ
-- History:
-- 1.2012-01-13 ����־ ����������ȥ��������
-- =============================================
ALTER VIEW [dbo].[View_SalesStatistics]
AS
select ID,TourId,TourClassId,LeaveDate,SellCompanyId,AreaId,BuyCompanyId,BuyCompanyName,SalerId,SalerName,FinanceSum,(PeopleNumber-LeaguePepoleNum) AS PeopleNumber,OrderState,OperatorID
,(select ProvinceName + '  ' + CityName from tbl_Customer where tbl_Customer.ID = tbl_TourOrder.BuyCompanyID) as ProvinceAndCity  --��������������
,(case when tbl_TourOrder.OrderType = 0 then tbl_TourOrder.OperatorName else '' end) as OperatorName  --����Ա����
,(select top 1 DepartmentId from tbl_CustomerContactInfo where tbl_CustomerContactInfo.UserId = tbl_TourOrder.OperatorID) as DepartName  --����Ա����
,(case when TourClassId = 2 then '�������' else RouteName end) as RouteName   --��·����
,(case when TourClassId = 2 then (select ServiceType from tbl_PlanSingle where tbl_PlanSingle.TourId = tbl_TourOrder.TourId for xml raw,root('root')) else (select AreaName from tbl_Area where tbl_Area.Id = tbl_TourOrder.AreaId) end) as AreaName   --��·��������
,(select OperatorId,(select ContactName from tbl_CompanyUser where tbl_CompanyUser.ID = tbl_TourOperator.OperatorId) as OperatorName from tbl_TourOperator where tbl_TourOperator.TourId = tbl_TourOrder.TourId for xml raw,root('root')) as Logistics  --�Ƶ�Ա
from tbl_TourOrder
where tbl_TourOrder.IsDelete = '0' and tbl_TourOrder.OrderState not in (3,4)
GO
--����ͳ�ƶ���������ȥ�������� 2012-01-13 ����־


--���۷���������ͳ������ͳ�Ʋ�һ�µ��� 2012-01-30 ����־

-- =============================================
-- Author:		���ĳ�
-- Create date: 2011-04-19
-- Description:	�ͻ���ϵ����-����ͳ����ͼ
-- History:
-- 1.2012-01-13 ����־ ����������ȥ��������
-- 2.2012-01-30 ����־ �����˴�ͳ������Ա��ż������С��Է�����Ա��ż�������
-- =============================================
ALTER VIEW [dbo].[View_SalesStatistics]
AS
select ID,TourId,TourClassId,LeaveDate,SellCompanyId,AreaId,BuyCompanyId,BuyCompanyName,SalerId,SalerName,FinanceSum,(PeopleNumber-LeaguePepoleNum) AS PeopleNumber,OrderState,OperatorID
,(select ProvinceName + '  ' + CityName from tbl_Customer where tbl_Customer.ID = tbl_TourOrder.BuyCompanyID) as ProvinceAndCity  --��������������
,(case when tbl_TourOrder.OrderType = 0 then tbl_TourOrder.OperatorName else '' end) as OperatorName  --����Ա����
,(select top 1 DepartmentId from tbl_CustomerContactInfo where tbl_CustomerContactInfo.UserId = tbl_TourOrder.OperatorID) as DepartName  --����Ա����
,(case when TourClassId = 2 then '�������' else RouteName end) as RouteName   --��·����
,(case when TourClassId = 2 then (select ServiceType from tbl_PlanSingle where tbl_PlanSingle.TourId = tbl_TourOrder.TourId for xml raw,root('root')) else (select AreaName from tbl_Area where tbl_Area.Id = tbl_TourOrder.AreaId) end) as AreaName   --��·��������
,(select OperatorId,(select ContactName from tbl_CompanyUser where tbl_CompanyUser.ID = tbl_TourOperator.OperatorId) as OperatorName from tbl_TourOperator where tbl_TourOperator.TourId = tbl_TourOrder.TourId for xml raw,root('root')) as Logistics  --�Ƶ�Ա
,PerTimeSellerId--�˴�ͳ������Ա���
,(SELECT ContactName,DepartName FROM tbl_CompanyUser WHERE Id=tbl_TourOrder.PerTimeSellerId FOR XML RAW,ROOT('root')) AS PerTimeSeller--�˴�ͳ������Ա����
,BuyerContactId--�Է�����Ա���
,BuyerContactName--�Է�����Ա����
from tbl_TourOrder
where tbl_TourOrder.IsDelete = '0' and tbl_TourOrder.OrderState not in (3,4)
GO

--���۷���������ͳ������ͳ�Ʋ�һ�µ��� 2012-01-30 ����־  �Ѹ��� 2012-01-30

--�޸Ķ��������޸���������Ϣ 2012-02-02 �ܺ���
-- =============================================
-- Author:		luofx
-- Create date: 2011-01-24
-- Description:	�޸Ķ���
-- History:
-- 1.����������ϵ����Ϣ����Ӷ��Ϣ ����־ 2011-06-08
-- 2.�����޸��ŶӼƻ�����ͬ���޸��ŶӼƻ���Ϣ������������֤����ȥ������������
-- 3.�޸Ŀͻ���λ(BuyCompanyName,BuyCompanyID,SalerId,SalerName,CommissionType)  �ܺ��� 2012-02-02
-- =============================================
ALTER PROCEDURE [dbo].[proc_TourOrder_Update]	
	@OrderID CHAR(36),                --�������
	@TourId CHAR(36),	             --�Ŷӱ��
	@OrderState TINYINT,              --����״̬
	@BuyCompanyID INT,	             --����˾���
	@BuyCompanyName nvarchar(250),	 --����˾����
	@SalerId int,					--��������Ա���
	@SalerName nvarchar(100),		--��������Ա����
	@PriceStandId INT,                --���۵ȼ����
	@PersonalPrice MONEY,             --���˼�
	@ChildPrice MONEY,                --��ͯ��
	@MarketPrice MONEY,               --������
	@AdultNumber INT,                 --������
	@ChildNumber INT,                 --��ͯ��
	@MarketNumber INT,                --��������
	@PeopleNumber INT,                --������
	@OtherPrice Decimal(10,4),        --��������
	@SaveSeatDate DATETIME,           --��λ����
	@OperatorContent NVARCHAR(1000),  --����˵��
	@SpecialContent NVARCHAR(1000),   --��������
	@SumPrice MONEY,          --�����н��
	@LastDate DATETIME,               --����޸�����
	@LastOperatorID INT,              --����޸���
	@CustomerDisplayType TINYINT,     --�ο���Ϣ�������ͣ�������ʽ�����뷽ʽ��
	@CustomerFilePath NVARCHAR(500),  --�ο���Ϣ����
	@CustomerLevId INT,               --�ͻ��ȼ����
	@ContactName NVARCHAR(250),	     --��ϵ��
	@ContactTel NVARCHAR(250),	     --��ϵ�˵绰
	@ContactMobile NVARCHAR(100),     --��ϵ���ֻ�
	@ContactFax NVARCHAR(100),        --����  
	@IsTourOrderEdit INT,             --1���������Ŷ˵Ķ����޸ģ�0������ר�߶˵Ķ����޸�
	@TourCustomerXML NVARCHAR(MAX),   --�ο�XML��ϸ��Ϣ:<ROOT><TourOrderCustomer ID="" CompanyId="" OrderId="" VisitorName="" CradType="" CradNumber="" Sex="" VisitorType"" ContactTel="" ProjectName="" ServiceDetail="" IsAdd="" Fee=""/></ROOT> 
	@Result INT OUTPUT,			 --�ش�����:0:ʧ�ܣ�1:�ɹ�;2�����ŶӵĶ���������+��ǰ�������������ŶӼƻ������ܺͣ�3���ÿͻ���Ƿ���������Ƿ���
	@AmountPlus NVARCHAR(MAX)=NULL--����������Ӽ��ٷ�����Ϣ XML:<ROOT><Info AddAmount="���ӷ���" ReduceAmount="���ٷ���" Remark="��ע" /></ROOT>
	,@BuyerContactId INT--������ϵ�˱��
	,@BuyerContactName NVARCHAR(50)--������ϵ������
	,@CommissionPrice MONEY--��Ӷ���
	,@CommissionType TINYINT--��Ӷ����
	,@TourTeamUnit nvarchar(max)=NULL--�ŶӼƻ����������ͼ۸���ϢXML:<ROOT><Info NumberCr="������" NumberEt="��ͯ��" NumberQp="ȫ����" UnitAmountCr="���˵���" UnitAmountEt="��ͯ����" UnitAmountQp="ȫ�㵥��" /></ROOT>
AS
	DECLARE @ErrorCount INT   --�����ۼƲ���
	DECLARE @hdoc INT         --XMLʹ�ò���
	DECLARE @TmpResult INT    --��ʱ�ݴ淵����
	DECLARE @OrderNO VARCHAR(100) --������
	DECLARE @MaxDebts MONEY       --���Ƿ����
	DECLARE @PeopleNumberCount INT--�ŶӶ���������:�ѳɽ�����+����λ��������(����δ����)
	DECLARE @PlanPeopleNumber INT --�Ŷ�����
	DECLARE @OldPeopleNumber INT  --����ԭ������
	DECLARE @VirtualPeopleNumber INT  --����ʵ��
	DECLARE @RealVirtualPeopleNumber INT     --�����޸�����ʵ�յ���ʱ����
	DECLARE @FinanceSum MONEY --����С��
	DECLARE @ShiShu INT --ʵ�ʶ�������
	declare @TourClassId tinyint   --�Ŷ�����
	DECLARE @SBuyCompanyID INT--֮ǰ�����������繫˾���
	SET @ErrorCount=0
	SET @Result=1
BEGIN	
	--��ȡ֮ǰ�����������繫˾���
	SELECT @SBuyCompanyID=BuyCompanyID FROM [tbl_TourOrder] WHERE [ID]=@OrderID
	--�ж϶��������Ƿ����ʣ������
    SELECT @OldPeopleNumber = ISNULL(PeopleNumber,0),@TourId=TourId,@TourClassId = TourClassId FROM tbl_TourOrder WHERE ID=@OrderID AND IsDelete=0
	--δ����������+����λ��������+�ѳɽ��������� - ����������
	SELECT @PeopleNumberCount = ISNULL(SUM(PeopleNumber - LeaguePepoleNum),0) FROM tbl_TourOrder WHERE id <> @OrderID AND TourId = @TourId AND (OrderState=1 OR OrderState=2 OR OrderState=5)  AND IsDelete = '0'	
	SELECT @VirtualPeopleNumber = ISNULL(VirtualPeopleNumber,0),@PlanPeopleNumber = ISNULL(PlanPeopleNumber,0) FROM tbl_Tour WHERE TourId = @TourId  AND IsDelete=0
	SELECT @ShiShu = ISNULL(SUM(PeopleNumber - LeaguePepoleNum),0) FROM tbl_TourOrder WHERE IsDelete=0 AND TourId=@TourId AND (OrderState=1 OR OrderState=2 OR OrderState=5)

	--�������ж������������ŶӼƻ��������������޸�
	if @PlanPeopleNumber < @PeopleNumberCount + @PeopleNumber
	begin
		SET @Result=2
	end
	
	--δ����,��λ�ͳɽ�ʱ����֤�Ƿ�ﵽ�ÿͻ������Ƿ���� 
	IF(@OrderState=1 OR @OrderState=2 OR @OrderState=5)
	BEGIN 			
		SELECT @MaxDebts=MaxDebts FROM tbl_Customer WHERE [ID]=@BuyCompanyID	 
		--�ÿͻ������Ƿ����-���������<�ѳɽ�+����λ�Ķ����� ����С��-��������˽�� ���ܺ�(����������)
		IF((@MaxDebts-@SumPrice)<(SELECT SUM(FinanceSum-HasCheckMoney) FROM tbl_TourOrder WHERE id<>@OrderID  AND IsDelete=0 AND BuyCompanyID=@BuyCompanyID AND TourId=@TourId AND (OrderState=5 OR OrderState=2)))
		BEGIN
			SET @Result=3
		END
		ELSE--����ͳ�Ʒ�������������ϸ��
		BEGIN			
			EXEC [proc_StatAllIncome_AboutDelete] @OrderID,0
		END
	END
	ELSE--����ͳ�Ʒ�������������ϸ��
	BEGIN
		EXEC [proc_StatAllIncome_AboutDelete] @OrderID,1			
	END
	BEGIN TRANSACTION TourOrder_Update			
	IF(@Result=1)
	BEGIN
		--DECLARE @CommissionType TINYINT --��Ӷ����
		DECLARE @CommissionIsPaid CHAR(1)--��Ӷ����Ƿ�֧��
		
		IF((@BuyerContactName IS NULL OR LEN(@BuyerContactName)<1) AND @BuyerContactId>0)
		BEGIN
			SELECT @BuyerContactName=[Name] FROM [tbl_CustomerContactInfo] WHERE [Id]=@BuyerContactId
		END
		--���¼������С��:�µĶ������+���ӷ���+���ٷ���@CommissionType=[CommissionType],
		SELECT @FinanceSum=@SumPrice+FinanceAddExpense-FinanceRedExpense,@CommissionIsPaid=[CommissionStatus] FROM [tbl_TourOrder] a WHERE a.[ID]=@OrderID				
		--�޸Ķ�����Ϣ
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
		--ά���Ŷ�ʵ����=��������ʵ������+����������
		SET @RealVirtualPeopleNumber = @VirtualPeopleNumber + @PeopleNumber - @OldPeopleNumber
		IF(@VirtualPeopleNumber+@PeopleNumber-@OldPeopleNumber)<(@PeopleNumberCount+@PeopleNumber)
		BEGIN
			SET @RealVirtualPeopleNumber=@PeopleNumberCount+@PeopleNumber
		END
		--��λ��ȷ�ϳɽ�ʱ��ά���Ŷ�����ʵ����
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
			--�ο�����ά��
			EXECUTE  dbo.proc_TourOrderCustomer_Update @TourCustomerXML,@OrderID,@Result OUTPUT	
			IF(@TmpResult<>1)
			BEGIN
				SET @ErrorCount = @ErrorCount + 1	
			END
		END
		IF(@ErrorCount=0 AND @AmountPlus IS NOT NULL AND LEN(@AmountPlus)>0)--����������Ӽ��ٷ�����Ϣ
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

		IF(@CommissionType=2 AND @CommissionIsPaid='1')--��Ӷ����Ϊ���ҷ�Ӷ�����֧���޸�����֧����Ϣ
		BEGIN
			DECLARE @OhterOutAmount MONEY--����֧�����
			DECLARE @CostId CHAR(36)--�ӷ�֧�����
			SELECT @CostId=[Id] FROM [tbl_TourOtherCost] WHERE [ItemType]=2 AND [ItemId]=@OrderId
			SET @OhterOutAmount=(@AdultNumber+@ChildNumber)*@CommissionPrice
			--�����ӷ�֧��
			UPDATE [tbl_TourOtherCost] SET [Proceed]=@OhterOutAmount,SumCost=@OhterOutAmount+[IncreaseCost]-[ReduceCost] WHERE [Id]=@CostId
			--����֧����ϸ
			UPDATE [tbl_StatAllOut] SET [Amount]=@OhterOutAmount,[TotalAmount]=@OhterOutAmount+[AddAmount]-[ReduceAmount]
			WHERE [ItemId]=@CostId AND [ItemType]=3
		END

		--�ŶӼƻ�����ʱ���޸Ķ���ͬ���޸��ŶӼƻ�
		if @TourClassId = 1
		begin
			if @TourTeamUnit is not null and len(@TourTeamUnit) > 1
			begin
				DELETE FROM [tbl_TourTeamUnit] WHERE [TourId] = @TourId --ɾ��ԭ��������
				
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
	--���������罻�״���
	--�����ǰѡ����������֮ǰ�����������粻һ�£���Ҫ��֮ǰ�����������罻�״�����1
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
	--�����Ƿ��տ�����
	UPDATE [tbl_Tour] SET [Status]=1  WHERE [Status]=0 AND PlanPeopleNumber=(SELECT ISNULL(SUM(PeopleNumber-LeaguePepoleNum),0) FROM tbl_TourOrder WHERE TourId=(SELECT TourId FROM tbl_TourOrder WHERE [id]=@OrderID) AND IsDelete='0' AND OrderState NOT IN(3,4))	and TourId=@TourId 						
	RETURN  @Result
END
GO
--�޸Ķ��������޸���������Ϣ �Ѹ���  2012-02-02


--������ʱ������ϵͳҵ�����ݲ�ѯ�ŶӴ洢���� ��ʼ
-- =============================================
-- Author:		<�ܺ���>
-- Create date: <2012-02-03>
-- Description:	<ɾ���ƻ�ҵ������>
-- =============================================
CREATE PROCEDURE [dbo].[proc_Tour_ClearData]
	@CompanyID int,--��ϵͳ��˾���
	@LDateStart datetime,--�������ڿ�ʼ
	@LDateEnd datetime--�������ڽ���
AS
BEGIN
	declare @DelTour table(TourId char(36))--Ҫɾ�����ź�
	declare @DelTemplateTuan table(TourId char(36)) --Ҫɾ����ģ����
	declare @DelZiTuan table(TourId char(36),TemplateId char(36))--Ҫɾ��������
	--Ҫɾ��������
	insert into @DelZiTuan select TourId,TemplateId from tbl_tour where CompanyId=@CompanyID and
	DATEDIFF(DAY,@LDateStart,LeaveDate)>=0 and DATEDIFF(DAY,@LDateEnd,LeaveDate)<=0 and TemplateId<>'' and IsDelete=0
	
	insert into @DelTemplateTuan select TemplateId from @DelZiTuan group by TemplateId
	--��Ҫɾ�������ŷ���ɾ������
	insert into @DelTour select TourId from @DelZiTuan
--	declare @ZiTourID char(36) --Ҫɾ���������ź�
--	DECLARE ZiTuanCursor CURSOR	FOR SELECT TourId FROM @DelZiTuan
--	OPEN ZiTuanCursor
--	FETCH NEXT FROM ZiTuanCursor INTO @ZiTourID
--	WHILE @@FETCH_STATUS =0
--	BEGIN
--		--exec proc_Tour_UpdateIsDelete @ZiTourID,@Result OUTPUT
--		insert into @DelTour values(@ZiTourID)
--		FETCH NEXT FROM  ZiTuanCursor INTO @ZiTourID
--	END	
--	--�ر��α�
--	CLOSE ZiTuanCursor
--	--�ͷ���Դ
--	DEALLOCATE ZiTuanCursor

	declare @TemplateTourID char(36)
	DECLARE TemplateTuanCursor CURSOR FOR SELECT TourId FROM @DelTemplateTuan
	OPEN TemplateTuanCursor
	FETCH NEXT FROM TemplateTuanCursor INTO @TemplateTourID
	WHILE @@FETCH_STATUS =0
	BEGIN
		--�жϸ�ģ������û�����ţ����û�о�ɾ����ģ����
		if(not exists(select 1 from tbl_tour where TourId not in(select TourID from @DelZiTuan 
			where TemplateId=@TemplateTourID) and TemplateId=@TemplateTourID and CompanyId=@CompanyID and IsDelete=0))
		begin
			--���ڵ���ҵ�����ŶӼƻ���ģ���ű�����Լ��������ظ�ɾ��
			if(not exists(select 1 from @DelTour where TourId=@TemplateTourID))
			begin
				insert into @DelTour values(@TemplateTourID)
			end
			--exec proc_Tour_UpdateIsDelete @TemplateTourID,@Result OUTPUT
		end
		FETCH NEXT FROM TemplateTuanCursor INTO @TemplateTourID
	END	
	--�ر��α�
	CLOSE TemplateTuanCursor
	--�ͷ���Դ
	DEALLOCATE TemplateTuanCursor
	select * from @DelTour
END
GO
--������ʱ������ϵͳҵ�����ݲ�ѯ�ŶӴ洢���� ���� 2012-02-09 ������������

--�޸������Ǽ�BUG S

-- =============================================
-- Author:		����־
-- Create date: 2011-07-04
-- Description:	�����ǼǸ���
-- @ExpenseType:(0:�������Ӧ�̰��ţ�1:�ؽӣ�2:Ʊ��255:����)
-- History:
-- 1.2012-03-13 ����־ ���»�Ʊ����Ǽǵ�����(δ��Ʊ�Ĳ��Ǽ�)
-- 2.2012-03-26 ����־ �����Ǽ�ʱ��ԶԹ�Ӧ�̵Ĳ�ѯ��������Ӧ�����͡���Ӧ�����ƣ� ȥ��@ExpenseType��ͨ��@GysType������
-- =============================================
ALTER PROCEDURE [dbo].[proc_BatchRegisterExpense]
	@PaymentTime DATETIME--֧��ʱ��
	,@PaymentType TINYINT--֧������
	,@PayerId INT--�����˱��
	,@Payer NVARCHAR(50)--����������
	,@Remark NVARCHAR(255)--��ע
	--,@ExpenseType TINYINT=255--֧�����ͣ�Ϊ255ʱ��������֧��
	,@OperatorId INT--����Ա���
	,@CompanyId INT--��˾���
	,@TourIdsXML NVARCHAR(MAX)--�ƻ���ż���XML��<ROOT><Info TourId="�ƻ����" /></ROOT>
	,@Result INT OUTPUT--�������
	,@GysName NVARCHAR(255)=NULL
	,@GysType TINYINT=255--��Ӧ�����ͣ�Ϊ255ʱ�������й�Ӧ������
AS
BEGIN
	SET @Result=0

	DECLARE @hdoc INT
	DECLARE @tmptbl_tour TABLE(TourId CHAR(36))
	DECLARE @tmptbl_zhichu TABLE(ZhiChuType TINYINT,ZhiChuLeiBie NVARCHAR(10),TourId CHAR(36),PlanId CHAR(36),Amount MONEY,GysId INT,GysName NVARCHAR(255))
	
	EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@TourIdsXML
	INSERT INTO @tmptbl_tour(TourId) SELECT A.TourId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TourId CHAR(36)) AS A
	EXECUTE sp_xml_removedocument @hdoc

	IF(@GysType=255)--����֧��
	BEGIN
		IF(@GysName IS NOT NULL AND LEN(@GysName)>0)
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuType,ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT 0,'����',A.TourId,B.PlanId,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.PlanId AND [ReceiveType]=0)),B.SupplierId,B.SupplierName FROM @tmptbl_tour AS A INNER JOIN tbl_PlanSingle AS B
			ON A.TourId=B.TourId AND B.SupplierName LIKE'%'+@GysName+'%'
		END
		ELSE
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuType,ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT 0,'����',A.TourId,B.PlanId,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.PlanId AND [ReceiveType]=0)),B.SupplierId,B.SupplierName FROM @tmptbl_tour AS A INNER JOIN tbl_PlanSingle AS B
			ON A.TourId=B.TourId
		END
	END

	IF(@GysType=1 OR @GysType=255)--�ؽ�֧��
	BEGIN
		IF(@GysName IS NOT NULL AND LEN(@GysName)>0)
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuType,ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT 1,'�ؽ�',A.TourId,B.Id,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=1)),B.TravelAgencyID,B.LocalTravelAgency FROM @tmptbl_tour AS A INNER JOIN tbl_PlanLocalAgency AS B
			ON A.TourId=B.TourId AND B.LocalTravelAgency LIKE'%'+@GysName+'%'
		END
		ELSE
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuType,ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT 1,'�ؽ�',A.TourId,B.Id,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=1)),B.TravelAgencyID,B.LocalTravelAgency FROM @tmptbl_tour AS A INNER JOIN tbl_PlanLocalAgency AS B
			ON A.TourId=B.TourId
		END
	END

	IF(@GysType=2 OR @GysType=255)--��Ʊ֧��
	BEGIN
		IF(@GysName IS NOT NULL AND LEN(@GysName)>0)
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuType,ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT 2,'��Ʊ',A.TourId,B.Id,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=2)),B.TicketOfficeId,B.TicketOffice FROM @tmptbl_tour AS A INNER JOIN tbl_PlanTicketOut AS B
			ON A.TourId=B.TourId AND B.State=3 AND B.TicketOffice LIKE'%'+@GysName+'%'
		END
		ELSE
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuType,ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT 2,'��Ʊ',A.TourId,B.Id,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=2)),B.TicketOfficeId,B.TicketOffice FROM @tmptbl_tour AS A INNER JOIN tbl_PlanTicketOut AS B
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
--�޸������Ǽ�BUG E


