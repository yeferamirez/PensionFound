using FluentValidation;

namespace PensionFund.Application.UseCases.CreateSubscription;
public class CreateSubcriptionCommandValidator : AbstractValidator<CreateSubcriptionCommand>
{
    public CreateSubcriptionCommandValidator()
    {

    }
}
