using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProviderServices.Application.DTOs;
using ProviderServices.Application.Interfaces;

namespace ProviderServices.Api.Controllers;

[ApiController]
[Route("api/providers/{providerId:guid}/services")]
[Authorize]
public class ServicesController : ControllerBase
{
    private readonly IServicesAppService _servicesAppService;

    public ServicesController(IServicesAppService servicesAppService)
    {
        _servicesAppService = servicesAppService;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceDto>> Create(Guid providerId, CreateServiceRequest request, CancellationToken ct)
    {
        var service = await _servicesAppService.CreateAsync(providerId, request, ct);
        return StatusCode(StatusCodes.Status201Created, service);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ServiceDto>>> GetByProvider(
        Guid providerId, [FromQuery] ServiceFilter filter, CancellationToken ct)
    {
        var result = await _servicesAppService.GetByProviderIdAsync(providerId, filter, ct);
        return Ok(result);
    }
}
