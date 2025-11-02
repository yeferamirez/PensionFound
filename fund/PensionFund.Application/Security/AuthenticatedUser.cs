namespace PensionFund.Application.Security;
public record class AuthenticatedUser(int Id, string Email, string Name);
