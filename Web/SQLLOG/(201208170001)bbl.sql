GO
--2012-08-17 ����־ ���ӷ�Ʊ�Ǽ���Ϣ��
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ǼǱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��˾���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ͻ���λ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'CrmId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'RiQi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊ���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'JinE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ʊ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'PiaoHao'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊ�˱��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'KaiPiaoRenId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'KaiPiaoRen'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��ע' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'BeiZhu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����˱��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'CaoZuoRenId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����ʱ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'IssueTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƿ�ɾ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao', @level2type=N'COLUMN',@level2name=N'IsDelete'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�������-��Ʊ�Ǽ���Ϣ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_FinFaPiao'
GO

-- =============================================
-- Author:		���ĳ�
-- Create date: 2010-01-21
-- Description:	�˴�ͳ�ƻ�����ͼ
-- ���ĳ� 2011-06-20���ӳ���������
-- 2011-12-20 ����־ �˴�ͳ������Ա�����˴�ͳ������Ա��
-- 2012-08-22 ����־ ����[tbl_TourOrder].[TourId]
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
-- Author:		����־
-- Create date: 2012-08-24
-- Description:	�տ�Ǽ���ͼ
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

--2012-08-24 ����־ ���˿�ǼǱ��������ʱ��
ALTER TABLE dbo.tbl_ReceiveRefund ADD
	ShenHeTime datetime NULL
GO
DECLARE @v sql_variant 
SET @v = N'���ʱ��'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_ReceiveRefund', N'COLUMN', N'ShenHeTime'
GO

-- =============================================
-- Author:		����־
-- Create date: 2012-08-24
-- Description:	�Ŷ�����ͳ����ͼ
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

--����־ �����ο���Ϣ���������Ŷӱ��
ALTER TABLE dbo.tbl_TourOrderCustomer ADD
	TourId char(36) NULL
GO
DECLARE @v sql_variant 
SET @v = N'�Ŷӱ��'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOrderCustomer', N'COLUMN', N'TourId'
GO

--����־ 2012-08-24 ͬ�������ο���Ϣ�����Ŷӱ��
UPDATE tbl_TourOrderCustomer SET TourId=(SELECT TourId FROM tbl_TourOrder AS B WHERE B.Id=A.OrderId)
FROM tbl_TourOrderCustomer AS A
GO

-- =============================================
-- Author:		luofx
-- Create date: 2011-01-23
-- Description:	�ŶӼƻ�-�����ο���Ϣ
-- History:
-- 1.����־ 2012-01-12 ��ʱ���ֶ�[Vid]����ɱ�ʶ��
-- 2.����־ 2012-08-24 �����Ŷӱ�ŵ�д��
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
	DECLARE @TourId CHAR(36)--�Ŷӱ��
	SELECT @TourId=TourId FROM tbl_TourOrder WHERE Id=@OrderId
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
					[CradType],[CradNumber],[Sex],[VisitorType],[ContactTel],[TourId])
					VALUES(@ID,@CompanyId,@OrderId,@VisitorName,
						@CradType,@CradNumber,@Sex,@VisitorType,@ContactTel,@TourId)
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

