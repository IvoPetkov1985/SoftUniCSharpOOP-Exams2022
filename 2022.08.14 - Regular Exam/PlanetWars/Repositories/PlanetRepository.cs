using PlanetWars.Models.Planets.Contracts;
using PlanetWars.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace PlanetWars.Repositories
{
    public class PlanetRepository : IRepository<IPlanet>
    {
        private readonly List<IPlanet> planets;

        public PlanetRepository()
        {
            planets = new List<IPlanet>();
        }

        public IReadOnlyCollection<IPlanet> Models
            => planets.AsReadOnly();

        public void AddItem(IPlanet planet)
        {
            planets.Add(planet);
        }

        public IPlanet FindByName(string name)
        {
            IPlanet planet = planets.FirstOrDefault(p => p.Name == name);
            return planet;
        }

        public bool RemoveItem(string name)
        {
            IPlanet planet = FindByName(name);
            return planets.Remove(planet);
        }
    }
}
