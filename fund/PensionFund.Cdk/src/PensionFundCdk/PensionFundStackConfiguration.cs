using System.Collections.Generic;

namespace PensionFundCdk;
internal class PensionFundStackConfiguration
{
    public List<string> BucketsArnToAccess { get; set; }
    public string BucketArn { get; set; }
    public string BucketArnLocalTest { get; set; }
    public string PrincipalListenerArn { get; set; }
    public string SecondListenerArn { get; set; }
    public Dictionary<string, string> TaskEnvironmentVariables { get; set; }
    public string VpcId { get; set; }
    public string WebAccessSg { get; set; }
    public string Account { get; set; }
    public string Region { get; set; }
}
