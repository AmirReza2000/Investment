using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Investment.Application.CryptoPayment.Commands.Deposit;

public class DepositWithCryptoCommand : IRequest<DepositWithCryptoResponse>
{
    public required decimal PriceAmount { get; set; }
}
