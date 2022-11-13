using System.ComponentModel.DataAnnotations;

namespace BrainBox.Data.DTOs.Auth
{
    public class TokenRequestDTO
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
