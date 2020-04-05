using System;

namespace mwo.GenericConfiguration.Samples
{
    public interface ICache
    {
        void Clear();
        TOut Get<TOut>(string key);
        bool Has(string key);
        void Remove(string key);
        void Set(string key, object value);
        void Set(string key, object value, TimeSpan expiration);
    }
}