using PlanetWars.Models.MilitaryUnits;
using PlanetWars.Models.MilitaryUnits.Contracts;
using PlanetWars.Models.Planets.Contracts;
using PlanetWars.Models.Weapons;
using PlanetWars.Models.Weapons.Contracts;
using PlanetWars.Repositories;
using PlanetWars.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanetWars.Models.Planets
{
    public class Planet : IPlanet
    {
        private string name;
        private double budget;
        private readonly IRepository<IMilitaryUnit> units;
        private readonly IRepository<IWeapon> weapons;

        public Planet(string name, double budget)
        {
            Name = name;
            Budget = budget;
            units = new UnitRepository();
            weapons = new WeaponRepository();
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Planet name cannot be null or empty.");
                }

                name = value;
            }
        }

        public double Budget
        {
            get => budget;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Budget's amount cannot be negative.");
                }

                budget = value;
            }
        }

        public double MilitaryPower
            => Math.Round(MilitaryPowerCalculator(), 3);

        public IReadOnlyCollection<IMilitaryUnit> Army
            => units.Models;

        public IReadOnlyCollection<IWeapon> Weapons
            => weapons.Models;

        public void AddUnit(IMilitaryUnit unit)
        {
            units.AddItem(unit);
        }

        public void AddWeapon(IWeapon weapon)
        {
            weapons.AddItem(weapon);
        }

        public string PlanetInfo()
        {
            StringBuilder builder = new();
            builder.AppendLine($"Planet: {Name}");
            builder.AppendLine($"--Budget: {Budget} billion QUID");
            builder.Append("--Forces: ");

            if (Army.Any())
            {
                List<string> unitNames = new();

                foreach (IMilitaryUnit unit in Army)
                {
                    unitNames.Add(unit.GetType().Name);
                }

                builder.AppendLine(string.Join(", ", unitNames));
            }
            else
            {
                builder.AppendLine("No units");
            }

            builder.Append("--Combat equipment: ");

            if (Weapons.Any())
            {
                List<string> weaponNames = new();

                foreach (IWeapon weapon in Weapons)
                {
                    weaponNames.Add(weapon.GetType().Name);
                }

                builder.AppendLine(string.Join(", ", weaponNames));
            }
            else
            {
                builder.AppendLine("No weapons");
            }

            builder.AppendLine($"--Military Power: {MilitaryPower}");
            return builder.ToString().TrimEnd();
        }

        public void Profit(double amount)
        {
            Budget += amount;
        }

        public void Spend(double amount)
        {
            if (amount > Budget)
            {
                throw new InvalidOperationException("Budget too low!");
            }

            Budget -= amount;
        }

        public void TrainArmy()
        {
            foreach (IMilitaryUnit unit in units.Models)
            {
                unit.IncreaseEndurance();
            }
        }

        private double MilitaryPowerCalculator()
        {
            double totalAmount = Army.Sum(mu => mu.EnduranceLevel) + Weapons.Sum(w => w.DestructionLevel);

            if (Army.Any(mu => mu.GetType().Name == nameof(AnonymousImpactUnit)))
            {
                totalAmount *= 1.3;
            }

            if (Weapons.Any(w => w.GetType().Name == nameof(NuclearWeapon)))
            {
                totalAmount *= 1.45;
            }

            return totalAmount;
        }
    }
}
