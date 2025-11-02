using PensionFund.Domain.Entities;

namespace PensionFund.Application.Models;
public class TransactionListDto
{
    public List<Transactions> Transactions { get; set; } = [];
}
