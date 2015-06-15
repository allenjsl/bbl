-- =============================================
-- Author:		<Author,,Name>
-- Create date: 2012-02-24
-- Description:	��ѯ�������ҳ�洢����
-- =============================================
CREATE PROCEDURE [dbo].[proc_Pading_BySqlTable]
(    
    @PageSize     int = 10,           -- ҳ�ߴ�
    @PageIndex    int = 1,            -- ҳ��
    @TableName      nvarchar(max),       -- ������
    @FieldsList	  nvarchar(2000),	      --����
    @FieldSearchKey     nvarchar(2000) = '', -- ��ѯ���� (ע��: ��Ҫ�� where)
    @OrderString      nvarchar(1000),       -- ����  
    @IsGroupBy   char(1) = '0',    --�����Ƿ����
    @GroupString      nvarchar(1000)=''       -- ��������  
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
	SET @OrderString = ' ORDER BY ' + @OrderString
ELSE
	SET @OrderString = ''
------------------------��������--------------------------
IF @GroupString IS NOT NULL AND @GroupString != '' AND @IsGroupBy IS NOT NULL AND @IsGroupBy = '1'
	SET @GroupString = ' GROUP BY ' + @GroupString
ELSE
	SET @GroupString = ''
------------------------��ѯ����--------------------------
if @FieldSearchKey IS NOT NULL AND @FieldSearchKey != ''--�Ƿ���ڲ�ѯ����
	SET @strTmp = 'SELECT @RecordCount=COUNT(*) FROM (' + @TableName + ') AS A WHERE ' + @FieldSearchKey + @GroupString
ELSE
	SET @strTmp = 'SELECT @RecordCount=COUNT(*) FROM (' + @TableName + ') AS A ' + @GroupString

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
SET @strSQL = 'SELECT t120.* FROM ( SELECT  ' + @FieldsList + ',ROW_NUMBER() OVER(' + @OrderString + ') AS RowNumber FROM (' + @TableName + ') AS A ' + @strTmp + @GroupString + ' ) AS t120 WHERE RowNumber BETWEEN ' + Cast(@PageLowerBound as nvarchar) + ' AND ' + Cast(@PageUpperBound as nvarchar)
select @RecordCount as RecordCount
--PRINT @strSQL

EXECUTE(@strSQL)
GO

-- =============================================
-- Author:		����־
-- Create date: 2012-02-24
-- Description:	ͳ�Ʒ���-�������-���ͻ���λͳ��
-- =============================================
CREATE VIEW view_ZhangLingAnKeHuDanWei
AS
SELECT A.Id--�ͻ���λ���
	,A.Name--�ͻ���λ����
	,A.CompanyId--ϵͳ��˾���
	,(SELECT ISNULL(SUM(FinanceSum-HasCheckMoney),0) FROM tbl_TourOrder AS B WHERE B.IsDelete='0' AND B.OrderState NOT IN(3,4) AND B.BuyCompanyId=A.Id) AS WeiShouKuan--δ�տ�
	,(SELECT MIN(IssueTime) FROM tbl_TourOrder AS C WHERE C.IsDelete='0' AND C.OrderState NOT IN(3,4) AND C.BuyCompanyId=A.Id) AS EarlyTime--����δ�տ��µ�ʱ��
	FROM tbl_Customer AS A
WHERE A.IsDelete='0'
GO

--�����������������ź� ����־ 2012-03-01
ALTER TABLE dbo.tbl_TourOrder ADD
	BuyerTourCode nvarchar(255) NULL
GO
DECLARE @v sql_variant 
SET @v = N'�������ź�'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOrder', N'COLUMN', N'BuyerTourCode'
GO

-- =============================================
-- Author:		luofx
-- Create date: 2011-01-24
-- Description:	��������
-- History:
-- 1.����������ϵ����Ϣ����Ӷ��Ϣ ����־ 2011-06-08
-- 2.����������֤����ȥ������������
-- 3.2011-12-20 ����־ ���Ӷ�����Ա�������˴�ͳ�ƣ��ֶεĴ���
-- 4.2012-03-01 ����־ �����������ź�
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
	,@BuyerTourCode NVARCHAR(255)--�������ź�
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
-- Author:		luofx
-- Create date: 2011-01-24
-- Description:	�޸Ķ���
-- History:
-- 1.����������ϵ����Ϣ����Ӷ��Ϣ ����־ 2011-06-08
-- 2.�����޸��ŶӼƻ�����ͬ���޸��ŶӼƻ���Ϣ������������֤����ȥ������������
-- 3.�޸Ŀͻ���λ(BuyCompanyName,BuyCompanyID,SalerId,SalerName,CommissionType)  �ܺ��� 2012-02-02
-- 4.2012-03-01 ����־ �����������ź�
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
	,@BuyerTourCode NVARCHAR(255)--�������ź�
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
				,BuyCompanyName=@BuyCompanyName,SalerId=@SalerId,SalerName=@SalerName,CommissionType=@CommissionType,BuyCompanyID=@BuyCompanyID,BuyerTourCode=@BuyerTourCode
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

-- =============================================
-- Author:		���ĳ�
-- Create date: 2011-04-19
-- Description:	�ͻ���ϵ����-����ͳ����ͼ
-- History:
-- 1.2012-01-13 ����־ ����������ȥ��������
-- 2.2012-01-30 ����־ �����˴�ͳ������Ա��ż������С��Է�����Ա��ż�������
-- 3.2012-03-01 ����־ �����µ�ʱ����
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
,IssueTime--�µ�ʱ��
from tbl_TourOrder
where tbl_TourOrder.IsDelete = '0' and tbl_TourOrder.OrderState not in (3,4)
GO

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
select a.Id,a.CustormerId,a.OperatorName,a.OperatorId,a.IssueTime,a.RefundAmount
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
go

-- =============================================
-- Author:		luofx
-- Create date: 2011-01-22
-- Description:	�޸����˿���Ϣ
-- History:
-- 1.2012-03-02 ����־ �����˿��������������DECIMAL�ĳ�MONEY�����޸��޸����ʱ���������BUG 
-- =============================================
ALTER PROCEDURE [dbo].[proc_ReceiveRefund_Update]
	@ReceiveRefundXML NVARCHAR(MAX),  --���˿�XML��ϸ��Ϣ:<ROOT><ReceiveRefund id="" CompanyId="" PayCompanyId="" PayCompanyName="" ItemId="" ItemType="" RefundDate="" StaffNo="" StaffName="" RefundMoney="" RefundType="" IsBill="" BillAmount="" IsReceive="" IsCheck="" Remark="" OperatorID="" CheckerId=""/> </ROOT>
	@IsRecive INT,                    --1�������տ0�������˿� 
	@OrderId CHAR(36),                --�������
	@Result INT output                --�ش�����
AS
DECLARE @ErrorCount INT   --�����ۼƲ���
DECLARE @hdoc INT         --XMLʹ�ò���
DECLARE @i INT            --������
DECLARE @MAXID INT        --�ݴ�����vid
DECLARE @CompanyID INT                  --������˾ID
DECLARE @RefundId CHAR(36)              --���˿���
DECLARE @PayCompanyId INT               --���˾���
DECLARE @PayCompanyName NVARCHAR(250)   --���˾����
DECLARE @ItemId CHAR(36)                --�������
DECLARE @ItemType TINYINT               --������ţ�������ţ��ſ���������֧����ţ�
DECLARE @RefundDate DATETIME            --���˿�����
DECLARE @StaffNo INT                    --Ա�����
DECLARE @StaffName NVARCHAR(250)        --Ա������
DECLARE @RefundMoney MONEY              --���
DECLARE @RefundType TINYINT             --���˿ʽ
DECLARE @IsBill CHAR(1)                 --�Ƿ�Ʊ
DECLARE @BillAmount MONEY               --��Ʊ���
DECLARE @IsReceive TINYINT              --�Ƿ��տ�
DECLARE @IsCheck TINYINT                --�Ƿ����
DECLARE @Remark NVARCHAR(1000)          --��ע
DECLARE @OperatorID INT                 --�����˱��
DECLARE @CheckerId INT                  --����˱��
SET @ErrorCount=0
SET @Result=1
SET @i=1
BEGIN
CREATE TABLE #TmpReceiveRefund_Update(
	   [Vid] INT,
       [Id] CHAR(36),                 --���˿���
       [CompanyID] INT,               --������˾ID
       [PayCompanyId] INT,            --���˾���
       [PayCompanyName] NVARCHAR(250),--���˾����
       [ItemId] CHAR(36),             --�������
       [ItemType] TINYINT,            --������ţ�������ţ��ſ���������֧����ţ�
       [RefundDate] DATETIME,         --���˿�����
       [StaffNo] INT,                 --Ա�����
       [StaffName] NVARCHAR(250),     --Ա������
       [RefundMoney] MONEY,           --���
       [RefundType] TINYINT,          --���˿ʽ
       [IsBill] CHAR(1),              --�Ƿ�Ʊ
       [BillAmount] MONEY,            --��Ʊ���
       [IsReceive] TINYINT,           --�Ƿ��տ�
       [IsCheck] TINYINT,             --�Ƿ����
       [Remark] NVARCHAR(1000),       --��ע
       [OperatorID] INT,              --�����˱��
       [CheckerId] INT                --����˱��
	)
	BEGIN TRANSACTION ReceiveRefund_Update
		IF(@IsRecive=1)
			BEGIN
				SELECT @PayCompanyId=BuyCompanyId,@PayCompanyName=ISNULL(BuyCompanyName,'') FROM tbl_TourOrder WHERE ID=@OrderId
			END
		ELSE
			BEGIN
				SELECT @PayCompanyId=SellCompanyId,@PayCompanyName=ISNULL(SellCompanyName,'') FROM tbl_TourOrder WHERE ID=@OrderId
			END
		--�������˿���Ϣ	
		EXEC sp_xml_preparedocument @hdoc OUTPUT, @ReceiveRefundXML
			INSERT INTO #TmpReceiveRefund_Update([Vid],id,CompanyId,PayCompanyId,PayCompanyName,ItemId,
				ItemType,RefundDate,StaffNo,StaffName,RefundMoney,RefundType,IsBill,BillAmount,IsCheck,
				Remark,OperatorID,CheckerId)
				SELECT ROW_NUMBER() OVER(ORDER BY [Id] DESC) AS [row_id],id,CompanyId,@PayCompanyId,
					@PayCompanyName,ItemId,ItemType,RefundDate,StaffNo,StaffName,RefundMoney,RefundType,
					IsBill,BillAmount,IsCheck,Remark,OperatorID,CheckerId FROM 
				OPENXML(@hdoc,N'/ROOT/ReceiveRefund') 
					WITH(id CHAR(36),CompanyId INT,PayCompanyId INT,PayCompanyName NVARCHAR(250),ItemId CHAR(36),
						ItemType INT,RefundDate DATETIME,StaffNo INT,StaffName NVARCHAR(250),
						RefundMoney MONEY,RefundType INT,IsBill TINYINT,BillAmount MONEY,IsCheck TINYINT,
						Remark NVARCHAR(1000),OperatorID INT,CheckerId INT)
			SET @ErrorCount = @ErrorCount + @@ERROR
		EXEC sp_xml_removedocument @hdoc
		--ȡ�����row_number()֮id
		SELECT @MAXID=MAX(Vid) FROM #TmpReceiveRefund_Update		
		WHILE(@i<=@MAXID AND @ErrorCount=0)
			BEGIN
				SELECT @RefundId=[ID],@RefundDate=RefundDate,@PayCompanyId=PayCompanyId,@PayCompanyName=PayCompanyName,
					@RefundType=RefundType,@IsBill=IsBill,@RefundMoney=RefundMoney,@Remark=Remark,@BillAmount=BillAmount,
					@IsCheck=IsCheck,@StaffName=StaffName,@StaffNo=StaffNo,@OperatorID=OperatorID FROM #TmpReceiveRefund_Update WHERE Vid=@i
				--�������˿���Ϣ
				UPDATE tbl_ReceiveRefund SET [RefundDate]=@RefundDate,[PayCompanyId]=@PayCompanyId,PayCompanyName=@PayCompanyName,
					[RefundType]=@RefundType,[IsBill]=@IsBill,[RefundMoney]=@RefundMoney,[Remark]=@Remark,BillAmount=@BillAmount,
					[IsCheck]=@IsCheck,StaffName=@StaffName,StaffNo=@StaffNo,OperatorID=@OperatorID
					WHERE [Id]=@RefundId
				SET @i = @i + 1
				SET @ErrorCount = @ErrorCount + @@ERROR
			END			
		--�Ƿ�ع�	
		IF(@ErrorCount>0)
			BEGIN
				SET @Result=0
				ROLLBACK TRANSACTION ReceiveRefund_Update
			END
	COMMIT TRANSACTION ReceiveRefund_Update	
	DROP TABLE #TmpReceiveRefund_Update
	RETURN @Result
