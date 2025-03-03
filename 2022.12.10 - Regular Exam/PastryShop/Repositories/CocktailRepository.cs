﻿using ChristmasPastryShop.Models.Cocktails.Contracts;
using ChristmasPastryShop.Repositories.Contracts;
using System.Collections.Generic;

namespace ChristmasPastryShop.Repositories
{
    public class CocktailRepository : IRepository<ICocktail>
    {
        private readonly List<ICocktail> cocktails;

        public CocktailRepository()
        {
            cocktails = new List<ICocktail>();
        }

        public IReadOnlyCollection<ICocktail> Models
            => cocktails.AsReadOnly();

        public void AddModel(ICocktail cocktail)
        {
            cocktails.Add(cocktail);
        }
    }
}
