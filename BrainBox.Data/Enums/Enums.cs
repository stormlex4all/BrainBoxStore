using System.ComponentModel;

namespace BrainBox.Data.Enums
{
    public enum CachePrefix
    {
        RefreshToken,

        UserEmail
    }

    public enum Service
    {
        BrainBoxAPI,

        Authentication
    }

    /// <summary>
    /// Vendor type = 0, Cutomer type = 1
    /// </summary>
    public enum BrainBoxUserType
    {
        [Description("A Vendor user")]
        Vendor,

        [Description("A Customer user")]
        Customer
    }
}
