using Microsoft.AspNetCore.Mvc;

namespace VeggieShop.Controllers
{
    public class Vegetable
    {
        public Guid Guid { get; set; }

        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Price in dollars per kilo
        /// </summary>
        public decimal PricePerKg { get; set; }

        /// <summary>
        /// Price in cents per gramm
        /// </summary>
        public decimal PricePerGr => PricePerKg * 100 / 1000;
    }

    public class VegetableDetailed : Vegetable
    {
        /// <summary>
        /// Stock quantity in Kilos
        /// </summary>
        public double StockQuantity { get; set; }

        /// <summary>
        /// Diameter in Centimeters
        /// </summary>
        public int Diameter { get; set; }
    }

    [ApiController]
    [Route("vegetable")]
    public class VegetableController : ControllerBase
    {

        private static readonly List<VegetableDetailed> _vegetablesList = new()
        {
            new VegetableDetailed
            {
                Guid = Guid.Parse("cf995fac-32ea-44d7-8d27-6191e7d8000f"),
                Name = "Potato",
                PricePerKg = 0.75M,
                StockQuantity = 453,
                Diameter = 5,
            },
            new VegetableDetailed
            {
                Guid = Guid.Parse("1666e4d2-87b8-4fbb-a369-92387fffc9b8"),
                Name = "Tomato",
                PricePerKg = 1.15M,
                StockQuantity = 50,
                Diameter = 7
            },
            new VegetableDetailed
            {
                Guid = Guid.Parse("a7443440-71b7-4e19-a7b2-2a6fc2e8f803"),
                Name = "Carrot",
                PricePerKg = 0.25M,
                StockQuantity = 46,
                Diameter = 3
            },
            new VegetableDetailed
            {
                Guid = Guid.Parse("8146f966-0825-410d-a4f8-211e261900fa"),
                Name = "Beetroot",
                PricePerKg = 0.5M,
                StockQuantity = 124,
                Diameter = 10
            },
            new VegetableDetailed
            {
                Guid = Guid.Parse("b2b5793e-6dde-407c-af90-8dfde2f45f9b"),
                Name = "Pumpkin",
                PricePerKg = 1.45M,
                StockQuantity = 276,
                Diameter = 50
            },
            new VegetableDetailed
            {
                Guid = Guid.Parse("eff0dead-9c93-4377-a62f-4222ce9ca7d1"),
                Name = "Cucumber",
                PricePerKg = 0.65M,
                StockQuantity = 43,
                Diameter = 4
            },
            new VegetableDetailed
            {
                Guid = Guid.Parse("3cbc6681-5dba-42d1-94a9-a327070a3157"),
                Name = "Garlic",
                PricePerKg = 3.75M,
                StockQuantity = 28,
                Diameter = 6
            },
            new VegetableDetailed
            {
                Guid = Guid.Parse("11069f2f-1636-4ffb-b6fa-b723756bd057"),
                Name = "Capsicum",
                PricePerKg = 2.55M,
                StockQuantity = 73,
                Diameter = 13
            },
            new VegetableDetailed
            {
                Guid = Guid.Parse("0c1c47b1-89b5-45cc-a914-d26ab14d824a"),
                Name = "Broccoli",
                PricePerKg = 2,
                StockQuantity = 82,
                Diameter = 16
            },
            new VegetableDetailed
            {
                Guid = Guid.Parse("b7c268f4-f4bf-4db8-ad48-9c5985758c1d"),
                Name = "Beans",
                PricePerKg = 1.25M,
                StockQuantity = 147,
                Diameter = 1
            },
        };

        [HttpGet]
        public IActionResult GetAllVegetables()
        {
            var veggieData = _vegetablesList.Select(veggie => veggie as Vegetable);
            return Ok(veggieData);
        }

        [HttpGet("{id}")]
        public IActionResult GetVegetableById(Guid id)
        {
            var vegie = _vegetablesList.FirstOrDefault(veggie => veggie.Guid == id);
            return vegie is not null ? Ok(vegie) : NotFound();
        }

        [HttpPost]
        public IActionResult AddVegetable(VegetableDetailed vegetable)
        {
            vegetable.Guid = Guid.NewGuid();
            _vegetablesList.Add(vegetable);
            return Ok(vegetable);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateVegetable(Guid id, VegetableDetailed vegetable)
        { 
            var veggie = _vegetablesList.Single(veggie => veggie.Guid == id);
            veggie.Name = vegetable.Name;
            veggie.PricePerKg = vegetable.PricePerKg;
            veggie.StockQuantity = vegetable.StockQuantity;
            veggie.Diameter = vegetable.Diameter;
            return Ok(veggie);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVegetable(Guid id)
        {
            var index = _vegetablesList.FindIndex(veggie => veggie.Guid == id);
            if (index != -1)
            {
                _vegetablesList.RemoveAt(index);
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
