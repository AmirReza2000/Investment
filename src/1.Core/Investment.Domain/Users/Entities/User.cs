using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Investment.Domain.Common;
using Investment.Domain.Common.Exceptions;
using Investment.Domain.Contracts.Entities;
using Investment.Domain.Users.Enums;
using Investment.Domain.Users.Enums.User;
using Investment.Domain.Users.Rules;
using MediatR;

namespace Investment.Domain.Users.Entities;

public class User : AggregateRoot<int>
{
    public string HashedPassword { get; private set; } = string.Empty;

    public string EmailAddress { get; private set; } = string.Empty;

    public RoleEnum Role { get; private set; }

    public UserStatusEnum UserStatus { get; private set; }

    public DateTime CreateDatetime { get; private set; }

    private List<ValidationToken> _validationTokens = [];

    public IReadOnlyList<ValidationToken> ValidationTokens => _validationTokens.AsReadOnly();

    private List<RefreshToken> _refreshTokens = [];

    public IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private User()
    {
    }

    private User(string emailAddress, string hashedPassword)
    {
        HashedPassword = hashedPassword;
        EmailAddress = emailAddress;
        CreateDatetime = DateTime.Now;
        UserStatus = UserStatusEnum.NotVerified;
    }

    public static User Create(string emailAddress,
        string password, IHashHelperService hashHelperService)
    {
        var hashedPassword = hashHelperService.HashPassword(password);

        return new User(
                        emailAddress: emailAddress,
                        hashedPassword: hashedPassword);
    }

    public void Login(string password, IHashHelperService hashHelperService)
    {
        var hashedPassword = hashHelperService.HashPassword(password);

        CheckRule(new ValidUsernameAndPasswordRule(this, hashedPassword));

        CheckRule(new UserMustBeActiveForLoginRule(this));

    }

    public RefreshToken SetRefreshToken(
        string refreshToken, DateTime refreshTokenExpiresAt,
        string? deviceName, string? userAgent, string? ipAddress,
        IHashHelperService hashHelperService)
    {
        var activeRefreshTokens = _refreshTokens.Where(refreshToken =>
        refreshToken.ExpiresAt > DateTime.Now && refreshToken.Revoked == false);

        if (activeRefreshTokens.Count() > 1000)
            throw new NotImplementedException();

        var refreshTokenHash = hashHelperService.HashRefreshToken(refreshToken);

        var refreshTokenNew = RefreshToken.Create
            (
             tokenHash: refreshTokenHash,
             expiresAt: refreshTokenExpiresAt,
             deviceName: deviceName,
             userAgent: userAgent,
             ipAddress: ipAddress
            );

        _refreshTokens = [.. _refreshTokens.Append(refreshTokenNew)];

        return refreshTokenNew;
    }

    internal bool IsActive()
    {
        return UserStatus == UserStatusEnum.Active;
    }

    public int? RotateRefreshToken(int refreshTokenId,
        string oldRefreshTokenString,
        string newRefreshTokenString,
        IHashHelperService hashHelperService)
    {
        var refreshTokenEntity = _refreshTokens.FirstOrDefault(rt => rt.Id == refreshTokenId);

        if (refreshTokenEntity == null) return null;

        string oldRefreshTokenHashString = hashHelperService.HashRefreshToken(oldRefreshTokenString);

        CheckRule(new RefreshTokenHashValidationRule(refreshTokenEntity, oldRefreshTokenHashString));

        CheckRule(new RefreshTokenValidationRule(refreshTokenEntity));

        CheckRule(new UserMustBeActiveForLoginRule(this));

        var newRefreshTokenHash = hashHelperService.HashRefreshToken(newRefreshTokenString);

        var newRefreshTokenEntity = refreshTokenEntity.Rotate(newRefreshTokenHash);

        return newRefreshTokenEntity.Id;
    }

