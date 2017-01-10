using System.Collections.Generic;
using MT.CSGPortal.Portable.Entities;
using MT.CSGPortal.Entities;

namespace MT.CSGPortal.BL
{
    public interface IActiveDirectoryManager
    {
        MindFullProfile GetMindFullProfileById(string mId);
        object GetUser(string mId);
        List<Mind> Search(string searchTerm);
        SearchResult<MindBasicProfile> Search(string searchTerm, byte pageNumber);
        bool ValidateUser(string mId, string password, string domain);
        byte[] GetImage(string mId);
    }
}
