GO
--2012-08-17 汪奇志 增加发票登记信息表
GO
CREATE TABLE [dbo].[tbl_FinFaPiao](
	[Id] [char](36) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[CrmId] [int] NOT NULL,
	[RiQi] [datetime] NOT NULL,
	[JinE] [money] NOT NULL CONSTRAINT [DF__tbl_FinFaP__JinE__014A0122]  DEFAULT ((0)),
	[PiaoHao] [nvarchar](255) NULL,
	[KaiPiaoRenId] [int] NOT NULL CONSTRAINT [DF__tbl_FinFa__KaiPi__023E255B]  DEFAULT ((0)),
	[KaiPiaoRen] [nvarchar](255) NULL,
	[BeiZhu] [nvarchar](255) NULL,
	[CaoZuoRenId] [int] NOT NULL,
	[IssueTime] [datetime] NOT NULL CONSTRAINT [DF__tbl_FinFa__Issue__03324994]  DEFAULT (getdate()),
	[IsDelete] [char](1) NOT NULL CONSTRAINT [DF__tbl_FinFa__IsDel__04266DCD]  DEFAULT ('0'),
 CONSTRAINT [PK_TBL_FINFAPIAO] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登记编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公司编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户单位编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'CrmId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开票日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'RiQi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开票金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'JinE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'票号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'PiaoHao'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开票人编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'KaiPiaoRenId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开票人姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'KaiPiaoRen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'BeiZhu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作人编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'CaoZuoRenId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'IssueTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'财务管理-发票登记信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao'
GO

-- =============================================
-- Author:		周文超
-- Create date: 2010-01-21
-- Description:	人次统计基本视图
-- 周文超 2011-06-20增加出团日期列
-- 2011-12-20 汪奇志 人次统计销售员改用人次统计销售员列
-- 2012-08-22 汪奇志 增加[tbl_TourOrder].[TourId]
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
,TourId
from tbl_TourOrder left join tbl_CompanyUser on tbl_CompanyUser.Id = tbl_TourOrder.PerTimeSellerId
where tbl_TourOrder.isdelete = '0' and tbl_TourOrder.IsDelete = '0' and tbl_TourOrder.OrderState not in (3,4) AND tbl_TourOrder.PerTimeSellerId>0
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2012-08-24
-- Description:	收款登记视图
-- =============================================
CREATE VIEW view_FinShouKuanDengJi
AS
SELECT A.Id
	,A.CompanyId,A.ItemId AS OrderId,A.RefundDate,A.RefundMoney,A.RefundType,A.IsCheck
	,B.OrderNo,B.TourId,B.BuyCompanyName,B.BuyCompanyId
	,C.TourCode,C.RouteName,C.LeaveDate
FROM tbl_ReceiveRefund AS A INNER JOIN tbl_TourOrder AS B
ON A.ItemId=B.Id INNER JOIN tbl_Tour AS C
ON C.TourId=B.TourId
WHERE A.IsReceive=1 AND A.ItemType=1
GO

--2012-08-24 汪奇志 收退款登记表增加审核时间
ALTER TABLE dbo.tbl_ReceiveRefund ADD
	ShenHeTime datetime NULL
GO
DECLARE @v sql_variant 
SET @v = N'审核时间'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_ReceiveRefund', N'COLUMN', N'ShenHeTime'
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2012-08-24
-- Description:	团队利润统计视图
-- =============================================
CREATE VIEW view_TongJiTuanDuiLiRun
AS
SELECT A.TourId,A.LeaveDate,A.AreaId,A.CompanyId,A.TotalOtherIncome,(A.TotalIncome + A.TotalOtherIncome) AS ZhiChuJinE,A.DistributionAmount
	,B.Id AS OrderId,B.SalerId,B.FinanceSum,(B.PeopleNumber-B.LeaguePepoleNum) AS RenShu
	,C.ProviceId AS ProvinceId,C.CityId
	,D.DepartId,D.ContactName AS XiaoShouYuanName
	,1 AS TuanDuiShu
FROM tbl_Tour AS A INNER JOIN tbl_TourOrder AS B
ON A.TourId=B.TourId INNER JOIN tbl_Customer AS C
ON B.BuyCompanyId=C.Id INNER JOIN tbl_CompanyUser AS D
ON B.SalerId=D.Id
WHERE A.IsDelete='0' AND A.TourType=1
GO

--汪奇志 订单游客信息表中增加团队编号
ALTER TABLE dbo.tbl_TourOrderCustomer ADD
	TourId char(36) NULL
GO
DECLARE @v sql_variant 
SET @v = N'团队编号'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOrderCustomer', N'COLUMN', N'TourId'
GO

--汪奇志 2012-08-24 同步订单游客信息表中团队编号
UPDATE tbl_TourOrderCustomer SET TourId=(SELECT TourId FROM tbl_TourOrder AS B WHERE B.Id=A.OrderId)
FROM tbl_TourOrderCustomer AS A
GO

-- =============================================
-- Author:		luofx
-- Create date: 2011-01-23
-- Description:	团队计划-增加游客信息
-- History:
-- 1.汪奇志 2012-01-12 临时表字段[Vid]变更成标识列
-- 2.汪奇志 2012-08-24 增加团队编号的写入
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
	DECLARE @TourId CHAR(36)--团队编号
	SELECT @TourId=TourId FROM tbl_TourOrder WHERE Id=@OrderId
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
					[CradType],[CradNumber],[Sex],[VisitorType],[ContactTel],[TourId])
					VALUES(@ID,@CompanyId,@OrderId,@VisitorName,
						@CradType,@CradNumber,@Sex,@VisitorType,@ContactTel,@TourId)
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

