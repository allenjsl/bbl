--(20110620)�ͱ���.doc start
GO
--��Ӧ�̻�����Ϣ���������֤���ֶ� ����־ 2011-06-30
ALTER TABLE dbo.tbl_CompanySupplier ADD
	LicenseKey nvarchar(50) NULL
GO
DECLARE @v sql_variant 
SET @v = N'���֤��'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_CompanySupplier', N'COLUMN', N'LicenseKey'
GO

-- =============================================
-- Author:		���ĳ�
-- Create date: 2011-03-28
-- Description:	������Ӧ����Ϣ�������ڵؽ�����Ϣ�޸ģ�
-- Error��0��������
-- Error��-1���������з�������
-- Success��1�����ɹ�
-- =============================================
ALTER PROCEDURE  [dbo].[proc_CompanySupplier_Add]
(
	@ProvinceId int = 0		--ʡ�ݱ��
    ,@ProvinceName nvarchar(255) = ''		--ʡ������
    ,@CityId int = 0		--���б��
    ,@CityName nvarchar(255) = ''		--��������
    ,@UnitName nvarchar(255) = ''		--��λ����
    ,@SupplierType tinyint = 0		--�ؽӡ���Ʊ
    ,@UnitAddress nvarchar(500) = ''		--��ַ
    ,@Commission money = 0		--��Ӷ
    ,@AgreementFile nvarchar(255) = ''		--����Э��
    ,@TradeNum int = 0		--���״���
    ,@UnitPolicy nvarchar(max) = ''		--����
    ,@Remark nvarchar(max) = ''		--��ע
    ,@CompanyId int = 0		--��˾���
    ,@OperatorId int = 0		--����Ա���
	,@SupplierContactXML nvarchar(max) = null  --��ϵ����ϢXML
	,@ErrorValue  int = 0 output   --�������
	,@LicenseKey NVARCHAR(50)--���֤��
)
as
BEGIN
	declare @ErrorCount int    --���������
	declare @IsUserExist int   --�û����Ƿ����
	declare @NewContactId int  --����ϵ��Id
	declare @NewUserId int   --���û�Id
	declare @SupplierId int  --�¹�Ӧ��Id
	DECLARE @hdoc int   --����XML����
	--�����α����
	declare @ContactName nvarchar(50),		--��ϵ������
			@JobTitle nvarchar(255),		--ְ��
			@ContactFax nvarchar(100),		--����
			@ContactTel nvarchar(100),		--��ϵ�绰
			@ContactMobile nvarchar(100),		--��ϵ���ֻ�
			@QQ nvarchar(100),		--QQ
			@Email nvarchar(100),		--Email
			@UserId int	,	--�û����
			@UserName nvarchar(100), --�û���
			@Password varchar(50),  --��������
			@MD5Password varchar(50)   --MD5����

	set @IsUserExist = 0
	set @ErrorCount = 0
	set @NewContactId = 0
	set @NewUserId = 0
	set @SupplierId = 0

	if @CompanyId is null or @CompanyId <= 0 or @UnitName is null or len(@UnitName) < 1
	begin
		set @ErrorValue = 0
		return
	end

	begin tran AddSupplier

	INSERT INTO [tbl_CompanySupplier]
           ([ProvinceId]
           ,[ProvinceName]
           ,[CityId]
           ,[CityName]
           ,[UnitName]
           ,[SupplierType]
           ,[UnitAddress]
           ,[Commission]
           ,[AgreementFile]
           ,[TradeNum]
           ,[UnitPolicy]
           ,[Remark]
           ,[CompanyId]
           ,[OperatorId]
           ,[IssueTime]
           ,[IsDelete]
		   ,[LicenseKey])
     VALUES
           (@ProvinceId
           ,@ProvinceName
           ,@CityId
           ,@CityName
           ,@UnitName
           ,@SupplierType
           ,@UnitAddress
           ,@Commission
           ,@AgreementFile
           ,@TradeNum
           ,@UnitPolicy
           ,@Remark
           ,@CompanyId
           ,@OperatorId
           ,getdate()
           ,'0'
		   ,@LicenseKey)

	select @SupplierId = @@identity
	--��֤����
	SET @ErrorCount = @ErrorCount + @@ERROR

	if @SupplierContactXML is not null and len(@SupplierContactXML) > 1
	begin
		set @SupplierContactXML = replace(replace(@SupplierContactXML,char(13),''),char(10),'')
		EXEC sp_xml_preparedocument @hdoc OUTPUT, @SupplierContactXML

		DECLARE cur_SupplierContact CURSOR FOR
		SELECT ContactName,JobTitle,ContactFax,ContactTel,ContactMobile,QQ,Email,UserName,[Password],MD5Password from openxml(@hdoc,'/ROOT/CustomerInfo')
		with(ContactName nvarchar(500),JobTitle nvarchar(255),ContactFax nvarchar(100),ContactTel nvarchar(100),ContactMobile nvarchar(100),QQ nvarchar(100),Email nvarchar(100),UserName nvarchar(100),[Password] varchar(50),MD5Password varchar(50))
		open cur_SupplierContact
		while 1 = 1
		begin
			fetch next from cur_SupplierContact into @ContactName,@JobTitle,@ContactFax,@ContactTel,@ContactMobile,@QQ,@Email,@UserName,@Password,@MD5Password
			if @@fetch_status<>0
				break
			set @NewContactId = 0;
			INSERT INTO [tbl_SupplierContact] (
					[CompanyId]
					,[SupplierId]
					,[SupplierType]
					,[ContactName]
					,[JobTitle]
					,[ContactFax]
					,[ContactTel]
					,[ContactMobile]
					,[QQ]
					,[Email]
					) VALUES (
					@CompanyId
					,@SupplierId
					,@SupplierType
					,@ContactName
					,@JobTitle
					,@ContactFax
					,@ContactTel
					,@ContactMobile
					,@QQ
					,@Email)
			select @NewContactId = @@identity
			--��֤����
			SET @ErrorCount = @ErrorCount + @@ERROR
			--����˻���Ϣ
			if @UserName is not null and len(@UserName) > 0 and @Password is not null and len(@Password) > 0
			begin
				set @IsUserExist = 0 ---�ж��û��Ƿ����
				select @IsUserExist = count(id) from tbl_CompanyUser where UserName=@UserName and CompanyId=@CompanyId
				if @IsUserExist <= 0
				begin
					INSERT INTO  tbl_CompanyUser 
						(UserName
						,[Password]
						,MD5Password
						,roleid
						,SuperviseDepartId
						,TourCompanyId
						,CompanyId
						,SuperviseDepartName
						,contactname
						,contactsex
						,contactmobile
						,contactemail
						,ContactFax
						,qq
						,jobname
						,departname
						,usertype
						,ContactTel)
						values 
						(@UserName,@Password,@MD5Password,0,0,@SupplierId,@CompanyId,'',@ContactName,'1',@ContactMobile,@Email,@ContactFax,@QQ,@JobTitle,'',3,@ContactTel)
					select @NewUserId = @@identity
					--��֤����
					SET @ErrorCount = @ErrorCount + @@ERROR  
					if @NewUserId > 0 and @NewContactId > 0  --������ϵ����Ϣ�е�UserId
					begin
						update [tbl_SupplierContact] set UserId = @NewUserId where Id = @NewContactId
						--��֤����
						SET @ErrorCount = @ErrorCount + @@ERROR
					end 
				end 
			end
		end
		close cur_SupplierContact
		deallocate cur_SupplierContact
		EXEC sp_xml_removedocument @hdoc 
	end
	
	IF @ErrorCount > 0 
	BEGIN
		SET @ErrorValue = -1;
		ROLLBACK TRAN AddSupplier
	end	
	ELSE
	BEGIN
		SET @ErrorValue = 1;
		COMMIT TRAN AddSupplier
	end
	
end
GO

-- =============================================
-- Author:		���ĳ�
-- Create date: 2011-03-28
-- Description:	�޸Ĺ�Ӧ����Ϣ�������ڵؽ�����Ϣ�޸ģ�
-- Error��0��������
-- Error��-1�޸Ĺ����з�������
-- Success��1�޸ĳɹ�
-- =============================================
ALTER PROCEDURE  [dbo].[proc_CompanySupplier_Update]
(
	@Id int = 0,   --��Ӧ�̱��
	@ProvinceId int = 0 ,  --ʡ�ݱ��
	@ProvinceName nvarchar(255) = '',  --  ʡ������
	@CityId int = 0,  --���б��
	@CityName nvarchar(255) = '',  --��������
	@UnitName nvarchar(255) = '',  --��λ����
	@SupplierType tinyint = 0,  --�ؽӡ���Ʊ
	@UnitAddress nvarchar(500) = '',  --��ַ
	@Commission money = 0,  --��Ӷ
	@AgreementFile nvarchar(255) = '',  --����Э��
	@TradeNum int = 0,  --���״���
	@UnitPolicy nvarchar(max) = '',  --����
	@Remark nvarchar(max) = '',  --��ע
	@CompanyId int = 0,  --��˾���
	@IsDelete char(1) = '0',  --�Ƿ�ɾ��
	@SupplierContactXML nvarchar(max) = null,  --��ϵ����ϢXML
	@ErrorValue  int = 0 output,   --�������
	@LicenseKey NVARCHAR(50)--���֤��
)
as
BEGIN
	declare @ErrorCount int    --���������
	declare @IsUserExist int   --�û����Ƿ����
	declare @NewContactId int  --����ϵ��Id
	declare @NewUserId int   --���û�Id
	DECLARE @hdoc int   --����XML����
	--�����α����
	declare @ContactId int,		--��ϵ�˱��
			@ContactName nvarchar(50),		--��ϵ������
			@JobTitle nvarchar(255),		--ְ��
			@ContactFax nvarchar(100),		--����
			@ContactTel nvarchar(100),		--��ϵ�绰
			@ContactMobile nvarchar(100),		--��ϵ���ֻ�
			@QQ nvarchar(100),		--QQ
			@Email nvarchar(100),		--Email
			@UserId int	,	--�û����
			@UserName nvarchar(100), --�û���
			@Password varchar(50),  --��������
			@MD5Password varchar(50)   --MD5����

	set @IsUserExist = 0
	set @ErrorCount = 0
	set @NewContactId = 0
	set @NewUserId = 0

	if @Id is null or @Id <= 0
	begin
		set @ErrorValue = 0
		return
	end

	begin tran UpdateSupplier

	update tbl_CompanySupplier set 
		ProvinceId = @ProvinceId,
		ProvinceName = @ProvinceName,
		CityId = @CityId,
		CityName = @CityName,
		UnitName = @UnitName,
		SupplierType = @SupplierType,
		UnitAddress = @UnitAddress,
		Commission = @Commission,
		AgreementFile = @AgreementFile,
		TradeNum = @TradeNum,
		UnitPolicy = @UnitPolicy,
		Remark = @Remark,
		IsDelete = @IsDelete
		,LicenseKey=@LicenseKey
	where Id = @Id
	--��֤����
	SET @ErrorCount = @ErrorCount + @@ERROR
	
	if @SupplierContactXML is not null and len(@SupplierContactXML) > 1
	begin

		if @CompanyId is null or @CompanyId <= 0  --Ϊ��˾Id��ֵ
		begin
			select @CompanyId = CompanyId from tbl_CompanySupplier where Id = @Id
		end
		
		set @SupplierContactXML = replace(replace(@SupplierContactXML,char(13),''),char(10),'')
		EXEC sp_xml_preparedocument @hdoc OUTPUT, @SupplierContactXML

		--��ɾ��Ҫɾ������ϵ�˵��ʻ���Ϣ
		delete from tbl_CompanyUser where CompanyId = @CompanyId and TourCompanyId = @Id and Id in (select UserId from tbl_SupplierContact where  CompanyId = @CompanyId and [SupplierId] = @Id and Id not in (select Id from openxml(@hdoc,'/ROOT/CustomerInfo')with(Id int)))
		--ɾ����ɾ������ϵ��
		delete from [tbl_SupplierContact] where CompanyId = @CompanyId and [SupplierId] = @Id and Id not in (select Id from openxml(@hdoc,'/ROOT/CustomerInfo')with(Id int))

		DECLARE cur_SupplierContact CURSOR FOR
		SELECT Id,ContactName,JobTitle,ContactFax,ContactTel,ContactMobile,QQ,Email,UserId,UserName,[Password],MD5Password from openxml(@hdoc,'/ROOT/CustomerInfo')
		with(Id int,ContactName nvarchar(500),JobTitle nvarchar(255),ContactFax nvarchar(100),ContactTel nvarchar(100),ContactMobile nvarchar(100),QQ nvarchar(100),Email nvarchar(100),UserId int,UserName nvarchar(100),[Password] varchar(50),MD5Password varchar(50))
		open cur_SupplierContact
		while 1 = 1
		begin
			fetch next from cur_SupplierContact into @ContactId,@ContactName,@JobTitle,@ContactFax,@ContactTel,@ContactMobile,@QQ,@Email,@UserId,@UserName,@Password,@MD5Password
			if @@fetch_status<>0
				break

			if @ContactId > 0   --�޸�
			begin
				UPDATE [tbl_SupplierContact]
				   SET [CompanyId] = @CompanyId
					  ,[SupplierId] = @Id
					  ,[SupplierType] = @SupplierType
					  ,[ContactName] = @ContactName
					  ,[JobTitle] = @JobTitle
					  ,[ContactFax] = @ContactFax
					  ,[ContactTel] = @ContactTel
					  ,[ContactMobile] = @ContactMobile
					  ,[QQ] = @QQ
					  ,[Email] = @Email
				 WHERE Id = @ContactId
				--��֤����
				SET @ErrorCount = @ErrorCount + @@ERROR
				--Ϊ�û�Id��ֵ
				select @UserId from [tbl_SupplierContact] WHERE Id = @ContactId
				if @UserId > 0  --�޸��û���Ϣ
				begin
					update tbl_CompanyUser set 
						[Password] = @Password
						,MD5Password = @MD5Password
						,contactname = @ContactName
						,contactmobile = @ContactMobile
						,contactemail = @Email
						,ContactFax = @ContactFax
						,qq = @QQ
						,jobname = @JobTitle
						,ContactTel = @ContactTel
						where Id = @UserId
					--��֤����
					SET @ErrorCount = @ErrorCount + @@ERROR
				end
				else
				begin
					--����˻���Ϣ
					if @UserName is not null and len(@UserName) > 0 and @Password is not null and len(@Password) > 0
					begin
						set @IsUserExist = 0 ---�ж��û��Ƿ����
						select @IsUserExist = count(id) from tbl_CompanyUser where UserName=@UserName and CompanyId=@CompanyId
						if @IsUserExist <= 0
						begin
							INSERT INTO  tbl_CompanyUser 
								(UserName
								,[Password]
								,MD5Password
								,roleid
								,SuperviseDepartId
								,TourCompanyId
								,CompanyId
								,SuperviseDepartName
								,contactname
								,contactsex
								,contactmobile
								,contactemail
								,ContactFax
								,qq
								,jobname
								,departname
								,usertype
								,ContactTel)
								values 
								(@UserName,@Password,@MD5Password,0,0,@Id,@CompanyId,'',@ContactName,'1',@ContactMobile,@Email,@ContactFax,@QQ,@JobTitle,'',3,@ContactTel)
							select @NewUserId = @@identity
							--��֤����
							SET @ErrorCount = @ErrorCount + @@ERROR  
							if @NewUserId > 0 and @ContactId > 0  --������ϵ����Ϣ�е�UserId
							begin
								update [tbl_SupplierContact] set UserId = @NewUserId where Id = @ContactId
								--��֤����
								SET @ErrorCount = @ErrorCount + @@ERROR
							end 
						end 
					end
				end
			end
			else   --����
			begin
				INSERT INTO [tbl_SupplierContact] (
					[CompanyId]
					,[SupplierId]
					,[SupplierType]
					,[ContactName]
					,[JobTitle]
					,[ContactFax]
					,[ContactTel]
					,[ContactMobile]
					,[QQ]
					,[Email]
					) VALUES (
					@CompanyId
					,@Id
					,@SupplierType
					,@ContactName
					,@JobTitle
					,@ContactFax
					,@ContactTel
					,@ContactMobile
					,@QQ
					,@Email)
				select @NewContactId = @@identity
				--��֤����
				SET @ErrorCount = @ErrorCount + @@ERROR
				--����˻���Ϣ
				if @UserName is not null and len(@UserName) > 0 and @Password is not null and len(@Password) > 0
				begin
					set @IsUserExist = 0 ---�ж��û��Ƿ����
					select @IsUserExist = count(id) from tbl_CompanyUser where UserName=@UserName and CompanyId=@CompanyId
					if @IsUserExist <= 0
					begin
						INSERT INTO  tbl_CompanyUser 
							(UserName
							,[Password]
							,MD5Password
							,roleid
							,SuperviseDepartId
							,TourCompanyId
							,CompanyId
							,SuperviseDepartName
							,contactname
							,contactsex
							,contactmobile
							,contactemail
							,ContactFax
							,qq
							,jobname
							,departname
							,usertype
							,ContactTel)
							values 
							(@UserName,@Password,@MD5Password,0,0,@Id,@CompanyId,'',@ContactName,'1',@ContactMobile,@Email,@ContactFax,@QQ,@JobTitle,'',3,@ContactTel)
						select @NewUserId = @@identity
						--��֤����
						SET @ErrorCount = @ErrorCount + @@ERROR  
						if @NewUserId > 0 and @NewContactId > 0  --������ϵ����Ϣ�е�UserId
						begin
							update [tbl_SupplierContact] set UserId = @NewUserId where Id = @NewContactId
							--��֤����
							SET @ErrorCount = @ErrorCount + @@ERROR
						end 
					end 
				end
			end
		end
		close cur_SupplierContact
		deallocate cur_SupplierContact
		EXEC sp_xml_removedocument @hdoc 
	end

	IF @ErrorCount > 0 
	BEGIN
		SET @ErrorValue = -1;
		ROLLBACK TRAN UpdateSupplier
	end	
	ELSE
	BEGIN
		SET @ErrorValue = 1;
		COMMIT TRAN UpdateSupplier
	end

