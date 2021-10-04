using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CPR2.Shared.RabbitMQ;

public abstract class RabbitMQPublisherBase<T, TMessage>
{
    protected readonly ILogger<T> _logger;
    protected readonly IConnection _amqpConnection;
    protected readonly IModel _amqpChannel;
    protected readonly RabbitMQPublisherOptions _rabbitMQPublisherOptions;
    public RabbitMQPublisherBase(ILogger<T> logger,
                                 IConnection amqpConnection,
                                 IOptionsMonitor<RabbitMQPublisherOptions> rabbitMQPublisherOptions)
    {
        _logger = logger;
        _amqpConnection = amqpConnection;
        _amqpChannel = _amqpConnection.CreateModel();
        _rabbitMQPublisherOptions = rabbitMQPublisherOptions.Get(typeof(T).Name);
    }

    public abstract void Publish(TMessage message);
}
