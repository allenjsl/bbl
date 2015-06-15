-- =============================================
-- Author:		<Author,,Name>
-- Create date: 2012-02-24
-- Description:	查询结果集分页存储过程
-- =============================================
CREATE PROCEDURE [dbo].[proc_Pading_BySqlTable]
(    
    @PageSize     int = 10,           -- 页尺寸
    @PageIndex    int = 1,            -- 页码
    @TableName      nvarchar(max),       -- 派生表
    @FieldsList	  nvarchar(2000),	      --列名
    @FieldSearchKey     nvarchar(2000) = '', -- 查询条件 (注意: 不要加 where)
    @OrderString      nvarchar(1000),       -- 排序  
    @IsGroupBy   char(1) = '0',    --条件是否分组
    @GroupString      nvarchar(1000)=''       -- 分组条件  
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
	SET @OrderString = ' ORDER BY ' + @OrderString
ELSE
	SET @OrderString = ''
------------------------分组条件--------------------------
IF @GroupString IS NOT NULL AND @GroupString != '' AND @IsGroupBy IS NOT NULL AND @IsGroupBy = '1'
	SET @GroupString = ' GROUP BY ' + @GroupString
ELSE
	SET @GroupString = ''
------------------------查询条件--------------------------
if @FieldSearchKey IS NOT NULL AND @FieldSearchKey != ''--是否存在查询条件
	SET @strTmp = 'SELECT @RecordCount=COUNT(*) FROM (' + @TableName + ') AS A WHERE ' + @FieldSearchKey + @GroupString
ELSE
	SET @strTmp = 'SELECT @RecordCount=COUNT(*) FROM (' + @TableName + ') AS A ' + @GroupString

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
SET @strSQL = 'SELECT t120.* FROM ( SELECT  ' + @FieldsList + ',ROW_NUMBER() OVER(' + @OrderString + ') AS RowNumber FROM (' + @TableName + ') AS A ' + @strTmp + @GroupString + ' ) AS t120 WHERE RowNumber BETWEEN ' + Cast(@PageLowerBound as nvarchar) + ' AND ' + Cast(@PageUpperBound as nvarchar)
select @RecordCount as RecordCount
--PRINT @strSQL

EXECUTE(@strSQL)
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2012-02-24
-- Description:	统计分析-账龄分析-按客户单位统计
-- =============================================
CREATE VIEW view_ZhangLingAnKeHuDanWei
AS
SELECT A.Id--客户单位编号
	,A.Name--客户单位名称
	,A.CompanyId--系统公司编号
	,(SELECT ISNULL(SUM(FinanceSum-HasCheckMoney),0) FROM tbl_TourOrder AS B WHERE B.IsDelete='0' AND B.OrderState NOT IN(3,4) AND B.BuyCompanyId=A.Id) AS WeiShouKuan--未收款
	,(SELECT MIN(IssueTime) FROM tbl_TourOrder AS C WHERE C.IsDelete='0' AND C.OrderState NOT IN(3,4) AND C.BuyCompanyId=A.Id) AS EarlyTime--最早未收款下单时间
	FROM tbl_Customer AS A
WHERE A.IsDelete='0'
GO

--订单表增加组团社团号 汪奇志 2012-03-01
ALTER TABLE dbo.tbl_TourOrder ADD
	BuyerTourCode nvarchar(255) NULL
GO
DECLARE @v sql_variant 
SET @v = N'组团社团号'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOrder', N'COLUMN', N'BuyerTourCode'
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
-- Author:		luofx
-- Create date: 2011-01-24
-- Description:	修改订单
-- History:
-- 1.增加组团联系人信息、返佣信息 汪奇志 2011-06-08
-- 2.增加修改团队计划订单同步修改团队计划信息，修正人数验证（减去已退团人数）
-- 3.修改客户单位(BuyCompanyName,BuyCompanyID,SalerId,SalerName,CommissionType)  曹胡生 2012-02-02
-- 4.2012-03-01 汪奇志 增加组团社团号
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

-- =============================================
-- Author:		周文超
-- Create date: 2011-04-19
-- Description:	客户关系管理-销售统计视图
-- History:
-- 1.2012-01-13 汪奇志 订单人数减去退团人数
-- 2.2012-01-30 汪奇志 增加人次统计销售员编号及姓名列、对方操作员编号及姓名列
-- 3.2012-03-01 汪奇志 增加下单时间列
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
,IssueTime--下单时间
from tbl_TourOrder
where tbl_TourOrder.IsDelete = '0' and tbl_TourOrder.OrderState not in (3,4)
GO

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
-- Description:	修改收退款信息
-- History:
-- 1.2012-03-02 汪奇志 将收退款金额的数据类型由DECIMAL改成MONEY，以修复修改审核时四舍五入的BUG 
-- =============================================
ALTER PROCEDURE [dbo].[proc_ReceiveRefund_Update]
	@ReceiveRefundXML NVARCHAR(MAX),  --收退款XML详细信息:<ROOT><ReceiveRefund id="" CompanyId="" PayCompanyId="" PayCompanyName="" ItemId="" ItemType="" RefundDate="" StaffNo="" StaffName="" RefundMoney="" RefundType="" IsBill="" BillAmount="" IsReceive="" IsCheck="" Remark="" OperatorID="" CheckerId=""/> </ROOT>
	@IsRecive INT,                    --1：销售收款；0：销售退款 
	@OrderId CHAR(36),                --订单编号
	@Result INT output                --回传参数
AS
DECLARE @ErrorCount INT   --错误累计参数
DECLARE @hdoc INT         --XML使用参数
DECLARE @i INT            --计数器
DECLARE @MAXID INT        --暂存表最大vid
DECLARE @CompanyID INT                  --所属公司ID
DECLARE @RefundId CHAR(36)              --收退款编号
DECLARE @PayCompanyId INT               --付款公司编号
DECLARE @PayCompanyName NVARCHAR(250)   --付款公司名称
DECLARE @ItemId CHAR(36)                --关联编号
DECLARE @ItemType TINYINT               --关联编号（订单编号，团款其他收入支出编号）
DECLARE @RefundDate DATETIME            --收退款日期
DECLARE @StaffNo INT                    --员工编号
DECLARE @StaffName NVARCHAR(250)        --员工姓名
DECLARE @RefundMoney MONEY              --金额
DECLARE @RefundType TINYINT             --收退款方式
DECLARE @IsBill CHAR(1)                 --是否开票
DECLARE @BillAmount MONEY               --开票金额
DECLARE @IsReceive TINYINT              --是否收款
DECLARE @IsCheck TINYINT                --是否审核
DECLARE @Remark NVARCHAR(1000)          --备注
DECLARE @OperatorID INT                 --操作人编号
DECLARE @CheckerId INT                  --审核人编号
SET @ErrorCount=0
SET @Result=1
SET @i=1
BEGIN
CREATE TABLE #TmpReceiveRefund_Update(
	   [Vid] INT,
       [Id] CHAR(36),                 --收退款编号
       [CompanyID] INT,               --所属公司ID
       [PayCompanyId] INT,            --付款公司编号
       [PayCompanyName] NVARCHAR(250),--付款公司名称
       [ItemId] CHAR(36),             --关联编号
       [ItemType] TINYINT,            --关联编号（订单编号，团款其他收入支出编号）
       [RefundDate] DATETIME,         --收退款日期
       [StaffNo] INT,                 --员工编号
       [StaffName] NVARCHAR(250),     --员工姓名
       [RefundMoney] MONEY,           --金额
       [RefundType] TINYINT,          --收退款方式
       [IsBill] CHAR(1),              --是否开票
       [BillAmount] MONEY,            --开票金额
       [IsReceive] TINYINT,           --是否收款
       [IsCheck] TINYINT,             --是否审核
       [Remark] NVARCHAR(1000),       --备注
       [OperatorID] INT,              --操作人编号
       [CheckerId] INT                --审核人编号
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
		--插入收退款信息	
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
		--取得最大row_number()之id
		SELECT @MAXID=MAX(Vid) FROM #TmpReceiveRefund_Update		
		WHILE(@i<=@MAXID AND @ErrorCount=0)
			BEGIN
				SELECT @RefundId=[ID],@RefundDate=RefundDate,@PayCompanyId=PayCompanyId,@PayCompanyName=PayCompanyName,
					@RefundType=RefundType,@IsBill=IsBill,@RefundMoney=RefundMoney,@Remark=Remark,@BillAmount=BillAmount,
					@IsCheck=IsCheck,@StaffName=StaffName,@StaffNo=StaffNo,@OperatorID=OperatorID FROM #TmpReceiveRefund_Update WHERE Vid=@i
				--更新收退款信息
				UPDATE tbl_ReceiveRefund SET [RefundDate]=@RefundDate,[PayCompanyId]=@PayCompanyId,PayCompanyName=@PayCompanyName,
					[RefundType]=@RefundType,[IsBill]=@IsBill,[RefundMoney]=@RefundMoney,[Remark]=@Remark,BillAmount=@BillAmount,
					[IsCheck]=@IsCheck,StaffName=@StaffName,StaffNo=@StaffNo,OperatorID=@OperatorID
					WHERE [Id]=@RefundId
				SET @i = @i + 1
				SET @ErrorCount = @ErrorCount + @@ERROR
			END			
		--是否回滚	
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
-- Author:		汪奇志
-- Create date: 2012-03-02
-- Description:	取消机票退票
-- =============================================
CREATE PROCEDURE proc_PlanTicketQuXiaoTuiPiao
	@CompanyId INT--公司编号
	,@JiPiaoGuanLiId INT--机票管理编号
	,@RetCode INT OUTPUT--处理结果 1:成功 其它失败
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
	--删除退票航段信息
	DELETE FROM tbl_CustomerRefundFlight WHERE RefundId=@TuiPiaoId
	SET @errorcount=@errorcount+@@ERROR

	IF(@errorcount=0)--删除退票信息
	BEGIN
		DELETE FROM tbl_CustomerRefund WHERE Id=@TuiPiaoId
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount=0)--删除关联的其它收入
	BEGIN
		DELETE FROM [tbl_TourOtherCost] WHERE [ItemId] =@TuiPiaoId AND [ItemType]=1 AND [CostType]=	0
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount=0)--删除关联的其它收入
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

