namespace BrainBox.Data.DTOs.Auth
{
    public class TokenDTO
    {
        public string? Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public string? RefreshToken { get; set; }
    }
}