END
GO

-- =============================================
-- Author:		����־
-- Create date: 2012-03-02
-- Description:	ȡ����Ʊ��Ʊ
-- =============================================
CREATE PROCEDURE proc_PlanTicketQuXiaoTuiPiao
	@CompanyId INT--��˾���
	,@JiPiaoGuanLiId INT--��Ʊ������
	,@RetCode INT OUTPUT--������ 1:�ɹ� ����ʧ��
AS
BEGIN
	SET @RetCode=0
	IF NOT EXISTS(SELECT 1 FROM tbl_PlanTicket WHERE CompanyId=@CompanyId AND Id=@JiPiaoGuanLiId AND TicketType=3)
	BEGIN
		RETURN 0
	END
	
	DECLARE @errorcount INT
	DECLARE @TuiPiaoId CHAR(36)
	SELECT @TuiPiaoId=RefundId FROM tbl_PlanTicket WHERE CompanyId=@CompanyId AND Id=@JiPiaoGuanLiId AND TicketType=3
	SET @errorcount=0

	BEGIN TRAN
	--ɾ����Ʊ������Ϣ
	DELETE FROM tbl_CustomerRefundFlight WHERE RefundId=@TuiPiaoId
	SET @errorcount=@errorcount+@@ERROR

	IF(@errorcount=0)--ɾ����Ʊ��Ϣ
	BEGIN
		DELETE FROM tbl_CustomerRefund WHERE Id=@TuiPiaoId
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount=0)--ɾ����������������
	BEGIN
		DELETE FROM [tbl_TourOtherCost] WHERE [ItemId] =@TuiPiaoId AND [ItemType]=1 AND [CostType]=	0
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount=0)--ɾ����������������
	BEGIN
		DELETE FROM [tbl_StatAllIncome] WHERE [ItemId]=@TuiPiaoId AND [ItemType]=2
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount=0)
	BEGIN
		DELETE FROM tbl_PlanTicketPlanId WHERE JiPiaoGuanLiId=@JiPiaoGuanLiId
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount=0)
	BEGIN
		DELETE FROM tbl_PlanTicket WHERE Id=@JiPiaoGuanLiId
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount>0)
	BEGIN		
		ROLLBACK TRAN
		SET @RetCode=-1
		RETURN -1
	END

	COMMIT TRAN
	SET @RetCode=1
	RETURN 1	
END
GO

