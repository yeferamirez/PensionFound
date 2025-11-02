using AutoMapper;
using PensionFund.Api.Models.PensionFund;
using PensionFund.Application.UseCases.CancelSubscription;
using PensionFund.Application.UseCases.CreateSubscription;

namespace PensionFund.Api.Mappers;

public class PensionFundsProfile : Profile
{
    public PensionFundsProfile()
    {
        CreateMap<CreateSubcriptionCommandModel, CreateSubcriptionCommand>();
        CreateMap<UnsubscribeCommandModel, UnsubscribeCommand>();
    }
}
