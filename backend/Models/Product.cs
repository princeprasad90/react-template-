using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Product
    {
        [Required]
        public string Code { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Quantity { get; set; }
    }
}
