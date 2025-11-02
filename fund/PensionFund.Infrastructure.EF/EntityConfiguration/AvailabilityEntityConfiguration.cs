using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PensionFund.Domain.Entities;

namespace PensionFund.Infrastructure.EF.EntityConfiguration;
public class AvailabilityEntityConfiguration : IEntityTypeConfiguration<Availability>
{
    public void Configure(EntityTypeBuilder<Availability> builder)
    {
        builder.HasKey(a => new { ProductId = a.ProductId, SiteId = a.SiteId });

        builder.HasOne(i => i.Product)
            .WithMany(p => p.Availabilities)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Site)
            .WithMany(c => c.Availabilities)
            .HasForeignKey(i => i.SiteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
