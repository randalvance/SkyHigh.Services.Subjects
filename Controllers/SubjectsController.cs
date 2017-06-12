using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SkyHigh.Services.Students.Repositories;
using SkyHigh.Services.Subjects.Models;
using SkyHigh.Services.Subjects.Options;

namespace SkyHigh.Services.Subjects.Controllers
{
    [Route("api/[controller]")]
    public class SubjectsController : Controller
    {
        private SubjectRepository subjectRepository;
        private EndpointOptions endpointOptions;
        private ILogger<SubjectsController> logger;

        public SubjectsController(SubjectRepository subjectRepository, IOptions<EndpointOptions> endpointOptions, ILogger<SubjectsController> logger)
        {
            this.subjectRepository = subjectRepository;
            this.endpointOptions = endpointOptions.Value;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Subject>> Get(string searchTerm = "")
        {
            return await this.subjectRepository.ListAsync(searchTerm);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Subject subject)
        {
            await this.subjectRepository.AddAsync(subject);

            this.logger.LogWarning($"Rabbit MQ Endpoint {this.endpointOptions.RabbitMqHostname}");
            
            var factory = new ConnectionFactory() { HostName = this.endpointOptions.RabbitMqHostname, Port = AmqpTcpEndpoint.UseDefaultPort };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "subjects", type: "fanout");

                string json = JsonConvert.SerializeObject(subject);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "subjects", routingKey: "", basicProperties: null, body: body);
            }

            return this.Created("", subject); // TODO: generate resource link
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int subjectId)
        {
            await this.subjectRepository.DeleteAsync(subjectId);

            return this.Ok();
        }
    }
}
