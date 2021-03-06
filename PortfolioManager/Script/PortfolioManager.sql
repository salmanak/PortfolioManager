USE [PortfolioManager]
GO
/****** Object:  Table [dbo].[Trades]    Script Date: 05/20/2016 15:54:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Trades](
	[ID] int IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Symbol] [varchar](30) NOT NULL,
	[Shares] [int] NOT NULL,
	[Price] [float] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[sp_GetTrade]    Script Date: 05/20/2016 15:54:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetTrade]
	@Symbol varchar(30)
AS
DECLARE @err_message nvarchar(255)
BEGIN
	SET NOCOUNT ON;
	
	--if symbol not found then return error to abort stored procedure
	IF NOT EXISTS (Select Symbol from Trades where Symbol = @Symbol) 
	BEGIN	
		SET @err_message = @Symbol + ' symbol not found.'
		RAISERROR (@err_message, 11,1)
	END

	SELECT [Symbol], 
		[Shares], 
		[Price]
	FROM Trades1
	WHERE [Symbol] = @Symbol

	IF @@ERROR <>0
	BEGIN
		SET @err_message = 'An error occured while getting the Trade for Symbol ' + @Symbol
		RAISERROR ( @err_message , 11,1)
	END

END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetTrades]    Script Date: 05/20/2016 15:54:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetTrades]

AS
DECLARE @err_message nvarchar(255)
BEGIN
	SET NOCOUNT ON;

	SELECT [Symbol], 
		[Shares], 
		[Price]
	FROM Trades


	IF @@ERROR <> 0
	 BEGIN
		SET @err_message = 'An error occured while getting the Trades.'
		RAISERROR ( @err_message , 11,1)
	 END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertTrade]    Script Date: 05/20/2016 15:54:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertTrade]
	@Symbol varchar(30),
	@Shares int,
	@Price float
AS
DECLARE @err_message nvarchar(255)
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Trades]
           ([Symbol]
           ,[Shares]
           ,[Price])
     VALUES
           (@Symbol
           ,@Shares
           ,@Price)
           
           
     IF @@ERROR <> 0
	 BEGIN
		SET @err_message = 'An error occured while getting the Trades.'
		RAISERROR ( @err_message , 11,1)		
	 END
END
GO
