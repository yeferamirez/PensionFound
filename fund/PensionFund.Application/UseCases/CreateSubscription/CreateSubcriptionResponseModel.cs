namespace PensionFund.Application.UseCases.CreateSubscription;
public class CreateSubcriptionResponseModel
{
    public string Id { get; set; } = default!;
    public string ClientName { get; set; } = default!;
    public string ProductName { get; set; } = default!;
    public string ProductType { get; set; } = default!;
    public string ProductCity { get; set; } = default!;
    public int Value { get; set; }
    public string State { get; set; } = default!;
    public string ModificationDate { get; set; } = default!;
}
