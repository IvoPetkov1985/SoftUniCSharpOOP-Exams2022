namespace ChristmasPastryShop.Models.Cocktails
{
    public class MulledWine : Cocktail
    {
        private const double LargeMulledWinePrice = 13.50;

        public MulledWine(string cocktailName, string size)
            : base(cocktailName, size, LargeMulledWinePrice)
        {
        }
    }
}
