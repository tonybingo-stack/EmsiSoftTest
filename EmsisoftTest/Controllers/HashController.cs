using Microsoft.AspNetCore.Mvc;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace EmsisoftTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HashController : ControllerBase
    {

        [HttpGet(Name = "GetHashes")]
        public IActionResult Get()
        {
            return Ok("ok");
        }
        [HttpPost(Name = "GenerateHashes")]
        public IActionResult Post()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var message = "Hello World!";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "hello",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine($" [x] Sent {message}");

            return Ok("ok");
        }
    }
}