﻿CREATE procedure [dbo].[sp_get_mindprofile]   

AS
    SET NOCOUNT ON
	
	SELECT MID,NAME,PROFESSIONAL_SUMMARY,EXPERIENCE_IN_MONTHS,DESIGNATION,BASE_LOCATION,QUALIFICATION,
	REPLACE(CONVERT(CHAR(10),(CONVERT(VARCHAR,JOINED_DATE_DD) + '-' + 
			CONVERT(VARCHAR,JOINED_DATE_MM) + '-' + CONVERT(VARCHAR,JOINED_DATE_YYYY)),103),'/','')
	AS JOINED_DATE
	FROM MIND
	WHERE IS_ACTIVE = 1