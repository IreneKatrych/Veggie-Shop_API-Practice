using VeggieShop.Interfaces;
using VeggieShop.Models;

namespace VeggieShop.Services
{
    public class VeggieService : IVeggieService
    {
        private readonly IVeggieRepository _vegetableRepository;

        public VeggieService(IVeggieRepository vegetableRepository)
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
    }
}
