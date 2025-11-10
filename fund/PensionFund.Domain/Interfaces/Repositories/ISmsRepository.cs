namespace PensionFund.Domain.Interfaces.Repositories;
public interface ISmsRepository
{
    Task SendNotification(string phoneNumber);
}
