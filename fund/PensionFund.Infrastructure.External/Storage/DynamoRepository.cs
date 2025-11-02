using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using PensionFund.Domain.Entities;
using PensionFund.Domain.Interfaces;
using PensionFund.Domain.Interfaces.Repositories;

namespace PensionFund.Infrastructure.External.Storage;

public class DynamoRepository<T> : IDynamoRepository<T>
{
    private readonly IDynamoDBContext context; 
    private readonly DynamoDBOperationConfig config;

    public DynamoRepository(IAmazonDynamoDB client)
    {
        context = new DynamoDBContext(client);

        if (typeof(T).Name == nameof(FundConfiguration))
            config = new DynamoDBOperationConfig { OverrideTableName = "Configurations" };
        else
            config = new DynamoDBOperationConfig(); // usa por defecto el nombre de la clase
    }

    public async Task<T?> GetByIdAsync(string id)
        => await context.LoadAsync<T>(id);

    public async Task SaveAsync(T entity)
        => await context.SaveAsync(entity);

    public async Task DeleteAsync(string id)
        => await context.DeleteAsync<T>(id);

    public async Task<IEnumerable<T>> ScanAsync()
    {
        var conditions = new List<ScanCondition>();
        return await context.ScanAsync<T>(conditions).GetRemainingAsync();
    }

    public async Task<IEnumerable<T>> QueryByIndexAsync(string value)
    {
        var queryConfig = new DynamoDBOperationConfig
        {
            IndexName = "ModificationDate-index"
        };

        var query = context.QueryAsync<T>(value, queryConfig);
        return await query.GetRemainingAsync();
    }
}
