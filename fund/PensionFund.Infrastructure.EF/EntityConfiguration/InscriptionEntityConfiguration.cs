using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PensionFund.Domain.Entities;

namespace PensionFund.Infrastructure.EF.EntityConfiguration;
public class InscriptionEntityConfiguration : IEntityTypeConfiguration<Inscription>
{
    public void Configure(EntityTypeBuilder<Inscription> builder)
    {
        builder.HasKey(i => new { ProductId = i.ProductId, ClientId = i.ClientId });

        builder.HasOne(i => i.Product)
            .WithMany(p => p.Inscriptions)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Client)
            .WithMany(c => c.Inscriptions)
            .HasForeignKey(i => i.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