-- =============================================
-- Author:		luofx
-- Create date: 2011-01-23
-- Description:	�ŶӼƻ�-�޸�(����)�ο���Ϣ   
-- History:
-- 1.����־ 2012-01-12 ��ʱ���ֶ�[Vid]����ɱ�ʶ��
-- 2.����־ 2012-08-24 �����Ŷӱ�ŵ�д��
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
	DECLARE @TourId CHAR(36)--�Ŷӱ��
	SELECT @TourId=TourId FROM tbl_TourOrder WHERE Id=@OrderId
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
									[CradType],[CradNumber],[Sex],[VisitorType],[ContactTel],[TourId])
								VALUES(@ID,@CompanyId,@OrderId,@VisitorName,
									@CradType,@CradNumber,@Sex,@VisitorType,@ContactTel,@TourId)
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
-- Author:		����־
-- Create date: 2011-07-06
-- Description:	ͬ������ѯ���ο���Ϣ���ŶӼƻ�����
-- History:
-- 1.����־ 2012-08-24 �����Ŷӱ�ŵ�д��
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
-- Author:		����־
-- Create date: 2011-01-21
-- Description:	д��ɢƴ�ƻ����ŶӼƻ������������Ϣ
-- History:
-- 1.2011-04-18 ���ӵؽӱ������������ۼ����籨������������
-- 2.2011-06-08 �ŶӼƻ��������ӷ�Ӷ
-- 3.2011-07-11 �ŶӼƻ����������ͼ۸���Ϣ��ɾ��SelfUnitPriceAmount
-- 4.2011-12-20 ���Ӷ�����Ա�������˴�ͳ�ƣ��ֶεĴ���
-- 5.2012-04-09 ����־ ����������ŵĴ���
-- 6.2012-08-24 ����־ �����ο���Ϣ�������Ŷӱ�ŵ�д��
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
			IF(@errorcount=0)--����������
			BEGIN
				EXECUTE proc_Tour_IsRecently @TourId=@TourId,@IsT=N'1'
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
				,[CustomerStatus],[TourId])
			SELECT A.TravelerId,@OrderId,@CompanyId,A.TravelerName,A.IdType
				,A.IdNo,A.Gender,A.TravelerType,A.ContactTel,GETDATE()
				,0,@TourId FROM OPENXML(@hdoc,'/ROOT/Info')
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
-- Author:		����־
-- Create date: 2011-01-23
-- Description:	����ɢƴ�ƻ����ŶӼƻ������������Ϣ
-- History:
-- 1.2011-07-11 �ŶӼƻ����������ͼ۸���Ϣ��ɾ��SelfUnitPriceAmount
-- 2.2012-04-09 ����־ ����������Ŵ���
-- 3.2012-07-30 ����־ �޸ļƻ�ͬ���޸Ķ�������·������Ϣ
-- 4.2012-08-24 ����־ �����ο���Ϣ�������Ŷӱ�ŵ�д��
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
			IF(@errorcount=0)--����������
			BEGIN
				EXECUTE proc_Tour_IsRecently @TourId=@TourId,@IsT=N'0'
			END
		END

		IF(@errorcount=0)--ͬ����������·����
		BEGIN
			UPDATE tbl_TourOrder SET AreaId=@AreaId WHERE TourId=@TourId
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
				,[CustomerStatus],[TourId])
			SELECT A.TravelerId,@OrderId,@CompanyId,A.TravelerName,A.IdType
				,A.IdNo,A.Gender,A.TravelerType,A.ContactTel,GETDATE()
				,0,@TourId FROM OPENXML(@hdoc,'/ROOT/Info')
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

-- =============================================
-- Author:		����־
-- Create date: 2011-03-21
-- Description:	ɢ�����췢�ƻ�����ɢƴ�ƻ���������(������������������״̬�����췢�ƻ��Ѵ�������ά����δ��������ά����)
-- History:
-- 1.2012-08-24 ����־ �����ο���Ϣ�������Ŷӱ�ŵ�д��
-- =============================================
ALTER PROCEDURE [dbo].[proc_TourEveryday_BuildTourFollow]
	@TourId CHAR(36),--ɢƴ�ƻ��ƻ����
	@EverydayTourId CHAR(36),--ɢ�����췢�ƻ����
	@EverydayTourApplyId CHAR(36),--ɢ�����췢�ƻ�������
	@Result INT OUTPUT,--��ֵʱ�ɹ�����ֵ��0ʱʧ��
	@ApplyCompanyId INT OUTPUT--�ͻ���λ���
