namespace Investment.Application.Authenticate.Login;

public class LoginResponse
{
    public required string AccessKey { get; set; }
    public required string RefreshToken { get; set; }
}