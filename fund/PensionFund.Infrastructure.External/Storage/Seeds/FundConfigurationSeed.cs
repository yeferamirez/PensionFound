using PensionFund.Domain.Entities;
using PensionFund.Domain.Interfaces.Repositories;

namespace PensionFund.Infrastructure.External.Storage.Seeds;
public static class FundConfigurationSeed
{
    public static async Task SeedAsync(IDynamoRepository<FundConfiguration> repository)
    {
        var existing = await repository.ScanAsync();
        if (existing.Any())
            return;

        var fondos = new List<FundConfiguration>
        {
            new("FPV_BTG_PACTUAL_RECAUDADORA", "FPV", 75000),
            new("FPV_BTG_PACTUAL_ECOPETROL", "FPV", 125000),
            new("DEUDAPRIVADA", "FIC", 50000),
            new("FDO-ACCIONES", "FIC", 250000),
            new("FPV_BTG_PACTUAL_DINAMICA", "FPV", 100000),
        };

        foreach (var fondo in fondos)
        {
            await repository.SaveAsync(fondo);
        }
    }
}

