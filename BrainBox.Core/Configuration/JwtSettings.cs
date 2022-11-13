namespace BrainBox.Core.Configuration
{
    public interface IJwtSettings
    {
        int TokenValidPeriodMinutes { get; set; }

        string Secret { get; set; }
    }

    public class JwtSettings : IJwtSettings
    {
        public string Secret { get; set; }

        public int TokenValidPeriodMinutes { get; set; }
    }
}