end
GO

-- =============================================
-- Author:		����־
-- Create date: 2011-07-04
-- Description:	�����ǼǸ���
-- @ExpenseType:(0:�������Ӧ�̰��ţ�1:�ؽӣ�2:Ʊ��255:����)
-- =============================================
CREATE PROCEDURE proc_BatchRegisterExpense
	@PaymentTime DATETIME--֧��ʱ��
	,@PaymentType TINYINT--֧������
	,@PayerId INT--�����˱��
	,@Payer NVARCHAR(50)--����������
	,@Remark NVARCHAR(255)--��ע
	,@ExpenseType TINYINT=255--֧�����ͣ�Ϊ255ʱ��������֧��
	,@OperatorId INT--����Ա���
	,@CompanyId INT--��˾���
	,@TourIdsXML NVARCHAR(MAX)--�ƻ���ż���XML��<ROOT><Info TourId="�ƻ����" /></ROOT>
	,@Result INT OUTPUT--�������
AS
BEGIN
	SET @Result=0

	DECLARE @hdoc INT
	DECLARE @tmptbl TABLE(TourId CHAR(36))
	
	EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@TourIdsXML
	INSERT INTO @tmptbl(TourId) SELECT A.TourId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TourId CHAR(36)) AS A
	EXECUTE sp_xml_removedocument @hdoc

	IF(@ExpenseType=0 OR @ExpenseType=255)--����֧��
	BEGIN
		INSERT INTO [tbl_FinancialPayInfo]([Id],[CompanyID],[ItemId],[ItemType],[ReceiveId]
			,[ReceiveType],[ReceiveCompanyId],[ReceiveCompanyName],[PaymentDate],[StaffNo]
			,[StaffName],[PaymentAmount],[PaymentType],[IsBill],[BillAmount]
			,[BillNo],[Remark],[IsChecked],[OperatorID],[IssueTime]
			,[CheckerId],[IsPay])
		SELECT NEWID(),@CompanyId,A.TourId,0,B.PlanId
			,0,0,'',@PaymentTime,@PayerId
			,@Payer,B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.PlanId AND [ReceiveType]=0),@PaymentType,'0',0
			,'',@Remark,'0',@OperatorId,GETDATE()
			,0,'0'
		FROM @tmptbl AS A INNER JOIN tbl_PlanSingle AS B
		ON A.TourId=B.TourId AND B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=B.TourId AND ItemType=0 AND [ReceiveId]=B.PlanId AND [ReceiveType]=0)>0
	END

	IF(@ExpenseType=1 OR @ExpenseType=255)--�ؽ�֧��
	BEGIN
		INSERT INTO [tbl_FinancialPayInfo]([Id],[CompanyID],[ItemId],[ItemType],[ReceiveId]
			,[ReceiveType],[ReceiveCompanyId],[ReceiveCompanyName],[PaymentDate],[StaffNo]
			,[StaffName],[PaymentAmount],[PaymentType],[IsBill],[BillAmount]
			,[BillNo],[Remark],[IsChecked],[OperatorID],[IssueTime]
			,[CheckerId],[IsPay])
		SELECT NEWID(),@CompanyId,A.TourId,0,B.Id
			,1,0,'',@PaymentTime,@PayerId
			,@Payer,B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=1),@PaymentType,'0',0
			,'',@Remark,'0',@OperatorId,GETDATE()
			,0,'0'
		FROM @tmptbl AS A INNER JOIN tbl_PlanLocalAgency AS B
		ON A.TourId=B.TourId AND B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=B.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=1)>0
	END

	IF(@ExpenseType=2 OR @ExpenseType=255)--��Ʊ֧��
	BEGIN
		INSERT INTO [tbl_FinancialPayInfo]([Id],[CompanyID],[ItemId],[ItemType],[ReceiveId]
			,[ReceiveType],[ReceiveCompanyId],[ReceiveCompanyName],[PaymentDate],[StaffNo]
			,[StaffName],[PaymentAmount],[PaymentType],[IsBill],[BillAmount]
			,[BillNo],[Remark],[IsChecked],[OperatorID],[IssueTime]
			,[CheckerId],[IsPay])
		SELECT NEWID(),@CompanyId,A.TourId,0,B.Id
			,2,0,'',@PaymentTime,@PayerId
			,@Payer,B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=2 AND [State]=3),@PaymentType,'0',0
			,'',@Remark,'0',@OperatorId,GETDATE()
			,0,'0'
		FROM @tmptbl AS A INNER JOIN tbl_PlanTicketOut AS B
		ON A.TourId=B.TourId AND B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=B.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=2 AND [State]=3)>0
	END

	SET @Result=1
	RETURN @Result
END
GO

-- =============================================
-- Author:		����־
-- Create date: 2011-07-04
-- Description:	ȡ������
-- =============================================
CREATE PROCEDURE proc_TourOrder_CancelOrder
	@OrderId CHAR(36)--�������
	,@Result INT OUTPUT--��� 1:�ɹ�
AS
BEGIN
	SET @Result=0

	DECLARE @TourId CHAR(36)--�ƻ����
	DECLARE @RouteId INT--��·���
	SELECT @TourId=[TourId] FROM [tbl_TourOrder] WHERE [Id]=@OrderId
	SELECT @RouteId=[RouteId] FROM [tbl_Tour] WHERE [TourId]=@TourId
	
	--���¶���״̬Ϊ������
	UPDATE [tbl_TourOrder] SET [OrderState]=4 WHERE [Id]=@OrderId
	--ɾ������
	UPDATE [tbl_StatAllIncome] SET [IsDelete]='1' WHERE [ItemId]=@OrderId AND [ItemType]=1	
	IF(@RouteId>0)--��·���տ���ά��
	BEGIN
		UPDATE [tbl_Route] SET VisitorCount=ISNULL((SELECT SUM(AdultNumber+ChildNumber) FROM [tbl_TourOrder] WHERE [OrderState] IN(1,2,5) AND RouteId=@RouteId AND IsDelete='0'),0) WHERE [Id]=@RouteId
	END
	--ά���Ŷ�����ʵ����=����������(������ס���ڡ�ȡ���Ķ���)
	UPDATE [tbl_Tour] SET [VirtualPeopleNumber]=(SELECT ISNULL(SUM([PeopleNumber]-[LeaguePepoleNum]),0) FROM [tbl_TourOrder] WHERE [TourId]=@TourId  AND [IsDelete]='0' AND [OrderState] IN(1,2,5))  WHERE [TourId]=@TourId
	--ά���ƻ�������
	UPDATE [tbl_Tour] SET [TotalIncome]=ISNULL((SELECT SUM([FinanceSum]) FROM [tbl_TourOrder] AS B WHERE B.[TourId]=@TourId AND B.[IsDelete]='0' AND B.[OrderState] NOT IN(3,4)),0) WHERE TourId=@TourId
	--�Ŷ�״̬ά�� ������״̬�������տ�״̬
	UPDATE [tbl_Tour] SET [Status]=0 FROM [tbl_Tour] AS A 
	WHERE A.[Status]=1 AND A.TourId=@TourId AND A.PlanPeopleNumber>(SELECT ISNULL(SUM(B.PeopleNumber-B.LeaguePepoleNum),0) FROM tbl_TourOrder AS B WHERE B.TourId=@TourId AND B.[OrderState] NOT IN(3,4) AND IsDelete='0')
	--ά���ƻ�����״̬
	DECLARE @CalculationTourSettleStatusResult INT
	EXEC [proc_Utility_CalculationTourSettleStatus] @TourId=@TourId,@Result=@CalculationTourSettleStatusResult OUTPUT

	SET @Result=1
	RETURN @Result
END
GO

--�ͻ���λ��Ϣ������logo�ֶ� 2011-07-05
ALTER TABLE dbo.tbl_Customer ADD
	FilePathLogo nvarchar(255) NULL
GO
DECLARE @v sql_variant 
SET @v = N'Logo'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Customer', N'COLUMN', N'FilePathLogo'
GO


--create table ����ѯ���ο���Ϣ�� 2011-07-06
create table tbl_CustomerQuoteTraveller (
   TravellerId          char(36)             not null,
   QuoteId              int                  not null,
   TravellerName        nvarchar(255)        null,
   CertificateType      tinyint              null default 0,
   CertificateCode      nvarchar(255)        null,
   Gender               tinyint              not null default 0,
   TravellerType        tinyint              not null default 0,
   Telephone            nvarchar(255)        null,
   CreateTime           DateTime             not null default getdate(),
   constraint PK_TBL_CUSTOMERQUOTETRAVELLER primary key (TravellerId)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '�οͱ��',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTraveller', 'column', 'TravellerId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'ѯ�۱��',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTraveller', 'column', 'QuoteId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '�ο�����',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTraveller', 'column', 'TravellerName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '֤������',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTraveller', 'column', 'CertificateType'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '֤�����',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTraveller', 'column', 'CertificateCode'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '�ο��Ա�',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTraveller', 'column', 'Gender'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '�ο����� (���ˡ���ͯ)',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTraveller', 'column', 'TravellerType'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '��ϵ�绰',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTraveller', 'column', 'Telephone'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '���ʱ��',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTraveller', 'column', 'CreateTime'
go

alter table tbl_CustomerQuoteTraveller
   add constraint FK_TBL_CUSTOMERQUOTETRA_REFERENCE_TBL_CUSTOMERQUOTE foreign key (QuoteId)
      references tbl_CustomerQuote (Id)
go

--����ѯ����Ϣ�������ֶ� 2011-07-06
ALTER TABLE dbo.tbl_CustomerQuote ADD
	TravellerDisplayType tinyint NOT NULL CONSTRAINT DF_tbl_CustomerQuote_TravellerDisplayType DEFAULT 0,
	TravellerFilePath nvarchar(255) NULL
GO
DECLARE @v sql_variant 
SET @v = N'�ο���Ϣ�������ͣ�������ʽ�����뷽ʽ��'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_CustomerQuote', N'COLUMN', N'TravellerDisplayType'
GO
DECLARE @v sql_variant 
SET @v = N'�ο���Ϣ����'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_CustomerQuote', N'COLUMN', N'TravellerFilePath'
GO

--=========================================
--���������Ŷ�ѯ�����
--�����ˣ��ܺ���
--ʱ�䣺2011-03-18
-- History:
-- 1.2011-07-06 �����ο���Ϣ ����־
--=========================================
ALTER procedure [dbo].[proc_Inquire_InsertInquire]
(
	@CompanyId int,--ר�߹�˾���
    @RouteId int,--��·���
    @RouteName nvarchar(255),--��·����
    @CustomerId int,--���Ź�˾���
    @CustomerName nvarchar(255),--���Ź�˾����
    @ContactTel nvarchar(255),-- ��ϵ�˵绰
    @ContactName nvarchar(255),--��ϵ������
    @AdultNumber int, --������
    @ChildNumber int, --��ͯ��
    @LeaveDate datetime, --Ԥ�Ƴ�������
    @SpecialClaim nvarchar(max), --����Ҫ��˵��
    @XingCheng nvarchar(max), --�г�Ҫ�� XML:<ROOT><XingCheng QuoteId="ѯ�۱��" QuotePlan="�г�Ҫ��" PlanAccessoryName="�г̸�������" PlanAccessory="�г̸���"></ROOT>
	@ASK NVARCHAR(MAX),--����Ҫ�� XML:<ROOT><QuoteAskInfo QuoteId="ѯ�۱��" ItemType="��Ŀ����" ConcreteAsk="����Ҫ��" /></ROOT>
    @QuoteState tinyint, --ѯ��״̬
    @Remark varchar(max), ---��ע
    @Result INT OUTPUT --������� ��ֵ���ɹ� ��ֵ��0��ʧ��
	,@TravellerDisplayType TINYINT--�ο���Ϣ��������
	,@TravellerFilePath NVARCHAR(255)=NULL--�ο���Ϣ����·��
	,@Travellers NVARCHAR(MAX)=NULL--�ο���Ϣ���� XML:<ROOT><Info TravellerId="�οͱ��" TravellerName="�ο�����" CertificateType="֤������" CertificateCode="֤������" Gender="�Ա�" TravellerType="�ο�����" Telephone="�绰" /></ROOT>
)
as
begin
	DECLARE @hdoc INT
	DECLARE @errorcount INT
    DECLARE @Id int
 	SET @Result=0
	SET @errorcount=0
    SET @Id=0
	begin tran
	begin
--==========================����ѯ�ۻ�����Ϣ==========================--
		insert into tbl_CustomerQuote
		(
			CompanyId,RouteId,RouteName,CustomerId,CustomerName,LeaveDate,
			ContactTel,ContactName,AdultNumber,ChildNumber,SpecialClaim,Remark,QuoteState
			,TravellerDisplayType,TravellerFilePath
		)
		values
		(
			@CompanyId,@RouteId,@RouteName,@CustomerId,@CustomerName,@LeaveDate,
			@ContactTel,@ContactName,@AdultNumber,@ChildNumber,@SpecialClaim,@Remark,@QuoteState
			,@TravellerDisplayType,@TravellerFilePath
		)
		SET @errorcount=@@error+@errorcount
		SELECT @Id=@@IDENTITY
		
--==========================�����г�Ҫ��================================----
       if @Id<>0 and @XingCheng is not null and len(@XingCheng)>0
           begin
	       EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@XingCheng
	       INSERT INTO [tbl_CustomerQuoteClaim]([QuoteId],[QuotePlan],[PlanAccessoryName],[PlanAccessory])
	       SELECT @Id,[QuotePlan],[PlanAccessoryName],[PlanAccessory]
	       FROM OPENXML(@hdoc,N'/ROOT/XingCheng')
	       WITH([QuotePlan] NVARCHAR(MAX),[PlanAccessoryName] NVARCHAR(255),[PlanAccessory] NVARCHAR(255))
	       SET @errorcount=@errorcount+@@ERROR
	       EXECUTE sp_xml_removedocument @hdoc
         end
--==========================�������Ҫ��================================----
	     if @Id<>0 and @ASK is not null and len(@ASK)>0
         begin
	       EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@ASK
	       INSERT INTO [tbl_CustomerQuoteAsk]([QuoteId],[ItemType],[ConcreteAsk])
	       SELECT @Id,[ItemType],[ConcreteAsk]
	       FROM OPENXML(@hdoc,N'/ROOT/QuoteAskInfo')
	       WITH([ItemType] INT,[ConcreteAsk] NVARCHAR(MAX))
	       SET @errorcount=@errorcount+@@ERROR
	       EXECUTE sp_xml_removedocument @hdoc
         end
--==========================�������Ҫ�����================================----

	IF(@errorcount=0 AND @Travellers IS NOT NULL AND LEN(@Travellers)>0)--д���ο���Ϣ
	BEGIN
		EXEC sp_xml_preparedocument @hdoc OUTPUT,@Travellers
		INSERT INTO [tbl_CustomerQuoteTraveller]([TravellerId],[QuoteId],[TravellerName],[CertificateType],[CertificateCode]
			,[Gender],[TravellerType],[Telephone],[CreateTime])
		SELECT A.TravellerId,@Id,A.TravellerName,A.CertificateType,A.CertificateCode
			,A.Gender,A.TravellerType,A.Telephone,GETDATE()
		FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TravellerId CHAR(36),TravellerName NVARCHAR(255),CertificateType TINYINT,CertificateCode NVARCHAR(255),Gender TINYINT,TravellerType TINYINT,Telephone NVARCHAR(255)) AS A
		SET @errorcount=@errorcount+@@ERROR
		EXEC sp_xml_removedocument @hdoc
	END

	if 	@errorcount<>0
	begin	
        rollback tran
        return @Result
	end
 end
