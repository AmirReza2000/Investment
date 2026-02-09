using Investment.Domain.Common;
using Investment.Domain.Users.Enums;
using Investment.Domain.Users.Rules;

namespace Investment.Domain.Users.Entities
{
    public class ValidationToken:Entity<int>
    {
        private ValidationToken()
        {
            
        }

        internal ValidationToken(string hashedToken, DateTime expireAt, ValidationTokenTypeEnum validationTokenType)
        {
            HashedToken = hashedToken;
            ExpireAt = expireAt;
            ValidationTokenType = validationTokenType;
        }

        public string HashedToken { get; private set; } = string.Empty;

        public bool Revoked { get; private set; }=false;

        public  DateTime ExpireAt { get; private set; }

        public  ValidationTokenTypeEnum ValidationTokenType { get;private set; }

        public int UserId{ get;private set; }

        public User? User { get; private set; }

        internal void AssertTokenValid(string hashedVerifyToken)
        {

            CheckRule(new ValidVerificationTokenRule(this, hashedVerifyToken));
           
        }
        
        internal void RevokeToken()
        {
            Revoked = true;
        }   
    }
}