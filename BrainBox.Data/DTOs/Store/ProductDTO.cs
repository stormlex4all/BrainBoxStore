namespace BrainBox.Data.DTOs.Store
{
    public class ProductDTO : BaseDTO
    {
        public string Name { get; set; }

        public virtual ProductCategoryDTO ProductCategory { get; set; }

        public decimal Price { get; set; }
    }
}
