using System.ComponentModel.DataAnnotations;

namespace BrainBox.Data.DTOs.Store
{
    public class CartProductDTO : BaseDTO
    {
        public string CartId { get; set; }

        public string ProductName { get; set; }

        public string ProductId { get; set; }

        public decimal ProductAmount { get; set; }

        public string ProductCategoryName { get; set; }
    }
}
