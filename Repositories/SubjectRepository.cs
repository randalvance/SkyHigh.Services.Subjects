using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkyHigh.Services.Subjects.Models;

namespace SkyHigh.Services.Students.Repositories
{
    // TODO: Use Entity Framework
    public class SubjectRepository
    {
        private List<Subject> subjects = new List<Subject>
      {
            new Subject
            {
                SubjectId = 1,
                Name = "Basics of Breathing",
                Description = "Master the art of breathing air by the end of this course."
            },
            new Subject
            {
                SubjectId = 2,
                Name = "How to go to Mars",
                Description = "Preparations needed to go to Mars."
            }
      };

        public async Task<IEnumerable<Subject>> ListAsync()
        {
            return await Task.FromResult<IEnumerable<Subject>>(this.subjects);
        }

        public async Task AddAsync(Subject subject)
        {
            await Task.Run(() =>
            {
                this.subjects.Add(subject);
            });
        }

        public async Task DeleteAsync(int studentId)
        {
            await Task.Run(() =>
            {
                var student = this.subjects.SingleOrDefault(x => x.SubjectId == studentId);

                if (student != null)
                {
                    this.subjects.Remove(student);
                }
            });
        }
    }
}