commit tran
set @Result=1
return @Result
end
GO

--=========================================
--���������Ŷ�ѯ�۸���
--�����ˣ��ܺ���
--ʱ�䣺2011-03-18
-- History:
-- 1.2011-07-06 �����ο���Ϣ ����־
--=========================================
ALTER procedure [dbo].[proc_Inquire_UpdateInquire]
(
    @Id int,--ѯ�۱��۱��
	@CompanyId int,--ר�߹�˾���
    @RouteId int,--��·���
    @RouteName nvarchar(255),--��·����
    @CustomerId int,--���Ź�˾���
    @CustomerName nvarchar(255),--���Ź�˾����
    @ContactTel nvarchar(255),-- ��ϵ�˵绰
    @ContactName nvarchar(255),--��ϵ������
    @LeaveDate datetime, --Ԥ�Ƴ�������
    @AdultNumber int, --������
    @ChildNumber int, --��ͯ��
    @SpecialClaim nvarchar(max), --����Ҫ��˵��
    @Remark varchar(max), ---��ע
    @QuoteState tinyint,  --ѯ��״̬ δ���� = 0,�Ѵ��� = 1,�ѳɹ� = 2
    @XingCheng nvarchar(max), --�г�Ҫ�� XML:<ROOT><XingCheng QuoteId="ѯ�۱��" QuotePlan="�г�Ҫ��" PlanAccessoryName="�г̸�������" PlanAccessory="�г̸���"></ROOT>
	@QuoteInfo NVARCHAR(MAX),--�۸���� XML:<ROOT><QuoteInfo QuoteId="ѯ�۱��" ItemId="��Ŀ����" Reception="�Ӵ���׼" LocalQuote="�ؽӱ���" MyQuote="���籨��" SumMoney="�ϼƽ��"/></ROOT>
	@ASK NVARCHAR(MAX),--����Ҫ�� XML:<ROOT><QuoteAskInfo QuoteId="ѯ�۱��" ItemType="��Ŀ����" ConcreteAsk="����Ҫ��" /></ROOT>
    @Result INT OUTPUT --������� ��ֵ���ɹ� ��ֵ��0��ʧ��
	,@TravellerDisplayType TINYINT--�ο���Ϣ��������
	,@TravellerFilePath NVARCHAR(255)=NULL--�ο���Ϣ����·��
	,@Travellers NVARCHAR(MAX)=NULL--�ο���Ϣ���� XML:<ROOT><Info TravellerId="�οͱ��" TravellerName="�ο�����" CertificateType="֤������" CertificateCode="֤������" Gender="�Ա�" TravellerType="�ο�����" Telephone="�绰" /></ROOT>
)
as
begin
	DECLARE @hdoc INT
	DECLARE @errorcount INT
 	SET @Result=0
	SET @errorcount=0
    begin tran
    IF EXISTS(SELECT * FROM [tbl_CustomerQuote] WHERE Id=@Id AND CustomerId=@CustomerId)
	begin
--==========================����ѯ�ۻ�����Ϣ==========================--
		update tbl_CustomerQuote
		set 
         RouteName=@RouteName,
--         CustomerId=@CustomerId,
--         CustomerName=@CustomerName,
		 ContactTel=@ContactTel,
         ContactName=@ContactName,
         AdultNumber=@AdultNumber,
         ChildNumber=@ChildNumber,
         SpecialClaim=@SpecialClaim,
         LeaveDate=@LeaveDate,
         Remark=@Remark,
         QuoteState=@QuoteState
			,TravellerDisplayType=@TravellerDisplayType
			,TravellerFilePath=@TravellerFilePath
        where Id=@Id
		SET @errorcount=@@error+@errorcount
		
--==========================�����г�Ҫ��================================----
       if @Id<>0 and @XingCheng is not null and len(@XingCheng)>0
         begin
           DELETE FROM [tbl_CustomerQuoteClaim] WHERE [QuoteId]=@Id
	       EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@XingCheng
	       INSERT INTO [tbl_CustomerQuoteClaim]([QuoteId],[QuotePlan],[PlanAccessoryName],[PlanAccessory])
	       SELECT @Id,[QuotePlan],[PlanAccessoryName],[PlanAccessory]
	       FROM OPENXML(@hdoc,'/ROOT/XingCheng')
	       WITH([QuotePlan] NVARCHAR(MAX),[PlanAccessoryName] NVARCHAR(255),[PlanAccessory] NVARCHAR(255))
	       SET @errorcount=@errorcount+@@ERROR
	       EXECUTE sp_xml_removedocument @hdoc
         end
--==========================����۸����,��ɾ�ٲ�================================----
       DELETE FROM [tbl_CustomerQuotePrice] WHERE [QuoteId]=@Id
       SET @errorcount=@errorcount+@@ERROR
       if @errorcount=0 and @QuoteInfo is not null and len(@QuoteInfo)>0
         begin
	       EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@QuoteInfo
	       INSERT INTO [tbl_CustomerQuotePrice]([QuoteId],[ItemId],[Reception],[LocalQuote],[MyQuote])
	       SELECT @Id,[ItemId],[Reception],[LocalQuote],[MyQuote]
	       FROM OPENXML(@hdoc,'/ROOT/QuoteInfo')
	       WITH([ItemId] INT,[Reception] NVARCHAR(MAX),[LocalQuote] MONEY,[MyQuote] MONEY)
	       SET @errorcount=@errorcount+@@ERROR
	       EXECUTE sp_xml_removedocument @hdoc
         end

--==========================�������Ҫ��================================----
	     if @Id<>0 and @ASK is not null and len(@ASK)>0
         begin
           DELETE FROM [tbl_CustomerQuoteAsk] WHERE [QuoteId]=@Id
	       EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@ASK
	       INSERT INTO [tbl_CustomerQuoteAsk]([QuoteId],[ItemType],[ConcreteAsk])
	       SELECT @Id,[ItemType],[ConcreteAsk]
	       FROM OPENXML(@hdoc,'/ROOT/QuoteAskInfo')
	       WITH([ItemType] INT,[ConcreteAsk] NVARCHAR(MAX))
	       SET @errorcount=@errorcount+@@ERROR
	       EXECUTE sp_xml_removedocument @hdoc
         end
--==========================�������Ҫ�����================================----

	IF(@errorcount=0)--�ο���Ϣ
	BEGIN
		DELETE FROM [tbl_CustomerQuoteTraveller] WHERE [QuoteId]=@Id
		SET @errorcount=@errorcount+@@ERROR
		
		IF(@errorcount=0 AND @Travellers IS NOT NULL AND LEN(@Travellers)>0)--д���ο���Ϣ
		BEGIN
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@Travellers
			INSERT INTO [tbl_CustomerQuoteTraveller]([TravellerId],[QuoteId],[TravellerName],[CertificateType],[CertificateCode]
				,[Gender],[TravellerType],[Telephone],[CreateTime])
			SELECT A.TravellerId,@Id,A.TravellerName,A.CertificateType,A.CertificateCode
				,A.Gender,A.TravellerType,A.Telephone,GETDATE()
			FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TravellerId CHAR(36),TravellerName NVARCHAR(255),CertificateType TINYINT,CertificateCode NVARCHAR(255),Gender TINYINT,TravellerType TINYINT,Telephone NVARCHAR(255)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXEC sp_xml_removedocument @hdoc
		END
	END

	if 	@errorcount<>0
	begin	
		rollback tran
        return @Result
	end
 end
    commit tran
    set @Result=1
	return @Result

end
GO

--=========================================
--������ר�߶˸��±���
--�����ˣ��ܺ���
--ʱ�䣺2011-03-20
--History:
--1.2011-04-18 �����ʽ����
--2.2011-04-18 ����־ ���ӵؽӱ������������ۼ����籨�����������ۣ��ϼƽ��
--3.2011-07-06 �����ο���Ϣ ����־
--=========================================
ALTER procedure [dbo].[proc_Inquire_UpdateQuote]
(
	@Id int, --ѯ�۱��
	@CompanyId int,--ר�߹�˾���
	@RouteId int,--��·���
	@RouteName nvarchar(255),--��·����
	@ContactTel nvarchar(255),-- ��ϵ�˵绰
	@ContactName nvarchar(255),--��ϵ������
	@AdultNumber int, --������
	@ChildNumber int, --��ͯ��
	@TicketAgio decimal,--��Ʊ�ۿ�
	@SpecialClaim nvarchar(max), --����Ҫ��˵��
    @LeaveDate datetime, --Ԥ�Ƴ�������
	@XingCheng nvarchar(max), --�г�Ҫ�� XML:<ROOT><XingCheng QuoteId="ѯ�۱��" QuotePlan="�г�Ҫ��" PlanAccessoryName="�г̸�������" PlanAccessory="�г̸���"></ROOT>
	@ASK NVARCHAR(MAX),--����Ҫ�� XML:<ROOT><QuoteAskInfo QuoteId="ѯ�۱��" ItemType="��Ŀ����" ConcreteAsk="����Ҫ��" /></ROOT>
	@QuoteInfo NVARCHAR(MAX),--�۸���� XML:<ROOT><QuoteInfo QuoteId="ѯ�۱��" ItemId="��Ŀ����" Reception="�Ӵ���׼" LocalQuote="�ؽӱ���" MyQuote="���籨��" LocalPeopleNumber="�ؽӱ�������" LocalUnitPrice="�ؽӱ��۵���" SelfPeopleNumber="���籨������" SelfUnitPrice="���籨�۵���" /></ROOT>
	@Remark varchar(max), ---��ע    
	@QuoteState int, --ѯ��״̬
	@Result INT OUTPUT, --������� ��ֵ���ɹ� ��ֵ��0��ʧ��
	@TotalAmount MONEY --�ϼƽ��
	,@TravellerDisplayType TINYINT--�ο���Ϣ��������
	,@TravellerFilePath NVARCHAR(255)=NULL--�ο���Ϣ����·��
	,@Travellers NVARCHAR(MAX)=NULL--�ο���Ϣ���� XML:<ROOT><Info TravellerId="�οͱ��" TravellerName="�ο�����" CertificateType="֤������" CertificateCode="֤������" Gender="�Ա�" TravellerType="�ο�����" Telephone="�绰" /></ROOT>
)
as
begin
	DECLARE @hdoc INT
	DECLARE @errorcount INT
 	SET @Result=0
	SET @errorcount=0
	begin tran Inquire_InsertQuote

	update tbl_CustomerQuote set RouteId=@RouteId,RouteName=@RouteName,ContactTel=@ContactTel,ContactName=@ContactName,
		AdultNumber=@AdultNumber,ChildNumber=@ChildNumber,QuoteState=@QuoteState,TicketAgio=@TicketAgio,SpecialClaim=@SpecialClaim,
		Remark=@Remark,TotalAmount=@TotalAmount,LeaveDate=@LeaveDate
		,TravellerDisplayType=@TravellerDisplayType,TravellerFilePath=@TravellerFilePath
    where Id=@Id
	SET @errorcount=@@error+@errorcount
	
	IF(@errorcount=0 and @XingCheng is not null and len(@XingCheng)>0)--�����г�Ҫ��
	BEGIN
		DELETE FROM [tbl_CustomerQuoteClaim] WHERE [QuoteId]=@Id
		SET @errorcount=@errorcount+@@ERROR
		if(@errorcount=0)
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@XingCheng
		   INSERT INTO [tbl_CustomerQuoteClaim]([QuoteId],[QuotePlan],[PlanAccessoryName],[PlanAccessory])
		   SELECT @Id,[QuotePlan],[PlanAccessoryName],[PlanAccessory]
		   FROM OPENXML(@hdoc,'/ROOT/XingCheng')
		   WITH([QuotePlan] NVARCHAR(MAX),[PlanAccessoryName] NVARCHAR(255),[PlanAccessory] NVARCHAR(255))
		   SET @errorcount=@errorcount+@@ERROR
		   EXECUTE sp_xml_removedocument @hdoc
		END
	END

	IF(@errorcount=0 and @QuoteInfo is not null and len(@QuoteInfo)>0)--����۸����,��ɾ�ٲ�
	BEGIN
		DELETE FROM [tbl_CustomerQuotePrice] WHERE [QuoteId]=@Id
		SET @errorcount=@errorcount+@@ERROR
		IF(@errorcount=0)
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@QuoteInfo
			INSERT INTO [tbl_CustomerQuotePrice]([QuoteId],[ItemId],[Reception],[LocalQuote],[MyQuote],[LocalPeopleNumber],[LocalUnitPrice],[SelfPeopleNumber],[SelfUnitPrice])
			SELECT @Id,[ItemId],[Reception],[LocalQuote],[MyQuote],[LocalPeopleNumber],[LocalUnitPrice],[SelfPeopleNumber],[SelfUnitPrice]
			FROM OPENXML(@hdoc,'/ROOT/QuoteInfo')
			WITH([ItemId] INT,[Reception] NVARCHAR(MAX),[LocalQuote] MONEY,[MyQuote] MONEY,[LocalPeopleNumber] INT,[LocalUnitPrice] MONEY,[SelfPeopleNumber] INT,[SelfUnitPrice] MONEY)
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
	END

	IF(@errorcount=0 and @ASK is not null and len(@ASK)>0)--=�������Ҫ��
	BEGIN
		DELETE FROM [tbl_CustomerQuoteAsk] WHERE [QuoteId]=@Id
		SET @errorcount=@errorcount+@@ERROR
		IF(@errorcount=0)
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@ASK
			INSERT INTO [tbl_CustomerQuoteAsk]([QuoteId],[ItemType],[ConcreteAsk])
			SELECT @Id,[ItemType],[ConcreteAsk]
			FROM OPENXML(@hdoc,'/ROOT/QuoteAskInfo')
			WITH([ItemType] INT,[ConcreteAsk] NVARCHAR(MAX))
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
	END

	IF(@errorcount=0)--�ο���Ϣ
	BEGIN
		DELETE FROM [tbl_CustomerQuoteTraveller] WHERE [QuoteId]=@Id
		SET @errorcount=@errorcount+@@ERROR
		
		IF(@errorcount=0 AND @Travellers IS NOT NULL AND LEN(@Travellers)>0)--д���ο���Ϣ
		BEGIN
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@Travellers
			INSERT INTO [tbl_CustomerQuoteTraveller]([TravellerId],[QuoteId],[TravellerName],[CertificateType],[CertificateCode]
				,[Gender],[TravellerType],[Telephone],[CreateTime])
			SELECT A.TravellerId,@Id,A.TravellerName,A.CertificateType,A.CertificateCode
				,A.Gender,A.TravellerType,A.Telephone,GETDATE()
			FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TravellerId CHAR(36),TravellerName NVARCHAR(255),CertificateType TINYINT,CertificateCode NVARCHAR(255),Gender TINYINT,TravellerType TINYINT,Telephone NVARCHAR(255)) AS A
			SET @errorcount=@errorcount+@@ERROR
			EXEC sp_xml_removedocument @hdoc
		END
	END

	IF(@errorcount>0)
	BEGIN
		ROLLBACK TRAN
		SET @Result=0
		RETURN -1
	END

	COMMIT TRAN
	SET @Result=1	