AS
BEGIN	
	DECLARE @errorcount INT	
	DECLARE @OrderId CHAR(36)--�������
	DECLARE @SellerId INT --����Ա���
	DECLARE @SellerName NVARCHAR(255)--����Ա����
	DECLARE @BuyerCompanyName NVARCHAR(255)--�ͻ���λ����	
	DECLARE @StandardId INT--���۵ȼ����
	DECLARE @LevelId INT--�ͻ��ȼ����
	DECLARE @AdultPrice MONEY--���˼۸�
	DECLARE @ChildrenPrice MONEY--��ͯ�۸�
	DECLARE @AdultNumber INT--������
	DECLARE @ChildrenNumber INT--��ͯ��
	DECLARE @Amount MONEY--�������
	DECLARE @TravellerDisplayType TINYINT--�ο���Ϣ��������
	DECLARE @TravellerFilePath NVARCHAR(255)--�ο���Ϣ����·��
	DECLARE @CompanyId INT--��˾(ר��)���
	DECLARE @BuildTourOperatorId INT--�ƻ�������
	DECLARE @AreaId INT--��·������
	DECLARE @TourType TINYINT--�ƻ�����
	DECLARE @ApplyUserId INT--�����˱��
	DECLARE @ApplyUserContactName NVARCHAR(255)--����������
	DECLARE @ApplyUserContactTelephone NVARCHAR(255)--�����˵绰
	DECLARE @ApplyUserContactMobile NVARCHAR(255)--�������ֻ�
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
	--���ɶ���
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

	IF(@errorcount=0)--�������Ӽ��ٷ���
	BEGIN
		INSERT INTO [tbl_TourOrderPlus](OrderId,AddAmount,ReduceAmount,Remark) VALUES(@OrderId,0,0,'')
		SET @errorcount=@errorcount+@@ERROR	
	END

	IF(@errorcount=0)--�ο���Ϣ
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

	IF(@errorcount=0)--д����
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

	IF(@errorcount=0)--���봦��״̬ά�� 
	BEGIN
		UPDATE [tbl_TourEverydayApply] SET [HandleStatus]=1,[HandleOperatorId]=@BuildTourOperatorId,[HandleTime]=GETDATE(),[BuildTourId]=@TourId
		WHERE [ApplyId]=@EverydayTourApplyId
		SET @errorcount=@errorcount+@@ERROR		
	END
	
	IF(@errorcount=0)--ɢ�����췢�ƻ��Ѵ���������δ��������ά��
	BEGIN
		DECLARE @HandleNumber INT --�Ѵ�������
		DECLARE @UntreatedNumber INT --δ��������
		SELECT @HandleNumber=COUNT(*) FROM [tbl_TourEverydayApply] WHERE [TourId]=@EverydayTourId AND [HandleStatus]=1
		SELECT @UntreatedNumber=COUNT(*) FROM [tbl_TourEverydayApply] WHERE [TourId]=@EverydayTourId AND [HandleStatus]=0

		UPDATE [tbl_TourEveryday] SET HandleNumber=@HandleNumber,UntreatedNumber=@UntreatedNumber WHERE [TourId]=@EverydayTourId
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount=0)--ά���Ŷ�����ʵ����=����������(������ס���ڡ�ȡ���Ķ���)
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

--����־ 2012-08-24 [tbl_TourOrderCustomer].[TourId] NOT NULL
ALTER TABLE [tbl_TourOrderCustomer]
ALTER COLUMN [TourId] CHAR(36) NOT NULL
GO

--����־ 2012-08-26 [tbl_Customer]����[JieSuanType]��[IsRequiredTourCode]�ֶ�
ALTER TABLE dbo.tbl_Customer ADD
	JieSuanType tinyint NOT NULL CONSTRAINT DF_tbl_Customer_JieSuanType DEFAULT 0,
	IsRequiredTourCode char(1) NOT NULL CONSTRAINT DF_tbl_Customer_IsRequiredTourCode DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'���㷽ʽ���½ᡢ���Ž��㣩'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Customer', N'COLUMN', N'JieSuanType'
GO
DECLARE @v sql_variant 
SET @v = N'�Է��ź��Ƿ����'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Customer', N'COLUMN', N'IsRequiredTourCode'
GO

-- =============================================
-- Author:		<�����,>
-- Create date: <Create 2011-1-21,,>
-- Description:	<��ӿͻ�,,>
-- History:
-- 1.2011-05-26 ����־ ��������
-- 2.2011-06-07 ����־ ���㷽ʽ ����������
-- 3.2012-08-26 ����־ ����@JieSuanType��@IsRequiredTourCode
-- =============================================
ALTER PROCEDURE  [dbo].[proc_Customer_Insert]
(
	@ProviceId  int ---ʡ�ݱ��
	,@ProvinceName nvarchar(255)---ʡ��
	,@CityId int ---���б��
	,@CityName nvarchar(255)---��������
	,@Name nvarchar(30)---�ͻ���λ����
	,@Licence nvarchar(30)---���֤��
	,@Adress nvarchar(50)---��ַ
	,@PostalCode nvarchar(10)---�ʱ�
	,@BankAccount nvarchar(255)---�����˺�
	,@MaxDebts money ---���Ƿ����
	,@PreDeposit money ---Ԥ���
	,@CommissionCount money---��Ӷ���
	,@Saler nvarchar(50)--����Ա
	,@SaleId int--����Ա���
	,@BrandId int --ר�߸��˿ͻ������Ʒ�Ʊ��
	,@CustomerLev int----�ͻ��ȼ�
	,@CommissionType tinyint ---��Ӷ����
	,@CompanyId int--��˾���
	,@ContactName nvarchar(255)---��ϵ��
	,@Phone nvarchar(255)---�绰
	,@Mobile nvarchar(255)---�ֻ�
	,@Fax nvarchar(255)--����
	,@Remark nvarchar(500)---��ע
	,@OperatorId int---����Ա���
	,@CustomerInfoXML NVARCHAR(MAX) ---��ϵ���б�
	,@Result int output --������� 9 �����벻�����˺ŵ���ϵ�� 2�������û� 8 ,δִ��xml	 
	,@AccountDay TINYINT--��������,
	,@IsEnable CHAR(1)--�Ƿ�����,
	,@AccountWay NVARCHAR(255)--���㷽ʽ
	,@AccountDayType TINYINT--����������
	,@JieSuanType TINYINT--���㷽ʽ
	,@IsRequiredTourCode CHAR(1)--�Է��ź��Ƿ����
)
as
BEGIN
	declare @CustomerId int 
	DECLARE @hdoc INT
	declare @newUserId int ---�´����û��ı��
	declare @errorCount int --�������
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

