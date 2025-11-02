using PensionFund.Domain.Interfaces;

namespace PensionFund.Domain.Entities;
public class Client : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string City { get; set; } = null!;

    public virtual ICollection<Inscription> Inscriptions { get; set; } = [];
    public virtual ICollection<Visit> Visits { get; set; } = [];
}
