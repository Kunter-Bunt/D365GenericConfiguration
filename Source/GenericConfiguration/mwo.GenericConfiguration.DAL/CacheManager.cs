using System;
using System.Linq;
using System.Runtime.Caching;

namespace mwo.GenericConfiguration.DAL
{
    public class CacheManager : ICache
    {
        private readonly string Name;
        private readonly MemoryCache Cache;
        private readonly TimeSpan CachingDuration;

        public CacheManager(string name, TimeSpan cachingDuration)
        {
            Name = name;
            Cache = MemoryCache.Default;
            CachingDuration = cachingDuration;
        }

        public void Set(string key, object value)
        {
            Set(key, value, CachingDuration);
        }

        public void Set(string key, object value, TimeSpan expiration)
        {
            Cache.Set(CreateIdentifier(key), value, CreatePolicy(expiration));
        }

        private CacheItemPolicy CreatePolicy(TimeSpan expiration)
        {
            return new CacheItemPolicy { SlidingExpiration = expiration };
        }

        public TOut Get<TOut>(string key)
        {
            if (Has(key))
            {
                var value = Cache.Get(CreateIdentifier(key));
                if (value is TOut) return (TOut)value;
            }
            return default;
        }

        public bool Has(string key)
        {
            return Cache.Contains(CreateIdentifier(key));
        }

        public void Remove(string key)
        {
            if (Has(key)) Cache.Remove(CreateIdentifier(key));
        }

        public void Clear()
        {
            Cache.Where(_ => _.Key.StartsWith(CreateIdentifier(string.Empty))).ToList()
                .ForEach(_ => Remove(_.Key.Substring(_.Key.IndexOf('_') + 1)));
        }

        private string CreateIdentifier(string key)
        {
            return string.Join("_", Name, key);
        }
    }
}
