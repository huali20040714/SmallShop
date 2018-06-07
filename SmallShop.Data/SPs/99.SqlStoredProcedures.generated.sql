
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_ExceptionLog_Create]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[gen_ExceptionLog_Create]
END
GO
-- =============================================
-- Create date: 6/6/2018 4:54:43 PM
-- Description: 创建ExceptionLog
-- =============================================
CREATE PROCEDURE [dbo].[gen_ExceptionLog_Create]
@Id int output,
@LoginName nvarchar(20) ,
@Url varchar(500) ,
@Message varchar(1000) ,
@StackTrace varchar(MAX) ,
@Ip varchar(20) ,
@CreateTime datetime 
AS
INSERT INTO [ExceptionLog](
[LoginName],
[Url],
[Message],
[StackTrace],
[Ip],
[CreateTime]
)
VALUES(
@LoginName,

@Url,

@Message,

@StackTrace,

@Ip,

@CreateTime
)

    select scope_identity();



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_ExceptionLog_Update]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[gen_ExceptionLog_Update]
END
GO
-- =============================================
-- Create date: 6/6/2018 4:54:43 PM
-- Description: 更新ExceptionLog
-- =============================================
CREATE PROCEDURE [dbo].[gen_ExceptionLog_Update]
@Id int,
@LoginName nvarchar(20),
@Url varchar(500),
@Message varchar(1000),
@StackTrace varchar(MAX),
@Ip varchar(20),
@CreateTime datetime
AS

UPDATE [ExceptionLog] set
	  
	[LoginName] = @LoginName,
	[Url] = @Url,
	[Message] = @Message,
	[StackTrace] = @StackTrace,
	[Ip] = @Ip,
	[CreateTime] = @CreateTime
WHERE 
Id = @Id



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_OperationLog_Create]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[gen_OperationLog_Create]
END
GO
-- =============================================
-- Create date: 6/6/2018 4:54:43 PM
-- Description: 创建OperationLog
-- =============================================
CREATE PROCEDURE [dbo].[gen_OperationLog_Create]
@Id int output,
@LoginName nvarchar(20) ,
@Type int ,
@BusinessName nvarchar(50) ,
@Description nvarchar(500) ,
@Ip varchar(20) ,
@CreateTime datetime 
AS
INSERT INTO [OperationLog](
[LoginName],
[Type],
[BusinessName],
[Description],
[Ip],
[CreateTime]
)
VALUES(
@LoginName,

@Type,

@BusinessName,

@Description,

@Ip,

@CreateTime
)

    select scope_identity();



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_OperationLog_Update]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[gen_OperationLog_Update]
END
GO
-- =============================================
-- Create date: 6/6/2018 4:54:43 PM
-- Description: 更新OperationLog
-- =============================================
CREATE PROCEDURE [dbo].[gen_OperationLog_Update]
@Id int,
@LoginName nvarchar(20),
@Type int,
@BusinessName nvarchar(50),
@Description nvarchar(500),
@Ip varchar(20),
@CreateTime datetime
AS

UPDATE [OperationLog] set
	  
	[LoginName] = @LoginName,
	[Type] = @Type,
	[BusinessName] = @BusinessName,
	[Description] = @Description,
	[Ip] = @Ip,
	[CreateTime] = @CreateTime
WHERE 
Id = @Id



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Order_Create]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[gen_Order_Create]
END
GO
-- =============================================
-- Create date: 6/6/2018 4:54:43 PM
-- Description: 创建Order
-- =============================================
CREATE PROCEDURE [dbo].[gen_Order_Create]
@Id int output,
@UserId int ,
@Tel varchar(11) ,
@Ip varchar(20) ,
@CreateTime datetime 
AS
INSERT INTO [Order](
[UserId],
[Tel],
[Ip],
[CreateTime]
)
VALUES(
@UserId,

@Tel,

@Ip,

@CreateTime
)

    select scope_identity();



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Order_Update]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[gen_Order_Update]
END
GO
-- =============================================
-- Create date: 6/6/2018 4:54:43 PM
-- Description: 更新Order
-- =============================================
CREATE PROCEDURE [dbo].[gen_Order_Update]
@Id int,
@UserId int,
@Tel varchar(11),
@Ip varchar(20),
@CreateTime datetime
AS

UPDATE [Order] set
	  
	[UserId] = @UserId,
	[Tel] = @Tel,
	[Ip] = @Ip,
	[CreateTime] = @CreateTime
