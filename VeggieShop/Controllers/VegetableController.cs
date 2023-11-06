using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VeggieShop.Controllers
{
    public class Vegetable
    {
        public Guid Guid { get; set; }

        [StringLength(16, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Price in dollars per kilo
        /// </summary>
        [Range(0.01, (double)decimal.MaxValue)]
        public decimal PricePerKg { get; set; }

        /// <summary>
        /// Price in cents per gramm
        /// </summary>
        public decimal PricePerGr => PricePerKg * 100 / 1000;

        /// <summary>
        /// Defines if vegetable could be edited
        /// </summary>
        public bool IsLocked { get; set; }
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
        [Range(1, int.MaxValue)]
        public int Diameter { get; set; }
    }

    public class SlicedVegetable : Vegetable
    {
        /// <summary>
        /// Price for slicing 1kg of vegetable.
        /// </summary>
        public decimal SlicingPrice { get; set; }

        /// <summary>
        /// Price per 1kg of sliced vegetable.
        /// </summary>
        public decimal SlicedPrice => SlicingPrice + PricePerKg;
    }

    public class SoupKit
    {
        public List<SlicedVegetable> Vegetables { get; set; } = new List<SlicedVegetable>();

        public double Weight { get; set; }

        public decimal Price { get; set; }

        public decimal PriceSliced { get; set; }
    }

    [ApiController]
    [Route("vegetable")]
    public class VegetableController : ControllerBase
    {
        const int SOUP_KIT_COUNT = 3;

        private static readonly List<VegetableDetailed> _vegetablesList = new()
        {
            new VegetableDetailed
            {
                Guid = Guid.Parse("cf995fac-32ea-44d7-8d27-6191e7d8000f"),
                Name = "Potato",
                PricePerKg = 0.75M,
                StockQuantity = 453,
                Diameter = 5,
                IsLocked = false,
            },
            new VegetableDetailed
            {
                Guid = Guid.Parse("1666e4d2-87b8-4fbb-a369-92387fffc9b8"),
                Name = "Tomato",
                PricePerKg = 1.15M,
                StockQuantity = 50,
                Diameter = 7,
                IsLocked = false,
            },
            new VegetableDetailed
            {
                Guid = Guid.Parse("a7443440-71b7-4e19-a7b2-2a6fc2e8f803"),
                Name = "Carrot",
                PricePerKg = 0.25M,
                StockQuantity = 46,
                Diameter = 3,
                IsLocked = false,
            },
            new VegetableDetailed
            {
                Guid = Guid.Parse("8146f966-0825-410d-a4f8-211e261900fa"),
                Name = "Beetroot",
                PricePerKg = 0.5M,
                StockQuantity = 124,
                Diameter = 10,
                IsLocked = false,
            },
            new VegetableDetailed
            {
                Guid = Guid.Parse("b2b5793e-6dde-407c-af90-8dfde2f45f9b"),
                Name = "Pumpkin",
                PricePerKg = 1.45M,
                StockQuantity = 276,
                Diameter = 50,
                IsLocked = false,
            },
            new VegetableDetailed
            {
                Guid = Guid.Parse("eff0dead-9c93-4377-a62f-4222ce9ca7d1"),
                Name = "Cucumber",
                PricePerKg = 0.65M,
                StockQuantity = 43,
                Diameter = 4,
                IsLocked = false,
            },
            new VegetableDetailed
            {
                Guid = Guid.Parse("3cbc6681-5dba-42d1-94a9-a327070a3157"),
                Name = "Garlic",
                PricePerKg = 3.75M,
                StockQuantity = 28,
                Diameter = 6,
                IsLocked = false,
            },
            new VegetableDetailed
            {
                Guid = Guid.Parse("11069f2f-1636-4ffb-b6fa-b723756bd057"),
                Name = "Capsicum",
                PricePerKg = 2.55M,
                StockQuantity = 73,
                Diameter = 13,
                IsLocked = false,
            },
            new VegetableDetailed
            {
                Guid = Guid.Parse("0c1c47b1-89b5-45cc-a914-d26ab14d824a"),
                Name = "Broccoli",
                PricePerKg = 2,
                StockQuantity = 82,
                Diameter = 16,
                IsLocked = false,
            },
            new VegetableDetailed
            {
                Guid = Guid.Parse("b7c268f4-f4bf-4db8-ad48-9c5985758c1d"),
                Name = "Beans",
                PricePerKg = 1.25M,
                StockQuantity = 147,
                Diameter = 1,
                IsLocked = false,
            },
        };

        [HttpGet]
        public IActionResult GetAllVegetables()
        {
            var veggieData = _vegetablesList.Select(veggie => veggie as Vegetable);
            return Ok(veggieData);
        }

        [HttpGet("{id}")]
        public IActionResult GetVegetableById(Guid id, [FromQuery] bool sliced)
        {
            var vegie = _vegetablesList.FirstOrDefault(veggie => veggie.Guid == id);

            if (vegie is null)
            {
                return NotFound();
            }
                 
            return sliced ? Ok(GetSlisedVegetable(vegie)) : Ok(vegie);
        }

        [HttpPost]
        public IActionResult AddVegetable(VegetableDetailed vegetable)
        {
            if (NameExists(vegetable.Name, null))
            {
                return Problem("Vegetable with same name already exists.", "Vegetable", StatusCodes.Status403Forbidden);
            }

            vegetable.Guid = Guid.NewGuid();
            _vegetablesList.Add(vegetable);
            return Ok(vegetable);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateVegetable(Guid id, VegetableDetailed vegetable)
        { 
            var veggie = _vegetablesList.Single(veggie => veggie.Guid == id);

            if (veggie.IsLocked) 
            {
                return Problem("Vegetable is already locked.", "Vegetable", StatusCodes.Status423Locked, "Locked");
            }

            if (NameExists(vegetable.Name, id))
            {
                return Problem("Vegetable with same name already exists.", "Vegetable", StatusCodes.Status403Forbidden);
            }

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

        [HttpPost("lock/{id}")]
        public IActionResult LockVegetable(Guid id)
        {
            var veggie = _vegetablesList.Single(veggie => veggie.Guid == id);

            if (veggie.IsLocked)
            {
                return Problem("Vegetable is already locked.", "Vegetable", StatusCodes.Status423Locked, "Locked");
            }

            veggie.IsLocked = true;
            return Ok();
        }

        [HttpPost("unlock/{id}")]
        public IActionResult UnlockVegetable(Guid id)
        {
            var veggie = _vegetablesList.Single(veggie => veggie.Guid == id);
            veggie.IsLocked = false;
            return Ok();
        }

        [HttpGet("composesoupkit")]
        public IActionResult GetSoupKit([FromQuery] double weight)
        {
            var eachWeight = weight / SOUP_KIT_COUNT;
            var availableVeggies = _vegetablesList.Where(veggie => veggie.StockQuantity >= eachWeight);

            if (availableVeggies.Count() < SOUP_KIT_COUNT)
            {
                return Problem("There are no available vegetables for the soupkit.", "Vegetable", StatusCodes.Status406NotAcceptable);
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

            return Ok(soupKit);
        }

        private static bool NameExists(string name, Guid? id)
        {
            return _vegetablesList.Any(veggie => veggie.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                && id != veggie.Guid);
        }

        private static decimal PriceSliced(int diameter)
        { 
            return 0.10M * diameter;
        }

        private static SlicedVegetable GetSlisedVegetable(VegetableDetailed veggie)
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

    }
}
