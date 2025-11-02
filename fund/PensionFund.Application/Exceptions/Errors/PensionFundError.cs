using FluentResults;

namespace PensionFund.Application.Exceptions.Errors;

public class PensionFundError : Error
{
    public PensionFundError(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }

    public PensionFundError(string errorCode) : base(errorCode)
    {
        ErrorCode = errorCode;
    }

    public string ErrorCode { get; }
}