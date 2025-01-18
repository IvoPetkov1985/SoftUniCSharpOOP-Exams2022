using ChristmasPastryShop.Models.Delicacies.Contracts;
using System;

namespace ChristmasPastryShop.Models.Delicacies
{
    public abstract class Delicacy : IDelicacy
    {
        private string name;
        private double price;

        protected Delicacy(string delicacyName, double price)
        {
            Name = delicacyName;
            Price = price;
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name cannot be null or whitespace!");
                }

                name = value;
            }
        }

        public double Price
        {
            get => price;
            private set
            {
                price = value;
            }
        }

        public override string ToString()
        {
            return $"{Name} - {Price:F2} lv";
        }
    }
}