--2012-03-02 汪奇志 增加机票管理及机票计调编号关联表 以方便查询
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机票管理编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_PlanTicketPlanId', @level2type=N'COLUMN',@level2name=N'JiPiaoGuanLiId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'机票计调编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_PlanTicketPlanId', @level2type=N'COLUMN',@level2name=N'PlanId'
GO
/****** 对象:  ForeignKey [FK_TBL_PLANTICKETPLANID_REFERENCE_TBL_PLANTICKET]    脚本日期: 03/02/2012 17:24:26 ******/
ALTER TABLE [dbo].[tbl_PlanTicketPlanId]  WITH CHECK ADD  CONSTRAINT [FK_TBL_PLANTICKETPLANID_REFERENCE_TBL_PLANTICKET] FOREIGN KEY([JiPiaoGuanLiId])
REFERENCES [dbo].[tbl_PlanTicket] ([Id])
GO
ALTER TABLE [dbo].[tbl_PlanTicketPlanId] CHECK CONSTRAINT [FK_TBL_PLANTICKETPLANID_REFERENCE_TBL_PLANTICKET]
GO

--更新机票管理表与机票管理及计调关联表信息
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
-- Description:	游客退票:新增退票记录并修改游客表的退票状态
-- Remark：周文超 2011-04-20  删除航班时间、出发地、目的地、人数字段的参数
-- History:
-- 1.2012-03-02 汪奇志 增加机票管理及机票计调安排信息的关联 以方便查询
-- =============================================
ALTER PROCEDURE [dbo].[proc_TourOrderCustomer_AddRefund] 
	@Id Char(36),                 --主键
	@CustormerId CHAR(36),       --游客编号
	@RouteName NVARCHAR(250),  --线路名称	
	@OperatorName NVARCHAR(250),      --操作人姓名
	@OperatorID INT,		--操作人编号	
	@RefundAmount DECIMAL(10,4),   --退款金额
	@RefundNote NVARCHAR(1000), --退票需知
	@OutTourId CHAR(36) OUTPUT,  -- 返回参数	
	@RefundFlightXML nvarchar(max)  --游客已退航段信息XMl  <CustomerRefundFlight_Add RefundId = "0" FlightId = "1" CustomerId = "2" />
