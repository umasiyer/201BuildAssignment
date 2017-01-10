﻿USE [CSGPortal]

GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'USP_FetchMIds')
                    AND type IN ( N'P', N'PC' ) ) 

DROP PROCEDURE [dbo].[USP_FetchMIds]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_FetchMIds]
   
AS

BEGIN
    SET NOCOUNT ON
	SELECT MID FROM MIND 
END

GO
