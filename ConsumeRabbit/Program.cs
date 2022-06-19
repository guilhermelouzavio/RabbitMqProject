using System;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

namespace ConsumeRabbit
{
    class Program
    {
         static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
                factory.UserName = "name";
                factory.Password = "password";
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "GuiApp",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                     Console.WriteLine(" [x] Received {0}", message);
                     channel.BasicAck(ea.DeliveryTag, false);
                    }
                    catch(Exception)
                    {
                        //o ideal é fazer essa tratativa caso houver falha de conexao 
                        //ou algo der errado ao consumir a fila
                        //voce devolve o erro para fila 
                        //o ultimo parametro significa retentativa
                       channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                };

                channel.BasicConsume(queue: "GuiApp",
                                    autoAck: false,
                                    consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
