﻿CREATE PROCEDURE [dbo].[USP_FetchActiveMIds]
   
AS

BEGIN
    SET NOCOUNT ON
	SELECT MID FROM MIND WHERE IS_ACTIVE<>0
END