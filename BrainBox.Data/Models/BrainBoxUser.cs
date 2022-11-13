using Microsoft.AspNetCore.Identity;

namespace BrainBox.Data.Models
{
    public class BrainBoxUser : IdentityUser
    {
        /// <summary>
        /// User type <see cref="Enums.BrainBoxUserType"/>
        /// </summary>
        public int UserType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string? UpdatedBy { get; set; }
    }
}