--2012-03-02 ����־ ���ӻ�Ʊ������Ʊ�Ƶ���Ź����� �Է����ѯ
CREATE TABLE [dbo].[tbl_PlanTicketPlanId](
	[JiPiaoGuanLiId] [int] NOT NULL,
	[PlanId] [char](36) NOT NULL,
 CONSTRAINT [PK_TBL_PLANTICKETPLANID] PRIMARY KEY CLUSTERED 
(
	[JiPiaoGuanLiId] ASC,
	[PlanId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊ������' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_PlanTicketPlanId', @level2type=N'COLUMN',@level2name=N'JiPiaoGuanLiId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��Ʊ�Ƶ����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_PlanTicketPlanId', @level2type=N'COLUMN',@level2name=N'PlanId'
GO
/****** ����:  ForeignKey [FK_TBL_PLANTICKETPLANID_REFERENCE_TBL_PLANTICKET]    �ű�����: 03/02/2012 17:24:26 ******/
ALTER TABLE [dbo].[tbl_PlanTicketPlanId]  WITH CHECK ADD  CONSTRAINT [FK_TBL_PLANTICKETPLANID_REFERENCE_TBL_PLANTICKET] FOREIGN KEY([JiPiaoGuanLiId])
REFERENCES [dbo].[tbl_PlanTicket] ([Id])
GO
ALTER TABLE [dbo].[tbl_PlanTicketPlanId] CHECK CONSTRAINT [FK_TBL_PLANTICKETPLANID_REFERENCE_TBL_PLANTICKET]
GO

--���»�Ʊ��������Ʊ�����Ƶ���������Ϣ
INSERT INTO tbl_PlanTicketPlanId(JiPiaoGuanLiId,PlanId)
SELECT Id,RefundId FROM tbl_PlanTicket WHERE TicketType=1
GO
INSERT INTO tbl_PlanTicketPlanId(JiPiaoGuanLiId,PlanId)
SELECT DISTINCT A.Id,D.TicketId FROM tbl_PlanTicket AS A INNER JOIN tbl_CustomerRefund AS B ON B.Id=A.RefundId INNER JOIN tbl_CustomerRefundFlight AS C ON C.RefundId=A.RefundId INNER JOIN 
tbl_PlanTicketFlight AS D ON D.Id=C.FlightId
WHERE A.TicketType=3
GO

-- =============================================
-- Author:luofx
-- Create date: 2011-01-18
-- Description:	�ο���Ʊ:������Ʊ��¼���޸��οͱ����Ʊ״̬
-- Remark�����ĳ� 2011-04-20  ɾ������ʱ�䡢�����ء�Ŀ�ĵء������ֶεĲ���
-- History:
-- 1.2012-03-02 ����־ ���ӻ�Ʊ������Ʊ�Ƶ�������Ϣ�Ĺ��� �Է����ѯ
-- =============================================
ALTER PROCEDURE [dbo].[proc_TourOrderCustomer_AddRefund] 
	@Id Char(36),                 --����
	@CustormerId CHAR(36),       --�οͱ��
	@RouteName NVARCHAR(250),  --��·����	
	@OperatorName NVARCHAR(250),      --����������
	@OperatorID INT,		--�����˱��	
	@RefundAmount DECIMAL(10,4),   --�˿���
	@RefundNote NVARCHAR(1000), --��Ʊ��֪
	@OutTourId CHAR(36) OUTPUT,  -- ���ز���	
	@RefundFlightXML nvarchar(max)  --�ο����˺�����ϢXMl  <CustomerRefundFlight_Add RefundId = "0" FlightId = "1" CustomerId = "2" />
AS
DECLARE @MAXID int              --�ݴ����id
DECLARE @ErrorCount int         --��֤����
DECLARE @hdoc int               --XMLʹ�ò���
DECLARE @TourId CHAR(36)        --�Ŷӱ��
DECLARE @TourNO NVARCHAR(250)   --�ź�
DECLARE @OrderId CHAR(36)		--�������
DECLARE @Saler NVARCHAR(200)    --����Ա����
DECLARE @CompanyId INT          --��˾���
SET @ErrorCount=0
SET @OutTourId=''
BEGIN
	BEGIN Transaction TourOrderCustomer_AddRefund 
		--���Ҷ�����Ϣ����Ա���ֶ���Ϣ
		SELECT @TourId=TourId,@OrderId=[id],@Saler=SalerName,@CompanyId=SellCompanyId,@RouteName=RouteName FROM tbl_TourOrder 
			WHERE [id]=(SELECT TOP 1 OrderId FROM tbl_TourOrderCustomer WHERE [Id]=@CustormerId)
		SET @OutTourId=@TourId
		--������Ʊ��¼
		INSERT INTO [tbl_CustomerRefund] ([id],CustormerId,RouteName,
		      OperatorName,OperatorID,RefundAmount,RefundNote)
				VALUES (@Id,@CustormerId,@RouteName,@OperatorName,@OperatorID,@RefundAmount,@RefundNote)
		SET @ErrorCount=@ErrorCount+@@ERROR
		INSERT INTO tbl_PlanTicket(TourId,OrderId,RefundId,TicketType,RouteName,Saler,OperatorId,Operator,RegisterTime,[State],CompanyId)
							VALUES(@TourId,@OrderId,@Id,3,@RouteName,@Saler,@OperatorID,@OperatorName,getdate(),1,@CompanyId)
		SET @ErrorCount=@ErrorCount+@@ERROR		
		
		--д�ο����˺�����Ϣ
		if @RefundFlightXML is not null or len(@RefundFlightXML) > 0
		begin
			EXEC sp_xml_preparedocument @hdoc OUTPUT, @RefundFlightXML
			insert into tbl_CustomerRefundFlight (RefundId,FlightId,CustomerId) select RefundId,FlightId,CustomerId FROM OPENXML(@hdoc,N'/ROOT/CustomerRefundFlight_Add') WITH (RefundId char(36),FlightId int,CustomerId char(36))
			SET @ErrorCount=@ErrorCount+@@ERROR
			EXEC sp_xml_removedocument @hdoc 
		end

		--д��Ʊ������Ʊ�Ƶ���Ź�����
		INSERT INTO tbl_PlanTicketPlanId(JiPiaoGuanLiId,PlanId)
		SELECT DISTINCT A.Id,D.TicketId FROM tbl_PlanTicket AS A INNER JOIN tbl_CustomerRefund AS B ON B.Id=A.RefundId INNER JOIN tbl_CustomerRefundFlight AS C ON C.RefundId=A.RefundId INNER JOIN tbl_PlanTicketFlight AS D ON D.Id=C.FlightId
		WHERE A.RefundId=@Id

		--�Ƿ�ع�
		IF(@ErrorCount>0)
			BEGIN
				SET @OutTourId=''
				ROLLBACK TRANSACTION TourOrderCustomer_AddRefund
			END
		COMMIT TRANSACTION TourOrderCustomer_AddRefund		
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
-- 4.2012-03-02 ����־ ���ӻ�Ʊ������Ʊ�Ƶ�������Ϣ�Ĺ��� �Է����ѯ
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

	IF(@errorcount=0/* AND @State=3*/)--д��Ʊ������Ʊ�Ƶ���Ź�����
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
GO

-- =============================================
-- Author:		<Author,,�����>
-- Create date: <Create Date,2011-02-14,>
-- Description:	<Description,,>
-- History:
-- 1.2012-03-03 ����־ ���ӻ�Ʊ������Ʊ�Ƶ�������Ϣ�Ĺ�����ɾ��
-- =============================================
ALTER PROCEDURE [dbo].[proc_Ticket_Del]
(	@TicketOutId char(36),
    @Result int output
)
AS
BEGIN
   declare @error int 
   set @error=0
  
	begin transaction TicketDel
    begin
     delete from tbl_PlanTicketOutCustomer where TicketOutId= @TicketOutId --ɾ����Ʊ�ͻ�
      if @@error>0
        set @error= @error+@@error
     delete from tbl_PlanTicketFlight where TicketId= @TicketOutId --ɾ������
      if @@error>0
        set @error= @error+@@error
     delete from tbl_PlanTicketKind where TicketId= @TicketOutId ---ɾ��Ʊ��
      if @@error>0
        set @error= @error+@@error
     delete from tbl_planticketout where id =@TicketOutId --ɾ��һ������
      if @@error>0
        set @error= @error+@@error
     delete from tbl_ReceiveRefund where ItemId =@TicketOutId
           if @@error>0
        set @error= @error+@@error
     delete from tbl_planticket where refundid=@TicketOutId
           if @@error>0
        set @error= @error+@@error
	 DELETE FROM tbl_PlanTicketPlanId WHERE PlanId=@TicketOutId
	 SET @error= @error+@@ERROR
    end
    if @error >0
    begin
     rollback transaction TicketDel
     set @Result=-1
     return @Result
    end
---�ɹ�
commit transaction TicketDel
set @Result=1
 return @Result
END
GO

-- =============================================
-- Author:		���ĳ�
-- Create date: 2011-04-25
-- Description:	ɾ���Ŷ���Ϣ������ɾ����
-- Error��0��������
-- Success��1ɾ���ɹ�
-- History:
-- 1.2011-12-16 ����־ �����ſ�֧���Ǽǵ�ɾ��
-- 2.2012-03-03 ����־ ���ӻ�Ʊ������Ʊ�Ƶ�������Ϣ�Ĺ�����ɾ��
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
	DELETE FROM tbl_PlanTicketPlanId WHERE JiPiaoGuanLiId IN(SELECT Id FROM tbl_PlanTicket WHERE TourId=@TourId)
	SET @errorcount = @errorcount + @@ERROR
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

-- =============================================
-- Author:		<�����,>
-- Create date: <Create 2011-1-21,,>
-- Description:	<�����Ʊ,,>
-- History:
-- 1.2011-04-21 ����־ ����������̼���ƱƱ�����Ӱٷֱȡ���������
-- 2.2012-03-03 ����־ ���ӻ�Ʊ������Ʊ�Ƶ�������Ϣ�Ĺ��� �Է����ѯ
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
		
		IF(@errorcount=0/* AND @State=3*/)--д��Ʊ������Ʊ�Ƶ���Ź�����
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
GO

-- =============================================
-- Author:		���ĳ�
-- Create date: 2011-01-26
-- Description:	���ݲ���Id��ȡ�ò����Լ��ò��ŵ������Ӳ��ţ��ݹ飩���û���Ϣ
-- History:
-- 1.����־ 2012-03-05 ȥ���Ƿ�ɾ���Ĳ�ѯ����
-- =============================================
ALTER PROCEDURE [dbo].[proc_CompanyUser_GetUserByDepart]
	@DepartId  int    --����Id
AS
BEGIN
	
	if @DepartId is null or @DepartId < 0
		return;

	WITH SubsCTE
	AS
	(
	  -- Anchor member returns root node
	  SELECT id, DepartName, 0 AS lvl 
	  FROM dbo.tbl_CompanyDepartment
	  WHERE id = @DepartId and IsDelete = '0'

	  UNION ALL

	  -- Recursive member returns next level of children
	  SELECT C.id, C.DepartName, P.lvl + 1
	  FROM SubsCTE AS P
		JOIN dbo.tbl_CompanyDepartment AS C
		  ON C.PrevDepartId = P.id
		where C.IsDelete = '0'
	)
	
	select id from dbo.tbl_CompanyUser where departid in (SELECT id FROM SubsCTE) /*and IsDelete = '0'*/;

END
GO

-- =============================================
-- Author:		���ĳ�
-- Create date: 2011-04-18
-- Description:	��Ʊȡ�����
-- Error��0��������
-- Error��-1δ�ҵ���Ӧ�Ļ�Ʊ����
-- Error��-2�Ŷ�״̬���߻�Ʊ����״̬������ȡ�����
-- Error��-3ȡ����˹����з�������
-- Success��1ȡ����˳ɹ�
-- History:
-- 1.2012-03-12 ����־ ����ɾ����Ʊ������Ʊ�Ƶ�������Ϣ�Ĺ���
-- =============================================
ALTER PROCEDURE  [dbo].[proc_TicketOut_UNChecked]
(
	@TicketId char(36) = '',    --��Ʊ����Id
	@ErrorValue  int = 0 output   --�������
)
as
BEGIN
	/*  
	1��ɾ����Ʊ�����б�tbl_PlanTicket��
	2��ɾ����Ʊ���뺽����Ϣ��tbl_PlanTicketFlight��
	3��ɾ����Ʊ����Ʊ����Ϣ��tbl_PlanTicketKind��
	4��ɾ����Ʊ�����ο͹�ϵ��tbl_PlanTicketOutCustomer��
	5��ɾ����Ʊ���������Ϣ��tbl_PlanTicketOut��
	*/
	
	if @TicketId is null or len(@TicketId) <> 36
	begin
		set @ErrorValue = 0
		return
	end
	
	declare @ErrorCount int    --���������
	declare @TourId char(36)   --�Ŷ�Id
	declare @TourState  tinyint  --�Ŷ�״̬
	declare @TicketState tinyint  --��Ʊ����״̬
	set @ErrorCount = 0
	
	--�����Ҫ����
	select @TourId = TourId,@TicketState = [State] from tbl_PlanTicketOut where ID = @TicketId
	
	if @TourId is null or len(@TourId) < 1 or @TicketState < 0
	begin
		set @ErrorValue = -1
		return
	end
	
	select @TourState = [Status] from tbl_Tour where TourId = @TourId

	--�Ŷӵݽ��������ѳ�Ʊ�Ĳ���ȡ��
	if @TourState in (4,5) or @TicketState in (3)
	begin
		set @ErrorValue = -2
		return
	end

	begin tran UNChecked

	--ɾ����Ʊ������Ʊ�Ƶ�������Ϣ�Ĺ���
	DELETE FROM tbl_PlanTicketPlanId WHERE PlanId=@TicketId
	SET @ErrorCount = @ErrorCount + @@ERROR 
	
	--1��ɾ����Ʊ�����б�tbl_PlanTicket��
	delete from tbl_PlanTicket where RefundId = @TicketId
	--��֤����
	SET @ErrorCount = @ErrorCount + @@ERROR 

	--2��ɾ����Ʊ���뺽����Ϣ��tbl_PlanTicketFlight��
	delete from tbl_PlanTicketFlight where TicketId = @TicketId
	--��֤����
	SET @ErrorCount = @ErrorCount + @@ERROR 

	--3��ɾ����Ʊ����Ʊ����Ϣ��tbl_PlanTicketKind��
	delete from tbl_PlanTicketKind where TicketId = @TicketId
	--��֤����
	SET @ErrorCount = @ErrorCount + @@ERROR 

	--4��ɾ����Ʊ�����ο͹�ϵ��tbl_PlanTicketOutCustomer��
	delete from tbl_PlanTicketOutCustomer where TicketOutId = @TicketId
	--��֤����
	SET @ErrorCount = @ErrorCount + @@ERROR 

	--5��ɾ����Ʊ���������Ϣ��tbl_PlanTicketOut��
	delete from tbl_PlanTicketOut where ID = @TicketId
	--��֤����
	SET @ErrorCount = @ErrorCount + @@ERROR 
	

	IF @ErrorCount > 0 
	BEGIN
		SET @ErrorValue = -3;
		ROLLBACK TRAN UNChecked
	end	
	ELSE
	BEGIN
		SET @ErrorValue = 1;
		COMMIT TRAN UNChecked
	end
end
GO

-- =============================================
-- Author:		����־
-- Create date: 2011-07-04
-- Description:	�����ǼǸ���
-- @ExpenseType:(0:�������Ӧ�̰��ţ�1:�ؽӣ�2:Ʊ��255:����)
-- History:
-- 1.2012-03-13 ����־ ���»�Ʊ����Ǽǵ�����(δ��Ʊ�Ĳ��Ǽ�)
-- =============================================
ALTER PROCEDURE [dbo].[proc_BatchRegisterExpense]
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
			,0,B.SupplierId,B.SupplierName,@PaymentTime,@PayerId
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
			,1,B.TravelAgencyID,B.LocalTravelAgency,@PaymentTime,@PayerId
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
			,2,B.TicketOfficeId,B.TicketOffice,@PaymentTime,@PayerId
			,@Payer,B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=2 AND [State]=3),@PaymentType,'0',0
			,'',@Remark,'0',@OperatorId,GETDATE()
			,0,'0'
		FROM @tmptbl AS A INNER JOIN tbl_PlanTicketOut AS B
		ON A.TourId=B.TourId AND B.[State]=3 AND B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=B.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=2)>0 
	END

	SET @Result=1
	RETURN @Result
END
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
--������־�Ѹ����������� 2012-03-19




--(201202150001)������������.doc 
--2012-03-20 ����־ ������·����������
ALTER TABLE dbo.tbl_Area ADD
	SortId int NOT NULL CONSTRAINT DF_tbl_Area_SortId DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'������'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Area', N'COLUMN', N'SortId'
GO
--=========================================
--��������·�޸�
--�����ˣ�xuqh
--ʱ�䣺2011-02-22
--History:
--1.������·�������� 2011-10-17
--2.����־ 2012-03-20 ����������
--=========================================
ALTER proc [dbo].[proc_UserArea_Update]
	@Id int,	--��·������
	@AreaName nvarchar(200),		--��·��������
	@UserAreaXml nvarchar(max),		--��·���Ƶ�Ա
	--XML:<ROOT><UserAreaInfo UserId="1" AreaId="1" ContactName="����" /></ROOT>
	@Result int output		--���ؽ�� 1�ɹ� 0ʧ��
	--@AreaType TINYINT=0--��·��������
	,@SortId INT=0--������
as
begin
	declare @hdoc int
	set @Result = 0
	
	begin try
		begin transaction tran_update
		--������·��������
		update tbl_Area set AreaName = @AreaName,SortId=@SortId where Id = @Id

		--���¼Ƶ�Ա��Ϣ
		if @UserAreaXml is not null and len(@UserAreaXml)>0
		begin
			--delete from tbl_UserArea where AreaId = @Id
			delete tbl_UserArea from tbl_CompanyUser b where AreaId = @Id and b.UserType != 1 and b.Id= UserID
			
			execute sp_xml_preparedocument @hdoc output,@UserAreaXml
			insert into tbl_UserArea(UserId,AreaId)
			select UserId,AreaId from openxml(@hdoc,'/ROOT/UserAreaInfo')
			WITH(UserId nvarchar(200),AreaId nvarchar(200))
			execute sp_xml_removedocument @hdoc
		end
		commit transaction tran_update
		set @Result = 1
		return @Result 	
	end try
	begin catch
		set @Result = 0
		rollback transaction tran_update
		return @Result
	end catch
end
GO
--======================================
--����:��ȡ��·��Ϣ
--������:xuqh
--ʱ��:2011-02-22
--History:
--1:2012-03-20 ����־ ����������
--======================================
ALTER proc [dbo].[proc_Area_GetAreaInfo]
	@Id int --��·���
as
begin
	--��ȡ��·������Ϣ
	select Id,AreaName,CompanyId,OperatorId,IssueTime,IsDelete,SortId
	from tbl_Area where Id = @Id and IsDelete = '0'
	
	--��ȡ��·�����Ӧ�Ƶ�Ա��Ϣ
	select a.UserId,a.AreaId,b.ContactName from tbl_UserArea a inner join tbl_CompanyUser b
	on a.UserId = b.Id where a.AreaId = @Id and UserType = 2 and IsDelete = '0' and IsEnable = '1'
end
GO
--(201202150001)������������.doc

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
	DECLARE @tmptbl_zhichu TABLE(ZhiChuLeiBie NVARCHAR(10),TourId CHAR(36),PlanId CHAR(36),Amount MONEY,GysId INT,GysName NVARCHAR(255))
	
	EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@TourIdsXML
	INSERT INTO @tmptbl_tour(TourId) SELECT A.TourId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TourId CHAR(36)) AS A
	EXECUTE sp_xml_removedocument @hdoc

	IF(@GysType=255)--����֧��
	BEGIN
		IF(@GysName IS NOT NULL AND LEN(@GysName)>0)
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT '����',A.TourId,B.PlanId,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.PlanId AND [ReceiveType]=0)),B.SupplierId,B.SupplierName FROM @tmptbl_tour AS A INNER JOIN tbl_PlanSingle AS B
			ON A.TourId=B.TourId AND B.SupplierName LIKE'%'+@GysName+'%'
		END
		ELSE
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT '����',A.TourId,B.PlanId,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.PlanId AND [ReceiveType]=0)),B.SupplierId,B.SupplierName FROM @tmptbl_tour AS A INNER JOIN tbl_PlanSingle AS B
			ON A.TourId=B.TourId
		END
	END

	IF(@GysType=1 OR @GysType=255)--�ؽ�֧��
	BEGIN
		IF(@GysName IS NOT NULL AND LEN(@GysName)>0)
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT '�ؽ�',A.TourId,B.Id,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=1)),B.TravelAgencyID,B.LocalTravelAgency FROM @tmptbl_tour AS A INNER JOIN tbl_PlanLocalAgency AS B
			ON A.TourId=B.TourId AND B.LocalTravelAgency LIKE'%'+@GysName+'%'
		END
		ELSE
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT '�ؽ�',A.TourId,B.Id,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=1)),B.TravelAgencyID,B.LocalTravelAgency FROM @tmptbl_tour AS A INNER JOIN tbl_PlanLocalAgency AS B
			ON A.TourId=B.TourId
		END
	END

	IF(@GysType=2 OR @GysType=255)--��Ʊ֧��
	BEGIN
		IF(@GysName IS NOT NULL AND LEN(@GysName)>0)
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT '��Ʊ',A.TourId,B.Id,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=2)),B.TicketOfficeId,B.TicketOffice FROM @tmptbl_tour AS A INNER JOIN tbl_PlanTicketOut AS B
			ON A.TourId=B.TourId AND B.State=3 AND B.TicketOffice LIKE'%'+@GysName+'%'
		END
		ELSE
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT '��Ʊ',A.TourId,B.Id,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=2)),B.TicketOfficeId,B.TicketOffice FROM @tmptbl_tour AS A INNER JOIN tbl_PlanTicketOut AS B
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
		,2,A.GysId,A.GysName,@PaymentTime,@PayerId
		,@Payer,A.Amount,@PaymentType,'0',0
		,'',@Remark,'0',@OperatorId,GETDATE()
		,0,'0'
	FROM @tmptbl_zhichu AS A WHERE A.Amount<>0

	SET @Result=1
	RETURN @Result
