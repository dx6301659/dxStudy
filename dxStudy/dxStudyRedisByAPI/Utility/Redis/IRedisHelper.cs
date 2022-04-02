namespace dxStudyRedisByAPI;
public interface IRedisHelper
{
    Task<bool> IsExistKeyInRedisAsync(string keyName);
    Task<string> GetStringAsync(string keyName);
    Task<IEnumerable<string>> GetStringAsync(IEnumerable<string> arrKeyName);
    Task<bool> SetStringAsync(string keyName, string inputValue, double? timeSpan = null, TimeSpanType spanType = TimeSpanType.Second);
    Task<bool> SetStringAsync(Dictionary<string, string> dicKeyValuePair);
    Task<bool> DeleteKeyAsync(string keyName);
    Task<bool> DeleteKeyAsync(IEnumerable<string> arrKeyName);
    Task<bool> DeleteAllKeysAsync();
}