AS
DECLARE @MAXID int              --暂存最大id
DECLARE @ErrorCount int         --验证错误
DECLARE @hdoc int               --XML使用参数
DECLARE @TourId CHAR(36)        --团队编号
DECLARE @TourNO NVARCHAR(250)   --团号
DECLARE @OrderId CHAR(36)		--订单编号
DECLARE @Saler NVARCHAR(200)    --销售员名字
DECLARE @CompanyId INT          --公司编号
SET @ErrorCount=0
SET @OutTourId=''
BEGIN
	BEGIN Transaction TourOrderCustomer_AddRefund 
		--查找订单信息销售员等字段信息
		SELECT @TourId=TourId,@OrderId=[id],@Saler=SalerName,@CompanyId=SellCompanyId,@RouteName=RouteName FROM tbl_TourOrder 
			WHERE [id]=(SELECT TOP 1 OrderId FROM tbl_TourOrderCustomer WHERE [Id]=@CustormerId)
		SET @OutTourId=@TourId
		--插入退票记录
		INSERT INTO [tbl_CustomerRefund] ([id],CustormerId,RouteName,
		      OperatorName,OperatorID,RefundAmount,RefundNote)
				VALUES (@Id,@CustormerId,@RouteName,@OperatorName,@OperatorID,@RefundAmount,@RefundNote)
		SET @ErrorCount=@ErrorCount+@@ERROR
		INSERT INTO tbl_PlanTicket(TourId,OrderId,RefundId,TicketType,RouteName,Saler,OperatorId,Operator,RegisterTime,[State],CompanyId)
							VALUES(@TourId,@OrderId,@Id,3,@RouteName,@Saler,@OperatorID,@OperatorName,getdate(),1,@CompanyId)
		SET @ErrorCount=@ErrorCount+@@ERROR		
		
		--写游客已退航段信息
		if @RefundFlightXML is not null or len(@RefundFlightXML) > 0
		begin
			EXEC sp_xml_preparedocument @hdoc OUTPUT, @RefundFlightXML
			insert into tbl_CustomerRefundFlight (RefundId,FlightId,CustomerId) select RefundId,FlightId,CustomerId FROM OPENXML(@hdoc,N'/ROOT/CustomerRefundFlight_Add') WITH (RefundId char(36),FlightId int,CustomerId char(36))
			SET @ErrorCount=@ErrorCount+@@ERROR
			EXEC sp_xml_removedocument @hdoc 
		end

		--写机票管理及机票计调编号关联表
		INSERT INTO tbl_PlanTicketPlanId(JiPiaoGuanLiId,PlanId)
		SELECT DISTINCT A.Id,D.TicketId FROM tbl_PlanTicket AS A INNER JOIN tbl_CustomerRefund AS B ON B.Id=A.RefundId INNER JOIN tbl_CustomerRefundFlight AS C ON C.RefundId=A.RefundId INNER JOIN tbl_PlanTicketFlight AS D ON D.Id=C.FlightId
		WHERE A.RefundId=@Id

		--是否回滚
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
-- Author:		李焕超
-- Create date: 2011-03-01
-- Description:	机票申请修改、出票
-- History:
-- 1.2011-03-30 汪奇志 调整过程书写格式
-- 2.2011-04-21 汪奇志 机票票款增加百分比、其它费用
-- 3.2011-08-15 汪奇志 完成出票机票款自动结清处理(自动结清按系统配置)
-- 4.2012-03-02 汪奇志 增加机票管理及机票计调安排信息的关联 以方便查询
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
GO

-- =============================================
-- Author:		<Author,,李焕超>
-- Create date: <Create Date,2011-02-14,>
-- Description:	<Description,,>
-- History:
-- 1.2012-03-03 汪奇志 增加机票管理及机票计调安排信息的关联的删除
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
     delete from tbl_PlanTicketOutCustomer where TicketOutId= @TicketOutId --删除机票客户
      if @@error>0
        set @error= @error+@@error
     delete from tbl_PlanTicketFlight where TicketId= @TicketOutId --删除航班
      if @@error>0
        set @error= @error+@@error
     delete from tbl_PlanTicketKind where TicketId= @TicketOutId ---删除票款
      if @@error>0
        set @error= @error+@@error
     delete from tbl_planticketout where id =@TicketOutId --删除一次申请
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
---成功
commit transaction TicketDel
set @Result=1
 return @Result
END
GO

-- =============================================
-- Author:		周文超
-- Create date: 2011-04-25
-- Description:	删除团队信息（虚拟删除）
-- Error：0参数错误
-- Success：1删除成功
-- History:
-- 1.2011-12-16 汪奇志 增加团款支出登记的删除
-- 2.2012-03-03 汪奇志 增加机票管理及机票计调安排信息的关联的删除
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
GO

