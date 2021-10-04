using Google.Protobuf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CPR2.Shared.RabbitMQ.Extensions;

public static class IConnectionExtensions
{
    public static string CreateChannelWithConsumer<T,TMessage>(this IConnection amqpConnection,
                                                      IOptionsMonitor<RabbitMQConsumerOptions> rabbitMQConsumerOptionsMonitor,
                                                      IServiceProvider serviceProvider,
                                                      params object[] parameters)
        where T : RabbitMQConsumerBase<T, TMessage> where TMessage : IMessage<TMessage>
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
