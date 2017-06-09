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
                    Name = "Traveling to Mars 101",
                    Description = "Preparations needed to go to Mars."
                },
                new Subject
                {
                    SubjectId = 3,
                    Name = "Sleeping for Beginners",
                    Description = "Learn the fundamentals of sleeping."
                },
                new Subject
                {
                    SubjectId = 4,
                    Name = "Counting 1, 2, 3: Advanced Techniques",
                    Description = "A 6 month course focusing on counting from 1 to 3"
                },
                new Subject
                {
                    SubjectId = 5,
                    Name = "Bullet Evasion Fundamentals",
                    Description = "How to evade bullets and other projectiles like a ninja."
                },
                new Subject
                {
                    SubjectId = 6,
                    Name = "X-Men Training Course",
                    Description = "Rigorous training and preparation to become a member of the X-Men."
                },
                new Subject
                {
                    SubjectId = 7,
                    Name = "How to Disappear Permanently",
                    Description = "Learn how you can disappear for good."
                }
        };

        public async Task<IEnumerable<Subject>> ListAsync(string searchTerm)
        {
            var results = this.subjects;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                results = this.subjects.Where(subjects => subjects.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }

            return await Task.FromResult<IEnumerable<Subject>>(results);
        }

        public async Task AddAsync(Subject subject)
        {
            subject.SubjectId = this.GetNextId();
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

        private int GetNextId()
        {
            return this.subjects.OrderBy(x => x.SubjectId).LastOrDefault()?.SubjectId + 1 ?? 0;
        }
    }
}