END
GO


--ͬ��ƽ̨���� 
--2012-04-01 ����־ ͬ��ƽ̨������Ϣ�������ֶ���ϵ��ʽ����ࣩ
ALTER TABLE dbo.tbl_Site ADD
	LianXiFangShi nvarchar(MAX) NULL
GO
DECLARE @v sql_variant 
SET @v = N'��ϵ��ʽ'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Site', N'COLUMN', N'LianXiFangShi'
GO

--2012-04-03 ����־ ϵͳ�����������ֶ���������
ALTER TABLE dbo.tbl_SysDomain ADD
	Type tinyint NOT NULL CONSTRAINT DF_tbl_SysDomain_Type DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'��������'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_SysDomain', N'COLUMN', N'Type'
GO

--2012-04-05 ����־ �ƻ���Ϣ�������Ƿ���������ֶ�
ALTER TABLE dbo.tbl_Tour ADD
	IsRecently char(1) NOT NULL CONSTRAINT DF_tbl_Tour_IsRecently DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'�Ƿ��������'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'IsRecently'
GO

-- =============================================
-- Author:		����־
-- Create date: 2011-04-23
-- Description:	������ϵͳ
-- =============================================
ALTER PROCEDURE [dbo].[proc_Sys_CreateSys]
	@SysName NVARCHAR(255),--ϵͳ����
	@SysDomainsXML NVARCHAR(MAX),--ϵͳ����XML:<ROOT><Info Domain="����" URL="Ŀ��λ��url" DoimainType"��������" /></ROOT>
	@FirstPermission NVARCHAR(MAX),--ϵͳһ����Ŀ
	@SecondPermission NVARCHAR(MAX),--ϵͳ������Ŀ
	@ThirdPermission NVARCHAR(MAX),--ϵͳȨ��
	@CompanyName NVARCHAR(255),--��˾����
	@Realname NVARCHAR(255),--��ϵ������
	@Telephone NVARCHAR(255),--��ϵ�绰
	@Mobile NVARCHAR(255),--��ϵ�ֻ�
	@Fax NVARCHAR(255),--��ϵ����
	@DepartmentName NVARCHAR(255),--�ܲ���������
	@Username NVARCHAR(255),--����Ա�˺�
	@Password NVARCHAR(255),--����Ա����(����)
	@MD5Password NVARCHAR(255),--����Ա����(MD5)
	@SettingsXML NVARCHAR(MAX),--��˾����XML:<ROOT><Info Key="����KEY" Value="����VALUE" /></ROOT>
	@Result INT OUTPUT,--��� 1:�ɹ� ����ʧ��
	@SysId INT OUTPUT, --ϵͳ���
	@CompanyId INT OUTPUT--��˾���
