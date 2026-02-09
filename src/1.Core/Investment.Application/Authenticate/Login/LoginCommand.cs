using MediatR;

namespace Investment.Application.Authenticate.Login;

public class LoginCommand : IRequest<LoginResponse>
{
    public required string EmailAddress { get; set; }
    public required string Password { get; set; }
    public string? deviceName { get; set; }
    public string? userAgent { get; set; }
    public string? ipAddress { get; set; }
}
