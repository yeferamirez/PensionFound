using Amazon.DynamoDBv2.DataModel;

namespace PensionFund.Domain.Entities;

[DynamoDBTable("Transactions")]
public class Transactions
{
    [DynamoDBProperty]
    public string Id { get; set; } = default!;
    [DynamoDBHashKey]
    public string ClientName { get; set; } = default!;
    [DynamoDBProperty]
    public string ProductName { get; set; } = default!;
    [DynamoDBProperty]
    public string ProductType { get; set; } = default!;
    [DynamoDBProperty]
    public string ProductCity { get; set; } = default!;
    [DynamoDBProperty]
    public int Value { get; set; }
    [DynamoDBProperty]
    public string State { get; set; } = default!;
    [DynamoDBProperty]
    public string ModificationDate { get; set; } = default!;
}
