using CPR2.Providers.IranAirtour.RabbitMQConsumers;
using CPR2.Shared.RabbitMQ;
using CPR2.Shared.RabbitMQ.Extensions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostingContext, services) =>
    {
        services.AddRabbitMQConnection();
        services.Configure<RabbitMQConsumerOptions>(nameof(AvailableFlightsRequestConsumer),hostingContext.Configuration.GetSection(nameof(AvailableFlightsRequestConsumer)));
    })
    .Build();


var rabbitMQConsumerOptions = host.Services.GetRequiredService<IOptionsMonitor<RabbitMQConsumerOptions>>();
IConnection amqpConnection = host.Services.GetRequiredService<IConnection>();


amqpConnection.CreateChannelWithConsumer<AvailableFlightsRequestConsumer>(rabbitMQConsumerOptions, host.Services);






await host.RunAsync();




