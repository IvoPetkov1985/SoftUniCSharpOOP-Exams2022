﻿using System.Collections.Generic;
using System.Linq;
using UniversityCompetition.Models.Contracts;
using UniversityCompetition.Repositories.Contracts;

namespace UniversityCompetition.Repositories
{
    public class UniversityRepository : IRepository<IUniversity>
    {
        private readonly List<IUniversity> universities;

        public UniversityRepository()
        {
            universities = new List<IUniversity>();
        }

        public IReadOnlyCollection<IUniversity> Models
            => universities.AsReadOnly();

        public void AddModel(IUniversity university)
        {
            universities.Add(university);
        }

        public IUniversity FindById(int id)
        {
            IUniversity university = universities.FirstOrDefault(u => u.Id == id);
            return university;
        }

        public IUniversity FindByName(string name)
        {
            IUniversity university = universities.FirstOrDefault(u => u.Name == name);
            return university;
        }
    }
}
