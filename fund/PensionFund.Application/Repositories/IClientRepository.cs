using PensionFund.Domain.Entities;
using PensionFund.Domain.Interfaces.Repositories;

namespace PensionFund.Application.Repositories;
public interface IClientRepository : IRepository<Client>
{
    Task<Client[]> GetClientsByCityAsync(string city);
}
