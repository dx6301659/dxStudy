using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using dxStudyDistributedRedisCache.Utility.Cache.Service;
using dxStudyDistributedRedisCache.Utility.Cache.Model;

namespace dxStudyDistributedRedisCache.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CacheController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IDistributedCacheHelper _distributedCacheHelper;
    private readonly IMemoryCacheHelper _memoryCacheHelper;
    private readonly CommonCacheSetting _commonCacheSetting;

    public CacheController(ILogger<CacheController> logger, IDistributedCacheHelper distributedCacheHelper, IMemoryCacheHelper memoryCacheHelper,
                           IOptionsMonitor<CommonCacheSetting> commonCacheSetting)
    {
        _logger = logger;
        _distributedCacheHelper = distributedCacheHelper;
        _memoryCacheHelper = memoryCacheHelper;
        _commonCacheSetting = commonCacheSetting.CurrentValue;
    }

    [HttpPost("/SetDistributedCache")]
    public async Task<ActionResult> SetDistributedCache(string keyName, string inputValue, double? secondExpirTime)
    {
        if (string.IsNullOrWhiteSpace(keyName))
        {
            var objResult = new { Status = "Key is empty." };
            return Ok(objResult);
            //return new JsonResult(objResult);
        }

        if (secondExpirTime == null)
            secondExpirTime = _commonCacheSetting.DefaultCacheTime;

        bool resVal = await _distributedCacheHelper.SetStringAsync(keyName.Trim(), inputValue, secondExpirTime);
        _logger.LogInformation($"Set redis cache result: {resVal}");
        var res = new { Status = resVal };
        return Ok(res);
        //return new JsonResult(res);
    }

    [HttpGet("/GetDistributedCacheByKey")]
    public async Task<ActionResult> GetDistributedCacheByKey(string keyName)
    {
        if (string.IsNullOrWhiteSpace(keyName))
        {
            var objResult = new { Status = "Key is empty." };
            return Ok(objResult);
            //return new JsonResult(objResult);
        }

        string distCached = await _distributedCacheHelper.GetStringAsync(keyName.Trim());
        var res = new { Status = distCached };
        return Ok(res);
        //return new JsonResult(res);
    }

    [HttpGet("/DeleteDistributedCacheByKey")]
    public async Task<ActionResult> DeleteDistributedCacheByKey(string keyName)
    {
        if (string.IsNullOrWhiteSpace(keyName))
        {
            var objResult = new { Status = "Key is empty." };
            return Ok(objResult);
            //return new JsonResult(objResult);
        }

        bool blnResult = await _distributedCacheHelper.DeleteKeyAsync(keyName.Trim());
        var res = new { Status = blnResult ? "success" : "failed" };
        return Ok(res);
        //return new JsonResult(res);
    }

    [HttpPost("/SetMemoryCache")]
    public ActionResult SetMemoryCache(string keyName, string inputValue, double? secondExpirTime)
    {
        if (string.IsNullOrWhiteSpace(keyName))
        {
            var objResult = new { Status = "Key is empty." };
            return Ok(objResult);
            //return new JsonResult(objResult);
        }

        keyName = keyName.Trim();
        bool blnResult = _memoryCacheHelper.SetMemoryCache(keyName, inputValue, secondExpirTime);
        var res = new { Status = blnResult ? "success" : "failed" };
        return Ok(res);
        //return new JsonResult(res);
    }

    [HttpGet("/GetMemoryCacheByKey")]
    public ActionResult GetMemoryCacheByKey(string keyName)
    {
        if (string.IsNullOrWhiteSpace(keyName))
        {
            var objResult = new { Status = "Key is empty." };
            return Ok(objResult);
            //return new JsonResult(objResult);
        }

        object objResultTemp = null;
        if (_memoryCacheHelper.IsExistKeyInMemoryCache(keyName.Trim(), out objResultTemp))
        {
            var res = new { Status = true, Value = objResultTemp?.ToString() };
            return Ok(res);
            //return new JsonResult(res);
        }

        var resNotExist = new { Status = false, Value = "Key doesn't exist." };
        return Ok(resNotExist);
        //return new JsonResult(resNotExist);
    }

    [HttpGet("/DeleteMemoryCacheByKey")]
    public ActionResult DeleteMemoryCacheByKey(string keyName)
    {
        if (string.IsNullOrWhiteSpace(keyName))
        {
            var objResult = new { Status = "Key is empty." };
            return Ok(objResult);
            //return new JsonResult(objResult);
        }

        keyName = keyName.Trim();
        bool blnResult = _memoryCacheHelper.DeleteMemoryCache(keyName);
        var res = new { Status = blnResult ? "success" : "failed" };
        return Ok(res);
        //return new JsonResult(res);
    }

    [HttpGet("/DeleteCacheByKey")]
    public async Task<ActionResult> DeleteCacheByKey(string keyName)
    {
        if (string.IsNullOrWhiteSpace(keyName))
        {
            var objResult = new { Status = "Key is empty." };
            return Ok(objResult);
            //return new JsonResult(objResult);
        }

        keyName = keyName.Trim();
        bool blnResult1 = await _distributedCacheHelper.DeleteKeyAsync(keyName);
        bool blnResult2 = _memoryCacheHelper.DeleteMemoryCache(keyName);
        var res = new { Status = (blnResult1 && blnResult2) ? "success" : "failed" };
        return Ok(res);
        //return new JsonResult(res);
    }

    [HttpGet("/DeleteAllCache")]
    public ActionResult DeleteAllCache()
    {
        var tupleResult1 = _distributedCacheHelper.DeleteAllDistributedCache();
        bool blnResult1 = tupleResult1.Item1;
        bool blnResult2 = _memoryCacheHelper.DeleteAllMemoryCache();
        string strResultTemp = tupleResult1.Item2;
        string strResult = (blnResult1 && blnResult2) ? "success" : (string.IsNullOrWhiteSpace(strResultTemp) ? "failed" : strResultTemp);
        var res = new { Status = strResult };
        return Ok(res);
        //return new JsonResult(res);
    }
}
