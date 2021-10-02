namespace CPR2.Shared.RabbitMQ;

public class RabbitMQPublisherOptions
{
    public string QueueName { get; set; }
    public string ExchangeName { get; set; }
    public string ExchangeType { get; set; }
    public string RoutingKey { get; set; } = string.Empty;
}
