using dxStudyDistributedRedisCache.Utility.Cache.Model;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using static dxStudyDistributedRedisCache.Utility.CommonUtility;

namespace dxStudyDistributedRedisCache.Utility.Cache.Service;

public class DistributedCacheHelper : IDistributedCacheHelper
{
    private readonly ILogger _logger;
    private readonly IDistributedCache _distCache;
    private readonly RedisConnectionSetting _redisConnectionSetting;

    public DistributedCacheHelper(ILogger<DistributedCacheHelper> logger, IDistributedCache distCache, IOptionsMonitor<RedisConnectionSetting> redisConnectionSetting)
    {
        _logger = logger;
        _distCache = distCache;
        _redisConnectionSetting = redisConnectionSetting.CurrentValue;
    }

    private TimeSpan? GetTimeSpan(double? timeSpan, TimeSpanType spanType)
    {
        if (timeSpan == null)
            return null;

        return CommonUtility.CalculateTimeSpan(timeSpan.Value, spanType);
    }

    public string GetString(string keyName)
    {
        if (string.IsNullOrWhiteSpace(keyName))
            return null;

        keyName = keyName.Trim();

        try
        {
            return _distCache.GetString(keyName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Get value by redis Key {keyName} failed.");
            return null;
        }
    }

    public async Task<string> GetStringAsync(string keyName)
    {
        if (string.IsNullOrWhiteSpace(keyName))
            return null;

        keyName = keyName.Trim();

        try
        {
            return await _distCache.GetStringAsync(keyName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Get value by redis Key {keyName} failed.");
            return null;
        }
    }

    public bool SetString(string keyName, string inputValue, double? timeSpan, TimeSpanType spanType = TimeSpanType.Second)
    {
        if (string.IsNullOrWhiteSpace(keyName) || string.IsNullOrWhiteSpace(inputValue))
            return false;

        keyName = keyName.Trim();
        inputValue = inputValue.Trim();

        DistributedCacheEntryOptions distCacheOptions = null;
        if (timeSpan != null)
        {
            distCacheOptions = new DistributedCacheEntryOptions();
            var cacheTimeSpan = GetTimeSpan(timeSpan, spanType);
            distCacheOptions.SetAbsoluteExpiration(cacheTimeSpan.Value);
        }

        try
        {
            if (distCacheOptions == null)
                _distCache.SetString(keyName, inputValue);
            else
                _distCache.SetString(keyName, inputValue, distCacheOptions);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Set value for redis Key {keyName} failed.");
            return false;
        }
    }

    public async Task<bool> SetStringAsync(string keyName, string inputValue, double? timeSpan, TimeSpanType spanType = TimeSpanType.Second)
    {
        if (string.IsNullOrWhiteSpace(keyName) || string.IsNullOrWhiteSpace(inputValue))
            return false;

        keyName = keyName.Trim();
        inputValue = inputValue.Trim();

        DistributedCacheEntryOptions distCacheOptions = null;
        if (timeSpan != null)
        {
            distCacheOptions = new DistributedCacheEntryOptions();
            var cacheTimeSpan = GetTimeSpan(timeSpan, spanType);
            distCacheOptions.SetAbsoluteExpiration(cacheTimeSpan.Value);
        }

        try
        {
            if (distCacheOptions == null)
                await _distCache.SetStringAsync(keyName, inputValue);
            else
                await _distCache.SetStringAsync(keyName, inputValue, distCacheOptions);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Set value for redis Key {keyName} failed.");
            return false;
        }
    }

    public bool SetString(string keyName, string inputValue, TimeSpan? timeSpan)
    {
        if (string.IsNullOrWhiteSpace(keyName) || string.IsNullOrWhiteSpace(inputValue))
            return false;

        keyName = keyName.Trim();
        inputValue = inputValue.Trim();

        DistributedCacheEntryOptions distCacheOptions = null;
        if (timeSpan != null)
        {
            distCacheOptions = new DistributedCacheEntryOptions();
            distCacheOptions.SetAbsoluteExpiration(timeSpan.Value);
        }

        try
        {
            if (distCacheOptions == null)
                _distCache.SetString(keyName, inputValue);
            else
                _distCache.SetString(keyName, inputValue, distCacheOptions);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Set value for redis Key {keyName} failed.");
            return false;
        }
    }

    public async Task<bool> SetStringAsync(string keyName, string inputValue, TimeSpan? timeSpan)
    {
        if (string.IsNullOrWhiteSpace(keyName) || string.IsNullOrWhiteSpace(inputValue))
            return false;

        keyName = keyName.Trim();
        inputValue = inputValue.Trim();

        DistributedCacheEntryOptions distCacheOptions = null;
        if (timeSpan != null)
        {
            distCacheOptions = new DistributedCacheEntryOptions();
            distCacheOptions.SetAbsoluteExpiration(timeSpan.Value);
        }

        try
        {
            if (distCacheOptions == null)
                await _distCache.SetStringAsync(keyName, inputValue);
            else
                await _distCache.SetStringAsync(keyName, inputValue, distCacheOptions);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Set value for redis Key {keyName} failed.");
            return false;
        }
    }

    public bool DeleteKey(string keyName)
    {
        if (string.IsNullOrWhiteSpace(keyName))
            return false;

        keyName = keyName.Trim();
        _logger.LogInformation($"Remove Key {keyName} from redis...");

        try
        {
            _distCache.Remove(keyName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Remove Key {keyName} from redis failed.");
            return false;
        }
    }

    public async Task<bool> DeleteKeyAsync(string keyName)
    {
        if (string.IsNullOrWhiteSpace(keyName))
            return false;

        keyName = keyName.Trim();
        _logger.LogInformation($"Remove Key {keyName} from redis...");

        try
        {
            await _distCache.RemoveAsync(keyName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Remove Key {keyName} from redis failed.");
            return false;
        }
    }

    public Tuple<bool, string> DeleteAllDistributedCache()
    {
        if (!_redisConnectionSetting.IsUsingRedis)
            return new Tuple<bool, string>(false, "Please recycle application pool or restart iis service to clear all distributed memory cache.");

        _logger.LogInformation("Remove all keys from redis...");

        var options = new ConfigurationOptions
        {
            EndPoints = { $"{_redisConnectionSetting.HostName}:{_redisConnectionSetting.PortNo}" },
            Password = _redisConnectionSetting.Password,
            AllowAdmin = _redisConnectionSetting.IsAllowAdmin,
            Ssl = _redisConnectionSetting.IsUsingSSL
        };

        try
        {
            using var redisConnection = ConnectionMultiplexer.Connect(options);
            var redisServer = redisConnection.GetServer(_redisConnectionSetting.HostName, _redisConnectionSetting.PortNo);
            redisServer.FlushDatabase();
            return new Tuple<bool, string>(true, string.Empty);
        }
        catch (Exception ex)
        {
            string strErrMessage = "Remove all keys from redis failed.";
            _logger.LogError(ex, strErrMessage);
            return new Tuple<bool, string>(false, $"{strErrMessage} Please check log for details.");
        }
    }
}
