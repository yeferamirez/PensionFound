using Amazon.CDK;
using Amazon.CDK.AWS.Logs;
using Newtonsoft.Json;
using System;

namespace PensionFundCdk;

sealed class Program
{
    public static void Main(string[] args)
    {
        var app = new App();
        var labsConfig = JsonConvert.DeserializeObject<PensionFundStackConfiguration>(JsonConvert.SerializeObject(app.Node.TryGetContext("LabsConfig")));
        var labsStack = MakeStack(app, "PensionFundApi", PensionFundApiLabs(labsConfig));
        Console.WriteLine($"Finished to build stack on Account {labsStack.Account}");
        app.Synth();
    }

    private static PensionFundCdkStack MakeStack(App app, string id, PensionFundStackProps props)
    {
        return new PensionFundCdkStack(app, id, props);
    }

    private static PensionFundStackProps PensionFundApiLabs(PensionFundStackConfiguration stackConfiguration)
    {
        return new PensionFundStackProps
        {
            Env = new Amazon.CDK.Environment
            {
                Account = stackConfiguration.Account,
                Region = stackConfiguration.Region
            },
            VpcId = stackConfiguration.VpcId,
            WebAccessSg = stackConfiguration.WebAccessSg,
            PrincipalListenerArn = stackConfiguration.PrincipalListenerArn,
            SecondListenerArn = stackConfiguration.SecondListenerArn,
            Priority = 1,
            ProjectName = "PensionFund",
            LogRetention = RetentionDays.ONE_WEEK,
            Description = "Pension Fund API Stack for Labs Environment",
            StackName = "PensionFundApi-Labs",
        };
    }
}
