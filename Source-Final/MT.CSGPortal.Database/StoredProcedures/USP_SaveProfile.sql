USE [CSGPortal]

GO

IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'USP_SaveProfile')
                    AND type IN ( N'P', N'PC' ) ) 

DROP PROCEDURE [dbo].USP_SaveProfile
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_SaveProfile]
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
   @InactiveDateYYYY int=NULL,
   @contacts UDTT_MindContact readonly
AS

BEGIN
    SET NOCOUNT ON
	exec USP_SaveMind @MID,@Name,@ProfessionalSummary,@ExperienceInMonths,@Designation,@BaseLocation,@Qualification,@IsActive,@JoinedDateDD,@JoinedDateMM,@JoinedDateYYYY,@InactiveDateDD,@InactiveDateMM,@InactiveDateYYYY
	exec USP_SaveContact @contacts
END

GO
