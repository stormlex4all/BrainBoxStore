using System.ComponentModel.DataAnnotations.Schema;

namespace BrainBox.Data.Models
{
    public class CartProduct : BaseModel
    {
        public virtual Cart Cart { get; set; }

        public virtual Product Product { get; set; }

        [ForeignKey("Cart")]
        public string CartId { get; set; }

        [ForeignKey("Product")]
        public string ProductId { get; set; }
    }
}