-----��ʼ ����
 BEGIN TRANSACTION addCustomer

 EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@CustomerInfoXML
        
	---�α꿪ʼ
	DECLARE @cAreaIds nvarchar(300),@cSex char(1),@cContactName varchar(200),@cMobile varchar(200),@cqq varchar(200),@cBirthDay varchar(200),@cEmail varchar(200),@cDepartment varchar(200),@cJob varchar(500),@cusername varchar(50),@cPwd nvarchar(50),@cPwdMd5 nvarchar(50),@cContactId int,@cTel varchar(200),@cFax nvarchar(255)--�����α��ѯ�ֶ�
	DECLARE contact_cursor CURSOR FOR  ---�����α�
	SELECT AreaIds,Sex,ContactName,Mobile,qq,BirthDay,Email,Job,Department,username,Pwd,Md5,ContactId,Tel,Fax  FROM OPENXML(@hdoc,'/ROOT/CustomerInfo')
	WITH(AreaIds varchar(250),Sex char(1),ContactName varchar(200),Mobile varchar(200),qq varchar(200),BirthDay varchar(200),Email varchar(200),Job varchar(200),Department varchar(500),username varchar(50),Pwd nvarchar(50),Md5 nvarchar(50),ContactId int,Tel varchar(200),Fax nvarchar(255))
	OPEN contact_cursor----���α�
	FETCH NEXT FROM contact_cursor----�����α�
	INTO  @cAreaIds,@cSex,@cContactName,@cMobile,@cqq,@cBirthDay,@cEmail,@cJob,@cDepartment,@cusername,@cPwd,@cPwdMd5,@cContactId,@cTel,@cFax
	WHILE @@FETCH_STATUS = 0
	begin		
		if  @cContactId=0 ---������ϵ��
		SET @newUserId=0
		begin
			if @cusername is not null and @cusername <>''---����û�
			begin
				declare @IsUserExist int
				set @IsUserExist=0---�ж��û��Ƿ����
				select @IsUserExist=count(id) from tbl_CompanyUser where UserName=@cusername and CompanyId=@CompanyId AND IsDelete='0'
				if @IsUserExist<=0---�û������ڿ������
				begin
					INSERT INTO  tbl_CompanyUser 
					(UserName,[Password],MD5Password,roleid,SuperviseDepartId,TourCompanyId,CompanyId,SuperviseDepartName,contactname
					,contactsex,contactmobile,contactemail,qq,jobname,departname,usertype,ContactTel,ContactFax,[IsEnable])
					values(@cusername,@cPwd,@cPwdMd5,0,0,@CustomerId,@CompanyId,'',@cContactName
					,@cSex,@cMobile,@cEmail,@cqq,@cJob,@cDepartment,1,@cTel,@cFax,@IsEnable)
					select @newUserId=@@identity
					if @cAreaIds is not null and len(@cAreaIds)>0 and @newUserId>0---����ע���û��������ϵ
					BEGIN
						INSERT INTO tbl_UserArea(userid,areaid)(select @newUserId,value from fn_split(@cAreaIds,','))
					END
				end --�û������ڿ�����ӽ���
			end --����û�����
			INSERT INTO [tbl_CustomerContactInfo] ---�����ϵ��
				([CustomerId],[UserId],[Sex],[Name],[Mobile],[qq] ,[BirthDay],[Email],JobId,DepartmentId ,[companyId],[Tel],[Fax])
			values(@CustomerId,@newUserId,@cSex,@cContactName,@cMobile,@cqq,@cBirthDay,@cEmail,@cJob,@cDepartment,@CompanyId,@cTel,@cFax)
		end  ---������ϵ�˽���
		if @@error>0
			set @errorCount =@errorCount+1
		FETCH NEXT FROM contact_cursor
		INTO  @cAreaIds,@cSex,@cContactName,@cMobile,@cqq,@cBirthDay,@cEmail,@cJob,@cDepartment,@cusername,@cPwd,@cPwdMd5,@cContactId,@cTel,@cFax
	end
	CLOSE contact_cursor;---�ر��α�
	DEALLOCATE contact_cursor---����α�
	EXECUTE sp_xml_removedocument @hdoc---ɾ��xml���

	if @errorCount >0 
		rollback transaction addCustomer
	else
		commit transaction addCustomer

	SET @Result=1
	return @Result
