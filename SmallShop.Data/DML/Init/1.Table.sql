
--用户表
DROP TABLE [User]
go
CREATE TABLE [User]
(
	[Id] [int] IDENTITY(1001, 1) Primary key NOT NULL,
	[LoginName] [nvarchar](20) NULL,						-- 客户端只能是电话号码
	[Password] [varchar](32) NULL,                          -- MD5加密（小写）
    [Type] [int] NULL,                                      -- 用户类型
	[Balance] [decimal](18, 4) NULL,	                    -- 余额
	[Depth] int NOT NULL,                                   -- 管理员是1,每一级下面加1
	[ParentId] int NULL,						            -- 父类Id
	[WeiXinOpenId] [varchar](32) NULL,                      -- 微信OpenId
	[CreateTime] [datetime] NULL                            -- 创建时间
)
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'enum=UserType' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User', @level2type=N'COLUMN',@level2name=N'Type'
go

-- 用户角色
DROP TABLE [Role]
go
CREATE TABLE [Role]
(
	[Id] [int] IDENTITY(1,1) primary key NOT NULL,
	[Name] [nvarchar](20) NULL,                     -- 角色名称
	[Permissions] [varchar](4000) NULL,
    [IsInner] bit NULL,                             -- 是否为内部角色
	[CreateTime] [datetime] NULL
)
go

-- 用户对应的角色(一对多)
DROP TABLE [RoleUser]
GO
CREATE TABLE [RoleUser]
(
    [UserId] Int not null,
	[RoleId] [int] not NULL,
    primary key(UserId, RoleId)
)
GO

-- 操作日志                   
DROP TABLE [OperationLog]
GO
CREATE TABLE [OperationLog]
(
	[Id] [int] IDENTITY(1,1) primary key NOT NULL,	
	[LoginName] [nvarchar](20) NULL,
	[Type] [int] NOT NULL,
	[BusinessName] [nvarchar](50) NULL,
	[Description] [nvarchar](500) NULL,
	[Ip] [varchar](20) NOT NULL,
	[CreateTime] [datetime] NULL
)
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'enum=OperationType' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OperationLog', @level2type=N'COLUMN',@level2name=N'Type'
GO

-- 日志记录
DROP TABLE [ExceptionLog]
GO
CREATE TABLE [ExceptionLog]
(
	[Id] [int] IDENTITY(1,1) primary key NOT NULL,
	[LoginName] [nvarchar](20) NULL,
	[Url] [varchar](500) NULL,
	[Message] [varchar](1000) NULL,
	[StackTrace] [varchar](max) NULL,
	[Ip] [varchar](20) NOT NULL,
	[CreateTime] [datetime] NULL
)
go

-- 投注订单
DROP TABLE [Order]
GO
CREATE TABLE [Order]
(
	[Id] [int] identity(1,1) primary key NOT NULL,
	[UserId] [int] NULL,
	[Tel] [varchar](11) NULL,									-- (用于)接收验证码的电话号码

	[Ip] [varchar](20) NOT NULL,                                -- 下单Ip
	[CreateTime] [datetime] NULL                                -- 下单时间
)
GO

-- 投注订单
DROP TABLE [OrderItem]
GO
CREATE TABLE [OrderItem]
(
	[Id] [int] identity(1,1) primary key NOT NULL,
	[OrderId] [int] NOT NULL,
	[Number] [varchar](20) NULL,								-- 订单项(订单号)
    
	[ProductId] [int] NOT NULL,
	[OnlinePrice] [decimal](18, 2) NULL,
	[SettlementPrice] [decimal](18, 2) NULL,

	[Copies] [int] NOT NULL,
	[UsedCopies] [int] NULL,
	[RefundCopies] [int] NULL,
	[Barcode] [varchar](20) NULL,
	
	[ExpendSolidStartDate] [datetime] NULL,
	[ExpendSolidEndDate] [datetime] NULL,
)
GO

DROP TABLE [UserParent]
GO
CREATE TABLE [UserParent]
(
	[UserId] [int] Primary key NOT NULL,
    [ParentId1] int NULL,						            -- 父类Id1(1级)
    [ParentId2] int NULL,						            -- 父类Id1(2级)
    [ParentId3] int NULL,						            -- 父类Id1(3级)
    [ParentId4] int NULL,						            -- 父类Id1(4级)
    [ParentId5] int NULL,						            -- 父类Id1(5级)
    [ParentId6] int NULL,						            -- 父类Id1(6级)
    [ParentId7] int NULL,						            -- 父类Id1(7级)
    [ParentId8] int NULL,						            -- 父类Id1(8级)
    [ParentId9] int NULL,						            -- 父类Id1(9级)
    [ParentId10] int NULL,						            -- 父类Id1(10级)
)
