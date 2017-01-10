using System.Collections.Generic;
using MT.CSGPortal.Portable.Entities;

namespace MT.CSGPortal.DAL
{
    public interface IMindDataAccess
    {
        List<Mind> GetAllMinds { get; }
        Mind GetMindByID(string id);
        List<MindContact> GetMindContactsByMid(string mId);
        MindFullProfile GetMindFullProfileById(string id);
        int ManageMindProfile(MindFullProfile mindProfile);
        int GetArchitectCount();
        List<string> GetAllMIds { get; }
        bool IsMindInPortal(string MID);
    }
}
