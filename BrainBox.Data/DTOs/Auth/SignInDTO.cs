using System.ComponentModel.DataAnnotations;

namespace BrainBox.Data.DTOs.Auth
{
    public class SignInDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
