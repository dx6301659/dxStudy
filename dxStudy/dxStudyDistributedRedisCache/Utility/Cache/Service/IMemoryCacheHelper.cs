using Microsoft.Extensions.Caching.Memory;
using static dxStudyDistributedRedisCache.Utility.CommonUtility;

namespace dxStudyDistributedRedisCache.Utility.Cache.Service;

public interface IMemoryCacheHelper
{
    bool IsExistKeyInMemoryCache<T>(string keyName, out T cacheValue);
    T GetMemoryCache<T>(string keyName);
    bool SetMemoryCache<T>(string keyName, T inputValue, MemoryCacheEntryOptions cacheOptions, bool isOverrideOldRecord = false);
    bool SetMemoryCache<T>(string keyName, T inputValue, double? timeSpan, TimeSpanType spanType = TimeSpanType.Second, bool isOverrideOldRecord = false);
    bool SetMemoryCache<T>(string keyName, T inputValue, TimeSpan timeSpan, bool isOverrideOldRecord = false);
    bool SetMemoryCacheConcurrent<T>(string keyName, T inputValue, MemoryCacheEntryOptions cacheOptions, bool isOverrideOldRecord = false);
    bool SetMemoryCacheConcurrent<T>(string keyName, T inputValue, double? timeSpan, TimeSpanType spanType = TimeSpanType.Second, bool isOverrideOldRecord = false);
    bool SetMemoryCacheConcurrent<T>(string keyName, T inputValue, TimeSpan timeSpan, bool isOverrideOldRecord = false);
    bool DeleteMemoryCache(string keyName);
    bool DeleteAllMemoryCache();
}
