namespace Sample.Webhook.Api.Server.Services;

public interface IWebhookService
{
    Task Subscribe(SubscriptionRequestModel subscription);
    Task PublishMessage(string topic, object message);
    Task Unsubscribe(SubscriptionRequestModel subscription);
}
