using Microsoft.AspNetCore.Mvc;

namespace VeggieShop.Controllers
{
    public class Vegetable
    {
        public int GUID { get; set; }

        public string Name { get; set; } = string.Empty;

        public float PriceDollarPerKg { get; set; }

        public float PriceCentPerGr => PriceDollarPerKg * 100 / 1000;
    }

    public class DetailedVegetable : Vegetable
    {
        public float StockQuantityKg { get; set; }

        public float DiameterCm { get; set; }
    }

    [ApiController]
    [Route("vegetable")]
    public class VegetableController : ControllerBase
    {

        private static List<DetailedVegetable> vegetablesList = new List<DetailedVegetable> {
            new DetailedVegetable
            {
                GUID = 1,
                Name = "Potato",
                PriceDollarPerKg = 0.75f,
                StockQuantityKg = 453,
                DiameterCm = 5
            },
            new DetailedVegetable
            {
                GUID = 2,
                Name = "Tomato",
                PriceDollarPerKg = 1.15f,
                StockQuantityKg = 50,
                DiameterCm = 7.5f
            },
            new DetailedVegetable
            {
                GUID = 3,
                Name = "Carrot",
                PriceDollarPerKg = 0.25f,
                StockQuantityKg = 46,
                DiameterCm = 3.3f
            },
            new DetailedVegetable
            {
                GUID = 4,
                Name = "Beetroot",
                PriceDollarPerKg = 0.5f,
                StockQuantityKg = 124,
                DiameterCm = 10
            },
            new DetailedVegetable
            {
                GUID = 5,
                Name = "Pumpkin",
                PriceDollarPerKg = 1.45f,
                StockQuantityKg = 276,
                DiameterCm = 50
            },
            new DetailedVegetable
            {
                GUID = 6,
                Name = "Cucumber",
                PriceDollarPerKg = 0.65f,
                StockQuantityKg = 43,
                DiameterCm = 4
            },
            new DetailedVegetable
            {
                GUID = 7,
                Name = "Garlic",
                PriceDollarPerKg = 3.75f,
                StockQuantityKg = 28,
                DiameterCm = 6.5f
            },
            new DetailedVegetable
            {
                GUID = 8,
                Name = "Capsicum",
                PriceDollarPerKg = 2.55f,
                StockQuantityKg = 73,
                DiameterCm = 13.5f
            },
            new DetailedVegetable
            {
                GUID = 9,
                Name = "Broccoli",
                PriceDollarPerKg = 2,
                StockQuantityKg = 82,
                DiameterCm = 16.8f
            },
            new DetailedVegetable
            {
                GUID = 10,
                Name = "Beans",
                PriceDollarPerKg = 1.25f,
                StockQuantityKg = 147,
                DiameterCm = 1.5f
            },
        };

        [HttpGet]
        public IEnumerable<Vegetable> GetAllVegetables()
        {
            return vegetablesList.Select(veggie => veggie as Vegetable);
        }

        [HttpGet("{name}")]
        public DetailedVegetable? GetVegetableByName(string name)
        {
            return vegetablesList.Where(veggie => veggie.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        [HttpPost]
        public DetailedVegetable? addVegetable(DetailedVegetable vegetable)
        {
            int? id = vegetablesList.OrderBy(veggie => veggie.GUID).Last()?.GUID;
            vegetable.GUID = (int)(id == null ? 1 : ++id);
            vegetablesList.Add(vegetable);
            return vegetablesList.Where(veggie => veggie.GUID == id).FirstOrDefault();
        }

        [HttpPut("{id}")]
        public DetailedVegetable? updateVegetable(int id, DetailedVegetable vegetable)
        { 
            var index = vegetablesList.FindIndex(veggie => veggie.GUID == id);
            if (index != -1)
            {
                vegetablesList[index] = vegetable;
                return vegetablesList[index];
            }
            else
            {
                return null;
            }
        }

        [HttpDelete("{id}")]
        public IActionResult deleteVegetable(int id)
        {
            var index = vegetablesList.FindIndex(veggie => veggie.GUID == id);
            if (index != -1)
            {
                vegetablesList.RemoveAt(index);
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
