﻿using ChatCodeChallenge.Hubs;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ChatCodeChallenge.RabbitMQ
{
    public class RabitMQService : IRabitMQService
    {
        public void SendStockMessage<T>(T message)
        {
            try
            {
                //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
                var factory = new ConnectionFactory
                {
                    HostName = "localhost"
                };

                //Create the RabbitMQ connection using connection factory details as i mentioned above
                var connection = factory.CreateConnection();

                //Here we create channel with session and model
                using var channel = connection.CreateModel();

                //declare the queue after mentioning name and a few property related to that
                channel.QueueDeclare("stockInfo", exclusive: false);

                //Serialize the message
                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                //put the data on to the product queue
                channel.BasicPublish(exchange: "", routingKey: "stockInfo", body: body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message to RabbitMQ: {ex.Message}");
            }
        }
    }
}
