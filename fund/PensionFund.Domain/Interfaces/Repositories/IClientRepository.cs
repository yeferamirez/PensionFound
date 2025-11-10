using PensionFund.Domain.Entities;

namespace PensionFund.Domain.Interfaces.Repositories;
public interface IClientRepository : IRepository<Client>
{
    Task<Client[]> GetClientsByCityAsync(string city);
}
