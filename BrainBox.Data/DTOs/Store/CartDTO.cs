using BrainBox.Data.DTOs.Auth;
using BrainBox.Data.Models;

namespace BrainBox.Data.DTOs.Store
{
    public class CartDTO : BaseDTO
    {
        public UserDTO User { get; set; }

        public virtual IList<CartProductDTO> CartProducts { get; set; }
    }
}
