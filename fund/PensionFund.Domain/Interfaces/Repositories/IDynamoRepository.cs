namespace PensionFund.Domain.Interfaces.Repositories;
public interface IDynamoRepository<T>
{
    Task<T?> GetByIdAsync(string id);
    Task SaveAsync(T entity);
    Task DeleteAsync(string id);
    Task<IEnumerable<T>> ScanAsync();
    Task<IEnumerable<T>> QueryByIndexAsync(string value);
}
