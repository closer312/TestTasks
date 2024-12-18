namespace UserApi.Entities;

public class Role : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public List<UserRole> UserRoles { get; set; }
}