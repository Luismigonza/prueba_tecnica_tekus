using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProviderServices.Application.DTOs;
using ProviderServices.Application.Interfaces;

namespace ProviderServices.Api.Controllers;

[ApiController]
[Route("api/providers")]
[Authorize]
public class ProvidersController : ControllerBase
{
    private readonly IProvidersAppService _providersAppService;

    public ProvidersController(IProvidersAppService providersAppService)
    {
        _providersAppService = providersAppService;
    }

    [HttpPost]
    public async Task<ActionResult<ProviderDto>> Create(CreateProviderRequest request, CancellationToken ct)
    {
        var provider = await _providersAppService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = provider.Id }, provider);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ProviderDto>>> GetAll([FromQuery] ProviderFilter filter, CancellationToken ct)
    {
        var result = await _providersAppService.GetAllAsync(filter, ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProviderDto>> GetById(Guid id, CancellationToken ct)
    {
        var provider = await _providersAppService.GetByIdAsync(id, ct);
        return Ok(provider);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProviderDto>> Update(Guid id, UpdateProviderRequest request, CancellationToken ct)
    {
        var provider = await _providersAppService.UpdateAsync(id, request, ct);
        return Ok(provider);
    }
}
