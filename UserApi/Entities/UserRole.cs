using System.Data;

namespace UserApi.Entities;

public class UserRole: BaseEntity<int>
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public int RoleId { get; set; }
    public Role? Role { get; set; }
}