﻿CREATE PROCEDURE [dbo].[USP_FetchMindContacts]
@Mid varchar(12)

AS
SET NOCOUNT ON

BEGIN

SELECT 
	MID,
	MIND_CONTACT.CONTACT_TYPE_ID AS ContactTypeId,
	CONTACT_TEXT AS ContactText,
	MIND_CONTACT_ID AS MindContactId,
	CONTACT_TYPE.NAME AS Name
FROM 
MIND_CONTACT
INNER JOIN CONTACT_TYPE
ON MIND_CONTACT.CONTACT_TYPE_ID = CONTACT_TYPE.CONTACT_TYPE_ID
WHERE MID=@Mid

END