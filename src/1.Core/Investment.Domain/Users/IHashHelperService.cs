namespace Investment.Domain.Users
{
    public interface IHashHelperService
    {
        string HashRefreshToken(string refreshToken);

        string HashPassword(string password);
        string HashVerificationToken(string token);
    }
}