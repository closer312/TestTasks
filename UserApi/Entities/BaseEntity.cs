namespace UserApi.Entities;

public class BaseEntity<Tkey>
{
    public Tkey Id { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateDate { get; set; }
}
