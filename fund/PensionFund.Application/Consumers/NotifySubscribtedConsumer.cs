using MassTransit;
using MediatR;
using PensionFund.Application.Configuration;
using PensionFund.Application.Messaging;
using PensionFund.Application.UseCases.NotifySubscriptionCreated;

namespace PensionFund.Application.Consumers;
public class NotifySubscribtedConsumer : IConsumer<Subscripted>
{
    public static string EndpointName => $"{MessagingEndpoints.Subscripted}-{nameof(NotifySubscribtedConsumer)}";

    private readonly ISender sender;

    public NotifySubscribtedConsumer(ISender sender)
    {
        this.sender = sender;
    }

    public async Task Consume(ConsumeContext<Subscripted> context)
    {
        var notifyCommand = new NotifySubcribtedCommand
        {
            Email = context.Message.Email,
            PhoneNumber = context.Message.PhoneNumber,
            Type = context.Message.Type
        };

        await this.sender.Send(notifyCommand);
    }
}
