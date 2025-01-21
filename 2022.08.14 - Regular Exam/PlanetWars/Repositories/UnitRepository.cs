using PlanetWars.Models.MilitaryUnits.Contracts;
using PlanetWars.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace PlanetWars.Repositories
{
    public class UnitRepository : IRepository<IMilitaryUnit>
    {
        private readonly List<IMilitaryUnit> militaryUnits;

        public UnitRepository()
        {
            militaryUnits = new List<IMilitaryUnit>();
        }

        public IReadOnlyCollection<IMilitaryUnit> Models
            => militaryUnits.AsReadOnly();

        public void AddItem(IMilitaryUnit militaryUnit)
        {
            militaryUnits.Add(militaryUnit);
        }

        public IMilitaryUnit FindByName(string unitTypeName)
        {
            IMilitaryUnit militaryUnit = militaryUnits.FirstOrDefault(mu => mu.GetType().Name == unitTypeName);
            return militaryUnit;
        }

        public bool RemoveItem(string unitTypeName)
        {
            IMilitaryUnit militaryUnit = FindByName(unitTypeName);
            return militaryUnits.Remove(militaryUnit);
        }
    }
}