AS
BEGIN
	DECLARE @errorcount INT
	DECLARE @hdoc INT
	DECLARE @UserId INT--�û����
	DECLARE @DepartmentId INT--���ű��
	DECLARE @RoleId INT--��ɫ���

	
	SET @SysId=0
	SET @CompanyId=0
	SET @Result=0
	SET @errorcount=0

	IF(@SysDomainsXML IS NOT NULL AND LEN(@SysDomainsXML)>0)
	BEGIN
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@SysDomainsXML
		IF EXISTS(SELECT 1 FROM [tbl_SysDomain] WHERE [Domain] IN(SELECT A.Domain FROM OPENXML(@hdoc,'/ROOT/Info') WITH(Domain NVARCHAR(255)) AS A WHERE A.Domain IS NOT NULL OR LEN(A.Domain)>0))--��֤�����ظ�
		BEGIN
			SET @Result=-1
			RETURN -1
		END
		EXECUTE sp_xml_removedocument @hdoc
	END	

	BEGIN TRAN
	--д��ϵͳ��Ϣ
	INSERT INTO [tbl_Sys]([SysName],[Module],[Part],[Permission],[CreateTime])
	VALUES(@SysName,@FirstPermission,@SecondPermission,@ThirdPermission,GETDATE())
	SET @errorcount=@errorcount+@@ERROR
	SET @SysId=@@IDENTITY

	IF(@errorcount=0 AND @SysDomainsXML IS NOT NULL AND LEN(@SysDomainsXML)>0)--д��ϵͳ������Ϣ
	BEGIN
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@SysDomainsXML
		INSERT INTO [tbl_SysDomain]([SysId],[Domain],[Url],[Type])
		SELECT @SysId,A.Domain,A.Url,A.DomainType FROM OPENXML(@hdoc,'/ROOT/Info') WITH(Domain NVARCHAR(255),URL NVARCHAR(255),DomainType TINYINT) AS A WHERE A.Domain IS NOT NULL OR LEN(A.Domain)>0
		SET @errorcount=@errorcount+@@ERROR
		EXECUTE sp_xml_removedocument @hdoc
	END

	IF(@errorcount=0)--д�빫˾��Ϣ
	BEGIN
		INSERT INTO [tbl_CompanyInfo]([CompanyName],[CompanyType],[CompanyEnglishName],[License],[ContactName]
			,[ContactTel],[ContactMobile],[ContactFax],[CompanyAddress],[CompanyZip]
			,[CompanySiteUrl],[SystemId],[IssueTime])
		VALUES(@CompanyName,'','','',@Realname
			,@Telephone,@Mobile,@Fax,'',''
			,'',@SysId,GETDATE())
		SET @errorcount=@errorcount+@@ERROR
		SET @CompanyId=@@IDENTITY
	END

	IF(@errorcount=0)--д�����Ա��ɫ
	BEGIN
		INSERT INTO [tbl_SysRoleManage]([RoleName],[RoleChilds],[CompanyId],[IsDelete])
		VALUES('����Ա',@ThirdPermission,@CompanyId,'0')
		SET @errorcount=@errorcount+@@ERROR
		SET @RoleId=@@IDENTITY
	END	

	IF(@errorcount=0)--д�벿����Ϣ
	BEGIN
		INSERT INTO [tbl_CompanyDepartment]([DepartName],[PrevDepartId],[DepartManger],[ContactTel],[ContactFax]
			,[PageHeadFile],[PageFootFile],[TemplateFile],[DepartStamp],[Remark]
			,[CompanyId],[OperatorId],[IssueTime],[IsDelete])
		VALUES(@DepartmentName,0,0,@Telephone,@Fax
			,'','','','',''
			,@CompanyId,0,GETDATE(),'0')
		SET @errorcount=@errorcount+@@ERROR
		SET @DepartmentId=@@IDENTITY
	END

	IF(@errorcount=0)--д�����Ա�˺�
	BEGIN
		INSERT INTO [tbl_CompanyUser]([CompanyId],[TourCompanyId],[UserName],[Password],[MD5Password]
			,[UserType],[ContactName],[ContactSex],[ContactTel],[ContactFax]
			,[ContactMobile],[ContactEmail],[QQ],[MSN],[JobName]
			,[LastLoginIP],[LastLoginTime],[RoleID],[PermissionList],[PeopProfile]
			,[Remark],[IsDelete],[IsEnable],[IsAdmin],[IssueTime]
			,[DepartName],[DepartId],[SuperviseDepartId],[SuperviseDepartName])
		VALUES(@CompanyId,0,@Username,@Password,@MD5Password
			,2,@Realname,1,@Telephone,@Fax
			,@Mobile,'','','',''
			,'','',@RoleId,@ThirdPermission,''
			,'','0','1','1',GETDATE()
			,@DepartmentName,@DepartmentId,0,'')
		SET @errorcount=@errorcount+@@ERROR
		SET @UserId=@@IDENTITY
	END

	IF(@errorcount=0)--������ԱĬ���ܲ����Ÿ�����
	BEGIN
		UPDATE [tbl_CompanyDepartment] SET [DepartManger]=@UserId WHERE Id=@DepartmentId
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount=0)--Ĭ��������пͻ��ȼ�
	BEGIN
		INSERT INTO [tbl_CompanyCustomStand]([CustomStandName],[CompanyId],[OperatorId],[IssueTime],[IsSystem],[IsDelete],[LevType])
		VALUES('����',@CompanyId,0,GETDATE(),'1','0',1)
		SET @errorcount=@errorcount+@@ERROR
	END
	IF(@errorcount=0)--Ĭ�����ͬ�пͻ��ȼ�
	BEGIN
		INSERT INTO [tbl_CompanyCustomStand]([CustomStandName],[CompanyId],[OperatorId],[IssueTime],[IsSystem],[IsDelete],[LevType])
		VALUES('ͬ��',@CompanyId,0,GETDATE(),'1','0',2)
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount=0)--Ĭ����ӳ��汨�۱�׼
	BEGIN
		INSERT INTO [tbl_CompanyPriceStand]([PriceStandName],[CompanyId],[OperatorId],[IssueTime],[IsDelete])
		VALUES('����',@CompanyId,0,GETDATE(),'0')
	END

	IF(@errorcount=0)--����ϵͳĬ�ϵ�ʡ�ݳ�����Ϣ
	BEGIN
		DECLARE @i INT--������
		DECLARE @ProvinceId INT--��˾ʡ�ݱ��
		DECLARE @ProvinceCount INT--ʡ������
		
		SELECT @ProvinceCount=COUNT(*) FROM [tbl_SysProvince]
		SET @i=1

		WHILE(@i<=@ProvinceCount AND @errorcount=0)
		BEGIN
			INSERT INTO [tbl_CompanyProvince]([ProvinceName],[CompanyId],[OperatorId])
			SELECT [ProvinceName],@CompanyId,0 FROM [tbl_SysProvince] WHERE [Id]=@i
			SET @errorcount=@errorcount+@@ERROR
			SET @ProvinceId=@@IDENTITY			

			INSERT INTO [tbl_CompanyCity] ([ProvinceId],[CityName],[CompanyId],[IsFav],[OperatorId])
			SELECT @ProvinceId,[CityName],@CompanyId,'0',0 FROM [tbl_SysCity] WHERE [ProvinceId]=@i
			SET @errorcount=@errorcount+@@ERROR
			SET @i=@i+1
		END
	END

	IF(@errorcount=0)--д�빫˾����
	BEGIN	
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@SettingsXML
		INSERT INTO [tbl_CompanySetting]([Id],[FieldName],[FieldValue])
		SELECT @CompanyId,A.[Key],A.[Value] FROM OPENXML(@hdoc,'/ROOT/Info') WITH([Key] NVARCHAR(255),[Value] NVARCHAR(255)) AS A
		SET @errorcount=@errorcount+@@ERROR
		EXECUTE sp_xml_removedocument @hdoc
	END

	IF(@errorcount>0)
	BEGIN
		ROLLBACK TRAN
		SET @Result=0
		RETURN -1
	END

	COMMIT TRAN
	SET @Result=1
	RETURN 1
