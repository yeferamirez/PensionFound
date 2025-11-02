using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PensionFund.Application.Exceptions.Errors;
using PensionFund.Application.UseCases.GetClientByCity;

namespace PensionFund.Api.Controllers;

[Route("api/v1/[controller]/")]
public class ClientController : BaseApiController
{
    private readonly ISender sender;
    private readonly IMapper mapper;

    public ClientController(
        ISender sender,
        IMapper mapper)
    {
        this.sender = sender;
        this.mapper = mapper;
    }

    [HttpGet]
    [Authorize]
    [Route("get-clients")]
    public async Task<IActionResult> GetClients(string city)
    {
        var query = new GetClientByIdQuery
        {
            City = city
        };

        var result = await this.sender.Send(query);

        if (!result.IsSuccess)
        {
            if (result.HasError<NotFoundError>())
            {
                return this.NotFound();
            }

            return this.BadRequest(result.Errors);
        }

        return this.Ok(result.Value);
    }
}