WHERE 
Id = @Id



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_OrderItem_Create]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[gen_OrderItem_Create]
END
GO
-- =============================================
-- Create date: 6/6/2018 4:54:43 PM
-- Description: 创建OrderItem
-- =============================================
CREATE PROCEDURE [dbo].[gen_OrderItem_Create]
@Id int output,
@OrderId int ,
@Number varchar(20) ,
@ProductId int ,
@OnlinePrice decimal(18, 2) ,
@SettlementPrice decimal(18, 2) ,
@Copies int ,
@UsedCopies int ,
@RefundCopies int ,
@Barcode varchar(20) ,
@ExpendSolidStartDate datetime ,
@ExpendSolidEndDate datetime 
AS
INSERT INTO [OrderItem](
[OrderId],
[Number],
[ProductId],
[OnlinePrice],
[SettlementPrice],
[Copies],
[UsedCopies],
[RefundCopies],
[Barcode],
[ExpendSolidStartDate],
[ExpendSolidEndDate]
)
VALUES(
@OrderId,

@Number,

@ProductId,

@OnlinePrice,

@SettlementPrice,

@Copies,

@UsedCopies,

@RefundCopies,

@Barcode,

@ExpendSolidStartDate,

@ExpendSolidEndDate
)

    select scope_identity();



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_OrderItem_Update]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[gen_OrderItem_Update]
END
GO
-- =============================================
-- Create date: 6/6/2018 4:54:43 PM
-- Description: 更新OrderItem
-- =============================================
CREATE PROCEDURE [dbo].[gen_OrderItem_Update]
@Id int,
@OrderId int,
@Number varchar(20),
@ProductId int,
@OnlinePrice decimal(18, 2),
@SettlementPrice decimal(18, 2),
@Copies int,
@UsedCopies int,
@RefundCopies int,
@Barcode varchar(20),
@ExpendSolidStartDate datetime,
@ExpendSolidEndDate datetime
AS

UPDATE [OrderItem] set
	  
	[OrderId] = @OrderId,
	[Number] = @Number,
	[ProductId] = @ProductId,
	[OnlinePrice] = @OnlinePrice,
	[SettlementPrice] = @SettlementPrice,
	[Copies] = @Copies,
	[UsedCopies] = @UsedCopies,
	[RefundCopies] = @RefundCopies,
	[Barcode] = @Barcode,
	[ExpendSolidStartDate] = @ExpendSolidStartDate,
	[ExpendSolidEndDate] = @ExpendSolidEndDate
WHERE 
Id = @Id



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Role_Create]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[gen_Role_Create]
END
GO
-- =============================================
-- Create date: 6/6/2018 4:54:43 PM
-- Description: 创建Role
-- =============================================
CREATE PROCEDURE [dbo].[gen_Role_Create]
@Id int output,
@Name nvarchar(20) ,
@Permissions varchar(4000) ,
@IsInner bit ,
@CreateTime datetime 
AS
INSERT INTO [Role](
[Name],
[Permissions],
[IsInner],
[CreateTime]
)
VALUES(
@Name,

@Permissions,

@IsInner,

@CreateTime
)

    select scope_identity();



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_Role_Update]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[gen_Role_Update]
END
GO
-- =============================================
-- Create date: 6/6/2018 4:54:43 PM
-- Description: 更新Role
-- =============================================
CREATE PROCEDURE [dbo].[gen_Role_Update]
@Id int,
@Name nvarchar(20),
@Permissions varchar(4000),
@IsInner bit,
@CreateTime datetime
AS

UPDATE [Role] set
	  
	[Name] = @Name,
	[Permissions] = @Permissions,
	[IsInner] = @IsInner,
	[CreateTime] = @CreateTime
WHERE 
Id = @Id



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_RoleUser_Create]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[gen_RoleUser_Create]
END
GO
-- =============================================
-- Create date: 6/6/2018 4:54:43 PM
-- Description: 创建RoleUser
-- =============================================
CREATE PROCEDURE [dbo].[gen_RoleUser_Create]

@UserId int ,
@RoleId int 
AS
INSERT INTO [RoleUser](
[UserId],
[RoleId]
)
VALUES(
@UserId,

@RoleId
)


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_RoleUser_Update]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[gen_RoleUser_Update]
END
GO
-- =============================================
-- Create date: 6/6/2018 4:54:43 PM
-- Description: 更新RoleUser
-- =============================================
CREATE PROCEDURE [dbo].[gen_RoleUser_Update]
@UserId int,
@RoleId int
AS

