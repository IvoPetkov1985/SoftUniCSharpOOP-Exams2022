using System.Collections.Generic;
using System.Linq;
using UniversityCompetition.Models.Contracts;
using UniversityCompetition.Repositories.Contracts;

namespace UniversityCompetition.Repositories
{
    public class StudentRepository : IRepository<IStudent>
    {
        private readonly List<IStudent> students;

        public StudentRepository()
        {
            students = new List<IStudent>();
        }

        public IReadOnlyCollection<IStudent> Models
            => students.AsReadOnly();

        public void AddModel(IStudent student)
        {
            students.Add(student);
        }

        public IStudent FindById(int id)
        {
            IStudent student = students.FirstOrDefault(s => s.Id == id);
            return student;
        }

        public IStudent FindByName(string name)
        {
            string[] names = name.Split(" ");
            string firstName = names[0];
            string lastName = names[1];
            IStudent student = students.FirstOrDefault(s => s.FirstName == firstName && s.LastName == lastName);
            return student;
        }
    }
}
