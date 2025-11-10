using Amazon.CDK;
using Amazon.CDK.AWS.Logs;
using System.Collections.Generic;

namespace PensionFundCdk;
internal class PensionFundStackProps : StackProps
{
    public int AsgMaxCapacity { get; set; }
    public int AsgMinCapacity { get; set; }
    public string BucketArn { get; set; }
    public string BucketArnLocalTest { get; set; }
    public RetentionDays LogRetention { get; set; } = RetentionDays.TWO_WEEKS;
    public string PrincipalListenerArn { get; set; }
    public string SecondListenerArn { get; set; }
    public int Priority { get; set; }
    public Dictionary<string, string> TaskEnvironmentVariables { get; set; }
    public string VpcId { get; set; }
    public string WebAccessSg { get; set; }
    public string ProjectName { get; set; }
}
