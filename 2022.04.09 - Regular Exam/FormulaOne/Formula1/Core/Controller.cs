using Formula1.Core.Contracts;
using Formula1.Models.Cars;
using Formula1.Models.Contracts;
using Formula1.Models.Pilots;
using Formula1.Models.Races;
using Formula1.Repositories;
using Formula1.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formula1.Core
{
    public class Controller : IController
    {
        private readonly IRepository<IPilot> pilots;
        private readonly IRepository<IRace> races;
        private readonly IRepository<IFormulaOneCar> cars;

        public Controller()
        {
            pilots = new PilotRepository();
            races = new RaceRepository();
            cars = new FormulaOneCarRepository();
        }

        public string AddCarToPilot(string pilotName, string carModel)
        {
            IPilot pilot = pilots.FindByName(pilotName);

            if (pilot == null || pilot.Car != null)
            {
                throw new InvalidOperationException($"Pilot {pilotName} does not exist or has a car.");
            }

            IFormulaOneCar car = cars.FindByName(carModel);

            if (car == null)
            {
                throw new NullReferenceException($"Car {carModel} does not exist.");
            }

            pilot.AddCar(car);
            cars.Remove(car);
            return $"Pilot {pilotName} will drive a {car.GetType().Name} {carModel} car.";
        }

        public string AddPilotToRace(string raceName, string pilotFullName)
        {
            IRace race = races.FindByName(raceName);

            if (race == null)
            {
                throw new NullReferenceException($"Race {raceName} does not exist.");
            }

            IPilot pilot = pilots.FindByName(pilotFullName);

            if (pilot == null || pilot.Car == null || race.Pilots.Contains(pilot))
            {
                throw new InvalidOperationException($"Can not add pilot {pilotFullName} to the race.");
            }

            race.AddPilot(pilot);
            return $"Pilot {pilotFullName} is added to the {raceName} race.";
        }

        public string CreateCar(string type, string model, int horsepower, double engineDisplacement)
        {
            IFormulaOneCar car = cars.FindByName(model);

            if (car != null)
            {
                throw new InvalidOperationException($"Formula one car {model} is already created.");
            }

            if (type != nameof(Ferrari) && type != nameof(Williams))
            {
                throw new InvalidOperationException($"Formula one car type {type} is not valid.");
            }

            if (type == nameof(Ferrari))
            {
                car = new Ferrari(model, horsepower, engineDisplacement);
            }
            else
            {
                car = new Williams(model, horsepower, engineDisplacement);
            }

            cars.Add(car);
            return $"Car {type}, model {model} is created.";
        }

        public string CreatePilot(string fullName)
        {
            IPilot pilot = pilots.FindByName(fullName);

            if (pilot != null)
            {
                throw new InvalidOperationException($"Pilot {fullName} is already created.");
            }

            pilot = new Pilot(fullName);
            pilots.Add(pilot);
            return $"Pilot {fullName} is created.";
        }

        public string CreateRace(string raceName, int numberOfLaps)
        {
            IRace race = races.FindByName(raceName);

            if (race != null)
            {
                throw new InvalidOperationException($"Race {raceName} is already created.");
            }

            race = new Race(raceName, numberOfLaps);
            races.Add(race);
            return $"Race {raceName} is created.";
        }

        public string PilotReport()
        {
            StringBuilder builder = new();

            foreach (IPilot pilot in pilots.Models
                .OrderByDescending(p => p.NumberOfWins))
            {
                builder.AppendLine(pilot.ToString());
            }

            return builder.ToString().TrimEnd();
        }

        public string RaceReport()
        {
            StringBuilder builder = new();

            foreach (IRace race in races.Models
                .Where(r => r.TookPlace))
            {
                builder.AppendLine(race.RaceInfo());
            }

            return builder.ToString().TrimEnd();
        }

        public string StartRace(string raceName)
        {
            IRace race = races.FindByName(raceName);

            if (race == null)
            {
                throw new NullReferenceException($"Race {raceName} does not exist.");
            }

            if (race.Pilots.Count < 3)
            {
                throw new InvalidOperationException($"Race {raceName} cannot start with less than three participants.");
            }

            if (race.TookPlace == true)
            {
                throw new InvalidOperationException($"Can not execute race {raceName}.");
            }

            race.TookPlace = true;
            List<IPilot> selectedPilots = race.Pilots
                .OrderByDescending(p => p.Car.RaceScoreCalculator(race.NumberOfLaps))
                .Take(3)
                .ToList();

            IPilot winner = selectedPilots[0];
            winner.WinRace();

            StringBuilder builder = new();
            builder.AppendLine($"Pilot {selectedPilots[0].FullName} wins the {raceName} race.");
            builder.AppendLine($"Pilot {selectedPilots[1].FullName} is second in the {raceName} race.");
            builder.AppendLine($"Pilot {selectedPilots[2].FullName} is third in the {raceName} race.");

            return builder.ToString().TrimEnd();
        }
    }
}
