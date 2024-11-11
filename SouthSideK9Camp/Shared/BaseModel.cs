namespace SouthSideK9Camp.Shared;

public abstract class BaseModel
{
    public int ID { get; set; }
    public Guid GUID { get; set; } = Guid.NewGuid();
    public DateTime DateCreated { get; set; } = DateTime.UtcNow.AddHours(8);
    public bool IsDeleted { get; set; }
}
