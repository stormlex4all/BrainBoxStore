namespace POSSAP.Core.Configuration
{
    public interface IAPISettings
    {
        string AESEncryptionSecret { get; set; }

        int RefreshTokenValidPeriodMonths { get; set; }
    }

    public class APISettings : IAPISettings
    {
        public string AESEncryptionSecret { get; set; }

        public int RefreshTokenValidPeriodMonths { get; set; }
    }
}
