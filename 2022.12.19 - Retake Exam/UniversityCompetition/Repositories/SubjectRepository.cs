using System.Collections.Generic;
using System.Linq;
using UniversityCompetition.Models.Contracts;
using UniversityCompetition.Repositories.Contracts;

namespace UniversityCompetition.Repositories
{
    public class SubjectRepository : IRepository<ISubject>
    {
        private readonly List<ISubject> subjects;

        public SubjectRepository()
        {
            subjects = new List<ISubject>();
        }

        public IReadOnlyCollection<ISubject> Models
            => subjects.AsReadOnly();

        public void AddModel(ISubject subject)
        {
            subjects.Add(subject);
        }

        public ISubject FindById(int id)
        {
            ISubject subject = subjects.FirstOrDefault(s => s.Id == id);
            return subject;
        }

        public ISubject FindByName(string name)
        {
            ISubject subject = subjects.FirstOrDefault(s => s.Name == name);
            return subject;
        }
    }
}
