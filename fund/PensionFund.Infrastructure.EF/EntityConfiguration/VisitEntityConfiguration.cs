using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PensionFund.Domain.Entities;

namespace PensionFund.Infrastructure.EF.EntityConfiguration;
public class VisitEntityConfiguration : IEntityTypeConfiguration<Visit>
{
    public void Configure(EntityTypeBuilder<Visit> builder)
    {
        builder.HasKey(v => new { SiteId = v.SiteId, ClientId = v.ClientId });

        builder.Property(v => v.VisitDate)
            .IsRequired();

        builder.HasOne(i => i.Client)
            .WithMany(p => p.Visits)
            .HasForeignKey(i => i.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Site)
            .WithMany(c => c.Visits)
            .HasForeignKey(i => i.SiteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
