using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQTopicConsumer.Hubs;
using System.Text;

namespace RabbitMQTopicConsumer.Services
{
    public class TopicConsumerService
    {
        private readonly IConnection _connection;
        private readonly IHubContext<MessageHub> _hubContext;
        private static List<string> _existings { get; set; } = new List<string>();
        public string? LastQueueName { get; set; }
        public TopicConsumerService(IConnection connection, IHubContext<MessageHub> hubContext)
        {
            _connection = connection;
            _hubContext=hubContext;
        }

        public void ConnectPatterns(string[] args)
        {
            if (args.Length < 1)
            {
                return;
            }

            var channel = _connection.CreateModel();
            if (LastQueueName!=null)
            {
                Console.Out.WriteLine($" queuename::: {LastQueueName}");

                foreach (var bindingKey in _existings)
                {
                    channel.QueueUnbind(queue: LastQueueName,
                                        exchange: "topic_logs",
                                        routingKey: bindingKey);
                }
            }


            channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);
            var queueName = channel.QueueDeclare().QueueName;
            LastQueueName=queueName;
            foreach (var bindingKey in args)
            {
                channel.QueueBind(queue: queueName,
                                  exchange: "topic_logs",
                                  routingKey: bindingKey);
            }


            _existings = args.ToList();

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Console.Out.WriteLine(message);
                await _hubContext.Clients.All.SendAsync("newmessage", new { routingKey, message });
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

        }
    }
}
