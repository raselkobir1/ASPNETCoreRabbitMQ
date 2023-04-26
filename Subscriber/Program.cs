using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory 
{ 
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
};
var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare("orders", exclusive: false);

var consummer = new EventingBasicConsumer(channel);
consummer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(message);
};
channel.BasicConsume(queue: "orders", autoAck: true, consumer: consummer);
Console.ReadKey();