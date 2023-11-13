using VeggieShop.Interfaces;
using VeggieShop.Models;
using VeggieShop.VeggiesList;

namespace VeggieShop.Repository
{
    public class VeggieRepository : IVeggieRepozitory
    {
        public bool Delete(Guid id)
        {
            var index = VeggiesData.vegetablesList.FindIndex(veggie => veggie.Guid == id);
            if (index != -1)
            {
                VeggiesData.vegetablesList.RemoveAt(index);
                return true;
            }
            return false;
        }
        public Models.VegetableDetailed? Get(Guid id)
        {
            return VeggiesData.vegetablesList.FirstOrDefault(veggie => veggie.Guid == id);
        }

        public void Insert(Models.VegetableDetailed vegetable)
        {
            VeggiesData.vegetablesList.Add(vegetable);
        }

        public void Update(Guid id, Models.VegetableDetailed vegetable)
        {
            var veggie = VeggiesData.vegetablesList.Single(veggie => veggie.Guid == id);
            veggie.Name = vegetable.Name;
            veggie.PricePerKg = vegetable.PricePerKg;
            veggie.StockQuantity = vegetable.StockQuantity;
            veggie.Diameter = vegetable.Diameter;
        }

        public void SetLocked(Guid id, bool isLocked)
        {
            var veggie = VeggiesData.vegetablesList.Single(veggie => veggie.Guid == id);
            veggie.IsLocked = isLocked;
        }

        public IEnumerable<Models.VegetableDetailed> GetAll()
        {
            return VeggiesData.vegetablesList.Select(veggie => veggie);
        }
    }
}
