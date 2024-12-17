// using RabbitMQ.Client;
// using RabbitMQ.Client.Events;
// using System.Text;
// using System.Threading.Tasks;

// namespace Back.Infra.Queue;
// public class RabbitMqService
// {
//     private readonly IConnection _connection;
//     private readonly IModel _channel;
//     private const string QueueName = "example_queue";

//     public RabbitMqService()
//     {
//         var factory = new ConnectionFactory
//         {
//             HostName = "localhost", // Endereço do RabbitMQ
//             UserName = "guest",     // Usuário padrão
//             Password = "guest"      // Senha padrão
//         };

//         _connection = factory.CreateConnection();
//         _channel = _connection.CreateModel();
//         _channel.QueueDeclare(
//             queue: QueueName,
//             durable: true,
//             exclusive: false,
//             autoDelete: false,
//             arguments: null
//         );
//     }

//     public void PublishMessage(string message)
//     {
//         var body = Encoding.UTF8.GetBytes(message);
//         _channel.BasicPublish(
//             exchange: "",
//             routingKey: QueueName,
//             basicProperties: null,
//             body: body
//         );
//         Console.WriteLine($"[x] Mensagem enviada: {message}");
//     }

//     public void ConsumeMessages()
//     {
//         var consumer = new EventingBasicConsumer(_channel);

//         consumer.Received += async (model, ea) =>
//         {
//             var body = ea.Body.ToArray();
//             var message = Encoding.UTF8.GetString(body);

//             Console.WriteLine($"[x] Mensagem recebida: {message}");

//             // Simula o atraso de 2 segundos entre as mensagens
//             await Task.Delay(2000);

//             Console.WriteLine($"[x] Mensagem processada: {message}");
//             _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
//         };

//         _channel.BasicConsume(
//             queue: QueueName,
//             autoAck: false,
//             consumer: consumer
//         );
//     }

//     public void Dispose()
//     {
//         _channel?.Close();
//         _connection?.Close();
//     }
// }
