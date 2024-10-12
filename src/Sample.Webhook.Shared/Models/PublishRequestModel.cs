namespace Sample.Webhook.Shared.Models;

public record PublishRequestModel(string Topic, object Message);