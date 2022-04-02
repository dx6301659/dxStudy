using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Security.Cryptography.X509Certificates;

namespace dxStudyRedisByAPI;
public class RedisHelper : IRedisHelper
{
    private readonly ILogger _logger;
    private readonly RedisSetting _redisSetting;

    public RedisHelper(ILogger<RedisHelper> logger, IOptionsMonitor<RedisSetting> redisSetting)
    {
        _logger = logger;
        _redisSetting = redisSetting.CurrentValue;

        if (!_redisSetting.IsUsingRedis)
            logger.LogInformation("Redis functiion is not using......");
    }

    private ConnectionMultiplexer GetConnectionMultiplexer()
    {
        var options = new ConfigurationOptions
        {
            EndPoints = { $"{_redisSetting.HostName}:{_redisSetting.PortNo}" },
            Password = _redisSetting.Password,
            AllowAdmin = _redisSetting.IsAllowAdmin
        };

        if (_redisSetting.IsUsingSSL)
        {
            try
            {
                options.Ssl = true;
                options.CertificateSelection += delegate
                {
                    return new X509Certificate2(_redisSetting.SSLCertPath, _redisSetting.SSLCertPassword);
                };
            }
            catch
            {
                _logger.LogError("Redis certification load failed.");
                return null;
            }
        }

        try
        {
            return ConnectionMultiplexer.Connect(options);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Redis server connect failed.");
            return null;
        }
    }

    private IDatabase GetRedisConnection()
    {
        var muxer = GetConnectionMultiplexer();
        if (muxer == null)
            return null;

        try
        {
            return muxer.GetDatabase();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Redis server connect failed.");
            return null;
        }
    }

    private bool CloseRedisConnection(ConnectionMultiplexer muxer)
    {
        if (muxer == null)
            return true;

        if (!muxer.IsConnected)
            return true;

        try
        {
            muxer.Close();
            muxer.Dispose();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Close Redis server connection failed.");
            return false;
        }
    }

    private bool CloseRedisConnection(IDatabase redisConnection)
    {
        if (redisConnection == null || redisConnection.Multiplexer == null)
            return true;

        var muxer = redisConnection.Multiplexer;
        if (!muxer.IsConnected)
            return true;

        try
        {
            muxer.Close();
            muxer.Dispose();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Close Redis server connection failed.");
            return false;
        }
    }

