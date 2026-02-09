using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Windows.Input;
using MediatR;

namespace Investment.Application.CryptoPayment.Commands.Withdrawal;

public class WithdrawalCommand : IRequest<WithdrawalResponse>
{
    public required string AddressWithdrawal { get; set; }

    public required decimal Amount { get; set; }
}
