using System.ComponentModel.DataAnnotations;

namespace UserApi.Models.Login;

public class LoginRequest
{
    [Required]
    public string Pin { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}
