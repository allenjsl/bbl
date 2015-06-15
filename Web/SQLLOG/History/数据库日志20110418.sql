

--========================================巴比来  二次修改   数据库日志   开始  =================================
GO
--线路产品库地接我社报价明细增加地接及我社报价人数、单价字段
ALTER TABLE dbo.tbl_RouteQuoteList ADD
	LocalPeopleNumber int NOT NULL CONSTRAINT DF_tbl_RouteQuoteList_LocalPeopleNumber DEFAULT 0,
	LocalUnitPrice money NOT NULL CONSTRAINT DF_tbl_RouteQuoteList_LocalUnitPrice DEFAULT 0,
	SelfPeopleNumber int NOT NULL CONSTRAINT DF_tbl_RouteQuoteList_SelfPeopleNumber DEFAULT 0,
	SelfUnitPrice money NOT NULL CONSTRAINT DF_tbl_RouteQuoteList_SelfUnitPrice DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'地接报价人数'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_RouteQuoteList', N'COLUMN', N'LocalPeopleNumber'
GO
DECLARE @v sql_variant 
SET @v = N'地接报价单价'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_RouteQuoteList', N'COLUMN', N'LocalUnitPrice'
GO
DECLARE @v sql_variant 
SET @v = N'我社报价人数'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_RouteQuoteList', N'COLUMN', N'SelfPeopleNumber'
GO
DECLARE @v sql_variant 
SET @v = N'我社报价单价'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_RouteQuoteList', N'COLUMN', N'SelfUnitPrice'
GO

--团队计划报价信息表增加地接及我社报价人数、单价字段
ALTER TABLE dbo.tbl_TourTeamPrice ADD
	LocalPeopleNumber int NOT NULL CONSTRAINT DF_tbl_TourTeamPrice_LocalPeopleNumber DEFAULT 0,
	LocalUnitPrice money NOT NULL CONSTRAINT DF_tbl_TourTeamPrice_LocalUnitPrice DEFAULT 0,
	SelfPeopleNumber int NOT NULL CONSTRAINT DF_tbl_TourTeamPrice_SelfPeopleNumber DEFAULT 0,
	SelfUnitPrice money NOT NULL CONSTRAINT DF_tbl_TourTeamPrice_SelfUnitPrice DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'地接报价人数'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourTeamPrice', N'COLUMN', N'LocalPeopleNumber'
GO
DECLARE @v sql_variant 
SET @v = N'地接报价单价'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourTeamPrice', N'COLUMN', N'LocalUnitPrice'
GO
DECLARE @v sql_variant 
SET @v = N'我社报价人数'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourTeamPrice', N'COLUMN', N'SelfPeopleNumber'
GO
DECLARE @v sql_variant 
SET @v = N'我社报价单价'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourTeamPrice', N'COLUMN', N'SelfUnitPrice'
GO





--周文超  供应商增加航空公司后对应的权限插入
GO
INSERT [tbl_SysPermissionClass] ([Id],[CategoryId],[ClassName],[SortId],[IsEnable]) VALUES ( 66,8,'航空公司',0,'1')
INSERT [tbl_SysPermissionClass] ([Id],[CategoryId],[ClassName],[SortId],[IsEnable]) VALUES ( 67,9,'销售统计',0,'1')
INSERT [tbl_SysPermissionList] ([Id],[ClassId],[PermissionName],[SortId],[IsEnable]) VALUES ( 231,66,'栏目',0,'1')
INSERT [tbl_SysPermissionList] ([Id],[ClassId],[PermissionName],[SortId],[IsEnable]) VALUES ( 232,66,'新增',0,'1')
INSERT [tbl_SysPermissionList] ([Id],[ClassId],[PermissionName],[SortId],[IsEnable]) VALUES ( 233,66,'修改',0,'1')
INSERT [tbl_SysPermissionList] ([Id],[ClassId],[PermissionName],[SortId],[IsEnable]) VALUES ( 234,66,'删除',0,'1')
INSERT [tbl_SysPermissionList] ([Id],[ClassId],[PermissionName],[SortId],[IsEnable]) VALUES ( 235,66,'导入',0,'1')
INSERT [tbl_SysPermissionList] ([Id],[ClassId],[PermissionName],[SortId],[IsEnable]) VALUES ( 236,66,'导出',0,'1')
INSERT [tbl_SysPermissionList] ([Id],[ClassId],[PermissionName],[SortId],[IsEnable]) VALUES ( 237,67,'栏目',0,'1')
GO

UPDATE [tbl_Sys] 
SET [Part]=[Part]+',66,67'
,
[Permission]=[Permission]+',231,232,233,234,235,236,237'
WHERE SysId=1
GO

UPDATE [tbl_Sys] 
SET [Part]=[Part]+',67',
[Permission]=[Permission]+',237'
WHERE SysId<>1
GO

UPDATE tbl_CompanyUser
SET IsAdmin='1'
WHERE UserName='admin'
GO

UPDATE tbl_CompanyUser
SET PermissionList=PermissionList+',231,232,233,234,235,236,237'
WHERE CompanyID IN (select id from dbo.tbl_CompanyInfo where SystemId=1)
AND TourCompanyId=0 AND IsAdmin='1'
GO

UPDATE tbl_CompanyUser
SET PermissionList=PermissionList+',237'
WHERE CompanyID IN (select id from dbo.tbl_CompanyInfo where SystemId<>1)
AND TourCompanyId=0  AND IsAdmin='1'
GO


--组团社询价表增加合计金额字段
GO
ALTER TABLE dbo.tbl_CustomerQuote ADD
	TotalAmount money NOT NULL CONSTRAINT DF_tbl_CustomerQuote_TotalAmount DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'合计金额'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_CustomerQuote', N'COLUMN', N'TotalAmount'
GO

--组团询价价格组成删除合计金额字段
GO
ALTER TABLE dbo.tbl_CustomerQuotePrice
	DROP CONSTRAINT DF__tbl_Custo__SumMo__5CA1C101--DF__tbl_Custo__SumMo__5D8BC399
GO
ALTER TABLE dbo.tbl_CustomerQuotePrice
	DROP COLUMN SumMoney
GO

--组团询价价格组成增加地接报价人数、单价及我社报价人数、单价
GO
ALTER TABLE dbo.tbl_CustomerQuotePrice ADD
	LocalPeopleNumber int NOT NULL CONSTRAINT DF_tbl_CustomerQuotePrice_LocalPeopleNumber DEFAULT 0,
	LocalUnitPrice money NOT NULL CONSTRAINT DF_tbl_CustomerQuotePrice_LocalUnitPrice DEFAULT 0,
	SelfPeopleNumber int NOT NULL CONSTRAINT DF_tbl_CustomerQuotePrice_SelfPeopleNumber DEFAULT 0,
	SelfUnitPrice money NOT NULL CONSTRAINT DF_tbl_CustomerQuotePrice_SelfUnitPrice DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'地接报价人数'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_CustomerQuotePrice', N'COLUMN', N'LocalPeopleNumber'
GO
DECLARE @v sql_variant 
SET @v = N'地接报价单价'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_CustomerQuotePrice', N'COLUMN', N'LocalUnitPrice'
GO
DECLARE @v sql_variant 
SET @v = N'我社报价人数'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_CustomerQuotePrice', N'COLUMN', N'SelfPeopleNumber'
GO
DECLARE @v sql_variant 
SET @v = N'我社报价单价'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_CustomerQuotePrice', N'COLUMN', N'SelfUnitPrice'
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		周文超
-- Create date: 2011-04-19
-- Description:	客户关系管理-销售统计视图
-- =============================================
CREATE VIEW [dbo].[View_SalesStatistics]
AS
select TourId,TourType,LeaveDate,CompanyId,AreaId
,(case when TourType = 2 then '单项服务' else RouteName end) as RouteName   --线路名称
,(case when TourType = 2 then (select top 1 cast(ServiceType as nvarchar) from tbl_PlanSingle where tbl_PlanSingle.TourId = tbl_Tour.TourId) else (select AreaName from tbl_Area where tbl_Area.Id = tbl_Tour.AreaId) end) as AreaName   --线路区域名称
,(select OperatorId,(select ContactName from tbl_CompanyUser where tbl_CompanyUser.ID = tbl_TourOperator.OperatorId) as OperatorName from tbl_TourOperator where tbl_TourOperator.TourId = tbl_Tour.TourId for xml raw,root('root')) as Logistics  --计调员
,(select BuyCompanyName,SalerId,SalerName,FinanceSum,PeopleNumber,OrderState,
	(select ProvinceName + '  ' + CityName from tbl_Customer where tbl_Customer.ID = tbl_TourOrder.BuyCompanyID) as ProvinceAndCity,  --组团社所在区域
	(case when tbl_TourOrder.OrderType = 1 then '' else tbl_TourOrder.OperatorName end) as OperatorName,  --操作员名称
	(select top 1 DepartmentId from tbl_CustomerContactInfo where tbl_CustomerContactInfo.UserId = tbl_TourOrder.OperatorID) as DepartName		from tbl_TourOrder where tbl_TourOrder.TourId = tbl_Tour.TourId and tbl_TourOrder.IsDelete = '0' and tbl_TourOrder.OrderState not in (3,4) for xml raw,root('root')) as OrderInfo   --订单信息
from tbl_Tour
where tbl_Tour.TemplateId > '' and tbl_Tour.IsDelete = '0'



--周文超    2011-04-19  组团联系人增加传真字段
GO
ALTER TABLE dbo.tbl_CustomerContactInfo ADD
	Fax nvarchar(255) NULL
GO
DECLARE @v sql_variant 
SET @v = N'传真'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_CustomerContactInfo', N'COLUMN', N'Fax'
GO

--以上脚本2011-04-26已更新到服务器










--增加退票航段表   周文超  2011-04-20   
GO
if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('tbl_CustomerRefundFlight') and o.name = 'FK_TBL_CUSTOMERREFUNDFL_REFERENCE_TBL_CUSTOMERREFUND')
alter table tbl_CustomerRefundFlight
   drop constraint FK_TBL_CUSTOMERREFUNDFL_REFERENCE_TBL_CUSTOMERREFUND
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tbl_CustomerRefundFlight')
            and   type = 'U')
   drop table tbl_CustomerRefundFlight
go