end
GO

-- =============================================
-- Author:		<�ܺ���>
-- Create date: <2011-3-18>
-- Description:	<��ȡѯ�۱�����Ϣ>
-- =============================================
ALTER PROCEDURE [dbo].[proc_Tour_GetInquireQuote] 
@Id int, ---�������
@CompanyId int, ---ר�߹�˾���
@CustomerId int, ---���Ź�˾���
@IsZhuTuan varchar(1) ---�Ƿ�����
AS
---�������Ŷ�
IF(@IsZhuTuan=0)
begin
IF EXISTS(SELECT 1 FROM tbl_CustomerQuote WHERE Id=@Id AND CompanyId=@CompanyId)
BEGIN
---ѯ�۱��ۻ�����Ϣ
SELECT * FROM tbl_CustomerQuote WHERE Id=@Id
---����ѯ���г�Ҫ��
SELECT * FROM tbl_CustomerQuoteClaim WHERE [QuoteId]=@Id
---����ѯ�ۿͻ�Ҫ��
SELECT * FROM tbl_CustomerQuoteAsk WHERE [QuoteId]=@Id
---����ѯ�ۼ۸����
SELECT * FROM tbl_CustomerQuotePrice WHERE [QuoteId]=@Id
--�ο���Ϣ
SELECT * FROM [tbl_CustomerQuoteTraveller] WHERE [QuoteId]=@Id
END
end
----�����Ŷ�
else if(@IsZhuTuan=1)
begin
IF EXISTS(SELECT 1 FROM tbl_CustomerQuote WHERE Id=@Id AND CustomerId=@CustomerId)
BEGIN
---ѯ�۱��ۻ�����Ϣ
SELECT * FROM tbl_CustomerQuote WHERE Id=@Id
---����ѯ���г�Ҫ��
SELECT * FROM tbl_CustomerQuoteClaim WHERE [QuoteId]=@Id
---����ѯ�ۿͻ�Ҫ��
SELECT * FROM tbl_CustomerQuoteAsk WHERE [QuoteId]=@Id
---����ѯ�ۼ۸����
SELECT * FROM tbl_CustomerQuotePrice WHERE [QuoteId]=@Id
--�ο���Ϣ
SELECT * FROM [tbl_CustomerQuoteTraveller] WHERE [QuoteId]=@Id
END
end
GO

-- =============================================
-- Author:		����־
-- Create date: 2011-07-06
-- Description:	ͬ������ѯ���ο���Ϣ���ŶӼƻ�����
-- =============================================
CREATE PROCEDURE proc_SyncQuoteTravellerToTourTeamOrder
	@QuoteId INT--ѯ�۱��
	,@TourId CHAR(36)--�ŶӼƻ����
	,@Result INT OUTPUT --���
AS
BEGIN
	SET @Result=0
	IF NOT EXISTS(SELECT 1 FROM [tbl_CustomerQuote] WHERE [Id]=@QuoteId) RETURN
	IF NOT EXISTS(SELECT 1 FROM [tbl_Tour] WHERE [TourId]=@TourId AND [TourType]=1) RETURN
	DECLARE @OrderId CHAR(36)--�ŶӼƻ��������
	DECLARE @TravellerDisplayType TINYINT--�ο���Ϣ��������
	DECLARE @TravellerFilePath NVARCHAR(255)--�ο���Ϣ����
	DECLARE @CompanyId INT--��˾���

	SELECT @OrderId=[Id] FROM [tbl_TourOrder] WHERE [TourId]=@TourId
	SELECT @TravellerDisplayType=@TravellerDisplayType,@TravellerFilePath=@TravellerFilePath,@CompanyId=[CompanyId] FROM [tbl_CustomerQuote] WHERE [Id]=@QuoteId
	
	UPDATE [tbl_TourOrder] SET [CustomerDisplayType]=@TravellerDisplayType,[CustomerFilePath]=@TravellerFilePath WHERE [Id]=@OrderId
	
	INSERT INTO [tbl_TourOrderCustomer]([ID],[OrderId],[CompanyID],[VisitorName],[CradType]
		,[CradNumber],[Sex],[VisitorType],[ContactTel],[IssueTime]
		,[TicketStatus],[CustomerStatus])
	SELECT A.TravellerId,@OrderId,@CompanyId,A.TravellerName,A.CertificateType
		,A.CertificateCode,A.Gender,A.TravellerType,A.Telephone,GETDATE()
		,0,0
	FROM [tbl_CustomerQuoteTraveller] AS A
	WHERE A.[QuoteId]=@QuoteId
	
	SET @Result=1
	RETURN	
END
GO

--create table tbl_CustomerQuoteTravellerSpecialService ��������ѯ���ο��ط���Ϣ�� 2011-07-07
if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tbl_CustomerQuoteTravellerSpecialService') and o.name = 'FK_TBL_CUSTOMERQUOTETRA_REFERENCE_TBL_CUSTOMERQUOTETRA')
alter table tbl_CustomerQuoteTravellerSpecialService
   drop constraint FK_TBL_CUSTOMERQUOTETRA_REFERENCE_TBL_CUSTOMERQUOTETRA
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tbl_CustomerQuoteTravellerSpecialService')
            and   type = 'U')
   drop table tbl_CustomerQuoteTravellerSpecialService
go

create table tbl_CustomerQuoteTravellerSpecialService (
   TravellerId          char(36)             not null,
   ServiceName          nvarchar(250)        not null,
   ServiceDetail        nvarchar(500)        null,
   IsAdd                tinyint              not null default 0,
   Fee                  decimal              not null default 0,
   IssueTime            datetime             not null default getdate()
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '�οͱ��',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTravellerSpecialService', 'column', 'TravellerId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '������Ŀ',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTravellerSpecialService', 'column', 'ServiceName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '��������',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTravellerSpecialService', 'column', 'ServiceDetail'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '��/��',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTravellerSpecialService', 'column', 'IsAdd'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '����',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTravellerSpecialService', 'column', 'Fee'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '���ʱ��',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTravellerSpecialService', 'column', 'IssueTime'
go

alter table tbl_CustomerQuoteTravellerSpecialService
   add constraint FK_TBL_CUSTOMERQUOTETRA_REFERENCE_TBL_CUSTOMERQUOTETRA foreign key (TravellerId)
      references tbl_CustomerQuoteTraveller (TravellerId)
go

--=========================================
--���������Ŷ�ѯ�����
--�����ˣ��ܺ���
--ʱ�䣺2011-03-18
-- History:
-- 1.2011-07-06 �����ο���Ϣ ����־
--=========================================
ALTER procedure [dbo].[proc_Inquire_InsertInquire]
(
	@CompanyId int,--ר�߹�˾���
    @RouteId int,--��·���
    @RouteName nvarchar(255),--��·����
    @CustomerId int,--���Ź�˾���
    @CustomerName nvarchar(255),--���Ź�˾����
    @ContactTel nvarchar(255),-- ��ϵ�˵绰
    @ContactName nvarchar(255),--��ϵ������
    @AdultNumber int, --������
    @ChildNumber int, --��ͯ��
    @LeaveDate datetime, --Ԥ�Ƴ�������
    @SpecialClaim nvarchar(max), --����Ҫ��˵��
    @XingCheng nvarchar(max), --�г�Ҫ�� XML:<ROOT><XingCheng QuoteId="ѯ�۱��" QuotePlan="�г�Ҫ��" PlanAccessoryName="�г̸�������" PlanAccessory="�г̸���"></ROOT>
	@ASK NVARCHAR(MAX),--����Ҫ�� XML:<ROOT><QuoteAskInfo QuoteId="ѯ�۱��" ItemType="��Ŀ����" ConcreteAsk="����Ҫ��" /></ROOT>
    @QuoteState tinyint, --ѯ��״̬
    @Remark varchar(max), ---��ע
    @Result INT OUTPUT --������� ��ֵ���ɹ� ��ֵ��0��ʧ��
	,@TravellerDisplayType TINYINT--�ο���Ϣ��������
	,@TravellerFilePath NVARCHAR(255)=NULL--�ο���Ϣ����·��
	,@Travellers NVARCHAR(MAX)=NULL--�ο���Ϣ���� XML:<ROOT><Info TravellerId="�οͱ��" TravellerName="�ο�����" CertificateType="֤������" CertificateCode="֤������" Gender="�Ա�" TravellerType="�ο�����" Telephone="�绰" SpecialServiceName="�ط���Ŀ" SpecialServiceDetail="�ط�����" SpecialServiceIsAdd="�ط���/��" SpecialServiceFee="�ط�����" /></ROOT>
)
as
begin
	DECLARE @hdoc INT
	DECLARE @errorcount INT
    DECLARE @Id int
 	SET @Result=0
	SET @errorcount=0
    SET @Id=0
	begin tran
	begin
--==========================����ѯ�ۻ�����Ϣ==========================--
		insert into tbl_CustomerQuote
		(
			CompanyId,RouteId,RouteName,CustomerId,CustomerName,LeaveDate,
			ContactTel,ContactName,AdultNumber,ChildNumber,SpecialClaim,Remark,QuoteState
			,TravellerDisplayType,TravellerFilePath
		)
		values
		(
			@CompanyId,@RouteId,@RouteName,@CustomerId,@CustomerName,@LeaveDate,
			@ContactTel,@ContactName,@AdultNumber,@ChildNumber,@SpecialClaim,@Remark,@QuoteState
			,@TravellerDisplayType,@TravellerFilePath
		)
		SET @errorcount=@@error+@errorcount
		SELECT @Id=@@IDENTITY
		
--==========================�����г�Ҫ��================================----
       if @Id<>0 and @XingCheng is not null and len(@XingCheng)>0
           begin
	       EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@XingCheng
	       INSERT INTO [tbl_CustomerQuoteClaim]([QuoteId],[QuotePlan],[PlanAccessoryName],[PlanAccessory])
	       SELECT @Id,[QuotePlan],[PlanAccessoryName],[PlanAccessory]
	       FROM OPENXML(@hdoc,N'/ROOT/XingCheng')
	       WITH([QuotePlan] NVARCHAR(MAX),[PlanAccessoryName] NVARCHAR(255),[PlanAccessory] NVARCHAR(255))
	       SET @errorcount=@errorcount+@@ERROR
	       EXECUTE sp_xml_removedocument @hdoc
         end
--==========================�������Ҫ��================================----
	     if @Id<>0 and @ASK is not null and len(@ASK)>0
         begin
	       EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@ASK
	       INSERT INTO [tbl_CustomerQuoteAsk]([QuoteId],[ItemType],[ConcreteAsk])
	       SELECT @Id,[ItemType],[ConcreteAsk]
	       FROM OPENXML(@hdoc,N'/ROOT/QuoteAskInfo')
	       WITH([ItemType] INT,[ConcreteAsk] NVARCHAR(MAX))
	       SET @errorcount=@errorcount+@@ERROR
	       EXECUTE sp_xml_removedocument @hdoc
         end
--==========================�������Ҫ�����================================----

	IF(@errorcount=0 AND @Travellers IS NOT NULL AND LEN(@Travellers)>0)--д���ο���Ϣ
	BEGIN
		EXEC sp_xml_preparedocument @hdoc OUTPUT,@Travellers
		INSERT INTO [tbl_CustomerQuoteTraveller]([TravellerId],[QuoteId],[TravellerName],[CertificateType],[CertificateCode]
			,[Gender],[TravellerType],[Telephone],[CreateTime])
		SELECT A.TravellerId,@Id,A.TravellerName,A.CertificateType,A.CertificateCode
			,A.Gender,A.TravellerType,A.Telephone,GETDATE()
		FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TravellerId CHAR(36),TravellerName NVARCHAR(255),CertificateType TINYINT,CertificateCode NVARCHAR(255),Gender TINYINT,TravellerType TINYINT,Telephone NVARCHAR(255)) AS A
		SET @errorcount=@errorcount+@@ERROR

		IF(@errorcount=0)
		BEGIN
			INSERT INTO [tbl_CustomerQuoteTravellerSpecialService]([TravellerId],[ServiceName],[ServiceDetail],[IsAdd],[Fee],[IssueTime])
			SELECT A.TravellerId,A.SpecialServiceName,A.SpecialServiceDetail,A.SpecialServiceIsAdd,A.SpecialServiceFee,GETDATE()
			FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TravellerId CHAR(36),SpecialServiceName NVARCHAR(255),SpecialServiceDetail NVARCHAR(255),SpecialServiceIsAdd TINYINT,SpecialServiceFee MONEY) AS A
			SET @errorcount=@errorcount+@@ERROR
		END

		EXEC sp_xml_removedocument @hdoc
	END

	if 	@errorcount<>0
	begin	
        rollback tran
        return @Result
	end
 end
