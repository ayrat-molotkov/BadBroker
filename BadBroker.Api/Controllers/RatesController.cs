using BadBroker.Api.Models;
using BadBroker.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BadBroker.Api.Controllers;

[ApiController]
[Route("rates/best")]
[Produces("application/json")]
public class RatesController : ControllerBase
{
    public RatesController(IRatesService ratesService)
    {
        _ratesService = ratesService;
    }


    /// <summary>
    /// Returns the best rates plan.
    /// </summary>
    /// <param name="moneyUsd">Money</param>
    /// <param name="startDate">Start Date</param>
    /// <param name="endDate">End date</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns the best rates plan</returns>
    [HttpGet]
    [ProducesResponseType(typeof(BestRatesPlan), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get([FromQuery] int moneyUsd, [FromQuery(Name = "startDate")] DateTime startDate,
        [FromQuery(Name = "endDate")] DateTime endDate, CancellationToken cancellationToken = default)
    {
        return Ok(await _ratesService.GetBestRatesPlan(startDate, endDate, moneyUsd, cancellationToken));
    }


    private readonly IRatesService _ratesService;
}
