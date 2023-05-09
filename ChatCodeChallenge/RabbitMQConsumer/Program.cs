﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;

var hubConnection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7187/chatHub")
    .Build();

hubConnection.StartAsync().Wait();

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
    hubConnection.InvokeAsync("SendMessage", "Skynet", message, DateTime.Now).Wait();

};

channel.BasicConsume(queue: "stockInfo", autoAck: true, consumer: consumer);
Console.ReadKey();


