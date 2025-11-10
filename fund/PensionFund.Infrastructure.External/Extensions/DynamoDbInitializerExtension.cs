using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using PensionFund.Domain.Entities;
using PensionFund.Infrastructure.External.Storage;
using PensionFund.Infrastructure.External.Storage.Seeds;

namespace PensionFund.Infrastructure.External.Extensions;
public class DynamoDbInitializer
{
    private readonly IAmazonDynamoDB _client;

    public DynamoDbInitializer(IAmazonDynamoDB client)
    {
        _client = client;
    }

    public async Task InitializeAsync()
    {
        await EnsureTableExistsAsync("ClientInformation", "ClientName");
        await EnsureTableExistsAsync("Transactions", "ClientName");
        await EnsureTableExistsAsync("Configurations", "FundName");
    }

    private async Task EnsureTableExistsAsync(string tableName, string partitionKey)
    {
        var existingTables = await _client.ListTablesAsync();

        if (!existingTables.TableNames.Contains(tableName))
        {
            var createRequest = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = partitionKey,
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = partitionKey,
                        KeyType = "HASH"
                    }
                }
            };

            if (tableName == "Transactions")
            {
                createRequest.AttributeDefinitions.Add(
                    new AttributeDefinition
                    {
                        AttributeName = "ModificationDate",
                        AttributeType = "S"
                    });

                createRequest.GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
                {
                    new GlobalSecondaryIndex
                    {
                        IndexName = "ModificationDate-index",
                        KeySchema = new List<KeySchemaElement>
                        {
                            new KeySchemaElement
                            {
                                AttributeName = "ModificationDate",
                                KeyType = "HASH"
                            }
                        },
                        Projection = new Projection
                        {
                            ProjectionType = ProjectionType.ALL
                        }
                    }
                };
            }


            await _client.CreateTableAsync(createRequest);
        }

        var describe = await _client.DescribeTableAsync(tableName);
        if (describe.Table.TableStatus == TableStatus.ACTIVE)
        {
            if (tableName == "Configurations")
            {
                var repo = new DynamoRepository<FundConfiguration>(_client);
                await FundConfigurationSeed.SeedAsync(repo);
            }
        }
    }
}