-- =============================================
-- Author:		luofx
-- Create date: 2011-01-23
-- Description:	团队计划-修改(增加)游客信息   
-- History:
-- 1.汪奇志 2012-01-12 临时表字段[Vid]变更成标识列
-- 2.汪奇志 2012-08-24 增加团队编号的写入
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
	DECLARE @TourId CHAR(36)--团队编号
	SELECT @TourId=TourId FROM tbl_TourOrder WHERE Id=@OrderId
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
									[CradType],[CradNumber],[Sex],[VisitorType],[ContactTel],[TourId])
								VALUES(@ID,@CompanyId,@OrderId,@VisitorName,
									@CradType,@CradNumber,@Sex,@VisitorType,@ContactTel,@TourId)
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
-- Author:		汪奇志
-- Create date: 2011-07-06
-- Description:	同步组团询价游客信息到团队计划订单
-- History:
-- 1.汪奇志 2012-08-24 增加团队编号的写入
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
		,[TicketStatus],[CustomerStatus],[TourId])
	SELECT A.TravellerId,@OrderId,@CompanyId,A.TravellerName,A.CertificateType
		,A.CertificateCode,A.Gender,A.TravellerType,A.Telephone,GETDATE()
		,0,0,@TourId
	FROM [tbl_CustomerQuoteTraveller] AS A
	WHERE A.[QuoteId]=@QuoteId

	INSERT INTO [tbl_CustomerSpecialService]([CustormerId],[ProjectName],[ServiceDetail],[IsAdd],[Fee],[IssueTime])
	SELECT A.TravellerId,A.[ServiceName],A.[ServiceDetail],A.[IsAdd],A.[Fee],GETDATE()
	FROM [tbl_CustomerQuoteTravellerSpecialService] AS A WHERE A.[TravellerId] IN(SELECT B.[TravellerId] FROM [tbl_CustomerQuoteTraveller] AS B WHERE B.[QuoteId]=@QuoteId )
	
	SET @Result=1
	RETURN	
END
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
			IF(@errorcount=0)--标记最近出团
			BEGIN
				EXECUTE proc_Tour_IsRecently @TourId=@TourId,@IsT=N'1'
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
			IF(@errorcount=0)
			BEGIN
				UPDATE [tbl_Tour] SET [Status]=1  WHERE [Status]=0 AND PlanPeopleNumber=(SELECT ISNULL(SUM(PeopleNumber-LeaguePepoleNum),0) FROM tbl_TourOrder WHERE TourId=(SELECT TourId FROM tbl_TourOrder WHERE [id]=@OrderID) AND IsDelete='0' AND OrderState NOT IN(3,4))	and TourId=@TourId 
				SET @errorcount=@errorcount+@@ERROR
			END
			IF(@errorcount=0)
			BEGIN
				UPDATE [tbl_Tour] SET [Status]=0  WHERE [Status]=1 AND PlanPeopleNumber>(SELECT ISNULL(SUM(PeopleNumber-LeaguePepoleNum),0) FROM tbl_TourOrder WHERE TourId=(SELECT TourId FROM tbl_TourOrder WHERE [id]=@OrderID) AND IsDelete='0' AND OrderState NOT IN(3,4))	and TourId=@TourId 
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
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-03-21
-- Description:	散客天天发计划生成散拼计划后续工作(创建订单、更新申请状态、天天发计划已处理数量维护、未处理数量维护等)
-- History:
-- 1.2012-08-24 汪奇志 订单游客信息表增加团队编号的写入
-- =============================================
ALTER PROCEDURE [dbo].[proc_TourEveryday_BuildTourFollow]
	@TourId CHAR(36),--散拼计划计划编号
	@EverydayTourId CHAR(36),--散客天天发计划编号
	@EverydayTourApplyId CHAR(36),--散客天天发计划申请编号
	@Result INT OUTPUT,--正值时成功，负值或0时失败
	@ApplyCompanyId INT OUTPUT--客户单位编号
