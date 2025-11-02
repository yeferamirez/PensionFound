namespace PensionFund.Application.Exceptions.Errors;
public class NotFoundError : PensionFundError
{


    public NotFoundError(string message = "Not found") : base("NotFound", message)
    {
    }
}
