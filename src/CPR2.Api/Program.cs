using CPR2.Api.RabbitMQPublishers;
using CPR2.Data.Redis;
using CPR2.Shared.RabbitMQ;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CPR2.Api", Version = "v1" });
});

builder.Services.AddRabbitMQConnection();

builder.Services.Configure<RabbitMQPublisherOptions>(nameof(AvailableFlightsRequestPublisher),
                                                     builder.Configuration.GetSection(nameof(AvailableFlightsRequestPublisher)));
builder.Services.AddSingleton<AvailableFlightsRequestPublisher>();




var redisConnection = await ConnectionMultiplexer.ConnectAsync("localhost");
builder.Services.AddSingleton(redisConnection);
builder.Services.AddSingleton<IPublisherConfirmsRepository, PublisherConfirmsRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CPR2.Api v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
