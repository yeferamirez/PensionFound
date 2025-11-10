using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PensionFund.Api.Attributes;
using PensionFund.Api.Models.PensionFund;
using PensionFund.Application.Exceptions.Errors;
using PensionFund.Application.UseCases.CancelSubscription;
using PensionFund.Application.UseCases.CreateSubscription;
using PensionFund.Application.UseCases.GetAllTransactions;
using PensionFund.Application.UseCases.GetFundConfiguration;

namespace PensionFund.Api.Controllers;

[Route("api/v1/[controller]/")]
public class PensionFundController : BaseApiController
{
    private readonly ISender sender;
    private readonly IMapper mapper;

    public PensionFundController(
        ISender sender,
        IMapper mapper)
    {
        this.sender = sender;
        this.mapper = mapper;
    }

    [HttpPost]
    //[Authorize]
    [NonEmptyModel]
    [Route("subscribe-fund")]
    public async Task<IActionResult> SubcribeFund([FromBody] CreateSubcriptionCommandModel model)
    {
        var request = this.mapper.Map<CreateSubcriptionCommand>(model);

        var result = await this.sender.Send(request);

        if (!result.IsSuccess)
        {
            if (result.HasError<NotFoundError>())
            {
                return this.NotFound(result.Errors);
            }

            return this.BadRequest(result.Errors);
        }

        return this.Created("subscribted", result.Value);
    }

    [HttpPost]
    //[Authorize]
    [NonEmptyModel]
    [Route("unsubscribe-fund")]
    public async Task<IActionResult> UnsubscribeFund([FromBody] UnsubscribeCommandModel model)
    {
        var request = this.mapper.Map<UnsubscribeCommand>(model);

        var result = await this.sender.Send(request);

        if (!result.IsSuccess)
        {
            if (result.HasError<NotFoundError>())
            {
                return this.NotFound(result.Errors);
            }

            return this.BadRequest(result.Errors);
        }

        return this.Created("unsubscribted", result.Value);
    }


    [HttpGet]
    //[Authorize]
    [Route("list-transactions")]
    public async Task<IActionResult> GetTransactions(string date)
    {
        var query = new GetAllTransactionsQuery
        {
            Date = date
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

    [HttpGet]
    //[Authorize]
    [Route("get-fundconfiguration")]
    public async Task<IActionResult> GetFundconfiguration()
    {
        var query = new GetFundConfigurationQuery();

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