END
GO

-- =============================================
-- Author:		<�����,>
-- Create date: <Create 2011-1-21,,>
---Update date:2011-02-17 ���MD5
-- Description:	<��ӿͻ�,,>
-- History:
-- 1.2011-05-26 ����־ ��������
-- 2.2011-06-07 ����־ ���㷽ʽ ����������
-- 3.2012-08-26 ����־ ����@JieSuanType��@IsRequiredTourCode
-- =============================================
ALTER PROCEDURE  [dbo].[proc_Customer_Update]
(
	@CustomerId int----���
	,@ProviceId  int ---ʡ�ݱ��
	,@ProvinceName nvarchar(255)---ʡ��
	,@CityId int ---���б��
	,@CityName nvarchar(255)---��������
	,@Name nvarchar(30)---�ͻ���λ����
	,@Licence nvarchar(30)---���֤��
	,@Adress nvarchar(50)---��ַ
	,@PostalCode nvarchar(10)---�ʱ�
	,@BankAccount nvarchar(255)---�����˺�
	,@MaxDebts money ---���Ƿ����
	,@PreDeposit money ---Ԥ���
	,@CommissionCount money---��Ӷ���
	,@Saler nvarchar(50)--����Ա
	,@SaleId int--����Ա���
	,@CustomerLev int----�ͻ��ȼ�
	,@CommissionType tinyint ---��Ӷ����
	,@CompanyId int--��˾���
	,@ContactName nvarchar(255)---��ϵ��
	,@Phone nvarchar(255)---�绰
	,@Mobile nvarchar(255)---�ֻ�
	,@Fax nvarchar(255)--����
	,@Remark nvarchar(500)---��ע
	,@OperatorId int---����Ա���
	,@IsEnable char(1)
	,@IssueTime datetime
	,@IsDelete char(1)
	,@BrandId int --ר�߸��˿ͻ������Ʒ�Ʊ��
	,@CustomerInfoXML NVARCHAR(MAX) ---��ϵ���б�<root><AreaIds=  username= Pwd= Sex= ContactName= Mobile= qq= BirthDay= Email= Job= Department= ContactId= Md5= /><root>
	,@Result int output --������� 0ʧ�ܣ�1�ɹ���9�û��Ѵ���
	,@AccountDay TINYINT --��������	 
	,@AccountWay NVARCHAR(255)--���㷽ʽ
	,@AccountDayType TINYINT--����������
	,@JieSuanType TINYINT--���㷽ʽ
	,@IsRequiredTourCode CHAR(1)--�Է��ź��Ƿ����
)
as
BEGIN

	DECLARE @hdoc INT
	declare @newUserId int ---�´����û��ı��
	declare @IsUserExist int ---�ж��û��ı��
	declare @IsUserExistCount int --�ۼƴ����û���
	declare @errorCount int --�������
	set @newUserId =0
	set @IsUserExist=0
	set @IsUserExistCount =0
	set @errorCount=0
  
 --------------------------------------------------------����û���ʼ
--------------------------------------------------------��ӿͻ���ʼ
begin 

	update [tbl_Customer] set [ProviceId]=@ProviceId,[ProvinceName]=@ProvinceName,[CityId]=@CityId,[CityName]=@CityName,[Name]=@Name
		,[Licence]=@Licence,[Adress]=@Adress,[PostalCode]=@PostalCode,[BankAccount]=@BankAccount,[MaxDebts]=@MaxDebts
		,[PreDeposit]=@PreDeposit,[CommissionCount]=@CommissionCount,[Saler]=@Saler,[SaleId]=@SaleId,[CustomerLev]=@CustomerLev
		,[CommissionType]=@CommissionType,[CompanyId]=@CompanyId,[ContactName]=@ContactName,[Phone]=@Phone,[Mobile]=@Mobile
		,[Fax]=@Fax,[Remark]=@Remark,[OperatorId]=@OperatorId,[IssueTime]=@IssueTime,[BrandId]=@BrandId
		,[AccountDay]=@AccountDay,[AccountWay]=@AccountWay,[AccountDayType]=@AccountDayType,[JieSuanType]=@JieSuanType,[IsRequiredTourCode]=@IsRequiredTourCode
	where id=@CustomerId
