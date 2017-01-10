using MT.CSGPortal.Entities;
using MT.CSGPortal.Portable.Entities;
using MT.CSGPortal.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace MT.CSGPortal.BL
{
    public class ActiveDirectoryManagerMocked:IActiveDirectoryManager
    {
        public Portable.Entities.MindFullProfile GetMindFullProfileById(string mId)
        {
            var res = (from p in ADMocked.Mindlist where p.MindDetails.MID==mId select p).FirstOrDefault();
            return (MindFullProfile)res;
        }

        public Entities.SearchResult<Portable.Entities.MindBasicProfile> Search(string searchTerm,byte pageNumber)
        {
            //Get PageSize from Configuration 
            int pageSize = ApplicationSettingsReader.PageSizeForADSearchResult;

            //Get Minds from AD
            SearchResult<MindBasicProfile> result = new SearchResult<MindBasicProfile>();
            List<Mind> minds = ADMocked.Mindlist.Select(p=>p.MindDetails).ToList<Mind>();
            result.ResultData = new List<MindBasicProfile>();

            //Filtering records based on Page Number.
            int lowerindex = (pageNumber - 1) * pageSize;
            int upperindex = lowerindex + pageSize;
            if (minds != null)
            {
                if (minds.Count <= lowerindex || minds.Count == 0)
                {
                    result.EndOfRecords = true;
                }
                if (minds.Count > lowerindex && minds.Count <= upperindex)
                {
                    int n = minds.Count - lowerindex;
                    List<Mind> filteredminds = (from m in minds.Skip(lowerindex).Take(n) select m).ToList<Mind>();
                    result.ResultData = filteredminds.Select(m => new MindBasicProfile(m)).ToList<MindBasicProfile>();
                    result.TotalRecordCount = n;
                    result.EndOfRecords = true;
                }
                if (minds.Count > upperindex)
                {
                    List<Mind> filteredminds = (from m in minds.Skip(lowerindex).Take(pageSize) select m).ToList<Mind>();
                    result.ResultData = filteredminds.Select(m => new MindBasicProfile(m)).ToList<MindBasicProfile>();
                    result.TotalRecordCount = pageSize;
                    result.EndOfRecords = false;
                }
            }
            else
            {
                result.EndOfRecords = true;
            }
            return result;
        }

        public object GetUser(string mId)
        {
            throw new NotImplementedException();
        }

        public List<Portable.Entities.Mind> Search(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUser(string mId, string password, string domain)
        {
            throw new NotImplementedException();
        }

        public byte[] GetImage(string mId)
        {
            string basePath= AppDomain.CurrentDomain.BaseDirectory;
            string path = basePath + "Images\\users\\user-male.jpg";
            Image img = Image.FromFile(path);
            using (var ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }

    public static class ADMocked
    {
        private static IList<MindFullProfile> mindList = new List<MindFullProfile>();

        static ADMocked()
        {
            for (int i = 1; i <= 100; i++)
            {
                MindFullProfile profile = new MindFullProfile();
                profile.MindDetails = new Mind()
                {
                    MID="M"+(1040000+i).ToString(),
                    Name="Sample Name "+i.ToString(),
                    BaseLocation=(i%2==0)?"Bangalore":"Pune",
                    Designation=(i%2==0)?"Architect":"Module Lead",
                    JoinedDateDD=(i%28)+1,
                    JoinedDateMM=(i%12)+1,
                    JoinedDateYYYY=(i%10)+2000
                };
                var contacts = new List<MindContact>();
                contacts.Add(new MindContact() { ContactText = "emailAddress"+i.ToString()+"@mindtree.com", MID = (1125000+i).ToString(), MindContactType = new Portable.Entities.ContactType() { ContactTypeId = (int)Entities.Enumerations.ContactType.WorkEmail } });
                contacts.Add(new MindContact() { ContactText = "987654"+(1000+i).ToString(), MID = (1125000+i).ToString(), MindContactType = new Portable.Entities.ContactType() { ContactTypeId = (int)Entities.Enumerations.ContactType.HomePhone } });
                profile.MindContacts = contacts;

                mindList.Add(profile);
            }
        }

        public static IList<MindFullProfile> Mindlist { get { return mindList; } }
    }
}
