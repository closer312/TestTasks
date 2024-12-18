using System.ComponentModel.DataAnnotations;

namespace UserApi.Models.Users;

public class RegisterResponse
{
    [Required]
    public string Pin { get; set; } = string.Empty;
    [Required]
    public string Fio { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

}
