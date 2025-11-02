using PensionFund.Domain.Interfaces;

namespace PensionFund.Domain.Entities;
public class Product : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ProductType { get; set; } = null!;
    public virtual ICollection<Inscription> Inscriptions { get; set; } = [];
    public virtual ICollection<Availability> Availabilities { get; set; } = [];
}
