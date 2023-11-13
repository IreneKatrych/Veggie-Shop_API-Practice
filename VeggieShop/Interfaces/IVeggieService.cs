using VeggieShop.Models;

namespace VeggieShop.Interfaces
{
    public interface IVeggieService
    {
        IEnumerable<Vegetable> GetAll();
        VegetableDetailed? GetById(Guid id);
        SlicedVegetable GetSlisedVegetable(VegetableDetailed veggie);
        void Insert(VegetableDetailed veggie);
        void Update(Guid id, VegetableDetailed veggie);
        bool Delete(Guid id);
        void SetLock(Guid id, bool isLocked);
        SoupKit? GetSoupKit(double weight);
        bool NameExists(string name, Guid? id);
        bool IsLocked(Guid id);
    }
}
