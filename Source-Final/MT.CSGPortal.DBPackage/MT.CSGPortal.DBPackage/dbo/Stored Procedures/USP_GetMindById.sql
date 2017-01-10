CREATE PROCEDURE [dbo].[USP_GetMindById]
	@mid NVARCHAR(12)
AS
BEGIN

	SELECT 
		[MID],
		[NAME],
		[PROFESSIONAL_SUMMARY] as ProfessionalSummary,
		[EXPERIENCE_IN_MONTHS] as ExperienceInMonths,
		[DESIGNATION],
		[BASE_LOCATION] as BaseLocation,
		[QUALIFICATION],
		[IS_ACTIVE] as IsActive,
		[JOINED_DATE_DD] as JoinedDateDD,
		[JOINED_DATE_MM] as JoinedDateMM,
		[JOINED_DATE_YYYY] as JoinedDateYYYY,
		[INACTIVE_DATE_DD] as InactiveDateDD,
		[INACTIVE_DATE_MM] as InactiveDateMM,
		[INACTIVE_DATE_YYYY] as InactiveDateYYYY
	FROM 
		[dbo].[MIND]
	WHERE
		[MID] = @mid;

END