end
--------------------------------------------------------��ӿͻ�����
-----δִ��xml��ʼ
if @CustomerInfoXML is null or len(@CustomerInfoXML)<10
begin
    set @Result=1 
    return @Result
 end
-----δִ��xml����
-----��ʼ ����
 BEGIN TRANSACTION addCustomer

 EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@CustomerInfoXML

        
---�α꿪ʼ
DECLARE @cAreaIds nvarchar(300),@cSex char(1),@cContactName varchar(200),@cMobile varchar(200),@cqq varchar(200),@cBirthDay varchar(200),@cEmail varchar(200),
            @cJob varchar(200),@cDepartment varchar(500),@cusername varchar(50),@cPwd nvarchar(50),@cPwdMd5 nvarchar(100),@cContactId int,@cTel varchar(200),@cFax nvarchar(255)--�����α��ѯ�ֶ�
DECLARE contact_cursor CURSOR FOR  ---�����α�
SELECT AreaIds,Sex,ContactName,Mobile,qq,BirthDay,Email,Job,Department,username,Pwd,Md5,ContactId,Tel,Fax  FROM OPENXML(@hdoc,'/ROOT/CustomerInfo')
	   WITH(AreaIds varchar(250),Sex char(1),ContactName varchar(200),Mobile varchar(200),qq varchar(200),BirthDay varchar(200),Email varchar(200),Job varchar(200),
            Department varchar(200),username varchar(50),Pwd nvarchar(50),Md5 nvarchar(100),ContactId int,Tel varchar(200),Fax nvarchar(255))
OPEN contact_cursor----���α�
FETCH NEXT FROM contact_cursor----�����α�
INTO  @cAreaIds,@cSex,@cContactName,@cMobile,@cqq,@cBirthDay,@cEmail,@cJob,@cDepartment,@cusername,@cPwd,@cPwdMd5,@cContactId,@cTel,@cFax


WHILE @@FETCH_STATUS = 0
begin
    if  @cContactId=0 ---������ϵ��
    begin	
		SET @newUserId=0
      if @cusername is not null and @cusername <>''---����û�
        begin
          set @IsUserExist=0---�ж��û��Ƿ����
          select @IsUserExist=count(id) from tbl_CompanyUser where UserName=@cusername and CompanyId=@CompanyId AND IsDelete='0'
          if @IsUserExist<=0---�û������ڿ������
           begin
               INSERT INTO  tbl_CompanyUser 
               (UserName,Password,MD5Password,roleid,SuperviseDepartId,TourCompanyId,CompanyId,SuperviseDepartName,contactname,contactsex,contactmobile,contactemail,qq,jobname,departname,usertype,ContactTel,ContactFax)
               values(@cusername,@cPwd,@cPwdMd5,0,0,@CustomerId,@CompanyId,'',@cContactName,@cSex,@cMobile,@cEmail,@cqq,@cJob,@cDepartment,1,@cTel,@cFax)
              select @newUserId=@@identity
            end 
           else
           set @IsUserExistCount =@IsUserExistCount+1
      end
		

      INSERT INTO [tbl_CustomerContactInfo] ---�����ϵ��
      ([CustomerId],[UserId],[Sex],[Name],[Mobile],[qq] ,[BirthDay],[Email] ,[JobId],[DepartmentId],[companyId],[Tel],[Fax])
      values(@CustomerId,@newUserId,@cSex,@cContactName,@cMobile,@cqq,@cBirthDay,@cEmail,@cJob,@cDepartment,@CompanyId,@cTel,@cFax)

      if @cAreaIds is not null and len(@cAreaIds)>0 and @newUserId>0---����ע���û��������ϵ
      BEGIN
          INSERT INTO tbl_UserArea(userid,areaid)(select @newUserId,value from fn_split(@cAreaIds,','))
      END
    end
    else ---�޸���ϵ�� �������޸��û�
    begin---�޸���ϵ�˿�ʼ
		set @newUserId=0
      if @cusername is not null and len(@cusername)>0 and len(@cPwd)>0 ---����д�û����������ǿ����������޸��û�
      begin
		  set @IsUserExist=0---�ж��û��Ƿ����
		  select  @IsUserExist=count(id) from tbl_CompanyUser where UserName=@cusername and CompanyId=@CompanyId AND IsDelete='0'
		  if @IsUserExist<=0---�û������ڿ������
		   begin
               INSERT INTO  tbl_CompanyUser 
               (UserName,Password,MD5Password,roleid,SuperviseDepartId,TourCompanyId,CompanyId,SuperviseDepartName,contactname,contactsex,contactmobile,contactemail,qq,jobname,departname,usertype,ContactTel,ContactFax)
               values(@cusername,@cPwd,@cPwdMd5,0,0,@CustomerId,@CompanyId,'',@cContactName,@cSex,@cMobile,@cEmail,@cqq,@cJob,@cDepartment,1,@cTel,@cFax)
              select @newUserId=@@identity
            end            
		  else---�޸��û�
		  begin
			select @newUserId=id from tbl_CompanyUser where username=@cusername and usertype=1 and CompanyId=@CompanyId
			update tbl_CompanyUser set Password=@cPwd,MD5Password=@cPwdMd5,ContactName=@cContactName,
