using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;

try
{
    var hubConnection = new HubConnectionBuilder()
        .WithUrl("https://localhost:7187/chatHub")
        .Build();

    await hubConnection.StartAsync();

    var factory = new ConnectionFactory
    {
        HostName = "localhost"
    };

    var connection = factory.CreateConnection();

    using var channel = connection.CreateModel();

    channel.QueueDeclare("stockInfo", exclusive: false);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += async (model, eventArgs) =>
    {
        try
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            await hubConnection.InvokeAsync("SendMessage", "Skynet", message, DateTime.Now);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while processing message: {ex.Message}");
        }
    };

    channel.BasicConsume(queue: "stockInfo", autoAck: true, consumer: consumer);
    Console.ReadKey();
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}