-- =============================================
-- Author:		周文超
-- Create date: 2011-01-26
-- Description:	根据部门Id获取该部门以及该部门的所有子部门（递归）的用户信息
-- History:
-- 1.汪奇志 2012-03-05 去掉是否删除的查询条件
-- =============================================
ALTER PROCEDURE [dbo].[proc_CompanyUser_GetUserByDepart]
	@DepartId  int    --部门Id
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
-- Author:		周文超
-- Create date: 2011-04-18
-- Description:	机票取消审核
-- Error：0参数错误
-- Error：-1未找到对应的机票申请
-- Error：-2团队状态或者机票申请状态不允许取消审核
-- Error：-3取消审核过程中发生错误
-- Success：1取消审核成功
-- History:
-- 1.2012-03-12 汪奇志 增加删除机票管理及机票计调安排信息的关联
-- =============================================
ALTER PROCEDURE  [dbo].[proc_TicketOut_UNChecked]
(
	@TicketId char(36) = '',    --机票申请Id
	@ErrorValue  int = 0 output   --错误代码
)
as
BEGIN
	/*  
	1、删除机票申请列表（tbl_PlanTicket）
	2、删除机票申请航班信息（tbl_PlanTicketFlight）
	3、删除机票申请票款信息（tbl_PlanTicketKind）
	4、删除机票申请游客关系（tbl_PlanTicketOutCustomer）
	5、删除机票申请基本信息（tbl_PlanTicketOut）
	*/
	
	if @TicketId is null or len(@TicketId) <> 36
	begin
		set @ErrorValue = 0
		return
	end
	
	declare @ErrorCount int    --错误计数器
	declare @TourId char(36)   --团队Id
	declare @TourState  tinyint  --团队状态
	declare @TicketState tinyint  --机票申请状态
	set @ErrorCount = 0
	
	--保存必要数据
	select @TourId = TourId,@TicketState = [State] from tbl_PlanTicketOut where ID = @TicketId
	
	if @TourId is null or len(@TourId) < 1 or @TicketState < 0
	begin
		set @ErrorValue = -1
		return
	end
	
	select @TourState = [Status] from tbl_Tour where TourId = @TourId

	--团队递交财务后和已出票的不能取消
	if @TourState in (4,5) or @TicketState in (3)
	begin
		set @ErrorValue = -2
		return
	end

	begin tran UNChecked

	--删除机票管理及机票计调安排信息的关联
	DELETE FROM tbl_PlanTicketPlanId WHERE PlanId=@TicketId
	SET @ErrorCount = @ErrorCount + @@ERROR 
	
	--1、删除机票申请列表（tbl_PlanTicket）
	delete from tbl_PlanTicket where RefundId = @TicketId
	--验证错误
	SET @ErrorCount = @ErrorCount + @@ERROR 

	--2、删除机票申请航班信息（tbl_PlanTicketFlight）
	delete from tbl_PlanTicketFlight where TicketId = @TicketId
	--验证错误
	SET @ErrorCount = @ErrorCount + @@ERROR 

	--3、删除机票申请票款信息（tbl_PlanTicketKind）
	delete from tbl_PlanTicketKind where TicketId = @TicketId
	--验证错误
	SET @ErrorCount = @ErrorCount + @@ERROR 

	--4、删除机票申请游客关系（tbl_PlanTicketOutCustomer）
	delete from tbl_PlanTicketOutCustomer where TicketOutId = @TicketId
	--验证错误
	SET @ErrorCount = @ErrorCount + @@ERROR 

	--5、删除机票申请基本信息（tbl_PlanTicketOut）
	delete from tbl_PlanTicketOut where ID = @TicketId
	--验证错误
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
-- Author:		汪奇志
-- Create date: 2011-07-04
-- Description:	批量登记付款
-- @ExpenseType:(0:单项服务供应商安排，1:地接，2:票务，255:所有)
-- History:
-- 1.2012-03-13 汪奇志 更新机票付款登记的条件(未出票的不登记)
-- =============================================
ALTER PROCEDURE [dbo].[proc_BatchRegisterExpense]
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
			,0,B.SupplierId,B.SupplierName,@PaymentTime,@PayerId
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
			,1,B.TravelAgencyID,B.LocalTravelAgency,@PaymentTime,@PayerId
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
--以上日志已更新至服务器 2012-03-19




--(201202150001)望海假期需求.doc 
--2012-03-20 汪奇志 增加线路区域排序编号
ALTER TABLE dbo.tbl_Area ADD
	SortId int NOT NULL CONSTRAINT DF_tbl_Area_SortId DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'排序编号'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Area', N'COLUMN', N'SortId'
GO
--=========================================
--描述：线路修改
--创建人：xuqh
--时间：2011-02-22
--History:
--1.增加线路区域类型 2011-10-17
--2.汪奇志 2012-03-20 增加排序编号
--=========================================
ALTER proc [dbo].[proc_UserArea_Update]
	@Id int,	--线路区域编号
	@AreaName nvarchar(200),		--线路区域名称
	@UserAreaXml nvarchar(max),		--线路区计调员
	--XML:<ROOT><UserAreaInfo UserId="1" AreaId="1" ContactName="管理" /></ROOT>
	@Result int output		--返回结果 1成功 0失败
	--@AreaType TINYINT=0--线路区域类型
	,@SortId INT=0--排序编号
as
begin
	declare @hdoc int
	set @Result = 0
	
	begin try
		begin transaction tran_update
		--更新线路区域名称
		update tbl_Area set AreaName = @AreaName,SortId=@SortId where Id = @Id

		--更新计调员信息
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
--描述:获取线路信息
--创建人:xuqh
--时间:2011-02-22
--History:
--1:2012-03-20 汪奇志 增加排序编号
--======================================
ALTER proc [dbo].[proc_Area_GetAreaInfo]
	@Id int --线路编号
as
begin
	--获取线路区域信息
	select Id,AreaName,CompanyId,OperatorId,IssueTime,IsDelete,SortId
	from tbl_Area where Id = @Id and IsDelete = '0'
	
	--获取线路区域对应计调员信息
	select a.UserId,a.AreaId,b.ContactName from tbl_UserArea a inner join tbl_CompanyUser b
	on a.UserId = b.Id where a.AreaId = @Id and UserType = 2 and IsDelete = '0' and IsEnable = '1'
end
GO
--(201202150001)望海假期需求.doc

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
	DECLARE @tmptbl_zhichu TABLE(ZhiChuLeiBie NVARCHAR(10),TourId CHAR(36),PlanId CHAR(36),Amount MONEY,GysId INT,GysName NVARCHAR(255))
	
	EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@TourIdsXML
	INSERT INTO @tmptbl_tour(TourId) SELECT A.TourId FROM OPENXML(@hdoc,'/ROOT/Info') WITH(TourId CHAR(36)) AS A
	EXECUTE sp_xml_removedocument @hdoc

	IF(@GysType=255)--单项支出
	BEGIN
		IF(@GysName IS NOT NULL AND LEN(@GysName)>0)
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT '单项',A.TourId,B.PlanId,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.PlanId AND [ReceiveType]=0)),B.SupplierId,B.SupplierName FROM @tmptbl_tour AS A INNER JOIN tbl_PlanSingle AS B
			ON A.TourId=B.TourId AND B.SupplierName LIKE'%'+@GysName+'%'
		END
		ELSE
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT '单项',A.TourId,B.PlanId,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.PlanId AND [ReceiveType]=0)),B.SupplierId,B.SupplierName FROM @tmptbl_tour AS A INNER JOIN tbl_PlanSingle AS B
			ON A.TourId=B.TourId
		END
	END

	IF(@GysType=1 OR @GysType=255)--地接支出
	BEGIN
		IF(@GysName IS NOT NULL AND LEN(@GysName)>0)
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT '地接',A.TourId,B.Id,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=1)),B.TravelAgencyID,B.LocalTravelAgency FROM @tmptbl_tour AS A INNER JOIN tbl_PlanLocalAgency AS B
			ON A.TourId=B.TourId AND B.LocalTravelAgency LIKE'%'+@GysName+'%'
		END
		ELSE
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT '地接',A.TourId,B.Id,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=1)),B.TravelAgencyID,B.LocalTravelAgency FROM @tmptbl_tour AS A INNER JOIN tbl_PlanLocalAgency AS B
			ON A.TourId=B.TourId
		END
	END

	IF(@GysType=2 OR @GysType=255)--机票支出
	BEGIN
		IF(@GysName IS NOT NULL AND LEN(@GysName)>0)
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT '机票',A.TourId,B.Id,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=2)),B.TicketOfficeId,B.TicketOffice FROM @tmptbl_tour AS A INNER JOIN tbl_PlanTicketOut AS B
			ON A.TourId=B.TourId AND B.State=3 AND B.TicketOffice LIKE'%'+@GysName+'%'
		END
		ELSE
		BEGIN
			INSERT INTO @tmptbl_zhichu(ZhiChuLeiBie,TourId,PlanId,Amount,GysId,GysName)
			SELECT '机票',A.TourId,B.Id,(B.TotalAmount-(SELECT ISNULL(SUM([PaymentAmount]),0) FROM tbl_FinancialPayInfo WHERE ItemId=A.TourId AND ItemType=0 AND [ReceiveId]=B.Id AND [ReceiveType]=2)),B.TicketOfficeId,B.TicketOffice FROM @tmptbl_tour AS A INNER JOIN tbl_PlanTicketOut AS B
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