END
GO

-- =============================================
-- Author:		����־
-- Create date: 2011-04-25
-- Description:	������ϵͳ
-- =============================================
ALTER PROCEDURE [dbo].[proc_Sys_UpdateSys]
	@SysId INT,--ϵͳ���
	@SysDomainsXML NVARCHAR(MAX),--ϵͳ����XML:<ROOT><Info Domain="����" URL="Ŀ��λ��url" DomainType="��������" /></ROOT>
	@FirstPermission NVARCHAR(MAX),--ϵͳһ����Ŀ
	@SecondPermission NVARCHAR(MAX),--ϵͳ������Ŀ
	@ThirdPermission NVARCHAR(MAX),--ϵͳȨ��	
	@Username NVARCHAR(255),--����Ա�˺�
	@Password NVARCHAR(255),--����Ա����(����)
	@MD5Password NVARCHAR(255),--����Ա����(MD5)
	@SettingsXML NVARCHAR(MAX),--��˾����XML:<ROOT><Info Key="����KEY" Value="����VALUE" /></ROOT>
	@Result INT OUTPUT--��� 1:�ɹ� ����ʧ��
AS
BEGIN
	DECLARE @errorcount INT
	DECLARE @CompanyId INT--��˾���
	DECLARE @UserId INT--����Ա�˻����
	DECLARE @hdoc INT

	SET @errorcount=0

	IF(@SysDomainsXML IS NOT NULL AND LEN(@SysDomainsXML)>0)
	BEGIN
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@SysDomainsXML
		IF EXISTS(SELECT 1 FROM [tbl_SysDomain] WHERE [SysId]<>@SysId AND [Domain] IN(SELECT A.Domain FROM OPENXML(@hdoc,'/ROOT/Info') WITH(Domain NVARCHAR(255)) AS A WHERE A.Domain IS NOT NULL OR LEN(A.Domain)>0))--��֤�����ظ�
		BEGIN
			SET @Result=-1
			RETURN -1
		END
		EXECUTE sp_xml_removedocument @hdoc
	END	

	SELECT @CompanyId=[Id] FROM [tbl_CompanyInfo] WHERE [SystemId]=@SysId
	SELECT TOP(1) @UserId=[Id] FROM [tbl_CompanyUser] WHERE [CompanyId]=@CompanyId AND [IsAdmin]='1'

	BEGIN TRAN
	--����ϵͳ��Ϣ
	UPDATE [tbl_Sys] SET [Module]=@FirstPermission,[Part]=@SecondPermission,[Permission]=@ThirdPermission WHERE [SysId]=@SysId

	IF(@Password IS NOT NULL AND LEN(@Password)>0)--���¹���Ա�˺���Ϣ
	BEGIN
		UPDATE [tbl_CompanyUser] SET [UserName]=@Username,[Password]=@Password,[MD5Password]=@MD5Password WHERE [Id]=@UserId
	END

	IF(@errorcount=0 AND @SysDomainsXML IS NOT NULL AND LEN(@SysDomainsXML)>0)--ϵͳ����
	BEGIN
		DELETE FROM [tbl_SysDomain] WHERE [SysId]=@SysId
		SET @errorcount=@errorcount+@@ERROR
		IF(@errorcount=0)
		BEGIN
			EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@SysDomainsXML
			INSERT INTO [tbl_SysDomain]([SysId],[Domain],[Url],[Type])
			SELECT @SysId,A.Domain,A.Url,A.DomainType FROM OPENXML(@hdoc,'/ROOT/Info') WITH(Domain NVARCHAR(255),URL NVARCHAR(255),DomainType TINYINT) AS A WHERE A.Domain IS NOT NULL OR LEN(A.Domain)>0
			SET @errorcount=@errorcount+@@ERROR
			EXECUTE sp_xml_removedocument @hdoc
		END
	END

	IF(@errorcount=0)--��˾����
	BEGIN	
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@SettingsXML
		DELETE FROM [tbl_CompanySetting] WHERE [Id]=@SysId AND [FieldName] IN(SELECT A.[Key] FROM OPENXML(@hdoc,'/ROOT/Info') WITH([Key] NVARCHAR(255),[Value] NVARCHAR(255)) AS A)
		SET @errorcount=@errorcount+@@ERROR
		IF(@errorcount=0)
		BEGIN			
			INSERT INTO [tbl_CompanySetting]([Id],[FieldName],[FieldValue])
			SELECT @CompanyId,A.[Key],A.[Value] FROM OPENXML(@hdoc,'/ROOT/Info') WITH([Key] NVARCHAR(255),[Value] NVARCHAR(255)) AS A
			SET @errorcount=@errorcount+@@ERROR			
		END
		EXECUTE sp_xml_removedocument @hdoc
	END

	
	/*IF(@errorcount=0)--��ɫȨ�޴���ȥ����ϵͳÿ����ɫȨ���ڸ�ϵͳ�в����ڵ�Ȩ��
	BEGIN
		
	END

	IF(@errorcount=0)--�û�Ȩ�޴���ȥ����ϵͳÿ���û�Ȩ���ڸ�ϵͳ�в����ڵ�Ȩ��
	BEGIN
		
	END*/
	

	IF(@errorcount>0)
	BEGIN
		ROLLBACK TRAN
		SET @Result=0
		RETURN -1
	END

	COMMIT TRAN
	SET @Result=1
	RETURN 1
END
GO

-- =============================================
-- Author:		����־
-- Create date: 2012-04-09
-- Description:	����������
-- =============================================
CREATE PROCEDURE proc_Tour_IsRecently
	@TourId CHAR(36)--�ƻ����
	,@IsT CHAR(1)--�Ƿ�ģ����
AS
BEGIN
	DECLARE @TTourId CHAR(36)--ģ���ű��
	DECLARE @RTourId CHAR(36)--������ŵ��Ŷӱ��
	IF(@TourId IS NULL OR LEN(@TourId)=0) RETURN 0
	IF(@IsT='0') SELECT @TourId=TemplateId FROM tbl_Tour WHERE TourId=@TourId

	SET @TTourId=@TourId

	SELECT TOP(1) @RTourId=TourId FROM tbl_Tour WHERE TemplateId=@TTourId AND IsDelete='0' AND LeaveDate>=GETDATE() ORDER BY LeaveDate ASC
	
	UPDATE tbl_Tour SET IsRecently='0' WHERE TemplateId=@TTourId
	UPDATE tbl_Tour SET IsRecently='1' WHERE TourId=@RTourId

	RETURN 1
END
GO
-- =============================================
-- Author:		����־
-- Create date: 2011-01-28
-- Description:	�ƻ���Ϣά��SQL�ƻ�
-- History:
-- 1.����־ 2012-04-09 ����������Ŵ���
-- =============================================
ALTER PROCEDURE [dbo].[SQLPlan_Tour]

