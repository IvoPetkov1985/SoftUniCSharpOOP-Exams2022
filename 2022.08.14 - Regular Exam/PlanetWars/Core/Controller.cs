using PlanetWars.Core.Contracts;
using PlanetWars.Models.MilitaryUnits;
using PlanetWars.Models.MilitaryUnits.Contracts;
using PlanetWars.Models.Planets;
using PlanetWars.Models.Planets.Contracts;
using PlanetWars.Models.Weapons;
using PlanetWars.Models.Weapons.Contracts;
using PlanetWars.Repositories;
using PlanetWars.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanetWars.Core
{
    public class Controller : IController
    {
        private readonly IRepository<IPlanet> planets;

        public Controller()
        {
            planets = new PlanetRepository();
        }

        public string AddUnit(string unitTypeName, string planetName)
        {
            IPlanet planet = planets.FindByName(planetName) ?? throw new InvalidOperationException($"Planet {planetName} does not exist!");

            if (unitTypeName != nameof(AnonymousImpactUnit) &&
                unitTypeName != nameof(SpaceForces) &&
                unitTypeName != nameof(StormTroopers))
            {
                throw new InvalidOperationException($"{unitTypeName} still not available!");
            }

            if (planet.Army.Any(mu => mu.GetType().Name == unitTypeName))
            {
                throw new InvalidOperationException($"{unitTypeName} already added to the Army of {planetName}!");
            }

            IMilitaryUnit unit = null;

            switch (unitTypeName)
            {
                case nameof(AnonymousImpactUnit):
                    unit = new AnonymousImpactUnit();
                    break;
                case nameof(SpaceForces):
                    unit = new SpaceForces();
                    break;
                case nameof(StormTroopers):
                    unit = new StormTroopers();
                    break;
            }

            planet.Spend(unit.Cost);
            planet.AddUnit(unit);
            return $"{unitTypeName} added successfully to the Army of {planetName}!";
        }

        public string AddWeapon(string planetName, string weaponTypeName, int destructionLevel)
        {
            IPlanet planet = planets.FindByName(planetName) ?? throw new InvalidOperationException($"Planet {planetName} does not exist!");

            if (planet.Weapons.Any(w => w.GetType().Name == weaponTypeName))
            {
                throw new InvalidOperationException($"{weaponTypeName} already added to the Weapons of {planetName}!");
            }

            if (weaponTypeName != nameof(BioChemicalWeapon) &&
                weaponTypeName != nameof(NuclearWeapon) &&
                weaponTypeName != nameof(SpaceMissiles))
            {
                throw new InvalidOperationException($"{weaponTypeName} still not available!");
            }

            IWeapon weapon = null;

            switch (weaponTypeName)
            {
                case nameof(BioChemicalWeapon):
                    weapon = new BioChemicalWeapon(destructionLevel);
                    break;
                case nameof(NuclearWeapon):
                    weapon = new NuclearWeapon(destructionLevel);
                    break;
                case nameof(SpaceMissiles):
                    weapon = new SpaceMissiles(destructionLevel);
                    break;
            }

            planet.Spend(weapon.Price);
            planet.AddWeapon(weapon);
            return $"{planetName} purchased {weaponTypeName}!";
        }

        public string CreatePlanet(string name, double budget)
        {
            if (planets.FindByName(name) != null)
            {
                return $"Planet {name} is already added!";
            }

            IPlanet planet = new Planet(name, budget);
            planets.AddItem(planet);
            return $"Successfully added Planet: {name}";
        }

        public string ForcesReport()
        {
            StringBuilder builder = new();
            builder.AppendLine("***UNIVERSE PLANET MILITARY REPORT***");

            IEnumerable<IPlanet> orderedPlanets = planets.Models
                .OrderByDescending(p => p.MilitaryPower)
                .ThenBy(p => p.Name);

            foreach (IPlanet planet in orderedPlanets)
            {
                builder.AppendLine(planet.PlanetInfo());
            }

            return builder.ToString().TrimEnd();
        }

        public string SpaceCombat(string planetOne, string planetTwo)
        {
            IPlanet firstPlanet = planets.FindByName(planetOne);
            IPlanet secondPlanet = planets.FindByName(planetTwo);

            double firstPlanetValues = firstPlanet.Army.Sum(mu => mu.Cost) + firstPlanet.Weapons.Sum(w => w.Price);
            double secondPlanetValues = secondPlanet.Army.Sum(mu => mu.Cost) + secondPlanet.Weapons.Sum(w => w.Price);

            double firstPlanetHalfBudget = firstPlanet.Budget / 2;
            double secondPlanetHalfBudget = secondPlanet.Budget / 2;

            if (firstPlanet.MilitaryPower > secondPlanet.MilitaryPower)
            {
                firstPlanet.Spend(firstPlanetHalfBudget);
                firstPlanet.Profit(secondPlanetHalfBudget);
                firstPlanet.Profit(secondPlanetValues);
                planets.RemoveItem(planetTwo);
                return $"{planetOne} destructed {planetTwo}!";
            }
            else if (firstPlanet.MilitaryPower < secondPlanet.MilitaryPower)
            {
                secondPlanet.Spend(secondPlanetHalfBudget);
                secondPlanet.Profit(firstPlanetHalfBudget);
                secondPlanet.Profit(firstPlanetValues);
                planets.RemoveItem(planetOne);
                return $"{planetTwo} destructed {planetOne}!";
            }
            else
            {
                if (firstPlanet.Weapons.Any(w => w.GetType().Name == nameof(NuclearWeapon)) &&
                    !secondPlanet.Weapons.Any(w => w.GetType().Name == nameof(NuclearWeapon)))
                {
                    firstPlanet.Spend(firstPlanetHalfBudget);
                    firstPlanet.Profit(secondPlanetHalfBudget);
                    firstPlanet.Profit(secondPlanetValues);
                    planets.RemoveItem(planetTwo);
                    return $"{planetOne} destructed {planetTwo}!";
                }
                else if (!firstPlanet.Weapons.Any(w => w.GetType().Name == nameof(NuclearWeapon)) &&
                    secondPlanet.Weapons.Any(w => w.GetType().Name == nameof(NuclearWeapon)))
                {
                    secondPlanet.Spend(secondPlanetHalfBudget);
                    secondPlanet.Profit(firstPlanetHalfBudget);
                    secondPlanet.Profit(firstPlanetValues);
                    planets.RemoveItem(planetOne);
                    return $"{planetTwo} destructed {planetOne}!";
                }
                else
                {
                    firstPlanet.Spend(firstPlanetHalfBudget);
                    secondPlanet.Spend(secondPlanetHalfBudget);
                    return "The only winners from the war are the ones who supply the bullets and the bandages!";
                }
            }
        }

        public string SpecializeForces(string planetName)
        {
            IPlanet planet = planets.FindByName(planetName) ?? throw new InvalidOperationException($"Planet {planetName} does not exist!");

            if (planet.Army.Any() == false)
            {
                throw new InvalidOperationException("No units available for upgrade!");
            }

            planet.Spend(1.25);
            planet.TrainArmy();
            return $"{planetName} has upgraded its forces!";
        }
    }
}
