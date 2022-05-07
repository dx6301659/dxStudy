using static dxStudyDistributedRedisCache.Utility.CommonUtility;

namespace dxStudyDistributedRedisCache.Utility.Cache.Service;

public interface IDistributedCacheHelper
{
    string GetString(string keyName);
    Task<string> GetStringAsync(string keyName);
    bool SetString(string keyName, string inputValue, double? timeSpan, TimeSpanType spanType = TimeSpanType.Second);
    Task<bool> SetStringAsync(string keyName, string inputValue, double? timeSpan, TimeSpanType spanType = TimeSpanType.Second);
    bool SetString(string keyName, string inputValue, TimeSpan? timeSpan);
    Task<bool> SetStringAsync(string keyName, string inputValue, TimeSpan? timeSpan);
    bool DeleteKey(string keyName);
    Task<bool> DeleteKeyAsync(string keyName);
    Tuple<bool, string> DeleteAllDistributedCache();
}
