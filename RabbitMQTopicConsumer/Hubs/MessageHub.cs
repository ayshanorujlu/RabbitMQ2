using Microsoft.AspNetCore.SignalR;

namespace RabbitMQTopicConsumer.Hubs
{
    public class MessageHub:Hub
    {
        public override Task OnConnectedAsync()
        {
            return Clients.All.SendAsync("Connected");
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return Clients.All.SendAsync("Disconnected");
        }

        public async Task SendMessageToClients(string routingKey, string message)
        {
            await Clients.All.SendAsync("newmessage", new { routingKey, message });
        }
    }
}
