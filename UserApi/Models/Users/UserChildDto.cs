using System.ComponentModel.DataAnnotations;

namespace UserApi.Models.Users;

public class UserChildDto
{
    public int Id { get; set; }
    public string Fio { get; set; } = string.Empty;
    [StringLength(14, MinimumLength = 14, ErrorMessage = "PIN length must be 14 characters.")]
    public string Pin { get; set; } = string.Empty;

}
