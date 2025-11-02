using PensionFund.Domain.Interfaces;

namespace PensionFund.Domain.Entities;
public class Site : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string City { get; set; } = null!;
    public virtual ICollection<Availability> Availabilities { get; set; } = [];
    public virtual ICollection<Visit> Visits { get; set; } = [];
}
