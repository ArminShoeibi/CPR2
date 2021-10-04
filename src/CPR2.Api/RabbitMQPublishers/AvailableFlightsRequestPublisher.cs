using CPR.Shared.RabbitMQ.Extensions;
using CPR2.ProtocolBuffers;
using CPR2.Shared.RabbitMQ;
using Google.Protobuf;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace CPR2.Api.RabbitMQPublishers;


public class AvailableFlightsRequestPublisher : RabbitMQPublisherBase<AvailableFlightsRequestPublisher, AvailableFlightsRequestProto>
{
    public AvailableFlightsRequestPublisher(ILogger<AvailableFlightsRequestPublisher> logger,
                                            IConnection amqpConnection,
                                            IOptionsMonitor<RabbitMQPublisherOptions> rabbitMQPublisherOtions)

        : base(logger, amqpConnection, rabbitMQPublisherOtions)
    {
        _amqpChannel.BindQueuesToExchange(_rabbitMQPublisherOptions);
        _amqpChannel.ConfirmSelect();
    }

    public override void Publish(AvailableFlightsRequestProto message)
    {
        try
        {
            _amqpChannel.BasicPublish(_rabbitMQPublisherOptions.ExchangeName,
                  _rabbitMQPublisherOptions.RoutingKey,
                  null,
                  message.ToByteArray());

            _amqpChannel.WaitForConfirmsOrDie(TimeSpan.FromMilliseconds(0));
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, "yoho");
        }
    }
}
