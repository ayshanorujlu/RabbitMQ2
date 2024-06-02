using RabbitMQ.Client;
using System.Text;

namespace RabbitMQTopicExchange.Services
{
    public class EchangeTopicService
    {
        private readonly IConnection _connection;

        public EchangeTopicService(IConnection connection)
        {
            _connection = connection;
        }

        public Task SendMessage(string[] args, string message)
        {
            using var channel = _connection.CreateModel();

            channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);
            foreach (var routingKey in args)
            {
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "topic_logs",
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);

            }
            return Task.CompletedTask;
        }

    }
}
