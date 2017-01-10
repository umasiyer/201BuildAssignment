
namespace MT.CSGPortal.Portable.Entities
{
    public class MindBasicProfile
    {
        public MindBasicProfile(){}
        public MindBasicProfile(Mind mindObj)
        {
            Mid = mindObj.MID;
            Name = mindObj.Name;
            ProfessionalSummary = mindObj.ProfessionalSummary;
            ExperienceInMonths = mindObj.ExperienceInMonths;
            JoinedDate = string.Format("{0} {1} {2}", mindObj.JoinedDateDD == 0 ? string.Empty : mindObj.JoinedDateDD.ToString(), (mindObj.JoinedDateMM > 0 && mindObj.JoinedDateMM < 13) ? System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(mindObj.JoinedDateMM) : string.Empty, mindObj.JoinedDateYYYY == 0 ? string.Empty : mindObj.JoinedDateYYYY.ToString()); 
            Qualification = mindObj.Qualification;
            Designation = mindObj.Designation;
            BaseLocation = mindObj.BaseLocation;
        }

        public string Mid { get; set; }
        public string Name { get; set; }
        public string ProfessionalSummary { get; set; }
        public int ExperienceInMonths { get; set; }
        public string JoinedDate { get; set; }
        public string Qualification { get; set; }
        public string Designation { get; set; }
        public string BaseLocation { get; set; }
    }
}