contactsex=@cSex,contactmobile=@cMobile,contactemail=@cEmail,qq=@cqq,jobname=@cJob,departname=@cDepartment,ContactTel=@cTel,ContactFax = @cFax where username=@cusername and usertype=1 and CompanyId=@CompanyId---�޸��Ѵ����û�����
			
		  end
     end--�ж��Ƿ���д�û����������
      
      
      
      update[tbl_CustomerContactInfo] ---�޸���ϵ��
      set [UserId]=@newUserId,[Sex]=@cSex,[Name]=@cContactName,[Mobile]=@cMobile,[qq]=@cqq ,[BirthDay]=@cBirthDay,[Email]=@cEmail,
          [JobId]=@cJob,[DepartmentId]=@cDepartment,[Tel]=@cTel,[Fax] = @cFax
      where id=@cContactId

     delete from tbl_UserArea where userid=@newUserId---��ɾ���ٽ�����ϵ
     if @cAreaIds is not null and len(@cAreaIds)>0 and @newUserId>0---����ע���û��������ϵ
      BEGIN 
          INSERT INTO tbl_UserArea(userid,areaid)(select @newUserId,value from fn_split(@cAreaIds,','))
      END  
    end---�޸���ϵ�˽��� 
if @@error>0
set @errorCount =@errorCount+1
FETCH NEXT FROM contact_cursor
INTO  @cAreaIds,@cSex,@cContactName,@cMobile,@cqq,@cBirthDay,@cEmail,@cJob,@cDepartment,@cusername,@cPwd,@cPwdMd5,@cContactId,@cTel,@cFax
end
CLOSE contact_cursor;---�ر��α�
DEALLOCATE contact_cursor---����α�
EXECUTE sp_xml_removedocument @hdoc---ɾ��xml���

if @errorCount >0 
rollback transaction addCustomer
else
commit transaction addCustomer

--if @IsUserExistCount >0 --�û����ڷ���9
-- begin
--   set @Result=9
--   return @Result
-- end

SET @Result=1
return @Result
END
GO

-- =============================================
-- Author:		����־
-- Create date: 2012-08-26
-- Description:	ȡ����Ʊ���
-- =============================================
CREATE PROCEDURE proc_PlanTicketQuXiaoShenHe
	 @TicketId CHAR(36)--��Ʊ������
	,@RetCode INT OUTPUT--���������1:�ɹ�,-1:�����ͨ��״̬�µĻ�Ʊ���벻����ȡ����˲���,-2:�Ŷ����ύ���񲻿�ȡ�����
AS
BEGIN
	DECLARE @TourId CHAR(36)--�Ŷӱ��
	DECLARE @TourStatus TINYINT--�Ŷ�״̬
	DECLARE @TicketStatus TINYINT--��Ʊ����״̬

	SELECT @TourId = TourId,@TicketStatus = [State] FROM tbl_PlanTicketOut WHERE ID = @TicketId
	SELECT @TourStatus = [Status] FROM tbl_Tour where TourId = @TourId

	--��Ʊ״̬ =2���ͨ��
	IF(@TicketStatus<>2)
	BEGIN
		SET @RetCode=-1
		RETURN @RetCode
	END

	--�Ŷ�״̬ =4������� =5������
	IF(@TourStatus IN (4,5))
	BEGIN
		SET @RetCode=-2
		RETURN @RetCode
	END

	--��Ʊ״̬ =1��Ʊ����
	UPDATE [tbl_Planticket] SET [State]=1,VerifyTime=GETDATE() WHERE [RefundId]=@TicketId
	UPDATE [tbl_PlanTicketOut] SET [State]=1,VerifyTime=GETDATE() WHERE [Id]=@TicketId

	SET @RetCode=1
	RETURN @RetCode
