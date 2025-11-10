using Microsoft.EntityFrameworkCore;
using PensionFund.Domain.Entities;
using PensionFund.Domain.Interfaces.Repositories;

namespace PensionFund.Infrastructure.EF.Repositories;
public class ClientRepository(PensionFundDbContext context) : EFRepository<Client>(context), IClientRepository
{
    public async Task<Client[]> GetClientsByCityAsync(string city)
    {
        var query = context.Clients
        .Include(c => c.Inscriptions)
            .ThenInclude(i => i.Product)
                .ThenInclude(p => p.Availabilities)
                    .ThenInclude(d => d.Site)
        .Include(c => c.Visits)
            .ThenInclude(v => v.Site)
        .AsNoTracking();

        query = query.Where(c =>
            c.Visits.Any(v => v.Site.City == city) &&
            c.Inscriptions.Any(i =>
                i.Product.Availabilities.All(d =>
                    c.Visits.Any(v => v.SiteId == d.SiteId)
                )
            )
        );

        return await query.ToArrayAsync();
    }
}