AS
BEGIN	
	DECLARE @errorcount INT	
	DECLARE @OrderId CHAR(36)--订单编号
	DECLARE @SellerId INT --销售员编号
	DECLARE @SellerName NVARCHAR(255)--销售员姓名
	DECLARE @BuyerCompanyName NVARCHAR(255)--客户单位名称	
	DECLARE @StandardId INT--报价等级编号
	DECLARE @LevelId INT--客户等级编号
	DECLARE @AdultPrice MONEY--成人价格
	DECLARE @ChildrenPrice MONEY--儿童价格
	DECLARE @AdultNumber INT--成人数
	DECLARE @ChildrenNumber INT--儿童数
	DECLARE @Amount MONEY--订单金额
	DECLARE @TravellerDisplayType TINYINT--游客信息体现类型
	DECLARE @TravellerFilePath NVARCHAR(255)--游客信息附件路径
	DECLARE @CompanyId INT--公司(专线)编号
	DECLARE @BuildTourOperatorId INT--计划操作人
	DECLARE @AreaId INT--线路区域编号
	DECLARE @TourType TINYINT--计划类型
	DECLARE @ApplyUserId INT--申请人编号
	DECLARE @ApplyUserContactName NVARCHAR(255)--申请人姓名
	DECLARE @ApplyUserContactTelephone NVARCHAR(255)--申请人电话
	DECLARE @ApplyUserContactMobile NVARCHAR(255)--申请人手机
	SET @errorcount=0
	SET @OrderId=NEWID()
	SELECT @ApplyCompanyId=[ApplyCompanyId],@StandardId=[Standard],@LevelId=[LevelId],@AdultNumber=[AdultNumber]
		,@ChildrenNumber=[ChildrenNumber],@TravellerDisplayType=[CustomerDisplayType],@TravellerFilePath=[CustomerFilePath],@ApplyUserId=[OperatorId]
	FROM [tbl_TourEverydayApply] WHERE [ApplyId]=@EverydayTourApplyId
	SELECT @SellerId=[SaleId],@SellerName=[Saler] 
	FROM [tbl_Customer] WHERE [Id]=@ApplyCompanyId	
	SELECT 	@AdultPrice=[AdultPrice],@ChildrenPrice=[ChildPrice] 
	FROM [tbl_TourPrice] WHERE [TourId]=@TourId AND [Standard]=@StandardId AND [LevelId]=@LevelId
	SELECT @BuildTourOperatorId=[OperatorId],@CompanyId=[CompanyId],@AreaId=[AreaId],@TourType=[TourType] FROM [tbl_Tour] WHERE [TourId]=@TourId
	SET @Amount=@AdultPrice*@AdultNumber+@ChildrenPrice*@ChildrenNumber
	SELECT @BuyerCompanyName=[Name] FROM [tbl_Customer] WHERE [Id]=@ApplyCompanyId
	SELECT @ApplyUserContactName=[ContactName],@ApplyUserContactTelephone=[ContactTel],@ApplyUserContactMobile=[ContactMobile] FROM [tbl_CompanyUser] WHERE [Id]=@ApplyUserId

	BEGIN TRAN
	--生成订单
	INSERT INTO [tbl_TourOrder]([ID],[OrderNo],[TourId],[AreaId],[RouteId]
		,[RouteName],[TourNo],[TourClassId],[TourDays],[LeaguePepoleNum]
		,[LeaveDate],[LeaveTraffic],[ReturnTraffic],[OrderType],[OrderState]
		,[BuyCompanyID],[BuyCompanyName],[ContactName],[ContactTel],[ContactMobile]
		,[ContactFax],[SalerName],[SalerId],[OperatorName],[OperatorID]
		,[CustomerLevId],[PriceStandId],[PersonalPrice],[ChildPrice],[MarketPrice]
		,[AdultNumber],[ChildNumber],[MarketNumber],[PeopleNumber],[OtherPrice]
		,[SaveSeatDate],[OperatorContent],[SpecialContent],[SumPrice],[SellCompanyName]
		,[SellCompanyId],[LastDate],[LastOperatorID],[SuccessTime],[FinanceAddExpense]
		,[FinanceRedExpense],[FinanceSum],[FinanceRemark],[HasCheckMoney],[NotCheckMoney]
		,[IssueTime],[IsDelete],[ViewOperatorId],[CustomerDisplayType],[CustomerFilePath])
	SELECT @OrderId,dbo.fn_TourOrder_CreateOrderNo(),@TourId,@AreaId,A.RouteId
		,A.RouteName,A.TourCode,@TourType,A.TourDays,0
		,A.LeaveDate,A.LTraffic,A.RTraffic,3,5
		,@ApplyCompanyId,@BuyerCompanyName,@ApplyUserContactName,@ApplyUserContactTelephone,@ApplyUserContactMobile
		,'',@SellerName,@SellerId,(SELECT [ContactName] FROM [tbl_CompanyUser] WHERE [Id]=A.OperatorId),A.OperatorId
		,@LevelId,@StandardId,@AdultPrice,@ChildrenPrice,0
		,@AdultNumber,@ChildrenNumber,0,@AdultNumber+@ChildrenNumber,0
		,GETDATE(),'','',@Amount,(SELECT [CompanyName] FROM [tbl_CompanyInfo] WHERE [Id]=A.CompanyId)
		,A.CompanyId,GETDATE(),@BuildTourOperatorId,NULL,0
		,0,@Amount,'',0,0
		,GETDATE(),'0',A.OperatorId,@TravellerDisplayType,@TravellerFilePath
	FROM [tbl_Tour] AS A WHERE A.TourId=@TourId
	SET @errorcount=@errorcount+@@ERROR

	IF(@errorcount=0)--订单增加减少费用
	BEGIN
		INSERT INTO [tbl_TourOrderPlus](OrderId,AddAmount,ReduceAmount,Remark) VALUES(@OrderId,0,0,'')
		SET @errorcount=@errorcount+@@ERROR	
	END

	IF(@errorcount=0)--游客信息
	BEGIN
		
		INSERT INTO [tbl_TourOrderCustomer]([ID],[OrderId],[CompanyID],[VisitorName],[CradType]
			,[CradNumber],[Sex],[VisitorType],[ContactTel],[IssueTime]
			,[TicketStatus],[CustomerStatus],[TourId])
		SELECT A.TravellerId,@OrderId,@CompanyId,A.TravellerName,A.CertificateType
			,A.CertificateCode,A.Gender,A.TravellerType,A.Telephone,GETDATE()
			,0,0,@TourId
		FROM [tbl_TourEverydayApplyTraveller] AS A WHERE A.ApplyId=@EverydayTourApplyId
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount=0)--写收入
	BEGIN
		INSERT INTO [tbl_StatAllIncome]([CompanyId],[TourId],[AreaId],[TourType],[ItemId]
			,[ItemType],[Amount],[AddAmount],[ReduceAmount],[TotalAmount]
			,[OperatorId],[DepartmentId],[CreateTime],[HasCheckAmount],[NotCheckAmount]
			,[IsDelete])
		SELECT @CompanyId,@TourId,@AreaId,@TourType,@OrderId
			,1,@Amount,0,0,@Amount
			,@SellerId,(SELECT [DepartId] FROM [tbl_CompanyUser] WHERE [Id]=@SellerId),GETDATE(),0,0
			,'0'		
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount=0)--申请处理状态维护 
	BEGIN
		UPDATE [tbl_TourEverydayApply] SET [HandleStatus]=1,[HandleOperatorId]=@BuildTourOperatorId,[HandleTime]=GETDATE(),[BuildTourId]=@TourId
		WHERE [ApplyId]=@EverydayTourApplyId
		SET @errorcount=@errorcount+@@ERROR		
	END
	
	IF(@errorcount=0)--散客天天发计划已处理数量、未处理数量维护
	BEGIN
		DECLARE @HandleNumber INT --已处理数量
		DECLARE @UntreatedNumber INT --未处理数量
		SELECT @HandleNumber=COUNT(*) FROM [tbl_TourEverydayApply] WHERE [TourId]=@EverydayTourId AND [HandleStatus]=1
		SELECT @UntreatedNumber=COUNT(*) FROM [tbl_TourEverydayApply] WHERE [TourId]=@EverydayTourId AND [HandleStatus]=0

		UPDATE [tbl_TourEveryday] SET HandleNumber=@HandleNumber,UntreatedNumber=@UntreatedNumber WHERE [TourId]=@EverydayTourId
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount=0)--维护团队虚拟实收数=订单总人数(不含留住过期、取消的订单)
	BEGIN
		UPDATE [tbl_Tour] SET [VirtualPeopleNumber]=(SELECT ISNULL(SUM([PeopleNumber]-[LeaguePepoleNum]),0) FROM [tbl_TourOrder] WHERE [TourId]=[tbl_Tour].[TourId]  AND [IsDelete]='0' AND [OrderState] IN(1,2,5))
		WHERE [TourId]=@TourId 
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

--汪奇志 2012-08-24 [tbl_TourOrderCustomer].[TourId] NOT NULL
ALTER TABLE [tbl_TourOrderCustomer]
ALTER COLUMN [TourId] CHAR(36) NOT NULL
GO

--汪奇志 2012-08-26 [tbl_Customer]增加[JieSuanType]、[IsRequiredTourCode]字段
ALTER TABLE dbo.tbl_Customer ADD
	JieSuanType tinyint NOT NULL CONSTRAINT DF_tbl_Customer_JieSuanType DEFAULT 0,
	IsRequiredTourCode char(1) NOT NULL CONSTRAINT DF_tbl_Customer_IsRequiredTourCode DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'结算方式（月结、单团结算）'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Customer', N'COLUMN', N'JieSuanType'
