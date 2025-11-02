using PensionFund.Domain.Entities;

namespace PensionFund.Application.Interfaces;
public interface IStorageService
{
    Task<Transactions?> GetTransactionsAsync(string id);
    Task<FundConfiguration?> GetConfigurationAsync(string id);
    Task<ClientInformation?> GetClientAsync(string id);
    Task SaveClient(ClientInformation client);
    Task SaveTransaction(Transactions model);
    Task<List<Transactions>?> GetTransactions(string model);
    Task<List<FundConfiguration>> GetConfiguration();
}
