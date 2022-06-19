using System.Collections.Specialized;
using System.Threading;
using System;
using RabbitMQ.Client;
using System.Text;


namespace ProjetoRabbitMQ
{
    public class Program
    {
        static void Main(string[] args)
        {
           string queue = ""; 
           string msg = ""; 
         Console.WriteLine("Testando o envio de mensagens para uma Fila do RabbitMQ");
         Console.WriteLine("Coloque o nome da fila para onde ser enviada a mensagem");

        queue =  Console.ReadLine();


           var factory = new ConnectionFactory() { HostName = "localhost" };
               factory.UserName = "guilrez";
                factory.Password = "Guilherme99";
                using(var connection = factory.CreateConnection())
                using(var channel = connection.CreateModel())
                {
                    queue = String.IsNullOrEmpty(queue) ? "FilaPadrao" : queue;
                    channel.QueueDeclare(queue: queue,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
                    
                    Console.WriteLine($"Fila {queue} Criada");
                    Console.WriteLine($"Digite a mensagem a ser enviada ");
                   
                    msg =  Console.ReadLine();

                    int count = 0;
                    while(true){                  
                        msg = count.ToString();
                        var body = Encoding.UTF8.GetBytes(msg);

                        channel.BasicPublish(exchange: "",
                                            routingKey: queue,
                                            basicProperties: null,
                                            body: body);
                        Console.WriteLine(" [x] Sent {0}", msg);
                        Thread.Sleep(200); 

                        //Console.WriteLine($"Gostaria de enviar uma nova mensagem?"); 
                        //msg =  Console.ReadLine();
                        count++;                  
                    }
                }

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();        
        }
    }
}
