USE [PortfolioManager]
GO
DECLARE	@return_value int
EXEC	@return_value = [dbo].[sp_InsertTrade] @Symbol = N'QQQ', @Shares = 10, @Price = 109
SELECT	'Return Value' = @return_value
GO

GO
DECLARE	@return_value int
EXEC	@return_value = [dbo].[sp_InsertTrade] @Symbol = N'AAPL', @Shares = 15, @Price = 99
SELECT	'Return Value' = @return_value
GO

GO
DECLARE	@return_value int
EXEC	@return_value = [dbo].[sp_InsertTrade] @Symbol = N'YHOO', @Shares = 40, @Price = 35
SELECT	'Return Value' = @return_value
GO

GO
DECLARE	@return_value int
EXEC	@return_value = [dbo].[sp_InsertTrade] @Symbol = N'MSFT', @Shares = 20, @Price = 52
SELECT	'Return Value' = @return_value
GO

GO
DECLARE	@return_value int
EXEC	@return_value = [dbo].[sp_InsertTrade] @Symbol = N'FB', @Shares = 10, @Price = 117
SELECT	'Return Value' = @return_value
GO

GO
DECLARE	@return_value int
EXEC	@return_value = [dbo].[sp_InsertTrade] @Symbol = N'INTC', @Shares = 35, @Price = 31
SELECT	'Return Value' = @return_value
GO

GO
DECLARE	@return_value int
EXEC	@return_value = [dbo].[sp_InsertTrade] @Symbol = N'NFLX', @Shares = 10, @Price = 100
SELECT	'Return Value' = @return_value
GO