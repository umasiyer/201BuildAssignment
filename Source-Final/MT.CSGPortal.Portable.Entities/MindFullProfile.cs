using System.Collections.Generic;

namespace MT.CSGPortal.Portable.Entities
{
    public class MindFullProfile
    {
        public MindFullProfile()
        {
            MindDetails = new Mind();
            MindContacts = new List<MindContact>();
        }

        public Mind MindDetails { get; set; }
        public IEnumerable<MindContact> MindContacts { get; set; }
    }
}