AS
BEGIN
	--����Ŷ��ѳ���״̬
	UPDATE tbl_Tour SET IsLeave='1' WHERE TourType IN(0,1) AND DATEDIFF(DAY,GETDATE(),[LeaveDate])<=0 

	--����Ŷ�״̬Ϊ�г�;��
	UPDATE tbl_Tour SET Status=2 WHERE TourType IN(0,1) AND [Status] IN(0,1,3) AND DATEDIFF(DAY,GETDATE(),[LeaveDate])<=0 AND DATEDIFF(DAY,GETDATE(),[RDate])>0

	--����Ŷ�״̬Ϊ���ű���
	UPDATE tbl_Tour SET Status=3 WHERE TourType IN(0,1) AND [Status] IN(0,1,2) AND DATEDIFF(DAY,GETDATE(),[RDate])<0	


	--������Ŵ���
	--��������������Ҫ���������š��������������ģ����
	DECLARE @tmptbl TABLE(TourId CHAR(36))
	INSERT INTO @tmptbl(TourId) SELECT TourId FROM tbl_Tour WHERE TemplateId='' AND TourType=0
	--ģ���ű��
	DECLARE @TemplateTourId CHAR(36)

	DECLARE tmpcursor CURSOR
	FOR SELECT TourId FROM @tmptbl
	OPEN tmpcursor
	FETCH NEXT FROM tmpcursor INTO @TemplateTourId
	
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		--����������
		EXECUTE proc_Tour_IsRecently @TourId=@TemplateTourId,@IsT=N'1'
		FETCH NEXT FROM tmpcursor INTO @TemplateTourId
	END

	CLOSE tmpcursor
	DEALLOCATE tmpcursor
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
-- 5.2012-04-09 ����־ ����������Ŵ���
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
-- Author:		����־
-- Create date: 2011-01-23
-- Description:	����ɢƴ�ƻ����ŶӼƻ������������Ϣ
-- History:
-- 1.2011-07-11 �ŶӼƻ����������ͼ۸���Ϣ��ɾ��SelfUnitPriceAmount
-- 2.2012-04-09 ����־ ����������Ŵ���
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
-- =============================================
-- Author:		���ĳ�
-- Create date: 2011-04-25
-- Description:	ɾ���Ŷ���Ϣ������ɾ����
-- Error��0��������
-- Success��1ɾ���ɹ�
-- History:
-- 1.2011-12-16 ����־ �����ſ�֧���Ǽǵ�ɾ��
-- 2.2012-03-03 ����־ ���ӻ�Ʊ������Ʊ�Ƶ�������Ϣ�Ĺ�����ɾ��
-- 3.2012-04-09 ����־ ����������Ŵ���
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
	DELETE FROM tbl_PlanTicketPlanId WHERE JiPiaoGuanLiId IN(SELECT Id FROM tbl_PlanTicket WHERE TourId=@TourId)
	SET @errorcount = @errorcount + @@ERROR
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

	IF(@errorcount=0)--����������
	BEGIN
		EXECUTE proc_Tour_IsRecently @TourId=@TourId,@IsT=N'0'
	END

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

--���ݴ����֣�
--������Ŵ���
EXEC SQLPlan_Tour
GO
--�������� ��������������
UPDATE [tbl_SysDomain] SET [Type]=1 WHERE LEN([Url])>0
GO

--ͬ��ƽ̨���� 

--������־���Ѹ���



-- =============================================
-- Author:		<Author,,�����>
-- Create date: <Create Date,2011-02-14,>
-- Description:	<Description,��Ʊ,>
-- History:
-- 1.2011-04-21 ����־ ���̵��� 
-- 2.2011-08-09 ���ĳ� ��Ʊ����ӷ�����Ĭ������
-- 3.2012-05-28 ����־ �����������Ʊ���޸Ĳ��������@TourId��BUG
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

		IF NOT EXISTS(SELECT 1 FROM [tbl_TourOtherCost] WHERE [ItemId] =@travellerRefundId AND [ItemType]=1 AND [CostType]=0)--д�ӷ�����
		BEGIN			
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

/*

declare @error int
declare  @TicketOutId char(36)
--     declare  @TicketId int
--     declare  @ReturnMoney money
--     declare  @State tinyint
--     declare  @Remark varchar(250)
--     declare @Result int 
--    set @State=4
--    set @Remark=N''
--    set @ReturnMoney=110.0000
--    set @ticketId=25
declare @companyId int
declare @tourId char(36)
declare @OperateID int
declare @UserId char(36)
set @companyId =0
set @tourId =''
set @OperateID =0
set @UserId=''
begin transaction UpdateTicket
  begin
    select @TicketOutId=d.id,@companyId=a.companyId,@tourId=a.tourid,@OperateID=a.OperatorId,@UserId=c.userid  from tbl_PlanTicket a join tbl_CustomerRefund b on a.RefundId=b.id join tbl_PlanTicketOutCustomer c on b.CustormerId=c.UserId join tbl_PlanTicketOut d on c.TicketOutId=d.id  where a.id=@TicketId
   --- print @TicketOutId
   if(len(@TicketOutId)> 1) 
      begin 
--        print @TicketOutId
           --============================�����˿���========================
        update tbl_CustomerRefund set RefundAmount=@ReturnMoney,IsRefund=case @State when 4 then 1 else 0 end,RefundNote=@Remark where CustormerId in(select b.UserId from tbl_PlanTicketOut a join tbl_PlanTicketOutCustomer b on a.id=b.TicketOutId where a.id=@TicketOutId)
         set  @error =@error +@@error
        update tbl_TourOrderCustomer set TicketStatus=0 where ID=@UserId
         set  @error =@error +@@error
        update tbl_Planticket set tbl_Planticket.state=@State where id=@TicketId
         set  @error =@error +@@error
--        update tbl_PlanTicketOutCustomer set IsApplyRefund=1 where TicketOutId=@TicketOutId
--          set  @error =@error +@@error
           --============================�����˿���========================

           --============================ͬ���ӷ�����========================
       declare @isExists int 
       select @isExists=count(ItemId) from  tbl_TourOtherCost where ItemId =@TicketOutId and ItemType=1 and CostType=0
       if @isExists>0 ---���ڣ�ִ���޸�
        begin
          declare @difference decimal --���
          declare @oldReturnAmont decimal --ԭ�����˻����

          select @oldReturnAmont=Proceed from tbl_TourOtherCost where ItemId=@TicketOutId and ItemType=1 and CostType =0
          set @difference =@ReturnMoney-@oldReturnAmont
         update tbl_TourOtherCost set Proceed =Proceed+@difference,SumCost=Proceed+IncreaseCost-ReduceCost+@difference where ItemId=@TicketOutId and ItemType=1 and CostType =0
         set  @error =@error +@@error
       end
       else ----�����ڣ�ִ�����
        begin
          INSERT INTO [tbl_TourOtherCost]
           (id,[CompanyId],[TourId],[CostType],[Proceed]
           ,[SumCost],[Remark],OperatorId,PayTime,CreateTime,[ItemType],[ItemId],ProceedItem)
          VALUES
           (newid(),@companyId,@tourId,0,@ReturnMoney
           ,@ReturnMoney,@Remark,@OperateID ,getdate(),getdate(),1,@TicketOutId,'��Ʊ��Ʊ��'
            )
        set  @error =@error +@@error
        end
           --============================ͬ���ӷ�����========================

--          --============================ͬ���ο���Ϣ========================
--	  if len(@TourOrderCustomer)>0
--		  begin
--			  declare @hdoc int 
--			  declare @VisitorName varchar(250)
--			  declare @CradNumber varchar(250)
--			  declare @CradType int
--			  declare @id char(36)
--			  set @id=''
--			  EXECUTE sp_xml_preparedocument @hdoc output,@TourOrderCustomer
--			   SELECT top 1 @VisitorName=VisitorName,@CradNumber=CradNumber,@CradType=CradType,@id=id  FROM OPENXML(@hdoc,'/ROOT/TourOrderCustomer') 
--			   with(VisitorName varchar(250),CradNumber varchar(250),CradType int,id char(36))
--			   EXECUTE sp_xml_removedocument @hdoc
--			 if len(@id)>1
--			  begin
--				update TourOrderCustomer set VisitorName=@VisitorName,CradNumber=@CradNumber,CradType=@CradType where id=@id
--			  end  
--		 end
--       --============================ͬ���ο���Ϣ����========================
    end
    else
    begin
      commit transaction UpdateTicket
      set @Result =0
      return @Result
    end
end
if @error >0
begin
 rollback transaction UpdateTicket
 set @Result =0
 return @Result
end

 commit transaction UpdateTicket
 set @Result =1
 return @Result
END
*/
GO

--������־�Ѹ��� 2012-05-28
GO


