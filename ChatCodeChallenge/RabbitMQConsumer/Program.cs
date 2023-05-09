using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;



        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        var connection = factory.CreateConnection();

        using
        var channel = connection.CreateModel();

        channel.QueueDeclare("stockInfo", exclusive: false);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"message received: {message}");
           
        };

        channel.BasicConsume(queue: "stockInfo", autoAck: true, consumer: consumer);
        Console.ReadKey();


