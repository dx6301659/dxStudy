using dxStudyDistributedRedisCache.Utility.Cache.Model;
using dxStudyDistributedRedisCache.Utility.Cache.Service;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

// Add services to the container.
services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog(dispose: true);
});

services.AddMemoryCache();
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddSingleton<IDistributedCacheHelper, DistributedCacheHelper>();
services.AddSingleton<IMemoryCacheHelper, MemoryCacheHelper>();

var redisConnectionSetting = configuration.GetSection("RedisConnectionSetting");
services.Configure<RedisConnectionSetting>(redisConnectionSetting);

if (configuration.GetValue<bool>("RedisConnectionSetting:IsUsingRedis"))
{
    services.AddStackExchangeRedisCache(options =>
    {
        options.InstanceName = configuration.GetValue<string>("RedisConnectionSetting:InstanceName");
        string strHostName = configuration.GetValue<string>("RedisConnectionSetting:HostName");
        int intPortNo = configuration.GetValue<int>("RedisConnectionSetting:PortNo");
        options.ConfigurationOptions = new ConfigurationOptions()
        {
            EndPoints = { $"{strHostName}:{intPortNo}" },
            Password = configuration.GetValue<string>("RedisConnectionSetting:Password"),
            AllowAdmin = configuration.GetValue<bool>("RedisConnectionSetting:IsAllowAdmin"),
            Ssl = configuration.GetValue<bool>("RedisConnectionSetting:IsUsingSSL")
        };
    });
}
else
{
    services.AddDistributedMemoryCache();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
