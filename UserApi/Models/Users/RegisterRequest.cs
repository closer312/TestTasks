using System.ComponentModel.DataAnnotations;

namespace UserApi.Models.Users;

public class RegisterRequest
{
    [Required]
    [StringLength(14, MinimumLength = 14, ErrorMessage = "PIN length must be 14 characters.")]
    public string Pin { get; set; } = string.Empty;
    [Required]
    public string Fio { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    public UserParentDto? Parent { get; set; }
    public List<UserChildDto> Children { get; set; }
    public List<RoleDto> Roles { get; set; }

}