GO
DECLARE @v sql_variant 
SET @v = N'对方团号是否必填'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Customer', N'COLUMN', N'IsRequiredTourCode'
GO

-- =============================================
-- Author:		<李焕超,>
-- Create date: <Create 2011-1-21,,>
-- Description:	<添加客户,,>
-- History:
-- 1.2011-05-26 汪奇志 结算日期
-- 2.2011-06-07 汪奇志 结算方式 结算日类型
-- 3.2012-08-26 汪奇志 增加@JieSuanType，@IsRequiredTourCode
-- =============================================
ALTER PROCEDURE  [dbo].[proc_Customer_Insert]
(
	@ProviceId  int ---省份编号
	,@ProvinceName nvarchar(255)---省份
	,@CityId int ---城市编号
	,@CityName nvarchar(255)---城市名称
	,@Name nvarchar(30)---客户单位名称
	,@Licence nvarchar(30)---许可证号
	,@Adress nvarchar(50)---地址
	,@PostalCode nvarchar(10)---邮编
	,@BankAccount nvarchar(255)---银行账号
	,@MaxDebts money ---最高欠款金额
	,@PreDeposit money ---预存款
	,@CommissionCount money---返佣金额
	,@Saler nvarchar(50)--销售员
	,@SaleId int--销售员编号
	,@BrandId int --专线给此客户分配的品牌编号
	,@CustomerLev int----客户等级
	,@CommissionType tinyint ---返佣类型
	,@CompanyId int--公司编号
	,@ContactName nvarchar(255)---联系人
	,@Phone nvarchar(255)---电话
	,@Mobile nvarchar(255)---手机
	,@Fax nvarchar(255)--传真
	,@Remark nvarchar(500)---备注
	,@OperatorId int---操作员编号
	,@CustomerInfoXML NVARCHAR(MAX) ---联系人列表
	,@Result int output --输出参数 9 ，插入不分配账号的联系人 2，插入用户 8 ,未执行xml	 
	,@AccountDay TINYINT--结算日期,
	,@IsEnable CHAR(1)--是否启用,
	,@AccountWay NVARCHAR(255)--结算方式
	,@AccountDayType TINYINT--结算日类型
	,@JieSuanType TINYINT--结算方式
	,@IsRequiredTourCode CHAR(1)--对方团号是否必填
)
as
BEGIN
	declare @CustomerId int 
	DECLARE @hdoc INT
	declare @newUserId int ---新创建用户的编号
	declare @errorCount int --错误次数
	set @newUserId =0
	set @errorCount=0

	INSERT INTO [tbl_Customer]([ProviceId],[ProvinceName],[CityId],[CityName],[Name]
		,[Licence],[Adress],[PostalCode],[BankAccount],[MaxDebts]
		,[PreDeposit],[CommissionCount],[Saler],[SaleId],[CustomerLev]
		,[CommissionType],[CompanyId],[IsEnable],[ContactName],[Phone]
		,[Mobile],[Fax],[Remark],[OperatorId],[IssueTime]
		,[IsDelete],[BrandId],[AccountDay],[AccountWay],[AccountDayType]
		,[JieSuanType],[IsRequiredTourCode])
	VALUES(@ProviceId,@ProvinceName,@CityId,@CityName,@Name
		,@Licence,@Adress,@PostalCode,@BankAccount,@MaxDebts
		,@PreDeposit,@CommissionCount,@Saler,@SaleId,@CustomerLev
		,@CommissionType,@CompanyId,@IsEnable,@ContactName,@Phone
		,@Mobile,@Fax,@Remark,@OperatorId,getdate()
		,0,@BrandId,@AccountDay,@AccountWay,@AccountDayType
		,@JieSuanType,@IsRequiredTourCode)
	SELECT  @CustomerId =@@IDENTITY 

	if @CustomerInfoXML is null or len(@CustomerInfoXML)<10
	begin
		set @Result=1 
		return @Result
	end

-----开始 事务
 BEGIN TRANSACTION addCustomer

 EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@CustomerInfoXML
        
	---游标开始
	DECLARE @cAreaIds nvarchar(300),@cSex char(1),@cContactName varchar(200),@cMobile varchar(200),@cqq varchar(200),@cBirthDay varchar(200),@cEmail varchar(200),@cDepartment varchar(200),@cJob varchar(500),@cusername varchar(50),@cPwd nvarchar(50),@cPwdMd5 nvarchar(50),@cContactId int,@cTel varchar(200),@cFax nvarchar(255)--定义游标查询字段
	DECLARE contact_cursor CURSOR FOR  ---声明游标
	SELECT AreaIds,Sex,ContactName,Mobile,qq,BirthDay,Email,Job,Department,username,Pwd,Md5,ContactId,Tel,Fax  FROM OPENXML(@hdoc,'/ROOT/CustomerInfo')
	WITH(AreaIds varchar(250),Sex char(1),ContactName varchar(200),Mobile varchar(200),qq varchar(200),BirthDay varchar(200),Email varchar(200),Job varchar(200),Department varchar(500),username varchar(50),Pwd nvarchar(50),Md5 nvarchar(50),ContactId int,Tel varchar(200),Fax nvarchar(255))
	OPEN contact_cursor----打开游标
	FETCH NEXT FROM contact_cursor----遍历游标
	INTO  @cAreaIds,@cSex,@cContactName,@cMobile,@cqq,@cBirthDay,@cEmail,@cJob,@cDepartment,@cusername,@cPwd,@cPwdMd5,@cContactId,@cTel,@cFax
	WHILE @@FETCH_STATUS = 0
	begin		
		if  @cContactId=0 ---新增联系人
		SET @newUserId=0
		begin
			if @cusername is not null and @cusername <>''---添加用户
			begin
				declare @IsUserExist int
				set @IsUserExist=0---判断用户是否存在
				select @IsUserExist=count(id) from tbl_CompanyUser where UserName=@cusername and CompanyId=@CompanyId AND IsDelete='0'
				if @IsUserExist<=0---用户不存在可以添加
				begin
					INSERT INTO  tbl_CompanyUser 
					(UserName,[Password],MD5Password,roleid,SuperviseDepartId,TourCompanyId,CompanyId,SuperviseDepartName,contactname
					,contactsex,contactmobile,contactemail,qq,jobname,departname,usertype,ContactTel,ContactFax,[IsEnable])
					values(@cusername,@cPwd,@cPwdMd5,0,0,@CustomerId,@CompanyId,'',@cContactName
					,@cSex,@cMobile,@cEmail,@cqq,@cJob,@cDepartment,1,@cTel,@cFax,@IsEnable)
					select @newUserId=@@identity
					if @cAreaIds is not null and len(@cAreaIds)>0 and @newUserId>0---插入注册用户和区域关系
					BEGIN
						INSERT INTO tbl_UserArea(userid,areaid)(select @newUserId,value from fn_split(@cAreaIds,','))
					END
				end --用户不存在可以添加结束
			end --添加用户结束
			INSERT INTO [tbl_CustomerContactInfo] ---添加联系人
				([CustomerId],[UserId],[Sex],[Name],[Mobile],[qq] ,[BirthDay],[Email],JobId,DepartmentId ,[companyId],[Tel],[Fax])
			values(@CustomerId,@newUserId,@cSex,@cContactName,@cMobile,@cqq,@cBirthDay,@cEmail,@cJob,@cDepartment,@CompanyId,@cTel,@cFax)
		end  ---新增联系人结束
		if @@error>0
			set @errorCount =@errorCount+1
		FETCH NEXT FROM contact_cursor
		INTO  @cAreaIds,@cSex,@cContactName,@cMobile,@cqq,@cBirthDay,@cEmail,@cJob,@cDepartment,@cusername,@cPwd,@cPwdMd5,@cContactId,@cTel,@cFax
	end
	CLOSE contact_cursor;---关闭游标
	DEALLOCATE contact_cursor---清除游标
	EXECUTE sp_xml_removedocument @hdoc---删除xml句柄

	if @errorCount >0 
		rollback transaction addCustomer
	else
		commit transaction addCustomer

	SET @Result=1
	return @Result
