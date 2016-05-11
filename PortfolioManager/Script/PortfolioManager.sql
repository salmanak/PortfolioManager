USE [PortfolioManager]
GO
/****** Object:  Table [dbo].[Trades]    Script Date: 05/11/2016 14:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Trades](
	[Symbol] [varchar](30) NOT NULL,
	[Shares] [int] NOT NULL,
	[Price] [float] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[sp_GetTrade]    Script Date: 05/11/2016 14:33:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetTrade]
	@Symbol varchar(30)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Symbol], 
		[Shares], 
		[Price]
	FROM Trades
	WHERE [Symbol] = @Symbol

END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetTrades]    Script Date: 05/11/2016 14:33:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetTrades]

AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Symbol], 
		[Shares], 
		[Price]
	FROM Trades

END
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertTrade]    Script Date: 05/11/2016 14:33:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertTrade]
	@Symbol varchar(30),
	@Shares int,
	@Price float
AS
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
END
GO
