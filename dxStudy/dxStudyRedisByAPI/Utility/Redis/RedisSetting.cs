namespace dxStudyRedisByAPI;
public class RedisSetting
{
    public bool IsUsingRedis { get; set; }
    public string HostName { get; set; } = string.Empty;
    public int PortNo { get; set; }
    public string Password { get; set; } = string.Empty;
    public bool IsAllowAdmin { get; set; }
    public bool IsUsingSSL { get; set; }
    public string SSLCertPath { get; set; } = string.Empty;
    public string SSLCertPassword { get; set; } = string.Empty;
    public int CacheTimeSpan { get; set; }
    public bool IsOverrideOldRecord { get; set; }
}
