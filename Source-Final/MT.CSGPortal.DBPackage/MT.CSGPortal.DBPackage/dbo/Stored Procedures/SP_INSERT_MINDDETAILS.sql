CREATE PROCEDURE [dbo].[SP_INSERT_MINDDETAILS]
@mid varchar(12),
@name varchar(50),
@professionSummary varchar(4000),
@experienceInMonths int,
@designation varchar(50),
@baseLocation varchar(50),
@qualification varchar(50),
@isActive int,
@joinedDate varchar(10),
@inactiveDate varchar(10),
@organizationGroupId int

AS
SET NOCOUNT ON

BEGIN

-- split the date value to DD,MM & YYYY
DECLARE @joinedDD int,@joinedMM int,@joinedYYYY int
-- sample date value 19-10-2011
	set @joinedDD = CONVERT(int,SUBSTRING(@joinedDate,1,2))
	set @joinedMM = CONVERT(int,SUBSTRING(@joinedDate,4,2))
	set @joinedYYYY = CONVERT(int,SUBSTRING(@joinedDate,7,4))

INSERT INTO MIND 
	(MID,NAME,PROFESSIONAL_SUMMARY,EXPERIENCE_IN_MONTHS,DESIGNATION,BASE_LOCATION,
	QUALIFICATION,IS_ACTIVE,JOINED_DATE_DD,JOINED_DATE_MM,JOINED_DATE_YYYY,INACTIVE_DATE_DD,INACTIVE_DATE_MM,INACTIVE_DATE_YYYY,
	ORGANIZATION_GROUP_ID)
VALUES
	(@mid,@name,@professionSummary,@experienceInMonths,@designation,@baseLocation,
	@qualification,@isActive,@joinedDD,@joinedMM,@joinedYYYY,null,null,null,@organizationGroupId)

END