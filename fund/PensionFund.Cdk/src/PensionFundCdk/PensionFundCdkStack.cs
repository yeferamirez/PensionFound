using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ElasticLoadBalancingV2;
using Amazon.CDK.AWS.ElasticLoadBalancingV2.Targets;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Constructs;
using System;
using System.IO;
using Attribute = Amazon.CDK.AWS.DynamoDB.Attribute;

namespace PensionFundCdk;

public class PensionFundCdkStack : Stack
{
    internal PensionFundCdkStack(Construct scope, string id, PensionFundStackProps props) : base(scope, id, props)
    {
        var vpc = Vpc.FromLookup(this, "Vpc", new VpcLookupOptions { VpcId = props.VpcId });
        var webAccessSg = SecurityGroup.FromSecurityGroupId(this, "WebAccessSg", props.WebAccessSg,
                new SecurityGroupImportOptions { Mutable = false });

        var policy = CreatePolicy(props);
        var function = GenerateLambdaFunction(policy, vpc, webAccessSg);
        CreateTargetGroup(function, vpc);

        CreateConfigurationsTableDynamoDb();
        CreateClientsTableDynamoDb();
        CreateTransactionsTableDynamoDb();
    }

    private Function GenerateLambdaFunction(Policy policy, IVpc vpc, ISecurityGroup securityGroup)
    {
        var lambdaPath = Path.GetFullPath(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                @"..\..\..\..\..\..\PensionFund.Api\bin\Release\net9.0\linux-x64\publish"
            ));

        var function = new Function(this, "EtlDailyConsumption", new FunctionProps
        {
            Code = Code.FromAsset(lambdaPath),
            Runtime = Runtime.DOTNET_9,
            Handler = "PensionFund.Api",
            Timeout = Duration.Seconds(60),
            MemorySize = 512,
            Architecture = Architecture.X86_64,
            Vpc = vpc,
            SecurityGroups = new[] { securityGroup }
        });
        var ssmPolicy = ManagedPolicy.FromAwsManagedPolicyName("AmazonSSMReadOnlyAccess");
        function.Role.AddManagedPolicy(ssmPolicy);
        function.Role.AttachInlinePolicy(policy);
        return function;
    }

    private void CreateTargetGroup(Function api, IVpc vpc)
    {
        var targetGroup = new ApplicationTargetGroup(this, "alb-lambda-target-group", new ApplicationTargetGroupProps
        {
            Vpc = vpc,
            TargetType = TargetType.LAMBDA,
            Targets = new IApplicationLoadBalancerTarget[] { new LambdaTarget(api) }
        });

    }

    private string CreateClientsTableDynamoDb()
    {
        var clientTable = new Table(this, "TableClientInformationDynamoBack", new TableProps
        {
            PartitionKey = new Attribute { Name = "ClientName", Type = AttributeType.STRING },
            BillingMode = BillingMode.PAY_PER_REQUEST,
            TableName = "ClientInformation"
        });
        return clientTable.TableName;
    }

    private string CreateTransactionsTableDynamoDb()
    {
        var transactionsTable = new Table(this, "TableTransactionsDynamoBack", new TableProps
        {
            PartitionKey = new Attribute { Name = "ClientName", Type = AttributeType.STRING },
            BillingMode = BillingMode.PAY_PER_REQUEST,
            TableName = "Transactions"
        });

        transactionsTable.AddGlobalSecondaryIndex(new GlobalSecondaryIndexProps
        {
            IndexName = "ModificationDate-index",
            PartitionKey = new Attribute { Name = "ModificationDate", Type = AttributeType.STRING },
            ProjectionType = ProjectionType.ALL
        });

        return transactionsTable.TableName;
    }

    private string CreateConfigurationsTableDynamoDb()
    {
        var configurationTable = new Table(this, "TableConfigurationsDynamoBack", new TableProps
        {
            PartitionKey = new Attribute { Name = "FundName", Type = AttributeType.STRING },
            BillingMode = BillingMode.PAY_PER_REQUEST,
            TableName = "Configurations"
        });
        return configurationTable.TableName;
    }

    private Policy CreatePolicy(PensionFundStackProps props)
    {
        Policy policy = new Policy(this, "GeneralDeliveryPolicy",
        new PolicyProps
        {
            Statements = new[]
            {
                    new PolicyStatement(new PolicyStatementProps
                    {
                        Actions = new[] {
                            "dynamodb:DescribeTable",
                            "dynamodb:Query",
                            "dynamodb:DeleteItem",
                            "dynamodb:UpdateItem",
                            "dynamodb:PutItem"
                        },
                        Effect = Effect.ALLOW,
                        Resources = new[] { "*" }
                    })
            }
        });
        return policy;
    }
}
