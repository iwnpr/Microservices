using System.Text;
using System.Text.Json;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;

            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("---> Connected to the Message Bus.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not connect to the Message Bus: {ex.Message}");
            }
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);

            if (_connection.IsOpen)
            {
                Console.WriteLine("---> RabbitMQ connection is open, sending message...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("---> RabbitMQ connection is closed, not sending message.");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger",
                                  routingKey: "",
                                  basicProperties: null,
                                  body: body);

            Console.WriteLine($"---> Message sent: {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("---> MessageBus Disposed.");

            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        public void SubscribeToPlatformEvents()
        {
            // Implementation for subscribing to platform events
        }

        public void UnsubscribeFromPlatformEvents()
        {
            // Implementation for unsubscribing from platform events
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("---> RabbitMQ connection shutdown.");
        }
    }
}