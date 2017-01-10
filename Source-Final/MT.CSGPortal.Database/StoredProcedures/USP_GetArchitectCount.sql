USE [CSGPortal]
GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'USP_GetArchitectCount')
                    AND type IN ( N'P', N'PC' ) ) 

DROP PROCEDURE [dbo].[USP_GetArchitectCount]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_GetArchitectCount]
  @ArchitectCount int out
AS
  BEGIN
	select @ArchitectCount= COUNT(1) FROM [dbo].[MIND] where IS_ACTIVE=1
  END