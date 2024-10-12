var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

const string server = "http://localhost:5041";
const string callback = "http://localhost:5003/webhook/item";
const string topic = "item.new";
using var client = new HttpClient();
await client.PostAsJsonAsync(server + "/subscribe", new { topic, callback });

app.MapPost("/webhook/item", (object payload, [FromServices] ILogger<Program> logger) =>
{
    logger.LogInformation("{Class} | /webhook/item | Received: {payload}", nameof(Program), payload);
});

app.Run();
