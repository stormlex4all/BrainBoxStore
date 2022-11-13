using System.ComponentModel.DataAnnotations;

namespace BrainBox.Data.DTOs.Store
{
    public class ProductCategoryCreateDTO
    {
        public string? Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
