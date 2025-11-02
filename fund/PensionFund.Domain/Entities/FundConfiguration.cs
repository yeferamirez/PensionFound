using Amazon.DynamoDBv2.DataModel;

namespace PensionFund.Domain.Entities;

[DynamoDBTable("Configurations")]
public class FundConfiguration
{
    [DynamoDBHashKey]
    public string FundName { get; set; } = default!;
    [DynamoDBProperty]
    public string Category { get; set; } = default!;
    [DynamoDBProperty]
    public double MinimumCost { get; set; }
    public FundConfiguration() { }

    public FundConfiguration(string fundName, string category, double minimumCost)
    {
        FundName = fundName;
        Category = category;
        MinimumCost = minimumCost;
    }
}
