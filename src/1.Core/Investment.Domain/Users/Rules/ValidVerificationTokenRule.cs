using Investment.Domain.Common;
using Investment.Domain.Users.Entities;

namespace Investment.Domain.Users.Rules;

public class ValidVerificationTokenRule : IBusinessRule
{
    private readonly ValidationToken _validationToken;
    private readonly string _hashedVerifyToken;

    public ValidVerificationTokenRule(ValidationToken validationToken, string hashedVerifyToken)
    {
        this._validationToken = validationToken;
        this._hashedVerifyToken = hashedVerifyToken;
    }

    public string Message =>  "Token isn't valid.";

    public bool IsBroken() 
    {
        if(_validationToken.HashedToken!=_hashedVerifyToken)
            return true;

        if(_validationToken.Revoked==true||_validationToken.ExpireAt<=DateTime.Now )
            return true;        

        return false;
    }

    public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.ValidVerificationToken;

}