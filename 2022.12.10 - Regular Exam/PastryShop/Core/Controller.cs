using ChristmasPastryShop.Core.Contracts;
using ChristmasPastryShop.Models.Booths;
using ChristmasPastryShop.Models.Booths.Contracts;
using ChristmasPastryShop.Models.Cocktails;
using ChristmasPastryShop.Models.Cocktails.Contracts;
using ChristmasPastryShop.Models.Delicacies;
using ChristmasPastryShop.Models.Delicacies.Contracts;
using ChristmasPastryShop.Repositories;
using ChristmasPastryShop.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChristmasPastryShop.Core
{
    public class Controller : IController
    {
        private readonly IEnumerable<string> sizes;
        private readonly IRepository<IBooth> booths;

        public Controller()
        {
            booths = new BoothRepository();
            sizes = new List<string>() { "Small", "Middle", "Large" };
        }

        public string AddBooth(int capacity)
        {
            int boothId = booths.Models.Count + 1;
            IBooth booth = new Booth(boothId, capacity);
            booths.AddModel(booth);
            return $"Added booth number {boothId} with capacity {capacity} in the pastry shop!";
        }

        public string AddCocktail(int boothId, string cocktailTypeName, string cocktailName, string size)
        {
            if (cocktailTypeName != nameof(MulledWine) &&
                cocktailTypeName != nameof(Hibernation))
            {
                return $"Cocktail type {cocktailTypeName} is not supported in our application!";
            }

            if (sizes.Contains(size) == false)
            {
                return $"{size} is not recognized as valid cocktail size!";
            }

            IBooth booth = GetBooth(boothId);

            if (booth.CocktailMenu.Models.Any(c => c.Name == cocktailName && c.Size == size))
            {
                return $"{size} {cocktailName} is already added in the pastry shop!";
            }

            ICocktail cocktail = null;

            if (cocktailTypeName == nameof(MulledWine))
            {
                cocktail = new MulledWine(cocktailName, size);
            }
            else
            {
                cocktail = new Hibernation(cocktailName, size);
            }

            booth.CocktailMenu.AddModel(cocktail);
            return $"{size} {cocktailName} {cocktailTypeName} added to the pastry shop!";
        }

        public string AddDelicacy(int boothId, string delicacyTypeName, string delicacyName)
        {
            if (delicacyTypeName != nameof(Gingerbread) &&
                delicacyTypeName != nameof(Stolen))
            {
                return $"Delicacy type {delicacyTypeName} is not supported in our application!";
            }

            IBooth booth = GetBooth(boothId);

            if (booth.DelicacyMenu.Models.Any(d => d.Name == delicacyName))
            {
                return $"{delicacyName} is already added in the pastry shop!";
            }

            IDelicacy delicacy = null;

            if (delicacyTypeName == nameof(Gingerbread))
            {
                delicacy = new Gingerbread(delicacyName);
            }
            else
            {
                delicacy = new Stolen(delicacyName);
            }

            booth.DelicacyMenu.AddModel(delicacy);
            return $"{delicacyTypeName} {delicacyName} added to the pastry shop!";
        }

        public string BoothReport(int boothId)
        {
            IBooth booth = GetBooth(boothId);
            return booth.ToString();
        }

        public string LeaveBooth(int boothId)
        {
            IBooth booth = GetBooth(boothId);
            double currentBill = booth.CurrentBill;
            booth.Charge();
            booth.ChangeStatus();

            StringBuilder builder = new();
            builder.AppendLine($"Bill {currentBill:F2} lv");
            builder.AppendLine($"Booth {boothId} is now available!");
            return builder.ToString().TrimEnd();
        }

        public string ReserveBooth(int countOfPeople)
        {
            IEnumerable<IBooth> selectedBooths = booths.Models
                .Where(b => b.Capacity >= countOfPeople && b.IsReserved == false)
                .OrderBy(b => b.Capacity)
                .ThenByDescending(b => b.BoothId);

            if (selectedBooths.Any() == false)
            {
                return $"No available booth for {countOfPeople} people!";
            }

            IBooth boothForReservation = selectedBooths.First();
            boothForReservation.ChangeStatus();
            return $"Booth {boothForReservation.BoothId} has been reserved for {countOfPeople} people!";
        }

        public string TryOrder(int boothId, string order)
        {
            IBooth booth = GetBooth(boothId);
            string[] orderTokens = order.Split("/");
            string itemTypeName = orderTokens[0];

            if (itemTypeName != nameof(MulledWine) &&
                itemTypeName != nameof(Hibernation) &&
                itemTypeName != nameof(Gingerbread) &&
                itemTypeName != nameof(Stolen))
            {
                return $"{itemTypeName} is not recognized type!";
            }

            string itemName = orderTokens[1];

            if ((itemTypeName == nameof(Hibernation) || itemTypeName == nameof(MulledWine)) &&
                booth.CocktailMenu.Models.Any(c => c.Name == itemName) == false)
            {
                return $"There is no {itemTypeName} {itemName} available!";
            }

            if ((itemTypeName == nameof(Gingerbread) || itemTypeName == nameof(Stolen)) &&
                booth.DelicacyMenu.Models.Any(d => d.Name == itemName) == false)
            {
                return $"There is no {itemTypeName} {itemName} available!";
            }

            int piecesCount = int.Parse(orderTokens[2]);

            if (itemTypeName == nameof(Hibernation) || itemTypeName == nameof(MulledWine))
            {
                string cocktailSize = orderTokens[3];

                if (booth.CocktailMenu.Models.Any(c => c.GetType().Name == itemTypeName && c.Name == itemName && c.Size == cocktailSize) == false)
                {
                    return $"There is no {cocktailSize} {itemName} available!";
                }

                ICocktail cocktail = booth.CocktailMenu.Models.First(c => c.GetType().Name == itemTypeName && c.Name == itemName && c.Size == cocktailSize);

                double bill = piecesCount * cocktail.Price;
                booth.UpdateCurrentBill(bill);
                return $"Booth {boothId} ordered {piecesCount} {itemName}!";
            }
            else
            {
                if (booth.DelicacyMenu.Models.Any(d => d.GetType().Name == itemTypeName && d.Name == itemName) == false)
                {
                    return $"There is no {itemTypeName} {itemName} available!";
                }

                IDelicacy delicacy = booth.DelicacyMenu.Models.First(d => d.GetType().Name == itemTypeName && d.Name == itemName);

                double bill = piecesCount * delicacy.Price;
                booth.UpdateCurrentBill(bill);
                return $"Booth {boothId} ordered {piecesCount} {itemName}!";
            }
        }

        private IBooth GetBooth(int id)
        {
            return booths.Models.FirstOrDefault(b => b.BoothId == id);
        }
    }
}
