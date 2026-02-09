using Investment.Application.CryptoPayment.Queries.MinimumPaymentAmount;
using Investment.Application.Utilities.DTO.Nowpayment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.CryptoPayment.Queries.GetMinimumPaymentAmount;

public class MinimumPaymentAmountQuery : IRequest<MinimumPaymentAmountQueryResponse>
{
}
