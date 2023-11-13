namespace VeggieShop.Models
{
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
}
