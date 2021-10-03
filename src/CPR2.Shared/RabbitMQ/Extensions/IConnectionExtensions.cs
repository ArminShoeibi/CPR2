using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CPR2.Shared.RabbitMQ.Extensions;

public static class IConnectionExtensions
{
    public static string CreateChannelWithConsumer<T>(this IConnection amqpConnection,
                                                      IOptionsMonitor<RabbitMQConsumerOptions> rabbitMQConsumerOptionsMonitor,
                                                      IServiceProvider serviceProvider,
                                                      params object[] parameters)
        where T : AsyncEventingBasicConsumer
    {
        var rabbitMQConsumerOptions = rabbitMQConsumerOptionsMonitor.Get(typeof(T).Name);
        var amqpChannel = amqpConnection.CreateModel();
        if (parameters.Any())
            parameters = new object[] { amqpChannel, parameters };
        else
            parameters = new object[] { amqpChannel };

        var amqpConsumer = ActivatorUtilities.CreateInstance<T>(serviceProvider, parameters);
        return amqpChannel.BasicConsume(rabbitMQConsumerOptions.QueueName, rabbitMQConsumerOptions.AutoAcknowledgment, amqpConsumer);
    }
}
