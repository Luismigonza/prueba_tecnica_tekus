using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProviderServices.Application.DTOs;
using ProviderServices.Application.Interfaces;
using ProviderServices.Domain.Entities;
using ProviderServices.Infrastructure.Persistence;

namespace ProviderServices.Infrastructure.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly AppDbContext _context;

    public ServiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Service?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Services.FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<(IReadOnlyList<Service> Items, int Total)> GetByProviderIdAsync(
        Guid providerId, ServiceFilter filter, CancellationToken ct = default)
    {
        var query = _context.Services.Where(s => s.ProviderId == providerId);

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(s => s.Name.Contains(filter.Search));

        query = ApplySorting(query, filter.SortBy, filter.SortDescending);

        var total = await query.CountAsync(ct);

        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task AddAsync(Service service, CancellationToken ct = default)
    {
        await _context.Services.AddAsync(service, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }

    private static IQueryable<Service> ApplySorting(IQueryable<Service> query, string? sortBy, bool descending)
    {
        Expression<Func<Service, object>> keySelector = sortBy?.ToLowerInvariant() switch
        {
            "hourlyrateusd" => s => s.HourlyRateUsd,
            "createdat" => s => s.CreatedAt,
            _ => s => s.Name
        };

        return descending ? query.OrderByDescending(keySelector) : query.OrderBy(keySelector);
    }
}
