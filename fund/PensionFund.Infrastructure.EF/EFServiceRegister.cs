using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PensionFund.Application.Repositories;
using PensionFund.Domain.Entities;
using PensionFund.Domain.Interfaces;
using PensionFund.Domain.Interfaces.Repositories;
using PensionFund.Infrastructure.EF.Repositories;

namespace PensionFund.Infrastructure.EF;
public static class EFServiceRegister
{
    public static void RegisterDatabase(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopment)
    {
        var sqlServiceConnectionString = configuration["ConnectionStrings:DefaultConnection"];
        Guard.Against.Null(sqlServiceConnectionString,
            message: "Connection string 'ConnectionStrings:DefaultConnection' not found.");

        services.AddDbContext<PensionFundDbContext>(options =>
            options.UseSqlServer(
                    sqlServiceConnectionString, x =>
                    {
                        x.MigrationsHistoryTable("__EFMigrationsHistory", PensionFundDbContext.ContractSchema);
                    })
                .EnableDetailedErrors()
                .UseSeeding((dbContext, _) => SeedData(dbContext, isDevelopment)));

        services.AddScoped<IDbContext, PensionFundDbContext>();

        services.AddScoped<IUnitOfWork>((sp) => sp.GetRequiredService<PensionFundDbContext>());

        services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));

        services.AddScoped<IClientRepository, ClientRepository>();
    }

    private static void SeedData(DbContext context, bool isDevelopment)
    {
        if (isDevelopment)
        {
            var db = (PensionFundDbContext)context;

            AddClients(db);
            AddProducts(db);
            AddSites(db);
            AddInscriptions(db);
            AddAvailabilities(db);
            AddVisits(db);
        }
    }
    private static void AddClients(PensionFundDbContext db)
    {
        // Clientes
        if (!db.Clients.Any())
        {
            var clients = new List<Client>
            {
                new Client { Name = "Juan", LastName = "Perez", City = "Bogota" },
                new Client { Name = "Maria", LastName = "Lopez", City = "Medellin" },
                new Client { Name = "Carlos", LastName = "Ramirez", City = "Cali" }
            };
            db.Clients.AddRange(clients);
            db.SaveChanges();
        }
    }
    private static void AddProducts(PensionFundDbContext db)
    {
        // Productos
        if (!db.Products.Any())
        {
            var products = new List<Product>
            {
                new Product { Name = "Colanta", ProductType = "Lacteos" },
                new Product { Name = "Platano", ProductType = "Verdura" },
                new Product { Name = "Pera", ProductType = "Fruta" }
            };
            db.Products.AddRange(products);
            db.SaveChanges();
        }
    }
    private static void AddSites(PensionFundDbContext db)
    {
        // Sucursales
        if (!db.Sites.Any())
        {
            var sites = new List<Site>
            {
                new Site { Name = "Sucursal Norte", City = "Bogota" },
                new Site { Name = "Sucursal Sur", City = "Bogota" },
                new Site { Name = "Sucursal Medellin", City = "Medellin" }
            };
            db.Sites.AddRange(sites);
            db.SaveChanges();
        }
    }
    private static void AddInscriptions(PensionFundDbContext db)
    {
        // Inscripciones
        if (!db.Inscriptions.Any())
        {
            var inscription = new List<Inscription>
            {
                new Inscription { ClientId = 1, ProductId = 1 },
                new Inscription { ClientId = 1, ProductId = 2 },
                new Inscription { ClientId = 2, ProductId = 2 },
                new Inscription { ClientId = 3, ProductId = 3 }
            };
            db.Inscriptions.AddRange(inscription);
            db.SaveChanges();
        }
    }
    private static void AddAvailabilities(PensionFundDbContext db)
    {
        // Disponibilidad
        if (!db.Availabilities.Any())
        {
            var availability = new List<Availability>
            {
                new Availability { SiteId = 1, ProductId = 1 }, // Gimnasio en Norte
                new Availability { SiteId = 1, ProductId = 2 }, // Natación en Norte
                new Availability { SiteId = 2, ProductId = 3 }, // Yoga en Sur
                new Availability { SiteId = 3, ProductId = 2 }  // Natación en Medellín
            };
            db.Availabilities.AddRange(availability);
            db.SaveChanges();
        }
    }
    private static void AddVisits(PensionFundDbContext db)
    {
        // Visitas
        if (!db.Visits.Any())
        {
            var visit = new List<Visit>
            {
                new Visit { ClientId = 1, SiteId = 1,VisitDate = DateTime.UtcNow.AddDays(-1) },
                new Visit { ClientId = 2, SiteId = 3, VisitDate = DateTime.UtcNow.AddDays(-3) },
                new Visit { ClientId = 3, SiteId = 2,VisitDate = DateTime.UtcNow.AddDays(-2) }
            };
            db.Visits.AddRange(visit);
            db.SaveChanges();
        }
    }
}
