using ChristmasPastryShop.Models.Cocktails.Contracts;
using System;

namespace ChristmasPastryShop.Models.Cocktails
{
    public abstract class Cocktail : ICocktail
    {
        private string name;
        private string size;
        private double price;

        protected Cocktail(string cocktailName, string size, double price)
        {
            Name = cocktailName;
            Size = size;
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

        public string Size
        {
            get => size;
            private set
            {
                size = value;
            }
        }

        public double Price
        {
            get => price;
            private set
            {
                if (Size == "Middle")
                {
                    value = value * 2 / 3;
                }
                else if (Size == "Small")
                {
                    value = value / 3;
                }

                price = value;
            }
        }

        public override string ToString()
        {
            return $"{Name} ({Size}) - {Price:F2} lv";
        }
    }
}
