using ChristmasPastryShop.Models.Booths.Contracts;
using ChristmasPastryShop.Models.Cocktails;
using ChristmasPastryShop.Models.Cocktails.Contracts;
using ChristmasPastryShop.Models.Delicacies.Contracts;
using ChristmasPastryShop.Repositories;
using ChristmasPastryShop.Repositories.Contracts;
using System;
using System.Text;

namespace ChristmasPastryShop.Models.Booths
{
    public class Booth : IBooth
    {
        private int boothId;
        private int capacity;
        private readonly IRepository<IDelicacy> delicacyMenu;
        private readonly IRepository<ICocktail> cocktailMenu;

        public Booth(int boothId, int capacity)
        {
            BoothId = boothId;
            Capacity = capacity;
            delicacyMenu = new DelicacyRepository();
            cocktailMenu = new CocktailRepository();
        }

        public int BoothId
        {
            get => boothId;
            private set
            {
                boothId = value;
            }
        }

        public int Capacity
        {
            get => capacity;
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Capacity has to be greater than 0!");
                }

                capacity = value;
            }
        }

        public IRepository<IDelicacy> DelicacyMenu
            => delicacyMenu;

        public IRepository<ICocktail> CocktailMenu
            => cocktailMenu;

        public double CurrentBill { get; private set; }

        public double Turnover { get; private set; }

        public bool IsReserved { get; private set; }

        public void ChangeStatus()
        {
            IsReserved = !IsReserved;
        }

        public void Charge()
        {
            Turnover += CurrentBill;
            CurrentBill = 0;
        }

        public void UpdateCurrentBill(double amount)
        {
            CurrentBill += amount;
        }

        public override string ToString()
        {
            StringBuilder builder = new();
            builder.AppendLine($"Booth: {BoothId}");
            builder.AppendLine($"Capacity: {Capacity}");
            builder.AppendLine($"Turnover: {Turnover:F2} lv");
            builder.AppendLine("-Cocktail menu:");

            foreach (ICocktail cocktail in cocktailMenu.Models)
            {
                builder.AppendLine($"--{cocktail.ToString()}");
            }

            builder.AppendLine("-Delicacy menu:");

            foreach (IDelicacy delicacy in delicacyMenu.Models)
            {
                builder.AppendLine($"--{delicacy.ToString()}");
            }

            return builder.ToString().TrimEnd();
        }
    }
}
