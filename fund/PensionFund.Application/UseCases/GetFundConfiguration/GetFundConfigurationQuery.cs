using FluentResults;
using MediatR;
using PensionFund.Application.Extensions;
using PensionFund.Application.Interfaces;
using PensionFund.Application.Models;

namespace PensionFund.Application.UseCases.GetFundConfiguration;
public class GetFundConfigurationQuery : IRequest<Result<List<ConfigurationDto>>>
{
}
public class GetFundConfigurationQueryHandler : IRequestHandler<GetFundConfigurationQuery, Result<List<ConfigurationDto>>>
{
    private readonly IStorageService storageService;

    public GetFundConfigurationQueryHandler(
        IStorageService storageService)
    {
        this.storageService = storageService;
    }

    public async Task<Result<List<ConfigurationDto>>> Handle(GetFundConfigurationQuery request, CancellationToken cancellationToken)
    {
        var config = await this.storageService.GetConfiguration();

        var condifurations = config.ToGetconfigurations();
        return condifurations;
    }
}