--同行平台调整 
--2012-04-01 汪奇志 同行平台基本信息表增加字段联系方式（左侧）
ALTER TABLE dbo.tbl_Site ADD
	LianXiFangShi nvarchar(MAX) NULL
GO
DECLARE @v sql_variant 
SET @v = N'联系方式'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Site', N'COLUMN', N'LianXiFangShi'
GO

--2012-04-03 汪奇志 系统域名表增加字段域名类型
ALTER TABLE dbo.tbl_SysDomain ADD
	Type tinyint NOT NULL CONSTRAINT DF_tbl_SysDomain_Type DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'域名类型'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_SysDomain', N'COLUMN', N'Type'
GO

--2012-04-05 汪奇志 计划信息表增加是否最近出团字段
ALTER TABLE dbo.tbl_Tour ADD
	IsRecently char(1) NOT NULL CONSTRAINT DF_tbl_Tour_IsRecently DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'是否最近出团'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'IsRecently'
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-04-23
-- Description:	创建子系统
-- =============================================
ALTER PROCEDURE [dbo].[proc_Sys_CreateSys]
	@SysName NVARCHAR(255),--系统名称
	@SysDomainsXML NVARCHAR(MAX),--系统域名XML:<ROOT><Info Domain="域名" URL="目标位置url" DoimainType"域名类型" /></ROOT>
	@FirstPermission NVARCHAR(MAX),--系统一级栏目
	@SecondPermission NVARCHAR(MAX),--系统二级栏目
	@ThirdPermission NVARCHAR(MAX),--系统权限
	@CompanyName NVARCHAR(255),--公司名称
	@Realname NVARCHAR(255),--联系人姓名
	@Telephone NVARCHAR(255),--联系电话
	@Mobile NVARCHAR(255),--联系手机
	@Fax NVARCHAR(255),--联系传真
	@DepartmentName NVARCHAR(255),--总部部门名称
	@Username NVARCHAR(255),--管理员账号
	@Password NVARCHAR(255),--管理员密码(明文)
	@MD5Password NVARCHAR(255),--管理员密码(MD5)
	@SettingsXML NVARCHAR(MAX),--公司配置XML:<ROOT><Info Key="配置KEY" Value="配置VALUE" /></ROOT>
	@Result INT OUTPUT,--结果 1:成功 其它失败
	@SysId INT OUTPUT, --系统编号
	@CompanyId INT OUTPUT--公司编号
AS
BEGIN
	DECLARE @errorcount INT
	DECLARE @hdoc INT
	DECLARE @UserId INT--用户编号
	DECLARE @DepartmentId INT--部门编号
	DECLARE @RoleId INT--角色编号

	
	SET @SysId=0
	SET @CompanyId=0
	SET @Result=0
	SET @errorcount=0

	IF(@SysDomainsXML IS NOT NULL AND LEN(@SysDomainsXML)>0)
	BEGIN
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@SysDomainsXML
		IF EXISTS(SELECT 1 FROM [tbl_SysDomain] WHERE [Domain] IN(SELECT A.Domain FROM OPENXML(@hdoc,'/ROOT/Info') WITH(Domain NVARCHAR(255)) AS A WHERE A.Domain IS NOT NULL OR LEN(A.Domain)>0))--验证域名重复
		BEGIN
			SET @Result=-1
			RETURN -1
		END
		EXECUTE sp_xml_removedocument @hdoc
	END	

	BEGIN TRAN
	--写入系统信息
	INSERT INTO [tbl_Sys]([SysName],[Module],[Part],[Permission],[CreateTime])
	VALUES(@SysName,@FirstPermission,@SecondPermission,@ThirdPermission,GETDATE())
	SET @errorcount=@errorcount+@@ERROR
	SET @SysId=@@IDENTITY

	IF(@errorcount=0 AND @SysDomainsXML IS NOT NULL AND LEN(@SysDomainsXML)>0)--写入系统域名信息
	BEGIN
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@SysDomainsXML
		INSERT INTO [tbl_SysDomain]([SysId],[Domain],[Url],[Type])
		SELECT @SysId,A.Domain,A.Url,A.DomainType FROM OPENXML(@hdoc,'/ROOT/Info') WITH(Domain NVARCHAR(255),URL NVARCHAR(255),DomainType TINYINT) AS A WHERE A.Domain IS NOT NULL OR LEN(A.Domain)>0
		SET @errorcount=@errorcount+@@ERROR
		EXECUTE sp_xml_removedocument @hdoc
	END

	IF(@errorcount=0)--写入公司信息
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

	IF(@errorcount=0)--写入管理员角色
	BEGIN
		INSERT INTO [tbl_SysRoleManage]([RoleName],[RoleChilds],[CompanyId],[IsDelete])
		VALUES('管理员',@ThirdPermission,@CompanyId,'0')
		SET @errorcount=@errorcount+@@ERROR
		SET @RoleId=@@IDENTITY
	END	

	IF(@errorcount=0)--写入部门信息
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

	IF(@errorcount=0)--写入管理员账号
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

	IF(@errorcount=0)--将管理员默认总部部门负责人
	BEGIN
		UPDATE [tbl_CompanyDepartment] SET [DepartManger]=@UserId WHERE Id=@DepartmentId
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount=0)--默认添加门市客户等级
	BEGIN
		INSERT INTO [tbl_CompanyCustomStand]([CustomStandName],[CompanyId],[OperatorId],[IssueTime],[IsSystem],[IsDelete],[LevType])
		VALUES('门市',@CompanyId,0,GETDATE(),'1','0',1)
		SET @errorcount=@errorcount+@@ERROR
	END
	IF(@errorcount=0)--默认添加同行客户等级
	BEGIN
		INSERT INTO [tbl_CompanyCustomStand]([CustomStandName],[CompanyId],[OperatorId],[IssueTime],[IsSystem],[IsDelete],[LevType])
		VALUES('同行',@CompanyId,0,GETDATE(),'1','0',2)
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount=0)--默认添加常规报价标准
	BEGIN
		INSERT INTO [tbl_CompanyPriceStand]([PriceStandName],[CompanyId],[OperatorId],[IssueTime],[IsDelete])
		VALUES('常规',@CompanyId,0,GETDATE(),'0')
	END

	IF(@errorcount=0)--导入系统默认的省份城市信息
	BEGIN
		DECLARE @i INT--计数器
		DECLARE @ProvinceId INT--公司省份编号
		DECLARE @ProvinceCount INT--省份数量
		
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

	IF(@errorcount=0)--写入公司配置
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
-- Author:		汪奇志
-- Create date: 2011-04-25
-- Description:	更新子系统
-- =============================================
ALTER PROCEDURE [dbo].[proc_Sys_UpdateSys]
	@SysId INT,--系统编号
	@SysDomainsXML NVARCHAR(MAX),--系统域名XML:<ROOT><Info Domain="域名" URL="目标位置url" DomainType="域名类型" /></ROOT>
	@FirstPermission NVARCHAR(MAX),--系统一级栏目
	@SecondPermission NVARCHAR(MAX),--系统二级栏目
	@ThirdPermission NVARCHAR(MAX),--系统权限	
	@Username NVARCHAR(255),--管理员账号
	@Password NVARCHAR(255),--管理员密码(明文)
	@MD5Password NVARCHAR(255),--管理员密码(MD5)
	@SettingsXML NVARCHAR(MAX),--公司配置XML:<ROOT><Info Key="配置KEY" Value="配置VALUE" /></ROOT>
	@Result INT OUTPUT--结果 1:成功 其它失败
