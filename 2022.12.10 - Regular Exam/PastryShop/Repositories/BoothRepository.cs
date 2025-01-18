using ChristmasPastryShop.Models.Booths.Contracts;
using ChristmasPastryShop.Repositories.Contracts;
using System.Collections.Generic;

namespace ChristmasPastryShop.Repositories
{
    public class BoothRepository : IRepository<IBooth>
    {
        private readonly List<IBooth> booths;

        public BoothRepository()
        {
            booths = new List<IBooth>();
        }

        public IReadOnlyCollection<IBooth> Models
            => booths.AsReadOnly();

        public void AddModel(IBooth booth)
        {
            booths.Add(booth);
        }
    }
}
