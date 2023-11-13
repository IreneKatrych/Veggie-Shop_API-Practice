using System.ComponentModel.DataAnnotations;

namespace VeggieShop.Models
{
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
}
