using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using PensionFund.Application.Exceptions.Errors;
using PensionFund.Application.Extensions;
using PensionFund.Application.Models;
using PensionFund.Domain.Interfaces.Repositories;

namespace PensionFund.Application.UseCases.GetClientByCity;
public class GetClientByIdQuery : IRequest<Result<GetByCityClientDto>>
{
    public string City { get; set; } = default!;
}

public class GetClientByIdQueryHandler(
    IClientRepository clientRepository,
    ILogger<GetClientByIdQueryHandler> logger) : IRequestHandler<GetClientByIdQuery, Result<GetByCityClientDto>>
{
    public async Task<Result<GetByCityClientDto>> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var clients = await clientRepository.GetClientsByCityAsync(request.City);

        if (clients == null)
        {
            logger.LogInformation("Clients {City} not found", request.City);
            return Result.Fail(new NotFoundError());
        }

        var clientNames = clients.ToGetByCityDto();

        return clientNames;
    }
}
