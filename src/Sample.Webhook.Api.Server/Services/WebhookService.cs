namespace Sample.Webhook.Api.Server.Services;

public class WebhookService(IHttpClientFactory httpClientFactory, IConnectionMultiplexer redis) : IWebhookService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
    //Using redis to practice, TODO add persistence with sqlserver or aof
    private readonly IDatabase _database = redis.GetDatabase();

    private const string SubscriptionKeyPrefix = "subscriptions:";

    public async Task Subscribe(SubscriptionRequestModel subscription)
    {
        var key = SubscriptionKeyPrefix + subscription.Topic;
        await _database.SetAddAsync(key, subscription.Callback);
    }

    public async Task Unsubscribe(SubscriptionRequestModel subscription)
    {
        var key = SubscriptionKeyPrefix + subscription.Topic;
        await _database.SetRemoveAsync(key, subscription.Callback);
    }

    public async Task PublishMessage(string topic, object message)
    {
        var key = SubscriptionKeyPrefix + topic;
        var subscribers = await _database.SetMembersAsync(key);

        foreach (var subscriber in subscribers)
        {
            await _httpClient.PostAsJsonAsync(subscriber.ToString(), message);
        }
    }
}