END
GO

-- =============================================
-- Author:		<李焕超,>
-- Create date: <Create 2011-1-21,,>
---Update date:2011-02-17 添加MD5
-- Description:	<添加客户,,>
-- History:
-- 1.2011-05-26 汪奇志 结算日期
-- 2.2011-06-07 汪奇志 结算方式 结算日类型
-- 3.2012-08-26 汪奇志 增加@JieSuanType，@IsRequiredTourCode
-- =============================================
ALTER PROCEDURE  [dbo].[proc_Customer_Update]
(
	@CustomerId int----编号
	,@ProviceId  int ---省份编号
	,@ProvinceName nvarchar(255)---省份
	,@CityId int ---城市编号
	,@CityName nvarchar(255)---城市名称
	,@Name nvarchar(30)---客户单位名称
	,@Licence nvarchar(30)---许可证号
	,@Adress nvarchar(50)---地址
	,@PostalCode nvarchar(10)---邮编
	,@BankAccount nvarchar(255)---银行账号
	,@MaxDebts money ---最高欠款金额
	,@PreDeposit money ---预存款
	,@CommissionCount money---返佣金额
	,@Saler nvarchar(50)--销售员
	,@SaleId int--销售员编号
	,@CustomerLev int----客户等级
	,@CommissionType tinyint ---返佣类型
	,@CompanyId int--公司编号
	,@ContactName nvarchar(255)---联系人
	,@Phone nvarchar(255)---电话
	,@Mobile nvarchar(255)---手机
	,@Fax nvarchar(255)--传真
	,@Remark nvarchar(500)---备注
	,@OperatorId int---操作员编号
	,@IsEnable char(1)
	,@IssueTime datetime
	,@IsDelete char(1)
	,@BrandId int --专线给此客户分配的品牌编号
	,@CustomerInfoXML NVARCHAR(MAX) ---联系人列表<root><AreaIds=  username= Pwd= Sex= ContactName= Mobile= qq= BirthDay= Email= Job= Department= ContactId= Md5= /><root>
	,@Result int output --输出参数 0失败，1成功，9用户已存在
	,@AccountDay TINYINT --结算日期	 
	,@AccountWay NVARCHAR(255)--结算方式
	,@AccountDayType TINYINT--结算日类型
	,@JieSuanType TINYINT--结算方式
	,@IsRequiredTourCode CHAR(1)--对方团号是否必填
)
as
BEGIN

	DECLARE @hdoc INT
	declare @newUserId int ---新创建用户的编号
	declare @IsUserExist int ---判断用户的编号
	declare @IsUserExistCount int --累计存在用户数
	declare @errorCount int --错误次数
	set @newUserId =0
	set @IsUserExist=0
	set @IsUserExistCount =0
	set @errorCount=0
  
 --------------------------------------------------------添加用户开始
--------------------------------------------------------添加客户开始
begin 

	update [tbl_Customer] set [ProviceId]=@ProviceId,[ProvinceName]=@ProvinceName,[CityId]=@CityId,[CityName]=@CityName,[Name]=@Name
		,[Licence]=@Licence,[Adress]=@Adress,[PostalCode]=@PostalCode,[BankAccount]=@BankAccount,[MaxDebts]=@MaxDebts
		,[PreDeposit]=@PreDeposit,[CommissionCount]=@CommissionCount,[Saler]=@Saler,[SaleId]=@SaleId,[CustomerLev]=@CustomerLev
		,[CommissionType]=@CommissionType,[CompanyId]=@CompanyId,[ContactName]=@ContactName,[Phone]=@Phone,[Mobile]=@Mobile
		,[Fax]=@Fax,[Remark]=@Remark,[OperatorId]=@OperatorId,[IssueTime]=@IssueTime,[BrandId]=@BrandId
		,[AccountDay]=@AccountDay,[AccountWay]=@AccountWay,[AccountDayType]=@AccountDayType,[JieSuanType]=@JieSuanType,[IsRequiredTourCode]=@IsRequiredTourCode
	where id=@CustomerId
end
--------------------------------------------------------添加客户结束
-----未执行xml开始
if @CustomerInfoXML is null or len(@CustomerInfoXML)<10
begin
    set @Result=1 
    return @Result
 end
-----未执行xml结束
-----开始 事务
 BEGIN TRANSACTION addCustomer

 EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@CustomerInfoXML

        
