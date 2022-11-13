using System.ComponentModel.DataAnnotations.Schema;

namespace BrainBox.Data.Models
{
    public class Product : BaseModel
    {
        public string Name { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }

        public decimal Price { get; set; }

        [ForeignKey("ProductCategory")]
        public string ProductCategoryId { get; set; }
    }
}
