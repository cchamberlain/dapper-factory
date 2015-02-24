using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Chambersoft.DapperFactory.Providers
{
    public class DefaultCacheProvider : ICacheProvider
    {
        private static ObjectCache Cache { get { return MemoryCache.Default; } }

        public object Get(string key)
        {
            return Cache[key];
        }

        public T GetWithRefresh<T>(string key, Func<T> refreshFunc, int cacheTime = 360) where T : class
        {
            var cached = Get(key) as T;
            if (cached == null)
            {
                cached = refreshFunc();
                if (cached != null)
                {
                    Set(key, cached, cacheTime);
                }
            }
            return cached;
        }

        public async Task<T> GetWithRefreshAsync<T>(string key, Func<Task<T>> refreshFuncAsync, int cacheTime = 360) where T : class
        {
            var cached = Get(key) as T;
            if (cached == null)
            {
                cached = await refreshFuncAsync();
                if (cached != null)
                {
                    Set(key, cached, cacheTime);
                }
            }
            return cached;
        }


        public void Set(string key, object data, int cacheTime)
        {
            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime) };
            Cache.Add(new CacheItem(key, data), policy);
        }

        public bool IsSet(string key)
        {
            return (Cache[key] != null);
        }

        public void Invalidate(string key)
        {
            Cache.Remove(key);
        }
    }
}
