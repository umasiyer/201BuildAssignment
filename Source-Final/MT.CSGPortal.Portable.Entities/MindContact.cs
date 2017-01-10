
namespace MT.CSGPortal.Portable.Entities
{
    public class MindContact
    {
        public MindContact()
        {
            MindContactType = new ContactType();
        }
        public string MID { get; set; }
        public ContactType MindContactType { get; set; }
        public string ContactText { get; set; }
        public int MindContactId { get; set; }
    }
}
