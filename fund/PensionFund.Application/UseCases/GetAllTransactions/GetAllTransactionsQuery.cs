using FluentResults;
using MediatR;
using PensionFund.Application.Extensions;
using PensionFund.Application.Interfaces;
using PensionFund.Application.Models;

namespace PensionFund.Application.UseCases.GetAllTransactions;
public class GetAllTransactionsQuery : IRequest<Result<TransactionListDto>>
{
    public string Date { get; set; } = default!;
}

public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, Result<TransactionListDto>>
{
    private readonly IStorageService storageService;

    public GetAllTransactionsQueryHandler(
        IStorageService storageService)
    {
        this.storageService = storageService;
    }

    public async Task<Result<TransactionListDto>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await this.storageService.GetTransactions(request.Date);

        return transactions.ToGetTransactionDto();
    }
}
