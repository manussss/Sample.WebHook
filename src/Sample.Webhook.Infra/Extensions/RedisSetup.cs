namespace Sample.Webhook.Infra.Extensions;

public static class RedisSetup
{
    public static void AddRedisSetup(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionMultiplexer = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!);
        services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
    }
}
