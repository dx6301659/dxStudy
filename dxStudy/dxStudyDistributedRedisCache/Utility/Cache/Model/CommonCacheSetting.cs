namespace dxStudyDistributedRedisCache.Utility.Cache.Model;

public class CommonCacheSetting
{
    public string CacheKeyPrifix { get; set; } = string.Empty;
    public int? DefaultCacheTime { get; set; }
    public string CacheKeyDelimeter { get; set; } = string.Empty;
    public int? CacheTimeForConfig { get; set; }
    public string CacheKeyNameForConfig { get; set; } = string.Empty;
    public int? CacheTimeForToken { get; set; }
    public string CacheKeyNameForToken { get; set; } = string.Empty;
    public int? CacheTimeForAccount { get; set; }
    public string CacheKeyNameForAccount { get; set; } = string.Empty;
}
