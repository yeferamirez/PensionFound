using PensionFund.Application.Models;
using PensionFund.Domain.Entities;

namespace PensionFund.Application.Extensions;
public static class ClientDtoExtensions
{
    public static GetByCityClientDto ToGetByCityDto(
        this Client[] clients)
    {
        return new GetByCityClientDto
        {
            Names = clients
            .Select(x => $"{x.Name} {x.LastName}")
            .Distinct()
            .ToList(),
        };
    }
}
