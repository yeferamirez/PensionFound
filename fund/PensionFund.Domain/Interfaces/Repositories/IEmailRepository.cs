namespace PensionFund.Domain.Interfaces.Repositories;
public interface IEmailRepository
{
    Task SendNotification(string email);
}
