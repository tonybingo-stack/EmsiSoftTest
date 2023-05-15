using Microsoft.AspNetCore.Mvc;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Security.Cryptography;

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

            for (int i = 0; i < 10; i++) //FIXME: 40000
            {
                string randomString = GenerateRandomString();
                string sha1Hash = CalculateSHA1Hash(randomString);

                var message = Encoding.UTF8.GetBytes(sha1Hash);

                channel.BasicPublish(exchange: string.Empty,
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: message);
                Console.WriteLine(sha1Hash);
            }

            return Ok("success");
        }

        static string GenerateRandomString()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }

        static string CalculateSHA1Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    stringBuilder.Append(hashBytes[i].ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
    }
}