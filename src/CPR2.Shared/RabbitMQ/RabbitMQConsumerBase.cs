using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CPR2.Shared.RabbitMQ;

public class RabbitMQConsumerBase<T> : AsyncEventingBasicConsumer
{
    private readonly ILogger<T> _logger;
    private readonly RabbitMQConsumerOptions rabbitMQConsumerOptions;
    public RabbitMQConsumerBase(IModel amqpChannel,
                                IOptionsMonitor<RabbitMQConsumerOptions> rabbitMQConsumerOptionsMonitor,
                                ILogger<T> logger) : base(amqpChannel)

    {
        _logger = logger;
        rabbitMQConsumerOptions = rabbitMQConsumerOptionsMonitor.Get(typeof(T).Name);
    }
}
