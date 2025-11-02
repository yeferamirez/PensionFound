using Microsoft.EntityFrameworkCore;
using PensionFund.Domain.Entities;
using PensionFund.Domain.Interfaces;

namespace PensionFund.Infrastructure.EF;
public class PensionFundDbContext : DbContext, IDbContext, IUnitOfWork
{
    public const string ContractSchema = "PensionFund";

    public PensionFundDbContext(DbContextOptions<PensionFundDbContext> options) : base(options)
    {

    }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Site> Sites { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Inscription> Inscriptions { get; set; }
    public DbSet<Availability> Availabilities { get; set; }
    public DbSet<Visit> Visits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(ContractSchema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PensionFundDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
