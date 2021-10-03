using RabbitMQ.Client;

namespace Microsoft.Extensions.DependencyInjection;
public static class RabbitMqServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMQConnection(this IServiceCollection services)
    {
        ConnectionFactory amqpConnectionFactory = new()
        {
            ConsumerDispatchConcurrency = 1,
            DispatchConsumersAsync = true,
            AutomaticRecoveryEnabled = true,
            TopologyRecoveryEnabled = true,
            Password = ConnectionFactory.DefaultPass,
            UserName = ConnectionFactory.DefaultUser,
            HostName = "localhost",
            RequestedHeartbeat = TimeSpan.FromSeconds(40),
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
        };
        IConnection amqpConnection = amqpConnectionFactory.CreateConnection();
        return  services.AddSingleton(amqpConnection);
    }
}
