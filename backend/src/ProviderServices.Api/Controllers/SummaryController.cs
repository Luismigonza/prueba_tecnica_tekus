using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProviderServices.Application.DTOs;
using ProviderServices.Application.Interfaces;

namespace ProviderServices.Api.Controllers;

[ApiController]
[Route("api/summary")]
[Authorize]
public class SummaryController : ControllerBase
{
    private readonly ISummaryQueries _summaryQueries;

    public SummaryController(ISummaryQueries summaryQueries)
    {
        _summaryQueries = summaryQueries;
    }

    [HttpGet]
    public async Task<ActionResult<SummaryDto>> Get(CancellationToken ct)
    {
        var summary = await _summaryQueries.GetSummaryAsync(ct);
        return Ok(summary);
    }
}
