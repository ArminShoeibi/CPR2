﻿using CPR2.ProtocolBuffers;
using CPR2.Shared.RabbitMQ;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CPR2.Providers.IranAirtour.RabbitMQConsumers;

//public class AvailableFlightsRequestConsumer : AsyncEventingBasicConsumer
//{
//    private readonly ILogger<AvailableFlightsRequestConsumer> _logger;
//    public AvailableFlightsRequestConsumer(IModel amqpChannel,
//        ILogger<AvailableFlightsRequestConsumer> logger) : base(amqpChannel)
//    {
//        _logger = logger;
//    }
//    public override Task HandleBasicDeliver(string consumerTag,
//                                            ulong deliveryTag,
//                                            bool redelivered,
//                                            string exchange,
//                                            string routingKey,
//                                            IBasicProperties properties,
//                                            ReadOnlyMemory<byte> body)
//    {
//        var availableFlightsRequestProto = AvailableFlightsRequestProto.Parser.ParseFrom(body.ToArray());
//        _logger.LogInformation("New Available Flights Request: {AvailableFlightsRequestProto}", availableFlightsRequestProto);
//        Model.BasicAck(deliveryTag, false);
//        return Task.CompletedTask;
//    }
//}

public class AvailableFlightsRequestConsumer : RabbitMQConsumerBase<AvailableFlightsRequestConsumer>
{
    private readonly ILogger<AvailableFlightsRequestConsumer> _logger;
    public AvailableFlightsRequestConsumer(IModel amqpChannel,
                                           IOptionsMonitor<RabbitMQConsumerOptions> rabbitMQConsumerOptionsMonitor,
                                           ILogger<AvailableFlightsRequestConsumer> logger)
        : base(amqpChannel, rabbitMQConsumerOptionsMonitor, logger)
    {
    }

    public override Task HandleBasicDeliver(string consumerTag,
                                            ulong deliveryTag,
                                            bool redelivered,
                                            string exchange,
                                            string routingKey,
                                            IBasicProperties properties,
                                            ReadOnlyMemory<byte> body)
    {
        var availableFlightsRequestProto = AvailableFlightsRequestProto.Parser.ParseFrom(body.ToArray());
        _logger.LogInformation("New Available Flights Request: {AvailableFlightsRequestProto}", availableFlightsRequestProto);
        Model.BasicAck(deliveryTag, false);
        return Task.CompletedTask;
    }
}
