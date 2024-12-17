using Back;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
namespace Back.Infra.Queue;
public class Queue
{
    public static void ConsumerLoop(Settings settings)
    {
        var factory = new ConnectionFactory()
        {
            HostName = settings.RMqHostName,
            UserName = settings.RMqUserName,
            Password = settings.RMqPassword,
        };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            string queueName = "test_queue";
            channel.QueueDeclare(queueName, false, false, false, null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());

                Console.WriteLine($"Mensagem recebida: {message}");
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
    public static void Sender(Settings settings)
    {
        var factory = new ConnectionFactory()
        {
            HostName = settings.RMqHostName,
            UserName = settings.RMqUserName,
            Password = settings.RMqPassword,
        };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            string queueName = "test_queue";
            string message = "Olá, RabbitMQ!";

            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

            Console.WriteLine($"Mensagem enviada: {message}");
        }
    }
}
