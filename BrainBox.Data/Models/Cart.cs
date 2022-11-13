using System.ComponentModel.DataAnnotations.Schema;

namespace BrainBox.Data.Models
{
    public class Cart : BaseModel
    {
        public BrainBoxUser User { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual IList<CartProduct> CartProducts { get; set; }
    }
}
