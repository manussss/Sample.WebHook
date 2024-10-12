namespace Sample.Webhook.Api.Server.Services;

public interface IWebhookService
{
    void Subscribe(SubscriptionRequestModel subscription);
    Task PublishMessage(string topic, object message);
    void Unsubscribe(SubscriptionRequestModel subscription);
}
