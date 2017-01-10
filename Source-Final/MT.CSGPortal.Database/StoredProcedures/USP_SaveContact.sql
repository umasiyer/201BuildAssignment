USE [CSGPortal]
GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'USP_SaveContact')
                    AND type IN ( N'P', N'PC' ) ) 

DROP PROCEDURE [dbo].[USP_SaveContact]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_SaveContact]
@contacts UDTT_MindContact readonly

AS
SET NOCOUNT ON

BEGIN

SET NOCOUNT ON;
	UPDATE MIND_CONTACT 
	    set CONTACT_TEXT=c.ContactText from MIND_CONTACT as m inner join @contacts as c on (m.MID=c.MID and m.CONTACT_TYPE_ID=c.ContactTypeId) 
		  where (c.ContactText is not null and c.ContactText<>'');
	
	DELETE  MIND_CONTACT from MIND_CONTACT as m inner join @contacts as c on (m.MID=c.MID and m.CONTACT_TYPE_ID=c.ContactTypeId) 
	     where (c.ContactText is null or c.ContactText='') 

	INSERT  into MIND_CONTACT(MID,CONTACT_TYPE_ID,CONTACT_TEXT) 
	    select * from @contacts as c where not exists(select  MID 
		  from MIND_CONTACT where MID=c.MID and CONTACT_TYPE_ID=c.ContactTypeId) and (c.ContactText is not null and c.ContactText<>'') 
END


GO