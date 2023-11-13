using System.ComponentModel.DataAnnotations;

namespace VeggieShop.Models
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
}