UPDATE [RoleUser] set
	[UserId] = @UserId,
	[RoleId] = @RoleId
WHERE 	[UserId] = @UserId And
	[RoleId] = @RoleId 



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_User_Create]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[gen_User_Create]
END
GO
-- =============================================
-- Create date: 6/6/2018 4:54:43 PM
-- Description: 创建User
-- =============================================
CREATE PROCEDURE [dbo].[gen_User_Create]
@Id int output,
@LoginName nvarchar(20) ,
@Password varchar(32) ,
@Type int ,
@Balance decimal(18, 4) ,
@Depth int ,
@ParentId int ,
@WeiXinOpenId varchar(32) ,
@CreateTime datetime 
AS
INSERT INTO [User](
[LoginName],
[Password],
[Type],
[Balance],
[Depth],
[ParentId],
[WeiXinOpenId],
[CreateTime]
)
VALUES(
@LoginName,

@Password,

@Type,

@Balance,

@Depth,

@ParentId,

@WeiXinOpenId,

@CreateTime
)

    select scope_identity();



GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_User_Update]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[gen_User_Update]
END
GO
-- =============================================
-- Create date: 6/6/2018 4:54:43 PM
-- Description: 更新User
-- =============================================
CREATE PROCEDURE [dbo].[gen_User_Update]
@Id int,
@LoginName nvarchar(20),
@Password varchar(32),
@Type int,
@Balance decimal(18, 4),
@Depth int,
@ParentId int,
@WeiXinOpenId varchar(32),
@CreateTime datetime
AS

UPDATE [User] set
	  
	[LoginName] = @LoginName,
	[Password] = @Password,
	[Type] = @Type,
	[Balance] = @Balance,
	[Depth] = @Depth,
	[ParentId] = @ParentId,
	[WeiXinOpenId] = @WeiXinOpenId,
	[CreateTime] = @CreateTime
WHERE 
Id = @Id



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_UserParent_Create]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[gen_UserParent_Create]
END
GO
-- =============================================
-- Create date: 6/6/2018 4:54:43 PM
-- Description: 创建UserParent
-- =============================================
CREATE PROCEDURE [dbo].[gen_UserParent_Create]

@UserId int ,
@ParentId1 int ,
@ParentId2 int ,
@ParentId3 int ,
@ParentId4 int ,
@ParentId5 int ,
@ParentId6 int ,
@ParentId7 int ,
@ParentId8 int ,
@ParentId9 int ,
@ParentId10 int 
AS
INSERT INTO [UserParent](
[UserId],
[ParentId1],
[ParentId2],
[ParentId3],
[ParentId4],
[ParentId5],
[ParentId6],
[ParentId7],
[ParentId8],
[ParentId9],
[ParentId10]
)
VALUES(
@UserId,

@ParentId1,

@ParentId2,

@ParentId3,

@ParentId4,

@ParentId5,

@ParentId6,

@ParentId7,

@ParentId8,

@ParentId9,

@ParentId10
)


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gen_UserParent_Update]') AND type in (N'P', N'PC'))
BEGIN
DROP PROCEDURE [dbo].[gen_UserParent_Update]
END
GO
-- =============================================
-- Create date: 6/6/2018 4:54:43 PM
-- Description: 更新UserParent
-- =============================================
CREATE PROCEDURE [dbo].[gen_UserParent_Update]
@UserId int,
@ParentId1 int,
@ParentId2 int,
@ParentId3 int,
@ParentId4 int,
@ParentId5 int,
@ParentId6 int,
@ParentId7 int,
@ParentId8 int,
@ParentId9 int,
@ParentId10 int
AS

UPDATE [UserParent] set
	  
	[ParentId1] = @ParentId1,
	[ParentId2] = @ParentId2,
	[ParentId3] = @ParentId3,
	[ParentId4] = @ParentId4,
	[ParentId5] = @ParentId5,
	[ParentId6] = @ParentId6,
	[ParentId7] = @ParentId7,
	[ParentId8] = @ParentId8,
	[ParentId9] = @ParentId9,
	[ParentId10] = @ParentId10
WHERE 
UserId = @UserId




