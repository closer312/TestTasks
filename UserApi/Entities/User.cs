using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UserApi.Entities;
[Index(nameof(Pin), IsUnique = true)]
public class User : BaseEntity<Guid>
{
    [StringLength(14, MinimumLength = 14, ErrorMessage = "PIN length must be 14 characters.")]
   
    public string Pin { get; set; } = string.Empty;
    public string Fio { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    public List<UserRole> UserRoles { get; set; }
    public List<UserChild> UserChildren { get; set; }
    public UserParent UserParent { get; set; }
}