commit tran
set @Result=1
return @Result
end
GO


--=========================================
--���������Ŷ�ѯ�۸���
--�����ˣ��ܺ���
--ʱ�䣺2011-03-18
-- History:
-- 1.2011-07-06 �����ο���Ϣ ����־
--=========================================
ALTER procedure [dbo].[proc_Inquire_UpdateInquire]
(
    @Id int,--ѯ�۱��۱��
	@CompanyId int,--ר�߹�˾���
    @RouteId int,--��·���
    @RouteName nvarchar(255),--��·����
    @CustomerId int,--���Ź�˾���
    @CustomerName nvarchar(255),--���Ź�˾����
    @ContactTel nvarchar(255),-- ��ϵ�˵绰
    @ContactName nvarchar(255),--��ϵ������
    @LeaveDate datetime, --Ԥ�Ƴ�������
    @AdultNumber int, --������
    @ChildNumber int, --��ͯ��
    @SpecialClaim nvarchar(max), --����Ҫ��˵��
    @Remark varchar(max), ---��ע
    @QuoteState tinyint,  --ѯ��״̬ δ���� = 0,�Ѵ��� = 1,�ѳɹ� = 2
    @XingCheng nvarchar(max), --�г�Ҫ�� XML:<ROOT><XingCheng QuoteId="ѯ�۱��" QuotePlan="�г�Ҫ��" PlanAccessoryName="�г̸�������" PlanAccessory="�г̸���"></ROOT>
	@QuoteInfo NVARCHAR(MAX),--�۸���� XML:<ROOT><QuoteInfo QuoteId="ѯ�۱��" ItemId="��Ŀ����" Reception="�Ӵ���׼" LocalQuote="�ؽӱ���" MyQuote="���籨��" SumMoney="�ϼƽ��"/></ROOT>
	@ASK NVARCHAR(MAX),--����Ҫ�� XML:<ROOT><QuoteAskInfo QuoteId="ѯ�۱��" ItemType="��Ŀ����" ConcreteAsk="����Ҫ��" /></ROOT>
    @Result INT OUTPUT --������� ��ֵ���ɹ� ��ֵ��0��ʧ��
	,@TravellerDisplayType TINYINT--�ο���Ϣ��������
	,@TravellerFilePath NVARCHAR(255)=NULL--�ο���Ϣ����·��
	,@Travellers NVARCHAR(MAX)=NULL--�ο���Ϣ���� XML:<ROOT><Info TravellerId="�οͱ��" TravellerName="�ο�����" CertificateType="֤������" CertificateCode="֤������" Gender="�Ա�" TravellerType="�ο�����" Telephone="�绰" SpecialServiceName="�ط���Ŀ" SpecialServiceDetail="�ط�����" SpecialServiceIsAdd="�ط���/��" SpecialServiceFee="�ط�����" /></ROOT>
)
as
begin
	DECLARE @hdoc INT
	DECLARE @errorcount INT
 	SET @Result=0
	SET @errorcount=0
    begin tran
    IF EXISTS(SELECT * FROM [tbl_CustomerQuote] WHERE Id=@Id AND CustomerId=@CustomerId)
	begin
--==========================����ѯ�ۻ�����Ϣ==========================--
		update tbl_CustomerQuote
		set 
         RouteName=@RouteName,
--         CustomerId=@CustomerId,
--         CustomerName=@CustomerName,
		 ContactTel=@ContactTel,
         ContactName=@ContactName,
         AdultNumber=@AdultNumber,
         ChildNumber=@ChildNumber,
         SpecialClaim=@SpecialClaim,
         LeaveDate=@LeaveDate,
         Remark=@Remark,
         QuoteState=@QuoteState
			,TravellerDisplayType=@TravellerDisplayType
			,TravellerFilePath=@TravellerFilePath
        where Id=@Id
		SET @errorcount=@@error+@errorcount
		
--==========================�����г�Ҫ��================================----
       if @Id<>0 and @XingCheng is not null and len(@XingCheng)>0
         begin
           DELETE FROM [tbl_CustomerQuoteClaim] WHERE [QuoteId]=@Id
	       EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@XingCheng
	       INSERT INTO [tbl_CustomerQuoteClaim]([QuoteId],[QuotePlan],[PlanAccessoryName],[PlanAccessory])
	       SELECT @Id,[QuotePlan],[PlanAccessoryName],[PlanAccessory]
	       FROM OPENXML(@hdoc,'/ROOT/XingCheng')
	       WITH([QuotePlan] NVARCHAR(MAX),[PlanAccessoryName] NVARCHAR(255),[PlanAccessory] NVARCHAR(255))
	       SET @errorcount=@errorcount+@@ERROR
	       EXECUTE sp_xml_removedocument @hdoc
         end
--==========================����۸����,��ɾ�ٲ�================================----
       DELETE FROM [tbl_CustomerQuotePrice] WHERE [QuoteId]=@Id
       SET @errorcount=@errorcount+@@ERROR
       if @errorcount=0 and @QuoteInfo is not null and len(@QuoteInfo)>0
         begin
	       EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@QuoteInfo
	       INSERT INTO [tbl_CustomerQuotePrice]([QuoteId],[ItemId],[Reception],[LocalQuote],[MyQuote])
	       SELECT @Id,[ItemId],[Reception],[LocalQuote],[MyQuote]
	       FROM OPENXML(@hdoc,'/ROOT/QuoteInfo')
	       WITH([ItemId] INT,[Reception] NVARCHAR(MAX),[LocalQuote] MONEY,[MyQuote] MONEY)
	       SET @errorcount=@errorcount+@@ERROR
	       EXECUTE sp_xml_removedocument @hdoc
         end

--==========================�������Ҫ��================================----
	     if @Id<>0 and @ASK is not null and len(@ASK)>0
         begin
           DELETE FROM [tbl_CustomerQuoteAsk] WHERE [QuoteId]=@Id
	       EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@ASK
	       INSERT INTO [tbl_CustomerQuoteAsk]([QuoteId],[ItemType],[ConcreteAsk])
	       SELECT @Id,[ItemType],[ConcreteAsk]
	       FROM OPENXML(@hdoc,'/ROOT/QuoteAskInfo')
	       WITH([ItemType] INT,[ConcreteAsk] NVARCHAR(MAX))
	       SET @errorcount=@errorcount+@@ERROR
	       EXECUTE sp_xml_removedocument @hdoc
         end
--==========================�������Ҫ�����================================----

	IF(@errorcount=0)--�ο���Ϣ
	BEGIN
		DELETE FROM [tbl_CustomerQuoteTravellerSpecialService] WHERE TravellerId IN(SELECT [TravellerId] FROM [tbl_CustomerQuoteTraveller] WHERE [QuoteId]=@Id)
		DELETE FROM [tbl_CustomerQuoteTraveller] WHERE [QuoteId]=@Id
		SET @errorcount=@errorcount+@@ERROR
		
		IF(@errorcount=0 AND @Travellers IS NOT NULL AND LEN(@Travellers)>0)--д���ο���Ϣ
		BEGIN
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@Travellers
			INSERT INTO [tbl_CustomerQuoteTraveller]([TravellerId],[QuoteId],[TravellerName],[CertificateType],[CertificateCode]
				,[Gender],[TravellerType],[Telephone],[CreateTime])
			SELECT A.TravellerId,@Id,A.TravellerName,A.CertificateType,A.CertificateCode
				,A.Gender,A.TravellerType,A.Telephone,GETDATE()
			FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TravellerId CHAR(36),TravellerName NVARCHAR(255),CertificateType TINYINT,CertificateCode NVARCHAR(255),Gender TINYINT,TravellerType TINYINT,Telephone NVARCHAR(255)) AS A
			SET @errorcount=@errorcount+@@ERROR
			
			IF(@errorcount=0)
			BEGIN
				INSERT INTO [tbl_CustomerQuoteTravellerSpecialService]([TravellerId],[ServiceName],[ServiceDetail],[IsAdd],[Fee],[IssueTime])
				SELECT A.TravellerId,A.SpecialServiceName,A.SpecialServiceDetail,A.SpecialServiceIsAdd,A.SpecialServiceFee,GETDATE()
				FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TravellerId CHAR(36),SpecialServiceName NVARCHAR(255),SpecialServiceDetail NVARCHAR(255),SpecialServiceIsAdd TINYINT,SpecialServiceFee MONEY) AS A
				SET @errorcount=@errorcount+@@ERROR
			END

			EXEC sp_xml_removedocument @hdoc
		END
	END

	if 	@errorcount<>0
	begin	
		rollback tran
        return @Result
	end
 end
    commit tran
    set @Result=1
	return @Result

end
GO

--=========================================
--������ר�߶˸��±���
--�����ˣ��ܺ���
--ʱ�䣺2011-03-20
--History:
--1.2011-04-18 �����ʽ����
--2.2011-04-18 ����־ ���ӵؽӱ������������ۼ����籨�����������ۣ��ϼƽ��
--3.2011-07-06 �����ο���Ϣ ����־
--=========================================
ALTER procedure [dbo].[proc_Inquire_UpdateQuote]
(
	@Id int, --ѯ�۱��
	@CompanyId int,--ר�߹�˾���
	@RouteId int,--��·���
	@RouteName nvarchar(255),--��·����
	@ContactTel nvarchar(255),-- ��ϵ�˵绰
	@ContactName nvarchar(255),--��ϵ������
	@AdultNumber int, --������
	@ChildNumber int, --��ͯ��
	@TicketAgio decimal,--��Ʊ�ۿ�
	@SpecialClaim nvarchar(max), --����Ҫ��˵��
    @LeaveDate datetime, --Ԥ�Ƴ�������
	@XingCheng nvarchar(max), --�г�Ҫ�� XML:<ROOT><XingCheng QuoteId="ѯ�۱��" QuotePlan="�г�Ҫ��" PlanAccessoryName="�г̸�������" PlanAccessory="�г̸���"></ROOT>
	@ASK NVARCHAR(MAX),--����Ҫ�� XML:<ROOT><QuoteAskInfo QuoteId="ѯ�۱��" ItemType="��Ŀ����" ConcreteAsk="����Ҫ��" /></ROOT>
	@QuoteInfo NVARCHAR(MAX),--�۸���� XML:<ROOT><QuoteInfo QuoteId="ѯ�۱��" ItemId="��Ŀ����" Reception="�Ӵ���׼" LocalQuote="�ؽӱ���" MyQuote="���籨��" LocalPeopleNumber="�ؽӱ�������" LocalUnitPrice="�ؽӱ��۵���" SelfPeopleNumber="���籨������" SelfUnitPrice="���籨�۵���" /></ROOT>
	@Remark varchar(max), ---��ע    
	@QuoteState int, --ѯ��״̬
	@Result INT OUTPUT, --������� ��ֵ���ɹ� ��ֵ��0��ʧ��
	@TotalAmount MONEY --�ϼƽ��
	,@TravellerDisplayType TINYINT--�ο���Ϣ��������
	,@TravellerFilePath NVARCHAR(255)=NULL--�ο���Ϣ����·��
	,@Travellers NVARCHAR(MAX)=NULL--�ο���Ϣ���� XML:<ROOT><Info TravellerId="�οͱ��" TravellerName="�ο�����" CertificateType="֤������" CertificateCode="֤������" Gender="�Ա�" TravellerType="�ο�����" Telephone="�绰" SpecialServiceName="�ط���Ŀ" SpecialServiceDetail="�ط�����" SpecialServiceIsAdd="�ط���/��" SpecialServiceFee="�ط�����" /></ROOT>
)
as
begin
	DECLARE @hdoc INT
	DECLARE @errorcount INT
 	SET @Result=0
	SET @errorcount=0
	begin tran Inquire_InsertQuote

	update tbl_CustomerQuote set RouteId=@RouteId,RouteName=@RouteName,ContactTel=@ContactTel,ContactName=@ContactName,
		AdultNumber=@AdultNumber,ChildNumber=@ChildNumber,QuoteState=@QuoteState,TicketAgio=@TicketAgio,SpecialClaim=@SpecialClaim,
		Remark=@Remark,TotalAmount=@TotalAmount,LeaveDate=@LeaveDate
		,TravellerDisplayType=@TravellerDisplayType,TravellerFilePath=@TravellerFilePath
    where Id=@Id
	SET @errorcount=@@error+@errorcount
	
	IF(@errorcount=0 and @XingCheng is not null and len(@XingCheng)>0)--�����г�Ҫ��
	BEGIN
		DELETE FROM [tbl_CustomerQuoteClaim] WHERE [QuoteId]=@Id
		SET @errorcount=@errorcount+@@ERROR
		if(@errorcount=0)
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@XingCheng
		   INSERT INTO [tbl_CustomerQuoteClaim]([QuoteId],[QuotePlan],[PlanAccessoryName],[PlanAccessory])
		   SELECT @Id,[QuotePlan],[PlanAccessoryName],[PlanAccessory]
		   FROM OPENXML(@hdoc,'/ROOT/XingCheng')
		   WITH([QuotePlan] NVARCHAR(MAX),[PlanAccessoryName] NVARCHAR(255),[PlanAccessory] NVARCHAR(255))
		   SET @errorcount=@errorcount+@@ERROR
		   EXECUTE sp_xml_removedocument @hdoc
		END
	END

	IF(@errorcount=0 and @QuoteInfo is not null and len(@QuoteInfo)>0)--����۸����,��ɾ�ٲ�
	BEGIN
		DELETE FROM [tbl_CustomerQuotePrice] WHERE [QuoteId]=@Id
		SET @errorcount=@errorcount+@@ERROR
		IF(@errorcount=0)
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@QuoteInfo
			INSERT INTO [tbl_CustomerQuotePrice]([QuoteId],[ItemId],[Reception],[LocalQuote],[MyQuote],[LocalPeopleNumber],[LocalUnitPrice],[SelfPeopleNumber],[SelfUnitPrice])
			SELECT @Id,[ItemId],[Reception],[LocalQuote],[MyQuote],[LocalPeopleNumber],[LocalUnitPrice],[SelfPeopleNumber],[SelfUnitPrice]
			FROM OPENXML(@hdoc,'/ROOT/QuoteInfo')
			WITH([ItemId] INT,[Reception] NVARCHAR(MAX),[LocalQuote] MONEY,[MyQuote] MONEY,[LocalPeopleNumber] INT,[LocalUnitPrice] MONEY,[SelfPeopleNumber] INT,[SelfUnitPrice] MONEY)
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
	END

	IF(@errorcount=0 and @ASK is not null and len(@ASK)>0)--=�������Ҫ��
	BEGIN
		DELETE FROM [tbl_CustomerQuoteTravellerSpecialService] WHERE TravellerId IN(SELECT [TravellerId] FROM [tbl_CustomerQuoteTraveller] WHERE [QuoteId]=@Id)
		DELETE FROM [tbl_CustomerQuoteTraveller] WHERE [QuoteId]=@Id
		SET @errorcount=@errorcount+@@ERROR
		IF(@errorcount=0)
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@ASK
			INSERT INTO [tbl_CustomerQuoteAsk]([QuoteId],[ItemType],[ConcreteAsk])
			SELECT @Id,[ItemType],[ConcreteAsk]
			FROM OPENXML(@hdoc,'/ROOT/QuoteAskInfo')
			WITH([ItemType] INT,[ConcreteAsk] NVARCHAR(MAX))
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
	END

	IF(@errorcount=0)--�ο���Ϣ
	BEGIN
		DELETE FROM [tbl_CustomerQuoteTraveller] WHERE [QuoteId]=@Id
		SET @errorcount=@errorcount+@@ERROR
		
		IF(@errorcount=0 AND @Travellers IS NOT NULL AND LEN(@Travellers)>0)--д���ο���Ϣ
		BEGIN
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@Travellers
			INSERT INTO [tbl_CustomerQuoteTraveller]([TravellerId],[QuoteId],[TravellerName],[CertificateType],[CertificateCode]
				,[Gender],[TravellerType],[Telephone],[CreateTime])
			SELECT A.TravellerId,@Id,A.TravellerName,A.CertificateType,A.CertificateCode
				,A.Gender,A.TravellerType,A.Telephone,GETDATE()
			FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TravellerId CHAR(36),TravellerName NVARCHAR(255),CertificateType TINYINT,CertificateCode NVARCHAR(255),Gender TINYINT,TravellerType TINYINT,Telephone NVARCHAR(255)) AS A
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				INSERT INTO [tbl_CustomerQuoteTravellerSpecialService]([TravellerId],[ServiceName],[ServiceDetail],[IsAdd],[Fee],[IssueTime])
				SELECT A.TravellerId,A.SpecialServiceName,A.SpecialServiceDetail,A.SpecialServiceIsAdd,A.SpecialServiceFee,GETDATE()
				FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TravellerId CHAR(36),SpecialServiceName NVARCHAR(255),SpecialServiceDetail NVARCHAR(255),SpecialServiceIsAdd TINYINT,SpecialServiceFee MONEY) AS A
				SET @errorcount=@errorcount+@@ERROR
			END
			EXEC sp_xml_removedocument @hdoc
		END
	END

	IF(@errorcount>0)
	BEGIN
		ROLLBACK TRAN
		SET @Result=0
		RETURN -1
	END

	COMMIT TRAN
	SET @Result=1	
