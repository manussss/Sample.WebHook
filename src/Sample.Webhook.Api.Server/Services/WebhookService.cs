namespace Sample.Webhook.Api.Server.Services;

public class WebhookService(IHttpClientFactory httpClientFactory) : IWebhookService
{
    private readonly List<SubscriptionRequestModel> _subscriptions = new();
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient();

    public void Subscribe(SubscriptionRequestModel subscription)
    {
        _subscriptions.Add(subscription);
    }

    public void Unsubscribe(SubscriptionRequestModel subscription)
    {
        _subscriptions.Remove(subscription);
    }

    public async Task PublishMessage(string topic, object message)
    {
        var subscribedWebhooks = _subscriptions.Where(w => w.Topic == topic);

        foreach (var webhook in subscribedWebhooks)
        {
            await _httpClient.PostAsJsonAsync(webhook.Callback, message);
        }
    }
}
