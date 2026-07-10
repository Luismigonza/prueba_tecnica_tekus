using Microsoft.EntityFrameworkCore;
using ProviderServices.Application.DTOs;
using ProviderServices.Application.Interfaces;
using ProviderServices.Infrastructure.Persistence;

namespace ProviderServices.Infrastructure.Repositories;

public class SummaryQueries : ISummaryQueries
{
    private readonly AppDbContext _context;

    public SummaryQueries(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SummaryDto> GetSummaryAsync(CancellationToken ct = default)
    {
        var providersByCountry = await _context.Providers
            .GroupBy(p => p.Country)
            .Select(g => new CountByCountryDto { Country = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        var servicesByCountry = await _context.Services
            .Join(_context.Providers, s => s.ProviderId, p => p.Id, (s, p) => p.Country)
            .GroupBy(country => country)
            .Select(g => new CountByCountryDto { Country = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        return new SummaryDto
        {
            ProvidersByCountry = providersByCountry,
            ServicesByCountry = servicesByCountry
        };
    }
}