end
GO

-- =============================================
-- Author:		<�ܺ���>
-- Create date: <2011-3-18>
-- Description:	<��ȡѯ�۱�����Ϣ>
-- =============================================
ALTER PROCEDURE [dbo].[proc_Tour_GetInquireQuote] 
@Id int, ---�������
@CompanyId int, ---ר�߹�˾���
@CustomerId int, ---���Ź�˾���
@IsZhuTuan varchar(1) ---�Ƿ�����
AS
---�������Ŷ�
IF(@IsZhuTuan=0)
begin
IF EXISTS(SELECT 1 FROM tbl_CustomerQuote WHERE Id=@Id AND CompanyId=@CompanyId)
BEGIN
---ѯ�۱��ۻ�����Ϣ
SELECT * FROM tbl_CustomerQuote WHERE Id=@Id
---����ѯ���г�Ҫ��
SELECT * FROM tbl_CustomerQuoteClaim WHERE [QuoteId]=@Id
---����ѯ�ۿͻ�Ҫ��
SELECT * FROM tbl_CustomerQuoteAsk WHERE [QuoteId]=@Id
---����ѯ�ۼ۸����
SELECT * FROM tbl_CustomerQuotePrice WHERE [QuoteId]=@Id
--�ο���Ϣ
SELECT A.*,B.ServiceName,B.ServiceDetail,B.IsAdd,B.Fee FROM [tbl_CustomerQuoteTraveller] AS A LEFT OUTER JOIN [tbl_CustomerQuoteTravellerSpecialService] AS B ON A.TravellerId=B.TravellerId WHERE A.[QuoteId]=@Id
END
end
----�����Ŷ�
else if(@IsZhuTuan=1)
begin
IF EXISTS(SELECT 1 FROM tbl_CustomerQuote WHERE Id=@Id AND CustomerId=@CustomerId)
BEGIN
---ѯ�۱��ۻ�����Ϣ
SELECT * FROM tbl_CustomerQuote WHERE Id=@Id
---����ѯ���г�Ҫ��
SELECT * FROM tbl_CustomerQuoteClaim WHERE [QuoteId]=@Id
---����ѯ�ۿͻ�Ҫ��
SELECT * FROM tbl_CustomerQuoteAsk WHERE [QuoteId]=@Id
---����ѯ�ۼ۸����
SELECT * FROM tbl_CustomerQuotePrice WHERE [QuoteId]=@Id
--�ο���Ϣ
SELECT A.*,B.ServiceName,B.ServiceDetail,B.IsAdd,B.Fee FROM [tbl_CustomerQuoteTraveller] AS A LEFT OUTER JOIN [tbl_CustomerQuoteTravellerSpecialService] AS B ON A.TravellerId=B.TravellerId WHERE A.[QuoteId]=@Id
END
end
GO

-- =============================================
-- Author:		����־
-- Create date: 2011-07-06
-- Description:	ͬ������ѯ���ο���Ϣ���ŶӼƻ�����
-- =============================================
ALTER PROCEDURE [dbo].[proc_SyncQuoteTravellerToTourTeamOrder]
	@QuoteId INT--ѯ�۱��
	,@TourId CHAR(36)--�ŶӼƻ����
	,@Result INT OUTPUT --���
AS
BEGIN
	SET @Result=0
	IF NOT EXISTS(SELECT 1 FROM [tbl_CustomerQuote] WHERE [Id]=@QuoteId) RETURN
	IF NOT EXISTS(SELECT 1 FROM [tbl_Tour] WHERE [TourId]=@TourId AND [TourType]=1) RETURN
	DECLARE @OrderId CHAR(36)--�ŶӼƻ��������
	DECLARE @TravellerDisplayType TINYINT--�ο���Ϣ��������
	DECLARE @TravellerFilePath NVARCHAR(255)--�ο���Ϣ����
	DECLARE @CompanyId INT--��˾���

	SELECT @OrderId=[Id] FROM [tbl_TourOrder] WHERE [TourId]=@TourId
	SELECT @TravellerDisplayType=@TravellerDisplayType,@TravellerFilePath=@TravellerFilePath,@CompanyId=[CompanyId] FROM [tbl_CustomerQuote] WHERE [Id]=@QuoteId
	
	UPDATE [tbl_TourOrder] SET [CustomerDisplayType]=@TravellerDisplayType,[CustomerFilePath]=@TravellerFilePath WHERE [Id]=@OrderId
	
	INSERT INTO [tbl_TourOrderCustomer]([ID],[OrderId],[CompanyID],[VisitorName],[CradType]
		,[CradNumber],[Sex],[VisitorType],[ContactTel],[IssueTime]
		,[TicketStatus],[CustomerStatus])
	SELECT A.TravellerId,@OrderId,@CompanyId,A.TravellerName,A.CertificateType
		,A.CertificateCode,A.Gender,A.TravellerType,A.Telephone,GETDATE()
		,0,0
	FROM [tbl_CustomerQuoteTraveller] AS A
	WHERE A.[QuoteId]=@QuoteId

	INSERT INTO [tbl_CustomerSpecialService]([CustormerId],[ProjectName],[ServiceDetail],[IsAdd],[Fee],[IssueTime])
	SELECT A.TravellerId,A.[ServiceName],A.[ServiceDetail],A.[IsAdd],A.[Fee],GETDATE()
	FROM [tbl_CustomerQuoteTravellerSpecialService] AS A WHERE A.[TravellerId] IN(SELECT B.[TravellerId] FROM [tbl_CustomerQuoteTraveller] AS B WHERE B.[QuoteId]=@QuoteId )
	
	SET @Result=1
	RETURN	
END
GO

--(20110620)�ͱ���.doc end
--������־�Ѹ��� 20110711














--(20110629)�ű���.doc start

/*
--�ƻ��������ֶ� 2011-07-05
ALTER TABLE dbo.tbl_Tour ADD
	NumberCR int NOT NULL CONSTRAINT DF_tbl_Tour_NumberCR DEFAULT 0,
	NumberET int NOT NULL CONSTRAINT DF_tbl_Tour_NumberET DEFAULT 0,
	NumberQP int NOT NULL CONSTRAINT DF_tbl_Tour_NumberQP DEFAULT 0,
	UnitAmountCR money NOT NULL CONSTRAINT DF_tbl_Tour_UnitAmountCR DEFAULT 0,
	UnitAmountET money NOT NULL CONSTRAINT DF_tbl_Tour_UnitAmountET DEFAULT 0,
	UnitAmountQP money NOT NULL CONSTRAINT DF_tbl_Tour_UnitAmountQP DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'������'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'NumberCR'
GO
DECLARE @v sql_variant 
SET @v = N'��ͯ��'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'NumberET'
GO
DECLARE @v sql_variant 
SET @v = N'ȫ����'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'NumberQP'
GO
DECLARE @v sql_variant 
SET @v = N'���˵��ۺϼ�'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'UnitAmountCR'
GO
DECLARE @v sql_variant 
SET @v = N'��ͯ���ۺϼ�'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'UnitAmountET'
GO
DECLARE @v sql_variant 
SET @v = N'ȫ�㵥�ۺϼ�'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'UnitAmountQP'
GO

--�����������ֶ� 2011-07-05
ALTER TABLE dbo.tbl_TourOrder ADD
	NumberQP int NOT NULL CONSTRAINT DF_tbl_TourOrder_NumberQP DEFAULT 0,
	UnitAmountQP money NOT NULL CONSTRAINT DF_tbl_TourOrder_UnitAmountQP DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'ȫ����'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOrder', N'COLUMN', N'NumberQP'
GO
DECLARE @v sql_variant 
SET @v = N'ȫ�㵥�ۺϼ�'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOrder', N'COLUMN', N'UnitAmountQP'
GO

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tbl_TourTeamUnit') and o.name = 'FK_TBL_TOURTEAMUNIT_REFERENCE_TBL_TOUR')
alter table tbl_TourTeamUnit
   drop constraint FK_TBL_TOURTEAMUNIT_REFERENCE_TBL_TOUR
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tbl_TourTeamUnit')
            and   type = 'U')
   drop table tbl_TourTeamUnit
go


--create table tbl_TourTeamUnit 2011-07-05
create table tbl_TourTeamUnit (
	TourId               char(36)             not null,
	NumberCR int NOT NULL CONSTRAINT DF_tbl_Tour_NumberCR DEFAULT 0,
	NumberET int NOT NULL CONSTRAINT DF_tbl_Tour_NumberET DEFAULT 0,
	NumberQP int NOT NULL CONSTRAINT DF_tbl_Tour_NumberQP DEFAULT 0,
	UnitAmountCR money NOT NULL CONSTRAINT DF_tbl_Tour_UnitAmountCR DEFAULT 0,
	UnitAmountET money NOT NULL CONSTRAINT DF_tbl_Tour_UnitAmountET DEFAULT 0,
	UnitAmountQP money NOT NULL CONSTRAINT DF_tbl_Tour_UnitAmountQP DEFAULT 0,
	constraint PK_TBL_TOURTEAMUNIT primary key (TourId)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '�ƻ����',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'TourId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '������',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'NumberCR'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '��ͯ��',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'NumberET'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'ȫ����',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'NumberQP'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '���˵��ۺϼ�',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'UnitAmountCR'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '��ͯ���ۺϼ�',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'UnitAmountET'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'ȫ�㵥�ۺϼ�',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'UnitAmountQP'
go

alter table tbl_TourTeamUnit
   add constraint FK_TBL_TOURTEAMUNIT_REFERENCE_TBL_TOUR foreign key (TourId)
      references tbl_Tour (TourId)
go
*/

--create table tbl_TourTeamUnit �ŶӼƻ��ƻ��������۸���Ϣ�� 2011-07-05 
create table tbl_TourTeamUnit (
   TourId               char(36)             not null,
   NumberCR             int                  not null default 0,
   NumberET             int                  not null default 0,
   NumberQP             int                  not null default 0,
   UnitAmountCR         money                not null default 0,
   UnitAmountET         money                not null default 0,
   UnitAmountQP         money                not null default 0,
   constraint PK_TBL_TOURTEAMUNIT primary key (TourId)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '�ƻ����',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'TourId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '������',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'NumberCR'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '��ͯ��',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'NumberET'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'ȫ����',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'NumberQP'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '���˵��ۺϼ�',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'UnitAmountCR'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '��ͯ���ۺϼ�',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'UnitAmountET'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'ȫ�㵥�ۺϼ�',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'UnitAmountQP'
go

alter table tbl_TourTeamUnit
   add constraint FK_TBL_TOURTEAMUNIT_REFERENCE_TBL_TOUR foreign key (TourId)
      references tbl_Tour (TourId)
go


GO
-- =============================================
-- Author:		����־
-- Create date: 2011-01-21
-- Description:	д��ɢƴ�ƻ����ŶӼƻ������������Ϣ
-- History:
-- 1.2011-04-18 ���ӵؽӱ������������ۼ����籨������������
-- 2.2011-06-08 �ŶӼƻ��������ӷ�Ӷ
-- 3.2011-07-11 �ŶӼƻ����������ͼ۸���Ϣ��ɾ��SelfUnitPriceAmount
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
			,[CommissionPrice])
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
			,@CommissionPrice
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
			,[CustomerDisplayType],[CustomerFilePath],[LeaguePepoleNum],[TourDays])
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
			,@CustomerDisplayType,@CustomerFilePath,0,@TourDays
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


