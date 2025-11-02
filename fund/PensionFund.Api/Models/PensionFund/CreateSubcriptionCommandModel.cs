namespace PensionFund.Api.Models.PensionFund;

public class CreateSubcriptionCommandModel
{
    public string ClientName { get; set; } = null!;
    public string ClientLastName { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string NotificationType { get; set; } = null!;
    public string ProductType { get; set; } = null!;
    public string ProductCity { get; set; } = null!;
    public int Value { get; set; }
}
