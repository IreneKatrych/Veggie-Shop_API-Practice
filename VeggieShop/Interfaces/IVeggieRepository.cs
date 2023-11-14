using VeggieShop.Models;

namespace VeggieShop.Interfaces
{
    public interface IVeggieRepository
    {
        IEnumerable<VegetableDetailed> GetAll();
        VegetableDetailed? Get(Guid Id);
        void Insert(VegetableDetailed veggie);
        void Update(Guid id, VegetableDetailed veggie);
        bool Delete(Guid id);
        void SetLocked(Guid id, bool isLocked);
    }
}