GO
-- =============================================
-- Author:		����־
-- Create date: 2011-01-23
-- Description:	����ɢƴ�ƻ����ŶӼƻ������������Ϣ
-- History:
-- 1.2011-07-11 �ŶӼƻ����������ͼ۸���Ϣ��ɾ��SelfUnitPriceAmount
-- =============================================
ALTER PROCEDURE [dbo].[proc_Tour_UpdateTourInfo]
	@TourId CHAR(36),--�Ŷӱ��
	@TourCode NVARCHAR(50),--�ź�
	@TourType TINYINT,--�Ŷ�����
	@TourDays INT,--�Ŷ�����
	@LDate DATETIME,--��������
	@AreaId INT,--��·������
	@RouteName NVARCHAR(255),--��·����
	@PlanPeopleNumber INT,--�ƻ�����
	@ReleaseType CHAR(1),--��������
	@OperatorId INT,--�����˱��
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
	@TourPrice NVARCHAR(MAX)=NULL,--ɢƴ�ƻ�������Ϣ XML:<ROOT><Info AdultPrice="MONEY���˼�" ChildrenPrice="MONEY��ͯ��" StandardId="���۱�׼���" LevelId="�ͻ��ȼ����" /></ROOT>
	@SinglePrivate NVARCHAR(MAX)=NULL,--�������ר����Ϣ XML:<ROOT><Info OrderId="�������" SellerId="����Ա���" SellerName="����Ա����" TotalAmount="�ϼ�������" GrossProfit="�Ŷ�ë��" BuyerCId="�ͻ���λ���" BuyerCName="�ͻ���λ����" ContactName="��ϵ��" ContactTelephone="��ϵ�绰" ContactMobile="��ϵ�ֻ�" TotalOutAmount="�ϼ�֧�����" /></ROOT>
	@PlansSingle NVARCHAR(MAX)=NULL,--�������Ӧ�̰�����Ϣ XML:<ROOT><Info PlanId="���ű��" ServiceType="��Ŀ����" SupplierId="��Ӧ�̱��" SupplierName="��Ӧ������" Arrange="���尲��" Amount="�������" Remark="��ע" /></ROOT>
	@TeamPrivate NVARCHAR(MAX)=NULL,--�ŶӼƻ�ר����Ϣ XML:<ROOT><Info OrderId="�������" SellerId="����Ա���" SellerName="����Ա����" TotalAmount="�ϼƽ��" BuyerCId="�ͻ���λ���" BuyerCName="�ͻ���λ����" ContactName="��ϵ��" ContactTelephone="��ϵ�绰" ContactMobile="��ϵ�ֻ�" /></ROOT>
	@CustomerDisplayType TINYINT=0,--�ο���Ϣ��������
	@Customers NVARCHAR(MAX)=NULL,--�����ο���Ϣ XML:<ROOT><Info TravelerId="�οͱ��" TravelerName="�ο�����" IdType="֤������" IdNo="֤����" Gender="�Ա�" TravelerType="����(����/��ͯ)" ContactTel="��ϵ�绰" TF_XiangMu="�ط���Ŀ" TF_NeiRong="�ط�����" TF_ZengJian="1:���� 0:����" TF_FeiYong="�ط�����" /></ROOT>	
	@CustomerFilePath NVARCHAR(255)=NULL,--�����ο͸���
	@Result INT OUTPUT,--������� ��ֵ���ɹ� ��ֵ��0��ʧ��
	@OrderId CHAR(36) OUTPUT,--������ţ�������ŶӼƻ����������
	@CoordinatorId INT=0,--�Ƶ�Ա���
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
	SET @OrderId=''

	DECLARE @CompanyName NVARCHAR(255)--��˾����
	DECLARE @OperatorName NVARCHAR(255)--����������
	DECLARE @OperatorTelephone NVARCHAR(255)--�����˵绰
	DECLARE @OperatorMobile NVARCHAR(255)--�������ֻ�
	DECLARE @OperatorFax NVARCHAR(255)--�����˴���
	DECLARE @DepartmentId INT--����Ա���ű��	
	DECLARE @CRouteId INT--�޸�ǰ��·���	

	SELECT @CompanyName=[CompanyName] FROM [tbl_CompanyInfo] WHERE Id=@CompanyId
	SELECT @OperatorName=[ContactName],@OperatorTelephone=[ContactTel],@OperatorMobile=[ContactMobile],@OperatorFax=[ContactFax] FROM [tbl_COmpanyUser] WHERE Id=@OperatorId
	SELECT @DepartmentId=[DepartId] FROM [tbl_CompanyUser] WHERE Id=@OperatorId	
	SELECT @CRouteId=[RouteId] FROM [tbl_Tour] WHERE [TourId]=@TourId

	BEGIN TRAN
	IF(@TourType=0)--ɢƴ�ƻ�
	BEGIN
		--�ƻ���Ϣ
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
		IF(@errorcount=0)--������Ϣ
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
		IF(@errorcount=0)--�ŶӲ���Ա�Ƶ�Ա��Ϣ
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
		IF(@errorcount=0 AND @LocalAgencys IS NOT NULL)--�ؽ�����Ϣ
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
		IF(@errorcount=0 AND @Attachs IS NOT NULL)--������Ϣ
		BEGIN
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@Attachs
			--ɾ���ĸ���
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
		IF(@errorcount=0 AND @ReleaseType='0')--��׼�����г���Ϣ
		BEGIN
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@Plans
			--ɾ���ĸ���
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
		IF(@errorcount=0 AND @ReleaseType='1')--���ٷ����г���Ϣ��
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
		IF(@errorcount=0 AND @ReleaseType='0' AND @Service IS NOT NULL)--��׼����������Ŀ
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
		IF(@errorcount=0 AND @ReleaseType='0' AND @NormalPrivate IS NOT NULL)--��׼����������Ŀ��
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
		IF(@errorcount=0 AND @SentPeoples IS NOT NULL)--��������Ϣ
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
		IF(@errorcount=0 AND @TourCityId IS NOT NULL AND @TourCityId>0)--���۳���
		BEGIN
			DELETE FROM [tbl_TourCity] WHERE [TourId]=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				INSERT INTO [tbl_TourCity]([TourId],[CityId]) VALUES(@TourId,@TourCityId)
				SET @errorcount=@errorcount+@@ERROR
			END
		END
		IF(@errorcount=0)--�Ƿ����
		BEGIN
			UPDATE [tbl_Tour] SET IsLeave='1' WHERE TourId=@TourId AND LeaveDate<=GETDATE()
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0)--�������źŴ���
		BEGIN
			UPDATE [tbl_TourOrder] SET [TourNo]=@TourCode WHERE [TourId]=@TourId
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0)--�Ŷ�״̬ά��
		BEGIN
			--����Ŷ�״̬Ϊ�г�;��
			UPDATE tbl_Tour SET Status=2 WHERE TourType IN(0,1) AND [Status] IN(0,1,3) AND DATEDIFF(DAY,GETDATE(),[LeaveDate])<=0 AND DATEDIFF(DAY,GETDATE(),[RDate])>0 AND TourId=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)--����Ŷ�״̬Ϊ���ű���
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
	END


	IF(@TourType=1)--�ŶӼƻ�
	BEGIN
		DECLARE @SellerId INT--����Ա���
		DECLARE @SellerName NVARCHAR(255)--����Ա����
		--�Ŷ���Ϣ
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
		IF(@errorcount=0)--�ŶӲ���Ա�Ƶ�Ա��Ϣ
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
		IF(@errorcount=0 AND @LocalAgencys IS NOT NULL)--�ؽ�����Ϣ
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
		IF(@errorcount=0 AND @Attachs IS NOT NULL)--������Ϣ
		BEGIN
			EXEC sp_xml_preparedocument @hdoc OUTPUT,@Attachs
			--ɾ���ĸ���
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
		IF(@errorcount=0 AND @ReleaseType='0')--��׼�����г���Ϣ
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
		IF(@errorcount=0 AND @ReleaseType='1')--���ٷ����г���Ϣ��
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
		IF(@errorcount=0 AND @NormalPrivate IS NOT NULL)--��׼����������Ŀ��
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
		IF(@errorcount=0 AND @Service IS NOT NULL)--�۸����
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
		--����
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
			,[AdultNumber]=@PlanPeopleNumber
		FROM [tbl_TourOrder] AS B,(
			SELECT OrderId,BuyerCId,BuyerCName,ContactName,ContactTelephone,SellerName,SellerId,TotalAmount FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(OrderId CHAR(36),SellerId INT,SellerName NVARCHAR(255),TotalAmount MONEY,BuyerCId INT,BuyerCName NVARCHAR(255),ContactName NVARCHAR(100),ContactTelephone NVARCHAR(50))
		) A
		WHERE B.Id=@OrderId
		SET @errorcount=@errorcount+@@ERROR
		EXEC sp_xml_removedocument @hdoc
		IF(@errorcount=0 AND @SentPeoples IS NOT NULL)--��������Ϣ
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
		IF(@errorcount=0 AND @TourCityId IS NOT NULL AND @TourCityId>0)--���۳���
		BEGIN
			DELETE FROM [tbl_TourCity] WHERE [TourId]=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN
				INSERT INTO [tbl_TourCity]([TourId],[CityId]) VALUES(@TourId,@TourCityId)
				SET @errorcount=@errorcount+@@ERROR
			END
		END
		if @errorcount = 0 and @TourTeamUnit is not null  --�ŶӼƻ����������ͼ۸���Ϣ
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
		IF(@errorcount=0)--�Ƿ����
		BEGIN
			UPDATE [tbl_Tour] SET IsLeave='1' WHERE TourId=@TourId AND LeaveDate<=GETDATE()
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0)--�Ŷ�״̬ά��
		BEGIN
			--����Ŷ�״̬Ϊ�г�;��
			UPDATE tbl_Tour SET Status=2 WHERE TourType IN(0,1) AND [Status] IN(0,1,3) AND DATEDIFF(DAY,GETDATE(),[LeaveDate])<=0 AND DATEDIFF(DAY,GETDATE(),[RDate])>0 AND TourId=@TourId
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)--����Ŷ�״̬Ϊ���ű���
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
	END


	IF(@TourType=2)--�������
	BEGIN
		--�Ŷ���Ϣ
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
		IF(@errorcount=0 AND @Service IS NOT NULL)--�۸����
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
		--����
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
		--��Ӧ�̰��Ŵ���
		EXEC sp_xml_preparedocument @hdoc OUTPUT,@PlansSingle		
		IF(@errorcount=0 AND @PlansSingle IS NOT NULL)--ɾ��Ҫɾ���Ĺ�Ӧ�̰���
		BEGIN
			--ɾ����Ӧ�Ĺ�Ӧ�̰���
			DELETE FROM [tbl_PlanSingle] WHERE [TourId]=@TourId AND [PlanId] NOT IN(SELECT A.PlanId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(PlanId CHAR(36)) AS A)
			SET @errorcount=@errorcount+@@ERROR
			--ɾ��ͳ�Ʒ���֧�����˵���Ӧ�İ���
			DELETE FROM [tbl_StatAllOut] WHERE [TourId]=@TourId AND [ItemType]=4 AND [ItemId] NOT IN(SELECT A.PlanId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(PlanId CHAR(36)) AS A)
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0 AND @PlansSingle IS NOT NULL)--�޸�Ҫ���µĹ�Ӧ�̰���
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
		IF(@errorcount=0 AND @PlansSingle IS NOT NULL)--д��Ҫ�����Ĺ�Ӧ�̰���
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
		IF(@errorcount=0 AND @PlansSingle IS NOT NULL)--д�����Ĺ�Ӧ�̰���֧��
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
		IF(@errorcount=0 AND @PlansSingle IS NOT NULL)--����֧���ϼƽ����Ϣ
		BEGIN
			UPDATE [tbl_PlanSingle] SET [TotalAmount]=[Amount]+[AddAmount]-[ReduceAmount] WHERE [TourId]=@TourId AND [PlanId] IN(SELECT A.PlanId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(PlanId CHAR(36)) AS A)
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)
			BEGIN 
				UPDATE [tbl_StatAllOut] SET [TotalAmount]=[Amount]+[AddAmount]-[ReduceAmount] WHERE [TourId]=@TourId AND [ItemType]=4 AND [ItemId] IN(SELECT A.PlanId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(PlanId CHAR(36)) AS A)
			END
		END
		EXEC sp_xml_removedocument @hdoc
		--�οʹ���
		EXEC sp_xml_preparedocument @hdoc OUTPUT,@Customers
		IF(@errorcount=0)--ɾ��Ҫɾ�����ο���Ϣ
		BEGIN
			DELETE FROM [tbl_TourOrderCustomer] WHERE [OrderId]=@OrderId AND [Id] NOT IN(SELECT A.TravelerId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TravelerId CHAR(36)) AS A)
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0 AND @Customers IS NOT NULL)--����Ҫ�޸ĵ��ο���Ϣ
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
		IF(@errorcount=0 AND @Customers IS NOT NULL)--д��Ҫ�������ο���Ϣ
		BEGIN			
			INSERT INTO [tbl_TourOrderCustomer]([ID],[OrderId],[CompanyID],[VisitorName],[CradType]
				,[CradNumber],[Sex],[VisitorType],[ContactTel],[IssueTime]
				,[CustomerStatus])
			SELECT A.TravelerId,@OrderId,@CompanyId,A.TravelerName,A.IdType
				,A.IdNo,A.Gender,A.TravelerType,A.ContactTel,GETDATE()
				,0 FROM OPENXML(@hdoc,'/ROOT/Info')
			WITH(TravelerId CHAR(36),TravelerName NVARCHAR(100),IdType TINYINT,IdNo NVARCHAR(255),Gender TINYINT,TravelerType TINYINT,ContactTel NVARCHAR(100)) AS A
			WHERE NOT EXISTS(SELECT 1 FROM [tbl_TourOrderCustomer] WHERE OrderId=@OrderId AND [Id]=A.TravelerId)
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0 AND @Customers IS NOT NULL)--�ط���Ϣ
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
	
	IF(@errorcount=0 AND @CRouteId>0)--��·������ά��
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

GO

GO

-- =============================================
-- Author:		luofx
-- Create date: 2011-01-24
-- Description:	�޸Ķ���
-- History:
-- 1.����������ϵ����Ϣ����Ӷ��Ϣ ����־ 2011-06-08
-- 2.�����޸��ŶӼƻ�����ͬ���޸��ŶӼƻ���Ϣ������������֤����ȥ������������
-- =============================================
ALTER PROCEDURE [dbo].[proc_TourOrder_Update]	
	@OrderID CHAR(36),                --�������
	@TourId CHAR(36),	             --�Ŷӱ��
	@OrderState TINYINT,              --����״̬
	@BuyCompanyID INT,	             --����˾���
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
	SET @ErrorCount=0
	SET @Result=1
