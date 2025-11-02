using FluentResults;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using PensionFund.Application.Configuration;
using PensionFund.Application.Constants;
using PensionFund.Application.Exceptions.Errors;
using PensionFund.Application.Interfaces;
using PensionFund.Domain.Entities;

namespace PensionFund.Application.UseCases.CreateSubscription;
public class CreateSubcriptionCommand : IRequest<Result<Unit>>
{
    public string ClientName { get; set; } = default!;
    public string ClientLastName { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string NotificationType { get; set; } = default!;
    public string ProductType { get; set; } = default!;
    public string ProductCity { get; set; } = default!;
    public int Value { get; set; }
}

public class CreateSubcriptionCommandHandler : IRequestHandler<CreateSubcriptionCommand, Result<Unit>>
{

    private readonly IStorageService storageService;
    private readonly TimeProvider timeProvider;
    public readonly ILogger<CreateSubcriptionCommandHandler> logger;
    private readonly IPublishEndpoint publishEndpoint;

    public CreateSubcriptionCommandHandler(
        TimeProvider timeProvider,
        IStorageService storageService,
        ILogger<CreateSubcriptionCommandHandler> logger,
        IPublishEndpoint publishEndpoint
        )
    {
        this.storageService = storageService;
        this.logger = logger;
        this.timeProvider = timeProvider;
        this.publishEndpoint = publishEndpoint;
    }

    public async Task<Result<Unit>> Handle(CreateSubcriptionCommand request, CancellationToken cancellationToken)
    {
        var transaction = new Transactions
        {
            Id = Guid.NewGuid().ToString(),
            ClientName = $"{request.ClientName} {request.ClientLastName}",
            ProductName = request.ProductName,
            ProductType = request.ProductType,
            ProductCity = request.ProductCity,
            Value = request.Value,
            State = TransactionConstants.ACTIVATED,
            ModificationDate = timeProvider.GetUtcNow().DateTime.ToString("yyyy-MM-dd"),
        };

        var subscribed = await this.storageService.GetTransactionsAsync(transaction.ClientName);

        if (subscribed != null &&
            subscribed.ClientName.Equals(transaction.ClientName) &&
            subscribed.ProductName.Equals(transaction.ProductName) &&
            subscribed.ProductType.Equals(transaction.ProductType) &&
            subscribed.ProductCity.Equals(transaction.ProductCity))
        {
            return Result.Fail(new NotFoundError("El usuario ya pertenece a ese fondo"));
        }

        var fundConfiguration = await storageService.GetConfigurationAsync(transaction.ProductName);
        var client = await storageService.GetClientAsync(transaction.ClientName);

        if (fundConfiguration != null && client == null)
        {
            client = new ClientInformation
            {
                ClientName = transaction.ClientName,
                City = transaction.ProductCity,
                InitialValue = 500000
            };
        }

        if (fundConfiguration != null && fundConfiguration.MinimumCost == transaction.Value)
        {
            client.InitialValue = client.InitialValue - (subscribed == null ? transaction.Value : subscribed.Value);
            if (client.InitialValue <= 0)
                return Result.Fail(new NotFoundError("El usuario no posee el monto suficiente"));
            await this.storageService.SaveClient(client);
            await this.storageService.SaveTransaction(transaction);

            await this.publishEndpoint.Publish(new Subscripted(request.NotificationType, request.Email, request.PhoneNumber));

            return Unit.Value;
        }

        logger.LogInformation("Subcription Fail - ProductName {ProductName}", transaction.ProductName);

        return Result.Fail(new NotFoundError("No tiene saldo disponible para vincularse al fondo"));
    }
}

