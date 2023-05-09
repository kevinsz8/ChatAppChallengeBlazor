using ChatCodeChallenge.Hubs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ChatCodeChallenge.RabbitMQ
{
    public class RabbitMQConsumer
    {

        private readonly ChatHub _hub;

        public RabbitMQConsumer(ChatHub hub)
        {

            _hub = hub;
        }

        public void ConsumeStockMessage()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
            //Create the RabbitMQ connection using connection factory details as i mentioned above
            var connection = factory.CreateConnection();
            //Here we create channel with session and model
            using
            var channel = connection.CreateModel();
            //declare the queue after mentioning name and a few property related to that
            channel.QueueDeclare("stockInfo", exclusive: false);
            //Set Event object which listen message from chanel which is sent by producer
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Thread.Sleep(3000);
                if (message.Any())
                    await _hub.SendMessage("Skynet", message, DateTime.Now);
            };
            //read the message
            channel.BasicConsume(queue: "stockInfo", autoAck: true, consumer: consumer);
        }
    }
}
