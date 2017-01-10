CREATE PROCEDURE [dbo].[USP_GetArchitectCount]
  @ArchitectCount int out
AS
  SET NOCOUNT ON
  BEGIN
	select @ArchitectCount= COUNT(1) FROM [dbo].[MIND] where IS_ACTIVE=1
  END