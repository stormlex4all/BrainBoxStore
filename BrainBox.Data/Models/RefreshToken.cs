
namespace BrainBox.Data.Models
{
    public class RefreshToken : BaseModel
    {
        public string Token { get; set; }

        public string JwtId { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime ExpireAt { get; set; }

        public string UserEmail { get; set; }
    }
}
