using VeggieShop.Interfaces;
using VeggieShop.Models;

namespace VeggieShop.Services
{
    public class VeggieService : IVeggieService
    {
        const int SOUP_KIT_COUNT = 3;
        private readonly IVeggieRepozitory _vegetableRepository;

        public VeggieService(IVeggieRepozitory vegetableRepository)
        {
            _vegetableRepository = vegetableRepository;
        }

        public bool Delete(Guid id)
        {
            return _vegetableRepository.Delete(id);
        }

        public IEnumerable<Vegetable> GetAll()
        {
            return _vegetableRepository.GetAll().Select(veggie => veggie as Vegetable);
        }

        public VegetableDetailed? GetById(Guid id)
        {
            return _vegetableRepository.Get(id);
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

        public void Insert(VegetableDetailed veggie)
        {
            veggie.Guid = Guid.NewGuid();
            _vegetableRepository.Insert(veggie);
        }

        public void SetLock(Guid id, bool isLocked)
        {
            _vegetableRepository.SetLocked(id, isLocked);
        }

        public void Update(Guid id, VegetableDetailed veggie)
        {
            _vegetableRepository.Update(id, veggie);
        }

        public bool NameExists(string name, Guid? id)
        {
            return GetAll()
                .Any(veggie => veggie.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                && id != veggie.Guid);
        }

        public bool IsLocked(Guid id)
        {
            var veggie = GetById(id);
            return veggie != null && veggie.IsLocked;
        }

        private static decimal PriceSliced(int diameter)
        {
            return 0.10M * diameter;
        }
    }
}
