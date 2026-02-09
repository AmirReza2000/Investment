using Investment.Application.Utilities;
using Investment.Application.Utilities.Abstractions;
using Investment.Application.Utilities.Configs;
using Investment.Application.Utilities.Exceptions;
using Investment.Domain.Transactions.Entities;
using Investment.Domain.Transactions.Enums.Transaction;
using Investment.Domain.Transactions.Repositories;
using MediatR;

namespace Investment.Application.CryptoPayment.Commands.Deposit;

public class DepositWithCryptoCommandHandler(
    ICryptoPaymentsService cryptoPaymentsService,
    IUserInfoService userInfoService,
    IUserBalanceCommandRepository userBalanceRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DepositWithCryptoCommand, DepositWithCryptoResponse>
{

    public async Task<DepositWithCryptoResponse> Handle(DepositWithCryptoCommand request, CancellationToken cancellationToken)
    {

        await cryptoPaymentsService.AssertApiIsAvailableAsync(cancellationToken);

        var minimumPaymentAmountResponse = await cryptoPaymentsService.GetMinimumPaymentAmountAsync(cancellationToken);

        if (minimumPaymentAmountResponse.FiatEquivalent > request.PriceAmount)
        {
            throw new InvalidCommandException(
                "The amount is less than the minimum required.",
                $"Minimum amount is {minimumPaymentAmountResponse.FiatEquivalent}",
                ErrorCodeEnum.None);
        }

        int userId = userInfoService.GetUserId();

        var paymentResponse = await cryptoPaymentsService.DepositRequestAsync(
              request.PriceAmount,
              userId,
              cancellationToken
        );

        var userBalance = await userBalanceRepository.GetUserBalanceByUserIdAsync(userId, cancellationToken)
            ?? throw new EntityNotFoundException<UserBalance>();

        var transaction = userBalance.CreateDeposit(
            amount: request.PriceAmount,
            paymentId: paymentResponse.PaymentId.ToString(),
            paymentAddress: paymentResponse.PayAddress
        );

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new DepositWithCryptoResponse
        {
            PayAddress = paymentResponse.PayAddress,
            PaymentId = paymentResponse.PaymentId,
            transactionId = transaction.Id,
            PayAmount = paymentResponse.PayAmount
        };
    }
}
