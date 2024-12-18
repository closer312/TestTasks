using System.ComponentModel.DataAnnotations;

namespace UserApi.Models.Users;

public class UserParentDto
{
    [StringLength(14, MinimumLength = 14, ErrorMessage = "PIN length must be 14 characters.")]
    public string PinMother { get; set; } = string.Empty;

    [StringLength(14, MinimumLength = 14, ErrorMessage = "PIN length must be 14 characters.")]
    public string PinFather { get; set; } = string.Empty;

}
