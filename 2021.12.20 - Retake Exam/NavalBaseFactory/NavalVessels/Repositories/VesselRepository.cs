﻿using NavalVessels.Models.Contracts;
using NavalVessels.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace NavalVessels.Repositories
{
    public class VesselRepository : IRepository<IVessel>
    {
        private readonly List<IVessel> vessels;

        public VesselRepository()
        {
            vessels = new List<IVessel>();
        }

        public IReadOnlyCollection<IVessel> Models
            => vessels.AsReadOnly();

        public void Add(IVessel model)
        {
            vessels.Add(model);
        }

        public IVessel FindByName(string name)
        {
            return vessels.FirstOrDefault(v => v.Name == name);
        }

        public bool Remove(IVessel model)
        {
            return vessels.Remove(model);
        }
    }
}
