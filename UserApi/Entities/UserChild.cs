using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UserApi.Entities;

[Index(nameof(Pin), IsUnique = true)]
public class UserChild : BaseEntity<int>
{
    [StringLength(14, MinimumLength = 14, ErrorMessage = "PIN length must be 14 characters.")]
    public string Pin { get; set; } = string.Empty;
    public string Fio { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User? User { get; set; }
}