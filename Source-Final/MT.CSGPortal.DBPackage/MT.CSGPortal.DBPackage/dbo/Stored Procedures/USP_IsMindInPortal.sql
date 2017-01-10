CREATE PROCEDURE [dbo].[USP_IsMindInPortal]
	-- Add the parameters for the stored procedure here
	 @MID varchar(12),
	 @result int out
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select @result=count(1) from dbo.MIND 
	               where exists(select 1 from dbo.MIND
				     where MID=@MID)
	
END


/****** Object:  Table [dbo].[ALLOCATION_REQUEST]    Script Date: 3/20/2014 3:16:49 PM ******/
SET ANSI_NULLS ON