---游标开始
DECLARE @cAreaIds nvarchar(300),@cSex char(1),@cContactName varchar(200),@cMobile varchar(200),@cqq varchar(200),@cBirthDay varchar(200),@cEmail varchar(200),
            @cJob varchar(200),@cDepartment varchar(500),@cusername varchar(50),@cPwd nvarchar(50),@cPwdMd5 nvarchar(100),@cContactId int,@cTel varchar(200),@cFax nvarchar(255)--定义游标查询字段
DECLARE contact_cursor CURSOR FOR  ---声明游标
SELECT AreaIds,Sex,ContactName,Mobile,qq,BirthDay,Email,Job,Department,username,Pwd,Md5,ContactId,Tel,Fax  FROM OPENXML(@hdoc,'/ROOT/CustomerInfo')
	   WITH(AreaIds varchar(250),Sex char(1),ContactName varchar(200),Mobile varchar(200),qq varchar(200),BirthDay varchar(200),Email varchar(200),Job varchar(200),
            Department varchar(200),username varchar(50),Pwd nvarchar(50),Md5 nvarchar(100),ContactId int,Tel varchar(200),Fax nvarchar(255))
OPEN contact_cursor----打开游标
FETCH NEXT FROM contact_cursor----遍历游标
INTO  @cAreaIds,@cSex,@cContactName,@cMobile,@cqq,@cBirthDay,@cEmail,@cJob,@cDepartment,@cusername,@cPwd,@cPwdMd5,@cContactId,@cTel,@cFax


WHILE @@FETCH_STATUS = 0
begin
    if  @cContactId=0 ---新增联系人
    begin	
		SET @newUserId=0
      if @cusername is not null and @cusername <>''---添加用户
        begin
          set @IsUserExist=0---判断用户是否存在
          select @IsUserExist=count(id) from tbl_CompanyUser where UserName=@cusername and CompanyId=@CompanyId AND IsDelete='0'
          if @IsUserExist<=0---用户不存在可以添加
           begin
               INSERT INTO  tbl_CompanyUser 
               (UserName,Password,MD5Password,roleid,SuperviseDepartId,TourCompanyId,CompanyId,SuperviseDepartName,contactname,contactsex,contactmobile,contactemail,qq,jobname,departname,usertype,ContactTel,ContactFax)
               values(@cusername,@cPwd,@cPwdMd5,0,0,@CustomerId,@CompanyId,'',@cContactName,@cSex,@cMobile,@cEmail,@cqq,@cJob,@cDepartment,1,@cTel,@cFax)
              select @newUserId=@@identity
            end 
           else
           set @IsUserExistCount =@IsUserExistCount+1
      end
		

      INSERT INTO [tbl_CustomerContactInfo] ---添加联系人
      ([CustomerId],[UserId],[Sex],[Name],[Mobile],[qq] ,[BirthDay],[Email] ,[JobId],[DepartmentId],[companyId],[Tel],[Fax])
      values(@CustomerId,@newUserId,@cSex,@cContactName,@cMobile,@cqq,@cBirthDay,@cEmail,@cJob,@cDepartment,@CompanyId,@cTel,@cFax)

      if @cAreaIds is not null and len(@cAreaIds)>0 and @newUserId>0---插入注册用户和区域关系
      BEGIN
          INSERT INTO tbl_UserArea(userid,areaid)(select @newUserId,value from fn_split(@cAreaIds,','))
      END
    end
    else ---修改联系人 新增或修改用户
    begin---修改联系人开始
		set @newUserId=0
      if @cusername is not null and len(@cusername)>0 and len(@cPwd)>0 ---当填写用户名和密码是可以新增或修改用户
      begin
		  set @IsUserExist=0---判断用户是否存在
		  select  @IsUserExist=count(id) from tbl_CompanyUser where UserName=@cusername and CompanyId=@CompanyId AND IsDelete='0'
		  if @IsUserExist<=0---用户不存在可以添加
		   begin
               INSERT INTO  tbl_CompanyUser 
               (UserName,Password,MD5Password,roleid,SuperviseDepartId,TourCompanyId,CompanyId,SuperviseDepartName,contactname,contactsex,contactmobile,contactemail,qq,jobname,departname,usertype,ContactTel,ContactFax)
               values(@cusername,@cPwd,@cPwdMd5,0,0,@CustomerId,@CompanyId,'',@cContactName,@cSex,@cMobile,@cEmail,@cqq,@cJob,@cDepartment,1,@cTel,@cFax)
              select @newUserId=@@identity
            end            
		  else---修改用户
		  begin
			select @newUserId=id from tbl_CompanyUser where username=@cusername and usertype=1 and CompanyId=@CompanyId
			update tbl_CompanyUser set Password=@cPwd,MD5Password=@cPwdMd5,ContactName=@cContactName,
contactsex=@cSex,contactmobile=@cMobile,contactemail=@cEmail,qq=@cqq,jobname=@cJob,departname=@cDepartment,ContactTel=@cTel,ContactFax = @cFax where username=@cusername and usertype=1 and CompanyId=@CompanyId---修改已存在用户密码
			
		  end
     end--判断是否填写用户名密码结束
      
      
      
      update[tbl_CustomerContactInfo] ---修改联系人
      set [UserId]=@newUserId,[Sex]=@cSex,[Name]=@cContactName,[Mobile]=@cMobile,[qq]=@cqq ,[BirthDay]=@cBirthDay,[Email]=@cEmail,
          [JobId]=@cJob,[DepartmentId]=@cDepartment,[Tel]=@cTel,[Fax] = @cFax
      where id=@cContactId

     delete from tbl_UserArea where userid=@newUserId---先删除再建立关系
     if @cAreaIds is not null and len(@cAreaIds)>0 and @newUserId>0---插入注册用户和区域关系
      BEGIN 
          INSERT INTO tbl_UserArea(userid,areaid)(select @newUserId,value from fn_split(@cAreaIds,','))
      END  
    end---修改联系人结束 
if @@error>0
set @errorCount =@errorCount+1
FETCH NEXT FROM contact_cursor
INTO  @cAreaIds,@cSex,@cContactName,@cMobile,@cqq,@cBirthDay,@cEmail,@cJob,@cDepartment,@cusername,@cPwd,@cPwdMd5,@cContactId,@cTel,@cFax
end
CLOSE contact_cursor;---关闭游标
DEALLOCATE contact_cursor---清除游标
EXECUTE sp_xml_removedocument @hdoc---删除xml句柄

