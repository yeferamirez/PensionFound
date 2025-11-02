namespace PensionFund.Api.Models.PensionFund;

public class UnsubscribeCommandModel
{
    public string ClientName { get; set; } = null!;
    public string ClientLastName { get; set; } = null!;
    public string City { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string ProductType { get; set; } = null!;
}
