using PensionFund.Application.Models;
using PensionFund.Domain.Entities;

namespace PensionFund.Application.Extensions;
public static class TransactionDtoExtensions
{
    public static TransactionListDto ToGetTransactionDto(
        this List<Transactions> transactions)
    {
        return new TransactionListDto
        {
            Transactions = transactions
        };
    }
}
