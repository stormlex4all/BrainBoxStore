namespace BrainBox.Data.DTOs.Auth
{
    public class RefreshTokenDTO
    {
        public long Id { get; set; }

        public string Token { get; set; }

        public string JwtId { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime ExpireAt { get; set; }

        public string UserEmail { get; set; }
    }
}
