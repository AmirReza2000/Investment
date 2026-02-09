using Investment.Domain.Common;
using Investment.Domain.Contracts.Entities;

namespace Investment.Domain.Contracts.Rules
{
    internal class ContractMarketTypeMustBeActiveRule : IBusinessRule
    {
        private ContractMarketType _contractMarketType;

        public ContractMarketTypeMustBeActiveRule(ContractMarketType contractMarketType)
        {
            this._contractMarketType = contractMarketType;
        }

        public string Message => "contract market Type must be active.";

        public BusinessRuleCodeEnum BusinessRuleCode => BusinessRuleCodeEnum.ContractMarketTypeMustBeActive;

        public bool IsBroken() => !_contractMarketType.IsActive;
    }
}