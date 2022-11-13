namespace BrainBox.Data.DTOs
{
    public interface ICurrentActiveToken
    {
        string Token { get; set; }
    }
    public class CurrentActiveToken : ICurrentActiveToken
    {
        public string Token { get; set; }
    }
}
