using Amazon.DynamoDBv2.DataModel;

namespace PensionFund.Domain.Entities;

[DynamoDBTable("ClientInformation")]
public class ClientInformation
{
    [DynamoDBHashKey]
    public string ClientName { get; set; } = default!;
    [DynamoDBProperty]
    public string City { get; set; } = default!;
    [DynamoDBProperty]
    public int InitialValue { get; set; }
}