    public void SetEmailVerification
        (string token, DateTime expireAt, IHashHelperService hashHelperService)
    {
        if (UserStatus != UserStatusEnum.NotVerified)
            return;

        var hashedToken = hashHelperService.HashVerificationToken(token);

        _validationTokens.Add(
            new ValidationToken(hashedToken,
            expireAt,
            ValidationTokenTypeEnum.UserActivation));
    }

    public void Verify(string hashedVerifyToken)
    {

        var ValidationTokenEntity =
            ValidationTokens.FirstOrDefault(token => token.HashedToken == hashedVerifyToken &&
                   token.ValidationTokenType == ValidationTokenTypeEnum.UserActivation);

        if (ValidationTokenEntity == null)
            throw new BusinessRuleValidationException(
               new ValidVerificationTokenRule(null!, null!));

        CheckRule(new UserMustBeNotVerifiedRule(this));

        ValidationTokenEntity.AssertTokenValid(hashedVerifyToken);

        ValidationTokenEntity.RevokeToken();

        UserStatus = UserStatusEnum.Active;
    }

    public void SetRestPasswordToken
        (string token, DateTime expireAt, IHashHelperService hashHelperService)
    {

        CheckRule(new ValidRestPasswordTokenShouldNotExistRule(this));

        CheckRule(new UserMustBeVerifiedRule(this));

        var validationTokenEntity = new ValidationToken(
                    hashedToken: hashHelperService.HashVerificationToken(token),
                    expireAt: expireAt,
                    ValidationTokenTypeEnum.ResetPassword);

        _validationTokens.Add(validationTokenEntity);

    }

    public void RestPassword(string newPassword, string hashedToken, IHashHelperService hashHelperService)
    {
        var ValidationTokenEntity =
                   ValidationTokens.FirstOrDefault(token => token.HashedToken == hashedToken &&
                   token.ValidationTokenType == ValidationTokenTypeEnum.ResetPassword);

        if (ValidationTokenEntity == null)
            throw new BusinessRuleValidationException(
               new ValidVerificationTokenRule(null!, null!));

        ValidationTokenEntity.AssertTokenValid(hashedToken);

        ValidationTokenEntity.RevokeToken();

        HashedPassword = hashHelperService.HashPassword(newPassword);

        RevokeAllRefreshTokens();
    }

    private void RevokeAllRefreshTokens()
    {
        foreach (var refreshToken in RefreshTokens)
        {
            refreshToken.Revoke();
        }
    }

    public void SetUserVerifyToken(
        string token, DateTime expireAt, IHashHelperService hashHelperService)
    {
        CheckRule(new ValidVerifyTokenShouldNotExistRule(this));

        CheckRule(new UserMustBeNotVerifiedRule(this));

        var validationTokenEntity = new ValidationToken(
                  hashedToken: hashHelperService.HashVerificationToken(token),
                  expireAt: expireAt,
                  ValidationTokenTypeEnum.UserActivation);

        _validationTokens.Add(validationTokenEntity);
    }

    public void ChangePassword(string currentPassword, string newPassword, IHashHelperService hashHelperService)
    {
        var hashedPassword = hashHelperService.HashPassword(currentPassword);

        CheckRule(new ValidUsernameAndPasswordRule(this, hashedPassword));

        CheckRule(new UserMustBeActiveForLoginRule(this));

        HashedPassword = hashHelperService.HashPassword(newPassword);

        RevokeAllRefreshTokens();
    }

    public void Logout(int refreshTokenId)
    {
        var refreshTokenEntity = _refreshTokens.FirstOrDefault(rt => rt.Id == refreshTokenId)
            ??throw new BusinessRuleValidationException(new GeneralBusinessRule(
                $"session with id:{refreshTokenId} not found.",BusinessRuleCodeEnum.EntityNotFound));        

        CheckRule(new RefreshTokenValidationRule(refreshTokenEntity!));

        refreshTokenEntity.Revoke();
    }
}
