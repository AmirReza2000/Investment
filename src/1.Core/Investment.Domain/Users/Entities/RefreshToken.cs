using System;
using System.Collections.Generic;
using System.Text;
using Investment.Domain.Common;

namespace Investment.Domain.Users.Entities;

public class RefreshToken : Entity<int>
{

    private RefreshToken()
    {

    }

    private RefreshToken(
        string refreshTokenHash,
        DateTime expiresAt,
        string? deviceName,
        string? userAgent,
        string? ipAddress,
        int? replacedByTokenId)
    {

        RefreshTokenHash = refreshTokenHash;
        ExpiresAt = expiresAt;
        DeviceName = deviceName;
        UserAgent = userAgent;
        IpAddress = ipAddress;
        ReplacedByTokenId = replacedByTokenId;
        CreatedAt = DateTime.Now;
    }

    internal static RefreshToken Create(string tokenHash, DateTime expiresAt,
          string? deviceName, string? userAgent, string? ipAddress)
    {
        return new RefreshToken
            (
             refreshTokenHash: tokenHash,
             expiresAt: expiresAt,
             deviceName: deviceName,
             userAgent: userAgent,
             ipAddress: ipAddress,
             replacedByTokenId: null
            );
    }

    internal void Revoke()
    {
        if (Revoked == false && ExpiresAt >=DateTime.Now)
        {
            Revoked = true;
            RevokedAt = DateTime.Now;
        }
    }

    internal RefreshToken Rotate(string refreshTokenHash)
    {
        Revoked = true;
        RevokedAt = DateTime.Now;
        ReplacedByToken
        = new RefreshToken
            (
                 refreshTokenHash: refreshTokenHash,
                 expiresAt: this.ExpiresAt,
                 deviceName: this.DeviceName,
                 userAgent: this.UserAgent,
                 ipAddress: this.IpAddress,
                 null
            );

        ReplacedByToken.UserId = UserId;
        ReplacedByToken.User = User;

        return ReplacedByToken;
    }

    internal bool IsRefreshTokenHashValid(string refreshTokenHashString)
        => RefreshTokenHash == refreshTokenHashString;

    // FK to user
    public int UserId { get; private set; }

    public User User { get; private set; } = default!;

    public string RefreshTokenHash { get; private set; } = default!;

    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    public bool Revoked { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public RefreshTokenRevokeReason? RevokeReason { get; private set; }

    // optional metadata for UI
    public string? DeviceName { get; private set; }        // e.g. "Chrome on Windows"
    public string? UserAgent { get; private set; }
    public string? IpAddress { get; private set; }

    // rotation chain (optional but useful)
    public int? ReplacedByTokenId { get; set; }
    public RefreshToken? ReplacedByToken { get; set; }
}
