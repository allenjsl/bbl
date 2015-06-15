--(20110620)巴比来.doc start
GO
--供应商基本信息表增加许可证号字段 汪奇志 2011-06-30
ALTER TABLE dbo.tbl_CompanySupplier ADD
	LicenseKey nvarchar(50) NULL
GO
DECLARE @v sql_variant 
SET @v = N'许可证号'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_CompanySupplier', N'COLUMN', N'LicenseKey'
GO

-- =============================================
-- Author:		周文超
-- Create date: 2011-03-28
-- Description:	新增供应商信息（适用于地接社信息修改）
-- Error：0参数错误
-- Error：-1新增过程中发生错误
-- Success：1新增成功
-- =============================================
ALTER PROCEDURE  [dbo].[proc_CompanySupplier_Add]
(
	@ProvinceId int = 0		--省份编号
    ,@ProvinceName nvarchar(255) = ''		--省份名称
    ,@CityId int = 0		--城市编号
    ,@CityName nvarchar(255) = ''		--城市名称
    ,@UnitName nvarchar(255) = ''		--单位名称
    ,@SupplierType tinyint = 0		--地接、机票
    ,@UnitAddress nvarchar(500) = ''		--地址
    ,@Commission money = 0		--返佣
    ,@AgreementFile nvarchar(255) = ''		--合作协议
    ,@TradeNum int = 0		--交易次数
    ,@UnitPolicy nvarchar(max) = ''		--政策
    ,@Remark nvarchar(max) = ''		--备注
    ,@CompanyId int = 0		--公司编号
    ,@OperatorId int = 0		--操作员编号
	,@SupplierContactXML nvarchar(max) = null  --联系人信息XML
	,@ErrorValue  int = 0 output   --错误代码
	,@LicenseKey NVARCHAR(50)--许可证号
)
as
BEGIN
	declare @ErrorCount int    --错误计数器
	declare @IsUserExist int   --用户名是否存在
	declare @NewContactId int  --新联系人Id
	declare @NewUserId int   --新用户Id
	declare @SupplierId int  --新供应商Id
	DECLARE @hdoc int   --解析XML变量
	--定义游标变量
	declare @ContactName nvarchar(50),		--联系人姓名
			@JobTitle nvarchar(255),		--职务
			@ContactFax nvarchar(100),		--传真
			@ContactTel nvarchar(100),		--联系电话
			@ContactMobile nvarchar(100),		--联系人手机
			@QQ nvarchar(100),		--QQ
			@Email nvarchar(100),		--Email
			@UserId int	,	--用户编号
			@UserName nvarchar(100), --用户名
			@Password varchar(50),  --明文密码
			@MD5Password varchar(50)   --MD5密码

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
	--验证错误
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
			--验证错误
			SET @ErrorCount = @ErrorCount + @@ERROR
			--添加账户信息
			if @UserName is not null and len(@UserName) > 0 and @Password is not null and len(@Password) > 0
			begin
				set @IsUserExist = 0 ---判断用户是否存在
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
					--验证错误
					SET @ErrorCount = @ErrorCount + @@ERROR  
					if @NewUserId > 0 and @NewContactId > 0  --更新联系人信息中的UserId
					begin
						update [tbl_SupplierContact] set UserId = @NewUserId where Id = @NewContactId
						--验证错误
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
-- Author:		周文超
-- Create date: 2011-03-28
-- Description:	修改供应商信息（适用于地接社信息修改）
-- Error：0参数错误
-- Error：-1修改过程中发生错误
-- Success：1修改成功
-- =============================================
ALTER PROCEDURE  [dbo].[proc_CompanySupplier_Update]
(
	@Id int = 0,   --供应商编号
	@ProvinceId int = 0 ,  --省份编号
	@ProvinceName nvarchar(255) = '',  --  省份名称
	@CityId int = 0,  --城市编号
	@CityName nvarchar(255) = '',  --城市名称
	@UnitName nvarchar(255) = '',  --单位名称
	@SupplierType tinyint = 0,  --地接、机票
	@UnitAddress nvarchar(500) = '',  --地址
	@Commission money = 0,  --返佣
	@AgreementFile nvarchar(255) = '',  --合作协议
	@TradeNum int = 0,  --交易次数
	@UnitPolicy nvarchar(max) = '',  --政策
	@Remark nvarchar(max) = '',  --备注
	@CompanyId int = 0,  --公司编号
	@IsDelete char(1) = '0',  --是否删除
	@SupplierContactXML nvarchar(max) = null,  --联系人信息XML
	@ErrorValue  int = 0 output,   --错误代码
	@LicenseKey NVARCHAR(50)--许可证号
)
as
BEGIN
	declare @ErrorCount int    --错误计数器
	declare @IsUserExist int   --用户名是否存在
	declare @NewContactId int  --新联系人Id
	declare @NewUserId int   --新用户Id
	DECLARE @hdoc int   --解析XML变量
	--定义游标变量
	declare @ContactId int,		--联系人编号
			@ContactName nvarchar(50),		--联系人姓名
			@JobTitle nvarchar(255),		--职务
			@ContactFax nvarchar(100),		--传真
			@ContactTel nvarchar(100),		--联系电话
			@ContactMobile nvarchar(100),		--联系人手机
			@QQ nvarchar(100),		--QQ
			@Email nvarchar(100),		--Email
			@UserId int	,	--用户编号
			@UserName nvarchar(100), --用户名
			@Password varchar(50),  --明文密码
			@MD5Password varchar(50)   --MD5密码

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
	--验证错误
	SET @ErrorCount = @ErrorCount + @@ERROR
	
	if @SupplierContactXML is not null and len(@SupplierContactXML) > 1
	begin

		if @CompanyId is null or @CompanyId <= 0  --为公司Id赋值
		begin
			select @CompanyId = CompanyId from tbl_CompanySupplier where Id = @Id
		end
		
		set @SupplierContactXML = replace(replace(@SupplierContactXML,char(13),''),char(10),'')
		EXEC sp_xml_preparedocument @hdoc OUTPUT, @SupplierContactXML

		--先删除要删除的联系人的帐户信息
		delete from tbl_CompanyUser where CompanyId = @CompanyId and TourCompanyId = @Id and Id in (select UserId from tbl_SupplierContact where  CompanyId = @CompanyId and [SupplierId] = @Id and Id not in (select Id from openxml(@hdoc,'/ROOT/CustomerInfo')with(Id int)))
		--删除待删除的联系人
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

			if @ContactId > 0   --修改
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
				--验证错误
				SET @ErrorCount = @ErrorCount + @@ERROR
				--为用户Id赋值
				select @UserId from [tbl_SupplierContact] WHERE Id = @ContactId
				if @UserId > 0  --修改用户信息
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
					--验证错误
					SET @ErrorCount = @ErrorCount + @@ERROR
				end
				else
				begin
					--添加账户信息
					if @UserName is not null and len(@UserName) > 0 and @Password is not null and len(@Password) > 0
					begin
						set @IsUserExist = 0 ---判断用户是否存在
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
							--验证错误
							SET @ErrorCount = @ErrorCount + @@ERROR  
							if @NewUserId > 0 and @ContactId > 0  --更新联系人信息中的UserId
							begin
								update [tbl_SupplierContact] set UserId = @NewUserId where Id = @ContactId
								--验证错误
								SET @ErrorCount = @ErrorCount + @@ERROR
							end 
						end 
					end
				end
			end
			else   --新增
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
				--验证错误
				SET @ErrorCount = @ErrorCount + @@ERROR
				--添加账户信息
				if @UserName is not null and len(@UserName) > 0 and @Password is not null and len(@Password) > 0
				begin
					set @IsUserExist = 0 ---判断用户是否存在
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
						--验证错误
						SET @ErrorCount = @ErrorCount + @@ERROR  
						if @NewUserId > 0 and @NewContactId > 0  --更新联系人信息中的UserId
						begin
							update [tbl_SupplierContact] set UserId = @NewUserId where Id = @NewContactId
							--验证错误
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
-- Author:		汪奇志
-- Create date: 2011-07-04
-- Description:	批量登记付款
-- @ExpenseType:(0:单项服务供应商安排，1:地接，2:票务，255:所有)
-- =============================================
CREATE PROCEDURE proc_BatchRegisterExpense
	@PaymentTime DATETIME--支付时间
	,@PaymentType TINYINT--支付类型
	,@PayerId INT--付款人编号
	,@Payer NVARCHAR(50)--付款人姓名
	,@Remark NVARCHAR(255)--备注
	,@ExpenseType TINYINT=255--支出类型，为255时关联所有支出
	,@OperatorId INT--操作员编号
	,@CompanyId INT--公司编号
	,@TourIdsXML NVARCHAR(MAX)--计划编号集合XML：<ROOT><Info TourId="计划编号" /></ROOT>
	,@Result INT OUTPUT--操作结果