END
GO

-- =============================================
-- Author:		����־
-- Create date: 2012-08-26
-- Description:	�ſ�֧����֧�������ͼ
-- =============================================
CREATE VIEW view_FinJiDiaoZhiChu
AS
--�ؽӰ���֧��
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

--��Ʊ����֧��
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
-- Author:		����־
-- Create date: 2012-08-24
-- Description:	��֧����������Ǽ�֧��
-- =============================================
CREATE PROCEDURE proc_FinPiLiangDengJiZhiChu
	 @CompanyId INT--��˾���
	,@OperatorId INT--����Ա���
	,@FKTime DATETIME--����ʱ��
	,@FKRenId INT--�����˱��
	,@FKRenName NVARCHAR(255)--����������
	,@FKFangShi TINYINT--���ʽ
	,@BeiZhu NVARCHAR(255)--��ע
	,@AnPaiIdsXML NVARCHAR(255)--�Ƶ����ű��XML:<root><info anpaiid="�Ƶ����ű��" /></root>
	,@RetCode INT OUTPUT--������� 1�ɹ� ����ʧ��
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
-- Author:		����־
-- Create date: 2012-08-26
-- Description:	ȡ����Ʊ���
-- =============================================
ALTER PROCEDURE [dbo].[proc_PlanTicketQuXiaoShenHe]
	 @TicketId CHAR(36)--��Ʊ������
	,@RetCode INT OUTPUT--���������1:�ɹ�,-1:�����ͨ��״̬�µĻ�Ʊ���벻����ȡ����˲���,-2:�Ŷ����ύ���񲻿�ȡ�����
	,@TourId CHAR(36) OUTPUT--�Ŷӱ��
AS
BEGIN
	--DECLARE @TourId CHAR(36)--�Ŷӱ��
	DECLARE @TourStatus TINYINT--�Ŷ�״̬
	DECLARE @TicketStatus TINYINT--��Ʊ����״̬

	SELECT @TourId = TourId,@TicketStatus = [State] FROM tbl_PlanTicketOut WHERE ID = @TicketId
	SELECT @TourStatus = [Status] FROM tbl_Tour where TourId = @TourId

	--��Ʊ״̬ =2���ͨ��
	IF(@TicketStatus<>2)
	BEGIN
		SET @RetCode=-1
		RETURN @RetCode
	END

	--�Ŷ�״̬ =4������� =5������
	IF(@TourStatus IN (4,5))
	BEGIN
		SET @RetCode=-2
		RETURN @RetCode
	END

	--��Ʊ״̬ =1��Ʊ����
	UPDATE [tbl_Planticket] SET [State]=1,VerifyTime=GETDATE() WHERE [RefundId]=@TicketId
	UPDATE [tbl_PlanTicketOut] SET [State]=1,VerifyTime=GETDATE() WHERE [Id]=@TicketId

	SET @RetCode=1
	RETURN @RetCode
END
GO

-- =============================================
-- Author:		����־
-- Create date: 2012-08-24
-- Description:	�Ŷ�����ͳ����ͼ
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
-- Author:		����־
-- Create date: 2012-08-24
-- Description:	��֧����������Ǽ�֧��
-- =============================================
ALTER PROCEDURE [dbo].[proc_FinPiLiangDengJiZhiChu]
	 @CompanyId INT--��˾���
	,@OperatorId INT--����Ա���
	,@FKTime DATETIME--����ʱ��
	,@FKRenId INT--�����˱��
	,@FKRenName NVARCHAR(255)--����������
	,@FKFangShi TINYINT--���ʽ
	,@BeiZhu NVARCHAR(255)--��ע
	,@AnPaiIdsXML NVARCHAR(MAX)--�Ƶ����ű��XML:<root><info anpaiid="�Ƶ����ű��" /></root>
	,@RetCode INT OUTPUT--������� 1�ɹ� ����ʧ��
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

--������־�Ѹ��� 2012-09-06 173000 ����־
GO

-- =============================================
-- Author:		����־
-- Create date: 2012-08-24
-- Description:	�տ�Ǽ���ͼ
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
--������־�Ѹ��� 2012-09-07 130000 ����־
