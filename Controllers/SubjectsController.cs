using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async  Task<IEnumerable<Subject>> Get()
        {
            return await this.subjectRepository.ListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Subject student)
        {
            await this.subjectRepository.AddAsync(student);

            return this.Created("", student); // TODO: generate resource link
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int studentId)
        {
            await this.subjectRepository.DeleteAsync(studentId);

            return this.Ok();
        }
    }
}
