using PensionFund.Domain.Interfaces;

namespace PensionFund.Domain.Entities;
public class Availability : IEntity
{
    public int SiteId { get; set; }
    public int ProductId { get; set; }
    public Site Site { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
