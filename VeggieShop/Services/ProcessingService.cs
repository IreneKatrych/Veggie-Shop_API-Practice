using VeggieShop.Interfaces;
using VeggieShop.Models;

namespace VeggieShop.Services
{
    public class ProcessingService : IProcessingService
    {
        const int SOUP_KIT_COUNT = 3;
        private readonly IVeggieRepository _vegetableRepository;

        public ProcessingService(IVeggieRepository vegetableRepository)
        {
            _vegetableRepository = vegetableRepository;
        }

        public SlicedVegetable GetSlisedVegetable(VegetableDetailed veggie)
        {
            SlicedVegetable result = new()
            {
                Guid = veggie.Guid,
                Name = veggie.Name,
                IsLocked = veggie.IsLocked,
                PricePerKg = veggie.PricePerKg,
                SlicingPrice = PriceSliced(veggie.Diameter)
            };
            return result;
        }

        public SoupKit? GetSoupKit(double weight)
        {
            var eachWeight = weight / SOUP_KIT_COUNT;
            var availableVeggies = _vegetableRepository.GetAll().Where(veggie => veggie.StockQuantity >= eachWeight);

            if (availableVeggies.Count() < SOUP_KIT_COUNT)
            {
                return null;
            }

            var soupKit = new SoupKit();
            var randomVeggies = availableVeggies.OrderBy(veggie => Random.Shared.Next())
                .Take(SOUP_KIT_COUNT);

            foreach (var randVeggie in randomVeggies)
            {
                soupKit.Vegetables.Add(GetSlisedVegetable(randVeggie));
            }

            soupKit.Weight = weight;
            soupKit.Price = soupKit.Vegetables.Sum(veggie => veggie.PricePerKg * (decimal)eachWeight);
            soupKit.PriceSliced = soupKit.Vegetables.Sum(veggie => veggie.SlicedPrice * (decimal)eachWeight);
            return soupKit;
        }

        private static decimal PriceSliced(int diameter)
        {
            return 0.10M * diameter;
        }
    }
}
