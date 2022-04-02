using Microsoft.AspNetCore.Mvc;

namespace dxStudyRedisByAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IRedisHelper _redisHelper;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IRedisHelper redisHelper)
        {
            _logger = logger;
            _redisHelper = redisHelper;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("GetRedisCacheByKey")]
        public async Task<ActionResult> GetRedisCacheByKey(string keyName)
        {
            if (string.IsNullOrWhiteSpace(keyName))
                return Ok("{\"result\":\"Key is empty.\"}");

            keyName = keyName.Trim();
            _logger.LogInformation($"Get Key {keyName} information from redis...");

            string strResult = await _redisHelper.GetStringAsync(keyName);
            return Ok("{\"result\":\"" + strResult + "\"}");
        }

        [HttpPost("SetRedisCache")]
        public async Task<ActionResult> SetRedisCache(string keyName, string inputValue, double? secondExpiryTime)
        {
            if (string.IsNullOrWhiteSpace(keyName))
                return Ok("{\"result\":\"Key is empty.\"}");

            keyName = keyName.Trim();
            _logger.LogInformation($"Add Key {keyName} to redis...");

            bool blnResult = await _redisHelper.SetStringAsync(keyName, inputValue, secondExpiryTime);
            return Ok("{\"result\":\"" + (blnResult ? "success" : "failed") + "\"}");
        }
    }
}