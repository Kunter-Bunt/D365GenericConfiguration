using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mwo.GenericConfiguration.Samples
{
    public class CacheManager : ICache
    {
        private readonly string Name;

        public CacheManager(string name)
        {
            Name = name;
        }

        public void Set(string key, object value)
        {

        }

        public TOut Get<TOut>(string key)
        {
            return default;
        }

        public bool Has(string key)
        {
            return false;
        }

        public void Remove(string key)
        {

        }

        public void Clear()
        {

        }
    }
}
