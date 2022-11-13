namespace BrainBox.Data.Models
{
    public class ProductCategory : BaseModel
    {
        public string Name { get; set; }

        public virtual IList<Product> Products { get; set; }
    }
}
