CREATE PROCEDURE [dbo].[USP_GetAllMinds]
AS
BEGIN

	SELECT 
		[MID] as [MID],
		[NAME] as [Name],
		[PROFESSIONAL_SUMMARY] as [ProfessionalSummary],
		[EXPERIENCE_IN_MONTHS] as [ExperienceInMonths],
		[DESIGNATION] as [Designation],
		[BASE_LOCATION] as [BaseLocation],
		[QUALIFICATION] as [Qualification],
		[IS_ACTIVE] as [IsActive],
		[JOINED_DATE_DD] as [JoinedDateDD],
		[JOINED_DATE_MM] as [JoinedDateMM],
		[JOINED_DATE_YYYY] as [JoinedDateYYYY],
		[INACTIVE_DATE_DD] as [InactiveDateDD],
		[INACTIVE_DATE_MM] as [InactiveDateMM],
		[INACTIVE_DATE_YYYY] as [InactiveDateYYYY]
	FROM 
		[dbo].[MIND]

END