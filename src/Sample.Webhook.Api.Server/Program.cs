var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IWebhookService, WebhookService>();
builder.Services.AddHttpClient();
builder.Services.AddLogging();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapPost("/subscribe", ([FromServices] IWebhookService webhookService, [FromServices] ILogger<Program> logger, SubscriptionRequestModel subscription) =>
{
    logger.LogInformation("{Class} | /subscribe | Starting", nameof(Program));

    webhookService.Subscribe(subscription);
});

app.MapPost("/unsubscribe", ([FromServices] IWebhookService webhookService, [FromServices] ILogger<Program> logger, SubscriptionRequestModel subscription) =>
{
    logger.LogInformation("{Class} | /unsubscribe | Starting", nameof(Program));

    webhookService.Unsubscribe(subscription);
});

app.MapPost("/publish", async ([FromServices] IWebhookService webhookService, [FromServices] ILogger<Program> logger, PublishRequestModel request) =>
{
    logger.LogInformation("{Class} | /publish | Starting", nameof(Program));

    await webhookService.PublishMessage(request.Topic, request.Message);
});

app.Run();
