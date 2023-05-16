using Microsoft.AspNetCore.Mvc;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Security.Cryptography;
using EmsisoftTest.Context;
using EmsisoftTest.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EmsisoftTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HashController : ControllerBase
    {
        private MyContext _context;
        public HashController(MyContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetHashes")]
        public string Get()
        {
            var hashTable = _context.hashes;

            var hashResponse = new HashResponse
            {
                Hashes = hashTable
                    .GroupBy(h => h.date.Date)
                    .Select(g => new HashEntry
                    {
                        Date = g.Key.Date,
                        Count = g.Sum(h => 1)
                    })
                    .ToList()
            };

            var jsonSettings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd",
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            string jsonResponse = JsonConvert.SerializeObject(hashResponse, jsonSettings);

            return jsonResponse;
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

            for (int i = 0; i < 40000; i++) 
            {
                string randomString = GenerateRandomString();
                string sha1Hash = CalculateSHA1Hash(randomString);

                var message = Encoding.UTF8.GetBytes(sha1Hash);

                channel.BasicPublish(exchange: string.Empty,
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: message);
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