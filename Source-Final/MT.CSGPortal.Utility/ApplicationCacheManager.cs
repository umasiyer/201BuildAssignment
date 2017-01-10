using System;
using System.Runtime.Caching;


namespace MT.CSGPortal.Utility
{
    public static class ApplicationCacheManager
    {
        private static ObjectCache cache = MemoryCache.Default;
        
        public static void AddItem(object objectToCache,string key,CacheItemPolicy policy)
        {   
            if(cache[key]==null)
                 cache.Add(key, objectToCache,policy);
        }

        public static T GetCacheItem<T>(string key) 
        {
             try
             {
                 return (T)cache[key];
             }
             catch (Exception ex)
             {
                 Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                 return default(T);
             }
        }
    }
}
