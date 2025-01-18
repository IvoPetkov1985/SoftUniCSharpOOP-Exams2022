using ChristmasPastryShop.Models.Delicacies.Contracts;
using ChristmasPastryShop.Repositories.Contracts;
using System.Collections.Generic;

namespace ChristmasPastryShop.Repositories
{
    public class DelicacyRepository : IRepository<IDelicacy>
    {
        private readonly List<IDelicacy> delicacies;

        public DelicacyRepository()
        {
            delicacies = new List<IDelicacy>();
        }

        public IReadOnlyCollection<IDelicacy> Models
            => delicacies.AsReadOnly();

        public void AddModel(IDelicacy delicacy)
        {
            delicacies.Add(delicacy);
        }
    }
}
