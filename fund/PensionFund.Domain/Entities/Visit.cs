using PensionFund.Domain.Interfaces;

namespace PensionFund.Domain.Entities;
public class Visit : IEntity
{
    public int SiteId { get; set; }
    public int ClientId { get; set; }
    public DateTime VisitDate { get; set; }
    public virtual Client Client { get; set; } = null!;
    public virtual Site Site { get; set; } = null!;
}
