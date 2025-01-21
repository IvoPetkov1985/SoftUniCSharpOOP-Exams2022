using PlanetWars.Models.Weapons.Contracts;
using PlanetWars.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace PlanetWars.Repositories
{
    public class WeaponRepository : IRepository<IWeapon>
    {
        private readonly List<IWeapon> weapons;

        public WeaponRepository()
        {
            weapons = new List<IWeapon>();
        }

        public IReadOnlyCollection<IWeapon> Models
            => weapons.AsReadOnly();

        public void AddItem(IWeapon weapon)
        {
            weapons.Add(weapon);
        }

        public IWeapon FindByName(string weaponTypeName)
        {
            IWeapon weapon = weapons.FirstOrDefault(w => w.GetType().Name == weaponTypeName);
            return weapon;
        }

        public bool RemoveItem(string weaponTypeName)
        {
            IWeapon weapon = FindByName(weaponTypeName);
            return weapons.Remove(weapon);
        }
    }
}
