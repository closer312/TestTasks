namespace UserApi.Models.Users;

public class UserDto
{
    public string Pin { get; set; } = string.Empty;
    public string Fio { get; set; } = string.Empty;
    public UserParentDto? Parent { get; set; }
    public List<UserChildDto> Children { get; set; }
    public List<RoleDto> Roles { get; set; }
}