/*==============================================================*/
/* Table: tbl_CustomerRefundFlight                              */
/*==============================================================*/
create table tbl_CustomerRefundFlight (
   RefundId             char(36)             not null,
   FlightId             int                  not null,
   CustomerId           char(36)             not null,
   constraint PK_TBL_CUSTOMERREFUNDFLIGHT primary key (RefundId, FlightId, CustomerId)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '游客退票编号',
   'user', @CurrentUser, 'table', 'tbl_CustomerRefundFlight', 'column', 'RefundId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '航段编号',
   'user', @CurrentUser, 'table', 'tbl_CustomerRefundFlight', 'column', 'FlightId'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '游客编号',
   'user', @CurrentUser, 'table', 'tbl_CustomerRefundFlight', 'column', 'CustomerId'
go

alter table tbl_CustomerRefundFlight
   add constraint FK_TBL_CUSTOMERREFUNDFL_REFERENCE_TBL_CUSTOMERREFUND foreign key (RefundId)
      references tbl_CustomerRefund (Id)
go




--删除订单游客退票表的 航班时间（FlightTime）、始发地（SendPlace）、目的地（EndPlace）、人数（CustomerNum）  字段   周文超 2011-04-20
GO
ALTER TABLE dbo.tbl_CustomerRefund
	DROP CONSTRAINT DF__tbl_Custo__Custo__607251E5--DF__tbl_Custo__Custo__4B7734FF
GO
ALTER TABLE dbo.tbl_CustomerRefund
	DROP COLUMN FlightTime, SendPlace, EndPlace, CustomerNum
GO


--机票票款信息表增加百分比['Discount]、其它费用['OtherPrice]字段 汪奇志 2011-04-20
GO
ALTER TABLE dbo.tbl_PlanTicketKind ADD
	Discount decimal(8, 6) NOT NULL CONSTRAINT DF_tbl_PlanTicketKind_Discount DEFAULT 1,
	OtherPrice money NOT NULL CONSTRAINT DF_tbl_PlanTicketKind_OtherPrice DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'百分比（折扣）'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_PlanTicketKind', N'COLUMN', N'Discount'
GO
DECLARE @v sql_variant 
SET @v = N'其它费用'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_PlanTicketKind', N'COLUMN', N'OtherPrice'
GO



--Table: tbl_Webmaster 
if exists (select 1
            from  sysobjects
           where  id = object_id('tbl_SysUser')
            and   type = 'U')
   drop table tbl_SysUser
go

if exists (select 1
            from  sysobjects
           where  id = object_id('tbl_Webmaster')
            and   type = 'U')
   drop table tbl_Webmaster
go

CREATE TABLE [dbo].[tbl_Webmaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [varchar](50) NULL,
	[MD5Password] [varchar](50) NULL,
	[Realname] [nvarchar](50) NOT NULL,
	[Telephone] [nvarchar](50) NULL,
	[Fax] [nvarchar](50) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Permissions] [varchar](max) NOT NULL,
	[IsEnable] [char](1) NOT NULL CONSTRAINT [DF__tbl_Webma__IsEna__5A6F5FCC]  DEFAULT ('0'),
	[IsAdmin] [char](1) NOT NULL CONSTRAINT [DF__tbl_Webma__IsAdm__5B638405]  DEFAULT ('0'),
	[CreateTime] [datetime] NOT NULL CONSTRAINT [DF__tbl_Webma__Creat__5C57A83E]  DEFAULT (getdate()),
 CONSTRAINT [PK_TBL_WEBMASTER] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自动编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Webmaster', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Webmaster', @level2type=N'COLUMN',@level2name=N'Username'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'明文密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Webmaster', @level2type=N'COLUMN',@level2name=N'Password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MD5密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Webmaster', @level2type=N'COLUMN',@level2name=N'MD5Password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Webmaster', @level2type=N'COLUMN',@level2name=N'Realname'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'联系电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Webmaster', @level2type=N'COLUMN',@level2name=N'Telephone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'传真' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Webmaster', @level2type=N'COLUMN',@level2name=N'Fax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Webmaster', @level2type=N'COLUMN',@level2name=N'Mobile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色权限，多个权限间用半角逗号间隔' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Webmaster', @level2type=N'COLUMN',@level2name=N'Permissions'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Webmaster', @level2type=N'COLUMN',@level2name=N'IsEnable'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否管理员' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Webmaster', @level2type=N'COLUMN',@level2name=N'IsAdmin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_Webmaster', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO

--增加webmaster account info
GO
SET IDENTITY_INSERT [tbl_Webmaster] ON
INSERT [tbl_Webmaster] ([Id],[Username],[Password],[MD5Password],[Realname],[Telephone],[Fax],[Mobile],[Permissions],[IsEnable],[IsAdmin],[CreateTime]) VALUES ( 1,'webmaster','000000','670b14728ad9902aecba32e22fa4f6bd','汪奇志','0571','0571','15868456678','-1','1','1','2011-04-15 13:57:32')
SET IDENTITY_INSERT [tbl_Webmaster] OFF
GO

--========================================巴比来  二次修改   数据库日志   结束  =================================










--========================================望海需求 （2011-04-28）数据库日志   开始  =================================



--周文超 2011-04-28  在客户等级表增加销售员、计调员提成比例
GO
ALTER TABLE dbo.tbl_CompanyCustomStand ADD
	SalerCommision decimal(8, 6) NOT NULL CONSTRAINT DF_tbl_CompanyCustomStand_SalerCommision DEFAULT 0,
	LogisticsCommision decimal(8, 6) NOT NULL CONSTRAINT DF_tbl_CompanyCustomStand_LogisticsCommision DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'销售员提成比例'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_CompanyCustomStand', N'COLUMN', N'SalerCommision'
GO
DECLARE @v sql_variant 
SET @v = N'计调员提成比例'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_CompanyCustomStand', N'COLUMN', N'LogisticsCommision'
GO




--收入、支出增加减少费用明细信息表
CREATE TABLE [dbo].[tbl_GoldDetail](
	[DetailId] [char](36) NOT NULL,
	[ItemType] [tinyint] NOT NULL DEFAULT ((0)),
	[ItemId] [char](36) NOT NULL,
	[AddAmount] [money] NOT NULL DEFAULT ((0)),
	[ReduceAmount] [money] NOT NULL DEFAULT ((0)),
	[Remark] [nvarchar](max) NULL,
	[OperatorId] [int] NOT NULL DEFAULT ((0)),
	[CreateTime] [datetime] NOT NULL DEFAULT (getdate()),
 CONSTRAINT [PK_TBL_GOLDDETAIL] PRIMARY KEY CLUSTERED 
(
	[DetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'明细编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_GoldDetail', @level2type=N'COLUMN',@level2name=N'DetailId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联类型 详见枚举' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_GoldDetail', @level2type=N'COLUMN',@level2name=N'ItemType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_GoldDetail', @level2type=N'COLUMN',@level2name=N'ItemId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'增加费用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_GoldDetail', @level2type=N'COLUMN',@level2name=N'AddAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'减少费用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_GoldDetail', @level2type=N'COLUMN',@level2name=N'ReduceAmount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_GoldDetail', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作人编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_GoldDetail', @level2type=N'COLUMN',@level2name=N'OperatorId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_GoldDetail', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO


--包含项目接待标准、不含项目、自费项目、儿童安排、购物安排、注意事项、温馨提醒信息表
CREATE TABLE [dbo].[tbl_NotepadService](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL DEFAULT ((0)),
	[Type] [tinyint] NOT NULL DEFAULT ((0)),
	[Text] [nvarchar](max) NULL,
	[OperatorId] [int] NOT NULL DEFAULT ((0)),
	[CreateTime] [datetime] NOT NULL DEFAULT (getdate()),
 CONSTRAINT [PK_TBL_NOTEPADSERVICE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'自动编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_NotepadService', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公司编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_NotepadService', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型 详见枚举' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_NotepadService', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_NotepadService', @level2type=N'COLUMN',@level2name=N'Text'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_NotepadService', @level2type=N'COLUMN',@level2name=N'OperatorId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'添加时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_NotepadService', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO


--支出登账信息表
CREATE TABLE [dbo].[tbl_SpendRegister](
	[RegisterId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL DEFAULT ((0)),
	[PayTime] [datetime] NOT NULL DEFAULT (getdate()),
	[Amount] [money] NOT NULL DEFAULT ((0)),
	[PayType] [tinyint] NOT NULL DEFAULT ((0)),
	[SupplierId] [int] NOT NULL DEFAULT ((0)),
	[SupplierName] [nvarchar](255) NULL,
	[Realname] [nvarchar](255) NULL,
	[Telephone] [nvarchar](255) NULL,
	[Remark] [nvarchar](max) NULL,
	[RegisterTime] [datetime] NOT NULL DEFAULT (getdate()),
	[OperatorId] [int] NOT NULL DEFAULT ((0)),
	[OffAmount] [money] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_TBL_SPENDREGISTER] PRIMARY KEY CLUSTERED 
(
	[RegisterId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支出登账编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegister', @level2type=N'COLUMN',@level2name=N'RegisterId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'公司编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegister', @level2type=N'COLUMN',@level2name=N'CompanyId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支出时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegister', @level2type=N'COLUMN',@level2name=N'PayTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支出金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegister', @level2type=N'COLUMN',@level2name=N'Amount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegister', @level2type=N'COLUMN',@level2name=N'PayType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供应商编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegister', @level2type=N'COLUMN',@level2name=N'SupplierId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供应商名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegister', @level2type=N'COLUMN',@level2name=N'SupplierName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供应商联系人姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegister', @level2type=N'COLUMN',@level2name=N'Realname'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'供应商联系人电话' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegister', @level2type=N'COLUMN',@level2name=N'Telephone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegister', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegister', @level2type=N'COLUMN',@level2name=N'RegisterTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作员编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegister', @level2type=N'COLUMN',@level2name=N'OperatorId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'已销账金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegister', @level2type=N'COLUMN',@level2name=N'OffAmount'
GO

--支出销账明细信息表
CREATE TABLE [dbo].[tbl_SpendRegisterDetail](
	[RegisterId] [int] NOT NULL,
	[Amount] [money] NOT NULL DEFAULT ((0)),
	[ItemId] [char](36) NOT NULL,
	[ItemType] [tinyint] NOT NULL DEFAULT ((0)),
	[OperatorId] [int] NOT NULL DEFAULT ((0)),
	[CreateTime] [datetime] NOT NULL DEFAULT (getdate())
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支出登记编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegisterDetail', @level2type=N'COLUMN',@level2name=N'RegisterId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'销账金额' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegisterDetail', @level2type=N'COLUMN',@level2name=N'Amount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支出项目编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegisterDetail', @level2type=N'COLUMN',@level2name=N'ItemId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支出项目类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegisterDetail', @level2type=N'COLUMN',@level2name=N'ItemType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作人编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegisterDetail', @level2type=N'COLUMN',@level2name=N'OperatorId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'销账时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tbl_SpendRegisterDetail', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
ALTER TABLE [dbo].[tbl_SpendRegisterDetail]  WITH CHECK ADD  CONSTRAINT [FK_TBL_SPENDREGISTERDET_REFERENCE_TBL_SPENDREGISTER] FOREIGN KEY([RegisterId])
REFERENCES [dbo].[tbl_SpendRegister] ([RegisterId])
GO
ALTER TABLE [dbo].[tbl_SpendRegisterDetail] CHECK CONSTRAINT [FK_TBL_SPENDREGISTERDET_REFERENCE_TBL_SPENDREGISTER]
GO

--修改机票管理列表表RouteName字段长度
GO
ALTER TABLE tbl_PlanTicket 
ALTER COLUMN [RouteName] NVARCHAR(255);
go


--========================================望海需求 （2011-04-28）数据库日志   结束  =================================









--周文超 2011-05-11修改，增加退回金额字段
GO
-- =============================================
-- Author:		周文超
-- Create date: 2011-04-19
-- Description:	机票管理--退票统计视图
-- =============================================
alter VIEW [dbo].[View_RefundStatistics]
AS
select a.Id,a.CustormerId,a.OperatorName,a.IssueTime,a.RefundAmount
,b.VisitorName
,c.TourNo,c.RouteName,c.LeaveDate,c.BuyCompanyName,c.OperatorName as OrderOperatorName,c.SellCompanyId,c.BuyCompanyID
,(select sum(TotalMoney) from tbl_PlanTicketKind where tbl_PlanTicketKind.TicketId in (select TicketOutId from tbl_PlanTicketOutCustomer where tbl_PlanTicketOutCustomer.UserId = a.CustormerId)) as TotalAmount
,(select ID,FligthSegment,DepartureTime,AireLine,Discount,TicketId,TicketTime from tbl_PlanTicketFlight where tbl_PlanTicketFlight.ID in (select tbl_CustomerRefundFlight.FlightId from tbl_CustomerRefundFlight where tbl_CustomerRefundFlight.CustomerId = a.CustormerId and tbl_CustomerRefundFlight.RefundId = a.Id) for xml raw,root('root')) as FligthInfo
from tbl_CustomerRefund as a inner join tbl_TourOrderCustomer as b on a.CustormerId = b.Id inner join tbl_TourOrder as c on c.ID = b.OrderId
Go



--(20110523)专线版管理系统.mmap start

-- 2011-05-24 汪奇志 同行平台信息表字段描述更改
GO
DECLARE @v sql_variant 
SET @v = N'公司介绍（企业简介）'
EXECUTE sp_updateextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Site', N'COLUMN', N'Intr'
GO
DECLARE @v sql_variant 
SET @v = N'联系我们'
EXECUTE sp_updateextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Site', N'COLUMN', N'FriendLink'
GO
-- 2011-05-24 汪奇志 同行平台信息表增加字段
ALTER TABLE dbo.tbl_Site ADD
	MainRoute nvarchar(MAX) NULL,
	CorporateCulture nvarchar(MAX) NULL
GO
DECLARE @v sql_variant 
SET @v = N'主营线路'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Site', N'COLUMN', N'MainRoute'
GO
DECLARE @v sql_variant 
SET @v = N'企业文化'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Site', N'COLUMN', N'CorporateCulture'
GO
--2011-05-26 汪奇志 客户表增加结算日期字段
GO
ALTER TABLE dbo.tbl_Customer ADD
	AccountDay tinyint NOT NULL CONSTRAINT DF_tbl_Customer_AccountDay DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'结算日期，用于控制收款提醒，在每月X号前不提醒，X号开始提醒，X=0时该客户有欠款就提醒'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Customer', N'COLUMN', N'AccountDay'
GO

-- =============================================
-- Author:		<李焕超,>
-- Create date: <Create 2011-1-21,,>
-- Description:	<添加客户,,>
-- History:
-- 1.2011-05-26 汪奇志 结算日期
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
	,@Phone nvarchar(20)---电话
	,@Mobile nvarchar(20)---手机
	,@Fax nvarchar(20)--传真
	,@Remark nvarchar(500)---备注
	,@OperatorId int---操作员编号
	,@CustomerInfoXML NVARCHAR(MAX) ---联系人列表
	,@Result int output --输出参数 9 ，插入不分配账号的联系人 2，插入用户 8 ,未执行xml	 
	,@AccountDay TINYINT--结算日期
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
		,[IsDelete],[BrandId],[AccountDay])
	VALUES(@ProviceId,@ProvinceName,@CityId,@CityName,@Name
		,@Licence,@Adress,@PostalCode,@BankAccount,@MaxDebts
		,@PreDeposit,@CommissionCount,@Saler,@SaleId,@CustomerLev
		,@CommissionType,@CompanyId,1,@ContactName,@Phone
		,@Mobile,@Fax,@Remark,@OperatorId,getdate()
		,0,@BrandId,@AccountDay)
	SELECT  @CustomerId =@@IDENTITY 

	if @CustomerInfoXML is null or len(@CustomerInfoXML)<10
	begin
		set @Result=1 
		return @Result
	end

-----开始 事务
 BEGIN TRANSACTION addCustomer

 EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@CustomerInfoXML
 declare @cursorCount int
 set @cursorCount=0  
        
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
		set @cursorCount = @cursorCount+1
		if @cursorCount >20
		begin
			set @Result =0
			return @Result
		end
		if  @cContactId=0 ---新增联系人
		begin
			if @cusername is not null and @cusername <>''---添加用户
			begin
				declare @IsUserExist int
				set @IsUserExist=0---判断用户是否存在
				select @IsUserExist=count(id) from tbl_CompanyUser where UserName=@cusername and CompanyId=@CompanyId
				if @IsUserExist<=0---用户不存在可以添加
				begin
					INSERT INTO  tbl_CompanyUser 
					(UserName,[Password],MD5Password,roleid,SuperviseDepartId,TourCompanyId,CompanyId,SuperviseDepartName,contactname
					,contactsex,contactmobile,contactemail,qq,jobname,departname,usertype,ContactTel,ContactFax)
					values(@cusername,@cPwd,@cPwdMd5,0,0,@CustomerId,@CompanyId,'',@cContactName,@cSex,@cMobile,@cEmail,@cqq,@cJob
					,@cDepartment,1,@cTel,@cFax)
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
	,@Phone nvarchar(20)---电话
	,@Mobile nvarchar(20)---手机
	,@Fax nvarchar(20)--传真
	,@Remark nvarchar(500)---备注
	,@OperatorId int---操作员编号
	,@IsEnable char(1)
	,@IssueTime datetime
	,@IsDelete char(1)
	,@BrandId int --专线给此客户分配的品牌编号
	,@CustomerInfoXML NVARCHAR(MAX) ---联系人列表<root><AreaIds=  username= Pwd= Sex= ContactName= Mobile= qq= BirthDay= Email= Job= Department= ContactId= Md5= /><root>
	,@Result int output --输出参数 0失败，1成功，9用户已存在
	,@AccountDay TINYINT --结算日期	 
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
		,[AccountDay]=@AccountDay
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
 declare @cursorCount int
 set @cursorCount=0  
        
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
 set @cursorCount = @cursorCount+1
 if @cursorCount >20
 begin
 set @Result =1
 return @Result
 end
    if  @cContactId=0 ---新增联系人
    begin
      if @cusername is not null and @cusername <>''---添加用户
        begin
          set @IsUserExist=0---判断用户是否存在
          select @IsUserExist=count(id) from tbl_CompanyUser where UserName=@cusername and CompanyId=@CompanyId
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
      if @cusername is not null and len(@cusername)>0 and len(@cPwd)>0 ---当填写用户名和密码是可以新增或修改用户
      begin
		  set @IsUserExist=0---判断用户是否存在
		  select  @IsUserExist=count(id) from tbl_CompanyUser where UserName=@cusername and CompanyId=@CompanyId
		  if @IsUserExist<=0---用户不存在可以添加
		   begin
               INSERT INTO  tbl_CompanyUser 
               (UserName,Password,MD5Password,roleid,SuperviseDepartId,TourCompanyId,CompanyId,SuperviseDepartName,contactname,contactsex,contactmobile,contactemail,qq,jobname,departname,usertype,ContactTel,ContactFax)
               values(@cusername,@cPwd,@cPwdMd5,0,0,@CustomerId,@CompanyId,'',@cContactName,@cSex,@cMobile,@cEmail,@cqq,@cJob,@cDepartment,1,@cTel,@cFax)
              select @newUserId=@@identity
            end            
		  else---修改用户
		  begin
			update tbl_CompanyUser set Password=@cPwd,MD5Password=@cPwdMd5,ContactName=@cContactName,
contactsex=@cSex,contactmobile=@cMobile,contactemail=@cEmail,qq=@cqq,jobname=@cJob,departname=@cDepartment,ContactTel=@cTel,ContactFax = @cFax where username=@cusername and usertype=1 and CompanyId=@CompanyId---修改已存在用户密码
		  end
     end--判断是否填写用户名密码结束
      
      select top 1 @newUserId=id from tbl_CompanyUser where username=@cusername
      if @newUserId is null 
      set @newUserId =0
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
-- Author:		周文超
-- Create date: 2010-01-24
-- Description:	收款提醒视图
-- =============================================
ALTER VIEW [dbo].[View_ReceiptRemind_GetList]
AS
select 
Id,[Name],ContactName,Phone,CompanyId
,(select isnull(sum(FinanceSum - HasCheckMoney),0) from tbl_TourOrder as tto where tto.BuyCompanyID = tc.Id and tto.SellCompanyId = tc.CompanyId and tto.IsDelete = '0' and tto.OrderState not in (3,4)) as ArrearCash
,(select SalerName from tbl_TourOrder where tbl_TourOrder.BuyCompanyID = tc.Id and tbl_TourOrder.SellCompanyId = tc.CompanyId and tbl_TourOrder.IsDelete = '0' and tbl_TourOrder.OrderState not in (3,4) for xml auto,root('root')) as SalerInfo
,AccountDay
from tbl_Customer as tc
where tc.IsDelete = '0'
GO

-- =============================================
-- Author:		luofx
-- Create date: 2011-01-28
-- Description:	订单自动留位过期计划
-- History:
-- 1.汪奇志 2011-03-25 调整更新订单状态与设置团队虚拟实收人数的执行顺序
-- 2.汪奇志 2011-05-30 增加对线路库收客数的维护
-- =============================================
ALTER PROCEDURE  [dbo].[SQLPlan_TourOder_SaveSeatUpdate]
	  
AS
BEGIN
	CREATE TABLE #TmpSQLPlan_TourOder_SaveSeatUpdate(
		TourId CHAR(36),
		OrderId CHAR(36),
		RouteId INT
	)
	INSERT INTO #TmpSQLPlan_TourOder_SaveSeatUpdate(TourId,OrderId,RouteId) 
	SELECT TourId,Id,RouteId FROM [tbl_TourOrder] WHERE OrderState=2  AND IsDelete=0 AND DATEDIFF(ms,SaveSeatDate,getdate())>0
	--修改留位已过时订单的状态为留位过期
	UPDATE [tbl_TourOrder] SET OrderState=3 
	WHERE ID IN(SELECT OrderId FROM #TmpSQLPlan_TourOder_SaveSeatUpdate)
	--维护团队虚拟实收数=订单总人数(不含留住过期、取消的订单)
	UPDATE tbl_Tour SET VirtualPeopleNumber=(SELECT ISNULL(SUM(PeopleNumber-LeaguePepoleNum),0) FROM tbl_TourOrder WHERE TourId=tbl_Tour.TourId  AND IsDelete='0' AND OrderState IN(1,2,5)) 
	WHERE TourId IN(SELECT TourId FROM #TmpSQLPlan_TourOder_SaveSeatUpdate)	
	--留位过期时，更新统计分析所有收入明细表   
	UPDATE tbl_StatAllIncome SET IsDelete=1 WHERE ItemId=(SELECT OrderId FROM #TmpSQLPlan_TourOder_SaveSeatUpdate) AND ItemType=1
	--团队状态维护 仅客满状态到正在收客状态
	UPDATE [tbl_Tour] SET [Status]=0 FROM [tbl_Tour] AS A 
	WHERE A.[Status]=1 AND A.TourId IN(SELECT TourId FROM #TmpSQLPlan_TourOder_SaveSeatUpdate) AND A.PlanPeopleNumber>(SELECT ISNULL(SUM(B.PeopleNumber-B.LeaguePepoleNum),0) FROM tbl_TourOrder AS B WHERE B.TourId=A.TourId AND B.[OrderState] NOT IN(3,4) AND IsDelete='0')
	--线路库收客数维护
	UPDATE [tbl_Route] SET VisitorCount=ISNULL((SELECT SUM(AdultNumber+ChildNumber) FROM [tbl_TourOrder] WHERE [OrderState] IN(1,2,5) AND RouteId=A.Id AND IsDelete='0'),0)
	FROM [tbl_Route] AS A WHERE A.Id IN(SELECT RouteId FROM #TmpSQLPlan_TourOder_SaveSeatUpdate WHERE RouteId>0)
	DROP TABLE #TmpSQLPlan_TourOder_SaveSeatUpdate
END
GO

-- =============================================
-- Author: luofx
-- Create date:2011-01-22
-- Description:	修改团队收客数，上团数
-- History:
-- 1.汪奇志 2011-05-30 修改收客数的统计条件
-- =============================================
ALTER PROCEDURE [dbo].[proc_UpdateTourCountOrVisitorCount] 
	@RouteId NVARCHAR(MAX), --线路编号:多个编号以半角逗号分开
	@UpdateTourType INT,  --要修改的类型：0:修改上团数；1：修改收客数；2：修改上团数和收客数
	@Result INT OUTPUT         --回传参数
AS
DECLARE @TourCount INT	  --上团数
DECLARE @RId INT      --单个线路Id值
DECLARE @VistorCount INT  --收客数
DECLARE @MaxId INT --临时id变量最大值
DECLARE @i INT     
SET @TourCount=0
SET @VistorCount=0
SET @i=1
SET @Result=1
BEGIN
	IF(len(@RouteId)>0)
		BEGIN
			SELECT @MaxId=MAX(Id) FROM dbo.fn_split(@RouteId,',')
			WHILE(@i<=@MaxId)
				BEGIN
					SELECT @RId=[VALUE] FROM dbo.fn_split(@RouteId,',') WHERE ID=@i
					IF(@UpdateTourType=0) --修改上团数				
						BEGIN
							SELECT @TourCount=ISNULL(COUNT(1),0) FROM [tbl_Tour] a WHERE a.[RouteId]=@RId AND TemplateId<>''
							UPDATE [tbl_Route] SET TourCount=@TourCount WHERE ID=@RId
						END	
					ELSE IF(@UpdateTourType=1)--修改收客数
						BEGIN							
							SELECT @VistorCount=ISNULL(SUM(AdultNumber+ChildNumber),0) FROM [tbl_TourOrder] WHERE [RouteId]=@RId AND [IsDelete]='0' AND [OrderState] IN(1,2,5)
							UPDATE [tbl_Route] SET VisitorCount=@VistorCount WHERE ID=@RId							
						END	
					ELSE IF(@UpdateTourType=2)--修改上团数和收客数
						BEGIN
							SELECT @TourCount=ISNULL(COUNT(1),0) FROM [tbl_Tour] a WHERE [RouteId]=@RId AND TemplateId<>''
							SELECT @VistorCount=ISNULL(SUM(AdultNumber+ChildNumber),0) FROM [tbl_TourOrder] WHERE [RouteId]=@RId AND [IsDelete]='0' AND [OrderState] IN(1,2,5)
							UPDATE [tbl_Route] SET TourCount=@TourCount,VisitorCount=@VistorCount WHERE ID=@RId
						END
					SET @i=@i+1
				END			
			IF(@@ERROR>0)
				BEGIN
					SET @Result=0
				END
		END
RETURN @Result
END
GO

-- =============================================
-- Author:		周文超
-- Create date: 2011-01-26
-- Description:	重新计算所有模块的已确认支付金额
-- Error：0参数错误
-- Error：-1计算过程中发生错误
-- Success：1重新计算成功
-- History:
-- 1.2011-03-02 汪奇志 增加对支出对账单表已支付金额的维护
-- =============================================
ALTER PROCEDURE [dbo].[proc_Utility_CalculationCheckedOut]
	@FinancialPayId nvarchar(max) ,         --支出登记明细Id集合
	@ErrorValue  int = 0 output   --错误代码
AS
BEGIN
	if @FinancialPayId is null or len(@FinancialPayId) < 1
	begin
		set @ErrorValue = 0
		return
	end
	
	declare @ErrorCount int    --错误计数器
	declare @ItemId char(36)    --项目编号
	declare @ItemType tinyint    --项目类型
	declare @ReceiveId char(36)  --计调项目编号
	declare @ReceiveType tinyint  --计调项目类型
	declare @CheckedPaymentAmount money  --已确认支付金额
	declare @TotalExpenses money  --团队计调总支出
	declare @TmpSql   nvarchar(4000)   --临时Sql语句
	set @ErrorCount = 0
	set @CheckedPaymentAmount = 0
	set @TotalExpenses = 0

	begin tran CalculationCheckedOut

	set @TmpSql = 'DECLARE cur_CalculationCheckedOut CURSOR FOR
		SELECT ItemId,ItemType,ReceiveId,ReceiveType from tbl_FinancialPayInfo where Id in (' + @FinancialPayId + ')'
	exec sp_executesql @TmpSql
	--验证错误
	SET @ErrorCount = @ErrorCount + @@ERROR
	 
	open cur_CalculationCheckedOut
	while 1 = 1
	begin
		fetch next from cur_CalculationCheckedOut into @ItemId,@ItemType,@ReceiveId,@ReceiveType
	    if @@fetch_status<>0
		    break
		if @ReceiveId is not null and @ReceiveType is not null and len(@ReceiveId) = 36
		begin
			select @CheckedPaymentAmount = isnull(sum(PaymentAmount),0) from tbl_FinancialPayInfo where ReceiveId = @ReceiveId and ReceiveType = @ReceiveType and IsChecked = '1' and IsPay = '1'
			if @ReceiveType = 0  --单项服务供应商安排
			begin
				--更新单项服务供应商安排已确认支付金额
				update tbl_PlanSingle set PaidAmount = @CheckedPaymentAmount where PlanId = @ReceiveId
				--验证错误
				SET @ErrorCount = @ErrorCount + @@ERROR
				--更新支出对账单表已支付金额
				UPDATE [tbl_StatAllOut] SET [HasCheckAmount]=@CheckedPaymentAmount WHERE [ItemType]=4 AND [ItemId]=@ReceiveId
				SET @ErrorCount=@ErrorCount + @@ERROR
			end
			else if @ReceiveType = 1  --地接
			begin
				--更新地接已确认支付金额
				update tbl_PlanLocalAgency set PayAmount = @CheckedPaymentAmount where ID = @ReceiveId
				--验证错误
				SET @ErrorCount = @ErrorCount + @@ERROR
				--更新支出对账单表已支付金额
				UPDATE [tbl_StatAllOut] SET [HasCheckAmount]=@CheckedPaymentAmount WHERE [ItemType]=1 AND [ItemId]=@ReceiveId
				SET @ErrorCount=@ErrorCount + @@ERROR
			end
			else if @ReceiveType = 2  --票务
			begin
				--更新票务已确认支付金额
				update tbl_PlanTicketOut set PayAmount = @CheckedPaymentAmount where ID = @ReceiveId
				--验证错误
				SET @ErrorCount = @ErrorCount + @@ERROR
				--更新支出对账单表已支付金额
				UPDATE [tbl_StatAllOut] SET [HasCheckAmount]=@CheckedPaymentAmount WHERE [ItemType]=2 AND [ItemId]=@ReceiveId
				SET @ErrorCount=@ErrorCount + @@ERROR
			end
		end
		
		--判断团队支出是否已结清
		if @ItemId is not null and @ItemType is not null and len(@ItemId) = 36
		begin
			if @ItemType = 0  --团队支出
			begin
				select @TotalExpenses = TotalExpenses from tbl_Tour where TourId = @ItemId
				select @CheckedPaymentAmount = sum(PaymentAmount) from tbl_FinancialPayInfo where ItemId = @ItemId and ItemType = @ItemType and IsChecked = '1' and IsPay = '1'
				if @TotalExpenses = @CheckedPaymentAmount
				begin
					--更新团队支出是否已结清
					update tbl_Tour set IsSettleOut = '1' where TourId = @ItemId
					--验证错误
					SET @ErrorCount = @ErrorCount + @@ERROR
				end
				ELSE
				BEGIN
					--更新团队支出是否已结清
					update tbl_Tour set IsSettleOut = '0' where TourId = @ItemId
					--验证错误
					SET @ErrorCount = @ErrorCount + @@ERROR
				END
			end
		end
	end
	close cur_CalculationCheckedOut
    deallocate cur_CalculationCheckedOut

	IF @ErrorCount > 0 
	BEGIN
		SET @ErrorValue = -1;
		ROLLBACK TRAN CalculationCheckedOut
	end	
	ELSE
	BEGIN
		SET @ErrorValue = 1;
		COMMIT TRAN CalculationCheckedOut
	end
END
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-05-31
-- Description:	维护计划款项结清状态
-- =============================================
CREATE PROCEDURE proc_Utility_CalculationTourSettleStatus
	@TourId CHAR(36),--计划编号
	@Result INT OUTPUT--维护结果（1成功，其它失败）
AS
BEGIN
	--收入是否已结清
	IF(EXISTS(SELECT 1 FROM [tbl_TourOrder] WHERE [TourId]=@TourId AND [IsDelete]='0'))--有收入
	BEGIN
		IF(EXISTS(SELECT 1 FROM [tbl_TourOrder] WHERE [TourId]=@TourId AND [IsDelete]='0' AND [FinanceSum]<>[HasCheckMoney]))--收入金额<>已收已审核金额
		BEGIN
			UPDATE tbl_Tour SET [IsSettleInCome]='0' WHERE [TourId]=@TourId
		END
		ELSE
		BEGIN
			UPDATE tbl_Tour SET [IsSettleInCome]='1' WHERE [TourId]=@TourId
		END
	END
	ELSE
	BEGIN
		UPDATE tbl_Tour SET [IsSettleInCome]='0' WHERE [TourId]=@TourId
	END

	--支出是否已结清
	IF(EXISTS(SELECT 1 FROM [tbl_PlanLocalAgency] WHERE [TourId]=@TourId) OR EXISTS(SELECT 1 FROM [tbl_PlanTicketOut] WHERE [TourId]=@TourId AND [State]=3) OR EXISTS(SELECT 1 FROM [tbl_PlanSingle] WHERE [TourId]=@TourId))--有支出
	BEGIN
		IF(EXISTS(SELECT 1 FROM [tbl_PlanLocalAgency] WHERE [TourId]=@TourId AND TotalAmount<>PayAmount) OR EXISTS(SELECT 1 FROM [tbl_PlanTicketOut] WHERE [TourId]=@TourId AND TotalAmount<>PayAmount AND [State]=3) OR EXISTS(SELECT 1 FROM [tbl_PlanSingle] WHERE [TourId]=@TourId AND TotalAmount<>PaidAmount))
		BEGIN
			UPDATE tbl_Tour SET [IsSettleOut]='0' WHERE [TourId]=@TourId
		END
		ELSE
		BEGIN
			UPDATE tbl_Tour SET [IsSettleOut]='1' WHERE [TourId]=@TourId
		END
	END	
	ELSE
	BEGIN
		UPDATE tbl_Tour SET [IsSettleOut]='0' WHERE [TourId]=@TourId
	END
	
	SET @Result=1
	RETURN 1
END
GO

-- =============================================
-- Author:		<李焕超,>
-- Create date: <Create 2011-1-21,,>
-- Description:	<添加客户,,>
-- History:
-- 1.2011-05-26 汪奇志 结算日期
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
	,@Phone nvarchar(20)---电话
	,@Mobile nvarchar(20)---手机
	,@Fax nvarchar(20)--传真
	,@Remark nvarchar(500)---备注
	,@OperatorId int---操作员编号
	,@CustomerInfoXML NVARCHAR(MAX) ---联系人列表
	,@Result int output --输出参数 9 ，插入不分配账号的联系人 2，插入用户 8 ,未执行xml	 
	,@AccountDay TINYINT--结算日期,
	,@IsEnable CHAR(1)--是否启用
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
		,[IsDelete],[BrandId],[AccountDay])
	VALUES(@ProviceId,@ProvinceName,@CityId,@CityName,@Name
		,@Licence,@Adress,@PostalCode,@BankAccount,@MaxDebts
		,@PreDeposit,@CommissionCount,@Saler,@SaleId,@CustomerLev
		,@CommissionType,@CompanyId,@IsEnable,@ContactName,@Phone
		,@Mobile,@Fax,@Remark,@OperatorId,getdate()
		,0,@BrandId,@AccountDay)
	SELECT  @CustomerId =@@IDENTITY 

	if @CustomerInfoXML is null or len(@CustomerInfoXML)<10
	begin
		set @Result=1 
		return @Result
	end

-----开始 事务
 BEGIN TRANSACTION addCustomer

 EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@CustomerInfoXML
 declare @cursorCount int
 set @cursorCount=0  
        
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
		set @cursorCount = @cursorCount+1
		if @cursorCount >20
		begin
			set @Result =0
			return @Result
		end
		if  @cContactId=0 ---新增联系人
		begin
			if @cusername is not null and @cusername <>''---添加用户
			begin
				declare @IsUserExist int
				set @IsUserExist=0---判断用户是否存在
				select @IsUserExist=count(id) from tbl_CompanyUser where UserName=@cusername and CompanyId=@CompanyId
				if @IsUserExist<=0---用户不存在可以添加
				begin
					INSERT INTO  tbl_CompanyUser 
					(UserName,[Password],MD5Password,roleid,SuperviseDepartId,TourCompanyId,CompanyId,SuperviseDepartName,contactname
					,contactsex,contactmobile,contactemail,qq,jobname,departname,usertype,ContactTel,ContactFax)
					values(@cusername,@cPwd,@cPwdMd5,0,0,@CustomerId,@CompanyId,'',@cContactName,@cSex,@cMobile,@cEmail,@cqq,@cJob
					,@cDepartment,1,@cTel,@cFax)
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

--(20110523)专线版管理系统.mmap end


--(20110607)bbl.txt start

--客户关系增加结算方式列 汪奇志 2011-06-07
GO
ALTER TABLE dbo.tbl_Customer ADD
	AccountWay nvarchar(255) NULL
GO
DECLARE @v sql_variant 
SET @v = N'结算方式'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Customer', N'COLUMN', N'AccountWay'
GO

--客户关系增加结算日类型列 汪奇志 2011-06-07
ALTER TABLE dbo.tbl_Customer ADD
	AccountDayType tinyint NOT NULL CONSTRAINT DF_tbl_Customer_AccountDayType DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'结算日类型 详见枚举'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Customer', N'COLUMN', N'AccountDayType'
GO

-- =============================================
-- Author:		<李焕超,>
-- Create date: <Create 2011-1-21,,>
-- Description:	<添加客户,,>
-- History:
-- 1.2011-05-26 汪奇志 结算日期
-- 2.2011-06-07 汪奇志 结算方式 结算日类型
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
	,@Phone nvarchar(20)---电话
	,@Mobile nvarchar(20)---手机
	,@Fax nvarchar(20)--传真
	,@Remark nvarchar(500)---备注
	,@OperatorId int---操作员编号
	,@CustomerInfoXML NVARCHAR(MAX) ---联系人列表
	,@Result int output --输出参数 9 ，插入不分配账号的联系人 2，插入用户 8 ,未执行xml	 
	,@AccountDay TINYINT--结算日期,
	,@IsEnable CHAR(1)--是否启用,
	,@AccountWay NVARCHAR(255)--结算方式
	,@AccountDayType TINYINT--结算日类型
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
		,[IsDelete],[BrandId],[AccountDay],[AccountWay],[AccountDayType])
	VALUES(@ProviceId,@ProvinceName,@CityId,@CityName,@Name
		,@Licence,@Adress,@PostalCode,@BankAccount,@MaxDebts
		,@PreDeposit,@CommissionCount,@Saler,@SaleId,@CustomerLev
		,@CommissionType,@CompanyId,@IsEnable,@ContactName,@Phone
		,@Mobile,@Fax,@Remark,@OperatorId,getdate()
		,0,@BrandId,@AccountDay,@AccountWay,@AccountDayType)
	SELECT  @CustomerId =@@IDENTITY 

	if @CustomerInfoXML is null or len(@CustomerInfoXML)<10
	begin
		set @Result=1 
		return @Result
	end

-----开始 事务
 BEGIN TRANSACTION addCustomer

 EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@CustomerInfoXML
 declare @cursorCount int
 set @cursorCount=0  
        
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
		set @cursorCount = @cursorCount+1
		if @cursorCount >20
		begin
			set @Result =0
			return @Result
		end
		if  @cContactId=0 ---新增联系人
		begin
			if @cusername is not null and @cusername <>''---添加用户
			begin
				declare @IsUserExist int
				set @IsUserExist=0---判断用户是否存在
				select @IsUserExist=count(id) from tbl_CompanyUser where UserName=@cusername and CompanyId=@CompanyId
				if @IsUserExist<=0---用户不存在可以添加
				begin
					INSERT INTO  tbl_CompanyUser 
					(UserName,[Password],MD5Password,roleid,SuperviseDepartId,TourCompanyId,CompanyId,SuperviseDepartName,contactname
					,contactsex,contactmobile,contactemail,qq,jobname,departname,usertype,ContactTel,ContactFax)
					values(@cusername,@cPwd,@cPwdMd5,0,0,@CustomerId,@CompanyId,'',@cContactName,@cSex,@cMobile,@cEmail,@cqq,@cJob
					,@cDepartment,1,@cTel,@cFax)
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
	,@Phone nvarchar(20)---电话
	,@Mobile nvarchar(20)---手机
	,@Fax nvarchar(20)--传真
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
		,[AccountDay]=@AccountDay,[AccountWay]=@AccountWay,[AccountDayType]=@AccountDayType
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
 declare @cursorCount int
 set @cursorCount=0  
        
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
 set @cursorCount = @cursorCount+1
 if @cursorCount >20
 begin
 set @Result =1
 return @Result
 end
    if  @cContactId=0 ---新增联系人
    begin
      if @cusername is not null and @cusername <>''---添加用户
        begin
          set @IsUserExist=0---判断用户是否存在
          select @IsUserExist=count(id) from tbl_CompanyUser where UserName=@cusername and CompanyId=@CompanyId
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
      if @cusername is not null and len(@cusername)>0 and len(@cPwd)>0 ---当填写用户名和密码是可以新增或修改用户
      begin
		  set @IsUserExist=0---判断用户是否存在
		  select  @IsUserExist=count(id) from tbl_CompanyUser where UserName=@cusername and CompanyId=@CompanyId
		  if @IsUserExist<=0---用户不存在可以添加
		   begin
               INSERT INTO  tbl_CompanyUser 
               (UserName,Password,MD5Password,roleid,SuperviseDepartId,TourCompanyId,CompanyId,SuperviseDepartName,contactname,contactsex,contactmobile,contactemail,qq,jobname,departname,usertype,ContactTel,ContactFax)
               values(@cusername,@cPwd,@cPwdMd5,0,0,@CustomerId,@CompanyId,'',@cContactName,@cSex,@cMobile,@cEmail,@cqq,@cJob,@cDepartment,1,@cTel,@cFax)
              select @newUserId=@@identity
            end            
		  else---修改用户
		  begin
			update tbl_CompanyUser set Password=@cPwd,MD5Password=@cPwdMd5,ContactName=@cContactName,
contactsex=@cSex,contactmobile=@cMobile,contactemail=@cEmail,qq=@cqq,jobname=@cJob,departname=@cDepartment,ContactTel=@cTel,ContactFax = @cFax where username=@cusername and usertype=1 and CompanyId=@CompanyId---修改已存在用户密码
		  end
     end--判断是否填写用户名密码结束
      
      select top 1 @newUserId=id from tbl_CompanyUser where username=@cusername
      if @newUserId is null 
      set @newUserId =0
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

--订单表增加列 汪奇志 2011-06-07
ALTER TABLE dbo.tbl_TourOrder ADD
	BuyerContactId int NOT NULL CONSTRAINT DF_tbl_TourOrder_BuyerContactId DEFAULT 0,
	BuyerContactName nvarchar(50) NULL,
	CommissionType tinyint NOT NULL CONSTRAINT DF_tbl_TourOrder_CommissionType DEFAULT 1,
	CommissionPrice money NOT NULL CONSTRAINT DF_tbl_TourOrder_CommissionPrice DEFAULT 0,
	CommissionStatus char(1) NOT NULL CONSTRAINT DF_tbl_TourOrder_CommissionStatus DEFAULT 0,
	CommissionPayUId int NOT NULL CONSTRAINT DF_tbl_TourOrder_CommissionPayUId DEFAULT 0,
	CommissionPayTime datetime NULL
GO
DECLARE @v sql_variant 
SET @v = N'组团联系人编号'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOrder', N'COLUMN', N'BuyerContactId'
GO
DECLARE @v sql_variant 
SET @v = N'组团联系人姓名'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOrder', N'COLUMN', N'BuyerContactName'
GO
DECLARE @v sql_variant 
SET @v = N'返佣类型'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOrder', N'COLUMN', N'CommissionType'
GO
DECLARE @v sql_variant 
SET @v = N'返佣金额(单价)'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOrder', N'COLUMN', N'CommissionPrice'
GO
DECLARE @v sql_variant 
SET @v = N'返佣支付状态'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOrder', N'COLUMN', N'CommissionStatus'
GO
DECLARE @v sql_variant 
SET @v = N'返佣支付人编号'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOrder', N'COLUMN', N'CommissionPayUId'
GO
DECLARE @v sql_variant 
SET @v = N'返佣支付时间'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOrder', N'COLUMN', N'CommissionPayTime'
GO

-- =============================================
-- Author:		luofx
-- Create date: 2011-01-24
-- Description:	新增订单
-- History:
-- 1.增加组团联系人信息、返佣信息 汪奇志 2011-06-08
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
	SELECT @ShiShu=ISNULL(SUM(PeopleNumber),0) FROM tbl_TourOrder WHERE IsDelete=0 AND TourId=@TourId AND OrderState IN(1,2,5)
	IF(@IsTourOrderEdit=0 AND (@PlanPeopleNumber-@ShiShu)-@PeopleNumber<0)
	BEGIN			
		SET @Result=2
	END
	IF(@IsTourOrderEdit=1 AND (@PlanPeopleNumber-@VirtualPeopleNumber)-@PeopleNumber<0)
	BEGIN			
		SET @Result=2
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

-- =============================================
-- Author:		luofx
-- Create date: 2011-01-24
-- Description:	修改订单
-- History:
-- 1.增加组团联系人信息、返佣信息 汪奇志 2011-06-08
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
	SET @ErrorCount=0
	SET @Result=1
BEGIN	
	--判断订单人数是否大于剩余人数
    SELECT @OldPeopleNumber=ISNULL(PeopleNumber,0),@TourId=TourId FROM tbl_TourOrder WHERE ID=@OrderID AND IsDelete=0
	--未处理订单人数+已留位订单人数+已成交订单人数
	SELECT @PeopleNumberCount=ISNULL(SUM(PeopleNumber),0) FROM tbl_TourOrder WHERE id<>@OrderID AND TourId=@TourId AND (OrderState=1 OR OrderState=2 OR OrderState=5)  AND IsDelete=0	
	SELECT @VirtualPeopleNumber=ISNULL(VirtualPeopleNumber,0),@PlanPeopleNumber=ISNULL(PlanPeopleNumber,0) FROM tbl_Tour WHERE TourId=@TourId  AND IsDelete=0
	SELECT @ShiShu=ISNULL(SUM(PeopleNumber),0) FROM tbl_TourOrder WHERE IsDelete=0 AND TourId=@TourId AND (OrderState=1 OR OrderState=2 OR OrderState=5)
	IF(@IsTourOrderEdit=1 AND @PlanPeopleNumber -(@VirtualPeopleNumber-@OldPeopleNumber+@PeopleNumber)<0)
	BEGIN						
		SET @Result=2
	END

	IF(@IsTourOrderEdit=0 AND @PlanPeopleNumber-(@ShiShu-@OldPeopleNumber+@PeopleNumber)<0)
	BEGIN			
		SET @Result=2
	END
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
		SET @RealVirtualPeopleNumber=@VirtualPeopleNumber+@PeopleNumber-@OldPeopleNumber
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
			SET @OhterOutAmount=(@AdultNumber+@ChildNumber)*@CommissionPrice
			UPDATE [tbl_TourOtherCost] SET [Proceed]=@OhterOutAmount,SumCost=@OhterOutAmount+[IncreaseCost]-[ReduceCost] WHERE [ItemType]=2 AND [ItemId]=@OrderId
		END
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

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-01-21
-- Description:	写入散拼计划、团队计划、单项服务信息
-- History:
-- 1.2011-04-18 增加地接报价人数、单价及我社报价人数、单价
-- 2.2011-06-08 团队计划订单增加返佣
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
	@TourCityId INT=0--出港城市编号
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
		IF(@errorcount=0)--是否出团
		BEGIN
			UPDATE [tbl_Tour] SET IsLeave='1' WHERE TourId=@TourId AND LeaveDate<=GETDATE()
			SET @errorcount=@errorcount+@@ERROR
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

--修改杂费支出表字段属性 汪奇志 2011-06-08
DECLARE @v sql_variant 
SET @v = N'项目类型
0：默认值
1：机票退票款
2：订单返佣（后返）'
EXECUTE sp_updateextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_TourOtherCost', N'COLUMN', N'ItemType'
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-06-07
-- Description:	支付返佣金额
-- =============================================
CREATE PROCEDURE proc_OtherCost_PayCommission
	@CostId CHAR(36)--支出编号
	,@TourId CHAR(36)--计划编号
	,@OrderId CHAR(36)--订单编号
	,@CompanyId INT--公司编号（专线）
	,@BuyerCompanyId INT--客户单位编号
	,@BuyerCompanyName NVARCHAR(255)--客户单位名称
	,@Amount MONEY--支付金额
	,@PayerId INT--支付人编号
	,@PayTime DATETIME--支付时间
	,@PayType TINYINT--支付方式
	,@CostType TINYINT--支出类型
	,@CostName NVARCHAR(50)--CostName
	,@ItemType TINYINT--项目类型
	,@Result INT OUTPUT--结果 1：成功 其它失败
AS
BEGIN
	DECLARE @CommissionType TINYINT--返佣类型
	DECLARE @CommissionAmount MONEY--返佣金额
	DECLARE @errorcount INT
	
	SET @errorcount=0
	SELECT @CommissionType=[CommissionType],@CommissionAmount=[CommissionPrice]*[PeopleNumber] FROM [tbl_TourOrder] WHERE [Id]=@OrderId
	
	IF(@CommissionType<>2 OR @Amount<>@CommissionAmount)
	BEGIN
		SET @Result=-1
		RETURN
	END
	
	BEGIN TRAN
	INSERT INTO [tbl_TourOtherCost]([Id],[CompanyId],[TourId],[CostType],[CustromCName]
		,[CustromCId],[ProceedItem],[Proceed],[IncreaseCost],[ReduceCost]
		,[SumCost],[Remark],[OperatorId],[CreateTime],[PayTime]
		,[Payee],[PayType],[ItemType],[ItemId])
	VALUES(@CostId,@CompanyId,@TourId,@CostType,@BuyerCompanyName
		,@BuyerCompanyId,@CostName,@Amount,0,0
		,@Amount,'',@PayerId,GETDATE(),@PayTime
		,'',@PayType,@ItemType,@OrderId)
	SET @errorcount=@errorcount+@@ERROR

	IF(@errorcount=0)
	BEGIN
		UPDATE [tbl_TourOrder] SET [CommissionStatus]='1',[CommissionPayUId]=@PayerId,[CommissionPayTime]=GETDATE() WHERE [Id]=@OrderId
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount>0)
	BEGIN
		ROLLBACK TRAN
		SET @Result=0
		RETURN
	END

	COMMIT TRAN
	SET @Result=1
END
GO

-- =============================================
-- Author:		周文超
-- Create date: 2010-01-24
-- Description:	收款提醒视图
-- =============================================
ALTER VIEW [dbo].[View_ReceiptRemind_GetList]
AS
select 
Id,[Name],ContactName,Phone,CompanyId
,(select isnull(sum(FinanceSum - HasCheckMoney),0) from tbl_TourOrder as tto where tto.BuyCompanyID = tc.Id and tto.SellCompanyId = tc.CompanyId and tto.IsDelete = '0' and tto.OrderState not in (3,4)) as ArrearCash
,(select SalerName from tbl_TourOrder where tbl_TourOrder.BuyCompanyID = tc.Id and tbl_TourOrder.SellCompanyId = tc.CompanyId and tbl_TourOrder.IsDelete = '0' and tbl_TourOrder.OrderState not in (3,4) for xml auto,root('root')) as SalerInfo
,AccountDay,AccountDayType
from tbl_Customer as tc
where tc.IsDelete = '0'
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-06-07
-- Description:	支付返佣金额
-- =============================================
ALTER PROCEDURE [dbo].[proc_OtherCost_PayCommission]
	@CostId CHAR(36)--支出编号
	,@TourId CHAR(36)--计划编号
	,@OrderId CHAR(36)--订单编号
	,@CompanyId INT--公司编号（专线）
	,@BuyerCompanyId INT--客户单位编号
	,@BuyerCompanyName NVARCHAR(255)--客户单位名称
	,@Amount MONEY--支付金额
	,@PayerId INT--支付人编号
	,@PayTime DATETIME--支付时间
	,@PayType TINYINT--支付方式
	,@CostType TINYINT--支出类型
	,@CostName NVARCHAR(50)--CostName
	,@ItemType TINYINT--项目类型
	,@Result INT OUTPUT--结果 1：成功 其它失败
AS
BEGIN
	DECLARE @CommissionType TINYINT--返佣类型
	DECLARE @CommissionAmount MONEY--返佣金额
	DECLARE @IsPaid CHAR(1)--是否支付
	DECLARE @errorcount INT	
	
	SET @errorcount=0
	SELECT @CommissionType=[CommissionType],@CommissionAmount=[CommissionPrice]*[PeopleNumber],@IsPaid=[CommissionStatus] FROM [tbl_TourOrder] WHERE [Id]=@OrderId
	
	IF(@CommissionType<>2 OR @Amount<>@CommissionAmount)
	BEGIN
		SET @Result=-1
		RETURN
	END

	IF(@IsPaid='1')
	BEGIN
		SET @Result=2
		RETURN
	END
	
	BEGIN TRAN
	INSERT INTO [tbl_TourOtherCost]([Id],[CompanyId],[TourId],[CostType],[CustromCName]
		,[CustromCId],[ProceedItem],[Proceed],[IncreaseCost],[ReduceCost]
		,[SumCost],[Remark],[OperatorId],[CreateTime],[PayTime]
		,[Payee],[PayType],[ItemType],[ItemId])
	VALUES(@CostId,@CompanyId,@TourId,@CostType,@BuyerCompanyName
		,@BuyerCompanyId,@CostName,@Amount,0,0
		,@Amount,'',@PayerId,GETDATE(),@PayTime
		,'',@PayType,@ItemType,@OrderId)
	SET @errorcount=@errorcount+@@ERROR

	IF(@errorcount=0)
	BEGIN
		UPDATE [tbl_TourOrder] SET [CommissionStatus]='1',[CommissionPayUId]=@PayerId,[CommissionPayTime]=GETDATE() WHERE [Id]=@OrderId
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount>0)
	BEGIN
		ROLLBACK TRAN
		SET @Result=0
		RETURN
	END

	COMMIT TRAN
	SET @Result=1
END
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-06-09
-- Description:	删除订单
-- =============================================
CREATE PROCEDURE proc_TourOrder_Delete
	@OrderId CHAR(36)--订单编号
AS
BEGIN
	DECLARE @TourId CHAR(36)--计划编号
	DECLARE @ShiShou INT--实收人数
	DECLARE @PlanPeopleNumber INT--计划人数
	DECLARE @TourStatus TINYINT--计划状态

	SELECT @TourId=[TourId] FROM [tbl_TourOrder] WHERE [ID]=@OrderId
	--标记订单删除
	UPDATE [tbl_TourOrder] SET [IsDelete]='1' WHERE [ID]=@OrderId
	SELECT @ShiShou=ISNULL(SUM(PeopleNumber-LeaguePepoleNum),0) FROM [tbl_TourOrder] WHERE [TourId]=@TourId AND [IsDelete]='0' AND [OrderState] NOT IN(3,4)
	--计划实收人数
	UPDATE [tbl_Tour] SET [VirtualPeopleNumber]=@ShiShou WHERE TourId=@TourId
	SELECT @PlanPeopleNumber=[PlanPeopleNumber],@TourStatus=[Status] FROM [tbl_Tour] WHERE [TourId]=@TourId

	--标记计划状态为客满 
	IF(@TourStatus=0 AND @PlanPeopleNumber=@ShiShou)
	BEGIN
		UPDATE [tbl_Tour] SET [Status]=1 WHERE [TourId]=@TourId
	END

	--标记计划状态为正常
	IF(@TourStatus=1 AND @PlanPeopleNumber>@ShiShou)
	BEGIN
		UPDATE [tbl_Tour] SET [Status]=0 WHERE [TourId]=@TourId
	END	
END
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-01-23
-- Description:	更新散拼计划、团队计划、单项服务信息
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
	@TourCityId INT=0--出港城市编号
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
	SELECT @CompanyName=[CompanyName] FROM [tbl_CompanyInfo] WHERE Id=@CompanyId
	SELECT @OperatorName=[ContactName],@OperatorTelephone=[ContactTel],@OperatorMobile=[ContactMobile],@OperatorFax=[ContactFax] FROM [tbl_COmpanyUser] WHERE Id=@OperatorId
	SELECT @DepartmentId=[DepartId] FROM [tbl_CompanyUser] WHERE Id=@OperatorId	

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
			UPDATE [tbl_Tour] SET [Status]=2 WHERE [TourId]=@TourId AND [Status] =3 AND DATEDIFF(DAY,GETDATE(),[RDate])>0
			SET @errorcount=@errorcount+@@ERROR
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
		IF(@errorcount=0)--是否出团
		BEGIN
			UPDATE [tbl_Tour] SET IsLeave='1' WHERE TourId=@TourId AND LeaveDate<=GETDATE()
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0)--团队状态维护
		BEGIN
			UPDATE [tbl_Tour] SET [Status]=2 WHERE [TourId]=@TourId AND [Status] =3 AND DATEDIFF(DAY,GETDATE(),[RDate])>0
			SET @errorcount=@errorcount+@@ERROR
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
-- Create date: 2011-01-28
-- Description:	计划信息维护SQL计划
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
END
GO


-- =============================================
-- Author:		鲁功源
-- Create date: 2011-02-16
-- Description:	验证当前登录用户是否有权限查看工作计划[发布人的同部门的主管或者上级部门的主管才能查看]
-- History:
-- 2011-06-10 更改递归表达式
-- =============================================
ALTER FUNCTION [dbo].[fn_ValidUserLevDepartManagers]
(	
	@CurrUserId int, --当前登录人编号
	@OperatorId int --工作计划操作人编号
)
RETURNS INT
AS
begin
	DECLARE @Result int --回传参数
	DECLARE @OperatorDepartID int--工作计划操作人所在部门
	SET @Result=0
	SET @OperatorDepartID=0
	
	SELECT @OperatorDepartID=DepartId from tbl_CompanyUser where ID=@OperatorId

	if @OperatorDepartID is null or @OperatorDepartID < 0	
	BEGIN
		RETURN @Result
	END

	ELSE
	BEGIN
		WITH SubsCTE
		AS
		(
		  -- Anchor member returns root node
		  SELECT PrevDepartId,DepartManger, 0 AS lvl ,id
		  FROM dbo.tbl_CompanyDepartment
		  WHERE id = @OperatorDepartID

		  UNION ALL

		  -- Recursive member returns next level of children
		  SELECT C.PrevDepartId, C.DepartManger, P.lvl + 1,c.id
		  FROM SubsCTE AS P
			JOIN dbo.tbl_CompanyDepartment AS C
			  ON C.id = P.PrevDepartId
		)
		select @Result=count(*) from SubsCTE where DepartManger=@CurrUserId
	END
	RETURN @Result	
END
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-06-07
-- Description:	支付返佣金额
-- =============================================
ALTER PROCEDURE [dbo].[proc_OtherCost_PayCommission]
	@CostId CHAR(36)--支出编号
	,@TourId CHAR(36)--计划编号
	,@OrderId CHAR(36)--订单编号
	,@CompanyId INT--公司编号（专线）
	,@BuyerCompanyId INT--客户单位编号
	,@BuyerCompanyName NVARCHAR(255)--客户单位名称
	,@Amount MONEY--支付金额
	,@PayerId INT--支付人编号
	,@PayTime DATETIME--支付时间
	,@PayType TINYINT--支付方式
	,@CostType TINYINT--支出类型
	,@CostName NVARCHAR(50)--CostName
	,@ItemType TINYINT--项目类型
	,@Result INT OUTPUT--结果 1：成功 其它失败
AS
BEGIN
	DECLARE @CommissionType TINYINT--返佣类型
	DECLARE @CommissionAmount MONEY--返佣金额
	DECLARE @IsPaid CHAR(1)--是否支付
	DECLARE @errorcount INT	
	
	SET @errorcount=0
	SELECT @CommissionType=[CommissionType],@CommissionAmount=[CommissionPrice]*[PeopleNumber],@IsPaid=[CommissionStatus] FROM [tbl_TourOrder] WHERE [Id]=@OrderId
	
	IF(@CommissionType<>2 OR @Amount<>@CommissionAmount)
	BEGIN
		SET @Result=-1
		RETURN
	END

	IF(@IsPaid='1')
	BEGIN
		SET @Result=2
		RETURN
	END
	
	BEGIN TRAN
	--写杂费支出表
	INSERT INTO [tbl_TourOtherCost]([Id],[CompanyId],[TourId],[CostType],[CustromCName]
		,[CustromCId],[ProceedItem],[Proceed],[IncreaseCost],[ReduceCost]
		,[SumCost],[Remark],[OperatorId],[CreateTime],[PayTime]
		,[Payee],[PayType],[ItemType],[ItemId])
	VALUES(@CostId,@CompanyId,@TourId,@CostType,@BuyerCompanyName
		,@BuyerCompanyId,@CostName,@Amount,0,0
		,@Amount,'',@PayerId,GETDATE(),@PayTime
		,'',@PayType,@ItemType,@OrderId)
	SET @errorcount=@errorcount+@@ERROR

	IF(@errorcount=0)--写支出明细表
	BEGIN
		DECLARE @AreaId INT--线路区域编号
		DECLARE @TourType TINYINT--计划类型
		DECLARE @DepartmentId INT--支付人部门编号
		SELECT @AreaId=[AreaId],@TourType=[TourType] FROM [tbl_Tour] WHERE TourId=@TourId
		SELECT @DepartmentId=[DepartId] FROM [tbl_CompanyUser] WHERE [Id]=@PayerId

		INSERT INTO [tbl_StatAllOut]([CompanyId],[TourId],[AreaId],[TourType],[ItemId]
			,[ItemType],[Amount],[AddAmount],[ReduceAmount],[TotalAmount]
			,[OperatorId],[DepartmentId],[CreateTime],[HasCheckAmount],[NotCheckAmount]
			,[IsDelete],[SupplierId],[SupplierName])
		VALUES(@CompanyId,@TourId,@AreaId,@TourType,@CostId
			,3,@Amount,0,0,@Amount
			,@PayerId,@DepartmentId,GETDATE(),0,0
			,'0',0,'')
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount=0)--标记订单返佣支付状态
	BEGIN
		UPDATE [tbl_TourOrder] SET [CommissionStatus]='1',[CommissionPayUId]=@PayerId,[CommissionPayTime]=GETDATE() WHERE [Id]=@OrderId
		SET @errorcount=@errorcount+@@ERROR
	END

	IF(@errorcount>0)
	BEGIN
		ROLLBACK TRAN
		SET @Result=0
		RETURN
	END

	COMMIT TRAN
	SET @Result=1
END
GO

-- =============================================
-- Author:		luofx
-- Create date: 2011-01-24
-- Description:	修改订单
-- History:
-- 1.增加组团联系人信息、返佣信息 汪奇志 2011-06-08
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
	SET @ErrorCount=0
	SET @Result=1
BEGIN	
	--判断订单人数是否大于剩余人数
    SELECT @OldPeopleNumber=ISNULL(PeopleNumber,0),@TourId=TourId FROM tbl_TourOrder WHERE ID=@OrderID AND IsDelete=0
	--未处理订单人数+已留位订单人数+已成交订单人数
	SELECT @PeopleNumberCount=ISNULL(SUM(PeopleNumber),0) FROM tbl_TourOrder WHERE id<>@OrderID AND TourId=@TourId AND (OrderState=1 OR OrderState=2 OR OrderState=5)  AND IsDelete=0	
	SELECT @VirtualPeopleNumber=ISNULL(VirtualPeopleNumber,0),@PlanPeopleNumber=ISNULL(PlanPeopleNumber,0) FROM tbl_Tour WHERE TourId=@TourId  AND IsDelete=0
	SELECT @ShiShu=ISNULL(SUM(PeopleNumber),0) FROM tbl_TourOrder WHERE IsDelete=0 AND TourId=@TourId AND (OrderState=1 OR OrderState=2 OR OrderState=5)
	IF(@IsTourOrderEdit=1 AND @PlanPeopleNumber -(@VirtualPeopleNumber-@OldPeopleNumber+@PeopleNumber)<0)
	BEGIN						
		SET @Result=2
	END

	IF(@IsTourOrderEdit=0 AND @PlanPeopleNumber-(@ShiShu-@OldPeopleNumber+@PeopleNumber)<0)
	BEGIN			
		SET @Result=2
	END
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
		SET @RealVirtualPeopleNumber=@VirtualPeopleNumber+@PeopleNumber-@OldPeopleNumber
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

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-06-09
-- Description:	删除订单
-- =============================================
ALTER PROCEDURE [dbo].[proc_TourOrder_Delete]
	@OrderId CHAR(36)--订单编号
AS
BEGIN
	DECLARE @TourId CHAR(36)--计划编号
	DECLARE @ShiShou INT--实收人数
	DECLARE @PlanPeopleNumber INT--计划人数
	DECLARE @TourStatus TINYINT--计划状态

	SELECT @TourId=[TourId] FROM [tbl_TourOrder] WHERE [ID]=@OrderId
	--标记订单删除
	UPDATE [tbl_TourOrder] SET [IsDelete]='1' WHERE [ID]=@OrderId
	SELECT @ShiShou=ISNULL(SUM(PeopleNumber-LeaguePepoleNum),0) FROM [tbl_TourOrder] WHERE [TourId]=@TourId AND [IsDelete]='0' AND [OrderState] NOT IN(3,4)
	--计划实收人数
	UPDATE [tbl_Tour] SET [VirtualPeopleNumber]=@ShiShou WHERE TourId=@TourId
	SELECT @PlanPeopleNumber=[PlanPeopleNumber],@TourStatus=[Status] FROM [tbl_Tour] WHERE [TourId]=@TourId

	--标记计划状态为客满 
	IF(@TourStatus=0 AND @PlanPeopleNumber=@ShiShou)
	BEGIN
		UPDATE [tbl_Tour] SET [Status]=1 WHERE [TourId]=@TourId
	END

	--标记计划状态为正常
	IF(@TourStatus=1 AND @PlanPeopleNumber>@ShiShou)
	BEGIN
		UPDATE [tbl_Tour] SET [Status]=0 WHERE [TourId]=@TourId
	END	

	--删除收入明细
	UPDATE [tbl_StatAllIncome] SET [IsDelete]='1' WHERE [ItemType]=1 AND [ItemId]=@OrderId
END
GO

-- =============================================
-- Author:		李焕超
-- Create date: 2011-03-01
-- Description:	机票申请修改、出票
-- History:
-- 1.2011-03-30 汪奇志 调整过程书写格式
-- 2.2011-04-21 汪奇志 机票票款增加百分比、其它费用
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

UPDATE tbl_PlanTicket SET TicketType=1 WHERE TicketType<>3
GO

-- =============================================
-- Author:		luofx
-- Create date: 2011-01-28
-- Description:	订单自动留位过期计划
-- History:
-- 1.汪奇志 2011-03-25 调整更新订单状态与设置团队虚拟实收人数的执行顺序
-- 2.汪奇志 2011-05-30 增加对线路库收客数的维护
-- 3.汪奇志	2011-06-14 增加对计划款项的维护
-- =============================================
ALTER PROCEDURE  [dbo].[SQLPlan_TourOder_SaveSeatUpdate]
	  
AS
BEGIN
	CREATE TABLE #TmpSQLPlan_TourOder_SaveSeatUpdate(
		TourId CHAR(36),
		OrderId CHAR(36),
		RouteId INT
	)
	INSERT INTO #TmpSQLPlan_TourOder_SaveSeatUpdate(TourId,OrderId,RouteId) 
	SELECT TourId,Id,RouteId FROM [tbl_TourOrder] WHERE OrderState=2  AND IsDelete=0 AND DATEDIFF(ms,SaveSeatDate,getdate())>0
	--修改留位已过时订单的状态为留位过期
	UPDATE [tbl_TourOrder] SET OrderState=3 
	WHERE ID IN(SELECT OrderId FROM #TmpSQLPlan_TourOder_SaveSeatUpdate)
	--维护团队虚拟实收数=订单总人数(不含留住过期、取消的订单)
	UPDATE tbl_Tour SET VirtualPeopleNumber=(SELECT ISNULL(SUM(PeopleNumber-LeaguePepoleNum),0) FROM tbl_TourOrder WHERE TourId=tbl_Tour.TourId  AND IsDelete='0' AND OrderState IN(1,2,5)) 
	WHERE TourId IN(SELECT TourId FROM #TmpSQLPlan_TourOder_SaveSeatUpdate)	
	--留位过期时，更新统计分析所有收入明细表   
	UPDATE tbl_StatAllIncome SET IsDelete=1 WHERE ItemId=(SELECT OrderId FROM #TmpSQLPlan_TourOder_SaveSeatUpdate) AND ItemType=1
	--团队状态维护 仅客满状态到正在收客状态
	UPDATE [tbl_Tour] SET [Status]=0 FROM [tbl_Tour] AS A 
	WHERE A.[Status]=1 AND A.TourId IN(SELECT TourId FROM #TmpSQLPlan_TourOder_SaveSeatUpdate) AND A.PlanPeopleNumber>(SELECT ISNULL(SUM(B.PeopleNumber-B.LeaguePepoleNum),0) FROM tbl_TourOrder AS B WHERE B.TourId=A.TourId AND B.[OrderState] NOT IN(3,4) AND IsDelete='0')
	--线路库收客数维护
	UPDATE [tbl_Route] SET VisitorCount=ISNULL((SELECT SUM(AdultNumber+ChildNumber) FROM [tbl_TourOrder] WHERE [OrderState] IN(1,2,5) AND RouteId=A.Id AND IsDelete='0'),0)
	FROM [tbl_Route] AS A WHERE A.Id IN(SELECT RouteId FROM #TmpSQLPlan_TourOder_SaveSeatUpdate WHERE RouteId>0)
	--维护计划总收入
	UPDATE [tbl_Tour] SET [TotalIncome]=ISNULL((SELECT SUM([FinanceSum]) FROM [tbl_TourOrder] AS B WHERE B.[TourId]=A.[TourId] AND B.[IsDelete]='0' AND B.[OrderState] NOT IN(3,4)),0)
	FROM [tbl_Tour] AS A
	WHERE A.[TourId] IN(SELECT [TourId] FROM #TmpSQLPlan_TourOder_SaveSeatUpdate)

	--维护计划结清状态
	DECLARE @TourId CHAR(36)
	DECLARE tmpcursor CURSOR
	FOR SELECT TourId FROM #TmpSQLPlan_TourOder_SaveSeatUpdate
	OPEN tmpcursor
	FETCH NEXT FROM tmpcursor INTO @TourId
	
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		DECLARE @CalculationTourSettleStatusResult INT
		EXEC [proc_Utility_CalculationTourSettleStatus] @TourId=@TourId,@Result=@CalculationTourSettleStatusResult OUTPUT

		FETCH NEXT FROM tmpcursor INTO @TourId
	END

	CLOSE tmpcursor
	DEALLOCATE tmpcursor	
	
	DROP TABLE #TmpSQLPlan_TourOder_SaveSeatUpdate
END
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-06-14
-- Description:	维护计划相关数据
-- =============================================
CREATE PROCEDURE proc_Utility_Maintenance
	@TourId CHAR(36)=NULL
AS
BEGIN
	DECLARE @procResult INT

	IF(@TourId IS NULL OR LEN(@TourId)<>36)--所有计划
	BEGIN
		DECLARE @tmptbl TABLE([TourId] CHAR(36))
		INSERT INTO @tmptbl([TourId])
		SELECT [TourId] FROM [tbl_Tour] WHERE IsDelete='0' AND [TemplateId]>''
		
		DECLARE tmpcursor CURSOR
		FOR SELECT [TourId] FROM @tmptbl
		OPEN tmpcursor
		FETCH NEXT FROM tmpcursor INTO @TourId
		
		WHILE (@@FETCH_STATUS = 0)
		BEGIN
			--维护计划收入相关项
			EXEC [proc_Utility_CalculationTourIncome] @TourId=@TourId,@IncomeItemIdAndTypeXML=NULL,@ErrorValue=@procResult OUTPUT
			--维护计划支出相关项
			EXEC [proc_Utility_CalculationTourOut] @TourId=@TourId,@ItemIdAndTypeXML=NULL,@ErrorValue=@procResult OUTPUT
			--维护计划款项结清状态
			EXEC [proc_Utility_CalculationTourSettleStatus] @TourId=@TourId,@Result=@procResult OUTPUT

			FETCH NEXT FROM tmpcursor INTO @TourId
		END

		CLOSE tmpcursor
		DEALLOCATE tmpcursor	

		RETURN
	END

	--维护计划收入相关项
	EXEC [proc_Utility_CalculationTourIncome] @TourId=@TourId,@IncomeItemIdAndTypeXML=NULL,@ErrorValue=@procResult OUTPUT
	--维护计划支出相关项
	EXEC [proc_Utility_CalculationTourOut] @TourId=@TourId,@ItemIdAndTypeXML=NULL,@ErrorValue=@procResult OUTPUT
	--维护计划款项结清状态
	EXEC [proc_Utility_CalculationTourSettleStatus] @TourId=@TourId,@Result=@procResult OUTPUT
	RETURN
END
GO


--(20110607)bbl.txt end


--线路上团数维护 start
-- =============================================
-- Author: luofx
-- Create date:2011-01-22
-- Description:	修改团队收客数，上团数
-- History:
-- 1.汪奇志 2011-05-30 修改收客数的统计条件
-- 2.汪奇志 2011-06-17 上团数统计条件加上是否删除
-- =============================================
ALTER PROCEDURE [dbo].[proc_UpdateTourCountOrVisitorCount] 
	@RouteId NVARCHAR(MAX), --线路编号:多个编号以半角逗号分开
	@UpdateTourType INT,  --要修改的类型：0:修改上团数；1：修改收客数；2：修改上团数和收客数
	@Result INT OUTPUT         --回传参数
AS
DECLARE @TourCount INT	  --上团数
DECLARE @RId INT      --单个线路Id值
DECLARE @VistorCount INT  --收客数
DECLARE @MaxId INT --临时id变量最大值
DECLARE @i INT     
SET @TourCount=0
SET @VistorCount=0
SET @i=1
SET @Result=1
BEGIN
	IF(len(@RouteId)>0)
		BEGIN
			SELECT @MaxId=MAX(Id) FROM dbo.fn_split(@RouteId,',')
			WHILE(@i<=@MaxId)
				BEGIN
					SELECT @RId=[VALUE] FROM dbo.fn_split(@RouteId,',') WHERE ID=@i
					IF(@UpdateTourType=0) --修改上团数				
						BEGIN
							SELECT @TourCount=ISNULL(COUNT(1),0) FROM [tbl_Tour] a WHERE a.[RouteId]=@RId AND TemplateId<>'' AND [IsDelete]='0'
							UPDATE [tbl_Route] SET TourCount=@TourCount WHERE ID=@RId
						END	
					ELSE IF(@UpdateTourType=1)--修改收客数
						BEGIN							
							SELECT @VistorCount=ISNULL(SUM(AdultNumber+ChildNumber),0) FROM [tbl_TourOrder] WHERE [RouteId]=@RId AND [IsDelete]='0' AND [OrderState] IN(1,2,5)
							UPDATE [tbl_Route] SET VisitorCount=@VistorCount WHERE ID=@RId							
						END	
					ELSE IF(@UpdateTourType=2)--修改上团数和收客数
						BEGIN
							SELECT @TourCount=ISNULL(COUNT(1),0) FROM [tbl_Tour] a WHERE [RouteId]=@RId AND TemplateId<>'' AND [IsDelete]='0'
							SELECT @VistorCount=ISNULL(SUM(AdultNumber+ChildNumber),0) FROM [tbl_TourOrder] WHERE [RouteId]=@RId AND [IsDelete]='0' AND [OrderState] IN(1,2,5)
							UPDATE [tbl_Route] SET TourCount=@TourCount,VisitorCount=@VistorCount WHERE ID=@RId
						END
					SET @i=@i+1
				END			
			IF(@@ERROR>0)
				BEGIN
					SET @Result=0
				END
		END
RETURN @Result
END
GO
UPDATE tbl_Route SET [TourCount]=(SELECT COUNT(*) FROM [tbl_Tour] AS A WHERE A.RouteId=tbl_Route.Id AND A.TemplateId>'' AND A.IsDelete='0')
WHERE IsDelete='0'
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-01-23
-- Description:	更新散拼计划、团队计划、单项服务信息
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
	@TourCityId INT=0--出港城市编号
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
			UPDATE [tbl_Tour] SET [Status]=2 WHERE [TourId]=@TourId AND [Status] =3 AND DATEDIFF(DAY,GETDATE(),[RDate])>0
			SET @errorcount=@errorcount+@@ERROR
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
		IF(@errorcount=0)--是否出团
		BEGIN
			UPDATE [tbl_Tour] SET IsLeave='1' WHERE TourId=@TourId AND LeaveDate<=GETDATE()
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0)--团队状态维护
		BEGIN
			UPDATE [tbl_Tour] SET [Status]=2 WHERE [TourId]=@TourId AND [Status] =3 AND DATEDIFF(DAY,GETDATE(),[RDate])>0
			SET @errorcount=@errorcount+@@ERROR
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
-- Author:		<李焕超,>
-- Create date: <Create 2011-1-21,,>
-- Description:	<添加客户,,>
-- History:
-- 1.2011-05-26 汪奇志 结算日期
-- 2.2011-06-07 汪奇志 结算方式 结算日类型
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
	,@Phone nvarchar(20)---电话
	,@Mobile nvarchar(20)---手机
	,@Fax nvarchar(20)--传真
	,@Remark nvarchar(500)---备注
	,@OperatorId int---操作员编号
	,@CustomerInfoXML NVARCHAR(MAX) ---联系人列表
	,@Result int output --输出参数 9 ，插入不分配账号的联系人 2，插入用户 8 ,未执行xml	 
	,@AccountDay TINYINT--结算日期,
	,@IsEnable CHAR(1)--是否启用,
	,@AccountWay NVARCHAR(255)--结算方式
	,@AccountDayType TINYINT--结算日类型
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
		,[IsDelete],[BrandId],[AccountDay],[AccountWay],[AccountDayType])
	VALUES(@ProviceId,@ProvinceName,@CityId,@CityName,@Name
		,@Licence,@Adress,@PostalCode,@BankAccount,@MaxDebts
		,@PreDeposit,@CommissionCount,@Saler,@SaleId,@CustomerLev
		,@CommissionType,@CompanyId,@IsEnable,@ContactName,@Phone
		,@Mobile,@Fax,@Remark,@OperatorId,getdate()
		,0,@BrandId,@AccountDay,@AccountWay,@AccountDayType)
	SELECT  @CustomerId =@@IDENTITY 

	if @CustomerInfoXML is null or len(@CustomerInfoXML)<10
	begin
		set @Result=1 
		return @Result
	end

-----开始 事务
 BEGIN TRANSACTION addCustomer

 EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@CustomerInfoXML
 declare @cursorCount int
 set @cursorCount=0  
        
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
		set @cursorCount = @cursorCount+1
		if @cursorCount >20
		begin
			set @Result =0
			return @Result
		end
		if  @cContactId=0 ---新增联系人
		begin
			if @cusername is not null and @cusername <>''---添加用户
			begin
				declare @IsUserExist int
				set @IsUserExist=0---判断用户是否存在
				select @IsUserExist=count(id) from tbl_CompanyUser where UserName=@cusername and CompanyId=@CompanyId
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
-- Author:		汪奇志
-- Create date: 2011-01-23
-- Description:	更新散拼计划、团队计划、单项服务信息
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
	@TourCityId INT=0--出港城市编号
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
			UPDATE [tbl_Tour] SET [Status]=2 WHERE [TourId]=@TourId AND [Status] =3 AND DATEDIFF(DAY,GETDATE(),[RDate])>0
			SET @errorcount=@errorcount+@@ERROR
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
		IF(@errorcount=0)--是否出团
		BEGIN
			UPDATE [tbl_Tour] SET IsLeave='1' WHERE TourId=@TourId AND LeaveDate<=GETDATE()
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0)--团队状态维护
		BEGIN
			UPDATE [tbl_Tour] SET [Status]=2 WHERE [TourId]=@TourId AND [Status] =3 AND DATEDIFF(DAY,GETDATE(),[RDate])>0
			SET @errorcount=@errorcount+@@ERROR
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

--线路上团数维护 end


--以上日志已更新至服务器






--(20110610)策划文档.txt  start
GO
-- =============================================
-- Author:		周文超
-- Create date: 2010-01-21
-- Description:	人次统计基本视图
-- Description:	周文超 2011-06-13修改,增加团队发布人列（TourOperatorId）
-- =============================================
ALTER VIEW [dbo].[View_InayatStatistic]
AS
select 
DepartId,DepartName,
SalerId,
tbl_CompanyUser.ContactName as SalerName,
ViewOperatorId,
SellCompanyId,AreaId,tbl_TourOrder.IssueTime,tbl_TourOrder.IsDelete,TourClassId,OrderState,
(PeopleNumber - LeaguePepoleNum) as PeopleCount,
((PeopleNumber - LeaguePepoleNum) * cast(TourDays as decimal)) as PeopleDays
,(select OperatorId from tbl_Tour where tbl_Tour.TourId = tbl_TourOrder.TourId) as TourOperatorId
from tbl_TourOrder left join tbl_CompanyUser on tbl_CompanyUser.Id = tbl_TourOrder.SalerId
where tbl_TourOrder.isdelete = '0' and tbl_TourOrder.IsDelete = '0' and tbl_TourOrder.OrderState not in (3,4) AND tbl_TourOrder.SalerId>0
GO
-- =============================================
-- Author:		周文超
-- Create date: 2011-04-19
-- Description:	机票管理--退票统计视图
-- History:
-- 1.2011-06-14 增加团队发布人列 [TourOperatorId]
-- =============================================
ALTER VIEW [dbo].[View_RefundStatistics]
AS
select a.Id,a.CustormerId,a.OperatorName,a.IssueTime,a.RefundAmount
,b.VisitorName
,c.TourNo,c.RouteName,c.LeaveDate,c.BuyCompanyName,c.OperatorName as OrderOperatorName,c.SellCompanyId,c.BuyCompanyID
,(select sum(TotalMoney) from tbl_PlanTicketKind where tbl_PlanTicketKind.TicketId in (select TicketOutId from tbl_PlanTicketOutCustomer where tbl_PlanTicketOutCustomer.UserId = a.CustormerId)) as TotalAmount
,(select ID,FligthSegment,DepartureTime,AireLine,Discount,TicketId,TicketTime from tbl_PlanTicketFlight where tbl_PlanTicketFlight.ID in (select tbl_CustomerRefundFlight.FlightId from tbl_CustomerRefundFlight where tbl_CustomerRefundFlight.CustomerId = a.CustormerId and tbl_CustomerRefundFlight.RefundId = a.Id) for xml raw,root('root')) as FligthInfo
,D.OperatorId AS TourOperatorId
from tbl_CustomerRefund as a inner join tbl_TourOrderCustomer as b on a.CustormerId = b.Id inner join tbl_TourOrder as c on c.ID = b.OrderId INNER JOIN tbl_Tour AS D
ON c.TourId=D.TourId
GO


-- =============================================
-- Author:		周文超
-- Create date: 2011-04-19
-- Description:	收入统计视图
-- History:
-- 1.2011-06-14 增加团队发布人列 [TourOperatorId]
-- =============================================
GO
ALTER VIEW [dbo].[view_StatAllIncomeStatement]
AS
SELECT a.CompanyId, a.TotalAmount, a.HasCheckAmount, a.NotCheckAmount,
	   b.SalerId, b.AreaId, b.LeaveDate, b.TourClassId AS TourType, 
		b.BuyCompanyName, b.BuyCompanyId,B.OrderState
		,(select OperatorId from tbl_tour where TourId = b.TourId) as TourOperatorId
FROM  dbo.tbl_StatAllIncome AS a INNER JOIN
   dbo.tbl_TourOrder AS b ON a.ItemId = b.ID AND b.IsDelete = '0'
WHERE (a.IsDelete = '0') AND (a.ItemType = 1)

GO


GO
-- =============================================
-- Author:		周文超
-- Create date: 2010-03-21
-- Description:	出票统计视图
-- History:
-- 1.2011-06-14 增加团队Id [pto.TourId]
-- =============================================
ALTER VIEW [dbo].[View_TicketOutStatisticDepart]
AS
select cu.DepartId
,cd.DepartName
,(select isnull(sum(PeopleCount),0) from tbl_PlanTicketKind as ptk where ptk.TicketId = pto.ID) as PeopleCount
,pto.ID,pto.TotalAmount,pto.PayAmount,pto.TicketOfficeId,pto.CompanyID,pto.RegisterOperatorId,pto.[State],pto.TicketOffice,pto.TicketOutTime,pto.PNR,pto.TicketNum,pto.TourId
,(select AireLine,FligthSegment,DepartureTime,Discount,TicketTime from tbl_PlanTicketFlight where tbl_PlanTicketFlight.TicketId = pto.ID for xml raw,root('root')) as AireLine
,(select Price,OilFee,PeopleCount,AgencyPrice,TotalMoney,TicketType from tbl_PlanTicketKind where tbl_PlanTicketKind.TicketId = pto.ID for xml raw,root('root')) as TicketKind
from tbl_PlanTicketOut as pto left join tbl_CompanyUser as cu on pto.RegisterOperatorId = cu.ID left join tbl_CompanyDepartment as cd on cu.DepartId = cd.ID

GO


GO
-- =============================================
-- Author:		周文超
-- Create date: 2011-04-19
-- Description:	机票管理--退票统计视图
-- History:
-- 1.2011-06-14 增加团队发布人列 [TourOperatorId]
-- =============================================
ALTER VIEW [dbo].[View_RefundStatistics]
AS
select a.Id,a.CustormerId,a.OperatorName,a.IssueTime,a.RefundAmount
,b.VisitorName
,c.TourNo,c.RouteName,c.LeaveDate,c.BuyCompanyName,c.BuyerContactName as OrderOperatorName,c.SellCompanyId,c.BuyCompanyID
,(select sum(TotalMoney) from tbl_PlanTicketKind where tbl_PlanTicketKind.TicketId in (select TicketOutId from tbl_PlanTicketOutCustomer where tbl_PlanTicketOutCustomer.UserId = a.CustormerId)) as TotalAmount
,(select ID,FligthSegment,DepartureTime,AireLine,Discount,TicketId,TicketTime from tbl_PlanTicketFlight where tbl_PlanTicketFlight.ID in (select tbl_CustomerRefundFlight.FlightId from tbl_CustomerRefundFlight where tbl_CustomerRefundFlight.CustomerId = a.CustormerId and tbl_CustomerRefundFlight.RefundId = a.Id) for xml raw,root('root')) as FligthInfo
,D.OperatorId AS TourOperatorId
from tbl_CustomerRefund as a inner join tbl_TourOrderCustomer as b on a.CustormerId = b.Id inner join tbl_TourOrder as c on c.ID = b.OrderId INNER JOIN tbl_Tour AS D
ON c.TourId=D.TourId




-- =============================================
-- Author:		周文超
-- Create date: 2011-04-19
-- History:
-- 1.2011-06-14 增加团队发布人列 [TourOperatorId]
-- =============================================
GO
ALTER VIEW [dbo].[View_TravelAndTicketArrear]
AS
SELECT	a.TourId, a.TotalAmount - a.PayAmount AS Arrear, a.TotalAmount,a.PayAmount as PayAmount, b.TourCode, b.RouteName, b.LeaveDate, a.TicketOfficeId AS ServerId, 
		a.CompanyID AS CompanyId, 2 AS ServerType, a.OperateID, a.Operator AS operator, b.TourType, b.AreaId, a.TicketOffice AS Supplier, 
		a.RegisterTime AS regtime, b.SellerId AS salerId,(select isnull(sum(peopleCount),0) from tbl_PlanTicketKind where tbl_PlanTicketKind.TicketId=a.id) as peopleCount,b.OperatorId as TourOperatorId
FROM         dbo.tbl_PlanTicketOut AS a INNER JOIN
                      dbo.tbl_Tour AS b ON a.TourId = b.TourId where a.state = 3 
UNION ALL
SELECT     c.TourId, c.TotalAmount - c.PayAmount AS Arrear, c.TotalAmount,c.PayAmount as PayAmount, d.TourCode, d.RouteName, d.LeaveDate, c.TravelAgencyID AS ServerId, c.CompanyId, 
                      1 AS ServerType, c.OperatorID AS OperateID, c.Operator, d.TourType, d.AreaId, c.LocalTravelAgency AS Supplier, c.OperateTime AS regtime, 
                      d.SellerId AS salerId,(select isnull(sum(PeopleCount),0) from tbl_PlanLocalAgencyPrice where tbl_PlanLocalAgencyPrice.ReferenceID=c.id) as peoplecount,d.OperatorId as TourOperatorId
FROM         dbo.tbl_PlanLocalAgency AS c INNER JOIN
                      dbo.tbl_Tour AS d ON c.TourId = d.TourId
union all  --单项服务支出
select 
a.TourId,a.TotalAmount - a.PaidAmount AS Arrear,a.TotalAmount,a.PaidAmount as PayAmount,b.TourCode,'单项服务' as RouteName,leavedate as leavedate,a.SupplierId as ServerId,b.companyid as companyid,a.ServiceType as ServerType,a.OperatorId as OperateID,
(select ContactName from tbl_CompanyUser where tbl_CompanyUser.id = a.OperatorId) as operator
,b.TourType,b.AreaId,a.SupplierName as Supplier,a.CreateTime as regtime, b.SellerId AS salerId,b.PlanPeopleNumber as peopleCount,b.OperatorId as TourOperatorId
from tbl_PlanSingle as a inner join  tbl_Tour as b on a.tourid = b.tourid
GO

--(20110610)策划文档.txt  End







--(20110620)巴比来.doc   Start


GO
-- =============================================
-- Author:		周文超
-- Create date: 2010-01-21
-- Description:	人次统计基本视图
-- 周文超 2011-06-20增加出团日期列
-- =============================================
ALTER VIEW [dbo].[View_InayatStatistic]
AS
select 
DepartId,DepartName,
SalerId,
tbl_CompanyUser.ContactName as SalerName,
ViewOperatorId,
SellCompanyId,AreaId,tbl_TourOrder.IssueTime,tbl_TourOrder.IsDelete,TourClassId,OrderState,LeaveDate,
(PeopleNumber - LeaguePepoleNum) as PeopleCount,
((PeopleNumber - LeaguePepoleNum) * cast(TourDays as decimal)) as PeopleDays
,(select OperatorId from tbl_Tour where tbl_Tour.TourId = tbl_TourOrder.TourId) as TourOperatorId
from tbl_TourOrder left join tbl_CompanyUser on tbl_CompanyUser.Id = tbl_TourOrder.SalerId
where tbl_TourOrder.isdelete = '0' and tbl_TourOrder.IsDelete = '0' and tbl_TourOrder.OrderState not in (3,4) AND tbl_TourOrder.SalerId>0

GO

-- =============================================
-- Author:		周文超
-- Create date: 2010-01-24
-- Description:	收款提醒视图-全部用户
-- =============================================
ALTER VIEW [dbo].[View_ReceiptRemind_GetList]
AS
select 
Id,[Name],ContactName,Phone,CompanyId
,(select isnull(sum(FinanceSum - HasCheckMoney),0) from tbl_TourOrder as tto where tto.BuyCompanyID = tc.Id and tto.SellCompanyId = tc.CompanyId and tto.IsDelete = '0' and tto.OrderState not in (3,4)) as ArrearCash
,(select SalerName from tbl_TourOrder where tbl_TourOrder.BuyCompanyID = tc.Id and tbl_TourOrder.SellCompanyId = tc.CompanyId and tbl_TourOrder.IsDelete = '0' and tbl_TourOrder.OrderState not in (3,4) for xml auto,root('root')) as SalerInfo
,AccountDay,AccountDayType
from tbl_Customer as tc
where tc.IsDelete = '0'
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2010-06-21
-- Description:	收款提醒视图-按订单销售员
-- =============================================
CREATE VIEW View_ReceiptRemind
AS
SELECT C.*,D.[Name],D.[ContactName],D.[Phone],D.[CompanyId],D.[Id],D.[AccountDay],D.[AccountDayType],(SELECT ContactName FROM tbl_CompanyUser WHERE [Id]=C.SalerId) AS SalerName FROM (
SELECT A.[SellCompanyId],A.[SalerId],A.[BuyCompanyId],SUM(A.[FinanceSum]-A.[HasCheckMoney]) AS NotPayAmount
FROM [tbl_TourOrder] AS A
WHERE A.[IsDelete]='0' AND A.[OrderState] NOT IN(3,4) AND A.[SalerId]>0
GROUP BY A.[SellCompanyId],A.[BuyCompanyId],A.[SalerId]) AS C INNER JOIN tbl_Customer AS D
ON C.BuyCompanyId=D.[Id] AND D.[IsDelete]='0'
GO

--(20110620)巴比来.doc   End

--以上日志已更新至服务器（2011-06-24）




--第二批次(20110620)巴比来.doc start

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- History:
-- 1.2011-06-27 汪奇志 增加对支出明细表的删除
-- =============================================
ALTER PROCEDURE [dbo].[proc_PlanTravelAgency_Del]
(
@TravelId char(36),
@Result int output
)
AS
BEGIN
	set @Result=0
	DECLARE @errorCount INT
	SET @errorCount=0
	begin transaction

	delete from [tbl_PlanLocalAgencyPrice] where ReferenceID=@TravelId
	SET @errorCount=@errorCount+@@ERROR

	IF(@errorCount=0)
	BEGIN
		delete from [tbl_PlanLocalAgencyTourPlan] where LocalTravelId=@TravelId
		SET @errorCount=@errorCount+@@ERROR
	END

	IF(@errorCount=0)
	BEGIN
		delete from [tbl_PlanLocalAgency] where id=@TravelId
		SET @errorCount=@errorCount+@@ERROR
	END

	IF(@errorCount=0)
	BEGIN
		UPDATE [tbl_StatAllOut] SET [IsDelete]='1' WHERE [ItemId]=@TravelId AND [ItemType]=1
		SET @errorCount=@errorCount+@@ERROR
	END

	if(@errorCount>0)
	begin
		set @Result=0
		rollback transaction
		return @Result 
	end
	
	commit transaction
	SET @Result=1
	return @Result 
END
GO

-- =============================================
-- Author:		<李焕超,>
-- Create date: <Create 2011-1-21,,>
-- Description:	<添加客户,,>
-- History:
-- 1.2011-05-26 汪奇志 结算日期
-- 2.2011-06-07 汪奇志 结算方式 结算日类型
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
	,@Phone nvarchar(20)---电话
	,@Mobile nvarchar(20)---手机
	,@Fax nvarchar(20)--传真
	,@Remark nvarchar(500)---备注
	,@OperatorId int---操作员编号
	,@CustomerInfoXML NVARCHAR(MAX) ---联系人列表
	,@Result int output --输出参数 9 ，插入不分配账号的联系人 2，插入用户 8 ,未执行xml	 
	,@AccountDay TINYINT--结算日期,
	,@IsEnable CHAR(1)--是否启用,
	,@AccountWay NVARCHAR(255)--结算方式
	,@AccountDayType TINYINT--结算日类型
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
		,[IsDelete],[BrandId],[AccountDay],[AccountWay],[AccountDayType])
	VALUES(@ProviceId,@ProvinceName,@CityId,@CityName,@Name
		,@Licence,@Adress,@PostalCode,@BankAccount,@MaxDebts
		,@PreDeposit,@CommissionCount,@Saler,@SaleId,@CustomerLev
		,@CommissionType,@CompanyId,@IsEnable,@ContactName,@Phone
		,@Mobile,@Fax,@Remark,@OperatorId,getdate()
		,0,@BrandId,@AccountDay,@AccountWay,@AccountDayType)
	SELECT  @CustomerId =@@IDENTITY 

	if @CustomerInfoXML is null or len(@CustomerInfoXML)<10
	begin
		set @Result=1 
		return @Result
	end

-----开始 事务
 BEGIN TRANSACTION addCustomer

 EXECUTE sp_xml_preparedocument @hdoc OUTPUT,@CustomerInfoXML
 declare @cursorCount int
 set @cursorCount=0  
        
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
		set @cursorCount = @cursorCount+1
		if @cursorCount >20
		begin
			set @Result =0
			return @Result
		end
		if  @cContactId=0 ---新增联系人
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
	,@Phone nvarchar(20)---电话
	,@Mobile nvarchar(20)---手机
	,@Fax nvarchar(20)--传真
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
		,[AccountDay]=@AccountDay,[AccountWay]=@AccountWay,[AccountDayType]=@AccountDayType
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
 declare @cursorCount int
 set @cursorCount=0  
        
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
 set @cursorCount = @cursorCount+1
 if @cursorCount >20
 begin
 set @Result =1
 return @Result
 end
    if  @cContactId=0 ---新增联系人
    begin
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
			update tbl_CompanyUser set Password=@cPwd,MD5Password=@cPwdMd5,ContactName=@cContactName,
contactsex=@cSex,contactmobile=@cMobile,contactemail=@cEmail,qq=@cqq,jobname=@cJob,departname=@cDepartment,ContactTel=@cTel,ContactFax = @cFax where username=@cusername and usertype=1 and CompanyId=@CompanyId---修改已存在用户密码
		  end
     end--判断是否填写用户名密码结束
      
      select top 1 @newUserId=id from tbl_CompanyUser where username=@cusername
      if @newUserId is null 
      set @newUserId =0
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


--以上日志已更新 2011-06-28



--计划表增加我社报价单价合计金额字段 汪奇志 2011-06-28
ALTER TABLE dbo.tbl_Tour ADD
	SelfUnitPriceAmount money NOT NULL CONSTRAINT DF_tbl_Tour_SelfUnitPriceAmount DEFAULT 0
GO
DECLARE @v sql_variant 
SET @v = N'我社报价单价合计金额'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'dbo', N'TABLE', N'tbl_Tour', N'COLUMN', N'SelfUnitPriceAmount'
GO

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-01-21
-- Description:	写入散拼计划、团队计划、单项服务信息
-- History:
-- 1.2011-04-18 增加地接报价人数、单价及我社报价人数、单价
-- 2.2011-06-08 团队计划订单增加返佣
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
	@TeamPrivate NVARCHAR(MAX)=NULL,--团队计划专有信息 XML:<ROOT><Info OrderId="订单编号" SellerId="销售员编号" SellerName="销售员姓名" TotalAmount="合计金额" BuyerCId="客户单位编号" BuyerCName="客户单位名称" ContactName="联系人" ContactTelephone="联系电话" Mobile="联系手机" QuoteId="报价编号" SelfUnitPriceAmount="我社报价单价合计金额" /></ROOT>
	@CustomerDisplayType TINYINT=0,--游客信息体现类型
	@Customers NVARCHAR(MAX)=NULL,--订单游客信息 XML:<ROOT><Info TravelerId="游客编号" TravelerName="游客姓名" IdType="证件类型" IdNo="证件号" Gender="性别" TravelerType="类型(成人/儿童)" ContactTel="联系电话" TF_XiangMu="特服项目" TF_NeiRong="特服内容" TF_ZengJian="1:增加 0:减少" TF_FeiYong="特服费用" /></ROOT>	
	@CustomerFilePath NVARCHAR(255)=NULL,--订单游客附件
	@Result INT OUTPUT,--操作结果 正值：成功 负值或0：失败
	@CoordinatorId INT=0,--计调员编号,
	@GatheringTime NVARCHAR(MAX)=NULL,--集合时间
	@GatheringPlace NVARCHAR(MAX)=NULL,--集合地点
	@GatheringSign NVARCHAR(MAX)=NULL,--集合标志
	@SentPeoples NVARCHAR(MAX)=NULL,--送团人信息集合 XML:<ROOT><Info UID="送团人编号" /></ROOT>
	@TourCityId INT=0--出港城市编号
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
			,[GatheringSign],[SelfUnitPriceAmount])
		SELECT @TourId,@TourId,@TourType,@LDate,DATEADD(DAY,@TourDays-1,@LDate)
			,@AreaId,@Status,'0','0',@TicketStatus
			,@TourCode,@RouteId,@RouteName,@TourDays,@PlanPeopleNumber
			,@PlanPeopleNumber,@ReleaseType,@LTraffic,@RTraffic,@Gather
			,GETDATE(),@OperatorId,@SellerId,@CompanyId,A.TotalAmount
			,GETDATE(),@IncomeAmount,0,0,0
			,0,0,'0','0','0'
			,NULL,0,A.QuoteId,@GatheringTime,@GatheringPlace
			,@GatheringSign,A.SelfUnitPriceAmount
		FROM OPENXML(@hdoc,'/ROOT/Info')
		WITH(SellerId INT,TotalAmount MONEY,QuoteId INT,SelfUnitPriceAmount MONEY) AS A
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
		IF(@errorcount=0)--是否出团
		BEGIN
			UPDATE [tbl_Tour] SET IsLeave='1' WHERE TourId=@TourId AND LeaveDate<=GETDATE()
			SET @errorcount=@errorcount+@@ERROR
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

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-01-23
-- Description:	更新散拼计划、团队计划、单项服务信息
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
	@TeamPrivate NVARCHAR(MAX)=NULL,--团队计划专有信息 XML:<ROOT><Info OrderId="订单编号" SellerId="销售员编号" SellerName="销售员姓名" TotalAmount="合计金额" BuyerCId="客户单位编号" BuyerCName="客户单位名称" ContactName="联系人" ContactTelephone="联系电话" ContactMobile="联系手机" SelfUnitPriceAmount="我社报价单价合计金额" /></ROOT>
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
	@TourCityId INT=0--出港城市编号
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
			UPDATE [tbl_Tour] SET [Status]=2 WHERE [TourId]=@TourId AND [Status] =3 AND DATEDIFF(DAY,GETDATE(),[RDate])>0
			SET @errorcount=@errorcount+@@ERROR
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
			,[SelfUnitPriceAmount]=A.SelfUnitPriceAmount
		FROM [tbl_Tour] AS B,(SELECT SellerId,TotalAmount,SelfUnitPriceAmount FROM OPENXML(@hdoc,'/ROOT/Info')
		WITH(SellerId INT,TotalAmount MONEY,SelfUnitPriceAmount MONEY)) A
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
		IF(@errorcount=0)--是否出团
		BEGIN
			UPDATE [tbl_Tour] SET IsLeave='1' WHERE TourId=@TourId AND LeaveDate<=GETDATE()
			SET @errorcount=@errorcount+@@ERROR
		END
		IF(@errorcount=0)--团队状态维护
		BEGIN
			UPDATE [tbl_Tour] SET [Status]=2 WHERE [TourId]=@TourId AND [Status] =3 AND DATEDIFF(DAY,GETDATE(),[RDate])>0
			SET @errorcount=@errorcount+@@ERROR
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
-- Author:		汪奇志
-- Create date: 2011-01-21
-- Description:	写入散拼计划、团队计划、单项服务信息
-- History:
-- 1.2011-04-18 增加地接报价人数、单价及我社报价人数、单价
-- 2.2011-06-08 团队计划订单增加返佣
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
	@TeamPrivate NVARCHAR(MAX)=NULL,--团队计划专有信息 XML:<ROOT><Info OrderId="订单编号" SellerId="销售员编号" SellerName="销售员姓名" TotalAmount="合计金额" BuyerCId="客户单位编号" BuyerCName="客户单位名称" ContactName="联系人" ContactTelephone="联系电话" Mobile="联系手机" QuoteId="报价编号" SelfUnitPriceAmount="我社报价单价合计金额" /></ROOT>
	@CustomerDisplayType TINYINT=0,--游客信息体现类型
	@Customers NVARCHAR(MAX)=NULL,--订单游客信息 XML:<ROOT><Info TravelerId="游客编号" TravelerName="游客姓名" IdType="证件类型" IdNo="证件号" Gender="性别" TravelerType="类型(成人/儿童)" ContactTel="联系电话" TF_XiangMu="特服项目" TF_NeiRong="特服内容" TF_ZengJian="1:增加 0:减少" TF_FeiYong="特服费用" /></ROOT>	
	@CustomerFilePath NVARCHAR(255)=NULL,--订单游客附件
	@Result INT OUTPUT,--操作结果 正值：成功 负值或0：失败
	@CoordinatorId INT=0,--计调员编号,
	@GatheringTime NVARCHAR(MAX)=NULL,--集合时间
	@GatheringPlace NVARCHAR(MAX)=NULL,--集合地点
	@GatheringSign NVARCHAR(MAX)=NULL,--集合标志
	@SentPeoples NVARCHAR(MAX)=NULL,--送团人信息集合 XML:<ROOT><Info UID="送团人编号" /></ROOT>
	@TourCityId INT=0--出港城市编号
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
			,[GatheringSign],[SelfUnitPriceAmount])
		SELECT @TourId,@TourId,@TourType,@LDate,DATEADD(DAY,@TourDays-1,@LDate)
			,@AreaId,@Status,'0','0',@TicketStatus
			,@TourCode,@RouteId,@RouteName,@TourDays,@PlanPeopleNumber
			,@PlanPeopleNumber,@ReleaseType,@LTraffic,@RTraffic,@Gather
			,GETDATE(),@OperatorId,@SellerId,@CompanyId,A.TotalAmount
			,GETDATE(),@IncomeAmount,0,0,0
			,0,0,'0','0','0'
			,NULL,0,A.QuoteId,@GatheringTime,@GatheringPlace
			,@GatheringSign,A.SelfUnitPriceAmount
		FROM OPENXML(@hdoc,'/ROOT/Info')
		WITH(SellerId INT,TotalAmount MONEY,QuoteId INT,SelfUnitPriceAmount MONEY) AS A
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

-- =============================================
-- Author:		汪奇志
-- Create date: 2011-01-23
-- Description:	更新散拼计划、团队计划、单项服务信息
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
	@TeamPrivate NVARCHAR(MAX)=NULL,--团队计划专有信息 XML:<ROOT><Info OrderId="订单编号" SellerId="销售员编号" SellerName="销售员姓名" TotalAmount="合计金额" BuyerCId="客户单位编号" BuyerCName="客户单位名称" ContactName="联系人" ContactTelephone="联系电话" ContactMobile="联系手机" SelfUnitPriceAmount="我社报价单价合计金额" /></ROOT>
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
	@TourCityId INT=0--出港城市编号
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
			,[SelfUnitPriceAmount]=A.SelfUnitPriceAmount
		FROM [tbl_Tour] AS B,(SELECT SellerId,TotalAmount,SelfUnitPriceAmount FROM OPENXML(@hdoc,'/ROOT/Info')
		WITH(SellerId INT,TotalAmount MONEY,SelfUnitPriceAmount MONEY)) A
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

--以上日志已更新
--第二批次(20110620)巴比来.doc end
