using CPR2.Shared.RabbitMQ;
using RabbitMQ.Client;

namespace CPR.Shared.RabbitMQ.Extensions;
public static class IModelExtensions
{
    public static void BindQueuesToExchange(this IModel amqpChannel, RabbitMQPublisherOptions rabbitMQPublisherOptionsBase)
    {
        amqpChannel.ExchangeDeclare(rabbitMQPublisherOptionsBase.ExchangeName, rabbitMQPublisherOptionsBase.ExchangeType, true, false);

        Dictionary<string, object> queueArguments = new();
        queueArguments.Add("x-queue-type", "quorum");

        foreach (var provider in Providers)
        {
            string providerQueueName = string.Format(provider.Value, rabbitMQPublisherOptionsBase.QueueName);
            amqpChannel.QueueDeclare(providerQueueName, true, false, false, queueArguments);

            string providerRoutingKeyName = rabbitMQPublisherOptionsBase.RoutingKey;
            if (providerRoutingKeyName != "")
                providerRoutingKeyName = string.Format(provider.Value, rabbitMQPublisherOptionsBase.RoutingKey);

            amqpChannel.QueueBind(providerQueueName, rabbitMQPublisherOptionsBase.ExchangeName, providerRoutingKeyName, null);
        }
    }

    public static readonly Dictionary<string, string> Providers = new()
    {
        { "IranAir", "IranAir{0}" },
        { "IranAirtour", "IranAirtour{0}" },
        { "Mahan", "Mahan{0}" },
        { "Varesh", "Varesh{0}" },
        { "Aseman", "Aseman{0}" },
        { "Ata", "Ata{0}" },
        { "Caspian", "Caspian{0}" },
        { "Karun", "Karun{0}" },
        { "KishAir", "KishAir{0}" },
        { "Meraj", "Meraj{0}" },
        { "Sepehran", "Sepehran{0}" },
        { "Saha", "Saha{0}" },
        { "QeshmAir", "QeshmAir{0}" },
        { "Zagros", "Zagros{0}" },
        { "ParsAir", "ParsAir{0}" },
    };
}
