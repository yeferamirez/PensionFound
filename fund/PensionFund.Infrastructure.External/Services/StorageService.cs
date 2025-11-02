using PensionFund.Application.Interfaces;
using PensionFund.Domain.Entities;
using PensionFund.Domain.Interfaces.Repositories;

namespace PensionFund.Infrastructure.External.Services;
public class StorageService : IStorageService
{
    private readonly IDynamoRepository<Transactions> transaction;
    private readonly IDynamoRepository<FundConfiguration> configuration;
    private readonly IDynamoRepository<ClientInformation> client;

    public StorageService(
        IDynamoRepository<Transactions> transaction,
        IDynamoRepository<FundConfiguration> configuration,
        IDynamoRepository<ClientInformation> client)
    {
        this.configuration = configuration;
        this.transaction = transaction;
        this.client = client;
    }

    public async Task<ClientInformation?> GetClientAsync(string id)
    {
        return await client.GetByIdAsync(id);
    }

    public async Task<FundConfiguration?> GetConfigurationAsync(string id)
    {
        return await configuration.GetByIdAsync(id);
    }

    public async Task<Transactions?> GetTransactionsAsync(string id)
    {
        return await transaction.GetByIdAsync(id);
    }

    public async Task SaveClient(ClientInformation model)
    {
        await this.client.SaveAsync(model);
    }
    public async Task SaveTransaction(Transactions model)
    {
        await this.transaction.SaveAsync(model);
    }

    public async Task<List<Transactions>?> GetTransactions(string model)
    {
        var transactions = await this.transaction.QueryByIndexAsync(model);
        return transactions.ToList();
    }

    public async Task<List<FundConfiguration>> GetConfiguration()
    {
        var configuration = await this.configuration.ScanAsync();
        return configuration.ToList();
    }
}