AS
BEGIN
	SET @Result=0

	DECLARE @hdoc INT
	DECLARE @tmptbl TABLE(TourId CHAR(36))
	
	EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@TourIdsXML
	INSERT INTO @tmptbl(TourId) SELECT A.TourId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TourId CHAR(36)) AS A
	EXECUTE sp_xml_removedocument @hdoc

	IF(@ExpenseType=0 OR @ExpenseType=255)--单项支出
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

	IF(@ExpenseType=1 OR @ExpenseType=255)--地接支出
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

	IF(@ExpenseType=2 OR @ExpenseType=255)--机票支出
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
-- Author:		汪奇志
-- Create date: 2011-07-04
-- Description:	取消订单
-- =============================================
CREATE PROCEDURE proc_TourOrder_CancelOrder
	@OrderId CHAR(36)--订单编号
	,@Result INT OUTPUT--结果 1:成功
AS
BEGIN
	SET @Result=0

	DECLARE @TourId CHAR(36)--计划编号
	DECLARE @RouteId INT--线路编号
	SELECT @TourId=[TourId] FROM [tbl_TourOrder] WHERE [Id]=@OrderId
	SELECT @RouteId=[RouteId] FROM [tbl_Tour] WHERE [TourId]=@TourId
	
	--更新订单状态为不受理
	UPDATE [tbl_TourOrder] SET [OrderState]=4 WHERE [Id]=@OrderId
	--删除收入
	UPDATE [tbl_StatAllIncome] SET [IsDelete]='1' WHERE [ItemId]=@OrderId AND [ItemType]=1	
	IF(@RouteId>0)--线路库收客数维护
	BEGIN
		UPDATE [tbl_Route] SET VisitorCount=ISNULL((SELECT SUM(AdultNumber+ChildNumber) FROM [tbl_TourOrder] WHERE [OrderState] IN(1,2,5) AND RouteId=@RouteId AND IsDelete='0'),0) WHERE [Id]=@RouteId
	END
	--维护团队虚拟实收数=订单总人数(不含留住过期、取消的订单)
	UPDATE [tbl_Tour] SET [VirtualPeopleNumber]=(SELECT ISNULL(SUM([PeopleNumber]-[LeaguePepoleNum]),0) FROM [tbl_TourOrder] WHERE [TourId]=@TourId  AND [IsDelete]='0' AND [OrderState] IN(1,2,5))  WHERE [TourId]=@TourId
	--维护计划总收入
	UPDATE [tbl_Tour] SET [TotalIncome]=ISNULL((SELECT SUM([FinanceSum]) FROM [tbl_TourOrder] AS B WHERE B.[TourId]=@TourId AND B.[IsDelete]='0' AND B.[OrderState] NOT IN(3,4)),0) WHERE TourId=@TourId
	--团队状态维护 仅客满状态到正在收客状态
	UPDATE [tbl_Tour] SET [Status]=0 FROM [tbl_Tour] AS A 
	WHERE A.[Status]=1 AND A.TourId=@TourId AND A.PlanPeopleNumber>(SELECT ISNULL(SUM(B.PeopleNumber-B.LeaguePepoleNum),0) FROM tbl_TourOrder AS B WHERE B.TourId=@TourId AND B.[OrderState] NOT IN(3,4) AND IsDelete='0')
	--维护计划结清状态
	DECLARE @CalculationTourSettleStatusResult INT
	EXEC [proc_Utility_CalculationTourSettleStatus] @TourId=@TourId,@Result=@CalculationTourSettleStatusResult OUTPUT

	SET @Result=1
	RETURN @Result
END
GO

--客户单位信息表增加logo字段 2011-07-05
ALTER TABLE dbo.tbl_Customer ADD
	FilePathLogo nvarchar(255) NULL
GO
DECLARE @v sql_variant 
SET @v = N'Logo'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Customer', N'COLUMN', N'FilePathLogo'
GO


--create table 组团询价游客信息表 2011-07-06
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
   '游客编号',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTraveller', 'column', 'TravellerId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '询价编号',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTraveller', 'column', 'QuoteId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '游客姓名',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTraveller', 'column', 'TravellerName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '证件类型',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTraveller', 'column', 'CertificateType'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '证件编号',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTraveller', 'column', 'CertificateCode'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '游客性别',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTraveller', 'column', 'Gender'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '游客类型 (成人、儿童)',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTraveller', 'column', 'TravellerType'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '联系电话',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTraveller', 'column', 'Telephone'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '添加时间',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTraveller', 'column', 'CreateTime'
go

alter table tbl_CustomerQuoteTraveller
   add constraint FK_TBL_CUSTOMERQUOTETRA_REFERENCE_TBL_CUSTOMERQUOTE foreign key (QuoteId)
      references tbl_CustomerQuote (Id)
go

