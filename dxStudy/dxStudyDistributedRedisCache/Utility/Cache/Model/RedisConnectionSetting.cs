namespace dxStudyDistributedRedisCache.Utility.Cache.Model;

public class RedisConnectionSetting
{
    public bool IsUsingRedis { get; set; }
    public string HostName { get; set; } = string.Empty;
    public int PortNo { get; set; }
    public string Password { get; set; } = string.Empty;
    public string InstanceName { get; set; } = string.Empty;
    public bool IsAllowAdmin { get; set; }
    public bool IsUsingSSL { get; set; }
}