AS
BEGIN
	DECLARE @errorcount INT
	DECLARE @CompanyId INT--公司编号
	DECLARE @UserId INT--管理员账户编号
	DECLARE @hdoc INT

	SET @errorcount=0

	IF(@SysDomainsXML IS NOT NULL AND LEN(@SysDomainsXML)>0)
	BEGIN
		EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@SysDomainsXML
		IF EXISTS(SELECT 1 FROM [tbl_SysDomain] WHERE [SysId]<>@SysId AND [Domain] IN(SELECT A.Domain FROM OPENXML(@hdoc,'/ROOT/Info') WITH(Domain NVARCHAR(255)) AS A WHERE A.Domain IS NOT NULL OR LEN(A.Domain)>0))--验证域名重复
		BEGIN
			SET @Result=-1
			RETURN -1
		END
		EXECUTE sp_xml_removedocument @hdoc
	END	

	SELECT @CompanyId=[Id] FROM [tbl_CompanyInfo] WHERE [SystemId]=@SysId
	SELECT TOP(1) @UserId=[Id] FROM [tbl_CompanyUser] WHERE [CompanyId]=@CompanyId AND [IsAdmin]='1'

	BEGIN TRAN
	--更新系统信息
	UPDATE [tbl_Sys] SET [Module]=@FirstPermission,[Part]=@SecondPermission,[Permission]=@ThirdPermission WHERE [SysId]=@SysId

	IF(@Password IS NOT NULL AND LEN(@Password)>0)--更新管理员账号信息
	BEGIN
		UPDATE [tbl_CompanyUser] SET [UserName]=@Username,[Password]=@Password,[MD5Password]=@MD5Password WHERE [Id]=@UserId
	END

	IF(@errorcount=0 AND @SysDomainsXML IS NOT NULL AND LEN(@SysDomainsXML)>0)--系统域名
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

	IF(@errorcount=0)--公司配置
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

	
	/*IF(@errorcount=0)--角色权限处理，去除该系统每个角色权限在该系统中不存在的权限
	BEGIN
		
	END

	IF(@errorcount=0)--用户权限处理，去除该系统每个用户权限在该系统中不存在的权限
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
-- Author:		汪奇志
-- Create date: 2012-04-09
-- Description:	标记最近出团
-- =============================================
CREATE PROCEDURE proc_Tour_IsRecently
	@TourId CHAR(36)--计划编号
	,@IsT CHAR(1)--是否模板团
AS
BEGIN
	DECLARE @TTourId CHAR(36)--模板团编号
	DECLARE @RTourId CHAR(36)--最近出团的团队编号
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
-- Author:		汪奇志
-- Create date: 2011-01-28
-- Description:	计划信息维护SQL计划
-- History:
-- 1.汪奇志 2012-04-09 增加最近出团处理
-- =============================================
ALTER PROCEDURE [dbo].[SQLPlan_Tour]

AS
BEGIN
	--标记团队已出团状态
	UPDATE tbl_Tour SET IsLeave='1' WHERE TourType IN(0,1) AND DATEDIFF(DAY,GETDATE(),[LeaveDate])<=0 

	--标记团队状态为行程途中
	UPDATE tbl_Tour SET Status=2 WHERE TourType IN(0,1) AND [Status] IN(0,1,3) AND DATEDIFF(DAY,GETDATE(),[LeaveDate])<=0 AND DATEDIFF(DAY,GETDATE(),[RDate])>0

	--标记团队状态为回团报账
	UPDATE tbl_Tour SET Status=3 WHERE TourType IN(0,1) AND [Status] IN(0,1,2) AND DATEDIFF(DAY,GETDATE(),[RDate])<0	


	--最近出团处理
	--表变量用来存放需要标记最近出团、最近出团数量的模板团
	DECLARE @tmptbl TABLE(TourId CHAR(36))
	INSERT INTO @tmptbl(TourId) SELECT TourId FROM tbl_Tour WHERE TemplateId='' AND TourType=0
	--模板团编号
	DECLARE @TemplateTourId CHAR(36)

	DECLARE tmpcursor CURSOR
	FOR SELECT TourId FROM @tmptbl
	OPEN tmpcursor
	FETCH NEXT FROM tmpcursor INTO @TemplateTourId
	
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		--标记最近出团
		EXECUTE proc_Tour_IsRecently @TourId=@TemplateTourId,@IsT=N'1'
		FETCH NEXT FROM tmpcursor INTO @TemplateTourId
	END

	CLOSE tmpcursor
	DEALLOCATE tmpcursor
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
-- 5.2012-04-09 汪奇志 增加最近出团处理
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
-- Author:		汪奇志
-- Create date: 2011-01-23
-- Description:	更新散拼计划、团队计划、单项服务信息
-- History:
-- 1.2011-07-11 团队计划增加人数和价格信息，删除SelfUnitPriceAmount
-- 2.2012-04-09 汪奇志 增加最近出团处理
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
-- =============================================
-- Author:		周文超
-- Create date: 2011-04-25
-- Description:	删除团队信息（虚拟删除）
-- Error：0参数错误
-- Success：1删除成功
-- History:
-- 1.2011-12-16 汪奇志 增加团款支出登记的删除
-- 2.2012-03-03 汪奇志 增加机票管理及机票计调安排信息的关联的删除
-- 3.2012-04-09 汪奇志 增加最近出团处理
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

	--线路Id赋值
	select @RouteId = RouteId from tbl_Tour where TourId = @TourId
	--上团数赋值
	select @TourCount = count(*) from tbl_Tour as a where a.RouteId = @RouteId and a.IsDelete = '0' and a.TemplateId <> ''
	--收客数赋值
	select @CustomerCount = sum(PeopleNumber) from tbl_TourOrder as a where a.TourId in (select b.TourId from tbl_Tour as b where b.RouteId =@RouteId and b.IsDelete = '0') and a.IsDelete = '0' and a.OrderState not in (3,4)
	--维护线路区域的上团数、收客数
	update tbl_Route set TourCount = isnull(@TourCount,0),VisitorCount = isnull(@CustomerCount,0) where ID = @RouteId
	SET @errorcount = @errorcount + @@ERROR

	IF(@errorcount=0)--标记最近出团
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

--数据处理部分：
--最近出团处理
EXEC SQLPlan_Tour
GO
--域名处理 标记组团入口域名
UPDATE [tbl_SysDomain] SET [Type]=1 WHERE LEN([Url])>0
GO

--同行平台调整 

--以上日志均已更新



-- =============================================
-- Author:		<Author,,李焕超>
-- Create date: <Create Date,2011-02-14,>
-- Description:	<Description,退票,>
-- History:
-- 1.2011-04-21 汪奇志 过程调整 
-- 2.2011-08-09 周文超 退票金额杂费收入默认已收
-- 3.2012-05-28 汪奇志 修正已完成退票的修改操作不输出@TourId的BUG
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

		IF NOT EXISTS(SELECT 1 FROM [tbl_TourOtherCost] WHERE [ItemId] =@travellerRefundId AND [ItemType]=1 AND [CostType]=0)--写杂费收入
		BEGIN			
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
           --============================更新退款金额========================
        update tbl_CustomerRefund set RefundAmount=@ReturnMoney,IsRefund=case @State when 4 then 1 else 0 end,RefundNote=@Remark where CustormerId in(select b.UserId from tbl_PlanTicketOut a join tbl_PlanTicketOutCustomer b on a.id=b.TicketOutId where a.id=@TicketOutId)
         set  @error =@error +@@error
        update tbl_TourOrderCustomer set TicketStatus=0 where ID=@UserId
         set  @error =@error +@@error
        update tbl_Planticket set tbl_Planticket.state=@State where id=@TicketId
         set  @error =@error +@@error
--        update tbl_PlanTicketOutCustomer set IsApplyRefund=1 where TicketOutId=@TicketOutId
--          set  @error =@error +@@error
           --============================更新退款金额========================

           --============================同步杂费收入========================
       declare @isExists int 
       select @isExists=count(ItemId) from  tbl_TourOtherCost where ItemId =@TicketOutId and ItemType=1 and CostType=0
       if @isExists>0 ---存在，执行修改
        begin
          declare @difference decimal --差额
          declare @oldReturnAmont decimal --原来的退还金额

          select @oldReturnAmont=Proceed from tbl_TourOtherCost where ItemId=@TicketOutId and ItemType=1 and CostType =0
          set @difference =@ReturnMoney-@oldReturnAmont
         update tbl_TourOtherCost set Proceed =Proceed+@difference,SumCost=Proceed+IncreaseCost-ReduceCost+@difference where ItemId=@TicketOutId and ItemType=1 and CostType =0
         set  @error =@error +@@error
       end
       else ----不存在，执行添加
        begin
          INSERT INTO [tbl_TourOtherCost]
           (id,[CompanyId],[TourId],[CostType],[Proceed]
           ,[SumCost],[Remark],OperatorId,PayTime,CreateTime,[ItemType],[ItemId],ProceedItem)
          VALUES
           (newid(),@companyId,@tourId,0,@ReturnMoney
           ,@ReturnMoney,@Remark,@OperateID ,getdate(),getdate(),1,@TicketOutId,'机票退票款'
            )
        set  @error =@error +@@error
        end
           --============================同步杂费收入========================

--          --============================同步游客信息========================
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
--       --============================同步游客信息结束========================
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

--以上日志已更新 2012-05-28
GO


--2012-05-29 汪奇志 地接安排价格组成具体要求字段更改成NVARCHAR(MAX)
ALTER TABLE [tbl_PlanLocalAgencyPrice] 
ALTER COLUMN [Remark] NVARCHAR(MAX)
GO

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
--以上日志已更新 2012-05-29 汪奇志
GO
-- =============================================
-- Author:		周文超
-- Create date: 2011-01-26
-- Description:	重新计算团队支出相关项、所有支出相关项、
-- Error：0参数错误
-- Error：-1计算过程中发生错误
-- Success：1重新计算成功
-- History:
-- 1.2012-06-13 汪奇志 修复支出同步时未更新tbl_StatAllOut表供应商编号的BUG
-- =============================================
ALTER PROCEDURE [dbo].[proc_Utility_CalculationTourOut]
	@TourId char(36) = null,         --团队Id
	@ItemIdAndTypeXML nvarchar(max),    --支出项目编号、类型集合
	@ErrorValue  int = 0 output    --错误代码
AS
BEGIN

	declare @ErrorCount int    --错误计数器
	declare @TourType tinyint  --团队类型
	declare @TotalExpenses money  --团队计调总支出
	declare @TotalOtherExpenses money   --团队杂费总支出
	declare @ItemCount int    --项存在的数量
	declare @Amount money     --金额
	declare @AddAmount money  --增加费用
	declare @ReduceAmount money   --减少费用
	declare @TotalAmount money  --合计金额
	declare @SupplierId  int   --供应商编号
	declare @SupplierName  nvarchar(255)    --供应商名称
	set @ErrorCount = 0
	set @TotalExpenses = 0
	set @TotalOtherExpenses = 0
	set @ItemCount = 0
	set @AddAmount = 0
	set @ReduceAmount = 0
	set @TotalAmount = 0
	set @Amount = 0

	create table #tbl_ItemIdAndType(ItemId char(36),ItemType tinyint)  --项目临时表

	begin tran 	Calculation

	if @ItemIdAndTypeXML is not null and len(@ItemIdAndTypeXML) > 0
	begin
		set @ItemIdAndTypeXML = replace(replace(@ItemIdAndTypeXML,char(13),' '),char(10),' ')
		DECLARE @hdoc int
		EXEC sp_xml_preparedocument @hdoc OUTPUT, @ItemIdAndTypeXML
		insert into #tbl_ItemIdAndType(ItemId,ItemType) SELECT ItemId,ItemType FROM OPENXML(@hdoc,N'/ROOT/ItemIdAndType')
		WITH (ItemId char(36),ItemType tinyint)
		--验证错误
		SET @ErrorCount = @ErrorCount + @@ERROR
		EXEC sp_xml_removedocument @hdoc 
	end

	--团队Id不为空的时候重新计算团队计调总支出、杂费总支出
	if @TourId is not null and len(@TourId) = 36
	begin
		select @TourType = TourType from tbl_Tour where TourId = @TourId
		if @TourType = 2  --单项服务团
		begin
			select @TotalExpenses = @TotalExpenses + isnull(sum(TotalAmount),0) from tbl_PlanSingle where TourId = @TourId
			--查询计调项
			insert into #tbl_ItemIdAndType(ItemId,ItemType) select PlanId,4 from tbl_PlanSingle where  TourId = @TourId
		end
		else     --散拼计划、团队计划团
		begin
			--查询团队地接总支出
			select @TotalExpenses = @TotalExpenses + isnull(sum(TotalAmount),0) from tbl_PlanLocalAgency where TourId = @TourId
			--查询计调项
			insert into #tbl_ItemIdAndType(ItemId,ItemType) select Id,1 from tbl_PlanLocalAgency where TourId = @TourId
			
			--查询团队机票总支出
			select @TotalExpenses = @TotalExpenses + isnull(sum(TotalAmount),0) from tbl_PlanTicketOut where TourId = @TourId and [State] = 3
			--查询计调项
			insert into #tbl_ItemIdAndType(ItemId,ItemType) select Id,2 from tbl_PlanTicketOut where TourId = @TourId
		end
		--查询团队的杂费总支出
		select @TotalOtherExpenses = @TotalOtherExpenses + isnull(sum(SumCost),0) from tbl_TourOtherCost where TourId = @TourId and CostType = 1
		--查询团队杂费支出项
		insert into #tbl_ItemIdAndType(ItemId,ItemType) select Id,3 from tbl_TourOtherCost where TourId = @TourId and CostType = 1

		--更新团队计调总支出、团队杂费总支出
		update tbl_Tour set TotalExpenses = @TotalExpenses,TotalOtherExpenses = @TotalOtherExpenses where TourId = @TourId 
		--验证错误
		SET @ErrorCount = @ErrorCount + @@ERROR
	end

	declare @ItemId char(36)   --项Id
	declare @ItemType  tinyint  --项类型
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

		if @ItemType = 1  --地接支出
		begin
			select @Amount = isnull(Settlement,0),@AddAmount = isnull(AddAmount,0),@ReduceAmount = isnull(ReduceAmount,0),@TotalAmount = isnull(TotalAmount,0),@SupplierId = isnull(TravelAgencyID,0),@SupplierName = isnull(LocalTravelAgency,'') from tbl_PlanLocalAgency where ID = @ItemId
		end
		else if @ItemType = 2  --机票支出
		begin
			select @Amount = isnull(Total,0),@AddAmount = isnull(AddAmount,0),@ReduceAmount = isnull(ReduceAmount,0),@TotalAmount = isnull(TotalAmount,0),@SupplierId = isnull(TicketOfficeId,0),@SupplierName = isnull(TicketOffice,'') from tbl_PlanTicketOut where ID = @ItemId
		end
		else if @ItemType = 3  --杂费支出
		begin
			select @Amount = isnull(Proceed,0),@AddAmount = isnull(IncreaseCost,0),@ReduceAmount = isnull(ReduceCost,0),@TotalAmount = isnull(SumCost,0),@SupplierId = isnull(CustromCId,0),@SupplierName = isnull(CustromCName,'') from tbl_TourOtherCost where ID = @ItemId
		end
		else if @ItemType = 4  --单项服务供应商安排
		begin
			select @Amount = isnull(Amount,0),@AddAmount = isnull(AddAmount,0),@ReduceAmount = isnull(ReduceAmount,0),@TotalAmount = isnull(TotalAmount,0),@SupplierId = isnull(SupplierId,0),@SupplierName = isnull(SupplierName,'') from tbl_PlanSingle where PlanId = @ItemId
		end

		select @ItemCount = count(Id) from tbl_StatAllOut where ItemId = @ItemId and ItemType = @ItemType
		if @ItemCount > 0
		begin
			--更新统计分析所有支出表
			update tbl_StatAllOut set Amount = @Amount,AddAmount = @AddAmount,ReduceAmount = @ReduceAmount,TotalAmount = @TotalAmount,SupplierId = @SupplierId,SupplierName = @SupplierName where ItemId = @ItemId and ItemType = @ItemType
			--验证错误
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
	
	drop table #tbl_ItemIdAndType  --删除临时表
	
END
GO
--以上日志已更新 2012-06-13 汪奇志
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
--以上日志已更新 2012-07-09 汪奇志
GO


--计划服务标准信息表增加索引 汪奇志 2012-07-11
ALTER TABLE dbo.tbl_TourService
	DROP CONSTRAINT PK_TBL_TOURSERVICE
GO
CREATE CLUSTERED INDEX IX_tbl_TourService_01 ON dbo.tbl_TourService
	(
	TourId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
DECLARE @v sql_variant 
SET @v = N'团号索引'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourService', N'INDEX', N'IX_tbl_TourService_01'
GO

--计划行程信息表增加索引
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
SET @v = N'团号索引'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourPlan', N'INDEX', N'IX_tbl_TourPlan_01'
GO
--以上日志已更新 2012-07-11 094700 汪奇志
GO

