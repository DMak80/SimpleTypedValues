using System;
using System.Collections.Generic;
using System.Threading;

namespace SimpleTypedValue.Utils;

public class StaticCache<TKey, TValue>
    where TKey : notnull
{
    private readonly object _lockObj = new();

    private CacheDictionary _cache = new();

    public TValue GetOrAdd(TKey type, Func<TKey, TValue> func)
    {
        return _cache.TryGetValue(type, out var value)
            ? value
            : CacheValue(type, func);
    }

    private TValue CacheValue(TKey type, Func<TKey, TValue> func)
    {
        lock (_lockObj)
        {
            var cache = Volatile.Read(ref _cache);
            if (cache.TryGetValue(type, out var info))
                return info;

            info = func(type);
            var newCache = cache.AddCache(type, info);
            Volatile.Write(ref _cache, newCache);
            return info;
        }
    }

    private class CacheDictionary : Dictionary<TKey, TValue>
    {
        public CacheDictionary()
        {
        }

        private CacheDictionary(CacheDictionary dic) : base(dic)
        {
        }

        public CacheDictionary AddCache(TKey key, TValue value)
        {
            var dic = new CacheDictionary(this);
            dic[key] = value;
            return dic;
        }
    }
}