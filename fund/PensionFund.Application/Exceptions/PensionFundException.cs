namespace PensionFund.Application.Exceptions;

public class PensionFundException : Exception
{
    public PensionFundException(string error) : base(error)
    {
    }

    public PensionFundException(PensionFundExceptionCodes code) : base(EnumHelpers.GetDescription(code))
    {
        this.Code = code;
    }

    public PensionFundException(PensionFundExceptionCodes code, string error) : base(error)
    {
        this.Code = code;
    }

    public PensionFundException(string target, PensionFundExceptionCodes code) : base(EnumHelpers.GetDescription(code))
    {
        this.Target = target;
        this.Code = code;
    }

    public PensionFundException(PensionFundExceptionCodes code, string error, string target) : base(error)
    {
        this.Target = target;
        this.Code = code;
    }

    public PensionFundExceptionCodes Code { get; set; }

    public string? Target { get; set; }
}
