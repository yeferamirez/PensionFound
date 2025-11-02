using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using PensionFund.Application.Constants;
using PensionFund.Application.Exceptions.Errors;
using PensionFund.Application.Interfaces;
using PensionFund.Domain.Entities;

namespace PensionFund.Application.UseCases.CancelSubscription;
public class UnsubscribeCommand : IRequest<Result<Unit>>
{
    public string ClientName { get; set; } = default!;
    public string ClientLastName { get; set; } = default!;
    public string City { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public string ProductType { get; set; } = default!;
}

public class UnsubscribeCommandHandler : IRequestHandler<UnsubscribeCommand, Result<Unit>>
{
    private readonly IStorageService storageService;
    private readonly TimeProvider timeProvider;
    public readonly ILogger<UnsubscribeCommandHandler> logger;

    public UnsubscribeCommandHandler(
       TimeProvider timeProvider,
       IStorageService storageService,
       ILogger<UnsubscribeCommandHandler> logger
       )
    {
        this.storageService = storageService;
        this.logger = logger;
        this.timeProvider = timeProvider;
    }

    public async Task<Result<Unit>> Handle(UnsubscribeCommand request, CancellationToken cancellationToken)
    {
        var transaction = new Transactions
        {
            ClientName = $"{request.ClientName} {request.ClientLastName}",
            ModificationDate = timeProvider.GetUtcNow().DateTime.ToString("yyyy-MM-dd"),
            ProductCity = request.City,
            ProductName = request.ProductName,
            ProductType = request.ProductType,
            State = TransactionConstants.REMOVED,
            Value = 0
        };

        var Unsubscribe = await this.storageService.GetTransactionsAsync(transaction.ClientName);

        if (Unsubscribe != null &&
            Unsubscribe.ClientName.Equals(transaction.ClientName) &&
            Unsubscribe.ProductName.Equals(transaction.ProductName) &&
            Unsubscribe.ProductType.Equals(transaction.ProductType) &&
            Unsubscribe.ProductCity.Equals(transaction.ProductCity))
        {
            transaction.Id = Unsubscribe.Id;
            await this.storageService.SaveTransaction(transaction);
            var client = await this.storageService.GetClientAsync(transaction.ClientName);
            client.InitialValue = client.InitialValue + Unsubscribe.Value;
            await this.storageService.SaveClient(client);

            return Unit.Value;
        }

        logger.LogInformation("UnSubcripted Fail - ProductName {ProductName}", transaction.ProductName);

        return Result.Fail(new NotFoundError("El usuario no pertenece a ese fondo"));
    }
}
