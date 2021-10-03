using CPR.Shared.RabbitMQ.Extensions;
using CPR2.ProtocolBuffers;
using CPR2.Shared.DTOs;
using CPR2.Shared.RabbitMQ;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace CPR2.Api.RabbitMQPublishers;

//public class AvailableFlightsRequestPublisher : RabbitMQPublisherBase<AvailableFlightsRequestPublisher>
//{
//    private readonly IPublisherConfirmsRepository _publisherConfirmsRepository;
//    public AvailableFlightsRequestPublisher(ILogger<AvailableFlightsRequestPublisher> logger,
//                                            IConnection amqpConnection,
//                                            IOptionsMonitor<RabbitMQPublisherOptions> rabbitMQPublisherOtions,
//                                            IPublisherConfirmsRepository publisherConfirmsRepository)
//        : base(logger, amqpConnection, rabbitMQPublisherOtions, nameof(AvailableFlightsRequestPublisher))
//    {
//        _publisherConfirmsRepository = publisherConfirmsRepository;
//        _amqpChannel.BindQueuesToExchange(_rabbitMQPublisherOptions);
//        _amqpChannel.ConfirmSelect();

//        _amqpChannel.BasicAcks += AmqpChannelBasicAcksEventHandler;
//        _amqpChannel.BasicNacks += AmqpChannelBasicNacksEventHandler;
//    }

//    private async void AmqpChannelBasicNacksEventHandler(object? sender, RabbitMQ.Client.Events.BasicNackEventArgs e)
//    {
//        string cacheKey = string.Format(CacheKeys.AvailableFlightsRequestPublisher, e.DeliveryTag);
//        byte[] availableFlightsRequestProtoBytes = await _publisherConfirmsRepository.GetValueByKey(cacheKey);
//        AvailableFlightsRequestProto availableFlightsRequestProto = AvailableFlightsRequestProto.Parser.ParseFrom(availableFlightsRequestProtoBytes);
//        _logger.LogError("Message: {AvailableFlightsRequestProto} has been nacked. Sequence Number {DeliveryTag}, Multiple {Multiple}",
//                         availableFlightsRequestProto,
//                         e.DeliveryTag,
//                         e.Multiple);
//    }

//    private async void AmqpChannelBasicAcksEventHandler(object? sender, RabbitMQ.Client.Events.BasicAckEventArgs e)
//    {
//        string cacheKey = string.Format(CacheKeys.AvailableFlightsRequestPublisher, e.DeliveryTag);
//        byte[] availableFlightsRequestProtoBytes = await _publisherConfirmsRepository.GetValueByKeyAndThenDelete(cacheKey);
//        AvailableFlightsRequestProto availableFlightsRequestProto = AvailableFlightsRequestProto.Parser.ParseFrom(availableFlightsRequestProtoBytes);
//        _logger.LogInformation("Message: {AvailableFlightsRequestProto} has been acked. Sequence Number {DeliveryTag}, Multiple {Multiple}",
//                                availableFlightsRequestProto,
//                                e.DeliveryTag,
//                                e.Multiple);
//    }

//    public void PublishAvailableFlightsRequest(AvailableFlightsRequestDto availableFlightsRequestDto)
//    {
//        AvailableFlightsRequestProto availableFlightsRequestProto = new()
//        {
//            DepartureDate = Timestamp.FromDateTimeOffset(availableFlightsRequestDto.DepartureDate),
//            Destination = availableFlightsRequestDto.Destination,
//            Origin = availableFlightsRequestDto.Origin
//        };
//        byte[] availableFlightsRequestProtoBytes = availableFlightsRequestProto.ToByteArray();

//        string cacheKey = string.Format(CacheKeys.AvailableFlightsRequestPublisher, _amqpChannel.NextPublishSeqNo);
//        _publisherConfirmsRepository.SetKeyValue(cacheKey, availableFlightsRequestProtoBytes);

//        _amqpChannel.BasicPublish(_rabbitMQPublisherOptions.ExchangeName,
//                                  _rabbitMQPublisherOptions.RoutingKey,
//                                  null,
//                                  availableFlightsRequestProtoBytes);
//    }
//}


public class AvailableFlightsRequestPublisher : RabbitMQPublisherBase<AvailableFlightsRequestPublisher, AvailableFlightsRequestProto>
{
    public AvailableFlightsRequestPublisher(ILogger<AvailableFlightsRequestPublisher> logger,
                                            IConnection amqpConnection,
                                            IOptionsMonitor<RabbitMQPublisherOptions> rabbitMQPublisherOtions)

        : base(logger, amqpConnection, rabbitMQPublisherOtions, nameof(AvailableFlightsRequestPublisher))
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
