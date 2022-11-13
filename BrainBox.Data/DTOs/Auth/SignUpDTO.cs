using BrainBox.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace BrainBox.Data.DTOs.Auth
{
    public class SignUpDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        /// <summary>
        /// User type <see cref="Enums.BrainBoxUserType"/>
        /// </summary>
        [Required]
        public BrainBoxUserType UserType { get; set; }
    }
}
