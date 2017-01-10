#region Using Statements
using MT.CSGPortal.Entities;
using MT.CSGPortal.Portable.Entities;
using MT.CSGPortal.Utility;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Web.Hosting;

#endregion Using Statements

#region MT.CSGPortal.BL
namespace MT.CSGPortal.BL
{
    #region MindADManager
    /// <summary>
    /// Business Manager class for AD related tasks of Minds
    /// </summary>
    public class ActiveDirectoryManager : IActiveDirectoryManager
    {
        #region Properties and Constants
        /// <summary>
        /// LDAP Server details: Get this value from Config
        /// </summary>
        private string ldapServer = ApplicationSettingsReader.LDAPServer;

        /// <summary>
        /// LDAP Server Root container path: get this value from config
        /// </summary>
        private string rootContainerPath = ApplicationSettingsReader.ADRootContainerPath;
        #endregion Properties and Constants

        #region Public Methods

        #region GetUser
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mId"></param>
        /// <returns></returns>
        public object GetUser(string mId)
        {
            UserPrincipal userData = null;
            using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, ldapServer, rootContainerPath))
            {
                using (UserPrincipal currentUser = UserPrincipal.FindByIdentity(ctx, mId))
                {
                    if (currentUser != null && currentUser.Enabled.Value)
                    {
                        userData = currentUser;
                    }
                }
            }
            return userData;
        }
        #endregion GetUser


        #region ValidateUser
        /// <summary>
        /// Validate the sent credentials for against a user in AD
        /// </summary>
        /// <param name="mId"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public bool ValidateUser(string mId, string password, string domain)
        {
            using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, ldapServer, rootContainerPath))
            {
                return ctx.ValidateCredentials(mId, password);
            }

        }
        #endregion ValidateUser

        /// <summary>
        /// Searches AD based on MID or Name
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns>List of Minds which matching Name or MID</returns>
        public List<Mind> Search(string searchTerm)
        {
            List<Mind> results = null;
            using (HostingEnvironment.Impersonate())
            {
                using (DirectoryEntry entry = new DirectoryEntry(string.Format(CultureInfo.InvariantCulture, "LDAP://{0}", ldapServer.Replace("/", "\\/"))))
                {
                    DirectorySearcher searcher = new DirectorySearcher(entry);
                    {
                        if (ApplicationCacheManager.GetCacheItem<DirectorySearcher>("ADSearcher") == default(DirectorySearcher))
                        {
                            searcher.PropertiesToLoad.Add("cn");
                            searcher.PropertiesToLoad.Add("description");
                            searcher.PropertiesToLoad.Add("l");
                            searcher.PropertiesToLoad.Add("objectCategory");
                            searcher.PropertiesToLoad.Add("objectClass");
                            searcher.PropertiesToLoad.Add("samaccountname");
                            searcher.PropertiesToLoad.Add("title");
                            searcher.PropertiesToLoad.Add("employeeNumber");
                            searcher.Sort = new SortOption("cn", SortDirection.Ascending);
                            CacheItemPolicy policy = new CacheItemPolicy() { Priority = CacheItemPriority.NotRemovable };
                            ApplicationCacheManager.AddItem(searcher, "ADSearcher", policy);
                        }
                        else
                        {
                            searcher = ApplicationCacheManager.GetCacheItem<DirectorySearcher>("ADSearcher");
                        }
                        searcher.Filter = "(&(objectClass=user)(objectCategory=user)(|(cn=*" + searchTerm + "*)(samaccountname=*" + searchTerm + "*)))";
                        using (SearchResultCollection matches = searcher.FindAll())
                        {
                            results = SearchResultsToBasicProfileList(matches);
                        }
                    }
                    searcher.Dispose();
                }
            }
            return results;
        }

        /// <summary>
        /// Searches AD and returns results corresponding to Page Number
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="pageNumber"></param>
        /// <returns>SearchResultDTO</returns>
        public SearchResult<MindBasicProfile> Search(string searchTerm, byte pageNumber)
        {
            //Get PageSize from Configuration 
            int pageSize = ApplicationSettingsReader.PageSizeForADSearchResult;

            //Get Minds from AD
            SearchResult<MindBasicProfile> result = new SearchResult<MindBasicProfile>();
            List<Mind> minds = Search(searchTerm);
            result.ResultData = new List<MindBasicProfile>();
            //result.ResultData = minds.Select(m => new MindBasicProfile(m)).ToList<MindBasicProfile>();

            //Filtering records based on Page Number.
            //int lowerindex = (pageNumber - 1) * pageSize;
            //int upperindex = pageNumber + pageSize;

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

        /// <summary>
        /// Fetches mind from AD based on MID
        /// </summary>
        /// <param name="mid"></param>
        /// <returns>Full Profile of Mind including contact information</returns>
        public MindFullProfile GetMindFullProfileById(string mid)
        {
            MindFullProfile profile = new MindFullProfile();
            using (HostingEnvironment.Impersonate())
            {
                using (DirectoryEntry entry = new DirectoryEntry(string.Format(CultureInfo.InvariantCulture, "LDAP://{0}", ldapServer.Replace("/", "\\/"))))
                {
                    using (DirectorySearcher searcher = CreateFullProfileSearcher(entry, mid))
                    {
                        SearchResult result = searcher.FindOne();
                        if (result == null)
                            profile = null;
                        else
                        {
                            ResultPropertyCollection resultPropCollection = result.Properties;
                            profile = SeartchResultPropertyCollectionToFullProfile(resultPropCollection);
                        }
                    }
                }
            }
            return profile;
        }

        /// <summary>
        /// Retrieve the image from AD
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Image in Byte</returns>
        public Byte[] GetImage(string id)
        {
            try
            {
                LdapConnection ldapConnection = new LdapConnection(ldapServer);
                ldapConnection.Credential = CredentialCache.DefaultNetworkCredentials;
                using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "mindtree"))
                {
                    UserPrincipal currentUser = UserPrincipal.FindByIdentity(ctx, id);
                    if (currentUser != null && currentUser.Enabled.Value)
                    {
                        byte[] thumbnailPhoto = (byte[])(currentUser.GetUnderlyingObject() as DirectoryEntry).Properties["thumbnailPhoto"].Value;
                        return thumbnailPhoto;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }
        #endregion Public Methods

        private static DirectorySearcher CreateFullProfileSearcher(DirectoryEntry entry, string mid)
        {
            DirectorySearcher searcher = new DirectorySearcher(entry);
            searcher.Filter = "(&(samAccountType=805306368)(samaccountname=*" + mid + "*))";
            searcher.PropertiesToLoad.Add("cn");
            searcher.PropertiesToLoad.Add("description");
            searcher.PropertiesToLoad.Add("extensionAttribute1");
            searcher.PropertiesToLoad.Add("homePhone");
            searcher.PropertiesToLoad.Add("l");
            searcher.PropertiesToLoad.Add("mail");
            searcher.PropertiesToLoad.Add("mailNickname");
            searcher.PropertiesToLoad.Add("mobile");
            searcher.PropertiesToLoad.Add("name");
            searcher.PropertiesToLoad.Add("objectCategory");
            searcher.PropertiesToLoad.Add("objectClass");
            searcher.PropertiesToLoad.Add("samaccountname");
            searcher.PropertiesToLoad.Add("telephoneNumber");
            searcher.PropertiesToLoad.Add("thumbnailPhoto");
            searcher.PropertiesToLoad.Add("title");
            searcher.PropertiesToLoad.Add("employeeNumber");
            searcher.PropertiesToLoad.Add("homePhone");
            return searcher;
        }

        private static List<Mind> SearchResultsToBasicProfileList(SearchResultCollection matches)
        {
            List<Mind> results = new List<Mind>();
            foreach (SearchResult item in matches)
            {
                ResultPropertyCollection resultPropCollection = item.Properties;
                Mind m = new Mind();
                if (resultPropCollection.Contains("l"))
                {
                    m.BaseLocation = resultPropCollection["l"][0].ToString();
                }
                if (resultPropCollection.Contains("title"))
                {
                    m.Designation = resultPropCollection["title"][0].ToString();
                }
                if (resultPropCollection.Contains("samaccountname"))
                {
                    m.MID = resultPropCollection["samaccountname"][0].ToString();
                }
                if (resultPropCollection.Contains("cn"))
                {
                    m.Name = resultPropCollection["cn"][0].ToString();
                }
                if (resultPropCollection.Contains("employeeNumber"))
                {
                    string date = resultPropCollection["employeeNumber"][0].ToString();
                    m.JoinedDateDD = int.Parse(date.Substring(0, 2));
                    m.JoinedDateMM = int.Parse(date.Substring(3, 2));
                    m.JoinedDateYYYY = int.Parse(date.Substring(6, 4));
                }
                results.Add(m);
            }
            return results;
        }

        private static MindFullProfile SeartchResultPropertyCollectionToFullProfile(ResultPropertyCollection resultPropCollection)
        {
            Mind mindObj = new Mind();
            List<MindContact> contacts = new List<MindContact>();
            if (resultPropCollection.Contains("cn"))
                mindObj.Name = resultPropCollection["cn"][0].ToString();
            if (resultPropCollection.Contains("l"))
                mindObj.BaseLocation = resultPropCollection["l"][0].ToString();
            if (resultPropCollection.Contains("title"))
                mindObj.Designation = resultPropCollection["title"][0].ToString();
            if (resultPropCollection.Contains("samaccountname"))
                mindObj.MID = resultPropCollection["samaccountname"][0].ToString();

            if (resultPropCollection.Contains("mail"))
            {
                string contactText = resultPropCollection["mail"][0].ToString();
                contacts.Add(new MindContact() { ContactText = contactText, MID = mindObj.MID, MindContactType = new Portable.Entities.ContactType() { ContactTypeId = (int)Entities.Enumerations.ContactType.WorkEmail } });
            }

            if (resultPropCollection.Contains("telephoneNumber"))
            {
                string contactText = resultPropCollection["telephoneNumber"][0].ToString();
                contacts.Add(new MindContact() { ContactText = contactText, MID = mindObj.MID, MindContactType = new Portable.Entities.ContactType() { ContactTypeId = (int)Entities.Enumerations.ContactType.DeskPhone } });
            }

            if (resultPropCollection.Contains("mobile"))
            {
                string contactText = resultPropCollection["mobile"][0].ToString();
                contacts.Add(new MindContact() { ContactText = contactText, MID = mindObj.MID, MindContactType = new Portable.Entities.ContactType() { ContactTypeId = (int)Entities.Enumerations.ContactType.MobilePhone } });
            }

            if (resultPropCollection.Contains("homePhone"))
            {
                string contactText = resultPropCollection["homePhone"][0].ToString();
                contacts.Add(new MindContact() { ContactText = contactText, MID = mindObj.MID, MindContactType = new Portable.Entities.ContactType() { ContactTypeId = (int)Entities.Enumerations.ContactType.HomePhone } });
            }

            if (resultPropCollection.Contains("employeeNumber"))
            {
                string date = resultPropCollection["employeeNumber"][0].ToString();
                mindObj.JoinedDateDD = int.Parse(date.Substring(0, 2));
                mindObj.JoinedDateMM = int.Parse(date.Substring(3, 2));
                mindObj.JoinedDateYYYY = int.Parse(date.Substring(6, 4));
            }

            return new MindFullProfile()
            {
                MindDetails = mindObj,
                MindContacts = contacts
            };
        }
    }

    #endregion MindADManager
}
#endregion MT.CSGPortal.BL