BEGIN	
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
		DECLARE @CommissionType TINYINT --��Ӷ����
		DECLARE @CommissionIsPaid CHAR(1)--��Ӷ����Ƿ�֧��
		
		IF((@BuyerContactName IS NULL OR LEN(@BuyerContactName)<1) AND @BuyerContactId>0)
		BEGIN
			SELECT @BuyerContactName=[Name] FROM [tbl_CustomerContactInfo] WHERE [Id]=@BuyerContactId
		END
		--���¼������С��:�µĶ������+���ӷ���+���ٷ���
		SELECT @FinanceSum=@SumPrice+FinanceAddExpense-FinanceRedExpense,@CommissionType=[CommissionType],@CommissionIsPaid=[CommissionStatus] FROM [tbl_TourOrder] a WHERE a.[ID]=@OrderID				
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
	IF(@ErrorCount>0)
	BEGIN
		SET @Result=0
		ROLLBACK TRANSACTION TourOrder_Update
	END
	COMMIT TRANSACTION TourOrder_Update
	--�����Ƿ��տ�����
	UPDATE [tbl_Tour] SET [Status]=1  WHERE [Status]=0 AND PlanPeopleNumber=(SELECT ISNULL(SUM(PeopleNumber-LeaguePepoleNum),0) FROM tbl_TourOrder WHERE TourId=(SELECT TourId FROM tbl_TourOrder WHERE [id]=@OrderID) AND IsDelete='0' AND OrderState NOT IN(3,4))						
	RETURN  @Result
END

GO


GO
-- =============================================
-- Author:		luofx
-- Create date: 2011-01-24
-- Description:	��������
-- History:
-- 1.����������ϵ����Ϣ����Ӷ��Ϣ ����־ 2011-06-08
-- 2.����������֤����ȥ������������
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
				,CommissionType,CommissionPrice)
			VALUES(@OrderID,@OrderNO,@TourId,@AreaId,@RouteId,@RouteName,@TourNo,@TourClassId
				,@LeaveDate,@TourDays,@LeaveTraffic,@ReturnTraffic,@OrderType,@OrderState,@BuyCompanyID
				,@BuyCompanyName,@ContactName,@ContactTel,@ContactMobile,@ContactFax,@SalerName
				,@SalerId,@OperatorName, @OperatorID,@PriceStandId,@PersonalPrice
				,@ChildPrice,@MarketPrice,@AdultNumber,@ChildNumber,@MarketNumber
				,@PeopleNumber,@OtherPrice,@SaveSeatDate,@OperatorContent
				,@SpecialContent,@SumPrice,@SellCompanyName,@SellCompanyId,@LastDate
				,@LastOperatorID,getdate(),@ViewOperatorId,@CustomerDisplayType,@CustomerFilePath
				,@CustomerLevId,@SumPrice,getdate(),@BuyerContactId,@BuyerContactName
				,@CommissionType,@CommissionPrice)
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
	UPDATE [tbl_Tour] SET [Status]=1  WHERE [Status]=0  AND IsDelete='0' AND PlanPeopleNumber=(SELECT ISNULL(SUM(PeopleNumber-LeaguePepoleNum),0) FROM tbl_TourOrder WHERE TourId=@TourId  AND IsDelete='0' AND OrderState NOT IN(3,4))						
	RETURN  @Result
END

GO


GO
-- =============================================
-- Author:luofx
-- Create date: 2011-01-18
-- Description:	�ο�����:�������ż�¼���޸��οͱ������״̬
-- =============================================
ALTER PROCEDURE [dbo].[proc_TourOrderCustomer_AddLeague] 
	@CustormerId CHAR(36),       --�οͱ��
	@OperatorName NVARCHAR(250),      --����������
	@OperatorID INT,		--�����˱��
	@RefundReason NVARCHAR(1000),      --�˿�ԭ��
	@RefundAmount DECIMAL(10,4),       --�˿���
	@AddOrEdit int,		--���������޸ģ�0��������1���޸ģ�				
	@Result INT OUTPUT,  -- ���ز���	
	@ReturnTourId char(36) OUTPUT,	--�Ŷ�Id�����ز���
	@ReturnOrderId char(36) OUTPUT	--����Id�����ز���
AS
	DECLARE @ErrorCount int         --��֤����
	declare @TourId char(36)		--�Ŷ�Id
	DECLARE @OrderId CHAR(36)       --�������
	declare @TourType tinyint		--�Ŷ�����
	declare @CustomerType tinyint	--�ο����ͣ����ˡ���ͯ��
	declare @OldLeaguePepoleNum int --����������
	declare @AdultPrice money		--���˵���
	declare @ChildPrice money		--��ͯ����
	declare @OrderPrice money		--�������ǲ���С�ƣ�
	SET @ErrorCount=0
BEGIN
	BEGIN Transaction TourOrderCustomer_AddLeague

	SELECT TOP 1 @OrderId=OrderId,@CustomerType = VisitorType FROM tbl_TourOrderCustomer WHERE ID=@CustormerId
	SELECT @TourId = TourId,@OldLeaguePepoleNum = LeaguePepoleNum,@TourType = TourClassId,@AdultPrice = PersonalPrice,@ChildPrice = ChildPrice,@OrderPrice = SumPrice FROM  [tbl_TourOrder] WHERE ID=@OrderId

	if @AddOrEdit = 0 --��������
	begin
		INSERT INTO [tbl_CustomerLeague] ([CustormerId],[OperatorName],[OperatorID],[RefundReason],
				[IssueTime],[RefundAmount])VALUES (@CustormerId,@OperatorName,@OperatorID,
					@RefundReason,getdate(),@RefundAmount)
		SET @ErrorCount=@ErrorCount+@@ERROR

		UPDATE [tbl_TourOrderCustomer] SET CustomerStatus=1 WHERE [ID]=@CustormerId
		SET @ErrorCount=@ErrorCount+@@ERROR

		--��������������
		update [tbl_TourOrder] set 
			LeaguePepoleNum = @OldLeaguePepoleNum + 1 
		where ID = @OrderId
		SET @ErrorCount=@ErrorCount+@@ERROR
	end
	else if @AddOrEdit = 1 --�޸�����
	begin
		UPDATE tbl_CustomerLeague SET 
			OperatorName = @OperatorName
			,OperatorID = @OperatorID
			,RefundReason = @RefundReason
			,RefundAmount = @RefundAmount 
		WHERE CustormerId = @CustormerId
	end

	--�Ŷ�����ö�٣� ɢƴ�ƻ� = 0,�ŶӼƻ� = 1,������� = 2
	--�ο�����ö�٣� δ֪ = 0,���� = 1,��ͯ = 2

	if @TourType = 0 --ɢƴ�ƻ�
	begin
		set @OrderPrice = @OrderPrice + @RefundAmount - (case when @CustomerType = 1 then @AdultPrice when @CustomerType = 2 then @ChildPrice else 0 end)
		update [tbl_TourOrder] set 
			[SumPrice] = @OrderPrice
			,FinanceSum = @OrderPrice + FinanceAddExpense - FinanceRedExpense
		where ID = @OrderId
		SET @ErrorCount=@ErrorCount+@@ERROR
	end
	else if @TourType = 1 --�ŶӼƻ�
	begin
		select @AdultPrice = UnitAmountCR,@ChildPrice = UnitAmountET from tbl_TourTeamUnit where TourId = @TourId
		set @AdultPrice = isnull(@AdultPrice,0)
		set @ChildPrice = isnull(@ChildPrice,0)
		if (@CustomerType = 1 and @AdultPrice > 0) or (@CustomerType = 2 and @ChildPrice > 0)
		begin
			set @OrderPrice = @OrderPrice + @RefundAmount - (case when @CustomerType = 1 then @AdultPrice when @CustomerType = 2 then @ChildPrice else 0 end)
			update [tbl_TourOrder] set 
				[SumPrice] = @OrderPrice
				,FinanceSum = @OrderPrice + FinanceAddExpense - FinanceRedExpense
			where ID = @OrderId
			SET @ErrorCount=@ErrorCount+@@ERROR
		end
	end
--	else if @TourType = 2  --�������
--	begin
--		
--	end

	--�Ƿ�ع�
	IF(@ErrorCount>0)
	BEGIN
		ROLLBACK TRANSACTION TourOrderCustomer_AddLeague
		SET @Result=0
	END
	else
	begin
		COMMIT TRANSACTION TourOrderCustomer_AddLeague
		SET @Result=1
	end
	set @ReturnTourId = @TourId
	set @ReturnOrderId = @OrderId
END

Go

/*
�ŶӼƻ����㵥�ݣ�ö��ֵ 35   �Ŷӽ�����ϸ�� ��webmaster���޸�
����ϵͳ��/print/wh/TourTeamSettlePrint.aspx    
����ϵͳ��/print/bbl/TeamSettledPrint.aspx
INSERT INTO [tbl_CompanySetting]([Id],[FieldName],[FieldValue])VALUES(1,'PrintDocument_35','/print/bbl/TeamSettledPrint.aspx')
INSERT INTO [tbl_CompanySetting]([Id],[FieldName],[FieldValue])VALUES(2,'PrintDocument_35','/print/wh/TourTeamSettlePrint.aspx')
INSERT INTO [tbl_CompanySetting]([Id],[FieldName],[FieldValue])VALUES(3,'PrintDocument_35','/print/bbl/TeamSettledPrint.aspx')
*/


--(20110629)�ű���.doc end

--������־�Ѹ���


--�ƻ���Ϣ��������·״̬
ALTER TABLE dbo.tbl_Tour ADD
	RouteStatus tinyint NOT NULL CONSTRAINT DF_tbl_Tour_RouteStatus DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'��·״̬'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'RouteStatus'
GO

ALTER TABLE dbo.tbl_Tour ADD
	HandStatus tinyint NOT NULL CONSTRAINT DF_tbl_Tour_HandStatus DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'�ֶ����õļƻ�״̬'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'HandStatus'
GO

--������־�Ѹ���


--�ӷ�����֧���������տ�򸶿�״̬�ֶ�
GO
ALTER TABLE dbo.tbl_TourOtherCost ADD
	Status char(1) NOT NULL CONSTRAINT DF_tbl_TourOtherCost_Status DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'�տ�򸶿�״̬ 0��δ�ջ�δ�� 1�����ջ��Ѹ�'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOtherCost', N'COLUMN', N'Status'
GO

--ԭ���ӷ�����֧��״̬������������ջ��Ѹ�
UPDATE [tbl_TourOtherCost] SET [Status]='1'
GO

GO
-- =============================================
-- Author:		<Author,,�����>
-- Create date: <Create Date,2011-02-14,>
-- Description:	<Description,��Ʊ,>
-- History:
-- 1.2011-04-21 ����־ ���̵��� 
-- 2.2011-08-09 ���ĳ� ��Ʊ����ӷ�����Ĭ������
-- =============================================
ALTER PROCEDURE [dbo].[proc_Ticket_Return]
(	 
	@TicketListId int,--��Ʊ�����б���
	@RefundAmount money,--�˻ؽ��
	@Status tinyint,--��Ʊ״̬
	@Remark varchar(250),--��Ʊ��֪
	@OperatorId INT,--������
	@Result int output,--������� 1��success 	
	@TourId CHAR(36) OUTPUT--��Ʊ��������ļƻ����
)
AS
BEGIN
	DECLARE @errorcount INT
	DECLARE @TravellerRefundId CHAR(36)--�ο���Ʊ���

	SET @Result=0
	SET @TourId=''
	SET @errorcount=0
	SELECT @TravellerRefundId=[RefundId] FROM [tbl_PlanTicket] WHERE [Id]=@TicketListId AND [TicketType]=3

	IF(@TravellerRefundId IS NULL) RETURN -1

	BEGIN TRAN
	--�����ο���Ʊ��״̬���˻ؽ��
	UPDATE [tbl_CustomerRefund] SET [IsRefund]=@Status,RefundAmount = @RefundAmount WHERE [Id]=@TravellerRefundId
	SET @errorcount=@errorcount+@@ERROR
	--���Ļ�Ʊ�����б�״̬
	UPDATE [tbl_Planticket] SET [State]=@Status WHERE Id=@TicketListId
	SET @errorcount=@errorcount+@@ERROR

	IF(@Status=4)--��Ʊ���
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM [tbl_TourOtherCost] WHERE [ItemId] =@travellerRefundId AND [ItemType]=1 AND [CostType]=0)--д�ӷ�����
		BEGIN
			DECLARE @OtherInComeId CHAR(36)--�ӷ��������	
			DECLARE @CompanyId INT--��˾���
			DECLARE @AreaId INT--��·������
			DECLARE @TourType TINYINT--�ƻ�����
			DECLARE @TravellerId CHAR(36)--��Ʊ�οͱ��
			DECLARE @OrderId CHAR(36)--��Ʊ�ο����ڶ������
			SET @OtherInComeId=NEWID()
			
			SELECT @TravellerId=[CustormerId] FROM [tbl_CustomerRefund] WHERE [Id]=@TravellerRefundId
			SELECT @OrderId =[OrderId] FROM [tbl_TourOrderCustomer] WHERE [Id]=@TravellerId
			SELECT @TourId=[TourId] FROM [tbl_TourOrder] WHERE [Id]=@OrderId
			SELECT @CompanyId=[CompanyId],@TourType=[TourType] FROM [tbl_Tour] WHERE [TourId]=@TourId
			--д�ӷ�����
			INSERT INTO [tbl_TourOtherCost]([Id],[CompanyId],[TourId],[CostType],[CustromCName]
				,[CustromCId],[ProceedItem],[Proceed],[IncreaseCost],[ReduceCost]
				,[SumCost],[Remark],[OperatorId],[CreateTime],[PayTime]
				,[Payee],[PayType],[ItemType],[ItemId],[Status])
			VALUES(@OtherInComeId,@CompanyId,@TourId,0,''
				,0,'��Ʊ��Ʊ��',@RefundAmount,0,0
				,@RefundAmount,'',@OperatorId,GETDATE(),GETDATE()
				,'',1,1,@TravellerRefundId,'1')
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)--д������ϸ
			BEGIN
				INSERT INTO [tbl_StatAllIncome]([CompanyId],[TourId],[AreaId],[TourType],[ItemId]
					,[ItemType],[Amount],[AddAmount],[ReduceAmount],[TotalAmount]
					,[OperatorId],[DepartmentId],[CreateTime],[HasCheckAmount],[NotCheckAmount]
					,[IsDelete])
				SELECT @CompanyId,@TourId,0,@TourType,@OtherInComeId
					,2,@RefundAmount,0,0,@RefundAmount
					,@OperatorId,(SELECT [DepartId] FROM [tbl_CompanyUser] WHERE [Id]=@OperatorId),GETDATE(),0,0
					,'0'
				SET @errorcount=@errorcount+@@ERROR
			END
		END
		ELSE--�����ӷ�����
		BEGIN
			UPDATE [tbl_TourOtherCost] SET [Proceed]=@RefundAmount,[SumCost]=@RefundAmount+[IncreaseCost]-[ReduceCost]
			WHERE [ItemId] =@TravellerRefundId AND [ItemType]=1 AND [CostType]=0
			SET @errorcount=@errorcount+@@ERROR
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

GO
--����SQL�Ѹ����������� 2011-08-12
