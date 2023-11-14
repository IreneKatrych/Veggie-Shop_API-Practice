using VeggieShop.Models;

namespace VeggieShop.Interfaces
{
    public interface IProcessingService
    {
        SlicedVegetable GetSlisedVegetable(VegetableDetailed veggie);
        SoupKit? GetSoupKit(double weight);
    }
}
