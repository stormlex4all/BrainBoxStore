namespace BrainBox.Data.Models
{
    public class BaseModel
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
