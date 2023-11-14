namespace VeggieShop.Models
{
    public class SoupKit
    {
        public List<SlicedVegetable> Vegetables { get; set; } = new List<SlicedVegetable>();

        public double Weight { get; set; }

        public decimal Price { get; set; }

        public decimal PriceSliced { get; set; }
    }
}
