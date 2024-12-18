namespace UserApi.Models.Login;

public class LoginResponse
{
    public bool IsLogedIn { get; set; } = false;
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;

}