--组团询价信息表增加字段 2011-07-06
ALTER TABLE dbo.tbl_CustomerQuote ADD
	TravellerDisplayType tinyint NOT NULL CONSTRAINT DF_tbl_CustomerQuote_TravellerDisplayType DEFAULT 0,
	TravellerFilePath nvarchar(255) NULL
GO
DECLARE @v sql_variant 
SET @v = N'游客信息体现类型（附件方式、输入方式）'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_CustomerQuote', N'COLUMN', N'TravellerDisplayType'
GO
DECLARE @v sql_variant 
SET @v = N'游客信息附件'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_CustomerQuote', N'COLUMN', N'TravellerFilePath'
GO

--=========================================
--描述：组团端询价添加
--创建人：曹胡生
--时间：2011-03-18
-- History:
-- 1.2011-07-06 增加游客信息 汪奇志
--=========================================
ALTER procedure [dbo].[proc_Inquire_InsertInquire]
(
	@CompanyId int,--专线公司编号
    @RouteId int,--线路编号
    @RouteName nvarchar(255),--线路名称
    @CustomerId int,--组团公司编号
    @CustomerName nvarchar(255),--组团公司名称
    @ContactTel nvarchar(255),-- 联系人电话
    @ContactName nvarchar(255),--联系人姓名
    @AdultNumber int, --成人数
    @ChildNumber int, --儿童数
    @LeaveDate datetime, --预计出团日期
    @SpecialClaim nvarchar(max), --特殊要求说明
    @XingCheng nvarchar(max), --行程要求 XML:<ROOT><XingCheng QuoteId="询价编号" QuotePlan="行程要求" PlanAccessoryName="行程附件名称" PlanAccessory="行程附件"></ROOT>
	@ASK NVARCHAR(MAX),--客人要求 XML:<ROOT><QuoteAskInfo QuoteId="询价编号" ItemType="项目类型" ConcreteAsk="具体要求" /></ROOT>
    @QuoteState tinyint, --询价状态
    @Remark varchar(max), ---备注
    @Result INT OUTPUT --操作结果 正值：成功 负值或0：失败
	,@TravellerDisplayType TINYINT--游客信息体现类型
	,@TravellerFilePath NVARCHAR(255)=NULL--游客信息附件路径
	,@Travellers NVARCHAR(MAX)=NULL--游客信息集合 XML:<ROOT><Info TravellerId="游客编号" TravellerName="游客姓名" CertificateType="证件类型" CertificateCode="证件号码" Gender="性别" TravellerType="游客类型" Telephone="电话" /></ROOT>
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
--==========================插入询价基本信息==========================--
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
		
--==========================插入行程要求================================----
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
--==========================插入客人要求================================----
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
--==========================插入客人要求结束================================----

	IF(@errorcount=0 AND @Travellers IS NOT NULL AND LEN(@Travellers)>0)--写入游客信息
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
--描述：组团端询价更改
--创建人：曹胡生
--时间：2011-03-18
-- History:
-- 1.2011-07-06 增加游客信息 汪奇志
--=========================================
ALTER procedure [dbo].[proc_Inquire_UpdateInquire]
(
    @Id int,--询价报价编号
	@CompanyId int,--专线公司编号
    @RouteId int,--线路编号
    @RouteName nvarchar(255),--线路名称
    @CustomerId int,--组团公司编号
    @CustomerName nvarchar(255),--组团公司名称
    @ContactTel nvarchar(255),-- 联系人电话
    @ContactName nvarchar(255),--联系人姓名
    @LeaveDate datetime, --预计出团日期
    @AdultNumber int, --成人数
    @ChildNumber int, --儿童数
    @SpecialClaim nvarchar(max), --特殊要求说明
    @Remark varchar(max), ---备注
    @QuoteState tinyint,  --询价状态 未处理 = 0,已处理 = 1,已成功 = 2
    @XingCheng nvarchar(max), --行程要求 XML:<ROOT><XingCheng QuoteId="询价编号" QuotePlan="行程要求" PlanAccessoryName="行程附件名称" PlanAccessory="行程附件"></ROOT>
	@QuoteInfo NVARCHAR(MAX),--价格组成 XML:<ROOT><QuoteInfo QuoteId="询价编号" ItemId="项目类型" Reception="接待标准" LocalQuote="地接报价" MyQuote="我社报价" SumMoney="合计金额"/></ROOT>
	@ASK NVARCHAR(MAX),--客人要求 XML:<ROOT><QuoteAskInfo QuoteId="询价编号" ItemType="项目类型" ConcreteAsk="具体要求" /></ROOT>
    @Result INT OUTPUT --操作结果 正值：成功 负值或0：失败
	,@TravellerDisplayType TINYINT--游客信息体现类型
	,@TravellerFilePath NVARCHAR(255)=NULL--游客信息附件路径
	,@Travellers NVARCHAR(MAX)=NULL--游客信息集合 XML:<ROOT><Info TravellerId="游客编号" TravellerName="游客姓名" CertificateType="证件类型" CertificateCode="证件号码" Gender="性别" TravellerType="游客类型" Telephone="电话" /></ROOT>
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
--==========================插入询价基本信息==========================--
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
		
--==========================插入行程要求================================----
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
--==========================插入价格组成,先删再插================================----
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

--==========================插入客人要求================================----
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
--==========================插入客人要求结束================================----

	IF(@errorcount=0)--游客信息
	BEGIN
		DELETE FROM [tbl_CustomerQuoteTraveller] WHERE [QuoteId]=@Id
		SET @errorcount=@errorcount+@@ERROR
		
		IF(@errorcount=0 AND @Travellers IS NOT NULL AND LEN(@Travellers)>0)--写入游客信息
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
--描述：专线端更新报价
--创建人：曹胡生
--时间：2011-03-20
--History:
--1.2011-04-18 代码格式调整
--2.2011-04-18 汪奇志 增加地接报价人数、单价及我社报价人数、单价，合计金额
--3.2011-07-06 增加游客信息 汪奇志
--=========================================
ALTER procedure [dbo].[proc_Inquire_UpdateQuote]
(
	@Id int, --询价编号
	@CompanyId int,--专线公司编号
	@RouteId int,--线路编号
	@RouteName nvarchar(255),--线路名称
	@ContactTel nvarchar(255),-- 联系人电话
	@ContactName nvarchar(255),--联系人姓名
	@AdultNumber int, --成人数
	@ChildNumber int, --儿童数
	@TicketAgio decimal,--机票折扣
	@SpecialClaim nvarchar(max), --特殊要求说明
    @LeaveDate datetime, --预计出团日期
	@XingCheng nvarchar(max), --行程要求 XML:<ROOT><XingCheng QuoteId="询价编号" QuotePlan="行程要求" PlanAccessoryName="行程附件名称" PlanAccessory="行程附件"></ROOT>
	@ASK NVARCHAR(MAX),--客人要求 XML:<ROOT><QuoteAskInfo QuoteId="询价编号" ItemType="项目类型" ConcreteAsk="具体要求" /></ROOT>
	@QuoteInfo NVARCHAR(MAX),--价格组成 XML:<ROOT><QuoteInfo QuoteId="询价编号" ItemId="项目类型" Reception="接待标准" LocalQuote="地接报价" MyQuote="我社报价" LocalPeopleNumber="地接报价人数" LocalUnitPrice="地接报价单价" SelfPeopleNumber="我社报价人数" SelfUnitPrice="我社报价单价" /></ROOT>
	@Remark varchar(max), ---备注    
	@QuoteState int, --询价状态
	@Result INT OUTPUT, --操作结果 正值：成功 负值或0：失败
	@TotalAmount MONEY --合计金额
	,@TravellerDisplayType TINYINT--游客信息体现类型
	,@TravellerFilePath NVARCHAR(255)=NULL--游客信息附件路径
	,@Travellers NVARCHAR(MAX)=NULL--游客信息集合 XML:<ROOT><Info TravellerId="游客编号" TravellerName="游客姓名" CertificateType="证件类型" CertificateCode="证件号码" Gender="性别" TravellerType="游客类型" Telephone="电话" /></ROOT>
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
	
	IF(@errorcount=0 and @XingCheng is not null and len(@XingCheng)>0)--插入行程要求
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

	IF(@errorcount=0 and @QuoteInfo is not null and len(@QuoteInfo)>0)--插入价格组成,先删再插
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

	IF(@errorcount=0 and @ASK is not null and len(@ASK)>0)--=插入客人要求
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

	IF(@errorcount=0)--游客信息
	BEGIN
		DELETE FROM [tbl_CustomerQuoteTraveller] WHERE [QuoteId]=@Id
		SET @errorcount=@errorcount+@@ERROR
		
		IF(@errorcount=0 AND @Travellers IS NOT NULL AND LEN(@Travellers)>0)--写入游客信息
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
-- Author:		<曹胡生>
-- Create date: <2011-3-18>
-- Description:	<获取询价报价信息>
-- =============================================
ALTER PROCEDURE [dbo].[proc_Tour_GetInquireQuote] 
@Id int, ---主键编号
@CompanyId int, ---专线公司编号
@CustomerId int, ---组团公司编号
@IsZhuTuan varchar(1) ---是否组团
AS
---不是组团端
IF(@IsZhuTuan=0)
begin
IF EXISTS(SELECT 1 FROM tbl_CustomerQuote WHERE Id=@Id AND CompanyId=@CompanyId)
BEGIN
---询价报价基本信息
SELECT * FROM tbl_CustomerQuote WHERE Id=@Id
---组团询价行程要求
SELECT * FROM tbl_CustomerQuoteClaim WHERE [QuoteId]=@Id
---组团询价客户要求
SELECT * FROM tbl_CustomerQuoteAsk WHERE [QuoteId]=@Id
---组团询价价格组成
SELECT * FROM tbl_CustomerQuotePrice WHERE [QuoteId]=@Id
--游客信息
SELECT * FROM [tbl_CustomerQuoteTraveller] WHERE [QuoteId]=@Id
END
end
----是组团端
else if(@IsZhuTuan=1)
begin
IF EXISTS(SELECT 1 FROM tbl_CustomerQuote WHERE Id=@Id AND CustomerId=@CustomerId)
BEGIN
---询价报价基本信息
SELECT * FROM tbl_CustomerQuote WHERE Id=@Id
---组团询价行程要求
SELECT * FROM tbl_CustomerQuoteClaim WHERE [QuoteId]=@Id
---组团询价客户要求
SELECT * FROM tbl_CustomerQuoteAsk WHERE [QuoteId]=@Id
---组团询价价格组成
SELECT * FROM tbl_CustomerQuotePrice WHERE [QuoteId]=@Id
--游客信息
SELECT * FROM [tbl_CustomerQuoteTraveller] WHERE [QuoteId]=@Id
END
end
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-07-06
-- Description:	同步组团询价游客信息到团队计划订单
-- =============================================
CREATE PROCEDURE proc_SyncQuoteTravellerToTourTeamOrder
	@QuoteId INT--询价编号
	,@TourId CHAR(36)--团队计划编号
	,@Result INT OUTPUT --结果
AS
BEGIN
	SET @Result=0
	IF NOT EXISTS(SELECT 1 FROM [tbl_CustomerQuote] WHERE [Id]=@QuoteId) RETURN
	IF NOT EXISTS(SELECT 1 FROM [tbl_Tour] WHERE [TourId]=@TourId AND [TourType]=1) RETURN
	DECLARE @OrderId CHAR(36)--团队计划订单编号
	DECLARE @TravellerDisplayType TINYINT--游客信息体现类型
	DECLARE @TravellerFilePath NVARCHAR(255)--游客信息附件
	DECLARE @CompanyId INT--公司编号

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

--create table tbl_CustomerQuoteTravellerSpecialService 增加组团询价游客特服信息表 2011-07-07
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
   '游客编号',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTravellerSpecialService', 'column', 'TravellerId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '服务项目',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTravellerSpecialService', 'column', 'ServiceName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '服务内容',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTravellerSpecialService', 'column', 'ServiceDetail'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '增/减',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTravellerSpecialService', 'column', 'IsAdd'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '费用',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTravellerSpecialService', 'column', 'Fee'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '添加时间',
   'user', @CurrentUser, 'table', 'tbl_CustomerQuoteTravellerSpecialService', 'column', 'IssueTime'
go

alter table tbl_CustomerQuoteTravellerSpecialService
   add constraint FK_TBL_CUSTOMERQUOTETRA_REFERENCE_TBL_CUSTOMERQUOTETRA foreign key (TravellerId)
      references tbl_CustomerQuoteTraveller (TravellerId)
go

--=========================================
--描述：组团端询价添加
--创建人：曹胡生
--时间：2011-03-18
-- History:
-- 1.2011-07-06 增加游客信息 汪奇志
--=========================================
ALTER procedure [dbo].[proc_Inquire_InsertInquire]
(
	@CompanyId int,--专线公司编号
    @RouteId int,--线路编号
    @RouteName nvarchar(255),--线路名称
    @CustomerId int,--组团公司编号
    @CustomerName nvarchar(255),--组团公司名称
    @ContactTel nvarchar(255),-- 联系人电话
    @ContactName nvarchar(255),--联系人姓名
    @AdultNumber int, --成人数
    @ChildNumber int, --儿童数
    @LeaveDate datetime, --预计出团日期
    @SpecialClaim nvarchar(max), --特殊要求说明
    @XingCheng nvarchar(max), --行程要求 XML:<ROOT><XingCheng QuoteId="询价编号" QuotePlan="行程要求" PlanAccessoryName="行程附件名称" PlanAccessory="行程附件"></ROOT>
	@ASK NVARCHAR(MAX),--客人要求 XML:<ROOT><QuoteAskInfo QuoteId="询价编号" ItemType="项目类型" ConcreteAsk="具体要求" /></ROOT>
    @QuoteState tinyint, --询价状态
    @Remark varchar(max), ---备注
    @Result INT OUTPUT --操作结果 正值：成功 负值或0：失败
	,@TravellerDisplayType TINYINT--游客信息体现类型
	,@TravellerFilePath NVARCHAR(255)=NULL--游客信息附件路径
	,@Travellers NVARCHAR(MAX)=NULL--游客信息集合 XML:<ROOT><Info TravellerId="游客编号" TravellerName="游客姓名" CertificateType="证件类型" CertificateCode="证件号码" Gender="性别" TravellerType="游客类型" Telephone="电话" SpecialServiceName="特服项目" SpecialServiceDetail="特服内容" SpecialServiceIsAdd="特服增/减" SpecialServiceFee="特服费用" /></ROOT>
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
--==========================插入询价基本信息==========================--
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
		
--==========================插入行程要求================================----
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
--==========================插入客人要求================================----
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
--==========================插入客人要求结束================================----

	IF(@errorcount=0 AND @Travellers IS NOT NULL AND LEN(@Travellers)>0)--写入游客信息
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
--描述：组团端询价更改
--创建人：曹胡生
--时间：2011-03-18
-- History:
-- 1.2011-07-06 增加游客信息 汪奇志
--=========================================
ALTER procedure [dbo].[proc_Inquire_UpdateInquire]
(
    @Id int,--询价报价编号
	@CompanyId int,--专线公司编号
    @RouteId int,--线路编号
    @RouteName nvarchar(255),--线路名称
    @CustomerId int,--组团公司编号
    @CustomerName nvarchar(255),--组团公司名称
    @ContactTel nvarchar(255),-- 联系人电话
    @ContactName nvarchar(255),--联系人姓名
    @LeaveDate datetime, --预计出团日期
    @AdultNumber int, --成人数
    @ChildNumber int, --儿童数
    @SpecialClaim nvarchar(max), --特殊要求说明
    @Remark varchar(max), ---备注
    @QuoteState tinyint,  --询价状态 未处理 = 0,已处理 = 1,已成功 = 2
    @XingCheng nvarchar(max), --行程要求 XML:<ROOT><XingCheng QuoteId="询价编号" QuotePlan="行程要求" PlanAccessoryName="行程附件名称" PlanAccessory="行程附件"></ROOT>
	@QuoteInfo NVARCHAR(MAX),--价格组成 XML:<ROOT><QuoteInfo QuoteId="询价编号" ItemId="项目类型" Reception="接待标准" LocalQuote="地接报价" MyQuote="我社报价" SumMoney="合计金额"/></ROOT>
	@ASK NVARCHAR(MAX),--客人要求 XML:<ROOT><QuoteAskInfo QuoteId="询价编号" ItemType="项目类型" ConcreteAsk="具体要求" /></ROOT>
    @Result INT OUTPUT --操作结果 正值：成功 负值或0：失败
	,@TravellerDisplayType TINYINT--游客信息体现类型
	,@TravellerFilePath NVARCHAR(255)=NULL--游客信息附件路径
	,@Travellers NVARCHAR(MAX)=NULL--游客信息集合 XML:<ROOT><Info TravellerId="游客编号" TravellerName="游客姓名" CertificateType="证件类型" CertificateCode="证件号码" Gender="性别" TravellerType="游客类型" Telephone="电话" SpecialServiceName="特服项目" SpecialServiceDetail="特服内容" SpecialServiceIsAdd="特服增/减" SpecialServiceFee="特服费用" /></ROOT>
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
--==========================插入询价基本信息==========================--
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
		
--==========================插入行程要求================================----
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
--==========================插入价格组成,先删再插================================----
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

--==========================插入客人要求================================----
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
--==========================插入客人要求结束================================----

	IF(@errorcount=0)--游客信息
	BEGIN
		DELETE FROM [tbl_CustomerQuoteTravellerSpecialService] WHERE TravellerId IN(SELECT [TravellerId] FROM [tbl_CustomerQuoteTraveller] WHERE [QuoteId]=@Id)
		DELETE FROM [tbl_CustomerQuoteTraveller] WHERE [QuoteId]=@Id
		SET @errorcount=@errorcount+@@ERROR
		
		IF(@errorcount=0 AND @Travellers IS NOT NULL AND LEN(@Travellers)>0)--写入游客信息
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
--描述：专线端更新报价
--创建人：曹胡生
--时间：2011-03-20
--History:
--1.2011-04-18 代码格式调整
--2.2011-04-18 汪奇志 增加地接报价人数、单价及我社报价人数、单价，合计金额
--3.2011-07-06 增加游客信息 汪奇志
--=========================================
ALTER procedure [dbo].[proc_Inquire_UpdateQuote]
(
	@Id int, --询价编号
	@CompanyId int,--专线公司编号
	@RouteId int,--线路编号
	@RouteName nvarchar(255),--线路名称
	@ContactTel nvarchar(255),-- 联系人电话
	@ContactName nvarchar(255),--联系人姓名
	@AdultNumber int, --成人数
	@ChildNumber int, --儿童数
	@TicketAgio decimal,--机票折扣
	@SpecialClaim nvarchar(max), --特殊要求说明
    @LeaveDate datetime, --预计出团日期
	@XingCheng nvarchar(max), --行程要求 XML:<ROOT><XingCheng QuoteId="询价编号" QuotePlan="行程要求" PlanAccessoryName="行程附件名称" PlanAccessory="行程附件"></ROOT>
	@ASK NVARCHAR(MAX),--客人要求 XML:<ROOT><QuoteAskInfo QuoteId="询价编号" ItemType="项目类型" ConcreteAsk="具体要求" /></ROOT>
	@QuoteInfo NVARCHAR(MAX),--价格组成 XML:<ROOT><QuoteInfo QuoteId="询价编号" ItemId="项目类型" Reception="接待标准" LocalQuote="地接报价" MyQuote="我社报价" LocalPeopleNumber="地接报价人数" LocalUnitPrice="地接报价单价" SelfPeopleNumber="我社报价人数" SelfUnitPrice="我社报价单价" /></ROOT>
	@Remark varchar(max), ---备注    
	@QuoteState int, --询价状态
	@Result INT OUTPUT, --操作结果 正值：成功 负值或0：失败
	@TotalAmount MONEY --合计金额
	,@TravellerDisplayType TINYINT--游客信息体现类型
	,@TravellerFilePath NVARCHAR(255)=NULL--游客信息附件路径
	,@Travellers NVARCHAR(MAX)=NULL--游客信息集合 XML:<ROOT><Info TravellerId="游客编号" TravellerName="游客姓名" CertificateType="证件类型" CertificateCode="证件号码" Gender="性别" TravellerType="游客类型" Telephone="电话" SpecialServiceName="特服项目" SpecialServiceDetail="特服内容" SpecialServiceIsAdd="特服增/减" SpecialServiceFee="特服费用" /></ROOT>
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
	
	IF(@errorcount=0 and @XingCheng is not null and len(@XingCheng)>0)--插入行程要求
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

	IF(@errorcount=0 and @QuoteInfo is not null and len(@QuoteInfo)>0)--插入价格组成,先删再插
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

	IF(@errorcount=0 and @ASK is not null and len(@ASK)>0)--=插入客人要求
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

	IF(@errorcount=0)--游客信息
	BEGIN
		DELETE FROM [tbl_CustomerQuoteTraveller] WHERE [QuoteId]=@Id
		SET @errorcount=@errorcount+@@ERROR
		
		IF(@errorcount=0 AND @Travellers IS NOT NULL AND LEN(@Travellers)>0)--写入游客信息
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
-- Author:		<曹胡生>
-- Create date: <2011-3-18>
-- Description:	<获取询价报价信息>
-- =============================================
ALTER PROCEDURE [dbo].[proc_Tour_GetInquireQuote] 
@Id int, ---主键编号
@CompanyId int, ---专线公司编号
@CustomerId int, ---组团公司编号
@IsZhuTuan varchar(1) ---是否组团
AS
---不是组团端
IF(@IsZhuTuan=0)
begin
IF EXISTS(SELECT 1 FROM tbl_CustomerQuote WHERE Id=@Id AND CompanyId=@CompanyId)
BEGIN
---询价报价基本信息
SELECT * FROM tbl_CustomerQuote WHERE Id=@Id
---组团询价行程要求
SELECT * FROM tbl_CustomerQuoteClaim WHERE [QuoteId]=@Id
---组团询价客户要求
SELECT * FROM tbl_CustomerQuoteAsk WHERE [QuoteId]=@Id
---组团询价价格组成
SELECT * FROM tbl_CustomerQuotePrice WHERE [QuoteId]=@Id
--游客信息
SELECT A.*,B.ServiceName,B.ServiceDetail,B.IsAdd,B.Fee FROM [tbl_CustomerQuoteTraveller] AS A LEFT OUTER JOIN [tbl_CustomerQuoteTravellerSpecialService] AS B ON A.TravellerId=B.TravellerId WHERE A.[QuoteId]=@Id
END
end
----是组团端
else if(@IsZhuTuan=1)
begin
IF EXISTS(SELECT 1 FROM tbl_CustomerQuote WHERE Id=@Id AND CustomerId=@CustomerId)
BEGIN
---询价报价基本信息
SELECT * FROM tbl_CustomerQuote WHERE Id=@Id
---组团询价行程要求
SELECT * FROM tbl_CustomerQuoteClaim WHERE [QuoteId]=@Id
---组团询价客户要求
SELECT * FROM tbl_CustomerQuoteAsk WHERE [QuoteId]=@Id
---组团询价价格组成
SELECT * FROM tbl_CustomerQuotePrice WHERE [QuoteId]=@Id
--游客信息
SELECT A.*,B.ServiceName,B.ServiceDetail,B.IsAdd,B.Fee FROM [tbl_CustomerQuoteTraveller] AS A LEFT OUTER JOIN [tbl_CustomerQuoteTravellerSpecialService] AS B ON A.TravellerId=B.TravellerId WHERE A.[QuoteId]=@Id
END
end
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-07-06
-- Description:	同步组团询价游客信息到团队计划订单
-- =============================================
ALTER PROCEDURE [dbo].[proc_SyncQuoteTravellerToTourTeamOrder]
	@QuoteId INT--询价编号
	,@TourId CHAR(36)--团队计划编号
	,@Result INT OUTPUT --结果
AS
BEGIN
	SET @Result=0
	IF NOT EXISTS(SELECT 1 FROM [tbl_CustomerQuote] WHERE [Id]=@QuoteId) RETURN
	IF NOT EXISTS(SELECT 1 FROM [tbl_Tour] WHERE [TourId]=@TourId AND [TourType]=1) RETURN
	DECLARE @OrderId CHAR(36)--团队计划订单编号
	DECLARE @TravellerDisplayType TINYINT--游客信息体现类型
	DECLARE @TravellerFilePath NVARCHAR(255)--游客信息附件
	DECLARE @CompanyId INT--公司编号

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

--(20110620)巴比来.doc end
--以上日志已更新 20110711














--(20110629)芭比来.doc start

/*
--计划表增加字段 2011-07-05
ALTER TABLE dbo.tbl_Tour ADD
	NumberCR int NOT NULL CONSTRAINT DF_tbl_Tour_NumberCR DEFAULT 0,
	NumberET int NOT NULL CONSTRAINT DF_tbl_Tour_NumberET DEFAULT 0,
	NumberQP int NOT NULL CONSTRAINT DF_tbl_Tour_NumberQP DEFAULT 0,
	UnitAmountCR money NOT NULL CONSTRAINT DF_tbl_Tour_UnitAmountCR DEFAULT 0,
	UnitAmountET money NOT NULL CONSTRAINT DF_tbl_Tour_UnitAmountET DEFAULT 0,
	UnitAmountQP money NOT NULL CONSTRAINT DF_tbl_Tour_UnitAmountQP DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'成人数'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'NumberCR'
GO
DECLARE @v sql_variant 
SET @v = N'儿童数'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'NumberET'
GO
DECLARE @v sql_variant 
SET @v = N'全陪数'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'NumberQP'
GO
DECLARE @v sql_variant 
SET @v = N'成人单价合计'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'UnitAmountCR'
GO
DECLARE @v sql_variant 
SET @v = N'儿童单价合计'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'UnitAmountET'
GO
DECLARE @v sql_variant 
SET @v = N'全陪单价合计'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'UnitAmountQP'
GO

--订单表增加字段 2011-07-05
ALTER TABLE dbo.tbl_TourOrder ADD
	NumberQP int NOT NULL CONSTRAINT DF_tbl_TourOrder_NumberQP DEFAULT 0,
	UnitAmountQP money NOT NULL CONSTRAINT DF_tbl_TourOrder_UnitAmountQP DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'全陪数'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOrder', N'COLUMN', N'NumberQP'
GO
DECLARE @v sql_variant 
SET @v = N'全陪单价合计'
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
   '计划编号',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'TourId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '成人数',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'NumberCR'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '儿童数',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'NumberET'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '全陪数',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'NumberQP'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '成人单价合计',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'UnitAmountCR'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '儿童单价合计',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'UnitAmountET'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '全陪单价合计',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'UnitAmountQP'
go

alter table tbl_TourTeamUnit
   add constraint FK_TBL_TOURTEAMUNIT_REFERENCE_TBL_TOUR foreign key (TourId)
      references tbl_Tour (TourId)
go
*/

--create table tbl_TourTeamUnit 团队计划计划人数及价格信息表 2011-07-05 
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
   '计划编号',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'TourId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '成人数',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'NumberCR'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '儿童数',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'NumberET'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '全陪数',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'NumberQP'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '成人单价合计',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'UnitAmountCR'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '儿童单价合计',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'UnitAmountET'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '全陪单价合计',
   'user', @CurrentUser, 'table', 'tbl_TourTeamUnit', 'column', 'UnitAmountQP'
go

alter table tbl_TourTeamUnit
   add constraint FK_TBL_TOURTEAMUNIT_REFERENCE_TBL_TOUR foreign key (TourId)
      references tbl_Tour (TourId)
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


GO
-- =============================================
-- Author:		汪奇志
-- Create date: 2011-01-23
-- Description:	更新散拼计划、团队计划、单项服务信息
-- History:
-- 1.2011-07-11 团队计划增加人数和价格信息，删除SelfUnitPriceAmount
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
			,[AdultNumber]=@PlanPeopleNumber
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
				,[CustomerStatus])
			SELECT A.TravelerId,@OrderId,@CompanyId,A.TravelerName,A.IdType
				,A.IdNo,A.Gender,A.TravelerType,A.ContactTel,GETDATE()
				,0 FROM OPENXML(@hdoc,'/ROOT/Info')
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

GO

GO

-- =============================================
-- Author:		luofx
-- Create date: 2011-01-24
-- Description:	修改订单
-- History:
-- 1.增加组团联系人信息、返佣信息 汪奇志 2011-06-08
-- 2.增加修改团队计划订单同步修改团队计划信息，修正人数验证（减去已退团人数）
-- =============================================
ALTER PROCEDURE [dbo].[proc_TourOrder_Update]	
	@OrderID CHAR(36),                --订单编号
	@TourId CHAR(36),	             --团队编号
	@OrderState TINYINT,              --订单状态
	@BuyCompanyID INT,	             --购买公司编号
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
	SET @ErrorCount=0
	SET @Result=1
BEGIN	
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
		DECLARE @CommissionType TINYINT --返佣类型
		DECLARE @CommissionIsPaid CHAR(1)--返佣金额是否支付
		
		IF((@BuyerContactName IS NULL OR LEN(@BuyerContactName)<1) AND @BuyerContactId>0)
		BEGIN
			SELECT @BuyerContactName=[Name] FROM [tbl_CustomerContactInfo] WHERE [Id]=@BuyerContactId
		END
		--重新计算财务小计:新的订单金额+增加费用+减少费用
		SELECT @FinanceSum=@SumPrice+FinanceAddExpense-FinanceRedExpense,@CommissionType=[CommissionType],@CommissionIsPaid=[CommissionStatus] FROM [tbl_TourOrder] a WHERE a.[ID]=@OrderID				
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
	IF(@ErrorCount>0)
	BEGIN
		SET @Result=0
		ROLLBACK TRANSACTION TourOrder_Update
	END
	COMMIT TRANSACTION TourOrder_Update
	--更新是否收客收满
	UPDATE [tbl_Tour] SET [Status]=1  WHERE [Status]=0 AND PlanPeopleNumber=(SELECT ISNULL(SUM(PeopleNumber-LeaguePepoleNum),0) FROM tbl_TourOrder WHERE TourId=(SELECT TourId FROM tbl_TourOrder WHERE [id]=@OrderID) AND IsDelete='0' AND OrderState NOT IN(3,4))						
	RETURN  @Result
END

GO


GO
-- =============================================
-- Author:		luofx
-- Create date: 2011-01-24
-- Description:	新增订单
-- History:
-- 1.增加组团联系人信息、返佣信息 汪奇志 2011-06-08
-- 2.修正人数验证（减去已退团人数）
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
	UPDATE [tbl_Tour] SET [Status]=1  WHERE [Status]=0  AND IsDelete='0' AND PlanPeopleNumber=(SELECT ISNULL(SUM(PeopleNumber-LeaguePepoleNum),0) FROM tbl_TourOrder WHERE TourId=@TourId  AND IsDelete='0' AND OrderState NOT IN(3,4))						
	RETURN  @Result
END

GO


GO
-- =============================================
-- Author:luofx
-- Create date: 2011-01-18
-- Description:	游客退团:新增退团记录并修改游客表的退团状态
-- =============================================
ALTER PROCEDURE [dbo].[proc_TourOrderCustomer_AddLeague] 
	@CustormerId CHAR(36),       --游客编号
	@OperatorName NVARCHAR(250),      --操作人姓名
	@OperatorID INT,		--操作人编号
	@RefundReason NVARCHAR(1000),      --退款原因
	@RefundAmount DECIMAL(10,4),       --退款金额
	@AddOrEdit int,		--新增或者修改（0：新增；1：修改）				
	@Result INT OUTPUT,  -- 返回参数	
	@ReturnTourId char(36) OUTPUT,	--团队Id，返回参数
	@ReturnOrderId char(36) OUTPUT	--订单Id，返回参数
AS
	DECLARE @ErrorCount int         --验证错误
	declare @TourId char(36)		--团队Id
	DECLARE @OrderId CHAR(36)       --订单编号
	declare @TourType tinyint		--团队类型
	declare @CustomerType tinyint	--游客类型（成人、儿童）
	declare @OldLeaguePepoleNum int --已退团人数
	declare @AdultPrice money		--成人单价
	declare @ChildPrice money		--儿童单价
	declare @OrderPrice money		--订单金额（非财务小计）
	SET @ErrorCount=0
BEGIN
	BEGIN Transaction TourOrderCustomer_AddLeague

	SELECT TOP 1 @OrderId=OrderId,@CustomerType = VisitorType FROM tbl_TourOrderCustomer WHERE ID=@CustormerId
	SELECT @TourId = TourId,@OldLeaguePepoleNum = LeaguePepoleNum,@TourType = TourClassId,@AdultPrice = PersonalPrice,@ChildPrice = ChildPrice,@OrderPrice = SumPrice FROM  [tbl_TourOrder] WHERE ID=@OrderId

	if @AddOrEdit = 0 --新增退团
	begin
		INSERT INTO [tbl_CustomerLeague] ([CustormerId],[OperatorName],[OperatorID],[RefundReason],
				[IssueTime],[RefundAmount])VALUES (@CustormerId,@OperatorName,@OperatorID,
					@RefundReason,getdate(),@RefundAmount)
		SET @ErrorCount=@ErrorCount+@@ERROR

		UPDATE [tbl_TourOrderCustomer] SET CustomerStatus=1 WHERE [ID]=@CustormerId
		SET @ErrorCount=@ErrorCount+@@ERROR

		--更新已退团人数
		update [tbl_TourOrder] set 
			LeaguePepoleNum = @OldLeaguePepoleNum + 1 
		where ID = @OrderId
		SET @ErrorCount=@ErrorCount+@@ERROR
	end
	else if @AddOrEdit = 1 --修改退团
	begin
		UPDATE tbl_CustomerLeague SET 
			OperatorName = @OperatorName
			,OperatorID = @OperatorID
			,RefundReason = @RefundReason
			,RefundAmount = @RefundAmount 
		WHERE CustormerId = @CustormerId
	end

	--团队类型枚举： 散拼计划 = 0,团队计划 = 1,单项服务 = 2
	--游客类型枚举： 未知 = 0,成人 = 1,儿童 = 2

	if @TourType = 0 --散拼计划
	begin
		set @OrderPrice = @OrderPrice + @RefundAmount - (case when @CustomerType = 1 then @AdultPrice when @CustomerType = 2 then @ChildPrice else 0 end)
		update [tbl_TourOrder] set 
			[SumPrice] = @OrderPrice
			,FinanceSum = @OrderPrice + FinanceAddExpense - FinanceRedExpense
		where ID = @OrderId
		SET @ErrorCount=@ErrorCount+@@ERROR
	end
	else if @TourType = 1 --团队计划
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
--	else if @TourType = 2  --单项服务
--	begin
--		
--	end

	--是否回滚
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
团队计划结算单据：枚举值 35   团队结算明细单 到webmaster里修改
望海系统：/print/wh/TourTeamSettlePrint.aspx    
其它系统：/print/bbl/TeamSettledPrint.aspx
INSERT INTO [tbl_CompanySetting]([Id],[FieldName],[FieldValue])VALUES(1,'PrintDocument_35','/print/bbl/TeamSettledPrint.aspx')
INSERT INTO [tbl_CompanySetting]([Id],[FieldName],[FieldValue])VALUES(2,'PrintDocument_35','/print/wh/TourTeamSettlePrint.aspx')
INSERT INTO [tbl_CompanySetting]([Id],[FieldName],[FieldValue])VALUES(3,'PrintDocument_35','/print/bbl/TeamSettledPrint.aspx')
*/


--(20110629)芭比来.doc end

--以上日志已更新


--计划信息表增加线路状态
ALTER TABLE dbo.tbl_Tour ADD
	RouteStatus tinyint NOT NULL CONSTRAINT DF_tbl_Tour_RouteStatus DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'线路状态'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'RouteStatus'
GO

ALTER TABLE dbo.tbl_Tour ADD
	HandStatus tinyint NOT NULL CONSTRAINT DF_tbl_Tour_HandStatus DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'手动设置的计划状态'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'HandStatus'
GO

--以上日志已更新


--杂费收入支出表增加收款或付款状态字段
GO
ALTER TABLE dbo.tbl_TourOtherCost ADD
	Status char(1) NOT NULL CONSTRAINT DF_tbl_TourOtherCost_Status DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'收款或付款状态 0：未收或未付 1：已收或已付'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOtherCost', N'COLUMN', N'Status'
GO

--原有杂费收入支出状态批量处理成已收或已付
UPDATE [tbl_TourOtherCost] SET [Status]='1'
GO

GO
-- =============================================
-- Author:		<Author,,李焕超>
-- Create date: <Create Date,2011-02-14,>
-- Description:	<Description,退票,>
-- History:
-- 1.2011-04-21 汪奇志 过程调整 
-- 2.2011-08-09 周文超 退票金额杂费收入默认已收
-- =============================================
ALTER PROCEDURE [dbo].[proc_Ticket_Return]
(	 
	@TicketListId int,--机票管理列表编号
	@RefundAmount money,--退回金额
	@Status tinyint,--退票状态
	@Remark varchar(250),--退票需知
	@OperatorId INT,--操作人
	@Result int output,--操作结果 1：success 	
	@TourId CHAR(36) OUTPUT--退票所在申请的计划编号
)
AS
BEGIN
	DECLARE @errorcount INT
	DECLARE @TravellerRefundId CHAR(36)--游客退票编号

	SET @Result=0
	SET @TourId=''
	SET @errorcount=0
	SELECT @TravellerRefundId=[RefundId] FROM [tbl_PlanTicket] WHERE [Id]=@TicketListId AND [TicketType]=3

	IF(@TravellerRefundId IS NULL) RETURN -1

	BEGIN TRAN
	--更改游客退票表状态及退回金额
	UPDATE [tbl_CustomerRefund] SET [IsRefund]=@Status,RefundAmount = @RefundAmount WHERE [Id]=@TravellerRefundId
	SET @errorcount=@errorcount+@@ERROR
	--更改机票管理列表状态
	UPDATE [tbl_Planticket] SET [State]=@Status WHERE Id=@TicketListId
	SET @errorcount=@errorcount+@@ERROR

	IF(@Status=4)--退票完成
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM [tbl_TourOtherCost] WHERE [ItemId] =@travellerRefundId AND [ItemType]=1 AND [CostType]=0)--写杂费收入
		BEGIN
			DECLARE @OtherInComeId CHAR(36)--杂费收入表编号	
			DECLARE @CompanyId INT--公司编号
			DECLARE @AreaId INT--线路区域编号
			DECLARE @TourType TINYINT--计划类型
			DECLARE @TravellerId CHAR(36)--退票游客编号
			DECLARE @OrderId CHAR(36)--退票游客所在订单编号
			SET @OtherInComeId=NEWID()
			
			SELECT @TravellerId=[CustormerId] FROM [tbl_CustomerRefund] WHERE [Id]=@TravellerRefundId
			SELECT @OrderId =[OrderId] FROM [tbl_TourOrderCustomer] WHERE [Id]=@TravellerId
			SELECT @TourId=[TourId] FROM [tbl_TourOrder] WHERE [Id]=@OrderId
			SELECT @CompanyId=[CompanyId],@TourType=[TourType] FROM [tbl_Tour] WHERE [TourId]=@TourId
			--写杂费收入
			INSERT INTO [tbl_TourOtherCost]([Id],[CompanyId],[TourId],[CostType],[CustromCName]
				,[CustromCId],[ProceedItem],[Proceed],[IncreaseCost],[ReduceCost]
				,[SumCost],[Remark],[OperatorId],[CreateTime],[PayTime]
				,[Payee],[PayType],[ItemType],[ItemId],[Status])
			VALUES(@OtherInComeId,@CompanyId,@TourId,0,''
				,0,'机票退票款',@RefundAmount,0,0
				,@RefundAmount,'',@OperatorId,GETDATE(),GETDATE()
				,'',1,1,@TravellerRefundId,'1')
			SET @errorcount=@errorcount+@@ERROR
			IF(@errorcount=0)--写收入明细
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
		ELSE--更新杂费收入
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
--以上SQL已更新至服务器 2011-08-12
