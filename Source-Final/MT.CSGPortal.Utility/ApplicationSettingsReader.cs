using System;
using System.Configuration;

namespace MT.CSGPortal.Utility
{
    public static class ApplicationSettingsReader
    {
        public static int MaxPageResultsLucene { get { return int.Parse(ReadAppSetting("MaxPageResultsLucene", typeof(int)).ToString()); } }
        public static int PageSizeForLuceneSearch { get { return int.Parse(ReadAppSetting("PageSizeForLuceneSearch", typeof(int)).ToString()); } }
        public static int PageSizeForADSearchResult { get { return int.Parse(ReadAppSetting("PageSizeForADSearchResult", typeof(int)).ToString()); } }
        public static string LDAPServer { get { return ReadAppSetting("LDAPServer", typeof(string)).ToString(); } }
        public static string ADRootContainerPath { get {return ReadAppSetting("RootContainerPath", typeof(string)).ToString(); } }
        public static int IsADMocked { get { return int.Parse(ReadAppSetting("IsADMocked", typeof(int)).ToString()); } }

        private static object ReadAppSetting(string name,Type type)
        {
            AppSettingsReader reader = new AppSettingsReader();
            return reader.GetValue(name,type);
        }
    }
}
