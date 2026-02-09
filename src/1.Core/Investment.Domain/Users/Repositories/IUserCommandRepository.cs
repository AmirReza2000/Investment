using Investment.Domain.Common;
using Investment.Domain.Users.Entities;

namespace Investment.Domain.Users.Repositories;

public interface IUserCommandRepository : ICommandRepository<User, int>
{
    Task AssertUserNotExistAsync(string emailAddress, CancellationToken cancellationToken);
    Task<User?> GetUserByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken);
    Task<User?> GetUserByEmailWithValidRefreshTokensAsync(string emailAddress, CancellationToken cancellationToken);
    Task<User?> GetUserByIdWithValidRefreshTokensAsync(int userId, CancellationToken cancellationToken);
    Task<User?> GetUserWithSpecifiedValidationTokenAsync(string emailAddress, string hashedVerifyToken, CancellationToken cancellationToken);
    Task<User?> GetUserWithRestPasswordValidTokensAsync(string emailAddress, CancellationToken cancellationToken);
    Task<bool> IsTokenResetPasswordValidAsync(string emailAddress, string hashedToken, CancellationToken cancellationToken);
    Task<User?> GetUserForResetPasswordAsync(string emailAddress, string hashedToken, CancellationToken cancellationToken);
    Task<User?> GetUserWithVerifyValidTokensAsync(string emailAddress, CancellationToken cancellationToken);
    Task<User?> GetUserForRotateRefreshTokensAsync(int userId, int refreshTokenId, CancellationToken cancellationToken);
    Task<User?> GetUserForLogoutAsync(int userId, int refreshTokenId,CancellationToken cancellationToken);
}
