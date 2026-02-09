using Investment.Application.CryptoPayment.Queries.GetMinimumPaymentAmount;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.DTO.Nowpayment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Investment.Application.CryptoPayment.Queries.MinimumPaymentAmount;

public class MinimumPaymentAmountQueryHandler(ICryptoPaymentsService cryptoPaymentsService)
    : IRequestHandler<MinimumPaymentAmountQuery, MinimumPaymentAmountQueryResponse>
{
    public async Task<MinimumPaymentAmountQueryResponse> Handle(MinimumPaymentAmountQuery request, CancellationToken cancellationToken)
    {
        var responseService = await cryptoPaymentsService.GetMinimumPaymentAmountAsync(cancellationToken);

        return
            new MinimumPaymentAmountQueryResponse
            {
                MinAmount = responseService.MinAmount,
                CurrencyFrom = responseService.CurrencyFrom,
                CurrencyTo = responseService.CurrencyTo,
                FiatEquivalent = responseService.FiatEquivalent
            };
    }
}
