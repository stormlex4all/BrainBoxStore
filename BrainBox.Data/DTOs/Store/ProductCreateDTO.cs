using System.ComponentModel.DataAnnotations;

namespace BrainBox.Data.DTOs.Store
{
    public class ProductCreateDTO
    {
        public string? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ProductCategoryId { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
