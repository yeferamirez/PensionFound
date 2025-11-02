using MediatR;
using PensionFund.Domain.Interfaces.Repositories;

namespace PensionFund.Application.UseCases.NotifySubscriptionCreated;
public class NotifySubcribtedCommand : IRequest<Unit>
{
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Type { get; set; } = null!;

}

public class NotifySubcribtedCommandHandler : IRequestHandler<NotifySubcribtedCommand, Unit>
{
    private readonly IEmailRepository emailRepository;
    private readonly ISmsRepository smsRepository;

    public NotifySubcribtedCommandHandler(
        IEmailRepository emailRepository,
        ISmsRepository smsRepository)
    {
        this.emailRepository = emailRepository;
        this.smsRepository = smsRepository;
    }

    public async Task<Unit> Handle(NotifySubcribtedCommand request, CancellationToken cancellationToken)
    {
        if (request.Type.ToUpper().Equals("SMS"))
        {
            await smsRepository.SendNotification(request.PhoneNumber);
        }
        else
        {
            await emailRepository.SendNotification(request.Email);
        }

        return Unit.Value;
    }
}
