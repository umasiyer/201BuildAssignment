USE [CSGPortal]
GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'USP_SaveMind')
                    AND type IN ( N'P', N'PC' ) ) 

DROP PROCEDURE [dbo].[USP_SaveMind]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_SaveMind]
@MID varchar(12),
   @Name varchar(50),
   @ProfessionalSummary varchar(4000)=NULL,
   @ExperienceInMonths int,
   @Designation varchar(50),
   @BaseLocation varchar(50)=NULL,
   @Qualification varchar(50)=NULL,
   @IsActive int=NULL,
   @JoinedDateDD int=NULL,
   @JoinedDateMM int=NULL,
   @JoinedDateYYYY int=NULL,
   @InactiveDateDD int=NULL,
   @InactiveDateMM int=NULL,
   @InactiveDateYYYY int=NULL

AS
SET NOCOUNT ON

BEGIN

IF EXISTS(SELECT TOP 1 MID FROM [dbo].[MIND] WHERE MID=@MID)

UPDATE MIND SET
	NAME=@Name,
	PROFESSIONAL_SUMMARY=@ProfessionalSummary,
	EXPERIENCE_IN_MONTHS=@ExperienceInMonths,
	DESIGNATION=@Designation,
	BASE_LOCATION=@BaseLocation,
	QUALIFICATION=@Qualification,
	IS_ACTIVE=@IsActive,
	JOINED_DATE_DD=@JoinedDateDD,
	JOINED_DATE_MM=@JoinedDateMM,
	JOINED_DATE_YYYY=@JoinedDateYYYY,
	INACTIVE_DATE_DD=@InactiveDateDD,
	INACTIVE_DATE_MM=@InactiveDateMM,
	INACTIVE_DATE_YYYY=@InactiveDateYYYY

ELSE

INSERT INTO MIND (
	MID,
	NAME,
	PROFESSIONAL_SUMMARY,
	EXPERIENCE_IN_MONTHS,
	DESIGNATION,
	BASE_LOCATION,
	QUALIFICATION,
	IS_ACTIVE,
	JOINED_DATE_DD,
	JOINED_DATE_MM,
	JOINED_DATE_YYYY,
	INACTIVE_DATE_DD,
	INACTIVE_DATE_MM,
	INACTIVE_DATE_YYYY
	)
VALUES(
	@MID,
	@Name ,
	@ProfessionalSummary,
	@ExperienceInMonths,
	@Designation,
	@BaseLocation,
	@Qualification,
	1,
	@JoinedDateDD,
	@JoinedDateMM,
	@JoinedDateYYYY,
	@InactiveDateDD,
	@InactiveDateMM,
	@InactiveDateYYYY
	)
END

GO


