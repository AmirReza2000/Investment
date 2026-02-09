using System;
using System.Collections.Generic;
using System.Text;
using Investment.Application.Utilities;
using Investment.Application.Utilities.Exceptions;
using Investment.Application.Utilities.Resources;
using Investment.Domain.Users.Entities;
using Investment.Domain.Users.Enums;
using Investment.Domain.Users.Repositories;
using Investment.Infrastructure.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace Investment.Infrastructure.Domain.Users.Repositories;

public class UserCommandRepository :
    CommandRepository<User, int>, IUserCommandRepository
{
    private readonly InvestmentDbContext _dbContext;

    public UserCommandRepository(InvestmentDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AssertUserNotExistAsync(string emailAddress, CancellationToken cancellationToken)
    {
        var userIsExist =
            await _dbContext.Users
             .AnyAsync(user => user.EmailAddress == emailAddress, cancellationToken);

        if (userIsExist)
        {
            throw
                new EntityDuplicateException<User>(nameof(User.EmailAddress));
        }
    }

    public Task<User?> GetUserByEmailAddressAsync(string username, CancellationToken cancellationToken)
    {
        var user = _dbContext.Users
            .FirstOrDefaultAsync(user => user.EmailAddress == username, cancellationToken);

        return user;
    }

    private Task<User?> GetUserWithSpecifiedRefreshTokensAsync(int userId, int refreshTokenId, CancellationToken cancellationToken)
    {
        var user = _dbContext.Users
            .Include(u => u.RefreshTokens.Where(refreshToken =>
            refreshToken.Id == refreshTokenId && refreshToken.Revoked == false))
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return user;
    }

    public Task<User?> GetUserForRotateRefreshTokensAsync(int userId, int refreshTokenId, CancellationToken cancellationToken)
    {

        var user = GetUserWithSpecifiedRefreshTokensAsync(userId, refreshTokenId, cancellationToken);

        return user;
    }

    public Task<User?> GetUserForLogoutAsync(int userId,int  refreshTokenId, CancellationToken cancellationToken)
    {
        var user=GetUserWithSpecifiedRefreshTokensAsync(userId,refreshTokenId, cancellationToken);

        return user;
    }

    public Task<User?> GetUserByEmailWithValidRefreshTokensAsync(string emailAddress, CancellationToken cancellationToken)
    {
        var user = _dbContext.Users
                  .Include(user => user.RefreshTokens.Where(refreshToken => refreshToken.Revoked == false && refreshToken.ExpiresAt > DateTime.Now))
                  .FirstOrDefaultAsync(user => user.EmailAddress == emailAddress, cancellationToken);

        return user;
    }

    public Task<User?> GetUserByIdWithValidRefreshTokensAsync(int userId, CancellationToken cancellationToken)
    {
        var user = _dbContext.Users
                  .Include(user => user.RefreshTokens.Where(refreshToken => refreshToken.Revoked == false && refreshToken.ExpiresAt > DateTime.Now))
                  .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);

        return user;
    }

    public Task<User?> GetUserWithSpecifiedValidationTokenAsync(string emailAddress, string hashedVerifyToken, CancellationToken cancellationToken)
    {
        var user = _dbContext.Users.Where(user => user.EmailAddress == emailAddress)
            .Include(u => u.ValidationTokens.Where(validationToken =>
            validationToken.HashedToken == hashedVerifyToken && validationToken.Revoked == false))
            .FirstOrDefaultAsync(cancellationToken);

        return user;
    }

    public Task<User?> GetUserWithRestPasswordValidTokensAsync(string emailAddress, CancellationToken cancellationToken)
    {
        return _dbContext.Users.Where(user => user.EmailAddress == emailAddress)
            .Include(user => user.ValidationTokens.Where(validationToken =>
            validationToken.Revoked == false && validationToken.ExpireAt > DateTime.Now &&
            validationToken.ValidationTokenType == ValidationTokenTypeEnum.ResetPassword)).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<User?> GetUserWithVerifyValidTokensAsync(string emailAddress, CancellationToken cancellationToken)
    {
        return _dbContext.Users.Where(user => user.EmailAddress == emailAddress)
            .Include(user => user.ValidationTokens.Where(validationToken =>
            //validationToken.Revoked == false && validationToken.ExpireAt > DateTime.Now &&
            validationToken.ValidationTokenType == ValidationTokenTypeEnum.UserActivation)
            )
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> IsTokenResetPasswordValidAsync(string emailAddress, string hashedToken, CancellationToken cancellationToken)
    {
        return _dbContext.Users
             .Include(user => user.ValidationTokens)
             .AnyAsync(user => user.EmailAddress == emailAddress && user.ValidationTokens.
             Any(validationToken => validationToken.HashedToken == hashedToken && validationToken.Revoked == false && validationToken.ExpireAt > DateTime.Now &&
             validationToken.ValidationTokenType == ValidationTokenTypeEnum.ResetPassword), cancellationToken);
    }

    public Task<User?> GetUserForResetPasswordAsync(string emailAddress, string hashedVerifyToken, CancellationToken cancellationToken)
    {
        var user = _dbContext.Users.Where(user => user.EmailAddress == emailAddress)
            .Include(u => u.ValidationTokens.Where(validationToken =>
            validationToken.HashedToken == hashedVerifyToken && validationToken.Revoked == false))
            .Include(user => user.RefreshTokens.Where(refreshToken => refreshToken.ExpiresAt >= DateTime.Now && refreshToken.Revoked == false))
            .FirstOrDefaultAsync(user => user.ValidationTokens.Any(), cancellationToken);

        return user;
    }
}