    public async Task<bool> IsExistKeyInRedisAsync(string keyName)
    {
        if (!_redisSetting.IsUsingRedis)
            return false;

        if (string.IsNullOrWhiteSpace(keyName))
            return false;

        var redisConnection = GetRedisConnection();
        if (redisConnection == null)
            return false;

        keyName = keyName.Trim();

        try
        {
            return await redisConnection.KeyExistsAsync(keyName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Check if Key {keyName} exist in redis failed.");
            return false;
        }
        finally
        {
            CloseRedisConnection(redisConnection);
        }
    }

    public async Task<string> GetStringAsync(string keyName)
    {
        if (!_redisSetting.IsUsingRedis)
            return null;

        if (string.IsNullOrWhiteSpace(keyName))
            return null;

        var redisConnection = GetRedisConnection();
        if (redisConnection == null)
            return null;

        keyName = keyName.Trim();

        try
        {
            return await redisConnection.StringGetAsync(keyName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Get value by redis Key {keyName} failed.");
            return null;
        }
        finally
        {
            CloseRedisConnection(redisConnection);
        }
    }

    public async Task<IEnumerable<string>> GetStringAsync(IEnumerable<string> arrKeyName)
    {
        if (!_redisSetting.IsUsingRedis)
            return null;

        if (arrKeyName == null || !arrKeyName.Any())
            return null;

        arrKeyName = arrKeyName.Where(item => !string.IsNullOrWhiteSpace(item)).Select(item => item.Trim()).Distinct();
        if (arrKeyName == null || !arrKeyName.Any())
            return null;

        var redisConnection = GetRedisConnection();
        if (redisConnection == null)
            return null;

        var arrKeys = arrKeyName.Select(item => new RedisKey(item)).ToArray();

        try
        {
            var arrValue = await redisConnection.StringGetAsync(arrKeys);
            return arrValue.ToStringArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Get values by redis Key {string.Join(",", arrKeyName)} failed.");
            return null;
        }
        finally
        {
            CloseRedisConnection(redisConnection);
        }
    }

    public async Task<bool> SetStringAsync(string keyName, string inputValue, double? timeSpan = null, TimeSpanType spanType = TimeSpanType.Second)
    {
        if (!_redisSetting.IsUsingRedis)
            return false;

        if (string.IsNullOrWhiteSpace(keyName) || string.IsNullOrWhiteSpace(inputValue))
            return false;

        var redisConnection = GetRedisConnection();
        if (redisConnection == null)
            return false;

        keyName = keyName.Trim();
        inputValue = inputValue.Trim();

        var cacheTimeSpan = TimeSpan.FromSeconds(_redisSetting.CacheTimeSpan);
        if (timeSpan != null)
            cacheTimeSpan = CommonUtility.CalculateTimeSpan(timeSpan.Value, spanType);

        try
        {
            var operationType = _redisSetting.IsOverrideOldRecord ? When.Always : When.NotExists;
            await redisConnection.StringSetAsync(keyName, inputValue, cacheTimeSpan, operationType);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Set value for redis Key {keyName} failed.");
            return false;
        }
        finally
        {
            CloseRedisConnection(redisConnection);
        }
    }

    public async Task<bool> SetStringAsync(Dictionary<string, string> dicKeyValuePair)
    {
        if (!_redisSetting.IsUsingRedis)
            return false;

        if (dicKeyValuePair == null || dicKeyValuePair.Count == 0)
            return false;

        var arrDicKeyValuePair = dicKeyValuePair.Where(item => (!string.IsNullOrWhiteSpace(item.Key)) && (!string.IsNullOrWhiteSpace(item.Value)));
        if (arrDicKeyValuePair == null || !arrDicKeyValuePair.Any())
            return false;

        var redisConnection = GetRedisConnection();
        if (redisConnection == null)
            return false;

        var arrKeyValuePair = arrDicKeyValuePair.Select(item => KeyValuePair.Create(new RedisKey(item.Key.Trim()), new RedisValue(item.Value.Trim()))).ToArray();

        try
        {
            var operationType = _redisSetting.IsOverrideOldRecord ? When.Always : When.NotExists;
            await redisConnection.StringSetAsync(arrKeyValuePair, operationType);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Set values for redis Key {string.Join(",", dicKeyValuePair.Keys)} failed.");
            return false;
        }
        finally
        {
            CloseRedisConnection(redisConnection);
        }
    }

    public async Task<bool> DeleteKeyAsync(string keyName)
    {
        if (!_redisSetting.IsUsingRedis)
            return false;

        if (string.IsNullOrWhiteSpace(keyName))
            return false;

        var redisConnection = GetRedisConnection();
        if (redisConnection == null)
            return false;

        keyName = keyName.Trim();
        _logger.LogInformation($"Remove Key {keyName} from redis...");

        try
        {
            await redisConnection.KeyDeleteAsync(keyName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Remove Key {keyName} from redis failed.");
            return false;
        }
        finally
        {
            CloseRedisConnection(redisConnection);
        }
    }

    public async Task<bool> DeleteKeyAsync(IEnumerable<string> arrKeyName)
    {
        if (!_redisSetting.IsUsingRedis)
            return false;

        if (arrKeyName == null || !arrKeyName.Any())
            return false;

        arrKeyName = arrKeyName.Where(item => !string.IsNullOrWhiteSpace(item)).Select(item => item.Trim()).Distinct();
        if (arrKeyName == null || !arrKeyName.Any())
            return false;

        var redisConnection = GetRedisConnection();
        if (redisConnection == null)
            return false;

        string strArrKeys = string.Join(",", arrKeyName);
        var arrKeys = arrKeyName.Select(item => new RedisKey(item)).ToArray();
        _logger.LogInformation($"Remove Key {strArrKeys} from redis...");

        try
        {
            await redisConnection.KeyDeleteAsync(arrKeys);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Remove Key {strArrKeys} from redis failed.");
            return false;
        }
        finally
        {
            CloseRedisConnection(redisConnection);
        }
    }

    public async Task<bool> DeleteAllKeysAsync()
    {
        if (!_redisSetting.IsUsingRedis)
            return false;

        var muxer = GetConnectionMultiplexer();
        if (muxer == null)
            return false;

        _logger.LogInformation("Remove all keys from redis...");

        try
        {
            var objServer = muxer.GetServer(_redisSetting.HostName, _redisSetting.PortNo);
            if (objServer == null)
                return false;

            await objServer.FlushDatabaseAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Remove all keys from redis failed.");
            return false;
        }
        finally
        {
            CloseRedisConnection(muxer);
        }
    }
}
