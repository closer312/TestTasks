using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace UserApi.Entities;

[Index(nameof(PinMother), IsUnique = true)]
[Index(nameof(PinFather), IsUnique = true)]
public class UserParent : BaseEntity<int>
{
    [StringLength(14, MinimumLength = 14, ErrorMessage = "PIN length must be 14 characters.")]
    public string PinMother { get; set; } = string.Empty;

    [StringLength(14, MinimumLength = 14, ErrorMessage = "PIN length must be 14 characters.")]
    public string PinFather { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User User { get; set; }
}