--2012-05-29 ����־ �ؽӰ��ż۸���ɾ���Ҫ���ֶθ��ĳ�NVARCHAR(MAX)
ALTER TABLE [tbl_PlanLocalAgencyPrice] 
ALTER COLUMN [Remark] NVARCHAR(MAX)
GO

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
         with(PriceTpyeId tinyint,Title nvarchar(50), Price money, Remark nvarchar(MAX), PeopleCount int, ReferenceID char(36), PriceType tinyint)
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
         with(PriceType tinyint,Title nvarchar(50), Price money, Remark nvarchar(MAX), PeopleCount int, PriceTpyeId tinyint)
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
--������־�Ѹ��� 2012-05-29 ����־
GO
-- =============================================
-- Author:		���ĳ�
-- Create date: 2011-01-26
-- Description:	���¼����Ŷ�֧����������֧������
-- Error��0��������
-- Error��-1��������з�������
-- Success��1���¼���ɹ�
-- History:
-- 1.2012-06-13 ����־ �޸�֧��ͬ��ʱδ����tbl_StatAllOut��Ӧ�̱�ŵ�BUG
-- =============================================
ALTER PROCEDURE [dbo].[proc_Utility_CalculationTourOut]
	@TourId char(36) = null,         --�Ŷ�Id
	@ItemIdAndTypeXML nvarchar(max),    --֧����Ŀ��š����ͼ���
	@ErrorValue  int = 0 output    --�������
AS
BEGIN

	declare @ErrorCount int    --���������
	declare @TourType tinyint  --�Ŷ�����
	declare @TotalExpenses money  --�ŶӼƵ���֧��
	declare @TotalOtherExpenses money   --�Ŷ��ӷ���֧��
	declare @ItemCount int    --����ڵ�����
	declare @Amount money     --���
	declare @AddAmount money  --���ӷ���
	declare @ReduceAmount money   --���ٷ���
	declare @TotalAmount money  --�ϼƽ��
	declare @SupplierId  int   --��Ӧ�̱��
	declare @SupplierName  nvarchar(255)    --��Ӧ������
	set @ErrorCount = 0
	set @TotalExpenses = 0
	set @TotalOtherExpenses = 0
	set @ItemCount = 0
	set @AddAmount = 0
	set @ReduceAmount = 0
	set @TotalAmount = 0
	set @Amount = 0

	create table #tbl_ItemIdAndType(ItemId char(36),ItemType tinyint)  --��Ŀ��ʱ��

	begin tran 	Calculation

	if @ItemIdAndTypeXML is not null and len(@ItemIdAndTypeXML) > 0
	begin
		set @ItemIdAndTypeXML = replace(replace(@ItemIdAndTypeXML,char(13),' '),char(10),' ')
		DECLARE @hdoc int
		EXEC sp_xml_preparedocument @hdoc OUTPUT, @ItemIdAndTypeXML
		insert into #tbl_ItemIdAndType(ItemId,ItemType) SELECT ItemId,ItemType FROM OPENXML(@hdoc,N'/ROOT/ItemIdAndType')
		WITH (ItemId char(36),ItemType tinyint)
		--��֤����
		SET @ErrorCount = @ErrorCount + @@ERROR
		EXEC sp_xml_removedocument @hdoc 
	end

	--�Ŷ�Id��Ϊ�յ�ʱ�����¼����ŶӼƵ���֧�����ӷ���֧��
	if @TourId is not null and len(@TourId) = 36
	begin
		select @TourType = TourType from tbl_Tour where TourId = @TourId
		if @TourType = 2  --���������
		begin
			select @TotalExpenses = @TotalExpenses + isnull(sum(TotalAmount),0) from tbl_PlanSingle where TourId = @TourId
			--��ѯ�Ƶ���
			insert into #tbl_ItemIdAndType(ItemId,ItemType) select PlanId,4 from tbl_PlanSingle where  TourId = @TourId
		end
		else     --ɢƴ�ƻ����ŶӼƻ���
		begin
			--��ѯ�Ŷӵؽ���֧��
			select @TotalExpenses = @TotalExpenses + isnull(sum(TotalAmount),0) from tbl_PlanLocalAgency where TourId = @TourId
			--��ѯ�Ƶ���
			insert into #tbl_ItemIdAndType(ItemId,ItemType) select Id,1 from tbl_PlanLocalAgency where TourId = @TourId
			
			--��ѯ�Ŷӻ�Ʊ��֧��
			select @TotalExpenses = @TotalExpenses + isnull(sum(TotalAmount),0) from tbl_PlanTicketOut where TourId = @TourId and [State] = 3
			--��ѯ�Ƶ���
			insert into #tbl_ItemIdAndType(ItemId,ItemType) select Id,2 from tbl_PlanTicketOut where TourId = @TourId
		end
		--��ѯ�Ŷӵ��ӷ���֧��
		select @TotalOtherExpenses = @TotalOtherExpenses + isnull(sum(SumCost),0) from tbl_TourOtherCost where TourId = @TourId and CostType = 1
		--��ѯ�Ŷ��ӷ�֧����
		insert into #tbl_ItemIdAndType(ItemId,ItemType) select Id,3 from tbl_TourOtherCost where TourId = @TourId and CostType = 1

		--�����ŶӼƵ���֧�����Ŷ��ӷ���֧��
		update tbl_Tour set TotalExpenses = @TotalExpenses,TotalOtherExpenses = @TotalOtherExpenses where TourId = @TourId 
		--��֤����
		SET @ErrorCount = @ErrorCount + @@ERROR
	end

	declare @ItemId char(36)   --��Id
	declare @ItemType  tinyint  --������
	DECLARE cur_Calculation CURSOR FOR
	SELECT distinct ItemId,ItemType FROM #tbl_ItemIdAndType
	open cur_Calculation
	while 1 = 1
	begin
		fetch next from cur_Calculation into @ItemId,@ItemType
		if @@fetch_status<>0
			break

		if @ItemId is null or len(@ItemId) <> 36 or @ItemType is null
			continue

		if @ItemType = 1  --�ؽ�֧��
		begin
			select @Amount = isnull(Settlement,0),@AddAmount = isnull(AddAmount,0),@ReduceAmount = isnull(ReduceAmount,0),@TotalAmount = isnull(TotalAmount,0),@SupplierId = isnull(TravelAgencyID,0),@SupplierName = isnull(LocalTravelAgency,'') from tbl_PlanLocalAgency where ID = @ItemId
		end
		else if @ItemType = 2  --��Ʊ֧��
		begin
			select @Amount = isnull(Total,0),@AddAmount = isnull(AddAmount,0),@ReduceAmount = isnull(ReduceAmount,0),@TotalAmount = isnull(TotalAmount,0),@SupplierId = isnull(TicketOfficeId,0),@SupplierName = isnull(TicketOffice,'') from tbl_PlanTicketOut where ID = @ItemId
		end
		else if @ItemType = 3  --�ӷ�֧��
		begin
			select @Amount = isnull(Proceed,0),@AddAmount = isnull(IncreaseCost,0),@ReduceAmount = isnull(ReduceCost,0),@TotalAmount = isnull(SumCost,0),@SupplierId = isnull(CustromCId,0),@SupplierName = isnull(CustromCName,'') from tbl_TourOtherCost where ID = @ItemId
		end
		else if @ItemType = 4  --�������Ӧ�̰���
		begin
			select @Amount = isnull(Amount,0),@AddAmount = isnull(AddAmount,0),@ReduceAmount = isnull(ReduceAmount,0),@TotalAmount = isnull(TotalAmount,0),@SupplierId = isnull(SupplierId,0),@SupplierName = isnull(SupplierName,'') from tbl_PlanSingle where PlanId = @ItemId
		end

		select @ItemCount = count(Id) from tbl_StatAllOut where ItemId = @ItemId and ItemType = @ItemType
		if @ItemCount > 0
		begin
			--����ͳ�Ʒ�������֧����
			update tbl_StatAllOut set Amount = @Amount,AddAmount = @AddAmount,ReduceAmount = @ReduceAmount,TotalAmount = @TotalAmount,SupplierId = @SupplierId,SupplierName = @SupplierName where ItemId = @ItemId and ItemType = @ItemType
			--��֤����
			SET @ErrorCount = @ErrorCount + @@ERROR
		end
	end
	close cur_Calculation
	deallocate cur_Calculation

	IF @ErrorCount > 0 
	BEGIN
		SET @ErrorValue = -1;
		ROLLBACK TRAN Calculation
	end	
	ELSE
	BEGIN
		SET @ErrorValue = 1;
		COMMIT TRAN Calculation
	end
	
	drop table #tbl_ItemIdAndType  --ɾ����ʱ��
	
END
GO
--������־�Ѹ��� 2012-06-13 ����־
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
--������־�Ѹ��� 2012-07-09 ����־
GO


--�ƻ������׼��Ϣ���������� ����־ 2012-07-11
ALTER TABLE dbo.tbl_TourService
	DROP CONSTRAINT PK_TBL_TOURSERVICE
GO
CREATE CLUSTERED INDEX IX_tbl_TourService_01 ON dbo.tbl_TourService
	(
	TourId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
DECLARE @v sql_variant 
SET @v = N'�ź�����'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourService', N'INDEX', N'IX_tbl_TourService_01'
GO

--�ƻ��г���Ϣ����������
ALTER TABLE dbo.tbl_TourPlan
	DROP CONSTRAINT PK_TBL_TOURPLAN
GO
CREATE CLUSTERED INDEX IX_tbl_TourPlan_01 ON dbo.tbl_TourPlan
	(
	TourId,
	PlanId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
DECLARE @v sql_variant 
SET @v = N'�ź�����'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourPlan', N'INDEX', N'IX_tbl_TourPlan_01'
GO
--������־�Ѹ��� 2012-07-11 094700 ����־
GO

