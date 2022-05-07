using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using System.Reflection;
using static dxStudyDistributedRedisCache.Utility.CommonUtility;

namespace dxStudyDistributedRedisCache.Utility.Cache.Service;

public class MemoryCacheHelper : IMemoryCacheHelper
{
    private static object staticObj = new object();
    private readonly ILogger _logger;
    private readonly IMemoryCache _memoryCache;

    private MemoryCacheEntryOptions GetMemoryCacheOption(double? timeSpan, TimeSpanType spanType)
    {
        var memoryCacheOptions = new MemoryCacheEntryOptions();
        if (timeSpan == null)
        {
            memoryCacheOptions.SetPriority(CacheItemPriority.NeverRemove);
            return memoryCacheOptions;
        }

        memoryCacheOptions.SetPriority(CacheItemPriority.High);
        var cacheTimeSpan = CommonUtility.CalculateTimeSpan(timeSpan.Value, spanType);
        memoryCacheOptions.SetAbsoluteExpiration(cacheTimeSpan);
        return memoryCacheOptions;
    }

    public MemoryCacheHelper(ILogger<MemoryCacheHelper> logger, IMemoryCache memoryCache)
    {
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public bool IsExistKeyInMemoryCache<T>(string keyName, out T cacheValue)
    {
        cacheValue = default(T);

        if (string.IsNullOrWhiteSpace(keyName))
            return false;

        return _memoryCache.TryGetValue<T>(keyName.Trim(), out cacheValue);
    }

    public T GetMemoryCache<T>(string keyName)
    {
        if (string.IsNullOrWhiteSpace(keyName))
            return default(T);

        return _memoryCache.Get<T>(keyName.Trim());
    }

    public bool SetMemoryCache<T>(string keyName, T inputValue, MemoryCacheEntryOptions cacheOptions, bool isOverrideOldRecord = false)
    {
        if (string.IsNullOrWhiteSpace(keyName))
            return false;

        keyName = keyName.Trim();

        if (isOverrideOldRecord)
        {
            _memoryCache.Set(keyName, inputValue, cacheOptions);
            return true;
        }

        T cacheValue = default(T);
        bool blnExist = _memoryCache.TryGetValue<T>(keyName, out cacheValue);
        if (!blnExist)
            _memoryCache.Set(keyName, inputValue, cacheOptions);

        return true;
    }

    public bool SetMemoryCache<T>(string keyName, T inputValue, double? timeSpan, TimeSpanType spanType = TimeSpanType.Second, bool isOverrideOldRecord = false)
    {
        var memoryCacheOption = GetMemoryCacheOption(timeSpan, spanType);
        return SetMemoryCache(keyName, inputValue, memoryCacheOption, isOverrideOldRecord);
    }

    public bool SetMemoryCache<T>(string keyName, T inputValue, TimeSpan timeSpan, bool isOverrideOldRecord = false)
    {
        if (string.IsNullOrWhiteSpace(keyName))
            return false;

        keyName = keyName.Trim();

        if (isOverrideOldRecord)
        {
            _memoryCache.Set(keyName, inputValue, timeSpan);
            return true;
        }

        T cacheValue = default(T);
        bool blnExist = _memoryCache.TryGetValue<T>(keyName, out cacheValue);
        if (!blnExist)
            _memoryCache.Set(keyName, inputValue, timeSpan);

        return true;
    }

    public bool SetMemoryCacheConcurrent<T>(string keyName, T inputValue, MemoryCacheEntryOptions cacheOptions, bool isOverrideOldRecord = false)
    {
        if (string.IsNullOrWhiteSpace(keyName))
            return false;

        keyName = keyName.Trim();
        if (isOverrideOldRecord)
        {
            _memoryCache.Set(keyName, inputValue, cacheOptions);
            return true;
        }

        T cacheValue = default(T);
        bool blnExist = _memoryCache.TryGetValue<T>(keyName, out cacheValue);
        if (!blnExist)
        {
            lock (staticObj)
            {
                blnExist = _memoryCache.TryGetValue<T>(keyName, out cacheValue);
                if (!blnExist)
                    _memoryCache.Set(keyName, inputValue, cacheOptions);
            }
        }

        return true;
    }

    public bool SetMemoryCacheConcurrent<T>(string keyName, T inputValue, double? timeSpan, TimeSpanType spanType = TimeSpanType.Second, bool isOverrideOldRecord = false)
    {
        var memoryCacheOption = GetMemoryCacheOption(timeSpan, spanType);
        return SetMemoryCacheConcurrent(keyName, inputValue, memoryCacheOption, isOverrideOldRecord);
    }

    public bool SetMemoryCacheConcurrent<T>(string keyName, T inputValue, TimeSpan timeSpan, bool isOverrideOldRecord = false)
    {
        if (string.IsNullOrWhiteSpace(keyName))
            return false;

        keyName = keyName.Trim();
        if (isOverrideOldRecord)
        {
            _memoryCache.Set(keyName, inputValue, timeSpan);
            return true;
        }

        T cacheValue = default(T);
        bool blnExist = _memoryCache.TryGetValue<T>(keyName, out cacheValue);
        if (!blnExist)
        {
            lock (staticObj)
            {
                blnExist = _memoryCache.TryGetValue<T>(keyName, out cacheValue);
                if (!blnExist)
                    _memoryCache.Set(keyName, inputValue, timeSpan);
            }
        }

        return true;
    }

    public bool DeleteMemoryCache(string keyName)
    {
        if (string.IsNullOrWhiteSpace(keyName))
            return false;

        _logger.LogInformation($"Remove Key {keyName} from memory cache...");
        _memoryCache.Remove(keyName.Trim());
        return true;
    }

    public bool DeleteAllMemoryCache()
    {
        var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
        var entries = _memoryCache.GetType().GetField("_entries", bindingFlags)?.GetValue(_memoryCache);
        if (entries == null)
            return false;

        var dicCacheItems = entries as IDictionary;
        if (dicCacheItems == null)
            return false;

        _logger.LogInformation($"Remove all keys from memory cache...");
        foreach (var item in dicCacheItems.Keys)
        {
            _memoryCache.Remove(item);
        }

        return true;
    }
}