if @errorCount >0 
rollback transaction addCustomer
else
commit transaction addCustomer

--if @IsUserExistCount >0 --用户存在返回9
-- begin
--   set @Result=9
--   return @Result
-- end

SET @Result=1
return @Result
END
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2012-08-26
-- Description:	取消机票审核
-- =============================================
CREATE PROCEDURE proc_PlanTicketQuXiaoShenHe
	 @TicketId CHAR(36)--机票申请编号
	,@RetCode INT OUTPUT--操作结果：1:成功,-1:非审核通过状态下的机票申请不存在取消审核操作,-2:团队已提交财务不可取消审核
AS
BEGIN
	DECLARE @TourId CHAR(36)--团队编号
	DECLARE @TourStatus TINYINT--团队状态
	DECLARE @TicketStatus TINYINT--机票申请状态

	SELECT @TourId = TourId,@TicketStatus = [State] FROM tbl_PlanTicketOut WHERE ID = @TicketId
	SELECT @TourStatus = [Status] FROM tbl_Tour where TourId = @TourId

	--机票状态 =2审核通过
	IF(@TicketStatus<>2)
	BEGIN
		SET @RetCode=-1
		RETURN @RetCode
	END

	--团队状态 =4财务核算 =5核算结果
	IF(@TourStatus IN (4,5))
	BEGIN
		SET @RetCode=-2
		RETURN @RetCode
	END

	--机票状态 =1机票申请
	UPDATE [tbl_Planticket] SET [State]=1,VerifyTime=GETDATE() WHERE [RefundId]=@TicketId
	UPDATE [tbl_PlanTicketOut] SET [State]=1,VerifyTime=GETDATE() WHERE [Id]=@TicketId

	SET @RetCode=1
	RETURN @RetCode
END
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2012-08-26
-- Description:	团款支出按支出类别视图
-- =============================================
CREATE VIEW view_FinJiDiaoZhiChu
AS
--地接安排支出
SELECT 1 AS ZhiChuLeiBie
	,A.Id AS AnPaiId
	,A.TravelAgencyID AS GYSId
	,A.LocalTravelAgency AS GYSName
	,A.TotalAmount AS ZhiChuJinE
	,A.OperateTime AS AnPaiTime
	,(SELECT ISNULL(SUM(C.[PaymentAmount]),0) FROM tbl_FinancialPayInfo AS C WHERE C.ItemType=0 AND C.[ReceiveId]=A.Id AND C.[ReceiveType]=1) AS YiDengJiJinE
	,(SELECT ISNULL(SUM(C.[PaymentAmount]),0) FROM tbl_FinancialPayInfo AS C WHERE C.ItemType=0 AND C.[ReceiveId]=A.Id AND C.[ReceiveType]=1 AND C.IsPay='1') AS YiZhiFuJinE
	,B.TourId
	,B.RouteName
	,B.LeaveDate
	,B.CompanyId
	,B.TourCode
	,B.TourType	
FROM tbl_PlanLocalAgency AS A INNER JOIN tbl_Tour AS B
ON A.TourId=B.TourId

UNION ALL

--机票安排支出
SELECT 2 AS ZhiChuLeiBie
	,A.Id AS AnPaiId
	,A.TicketOfficeId AS GYSId
	,A.TicketOffice AS GYSName
	,A.TotalAmount AS ZhiChuJinE
	,A.RegisterTime AS AnPaiTime
	,(SELECT ISNULL(SUM(C.[PaymentAmount]),0) FROM tbl_FinancialPayInfo AS C WHERE C.ItemType=0 AND C.[ReceiveId]=A.Id AND C.[ReceiveType]=2) AS YiDengJiJinE
	,(SELECT ISNULL(SUM(C.[PaymentAmount]),0) FROM tbl_FinancialPayInfo AS C WHERE C.ItemType=0 AND C.[ReceiveId]=A.Id AND C.[ReceiveType]=2 AND C.IsPay='1') AS YiZhiFuJinE
	,B.TourId
	,B.RouteName
	,B.LeaveDate
	,B.CompanyId
	,B.TourCode
	,B.TourType	
FROM tbl_PlanTicketOut AS A INNER JOIN tbl_Tour AS B
ON A.TourId=B.TourId
WHERE A.[State]=3
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2012-08-24
-- Description:	按支出类别批量登记支出
-- =============================================
CREATE PROCEDURE proc_FinPiLiangDengJiZhiChu
	 @CompanyId INT--公司编号
	,@OperatorId INT--操作员编号
	,@FKTime DATETIME--付款时间
	,@FKRenId INT--付款人编号
	,@FKRenName NVARCHAR(255)--付款人姓名
	,@FKFangShi TINYINT--付款方式
	,@BeiZhu NVARCHAR(255)--备注
	,@AnPaiIdsXML NVARCHAR(255)--计调安排编号XML:<root><info anpaiid="计调安排编号" /></root>
	,@RetCode INT OUTPUT--操作结果 1成功 其它失败
AS
BEGIN
	DECLARE @errorcount INT
	DECLARE @hdoc INT

	SET @errorcount=0	

	BEGIN TRAN

	EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@AnPaiIdsXML

	INSERT INTO [tbl_FinancialPayInfo]([Id],[CompanyID],[ItemId],[ItemType],[ReceiveId]
		,[ReceiveType],[ReceiveCompanyId],[ReceiveCompanyName],[PaymentDate],[StaffNo]
		,[StaffName],[PaymentAmount],[PaymentType],[IsBill],[BillAmount]
		,[BillNo],[Remark],[IsChecked],[OperatorID],[IssueTime]
		,[CheckerId],[IsPay])
	SELECT NEWID(),@CompanyId,A.TourId,0,A.AnPaiId
		,A.ZhiChuLeiBie,A.GYSId,A.GysName,@FKTime,@FKRenId
		,@FKRenName,(A.ZhiChuJinE-YiDengJiJinE),@FKFangShi,'0',0
		,'',@BeiZhu,'0',@OperatorId,GETDATE()
		,0,'0'
	FROM view_FinJiDiaoZhiChu AS A WHERE A.CompanyId=@CompanyId
	AND A.AnPaiId IN(SELECT anpaiid FROM OPENXML(@hdoc,'/root/info') WITH(anpaiid CHAR(36)))
	AND (A.ZhiChuJinE-YiDengJiJinE)<>0
	SET @errorcount=@errorcount+@@ERROR	

	EXECUTE sp_xml_removedocument @hdoc

	IF(@errorcount<>0)
	BEGIN
		ROLLBACK TRAN
		SET @RetCode=0
		RETURN @RetCode
	END

	COMMIT TRAN
	SET @RetCode=1
	RETURN @RetCode
