using MT.CSGPortal.DAL;
using MT.CSGPortal.Entities;

using MT.CSGPortal.Portable.Entities;
using MT.CSGPortal.Utility;
using System.Collections.Generic;
using System.Linq;

namespace MT.CSGPortal.BL
{
    public class ProfileManager : IProfileManager
    {
        private IMindDataAccess dataAccessObj;
        private LuceneSearch luceneSearchObj;
        private IActiveDirectoryManager actvDirMgr;
        /// <summary>
        /// Search in Portal DB using key words (Name,MID,Location,Designation)
        /// </summary>
        /// <param name="searchParameter">keyword</param>
        /// <param name="pageNumber">current page number(minimum 1)</param>
        /// <returns>DTO with basic mind details</returns>
        public SearchResult<MindBasicProfile> SearchMinds(string searchParameter, int pageNumber)
        {
            SearchResult<MindBasicProfile> mindProfileDtoObj;                     
            dataAccessObj = new MindDataAccess();
            List<MindBasicProfile> mindProfileLstObj = new List<MindBasicProfile>();
            mindProfileLstObj = dataAccessObj.GetAllMinds.Select(m=>new MindBasicProfile(m)).ToList<MindBasicProfile>();
            luceneSearchObj = new LuceneSearch(); 
            //perform lucene index search
            mindProfileDtoObj = luceneSearchObj.SearchProfileData(mindProfileLstObj,searchParameter, pageNumber);
            return mindProfileDtoObj;
        }

        /// <summary>
        /// Get mind details from Portal DB by MID
        /// </summary>
        /// <param name="id">MID</param>
        /// <returns>Mind details excluding contact info</returns>
        public Mind GetMindByID(string id)
        {
            dataAccessObj = new MindDataAccess();
            Mind mind = dataAccessObj.GetMindByID(id);
            return mind;
        }

        /// <summary>
        /// Fetch details of all minds in portal DB
        /// </summary>
        /// <returns>Details of minds excluding contact</returns>
        public List<Mind> GetAllMinds
        {
            get
            {
                dataAccessObj = new MindDataAccess();
                return dataAccessObj.GetAllMinds;
            }
        }

        /// <summary>
        /// Get contact details of a mind by MID
        /// </summary>
        /// <param name="mId">MID</param>
        /// <returns>List of mind contacts</returns>
        public List<MindContact>GetMindContactsByMid(string mId)
        {
            dataAccessObj = new MindDataAccess();
            return dataAccessObj.GetMindContactsByMid(mId);
        }

        /// <summary>
        /// Get all details of a mind from portal DB by MID
        /// </summary>
        /// <param name="id">MID</param>
        /// <returns>Mind's full profile</returns>
        public MindFullProfile GetMindFullProfileByMid(string id)
        {
            dataAccessObj = new MindDataAccess();
            return dataAccessObj.GetMindFullProfileById(id);
        }

        /// <summary>
        /// Add, edit (includes setting status to innactive) mind information in Portal DB
        /// </summary>
        /// <param name="profile">Mind Details including contacts</param>
        public void ManageMindProfile(MindFullProfile profile)
        {
            dataAccessObj=new MindDataAccess();
           
            dataAccessObj.ManageMindProfile(profile);
        }

        /// <summary>
        /// Fetch all active MIDs from Portal DB and update the data for each
        /// </summary>
        public void SyncAllProfileDetails()
        {
            dataAccessObj = new MindDataAccess();
            IList<string> mIdList = dataAccessObj.GetAllMIds;
            foreach (string id in mIdList)
            {
                SyncProfileDetailsByMId(id);
            }
        }

        /// <summary>
        /// update the details of a mind with the data from AD
        /// </summary>
        /// <param name="id">MID</param>
        public void SyncProfileDetailsByMId(string id)
        {
            MindFullProfile fullProfileAd = new MindFullProfile();
            dataAccessObj = new MindDataAccess();
            actvDirMgr = new ActiveDirectoryManager();
            if (ApplicationSettingsReader.IsADMocked > 0)
            {
                actvDirMgr = new ActiveDirectoryManagerMocked();
            }
            MindFullProfile fullProfilePortal = dataAccessObj.GetMindFullProfileById(id);
            fullProfileAd = actvDirMgr.GetMindFullProfileById(id);
            if (fullProfileAd != null)
            {
                fullProfileAd.MindDetails.ProfessionalSummary = fullProfilePortal.MindDetails.ProfessionalSummary;
                fullProfileAd.MindDetails.ExperienceInMonths = fullProfilePortal.MindDetails.ExperienceInMonths;
                dataAccessObj.ManageMindProfile(fullProfileAd);
            }
            else
            {
                dataAccessObj.ManageMindProfile(fullProfilePortal);
            }
        }

        /// <summary>
        /// Checks if mind exists in portal
        /// </summary>
        /// <param name="MID"></param>
        /// <returns>True if mind exists</returns>
        public bool IsMindInPortal(string id)
        {
            dataAccessObj = new MindDataAccess();
            return dataAccessObj.IsMindInPortal(id);

        }
    }
}
