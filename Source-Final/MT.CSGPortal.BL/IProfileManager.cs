using System.Collections.Generic;
using MT.CSGPortal.Portable.Entities;
using MT.CSGPortal.Entities;

namespace MT.CSGPortal.BL
{
    public interface IProfileManager
    {
        List<Mind> GetAllMinds { get; }
        SearchResult<MindBasicProfile> SearchMinds(string searchParameter, int pageNumber);
        Mind GetMindByID(string id);
        List<MindContact> GetMindContactsByMid(string mId);
        MindFullProfile GetMindFullProfileByMid(string id);
        void ManageMindProfile(MindFullProfile profile);
        void SyncAllProfileDetails();
        bool IsMindInPortal(string MID);
    }
}