END
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2012-08-26
-- Description:	取消机票审核
-- =============================================
ALTER PROCEDURE [dbo].[proc_PlanTicketQuXiaoShenHe]
	 @TicketId CHAR(36)--机票申请编号
	,@RetCode INT OUTPUT--操作结果：1:成功,-1:非审核通过状态下的机票申请不存在取消审核操作,-2:团队已提交财务不可取消审核
	,@TourId CHAR(36) OUTPUT--团队编号
AS
BEGIN
	--DECLARE @TourId CHAR(36)--团队编号
	DECLARE @TourStatus TINYINT--团队状态
	DECLARE @TicketStatus TINYINT--机票申请状态

	SELECT @TourId = TourId,@TicketStatus = [State] FROM tbl_PlanTicketOut WHERE ID = @TicketId
	SELECT @TourStatus = [Status] FROM tbl_Tour where TourId = @TourId

	--机票状态 =2审核通过
	IF(@TicketStatus<>2)
	BEGIN
		SET @RetCode=-1
		RETURN @RetCode
	END

	--团队状态 =4财务核算 =5核算结果
	IF(@TourStatus IN (4,5))
	BEGIN
		SET @RetCode=-2
		RETURN @RetCode
	END

	--机票状态 =1机票申请
	UPDATE [tbl_Planticket] SET [State]=1,VerifyTime=GETDATE() WHERE [RefundId]=@TicketId
	UPDATE [tbl_PlanTicketOut] SET [State]=1,VerifyTime=GETDATE() WHERE [Id]=@TicketId

	SET @RetCode=1
	RETURN @RetCode
END
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2012-08-24
-- Description:	团队利润统计视图
-- =============================================
ALTER VIEW [dbo].[view_TongJiTuanDuiLiRun]
AS
SELECT A.TourId,A.LeaveDate,A.AreaId,A.CompanyId,A.TotalOtherIncome,(A.TotalExpenses + A.TotalOtherExpenses) AS ZhiChuJinE,A.DistributionAmount
	,B.Id AS OrderId,B.SalerId,B.FinanceSum,(B.PeopleNumber-B.LeaguePepoleNum) AS RenShu
	,C.ProviceId AS ProvinceId,C.CityId
	,D.DepartId,D.ContactName AS XiaoShouYuanName
	,1 AS TuanDuiShu
FROM tbl_Tour AS A INNER JOIN tbl_TourOrder AS B
ON A.TourId=B.TourId INNER JOIN tbl_Customer AS C
ON B.BuyCompanyId=C.Id INNER JOIN tbl_CompanyUser AS D
ON B.SalerId=D.Id
WHERE A.IsDelete='0' AND A.TourType=1
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2012-08-24
-- Description:	按支出类别批量登记支出
-- =============================================
ALTER PROCEDURE [dbo].[proc_FinPiLiangDengJiZhiChu]
	 @CompanyId INT--公司编号
	,@OperatorId INT--操作员编号
	,@FKTime DATETIME--付款时间
	,@FKRenId INT--付款人编号
	,@FKRenName NVARCHAR(255)--付款人姓名
	,@FKFangShi TINYINT--付款方式
	,@BeiZhu NVARCHAR(255)--备注
	,@AnPaiIdsXML NVARCHAR(MAX)--计调安排编号XML:<root><info anpaiid="计调安排编号" /></root>
	,@RetCode INT OUTPUT--操作结果 1成功 其它失败
AS
BEGIN
	DECLARE @errorcount INT
	DECLARE @hdoc INT

	SET @errorcount=0	

	BEGIN TRAN

	EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@AnPaiIdsXML

	INSERT INTO [tbl_FinancialPayInfo]([Id],[CompanyID],[ItemId],[ItemType],[ReceiveId]
		,[ReceiveType],[ReceiveCompanyId],[ReceiveCompanyName],[PaymentDate],[StaffNo]
		,[StaffName],[PaymentAmount],[PaymentType],[IsBill],[BillAmount]
		,[BillNo],[Remark],[IsChecked],[OperatorID],[IssueTime]
		,[CheckerId],[IsPay])
	SELECT NEWID(),@CompanyId,A.TourId,0,A.AnPaiId
		,A.ZhiChuLeiBie,A.GYSId,A.GysName,@FKTime,@FKRenId
		,@FKRenName,(A.ZhiChuJinE-YiDengJiJinE),@FKFangShi,'0',0
		,'',@BeiZhu,'0',@OperatorId,GETDATE()
		,0,'0'
	FROM view_FinJiDiaoZhiChu AS A WHERE A.CompanyId=@CompanyId
	AND A.AnPaiId IN(SELECT anpaiid FROM OPENXML(@hdoc,'/root/info') WITH(anpaiid CHAR(36)))
	AND (A.ZhiChuJinE-YiDengJiJinE)<>0
	SET @errorcount=@errorcount+@@ERROR	

	EXECUTE sp_xml_removedocument @hdoc

	IF(@errorcount<>0)
	BEGIN
		ROLLBACK TRAN
		SET @RetCode=0
		RETURN @RetCode
	END

	COMMIT TRAN
	SET @RetCode=1
	RETURN @RetCode
END
GO

--以上日志已更新 2012-09-06 173000 汪奇志
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2012-08-24
-- Description:	收款登记视图
-- =============================================
ALTER VIEW [dbo].[view_FinShouKuanDengJi]
AS
SELECT A.Id
	,A.CompanyId,A.ItemId AS OrderId,A.RefundDate,A.RefundMoney,A.RefundType,A.IsCheck,A.Remark,A.StaffName
	,B.OrderNo,B.TourId,B.BuyCompanyName,B.BuyCompanyId,B.OrderState
	,C.TourCode,C.RouteName,C.LeaveDate
FROM tbl_ReceiveRefund AS A INNER JOIN tbl_TourOrder AS B
ON A.ItemId=B.Id AND B.IsDelete='0' INNER JOIN tbl_Tour AS C
ON C.TourId=B.TourId AND C.IsDelete='0'
WHERE A.IsReceive=1 AND A.ItemType=1
GO
--以上日志已更新 2012-09-07 130000 汪奇志
