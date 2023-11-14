using VeggieShop.Models;

namespace VeggieShop.Interfaces
{
    public interface IVeggieService
    {
        IEnumerable<Vegetable> GetAll();
        VegetableDetailed? GetById(Guid id);
        void Insert(VegetableDetailed veggie);
        void Update(Guid id, VegetableDetailed veggie);
        bool Delete(Guid id);
        void SetLock(Guid id, bool isLocked);
        bool NameExists(string name, Guid? id);
        bool IsLocked(Guid id);
    }
}
