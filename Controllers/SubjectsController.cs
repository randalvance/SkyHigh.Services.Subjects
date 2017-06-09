using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SkyHigh.Services.Students.Repositories;
using SkyHigh.Services.Subjects.Models;

namespace SkyHigh.Services.Subjects.Controllers
{
    [Route("api/[controller]")]
    public class SubjectsController : Controller
    {
        private SubjectRepository subjectRepository;

        public SubjectsController(SubjectRepository subjectRepository)
        {
            this.subjectRepository = subjectRepository;
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

            var factory = new ConnectionFactory() { HostName = "localhost" };

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
