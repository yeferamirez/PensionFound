using PensionFund.Domain.Interfaces;

namespace PensionFund.Domain.Entities;
public class Inscription : IEntity
{
    public int ProductId { get; set; }
    public int ClientId { get; set; }

    public virtual Client Client { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;

}
